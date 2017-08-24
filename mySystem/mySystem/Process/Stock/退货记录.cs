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

namespace mySystem.Process.Stock
{
    public partial class 退货记录 : BaseForm
    {

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOute;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;

        Parameter.UserState _userState;
        Parameter.FormState _formState;

        string _code;
        int _id;
        CheckForm ckform;

        public 退货记录(MainForm mainform, string 退货编码):base(mainform)
        {
            conn = new OleDbConnection(strConnect);
            conn.Open();
            _code = 退货编码;
            InitializeComponent();
            fillPrinter();
            getPeople();
            setUseState();

            readOuterData(退货编码);
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update(dtOuter);
                readOuterData(退货编码);
                
            }
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            outerBind();
            setFormState();
            setEnableReadOnly();

        }

        

        

        //public 退货记录(MainForm mainform, int id)
        //    : base(mainform)
        //{
        //}

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fillPrinter()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                combox打印机选择.Items.Add(sPrint);
            }
            combox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 库存用户权限 where 步骤='退货记录'", conn);
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

        void setFormState(bool newForm = false)
        {
            if (newForm)
            {

                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dtOuter.Rows[0]["审核人"].ToString();
            string b = dtOuter.Rows[0]["审核结果"].ToString();
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b == "同意") _formState = Parameter.FormState.审核通过;
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
                if (c.Name == "btn查询") continue;
                if (c.Name == "tb退货编号") continue;
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
                if (c.Name == "btn查询") continue;
                if (c.Name == "tb退货编号") continue;
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

        void readOuterData(String 退货编码)
        {
            // 根据退货编码读取数据，如果没有就新建
            daOuter = new OleDbDataAdapter("select * from 退货记录 where 退货编号='" + 退货编码 + "'", conn);
            dtOuter = new DataTable("退货记录");
            cbOute = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            // 读取退货申请
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 退货申请 where 退货编号='" + _code + "'", conn);
            DataTable dt = new DataTable();
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);

            da.Fill(dt);
            DataRow src = dt.Rows[0];
            src["状态"] = "已申请";
            da.Update(dt);

            dr["退货编号"] = _code;
            dr["合同号"] = src["合同号"];
            dr["客户名称"] = src["客户名称"];
            dr["退货产品代码"] = src["退货产品代码"];
            dr["退货产品批号"] = src["退货产品批号"];
            dr["退货数量"] = src["退货数量"];
            dr["接收人"] = mySystem.Parameter.userName;
            dr["接收日期"] = DateTime.Now;
            dr["请验日期"] = DateTime.Now;
            dr["质检接收日期"] = DateTime.Now;
            dr["检验日期"] = DateTime.Now;
            dr["评审日期"] = DateTime.Now;
            dr["批准日期"] = DateTime.Now;
            dr["实施日期"] = DateTime.Now;
            dr["验证日期"] = DateTime.Now;
            
            return dr;
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

        private void btn确认_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_code);
            outerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='退货记录' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "退货记录";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = "__待审核";
            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.Show();
        }

        public override void CheckResult()
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='退货记录' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = ckform.userName;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            dtOuter.Rows[0]["评审意见"] = ckform.opinion;
            
            //String log = "===================================\n";
            //log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            //log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            //log += "审核结果为：" + (ckform.ischeckOk ? "通过" : "不通过") + "\n";
            //log += "审核意见为：" + ckform.opinion + "\n";
            //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            save();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;

            if (ckform.ischeckOk)
            {
                // 进台账
                da = new OleDbDataAdapter("select * from 退货台账 where 退货编号='" + dtOuter.Rows[0]["退货编号"] + "'", conn);
                cb = new OleDbCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                DataRow cur;
                if (dt.Rows.Count == 0)
                {
                    cur = dt.NewRow();
                }
                else
                {
                    cur = dt.Rows[0];
                }
                cur["退货编号"] = dtOuter.Rows[0]["退货编号"];
                cur["退货日期"] = dtOuter.Rows[0]["接收日期"];
                cur["合同号"] = dtOuter.Rows[0]["合同号"];
                cur["客户简称"] = dtOuter.Rows[0]["客户名称"];
                cur["退货原因"] = dtOuter.Rows[0]["退货原因"];
                cur["产品编码"] = dtOuter.Rows[0]["退货产品代码"];
                cur["退货箱数"] = 0;
                cur["退货数量"] = dtOuter.Rows[0]["退货数量"];
                cur["处理意见"] = dtOuter.Rows[0]["评审意见"];
                cur["完成日期"] = DateTime.Now;
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(cur);
                }
                da.Update(dt);
                // TOOD 更新库存信息
            }

            base.CheckResult();
        }
    }
}
