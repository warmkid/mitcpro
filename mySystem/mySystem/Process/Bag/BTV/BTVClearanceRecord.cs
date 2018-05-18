using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace mySystem.Process.Bag.BTV
{
    public partial class BTVClearanceRecord : BaseForm
    {
        // TODO   需要从Parameter 中读取生产指令ID或编号，这里假装填写当前生产指令编号和ID
        string CODE;
        int ID;
        // TODO : 注意处理生产指令的状态
        // TODO: 打印
        // TODO：要加到mainform中去，现在连按钮都没有

        private CheckForm checkform = null;

        // 需要保存的状态
        /// <summary>
        /// 0:操作员，1：审核员，2：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        // 当前数据在自己表中的id
        int _id;
        String _code;
        Boolean isFirstBind = true;
        // 显示界面需要的信息
        String str产品代码;
        String str产品批号;
        Int32 i生产指令ID;
        List<String> ls操作员;
        List<String> ls审核员;
        List<String> ls清场项目;
        List<String> ls清场要点;


        // 数据库连接
        
        SqlConnection conn;
        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;


        public BTVClearanceRecord(MainForm mainform)
            : base(mainform)
        {
            // 判断设置是否变化
            InitializeComponent();
            variableInit();
            // 若为未保存状态，则判断设置是否变化
            getOtherData();
            getPeople();
            setUseState();

            // 读取数据
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                if (Parameter.UserState.审核员 == _userState)
                {
                    MessageBox.Show("审核员不能新建指令");
                    setControlFalse();
                    btn查看日志.Enabled = false;
                    btn打印.Enabled = false ;
                    return;
                }
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData();
                outerBind();
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                setDataGridViewColumn();
                innerBind();
                generateRowOfDataTableInner();
                setDataGridViewNO();

                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }
            else
            {
                cb白班.Checked = dtOuter.Rows[0]["生产班次"].ToString() == "白班";
                cb夜班.Checked = !cb白班.Checked;

                if (dtOuter.Rows[0]["审核员"].ToString() != "")
                {
                    cb合格.Checked = (bool)dtOuter.Rows[0]["审核是否通过"];
                    cb不合格.Checked = !cb合格.Checked;
                }
            }

            if (null == dtInner)
            {
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                setDataGridViewColumn();
                innerBind();
            }
            matchInnerData();

            // 获取和显示内容有关的变量
            setFormVariable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));

            // 设置状态和控件可用性
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        public BTVClearanceRecord(MainForm mainform, int id)
            : base(mainform)
        {
            
            InitializeComponent();
            variableInit(id);
            // 若为未保存状态，则判断设置是否变化
            getOtherData();
            getPeople();


            // 读取数据
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();
            setDataGridViewNO();
            // 获取和显示内容有关的变量
            setFormVariable(id);

            // 设置状态和控件可用性
            setUseState();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        void variableInit()
        {
            conn = mySystem.Parameter.conn;
            
            ID = mySystem.Parameter.bpvbagInstruID;
            i生产指令ID = ID;
            CODE = mySystem.Parameter.bpvbagInstruction;
          
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            ls清场项目 = new List<string>();
            ls清场要点 = new List<string>();
            
        }

        void variableInit(int id)
        {
            conn = mySystem.Parameter.conn;

            SqlDataAdapter da = new SqlDataAdapter("select * from 清场记录 where ID=" + id, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            
            i生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            ID = i生产指令ID;
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            ls清场项目 = new List<string>();
            ls清场要点 = new List<string>();
        }

        

        void getOtherData()
        {
            fill_printer(); //添加打印机

            // 读取用于显示界面的重要信息
            SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + i生产指令ID, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            str产品代码 = dt.Rows[0]["产品代码"].ToString();
            str产品批号 = dt.Rows[0]["产品批号"].ToString();

            da = new SqlDataAdapter("select * from 设置清场项目", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls清场项目.Add(dr["清场项目"].ToString());
                ls清场要点.Add(dr["清场要点"].ToString());
            }

        }

        void setUseState()
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

        // 读取数据，根据自己表的ID
        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 清场记录 where ID=" + id, conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("清场记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        // 读取数据，无参数表示从Paramter中读取数据
        void readOuterData()
        {
            String sql = "select * from 清场记录 where 生产指令ID={0} and 生产日期='{1}'";
            DateTime date = DateTime.Parse(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            daOuter = new SqlDataAdapter(String.Format(sql, i生产指令ID, date), conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("清场记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);


        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = i生产指令ID;
            dr["产品代码"] = str产品代码;
            dr["产品批号"] = str产品批号;
            dr["生产日期"] = DateTime.Parse(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["生产班次"] = mySystem.Parameter.userflight;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["检查结果"] = "合格";
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + CODE + "\n";
            dr["日志"] = log;
            dr["审核是否通过"] = false;
            cb白班.Checked = mySystem.Parameter.userflight == "白班";
            cb夜班.Checked = !cb白班.Checked;
            return dr;
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            //foreach (Control c in this.Controls)
            //{
            //    if (c.Name.StartsWith("tb"))
            //    {
            //        (c as TextBox).DataBindings.Clear();
            //        (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2));
            //    }
            //    else if (c.Name.StartsWith("lbl"))
            //    {
            //        (c as Label).DataBindings.Clear();
            //        (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
            //    }
            //    else if (c.Name.StartsWith("cmb"))
            //    {
            //        (c as ComboBox).DataBindings.Clear();
            //        (c as ComboBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
            //    }
            //    else if (c.Name.StartsWith("dtp"))
            //    {
            //        (c as DateTimePicker).DataBindings.Clear();
            //        (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
            //    }
            //}

            tb产品代码.DataBindings.Clear();
            tb产品代码.DataBindings.Add("Text", bsOuter.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bsOuter.DataSource, "产品批号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Value", bsOuter.DataSource, "生产日期");
            tb操作员.DataBindings.Clear();
            tb操作员.DataBindings.Add("Text", bsOuter.DataSource, "操作员");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");


        }
        void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 清场记录详细信息 where T清场记录ID=" + dtOuter.Rows[0]["ID"], conn);
            dtInner = new DataTable("清场记录详细信息");
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void matchInnerData()
        {
            if (0 != _formState) return;
            bool isChanged = false;
            if (ls清场要点.Count != dtInner.Rows.Count) isChanged = true;
            else
            {
                for (int i = 0; i < ls清场项目.Count; ++i)
                {
                    if (dtInner.Rows[i]["清场项目"].ToString() != ls清场项目[i] ||
                        dtInner.Rows[i]["清场要点"].ToString() != ls清场要点[i])
                    {
                        isChanged = true;
                    }
                }
            }

            if (isChanged)
            {
                if (DialogResult.OK == MessageBox.Show("检测到清场设置项已被修改，是否使用最新的设置?",
                    "提示", MessageBoxButtons.OKCancel))
                {
                    foreach (DataRow dr in dtInner.Rows) dr.Delete();
                    daInner.Update((DataTable)bsInner.DataSource);
                    readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                    innerBind();
                    generateRowOfDataTableInner();
                    daInner.Update((DataTable)bsInner.DataSource);
                    readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                    innerBind();
                }
            }

        }

        void generateRowOfDataTableInner()
        {
            for (int i = 0; i < ls清场项目.Count; ++i)
            {
                DataRow dr = dtInner.NewRow();
                dr = writeInnerDefault(dr, ls清场项目[i], ls清场要点[i]);
                dtInner.Rows.Add(dr);
            }
        }
        void setDataGridViewColumn()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "清洁操作")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("完成");
                    cbc.Items.Add("不适用");
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

            // 然后修改其他特殊属性
            
        }

        // 写序号
        void setDataGridViewNO()
        {

            for (int i = 0; i < dtInner.Rows.Count;++i )
            {
                dtInner.Rows[i]["序号"] = i + 1;
            }
        }

        DataRow writeInnerDefault(DataRow dr, String xm, String yd)
        {
            dr["T清场记录ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["序号"] = 0;
            dr["清场项目"] = xm;
            dr["清场要点"] = yd;
            dr["清洁操作"] = "完成";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
        }

        // 获取和显示内容有关的变量
        void setFormVariable(int id)
        {
            _id = id;
        }


        void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='清场记录'", conn);
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

        void setFormState()
        {
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = Parameter.FormState.未保存;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }
        void setEnableReadOnly()
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

        }

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
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;

            cb白班.Enabled = false;
            cb夜班.Enabled = false;
            cb合格.Enabled = false;
            cb不合格.Enabled = false;
            tb审核员.ReadOnly = true;
        }

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
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;

            cb白班.Enabled = false;
            cb夜班.Enabled = false;
            cb合格.Enabled = false;
            cb不合格.Enabled = false;

        }

        // 事件部分
        void addComputerEventHandler()
        {
        }

     
        void addOtherEvenHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // DataGridView的可见和只读等属性最好在这个事件中处理
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            
            for (int i = 0; i < 5; ++i)
            {
                dataGridView1.Columns[i].ReadOnly = true;
            }
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
                btn提交审核.Enabled = true;
        }

        //保存功能
        private bool Save()
        {
            if (mySystem.Parameter.NametoID(tb操作员.Text) == 0)
            {
                MessageBox.Show("操作员ID不存在");
                return false;
            }
            else if (datagridview_check() == false)
            {
                return false;
            }
            else
            {
                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                outerBind();

                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();

                return true;
            }
        }
        private bool datagridview_check()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["清洁操作"].Value.ToString() == "不适用")
                {
                    MessageBox.Show("有带确认项目未完成");
                    return false;
                }                 
            }
            return true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            if (!dataValidate())
            {
                MessageBox.Show("数据填写不完整，请仔细检查！");
                return;
            }

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='清场记录' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "清场记录";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        bool dataValidate()
        {
            return true;
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            mySystem.Other.LogForm logform = new mySystem.Other.LogForm();
            logform.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
        }

        public override void CheckResult()
        {
            base.CheckResult();

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='清场记录' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核意见"] = checkform.opinion;
            dtOuter.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            cb合格.Checked = checkform.ischeckOk;
            cb不合格.Checked = !cb合格.Checked;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter("select * from 待审核 where 表名='清场记录' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            // TODO 弹出赵梦的窗口
            if (mySystem.Parameter.userName == tb操作员.Text)
            {
                MessageBox.Show("审核员不能和操作员为同一个人");
                return;
            }
            checkform = new CheckForm(this);
            checkform.ShowDialog();
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
        public int print(bool preview)
        {
            int pageCount = 0;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\SOP-MFG-110-R01A 清场记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            int rowStartAt = 5;
            my.Cells[3, 1].Value = "生产指令编号：" + dtOuter.Rows[0]["生产指令编号"];
            my.Cells[3, 1].Value = "产品代码/规格：" + dtOuter.Rows[0]["产品代码"];
            my.Cells[3, 5].Value = "生产批号：" + dtOuter.Rows[0]["生产批号"];
            my.Cells[3, 7].Value = "生产日期：" + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日")+"生产班次："+ dtOuter.Rows[0]["生产班次"];


            //EVERY SHEET CONTAINS 14 RECORDS
            int rowNumPerSheet = 13;
            int rowNumTotal = dtInner.Rows.Count;
            for (int i = 0; i < (rowNumTotal > rowNumPerSheet ? rowNumPerSheet : rowNumTotal); i++)
            {

                my.Cells[i + rowStartAt, 1].Value = dtInner.Rows[i]["序号"];
                my.Cells[i + rowStartAt, 2].Value = dtInner.Rows[i]["清场项目"];
                //my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产日期时间"]).ToString("MM/dd HH:mm");
                //my.Cells[i + rowStartAt, 2].Font.Size = 11;
                my.Cells[i + rowStartAt, 3].Value = dtInner.Rows[i]["清场要点"];
                my.Cells[i + rowStartAt, 6].Value = dtInner.Rows[i]["清洁操作"];
               
            }

            //THIS PART HAVE TO INSERT NOEW BETWEEN THE HEAD AND BOTTM
            if (rowNumTotal > rowNumPerSheet)
            {
                for (int i = rowNumPerSheet; i < rowNumTotal; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    my.Cells[i + rowStartAt, 1].Value = dtInner.Rows[i]["序号"];
                    my.Cells[i + rowStartAt, 2].Value = dtInner.Rows[i]["清场项目"];
                    //my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产日期时间"]).ToString("MM/dd HH:mm");
                    //my.Cells[i + rowStartAt, 2].Font.Size = 11;
                    my.Cells[i + rowStartAt, 3].Value = dtInner.Rows[i]["清场要点"];
                    my.Cells[i + rowStartAt, 6].Value = dtInner.Rows[i]["清洁操作"];
               
                }
            }

            Microsoft.Office.Interop.Excel.Range range1 = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + rowNumTotal, Type.Missing];
            range1.EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);

            //THE BOTTOM HAVE TO CHANGE LOCATE ACCORDING TO THE ROWS NUMBER IN DT.
            int varOffset = (rowNumTotal > rowNumPerSheet) ? rowNumTotal - rowNumPerSheet - 1 : 0;
            my.Cells[19 + varOffset, 1].Value = "备注:\n"+dtOuter.Rows[0]["备注"];
            my.Cells[5, 7].Value = dtOuter.Rows[0]["操作员"];
            my.Cells[5, 8].Value = dtOuter.Rows[0]["检查结果"];
            my.Cells[5, 9].Value = dtOuter.Rows[0]["审核员"];
            if (preview)
            {
                my.Select();
                oXL.Visible = true; //加上这一行  就相当于预览功能            
            }
            else
            {
                //add footer
                my.PageSetup.RightFooter = CODE + "-10-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
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
            return pageCount;
        }


        int find_indexofprint()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 清场记录 where 生产指令ID=" + ID, mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
        }

        private void BTVClearanceRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
        }

       

    }
}

