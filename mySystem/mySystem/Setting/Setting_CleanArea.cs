using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{

    public partial class Setting_CleanArea : mySystem.BaseForm
    {
        public Setting_CleanArea(mySystem.MainForm mainform):base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            Init();
            qury();
        }

        //自定义变量
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access

        DataTable dt;
        //private bool isOk;//是否连接数据库成功
        //string strCon;
        //string sql;

        //自定义函数
        private void Init()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Font = new Font("宋体", 10);

            //strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //sql = "select * from cleanarea";
            //isOk = false;

        }
        /*仅用来来测试，实际早已在上一步登陆*/
        public void connToServer()
        {
            //conn = new SqlConnection(strCon);
            //conn.Open();
            //isOk = true;
        }
        private void add_Click(object sender, EventArgs e)
        {
            //CleanArea_add add = new CleanArea_add(base.mainform);
            //add.Show();
            string clean = textBox1.Text;
            string cont = textBox2.Text;
            if (clean == "" || cont == "")
            {
                MessageBox.Show("清洁区域和清洁内容均不能为空");
                return;
            }
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = dr.Cells[0].Value = dataGridView1.Rows.Count + 1;
            dr.Cells[1].Value = clean;
            dr.Cells[2].Value = cont;
            dataGridView1.Rows.Add(dr);
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private void qury()
        {
            //if (!isOk)
            //{
            //    MessageBox.Show("连接数据库失败", "error");
            //    return;
            //}
            if (isSqlOk)
            {
                string sql = "select * from cleanarea";
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                string accessql = "select * from cleanarea";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                data.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = i+1;
                    dr.Cells[1].Value = dt.Rows[i][0].ToString();
                    dr.Cells[2].Value = dt.Rows[i][1].ToString();
                    dataGridView1.Rows.Add(dr);
                }
            }
            
            //dataGridView1.Columns[0].Width = 370;
            //dataGridView1.Columns[1].Width = 800;
        }
        private void fresh_Click(object sender, EventArgs e)
        {
            qury();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //System.Console.WriteLine("**********************************************************");
            //System.Console.WriteLine(dataGridView1.SelectedRows.Count);
        }

        private void del_Click(object sender, EventArgs e)
        {
            if (isSqlOk)
            {
                DataRowView drv = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                //System.Console.WriteLine(drv[1].ToString());
                string id = drv[0].ToString().Trim();
                string strsql = "delete from cleanarea where cast([清洁区域] as nvarchar(50))=" + "'" + id + "'";
                SqlCommand Cmd = new SqlCommand(strsql, conn);
                int i = Cmd.ExecuteNonQuery();
                if (i < 0)
                {
                    MessageBox.Show("删除失败，请重试");
                    return;
                }
                drv.Row.Delete();
            }
            else
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int k = dataGridView1.SelectedRows[0].Index;
                    dataGridView1.Rows.RemoveAt(k);
                    //改变序号
                    for (int i = k; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = i + 1;
                    }
                }
            }

        }
        public void DataSave()
        {
            if (mainform.isSqlOk)
            {

            }
            else
            {
                string accessql = "delete * from cleanarea";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    cmd.CommandText = "insert into cleanarea(清洁区域,清洁内容) values ('" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
        }
    }
}
