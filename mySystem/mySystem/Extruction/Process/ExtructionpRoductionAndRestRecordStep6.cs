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

        public ExtructionpRoductionAndRestRecordStep6(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            InformationViewInitialize();
            RecordViewInitialize();
        }

        private void InformationViewInitialize()
        {
            //添加六列
            dtInformation.Columns.Add("1", typeof(String));
            dtInformation.Columns.Add("2", typeof(String));
            dtInformation.Columns.Add("3", typeof(String));
            dtInformation.Columns.Add("4", typeof(String));
            dtInformation.Columns.Add("5", typeof(String));
            //添加具体元素
            dtInformation.Rows.Add("产品名称：", " ", "产品批号：", "", "依据工艺：吹膜工艺规程");
            dtInformation.Rows.Add("环境温度(°C)：", " ", "相对湿度(%)：", "", "生产设备：AA-EQM-032");
            this.InformationView.DataSource = dtInformation;
            //设置
            this.InformationView.RowHeadersVisible = false;
            this.InformationView.ColumnHeadersVisible = false;
            this.InformationView.AllowUserToResizeColumns = false;
            this.InformationView.AllowUserToResizeRows = false;
            this.InformationView.AllowUserToAddRows = false;
            for (int i = 0; i < this.InformationView.Columns.Count; i++)
            {
                this.InformationView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.InformationView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.InformationView.Columns[i].MinimumWidth = 40;
            }
            this.InformationView.Columns[0].ReadOnly = true;
            this.InformationView.Columns[1].ReadOnly = false;
            this.InformationView.Columns[2].ReadOnly = true;
            this.InformationView.Columns[3].ReadOnly = false;
            this.InformationView.Columns[4].ReadOnly = true;
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
            dtRecord.Columns.Add("宽度(mm)", typeof(String));
            dtRecord.Columns.Add("最大厚度（μm）", typeof(String));
            dtRecord.Columns.Add("最小厚度（μm）", typeof(String));
            dtRecord.Columns.Add("平均厚度（μm）", typeof(String));
            dtRecord.Columns.Add("厚度公差(%)", typeof(String));
            dtRecord.Columns.Add("检查人", typeof(String));
            dtRecord.Columns.Add("判定", typeof(String));
            //添加内容
            AddRecordRowLine(); 
            this.RecordView.DataSource = dtRecord;
            this.RecordView.AllowUserToAddRows = false;
            //设置对齐
            this.RecordView.RowHeadersVisible = false;
            this.RecordView.AllowUserToResizeRows = false;
            for (int i = 0; i < this.RecordView.Columns.Count; i++)
            {
                this.RecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RecordView.Columns[i].MinimumWidth = 40;
            }
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
            rowline["宽度(mm)"] = "";
            rowline["最大厚度（μm）"] = "";
            rowline["最小厚度（μm）"] = "";
            rowline["平均厚度（μm）"] = "";
            rowline["厚度公差(%)"] = "";
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

        private void DeleteRecordRowLine()
        {
            if (Recordnum > 0)
            {
                this.RecordView.Rows.RemoveAt(Recordnum-1);//删除行
                Recordnum = Recordnum - 1;
                if (Recordnum == 0)
                {
                    this.RecordView.Rows.RemoveAt(Recordnum);//删除行();
                }
            }           
        }

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
            rowline["宽度(mm)"] = "";
            rowline["最大厚度（μm）"] = "";
            rowline["最小厚度（μm）"] = "";
            rowline["平均厚度（μm）"] = "";
            rowline["厚度公差(%)"] = "";
            rowline["检查人"] = "";
            rowline["判定"] = "";
            //添加行
            dtRecord.Rows.Add(rowline);
        }

        private void DatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DatecheckBox.Checked)
            {
                NeightcheckBox.Checked = false;
                DatecheckBox.Checked = true;
            }
        }

        private void NeightcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NeightcheckBox.Checked)
            {
                DatecheckBox.Checked = false;
                NeightcheckBox.Checked = true;
            }
        }

        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            AddRecordRowLine();
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
