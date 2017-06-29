using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class LoginForm : BaseForm
    {
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        public int userID;
        bool isSqlOk;

        public LoginForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;
            //pictureBox1.Image = Image.FromFile(@"../../pic/logonew.jpg", false);
            
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
                if (Parameter.isSqlOk)
                {
                    Parameter.userID = CheckUser(Parameter.conn, myID, mypassword);
                }
                else
                {
                    Parameter.userID = CheckUser(Parameter.connOle, myID, mypassword);
                }

            }
                       
            
            //if (UserIDTextBox.Text.Trim() == "" || UserPWTextBox.Text.Trim() == "")
            //{
            //    MessageBox.Show("提示：请输入操作员ID和密码！", "警告");
            //    UserIDTextBox.Focus();
            //}
            //else
            //{
            //    String myID = this.UserIDTextBox.Text;
            //    String mypassword = this.UserPWTextBox.Text;
            //    if (isSqlOk)
            //    {
            //        userID = CheckUser(conn, myID, mypassword);
            //    }
            //    else
            //    {
            //        userID = CheckUser(connOle, myID, mypassword);
            //    }               

            //}
        }



        private int CheckUser(SqlConnection Connection,string ID,string password)
        {
            string searchstr = "select * from user_aoxing where user_id='" + ID + "'and user_password='" + password + "'";
            SqlCommand comm = new SqlCommand(searchstr, Connection);
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

        private int CheckUser(OleDbConnection Connection, string ID, string password)
        {
            //string searchstr = "select * from [user] where user_id='" + ID + "'and user_password='" + password + "'";
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Connection;
            comm.CommandText = "select * from user_aoxing where user_id= @ID and user_password= @password";
            comm.Parameters.AddWithValue("@ID" , ID);
            comm.Parameters.AddWithValue("@password", password);

            //OleDbCommand comm = new OleDbCommand(searchstr, Connection);
            OleDbDataReader sdr = comm.ExecuteReader();//执行查询
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