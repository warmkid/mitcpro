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

namespace mySystem.Process.Extruction.C
{
    public partial class Feed : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜供料系统运行记录";
        DataTable dtFeed;
        OleDbDataAdapter daFeed;
        BindingSource bsFeed;
        OleDbCommandBuilder cbFeed;

        string tablename2 = "吹膜供料系统运行记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> list操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> list审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
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
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;
        public Feed(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            getPeople();
            setUserState();
            __生产指令编号 = Parameter.proInstruction;
            __生产日期 = Convert.ToDateTime(DateTime.Now.Date.ToString());
            __班次 = Parameter.userflight;
            txb生产指令编号.Text = __生产指令编号;
            dtp生产日期.Value =__生产日期;
            for (int i = 0; i < flight.Count; i++)
            {
                cmb班次.Items.Add(flight[i]);
            }
            cmb班次.SelectedItem = __班次;
            readFeedData(__生产指令编号, __生产日期, __班次);
            removeFeedBinding();
            feedBinding();
            if (0 == dtFeed.Rows.Count)
            {
                DataRow newrow = dtFeed.NewRow();
                newrow = writeFeedDefault(newrow);
                dtFeed.Rows.Add(newrow);
                daFeed.Update((DataTable)bsFeed.DataSource);
                readFeedData(__生产指令编号, __生产日期, __班次);
                removeFeedBinding();
                feedBinding();
            }
            searchId = Convert.ToInt32(dtFeed.Rows[0]["ID"]);
            readItemData(searchId);
            ItemBind();
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
            dtFeed = new DataTable(tablename1);
            daFeed = new OleDbDataAdapter("SELECT * FROM 吹膜供料系统运行记录 WHERE ID =" + Id, conOle);
            bsFeed = new BindingSource();
            cbFeed = new OleDbCommandBuilder(daFeed);
            daFeed.Fill(dtFeed);
            removeFeedBinding();
            feedBinding();
            __生产指令编号 = Convert.ToString(dtFeed.Rows[0]["生产指令编号"]);
            __生产日期=Convert.ToDateTime(dtFeed.Rows[0]["生产日期"]);
            __班次=Convert.ToString( dtFeed.Rows[0]["班次"]);
            searchId = Id;
            cmb班次.Text = __班次;
            
            readItemData(searchId);
            
            ItemBind();

            setFormState();
            setEnableReadOnly();
        }

        private DataRow writeFeedDefault(DataRow dr)
        {
            dr["生产指令编号"] = Parameter.proInstruction;
            dr["生产指令ID"] = Parameter.proInstruID;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dr["班次"] = Parameter.userflight;
            dr["审核人"] = "";
            return dr;
        }
        private DataRow writeItemDefault(DataRow dr)
        {
            dr["T吹膜供料系统运行记录ID"] = dtFeed.Rows[0]["ID"];
            dr["检查时间"] = Convert.ToDateTime(DateTime.Now.TimeOfDay.ToString());
            dr["电机工作是否正常"] = "正常";
            dr["气动阀工作是否正常"] = "正常";
            dr["供料运行是否正常"] = "正常";
            dr["有无警报显示"] = "无";
            dr["是否解除警报"] = "无";
            dr["检查人"]= Parameter.userName;
            return dr;
        }

        void readFeedData(string 生产指令编号, DateTime 生产日期, string 班次)
        {
            daFeed = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + 生产指令编号 + "' AND 生产日期=#" + 生产日期 + "# AND 班次='" + 班次 + "';", conOle);
            cbFeed = new OleDbCommandBuilder(daFeed);
            dtFeed = new DataTable(tablename1);
            bsFeed = new BindingSource();
            daFeed.Fill(dtFeed);
        }
        void readFeedData(int Id)
        {
            daFeed = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE ID="+Id+";", conOle);
            cbFeed = new OleDbCommandBuilder(daFeed);
            dtFeed = new DataTable(tablename1);
            bsFeed = new BindingSource();
            daFeed.Fill(dtFeed);
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
            btn打印.Enabled = true;
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
            if ("" == dtFeed.Rows[0]["审核人"].ToString().Trim())
            {
                //this means the record hasn't been saved
                formState = 0;
            }
            else if (__待审核 == dtFeed.Rows[0]["审核人"].ToString().Trim())
            {
                //this means this record should be checked
                formState = 1;
            }
            else if (Convert.ToBoolean(dtFeed.Rows[0]["审核是否通过"]))
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

        private void readItemData(int id)
        {
            daItem = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T吹膜供料系统运行记录ID=" + id, conOle);
            cbItem = new OleDbCommandBuilder(daItem);
            dtItem = new DataTable(tablename2);
            bsItem = new BindingSource();
            daItem.Fill(dtItem);
        }

        private void removeFeedBinding()
        {
            txb生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            cmb班次.DataBindings.Clear();
            txb审核人.DataBindings.Clear();
        }
        private void feedBinding()
        {
            bsFeed.DataSource = dtFeed;
            txb生产指令编号.DataBindings.Add("Text", bsFeed.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsFeed.DataSource, "生产日期");
            cmb班次.DataBindings.Add("SelectedItem", bsFeed.DataSource, "班次");
            txb审核人.DataBindings.Add("Text", bsFeed.DataSource, "审核人");
        }
        private void ItemBind()
        {
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
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
            
            foreach (DataColumn dc in dtItem.Columns)
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
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "';";
            return (int)comm.ExecuteScalar();           
        }
        
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                //to update the Feed record
                base.CheckResult();
                txb审核人.Text = check.userName.ToString();
                dtFeed.Rows[0]["审核人"] = check.userName.ToString();
                dtFeed.Rows[0]["审核意见"] = check.opinion.ToString();
                dtFeed.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtFeed.Rows[0]["日志"] = dtFeed.Rows[0]["日志"].ToString() + log;


                bsFeed.EndEdit();
                daFeed.Update((DataTable)bsFeed.DataSource);

                readFeedData(searchId);

                removeFeedBinding();
                feedBinding();

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
                    newrow["对应ID"] = dtFeed.Rows[0]["ID"];
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
                txb审核人.Text = check.userName.ToString();
                dtFeed.Rows[0]["审核人"] = check.userName.ToString();
                dtFeed.Rows[0]["审核意见"] = check.opinion.ToString();
                dtFeed.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtFeed.Rows[0]["日志"] = dtFeed.Rows[0]["日志"].ToString() + log;


                bsFeed.EndEdit();
                daFeed.Update((DataTable)bsFeed.DataSource);

                readFeedData(searchId);

                removeFeedBinding();
                feedBinding();
                formState = 3;
                setEnableReadOnly();
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
                newrow["对应ID"] = dtFeed.Rows[0]["ID"];
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
            dtFeed.Rows[0]["日志"] = dtFeed.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtFeed.Rows[0]["审核人"] = __待审核;
            //update log into table
            bsFeed.EndEdit();
            daFeed.Update((DataTable)bsFeed.DataSource);
            try
            {
                readFeedData(searchId);
            }
            catch
            {
                readFeedData(__生产指令编号,__生产日期,__班次);
            }
            removeFeedBinding();
            feedBinding();

            formState = 1;
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            { MessageBox.Show(dtFeed.Rows[0]["日志"].ToString()); }
            catch
            { MessageBox.Show(" !"); }
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {           
            bsFeed.EndEdit();
            daFeed.Update((DataTable)bsFeed.DataSource);
            readFeedData(__生产指令编号, __生产日期, __班次);
            removeFeedBinding();
            feedBinding();

            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtFeed.Rows[0]["ID"]));
            ItemBind();
            btnSave.Enabled = false;
            if (0== userState)
            {
                btn提交审核.Enabled = true;
            }
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtItem.NewRow();
            dr = writeItemDefault(dr);
            dtItem.Rows.Add(dr);
            //setDataGridViewRowNums();
            btnSave.Enabled = true;
            btn审核.Enabled = false;
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\C\SOP-MFG-301-R07 吹膜供料系统运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 6].Value = "生产指令：" + txb生产指令编号.Text;
            my.Cells[5, 1].Value = dtp生产日期.Value.ToLongDateString() + "   " + cmb班次.Text;
            my.Cells[5, 9].Value = "";
            



            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                my.Cells[5 + i, 2].Value = Convert.ToDateTime(dtItem.Rows[i]["检查时间"]).ToShortTimeString();
                my.Cells[5 + i, 3].Value = dtItem.Rows[i]["电机工作是否正常"];
                my.Cells[5 + i, 4].Value = dtItem.Rows[i]["气动阀工作是否正常"];
                my.Cells[5 + i, 5].Value = dtItem.Rows[i]["供料运行是否正常"];
                my.Cells[5 + i, 6].Value = dtItem.Rows[i]["有无警报显示"];
                my.Cells[5 + i, 7].Value = dtItem.Rows[i]["是否解除警报"];
                my.Cells[5 + i, 8].Value = dtItem.Rows[i]["检查人"];
            }
            my.Cells[5, 9].Value = dtFeed.Rows[0]["审核人"];
            // 让这个Sheet为被选中状态
            
			if(preview)
			{
			my.Select();  
			 oXL.Visible=true; //加上这一行  就相当于预览功能
			}
			else
			{
            // 直接用默认打印机打印该Sheet
            // my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
			}
		}

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }
    }
	
}
