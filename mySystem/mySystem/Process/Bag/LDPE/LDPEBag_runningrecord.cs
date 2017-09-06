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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPEBag_runningrecord : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情; 
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;
        
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String str生产指令;
        DateTime date;

        public LDPEBag_runningrecord(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            setControlFalse();
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
            btn打印.Enabled = false;

            dtp日期.Enabled = true;
            bt插入查询.Enabled = true;
        }

        public LDPEBag_runningrecord(MainForm mainform, Int32 ID)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            string asql = "select * from 制袋机组运行记录 where ID=" + ID;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            InstruID = (int)tempdt.Rows[0]["生产指令ID"];
            date = DateTime.Parse(tempdt.Rows[0]["生产日期"].ToString());

            readOuterData(InstruID, date);
            outerBind();

            readInnerData((int)dt记录.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            dtp日期.Enabled = false;
            bt插入查询.Enabled = false;
        }

        //******************************初始化******************************//

        // 获取操作员和审核员
        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='制袋机运行记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string[] s = Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        ls操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        ls审核员.Add(s1[i]);
                }
            }
        }

        // 根据登录人，设置stat_user
        private void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = "管理员";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = "审核员";
        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setFormState(bool newForm = false)
        {
            if (newForm)
            {
                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dt记录.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]);
            if (s == "") _formState = Parameter.FormState.未保存;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        //读取设置内容  //GetProductInfo //产品代码、产品批号初始化
        private void getOtherData()
        {
            //获取打印机
            fill_printer();
        }

        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (_userState == Parameter.UserState.管理员)//管理员
            {
                //控件都能点
                setControlTrue();
            }
            else if (_userState == Parameter.UserState.审核员)//审核人
            {
                if (_formState == Parameter.FormState.待审核)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
            }
            else//操作员
            {
                if (_formState == Parameter.FormState.待审核 || _formState == Parameter.FormState.审核通过) //1待审核||2审核通过
                {
                    //控件都不能点
                    setControlFalse();
                }
                else
                {
                    //发送审核，审核不能点
                    setControlTrue();
                }
            }
        }

        /// <summary>
        /// 设置所有控件可用；
        /// btn审核、btn提交审核两个按钮一直是false；内表审核、提交审核false
        /// 部分控件防作弊，不可改；
        /// 查询条件始终不可编辑
        /// </summary>
        void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = false;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮、审核人姓名框一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            tb审核员.ReadOnly = true;
            
        }

        /// <summary>
        /// 设置所有控件不可用；
        /// 查看日志、打印始终可用
        /// </summary>
        void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = true;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            //查看日志、打印始终可用
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cb打印机.Enabled = true;
        }

        // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
        private void addOtherEvnetHandler()
        {
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件：TextChanged&Leave
        private void addComputerEventHandler() { }

        //修改单个控件的值
        private void outerDataSync(String name, String val)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == name)
                {
                    c.Text = val;
                    c.DataBindings[0].WriteValue();
                }
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID,DateTime dtime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable("制袋机组运行记录");
            da记录 = new OleDbDataAdapter("select * from 制袋机组运行记录 where 生产指令ID = " + InstruID + " and 生产日期=#" + dtime + "#", mySystem.Parameter.connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }
        
        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //解绑->绑定
            tb环境温度.DataBindings.Clear();
            tb环境温度.DataBindings.Add("Text", bs记录.DataSource, "生产环境温度");
            tb环境湿度.DataBindings.Clear();
            tb环境湿度.DataBindings.Add("Text", bs记录.DataSource, "生产环境湿度");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }
        
        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产日期"] = DateTime.Parse(dtp日期.Value.ToShortDateString());
            dr["审核员"] = "";
            dr["审核日期"] = DateTime.Now;
            dr["审核是否通过"] = false;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + mySystem.Parameter.ldpebagInstruction + "\n";
            dr["日志"] = log;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable("制袋机组运行记录详细信息");
            da记录详情 = new OleDbDataAdapter("select * from 制袋机组运行记录详细信息 where T制袋机组运行记录ID = " + ID, mySystem.Parameter.connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
            setDataGridViewFormat();
            bs记录详情.DataSource = dt记录详情;
            dataGridView1.DataSource = bs记录详情.DataSource;           
        }

        //添加行代码
        private DataRow writeInnerDefault(DataRow dr)
        {
            dr["T制袋机组运行记录ID"] = dt记录.Rows[0]["ID"];
            dr["检查时间"] = DateTime.Now;
            dr["热封刀温度"] = 0;
            dr["底座温度"] = 0;
            dr["制袋速度"] = 0;
            dr["切刀工作是否正常"] = "是";
            dr["张力控制是否正常"] = "是";
            dr["膜材运转是否平整"] = "是";
            dr["操作员"] = mySystem.Parameter.userName;
            return dr;
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
                    case "切刀工作是否正常":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("是");
                        cbc.Items.Add("否");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 120;
                        break;
                    case "张力控制是否正常":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("是");
                        cbc.Items.Add("否");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 120;
                        break;
                    case "膜材运转是否平整":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("是");
                        cbc.Items.Add("否");
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
            //隐藏
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T制袋机组运行记录ID"].Visible = false;

        }

        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
                btn提交审核.Enabled = true;
        }

        //保存功能
        private bool Save()
        {
            //温度湿度是否是数字
            float a;
            if (!float.TryParse(tb环境温度.Text, out a))
            {
                MessageBox.Show("环境温度必须为数字");
                return false;
            }
            if (!float.TryParse(tb环境湿度.Text, out a))
            {
                MessageBox.Show("环境湿度必须为数字");
                return false;
            }
            //检查操作员是否存在
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                string s=dataGridView1.Rows[i].Cells["操作员"].Value.ToString();
                if(mySystem.Parameter.NametoID(s)<=0)
                {
                    MessageBox.Show("第"+(i+1)+"行操作员不存在");
                    return false;
                }
            }

            // 内表保存
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();

            //外表保存
            bs记录.EndEdit();
            da记录.Update((DataTable)bs记录.DataSource);
            readOuterData(InstruID,date);
            outerBind();
            return true;

        }

        //提交审核按钮
        private void btn提交审核_Click_1(object sender, EventArgs e)
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='制袋机组运行记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "制袋机组运行记录";
                dr["对应ID"] = (int)dt记录.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            da_temp.Update(dt_temp);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 提交审核\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;
            dt记录.Rows[0]["审核员"] = "__待审核";

            Save();
            _formState = Parameter.FormState.待审核;
            setEnableReadOnly();
        } 

        //查看日志按钮
        private void btn查看日志_Click_1(object sender, EventArgs e)
        {
            mySystem.Other.LogForm logform = new mySystem.Other.LogForm();
            logform.setLog(dt记录.Rows[0]["日志"].ToString()).Show();
        }
        //审核功能
        private void btn审核_Click_1(object sender, EventArgs e)
        {
            //if (mySystem.Parameter.userName == dt记录.Rows[0]["操作员"].ToString())
            //{
            //    MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
            //    return;
            //}
            checkform = new CheckForm(this);
            checkform.Show();
        }
        //审核功能
        public override void CheckResult()
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            base.CheckResult();

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='制袋机组运行记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;

            Save();

            //修改状态，设置可控性
            if (checkform.ischeckOk)
            { _formState = Parameter.FormState.审核通过; }//审核通过
            else
            { _formState = Parameter.FormState.审核未通过; }//审核未通过              
            setEnableReadOnly();
        }

        //添加打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        private void fill_printer()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
        }

        //打印按钮
        private void btn打印_Click_1(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }
        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\LDPE\SOP-MFG-304-R03A 1#制袋机运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        string log = "=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;

                        bs记录.EndEdit();
                        da记录.Update((DataTable)bs记录.DataSource);
                    }
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
            }
        }

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            int ind = 0;
            //外表信息
            String str生产日期 = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            String str温度湿度 = "环境温度：" + Convert.ToDouble(dt记录.Rows[0]["生产环境温度"]).ToString() + " ℃      环境湿度：" + Convert.ToDouble(dt记录.Rows[0]["生产环境湿度"]).ToString() + "%";
            mysheet.Cells[5, 1].Value = str生产日期 + str温度湿度;
            

            //内表信息
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 6 ? 6 : rownum); i++)
            {
                mysheet.Cells[8 + i, 1].Value = dt记录详情.Rows[i]["检查时间"].ToString();
                mysheet.Cells[8 + i, 2].Value = dt记录详情.Rows[i]["热封刀温度"].ToString();
                mysheet.Cells[8 + i, 3].Value = dt记录详情.Rows[i]["底座温度"].ToString();
                mysheet.Cells[8 + i, 4].Value = dt记录详情.Rows[i]["制袋速度"].ToString();
                mysheet.Cells[8 + i, 5].Value = dt记录详情.Rows[i]["切刀工作是否正常"].ToString() == "Yes" ? "√" : "×";
                mysheet.Cells[8 + i, 6].Value = dt记录详情.Rows[i]["张力控制是否正常"].ToString() == "Yes" ? "√" : "×";
                mysheet.Cells[8 + i, 7].Value = dt记录详情.Rows[i]["膜材运转是否平整"].ToString() == "Yes" ? "√" : "×";
                mysheet.Cells[8 + i, 8].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[8 + i, 9].Value = dt记录详情.Rows[i]["操作员备注"].ToString();
            }
            //需要插入的部分
            if (rownum > 6)
            {
                for (int i = 6; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[8 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[8 + i, 1].Value = dt记录详情.Rows[i]["检查时间"].ToString();
                    mysheet.Cells[8 + i, 2].Value = dt记录详情.Rows[i]["热封刀温度"].ToString();
                    mysheet.Cells[8 + i, 3].Value = dt记录详情.Rows[i]["底座温度"].ToString();
                    mysheet.Cells[8 + i, 4].Value = dt记录详情.Rows[i]["制袋速度"].ToString();
                    mysheet.Cells[8 + i, 5].Value = dt记录详情.Rows[i]["切刀工作是否正常"].ToString() == "Yes" ? "√" : "×";
                    mysheet.Cells[8 + i, 6].Value = dt记录详情.Rows[i]["张力控制是否正常"].ToString() == "Yes" ? "√" : "×";
                    mysheet.Cells[8 + i, 7].Value = dt记录详情.Rows[i]["膜材运转是否平整"].ToString() == "Yes" ? "√" : "×";
                    mysheet.Cells[8 + i, 8].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[8 + i, 9].Value = dt记录详情.Rows[i]["操作员备注"].ToString();
                }
                ind = rownum - 6;
            }
           
            mysheet.Cells[14 + ind, 1].Value = " 备注：填写方式：正常或合格划“√”，异常写明原因。 ";
            mysheet.Cells[15 + ind, 6].Value = dt记录.Rows[0]["审核员"].ToString();
            mysheet.Cells[15 + ind, 7].Value = Convert.ToDateTime(dt记录.Rows[0]["审核日期"]).ToString("D");
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from 制袋机组运行记录  where 生产指令ID=" + InstruID.ToString(), mySystem.Parameter.connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            //instrcode怎样获取？
            //读取ID对应的生产指令编码
            OleDbCommand comm生产指令编码 = new OleDbCommand();
            comm生产指令编码.Connection = mySystem.Parameter.connOle;
            comm生产指令编码.CommandText = "select * from 生产指令 where ID= @name";
            comm生产指令编码.Parameters.AddWithValue("@name", InstruID);

            OleDbDataReader myReader生产指令编码 = comm生产指令编码.ExecuteReader();
            while (myReader生产指令编码.Read())
            {
                str生产指令 = myReader生产指令编码["生产指令编号"].ToString();
                //List<String> list班次 = new List<string>();
                //list班次.Add(myReader班次["班次"].ToString());
            }

            myReader生产指令编码.Close();
            comm生产指令编码.Dispose();  
            mysheet.PageSetup.RightFooter = str生产指令 + "-03-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
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

        //******************************插入查询按钮******************************// 
        private void bt插入查询_Click(object sender, EventArgs e)
        {
            InstruID = mySystem.Parameter.ldpebagInstruID;
            date = DateTime.Parse(dtp日期.Value.ToShortDateString());
            readOuterData(InstruID,date);
            outerBind();
            if (dt记录.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                return;
            }

            if (dt记录.Rows.Count <= 0)
            {
                //新建记录
                DataRow dr = dt记录.NewRow();
                dr = writeOuterDefault(dr);
                dt记录.Rows.Add(dr);
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(InstruID, date);
                outerBind();
            }

            readInnerData((int)dt记录.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            dtp日期.Enabled = false;
            bt插入查询.Enabled = false;
        }

        //******************************添加删除按钮功能******************************//   
        //添加按钮
        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);
            dt记录详情.Rows.Add(dr);
        }
        //删除按钮
        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }
        }       
    }
}

