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

        public MainForm()
        {
            readSQLConfig();
            Parameter.InitConnUser(); //初始化连接到有用户表的数据库
            //Parameter.ConnUserInit();
            LoginForm login = new LoginForm(this);
            login.ShowDialog();
            
            InitializeComponent();
            userLabel.Text = Parameter.userName;
            InitTaskBar();
            SearchUnchecked();
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

        private void SearchUnchecked()
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
                list清洁分切 = EachSearchUnchecked(strCon清洁分切);
                String strConCS制袋 = "server=" + mySystem.Parameter.IP_port + ";database=csbag;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listCS制袋 = EachSearchUnchecked(strConCS制袋);
                String strConPE制袋 = "server=" + mySystem.Parameter.IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listPE制袋 = EachSearchUnchecked(strConPE制袋);
                String strConBPV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=BPV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listBPV制袋 = EachSearchUnchecked(strConBPV制袋);
                String strConPTV制袋 = "server=" + mySystem.Parameter.IP_port + ";database=PTV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                listPTV制袋 = EachSearchUnchecked(strConPTV制袋);
            }

            if (list吹膜.Count + list清洁分切.Count + listCS制袋.Count + listPE制袋.Count + listBPV制袋.Count + listPTV制袋.Count == 0) return;

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

            MessageBox.Show(message, "提示");
            //taskbarNotifier1.Show("提示", message, 500, 10000, 500);
        }

        private List<String> EachSearchUnchecked(string strcon)
        {
            if (!mySystem.Parameter.isSqlOk)
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
            else 
            {
                SqlConnection Conn;
                SqlCommand comm;
                Conn = new SqlConnection(strcon);
                Conn.Open();
                comm = new SqlCommand();
                comm.Connection = Conn;
                comm.CommandText = "SELECT * FROM 用户权限 WHERE 审核员 LIKE " + "'%" + Parameter.userName + "%'";
                SqlDataReader reader1 = comm.ExecuteReader();
                List<String> rightlist = new List<String>();
                while (reader1.Read())
                {
                    rightlist.Add(reader1["步骤"].ToString());
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
            import库存台账();
           
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
            
            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
            OleDbConnection conn;
            conn = new OleDbConnection(strConnect);
            conn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置存货档案", conn);
            DataTable dt存货档案 = new DataTable();
            da.Fill(dt存货档案);
            da = new OleDbDataAdapter("select * from 库存台帐 where 0=1", conn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 2; ; ++i)
            {

                string 仓库名称 = my.Cells[i, 1].Value != null ? Convert.ToString( my.Cells[i, 1].Value) : "";
                string 存货名称 = my.Cells[i, 3].Value != null ?Convert.ToString( my.Cells[i, 3].Value) : "";
                string 规格型号 = my.Cells[i, 4].Value != null ?Convert.ToString( my.Cells[i, 4].Value ): "";
                string 存货代码 = my.Cells[i, 5].Value != null ?Convert.ToString( my.Cells[i, 5].Value ): "";
                if (my.Cells[i, 5].Value == null) break;
                string 批号 = my.Cells[i, 6].Value != null ? Convert.ToString( my.Cells[i, 6].Value ): "";
                double 现存数量 = my.Cells[i, 8].Value != null ?Convert.ToDouble( my.Cells[i, 8].Value ): 0;

                

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
            da.Update(dt);
            MessageBox.Show("导入供应商成功");
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
                    MessageBox.Show("该账号已在别处登陆，您已下线","下线提醒",MessageBoxButtons.OK);
                    ExitBtn.PerformClick();
                }
            }
        }


    }
}
