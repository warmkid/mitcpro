using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
//using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;



namespace mySystem.Extruction.Process
{
    /// <summary>
    /// 吹膜工序清场记录
    /// </summary>
    public partial class Record_extrusSiteClean : mySystem.BaseForm
    {
        bool checkout;//检查结果
        mySystem.CheckForm checkform;

        //OleDbConnection connOle;
        private DataTable dt_prodinstr, dt_prodlist,dt_prodlist2;//大表，供料工序清洁项目，吹膜工序清场项目

        private OleDbDataAdapter da_prodinstr, da_prodlist, da_prodlist2;
        private BindingSource bs_prodinstr, bs_prodlist, bs_prodlist2;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist, cb_prodlist2;

        private SqlDataAdapter da_prodinstr_sql, da_prodlist_sql, da_prodlist2_sql;
        private SqlCommandBuilder cb_prodinstr_sql, cb_prodlist_sql, cb_prodlist2_sql;

        DataTable dt_供料设置,dt_吹膜设置;
        string code, batch;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        bool isFirstBind1 = true;
        bool isFirstBind2 = true; 

        private void readsetting()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                //先读取供料工序清场设置，拷贝到上面table中
                string asql = "select 序号,清场内容 from 设置供料工序清场项目";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                da.Fill(dt_供料设置);

                //先读取吹膜工序清场设置，拷贝到上面table中
                asql = "select 序号,清场内容 from 设置吹膜工序清场项目";
                OleDbCommand comm2 = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da2 = new OleDbDataAdapter(comm2);
                da2.Fill(dt_吹膜设置);
            }
            else
            {
                //先读取供料工序清场设置，拷贝到上面table中
                string asql = "select 序号,清场内容 from 设置供料工序清场项目";
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt_供料设置);

                //先读取吹膜工序清场设置，拷贝到上面table中
                asql = "select 序号,清场内容 from 设置吹膜工序清场项目";
                SqlCommand comm2 = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da2 = new SqlDataAdapter(comm2);
                da2.Fill(dt_吹膜设置);
            }

        }
        //判断之前的内容是否与设置表中内容一致
        private bool is_serve_sameto_setting()
        {
            if (dt_prodlist.Rows.Count != dt_供料设置.Rows.Count)
                return false;
            for (int i = 0; i < dt_prodlist.Rows.Count; i++)
            {
                if (dt_prodlist.Rows[i]["清场要点"].ToString() != dt_供料设置.Rows[i]["清场内容"].ToString())
                    return false;
            }
            return true;
        }
        private bool is_extrusion_sameto_setting()
        {
            if (dt_prodlist2.Rows.Count != dt_吹膜设置.Rows.Count)
                return false;
            for (int i = 0; i < dt_prodlist2.Rows.Count; i++)
            {
                if (dt_prodlist2.Rows[i]["清场要点"].ToString() != dt_吹膜设置.Rows[i]["清场内容"].ToString())
                    return false;
            }
            return true;
        }

        private void Init()
        {

            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            //dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();

            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            da_prodinstr_sql = new SqlDataAdapter();
            cb_prodinstr_sql = new SqlCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();

            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            da_prodlist_sql = new SqlDataAdapter();
            cb_prodlist_sql = new SqlCommandBuilder();

            dt_prodlist2 = new System.Data.DataTable();
            bs_prodlist2 = new System.Windows.Forms.BindingSource();

            da_prodlist2 = new OleDbDataAdapter();
            cb_prodlist2 = new OleDbCommandBuilder();

            da_prodlist2_sql = new SqlDataAdapter();
            cb_prodlist2_sql = new SqlCommandBuilder();

            dt_吹膜设置 = new DataTable();
            dt_供料设置 = new DataTable();

            tb生产指令.Text = mySystem.Parameter.proInstruction;            

        }

        void checkform_FormClosed(object sender, FormClosedEventArgs e)
        {
            fresh_otherform();
            //throw new NotImplementedException();        
        }

        //查找清场前产品代码和批号
        private void query_prodandbatch()
        {
            DataTable tempdt = new DataTable();
            string asql = "select 产品名称,产品批号 from 吹膜工序生产和检验记录 where 生产指令ID=" + mySystem.Parameter.proInstruID + " ORDER BY 生产日期 DESC";
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                da.Fill(tempdt);
            }
            else
            {
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(tempdt);
            }

            if (tempdt.Rows.Count == 0)
            {
                tempdt.Dispose();
                return;
            }               
            else
            {
                code = (string)tempdt.Rows[0][0];
                batch = (string)tempdt.Rows[0][1];
            }

        }

        private void begin()
        {
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                dataGridView2.Enabled = true;
                dataGridView2.ReadOnly = true;
                return;
            }

            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                if (!mySystem.Parameter.isSqlOk)
                    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                else
                {
                    if (((DataTable)bs_prodinstr.DataSource).Rows[0]["审核是否通过"] == DBNull.Value)
                    {
                        ((DataTable)bs_prodinstr.DataSource).Rows[0]["审核是否通过"] = 0;
                    }
                    //((DataTable)bs_prodinstr.DataSource).Rows[0]["审核是否通过"] = 0;
                    da_prodinstr_sql.Update((DataTable)bs_prodinstr.DataSource);
                }

                readOuterData(mySystem.Parameter.proInstruID);
                removeOuterBinding();
                outerBind();
                instrid = mySystem.Parameter.proInstruID;
            }
           
            ckb合格.Checked = (bool)dt_prodinstr.Rows[0]["审核是否通过"];
            ckb不合格.Checked = !ckb合格.Checked;
            instrid = mySystem.Parameter.proInstruID;

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            dataGridView1.Columns[0].Visible = false;

            readInnerData2((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind2();
            dataGridView2.Columns[0].Visible = false;

            DialogResult result;
            if (!is_serve_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的供料清场记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    if (!mySystem.Parameter.isSqlOk)
                        da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                    else
                        da_prodlist_sql.Update((DataTable)bs_prodlist.DataSource);
                    int index = 1;
                    foreach (DataRow dr in dt_供料设置.Rows)
                    {
                        DataRow ndr = dt_prodlist.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr["序号"] = index++;
                        ndr["清场要点"] = dr["清场内容"];
                        ndr["清场操作"] = "合格";
                        dt_prodlist.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);
                }

            }

            if (!is_extrusion_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的吹膜清场记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView2.Rows.Count > 0)
                        dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
                    if (!mySystem.Parameter.isSqlOk)
                        da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
                    else
                        da_prodlist2_sql.Update((DataTable)bs_prodlist2.DataSource);
                    int index = 1;
                    foreach (DataRow dr in dt_吹膜设置.Rows)
                    {
                        DataRow ndr = dt_prodlist2.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr["序号"] = index++;
                        ndr["清场要点"] = dr["清场内容"];
                        ndr["清场操作"] = "合格";
                        dt_prodlist2.Rows.Add(ndr);
                    }
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);
                }

            }

            setFormState();
            setEnableReadOnly();
        }

        public Record_extrusSiteClean(mySystem.MainForm mainform):base(mainform)
        {

            InitializeComponent();
            Init();
            getPeople();
            setUserState();
            getOtherData();
            begin();
        }

        public Record_extrusSiteClean(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            Init();
            getPeople();
            setUserState();
            getOtherData();

            DataTable tempdt = new DataTable();
            string asql = "select * from 吹膜工序清场记录 where ID=" + id;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                da.Fill(tempdt);
            }
            else
            {
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(tempdt);
            }

            instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());

            readOuterData(instrid);
            removeOuterBinding();
            outerBind();
            ckb合格.Checked = (bool)dt_prodinstr.Rows[0]["审核是否通过"];
            ckb不合格.Checked = !ckb合格.Checked;

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            dataGridView1.Columns[0].Visible = false;

            readInnerData2((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind2();
            dataGridView2.Columns[0].Visible = false;

            setFormState();
            setEnableReadOnly();
            
        }

        //获取设置清场项目和清场前产品代码、批次
        private void getOtherData()
        {
            fill_printer();
            readsetting();
            query_prodandbatch();
        }
        //// 获取操作员和审核员
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();
            DataTable dt = new DataTable("用户权限");
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜工序清场记录'", mySystem.Parameter.connOle);
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter(@"select * from 用户权限 where 步骤='吹膜工序清场记录'", mySystem.Parameter.conn);
                da.Fill(dt);
            }


            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
                string[] s = Regex.Split(person_操作员, ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        list_操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(person_审核员, ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        list_审核员.Add(s1[i]);
                }
            }
        }

        //设置登录人状态
        void setUserState()
        {
            //if (mySystem.Parameter.userName == person_操作员)
            //    stat_user = 0;
            //else if (mySystem.Parameter.userName == person_审核员)
            //    stat_user = 1;
            //else
            //    stat_user = 2;

            _userState = Parameter.UserState.NoBody;
            if (list_操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (list_审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
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

        //设置窗口状态
        void setFormState()
        {
            //if (dt_prodinstr.Rows[0]["审核人"].ToString() == "")
            //    stat_form = 0;
            //else if (dt_prodinstr.Rows[0]["审核人"].ToString() == "__待审核")
            //    stat_form = 1;
            //else if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            //    stat_form = 2;
            //else
            //    stat_form = 3;

            string s = dt_prodinstr.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dt_prodinstr.Rows[0]["审核是否通过"]);
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
                    setControlFalse();
                    bt审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }
        }

        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮一直是false
            bt审核.Enabled = false;
            bt提交审核.Enabled = false;

            ckb不合格.Enabled = false;
            ckb合格.Enabled = false;
        }

        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            bt日志.Enabled = true;
            bt打印.Enabled = true;
            cb打印机.Enabled = true;
            bt查看人员信息.Enabled = true;
        }

        //内外表保存
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
            {
                return false;
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                //外表保存
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                //内表保存
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
                innerBind();
                da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
                readInnerData2(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
                innerBind2();
            }
            else
            {
                //外表保存
                bs_prodinstr.EndEdit();
                da_prodinstr_sql.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                //内表保存
                da_prodlist_sql.Update((DataTable)bs_prodlist.DataSource);
                readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
                innerBind();
                da_prodlist2_sql.Update((DataTable)bs_prodlist2.DataSource);
                readInnerData2(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
                innerBind2();
            }
            setUndoColor();
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt提交审核.Enabled = true;
        }

        private bool input_Judge()
        {
            if (mySystem.Parameter.NametoID(tb清场人.Text) <= 0)
            {
                MessageBox.Show("清场人ID不存在");
                return false;
            }
            return true;
        }

        //重写函数，获得审核信息
        public override void CheckResult()
        {
            //获得审核信息
            tb检查人.Text = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["检查人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["审核人"] = mySystem.Parameter.userName;
            ckb合格.Checked = checkform.ischeckOk;
            ckb不合格.Checked = !ckb合格.Checked;
            dt_prodinstr.Rows[0]["检查结果"] = checkform.ischeckOk;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;

            //状态
            setControlFalse();

            if (!mySystem.Parameter.isSqlOk)
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜工序清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                //写日志
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
                log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
                log += "审核意见：" + checkform.opinion;
                dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
            else
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='吹膜工序清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                //写日志
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
                log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
                log += "审核意见：" + checkform.opinion;
                dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

                bs_prodinstr.EndEdit();
                da_prodinstr_sql.Update((DataTable)bs_prodinstr.DataSource);
            }

            base.CheckResult();
            checkform.Close();
            try
            {
                (this.Owner as mySystem.Query.QueryExtruForm).search();
            }
            catch (NullReferenceException exp) { }
            
        }

        //审核通过之后刷新其他的表
        private void fresh_otherform()
        {
            //审核通过
            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                //日报表调用带ID的
                DataTable dt_日报表 = new DataTable("吹膜生产日报表");
                if (!mySystem.Parameter.isSqlOk)
                {
                    OleDbDataAdapter da_日报表 = new OleDbDataAdapter("select * from 吹膜生产日报表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                    OleDbCommandBuilder cb_日报表 = new OleDbCommandBuilder(da_日报表);
                    da_日报表.Fill(dt_日报表);
                }
                else
                {
                    SqlDataAdapter da_日报表 = new SqlDataAdapter("select * from 吹膜生产日报表 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                    SqlCommandBuilder cb_日报表 = new SqlCommandBuilder(da_日报表);
                    da_日报表.Fill(dt_日报表);
                }

                int id_日报表 = (int)dt_日报表.Rows[0]["ID"];
                new mySystem.ProdctDaily_extrus(mainform,id_日报表);

                //查找该生产指令ID下对应的物料平衡表记录的ID
                DataTable dt_物料 = new DataTable("吹膜工序物料平衡记录");
                if (!mySystem.Parameter.isSqlOk)
                {
                    OleDbDataAdapter da_物料 = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                    OleDbCommandBuilder cb_物料 = new OleDbCommandBuilder(da_物料);
                    da_物料.Fill(dt_物料);
                }
                else
                {
                    SqlDataAdapter da_物料 = new SqlDataAdapter("select * from 吹膜工序物料平衡记录 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                    SqlCommandBuilder cb_物料 = new SqlCommandBuilder(da_物料);
                    da_物料.Fill(dt_物料);
                }

                int id_物料 = (int)dt_物料.Rows[0]["ID"];

                new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform, id_物料);

                DialogResult result;
                //result = MessageBox.Show("是否确定完成当前生产指令", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (true)
                {
                    DataTable dt_tempdt = new DataTable("生产指令信息");
                    if (!mySystem.Parameter.isSqlOk)
                    {
                        OleDbDataAdapter da_tempdt = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + instrid, mySystem.Parameter.connOle);
                        OleDbCommandBuilder cb_prodinstr = new OleDbCommandBuilder(da_tempdt);
                        da_tempdt.Fill(dt_tempdt);
                        dt_tempdt.Rows[0]["状态"] = 4;
                        da_tempdt.Update(dt_tempdt);
                    }
                    else
                    {
                        SqlDataAdapter da_tempdt = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + instrid, mySystem.Parameter.conn);
                        SqlCommandBuilder cb_prodinstr = new SqlCommandBuilder(da_tempdt);
                        da_tempdt.Fill(dt_tempdt);
                        dt_tempdt.Rows[0]["状态"] = 4;
                        da_tempdt.Update(dt_tempdt);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否确定完成当前生产指令", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                if (checkform != null)
                    checkform.Dispose();
                checkform = new mySystem.CheckForm(this);
                checkform.FormClosed += new FormClosedEventHandler(checkform_FormClosed);
                checkform.ShowDialog();
            }
        }


        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产指令"] = mySystem.Parameter.proInstruction;
            dr["清场前产品代码"] =code;
            dr["清场前产品批号"] = batch;
            dr["清场日期"] = DateTime.Now;
            dr["检查结果"]=false;
            dr["清场人"] = mySystem.Parameter.userName;
            dr["审核时间"] = DateTime.Now;

            dr["操作员备注"] = "无";
            dr["检查人"] = "";
            dr["审核人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编号：" + mySystem.Parameter.proInstruction + "\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值 datagridview1
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["吹膜工序清场记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["清场操作"] = "合格";
            return dr;
        }

        // 给内表的一行写入默认值 datagridview2
        DataRow writeInnerDefault2(DataRow dr)
        {
            dr["吹膜工序清场记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["清场操作"] = "合格";
            return dr;
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("吹膜工序清场记录");
            bs_prodinstr = new BindingSource();

            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序清场记录 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
                da_prodinstr.Fill(dt_prodinstr);
            }
            else
            {
                da_prodinstr_sql = new SqlDataAdapter("select * from 吹膜工序清场记录 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                cb_prodinstr_sql = new SqlCommandBuilder(da_prodinstr_sql);
                da_prodinstr_sql.Fill(dt_prodinstr);
            }

        }
        // 根据条件从数据库中读取多行内表数据,datagridview1,对应供料清场项目
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜工序清场项目记录");
            bs_prodlist = new BindingSource();

            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist = new OleDbDataAdapter("select * from 吹膜工序清场项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
                cb_prodlist = new OleDbCommandBuilder(da_prodlist);
                da_prodlist.Fill(dt_prodlist);
            }
            else
            {
                da_prodlist_sql = new SqlDataAdapter("select * from 吹膜工序清场项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.conn);
                cb_prodlist_sql = new SqlCommandBuilder(da_prodlist_sql);
                da_prodlist_sql.Fill(dt_prodlist);
            }

            if (dt_prodlist.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_供料设置.Rows)
                {
                    DataRow ndr = dt_prodlist.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场要点"] = dr["清场内容"];
                    ndr["清场操作"] ="合格";
                    dt_prodlist.Rows.Add(ndr);
                }
            }
        }

        void readInnerData2(int id)
        {
            dt_prodlist2 = new DataTable("吹膜工序清场吹膜工序项目记录");
            bs_prodlist2 = new BindingSource();

            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist2 = new OleDbDataAdapter("select * from 吹膜工序清场吹膜工序项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
                cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
                da_prodlist2.Fill(dt_prodlist2);
            }
            else
            {
                da_prodlist2_sql = new SqlDataAdapter("select * from 吹膜工序清场吹膜工序项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.conn);
                cb_prodlist2_sql = new SqlCommandBuilder(da_prodlist2_sql);
                da_prodlist2_sql.Fill(dt_prodlist2);
            }

            if (dt_prodlist2.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_吹膜设置.Rows)
                {
                    DataRow ndr = dt_prodlist2.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场要点"] = dr["清场内容"];
                    ndr["清场操作"] = "合格";
                    dt_prodlist2.Rows.Add(ndr);
                }
            }
        }

        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb生产指令.DataBindings.Clear();
            //tb产品代码.DataBindings.Clear();
            //tb产品批号.DataBindings.Clear();
            dtp清场日期.DataBindings.Clear();

            tb清场人.DataBindings.Clear();
            tb检查人.DataBindings.Clear();

            tb备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Clear();


        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令");
            //tb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品代码");
            //tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品批号");
            tb清场人.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场人");
            tb检查人.DataBindings.Add("Text", bs_prodinstr.DataSource, "检查人");
            dtp清场日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "清场日期");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
            tb操作员备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "操作员备注");

        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewCombox();
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
            setDataGridViewRowNums();
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
            
        }

        void innerBind2()
        {
            //移除所有列
            while (dataGridView2.Columns.Count > 0)
                dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            setDataGridViewCombox2();
            bs_prodlist2.DataSource = dt_prodlist2;
            dataGridView2.DataSource = bs_prodlist2.DataSource;
            setDataGridViewColumns2();
            setDataGridViewRowNums2();
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清场操作":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("合格");
                        c1.Items.Add("不合格");
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }

        void setDataGridViewCombox2()
        {
            foreach (DataColumn dc in dt_prodlist2.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清场操作":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("合格");
                        c1.Items.Add("不合格");
                        dataGridView2.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c2);
                        break;
                }
            }
        }

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }

        void setDataGridViewColumns2()
        {
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;

        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }
        void setDataGridViewRowNums2()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            (new Record_extrusSiteClean(mainform, 1)).Show();
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }

        //根据生产指令id转换成生产指令编码
        private string idtocode(int id)
        {
            string ret = "";

            DataTable dt_temp = new DataTable("生产指令表");
            BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 生产指令信息表 where ID=" + id, mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 生产指令信息表 where ID=" + id, mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
            }

            if (dt_temp.Rows.Count > 0)
            {
                ret = dt_temp.Rows[0]["生产指令编号"].ToString();
            }
            return ret;
        }

        public int print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/A/SOP-MFG-301-R11 吹膜工序清场记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            string str_instruction = idtocode(instrid);
            my.PageSetup.RightFooter = str_instruction + "-11-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                {
                    label_打印成功 = 0;
                }
                finally
                {
                    if (1 == label_打印成功)
                    {
                        string log = "\n=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;
                        bs_prodinstr.EndEdit();
                        if (!mySystem.Parameter.isSqlOk)
                            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                        else
                            da_prodinstr_sql.Update((DataTable)bs_prodinstr.DataSource);

                    }
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
                return pageCount;
            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            int index = 11;//吹膜起始行号
            if (dataGridView1.Rows.Count > 6)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 6; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[6, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                index += dataGridView1.Rows.Count - 6;
            }

            if (dataGridView2.Rows.Count > 8)
            {
                //在第index+1行插入
                for (int i = 0; i < dataGridView2.Rows.Count - 8; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[index + 1, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
            }
            

            my.Cells[3, 1].Value = "生产指令："+mySystem.Parameter.proInstruction;
            //my.Cells[3, 3].Value = "清场前产品代码及批号：" + tb产品代码.Text + "  " + tb产品批号.Text ;
            my.Cells[3, 5].Value = "清场日期：" + dtp清场日期.Value.ToString("yyyy年MM月dd日");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[5 + i, 2].Value = i+1;
                my.Cells[5 + i, 3].Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 4].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
            }
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                my.Cells[index + i, 2].Value = i + 1;
                my.Cells[index + i, 3].Value = dataGridView2.Rows[i].Cells[3].Value.ToString();
                my.Cells[index + i, 4].Value = dataGridView2.Rows[i].Cells[4].Value.ToString();
            }
            my.Cells[5, 5].Value = tb清场人.Text;
            my.Cells[5, 6].Value = ckb合格.Checked==true?"合格":"不合格";
            my.Cells[5, 7].Value = tb检查人.Text;
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                List<int> list_id = new List<int>();
                string asql = "select * from 吹膜工序清场记录 where 生产指令ID=" + instrid;
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                DataTable tempdt = new DataTable();
                da.Fill(tempdt);

                for (int i = 0; i < tempdt.Rows.Count; i++)
                    list_id.Add((int)tempdt.Rows[i]["ID"]);
                return list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]) + 1;
            }
            else
            {
                List<int> list_id = new List<int>();
                string asql = "select * from 吹膜工序清场记录 where 生产指令ID=" + instrid;
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataTable tempdt = new DataTable();
                da.Fill(tempdt);

                for (int i = 0; i < tempdt.Rows.Count; i++)
                    list_id.Add((int)tempdt.Rows[i]["ID"]);
                return list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]) + 1;
            }


        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //不合格标红，合格标白
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "不合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void setUndoColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "合格")
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "不合格")
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_prodinstr.Rows[0]["日志"].ToString()).Show();
        }

        private void bt提交审核_Click(object sender, EventArgs e)
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "不合格")
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();

            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜工序清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜工序清场记录";
                    dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='吹膜工序清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜工序清场记录";
                    dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);
            }



            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows[0]["检查人"] = "__待审核";
            dt_prodinstr.Rows[0]["审核时间"] = DateTime.Now;
            dt_prodinstr.Rows[0]["审核人"] = "__待审核";

            save();

            //空间都不能点
            setControlFalse();
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //不合格标红，合格标白
            if (e.ColumnIndex == 4)
            {
                if (dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString() == "合格")
                    dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString() == "不合格")
                    dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜工序清场记录'", mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='吹膜工序清场记录'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind1)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind1 = false;
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind2)
            {
                readDGVWidthFromSettingAndSet(dataGridView2);
                isFirstBind2 = false;
            }
        }

        private void Record_extrusSiteClean_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
            if (dataGridView2.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView2);
            }
        }
    }
}
