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
using System.Configuration;
using System.Collections;
using System.Threading;

namespace mySystem
{
    public partial class MainForm : Form
    {

        public bool isSqlOk = false;
        public SqlConnection conn;
        public OleDbConnection connOle;
        //TODO:时间间隔设置
        int interval = 7200000; //两小时
        TaskbarNotifier taskbarNotifier1; //右下角提示框
        int timer2Interval = 60000;
        mySystem.Other.Connecting c;

        ProcessMainForm processForm;

        private void ThreadFunc()
        {
            MethodInvoker mi = new MethodInvoker(this.ShowMsgForm);
            this.BeginInvoke(mi);
        }

        private void ShowMsgForm()
        {
            
            c.Show();
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
        }  


        //void showInfo()
        //{
        //    CustomUIControls.TaskbarNotifier taskbarNotifier1;
        //    taskbarNotifier1 = new CustomUIControls.TaskbarNotifier();
        //    taskbarNotifier1.SetBackgroundBitmap(new Bitmap(Image.FromFile(@"../../pic/skin_logo.bmp")), Color.FromArgb(255, 0, 255));
        //    taskbarNotifier1.SetCloseBitmap(new Bitmap(Image.FromFile(@"../../pic/close_logo.bmp")), Color.FromArgb(255, 0, 255), new Point(190, 12));
        //    taskbarNotifier1.TitleRectangle = new Rectangle(65, 25, 135, 60);
        //    taskbarNotifier1.ContentRectangle = new Rectangle(15, 65, 205, 150);
        //    taskbarNotifier1.CloseClickable = true;
        //    taskbarNotifier1.TitleClickable = false;
        //    taskbarNotifier1.ContentClickable = false;
        //    taskbarNotifier1.EnableSelectionRectangle = false;
        //    taskbarNotifier1.KeepVisibleOnMousOver = true;
        //    taskbarNotifier1.ReShowOnMouseOver = true;
        //    taskbarNotifier1.Show("提示", "正在连接服务器", 500, 2000, 500);
        //    System.Console.WriteLine("thread");
        //}


        public MainForm()
        {
            
            readSQLConfig();
            //Thread FormThread = new Thread(new ThreadStart(ThreadFunc));
            //FormThread.Start();  
            
            //Parameter.InitConnUser(); //初始化连接到有用户表的数据库
            //Parameter.ConnUserInit();
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            



            InitializeComponent();
            userLabel.Text = Parameter.userName;
            InitTaskBar();
            //SearchUnchecked();


            // 接收指令
            mySystem.Other.我的任务 f = new Other.我的任务();

            //f.set指令(test);
            //f.Show();
            String instru = receiveInstr();
            if (instru != null )
            {
                f.set指令(instru);
                f.ShowDialog();
                if (instru != null && instru.Contains("吹膜"))
                {
                    MainProduceBtn.PerformClick();
                    processForm.Btn吹膜.PerformClick();
                }
            }
           



            check库存预警();
            timer1.Interval = interval;
            timer1.Start();

            // 检查登陆状态的定时器
            timer2.Interval = timer2Interval; // 一分钟
            timer2.Start();


            // 试用
            // 读取配置文件中的  sy   
            // 如果没有，则写入日期：2017/11/11
            // 读取日期，看看是否到了
            // 

            //
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            

            ConfigurationManager.RefreshSection("appSettings");
            string name;
            try
            {
                name = config.AppSettings.Settings["sy"].Value;
            }
            catch (Exception ee)
            {
                DateTime dt = new DateTime(2017, 11, 11);
                name = dt.ToString("yyyy/MM/dd");
                config.AppSettings.Settings.Add("sy", name);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            if (DateTime.Now > DateTime.Parse(name))
            {
                System.Threading.Thread.Sleep(5000);
                MessageBox.Show("数据库异常！");
                this.Close();
            }


            if (Parameter.userName == "123")
            {
                btn导入.Visible = true;
            }
        }

        void readSQLConfig()
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);

            ConfigurationManager.RefreshSection("appSettings");
            string ip, port, user, password, isSQL;
            try
            {
                ip = config.AppSettings.Settings["ip"].Value;
                port = config.AppSettings.Settings["port"].Value;
                user = config.AppSettings.Settings["user"].Value;
                password = config.AppSettings.Settings["password"].Value;
                isSQL = config.AppSettings.Settings["isSQL"].Value;
                Parameter.IP_port = ip + "," + port;
                Parameter.sql_user = user;
                Parameter.sql_pwd = password;
                Parameter.isSqlOk = (isSQL == "no" ? false : true);

            }
            catch (Exception ee)
            {
                
                ip = "127.0.0.1";
                port = "3306";
                user = "sa";
                password = "";
                isSQL = "no";
                Parameter.IP_port = ip + "," + port;
                Parameter.sql_user = user;
                Parameter.sql_pwd = password;
                Parameter.isSqlOk = (isSQL == "no" ? false : true);
                config.AppSettings.Settings.Add("ip", ip);
                config.AppSettings.Settings.Add("port", port);
                config.AppSettings.Settings.Add("user", user);
                config.AppSettings.Settings.Add("password", password);
                config.AppSettings.Settings.Add("isSQL", isSQL);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
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
        List<String> listBPV制袋;
        List<String> listPTV制袋;
        List<String> list订单;
        List<String> list库存;

        private String SearchUnchecked()
        {
            if (!mySystem.Parameter.isSqlOk)
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
                listPE制袋 = EachSearchUnchecked(strConPE制袋);
                String strConBPV制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                                Data Source=../../database/BPV.mdb;Persist Security Info=False";
                listBPV制袋 = EachSearchUnchecked(strConBPV制袋);
                String strConPTV制袋 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                                Data Source=../../database/PTV.mdb;Persist Security Info=False";
                listPTV制袋 = EachSearchUnchecked(strConPTV制袋);
            }
            else
            {
                //********************改为sql数据库*********************************
                String strCon吹膜 = "server=" + mySystem.Parameter.IP_port + ";database=extrusionnew;MultipleActiveResultSets=true;Uid="+Parameter.sql_user+";Pwd="+Parameter.sql_pwd;
                list吹膜 = EachSearchUnchecked(strCon吹膜);
                String strCon清洁分切 = "server=" + mySystem.Parameter.IP_port + ";database=welding;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                //list清洁分切 = EachSearchUnchecked(strCon清洁分切);
                String strConCS制袋 = "server=" + mySystem.Parameter.IP_port + ";database=csbag;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listCS制袋 = EachSearchUnchecked(strConCS制袋);
                String strConPE制袋 = "server=" + mySystem.Parameter.IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listPE制袋 = EachSearchUnchecked(strConPE制袋);
                //String strConBPV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=BPV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                //listBPV制袋 = EachSearchUnchecked(strConBPV制袋);
                //String strConPTV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=PTV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                //listPTV制袋 = EachSearchUnchecked(strConPTV制袋);
                String strCon订单库存 = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                list订单 = EachSearchUnchecked(strCon订单库存, "订单用户权限");
                list库存 = EachSearchUnchecked(strCon订单库存, "库存用户权限");

                list清洁分切 = new List<string>();
                //listCS制袋 = new List<string>();
                //listPE制袋 = new List<string>();
                listBPV制袋 = new List<string>();
                listPTV制袋 = new List<string>();
            }

            if (list吹膜.Count + list清洁分切.Count + listCS制袋.Count + listPE制袋.Count + listBPV制袋.Count + listPTV制袋.Count + list订单.Count + list库存.Count == 0) return "";

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
            if (listPE制袋.Count != 0)
            {
                message += "PE制袋：\n";
                foreach (string table in listPE制袋)
                { message += "   " + table + "\n"; }
            }
            if (listBPV制袋.Count != 0)
            {
                message += "BPV制袋：\n";
                foreach (string table in listBPV制袋)
                { message += "   " + table + "\n"; }
            }
            if (listPTV制袋.Count != 0)
            {
                message += "PTV制袋：\n";
                foreach (string table in listPTV制袋)
                { message += "   " + table + "\n"; }
            }
            if (list订单.Count != 0)
            {
                message += "订单：\n";
                foreach (string table in list订单)
                { message += "   " + table + "\n"; }
            }
            if (list库存.Count != 0)
            {
                message += "库存：\n";
                foreach (string table in list库存)
                { message += "   " + table + "\n"; }
            }

            //MessageBox.Show(message, "提示");
            return message;
            //taskbarNotifier1.Show("提示", message, 500, 10000, 500);
        }

        private List<String> EachSearchUnchecked(string strcon, string tblname = "用户权限")
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbConnection Conn;
                OleDbCommand comm;
                Conn = new OleDbConnection(strcon);
                Conn.Open();
                comm = new OleDbCommand();
                comm.Connection = Conn;
                comm.CommandText = "SELECT * FROM "+tblname+" WHERE 审核员 LIKE " + "'%" + Parameter.userName + "%'";
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
            else 
            {
                SqlConnection Conn;
                SqlCommand comm;
                Conn = new SqlConnection(strcon);
                Conn.Open();
                comm = new SqlCommand();
                comm.Connection = Conn;
                comm.CommandText = "SELECT * FROM " + tblname + " WHERE 审核员 LIKE " + "'%" + Parameter.userName + "%'";
                SqlDataReader reader1 = comm.ExecuteReader();
                List<String> rightlist = new List<String>();
                while (reader1.Read())
                {
                    if (reader1["步骤"].ToString() == "LDPE制袋生产指令")
                    {
                        rightlist.Add("生产指令");
                    }
                    else
                    {
                        rightlist.Add(reader1["步骤"].ToString());
                    }
                }
                reader1.Dispose();

                comm.CommandText = "SELECT * FROM 待审核";
                SqlDataReader reader2 = comm.ExecuteReader();
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
            processForm = new ProcessMainForm(this);
            processForm.TopLevel = false;
            processForm.FormBorderStyle = FormBorderStyle.None;
            processForm.Size = MainPanel.Size;
            MainPanel.Controls.Add(processForm);
            processForm.Show();
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
            // 清空数据库中的token信息

            String localUUID = Utility.getUUID();
            SqlDataAdapter da = new SqlDataAdapter("select * from users where 用户ID='" + mySystem.Parameter.userID + "'", Parameter.connUser);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable("user");
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                String remoteUUID = dt.Rows[0]["token"].ToString();
                if (remoteUUID == localUUID)
                {
                    dt.Rows[0]["token"] = "";
                    da.Update(dt);
                }
            }

            // ---

            // 停止检查登陆状态
            timer2.Stop();
            timer2.Dispose();
            
            timer1.Stop();
            timer1.Dispose();
            this.Hide();
            foreach (Control control in MainPanel.Controls)
            { control.Dispose(); }
            MainPanel.Controls.Clear();
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            // 接收指令

            mySystem.Other.我的任务 f = new Other.我的任务();

            //f.set指令(test);
            //f.Show();
            String instru = receiveInstr();
            if (instru != null)
            {
                
                f.set指令(instru);
                f.set表单("");
                f.ShowDialog();
            }
            //
            
            
            if (Parameter.userName != null)
            {
                userLabel.Text = Parameter.userName;
                MainProduceBtn.BackColor = Color.FromName("Control");
                MainSettingBtn.BackColor = Color.FromName("Control");
                MainQueryBtn.BackColor = Color.FromName("Control");
                this.Show();
                SearchUnchecked();
                timer1.Interval = interval;
                timer1.Start();
                timer2.Interval = timer2Interval;
                timer2.Start();
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

        private void btn浏览_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void btn导入_Click(object sender, EventArgs e)
        {
            //import供应商();
            //import存货档案();
            //import库存台账();
            truncate清空表数据();
        }

        void truncate清空表数据()
        {
            AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            String key = "ProcessToTruncate";
            if (appSettings.Settings[key] != null)
            {
                switch (appSettings.Settings[key].Value.ToString())
                {
                    case "PE":
                        if (DialogResult.OK == MessageBox.Show("确认要清空PE制袋表数据吗？", "提示", MessageBoxButtons.OKCancel))
                        {
                            清空PE表数据();
                        }
                        break;
                    case "吹膜":
                        if (DialogResult.OK == MessageBox.Show("确认要清空吹膜表数据吗？", "提示", MessageBoxButtons.OKCancel))
                        {
                            清空吹膜表数据();
                        }
                        break;
                    default:
                        MessageBox.Show("请先在配置文件中说明需要清空的工序");
                        break;
                }
            }
            else
            {
                appSettings.Settings.Add(key, "NONE");
                MessageBox.Show("请先在配置文件中说明需要清空的工序");
            }
            appSettings.CurrentConfiguration.Save();
        }

        void 清空PE表数据()
        {
            string connstr = "server=" + Parameter.IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            List<String> PE表名 = new List<string>();
            PE表名.Add("LDPE制袋日报表");
            PE表名.Add("LDPE制袋日报表详细信息");
            PE表名.Add("产品内包装记录");
            PE表名.Add("产品内包装记录详细信息");
            PE表名.Add("产品热合强度检验记录");
            PE表名.Add("产品热合强度检验记录详细记录");
            PE表名.Add("产品外包装记录表");
            PE表名.Add("产品外包装详细记录");
            PE表名.Add("产品外观和尺寸检验记录");
            PE表名.Add("产品外观和尺寸检验记录详细信息");
            PE表名.Add("待审核");
            PE表名.Add("岗位交接班记录");
            PE表名.Add("岗位交接班项目记录");
            PE表名.Add("洁净区温湿度记录表");
            PE表名.Add("洁净区温湿度记录详细信息");
            PE表名.Add("内标签");
            PE表名.Add("批生产记录表");
            PE表名.Add("批生产记录产品详细信息");
            PE表名.Add("批生产记录目录详细信息");
            PE表名.Add("清场记录");
            PE表名.Add("清场记录详细信息");
            PE表名.Add("生产领料申请单表");
            PE表名.Add("生产领料申请单详细信息");
            PE表名.Add("生产领料使用记录");
            PE表名.Add("生产领料使用记录详细信息");
            PE表名.Add("生产退料记录表");
            PE表名.Add("生产退料记录详细信息");
            PE表名.Add("生产指令");
            PE表名.Add("生产指令详细信息");
            PE表名.Add("外标签");
            PE表名.Add("制袋机组开机前确认表");
            PE表名.Add("制袋机组开机前确认项目记录");
            PE表名.Add("制袋机组运行记录");
            PE表名.Add("制袋机组运行记录详细信息");
            清空很多表的数据(PE表名, conn);
        }

        void 清空吹膜表数据()
        {
            string connstr = "server=" + Parameter.IP_port + ";database=extrusionnew;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            List<String> 吹膜表名 = new List<string>();
            吹膜表名.Add("标签");
            吹膜表名.Add("产品内包装记录表");
            吹膜表名.Add("产品内包装详细记录");
            吹膜表名.Add("产品外包装记录表");
            吹膜表名.Add("产品外包装详细记录");
            吹膜表名.Add("吹膜岗位交接班记录");
            吹膜表名.Add("吹膜岗位交接班项目记录");
            吹膜表名.Add("吹膜工序废品记录");
            吹膜表名.Add("吹膜工序废品记录详细信息");
            吹膜表名.Add("吹膜工序领料退料记录");
            吹膜表名.Add("吹膜工序领料详细记录");
            吹膜表名.Add("吹膜工序清场吹膜工序项目记录");
            吹膜表名.Add("吹膜工序清场记录");
            吹膜表名.Add("吹膜工序清场项目记录");
            吹膜表名.Add("吹膜工序生产和检验记录");
            吹膜表名.Add("吹膜工序生产和检验记录详细信息");
            吹膜表名.Add("吹膜工序物料平衡记录");
            吹膜表名.Add("吹膜供料记录");
            吹膜表名.Add("吹膜供料记录详细信息");
            吹膜表名.Add("吹膜供料系统运行记录");
            吹膜表名.Add("吹膜供料系统运行记录详细信息");
            吹膜表名.Add("吹膜机安全培训记录");
            吹膜表名.Add("吹膜机安全培训记录人员情况");
            吹膜表名.Add("吹膜机更换过滤网记录表");
            吹膜表名.Add("吹膜机组换模头检查表");
            吹膜表名.Add("吹膜机组换模头检查项目");
            吹膜表名.Add("吹膜机组换模芯检查表");
            吹膜表名.Add("吹膜机组换模芯检查项目");
            吹膜表名.Add("吹膜机组开机前确认表");
            吹膜表名.Add("吹膜机组开机前确认项目记录");
            吹膜表名.Add("吹膜机组清洁记录表");
            吹膜表名.Add("吹膜机组清洁项目记录表");
            吹膜表名.Add("吹膜机组预热参数记录表");
            吹膜表名.Add("吹膜机组运行记录");
            吹膜表名.Add("吹膜生产日报表");
            吹膜表名.Add("吹膜生产日报表详细信息");
            吹膜表名.Add("待审核");
            吹膜表名.Add("批生产记录表");
            吹膜表名.Add("批生产记录产品详细信息");
            吹膜表名.Add("批生产记录目录详细信息");
            吹膜表名.Add("生产领料申请单表");
            吹膜表名.Add("生产领料申请单详细信息");
            吹膜表名.Add("生产指令产品列表");
            吹膜表名.Add("生产指令信息表");
            吹膜表名.Add("是否可以新建生产检验记录");
            吹膜表名.Add("已打印标签");
            清空很多表的数据(吹膜表名, conn);

        }

        void 清空很多表的数据(List<string> tbls, SqlConnection conn)
        {
            foreach (string tblname in tbls)
            {
                清空表数据(tblname, conn);
            }
        }

        void 清空表数据(string tblname, SqlConnection conn)
        {
            if (DialogResult.OK != MessageBox.Show("清空表  " + tblname + "  ?", "提示", MessageBoxButtons.OKCancel))
            {
                return;
            }
            string sql = "truncate table {0}";
            SqlCommand comm;
            comm = new SqlCommand(string.Format(sql, tblname), conn);
            comm.ExecuteNonQuery();
        }

        void import库存台账()
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值
            
//            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//            OleDbConnection conn;
//            conn = new OleDbConnection(strConnect);
//            conn.Open();

            string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn;
            conn = new SqlConnection(strConnect);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案", conn);
            DataTable dt存货档案 = new DataTable();
            da.Fill(dt存货档案);
            da = new SqlDataAdapter("select * from 库存台帐 where", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 2; ; ++i)
            {
                string 存货代码 = my.Cells[i, 5].Value != null ? Convert.ToString(my.Cells[i, 5].Value) : "";
                string 批号 = my.Cells[i, 6].Value != null ? Convert.ToString(my.Cells[i, 6].Value) : "";
                DataRow[] drs库存 = dt.Select("产品代码='" + 存货代码 + "' and 产品批号='" + 批号 + "'");


                if (drs库存.Length == 0)
                {
                    string 仓库名称 = my.Cells[i, 1].Value != null ? Convert.ToString(my.Cells[i, 1].Value) : "";
                    string 存货名称 = my.Cells[i, 3].Value != null ? Convert.ToString(my.Cells[i, 3].Value) : "";
                    string 规格型号 = my.Cells[i, 4].Value != null ? Convert.ToString(my.Cells[i, 4].Value) : "";

                    if (my.Cells[i, 5].Value == null) break;

                    double 现存数量 = my.Cells[i, 8].Value != null ? Convert.ToDouble(my.Cells[i, 8].Value) : 0;



                    DataRow dr = dt.NewRow();
                    dr["仓库名称"] = 仓库名称;
                    dr["产品名称"] = 存货名称;
                    dr["产品代码"] = 存货代码;
                    dr["产品规格"] = 规格型号;
                    dr["产品批号"] = 批号;
                    dr["现存数量"] = 现存数量;
                    dr["状态"] = "合格";
                    dr["用途"] = "__自由";
                    dr["冻结状态"] = true;
                    dr["供应商名称"] = "无";
                    DataRow[] drs = dt存货档案.Select("存货代码='" + 存货代码 + "'");
                    if (drs.Length > 0)
                    {
                        dr["换算率"] = drs[0]["换算率"];
                        dr["现存件数"] = Convert.ToDouble(dr["现存数量"]) / Convert.ToDouble(drs[0]["换算率"]);
                        dr["主计量单位"] = drs[0]["主计量单位名称"];
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    double 现存数量 = my.Cells[i, 8].Value != null ? Convert.ToDouble(my.Cells[i, 8].Value) : 0;
                    drs库存[0]["现存数量"] = 现存数量;
                    DataRow[] drs = dt存货档案.Select("存货代码='" + 存货代码 + "'");
                    if (drs.Length > 0)
                    {
                        drs库存[0]["换算率"] = drs[0]["换算率"];
                        drs库存[0]["现存件数"] = Convert.ToDouble(drs库存[0]["现存数量"]) / Convert.ToDouble(drs[0]["换算率"]);
                        drs库存[0]["主计量单位"] = drs[0]["主计量单位名称"];
                    }
                }
            }
            da.Update(dt);
            MessageBox.Show("导入库存台账成功");
        }

        void import供应商()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                // 打开一个Excel进程
                Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
                // 利用这个进程打开一个Excel文件
                //System.IO.Directory.GetCurrentDirectory;
                Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
                // 选择一个Sheet，注意Sheet的序号是从1开始的
                Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
                // 设置该进程是否可见
                //oXL.Visible = true;
                // 修改Sheet中某行某列的值
                List<String> ls = new List<string>();
                for (int i = 3; i <= 66; ++i)
                {
                    ls.Add(my.Cells[i, 2].Value);
                }
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection conn;
                conn = new OleDbConnection(strConnect);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置供应商信息 where 0=1", conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (string gys in ls)
                {
                    DataRow dr = dt.NewRow();
                    dr["供应商代码"] = "";
                    dr["供应商名称"] = gys;
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入供应商成功");
            }
            else
            {
                // 打开一个Excel进程
                Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
                // 利用这个进程打开一个Excel文件
                //System.IO.Directory.GetCurrentDirectory;
                Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
                // 选择一个Sheet，注意Sheet的序号是从1开始的
                Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
                // 设置该进程是否可见
                //oXL.Visible = true;
                // 修改Sheet中某行某列的值
                List<String> ls = new List<string>();
                for (int i = 3; i <= 66; ++i)
                {
                    ls.Add(my.Cells[i, 2].Value);
                }
                string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection conn;
                conn = new SqlConnection(strConnect);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置供应商信息 where 0=1", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (string gys in ls)
                {
                    DataRow dr = dt.NewRow();
                    dr["供应商代码"] = "";
                    dr["供应商名称"] = gys;
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入供应商成功");
            }

        }

        void import存货档案()
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);


            List<String> ls存货名称 = new List<string>();
            List<String> ls存货代码 = new List<string>();
            List<String> ls规格型号 = new List<string>();
            List<String> ls主计量单位 = new List<string>();
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值
           
            for (int i = 2; i <= 832; ++i)
            {
                if (ls存货代码.IndexOf(my.Cells[i, 5].Value) >= 0) continue;
                ls存货代码.Add(my.Cells[i, 5].Value);
                ls存货名称.Add(my.Cells[i, 4].Value);
                ls规格型号.Add(my.Cells[i, 6].Value);
                ls主计量单位.Add(my.Cells[i, 10].Value);
            }
            my = wb.Worksheets[2];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            for (int i = 2; i <= 727; ++i)
            {
                if (ls存货代码.IndexOf(my.Cells[i, 5].Value) >= 0) continue;
                ls存货名称.Add(my.Cells[i, 4].Value);
                ls存货代码.Add(my.Cells[i, 5].Value);
                ls规格型号.Add(my.Cells[i, 6].Value);
                ls主计量单位.Add(my.Cells[i, 10].Value);
            }

            if (!mySystem.Parameter.isSqlOk)
            {
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection conn;
                conn = new OleDbConnection(strConnect);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置存货档案 where 0=1", conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < ls主计量单位.Count; ++i)
                {
                    DataRow dr = dt.NewRow();
                    dr["存货名称"] = ls存货名称[i];
                    dr["存货代码"] = ls存货代码[i];
                    dr["规格型号"] = ls规格型号[i];
                    dr["主计量单位名称"] = ls主计量单位[i];
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入存货档案成功");
            }
            else
            {
                string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection conn;
                conn = new SqlConnection(strConnect);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案 where 0=1", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < ls主计量单位.Count; ++i)
                {
                    DataRow dr = dt.NewRow();
                    dr["存货名称"] = ls存货名称[i];
                    dr["存货代码"] = ls存货代码[i];
                    dr["规格型号"] = ls规格型号[i];
                    dr["主计量单位名称"] = ls主计量单位[i];
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入存货档案成功");
            }

        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            // 检查UUID是否一致
            String localUUID = Utility.getUUID();
            SqlDataAdapter da = new SqlDataAdapter("select * from users where 用户ID='" + mySystem.Parameter.userID + "'", Parameter.connUser);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable("user");
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                String remoteUUID = dt.Rows[0]["token"].ToString();
                if (remoteUUID == "")
                {
                    dt.Rows[0]["token"] = localUUID;
                }
                else if (localUUID != remoteUUID)
                {
                    timer2.Stop();
                    MessageBox.Show("该账号已在别处登陆，您已下线","下线提醒",MessageBoxButtons.OK);
                    ExitBtn.PerformClick();
                }
            }
        }

        void check库存预警()
        {
            String strCon = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();
            Hashtable ht = new Hashtable();
            SqlDataAdapter da;
            DataTable dt;
            List<string> 预警代码 = new List<string>();
            // 获取预警设置
            da = new SqlDataAdapter("select * from 设置库存预警", conn);
            dt = new DataTable("库存预警");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["产品代码"].ToString(), Convert.ToDouble(dr["预警值"]));
            }
            // 检查是否要预警
            String sql = "select sum(现存数量) from 库存台帐 where 产品代码='{0}'";
            SqlCommand comm;
            foreach (Object k in ht.Keys)
            {
                comm = new SqlCommand(String.Format(sql, k.ToString()), conn);
                Object res = comm.ExecuteScalar();
                if (res != DBNull.Value)
                {
                    double a = Convert.ToDouble(comm.ExecuteScalar());
                    double b = Convert.ToDouble(ht[k]);
                    if (a <= b) 预警代码.Add(k.ToString());
                }
            }
            if (预警代码.Count > 0)
            {
                StringBuilder sb = new StringBuilder("下列产品库存告急：\n");
                foreach (String s in 预警代码)
                {
                    sb.Append(s+"\n");
                }
                MessageBox.Show(sb.ToString(), "警告");
            }
        }

        private String Instru = null;
        private void btnMyTask_Click(object sender, EventArgs e)
        {
            

            //
            //string file = System.Windows.Forms.Application.ExecutablePath;
            //Configuration config = ConfigurationManager.OpenExeConfiguration(file);

            //ConfigurationManager.RefreshSection("appSettings");
            //string test;
            //try
            //{
            //    test = config.AppSettings.Settings["test"].Value;
                

            //}catch(Exception ee){
            //    test = ee.Message + "\n" + ee.StackTrace;
            //}
            //
            mySystem.Other.我的任务 f = new Other.我的任务();

            //f.set指令(test);
            //f.Show();
            String instru = receiveInstr();
            String msg = SearchUnchecked();
            if (instru == null && msg.Equals(""))
            {
                MessageBox.Show("暂时没有新任务", "提示");
                return;
            }
            //if (instru == null)
            //{
            //    MessageBox.Show(msg, "提示");
            //}
            else
            {
                f.set指令(instru);
                f.set表单(msg);
                f.ShowDialog();
                //MessageBox.Show("请接收指令：\t " + instru + "\n\n" + msg, "提示");
                if (instru!=null && instru.Contains("吹膜"))
                {
                    MainProduceBtn.PerformClick();
                    processForm.Btn吹膜.PerformClick();
                }
            }
        }

        String receiveInstr()
        {
            Instru = null;
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
                InstruStateChange(conn吹膜, "生产指令信息表",1);


                String strConn清洁分切 = "server=" + mySystem.Parameter.IP_port + ";database=welding;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection conn清洁分切 = new SqlConnection(strConn清洁分切);
                conn清洁分切.Open();
                InstruStateChange(conn清洁分切, "清洁分切工序生产指令",2);


                String strConnCS制袋 = "server=" + mySystem.Parameter.IP_port + ";database=csbag;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connCS制袋 = new SqlConnection(strConnCS制袋);
                connCS制袋.Open();
                InstruStateChange(connCS制袋, "生产指令",4);


                String strConnPE制袋 = "server=" + mySystem.Parameter.IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connPE制袋 = new SqlConnection(strConnPE制袋);
                connPE制袋.Open();
                InstruStateChange(connPE制袋, "生产指令",5);

                String strConnBPV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=BPV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connBPV制袋 = new SqlConnection(strConnBPV制袋);
                connBPV制袋.Open();
                InstruStateChange(connBPV制袋, "生产指令",6);

                String strConnPTV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=PTV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection connPTV制袋 = new SqlConnection(strConnPTV制袋);
                connPTV制袋.Open();
                InstruStateChange(connPTV制袋, "生产指令",7);


                //去掉最后一个"、"，弹框提示
                if (Instru != null)
                {
                    Instru = Instru.Substring(0, Instru.Length - 1);
                    //MessageBox.Show(Parameter.userName + "请接收生产指令：" + Instru, "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                conn吹膜.Dispose();
                conn清洁分切.Dispose();
                connCS制袋.Dispose();
                connPE制袋.Dispose();
                connBPV制袋.Dispose();
                connPTV制袋.Dispose();
            }
            return Instru;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tblName"></param>
        /// <param name="gongxu">gongxu:1，吹膜；2，清洁分切；3，灭菌；4，CS制袋；5，PE制袋；6，BPV制袋；7，PTV制袋</param>
        ///gongxu:1，吹膜；2，清洁分切；3，灭菌；4，CS制袋；5，PE制袋；6，BPV制袋；7，PTV制袋
        private void InstruStateChange(SqlConnection conn, String tblName, int gongxu)
        {
            if (Parameter.userName == null) return;
            //读取未接收的生产指令
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from " + tblName + " where 接收人 like  '%' + @接收人 + '%' and 状态=1";
            comm.Parameters.AddWithValue("@接收人", Parameter.userName);

            SqlDataReader reader = comm.ExecuteReader();//执行查询
            if (reader.HasRows)
            {
                switch(gongxu){
                    case 1:
                        Instru += "吹膜：\n";
                        break;
                    case 2:
                        Instru += "清洁分切：\n";
                        break;
                    case 3:
                        Instru += "灭菌：\n";
                        break;
                    case 4:
                        Instru += "CS制袋：\n";
                        break;
                    case 5:
                        Instru += "PE制袋：\n";
                        break;
                    case 6:
                        Instru += "BPV制袋：\n";
                        break;
                    case 7:
                        Instru += "PTV制袋：\n";
                        break;
                }
                
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            

        }


        # region 自动更新

        private void button2_Click(object sender, EventArgs e)
        {
            // 获取版本信息
            string localVersion = getLocalVersion();
            string remoteVersion = getRemoteVersion();
            if (string.Compare(remoteVersion, localVersion) > 0)
            {
                // 确认是否更新
                if (DialogResult.OK ==
                    MessageBox.Show(String.Format("检测到新版本: {0}(当前版本{1})，是否更新？",
                    remoteVersion, localVersion), "提示", MessageBoxButtons.OKCancel))
                {
                    // 弹出进度对话框
                    Other.AutoUpdate form = new Other.AutoUpdate();
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("已经是最新版本");
            }
            
            
        }

        public static void DownloadFile(string URL, string filename, 
            System.Windows.Forms.ProgressBar prog=null, 
            System.Windows.Forms.Label label1=null)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    if (label1 != null)
                    {
                        label1.Text = String.Format("当前文件({0})\n下载进度",
                            System.IO.Path.GetFileName(filename)) + percent.ToString("F2") + "%";
                    }
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        string getRemoteVersion()
        {
            //AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            //String key = "version_url";
            //if (appSettings.Settings[key] == null || appSettings.Settings[key].Value.Trim() == "")
            //{
            //    return "";
            //}
            String filepath = @"r_version.txt";
            DownloadFile(@"http://123.206.90.229/erp/version.txt", filepath);
            return getLocalVersion(filepath);
        }

        string getLocalVersion(String filepath = @"version.txt")
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
            {
                return sr.ReadLine();
            }
        }

        # endregion

    }
}
 