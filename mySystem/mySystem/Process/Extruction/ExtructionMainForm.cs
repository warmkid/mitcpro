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
            CheckForIllegalCrossThreadCalls = false; //新创建的线程可以访问UI线程创建的窗口控件
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
            String tblName1 = "extrusion_s2_confirm";
            List<String> queryCols1 = new List<String>(new String[] { "s2_reviewer_id" });
            List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
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
            String tblName2 = "extrusion_s1_cleanrecord";
            List<String> queryCols2 = new List<String>(new String[] { "s1_reviewer_id" });
            List<String> whereCols2 = new List<String>(new String[] { "production_instruction" });
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
            String tblName3 = "extrusion_s3_preheat";
            List<String> queryCols3 = new List<String>(new String[] { "s3_reviewer_id" });
            List<String> whereCols3 = new List<String>(new String[] { "production_instruction_id" });
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
                otherBtnInit(false);
            }
            else
            {
                C1Btn.Enabled = true;
                A3Btn.Enabled = false;
                C2Btn.Enabled = false;
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

        private void otherBtnInit(bool b)
        {
            A1Btn.Enabled = b;
            A4Btn.Enabled = b;
            A5Btn.Enabled = b;
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
        private void comboInit()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select production_instruction_code from production_instruction";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["production_instruction_code"]);
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select production_instruction_code from production_instruction";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["production_instruction_code"]);
                    }
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.proInstruction = instruction;
            String tblName = "production_instruction";
            List<String> queryCols = new List<String>(new String[] { "production_instruction_id" });
            List<String> whereCols = new List<String>(new String[] { "production_instruction_code" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.proInstruID = instruID;
            InitBtn();
            ////定时器开始计时   
            ////CheckHour();
            ////timer.Stop();
            //timer.Start();
            //timer.Interval = 1000;
            ////timer.Enabled = true;
            ////timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckHour);
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(test);
            
            timer1.Interval = 1000;
            timer1.Start();

        }
        int num = Parameter.i;
        void test(object sender, System.Timers.ElapsedEventArgs e)
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
            Parameter.i = num;
        }

        void test()
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
            Parameter.i = num;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //test();
        }

        //定时器调用的函数，判断时间，查看是否填写
        private void CheckHour(object sender, System.Timers.ElapsedEventArgs e)
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
            Parameter.i = num;
            DateTime now = DateTime.Now;
            DateTime preheattime;
            //获取开机时间
            String table = "extrusion_s3_preheat";
            List<String> queryCols = new List<String>(new String[] { "s3_end_insulation_time_2" });
            List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
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
                int duration = Convert.ToInt32(delt.TotalMinutes) % 120; //余数
                if (110 <= duration && duration < 120) //差十分钟
                {
                    //检查是否填写
                    String table1 = "running_record_of_feeding_unit"; //吹膜供料系统运行记录
                    List<String> queryCols1 = new List<String>(new String[] { "id" });
                    List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals1 = new List<Object>(new Object[] { Convert.ToInt32(Parameter.proInstruID) });
                    String betweenCol1 = "modifytime";
                    DateTime right1 = now;
                    DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120));
                    DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                    DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                    List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left11, right11);

                    String table2 = "running_record_of_extrusion_unit"; //吹膜机组运行记录
                    List<String> queryCols2 = new List<String>(new String[] { "id" });
                    List<String> whereCols2 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol2 = "modifytime";
                    DateTime right2 = now;
                    DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120));
                    DateTime right21 = new DateTime(right2.Year, right2.Month, right2.Day, right2.Hour, right2.Minute, right2.Second);
                    DateTime left21 = new DateTime(left2.Year, left2.Month, left2.Day, left2.Hour, left2.Minute, left2.Second);
                    List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left21, right21);

                    if (res1.Count != 0 && res2.Count != 0)
                    {
                        return;
                    }
                    else if (res1.Count == 0 && res2.Count != 0)
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜供料系统运行记录”！", "警告");
                        return;
                    }
                    else if (res1.Count != 0 && res2.Count == 0)
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜机组运行记录”！", "警告");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                        return;
                    }

                }
                else if (0 <= duration && duration < 1) //到了两小时
                {
                    //检查是否填写
                    String table1 = "runnig_record_of_feeding_unit"; //吹膜供料系统运行记录
                    List<String> queryCols1 = new List<String>(new String[] { "id" });
                    List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals1 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol1 = "modifytime";
                    DateTime right1 = now;
                    DateTime left1 = right1.AddMinutes(-120);
                    List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left1, right1);
                    String table2 = "running_record_of_extrusion_unit"; //吹膜机组运行记录
                    List<String> queryCols2 = new List<String>(new String[] { "id" });
                    List<String> whereCols2 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol2 = "modifytime";
                    DateTime right2 = now;
                    DateTime left2 = right2.AddMinutes(-120);
                    List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left2, right2);

                    if (res1.Count != 0 && res2.Count != 0)
                    {
                        return;
                    }
                    else
                    {
                        mainform.Hide();
                        if (form1 != null)
                        { form1.Hide(); }
                        if (form2 != null)
                        { form2.Hide(); }
                        if (form3 != null)
                        { form3.Hide(); }
                        if (form4 != null)
                        { form4.Hide(); }
                        if (form5 != null)
                        { form5.Hide(); }
                        if (form6 != null)
                        { form6.Hide(); }
                        if (form7 != null)
                        { form7.Hide(); }
                        if (form8 != null)
                        { form8.Hide(); }
                        if (form9 != null)
                        { form9.Hide(); }
                        if (form10 != null)
                        { form10.Hide(); }
                        if (form11 != null)
                        { form11.Hide(); }
                        if (form12 != null)
                        { form12.Hide(); }
                        if (form13 != null)
                        { form13.Hide(); }
                        if (form14 != null)
                        { form14.Hide(); }
                        if (form15 != null)
                        { form15.Hide(); }
                        if (form16 != null)
                        { form16.Hide(); }
                        if (form17 != null)
                        { form17.Hide(); }
                        if (form18 != null)
                        { form18.Hide(); }
                        if (form19 != null)
                        { form19.Hide(); }
                        if (form20 != null)
                        { form20.Hide(); }

                        if (form22 != null)
                        { form22.Hide(); }

                        CheckForm checkform = new CheckForm(this);
                        checkform.CancelBtn.Enabled = false;
                        checkform.ShowDialog();
                        mainform.Show();
                        if (form1 != null)
                        { form1.Show(); }
                        if (form2 != null)
                        { form2.Show(); }
                        if (form3 != null)
                        { form3.Show(); }
                        if (form4 != null)
                        { form4.Show(); }
                        if (form5 != null)
                        { form5.Show(); }
                        if (form6 != null)
                        { form6.Show(); }
                        if (form7 != null)
                        { form7.Show(); }
                        if (form8 != null)
                        { form8.Show(); }
                        if (form9 != null)
                        { form9.Show(); }
                        if (form10 != null)
                        { form10.Show(); }
                        if (form11 != null)
                        { form11.Show(); }
                        if (form12 != null)
                        { form12.Show(); }
                        if (form13 != null)
                        { form13.Show(); }
                        if (form14 != null)
                        { form14.Show(); }
                        if (form15 != null)
                        { form15.Show(); }
                        if (form16 != null)
                        { form16.Show(); }
                        if (form17 != null)
                        { form17.Show(); }
                        if (form18 != null)
                        { form18.Show(); }
                        if (form19 != null)
                        { form19.Show(); }
                        if (form20 != null)
                        { form20.Show(); }

                        if (form22 != null)
                        { form22.Show(); }
                    }

                }
                else
                {
                    mainform.Hide();
                    if (form1 != null)
                    { form1.Hide(); }
                    if (form2 != null)
                    { form2.Hide(); }
                    if (form3 != null)
                    { form3.Hide(); }
                    if (form4 != null)
                    { form4.Hide(); }
                    if (form5 != null)
                    { form5.Hide(); }
                    if (form6 != null)
                    { form6.Hide(); }
                    if (form7 != null)
                    { form7.Hide(); }
                    if (form8 != null)
                    { form8.Hide(); }
                    if (form9 != null)
                    { form9.Hide(); }
                    if (form10 != null)
                    { form10.Hide(); }
                    if (form11 != null)
                    { form11.Hide(); }
                    if (form12 != null)
                    { form12.Hide(); }
                    if (form13 != null)
                    { form13.Hide(); }
                    if (form14 != null)
                    { form14.Hide(); }
                    if (form15 != null)
                    { form15.Hide(); }
                    if (form16 != null)
                    { form16.Hide(); }
                    if (form17 != null)
                    { form17.Hide(); }
                    if (form18 != null)
                    { form18.Hide(); }
                    if (form19 != null)
                    { form19.Hide(); }
                    if (form20 != null)
                    { form20.Hide(); }

                    if (form22 != null)
                    { form22.Hide(); }
                    
                    CheckForm checkform = new CheckForm(this);
                    checkform.CancelBtn.Enabled = false;
                    checkform.ShowDialog();
                    mainform.Show();
                    if (form1 != null)
                    { form1.Show(); }
                    if (form2 != null)
                    { form2.Show(); }
                    if (form3 != null)
                    { form3.Show(); }
                    if (form4 != null)
                    { form4.Show(); }
                    if (form5 != null)
                    { form5.Show(); }
                    if (form6 != null)
                    { form6.Show(); }
                    if (form7 != null)
                    { form7.Show(); }
                    if (form8 != null)
                    { form8.Show(); }
                    if (form9 != null)
                    { form9.Show(); }
                    if (form10 != null)
                    { form10.Show(); }
                    if (form11 != null)
                    { form11.Show(); }
                    if (form12 != null)
                    { form12.Show(); }
                    if (form13 != null)
                    { form13.Show(); }
                    if (form14 != null)
                    { form14.Show(); }
                    if (form15 != null)
                    { form15.Show(); }
                    if (form16 != null)
                    { form16.Show(); }
                    if (form17 != null)
                    { form17.Show(); }
                    if (form18 != null)
                    { form18.Show(); }
                    if (form19 != null)
                    { form19.Show(); }
                    if (form20 != null)
                    { form20.Show(); }

                    if (form22 != null)
                    { form22.Show(); }
                }
            }
        }

        private void CheckHour()
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
            Parameter.i = num;
            DateTime now = DateTime.Now;
            DateTime preheattime;
            //获取开机时间
            String table = "extrusion_s3_preheat";
            List<String> queryCols = new List<String>(new String[] { "s3_end_insulation_time_2" });
            List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
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
                int duration = Convert.ToInt32(delt.TotalMinutes) % 120; //余数
                if (110 <= duration && duration < 120) //差十分钟
                {
                    //检查是否填写
                    String table1 = "running_record_of_feeding_unit"; //吹膜供料系统运行记录
                    List<String> queryCols1 = new List<String>(new String[] { "id" });
                    List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals1 = new List<Object>(new Object[] { Convert.ToInt32(Parameter.proInstruID) });
                    String betweenCol1 = "modifytime";
                    DateTime right1 = now;
                    DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120));
                    DateTime right11 = new DateTime(right1.Year, right1.Month, right1.Day, right1.Hour, right1.Minute, right1.Second); //格式
                    DateTime left11 = new DateTime(left1.Year, left1.Month, left1.Day, left1.Hour, left1.Minute, left1.Second);
                    List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left11, right11);

                    String table2 = "running_record_of_extrusion_unit"; //吹膜机组运行记录
                    List<String> queryCols2 = new List<String>(new String[] { "id" });
                    List<String> whereCols2 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol2 = "modifytime";
                    DateTime right2 = now;
                    DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120));
                    DateTime right21 = new DateTime(right2.Year, right2.Month, right2.Day, right2.Hour, right2.Minute, right2.Second);
                    DateTime left21 = new DateTime(left2.Year, left2.Month, left2.Day, left2.Hour, left2.Minute, left2.Second);
                    List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left21, right21);

                    if (res1.Count != 0 && res2.Count != 0)
                    {
                        return;
                    }
                    else if (res1.Count == 0 && res2.Count != 0)
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜供料系统运行记录”！", "警告");
                        return;
                    }
                    else if (res1.Count != 0 && res2.Count == 0)
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜机组运行记录”！", "警告");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("请在10分钟之内填写“吹膜供料系统运行记录”和“吹膜机组运行记录”！", "警告");
                        return;
                    }

                }
                else if (0 <= duration && duration < 1) //到了两小时
                {
                    //检查是否填写
                    String table1 = "runnig_record_of_feeding_unit"; //吹膜供料系统运行记录
                    List<String> queryCols1 = new List<String>(new String[] { "id" });
                    List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals1 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol1 = "modifytime";
                    DateTime right1 = now;
                    DateTime left1 = right1.AddMinutes(-120);
                    List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left1, right1);
                    String table2 = "running_record_of_extrusion_unit"; //吹膜机组运行记录
                    List<String> queryCols2 = new List<String>(new String[] { "id" });
                    List<String> whereCols2 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol2 = "modifytime";
                    DateTime right2 = now;
                    DateTime left2 = right2.AddMinutes(-120);
                    List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left2, right2);

                    if (res1.Count != 0 && res2.Count != 0)
                    {
                        return;
                    }
                    else
                    {
                        mainform.Hide();
                        if (form1 != null)
                        { form1.Hide(); }
                        if (form2 != null)
                        { form2.Hide(); }
                        if (form3 != null)
                        { form3.Hide(); }
                        if (form4 != null)
                        { form4.Hide(); }
                        if (form5 != null)
                        { form5.Hide(); }
                        if (form6 != null)
                        { form6.Hide(); }
                        if (form7 != null)
                        { form7.Hide(); }
                        if (form8 != null)
                        { form8.Hide(); }
                        if (form9 != null)
                        { form9.Hide(); }
                        if (form10 != null)
                        { form10.Hide(); }
                        if (form11 != null)
                        { form11.Hide(); }
                        if (form12 != null)
                        { form12.Hide(); }
                        if (form13 != null)
                        { form13.Hide(); }
                        if (form14 != null)
                        { form14.Hide(); }
                        if (form15 != null)
                        { form15.Hide(); }
                        if (form16 != null)
                        { form16.Hide(); }
                        if (form17 != null)
                        { form17.Hide(); }
                        if (form18 != null)
                        { form18.Hide(); }
                        if (form19 != null)
                        { form19.Hide(); }
                        if (form20 != null)
                        { form20.Hide(); }

                        if (form22 != null)
                        { form22.Hide(); }

                        CheckForm checkform = new CheckForm(this);
                        checkform.CancelBtn.Enabled = false;
                        checkform.ShowDialog();
                        mainform.Show();
                        if (form1 != null)
                        { form1.Show(); }
                        if (form2 != null)
                        { form2.Show(); }
                        if (form3 != null)
                        { form3.Show(); }
                        if (form4 != null)
                        { form4.Show(); }
                        if (form5 != null)
                        { form5.Show(); }
                        if (form6 != null)
                        { form6.Show(); }
                        if (form7 != null)
                        { form7.Show(); }
                        if (form8 != null)
                        { form8.Show(); }
                        if (form9 != null)
                        { form9.Show(); }
                        if (form10 != null)
                        { form10.Show(); }
                        if (form11 != null)
                        { form11.Show(); }
                        if (form12 != null)
                        { form12.Show(); }
                        if (form13 != null)
                        { form13.Show(); }
                        if (form14 != null)
                        { form14.Show(); }
                        if (form15 != null)
                        { form15.Show(); }
                        if (form16 != null)
                        { form16.Show(); }
                        if (form17 != null)
                        { form17.Show(); }
                        if (form18 != null)
                        { form18.Show(); }
                        if (form19 != null)
                        { form19.Show(); }
                        if (form20 != null)
                        { form20.Show(); }

                        if (form22 != null)
                        { form22.Show(); }
                    }

                }
                else
                {
                    mainform.Hide();
                    if (form1 != null)
                    { form1.Hide(); }
                    if (form2 != null)
                    { form2.Hide(); }
                    if (form3 != null)
                    { form3.Hide(); }
                    if (form4 != null)
                    { form4.Hide(); }
                    if (form5 != null)
                    { form5.Hide(); }
                    if (form6 != null)
                    { form6.Hide(); }
                    if (form7 != null)
                    { form7.Hide(); }
                    if (form8 != null)
                    { form8.Hide(); }
                    if (form9 != null)
                    { form9.Hide(); }
                    if (form10 != null)
                    { form10.Hide(); }
                    if (form11 != null)
                    { form11.Hide(); }
                    if (form12 != null)
                    { form12.Hide(); }
                    if (form13 != null)
                    { form13.Hide(); }
                    if (form14 != null)
                    { form14.Hide(); }
                    if (form15 != null)
                    { form15.Hide(); }
                    if (form16 != null)
                    { form16.Hide(); }
                    if (form17 != null)
                    { form17.Hide(); }
                    if (form18 != null)
                    { form18.Hide(); }
                    if (form19 != null)
                    { form19.Hide(); }
                    if (form20 != null)
                    { form20.Hide(); }

                    if (form22 != null)
                    { form22.Hide(); }

                    CheckForm checkform = new CheckForm(this);
                    checkform.CancelBtn.Enabled = false;
                    checkform.ShowDialog();
                    mainform.Show();
                    if (form1 != null)
                    { form1.Show(); }
                    if (form2 != null)
                    { form2.Show(); }
                    if (form3 != null)
                    { form3.Show(); }
                    if (form4 != null)
                    { form4.Show(); }
                    if (form5 != null)
                    { form5.Show(); }
                    if (form6 != null)
                    { form6.Show(); }
                    if (form7 != null)
                    { form7.Show(); }
                    if (form8 != null)
                    { form8.Show(); }
                    if (form9 != null)
                    { form9.Show(); }
                    if (form10 != null)
                    { form10.Show(); }
                    if (form11 != null)
                    { form11.Show(); }
                    if (form12 != null)
                    { form12.Show(); }
                    if (form13 != null)
                    { form13.Show(); }
                    if (form14 != null)
                    { form14.Show(); }
                    if (form15 != null)
                    { form15.Show(); }
                    if (form16 != null)
                    { form16.Show(); }
                    if (form17 != null)
                    { form17.Show(); }
                    if (form18 != null)
                    { form18.Show(); }
                    if (form19 != null)
                    { form19.Show(); }
                    if (form20 != null)
                    { form20.Show(); }

                    if (form22 != null)
                    { form22.Show(); }
                }
            }
        }


        //定义各窗体变量
        BatchProductRecord.BatchProductRecord form1 = null;
        BatchProductRecord.ProcessProductInstru form2 = null;
        Record_extrusClean form3 = null;
        Record_extrusSiteClean form4 = null;
        HandoverRecordofExtrusionProcess form5 = null;
        Record_extrusSupply form6 = null;
        mySystem.Extruction.Chart.wasterecord form7 = null;
        Record_material_reqanddisg form8 = null;
        ProdctDaily_extrus form9 = null;
        ExtructionpRoductionAndRestRecordStep6 form10 = null;
        MaterialBalenceofExtrusionProcess form11 = null;
        ProductInnerPackagingRecord form12 = null;
        mySystem.Extruction.Chart.outerpack form13 = null;
        ExtructionCheckBeforePowerStep2 form14 = null;
        ExtructionPreheatParameterRecordStep3 form15 = null;
        mySystem.Extruction.Chart.feedrecord form16 = null;
        mySystem.Extruction.Chart.beeholetable form17 = null;
        Record_train form18 = null;
        ReplaceHeadForm form19 = null;
        ExtructionReplaceCore form20 = null;
        
        
        ExtructionTransportRecordStep4 form22 = null;
          

        private void A1Btn_Click(object sender, EventArgs e)
        {
            if (form1 == null || form1.IsDisposed)
            { form1 = new BatchProductRecord.BatchProductRecord(mainform); }            
            form1.Show();
            form1.BringToFront();
        }
        private void A2Btn_Click(object sender, EventArgs e)
        {
            if (form2 == null || form2.IsDisposed)
            { form2 = new BatchProductRecord.ProcessProductInstru(mainform);}            
            form2.Show();
            form2.BringToFront();
        }
        private void A3Btn_Click(object sender, EventArgs e)
        {
            if (form3 == null || form3.IsDisposed)
            { form3 = new Record_extrusClean(mainform);}            
            form3.Show();
            form3.BringToFront();
        }
        private void A5Btn_Click(object sender, EventArgs e)
        {
            if (form4 == null || form4.IsDisposed)
            { form4 = new Record_extrusSiteClean(mainform);}            
            form4.Show();
            form4.BringToFront();
        }
        private void A4Btn_Click(object sender, EventArgs e)
        {
            if (form5 == null || form5.IsDisposed)
            { form5 = new HandoverRecordofExtrusionProcess(mainform);}            
            form5.Show();
            form5.BringToFront();
        }
        private void B2Btn_Click_2(object sender, EventArgs e)
        {
            if (form6 == null || form6.IsDisposed)
            { form6 = new Record_extrusSupply(mainform);}            
            form6.Show();
            form6.BringToFront();
        }
        private void B3Btn_Click(object sender, EventArgs e)
        {
            if (form7 == null || form7.IsDisposed)
            {form7 = new Extruction.Chart.wasterecord(mainform); }            
            form7.Show();
            form7.BringToFront();
        }
        private void B4Btn_Click_2(object sender, EventArgs e)
        {
            if (form8 == null || form8.IsDisposed)
            { form8 = new Record_material_reqanddisg(mainform); }            
            form8.Show();
            form8.BringToFront();
        }
        private void B5Btn_Click_2(object sender, EventArgs e)
        {
            if (form9 == null || form9.IsDisposed)
            { form9 = new ProdctDaily_extrus(mainform);}            
            form9.Show();
            form9.BringToFront();
        }
        private void B6Btn_Click(object sender, EventArgs e)
        {
            if (form10 == null || form10.IsDisposed)
            { form10 = new ExtructionpRoductionAndRestRecordStep6(mainform);}           
            form10.Show();
            form10.BringToFront();
        }
        private void B7Btn_Click(object sender, EventArgs e)
        {
            if (form11 == null || form11.IsDisposed)
            { form11 = new MaterialBalenceofExtrusionProcess(mainform);}            
            form11.Show();
            form11.BringToFront();
        }
        private void B8Btn_Click(object sender, EventArgs e)
        {
            if (form12 == null || form12.IsDisposed)
            { form12 = new ProductInnerPackagingRecord(mainform);}           
            form12.Show();
            form12.BringToFront();
        }
        private void B9Btn_Click(object sender, EventArgs e)
        {
            if (form13 == null || form13.IsDisposed)
            { form13 = new Extruction.Chart.outerpack(mainform); }            
            form13.Show();
            form13.BringToFront();
        }
        private void C1Btn_Click(object sender, EventArgs e)
        {
            if (form14 == null || form14.IsDisposed)
            { form14 = new ExtructionCheckBeforePowerStep2(mainform);}            
            form14.Show();
            form14.BringToFront();
        }
        private void C2Btn_Click(object sender, EventArgs e)
        {
            if (form15 == null || form15.IsDisposed)
            { form15 = new ExtructionPreheatParameterRecordStep3(mainform);}            
            form15.Show();
            form15.BringToFront();
        }
        private void C3Btn_Click(object sender, EventArgs e)
        {
            if (form16 == null || form16.IsDisposed)
            { form16 = new Extruction.Chart.feedrecord(mainform); }           
            form16.Show();
            form16.BringToFront();
        }
        private void C4Btn_Click(object sender, EventArgs e)
        {
            if (form17 == null || form17.IsDisposed)
            { form17 = new Extruction.Chart.beeholetable(mainform);}            
            form17.Show();
            form17.BringToFront();
        }
        private void D1Btn_Click_2(object sender, EventArgs e)
        {
            if (form18 == null || form18.IsDisposed)
            { form18 = new Record_train(mainform);}           
            form18.Show();
            form18.BringToFront();
        }
        private void D2Btn_Click(object sender, EventArgs e)
        {
            if (form19 == null || form19.IsDisposed)
            { form19 = new ReplaceHeadForm();}            
            form19.Show();
            form19.BringToFront();
        }
        private void D3Btn_Click(object sender, EventArgs e)
        {
            if (form20 == null || form20.IsDisposed)
            { form20 = new ExtructionReplaceCore(mainform); }            
            form20.Show();
            form20.BringToFront();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            if (form22 == null || form22.IsDisposed)
            { form22 = new ExtructionTransportRecordStep4(mainform); }           
            form22.Show();
            form22.BringToFront();
        }
        private void D4Btn_Click(object sender, EventArgs e)
        {

        }

        private void ExtructionMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ExtructionMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();

        }

        private void ExtructionMainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true && comboBox1.SelectedIndex != -1)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }

        }

        

        
    }
}



