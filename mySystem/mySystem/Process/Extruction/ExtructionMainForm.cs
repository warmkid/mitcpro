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


namespace mySystem
{
    public partial class ExtructionMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;
        System.Timers.Timer timer = new System.Timers.Timer();//实例化Timer类

        public ExtructionMainForm(MainForm mainform): base(mainform)
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            InitBtn();
        }

        //初始化按钮状态
        public void InitBtn()
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
                C1Btn.Enabled = true;
                A3Btn.Enabled = false;
                C2Btn.Enabled = false;
                cleanBtnInit(); //判断清场按钮是否可点
                otherBtnInit(false);
                if (checkBeforePower)
                {
                    A3Btn.Enabled = true;
                    if (extrusClean)
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

        //下拉框获取生产指令
        public void comboInit()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select ID,生产指令编号 from 生产指令信息表 where 状态 = 2 ";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }
                comm.Dispose();
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select ID,生产指令编号 from 生产指令信息表 where 状态 = 2 ";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }
                comm.Dispose();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.proInstruction = instruction;
            String tblName = "生产指令信息表";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.proInstruID = instruID;
            InitBtn();
  
            //CheckHour(); //立即执行一次 
            //定时器开始计时
            timer1.Interval = 300000; //五分钟
            timer1.Start();

        }
        int num = Parameter.i;
        void test()
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
            Parameter.i = num;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //test();
            //CheckHour();
        }
        
        //定时器调用的函数，判断时间，查看是否填写
        private void CheckHour()
        {
            DateTime now = DateTime.Now;
            //DateTime now = new DateTime(2017, 7, 4, 18, 14, 30); //测试用，可以跳出界面的时间
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
                        List<String> queryCols1 = new List<String>(new String[] { "ID" });
                        List<String> whereCols1 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals1 = new List<Object>(new Object[] { Convert.ToInt32(Parameter.proInstruID) });
                        String betweenCol1 = "检查时间";
                        DateTime right1 = now;
                        DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 5));
                        DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                        DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                        List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left11, right11);

                        String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                        List<String> queryCols2 = new List<String>(new String[] { "ID" });
                        List<String> whereCols2 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                        String betweenCol2 = "记录时间";
                        DateTime right2 = now;
                        DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120 + 5));
                        DateTime right21 = new DateTime(right2.Year, right2.Month, right2.Day, right2.Hour, right2.Minute, right2.Second);
                        DateTime left21 = new DateTime(left2.Year, left2.Month, left2.Day, left2.Hour, left2.Minute, left2.Second);
                        List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left21, right21);

                        if (res1.Count != 0 && res2.Count != 0)
                        {
                            return;
                        }
                        else if (res1.Count == 0 && res2.Count != 0)
                        {
                            MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                            return;
                        }
                        else if (res1.Count != 0 && res2.Count == 0)
                        {
                            MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                            return;
                        }
                    }
                    else if (0 <= duration && duration < 5) //超时五分钟
                    {
                        //检查是否填写
                        String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                        List<String> queryCols1 = new List<String>(new String[] { "ID" });
                        List<String> whereCols1 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals1 = new List<Object>(new Object[] { Convert.ToInt32(Parameter.proInstruID) });
                        String betweenCol1 = "检查时间";
                        DateTime right1 = now;
                        DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                        DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                        DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                        List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left11, right11);

                        String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                        List<String> queryCols2 = new List<String>(new String[] { "ID" });
                        List<String> whereCols2 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                        String betweenCol2 = "记录时间";
                        DateTime right2 = now;
                        DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                        DateTime right21 = new DateTime(right2.Year, right2.Month, right2.Day, right2.Hour, right2.Minute, right2.Second);
                        DateTime left21 = new DateTime(left2.Year, left2.Month, left2.Day, left2.Hour, left2.Minute, left2.Second);
                        List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left21, right21);

                        if (res1.Count != 0 && res2.Count != 0)
                        {
                            return;
                        }
                        else if (res1.Count == 0 && res2.Count != 0)
                        {
                            MessageBox.Show("请尽快填写“吹膜供料系统运行记录”！", "警告");
                            return;
                        }
                        else if (res1.Count != 0 && res2.Count == 0)
                        {
                            MessageBox.Show("请尽快填写“吹膜机组运行记录”！", "警告");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("请尽快填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                            return;
                        }
                    }
                    else if (5 <= duration && duration < 10) //超时十分钟，弹出
                    {
                        //检查是否填写
                        String table1 = "吹膜供料系统运行记录"; //吹膜供料系统运行记录
                        List<String> queryCols1 = new List<String>(new String[] { "ID" });
                        List<String> whereCols1 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals1 = new List<Object>(new Object[] { Convert.ToInt32(Parameter.proInstruID) });
                        String betweenCol1 = "检查时间";
                        DateTime right1 = now;
                        DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                        DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                        DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                        List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left11, right11);

                        String table2 = "吹膜机组运行记录"; //吹膜机组运行记录
                        List<String> queryCols2 = new List<String>(new String[] { "ID" });
                        List<String> whereCols2 = new List<String>(new String[] { "生产指令ID" });
                        List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                        String betweenCol2 = "记录时间";
                        DateTime right2 = now;
                        DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120 + 125));
                        DateTime right21 = new DateTime(right2.Year, right2.Month, right2.Day, right2.Hour, right2.Minute, right2.Second);
                        DateTime left21 = new DateTime(left2.Year, left2.Month, left2.Day, left2.Hour, left2.Minute, left2.Second);
                        List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left21, right21);

                        if (res1.Count != 0 && res2.Count != 0)
                        {
                            return;
                        }
                        else if (res1.Count == 0 && res2.Count != 0)
                        {
                            MessageBox.Show("请填写“吹膜供料系统运行记录”！", "警告");
                            form16.ShowDialog();
                            return;
                        }
                        else if (res1.Count != 0 && res2.Count == 0)
                        {
                            MessageBox.Show("请填写“吹膜机组运行记录”！", "警告");
                            form17.ShowDialog();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("请填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                            form16 = new Process.Extruction.C.Feed(mainform);
                            form16.ShowDialog();
                            form17 = new Process.Extruction.B.Running(mainform);
                            form17.ShowDialog();
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


        //定义各窗体变量
        BatchProductRecord.BatchProductRecord form1 = null;
        BatchProductRecord.ProcessProductInstru form2 = null;
        Record_extrusClean form3 = null;
        Record_extrusSiteClean form4 = null;
        mySystem.Process.Extruction.A.HandOver form5 = null;
        Record_extrusSupply form6 = null;
        mySystem.Process.Extruction.B.Waste form7 = null;
        Record_material_reqanddisg form8 = null;
        ProdctDaily_extrus form9 = null;
        ExtructionpRoductionAndRestRecordStep6 form10 = null;
        MaterialBalenceofExtrusionProcess form11 = null;
        ProductInnerPackagingRecord form12 = null;
        mySystem.Extruction.Chart.outerpack form13 = null;
        ExtructionCheckBeforePowerStep2 form14 = null;
        ExtructionPreheatParameterRecordStep3 form15 = null;
        new mySystem.Process.Extruction.C.Feed form16 = null;
        mySystem.Process.Extruction.B.Running form17 = null;
        Record_train form18 = null;
        ReplaceHeadForm form19 = null;
        ExtructionReplaceCore form20 = null;
        mySystem.Process.Extruction.D.NetExchange form24 = null;
        
        
        ExtructionTransportRecordStep4 form22 = null;

        private void A1Btn_Click(object sender, EventArgs e)
        {
            form1 = new BatchProductRecord.BatchProductRecord(mainform);
            form1.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            form2 = new BatchProductRecord.ProcessProductInstru(mainform);
            form2.ShowDialog();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            form3 = new Record_extrusClean(mainform);
            form3.ShowDialog();
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            form4 = new Record_extrusSiteClean(mainform);
            form4.ShowDialog();
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            form5 = new mySystem.Process.Extruction.A.HandOver(mainform);
            form5.ShowDialog();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            form6 = new Record_extrusSupply(mainform);
            form6.ShowDialog();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            form7 = new mySystem.Process.Extruction.B.Waste(mainform);
            form7.ShowDialog();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            form8 = new Record_material_reqanddisg(mainform);
            form8.ShowDialog();
        }

        private void B5Btn_Click(object sender, EventArgs e)
        {
            form9 = new ProdctDaily_extrus(mainform);
            form9.ShowDialog();
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            form10 = new ExtructionpRoductionAndRestRecordStep6(mainform);
            form10.ShowDialog();
        }

        private void B7Btn_Click(object sender, EventArgs e)
        {
            form11 = new MaterialBalenceofExtrusionProcess(mainform);
            form11.ShowDialog();
        }

        private void B8Btn_Click(object sender, EventArgs e)
        {
            form12 = new ProductInnerPackagingRecord(mainform);
            form12.ShowDialog();
        }

        private void B9Btn_Click(object sender, EventArgs e)
        {
            form13 = new Extruction.Chart.outerpack(mainform);
            form13.ShowDialog();
        }

        private void C1Btn_Click(object sender, EventArgs e)
        {
            form14 = new ExtructionCheckBeforePowerStep2(mainform);
            form14.ShowDialog();
        }

        private void C2Btn_Click(object sender, EventArgs e)
        {
            form15 = new ExtructionPreheatParameterRecordStep3(mainform);
            form15.ShowDialog();
        }

        private void C3Btn_Click(object sender, EventArgs e)
        {
            form16 = new mySystem.Process.Extruction.C.Feed(mainform);
            form16.ShowDialog();
        }

        private void C4Btn_Click(object sender, EventArgs e)
        {
            form17 = new mySystem.Process.Extruction.B.Running(mainform);
            form17.ShowDialog();
        }

        private void D1Btn_Click(object sender, EventArgs e)
        {
            form18 = new Record_train(mainform);
            form18.ShowDialog();
        }

        private void D2Btn_Click(object sender, EventArgs e)
        {
            form19 = new ReplaceHeadForm();
            form19.ShowDialog();
        }

        private void D3Btn_Click(object sender, EventArgs e)
        {
            form20 = new ExtructionReplaceCore(mainform);
            form20.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            form22 = new ExtructionTransportRecordStep4(mainform);
            form22.ShowDialog();
        }

        private void LabelBtn_Click(object sender, EventArgs e)
        {
            LabelPrint label = new LabelPrint();
            label.ShowDialog();
        }

        private void D4Btn_Click(object sender, EventArgs e)
        {
            form24 = new Process.Extruction.D.NetExchange(mainform);
            form24.ShowDialog();
        }

        

        
    }
}



