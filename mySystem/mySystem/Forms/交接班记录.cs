using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace mySystem.Forms
{
    public partial class 交接班记录 : BaseForm
    {

        Controllers.交接班Controller controller;
        Controllers.交接班设置Controller settingController;
        int _id;
        String _OuterTblName;
        String _InnerTblName;
        String _SettingTblName;

        DataTable _dtOuter;
        DataTable _dtInner;
        DataTable _dtSetting;

        String _instruction;
        int _instructID;

        BindingSource bs;

        // 常量
        String 待审核 = "__待审核";
        List<string> flight = new List<string>(new string[] { "是", "否" });
        // 列名
        String oc夜班接班人 = "夜班接班人";
        String oc白班交接时间 = "白班交接时间";

        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        Parameter.UserState _userState;

        bool sameDate(DateTime f, DateTime s)
        {
            if (f.Year == s.Year && f.Month == s.Month && f.Date == s.Date)
            {
                return true;
            }
            return false;
        }

        public 交接班记录(int instructid,string instruction,string gongxu)
        {
            _instructID = instructid;
            _instruction = instruction;
            setTblName(gongxu);
            setController();
            readSetting();

            DataTable dt = getLastestRecord();
            if (
                dt.Rows.Count < 0 || // 第一个班次
               (dt.Rows[0][oc夜班接班人].ToString() != "" && 
               dt.Rows[0][oc夜班接班人].ToString() != 待审核 &&// 上一个夜班已经完成交接
               !sameDate(Convert.ToDateTime( dt.Rows[0][oc白班交接时间].ToString()),DateTime.Now)   )
                // 不是今天交的班
                )
            {
                createNewRecord(_instructID, _instruction, DateTime.Now);
                dt = getLastestRecord();
            }

            _id = Convert.ToInt32(dt.Rows[0]["ID"]);
            showByID(_id);
        }
        public 交接班记录(int id, String gongxu)
        {
            // init sth
            _id = id;
            setTblName(gongxu);
            setController();
            readSetting();
            showByID(_id);
        }

        void showByID(int _id)
        {
            InitializeComponent();
            controller.readById(_id, out _dtOuter, out _dtInner);

            // show inner
            bs = new BindingSource();
            bs.DataSource = _dtInner;
            dataGridView1.DataSource = bs.DataSource;

            // show outer
            lbl生产指令编号.Text = readStringFromDataTable(_dtOuter.Rows[0]["生产指令编号"]);
            txb白班异常情况处理.Text =readStringFromDataTable (_dtOuter.Rows[0]["白班异常情况处理"]);
            txb夜班异常情况处理.Text =readStringFromDataTable (_dtOuter.Rows[0]["夜班异常情况处理"]);
            txb白班交班员.Text = readStringFromDataTable(_dtOuter.Rows[0]["白班交班员"]);
            txb白班接班员.Text = readStringFromDataTable(_dtOuter.Rows[0]["白班接班员"]);
            txb夜班交班员.Text = readStringFromDataTable(_dtOuter.Rows[0]["夜班交班员"]);
            txb夜班接班员.Text = readStringFromDataTable(_dtOuter.Rows[0]["夜班接班员"]);

            dtp生产日期.Value = Convert.ToDateTime( _dtOuter.Rows[0]["生产日期"]);
            dtp白班交接班时间.Value = Convert.ToDateTime(_dtOuter.Rows[0]["白班交接班时间"]);
            dtp夜班交接班时间.Value = Convert.ToDateTime(_dtOuter.Rows[0]["夜班交接班时间"]);

            setDataGridViewColumns();

            getPeople();
            setUserState();
            填写交班员();

            setEnableReadOnly(true);
        }

        private void setEnableReadOnly(bool bl)
        {
            dtp生产日期.Enabled = false;
            if ("白班" == Parameter.userflight)
            {
                //flight night part disable
                夜班Enable(false);

                //current flight day and already handed over
                if (txb夜班接班员.Text.Trim() != "")
                {
                    白班Enable(false);
                    btn保存.Enabled = false;
                }
                //current flight day and haven't handed yet
                else
                {
                    白班Enable(true);
                    btn保存.Enabled = true;
                }

                //this part to control the buttons
                if ("" == _dtOuter.Rows[0]["白班接班员"].ToString().Trim())
                {
                    allButtonDisable();
                }
                else if ("__待审核" == txb白班接班员.Text.Trim())
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
                    if (txb白班接班员.Text.Trim() != "")
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
                if ("" == txb夜班接班员.Text.Trim())
                {
                    allButtonDisable();
                }
                else if ("__待审核" == txb夜班接班员.Text.Trim())
                {
                    onlyCheckEnable();
                }
                else
                {
                    checkDisable();
                }
            }
        }
        private void onlyCheckEnable()
        {
            //btn保存.Enabled = false;
            btn提交审核.Enabled = false;
            btn审核.Enabled = true;
        }
        private void checkDisable()
        {
            //btn保存.Enabled = true;
            btn提交审核.Enabled = false;
            btn审核.Enabled = false;
        }
        private void allButtonDisable()
        {
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            //btn保存.Enabled=false;
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
        }
        private void 夜班Enable(bool night)
        {
            txb夜班异常情况处理.Enabled = night;
            txb夜班交班员.Enabled = night;
            txb白班接班员.Enabled = night;
            dtp夜班交接班时间.Enabled = night;
            //this is part of day, but logically works here
            dataGridView1.Columns[5].ReadOnly = (true == night) ? false : true;
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
        private void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = mySystem.Parameter.userName + "(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName + "(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName + "(审核员)";
            label角色.Text += " " + Parameter.userflight;
        }


        private void getPeople()
        {
            
                string tabName = "用户权限";
                DataTable dtUser = new DataTable(tabName);
                SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + _OuterTblName + "';", mySystem.Parameter.conn);
                BindingSource bsUser = new BindingSource();
                SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
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

        private void 填写交班员()
        {
            if ((Convert.ToString(Parameter.userflight) == "白班") && (_dtOuter.Rows[0]["白班交班员"].ToString() == ""))
            {
                _dtOuter.Rows[0]["白班交班员"] = Parameter.userName;
            }
            if ((Convert.ToString(Parameter.userflight) == "夜班") && (_dtOuter.Rows[0]["夜班交班员"].ToString() == ""))
            {
                _dtOuter.Rows[0]["夜班交班员"] = Parameter.userName;
            }
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in _dtInner.Columns)
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
            dataGridView1.Columns[2].ReadOnly = true;  //序号
            dataGridView1.Columns[3].ReadOnly = true;
            // setting width in confirm items
            dataGridView1.Columns[3].Width = 300;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
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


        String readStringFromDataTable(Object o){
            if (o == DBNull.Value)
            {
                return "";
            }
            else
            {
                return o.ToString();
            }
        }

        

        void setTblName(String gongxu)
        {
            switch (gongxu)
            {
                case "吹膜":
                    _OuterTblName = "吹膜岗位交接班记录";
                    _InnerTblName = "吹膜岗位交接班项目记录";
                    _SettingTblName = "设置岗位交接班项目";
                    break;
            }
        }

        void setController()
        {
            controller = new Controllers.交接班Controller(_OuterTblName, _InnerTblName,mySystem.Parameter.conn);
            settingController = new Controllers.交接班设置Controller("设置岗位交接班项目",mySystem.Parameter.conn);

        }

        DataTable getLastestRecord()
        {
            return controller.getLastest(_instructID);
        }

        bool createNewRecord(int iid, string ins, DateTime time)
        {
            if (time == null) time = DateTime.Now;
            return controller.createNewRecord(iid, ins, time,_dtSetting);
         
        }

        void readSetting()
        {
            _dtSetting = settingController.read();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            controller.save(_id,
                new String[]{dtp生产日期.Value.ToString("yyyy-MM-dd"),
                txb白班异常情况处理.Text,txb白班交班员.Text,txb白班接班员.Text,dtp白班交接班时间.Value.ToString("yyyy-MM-dd"),
                txb夜班异常情况处理.Text,txb夜班交班员.Text, txb夜班接班员.Text,dtp夜班交接班时间.Value.ToString("yyyy-MM-dd")},
                (DataTable)dataGridView1.DataSource);
            
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();

                logForm.setLog(_dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {

        }

    }
}
