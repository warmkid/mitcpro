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
        private ExtructionProcess extructionformfather = null;

        private string sql = "Select * From extrusion";
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();
            
            conn = base.mainform.conn;
            connOle = base.mainform.connOle;

            InformationInitialize();
            
        }

        private void InformationInitialize()
        {
            ///***********************表头数据初始化************************///
            this.PSbox.AutoSize = false;
            this.PSbox.Height = 32;

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
            }
            

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
