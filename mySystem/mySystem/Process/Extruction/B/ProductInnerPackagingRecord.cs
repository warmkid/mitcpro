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
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace mySystem.Extruction.Process
{
    public partial class ProductInnerPackagingRecord : BaseForm
    {
        private String table = "产品内包装记录表";
        private String tableInfo = "产品内包装详细记录";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private CheckForm check = null;

        private DataTable dt记录, dt记录详情, dt代码批号; 
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private Int32 KeyID;
        
        //绑定dtp生产日期，会无限死循环？？？
        //Init()后，后面始终无法恢复 ---> groupbox的问题
        public ProductInnerPackagingRecord(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;

            Init(); 
            GetProductInfo();
        }

        public ProductInnerPackagingRecord(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            Init();
            IDShow(ID);
            foreach (Control c in this.Controls) { c.Enabled = false; }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
        }

        //******************************初始化******************************//
        //控件不可用
        private void Init()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; } //后面始终无法恢复

            {
                //EnableInit(false);
                //AddLineBtn.Enabled = false;
                //DelLineBtn.Enabled = false;
                //SaveBtn.Enabled = false;
                //CheckBtn.Enabled = false;
                //tb审核人.Enabled = false;
                //dtp审核日期.Enabled = false;
                //printBtn.Enabled = false;
            }
            cb产品代码.Enabled = true;
            dtp生产日期.Enabled = true;
            tb产品批号.ReadOnly = true;
        }

        //可编辑，控件初始化
        private void EnableInit(bool able)
        {
            this.groupBox2.Enabled = true;
            this.dataGridView1.Enabled = able;

            this.groupBox1.Enabled = able;
            //this.tb包材名称.Enabled = able;
            //this.tb包材批号.Enabled = able;
            //this.tb包材接上班数量.Enabled = able;
            //this.tb包材领取数量.Enabled = able;
            //this.tb包材剩余数量.Enabled = able;
            //this.tb包材使用数量.Enabled = able;
            //this.tb包材退库数量.Enabled = able;
            //this.tb指示剂批号.Enabled = able;
            //this.tb指示剂接上班数量.Enabled = able;
            //this.tb指示剂领取数量.Enabled = able;
            //this.tb指示剂剩余数量.Enabled = able;
            //this.tb指示剂使用数量.Enabled = able;
            //this.tb指示剂退库数量.Enabled = able;

            this.groupBox3.Enabled = able;
            //this.tb标签发放数量.Enabled = able;
            //this.tb标签使用数量.Enabled = able;
            //this.tb标签销毁数量.Enabled = able;
            //this.tb包装规格.Enabled = able;
            //this.tb总计包数.Enabled = able;
            //this.tb每片只数.Enabled = able;
            //this.cb标签语言是否中文.Enabled = able;
            //this.cb标签语言是否英文.Enabled = able;

            this.tb操作人.Enabled = able;
            this.dtp操作日期.Enabled = able;
        }

        //产品名称、产品批号列表获取+产品工艺、设备、班次填写
        private void GetProductInfo()
        {
            dt代码批号 = new DataTable("代码批号");

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (!isSqlOk)
            {
                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + mySystem.Parameter.proInstruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    //查找该生产ID下的产品编码、产品批号
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select ID, 产品编码, 产品批号 from 生产指令产品列表 where 生产指令ID = " + reader1["ID"].ToString();
                    OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                    datemp.Fill(dt代码批号);
                    if (dt代码批号.Rows.Count == 0)
                    { 
                        /* 尚未生成该生产指令下的信息 */
                        MessageBox.Show("该生产指令编码下的『生产指令产品列表』尚未生成！");
                    }
                    else
                    {
                        for (int i = 0; i < dt代码批号.Rows.Count; i++)
                        {
                            cb产品代码.Items.Add(dt代码批号.Rows[i]["产品编码"].ToString());//添加
                        }
                    }
                    datemp.Dispose();
                }
                else
                {
                    MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
                    //dt代码批号为空
                    ////填入产品编码、批号 
                    //dt代码批号.Columns.Add("产品编码", typeof(String));   //新建第一列
                    //dt代码批号.Columns.Add("产品批号", typeof(String));      //新建第二列
                    //dt代码批号.Rows.Add(" ", " ");
                }
                reader1.Dispose();
            }
            else
            {
                //从SQL数据库中读取;

            }

            //*********数据填写*********//
            //cb产品代码.SelectedIndex = 0;
            //tb产品批号.Text = dt代码批号.Rows[0]["产品批号"].ToString();
            cb产品代码.SelectedIndex = -1;
            tb产品批号.Text = "";
        }

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID, String productCode, DateTime searchTime)
        {
            Init();

            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productCode, searchTime);
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
                readOuterData(InstruID, productCode, searchTime);
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
                if (isnew == true)
                {
                    //新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = true;
                }
                else
                {
                    //非新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    tb审核人.Enabled = true;
                    dtp审核日期.Enabled = true;
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

            readOuterData(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品代码"].ToString(), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            outerBind();
            readInnerData(ID);
            setDataGridViewColumns();
            innerBind();  
        }

        //****************************** 嵌套 ******************************//
        
        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            //最上面
            cb产品代码.DataBindings.Clear();
            cb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            //dtp生产日期.DataBindings.Clear();
            //dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            //左中（包材、指示剂）
            tb包材名称.DataBindings.Clear();
            tb包材名称.DataBindings.Add("Text", bs记录.DataSource, "包材名称");
            tb包材批号.DataBindings.Clear();
            tb包材批号.DataBindings.Add("Text", bs记录.DataSource, "包材批号");
            tb包材接上班数量.DataBindings.Clear();
            tb包材接上班数量.DataBindings.Add("Text", bs记录.DataSource, "包材接上班数量");
            tb包材领取数量.DataBindings.Clear();
            tb包材领取数量.DataBindings.Add("Text", bs记录.DataSource, "包材领取数量");
            tb包材剩余数量.DataBindings.Clear();
            tb包材剩余数量.DataBindings.Add("Text", bs记录.DataSource, "包材剩余数量");
            tb包材使用数量.DataBindings.Clear();
            tb包材使用数量.DataBindings.Add("Text", bs记录.DataSource, "包材使用数量");
            tb包材退库数量.DataBindings.Clear();
            tb包材退库数量.DataBindings.Add("Text", bs记录.DataSource, "包材退库数量");
            tb指示剂批号.DataBindings.Clear();
            tb指示剂批号.DataBindings.Add("Text", bs记录.DataSource, "指示剂批号");
            tb指示剂接上班数量.DataBindings.Clear();
            tb指示剂接上班数量.DataBindings.Add("Text", bs记录.DataSource, "指示剂接上班数量");
            tb指示剂领取数量.DataBindings.Clear();
            tb指示剂领取数量.DataBindings.Add("Text", bs记录.DataSource, "指示剂领取数量");
            tb指示剂剩余数量.DataBindings.Clear();
            tb指示剂剩余数量.DataBindings.Add("Text", bs记录.DataSource, "指示剂剩余数量");
            tb指示剂使用数量.DataBindings.Clear();
            tb指示剂使用数量.DataBindings.Add("Text", bs记录.DataSource, "指示剂使用数量");
            tb指示剂退库数量.DataBindings.Clear();
            tb指示剂退库数量.DataBindings.Add("Text", bs记录.DataSource, "指示剂退库数量");
            //右侧，数据统计
            tb标签发放数量.DataBindings.Clear();
            tb标签发放数量.DataBindings.Add("Text", bs记录.DataSource, "标签发放数量");
            tb标签使用数量.DataBindings.Clear();
            tb标签使用数量.DataBindings.Add("Text", bs记录.DataSource, "标签使用数量");
            tb标签销毁数量.DataBindings.Clear();
            tb标签销毁数量.DataBindings.Add("Text", bs记录.DataSource, "标签销毁数量");
            tb包装规格.DataBindings.Clear();
            tb包装规格.DataBindings.Add("Text", bs记录.DataSource, "包装规格");
            tb总计包数.DataBindings.Clear();
            tb总计包数.DataBindings.Add("Text", bs记录.DataSource, "总计包数");
            tb每片只数.DataBindings.Clear();
            tb每片只数.DataBindings.Add("Text", bs记录.DataSource, "每片只数");
            cb标签语言是否中文.DataBindings.Clear();
            cb标签语言是否中文.DataBindings.Add("Checked", bs记录.DataSource, "标签语言是否中文");
            cb标签语言是否英文.DataBindings.Clear();
            cb标签语言是否英文.DataBindings.Add("Checked", bs记录.DataSource, "标签语言是否英文");
            //下面，操作人、审核人
            tb操作人.DataBindings.Clear();
            tb操作人.DataBindings.Add("Text", bs记录.DataSource, "操作人");
            dtp操作日期.DataBindings.Clear();
            dtp操作日期.DataBindings.Add("Text", bs记录.DataSource, "操作日期");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["产品代码"] = cb产品代码.Text;
            dr["产品批号"] = dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["产品批号"].ToString();
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T产品内包装记录ID = " + ID.ToString(), connOle);
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
            dr["T产品内包装记录ID"] = ID;
            dr["生产时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            dr["产品外观"] = 0;
            dr["包装后外观"] = 0;
            dr["包装袋热封线"] = 0;
            dr["贴标签"] = 0;
            dr["贴指示剂"] = 0;
            dr["包装人"] = mySystem.Parameter.userName;
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
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
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
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T产品内包装记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["生产时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //******************************按钮功能******************************//

        //添加行按钮
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(KeyID, dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
        }

        //删除行按钮 
        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                dt记录详情.Rows.RemoveAt(deletenum);
                setDataGridViewRowNums();
            }
        }
        
        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Name_check() == false)
            { /*包装人不合格*/}
            else if (mySystem.Parameter.NametoID(tb操作人.Text.ToString()) == 0)
            { 
                /*操作人不合格*/
                MessageBox.Show("请重新输入『操作人』信息", "ERROR");
            }
            else if (TextBox_check() == false)
            { /*各种数量填写不合格*/ }
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
                    readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, dtp生产日期.Value);
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
            check.Show();
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(@"D:\excel\SOP-MFG-109-R01A 产品内包装记录_何.xlsx");
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

        //打印填数据
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet)
        {
            //外表信息
            mysheet.Cells[3, 1].Value = "产品代码/规格：" + dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[3, 7].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 11].Value = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[5, 10].Value = dt记录.Rows[0]["包材名称"].ToString();
            mysheet.Cells[5, 11].Value = dt记录.Rows[0]["包材批号"].ToString();
            mysheet.Cells[5, 12].Value = dt记录.Rows[0]["包材接上班数量"].ToString();
            mysheet.Cells[5, 13].Value = dt记录.Rows[0]["包材领取数量"].ToString();
            mysheet.Cells[5, 14].Value = dt记录.Rows[0]["包材剩余数量"].ToString();
            mysheet.Cells[5, 15].Value = dt记录.Rows[0]["包材使用数量"].ToString();
            mysheet.Cells[5, 16].Value = dt记录.Rows[0]["包材退库数量"].ToString();
            mysheet.Cells[7, 11].Value = dt记录.Rows[0]["指示剂批号"].ToString();
            mysheet.Cells[7, 12].Value = dt记录.Rows[0]["指示剂接上班数量"].ToString();
            mysheet.Cells[7, 13].Value = dt记录.Rows[0]["指示剂领取数量"].ToString();
            mysheet.Cells[7, 14].Value = dt记录.Rows[0]["指示剂剩余数量"].ToString();
            mysheet.Cells[7, 15].Value = dt记录.Rows[0]["指示剂使用数量"].ToString();
            mysheet.Cells[7, 16].Value = dt记录.Rows[0]["指示剂退库数量"].ToString();            
            String stringtemp = "";
            stringtemp = "标签：  发放数量" + dt记录.Rows[0]["标签发放数量"].ToString() + "张；  ";
            stringtemp = stringtemp + "使用数量" + dt记录.Rows[0]["标签使用数量"].ToString() + "张；  ";
            stringtemp = stringtemp + "销毁数量" + dt记录.Rows[0]["标签销毁数量"].ToString() + "张。";
            mysheet.Cells[8, 10].Value = stringtemp;
            stringtemp = "包装规格：" + dt记录.Rows[0]["包装规格"].ToString() + "只/包；   标签：中文";
            stringtemp = stringtemp + (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否中文"].ToString()) == true ? "√" : "×") + " 英文";
            stringtemp = stringtemp + (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否英文"].ToString()) == true ? "√" : "×");
            stringtemp = stringtemp + "\n总 计：共包装" + dt记录.Rows[0]["总计包数"].ToString() + "包；   计";
            stringtemp = stringtemp + dt记录.Rows[0]["每片只数"].ToString() + "只/片。";
            mysheet.Cells[9, 10].Value = stringtemp;
            stringtemp = "操作人：" + dt记录.Rows[0]["操作人"].ToString();
            stringtemp = stringtemp + "       操作日期：" + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[17, 1].Value = stringtemp;
            stringtemp = "审核人：" + dt记录.Rows[0]["审核人"].ToString();
            stringtemp = stringtemp + "       审核日期：" + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[17, 10].Value = stringtemp;

            //内表信息
            int rownum = dt记录详情.Rows.Count > 12 ? 12 : dt记录详情.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[5 + i, 2].Value = dt记录详情.Rows[i]["生产时间"].ToString();
                mysheet.Cells[5 + i, 3].Value = dt记录详情.Rows[i]["内包序号"].ToString();
                mysheet.Cells[5 + i, 4].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["产品外观"].ToString()));
                mysheet.Cells[5 + i, 5].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装后外观"].ToString()));
                mysheet.Cells[5 + i, 6].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装袋热封线"].ToString()));
                mysheet.Cells[5 + i, 7].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴标签"].ToString()));
                mysheet.Cells[5 + i, 8].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴指示剂"].ToString()));
                mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["包装人"].ToString();
            }
            return mysheet;
        }

        //打印小功能(0/1/2 -〉“√”/“×”/“N”)
        private String okayInt2String(int okaynum)
        {
            String okaystring = "";
            switch (okaynum)
            {
                case 0:
                    okaystring = "√";
                    break;
                case 1:
                    okaystring = "×";
                    break;
                case 2:
                    okaystring = "N";
                    break;
                default:
                    okaystring = "填表时未按要求填入数据!";
                    break;
            }
            return okaystring;
        }

        //******************************小功能******************************// 

        // 检查 包装的姓名
        private bool Name_check()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["包装人"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["包装人"] = mySystem.Parameter.userName;
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的『包装人』信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb包材接上班数量, tb包材领取数量, tb包材剩余数量, tb包材使用数量, tb包材退库数量, 
                tb指示剂接上班数量, tb指示剂领取数量, tb指示剂剩余数量, tb指示剂使用数量, tb指示剂退库数量, 
                tb标签发放数量, tb标签使用数量, tb标签销毁数量, tb包装规格, tb总计包数, tb每片只数 });

            List<String> StringList = new List<String>(new String[] { "包材接上班数量", "包材领取数量", "包材剩余数量", "包材使用数量", "包材退库数量", 
                "指示剂接上班数量", "指示剂领取数量", "指示剂剩余数量", "指示剂使用数量", "指示剂退库数量", 
                "标签发放数量", "标签使用数量", "标签销毁数量", "包装规格", "总计包数", "每片只数" });
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
        
        //******************************选择条件******************************//  
        
        //产品代码改变
        private void cb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DataShow(mySystem.Parameter.proInstruID, cb产品代码.Text.ToString(), dtp生产日期.Value); }            
        }

        //生产日期改变
        private void dtp生产日期_ValueChanged(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DataShow(mySystem.Parameter.proInstruID, cb产品代码.Text.ToString(), dtp生产日期.Value); }            
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

        //实时检查包装人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "包装人")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["包装人"] = "";
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『包装人』信息", "ERROR");
                    }
                }
                else
                { }
            }
        }
        
    }
}
