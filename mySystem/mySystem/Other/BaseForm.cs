using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace mySystem
{
    public partial class BaseForm : Form
    {
        String DEFAULT_DGV_COLUMN_WIDTH = "100";

        public MainForm mainform;
        public BaseForm(MainForm mForm)
        {
            mainform = mForm;
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        public BaseForm()
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        public virtual void CheckResult()
        {
        }

        #region dgv width function

        

        protected String readDGVWidthFromSetting(DataGridView dgv)
        {
            AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            String key = dgv.Name + "@" + this.GetType().ToString() + "@ColumnWidths";
            if (appSettings.Settings[key] != null)
            {
                return appSettings.Settings[key].Value;
            }
            else
            {
                return DEFAULT_DGV_COLUMN_WIDTH;
            }
            
        }

        protected String readDGVWidthFromSetting(DataGridView dgv, String tbl)
        {
            AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            String key = tbl + "@" + dgv.Name + "@" + this.GetType().ToString() + "@ColumnWidths";
            if (appSettings.Settings[key] != null)
            {
                return appSettings.Settings[key].Value;
            }
            else
            {
                return DEFAULT_DGV_COLUMN_WIDTH;
            }
        }


        protected void setDGVWidth(DataGridView dgv, int w)
        {
            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                dgvc.Width = w;
            }
        }

        protected void setDGVWidth(DataGridView dgv, String ws)
        {
            string[] widths = ws.Split('|');
            if (widths.Length != dgv.Columns.Count)
            {
                MessageBox.Show(this.GetType().ToString() +"中的"+dgv.Name+"列宽设置失败，列数不匹配");
                setDGVWidth(dgv, Convert.ToInt32(DEFAULT_DGV_COLUMN_WIDTH));
            }
            else
            {
                for (int i = 0; i < widths.Length; ++i)
                {
                    dgv.Columns[i].Width = Convert.ToInt32(widths[i]);
                }
            }
        }

        protected String getDGVWidth(DataGridView dgv)
        {
            int count = dgv.Columns.Count;
            int[] widths = new int[count];
            
            for (int i = 0; i < dgv.Columns.Count; ++i)
            {
                widths[i] = dgv.Columns[i].Width;
            }
            return String.Join("|", widths);
        }



        protected void writeDGVWidthToSetting(DataGridView dgv)
        {
            AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            String key = dgv.Name + "@" + this.GetType().ToString() + "@ColumnWidths";
            if (appSettings.Settings[key] != null)
            {
                appSettings.Settings[key].Value = getDGVWidth(dgv);
            }
            else
            {
                appSettings.Settings.Add(key, getDGVWidth(dgv));
            }
            appSettings.CurrentConfiguration.Save();
        }

        protected void writeDGVWidthToSetting(DataGridView dgv, String tbl)
        {
            AppSettingsSection appSettings = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings;
            String key = tbl + "@" + dgv.Name + "@" + this.GetType().ToString() + "@ColumnWidths";
            if (appSettings.Settings[key] != null)
            {
                appSettings.Settings[key].Value = getDGVWidth(dgv);
            }
            else
            {
                appSettings.Settings.Add(key, getDGVWidth(dgv));
            }
            appSettings.CurrentConfiguration.Save();
        }

        protected bool isWidthDigit(string width)
        {
            foreach (char c in width)
            {
                if (!Char.IsDigit(c)) return false;
            }
            return true;
        }


        protected void readDGVWidthFromSettingAndSet(DataGridView dgv)
        {
            string widths = readDGVWidthFromSetting(dgv);
            if (isWidthDigit(widths)) setDGVWidth(dgv, Convert.ToInt32(widths));
            else setDGVWidth(dgv, widths);
        }

        protected void readDGVWidthFromSettingAndSet(DataGridView dgv, String tbl)
        {
            string widths = readDGVWidthFromSetting(dgv, tbl);
            if (isWidthDigit(widths)) setDGVWidth(dgv, Convert.ToInt32(widths));
            else setDGVWidth(dgv, widths);
        }
        #endregion



    }
}
