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
    public partial class 订单管理 : mySystem.BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        HashSet<String> hs业务类型, hs销售类型, hs客户简称, hs销售部门, hs币种;

        public 订单管理(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            // 连接数据库
            conn = new OleDbConnection(strConnect);
            conn.Open();
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init销售订单();
                    break;
                case 1:
                    init采购需求单();
                    break;
                case 2:
                    break;
            }
            dgv销售订单.CellDoubleClick += new DataGridViewCellEventHandler(dgv销售订单_CellDoubleClick);
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init销售订单();
                    break;
                case 1:
                    init采购需求单();
                    break;
                case 2:
                    break;
            } 
        }

        #region 销售订单
        void dgv销售订单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgv销售订单[0, e.RowIndex].Value);
            mySystem.Process.Order.销售订单 form = new mySystem.Process.Order.销售订单(mainform, id);
            form.Show();
        }

        void init销售订单()
        {
            dtp销售订单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp销售订单结束时间.Value = DateTime.Now;
            cmb销售订单审核状态.SelectedItem = "待审核";
            dt = get销售订单(dtp销售订单开始时间.Value, dtp销售订单结束时间.Value, tb销售订单号.Text, cmb销售订单审核状态.Text);
            dgv销售订单.DataSource = dt;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);
            dgv销售订单.ReadOnly = true;
            //get销售订单OtherData();
        }

        //private void get销售订单OtherData()
        //{
        //    hs币种 = new HashSet<string>();
        //    hs客户简称 = new HashSet<string>();
        //    hs销售部门 = new HashSet<string>();
        //    hs销售类型 = new HashSet<string>();
        //    hs业务类型 = new HashSet<string>();
        //}

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV销售订单格式();
        }

        private void setDGV销售订单格式()
        {
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.Columns["ID"].Visible = false;
        }

        DataTable get销售订单(DateTime start, DateTime end, string code, string status)
        {
            string sql = "select * from 销售订单 where 订单日期 between #{0}# and #{1}# and 状态 like '%{2}%' and 订单号 like '%{3}%'";
            
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, start, end, status, code), mySystem.Parameter.connOle);
            DataTable dt = new DataTable("销售订单");
            da.Fill(dt);
            return dt;
        }

        private void btn查询销售订单_Click(object sender, EventArgs e)
        {
            dt = get销售订单(dtp销售订单开始时间.Value, dtp销售订单结束时间.Value, tb销售订单号.Text, cmb销售订单审核状态.Text);
            dgv销售订单.DataSource = dt;
        }

        private void btn添加销售订单_Click(object sender, EventArgs e)
        {
            mySystem.Process.Order.销售订单 form = new mySystem.Process.Order.销售订单(mainform);
            form.Show();
        }
        #endregion

        #region 采购需求单
        void init采购需求单()
        {
            dtp采购需求单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp采购需求单结束时间.Value = DateTime.Now;
            cmb采购需求单审核状态.SelectedItem = "待审核";
            dt = get采购需求单(dtp采购需求单开始时间.Value, dtp采购需求单结束时间.Value, tb用途.Text, cmb采购需求单审核状态.Text);
            dgv采购需求单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购需求单_DataBindingComplete);
            dgv采购需求单.DataSource = dt;
            dgv采购需求单.ReadOnly = true;

        }

        void dgv采购需求单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV采购需求单格式();
        }

        private void setDGV采购需求单格式()
        {
            dgv采购需求单.AllowUserToAddRows = false;
            dgv采购需求单.Columns["ID"].Visible = false;
        }

        

        private DataTable get采购需求单(DateTime start, DateTime end, string yongtu, string status)
        {
            string sql = "select * from 采购需求单 where 申请日期 between #{0}# and #{1}# and 状态 like '*{2}*' and 用途 like '*{3}*'";
            //string sql = "select * from 采购需求单 where 申请日期 between #{0}# and #{1}#";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, start, end, status, yongtu), mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, start, end), mySystem.Parameter.connOle);
            DataTable dt = new DataTable("采购需求单");
            da.Fill(dt);
            return dt;
        }

        #endregion

        private void btn查询采购需求单_Click(object sender, EventArgs e)
        {
            dt = get采购需求单(dtp采购需求单开始时间.Value, dtp采购需求单结束时间.Value, tb用途.Text, cmb采购需求单审核状态.Text);
            dgv销售订单.DataSource = dt;
        }

        private void btn添加采购需求单_Click(object sender, EventArgs e)
        {
            // 获取所有审核完成的订单
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 状态='审核完成'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            String ids = mySystem.Other.InputDataGridView.getIDs("", dt, false);
            // 把id变成订单号
            string 订单号 = dt.Select("ID=" + ids)[0]["订单号"].ToString();
            mySystem.Process.Order.采购需求单 form = new mySystem.Process.Order.采购需求单(mainform, 订单号);
            form.Show();
        }

    }
}
