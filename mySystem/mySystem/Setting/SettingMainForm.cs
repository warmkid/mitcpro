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
                    PeopleSetBtn.Enabled = true;
                    SystemSetBtn.Enabled = false;
                    ExtruSetBtn.Enabled = false;
                    Btn清洁分切.Enabled = false;
                    Btn灭菌.Enabled = false;
                    break;
                case 2:
                    PeopleSetBtn.Enabled = true;
                    SystemSetBtn.Enabled = false;
                    ExtruSetBtn.Enabled = false;
                    Btn清洁分切.Enabled = false;
                    Btn灭菌.Enabled = false;
                    break;
                case 3:
                    PeopleSetBtn.Enabled = true;
                    SystemSetBtn.Enabled = true;
                    ExtruSetBtn.Enabled = true;
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
            ExtruSetBtn.BackColor = Color.FromArgb(138, 158, 196);
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            SettingPanelRight.Controls.Clear();
            吹膜设置 myDlg = new 吹膜设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }


        private void SystemSetBtn_Click(object sender, EventArgs e)
        {
            ExtruSetBtn.BackColor = Color.FromName("ControlLightLight");
            SystemSetBtn.BackColor = Color.FromArgb(138, 158, 196);
            PeopleSetBtn.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            SettingPanelRight.Controls.Clear();
            SettingForm myDlg = new SettingForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
            
        }


        private void PeopleSetBtn_Click(object sender, EventArgs e)
        {
            Parameter.InitConnUser();
            ExtruSetBtn.BackColor = Color.FromName("ControlLightLight");
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromArgb(138, 158, 196);
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
            Btn清洁分切.BackColor = Color.FromArgb(138, 158, 196);
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            ExtruSetBtn.BackColor = Color.FromName("ControlLightLight");
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromName("ControlLightLight");
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
            Parameter.selectCon = 2;
            Parameter.InitCon();
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromArgb(138, 158, 196);
            ExtruSetBtn.BackColor = Color.FromName("ControlLightLight");
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromName("ControlLightLight");
            SettingPanelRight.Controls.Clear();
            灭菌设置 myDlg = new 灭菌设置();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }


        
    }
}
