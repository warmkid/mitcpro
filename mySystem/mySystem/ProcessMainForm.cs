using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mySystem
{
    public partial class ProcessMainForm : Form
    {
        SqlConnection conn = null;

        public ProcessMainForm(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            ExtructionBtn.Image = Image.FromFile(@"../../pic/Extruction.png", false);
            CleanBtn.Image = Image.FromFile(@"../../pic/Clean.png", false);
            BagBtn.Image = Image.FromFile(@"../../pic/Bag.png", false);
            KillBtn.Image = Image.FromFile(@"../../pic/Kill.png", false);
            PlanBtn.Image = Image.FromFile(@"../../pic/Plan.png", false);
            OrderBtn.Image = Image.FromFile(@"../../pic/Order.png", false);
            StockBtn.Image = Image.FromFile(@"../../pic/Stock.png", false);
        }

        //吹膜
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            ExtructionForm myDlg = new ExtructionForm(conn);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //清洁分切
        private void CleanBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
        }

        //生产计划
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            PlanForm myDlg = new PlanForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //订单管理
        private void OrderBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
        }

        //库存管理
        private void StockBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            StockCheckForm myDlg = new StockCheckForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void ProducePanelLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ProducePanelRight_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
