using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Setting
{
    public partial class Setting_CleanSite : mySystem.BaseForm
    {
        //List<string> cleansite;
        public Setting_CleanSite(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            query();
            
        }
        private void query()
        {
            if (mainform.isSqlOk)
            {

            }
            else
            {
                string accessql = "select * from feedingprocess_cleansite";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = i+1;
                    dr.Cells[1].Value = dt.Rows[i][1].ToString();
                    dataGridView1.Rows.Add(dr);
                }
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
                accessql = "select * from extrusion_cleansite";
                cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                data = new OleDbDataAdapter(cmd);
                data.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView2.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = i + 1;
                    dr.Cells[1].Value = dt.Rows[i][1].ToString();
                    dataGridView2.Rows.Add(dr);
                }
                cmd.Dispose();
                data.Dispose();
                dt.Dispose();
                
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string strsite = textBox1.Text;
            string strprocess = comboBox1.Text;
            DataGridViewRow dr = new DataGridViewRow();
            if (strprocess == "供料工序")
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dataGridView1.Rows.Count+1;
                dr.Cells[1].Value = strsite;
                dataGridView1.Rows.Add(dr);
            }
            else
            {
                foreach (DataGridViewColumn c in dataGridView2.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dataGridView2.Rows.Count + 1;
                dr.Cells[1].Value = strsite;
                dataGridView2.Rows.Add(dr);
            }
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count>0)
            {
                int k = dataGridView1.SelectedRows[0].Index;
                dataGridView1.Rows.RemoveAt(k);
                //改变序号
                for (int i = k; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                }
            }
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int k = dataGridView2.SelectedRows[0].Index;
                dataGridView2.Rows.RemoveAt(k);
                //改变序号
                for (int i = k; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[0].Value = i + 1;
                }
            }
            
        }
        public void DataSave()
        {
            //清空数据库
            if (mainform.isSqlOk)
            {

            }
            else
            {
                string accessql = "delete * from feedingprocess_cleansite";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                cmd.ExecuteNonQuery();
                for(int i=0;i<dataGridView1.Rows.Count;i++)
                {
                    cmd.CommandText = "insert into feedingprocess_cleansite(id,clean_cont) values ('" + dataGridView1.Rows[i].Cells[0].Value+"','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "delete * from extrusion_cleansite";
                cmd.ExecuteNonQuery();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    cmd.CommandText = "insert into extrusion_cleansite(id,clean_cont) values ('" + dataGridView2.Rows[i].Cells[0].Value + "','" + dataGridView2.Rows[i].Cells[1].Value.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
        }
    }
}
