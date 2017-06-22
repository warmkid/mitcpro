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
    public partial class ExtructionCheckBeforePowerStep2 : BaseForm
    {
        private DataTable dt_confirmarea = new DataTable();

        private string sql = "Select * From extrusion";
        private string sqlSel = "select * from confirmarea";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int num = 0;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;


        public ExtructionCheckBeforePowerStep2(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            DataTabelInitialize();

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
                SqlCommand comm = new SqlCommand(sqlSel, conn);
                SqlDataAdapter daSql = new SqlDataAdapter(comm);
                daSql.Fill(dt_confirmarea);
                daSql.Dispose();
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(sqlSel, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(cmd);
                daOle.Fill(dt_confirmarea);
                daOle.Dispose();
            }
            ///填写项目内容
            num = dt_confirmarea.Rows.Count + 1;
            //临时保护措施，防止项目数目太多
            if (num > 14)
            { num = 14; }
            for (int i = 0; i < num; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in CheckBeforePowerView.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dt_confirmarea.Rows[i]["确认序号"].ToString(); //序号
                dr.Cells[1].Value = dt_confirmarea.Rows[i]["确认项目"].ToString(); //确认项目
                dr.Cells[2].Value = "  " + dt_confirmarea.Rows[i]["确认内容"].ToString(); //确认内容
                CheckBeforePowerView.Rows.Add(dr);
            }
            
            ///填写数据库的确认结果、用户id、审核人id
            if (isSqlOk)
            {
                //若已有数据，向内部添加现有数据
                SqlCommand commSQL = new SqlCommand(sql, conn);
                SqlDataAdapter daSQL = new SqlDataAdapter(commSQL);
                DataTable dtSQL = new DataTable();
                daSQL.Fill(dtSQL);

                for (int i = 0; i < num; i++)
                {
                    string qualified_string = "s2_item" + (i+1).ToString() + "_qualified";
                    bool qualified_bool = bool.Parse(dtSQL.Rows[0][qualified_string].ToString());
                    this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value = qualified_bool;
                }
                operator_id = Convert.ToInt32(dtSQL.Rows[0]["s2_operator_id"]);
                operator_name = checkIDSQL(operator_id);
                operate_date = Convert.ToDateTime(dtSQL.Rows[0]["s2_operate_date"].ToString());
                review_id = Convert.ToInt32(dtSQL.Rows[0]["s2_reviewer_id"]);
                reviewer_name = checkIDSQL(review_id);
                review_date = Convert.ToDateTime(dtSQL.Rows[0]["s2_review_date"].ToString());
                commSQL.Dispose();
                daSQL.Dispose();
                dtSQL.Dispose();
            }
            else
            {
                //若已有数据，向内部添加现有数据
                OleDbCommand commOle = new OleDbCommand(sql, connOle);
                OleDbDataAdapter daOle = new OleDbDataAdapter(commOle);
                DataTable dtOle = new DataTable();
                daOle.Fill(dtOle);
                for (int i = 0; i < num; i++)
                {
                    string qualified_string = "s2_item" + (i+1).ToString() + "_qualified";
                    bool qualified_bool = (dtOle.Rows[0][qualified_string].ToString() == "1" ? true : false);
                    this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value = qualified_bool;
                }
                operator_name = checkIDOle(operator_id);
                operate_date = Convert.ToDateTime(dtOle.Rows[0]["s2_operate_date"].ToString());
                review_id = Convert.ToInt32(dtOle.Rows[0]["s2_reviewer_id"]);
                reviewer_name = checkIDOle(review_id);
                review_date = Convert.ToDateTime(dtOle.Rows[0]["s2_review_date"].ToString());

                commOle.Dispose();
                daOle.Dispose();
                dtOle.Dispose();
            }
            
            PSLabel.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";
            recordTimePicker.Value = operate_date;
            this.recorderBox.Text = operator_name;
            checkTimePicker.Value = review_date;
            this.checkerBox.Text = reviewer_name;

        }

        private void DataSaveSQL()
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

        private void DataSaveOleDb()
        {

            List<String> queryCols = new List<String>(new String[] { "s2_operator_id", "s2_operate_date"});
            for (int i = 0; i < 14; i++)
            { queryCols.Add("s2_item" + (i + 1).ToString() + "_qualified"); }

            List<Object> queryVals = new List<Object>(new Object[] { operator_id, recordTimePicker.Value});
            for (int i = 0; i < 14; i++)
            { queryVals.Add(this.CheckBeforePowerView.Rows[i].Cells["确认结果"].Value.ToString() == "True" ? "1" : "0"); }

            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            String tblName = "extrusion";
            Boolean b = updateAccessOle(connOle, tblName, queryCols, queryVals, whereCols, whereVals);
        }

        private void CheckBeforePowerView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            
            if (isSqlOk) { DataSaveSQL(); }
            else { DataSaveOleDb(); }
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            CheckForm check = new CheckForm(base.mainform);
            check.ShowDialog();
            review_id = check.userID;
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
                String tblName = "extrusion";
                Boolean b = updateAccessOle(connOle, tblName, queryCols, queryVals, whereCols, whereVals);

                /*
                if (b==true)
                { MessageBox.Show("添加成功"); }
                else { MessageBox.Show("错误"); }
                 * */
            }
            checkerBox.Text = reviewer_name;
        }


        private string checkIDSQL(int userID)
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

        private Boolean updateAccessOle(OleDbConnection conn, String tblName, List<String> updateCols, List<Object> updateVals, List<String> whereCols, List<Object> whereVals)
        {
            String updates = "";
            for (int i = 0; i < updateCols.Count; ++i)
            {
                if (updateCols.Count - 1 != i)
                {
                    updates += updateCols[i] + "=@" + updateCols[i] + ",";
                }
                else
                {
                    updates += updateCols[i] + "=@" + updateCols[i];
                }

            }
            String wheres = "";
            for (int i = 0; i < whereCols.Count; ++i)
            {
                if (whereCols.Count - 1 != i)
                {
                    wheres += whereCols[i] + "=@" + whereCols[i] + ",";
                }
                else
                {
                    wheres += whereCols[i] + "=@" + whereCols[i];
                }

            }

            String strSQL = String.Format("UPDATE {0} SET {1} WHERE {2}", tblName, updates, wheres);
            OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            for (int i = 0; i < updateCols.Count; ++i)
            {
                String c = updateCols[i];
                Object v = updateVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            for (int i = 0; i < whereCols.Count; ++i)
            {
                String c = whereCols[i];
                Object v = whereVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
