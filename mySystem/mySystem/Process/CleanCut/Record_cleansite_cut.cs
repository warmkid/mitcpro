using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Process.CleanCut
{
    public partial class Record_cleansite_cut : mySystem.BaseForm
    {
        string prodcode;
        string level;
        string batch;
        DateTime date;
        int classes;
        string cleaner;
        int isok;
        string checker;
        string extr;//备注

        private void queryjob()
        {
            if (mainform.isSqlOk)
            {

            }
            else
            {
                string accessql = "select * from weildprocess_cleansite";
                OleDbCommand cmd = new OleDbCommand(accessql, mainform.connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);
                //填写表格
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = i + 1;
                    dr.Cells[1].Value = dt.Rows[i][0];
                    dr.Cells[2].Value = dt.Rows[i][1];
                    dr.Cells[3].Value = false;
                    dataGridView1.Rows.Add(dr);
                }

                //释放资源
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
            }

        }

        public Record_cleansite_cut(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            queryjob();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
            //System.Console.WriteLine("白班{0} 夜班{1}", checkBox1.Checked, checkBox2.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
            //System.Console.WriteLine("白班{0} 夜班{1}", checkBox1.Checked, checkBox2.Checked);
        }
    }
}
