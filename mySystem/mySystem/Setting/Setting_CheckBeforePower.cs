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
        private string sqlSel = "select * from confirmarea";

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
                SqlCommand comm = new SqlCommand(sqlSel, conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(sqlSel, connOle);
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
