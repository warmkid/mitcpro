﻿using mySystem.Setting;
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
using WindowsFormsApplication1;

namespace mySystem
{
    public partial class SetExtruForm : BaseForm
    {
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;

        public SetExtruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;
        }

        private void cleanPanel_Paint(object sender, PaintEventArgs e)
        {
            //cleanPanel.Controls.Clear();
            Setting_CleanArea setcleanDlg = new Setting_CleanArea(base.mainform);
            setcleanDlg.TopLevel = false;
            setcleanDlg.FormBorderStyle = FormBorderStyle.None;
            cleanPanel.Controls.Add(setcleanDlg);
            setcleanDlg.Show();
        }

        private void preHeatPanel_Paint(object sender, PaintEventArgs e)
        {
            PreheatParameterForm preheatDlg = new PreheatParameterForm(base.mainform);
            preheatDlg.TopLevel = false;
            preheatDlg.FormBorderStyle = FormBorderStyle.None;
            preHeatPanel.Controls.Add(preheatDlg);
            preheatDlg.Show();
        }

        private void bfStartPanel_Paint(object sender, PaintEventArgs e)
        {
            Setting_CheckBeforePower bfPowerDlg = new Setting_CheckBeforePower(base.mainform);
            bfPowerDlg.TopLevel = false;
            bfPowerDlg.FormBorderStyle = FormBorderStyle.None;
            bfStartPanel.Controls.Add(bfPowerDlg);
            bfPowerDlg.Show();

        }

    }
}
