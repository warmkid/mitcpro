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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Process.Order
{
    public partial class 销售订单 : BaseForm
    {

//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;

        List<String> ls业务类型, ls销售类型, ls客户简称, ls销售部门, ls币种,ls付款条件;
        List<String> ls存货代码, ls存货名称, ls规格型号;
       
        List<double> ld数量每件;
        List<String> ls操作员, ls审核员;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        Int32 _id;

        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;

        CheckForm ckform;
        bool isFirstBind = true; 
        
        public 销售订单(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            getOtherData();
            getPeople();
            setUseState();
            setFormState(true);
            setEnableReadOnly();
            addOtherEventHandler();
        }


        public 销售订单(MainForm mainform, Int32 id)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            _id = id;

            getOtherData();
            getPeople();
            setUseState();
            readOuterData(_id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
            tb订单号.ReadOnly = true;
            btn新建.Enabled = false;
        }
        
        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 订单用户权限 where 步骤='销售订单'", mySystem.Parameter.conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();
            ls操作员 = new List<string>(Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，"));
            ls审核员 = new List<string>(Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，"));
        }


        private void getOtherData()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls业务类型 = new List<string>();
            da = new SqlDataAdapter("select * from 设置业务类型", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls业务类型.Add(dr["业务类型"].ToString());
            }
            cmb业务类型.Items.AddRange(ls业务类型.ToArray());

            ls销售类型 = new List<string>();
            da = new SqlDataAdapter("select * from 设置销售类型", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls销售类型.Add(dr["销售类型"].ToString());
            }
            cmb销售类型.Items.AddRange(ls销售类型.ToArray());

            ls客户简称 = new List<string>();
            da = new SqlDataAdapter("select * from 设置客户简称", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls客户简称.Add(dr["客户简称"].ToString());
            }
            cmb客户简称.Items.AddRange(ls客户简称.ToArray());

            ls销售部门 = new List<string>();
            da = new SqlDataAdapter("select * from 设置销售部门", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls销售部门.Add(dr["销售部门"].ToString());
            }
            cmb销售部门.Items.AddRange(ls销售部门.ToArray());

            ls币种 = new List<string>();
            da = new SqlDataAdapter("select * from 设置币种", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls币种.Add(dr["币种"].ToString());
            }
            cmb币种.Items.AddRange(ls币种.ToArray());

            ls付款条件 = new List<string>();
            da = new SqlDataAdapter("select * from 设置付款条件", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls付款条件.Add(dr["付款条件"].ToString());
            }
            cmb付款条件.Items.AddRange(ls付款条件.ToArray());
            


            ls存货代码 = new List<string>();
            ls存货名称 = new List<string>();
            ls规格型号 = new List<string>();
            ld数量每件 = new List<double>();
            da = new SqlDataAdapter("select * from 设置存货档案", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls存货代码.Add(dr["存货代码"].ToString());
                ls存货名称.Add(dr["存货名称"].ToString());
                ls规格型号.Add(dr["规格型号"].ToString());
                try
                {
                    ld数量每件.Add(Convert.ToDouble(dr["换算率"]));
                }
                catch (Exception ee)
                {
                    MessageBox.Show(dr["存货代码"].ToString() + " 的换算率读取失败，默认设为0");
                    ld数量每件.Add(0);
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
            if(_formState == Parameter.FormState.无数据)
            {
                setControlFalse();
                tb订单号.Enabled = true;
                tb订单号.ReadOnly = false;
                btn新建.Enabled = true;
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
                else if (Parameter.FormState.审核通过 == _formState)
                {
                    setControlFalse();
                    btn取消订单.Enabled = true;
                    btn修改.Enabled = true;
                }
                else
                {
                    btn取消订单.Enabled = true;
                    btn修改.Enabled = false;
                }
                    
                
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
                // 修改时的状态
                if (dtOuter.Rows[0]["审核员"].ToString().Equals("_修改中"))
                {
                    setControlFalse();
                    dataGridView1.ReadOnly = false;
                    btn提交审核.Enabled = true;
                    btn确认.Enabled = true;
                }
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
            btn新建.Enabled = false;
            btn取消订单.Enabled = false;
            btn修改.Enabled = false;
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
            btn查看日志.Enabled = true;
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
            readOuterData(tb订单号.Text);
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(_id);
            innerBind();
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["订单号"] = tb订单号.Text;
            dr["订单日期"] = dtp订单日期.Value;

            if (cmb业务类型.Text.Trim()!="" &&!cmb业务类型.Items.Contains(cmb业务类型.Text))
            {
                cmb业务类型.Items.Add(cmb业务类型.Text);
            }
            dr["业务类型"] = cmb业务类型.Text;

            if (cmb销售类型.Text.Trim() != "" && !cmb销售类型.Items.Contains(cmb销售类型.Text))
            {
                cmb销售类型.Items.Add(cmb销售类型.Text);
            }
            dr["销售类型"] = cmb销售类型.Text;

            if (cmb客户简称.Text.Trim() != "" && !cmb客户简称.Items.Contains(cmb客户简称.Text))
            {
                cmb客户简称.Items.Add(cmb客户简称.Text);
            }
            dr["客户简称"] = cmb客户简称.Text;

            if (cmb销售部门.Text.Trim() != "" && !cmb销售部门.Items.Contains(cmb销售部门.Text))
            {
                cmb销售部门.Items.Add(cmb销售部门.Text);
            }
            dr["销售部门"] = cmb销售部门.Text;

            if (cmb币种.Text.Trim() != "" && !cmb币种.Items.Contains(cmb币种.Text))
            {
                cmb币种.Items.Add(cmb币种.Text);
            }
            dr["币种"] = cmb币种.Text;

            dr["付款条件"] = cmb付款条件.Text;
            bool ok;
            double temp;
            ok = double.TryParse(tb税率.Text, out temp);
            dr["税率"] = ok ? temp : 17;
            ok = double.TryParse(tb汇率.Text, out temp);
            dr["汇率"] = ok ? temp : 1;
            dr["备注"] = tb备注.Text;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["状态"] = "编辑中";
            dr["件数合计"] = 0;
            dr["数量合计"] = 0;
            dr["拟交货日期"] = DateTime.Now;
            dr["价税总计"] = 0;
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            dr["日志"] = log;
            return dr;
        }

        void readOuterData(String code)
        {
            daOuter = new SqlDataAdapter("select * from 销售订单 where 订单号='" + code + "'", mySystem.Parameter.conn);
            dtOuter = new DataTable("销售订单");
            cbOuter = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 销售订单 where ID=" + id, mySystem.Parameter.conn);
            dtOuter = new DataTable("销售订单");
            cbOuter = new SqlCommandBuilder(daOuter);
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

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void btn新建_Click(object sender, EventArgs e)
        {
            // 先看看订单号是否已经存在
            SqlDataAdapter da = new SqlDataAdapter("select 订单号 from 销售订单", mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow[] drs = dt.Select("订单号='" + tb订单号.Text + "'");
            if (drs.Length > 0)
            {
                if (DialogResult.No == MessageBox.Show("该订单号已存在，是否读取现有数据", "提示", MessageBoxButtons.YesNo))
                {
                    // 订单已经存在，不读取历史数据
                    return;
                }
            }
            // 进入这里就表示订单存在，但是读取历史数据，或者订单不存在
            readOuterData(tb订单号.Text);
            if (dtOuter.Rows.Count == 0)
            {
                // 如果读出来是空的，这里要保存已经填过的数据
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                dtOuter.Rows[0]["审核结果"] = 0;
                daOuter.Update(dtOuter);
                readOuterData(tb订单号.Text);

            }
            outerBind();
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            setUseState();
            setFormState();
            setEnableReadOnly();

            btn新建.Enabled = false;
            tb订单号.ReadOnly = true;
        }

        void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + id, mySystem.Parameter.conn);
            dtInner = new DataTable("销售订单详细信息");
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();
            
            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
            setDGV规格型号Column();
        }

        DataRow writeInnerDefault(DataRow dr)
        {
            dr["销售订单ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["数量"] = 0;
            dr["件数"] = 0;
            dr["含税单价"] = 0;
            dr["价税合计"] = 0;
            return dr;
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                dtInner.Rows[dataGridView1.CurrentCell.RowIndex].Delete();
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
                calc合计();
            }
        }

        private void btn上移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;
        }

        private void btn下移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='销售订单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "销售订单";
                dr["对应ID"] = (int)dtOuter.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bsOuter.DataSource = dtOuter;
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);


            dtOuter.Rows[0]["状态"] = "待审核";

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            dtOuter.Rows[0]["审核员"] = "__待审核";
            dtOuter.Rows[0]["审核时间"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dtOuter.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("操作员和审核员不能是同一个人");
                return;
            }
            ckform = new mySystem.CheckForm(this);
            ckform.Show();  
        }

        public override void CheckResult()
        {
            bool is修改后 = dtOuter.Rows[0]["审核员"].Equals("_修改中");
            //获得审核信息
            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核时间"] = ckform.time;
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
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='销售订单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (ckform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + ckform.opinion;
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;


            save();

            // 更新后续可能的表单
            if (is修改后)
            {
                更新销售需求单和销售批准单();

            }
            

            base.CheckResult();
        }

        void 更新销售需求单和销售批准单()
        {
            string 订单号 = dtOuter.Rows[0]["订单号"].ToString();
            string 状态 = dtOuter.Rows[0]["状态"].ToString();
            if (状态.Equals("已生成采购需求单"))
            {
                mySystem.Process.Order.采购需求单 form = new mySystem.Process.Order.采购需求单(mainform, 订单号);
                form.更新();
                // 获取所有需求单流水号，每个流水号循环处理
                List<String> x需求单流水号 = form.get需求流水号();
                foreach (String l流水号 in x需求单流水号)
                {
                    // 查找批准单详细信息，是否有这个号
                    String sql = "select * from 采购批准单详细信息 where 组件订单需求流水号='{0}' and 用途='{1}'";
                    SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, l流水号, 订单号), mySystem.Parameter.conn);
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        // 如果有，比较区别，记录变化
                        double 原来的数量 = Convert.ToDouble(dt.Rows[0]["采购数量"]);
                        double 原来的件数 = Convert.ToDouble(dt.Rows[0]["采购件数"]);
                       
                        DataRow[] drs = form.dtInner.Select("组件订单需求流水号='x需求单流水号'");
                        double 现在的数量 = Convert.ToDouble(drs[0]["采购数量"]);
                        double 现在的件数 = Convert.ToDouble(drs[0]["采购件数"]);
                        // 更新批准单详细信息
                        dt.Rows[0]["采购数量"] = 现在的数量;
                        dt.Rows[0]["采购件数"] = 现在的件数;
                        da.Update(dt);
                        // 拿到产品代码和批准单ID
                        string 代码 = dt.Rows[0]["存货代码"].ToString();
                        int 批准单id = Convert.ToInt32(dt.Rows[0]["ID"]);
                        // 更新实际购入信息
                        sql = "select * from 采购批准单实际购入信息 where 采购批准单ID={0} and 产品代码='{1}'";
                        da = new SqlDataAdapter(String.Format(sql, 批准单id, 代码), mySystem.Parameter.conn);
                        cb = new SqlCommandBuilder(da);
                        dt = new DataTable();
                        da.Fill(dt);
                        dt.Rows[0]["订单需求数量"] = Convert.ToDouble(dt.Rows[0]["订单需求数量"]) + 现在的数量 - 原来的数量;
                        dt.Rows[0]["实际购入"] = Convert.ToDouble(dt.Rows[0]["实际购入"]) + 现在的数量 - 原来的数量;
                        da.Update(dt);
                    }
                    
                }
            }
        }

        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            
        }

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (e.Control as TextBox);
            tb.AutoCompleteCustomSource = null;
            AutoCompleteStringCollection acsc;
            if (tb == null) return;
            switch (dataGridView1.CurrentCell.OwningColumn.Name)
            {
                case "存货代码":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls存货代码.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    break;
                //case "存货名称":
                //    acsc = new AutoCompleteStringCollection();
                //    acsc.AddRange(ls存货名称.ToArray());
                //    tb.AutoCompleteCustomSource = acsc;
                //    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //    break;
                //case "规格型号":
                //    acsc = new AutoCompleteStringCollection();
                //    acsc.AddRange(ls规格型号.ToArray());
                //    tb.AutoCompleteCustomSource = acsc;
                //    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //    break;
            }
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            String curStr;
            double curDou;
            int idx;
            bool ok;
            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "存货代码":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货代码.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
                //case "存货名称":
                //    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                //    idx = ls存货名称.IndexOf(curStr);
                //    if (idx >= 0)
                //    {
                //        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                //        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                //        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                //    }
                //    else
                //    {
                //        dataGridView1["存货代码", e.RowIndex].Value = "";
                //        dataGridView1["存货名称", e.RowIndex].Value = "";
                //        dataGridView1["规格型号", e.RowIndex].Value = "";
                //    }
                //    break;
                //case "规格型号":
                //    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                //    idx = ls规格型号.IndexOf(curStr);
                //    if (idx >=0)
                //    {
                //        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                //        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                //        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                //    }
                //    else
                //    {
                //        dataGridView1["存货代码", e.RowIndex].Value = "";
                //        dataGridView1["存货名称", e.RowIndex].Value = "";
                //        dataGridView1["规格型号", e.RowIndex].Value = "";
                //    }
                //    break;
                case "数量":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    ok = double.TryParse(curStr, out curDou);
                    if(!ok) break;
                    idx = ls存货代码.IndexOf(dataGridView1["存货代码", e.RowIndex].Value.ToString());
                    if (idx >= 0)
                    {
                        dataGridView1["件数", e.RowIndex].Value = Math.Round( curDou / ld数量每件[idx],2);
                        double danjia;
                        try
                        {
                             danjia = Convert.ToDouble(dataGridView1["含税单价", e.RowIndex].Value);
                        }
                        catch
                        {
                            danjia = 0;
                        }
                        dataGridView1["价税合计", e.RowIndex].Value = Math.Round(curDou * danjia, 2);
                    }
                    calc合计();
                    break;
                case "件数":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    ok = double.TryParse(curStr, out curDou);
                    if(!ok) break;
                    idx = ls存货代码.IndexOf(dataGridView1["存货代码", e.RowIndex].Value.ToString());
                    if (idx >= 0)
                    {
                        dataGridView1["数量", e.RowIndex].Value = Math.Round( curDou * ld数量每件[idx],2);
                        double shulaing = Convert.ToDouble(dataGridView1["数量", e.RowIndex].Value);
                        double danjia;
                        try
                        {
                            danjia = Convert.ToDouble(dataGridView1["含税单价", e.RowIndex].Value);
                        }
                        catch
                        {
                            danjia = 0;
                        }
                        dataGridView1["价税合计", e.RowIndex].Value = Math.Round(shulaing * danjia, 2);
                    }
                    calc合计();
                    break;
                case "含税单价":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    ok = double.TryParse(curStr, out curDou);
                    if(!ok) break;
                    idx = ls存货代码.IndexOf(dataGridView1["存货代码", e.RowIndex].Value.ToString());
                    if (idx >= 0)
                    {
                        double shulaing;
                        try
                        {
                             shulaing = Convert.ToDouble(dataGridView1["数量", e.RowIndex].Value);
                        }
                        catch
                        {
                            shulaing = 0;
                        }
                        dataGridView1["价税合计", e.RowIndex].Value = Math.Round(curDou * shulaing, 2);
                    }
                    calc合计();
                    break;
                //case "价税合计":
                //    calc合计();
                //    break;
            }
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string name = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            int row = e.RowIndex + 1;
            MessageBox.Show(string.Format("第{0}行的{1}填写错误！",row, name));
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["销售订单ID"].Visible = false;
            dataGridView1.Columns["存货名称"].ReadOnly = true;
            dataGridView1.Columns["规格型号"].ReadOnly = true;
            dataGridView1.Columns["价税合计"].ReadOnly = true;
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        void calc合计()
        {
            double 件数合计 = 0;
            double 数量合计 = 0;
            double 价税合计合计 = 0;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                件数合计 += Convert.ToDouble(dgvr.Cells["件数"].Value);
                数量合计 += Convert.ToDouble(dgvr.Cells["数量"].Value);
                价税合计合计 += Convert.ToDouble(dgvr.Cells["价税合计"].Value);

            }
            dtOuter.Rows[0]["件数合计"] = 件数合计;
            dtOuter.Rows[0]["数量合计"] = 数量合计;
            dtOuter.Rows[0]["价税总计"] = 价税合计合计;
            lbl价税总计.DataBindings[0].ReadValue();
            lbl数量合计.DataBindings[0].ReadValue();
            lbl件数合计.DataBindings[0].ReadValue();
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            (new mySystem.Other.LogForm()).setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
        }

        private void btn取消订单_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确认取消订单吗?"))
            {
                // 采购批准单详细信息
                SqlDataAdapter da = new SqlDataAdapter("select * from 采购批准单详细信息 where 用途='" + dtOuter.Rows[0]["订单号"].ToString() + "'", mySystem.Parameter.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["用途"] = "__自由";
                }
                da.Update(dt);

                // 采购批准单借用订单详细信息
                da = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 用途='" + dtOuter.Rows[0]["订单号"].ToString() + "'", mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["用途"] = "__自由";
                }
                da.Update(dt);

                // 采购订单详细信息
                da = new SqlDataAdapter("select * from 采购订单详细信息 where 用途='" + dtOuter.Rows[0]["订单号"].ToString() + "'", mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["用途"] = "__自由";
                }
                da.Update(dt);

                // 库存台账
                da = new SqlDataAdapter("select * from 库存台帐 where 用途='" + dtOuter.Rows[0]["订单号"].ToString() + "'", mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["用途"] = "__自由";
                }
                da.Update(dt);
                dtOuter.Rows[0]["状态"] = "已取消";
                dtOuter.Rows[0]["取消人"] = mySystem.Parameter.userName;
                save();
                this.Close();
            }


        }

        void setDGV规格型号Column()
        {
            dataGridView1.Columns["规格型号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            dataGridView1.Columns["规格型号"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns["规格型号"].Width = 300;

        }

        private void 销售订单_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
        }

        private void btn打印_Click(object sender, EventArgs e)
        {

        }

        private void btn修改_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("修改意见为:" + textbox修改.Text, "确认", MessageBoxButtons.YesNo))
            {
                dtOuter.Rows[0]["审核员"] = "_修改中";
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 要求修改\n";
                log += "修改意见：" + textbox修改.Text;
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
                save();

                if (_userState == Parameter.UserState.操作员)
                {
                    btn提交审核.Enabled = true;
                }
            }
        }

    }
}
