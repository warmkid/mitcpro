using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Setting
{
    public partial class SettingHandOver : BaseForm
    {
        OleDbConnection conOle;
        public SettingHandOver(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getItem();
        }
        private void getItem()
        {
            List<List<Object>> ret = new List<List<Object>>();
            //string tableStr = "set_handover";
            string cmdStr = "SELECT 确认项目 FROM handoveritem;";
            //ret = Utility.selectAccess(mainform.connOle, tableStr, null, null, null, null, null, null, null, null);
            OleDbCommand sqlcmd = new OleDbCommand(cmdStr, conOle);
            //sqlcmd.ExecuteNonQuery();

            OleDbDataReader reader = null;
            reader = sqlcmd.ExecuteReader();
            sqlcmd.Dispose();
            while (reader.Read())
            {
                List<Object> row = new List<Object>(new Object[] { reader["确认项目"]});
                //row.Add(row);

                ret.Add(row);
            }
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
            Utility.fillDataGridView(dataGridView1, ret);

        }
        public void DataSave()
        {
            List<List<Object>> data = new List<List<object>>();
            string sqlStr = "DELETE * FROM handoveritem";
            string tableStr = "handoveritem";
            bool flag;
            List<String> insertCols = new List<string>(new string[] { "确认序号", "确认项目" });
            List<Object> insertVals;
            OleDbCommand sqlcmd = new OleDbCommand(sqlStr, conOle);
            sqlcmd.ExecuteNonQuery();
            sqlcmd.Dispose();
            data = Utility.readFromDataGridView(dataGridView1);
            for (int i = 0; i < data.Count - 1; i++)
            {
                //insertVals=new List<object>(new object[]{Convert.ToString(data[i])});
                insertVals = new List<object>();
                insertVals.Add(i + 1);
                insertVals.Add(data[i][0]);
                flag = Utility.insertAccess(conOle, tableStr, insertCols, insertVals);
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            //dataGridView1.
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Selected)
                {
                    dataGridView1.Rows[i].Selected = true;
                }
            }
            int k = dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("您确认要删除这" + Convert.ToString(k) + "项吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)//给出提示
            {

            }
            else
            {
                if (k != dataGridView1.Rows.Count - 2)//因为还有一行为统计行所以减2
                {

                    for (int i = k; i >= 1; i--)//从下往上删，避免沙漏效应
                    {

                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                    }
                }
                else
                {
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<List<Object>> data = new List<List<object>>();
            string sqlStr = "DELETE * FROM handoveritem";
            string tableStr = "handoveritem";
            bool flag;
            List<String> insertCols = new List<string>(new string[] { "确认序号", "确认项目" });
            List<Object> insertVals;
            OleDbCommand sqlcmd = new OleDbCommand(sqlStr, conOle);
            sqlcmd.ExecuteNonQuery();
            sqlcmd.Dispose();
            data=Utility.readFromDataGridView(dataGridView1);
            for (int i = 0; i < data.Count-1; i++)
            {
                //insertVals=new List<object>(new object[]{Convert.ToString(data[i])});
                insertVals = new List<object>();
                insertVals.Add(i+1);
                insertVals.Add(data[i][0]);
                flag = Utility.insertAccess(conOle, tableStr, insertCols, insertVals);
            }
        }
    }
}

