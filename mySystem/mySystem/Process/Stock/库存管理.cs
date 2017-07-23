﻿using System;
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
    public partial class 库存管理 : Form
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 库存管理()
        {
            InitializeComponent();

            conn = new OleDbConnection(strConnect);
            conn.Open();

            // 绑定控件
            readFromDatabase();
            bindControl();
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void readFromDatabase()
        {
            da = new OleDbDataAdapter("select * from 库存信息", conn);
            cb = new OleDbCommandBuilder(da);
            dt = new DataTable("库存信息");
            bs = new BindingSource();
            da.Fill(dt);
        }

        private void bindControl()
        {
            bs.DataSource = dt;
            dataGridView1.DataSource = bs.DataSource;
        }

        private void btn入库_Click(object sender, EventArgs e)
        {
            入库 form = new 入库();
            form.Show();
        }

        private void btn出库_Click(object sender, EventArgs e)
        {
            出库 form = new 出库();
            form.Show();
        }

        private void btn退货_Click(object sender, EventArgs e)
        {
            退货 form = new 退货();
            form.Show();
        }

        private void btn原料入库_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.原料入库管理 form = new mySystem.Process.Stock.原料入库管理();
            form.ShowDialog();
        }
    }
}
