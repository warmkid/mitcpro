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

namespace mySystem
{
    public partial class MainForm : Form
    {

        bool isOk;
        string strCon;
        SqlConnection conn;
        string username; //登录用户名

        public MainForm()
        {
            conn = Init(conn);
            LoginForm login = new LoginForm(conn);
            login.ShowDialog();
            username = login.usernameLabel.Text;
            InitializeComponent();

            //Rectangle ScreenArea = Screen.GetWorkingArea(this);
            //ProducePanelLeft.Size = new Size(160, ScreenArea.Height - 260);
            //ProducePanelRight.Size = new Size(ScreenArea.Width - 184, ScreenArea.Height - 260);
            //StockPanelLeft.Size = SystemPanelLeft.Size = ProducePanelLeft.Size;
            //StockPanelRight.Size = SystemPanelRight.Size = ProducePanelRight.Size;
            //this.textBox1.Text = ProducePanelRight.Size.Height.ToString();
            MainProduceBtn.Image = Image.FromFile(@"../../pic/MainProduce.png", false);
            MainSettingBtn.Image = Image.FromFile(@"../../pic/MainSetting.png", false);
            MainQueryBtn.Image = Image.FromFile(@"../../pic/MainQuery.png", false);

        }

        private SqlConnection Init(SqlConnection myConnection)
        {
            //错误IP测试
            //strCon = @"server=10.105.223.14,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //正确IP
            strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            isOk = false;
            myConnection = connToServer(strCon);
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                Connect2SqlForm con2sql = new Connect2SqlForm();
                con2sql.IPChange += new Connect2SqlForm.DelegateIPChange(IPChanged);
                con2sql.ShowDialog();

                myConnection = connToServer(strCon);
            }
            MessageBox.Show("连接数据库成功", "success");
            return myConnection;
        }

        public void IPChanged(string IP,string port)
        {
            //获取新IP
            strCon = @"server=" + IP + "," + port + ";database=ProductionPlan;Uid=sa;Pwd=mitc";
        }


        private SqlConnection connToServer(string connectStr)
        {
            SqlConnection myConnection; 
            try
            {               
                myConnection=new SqlConnection(connectStr);
                myConnection.Open();
            }
            catch 
            {
                return null;
            }
            isOk = true;
            return myConnection;
        }


        //工序按钮
        private void MainProduceBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ProcessMainForm myDlg = new ProcessMainForm(conn);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //设置按钮
        private void MainSettingBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            SettingMainForm myDlg = new SettingMainForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //台帐查询按钮
        private void MainQueryBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            QueryMainForm myDlg = new QueryMainForm(conn);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }


    }
}
