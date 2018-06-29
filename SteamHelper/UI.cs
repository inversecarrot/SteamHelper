using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamHelper
{
    public partial class UI : Form
    {

        private static LogonManager man;
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

        private void UI_FormClosing(object sender, EventArgs e)
        {
            ThreadManager.close();
        }

        private void UI_Load(object sender, EventArgs e)
        {

        }
    }
}
