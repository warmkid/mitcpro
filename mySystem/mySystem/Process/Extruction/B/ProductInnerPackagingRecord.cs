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
using System.Text.RegularExpressions;

namespace mySystem.Extruction.Process
{
    public partial class ProductInnerPackagingRecord : BaseForm
    {
        private String table = "产品内包装记录表";
        private String tableInfo = "产品内包装详细记录";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt代码批号; 
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private SqlDataAdapter da记录sql, da记录详情sql;
        private SqlCommandBuilder cb记录sql, cb记录详情sql;

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
        bool isFirstBind = true; 

        public ProductInnerPackagingRecord(MainForm mainform): base(mainform)
        {
            

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.proInstruID;
            InitializeComponent();
            Instruction = Parameter.proInstruction;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            //查询条件可编辑
            cb产品代码.Enabled = true;
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
        }

        public ProductInnerPackagingRecord(MainForm mainform, Int32 ID) : base(mainform)
        {
            

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            SqlDataAdapter da = new SqlDataAdapter("select * from 产品内包装记录表 where ID=" + ID, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            InstruID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            InitializeComponent();
            da = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + InstruID, conn);
            dt = new DataTable();
            da.Fill(dt);
            Instruction = dt.Rows[0]["生产指令编号"].ToString();

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
            
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();

            DataTable dt = null;

            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜生产和检验记录表'", connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='吹膜生产和检验记录表'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            
            

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

        //读取设置内容  //GetProductInfo //产品代码、产品批号初始化
        private void getOtherData()
        {
            dt代码批号 = new DataTable("代码批号");

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (!isSqlOk)
            {
                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
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
                SqlCommand comm1 = new SqlCommand();
                comm1.Connection = Parameter.conn;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
                SqlDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    //查找该生产ID下的产品编码、产品批号
                    SqlCommand comm2 = new SqlCommand();
                    comm2.Connection = Parameter.conn;
                    comm2.CommandText = "select ID, 产品编码, 产品批号 from 生产指令产品列表 where 生产指令ID = " + reader1["ID"].ToString();
                    SqlDataAdapter datemp = new SqlDataAdapter(comm2);
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

            //*********数据填写*********//
            //cb产品代码.SelectedIndex = 0;
            //tb产品批号.Text = dt代码批号.Rows[0]["产品批号"].ToString();
            cb产品代码.SelectedIndex = -1;
            tb产品批号.Text = "";
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
                    setControlFalse();
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
            tb产品批号.ReadOnly = true;
            tb指示剂.ReadOnly = true;
            //查询条件始终不可编辑
            cb产品代码.Enabled = false;
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
            bt查看人员信息.Enabled = true;
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
        private void DataShow(Int32 InstruID, String productCode, DateTime searchTime)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productCode, searchTime);
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
                if (!mySystem.Parameter.isSqlOk)
                {
                    da记录.Update((DataTable)bs记录.DataSource);
                }
                else
                {
                    da记录sql.Update((DataTable)bs记录.DataSource);
                }
                
                //外表重新绑定
                readOuterData(InstruID, productCode, searchTime);
                outerBind();

                //********* 内表新建、保存、重新绑定 *********//

                //内表绑定
                //readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                //innerBind();
                //DataRow dr2 = dt记录详情.NewRow();
                //dr2 = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr2);
                //dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
                //setDataGridViewRowNums();
                ////立马保存内表
                //if (!mySystem.Parameter.isSqlOk)
                //{
                //    da记录详情.Update((DataTable)bs记录详情.DataSource);
                //}
                //else
                //{
                //    da记录详情sql.Update((DataTable)bs记录详情.DataSource);
                //}
                
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
            SqlDataAdapter da1 = new SqlDataAdapter("select * from " + table + " where ID = " + ID.ToString(), conn);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            { 
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), dt1.Rows[0]["产品代码"].ToString(), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString())); 
            }            
        }

        //****************************** 嵌套 ******************************//
        
        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode, DateTime searchTime)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                bs记录 = new BindingSource();
                dt记录 = new DataTable(table);
                da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
                cb记录 = new OleDbCommandBuilder(da记录);
                da记录.Fill(dt记录);
            }
            else
            {
                bs记录 = new BindingSource();
                dt记录 = new DataTable(table);
                da记录sql = new SqlDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 产品代码 = '" + productCode + "' and 生产日期 = '" + searchTime.ToString("yyyy/MM/dd") + "' ", mySystem.Parameter.conn);
                cb记录sql = new SqlCommandBuilder(da记录sql);
                da记录sql.Fill(dt记录);
            }
           
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
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
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
            tb操作员备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Add("Text", bs记录.DataSource, "操作员备注");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["产品代码"] = cb产品代码.Text;
            dr["产品批号"] = dt代码批号.Rows[cb产品代码.FindString(cb产品代码.Text)]["产品批号"].ToString();
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["标签语言是否中文"] = true;
            dr["标签语言是否英文"] = true;
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = Convert.ToDateTime(dtp操作日期.Value.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(dtp审核日期.Value.ToString("yyyy/MM/dd"));
            dr["操作员备注"] = "无";
            dr["审核是否通过"] = false;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + Instruction + "\n";
            dr["日志"] = log;            
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                bs记录详情 = new BindingSource();
                dt记录详情 = new DataTable(tableInfo);
                da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T产品内包装记录ID = " + ID.ToString(), connOle);
                cb记录详情 = new OleDbCommandBuilder(da记录详情);
                da记录详情.Fill(dt记录详情);
            }
            else
            {
                bs记录详情 = new BindingSource();
                dt记录详情 = new DataTable(tableInfo);
                da记录详情sql = new SqlDataAdapter("select * from " + tableInfo + " where T产品内包装记录ID = " + ID.ToString(), mySystem.Parameter.conn);
                cb记录详情sql = new SqlCommandBuilder(da记录详情sql);
                da记录详情sql.Fill(dt记录详情);
            }
            
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
            //DataRow dr = dt记录详情.NewRow();
            dr["序号"] = 0;
            dr["T产品内包装记录ID"] = ID;
            dr["生产时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            dr["内包序号"] = 0;
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
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "产品外观":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("0");
                        cbc.Items.Add("1");
                        cbc.Items.Add("2");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    case "包装后外观":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("0");
                        cbc.Items.Add("1");
                        cbc.Items.Add("2");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    case "包装袋热封线":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("0");
                        cbc.Items.Add("1");
                        cbc.Items.Add("2");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    case "贴标签":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("0");
                        cbc.Items.Add("1");
                        cbc.Items.Add("2");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //cbc.MinimumWidth = 120;
                        break;
                    case "贴指示剂":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("0");
                        cbc.Items.Add("1");
                        cbc.Items.Add("2");
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
                        //tbc.MinimumWidth = 80;
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
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T产品内包装记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DataShow(InstruID, cb产品代码.Text.ToString(), dtp生产日期.Value); }
        }

        //添加按钮
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
                if (!mySystem.Parameter.isSqlOk)
                {
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                }
                else
                {
                    da记录详情sql.Update((DataTable)bs记录详情.DataSource);
                }
                
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                setDataGridViewRowNums();
            }
        }
        
        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
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
                /*包装人不合格*/
                return false;
            }
            else if (mySystem.Parameter.NametoID(tb操作人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『操作员』信息", "ERROR");
                return false;
            }
            else if (TextBox_check() == false)
            { 
                /*各种数量填写不合格*/
                return false;
            }
            else
            {
                // 内表保存
                if (!mySystem.Parameter.isSqlOk)
                {
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                }
                else
                {
                    da记录详情sql.Update((DataTable)bs记录详情.DataSource);
                }
                
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                //外表保存
                bs记录.EndEdit();
                if (!mySystem.Parameter.isSqlOk)
                {
                    da记录.Update((DataTable)bs记录.DataSource);
                }
                else
                {
                    da记录sql.Update((DataTable)bs记录.DataSource);
                }
                
                readOuterData(InstruID, cb产品代码.Text, dtp生产日期.Value);
                outerBind();

                return true;
            }
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }



            if (!checkInnerData(dataGridView1))
            {
                MessageBox.Show("请填写完整的表单信息", "提示");
                return;
            }
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜产品内包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜产品内包装记录表";
                    dr["对应ID"] = (int)dt记录.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                da_temp.Update(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='吹膜产品内包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜产品内包装记录表";
                    dr["对应ID"] = (int)dt记录.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                da_temp.Update(dt_temp);
            }
            

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

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录.Rows[0]["操作人"].ToString())
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

            dt记录.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜产品内包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='吹膜产品内包装记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
           

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
            try
            {
                (this.Owner as mySystem.Query.QueryExtruForm).search();
            }
            catch (NullReferenceException exp) { }
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
        public int print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-109-R01A 产品内包装记录.xlsx");
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
                        if (!mySystem.Parameter.isSqlOk)
                        {
                            da记录.Update((DataTable)bs记录.DataSource);
                        }
                        else
                        {
                            da记录sql.Update((DataTable)bs记录.DataSource);
                        }

                    }
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
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

        //打印填数据
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
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
            int rownum = dt记录详情.Rows.Count;
            for (int i = 0; i < (rownum > 12 ? 12 : rownum); i++)
            {
                mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                mysheet.Cells[5 + i, 2].Value = dt记录详情.Rows[i]["生产时间"].ToString();
                mysheet.Cells[5 + i, 3].Value = dt记录详情.Rows[i]["内包序号"].ToString();
                mysheet.Cells[5 + i, 4].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["产品外观"].ToString()));
                mysheet.Cells[5 + i, 5].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装后外观"].ToString()));
                mysheet.Cells[5 + i, 6].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装袋热封线"].ToString()));
                mysheet.Cells[5 + i, 7].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴标签"].ToString()));
                mysheet.Cells[5 + i, 8].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴指示剂"].ToString()));
                mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["包装人"].ToString();
            }
            //需要插入的部分
            if (rownum > 12)
            {
                for (int i = 12; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[5 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);

                    mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                    mysheet.Cells[5 + i, 2].Value = dt记录详情.Rows[i]["生产时间"].ToString();
                    mysheet.Cells[5 + i, 3].Value = dt记录详情.Rows[i]["内包序号"].ToString();
                    mysheet.Cells[5 + i, 4].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["产品外观"].ToString()));
                    mysheet.Cells[5 + i, 5].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装后外观"].ToString()));
                    mysheet.Cells[5 + i, 6].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["包装袋热封线"].ToString()));
                    mysheet.Cells[5 + i, 7].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴标签"].ToString()));
                    mysheet.Cells[5 + i, 8].Value = okayInt2String(Convert.ToInt32(dt记录详情.Rows[i]["贴指示剂"].ToString()));
                    mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["包装人"].ToString();
                }
            }
            //加页脚
            int sheetnum;
            SqlDataAdapter da = new SqlDataAdapter("select ID from " + table + " where 生产指令ID=" + InstruID.ToString(), Parameter.conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            mysheet.PageSetup.RightFooter = Instruction + "-15-" + sheetnum.ToString("D3") + " / &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
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
        
        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
                String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
                String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString();
                String val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误：" + val);
            //MessageBox.Show("")
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
                        dt记录详情.Rows[e.RowIndex]["包装人"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『包装人』信息", "ERROR");
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
                isFirstBind = false;
            }
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜产品内包装记录表'", mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='吹膜产品内包装记录表'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            
        }

        private void ProductInnerPackagingRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
        }

        private void ProductInnerPackagingRecord_Load(object sender, EventArgs e)
        {
            String sql1 = "select * from 吹膜供料记录 where 生产指令ID ={0}";
            SqlDataAdapter da = new SqlDataAdapter(String.Format(sql1, InstruID), mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("请先填写吹膜供料记录！", "提示");
                this.Close();
                //return;
            }
        }



        public ProductInnerPackagingRecord(MainForm mainform, Int32 ID, bool forprint)
            : base(mainform)
        {


            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            SqlDataAdapter da = new SqlDataAdapter("select * from 产品内包装记录表 where ID=" + ID, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            InstruID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            InitializeComponent();
            da = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + InstruID, conn);
            dt = new DataTable();
            da.Fill(dt);
            Instruction = dt.Rows[0]["生产指令编号"].ToString();

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            //setUserState();  // 根据登录人，设置stat_user
            _userState = Parameter.UserState.NoBody;
            //getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            IDShow(ID);
        }


    }
}
