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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace mySystem.Process.CleanCut
{
    public partial class DailyRecord : mySystem.BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜工序废品记录";
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        BindingSource bsOuter;
        OleDbCommandBuilder cbOuter;

        string tablename2 = "吹膜工序废品记录详细信息";
        DataTable dtInner;
        OleDbDataAdapter daInner;
        BindingSource bsInner;
        OleDbCommandBuilder cbInner;

        List<string> productCode;
        List<string> productCodeLst;
        List<string> wasteReason = new List<string>();
        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> usrList = new List<string>();
        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string __待审核 = "__待审核";
        string __生产指令;
        int __生产指令ID;
        private CheckForm check = null;
        int outerId;
        int searchId;

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>

        Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        public DailyRecord()
            : base()
        {
            InitializeComponent();
            //getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()

        }

        public DailyRecord(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            //getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()

        }

        public DailyRecord(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            
        }
        private void readOuterData(int Id)
        {
            dtOuter = new DataTable(tablename1);
            daOuter = new OleDbDataAdapter("SELECT * FROM 吹膜工序废品记录 WHERE ID =" + Id, conOle);
            bsOuter = new BindingSource();
            cbOuter = new OleDbCommandBuilder(daOuter);
            daOuter.Fill(dtOuter);
        }
/*
        private void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", conOle);
            BindingSource bsUser = new BindingSource();
            OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
            daUser.Fill(dtUser);
            if (dtUser.Rows.Count != 1)
            {
                MessageBox.Show("请确认表单权限信息");
                this.Close();
            }

            //the getPeople and setUserState combine here
            ls操作员 = new List<string>(Regex.Split(dtUser.Rows[0]["操作员"].ToString(), ",|，"));
            ls审核员 = new List<string>(Regex.Split(dtUser.Rows[0]["审核员"].ToString(), ",|，"));

        }




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
        private void setFormState(bool newForm = false)
        {
            if (newForm)
            {

                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }

        }

        // 获取其他需要的数据，比如产品代码，产生废品原因等
        //void getOtherData();
        // 获取操作员和审核员
        //void getPeople();
        // 计算，主要用于日报表、物料平衡记录中的计算
        //void compute();


        /// 数据读取类函数 ================================================


        /// 主要事件处理，格式处理
        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        // 这个函数中先通过遍历把列加全，并设置全局属性（列的类型，是否可排序）； 然后再设置各类的可见性等属性


        // 设置各控件的事件 ================================================
        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        //void addDateEventHandler();
        // 设置自动计算类事件
        //void addComputerEventHandler();
        // 其他事件，比如按钮的点击，数据有效性判断
        //void addOtherEvnetHandler();
        // 打印函数

        /// 主要事件处理，格式处理 ================================================




        /// 控件状态类 ================================================
        // 获取当前窗体状态：
        // 如果『审核员』为空，则为未保存
        // 否则，如果『审核员』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』

        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员

        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过


        //for different user, the form will open different controls
        private void setEnableReadOnly()
        {
            if (Parameter.FormState.无数据 == _formState)
            {
                setControlFalse();
                btn添加.Enabled = true;
                //lbl生产指令.ReadOnly = false;
                return;
            }
            switch (_userState)
            {
                case Parameter.UserState.操作员: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlTrue();
                        btn提交数据审核.Enabled = true;
                        btn数据审核.Enabled = false;
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (Parameter.FormState.待审核 == _formState || Parameter.FormState.审核通过 == _formState)
                    {
                        setControlFalse();
                    }

                    break;
                case Parameter.UserState.审核员: //1--审核员
                    //the formState is to be checked
                    if (Parameter.FormState.待审核 == _formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the formState do not have to be checked
                    else if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核通过 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlFalse();

                    }
                    if (Parameter.FormState.审核通过 != _formState)
                    {
                        btn数据审核.Enabled = true;
                    }
                    else
                    {
                        btn数据审核.Enabled = false;
                    }
                    break;
                case Parameter.UserState.管理员: //2--管理员
                    setControlTrue();
                    break;
                default:
                    break;
            }
        }
        // 为了方便设置控件状态，完成如下两个函数：分别用于设置所有控件可用和所有控件不可用

        //this guarantee the controls are editable
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
            // 保证这两个按钮一直是false
            txb审核员.ReadOnly = true;
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
        }

        //this guarantees the controls are uneditable

        private void setControlFalse()
        //this guarantees the controls are uneditable
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
        }
        // “审核”和“提交审核”按钮特殊，在以上两个函数中要设为false。
        // 当登陆人是审核员时，在外面设置它为true
        // 以上两个函数的写法见示例


        // 如果有需要单行审核的表，在DataGridView下加一个“提交数据审核”按钮和“数据审核”，点击该按钮后，DataGridView中无“审核员”的行都填入：__待审核，同时设为ReadOnly
        // 下面这个函数完成功能：遍历DataGridView的行：只要审核员不为空，则该行ReadOnly
        // 该函数需要在DataGridView的DataBindingComplete事件中和“提交数据审核”点击事件中调用
        //void setDataGridViewColumnReadOnly();
        // 注意：删除按钮点击是要判断：如果该行有审核员信息，则无法删除
        /// 控件状态类 ================================================

        /// 需要单行审核的审核事件 ================================================
        // 下面函数当碰到审核员时，将审核员不为空也不为“__待审核”的设为只读（也就是有了审核结果的）
        //void setDataGridViewColumnReadOnly();
        // “数据审核按钮”点击事件遍历整个DataGridView，找到“审核员”为“__待审核”的行，修改“审核员”为自己
        // 然后调用setDataGridViewColumnReadOnly();

        public override void CheckResult()
        {
            //if (Parameter.userName == dtOuter.Rows[0]["记录员"].ToString())
            //{
            //    MessageBox.Show("记录员,审核员重复");
            //    return;
            //}
            if (check.ischeckOk)
            {
                //to update the Waste record
                base.CheckResult();

                dtOuter.Rows[0]["审核员"] = Parameter.userName;
                dtOuter.Rows[0]["审核意见"] = check.opinion.ToString();
                dtOuter.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 审核通过\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;


                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);

                readOuterData(searchId);

                removeOuterBind();
                outerBind();

                //to delete the unchecked table
                //read from database table and find current record
                string checkName = "待审核";
                DataTable dtCheck = new DataTable(checkName);
                OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", conOle);
                BindingSource bsCheck = new BindingSource();
                OleDbCommandBuilder cbCheck = new OleDbCommandBuilder(daCheck);
                daCheck.Fill(dtCheck);

                //this part will never be run, for there must be a unchecked recird before this button becomes enable
                if (0 == dtCheck.Rows.Count)
                {
                    DataRow newrow = dtCheck.NewRow();
                    newrow["表名"] = tablename1;
                    newrow["对应ID"] = dtOuter.Rows[0]["ID"];
                    dtCheck.Rows.Add(newrow);
                }
                //remove the record
                dtCheck.Rows[0].Delete();
                bsCheck.DataSource = dtCheck;
                daCheck.Update((DataTable)bsCheck.DataSource);
                setFormState();
                setEnableReadOnly();
            }
            else
            {
                //check unpassed
                base.CheckResult();

                dtOuter.Rows[0]["审核员"] = Parameter.userName;
                dtOuter.Rows[0]["审核意见"] = check.opinion.ToString();
                dtOuter.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;


                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);

                readOuterData(searchId);

                removeOuterBind();
                outerBind();
                setFormState();
                setEnableReadOnly();
            }
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        //// 给外表的一行写入默认值，包括操作人，时间，班次等
        //DataRow writeOuterDefault(DataRow);
        //// 给内表的一行写入默认值，包括操作人，时间，Y/N等
        //DataRow writeInnerDefault(DataRow);
        //// 根据条件从数据库中读取一行外表的数据
        //void readOuterData(能唯一确定一行外表数据的参数，一般是生产指令ID或生产指令编号)；
        //// 根据条件从数据库中读取多行内表数据
        //void readInnerData(int 外表行ID);
        //// 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        //void removeOuterBinding();
        //// 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        //void removeInner(Binding);
        //// 外表和控件的绑定
        //void outerBind();
        //// 内表和控件的绑定
        //void innerBind();
        //// 设置DataGridView中各列的格
        //void setDataGridViewColumns();
        //// 刷新DataGridView中的列：序号
        //void setDataGridViewRowNums();
        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令='" + name + "';", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = __生产指令ID;
            dr["生产指令"] = lbl生产指令.Text;
            dr["生产开始时间"] = Convert.ToDateTime(lbl生产开始时间.Text);
            dr["生产结束时间"] = Convert.ToDateTime(dtp生产结束时间.Value.ToString());
            dr["审核员"] = "";
            dr["合计不良品数量"] = 0;

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 创建记录
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 创建记录\n";
            dr["日志"] = log;
            return dr;
        }
        

        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("设置清洁分切产品");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 设置清洁分切产品", conOle);
            da.Fill(DtproductCode);
            productCode = new List<string>();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品名称"].ToString());
            }
        }
        

        private void getWasteRason()
        {
            DataTable Dt = new DataTable("设置废品产生原因");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT 废品产生原因 FROM 设置废品产生原因", conOle);
            da.Fill(Dt);

            wasteReason = new List<string>();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                wasteReason.Add(Convert.ToString(Dt.Rows[i]["废品产生原因"]));
            }
        }
        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("txb"))
                {
                    //(c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    //(c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    //(c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    //(c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        private void removeOuterBind()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("txb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    //(c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    //(c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    //(c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    //(c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T吹膜工序废品记录ID=" + id, conOle);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable(tablename2);
            bsInner = new BindingSource();
            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        private void removeInnerBind()
        {
            dataGridView1.DataBindings.Clear();
        }
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtInner.Columns)
            {

                switch (dc.ColumnName)
                {
                    case "班次":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in flight)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    case "产品代码":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in productCodeLst)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;

                    case "废品产生原因":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in wasteReason)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;


                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);

                        break;
                }
            }
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Width = 60;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[4].Width = 70;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].Width = 150;
            dataGridView1.Columns[6].Width = 120;
            dataGridView1.Columns[7].Width = 120;
            dataGridView1.Columns[9].Width = 120;
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
        }



        private DataRow writeInnerDefault(int Id)
        {
            OleDbDataAdapter daReadFromDatabase = new OleDbDataAdapter("SELECT * FROM 清洁分切生产记录详细信息 WHERE T清洁分切生产记录ID =" + Id, conOle);
            DataTable dtReadFromDatebase = new DataTable("清洁分切生产记录详细信息");
            BindingSource bsReadFromDatabase = new BindingSource();
            OleDbCommandBuilder cbReadFromDatebase = new OleDbCommandBuilder(daReadFromDatabase);
            daReadFromDatabase.Fill(dtReadFromDatebase);

            if (dtReadFromDatebase.Rows.Count < 1)
            {
                MessageBox.Show("未查询到相关信息");
                this.Close();
            }

            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE ", conOle);



            dr["T吹膜工序废品记录ID"] = dtOuter.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["生产日期"] = Convert.ToDateTime(DateTime.Now.ToString());
            dr["班次"] = Parameter.userflight;
            dr["产品代码"] = "";
            dr["不良品数量"] = 0;
            dr["废品产生原因"] = "";
            dr["记录员"] = Parameter.userName;
            dr["记录员备注"] = "";
            dr["审核员"] = "";
            return dr;
        }

        //check the operator, make sure the operator exists in userlist
        private void btn保存_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (usrList.IndexOf(dtInner.Rows[i]["记录员"].ToString().Trim()) < 0)
                {
                    MessageBox.Show("用户不存在");
                    return;
                }
            }

            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setRowNums();

            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(lbl生产指令.Text);
            removeOuterBind();
            outerBind();
            if (Parameter.UserState.操作员 == _userState)
            {
                btn提交审核.Enabled = true;
            }
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dtInner.Rows)
            {
                if (dr["审核员"].ToString() == "")
                {
                    MessageBox.Show("请先提交数据审核!");
                    return;
                }
            }
            //after saving, inner item haven't changed but we update once more here
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(searchId);
            innerBind();
            setRowNums();

            //read from database table and find current record
            string checkName = "待审核";
            DataTable dtCheck = new DataTable(checkName);
            OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", conOle);
            BindingSource bsCheck = new BindingSource();
            OleDbCommandBuilder cbCheck = new OleDbCommandBuilder(daCheck);
            daCheck.Fill(dtCheck);

            //if current hasn't been stored, insert a record in table
            if (0 == dtCheck.Rows.Count)
            {
                DataRow newrow = dtCheck.NewRow();
                newrow["表名"] = tablename1;
                newrow["对应ID"] = dtOuter.Rows[0]["ID"];
                dtCheck.Rows.Add(newrow);
            }
            bsCheck.DataSource = dtCheck;
            daCheck.Update((DataTable)bsCheck.DataSource);

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtOuter.Rows[0]["审核员"] = __待审核;
            //update log into table
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);

            readOuterData(searchId);
            removeOuterBind();
            outerBind();

            btn提交审核.Enabled = false;
            // insert into database
            setFormState();
            setEnableReadOnly();
        }

        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            //find the uncheck item in inner list and tag the revoewer __待审核
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() == "")
                {
                    dtInner.Rows[i]["审核员"] = __待审核;
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setRowNums();
            setDataGridViewColumnReadOnly();
        }


        /// <summary>
        /// for the checked record set rows readonly
        /// </summary>
        private void setDataGridViewColumnReadOnly()
        {
            //for (int i = 0; i < dtInner.Rows.Count; i++)
            //{
            //    if ((Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() != "") && (Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() != __待审核))
            //    {
            //        dataGridView1.Rows[i].ReadOnly = true;
            //    }
            //    continue;
            //}
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToString(dataGridView1.Rows[i].Cells["审核员"].Value).ToString().Trim() != "") && (Convert.ToString(dataGridView1.Rows[i].Cells["审核员"].Value).ToString().Trim() != __待审核))
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                continue;
            }
            dataGridView1.Columns[10].ReadOnly = true;
        }




        //this function just fill the name but dooesn't catch the opinion
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            //find the item in inner tagged the reviewer __待审核 and replace the content his name
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (__待审核 == Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim())
                {
                    if (Parameter.userName != dtInner.Rows[i]["记录员"].ToString())
                    {
                        dtInner.Rows[i]["审核员"] = Parameter.userName;
                    }
                    else
                    {
                        MessageBox.Show("记录员,审核员相同");
                    }
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setRowNums();
        }
        private void btn添加_Click(object sender, EventArgs e)
        {
            // 内表中添加一行
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
            setRowNums();
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            sumWaste();
            daOuter.Fill((DataTable)bsOuter.DataSource);
            removeOuterBind();
            outerBind();
            btn保存.Enabled = true;

        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();

                logForm.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void setRowNums()
        {
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                dtInner.Rows[i]["序号"] = i + 1;
            }
        }


        private void sumWaste()
        {
            double sum = 0;
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dtInner.Rows[i]["不良品数量"]);
            }
            outerDataSync("lbl合计不良品数量", (sum).ToString());
            //DataGridViewBindingCompleteEventArgs e=new DataGridViewBindingCompleteEventArgs;
            //dataGridView1_DataBindingComplete(dataGridView1,e);
            //dtOuter.Rows[0]["合计不良品数量"] = sum.ToString();
            //txb合计不良品数量.DataBindings.Clear();
            //txb合计不良品数量.DataBindings.Add("Text", bsOuter.DataSource, "合计不良品数量");
        }

        void outerDataSync(String name, String val)
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (6 == e.ColumnIndex)
            {
                sumWaste();

            }
            if (8 == e.ColumnIndex || 10 == e.ColumnIndex)     //how to check the usr name of this list
            {
                if (usrList.IndexOf(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim()) < 0)
                {
                    MessageBox.Show("用户不存在");
                }
            }

        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            if (name == "ID")
            {
                return;
            }
            MessageBox.Show(name + "填写错误");
        }

        private void btn删除_Click(object sender, EventArgs e)
        {

            if ("" == (Convert.ToString(dtInner.Rows[dataGridView1.SelectedCells[0].RowIndex]["审核员"]).ToString().Trim()))
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                //.Rows[dataGridView1.SelectedCells[0].RowIndex].Delete();
                //this line disable
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
                sumWaste();
                daOuter.Fill((DataTable)bsOuter.DataSource);
                removeOuterBind();
                outerBind();
            }
            else
            {
                MessageBox.Show("不可删除");
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            setDataGridViewColumnReadOnly();

        }



        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印机选择.Items.Add(sPrint);
            }
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cmb打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cmb打印机选择.Text);
            print(true);
            GC.Collect();
        }
        public void print(bool preview)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R10 吹膜工序废品记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 1].Value = "生产指令：" + lbl生产指令.Text;
            my.Cells[3, 6].Value = "生产时段：" + lbl生产开始时间.Text + "--" + dtp生产结束时间.Value.ToLongDateString();


            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                my.Cells[i + 5, 1].Value = dtInner.Rows[i]["序号"];
                my.Cells[i + 5, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["生产日期"]).ToLongDateString();
                my.Cells[i + 5, 3].Value = dtInner.Rows[i]["班次"];
                my.Cells[i + 5, 4].Value = dtInner.Rows[i]["产品代码"];
                my.Cells[i + 5, 5].Value = dtInner.Rows[i]["不良品数量"].ToString();
                my.Cells[i + 5, 6].Value = dtInner.Rows[i]["废品产生原因"];
                my.Cells[i + 5, 7].Value = dtInner.Rows[i]["记录人"];
                my.Cells[i + 5, 8].Value = dtInner.Rows[i]["审核员"];
            }

            //my.Cells[16, 10] = "A层 " + array1[9][11].Text + "  (℃)";
            //my.Cells[16, 12] = "B层 " + array1[11][11].Text + "  (℃)";
            //my.Cells[16, 14] = "C层 " + array1[13][11].Text + "  (℃)";

            if (preview)
            {
                // 让这个Sheet为被选中状态
                my.Select();
                oXL.Visible = true; //加上这一行  就相当于预览功能
            }
            else
            {

                //add footer
                my.PageSetup.RightFooter = __生产指令 + "02-1 /&/P";

                // 直接用默认打印机打印该Sheet
                //my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
                my = null;
                oXL = null;
                wb = null;
            }
        }

       */
    }
}
