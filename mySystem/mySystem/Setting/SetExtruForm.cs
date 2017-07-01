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
        Setting_CleanArea setcleanDlg = null;
        Setting_CheckBeforePower bfPowerDlg = null;
        PreheatParameterForm preheatDlg = null;
        Setting_CleanSite setsiteDlg = null;
        SettingHandOver handoverDlg = null;

        public SetExtruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
        }

        //加载panel
        private void Init()
        {
            //清洁区域设置
            setcleanDlg = new Setting_CleanArea(base.mainform);
            setcleanDlg.TopLevel = false;
            setcleanDlg.FormBorderStyle = FormBorderStyle.None;
            cleanPanel.Controls.Add(setcleanDlg);
            setcleanDlg.Show();

            //开机前确认项目设置
            bfPowerDlg = new Setting_CheckBeforePower(base.mainform);
            bfPowerDlg.TopLevel = false;
            bfPowerDlg.FormBorderStyle = FormBorderStyle.None;
            bfStartPanel.Controls.Add(bfPowerDlg);
            bfPowerDlg.Show();

            //预热参数设置
            preheatDlg = new PreheatParameterForm(base.mainform);
            preheatDlg.TopLevel = false;
            preheatDlg.FormBorderStyle = FormBorderStyle.None;
            preHeatPanel.Controls.Add(preheatDlg);
            preheatDlg.Show();

            //清场点设置
            setsiteDlg = new Setting_CleanSite(base.mainform);
            setsiteDlg.TopLevel = false;
            setsiteDlg.FormBorderStyle = FormBorderStyle.None;
            procClearPanel.Controls.Add(setsiteDlg);
            setsiteDlg.Show();

            //交接班设置
            handoverDlg = new SettingHandOver(base.mainform);
            handoverDlg.TopLevel = false;
            handoverDlg.FormBorderStyle = FormBorderStyle.None;
            this.handoverPanel.Controls.Add(handoverDlg);
            handoverDlg.Show();

 
        }

   
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Parameter.isSqlOk)
            {

            }
            else
            {
                bfPowerDlg.DataSave();
                preheatDlg.DataSave();
                setsiteDlg.DataSave();
                setcleanDlg.DataSave();
                handoverDlg.DataSave();
 
            }
        }

        private void procClearPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cleanPanel_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
