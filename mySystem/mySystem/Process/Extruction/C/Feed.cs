using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Process.Extruction.C
{
    public partial class Feed : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜供料系统运行记录";
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        BindingSource bsOuter;
        OleDbCommandBuilder cbOuter;

        string tablename2 = "吹膜供料系统运行记录详细信息";
        DataTable dtInner;
        OleDbDataAdapter daInner;
        BindingSource bsInner;
        OleDbCommandBuilder cbInner;

        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string __待审核 = "__待审核";
        int searchId;
        string __生产指令编号, __班次;
        DateTime __生产日期;
        private CheckForm check = null;
        int outerId;
        int status;
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>

        Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        public Feed(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            fill_printer();
            __生产指令编号 = Parameter.proInstruction;
            __生产日期 = Convert.ToDateTime(DateTime.Now.Date.ToString());
            __班次 = Parameter.userflight;
            lbl生产指令编号.Text = __生产指令编号;
            dtp生产日期.Value =__生产日期;
            for (int i = 0; i < flight.Count; i++)
            {
                cmb班次.Items.Add(flight[i]);
            }
            cmb班次.SelectedItem = __班次;
            //readOuterData(__生产指令编号, __生产日期, __班次);
            readOuterData(__生产指令编号);
            removeOuterBind();
            outerBind();
            if (0 == dtOuter.Rows.Count)
            {
                DataRow newrow = dtOuter.NewRow();
                newrow = writeOuterDefault(newrow);
                dtOuter.Rows.Add(newrow);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(__生产指令编号);
                //readOuterData(__生产指令编号, __生产日期, __班次);
                removeOuterBind();
                outerBind();
            }
            searchId = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            readInnerData(searchId);
            innerBind();
            setFormState();
            setEnableReadOnly();
            
        }

        public Feed(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            fill_printer();
            dtOuter = new DataTable(tablename1);
            daOuter = new OleDbDataAdapter("SELECT * FROM 吹膜供料系统运行记录 WHERE ID =" + Id, conOle);
            bsOuter = new BindingSource();
            cbOuter = new OleDbCommandBuilder(daOuter);
            daOuter.Fill(dtOuter);
            removeOuterBind();
            outerBind();
            __生产指令编号 = Convert.ToString(dtOuter.Rows[0]["生产指令编号"]);
            __生产日期=Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]);
            __班次=Convert.ToString( dtOuter.Rows[0]["班次"]);
            searchId = Id;
            cmb班次.Text = __班次;
            
            readInnerData(searchId);
            
            innerBind();

            setFormState();
            setEnableReadOnly();
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = Parameter.proInstruction;
            dr["生产指令ID"] = Parameter.proInstruID;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dr["班次"] = Parameter.userflight;
            dr["审核员"] = "";
            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 创建记录
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 创建记录\n";
            dr["日志"] = log;
            return dr;
        }
        private DataRow writeItemDefault(DataRow dr)
        {
            dr["T吹膜供料系统运行记录ID"] = dtOuter.Rows[0]["ID"];
            dr["检查时间"] = Convert.ToDateTime(DateTime.Now.TimeOfDay.ToString());
            dr["班次"] = mySystem.Parameter.userflight;
            dr["电机工作是否正常"] = "正常";
            dr["气动阀工作是否正常"] = "正常";
            dr["供料运行是否正常"] = "正常";
            dr["有无警报显示"] = "无";
            dr["是否解除警报"] = "无";
            dr["检查员"] = Parameter.userName;

            dr["检查员备注"] = "无";
            return dr;
        }

        void readOuterData(string __生产指令编号, DateTime __生产日期, string __班次)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + __生产指令编号 + "' AND 生产日期=#" + __生产日期 + "# AND 班次='" + __班次 + "';", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        void readOuterData(string __生产指令编号)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + __生产指令编号 + "'", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        void readOuterData(int Id)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE ID="+Id+";", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }
        private void setEnableReadOnly()
        {
            switch (_userState)
            {
                case Parameter.UserState.操作员: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlTrue();
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (Parameter.FormState.待审核 == _formState || Parameter.FormState.审核通过 == _formState)
                    {
                        setControlFalse();
                    }
                    else if (Parameter.FormState.无数据 == _formState)
                    {
                        setControlFalse();
                        
                    }
                    btn数据审核.Enabled = false;
                    break;
                case Parameter.UserState.审核员: //1--审核员
                    //the _formState is to be checked
                    if (Parameter.FormState.待审核 == _formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the _formState do not have to be checked
                    else if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核通过 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlFalse();
                    }
                    if (Parameter.FormState.审核通过 != _formState)
                    {
                        btn数据审核.Enabled = true;
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
            //some textboxes act as column name and row name, so these shoule be forbidden
           
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            lbl生产指令编号.Enabled = false;
            dtp生产日期.Enabled = false;
            cmb班次.Enabled = false;
            txb审核员.ReadOnly = true;
        }


        /// <summary>
        /// this guarantees the controls are uneditable
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
            //this act as the same in function upper
            
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cmb打印机选择.Enabled = true;
            bt查看人员信息.Enabled = true;
        }


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
        

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T吹膜供料系统运行记录ID=" + id, conOle);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable(tablename2);
            bsInner = new BindingSource();
            daInner.Fill(dtInner);
        }
        private void removeInnerData(int id)
        {
           OleDbDataAdapter daInnerTemp = new OleDbDataAdapter("DELETE FROM " + tablename2 + " WHERE T吹膜供料系统运行记录ID=" + id, conOle);
           OleDbCommandBuilder cbInnerTemp = new OleDbCommandBuilder(daInnerTemp);
           DataTable dtInnerTemp = new DataTable(tablename2+"Temp");
           BindingSource bsInnerTemp = new BindingSource();
           daInnerTemp.Fill(dtInnerTemp);
        }
        private void removeOuterBind()
        {
            lbl生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            cmb班次.DataBindings.Clear();
            txb审核员.DataBindings.Clear();
        }
        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            lbl生产指令编号.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsOuter.DataSource, "生产日期");
            cmb班次.DataBindings.Add("SelectedItem", bsOuter.DataSource, "班次");
            txb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
        }
        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            
            foreach (DataColumn dc in dtInner.Columns)
            {

                switch (dc.ColumnName)
                {
                    case "检查时间":
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
                    
                    //case "不良品数量":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    
                    //case "记录人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    //case "审核人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break; 

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
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            //dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
        }

        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + lbl生产指令编号.Text + "';";
            return (int)comm.ExecuteScalar();           
        }
        
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                //to update the Feed record
                base.CheckResult();
                txb审核员.Text = Parameter.userName;
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
                txb审核员.Text = Parameter.userName;
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

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("确认本表已经填完吗？提交审核之后不可修改", "提示", MessageBoxButtons.YesNo))
            {
                foreach (DataRow dr in dtInner.Rows)
                {
                    if (dr["审核员"].ToString() == "" || dr["审核员"].ToString() == __待审核)
                    {
                        MessageBox.Show("请先完成数据审核!");
                        return;
                    }
                }
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
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                //fill reviwer information
                dtOuter.Rows[0]["审核员"] = __待审核;
                //update log into table
                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                try
                {
                    readOuterData(searchId);
                }
                catch
                {
                    readOuterData(__生产指令编号);
                }
                removeOuterBind();
                outerBind();

                setFormState();
                setEnableReadOnly();
                btn提交审核.Enabled = false;
            }
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

        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {           
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(__生产指令编号);
            removeOuterBind();
            outerBind();

            
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            btnSave.Enabled = false;
            if (Parameter.UserState.操作员==_userState)
            {
                btn提交审核.Enabled = true;
            }
            try { (this.Owner as ExtructionMainForm).InitBtn(); }
            catch (NullReferenceException exp)
            {

            }
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeItemDefault(dr);
            dtInner.Rows.Add(dr);
            daInner.Update((DataTable)bsInner.DataSource);
           
            readInnerData(searchId);
            //dataGridView1.Rows.Clear();
            innerBind();

            //setDataGridViewRowNums();
            btnSave.Enabled = true;
            btn审核.Enabled = false;
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }
        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0)
            {
                return;
            }
            if (!("" == (Convert.ToString(dtInner.Rows[dataGridView1.SelectedCells[0].RowIndex]["审核员"]).ToString().Trim())))
            {
                return;
            }
           dtInner.Rows[dataGridView1.SelectedCells[0].RowIndex].Delete();
           daInner.Update((DataTable)bsInner.DataSource);
           
           readInnerData(searchId);
           //dataGridView1.Rows.Clear();
           innerBind();
           
        }

        private void clearInnerID()
        {
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                dtInner.Rows[i]["ID"] = DBNull.Value;
            }
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
            cmb打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cmb打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cmb打印机选择.Text);
            print(false);
            GC.Collect();
        }
		public void print(bool preview)
		{
			// 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\C\SOP-MFG-301-R07 吹膜供料系统运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[3];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 6].Value = "生产指令：" + __生产指令编号;
            my.Cells[5, 1].Value = __生产日期.ToString("yyyy年MM月dd日") + "   " + __班次;
            my.Cells[5, 9].Value = "";
            



            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                my.Cells[5 + i, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["检查时间"]).ToShortTimeString();
                my.Cells[5 + i, 3].Value = dtInner.Rows[i]["电机工作是否正常"];
                my.Cells[5 + i, 4].Value = dtInner.Rows[i]["气动阀工作是否正常"];
                my.Cells[5 + i, 5].Value = dtInner.Rows[i]["供料运行是否正常"];
                my.Cells[5 + i, 6].Value = dtInner.Rows[i]["有无警报显示"];
                my.Cells[5 + i, 7].Value = dtInner.Rows[i]["是否解除警报"];
                my.Cells[5 + i, 8].Value = dtInner.Rows[i]["检查员"];
            }
            my.Cells[5, 9].Value = dtOuter.Rows[0]["审核员"];
            // 让这个Sheet为被选中状态

            my.PageSetup.RightFooter = __生产指令编号 + "-07-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码
            
			if(preview)
			{
			my.Select();  
			 oXL.Visible=true; //加上这一行  就相当于预览功能
			}
			else
			{
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
            wb = null;
            my = null;
			}
		}

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        int find_indexofprint()
        {
            OleDbDataAdapter dat = new OleDbDataAdapter("select * from 生产指令信息表 where 生产指令编号='" + __生产指令编号+"'", mySystem.Parameter.connOle);
            DataTable dtt = new DataTable();
            dat.Fill(dtt);
            int __pid = Convert.ToInt32(dtt.Rows[0]["ID"]);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where 生产指令ID=" + __pid, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
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
        }

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
                if (__待审核 == Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim())
                {
                    if (Parameter.userName != dtInner.Rows[i]["检查员"].ToString())
                    {
                        dtInner.Rows[i]["审核员"] = Parameter.userName;
                    }
                    else
                    {
                        MessageBox.Show("检查员和审核员不能相同");
                    }
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜供料系统运行记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);
            String str操作员 = dt.Rows[0]["操作员"].ToString();
            String str审核员 = dt.Rows[0]["审核员"].ToString();
            String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
            MessageBox.Show(str人员信息);
        }
    }
	
}
