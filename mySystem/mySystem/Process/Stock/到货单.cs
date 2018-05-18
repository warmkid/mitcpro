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

namespace mySystem.Process.Stock
{
    public partial class 到货单 : BaseForm
    {
        bool isSaved;

        List<String> ls业务类型, ls币种;
        List<String> ls存货代码, ls存货名称, ls规格型号, ls采购订单合同号, ls主计量单位, ls供应商;

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

        public 到货单(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            getOtherData();
            getPeople();
            setUseState();
            setFormState(true);
            setEnableReadOnly();
            addOtherEventHandler();
        }

        public 到货单(MainForm mainform, Int32 id)
            : base(mainform)
        {
            isSaved = true;
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
            tb单据号.ReadOnly = true;
            btn新建.Enabled = false;
        }


        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='到货单'", mySystem.Parameter.conn);
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
            if (cmb业务类型.Items.Count >= 1)
            {
                cmb业务类型.SelectedIndex = 0;
            }

           

            

           

            ls币种 = new List<string>();
            da = new SqlDataAdapter("select * from 设置币种", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls币种.Add(dr["币种"].ToString());
            }
            cmb币种.Items.AddRange(ls币种.ToArray());
            if (cmb币种.Items.Count >= 1)
            {
                cmb币种.SelectedIndex = 0;
            }
            



            ls存货代码 = new List<string>();
            ls存货名称 = new List<string>();
            ls规格型号 = new List<string>();
            ld数量每件 = new List<double>();
            ls主计量单位 = new List<string>();
            da = new SqlDataAdapter("select * from 设置存货档案", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls存货代码.Add(dr["存货代码"].ToString());
                ls存货名称.Add(dr["存货名称"].ToString());
                ls规格型号.Add(dr["规格型号"].ToString());
                ls主计量单位.Add(dr["主计量单位名称"].ToString());
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

            ls采购订单合同号 = new List<string>();
            da = new SqlDataAdapter("select * from 采购订单 where 状态='审核完成'", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls采购订单合同号.Add(dr["采购合同号"].ToString());
            }

            ls供应商 = new List<string>();
            da = new SqlDataAdapter("select * from 设置供应商信息", mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls供应商.Add(dr["供应商名称"].ToString());
            }
            AutoCompleteStringCollection  acsc = new AutoCompleteStringCollection();
            acsc.AddRange(ls供应商.ToArray());
            tb供应商.AutoCompleteCustomSource = acsc;
            tb供应商.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tb供应商.AutoCompleteSource = AutoCompleteSource.CustomSource;
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

        void setFormState(bool newForm = false)
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
            if (_formState == Parameter.FormState.无数据)
            {
                setControlFalse();
                btn新建.Enabled = true;
                tb单据号.ReadOnly = false;
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
                else 
                {
                    setControlFalse();
                    
                }
                


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
            btn新建.Enabled = false;
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
            btn日志.Enabled = true;
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
            readOuterData(tb单据号.Text);
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            innerBind();
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["业务员"] = mySystem.Parameter.userName;
            dr["单据号"] = tb单据号.Text;
            dr["日期"] = DateTime.Now;
            dr["审核日期"] = DateTime.Now;
            dr["汇率"] = 1;
            if (cmb币种.Items.Count > 0) dr["币种"] = cmb币种.Items[0].ToString();
            if (cmb业务类型.Items.Count > 0) dr["业务类型"] = cmb业务类型.Items[0].ToString();
            dr["税率"] = 17;
            dr["备注"] = "无";
            dr["部门"] = "采购部";
            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";

            dr["日志"] = log;
            return dr;
        }

        void readOuterData(String code)
        {
            daOuter = new SqlDataAdapter("select * from 到货单 where 单据号='" + code + "'", mySystem.Parameter.conn);
            dtOuter = new DataTable("到货单");
            cbOuter = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 到货单 where ID=" + id, mySystem.Parameter.conn);
            dtOuter = new DataTable("到货单");
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

        private void btn新建_Click(object sender, EventArgs e)
        {
            if (tb单据号.Text.Trim() == "")
            {
                MessageBox.Show("请填写单据号");
                return;
            }
            // 先看看订单号是否已经存在
            SqlDataAdapter da = new SqlDataAdapter("select 单据号 from 到货单", mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow[] drs = dt.Select("单据号='" + tb单据号.Text + "'");
            //if (drs.Length > 0)
            //{
            //    if (DialogResult.No == MessageBox.Show("该单据号已存在，是否读取现有数据", "提示", MessageBoxButtons.YesNo))
            //    {
            //        // 订单已经存在，不读取历史数据
            //        return;
            //    }
            //}
            // 进入这里就表示订单存在，但是读取历史数据，或者订单不存在
            readOuterData(tb单据号.Text);
            if (dtOuter.Rows.Count == 0)
            {
                // 如果读出来是空的，这里要保存已经填过的数据
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                dtOuter.Rows[0]["审核结果"] = 0;
                daOuter.Update(dtOuter);
                readOuterData(tb单据号.Text);

            }
            outerBind();
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            setUseState();
            setFormState();
            setEnableReadOnly();

            btn新建.Enabled = false;
            tb单据号.ReadOnly = true;
        }

        void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 到货单详细信息 where 到货单ID=" + id, mySystem.Parameter.conn);
            dtInner = new DataTable("到货单详细信息");
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        DataRow writeInnerDefault(DataRow dr)
        {
            dr["到货单ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["数量"] = 0;
            dr["原币含税单价"] = 0;
            dr["原币价税合计"] = 0;
            dr["采购订单号"] = "__自由";
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
            
            
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='到货单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "到货单";
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
            if (mySystem.Parameter.userName == dtOuter.Rows[0]["业务员"].ToString())
            {
                MessageBox.Show("业务员和审核员不能是同一个人");
                return;
            }
            ckform = new mySystem.CheckForm(this);
            ckform.ShowDialog();
        }

        public override void CheckResult()
        {
            //获得审核信息
            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核日期"] = ckform.time;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;

            //if (ckform.ischeckOk)//审核通过
            //{
            //    dtOuter.Rows[0]["状态"] = "审核完成";
            //}
            //else
            //{
            //    dtOuter.Rows[0]["状态"] = "编辑中";//未审核，草稿
            //}

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='到货单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
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

            // TODO 自动生成验收记录
            if (ckform.ischeckOk)
            {
                SqlDataAdapter da;
                SqlCommandBuilder cb;
                DataTable dt;
                string bianhao = 物资验收记录.create验收记录编号();
                da = new SqlDataAdapter("select * from 物资验收记录 where 验收记录编号='" + bianhao + "'", mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable("物资验收记录");
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dr["验收记录编号"] = bianhao;
                dr["供应商名称"] = dtOuter.Rows[0]["供应商"];
                dr["接收时间"] = DateTime.Now;
                dr["验收人"] = mySystem.Parameter.userName;
                dr["请验人"] = mySystem.Parameter.userName;
                dr["厂家随附检验报告"] = "无";
                dr["是否有样品"] = "无";
                dr["无样品理由"] = "无";
                dr["审核时间"] = DateTime.Now;
                dr["请验时间"] = DateTime.Now;
                dr["日志"] = "\n=====================================\n" + DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";;
                dr["审核结果"] = false;//默认值
                dt.Rows.Add(dr);
                da.Update(dt);

                da = new SqlDataAdapter("select * from 物资验收记录 where 验收记录编号='" + bianhao + "'", mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);

                create物资验收记录内表(Convert.ToInt32( dt.Rows[0]["ID"]));
            }
            base.CheckResult();
        }


        void create物资验收记录内表(int id)
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;
            da = new SqlDataAdapter("select * from 物资验收记录详细信息 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable("物资验收记录详细信息");
            da.Fill(dt);
            foreach (DataRow tdr in dtInner.Rows)
            {
                DataRow ndr = dt.NewRow();
                ndr["物资验收记录ID"] = id;
                ndr["物料名称"] = tdr["产品名称"];
                ndr["物料代码"] = tdr["产品代码"];
                ndr["规格型号"] = tdr["规格型号"];
                ndr["单位"] = tdr["主计量单位"];
                ndr["数量（主计量）"] = tdr["数量"];
                SqlDataAdapter daT = new SqlDataAdapter("select 换算率 from 设置存货档案 where 存货代码='" + tdr["产品代码"] + "'", mySystem.Parameter.conn);
                DataTable dtT = new DataTable();
                daT.Fill(dtT);
                if (dtT.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到代码为：" + tdr["产品代码"] + "的产品");
                    continue;
                }
                ndr["换算率"] = dtT.Rows[0]["换算率"];
                ndr["数量（辅计量）"] = Math.Round(Convert.ToDouble(tdr["数量"]) / Convert.ToDouble(dtT.Rows[0]["换算率"]), 2);
                ndr["厂家COA"] = "齐全";
                ndr["厂家样品"] = "有";
                ndr["外包装验收情况"] = "合格";
                ndr["拒收原因"] = "无";
                ndr["拒收数量"] = 0;
                ndr["采购订单号"] = tdr["采购订单号"];
                dt.Rows.Add(ndr);
            }
            da.Update(dt);
            MessageBox.Show("已自动生成物资验收记录");
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
                case "产品代码":
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
                case "采购订单号":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls采购订单合同号.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    break;
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
                case "产品代码":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货代码.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["产品代码", e.RowIndex].Value = ls存货代码[idx];
                        dataGridView1["产品名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                        dataGridView1["主计量单位", e.RowIndex].Value = ls主计量单位[idx];
                    }
                    else
                    {
                        dataGridView1["产品代码", e.RowIndex].Value = "";
                        dataGridView1["产品名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                        dataGridView1["主计量单位", e.RowIndex].Value = "";
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
                case "原币含税单价":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    ok = double.TryParse(curStr, out curDou);
                    if (!ok) break;
                    //idx = ls存货代码.IndexOf(dataGridView1["存货代码", e.RowIndex].Value.ToString());
                    //if (idx >= 0)
                    //{
                    double 数量 = Convert.ToDouble(dataGridView1["数量", e.RowIndex].Value);
                    dataGridView1["原币价税合计", e.RowIndex].Value = Math.Round(curDou * 数量, 2);
                    //}
                    //calc合计();
                    break;
                //case "件数":
                //    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                //    ok = double.TryParse(curStr, out curDou);
                //    if (!ok) break;
                //    idx = ls存货代码.IndexOf(dataGridView1["存货代码", e.RowIndex].Value.ToString());
                //    if (idx >= 0)
                //    {
                //        dataGridView1["数量", e.RowIndex].Value = Math.Round(curDou * ld数量每件[idx], 2);
                //    }
                //    calc合计();
                //    break;
                //case "价税合计":
                //    calc合计();
                //    break;
            }
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string name = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            int row = e.RowIndex + 1;
            MessageBox.Show(string.Format("第{0}行的{1}填写错误！", row, name));
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["到货单ID"].Visible = false;
            dataGridView1.Columns["产品名称"].ReadOnly = true;
            dataGridView1.Columns["规格型号"].ReadOnly = true;
            dataGridView1.Columns["主计量单位"].ReadOnly = true;
            dataGridView1.Columns["原币价税合计"].ReadOnly = true;

            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
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

        private void btn打印_Click(object sender, EventArgs e)
        {

        }

        private void 到货单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null && dtOuter.Rows.Count > 0)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update(dtOuter);
                }
            }

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
