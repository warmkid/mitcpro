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
        bool isOk;
        string strCon;
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

            if (Parameter.userID != 0)
            {
                if (Parameter.isSqlOk)
                {
                    //Parameter.userName = Parameter.IDtoName(Parameter.userID);
                    //Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                    //Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                }
                else
                {
                    Parameter.userName = Parameter.IDtoName(Parameter.userID);
                    Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                    Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                }
            }

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
            //MainPanel.Controls[0].Left.controls[0].dis
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
                if (Parameter.isSqlOk)
                {
                    //Parameter.userName = Parameter.IDtoName(Parameter.userID);
                    //Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                    //Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                }
                else
                {
                    Parameter.userName = Parameter.IDtoName(Parameter.userID);
                    Parameter.userRole = Parameter.IDtoRole(Parameter.userID);
                    Parameter.userflight = Parameter.IDtoFlight(Parameter.userID);
                }
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



    }
}
