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
            cmb销售订单审核状态.SelectedItem = "__待审核";

        }

        //DataTable get销售订单(DateTime start, DateTime end, string code, string status)
        //{
        //    // TODO 添加销售订单的数据库
        //    //string sql = "select * from "
        //    //OleDbDataAdapter da = new OleDbDataAdapter("select ");
        //}

        
    }
}
