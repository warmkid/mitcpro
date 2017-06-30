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
using mySystem.Process.Bag;
using mySystem.Process.CleanCut;
using mySystem.Process.Bag.LDPE;

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
            RoleInit();

        }

        private void RoleInit()
        {
            switch (mform.userRole)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                default:
                    break;
            }
        }


        //吹膜
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
            bagBtnColor();
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
            Parameter.selectCon = 2;
            Parameter.InitCon();
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromArgb(138, 158, 196);
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            ProducePanelRight.Controls.Clear();
            CleanCutMainForm myDlg = new CleanCutMainForm(mform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromArgb(138, 158, 196);
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
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
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromArgb(138, 158, 196);
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            ProducePanelRight.Controls.Clear();
        }

        //生产计划
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromArgb(138, 158, 196);
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
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
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromArgb(138, 158, 196);
            StockBtn.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            ProducePanelRight.Controls.Clear();
        }

        //库存管理
        private void StockBtn_Click(object sender, EventArgs e)
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromArgb(138, 158, 196);
            bagBtnColor();
            ProducePanelRight.Controls.Clear();
            StockCheckForm myDlg = new StockCheckForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //CS制袋
        private void CSbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 3;
            Parameter.InitCon();
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromArgb(138, 158, 196);
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
            CSBagMainForm myDlg = new CSBagMainForm(mform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //LDPE制袋
        private void LDPEbagBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromArgb(138, 158, 196);
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
            LDPEMainForm myDlg = new LDPEMainForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //连续袋
        private void bag3Btn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromArgb(138, 158, 196);
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
        }

        //PTV制袋
        private void PTVbagBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromArgb(138, 158, 196);
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
        }

        //BTV制袋
        private void BTVbagBtn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromArgb(138, 158, 196);
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
        }

        //防护罩
        private void bag6Btn_Click(object sender, EventArgs e)
        {
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromArgb(138, 158, 196);
        }

        //其他按钮背景色不高亮
        private void otherBtnColor()
        {
            ExtructionBtn.BackColor = Color.FromName("ControlLightLight");
            CleanBtn.BackColor = Color.FromName("ControlLightLight");
            BagBtn.BackColor = Color.FromName("ControlLightLight");
            KillBtn.BackColor = Color.FromName("ControlLightLight");
            PlanBtn.BackColor = Color.FromName("ControlLightLight");
            OrderBtn.BackColor = Color.FromName("ControlLightLight");
            StockBtn.BackColor = Color.FromName("ControlLightLight");
        }

        //制袋组按钮背景色不高亮
        private void bagBtnColor()
        {
            CSbagBtn.BackColor = Color.FromName("ControlLightLight");
            LDPEbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag3Btn.BackColor = Color.FromName("ControlLightLight");
            PTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            BTVbagBtn.BackColor = Color.FromName("ControlLightLight");
            bag6Btn.BackColor = Color.FromName("ControlLightLight");
        }


    }
}
