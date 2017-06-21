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
            dataGridView1.Font = new Font("宋体", 12);

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
            CleanArea_add add = new CleanArea_add(base.mainform);
            add.Show();
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
            }
            else
            {
                string accessql = "select * from cleanarea";
                OleDbCommand cmd = new OleDbCommand(accessql, connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                data.Fill(dt);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 370;
            dataGridView1.Columns[1].Width = 800;
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
            DataRowView drv = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
            //System.Console.WriteLine(drv[1].ToString());
            string id=drv[0].ToString().Trim();

            if (isSqlOk)
            {
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
                //string strasql = "delete from cleanarea where cast([清洁区域] as nvarchar(50))=" + "'" + id + "'";
                string strasql = "delete from cleanarea where 清洁区域=" + "'" + id + "'";
                OleDbCommand Cmd = new OleDbCommand(strasql, connOle);
                int i = Cmd.ExecuteNonQuery();
                if (i < 0)
                {
                    MessageBox.Show("删除失败，请重试");
                    return;
                }
                drv.Row.Delete();
            }

        }
    }
}
