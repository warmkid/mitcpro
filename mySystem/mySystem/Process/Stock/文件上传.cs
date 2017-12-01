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

namespace mySystem.Process.Stock
{
    public partial class 文件上传 : BaseForm
    {
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;

        public 文件上传()
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            refresh();
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        void refresh()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 文件上传记录", mySystem.Parameter.conn);
            DataTable dt = new DataTable("文件上传记录");
            da.Fill(dt);
            dataGridView1.DataSource = dt;
   
        }

        private void btn上传_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                
                String fullName = ofd.FileName;
                String name = ofd.SafeFileName;
                DateTime now = DateTime.Now;
                String path = System.Environment.CurrentDirectory + @"/../../物料入场检验报告/";
                System.IO.File.Copy(fullName, path + name, true);
                SqlDataAdapter da = new SqlDataAdapter("select * from 文件上传记录 where 0=1", mySystem.Parameter.conn);
                DataTable dt = new DataTable("文件上传记录");
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dr["文件名"] = name;
                dr["上传时间"] = now;
                dr["文件路径"] = path + name;
                dt.Rows.Add(dr);
                da.Update(dt);
            }
            refresh();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    System.Diagnostics.Process.Start(dataGridView1.Rows[e.RowIndex].Cells["文件路径"].Value.ToString());
                }
                catch (Win32Exception ee)
                {
                    MessageBox.Show("找不到文件！");
                }
            }
        }
    }
}
