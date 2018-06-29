using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace SteamHelper
{
    public class LogonManager
    {
        public static bool RUN_FLAG = true;
        bool attemptedToConnect = false;
        public bool LoggedOn
        {
            get
            {
                return LoggedOn;
            }
            private set
            {
                LoggedOn = value;
            }
        }

        static SteamClient steamClient;
        static CallbackManager manager;

        static SteamUser steamUser;

        private static string user;
        private static string pass;
        static string authCode, twoFactorAuth;

        public LogonManager(String username = null, String password = null)
        {
            LoggedOn = false;
            user = username;
            pass = password;
        }

        public void Run()
        {
            while (RUN_FLAG)
            {
                if (user != null && pass != null)
                {
                    if (!attemptedToConnect)
                    {
                        ConnectToSteam();
                    }
                    else
                    {
                        manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
                    }
                }
            }
            logoff();
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

            steamClient.Connect();
            attemptedToConnect = true;
        }

        // Runs when connected to the steam interface
        // Attempts to log on to steam
        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,
            });
        }

        // Run when disconnected from steam service and given the disconnected callback
        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            attemptedToConnect = false;
            user = null;
            pass = null;
        }

        // Run when given logged on callback
        // Processes successful or unsuccessful logon
        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    user = null;
                    pass = null;
                    attemptedToConnect = false;
                }

                user = null;
                pass = null;
                attemptedToConnect = false;
            }

            Console.WriteLine("Successfully logged on");
            LoggedOn = true;
            steamUser.LogOff();
        }

        // Run when given logged off callback
        // Resets logonmanager to default state
        private void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            user = null;
            pass = null;
            LoggedOn = false;
            attemptedToConnect = false;
        }        

        // Tells the steam interface to log off
        private void logoff()
        {
            steamUser.LogOff();
        }


        // Sets the username if not currently logged on, returns whether username was set
        public bool SetUsername(String username)
        {
            if (user == null)
            {
                user = username;
                return true;
            }
            return false;
        }

        // Sets the password if not currently logged on, returns whether password was set
        public bool SetPassword(String password)
        {
            if (pass == null)
            {
                pass = password;
                return true;
            }
            return false;
        }
    }
}
