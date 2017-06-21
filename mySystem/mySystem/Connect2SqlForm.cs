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

        // 1.定义委托类型  
        public delegate void DelegateIPChange(string IP, string port);
        // 2.定义委托事件  
        public event DelegateIPChange IPChange;  

        private void Connect2SqlButton_Click(object sender, EventArgs e)
        {
            ChangeIPText(IPTextBox.Text, PortTextBox.Text);
            this.Close();
        }

        private void ChangeIPText(string text1,string text2)
        {
            // 4.调用委托事件函数  
            IPChange(text1, text2);
        }  


        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.ExitThread();
        }

        private void IPTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PortTextBox.Focus();
            }
        }

        private void PortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Connect2SqlButton_Click(sender, e);
            }
        }

        private void Connect2SqlForm_Load(object sender, EventArgs e)
        {

        }


    }
}