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

namespace mySystem.Process.Bag
{
    public partial class CSBag_InnerPackaging : BaseForm
    {
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

        public CSBag_InnerPackaging(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;
            operator_name = checkID(operator_id);
            operate_date = DateTime.Now.Date;

            DgvInitialize();

            AddLineBtn.Enabled = false;
            CheckBtn.Enabled = false;
        }

        private void DgvInitialize()
        {
            //表格界面设置
            this.RecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeColumns = false;
            this.RecordView.AllowUserToResizeRows = false;
            this.RecordView.ColumnHeadersHeight = 80;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].MinimumWidth = 70;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].ReadOnly = true;
            }
            this.RecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.Columns["序号"].MinimumWidth = 40;
            //this.RecordView.Columns["生产日期时间"].MinimumWidth = 120;
            this.RecordView.Columns["生产日期时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RecordView.Columns["内包序号"].MinimumWidth = 80;
            this.RecordView.Columns["包装规格"].HeaderText = "包装规格\r(只/包)";
            this.RecordView.Columns["包装规格"].MinimumWidth = 80;
            this.RecordView.Columns["产品数量包"].HeaderText = "产品数量\r(包)";
            this.RecordView.Columns["产品数量包"].MinimumWidth = 80;
            this.RecordView.Columns["产品数量只"].HeaderText = "产品数量\r(只)";
            this.RecordView.Columns["产品数量只"].MinimumWidth = 80;
            this.RecordView.Columns["热封线不合格"].HeaderText = "热封线\r不合格\r(只)";
            this.RecordView.Columns["黑点晶点"].HeaderText = "黑点\r晶点\r(只)";
            this.RecordView.Columns["指示剂不良"].HeaderText = "指示剂\r不良\r(只)";
            this.RecordView.Columns["其他"].HeaderText = "其他\r(只)";
            this.RecordView.Columns["不良合计"].HeaderText = "不良\r合计\r(只)";
            this.RecordView.Columns["包装袋热封线"].HeaderText = "包装袋\r热封线";
            this.RecordView.Columns["内标签"].HeaderText = "内标\r签";
            this.RecordView.Columns["内标签"].MinimumWidth = 60;
            this.RecordView.Columns["内包装外观"].HeaderText = "内包装\r外观";
            this.RecordView.Columns["制袋包装分检人"].HeaderText = "制袋/包装\r/分检人";
            this.RecordView.Columns["制袋包装分检人"].MinimumWidth = 120;
            this.RecordView.Columns["复核人"].MinimumWidth = 80;
            
            AddRecordRowLine();
            
        }


        private void AddRecordRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in RecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = (Recordnum + 1).ToString();
            dr.Cells[1].Value = operate_date.ToString("yyyy-mm-dd") + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = ""; 
            dr.Cells[7].Value = "";
            dr.Cells[8].Value = "";
            dr.Cells[9].Value = "";
            dr.Cells[10].Value = "";
            dr.Cells[11].Value = true;
            dr.Cells[12].Value = true; 
            dr.Cells[13].Value = true;
            dr.Cells[14].Value = operator_name;
            dr.Cells[15].Value = "";
            RecordView.Rows.Insert(Recordnum, dr);
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
            dr.Cells[4].Value = "A";
            dr.Cells[5].Value = "B";
            dr.Cells[6].Value = ""; 
            dr.Cells[7].Value = "";
            dr.Cells[8].Value = "";
            dr.Cells[9].Value = "";
            dr.Cells[10].Value = "";
            dr.Cells[11].Value = true;
            dr.Cells[12].Value = true; 
            dr.Cells[13].Value = true;
            dr.Cells[14].Value = "";
            dr.Cells[15].Value = "";
            RecordView.Rows.Add(dr);
        }

        //更新序号
        private void RefreshNum()
        {
            for (int i = 0; i < RecordView.Rows.Count - 1; i++)
            {
                RecordView.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
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

        }

        private void RecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            AddRecordRowLine();
            AddLineBtn.Enabled = false;
            SaveBtn.Enabled = true;
            DelLineBtn.Enabled = true;
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
            { //DataSaveSql(); 
            }
            else
            { //DataSaveOle(); 
            }
            CheckBtn.Enabled = true;
            DelLineBtn.Enabled = false;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            //CheckForm check = new CheckForm(base.mainform);
            //check.ShowDialog();
            //review_id = check.userID;
            //reviewer_name = checkID(review_id);

            //if (isSqlOk)
            //{ }
            //else
            //{ }

            //CheckBtn.Enabled = false;
            //SaveBtn.Enabled = false;
            //AddLineBtn.Enabled = true;
            //RecordView.Rows[checknum].Cells["复核人"].Value = reviewer_name;
            //checknum++;
        }




    }
}
