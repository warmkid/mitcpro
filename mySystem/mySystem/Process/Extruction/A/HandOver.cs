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

//main functions are listed below
//wneh the form showed, the information bust bind again so that can display

namespace mySystem.Process.Extruction.A
{
    public partial class HandOver : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜岗位交接班记录";
        DataTable dtHandOver;
        OleDbDataAdapter daHandOver;
        BindingSource bsHandOver;
        OleDbCommandBuilder cbHandOver;

        string tablename2 = "吹膜岗位交接班项目记录";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        string tablename3 = "设置岗位交接班项目";
        DataTable dtSettingHandOver;
        OleDbDataAdapter daSettingHandOver;
        BindingSource bsSettingHandOver;
        OleDbCommandBuilder cbSettingHandOver;

        List<string> settingItem;
        List<string> flight = new List<string>(new string[] { "是","否" });
        List<string> list操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> list审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        int __生产指令ID;
        string __生产指令编号;
        DateTime __dtp生产日期;
        private CheckForm check = null;
        string __待审核 = "__待审核";


        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;
        int searchId;
        int status;
        int outerId;
        
        public HandOver(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            getOtherData();
            searchRecord();

            readItemData(Convert.ToInt32(dtHandOver.Rows[0]["ID"]));
            setDataGridViewColumns();



            if (0 == dtItem.Rows.Count)
            {
                writeItemDefault(dtItem, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
            }
            setDataGridViewRowNums();
            ItemBind();
            askToReload();
            writeName();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            readHandOverData(__生产指令编号, __dtp生产日期);
            removeHandOverBinding();
            handOverBind();
            setEnableReadOnly(true);


            /*
            init();            
            txb生产指令编号.Text = Parameter.proInstruction;
            txb生产指令编号.Enabled = false;
            dtp生产日期.Value = DateTime.Now.Date;
            dtp生产日期.Enabled = false;
            settingItem = getSettingItem();
             * 
             * 
            readHandOverData(txb生产指令编号.Text, dtp生产日期.Value.Date);
            removeHandOverBinding();
            handOverBind();
            if (0 == dtHandOver.Rows.Count)
            {
                DataRow newrow=dtHandOver.NewRow();
                newrow = writeHandOverDefault(newrow, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
                dtHandOver.Rows.Add(newrow);
                daHandOver.Update((DataTable)bsHandOver.DataSource);
                readHandOverData(txb生产指令编号.Text, dtp生产日期.Value.Date);
                removeHandOverBinding();
                handOverBind();
            }

            

            writeName();
            readItemData(Convert.ToInt32(dtHandOver.Rows[0]["ID"]));
            askToReload();
            
            setDataGridViewColumns();
            if (0 == dtItem.Rows.Count)
            {
                writeItemDefault(dtItem, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
            }
            finishReview();
            
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[3].Width = 300;
            writeDayNightCheck();
            ItemBind();
            setDataGridViewRowNums();
            btn审核.Enabled = false;
             */
        }

        public HandOver(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            searchId=Id;
            
            getPeople();
            setUserState();
            settingItem= getSettingItem();
            readHandOverData(searchId);
            __生产指令编号 = Convert.ToString(dtHandOver.Rows[0]["生产指令编号"]);
            __dtp生产日期 = Convert.ToDateTime(dtHandOver.Rows[0]["生产日期"]);
            removeHandOverBinding();
            handOverBind();
           
            readItemData(Convert.ToInt32(dtHandOver.Rows[0]["ID"]));
            setDataGridViewColumns();



            if (0 == dtItem.Rows.Count)
            {
                writeItemDefault(dtItem, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
            }
            setDataGridViewRowNums();
            ItemBind();
            askToReload();
            writeName();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            readHandOverData(__生产指令编号, __dtp生产日期);
            removeHandOverBinding();
            handOverBind();
            setEnableReadOnly(true);




            /*
            dtHandOver = new DataTable(tablename1);
            daHandOver = new OleDbDataAdapter("SELECT * FROM 吹膜岗位交接班记录 WHERE ID =" + Id, conOle);
            bsHandOver = new BindingSource();
            cbHandOver = new OleDbCommandBuilder(daHandOver);
            daHandOver.Fill(dtHandOver);
            removeHandOverBinding();
            handOverBind();

            readItemData(Convert.ToInt32(dtHandOver.Rows[0]["ID"]));           
            ItemBind();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            this.btn保存.Visible = false;
           
            this.btn审核.Visible = false;
            
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            */
            

        }

        /// <summary>
        /// this function read database and try to find a record, if not, insert a new record
        /// </summary>
        private void searchRecord()
        {
            readHandOverData(__生产指令编号,__dtp生产日期);
            removeHandOverBinding();
            handOverBind();
            if (0 == dtHandOver.Rows.Count)
            {
                DataRow newrow = dtHandOver.NewRow();
                newrow = writeHandOverDefault(newrow, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
                dtHandOver.Rows.Add(newrow);
                daHandOver.Update((DataTable)bsHandOver.DataSource);
                readHandOverData(__生产指令编号, __dtp生产日期);
                removeHandOverBinding();
                handOverBind();
            }
        }


        /// <summary>
        /// get settings anf fetch checking items
        /// </summary>
        private void getOtherData()
        {
            __生产指令ID = Parameter.proInstruID;
            __生产指令编号 = Parameter.proInstruction;
            __dtp生产日期 = DateTime.Now.Date;
            settingItem = getSettingItem();
        }

        private void finishReview()
        {
            if(     Convert.ToString    (Parameter.userflight)  == "白班")  
            {
                dataGridView1.Columns[5].ReadOnly=true;
            }
            else
            {
                dataGridView1.Columns[4].ReadOnly=true;
            }

            if (Convert.ToString(dtHandOver.Rows[0]["夜班接班人"]).Trim() != "")     //
            {
                dataGridView1.Columns[4].ReadOnly = true;
                txb白班异常情况处理.Enabled = false;
            }
            if (Convert.ToString(dtHandOver.Rows[0]["白班接班人"]).Trim() != "")
            {
                dataGridView1.Columns[5].ReadOnly = true;
                txb夜班异常情况处理.Enabled = false;
            }
        }
    
        private void askToReload()
        {
            if (dtItem.Rows.Count > 0)  //when there are some items 
            {
                if (!matchDt_List())
                {
                    if (DialogResult.OK == MessageBox.Show("确认项目与数据库不匹配,重新载入确认项目", "", MessageBoxButtons.OKCancel))
                    {
                        //this part will override the history
                        if (settingItem.Count < dtItem.Rows.Count)  // reduce confirm items and the cut item get ""
                        {

                            for (int i = 0; i < settingItem.Count; i++)
                            {
                                dtItem.Rows[i]["确认项目"] = settingItem[i];
                            }
                            for (int i = dtItem.Rows.Count - 1; i >= settingItem.Count; i--)
                            {
                                dtItem.Rows.RemoveAt(i);
                            }
                        }
                        if (settingItem.Count > dtItem.Rows.Count)
                        {
                            for (int i = 0; i < dtItem.Rows.Count; i++)
                            {
                                dtItem.Rows[i]["确认项目"] = settingItem[i];
                            }
                            for (int i = dtItem.Rows.Count; i < settingItem.Count; i++)
                            {
                                DataRow newrow = dtItem.NewRow();
                                newrow["T吹膜岗位交接班记录ID"] = dtHandOver.Rows[0]["ID"];
                                newrow["序号"] = i + 1;
                                newrow["确认项目"] = settingItem[i];
                                dtItem.Rows.Add(newrow);
                            }
                        }
                    }
                }
            }
        }

        private bool matchDt_List()
        {
            bool rtn = true;
            if (dtItem.Rows.Count != settingItem.Count)
            {
                rtn = false;
            }
            else
            {
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    if (settingItem.IndexOf(Convert.ToString( dtItem.Rows[i]["确认项目"])) < 0)
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
            txb白班交班人.Enabled = false;
            txb白班接班人.Enabled = false;
            txb夜班交班人.Enabled = false;
            txb夜班接班人.Enabled = false;
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

        DataRow writeHandOverDefault(DataRow dr,bool day)
        {
            dr["生产指令ID"] = __生产指令ID;
            dr["生产指令编号"] = __生产指令编号;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.Date.ToString());
            if (day)
            {
                dr["白班异常情况处理"] = "";
                
                dr["白班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToString());
                dr["夜班交接班时间"] = Convert.ToDateTime(DateTime.MinValue.ToString());
            }
            else
            {
                dr["夜班异常情况处理"] = "";
                
                dr["夜班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToString());
                dr["白班交接班时间"] = Convert.ToDateTime(DateTime.MinValue.ToString());
            }
            return dr;
        }
        private void writeName()
        {
            if ((Convert.ToString(Parameter.userflight) == "白班") && (dtHandOver.Rows[0]["白班交班人"].ToString() == ""))
            {
                dtHandOver.Rows[0]["白班交班人"] = Parameter.userName;                
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (dtHandOver.Rows[0]["夜班交班人"].ToString() == ""))
            {
                dtHandOver.Rows[0]["夜班交班人"] = Parameter.userName;
            }
        }
        void writeItemDefault(DataTable dt, bool day)
        {
            for (int i = 0; i <settingItem.Count; i++)
            {
                DataRow dr = dtItem.NewRow();
                dr["T吹膜岗位交接班记录ID"] = dtHandOver.Rows[0]["ID"];
                dr["确认项目"] = settingItem[i];               
                dtItem.Rows.Add(dr);
            }
        }
        void writeDayNightCheck()
        {
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if ((Convert.ToString(Parameter.userflight) == "白班")&&(txb夜班接班人.Text==""))
                {
                    dtItem.Rows[i]["确认结果白班"] = "Yes";
                }
                if ((Convert.ToString(Parameter.userflight) == "夜班") && (txb白班接班人.Text == ""))
                {
                    dtItem.Rows[i]["确认结果夜班"] = "Yes";
                }
            }
        }
        void readHandOverData(string 生产指令编号, DateTime 生产日期)
        {
            daHandOver = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + 生产指令编号 + "' AND 生产日期=#" + 生产日期.ToString() + "#;", conOle);
            cbHandOver = new OleDbCommandBuilder(daHandOver);
            dtHandOver = new DataTable(tablename1);
            bsHandOver = new BindingSource();
            daHandOver.Fill(dtHandOver);
        }
        void readHandOverData(int searchId)
        {
            daHandOver = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE ID=" +searchId+ ";", conOle);
            cbHandOver = new OleDbCommandBuilder(daHandOver);
            dtHandOver = new DataTable(tablename1);
            bsHandOver = new BindingSource();
            daHandOver.Fill(dtHandOver);
        }
        private void readItemData(int id)
        {
            daItem = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T吹膜岗位交接班记录ID=" + id, conOle);
            cbItem = new OleDbCommandBuilder(daItem);
            dtItem = new DataTable(tablename2);
            bsItem = new BindingSource();
            daItem.Fill(dtItem);
        }

        private void removeHandOverBinding()
        {
            txb生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            txb白班异常情况处理.DataBindings.Clear();
            txb白班交班人.DataBindings.Clear();
            txb白班接班人.DataBindings.Clear();
            dtp白班交接班时间.DataBindings.Clear();
            txb夜班异常情况处理.DataBindings.Clear();
            txb夜班交班人.DataBindings.Clear();
            txb夜班接班人.DataBindings.Clear();
            dtp夜班交接班时间.DataBindings.Clear();
        }

        private void handOverBind()
        {
            bsHandOver.DataSource = dtHandOver;
            txb生产指令编号.DataBindings.Add("Text", bsHandOver.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsHandOver.DataSource, "生产日期");
            txb白班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "白班异常情况处理");
            txb白班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班交班人");
            txb白班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班接班人");
            dtp白班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "白班交接班时间");
            txb夜班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "夜班异常情况处理");
            txb夜班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班交班人");
            txb夜班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班接班人");
            dtp夜班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "夜班交接班时间");


            

        }

        private void ItemBind()
        {
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtItem.Columns)
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
            dataGridView1.Columns[3].ReadOnly = true; ;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4||e.ColumnIndex==5)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Yes")
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "No")
                {
                    //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                }
            }
           
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                dtItem.Rows[i]["序号"] = i + 1;
            }
        }
        
     
        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "' and 生产日期 = #" + dtp生产日期.Value.ToShortDateString() + "#;";
            return (int)comm.ExecuteScalar();
        }
      
        private List<string> getSettingItem()
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

       

        private void btn保存_Click(object sender, EventArgs e)
        {
           
            daItem.Update((DataTable)bsItem.DataSource);
            ItemBind();
           
            

            bsHandOver.EndEdit();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            readHandOverData(__生产指令编号,__dtp生产日期);
            removeHandOverBinding();
            handOverBind();

            btn提交审核.Enabled = true;
           
        }

    

      
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();

                if (Parameter.userflight == "夜班" && dtHandOver.Rows[0]["夜班接班人"].ToString() == __待审核)
                {

                    dtHandOver.Rows[0]["夜班接班人"] = check.userName;
                    txb白班异常情况处理.Enabled = false;

                }
                if (Parameter.userflight == "白班" && dtHandOver.Rows[0]["白班接班人"].ToString() == __待审核)
                {

                    dtHandOver.Rows[0]["白班接班人"] = check.userName;
                    txb白班异常情况处理.Enabled = false;

                }

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtHandOver.Rows[0]["日志"] = dtHandOver.Rows[0]["日志"].ToString() + log;

                bsHandOver.EndEdit();
                daHandOver.Update((DataTable)bsHandOver.DataSource);
                readHandOverData(__生产指令编号, __dtp生产日期);
                removeHandOverBinding();
                handOverBind();

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
                    newrow["对应ID"] = dtHandOver.Rows[0]["ID"];
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\A\SOP-MFG-301-R13 吹膜岗位交接班记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 1].Value = "生产指令编号：" + txb生产指令编号.Text;
            my.Cells[3, 5].Value = "生产日期：" + dtp生产日期.Value.ToLongDateString();


            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                my.Cells[i + 6, 1].Value = dtItem.Rows[i]["序号"];
                my.Cells[i + 6, 2].Value = dtItem.Rows[i]["确认项目"];
                my.Cells[i + 6, 3].Value = dtItem.Rows[i]["确认结果白班"];
                my.Cells[i + 6, 4].Value = dtItem.Rows[i]["确认结果夜班"];
         
            }
            my.Cells[12,6].Value="交班人："+txb白班交班人.Text+"   接班人："+txb夜班接班人.Text+"   时间："+dtp白班交接班时间.Value.ToShortTimeString();
           my.Cells[21,6].Value="交班人："+txb夜班交班人.Text+"   接班人："+txb白班接班人.Text+"   时间："+dtp夜班交接班时间.Value.ToShortTimeString();
           my.Cells[5, 6].Value = txb白班异常情况处理.Text;
           my.Cells[15, 6].Value = txb夜班异常情况处理.Text;

			if(preview)
			{
				my.Select();   
				oXL.Visible=true; //加上这一行  就相当于预览功能            
			}
			else
			{
            // 让这个Sheet为被选中状态
            my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
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
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            { MessageBox.Show(dtHandOver.Rows[0]["日志"].ToString()); }
            catch
            { MessageBox.Show(" !"); }
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
                newrow["对应ID"] = dtHandOver.Rows[0]["ID"];
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
            dtHandOver.Rows[0]["日志"] = dtHandOver.Rows[0]["日志"].ToString() + log;

            //fill reviwer information

            if ((Convert.ToString(Parameter.userflight) == "白班") && (dtHandOver.Rows[0]["夜班接班人"].ToString() == ""))
            {
                dtHandOver.Rows[0]["夜班接班人"] = __待审核;
                dtHandOver.Rows[0]["白班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (dtHandOver.Rows[0]["白班接班人"].ToString() == ""))
            {
                dtHandOver.Rows[0]["白班接班人"] = __待审核;
                dtHandOver.Rows[0]["夜班交接班时间"] = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            }



            
            //update log into table
            bsHandOver.EndEdit();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            
            readHandOverData(__生产指令编号,__dtp生产日期);
           
            removeHandOverBinding();
            handOverBind();

            formState = 1;
            setEnableReadOnly(true);
            btn提交审核.Enabled = false;
            btn保存.Enabled = false;
        }

        private void setEnableReadOnly(bool bl)
        {
            if ("白班" == Parameter.userflight)
            {
                setNightReadOnly(false);
                if (dtHandOver.Rows[0]["夜班接班人"].ToString().Trim() != "")
                {
                    setDayReadOnly(false);
                    //dataGridView1.Columns[4].ReadOnly = true;
                    btn保存.Enabled = false;
                }
                else
                {
                    setDayReadOnly(true);
                    btn保存.Enabled=true;
                }

                //this part to control the buttons
                if ("" ==dtHandOver.Rows[0]["白班接班人"].ToString().Trim())
                {
                    allButtonDisable();
                }
                else if (__待审核 ==dtHandOver.Rows[0]["白班接班人"].ToString().Trim())
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
                setDayReadOnly(false);
                {
                    if (dtHandOver.Rows[0]["白班接班人"].ToString().Trim() != "")
                    {
                        setNightReadOnly(false);
                        //dataGridView1.Columns[5].ReadOnly = true;
                        btn保存.Enabled = false;
                    }
                    else
                    {
                        setNightReadOnly(true);
                        btn保存.Enabled = true;
                    }
                }
                if ("" ==dtHandOver.Rows[0]["夜班接班人"].ToString().Trim())
                {
                    allButtonDisable();
                }
                else if (__待审核 ==dtHandOver.Rows[0]["夜班接班人"].ToString().Trim())
                {
                    onlyCheckEnable();
                }
                else
                {
                    checkDisable();
                }
            }

        }
        private void setNightReadOnly(bool night)
        {
            txb夜班异常情况处理.Enabled = night;
            txb夜班交班人.Enabled = night;
            txb白班接班人.Enabled = night;
            dtp夜班交接班时间.Enabled = night;
            //this is part of day, but logically works here
            dataGridView1.Columns[5].ReadOnly = (true==night) ?false : true ;
        }
        private void setDayReadOnly(bool day)
        {
            txb白班交班人.Enabled = day;
            txb白班异常情况处理.Enabled = day;
            txb夜班接班人.Enabled = day;
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
        private void setEnableReadOnly()
        {
            switch (userState)
            {
                case 0: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (0 == formState || 3 == formState)
                    {
                        setControlTrue();
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
            if ("" == dtHandOver.Rows[0]["审核人"].ToString().Trim())
            {
                //this means the record hasn't been saved
                formState = 0;
            }
            else if (__待审核 == dtHandOver.Rows[0]["审核人"].ToString().Trim())
            {
                //this means this record should be checked
                formState = 1;
            }
            else if (Convert.ToBoolean(dtHandOver.Rows[0]["审核是否通过"]))
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
    }
}
