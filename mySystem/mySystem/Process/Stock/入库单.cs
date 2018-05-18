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
    public partial class 入库单 : BaseForm
    {

        List<String> ls操作员;
        List<String> ls审核员;
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        mySystem.Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        mySystem.Parameter.FormState _formState;

        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

        int _id;
       
        CheckForm ckform;
        bool isFirstBind = true;

        public 入库单(MainForm mainform, int id):base(mainform)
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            _id = id;

            getPeople();
            setUseState();
            readOuterData(_id);
            outerBind();
            readInnerData(_id);
            innerBind();
            readFromBinding();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
            
        }

        void readInnerData(int id)
        {
            
            dtInner = new DataTable("入库单详细信息");
            bsInner = new BindingSource();
            daInner = new SqlDataAdapter("select * from 入库单详细信息 where 入库单ID=" + id, mySystem.Parameter.conn);
            cbInner = new SqlCommandBuilder(daInner);
            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        private void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.CurrentCell.OwningColumn.Name == "二维码" && dataGridView1.CurrentCell.RowIndex == dataGridView1.Rows.Count - 1)
            //{
            //    DataRow dr = dtInner.NewRow();
            //    dr["入库单ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            //    string[] str = Regex.Split(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "@");
            //    if (str.Length >= 2)
            //    {
            //        dr["产品代码"] = str[0];
            //        dr["产品批号"] = str[1];
            //    }
            //    dtInner.Rows.Add(dr);

            //}
            if (dataGridView1.CurrentCell.OwningColumn.Name == "二维码")
            {
                if (e.RowIndex >= 0)
                {
                     string[] str = Regex.Split(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "@");
                     if (str.Length >= 2)
                     {
                         dtInner.Rows[e.RowIndex]["产品代码"] = str[0];
                         dtInner.Rows[e.RowIndex]["产品批号"] = str[1];
                     }
                }


            }

        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["入库单ID"].Visible = false;

            dataGridView1.Columns["产品代码"].ReadOnly = true;
            dataGridView1.Columns["产品批号"].ReadOnly = true;

            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        void readFromBinding()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings[0].ReadValue();
                }
            }
        }
        public static string create入库单号()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 1 入库单号 from 入库单 order by ID DESC", mySystem.Parameter.conn);
            DataTable dt = new DataTable("入库单");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            else
            {
                string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
                string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
                if (Int32.Parse(yearInCode) == yearNow)
                {
                    codeNow = Int32.Parse(NOInCode) + 1;
                }
                else
                {
                    codeNow = 1;
                }
                return yearNow.ToString() + codeNow.ToString("D4");
            }
        }

        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='入库单'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();
            if (dt.Rows.Count > 0)
            {
                string[] s = Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        ls操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        ls审核员.Add(s1[i]);
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

        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 入库单 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("入库单");
            bsOuter = new BindingSource();

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
            btn日志.Enabled = true;
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            
            save();
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            if (_userState == Parameter.UserState.操作员)
                btn提交审核.Enabled = true;
        }

        bool checkBeforeSave()
        {
            String sql = @"select sum(数量) from 入库单详细信息 where 入库单ID={0}";
            SqlCommand comm = new SqlCommand(String.Format(sql,_id),mySystem.Parameter.conn);
            object o = comm.ExecuteScalar();
            double d;
            double eps = 1e-4;
            if (o == DBNull.Value) d = 0;
            else d = Convert.ToDouble(o);

            if (!(Math.Abs( d - Convert.ToDouble(tb数量.Text)) < eps))
            {
                MessageBox.Show("入库数量不匹配！","错误");
                return false;
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

            if (!checkInnerData(dataGridView1))
            {
                MessageBox.Show("请填写完整的表单信息", "提示");
                return;
            }
            
            
            // 判断，二维码中的数量应该和总量一致
            if (!checkBeforeSave())
                return;
            
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='入库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "入库单";
                dr["对应ID"] = (int)dtOuter.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bsOuter.DataSource = dtOuter;
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);


            //dtOuter.Rows[0]["状态"] = "待审核";

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            dtOuter.Rows[0]["审核员"] = "__待审核";
            dtOuter.Rows[0]["审核日期"] = DateTime.Now;


            save();
            //空间都不能点
            setControlFalse();
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.ShowDialog();
        }

        public override void CheckResult()
        {
            //获得审核信息
            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核日期"] = ckform.time;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            //写日志
            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";

            log += "审核结果：" + (ckform.ischeckOk == true ? "通过\n" : "不通过\n");

            log += "审核意见：" + ckform.opinion;

            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;

            if (ckform.ischeckOk)//审核通过
            {
               // 生产请验单
                create请验单();
                // 生成取样记录
                create取样记录();
               // 入库
                int 库存ID = insert库存台帐();
                // 生成检验记录
                create检验记录();
                // 拿到库存ID，向两张二维码表插入数据
                save二维码信息(dtInner, 库存ID);
                MessageBox.Show("已自动生产物资请验单，取样记录，检验记录；插入库存台账！","提示");
            }
            else
            {
            }

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='入库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();
            base.CheckResult();
        }

        void save二维码信息(DataTable dt, int id)
        {
            SqlDataAdapter daLishi, daXinxi;
            SqlCommandBuilder cbLishi, cbXinxi;
            DataTable dtLishi, dtXinxi;
            daLishi = new SqlDataAdapter("select * from 二维码历史记录 where 0=1",mySystem.Parameter.conn);
            daXinxi = new SqlDataAdapter("select * from 二维码信息 where 0=1", mySystem.Parameter.conn);
            cbLishi = new SqlCommandBuilder(daLishi);
            cbXinxi = new SqlCommandBuilder(daXinxi);
            dtLishi = new DataTable("二维码历史记录");
            dtXinxi = new DataTable("二维码信息");
            daLishi.Fill(dtLishi);
            daXinxi.Fill(dtXinxi);
            foreach (DataRow dr in dt.Rows)
            {
                string erweima = dr["二维码"].ToString();
                if (erweima.Trim() == "") continue;
                double shulaing = Convert.ToDouble(dr["数量"]);
                DataRow ndr;
                ndr = dtLishi.NewRow();
                ndr["时间"] = DateTime.Now;
                ndr["二维码"] = erweima;
                ndr["操作"] = "入库";
                dtLishi.Rows.Add(ndr);

                daXinxi = new SqlDataAdapter("select * from 二维码信息 where 二维码='"+erweima+"'", mySystem.Parameter.conn);
                cbXinxi = new SqlCommandBuilder(daXinxi);
                dtXinxi = new DataTable("二维码信息");
                daXinxi.Fill(dtXinxi);
                if (dtXinxi.Rows.Count <= 0)
                {
                    ndr = dtXinxi.NewRow();
                }
                else
                {
                    ndr = dtXinxi.Rows[0];
                }
                ndr["二维码"] = erweima;
                ndr["库存ID"] = id;
                ndr["数量"] = shulaing;
                if (dtXinxi.Rows.Count <= 0)
                {
                    dtXinxi.Rows.Add(ndr);
                }
                
            }
            daLishi.Update(dtLishi);
            daXinxi.Update(dtXinxi);
        }

        public int insert库存台帐()
        {
            ////SqlDataAdapter da = new SqlDataAdapter("select * from 库存台帐 where ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            //SqlDataAdapter da = new SqlDataAdapter("select * from 库存台帐 where 1=2", mySystem.Parameter.conn);
            //DataTable dt = new DataTable("库存台帐");
            //SqlCommandBuilder cb = new SqlCommandBuilder(da);
            //BindingSource bs = new BindingSource();
            //da.Fill(dt);

            //// 填写值
            //// Outer
            //DataRow ndr = dt.NewRow();
            //DataRow dr = dtOuter.Rows[0];
            //ndr["仓库名称"] = dr["仓库名"];
            ////ndr["供应商代码"] = dr["供应商"];
            //ndr["供应商名称"] = dr["供应商"];
            //ndr["产品代码"] = dr["产品代码"];
            //ndr["产品名称"] = dr["产品名称"];
            //ndr["产品规格"] = dr["规格型号"];
            //ndr["产品批号"] = dr["批号"];

            //ndr["现存数量"] = dr["数量"];
            //ndr["主计量单位"] = dr["主计量单位"];

            //ndr["用途"] = dr["采购订单号"];
            //ndr["状态"] = "待验";
            //ndr["借用日志"] = "";
            //ndr["冻结状态"] = true;
            //ndr["物资验收记录详细信息ID"] = dr["关联的验收记录ID"];
            //ndr["换算率"] = dr["换算率"];
            //ndr["现存件数"] = Convert.ToDouble(ndr["现存数量"]) / Convert.ToDouble(ndr["换算率"]);
            //ndr["实盘数量"] = ndr["现存件数"];

            //ndr["是否盘点"] = false;
            //ndr["有效期"] = DateTime.Now;
            //ndr["入库单号"] = dr["入库单号"];
            //dt.Rows.Add(ndr);

            //da.Update(dt);

            //SqlCommand comm = new SqlCommand();
            //comm.Connection = mySystem.Parameter.conn;
            //comm.CommandText = "select @@identity";

            //Int32 idd1 = (Int32)comm.ExecuteScalar();
            //MessageBox.Show("已加入库存台账！");
            //return idd1;

            //参照Utility中写法插入数据库，并增加select SCOPE_IDENTITY()
            List<string> insertCols =new List<string>() { "仓库名称", "供应商代码", "供应商名称", "产品代码", "产品名称", "产品规格", "产品批号", "现存数量", "主计量单位", "用途", "状态", "借用日志", "冻结状态", "物资验收记录详细信息ID", "换算率", "现存件数", "实盘数量", "是否盘点", "有效期", "入库单号" };
            List<Object> insertVals = new List<object>() { dtOuter.Rows[0]["仓库名"], dtOuter.Rows[0]["供应商"],dtOuter.Rows[0]["供应商"], dtOuter.Rows[0]["产品代码"], dtOuter.Rows[0]["产品名称"], dtOuter.Rows[0]["规格型号"], dtOuter.Rows[0]["批号"], dtOuter.Rows[0]["数量"], dtOuter.Rows[0]["主计量单位"], dtOuter.Rows[0]["采购订单号"], 
                 "待验", "", true, dtOuter.Rows[0]["关联的验收记录ID"], dtOuter.Rows[0]["换算率"], Convert.ToDouble(dtOuter.Rows[0]["数量"]) / Convert.ToDouble(dtOuter.Rows[0]["换算率"]), Convert.ToDouble(dtOuter.Rows[0]["数量"]) / Convert.ToDouble(dtOuter.Rows[0]["换算率"]), 
                false, DateTime.Now, dtOuter.Rows[0]["入库单号"] };
            
            String cols = Utility.joinList(insertCols, ",");
            List<String> temp = new List<string>();
            foreach (String s in insertCols)
            {
                temp.Add("@" + s);
            }
            String vals = Utility.joinList(temp, ",");
            String strSQL = String.Format("INSERT INTO {0} ({1}) VALUES ({2})select IDENT_CURRENT('库存台帐')", "库存台帐", cols, vals);
            SqlCommand cmd = new SqlCommand(strSQL, mySystem.Parameter.conn);
            for (int i = 0; i < insertCols.Count; ++i)
            {
                String c = insertCols[i];
                Object v = insertVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            int n = Convert .ToInt32 (cmd.ExecuteScalar ());

            return n;
        }


        public void create请验单()
        {
            string haoma = create请验编号();
            SqlDataAdapter da = new SqlDataAdapter("select * from 物资请验单 where 请验编号='" + haoma + "'", mySystem.Parameter.conn);
            DataTable dt = new DataTable("物资请验单");
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow ndr = dt.NewRow();
            DataRow dr = dtOuter.Rows[0];
            ndr["供应商名称"] = dr["供应商"];
            ndr["请验时间"] = DateTime.Now;
            ndr["审核时间"] = DateTime.Now;
            ndr["请验人"] = mySystem.Parameter.userName;
            ndr["请验编号"] = haoma;
            ndr["物资验收记录ID"] = dr["关联的验收记录ID"];
            ndr["审核结果"] = false;//默认值
            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";

            ndr["日志"] = log;
            dt.Rows.Add(ndr);

            da.Update(dt);
            da = new SqlDataAdapter("select * from 物资请验单 where 请验编号='" + haoma + "'", mySystem.Parameter.conn);

            dt = new DataTable("物资请验单");
            da.Fill(dt);
            // Inner
            da = new SqlDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + dt.Rows[0]["ID"], mySystem.Parameter.conn);
            DataTable dtMore = new DataTable("物资请验单详细信息");
            cb = new SqlCommandBuilder(da);
            da.Fill(dtMore);

            ndr = dtMore.NewRow();
            // 注意ID的值
            ndr["物资请验单ID"] = dt.Rows[0]["ID"];
            ndr["产品名称"] = dr["产品名称"];
            ndr["产品代码"] = dr["产品代码"];
            ndr["包装规格"] = dr["规格型号"];
            ndr["本厂批号"] = dr["批号"];
            ndr["单位"] = dr["主计量单位"];
            ndr["数量"] = dr["数量"];
            ndr["有无厂家检验报告"] = "无";

            dtMore.Rows.Add(ndr);

            da.Update(dtMore);
            
        }

        private string create请验编号()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 1 请验编号 from 物资请验单 order by ID DESC", mySystem.Parameter.conn);
            DataTable dt = new DataTable("物资请验单");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
            string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
            if (Int32.Parse(yearInCode) == yearNow)
            {
                codeNow = Int32.Parse(NOInCode) + 1;
            }
            else
            {
                codeNow = 1;
            }
            return yearNow.ToString() + codeNow.ToString("D4");

        }


        private string create取样单号()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 1 取样单号 from 取样记录 order by ID DESC", mySystem.Parameter.conn);
            DataTable dt = new DataTable("取样记录");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
            string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
            if (Int32.Parse(yearInCode) == yearNow)
            {
                codeNow = Int32.Parse(NOInCode) + 1;
            }
            else
            {
                codeNow = 1;
            }
            return yearNow.ToString() + codeNow.ToString("D4");

        }

        public void create取样记录()
        {
            string haoma = create取样单号();
            SqlDataAdapter da = new SqlDataAdapter("select * from 取样记录 where 取样单号='" + haoma + "'", mySystem.Parameter.conn);
            DataTable dt = new DataTable("取样记录");
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow ndr = dt.NewRow();
            DataRow dr = dtOuter.Rows[0];
            ndr["物资验收记录ID"] = dr["关联的验收记录ID"];
            ndr["审核时间"] = DateTime.Now;
            ndr["取样单号"] = haoma;
            //dr["供应商代码"] = dtOuter.Rows[0]["供应商代码"].ToString();
            ndr["供应商名称"] = dr["供应商"];
            ndr["审核结果"] = false;//默认值
            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";

            ndr["日志"] = log;


            dt.Rows.Add(ndr);

            da.Update(dt);
            da = new SqlDataAdapter("select * from 取样记录 where 取样单号='" + haoma + "'", mySystem.Parameter.conn);

            dt = new DataTable("取样记录");
            da.Fill(dt);
            // Inner
            da = new SqlDataAdapter("select * from 取样记录详细信息 where 取样记录ID=" + dt.Rows[0]["ID"], mySystem.Parameter.conn);
            DataTable dtMore = new DataTable("取样记录详细信息");
            cb = new SqlCommandBuilder(da);
            da.Fill(dtMore);


            ndr = dtMore.NewRow();
            // 注意ID的值
            ndr["取样记录ID"] = dt.Rows[0]["ID"];
            ndr["物料名称"] = dr["产品名称"];
            ndr["物料代码"] = dr["产品代码"];
            ndr["包装规格"] = dr["规格型号"];
            ndr["本厂批号"] = dr["批号"];
            ndr["单位"] = dr["主计量单位"];
            ndr["数量"] = dr["数量"];
            ndr["日期"] = DateTime.Now;
            ndr["取样人"] = mySystem.Parameter.userName;
            dtMore.Rows.Add(ndr);

            da.Update(dtMore);
        }


        private string create检验单号()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 1 检验单号 from 检验记录 order by ID DESC", mySystem.Parameter.conn);
            DataTable dt = new DataTable("检验记录");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
            string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
            if (Int32.Parse(yearInCode) == yearNow)
            {
                codeNow = Int32.Parse(NOInCode) + 1;
            }
            else
            {
                codeNow = 1;
            }
            return yearNow.ToString() + codeNow.ToString("D4");

        }

        public void create检验记录()
        {
            string haoma = create检验单号();
            SqlDataAdapter da = new SqlDataAdapter("select * from 检验记录 where 检验单号='" + haoma + "'", mySystem.Parameter.conn);
            DataTable dt = new DataTable("检验记录");
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow ndr = dt.NewRow();
            DataRow dr = dtOuter.Rows[0];
            ndr["物资验收记录ID"] = dr["关联的验收记录ID"];
            ndr["检验日期"] = DateTime.Now;
            ndr["检验单号"] = haoma;
            //dr["供应商代码"] = dtOuter.Rows[0]["供应商代码"].ToString();
            ndr["供应商"] = dr["供应商"];
            ndr["产品代码"] = dr["产品代码"];
            ndr["产品名称"] = dr["产品名称"];
            ndr["规格型号"] = dr["规格型号"];
            ndr["批号"] = dr["批号"];
            ndr["主计量单位"] = dr["主计量单位"];
            ndr["数量"] = dr["数量"];
            ndr["产品代码"] = dr["产品代码"];
            ndr["检验人"] = mySystem.Parameter.userName;
            ndr["审核日期"] = DateTime.Now;
            ndr["检验结论"] = "合格";
            ndr["入库单号"] = dr["入库单号"];
            ndr["采购订单号"] = dr["采购订单号"];
            ndr["审核结果"] = false;//默认值
            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";

            ndr["日志"] = log;



            dt.Rows.Add(ndr);

            da.Update(dt);
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr["入库单ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dtInner.Rows.Add(dr);
        }

        private void btn生成二维码_Click(object sender, EventArgs e)
        {
            mySystem.Other.二维码打印 form = mySystem.Other.二维码打印.create(dtOuter.Rows[0]["产品代码"].ToString(),
                dtOuter.Rows[0]["批号"].ToString());
            form.Show();
        }

        private void 入库单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView1);
        }

        private void btn日志_Click(object sender, EventArgs e)
        {
            try
            {
                (new mySystem.Other.LogForm()).setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
            }
        }

    }
}
