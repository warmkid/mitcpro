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
using System.Runtime.InteropServices;

namespace mySystem
{
    public partial class ReplaceHeadForm : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm check = null;

        private String table = "吹膜机组换模头检查表";
        private String tableInfo = "吹膜机组换模头检查项目";

        private DataTable dt设置, dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;
        private Int32 KeyID;

        public ReplaceHeadForm(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            GetSettingInfo();
            DataNew();
        }

        public ReplaceHeadForm(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            GetSettingInfo();
            IDShow(ID);
            Init();
            printBtn.Enabled = true;
        }

        //******************************初始化******************************//

        //控件不可用
        private void Init()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
        }

        //可编辑，控件初始化
        private void EnableInit(bool able)
        {
            tb更换原因.Enabled = able;
            tb更换后模头型号.Enabled = able;
            tb更换前模头型号.Enabled = able;
            dtp更换日期.Enabled = able;
            tb检查人.Enabled = able;
            dtp检查日期.Enabled = able;
            dataGridView1.ReadOnly = !able;
            SaveBtn.Enabled = able;
        }

        //已经设置好的检查内容
        private void GetSettingInfo()
        {
            //新建表
            dt设置 = new DataTable("设置");
            dt设置.Columns.Add("检查项目", typeof(string));
            dt设置.Columns.Add("检查标准", typeof(string));
            dt设置.Columns.Add("检查结果", typeof(string));
            //给表里添加内容
            dt设置.Rows.Add("模头", "吊中心线，检查模头应居中。前后误差±1mm。左右误差±2mm。", "Yes");
            dt设置.Rows.Add("模头", "校水平，检查模头应水平。上下误差±1mm。", "Yes");
            dt设置.Rows.Add("挤出机", "校水平，流道连接处水平度误差±1mm。（分别检查A、B、C三层）", "Yes");
            dt设置.Rows.Add("挤出机", "连接法兰连接紧固，水平度误差±1mm。", "Yes");
            dt设置.Rows.Add("加热片", "分别检查A、B、C层加热片位置安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("电热偶", "分别检查A、B、C层及模头电热偶位置安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("电源插头", "分别检查A、B、C层及模头电源插头位置安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("风环", "吊中心线，检查风环应居中。前后误差±1mm。左右误差±1mm。", "Yes");
            dt设置.Rows.Add("风环", "校水平，检查风环应水平。上下误差±1mm。", "Yes");
            dt设置.Rows.Add("风环", "检查内风环安装位置正确。（拧紧后回一圈）", "Yes");
            dt设置.Rows.Add("风管", "检查风环6根外冷风管安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("风管", "检查内冷进风（短的3根）及内排风6根不锈钢管安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("风管", "检查内冷进风及内排风6根风管安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("风管", "检查外冷、内冷、内排3根主风管安装正确，连接牢固。", "Yes");
            dt设置.Rows.Add("气管", "气管连接牢固，无漏气。", "Yes");
            dt设置.Rows.Add("盖板", "不锈钢盖板连接牢固。（盖板下安装耐高温密封垫）", "Yes");

        }

        //******************************显示数据******************************//

        //新建信息表
        private void DataNew()
        {
            Init();

            //********* 外表新建、保存、重新绑定 *********//

            //外表新建 
            readOuterData(0, true);
            outerBind();
            DataRow dr1 = dt记录.NewRow();
            dr1 = writeOuterDefault(dr1);
            dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);
            //外表保存 
            bs记录.EndEdit();
            da记录.Update((DataTable)bs记录.DataSource);
            //获取外表主键
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";
            Int32 idd1 = (Int32)comm.ExecuteScalar();
            KeyID = idd1;
            //外表重新绑定
            readOuterData(KeyID, false);
            outerBind();

            //********* 内表新建、保存、重新绑定 *********//

            //内表绑定
            readInnerData(KeyID);
            innerBind();
            dt记录详情.Rows.Clear();
            dt记录详情 = writeInnerDefault(KeyID, dt记录详情);
            //立马保存内表
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            //内表重新绑定
            dataGridView1.Columns.Clear();
            readInnerData(KeyID);
            setDataGridViewColumns();
            innerBind();

            //********* 控件可用性 *********// 

            EnableInit(true);
            setDataGridViewFormat();
            SaveBtn.Enabled = true;
        }

        //根据主键显示
        private void IDShow(Int32 ID)
        {
            readOuterData(ID, false);
            outerBind();
            readInnerData(ID);
            setDataGridViewColumns();
            innerBind();
        }
        
        //****************************** 嵌套 ******************************//

        //isNew==true：新建一行  isNew==false：依据主键选择行
        private void readOuterData(Int32 ID, Boolean isNew)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            if (isNew)
            { da记录 = new OleDbDataAdapter("select * from " + table + " where  0 = 1 ", connOle); }
            else
            { da记录 = new OleDbDataAdapter("select * from " + table + " where  ID = " + ID.ToString(), connOle); }
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定
            tb更换原因.DataBindings.Clear();
            tb更换原因.DataBindings.Add("Text", bs记录.DataSource, "更换原因");
            dtp更换日期.DataBindings.Clear();
            dtp更换日期.DataBindings.Add("Text", bs记录.DataSource, "更换日期");
            tb更换前模头型号.DataBindings.Clear();
            tb更换前模头型号.DataBindings.Add("Text", bs记录.DataSource, "更换前模头型号");
            tb更换后模头型号.DataBindings.Clear();
            tb更换后模头型号.DataBindings.Add("Text", bs记录.DataSource, "更换后模头型号");

            tb检查人.DataBindings.Clear();
            tb检查人.DataBindings.Add("Text", bs记录.DataSource, "检查人");
            dtp检查日期.DataBindings.Clear();
            dtp检查日期.DataBindings.Add("Text", bs记录.DataSource, "检查日期");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息        
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["更换日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["检查人"] = mySystem.Parameter.userName;
            dr["检查日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;
            return dr;
        }
        
        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            //读取记录表里的记录
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where 换模头ID = " + ID.ToString(), connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
        }

        //添加行代码，从dt设置表里读取
        private DataTable writeInnerDefault(Int32 ID, DataTable dt)
        {
            for (int i = 0; i < dt设置.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["换模头ID"] = ID;
                dr["检查项目"] = dt设置.Rows[i]["检查项目"];
                dr["检查标准"] = dt设置.Rows[i]["检查标准"];
                dr["检查结果"] = dt设置.Rows[i]["检查结果"];
                dt.Rows.InsertAt(dr, dt.Rows.Count);
            }
            return dt;
        }

        //设置DataGridView中各列的格式
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "检查结果":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("Yes");
                        cbc.Items.Add("No");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 120;
                        break;
                    default:
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = 120;
                        break;
                }
            }
            setDataGridViewFormat();
        }

        //设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["换模头ID"].Visible = false;
            dataGridView1.Columns["检查项目"].ReadOnly = true;
            dataGridView1.Columns["检查标准"].ReadOnly = true;
            dataGridView1.Columns["检查标准"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["检查标准"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        //******************************按钮功能******************************//

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(tb检查人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『检查人』信息", "ERROR");
            }
            else
            {
                if (isSqlOk)
                { }
                else
                {
                    // 内表保存
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                    readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                    innerBind();

                    //外表保存
                    bs记录.EndEdit();
                    da记录.Update((DataTable)bs记录.DataSource);
                    readOuterData(KeyID, false);
                    outerBind();
                }
                CheckBtn.Enabled = true;
                tb审核人.Enabled = true;
                dtp审核日期.Enabled = true;
            }
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            if (check.ischeckOk == true)
            {
                dt记录.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
                dt记录.Rows[0]["审核意见"] = check.opinion;
                dt记录.Rows[0]["审核是否通过"] = check.ischeckOk;

                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);

                Init();
                printBtn.Enabled = true;
            }
        }

        //打印按钮
        private void printBtn_Click(object sender, EventArgs e)
        {
            //true->预览
            //false->打印
            print(true);
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\D\SOP-MFG-403-R01A 吹膜机组换模头检查表.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 修改Sheet中某行某列的值
                my = printValue(my);
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                //false->打印
                // 设置该进程是否可见
                oXL.Visible = false;
                // 修改Sheet中某行某列的值
                my = printValue(my);
                // 直接用默认打印机打印该Sheet
                my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
            }
        }

        //打印功能，填写Value
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet)
        {
            //外表信息
            mysheet.Cells[3, 2].Value = dt记录.Rows[0]["更换原因"].ToString();
            mysheet.Cells[3, 5].Value = dt记录.Rows[0]["更换前模头型号"].ToString();
            mysheet.Cells[4, 2].Value = Convert.ToDateTime(dt记录.Rows[0]["更换日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["更换日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["更换日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[4, 5].Value = dt记录.Rows[0]["更换后模头型号"].ToString();
            String stringtemp = "";
            stringtemp = dt记录.Rows[0]["检查人"].ToString();
            stringtemp = stringtemp + "  /  " + Convert.ToDateTime(dt记录.Rows[0]["检查日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["检查日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["检查日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[23, 2].Value = stringtemp;
            stringtemp = dt记录.Rows[0]["审核人"].ToString();
            stringtemp = stringtemp + "  /  " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[23, 5].Value = stringtemp;
            //内表信息
            int rownum = dt记录详情.Rows.Count > 16 ? 16 : dt记录详情.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[6 + i, 6].Value = dt记录详情.Rows[i]["检查结果"].ToString() == "Yes" ? "√" : "×";
            }
            return mysheet;
        }

    }
}
