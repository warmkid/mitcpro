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

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : Form
    {
        private ExtructionProcess extructionformfather = null;
        public string recorder; //记录人
        public string checker; //复核人
        public string date; //日期

        private SqlConnection conn = null;
        private string sql = "Select * From extrusion";

        public ExtructionPreheatParameterRecordStep3(ExtructionProcess winMain, SqlConnection Mainconn)
        {
            InitializeComponent();
            extructionformfather = winMain;
            conn = Mainconn;
            InformationInitialize();
            
        }

        private void InformationInitialize()
        {
            ///***********************表头数据初始化************************///
            recorder = "记录人员";
            checker = "复核人员";
            date = DateTime.Now.ToLongDateString().ToString();
            /*
            this.recorderlabel.Text = recorder;
            this.checkerlabel.Text = checker;
            this.datelabel.Text = date;
             * */
            this.PSbox.AutoSize = false;
            this.PSbox.Height = 32;

            //若已有数据，向内部添加现有数据
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter daSQL = new SqlDataAdapter(comm);
            DataTable dtSQL = new DataTable();
            daSQL.Fill(dtSQL);

            int stepnow = Convert.ToInt32(dtSQL.Rows[0]["step_status"]);
            if (stepnow >= 3)
            {
                this.phiBox.Text = dtSQL.Rows[0]["s3_core_specifications_1"].ToString();
                this.gapBox.Text = dtSQL.Rows[0]["s3_core_specifications_2"].ToString();
                this.timeBox1.Text = dtSQL.Rows[0]["s3_start_preheat_time"].ToString();
                this.timeBox2.Text = dtSQL.Rows[0]["s3_end_preheat_time"].ToString();
                this.timeBox3.Text = dtSQL.Rows[0]["s3_start_insulation_time"].ToString();
                this.timeBox4.Text = dtSQL.Rows[0]["s3_end_insulation_time_1"].ToString();
                this.timeBox5.Text = dtSQL.Rows[0]["s3_end_insulation_time_2"].ToString();                
            }

            comm.Dispose();
            daSQL.Dispose();
            dtSQL.Dispose();

           this.PSbox.Text = "时间输入的格式示例为：2003-12-24 14:34:08";

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

        public void DataSave()
        {
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
        }
    }
}
