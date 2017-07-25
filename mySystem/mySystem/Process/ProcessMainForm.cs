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
using mySystem.Process.Bag.PTV;
using mySystem.Process.Bag.BTV;
using 订单和库存管理;
using mySystem.Process.Stock;
using mySystem.Process.灭菌;

namespace mySystem
{
    public partial class ProcessMainForm : BaseForm
    {
        MainForm mform = null;
        public ExtructionMainForm extruform = null;
        CleanCutMainForm cleancutform = null;
        CSBagMainForm csbagform = null;
        LDPEMainForm ldpebagform = null;
        PTVMainForm ptvbagform = null;
        BTVMainForm btvbagform = null;

        public ProcessMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            mform = mainform;
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
            Btn吹膜.BackColor = Color.FromArgb(138, 158, 196);
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();

            extruform = new ExtructionMainForm(mform);
            Parameter.parentExtru = extruform;      
            extruform.TopLevel = false;
            extruform.FormBorderStyle = FormBorderStyle.None;
            extruform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(extruform);
            extruform.Show();
        }

        //清洁分切
        private void CleanBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 2;
            Parameter.InitCon();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromArgb(138, 158, 196);
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();        

            cleancutform = new CleanCutMainForm(mform);         
            cleancutform.TopLevel = false;
            cleancutform.FormBorderStyle = FormBorderStyle.None;
            cleancutform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(cleancutform);
            cleancutform.Show();
            Parameter.parentClean = cleancutform;

        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromArgb(138, 158, 196);
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            if (Panel制袋.Visible == true)
            {
                Panel其他按钮.Location = new Point(0, 130);
                //PlanBtn.Location = new Point(3, 133);
                Panel制袋.Visible = false;
            }
            else
            {
                Panel其他按钮.Location = new Point(0, 361);
                //PlanBtn.Location = new Point(3, 361);
                Panel制袋.Visible = true;
 
            }          

        }

        //CS制袋
        private void CSbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 3;
            Parameter.InitCon();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromArgb(138, 158, 196);
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");

            csbagform = new CSBagMainForm(mform);
            csbagform.TopLevel = false;
            csbagform.FormBorderStyle = FormBorderStyle.None;
            csbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(csbagform);
            csbagform.Show();
            Parameter.parentCS = csbagform;
            
        }

        //LDPE制袋
        private void LDPEbagBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromArgb(138, 158, 196);
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");

            ldpebagform = new LDPEMainForm();
            ldpebagform.TopLevel = false;
            ldpebagform.FormBorderStyle = FormBorderStyle.None;
            ldpebagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(ldpebagform);
            ldpebagform.Show();
            Parameter.parentLDPE = ldpebagform;
        }

        //连续袋
        private void bag3Btn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromArgb(138, 158, 196);
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");
        }

        //PTV制袋
        private void PTVbagBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromArgb(138, 158, 196);
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");

            ptvbagform = new PTVMainForm(); 
            ptvbagform.TopLevel = false;
            ptvbagform.FormBorderStyle = FormBorderStyle.None;
            ptvbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(ptvbagform);
            ptvbagform.Show();
            Parameter.parentPTV = ptvbagform;
        }

        //BPV制袋
        private void BTVbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 6;
            Parameter.InitCon();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromArgb(138, 158, 196);
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");

            btvbagform = new BTVMainForm();
            btvbagform.TopLevel = false;
            btvbagform.FormBorderStyle = FormBorderStyle.None;
            btvbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(btvbagform);
            btvbagform.Show();
            Parameter.parentBPV = btvbagform;

        }

        //防护罩
        private void bag6Btn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            otherBtnColor();
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromArgb(138, 158, 196);
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 5;
            Parameter.InitCon();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromArgb(138, 158, 196);
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();

            灭菌mainform myDlg = new 灭菌mainform();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //生产计划
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromArgb(138, 158, 196);
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            //PlanForm myDlg = new PlanForm();
            //myDlg.TopLevel = false;
            //myDlg.FormBorderStyle = FormBorderStyle.None;
            //myDlg.Size = ProducePanelRight.Size;
            //ProducePanelRight.Controls.Add(myDlg);
            //myDlg.Show();
        }

        //订单管理
        private void OrderBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromArgb(138, 158, 196);
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            bagBtnColor();
            订单管理 myDlg = new 订单管理();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //库存管理
        private void StockBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromArgb(138, 158, 196);
            bagBtnColor();

            库存管理主界面 myDlg = new 库存管理主界面();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        

        //其他按钮背景色不高亮
        private void otherBtnColor()
        {
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
        }

        //制袋组按钮背景色不高亮
        private void bagBtnColor()
        {
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");
        }


    }
}
