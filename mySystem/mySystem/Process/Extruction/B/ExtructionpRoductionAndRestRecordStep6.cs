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
    public partial class ExtructionpRoductionAndRestRecordStep6 : BaseForm
    {        
        private DataTable dtInformation = new DataTable();
        //private DataTable dtRecord = new DataTable();

        private String table = "extrusion_s6_production_check";
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int checknum = 0;

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

        private CheckForm check = null;
        
        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            if (isSqlOk) { operator_name = checkIDSql(operator_id); }
            else { operator_name = checkIDOle(operator_id); }

            RecordViewInitialize();

            AddLineBtn.Enabled = false;
            CheckBtn.Enabled = false;
        }

        private void RecordViewInitialize()
        {
            this.DatecheckBox.Checked = spot;
            this.NeightcheckBox.Checked = !spot;
            dateTimePicker1.Value = DateTime.Now;

            //添加内容
            AddRecordRowLine(); 
            this.RecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.RecordView.AllowUserToAddRows = false;
            this.RecordView.Columns["膜卷编号"].HeaderText = "膜卷编号\r(卷)";
            this.RecordView.Columns["膜卷长度"].HeaderText = "膜卷长度\r(m)";
            this.RecordView.Columns["膜卷重量"].HeaderText = "膜卷重量\r(kg)";
            this.RecordView.Columns["宽度"].HeaderText = "宽度\r(mm)";
            this.RecordView.Columns["最大厚度"].HeaderText = "最大厚度\r（μm）";
            this.RecordView.Columns["最小厚度"].HeaderText = "最小厚度\r（μm）";
            this.RecordView.Columns["平均厚度"].HeaderText = "平均厚度\r（μm）";
            this.RecordView.Columns["厚度公差"].HeaderText = "厚度公差\r(%)";
            //添加按钮列
            /*
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.RecordView.Columns.Add(MyButtonColumn);
            this.RecordView.AllowUserToAddRows = false;*/
            //设置对齐
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].MinimumWidth = 80;
            }
            this.RecordView.Columns[0].MinimumWidth = 40;
            this.RecordView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.ColumnHeadersHeight = 40;
            
        }

        private void AddRecordRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in RecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = (Recordnum + 1).ToString();
            dr.Cells[1].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = operator_name;
            dr.Cells[6].Value = true; //"外观"
            dr.Cells[7].Value = ""; 
            dr.Cells[8].Value = ""; 
            dr.Cells[9].Value = ""; 
            dr.Cells[10].Value = "";
            dr.Cells[11].Value = "";
            dr.Cells[12].Value = ""; //检查人
            dr.Cells[13].Value = true;
            RecordView.Rows.Insert(Recordnum,dr);
            if (Recordnum == 0)
            {
                AddTotalLine();
            }
            Recordnum = Recordnum + 1;
        }

        //添加最后一行
        private void AddTotalLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in RecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = "";
            dr.Cells[1].Value = "总计";
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = true; //"外观"
            dr.Cells[7].Value = "";
            dr.Cells[8].Value = "";
            dr.Cells[9].Value = "";
            dr.Cells[10].Value = "";
            dr.Cells[11].Value = "";
            dr.Cells[12].Value = ""; //检查人
            dr.Cells[13].Value = true;
            RecordView.Rows.Add(dr);

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
            AddLineBtn.Enabled = false;
            SaveBtn.Enabled = true;
            DelLineBtn.Enabled = true;
        }

        //保存数据
        public void DataSaveSql()
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

        public void DataSaveOle()
        {
            int numtemp;
            if (Int32.TryParse((temperatureBox.Text.ToString()), out numtemp) == false)
            { MessageBox.Show("请在温度框内填入数字!"); }
            else if(Int32.TryParse((humidityBox.Text.ToString()), out numtemp) == false)
            { MessageBox.Show("请在湿度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["膜卷编号"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷编号框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["膜卷长度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷长度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["膜卷重量"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷重量框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["宽度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在宽度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["最大厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在最大厚度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["最小厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在最小厚度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["平均厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在平均厚度框内填入数字!"); }
            else if (Int32.TryParse((RecordView.Rows[checknum].Cells["厚度公差"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在厚度公差框内填入数字!"); }
            else
            {
                if (checknum == 0)
                {
                    //jason 保存表格
                    JArray jarray = JArray.Parse("[]");
                    for (int i = 0; i < RecordView.Rows.Count-1; i++)
                    {
                        string json = @"{}";
                        JObject j = JObject.Parse(json);
                        j.Add("s6_time", new JValue(Convert.ToDateTime(RecordView.Rows[i].Cells["时间"].Value.ToString())));
                        j.Add("s6_mojuan_number", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["膜卷编号"].Value.ToString())));
                        j.Add("s6_mojuan_length", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["膜卷长度"].Value.ToString())));
                        j.Add("s6_mojuan_weight", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["膜卷重量"].Value.ToString())));
                        j.Add("s6_recorder_id", new JValue(Convert.ToInt32(operator_id.ToString())));
                        j.Add("s6_outward", new JValue(RecordView.Rows[i].Cells["外观"].Value.ToString() == "True" ? "1" : "0"));
                        j.Add("s6_width", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["宽度"].Value.ToString())));
                        j.Add("s6_max_thickness", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["最大厚度"].Value.ToString())));
                        j.Add("s6_min_thickness", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["最小厚度"].Value.ToString())));
                        j.Add("s6_aver_thickness", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["平均厚度"].Value.ToString())));
                        j.Add("s6_tolerance_thickness", new JValue(Convert.ToInt32(RecordView.Rows[i].Cells["厚度公差"].Value.ToString())));
                        j.Add("s6_checker_id", new JValue(""));
                        j.Add("s6_is_qualified", new JValue(RecordView.Rows[i].Cells["判定"].Value.ToString() == "True" ? "1" : "0"));
                        jarray.Add(j);
                    }
                    List<String> queryCols = new List<String>(new String[] { "s6_review_opinion", "s6_temperature", "s6_relative_humidity" });
                    List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), temperatureBox.Text.ToString(), humidityBox.Text.ToString() });
                    List<String> whereCols = new List<String>(new String[] { "id" });
                    List<Object> whereVals = new List<Object>(new Object[] { 1 });
                    //Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
                    Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                }
                else
                {
                    List<String> queryCols = new List<String>(new String[] { "s6_review_opinion" });
                    List<String> whereCols = new List<String>(new String[] { "id" });
                    List<Object> whereVals = new List<Object>(new Object[] { 1 });
                    List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                    //解析jason
                    JArray jo = JArray.Parse(queryValsList[0][0].ToString());

                    int rownum = RecordView.Rows.Count-1;

                    string json = @"{}";
                    JObject j = JObject.Parse(json);
                    j.Add("s6_time", new JValue(Convert.ToDateTime(RecordView.Rows[rownum - 1].Cells["时间"].Value.ToString())));
                    j.Add("s6_mojuan_number", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["膜卷编号"].Value.ToString())));
                    j.Add("s6_mojuan_length", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["膜卷长度"].Value.ToString())));
                    j.Add("s6_mojuan_weight", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["膜卷重量"].Value.ToString())));
                    j.Add("s6_recorder_id", new JValue(Convert.ToInt32(operator_id.ToString())));
                    j.Add("s6_outward", new JValue(RecordView.Rows[rownum - 1].Cells["外观"].Value.ToString() == "True" ? "1" : "0"));
                    j.Add("s6_width", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["宽度"].Value.ToString())));
                    j.Add("s6_max_thickness", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["最大厚度"].Value.ToString())));
                    j.Add("s6_min_thickness", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["最小厚度"].Value.ToString())));
                    j.Add("s6_aver_thickness", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["平均厚度"].Value.ToString())));
                    j.Add("s6_tolerance_thickness", new JValue(Convert.ToInt32(RecordView.Rows[rownum - 1].Cells["厚度公差"].Value.ToString())));
                    j.Add("s6_checker_id", new JValue(""));
                    j.Add("s6_is_qualified", new JValue(RecordView.Rows[rownum - 1].Cells["判定"].Value.ToString() == "True" ? "1" : "0"));
                    jo.Add(j);

                    queryCols = new List<String>(new String[] { "s6_review_opinion", "s6_temperature", "s6_relative_humidity" });
                    List<Object> queryVals = new List<Object>(new Object[] { jo.ToString(), temperatureBox.Text.ToString(), humidityBox.Text.ToString() });
                    Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                }
            }
        }

        public void DataShow()
        {
            /*
            //若已有数据，向内部添加现有数据
            string sql = "Select * From extrusion";
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
             * */
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
            if (RecordView.Rows.Count >= 3)
            {
                int deletenum = RecordView.CurrentRow.Index;
                if (deletenum == RecordView.Rows.Count - 2)
                {
                    this.RecordView.Rows.RemoveAt(deletenum);
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = false;
                    SaveBtn.Enabled = false;
                    Recordnum = Recordnum - 1;
                }
            }             
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSqlOk)
            { DataSaveSql(); }
            else
            { DataSaveOle(); }
            CheckBtn.Enabled = true;
            DelLineBtn.Enabled = false;
        }

        public override void CheckResult()
        {
            base.CheckResult();
            CheckerBox.Text = check.opinion;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
            /*
             * CheckForm check = new CheckForm(base.mainform);
            check.ShowDialog();
            review_id = check.userID;

            if (isSqlOk)
            { }
            else
            {
                reviewer_name = checkIDOle(review_id);

                List<String> queryCols = new List<String>(new String[] { "s6_review_opinion" });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                //填数据

                DataTable dtsave = new DataTable();

                dtsave.Columns.Add("0时间", typeof(String));
                dtsave.Columns.Add("1膜卷编号", typeof(String));
                dtsave.Columns.Add("2膜卷长度", typeof(String));
                dtsave.Columns.Add("3膜卷重量", typeof(String));
                dtsave.Columns.Add("4记录人", typeof(String));
                dtsave.Columns.Add("5外观", typeof(String));
                dtsave.Columns.Add("6宽度", typeof(String));
                dtsave.Columns.Add("7最大厚度", typeof(String));
                dtsave.Columns.Add("8最小厚度", typeof(String));
                dtsave.Columns.Add("9平均厚度", typeof(String));
                dtsave.Columns.Add("10厚度公差", typeof(String));
                dtsave.Columns.Add("11检查人", typeof(String));
                dtsave.Columns.Add("12判定", typeof(String));

                foreach (var ss in jo)  //查找某个字段与值
                {
                    DataRow dr = dtsave.NewRow();
                    dr[0] = ss["s6_time"].ToString();
                    dr[1] = ss["s6_mojuan_number"].ToString();
                    dr[2] = ss["s6_mojuan_length"].ToString();
                    dr[3] = ss["s6_mojuan_weight"].ToString();
                    dr[4] = ss["s6_recorder_id"].ToString();
                    dr[5] = ss["s6_outward"].ToString();
                    dr[6] = ss["s6_width"].ToString();
                    dr[7] = ss["s6_max_thickness"].ToString();
                    dr[8] = ss["s6_min_thickness"].ToString();
                    dr[9] = ss["s6_aver_thickness"].ToString();
                    dr[10] = ss["s6_tolerance_thickness"].ToString();
                    dr[11] = ss["s6_checker_id"].ToString();
                    dr[12] = ss["s6_is_qualified"].ToString();
                    dtsave.Rows.Add(dr);
                }

                //添加复核人id
                dtsave.Rows[dtsave.Rows.Count - 1][11] = review_id.ToString();

                //jason 保存表格
                JArray jarray = JArray.Parse("[]");
                for (int i = 0; i < dtsave.Rows.Count; i++)
                {
                    string json = @"{}";
                    JObject j = JObject.Parse(json);

                    j.Add("s6_time", new JValue(dtsave.Rows[i][0].ToString()));
                    j.Add("s6_mojuan_number", new JValue(dtsave.Rows[i][1].ToString()));
                    j.Add("s6_mojuan_length", new JValue(dtsave.Rows[i][2].ToString()));
                    j.Add("s6_mojuan_weight", new JValue(dtsave.Rows[i][3].ToString()));
                    j.Add("s6_recorder_id", new JValue(dtsave.Rows[i][4].ToString()));
                    j.Add("s6_outward", new JValue(dtsave.Rows[i][5].ToString()));
                    j.Add("s6_width", new JValue(dtsave.Rows[i][6].ToString()));
                    j.Add("s6_max_thickness", new JValue(dtsave.Rows[i][7].ToString()));
                    j.Add("s6_min_thickness", new JValue(dtsave.Rows[i][8].ToString()));
                    j.Add("s6_aver_thickness", new JValue(dtsave.Rows[i][9].ToString()));
                    j.Add("s6_tolerance_thickness", new JValue(dtsave.Rows[i][10].ToString()));
                    j.Add("s6_checker_id", new JValue(dtsave.Rows[i][11].ToString()));
                    j.Add("s6_is_qualified", new JValue(dtsave.Rows[i][12].ToString()));
                    jarray.Add(j);
                }
                List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString() });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            CheckBtn.Enabled = false;
            SaveBtn.Enabled = false;
            AddLineBtn.Enabled = true;
            RecordView.Rows[checknum].Cells["检查人"].Value = reviewer_name;
            CheckerBox.Text = reviewer_name;
            checknum++;
             * */
        }        
    }
}
