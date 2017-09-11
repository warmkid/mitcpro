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
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace mySystem.Process.Order
{
    public partial class 采购需求单 : BaseForm
    {
        bool isSaved = false;
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
        Hashtable ht产成品BOM;
        DataTable dt存货档案;
        List<String> ls操作员, ls审核员;
        mySystem.Parameter.FormState _formState;
        mySystem.Parameter.UserState _userState;
        string _订单号;
        CheckForm ckform;
        List<String> ls供应商;

        OleDbDataAdapter daCH;
        DataTable dtCH;

        public 采购需求单(MainForm mainform, string 订单号):base(mainform)
        {
            
            _订单号 = 订单号;
            conn = new OleDbConnection(strConnect);
            conn.Open();
            InitializeComponent();
            fillPrinter();
            getPeople();
            setUseState();
            
            getOtherData();
            fillBy订单号(订单号);
            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
            // 加入产成品列表
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + 订单号 + "'", conn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("读取销售订单数据失败");
                return;

            }
            int 销售订单ID = Convert.ToInt32(dt.Rows[0]["ID"]);
            da = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + 销售订单ID, conn);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["销售订单ID"].Visible = false;
            //
        }

        private void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
        }

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "推荐供应商")
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls供应商.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["采购需求单ID"].Visible = false;
            dataGridView1.Columns["批准状态"].Visible = false;
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

            da = new OleDbDataAdapter("select * from 设置存货档案", conn);
            dt存货档案 = new DataTable("设置存货档案");
            da.Fill(dt存货档案);

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
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("读取销售订单数据失败");
                return;
                    
            }
            

            int 销售订单ID = Convert.ToInt32(dt.Rows[0]["ID"]);
            DateTime dt拟交货日期 = Convert.ToDateTime(dt.Rows[0]["拟交货日期"]);
            // 修改销售订单的状态
            dt.Rows[0]["状态"] = "已生成采购需求单";
            da.Update(dt);

            // 外包表
            readOuterData(订单号);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {

                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, 订单号, dt拟交货日期);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(订单号);
                outerBind();
            }
            else
            {
                isSaved = true;
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
                // 获取所有存货档案信息
                daCH = new OleDbDataAdapter("select ID,存货代码,BOM列表 from 设置存货档案", conn) ;
                dtCH = new DataTable();
                daCH.Fill(dtCH);
                foreach (DataRow dr in dt.Rows)
                {
                    string 存货代码 = dr["存货代码"].ToString();
                    double 订单数量 = Convert.ToDouble(dr["数量"]);
                    if (!ht产成品.ContainsKey(存货代码))
                    {
                        ht产成品[存货代码] = 0;
                    }
                    ht产成品[存货代码] = Convert.ToDouble(ht产成品[存货代码]) + 订单数量;
                    // 通过函数获得者一个代码需要的组件hashtable
                    // 然后和总的hashtable 合并
                    /*
                    JArray BOM_IDS = JArray.Parse(ht产成品BOM[存货代码].ToString());
                    foreach (JToken sid in BOM_IDS)
                    {
                        int id = Convert.ToInt32(sid["ID"]);
                        double 数量每产品 = Convert.ToDouble(sid["数量"]);
                        if (ht组件.ContainsKey(id))
                        {
                            ht组件[id] = Convert.ToInt32(ht组件[id]) + 订单数量 * 数量每产品;
                        }
                        else
                        {
                            ht组件[id] = 订单数量 * 数量每产品;
                        }
                    }*/
                    //写个函数生成ht组件
                    Hashtable tmp = get组件信息(存货代码, 订单数量);
                    // 合并
                    foreach (int k in tmp.Keys.OfType<int>().ToArray<int>())
                    {
                        if (!ht组件.ContainsKey(k)) ht组件[k] = 0;
                        ht组件[k] = Convert.ToDouble(ht组件[k]) + Convert.ToDouble(tmp[k]);
                    }
                    //
                }

                bool first = true;
                foreach (int id in ht组件.Keys.OfType<int>().ToArray<int>())
                {
                    DataRow dr = dt存货档案.Select("ID=" + id)[0];
                    DataRow ndr = dtInner.NewRow();
                    ndr["采购需求单ID"] = dtOuter.Rows[0]["ID"];
                    if (first)
                    {
                        ndr["组件订单需求流水号"] = generate组件订单需求流水号();
                    }
                    else
                    {
                        string prefix = "PACR";
                        string yymmdd = DateTime.Now.ToString("yyMMdd");
                        string s = dtInner.Rows[dtInner.Rows.Count - 1]["组件订单需求流水号"].ToString();
                        int no = Convert.ToInt32(s.Substring(10, 3));
                        ndr["组件订单需求流水号"] = prefix + yymmdd + (no + 1).ToString("D3");
                    }
                    ndr["存货代码"] = dr["存货代码"];
                    ndr["存货名称"] = dr["存货名称"];
                    ndr["规格型号"] = dr["规格型号"];
                    ndr["件数"] = 1;
                    ndr["数量"] = dr["换算率"];
                    ndr["单位"] = dr["主计量单位名称"];
                    ndr["订单数量"] = ht组件[id];
                    ndr["采购数量"] = ht组件[id];
                    ndr["采购件数"] = Math.Round(Convert.ToDouble(ht组件[id]) / Convert.ToDouble(dr["换算率"]), 2);
                    ndr["推荐供应商"] = dr["推荐供应商"];
                    dtInner.Rows.Add(ndr);
                    first = false;
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

        DataRow writeOuterDefault(DataRow dr, string 订单号, DateTime dt)
        {

            dr["用途"] = 订单号;
            dr["期望到货时间"] = dt;
            dr["采购申请单号"] = generate需求单号();
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
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
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
            tb采购申请单号.ReadOnly = true;
            tb用途.ReadOnly = true;
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
            isSaved = true;
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

            if (!check供应商())
            {
                MessageBox.Show("供应商未填写完整！");
                return;
            }
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
            foreach (DataRow dr in dtInner.Rows)
            {
                dr["批准状态"] = "未批准";
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
                //if (dc.ColumnName == "推荐供应商")
                //{
                //    cbc = new DataGridViewComboBoxColumn();
                //    cbc.HeaderText = dc.ColumnName;
                //    cbc.Name = dc.ColumnName;
                //    cbc.ValueType = dc.DataType;
                //    cbc.DataPropertyName = dc.ColumnName;
                //    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                //    foreach (string gys in ls供应商)
                //    {
                //        cbc.Items.Add(gys);
                //    }
                //    dataGridView1.Columns.Add(cbc);
                //    continue;
                //}
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

        private void fillPrinter()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                combox打印机选择.Items.Add(sPrint);
            }
            combox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);



        string generate需求单号()
        {
            string prefix = "PAPN";
            string yymmdd = DateTime.Now.ToString("yyMMdd");
            string sql = "select * from 采购需求单 where 采购申请单号 like '{0}%' order by ID";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, prefix + yymmdd), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["采购申请单号"].ToString().Substring(10, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }


        bool check供应商()
        {
            foreach (DataRow dr in dtInner.Rows)
            {
                if (dr["推荐供应商"] == DBNull.Value)
                    return false;
            }
            return true;
        }


        Hashtable get组件信息(string 产品代码, double 数量)
        {
            Hashtable ret = new Hashtable();
            DataRow[] drs = dtCH.Select("存货代码='" + 产品代码 + "'");
            if (drs.Length == 0)
            {
                return ret;
            }
            
            foreach (DataRow dr in drs)
            {
                string bom = dr["BOM列表"].ToString();
                if (bom == "")
                {
                    ret[ Convert.ToInt32(drs[0]["ID"]) ] = 数量;
                    return ret;
                }
                JArray ja = JArray.Parse(bom);
                if (ja.Count == 0)
                {
                    ret[Convert.ToInt32(drs[0]["ID"])] = 数量;
                    return ret;
                }
                foreach (JToken jt in ja)
                {
                    int id = Convert.ToInt32( jt["ID"]);
                    double num = Convert.ToDouble(jt["数量"]);
                    DataRow[] drss = dtCH.Select("ID="+id);
                    if(drss.Length==0) continue;

                    Hashtable tmp = get组件信息(drss[0]["存货代码"].ToString(), 数量 * num);
                    foreach (int k in tmp.Keys.OfType<int>().ToArray<int>())
                    {
                        if (!ret.ContainsKey(k)) ret[k] = 0;
                        ret[k] = Convert.ToDouble(ret[k]) + Convert.ToDouble(tmp[k]);
                    }
                }
            }
            return ret;
        }

        string generate组件订单需求流水号()
        {
            string prefix = "PACR";
            string yymmdd = DateTime.Now.ToString("yyMMdd");
            string sql = "select * from 采购需求单详细信息 where 组件订单需求流水号 like '{0}%' order by ID";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, prefix + yymmdd), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["组件订单需求流水号"].ToString().Substring(10, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }

        private void 采购需求单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null && dtOuter.Rows.Count > 0)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update(dtOuter);

                    OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + _订单号 + "'", conn);
                    OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {


                    }
                    else
                    {
                        // 修改销售订单的状态
                        dt.Rows[0]["状态"] = "审核完成";
                        da.Update(dt);
                    }

                    foreach (DataRow dr in dtInner.Rows)
                    {
                        dr.Delete();
                    }
                    daInner.Update(dtInner);
                }
            }
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (combox打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(combox打印机选择.Text);
            print(false);
            GC.Collect();
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\caigou\表90. PALL AUSTAR采购需求单.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        //string log = "=====================================\n";
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                        bsOuter.EndEdit();
                        daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
            }
        }

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[4, 3].Value = dtOuter.Rows[0]["用途"].ToString();
            mysheet.Cells[4, 7].Value = Convert.ToDateTime(dtOuter.Rows[0]["期望到货时间"]).ToString("D");
            mysheet.Cells[4, 10].Value = dtOuter.Rows[0]["采购申请单号"].ToString();

            int ind = 0;
            //内表信息
            int rownum = dtInner.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 5 ? 5 : rownum); i++)
            {
                mysheet.Cells[8 + i, 1].Value = dtInner.Rows[i]["组件订单需求流水号"].ToString();
                mysheet.Cells[8 + i, 2].Value = dtInner.Rows[i]["存货代码"].ToString();
                mysheet.Cells[8 + i, 3].Value = dtInner.Rows[i]["存货名称"].ToString();
                mysheet.Cells[8 + i, 4].Value = dtInner.Rows[i]["规格型号"].ToString();
                mysheet.Cells[8 + i, 5].Value = dtInner.Rows[i]["件数"].ToString();
                mysheet.Cells[8 + i, 6].Value = dtInner.Rows[i]["数量"].ToString();
                mysheet.Cells[8 + i, 7].Value = dtInner.Rows[i]["单位"].ToString();
                mysheet.Cells[8 + i, 8].Value = dtInner.Rows[i]["订单数量"].ToString();
                mysheet.Cells[8 + i, 9].Value = dtInner.Rows[i]["采购件数"].ToString();
                mysheet.Cells[8 + i, 10].Value = dtInner.Rows[i]["采购数量"].ToString();
                mysheet.Cells[8 + i, 11].Value = dtInner.Rows[i]["推荐供应商"].ToString();
            }
            //需要插入的部分
            if (rownum > 5)
            {
                for (int i = 5; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[8 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[8 + i, 1].Value = dtInner.Rows[i]["组件订单需求流水号"].ToString();
                    mysheet.Cells[8 + i, 2].Value = dtInner.Rows[i]["存货代码"].ToString();
                    mysheet.Cells[8 + i, 3].Value = dtInner.Rows[i]["存货名称"].ToString();
                    mysheet.Cells[8 + i, 4].Value = dtInner.Rows[i]["规格型号"].ToString();
                    mysheet.Cells[8 + i, 5].Value = dtInner.Rows[i]["件数"].ToString();
                    mysheet.Cells[8 + i, 6].Value = dtInner.Rows[i]["数量"].ToString();
                    mysheet.Cells[8 + i, 7].Value = dtInner.Rows[i]["单位"].ToString();
                    mysheet.Cells[8 + i, 8].Value = dtInner.Rows[i]["订单数量"].ToString();
                    mysheet.Cells[8 + i, 9].Value = dtInner.Rows[i]["采购件数"].ToString();
                    mysheet.Cells[8 + i, 10].Value = dtInner.Rows[i]["采购数量"].ToString();
                    mysheet.Cells[8 + i, 11].Value = dtInner.Rows[i]["推荐供应商"].ToString();
                }
                ind = rownum - 5;
            }

            mysheet.Cells[15 + ind, 3].Value = Convert.ToDateTime(dtOuter.Rows[0]["申请日期"]).ToString("D");
            mysheet.Cells[15 + ind, 5].Value = dtOuter.Rows[0]["申请人"].ToString();
            mysheet.Cells[15 + ind, 7].Value = dtOuter.Rows[0]["申请部门"].ToString();
            mysheet.Cells[15 + ind, 9].Value = dtOuter.Rows[0]["审核人"].ToString();
            mysheet.Cells[15 + ind, 11].Value = Convert.ToDateTime(dtOuter.Rows[0]["审核日期"]).ToString("D");
            
            //返回
            return mysheet;
        }

    }
}
