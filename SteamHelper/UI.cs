using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;

namespace SteamHelper
{
    public partial class UI : Form
    {
        delegate void StringArgReturningVoidDelegate(string text);
        delegate void VoidDelegate();
        private static LogonManager man;
        private ConcurrentQueue<object> senderLM;
        private ConcurrentQueue<object> recieverLM;
        private bool twofa = false;
        private bool auth = false;
        private bool timeToClose = false;
        public UI(LogonManager manager)
        {
            InitializeComponent();
            FormClosing += UI_FormClosing;
            man = manager;
        }

        private void Usr_Click(object sender, EventArgs e)
        {

        }

        private void Pswd_Click(object sender, EventArgs e)
        {

        }

        private void LogonButton_Click(object sender, EventArgs e)
        {
            man.SetUsername(UsernameBox.Text);
            man.SetPassword(PasswordBox.Text);
            if (twofa)
            {
                man.SetGuardCode(TwoFABox.Text, true);
            }
            if (auth)
            {
                man.SetGuardCode(TwoFABox.Text, false);
            }
        }

        private void UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            ManualResetEvent closeEvent = new ManualResetEvent(false);
            senderLM.Enqueue(closeEvent);
            senderLM.Enqueue("close");
            if (man.LoggedOn)
            {
                man.Logoff();
            }
            closeEvent.WaitOne();
            ThreadManager.Close();
        }

        private void UI_Load(object sender, EventArgs e)
        {

        }        

        public void UpdateLabelText(string text)
        {
            if (MessageLabel.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(UpdateLabelText);
                Invoke(d, new object[] { text });
            }
            else
            {
                MessageLabel.Text = text;
            }
        }

        public void RecieveMessage()
        {
            VoidDelegate d = new VoidDelegate(RecieveMessageInternal);
            Invoke(d);
        }

        private void RecieveMessageInternal()
        {
            object message = null;
            recieverLM.TryDequeue(out message);
            switch (message)
            {
                case string strmessage:
                    switch (strmessage)
                    {
                        case "2fa":
                            make2fa();
                            twofa = true;
                            return;
                        case "auth":
                            makeAuth();
                            auth = true;
                            return;
                        default:
                            throw new InvalidMessageException(message);
                    }
                default:
                    throw new InvalidMessageException(message);            
            }
        }

        private void make2fa()
        {
            TwoFABox.Visible = true;
            TwoFAText.Text = "2FA code";
            TwoFAText.Visible = true;
        }

        private void makeAuth()
        {
            TwoFABox.Visible = true;
            TwoFAText.Text = "Auth code";
            TwoFAText.Visible = true;
        }

        private void LogoffButton_Click(object sender, EventArgs e)
        {
            man.Logoff();
            TwoFABox.Visible = false;
            TwoFAText.Visible = false;
        }

        public void loadMessagePipelineLM(ConcurrentQueue<object> sender, ConcurrentQueue<object> reciever)
        {
            senderLM = sender;
            recieverLM = reciever;
        }
    }
}
