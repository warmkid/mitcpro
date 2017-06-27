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
        public string username; //登录用户名
        public int userID;
        public int userRole; //用户角色
        public string proInstruction; //吹膜生产指令
        public string csbagInstruction; //cs制袋生产指令
        public string cleancutInstruction; //清洁分切生产指令

        public MainForm()
        {
            if (isSqlOk)
            {
                conn = Init(conn);              
            }
            else
            {
                connOle = Init(connOle);               
            }
            
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            userID = login.userID;
            if (userID != 0)
            {
                userRole = checkRole(userID);
            }
            
            if (isSqlOk)
            {
                username = checkID(userID);
            }
            else
            {
                username = checkIDOle(userID);
            }
            
            
            InitializeComponent();
            RoleInit();
            userLabel.Text = username;

            //Rectangle ScreenArea = Screen.GetWorkingArea(this);
            //ProducePanelLeft.Size = new Size(160, ScreenArea.Height - 260);
            //ProducePanelRight.Size = new Size(ScreenArea.Width - 184, ScreenArea.Height - 260);
            //StockPanelLeft.Size = SystemPanelLeft.Size = ProducePanelLeft.Size;
            //StockPanelRight.Size = SystemPanelRight.Size = ProducePanelRight.Size;
            //this.textBox1.Text = ProducePanelRight.Size.Height.ToString();           

        }

        private void RoleInit()
        {
            switch (userRole)
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


        private int checkRole(int userID)
        {
            String tblName = "user_aoxing";
            List<String> queryCols = new List<String>(new String[] { "role_id" });
            List<String> whereCols = new List<String>(new String[] { "user_id" });
            List<Object> whereVals = new List<Object>(new Object[] { userID });
            List<List<Object>> res = Utility.selectAccess(connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            int user_Role = Convert.ToInt32(res[0][0]);

            return user_Role;

        }

        
        private string checkID(int userID)
        {
            string user = null;
            string searchsql = "select * from user_aoxing where user_id='" + userID + "'";
            SqlCommand comm = new SqlCommand(searchsql, conn);
            SqlDataReader myReader = comm.ExecuteReader();
            while (myReader.Read())
            {
                user = myReader.GetString(4);
            }

            myReader.Close();  
            comm.Dispose();
            return user;
        }

        private string checkIDOle(int userID)
        {
            string user = null;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = connOle;
            comm.CommandText = "select * from user_aoxing where user_id= @ID";
            comm.Parameters.AddWithValue("@ID", userID);

            OleDbDataReader myReader = comm.ExecuteReader();
            while (myReader.Read())
            {
                user = myReader.GetString(4);
            }

            myReader.Close();  
            comm.Dispose();
            return user;
        }

        private SqlConnection Init(SqlConnection myConnection)
        {
            //错误IP测试
            //strCon = @"server=10.105.223.14,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //正确IP
            strCon = @"server=10.105.223.19,56625;database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
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

        private OleDbConnection Init(OleDbConnection myConnection)
        {
            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/database1.mdb;Persist Security Info=False";
            isOk = false;
            myConnection = connToServerOle(strConnect);            
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return null;

            }
            MessageBox.Show("连接数据库成功", "success");
            return myConnection;
        }


        public void IPChanged(string IP,string port)
        {
            //获取新IP
            strCon = @"server=" + IP + "," + port + ";database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
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

        private OleDbConnection connToServerOle(string connectStr)
        {
            OleDbConnection myConn;
            try
            {
                myConn = new OleDbConnection(connectStr);
                myConn.Open();
            }
            catch
            {
                return null;
            }
            isOk = true;
            return myConn;
        }


        //工序按钮
        private void MainProduceBtn_Click(object sender, EventArgs e)
        {
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
            username = null;
            this.Hide();

            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            userID = login.userID;
            if (isSqlOk)
            {
                username = checkID(userID);
            }
            else
            {
                username = checkIDOle(userID);
            }
            if (username != null)
            {
                userLabel.Text = username;
                MainPanel.Controls.Clear();
                userRole = checkRole(userID);
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
