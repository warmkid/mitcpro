using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class Setting_CheckBeforePower : BaseForm
    {
        //setting_checkbeforepower
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private string tableSel = "confirmarea";

        private DataTable dt;

        public Setting_CheckBeforePower(mySystem.MainForm mainform) : base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            Init();
        }

        //内容初始化
        private void Init()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Font = new Font("宋体", 12);

            if (isSqlOk)
            {
                SqlCommand comm = new SqlCommand("Select * From "+tableSel, conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand("Select * From " + tableSel, connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                data.Fill(dt);
            }

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Columns["确认项目"].MinimumWidth = 160;
            dataGridView1.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dt.Rows[i]["确认序号"].ToString(); 
                dr.Cells[1].Value = dt.Rows[i]["确认项目"].ToString(); //确认项目
                dr.Cells[2].Value = dt.Rows[i]["确认内容"].ToString(); //确认内容
                dataGridView1.Rows.Add(dr);
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = (dataGridView1.Rows.Count + 1).ToString();
            dataGridView1.Rows.Add(dr);
        }

        private void del_Click(object sender, EventArgs e)
        {
            int deletenum = dataGridView1.CurrentRow.Index; 
            this.dataGridView1.Rows.RemoveAt(deletenum);

            numFresh();
        }

        private void numFresh()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            { dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString(); }
        }

        public void DataSave()
        {
            if (isSqlOk)
            { }
            else
            {
                //先删除数据库内容
                string strDel = "DELETE  FROM "+tableSel;
                OleDbCommand inst = new OleDbCommand(strDel, connOle);
                inst.ExecuteNonQuery();

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    List<String> queryCols = new List<String>(new String[] {"确认序号","确认项目","确认内容" });
                    List<Object> queryVals = new List<Object>(new Object[] { dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString() });
                    Boolean b = Utility.insertAccess(connOle, tableSel, queryCols, queryVals);
                }

                //public static Boolean insertAccess(OleDbConnection conn, String tblName, List<String> insertCols, List<Object> insertVals)


                /*
                List<List<Object>> reasList = Utility.readFromDataGridView(dataGridView1);
                List<String> queryCols = new List<String>(new String[] {  });
                for (int i = 0; i < reasList.Count; i++)
                { queryCols.Add{""}; }

                List<String> queryCols = new List<String>(new String[] { "s4_review_opinion" });
                List<Object> queryVals = new List<Object>(new Object[] { jarray.ToString() });
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                //Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
                Boolean b = Utility.updateAccess(connOle, tableSel, queryCols, queryVals, whereCols, whereVals);*/

            }
            
        }

    }
}
