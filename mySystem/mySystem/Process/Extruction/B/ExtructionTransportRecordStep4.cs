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
using Newtonsoft.Json.Linq;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionTransportRecordStep4 : BaseForm
    {
        //private DataTable dt = new DataTable();

        private String table = "extrusion_s4_delivery";
        
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false;
        //private DateTimePicker dtp = new DateTimePicker();

        private int checknum = 0;
        bool isFirstBind = true;
                
        public ExtructionTransportRecordStep4(MainForm mainform): base(mainform)
        {
            //mainform = mForm;
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;

            if (isSqlOk) { operator_name = checkIDSql(operator_id); }
            else { operator_name = checkIDOle(operator_id); }

            DataTabelInitialize();

            AddLineBtn.Enabled = false;
            CheckBtn.Enabled = false;
        }


        private void DataTabelInitialize()
        {
            ///***********************表格数据初始化************************///
            //添加列
            
            AddRowLine();
            //this.TransportRecordView.DataSource = dt;
            this.TransportRecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            //添加按钮列
            /*
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.TransportRecordView.Columns.Add(MyButtonColumn);
            this.TransportRecordView.AllowUserToAddRows = false;
             * */
            //设置对齐
            this.TransportRecordView.AllowUserToAddRows = false;
            this.TransportRecordView.RowHeadersVisible = false;
            this.TransportRecordView.AllowUserToResizeColumns = false;
            this.TransportRecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.TransportRecordView.Columns.Count; i++)
            {
                this.TransportRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.TransportRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.TransportRecordView.Columns[i].MinimumWidth = 90;
            }
            //this.TransportRecordView.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.TransportRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.ColumnHeadersHeight = 40;

            this.TransportRecordView.Columns["传料日期"].ReadOnly = true;
            this.TransportRecordView.Columns["时间"].ReadOnly = true;
            this.TransportRecordView.Columns["kgper件"].ReadOnly = true;
            this.TransportRecordView.Columns["数量Total"].ReadOnly = true;

            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.TransportRecordView.Columns["传料日期\r2016年"].Width = 80;
        }

        //添加单行模板
        private void AddRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in TransportRecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = DateTime.Now.ToString("yyyy-MM-dd");
            dr.Cells[1].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString(); ;
            dr.Cells[2].Value = "SPM-PE";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "5";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = true; //"包装是否完好"
            dr.Cells[7].Value = true; //"是否清洁合格"
            dr.Cells[8].Value = operator_name; //"操作人"
            dr.Cells[9].Value = ""; //"复核人"

            TransportRecordView.Rows.Add(dr);

        }

        //删除单条记录
        private void TransportRecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            //if (true == dtp.Visible) dtp.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                
            }
        }

        private void TransportRecordView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (TransportRecordView.Columns[e.ColumnIndex].Name == "数量")
                {
                    int numtemp;
                    if (Int32.TryParse((TransportRecordView.Rows[e.RowIndex].Cells["数量"].Value.ToString()), out numtemp) == false)
                    { MessageBox.Show("请在数量(件)内填入数字!"); TransportRecordView.Rows[e.RowIndex].Cells["数量"].Value = ""; }
                    else { TransportRecordView.Rows[e.RowIndex].Cells["数量Total"].Value = (numtemp * 5).ToString(); }
                }
                else
                {

                }
            }
        }

        /*
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
         * */

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            //TransportRecordView.CurrentCell.Value = dtp.Value.ToShortDateString();
        }

        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRowLine();
            AddLineBtn.Enabled = false;
            SaveBtn.Enabled = true;
            DelLineBtn.Enabled = true;
            if (TransportRecordView.Rows.Count > 0)
                TransportRecordView.FirstDisplayedScrollingRowIndex = TransportRecordView.Rows.Count - 1;
        }

        public void DataShow()
        {
            //若已有数据，向内部添加现有数据
            ///***********************表头数据初始化************************///
            /*
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
            }*/
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
  
        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (TransportRecordView.Rows.Count >= 2)
            {
                int deletenum = TransportRecordView.CurrentRow.Index;
                if (deletenum == TransportRecordView.Rows.Count - 1)
                { 
                    this.TransportRecordView.Rows.RemoveAt(deletenum);
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = false;
                    SaveBtn.Enabled = false;
                }
            }             
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSqlOk) { DataSaveSql(); }
            else { DataSaveOle(); }
            CheckBtn.Enabled = true;
            DelLineBtn.Enabled = false;
        }

        private void DataSaveSql()
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

        private void DataSaveOle()
        {
            int numtemp;
            if (Int32.TryParse((TransportRecordView.Rows[checknum].Cells["数量"].Value.ToString()), out numtemp) == true)
            {
                if (checknum == 0)
                {
                    //jason 保存表格
                    JArray jarray = JArray.Parse("[]");
                    for (int i = 0; i < TransportRecordView.Rows.Count; i++)
                    {
                        string json = @"{}";
                        JObject j = JObject.Parse(json);
                        j.Add("s4_raw_material_delivery_date", new JValue(Convert.ToDateTime(TransportRecordView.Rows[i].Cells["传料日期"].Value.ToString())));
                        j.Add("s4_time", new JValue(Convert.ToDateTime(TransportRecordView.Rows[i].Cells["时间"].Value.ToString())));
                        j.Add("s4_raw_material_id", new JValue(TransportRecordView.Rows[i].Cells["物料代码"].Value.ToString()));
                        j.Add("s4_quantity", new JValue(Convert.ToInt32(TransportRecordView.Rows[i].Cells["数量"].Value.ToString())));
                        j.Add("s4_kilogram_per_piece", new JValue(Convert.ToInt32(TransportRecordView.Rows[i].Cells["kgper件"].Value.ToString())));
                        j.Add("s4_quantity_per_kilogram", new JValue(Convert.ToInt32(TransportRecordView.Rows[i].Cells["数量Total"].Value.ToString())));
                        j.Add("s4_is_packed_well", new JValue(TransportRecordView.Rows[i].Cells["包装是否完好"].Value.ToString() == "True" ? "1" : "0"));
                        j.Add("s4_is_cleaned", new JValue(TransportRecordView.Rows[i].Cells["是否清洁合格"].Value.ToString() == "True" ? "1" : "0"));
                        j.Add("s4_operator_id", new JValue(operator_id.ToString()));
                        j.Add("s4_reviewer_id", new JValue(""));
                        jarray.Add(j);
                    }
                    List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
                    List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString() });
                    List<String> whereCols = new List<String>(new String[] { "id" });
                    List<Object> whereVals = new List<Object>(new Object[] { 1 });
                    //Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
                    Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                }
                else
                {
                    List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
                    List<String> whereCols = new List<String>(new String[] { "id" });
                    List<Object> whereVals = new List<Object>(new Object[] { 1 });
                    List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                    //解析jason
                    JArray jo = JArray.Parse(queryValsList[0][0].ToString());

                    int rownum = TransportRecordView.Rows.Count;
                    string json = @"{}";
                    JObject j = JObject.Parse(json);
                    j.Add("s4_raw_material_delivery_date", new JValue(Convert.ToDateTime(TransportRecordView.Rows[rownum - 1].Cells["传料日期"].Value.ToString())));
                    j.Add("s4_time", new JValue(Convert.ToDateTime(TransportRecordView.Rows[rownum - 1].Cells["时间"].Value.ToString())));
                    j.Add("s4_raw_material_id", new JValue(TransportRecordView.Rows[rownum - 1].Cells["物料代码"].Value.ToString()));
                    j.Add("s4_quantity", new JValue(Convert.ToInt32(TransportRecordView.Rows[rownum - 1].Cells["数量"].Value.ToString())));
                    j.Add("s4_kilogram_per_piece", new JValue(Convert.ToInt32(TransportRecordView.Rows[rownum - 1].Cells["kgper件"].Value.ToString())));
                    j.Add("s4_quantity_per_kilogram", new JValue(Convert.ToInt32(TransportRecordView.Rows[rownum - 1].Cells["数量Total"].Value.ToString())));
                    j.Add("s4_is_packed_well", new JValue(TransportRecordView.Rows[rownum - 1].Cells["包装是否完好"].Value.ToString() == "True" ? "1" : "0"));
                    j.Add("s4_is_cleaned", new JValue(TransportRecordView.Rows[rownum - 1].Cells["是否清洁合格"].Value.ToString() == "True" ? "1" : "0"));
                    j.Add("s4_operator_id", new JValue(operator_id.ToString()));
                    j.Add("s4_reviewer_id", new JValue(""));
                    jo.Add(j);

                    List<Object> queryVals = new List<Object>(new Object[] { jo.ToString() });
                    Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                }
            }
            else
            { MessageBox.Show("请在数量(件)内填入数字!"); TransportRecordView.Rows[checknum].Cells["数量"].Value = ""; }
        }

        public override void CheckResult()
        {
            base.CheckResult();
            review_id = check.userID;
            review_opinion = check.opinion;
            ischeckOk = check.ischeckOk;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();

            if (isSqlOk)
            {
                reviewer_name = checkIDOle(review_id);

                List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                List<List<Object>> queryValsList = Utility.selectAccess(mySystem.Parameter.conn, table, queryCols, whereCols, whereVals, null, null, null, null, null);

                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                //填数据

                DataTable dtsave = new DataTable();

                dtsave.Columns.Add("传料日期", typeof(String));
                dtsave.Columns.Add("时间", typeof(String));
                dtsave.Columns.Add("物料代码", typeof(String));
                dtsave.Columns.Add("数量", typeof(String));
                dtsave.Columns.Add("kgper件", typeof(String));
                dtsave.Columns.Add("数量Total", typeof(String));
                dtsave.Columns.Add("包装是否完好", typeof(String));
                dtsave.Columns.Add("是否清洁合格", typeof(String));
                dtsave.Columns.Add("操作人", typeof(String));
                dtsave.Columns.Add("复核人", typeof(String));

                foreach (var ss in jo)  //查找某个字段与值
                {
                    DataRow dr = dtsave.NewRow();
                    dr[0] = ss["s4_raw_material_delivery_date"].ToString();
                    dr[1] = ss["s4_time"].ToString();
                    dr[2] = ss["s4_raw_material_id"].ToString();
                    dr[3] = ss["s4_quantity"].ToString();
                    dr[4] = ss["s4_kilogram_per_piece"].ToString();
                    dr[5] = ss["s4_quantity_per_kilogram"].ToString();
                    dr[6] = ss["s4_is_packed_well"].ToString();
                    dr[7] = ss["s4_is_cleaned"].ToString();
                    dr[8] = ss["s4_operator_id"].ToString();
                    dr[9] = ss["s4_reviewer_id"].ToString();

                    dtsave.Rows.Add(dr);
                }
                //添加复核人id
                dtsave.Rows[dtsave.Rows.Count - 1][9] = review_id.ToString();

                //jason 保存表格
                JArray jarray = JArray.Parse("[]");
                for (int i = 0; i < dtsave.Rows.Count; i++)
                {
                    string json = @"{}";
                    JObject j = JObject.Parse(json);
                    j.Add("s4_raw_material_delivery_date", new JValue(dtsave.Rows[i][0].ToString()));
                    j.Add("s4_time", new JValue(dtsave.Rows[i][1].ToString()));
                    j.Add("s4_raw_material_id", new JValue(dtsave.Rows[i][2].ToString()));
                    j.Add("s4_quantity", new JValue(dtsave.Rows[i][3].ToString()));
                    j.Add("s4_kilogram_per_piece", new JValue(dtsave.Rows[i][4].ToString()));
                    j.Add("s4_quantity_per_kilogram", new JValue(dtsave.Rows[i][5].ToString()));
                    j.Add("s4_is_packed_well", new JValue(dtsave.Rows[i][6].ToString()));
                    j.Add("s4_is_cleaned", new JValue(dtsave.Rows[i][7].ToString()));
                    j.Add("s4_operator_id", new JValue(dtsave.Rows[i][8].ToString()));
                    j.Add("s4_reviewer_id", new JValue(dtsave.Rows[i][9].ToString()));
                    jarray.Add(j);
                }
                List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString() });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);

            }
            else
            {
                reviewer_name = checkIDOle(review_id);

                List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);

                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                //填数据

                DataTable dtsave = new DataTable();

                dtsave.Columns.Add("传料日期", typeof(String));
                dtsave.Columns.Add("时间", typeof(String));
                dtsave.Columns.Add("物料代码", typeof(String));
                dtsave.Columns.Add("数量", typeof(String));
                dtsave.Columns.Add("kgper件", typeof(String));
                dtsave.Columns.Add("数量Total", typeof(String));
                dtsave.Columns.Add("包装是否完好", typeof(String));
                dtsave.Columns.Add("是否清洁合格", typeof(String));
                dtsave.Columns.Add("操作人", typeof(String));
                dtsave.Columns.Add("复核人", typeof(String));

                foreach (var ss in jo)  //查找某个字段与值
                {
                    DataRow dr = dtsave.NewRow();
                    dr[0] = ss["s4_raw_material_delivery_date"].ToString();
                    dr[1] = ss["s4_time"].ToString();
                    dr[2] = ss["s4_raw_material_id"].ToString();
                    dr[3] = ss["s4_quantity"].ToString();
                    dr[4] = ss["s4_kilogram_per_piece"].ToString();
                    dr[5] = ss["s4_quantity_per_kilogram"].ToString();
                    dr[6] = ss["s4_is_packed_well"].ToString();
                    dr[7] = ss["s4_is_cleaned"].ToString();
                    dr[8] = ss["s4_operator_id"].ToString();
                    dr[9] = ss["s4_reviewer_id"].ToString();

                    dtsave.Rows.Add(dr);
                }
                //添加复核人id
                dtsave.Rows[dtsave.Rows.Count - 1][9] = review_id.ToString();

                //jason 保存表格
                JArray jarray = JArray.Parse("[]");
                for (int i = 0; i < dtsave.Rows.Count; i++)
                {
                    string json = @"{}";
                    JObject j = JObject.Parse(json);
                    j.Add("s4_raw_material_delivery_date", new JValue(dtsave.Rows[i][0].ToString()));
                    j.Add("s4_time", new JValue(dtsave.Rows[i][1].ToString()));
                    j.Add("s4_raw_material_id", new JValue(dtsave.Rows[i][2].ToString()));
                    j.Add("s4_quantity", new JValue(dtsave.Rows[i][3].ToString()));
                    j.Add("s4_kilogram_per_piece", new JValue(dtsave.Rows[i][4].ToString()));
                    j.Add("s4_quantity_per_kilogram", new JValue(dtsave.Rows[i][5].ToString()));
                    j.Add("s4_is_packed_well", new JValue(dtsave.Rows[i][6].ToString()));
                    j.Add("s4_is_cleaned", new JValue(dtsave.Rows[i][7].ToString()));
                    j.Add("s4_operator_id", new JValue(dtsave.Rows[i][8].ToString()));
                    j.Add("s4_reviewer_id", new JValue(dtsave.Rows[i][9].ToString()));
                    jarray.Add(j);
                }
                List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString() });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            CheckBtn.Enabled = false;
            SaveBtn.Enabled = false;
            AddLineBtn.Enabled = true;
            TransportRecordView.Rows[checknum].Cells["复核人"].Value = reviewer_name;
            checknum++;
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            
        }

        private void ExtructionTransportRecordStep4_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            writeDGVWidthToSetting(TransportRecordView);
        }

        private void TransportRecordView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(TransportRecordView);
                isFirstBind = false;
            }
        }
    }
}
