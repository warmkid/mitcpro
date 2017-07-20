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
using System.Data.OleDb;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionCheckBeforePowerStep2 : BaseForm
    {
        private DataTable dt_confirmarea = new DataTable();

        private String table = "吹膜机组开机前确认表";
        private String tableInfo = "吹膜机组开机前确认项目记录";
        private String tableSet = "设置吹膜机组开机前确认项目";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm check = null;
        
        private DataTable dt设置, dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private Int32 KeyID;
                
        public ExtructionCheckBeforePowerStep2(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);

            GetSettingInfo();
            if (dt设置.Rows.Count > 0)
            {
                DataShow(mySystem.Parameter.proInstruID);
            }
            else
            { MessageBox.Show("开机确认项目设置尚未完成，请先去设置！"); }
        }

        public ExtructionCheckBeforePowerStep2(MainForm mainform, Int32 ID): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            IDShow(ID);
            Init();
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
            tb确认人.Enabled = able;
            dtp确认日期.Enabled = able;
            dataGridView1.ReadOnly = !able;
            SaveBtn.Enabled = able;
        }

        //读取设置内容
        private void GetSettingInfo()
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();
        }
        
        //******************************显示数据******************************//
         
        //显示根据信息查找
        private void DataShow(Int32 InstruID)
        {

            Init();

            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID);
            outerBind();
            //MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************// 
            Boolean isnew = true;
            if (dt记录.Rows.Count <= 0)
            {
                isnew = true;
                //********* 外表新建、保存、重新绑定 *********//                
                //初始化外表这一行
                DataRow dr1 = dt记录.NewRow();
                dr1 = writeOuterDefault(dr1);
                dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);
                //立马保存这一行
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                //外表重新绑定
                readOuterData(InstruID);
                outerBind();

                //********* 内表新建、保存、重新绑定 *********//

                //获取外表主键
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                comm.CommandText = "select @@identity";
                Int32 idd1 = (Int32)comm.ExecuteScalar();
                KeyID = idd1;
                //内表绑定
                readInnerData(KeyID);
                innerBind();
                dt记录详情.Rows.Clear();
                dt记录详情 = writeInnerDefault(KeyID, dt记录详情);
                setDataGridViewRowNums();

                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                //内表重新绑定
                dataGridView1.Columns.Clear();
                readInnerData(KeyID);
                setDataGridViewColumns();
                innerBind();
            }
            else
            {
                isnew = false;
                KeyID = Convert.ToInt32(dt记录.Rows[0]["ID"].ToString());
                //内表绑定
                dataGridView1.Columns.Clear();
                readInnerData(KeyID);
                setDataGridViewColumns();
                innerBind();
            }

            //********* 控件可用性 *********//   

            if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"].ToString()) == true)
            {
                //审核通过表
                printBtn.Enabled = true;
            }
            else
            {
                if (isnew == true)
                {
                    //新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                }
                else
                {
                    //非新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    tb审核人.Enabled = true;
                    dtp审核日期.Enabled = true;
                }
            }
        }

        //根据主键显示
        private void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                readOuterData(Convert.ToInt32(reader1["生产指令ID"].ToString()));
                outerBind();
                readInnerData(ID);
                setDataGridViewColumns();
                innerBind();  
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString(), connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;

            tb确认人.DataBindings.Clear();
            tb确认人.DataBindings.Add("Text", bs记录.DataSource, "确认人");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp确认日期.DataBindings.Clear();
            dtp确认日期.DataBindings.Add("Text", bs记录.DataSource, "确认日期");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }
        
        //添加外表默认信息        
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["确认人"] = mySystem.Parameter.userName;
            dr["确认日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
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
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T吹膜机组开机前确认表ID = " + ID.ToString(), connOle);
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

        //添加行代码，从设置表里读取
        private DataTable writeInnerDefault(Int32 ID, DataTable dt)
        {
            for (int i = 0; i < dt设置.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["T吹膜机组开机前确认表ID"] = ID;
                dr["序号"] = (i + 1);
                dr["确认项目"] = dt设置.Rows[i]["确认项目"];
                dr["确认内容"] = dt设置.Rows[i]["确认内容"];
                dr["确认结果"] = "Yes";
                dt.Rows.InsertAt(dr, dt.Rows.Count);
            }
            return dt;
        }

        //序号刷新
        private void setDataGridViewRowNums()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
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
                    case "确认结果":
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
            dataGridView1.Columns["T吹膜机组开机前确认表ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["确认内容"].ReadOnly = true;
            dataGridView1.Columns["确认项目"].ReadOnly = true;
            dataGridView1.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; 
        }

        //设置datagridview背景颜色
        private void setDataGridViewBackColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["确认结果"].Value.ToString() == "Yes")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                if (dataGridView1.Rows[i].Cells["确认结果"].Value.ToString() == "No")
                {
                    //dataGridView1.Rows[i].Cells["确认结果"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        //******************************按钮功能******************************//

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(tb确认人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『确认人』信息", "ERROR");
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
                    readOuterData(mySystem.Parameter.proInstruID);
                    outerBind();
                }
                CheckBtn.Enabled = true;
                tb审核人.Enabled = true;
                dtp审核日期.Enabled = true;
            }
            setDataGridViewBackColor();
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow gdvr in dataGridView1.Rows)
            {
                if(gdvr.DefaultCellStyle.BackColor==Color.Red)
                {
                    MessageBox.Show("有条目待确认");
                    return;
                } 
            }
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
            //Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(@"D:\excel\SOP-MFG-301-R04 吹膜机组开机前确认表_何.xlsx");
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\C\SOP-MFG-301-R04 吹膜机组开机前确认表.xlsx");            
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

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet)
        {
            //外表信息
            String stringtemp = "";
            stringtemp = "确认人：" + dt记录.Rows[0]["确认人"].ToString();
            stringtemp = stringtemp + "       确认日期：" + Convert.ToDateTime(dt记录.Rows[0]["确认日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["确认日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["确认日期"].ToString()).Day.ToString() + "日";
            stringtemp = stringtemp + "      审核人：" + dt记录.Rows[0]["审核人"].ToString();
            stringtemp = stringtemp + "       审核日期：" + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[19, 1].Value = stringtemp;            
            //内表信息
            int rownum = dt记录详情.Rows.Count > 14 ? 14 : dt记录详情.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[4 + i, 1].Value = (i + 1).ToString();
                mysheet.Cells[4 + i, 2].Value = dt记录详情.Rows[i]["确认项目"].ToString();
                mysheet.Cells[4 + i, 3].Value = dt记录详情.Rows[i]["确认内容"].ToString();
                mysheet.Cells[4 + i, 4].Value = dt记录详情.Rows[i]["确认结果"].ToString() == "Yes" ? "√" : "×";
            }
            for (int i = rownum; i < 14; i++)
            {
                mysheet.Cells[4 + i, 1].Value = "  ";
                mysheet.Cells[4 + i, 2].Value = "  ";
                mysheet.Cells[4 + i, 3].Value = "  ";
                mysheet.Cells[4 + i, 4].Value = "  ";
            }
            return mysheet;
        }
       
        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        //检查是否为合格，下拉框不是Yes/合格/是，则标红，并不准审核
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            setDataGridViewBackColor();
        }

        //数据绑定结束，设置背景颜色
        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //throw new NotImplementedException();
            setDataGridViewBackColor();
            setDataGridViewFormat();
        }

    }
}
