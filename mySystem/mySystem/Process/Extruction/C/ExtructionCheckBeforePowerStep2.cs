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

        private int checknum = 0;
        private bool[] checklist;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false; 

        public ExtructionCheckBeforePowerStep2(MainForm mainform): base(mainform)
        {
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

            //DataShow();

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
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count - 1; i++)
            {
                this.CheckBeforePowerView.Columns[i].ReadOnly = true;
            }
            this.CheckBeforePowerView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            ///***********************表头数据初始化************************///
            if (isSqlOk)
            {
                SqlCommand comm = new SqlCommand("select * from " + tableSel, conn);
                SqlDataAdapter daSql = new SqlDataAdapter(comm);
                daSql.Fill(dt_confirmarea);
                daSql.Dispose();
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand("select * from " + tableSel, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(cmd);
                daOle.Fill(dt_confirmarea);
                daOle.Dispose();
            }
            ///填写项目内容
            checknum = dt_confirmarea.Rows.Count;
            checklist = new bool[checknum];

            
            for (int i = 0; i < checknum; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in CheckBeforePowerView.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dt_confirmarea.Rows[i]["确认序号"].ToString(); //序号
                dr.Cells[1].Value = dt_confirmarea.Rows[i]["确认项目"].ToString(); //确认项目
                dr.Cells[2].Value = " "+dt_confirmarea.Rows[i]["确认内容"].ToString(); ; //确认内容
                dr.Cells[3].Value = true;
                CheckBeforePowerView.Rows.Add(dr);
            }

            PSLabel.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";
                                  
            this.recorderBox.Text = operator_name;
            operate_date = DateTime.Now.Date;
            recordTimePicker.Value = operate_date;
            review_date = DateTime.Now.Date;
            checkTimePicker.Value = review_date;            
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
            //jason 保存表格
            JArray jarray = JArray.Parse("[]");
            for (int i = 0; i < checknum; i++)
            {
                string json = @"{}";
                JObject j = JObject.Parse(json);
                j.Add("s2_item_qualified", new JValue(this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value.ToString() == "True" ? "1" : "0"));
                jarray.Add(j);
            }

            List<String> queryCols = new List<String>(new String[] { "s2_is_qualified", "s2_operator_id", "s2_operate_date" });
            List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString(),operator_id, recordTimePicker.Value });
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            //Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
            
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
            review_opinion = check.opinion;
            ischeckOk = check.ischeckOk;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();

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
                List<String> queryCols = new List<String>(new String[] { "s2_reviewer_id", "s2_review_date" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, checkTimePicker.Value });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
                reviewer_name = checkIDOle(review_id);
            }
            checkerBox.Text = reviewer_name;
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

        private void DataShow()
        {
             ///填写数据库的确认结果、用户id、审核人id
            if (isSqlOk)
            {
            }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "s2_is_qualified", "s2_operator_id", "s2_operate_date", "s2_reviewer_id", "s2_review_date" });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);

                //解析jason
                JArray jo = JArray.Parse(queryValsList[0][0].ToString());
                //填数据
                int i = 0;
                foreach (var ss in jo)  //查找某个字段与值
                {  checklist[i++] = ss["s2_item_qualified"].ToString() == "1" ? true : false; }

                operator_id = Convert.ToInt32(queryValsList[0][1]);
                operator_name = checkIDOle(operator_id);
                operate_date = Convert.ToDateTime(queryValsList[0][2]);
                review_id = Convert.ToInt32(queryValsList[0][3]);
                reviewer_name = checkIDOle(review_id);
                review_date = Convert.ToDateTime(queryValsList[0][4]);

                for (i = 0; i < checknum; i++)
                {  this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value = checklist[i];  }
            }

            this.recorderBox.Text = operator_name;
            recordTimePicker.Value = operate_date.Date;
            this.checkerBox.Text = reviewer_name; 
            checkTimePicker.Value = review_date;             
        }
    }
}
