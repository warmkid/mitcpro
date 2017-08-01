using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class SettingMainForm : BaseForm
    {       
        public SettingMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            RoleInit();

        }

        private void RoleInit()
        {
            switch (Parameter.userRole)
            {
                case 1:
                    Btn人员.Enabled = true;
                    Btn制袋.Enabled = false;
                    Btn吹膜.Enabled = false;
                    Btn清洁分切.Enabled = false;
                    Btn灭菌.Enabled = false;
                    break;
                case 2:
                    Btn人员.Enabled = true;
                    Btn制袋.Enabled = false;
                    Btn吹膜.Enabled = false;
                    Btn清洁分切.Enabled = false;
                    Btn灭菌.Enabled = false;
                    break;
                case 3:
                    Btn人员.Enabled = true;
                    Btn制袋.Enabled = true;
                    Btn吹膜.Enabled = true;
                    Btn清洁分切.Enabled = true;
                    Btn灭菌.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void ExtruSetBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 1;
            Parameter.InitCon();
            BtnColor();
            Btn吹膜.BackColor = Color.FromArgb(138, 158, 196);
            
            SettingPanelRight.Controls.Clear();
            吹膜设置 myDlg = new 吹膜设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }


        private void Btn制袋_Click(object sender, EventArgs e)
        {
            BtnColor();
            Btn制袋.BackColor = Color.FromArgb(138, 158, 196);            
            SettingPanelRight.Controls.Clear();
                   
            if (Panel制袋.Visible == true)
            {
                Btn灭菌.Location = new Point(3, 130);
                Btn人员.Location = new Point(3, 176);
                Panel制袋.Visible = false;
            }
            else
            {
                Btn灭菌.Location = new Point(3, 366);
                Btn人员.Location = new Point(3, 409);
                Panel制袋.Visible = true;

            }    
        }


        private void PeopleSetBtn_Click(object sender, EventArgs e)
        {
            Parameter.InitConnUser();
            BtnColor();
            Btn人员.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
            SetPeopleForm myDlg = new SetPeopleForm(base.mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();

        }

        private void Btn清洁分切_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 2;
            Parameter.InitCon();
            BtnColor();
            Btn清洁分切.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
            清洁分切设置 myDlg = new 清洁分切设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void Btn灭菌_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 5;
            Parameter.InitCon();
            BtnColor();
            Btn灭菌.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
            灭菌设置 myDlg = new 灭菌设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        private void BtnCS制袋_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 3;
            Parameter.InitCon();
            BtnColor();
            BtnCS制袋.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();

            CS制袋设置 csbagform = new CS制袋设置();
            csbagform.TopLevel = false;
            csbagform.FormBorderStyle = FormBorderStyle.None;
            csbagform.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(csbagform);
            csbagform.Show();
        }
        
        private void BtnPE制袋_Click(object sender, EventArgs e)
        {
            BtnColor();
            BtnPE制袋.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();

        }

        private void Btn连续袋_Click(object sender, EventArgs e)
        {
            BtnColor();
            Btn连续袋.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
        }

        private void BtnPTV制袋_Click(object sender, EventArgs e)
        {
            BtnColor();
            BtnPTV制袋.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
        }

        private void BtnBPV制袋_Click(object sender, EventArgs e)
        {
            BtnColor();
            BtnBPV制袋.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
        }

        private void Btn防护罩_Click(object sender, EventArgs e)
        {
            BtnColor();
            Btn防护罩.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
        }

        //按钮背景色不高亮
        private void BtnColor()
        {
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn人员.BackColor = Color.FromName("ControlLightLight");
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");
        }


    }
}
