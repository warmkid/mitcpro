using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem.Extruction.Process
{
    public partial class ProductInnerPackagingRecord : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        public ProductInnerPackagingRecord(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            if (isSqlOk) { operator_name = checkIDSql(operator_id); }
            else { operator_name = checkIDOle(operator_id); }
            DataInitialize();
        }

        private void DataInitialize()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView1.Columns[i].MinimumWidth = 70;
            }
            this.dataGridView1.Columns["内包序号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns["内包序号"].ReadOnly = true;
            this.dataGridView1.Columns["生产日期"].MinimumWidth = 100;
            this.dataGridView1.Columns["生产日期"].ReadOnly = true;
            this.dataGridView1.Columns["内包序号"].MinimumWidth = 100;
            this.dataGridView1.Columns["包装人"].MinimumWidth = 100;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //this.dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            AddRowLine();


            //表二
            this.dataGridView2.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView2.Columns.Count; i++)
            {
                this.dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView2.Columns[i].MinimumWidth = 80;
            }
            this.dataGridView2.Columns["批号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView2.Columns["包材"].MinimumWidth = 150;

            this.dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AddRowLine2();
            AddRowLine2();
            dataGridView2.Rows[0].Cells[0].Value = "BZD-";
            dataGridView2.Rows[1].Cells[0].Value = "指示剂";

            //其他信息
            recorderBox.Text = operator_name;
        
        }

        //添加单行模板
        private void AddRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = (dataGridView1.RowCount+1).ToString(); //DateTime.Now.ToString("yyyy-MM-dd");
            dr.Cells[1].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = true;
            dr.Cells[4].Value = true;
            dr.Cells[5].Value = true;
            dr.Cells[6].Value = true; //"包装是否完好"
            dr.Cells[7].Value = true; //"是否清洁合格"
            dr.Cells[8].Value = ""; //"包装人
            dataGridView1.Rows.Add(dr);
        }


        //添加单行模板
        private void AddRowLine2()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView2.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = "";
            dr.Cells[1].Value = "";
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = ""; 
            dataGridView2.Rows.Add(dr);
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

        private void AddLineBtn_Click(object sender, EventArgs e)
        {  AddRowLine();  }

        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1)
            {
                this.dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                numFresh();
            }            
        }

        private void numFresh()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            { dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString(); }
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            //CheckForm check = new CheckForm(base.mainform);
            //check.ShowDialog();
            //review_id = check.userID;

            //if (isSqlOk)
            //{ }
            //else
            //{
            //    reviewer_name = checkIDOle(review_id);
            //}

            //checkerBox.Text = reviewer_name;
        }

    }
}
