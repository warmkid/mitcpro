﻿using System;
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

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVBag_runningrecordofyk : BaseForm
    {
        private String table = "圆口焊接机运行记录";
        private String tableInfo = "圆口焊接机运行记录详细信息";

        private SqlConnection conn = null;
        //private OleDbConnection mySystem.Parameter.conn = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情;
        private SqlDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private SqlCommandBuilder cb记录, cb记录详情;
        private int[] sum = { 0, 0 };

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        bool isFirstBind = true;

        public PTVBag_runningrecordofyk(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            //mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.ptvbagInstruID;
            Instruction = Parameter.ptvbagInstruction;
            tb生产指令编码.Text = Instruction;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            DataShow(InstruID);

        }

        public PTVBag_runningrecordofyk(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            //mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            IDShow(ID);
        }

        //******************************初始化******************************//

        // 获取操作员和审核员
        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='圆口焊接机运行记录'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

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
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
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

        //读取设置内容  //GetProductInfo //产品名称、产品批号列表获取+产品工艺、设备、班次填写
        private void getOtherData()
        {
            //*********产品代码、产品批号 -----> 数据获取*********//
            if (isSqlOk)
            {
                SqlCommand comm1 = new SqlCommand();
                comm1.Connection = mySystem.Parameter.conn;
                comm1.CommandText = "select * from 生产指令详细信息 where T生产指令ID = " + InstruID ;
                SqlDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    tb产品代码.Text = reader1["产品代码"].ToString();
                    tb产品批号.Text = reader1["产品批号"].ToString();
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
        }
        
        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (_userState == Parameter.UserState.管理员)
            {
                //控件都能点
                setControlTrue();
            }
            else if (_userState == Parameter.UserState.审核员)//审核人
            {
                if (_formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.审核未通过)  //0未保存||2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                else if (_formState == Parameter.FormState.未保存)
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value != null && dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView1.Rows[i].ReadOnly = false;
                        else
                            dataGridView1.Rows[i].ReadOnly = true;
                    }
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value != null && dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView1.Rows[i].ReadOnly = false;
                        else
                            dataGridView1.Rows[i].ReadOnly = true;
                    }
                }
            }
            else//操作员
            {
                if (_formState == Parameter.FormState.待审核 || _formState == Parameter.FormState.审核通过) //1待审核||2审核通过
                {
                    //控件都不能点
                    setControlFalse();
                }
                else //0未保存||3审核未通过
                {
                    //发送审核，审核不能点
                    setControlTrue();
                    btn提交数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为未审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value != null && dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                            dataGridView1.Rows[i].ReadOnly = true;
                        else
                            dataGridView1.Rows[i].ReadOnly = false;
                    }
                }
            }
            //datagridview格式，包含序号不可编辑
            setDataGridViewFormat();
        }

        /// <summary>
        /// 设置所有控件可用；
        /// btn审核、btn提交审核两个按钮一直是false；
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
            tb审核员.Enabled = false;

            btn提交数据审核.Enabled = false;
            btn数据审核.Enabled = false;

            //部分空间防作弊，不可改
            tb生产指令编码.ReadOnly = true;
            tb产品批号.ReadOnly = true;
            tb产品代码.ReadOnly = true;
            tb不良品数量.ReadOnly = true;
            tb合格品数量.ReadOnly = true;
            //查询条件始终不可编辑
        }

        /// <summary>
        /// 设置所有控件不可用；
        /// 查看日志、打印、cb打印机始终可用
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
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件
        private void addComputerEventHandler() { }

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID);
            outerBind();
            //MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************// 
            if (dt记录.Rows.Count <= 0)
            {
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

                //内表绑定
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();
                DataRow dr2 = dt记录详情.NewRow();
                dr2 = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr2);
                dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
                setDataGridViewRowNums();
                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
            }
            //内表绑定
            dataGridView1.Columns.Clear();
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();

            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  

        }

        //根据主键显示
        public void IDShow(Int32 ID)
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select * from " + table + " where ID = " + ID.ToString(), mySystem.Parameter.conn);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                DataShow(InstruID);
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID, mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            tb生产指令编码.DataBindings.Clear();
            tb生产指令编码.DataBindings.Add("Text", bs记录.DataSource, "生产指令编码");
            tb产品代码.DataBindings.Clear();
            tb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            tb电压.DataBindings.Clear();
            tb电压.DataBindings.Add("Text", bs记录.DataSource, "电压");
            //各种参数设定
            tb焊线1参数1.DataBindings.Clear();
            tb焊线1参数1.DataBindings.Add("Text", bs记录.DataSource, "焊线1参数1");
            tb焊线1参数2.DataBindings.Clear();
            tb焊线1参数2.DataBindings.Add("Text", bs记录.DataSource, "焊线1参数2");
            tb焊线1参数3.DataBindings.Clear();
            tb焊线1参数3.DataBindings.Add("Text", bs记录.DataSource, "焊线1参数3");
            tb焊线1参数4.DataBindings.Clear();
            tb焊线1参数4.DataBindings.Add("Text", bs记录.DataSource, "焊线1参数4");
            tb焊线1参数5.DataBindings.Clear();
            tb焊线1参数5.DataBindings.Add("Text", bs记录.DataSource, "焊线1参数5");

            tb焊线2参数1.DataBindings.Clear();
            tb焊线2参数1.DataBindings.Add("Text", bs记录.DataSource, "焊线2参数1");
            tb焊线2参数2.DataBindings.Clear();
            tb焊线2参数2.DataBindings.Add("Text", bs记录.DataSource, "焊线2参数2");
            tb焊线2参数3.DataBindings.Clear();
            tb焊线2参数3.DataBindings.Add("Text", bs记录.DataSource, "焊线2参数3");
            tb焊线2参数4.DataBindings.Clear();
            tb焊线2参数4.DataBindings.Add("Text", bs记录.DataSource, "焊线2参数4");
            tb焊线2参数5.DataBindings.Clear();
            tb焊线2参数5.DataBindings.Add("Text", bs记录.DataSource, "焊线2参数5");          

            tb合格品数量.DataBindings.Clear();
            tb合格品数量.DataBindings.Add("Text", bs记录.DataSource, "合格品数量");
            tb不良品数量.DataBindings.Clear();
            tb不良品数量.DataBindings.Add("Text", bs记录.DataSource, "不良品数量");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产指令编码"] = tb生产指令编码.Text;
            dr["产品代码"] = tb产品代码.Text;
            dr["产品批号"] = tb产品批号.Text;

            dr["电压"] = 0;
            dr["焊线1参数1"] = 0;
            dr["焊线1参数2"] = 0;
            dr["焊线1参数3"] = 0;
            dr["焊线1参数4"] = 0;
            dr["焊线1参数5"] = 0;

            dr["焊线2参数1"] = 0;
            dr["焊线2参数2"] = 0;
            dr["焊线2参数3"] = 0;
            dr["焊线2参数4"] = 0;
            dr["焊线2参数5"] = 0;
            dr["电压"] = 0;
            dr["合格品数量"] = 0;
            dr["不良品数量"] = 0;

            dr["审核员"] = "";
            dr["审核是否通过"] = false;
            dr["审核日期"] =DateTime.Now;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + Instruction + "\n";
            dr["日志"] = log;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new SqlDataAdapter("select * from " + tableInfo + " where T圆口焊接机运行记录ID = " + ID.ToString(), mySystem.Parameter.conn);
            cb记录详情 = new SqlCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
            dataGridView1.Columns["审核员"].ReadOnly = true;
        }

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            //DataRow dr = dt记录详情.NewRow();
            dr["T圆口焊接机运行记录ID"] = ID;
            dr["序号"] = 0;
            dr["生产时间"] = DateTime.Now;
            dr["焊线1参数1"] = "√";
            dr["焊线1参数2"] = "√";
            dr["焊线1参数3"] = "√";
            dr["焊线1参数4"] = "√";
            dr["焊线1参数5"] = "√";

            dr["焊线2参数1"] = "√";
            dr["焊线2参数2"] = "√";
            dr["焊线2参数3"] = "√";
            dr["焊线2参数4"] = "√";
            dr["焊线2参数5"] = "√";
            dr["电压"] = "√";
            dr["外观检查"] = "√";
            dr["生产合格品数量"] = 0;
            dr["不良品数量"] = 0;
            dr["操作员"] = mySystem.Parameter.userName;
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
                        //tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = 80;
                        break;
                }
            }
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
            dataGridView1.Columns["T圆口焊接机运行记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;

            dataGridView1.Columns["焊线1参数1"].HeaderText = "焊线1#\rWELDING\rPRESSURE\r(bar)";
            dataGridView1.Columns["焊线1参数2"].HeaderText = "焊线1#\rSEALING\rTEMP\r(°C)";
            dataGridView1.Columns["焊线1参数3"].HeaderText = "焊线1#\rSEALING\rTIME\r(s)";
            dataGridView1.Columns["焊线1参数4"].HeaderText = "焊线1#\rCOOLING\rTEMP\r(°C)";
            dataGridView1.Columns["焊线1参数5"].HeaderText = "焊线1#\rTCR";

            dataGridView1.Columns["焊线2参数1"].HeaderText = "焊线2#\rWELDING\rPRESSURE\r(bar)";
            dataGridView1.Columns["焊线2参数2"].HeaderText = "焊线2#\rSEALING\rTEMP\r(°C)";
            dataGridView1.Columns["焊线2参数3"].HeaderText = "焊线2#\rSEALING\rTIME\r(s)";
            dataGridView1.Columns["焊线2参数4"].HeaderText = "焊线2#\rCOOLING\rTEMP\r(°C)";
            dataGridView1.Columns["焊线2参数5"].HeaderText = "焊线2#\rTCR";

            dataGridView1.Columns["生产合格品数量"].HeaderText = "合格品\r数量\r(只)";
            dataGridView1.Columns["生产合格品数量"].Visible = false;
            dataGridView1.Columns["生产合格品数量"].ReadOnly = true;
            dataGridView1.Columns["不良品数量"].HeaderText = "不良品\r数量\r(只)";
            dataGridView1.Columns["不良品数量"].Visible = false;
            dataGridView1.Columns["不良品数量"].ReadOnly = true;
        }

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

        //******************************按钮功能******************************//
               
        //添加行按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        //删除行按钮
        private void btn删除记录_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                if (dt记录详情.Rows[deletenum]["审核员"].ToString() != "" && _userState==Parameter.UserState.操作员)
                    return;
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
            if (Name_check() == false)
            {
                /*操作员不合格*/
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*环境温度、环境湿度不合格*/
                return false;
            }
            else
            {
                // 内表保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(InstruID);
                outerBind();

                return true;
            }
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
            { return; }
            else if (need提交数据审核())
            {
                MessageBox.Show("需要提交数据审核");
                return;
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='圆口焊接机运行记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "圆口焊接机运行记录";
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
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            mySystem.Other.LogForm logform = new mySystem.Other.LogForm();
            logform.setLog(dt记录.Rows[0]["日志"].ToString()).Show();
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
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='圆口焊接机运行记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
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
        
        //审核按钮
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
                return;
            }
            else if (need数据审核())
            {
                MessageBox.Show("需要数据审核");
                return;
            }
            checkform = new CheckForm(this);
            checkform.Show();
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
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        //打印按钮
        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
        }

        //打印填数据
        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            int ind = 0;
            if (dataGridView1.Rows.Count > 10)
            {
                //在第9行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 10; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[10, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView1.Rows.Count - 10;
            }

            //外表信息
            mysheet.Cells[3, 1].Value = "产品代码/规格：" + dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[3, 8].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 14].Value = "生产指令编码：" + dt记录.Rows[0]["生产指令编码"].ToString();
            //mysheet.Cells[3, 15].Value = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日");

            mysheet.Cells[4, 15].Value = "电压 voltage: " + dt记录.Rows[0]["电压"] + "V";

            mysheet.Cells[8, 3].Value = dt记录.Rows[0]["焊线1参数1"];
            mysheet.Cells[8, 4].Value = dt记录.Rows[0]["焊线1参数2"];
            mysheet.Cells[8, 5].Value = dt记录.Rows[0]["焊线1参数3"];
            mysheet.Cells[8, 6].Value = dt记录.Rows[0]["焊线1参数4"];
            mysheet.Cells[8, 7].Value = dt记录.Rows[0]["焊线1参数5"];

            mysheet.Cells[8, 8].Value = dt记录.Rows[0]["焊线2参数1"];
            mysheet.Cells[8, 9].Value = dt记录.Rows[0]["焊线2参数2"];
            mysheet.Cells[8, 10].Value = dt记录.Rows[0]["焊线2参数3"];
            mysheet.Cells[8, 11].Value = dt记录.Rows[0]["焊线2参数4"];
            mysheet.Cells[8, 12].Value = dt记录.Rows[0]["焊线2参数5"];
            mysheet.Cells[8, 13].Value = dt记录.Rows[0]["电压"];
            //内表信息
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                mysheet.Cells[9 + i, 1] = i + 1;
                mysheet.Cells[9 + i, 2] = Convert.ToDateTime(dt记录详情.Rows[i]["生产时间"].ToString());
                mysheet.Cells[9 + i, 3] = dt记录详情.Rows[i]["焊线1参数1"].ToString();
                mysheet.Cells[9 + i, 4] = dt记录详情.Rows[i]["焊线1参数2"].ToString();
                mysheet.Cells[9 + i, 5] = dt记录详情.Rows[i]["焊线1参数3"].ToString();
                mysheet.Cells[9 + i, 6] = dt记录详情.Rows[i]["焊线1参数4"].ToString();
                mysheet.Cells[9 + i, 7] = dt记录详情.Rows[i]["焊线1参数5"].ToString();
                mysheet.Cells[9 + i, 8] = dt记录详情.Rows[i]["焊线2参数1"].ToString();
                mysheet.Cells[9 + i, 9] = dt记录详情.Rows[i]["焊线2参数2"].ToString();
                mysheet.Cells[9 + i, 10] = dt记录详情.Rows[i]["焊线2参数3"].ToString();
                mysheet.Cells[9 + i, 11] = dt记录详情.Rows[i]["焊线2参数4"].ToString();
                mysheet.Cells[9 + i, 12] = dt记录详情.Rows[i]["焊线2参数5"].ToString();
                mysheet.Cells[9 + i, 13] = dt记录详情.Rows[i]["电压"].ToString();
                mysheet.Cells[9 + i, 14] = dt记录详情.Rows[i]["外观检查"].ToString();
                mysheet.Cells[9 + i, 15] = dt记录详情.Rows[i]["操作员"];
                mysheet.Cells[9 + i, 16] = dt记录详情.Rows[i]["审核员"];
                
                //mysheet.Cells[9 + i, 17] = dt记录详情.Rows[i]["操作员备注"].ToString();

            }


            //mysheet.Cells[19 + ind, 13].Value = string.Format("合格品数量：    {0}只\n不良品数量：    {1}只", dt记录.Rows[0]["合格品数量"], dt记录.Rows[0]["不良品数量"]);
            //mysheet.Cells[19 + ind, 16].Value = string.Format("复核人： {0}\n日期：{1}", dt记录.Rows[0]["审核员"].ToString(), Convert.ToDateTime(dt记录.Rows[0]["审核日期"]).ToString("yyyy年MM月dd日"));
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from " + table + " where 生产指令ID=" + InstruID;
            SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt记录.Rows[0]["ID"]) + 1;

        }

        /// <summary>
        /// this function is responsbily  by pool 2017-11-03
        /// </summary>
        /// <param name="b"></param>
        public int print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\CSBag\SOP-MFG-109-R01A 产品内包装记录.xlsx");
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\PTV\9 SOP-MFG-413-R01A 圆口焊接机运行记录.xlsx");

            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];
            // 修改Sheet中某行某列的值
            fill_excel(my, wb);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = Instruction + "-" + find_indexofprint().ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码


            if (b)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
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
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
                return pageCount;
            }
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
                if (Int32.TryParse(dt记录详情.Rows[i]["生产合格品数量"].ToString(), out numtemp) == true)
                { sum[0] += numtemp; }
            }
            dt记录.Rows[0]["合格品数量"] = sum[0];
            outerDataSync("tb合格品数量", sum[0].ToString());
            // 膜卷重量求和
            sum[1] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["外观检查"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            dt记录.Rows[0]["不良品数量"] = sum[1];
            outerDataSync("tb不良品数量", sum[1].ToString());
        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] {
                tb焊线1参数1,tb焊线1参数2,tb焊线1参数3,tb焊线1参数4,tb焊线1参数5,
                tb焊线2参数1,tb焊线2参数2,tb焊线2参数3,tb焊线2参数4,tb焊线2参数5});
            List<String> StringList = new List<String>(new String[] {
                "焊线1#  WELDING PRESSURE(bar)","焊线1#  SEALING TEMP(°C)","焊线1#  SEALING TIME(s)","焊线1#  COOLING TEMP(°C)","焊线1#  TCR",
                "焊线2#  WELDING PRESSURE(bar)","焊线2#  SEALING TEMP(°C)","焊线2#  SEALING TIME(s)","焊线2#  COOLING TEMP(°C)","焊线2#  TCR"});
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

        // 检查 操作员的姓名
        private bool Name_check()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["操作员"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["操作员"] = mySystem.Parameter.userName;
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的『操作员』信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
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
                if (dataGridView1.Columns[e.ColumnIndex].Name == "生产合格品数量")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "外观检查")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "操作员")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
                    }
                }
                else
                { }
            }
        }

        //数据绑定结束，设置表格格式
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //审核完成的行不能修改，包括审核员，但审核员可以删除改行
            setEnableReadOnly();

            setDataGridViewFormat();
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        //内表提交审核按钮
        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            //find the uncheck item in inner list and tag the revoewer __待审核
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Convert.ToString(dt记录详情.Rows[i]["审核员"]).ToString().Trim() == "")
                {
                    dt记录详情.Rows[i]["审核员"] = "__待审核";
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();
            setEnableReadOnly();
        }

        //内标审核按钮 //this function just fill the name but dooesn't catch the opinion
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            HashSet<Int32> hi待审核行号 = new HashSet<int>();
            foreach (DataGridViewCell dgvc in dataGridView1.SelectedCells)
            {
                hi待审核行号.Add(dgvc.RowIndex);
            }
            //find the item in inner tagged the reviewer __待审核 and replace the content his name
            foreach (int i in hi待审核行号)
            {
                if ("__待审核" == Convert.ToString(dt记录详情.Rows[i]["审核员"]).ToString().Trim())
                {
                    if (Parameter.userName != dt记录详情.Rows[i]["操作员"].ToString())
                    {
                        dt记录详情.Rows[i]["审核员"] = Parameter.userName;
                    }
                    else
                    {
                        MessageBox.Show("记录员,审核员相同");
                    }
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();
        }
        //need提交数据审核
        private bool need提交数据审核()
        {
            bool rtn = false;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    rtn = true;
                }
            }
            return rtn;
        }

        //
        private bool need数据审核()
        {
            bool rtn = false;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    rtn = true;
                }
            }
            return rtn;
        }

        private void PTVBag_runningrecordofyk_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView1);
        }
        
    }
}





