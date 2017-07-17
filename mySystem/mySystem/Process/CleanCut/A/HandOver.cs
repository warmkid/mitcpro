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
            setDataGridViewColumns();
            if (0 == dtItem.Rows.Count)
            {
                writeItemDefault(dtItem, (Convert.ToString(Parameter.userflight) == "白班") ? true : false);
            }

            if(     Convert.ToString    (Parameter.userflight)  == "白班")  
            {
                dataGridView1.Columns[5].ReadOnly=true;
            }
            else
            {
                dataGridView1.Columns[4].ReadOnly=true;
            }

            writeDayNightCheck();
            ItemBind();
            setDataGridViewRowNums();
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
            if ((Convert.ToString(Parameter.userflight) == "白班") && (txb白班交班人.Text == ""))
            {
                dtHandOver.Rows[0]["白班交班人"] = Parameter.userName;
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (txb夜班交班人.Text == ""))
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
            //dataGridView1.Columns[2].Visible = false;  //序号
            //dataGridView1.Columns[].Visible = false;
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }
        
        private void binding()
        {
            bsHandOver = new BindingSource();
            dtHandOver = new DataTable(tablename1);
            daHandOver = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "' and 生产日期 = #"+dtp生产日期.Value.ToShortDateString()+"#;", conOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
            cbHandOver = new OleDbCommandBuilder(daHandOver);
            daHandOver.Fill(dtHandOver);

            if (dtHandOver.Rows.Count < 1)
            {
                status = 1;
                DataRow newrow = dtHandOver.NewRow();
                dtHandOver.Rows.Add(newrow);
                binding2(0, false);
            }
            else
            {
                status = 2;
                binding2(getId(), true);
            }
            bsHandOver.DataSource = dtHandOver;
            // 
            txb生产指令编号.DataBindings.Add("Text", bsHandOver.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsHandOver.DataSource, "生产日期");
            txb白班产品代码批号数量.DataBindings.Add("Text", bsHandOver.DataSource, "白班产品代码批号数量");
            txb夜班产品代码批号数量.DataBindings.Add("Text", bsHandOver.DataSource, "夜班产品代码批号数量");
            txb白班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "白班异常情况处理");
            txb白班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班交班人");
            txb白班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班接班人");
            dtp白班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "白班交接班时间");
            txb夜班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "夜班异常情况处理");
            txb夜班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班交班人");
            txb夜班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班接班人");
            dtp夜班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "夜班交接班时间");
        }
        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "' and 生产日期 = #" + dtp生产日期.Value.ToShortDateString() + "#;";
            return (int)comm.ExecuteScalar();
        }
        private void binding2(int outerId, bool flag)
        {
            bsItem = new BindingSource();
            dtItem = new DataTable(tablename2);
            if (flag)
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where T吹膜岗位交接班记录ID = " + outerId, conOle);// +" where T吹膜岗位交接班记录ID in (select ID from " + tablename1 + " where 生产日期 = '" + dtp生产日期.Value.Date+"');",conOle);
            }
            else
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where false", conOle);
            }

            cbItem = new OleDbCommandBuilder(daItem);
            daItem.Fill(dtItem);
            if (dtItem.Rows.Count < 1)  //dtItem is NULL
            {
                //DataRow newrow = dtItem.NewRow();
                //dtItem.Rows.Add(newrow);
                
                if (flag)
                {
                    dtItem.Rows[0]["T吹膜岗位交接班记录ID"] = outerId;
                }
            }
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
            
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

        private void pullData()
        {
            dtHandOver.Rows[0]["生产指令编号"] = txb生产指令编号.Text;
            dtHandOver.Rows[0]["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dtHandOver.Rows[0]["白班产品代码批号数量"] = txb白班产品代码批号数量.Text;
            dtHandOver.Rows[0]["夜班产品代码批号数量"] = txb夜班产品代码批号数量.Text;
            dtHandOver.Rows[0]["白班异常情况处理"] = txb白班异常情况处理.Text;
            dtHandOver.Rows[0]["白班交班人"] = txb白班交班人.Text;
            dtHandOver.Rows[0]["白班接班人"] = txb白班接班人.Text;
            dtHandOver.Rows[0]["白班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToShortTimeString());
            dtHandOver.Rows[0]["夜班异常情况处理"] = txb夜班异常情况处理.Text;
            dtHandOver.Rows[0]["夜班交班人"] = txb夜班交班人.Text;
            dtHandOver.Rows[0]["夜班接班人"] = txb夜班接班人.Text;
            dtHandOver.Rows[0]["夜班交接班时间"] = Convert.ToDateTime(dtp夜班交接班时间.Value.ToShortTimeString());
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            pullData();
            

            bsHandOver.EndEdit();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            if (1 == status)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = conOle;
                comm.CommandText = "select @@identity";
                outerId = (int)comm.ExecuteScalar();
                dtItem.Rows[0]["T吹膜岗位交接班记录ID"] = Convert.ToInt32(outerId);
            }
            for (int i = 1; i < dtItem.Rows.Count; i++)
            {
                dtItem.Rows[i]["T吹膜岗位交接班记录ID"] = dtItem.Rows[0]["T吹膜岗位交接班记录ID"];
            }
            bsItem.EndEdit();
            daItem.Update((DataTable)bsItem.DataSource);
            fresh1();
        }

        private void fresh1()
        {
            string txb = this.txb生产指令编号.Text;
            DateTime start = this.dtp生产日期.Value;
            mySystem.Process.Extruction.A.HandOver fresh = new mySystem.Process.Extruction.A.HandOver(mainform);
            fresh.StartPosition = FormStartPosition.Manual;
            fresh.Location = this.Location;
            fresh.txb生产指令编号.Text = txb;
            fresh.dtp生产日期.Value = start;
            fresh.Show();
            this.Close();
        }

        public static void IdMotivation(int intId)
        {
            string tablename1 = "吹膜岗位交接班记录";
            MainForm mainform = new MainForm();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            comm.CommandText = "select 生产指令编号,生产日期 from " + tablename1 + " where ID =" + intId + ";";
            //////
            mySystem.Process.Extruction.A.HandOver search = new mySystem.Process.Extruction.A.HandOver(mainform);
            OleDbDataReader reader = null;
            reader=comm.ExecuteReader();
            while (reader.Read())
            {
                search.txb生产指令编号.Text = reader[0].ToString();
                search.dtp生产日期.Value = Convert.ToDateTime( reader[1]);
            }
            search.InitializeComponent();
            search.init();
            //txb生产指令编号.Text = Parameter.proInstruction;
            //dtp生产日期.Value = Convert.ToDateTime("2017/7/12"); ;
            search.binding();
            search.Show();
            
        }
        public override void CheckResult()
        {
            base.CheckResult();
            string sqlstr = "SELECT 班次 FROM users WHERE 用户ID=" + check.userID.ToString() + ";";
            OleDbCommand CHECK_FLIGHT = new OleDbCommand(sqlstr, Parameter.connOleUser);
            string checkFlight = Convert.ToString(CHECK_FLIGHT.ExecuteScalar());
            if (Parameter.userflight == "白班" && dtHandOver.Rows[0]["夜班接班人"] == "")
            {
                if (checkFlight == "夜班")
                {
                    dtHandOver.Rows[0]["夜班接班人"] = check.userName;
                }
                else
                {
                    MessageBox.Show("审核人班次不匹配");
                }
            }
            if (Parameter.userflight == "夜班" && dtHandOver.Rows[0]["白班接班人"] == "")
            {
                if (checkFlight == "白班")
                {
                    dtHandOver.Rows[0]["白班接班人"] = check.userName;
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
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
    }
}
