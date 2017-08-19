using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 订单和库存管理;

namespace mySystem.Process.Stock
{
    public partial class 库存管理主界面 : BaseForm
    {
        public 库存管理主界面(MainForm mainform):base(mainform)
        {
            InitializeComponent();
        }

        private void Page库存管理_Paint(object sender, PaintEventArgs e)
        {
            库存管理 myDlg = new 库存管理();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = this.Page库存管理.Size;
            this.Page库存管理.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Page商品设置_Paint(object sender, PaintEventArgs e)
        {
            商品设置 myDlg = new 商品设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = this.Page商品设置.Size;
            this.Page商品设置.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Page采购单管理_Paint(object sender, PaintEventArgs e)
        {
            采购单管理 myDlg = new 采购单管理();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = this.Page采购单管理.Size;
            this.Page采购单管理.Controls.Add(myDlg);
            myDlg.Show();
        }


    }
}
