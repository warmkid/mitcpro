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
    public partial class 库存查询 : Form
    {
         OleDbDataAdapter da退货台账,da出库单;
        DataTable dt退货台账,dt出库单;

        public 库存查询()
        {
            InitializeComponent();

            dtp退货台账申请开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dgv退货台账.AllowUserToAddRows = false;
            dgv退货台账.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv退货台账_DataBindingComplete);
            dgv退货台账.ReadOnly = true;

            dtp出库单出库开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dgv出库单.AllowUserToAddRows = false;
            dgv出库单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv出库单_DataBindingComplete);
            dgv出库单.ReadOnly = true;

        }

        void dgv出库单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv出库单.Columns["ID"].Visible = false;
        }

        void dgv退货台账_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv退货台账.Columns["ID"].Visible = false;
        }

        private void btn退货台账查询_Click(object sender, EventArgs e)
        {
            string sql = "select * from 产品退货记录 where 申请日期 between #{0}# and #{1}# and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%' and 产品名称 like '%{4}%' and 产品批号 like '%{5}%'";
            DateTime start = dtp退货台账申请开始时间.Value;
            DateTime end = dtp退货台账申请结束时间.Value;
            string 订单号 = tb退货台账销售合同号.Text;
            string 客户 = tb退货台账客户名称.Text;
            string 产品名称 = tb退货台账产品名称.Text;
            string 产品批号 = tb退货台账产品批号.Text;
            da退货台账 = new OleDbDataAdapter(string.Format(sql, start, end, 订单号, 客户, 产品名称, 产品批号), mySystem.Parameter.connOle);
            dt退货台账 = new DataTable("产品退货记录");
            da退货台账.Fill(dt退货台账);
            dgv退货台账.DataSource = dt退货台账;
        }

        private void btn查询出库单_Click(object sender, EventArgs e)
        {
            string sql = "select * from 出库单详细信息 where 出库日期 between #{0}# and #{1}# and 发货公司 like '%{2}%' and 客户 like '%{3}%' and 存货名称 like '%{4}%' and 批号 like '%{5}%'";
            DateTime start = dtp出库单出库开始时间.Value;
            DateTime end = dtp出库单出库结束时间.Value;
            string 发货公司 = tb出库单发货公司.Text;
            string 客户 = tb退货台账客户名称.Text;
            string 产品名称 = tb退货台账产品名称.Text;
            string 产品批号 = tb退货台账产品批号.Text;
            da出库单 = new OleDbDataAdapter(string.Format(sql, start, end, 发货公司, 客户, 产品名称, 产品批号), mySystem.Parameter.connOle);
            dt出库单 = new DataTable("出库单详细信息");
            da出库单.Fill(dt出库单);
            dgv出库单.DataSource = dt出库单;
        }
    }
}
