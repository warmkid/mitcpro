using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_RunRecord : BaseForm
    {
        public CleanCut_RunRecord(MainForm mainform) : base(mainform)
        {
            InitializeComponent();
        }


        private void RunRecordDgvInitialize()
        {
            //    ///***********************表格数据初始化************************///
            //    //表格界面设置
            //    this.RunRecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            //    this.RunRecordView.RowHeadersVisible = false;
            //    this.RunRecordView.AllowUserToResizeColumns = false;
            //    this.RunRecordView.AllowUserToResizeRows = false;
            //    this.RunRecordView.ColumnHeadersHeight = 50;
            //    for (int i = 0; i < this.RunRecordView.Columns.Count; i++)
            //    {
            //        this.RunRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //        this.RunRecordView.Columns[i].MinimumWidth = 90;
            //        this.RunRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //        this.RunRecordView.Columns[i].ReadOnly = true;
            //    }
            //    this.RunRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    this.RunRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    this.RunRecordView.Columns["生产时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //    this.RunRecordView.Columns["分切速度"].HeaderText = "分切速度\r(m/min)";
            //    this.RunRecordView.Columns["自动张力设定"].HeaderText = "自动张力\r设定(kg)";
            //    this.RunRecordView.Columns["自动张力显示"].HeaderText = "自动张力\r显示(kg)";
            //    this.RunRecordView.Columns["张力输出显示"].HeaderText = "张力输出\r显示(%)";

            //    AddRecordRowLine();
        }

        //添加单行模板
        private void AddRecordRowLine()
        {
            //DataGridViewRow dr = new DataGridViewRow();
            //foreach (DataGridViewColumn c in RunRecordView.Columns)
            //{
            //    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //}

            //dr.Cells[0].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString(); ;
            //dr.Cells[1].Value = "";
            //dr.Cells[2].Value = "";
            //dr.Cells[3].Value = "";
            //dr.Cells[4].Value = "";
            //dr.Cells[5].Value = operator_name;

            //RunRecordView.Rows.Add(dr);
        }

        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            AddRecordRowLine();
        }

        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            //int deletenum = RunRecordView.CurrentRow.Index;
            //this.RunRecordView.Rows.RemoveAt(deletenum);            
        }


    }
}
