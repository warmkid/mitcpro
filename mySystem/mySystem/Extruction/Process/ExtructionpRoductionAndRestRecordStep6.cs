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
    public partial class ExtructionpRoductionAndRestRecordStep6 : Form
    {
        private ExtructionProcess extructionformfather = null;
        private DataTable dtInformation = new DataTable();
        private DataTable dtRecord = new DataTable();
        private int Recordnum = 0;
        private string productname = "";  //产品名称
        private string productnumber = "";  //产品批号
        private string temperature = "";  //环境温度
        private string humidity = "";  //相对湿度

        private string date = "";  //生产日期
        private bool spot = true;  //班次；true白班；false夜班
        private string checkername = "";  //复核人

        private class record
        {
            public string time; //时间
            public string number; //膜卷编号
            public string length;  //膜卷长度
            public string weight;  //膜卷重量
            public string recorder;  //记录人
            public string outward;   //外观
            public string width;  //宽度
            public string maxthickness;  //最大厚度
            public string minthickness;  //最小厚度
            public string avethickness;  //平均厚度
            public string marginthickness;  //厚度公差
            public string checker;  //检查人
            public string judge;  //判定

            public record()
            {
                time = DateTime.Now.ToLongDateString().ToString();
                number = "";
                length = "";
                weight = "";
                recorder = "";
                outward = "";
                width = "";
                maxthickness = "";
                minthickness = "";
                avethickness = "";
                marginthickness = "";
                checker = "";
                judge = "";
            }
        }
        private record recorddata = new record();
        private record[] recordlist = null;

        public ExtructionpRoductionAndRestRecordStep6(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            InformationViewInitialize();
            RecordViewInitialize();
        }

        private void InformationViewInitialize()
        {
            ///***********************表头数据初始化************************///
            date = DateTime.Now.ToLongDateString().ToString();
            spot = true;
            checkername = "复核人姓名";
            this.Datelabel.Text = "生产日期：" + date;
            this.CheckNameLabel.Text = "复核人:" + checkername;
            this.DatecheckBox.Checked = spot;
            this.NeightcheckBox.Checked = !spot;            
        }

        private void RecordViewInitialize()
        {
            //添加列
            dtRecord.Columns.Add("序号", typeof(String));
            dtRecord.Columns.Add("时间", typeof(String));
            dtRecord.Columns.Add("膜卷编号\r(卷)", typeof(String));
            dtRecord.Columns.Add("膜卷长度\r(m)", typeof(String));
            dtRecord.Columns.Add("膜卷重量\r(kg)", typeof(String));
            dtRecord.Columns.Add("记录人", typeof(String));
            dtRecord.Columns.Add("外观", typeof(String));
            dtRecord.Columns.Add("宽度\r(mm)", typeof(String));
            dtRecord.Columns.Add("最大厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("最小厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("平均厚度\r（μm）", typeof(String));
            dtRecord.Columns.Add("厚度公差\r(%)", typeof(String));
            dtRecord.Columns.Add("检查人", typeof(String));
            dtRecord.Columns.Add("判定", typeof(String));
            //添加内容
            AddRecordRowLine(); 
            this.RecordView.DataSource = dtRecord;
            this.RecordView.AllowUserToAddRows = false;
            //添加按钮列
            DataGridViewButtonColumn MyButtonColumn = new DataGridViewButtonColumn();
            MyButtonColumn.Name = "删除该条记录";
            MyButtonColumn.UseColumnTextForButtonValue = true;
            MyButtonColumn.Text = "删除";
            this.RecordView.Columns.Add(MyButtonColumn);
            this.RecordView.AllowUserToAddRows = false;
            //设置对齐
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].MinimumWidth = 75;
            }
            this.RecordView.Columns[0].MinimumWidth = 40;
            this.RecordView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RecordView.ColumnHeadersHeight = 40;
        }

        private void AddRecordRowLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dtRecord.NewRow();
            rowline["序号"] = (Recordnum+1).ToString();
            rowline["时间"] = "";
            rowline["膜卷编号\r(卷)"] = "";
            rowline["膜卷长度\r(m)"] = "";
            rowline["膜卷重量\r(kg)"] = "";
            rowline["记录人"] = "";
            rowline["外观"] = "";
            rowline["宽度\r(mm)"] = "";
            rowline["最大厚度\r（μm）"] = "";
            rowline["最小厚度\r（μm）"] = "";
            rowline["平均厚度\r（μm）"] = "";
            rowline["厚度公差\r(%)"] = "";
            rowline["检查人"] = "";
            rowline["判定"] = "";
            //添加行
            dtRecord.Rows.InsertAt(rowline, Recordnum); 
            if (Recordnum==0)
            {
                AddTotalLine();
            }
            Recordnum = Recordnum + 1;
        }

        //添加最后一行
        private void AddTotalLine()
        {
            //添加行模板
            DataRow rowline;
            rowline = dtRecord.NewRow();
            rowline["序号"] = "";
            rowline["时间"] = "总计";
            rowline["膜卷编号\r(卷)"] = "";
            rowline["膜卷长度\r(m)"] = "";
            rowline["膜卷重量\r(kg)"] = "";
            rowline["记录人"] = "";
            rowline["外观"] = "";
            rowline["宽度\r(mm)"] = "";
            rowline["最大厚度\r（μm）"] = "";
            rowline["最小厚度\r（μm）"] = "";
            rowline["平均厚度\r（μm）"] = "";
            rowline["厚度公差\r(%)"] = "";
            rowline["检查人"] = "";
            rowline["判定"] = "";
            //添加行
            dtRecord.Rows.Add(rowline);
        }

        //删除单条记录
        private void RecordView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RecordView.Columns[e.ColumnIndex].Name == "删除该条记录")
            {
                if (e.RowIndex != Recordnum)
                {
                    if (Recordnum > 1)
                    {
                        this.RecordView.Rows.RemoveAt(e.RowIndex);//删除行
                        Recordnum = Recordnum - 1;
                    }
                    else if (Recordnum > 0)
                    {
                        this.RecordView.Rows.RemoveAt(e.RowIndex);//删除行
                        this.RecordView.Rows.RemoveAt(0);//删除行
                        Recordnum = Recordnum - 1;
                    }
                    RefreshNum();
                }                
            }
        }        

        //更新序号
        private void RefreshNum()
        {
            for (int i = 0; i < RecordView.Rows.Count-1; i++)
            {
                RecordView.Rows[i].Cells["序号"].Value = (i+1).ToString();
            }
        }

        //日班改变时，夜班也随之改变
        private void DatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DatecheckBox.Checked)
            {
                NeightcheckBox.Checked = false;
                DatecheckBox.Checked = true;
            }
        }

        //夜班改变时，夜班也随之改变
        private void NeightcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NeightcheckBox.Checked)
            {
                DatecheckBox.Checked = false;
                NeightcheckBox.Checked = true;
            }
        }

        //添加行按钮
        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRecordRowLine();
        }

        //保存数据
        public void DataSave()
        {
            productname = productnameBox.Text;  //产品名称
            productnumber = productnumberBox.Text;  //产品批号
            temperature = temperatureBox.Text;  //环境温度
            humidity = "";  //相对湿度
            recordlist = new record[RecordView.Rows.Count];
            for (int i = 0; i < RecordView.Rows.Count; i++)
            {
                recorddata.time = RecordView.Rows[i].Cells[1].Value.ToString();
                recorddata.number = RecordView.Rows[i].Cells[2].Value.ToString();
                recorddata.length = RecordView.Rows[i].Cells[3].Value.ToString();
                recorddata.weight = RecordView.Rows[i].Cells[4].Value.ToString();
                recorddata.recorder = RecordView.Rows[i].Cells[5].Value.ToString();
                recorddata.outward = RecordView.Rows[i].Cells[6].Value.ToString();
                recorddata.width = RecordView.Rows[i].Cells[7].Value.ToString();
                recorddata.maxthickness = RecordView.Rows[i].Cells[8].Value.ToString();
                recorddata.minthickness = RecordView.Rows[i].Cells[9].Value.ToString();
                recorddata.avethickness = RecordView.Rows[i].Cells[10].Value.ToString();
                recorddata.marginthickness = RecordView.Rows[i].Cells[11].Value.ToString();
                recorddata.checker = RecordView.Rows[i].Cells[12].Value.ToString();
                recorddata.judge = RecordView.Rows[i].Cells[13].Value.ToString();
                recordlist[i] = recorddata;
            }
            //最后一行是合计 
        }        
    }
}
