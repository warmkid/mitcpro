using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class SettingMainForm : Form
    {
        public SettingMainForm()
        {
            InitializeComponent();
            //SystemSetBtn.Image = Image.FromFile(@"../../pic/SystemSet.png", false);
            //ExtruSetBtn.Image = Image.FromFile(@"../../pic/AuthoritySet.png", false);
            //PeopleSetBtn.Image = Image.FromFile(@"../../pic/PeopleSet.png", false);
        }


        private void ExtruSetBtn_Click(object sender, EventArgs e)
        {
            SettingPanelRight.Controls.Clear();
            SetExtruForm myDlg = new SetExtruForm();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = SettingPanelRight.Size;
            SettingPanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }


        private void SystemSetBtn_Click(object sender, EventArgs e)
        {
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
            SettingPanelRight.Controls.Clear();
        }


        
    }
}
