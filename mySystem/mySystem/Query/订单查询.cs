using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Query
{
    public partial class 订单查询 : BaseForm
    {

        SqlDataAdapter da销售订单,da采购订单;
        DataTable dt销售订单,dt采购订单;
        SqlCommandBuilder cb采购订单;

        public 订单查询(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);
            dgv销售订单.ReadOnly = true;
            tb销售订单产品代码.PreviewKeyDown += new PreviewKeyDownEventHandler(tb销售订单产品代码_PreviewKeyDown);
            tb销售订单产品名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb销售订单产品名称_PreviewKeyDown);

            dgv采购订单.AllowUserToAddRows = false;
            dgv采购订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购订单_DataBindingComplete);
            dgv采购订单.CellDoubleClick += new DataGridViewCellEventHandler(dgv采购订单_CellDoubleClick);
            dgv采购订单.CellEndEdit += new DataGridViewCellEventHandler(dgv采购订单_CellEndEdit);
            tb采购订单产品代码.PreviewKeyDown += new PreviewKeyDownEventHandler(tb采购订单产品代码_PreviewKeyDown);
            tb采购订单供应商.PreviewKeyDown += new PreviewKeyDownEventHandler(tb采购订单供应商_PreviewKeyDown);
            tb采购订单销售订单.PreviewKeyDown += new PreviewKeyDownEventHandler(tb采购订单销售订单_PreviewKeyDown);
        }

        void tb采购订单销售订单_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询采购订单.PerformClick();
        }

        void tb采购订单供应商_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询采购订单.PerformClick();
        }

        void tb采购订单产品代码_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询采购订单.PerformClick();
        }

        void tb销售订单产品名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn销售订单查询.PerformClick();
        }

        void tb销售订单产品代码_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn销售订单查询.PerformClick();
        }

        void dgv采购订单_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgv采购订单.Columns[e.ColumnIndex].Name == "单价")
                {
                    double 单价 = Convert.ToDouble(dgv采购订单["单价", e.RowIndex].Value);
                    double 数量 = Convert.ToDouble(dgv采购订单["采购数量", e.RowIndex].Value);
                    dgv采购订单["金额", e.RowIndex].Value = 数量 * 单价;
                }
            }
        }

        void dgv采购订单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgv采购订单.Columns[e.ColumnIndex].Name == "用途")
                {
                    string 销售订单号 = dgv采购订单["用途", e.RowIndex].Value.ToString();
                    SqlDataAdapter da = new SqlDataAdapter("select * from 销售订单 where 订单号='" + 销售订单号 + "'", mySystem.Parameter.conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        int id = Convert.ToInt32(dt.Rows[0]["ID"]);
                        (new mySystem.Process.Order.销售订单(mainform, id)).ShowDialog();
                    }
                }
            }
            if (dgv采购订单.Columns[e.ColumnIndex].Name == "进度")
            {
                //string t = mySystem.Other.InputTextWindow.getString("jindu");
                //dgv采购订单[e.ColumnIndex, e.RowIndex].Value = t;
            }
        }

        void dgv采购订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv采购订单.Columns["ID"].Visible = false;
            dgv采购订单.Columns["采购订单ID"].Visible = false;
            dgv采购订单.Columns["关联的采购批准详细信息ID"].Visible = false;
            dgv采购订单.Columns["关联的采购批转单借用单ID"].Visible = false;
            dgv采购订单.Columns["进度"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv采购订单.RowHeadersVisible = false;
            Utility.setDataGridViewAutoSizeMode(dgv采购订单);
            int cidx = dgv采购订单.Columns["金额"].Index;
            for (int i = 0; i <= cidx; ++i)
            {
                dgv采购订单.Columns[i].ReadOnly = true;
            }
            dgv采购订单.Columns["单价"].ReadOnly = false;
        }

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgv销售订单.Columns["ID"].Visible = false;
            //dgv销售订单.Columns["销售订单ID"].Visible = false;
            dgv销售订单.RowHeadersVisible = false;
            Utility.setDataGridViewAutoSizeMode(dgv销售订单);
        }

        private void btn销售订单查询_Click(object sender, EventArgs e)
        {
            string 产品代码 =tb销售订单产品代码.Text;
            string 产品名称 = tb销售订单产品名称.Text;
            string sql = "select 销售订单.订单号,销售订单.订单日期,销售订单.客户简称,销售订单详细信息.存货代码,销售订单详细信息.存货名称,销售订单详细信息.规格型号 from 销售订单,销售订单详细信息 where 销售订单详细信息.存货代码 like '%{0}%' and 销售订单详细信息.存货名称 like '%{1}%' and 销售订单详细信息.销售订单ID=销售订单.ID";
            da销售订单 = new SqlDataAdapter(string.Format(sql, 产品代码, 产品名称),mySystem.Parameter.conn);
            dt销售订单 = new DataTable("销售订单详细信息");
            da销售订单.Fill(dt销售订单);
            dgv销售订单.DataSource = dt销售订单;
            Utility.setDataGridViewAutoSizeMode(dgv销售订单);
        }
        private void btn采购订单查询_Click(object sender, EventArgs e)
        {
            string 产品代码 = tb采购订单产品代码.Text;
            string 用途 = tb采购订单销售订单.Text;
            string 供应商= tb采购订单供应商.Text;
            string sql = "select * from 采购订单详细信息 where 存货代码 like '%{0}%' and 用途 like '%{1}%' and 供应商 like '%{2}%'";
            da采购订单 = new SqlDataAdapter(string.Format(sql, 产品代码, 用途, 供应商), mySystem.Parameter.conn);
            dt采购订单 = new DataTable("采购订单详细信息");
            cb采购订单 = new SqlCommandBuilder(da采购订单);
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
