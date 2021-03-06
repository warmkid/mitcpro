﻿using System;
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
    public partial class 复验记录 : BaseForm
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



        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

//        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";

//        OleDbConnection conn;

        int _id;

        CheckForm ckform;

        public 复验记录(MainForm mainform, int id):base(mainform)
        {
            InitializeComponent();
            _id = id;
            //conn = new OleDbConnection(strConn);
            //conn.Open();
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
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='" + "复验记录" + "'", mySystem.Parameter.conn);
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
            daOuter = new SqlDataAdapter("select * from 复验记录 where ID=" + id, mySystem.Parameter.conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("复验记录");
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
            foreach (ToolStripItem tsi in contextMenuStrip1.Items)
            {
                tsi.Click += new EventHandler(tsi_Click);
                this.ContextMenuStrip = contextMenuStrip1;
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da;
            DataTable dt;
            if (this.Name == sender.ToString())
            {
                return;
            }
            int id;
            if (this.Name == "物资验收记录")
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            }
            else
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]);
            }
            try
            {
                switch (sender.ToString())
                {
                    case "物资验收记录":
                        物资验收记录 form1 = new 物资验收记录(mainform,id);
                        form1.Show();
                        break;
                    case "物资请验单":
                        da = new SqlDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        物资请验单 form2 = new 物资请验单(mainform, Convert.ToInt32(dt.Rows[0]["ID"]));
                        form2.Show();
                        break;
                    case "检验记录":
                        da = new SqlDataAdapter("select * from 检验记录 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0) MessageBox.Show("没有关联的检验记录");
                        foreach (DataRow dr in dt.Rows)
                        {
                            (new 复验记录(mainform, Convert.ToInt32(dr["ID"]))).Show();                            //form3.Show();
                        }
                        break;
                    case "取样记录":
                        da = new SqlDataAdapter("select * from 取样记录 where 物资验收记录ID=" + id, mySystem.Parameter.conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        取样记录 form4 = new 取样记录(mainform, Convert.ToInt32(dt.Rows[0]["ID"]));
                        form4.Show();
                        break;
                }
            }
            catch
            {
                MessageBox.Show("关联失败，请检查是否有相应数据");
            }
            //MessageBox.Show(this.Name + "\n" + sender.ToString());
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



            
            
            
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;



            da = new SqlDataAdapter("select * from 待审核 where 表名='复验记录' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow odr = dt.NewRow();
            odr["表名"] = "复验记录";
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
            //if (cmb检验结论.SelectedItem.ToString() == "合格")
            //{
            //    // 修改验收记录，判断可以生产请验单
            //    // 根据dtOuter中的物资验收记录 ID 和 产品名称找到  物资验收记录详细信息  中的那一行数据
            //    String sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0} and 物料名称='{1}'";
            //    da = new SqlDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]), dtOuter.Rows[0]["物料名称"].ToString()), mySystem.Parameter.conn);
            //    dt = new DataTable("temp");
            //    cb = new SqlCommandBuilder(da);
            //    da.Fill(dt);
            //    // 修改  是  为  否
            //    dt.Rows[0]["是否需要检验"] = "否";
            //    da.Update(dt);
            //    // 读取该 物资验收记录 ID 下的所有详细信息，看是否都为否了
            //    sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0}";
            //    da = new SqlDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"])), mySystem.Parameter.conn);
            //    dt = new DataTable("temp");
            //    da.Fill(dt);
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (dr["是否需要检验"].ToString() == "是") { return; }
            //    }
            //    // 如果都为否了，则自动生成请验单
            //    MessageBox.Show("当期验收单下的所有产品都通过检查，正在为您自动生产物资请验单");
            //    物资验收记录 form = new 物资验收记录(mainform, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]));
            //    form.create请验单();
            //    form.create取样记录();
            //    form.insert检验台账();
            //    form.insert库存台帐();
            //}
            //// 否则 生成不合格品处理记录
            //else
            //{
            //    // TODO 不合格品记录如何点击？？
            //    da = new SqlDataAdapter("select * from 不合格品处理记录 where 物资验收记录ID=" + dtOuter.Rows[0]["物资验收记录ID"] + " and 物料名称='" + dtOuter.Rows[0]["物料名称"] + "'", mySystem.Parameter.conn);
            //    dt = new DataTable("不合格品处理记录");
            //    cb = new SqlCommandBuilder(da);
            //    BindingSource bs = new BindingSource();
            //    da.Fill(dt);
            //    if (dt.Rows.Count == 0)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr["物资验收记录ID"] = dtOuter.Rows[0]["物资验收记录ID"];
            //        dr["物料名称"] = dtOuter.Rows[0]["物料名称"];
            //        dr["物料代码"] = dtOuter.Rows[0]["物料代码"];
            //        dr["编号"] = create检验记录编号();
            //        dr["产品批号"] = dtOuter.Rows[0]["产品批号"];
            //        dr["数量"] = dtOuter.Rows[0]["数量"];
            //        dr["生产日期"] = DateTime.Now;
            //        dr["不合格项描述填写日期"] = DateTime.Now;
            //        dr["现场应急处理措施日期"] = DateTime.Now;
            //        dr["调查日期"] = DateTime.Now;
            //        dr["不合格品处理评审时间"] = DateTime.Now;
            //        dr["不合格品处理批准时间"] = DateTime.Now;
            //        dr["确认日期"] = DateTime.Now;
            //        dr["审核日期"] = DateTime.Now;
            //        dt.Rows.Add(dr);
            //        da.Update(dt);
            //        MessageBox.Show("已经自动生成不合格品记录");
            //    }
                
               
                
            //}
            
        }


        string create检验记录编号()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 1 编号 from 不合格品处理记录 order by ID DESC", mySystem.Parameter.conn);
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
                // 判断检验结论，如果合格，则：
                if (cmb检验结论.SelectedItem.ToString() == "合格")
                {
                    //// 修改验收记录，判断可以生产请验单
                    //// 根据dtOuter中的物资验收记录 ID 和 产品名称找到  物资验收记录详细信息  中的那一行数据
                    //String sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0} and 物料名称='{1}'";
                    //da = new SqlDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]), dtOuter.Rows[0]["物料名称"].ToString()), mySystem.Parameter.conn);
                    //dt = new DataTable("temp");
                    //cb = new SqlCommandBuilder(da);
                    //da.Fill(dt);
                    //// 修改  是  为  否
                    //dt.Rows[0]["是否需要检验"] = "否";
                    //da.Update(dt);
                    //// 读取该 物资验收记录 ID 下的所有详细信息，看是否都为否了
                    //sql = "select * from 物资验收记录详细信息 where 物资验收记录ID={0}";
                    //da = new SqlDataAdapter(String.Format(sql, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"])), mySystem.Parameter.conn);
                    //dt = new DataTable("temp");
                    //da.Fill(dt);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    if (dr["是否需要检验"].ToString() == "是") { return; }
                    //}
                    //// 如果都为否了，则自动生成请验单
                    //MessageBox.Show("当期验收单下的所有产品都通过检查，正在为您自动生产物资请验单");
                    //物资验收记录 form = new 物资验收记录(mainform, Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]));
                    //form.create请验单();
                    //form.create取样记录();
                    //form.insert检验台账();
                    //form.insert库存台帐();




                    // 生成入库单
                    create入库单();
                }
                // 否则 生成不合格品处理记录
                else
                {
                    // TODO 不合格品记录如何点击？？
                    da = new SqlDataAdapter("select * from 不合格品处理记录 where 物资验收记录ID=" + dtOuter.Rows[0]["物资验收记录ID"] + " and 物料名称='" + dtOuter.Rows[0]["物料名称"] + "'", mySystem.Parameter.conn);
                    dt = new DataTable("不合格品处理记录");
                    cb = new SqlCommandBuilder(da);
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
                        dr["审核结果"] = false;//默认值
                        dt.Rows.Add(dr);
                        da.Update(dt);
                        MessageBox.Show("已经自动生成不合格品记录");
                    }



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
            SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='复验记录' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();
            base.CheckResult();

            try
            {
                (this.Owner as mySystem.Process.Stock.原料入库管理).search();
            }
            catch (Exception ee)
            {
            }
        }


        void create入库单()
        {
            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;
            string haoma = 入库单.create入库单号();
            da = new SqlDataAdapter("select * from 入库单 where 入库单号='" + haoma + "'", mySystem.Parameter.conn);
            dt = new DataTable("入库单");
            cb = new SqlCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);
            DataRow ndr = dt.NewRow();
            DataRow dr = dtOuter.Rows[0];
            ndr["关联的验收记录ID"] = dr["物资验收记录ID"];
            ndr["入库单号"] = haoma;
            ndr["入库日期"] = DateTime.Now;
            ndr["采购订单号"] = dr["采购订单号"];//
            ndr["供应商"] = dr["供应商名称"];
            ndr["产品代码"] = dr["物料代码"];
            ndr["产品名称"] = dr["物料名称"];
            ndr["规格型号"] = dr["规格型号"];
            ndr["批号"] = dr["产品批号"];
            ndr["主计量单位"] = dr["单位"];
            ndr["数量"] = dr["数量"];
            ndr["换算率"] = dr["换算率"];
            ndr["入库人"] = mySystem.Parameter.userName;
            ndr["审核日期"] = DateTime.Now;
            ndr["审核结果"] = false;//默认值
            dt.Rows.Add(ndr);
            da.Update(dt);
            MessageBox.Show("已为物料：" + dr["物料名称"] + "生成入库单");
        }
    }
}
