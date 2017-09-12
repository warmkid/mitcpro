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

namespace mySystem.Process.Bag.BTV
{
    public partial class BPV生产退料记录 : mySystem.BaseForm
    {
        private String table = "生产退料记录表";
        private String tableInfo = "生产退料记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private Boolean isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt生产指令, dt生产指令详情, dt生产指令物料, dt物料简称批号代码;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        String Flight = "";

        public BPV生产退料记录(mySystem.MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.bpvbagInstruID;
            Instruction = Parameter.bpvbagInstruction;
            Flight = Parameter.userflight;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            //查询条件可编辑
            cb产品代码.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
        }

        public BPV生产退料记录(mySystem.MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

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
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='生产退料记录表'", connOle);
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

        //读取设置内容  
        private void getOtherData()
        {
            GetProductInfo(Instruction);

            //*********数据填写*********//
            if (dt生产指令详情.Rows.Count == 0)
            { MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！"); }
            else if (dt生产指令物料.Rows.Count == 0)
            { MessageBox.Show("该生产指令编码下的『生产指令物料』尚未生成！"); }
            else
            {
                for (int i = 0; i < dt生产指令详情.Rows.Count; i++)
                {
                    cb产品代码.Items.Add(dt生产指令详情.Rows[i][1].ToString());//添加
                }
            }

            cb产品代码.SelectedIndex = -1;
        }

        //GetProductInfo //产品代码、产品批号初始化
        private void GetProductInfo(String InsCode)
        {

            dt生产指令 = new DataTable("生产指令");
            dt生产指令详情 = new DataTable("生产指令详情");
            dt生产指令物料 = new DataTable("生产指令物料");
            dt物料简称批号代码 = new DataTable("物料简称批号代码");
            dt物料简称批号代码.Columns.Add("物料简称", Type.GetType("System.String"));
            dt物料简称批号代码.Columns.Add("物料代码", Type.GetType("System.String"));
            dt物料简称批号代码.Columns.Add("物料批号", Type.GetType("System.String"));

            if (!isSqlOk)
            {
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令 where 生产指令编号 = '" + InsCode + "' ";//这里应有生产指令编码
                OleDbDataAdapter datemp1 = new OleDbDataAdapter(comm1);
                datemp1.Fill(dt生产指令);

                //OleDbDataReader reader1 = comm1.ExecuteReader();
                //if (reader1.Read())
                if (dt生产指令.Rows.Count > 0)
                {
                    //添加物料简称、批号、代码
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["内包物料名称1"], dt生产指令.Rows[0]["内包物料代码1"], dt生产指令.Rows[0]["内包物料批号1"] });
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["内包物料名称2"], dt生产指令.Rows[0]["内包物料代码2"], dt生产指令.Rows[0]["内包物料批号2"] });
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["内包物料名称3"], dt生产指令.Rows[0]["内包物料代码3"], dt生产指令.Rows[0]["内包物料批号3"] });
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["外包物料名称1"], dt生产指令.Rows[0]["外包物料代码1"], dt生产指令.Rows[0]["外包物料批号1"] });
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["外包物料名称2"], dt生产指令.Rows[0]["外包物料代码2"], dt生产指令.Rows[0]["外包物料批号2"] });
                    dt物料简称批号代码.Rows.Add(new object[] { dt生产指令.Rows[0]["外包物料名称3"], dt生产指令.Rows[0]["外包物料代码3"], dt生产指令.Rows[0]["外包物料批号3"] });

                    //读取生产指令详细信息
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    //comm2.CommandText = "select ID, 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID = " + reader1["ID"].ToString();
                    comm2.CommandText = "select ID, 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID = " + dt生产指令.Rows[0]["ID"].ToString();
                    OleDbDataAdapter datemp2 = new OleDbDataAdapter(comm2);
                    datemp2.Fill(dt生产指令详情);
                    datemp2.Dispose();

                    //读取生产指令物料
                    OleDbCommand comm3 = new OleDbCommand();
                    comm3.Connection = Parameter.connOle;
                    //comm2.CommandText = "select ID, 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID = " + reader1["ID"].ToString();
                    comm3.CommandText = "select * from 生产指令物料 where T生产指令ID = " + dt生产指令.Rows[0]["ID"].ToString();
                    OleDbDataAdapter datemp3 = new OleDbDataAdapter(comm3);
                    datemp3.Fill(dt生产指令物料);
                    datemp3.Dispose();
                    for (int i = 0; i < dt生产指令物料.Rows.Count; i++)
                    { dt物料简称批号代码.Rows.Add(new object[] { dt生产指令物料.Rows[i]["物料名称"], dt生产指令物料.Rows[i]["物料代码"], dt生产指令物料.Rows[i]["物料批号"] }); }
                }
                else
                {
                    //添加物料简称、批号、代码
                    dt物料简称批号代码.Rows.Add(new object[] { "暂无", "暂无", "暂无" });
                    //dt代码批号为空
                    MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
                }
                //reader1.Dispose();
                datemp1.Dispose();
            }
            else
            { }
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
                if (_formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.审核未通过)  //2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                else if (_formState == Parameter.FormState.未保存)//0未保存
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
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
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
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
                else if (_formState == Parameter.FormState.未保存) //0未保存
                {
                    //发送审核，审核不能点
                    setControlTrue();
                    btn提交数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为未审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                            dataGridView1.Rows[i].ReadOnly = true;
                        else
                            dataGridView1.Rows[i].ReadOnly = false;
                    }
                }
                else //3审核未通过
                {
                    //发送审核，审核不能点
                    setControlTrue();
                    btn提交数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为未审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                            dataGridView1.Rows[i].ReadOnly = true;
                        else
                            dataGridView1.Rows[i].ReadOnly = false;
                    }
                }
            }
            //datagridview格式，包含序号不可编辑
            setDataGridViewFormat();
        }

        // 设置所有控件可用
        private void setControlTrue()
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
            btn数据审核.Enabled = false;
            btn提交数据审核.Enabled = false;
            tb审核员.Enabled = false;
            //部分空间防作弊，不可改
            //查询条件始终不可编辑
            cb产品代码.Enabled = false;
            btn查询新建.Enabled = false;
        }

        // 设置所有控件不可用
        private void setControlFalse()
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
        private void DataShow(Int32 InstruID, String productCode)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productCode);
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
                readOuterData(InstruID, productCode);
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
            OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + table + " where ID = " + ID.ToString(), connOle);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                Instruction = dt1.Rows[0]["生产指令编号"].ToString();
                GetProductInfo(Instruction);

                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品代码"].ToString());
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;

            lb生产指令编号.DataBindings.Clear();
            lb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            cb产品代码.DataBindings.Clear();
            cb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            lb产品批号.DataBindings.Clear();
            lb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");

            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产指令编号"] = Instruction;
            dr["产品代码"] = cb产品代码.Text;
            dr["产品批号"] = dt生产指令详情.Rows[cb产品代码.FindString(cb产品代码.Text)]["产品批号"].ToString();
            dr["备注"] = "物料单位---膜材用米，管用米，其余物料用个。";
            dr["审核员"] = "";
            dr["审核是否通过"] = false;
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
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T生产退料记录ID = " + ID.ToString(), connOle);
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
            dr["T生产退料记录ID"] = ID;
            dr["序号"] = 0;
            dr["领料日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["时间"] = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));
            dr["班次"] = Flight == "" ? "白班" : Flight;
            dr["物料简称"] = dt物料简称批号代码.Rows[0]["物料简称"].ToString();
            dr["物料代码"] = dt物料简称批号代码.Rows[0]["物料代码"].ToString();
            dr["物料批号"] = dt物料简称批号代码.Rows[0]["物料批号"].ToString();
            dr["退库数量"] = 0;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["审核员"] = "";
            dr["操作员备注"] = "";
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
                    case "物料简称":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        if (dt物料简称批号代码 != null)
                        {
                            for (int i = 0; i < dt物料简称批号代码.Rows.Count; i++)
                            { cbc.Items.Add(dt物料简称批号代码.Rows[i]["物料简称"]); }
                        }   
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 120;
                        break;
                    case "班次":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("白班");
                        cbc.Items.Add("夜班"); 
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
            dataGridView1.Columns["T生产退料记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["物料代码"].ReadOnly = true;
            dataGridView1.Columns["物料批号"].ReadOnly = true;
            dataGridView1.Columns["物料简称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DataShow(InstruID, cb产品代码.Text); } 
        }

        //添加行按钮
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
        }

        //删除按钮
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

                setDataGridViewRowNums();
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
            else
            {
                // 内表保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(InstruID, cb产品代码.Text);
                outerBind();

                setEnableReadOnly();

                return true;
            }
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='生产退料记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "生产退料记录表";
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
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
                return;
            }
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='生产退料记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
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


            if (checkform.ischeckOk)
            {
                // 退料入库
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + Convert.ToInt32(dt记录.Rows[0]["生产指令ID"]), mySystem.Parameter.connOle);
                DataTable dt = new DataTable();
                OleDbCommandBuilder cb;
                da.Fill(dt);
                string 订单号 = dt.Rows[0]["客户或订单号"].ToString();
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection Tconn;
                Tconn = new OleDbConnection(strConnect);
                Tconn.Open();
                foreach (DataRow dr in dt记录详情.Rows)
                {
                    string 代码 = dr["物料代码"].ToString();
                    string 批号 = dr["物料批号"].ToString();
                    double 退库数量 = Convert.ToDouble(dr["退库数量"]);
                    string sql = "select * from 库存台帐 where 产品代码='{0}' and 产品批号='{1}' and 用途='{2}' and 状态='合格'";
                    da = new OleDbDataAdapter(string.Format(sql, 代码, 批号, 订单号), Tconn);
                    cb = new OleDbCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("原料:" + 代码 + ",批号：" + 批号 + "退库失败");
                        return;
                    }
                    else
                    {
                        dt.Rows[0]["现存数量"] = 退库数量 + Convert.ToDouble(dt.Rows[0]["现存数量"]);
                        da.Update(dt);
                    }
                }
            }

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

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\SOP-MFG-102-R02A 生产退料记录.xlsx");
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
            mysheet.Cells[3, 1].Value = "产品代码：" + dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[3, 6].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 9].Value = "生产指令编号：" + dt记录.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[15, 1].Value = "备注：" + dt记录.Rows[0]["备注"].ToString();
            mysheet.Cells[16, 1].Value = "审核员：" + dt记录.Rows[0]["审核员"].ToString();
            //内表信息
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 9 ? 9 : rownum); i++)
            {
                mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["领料日期"].ToString()).ToString("yyyy/MM/dd");
                mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["时间"].ToString()).ToString("HH:mm:ss");
                mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["班次"].ToString();
                mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["物料简称"].ToString();
                mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["物料批号"].ToString();
                mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["退库数量"].ToString();
                mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["审核员"].ToString();
                mysheet.Cells[5 + i, 11].Value = dt记录详情.Rows[i]["操作员备注"].ToString();
            }
            //需要插入的部分
            if (rownum > 9)
            {
                for (int i = 9; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[5 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                    mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["领料日期"].ToString()).ToString("yyyy/MM/dd");
                    mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["时间"].ToString()).ToString("HH:mm:ss");
                    mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["班次"].ToString();
                    mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["物料简称"].ToString();
                    mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                    mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["物料批号"].ToString();
                    mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["退库数量"].ToString();
                    mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["审核员"].ToString();
                    mysheet.Cells[5 + i, 11].Value = dt记录详情.Rows[i]["操作员备注"].ToString();
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
                if (dataGridView1.Columns[e.ColumnIndex].Name == "物料简称")
                {
                    DataRow[] rows = dt物料简称批号代码.Select("物料简称 = '" + dt记录详情.Rows[e.RowIndex]["物料简称"].ToString() + "'"); 
                    dt记录详情.Rows[e.RowIndex]["物料代码"] = rows[0]["物料代码"];
                    dt记录详情.Rows[e.RowIndex]["物料批号"] = rows[0]["物料批号"];
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
           
    }
}
