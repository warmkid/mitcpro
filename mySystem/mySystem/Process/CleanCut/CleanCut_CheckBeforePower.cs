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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_CheckBeforePower : BaseForm
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

        public CleanCut_CheckBeforePower(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;


            CheckDgvInitialize();
            RunRecordDgvInitialize();

            recorderBox.Text = operator_name;

        }

        private void CheckDgvInitialize()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.CheckBeforePowerView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.CheckBeforePowerView.RowHeadersVisible = false;
            this.CheckBeforePowerView.AllowUserToResizeColumns = false;
            this.CheckBeforePowerView.AllowUserToResizeRows = false;
            this.CheckBeforePowerView.ColumnHeadersHeight = 50;
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count; i++)
            {
                this.CheckBeforePowerView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.CheckBeforePowerView.Columns[i].MinimumWidth = 40;
                this.CheckBeforePowerView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.CheckBeforePowerView.Columns[i].ReadOnly = true;
            }
            this.CheckBeforePowerView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.Columns["确认项目"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.CheckBeforePowerView.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.CheckBeforePowerView.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.CheckBeforePowerView.Columns["确认结果"].Width = 40;
            this.CheckBeforePowerView.Columns["确认结果"].HeaderText = "确认\r结果";

            object[] row1 = new object[] { "1", " 生产指令认", " 确认生产指令单规格、数量", true };
            object[] row2 = new object[] { "2", " 环境卫生确认", " 确认车间环境、设备卫生符合生产工艺要求", true };
            object[] row3 = new object[] { "3", " 卷芯管确认 ", " 卷芯管数量及规格满足生产需要", true };
            object[] row4 = new object[] { "4", " 工具确认", "  确认所需工具、清洁毛巾、垃圾袋准备", true };
            object[] row5 = new object[] { "5", " 电气确认", " 确认电、气正常", true };
            object[] row6 = new object[] { "6", " 放料架确认", " 确认放料架升降、开合正常", true };
            object[] row7 = new object[] { "7", " 电眼确认", " 确认纠偏电眼在中心位置", true };
            object[] row8 = new object[] { "8", " 粘尘辊确认", " 确认粘尘辊架气缸开合动作正常", true };
            object[] row9 = new object[] { "9", " 收卷机确认", " 确认收卷机正常运转", true };

            object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9,};
            foreach (object[] rowArray in rows)
            {
                this.CheckBeforePowerView.Rows.Add(rowArray);
            }
        }

        private void RunRecordDgvInitialize()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.RunRecordView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.RunRecordView.RowHeadersVisible = false;
            this.RunRecordView.AllowUserToResizeColumns = false;
            this.RunRecordView.AllowUserToResizeRows = false;
            this.RunRecordView.ColumnHeadersHeight = 50;
            for (int i = 0; i < this.RunRecordView.Columns.Count; i++)
            {
                this.RunRecordView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.RunRecordView.Columns[i].MinimumWidth = 90;
                this.RunRecordView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.RunRecordView.Columns[i].ReadOnly = true;
            }
            this.RunRecordView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RunRecordView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.RunRecordView.Columns["生产时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.RunRecordView.Columns["分切速度"].HeaderText = "分切速度\r(m/min)";
            this.RunRecordView.Columns["自动张力设定"].HeaderText = "自动张力\r设定(kg)";
            this.RunRecordView.Columns["自动张力显示"].HeaderText = "自动张力\r显示(kg)";
            this.RunRecordView.Columns["张力输出显示"].HeaderText = "张力输出\r显示(%)";

            AddRecordRowLine();
        }

        //添加单行模板
        private void AddRecordRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in RunRecordView.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString(); ;
            dr.Cells[1].Value = "";
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = operator_name;

            RunRecordView.Rows.Add(dr);
        }
                
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            AddRecordRowLine();
        }

        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            int deletenum = RunRecordView.CurrentRow.Index;
            this.RunRecordView.Rows.RemoveAt(deletenum);            
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
            checkerBox.Text = reviewer_name;
        }        
    }
}
