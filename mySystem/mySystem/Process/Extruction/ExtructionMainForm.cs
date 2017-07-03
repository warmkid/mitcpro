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


namespace mySystem
{
    public partial class ExtructionMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;
        System.Timers.Timer timer = new System.Timers.Timer();//实例化Timer类

        public ExtructionMainForm(MainForm mainform)
            : base(mainform)
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
            //timer.Interval = 20000;
            //timer.Enabled = true;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckHour);

        }
        int num = 0;
        void test(object sender, System.Timers.ElapsedEventArgs e)
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
        }


        //定时器调用的函数，判断时间，查看是否填写
        private void CheckHour(object sender, System.Timers.ElapsedEventArgs e)
        {
            label1.Text = Convert.ToString((++num)); //显示到lable 
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
                preheattime = Convert.ToDateTime(res[0][0]);
                TimeSpan delt = now - preheattime;
                if (110 <= Convert.ToInt32(delt.TotalMinutes) % 120 || Convert.ToInt32(delt.TotalMinutes) % 120 < 120) //差十分钟
                {
                    //检查是否填写
                    //？？？？？？？？？？？标准表达式中数据类型不匹配。
                    String table1 = "running_record_of_feeding_unit"; //吹膜供料系统运行记录
                    List<String> queryCols1 = new List<String>(new String[] { "id" });
                    List<String> whereCols1 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals1 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol1 = "modifytime";
                    DateTime right1 = now;
                    DateTime left1 = right1.AddMinutes(-(delt.TotalMinutes % 120));
                    List<List<Object>> res1 = Utility.selectAccess(Parameter.connOle, table1, queryCols1, whereCols1, whereVals1, null, null, betweenCol1, left1, right1);

                    String table2 = "running_record_of_extrusion_unit"; //吹膜机组运行记录
                    List<String> queryCols2 = new List<String>(new String[] { "id" });
                    List<String> whereCols2 = new List<String>(new String[] { "production_instruction_id" });
                    List<Object> whereVals2 = new List<Object>(new Object[] { Parameter.proInstruID });
                    String betweenCol2 = "modifytime";
                    DateTime right2 = now;
                    DateTime left2 = right2.AddMinutes(-(delt.TotalMinutes % 120));
                    List<List<Object>> res2 = Utility.selectAccess(Parameter.connOle, table2, queryCols2, whereCols2, whereVals2, null, null, betweenCol2, left2, right2);

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
                else if (0 <= Convert.ToInt32(delt.TotalMinutes) % 120 || Convert.ToInt32(delt.TotalMinutes) % 120 < 1) //到了两小时
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
                        this.Hide();

                        CheckForm checkform = new CheckForm(this);
                        checkform.CancelBtn.Enabled = false;
                        checkform.ShowDialog();
                        this.Show();
                    }

                }
                else
                {
                    return;
                }
            }


        }


        private void A3Btn_Click(object sender, EventArgs e)
        {
            Record_extrusClean extrusclean = new Record_extrusClean(mainform);
            extrusclean.Show();
        }
        private void A5Btn_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean ext = new Record_extrusSiteClean(mainform);
            ext.Show();
        }
        private void A1Btn_Click(object sender, EventArgs e)
        {
            BatchProductRecord.BatchProductRecord b = new BatchProductRecord.BatchProductRecord(mainform);
            b.Show();
        }
        private void A2Btn_Click(object sender, EventArgs e)
        {
            BatchProductRecord.ProcessProductInstru ppi = new BatchProductRecord.ProcessProductInstru(mainform);
            ppi.Show();
        }
        private void C1Btn_Click(object sender, EventArgs e)
        {
            ExtructionCheckBeforePowerStep2 stepform = new ExtructionCheckBeforePowerStep2(mainform);
            stepform.Show();
        }
        private void C2Btn_Click(object sender, EventArgs e)
        {
            ExtructionPreheatParameterRecordStep3 stepform = new ExtructionPreheatParameterRecordStep3(mainform);
            stepform.Show();
        }
        private void B1Btn_Click(object sender, EventArgs e)
        {
            ExtructionTransportRecordStep4 stepform = new ExtructionTransportRecordStep4(mainform);
            stepform.Show();
        }
        private void B6Btn_Click(object sender, EventArgs e)
        {
            ExtructionpRoductionAndRestRecordStep6 stepform = new ExtructionpRoductionAndRestRecordStep6(mainform);
            stepform.Show();
        }
        private void C3Btn_Click(object sender, EventArgs e)
        {

            mySystem.Extruction.Chart.feedrecord feedrecord = new Extruction.Chart.feedrecord(mainform);
            feedrecord.Show();
        }
        private void B2Btn_Click(object sender, EventArgs e)
        {
            Record_extrusSupply recordsupply = new Record_extrusSupply(mainform);
            recordsupply.Show();
        }
        private void B4Btn_Click(object sender, EventArgs e)
        {
            Record_material_reqanddisg mat = new Record_material_reqanddisg(mainform);
            mat.Show();
        }
        private void B5Btn_Click(object sender, EventArgs e)
        {
            ProdctDaily_extrus pro = new ProdctDaily_extrus(mainform);
            pro.Show();
        }
        private void B2Btn_Click_1(object sender, EventArgs e)
        {
            Record_extrusSupply r = new Record_extrusSupply(mainform);
            r.Show();
        }
        private void B4Btn_Click_1(object sender, EventArgs e)
        {
            Record_material_reqanddisg r = new Record_material_reqanddisg(mainform);
            r.Show();
        }
        private void B5Btn_Click_1(object sender, EventArgs e)
        {
            ProdctDaily_extrus p = new ProdctDaily_extrus(mainform);
            p.Show();
        }
        private void B3Btn_Click(object sender, EventArgs e)
        {
            mySystem.Extruction.Chart.wasterecord wasterecord = new Extruction.Chart.wasterecord(mainform);
            wasterecord.Show();
        }
        private void B2Btn_Click_2(object sender, EventArgs e)
        {
            Record_extrusSupply r = new Record_extrusSupply(mainform);
            r.Show();
        }
        private void B4Btn_Click_2(object sender, EventArgs e)
        {
            Record_material_reqanddisg r = new Record_material_reqanddisg(mainform);
            r.Show();
        }
        private void B5Btn_Click_2(object sender, EventArgs e)
        {
            ProdctDaily_extrus p = new ProdctDaily_extrus(mainform);
            p.Show();
        }
        private void B8Btn_Click(object sender, EventArgs e)
        {
            ProductInnerPackagingRecord PIPRform = new ProductInnerPackagingRecord(mainform);
            PIPRform.Show();
        }
        private void D1Btn_Click(object sender, EventArgs e)
        {
            Record_train r = new Record_train(mainform);
            r.Show();
        }
        private void C4Btn_Click(object sender, EventArgs e)
        {
            mySystem.Extruction.Chart.beeholetable beeholetable = new Extruction.Chart.beeholetable(mainform);
            beeholetable.Show();
        }
        private void B7Btn_Click(object sender, EventArgs e)
        {
            MaterialBalenceofExtrusionProcess test = new MaterialBalenceofExtrusionProcess(mainform);
            test.Show();
        }
        private void A4Btn_Click(object sender, EventArgs e)
        {
            HandoverRecordofExtrusionProcess test = new HandoverRecordofExtrusionProcess(mainform);
            test.Show();
        }
        private void B9Btn_Click(object sender, EventArgs e)
        {
            mySystem.Extruction.Chart.outerpack test = new Extruction.Chart.outerpack(mainform);
            test.Show();
        }
        private void D3Btn_Click(object sender, EventArgs e)
        {
            ExtructionReplaceCore ERCform = new ExtructionReplaceCore(mainform);
            ERCform.Show();
        }
        private void D2Btn_Click(object sender, EventArgs e)
        {
            ReplaceHeadForm myDlg = new ReplaceHeadForm();
            myDlg.Show();
        }
        private void D1Btn_Click_1(object sender, EventArgs e)
        {
            Record_train r = new Record_train(mainform);
            r.Show();
        }
        private void D1Btn_Click_2(object sender, EventArgs e)
        {
            Record_train r = new Record_train(mainform);
            r.Show();
        }
        private void D4Btn_Click(object sender, EventArgs e)
        {

        }
    }
}



