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
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;

        public QueryMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            
        }

        //吹膜按钮
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 1;
            Parameter.InitCon();
            ExtructionBtn.BackColor = Color.FromArgb(138, 158, 196);
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
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
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromArgb(138, 158, 196);
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            QueryPanelRight.Controls.Clear();
            
        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromArgb(138, 158, 196);
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            QueryPanelRight.Controls.Clear();
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromArgb(138, 158, 196);
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            QueryPanelRight.Controls.Clear();
        }

        //生产指令查询
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 1;
            Parameter.InitCon();
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromArgb(138, 158, 196);
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
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
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromArgb(138, 158, 196);
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            QueryPanelRight.Controls.Clear();
        }

        //库存查询
        private void StockBtn_Click(object sender, EventArgs e)
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromArgb(138, 158, 196);
            QueryPanelRight.Controls.Clear();
        }
    }
}
