﻿using System;
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

        OleDbDataAdapter da销售订单,da采购订单;
        DataTable dt销售订单,dt采购订单;
        OleDbCommandBuilder cb采购订单;

        public 订单查询()
        {
            InitializeComponent();
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);
            dgv销售订单.ReadOnly = true;

            dgv采购订单.AllowUserToAddRows = false;
            dgv采购订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购订单_DataBindingComplete);
        }

        void dgv采购订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv采购订单.Columns["ID"].Visible = false;
            dgv采购订单.Columns["采购订单ID"].Visible = false;
            dgv采购订单.Columns["关联的采购批准详细信息ID"].Visible = false;
            dgv采购订单.Columns["关联的采购批转单借用单ID"].Visible = false;
            int cidx = dgv采购订单.Columns["金额"].Index;
            for (int i = 0; i < cidx; ++i)
            {
                dgv采购订单.Columns[i].ReadOnly = true;
            }
        }

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgv销售订单.Columns["ID"].Visible = false;
            //dgv销售订单.Columns["销售订单ID"].Visible = false;
        }

        private void btn销售订单查询_Click(object sender, EventArgs e)
        {
            string 产品代码 =tb销售订单产品代码.Text;
            string 产品名称 = tb销售订单产品名称.Text;
            string sql = "select 销售订单.订单号,销售订单.订单日期,销售订单.客户简称,销售订单详细信息.存货代码,销售订单详细信息.存货名称,销售订单详细信息.规格型号 from 销售订单,销售订单详细信息 where 销售订单详细信息.存货代码 like '%{0}%' and 销售订单详细信息.存货名称 like '%{1}%' and 销售订单详细信息.销售订单ID=销售订单.ID";
            da销售订单 = new OleDbDataAdapter(string.Format(sql, 产品代码, 产品名称),mySystem.Parameter.connOle);
            dt销售订单 = new DataTable("销售订单详细信息");
            da销售订单.Fill(dt销售订单);
            dgv销售订单.DataSource = dt销售订单;
            Utility.setDataGridViewAutoSizeMode(dgv销售订单);
        }
        private void btn采购订单查询_Click(object sender, EventArgs e)
        {
            string 产品代码 = tb采购订单产品代码.Text;
            string 用途 = tb采购订单销售订单.Text;
            string sql = "select * from 采购订单详细信息 where 存货代码 like '%{0}%' and 用途 like '%{1}%'";
            da采购订单 = new OleDbDataAdapter(string.Format(sql, 产品代码, 用途), mySystem.Parameter.connOle);
            dt采购订单 = new DataTable("采购订单详细信息");
            cb采购订单 = new OleDbCommandBuilder(da采购订单);
            da采购订单.Fill(dt采购订单);
            dgv采购订单.DataSource = dt采购订单;
        }

        private void btn采购订单保存_Click(object sender, EventArgs e)
        {
            if (da采购订单 != null)
                da采购订单.Update(dt采购订单);
        }


    }
}
