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
        string username;

        public LoginForm(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            pictureBox1.Image = Image.FromFile(@"../../pic/logo.png", false);
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UserIDTextBox.Text.Trim() == "" || UserPWTextBox.Text.Trim() == "")
            {
                MessageBox.Show("提示：请输入操作员ID和密码！", "警告");
            }
            else
            {
                String myID = this.UserIDTextBox.Text;
                String mypassword = this.UserPWTextBox.Text;
                //char[] key = mypassword.ToCharArray();
                //int userID;
                //int.TryParse(myID, out userID);
               
                username = CheckUser(conn, myID, mypassword);
                usernameLabel.Text = username;

            }
        }

        private string CheckUser(SqlConnection Connection,string ID,string password)
        {
            //string searchsql = "select * from test where userid='" + ID + "'and password='" + password + "'"; ;  
            string searchsql = "select * from test where userid='" + ID + "'";
            SqlCommand comm = new SqlCommand(searchsql, Connection);
            SqlDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                if (sdr.GetString(1).Trim() == password) //密码正确
                {
                    MessageBox.Show("登录成功！", "提示");
                    username = sdr.GetString(2).Trim();
                    sdr.Close();
                    this.Hide();
                    return username;
                }
                else         //密码错误
                {
                    MessageBox.Show("您输入的密码有误，请重新输入！", "警告");
                    this.UserPWTextBox.Text = null;
                    sdr.Close();
                    return null;
                }
            }
            else
            {
                MessageBox.Show("该用户不存在或用户名输入错误，请检查后重新输入！", "警告");
                this.UserIDTextBox.Text = null;
                this.UserPWTextBox.Text = null;
                UserIDTextBox.Focus();
                sdr.Close();
                return null;
 
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