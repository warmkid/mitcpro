using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Collections;

namespace mySystem.Process.Order
{
    public partial class 出库单 : BaseForm
    {

//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;

        List<String> ls操作员, ls审核员;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        Int32 _id;

        OleDbDataAdapter daOuter, daInner,da库存信息;
        OleDbCommandBuilder cbOuter, cbInner, cb库存信息;
        DataTable dtOuter, dtInner,dt未发货信息, dt库存信息;
        BindingSource bsOuter, bsInner,bs库存信息;

        CheckForm ckform;

        bool isSaved = false;

        public 出库单(MainForm mainform):base(mainform)
        {
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            InitializeComponent();
            fillPrinter();
            getPeople();
            setUseState();

            addOtherEventHandler();
            setFormState(true);
            setEnableReadOnly();
            lbl出库单号.Text = generate出库单号();
        }

        public 出库单(MainForm mainform, int id)
            : base(mainform)
        {
            isSaved = true;
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            InitializeComponent();
            fillPrinter();
            getPeople();
            setUseState();

            addOtherEventHandler();
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + dtOuter.Rows[0]["销售订单号"].ToString() + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未找到对应销售订单，请重新查询");
                return;
            }
            // 填写未发货信息

            dt未发货信息 = get未发货信息(Convert.ToInt32(dt.Rows[0]["ID"]), dtOuter.Rows[0]["销售订单号"].ToString());

            dataGridView2.DataSource = dt未发货信息;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);

            // 填写库存信息
            read库存信息(Convert.ToInt32(dt.Rows[0]["ID"]));
            // 查找库存中和这些存货代码对应的数据

            tb销售订单号.Enabled = false;
            btn查询.Enabled = false;




            setFormState();
            setEnableReadOnly();
        }

        private void addOtherEventHandler()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select 订单号 from 销售订单 where 状态='审核完成' OR 状态='已生成采购需求单'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> 销售订单号 = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                销售订单号.Add(dr["订单号"].ToString());
            }
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            acsc.AddRange(销售订单号.ToArray());
            tb销售订单号.AutoCompleteCustomSource = acsc;
            tb销售订单号.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tb销售订单号.AutoCompleteSource = AutoCompleteSource.CustomSource;


            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
          
            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;

            dataGridView3.CellEndEdit += new DataGridViewCellEventHandler(dataGridView3_CellEndEdit);
        }

        void dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView3.CurrentCell.OwningColumn.Name == "冻结状态")
                {
                    bool cur = Convert.ToBoolean(dataGridView3["冻结状态", e.RowIndex].Value);
                    if (cur)
                    {
                        string 产品代码=dataGridView3["产品代码", e.RowIndex].Value.ToString();
                        string 用途 = dataGridView3["用途", e.RowIndex].Value.ToString();
                        foreach(DataRow dr in dtInner.Select("存货代码='" + 产品代码 + "' and 原用途='" + 用途 + "'"))
                        {
                            dr.Delete();
                        }
                        daInner.Update((DataTable)bsInner.DataSource);

                        readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                        innerBind();
                    }
                }
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["出库单ID"].Visible = false;
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

        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 订单用户权限 where 步骤='出库单'", mySystem.Parameter.connOle);
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

        void setFormState(bool newForm=false)
        {
            if (newForm)
            {
                _formState = Parameter.FormState.无数据;
                return;
            }
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
            if (_formState == Parameter.FormState.无数据)
            {
                setControlFalse();
                btn查询.Enabled = true;
                tb销售订单号.ReadOnly = false;
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

        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "tb销售订单号") continue;
                if (c.Name == "btn查询") continue;
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
                if(c.Name=="tb销售订单号") continue;
                if(c.Name=="btn查询")continue;
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

        string generate出库单号()
        {
            string prefix = "XSCK";
            string yymmdd = DateTime.Now.ToString("yyyyMM");
            string sql = "select * from 出库单 where 出库单号 like '{0}%' order by ID";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, prefix + yymmdd), mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["出库单号"].ToString().Substring(10, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }


        private void btn查询_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + tb销售订单号.Text + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未找到对应销售订单，请重新查询");
                return;
            }
            // 填写未发货信息
            
            dt未发货信息 = get未发货信息(Convert.ToInt32(dt.Rows[0]["ID"]),tb销售订单号.Text);
            
            dataGridView2.DataSource = dt未发货信息;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);

            // 填写库存信息
            read库存信息(Convert.ToInt32(dt.Rows[0]["ID"]));
            // 查找库存中和这些存货代码对应的数据

            tb销售订单号.Enabled = false;
            btn查询.Enabled = false;



            readOuterData(lbl出库单号.Text);
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
            }

            daOuter.Update(dtOuter);
            readOuterData(lbl出库单号.Text);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            setFormState();
            setEnableReadOnly();
        }

        void readOuterData(string p)
        {
            daOuter = new OleDbDataAdapter("select * from 出库单 where 出库单号='" + p + "'", mySystem.Parameter.connOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("出库单");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 出库单 where ID=" + id, mySystem.Parameter.connOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("出库单");
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

        DataRow writeOuterDefault(DataRow dr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + tb销售订单号.Text + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("添加出库单信息错误，销售订单不存在");
            }
            string 客户名称 = dt.Rows[0]["客户简称"].ToString();
            dr["销售订单号"] = tb销售订单号.Text;
            dr["出库单号"] = lbl出库单号.Text;
            dr["出库日期"] = DateTime.Now;
            dr["客户名称"] = 客户名称;
            dr["业务员"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;
            dr["状态"] = "编辑中";
            return dr;
        }

        void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 出库单详细信息 where 出库单ID=" + id, mySystem.Parameter.connOle);
            dtInner = new DataTable("出库单详细信息");
            bsInner = new BindingSource();
            cbInner = new OleDbCommandBuilder(daInner);

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        private DataTable get未发货信息(int id, string dingdanhao)
        {
            // 拿到本订单下的存货代码和数量
            
            Hashtable ht存货代码2剩余数量 = new Hashtable();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double shuliang = Convert.ToDouble( dr["数量"]);
                if (!ht存货代码2剩余数量.ContainsKey(daima)) ht存货代码2剩余数量[daima] = 0;
                ht存货代码2剩余数量[daima] = Convert.ToDouble(ht存货代码2剩余数量[daima]) + shuliang;
    
            }

            // 拿到对本订单的出库单详细信息
            // 计算剩余未出库量
            da = new OleDbDataAdapter("select * from 出库单详细信息 where 用途='" + dingdanhao + "' and 状态='已出库'", mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double shuliang = Convert.ToDouble(dr["出库数量"]);
                if (ht存货代码2剩余数量.ContainsKey(daima))
                    ht存货代码2剩余数量[daima] = Convert.ToDouble(ht存货代码2剩余数量[daima]) - shuliang;

            }

            // 构建数据
            DataTable ret = new DataTable();
            ret.Columns.Add("存货代码", Type.GetType("System.String"));
            ret.Columns.Add("剩余数量", Type.GetType("System.Double"));
            foreach (string k in ht存货代码2剩余数量.Keys.OfType<String>().ToArray<String>())
            {
                DataRow dr = ret.NewRow();
                dr["存货代码"] = k;
                dr["剩余数量"] = ht存货代码2剩余数量[k];
                ret.Rows.Add(dr);
            }
            return ret;
        }

        void read库存信息(int id)
        {
            //da库存信息 = new OleDbDataAdapter("select ID,仓库名称,产品代码,产品批号,现存数量,用途,冻结状态 from 库存台帐 where 用途='" + dingdanhao + "' and 状态='合格'", conn);
            //cb库存信息 = new OleDbCommandBuilder(da库存信息);
            //dt库存信息 = new DataTable("库存台帐");
            //bs库存信息 = new BindingSource();

            //da库存信息.Fill(dt库存信息);

            //dataGridView3.DataSource = dt库存信息;

            Hashtable ht存货代码2剩余数量 = new Hashtable();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double shuliang = Convert.ToDouble(dr["数量"]);
                if (!ht存货代码2剩余数量.ContainsKey(daima)) ht存货代码2剩余数量[daima] = 0;
                ht存货代码2剩余数量[daima] = Convert.ToDouble(ht存货代码2剩余数量[daima]) + shuliang;

            }
            List<String> DaiMaS = ht存货代码2剩余数量.Keys.OfType<String>().ToList<String>();
            string 库存sql = @"select ID, 仓库名称,产品代码,产品名称,产品规格,产品批号,现存数量,用途,冻结状态 from (select * from 库存台帐 where 状态='合格') where 产品代码='{0}'";
            库存sql = string.Format(库存sql, DaiMaS[0]);
            List<String> whereS = new List<string>();
            if (DaiMaS.Count > 1) 库存sql += " or ";
            for (int i = 1; i < DaiMaS.Count; ++i)
            {
                whereS.Add("产品代码='" + DaiMaS[i] + "'");
            }
            库存sql += String.Join(" or ", whereS.ToArray());
            库存sql += " order by 产品代码";
            da库存信息 = new OleDbDataAdapter(库存sql, mySystem.Parameter.connOle);
            dt库存信息 = new DataTable("关联库存信息");
            cb库存信息 = new OleDbCommandBuilder(da库存信息);
            bs库存信息 = new BindingSource();
            da库存信息.Fill(dt库存信息);
            List<int> inv = new List<int>();
            for (int i = dt库存信息.Rows.Count - 1; i >= 0; --i)
            {
                if (Convert.ToDouble(dt库存信息.Rows[i]["现存数量"]) <= 0)
                {
                    inv.Add(i);
                }
            }
            foreach (int i in inv)
            {
                dt库存信息.Rows.RemoveAt(i);
            }
            bs库存信息.DataSource = dt库存信息;
            dataGridView3.DataSource = bs库存信息.DataSource;
            dataGridView3.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView3_DataBindingComplete);
            Utility.setDataGridViewAutoSizeMode(dataGridView3);
        }

        void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView3.Columns["ID"].Visible = false;
        }

        private void btn出库_Click(object sender, EventArgs e)
        {
            // 获取当前选中的库存信息，如果是冻结的，则retuern
            // 在dtinner里加一行
            // 最后审核时处理一下借用信息，确认数量，判断订单是否完成
            // 那边要加一个删除按钮
            int ridx = dataGridView3.CurrentCell.RowIndex;
            if (ridx < 0) return;
            if (Convert.ToBoolean(dataGridView3["冻结状态", ridx].Value)) return;
            string 代码 = dataGridView3["产品代码", ridx].Value.ToString();
            string 用途 = dataGridView3["用途", ridx].Value.ToString();
            if (dtInner.Select("存货代码='" + 代码 + "' and 原用途='" + 用途 + "'").Length > 0) return;
            DataRow dr = dtInner.NewRow();
            dr["出库单ID"] = dtOuter.Rows[0]["ID"];
            dr["出库单号"] = dtOuter.Rows[0]["出库单号"];
            dr["出库日期"] = DateTime.Now;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + tb销售订单号.Text + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("添加出库单信息错误，销售订单不存在");
            }
            string 客户名称 = dt.Rows[0]["客户简称"].ToString();
            dr["客户"] = 客户名称;
            dr["原用途"] = 用途;
            dr["用途"] = dtOuter.Rows[0]["销售订单号"];
            dr["仓库"] = dataGridView3["仓库名称", ridx].Value.ToString();
            dr["存货代码"] = dataGridView3["产品代码", ridx].Value.ToString();
            dr["存货名称"] = dataGridView3["产品名称", ridx].Value.ToString();
            dr["规格型号"] = dataGridView3["产品规格", ridx].Value.ToString();
            dr["批号"] = dataGridView3["产品批号", ridx].Value.ToString();
            dr["状态"] = "出库中";
            dr["关联的库存台帐ID"] = Convert.ToInt32(dataGridView3["ID", ridx].Value);
            dtInner.Rows.Add(dr);
        }

        private void btn确认_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
        }

        void save()
        {
            isSaved = true;
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(dtOuter.Rows[0]["出库单号"].ToString());
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            da库存信息.Update((DataTable)bs库存信息.DataSource);

            OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + tb销售订单号.Text + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            read库存信息(Convert.ToInt32(dt.Rows[0]["ID"]));

            //dataGridView2.DataSource = dt未发货信息;

        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            if (!check出库数量())
            {
                return;
            }
            //写待审核表

            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='出库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "出库单";
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

        bool check出库数量()
        {
            Hashtable ht代码2总出库数量 = new Hashtable();
            foreach (DataRow dr in dtInner.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double num = Convert.ToDouble(dr["出库数量"]);
                string yongtu = dr["原用途"].ToString();
                if (!ht代码2总出库数量.ContainsKey(daima)) ht代码2总出库数量[daima] = 0;
                ht代码2总出库数量[daima] = Convert.ToDouble(ht代码2总出库数量[daima]) + num;
                if (num > Convert.ToDouble(dt库存信息.Select("产品代码='" + daima + "' and 用途='" + yongtu + "'")[0]["现存数量"]))
                {
                    MessageBox.Show("出库数过大");
                    return false;
                }
            }
            foreach (string k in ht代码2总出库数量.Keys.OfType<String>().ToArray<String>())
            {
                if (Convert.ToDouble( ht代码2总出库数量[k]) > Convert.ToDouble(dt未发货信息.Select("存货代码='" + k + "'")[0]["剩余数量"]))
                {
                    MessageBox.Show("出库数过大");
                    return false;
                }
            }
            return true;

        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (!check出库数量())
            {
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
                foreach (DataRow dr in dtInner.Rows)
                {
                    dr["状态"] = "已出库";
                    // 减去库存中的数量
                    int kcid = Convert.ToInt32(dr["关联的库存台帐ID"]);
                    dt库存信息.Select("ID=" + kcid)[0]["现存数量"] = Convert.ToDouble(dt库存信息.Select("ID=" + kcid)[0]["现存数量"]) - Convert.ToDouble(dr["出库数量"]);
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
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='出库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            save();

            // 确认订单是否完成
            if (check订单是否完成())
            {
                MessageBox.Show("本订单已全部发货完毕，已关闭订单");
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + dtOuter.Rows[0]["销售订单号"].ToString() + "'", mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows[0]["状态"] = "已关闭";
                da.Update(dt);
            }

            // 借用后检查是否需要生产需求单


            base.CheckResult();
        }

        private bool check订单是否完成()
        {
             Hashtable ht代码2总出库数量 = new Hashtable();
            foreach (DataRow dr in dtInner.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double num = Convert.ToDouble(dr["出库数量"]);
                string yongtu = dr["原用途"].ToString();
                if (!ht代码2总出库数量.ContainsKey(daima)) ht代码2总出库数量[daima] = 0;
                ht代码2总出库数量[daima] = Convert.ToDouble(ht代码2总出库数量[daima]) + num;
            }
            foreach (DataRow dr in dt未发货信息.Rows)
            {
                string daima = dr["存货代码"].ToString();
                double num = Convert.ToDouble(dr["剩余数量"]);
                if (num - Convert.ToDouble(ht代码2总出库数量[daima]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void 出库单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null)
                {
                    if (dtOuter.Rows.Count > 0)
                    {
                        dtOuter.Rows[0].Delete();
                        daOuter.Update(dtOuter);
                    }
                }
            }
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            int ridx = dataGridView1.CurrentCell.RowIndex;
            dtInner.Rows[ridx].Delete();

            daInner.Update((DataTable)bsInner.DataSource);

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        
    }
}
