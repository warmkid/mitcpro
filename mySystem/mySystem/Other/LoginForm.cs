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
        public int userID;

        public LoginForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Parameter.InitConnUser();
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
                    Parameter.userID = CheckUser(Parameter.connUser, myID, mypassword);
                }
                else
                {
                    Parameter.userID = CheckUser(Parameter.connOleUser, myID, mypassword);
                }

            }

        }



        private int CheckUser(SqlConnection Connection,string ID,string password)
        {
            string searchsql = "select * from [users] where 用户ID='" + ID + "'";
            SqlCommand comm = new SqlCommand(searchsql, Connection);
            SqlDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                if (sdr["密码"].ToString().Trim() == password) //密码正确
                {
                    //MessageBox.Show("登录成功！", "提示");
                    userID = Convert.ToInt32(sdr["用户ID"]);
                    comm.Dispose();
                    sdr.Close();
                    sdr.Dispose();
                    this.Hide();
                    return userID;
                }
                else         //密码错误
                {
                    MessageBox.Show("您输入的密码有误，请重新输入！", "警告");
                    this.UserPWTextBox.Text = null;
                    this.UserPWTextBox.Focus();
                    sdr.Close();
                    sdr.Dispose();
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("该用户不存在，请检查后重新输入！", "警告");
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
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Connection;
            comm.CommandText = "select * from [users] where 用户ID= @ID";
            comm.Parameters.AddWithValue("@ID", ID);

            OleDbDataReader sdr = comm.ExecuteReader();//执行查询
            if (sdr.Read())  //如果该用户存在
            {
                if (sdr["密码"].ToString().Trim() == password) //密码正确
                {
                    //MessageBox.Show("登录成功！", "提示");
                    userID = Convert.ToInt32(sdr["用户ID"]);
                    comm.Dispose();
                    sdr.Close();
                    sdr.Dispose();
                    this.Hide();
                    return userID;
                }
                else         //密码错误
                {
                    MessageBox.Show("您输入的密码有误，请重新输入！", "警告");
                    this.UserPWTextBox.Text = null;
                    this.UserPWTextBox.Focus();
                    sdr.Close();
                    sdr.Dispose();
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("该用户不存在，请检查后重新输入！", "警告");
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