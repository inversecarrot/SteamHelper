using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using SteamKit2;
using System.Collections.Concurrent;
using System.Threading; // for sleep function

namespace SteamHelper
{
    public class LogonManager
    {
        private ConcurrentQueue<string> senderUI;
        private ConcurrentQueue<string> recieverUI;
        private static bool RUN_FLAG = true;
        bool attemptedToConnect = false;
        bool twofaReconnect = false;

        public bool LoggedOn
        {
            get
            {
                return loggedOn;
            }
            private set
            {
                loggedOn = value;
            }
        }
        private bool loggedOn;

        static SteamClient steamClient;
        static CallbackManager manager;

        static SteamUser steamUser;

        private UI ui;

        private static string user;
        private static string pass;
        private static string authCode, twoFactorAuth;

        public LogonManager(String username = null, String password = null)
        {
            loggedOn = false;
            user = username;
            pass = password;
        }

        public void Run()
        {
            while (RUN_FLAG || loggedOn)
            {
                if (user != null && pass != null)
                {
                    if (!attemptedToConnect)
                    {
                        UpdateUILabel("Connecting...");
                        ConnectToSteam();
                        attemptedToConnect = true;
                    }
                    else
                    {
                        manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(0.25));
                    }                    
                }
                if (!recieverUI.IsEmpty)
                {
                    string message = "";
                    recieverUI.TryDequeue(out message);
                    if (!message.Equals(""))
                    {
                        ProcessMessage(message);
                    }                    
                }
            }
        }

        private void ProcessMessage(string message)
        {
            Console.WriteLine("Recieved message {0}", message);
            switch (message)
            {
                case "close":
                    RUN_FLAG = false;
                    return;
                default:
                    throw new InvalidMessageException(message);
            }
        }
        private class InvalidMessageException : Exception
        {
            public InvalidMessageException(string message) : base(message)
            {
            }
        }

        // Registers callbacks and connects to steam client
        private void ConnectToSteam()
        {
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();

            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);

            steamClient.Connect();
        }

        // Runs when the machine is authorized and the sentry file needs to be stored
        private void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            UpdateUILabel("Updating sentryfile...");

            int fileSize;
            byte[] sentryHash;
            using (var fs = File.Open("sentry.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(callback.Offset, SeekOrigin.Begin);
                fs.Write(callback.Data, 0, callback.BytesToWrite);
                fileSize = (int)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);
                using (var sha = SHA1.Create())
                {
                    sentryHash = sha.ComputeHash(fs);
                }
            }

            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = fileSize,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });

            ui.UpdateLabelText("Done!");
        }

        // Runs when connected to the steam interface
        // Attempts to log on to steam
        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            UpdateUILabel("Connected to steam...");
            byte[] sentryHash = null;
            if (File.Exists("sentry.bin"))
            {
                // if we have a saved sentry file, read and sha-1 hash it
                byte[] sentryFile = File.ReadAllBytes("sentry.bin");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,
                AuthCode = authCode,
                TwoFactorCode = twoFactorAuth,
                SentryFileHash = sentryHash,
            });
        }

        // Run when disconnected from steam service and given the disconnected callback
        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {            
            if (twofaReconnect)
            {
                UpdateUILabel("Disconnected from steam, reconnecting...");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                steamClient.Connect();
            }
            else
            {
                UpdateUILabel("Logged off and disconnected");
                user = null;
                pass = null;
                loggedOn = false;
                attemptedToConnect = false;
            }
        }

        // Run when given logged on callback
        // Processes successful or unsuccessful logon
        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            UpdateUILabel("Recieved response from steam...");
            bool isSteamGuard = (callback.Result == EResult.AccountLogonDenied);
            bool is2FA = (callback.Result == EResult.AccountLoginDeniedNeedTwoFactor);
            
            if (isSteamGuard || is2FA)
            {
                twofaReconnect = true;
                UpdateUILabel("This account is SteamGuard protected!");

                if (is2FA)
                {
                    UpdateUILabel("Please enter your 2 factor auth code from your authenticator app ");
                }
                else
                {
                    UpdateUILabel("Please enter the auth code sent to the email at " + callback.EmailDomain);
                }
            }
            else if (callback.Result != EResult.OK)
            {
                UpdateUILabel("Incorrect username or password");
                user = null;
                pass = null;
                attemptedToConnect = false;
            }
            else
            {
                UpdateUILabel("Successfully logged on");
                loggedOn = true;
            }            
        }

        // Run when given logged off callback
        // Resets logonmanager to default state
        private void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            UpdateUILabel("System logged off");
            user = null;
            pass = null;
            loggedOn = false;
            attemptedToConnect = false;
        }        

        // Tells the steam interface to log off
        public void Logoff()
        {
            UpdateUILabel("Logging off");
            steamUser?.LogOff();
        }


        // Sets the username if not currently logged on, returns whether username was set
        public bool SetUsername(string username)
        {
            if (user == null)
            {
                user = username;
                return true;
            }
            return false;
        }

        // Sets the password if not currently logged on, returns whether password was set
        public bool SetPassword(string password)
        {
            if (pass == null)
            {
                pass = password;
                return true;
            }
            return false;
        }
        // Sets AuthCode if not currently set, returns whether successful
        public bool SetAuthCode(string newAuthCode)
        {
            if (authCode == null)
            {
                authCode = newAuthCode;
                return true;
            }
            return false;
        }

        public bool SetTwoFactorAuth(string TwoFactorAuthCode)
        {
            if (twoFactorAuth == null)
            {
                twoFactorAuth = TwoFactorAuthCode;
                return true;
            }
            return false;
        }

        // Loads the UI to be interacted with
        public void LoadUI(UI ui)
        {
            this.ui = ui;
        }
        public void loadMessagePipelineUI(ConcurrentQueue<string> sender, ConcurrentQueue<string> reciever)
        {
            this.senderUI = sender;
            this.recieverUI = reciever;
        }
        private void UpdateUILabel(String message)
        {
            if (RUN_FLAG)
            {
                ui.UpdateLabelText(message);
            }
        }
    }

}
