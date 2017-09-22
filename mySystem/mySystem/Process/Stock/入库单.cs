using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

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

        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOuter ;
        BindingSource bsOuter;
        DataTable dtOuter;

        int _id;
       
        CheckForm ckform;

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
            readFromBinding();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
            
        }

        private void addOtherEventHandler()
        {
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
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 入库单号 from 入库单 order by ID DESC", mySystem.Parameter.connOle);
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
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 库存用户权限 where 步骤='入库单'", mySystem.Parameter.connOle);
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
            daOuter = new OleDbDataAdapter("select * from 入库单 where ID=" + id, mySystem.Parameter.connOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
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
            if (_userState == Parameter.UserState.操作员)
                btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='入库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
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
            //string log = "\n=====================================\n";
            //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            dtOuter.Rows[0]["审核员"] = "__待审核";
            dtOuter.Rows[0]["审核日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.Show();
        }

        public override void CheckResult()
        {
            //获得审核信息
            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核日期"] = ckform.time;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;

            OleDbDataAdapter da;
            DataTable dt;
            OleDbCommandBuilder cb;

            if (ckform.ischeckOk)//审核通过
            {
               // 生产请验单
                create请验单();
                // 生成取样记录
                create取样记录();
               // 入库
                insert库存台帐();
            }
            else
            {
            }

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='入库单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();
            base.CheckResult();
        }



        public void insert库存台帐()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 库存台帐 where 0=1" + dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            DataTable dt = new DataTable("库存台帐");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow ndr = dt.NewRow();
            DataRow dr = dtOuter.Rows[0];
            ndr["仓库名称"] = dr["仓库名"];
            //ndr["供应商代码"] = dr["供应商"];
            ndr["供应商名称"] = dr["供应商"];
            ndr["产品代码"] = dr["产品代码"];
            ndr["产品名称"] = dr["产品名称"];
            ndr["产品规格"] = dr["规格型号"];
            ndr["产品批号"] = dr["批号"];

            ndr["现存数量"] = dr["数量"];
            ndr["主计量单位"] = dr["主计量单位"];

            ndr["用途"] = dr["采购订单号"];
            ndr["状态"] = "待验";
            ndr["借用日志"] = "";
            ndr["冻结状态"] = true;
            ndr["物资验收记录详细信息ID"] = dr["关联的验收记录ID"];
            ndr["换算率"] = dr["换算率"];
            ndr["现存件数"] = Convert.ToDouble(ndr["现存数量"]) / Convert.ToDouble(ndr["换算率"]);
            ndr["实盘数量"] = ndr["现存件数"];

            ndr["是否盘点"] = false;
            ndr["有效期"] = DateTime.Now;
            dt.Rows.Add(ndr);



            da.Update(dt);

            MessageBox.Show("已加入库存台账！");
        }


        public void create请验单()
        {
            string haoma = create请验编号();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 物资请验单 where 请验编号='" + haoma + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable("物资请验单");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
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
            dt.Rows.Add(ndr);

            da.Update(dt);
            da = new OleDbDataAdapter("select * from 物资请验单 where 请验编号='" + haoma + "'", mySystem.Parameter.connOle);

            dt = new DataTable("物资请验单");
            da.Fill(dt);
            // Inner
            da = new OleDbDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + dt.Rows[0]["ID"], mySystem.Parameter.connOle);
            DataTable dtMore = new DataTable("物资请验单详细信息");
            cb = new OleDbCommandBuilder(da);
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

            dtMore.Rows.Add(ndr);

            da.Update(dtMore);
            MessageBox.Show("已自动生产物资请验单！");
        }

        private string create请验编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 请验编号 from 物资请验单 order by ID DESC", mySystem.Parameter.connOle);
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
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 取样单号 from 取样记录 order by ID DESC", mySystem.Parameter.connOle);
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
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 取样记录 where 取样单号='" + haoma + "'", mySystem.Parameter.connOle);
            DataTable dt = new DataTable("取样记录");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
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
            dt.Rows.Add(ndr);

            da.Update(dt);
            da = new OleDbDataAdapter("select * from 取样记录 where 取样单号='" + haoma + "'", mySystem.Parameter.connOle);

            dt = new DataTable("取样记录");
            da.Fill(dt);
            // Inner
            da = new OleDbDataAdapter("select * from 取样记录详细信息 where 取样记录ID=" + dt.Rows[0]["ID"], mySystem.Parameter.connOle);
            DataTable dtMore = new DataTable("取样记录详细信息");
            cb = new OleDbCommandBuilder(da);
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
            MessageBox.Show("已自动生产取样记录！");
        }

    }
}
