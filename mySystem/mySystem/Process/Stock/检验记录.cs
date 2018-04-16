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
    public partial class 检验记录 : BaseForm
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

        SqlDataAdapter daOuter;
        SqlCommandBuilder cbOuter;
        BindingSource bsOuter;
        DataTable dtOuter;

        int _id;
        string fname;

        CheckForm ckform;

        public 检验记录(MainForm mainform, int id)
            : base(mainform)
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
        public static string create检验单号()
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
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='检验记录'", mySystem.Parameter.conn);
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
            daOuter = new SqlDataAdapter("select * from 检验记录 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("检验记录");
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
            btn查看.Enabled = true;
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
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='检验记录' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "检验记录";
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

            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;

            if (ckform.ischeckOk)//审核通过
            {

                if (dtOuter.Rows[0]["检验结论"].ToString() == "合格")
                {
                    // 修改库存台账
                    da = new SqlDataAdapter("select * from 库存台帐 where 入库单号='" + dtOuter.Rows[0]["入库单号"] + "'", mySystem.Parameter.conn);
                    cb = new SqlCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[0]["状态"] = "合格";
                        da.Update(dt);
                    }
                }
                else
                {
                    // 修改库存台账
                    da = new SqlDataAdapter("select * from 库存台帐 where 入库单号='" + dtOuter.Rows[0]["入库单号"] + "'", mySystem.Parameter.conn);
                    cb = new SqlCommandBuilder(da);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[0]["状态"] = "不合格";
                        da.Update(dt);
                    }
                    // 生成退货申请

                }
            }
            else
            {
            }

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='检验记录' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
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

        private void btn保存_Click_1(object sender, EventArgs e)
        {
            save();
        }

        private void btn浏览_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {

                String fullName = ofd.FileName;
                fname = ofd.SafeFileName;
                textBox1.Text = fullName;
                DateTime now = DateTime.Now;
            }
        }

        private void btn上传_Click(object sender, EventArgs e)
        {
            if (fname == null || fname == "")
            {
                MessageBox.Show("请先选择一个文件");
                return;
            }
            String path = System.Environment.CurrentDirectory + @"/../../物料入场检验报告/";
            System.IO.File.Copy(textBox1.Text, path + fname, true);
            dtOuter.Rows[0]["检验报告路径"] = path + fname;
        }

        private void btn查看_Click(object sender, EventArgs e)
        {
            string path = dtOuter.Rows[0]["检验报告路径"].ToString();
            try
            {
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                MessageBox.Show("找不到文件！");
            }
        }


        private string generate退货申请单编号()
        {
            string prefix = "PA-PRS-";
            string yymmdd = DateTime.Now.ToString("yyyy");
            string sql = "select * from 产品退货申请单 where 退货申请单编号 like '{0}%' order by ID";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, prefix + yymmdd), mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["退货申请单编号"].ToString().Substring(11, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }

        void create退货申请()
        {
            // ???? 产品退货申请单是别人退回来，这个是退给别人
            string haoma = generate退货申请单编号();
            SqlDataAdapter da = new SqlDataAdapter("select * from 产品退货申请单 where 退货申请单编号='" + haoma + "'", mySystem.Parameter.conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow ndr = dt.NewRow();
            ndr["退货申请单编号"] = haoma;
            ndr["申请人"] = mySystem.Parameter.userName;
            ndr["申请日期"] = DateTime.Now;
            
        }

    }
}
