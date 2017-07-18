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
using Newtonsoft.Json.Linq;

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
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
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
            button2.Enabled = false;
            queryjob();
        }
        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            //如果查找成功返回id，否则返回-1
            //int rtnum = -1;
            if (mainform.isSqlOk)
            {
                //未完成
                //string sql = "select user_id from cleanarea";
                //SqlCommand comm = new SqlCommand(sql, conn);
                //SqlDataAdapter da = new SqlDataAdapter(comm);

                //dt = new DataTable();
                //da.Fill(dt);
                return -1;
            }
            else
            {
                string asql = "select user_id from user_aoxing where user_name=" + "'" + s + "'";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cleaner = textBox4.Text;
            checker = textBox5.Text;
            if (cleaner == "")
            {
                MessageBox.Show("清场人不能为空");
                return;
            }
            int cleanerid = queryid(cleaner);
            if (cleanerid == -1)
            {
                MessageBox.Show("清场人id不存在");
                return;
            }

            prodcode = textBox1.Text;
            level = textBox2.Text;
            batch = textBox3.Text;
            date = dateTimePicker1.Value;
            classes = checkBox1.Checked == true ? 1 : 0;//白班1，夜班0
            extr = textBox6.Text;

            string st = "{}";
            JObject jobj = JObject.Parse(st);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int a=dataGridView1.Rows[i].Cells[3].Value.ToString()=="True"?1:0;
                jobj.Add(dataGridView1.Rows[i].Cells[1].Value.ToString(), new JValue(a));
            }
            System.Console.WriteLine(jobj.ToString());

            if (mainform.isSqlOk)
            { }
            else
            { }
            button2.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //mySystem.CheckForm checkform = new mySystem.CheckForm(mainform);
            //checkform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

    }
}
