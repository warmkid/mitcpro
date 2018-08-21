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
    public partial class LDPE产品外包装记录 : mySystem.BaseForm
    {        
        private String table = "产品外包装记录表";
        private String tableInfo = "产品外包装详细记录";

        private SqlConnection conn = null;
        //private OleDbConnection mySystem.Parameter.conn = null;
        private Boolean isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt生产指令, dt生产指令详情;
        private SqlDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private SqlCommandBuilder cb记录, cb记录详情;
        
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        bool isFirstBind = true;

        public LDPE产品外包装记录(mySystem.MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.ldpebagInstruID;
            Instruction = Parameter.ldpebagInstruction;

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

        public LDPE产品外包装记录(mySystem.MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
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
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='产品外包装记录表'", mySystem.Parameter.conn);
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

        //读取设置内容  //GetProductInfo //产品代码、产品批号初始化
        private void getOtherData()
        {
            dt生产指令 = new DataTable("生产指令");
            dt生产指令详情 = new DataTable("生产指令详情");

            if (isSqlOk)
            {
                SqlCommand comm1 = new SqlCommand();
                comm1.Connection = mySystem.Parameter.conn;
                comm1.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
                SqlDataReader reader1 = comm1.ExecuteReader();

                if (reader1.Read())
                {
                    //外面的信息填写
                    lb纸箱代码.Text = reader1["外包物料代码2"].ToString();
                    tb纸箱批号.Text = reader1["外包物料批号2"].ToString();
                    lb标签代码.Text = reader1["外包物料代码1"].ToString();
                    //读取详细信息
                    SqlCommand comm2 = new SqlCommand();
                    comm2.Connection = mySystem.Parameter.conn;
                    comm2.CommandText = "select ID, 产品代码, 产品批号, 外标签 from 生产指令详细信息 where T生产指令ID = " + reader1["ID"].ToString();

                    SqlDataAdapter datemp = new SqlDataAdapter(comm2);
                    datemp.Fill(dt生产指令详情);
                    if (dt生产指令详情.Rows.Count == 0)
                    {
                        MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！");
                    }
                    else
                    {
                        for (int i = 0; i < dt生产指令详情.Rows.Count; i++)
                        {
                            cb产品代码.Items.Add(dt生产指令详情.Rows[i][1].ToString());//添加
                        }
                    }
                    datemp.Dispose();
                }
                else
                {
                    //dt代码批号为空
                    MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
                }
                reader1.Dispose();
            }
            else
            { }

            //*********数据填写*********//
            cb产品代码.SelectedIndex = -1;
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
                btn数据审核.Enabled = true;
                btn退回数据审核.Enabled = true;
                if (_formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.待审核)
                {
                    btn退回数据审核.Enabled = false;
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
            btn退回数据审核.Enabled = false;
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
            SqlDataAdapter da1 = new SqlDataAdapter("select * from " + table + " where ID = " + ID.ToString(), mySystem.Parameter.conn);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                Instruction = dt1.Rows[0]["生产指令编号"].ToString();

                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品代码"].ToString());
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' ", mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
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

            lb纸箱代码.DataBindings.Clear();
            lb纸箱代码.DataBindings.Add("Text", bs记录.DataSource, "纸箱代码");
            tb纸箱批号.DataBindings.Clear();
            tb纸箱批号.DataBindings.Add("Text", bs记录.DataSource, "纸箱批号");
            tb领用数量.DataBindings.Clear();
            tb领用数量.DataBindings.Add("Text", bs记录.DataSource, "领用数量");
            tb退库数量.DataBindings.Clear();
            tb退库数量.DataBindings.Add("Text", bs记录.DataSource, "退库数量");

            lb外包标签.DataBindings.Clear();
            lb外包标签.DataBindings.Add("Text", bs记录.DataSource, "外包标签");
            lb标签代码.DataBindings.Clear();
            lb标签代码.DataBindings.Add("Text", bs记录.DataSource, "标签代码");
            tb标签领用数量.DataBindings.Clear();
            tb标签领用数量.DataBindings.Add("Text", bs记录.DataSource, "标签领用数量");

            tb包装规格每包千克.DataBindings.Clear();
            tb包装规格每包千克.DataBindings.Add("Text", bs记录.DataSource, "包装规格每包千克");
            tb包装规格每箱千克.DataBindings.Clear();
            tb包装规格每箱千克.DataBindings.Add("Text", bs记录.DataSource, "包装规格每箱千克");
            tb包装规格每箱只数.DataBindings.Clear();
            tb包装规格每箱只数.DataBindings.Add("Text", bs记录.DataSource, "包装规格每箱只数");

            lb包装数量箱数合计.DataBindings.Clear();
            lb包装数量箱数合计.DataBindings.Add("Text", bs记录.DataSource, "包装数量箱数合计");
            lb产品数量只数合计.DataBindings.Clear();
            lb产品数量只数合计.DataBindings.Add("Text", bs记录.DataSource, "产品数量只数合计");
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
            dr["纸箱代码"] = lb纸箱代码.Text;
            dr["纸箱批号"] = tb纸箱批号.Text;
            dr["领用数量"] = 0;
            dr["退库数量"] = 0;
            dr["外包标签"] = dt生产指令详情.Rows[cb产品代码.FindString(cb产品代码.Text)]["外标签"].ToString();
            dr["标签代码"] = lb标签代码.Text;
            dr["标签领用数量"] = 0;
            dr["包装规格每包千克"] = 0;
            dr["包装规格每箱千克"] = 0;
            dr["包装规格每箱只数"] = 0;
            dr["包装数量箱数合计"] = 0;
            dr["产品数量只数合计"] = 0;
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
            da记录详情 = new SqlDataAdapter("select * from " + tableInfo + " where T产品外包装记录ID = " + ID.ToString(), mySystem.Parameter.conn);
            cb记录详情 = new SqlCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
            //Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            //DataRow dr = dt记录详情.NewRow();
            dr["T产品外包装记录ID"] = ID;
            dr["序号"] = 0;
            dr["包装日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["时间"] = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));
            dr["包装箱号"] = 0;
            dr["包装明细"] = "";
            dr["包装数量箱数"] = 0;
            dr["产品数量只数"] = 0;
            dr["是否贴标签"] = "Yes";
            dr["是否打包封箱"] = "Yes";
            dr["操作员"] = mySystem.Parameter.userName;
            dr["审核员"] = "";
            dr["备注"] = "";
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
                    case "是否贴标签":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("Yes");
                        cbc.Items.Add("No");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    case "是否打包封箱":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("Yes");
                        cbc.Items.Add("No");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    default:
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //tbc.MinimumWidth = 120;
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
            //dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T产品外包装记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            //dataGridView1.Columns["包装明细"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //HeaderText
            dataGridView1.Columns["包装数量箱数"].HeaderText = "包装数量\r（箱）";
            dataGridView1.Columns["产品数量只数"].HeaderText = "产品数量\r（只）";
            dataGridView1.Columns["是否贴标签"].HeaderText = "贴标签\r（Y/N）";
            dataGridView1.Columns["是否打包封箱"].HeaderText = "打包封箱\r（Y/N）";
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
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
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
                getTotal();
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
            if (DialogResult.Yes != MessageBox.Show("提交最后审核后本表单数据不可修改，是否确定？", "提示", MessageBoxButtons.YesNo))
            {
                return;
            }
            //判断内表是否完全提交审核
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    MessageBox.Show("第" + (i + 1) + "行未提交数据审核");
                    return;
                }
            }

            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='产品外包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "产品外包装记录表";
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
            if (!Utility.check内表审核是否完成(dt记录详情)) return;
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
                return;
            }
            checkform = new CheckForm(this);
            checkform.ShowDialog();
        }

        //审核功能
        public override void CheckResult()
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            base.CheckResult();

            if (checkform.ischeckOk)
            {
                // 产品入库
                SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + Convert.ToInt32(dt记录.Rows[0]["生产指令ID"]),mySystem.Parameter.conn);
                DataTable dt = new DataTable();
                SqlCommandBuilder cb;
                da.Fill(dt);
                string 订单号 = dt.Rows[0]["客户或订单号"].ToString();
                string strConnect = "server=" + Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection Tconn;
                Tconn = new SqlConnection(strConnect);
                Tconn.Open();
                string sql = "select * from 设置存货档案 where 存货代码='{0}'";
                da = new SqlDataAdapter(string.Format(sql, dt记录.Rows[0]["产品代码"].ToString()), Tconn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("在存货档案中没有找到代码为:" + dt记录.Rows[0]["产品代码"].ToString() + " 的产品");
                    return;
                }
                string 名称 = dt.Rows[0]["存货名称"].ToString();
                string 产品规格 = dt.Rows[0]["规格型号"].ToString();
                string 主计量单位 = dt.Rows[0]["主计量单位名称"].ToString();


                sql = "select * from 库存台帐 where 产品代码='{0}' and 用途='{1}' and 状态='合格'";
                da = new SqlDataAdapter(string.Format(sql, dt记录.Rows[0]["产品代码"].ToString(), 订单号),Tconn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["产品代码"] = dt记录.Rows[0]["产品代码"].ToString();
                    dr["产品名称"] = 名称;
                    dr["产品规格"] = 产品规格;
                    dr["产品批号"] = dt记录.Rows[0]["产品批号"];
                    dr["现存数量"] = Convert.ToDouble(dt记录.Rows[0]["产品数量只数合计"]);
                    dr["主计量单位"] = 主计量单位;
                    dr["状态"] = "合格";
                    dr["用途"] = 订单号;
                    dt.Rows.Add(dr);
                }
                else
                {
                    dt.Rows[0]["现存数量"] = Convert.ToDouble(dt记录.Rows[0]["产品数量只数合计"]) + Convert.ToDouble(dt.Rows[0]["现存数量"]);
                }
                da.Update(dt);
            }

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='产品外包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
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
        public int print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + 
                @"\..\..\xls\LDPE\SOP-MFG-111-R01A 产品外包装记录.xlsx");
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
                return 0;
            }
            else
            {
                int pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
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
                return pageCount;
            }
        }

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[3, 1].Value = "生产指令编号：" + dt记录.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[3, 5].Value = "纸箱代码：" + dt记录.Rows[0]["纸箱代码"].ToString();
            mysheet.Cells[3, 9].Value = "外包标签：" + dt记录.Rows[0]["外包标签"].ToString();
            mysheet.Cells[3, 12].Value = dt记录.Rows[0]["包装规格每包千克"].ToString() + " Kg/包";
            mysheet.Cells[4, 1].Value = "产品代码：" + dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[4, 5].Value = "纸箱批号：" + dt记录.Rows[0]["纸箱批号"].ToString();
            mysheet.Cells[4, 9].Value = "标签代码：" + dt记录.Rows[0]["标签代码"].ToString();
            mysheet.Cells[4, 12].Value = dt记录.Rows[0]["包装规格每箱千克"].ToString() + " Kg/箱";
            mysheet.Cells[5, 1].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            //mysheet.Cells[5, 6].Value = " " + dt记录.Rows[0]["领用数量"].ToString();
            //mysheet.Cells[5, 8].Value = " " + dt记录.Rows[0]["退库数量"].ToString();
            //mysheet.Cells[5, 9].Value = "标签领用数量：" + dt记录.Rows[0]["标签领用数量"].ToString();
            mysheet.Cells[5, 12].Value = dt记录.Rows[0]["包装规格每箱只数"].ToString() + " 只/箱";
            
           // mysheet.Cells[22, 1].Value = "审核员：" + dt记录.Rows[0]["审核员"].ToString();
            //内表信息
            int rownum = dt记录详情.Rows.Count;
            int ind = 0;
            //插入新行
            if (dt记录详情.Rows.Count > 14)
            {
                ind = dt记录详情.Rows.Count - 14;
                for (int i = 0; i < ind; i++)
                {
                    //在第8行插入
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[8 + i, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
            }

            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[7 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                mysheet.Cells[7 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["包装日期"].ToString()).ToString("yyyy/MM/dd");
                mysheet.Cells[7 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["时间"].ToString()).ToString("HH:mm");
                mysheet.Cells[7 + i, 4].Value = dt记录详情.Rows[i]["包装箱号"].ToString();
                mysheet.Cells[7 + i, 5].Value = dt记录详情.Rows[i]["包装明细"].ToString();
                mysheet.Cells[7 + i, 6].Value = dt记录详情.Rows[i]["包装数量箱数"].ToString();
                mysheet.Cells[7 + i, 7].Value = dt记录详情.Rows[i]["产品数量只数"].ToString();
                mysheet.Cells[7 + i, 8].Value = dt记录详情.Rows[i]["是否贴标签"].ToString() == "Yes" ? "Y" : "N";
                mysheet.Cells[7 + i, 9].Value = dt记录详情.Rows[i]["是否打包封箱"].ToString() == "Yes" ? "Y" : "N";
                mysheet.Cells[7 + i, 10].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[7 + i, 11].Value = dt记录详情.Rows[i]["审核员"].ToString();
                mysheet.Cells[7 + i, 12].Value = dt记录详情.Rows[i]["备注"].ToString();
            }
            mysheet.Cells[21 + ind, 6].Value = dt记录.Rows[0]["包装数量箱数合计"].ToString();
            mysheet.Cells[21 + ind, 7].Value = dt记录.Rows[0]["产品数量只数合计"].ToString();
                        
            //加页脚
            int sheetnum;
            SqlDataAdapter da = new SqlDataAdapter("select ID from " + table + " where 生产指令ID=" + InstruID.ToString(), mySystem.Parameter.conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            mysheet.PageSetup.RightFooter = Instruction + "-06-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
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

        //求合计
        private void getTotal()
        {
            Int32 numtemp;
            // 膜卷长度求和
            Int32 sum1, sum2 = 0;
            sum1 = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["包装数量箱数"].ToString(), out numtemp) == true)
                { sum1 += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            outerDataSync("lb包装数量箱数合计", sum1.ToString());
            // 膜卷重量求和
            Int32 numtemp2;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["产品数量只数"].ToString(), out numtemp2) == true)
                { sum2 += numtemp2; }
            }
            //dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];
            outerDataSync("lb产品数量只数合计", sum2.ToString());    
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
                if (dataGridView1.Columns[e.ColumnIndex].Name == "包装数量箱数")
                {
                    int guige = 0;
                    int xiang = 0;
                    Int32.TryParse(dt记录.Rows[0]["包装规格每箱只数"].ToString(), out guige);
                    Int32.TryParse(dt记录详情.Rows[e.RowIndex]["包装数量箱数"].ToString(), out xiang);
                    dt记录详情.Rows[e.RowIndex]["产品数量只数"] = guige * xiang;
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量只数")
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
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = true;
            }
            dataGridView1.Columns["时间"].DefaultCellStyle.Format = "HH:mm";

        }

        private void LDPE产品外包装记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }

        private void cb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn查询新建.PerformClick();
        }

        private void btn退回数据审核_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells.Count <= 0) return;
                int iid = Convert.ToInt32(dataGridView1.SelectedCells[0].OwningRow.Cells["ID"].Value);
                Utility.t退回数据审核(dt记录详情, iid, "审核员");
                //btn确认.PerformClick();
                Save();
                //innerBind();
            }
            catch
            {
                return;
            }
        }

        private void LDPE产品外包装记录_Load(object sender, EventArgs e)
        {
            try
            {
                cb产品代码.SelectedIndex = 0;
            }
            catch
            {
            }
        }


    }
}
