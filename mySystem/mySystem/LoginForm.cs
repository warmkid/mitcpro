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
            pictureBox1.Image = Image.FromFile(@"../../pic/logo.png", false);  
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            String ID = this.UserIDTextBox.Text;
            String password = this.UserPWTextBox.Text;

            if (ID == "123" && password == "123")
            {
                MainForm MyDlg = new MainForm();
                this.Hide();
                MyDlg.ShowDialog();
                Application.ExitThread();
                
            }
            else if (ID == "456" && password == "456")
            {
                MessageBox.Show("复核完成！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            else
            {
                MessageBox.Show("用户名或密码错误，请重试", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.UserIDTextBox.Text = "";
                this.UserPWTextBox.Text = "";
                UserIDTextBox.Focus();
            }

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                UserPWTextBox.Focus();
            }
        }

        private void UserPWTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoginButton_Click(sender, e);
            }          
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}