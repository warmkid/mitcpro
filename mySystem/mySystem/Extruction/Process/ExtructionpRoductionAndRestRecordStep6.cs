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
    public partial class ExtructionpRoductionAndRestRecordStep6 : BaseForm
    {
        private ExtructionProcess extructionformfather = null;
        private DataTable dtInformation = new DataTable();
        private DataTable dtRecord = new DataTable();

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

        private int Recordnum = 0;
        private string productname = "";  //产品名称
        private string productnumber = "";  //产品批号
        private string temperature = "";  //环境温度
        private string humidity = "";  //相对湿度
        private bool spot = true;  //班次；true白班；false夜班


        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            RecordViewInitialize();

            AddLineBtn.Enabled = false;
        }


        private void RecordViewInitialize()
        {
            this.DatecheckBox.Checked = spot;
            this.NeightcheckBox.Checked = !spot;

            //添加列
            dtRecord.Columns.Add("序号", typeof(String));
            dtRecord.Columns.Add("时间", typeof(String));
            //dtRecord.Columns["时间"].ReadOnly = true;
            dtRecord.Columns.Add("膜卷编号\r(卷)", typeof(String));
            dtRecord.Columns.Add("膜卷长度\r(m)", typeof(String));
            dtRecord.Columns.Add("膜卷重量\r(kg)", typeof(String));
            dtRecord.Columns.Add("外观", typeof(bool));
            dtRecord.Columns.Add("宽度\r(mm)", typeof(String));
            dtRecord.Columns.Add("最大厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("最小厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("平均厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("厚度公差\r(%)", typeof(String));
            dtRecord.Columns.Add("判定", typeof(bool));
            //添加内容
            AddRecordRowLine(); 
            this.RecordView.DataSource = dtRecord;
            this.RecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.RecordView.AllowUserToAddRows = false;
            //添加按钮列
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.RecordView.Columns.Add(MyButtonColumn);
            this.RecordView.AllowUserToAddRows = false;
            //设置对齐
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].MinimumWidth = 87;
            }
            this.RecordView.Columns[0].MinimumWidth = 40;
            this.RecordView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.ColumnHeadersHeight = 40;


            ///***********************表头数据初始化************************///
            if (isSqlOk)
            {
                //若已有数据，向内部添加现有数据
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataAdapter daSQL = new SqlDataAdapter(comm);
                DataTable dtSQL = new DataTable();
                daSQL.Fill(dtSQL);

                int stepnow = Convert.ToInt32(dtSQL.Rows[0]["step_status"]);
                if (stepnow >= 6)
                {
                    this.productnameBox.Text = dtSQL.Rows[0]["product_name"].ToString();
                    this.productnumberBox.Text = dtSQL.Rows[0]["product_batch"].ToString();
                    this.temperatureBox.Text = dtSQL.Rows[0]["s6_temperature"].ToString();
                    this.humidityBox.Text = dtSQL.Rows[0]["s6_relative_humidity"].ToString();
                    bool val = bool.Parse(dtSQL.Rows[0]["s6_flight"].ToString());
                    this.DatecheckBox.Checked = val;
                    this.NeightcheckBox.Checked = !val;
                    this.RecordView.Rows[0].Cells["时间"].Value = dtSQL.Rows[0]["s6_time"].ToString();
                    this.RecordView.Rows[0].Cells["膜卷编号\r(卷)"].Value = dtSQL.Rows[0]["s6_mojuan_number"].ToString();
                    this.RecordView.Rows[0].Cells["膜卷长度\r(m)"].Value = dtSQL.Rows[0]["s6_mojuan_length"].ToString();
                    this.RecordView.Rows[0].Cells["膜卷重量\r(kg)"].Value = dtSQL.Rows[0]["s6_mojuan_weight"].ToString();
                    this.RecordView.Rows[0].Cells["外观"].Value = bool.Parse(dtSQL.Rows[0]["s6_outward"].ToString());
                    this.RecordView.Rows[0].Cells["宽度\r(mm)"].Value = dtSQL.Rows[0]["s6_width"].ToString();
                    this.RecordView.Rows[0].Cells["最大厚度\r（μm）"].Value = dtSQL.Rows[0]["s6_max_thickness"].ToString();
                    this.RecordView.Rows[0].Cells["最小厚度\r（μm）"].Value = dtSQL.Rows[0]["s6_min_thickness"].ToString();
                    this.RecordView.Rows[0].Cells["平均厚度\r（μm）"].Value = dtSQL.Rows[0]["s6_aver_thickness"].ToString();
                    this.RecordView.Rows[0].Cells["厚度公差\r(%)"].Value = dtSQL.Rows[0]["s6_tolerance_thickness"].ToString();
                    this.RecordView.Rows[0].Cells["判定"].Value = bool.Parse(dtSQL.Rows[0]["s6_is_qualified"].ToString());
                }
                comm.Dispose();
                daSQL.Dispose();
                dtSQL.Dispose();
            }
            else
            { }
            
        }

        private void AddRecordRowLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dtRecord.NewRow();
            rowline["序号"] = (Recordnum+1).ToString();
            rowline["时间"] = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();            
            rowline["膜卷编号\r(卷)"] = "";
            rowline["膜卷长度\r(m)"] = "";
            rowline["膜卷重量\r(kg)"] = "";
            rowline["外观"] = true;
            rowline["宽度\r(mm)"] = "";
            rowline["最大厚度\r（μm）"] = "";
            rowline["最小厚度\r（μm）"] = "";
            rowline["平均厚度\r（μm）"] = "";
            rowline["厚度公差\r(%)"] = "";
            rowline["判定"] = true;
            //添加行
            dtRecord.Rows.InsertAt(rowline, Recordnum); 
            if (Recordnum==0)
            {
                //AddTotalLine();
            }
            Recordnum = Recordnum + 1;
        }

        //添加最后一行
        private void AddTotalLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dtRecord.NewRow();
            rowline["序号"] = "";
            rowline["时间"] = "总计";
            rowline["膜卷编号\r(卷)"] = "";
            rowline["膜卷长度\r(m)"] = "";
            rowline["膜卷重量\r(kg)"] = "";
            rowline["外观"] = true;
            rowline["宽度\r(mm)"] = "";
            rowline["最大厚度\r（μm）"] = "";
            rowline["最小厚度\r（μm）"] = "";
            rowline["平均厚度\r（μm）"] = "";
            rowline["厚度公差\r(%)"] = "";
            rowline["判定"] = true;
            //添加行
            dtRecord.Rows.Add(rowline);
        }

        //删除单条记录
        private void RecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RecordView.Columns[e.ColumnIndex].Name == "删除该条记录")
            {
                if (e.RowIndex != Recordnum)
                {
                    if (Recordnum > 1)
                    {
                        this.RecordView.Rows.RemoveAt(e.RowIndex);//删除行
                        Recordnum = Recordnum - 1;
                    }
                    else if (Recordnum > 0)
                    {
                        this.RecordView.Rows.RemoveAt(e.RowIndex);//删除行
                        this.RecordView.Rows.RemoveAt(0);//删除行
                        Recordnum = Recordnum - 1;
                    }
                    RefreshNum();
                }                
            }
        }        

        //更新序号
        private void RefreshNum()
        {
            for (int i = 0; i < RecordView.Rows.Count-1; i++)
            {
                RecordView.Rows[i].Cells["序号"].Value = (i+1).ToString();
            }
        }

        //日班改变时，夜班也随之改变
        private void DatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DatecheckBox.Checked)
            {
                NeightcheckBox.Checked = false;
                DatecheckBox.Checked = true;
                spot = true;
            }
        }

        //夜班改变时，夜班也随之改变
        private void NeightcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NeightcheckBox.Checked)
            {
                DatecheckBox.Checked = false;
                NeightcheckBox.Checked = true;
                spot = false;
            }
        }

        //添加行按钮
        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRecordRowLine();
        }

        //保存数据
        public void DataSave()
        {
            string[] sqlstr = new string[17];
            SqlCommand com = null;

            sqlstr[0] = "update extrusion set product_name = '" + this.productnameBox.Text + "' where id =1";
            sqlstr[1] = "update extrusion set product_batch = '" + this.productnumberBox.Text + "' where id =1";
            sqlstr[2] = "update extrusion set s6_temperature = " + Convert.ToInt32(this.temperatureBox.Text.ToString()).ToString() + "  where id =1";
            sqlstr[3] = "update extrusion set s6_relative_humidity = " + Convert.ToInt32(this.humidityBox.Text.ToString()).ToString() + "  where id =1";
            string flight = this.DatecheckBox.Checked.ToString() == "True" ? "1" : "0";
            sqlstr[4] = "update extrusion set s6_flight = " + flight + " where id =1";
            sqlstr[5] = "update extrusion set s6_time =  CAST( '" + this.RecordView.Rows[0].Cells["时间"].Value.ToString() + "' AS time)  where id =1";
            sqlstr[6] = "update extrusion set s6_mojuan_number = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["膜卷编号\r(卷)"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[7] = "update extrusion set s6_mojuan_length = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["膜卷长度\r(m)"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[8] = "update extrusion set s6_mojuan_weight = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["膜卷重量\r(kg)"].Value.ToString()).ToString() + "  where id =1";
            string val = this.RecordView.Rows[0].Cells["外观"].Value.ToString() == "True" ? "1" : "0";
            sqlstr[9] = "update extrusion set s6_outward = " + val + "  where id =1";            
            sqlstr[10] = "update extrusion set s6_width = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["宽度\r(mm)"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[11] = "update extrusion set s6_max_thickness = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["最大厚度\r（μm）"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[12] = "update extrusion set s6_min_thickness = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["最小厚度\r（μm）"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[13] = "update extrusion set s6_aver_thickness = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["平均厚度\r（μm）"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[14] = "update extrusion set s6_tolerance_thickness = " + Convert.ToInt32(this.RecordView.Rows[0].Cells["厚度公差\r(%)"].Value.ToString()).ToString() + "  where id =1";
            val = this.RecordView.Rows[0].Cells["判定"].Value.ToString() == "True" ? "1" : "0";
            sqlstr[15] = "update extrusion set s6_is_qualified = " + val + "  where id =1";
            sqlstr[16] = "update extrusion set step_status = 6 where id =1";

            for (int i = 0; i < 17; i++)
            {
                com = new SqlCommand(sqlstr[i], conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }
            //最后一行是合计 
        }        
    }
}
