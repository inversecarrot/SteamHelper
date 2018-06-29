namespace SteamHelper
{
    partial class UI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MsgBtn = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.Usr = new System.Windows.Forms.Label();
            this.Pswd = new System.Windows.Forms.Label();
            this.LogonButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MsgBtn
            // 
            this.MsgBtn.AutoSize = true;
            this.MsgBtn.Location = new System.Drawing.Point(622, 286);
            this.MsgBtn.Name = "MsgBtn";
            this.MsgBtn.Size = new System.Drawing.Size(491, 32);
            this.MsgBtn.TabIndex = 0;
            this.MsgBtn.Text = "Please enter username and password";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(778, 392);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(100, 38);
            this.UsernameBox.TabIndex = 1;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(773, 467);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(100, 38);
            this.PasswordBox.TabIndex = 2;
            // 
            // Usr
            // 
            this.Usr.AutoSize = true;
            this.Usr.Location = new System.Drawing.Point(525, 392);
            this.Usr.Name = "Usr";
            this.Usr.Size = new System.Drawing.Size(153, 32);
            this.Usr.TabIndex = 3;
            this.Usr.Text = "Username:";
            this.Usr.Click += new System.EventHandler(this.Usr_Click);
            // 
            // Pswd
            // 
            this.Pswd.AutoSize = true;
            this.Pswd.Location = new System.Drawing.Point(525, 458);
            this.Pswd.Name = "Pswd";
            this.Pswd.Size = new System.Drawing.Size(147, 32);
            this.Pswd.TabIndex = 4;
            this.Pswd.Text = "Password:";
            this.Pswd.Click += new System.EventHandler(this.Pswd_Click);
            // 
            // LogonButton
            // 
            this.LogonButton.Location = new System.Drawing.Point(710, 630);
            this.LogonButton.Name = "LogonButton";
            this.LogonButton.Size = new System.Drawing.Size(249, 79);
            this.LogonButton.TabIndex = 5;
            this.LogonButton.Text = "Log on";
            this.LogonButton.UseVisualStyleBackColor = true;
            this.LogonButton.Click += new System.EventHandler(this.LogonButton_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1851, 979);
            this.Controls.Add(this.LogonButton);
            this.Controls.Add(this.Pswd);
            this.Controls.Add(this.Usr);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.MsgBtn);
            this.Name = "UI";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MsgBtn;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label Usr;
        private System.Windows.Forms.Label Pswd;
        private System.Windows.Forms.Button LogonButton;
    }
}

