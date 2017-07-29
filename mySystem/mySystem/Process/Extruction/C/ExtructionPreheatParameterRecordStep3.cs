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
using System.Text.RegularExpressions;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : BaseForm
    {
        private String table = "吹膜机组预热参数记录表";
        private String tableSet = "设置吹膜机组预热参数记录表";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;

        private CheckForm checkform = null;

        //List<Control> SettingConsList = new List<Control> { };//各种温度的控件
        //List<String> SettingValdata = new List<String> { };//设置界面内存储的各种温度

        private DataTable dt设置, dt记录;
        private OleDbDataAdapter da记录;
        private BindingSource bs记录;
        private OleDbCommandBuilder cb记录;

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

        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();          
            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，几个时间的格式
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            DataShow(mySystem.Parameter.proInstruID);
        }
        
        public ExtructionPreheatParameterRecordStep3(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，几个时间的格式
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
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜预热参数记录表'", connOle);
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

        //读取设置内容  //GetSettingInfo()
        private void getOtherData()
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();
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
            //setDataGridViewFormat();
        }

        /// <summary>
        /// 设置所有控件可用；
        /// btn审核、btn提交审核两个按钮一直是false；
        /// 部分控件防作弊，不可改；
        /// 查询条件始终不可编辑
        /// </summary>
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
            tb审核人.Enabled = false;
            TextboxReadOnly();
        }

        /// <summary>
        /// 设置所有控件不可用；
        /// 查看日志、打印始终可用
        /// </summary>
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

        //textbox不可用，仅可读取
        private void TextboxReadOnly()
        {
            tb换网预热参数设定1.ReadOnly = true;
            tb流道预热参数设定1.ReadOnly = true;
            tb模颈预热参数设定1.ReadOnly = true;
            tb机头1预热参数设定1.ReadOnly = true;
            tb机头2预热参数设定1.ReadOnly = true;
            tb口模预热参数设定1.ReadOnly = true;
            tb一区预热参数设定1.ReadOnly = true;
            tb二区预热参数设定1.ReadOnly = true;
            tb三区预热参数设定1.ReadOnly = true;
            tb四区预热参数设定1.ReadOnly = true;
            tb换网预热参数设定2.ReadOnly = true;
            tb流道预热参数设定2.ReadOnly = true;
            tb模颈预热参数设定2.ReadOnly = true;
            tb机头1预热参数设定2.ReadOnly = true;
            tb机头2预热参数设定2.ReadOnly = true;
            tb口模预热参数设定2.ReadOnly = true;
            tb一区预热参数设定2.ReadOnly = true;
            tb二区预热参数设定2.ReadOnly = true;
            tb三区预热参数设定2.ReadOnly = true;
            tb四区预热参数设定2.ReadOnly = true;
            tb加热保温时间1.ReadOnly = true;
            tb加热保温时间2.ReadOnly = true;
            tb加热保温时间3.ReadOnly = true;
        }
        
        // 其他事件，几个时间的格式
        private void addOtherEvnetHandler()
        {
            //时间控件初始化
            this.dtp预热开始时间.ShowUpDown = true;
            this.dtp预热开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp预热开始时间.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间1.ShowUpDown = true;
            this.dtp保温结束时间1.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温开始时间.ShowUpDown = true;
            this.dtp保温开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp保温开始时间.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间2.ShowUpDown = true;
            this.dtp保温结束时间2.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间3.ShowUpDown = true;
            this.dtp保温结束时间3.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间3.CustomFormat = "yyyy/MM/dd HH:mm";
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件
        private void addComputerEventHandler() { }

        //******************************显示数据******************************//
        
        //显示根据信息查找
        private void DataShow(int InstruID) 
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
            }

            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  
        }

        //根据主键显示
        private void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();

            if (reader1.Read())
            { DataShow(Convert.ToInt32(reader1["生产指令ID"].ToString())); }            
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString(), connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }
        
        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            // 绑定（先解除绑定）
            dtp日期.DataBindings.Clear();
            dtp日期.DataBindings.Add("Text", bs记录.DataSource, "日期");
            tb模芯规格参数1.DataBindings.Clear();
            tb模芯规格参数1.DataBindings.Add("Text", bs记录.DataSource, "模芯规格参数1");
            tb模芯规格参数2.DataBindings.Clear();
            tb模芯规格参数2.DataBindings.Add("Text", bs记录.DataSource, "模芯规格参数2");
            tb记录人.DataBindings.Clear();
            tb记录人.DataBindings.Add("Text", bs记录.DataSource, "记录人");
            tb操作员备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Add("Text", bs记录.DataSource, "操作员备注");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp预热开始时间.DataBindings.Clear();
            dtp预热开始时间.DataBindings.Add("Text", bs记录.DataSource, "预热开始时间");
            dtp保温结束时间1.DataBindings.Clear();
            dtp保温结束时间1.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间1");
            dtp保温开始时间.DataBindings.Clear();
            dtp保温开始时间.DataBindings.Add("Text", bs记录.DataSource, "保温开始时间");
            dtp保温结束时间2.DataBindings.Clear();
            dtp保温结束时间2.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间2");
            dtp保温结束时间3.DataBindings.Clear();
            dtp保温结束时间3.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间3");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            //不可用 绑定
            tb换网预热参数设定1.DataBindings.Clear();
            tb换网预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "换网预热参数设定1");
            tb流道预热参数设定1.DataBindings.Clear();
            tb流道预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "流道预热参数设定1");
            tb模颈预热参数设定1.DataBindings.Clear();
            tb模颈预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "模颈预热参数设定1");
            tb机头1预热参数设定1.DataBindings.Clear();
            tb机头1预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "机头1预热参数设定1");
            tb机头2预热参数设定1.DataBindings.Clear();
            tb机头2预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "机头2预热参数设定1");
            tb口模预热参数设定1.DataBindings.Clear();
            tb口模预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "口模预热参数设定1");
            tb一区预热参数设定1.DataBindings.Clear();
            tb一区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "一区预热参数设定1");
            tb二区预热参数设定1.DataBindings.Clear();
            tb二区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "二区预热参数设定1");
            tb三区预热参数设定1.DataBindings.Clear();
            tb三区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "三区预热参数设定1");
            tb四区预热参数设定1.DataBindings.Clear();
            tb四区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "四区预热参数设定1");
            tb换网预热参数设定2.DataBindings.Clear();
            tb换网预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "换网预热参数设定2");
            tb流道预热参数设定2.DataBindings.Clear();
            tb流道预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "流道预热参数设定2");
            tb模颈预热参数设定2.DataBindings.Clear();
            tb模颈预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "模颈预热参数设定2");
            tb机头1预热参数设定2.DataBindings.Clear();
            tb机头1预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "机头1预热参数设定2");
            tb机头2预热参数设定2.DataBindings.Clear();
            tb机头2预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "机头2预热参数设定2");
            tb口模预热参数设定2.DataBindings.Clear();
            tb口模预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "口模预热参数设定2");
            tb一区预热参数设定2.DataBindings.Clear();
            tb一区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "一区预热参数设定2");
            tb二区预热参数设定2.DataBindings.Clear();
            tb二区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "二区预热参数设定2");
            tb三区预热参数设定2.DataBindings.Clear();
            tb三区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "三区预热参数设定2");
            tb四区预热参数设定2.DataBindings.Clear();
            tb四区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "四区预热参数设定2");
            tb加热保温时间1.DataBindings.Clear();
            tb加热保温时间1.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间1");
            tb加热保温时间2.DataBindings.Clear();
            tb加热保温时间2.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间2");
            tb加热保温时间3.DataBindings.Clear();
            tb加热保温时间3.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间3");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;
            dr["生产指令id"] = mySystem.Parameter.proInstruID;
            dr["日期"] = Convert.ToDateTime(dtp日期.Value.ToString("yyyy/MM/dd"));
            dr["记录人"] = mySystem.Parameter.userName;
            dr["审核人"] = "";
            dr["审核是否通过"] = false;

            //dr["模芯规格参数1"] = 0;
            //dr["模芯规格参数2"] = 0;

            dr["预热开始时间"] = Convert.ToDateTime(dtp预热开始时间.Value.ToString("yyyy/MM/dd HH:mm"));
            dr["保温结束时间1"] = Convert.ToDateTime(dtp保温结束时间1.Value.ToString("yyyy/MM/dd HH:mm"));
            dr["保温开始时间"] = Convert.ToDateTime(dtp保温开始时间.Value.ToString("yyyy/MM/dd HH:mm"));
            dr["保温结束时间2"] = Convert.ToDateTime(dtp保温结束时间2.Value.ToString("yyyy/MM/dd HH:mm"));
            dr["保温结束时间3"] = Convert.ToDateTime(dtp保温结束时间3.Value.ToString("yyyy/MM/dd HH:mm"));

            dr["换网预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["换网预热参数设定1"].ToString());
            dr["流道预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["流道预热参数设定1"].ToString());
            dr["模颈预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["模颈预热参数设定1"].ToString());
            dr["机头1预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["机头1预热参数设定1"].ToString());
            dr["机头2预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["机头2预热参数设定1"].ToString());
            dr["口模预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["口模预热参数设定1"].ToString());

            dr["一区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["一区预热参数设定1"].ToString());
            dr["二区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["二区预热参数设定1"].ToString());
            dr["三区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["三区预热参数设定1"].ToString());
            dr["四区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["四区预热参数设定1"].ToString());

            dr["换网预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["换网预热参数设定2"].ToString());
            dr["流道预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["流道预热参数设定2"].ToString());
            dr["模颈预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["模颈预热参数设定2"].ToString());
            dr["机头1预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["机头1预热参数设定2"].ToString());
            dr["机头2预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["机头2预热参数设定2"].ToString());
            dr["口模预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["口模预热参数设定2"].ToString());

            dr["一区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["一区预热参数设定2"].ToString());
            dr["二区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["二区预热参数设定2"].ToString());
            dr["三区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["三区预热参数设定2"].ToString());
            dr["四区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["四区预热参数设定2"].ToString());

            dr["加热保温时间1"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间1"].ToString());
            dr["加热保温时间2"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间2"].ToString());
            dr["加热保温时间3"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间3"].ToString());

            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + mySystem.Parameter.proInstruction + "\n";
            dr["日志"] = log;
            
            return dr;
        }

        //******************************按钮功能******************************//

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
            if (mySystem.Parameter.NametoID(tb记录人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『操作员』信息", "ERROR");
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*模芯规格填入的不是数字*/
                return false;
            }
            else
            {
                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(mySystem.Parameter.proInstruID);
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜预热参数记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "吹膜预热参数记录表";
                dr["对应ID"] = Convert.ToInt32(dt记录.Rows[0]["ID"]);
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

        //日志按钮
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            mySystem.Other.LogForm logform = new mySystem.Other.LogForm();
            logform.setLog(dt记录.Rows[0]["日志"].ToString()).Show();
        }
        
        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录.Rows[0]["确认人"].ToString())
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='吹膜预热参数记录表' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
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

        //打印按钮
        private void printBtn_Click(object sender, EventArgs e)
        {

        }

        //******************************小功能******************************//
        
        //检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb模芯规格参数1, tb模芯规格参数2 });
            List<String> StringList = new List<String>(new String[] { "模芯规格参数φ", "模芯规格参数Gap" });

            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show("『" + StringList[i] + "』应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }
                        
    }
}
