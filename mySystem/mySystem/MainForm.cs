using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Setting;
using System.Data.SqlClient;
using System.Data.OleDb;


namespace mySystem
{
    public partial class MainForm : Form
    {

        public bool isSqlOk = false;
        public SqlConnection conn;
        public OleDbConnection connOle;
        public OleDbConnection connOleCleancut;
        public OleDbConnection connOleBag;
        public string username; //登录用户名
        public int userID;
        public int userRole; //用户角色
        public string proInstruction; //吹膜生产指令
        public string csbagInstruction; //cs制袋生产指令
        public string cleancutInstruction; //清洁分切生产指令
        

        public MainForm()
        {
            Parameter.InitConnUser(); //初始化连接到有用户表的数据库
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            
            InitializeComponent();
            RoleInit();
            userLabel.Text = Parameter.userName;

        }

        private void RoleInit()
        {
            switch (Parameter.userRole)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                    
                    break;
                default:
                    break;
            }
        }


        #region 接收生产指令
//        String Instru = null; //未接受的生产指令
        //未接收的生产指令
        private void InstruReceive()
        {
//            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/extrusionnew.mdb;Persist Security Info=False";
//            OleDbConnection connOle = new OleDbConnection(strConn);
//            connOle.Open();
//            OleDbCommand comm = new OleDbCommand();
//            comm.Connection = connOle;
//            comm.CommandText = "select * from 生产指令信息表 where 接收人= @接收人 and 状态=1";
//            comm.Parameters.AddWithValue("@接收人", Parameter.userName);
            
//            OleDbDataReader reader = comm.ExecuteReader();//执行查询
//            if (reader.HasRows)
//            {
//                while (reader.Read())
//                {
//                    Instru += reader["生产指令编号"];
//                    Instru += "、"; 
//                }
                
//            }
//            //去掉最后一个"、"
//            if (Instru != null)
//            { 
//                Instru = Instru.Substring(0, Instru.Length - 1);
//                MessageBox.Show(Parameter.userName + "请接收生产指令：" + Instru, "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//            }

//            //将状态变为已接收
//            OleDbCommand commnew = new OleDbCommand();
//            commnew.Connection = connOle;
//            commnew.CommandText = "UPDATE 生产指令信息表 SET 状态=2 where 接收人= @接收人 and 状态=1";
//            commnew.Parameters.AddWithValue("@接收人", Parameter.userName);
//            commnew.ExecuteNonQuery();

//            comm.Dispose();
//            commnew.Dispose();
        }
        #endregion

        //工序按钮
        private void MainProduceBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in MainPanel.Controls)
            { control.Dispose(); }
            MainPanel.Controls.Clear();
            MainProduceBtn.BackColor = Color.FromArgb(138, 158, 196);
            MainSettingBtn.BackColor = Color.FromName("Control");
            MainQueryBtn.BackColor = Color.FromName("Control");
            ProcessMainForm myDlg = new ProcessMainForm(this);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //设置按钮
        private void MainSettingBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in MainPanel.Controls)
            { control.Dispose(); }   
            MainPanel.Controls.Clear();
            MainProduceBtn.BackColor = Color.FromName("Control");
            MainSettingBtn.BackColor = Color.FromArgb(138, 158, 196);
            MainQueryBtn.BackColor = Color.FromName("Control");
            SettingMainForm myDlg = new SettingMainForm(this);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //台帐查询按钮
        private void MainQueryBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in MainPanel.Controls)
            { control.Dispose(); }
            MainPanel.Controls.Clear();
            MainProduceBtn.BackColor = Color.FromName("Control");
            MainSettingBtn.BackColor = Color.FromName("Control");
            MainQueryBtn.BackColor = Color.FromArgb(138, 158, 196);
            QueryMainForm myDlg = new QueryMainForm(this);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }


        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            foreach (Control control in MainPanel.Controls)
            { control.Dispose(); }
            MainPanel.Controls.Clear();
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            
            if (Parameter.userName != null)
            {
                userLabel.Text = Parameter.userName;
                RoleInit();
                MainProduceBtn.BackColor = Color.FromName("Control");
                MainSettingBtn.BackColor = Color.FromName("Control");
                MainQueryBtn.BackColor = Color.FromName("Control");
                this.Show();
            }
            else
            {
                this.Close();
                Application.ExitThread();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //InstruReceive();
        }



    }
}
