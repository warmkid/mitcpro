using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class ProcessMainForm : BaseForm
    {
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;
        MainForm mform = null;

        public ProcessMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            mform = mainform;
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

        }

        //吹膜
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            ExtructionMainForm myDlg = new ExtructionMainForm(mform);
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
            //ProducePanelRight.Controls.Clear();

            if (bagPanel.Visible == true)
            {
                otherPanel.Location = new Point(0, 130);
                //PlanBtn.Location = new Point(3, 133);
                bagPanel.Visible = false;
            }
            else
            {
                otherPanel.Location = new Point(0, 361);
                //PlanBtn.Location = new Point(3, 361);
                bagPanel.Visible = true;
 
            }


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
