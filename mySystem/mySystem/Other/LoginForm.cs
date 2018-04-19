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
using System.Threading;

namespace mySystem
{
    public partial class LoginForm : BaseForm
    {
        public int userID;
        mySystem.Other.Connecting c;

        //private void ThreadFunc()
        //{
        //    MethodInvoker mi = new MethodInvoker(this.ShowMsgForm);
        //    this.BeginInvoke(mi);
        //}

        //private void ShowMsgForm()
        //{
            
        //    c.Show();
            //CustomUIControls.TaskbarNotifier taskbarNotifier1;
            //taskbarNotifier1 = new CustomUIControls.TaskbarNotifier();
            //taskbarNotifier1.SetBackgroundBitmap(new Bitmap(Image.FromFile(@"../../pic/skin_logo.bmp")), Color.FromArgb(255, 0, 255));
            //taskbarNotifier1.SetCloseBitmap(new Bitmap(Image.FromFile(@"../../pic/close_logo.bmp")), Color.FromArgb(255, 0, 255), new Point(190, 12));
            //taskbarNotifier1.TitleRectangle = new Rectangle(65, 25, 135, 60);
            //taskbarNotifier1.ContentRectangle = new Rectangle(15, 65, 205, 150);
            //taskbarNotifier1.CloseClickable = true;
            //taskbarNotifier1.TitleClickable = false;
            //taskbarNotifier1.ContentClickable = false;
            //taskbarNotifier1.EnableSelectionRectangle = false;
            //taskbarNotifier1.KeepVisibleOnMousOver = true;
            //taskbarNotifier1.ReShowOnMouseOver = true;
            //taskbarNotifier1.Show("提示", "正在连接服务器", 500, 2000, 500);
            //System.Console.WriteLine("thread");
        //}  


        public LoginForm(MainForm mainform):base(mainform)
        {
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            this.backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            this.backgroundWorker1.RunWorkerAsync();
            c = new mySystem.Other.Connecting(this.backgroundWorker1);
            c.ShowDialog();
            c.Close();
            
            

            InitializeComponent();

            //if (c.Show() != DialogResult.OK)
            //{
            //    this.Close();
            //}
            //Parameter.InitConnUser();
            //c = new mySystem.Other.Connecting();
            
            //Parameter.ConnUserInit();
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //c.Close();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Parameter.InitConnUser();
            BackgroundWorker work = sender as BackgroundWorker;
            work.ReportProgress(99);

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

            //if (UserIDTextBox.Text.Trim() == "" || UserPWTextBox.Text.Trim() == "")
            if(false)
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
                    if (Parameter.userID != 0)
                    {
                        Parameter.userName = Parameter.IDtoName(Parameter.userID);
                        Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                        Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                        // ID和密码匹配完成，获取本机UUID，获取服务器的UUID
                        // 如果相同或者服务器的UUID为空，则update
                        // 否则弹出提示，问是否登陆
                        String localUUID = Utility.getUUID();
                        SqlDataAdapter da = new SqlDataAdapter("select * from users where 用户ID='" + myID + "'", Parameter.connUser);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataTable dt = new DataTable("user");
                        da.Fill(dt);

                        if (dt.Rows.Count < 1) return;
                        String remoteUUID = dt.Rows[0]["token"].ToString();
                        if (localUUID == remoteUUID || remoteUUID == "")
                        {
                            dt.Rows[0]["token"] = localUUID;
                            da.Update(dt);
                        }
                        else
                        {
                            if (DialogResult.OK == MessageBox.Show("该账号已在别处登陆，是否继续？", "登陆确认",MessageBoxButtons.OKCancel))
                            {
                                dt.Rows[0]["token"] = localUUID;
                                da.Update(dt);
                            }
                            else
                            {
                                this.Close();
                                Application.ExitThread();
                                //return;
                            }
                            
                        }



                        //InstruReceive();

                    }
                }
                else
                {
                    Parameter.userID = CheckUser(Parameter.connOleUser, myID, mypassword);
                    if (Parameter.userID != 0)
                    {
                        Parameter.userName = Parameter.IDtoName(Parameter.userID);
                        Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                        Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                        //InstruReceive();
                    }

                }
            }
        
            

        }



        private int CheckUser(SqlConnection Connection,string ID,string password)
        {
            string searchsql = "select * from users where 用户ID='" + ID + "'";
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
            if (!mySystem.Parameter.isSqlOk)
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


                String strConnCS制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
                OleDbConnection connOleCS制袋 = new OleDbConnection(strConnCS制袋);
                connOleCS制袋.Open();
                InstruStateChange(connOleCS制袋, "生产指令");


                String strConnPE制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/LDPE.mdb;Persist Security Info=False";
                OleDbConnection connOlePE制袋 = new OleDbConnection(strConnPE制袋);
                connOlePE制袋.Open();
                InstruStateChange(connOlePE制袋, "生产指令");

                String strConnBPV制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/BPV.mdb;Persist Security Info=False";
                OleDbConnection connOleBPV制袋 = new OleDbConnection(strConnBPV制袋);
                connOleBPV制袋.Open();
                InstruStateChange(connOleBPV制袋, "生产指令");

                String strConnPTV制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/PTV.mdb;Persist Security Info=False";
                OleDbConnection connOlePTV制袋 = new OleDbConnection(strConnPTV制袋);
                connOlePTV制袋.Open();
                InstruStateChange(connOlePTV制袋, "生产指令");


                //去掉最后一个"、"，弹框提示
                if (Instru != null)
                {
                    Instru = Instru.Substring(0, Instru.Length - 1);
                    MessageBox.Show(Parameter.userName + "请接收生产指令：" + Instru, "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                connOle吹膜.Dispose();
                connOle清洁分切.Dispose();
                connOleCS制袋.Dispose();
                connOlePE制袋.Dispose();
                connOleBPV制袋.Dispose();
                connOlePTV制袋.Dispose();
            }
            else
            {
                String strConn吹膜 = "server=" + mySystem.Parameter.IP_port + ";database=extrusionnew;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection conn吹膜 = new SqlConnection(strConn吹膜);
                conn吹膜.Open();
                InstruStateChange(conn吹膜, "生产指令信息表");


                String strConn清洁分切 = "server=" + mySystem.Parameter.IP_port + ";database=welding;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection conn清洁分切 = new SqlConnection(strConn清洁分切);
                conn清洁分切.Open();
                InstruStateChange(conn清洁分切, "清洁分切工序生产指令");


                String strConnCS制袋 = "server=" + mySystem.Parameter.IP_port + ";database=csbag;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connCS制袋 = new SqlConnection(strConnCS制袋);
                connCS制袋.Open();
                InstruStateChange(connCS制袋, "生产指令");


                String strConnPE制袋 = "server=" + mySystem.Parameter.IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connPE制袋 = new SqlConnection(strConnPE制袋);
                connPE制袋.Open();
                InstruStateChange(connPE制袋, "生产指令");

                String strConnBPV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=BPV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connBPV制袋 = new SqlConnection(strConnBPV制袋);
                connBPV制袋.Open();
                InstruStateChange(connBPV制袋, "生产指令");

                String strConnPTV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=PTV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connPTV制袋 = new SqlConnection(strConnPTV制袋);
                connPTV制袋.Open();
                InstruStateChange(connPTV制袋, "生产指令");
                

                //去掉最后一个"、"，弹框提示
                if (Instru != null)
                {
                    Instru = Instru.Substring(0, Instru.Length - 1);
                    MessageBox.Show(Parameter.userName + "请接收生产指令：" + Instru, "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                conn吹膜.Dispose();
                conn清洁分切.Dispose();
                connCS制袋.Dispose();
                connPE制袋.Dispose();
                connBPV制袋.Dispose();
                connPTV制袋.Dispose();
            }

        }

        //更改生产指令状态
        private void InstruStateChange(OleDbConnection connOle, String tblName)
        {
            //读取未接收的生产指令
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = connOle;
            comm.CommandText = "select * from " + tblName + " where 接收人 like  '%' + @接收人 + '%' and 状态=1";
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
            commnew.CommandText = "UPDATE " + tblName + " SET 状态=2 where 接收人 = @接收人 and 状态=1";
            commnew.Parameters.AddWithValue("@接收人", Parameter.userName);
            
            commnew.ExecuteNonQuery();

            reader.Dispose();
            comm.Dispose();
            commnew.Dispose();
        }

        private void InstruStateChange(SqlConnection conn, String tblName)
        {
            //读取未接收的生产指令
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from " + tblName + " where 接收人 like  '%' + @接收人 + '%' and 状态=1";
            comm.Parameters.AddWithValue("@接收人", Parameter.userName);

            SqlDataReader reader = comm.ExecuteReader();//执行查询
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Instru += reader["生产指令编号"];
                    Instru += "、";
                }

            }

            //将状态变为已接收
            SqlCommand commnew = new SqlCommand();
            commnew.Connection = conn;
            commnew.CommandText = "UPDATE " + tblName + " SET 状态=2 where 接收人 = @接收人 and 状态=1";
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

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            //Thread FormThread = new Thread(new ThreadStart(ThreadFunc));
            //FormThread.Start();
            //Parameter.InitConnUser();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            
        }

    }
}