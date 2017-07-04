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

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : BaseForm
    {
        private String table = "extrusion_s3_preheat";
        private String tableSetting = "extrusion_s3_preheat";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;

        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false;
        private bool isSaveOk = false;

        List<Control> TemConsList = new List<Control> { };//各种温度的控件
        List<String> TemValdata = new List<String> { };//设置界面内存储的各种温度
        private int TemTolerance;//温度公差

        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;

            //温度控件的初始化
            TemConsList = new List<Control> { label_s3_hw_set1, label_s3_ld_set1, label_s3_mj_set1, label_s3_jt1_set1, label_s3_jt2_set1, 
                    label_s3_km_set1, label_s3_region1_set1, label_s3_region2_set1, label_s3_region3_set1, label_s3_region4_set1, label_s3_hw_set2, 
                    label_s3_ld_set2, label_s3_mj_set2, label_s3_jt1_set2, label_s3_jt2_set2, label_s3_km_set2, label_s3_region1_set2, label_s3_region2_set2, 
                    label_s3_region3_set2, label_s3_region4_set2 }; 

            Init();
            TempInit();

            DataShow(Instructionid);   
            
        }

        private void Init()
        {
            ///***********************数据初始化************************///
            this.PSbox.AutoSize = false;
            this.PSbox.Height = 32;

            //基本信息的初始化
            phiBox.Text = "";
            gapBox.Text = "";
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            recorderBox.Text = operator_name;
            operate_date = DateTime.Now;
            dateTimePicker1.Value = operate_date.Date;
            PSbox.Text = "";

            //时间控件初始化
            this.Time1Picker.ShowUpDown = true;
            this.Time1Picker.Format = DateTimePickerFormat.Custom;
            this.Time1Picker.CustomFormat = "HH:mm";
            this.Time2Picker.ShowUpDown = true;
            this.Time2Picker.Format = DateTimePickerFormat.Custom;
            this.Time2Picker.CustomFormat = "HH:mm";
            this.Time3Picker.ShowUpDown = true;
            this.Time3Picker.Format = DateTimePickerFormat.Custom;
            this.Time3Picker.CustomFormat = "HH:mm";
            this.Time4Picker.ShowUpDown = true;
            this.Time4Picker.Format = DateTimePickerFormat.Custom;
            this.Time4Picker.CustomFormat = "HH:mm";
            this.Time5Picker.ShowUpDown = true;
            this.Time5Picker.Format = DateTimePickerFormat.Custom;
            this.Time5Picker.CustomFormat = "HH:mm";            

            /*
            if (isSqlOk)
            {
                //若已有数据，向内部添加现有数据
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataAdapter daSQL = new SqlDataAdapter(comm);
                DataTable dtSQL = new DataTable();
                daSQL.Fill(dtSQL);

                this.phiBox.Text = dtSQL.Rows[0]["s3_core_specifications_1"].ToString();
                this.gapBox.Text = dtSQL.Rows[0]["s3_core_specifications_2"].ToString();
                this.timeBox1.Text = dtSQL.Rows[0]["s3_start_preheat_time"].ToString();
                this.timeBox2.Text = dtSQL.Rows[0]["s3_end_preheat_time"].ToString();
                this.timeBox3.Text = dtSQL.Rows[0]["s3_start_insulation_time"].ToString();
                this.timeBox4.Text = dtSQL.Rows[0]["s3_end_insulation_time_1"].ToString();
                this.timeBox5.Text = dtSQL.Rows[0]["s3_end_insulation_time_2"].ToString();

                comm.Dispose();
                daSQL.Dispose();
                dtSQL.Dispose();
            }
            else
            {
                //若已有数据，向内部添加现有数据
                OleDbCommand comm = new OleDbCommand(sql, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(comm);
                DataTable dtOle = new DataTable();
                daOle.Fill(dtOle);

                this.phiBox.Text = dtOle.Rows[0]["s3_core_specifications_1"].ToString();
                this.gapBox.Text = dtOle.Rows[0]["s3_core_specifications_2"].ToString();
                this.timeBox1.Text = dtOle.Rows[0]["s3_start_preheat_time"].ToString();
                this.timeBox2.Text = dtOle.Rows[0]["s3_end_preheat_time"].ToString();
                this.timeBox3.Text = dtOle.Rows[0]["s3_start_insulation_time"].ToString();
                this.timeBox4.Text = dtOle.Rows[0]["s3_end_insulation_time_1"].ToString();
                this.timeBox5.Text = dtOle.Rows[0]["s3_end_insulation_time_2"].ToString();

                comm.Dispose();
                daOle.Dispose();
                dtOle.Dispose();
            }*/
            

           //this.PSbox.Text = "时间输入的格式示例为：2003-12-24 14:34:08";

        }

        private void otherConsChanged(bool canChanged)
        {

            List<TextBox> TextBoxList = new List<TextBox> { };//各种温度的控件
            TextBoxList = new List<TextBox> { label_s3_hw_set1, label_s3_ld_set1, label_s3_mj_set1, label_s3_jt1_set1, label_s3_jt2_set1, 
                    label_s3_km_set1, label_s3_region1_set1, label_s3_region2_set1, label_s3_region3_set1, label_s3_region4_set1, label_s3_hw_set2, 
                    label_s3_ld_set2, label_s3_mj_set2, label_s3_jt1_set2, label_s3_jt2_set2, label_s3_km_set2, label_s3_region1_set2, label_s3_region2_set2, 
                    label_s3_region3_set2, label_s3_region4_set2 };

            if (canChanged == true)
            {
                phiBox.ReadOnly = false;
                gapBox.ReadOnly = false;
                recorderBox.ReadOnly = false;
                checkerBox.ReadOnly = false;
                PSbox.ReadOnly = false;           
                Time1Picker.Enabled = true;
                Time2Picker.Enabled = true;
                Time3Picker.Enabled = true;
                Time4Picker.Enabled = true;
                Time5Picker.Enabled = true;
                for (int i = 0; i < TextBoxList.Count; i++)
                {
                    TextBoxList[i].ReadOnly = false;
                }
            }
            else
            {
                phiBox.ReadOnly = true;
                gapBox.ReadOnly = true;
                recorderBox.ReadOnly = true;
                checkerBox.ReadOnly = true;
                PSbox.ReadOnly = true;
                Time1Picker.Enabled = false;
                Time2Picker.Enabled = false;
                Time3Picker.Enabled = false;
                Time4Picker.Enabled = false;
                Time5Picker.Enabled = false;
                for (int i = 0; i < TextBoxList.Count; i++)
                {
                    TextBoxList[i].ReadOnly = true;
                }
            }
        }

        private void TempInit()
        {
            //设置界面内温度的初始化
            List<String> queryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2"});
            ///***********************以后要改数据库！！！！！***********************///
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queValsList = Utility.selectAccess(connOle, tableSetting, queryCols, whereCols, whereVals, null, null, null, null, null);
            TemValdata = new List<String> { };
            for (int i = 0; i < queValsList[0].Count; i++)
            { TemValdata.Add(queValsList[0][i].ToString()); }
            Utility.fillControl(TemConsList, TemValdata);   
            //获取公差
            queryCols = new List<String>(new String[] { "s3_temperature_tolerance" });
            queValsList = Utility.selectAccess(connOle, tableSetting, queryCols, whereCols, whereVals, null, null, null, null, null);
            TemTolerance = Convert.ToInt32(queValsList[0][0].ToString());
        }

        private void DataShow(int InstructionID)
        {
            List<String> queryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2","s3_duration1", "s3_duration2", "s3_duration3" });
            //顺序：温度->保温时间
            List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
            List<Object> whereVals = new List<Object>(new Object[] { InstructionID });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);

            if (queryValsList.Count == 0)
            {
                isSaveOk = false;
                //其他信息                
                Init();
                //填写设置界面内的温度
                Utility.fillControl(TemConsList, TemValdata);
                //保温时间初始化
                List<String> queryCols1 = new List<String>(new String[] { "s3_duration1", "s3_duration2", "s3_duration3" });
                ///***********************以后要改数据库！！！！！***********************///
                List<String> whereCols1 = new List<String>(new String[] { "id" });
                List<Object> whereVals1 = new List<Object>(new Object[] { 1 });
                List<List<Object>> queValsList1 = Utility.selectAccess(connOle, tableSetting, queryCols1, whereCols1, whereVals1, null, null, null, null, null);
                List<String> data = new List<String> { };
                for (int i = 0; i < queValsList1[0].Count; i++)
                { data.Add(queValsList1[0][i].ToString()); }
                List<Control> Cons = new List<Control> { s3_duration1_label, s3_duration2_label, s3_duration3_label };
                Utility.fillControl(Cons, data);
                
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;

                otherConsChanged(true);
            }
            else
            {
                isSaveOk = true;
                //填写数据库的温度
                List<String> data = new List<String> { };
                for (int i = 0; i < queryValsList[0].Count-3; i++)
                { data.Add(queryValsList[0][i].ToString()); }
                Utility.fillControl(TemConsList, data);
                //保温时间初始化
                data = new List<String> { };
                for (int i = queryValsList[0].Count-3; i < queryValsList[0].Count; i++)
                { data.Add(queryValsList[0][i].ToString()); }
                List<Control> Cons = new List<Control> { s3_duration1_label, s3_duration2_label, s3_duration3_label };
                Utility.fillControl(Cons, data);
                //填写其他的信息
                queryCols = new List<String>(new String[] { "s3_recorder_id", "s3_record_date", "s3_reviewer_id", "s3_review_opinion", "s3_is_review_qualified", 
                    "s3_core_specifications_1", "s3_core_specifications_2",
                    "s3_start_preheat_time", "s3_end_preheat_time", "s3_start_insulation_time", "s3_end_insulation_time_1", "s3_end_insulation_time_2" });
                List<List<Object>> queryValsList2 = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);                
                operator_id = Convert.ToInt32(queryValsList2[0][0].ToString());
                operator_name = Parameter.IDtoName(operator_id);
                recorderBox.Text = operator_name;
                operate_date = Convert.ToDateTime(queryValsList2[0][1].ToString());
                dateTimePicker1.Value = operate_date;
                review_id = Convert.ToInt32(queryValsList2[0][2].ToString());
                reviewer_name = Parameter.IDtoName(review_id);
                checkerBox.Text = reviewer_name;

                //填写已有的数据
                phiBox.Text = queryValsList2[0][5].ToString();
                gapBox.Text = queryValsList2[0][6].ToString();
                Time1Picker.Value = Convert.ToDateTime(queryValsList2[0][7].ToString());
                Time2Picker.Value = Convert.ToDateTime(queryValsList2[0][8].ToString());
                Time3Picker.Value = Convert.ToDateTime(queryValsList2[0][9].ToString());
                Time4Picker.Value = Convert.ToDateTime(queryValsList2[0][10].ToString());
                Time5Picker.Value = Convert.ToDateTime(queryValsList2[0][11].ToString());
                PSbox.Text = "";

                if (reviewer_name != null)
                {
                    //已有审核人信息
                    review_opinion = queryValsList2[0][3].ToString();
                    ischeckOk = Convert.ToBoolean(queryValsList2[0][4].ToString());
                    //审核通过，则确认、保存均不可点
                    if (ischeckOk)
                    {
                        otherConsChanged(false);
                        SaveBtn.Enabled = false;
                        CheckBtn.Enabled = false;
                        printBtn.Enabled = true;
                    }
                    else
                    {
                        otherConsChanged(true);
                        SaveBtn.Enabled = true;
                        CheckBtn.Enabled = true;
                        printBtn.Enabled = false;
                    }
                }
                else
                {
                    review_opinion = null;
                    ischeckOk = false;
                    review_id = 0;
                    reviewer_name = null;
                    checkerBox.Text = "";

                    otherConsChanged(true);
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                }
            }
        }

        private bool CheckTemperature()
        {
            Boolean Tempcheck =true ;
            int numtemp = 0;
            String[] TemString = new String[20];
            TemString[0] = "换网预热";
            TemString[1] = "流道预热";
            TemString[2] = "模颈预热";
            TemString[3] = "机头一预热";
            TemString[4] = "机头二预热";
            TemString[5] = "口模预热";
            TemString[6] = "I区预热";
            TemString[7] = "II区预热";
            TemString[8] = "III区预热";
            TemString[9] = "IV区预热";
            TemString[10] = "换网预热";
            TemString[11] = "流道预热";
            TemString[12] = "模颈预热";
            TemString[13] = "机头一预热";
            TemString[14] = "机头二预热";
            TemString[15] = "口模预热";
            TemString[16] = "I区预热";
            TemString[17] = "II区预热";
            TemString[18] = "III区预热";
            TemString[19] = "IV区预热";

            for (int i = 0; i < TemConsList.Count; i++)
            {
                if((Int32.TryParse(TemConsList[i].Text, out numtemp) == false))
                {
                    MessageBox.Show("请在" + TemString [i]+ "参数框内填入数字!");
                    Tempcheck = false;
                    break;
                }
                else if((Convert.ToInt32(TemValdata[i].ToString())-TemTolerance>Convert.ToInt32(TemConsList[i].Text))||(Convert.ToInt32(TemValdata[i].ToString())+TemTolerance<Convert.ToInt32(TemConsList[i].Text)))
                {
                    MessageBox.Show(TemString [i] + "参数超出允许范围，请重新设置");
                    Tempcheck = false;
                    break;
                }
                else
                {}
            }

            return Tempcheck;
        }

        private void TabelPaint()
        {
            Graphics g = this.CreateGraphics();
            this.Show();
            //出来一个画笔,这只笔画出来的颜色是红的  
            Pen p = new Pen(Brushes.Red);

            //创建两个点  
            Point p1 = new Point(0, 0);
            Point p2 = new Point(1000, 1000);

            //将两个点连起来  
            g.DrawLine(p, p1, p2);
        }

        public void DataSaveSql()
        {
            /*
            string[] sqlstr = new string[8];
            SqlCommand com = null;

            //sqlstr[0] = "update extrusion set s3_core_specifications_1 = " + Convert.ToInt32(this.phiBox.Text).ToString() + "  where id =1";
  
          //sqlstr[1] = "update extrusion set s3_core_specifications_2 = " + Convert.ToInt32(this.gapBox.Text).ToString() + "  where id =1";
            sqlstr[0] = "update extrusion set s3_core_specifications_1 = '" + Convert.ToInt32(this.phiBox.Text).ToString() + "'  where id =1";
            sqlstr[1] = "update extrusion set s3_core_specifications_2 = '" + Convert.ToInt32(this.gapBox.Text).ToString() + "'  where id =1";
            sqlstr[2] = "update extrusion set s3_start_preheat_time =  CAST( '"+timeBox1.Text+"' AS datetime)  where id =1";
            sqlstr[3] = "update extrusion set s3_end_preheat_time =  CAST( '" + timeBox2.Text + "' AS datetime)  where id =1";
            sqlstr[4] = "update extrusion set s3_start_insulation_time =  CAST( '" + timeBox3.Text + "' AS datetime)  where id =1";
            sqlstr[5] = "update extrusion set s3_end_insulation_time_1 =  CAST( '" + timeBox4.Text + "' AS datetime)  where id =1";
            sqlstr[6] = "update extrusion set s3_end_insulation_time_2 =  CAST( '" + timeBox5.Text + "' AS datetime)  where id =1";   
            sqlstr[7] = "update extrusion set step_status = 3 where id =1";
            
            for (int i = 0; i < 8; i++)
            {
                com = new SqlCommand(sqlstr[i], conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }  
             * */
        }

        public void DataSaveOle()
        {
            //jason 保存表格
            /*
            List<String> queryCols = new List<String>(new String[] { "s2_is_qualified", "s2_operator_id", "s2_operate_date" });
            List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), operator_id, recordTimePicker.Value });
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);*/
            int numtemp = 0;
            if ((Int32.TryParse(this.phiBox.Text, out numtemp) == false) || (Int32.TryParse(this.gapBox.Text, out numtemp) == false))
            {
                MessageBox.Show("请在模芯规格内填入数字!");
            }
            else if (CheckTemperature() == false)
            {   }
            else
            {
                Boolean b = false;
                operator_name = recorderBox.Text;
                if (operator_name != Parameter.IDtoName(operator_id))
                {
                    operator_id = Parameter.NametoID(operator_name);
                }
                List<String> queryCols = new List<String>(new String[] { "s3_recorder_id", "s3_record_date", "s3_core_specifications_1", "s3_core_specifications_2","s3_duration1", "s3_duration2", "s3_duration3", 
                    "s3_start_preheat_time", "s3_end_preheat_time", "s3_start_insulation_time", "s3_end_insulation_time_1", "s3_end_insulation_time_2" });
                List<Object> queryVals = new List<Object>(new Object[] { operator_id, operate_date.Date, Convert.ToInt32(this.phiBox.Text).ToString(), Convert.ToInt32(this.gapBox.Text).ToString(),
                    Convert.ToInt32(s3_duration1_label.Text).ToString(),Convert.ToInt32(s3_duration2_label.Text).ToString(),Convert.ToInt32(s3_duration3_label.Text).ToString() });
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
                List<Object> whereVals = new List<Object>(new Object[] { Instructionid });
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time1Picker.Value.Hour, Time1Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time2Picker.Value.Hour, Time2Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time3Picker.Value.Hour, Time3Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time4Picker.Value.Hour, Time4Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time5Picker.Value.Hour, Time5Picker.Value.Minute, operate_date.Second));

                if (isSaveOk == false)
                {
                    //新建记录                    
                    queryCols.Add("production_instruction_id");
                    queryVals.Add(Instructionid);
                    b = Utility.insertAccess(connOle, table, queryCols, queryVals);
                }
                else
                {
                    //已有的修改                    
                    b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                }

                //添加温度数据
                queryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2"});
                List<String> data = new List<String> { };
                data = Utility.readFromControl(TemConsList);
                queryVals = new List<Object> { };
                for (int i = 0; i < queryCols.Count; i++)
                {
                    queryVals.Add(data[i]);
                }
                b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);

                CheckBtn.Enabled = true;
            }            
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSqlOk) { DataSaveSql(); }
            else { DataSaveOle(); }
        }

        public override void CheckResult()
        {
            base.CheckResult();
            review_id = check.userID;
            reviewer_name = Parameter.IDtoName(review_id);
            review_opinion = check.opinion;
            ischeckOk = check.ischeckOk;

            if (isSqlOk)
            {  }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s3_reviewer_id", "s3_review_opinion", "s3_is_review_qualified" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, review_opinion, ischeckOk });
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
                List<Object> whereVals = new List<Object>(new Object[] { Instructionid });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            //判断检验合格
            if (ischeckOk)
            {
                otherConsChanged(false);
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
            }
            else
            {
                otherConsChanged(true);
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
            }
            checkerBox.Text = reviewer_name;  

        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
            if (isSqlOk)
            { }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s3_reviewer_id" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                reviewer_name = Parameter.IDtoName(review_id);
            }
            checkerBox.Text = reviewer_name;
        }
        
    }
}
