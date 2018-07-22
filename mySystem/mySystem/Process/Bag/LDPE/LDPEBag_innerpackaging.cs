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
    public partial class LDPEBag_innerpackaging : BaseForm
    {
        private String table = "产品内包装记录";
        private String tableInfo = "产品内包装记录详细信息";

        private SqlConnection conn = null;
        //private OleDbConnection mySystem.Parameter.conn = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt代码批号;
        private SqlDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private SqlCommandBuilder cb记录, cb记录详情;

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        String Flight = "";
        bool isFirstBind = true;

        List<String> dm所有相关产品代码;

        void getAll产品代码()
        {
            dm所有相关产品代码 = new List<string>();
            string strConnect = "server=" + Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection connToOrder = new SqlConnection(strConnect);
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 设置存货档案 where 属于工序 like '%PE制袋%'", connToOrder);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                dm所有相关产品代码.Add(dr["存货代码"].ToString());
            }

            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            acsc.AddRange(dm所有相关产品代码.ToArray());
            tb联产品代码.AutoCompleteCustomSource = acsc;
            tb联产品代码.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tb联产品代码.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

           

        }

        public LDPEBag_innerpackaging(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.ldpebagInstruID;
            Instruction = Parameter.ldpebagInstruction;
            Flight = Parameter.userflight;
            tb班次.Text = Flight;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();

            getAll产品代码();

            cb产品代码.Enabled = true;
            //dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;

        }

        public LDPEBag_innerpackaging(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            IDShow(ID);

            getAll产品代码();
        }
        
        //******************************初始化******************************//

        // 获取操作员和审核员
        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='产品内包装记录'", mySystem.Parameter.conn);
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
            dt代码批号 = new DataTable("代码批号");

            if (isSqlOk)
            {
                SqlCommand comm1 = new SqlCommand();
                comm1.Connection = mySystem.Parameter.conn;
                comm1.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
                SqlDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    SqlCommand comm2 = new SqlCommand();
                    comm2.Connection = mySystem.Parameter.conn;
                    comm2.CommandText = "select ID, 产品代码, 产品批号, 内包装规格每包只数, 内标签 from 生产指令详细信息 where T生产指令ID = " + reader1["ID"].ToString();

                    SqlDataAdapter datemp = new SqlDataAdapter(comm2);
                    datemp.Fill(dt代码批号);
                    if (dt代码批号.Rows.Count == 0)
                    {
                        MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！");
                    }
                    else
                    {
                        for (int i = 0; i < dt代码批号.Rows.Count; i++)
                        {
                            cb产品代码.Items.Add(dt代码批号.Rows[i][1].ToString());//添加
                        }
                    }
                    datemp.Dispose();








                }
                else
                {
                    //dt代码批号为空
                    MessageBox.Show("该生产指令编码下的『生产指令』尚未生成！");
                }
                reader1.Dispose();
            }
            else
            { }

            //*********数据填写*********//
            cb产品代码.SelectedIndex = -1;
            tb生产批号.Text = "";
            tb生产指令编号.Text = Instruction;
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
            tb审核员.Enabled = false;
            btn数据审核.Enabled = false;
            btn提交数据审核.Enabled = false;
            //部分空间防作弊，不可改
            tb生产指令编号.ReadOnly = true;
            tb生产批号.ReadOnly = true;
            tb班次.ReadOnly = true;
            tb包装规格每包只数.ReadOnly = true;
            //tb内标签.ReadOnly = true;
            tb热封线不合格合计.ReadOnly = true;
            tb黑点晶点不合格合计.ReadOnly = true;
            tb其他合计.ReadOnly = true;
            tb不良合计.ReadOnly = true;
            tb产品数量包数合计A.ReadOnly = true;
            tb产品数量只数合计B.ReadOnly = true;
            //tb成品率.ReadOnly = true;
            //查询条件始终不可编辑
            cb产品代码.Enabled = false;
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

        // 设置自动计算类事件：TextChanged&Leave
        private void addComputerEventHandler()
        {
            tb废品重量.TextChanged += new EventHandler(tb理论产量C_TextChanged);
            //tb理论产量C.Leave += new EventHandler(tb理论产量C_TextChanged);
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

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 SearchInstruID, String productCode, DateTime SearchTime, String SearchFlight)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(SearchInstruID, productCode, SearchTime, SearchFlight);
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
                readOuterData(SearchInstruID, productCode, SearchTime, SearchFlight);
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
                Flight = dt1.Rows[0]["班次"].ToString();
                DataShow(InstruID, dt1.Rows[0]["产品代码"].ToString(), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()), Flight);
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode, DateTime SearchTime, String SearchFlight)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' and 班次 = '" + SearchFlight.ToString() + "' and 生产日期 = '" + SearchTime.ToString("yyyy/MM/dd") + "' ", mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //解绑->绑定
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            cb产品代码.DataBindings.Clear();
            cb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb生产批号.DataBindings.Clear();
            tb生产批号.DataBindings.Add("Text", bs记录.DataSource, "生产批号");
            cb标签语言中文.DataBindings.Clear();
            cb标签语言中文.DataBindings.Add("Checked", bs记录.DataSource, "标签语言是否中文");
            cb标签语言英文.DataBindings.Clear();
            cb标签语言英文.DataBindings.Add("Checked", bs记录.DataSource, "标签语言是否英文");

            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            tb班次.DataBindings.Clear();
            tb班次.DataBindings.Add("Text", bs记录.DataSource, "班次");
            tb包装规格每包只数.DataBindings.Clear();
            tb包装规格每包只数.DataBindings.Add("Text", bs记录.DataSource, "包装规格每包只数");
            //tb内标签.DataBindings.Clear();
            //tb内标签.DataBindings.Add("Text", bs记录.DataSource, "内标签");

            tb产品数量包数合计A.DataBindings.Clear();
            tb产品数量包数合计A.DataBindings.Add("Text", bs记录.DataSource, "产品数量包合计A");
            tb产品数量只数合计B.DataBindings.Clear();
            tb产品数量只数合计B.DataBindings.Add("Text", bs记录.DataSource, "产品数量只合计B");
            tb废品重量.DataBindings.Clear();
            tb废品重量.DataBindings.Add("Text", bs记录.DataSource, "废品重量");
            //tb成品率.DataBindings.Clear();
            //tb成品率.DataBindings.Add("Text", bs记录.DataSource, "成品率");
            tb工时.DataBindings.Clear();
            tb工时.DataBindings.Add("Text", bs记录.DataSource, "工时");

            tb热封线不合格合计.DataBindings.Clear();
            tb热封线不合格合计.DataBindings.Add("Text", bs记录.DataSource, "热封线不合格合计");
            tb黑点晶点不合格合计.DataBindings.Clear();
            tb黑点晶点不合格合计.DataBindings.Add("Text", bs记录.DataSource, "黑点晶点不合格合计");
            tb其他合计.DataBindings.Clear();
            tb其他合计.DataBindings.Add("Text", bs记录.DataSource, "其他合计");
            tb不良合计.DataBindings.Clear();
            tb不良合计.DataBindings.Add("Text", bs记录.DataSource, "不良合计");                

            tb操作员备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Add("Text", bs记录.DataSource, "操作员备注");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");

            tb联产品代码.DataBindings.Clear();
            tb联产品代码.DataBindings.Add("Text", bs记录.DataSource, "联产品代码");

            tb联产品数量.DataBindings.Clear();
            tb联产品数量.DataBindings.Add("Text", bs记录.DataSource, "联产品数量");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产指令编号"] = Instruction;
            dr["产品代码"] = cb产品代码.Text;
            dr["生产批号"] = dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["产品批号"].ToString();
            dr["标签语言是否中文"] = (dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["内标签"].ToString() == "中文");
            dr["标签语言是否英文"] = !(dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["内标签"].ToString() == "中文");
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd")); 
            dr["班次"] = Flight;
            try { dr["包装规格每包只数"] = Convert.ToInt32(dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["内包装规格每包只数"].ToString()); }
            catch { dr["包装规格每包只数"] = 0; }
            dr["产品数量包合计A"] = 0;
            dr["产品数量只合计B"] = 0;
            dr["工时"] = 0;
            dr["热封线不合格合计"] = 0;
            dr["黑点晶点不合格合计"] = 0;
            dr["指示剂不良合计"] = 0;
            dr["其他合计"] = 0;
            dr["不良合计"] = 0;
            dr["废品重量"] = 0;
            //dr["理论产量C"] = 0;
            //dr["成品率"] = -1;
            dr["操作员备注"] = "";
            dr["审核员"] = "";
            dr["审核是否通过"] = false;
            dr["联产品数量"] = 0;
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
            da记录详情 = new SqlDataAdapter("select * from " + tableInfo + " where T产品内包装记录ID = " + ID.ToString() + " order by id ASC", mySystem.Parameter.conn);
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
            dr["序号"] = 0;
            dr["T产品内包装记录ID"] = ID;
            //dr["生产开始时间"] = DateTime.Now;
            dr["生产结束时间"] = DateTime.Now;
            if (dt记录详情.Rows.Count >= 1)
            {
                int numtemp;
                dr["生产开始时间"] = dt记录详情.Rows[dt记录详情.Rows.Count - 1]["生产结束时间"];  //根据上一行填写
                if (Int32.TryParse(dt记录详情.Rows[dt记录详情.Rows.Count - 1]["内包序号"].ToString().Split('-')[1], out numtemp) == true)
                {
                    dr["内包序号"] = (numtemp + 1).ToString("D4");
                }
                DateTime end, start;
                start = Convert.ToDateTime(dr["生产开始时间"]);
                end = Convert.ToDateTime(dr["生产结束时间"]);
                dr["生产工时"] = Math.Round((end - start).TotalMinutes / 60.0, 1);
            }
            else
            {
                dr["生产开始时间"] = DateTime.Now; //新建第一行
                dr["生产工时"] = 0;
                dr["内包序号"] = 0;
            }    
            
            
            //dr["包装规格每包只数"] = 0;
            dr["产品数量包"] = 0;
            dr["产品数量只"] = 0;
            dr["热封线不合格数量"] = 0;
            dr["黑点晶点数量"] = 0;
            dr["其他数量"] = 0;
            dr["不良合计"] = 0;
            dr["包装袋热封线"] = "Yes";
            dr["内标签"] = "Yes";
            dr["内包装外观"] = "Yes";
            
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
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "包装袋热封线":
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
                    case "内标签":
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
                    case "内包装外观":
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
            dataGridView1.ColumnHeadersHeight = 40;
            //隐藏
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T产品内包装记录ID"].Visible = false;
            //不可用
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["审核员"].ReadOnly = true;
            dataGridView1.Columns["产品数量只"].ReadOnly = true;
            dataGridView1.Columns["不良合计"].ReadOnly = true;
            //HeaderText
            dataGridView1.Columns["产品数量包"].HeaderText = "产品数量\r(包)";
            dataGridView1.Columns["产品数量只"].HeaderText = "产品数量\r(只)";
            dataGridView1.Columns["热封线不合格数量"].HeaderText = "热封线\r不合格\r(只)";
            dataGridView1.Columns["黑点晶点数量"].HeaderText = "黑点\r晶点\r(只)";
            //dataGridView1.Columns["指示剂不良数量"].HeaderText = "指示剂\r不良\r(只)";
            dataGridView1.Columns["其他数量"].HeaderText = "其他\r(只)";
            dataGridView1.Columns["不良合计"].HeaderText = "不良\r合计\r(只)";
            dataGridView1.Columns["包装袋热封线"].HeaderText = "包装袋\r热封线";
            dataGridView1.Columns["内包装外观"].HeaderText = "内包装\r外观";
            dataGridView1.Columns["生产工时"].HeaderText = "生产工时\r(h)";
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DataShow(InstruID, cb产品代码.Text.ToString(), dtp生产日期.Value, Flight); }
        }

        //添加按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
            setEnableReadOnly();
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }
        
        //删除按钮
        private void btn删除记录_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //仅当审核人为空时，可删除
                if (dataGridView1.Rows[deletenum].Cells["审核员"].Value.ToString() == "")
                {
                    //dt记录详情.Rows.RemoveAt(deletenum);
                    dt记录详情.Rows[deletenum].Delete();

                    // 保存
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                    readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                    innerBind();

                    getTotalCol("热封线不合格数量", "tb热封线不合格合计");
                    getTotalCol("黑点晶点数量", "tb黑点晶点不合格合计");
                    //getTotalCol("指示剂不良数量", "tb指示剂不良合计");
                    getTotalCol("其他数量", "tb其他合计");
                    getTotalCol("不良合计", "tb不良合计");
                    getTotalCol("产品数量包", "tb产品数量包数合计A");
                    getTotalCol("产品数量只", "tb产品数量只数合计B");
                    getTotalColFloat("生产工时", "tb工时");

                    setDataGridViewRowNums();
                    setEnableReadOnly();
                }
            }
        }

        //内表移交审核按钮
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

        //内表审核按钮
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
            else if (TextBox_check() == false)
            {
                /*数字框不合格*/
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
                readOuterData(InstruID, cb产品代码.Text, dtp生产日期.Value, Flight);
                outerBind();

                setEnableReadOnly();

                return true;
            }
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
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
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='产品内包装记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);                     
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "产品内包装记录";
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
            checkform.ShowDialog();
        }

        //审核功能
        public override void CheckResult()
        {
            // TODO 内包入库，联产品入库
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
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='产品内包装记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\LDPEBag\5 SOP-MFG-109-R01A 产品内包装记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];
            // 修改Sheet中某行某列的值
            fill_excel(my, wb);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = Instruction + "-" + find_indexofprint().ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码


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
                    my = null; 
                }
                return pageCount;
            }
        }

        //打印填数据
        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            int ind = 0;
            if (dt记录详情.Rows.Count > 13)
            {
                //在第8行插入
                for (int i = 0; i < dt记录详情.Rows.Count - 13; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[12, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dt记录详情.Rows.Count - 13;
            }

            //外表信息
            mysheet.Cells[3, 11].Value = "生产指令编号：" + dt记录.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[4, 1].Value = dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[4, 4].Value = dt记录.Rows[0]["生产批号"].ToString();
            mysheet.Cells[4, 6].Value = Convert.ToDouble(dt记录.Rows[0]["包装规格每包只数"]);
            //mysheet.Cells[3, 15].Value = "标签：" + "中文" + (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否中文"]) == true ? "☑" : "□") + "  英文" + (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否英文"]) == true ? "☑" : "□");
            //mysheet.Cells[4, 9].Value = Convert.ToBoolean(dt记录.Rows[0]["标签语言是否中文"]) ? "中文" : "英文";
            mysheet.Cells[4, 9].Value = fill标签();
            mysheet.Cells[4, 14].Value = Convert.ToDateTime(dt记录.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日");
            mysheet.Cells[4, 16].Value = dt记录.Rows[0]["班次"].ToString();

            mysheet.Cells[20 + ind, 1].Value = "工时：" + dt记录.Rows[0]["工时"].ToString();
            mysheet.Cells[20 + ind, 4].Value = dt记录.Rows[0]["产品数量包合计A"].ToString();
            mysheet.Cells[20 + ind, 5].Value = dt记录.Rows[0]["产品数量只合计B"].ToString();
            mysheet.Cells[20 + ind, 6].Value = dt记录.Rows[0]["热封线不合格合计"].ToString();
            mysheet.Cells[20 + ind, 7].Value = dt记录.Rows[0]["黑点晶点不合格合计"].ToString();
            mysheet.Cells[20 + ind, 8].Value = dt记录.Rows[0]["指示剂不良合计"].ToString();
            mysheet.Cells[20 + ind, 9].Value = dt记录.Rows[0]["其他合计"].ToString();
            mysheet.Cells[20 + ind, 10].Value = dt记录.Rows[0]["不良合计"].ToString();
            mysheet.Cells[20 + ind, 10].Value = dt记录.Rows[0]["不良合计"].ToString();
            mysheet.Cells[20 + ind, 12].Value = dt记录.Rows[0]["废品重量"].ToString();

            mysheet.Cells[21 + ind, 6].Value = "其他操作人员：" + dt记录.Rows[0]["操作员备注"].ToString();
            //mysheet.Cells[20 + ind, 11].Value = "审核员：" + dt记录.Rows[0]["审核员"].ToString();

            //内表信息
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                mysheet.Cells[7 + i, 1] = i + 1;
                mysheet.Cells[7 + i, 2] = Convert.ToDateTime(dt记录详情.Rows[i]["生产开始时间"].ToString()).ToString("HH:mm:ss") + "~" + Convert.ToDateTime(dt记录详情.Rows[i]["生产结束时间"].ToString()).ToString("HH:mm:ss");
                mysheet.Cells[7 + i, 3] = dt记录详情.Rows[i]["内包序号"].ToString();
                //mysheet.Cells[7 + i, 4] = dt记录详情.Rows[i]["包装规格每包只数"].ToString(); 
                mysheet.Cells[7 + i, 4] = dt记录详情.Rows[i]["产品数量包"].ToString();
                mysheet.Cells[7 + i, 5] = dt记录详情.Rows[i]["产品数量只"].ToString();
                mysheet.Cells[7 + i, 6] = dt记录详情.Rows[i]["热封线不合格数量"].ToString();
                mysheet.Cells[7 + i, 7] = dt记录详情.Rows[i]["黑点晶点数量"].ToString();
                //mysheet.Cells[7 + i, 8] = dt记录详情.Rows[i]["指示剂不良数量"].ToString();
                mysheet.Cells[7 + i, 9] = dt记录详情.Rows[i]["其他数量"].ToString();
                mysheet.Cells[7 + i, 10] = dt记录详情.Rows[i]["不良合计"].ToString();
                mysheet.Cells[7 + i, 11] = dt记录详情.Rows[i]["包装袋热封线"].ToString().Equals("Yes") ? "√" : "×";
                mysheet.Cells[7 + i, 12] = dt记录详情.Rows[i]["内标签"].ToString().Equals("Yes") ? "√" : "×";
                mysheet.Cells[7 + i, 13] = dt记录详情.Rows[i]["内包装外观"].ToString().Equals("Yes") ? "√" : "×";
                mysheet.Cells[7 + i, 14] = dt记录详情.Rows[i]["生产工时"].ToString();
                mysheet.Cells[7 + i, 15] = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[7 + i, 16] = dt记录详情.Rows[i]["审核员"].ToString(); 

            }
        }

        private string fill标签()
        {
            string rtn = "";
            rtn += "中文";
            if (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否中文"]))
            {
                rtn += "☑";
            }
            else
            {
                rtn += "□";
            }
            rtn += "英文";
            if (Convert.ToBoolean(dt记录.Rows[0]["标签语言是否英文"]))
            {
                rtn += "☑";
            }
            else
            {
                rtn += "□";
            }
            return rtn;
        }
    
        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from " + table + " where 生产指令ID=" + InstruID.ToString();
            SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt记录.Rows[0]["ID"]) + 1;

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

        //求合计：列求合计
        private void getTotalCol(String ColName, String SumName)
        {
            //求合计
            int sum = 0;
            int numtemp;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i][ColName].ToString(), out numtemp) == true)
                { sum += numtemp; }
            }
            //dt记录.Rows[0]["产品数量只数合计B"] = sum;
            outerDataSync(SumName, sum.ToString());
        }

        private void getTotalColFloat(String ColName, String SumName)
        {
            //求合计
            float sum = 0;
            float numtemp;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (float.TryParse(dt记录详情.Rows[i][ColName].ToString(), out numtemp) == true)
                { sum += numtemp; }
            }
            //dt记录.Rows[0]["产品数量只数合计B"] = sum;
            outerDataSync(SumName, sum.ToString());
        }
        
        //求合计：行求合计，并对“不良合计”求和
        private void getTotalRow(Int32 RowCount)
        {
            //求合计
            int sum = 0;
            int numtemp;
            String[] ColName = { "热封线不合格数量", "黑点晶点数量", "其他数量" };
            for (int i = 0; i < 4; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[RowCount][(ColName[i])].ToString(), out numtemp) == true)
                { sum += numtemp; }
            }
            //dt记录.Rows[0]["产品数量只数合计B"] = sum;
            dt记录详情.Rows[RowCount]["不良合计"] = sum;
            getTotalCol("不良合计", "tb不良合计");
        }

        //求“产品数量只”，并且求只数合计
        private void getNum(Int32 RowCount)
        {
            int numBao, numPerBao;
            Int32.TryParse(tb包装规格每包只数.Text,out numPerBao);
            if (Int32.TryParse(dt记录详情.Rows[RowCount]["产品数量包"].ToString(), out numBao) == true)
                dt记录详情.Rows[RowCount]["产品数量只"] = numPerBao * numBao;
            getTotalCol("产品数量只", "tb产品数量只数合计B");
        }

        //求成品率
        private void getPercent()
        {
            //求合计
            int sumB = 0;
            int numtemp;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["产品数量只"].ToString(), out numtemp) == true)
                { sumB += numtemp; }
            }
            //dt记录.Rows[0]["产品数量只数合计B"] = sum;
            outerDataSync("tb产品数量只数合计B", sumB.ToString());            
            //求成品率
            int outC;
            // 膜卷长度求和
            if (Int32.TryParse(dt记录.Rows[0]["理论产量C"].ToString(), out outC) == true)
            {
                //均为数值类型
                if (outC == 0)
                {
                    //dt记录.Rows[0]["成品率"] = -1;
                    outerDataSync("tb成品率", "-1");
                }
                else
                {
                    //dt记录.Rows[0]["成品率"] = (Int32)((Double)sumB / (Double)outC * 100);
                    outerDataSync("tb成品率", ((Int32)((Double)sumB / (Double)outC * 100)).ToString());
                }
            }
            else
            {
                //dt记录详情.Rows[0]["成品率"] = -1;
                outerDataSync("tb成品率", "-1");
            }
            
        }

        //tb理论产量->成品率计算
        private void tb理论产量C_TextChanged(object sender, EventArgs e)
        {
            //if (dt记录详情 != null && dt记录详情.Rows.Count > 0)
            //{ getPercent(); }
        }        

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb废品重量 });
            List<String> StringList = new List<String>(new String[] { "理论产量" });
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

        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        //数据绑定结束，设置表格格式
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat();
            setEnableReadOnly();
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量包")
                {
                    getTotalCol("产品数量包", "tb产品数量包数合计A");
                    getNum(e.RowIndex);
                    /*getPercent();*/
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量只")
                //{
                //    getTotalCol("产品数量只", "tb产品数量只数合计A"); 
                //}
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "热封线不合格数量")
                {
                    getTotalCol("热封线不合格数量", "tb热封线不合格合计");
                    getTotalRow(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "黑点晶点数量")
                {
                    getTotalCol("黑点晶点数量", "tb黑点晶点不合格合计");
                    getTotalRow(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "其他数量")
                {
                    getTotalCol("其他数量", "tb其他合计");
                    getTotalRow(e.RowIndex);
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "不良合计")
                //{ getTotalCol("不良合计", "tb不良合计"); }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "操作员")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "生产开始时间" ||
                    dataGridView1.Columns[e.ColumnIndex].Name == "生产结束时间")
                {
                    DateTime end, start;
                    start = Convert.ToDateTime(dt记录详情.Rows[e.RowIndex]["生产开始时间"]);
                    end = Convert.ToDateTime(dt记录详情.Rows[e.RowIndex]["生产结束时间"]);
                    dt记录详情.Rows[e.RowIndex]["生产工时"] = Math.Round((end - start).TotalMinutes / 60.0, 1);
                    getTotalColFloat("生产工时", "tb工时");
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "内包序号")
                {
                    String[] t = dt记录详情.Rows[e.RowIndex]["内包序号"].ToString().Split('-');
                    try
                    {
                        int start = Convert.ToInt32(t[0]);
                        int end = Convert.ToInt32(t[1]);
                        dataGridView1.Rows[e.RowIndex].Cells["产品数量包"].Value = end - start + 1;
                        //dt记录详情.Rows[e.RowIndex]["产品数量包"] = end - start + 1;
                    }
                    catch
                    {
                        dt记录详情.Rows[e.RowIndex]["产品数量包"] = 0;
                    }
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "热封线不合格数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "黑点晶点数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "指示剂不良数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "其他数量")
                //{
                //    try
                //    {
                //        int sum = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["热封线不合格数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["黑点晶点数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["指示剂不良数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["其他数量"].Value);
                //        dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = sum;
                //    }
                //    catch
                //    {
                //        dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = 0;
                //    }
                //}
            }
        }

        private void LDPEBag_innerpackaging_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量包")
                {
                    getTotalCol("产品数量包", "tb产品数量包数合计A");
                    getNum(e.RowIndex);
                    /*getPercent();*/
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量只")
                //{
                //    getTotalCol("产品数量只", "tb产品数量只数合计A"); 
                //}
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "热封线不合格数量")
                {
                    getTotalCol("热封线不合格数量", "tb热封线不合格合计");
                    getTotalRow(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "黑点晶点数量")
                {
                    getTotalCol("黑点晶点数量", "tb黑点晶点不合格合计");
                    getTotalRow(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "其他数量")
                {
                    getTotalCol("其他数量", "tb其他合计");
                    getTotalRow(e.RowIndex);
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "不良合计")
                //{ getTotalCol("不良合计", "tb不良合计"); }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "操作员")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "生产开始时间" ||
                    dataGridView1.Columns[e.ColumnIndex].Name == "生产结束时间")
                {
                    DateTime end, start;
                    start = Convert.ToDateTime(dt记录详情.Rows[e.RowIndex]["生产开始时间"]);
                    end = Convert.ToDateTime(dt记录详情.Rows[e.RowIndex]["生产结束时间"]);
                    dt记录详情.Rows[e.RowIndex]["生产工时"] = Math.Round((end - start).TotalMinutes / 60.0, 1);
                    getTotalColFloat("生产工时", "tb工时");
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "内包序号")
                {
                    String[] t = dt记录详情.Rows[e.RowIndex]["内包序号"].ToString().Split('-');
                    try
                    {
                        int start = Convert.ToInt32(t[0]);
                        int end = Convert.ToInt32(t[1]);
                        dataGridView1.Rows[e.RowIndex].Cells["产品数量包"].Value = end - start + 1;
                        //dt记录详情.Rows[e.RowIndex]["产品数量包"] = end - start + 1;
                    }
                    catch
                    {
                        dt记录详情.Rows[e.RowIndex]["产品数量包"] = 0;
                    }
                }
                //else if (dataGridView1.Columns[e.ColumnIndex].Name == "热封线不合格数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "黑点晶点数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "指示剂不良数量" ||
                //    dataGridView1.Columns[e.ColumnIndex].Name == "其他数量")
                //{
                //    try
                //    {
                //        int sum = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["热封线不合格数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["黑点晶点数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["指示剂不良数量"].Value) +
                //            Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["其他数量"].Value);
                //        dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = sum;
                //    }
                //    catch
                //    {
                //        dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = 0;
                //    }
                //}
            }
        }
        

    }
}
