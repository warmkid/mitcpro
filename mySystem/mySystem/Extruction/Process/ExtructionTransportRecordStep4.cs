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

        private class transport
        {
            public string date; //传料日期            
            public string time; //时间
            public string material; //物料代码
            public string num;  //数量
            public string weight;  //kg /件
            public string numperkg; //数量/kg
            public bool wetherbag;   //是否包装完好
            public bool wetherclean;   //是否清洁合格
            public string recorder; //操作人
            public string checker; //复核人

            public transport()
            {
                date = "";
                time = "";
                material = "SPM-PE";
                num = "";
                weight = "";
                numperkg = "";
                wetherbag = true;
                wetherclean = true;
                recorder = "";
                checker = "";
            }
        }
        private transport transportdata = new transport();
        private transport[] transportlist = null;


        public ExtructionTransportRecordStep4(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            DataTabelInitialize();
        }


        private void DataTabelInitialize()
        {
            ///***********************表格数据初始化************************///
            //添加列
            dt.Columns.Add("传料日期", typeof(String));
            dt.Columns["传料日期"].ReadOnly = true;
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
            //添加按钮列
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.TransportRecordView.Columns.Add(MyButtonColumn);
            this.TransportRecordView.AllowUserToAddRows = false;
            //设置对齐
            this.TransportRecordView.RowHeadersVisible = false;
            this.TransportRecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.TransportRecordView.Columns.Count; i++)
            {
                this.TransportRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.TransportRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.TransportRecordView.Columns[i].MinimumWidth = 100;
            }
            this.TransportRecordView.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.TransportRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.TransportRecordView.ColumnHeadersHeight = 40;
            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.TransportRecordView.Columns["传料日期\r2016年"].Width = 80;
        }        

        //添加单行模板
        private void AddRowLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dt.NewRow();
            rowline["传料日期"] = DateTime.Now.ToLongDateString().ToString();
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

        //删除单条记录
        private void TransportRecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TransportRecordView.Columns[e.ColumnIndex].Name == "删除该条记录")
            {
                if (Recordnum > 0)
                {
                    this.TransportRecordView.Rows.RemoveAt(e.RowIndex);//删除行
                    Recordnum = Recordnum - 1;
                }
            }
        }

        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRowLine();
        }

        public void DataSave()
        {
            transportlist = new transport[TransportRecordView.Rows.Count];
            for (int i = 0; i < TransportRecordView.Rows.Count; i++)
            {
                transportdata.date = TransportRecordView.Rows[i].Cells[0].Value.ToString();
                transportdata.time = TransportRecordView.Rows[i].Cells[1].Value.ToString();
                transportdata.material = TransportRecordView.Rows[i].Cells[2].Value.ToString();
                transportdata.num = TransportRecordView.Rows[i].Cells[3].Value.ToString();
                transportdata.weight = TransportRecordView.Rows[i].Cells[4].Value.ToString();
                transportdata.numperkg = TransportRecordView.Rows[i].Cells[5].Value.ToString();
                transportdata.wetherbag = TransportRecordView.Rows[i].Cells[6].Value.ToString() == "是" ? true : false;
                transportdata.wetherclean = TransportRecordView.Rows[i].Cells[7].Value.ToString() == "是" ? true : false;
                transportdata.recorder = TransportRecordView.Rows[i].Cells[8].Value.ToString();
                transportdata.checker = TransportRecordView.Rows[i].Cells[9].Value.ToString();
                //transportlist.Add(transportdata);
                transportlist[i] = transportdata;
            }                      
        }
    }
}
