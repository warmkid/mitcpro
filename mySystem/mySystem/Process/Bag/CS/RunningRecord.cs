using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using mySystem;

namespace mySystem.Process.Bag
{
    public partial class RunningRecord : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;//审核信息
        private DataTable dt_out, dt_in;
        private OleDbDataAdapter da_out, da_in;
        private BindingSource bs_out, bs_in;
        private OleDbCommandBuilder cb_out, cb_in;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;
        DateTime date;
        String instrcode;

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        public RunningRecord(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            dtp生产日期.Enabled = true;
            bt插入查询.Enabled = true;
        }

        public RunningRecord(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            string asql = "select * from 制袋机组运行记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            instrid = (int)tempdt.Rows[0]["生产指令ID"];
            date = DateTime.Parse(tempdt.Rows[0]["生产日期"].ToString());
           // instrcode = Parameter.csbagInstruction;

            readOuterData(instrid,date);
            outerBind();

            readInnerData((int)dt_out.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            dtp生产日期.Enabled = false;
            bt插入查询.Enabled = false;

        }

        // 设置读取数据的事件
        void addDataEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;
        }

        //表格填写错误提示
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='制袋机运行记录'", mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
                string[] s = Regex.Split(person_操作员, ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        list_操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(person_审核员, ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        list_审核员.Add(s1[i]);
                }
            }

        }
        void getOtherData()
        {
            //获取打印机
            fill_printer();
        }
        //设置登录人状态
        void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (list_操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (list_审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
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

        void setFormState()
        {
            string s = dt_out.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dt_out.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        void setEnableReadOnly()
        {
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
                    bt审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }
        }

        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮一直是false
            bt审核.Enabled = false;
            bt提交审核.Enabled = false;
        }

        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            bt日志.Enabled = true;
            bt打印.Enabled = true;
            cb打印机.Enabled = true;
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid, DateTime dtime)
        {
            dt_out = new DataTable("制袋机组运行记录");
            bs_out = new BindingSource();
            da_out = new OleDbDataAdapter("select * from 制袋机组运行记录 where 生产指令ID=" + instrid + " and 生产日期=#" + dtime + "#", mySystem.Parameter.connOle);
            cb_out = new OleDbCommandBuilder(da_out);
            da_out.Fill(dt_out);
        }

        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_in = new DataTable("制袋机组运行记录详细信息");
            bs_in = new BindingSource();
            da_in = new OleDbDataAdapter("select * from 制袋机组运行记录详细信息 where T制袋机组运行记录ID=" + id, mySystem.Parameter.connOle);
            cb_in = new OleDbCommandBuilder(da_in);
            da_in.Fill(dt_in);
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.csbagInstruID;
            dr["生产日期"] = DateTime.Parse(dtp生产日期.Value.ToShortDateString());
            dr["生产环境温度"] = 0;
            dr["生产环境湿度"] = 0;
            dr["上切刀温度"] = 0;
            dr["下切刀温度"] = 0;
            dr["冷却温度"] = 0;

            dr["审核员"] = "";
            dr["审核是否通过"] = false;
            dr["审核日期"] = DateTime.Now;

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编号：" + mySystem.Parameter.csbagInstruction + "\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T制袋机组运行记录ID"] = dt_out.Rows[0]["ID"];
            dr["检查时间"] = DateTime.Now;
            dr["横封温度1"] = 0;
            dr["横封温度2"] = 0;
            dr["纵封温度3"] = 0;
            dr["纵封温度4"] = 0;
            dr["纵封温度5"] = 0;
            dr["热封时间横封"] = 0;
            dr["热封时间纵封"] = 0;
            dr["传输速度"] = 0;
            dr["流量计"] = 0;
            dr["操作员"] = mySystem.Parameter.userName;
            return dr;
        }

        // 外表和控件的绑定
        void outerBind()
        {
            dtp生产日期.DataBindings.Clear();
            tb环境温度.DataBindings.Clear();
            tb环境湿度.DataBindings.Clear();
            tb上切刀温度.DataBindings.Clear();
            tb下切刀温度.DataBindings.Clear();
            tb冷却温度.DataBindings.Clear();
            tb审核员.DataBindings.Clear();
            dtp审核日期.DataBindings.Clear();

            bs_out.DataSource = dt_out;

            dtp生产日期.DataBindings.Add("Value", bs_out.DataSource, "生产日期");
            tb环境温度.DataBindings.Add("Text", bs_out.DataSource, "生产环境温度");
            tb环境湿度.DataBindings.Add("Text", bs_out.DataSource, "生产环境湿度");
            tb上切刀温度.DataBindings.Add("Text", bs_out.DataSource, "上切刀温度");
            tb下切刀温度.DataBindings.Add("Text", bs_out.DataSource, "下切刀温度");
            tb冷却温度.DataBindings.Add("Text", bs_out.DataSource, "冷却温度");
            tb审核员.DataBindings.Add("Text", bs_out.DataSource, "审核员");
            dtp审核日期.DataBindings.Add("Value", bs_out.DataSource, "审核日期");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewCombox();
            bs_in.DataSource = dt_in;
            dataGridView1.DataSource = bs_in.DataSource;
            setDataGridViewColumns();
        }

        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_in.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "横封温度1":
                        DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "1#";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c1);
                        break;

                    case "横封温度2":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = "2#";
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;
                    case "纵封温度3":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = "3#";
                        c4.Name = dc.ColumnName;
                        c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c4.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c4);
                        break;
                    case "纵封温度4":
                        DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
                        c5.DataPropertyName = dc.ColumnName;
                        c5.HeaderText = "4#";
                        c5.Name = dc.ColumnName;
                        c5.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c5.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c5);
                        break;
                    case "纵封温度5":
                        DataGridViewTextBoxColumn c6 = new DataGridViewTextBoxColumn();
                        c6.DataPropertyName = dc.ColumnName;
                        c6.HeaderText = "5#";
                        c6.Name = dc.ColumnName;
                        c6.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c6.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c6);
                        break;
                    case "热封时间横封":
                        DataGridViewTextBoxColumn c7 = new DataGridViewTextBoxColumn();
                        c7.DataPropertyName = dc.ColumnName;
                        c7.HeaderText = "横封";
                        c7.Name = dc.ColumnName;
                        c7.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c7.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c7);
                        break;
                    case "热封时间纵封":
                        DataGridViewTextBoxColumn c8 = new DataGridViewTextBoxColumn();
                        c8.DataPropertyName = dc.ColumnName;
                        c8.HeaderText = "纵封";
                        c8.Name = dc.ColumnName;
                        c8.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c8.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c8);
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        private bool input_Judge()
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
            if (!float.TryParse(tb上切刀温度.Text, out a))
            {
                MessageBox.Show("上切刀温度必须为数字");
                return false;
            }
            if (!float.TryParse(tb下切刀温度.Text, out a))
            {
                MessageBox.Show("下切刀温度必须为数字");
                return false;
            }
            if (!float.TryParse(tb冷却温度.Text, out a))
            {
                MessageBox.Show("冷却温度必须为数字");
                return false;
            }
            return true;
        }
        //保存内表和外表数据
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
            {
                return false;
            }

            //外表保存
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
            readOuterData(instrid, date);
            outerBind();

            //内表保存
            da_in.Update((DataTable)bs_in.DataSource);
            readInnerData(Convert.ToInt32(dt_out.Rows[0]["ID"]));
            innerBind();
            return true;
        }
        private void bt保存_Click(object sender, EventArgs e)
        {
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt提交审核.Enabled = true;
        }

        private void bt插入查询_Click(object sender, EventArgs e)
        {
            instrid = mySystem.Parameter.csbagInstruID;
            date = DateTime.Parse(dtp生产日期.Value.ToShortDateString());
            readOuterData(instrid, date);
            outerBind();
            if (dt_out.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                return;
            }

            if (dt_out.Rows.Count <= 0)
            {
                //新建记录
                DataRow dr = dt_out.NewRow();
                dr = writeOuterDefault(dr);
                dt_out.Rows.Add(dr);
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(instrid, date);
                outerBind();
            }

            readInnerData((int)dt_out.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();
        }

        private void bt提交审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='制袋机组运行记录' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "制袋机组运行记录";
                dr["对应ID"] = (int)dt_out.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

            dt_out.Rows[0]["审核员"] = "__待审核";
            dt_out.Rows[0]["审核日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }
        public override void CheckResult()
        {
            //获得审核信息
            dt_out.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt_out.Rows[0]["审核日期"] = checkform.time;
            dt_out.Rows[0]["审核意见"] = checkform.opinion;
            dt_out.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='制袋机组运行记录' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
            base.CheckResult();
        }


        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }
        private void bt打印_Click(object sender, EventArgs e)
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\CSBag\SOP-MFG-303-R03A  2# 制袋机运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];
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
                        dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

                        bs_out.EndEdit();
                        da_out.Update((DataTable)bs_out.DataSource);
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
            mysheet.Cells[3, 1].Value = "生产日期：" + Convert.ToDateTime(dt_out.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt_out.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt_out.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[4, 3].Value = "环境温度：" + Convert.ToDouble(dt_out.Rows[0]["生产环境温度"]).ToString() + " ℃      环境湿度：" + Convert.ToDouble(dt_out.Rows[0]["生产环境湿度"]).ToString() + "%";
            mysheet.Cells[3, 11].Value = "上切刀温度：" + Convert.ToDouble(dt_out.Rows[0]["上切刀温度"]).ToString() + " ℃";
            mysheet.Cells[4, 11].Value = "下切刀温度：" + Convert.ToDouble(dt_out.Rows[0]["下切刀温度"]).ToString() + " ℃";
            mysheet.Cells[3, 15].Value = "冷却温度：" + Convert.ToDouble(dt_out.Rows[0]["冷却温度"]).ToString() + " ℃";
            
            //内表信息
            int rownum = dt_in.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 10 ? 10 : rownum); i++)
            {
                mysheet.Cells[7 + i, 1].Value = dt_in.Rows[i]["检查时间"].ToString();
                mysheet.Cells[7 + i, 2].Value = dt_in.Rows[i]["横封温度1"].ToString();
                mysheet.Cells[7 + i, 3].Value = dt_in.Rows[i]["横封温度2"].ToString();
                mysheet.Cells[7 + i, 4].Value = dt_in.Rows[i]["纵封温度3"].ToString();
                mysheet.Cells[7 + i, 5].Value = dt_in.Rows[i]["纵封温度4"].ToString();
                mysheet.Cells[7 + i, 6].Value = dt_in.Rows[i]["纵封温度5"].ToString();
                mysheet.Cells[7 + i, 7].Value = dt_in.Rows[i]["热封时间横封"].ToString();
                mysheet.Cells[7 + i, 8].Value = dt_in.Rows[i]["热封时间纵封"].ToString();
                mysheet.Cells[7 + i, 9].Value = dt_in.Rows[i]["传输速度"].ToString();
                mysheet.Cells[7 + i, 10].Value = dt_in.Rows[i]["流量计"].ToString();
                mysheet.Cells[7 + i, 11].Value = dt_in.Rows[i]["产品质量"].ToString();
                mysheet.Cells[7 + i, 12].Value = dt_in.Rows[i]["张力均匀"].ToString();
                mysheet.Cells[7 + i, 13].Value = dt_in.Rows[i]["膜材运转"].ToString();
                mysheet.Cells[7 + i, 14].Value = dt_in.Rows[i]["纠偏控制器"].ToString();
                mysheet.Cells[7 + i, 15].Value = dt_in.Rows[i]["切刀运行"].ToString();
                mysheet.Cells[7 + i, 16].Value = dt_in.Rows[i]["操作员"].ToString();
            }
            //需要插入的部分
            if (rownum > 10)
            {
                for (int i = 10; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[7 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[7 + i, 1].Value = dt_in.Rows[i]["检查时间"].ToString();
                    mysheet.Cells[7 + i, 2].Value = dt_in.Rows[i]["横封温度1"].ToString();
                    mysheet.Cells[7 + i, 3].Value = dt_in.Rows[i]["横封温度2"].ToString();
                    mysheet.Cells[7 + i, 4].Value = dt_in.Rows[i]["纵封温度3"].ToString();
                    mysheet.Cells[7 + i, 5].Value = dt_in.Rows[i]["纵封温度4"].ToString();
                    mysheet.Cells[7 + i, 6].Value = dt_in.Rows[i]["纵封温度5"].ToString();
                    mysheet.Cells[7 + i, 7].Value = dt_in.Rows[i]["热封时间横封"].ToString();
                    mysheet.Cells[7 + i, 8].Value = dt_in.Rows[i]["热封时间纵封"].ToString();
                    mysheet.Cells[7 + i, 9].Value = dt_in.Rows[i]["传输速度"].ToString();
                    mysheet.Cells[7 + i, 10].Value = dt_in.Rows[i]["流量计"].ToString();
                    mysheet.Cells[7 + i, 11].Value = dt_in.Rows[i]["产品质量"].ToString();
                    mysheet.Cells[7 + i, 12].Value = dt_in.Rows[i]["张力均匀"].ToString();
                    mysheet.Cells[7 + i, 13].Value = dt_in.Rows[i]["膜材运转"].ToString();
                    mysheet.Cells[7 + i, 14].Value = dt_in.Rows[i]["纠偏控制器"].ToString();
                    mysheet.Cells[7 + i, 15].Value = dt_in.Rows[i]["切刀运行"].ToString();
                    mysheet.Cells[7 + i, 16].Value = dt_in.Rows[i]["操作员"].ToString();
                }
                ind = rownum - 10;
            }
            mysheet.Cells[7, 17].Value = dt_out.Rows[0]["审核员"].ToString() + "\n" + Convert.ToDateTime(dt_out.Rows[0]["审核日期"]).ToString("D");
            
            mysheet.Cells[18 + ind, 1].Value = " 备注：填写方式：正常或合格划“√”，异常写明原因。 ";
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from 制袋机组运行记录  where 生产指令ID=" + instrid.ToString(), mySystem.Parameter.connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt_out.Rows[0]["ID"])) + 1;
            //instrcode怎样获取？
            //读取ID对应的生产指令编码
            OleDbCommand comm生产指令编码 = new OleDbCommand();
            comm生产指令编码.Connection = mySystem.Parameter.connOle;
            comm生产指令编码.CommandText = "select * from 生产指令 where ID= @name";
            comm生产指令编码.Parameters.AddWithValue("@name", instrid);

            OleDbDataReader myReader生产指令编码 = comm生产指令编码.ExecuteReader();
            while (myReader生产指令编码.Read())
            {
                instrcode = myReader生产指令编码["生产指令编号"].ToString();
                //List<String> list班次 = new List<string>();
                //list班次.Add(myReader班次["班次"].ToString());
            }

            myReader生产指令编码.Close();
            comm生产指令编码.Dispose();  
            mysheet.PageSetup.RightFooter = instrcode + "-03-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }
           

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_in.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);
            dt_in.Rows.Add(dr);
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            //da_in.Update((DataTable)bs_in.DataSource);
            //readInnerData((int)dt_in.Rows[0]["ID"]);
            //innerBind();
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            (new mySystem.Other.LogForm()).setLog(dt_out.Rows[0]["日志"].ToString()).Show();
        }
    }
}
