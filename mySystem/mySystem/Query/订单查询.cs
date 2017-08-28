using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Query
{
    public partial class 订单查询 : Form
    {

        OleDbDataAdapter da销售订单;
        DataTable dt销售订单;

        public 订单查询()
        {
            InitializeComponent();
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);
        }

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv销售订单.Columns["ID"].Visible = false;
            dgv销售订单.Columns["销售订单ID"].Visible = false;
        }

        private void btn销售订单查询_Click(object sender, EventArgs e)
        {
            string 产品代码 =tb销售订单产品代码.Text;
            string 产品名称 = tb销售订单产品名称.Text;
            string sql = "select * from 销售订单详细信息 where 存货编码 like '%{0}%' and 存货名称 like '%{1}%'";
            da销售订单 = new OleDbDataAdapter(string.Format(sql, 产品代码, 产品名称),mySystem.Parameter.connOle);
            dt销售订单 = new DataTable("销售订单详细信息");
            da销售订单.Fill(dt销售订单);
            dgv销售订单.DataSource = dt销售订单;
        }
    }
}
