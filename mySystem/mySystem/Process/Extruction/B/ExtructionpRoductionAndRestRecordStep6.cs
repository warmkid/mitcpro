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
    public partial class ExtructionpRoductionAndRestRecordStep6 : BaseForm
    {
        private String table = "吹膜工序生产和检验记录";
        private String tableInfo = "吹膜工序生产和检验记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;
        private CheckForm check = null;

        private int[] sum = { 0, 0 };

        private DataTable dt记录, dt记录详情, dt代码批号, dt工艺设备;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private Int32 KeyID = 1;

        //找不到人，弹窗不太好看
        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            Instructionid = mySystem.Parameter.proInstruID;
            
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            //dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            Init();
            GetProductInfo();
        }

        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            
            IDShow(ID);
            foreach (Control c in this.Controls) { c.Enabled = false; }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
        }

        //******************************初始化******************************//

        //控件不可用
        private void Init()
        {     
            foreach(Control c in this.Controls){c.Enabled = false;}
            this.dataGridView1.Enabled = true;
            this.dataGridView1.ReadOnly = true;
            this.cb产品名称.Enabled = true;
            this.dtp生产日期.Enabled = true;
            this.cb白班.Enabled = false;
            this.cb夜班.Enabled = false;
            this.tb累计同规格膜卷长度R.ReadOnly = true;
            this.tb累计同规格膜卷重量T.ReadOnly = true;            
        }

        //可编辑，控件初始化
        private void EnableInit(bool able)
        {
            //dataGridView1.Enabled = able;
            dataGridView1.ReadOnly = !able;

            this.tb环境湿度.Enabled = able;
            this.tb环境温度.Enabled = able;
        }

        //产品名称、产品批号列表获取+产品工艺、设备、班次填写
        private void GetProductInfo()
        {
            dt工艺设备 = new DataTable("工艺设备");
            dt代码批号 = new DataTable("代码批号");

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (!isSqlOk)
            {
                //工艺、设备编号表格新建                
                dt工艺设备.Columns.Add("依据工艺", typeof(String));   //新建第一列
                dt工艺设备.Columns.Add("生产设备", typeof(String));      //新建第二列

                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + mySystem.Parameter.proInstruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    //填入生产工艺、生产设备编号
                    dt工艺设备.Rows.Add(reader1["生产工艺"].ToString(), reader1["生产设备编号"].ToString());
                    tb生产设备.Text = dt工艺设备.Rows[0]["生产设备"].ToString();
                    tb依据工艺.Text = dt工艺设备.Rows[0]["依据工艺"].ToString();

                    //查找该生产ID下的产品编码、产品批号
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select ID, 产品编码, 产品批号 from 生产指令产品列表 where 生产指令ID = " + reader1["ID"].ToString();
                    OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                    datemp.Fill(dt代码批号);
                    if (dt代码批号.Rows.Count == 0)
                    {
                        MessageBox.Show("该生产指令编码下的『生产指令产品列表』尚未生成！");
                    }
                    else
                    {
                        for (int i = 0; i < dt代码批号.Rows.Count; i++)
                        {
                            cb产品名称.Items.Add(dt代码批号.Rows[i]["产品编码"].ToString());//添加
                        }
                    }
                    datemp.Dispose();
                }
                else
                {
                    MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
                }
                reader1.Dispose();
            }
            else
            {
                //从SQL数据库中读取;                
            }

            //MessageBox.Show(dt代码批号.Rows.Count.ToString());

            //*********数据填写*********//

            cb产品名称.SelectedIndex = -1;
            tb产品批号.Text = "";
            cb白班.Checked = (mySystem.Parameter.userflight) == "白班" ? true : false;
            cb夜班.Checked = !(cb白班.Checked);            
        }
        
        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID, String productName, Boolean flight, DateTime searchTime)
        {
            Init();

            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productName, flight, searchTime);
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
                readOuterData(InstruID, productName, flight, searchTime);
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
                DataRow dr2 = dt记录详情.NewRow();
                dr2 = writeInnerDefault(KeyID, dr2);
                dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
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
                if (isnew ==true)
                {
                    //新建表
                    EnableInit(true);
                    setDataGridViewFormat();
                    dataGridView1.ReadOnly = false;
                    SaveBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = true;
                }
                else
                {
                    //非新建表
                    EnableInit(true);
                    setDataGridViewFormat();
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["检查人"].ToString()) != 0)
                        { dataGridView1.Rows[i].ReadOnly = true; }                        
                    }
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    tb审核人.Enabled = true;                    
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = true;
                }
            }
        }

        //根据主键显示
        public void IDShow(Int32 ID)
        {
            OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + table + " where ID = " + ID.ToString(), connOle);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            
            readOuterData(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品名称"].ToString(), Convert.ToBoolean(dt1.Rows[0]["班次"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            outerBind();
            readInnerData(ID);
            setDataGridViewColumns();
            innerBind();
            cb夜班.Checked = !cb白班.Checked;
        }
        
        //****************************** 嵌套 ******************************//
        
        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productName, Boolean flight, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品名称 = '" + productName + "' and 班次 = " + flight.ToString() + " and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            cb产品名称.DataBindings.Clear();
            cb产品名称.DataBindings.Add("Text", bs记录.DataSource, "产品名称");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            tb依据工艺.DataBindings.Clear();
            tb依据工艺.DataBindings.Add("Text", bs记录.DataSource, "依据工艺");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");            
            tb环境温度.DataBindings.Clear();
            tb环境温度.DataBindings.Add("Text", bs记录.DataSource, "环境温度");
            tb环境湿度.DataBindings.Clear();
            tb环境湿度.DataBindings.Add("Text", bs记录.DataSource, "环境湿度");
            tb生产设备.DataBindings.Clear();
            tb生产设备.DataBindings.Add("Text", bs记录.DataSource, "生产设备");
            //dtp生产日期.DataBindings.Clear();
            //dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            tb累计同规格膜卷长度R.DataBindings.Clear();
            tb累计同规格膜卷长度R.DataBindings.Add("Text", bs记录.DataSource, "累计同规格膜卷长度R");
            tb累计同规格膜卷重量T.DataBindings.Clear();
            tb累计同规格膜卷重量T.DataBindings.Add("Text", bs记录.DataSource, "累计同规格膜卷重量T");

            cb白班.DataBindings.Clear();
            cb白班.DataBindings.Add("Checked", bs记录.DataSource, "班次");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["产品名称"] = cb产品名称.Text;
            dr["产品批号"] = dt代码批号.Rows[cb产品名称.FindString(cb产品名称.Text)]["产品批号"].ToString();
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["班次"] = cb白班.Checked;
            dr["依据工艺"] = dt工艺设备.Rows[0]["依据工艺"];
            dr["生产设备"] = dt工艺设备.Rows[0]["生产设备"];
            dr["审核人"] = "";
            dr["审核是否通过"] = false;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {            
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T吹膜工序生产和检验记录ID = " + ID.ToString(), connOle);
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

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            //DataRow dr = dt记录详情.NewRow();
            dr["序号"] = 0;
            dr["T吹膜工序生产和检验记录ID"] = ID;
            if (dt记录详情.Rows.Count >= 1)
            { dr["开始时间"] = dt记录详情.Rows[dt记录详情.Rows.Count - 1]["结束时间"]; }
            else
            { dr["开始时间"] = DateTime.Now; }
            dr["结束时间"] = DateTime.Now;
            dr["记录人"] = mySystem.Parameter.userName;
            dr["检查人"] = mySystem.Parameter.userName;
            dr["外观"] = "Yes";
            dr["判定"] = "Yes";
            //dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            return dr;
        }
        
        //序号刷新
        private void setDataGridViewRowNums()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewColumns()
        {            
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "外观":
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
                        cbc.MinimumWidth = 80;
                        break;
                    case "判定":
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
                        cbc.MinimumWidth = 80;
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
                        tbc.MinimumWidth = 80;
                        break;
                }
            }
            setDataGridViewFormat();
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T吹膜工序生产和检验记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
        }

        //******************************按钮功能******************************//

        //添加行按钮
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["记录人"].Value.ToString()) == 0)
            {
                dt记录详情.Rows[dataGridView1.Rows.Count - 1]["记录人"] = "";
                MessageBox.Show("请重新输入最新建行的『记录人』信息", "ERROR");
            }
            if (mySystem.Parameter.NametoID(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["检查人"].Value.ToString()) == 0)
            {
                dt记录详情.Rows[dataGridView1.Rows.Count - 1]["检查人"] = "";
                MessageBox.Show("请重新输入最新建行的『检查人』信息", "ERROR");
            }
            else
            {
                DialogResult dr = MessageBox.Show("新建行后，上面的信息将不可修改，确认新建吗？", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    //用户选择确认的操作
                    //MessageBox.Show("您选择的是【确认】");
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;
                    DataRow dr2 = dt记录详情.NewRow();
                    dr2 = writeInnerDefault(KeyID, dr2);
                    dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
                    setDataGridViewRowNums();
                }
                else if (dr == DialogResult.Cancel)
                {
                    //用户选择取消的操作
                    //MessageBox.Show("您选择的是【取消】");
                }
            }                    
        }
        
        //删除行按钮  只允许删除没有检查人的行
        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //dt记录详情.Rows.RemoveAt(deletenum);
                dt记录详情.Rows[deletenum].Delete();
                // 保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind(); 
                //求合计
                getTotal();
                setDataGridViewRowNums();
            }
        }

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Name_check1() == false)
            { /*记录人不合格*/}
            else if (Name_check2() == false)
            { /*检查人不合格*/}
            else if (TextBox_check() == false)
            {/*环境温度、环境湿度不合格*/ }
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
                    readOuterData(mySystem.Parameter.proInstruID, cb产品名称.Text, cb白班.Checked, dtp生产日期.Value);
                    outerBind();
                }
                CheckBtn.Enabled = true;
                tb审核人.Enabled = true;
            }            
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

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        //打印按钮
        private void printBtn_Click(object sender, EventArgs e)
        {
            //print
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R09 吹膜工序生产和检验记录.xlsx");
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
            mysheet.Cells[3, 1].Value = "产品名称：" + dt记录.Rows[0]["产品名称"].ToString();
            mysheet.Cells[3, 5].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 9].Value = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            String flighttemp = Convert.ToBoolean(dt记录.Rows[0]["班次"].ToString()) == true ? "白班" : "夜班";
            mysheet.Cells[3, 13].Value = "班次：" + flighttemp;
            mysheet.Cells[4, 1].Value = "依据工艺：" + dt记录.Rows[0]["依据工艺"].ToString();
            mysheet.Cells[4, 5].Value = "生产设备：" + dt记录.Rows[0]["生产设备"].ToString();
            mysheet.Cells[4, 9].Value = "环境温度：" + dt记录.Rows[0]["环境温度"].ToString() + "℃";
            mysheet.Cells[4, 11].Value = "相对湿度：" + dt记录.Rows[0]["环境湿度"].ToString() + "%";
            mysheet.Cells[4, 13].Value = "审核人：" + dt记录.Rows[0]["审核人"].ToString();
            mysheet.Cells[18, 4].Value = dt记录.Rows[0]["累计同规格膜卷长度R"].ToString();
            mysheet.Cells[18, 5].Value = dt记录.Rows[0]["累计同规格膜卷重量T"].ToString();
            //内表信息
            int rownum = dt记录详情.Rows.Count > 10 ? 10 : dt记录详情.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[8 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["开始时间"].ToString()).ToString("HH:mm") + " ~ " + Convert.ToDateTime(dt记录详情.Rows[i]["结束时间"].ToString()).ToString("HH:mm");
                mysheet.Cells[8 + i, 3].Value = dt记录详情.Rows[i]["膜卷编号"].ToString();
                mysheet.Cells[8 + i, 4].Value = dt记录详情.Rows[i]["膜卷长度"].ToString();
                mysheet.Cells[8 + i, 5].Value = dt记录详情.Rows[i]["膜卷重量"].ToString();
                mysheet.Cells[8 + i, 6].Value = dt记录详情.Rows[i]["记录人"].ToString();
                mysheet.Cells[8 + i, 7].Value = dt记录详情.Rows[i]["外观"].ToString() == "Yes" ? "√" : "×";
                mysheet.Cells[8 + i, 8].Value = dt记录详情.Rows[i]["宽度"].ToString();
                mysheet.Cells[8 + i, 9].Value = dt记录详情.Rows[i]["最大厚度"].ToString();
                mysheet.Cells[8 + i, 10].Value = dt记录详情.Rows[i]["最小厚度"].ToString();
                mysheet.Cells[8 + i, 11].Value = dt记录详情.Rows[i]["平均厚度"].ToString();
                mysheet.Cells[8 + i, 12].Value = dt记录详情.Rows[i]["厚度公差"].ToString();
                mysheet.Cells[8 + i, 13].Value = dt记录详情.Rows[i]["检查人"].ToString();
                mysheet.Cells[8 + i, 14].Value = dt记录详情.Rows[i]["判定"].ToString() == "Yes" ? "√" : "×";
            }
            return mysheet;
        }

        //******************************小功能******************************//  

        //求合计
        private void getTotal()
        {
            int numtemp;
            // 膜卷长度求和
            sum[0] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["膜卷长度"].ToString(), out numtemp) == true)
                { sum[0] += numtemp; }
            }
            dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            // 膜卷重量求和
            sum[1] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["膜卷重量"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];            
        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb环境温度, tb环境湿度});
            List<String> StringList = new List<String>(new String[] { "环境温度", "环境湿度" });
            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show("『" + StringList[i] + "』框内应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }

        // 检查 记录人的姓名
        private bool Name_check1()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["记录人"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["记录人"] = mySystem.Parameter.userName;
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的『记录人』信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
        }

        // 检查 检查人的姓名
        private bool Name_check2()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["检查人"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["检查人"] = "";
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的『检查人』信息", "ERROR");
                    TypeCheck = false;
                }
                else
                { dataGridView1.Rows[i].ReadOnly = true; }
            }
            return TypeCheck;
        }

        //******************************选择条件******************************//  

        //产品名称改变
        private void cb产品名称_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb产品名称.SelectedIndex >= 0)
            { DataShow(mySystem.Parameter.proInstruID, cb产品名称.Text, cb白班.Checked, dtp生产日期.Value); }            
        }

        //生产日期改变
        private void dtp生产日期_ValueChanged(object sender, EventArgs e)
        {
            if (cb产品名称.SelectedIndex >= 0)
            { DataShow(mySystem.Parameter.proInstruID, cb产品名称.Text, cb白班.Checked, dtp生产日期.Value); }            
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

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷长度")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷重量")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "记录人")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["记录人"] = "";
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『记录人』信息", "ERROR");
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "检查人")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["检查人"] = " ";
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『检查人』信息", "ERROR");
                    }
                }
                else
                { }
            }
        }

        //是否勾选
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dt记录详情.Rows.Count > 0 && dt记录.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]) == false)
                {
                    //外观
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "外观_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["外观_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "外观_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["外观_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = false;
                        }
                    }
                    // 判定
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "判定_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["判定_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "判定_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["判定_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = false;
                        }
                    }
                    else
                    { }
                }
            }
        }

         
    
    }
}
