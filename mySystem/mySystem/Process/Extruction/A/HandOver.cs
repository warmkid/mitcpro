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
//1.this system will automatically judge the flight of user and fill default values of confirm items.
//2.the take_in operator act as reviewer, and operator with the same flight can not review the record.
//3.when the confirm items have benn reset, there will be a reload warnning message, choose 'yes' to reload the items, and the strategy will fill blank to the unmatched item;
//
//there are also some difficulties that haven't been solved
//when visiable the "ID" column, can not get it's columnIndex, see the end of function setDataGridViewColumns()

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
        List<string> flight = new List<string>(new string[] { "Yes", "No" });
        private CheckForm check = null;

        int status;
        int outerId;
        
        public HandOver(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            init();            
            txb生产指令编号.Text = Parameter.proInstruction;
            txb生产指令编号.Enabled = false;
            dtp生产日期.Value = DateTime.Now.Date;
            dtp生产日期.Enabled = false;
            settingItem = getSettingItem();
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
        }

        public HandOver(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
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
            dr["生产指令ID"] = Parameter.proInstruID;
            dr["生产指令编号"] = txb生产指令编号.Text;
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
            readHandOverData(txb生产指令编号.Text, dtp生产日期.Value.Date);
            removeHandOverBinding();
            handOverBind();

            btn审核.Enabled = true;
           
        }

    

      
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();
                string sqlstr = "SELECT 班次 FROM users WHERE 用户ID=" + check.userID.ToString() + ";";
                OleDbCommand CHECK_FLIGHT = new OleDbCommand(sqlstr, Parameter.connOleUser);
                string checkFlight = Convert.ToString(CHECK_FLIGHT.ExecuteScalar());
                if (Parameter.userflight == "白班" && dtHandOver.Rows[0]["夜班接班人"].ToString() == "")
                {
                    if (checkFlight == "夜班")
                    {
                        dtHandOver.Rows[0]["夜班接班人"] = check.userName;
                        txb白班异常情况处理.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("审核人班次不匹配");
                    }
                }
                if (Parameter.userflight == "夜班" && dtHandOver.Rows[0]["白班接班人"].ToString() == "")
                {
                    if (checkFlight == "白班")
                    {
                        dtHandOver.Rows[0]["白班接班人"] = check.userName;
                        txb白班异常情况处理.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("审核人班次不匹配");
                    }
                }
                bsHandOver.EndEdit();
                daHandOver.Update((DataTable)bsHandOver.DataSource);
                readHandOverData(txb生产指令编号.Text, dtp生产日期.Value.Date);
                removeHandOverBinding();
                handOverBind();
                finishReview();
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        
         private void btn打印_Click(object sender, EventArgs e)
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
}
