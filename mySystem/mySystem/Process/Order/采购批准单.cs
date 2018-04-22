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
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


// TODO 新数据，如果有可借用的东西，借用信息里为空，可用在generateinner时调用change借用信息多次
namespace mySystem.Process.Order
{
    public partial class 采购批准单 : BaseForm
    {
        bool isSaved = false;
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter daOuter, daInner,daInner库存,daInner实际购入, daInner借用订单;
        SqlCommandBuilder cbOuter, cbInner,cbInner库存,cbInner实际购入, cbInner借用订单;
        DataTable dtOuter, dtInner, dtInnerShow, dtInner库存, dtInner库存Show, dtInner实际购入, dtInner借用订单;
        BindingSource bsOuter, bsInner, bsInner库存, bsInner实际购入, bsInner借用订单;

        List<String> ls操作员, ls审核员;
        List<String> ls供应商;
        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        // 这个是原始数据，bing时用另外一个dt
        DataTable dt未批准需求单详细信息;
        // ht库存编码2采购数量 不考虑库存里的数量
        Hashtable ht未批准需求单ID2详细信息条数,ht库存编码2采购数量,ht采购需求单ID2用途,ht组件编码2组件订单需求号;
        Hashtable ht借用信息 = null;

        CheckForm ckform;

        string _ids;
        string 库存sql;
        string __自由 = "__自由";
        bool isFirstBind1 = true;
        bool isFirstBind2 = true;
        bool isFirstBind3 = true;
        bool isFirstBind4 = true; 

        public 采购批准单(MainForm mainform, string ids):base(mainform)
        {
            InitializeComponent();
            _ids = ids;
            // 新建
            fillPrinter();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
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
            dataRefresh();
            //refresh仓库可用数据();
            //refreshSum(combobox存货编码筛选);
        }

        public 采购批准单(MainForm mainform, int id)
            : base(mainform)
        {
            isSaved = true;
            // 根据ID显示
            InitializeComponent();
            fillPrinter();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
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
            dataRefresh();
            //refresh仓库可用数据();
            //refreshSum(combobox存货编码筛选);
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
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 订单用户权限 where 步骤='采购批准单'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

            ls操作员 = new List<string>(Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，"));
            ls审核员 = new List<string>(Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，"));
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
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
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
                else if (c is GroupBox)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is DataGridView) (cc as DataGridView).ReadOnly = true;
                    }
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
            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;

            //下拉框需要供应商
            ls供应商 = new List<string>();
            da = new SqlDataAdapter("select 供应商名称 from 设置供应商信息 order by 供应商名称", mySystem.Parameter.conn);
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
                da = new SqlDataAdapter("select * from 采购需求单 where ID=" + id, mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
                ht采购需求单ID2用途[id] = dt.Rows[0]["用途"];
            }
            

            ht库存编码2采购数量 = new Hashtable();
            da = new SqlDataAdapter("select * from 采购需求单详细信息 where 0=1", mySystem.Parameter.conn);
            dt未批准需求单详细信息 = new DataTable();
            da.Fill(dt未批准需求单详细信息);
            foreach (string strid in ids.Split(','))
            {
                int id = Convert.ToInt32(strid);
                da = new SqlDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                DataRow[] drs = dt.Select("批准状态='未批准'");
                foreach (DataRow dr in drs)
                {
                    string 存货代码 = dr["存货代码"].ToString();
                    double 采购数量 = Convert.ToDouble(dr["采购数量"]);
                    DataRow ndr = dt未批准需求单详细信息.NewRow();
                    ndr.ItemArray = dr.ItemArray.Clone() as object[];
                    dt未批准需求单详细信息.Rows.Add(ndr);
                    if (!ht库存编码2采购数量.ContainsKey(存货代码))
                    {
                        ht库存编码2采购数量[存货代码] = 0.0;
                    }
                    ht库存编码2采购数量[存货代码] = 采购数量 + Convert.ToDouble(ht库存编码2采购数量[存货代码]);
                }
            }
            //按存货代码排序
            dt未批准需求单详细信息.DefaultView.Sort = "组件订单需求流水号 ASC";
            dt未批准需求单详细信息 = dt未批准需求单详细信息.DefaultView.ToTable();

            //ht组件编码2组件订单需求号 = new Hashtable();
            //string curr编码 = generate组件订单需求流水号();
            //List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            //foreach (string daima in DaiMaS)
            //{
            //    if (ht组件编码2组件订单需求号.ContainsKey(daima)) continue;
            //    ht组件编码2组件订单需求号[daima] = curr编码;
            //    curr编码 = curr编码.Substring(0, 10) + (Convert.ToInt32(curr编码.Substring(10, 3)) + 1).ToString("D3");
            //}

        }

        void getOtherDataByID(int id)
        {
            // 拿到需求单中未批准的详细信息
            // 拿到去重的存货编码和需要的数量
            // 按照存货编码排序
            // 读取库存台账中这些存货编码的数据
            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;


            ls供应商 = new List<string>();
            da = new SqlDataAdapter("select 供应商名称 from 设置供应商信息 order by 供应商名称", mySystem.Parameter.conn);
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
            //    da = new SqlDataAdapter("select * from 采购需求单 where ID=" + id, conn);
            //    dt = new DataTable();
            //    da.Fill(dt);
            //    ht采购需求单ID2用途[id] = dt.Rows[0]["用途"];
            //}


            ht库存编码2采购数量 = new Hashtable();
            da = new SqlDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + id, mySystem.Parameter.conn);
            dt未批准需求单详细信息 = new DataTable();
            da.Fill(dt未批准需求单详细信息);
            //foreach (string strid in ids.Split(','))
            //{
            //    int id = Convert.ToInt32(strid);
            //    da = new SqlDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, conn);
            //    cb = new SqlCommandBuilder(da);
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

            ht组件编码2组件订单需求号 = new Hashtable();
            foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            {
                string 存货代码 = dr["存货代码"].ToString();
                string 组件订单需求流水号 = dr["组件订单需求流水号"].ToString();
                if (ht库存编码2采购数量.ContainsKey(存货代码)) continue;
                ht组件编码2组件订单需求号[存货代码] = 组件订单需求流水号;
            }
        }

        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 采购批准单 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购批准单");

            daOuter.Fill(dtOuter);
        }


        void generateOuterData()
        {
            daOuter = new SqlDataAdapter("select * from 采购批准单 where 0=1", mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购批准单");

            daOuter.Fill(dtOuter);

            DataRow dr = dtOuter.NewRow();
            dr["采购申请批准单号"] = generate采购申请批准单号();
            dr["申请日期"] = DateTime.Now;
            dr["申请人"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;
            dr["审核结果"]=0;
            dr["状态"] = "编辑中";
            dr["关联的采购需求单ID"] = _ids;
            //dr["审核结果"] = "未知";
            dtOuter.Rows.Add(dr);
            daOuter.Update(dtOuter);
            SqlCommand comm = new SqlCommand();
            comm.Connection = mySystem.Parameter.conn;
            comm.CommandText = "select @@identity";
            int id = Convert.ToInt32(comm.ExecuteScalar().ToString());
            //int id  = (Int32)comm.ExecuteScalar(); //id=0？？？？？
            daOuter = new SqlDataAdapter("select * from 采购批准单 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
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
            daInner = new SqlDataAdapter("select * from 采购批准单详细信息 where 0=1", mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);

            foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            {
                string gyscpbm;
                SqlDataAdapter da = new SqlDataAdapter("select 供应商产品编码 from 设置存货档案 where 存货代码='" + dr["存货代码"].ToString() + "'", mySystem.Parameter.conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0) gyscpbm = "";
                else
                {
                    gyscpbm = dt.Rows[0]["供应商产品编码"].ToString() ;
                }

                DataRow ndr = dtInner.NewRow();
                ndr["采购批准单ID"] = dtOuter.Rows[0]["ID"];
                ndr["是否批准"] = true;
                ndr["组件订单需求流水号"] = dr["组件订单需求流水号"];
                ndr["存货代码"] = dr["存货代码"];
                ndr["存货名称"] = dr["存货名称"];
                ndr["规格型号"] = dr["规格型号"];
                ndr["采购件数"] = dr["采购件数"];
                ndr["采购数量"] = dr["采购数量"];
                ndr["用途"] = ht采购需求单ID2用途[dr["采购需求单ID"]];
                ndr["预计到货时间"] = DateTime.Now;
                ndr["推荐供应商"] = dr["推荐供应商"];
                ndr["供应商产品编码"] = gyscpbm;
                ndr["采购需求单ID"] = dr["ID"];
                ndr["状态"] = "未采购";
                ndr["推荐供应商"] = dr["推荐供应商"];
                ndr["未借用前需要采购数量"] = dr["采购数量"];
                if (Convert.ToDouble(dr["采购件数"]) == 0)
                { ndr["主计量单位每辅计量单位"] = -1; }
                else
                { ndr["主计量单位每辅计量单位"] = (Convert.ToDouble(dr["采购数量"]) / Convert.ToDouble(dr["采购件数"])); }
                //ndr["主计量单位每辅计量单位"] = Convert.ToSingle(dr["采购数量"]) / Convert.ToSingle(dr["采购件数"]);
                ndr["已借用数量"] = 0;
                dtInner.Rows.Add(ndr);
            }

            daInner.Update(dtInner);
            daInner = new SqlDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);
            dtInnerShow = dtInner;


            // 库存信息
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            库存sql = @"select ID, 产品代码, 现存数量, 用途, 冻结状态 from ( select * from 库存台帐 where 状态 = '合格' )P where 产品代码 = '{0}'";
            库存sql = string.Format(库存sql, DaiMaS[0]);
            List<String> whereS = new List<string>();
            if (DaiMaS.Count > 1) 库存sql += " or ";
            for(int i=1;i<DaiMaS.Count;++i)
            {
                whereS.Add("产品代码='" + DaiMaS[i] + "'");
            }
            库存sql += String.Join(" or ", whereS.ToArray());
            库存sql += " order by 产品代码";
            daInner库存 = new SqlDataAdapter(库存sql, mySystem.Parameter.conn);
            dtInner库存 = new DataTable("关联库存信息");
            cbInner库存 = new SqlCommandBuilder(daInner库存);
            bsInner库存 =new BindingSource();         
            daInner库存.Fill(dtInner库存);
            List<int> inv = new List<int>();
            for(int i=dtInner库存.Rows.Count-1;i>=0;--i)
            {
                if (Convert.ToDouble(dtInner库存.Rows[i]["现存数量"]) <= 0)
                {
                    inv.Add(i);
                }
            }
            foreach (int i in inv)
            {
                dtInner库存.Rows.RemoveAt(i);
            }
            dtInner库存Show = dtInner库存;

            //refresh仓库可用数据();

            // 实际购入信息
            daInner实际购入 = new SqlDataAdapter("select * from 采购批准单实际购入信息 where 0=1", mySystem.Parameter.conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new SqlCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);

            foreach (string daima in DaiMaS)
            {
                DataRow ndr = dtInner实际购入.NewRow();
                ndr["采购批准单ID"] = dtOuter.Rows[0]["ID"];
                ndr["产品代码"] = daima;
                ndr["订单需求数量"] = ht库存编码2采购数量[daima];
                ndr["仓库可用"] = 0;
                ndr["实际购入"] = 0;
                ndr["富余量"] = 0;
                ndr["借用信息"] = "[]";
                ndr["可借数量"] = 0;
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案 where 存货代码='" + daima + "'", mySystem.Parameter.conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count==0)    {
                    MessageBox.Show("找不到代码为:"+daima+" 的组件");
                    continue;
                }
                double 换算率;
                try
                {
                    换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(daima + " 的换算率读取失败，默认设为0");
                    换算率 = 0;
                }
                ndr["换算率"] = 换算率;
                dtInner实际购入.Rows.Add(ndr);
            }

             refresh仓库可用数据();
            // 第一次填写实际购入值
             foreach (DataRow dr in dtInner实际购入.Rows)
             {
                 double d = Convert.ToDouble(dr["订单需求数量"]) - Convert.ToDouble(dr["仓库可用"]);
                 if (d < 0) d = 0;
                 dr["实际购入"] = d;
             }

            daInner实际购入.Update(dtInner实际购入);

            daInner实际购入 = new SqlDataAdapter("select * from 采购批准单实际购入信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new SqlCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);

           

            // 借用订单数据
            daInner借用订单 = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner借用订单 = new BindingSource();
            dtInner借用订单 = new DataTable("采购批准单借用订单详细信息");
            cbInner借用订单 = new SqlCommandBuilder(daInner借用订单);
            daInner借用订单.Fill(dtInner借用订单);
            dataRefresh();
            // 获取库存中非自由状态下的所有信息
            foreach (DataRow dr in dtInner库存.Rows)
            {
                string 产品代码 = dr["产品代码"].ToString();
                string 用途 = dr["用途"].ToString();
                bool 冻结 = Convert.ToBoolean(dr["冻结状态"]);
                change借用信息(产品代码, 用途, !冻结);
                change借用信息(产品代码, 用途, 冻结);
            }
        }

        void refresh仓库可用数据()
        {
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            foreach (string daima in DaiMaS)
            {
                double 可借数量 = 0;
                double 仓库可用 = 0;
                double 订单需求数量 = 0;
                DataRow[] drs = dtInner库存Show.Select("产品代码='" + daima + "'");
                foreach (DataRow dr in drs)
                {
                    if (!Convert.ToBoolean(dr["冻结状态"]) && dr["用途"].ToString()!=__自由)
                    {
                        可借数量 += Convert.ToDouble(dr["现存数量"]);
                    }
                    if (dr["用途"].ToString() == __自由)
                    {
                        仓库可用 += Convert.ToDouble(dr["现存数量"]);
                    }
                }
                drs = dtInner.Select("存货代码='" + daima + "'");
                //drs = dtInner.Select("是否批准=true");
                foreach (DataRow dr in drs)
                {
                    if (Convert.ToBoolean(dr["是否批准"]))
                        订单需求数量 += Convert.ToDouble(dr["采购数量"]) + Convert.ToDouble(dr["已借用数量"]);
                }


                drs = dtInner实际购入.Select("产品代码='" + daima + "'");
                if (drs.Length > 0)
                {
                    drs[0]["可借数量"] = 可借数量;
                    drs[0]["仓库可用"] = 仓库可用;
                    drs[0]["订单需求数量"] = 订单需求数量;
                }
            }


        }

        void readInnerData()
        {
            daInner = new SqlDataAdapter("select * from 采购批准单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购批准单详细信息");

            daInner.Fill(dtInner);
            dtInnerShow = dtInner;


            // 库存信息
            List<String> DaiMaS = ht库存编码2采购数量.Keys.OfType<String>().ToList<String>();
            库存sql = @"select ID, 产品代码,现存数量,用途,冻结状态 from (select * from 库存台帐 where 状态='合格') t where 产品代码='{0}'";
            if (DaiMaS.Count > 0){ 库存sql = string.Format(库存sql, DaiMaS[0]);}
            else { 库存sql = string.Format(库存sql, ""); }
            List<String> whereS = new List<string>();
            if (DaiMaS.Count > 1) 库存sql += " or ";
            for (int i = 1; i < DaiMaS.Count; ++i)
            {
                whereS.Add("产品代码='" + DaiMaS[i] + "'");
            }
            库存sql += String.Join(" or ", whereS.ToArray());
            库存sql += " order by 产品代码";
            daInner库存 = new SqlDataAdapter(库存sql, mySystem.Parameter.conn);
            dtInner库存 = new DataTable("关联库存信息");
            cbInner库存 = new SqlCommandBuilder(daInner库存);
            bsInner库存 = new BindingSource();
            daInner库存.Fill(dtInner库存);
            dtInner库存Show = dtInner库存;


            // 实际购入信息
            daInner实际购入 = new SqlDataAdapter("select * from 采购批准单实际购入信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner实际购入 = new BindingSource();
            dtInner实际购入 = new DataTable("采购批准单实际购入信息");
            cbInner实际购入 = new SqlCommandBuilder(daInner实际购入);
            daInner实际购入.Fill(dtInner实际购入);

            // 借用订单信息
            daInner借用订单 = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner借用订单 = new BindingSource();
            dtInner借用订单 = new DataTable("采购批准单借用订单详细信息");
            cbInner借用订单 = new SqlCommandBuilder(daInner借用订单);
            daInner借用订单.Fill(dtInner借用订单);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInnerShow;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
            //setDGV本订单采购信息Column();

            bsInner库存.DataSource = dtInner库存Show;
            dataGridView2.DataSource = bsInner库存.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);

            bsInner实际购入.DataSource = dtInner实际购入;
            dataGridView3.DataSource = bsInner实际购入.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView3);

            bsInner借用订单.DataSource = dtInner借用订单;
            dataGridView4.DataSource = bsInner借用订单.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView4);
            //setDGV借用订单采购信息Column();
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
            dataGridView3.CellValueChanged += new DataGridViewCellEventHandler(dataGridView3_CellValueChanged);
            dataGridView3.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView3_DataBindingComplete);

            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.RowHeadersVisible = false;
            dataGridView4.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView4_DataBindingComplete);
            dataGridView4.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView4_EditingControlShowing);

            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                if (dgvc.Name != "是否批准")
                    dgvc.ReadOnly = true;
            }
            foreach (DataGridViewColumn dgvc in dataGridView2.Columns)
            {
                if (dgvc.Name != "冻结状态")
                    dgvc.ReadOnly = true;
            }
            foreach (DataGridViewColumn dgvc in dataGridView3.Columns)
            {
                if (dgvc.Name != "实际购入")
                    dgvc.ReadOnly = true;
            }
            foreach (DataGridViewColumn dgvc in dataGridView4.Columns)
            {
                dgvc.ReadOnly = true;
            }

            dataGridView3.Columns["换算率"].Visible = false;

        }

        void dataGridView4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView4.CurrentCell.OwningColumn.Name == "推荐供应商")
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

        void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView3.Columns[e.ColumnIndex].Name == "实际购入")
                {
                    double 订单需求数量 = Convert.ToDouble(dataGridView3["订单需求数量", e.RowIndex].Value);
                    double 仓库可用 = Convert.ToDouble(dataGridView3["仓库可用", e.RowIndex].Value);
                    double 实际购入 = Convert.ToDouble(dataGridView3["实际购入", e.RowIndex].Value);
                    double 换算率 = Convert.ToDouble(dataGridView3["换算率", e.RowIndex].Value);
                    dataGridView3["富余量", e.RowIndex].Value = 实际购入 - (订单需求数量 - 仓库可用);
                    if (0 != 实际购入 % 换算率)
                    {
                        dataGridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dataGridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        void dataGridView4_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView4.Columns["ID"].Visible = false;
            dataGridView4.Columns["采购批准单ID"].Visible = false;
            if (isFirstBind4)
            {
                readDGVWidthFromSettingAndSet(dataGridView4);
                isFirstBind4 = false;
            }
        }

        void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "冻结状态")
                {
                    
                    int id = Convert.ToInt32(dataGridView2["ID", e.RowIndex].Value);
                    bool b = Convert.ToBoolean(dataGridView2["冻结状态", e.RowIndex].Value);
                    string 产品代码 = dataGridView2["产品代码", e.RowIndex].Value.ToString();
                    string 用途 = dataGridView2["用途", e.RowIndex].Value.ToString();
                    // 更新dtInner库存
                    if (combobox存货编码筛选.SelectedItem.ToString() != "全部")
                    {
                        
                        dtInner库存.Select("ID=" + id)[0]["冻结状态"] = b;
                    }
                    dataRefresh();
                    change借用信息(产品代码, 用途, b);
                    // 增减因为借用产生的详细信息
                    //change批准单详细信息(产品代码, 用途, b);
                    
                    generate因为借用产生的dtInner(产品代码);
                    innerBind();
                    //refresh仓库可用数据();
                    //refreshSum(combobox存货编码筛选);
                    
                }
            }
        }

        private void generate因为借用产生的dtInner(string 产品代码)
        {
            DataRow[] drs = dtInner.Select("存货代码='" + 产品代码 + "'");
            if (drs.Length <= 0) return;
            string 存货名称 = null;
            string 规格型号 = null;
            double 主计量单位每辅计量单位 = 1;
            string 需求单流水号 = "";
            存货名称 = drs[0]["存货名称"].ToString();
            规格型号 = drs[0]["规格型号"].ToString();
            主计量单位每辅计量单位 = Convert.ToDouble(drs[0]["主计量单位每辅计量单位"]);
            需求单流水号 = drs[0]["组件订单需求流水号"].ToString();
            //原始订单数据复原
            drs[0]["采购数量"] = drs[0]["未借用前需要采购数量"];
            drs[0]["已借用数量"] = 0;
            // 把本产品代码下所有借用dtinner信息删掉
            drs = dtInner借用订单.Select("存货代码='" + 产品代码 + "'");
            if (drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    
                        dr.Delete();
                }
            }

            daInner借用订单.Update((DataTable)bsInner借用订单.DataSource);

            daInner借用订单 = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner借用订单 = new BindingSource();
            dtInner借用订单 = new DataTable("采购批准单借用订单详细信息");
            cbInner借用订单 = new SqlCommandBuilder(daInner借用订单);
            daInner借用订单.Fill(dtInner借用订单);

            innerBind();
            // 然后根据借用信息生成新的dtinner
            // 生成的同时要减去现有的数据
            drs = dtInner实际购入.Select("产品代码 ='" + 产品代码 + "'");
            if(drs.Length>0){
                JArray ja = JArray.Parse(drs[0]["借用信息"].ToString());
                foreach (JToken jt in ja)
                {
                    string 用途 = jt["用途"].ToString();
                    double 数量 = Convert.ToDouble(jt["数量"]);
                    if (数量 == 0)
                    {
                        continue;
                    }
                    DataRow dr = dtInner借用订单.NewRow();
                    dr["采购批准单ID"] = dtOuter.Rows[0]["ID"];
                    dr["组件订单需求流水号"] = 需求单流水号;
                    dr["存货代码"] = 产品代码;
                    dr["存货名称"] = 存货名称;
                    dr["规格型号"] = 规格型号;
                    dr["采购件数"] = 数量 / 主计量单位每辅计量单位;
                    dr["采购数量"] = 数量;
                    dr["用途"] = 用途;
                    dr["预计到货时间"] = DateTime.Now;
                    dr["状态"] = "未采购";
                    dtInner借用订单.Rows.Add(dr);
                    // 减
                    double 待减数量 = 数量;
                    drs = dtInner.Select("存货代码='" + 产品代码 + "'");
                    foreach (DataRow idr in drs)
                    {
                        if (Convert.ToBoolean(idr["是否批准"]))
                        {
                            double 拟采购数量 = Convert.ToDouble(idr["采购数量"]);
                            if (数量 <= 拟采购数量)
                            {
                                idr["采购数量"] = Convert.ToDouble(idr["采购数量"]) - 数量;
                                idr["已借用数量"] = Convert.ToDouble(idr["已借用数量"]) + 数量;
                                break;
                            }
                            else
                            {
                                待减数量 -= 拟采购数量;
                                idr["采购数量"] = 0;
                                idr["已借用数量"] =  Convert.ToDouble(idr["已借用数量"]) + 拟采购数量;
                            }
                        }
                    }
                }
            }
            daInner借用订单.Update((DataTable)bsInner借用订单.DataSource);

            daInner借用订单 = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 采购批准单ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            bsInner借用订单 = new BindingSource();
            dtInner借用订单 = new DataTable("采购批准单借用订单详细信息");
            cbInner借用订单 = new SqlCommandBuilder(daInner借用订单);
            daInner借用订单.Fill(dtInner借用订单);

            innerBind();
        }

        private void change批准单详细信息(string 产品代码, string 用途, bool b)
        {
            if (用途 == __自由) return;
            if (b)
            {
                // b为真，表示去掉一个借用信息
                // 在dtinner找到对应记录，删除（是否需要保存？）
                DataRow[] drs = dtInner.Select("存货代码='" + 产品代码 + "' and 用途='" + 用途 + "'");
                foreach (DataRow dr in drs)
                {
                    dr.Delete();
                }
            }
            else
            {
                //b为假，表示要增加一条记录


            }
        }

        private void change借用信息(string 产品代码, string 用途, bool b)
        {
            DataRow[] drs;
            // 如果用途是自由，直接返回
            if (用途 == __自由) return;
            // 读取该产品代码的历史借用数据，如果没有则refresh借用信息
            refresh借用信息();
            Hashtable curr = (Hashtable) ht借用信息[产品代码];
            // 如果b为true，表示减少借用数量
            if (b)
            {
                if (curr.ContainsKey(用途)) curr.Remove(用途);
                drs = dtInner实际购入.Select("产品代码='" + 产品代码 + "'");
                if (drs.Length > 0)
                {
                    JArray ja = JArray.Parse(drs[0]["借用信息"].ToString());
                    JToken jt2Del = null;
                    foreach (JToken jt in ja)
                    {
                        if (jt["用途"].ToString() == 用途)
                        {
                            jt2Del = jt;
                            break;
                        }
                        
                    }
                    ja.Remove(jt2Del);
                    drs[0]["借用信息"] = ja.ToString().Replace("\r\n", "");
                }
            }
            // 在hashtable中找到该记录，删除
            //
            else
            {
                drs = dtInner库存Show.Select("用途='" + 用途 + "' and 产品代码='" + 产品代码 + "'");
                if (drs.Length > 0)
                {
                    double 库存数量 = Convert.ToDouble(drs[0]["现存数量"]);
                    double 需要数量;
                    DataRow[] tmp = dtInnerShow.Select("存货代码='" + 产品代码 + "'");
                    if (Convert.ToBoolean(tmp[0]["是否批准"]))
                    {
                        需要数量 = Convert.ToDouble(tmp[0]["采购数量"]);
                    }
                    else
                    {
                        需要数量 = 0;
                    }
                   
                    double 仓库可用 = Convert.ToDouble(dtInner实际购入.Select("产品代码='" + 产品代码 + "'")[0]["仓库可用"]);
                    double 已经借用 = 0;
                    foreach (string yt in curr.Keys.OfType<String>().ToArray<String>())
                    {
                        已经借用 += Convert.ToDouble(curr[yt]);
                    }
                    double 还需借数量 = 需要数量 - 仓库可用 - 已经借用;
                    if (还需借数量 < 0) 还需借数量 = 0;
                    double 本次实际借用数量 = Math.Min(还需借数量, 库存数量);
                    if (!curr.ContainsKey(用途)) curr[用途] = 0;
                    curr[用途] = Convert.ToDouble(curr[用途]) + 本次实际借用数量;
                    drs = dtInner实际购入.Select("产品代码='" + 产品代码 + "'");
                    if (drs.Length > 0)
                    {
                        // drs[0]["可借数量"] = Convert.ToDouble(drs[0]["可借数量"]) + 库存数量;
                        JArray ja = JArray.Parse("[]");
                        foreach (string yt in curr.Keys.OfType<String>().ToArray<String>())
                        {

                            JObject jo = JObject.Parse("{}");
                            jo["用途"] = yt;
                            jo["数量"] = Convert.ToDouble(curr[yt]);
                            ja.Add(jo);
                        }
                        drs[0]["借用信息"] = ja.ToString().Replace("\r\n", "");
                    }
                    
                }
                
            }
            refresh借用信息();
            // 如果b为false，表示增加借用数量
                // 在dtinner库存中找到该记录，读取数量
                // 可借数量增加
                // 需要数量-仓库可用-历史已经借用数量 = 还需借的数量
                // 本次借用数量= 还需借的数量和他有的数量的最小值
                // 完成计算加入hashtable
            // 要更新dtinner实际购入中的数据（可借数量，借用信息）
            // 将hashtable变成json，存入
            // 添加datagridview提示
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "是否批准")
                {
                    // 更新dtIner
                    string 产品代码 = dataGridView1["存货代码", e.RowIndex].Value.ToString(); ;
                    if (combobox存货编码筛选.SelectedItem.ToString() != "全部")
                    {
                        
                        int id = Convert.ToInt32(dataGridView1["ID", e.RowIndex].Value);
                        bool b = Convert.ToBoolean(dataGridView1["是否批准", e.RowIndex].Value);
                        dtInner.Select("ID=" + id)[0]["是否批准"] = b;
                    }
                    dataRefresh();
                    change借用信息(产品代码);
                    generate因为借用产生的dtInner(产品代码);
                    //refresh仓库可用数据();
                    //refreshSum(combobox存货编码筛选);
                }
            }
        }

        private void change借用信息(string 产品代码)
        {
            // 这个函数是在可借用的订单不变得情况下调用的
            // 读取订单需求数量，仓库可用，得到还缺少的数量
            DataRow[] drs = dtInner实际购入.Select("产品代码='" + 产品代码 + "'");
            if (drs.Length > 0) {
                double 订单需求数量 = Convert.ToDouble(drs[0]["订单需求数量"]);
                double 仓库可用 = Convert.ToDouble(drs[0]["仓库可用"]);
                double 还缺少 = 订单需求数量 - 仓库可用;
                // 看看现在已经借了多少了
                double 已经借用数量 = 0;
                JArray ja = JArray.Parse(drs[0]["借用信息"].ToString());
                foreach (JToken jt in ja)
                {
                    已经借用数量 += Convert.ToDouble(jt["数量"]);
                }
                // 如果缺少的和已经借的相等，就不管了
                if (还缺少 == 已经借用数量) return;
                // 如果还缺的大于可借的，表示要多借
                if (还缺少 > 已经借用数量)
                {
                    // 得到缺额，遍历库存信息，增加借用数量
                    // 直到缺额被补齐或者遍历结束
                    // 写dtinner实际购入
                    double 缺额 = 还缺少 - 已经借用数量;
                    foreach (JToken jt in ja)
                    {
                        string 用途 = jt["用途"].ToString();
                        DataRow[] tmp;
                        tmp = dtInner库存.Select("用途='" + 用途 + "' and 产品代码='" + 产品代码 + "'");
                        double 本次最多可借 = Convert.ToDouble(tmp[0]["现存数量"]);
                        // 补齐了
                        if (Convert.ToDouble(jt["数量"]) + 缺额 <= 本次最多可借)
                        {
                            jt["数量"] = Convert.ToDouble(jt["数量"]) + 缺额;
                            break;
                        }
                            //补不齐
                        else
                        {
                            缺额 -= 本次最多可借 - Convert.ToDouble(jt["数量"]);
                            jt["数量"] = 本次最多可借;
                        }
                    }

                    drs[0]["借用信息"] = ja.ToString().Replace("\r\n", "");

                }
                // 如果还缺的小于可借的，表示不用借那么多了
                 // 得到缺额，遍历库存信息，减少借用数量

                // 直到缺额被补齐或者遍历结束
                // 写dtinner实际购入
                if (还缺少 < 已经借用数量)
                {
                    double 缺额 = 已经借用数量 - 还缺少;
                    foreach (JToken jt in ja)
                    {
                        string 用途 = jt["用途"].ToString();
                        DataRow[] tmp;
                        tmp = dtInner库存.Select("用途='" + 用途 + "' and 产品代码='" + 产品代码 + "'");
                        double 本次最多可借 = Convert.ToDouble(tmp[0]["现存数量"]);
                        // 
                        if (Convert.ToDouble(jt["数量"]) >= 缺额)
                        {
                            jt["数量"] = Convert.ToDouble(jt["数量"]) - 缺额;
                            break;
                        }
                        //
                        else
                        {
                            缺额 -= Convert.ToDouble(jt["数量"]);
                            jt["数量"] = 0;
                        }
                    }

                    drs[0]["借用信息"] = ja.ToString().Replace("\r\n", "");
                }
            }
            
            
            
               
            
        }

        void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView3.Columns["ID"].Visible = false;
            dataGridView3.Columns["采购批准单ID"].Visible = false;


            dataGridView3.ShowCellToolTips = true;
            // 检查hashtable1是否初始化
            if (ht借用信息 == null) ht借用信息 = new Hashtable();

            // 遍历dtinner实际购入信息，以产品代码为hashtable1的key，没有该key，则为它加入一个，value为new hashtable
            foreach (DataGridViewRow dgvr in dataGridView3.Rows)
            {
                string 产品代码 = dgvr.Cells["产品代码"].Value.ToString();
                if (ht借用信息.ContainsKey(产品代码)) ht借用信息.Remove(产品代码);
                ht借用信息[产品代码] = new Hashtable();

                // 从dtinner实际购入中读取历史借用数据
                JArray ja = JArray.Parse(dgvr.Cells["借用信息"].Value.ToString());
                List<String> tips = new List<string>();
                foreach (JToken jt in ja)
                {
                    string 用途 = jt["用途"].ToString();
                    double 数量 = Convert.ToDouble(jt["数量"]);
                    ((Hashtable)ht借用信息[产品代码])[用途] = 数量;
                    tips.Add("用途: " + 用途 + "\t" + "借用数量：" + 数量);
                }

                // 添加datagridvew提示
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    dgvc.ToolTipText = String.Join("\n", tips);
                }
            }
            if (isFirstBind3)
            {
                readDGVWidthFromSettingAndSet(dataGridView3);
                isFirstBind3 = false;
            }

        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["采购批准单ID"].Visible = false;
            dataGridView1.Columns["未借用前需要采购数量"].Visible = false;
            dataGridView1.Columns["主计量单位每辅计量单位"].Visible = false;
            dataGridView1.Columns["已借用数量"].Visible = false;
            dataGridView1.Columns["采购需求单ID"].Visible = false;
            dataGridView1.Columns["状态"].Visible = false;
            List<string> ls可用修改的列 = new List<string>(new String[] { "是否批准", "预计到货时间", "备注","供应商产品编码" });
            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                if (!ls可用修改的列.Contains(dgvc.Name))
                {
                    dgvc.ReadOnly = true;
                }
            }
            if (isFirstBind1)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind1 = false;
            }
        }

        void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView2.Columns["ID"].Visible = false;
            if (isFirstBind2)
            {
                readDGVWidthFromSettingAndSet(dataGridView2);
                isFirstBind2 = false;
            }
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
                if (dtInner实际购入 == null) return;
                foreach (DataRow dr in dtInner实际购入.Rows)
                {
                    sum += Convert.ToDouble(dr["仓库可用"]);
                }
                label仓库可用.Text = sum.ToString();

                sum = 0;
                foreach (DataRow dr in dtInner实际购入.Rows)
                {
                    sum += Convert.ToDouble(dr["可借数量"]);
                }
                label可借数量.Text = sum.ToString();

                sum = 0;
                foreach (DataRow dr in dtInner.Rows)
                {
                    if (Convert.ToBoolean(dr["是否批准"]))
                        sum += Convert.ToDouble(dr["采购数量"]);
                }
                double tmp = (sum - Convert.ToDouble(label仓库可用.Text));
                if (tmp < 0) tmp = 0;
                label最少应购.Text = tmp.ToString();

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
                {
                    label仓库可用.Text = drs[0]["仓库可用"].ToString();
                    label可借数量.Text = drs[0]["可借数量"].ToString();
                }


                double sum = 0;
                foreach (DataRow dr in dtInnerShow.Rows)
                {
                    if (Convert.ToBoolean(dr["是否批准"]))
                        sum += Convert.ToDouble(dr["采购数量"]);
                }
                double tmp1 = (sum - Convert.ToDouble(label仓库可用.Text));
                if (tmp1 < 0) tmp1 = 0;
                label最少应购.Text = tmp1.ToString();

            }
            innerBind();
        }

        private void btn确认_Click(object sender, EventArgs e)
        {
            isSaved = true;
            int idx = combobox存货编码筛选.SelectedIndex;
            save();
            if (_userState == Parameter.UserState.操作员)
            {
                btn提交审核.Enabled = true;
            }
            combobox存货编码筛选.SelectedIndex = 0;

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;
            //foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            //{
            //    int id = Convert.ToInt32( dr["ID"]);
            //    da = new SqlDataAdapter("select * from 采购需求单详细信息 where ID=" + id, mySystem.Parameter.conn);
            //    cb = new SqlCommandBuilder(da);
            //    dt = new DataTable();
            //    da.Fill(dt);
            //    if (dt.Rows.Count == 0) continue;
            //    dt.Rows[0]["批准状态"] = "批准中";
            //    da.Update(dt);
            //}
            // 根据内表改，上面是错误的
            foreach (DataRow dr in dtInner.Rows)
            {
                if (Convert.ToBoolean(dr["是否批准"]))
                {
                    int xqdID = Convert.ToInt32(dr["采购需求单ID"]);
                    da = new SqlDataAdapter("select * from 采购需求单详细信息 where ID=" + xqdID, mySystem.Parameter.conn);
                    cb = new SqlCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) continue;
                    dt.Rows[0]["批准状态"] = "批准中";
                    da.Update(dt);
                }
                else
                {
                    int xqdID = Convert.ToInt32(dr["采购需求单ID"]);
                    da = new SqlDataAdapter("select * from 采购需求单详细信息 where ID=" + xqdID, mySystem.Parameter.conn);
                    cb = new SqlCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) continue;
                    dt.Rows[0]["批准状态"] = "未批准";
                    da.Update(dt);
                }
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
            daInner借用订单.Update((DataTable)bsInner借用订单.DataSource);

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
                SqlDataAdapter da;
                SqlCommandBuilder cb;
                DataTable dt;
                foreach (DataRow dr in dtInner.Rows)
                {
                    int xid = Convert.ToInt32(dr["采购需求单ID"]);
                    da = new SqlDataAdapter("select * from 采购需求单详细信息 where ID=" + xid, mySystem.Parameter.conn);
                    cb = new SqlCommandBuilder(da);
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
                // 修改库存台账
                update库存台账_审核通过之后调用();
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
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='采购批准单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            save();
            base.CheckResult();
        }

        void update库存台账_审核通过之后调用()
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;
            string sql;
            // 遍历dtinner，未批准的跳过
           
            foreach (DataRow dr in dtInner.Rows)
            {
                if (Convert.ToBoolean(dr["是否批准"]))
                {

                    double 主计量单位每辅计量单位 = Convert.ToDouble(dr["主计量单位每辅计量单位"]);
                    string 用途 = dr["用途"].ToString();
                    string 产品代码 = dr["存货代码"].ToString();
                    DataRow[] drs实际购入 = dtInner实际购入.Select("产品代码='" + 产品代码 + "'");
                    if (drs实际购入.Length > 0)
                    {
                        // 先挪自由的
                        double 自由部分挪用数量 = Math.Min(Convert.ToDouble(drs实际购入[0]["订单需求数量"]), Convert.ToDouble(drs实际购入[0]["仓库可用"]));
                        sql = "select * from 库存台帐 where 产品代码='{0}' and 用途='" + __自由 + "' and 状态='合格'";
                        da = new SqlDataAdapter(string.Format(sql, 产品代码), mySystem.Parameter.conn);
                        cb = new SqlCommandBuilder(da);
                        dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            // TODO 这里要考虑同一产品的不同批号，但是日志是统一的，因为还回来批号也变了
                            dt.Rows[0]["现存数量"] = Convert.ToDouble(dt.Rows[0]["现存数量"]) - 自由部分挪用数量;
                            dt.Rows[0]["现存件数"] = Convert.ToDouble(dt.Rows[0]["现存件数"]) - 自由部分挪用数量 / 主计量单位每辅计量单位;
                            da.Update(dt);
                            DataRow 库存模板 =  dt.Rows[0];
                            sql = "select * from 库存台帐 where 产品代码='{0}' and 用途='" + 用途 + "' and 状态='合格'";
                            da = new SqlDataAdapter(string.Format(sql, 产品代码), mySystem.Parameter.conn);
                            cb = new SqlCommandBuilder(da);
                            dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count == 0)
                            {
                                DataRow kc库存dr = dt.NewRow();
                                kc库存dr.ItemArray = 库存模板.ItemArray.Clone() as object[];
                                kc库存dr["现存件数"] = 自由部分挪用数量 / 主计量单位每辅计量单位;
                                kc库存dr["现存数量"] = 自由部分挪用数量;
                                kc库存dr["用途"] = 用途;
                                kc库存dr["冻结状态"] = true;
                                kc库存dr["借用日志"] = "";
                                kc库存dr["备注"] = "";
                                kc库存dr["物资验收记录详细信息ID"] = DBNull.Value;
                                dt.Rows.Add(kc库存dr);
                            }
                            else
                            {
                                dt.Rows[0]["现存数量"] = Convert.ToDouble(dt.Rows[0]["现存数量"]) + 自由部分挪用数量;
                                dt.Rows[0]["现存件数"] = Convert.ToDouble(dt.Rows[0]["现存件数"]) + 自由部分挪用数量 / 主计量单位每辅计量单位;
                            }
                            da.Update(dt);
                        }
                            
                        
                        // 再挪借用的，要写借用日志
                        JArray ja = JArray.Parse(drs实际购入[0]["借用信息"].ToString());
                        foreach (JToken jt in ja)
                        {
                            string 被借用订单号 = jt["用途"].ToString();
                            double 被借用数量 = Convert.ToDouble(jt["数量"]);
                            sql = "select * from 库存台帐 where 产品代码='{0}' and 用途='" + 被借用订单号 + "' and 状态='合格'";
                            da = new SqlDataAdapter(string.Format(sql, 产品代码), mySystem.Parameter.conn);
                            cb = new SqlCommandBuilder(da);
                            dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dt.Rows[0]["现存数量"] = Convert.ToDouble(dt.Rows[0]["现存数量"]) - 被借用数量;
                                dt.Rows[0]["现存件数"] = Convert.ToDouble(dt.Rows[0]["现存件数"]) - 被借用数量 / 主计量单位每辅计量单位;
                                string log = "";
                                log += DateTime.Now.ToString("yyy年MM月dd日被订单“"+用途+"”借用，数量为"+被借用数量+"。\n");
                                dt.Rows[0]["借用日志"] = dt.Rows[0]["借用日志"].ToString() + log;
                                da.Update(dt);
                                DataRow 库存模板 = dt.Rows[0];
                                sql = "select * from 库存台帐 where 产品代码='{0}' and 用途='" + 用途 + "' and 状态='合格'";
                                da = new SqlDataAdapter(string.Format(sql, 产品代码), mySystem.Parameter.conn);
                                cb = new SqlCommandBuilder(da);
                                dt = new DataTable();
                                da.Fill(dt);
                                if (dt.Rows.Count == 0)
                                {
                                    DataRow kc库存dr = dt.NewRow();
                                    kc库存dr.ItemArray = 库存模板.ItemArray.Clone() as object[];
                                    kc库存dr["现存件数"] = 被借用数量 / 主计量单位每辅计量单位;
                                    kc库存dr["现存数量"] = 被借用数量;
                                    kc库存dr["用途"] = 用途;
                                    kc库存dr["冻结状态"] = true;
                                    kc库存dr["借用日志"] = "";
                                    kc库存dr["备注"] = "";
                                    kc库存dr["物资验收记录详细信息ID"] = DBNull.Value;
                                    dt.Rows.Add(kc库存dr);
                                }
                                else
                                {
                                    dt.Rows[0]["现存数量"] = Convert.ToDouble(dt.Rows[0]["现存数量"]) + 被借用数量;
                                    dt.Rows[0]["现存件数"] = Convert.ToDouble(dt.Rows[0]["现存件数"]) + 被借用数量 / 主计量单位每辅计量单位;
                                }
                                da.Update(dt);
                            }
                        }
                    }
                    
                }
            }
        }
        bool check供应商()
        {
            foreach (DataRow dr in dtInner.Rows)
            {
                if (Convert.ToBoolean(dr["是否批准"]))
                    if (dr["推荐供应商"] == DBNull.Value)
                        return false;
            }
            foreach (DataRow dr in dtInner借用订单.Rows)
            {
                if (dr["推荐供应商"] == DBNull.Value)
                    return false;
            }
            return true;
        }

        bool check数量()
        {
            foreach (DataRow dr in dtInner实际购入.Rows)
            {
                double 需求 = Convert.ToDouble(dr["订单需求数量"]);
                double 可用 = Convert.ToDouble(dr["仓库可用"]);
                double 购入 = Convert.ToDouble(dr["实际购入"]);
                if (购入 < 需求 - 可用) return false;
            }
            return true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }

            if (!checkInnerData(dataGridView3))
            {
                MessageBox.Show("请填写完整的表单信息", "提示");
                return;
            }
            if (!checkInnerData(dataGridView4))
            {
                MessageBox.Show("请填写完整的表单信息", "提示");
                return;
            }
            
            
            
            // 检查推荐供应商是否都写了
            if (!check供应商())
            {
                MessageBox.Show("供应商未填写完整！");
                return;
            }
            if (!check数量())
            {
                MessageBox.Show("实际购入数量填写有误！");
                return;
            }
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='采购批准单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
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
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }

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
            readDGVWidthFromSettingAndSet(dataGridView1);


            if (dataGridView4.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView4);
            }


            dataGridView4.Columns.Clear();
            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner借用订单.Columns)
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
                //    dataGridView4.Columns.Add(cbc);
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
                        dataGridView4.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView4.Columns.Add(ckbc);
                        break;
                }
            }

            readDGVWidthFromSettingAndSet(dataGridView4);
        }


        void dataRefresh()
        {
            // 读取库存台账，写入dtInner实际购入信息中的  仓库可用、可借数量
            refresh仓库可用数据();
            // 更新右上角的label信息
            refreshSum(combobox存货编码筛选);
            // 刷新各组件的借用信息
            refresh借用信息();
            innerBind();
        }

        void refresh借用信息()
        {
            dataGridView3.ShowCellToolTips = true;
            // 检查hashtable1是否初始化
            if (ht借用信息 == null) ht借用信息 = new Hashtable();

            // 遍历dtinner实际购入信息，以产品代码为hashtable1的key，没有该key，则为它加入一个，value为new hashtable
            foreach (DataGridViewRow dgvr in dataGridView3.Rows)
            {
                if (dgvr.Cells["产品代码"].Value == null) continue;

                string 产品代码 = dgvr.Cells["产品代码"].Value.ToString();
                if (ht借用信息.ContainsKey(产品代码)) ht借用信息.Remove(产品代码);
                ht借用信息[产品代码] = new Hashtable();

                // 从dtinner实际购入中读取历史借用数据
                JArray ja = JArray.Parse(dgvr.Cells["借用信息"].Value.ToString());
                List<String> tips = new List<string>();
                foreach (JToken jt in ja)
                {
                    string 用途 = jt["用途"].ToString();
                    double 数量 = Convert.ToDouble(jt["数量"]);
                    ((Hashtable)ht借用信息[产品代码])[用途] = 数量;
                    tips.Add("用途: " + 用途 + "\t" + "借用数量：" + 数量);
                }

                // 添加datagridvew提示
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    dgvc.ToolTipText = String.Join("\n", tips);
                }
            }
            
            
        }

        string generate采购申请批准单号()
        {
            string prefix = "PAPA";
            string yymmdd = DateTime.Now.ToString("yyMMdd");
            string sql = "select * from 采购批准单 where 采购申请批准单号 like '{0}%' order by ID";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, prefix + yymmdd), mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["采购申请批准单号"].ToString().Substring(10, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }

        string generate组件订单需求流水号()
        {
            string prefix = "PACR";
            string yymmdd = DateTime.Now.ToString("yyMMdd");
            string sql = "select * from 采购批准单详细信息 where 组件订单需求流水号 like '{0}%' order by ID";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, prefix + yymmdd), mySystem.Parameter.conn);
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

        private void 采购批准单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null && dtOuter.Rows.Count > 0)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update(dtOuter);

                }
                if (dtInner != null)
                {
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        dr.Delete();
                    }
                    daInner.Update(dtInner);
                }
                if (dtInner借用订单 != null)
                {
                    foreach (DataRow dr in dtInner借用订单.Rows)
                    {
                        dr.Delete();
                    }
                    daInner借用订单.Update(dtInner借用订单);
                }
            }
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
            if (dataGridView2.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView2);
            }
            if (dataGridView3.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView3);
            }
            if (dataGridView4.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView4);
            }
        }

        void setDGV本订单采购信息Column()
        {
            //dataGridView1.Columns["规格型号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            dataGridView1.Columns["规格型号"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //dataGridView1.Columns["规格型号"].Width = 300;

        }

        void setDGV借用订单采购信息Column()
        {
            //dataGridView4.Columns["规格型号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            dataGridView4.Columns["规格型号"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //dataGridView4.Columns["规格型号"].Width = 300;

        }

        private void btn打印_Click(object sender, EventArgs e)
        {

        }


    }
}
