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
        private int Instructionid;

        private int operator_id;
        private string operator_name;
        private int review_id;
        private string reviewer_name;        

        private int Recordnum = 0;
        private bool spot = true;  //班次；true白班；false夜班

        private int[] sum = { 0, 0 };

        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false;

        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;
            spot = true; //需要从Parameter里面读取，暂无
            

            Init();

        }

        private void Init()
        {
            //产品名称初始化
            if (!isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select product_name from production_instruction";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productnamecomboBox.Items.Add(reader["product_name"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select product_name from production_instruction";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productnamecomboBox.Items.Add(reader["product_name"]);
                    }
                }
            }
            productnamecomboBox.SelectedIndex = 0;
            //班次初始化
            this.DatecheckBox.Checked = spot;
            this.NightcheckBox.Checked = !spot;
            //生产日期初始化
            productionDatePicker.Value = DateTime.Now.Date;
        }

        private void RecordViewInitialize()
        {
            //添加内容
            //AddRecordRowLine();
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

            RecordView.Columns["序号"].ReadOnly = true;
            RecordView.Columns["时间"].ReadOnly = true;
            RecordView.Columns["记录人"].ReadOnly = true;
            RecordView.Columns["检查人"].ReadOnly = true;

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
            dr.Cells[12].Value = operator_name; //检查人
            dr.Cells[13].Value = true;
            RecordView.Rows.Insert(Recordnum, dr);
            if (Recordnum == 0)
            {
                AddTotalLine();
            }
            Recordnum++;
            RecordView.Rows[Recordnum-1].ReadOnly = false;
            RecordView.Rows[Recordnum].ReadOnly = true;
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
            dr.Cells[2].Value = "-";
            dr.Cells[3].Value = sum[0].ToString(); ;
            dr.Cells[4].Value = sum[1].ToString(); ;
            dr.Cells[5].Value = "-";
            dr.Cells[6].Value = true; //"外观"
            dr.Cells[7].Value = "-";
            dr.Cells[8].Value = "-";
            dr.Cells[9].Value = "-";
            dr.Cells[10].Value = "-";
            dr.Cells[11].Value = "-";
            dr.Cells[12].Value = "-"; //检查人
            dr.Cells[13].Value = true;
            RecordView.Rows.Add(dr);
        }

        //删除单条记录
        private void RecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //之前添加ButtonColumn
            /*
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
            }*/
            if (RecordView.Columns[e.ColumnIndex].Name == "膜卷长度")
            {
                int numtemp;
                if (Int32.TryParse((RecordView.Rows[Recordnum-1].Cells["膜卷长度"].Value.ToString()), out numtemp) == true)
                {
                    for (int i = 0; i < Recordnum; i++)
                        sum[0] += Convert.ToInt32((RecordView.Rows[i].Cells["膜卷长度"].Value.ToString()));
                }
            }
            else if (RecordView.Columns[e.ColumnIndex].Name == "膜卷重量")
            {
                int numtemp;
                if (Int32.TryParse((RecordView.Rows[Recordnum-1].Cells["膜卷重量"].Value.ToString()), out numtemp) == true)
                {
                    for (int i = 0; i < Recordnum; i++)
                        sum[1] += Convert.ToInt32((RecordView.Rows[i].Cells["膜卷重量"].Value.ToString()));
                }
            }
            else
            { }
        }

        //更新序号
        private void RefreshNum()
        {
            for (int i = 0; i < RecordView.Rows.Count - 1; i++)
            {
                RecordView.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }

        //日班改变时，夜班也随之改变
        private void DatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.DatecheckBox.Checked = spot;
            /*
            if (DatecheckBox.Checked)
            {
                NightcheckBox.Checked = false;
                DatecheckBox.Checked = true;
                spot = true;
            }*/
        }

        //夜班改变时，夜班也随之改变
        private void NeightcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.NightcheckBox.Checked = !spot;
            /*
            if (NightcheckBox.Checked)
            {
                DatecheckBox.Checked = false;
                NightcheckBox.Checked = true;
                spot = false;
            }*/
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
        public bool DataSaveSql()
        {
            /*
            string[] sqlstr = new string[17];
            SqlCommand com = null;

            sqlstr[0] = "update extrusion set product_name = '" + this.batchIdBox.Text + "' where id =1";
            sqlstr[1] = "update extrusion set product_batch = '" + this.instructionIdBox.Text + "' where id =1";
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
             * */
            return true;
        }

        public bool DataSaveOle()
        {
            int numtemp;           
            //if (batchIdBox.Text.ToString() == "")
            //{ MessageBox.Show("请在产品名称框内输入内容!"); return false; }
            if (batchIdBox.Text.ToString() == "")
            { MessageBox.Show("请在产品批号框内输入内容!"); return false; }
            else if (Int32.TryParse((temperatureBox.Text.ToString()), out numtemp) == false)
            { MessageBox.Show("请在温度框内填入数字!"); return false; }
            else if (Int32.TryParse((humidityBox.Text.ToString()), out numtemp) == false)
            { MessageBox.Show("请在湿度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum-1].Cells["膜卷编号"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷编号框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["膜卷长度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷长度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["膜卷重量"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在膜卷重量框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["宽度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在宽度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["最大厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在最大厚度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["最小厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在最小厚度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["平均厚度"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在平均厚度框内填入数字!"); return false; }
            else if (Int32.TryParse((RecordView.Rows[Recordnum - 1].Cells["厚度公差"].Value.ToString()), out numtemp) == false)
            { MessageBox.Show("请在厚度公差框内填入数字!"); return false; }
            else
            {
                //存储jason块的形式 
                if (Recordnum - 1 == 0)
                {
                    //jason 保存表格
                    JArray jarray = JArray.Parse("[]");
                    for (int i = 0; i < RecordView.Rows.Count - 1; i++)
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
                        j.Add("s6_checker_id", new JValue(Convert.ToInt32(operator_id.ToString())));
                        j.Add("s6_is_qualified", new JValue(RecordView.Rows[i].Cells["判定"].Value.ToString() == "True" ? "1" : "0"));
                        jarray.Add(j);
                    }
                    List<String> queryCols = new List<String>(new String[] { "s6_check_info", "product_name", "product_batch_id", "production_instruction_id", "s6_temperature", "s6_relative_humidity", "s6_production_date", "s6_flight" });
                    //List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), Convert.ToInt32(batchIdBox.Text.ToString()), Convert.ToInt32(instructionIdBox.Text.ToString()), Convert.ToInt32(temperatureBox.Text.ToString()),Convert.ToInt32(humidityBox.Text.ToString()), productiondate.Date, DatecheckBox.Checked });
                    List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), productnamecomboBox.Text.ToString(), Convert.ToInt32(batchIdBox.Text.ToString()), Instructionid, Convert.ToInt32(temperatureBox.Text.ToString()), Convert.ToInt32(humidityBox.Text.ToString()), Convert.ToDateTime(productionDatePicker.Value.ToString("yyyy/MM/dd")), DatecheckBox.Checked });
                    Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
                    return true;
                }
                else
                {
                    List<String> queryCols = new List<String>(new String[] { "s6_check_info" });
                    List<String> whereCols = new List<String>(new String[] { "product_name", "s6_production_date", "s6_flight" });
                    //List<Object> whereVals = new List<Object>(new Object[] { Convert.ToInt32(batchIdBox.Text.ToString()), Convert.ToInt32(instructionIdBox.Text.ToString()), productiondate.Date, DatecheckBox.Checked });
                    List<Object> whereVals = new List<Object>(new Object[] { productnamecomboBox.Text.ToString(), Convert.ToDateTime(productionDatePicker.Value.ToString("yyyy/MM/dd")), DatecheckBox.Checked });                    
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
                    DataRow dr = dtsave.NewRow();
                    foreach (var ss in jo)  //查找某个字段与值
                    {
                        dr = dtsave.NewRow();
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

                    //检查保存的行是否已经保存过一次
                    if (dtsave.Rows.Count == Recordnum)
                    { dtsave.Rows[Recordnum - 1].Delete(); }

                    //存入最后一行
                    dr = dtsave.NewRow();
                    dr[0] = Convert.ToDateTime(RecordView.Rows[Recordnum - 1].Cells["时间"].Value.ToString());
                    dr[1] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["膜卷编号"].Value.ToString());
                    dr[2] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["膜卷长度"].Value.ToString());
                    dr[3] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["膜卷重量"].Value.ToString());
                    dr[4] = Convert.ToInt32(operator_id.ToString());
                    dr[5] = RecordView.Rows[Recordnum - 1].Cells["外观"].Value.ToString() == "True" ? "1" : "0";
                    dr[6] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["宽度"].Value.ToString());
                    dr[7] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["最大厚度"].Value.ToString());
                    dr[8] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["最小厚度"].Value.ToString());
                    dr[9] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["平均厚度"].Value.ToString());
                    dr[10] = Convert.ToInt32(RecordView.Rows[Recordnum - 1].Cells["厚度公差"].Value.ToString());
                    dr[11] = Convert.ToInt32(operator_id.ToString());
                    dr[12] = RecordView.Rows[Recordnum - 1].Cells["判定"].Value.ToString() == "True" ? "1" : "0";
                    dtsave.Rows.Add(dr);

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
                    queryCols = new List<String>(new String[] { "s6_check_info", "s6_temperature", "s6_relative_humidity" });
                    List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), temperatureBox.Text.ToString(), humidityBox.Text.ToString() });
                    Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);                                       

                    return true;
                }
            }
        }
        
        //显示数据
        public void DataShow(String Production_name, DateTime searchDate, Boolean searchFlight)
        {
            List<String> queryCols = new List<String>(new String[] { "s6_check_info", "product_batch_id", "s6_temperature", "s6_relative_humidity", "s6_reviewer_id" });
            //List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), Convert.ToInt32(batchIdBox.Text.ToString()), Convert.ToInt32(instructionIdBox.Text.ToString()), Convert.ToInt32(temperatureBox.Text.ToString()), Convert.ToInt32(humidityBox.Text.ToString()) });
            //List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), instructionIdBox.SelectedText.ToString(), Convert.ToInt32(instructionIdBox.Text.ToString()), Convert.ToInt32(temperatureBox.Text.ToString()), Convert.ToInt32(humidityBox.Text.ToString()) });

            List<String> whereCols = new List<String>(new String[] { "product_name", "s6_production_date", "s6_flight" });
            //List<Object> whereVals = new List<Object>(new Object[] { Production_name, searchDate.Date, searchFlight });
            List<Object> whereVals = new List<Object>(new Object[] { Production_name, Convert.ToDateTime(searchDate.Date.ToString("yyyy/MM/dd")), searchFlight });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);

            if (queryValsList.Count == 0)
            {
                RecordViewInitialize();
                RecordView.Rows.Clear();
                Recordnum = 0;
                AddRecordRowLine();

                batchIdBox.Text = "";
                temperatureBox.Text = "";
                humidityBox.Text = "";
                CheckerBox.Text = "";

                AddLineBtn.Enabled = false;
                DelLineBtn.Enabled = false;
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;
            }
            else
            {
                Recordnum = 0;
                RecordViewInitialize();
                RecordView.Rows.Clear();

                //显示查询到的信息                
                batchIdBox.Text = queryValsList[0][1].ToString();
                temperatureBox.Text = queryValsList[0][2].ToString();
                humidityBox.Text = queryValsList[0][3].ToString();
                CheckerBox.Text = Parameter.IDtoName(Convert.ToInt32(queryValsList[0][4].ToString()));

                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                DataGridViewRow dr = new DataGridViewRow();

                foreach (var ss in jo)  //查找某个字段与值
                {
                    dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in RecordView.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = (Recordnum + 1).ToString();
                    DateTime timetemp = Convert.ToDateTime(ss["s6_time"].ToString());
                    dr.Cells[1].Value = timetemp.Hour.ToString() + ":" + timetemp.Minute.ToString() + ":" + timetemp.Second.ToString();
                    dr.Cells[2].Value = ss["s6_mojuan_number"].ToString();
                    dr.Cells[3].Value = ss["s6_mojuan_length"].ToString();
                    dr.Cells[4].Value = ss["s6_mojuan_weight"].ToString();
                    dr.Cells[5].Value = ss["s6_recorder_id"].ToString();
                    dr.Cells[6].Value = (ss["s6_outward"].ToString()=="1"?true:false);
                    dr.Cells[7].Value = ss["s6_width"].ToString();
                    dr.Cells[8].Value = ss["s6_max_thickness"].ToString();
                    dr.Cells[9].Value = ss["s6_min_thickness"].ToString();
                    dr.Cells[10].Value = ss["s6_aver_thickness"].ToString();
                    dr.Cells[11].Value = ss["s6_tolerance_thickness"].ToString();
                    dr.Cells[12].Value = ss["s6_checker_id"].ToString();
                    dr.Cells[13].Value = (ss["s6_is_qualified"].ToString()=="1"?true:false);
                    RecordView.Rows.Insert(Recordnum, dr);                    
                    if (Recordnum == 0)
                    {
                        AddTotalLine();
                    }
                    Recordnum++;
                    RecordView.Rows[Recordnum - 1].ReadOnly = true;
                }
                Recordnum = RecordView.RowCount - 1;
                RecordView.Rows[Recordnum].ReadOnly = true;
                AddLineBtn.Enabled = true;
                DelLineBtn.Enabled = false;
                CheckBtn.Enabled = false;
                SaveBtn.Enabled = false;
                printBtn.Enabled = false;                
            }
        }

        /*
        private string checkID(int userID)
        {
            if (isSqlOk)
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
            else
            {
                List<String> queryCols = new List<String>(new String[] { "user_name" });
                List<String> whereCols = new List<String>(new String[] { "user_id" });
                List<Object> whereVals = new List<Object>(new Object[] { userID });
                List<List<Object>> queryValsList = Utility.selectAccess(connOle, "user_aoxing", queryCols, whereCols, whereVals, null, null, null, null, null);
                string user = queryValsList[0][0].ToString();
                return user;
            }
        }

        private bool checkFlight(int userID)
        {
            if (isSqlOk)
            {
                string Fightuser = null;
                string searchsql = "select * from user_aoxing where user_id='" + userID + "'";
                SqlCommand comm = new SqlCommand(searchsql, conn);
                SqlDataReader myReader = comm.ExecuteReader();
                while (myReader.Read())
                {
                    Fightuser = myReader.GetString(9);
                }

                myReader.Close();
                comm.Dispose();
                return (Fightuser == "1" ? true : false);
            }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "flight" });
                List<String> whereCols = new List<String>(new String[] { "user_id" });
                List<Object> whereVals = new List<Object>(new Object[] { userID });
                List<List<Object>> queryValsList = Utility.selectAccess(connOle, "user_aoxing", queryCols, whereCols, whereVals, null, null, null, null, null);
                string Fightuser = queryValsList[0][0].ToString();
                return (Fightuser == "1" ? true : false);
            }

        }
        */

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
                    Recordnum--;
                    RecordView.Rows[Recordnum].ReadOnly = true;
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSqlOk)
            {
                bool b = DataSaveSql();
                if (b)
                {
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = false;
                    RecordView.Rows[Recordnum - 1].ReadOnly = true;
                }
            }
            else
            {
                bool b = DataSaveOle();
                if (b)
                {
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = false;
                    RecordView.Rows[Recordnum-1].ReadOnly = true;
                }
            }
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
            check.Show();            

            if (isSqlOk)
            { }
            else
            {
                //暂时没有name返回

                reviewer_name = Parameter.IDtoName(review_id);

                List<String> queryCols = new List<String>(new String[] { "s6_reviewer_id", "s6_review_opinion", "s6_is_review_qualified" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, review_opinion, ischeckOk });
                List<String> whereCols = new List<String>(new String[] { "product_name", "s6_production_date", "s6_flight" });
                //List<Object> whereVals = new List<Object>(new Object[] { Convert.ToInt32(batchIdBox.Text.ToString()), Convert.ToInt32(instructionIdBox.Text.ToString()), productiondate.Date, DatecheckBox.Checked });
                List<Object> whereVals = new List<Object>(new Object[] { productnamecomboBox.Text.ToString(), Convert.ToDateTime(productionDatePicker.Value.ToString("yyyy/MM/dd")), DatecheckBox.Checked });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);                
            }
            CheckerBox.Text = reviewer_name;
            printBtn.Enabled = true;
           
        }

        private void productnamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataShow(productnamecomboBox.Text.ToString(), productionDatePicker.Value, DatecheckBox.Checked);
        }

        private void productionDatePicker_ValueChanged(object sender, EventArgs e)
        {
            DataShow(productnamecomboBox.Text.ToString(), productionDatePicker.Value, DatecheckBox.Checked);
        }
    }
}
