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
            //添加四列
            dt.Columns.Add("确认项目", typeof(String));
            dt.Columns.Add("确认内容", typeof(String));
            dt.Columns.Add(new DataColumn("确认结果", typeof(bool)));
            //添加内容，默认确认结果为“是”
            dt.Rows.Add("环境卫生", "确认车间环境、设备卫生符合生产工艺要求。", true);
            dt.Rows.Add("冷却风", "确认冷却风检测合格，符合生产工艺要求。", true);
            dt.Rows.Add("压缩空气", "确认压缩空气检测合格，气压符合生产工艺要求。", true);
            dt.Rows.Add("过滤网", "大小合适的160目不锈钢过滤网，数量满足生产要求。", true);
            dt.Rows.Add("减速器", "分别检查A、B、C三层减速器油位，是否在油镜中位以上。", true);
            dt.Rows.Add("运动部件", "检查旋转牵引、一牵引、人字夹板、稳泡器、纠偏、二牵引、收卷、冷却风机运转情况。", true);
            dt.Rows.Add("卷芯管", "确认卷芯管清洁合格，规格满足生产要求。", true);
            dt.Rows.Add("原料", "确认原料检测合格，数量满足生产要求。", true);
            dt.Rows.Add("工具准备", "是否按《吹膜机组开机工具表》准备工具。", true);
            dt.Rows.Add("电子称", "已清洁干净且运行正常，电量充足。", true);
            dt.Rows.Add("电动叉车", "已清洁干净且运行正常，电量充足。", true);
            dt.Rows.Add("测厚仪、电脑", "能正常打开，相关软件能正常运行。", true);
            dt.Rows.Add("吸尘器", "已清洁干净且运行正常。", true);
            dt.Rows.Add("供料系统", "设备卫生符合要求，供料系统各部件运行正常。", true);
            

            ///***********************表格数据初始化************************///
            //设置
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in CheckBeforePowerView.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = (i+1).ToString(); //序号
                dr.Cells[1].Value = dt.Rows[i][0].ToString(); //确认项目
                dr.Cells[2].Value = "  "+dt.Rows[i][1].ToString(); //确认内容
                dr.Cells[3].Value = true;
                CheckBeforePowerView.Rows.Add(dr);
            }
            
            if (isSqlOk)
            {
                //若已有数据，向内部添加现有数据
                SqlCommand commSQL = new SqlCommand(sql, conn);
                SqlDataAdapter daSQL = new SqlDataAdapter(commSQL);
                DataTable dtSQL = new DataTable();
                daSQL.Fill(dtSQL);

                for (int i = 1; i <= 14; i++)
                {
                    string qualified_string = "s2_item" + i.ToString() + "_qualified";
                    bool qualified_bool = bool.Parse(dtSQL.Rows[0][qualified_string].ToString());
                    this.CheckBeforePowerView.Rows[i - 1].Cells["确认结果"].Value = qualified_bool;
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
                for (int i = 1; i <= 14; i++)
                {
                    string qualified_string = "s2_item" + i.ToString() + "_qualified";
                    bool qualified_bool = (dtOle.Rows[0][qualified_string].ToString() == "1" ? true : false);
                    this.CheckBeforePowerView.Rows[i - 1].Cells["确认结果"].Value = qualified_bool;
                }
                //operator_id = Convert.ToInt32(dtOle.Rows[0]["s2_operator_id"]);
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
            string qualified_string = null;
            string val = null;
            string sqlstr = null;
            SqlCommand com = null;

            for (int i = 1; i <= 14; i++)
            {
                qualified_string = "s2_item" + i.ToString() + "_qualified";
                val = this.CheckBeforePowerView.Rows[i-1].Cells["确认结果"].Value.ToString() == "True"? "1":"0" ; 
                sqlstr = "update extrusion set " + qualified_string + " = " + val + "  where id =1";
                com = new SqlCommand(sqlstr, conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }

            sqlstr = "update extrusion set s2_operator_id = " + operator_id.ToString() + " where id =1";
            com = new SqlCommand(sqlstr, conn);
            com.ExecuteNonQuery();
            com.Dispose();

            operate_date = recordTimePicker.Value;
            sqlstr = "update extrusion set s2_operate_date = '" + operate_date.ToString() + "' where id =1";
            com = new SqlCommand(sqlstr, conn);
            com.ExecuteNonQuery();
            com.Dispose();

            sqlstr = "update extrusion set s2_reviewer_id = " + review_id.ToString() + " where id =1";
            com = new SqlCommand(sqlstr, conn);
            com.ExecuteNonQuery();
            com.Dispose();

            review_date = checkTimePicker.Value;
            sqlstr = "update extrusion set s2_review_date = '" + review_date.ToString() + "' where id =1";
            com = new SqlCommand(sqlstr, conn);
            com.ExecuteNonQuery();
            com.Dispose();

        }

        private void DataSaveOleDb()
        {
            string qualified_string = null;
            string val = null;
            string sqlstr = null;
            OleDbCommand com = null;

            for (int i = 1; i <= 14; i++)
            {
                qualified_string = "s2_item" + i.ToString() + "_qualified";
                val = this.CheckBeforePowerView.Rows[i - 1].Cells["确认结果"].Value.ToString() == "True" ? "1" : "0";
                sqlstr = "update extrusion set " + qualified_string + " = " + val + "  where id =1";
                com = new OleDbCommand(sqlstr, connOle);
                com.ExecuteNonQuery();
                com.Dispose();
            }

            sqlstr = "update extrusion set s2_operator_id = " + operator_id.ToString() + " where id =1";
            com = new OleDbCommand(sqlstr, connOle);
            com.ExecuteNonQuery();
            com.Dispose();

            operate_date = recordTimePicker.Value;
            sqlstr = "update extrusion set s2_operate_date = '" + operate_date.ToString() + "' where id =1";
            com = new OleDbCommand(sqlstr, connOle);
            com.ExecuteNonQuery();
            com.Dispose();

            sqlstr = "update extrusion set s2_reviewer_id = " + review_id.ToString() + " where id =1";
            com = new OleDbCommand(sqlstr, connOle);
            com.ExecuteNonQuery();
            com.Dispose();

            review_date = checkTimePicker.Value;
            sqlstr = "update extrusion set s2_review_date = '" + review_date.ToString() + "' where id =1";
            com = new OleDbCommand(sqlstr, connOle);
            com.ExecuteNonQuery();
            com.Dispose();
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
                SqlCommand com = null;
                reviewer_name = checkIDSQL(review_id);
                review_date = checkTimePicker.Value;
                string sqlstr = "update extrusion set s2_review_date = '" + review_date.ToString() + "' where id =1";
                com = new SqlCommand(sqlstr, conn);
                com.ExecuteNonQuery();
                com.Dispose();

                sqlstr = "update extrusion set s2_reviewer_id = " + review_id.ToString() + " where id =1";
                com = new SqlCommand(sqlstr, conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }
            else 
            {
                OleDbCommand com = null;
                reviewer_name = checkIDOle(review_id);
                review_date = checkTimePicker.Value;
                string sqlstr = "update extrusion set s2_review_date = '" + review_date.ToString() + "' where id =1";
                com = new OleDbCommand(sqlstr, connOle);
                com.ExecuteNonQuery();
                com.Dispose();

                sqlstr = "update extrusion set s2_reviewer_id = " + review_id.ToString() + " where id =1";
                com = new OleDbCommand(sqlstr, connOle);
                com.ExecuteNonQuery();
                com.Dispose();
            }
            checkerBox.Text = reviewer_name;
        }

        private string checkIDSQL(int userID)
        {
            string user = null;
            string searchsql = "select * from [user] where user_id='" + userID + "'";
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
            comm.CommandText = "select * from [user] where user_id= @ID";
            comm.Parameters.AddWithValue("@ID", userID);

            //string searchole = "select * from [user] where user_id='" + userID + "'";
            //OleDbCommand comm = new OleDbCommand(searchole, connOle);
            OleDbDataReader myReader = comm.ExecuteReader();
            while (myReader.Read())
            {
                user = myReader.GetString(4);
            }

            myReader.Close();
            comm.Dispose();
            return user;
        }
    }
}
