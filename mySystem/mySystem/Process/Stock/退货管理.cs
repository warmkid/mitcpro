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

namespace mySystem.Process.Stock
{
    public partial class 退货管理 : BaseForm
    {

//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter da;
        SqlCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 退货管理(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init产品退货申请单();
                    break;
                case 1:
                    //init采购需求单();
                    break;
                case 2:
                    break;
            }
            dgv产品退货申请单.CellDoubleClick += new DataGridViewCellEventHandler(dgv产品退货申请单_CellDoubleClick);
            dgv审批单1.CellDoubleClick += new DataGridViewCellEventHandler(dgv审批单1_CellDoubleClick);
            dgv审批单2.CellDoubleClick += new DataGridViewCellEventHandler(dgv审批单2_CellDoubleClick);
            dgv产品退货接收单.CellDoubleClick += new DataGridViewCellEventHandler(dgv产品退货接收单_CellDoubleClick);
            dgv退货请验单.CellDoubleClick += new DataGridViewCellEventHandler(dgv退货请验单_CellDoubleClick);
            dgv评审单1.CellDoubleClick += new DataGridViewCellEventHandler(dgv评审单1_CellDoubleClick);
            dgv评审单2.CellDoubleClick += new DataGridViewCellEventHandler(dgv评审单2_CellDoubleClick);
            dgv退货记录.CellDoubleClick += new DataGridViewCellEventHandler(dgv退货记录_CellDoubleClick);
        }

        void dgv退货记录_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货记录 form = new 产品退货记录(mainform, dgv退货记录["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv评审单2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货评审单2 form = new 产品退货评审单2(mainform, dgv评审单2["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv评审单1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货评审单1 form = new 产品退货评审单1(mainform, dgv评审单1["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv退货请验单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货请验单 form = new 产品退货请验单(mainform, dgv退货请验单["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv产品退货接收单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货接收单 form = new 产品退货接收单(mainform, dgv产品退货接收单["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv审批单2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货审批单2 form = new 产品退货审批单2(mainform, dgv审批单2["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv审批单1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货审批单1 form = new 产品退货审批单1(mainform, dgv审批单1["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void dgv产品退货申请单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                产品退货申请单 form = new 产品退货申请单(mainform, dgv产品退货申请单["退货申请单编号", e.RowIndex].Value.ToString());
                form.Show();
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init产品退货申请单();
                    break;
                case 1:
                    init产品退货审批单1();
                    break;
                case 2:
                    init产品退货审批单2();
                    break;
                case 3:
                    init产品退货接收单();
                    break;
                case 4:
                    init产品退货请验单();
                    break;
                case 5:
                    init产品退货评审单1();
                    break;
                case 6:
                    init产品退货评审单2();
                    break;
                case 7:
                    init产品退货记录();
                    break;
            }
        }

        #region 产品退货申请单

        void init产品退货申请单()
        {
            dtp退货申请单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货申请单结束时间.Value = DateTime.Now;
            dt = get产品退货申请单(dtp退货申请单开始时间.Value, dtp退货申请单结束时间.Value, tb退货申请单订单号.Text, tb退货申请单客户名称.Text);
            dgv产品退货申请单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv产品退货申请单_DataBindingComplete);
            dgv产品退货申请单.DataSource = dt;
            dgv产品退货申请单.ReadOnly = true;
        }

        void dgv产品退货申请单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品退货申请单.AllowUserToAddRows = false;
            dgv产品退货申请单.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv产品退货申请单);
        }

        DataTable get产品退货申请单(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货申请单 where 申请日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货申请单");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货申请单_Click(object sender, EventArgs e)
        {
            产品退货申请单 form = new 产品退货申请单(mainform);
            form.Show();
            if (dgv产品退货申请单.Rows.Count > 0)
                dgv产品退货申请单.FirstDisplayedScrollingRowIndex = dgv产品退货申请单.Rows.Count - 1;
        }

        private void btn查询退货申请单_Click(object sender, EventArgs e)
        {
            dt = get产品退货申请单(dtp退货申请单开始时间.Value, dtp退货申请单结束时间.Value, tb退货申请单订单号.Text, tb退货申请单客户名称.Text);
            dgv产品退货申请单.DataSource = dt;
        }
        #endregion


        #region 产品退货审批单1

        void init产品退货审批单1()
        {
            dtp产品退货审批单1开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp产品退货审批单1结束时间.Value = DateTime.Now;
            dt = get产品退货审批单1(dtp产品退货审批单1开始时间.Value, dtp产品退货审批单1结束时间.Value, tb审批单1订单号.Text, tb审批单1客户名称.Text);
            dgv审批单1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv审批单1_DataBindingComplete);
            dgv审批单1.DataSource = dt;
            
            dgv审批单1.ReadOnly = true;
        }

        void dgv审批单1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv审批单1.AllowUserToAddRows = false;
            dgv审批单1.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv审批单1);
        }



        DataTable get产品退货审批单1(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货审批单1 where 申请日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货审批单1");
            da.Fill(dt);
            return dt;
        }

        //private void btn添加退货审批单1_Click(object sender, EventArgs e)
        //{
        //    产品退货申请单 form = new 产品退货申请单(mainform);
        //    form.Show();
        //}

        private void btn查询退货审批单1_Click(object sender, EventArgs e)
        {
            dt = get产品退货审批单1(dtp产品退货审批单1开始时间.Value, dtp产品退货审批单1结束时间.Value, tb审批单1订单号.Text, tb审批单1客户名称.Text);
            dgv审批单1.DataSource = dt;
        }
        #endregion

        #region 产品退货审批单2

        void init产品退货审批单2()
        {
            dtp退货审批单2开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货审批单2结束时间.Value = DateTime.Now;
            dt = get产品退货审批单2(dtp退货审批单2开始时间.Value, dtp退货审批单2结束时间.Value, tb审批单2订单号.Text, tb审批单2客户名称.Text);
            dgv审批单2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv审批单2_DataBindingComplete);
            dgv审批单2.DataSource = dt;
            dgv审批单2.ReadOnly = true;
        }

        void dgv审批单2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv审批单2.AllowUserToAddRows = false;
            dgv审批单2.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv审批单2);
        }


        DataTable get产品退货审批单2(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货审批单2 where 申请日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货审批单2");
            da.Fill(dt);
            return dt;
        }

        //private void btn添加退货审批单2_Click(object sender, EventArgs e)
        //{
        //    产品退货申请单 form = new 产品退货申请单(mainform);
        //    form.Show();
        //}

        private void btn查询退货审批单2_Click(object sender, EventArgs e)
        {
            dt = get产品退货审批单2(dtp退货审批单2开始时间.Value, dtp退货审批单2结束时间.Value, tb审批单2订单号.Text, tb审批单2客户名称.Text);
            dgv审批单2.DataSource = dt;
        }
        #endregion

        #region 产品退货接收单

        void init产品退货接收单()
        {
            dtp退货接收单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货接收单结束时间.Value = DateTime.Now;
            dt = get产品退货接收单(dtp退货接收单开始时间.Value, dtp退货接收单结束时间.Value, tb退货接收单订单号.Text, tb接收单客户名称.Text);
            dgv产品退货接收单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv产品退货接收单_DataBindingComplete);
            dgv产品退货接收单.DataSource = dt;
            dgv产品退货接收单.ReadOnly = true;
        }

        void dgv产品退货接收单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品退货接收单.AllowUserToAddRows = false;
            dgv产品退货接收单.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv产品退货接收单);
        }



        DataTable get产品退货接收单(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货接收单 where 接收日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货接收单");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货接收单_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 产品退货审批单2 where 批准结果=1 and 状态='已批准'", mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                String ids = mySystem.Other.InputDataGridView.getIDs("", dt, false);
                // 把id变成订单号
                Int32 退货申请ID = Convert.ToInt32(dt.Select("ID=" + ids)[0]["产品退货申请单ID"]);
                产品退货接收单 form = new 产品退货接收单(mainform, 退货申请ID);
                form.Show();
            }
            catch (Exception ee)
            {
            }
            if (dgv产品退货接收单.Rows.Count > 0)
                dgv产品退货接收单.FirstDisplayedScrollingRowIndex = dgv产品退货接收单.Rows.Count - 1;
        }

        private void btn查询退货接收单_Click(object sender, EventArgs e)
        {
            dt = get产品退货接收单(dtp退货接收单开始时间.Value, dtp退货接收单结束时间.Value, tb退货接收单订单号.Text, tb接收单客户名称.Text);
            dgv产品退货接收单.DataSource = dt;
        }
        #endregion


        #region 产品退货请验单

        void init产品退货请验单()
        {
            dtp退货请验单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货请验单结束时间.Value = DateTime.Now;
            dt = get产品退货请验单(dtp退货请验单开始时间.Value, dtp退货请验单结束时间.Value, tb退货请验单销售订单.Text, tb退货请验单客户名称.Text);
            dgv退货请验单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv退货请验单_DataBindingComplete);
            dgv退货请验单.DataSource = dt;
            dgv退货请验单.ReadOnly = true;
        }

        void dgv退货请验单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv退货请验单.AllowUserToAddRows = false;
            dgv退货请验单.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv退货请验单);
        }




        DataTable get产品退货请验单(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货请验单 where 请验日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货请验单");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货请验单_Click(object sender, EventArgs e)
        {
           
        }

        private void btn查询退货请验单_Click(object sender, EventArgs e)
        {
            dt = get产品退货请验单(dtp退货请验单开始时间.Value, dtp退货请验单结束时间.Value, tb退货请验单销售订单.Text, tb退货请验单客户名称.Text);
            dgv退货请验单.DataSource = dt;
        }
        #endregion

        #region 产品退货评审单1

        void init产品退货评审单1()
        {
            dtp退货评审单1开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货评审单1结束时间.Value = DateTime.Now;
            dt = get产品退货评审单1(dtp退货评审单1开始时间.Value, dtp退货评审单1结束时间.Value, tb评审单1销售订单.Text, tb评审单1客户名称.Text);
            dgv评审单1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv评审单1_DataBindingComplete);
            dgv评审单1.DataSource = dt;
            dgv评审单1.ReadOnly = true;
        }

        void dgv评审单1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv评审单1.AllowUserToAddRows = false;
            dgv评审单1.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv评审单1);
        }


        DataTable get产品退货评审单1(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货评审单1 where 评审日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货评审单1");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货评审单1_Click(object sender, EventArgs e)
        {

        }

        private void btn查询退货评审单1_Click(object sender, EventArgs e)
        {
            dt = get产品退货评审单1(dtp退货评审单1开始时间.Value, dtp退货评审单1结束时间.Value, tb评审单1销售订单.Text, tb评审单1客户名称.Text);
            dgv评审单1.DataSource = dt;
        }
        #endregion

        #region 产品退货评审单2

        void init产品退货评审单2()
        {
            dtp退货评审单2开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货评审单2结束时间.Value = DateTime.Now;
            dt = get产品退货评审单2(dtp退货评审单2开始时间.Value, dtp退货评审单2结束时间.Value, tb评审单2销售订单.Text, tb评审单2客户名称.Text);
            dgv评审单2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv评审单2_DataBindingComplete);
            dgv评审单2.DataSource = dt;
            dgv评审单2.ReadOnly = true;
        }

        void dgv评审单2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv评审单2.AllowUserToAddRows = false;
            dgv评审单2.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv评审单2);
        }


        DataTable get产品退货评审单2(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货评审单2 where 评审日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货评审单2");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货评审单2_Click(object sender, EventArgs e)
        {

        }

        private void btn查询退货评审单2_Click(object sender, EventArgs e)
        {
            dt = get产品退货评审单2(dtp退货评审单2开始时间.Value, dtp退货评审单2结束时间.Value, tb评审单2销售订单.Text, tb评审单2客户名称.Text); 
            dgv评审单2.DataSource = dt;
        }
        #endregion

        #region 产品退货记录

        void init产品退货记录()
        {
            dtp退货记录开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp退货记录结束时间.Value = DateTime.Now;
            dt = get产品退货记录(dtp退货记录开始时间.Value, dtp退货记录结束时间.Value, tb退货记录销售订单.Text, tb退货记录客户名称.Text);
            dgv退货记录.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv退货记录_DataBindingComplete);
            dgv退货记录.DataSource = dt;
            dgv退货记录.ReadOnly = true;
        }

        void dgv退货记录_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv退货记录.AllowUserToAddRows = false;
            dgv退货记录.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv退货记录);
        }



        DataTable get产品退货记录(DateTime start, DateTime end, string oderNO, string name)
        {
            string sql = "select * from 产品退货记录 where 申请日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%'";

            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, oderNO, name), mySystem.Parameter.conn);
            DataTable dt = new DataTable("产品退货记录2");
            da.Fill(dt);
            return dt;
        }

        private void btn添加退货记录_Click(object sender, EventArgs e)
        {

        }

        private void btn查询退货记录_Click(object sender, EventArgs e)
        {
            dt = get产品退货记录(dtp退货记录开始时间.Value, dtp退货记录结束时间.Value, tb退货记录销售订单.Text, tb退货记录客户名称.Text);
            dgv退货记录.DataSource = dt;
        }
        #endregion


        

    }
}
