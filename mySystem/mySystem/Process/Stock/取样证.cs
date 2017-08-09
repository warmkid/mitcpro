using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Stock
{
    // TODO 打印
    public partial class 取样证 : BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        public 取样证(int id)
        {
            InitializeComponent();
            
            conn = new OleDbConnection(strConnect);
            conn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 取样记录详细信息 where ID=" + id, conn);
            DataTable dt = new DataTable("取样记录详细信息");

            da.Fill(dt);
            DataRow dr = dt.Rows[0];
            tb物料代码.Text = dr["物料代码"].ToString();
            tb物料名称.Text = dr["物料名称"].ToString();
            tb数量.Text = dr["数量"].ToString();
            tb取样人.Text = dr["取样人"].ToString();
            tb取样量.Text = dr["取样量"].ToString();
            tb本厂批号.Text = dr["本厂批号"].ToString();

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印机选择.Items.Add(sPrint);
            }

        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            // TODO 完成打印

            // TODO 记录
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 取样证打印记录 where 0=1", conn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable("取样证打印记录");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["物料代码"] = tb物料代码.Text;
            dr["本厂批号"] = tb本厂批号.Text;
            dr["取样人"] = tb取样人.Text;
            dr["取样时间"] = dtp取样时间.Value;
            dr["打印人"] = mySystem.Parameter.userName;
            dr["打印时间"] = DateTime.Now;
            dt.Rows.Add(dr);
            da.Update(dt);
        }
    }
}
