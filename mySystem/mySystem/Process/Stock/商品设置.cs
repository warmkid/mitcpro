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
    public partial class 商品设置 : Form
    {

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 商品设置()
        {
            InitializeComponent();
            conn = new OleDbConnection(strConnect);
            conn.Open();

            readFromDatabase();
            setDataGridViewColumns();
            bindControl();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void readFromDatabase()
        {
            da = new OleDbDataAdapter("select * from 商品信息", conn);
            cb = new OleDbCommandBuilder(da);
            dt = new DataTable("商品信息");
            bs = new BindingSource();
            da.Fill(dt);
        }

        private void bindControl()
        {
            bs.DataSource = dt;
            dataGridView1.DataSource = bs.DataSource;
        }

        private void setDataGridViewColumns()
        {

        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            int idx = dataGridView1.SelectedCells[0].RowIndex;
            dt.Rows[idx].Delete();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            da.Update((DataTable)bs.DataSource);
            readFromDatabase();
            bindControl();
        }
    }
}
