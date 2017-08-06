using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;

namespace mySystem
{
    public partial class LoginForm : BaseForm
    {
        public int userID;

        public LoginForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Parameter.InitConnUser();
            //Parameter.ConnUserInit();
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
                    //Parameter.userID = CheckUser(Parameter.connUser, myID, mypassword);
                }
                else
                {
                    Parameter.userID = CheckUser(Parameter.connOleUser, myID, mypassword);
                    if (Parameter.userID != 0)
                    {
                        Parameter.userName = Parameter.IDtoName(Parameter.userID);
                        Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                        Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                        InstruReceive();
                    }

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


        String Instru = null; //未接收的生产指令
        //未接收的生产指令
        private void InstruReceive()
        {
            String strConn吹膜 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/extrusionnew.mdb;Persist Security Info=False";
            OleDbConnection connOle吹膜 = new OleDbConnection(strConn吹膜);
            connOle吹膜.Open();
            InstruStateChange(connOle吹膜, "生产指令信息表");


            String strConn清洁分切 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/welding.mdb;Persist Security Info=False";
            OleDbConnection connOle清洁分切 = new OleDbConnection(strConn清洁分切);
            connOle清洁分切.Open();
            InstruStateChange(connOle清洁分切, "清洁分切工序生产指令");


            //去掉最后一个"、"，弹框提示
            if (Instru != null)
            {
                Instru = Instru.Substring(0, Instru.Length - 1);
                MessageBox.Show(Parameter.userName + "请接收生产指令：" + Instru, "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            connOle吹膜.Dispose();
            connOle清洁分切.Dispose();
        }

        //更改生产指令状态
        private void InstruStateChange(OleDbConnection connOle, String tblName)
        {
            //读取未接收的生产指令
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = connOle;
            comm.CommandText = "select * from " + tblName + " where 接收人= @接收人 and 状态=1";
            comm.Parameters.AddWithValue("@接收人", Parameter.userName);

            OleDbDataReader reader = comm.ExecuteReader();//执行查询
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Instru += reader["生产指令编号"];
                    Instru += "、";
                }

            }

            //将状态变为已接收
            OleDbCommand commnew = new OleDbCommand();
            commnew.Connection = connOle;
            commnew.CommandText = "UPDATE " + tblName + " SET 状态=2 where 接收人= @接收人 and 状态=1";
            commnew.Parameters.AddWithValue("@接收人", Parameter.userName);
            commnew.ExecuteNonQuery();

            reader.Dispose();
            comm.Dispose();
            commnew.Dispose();
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