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
    public partial class BTV2DProRecord : BaseForm
    { 
        /*
         * BECAUSE THE DATABASE HAS NOT BENN UPDATE, YOU HAVE TO MODIFY THE DATABASE
         * 1. ADD THE "检测时间" COLUMN 
         * 2. FOR BETTER 
        */
        /// <summary>
        /// 2D袋体生产记录
        /// </summary>
        private String table = "2D袋体生产记录";
        /// <summary>
        /// 2D袋体生产记录详细信息
        /// </summary>
        private String tableInfo = "2D袋体生产记录详细信息";

        private SqlConnection conn = null;
        //private OleDbConnection mySystem.Parameter.conn = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情, dt代码批号, dt膜代码, dt物料;
        private SqlDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private SqlCommandBuilder cb记录, cb记录详情;
        private int[] sum = { 0, 0 };

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        String MoCode;

        public BTV2DProRecord(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            //mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.bpvbagInstruID;
            Instruction = Parameter.bpvbagInstruction;


            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
        }

        public BTV2DProRecord(MainForm mainform, int ID)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            //mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherDataLocal();  //读取设置内容
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
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='" + table + "';", mySystem.Parameter.conn);
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
            dt膜代码 = new DataTable("膜代码");
            dt代码批号 = new DataTable("代码批号");
            dt物料 = new DataTable("物料");

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (isSqlOk)
            {
                //查找该生产ID下的生产时间
                DateTime 计划生产日期;
                string sqlStr = "SELECT 计划生产日期 FROM 生产指令 WHERE ID = " + InstruID + ";";
                SqlCommand get生产指令详细信息 = new SqlCommand(sqlStr, mySystem.Parameter.conn);
                try
                {
                    计划生产日期 = Convert.ToDateTime(get生产指令详细信息.ExecuteScalar());
                    //dtp生产日期.Value = 计划生产日期;
                }
                catch
                {
                    MessageBox.Show("该生产指令编码下的『计划生产日期』尚未填写");
                }

                SqlCommand comm1 = new SqlCommand();
                comm1.Connection = mySystem.Parameter.conn;
                comm1.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
                DataTable dt生产指令 = new DataTable("生产指令");
                SqlDataAdapter datemp1 = new SqlDataAdapter(comm1);
                datemp1.Fill(dt生产指令);
                if (dt生产指令.Rows.Count == 0)
                {
                    MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！");
                }
                else
                {
                    //TO ASK : IS THIS PART HAND ADDED? IS DATABASE UNAVAILIBLE NOW? AND HOW CAN PROGRAM KNOWS HOW MANY ROWS SHOULD BE?
                    //外表物料
                    dt物料.Columns.Add("物料简称", typeof(String));   //新建第1列
                    dt物料.Columns.Add("物料代码", typeof(String));   //新建第2列
                    dt物料.Columns.Add("物料批号", typeof(String));   //新建第3列
                    //dt物料.Rows.Add(dt生产指令.Rows[0]["制袋物料名称1"].ToString(), dt生产指令.Rows[0]["制袋物料代码1"].ToString(), dt生产指令.Rows[0]["制袋物料批号1"].ToString());
                    //dt物料.Rows.Add(dt生产指令.Rows[0]["制袋物料名称2"].ToString(), dt生产指令.Rows[0]["制袋物料代码2"].ToString(), dt生产指令.Rows[0]["制袋物料批号2"].ToString());
                    //dt物料.Rows.Add(dt生产指令.Rows[0]["制袋物料名称3"].ToString(), dt生产指令.Rows[0]["制袋物料代码3"].ToString(), dt生产指令.Rows[0]["制袋物料批号3"].ToString());
                    dt物料.Rows.Add(dt生产指令.Rows[0]["内包物料名称1"].ToString(), dt生产指令.Rows[0]["内包物料代码1"].ToString(), dt生产指令.Rows[0]["内包物料批号1"].ToString());
                    dt物料.Rows.Add(dt生产指令.Rows[0]["内包物料名称2"].ToString(), dt生产指令.Rows[0]["内包物料代码2"].ToString(), dt生产指令.Rows[0]["内包物料批号2"].ToString());
                    dt物料.Rows.Add(dt生产指令.Rows[0]["外包物料名称1"].ToString(), dt生产指令.Rows[0]["外包物料代码1"].ToString(), dt生产指令.Rows[0]["外包物料批号1"].ToString());
                    dt物料.Rows.Add(dt生产指令.Rows[0]["外包物料名称2"].ToString(), dt生产指令.Rows[0]["外包物料代码2"].ToString(), dt生产指令.Rows[0]["外包物料批号2"].ToString());
                    dt物料.Rows.Add(dt生产指令.Rows[0]["外包物料名称3"].ToString(), dt生产指令.Rows[0]["外包物料代码3"].ToString(), dt生产指令.Rows[0]["外包物料批号3"].ToString());
                    addMaterialToDt();


                    //内表代码批号
                    SqlCommand comm2 = new SqlCommand();
                    comm2.Connection = mySystem.Parameter.conn;
                    comm2.CommandText = "select * from 生产指令详细信息 where T生产指令ID = " + dt生产指令.Rows[0]["ID"].ToString();
                    DataTable dttemp = new DataTable("dttemp");
                    SqlDataAdapter datemp2 = new SqlDataAdapter(comm2);
                    datemp2.Fill(dttemp);
                    if (dttemp.Rows.Count == 0)
                    { MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！"); }
                    else
                    {
                        dt代码批号.Columns.Add("产品代码", typeof(String));   //新建第一列
                        dt代码批号.Columns.Add("产品批号", typeof(String));      //新建第二列
                        dt代码批号.Rows.Add(dttemp.Rows[0]["产品代码"].ToString(), dttemp.Rows[0]["产品批号"].ToString());
                        tb产品代码.Text = dttemp.Rows[0]["产品代码"].ToString();
                        tb产品批号.Text = dttemp.Rows[0]["产品批号"].ToString();
                    }
                }
                datemp1.Dispose();
                
            }
            else
            {
                //从SQL数据库中读取;                
            }
        }
        private void addMaterialToDt()
        {
            SqlDataAdapter daGetMaterial = new SqlDataAdapter("select * from 生产指令物料 where T生产指令ID =" + InstruID, mySystem.Parameter.conn);
            DataTable dtResult = new DataTable();
            daGetMaterial.Fill(dtResult);
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                DataRow dr = dt物料.NewRow();
                dr["物料简称"] = dtResult.Rows[i]["物料名称"];
                dr["物料代码"] = dtResult.Rows[i]["物料代码"];
                dr["物料批号"] = dtResult.Rows[i]["物料批号"];
                dt物料.Rows.Add(dr);
            }
            daGetMaterial.Dispose();
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
                    btn数据审核.Enabled = true;
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
            tb审核员.Enabled = false;
            btn提交数据审核.Enabled = false;
            btn数据审核.Enabled = false;

            //部分空间防作弊，不可改
            tb产品批号.ReadOnly = true;
            tb产品代码.ReadOnly = true;

            
            //查询条件始终不可编辑 instruction and 生产日期 both decide

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


        // ANOTHER EDITION: ADD TIME WHNE SEARCH
        private void DataShow(Int32 InstruID, DateTime _生产日期)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, _生产日期);
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
                readOuterData(InstruID, _生产日期);
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
            SqlDataAdapter da1 = new SqlDataAdapter("select * from [" + table + "] where ID = " + ID.ToString(), mySystem.Parameter.conn);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                //DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()));
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + ";", mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //ANOTHER EDITION
        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from [" + table + "] where 生产指令ID = " + InstruID.ToString() + " and 生产日期 = '" + searchTime.ToString("yyyy/MM/dd") + "' ", mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            tb产品代码.DataBindings.Clear();
            tb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            tb合格品数量.DataBindings.Clear();
            tb合格品数量.DataBindings.Add("Text", bs记录.DataSource, "合格品数量");
            tb不良品数量.DataBindings.Clear();
            tb不良品数量.DataBindings.Add("Text", bs记录.DataSource, "不良品数量");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
            //dtp审核日期.DataBindings.Clear();
            //dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            //dr["生产指令编号"] = Instruction;
            dr["产品代码"] = tb产品代码.Text;
            dr["产品批号"] = tb产品批号.Text;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));

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
            da记录详情 = new SqlDataAdapter("select * from [" + tableInfo + "] where T" + table + "ID = " + ID.ToString(), mySystem.Parameter.conn);
            cb记录详情 = new SqlCommandBuilder(da记录详情);
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
            //TO DO : CONFIRM THAT IS THERE EXIST 不良品数量
            //DataRow dr = dt记录详情.NewRow();
            dr["T" + table + "ID"] = ID;
            dr["序号"] = 0;
            dr["生产时间"] = DateTime.Now;
            dr["切片尺寸长"] = 0;
            dr["切片尺寸宽"] = 0;
            dr["袋体外观检查"] = "√";
            dr["袋体尺寸确认"] = "√";
            dr["热封线检查"] = "√";
            
            dr["合格品数量"] = 0;
            dr["不良品数量"] = 0;
            
            dr["操作员"] = mySystem.Parameter.userName;
            //dr["操作员备注"] = "";
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
            DataTable setWidth = new DataTable("setWidth");
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                setWidth.Columns.Add(dc.ColumnName, Type.GetType("System.Int32"));
            }
            DataRow nums = setWidth.NewRow();
            nums["ID"] = 50;
            nums["T" + table + "ID"] = 50;
            nums["序号"] = 50;
            nums["生产时间"] = 150;
            nums["切片尺寸长"] = 70;
            nums["切片尺寸宽"] = 65;
            nums["袋体外观检查"] = 65;
            nums["袋体尺寸确认"] = 65;
            nums["热封线检查"] = 65;
            nums["合格品数量"] = 65;
            nums["不良品数量"] = 65;
            nums["操作员"] = 75;
            //nums["操作员备注"] = 100;
            nums["审核员"] = 50;
            setWidth.Rows.Add(nums);
            DataGridViewTextBoxColumn tbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                if ("设定" == dc.ColumnName)
                {
                    continue;
                }
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
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = Convert.ToInt32(setWidth.Rows[0][tbc.Name]);
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
            dataGridView1.Columns["T" + table + "ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["审核员"].ReadOnly = true;


            dataGridView1.Columns["合格品数量"].HeaderText = "合格品\r数量\r(只)";
            dataGridView1.Columns["不良品数量"].HeaderText = "不良品\r数量\r(只)";
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

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            DataShow(InstruID, dtp生产日期.Value.Date);
        }

        //添加行按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
        }

        //删除行按钮
        private void btn删除记录_Click(object sender, EventArgs e)
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


        //内表移交审核按钮
        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    dt记录详情.Rows[i]["审核员"] = "__待审核";
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();

            // ADDTION: WHEN 提交数据审核, WE WANT 提交审核 ID STILL ACTIVE AFTER SET ENABLEREADONLY
            btn提交审核.Enabled = true;
        }

        //内表审核按钮
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            HashSet<Int32> hi待审核行号 = new HashSet<int>();
            foreach (DataGridViewCell dgvc in dataGridView1.SelectedCells)
            {
                hi待审核行号.Add(dgvc.RowIndex);
            }
            foreach (int i in hi待审核行号)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    dt记录详情.Rows[i]["审核员"] = mySystem.Parameter.userName;
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();




            // ADDTION: WHEN 数据审核, WE WANT 审核 ID STILL ACTIVE AFTER SET ENABLEREADONLY
            btn审核.Enabled = true;
        }


        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
            {
                btn提交审核.Enabled = true;
                btn提交数据审核.Enabled = true;
            }
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
                /*不良品数量*/
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
                readOuterData(InstruID, dtp生产日期.Value);
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
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='" + table + "' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = table;
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
            //dt记录.Rows[0]["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString());
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='" + table + "' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
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
            print(true);
            GC.Collect();
        }
        /// <summary>
        /// this print function has been updated by pool on 2017-11-01
        /// but hasn't been tested on printer
        /// this edition of function automacally insert new rows according to settings
        /// </summary>
        /// <param name="preview"></param>
        public void print(bool preview)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\14 SOP-MFG-502-R01A  BPV袋体生产记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[5];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值
            int rowStartAt = 6;
            my.Cells[3, 1].Value = "产品代码：" + dt记录.Rows[0]["产品代码"];
            my.Cells[3, 4].Value = "产品批号：" + dt记录.Rows[0]["产品批号"];
            my.Cells[3, 9].Value = "生产指令：" + getInsFromID(Convert.ToInt32(dt记录.Rows[0]["生产指令ID"]));

            int rowNumPerSheet = 15;
            int rowNumTotal = dt记录详情.Rows.Count;
            if (rowNumTotal > rowNumPerSheet)
            {
                for (int i = 0; i < rowNumTotal - rowNumPerSheet; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                }
            }
            for (int i = 0; i < rowNumTotal; i++)
            {
                my.Cells[i + rowStartAt, 1].Value = dt记录详情.Rows[i]["序号"];
                my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产时间"]).ToString("MM/dd HH:mm");
                my.Cells[i + rowStartAt, 2].Font.Size = 11;
                my.Cells[i + rowStartAt, 3].Value = "";
                my.Cells[i + rowStartAt, 3].Value = dt记录详情.Rows[i]["切片尺寸长"];
                my.Cells[i + rowStartAt, 4].Value = dt记录详情.Rows[i]["切片尺寸宽"];
                my.Cells[i + rowStartAt, 7].Value = dt记录详情.Rows[i]["袋体外观检查"];
                my.Cells[i + rowStartAt, 5].Value = dt记录详情.Rows[i]["袋体尺寸确认"];
                my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["热封线检查"];
                my.Cells[i + rowStartAt, 8].Value = dt记录详情.Rows[i]["合格品数量"];
                my.Cells[i + rowStartAt, 9].Value = dt记录详情.Rows[i]["不良品数量"];
                my.Cells[i + rowStartAt, 10].Value = dt记录详情.Rows[i]["操作员"];
                my.Cells[i + rowStartAt, 11].Value = dt记录详情.Rows[i]["审核员"];
            }
            int varOffset = (rowNumTotal > rowNumPerSheet) ? rowNumTotal - rowNumPerSheet : 0;
            my.Cells[21 + varOffset, 8].Value = dt记录.Rows[0]["合格品数量"];
            my.Cells[21 + varOffset, 9].Value = dt记录.Rows[0]["不良品数量"];

            if (preview)
            {
                my.Select();
                oXL.Visible = true; //加上这一行  就相当于预览功能            
            }
            else
            {
                //add footer
                my.PageSetup.RightFooter = Instruction + "-10-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch { }
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源

                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
                oXL = null;
                my = null;
                wb = null;
            }
        }


        int find_indexofprint()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from " + table + " where 生产指令ID=" + InstruID, mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
        }
		

        //******************************小功能******************************//  

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

        //内表审核按钮
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

        //求合计
        private void getTotal()
        {
            int numtemp;
            // 膜卷长度求和
            sum[0] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["合格品数量"].ToString(), out numtemp) == true)
                { sum[0] += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            outerDataSync("tb合格品数量", sum[0].ToString());
            // 膜卷重量求和
            sum[1] = 0;

            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["不良品数量"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];
            outerDataSync("tb不良品数量", sum[1].ToString());

        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] {  });
            List<String> StringList = new List<String>(new String[] { });
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
                if (dataGridView1.Columns[e.ColumnIndex].Name == "检测产品数量")
                {
                    getTotal();
                }

                else if (dataGridView1.Columns[e.ColumnIndex].Name == "不良品数量")
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
        private string getInsFromID(int ID)
        {
            string rtn;
            string sqlCmd = "SELECT 生产指令编号 FROM 生产指令 WHERE ID =" + ID.ToString();
            SqlCommand cmd = new SqlCommand(sqlCmd, mySystem.Parameter.conn);
            try
            {
                rtn = Convert.ToString(cmd.ExecuteScalar());
            }
            catch
            {
                MessageBox.Show("未找到对应生产指令编码!");
                rtn = "";
            }
            return rtn;
        }

        
    }
}
