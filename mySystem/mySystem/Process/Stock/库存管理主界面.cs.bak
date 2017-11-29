using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using 订单和库存管理;

namespace mySystem.Process.Stock
{
    public partial class 库存管理主界面 : BaseForm
    {
        public 库存管理主界面(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
        }

        private void Page库存管理_Paint(object sender, PaintEventArgs e)
        {
            库存管理 myDlg = new 库存管理(mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = this.Page库存管理.Size;
            this.Page库存管理.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Page商品设置_Paint(object sender, PaintEventArgs e)
        {
            //商品设置 myDlg = new 商品设置();
            //myDlg.TopLevel = false;
            //myDlg.FormBorderStyle = FormBorderStyle.None;
            //myDlg.Size = this.Page商品设置.Size;
            //this.Page商品设置.Controls.Add(myDlg);
            //myDlg.Show();
        }

        private void Page采购单管理_Paint(object sender, PaintEventArgs e)
        {
            //采购单管理 myDlg = new 采购单管理();
            //myDlg.TopLevel = false;
            //myDlg.FormBorderStyle = FormBorderStyle.None;
            //myDlg.Size = this.Page采购单管理.Size;
            //this.Page采购单管理.Controls.Add(myDlg);
            //myDlg.Show();
        }

        private void btn退货申请_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.退货申请 form = new 退货申请(mainform);
            form.Show();
        }

        private void btn退货记录_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 退货申请 where 状态='未申请'",mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string ids = mySystem.Other.InputDataGridView.getIDs("", dt, false);
            try
            {
                int id = Convert.ToInt32(ids);
                string 退货编码 = dt.Select("ID=" + id)[0]["退货编号"].ToString();
                退货记录 form = new 退货记录(mainform, 退货编码);
                form.Show();
            }
            catch (Exception ee)
            {
            }
            
        }

        private void tabPage退货管理_Paint(object sender, PaintEventArgs e)
        {
            dtp开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp结束时间.Value = DateTime.Now;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
           
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string 退货编号 = dataGridView1.Rows[e.RowIndex].Cells["退货编号"].Value.ToString();
                switch (退货申请or退货记录)
                {
                    case 0:
                        退货申请 form1 = new 退货申请(mainform, 退货编号);
                        form1.Show();
                        break;
                    case 1:
                        退货记录 form2 = new 退货记录(mainform, 退货编号);
                        form2.Show();
                        break;
                }
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        int 退货申请or退货记录 = 0;

        private void btn查询退货申请_Click(object sender, EventArgs e)
        {
            string sql = @"select * from 退货申请 where 退货编号 like '%{0}%' and 客户名称 like '%{1}%' and 退货产品代码 like '%{2}%'";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, tb退货编号.Text, tb客户名称.Text, tb产品代码.Text),mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            退货申请or退货记录 = 0;
        }

        private void btn查询退货记录_Click(object sender, EventArgs e)
        {
            string sql = @"select * from 退货记录 where  退货编号 like '%{0}%' and 客户名称 like '%{1}%' and 退货产品代码 like '%{2}%'";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, tb退货编号.Text, tb客户名称.Text, tb产品代码.Text), mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            退货申请or退货记录 = 1;

        }

        private void btn退货台账查询_Click(object sender, EventArgs e)
        {
            string sql = @"select * from 退货台账 where 退货编号 like '%{0}%' and 客户简称 like '%{1}%' and 产品编码 like '%{2}%' and 退货日期 between #{3}# and #{4}#";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, tb退货编号.Text, tb客户名称.Text, tb产品代码.Text, dtp开始时间.Value, dtp结束时间.Value), mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            退货申请or退货记录 = -1;
        }


    }
}
