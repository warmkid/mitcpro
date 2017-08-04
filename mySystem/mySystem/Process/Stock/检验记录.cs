using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace mySystem.Process.Stock
{
    public partial class 检验记录 : Form
    {

        List<String> ls操作员 = new List<string>();
        List<String> ls审核员 = new List<string>();

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        mySystem.Parameter.UserState _userState;

        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        mySystem.Parameter.FormState _formState;



        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";

        OleDbConnection conn;

        int _id;

        public 检验记录(int id)
        {
            InitializeComponent();
            _id = id;
            conn = new OleDbConnection(strConn);
            conn.Open();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData(id);
            outerBind();

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();
        }


        void getPeople()
        {
            // TODO
            ls审核员 = new List<string>();
            ls操作员 = new List<string>();
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='" + "检验记录" + "'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户权限设置有误，为避免出现错误，请尽快联系管理员完成设置！");
                this.Dispose();
            }

            string str操作员 = dt.Rows[0]["操作员"].ToString();
            string str审核员 = dt.Rows[0]["审核员"].ToString();
            String[] tmp = Regex.Split(str操作员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls操作员.Add(s);
                }
            }
            tmp = Regex.Split(str审核员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls审核员.Add(s);
                }
            }
        }

        void setUserState()
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

        void getOtherData()
        {

        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 检验记录 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            cmb检验结论.Items.Clear();
            cmb检验结论.Items.Add("合格");
            cmb检验结论.Items.Add("不合格");
            // 循环绑定和解绑
            // tb=textbox , lbl=label, cmb=combobox,dtp=datetimepicker
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
            //tb产品名称.DataBindings.Add("Text", bsOuter.DataSource, "产品名称");
            //tb产品代码.DataBindings.Add("Text", bsOuter.DataSource, "产品代码");
            //tb数量.DataBindings.Add("Text", bsOuter.DataSource, "数量");
            //tb产品批号.DataBindings.Add("Text", bsOuter.DataSource, "产品批号");
          


        }

        void readInnerData(int id)
        {
            
        }

        void innerBind()
        {
            //bsInner.DataSource = dtInner;
            //dataGridView1.DataSource = bsInner.DataSource;
        }

        void addComputerEventHandler()
        {

        }

        void setFormState()
        {
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

        void addOtherEvnetHandler()
        {
        }

        void setControlTrue()
        {
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
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
        }

        void setControlFalse()
        {
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
            // btn查看日志.Enabled = true;
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();

            btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;



            da = new OleDbDataAdapter("select * from 待审核 where 表名='检验记录' and 对应ID=" + dtOuter.Rows[0]["ID"], conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow odr = dt.NewRow();
            odr["表名"] = "检验记录";
            odr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(odr);
            da.Update(dt);


            dtOuter.Rows[0]["审核员"] = "__待审核";
            _formState = Parameter.FormState.待审核;
            btn提交审核.Enabled = false;
            daOuter.Update((DataTable)bsOuter.DataSource);
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();


            setControlFalse();


            
            // 判断检验结论，如果合格，则：
            if (cmb检验结论.SelectedItem.ToString() == "合格")
            {
                // 修改验收记录，判断可以生产请验单
                // 根据dtOuter中的物资验收记录 ID 和 产品名称找到  物资验收记录详细信息  中的那一行数据
                String sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0} and 物料名称='{1}'";
                 da = new OleDbDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]), dtOuter.Rows[0]["物料名称"].ToString()), conn);
                 dt = new DataTable("temp");
                 cb = new OleDbCommandBuilder(da);
                da.Fill(dt);
                // 修改  是  为  否
                dt.Rows[0]["是否需要检验"] = "否";
                da.Update(dt);
                // 读取该 物资验收记录 ID 下的所有详细信息，看是否都为否了
                sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0}";
                da = new OleDbDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"])), conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["是否需要检验"].ToString() == "是") { return; }
                }
                // 如果都为否了，则自动生成请验单
                MessageBox.Show("当期验收单下的所有产品都通过检查，正在为您自动生产物资请验单");
                物资验收记录 form = new 物资验收记录(Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]));
                form.create请验单();
                form.create取样记录();
                form.insert台账();
            }
            // 否则 生成不合格品处理记录
            else
            {
                // TODO 不合格品记录如何点击？？
                 da = new OleDbDataAdapter("select * from 不合格品处理记录 where 物资验收记录ID=" + dtOuter.Rows[0]["物资验收记录ID"] + " and 物料名称='" + dtOuter.Rows[0]["物料名称"] + "'", conn);
                 dt = new DataTable("不合格品处理记录");
                 cb = new OleDbCommandBuilder(da);
                BindingSource bs = new BindingSource();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["物资验收记录ID"] = dtOuter.Rows[0]["物资验收记录ID"];
                    dr["物料名称"] = dtOuter.Rows[0]["物料名称"];
                    dr["物料代码"] = dtOuter.Rows[0]["物料代码"];
                    dr["编号"] = create检验记录编号();
                    dr["产品批号"] = dtOuter.Rows[0]["产品批号"];
                    dr["数量"] = dtOuter.Rows[0]["数量"];
                    dr["生产日期"] = DateTime.Now;
                    dr["不合格项描述填写日期"] = DateTime.Now;
                    dr["现场应急处理措施日期"] = DateTime.Now;
                    dr["调查日期"] = DateTime.Now;
                    dr["不合格品处理评审时间"] = DateTime.Now;
                    dr["不合格品处理批准时间"] = DateTime.Now;
                    dr["确认日期"] = DateTime.Now;
                    dr["审核日期"] = DateTime.Now;
                    dt.Rows.Add(dr);
                    da.Update(dt);
                    MessageBox.Show("已经自动生成不合格品记录");
                }
                
               
                
            }
            
        }


        string create检验记录编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 编号 from 不合格品处理记录 order by ID DESC", conn);
            DataTable dt = new DataTable("不合格品处理记录");
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
    }
}
