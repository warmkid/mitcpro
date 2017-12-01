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
using System.Text.RegularExpressions;

namespace mySystem.Process.Stock
{
    public partial class 物资请验单 : BaseForm
    {
        List<String> ls操作员 = new List<string>();
        List<String> ls审核员 = new List<string>();

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        mySystem.Parameter.UserState _userState;

        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        mySystem.Parameter.FormState _formState;

        CheckForm ckform;

        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

//        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";

//        OleDbConnection conn;

        public 物资请验单(MainForm mainform, int id):base(mainform)
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConn);
            //conn.Open();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData(id);
            outerBind();

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();
        }

        // //getPeople()--> setUserState()--> getOtherData()-->
        // 读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->
        //addComputerEventHandler()--> setFormState()--> setEnableReadOnly() --> 
        //addOtherEvnetHandler()

        void getPeople()
        {
            // TODO
            ls审核员 = new List<string>();
            ls操作员 = new List<string>();
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='" + "物资验收记录" + "'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户权限设置有误，为避免出现错误，请尽快联系管理员完成设置！");
                this.Dispose();
            }

            string str操作员 = dt.Rows[0]["操作员"].ToString();
            string str审核员 = dt.Rows[0]["审核员"].ToString();
            String[] tmp = Regex.Split(str操作员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls操作员.Add(s);
                }
            }
            tmp = Regex.Split(str审核员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls审核员.Add(s);
                }
            }
        }

        void setUserState()
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

        void getOtherData()
        {

        }

        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 物资请验单 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("物资请验单");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            tb供应商代码.DataBindings.Clear();
            tb供应商名称.DataBindings.Clear();
            dtp请验时间.DataBindings.Clear();
            tb请验人.DataBindings.Clear();
            tb审核员.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            tb供应商代码.DataBindings.Add("Text", bsOuter.DataSource, "供应商代码");
            tb供应商名称.DataBindings.Add("Text", bsOuter.DataSource, "供应商名称");
            dtp请验时间.DataBindings.Add("Value", bsOuter.DataSource, "请验时间");
            tb请验人.DataBindings.Add("Text", bsOuter.DataSource, "请验人");
            tb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");

           
        }

        void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + id, mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            dtInner = new DataTable("物资请验单详细信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        void addComputerEventHandler()
        {

        }

        void setFormState()
        {
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核结果"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
            
        }

        void setEnableReadOnly()
        {
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].ReadOnly = false;
        }

        void addOtherEvnetHandler()
        {
            foreach (ToolStripItem tsi in contextMenuStrip1.Items)
            {
                tsi.Click += new EventHandler(tsi_Click);
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["物资请验单ID"].Visible = false;
        }

        void tsi_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da;
            DataTable dt;
            if (this.Name == sender.ToString())
            {
                return;
            }
            int id;
            if (this.Name == "物资验收记录")
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            }
            else
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]);
            }
            try
            {
                switch (sender.ToString())
                {
                    case "物资验收记录":
                        物资验收记录 form1 = new 物资验收记录(mainform,id);
                        form1.Show();
                        break;
                    case "物资请验单":
                        da = new SqlDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        物资请验单 form2 = new 物资请验单(mainform, Convert.ToInt32(dt.Rows[0]["ID"]));
                        form2.Show();
                        break;
                    case "检验记录":
                        da = new SqlDataAdapter("select * from 检验记录 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0) MessageBox.Show("没有关联的检验记录");
                        foreach (DataRow dr in dt.Rows)
                        {
                            (new 复验记录(mainform, Convert.ToInt32(dr["ID"]))).Show();                            //form3.Show();
                        }
                        break;
                    case "取样记录":
                        da = new SqlDataAdapter("select * from 取样记录 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        取样记录 form4 = new 取样记录(mainform, Convert.ToInt32(dt.Rows[0]["ID"]));
                        form4.Show();
                        break;
                }
            }
            catch
            {
                MessageBox.Show("关联失败，请检查是否有相应数据");
            }
            //MessageBox.Show(this.Name + "\n" + sender.ToString());
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
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
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
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            outerBind();


            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;



            da = new SqlDataAdapter("select * from 待审核 where 表名='物资请验单' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "物资请验单";
            dr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);
            da.Update(dt);


            dtOuter.Rows[0]["审核员"] = "__待审核";
            _formState = Parameter.FormState.待审核;
            btn提交审核.Enabled = false;
            daOuter.Update((DataTable)bsOuter.DataSource);
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();


            setControlFalse();

            // 入库  TODO

        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.Show();
        }
        public override void CheckResult()
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='物资请验单' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("物资请验单");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            //dtOuter.Rows[0]["审核意见"] = ckform.opinion;

            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
            base.CheckResult();
        }


        void setDataGridViewColumns()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理

                if (dc.ColumnName == "有无厂家检验报告")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("无");
                    cbc.Items.Add("齐全");
                    cbc.Items.Add("不齐全");
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
        }


    }
}
