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
    public partial class ExtructionCheckBeforePowerStep2 : BaseForm
    {
        private DataTable dt_confirmarea = new DataTable();

        private String table = "extrusion_s2_confirm";
        private String tableSel = "confirmarea";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;
        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false;
        private bool isSaveOk = false;

        //private int checknum = 0;
        //private bool[] checklist;


        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        //有信息的时候，并未修改operator_id、operator_name
        public ExtructionCheckBeforePowerStep2(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;
            
            Init();
            DataTabelInitialize();
            DataShow(Instructionid);
        }

        private void Init() 
        {
            //记录人初始化
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            this.recorderBox.Text = operator_name;
            operate_date = DateTime.Now.Date;
            recordTimePicker.Value = operate_date;
            //审核人初始化
            review_id = 0;
            reviewer_name = null;
            this.checkerBox.Text = reviewer_name;
            review_date = DateTime.Now.Date;
            checkTimePicker.Value = review_date;

            PSLabel.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";
        }

        private void DataShow(int InstructionID)
        {
            List<String> queryCols = new List<String>(new String[] { "s2_is_qualified", "s2_operator_id", "s2_operate_date", "s2_reviewer_id", "s2_review_date", "s2_is_review_qualified" });
            List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
            List<Object> whereVals = new List<Object>(new Object[] { InstructionID });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);

            if (queryValsList.Count == 0)
            {
                dt_confirmarea.Clear();
                CheckBeforePowerView.Rows.Clear();
                CheckBeforePowerView.ReadOnly = false;
                Init();
                ischeckOk = false;
                isSaveOk = false;
                ///***********************将设置界面的内容填入************************///
                OleDbCommand cmd = new OleDbCommand("select * from " + tableSel, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(cmd);
                daOle.Fill(dt_confirmarea);
                daOle.Dispose();
                ///填写项目内容

                for (int i = 0; i < dt_confirmarea.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in CheckBeforePowerView.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = dt_confirmarea.Rows[i]["确认序号"].ToString(); //序号
                    dr.Cells[1].Value = dt_confirmarea.Rows[i]["确认项目"].ToString(); //确认项目
                    dr.Cells[2].Value = " " + dt_confirmarea.Rows[i]["确认内容"].ToString(); ; //确认内容
                    dr.Cells[3].Value = true;
                    CheckBeforePowerView.Rows.Add(dr);
                }
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
            }
            else
            {
                dt_confirmarea.Clear();
                CheckBeforePowerView.Rows.Clear();
                isSaveOk = true;

                //记录人初始化
                operator_id = Convert.ToInt32(queryValsList[0][1].ToString());
                operator_name = Parameter.IDtoName(operator_id);
                this.recorderBox.Text = operator_name;
                operate_date = Convert.ToDateTime(queryValsList[0][2].ToString());
                recordTimePicker.Value = operate_date;
                //审核人初始化
                review_id = Convert.ToInt32(queryValsList[0][3].ToString());
                reviewer_name = Parameter.IDtoName(review_id);
                this.checkerBox.Text = reviewer_name;

                if (reviewer_name != null)
                {
                    review_date = Convert.ToDateTime(queryValsList[0][4].ToString());
                    checkTimePicker.Value = review_date;
                    ischeckOk = Convert.ToBoolean(queryValsList[0][5].ToString());
                    //审核通过，则确认、保存均不可点
                    if (ischeckOk)
                    {
                        SaveBtn.Enabled = false;
                        CheckBtn.Enabled = false;
                        printBtn.Enabled = true;
                        CheckBeforePowerView.ReadOnly = true;
                    }
                    else
                    {
                        SaveBtn.Enabled = true;
                        CheckBtn.Enabled = true;
                        printBtn.Enabled = false;
                        CheckBeforePowerView.ReadOnly = false;
                    }
                    printBtn.Enabled = true;
                    CheckBeforePowerView.Columns["确认结果"].ReadOnly = true;
                }
                else
                {
                    review_date = DateTime.Now.Date;
                    checkTimePicker.Value = review_date;
                    ischeckOk = false; 
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                } 

                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                int i = 0;
                foreach (var ss in jo)  //查找某个字段与值
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in CheckBeforePowerView.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = (i + 1).ToString(); //序号
                    dr.Cells[1].Value = ss["s2_确认项目"].ToString(); //确认项目
                    dr.Cells[2].Value = " " + ss["s2_确认内容"].ToString(); //确认内容
                    dr.Cells[3].Value = ss["s2_确认结果"].ToString() == "1" ? true : false;
                    CheckBeforePowerView.Rows.Add(dr);
                    i++;
                }
            }
        }
    
        private void DataTabelInitialize()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.CheckBeforePowerView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.CheckBeforePowerView.RowHeadersVisible = false;
            this.CheckBeforePowerView.AllowUserToResizeColumns = false;
            this.CheckBeforePowerView.AllowUserToResizeRows = false;
            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.CheckBeforePowerView.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count; i++)
            {
                this.CheckBeforePowerView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.CheckBeforePowerView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            this.CheckBeforePowerView.Columns[0].MinimumWidth = 80;
            this.CheckBeforePowerView.Columns[1].MinimumWidth = 160;
            this.CheckBeforePowerView.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.CheckBeforePowerView.Columns[3].MinimumWidth = 80;
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count - 2; i++)
            {
                this.CheckBeforePowerView.Columns[i].ReadOnly = true;
            }
            this.CheckBeforePowerView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;            
        }

        private void DataSaveSql()
        {
            int result = 0;
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            string commext = "update extrusion set ";
            for (int i = 0; i < 14; i++)
            { commext = commext + "s2_item" + (i + 1).ToString() + "_qualified = @s2_item" + (i + 1).ToString() + "_qualified, "; }
            commext = commext + "s2_operator_id=@s2_operator_id, s2_operate_date=@s2_operate_date, s2_reviewer_id=s2_reviewer_id, s2_review_date=s2_review_date where id =1";
            comm.CommandText = commext;
            for (int i = 0; i < 14; i++)
            {
                comm.Parameters.Add("@s2_item" + (i + 1).ToString() + "_qualified", System.Data.SqlDbType.Bit);
                comm.Parameters["@s2_item" + (i + 1).ToString() + "_qualified"].Value = (this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value.ToString() == "True" ? true : false);
            }
            comm.Parameters.Add("@s2_operator_id", System.Data.SqlDbType.Int);
            comm.Parameters.Add("@s2_operate_date", System.Data.SqlDbType.Date);
            comm.Parameters.Add("@s2_reviewer_id", System.Data.SqlDbType.Int);
            comm.Parameters.Add("@s2_review_date", System.Data.SqlDbType.Date);

            comm.Parameters["@s2_operator_id"].Value = operator_id;
            comm.Parameters["@s2_operate_date"].Value = recordTimePicker.Value;
            comm.Parameters["@s2_reviewer_id"].Value = review_id;
            comm.Parameters["@s2_review_date"].Value = checkTimePicker.Value;

            result = comm.ExecuteNonQuery();
            if (result > 0)
            { MessageBox.Show("添加成功"); }
            else { MessageBox.Show("错误"); }

            comm.Dispose();

        }

        private void DataSaveOle()
        {
            operator_name = recorderBox.Text;            
            if (operator_name != Parameter.IDtoName(operator_id))
            {
                operator_id = Parameter.NametoID(operator_name);
            }
            //jason 保存表格
            JArray jarray = JArray.Parse("[]");
            for (int i = 0; i < CheckBeforePowerView.Rows.Count; i++)
            {
                string json = @"{}";
                JObject j = JObject.Parse(json);
                j.Add("s2_确认项目", new JValue(this.CheckBeforePowerView.Rows[i].Cells["确认项目"].Value.ToString()));
                j.Add("s2_确认内容", new JValue(this.CheckBeforePowerView.Rows[i].Cells["确认内容"].Value.ToString()));
                j.Add("s2_确认结果", new JValue(this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value.ToString() == "True" ? "1" : "0"));
                jarray.Add(j);
            }

            //新建的
            if (isSaveOk == false)
            {
                List<String> queryCols = new List<String>(new String[] { "production_instruction_id" ,"s2_is_qualified", "s2_operator_id", "s2_operate_date" });
                List<Object> queryVals = new List<Object>(new Object[] { Instructionid, jarray.ToString(), operator_id, Convert.ToDateTime(recordTimePicker.Value.ToString("yyyy/MM/dd")) });
                Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
            }
            //已经有了
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s2_is_qualified", "s2_operator_id", "s2_operate_date" });
                List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(), operator_id, Convert.ToDateTime(recordTimePicker.Value.ToString("yyyy/MM/dd")) });                
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
                List<Object> whereVals = new List<Object>(new Object[] { Instructionid });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            CheckBtn.Enabled = true;            
        }

        private void CheckBeforePowerView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            {
                int result = 0;
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "update extrusion set s2_reviewer_id=@s2_reviewer_id, s2_review_date=@s2_review_date where id=1";

                comm.Parameters.Add("@s2_reviewer_id", System.Data.SqlDbType.Int);
                comm.Parameters.Add("@s2_review_date", System.Data.SqlDbType.Date);
                comm.Parameters["@s2_reviewer_id"].Value = review_id;
                comm.Parameters["@s2_review_date"].Value = checkTimePicker.Value;

                result = comm.ExecuteNonQuery();
                /*
                if (result > 0)
                { MessageBox.Show("添加成功"); }
                else { MessageBox.Show("错误"); }
                 * */
                comm.Dispose();
            }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s2_reviewer_id", "s2_review_date", "s2_review_opinion", "s2_is_review_qualified" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, Convert.ToDateTime(checkTimePicker.Value.ToString("yyyy/MM/dd")), review_opinion, ischeckOk });
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
                List<Object> whereVals = new List<Object>(new Object[] { Instructionid });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);                
            }
            if (ischeckOk)
            {
                CheckBeforePowerView.ReadOnly = true;
                SaveBtn.Enabled = false;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
            }
            checkerBox.Text = reviewer_name;            
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
        }
               
    }
}
