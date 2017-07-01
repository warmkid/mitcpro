using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem.Process.Bag
{
    public partial class CSBag_CheckBeforePower : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;
        private CheckForm check = null;
        private string review_opinion;
        private bool ischeckOk = false;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        public CSBag_CheckBeforePower(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;
            
            DgvInitialize();
        }

        private void DgvInitialize()
        {
         ///***********************表格数据初始化************************///
            //表格界面设置
            this.CheckView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.CheckView.RowHeadersVisible = false;
            this.CheckView.AllowUserToResizeColumns = false;
            this.CheckView.AllowUserToResizeRows = false;
            this.CheckView.ColumnHeadersHeight = 80;
            for (int i = 0; i < this.CheckView.Columns.Count; i++)
            {
                this.CheckView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.CheckView.Columns[i].MinimumWidth = 80;
                this.CheckView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.CheckView.Columns[i].ReadOnly = true;
            }
            this.CheckView.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.CheckView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckView.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            object[] row1 = new object[] { "1", " 环境卫生确认 ", " 1.1  车间环境、设备卫生是否符合生产工艺要求。", true };
            object[] row2 = new object[] { "1", " 环境卫生确认 ", " 1.2  清洁袋是否放置到位。", true };
            object[] row3 = new object[] { "2", " 设备确认 ", " 2.1  压缩空气供应≥0.6MPa。", true };
            object[] row4 = new object[] { "2", " 设备确认 ", " 2.2  冷却水与动力人员确认开启。	", true };
            object[] row5 = new object[] { "2", " 设备确认 ", " 2.3  开启纠偏系统，检查运行是否正常。", true };
            object[] row6 = new object[] { "2", " 设备确认 ", " 2.4  各气辊都处于关闭状态压住膜卷。", true };
            object[] row7 = new object[] { "2", " 设备确认 ", " 2.5  根据计划书规格调整切刀距离。", true };
            object[] row8 = new object[] { "2", " 设备确认 ", " 2.6  根据指令单规格调整电眼距离，检查电眼是否居中。", true };
            object[] row9 = new object[] { "2", " 设备确认 ", " 2.7  电眼镜头是否已清洁。", true };
            object[] row10 = new object[] { "3", " 原材料确认 ", " 3.1  原材料规格是否与计划书相符。", true };
            object[] row11 = new object[] { "3", " 原材料确认 ", " 3.2  内包装袋领用是否与计划书相符。", true };
            object[] row12 = new object[] { "3", " 原材料确认 ", " 3.3  内外包装标签是否与计划书相符。", true };
            object[] row13 = new object[] { "3", " 原材料确认 ", " 3.4  灭菌指示剂是否领取。", true };
            object[] row14 = new object[] { "4", " 工具确认 ", " 4.1  刀具、胶带、记号笔、清洁抹布、酒精壶是否就位。", true };

            object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14 };
            foreach (object[] rowArray in rows)
            {
                this.CheckView.Rows.Add(rowArray);
            }
            recorderBox.Text = operator_name;
        }

        public override void CheckResult()
        {
            base.CheckResult();
            review_id = check.userID;
            review_opinion = check.opinion;
            ischeckOk = check.ischeckOk;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
            reviewer_name = Parameter.IDtoName(review_id);
            if (isSqlOk)
            { }
            else
            { }
            checkerBox.Text = reviewer_name;
        }

    }
}
