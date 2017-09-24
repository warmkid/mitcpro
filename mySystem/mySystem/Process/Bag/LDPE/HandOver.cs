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

//main functions are listed below
//wneh the form showed, the information bust bind again so that can display

namespace mySystem.Process.Bag.LDPE
{
    public partial class HandOver : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "岗位交接班记录";
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        BindingSource bsOuter;
        OleDbCommandBuilder cbOuter;

        string tablename2 = "岗位交接班项目记录";
        DataTable dtInner;
        OleDbDataAdapter daInner;
        BindingSource bsInner;
        OleDbCommandBuilder cbInner;

        string tablename3 = "设置岗位交接班项目";
        DataTable dtSettingHandOver;
        OleDbDataAdapter daSettingHandOver;
        BindingSource bsSettingHandOver;
        OleDbCommandBuilder cbSettingHandOver;

        List<string> settingItem;
        List<string> flight = new List<string>(new string[] { "是","否" });
        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        int __生产指令ID;
        string __生产指令编号;
        DateTime __生产日期;
        private CheckForm check = null;
        string __待审核 = "__待审核";


       
        int searchId;
        int status;
        int outerId;
       
        Parameter.UserState _userState;      
        Parameter.FormState _formState;
        public HandOver(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            fill_printer();
            getPeople();
            setUserState();
            getOtherData();
            __生产指令编号 = Parameter.ldpebagInstruction;
            __生产日期 = DateTime.Now.Date;
            
            dtp生产日期.Value = __生产日期;

            readOuterData(__生产指令编号, __生产日期);
            removeOuterBind();
            outerBind();
            if (0 == dtOuter.Rows.Count)
            {
                DataRow newrow = dtOuter.NewRow();
                newrow = writeOuterDefault(newrow, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
                dtOuter.Rows.Add(newrow);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(__生产指令编号, __生产日期);
                removeOuterBind();
                outerBind();
            }
            __生产指令ID = Convert.ToInt32(dtOuter.Rows[0]["生产指令ID"]);

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumns();

            if (0 == dtInner.Rows.Count)
            {
                writeInnerDefault(dtInner);
            }           
            根据班次填写确认结果();
            setDataGridViewRowNums();
            InnerBind();            
            是否覆盖原有项目();
            填写交班员();

            //update Outer
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(__生产指令编号, __生产日期);
            removeOuterBind();
            outerBind();
            setEnableReadOnly(true);
          
        }

        public HandOver(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            fill_printer();
            searchId=Id;
            
            getPeople();
            setUserState();
            settingItem= 获取交接班项目();
            readOuterData(searchId);
            __生产指令编号 = Convert.ToString(dtOuter.Rows[0]["生产指令编号"]);
            __生产日期 = Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]);
            __生产指令ID = Convert.ToInt32(dtOuter.Rows[0]["生产指令ID"]);
            removeOuterBind();
            outerBind();
           
            readInnerData(searchId);
            setDataGridViewColumns();

            if (0 == dtInner.Rows.Count)
            {
                writeInnerDefault(dtInner);
            }
            setDataGridViewRowNums();
            InnerBind();
            是否覆盖原有项目();
            填写交班员();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(searchId);
            removeOuterBind();
            outerBind();
            setEnableReadOnly(true);            
        }

        /// <summary>
        /// this function read database and try to find a record, if not, insert a new record
        /// </summary>
        private void searchRecord()
        {
            
        }


        /// <summary>
        /// get settings anf fetch checking items
        /// </summary>
        private void getOtherData()
        {
            __生产指令ID = Parameter.ldpebagInstruID;
            __生产指令编号 = Parameter.ldpebagInstruction;
            __生产日期 = DateTime.Now.Date;
            settingItem = 获取交接班项目();
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
            label角色.Text += " " + Parameter.userflight;
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
        
    
        private void 是否覆盖原有项目()
        {
            if (dtInner.Rows.Count > 0)  //when there are some items 
            {
                if (!matchDt_List())
                {
                    if (DialogResult.OK == MessageBox.Show("确认项目与数据库不匹配,重新载入确认项目", "", MessageBoxButtons.OKCancel))
                    {
                        //this part will override the history
                        if (settingItem.Count < dtInner.Rows.Count)  // reduce confirm items and the cut item get ""
                        {

                            for (int i = 0; i < settingItem.Count; i++)
                            {
                                dtInner.Rows[i]["确认项目"] = settingItem[i];
                            }
                            for (int i = dtInner.Rows.Count - 1; i >= settingItem.Count; i--)
                            {
                                dtInner.Rows.RemoveAt(i);
                            }
                        }
                        if (settingItem.Count > dtInner.Rows.Count)
                        {
                            for (int i = 0; i < dtInner.Rows.Count; i++)
                            {
                                dtInner.Rows[i]["确认项目"] = settingItem[i];
                            }
                            for (int i = dtInner.Rows.Count; i < settingItem.Count; i++)
                            {
                                DataRow newrow = dtInner.NewRow();
                                newrow["T吹膜岗位交接班记录ID"] = dtOuter.Rows[0]["ID"];
                                newrow["序号"] = i + 1;
                                newrow["确认项目"] = settingItem[i];
                                dtInner.Rows.Add(newrow);
                            }
                        }
                    }
                }
            }
        }

        private bool matchDt_List()
        {
            bool rtn = true;
            if (dtInner.Rows.Count != settingItem.Count)
            {
                rtn = false;
            }
            else
            {
                for (int i = 0; i < dtInner.Rows.Count; i++)
                {
                    if (settingItem.IndexOf(Convert.ToString( dtInner.Rows[i]["确认项目"])) < 0)
                    {
                        rtn = false;
                    }
                }
            }
            return rtn; 
        }
        private void init()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DataError += dataGridView1_DataError;
            txb白班交班员.Enabled = false;
            txb白班接班员.Enabled = false;
            txb夜班交班员.Enabled = false;
            txb夜班接班员.Enabled = false;
            dtp白班交接班时间.Enabled = false;
            dtp夜班交接班时间.Enabled = false;
            dayNight((Convert.ToString( Parameter.userflight) == "白班")? true:false);
        }
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            if (name == "ID")
            {
                return;
            }
            MessageBox.Show(name + "填写错误");
        }

        private void dayNight(bool day)
        {
            if (day)
            {
                txb白班异常情况处理.Enabled = day;
                

                txb夜班异常情况处理.Enabled = false;
                
            }
            else
            {
                txb白班异常情况处理.Enabled = day;
               

                txb夜班异常情况处理.Enabled = true;
               
            }
        }

        DataRow writeOuterDefault(DataRow dr,bool day)
        {
            dr["生产指令ID"] = __生产指令ID;
            dr["生产指令编号"] = __生产指令编号;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.Date.ToString());
            if (day)
            {
                dr["白班异常情况处理"] = "";
                
                dr["白班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToString());
                dr["夜班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToString());
            }
            else
            {
                dr["夜班异常情况处理"] = "";
                
                dr["夜班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToString());
                dr["白班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToString());
            }

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 创建记录
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 创建记录\n";
            log += "生产指令编码：" + __生产指令编号 + "\n";
            dr["日志"] = log;
            return dr;
        }
        private void 填写交班员()
        {
            if ((Convert.ToString(Parameter.userflight) == "白班") && (dtOuter.Rows[0]["白班交班员"].ToString() == ""))
            {
                dtOuter.Rows[0]["白班交班员"] = Parameter.userName;                
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (dtOuter.Rows[0]["夜班交班员"].ToString() == ""))
            {
                dtOuter.Rows[0]["夜班交班员"] = Parameter.userName;
            }
        }
        void writeInnerDefault(DataTable dt)
        {
            for (int i = 0; i <settingItem.Count; i++)
            {
                DataRow dr = dtInner.NewRow();
                dr["T岗位交接班记录ID"] = dtOuter.Rows[0]["ID"];
                dr["确认项目"] = settingItem[i];               
                dtInner.Rows.Add(dr);
            }
        }
        void 根据班次填写确认结果()
        {
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if ((Convert.ToString(Parameter.userflight) == "白班")&&(txb夜班接班员.Text==""))
                {
                    dtInner.Rows[i]["确认结果白班"] = "是";
                }
                if ((Convert.ToString(Parameter.userflight) == "夜班") && (txb白班接班员.Text == ""))
                {
                    dtInner.Rows[i]["确认结果夜班"] = "是";
                }
            }
        }
        void readOuterData(string 生产指令编号, DateTime 生产日期)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + 生产指令编号 + "' AND 生产日期=#" + 生产日期.ToString() + "#;", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }
        void readOuterData(int searchId)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE ID=" +searchId+ ";", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }
        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T岗位交接班记录ID=" + id, conOle);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable(tablename2);
            bsInner = new BindingSource();
            daInner.Fill(dtInner);
        }

        private void removeOuterBind()
        {
            lbl生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            txb白班异常情况处理.DataBindings.Clear();
            txb白班交班员.DataBindings.Clear();
            txb白班接班员.DataBindings.Clear();
            dtp白班交接班时间.DataBindings.Clear();
            txb夜班异常情况处理.DataBindings.Clear();
            txb夜班交班员.DataBindings.Clear();
            txb夜班接班员.DataBindings.Clear();
            dtp夜班交接班时间.DataBindings.Clear();
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            lbl生产指令编号.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsOuter.DataSource, "生产日期");
            txb白班异常情况处理.DataBindings.Add("Text", bsOuter.DataSource, "白班异常情况处理");
            txb白班交班员.DataBindings.Add("Text", bsOuter.DataSource, "白班交班员");
            txb白班接班员.DataBindings.Add("Text", bsOuter.DataSource, "白班接班员");
            dtp白班交接班时间.DataBindings.Add("Value", bsOuter.DataSource, "白班交接班时间");
            txb夜班异常情况处理.DataBindings.Add("Text", bsOuter.DataSource, "夜班异常情况处理");
            txb夜班交班员.DataBindings.Add("Text", bsOuter.DataSource, "夜班交班员");
            txb夜班接班员.DataBindings.Add("Text", bsOuter.DataSource, "夜班接班员");
            dtp夜班交接班时间.DataBindings.Add("Value", bsOuter.DataSource, "夜班交接班时间");


            

        }

        private void InnerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtInner.Columns)
            {

                switch (dc.ColumnName)
                {
                    case "确认结果白班":
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
                    case "确认结果夜班":
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
            dataGridView1.Columns[2].ReadOnly=true;  //序号
            dataGridView1.Columns[3].ReadOnly = true; 
            // setting width in confirm items
            dataGridView1.Columns[3].Width=300;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4||e.ColumnIndex==5)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "是")
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "否")
                {
                    //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                }
            }
           
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                dtInner.Rows[i]["序号"] = i + 1;
            }
        }
        
     
        
      
        private List<string> 获取交接班项目()
        {
            List<string> settingList = new List<string>();
            bsSettingHandOver = new BindingSource();
            dtSettingHandOver = new DataTable(tablename3);
            daSettingHandOver = new OleDbDataAdapter("SELECT * FROM 设置岗位交接班项目", conOle);
            cbSettingHandOver = new OleDbCommandBuilder(daSettingHandOver);
            daSettingHandOver.Fill(dtSettingHandOver);
            bsSettingHandOver.DataSource = dtSettingHandOver;

            for (int i = 0; i < dtSettingHandOver.Rows.Count; i++)
            {            
                settingList.Add(Convert.ToString( dtSettingHandOver.Rows[i]["确认项目"]));
            }
            return settingList;
        }


        private Boolean 未确认()
        {
            bool rtn = false;
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "否" || dataGridView1.Rows[i].Cells[5].Value.ToString() == "否")
                {
                    rtn = true;
                    break;
                }
            }
            return rtn;
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            if (未确认())
            {
                MessageBox.Show("有未确认项目");
                return;
            }
           
            daInner.Update((DataTable)bsInner.DataSource);
            InnerBind();
           
            

            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(__生产指令编号,__生产日期);
            removeOuterBind();
            outerBind();

            btn提交审核.Enabled = true;
            
            
        }

    

      
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();

                if (Parameter.userflight == "夜班" && dtOuter.Rows[0]["夜班接班员"].ToString() == __待审核)
                {

                    dtOuter.Rows[0]["夜班接班员"] = Parameter.userName;
                    txb白班异常情况处理.Enabled = false;

                }
                if (Parameter.userflight == "白班" && dtOuter.Rows[0]["白班接班员"].ToString() == __待审核)
                {

                    dtOuter.Rows[0]["白班接班员"] = Parameter.userName;
                    txb白班异常情况处理.Enabled = false;

                }

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(__生产指令编号, __生产日期);
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

                setEnableReadOnly(true);

            }
            else 
            {
                MessageBox.Show("必须通过");
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            // 如果有不是yes的，就不准审核
            foreach (DataGridViewRow gdvr in dataGridView1.Rows)
            {
                if(gdvr.DefaultCellStyle.BackColor==Color.Red)
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
                
            }
            check = new CheckForm(this);
            check.Show();
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\LDPEBag\SOP-MFG-108-R01A  制袋岗位交接班记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 1].Value = "生产指令编号：" + dtOuter.Rows[0]["生产指令编号"];   // lbl生产指令编号.Text;
            my.Cells[3, 4].Value = "生产日期：" + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日"); //dtp生产日期.Value.ToString("yyyy年MM月dd日");


            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                my.Cells[i + 6, 1].Value = dtInner.Rows[i]["序号"];
                my.Cells[i + 6, 2].Value = dtInner.Rows[i]["确认项目"];
                my.Cells[i + 6, 4].Value = dtInner.Rows[i]["确认结果白班"];
                my.Cells[i + 6, 5].Value = dtInner.Rows[i]["确认结果夜班"];

            }
            my.Cells[8, 6].Value = "交班人：" + dtOuter.Rows[0]["白班交班员"].ToString() + "   接班人：" + dtOuter.Rows[0]["夜班接班员"].ToString() + "   时间：" + Convert.ToDateTime(dtOuter.Rows[0]["白班交接班时间"]).ToString("yyyy年MM月dd日");
            my.Cells[14, 6].Value = "交班人：" + dtOuter.Rows[0]["夜班交班员"].ToString() + "   接班人：" + dtOuter.Rows[0]["白班接班员"].ToString() + "   时间：" + Convert.ToDateTime(dtOuter.Rows[0]["夜班交接班时间"]).ToString("yyyy年MM月dd日");
            my.Cells[6, 6].Value = dtOuter.Rows[0]["白班异常情况处理"]; //txb白班异常情况处理.Text;
            my.Cells[10, 6].Value = dtOuter.Rows[0]["夜班异常情况处理"]; //txb夜班异常情况处理.Text;
            my.PageSetup.RightFooter = __生产指令编号 + "-" + "14" + "-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

            if (preview)
            {
                my.Select();
                oXL.Visible = true; //加上这一行  就相当于预览功能            
            }
            else
            {
                // 让这个Sheet为被选中状态
                //my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
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
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 岗位交接班记录 where 生产指令ID="+__生产指令ID,mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
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
        private void btn提交审核_Click(object sender, EventArgs e)
        {
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

            if ((Convert.ToString(Parameter.userflight) == "白班") && (dtOuter.Rows[0]["夜班接班员"].ToString() == ""))
            {
                dtOuter.Rows[0]["夜班接班员"] = __待审核;
                dtOuter.Rows[0]["白班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (dtOuter.Rows[0]["白班接班员"].ToString() == ""))
            {
                dtOuter.Rows[0]["白班接班员"] = __待审核;
                dtOuter.Rows[0]["夜班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            }



            
            //update log into table
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            
            readOuterData(__生产指令编号,__生产日期);
           
            removeOuterBind();
            outerBind();

            _formState = Parameter.FormState.待审核;
            setEnableReadOnly(true);
            btn提交审核.Enabled = false;
            btn保存.Enabled = false;
        }
        private void btn上一条记录_Click(object sender, EventArgs e)
        {
            DataTable dtOuter1;
            OleDbDataAdapter daOuter1;
            BindingSource bsOuter1;
            OleDbCommandBuilder cbOuter1;
            List<int> idList = new List<int>();
            daOuter1 = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + __生产指令编号 + "' ORDER BY ID ASC;", conOle);
            cbOuter1 = new OleDbCommandBuilder(daOuter);
            dtOuter1 = new DataTable(tablename1);
            bsOuter1 = new BindingSource();
            daOuter1.Fill(dtOuter1);
            for (int i = 0; i < dtOuter1.Rows.Count; i++)
            {
                idList.Add(Convert.ToInt32(dtOuter1.Rows[i]["ID"]));
            }
            int nowLocateion = idList.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            if (0 == nowLocateion)
            {
                MessageBox.Show("此消息为第一条");
                return;
            }
            try
            {
                HandOver previous = new HandOver(mainform, idList[nowLocateion - 1]);
                previous.ShowDialog();
            }
            catch
            { }
        }
        private void setEnableReadOnly(bool bl)
        {
            dtp生产日期.Enabled = false;
            if ("白班" == Parameter.userflight)
            {
                //flight night part disable
                夜班Enable(false);

                //current flight day and already handed over
                if (dtOuter.Rows[0]["夜班接班员"].ToString().Trim() != "")
                {
                    白班Enable(false);       
                    btn保存.Enabled = false;
                }
                //current flight day and haven't handed yet
                else
                {
                    白班Enable(true);
                    btn保存.Enabled=true;
                }

                //this part to control the buttons
                if ("" ==dtOuter.Rows[0]["白班接班员"].ToString().Trim())
                {
                    allButtonDisable();
                }
                else if (__待审核 ==dtOuter.Rows[0]["白班接班员"].ToString().Trim())
                {
                    onlyCheckEnable();
                }
                else
                {
                    checkDisable();
                }
            }
            else
            {
                白班Enable(false);
                {
                    if (dtOuter.Rows[0]["白班接班员"].ToString().Trim() != "")
                    {
                        夜班Enable(false);
                        //dataGridView1.Columns[5].ReadOnly = true;
                        btn保存.Enabled = false;
                    }
                    else
                    {
                        夜班Enable(true);
                        btn保存.Enabled = true;
                    }
                }
                if ("" ==dtOuter.Rows[0]["夜班接班员"].ToString().Trim())
                {
                    allButtonDisable();
                }
                else if (__待审核 ==dtOuter.Rows[0]["夜班接班员"].ToString().Trim())
                {
                    onlyCheckEnable();
                }
                else
                {
                    checkDisable();
                }
            }
        }
        private void 夜班Enable(bool night)
        {
            txb夜班异常情况处理.Enabled = night;
            txb夜班交班员.Enabled = night;
            txb白班接班员.Enabled = night;
            dtp夜班交接班时间.Enabled = night;
            //this is part of day, but logically works here
            dataGridView1.Columns[5].ReadOnly = (true==night) ?false : true ;
        }
        private void 白班Enable(bool day)
        {
            txb白班交班员.Enabled = day;
            txb白班异常情况处理.Enabled = day;
            txb夜班接班员.Enabled = day;
            dtp白班交接班时间.Enabled = day;
            //this is part of night, but logically works here
            dataGridView1.Columns[4].ReadOnly = (true == day) ? false : true;
        }
        private void allButtonDisable()
        {
            btn审核.Enabled=false;
            btn提交审核.Enabled=false;
            //btn保存.Enabled=false;
            btn查看日志.Enabled=true;
            btn打印.Enabled=true;
        }
        private void onlyCheckEnable()
        {
            //btn保存.Enabled = false;
            btn提交审核.Enabled = false;
            btn审核.Enabled=true;
        }
        private void checkDisable()
        {
            //btn保存.Enabled = true;
            btn提交审核.Enabled = false;
            btn审核.Enabled=false;
        }
     
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
        }

        //leave datagridview check the right things
        
        
    }
}
