using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using System.Data.OleDb;
using WindowsFormsApplication1;
using System.Threading;
using BatchProductRecord;
using mySystem.Process.Extruction;
using CustomUIControls;


namespace mySystem
{
    public partial class ExtructionMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;
        TaskbarNotifier taskbarNotifier1; //右下角提示框
        //System.Timers.Timer timer = new System.Timers.Timer();//实例化Timer类

        public ExtructionMainForm(MainForm mainform): base(mainform)
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            InitBtn();
            InitTaskBar();
        }

        #region 按钮状态
        //初始化按钮状态
        public void InitBtn()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                bool checkBeforePower;
                bool extrusClean;
                bool preheat;
                // 开机前确认表
                String tblName1 = "吹膜机组开机前确认表";
                List<String> queryCols1 = new List<String>(new String[] { "审核人" });
                List<String> whereCols1 = new List<String>(new String[] { "生产指令ID" });
                List<Object> whereVals1 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, tblName1, queryCols1, whereCols1, whereVals1, null, null, null, null, null);
                if (res1.Count != 0)
                {
                    checkBeforePower = true;
                }
                else
                {
                    checkBeforePower = false;
                }
                //吹膜机组清洁记录
                String tblName2 = "吹膜机组清洁记录表";
                List<String> queryCols2 = new List<String>(new String[] { "审核人" });
                List<String> whereCols2 = new List<String>(new String[] { "生产指令ID" });
                List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, tblName2, queryCols2, whereCols2, whereVals2, null, null, null, null, null);
                if (res2.Count != 0)
                {
                    extrusClean = true;
                }
                else
                {
                    extrusClean = false;
                }
                //吹膜机组预热参数记录表
                String tblName3 = "吹膜机组预热参数记录表";
                List<String> queryCols3 = new List<String>(new String[] { "审核人" });
                List<String> whereCols3 = new List<String>(new String[] { "生产指令id" });
                List<Object> whereVals3 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res3 = Utility.selectAccess(Parameter.connOle, tblName3, queryCols3, whereCols3, whereVals3, null, null, null, null, null);
                if (res3.Count != 0)
                {
                    preheat = true;
                }
                else
                {
                    preheat = false;
                }


                //按钮状态
                A2Btn.Enabled = true;
                int index = comboBox1.SelectedIndex;
                if (index == -1)  //未选择生产指令
                {
                    A3Btn.Enabled = false;
                    C1Btn.Enabled = false;
                    C2Btn.Enabled = false;
                    A5Btn.Enabled = false;
                    otherBtnInit(false);
                }
                else
                {
                    C1Btn.Enabled = false;
                    A3Btn.Enabled = true;
                    C2Btn.Enabled = false;
                    cleanBtnInit(); //判断清场按钮是否可点
                    otherBtnInit(false);
                    if (extrusClean)
                    {
                        C1Btn.Enabled = true;
                        if (checkBeforePower)
                        {
                            C2Btn.Enabled = true;
                            if (preheat)
                            {
                                otherBtnInit(true);
                            }
                        }
                    }

                }
            }
            else
            {
                bool checkBeforePower;
                bool extrusClean;
                bool preheat;
                // 开机前确认表
                String tblName1 = "吹膜机组开机前确认表";
                List<String> queryCols1 = new List<String>(new String[] { "审核人" });
                List<String> whereCols1 = new List<String>(new String[] { "生产指令ID" });
                List<Object> whereVals1 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res1 = Utility.selectAccess(Parameter.conn, tblName1, queryCols1, whereCols1, whereVals1, null, null, null, null, null);
                if (res1.Count != 0)
                {
                    checkBeforePower = true;
                }
                else
                {
                    checkBeforePower = false;
                }
                //吹膜机组清洁记录
                String tblName2 = "吹膜机组清洁记录表";
                List<String> queryCols2 = new List<String>(new String[] { "审核人" });
                List<String> whereCols2 = new List<String>(new String[] { "生产指令ID" });
                List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res2 = Utility.selectAccess(Parameter.conn, tblName2, queryCols2, whereCols2, whereVals2, null, null, null, null, null);
                if (res2.Count != 0)
                {
                    extrusClean = true;
                }
                else
                {
                    extrusClean = false;
                }
                //吹膜机组预热参数记录表
                String tblName3 = "吹膜机组预热参数记录表";
                List<String> queryCols3 = new List<String>(new String[] { "审核人" });
                List<String> whereCols3 = new List<String>(new String[] { "生产指令id" });
                List<Object> whereVals3 = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res3 = Utility.selectAccess(Parameter.conn, tblName3, queryCols3, whereCols3, whereVals3, null, null, null, null, null);
                if (res3.Count != 0)
                {
                    preheat = true;
                }
                else
                {
                    preheat = false;
                }


                //按钮状态
                A2Btn.Enabled = true;
                int index = comboBox1.SelectedIndex;
                if (index == -1)  //未选择生产指令
                {
                    A3Btn.Enabled = false;
                    C1Btn.Enabled = false;
                    C2Btn.Enabled = false;
                    A5Btn.Enabled = false;
                    otherBtnInit(false);
                }
                else
                {
                    C1Btn.Enabled = false;
                    A3Btn.Enabled = true;
                    C2Btn.Enabled = false;
                    cleanBtnInit(); //判断清场按钮是否可点
                    otherBtnInit(false);
                    if (extrusClean)
                    {
                        C1Btn.Enabled = true;
                        if (checkBeforePower)
                        {
                            C2Btn.Enabled = true;
                            if (preheat)
                            {
                                otherBtnInit(true);
                            }
                        }
                    }

                }
            }


        }

        //清场记录按钮状态
        public void cleanBtnInit()
        {
            //1、批生产记录（吹膜）  2、吹膜机组清洁记录  3、吹膜岗位交接班记录  4、吹膜供料记录
            //5、吹膜工序废品记录  6、吹膜工序领料退料记录  7、吹膜生产日报表  8、吹膜工序生产和检验记录
            //9、吹膜工序物料平衡记录  10、吹膜机组开机前确认表  11、吹膜机组预热参数记录表
            //12、吹膜供料系统运行记录 13、吹膜机组运行记录
            bool b2, b3, b4, b5, b6, b8, b10, b11, b12, b13;
            //string str1 = "select * from 批生产记录表 where 生产指令ID =" + Parameter.proInstruID;
            string str2 = "select * from 吹膜机组清洁记录表 where 生产指令ID = " + Parameter.proInstruID;
            string str3 = "select * from 吹膜岗位交接班记录 where 生产指令ID = " + Parameter.proInstruID;
            string str4 = "select * from 吹膜供料记录 where 生产指令ID = " + Parameter.proInstruID;
            string str5 = "select * from 吹膜工序废品记录 where 生产指令ID = " + Parameter.proInstruID;
            string str6 = "select * from 吹膜工序领料退料记录 where 生产指令ID = " + Parameter.proInstruID;
            //string str7 = "select * from 吹膜生产日报表 where 生产指令ID = " + Parameter.proInstruID;
            string str8 = "select * from 吹膜工序生产和检验记录 where 生产指令ID = " + Parameter.proInstruID;
            //string str9 = "select * from 吹膜工序物料平衡记录 where 生产指令ID = " + Parameter.proInstruID;
            string str10 = "select * from 吹膜机组开机前确认表 where 生产指令ID = " + Parameter.proInstruID;
            string str11 = "select * from 吹膜机组预热参数记录表 where 生产指令id = " + Parameter.proInstruID;
            string str12 = "select * from 吹膜供料系统运行记录 where 生产指令ID = " + Parameter.proInstruID;
            string str13 = "select * from 吹膜机组运行记录 where 生产指令ID = " + Parameter.proInstruID;

            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbConnection conn = Parameter.connOle;
                //OleDbCommand comm1 = new OleDbCommand(str1, conn);
                OleDbCommand comm2 = new OleDbCommand(str2, conn);
                OleDbCommand comm3 = new OleDbCommand(str3, conn);
                OleDbCommand comm4 = new OleDbCommand(str4, conn);
                OleDbCommand comm5 = new OleDbCommand(str5, conn);
                OleDbCommand comm6 = new OleDbCommand(str6, conn);
                //OleDbCommand comm7 = new OleDbCommand(str7, conn);
                OleDbCommand comm8 = new OleDbCommand(str8, conn);
                //OleDbCommand comm9 = new OleDbCommand(str9, conn);
                OleDbCommand comm10 = new OleDbCommand(str10, conn);
                OleDbCommand comm11 = new OleDbCommand(str11, conn);
                OleDbCommand comm12 = new OleDbCommand(str12, conn);
                OleDbCommand comm13 = new OleDbCommand(str13, conn);
                //OleDbDataReader reader1 = comm1.ExecuteReader();
                OleDbDataReader reader2 = comm2.ExecuteReader();
                OleDbDataReader reader3 = comm3.ExecuteReader();
                OleDbDataReader reader4 = comm4.ExecuteReader();
                OleDbDataReader reader5 = comm5.ExecuteReader();
                OleDbDataReader reader6 = comm6.ExecuteReader();
                //OleDbDataReader reader7 = comm7.ExecuteReader();
                OleDbDataReader reader8 = comm8.ExecuteReader();
                //OleDbDataReader reader9 = comm9.ExecuteReader();
                OleDbDataReader reader10 = comm10.ExecuteReader();
                OleDbDataReader reader11 = comm11.ExecuteReader();
                OleDbDataReader reader12 = comm12.ExecuteReader();
                OleDbDataReader reader13 = comm13.ExecuteReader();
                //b1 = reader1.HasRows == true ? true : false;
                b2 = reader2.HasRows == true ? true : false;
                b3 = reader3.HasRows == true ? true : false;
                b4 = reader4.HasRows == true ? true : false;
                b5 = reader5.HasRows == true ? true : false;
                b6 = reader6.HasRows == true ? true : false;
                //b7 = reader7.HasRows == true ? true : false;
                b8 = reader8.HasRows == true ? true : false;
                //b9 = reader9.HasRows == true ? true : false;
                b10 = reader10.HasRows == true ? true : false;
                b11 = reader11.HasRows == true ? true : false;
                b12 = reader12.HasRows == true ? true : false;
                b13 = reader13.HasRows == true ? true : false;
                if (b2 && b3 && b4 && b5 && b6 && b8 && b10 && b11 && b12 && b13)
                { A5Btn.Enabled = true; }
                else
                { A5Btn.Enabled = false; }

                //reader1.Dispose();
                reader2.Dispose();
                reader3.Dispose();
                reader4.Dispose();
                reader5.Dispose();
                reader6.Dispose();
                //reader7.Dispose();
                reader8.Dispose();
                //reader9.Dispose();
                reader10.Dispose();
                reader11.Dispose();
                reader12.Dispose();
                reader13.Dispose();
                //comm1.Dispose();
                comm2.Dispose();
                comm3.Dispose();
                comm4.Dispose();
                comm5.Dispose();
                comm6.Dispose();
                //comm7.Dispose();
                comm8.Dispose();
                //comm9.Dispose();
                comm10.Dispose();
                comm11.Dispose();
                comm12.Dispose();
                comm13.Dispose();
            }
            else
            {
                SqlConnection conn = Parameter.conn;
                //OleDbCommand comm1 = new OleDbCommand(str1, conn);
                SqlCommand comm2 = new SqlCommand(str2, conn);
                SqlCommand comm3 = new SqlCommand(str3, conn);
                SqlCommand comm4 = new SqlCommand(str4, conn);
                SqlCommand comm5 = new SqlCommand(str5, conn);
                SqlCommand comm6 = new SqlCommand(str6, conn);
                //OleDbCommand comm7 = new OleDbCommand(str7, conn);
                SqlCommand comm8 = new SqlCommand(str8, conn);
                //OleDbCommand comm9 = new OleDbCommand(str9, conn);
                SqlCommand comm10 = new SqlCommand(str10, conn);
                SqlCommand comm11 = new SqlCommand(str11, conn);
                SqlCommand comm12 = new SqlCommand(str12, conn);
                SqlCommand comm13 = new SqlCommand(str13, conn);
                //OleDbDataReader reader1 = comm1.ExecuteReader();
                SqlDataReader reader2 = comm2.ExecuteReader();
                SqlDataReader reader3 = comm3.ExecuteReader();
                SqlDataReader reader4 = comm4.ExecuteReader();
                SqlDataReader reader5 = comm5.ExecuteReader();
                SqlDataReader reader6 = comm6.ExecuteReader();
                //OleDbDataReader reader7 = comm7.ExecuteReader();
                SqlDataReader reader8 = comm8.ExecuteReader();
                //OleDbDataReader reader9 = comm9.ExecuteReader();
                SqlDataReader reader10 = comm10.ExecuteReader();
                SqlDataReader reader11 = comm11.ExecuteReader();
                SqlDataReader reader12 = comm12.ExecuteReader();
                SqlDataReader reader13 = comm13.ExecuteReader();
                //b1 = reader1.HasRows == true ? true : false;
                b2 = reader2.HasRows == true ? true : false;
                b3 = reader3.HasRows == true ? true : false;
                b4 = reader4.HasRows == true ? true : false;
                b5 = reader5.HasRows == true ? true : false;
                b6 = reader6.HasRows == true ? true : false;
                //b7 = reader7.HasRows == true ? true : false;
                b8 = reader8.HasRows == true ? true : false;
                //b9 = reader9.HasRows == true ? true : false;
                b10 = reader10.HasRows == true ? true : false;
                b11 = reader11.HasRows == true ? true : false;
                b12 = reader12.HasRows == true ? true : false;
                b13 = reader13.HasRows == true ? true : false;
                if (b2 && b3 && b4 && b5 && b6 && b8 && b10 && b11 && b12 && b13)
                { A5Btn.Enabled = true; }
                else
                { A5Btn.Enabled = false; }

                //reader1.Dispose();
                reader2.Dispose();
                reader3.Dispose();
                reader4.Dispose();
                reader5.Dispose();
                reader6.Dispose();
                //reader7.Dispose();
                reader8.Dispose();
                //reader9.Dispose();
                reader10.Dispose();
                reader11.Dispose();
                reader12.Dispose();
                reader13.Dispose();
                //comm1.Dispose();
                comm2.Dispose();
                comm3.Dispose();
                comm4.Dispose();
                comm5.Dispose();
                comm6.Dispose();
                //comm7.Dispose();
                comm8.Dispose();
                //comm9.Dispose();
                comm10.Dispose();
                comm11.Dispose();
                comm12.Dispose();
                comm13.Dispose();
            }



        }

        public void otherBtnInit(bool b)
        {
            A1Btn.Enabled = b;
            A4Btn.Enabled = b;
            B2Btn.Enabled = b;
            B3Btn.Enabled = b;
            B4Btn.Enabled = b;
            B5Btn.Enabled = b;
            B6Btn.Enabled = b;
            B7Btn.Enabled = b;
            B8Btn.Enabled = b;
            B9Btn.Enabled = b;
            C3Btn.Enabled = b;
            C4Btn.Enabled = b;
            D1Btn.Enabled = b;
            D2Btn.Enabled = b;
            D3Btn.Enabled = b;
            D4Btn.Enabled = b;
        }

        #endregion

        #region 下拉框生产指令 

        //下拉框获取生产指令
        public void comboInit()
        {
            HashSet<String> hash = new HashSet<String>();
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from 生产指令信息表 where 状态 = 2 ";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        //comboBox1.Items.Add(reader["生产指令编号"]);
                        hash.Add(reader["生产指令编号"].ToString());
                    }
                    foreach (String code in hash)
                    {
                        comboBox1.Items.Add(code);
                    }

                }
                comm.Dispose();
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select * from 生产指令信息表 where 状态 = 2 ";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        //comboBox1.Items.Add(reader["生产指令编号"]);
                        hash.Add(reader["生产指令编号"].ToString());
                    }
                    foreach (String code in hash)
                    {
                        comboBox1.Items.Add(code);
                    }

                }
                comm.Dispose();
            }
            //默认下拉框选最后一个
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                Parameter.proInstruction = comboBox1.SelectedItem.ToString();
                String tblName = "生产指令信息表";
                List<String> queryCols = new List<String>(new String[] { "ID" });
                List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
                List<Object> whereVals = new List<Object>(new Object[] { instruction });
                List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                Parameter.proInstruID = Convert.ToInt32(res[0][0]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                instruction = comboBox1.SelectedItem.ToString();
                Parameter.proInstruction = instruction;
                String tblName = "生产指令信息表";
                List<String> queryCols = new List<String>(new String[] { "ID" });
                List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
                List<Object> whereVals = new List<Object>(new Object[] { instruction });
                List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                instruID = Convert.ToInt32(res[0][0]);
                //instruID = Convert.ToInt32(res[res.Count-1][0]);
                Parameter.proInstruID = instruID;
                InitBtn();

                CheckHour(); //立即执行一次 
                //定时器开始计时

                //TODO: 时间间隔设置为参数
                int interval = 300000; //毫秒
                timer1.Interval = interval; //五分钟
                timer1.Start();
            }
            else
            {
                instruction = comboBox1.SelectedItem.ToString();
                Parameter.proInstruction = instruction;
                String tblName = "生产指令信息表";
                List<String> queryCols = new List<String>(new String[] { "ID" });
                List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
                List<Object> whereVals = new List<Object>(new Object[] { instruction });
                List<List<Object>> res = Utility.selectAccess(Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                instruID = Convert.ToInt32(res[0][0]);
                //instruID = Convert.ToInt32(res[res.Count-1][0]);
                Parameter.proInstruID = instruID;
                InitBtn();

                CheckHour(); //立即执行一次 
                //定时器开始计时

                //TODO: 时间间隔设置为参数
                int interval = 300000; //毫秒
                timer1.Interval = interval; //五分钟
                timer1.Start();
            }

        }

        #endregion

        #region 定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckHour();
        }
        
        //定时器调用的函数，判断时间，查看是否填写
        private void CheckHour()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                DateTime now = DateTime.Now;
                //DateTime now = new DateTime(2017, 7, 28, 17, 57, 00); //测试用
                DateTime preheattime;
                //获取开机时间
                String table = "吹膜机组预热参数记录表";
                List<String> queryCols = new List<String>(new String[] { "保温结束时间3" });
                List<String> whereCols = new List<String>(new String[] { "生产指令id" });
                List<Object> whereVals = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res = Utility.selectAccess(Parameter.connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                if (res.Count == 0)
                {
                    return;
                }
                else
                {
                    preheattime = Convert.ToDateTime(res[0][0]); //开机时间
                    TimeSpan delt = now - preheattime;
                    double duration = delt.TotalMinutes % 120; //余数
                    if (delt.TotalMinutes < 110) //刚开机不到两小时
                    {
                        return;
                    }
                    else
                    {
                        if (110 <= duration && duration < 120) //差十分钟
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 5));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);

                            OleDbCommand comm1 = new OleDbCommand();
                            comm1.Connection = Parameter.connOle;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 = " + "#" + now.Date + "#";
                            OleDbDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;

                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID   
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                OleDbCommand commInsert = new OleDbCommand();
                                commInsert.Connection = Parameter.connOle;
                                //String mySql = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES ('";
                                //mySql += Parameter.proInstruction + "', ";
                                //mySql += Parameter.proInstruID + ", #";
                                //mySql += now.Date + "#, '";
                                //mySql += Parameter.userflight + "', ";
                                //mySql += "'');";
                                //commInsert.CommandText = mySql;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", #" + now.Date + "#, '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);

                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            OleDbCommand comm11 = new OleDbCommand();
                            comm11.Connection = Parameter.connOle;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            OleDbCommand comm2 = new OleDbCommand();
                            comm2.Connection = Parameter.connOle;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); //提示框显示10s
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                return;
                            }
                        }
                        else if (0 <= duration && duration < 5) //超时五分钟
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);

                            OleDbCommand comm1 = new OleDbCommand();
                            comm1.Connection = Parameter.connOle;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 = " + "#" + now.Date + "#";
                            OleDbDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;
                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                OleDbCommand commInsert = new OleDbCommand();
                                commInsert.Connection = Parameter.connOle;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", #" + now.Date + "#, '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);
                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            OleDbCommand comm11 = new OleDbCommand();
                            comm11.Connection = Parameter.connOle;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            OleDbCommand comm2 = new OleDbCommand();
                            comm2.Connection = Parameter.connOle;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                return;
                            }
                        }
                        else if (5 <= duration && duration < 10) //超时十分钟，弹出
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                            OleDbCommand comm1 = new OleDbCommand();
                            comm1.Connection = Parameter.connOle;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 = " + "#" + now.Date + "#";
                            OleDbDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;
                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                OleDbCommand commInsert = new OleDbCommand();
                                commInsert.Connection = Parameter.connOle;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", #" + now.Date + "#, '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);
                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            OleDbCommand comm11 = new OleDbCommand();
                            comm11.Connection = Parameter.connOle;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            OleDbCommand comm2 = new OleDbCommand();
                            comm2.Connection = Parameter.connOle;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            OleDbDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜供料系统运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
                                if (a)
                                {
                                    Process.Extruction.C.Feed form16 = new Process.Extruction.C.Feed(mainform);
                                    form16.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜供料系统运行记录”"); }
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜机组运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
                                if (a)
                                {
                                    Process.Extruction.B.Running form17 = new Process.Extruction.B.Running(mainform);
                                    form17.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜机组运行记录”"); }
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
                                if (a)
                                {
                                    Process.Extruction.C.Feed form16 = new Process.Extruction.C.Feed(mainform);
                                    form16.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜供料系统运行记录”"); }

                                Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
                                if (b)
                                {
                                    Process.Extruction.B.Running form17 = new Process.Extruction.B.Running(mainform);
                                    form17.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜机组运行记录”"); }
                                return;
                            }
                        }
                        else //未到时间
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                //DateTime now = new DateTime(2017, 7, 28, 17, 57, 00); //测试用
                DateTime preheattime;
                //获取开机时间
                String table = "吹膜机组预热参数记录表";
                List<String> queryCols = new List<String>(new String[] { "保温结束时间3" });
                List<String> whereCols = new List<String>(new String[] { "生产指令id" });
                List<Object> whereVals = new List<Object>(new Object[] { Parameter.proInstruID });
                List<List<Object>> res = Utility.selectAccess(Parameter.conn, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                if (res.Count == 0)
                {
                    return;
                }
                else
                {
                    preheattime = Convert.ToDateTime(res[0][0]); //开机时间
                    TimeSpan delt = now - preheattime;
                    double duration = delt.TotalMinutes % 120; //余数
                    if (delt.TotalMinutes < 110) //刚开机不到两小时
                    {
                        return;
                    }
                    else
                    {
                        if (110 <= duration && duration < 120) //差十分钟
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 5));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);

                            SqlCommand comm1 = new SqlCommand();
                            comm1.Connection = Parameter.conn;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 like '" + "#" + now.Date.ToShortDateString() + "#'";
                            SqlDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;

                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID   
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                SqlCommand commInsert = new SqlCommand();
                                commInsert.Connection = Parameter.conn;
                                //String mySql = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES ('";
                                //mySql += Parameter.proInstruction + "', ";
                                //mySql += Parameter.proInstruID + ", #";
                                //mySql += now.Date + "#, '";
                                //mySql += Parameter.userflight + "', ";
                                //mySql += "'');";
                                //commInsert.CommandText = mySql;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", '" + now.Date + "', '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);

                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            SqlCommand comm11 = new SqlCommand();
                            comm11.Connection = Parameter.conn;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            SqlCommand comm2 = new SqlCommand();
                            comm2.Connection = Parameter.conn;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); //提示框显示10s
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                return;
                            }
                        }
                        else if (0 <= duration && duration < 5) //超时五分钟
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);

                            SqlCommand comm1 = new SqlCommand();
                            comm1.Connection = Parameter.conn;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 like " + "%" + now.Date.ToShortDateString() + "%";
                            SqlDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;
                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                SqlCommand commInsert = new SqlCommand();
                                commInsert.Connection = Parameter.conn;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", #" + now.Date + "#, '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);
                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            SqlCommand comm11 = new SqlCommand();
                            comm11.Connection = Parameter.conn;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            SqlCommand comm2 = new SqlCommand();
                            comm2.Connection = Parameter.conn;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                return;
                            }
                        }
                        else if (5 <= duration && duration < 10) //超时十分钟，弹出
                        {
                            //检查是否填写
                            String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                            DateTime right1 = now;
                            DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                            DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                            DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                            SqlCommand comm1 = new SqlCommand();
                            comm1.Connection = Parameter.conn;
                            comm1.CommandText = "select * from " + table1 + " where 生产指令ID = " + Parameter.proInstruID + " and 生产日期 like '" + "#" + now.Date.ToShortDateString() + "#'";
                            SqlDataReader reader1 = comm1.ExecuteReader();//执行查询
                            int instruID = -1;
                            if (reader1.Read())
                            {
                                instruID = Convert.ToInt32(reader1["ID"]); //获取大表ID
                            }
                            else
                            {
                                //若大表当日无记录则新建一条
                                SqlCommand commInsert = new SqlCommand();
                                commInsert.Connection = Parameter.conn;
                                commInsert.CommandText = "INSERT INTO 吹膜供料系统运行记录 (生产指令编号, 生产指令ID, 生产日期, 班次, 审核员) VALUES " + "('" + Parameter.proInstruction + "', " + Parameter.proInstruID + ", '" + now.Date + "', '" + Parameter.userflight + "', " + "''" + ");";
                                commInsert.ExecuteNonQuery();
                                //获取ID
                                commInsert.CommandText = "SELECT @@IDENTITY";
                                object n = commInsert.ExecuteScalar();
                                instruID = Convert.ToInt32(n);
                            }

                            String table11 = "吹膜供料系统运行记录详细信息";
                            SqlCommand comm11 = new SqlCommand();
                            comm11.Connection = Parameter.conn;
                            comm11.CommandText = "select * from " + table11 + " where T吹膜供料系统运行记录ID = " + instruID + " and 检查时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader11 = comm11.ExecuteReader();

                            String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                            SqlCommand comm2 = new SqlCommand();
                            comm2.Connection = Parameter.conn;
                            comm2.CommandText = "select * from " + table2 + " where 生产指令ID = " + Parameter.proInstruID + " and 记录时间 between " + "#" + left11 + "#" + " and " + "#" + right11 + "#";
                            SqlDataReader reader2 = comm2.ExecuteReader();

                            if (reader11.HasRows && reader2.HasRows)
                            { return; }
                            else if (!reader11.HasRows && reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请尽快填写 “吹膜供料系统运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜供料系统运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
                                if (a)
                                {
                                    Process.Extruction.C.Feed form16 = new Process.Extruction.C.Feed(mainform);
                                    form16.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜供料系统运行记录”"); }
                                return;
                            }
                            else if (reader11.HasRows && !reader2.HasRows)
                            {
                                //taskbarNotifier1.Show("提示", "请填写 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜机组运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
                                if (a)
                                {
                                    Process.Extruction.B.Running form17 = new Process.Extruction.B.Running(mainform);
                                    form17.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜机组运行记录”"); }
                                return;
                            }
                            else
                            {
                                //taskbarNotifier1.Show("提示", "请填写 “吹膜供料系统运行记录” 和 “吹膜机组运行记录”！", 500, 10000, 500); 
                                MessageBox.Show("请填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                                Boolean a = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
                                if (a)
                                {
                                    Process.Extruction.C.Feed form16 = new Process.Extruction.C.Feed(mainform);
                                    form16.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜供料系统运行记录”"); }

                                Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
                                if (b)
                                {
                                    Process.Extruction.B.Running form17 = new Process.Extruction.B.Running(mainform);
                                    form17.ShowDialog();
                                }
                                else
                                { MessageBox.Show("您无权填写“吹膜机组运行记录”"); }
                                return;
                            }
                        }
                        else //未到时间
                        {
                            return;
                        }
                    }
                }
            }
            
        }

        #endregion

        #region 按钮new窗口
        private void A1Btn_Click(object sender, EventArgs e)
        {
            BatchProductRecord.BatchProductRecord form1 = new BatchProductRecord.BatchProductRecord(mainform);
            form1.Owner = this;
            form1.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜工序生产指令");
            b = true;
            if (b)
            {
                BatchProductRecord.ProcessProductInstru form2 = new BatchProductRecord.ProcessProductInstru(mainform);
                form2.Owner = this;
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组清洁记录");
            if (b)
            {
                Record_extrusClean form3 = new Record_extrusClean(mainform);
                form3.Owner = this;
                form3.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }             
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜工序清场记录");
            if (b)
            {
                Record_extrusSiteClean form4 = new Record_extrusSiteClean(mainform);
                form4.Owner = this;
                form4.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜岗位交接班记录");
            if (b)
            {
                mySystem.Process.Extruction.A.HandOver form5 = new mySystem.Process.Extruction.A.HandOver(mainform);
                form5.Owner = this;
                form5.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料记录");
            if (b)
            {
                Record_extrusSupply form6 = new Record_extrusSupply(mainform);
                form6.Owner = this;
                form6.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }     
            
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜工序废品记录");
            if (b)
            {
                mySystem.Process.Extruction.B.Waste form7 = new mySystem.Process.Extruction.B.Waste(mainform);
                form7.Owner = this;
                form7.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜工序领料退料记录");
            if (b)
            {
                Record_material_reqanddisg form8 = new Record_material_reqanddisg(mainform);
                form8.Owner = this;
                form8.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        //日报表，谁都可以看
        private void B5Btn_Click(object sender, EventArgs e)
        {
            ProdctDaily_extrus form9 = new ProdctDaily_extrus(mainform);
            form9.Owner = this;
            form9.ShowDialog();
                              
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜生产和检验记录表");
            if (b)
            {
                ExtructionpRoductionAndRestRecordStep6 form10 = new ExtructionpRoductionAndRestRecordStep6(mainform);
                form10.Owner = this;
                form10.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void B7Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜工序物料平衡记录");
            if (b)
            {
                MaterialBalenceofExtrusionProcess form11 = new MaterialBalenceofExtrusionProcess(mainform);
                form11.Owner = this;
                form11.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B8Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜产品内包装记录表");
            if (b)
            {
                ProductInnerPackagingRecord form12 = new ProductInnerPackagingRecord(mainform);
                form12.Owner = this;
                form12.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B9Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜产品外包装记录表");
            if (b)
            {
                Extruction.Chart.outerpack form13 = new Extruction.Chart.outerpack(mainform);
                form13.Owner = this;
                form13.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void C1Btn_Click(object sender, EventArgs e)
        {            
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜开机前确认表");
            if (b)
            {
                ExtructionCheckBeforePowerStep2 form14 = new ExtructionCheckBeforePowerStep2(mainform);
                form14.Owner = this;
                form14.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }             
        }

        private void C2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜预热参数记录表");
            if (b)
            {
                ExtructionPreheatParameterRecordStep3 form15 = new ExtructionPreheatParameterRecordStep3(mainform);
                form15.Owner = this;
                form15.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }     
            
        }

        private void C3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
            if (b)
            {
                mySystem.Process.Extruction.C.Feed form16 = new mySystem.Process.Extruction.C.Feed(mainform);
                form16.Owner = this;
                form16.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void C4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
            if (b)
            {
                mySystem.Process.Extruction.B.Running form17 = new mySystem.Process.Extruction.B.Running(mainform);
                form17.Owner = this;
                form17.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }  
            
        }

        private void D1Btn_Click(object sender, EventArgs e)
        {
            Record_train form18 = new Record_train(mainform);
            form18.Owner = this;
            form18.ShowDialog();
        }

        private void D2Btn_Click(object sender, EventArgs e)
        {
            ReplaceHeadForm form19 = new ReplaceHeadForm(mainform);
            form19.Owner = this;
            form19.ShowDialog();
        }

        private void D3Btn_Click(object sender, EventArgs e)
        {
            ExtructionReplaceCore form20 = new ExtructionReplaceCore(mainform);
            form20.Owner = this;
            form20.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            ExtructionTransportRecordStep4 form22 = new ExtructionTransportRecordStep4(mainform);
            form22.Owner = this;
            form22.ShowDialog();
        }

        private void LabelBtn_Click(object sender, EventArgs e)
        {
            LabelPrint label = new LabelPrint();
            label.ShowDialog();
        }

        private void D4Btn_Click(object sender, EventArgs e)
        {
            Process.Extruction.D.NetExchange form24 = new Process.Extruction.D.NetExchange(mainform);
            form24.Owner = this;
            form24.ShowDialog();
        }

        #endregion

        //判断是否能查看
        private Boolean checkUser(String user, int role, String tblName)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                Boolean b = false;
                String[] name操作员 = null;
                String[] name审核员 = null;
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from 用户权限 where 步骤 = " + "'" + tblName + "' ";
                OleDbDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    name操作员 = reader["操作员"].ToString().Split("，,".ToCharArray());
                    name审核员 = reader["审核员"].ToString().Split("，,".ToCharArray());
                }

                if (role == 3)
                {
                    return b = true;
                }
                else
                {
                    foreach (String name in name操作员)
                    {
                        if (user == name)
                        { return b = true; }
                    }
                    foreach (String name in name审核员)
                    {
                        if (user == name)
                        { return b = true; }
                    }

                }
                return b = false;
            }
            else
            {
                Boolean b = false;
                String[] name操作员 = null;
                String[] name审核员 = null;
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select * from 用户权限 where 步骤 = " + "'" + tblName + "' ";
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    name操作员 = reader["操作员"].ToString().Split("，,".ToCharArray());
                    name审核员 = reader["审核员"].ToString().Split("，,".ToCharArray());
                }

                if (role == 3)
                {
                    return b = true;
                }
                else
                {
                    foreach (String name in name操作员)
                    {
                        if (user == name)
                        { return b = true; }
                    }
                    foreach (String name in name审核员)
                    {
                        if (user == name)
                        { return b = true; }
                    }

                }
                return b = false;
            }
        }

        //右下角提示框状态初始化
        private void InitTaskBar()
        {
            taskbarNotifier1 = new TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(new Bitmap(Image.FromFile(@"../../pic/skin_logo.bmp")), Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(new Bitmap(Image.FromFile(@"../../pic/close_logo.bmp")), Color.FromArgb(255, 0, 255), new Point(190, 12));
            taskbarNotifier1.TitleRectangle = new Rectangle(65, 25, 135, 60);
            taskbarNotifier1.ContentRectangle = new Rectangle(15, 65, 205, 150);
            taskbarNotifier1.CloseClickable = true;
            taskbarNotifier1.TitleClickable = false;
            taskbarNotifier1.ContentClickable = false;
            taskbarNotifier1.EnableSelectionRectangle = false;
            taskbarNotifier1.KeepVisibleOnMousOver = true;
            taskbarNotifier1.ReShowOnMouseOver = true;
        }
        
    }
}



