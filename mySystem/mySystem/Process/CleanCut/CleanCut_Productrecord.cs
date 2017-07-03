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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_Productrecord : BaseForm
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

        private int Recordnum = 0;

        public CleanCut_Productrecord(MainForm mainform) : base(mainform)
        {
            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            operator_name = checkID(operator_id);

            InitializeComponent();

            DgvInitialize();
        }

        private void DgvInitialize()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.RecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeColumns = false;
            this.RecordView.AllowUserToResizeRows = false;
            this.RecordView.AllowUserToAddRows = false;
            this.RecordView.ColumnHeadersHeight = 50;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].MinimumWidth = 80;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].ReadOnly = true;
            }
            this.RecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.Columns["序号"].MinimumWidth = 40;
            this.RecordView.Columns["物料代码"].MinimumWidth = 140;
            this.RecordView.Columns["膜卷批号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RecordView.Columns["膜卷批号"].HeaderText = "膜卷批号-卷号";
            this.RecordView.Columns["长度1"].HeaderText = "长度\r(m)";
            this.RecordView.Columns["长度2"].HeaderText = "长度\r(m)";
            this.RecordView.Columns["重量"].HeaderText = "重量\r(kg)";
            this.RecordView.Columns["清洁分切后代码"].MinimumWidth = 140;
            this.RecordView.Columns["收率"].HeaderText = "收率(%)";

            AddRowLine();
        }

        //添加单行模板
        private void AddRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in RecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = (Recordnum + 1).ToString();
            dr.Cells[1].Value = "";
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = "";
            dr.Cells[7].Value = "";
            dr.Cells[8].Value = true;
            RecordView.Rows.Add(dr);
            Recordnum = Recordnum + 1;
        }

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
                string user = null;
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
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

        }

        //更新序号
        private void RefreshNum()
        {
            for (int i = 0; i < RecordView.Rows.Count; i++)
            {
                RecordView.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }

        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            AddRowLine();
        }

        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (RecordView.Rows.Count >= 1)
            {
                int deletenum = RecordView.CurrentRow.Index;
                this.RecordView.Rows.RemoveAt(deletenum);
                Recordnum = Recordnum - 1;                
            }
            RefreshNum();
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            //CheckForm check = new CheckForm(base.mainform);
            //check.ShowDialog();
            //review_id = check.userID;
            //reviewer_name = checkID(review_id);
            //checkerBox.Text = reviewer_name;
        }
    }
}
