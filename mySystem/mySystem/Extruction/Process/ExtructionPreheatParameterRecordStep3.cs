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

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        //private DateTime review_date;

        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();
            
            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            operator_id = base.mainform.userID;
            isSqlOk = base.mainform.isSqlOk;

            if (isSqlOk) { operator_name = checkIDSql(operator_id); }
            else { operator_name = checkIDOle(operator_id); }

            InformationInitialize();
            
        }

        private void InformationInitialize()
        {
            ///***********************表头数据初始化************************///
            this.PSbox.AutoSize = false;
            this.PSbox.Height = 32;
            
            operator_name = checkIDOle(operator_id);
            recorderBox.Text = operator_name;

            operate_date = DateTime.Now;
            dateTimePicker1.Value = operate_date.Date;

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
            List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
            */

            List<String> queryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2", "s3_duration1", "s3_duration2", "s3_duration3" });
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                        
            List<String> data = new List<String>{ };
            for (int i = 0; i < queryValsList[0].Count-3; i++)
            { data.Add(queryValsList[0][i].ToString()); }
            data.Add("加热保温" + queryValsList[0][queryValsList[0].Count - 3].ToString() + "分钟");
            data.Add("加热保温" + queryValsList[0][queryValsList[0].Count - 2].ToString() + "分钟");
            data.Add("加热保温" + queryValsList[0][queryValsList[0].Count - 1].ToString() + "分钟");

            List<Control> cons = new List<Control> { label_s3_hw_set1, label_s3_ld_set1, label_s3_mj_set1, label_s3_jt1_set1, label_s3_jt2_set1, 
                    label_s3_km_set1, label_s3_region1_set1, label_s3_region2_set1, label_s3_region3_set1, label_s3_region4_set1, label_s3_hw_set2, 
                    label_s3_ld_set2, label_s3_mj_set2, label_s3_jt1_set2, label_s3_jt2_set2, label_s3_km_set2, label_s3_region1_set2, label_s3_region2_set2, 
                    label_s3_region3_set2, label_s3_region4_set2, label_s3_duration1, label_s3_duration2, label_s3_duration3};

            Utility.fillControl(cons, data);
            
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
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s3_recorder_id", "s3_record_date", "s3_reviewer_id", "s3_core_specifications_1", "s3_core_specifications_2", "s3_start_preheat_time", "s3_end_preheat_time", "s3_start_insulation_time", "s3_end_insulation_time_1", "s3_end_insulation_time_2" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, operate_date.Date, review_id, Convert.ToInt32(this.phiBox.Text).ToString(), Convert.ToInt32(this.gapBox.Text).ToString() });
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time1Picker.Value.Hour, Time1Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time2Picker.Value.Hour, Time2Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time3Picker.Value.Hour, Time3Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time4Picker.Value.Hour, Time4Picker.Value.Minute, operate_date.Second));
                queryVals.Add(new DateTime(operate_date.Year, operate_date.Month, operate_date.Day, Time5Picker.Value.Hour, Time5Picker.Value.Minute, operate_date.Second));

                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            
        }

        private string checkIDSql(int userID)
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSqlOk) { DataSaveSql(); }
            else { DataSaveOle(); }
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            CheckForm check = new CheckForm(base.mainform);
            check.ShowDialog();
            review_id = check.userID;
            if (isSqlOk)
            {   }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s3_reviewer_id"});
                List<Object> queryVals = new List<Object>(new Object[] { review_id });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                reviewer_name = checkIDOle(review_id);
            }
            checkerBox.Text = reviewer_name;
        }
           
    }
}
