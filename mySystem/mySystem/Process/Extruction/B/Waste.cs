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
using System.Collections;
using System.Runtime.InteropServices;

//this form haven't check the usr information before save


namespace mySystem.Process.Extruction.B
{

    public partial class Waste : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜工序废品记录";
        DataTable dtWaste;
        OleDbDataAdapter daWaste;
        BindingSource bsWaste;
        OleDbCommandBuilder cbWaste;

        string tablename2 = "吹膜工序废品记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        Hashtable productCode;
        List<string> productCodeLst;
        List<string> wasteReason = new List<string>();
        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> usrList = new List<string>();
        List<string> list操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> list审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string __待审核 = "__待审核";
        string __生产指令;
        private CheckForm check = null;
        int outerId;
        int searchId;
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;
        public Waste(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            __生产指令 = Parameter.proInstruction;
            txb生产指令.Text = __生产指令;
            getProductCode();
            getStartTime();
            getUsrList();
            getWasteRason();
            
            dtp生产结束时间.Value = DateTime.Now;


            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();
            if (0 == dtWaste.Rows.Count)
            {
                DataRow newrow = dtWaste.NewRow();
                newrow = writeWasteDefault(newrow);
                dtWaste.Rows.Add(newrow);
                daWaste.Update((DataTable)bsWaste.DataSource);
                readWasteData(txb生产指令.Text);    
                removeWasteBinding();
                WasteBind();
            }
            searchId = Convert.ToInt32(dtWaste.Rows[0]["ID"]);


            readItemData(searchId);
            setDataGridViewColumns();
            setRowNums();
            ItemBind();
            sumWaste();
            setFormState();
            setEnableReadOnly();
            

        }
        public Waste(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
           

            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            searchId = Id;
            readWasteData(searchId);
            __生产指令 = Convert.ToString(dtWaste.Rows[0]["生产指令"]);
            
            getProductCode();
            getStartTime();
            getUsrList();
            getWasteRason();
            txb生产指令.Text = __生产指令;
            
            dtp生产结束时间.Value = DateTime.Now;


            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();
            if (0 == dtWaste.Rows.Count)
            {
                DataRow newrow = dtWaste.NewRow();
                newrow = writeWasteDefault(newrow);
                dtWaste.Rows.Add(newrow);
                daWaste.Update((DataTable)bsWaste.DataSource);
                readWasteData(txb生产指令.Text);
                removeWasteBinding();
                WasteBind();
            }
            


            readItemData(searchId);
            setDataGridViewColumns();
            setRowNums();
            ItemBind();

            sumWaste();
            setFormState();
            setEnableReadOnly();
            
        }
        private void readWasteData(int Id)
        {
            dtWaste = new DataTable(tablename1);
            daWaste = new OleDbDataAdapter("SELECT * FROM 吹膜工序废品记录 WHERE ID =" + Id, conOle);
            bsWaste = new BindingSource();
            cbWaste = new OleDbCommandBuilder(daWaste);
            daWaste.Fill(dtWaste);
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
            list操作员 = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
            list审核员 = new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });

        }
        private void setUserState()
        {
            if (list操作员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 0;
            }
            else if (list审核员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 1;
            }
            else
            {
                userState = 2;
            }
        }
        private void setFormState()
        {
            if (""==dtWaste.Rows[0]["审核人"].ToString().Trim())
            {
                //this means the record hasn't been saved
                formState = 0;
            }
            else if (__待审核 == dtWaste.Rows[0]["审核人"].ToString().Trim())
            {
                //this means this record should be checked
                formState = 1;
            }
            else if (Convert.ToBoolean(dtWaste.Rows[0]["审核是否通过"]))
            {
                //this means this record has been checked
                formState = 2;
            }
            else
            {
                //this means the record has been checked but need more modification
                formState = 3;
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
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
       
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        
        
        //for different user, the form will open different controls
        private void setEnableReadOnly()
        {
            switch (userState)
            {
                case 0: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (0 == formState || 3 == formState)
                    {
                        setControlTrue();
                        btn提交数据审核.Enabled = true;
                        btn数据审核.Enabled = false;
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (1 == formState || 2 == formState)
                    {
                        setControlFalse();
                    }
                    
                    break;
                case 1: //1--审核员
                    //the formState is to be checked
                    if (1 == formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the formState do not have to be checked
                    else if (0 == formState || 2 == formState || 3 == formState)
                    {
                        setControlFalse();
                        
                    }
                    if (2 != formState)
                    {
                        btn数据审核.Enabled = true;
                    }
                    else
                    {
                        btn数据审核.Enabled = false;
                    }
                    break;
                case 2: //2--管理员
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
        // 当登陆人是审核人时，在外面设置它为true
        // 以上两个函数的写法见示例


        // 如果有需要单行审核的表，在DataGridView下加一个“提交数据审核”按钮和“数据审核”，点击该按钮后，DataGridView中无“审核人”的行都填入：__待审核，同时设为ReadOnly
        // 下面这个函数完成功能：遍历DataGridView的行：只要审核人不为空，则该行ReadOnly
        // 该函数需要在DataGridView的DataBindingComplete事件中和“提交数据审核”点击事件中调用
        //void setDataGridViewColumnReadOnly();
        // 注意：删除按钮点击是要判断：如果该行有审核人信息，则无法删除
        /// 控件状态类 ================================================

        /// 需要单行审核的审核事件 ================================================
        // 下面函数当碰到审核人时，将审核人不为空也不为“__待审核”的设为只读（也就是有了审核结果的）
        //void setDataGridViewColumnReadOnly();
        // “数据审核按钮”点击事件遍历整个DataGridView，找到“审核人”为“__待审核”的行，修改“审核人”为自己
        // 然后调用setDataGridViewColumnReadOnly();

        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                //to update the Waste record
                base.CheckResult();
                
                dtWaste.Rows[0]["审核人"] = check.userName.ToString();
                dtWaste.Rows[0]["审核意见"] = check.opinion.ToString();
                dtWaste.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtWaste.Rows[0]["日志"] = dtWaste.Rows[0]["日志"].ToString() + log;


                bsWaste.EndEdit();
                daWaste.Update((DataTable)bsWaste.DataSource);

                readWasteData(searchId);

                removeWasteBinding();
                WasteBind();

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
                    newrow["对应ID"] = dtWaste.Rows[0]["ID"];
                    dtCheck.Rows.Add(newrow);
                }
                //remove the record
                dtCheck.Rows[0].Delete();
                bsCheck.DataSource = dtCheck;
                daCheck.Update((DataTable)bsCheck.DataSource);
                formState = 2;
                setEnableReadOnly();
            }
            else
            {
                //check unpassed
                base.CheckResult();
            
                dtWaste.Rows[0]["审核人"] = check.userName.ToString();
                dtWaste.Rows[0]["审核意见"] = check.opinion.ToString();
                dtWaste.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtWaste.Rows[0]["日志"] = dtWaste.Rows[0]["日志"].ToString() + log;


                bsWaste.EndEdit();
                daWaste.Update((DataTable)bsWaste.DataSource);

                readWasteData(searchId);

                removeWasteBinding();
                WasteBind();
                formState = 3;
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
        private void readWasteData(String name)
        {
            daWaste = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令='" + name+"';", conOle);
            cbWaste = new OleDbCommandBuilder(daWaste);
            dtWaste = new DataTable(tablename1);
            bsWaste = new BindingSource();
            daWaste.Fill(dtWaste);
        }

        private DataRow writeWasteDefault(DataRow dr)
        {
            dr["生产指令ID"] = Parameter.proInstruID;
			dr["生产指令"]=txb生产指令.Text;
			dr["生产开始时间"]=Convert.ToDateTime(dtp生产开始时间.Value.ToString());
			dr["生产结束时间"]=Convert.ToDateTime(dtp生产结束时间.Value.ToString());
            dr["审核人"] = "";
            dr["合计不良品数量"] = 0;
            return dr;
        }
        private void getStartTime()
        {
            string sqlStr = "SELECT 开始生产日期 FROM 生产指令信息表 WHERE ID = " + Parameter.proInstruID.ToString();
            OleDbCommand sqlCmd = new OleDbCommand(sqlStr, conOle);
            dtp生产开始时间.Value = Convert.ToDateTime(sqlCmd.ExecuteScalar());
        }

        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("生产指令产品列表");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 生产指令产品列表 WHERE 生产指令ID = " + Parameter.proInstruID, conOle);
            da.Fill(DtproductCode);
            productCode = new Hashtable();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品编码"].ToString(), dr["产品批号"].ToString());
            }
            productCodeLst = new List<string>();
            foreach (string s in productCode.Keys.OfType<string>().ToList<string>())
            {
                productCodeLst.Add(s);
            }
        }
        private void getUsrList()
        {
            DataTable DtUsr = new DataTable("users");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT 姓名 FROM users", Parameter.connOleUser);
            da.Fill(DtUsr);
            
            usrList = new List<string>();
            for(int i=0;i<DtUsr.Rows.Count;i++)
            {
                usrList.Add(Convert.ToString( DtUsr.Rows[i]["姓名"]));
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
        private void WasteBind()
        {
            bsWaste.DataSource = dtWaste;
            txb生产指令.DataBindings.Add("Text", bsWaste.DataSource, "生产指令");
            dtp生产开始时间.DataBindings.Add("Value", bsWaste.DataSource, "生产开始时间");
            dtp生产结束时间.DataBindings.Add("Value", bsWaste.DataSource, "生产结束时间");
            txb合计不良品数量.DataBindings.Add("Text", bsWaste.DataSource, "合计不良品数量");
            txb审核人.DataBindings.Add("Text", bsWaste.DataSource, "审核人");
        }

        private void removeWasteBinding()
        {
            txb生产指令.DataBindings.Clear();
            dtp生产开始时间.DataBindings.Clear();
            dtp生产结束时间.DataBindings.Clear();
            txb合计不良品数量.DataBindings.Clear();
            txb审核人.DataBindings.Clear();
        }

        private void readItemData(int id)
        {
            daItem = new OleDbDataAdapter("SELECT * FROM "+tablename2 +" WHERE T吹膜工序废品记录ID=" + id, conOle);
            cbItem = new OleDbCommandBuilder(daItem);
            dtItem = new DataTable(tablename2);
            bsItem = new BindingSource();
            daItem.Fill(dtItem);
        }

        private void ItemBind()
        {
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }

        private void removeItemBinding()
        {
            ;
        }
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtItem.Columns)
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
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
        }



        private DataRow writeItemDefault(DataRow dr)
        {
            dr["T吹膜工序废品记录ID"] = dtWaste.Rows[0]["ID"];
            dr["序号"]=0;
            dr["生产日期"] = Convert.ToDateTime(DateTime.Now.ToString());
            dr["班次"] = Parameter.userflight;
            dr["产品代码"] = "";
            dr["不良品数量"] = 0;
            dr["废品产生原因"] = "";
            dr["记录人"] = Parameter.userName;
            dr["审核人"] = "";
            return dr;
        }

        //check the operator, make sure the operator exists in userlist
        private void btn保存_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if (usrList.IndexOf(dtItem.Rows[i]["记录人"].ToString().Trim()) < 0) 
                {
                    MessageBox.Show("用户不存在");
                    return;
                }
            }

            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtWaste.Rows[0]["ID"]));
            ItemBind();
            setRowNums();

            bsWaste.EndEdit();
            daWaste.Update((DataTable)bsWaste.DataSource);
            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();
            btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dtItem.Rows)
            {
                if (dr["审核人"].ToString() == "")
                {
                    MessageBox.Show("请先提交数据审核!");
                    return;
                }
            }
            //after saving, inner item haven't changed but we update once more here
            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(searchId);
            ItemBind();
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
                newrow["对应ID"] = dtWaste.Rows[0]["ID"];
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
            dtWaste.Rows[0]["日志"] = dtWaste.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtWaste.Rows[0]["审核人"] = __待审核;
            //update log into table
            bsWaste.EndEdit();
            daWaste.Update((DataTable)bsWaste.DataSource);

            readWasteData(searchId);
            removeWasteBinding();
            WasteBind();

            btn提交审核.Enabled = false;
            // insert into database
            formState = 1;
            setEnableReadOnly();
        }

        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            //find the uncheck item in inner list and tag the revoewer __待审核
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if (Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim() == "")
                {
                    dtItem.Rows[i]["审核人"] = __待审核;                    
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtWaste.Rows[0]["ID"]));
            ItemBind();
            setRowNums();
            setDataGridViewColumnReadOnly();
        }


        /// <summary>
        /// for the checked record set rows readonly
        /// </summary>
        private void setDataGridViewColumnReadOnly()
        {
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if ((Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim() != "")&&(Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim() != __待审核))
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                continue;
            }
            dataGridView1.Columns[9].ReadOnly = true;
        }




        //this function just fill the name but dooesn't catch the opinion
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            //find the item in inner tagged the reviewer __待审核 and replace the content his name
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if (__待审核==Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim())
                {
                    dtItem.Rows[i]["审核人"] = Parameter.userName;
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtWaste.Rows[0]["ID"]));
            ItemBind();
            setRowNums();
        }
        private void btn添加_Click(object sender, EventArgs e)
        {
            // 内表中添加一行
            DataRow dr = dtItem.NewRow();
            dr = writeItemDefault(dr);
            dtItem.Rows.Add(dr);
            setRowNums();
            btn保存.Enabled = true;
            
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            { MessageBox.Show(dtWaste.Rows[0]["日志"].ToString()); }
            catch
            { MessageBox.Show(" !"); }
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
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                dtItem.Rows[i]["序号"] = i + 1;
            }
        }


        private void sumWaste()
        {
            double sum = 0;
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dtItem.Rows[i]["不良品数量"]);
            }
            dtWaste.Rows[0]["合计不良品数量"] = sum.ToString();
            txb合计不良品数量.DataBindings.Clear();
            txb合计不良品数量.DataBindings.Add("Text", bsWaste.DataSource, "合计不良品数量");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (6 == e.ColumnIndex)
            {
                sumWaste();
                
            }
            if (8 == e.ColumnIndex || 9 == e.ColumnIndex)     //how to check the usr name of this list
            {
                if(usrList.IndexOf(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim())<0)
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

            if ((Convert.ToString(dtItem.Rows[dataGridView1.SelectedCells[0].RowIndex]["审核人"]).ToString().Trim() != ""))
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                //this line disable
                daItem.Update((DataTable)bsItem.DataSource);
                readItemData(Convert.ToInt32(dtWaste.Rows[0]["ID"]));
                ItemBind();
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

        private void btn打印_Click(object sender, EventArgs e)
        {
            print(true);
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

            my.Cells[3, 1].Value = "生产指令：" + txb生产指令.Text;
            my.Cells[3, 6].Value = "生产时段：" + dtp生产开始时间.Value.ToLongDateString() + "--" + dtp生产结束时间.Value.ToLongDateString();
            

            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                my.Cells[i + 5, 1].Value = dtItem.Rows[i]["序号"];
                my.Cells[i + 5, 2].Value = Convert.ToDateTime(dtItem.Rows[i]["生产日期"]).ToLongDateString();
                my.Cells[i + 5, 3].Value = dtItem.Rows[i]["班次"];
                my.Cells[i + 5, 4].Value = dtItem.Rows[i]["产品代码"];
                my.Cells[i + 5, 5].Value = dtItem.Rows[i]["不良品数量"].ToString();
                my.Cells[i + 5, 6].Value = dtItem.Rows[i]["废品产生原因"];
                my.Cells[i + 5, 7].Value = dtItem.Rows[i]["记录人"];
                my.Cells[i + 5, 8].Value = dtItem.Rows[i]["审核人"];               
            }

            //my.Cells[16, 10] = "A层 " + array1[9][11].Text + "  (℃)";
            //my.Cells[16, 12] = "B层 " + array1[11][11].Text + "  (℃)";
            //my.Cells[16, 14] = "C层 " + array1[13][11].Text + "  (℃)";

			if(preview)
			{
            // 让这个Sheet为被选中状态
                my.Select();  
				 oXL.Visible=true; //加上这一行  就相当于预览功能
			}else
			{
            // 直接用默认打印机打印该Sheet
            //my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
			}
		}

        
    }
}
