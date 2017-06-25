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
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;
        MainForm mform = null;
        
        public SettingMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            mform = mainform;
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

        }


        private void ExtruSetBtn_Click(object sender, EventArgs e)
        {
            ExtruSetBtn.BackColor = Color.FromArgb(138, 158, 196);
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromName("ControlLightLight");
            SettingPanelRight.Controls.Clear();
            SetExtruForm myDlg = new SetExtruForm(mform);
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
            ExtruSetBtn.BackColor = Color.FromName("ControlLightLight");
            SystemSetBtn.BackColor = Color.FromName("ControlLightLight");
            PeopleSetBtn.BackColor = Color.FromArgb(138, 158, 196);
            SettingPanelRight.Controls.Clear();
        }


        
    }
}
