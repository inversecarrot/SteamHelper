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
        private ConcurrentQueue<string> senderLM;
        private ConcurrentQueue<string> recieverLM;
        public UI(LogonManager manager)
        {
            InitializeComponent();
            this.FormClosing += UI_FormClosing;
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
        }

        private void UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (man.LoggedOn)
            {
                man.Logoff();
            }
            senderLM.Enqueue("close");
            Console.WriteLine("sent close");
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

        private void LogoffButton_Click(object sender, EventArgs e)
        {
            man.Logoff();
        }

        public void loadMessagePipelineLM(ConcurrentQueue<string> sender, ConcurrentQueue<string> reciever)
        {
            this.senderLM = sender;
            this.recieverLM = reciever;
        }
    }
}
