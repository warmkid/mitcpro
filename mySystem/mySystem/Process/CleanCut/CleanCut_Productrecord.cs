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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_Productrecord : BaseForm
    {
        private String table = "清洁分切生产记录";
        private String tableInfo = "清洁分切生产记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt物料代码;
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
        // TODO: IDShow后，datagridview_dataerror，序号报错？

        public CleanCut_Productrecord(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.cleancutInstruID;
            Instruction = Parameter.cleancutInstruction;
            cb白班.Checked = Parameter.userflight == "白班" ? true : false; 
            cb夜班.Checked = !cb白班.Checked;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            //查询条件可编辑
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
        }

        public CleanCut_Productrecord(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 清洁分切生产记录 where ID="+ID,connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            InstruID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            Instruction = dt.Rows[0]["生产指令编号"].ToString();


            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
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
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='全部'", connOle);
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

        //读取设置内容  //dt物料代码, 清洁前产品代码, 清洁前批号  //横向是否居中，判定是否合格，是否印刷状态添加“Yes”、“No”
        private void getOtherData()
        {
            //横向是否居中，判定是否合格，是否印刷状态添加“Yes”、“No”
            cmb横向是否居中.Items.Add("Yes");
            cmb横向是否居中.Items.Add("No");
            cmb判定是否合格.Items.Add("Yes");
            cmb判定是否合格.Items.Add("No");
            cmb是否印刷.Items.Add("Yes");
            cmb是否印刷.Items.Add("No");
            //*********物料代码*********//
            dt物料代码 = new DataTable("物料代码");            
            if (!isSqlOk)
            {
                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 清洁分切工序生产指令 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                 
                if (reader1.Read())
                {
                    //查找该生产ID下的产品编码、产品批号
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select ID, 清洁前产品代码, 清洁前批号 from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + reader1["ID"].ToString();
                    OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                    datemp.Fill(dt物料代码);
                    //if (dt物料代码.Rows.Count == 0)
                    //{
                    //    MessageBox.Show("该生产指令编码下的『清洁分切工序生产指令』尚未生成！");
                    //}
                    datemp.Dispose();
                }
                else
                {
                    //MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
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
            //if (stat_user == 2)//管理员
            if (_userState == Parameter.UserState.管理员)
            {
                //控件都能点
                setControlTrue();
            }
            //else if (stat_user == 1)//审核人
            else if (_userState == Parameter.UserState.审核员)//审核人
            {
                //if (stat_form == 0 || stat_form == 3 || stat_form == 2)  //0未保存||2审核通过||3审核未通过
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
                //if (stat_form == 1 || stat_form == 2) //1待审核||2审核通过
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
            tb生产指令编号.ReadOnly = true;
            cb白班.Enabled = false;
            cb夜班.Enabled = false;
            //查询条件始终不可编辑
            dtp生产日期.Enabled = false;
            btn查询新建.Enabled = false;
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
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件
        private void addComputerEventHandler() { }
        
        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID, Boolean flight, DateTime searchTime)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, flight, searchTime);
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
                readOuterData(InstruID, flight, searchTime);
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
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();
                // 添加分切前ID
                foreach (DataRow ndr in dt记录详情.Rows)
                {
                    if (ndr["分切前ID"].ToString().Trim() == "")
                    {
                        ndr["分切前ID"] = Convert.ToInt32(ndr["ID"]);
                    }
                }
                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

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
                cb白班.Checked = Convert.ToBoolean(dt1.Rows[0]["生产班次"].ToString());
                cb夜班.Checked = !cb白班.Checked;
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                Instruction = dt1.Rows[0]["生产指令编号"].ToString(); 
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), Convert.ToBoolean(dt1.Rows[0]["生产班次"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            }
        }
        
        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, Boolean flight, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 生产班次 = " + flight.ToString() + " and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            cb白班.DataBindings.Clear();
            cb白班.DataBindings.Add("Checked", bs记录.DataSource, "生产班次");
            //中间
            tb实测宽度.DataBindings.Clear();
            tb实测宽度.DataBindings.Add("Text", bs记录.DataSource, "实测宽度");
            cmb是否印刷.DataBindings.Clear();
            cmb是否印刷.DataBindings.Add("Text", bs记录.DataSource, "是否印刷");
            cmb横向是否居中.DataBindings.Clear();
            cmb横向是否居中.DataBindings.Add("Text", bs记录.DataSource, "横向是否居中");
            tb纵向尺寸测量实际值.DataBindings.Clear();
            tb纵向尺寸测量实际值.DataBindings.Add("Text", bs记录.DataSource, "纵向尺寸测量实际值");
            cmb判定是否合格.DataBindings.Clear();
            cmb判定是否合格.DataBindings.Add("Text", bs记录.DataSource, "判定是否合格");
            tb废品重量.DataBindings.Clear();
            tb废品重量.DataBindings.Add("Text", bs记录.DataSource, "废品重量");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            //下面
            tb操作人.DataBindings.Clear();
            tb操作人.DataBindings.Add("Text", bs记录.DataSource, "操作人");
            tb操作员备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Add("Text", bs记录.DataSource, "操作员备注");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp操作日期.DataBindings.Clear();
            dtp操作日期.DataBindings.Add("Text", bs记录.DataSource, "操作日期");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
                        
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产指令编号"] = Instruction;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["生产班次"] = cb白班.Checked;
            dr["是否印刷"] = "Yes";
            dr["横向是否居中"] = "Yes";
            dr["判定是否合格"] = "Yes";
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = Convert.ToDateTime(dtp操作日期.Value.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(dtp审核日期.Value.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;
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
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T清洁分切生产记录ID = " + ID.ToString() + " Order by 序号", connOle);
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

        //添加行代码, 要求：dt物料代码.Rows.Count > 0
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            dr["序号"] = 0;
            dr["T清洁分切生产记录ID"] = ID;
            dr["物料代码"] = dt物料代码.Rows[0]["清洁前产品代码"].ToString();
            dr["膜卷批号"] = dt物料代码.Rows[0]["清洁前批号"].ToString();
            dr["膜卷卷号"] = 0;
            dr["长度A"] = 0;
            dr["重量"] = 0;
            dr["清洁分切后代码"] = dr["物料代码"] + "C";
            dr["长度B"] = 0;
            dr["收率"] = -1;
            dr["外观检查"] = "Yes";
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
                    case "外观检查":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("Yes");
                        cbc.Items.Add("No");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        cbc.MinimumWidth = 80;
                        break;
                    case "物料代码":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        if (dt物料代码 != null)
                        {
                            for (int i = 0; i < dt物料代码.Rows.Count; i++)
                            { cbc.Items.Add(dt物料代码.Rows[i]["清洁前产品代码"]); }
                        }                        
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            dataGridView1.Columns["T清洁分切生产记录ID"].Visible = false;
            dataGridView1.Columns["分切前ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["膜卷批号"].ReadOnly = true;
            dataGridView1.Columns["长度A"].HeaderText = "长度\r(m)";
            dataGridView1.Columns["重量"].HeaderText = "重量\r(kg)";
            dataGridView1.Columns["长度B"].HeaderText = "长度\r(m)";
            dataGridView1.Columns["收率"].ReadOnly = true;
            dataGridView1.Columns["收率"].HeaderText = "收率\r(%)";
            dataGridView1.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (dt物料代码.Rows.Count > 0)
            { DataShow(InstruID, cb白班.Checked, dtp生产日期.Value); }
            else
            { MessageBox.Show("当前生产指令尚未设置完毕！"); }            
        }

        //添加行按钮
        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
            // 内表保存
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();
            // 添加分切前ID
            foreach (DataRow ndr in dt记录详情.Rows)
            {
                if (ndr["分切前ID"].ToString().Trim() == "")
                {
                    ndr["分切前ID"] = Convert.ToInt32(ndr["ID"]);
                }
            }
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();
        }

        //删除行按钮
        private void btn删除_Click(object sender, EventArgs e)
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

        //分切按钮
        private void btn分切_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            if (dataGridView1.SelectedCells.Count == 0) return;
            int idx = dataGridView1.SelectedCells[0].RowIndex;
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dr["序号"] = 0;
            dr["T清洁分切生产记录ID"] = dt记录.Rows[0]["ID"];
            dr["膜卷卷号"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["膜卷卷号"].ToString();
            dr["物料代码"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["物料代码"].ToString();
            dr["膜卷批号"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["膜卷批号"].ToString();
            dr["清洁分切后代码"] = getCodeAfter(dr["物料代码"].ToString());
            dr["外观检查"] = "Yes";
            dr["长度A"] = 0;
            dr["长度B"] = 0;
            dr["重量"] = 0;
            dr["收率"] = -1;
            dr["分切前ID"] = Convert.ToInt32(dataGridView1.Rows[idx].Cells["ID"].Value);
            dt记录详情.Rows.InsertAt(dr, dataGridView1.CurrentRow.Index+1);
            setDataGridViewRowNums();
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
            if (mySystem.Parameter.NametoID(tb操作人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『操作员』信息", "ERROR");
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*宽度、尺寸、质量不合格*/
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
                readOuterData(InstruID, cb白班.Checked, dtp生产日期.Value);
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
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切生产记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "清洁分切生产记录";
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切生产记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
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
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录.Rows[0]["操作人"].ToString())
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
        }

        //打印按钮 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!    
        // TODO 打印时“实测宽度”和“纵向尺寸实测值”如果为“”，则不填
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\cleancut\SOP-MFG-302-R03A 清洁分切生产记录表.xlsx");
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
            mysheet.Cells[3, 1].Value = "生产指令编号： " + dt记录.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[3, 7].Value = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            String flighttemp = Convert.ToBoolean(dt记录.Rows[0]["生产班次"].ToString()) == true ? "白班" : "夜班";
            mysheet.Cells[3, 9].Value = "生产班次：" + flighttemp;
            String stringtemp = "";
            stringtemp = dt记录.Rows[0]["实测宽度"].ToString() == "" ? "NA" : dt记录.Rows[0]["实测宽度"].ToString();
            mysheet.Cells[14, 2].Value = "2.产品规格确认：允许公差±5mm, 实测宽度：" + stringtemp + " mm。";
            stringtemp = "3.印刷：" + (dt记录.Rows[0]["是否印刷"].ToString() == "Yes" ? "是" : "否");
            stringtemp = stringtemp + " ；  横向是否居中，" + (dt记录.Rows[0]["横向是否居中"].ToString() == "Yes" ? "是" : "否");
            stringtemp = stringtemp + " ；  纵向尺寸测量实际值：" + (dt记录.Rows[0]["纵向尺寸测量实际值"].ToString() == "" ? "NA" : dt记录.Rows[0]["纵向尺寸测量实际值"].ToString());
            stringtemp = stringtemp + " mm。\r  判定： " + (dt记录.Rows[0]["判定是否合格"].ToString() == "Yes" ? "合格" : "不合格") + "。";
            mysheet.Cells[15, 2].Value = stringtemp;
            mysheet.Cells[13, 10].Value = "废品重量： " + dt记录.Rows[0]["废品重量"].ToString() + " kg";
            mysheet.Cells[16, 1].Value = " 备注： " + dt记录.Rows[0]["备注"].ToString();            
            stringtemp = "操作人：" + dt记录.Rows[0]["操作人"].ToString();
            stringtemp = stringtemp + "       操作日期：" + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["操作日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[17, 1].Value = stringtemp;
            stringtemp = "审核人：" + dt记录.Rows[0]["审核人"].ToString();
            stringtemp = stringtemp + "       审核日期：" + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dt记录.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            mysheet.Cells[17, 7].Value = stringtemp;

            //内表信息
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 8 ? 8 : rownum); i++)
            {
                mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString(); 
                mysheet.Cells[5 + i, 2].Value = dt记录详情.Rows[i]["工时"].ToString();
                mysheet.Cells[5 + i, 3].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["膜卷批号"].ToString() + " - " + dt记录详情.Rows[i]["膜卷卷号"].ToString();
                mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["长度A"].ToString();
                mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["重量"].ToString();
                mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["清洁分切后代码"].ToString();
                mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["长度B"].ToString();
                mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["收率"].ToString();
                mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["外观检查"].ToString() == "Yes" ? "合格" : "不合格";
            }
            //需要插入的部分
            if (rownum > 8)
            {
                for (int i = 8; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[5 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                    mysheet.Cells[5 + i, 2].Value = dt记录详情.Rows[i]["工时"].ToString();
                    mysheet.Cells[5 + i, 3].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                    mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["膜卷批号"].ToString() + " - " + dt记录详情.Rows[i]["膜卷卷号"].ToString();
                    mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["长度A"].ToString();
                    mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["重量"].ToString();
                    mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["清洁分切后代码"].ToString();
                    mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["长度B"].ToString();
                    mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["收率"].ToString();
                    mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["外观检查"].ToString() == "Yes" ? "合格" : "不合格";
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
            mysheet.PageSetup.RightFooter = Instruction + "-04-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }
           
        //******************************小功能******************************//  
        
        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb废品重量 });
            List<String> StringList = new List<String>(new String[] {  "废品重量" });
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
            if (cmb是否印刷.SelectedItem.ToString() == "Yes")
            {
                if (!Int32.TryParse(tb纵向尺寸测量实际值.Text.ToString(), out numtemp))
                {
                    MessageBox.Show("『纵向尺寸测量』框内应填数字，请重新填入！");
                    TypeCheck = false;
                    tb纵向尺寸测量实际值.Text = "";
                }
            }
            if (!Int32.TryParse(tb实测宽度.Text, out numtemp))
            {
                tb实测宽度.Text = "";
            }
            return TypeCheck;
        }

        //正则表达式，检查分切后格式
        private bool CheckCodeFormat(String Code)
        {
            //eg:TY-200*300*3C
            String pattern = @"^[a-zA-Z]+-[0-9]+\*[0-9]+\*[0-9]+C";//正则表达式
            if (!Regex.IsMatch(Code, pattern))
            {
                MessageBox.Show("清洁分切后代码格式不符合规定，重新输入，例如 TY-200*300*3C");
                return false;
            }
            return true;
        }

        //求出分切后的清洁分切后代码
        private String getCodeAfter(String codeBefore)
        {
            String codeAfter;
            return (codeBefore + "C");
            //Int32 leng;
            ////eg: codeBefore = TY-200*300*3
            //String pattern = @"^[a-zA-Z]+-[0-9]+\*[0-9]+\*[0-9]";//正则表达式
            //if (!Regex.IsMatch(codeBefore, pattern))
            //{
            //    MessageBox.Show("清洁分切前代码格式不正确");
            //    return (codeBefore + "C");
            //}
            //else
            //{
            //    string[] array1 = codeBefore.Split('-'); //array1[0]=TY array1[1]=200*300*3
            //    string[] array2 = array1[1].Split('*'); //array2[0]=200 array2[1]=300 array2[2]=3
            //    leng = Int32.Parse(array2[0]);
            //    codeAfter = array1[0] + "-0*" + array2[1] + "*" + array2[2] + "C";
            //    return codeAfter; //TY-0*300*3C
            //}
        }

        //实时求收率
        private void getPercent(Int32 Rownum)
        {
            //int len1;
            //int len2;
            //// 膜卷长度求和
            //if ((Int32.TryParse(dt记录详情.Rows[Rownum]["长度A"].ToString(), out len1) == true) && (Int32.TryParse(dt记录详情.Rows[Rownum]["长度B"].ToString(), out len2) == true))
            //{
            //    //均为数值类型
            //    if (len1 == 0)
            //    { dt记录详情.Rows[Rownum]["收率"] = -1;  }
            //    else
            //    { dt记录详情.Rows[Rownum]["收率"] = (Int32)((Double)len2/(Double)len1 * 100); }
            //}
            //else
            //{ dt记录详情.Rows[Rownum]["收率"] = -1; }

            DataRow[] drs;
            // 获取当前row的分切前ID，分切前的数据
            int idB = Convert.ToInt32(dt记录详情.Rows[Rownum]["分切前ID"]);
            drs = dt记录详情.Select("ID=" + idB);
            string codeB = drs[0]["物料代码"].ToString().Trim();
            int widthB = getWidthB(codeB);
            int lengthB = Convert.ToInt32(drs[0]["长度A"]);
            double before = widthB * lengthB / 1000.0;
            // 找到同一分切前ID的所有数据,并累计
            double after = 0;
            int widthA, lengthA;
            string codeA;
            drs = dt记录详情.Select("分切前ID=" + idB);
            foreach (DataRow dr in drs)
            {
                codeA = dr["清洁分切后代码"].ToString().Trim();
                widthA = getWidthA(codeA);
                lengthA = Convert.ToInt32(dr["长度B"]);
                after += widthA * lengthA / 1000.0;
            }
            double ratio = after / before;
            foreach (DataRow dr in drs)
            {
                dr["收率"] = Double.Parse((ratio * 100).ToString("f1"));
            }
            //
            


        }

        // TODO 乘法符号待统一
        int getWidthB(String code)
        {
            char MULTI = 'X';
            int widthB = 0;
            Regex re1B = new Regex(@"^SPM-TY-\d+X\d+X\d+$");
            Regex re2B = new Regex(@"^SPM-TY-\d+X\d+$");
            Regex re3B = new Regex(@"^SPM-TN-\d+$");
            Regex re4B = new Regex(@"^PEF-TA-\d+X\d+$");
            Regex re5B = new Regex(@"^XP1-SA-\d+X\d+$");
            Regex re6B = new Regex(@"^UP1-SA-\d+X\d+$");
            //Match m1 =  re1.Match(codeB);
            //Match m2 = re2.Match(codeB);
            string val;
            string[] tNum, tW;
            string nums;
            if (re1B.Match(code).Value != "")
            { // SPM-TY-320X450X2
                val = re1B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthB = Convert.ToInt32(tW[1]) * Convert.ToInt32(tW[2]);

            }
            else if (re2B.Match(code).Value != "")
            { // SPM-TY-320X450
                val = re2B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthB = Convert.ToInt32(tW[1]);

            }
            else if (re3B.Match(code).Value != "")
            { // SPM-TN-400
                val = re3B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                widthB = Convert.ToInt32(nums);
            }
            else if (re4B.Match(code).Value != "")
            { // PEF-TA-600X100
                val = re4B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthB = Convert.ToInt32(tW[0]);
            }
            else if (re5B.Match(code).Value != "")
            { // XP1-SA-1010X080
                val = re5B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthB = Convert.ToInt32(tW[0]);
            }
            else if (re6B.Match(code).Value != "")
            { // UP1-SA-1200X120
                val = re6B.Match(code).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthB = Convert.ToInt32(tW[0]);
            }
            return widthB;
        }

        int getWidthA(string codeA)
        {
            char MULTI = 'X';
            int widthA = 0;
            string val;
            string[] tNum, tW;
            string nums;
            Regex re1A = new Regex(@"^SPM-TY-\d+X\d+X\d+C$");//SPM-TY-320X450X2C
            Regex re2A = new Regex(@"^SPM-TN-\d+C$");//SPM-TN-400C
            Regex re3A = new Regex(@"^PEF-CA-\d+X\d+$"); // PEF-CA-450X100
            Regex re4A = new Regex(@"^XP1-SA-\d+X\d+$"); // XP1-SA-500X080
            Regex re5A = new Regex(@"^UP1-SA-\d+X\d+$"); // UP1-SA-900X120

            if (re1A.Match(codeA).Value != "")
            { //SPM-TY-320X450X2C
                val = re1A.Match(codeA).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                nums = nums.Substring(0, nums.Length - 1);
                tW = nums.Split(MULTI);
                widthA = Convert.ToInt32(tW[1]) * Convert.ToInt32(tW[2]);
            }
            else if (re2A.Match(codeA).Value != "")
            { // SPM-TN-400C
                val = re2A.Match(codeA).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                nums = nums.Substring(0, nums.Length - 1);
                widthA = Convert.ToInt32(nums);
            }
            else if (re3A.Match(codeA).Value != "")
            { // PEF-CA-450X100
                val = re3A.Match(codeA).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthA = Convert.ToInt32(tW[0]);
            }
            else if (re4A.Match(codeA).Value != "")
            { // XP1-SA-500X080
                val = re4A.Match(codeA).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthA = Convert.ToInt32(tW[0]);
            }
            else if (re5A.Match(codeA).Value != "")
            { // UP1-SA-900X120
                val = re5A.Match(codeA).Value;
                tNum = val.Split('-');
                nums = tNum[tNum.Length - 1];
                tW = nums.Split(MULTI);
                widthA = Convert.ToInt32(tW[0]);
            }
            return widthA;
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

        //实时求收率
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "物料代码")
                {
                    //实时根据“物料代码”，显示“膜卷批号”
                    //dt记录详情.Rows[e.RowIndex]["膜卷批号"] = dt物料代码.Rows[];
                    ComboBox cbc = new ComboBox();
                    for (int i = 0; i < dt物料代码.Rows.Count; i++)
                    { cbc.Items.Add(dt物料代码.Rows[i]["清洁前产品代码"]); }
                    Int32 index = cbc.FindString(dt记录详情.Rows[e.RowIndex]["物料代码"].ToString());
                    dt记录详情.Rows[e.RowIndex]["膜卷批号"] = dt物料代码.Rows[index]["清洁前批号"].ToString();
                    dt记录详情.Rows[e.RowIndex]["清洁分切后代码"] = dt记录详情.Rows[e.RowIndex]["物料代码"].ToString() + "C";
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "长度A")
                {
                    //实时求“收率”
                    getPercent(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "长度B")
                {
                    //实时求“收率”
                    getPercent(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "清洁分切后代码")
                {
                    //实时检查“清洁分切后代码”格式是否合格
                    //if (CheckCodeFormat(dt记录详情.Rows[e.RowIndex]["清洁分切后代码"].ToString()) == false)
                    //{ dt记录详情.Rows[e.RowIndex]["清洁分切后代码"] = getCodeAfter(dt记录详情.Rows[e.RowIndex]["物料代码"].ToString()); }
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
