using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Setting_CleanArea : Form
    {
        public Setting_CleanArea()
        {
            InitializeComponent();
            Init();
            connToServer();
            qury();
        }

        //自定义变量
        private bool isOk;//是否连接数据库成功
        string strCon;
        string sql;

        SqlConnection conn;
        //自定义函数
        private void Init()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            strCon = @"server=10.105.223.19,56625;database=wyttest;Uid=sa;Pwd=mitc";
            sql = "select * from CleanArea_table";
            isOk = false;
            dataGridView1.AllowUserToAddRows = false;
        }
        /*仅用来来测试，实际早已在上一步登陆*/
        public void connToServer()
        {
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }
        private void add_Click(object sender, EventArgs e)
        {
            CleanArea_add add = new CleanArea_add();
            add.Show();
        }
        private void qury()
        {
            if (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return;
            }
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);

            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void fresh_Click(object sender, EventArgs e)
        {
            qury();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            System.Console.WriteLine("**********************************************************");
            System.Console.WriteLine(dataGridView1.SelectedRows.Count);
        }

        private void del_Click(object sender, EventArgs e)
        {
            DataRowView drv = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
            //System.Console.WriteLine(drv[1].ToString());
            string id=drv[0].ToString().Trim();
            string strsql = "delete from CleanArea_table where 名称=" + "'"+id+"'";
            SqlCommand Cmd = new SqlCommand(strsql, conn);
            int i = Cmd.ExecuteNonQuery();
            if(i<0)
            {
                MessageBox.Show("删除失败，请重试");
                return;
            }       
            drv.Row.Delete();
        }
    }
}
