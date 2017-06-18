using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionCheckBeforePowerStep2 : Form
    {
        private ExtructionProcess extructionformfather = null;
        private DataTable dt = new DataTable();

        private string sql = "Select * From extrusion";
        private SqlConnection conn = null;

        public string confirmer = ""; //确认人
        public string confirmdate = ""; //确认日期
        public string checker = "";  //复核人
        public string checkdate = "";  //复核日期
        
        public ExtructionCheckBeforePowerStep2(ExtructionProcess winMain, SqlConnection Mainconn)
        {
            InitializeComponent();
            extructionformfather = winMain;

            conn = Mainconn;

            PSLabel.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";

            DataTabelInitialize();           
                      
        }

        private void DataTabelInitialize()
        {
            ///***********************表头数据初始化************************///
            confirmer = "确认姓名";
            confirmdate = DateTime.Now.ToLongDateString().ToString();
            checker = "复核姓名";
            checkdate = DateTime.Now.ToLongDateString().ToString();
            /*
            this.Confirm.Text = confirmer;
            this.ConfirmDate.Text = confirmdate;
            this.Check.Text = checker;
            this.CheckDate.Text = checkdate;
            */
            ///***********************表格数据初始化************************///            
            //添加四列
            dt.Columns.Add("序号", typeof(String));
            dt.Columns.Add("确认项目", typeof(String));
            dt.Columns.Add("确认内容", typeof(String));
            dt.Columns.Add(new DataColumn("确认结果", typeof(bool)));
            //添加内容，默认确认结果为“是”
            dt.Rows.Add("1", "环境卫生", "确认车间环境、设备卫生符合生产工艺要求。", true);
            dt.Rows.Add("2", "冷却风", "确认冷却风检测合格，符合生产工艺要求。", true);
            dt.Rows.Add("3", "压缩空气", "确认压缩空气检测合格，气压符合生产工艺要求。", true);
            dt.Rows.Add("4", "过滤网", "大小合适的160目不锈钢过滤网，数量满足生产要求。", true);
            dt.Rows.Add("5", "减速器", "分别检查A、B、C三层减速器油位，是否在油镜中位以上。", true);
            dt.Rows.Add("6", "运动部件", "检查旋转牵引、一牵引、人字夹板、稳泡器、纠偏、二牵引、收卷、冷却风机运转情况。", true);
            dt.Rows.Add("7", "卷芯管", "确认卷芯管清洁合格，规格满足生产要求。", true);
            dt.Rows.Add("8", "原料", "确认原料检测合格，数量满足生产要求。", true);
            dt.Rows.Add("9", "工具准备", "是否按《吹膜机组开机工具表》准备工具。", true);
            dt.Rows.Add("10", "电子称", "已清洁干净且运行正常，电量充足。", true);
            dt.Rows.Add("11", "电动叉车", "已清洁干净且运行正常，电量充足。", true);
            dt.Rows.Add("12", "测厚仪、电脑", "能正常打开，相关软件能正常运行。", true);
            dt.Rows.Add("13", "吸尘器", "已清洁干净且运行正常。", true);
            dt.Rows.Add("14", "供料系统", "设备卫生符合要求，供料系统各部件运行正常。", true);
            this.CheckBeforePowerView.DataSource = dt;
            this.CheckBeforePowerView.Font = new Font("宋体", 12, FontStyle.Regular);

            //设置
            this.CheckBeforePowerView.RowHeadersVisible = false;
            this.CheckBeforePowerView.AllowUserToResizeColumns = false;
            this.CheckBeforePowerView.AllowUserToResizeRows = false;
            //this.CheckBeforePowerTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.CheckBeforePowerView.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count; i++)
            {
                this.CheckBeforePowerView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.CheckBeforePowerView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            this.CheckBeforePowerView.Columns[0].MinimumWidth = 80;
            this.CheckBeforePowerView.Columns[1].MinimumWidth = 160;
            this.CheckBeforePowerView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.CheckBeforePowerView.Columns[3].MinimumWidth = 80;
            for (int i = 0; i < this.CheckBeforePowerView.Columns.Count - 1; i++)
            {
                this.CheckBeforePowerView.Columns[i].ReadOnly = true;
            }
            this.CheckBeforePowerView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CheckBeforePowerView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //若已有数据，向内部添加现有数据
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter daSQL = new SqlDataAdapter(comm);
            DataTable dtSQL = new DataTable();
            daSQL.Fill(dtSQL);
            
            int stepnow = Convert.ToInt32(dtSQL.Rows[0]["step_status"]);
            if (stepnow >= 2)
            {
                for (int i=1;i<=14;i++)
                {
                    string qualified_string = "s2_item"+i.ToString()+"_qualified";
                    bool qualified_bool = bool.Parse(dtSQL.Rows[0][qualified_string].ToString());
                    this.CheckBeforePowerView.Rows[i-1].Cells["确认结果"].Value = qualified_bool;
                }
            }
            comm.Dispose();
            daSQL.Dispose();
            dtSQL.Dispose();
        }

        public void DataSave()
        {
            string qualified_string = null;
            string val = null;
            string sqlstr = null;
            SqlCommand com = null;

            for (int i = 1; i <= 14; i++)
            {
                qualified_string = "s2_item" + i.ToString() + "_qualified";
                val = this.CheckBeforePowerView.Rows[i-1].Cells["确认结果"].Value.ToString() == "True"? "1":"0" ; 
                sqlstr = "update extrusion set " + qualified_string + " = " + val + "  where id =1";
                com = new SqlCommand(sqlstr, conn);
                com.ExecuteNonQuery();
                com.Dispose();
            }

            sqlstr = "update extrusion set step_status = 2 where id =1";
            com = new SqlCommand(sqlstr, conn);
            com.ExecuteNonQuery();
            com.Dispose();
        }

        private void CheckBeforePowerView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
