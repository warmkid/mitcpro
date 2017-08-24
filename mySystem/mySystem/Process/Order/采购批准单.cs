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

namespace mySystem.Process.Order
{
    public partial class 采购批准单 : BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner,daInner库存,daInner实际购入;
        OleDbCommandBuilder cbOuter, cbInner,cbInner库存,cbInner实际购入;
        DataTable dtOuter, dtInner, dtInnerShow, dtInner库存, dtInner库存Show, dtInner实际购入;
        BindingSource bsOuter, bsInner, bsInner库存, bsInner实际购入;

        List<String> ls操作员, ls审核员;
        List<String> ls供应商;
        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        // 这个是原始数据，bing时用另外一个dt
        DataTable dt未批准需求单详细信息;
        // ht库存编码2采购数量 不考虑库存里的数量
        Hashtable ht未批准需求单ID2详细信息条数,ht库存编码2采购数量,ht采购需求单ID2用途;

        CheckForm ckform;

        string _ids;
        string 库存sql;

        public 采购批准单(MainForm mainform, string ids):base(mainform)
        {
            InitializeComponent();
            _ids = ids;
            // 新建
            fillPrinter();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            getOtherData(ids);
            fillUI();
            getPeople();
            setUseState();
            
            generateOuterData();
            outerBind();
            generateInnerData();
            setDataGridViewColumn();
            innerBind();



            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
            refreshSum(combobox存货编码筛选);
        }

        public 采购批准单(MainForm mainform, int id)
            : base(mainform)
        {
            // 根据ID显示
            InitializeComponent();
            fillPrinter();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            readOuterData(id);
            outerBind();
            

            getOtherDataByID(id);
            fillUI();
            getPeople();
            setUseState();

            //generateOuterData();
            //outerBind();
            //generateInnerData();
            readInnerData();
            setDataGridViewColumn();
            innerBind();
            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
            refreshSum(combobox存货编码筛选);
        }

        private void fillPrinter()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                combox打印机选择.Items.Add(sPrint);
            }
            combox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
            combox打印机选择.SelectedIndexChanged += new EventHandler(combox打印机选择_SelectedIndexChanged);
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        void combox打印机选择_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 订单用户权限 where 步骤='采购批准单'", conn);
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

        void getOtherData(string ids)
        {

            



            // 拿到需求单中未批准的详细信息
            // 拿到去重的存货编码和需要的数量
            // 按照存货编码排序
            // 读取库存台账中这些存货编码的数据
            OleDbDataAdapter da;
            DataTable dt;
            OleDbCommandBuilder cb;

            //下拉框需要供应商
            ls供应商 = new List<string>();
            da = new OleDbDataAdapter("select 供应商名称 from 设置供应商信息 order by 供应商名称", conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls供应商.Add(dr["供应商名称"].ToString());
            }



            ht采购需求单ID2用途 = new Hashtable();
            foreach (string strid in ids.Split(','))
            {
                int id = Convert.ToInt32(strid);
                da = new OleDbDataAdapter("select * from 采购需求单 where ID="+id, conn);
                dt = new DataTable();
                da.Fill(dt);
                ht采购需求单ID2用途[id] = dt.Rows[0]["用途"];
            }
            

            ht库存编码2采购数量 = new Hashtable();
            da = new OleDbDataAdapter("select * from 采购需求单详细信息 where 0=1", conn);
            dt未批准需求单详细信息 = new DataTable();
            da.Fill(dt未批准需求单详细信息);
            foreach (string strid in ids.Split(','))
            {
                int id = Convert.ToInt32(strid);
                da = new OleDbDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, conn);
                cb = new OleDbCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                DataRow[] drs = dt.Select("批准状态='未批准'");
                foreach (DataRow dr in drs)
                {
                    string 存货代码 = dr["存货代码"].ToString();
                    double 采购数量 = Convert.ToDouble(dr["数量"]);
                    DataRow ndr = dt未批准需求单详细信息.NewRow();
                    ndr.ItemArray = dr.ItemArray.Clone() as object[];
                    dt未批准需求单详细信息.Rows.Add(ndr);
                    if (!ht库存编码2采购数量.ContainsKey(存货代码))
                    {
                        ht库存编码2采购数量[存货代码] = 0.0;
                    }
                    ht库存编码2采购数量[存货代码] = 采购数量 + Convert.ToDouble(ht库存编码2采购数量[存货代码]);
                    dr["批准状态"] = "批准中";
                }
                da.Update(dt);
            }
            //按存货代码排序
            dt未批准需求单详细信息.DefaultView.Sort = "存货代码 ASC";
            dt未批准需求单详细信息 = dt未批准需求单详细信息.DefaultView.ToTable();
        }

        void getOtherDataByID(int id)
        {
            // 拿到需求单中未批准的详细信息
            // 拿到去重的存货编码和需要的数量
            // 按照存货编码排序
            // 读取库存台账中这些存货编码的数据
            OleDbDataAdapter da;
            DataTable dt;
            OleDbCommandBuilder cb;


            ls供应商 = new List<string>();
            da = new OleDbDataAdapter("select 供应商名称 from 设置供应商信息 order by 供应商名称", conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls供应商.Add(dr["供应商名称"].ToString());
            }

            //ht采购需求单ID2用途 = new Hashtable();
            //foreach (string strid in ids.Split(','))
            //{
            //    int id = Convert.ToInt32(strid);
            //    da = new OleDbDataAdapter("select * from 采购需求单 where ID=" + id, conn);
            //    dt = new DataTable();
            //    da.Fill(dt);
            //    ht采购需求单ID2用途[id] = dt.Rows[0]["用途"];
            //}


            ht库存编码2采购数量 = new Hashtable();
            da = new OleDbDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + id, conn);
            dt未批准需求单详细信息 = new DataTable();
            da.Fill(dt未批准需求单详细信息);
            //foreach (string strid in ids.Split(','))
            //{
            //    int id = Convert.ToInt32(strid);
            //    da = new OleDbDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, conn);
            //    cb = new OleDbCommandBuilder(da);
            //    dt = new DataTable();
            //    da.Fill(dt);
            //    DataRow[] drs = dt.Select("批准状态='未批准'");
            //    foreach (DataRow dr in drs)
            //    {
            //        string 存货代码 = dr["存货代码"].ToString();
            //        double 采购数量 = Convert.ToDouble(dr["数量"]);
            //        DataRow ndr = dt未批准需求单详细信息.NewRow();
            //        ndr.ItemArray = dr.ItemArray.Clone() as object[];
            //        dt未批准需求单详细信息.Rows.Add(ndr);
            //        if (!ht库存编码2采购数量.ContainsKey(存货代码))
            //        {
            //            ht库存编码2采购数量[存货代码] = 0.0;
            //        }
            //        ht库存编码2采购数量[存货代码] = 采购数量 + Convert.ToDouble(ht库存编码2采购数量[存货代码]);
            //        dr["批准状态"] = "批准中";
            //    }
            //    da.Update(dt);
            //}
            foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            {
                string 存货代码 = dr["存货代码"].ToString();
                double 采购数量 = Convert.ToDouble(dr["采购数量"]);
                if (!ht库存编码2采购数量.ContainsKey(存货代码))
                {
                    ht库存编码2采购数量[存货代码] = 0.0;
                }
                ht库存编码2采购数量[存货代码] = 采购数量 + Convert.ToDouble(ht库存编码2采购数量[存货代码]);
            }
            //按存货代码排序
            dt未批准需求单详细信息.DefaultView.Sort = "存货代码 ASC";
            dt未批准需求单详细信息 = dt未批准需求单详细信息.DefaultView.ToTable();
        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 采购批准单 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购批准单");

            daOuter.Fill(dtOuter);
        }


        void generateOuterData()
        {
            daOuter = new OleDbDataAdapter("select * from 采购批准单 where 0=1", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购批准单");

            daOuter.Fill(dtOuter);

            DataRow dr = dtOuter.NewRow();
            dr["申请日期"] = DateTime.Now;
            dr["申请人"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;
            dr["状态"] = "编辑中";
            dr["关联的采购需求单ID"] = _ids;
            //dr["审核结果"] = "未知";
            dtOuter.Rows.Add(dr);
            daOuter.Update(dtOuter);
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn;
            comm.CommandText = "select @@identity";
            int id  = (Int32)comm.ExecuteScalar();
            daOuter = new OleDbDataAdapter("select * from 采购批准单 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("采购批准单");
            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {
                if (c.Name == "cmb负责人") continue;
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

        void generateInnerData()
        {
            daInner = new OleDbDataAdapter("select * from 采购批准单详细信息 where 0=1", conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);

            foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            {
                DataRow ndr = dtInner.NewRow();
                ndr["采购批准单ID"] = dtOuter.Rows[0]["ID"];
                ndr["是否批准"] = false;
                ndr["存货代码"] = dr["存货代码"];
                ndr["存货名称"] = dr["存货名称"];
                ndr["规格型号"] = dr["规格型号"];
                ndr["采购件数"] = dr["采购件数"];
                ndr["采购数量"] = dr["采购数量"];
                ndr["用途"] = ht采购需求单ID2用途[dr["采购需求单ID"]];
                ndr["预计到货时间"] = DateTime.Now;
                ndr["采购需求单ID"] = dr["ID"];
                ndr["状态"] = "未采购";
                dtInner.Rows.Add(ndr);
            }

            daInner.Update(dtInner);
            daInner = new OleDbDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);
            dtInnerShow = dtInner;


            // 库存信息
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            库存sql = @"select ID, 产品代码,现存数量,用途,冻结状态 from 库存台帐 where 产品代码='{0}'";
            库存sql = string.Format(库存sql, DaiMaS[0]);
            List<String> whereS = new List<string>();
            if (DaiMaS.Count >= 1) 库存sql += " or ";
            for(int i=1;i<DaiMaS.Count;++i)
            {
                whereS.Add("产品代码='" + DaiMaS[i] + "'");
            }
            库存sql += String.Join(" or ", whereS.ToArray());
            daInner库存 = new OleDbDataAdapter(库存sql, conn);
            dtInner库存 = new DataTable("关联库存信息");
            cbInner库存 = new OleDbCommandBuilder(daInner库存);
            bsInner库存 =new BindingSource();
            daInner库存.Fill(dtInner库存);
            dtInner库存Show = dtInner库存;


            // 实际购入信息
            daInner实际购入 = new OleDbDataAdapter("select * from 采购批准单实际购入信息 where 0=1",conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new OleDbCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);

            foreach (string daima in DaiMaS)
            {
                DataRow ndr = dtInner实际购入.NewRow();
                ndr["采购批准单ID"] = dtOuter.Rows[0]["ID"];
                ndr["产品代码"] = daima;
                ndr["订单需求数量"] = ht库存编码2采购数量[daima];
                ndr["仓库可用"] = 0;
                ndr["实际购入"] = 0;
                dtInner实际购入.Rows.Add(ndr);
            }
            refresh仓库可用数据();

            daInner实际购入.Update(dtInner实际购入);

            daInner实际购入 = new OleDbDataAdapter("select * from 采购批准单实际购入信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new OleDbCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);
        }

        void refresh仓库可用数据()
        {
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            foreach (string daima in DaiMaS)
            {
                double avaNum = 0;
                DataRow[] drs = dtInner库存.Select("产品代码='" + daima + "'");
                foreach (DataRow dr in drs)
                {
                    if (!Convert.ToBoolean(dr["冻结状态"]))
                    {
                        avaNum += Convert.ToDouble(dr["现存数量"]);
                    }
                }
                drs = dtInner实际购入.Select("产品代码='" + daima + "'");
                if (drs.Length > 0)
                {
                    drs[0]["仓库可用"] = avaNum;
                }
            }
        }

        void readInnerData()
        {
            daInner = new OleDbDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);
            dtInnerShow = dtInner;


            // 库存信息
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            库存sql = @"select ID, 产品代码,现存数量,用途,冻结状态 from 库存台帐 where 产品代码='{0}'";
            库存sql = string.Format(库存sql, DaiMaS[0]);
            List<String> whereS = new List<string>();
            if (DaiMaS.Count >= 1) 库存sql += " or ";
            for (int i = 1; i < DaiMaS.Count; ++i)
            {
                whereS.Add("产品代码='" + DaiMaS[i] + "'");
            }
            库存sql += String.Join(" or ", whereS.ToArray());
            daInner库存 = new OleDbDataAdapter(库存sql, conn);
            dtInner库存 = new DataTable("关联库存信息");
            cbInner库存 = new OleDbCommandBuilder(daInner库存);
            bsInner库存 = new BindingSource();
            daInner库存.Fill(dtInner库存);
            dtInner库存Show = dtInner库存;


            // 实际购入信息
            daInner实际购入 = new OleDbDataAdapter("select * from 采购批准单实际购入信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new OleDbCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInnerShow;
            dataGridView1.DataSource = bsInner.DataSource;

            bsInner库存.DataSource = dtInner库存Show;
            dataGridView2.DataSource = bsInner库存.DataSource;

            bsInner实际购入.DataSource = dtInner实际购入;
            dataGridView3.DataSource = bsInner实际购入.DataSource;
        }


        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView2_DataBindingComplete);
            dataGridView2.CellValueChanged += new DataGridViewCellEventHandler(dataGridView2_CellValueChanged);

            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView3_DataBindingComplete);
        }

        void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "冻结状态")
                {
                    //dtInner库存 = dtInner库存Show;
                    //daInner库存.Update(dtInner库存);

                    //daInner库存 = new OleDbDataAdapter(库存sql, conn);
                    //dtInner库存 = new DataTable("关联库存信息");
                    //cbInner库存 = new OleDbCommandBuilder(daInner库存);
                    //bsInner库存 = new BindingSource();

                    //daInner库存.Fill(dtInner库存);
                    refresh仓库可用数据();
                    refreshSum(combobox存货编码筛选);
                }
            }
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "是否批准")
                {
                    //dtInner = dtInnerShow;
                    //daInner.Update((DataTable)bsInner.DataSource);
                    //// TODO 重新读数据，然后搞一个dtinnerShow
                    //daInner = new OleDbDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], conn);
                    //cbInner = new OleDbCommandBuilder(daInner);
                    //bsInner = new BindingSource();
                    //dtInner = new DataTable("采购批准单详细信息");

                    //daInner.Fill(dtInner);

                    //innerBind();
                    refresh仓库可用数据();
                    refreshSum(combobox存货编码筛选);
                }
            }
        }

        void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView3.Columns["ID"].Visible = false;
            dataGridView3.Columns["采购批准单ID"].Visible = false;
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["采购批准单ID"].Visible = false;
        }

        void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        dataGridView2.Columns["ID"].Visible = false;
        }

        void fillUI()
        {
            combobox存货编码筛选.Items.Add("全部");
            foreach (string daima in ht库存编码2采购数量.Keys.OfType<String>().ToList<String>())
            {
                combobox存货编码筛选.Items.Add(daima);
            }
            // TODO 两个事件


            combobox库存部分存货编码筛选.Items.Add("全部");
            foreach (string daima in ht库存编码2采购数量.Keys.OfType<String>().ToList<String>())
            {
                combobox库存部分存货编码筛选.Items.Add(daima);
            }

            combobox存货编码筛选.SelectedItem = "全部";
            combobox存货编码筛选.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox存货编码筛选.SelectedIndexChanged += new EventHandler(combobox存货编码筛选_SelectedIndexChanged);
        }

        void combobox存货编码筛选_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (sender as ComboBox);
            refreshSum(cmb);
            //if ("全部" == cmb.SelectedItem.ToString())
            //{
            //    dtInnerShow = dtInner;
            //    dtInner库存Show = dtInner库存;

            //    double sum = 0;
            //    foreach (DataRow dr in dtInner实际购入.Rows)
            //    {
            //        sum += Convert.ToDouble(dr["仓库可用"]);
            //    }
            //    label库存可用.Text = sum.ToString();


            //    sum = 0;
            //    foreach (DataRow dr in dtInner.Rows)
            //    {
            //        if (Convert.ToBoolean(dr["是否批准"]))
            //            sum += Convert.ToDouble(dr["采购数量"]);
            //    }
            //    double tmp = (sum - Convert.ToDouble(label库存可用.Text));
            //    if (tmp < 0) tmp = 0;
            //    label还需购入.Text = tmp.ToString();

            //}
            //else
            //{
            //    dtInnerShow = dtInner.Clone();
            //    DataRow[] drs;
            //    drs = dtInner.Select("存货代码='" + cmb.SelectedItem.ToString() + "'");
            //    foreach (DataRow dr in drs)
            //    {
            //        dtInnerShow.ImportRow(dr);
            //    }

            //    dtInner库存Show = dtInner库存.Clone();
            //    drs = dtInner库存.Select("产品代码='" + cmb.SelectedItem.ToString() + "'");
            //    foreach (DataRow dr in drs)
            //    {
            //        dtInner库存Show.ImportRow(dr);
            //    }

            //    drs = dtInner实际购入.Select("产品代码='" + cmb.SelectedItem.ToString() + "'");
            //    if (drs.Length > 0)
            //        label库存可用.Text = drs[0]["仓库可用"].ToString();


            //    double sum = 0;
            //    foreach (DataRow dr in dtInnerShow.Rows)
            //    {
            //        if (Convert.ToBoolean(dr["是否批准"]))
            //            sum += Convert.ToDouble(dr["采购数量"]);
            //    }
            //    double tmp1 = (sum - Convert.ToDouble(label库存可用.Text));
            //    if (tmp1 < 0) tmp1 = 0;
            //    label还需购入.Text = tmp1.ToString();

            //}
            //innerBind();
        }

        void refreshSum(ComboBox cmb)
        {
            
            if ("全部" == cmb.SelectedItem.ToString())
            {
                dtInnerShow = dtInner;
                dtInner库存Show = dtInner库存;

                double sum = 0;
                foreach (DataRow dr in dtInner实际购入.Rows)
                {
                    sum += Convert.ToDouble(dr["仓库可用"]);
                }
                label库存可用.Text = sum.ToString();


                sum = 0;
                foreach (DataRow dr in dtInner.Rows)
                {
                    if (Convert.ToBoolean(dr["是否批准"]))
                        sum += Convert.ToDouble(dr["采购数量"]);
                }
                double tmp = (sum - Convert.ToDouble(label库存可用.Text));
                if (tmp < 0) tmp = 0;
                label还需购入.Text = tmp.ToString();

            }
            else
            {
                dtInnerShow = dtInner.Clone();
                DataRow[] drs;
                drs = dtInner.Select("存货代码='" + cmb.SelectedItem.ToString() + "'");
                foreach (DataRow dr in drs)
                {
                    dtInnerShow.ImportRow(dr);
                }

                dtInner库存Show = dtInner库存.Clone();
                drs = dtInner库存.Select("产品代码='" + cmb.SelectedItem.ToString() + "'");
                foreach (DataRow dr in drs)
                {
                    dtInner库存Show.ImportRow(dr);
                }

                drs = dtInner实际购入.Select("产品代码='" + cmb.SelectedItem.ToString() + "'");
                if (drs.Length > 0)
                    label库存可用.Text = drs[0]["仓库可用"].ToString();


                double sum = 0;
                foreach (DataRow dr in dtInnerShow.Rows)
                {
                    if (Convert.ToBoolean(dr["是否批准"]))
                        sum += Convert.ToDouble(dr["采购数量"]);
                }
                double tmp1 = (sum - Convert.ToDouble(label库存可用.Text));
                if (tmp1 < 0) tmp1 = 0;
                label还需购入.Text = tmp1.ToString();

            }
            innerBind();
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
            readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            daInner库存.Update((DataTable)bsInner库存.DataSource);
            daInner实际购入.Update((DataTable)bsInner实际购入.DataSource);


            readInnerData();
            innerBind();
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            // TOOD 注意要改变需求单中的状态
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
                // 读内表，然后改写需求单
                OleDbDataAdapter da;
                OleDbCommandBuilder cb;
                DataTable dt;
                foreach (DataRow dr in dtInner.Rows)
                {
                    int xid = Convert.ToInt32(dr["采购需求单ID"]);
                    da = new OleDbDataAdapter("select * from 采购需求单详细信息 where ID=" + xid, conn);
                    cb = new OleDbCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (Convert.ToBoolean(dr["是否批准"]))
                    {
                        dt.Rows[0]["批准状态"] = "已批准";
                    }
                    else
                    {
                        dt.Rows[0]["批准状态"] = "未批准";
                    }
                    da.Update(dt);
                } 
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购批准单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            save();
            base.CheckResult();
        }

        bool check供应商()
        {
            foreach (DataRow dr in dtInner.Rows)
            {
                if (dr["推荐供应商"] == DBNull.Value) return false;
            }
            return true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            // 检查推荐供应商是否都写了
            if (!check供应商())
            {
                MessageBox.Show("供应商未填写完整！");
                return;
            }
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购批准单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "采购批准单";
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
