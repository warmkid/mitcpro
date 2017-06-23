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
    public partial class ExtructionTransportRecordStep4 : BaseForm
    {
        private DataTable dt = new DataTable();

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

        private DateTimePicker dtp = new DateTimePicker();

        private int Recordnum = 0;
                
        public ExtructionTransportRecordStep4(MainForm mainform): base(mainform)
        {
            //mainform = mForm;
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            DataTabelInitialize();

            //AddLineBtn.Enabled = false;
        }


        private void DataTabelInitialize()
        {
            ///***********************表格数据初始化************************///
            //添加列
            dt.Columns.Add("时间", typeof(String));
            //dt.Columns["时间"].ReadOnly = true;
            dt.Columns.Add("物料代码", typeof(String));
            dt.Columns.Add("数量(件)", typeof(String));
            dt.Columns.Add("kg/件", typeof(String));
            dt.Columns.Add("数量/kg", typeof(String));
            dt.Columns.Add("包装是否完好", typeof(bool));
            dt.Columns.Add("是否清洁合格", typeof(bool));        
            AddRowLine();
            this.TransportRecordView.DataSource = dt;
            this.TransportRecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            //添加按钮列
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.TransportRecordView.Columns.Add(MyButtonColumn);
            this.TransportRecordView.AllowUserToAddRows = false;
            //设置对齐
            this.TransportRecordView.RowHeadersVisible = false;
            this.TransportRecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.TransportRecordView.Columns.Count; i++)
            {
                this.TransportRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.TransportRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.TransportRecordView.Columns[i].MinimumWidth = 120;
            }
            this.TransportRecordView.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.TransportRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.ColumnHeadersHeight = 40;
            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.TransportRecordView.Columns["传料日期\r2016年"].Width = 80;

            //若已有数据，向内部添加现有数据
            ///***********************表头数据初始化************************///
            if (isSqlOk)
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataAdapter daSQL = new SqlDataAdapter(comm);
                DataTable dtSQL = new DataTable();
                daSQL.Fill(dtSQL);

                int stepnow = Convert.ToInt32(dtSQL.Rows[0]["step_status"]);
                if (stepnow >= 4)
                {
                    this.TransportRecordView.Rows[0].Cells["时间"].Value = dtSQL.Rows[0]["s4_time"].ToString();
                    this.TransportRecordView.Rows[0].Cells["物料代码"].Value = dtSQL.Rows[0]["s4_raw_material_id"].ToString();
                    this.TransportRecordView.Rows[0].Cells["数量(件)"].Value = dtSQL.Rows[0]["s4_quantity"].ToString();
                    this.TransportRecordView.Rows[0].Cells["kg/件"].Value = dtSQL.Rows[0]["s4_kilogram_per_piece"].ToString();
                    this.TransportRecordView.Rows[0].Cells["数量/kg"].Value = dtSQL.Rows[0]["s4_quantity_per_kilogram"].ToString();
                    this.TransportRecordView.Rows[0].Cells["包装是否完好"].Value = bool.Parse(dtSQL.Rows[0]["s4_is_packed_well"].ToString());
                    this.TransportRecordView.Rows[0].Cells["是否清洁合格"].Value = bool.Parse(dtSQL.Rows[0]["s4_is_cleaned"].ToString());
                }
                comm.Dispose();
                daSQL.Dispose();
                dtSQL.Dispose();
            }
            else
            {
                OleDbCommand comm = new OleDbCommand(sql, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(comm);
                DataTable dtOle = new DataTable();
                daOle.Fill(dtOle);

                int stepnow = Convert.ToInt32(dtOle.Rows[0]["step_status"]);
                if (stepnow >= 4)
                {
                    this.TransportRecordView.Rows[0].Cells["时间"].Value = dtOle.Rows[0]["s4_time"].ToString();
                    this.TransportRecordView.Rows[0].Cells["物料代码"].Value = dtOle.Rows[0]["s4_raw_material_id"].ToString();
                    this.TransportRecordView.Rows[0].Cells["数量(件)"].Value = dtOle.Rows[0]["s4_quantity"].ToString();
                    this.TransportRecordView.Rows[0].Cells["kg/件"].Value = dtOle.Rows[0]["s4_kilogram_per_piece"].ToString();
                    this.TransportRecordView.Rows[0].Cells["数量/kg"].Value = dtOle.Rows[0]["s4_quantity_per_kilogram"].ToString();
                    bool val_bool = (dtOle.Rows[0]["s4_is_packed_well"].ToString() == "1" ? true : false);
                    this.TransportRecordView.Rows[0].Cells["包装是否完好"].Value = val_bool;
                    val_bool = (dtOle.Rows[0]["s4_is_cleaned"].ToString() == "1" ? true : false);
                    this.TransportRecordView.Rows[0].Cells["是否清洁合格"].Value = val_bool;
                }
                comm.Dispose();
                daOle.Dispose();
                dtOle.Dispose();
            }
        }        

        //添加单行模板
        private void AddRowLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dt.NewRow();            
            //rowline["时间"] = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            rowline["时间"] = null;//dtp.Value.ToShortDateString();
            //rowline["时间"] = DateTime.Now.TimeOfDay.ToString();
            //rowline["时间"] = DateTime.Now.ToLongDateString().ToString();
            rowline["物料代码"] = "SPM-PE";
            rowline["数量(件)"] = "";
            rowline["kg/件"] = "";
            rowline["数量/kg"] = "";
            rowline["包装是否完好"] = true;
            rowline["是否清洁合格"] = true;
            //添加行
            dt.Rows.Add(rowline);
            Recordnum = Recordnum + 1;       
        }

        //删除单条记录
        private void TransportRecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            //if (true == dtp.Visible) dtp.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                if (TransportRecordView.Columns[e.ColumnIndex].Name == "时间")
                {
                    dtp.Visible = true;
                    showdtp(e);
                }
                else
                {
                    dtp.Visible = false;
                    if (TransportRecordView.Columns[e.ColumnIndex].Name == "删除该条记录")
                    {
                        if (Recordnum > 0)
                        {
                            this.TransportRecordView.Rows.RemoveAt(e.RowIndex);//删除行
                            Recordnum = Recordnum - 1;
                        }
                    }
                }                
            }
        }

        private void showdtp(DataGridViewCellEventArgs e)
        {

            //dtp.Size = TransportRecordView.CurrentCell.Size;
            //dtp.Top = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top + dataGridView1.Top;
            //dtp.Left = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left + dataGridView1.Left;
            //dtp.Top = TransportRecordView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top;
            //dtp.Left = TransportRecordView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left;

            Rectangle Rect = this.TransportRecordView.GetCellDisplayRectangle(this.TransportRecordView.CurrentCell.ColumnIndex, this.TransportRecordView.CurrentCell.RowIndex, false);
            //显示dTimePicker在dataGridView1选中单元格显示区域的矩形里面,即选中单元格内
            //dtp.Visible = true;
            dtp.Top = Rect.Top;
            dtp.Left = Rect.Left;
            dtp.Height = Rect.Height;
            dtp.Width = Rect.Width;

            dtp.BringToFront();
            dtp.Visible = true;
            if (!(object.Equals(Convert.ToString(TransportRecordView.CurrentCell.Value), "")))
            {
                dtp.Value = Convert.ToDateTime(TransportRecordView.CurrentCell.Value);
            }

            dtp.Visible = true;
            this.TransportRecordView.Controls.Add(dtp);
            //dtp.Show();

            //DateTimePicker dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            //dateTimePicker1.Location = new System.Drawing.Point(0,0);
            //dateTimePicker1.Name = "dateTimePicker1";
            //dateTimePicker1.Size = dataGridView1.CurrentCell.Size;
            //dateTimePicker1.TabIndex = 24;
            //dateTimePicker1.BringToFront();
            //this.Controls.Add(dateTimePicker1);

            dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            TransportRecordView.CurrentCell.Value = dtp.Value.ToShortDateString();
        }

        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRowLine();
        }

        public void DataSave()
        {
            string[] sqlstr = new string[8];
            SqlCommand com = null;
            sqlstr[0] = "update extrusion set s4_time =  CAST( '" + this.TransportRecordView.Rows[0].Cells["时间"].Value.ToString() + "' AS time)  where id =1";
            sqlstr[1] = "update extrusion set s4_raw_material_id = '" + this.TransportRecordView.Rows[0].Cells["物料代码"].Value.ToString() + "' where id =1";
            sqlstr[2] = "update extrusion set s4_quantity = " + Convert.ToInt32(this.TransportRecordView.Rows[0].Cells["数量(件)"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[3] = "update extrusion set s4_kilogram_per_piece = " + Convert.ToInt32(this.TransportRecordView.Rows[0].Cells["kg/件"].Value.ToString()).ToString() + "  where id =1";
            sqlstr[4] = "update extrusion set s4_quantity_per_kilogram = " + Convert.ToInt32(this.TransportRecordView.Rows[0].Cells["数量/kg"].Value.ToString()).ToString() + "  where id =1";
            string val = this.TransportRecordView.Rows[0].Cells["包装是否完好"].Value.ToString() == "True" ? "1" : "0";
            sqlstr[5] = "update extrusion set s4_is_packed_well = " + val + "  where id =1";
            val = this.TransportRecordView.Rows[0].Cells["是否清洁合格"].Value.ToString() == "True" ? "1" : "0";
            sqlstr[6] = "update extrusion set s4_is_cleaned = " + val + "  where id =1";
            sqlstr[7] = "update extrusion set step_status = 4 where id =1";
            
            for (int i = 0; i < 8; i++)
            {
                com = new SqlCommand(sqlstr[i], conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }
            
        }
    }
}
