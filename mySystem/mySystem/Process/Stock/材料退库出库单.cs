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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace mySystem.Process.Stock
{
    public partial class 材料退库出库单 : BaseForm
    {
        
        private String table;
        private String tableInfo;//退库或出库详细信息
        private String tableInfo_二维码;//退库或出库二维码信息

        //HashSet<string> hs_cach;//存储已扫描的二维码
        //private Dictionary<string, string> dic_二维码;//
        private Dictionary<string, int> dic_材料代码;//
        private HashSet<string> hs_二维码;
        //private HashSet<string> hs_材料代码;

        private int NUM;//当鼠标刚进入单元格时，获取二维码对应出库或退库数量，用于更新

        private SqlConnection conn = null;
        //private OleDbConnection mySystem.Parameter.conn = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情,dt二维码信息;
        private SqlDataAdapter da记录, da记录详情, da二维码信息;
        private BindingSource bs记录, bs记录详情, bs二维码信息;
        private SqlCommandBuilder cb记录, cb记录详情, cb二维码信息;
        
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;

        private int label;//退库还是出库单，1代表退库，2代表出库
        Int32 InstruID;
        String Instruction;

        bool isFirstBind = true;
        bool isFirstBind2 = true;
        bool isFirstBind3 = true;

        public 材料退库出库单(MainForm mainform)
            : base(mainform)
        {
        }

        public 材料退库出库单(MainForm mainform, Int32 ID,int label)
            : base(mainform)
        {
            InitializeComponent();
            variableInit(label);

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            IDShow(ID);
            check库存();
        }
        //生产材料退库单或者材料出库单
        //dtOut中应包含生产指令ID，生产指令编码，产品代码，产品批号
        //dtInner中应包含对应退库单或出库单内表的信息
        static public bool 生成表单(int label,DataTable dtOut,DataTable dtInner,string str工序)
        {
            Int32 外表ID = 0;
            string temptable,temptable2;
            if (1 == label)
            {
                temptable = "材料退库单";
                temptable2 = "材料退库单详细信息";
            }

            else
            {
                temptable = "材料出库单";
                temptable2 = "材料出库单详细信息";
            }
                
//            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
            //SqlConnection conn = mySystem.Parameter.conn;
            string strCon = @"server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();      

            //写入外表
            SqlDataAdapter daout_reader;
            DataTable dt_reader = new DataTable();
            SqlCommandBuilder cb_reader;

            DataTable dt_temp = new DataTable("外表");
            BindingSource bsout = new BindingSource();
            SqlDataAdapter daout = new SqlDataAdapter("select * from " + temptable + " where 1 = 2", conn);
            SqlCommandBuilder cbout = new SqlCommandBuilder(daout);
            daout.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["生产指令ID"] = dtOut.Rows[0]["生产指令ID"];
                dr["生产指令编号"] = dtOut.Rows[0]["生产指令编号"];
                dr["产品代码"] = dtOut.Rows[0]["产品代码"];
                dr["产品批号"] = dtOut.Rows[0]["产品批号"];
                dr["属于工序"] = str工序;

                //无关项，但是需要给出默认值
                dr["审核是否通过"] = false;
                dr["审核日期"] = DateTime.Now;
                dt_temp.Rows.Add(dr);
                daout.Update(dt_temp);

                ////获得外表ID
                //DataTable dt_temp2 = new DataTable();
                //BindingSource bsout2 = new BindingSource();
                //SqlDataAdapter daout2 = new SqlDataAdapter("select ID from " + temptable + " where 生产指令ID=" + dtOut.Rows[0]["生产指令ID"].ToString() + " and 属于工序=" + str工序, conn);
                //SqlCommandBuilder cbout2 = new SqlCommandBuilder(daout2);
                //daout2.Fill(dt_temp2);

                //if (dt_temp2.Rows.Count <= 1)
                //    return false;
                //外表ID = int.Parse(dt_temp2.Rows[0]["ID"].ToString());


                daout_reader = new SqlDataAdapter("select top 1 * from " + temptable + " where 生产指令ID=" + dtOut.Rows[0]["生产指令ID"].ToString() + " and 属于工序='" + str工序+"' order by ID DESC", conn);
                cb_reader = new SqlCommandBuilder(daout_reader);
                daout_reader.Fill(dt_reader);
                if (dt_reader.Rows.Count <= 0)
                    return false;
            }
            if (dtInner.Rows.Count > 0)
            {
                //写入内表
                DataTable dt_temp2 = new DataTable("内表");
                BindingSource bsin = new BindingSource();
                SqlDataAdapter dain = new SqlDataAdapter("select * from " + temptable2 + " where 1 = 2", conn);
                SqlCommandBuilder cbin = new SqlCommandBuilder(dain);
                dain.Fill(dt_temp2);
                if (dt_temp2.Rows.Count == 0)
                {
                    for (int i = 0; i < dtInner.Rows.Count; i++)
                    {
                        // 去重
                        String daima = dtInner.Rows[i]["物料代码"].ToString();
                        string pihao = dtInner.Rows[i]["物料批号"].ToString();
                        DataRow[] existing = dt_temp2.Select(String.Format("物料代码='{0}' and 物料批号='{1}'", daima, pihao));
                        if (existing.Count() > 0)
                        {
                            existing[0]["发料数量"] = Convert.ToDouble(existing[0]["发料数量"]) + Convert.ToDouble(dtInner.Rows[i]["发料数量"]);
                            continue;
                        }
                        //  -----
                        DataRow dr = dt_temp2.NewRow();
                        if(1==label)//退库单
                        {
                            dr["T材料退库单ID"] = dt_reader.Rows[0]["ID"];
                            dr["退库日期时间"] = dtInner.Rows[i]["退库日期时间"];
                            dr["退库数量"] = dtInner.Rows[i]["退库数量"];
                        }
                            
                        else//出库单
                        {
                            dr["T材料出库单ID"] = dt_reader.Rows[0]["ID"];
                            dr["出库日期时间"] = dtInner.Rows[i]["出库日期时间"];
                            dr["发料数量"] = dtInner.Rows[i]["发料数量"];
                        }

                        dr["序号"] = i+1;
                        //dr["班次"] = dt_reader.Rows[i]["班次"];
                        dr["物料代码"] = dtInner.Rows[i]["物料代码"];
                        dr["物料批号"] = dtInner.Rows[i]["物料批号"];

                        dt_temp2.Rows.Add(dr);                       
                    }
                    dain.Update(dt_temp2);                  
                }
            }
            conn.Close();
            return true ;
        }

        void variableInit(int label)
        {
            conn = Parameter.conn;
            //mySystem.Parameter.conn = mySystem.Parameter.conn;
            isSqlOk = Parameter.isSqlOk;
            this.label = label;

            //dic_二维码 = new Dictionary<string, string>();
            dic_材料代码 = new Dictionary<string, int>();
            hs_二维码 = new HashSet<string>();

            for (int i = 0; i < 4; i++)
            {
                dataGridView3.Columns.Add(new DataGridViewTextBoxColumn());
            }
            dataGridView3.Columns[0].Name = "序号";
            dataGridView3.Columns[1].Name = "物料代码";
            dataGridView3.Columns[2].Name = "物料批号";
            dataGridView3.Columns[3].Name = "数量";
                
            if (1 == label)
            {
                lb标题.Text = "材料退库单";
                table = "材料退库单";
                tableInfo = "材料退库单详细信息";
                tableInfo_二维码 = "材料退库单二维码信息";

                dataGridView3.Columns[3].HeaderText = "退库数量";
            }
            else
            {
                lb标题.Text = "材料出库单";
                table = "材料出库单";
                tableInfo = "材料出库单详细信息";
                tableInfo_二维码 = "材料出库单二维码信息";

                dataGridView3.Columns[3].HeaderText= "出库数量";
            }
            dataGridView3.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView3.Columns[4].Name = "库存ID";//用于写入二维码信息表

            dataGridView3.Columns[4].Visible = false;

            if (isFirstBind3)
            {
                readDGVWidthFromSettingAndSet(dataGridView3);
                isFirstBind3 = false;
            }

        }

        //******************************初始化******************************//

        // 获取操作员和审核员
        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='"+table+"'", mySystem.Parameter.conn);
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
                if (_formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.审核未通过 || _formState == Parameter.FormState.未保存)  //2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                //else if (_formState == Parameter.FormState.未保存)//0未保存
                //{
                //    //控件都不能点，只有打印,日志可点
                //    setControlFalse();
                //    btn数据审核.Enabled = true;
                //    //遍历datagridview，如果有一行为待审核，则该行可以修改
                //    dataGridView1.ReadOnly = false;
                //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //    {
                //        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                //            dataGridView1.Rows[i].ReadOnly = false;
                //        else
                //            dataGridView1.Rows[i].ReadOnly = true;
                //    }
                //}
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;

                    //btn数据审核.Enabled = true;
                    ////遍历datagridview，如果有一行为待审核，则该行可以修改
                    //dataGridView1.ReadOnly = false;
                    //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //{
                    //    if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                    //        dataGridView1.Rows[i].ReadOnly = false;
                    //    else
                    //        dataGridView1.Rows[i].ReadOnly = true;
                    //}
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

                    //btn提交数据审核.Enabled = true;
                    ////遍历datagridview，如果有一行为未审核，则该行可以修改
                    //dataGridView1.ReadOnly = false;
                    //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //{
                    //    if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                    //        dataGridView1.Rows[i].ReadOnly = true;
                    //    else
                    //        dataGridView1.Rows[i].ReadOnly = false;
                    //}
                }
                else //3审核未通过
                {
                    //发送审核，审核不能点
                    setControlTrue();

                    //btn提交数据审核.Enabled = true;
                    ////遍历datagridview，如果有一行为未审核，则该行可以修改
                    //dataGridView1.ReadOnly = false;
                    //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //{
                    //    if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                    //        dataGridView1.Rows[i].ReadOnly = true;
                    //    else
                    //        dataGridView1.Rows[i].ReadOnly = false;
                    //}
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
            tb审核员.Enabled = false;
            btn提交审核.Enabled = false;           
            btn数据审核.Enabled = false;
            btn提交数据审核.Enabled = false;
            btn审核.Enabled = false;
            //部分空间防作弊，不可改
            tb生产指令编号.ReadOnly = true;
            tb产品代码.ReadOnly = true;
            tb产品批号.ReadOnly = true;
            //查询条件始终不可编辑
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

            dataGridView2.DataError += dataGridView2_DataError;
            dataGridView2.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView2_CellBeginEdit);
            dataGridView2.CellEndEdit += dataGridView2_CellEndEdit;
            if(label!=1)//出库
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow;//设置奇数行颜色，从0开始，即：设置小包行的背景颜色为黄色
            dataGridView3.ReadOnly = true;
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件：TextChanged&Leave
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
        //根据主键显示
        public void IDShow(Int32 ID)
        {
            //******************************外表******************************//  
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new SqlDataAdapter("select * from " + table + " where ID = " + ID, mySystem.Parameter.conn);
            cb记录 = new SqlCommandBuilder(da记录);
            da记录.Fill(dt记录);

            outerBind();

            //*******************************表格内部******************************// 
            if (dt记录.Rows.Count <= 0)
            {
                MessageBox.Show("该ID下没有对应的记录");
                return;
            }
            //二维码表绑定
            dataGridView2.Columns.Clear();
            readInnerData_二维码(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind_二维码();

            //内表绑定
            dataGridView1.Columns.Clear();
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();

            fill_datagridview3();
            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  

            if (_userState == mySystem.Parameter.UserState.操作员 && (_formState == Parameter.FormState.未保存 || _formState == Parameter.FormState.审核未通过))
            {
                //填写内表的班次信息
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if(dataGridView1.Rows[i].Cells["班次"].Value.ToString()=="")
                        dt记录详情.Rows[i]["班次"] = mySystem.Parameter.userflight;
                    if (dataGridView1.Rows[i].Cells["操作员"].Value.ToString() == "")
                        dt记录详情.Rows[i]["操作员"] = mySystem.Parameter.userName;
                }
                    
            }
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //解绑->绑定
            tb产品代码.DataBindings.Clear();
            tb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");

            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }
        
        //*************************************************出库/退库详细信息***************************************************
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            if(1==label)//材料退库单
                da记录详情 = new SqlDataAdapter("select * from " + tableInfo + " where T材料退库单ID = " + ID, mySystem.Parameter.conn);
            else//材料出库单
                da记录详情 = new SqlDataAdapter("select * from " + tableInfo + " where T材料出库单ID = " + ID, mySystem.Parameter.conn);

            cb记录详情 = new SqlCommandBuilder(da记录详情);
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

        //*************************************************二维码详细信息***************************************************
        private void readInnerData_二维码(Int32 ID)
        {
            bs二维码信息= new BindingSource();
            dt二维码信息 = new DataTable(tableInfo_二维码);
            if (1 == label)//材料退库单
                da二维码信息 = new SqlDataAdapter("select * from " + tableInfo_二维码 + " where T材料退库单ID = " + ID, mySystem.Parameter.conn);
            else//材料出库单
                da二维码信息 = new SqlDataAdapter("select * from " + tableInfo_二维码 + " where T材料出库单ID = " + ID, mySystem.Parameter.conn);

            cb二维码信息 = new SqlCommandBuilder(da二维码信息);
            da二维码信息.Fill(dt二维码信息);
        }
        //控件绑定
        private void innerBind_二维码()
        {
            bs二维码信息.DataSource = dt二维码信息;
            //dataGridView1.DataBindings.Clear();
            dataGridView2.DataSource = bs二维码信息.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);

            if (isFirstBind2)
            {
                readDGVWidthFromSettingAndSet(dataGridView2);
                isFirstBind2 = false;
            }

        }
        private void fill_datagridview3()
        {
            while (dataGridView3.Rows.Count > 0)
                dataGridView3.Rows.RemoveAt(dataGridView3.Rows.Count - 1);
            hs_二维码.Clear();
            dic_材料代码.Clear();
            for (int i = 0; i < dataGridView2.Rows.Count; i++) 
            {
                string code,code_外;
                string num = "";
                double num_int;//数量
                if (1 == label)//退库
                {
                    code = dataGridView2.Rows[i].Cells["二维码"].Value.ToString();
                    num = dataGridView2.Rows[i].Cells["数量"].Value.ToString();
                    if (num == "")
                        num_int = 0;
                    else
                        num_int = double.Parse(num);
                    addrow(code,num_int);        
                }
                    
                else//出库
                {
                    //code = dataGridView2.Rows[i].Cells["二维码内"].Value.ToString();
                    //code_外=dataGridView2.Rows[i].Cells["二维码外"].Value.ToString();

                    if (i == dataGridView2.Rows.Count - 1 && 0==i%2)//二维码外是最后一行
                        break;
                    code_外 = dataGridView2.Rows[i].Cells["二维码"].Value.ToString();//二维码外
                    code = dataGridView2.Rows[i + 1].Cells["二维码"].Value.ToString();//二维码内
                       
                    num = dataGridView2.Rows[i+1].Cells["数量"].Value.ToString();
                    if (num == "")
                        num_int = 0;
                    else
                        num_int = double.Parse(num);

                    if (code_外 == "")
                    {
                        i++;
                        continue;
                    }
                        
                    if(code!="")//判断内外二维码对应的产品代码和批号是否相同
                    {
                        List<string> li_外 = parse_二维码(code_外);
                        List<string> li_内 = parse_二维码(code);
                        //if (dt记录.Rows[0]["属于工序"].ToString() != "吹膜")
                        //{
                        if (li_内[0] != li_外[0] || li_内[1] != li_外[1])
                        {
                            MessageBox.Show("第 " + (i + 1).ToString() + " 行二维码有误");
                            continue;
                        }
                        //}
                        //else//吹膜的情况下只比较代码
                        //{
                        //    if (li_内[0] != li_外[0])
                        //    {
                        //        MessageBox.Show("第 " + (i + 1).ToString() + " 行二维码有误");
                        //        continue;
                        //    }
                        //}
                    }
                    //以二维码内为准
                    addrow(code_外,num_int,code);
                    i++;
                    
                }                   
            }
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
                    case "是否合格":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;

                        cbc.Items.Add("合格");
                        cbc.Items.Add("不合格");

                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
                        //tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = 80;
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
            //隐藏
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["班次"].Visible = false;
            dataGridView2.Columns["ID"].Visible = false;
            if (1 == label)//材料退库单
            {
                dataGridView1.Columns["T材料退库单ID"].Visible = false;
                dataGridView1.Columns["退库日期时间"].ReadOnly = true;
                dataGridView1.Columns["退库数量"].ReadOnly = true;

                dataGridView2.Columns["T材料退库单ID"].Visible = false;
            }

            else//材料出库单
            {
                dataGridView1.Columns["T材料出库单ID"].Visible = false;
                dataGridView1.Columns["出库日期时间"].ReadOnly = true;
                dataGridView1.Columns["发料数量"].ReadOnly = false;

                dataGridView2.Columns["T材料出库单ID"].Visible = false;
                //dataGridView2.Columns["二维码外"].HeaderText = "二维码";
                //dataGridView2.Columns["二维码内"].HeaderText = "二维码(内)";
            }
                
            //不可用
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["物料代码"].ReadOnly = true;
            dataGridView1.Columns["物料批号"].ReadOnly = true;
            dataGridView1.Columns["班次"].ReadOnly = true;
            dataGridView1.Columns["审核员"].ReadOnly = true;
            //HeaderText
            dataGridView1.Columns["审核员"].HeaderText = "复核人";

            //if (dt记录.Rows[0]["属于工序"].ToString() == "吹膜")
            //{
            //    dataGridView1.Columns["物料批号"].Visible = false;
            //    dataGridView3.Columns[2].Visible = false;

            //}

        }

        //******************************按钮功能******************************//

        //TODO:添加按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            //DataRow dr = dt记录详情.NewRow();
            //dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            //dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            //setDataGridViewRowNums();
            //setEnableReadOnly();
            //if (dataGridView1.Rows.Count > 0)
            //    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;

            DataRow dr = dt二维码信息.NewRow();
            if (1 == label)//退库
            {
                dr["T材料退库单ID"] = int.Parse(dt记录.Rows[0]["ID"].ToString());
            }

            else
            {
                dr["T材料出库单ID"] = int.Parse(dt记录.Rows[0]["ID"].ToString());
            }
            dt二维码信息.Rows.InsertAt(dr, dt二维码信息.Rows.Count);

            //if (queue.Count > 0)
            //{
            //    qs = queue.Dequeue().ToString();
            //    if (1 == label)//退库
            //    {
            //        dr["T材料退库单ID"] = int.Parse(dt记录.Rows[0]["ID"].ToString());
            //        dr["二维码"] = qs;
            //    }

            //    else
            //    {
            //        dr["T材料出库单ID"] = int.Parse(dt记录.Rows[0]["ID"].ToString());
            //        dr["二维码外"] = qs;
            //        dr["二维码内"] = qs;
            //    }

            //    dr["数量"] = 50;
            //    if(!dic_二维码.ContainsKey(qs))
            //        dt二维码信息.Rows.InsertAt(dr, dt二维码信息.Rows.Count);
            //}


            //addrow("12312321", 50);
        }

        //删除按钮
        private void btn删除记录_Click(object sender, EventArgs e)
        {
            //if (dt记录详情.Rows.Count >= 2)
            //{
            //    int deletenum = dataGridView1.CurrentRow.Index;
            //    //仅当审核人为空时，可删除
            //    if (dataGridView1.Rows[deletenum].Cells["审核员"].Value.ToString() == "")
            //    {
            //        //dt记录详情.Rows.RemoveAt(deletenum);
            //        dt记录详情.Rows[deletenum].Delete();

            //        // 保存
            //        da记录详情.Update((DataTable)bs记录详情.DataSource);
            //        readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            //        innerBind();

            //        setDataGridViewRowNums();
            //        setEnableReadOnly();
            //    }
            //}
            if (dataGridView2.Rows.Count > 0)
            {
                int delnum = dataGridView2.CurrentCell.RowIndex;
                if (delnum < 0)
                    return;
                if (1 != label)//退库
                {
                    if (delnum % 2 == 0)//删的是白色行，大包
                    {
                        if (delnum != dataGridView2.Rows.Count - 1)//不是最后一行
                        {
                            dataGridView2.Rows.RemoveAt(delnum);
                            dataGridView2.Rows.RemoveAt(delnum);
                        }
                        else//删的是最后一行白色
                            dataGridView2.Rows.RemoveAt(delnum);
                    }
                    else //删的是黄色行，小包
                    {
                        dataGridView2.Rows.RemoveAt(delnum - 1);//删掉上一行
                        dataGridView2.Rows.RemoveAt(delnum - 1);
                    }
                }
                else
                {
                    dataGridView2.Rows.RemoveAt(delnum);
                }
              
                fill_datagridview3();                      
       
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
        }

        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
                btn提交审核.Enabled = true;
        }

        //检查表三和表一是否相同
        private bool isOk_Table()
        {
            //检查表二
            if(label!=1)//出库
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells["二维码"].Value.ToString() == "")
                    {
                        MessageBox.Show("二维码表第 " + (i + 1).ToString() + " 行有未扫描的二维码");
                        return false;
                    }

                }
                if (0 != dataGridView2.Rows.Count % 2)
                {
                    MessageBox.Show("需再添加一行进行扫描！");
                    return false;
                }
            }

            //检查表三
            if (dataGridView1.Rows.Count != dataGridView3.Rows.Count)
            {
                MessageBox.Show("物料代码 种类个数 不匹配");
                return false;
            }
            //if (dt记录.Rows[0]["属于工序"].ToString() != "吹膜")
            //{
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                DataRow[] dr = dt记录详情.Select(string.Format("物料代码='{0}' and 物料批号='{1}'", dataGridView3.Rows[i].Cells["物料代码"].Value.ToString(), dataGridView3.Rows[i].Cells["物料批号"].Value.ToString()));
                if (dr.Length == 0)
                {
                    MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "不匹配");
                    return false;
                }
                if (label != 1)//出库
                {
                    if (double.Parse(dr[0][7].ToString()) > double.Parse(dataGridView3.Rows[i].Cells["数量"].Value.ToString()))//应该出库量>实际出库量
                    {
                        MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "出库量应该大于" + dr[0][7].ToString());
                        return false;
                    }
                }
                else//退库
                {
                    if (dr[0][7].ToString() != dataGridView3.Rows[i].Cells["数量"].Value.ToString())//退库数量不等
                    {
                        MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "对应数量不匹配");
                        return false;
                    }
                }

            }
            //}
            //else
            //{
            //    for (int i = 0; i < dataGridView3.Rows.Count; i++)
            //    {
            //        DataRow[] dr = dt记录详情.Select(string.Format("物料代码='{0}'", dataGridView3.Rows[i].Cells["物料代码"].Value.ToString()));
            //        if (dr.Length == 0)
            //        {
            //            MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "不匹配");
            //            return false;
            //        }
            //        if (label != 1)//出库
            //        {
            //            if (int.Parse(dr[0][7].ToString()) > int.Parse(dataGridView3.Rows[i].Cells["数量"].Value.ToString()))//应该出库量>实际出库量
            //            {
            //                MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "出库量应该大于" + dr[0][7].ToString());
            //                return false;
            //            }
            //        }
            //        else//退库
            //        {
            //            if (dr[0][7].ToString() != dataGridView3.Rows[i].Cells["数量"].Value.ToString())//退库数量不等
            //            {
            //                MessageBox.Show("物料代码" + dataGridView3.Rows[i].Cells["物料代码"].Value.ToString() + "对应数量不匹配");
            //                return false;
            //            }
            //        }

            //    }
            //}

            return true;
        }
        //保存功能
        private bool Save()
        {
            if (Name_check() == false)
            {
                /*操作员不合格*/
                return false;
            }
            //else if (mySystem.Parameter.NametoID(dt记录.Rows[0]["操作员"].ToString()) == 0)
            //{
            //    /*操作员不合格*/
            //    MessageBox.Show("操作员不存在，请重新输入！");
            //    return false;
            //}
            ////else if (TextBox_check() == false)
            ////{
            ////    /*数字框不合格*/
            ////    return false;
            ////}
            //else if (Batch_check() == false)
            //{
            //    /*批号不合格*/
            //    return false;
            //}
            else
            {
                //外表保存
                da记录.Update(dt记录);

                // 内表保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                //二维码表保存
                bs二维码信息.EndEdit();
                da二维码信息.Update((DataTable)bs二维码信息.DataSource);
                readInnerData_二维码(int.Parse(dt记录.Rows[0]["ID"].ToString()));
                innerBind_二维码();

                setEnableReadOnly();

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
            if (!checkInnerData(dataGridView2))
            {
                MessageBox.Show("请填写完整的表单信息", "提示");
                return;
            }
            //if (!checkInnerData(dataGridView3))
            //{
            //    MessageBox.Show("请填写完整的表单信息", "提示");
            //    return;
            //}
            
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;
                          
            //检查表3和表1是否相同
            if (!isOk_Table())
                return;

            //写待审核表
            //填写表格1内的审核员
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    dataGridView1.Rows[i].Cells["审核员"].Value = "__待审核";
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
                dt记录详情 .Rows[i]["审核员"] = "__待审核";

            DataTable dt_temp = new DataTable("待审核");
            SqlDataAdapter da_temp=new SqlDataAdapter("select * from 待审核 where 表名='"+ table +"' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
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
            try
            {
                (new mySystem.Other.LogForm()).setLog(dt记录.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
            }
        }

        //审核功能
        private void btn审核_Click(object sender, EventArgs e)
        {
            //if (mySystem.Parameter.userName == dt记录.Rows[0]["操作员"].ToString())
            //{
            //    MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
            //    return;
            //}
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

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            dt记录.Rows[0]["审核日期"] = DateTime.Now;

            //填写表格1内的审核员
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
                dt记录详情.Rows[i]["审核员"] = mySystem.Parameter.userName;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='"+ table +"' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.conn);
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
            if (checkform.ischeckOk)//审核通过
            { 
                _formState = Parameter.FormState.审核通过; 

                //更新二维码记录，二维码信息，库存台账
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = mySystem.Parameter.conn;
                string strcmd;

                if (1 == label)//退库
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        //更新二维码信息表，update操作
                        string strname = dataGridView2.Rows[i].Cells["二维码"].Value.ToString();//二维码
                        int num = int.Parse(dataGridView2.Rows[i].Cells["数量"].Value.ToString());//数量

                        strcmd = string.Format("UPDATE {0} SET {1}={2}+{3} WHERE 二维码='{4}'", "二维码信息", "数量", "数量", num, strname);
                        cmd.CommandText = strcmd;
                        int n = cmd.ExecuteNonQuery();
                        if (n <= 0)
                        {
                            MessageBox.Show(string.Format("表格第 {0} 行更新 二维码信息表 有误", i + 1));
                            return;
                        }

                        //更新库存台账，update操作
                        strcmd = string.Format("SELECT * from 二维码信息 where 二维码='{0}'", strname);
                        cmd.CommandText = strcmd;
                        List<List<Object>> ret = new List<List<Object>>();
                        SqlDataReader reader = null;
                        reader = cmd.ExecuteReader();
                        int id_库存 = 0;
                        while (reader.Read())//只有一行
                        {
                            id_库存 = int.Parse(reader[2].ToString());
                        }

                        strcmd = string.Format("UPDATE {0} SET {1}={2}+{3} WHERE ID={4}", "库存台账", "现存数量", "现存数量", num, id_库存);
                        cmd.CommandText = strcmd;
                        n = cmd.ExecuteNonQuery();
                        if (n <= 0)
                        {
                            MessageBox.Show(string.Format("表格第 {0} 行更新 库存台账表 有误", i + 1));
                            return;
                        }

                        //更新二维码历史记录,insert操作
                        List<string> name = new List<string>();
                        List<object> value = new List<object>();
                        name.Add("时间");
                        name.Add("二维码");
                        name.Add("操作");
                        name.Add("备注");

                        value.Add(DateTime.Now.ToString());
                        value.Add(strname);
                        value.Add("入库");
                        value.Add("");

                        if (!Utility.insertAccess(mySystem.Parameter.conn, "二维码历史记录", name, value))
                        {
                            MessageBox.Show(string.Format("表格第 {0} 行更新 二维码历史记录表 有误", i + 1));
                            return;
                        }
                    }

                }
                else//出库
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i=i+2)
                    {
                        //更新二维码信息表
                        string strname = dataGridView2.Rows[i+1].Cells["二维码"].Value.ToString();//二维码内
                        int num = int.Parse(dataGridView2.Rows[i+1].Cells["数量"].Value.ToString());//数量
                        int id_库存 = 0;

                        //先获得库存id
                        strcmd = string.Format("SELECT * from 二维码信息 where 二维码='{0}'", dataGridView2.Rows[i].Cells["二维码"].Value.ToString());
                        cmd.CommandText = strcmd;
                        List<List<Object>> ret = new List<List<Object>>();
                        SqlDataReader reader = null;
                        reader = cmd.ExecuteReader();

                        while (reader.Read())//只有一行
                        {
                            id_库存 = int.Parse(reader[2].ToString());
                        }
                        reader.Close();

                        if (strname == dataGridView2.Rows[i].Cells["二维码"].Value.ToString())//update操作
                        {
                            //strcmd = string.Format("UPDATE {0} SET {1}={2}-{3} WHERE 二维码='{4}'", "二维码信息", "数量", "数量", num, strname);
                            //cmd.CommandText = strcmd;
                            //int n = cmd.ExecuteNonQuery();
                            //if (n <= 0)
                            //{
                            //    MessageBox.Show(string.Format("表格第 {0} 行更新 二维码信息表 有误", i + 1));
                            //    return;
                            //}
                        }
                        else//
                        {
                            ////插入小包二维码
                            //List<string> name = new List<string>();
                            //List<object> value = new List<object>();
                            //name.Add("二维码");
                            //name.Add("库存ID");
                            //name.Add("数量");

                            //value.Add(strname);
                            //value.Add(id_库存);
                            //value.Add(0);

                            //if (!Utility.insertAccess(mySystem.Parameter.conn, "二维码信息", name, value))
                            //{
                            //    MessageBox.Show(string.Format("表格第 {0} 行更新 二维码信息 有误", i + 1));
                            //    return;
                            //}

                            //更新小包二维码对应数量
                            strcmd = string.Format("UPDATE {0} SET {1}={2}+{3} WHERE 二维码='{4}'", "二维码信息", "数量", "数量", num, strname);
                            cmd.CommandText = strcmd;
                            int n = cmd.ExecuteNonQuery();
                            if (n <= 0)
                            {
                                MessageBox.Show(string.Format("表格第 {0} 行更新 二维码信息表 有误", i + 2));
                                return;
                            }

                            //更新大包二维码对应数量
                            strcmd = string.Format("UPDATE {0} SET {1}={2}-{3} WHERE 二维码='{4}'", "二维码信息", "数量", "数量", num, dataGridView2.Rows[i].Cells["二维码"].Value.ToString());
                            cmd.CommandText = strcmd;
                            n = cmd.ExecuteNonQuery();
                            if (n <= 0)
                            {
                                MessageBox.Show(string.Format("表格第 {0} 行更新 二维码信息表 有误", i + 1));
                                return;
                            }
                        }

                        //更新库存台账，update操作
                        strcmd = string.Format("UPDATE {0} SET {1}={2}-{3} WHERE ID={4}", "库存台帐", "现存数量", "现存数量", num, id_库存);
                        cmd.CommandText = strcmd;
                        int m = cmd.ExecuteNonQuery();
                        if (m <= 0)
                        {
                            MessageBox.Show(string.Format("表格第 {0} 行更新 库存台账表 有误", i + 1));
                            return;
                        }

                        //更新二维码历史记录,insert操作
                        List<string> name1 = new List<string>();
                        List<object> value1 = new List<object>();
                        name1.Add("时间");
                        name1.Add("二维码");
                        name1.Add("操作");
                        name1.Add("备注");

                        //TODO：时间插入datatime.now有问题
                        value1.Add(DateTime.Now.ToString());
                        value1.Add(strname);
                        value1.Add("出库");
                        value1.Add(id_库存.ToString());

                        if (!Utility.insertAccess(mySystem.Parameter.conn, "二维码历史记录", name1, value1))
                        {
                            MessageBox.Show(string.Format("表格第 {0} 行更新 二维码历史记录表 有误", i + 1));
                            return;
                        }
                    }
                }

            }
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
            Microsoft.Office.Interop.Excel._Workbook wb;
            if(1==label)//退库
                wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\库存\SOP-WH-003-R04A 材料退库单.xlsx");
            else
                wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\库存\SOP-WH-003-R03A 材料出库单.xlsx");

            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
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
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒 打印文档\n");
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

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[3, 1].Value = "产品代码：" + dt记录.Rows[0]["产品代码"].ToString();
            mysheet.Cells[3, 6].Value = "产品批号：" + dt记录.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 9].Value = "生产指令编号：" +tb生产指令编号.Text;

            //内表信息
            int rownum = dt记录详情.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 9 ? 9 : rownum); i++)
            {
                mysheet.Cells[5 + i, 1].Value = dt记录详情.Rows[i]["序号"].ToString();
                if (1 == label)//退库
                {
                    mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["退库日期时间"].ToString()).ToString("yyyy/MM/dd");
                    mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["退库日期时间"].ToString()).ToShortTimeString();
                    mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["退库数量"].ToString();
                }
                else
                {
                    mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["出库日期时间"].ToString()).ToString("yyyy/MM/dd");
                    mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["出库日期时间"].ToString()).ToShortTimeString();
                    mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["发料数量"].ToString();
 
                }
                mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["班次"].ToString();
                mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["物料批号"].ToString();

                mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["是否合格"].ToString();
                mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["备注"].ToString();
                mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["操作员"].ToString();
                mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["审核员"].ToString();

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
                    if (1 == label)//退库
                    {
                        mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["退库日期时间"].ToString()).ToString("yyyy/MM/dd");
                        mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["退库日期时间"].ToString()).ToShortTimeString();
                        mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["退库数量"].ToString();
                    }
                    else
                    {
                        mysheet.Cells[5 + i, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["出库日期时间"].ToString()).ToString("yyyy/MM/dd");
                        mysheet.Cells[5 + i, 3].Value = Convert.ToDateTime(dt记录详情.Rows[i]["出库日期时间"].ToString()).ToShortTimeString();
                        mysheet.Cells[5 + i, 7].Value = dt记录详情.Rows[i]["发料数量"].ToString();

                    }
                    mysheet.Cells[5 + i, 4].Value = dt记录详情.Rows[i]["班次"].ToString();
                    mysheet.Cells[5 + i, 5].Value = dt记录详情.Rows[i]["物料代码"].ToString();
                    mysheet.Cells[5 + i, 6].Value = dt记录详情.Rows[i]["物料批号"].ToString();

                    mysheet.Cells[5 + i, 8].Value = dt记录详情.Rows[i]["是否合格"].ToString();
                    mysheet.Cells[5 + i, 9].Value = dt记录详情.Rows[i]["备注"].ToString();
                    mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["操作员"].ToString();
                    mysheet.Cells[5 + i, 10].Value = dt记录详情.Rows[i]["审核员"].ToString();
                }
            }
            //加页脚
            //int sheetnum;
            //SqlDataAdapter da = new SqlDataAdapter("select ID from " + table + " where 生产指令ID=" + InstruID.ToString(), mySystem.Parameter.conn);
            //DataTable dt = new DataTable("temp");
            //da.Fill(dt);
            //List<Int32> sheetList = new List<Int32>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            //sheetnum = sheetList.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
            //mysheet.PageSetup.RightFooter = Instruction + "-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            mysheet.PageSetup.RightFooter = tb生产指令编号.Text + "-" + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码

            return mysheet;
        }
               
        //******************************小功能******************************//  
        //TODO：解析二维码
        List<string> parse_二维码(string code)
        {
            List<string> ret = new List<string>();
            string[] s = Regex.Split(code, "@");
            for (int i = 0; i < s.Length; i++)
            {
                 ret.Add(s[i]);
            }
            return ret;
        }

        //讲根据二维码添加行
        private void addrow(string code,double num)
        {
            if (code != "" && !hs_二维码.Contains(code))
            {
                //添加并解析
                hs_二维码.Add(code);
                List<string> li = parse_二维码(code);
                if (!dic_材料代码.ContainsKey(li[0]+"@"+li[1]))//不包括该物料代码
                {
                    int rownum = dataGridView3.Rows.Add();
                    dataGridView3.Rows[rownum].Cells["序号"].Value = rownum + 1;//序号
                    dataGridView3.Rows[rownum].Cells["物料代码"].Value = li[0];//物料代码
                    dataGridView3.Rows[rownum].Cells["物料批号"].Value = li[1];//物料批号
                    dataGridView3.Rows[rownum].Cells["数量"].Value = num;//出库/退库数量

                    dic_材料代码.Add(li[0] + "@" + li[1], rownum);
                }
                else//物料代码已经存在，只需要更新数量
                {
                    dataGridView3.Rows[dic_材料代码[li[0] + "@" + li[1]]].Cells["数量"].Value = double.Parse(dataGridView3.Rows[dic_材料代码[li[0] + "@" + li[1]]].Cells["数量"].Value.ToString()) + num;//出库/退库数量
                }

            }
        }
        private void addrow(string code, double num,string code2)//code是二维码外，code2是二维码内
        {
            if (code != "" && !hs_二维码.Contains(code2))
            {
                //添加并解析
                hs_二维码.Add(code2);
                List<string> li = parse_二维码(code);
                if (!dic_材料代码.ContainsKey(li[0]+"@"+li[1]))//不包括该物料代码
                {
                    int rownum = dataGridView3.Rows.Add();
                    dataGridView3.Rows[rownum].Cells["序号"].Value = rownum + 1;//序号
                    dataGridView3.Rows[rownum].Cells["物料代码"].Value = li[0];//物料代码
                    dataGridView3.Rows[rownum].Cells["物料批号"].Value = li[1];//物料批号
                    dataGridView3.Rows[rownum].Cells["数量"].Value = num;//出库/退库数量

                    dic_材料代码.Add(li[0] + "@" + li[1], rownum);
                }
                else//物料代码已经存在，只需要更新数量
                {
                    dataGridView3.Rows[dic_材料代码[li[0] + "@" + li[1]]].Cells["数量"].Value = double.Parse(dataGridView3.Rows[dic_材料代码[li[0] + "@" + li[1]]].Cells["数量"].Value.ToString()) + num;//出库/退库数量
                }

            }
        }
        // 检查操作员的姓名（内表）
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
        
        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            //bool TypeCheck = true;
            //List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb废品重量, tb成品率 });
            //List<String> StringList = new List<String>(new String[] { "废品重量", "成品率" });
            //int numtemp = 0;
            //for (int i = 0; i < TextBoxList.Count; i++)
            //{
            //    if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
            //    {
            //        MessageBox.Show("『" + StringList[i] + "』框内应填数字，请重新填入！");
            //        TypeCheck = false;
            //        break;
            //    }
            //}
            //return TypeCheck;

            return true;
        }

        //检查批号是否合格
        private bool Batch_check()
        {
            bool TypeCheck = true;
            //for (int i = 0; i < dt记录详情.Rows.Count; i++)
            //{
            //    List<String> ls批号原始 = new List<string>();
            //    List<String> ls批号 = new List<string>();
            //    //从数据库获取信息->ls批号原始
            //    DataRow[] dr物料 = dt物料.Select("物料简称 = '" + dt记录详情.Rows[i]["物料简称"].ToString() + "'");
            //    string[] s = Regex.Split(dr物料[0]["物料批号"].ToString(), ",|，");
            //    for (int j = 0; j < s.Length; j++)
            //    { if (s[j] != "")  { ls批号原始.Add(s[j]); } }
            //    //获取DataGridView->ls批号
            //    s = Regex.Split(dt记录详情.Rows[i]["物料批号"].ToString(), ",|，");
            //    for (int j = 0; j < s.Length; j++)
            //    { if (s[j] != "") { ls批号.Add(s[j]); } }
            //    //检查是否都是存在
            //    for (int j = 0; j < ls批号.Count; j++)
            //    {
            //        if (ls批号原始.IndexOf(ls批号[j]) == -1)
            //        {
            //            MessageBox.Show("第" + i.ToString() + "行『物料批号』填写不符合要求！");
            //            TypeCheck = false;
            //            return TypeCheck;
            //        }
            //    }
            //}
            return TypeCheck;
        }

        //实时求收率
        private void getNum(Int32 Rownum)
        {
            //int numA, numB, numC, numD;
            //// 膜卷长度求和
            //if ((Int32.TryParse(dt记录详情.Rows[Rownum]["接上班数量A"].ToString(), out numA) == true) && (Int32.TryParse(dt记录详情.Rows[Rownum]["领取数量B"].ToString(), out numB) == true) && (Int32.TryParse(dt记录详情.Rows[Rownum]["使用数量C"].ToString(), out numC) == true) && (Int32.TryParse(dt记录详情.Rows[Rownum]["退库数量D"].ToString(), out numD) == true))
            //{
            //    //均为数值类型
            //    dt记录详情.Rows[Rownum]["物料平衡"] = (Int32)(numA + numB - numC - numD);
            //}
            //else
            //{ dt记录详情.Rows[Rownum]["物料平衡"] = -1; }
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
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
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

            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex >= 0)
            //{
            //    if (dataGridView1.Columns[e.ColumnIndex].Name == "接上班数量A")
            //    { getNum(e.RowIndex); }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "领取数量B")
            //    { getNum(e.RowIndex); }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "使用数量C")
            //    { getNum(e.RowIndex); }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "退库数量D")
            //    { getNum(e.RowIndex); }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "操作员")
            //    {
            //        if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
            //        {
            //            dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
            //            MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
            //        }
            //    }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "物料简称")
            //    {
            //        DataRow[] drs = dt物料.Select("物料简称='" + dataGridView1["物料简称", e.RowIndex].Value.ToString() + "'");
            //        dataGridView1["物料代码", e.RowIndex].Value = drs[0]["物料代码"].ToString();
            //        dataGridView1["物料批号", e.RowIndex].Value = drs[0]["物料批号"].ToString();
            //    }
            //    else
            //    { }
            //}
        }
        
        //改变单元格之前
        void dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           
        }
        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "二维码")
                {
                    List<String> l = parse_二维码(dataGridView2.Rows[e.RowIndex].Cells["二维码"].Value.ToString());
                    if (l.Count != 3)
                    {
                        MessageBox.Show("二维码格式有误");
                        dataGridView2.Rows[e.RowIndex].Cells["二维码"].Value = "";
                        return;
                    }
                }
            }
            if (1 != label)//出库
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    

                    if (e.RowIndex % 2 == 0 && dataGridView2.Columns[e.ColumnIndex].Name == "二维码")
                    {
                        
                        //杜数据库填数量
                        //先获得库存id
                        if (dataGridView2.Rows[e.RowIndex].Cells["二维码"].Value.ToString() != "")
                        {
                            
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = mySystem.Parameter.conn;
                            string strcmd = string.Format("SELECT * from 二维码信息 where 二维码='{0}'", dataGridView2.Rows[e.RowIndex].Cells["二维码"].Value.ToString());
                            cmd.CommandText = strcmd;
                            List<List<Object>> ret = new List<List<Object>>();
                            SqlDataReader reader = null;
                            reader = cmd.ExecuteReader();

                            while (reader.Read())//只有一行
                            {
                                dataGridView2.Rows[e.RowIndex].Cells["数量"].Value = int.Parse(reader[3].ToString());
                            }
                            reader.Close();
                        }

                    }
                    
                     if (e.RowIndex % 2 == 1 && dataGridView2.Columns[e.ColumnIndex].Name == "数量"){
                         if (dataGridView2.Rows[e.RowIndex].Cells["二维码"].Value.ToString() ==
                            dataGridView2.Rows[e.RowIndex - 1].Cells["二维码"].Value.ToString())
                         {
                             dataGridView2.Rows[e.RowIndex].Cells["数量"].Value =
                                 dataGridView2.Rows[e.RowIndex - 1].Cells["数量"].Value;
                         }
                         else
                         {
                             double u;
                             double d;
                             try
                             {
                                 u = Convert.ToDouble(dataGridView2.Rows[e.RowIndex - 1].Cells["数量"].Value);
                                 d = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells["数量"].Value);
                             }
                             catch (Exception eee)
                             {
                                 u = 0;
                                 d = 0;
                             }
                             if (d >= u)
                             {
                                 dataGridView2.Rows[e.RowIndex].Cells["数量"].Value =
                                     dataGridView2.Rows[e.RowIndex - 1].Cells["数量"].Value;
                             }
                         }
                     }
                }
            }

            fill_datagridview3();

        }

        private void btn打印二维码_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) return;
            string daima = dataGridView1["物料代码", dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            string pihao = dataGridView1["物料批号", dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            mySystem.Other.二维码打印 form = mySystem.Other.二维码打印.create(daima, pihao);
            form.Show();
        }

        private void 材料退库出库单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView1);
            if (dataGridView2.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView2);
            if (dataGridView3.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView3);
        }

        void check库存()
        {
            if (label == 2)
            {
                SqlCommand comm;
                String sql = "select sum(现存数量) from 库存台帐 where 产品代码='{0}'";
                foreach (DataRow dr in dt记录详情.Rows)
                {
                    String daima = dr["物料代码"].ToString();
                    double shuliang = Convert.ToDouble(dr["发料数量"]);
                    comm = new SqlCommand(String.Format(sql, daima), conn);
                    Object res = comm.ExecuteScalar();
                    double a = 0;
                    if (res != DBNull.Value)
                    {
                        a = Convert.ToDouble(comm.ExecuteScalar());
                    }

                    if (a <= shuliang)
                    {
                        MessageBox.Show("产品: " + daima + " 的库存数量不足以发货", "警告");
                        return;
                    }

                }
            }
        }
        
    }
}
