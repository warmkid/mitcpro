using mySystem.Setting;
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
        string para1 = null;
        string para2 = null;
        string para3 = null;
        string para4 = null;

        public SetExtruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
            InitParameter();
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

        //系数设置部分
        private void InitParameter()
        { 
            //读取数据库并显示
            String tblName = "setting_parameter";
            List<String> readqueryCols = new List<String>(new String[] { "matface_num", "matdens", "ration1", "ration2" });
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(Parameter.connOle, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);

            List<String> data = new List<String> { };
            for (int i = 0; i < queryValsList[0].Count; i++)
            {
                data.Add(queryValsList[0][i].ToString());
            }
            List<Control> textboxes = new List<Control> { tB面数, tB厚度密度, tB参数1, tB参数2 };
            Utility.fillControl(textboxes, data);

        }

        private void ParaSave()
        {
            para1 = tB面数.Text.Trim();
            para2 = tB厚度密度.Text.Trim();
            para3 = tB参数1.Text.Trim();
            para4 = tB参数2.Text.Trim();
            String tblName = "setting_parameter";
            List<String> updateCols = new List<String>(new String[] { "matface_num", "matdens", "ration1", "ration2" });
            List<Object> updateVals = new List<Object>(new Object[] { para1, para2, para3, para4 });
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(Parameter.connOle, tblName, updateCols, updateVals, whereCols, whereVals);
            if (b)
            {
                return;
            }
            else
            {
                MessageBox.Show("参数保存失败", "错误");
                return;
            }

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
                ParaSave();
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
