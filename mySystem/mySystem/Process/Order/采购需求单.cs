using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;

namespace mySystem.Process.Order
{
    public partial class 采购需求单 : BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
        Hashtable ht产成品BOM;
        DataTable dt组件存货档案;
        List<String> ls操作员, ls审核员;
        mySystem.Parameter.FormState _formState;
        mySystem.Parameter.UserState _userState;
        string _订单号;
        CheckForm ckform;
        List<String> ls供应商;

        public 采购需求单(MainForm mainform, string 订单号):base(mainform)
        {
            // id是销售订单ID
            // whatId 为真，表示销售订单
            _订单号 = 订单号;
            conn = new OleDbConnection(strConnect);
            conn.Open();
            InitializeComponent();
            getPeople();
            setUseState();
            
            getOtherData();
            fillBy订单号(订单号);
            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
        }

        private void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["采购需求单ID"].Visible = false;
        }

        private void getOtherData()
        {
            ht产成品BOM = new Hashtable();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置产成品存货档案", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ht产成品BOM.Add(dr["存货编码"].ToString(), dr["BOM列表"].ToString());
            }

            da = new OleDbDataAdapter("select * from 设置组件存货档案", conn);
            dt组件存货档案 = new DataTable("设置组件存货档案");
            da.Fill(dt组件存货档案);

            ls供应商 = new List<string>();
            da = new OleDbDataAdapter("select * from 设置供应商信息", conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls供应商.Add(dr["供应商名称"].ToString());
            }
        }

        private void fillBy订单号(string 订单号)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + 订单号 + "'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("读取销售订单数据失败");
                return;
                    
            }
            int 销售订单ID = Convert.ToInt32(dt.Rows[0]["ID"]);
            // 外包表
            readOuterData(订单号);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {

                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, 订单号);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(订单号);
                outerBind();
            }


            // 内表
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();
            if (dtInner.Rows.Count == 0)
            {
                Hashtable ht产成品, ht组件;
                ht产成品 = new Hashtable();
                ht组件 = new Hashtable();
                da = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + 销售订单ID, conn);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    string 存货编码 = dr["存货编码"].ToString();
                    double 订单数量 = Convert.ToDouble(dr["数量"]);
                    ht产成品.Add(存货编码,订单数量);
                    string BOM_IDS = ht产成品BOM[存货编码].ToString();
                    foreach (string sid in BOM_IDS.Split(','))
                    {
                        int id = Convert.ToInt32(sid);
                        if (ht组件.ContainsKey(id))
                        {
                            ht组件[id] = Convert.ToInt32(ht组件[id]) + 订单数量;
                        }
                        else
                        {
                            ht组件[id] = 订单数量;
                        }
                    }
                }
                foreach (int id in ht组件.Keys.OfType<int>().ToArray<int>())
                {
                    DataRow dr = dt组件存货档案.Select("ID=" + id)[0];
                    DataRow ndr = dtInner.NewRow();
                    ndr["采购需求单ID"] = dtOuter.Rows[0]["ID"];
                    ndr["存货代码"] = dr["存货编码"];
                    ndr["存货名称"] = dr["存货名称"];
                    ndr["规格型号"] = dr["规格型号"];
                    ndr["件数"] = 1;
                    ndr["数量"] = dr["数量每件"];
                    ndr["单位"] = dr["主计量单位名称"];
                    ndr["订单数量"] = ht组件[id];
                    ndr["采购数量"] = ht组件[id];
                    ndr["采购件数"] = Math.Round(Convert.ToDouble(ht组件[id]) / Convert.ToDouble(dr["数量每件"]), 2);
                    dtInner.Rows.Add(ndr);
                }
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }

        }

        void readOuterData(string yongtu)
        {
            daOuter = new OleDbDataAdapter("select * from 采购需求单 where 用途='" + yongtu + "'", conn);
            dtOuter = new DataTable("采购需求单");
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }


        void outerBind()
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

        DataRow writeOuterDefault(DataRow dr, string 订单号)
        {

            dr["用途"] = 订单号;
            dr["期望到货时间"] = DateTime.Now;
            dr["采购申请单号"] = "没有规则--" + 订单号;
            dr["申请日期"] = DateTime.Now;
            dr["申请人"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;
            dr["状态"] = "编辑中";
            return dr;
        }

        void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, conn);
            dtInner = new DataTable("采购需求单详细信息");
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
        }

        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 订单用户权限 where 步骤='采购需求单'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();
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

        void setFormState()
        {
            if (dtOuter == null || dtOuter.Rows.Count == 0)
            {
                _formState = Parameter.FormState.未保存;
                return;
            }
            string s = dtOuter.Rows[0]["审核人"].ToString();
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

        }

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

        void setControlFalse()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
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

            btn打印.Enabled = true;
            combox打印机选择.Enabled = true;
        }

        private void btn确认_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员)
            {
                btn提交审核.Enabled = true;
            }
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_订单号);
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购需求单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "采购需求单";
                dr["对应ID"] = (int)dtOuter.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bsOuter.DataSource = dtOuter;
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);


            dtOuter.Rows[0]["状态"] = "待审核";
            dtOuter.Rows[0]["审核人"] = "__待审核";
            dtOuter.Rows[0]["审核日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dtOuter.Rows[0]["申请人"].ToString())
            {
                MessageBox.Show("操作员和审核员不能是同一个人");
                return;
            }
            ckform = new mySystem.CheckForm(this);
            ckform.Show();  
        }

        public override void CheckResult()
        {
            //获得审核信息
            dtOuter.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核日期"] = ckform.time;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;

            if (ckform.ischeckOk)//审核通过
            {
                dtOuter.Rows[0]["状态"] = "审核完成";
            }
            else
            {
                dtOuter.Rows[0]["状态"] = "编辑中";//未审核，草稿
            }

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购需求单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            save();
            base.CheckResult();
        }

        void setDataGridViewColumn()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "推荐供应商")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (string gys in ls供应商)
                    {
                        cbc.Items.Add(gys);
                    }
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
