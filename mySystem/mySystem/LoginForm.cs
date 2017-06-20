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
        SqlConnection conn = null;
        public int userID;

        public LoginForm(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            pictureBox1.Image = Image.FromFile(@"../../pic/logonew.jpg", false);
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UserIDTextBox.Text.Trim() == "" || UserPWTextBox.Text.Trim() == "")
            {
                MessageBox.Show("提示：请输入操作员ID和密码！", "警告");
                UserIDTextBox.Focus();
            }
            else
            {
                String myID = this.UserIDTextBox.Text;
                String mypassword = this.UserPWTextBox.Text;  
                userID = CheckUser(conn, myID, mypassword);

            }
        }

        private int CheckUser(SqlConnection Connection,string ID,string password)
        {
            string searchsql = "select * from [user] where user_id='" + ID + "'and user_password='" + password + "'";
            //string searchsql = "select * from [user] where user_id='" + ID + "'";
            //string searchsql = "select * from test where userid='" + ID + "'and password='" + password + "'"; ;  
            //string searchsql = "select * from test where userid='" + ID + "'";
            SqlCommand comm = new SqlCommand(searchsql, Connection);
            SqlDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {              
                MessageBox.Show("登录成功！", "提示");
                userID = sdr.GetInt32(3);
                comm.Dispose();
                sdr.Close();
                sdr.Dispose();
                this.Hide();
                return userID;

            }
            else
            {
                MessageBox.Show("输入登录信息不正确，请重新输入！", "警告");
                this.UserIDTextBox.Text = null;
                this.UserPWTextBox.Text = null;
                UserIDTextBox.Focus();
                sdr.Close();
                sdr.Dispose();
                return 0;
 
            }

        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.ExitThread();
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

    }
}