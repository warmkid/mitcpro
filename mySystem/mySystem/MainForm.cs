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
            //Rectangle ScreenArea = Screen.GetWorkingArea(this);
            //ProducePanelLeft.Size = new Size(160, ScreenArea.Height - 260);
            //ProducePanelRight.Size = new Size(ScreenArea.Width - 184, ScreenArea.Height - 260);
            //StockPanelLeft.Size = SystemPanelLeft.Size = ProducePanelLeft.Size;
            //StockPanelRight.Size = SystemPanelRight.Size = ProducePanelRight.Size;
            //this.textBox1.Text = ProducePanelRight.Size.Height.ToString();

            ProducePanelLeft.Size = new Size(160, 672);
            ProducePanelRight.Size = new Size(1180, 672);
            StockPanelLeft.Size = SystemPanelLeft.Size = ProducePanelLeft.Size;
            StockPanelRight.Size = SystemPanelRight.Size = ProducePanelRight.Size;
        }
 
        private void AddOrderBtn_Click(object sender, EventArgs e)
        {
            AddOrderForm orderDlg = new AddOrderForm();
            orderDlg.Show();
        }

        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            ExtructionForm myDlg = new ExtructionForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

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

        private void StockCheckBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            StockCheckForm myDlg = new StockCheckForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = StockPanelRight.Size;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void StockOrderBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            StockOrderForm myDlg = new StockOrderForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = StockPanelRight.Size;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void InOutListBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            InOutListForm myDlg = new InOutListForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = StockPanelRight.Size;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void BuyBtn_Click(object sender, EventArgs e)
        {
            StockPanelRight.Controls.Clear();
            BuyForm myDlg = new BuyForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = StockPanelRight.Size;
            StockPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void SystemSetBtn_Click(object sender, EventArgs e)
        {
            SystemPanelRight.Controls.Clear();
            SettingForm myDlg = new SettingForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SystemPanelRight.Size;
            SystemPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

    }
}
