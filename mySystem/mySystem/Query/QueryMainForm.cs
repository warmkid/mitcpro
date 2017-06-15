using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Query;

namespace mySystem
{
    public partial class QueryMainForm : Form
    {
        public QueryMainForm()
        {
            InitializeComponent();
            ExtructionBtn.Image = Image.FromFile(@"../../pic/Extruction.png", false);
            CleanBtn.Image = Image.FromFile(@"../../pic/Clean.png", false);
            BagBtn.Image = Image.FromFile(@"../../pic/Bag.png", false);
            KillBtn.Image = Image.FromFile(@"../../pic/Kill.png", false);
            PlanBtn.Image = Image.FromFile(@"../../pic/Plan.png", false);
            OrderBtn.Image = Image.FromFile(@"../../pic/Order.png", false);
            StockBtn.Image = Image.FromFile(@"../../pic/Stock.png", false);
        }

        //吹膜按钮
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
            QueryExtruForm myDlg = new QueryExtruForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //清洁分切
        private void CleanBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }

        //生产计划查询
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }

        //订单查询
        private void OrderBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }

        //库存查询
        private void StockBtn_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
        }
    }
}
