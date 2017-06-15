using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Setting;

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

            MainProduceBtn.Image = Image.FromFile(@"../../pic/MainProduce.png", false); 
            MainSettingBtn.Image = Image.FromFile(@"../../pic/MainSetting.png", false); 
            MainQueryBtn.Image = Image.FromFile(@"../../pic/MainQuery.png", false); 

        }

        //工序按钮
        private void MainProduceBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ProcessMainForm myDlg = new ProcessMainForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //设置按钮
        private void MainSettingBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            SettingMainForm myDlg = new SettingMainForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

        //台帐查询按钮
        private void MainQueryBtn_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            QueryMainForm myDlg = new QueryMainForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = MainPanel.Size;
            MainPanel.Controls.Add(myDlg);
            myDlg.Show();
        }

    }
}
