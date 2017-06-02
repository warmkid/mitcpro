using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace mySystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            String ID = this.UserIDTextBox.Text;
            String password = this.UserPWTextBox.Text;

            if (ID != "123" || password != "123")
            {
                MessageBox.Show("用户名或密码错误，请重试", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.UserIDTextBox.Text = "";
                this.UserPWTextBox.Text = "";
            }
            else
            {
                MainForm MyDlg = new MainForm();
                this.Hide();
                MyDlg.ShowDialog();
                Application.ExitThread();
            }

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}