using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //DeleteOrderBtn.Image = Image.FromFile("D:\\C Sharp\\mitcpro\\mySystem\\mySystem\\pic\\delete.png");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
 
        private void AddOrderBtn_Click(object sender, EventArgs e)
        {
            AddOrderForm orderDlg = new AddOrderForm();
            orderDlg.Show();
        }

        private void BlowBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            BlowForm myDlg = new BlowForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void PlanBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            PlanForm myDlg = new PlanForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void StockCheckBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            StockCheckForm myDlg = new StockCheckForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void StockOrderBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            StockOrderForm myDlg = new StockOrderForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void InOutListBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            InOutListForm myDlg = new InOutListForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void BuyBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            BuyForm myDlg = new BuyForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void SystemSetBtn_Click(object sender, EventArgs e)
        {
            SystemPanelRight.Controls.Clear();
            SystemForm myDlg = new SystemForm();
            myDlg.TopLevel = false;
            myDlg.Dock = DockStyle.Fill;//把子窗体设置为控件
            myDlg.FormBorderStyle = FormBorderStyle.None;
            SystemPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

    }
}
