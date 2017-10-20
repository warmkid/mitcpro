using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Other
{
    public partial class 生产领料申请单 : BaseForm
    {
        private DataTable _dt物料代码数量;
        private Int32 _id, _生产指令ID;
        private String _生产指令编号, _申请单号, _属于工序, _产品代码, _产品批号;

        private String table = "生产领料申请单表";
        private String tableInfo = "生产领料申请单详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private Boolean isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private List<String> ls操作员, ls审核员;
        private Parameter.UserState _userState;
        private Parameter.FormState _formState;

        public 生产领料申请单(mySystem.MainForm mainform, DataTable dt生产指令构造, DataTable dt物料代码数量构造, SqlConnection conn构造, OleDbConnection connOle构造)
            : base(mainform)
        {
            InitializeComponent();
            //从基类继承各种函数
            conn = conn构造;
            connOle = connOle构造;
            variableInit(dt生产指令构造);
            getProductInfo(dt物料代码数量构造); //从参数中获取物料代码以及数量
            //设置
            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //______获取申请单号
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
            //新建外表
            readOuterData(_生产指令ID, _申请单号);
            outerBind();
            DataRow dr = dt记录.NewRow();
            dr = writeOuterDefault(dr);
            dt记录.Rows.Add(dr);
            da记录.Update((DataTable)bs记录.DataSource);
            readOuterData(_生产指令ID, _申请单号);
            outerBind();
            _id = Convert.ToInt32(dt记录.Rows[0]["ID"].ToString());
            //新建内表绑定
            readInnerData(_id);
            innerBind();
            setDataGridViewColumns();
            // 其他设置
            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  
        }

        public 生产领料申请单(mySystem.MainForm mainform, Int32 ID, DataTable dt生产指令构造, DataTable dt物料代码数量构造, SqlConnection conn构造, OleDbConnection connOle构造)
            : base(mainform)
        {
            InitializeComponent();
            //从基类继承各种函数

            conn = conn构造;
            connOle = connOle构造;
            variableInit(dt生产指令构造);
            getProductInfo(dt物料代码数量构造); //从参数中获取物料代码以及数量

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherData();  //______获取申请单号
            getProductInfo(dt物料代码数量构造); //______从参数中获取物料代码以及数量
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            // 读取外表数据
            readOuterData(ID);
            outerBind();
            _id = Convert.ToInt32(dt记录.Rows[0]["ID"]);
            readInnerData(_id);
            setDataGridViewColumns();
            innerBind();

            // 设置状态和控件可用性
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
        }

        //******************************初始化******************************//

        //从基类继承各种函数
        private void variableInit(DataTable 生产指令信息)
        {
            _生产指令ID = Convert.ToInt32(生产指令信息.Rows[0]["生产指令ID"]);
            _生产指令编号 = 生产指令信息.Rows[0]["生产指令编号"].ToString();
            _属于工序 = 生产指令信息.Rows[0]["属于工序"].ToString();
            _产品代码 = 生产指令信息.Rows[0]["产品代码"].ToString();
            _产品批号 = 生产指令信息.Rows[0]["产品批号"].ToString();
            isSqlOk = Convert.ToBoolean(生产指令信息.Rows[0]["isSqlOk"].ToString());
        }

        // 获取操作员和审核员
        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='生产领料申请单表'", connOle);
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

        // 获取申请单号  
        private void getOtherData()
        {
            DataTable dttemp = new DataTable("dttemp");

            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where 生产指令ID = " + _生产指令ID;//这里应有生产指令编码
            OleDbDataAdapter datemp1 = new OleDbDataAdapter(comm1);
            datemp1.Fill(dttemp);

            if (dttemp.Rows.Count <= 0)
            { _申请单号 = _生产指令编号 + "-1"; }
            else
            {
                List<String> stringNum = new List<string>();
                List<Int32> intNum = new List<Int32>();
                string[] tNum;
                Int32 tNumTemp;
                foreach (DataRow dr in dttemp.Rows)
                {
                    tNum = dr["申请单编号"].ToString().Split('-');
                    Int32.TryParse(tNum[tNum.Length - 1], out tNumTemp);
                    intNum.Add(tNumTemp);
                }
                intNum = intNum.OrderByDescending(s => s).ToList(); //从大到小排序
                if (intNum[0] >= 1)
                { _申请单号 = _生产指令编号 + "-" + (intNum[0] + 1).ToString(); }
                else
                { _申请单号 = _生产指令编号 + "-1"; }
            }
        }

        //产品代码、产品批号初始化
        private void getProductInfo(DataTable 物料代码)
        {
            _dt物料代码数量 = new DataTable("物料代码数量");
            _dt物料代码数量 = 物料代码.Copy();
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
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        //if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                        if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
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
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
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
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        if (dt记录详情.Rows[i]["审核员"].ToString() != "")
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
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        if (dt记录详情.Rows[i]["审核员"].ToString() != "")
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

        //****************************** 嵌套 ******************************//

        //读取外表
        private void readOuterData(Int32 InstruID, String RequestNum)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and  申请单编号 ='" + RequestNum + "'", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        // 读取数据，根据自己表的ID
        private void readOuterData(int id)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable("table");
            da记录 = new OleDbDataAdapter("select * from " + table + " where ID= " + id.ToString(), connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;

            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bs记录.DataSource, c.Name.Substring(2));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bs记录.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("Text", bs记录.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bs记录.DataSource, c.Name.Substring(3));
                }
            }

        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = _生产指令ID;
            dr["生产指令编号"] = _生产指令编号;
            dr["申请单编号"] = _申请单号;
            dr["备注"] = "物料单位---膜材用米，管用米,物料用kg，其余物料用个。";
            dr["审核员"] = "";
            dr["审核是否通过"] = false;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + _生产指令编号 + "\n";
            dr["日志"] = log;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where 生产领料申请表单ID = " + ID.ToString(), connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            dr["生产领料申请表单ID"] = ID;
            dr["序号"] = 0;
            dr["申请日期时间"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            dr["物料代码"] = _dt物料代码数量.Rows[0]["物料代码"].ToString();
            dr["物料批号"] = _dt物料代码数量.Rows[0]["物料代码"].ToString();
            dr["申请数量主计量"] = _dt物料代码数量.Rows[0]["数量"].ToString();
            dr["申请数量辅计量"] = 0;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["审核员"] = "";
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
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "物料代码")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (_dt物料代码数量 != null)
                    {
                        for (int i = 0; i < _dt物料代码数量.Rows.Count; i++)
                        { cbc.Items.Add(_dt物料代码数量.Rows[i]["物料代码"]); }
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }

                // 根据数据类型自动生成列的关键信息
                switch (dc.DataType.ToString())
                {

                    case "System.Int32":
                    case "System.String":
                    case "System.Double":
                    case "System.DateTime":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(ckbc);
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
            //隐藏
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["生产领料申请表单ID"].Visible = false;
            dataGridView1.Columns["物料批号"].Visible = false;
            //不可用
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["申请数量主计量"].ReadOnly = true;
            //dataGridView1.Columns["物料简称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //HeaderText
            dataGridView1.Columns["申请数量主计量"].HeaderText = "申请数量\r（主计量）";
            dataGridView1.Columns["申请数量辅计量"].HeaderText = "申请数量\r（辅计量）";
        }

        //******************************按钮功能******************************//

        //添加行按钮
        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(_id, dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        //删除按钮
        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 1)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //dt记录详情.Rows.RemoveAt(deletenum);
                dt记录详情.Rows[deletenum].Delete();

                // 保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(_id);
                innerBind();

                setDataGridViewRowNums();
            }
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
            readInnerData(_id);
            innerBind();
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
            readInnerData(_id);
            innerBind();
            setEnableReadOnly();
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
                readInnerData(_id);
                innerBind();

                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(_id);
                outerBind();

                setEnableReadOnly();

                return true;
            }
        }

        //检查操作员名字是否正确
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='生产领料申请单表' and 对应ID=" + dt记录.Rows[0]["ID"], connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "生产领料申请单表";
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

            //日志准备阶段
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";

            // 写数据
            if (checkform.ischeckOk)
            {
                DataTable dt出库Out = new DataTable("出库Outer");
                dt出库Out.Columns.Add("生产指令ID", Type.GetType("System.Int32"));
                dt出库Out.Columns.Add("生产指令编号", Type.GetType("System.String"));
                dt出库Out.Columns.Add("产品代码", Type.GetType("System.String"));
                dt出库Out.Columns.Add("产品批号", Type.GetType("System.String"));
                dt出库Out.Columns.Add("审核员", Type.GetType("System.String"));
                dt出库Out.Columns.Add("审核日期", Type.GetType("System.DateTime"));
                dt出库Out.Columns.Add("审核是否通过", Type.GetType("System.Boolean"));
                dt出库Out.Columns.Add("属于工序", Type.GetType("System.String"));
                dt出库Out.Columns.Add("日志", Type.GetType("System.String"));
                dt出库Out.Columns.Add("审核意见", Type.GetType("System.String"));
                dt出库Out.Rows.Add(new object[] { _生产指令ID, _生产指令编号, _产品代码, _产品批号, mySystem.Parameter.userName, DateTime.Now, true, _属于工序, (dt记录.Rows[0]["日志"].ToString() + log), checkform.opinion });

                DataTable dt出库Inner = new DataTable("出库Inner");
                dt出库Inner.Columns.Add("出库日期时间", Type.GetType("System.DateTime"));
                dt出库Inner.Columns.Add("物料代码", Type.GetType("System.String"));
                dt出库Inner.Columns.Add("物料批号", Type.GetType("System.String"));
                dt出库Inner.Columns.Add("发料数量", Type.GetType("System.Int32"));

                foreach (DataRow dr in dt记录详情.Rows)
                {
                    dt出库Inner.Rows.Add(new object[] { DateTime.Now, dr["物料代码"], dr["物料批号"], dr["申请数量辅计量"] });
                }

                //if (mySystem.Process.Stock.材料退库出库单.生成表单(2, dt出库Out, dt出库Inner, _属于工序) == false)
                //{ 
                //    MessageBox.Show("出库失败，请重新审核！");
                //    return;
                //}

            }

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='生产领料申请单表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志            
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
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\SOP-MFG-101-R06A 领料申请单.xlsx");
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
            mysheet.Cells[3, 3].Value = dt记录.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[3, 7].Value = dt记录.Rows[0]["申请单编号"].ToString();
            mysheet.Cells[20, 1].Value = "备注：" + dt记录.Rows[0]["备注"].ToString();
            mysheet.Cells[21, 1].Value = "审核员：" + dt记录.Rows[0]["审核员"].ToString();
            //内表信息
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 15 ? 15 : rownum); i++)
            {
                mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["申请日期时间"].ToString()).ToString("yyyy/MM/dd");
                mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["申请日期时间"].ToString()).ToString("HH:mm:ss");
                mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["申请数量主计量"].ToString();
                mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["申请数量辅计量"].ToString();
                mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["审核员"].ToString();
            }
            //需要插入的部分
            if (rownum > 9)
            {
                for (int i = 15; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[5 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                    mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["申请日期时间"].ToString()).ToString("yyyy/MM/dd");
                    mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["申请日期时间"].ToString()).ToString("HH:mm:ss");
                    mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                    mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["申请数量主计量"].ToString();
                    mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["申请数量辅计量"].ToString();
                    mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["审核员"].ToString();
                }
            }
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from " + table + " where 生产指令ID=" + _生产指令ID.ToString(), connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            mysheet.PageSetup.RightFooter = _生产指令编号 + "-09-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
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

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "物料代码")
                {
                    DataRow[] rows = _dt物料代码数量.Select("物料代码 = '" + dt记录详情.Rows[e.RowIndex]["物料代码"].ToString() + "'");
                    if (rows.Length > 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["物料批号"] = rows[0]["物料批号"];
                        dt记录详情.Rows[e.RowIndex]["申请数量主计量"] = rows[0]["数量"];
                    }
                    else
                    { MessageBox.Show("尚未查到物料代码为『" + dt记录详情.Rows[e.RowIndex]["物料代码"].ToString() + "』的数据，请完善后再填写!"); }
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
