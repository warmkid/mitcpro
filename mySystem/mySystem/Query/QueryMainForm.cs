using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Query;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class QueryMainForm : BaseForm
    {

        public QueryMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            
        }

        //吹膜按钮
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 1;
            Parameter.InitCon();
            BtnColor();
            Btn吹膜.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();

            QueryExtruForm myDlg = new QueryExtruForm(mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //清洁分切
        private void CleanBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 2;
            Parameter.InitCon();
            BtnColor();
            Btn清洁分切.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();

            清洁分切查询 myDlg = new 清洁分切查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            BtnColor();
            Btn制袋.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();

            if (Panel制袋.Visible == true)
            {
                Panel其他按钮.Location = new Point(0, 160);
                Panel制袋.Visible = false;
            }
            else
            {
                Panel其他按钮.Location = new Point(0, 391);
                Panel制袋.Visible = true;
            }   
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 5;
            Parameter.InitCon();
            BtnColor();
            Btn灭菌.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();

            灭菌查询 myDlg = new 灭菌查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //生产指令查询
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            BtnColor();
            Btn生产指令.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();

            QueryInstruForm myDlg = new QueryInstruForm(mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();

        }

        //订单查询
        private void OrderBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();
            BtnColor();
            Btn订单.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();
        }

        //库存查询
        private void StockBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();
            BtnColor();
            Btn库存.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();
        }

        private void BtnPE制袋_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 7;
            Parameter.InitCon();
            QueryPanelRight.Controls.Clear();
            BtnColor();
            BtnPE制袋.BackColor = Color.FromArgb(138, 158, 196);

            PE制袋查询 myDlg = new PE制袋查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void BtnCS制袋_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 3;
            Parameter.InitCon();
            QueryPanelRight.Controls.Clear();
            BtnColor();
            BtnCS制袋.BackColor = Color.FromArgb(138, 158, 196);

            CS制袋查询 myDlg = new CS制袋查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Btn连续袋_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
            BtnColor();
            Btn连续袋.BackColor = Color.FromArgb(138, 158, 196);
        }

        private void BtnPTV制袋_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 8;
            Parameter.InitCon();
            QueryPanelRight.Controls.Clear();
            BtnColor();
            BtnPTV制袋.BackColor = Color.FromArgb(138, 158, 196);

            PTV制袋查询 myDlg = new PTV制袋查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void BtnBPV制袋_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 6;
            Parameter.InitCon();
            QueryPanelRight.Controls.Clear();
            BtnColor();
            BtnBPV制袋.BackColor = Color.FromArgb(138, 158, 196);

            BPV制袋查询 myDlg = new BPV制袋查询();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = QueryPanelRight.Size;
            QueryPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Btn防护罩_Click(object sender, EventArgs e)
        {
            QueryPanelRight.Controls.Clear();
            BtnColor();
            Btn防护罩.BackColor = Color.FromArgb(138, 158, 196);
        }       



        //按钮背景色不高亮
        private void BtnColor()
        {
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单.BackColor = Color.FromName("ControlLightLight");
            Btn库存.BackColor = Color.FromName("ControlLightLight");
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");
        }

    }
}
