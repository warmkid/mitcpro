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
using System.Text.RegularExpressions;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionpRoductionAndRestRecordStep6 : BaseForm
    {
        bool isSaveClicked = false;
        private String table = "吹膜工序生产和检验记录";
        private String tableInfo = "吹膜工序生产和检验记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private int[] sum = { 0, 0 };

        private DataTable dt记录, dt记录详情, dt代码批号, dt工艺设备;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        #region
        //private string person_操作员;
        //private string person_审核员;
        ///// <summary>
        ///// 登录人状态，0 操作员， 1 审核员， 2管理员
        ///// </summary>
        //private int stat_user;
        ///// <summary>
        ///// 窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        ///// </summary>
        //private int stat_form;
        #endregion

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;  

        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.proInstruID;
            Instruction = Parameter.proInstruction;
            cb白班.Checked = Parameter.userflight == "白班" ? true : false; //生产班次的初始化？？？？？
            cb夜班.Checked = !cb白班.Checked;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            cb产品名称.Enabled = true;
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
        }

        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();
            isSaveClicked = true;
            conn = Parameter.conn;
            connOle = Parameter.connOle;
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
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜生产和检验记录表'", connOle);
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
            string s = dt记录.Rows[0]["审核人"].ToString();
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
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
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
                if (_formState == Parameter.FormState.未保存 || _formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.审核未通过)  //0未保存||2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;
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
            tb审核人.Enabled = false;
            //部分空间防作弊，不可改
            tb产品批号.ReadOnly = true;
            tb生产设备.ReadOnly = true;
            tb依据工艺.ReadOnly = true;
            cb白班.Enabled = false;
            cb夜班.Enabled = false;
            tb累计同规格膜卷长度R.ReadOnly = true;
            tb累计同规格膜卷重量T.ReadOnly = true;
            //查询条件始终不可编辑
            cb产品名称.Enabled = false;
            dtp生产日期.Enabled = false;
            btn查询新建.Enabled = false;
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

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID, String productName, Boolean flight, DateTime searchTime)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productName, flight, searchTime);
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
                readOuterData(InstruID, productName, flight, searchTime);
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
            else
            {
                isSaveClicked = true;
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
            OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + table + " where ID = " + ID.ToString(), connOle);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cb白班.Checked = Convert.ToBoolean(dt1.Rows[0]["班次"].ToString());
                cb夜班.Checked = !cb白班.Checked;
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品名称"].ToString(), Convert.ToBoolean(dt1.Rows[0]["班次"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            }
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
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
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
            dr["生产指令ID"] = InstruID;
            dr["产品名称"] = cb产品名称.Text;
            dr["产品批号"] = dt代码批号.Rows[cb产品名称.FindString(cb产品名称.Text)]["产品批号"].ToString();
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["班次"] = cb白班.Checked;
            dr["依据工艺"] = dt工艺设备.Rows[0]["依据工艺"];
            dr["生产设备"] = dt工艺设备.Rows[0]["生产设备"];
            dr["审核人"] = "";
            dr["审核是否通过"] = false;
            dr["累计同规格膜卷长度R"] = 0;
            dr["累计同规格膜卷重量T"] = 0;            
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
            { 
                //根据上一行填写
                dr["开始时间"] = dt记录详情.Rows[dt记录详情.Rows.Count - 1]["结束时间"];
                int numtemp = 0;
                if (Int32.TryParse(dt记录详情.Rows[dt记录详情.Rows.Count - 1]["膜卷编号"].ToString(), out numtemp) == true)
                {
                    dr["膜卷编号"] = (numtemp + 1).ToString("D2");
                }
                else
                { dr["膜卷编号"] = ""; }
            }
            else
            {
                //新建第一行
                dr["开始时间"] = DateTime.Now;
                dr["膜卷编号"] = "01";
            }            
            dr["结束时间"] = DateTime.Now;
            dr["膜卷长度"] = 0;
            dr["膜卷重量"] = 0;
            dr["外观"] = "Yes";
            dr["宽度"] = 0;
            dr["最大厚度"] = 0;
            dr["最小厚度"] = 0;
            dr["平均厚度"] = 0;
            dr["厚度公差"] = 0;
            dr["判定"] = "Yes";
            dr["操作员"] = mySystem.Parameter.userName;

            dr["操作员备注"] = "无";
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
            dataGridView1.Columns["操作员备注"].Visible = false;
            dataGridView1.Columns["T吹膜工序生产和检验记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["膜卷编号"].HeaderText = "膜卷编号\r(卷)";
            dataGridView1.Columns["膜卷长度"].HeaderText = "膜卷长度\r(m)";
            dataGridView1.Columns["膜卷重量"].HeaderText = "膜卷重量\r(kg)";
            dataGridView1.Columns["宽度"].HeaderText = "宽度\r(mm)";
            dataGridView1.Columns["最大厚度"].HeaderText = "最大厚度\r(μm)";
            dataGridView1.Columns["最小厚度"].HeaderText = "最小厚度\r(μm)";
            dataGridView1.Columns["平均厚度"].HeaderText = "平均厚度\r(μm)";
            dataGridView1.Columns["厚度公差"].HeaderText = "厚度公差\r(%)";
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (cb产品名称.SelectedIndex >= 0)
            { DataShow(InstruID, cb产品名称.Text, cb白班.Checked, dtp生产日期.Value); }
        }

        //添加行按钮
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();

            //if (mySystem.Parameter.NametoID(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["操作员"].Value.ToString()) == 0)
            //{
            //    dt记录详情.Rows[dataGridView1.Rows.Count - 1]["操作员"] = "";
            //    MessageBox.Show("请重新输入最新建行的『操作员』信息", "ERROR");
            //}
            //else
            //{
            //    DialogResult dr1 = MessageBox.Show("新建行后，上面的信息将不可修改，确认新建吗？", "提示", MessageBoxButtons.OKCancel);
            //    if (dr1 == DialogResult.OK)
            //    {
            //        //用户选择确认的操作
            //        //MessageBox.Show("您选择的是【确认】");
            //        dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;
            //        DataRow dr2 = dt记录详情.NewRow();
            //        dr2 = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"].ToString()), dr2);
            //        dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
            //        setDataGridViewRowNums();
            //    }
            //    else if (dr1 == DialogResult.Cancel)
            //    {
            //        //用户选择取消的操作
            //        //MessageBox.Show("您选择的是【取消】");
            //    }
            //}                    
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }
        
        //删除行按钮
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
            isSaveClicked = true;
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
                btn提交审核.Enabled = true;
            try { (this.Owner as ExtructionMainForm).InitBtn(); }
            catch (NullReferenceException exp)
            {  }            
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
                readOuterData(InstruID, cb产品名称.Text, cb白班.Checked, dtp生产日期.Value);
                outerBind();

                return true;
            }    
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("确认本表已经填完吗？提交审核之后不可修改", "提示", MessageBoxButtons.YesNo))
            {
                //保存
                bool isSaved = Save();
                if (isSaved == false)
                    return;

                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜生产和检验记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜生产和检验记录表";
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
                dt记录.Rows[0]["审核人"] = "__待审核";

                Save();
                _formState = Parameter.FormState.待审核;
                setEnableReadOnly();
            }
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

            dt记录.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜生产和检验记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
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

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
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
        private void printBtn_Click(object sender, EventArgs e)
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

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R09 吹膜工序生产和检验记录.xlsx");
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
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 10 ? 10 : rownum); i++)
            {
                mysheet.Cells[8 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["开始时间"].ToString()).ToString("HH:mm") + " ~ " + Convert.ToDateTime(dt记录详情.Rows[i]["结束时间"].ToString()).ToString("HH:mm");
                mysheet.Cells[8 + i, 3].Value = dt记录详情.Rows[i]["膜卷编号"].ToString();
                mysheet.Cells[8 + i, 4].Value = dt记录详情.Rows[i]["膜卷长度"].ToString();
                mysheet.Cells[8 + i, 5].Value = dt记录详情.Rows[i]["膜卷重量"].ToString();
                mysheet.Cells[8 + i, 6].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[8 + i, 7].Value = dt记录详情.Rows[i]["外观"].ToString() == "Yes" ? "√" : "×";
                mysheet.Cells[8 + i, 8].Value = dt记录详情.Rows[i]["宽度"].ToString();
                mysheet.Cells[8 + i, 9].Value = dt记录详情.Rows[i]["最大厚度"].ToString();
                mysheet.Cells[8 + i, 10].Value = dt记录详情.Rows[i]["最小厚度"].ToString();
                mysheet.Cells[8 + i, 11].Value = dt记录详情.Rows[i]["平均厚度"].ToString();
                mysheet.Cells[8 + i, 12].Value = dt记录详情.Rows[i]["厚度公差"].ToString();
                mysheet.Cells[8 + i, 13].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[8 + i, 14].Value = dt记录详情.Rows[i]["判定"].ToString() == "Yes" ? "√" : "×";
            }
            //需要插入的部分
            if (rownum > 10)
            {
                for (int i = 10; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[8+i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[8 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                    mysheet.Cells[8 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["开始时间"].ToString()).ToString("HH:mm") + " ~ " + Convert.ToDateTime(dt记录详情.Rows[i]["结束时间"].ToString()).ToString("HH:mm");
                    mysheet.Cells[8 + i, 3].Value = dt记录详情.Rows[i]["膜卷编号"].ToString();
                    mysheet.Cells[8 + i, 4].Value = dt记录详情.Rows[i]["膜卷长度"].ToString();
                    mysheet.Cells[8 + i, 5].Value = dt记录详情.Rows[i]["膜卷重量"].ToString();
                    mysheet.Cells[8 + i, 6].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[8 + i, 7].Value = dt记录详情.Rows[i]["外观"].ToString() == "Yes" ? "√" : "×";
                    mysheet.Cells[8 + i, 8].Value = dt记录详情.Rows[i]["宽度"].ToString();
                    mysheet.Cells[8 + i, 9].Value = dt记录详情.Rows[i]["最大厚度"].ToString();
                    mysheet.Cells[8 + i, 10].Value = dt记录详情.Rows[i]["最小厚度"].ToString();
                    mysheet.Cells[8 + i, 11].Value = dt记录详情.Rows[i]["平均厚度"].ToString();
                    mysheet.Cells[8 + i, 12].Value = dt记录详情.Rows[i]["厚度公差"].ToString();
                    mysheet.Cells[8 + i, 13].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[8 + i, 14].Value = dt记录详情.Rows[i]["判定"].ToString() == "Yes" ? "√" : "×";
                }
            }
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from " + table + " where 生产指令ID=" + InstruID.ToString(), connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            mysheet.PageSetup.RightFooter = Instruction + "-09-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
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
            //dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            outerDataSync("tb累计同规格膜卷长度R", sum[0].ToString());
            // 膜卷重量求和
            double numtemp2;
            sum[1] = 0;
            double sum2 = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Double.TryParse(dt记录详情.Rows[i]["膜卷重量"].ToString(), out numtemp2) == true)
                { sum2 += numtemp2; }
            }
            //dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];
            outerDataSync("tb累计同规格膜卷重量T", sum2.ToString());        
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
                if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷长度")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷重量")
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
            setDataGridViewFormat();
        }

        private void ExtructionpRoductionAndRestRecordStep6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaveClicked && dt记录.Rows.Count > 0)
            {
                dt记录.Rows[0].Delete();
                da记录.Update((DataTable)bs记录.DataSource);
            }
        }

    

        //private void setOtherEventHandler()
        //{
        //    tb产品批号.TextChanged += new EventHandler(tb产品批号_TextChanged);
        //    tb环境湿度.TextChanged += new EventHandler(tb产品批号_TextChanged);
        //}

        //void tb产品批号_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Int16.Parse(((TextBox)sender).Text);
        //    }
        //    catch { }
        //}
    }
}
