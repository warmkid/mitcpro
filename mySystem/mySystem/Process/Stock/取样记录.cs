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
    public partial class 取样记录 : BaseForm
    {
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;

        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;

        int _id;
        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        List<String> ls操作员, ls审核员;

        CheckForm ckform;

        bool isFirstBind = true;

        public 取样记录(MainForm mainform, int id):base(mainform)
        {
            _id = id;
            InitializeComponent();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            getPeople();
            setUseState();
            readOuterData(id);
            outerBind();
            readInnerData( Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setFormState();
            setEnableReadOnly();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            addOtherEventHandler();
        }


       
      

        private void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 取样记录 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("取样记录");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {
                
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2), false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as ComboBox).DataBindings["Text"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as ComboBox).DataBindings["Text"].DataSourceUpdateMode;
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as DateTimePicker).DataBindings["Value"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as DateTimePicker).DataBindings["Value"].DataSourceUpdateMode;
                }
            }
        }


        private void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 取样记录详细信息 where 取样记录ID=" + id, mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            dtInner = new DataTable("取样记录详细信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        private DataRow writeInnerDefault(DataRow dr)
        {
            dr["取样记录ID"] = dtOuter.Rows[0]["ID"];
            dr["取样量"] = 0;
            dr["日期"] = DateTime.Now;
            dr["取样人"] = mySystem.Parameter.userName;
            dr["取样量"] = 0;
            return dr;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            //DataRow dr = dtInner.NewRow();
            //dr = writeInnerDefault(dr);
            //dtInner.Rows.Add(dr);
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            //int idx = dataGridView1.SelectedCells[0].RowIndex;
            //dtInner.Rows[idx].Delete();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();

            btn提交审核.Enabled = true;
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

        void setUseState()
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


        void getPeople()
        {
            ls审核员 = new List<string>();
            ls操作员 = new List<string>();
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='" + "取样记录" + "'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户权限设置有误，为避免出现错误，请尽快联系管理员完成设置！");
                this.Dispose();
                return;
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

            if (Parameter.FormState.无数据 == _formState)
            {
                setControlFalse();
                return;
            }
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


        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
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

        /// <summary>
        /// 默认控件不可用状态
        /// </summary>
        void setControlFalse()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "btn查询插入") continue;
                if (c.Name == "tb生产指令编码") continue;
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
            btn取样证.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;



            da = new SqlDataAdapter("select * from 待审核 where 表名='取样记录' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "取样记录";
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

            da = new SqlDataAdapter("select * from 待审核 where 表名='取样记录' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;
            
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;

            base.CheckResult();
        }

        private void btn取样证_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0) return;
            int rIdx=dataGridView1.SelectedCells[0].RowIndex;
            int selectedId = Convert.ToInt32(dtInner.Rows[rIdx]["ID"]);
            取样证 form = new 取样证(selectedId);
            form.Show();
        }

        void addOtherEventHandler()
        {
            foreach (ToolStripItem tsi in contextMenuStrip1.Items)
            {
                tsi.Click += new EventHandler(tsi_Click);
                this.ContextMenuStrip = contextMenuStrip1;
            }
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["取样记录ID"].Visible = false;

            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
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
                        物资验收记录 form1 = new 物资验收记录(mainform, id);
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
        }

        private void 取样记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView1);
        }
    }
}
