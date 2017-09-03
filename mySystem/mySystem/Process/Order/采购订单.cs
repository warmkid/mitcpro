﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace mySystem.Process.Order
{
    public partial class 采购订单 : BaseForm
    {
        bool isSaved = false;
        // TODO 没点保存的话怎么办？数据库中状态变了，订单有！
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner,dtInnerShow;
        BindingSource bsOuter, bsInner;

        List<String> ls操作员, ls审核员;
        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;
        List<String> ls币种;
        
        CheckForm ckform;
        string _供应商;
        

        public 采购订单(MainForm mainform, string 供应商)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            conn = new OleDbConnection(strConnect);
            _供应商 = 供应商;
            conn.Open();
            getPeople();
            setUseState();

            getOtherData();

            generateOuterData(供应商);
            outerBind();
            generateInnerData(供应商);
            calcSumInner();
            innerBind();



            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
            //lbl供应商.Text = dtOuter.Rows[0]["供应商"].ToString();
        }

        public 采购订单(MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            isSaved = true;
            fillPrinter();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            getPeople();
            setUseState();

            getOtherData();

            readOuterData(id);
            outerBind();
            readInnerData(id);
            calcSumInner();
            innerBind();



            addOtherEventHandler();
            setFormState();
            setEnableReadOnly();
            //lbl供应商.Text = dtOuter.Rows[0]["供应商"].ToString();
        }

        void calcSumInner()
        {
            dtInnerShow = dtInner.Clone();
            // 拿到所有的存货代码
            HashSet<String> hs存货代码 = new HashSet<string>();
            foreach (DataRow dr in dtInner.Rows)
            {
                hs存货代码.Add(dr["存货代码"].ToString());
            }
            // 求和
            foreach (string dm in hs存货代码)
            {
                DataRow[] drs = dtInner.Select("存货代码='" + dm + "'");
                if (drs.Length <= 0) continue;
                DataRow ndr = dtInnerShow.NewRow();
                ndr["存货代码"] = drs[0]["存货代码"];
                ndr["存货名称"] = drs[0]["存货名称"];
                ndr["规格型号"] = drs[0]["规格型号"];
                ndr["采购件数"] = 0;
                ndr["采购数量"] = 0;
                ndr["预计到货时间"] = DateTime.Now;
                foreach (DataRow dr in drs)
                {
                    ndr["采购件数"] = Convert.ToDouble(ndr["采购件数"]) + Convert.ToDouble(dr["采购件数"]);
                    ndr["采购数量"] = Convert.ToDouble(ndr["采购数量"]) + Convert.ToDouble(dr["采购数量"]);
                }
                dtInnerShow.Rows.Add(ndr);
            }
            //写入dtinnershow
        }

        private void getOtherData()
        {
            OleDbDataAdapter da;
            DataTable dt;
            ls币种 = new List<string>();
            da = new OleDbDataAdapter("select * from 设置币种", conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls币种.Add(dr["币种"].ToString());
            }
            cmb币种.Items.AddRange(ls币种.ToArray());
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
            da = new OleDbDataAdapter("select * from 订单用户权限 where 步骤='采购批准单'", conn);
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
            tb采购合同号.ReadOnly = true;
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
        }

        void generateOuterData(string 供应商)
        {
            daOuter = new OleDbDataAdapter("select * from 采购订单 where 0=1", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购订单");

            daOuter.Fill(dtOuter);

            DataRow dr = dtOuter.NewRow();
            dr["采购合同号"] = generate采购合同号();
            dr["申请日期"] = DateTime.Now;
            dr["申请人"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;
            dr["状态"] = "编辑中";
            dr["供应商"] = 供应商;
            dr["订单日期"] = DateTime.Now;
            dr["汇率"] = 1;
            dtOuter.Rows.Add(dr);
            daOuter.Update(dtOuter);
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn;
            comm.CommandText = "select @@identity";
            int id = (Int32)comm.ExecuteScalar();
            daOuter = new OleDbDataAdapter("select * from 采购订单 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("采购订单");
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

        void generateInnerData(String 供应商)
        {

            daInner = new OleDbDataAdapter("select * from 采购订单详细信息 where 0=1", conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购订单详细信息");

            daInner.Fill(dtInner);


            string sql = @"select * from 采购批准单详细信息 where 推荐供应商='{0}' and 状态='未采购'";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, 供应商), conn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                OleDbDataAdapter daT;
                DataTable dtT;
                daT = new OleDbDataAdapter("select * from 设置存货档案 where 存货代码='" + dr["存货代码"] + "'", conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                string 主计量 = "";
                if (dtT.Rows.Count > 0)
                    主计量 = dtT.Rows[0]["主计量单位名称"].ToString();
                DataRow ndr = dtInner.NewRow();
                ndr["采购订单ID"] = dtOuter.Rows[0]["ID"];
                ndr["存货代码"] = dr["存货代码"];
                ndr["存货名称"] = dr["存货名称"];
                ndr["规格型号"] = dr["规格型号"];
                ndr["采购件数"] = dr["采购件数"];
                ndr["采购数量"] = dr["采购数量"];
                ndr["用途"] = dr["用途"];
                ndr["供应商产品编码"] = dr["供应商产品编码"];
                ndr["预计到货时间"] = dr["预计到货时间"];
                ndr["关联的采购批准详细信息ID"] = dr["ID"];
                ndr["主计量"] = 主计量;
                //daT = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + dr["用途"].ToString() + "'", conn);
                //dtT = new DataTable();
                //daT.Fill(dtT);
                //int xsID = Convert.ToInt32(dtT.Rows[0]["ID"]);
                //daT = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + xsID + " and 存货代码='" + dr["存货代码"].ToString() + "'", conn);
                //dtT = new DataTable();
                //daT.Fill(dtT);
                //double 价税合计 = Convert.ToDouble(dtT.Rows[0]["价税合计"]);
                ndr["单价"] = 0;
                ndr["金额"] = 0;
                ndr["付款日期"] = DateTime.Now;
                dtInner.Rows.Add(ndr);
                // 改变批准单详细信息中的状态
                //dr["状态"] = "采购订单编制中";
                // 自由订单部分
                int 采购批准单ID = Convert.ToInt32(dr["采购批准单ID"]);
                daT = new OleDbDataAdapter("select * from 采购批准单实际购入信息 where 采购批准单ID=" + 采购批准单ID + " and 产品代码='" + dr["存货代码"].ToString() + "'", conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                if(dtT.Rows.Count>0){
                    double 富余量 = Convert.ToDouble(dtT.Rows[0]["富余量"]);
                    if (富余量 > 0)
                    {
                        DataRow ndr自由 = dtInner.NewRow();
                        ndr自由["采购订单ID"] = dtOuter.Rows[0]["ID"];
                        ndr自由["存货代码"] = dr["存货代码"];
                        ndr自由["存货名称"] = dr["存货名称"];
                        ndr自由["规格型号"] = dr["规格型号"];
                        ndr自由["采购件数"] = 富余量 / Convert.ToDouble(dr["主计量单位每辅计量单位"]);
                        ndr自由["采购数量"] = 富余量;
                        ndr自由["用途"] = "__自由";
                        ndr自由["供应商产品编码"] = dr["供应商产品编码"];
                        ndr自由["预计到货时间"] = dr["预计到货时间"];
                        ndr自由["关联的采购批准详细信息ID"] = dr["ID"];
                        ndr["主计量"] = 主计量;
                        ndr["单价"] = 0;
                        ndr["金额"] = 0;
                        ndr["付款日期"] = DateTime.Now;
                        dtInner.Rows.Add(ndr自由);
                    }
                }

            }

            


            da.Update(dt);

            sql = @"select * from 采购批准单借用订单详细信息 where 推荐供应商='{0}' and 状态='未采购'";
            da = new OleDbDataAdapter(string.Format(sql, 供应商), conn);
            cb = new OleDbCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                OleDbDataAdapter daT;
                DataTable dtT;
                daT = new OleDbDataAdapter("select * from 设置组件存货档案 where 存货编码='" + dr["存货代码"] + "'", conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                string 主计量 = "";
                if (dtT.Rows.Count > 0)
                    主计量 = dtT.Rows[0]["主计量单位名称"].ToString();
                DataRow ndr = dtInner.NewRow();
                ndr["采购订单ID"] = dtOuter.Rows[0]["ID"];
                ndr["存货代码"] = dr["存货代码"];
                ndr["存货名称"] = dr["存货名称"];
                ndr["规格型号"] = dr["规格型号"];
                ndr["采购件数"] = dr["采购件数"];
                ndr["采购数量"] = dr["采购数量"];
                ndr["用途"] = dr["用途"];
                ndr["预计到货时间"] = dr["预计到货时间"];
                ndr["关联的采购批转单借用单ID"] = dr["ID"];
                ndr["主计量"] = 主计量;
                daT = new OleDbDataAdapter("select * from 销售订单 where 订单号='" + dr["用途"].ToString() + "'", conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                int xsID = Convert.ToInt32(dtT.Rows[0]["ID"]);
                daT = new OleDbDataAdapter("select * from 销售订单详细信息 where 销售订单ID=" + xsID + " and 存货代码='" + dr["存货代码"].ToString() + "'", conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                double 价税合计 = Convert.ToDouble(dtT.Rows[0]["价税合计"]);
                ndr["单价"] = 价税合计 / Convert.ToDouble(dr["采购数量"]);
                ndr["金额"] = 价税合计;
                ndr["付款日期"] = DateTime.Now;
                dtInner.Rows.Add(ndr);
                // 改变批准单详细信息中的状态
                dr["状态"] = "采购订单编制中";
            }
            da.Update(dt);

            
            daInner.Update(dtInner);
            daInner = new OleDbDataAdapter("select * from 采购订单详细信息 where 采购订单ID=" + dtOuter.Rows[0]["ID"], conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购订单详细信息");

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInnerShow;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        void addOtherEventHandler()
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["采购订单ID"].Visible = false;
            dataGridView1.Columns["关联的采购批准详细信息ID"].Visible = false;
            dataGridView1.Columns["关联的采购批转单借用单ID"].Visible = false;
            dataGridView1.Columns["用途"].Visible = false;
            dataGridView1.Columns["主计量"].Visible = false;
            dataGridView1.Columns["单价"].Visible = false;
            dataGridView1.Columns["金额"].Visible = false;
            dataGridView1.Columns["进度"].Visible = false;
            dataGridView1.Columns["COC"].Visible = false;
            dataGridView1.Columns["进度"].Visible = false;
            dataGridView1.Columns["付款进度"].Visible = false;
            dataGridView1.Columns["付款日期"].Visible = false;
            dataGridView1.Columns["发票"].Visible = false;
        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 采购订单 where ID="+id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("采购订单");

            daOuter.Fill(dtOuter);
        }


        void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 采购订单详细信息 where 采购订单ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("采购订单详细信息");

            daInner.Fill(dtInner);
        }

        private void btn确认_Click(object sender, EventArgs e)
        {
            isSaved = true;
            save();
            if (_userState == Parameter.UserState.操作员)
                btn提交审核.Enabled = true;



            string sql = @"select * from 采购批准单详细信息 where 推荐供应商='{0}' and 状态='未采购'";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, _供应商), conn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                
                dr["状态"] = "采购订单编制中";
            }
            da.Update(dt);
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            outerBind();

            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购订单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "采购订单";
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

        private void btn审核_Click(object sender, EventArgs e)
        {
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
                // 读内表，然后改批准单的状态
                OleDbDataAdapter da;
                OleDbCommandBuilder cb;
                DataTable dt;
                foreach (DataRow dr in dtInner.Rows)
                {
                    int xid;
                    if (dr["关联的采购批准详细信息ID"] == DBNull.Value)
                    {
                        xid = Convert.ToInt32(dr["关联的采购批转单借用单ID"]);
                        da = new OleDbDataAdapter("select * from 采购批准单借用订单详细信息 where ID=" + xid, conn);
                        cb = new OleDbCommandBuilder(da);
                        dt = new DataTable();
                        da.Fill(dt);
                        dt.Rows[0]["状态"] = "已完成采购订单";

                        da.Update(dt);
                        // 更新借用日志
                        string 产品代码 = dt.Rows[0]["存货代码"].ToString();
                        string 用途 = dt.Rows[0]["用途"].ToString();
                        OleDbDataAdapter daT = new OleDbDataAdapter("select * from 库存台帐 where 产品代码='" + 产品代码 + "' and 用途='" + 用途 + "'", conn);
                        OleDbCommandBuilder cbT = new OleDbCommandBuilder(daT);
                        DataTable dtT = new DataTable();
                        daT.Fill(dtT);
                        if (dtT.Rows.Count > 0)
                        {
                            foreach (DataRow tdr in dtT.Rows)
                            {
                                string log = "";
                                log += DateTime.Now.ToString("yyyy年MM月dd日，采购合同号："+dtOuter.Rows[0]["采购合同号"].ToString()+"，补充数量："+ Convert.ToDouble( dt.Rows[0]["采购数量"])+"。\n");
                                tdr["借用日志"] = tdr["借用日志"] + log;
                            }
                        }
                        daT.Update(dtT);

                    }
                    else
                    {
                        xid = Convert.ToInt32(dr["关联的采购批准详细信息ID"]);
                        da = new OleDbDataAdapter("select * from 采购批准单详细信息 where ID=" + xid, conn);
                        cb = new OleDbCommandBuilder(da);
                        dt = new DataTable();
                        da.Fill(dt);
                        dt.Rows[0]["状态"] = "已完成采购订单";

                        da.Update(dt);
                    }
                    
                } 



            }
            else
            {
                // 读内表，然后改写需求单
                dtOuter.Rows[0]["状态"] = "编辑中";//未审核，草稿
            }
            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='采购订单' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], conn);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);



            save();
            base.CheckResult();
        }

        string generate采购合同号()
        {
            string prefix = "PAPO";
            string yymmdd = DateTime.Now.ToString("yy");
            string sql = "select * from 采购订单 where 采购合同号 like '{0}%' order by ID";
            OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, prefix + yymmdd), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return prefix + yymmdd + "001";
            }
            else
            {
                int no = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["采购合同号"].ToString().Substring(6, 3));
                return prefix + yymmdd + (no + 1).ToString("D3");
            }
        }

        private void 采购订单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null && dtOuter.Rows.Count != 0)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update(dtOuter);

                    foreach (DataRow dr in dtInner.Rows)
                    {
                        dr.Delete();
                    }
                    daInner.Update(dtInner);
                }
            }
        }
    }
}
