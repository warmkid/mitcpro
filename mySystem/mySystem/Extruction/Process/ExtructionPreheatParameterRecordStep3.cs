using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : Form
    {
        private ExtructionProcess extructionformfather = null;
        private DataTable dtInformation = new DataTable();
        private DataTable dtRecord = new DataTable();

        public ExtructionPreheatParameterRecordStep3(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;

            InformationViewInitialize();
            RecordViewInitialize();
            //TabelPaint();
        }

        private void InformationViewInitialize()
        {
            //添加列
            dtInformation.Columns.Add("1", typeof(String));
            dtInformation.Columns.Add("2", typeof(String));
            dtInformation.Columns.Add("3", typeof(String));
            dtInformation.Columns.Add("4", typeof(String));
            //添加内容
            dtInformation.Rows.Add("日期：", "  年  月  日", "记录人：", "");
            dtInformation.Rows.Add("模芯规格：", "（φ     × Gap   ）", "复核人：", "");
            this.InformationView.DataSource = dtInformation;
            //设置
            this.InformationView.ColumnHeadersVisible = false;
            this.InformationView.RowHeadersVisible = false;
            this.InformationView.AllowUserToResizeRows = false;
            this.InformationView.AllowUserToResizeColumns = false;
            this.InformationView.AllowUserToAddRows = false;
            for (int i = 0; i < this.InformationView.Columns.Count; i++)
            {
                this.InformationView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.InformationView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.InformationView.Columns[i].MinimumWidth = 80;
                this.InformationView.Columns[i].ReadOnly = true;
            }
            this.InformationView.Columns[1].ReadOnly = false;
            this.InformationView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.InformationView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            //this.InformationView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders; 
            //this.InformationView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.InformationView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.InformationView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void RecordViewInitialize()
        {
            //添加列
            dtRecord.Columns.Add("1", typeof(String));
            dtRecord.Columns.Add("2", typeof(String));
            dtRecord.Columns.Add("3", typeof(String));
            dtRecord.Columns.Add("4", typeof(String));
            //添加内容
            dtRecord.Rows.Add("日期：", "  年  月  日", "记录人：", "");
            dtRecord.Rows.Add("模芯规格：", "（φ     × Gap   ）", "复核人：", "");
        }

        private void TabelPaint()
        {
            Graphics g = this.CreateGraphics();
            this.Show();
            //出来一个画笔,这只笔画出来的颜色是红的  
            Pen p = new Pen(Brushes.Red);

            //创建两个点  
            Point p1 = new Point(0, 0);
            Point p2 = new Point(1000, 1000);

            //将两个点连起来  
            g.DrawLine(p, p1, p2);
        }

        public void DataSave()
        {

        }

    }
}
