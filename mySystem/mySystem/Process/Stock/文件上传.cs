using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Stock
{
    public partial class 文件上传 : BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        public 文件上传()
        {
            InitializeComponent();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            refresh();
        }

        void refresh()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 文件上传记录", conn);
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
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 文件上传记录 where 0=1",conn);
                DataTable dt = new DataTable("文件上传记录");
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
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
            if (e.RowIndex > 0)
                System.Diagnostics.Process.Start(dataGridView1.Rows[e.RowIndex].Cells["文件路径"].Value.ToString());
        }
    }
}
