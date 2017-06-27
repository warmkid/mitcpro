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

namespace mySystem.Extruction.Process
{
    public partial class ExtructionReplaceCore : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;
        public ExtructionReplaceCore(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = base.mainform.conn;
            connOle = base.mainform.connOle;
            isSqlOk = base.mainform.isSqlOk;
            operator_id = base.mainform.userID;

            DgvInitialize();

        }

        private void DgvInitialize()
        {

            ///***********************表格数据初始化************************///
            //表格界面设置
            this.ReplaceCoreView.Font = new Font("宋体", 12, FontStyle.Regular);
            this.ReplaceCoreView.RowHeadersVisible = false;
            this.ReplaceCoreView.AllowUserToResizeColumns = false;
            this.ReplaceCoreView.AllowUserToResizeRows = false;
            this.ReplaceCoreView.ColumnHeadersHeight = 50;
            for (int i = 0; i < this.ReplaceCoreView.Columns.Count; i++)
            {
                this.ReplaceCoreView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.ReplaceCoreView.Columns[i].MinimumWidth = 120;
                this.ReplaceCoreView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.ReplaceCoreView.Columns[i].ReadOnly = true;
            }
            this.ReplaceCoreView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.ReplaceCoreView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ReplaceCoreView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ReplaceCoreView.Columns["检查标准"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            object[] row1 = new object[] { "模芯", " 检查模芯位置安装正确，连接牢固。", true };
            object[] row2 = new object[] { "模环", " 检查模环位置安装正确，连接牢固。", true };
            object[] row3 = new object[] { "加热片", " 检查加热片位置安装正确，连接牢固。", true };
            object[] row4 = new object[] { "电热偶", " 检查电热偶位置安装正确，连接牢固。", true };
            object[] row5 = new object[] { "电源插头", " 检查模头电源插头位置安装正确，连接牢固。", true };
            object[] row6 = new object[] { "风环", " 吊中心线，检查风环应居中。前后误差±1mm。左右误差±1mm。", true };
            object[] row7 = new object[] { "风环", " 校水平，检查风环应水平。上下误差±1mm。", true };
            object[] row8 = new object[] { "风环", " 检查内风环安装位置正确。（拧紧后回一圈）", true };
            object[] row9 = new object[] { "风管", " 检查风环6根外冷风管安装正确，连接牢固。", true };
            object[] row10 = new object[] { "风管", " 检查内冷进风及内排风6根风管安装正确，连接牢固。", true };
            object[] row11 = new object[] { "气管", " 气管连接牢固，无漏气。", true };
            object[] row12 = new object[] { "盖板", " 不锈钢盖板连接牢固。（盖板下安装耐高温密封垫）", true };

            object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, };
            foreach (object[] rowArray in rows)
            {
                this.ReplaceCoreView.Rows.Add(rowArray);
            }
        }


    }
}
