using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 订单和库存管理
{
    public partial class 订单管理 : Form
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 订单管理()
        {
            InitializeComponent();
            // 连接数据库
            conn = new OleDbConnection(strConnect);
            conn.Open();

            // 绑定控件
            //readFromDatabase();
            //bindControl();
            //datagridview1.ReadOnly = true;
            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.RowHeadersVisible = false;
            // TODO 查询
        }

        private void btn添加订单_Click(object sender, EventArgs e)
        {
            添加订单 form = new 添加订单();
            form.Show();
        }

        private void readFromDatabase()
        {
            da = new OleDbDataAdapter("select * from 订单信息", conn);
            cb = new OleDbCommandBuilder(da);
            dt = new DataTable("订单信息");
            bs = new BindingSource();
            da.Fill(dt);
        }

        private void bindControl()
        {
            bs.DataSource = dt;
            //dataGridView1.DataSource = bs.DataSource;
        }
    }
}
