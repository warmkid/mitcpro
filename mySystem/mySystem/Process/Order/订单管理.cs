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

        HashSet<String> hs业务类型, hs销售类型, hs客户简称, hs销售部门, hs币种;

        public 订单管理()
        {
            InitializeComponent();
            // 连接数据库
            conn = new OleDbConnection(strConnect);
            conn.Open();

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init销售订单();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }

        }

        void init销售订单()
        {
            dtp销售订单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp销售订单结束时间.Value = DateTime.Now;
            cmb销售订单审核状态.SelectedItem = "待审核";
            dt = get销售订单(dtp销售订单开始时间.Value, dtp销售订单结束时间.Value, tb销售订单号.Text, cmb销售订单审核状态.Text);
            dgv销售订单.DataSource = dt;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);

            get销售订单OtherData();
        }

        private void get销售订单OtherData()
        {
            hs币种 = new HashSet<string>();
            hs客户简称 = new HashSet<string>();
            hs销售部门 = new HashSet<string>();
            hs销售类型 = new HashSet<string>();
            hs业务类型 = new HashSet<string>();
        }

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV销售订单格式();
        }

        private void setDGV销售订单格式()
        {
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.Columns["ID"].Visible = false;
            // 下拉框的
        }

        DataTable get销售订单(DateTime start, DateTime end, string code, string status)
        {
             //TODO 添加销售订单的数据库
            string sql = "select * from 销售订单 where 订单日期 between #{0}# and #{1}# and 状态='{2}' and 订单号 like '%{3}%'";
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

        
    }
}
