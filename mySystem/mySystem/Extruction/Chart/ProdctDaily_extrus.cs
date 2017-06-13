using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApplication1
{
    /// <summary>
    ///  吹膜生产日报表
    /// </summary>
    public partial class ProdctDaily_extrus : Form
    {
        public ProdctDaily_extrus()
        {
            InitializeComponent();
            Init();
            connToServer();
            queryAndShow();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //自定义变量
        private bool isOk;//是否连接数据库成功
        string prodCode;//生产指令
        string strCon;
        string sql;

        SqlConnection conn;

        //自定义函数
        private void Init()
        {
            strCon = @"server=10.105.223.19,56625;database=wyttest;Uid=sa;Pwd=mitc";
            sql = "select * from ProduceDaily_table";
            prodCode = "0x34222fds";
            isOk = false;
            lastRow= new List<object[]>();
            dataGridView1.AllowUserToAddRows = false;
        }
        /*仅用来来测试，实际早已在上一步登陆*/
        private void connToServer()
        {             
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }
        private void queryAndShow()
        {
            if (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return;
            }
            //显示生产指令
            label3.Text = prodCode;

            //查询
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);

            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //da.Fill(dt);

            ///添加一列
            DataColumn col = new DataColumn("编号");
            dt.Columns.Add(col);
            da.Fill(dt);
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dt.Rows[row][0] = (row+1).ToString();
            }
            
            ///添加合计
            DataRow row1;
            row1=dt.NewRow();
            row1[0] = "合计";
            row1[5] = dt.Compute("sum("+dt.Columns[5].ColumnName+")", "TRUE");
            row1[6] = dt.Compute("sum(" + dt.Columns[6].ColumnName + ")", "TRUE");
            row1[7] = dt.Compute("sum(" + dt.Columns[7].ColumnName + ")", "TRUE");
            dt.Rows.Add(row1);
         
            dataGridView1.DataSource = dt;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //System.Console.WriteLine("**************************************************************************8");
            //if (e.RowIndex >= 0 || dataGridView1.Rows.Count == 0)
            //    return;

            //if (lastRow.Count == 0)
            //{
            //    colindex = e.ColumnIndex;
            //    int index = dataGridView1.Rows.Count - 1;
            //    System.Console.WriteLine(colindex.ToString());
            //    System.Console.WriteLine(index.ToString());
            //    lastRow.Add(((DataTable)dataGridView1.DataSource).Rows[index].ItemArray);
            //    System.Console.WriteLine(lastRow[0].ToString());
            //    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);

            //    DataTable dt = ((DataTable)dataGridView1.DataSource);
            //    DataView dv = dt.DefaultView;
            //    dv.Sort = dt.Columns[colindex].ColumnName;
            //    dt = dv.ToTable();
            //    dt.Rows.Add(lastRow[0]);
            //    lastRow.Clear();
            //    dataGridView1.DataSource = dt;
            //}
        }
        List<object[]> lastRow;
        int colindex;

    }
}
