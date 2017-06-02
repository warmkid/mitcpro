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
    public partial class Connect2SqlForm : Form
    {
        public Connect2SqlForm()
        {
            InitializeComponent();
        }

        private void Connect2SqlButton_Click(object sender, EventArgs e)
        {
            String IP = this.IPTextBox.Text;
            String port = this.PortTextBox.Text;
            
            if(IP!="1.1.1.1" || port!="11")
            {
                MessageBox.Show("连接失败，请重试", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.IPTextBox.Text = "";
                this.PortTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("连接成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoginForm MyDlg = new LoginForm();
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