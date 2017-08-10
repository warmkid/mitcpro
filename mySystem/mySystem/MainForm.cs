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
using CustomUIControls;


namespace mySystem
{
    public partial class MainForm : Form
    {

        public bool isSqlOk = false;
        public SqlConnection conn;
        public OleDbConnection connOle;
        //TODO:时间间隔设置
        int interval = 600000; //十分钟
        TaskbarNotifier taskbarNotifier1; //右下角提示框

        public MainForm()
        {
            Parameter.InitConnUser(); //初始化连接到有用户表的数据库
            //Parameter.ConnUserInit();
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            
            InitializeComponent();
            RoleInit();
            userLabel.Text = Parameter.userName;
            InitTaskBar();
            SearchUnchecked();
            timer1.Interval = interval;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SearchUnchecked();
        }

        //定时器代码
        List<String> list吹膜;
        List<String> list清洁分切;
        List<String> listCS制袋;
        List<String> listPE制袋;

        private void SearchUnchecked()
        {
            String strCon吹膜 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/extrusionnew.mdb;Persist Security Info=False";
            list吹膜 = EachSearchUnchecked(strCon吹膜);
            String strCon清洁分切 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/welding.mdb;Persist Security Info=False";
            list清洁分切 = EachSearchUnchecked(strCon清洁分切);
            String strConCS制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
            listCS制袋 = EachSearchUnchecked(strConCS制袋);
            String strConPE制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/LDPE.mdb;Persist Security Info=False";
            //listPE制袋 = EachSearchUnchecked(strConPE制袋);

            if (list吹膜.Count + list清洁分切.Count + listCS制袋.Count == 0) return;

            String message = "以下表单中有待审核记录：\n";
            if (list吹膜.Count != 0)
            {
                message += "吹膜：\n";
                foreach (string table in list吹膜)
                { message += "   " + table + "\n"; }
            }
            if (list清洁分切.Count != 0)
            {
                message += "清洁分切：\n";
                foreach (string table in list清洁分切)
                { message += "   " + table + "\n"; }
            }
            if (listCS制袋.Count != 0)
            {
                message += "CS制袋：\n";
                foreach (string table in listCS制袋)
                { message += "   " + table + "\n"; }
            }
            //message += "PE制袋：\n";
            //foreach (string table in listPE制袋)
            //{ message += "   " + table + "\n"; }

            MessageBox.Show(message, "提示");
            //taskbarNotifier1.Show("提示", message, 500, 10000, 500);
        }

        private List<String> EachSearchUnchecked(string strcon)
        {
            OleDbConnection Conn;
            OleDbCommand comm;
            Conn = new OleDbConnection(strcon);
            Conn.Open();
            comm = new OleDbCommand();
            comm.Connection = Conn;
            comm.CommandText = "SELECT * FROM 用户权限 WHERE 审核员 LIKE " + "'%" + Parameter.userName + "%'";
            OleDbDataReader reader1 = comm.ExecuteReader();
            List<String> rightlist = new List<String>();
            while (reader1.Read())
            {
                rightlist.Add(reader1["步骤"].ToString());
            }
            reader1.Dispose();

            comm.CommandText = "SELECT * FROM 待审核";
            OleDbDataReader reader2 = comm.ExecuteReader();
            List<String> formlist = new List<String>();
            while (reader2.Read())
            {
                formlist.Add(reader2["表名"].ToString());
            }
            reader2.Dispose();
            comm.Dispose();
            Conn.Dispose();

            List<String> list = rightlist.Intersect(formlist).ToList<String>();

            return list;
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

        //退出登录按钮
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
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
                SearchUnchecked();
                timer1.Interval = interval;
                timer1.Start();
            }
            else
            {
                this.Close();
                Application.ExitThread();
            }
        }


        //右下角提示框状态初始化
        private void InitTaskBar()
        {
            taskbarNotifier1 = new TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(new Bitmap(Image.FromFile(@"../../pic/skin_big.bmp")), Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(new Bitmap(Image.FromFile(@"../../pic/close_logo.bmp")), Color.FromArgb(255, 0, 255), new Point(300, 12));
            taskbarNotifier1.TitleRectangle = new Rectangle(90, 25, 135, 60);
            taskbarNotifier1.ContentRectangle = new Rectangle(29, 70, 315, 250);
            taskbarNotifier1.CloseClickable = true;
            taskbarNotifier1.TitleClickable = false;
            taskbarNotifier1.ContentClickable = false;
            taskbarNotifier1.EnableSelectionRectangle = false;
            taskbarNotifier1.KeepVisibleOnMousOver = true;
            taskbarNotifier1.ReShowOnMouseOver = true;
        }


    }
}
