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
    public partial class ExtructionTransportRecordStep4 : Form
    {
        private ExtructionProcess extructionformfather = null;
        private DataTable dt = new DataTable();
        private int Recordnum = 0;

        public ExtructionTransportRecordStep4(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            DataTabelInitialize();
        }

        private void DataTabelInitialize()
        {
            //添加列
            dt.Columns.Add("传料日期\r2016年", typeof(String));
            dt.Columns.Add("时间", typeof(String));
            dt.Columns.Add("物料代码", typeof(String));
            dt.Columns.Add("数量(件)", typeof(String));
            dt.Columns.Add("kg/件", typeof(String));
            dt.Columns.Add("数量/kg", typeof(String));
            dt.Columns.Add("包装是否完好", typeof(bool));
            dt.Columns.Add("是否清洁合格", typeof(bool));
            dt.Columns.Add("操作人", typeof(String));
            dt.Columns["操作人"].ReadOnly = true;
            dt.Columns.Add("复核人", typeof(String));
            dt.Columns["复核人"].ReadOnly = true;
            AddRowLine();            
            this.TransportRecordView.DataSource = dt;
            this.TransportRecordView.AllowUserToAddRows = false;
            //设置对齐
            this.TransportRecordView.RowHeadersVisible = false;
            this.TransportRecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.TransportRecordView.Columns.Count; i++)
            {
                this.TransportRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.TransportRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.TransportRecordView.Columns[i].MinimumWidth = 80;
            }
            this.TransportRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.ColumnHeadersHeight = 40;
            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.TransportRecordView.Columns["传料日期\r2016年"].Width = 80;
        }


        private void AddRowLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dt.NewRow();
            rowline["传料日期\r2016年"] = "  年   日";
            rowline["时间"] = "";
            rowline["物料代码"] = "SPM-PE";
            rowline["数量(件)"] = "";
            rowline["kg/件"] = "";
            rowline["数量/kg"] = "";
            rowline["包装是否完好"] = true;
            rowline["是否清洁合格"] = true;
            rowline["操作人"] = "";
            rowline["复核人"] = "";
            //添加行
            dt.Rows.Add(rowline);
            Recordnum = Recordnum + 1;
        }

        private void DeleteRecordRowLine()
        {
            if (Recordnum > 0)
            {
                this.TransportRecordView.Rows.RemoveAt(Recordnum - 1);//删除行
                Recordnum = Recordnum - 1;
            }
        }

        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRowLine();
        }

        private void DeleteLineBtn_Click_1(object sender, EventArgs e)
        {
            DeleteRecordRowLine();
        }

        public void DataSave()
        {

        }
    }
}
