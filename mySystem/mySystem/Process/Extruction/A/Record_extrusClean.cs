using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using mySystem;

namespace WindowsFormsApplication1
{
    //由生产指令唯一确定该表
    public partial class Record_extrusClean : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access

        mySystem.CheckForm checkform;//审核信息
        private DataTable dt;//之前清洁项目

        private DataTable dt_out, dt_in;
        private OleDbDataAdapter da_out, da_in;
        private BindingSource bs_out, bs_in;
        private OleDbCommandBuilder cb_out, cb_in;

        private SqlDataAdapter da_out_sql, da_in_sql;
        private SqlCommandBuilder cb_out_sql, cb_in_sql;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;
        string instr;

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        public Record_extrusClean(mySystem.MainForm mainform)
            : base(mainform)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                conn = mainform.conn;
                connOle = mainform.connOle;
                isSqlOk = mainform.isSqlOk;

                InitializeComponent();
                Init();
                getPeople();
                setUserState();
                getOtherData();

                begin();
                instrid = Convert.ToInt32(dt_out.Rows[0]["生产指令ID"]);
                String asql = "select * from 生产指令信息表 where ID=" + instrid;

                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                DataTable tempdt = new DataTable();
                da.Fill(tempdt);

                instr = tempdt.Rows[0]["生产指令编号"].ToString();
            }
            else
            {
                conn = mainform.conn;
                connOle = mainform.connOle;
                isSqlOk = mainform.isSqlOk;

                InitializeComponent();
                Init();
                getPeople();
                setUserState();
                getOtherData();

                begin();
                instrid = Convert.ToInt32(dt_out.Rows[0]["生产指令ID"]);
                String asql = "select * from 生产指令信息表 where ID=" + instrid;

                //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                //OleDbDataAdapter da = new OleDbDataAdapter(comm);
                //DataTable tempdt = new DataTable();
                //da.Fill(tempdt);

                //*****************改为sql连接***********************
                SqlCommand comm_sql = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da_sql = new SqlDataAdapter(comm_sql);
                DataTable tempdt = new DataTable();
                da_sql.Fill(tempdt);

                instr = tempdt.Rows[0]["生产指令编号"].ToString();
            }

        }

        public Record_extrusClean(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                conn = mainform.conn;
                connOle = mainform.connOle;
                isSqlOk = mainform.isSqlOk;

                InitializeComponent();
                Init();
                getPeople();
                setUserState();
                getOtherData();

                string asql = "select * from 吹膜机组清洁记录表 where ID=" + id;
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());


                asql = "select * from 生产指令信息表 where ID=" + instrid;
                comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                da = new OleDbDataAdapter(comm);
                tempdt = new DataTable();
                da.Fill(tempdt);
                instr = tempdt.Rows[0]["生产指令编号"].ToString();


                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                ckb白班.Checked = (bool)dt_out.Rows[0]["班次"];
                ckb夜班.Checked = !ckb白班.Checked;

                readInnerData((int)dt_out.Rows[0]["ID"]);
                innerBind();

                DialogResult result;
                if (!is_sameto_setting())
                {
                    result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)//保留当前设置
                    {
                        while (dataGridView1.Rows.Count > 0)
                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                        da_in.Update((DataTable)bs_in.DataSource);
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow ndr = dt_in.NewRow();
                            ndr[1] = (int)dt_out.Rows[0]["ID"];
                            // 注意ID不要复制过去了，所以从1开始
                            for (int i = 1; i < dr.Table.Columns.Count; ++i)
                            {
                                ndr[i + 1] = dr[i];
                            }
                            // 这里添加检查是否合格、检查人、审核人等默认信息
                            ndr["合格"] = "合格";
                            ndr["清洁人"] = mySystem.Parameter.userName;
                            ndr["检查人"] = "";
                            ndr["清洁员备注"] = "";
                            dt_in.Rows.Add(ndr);
                        }
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        //da_in.Update((DataTable)bs_in.DataSource);

                    }
                }

                setFormState();
                setEnableReadOnly();
            }
            else
            {
                conn = mainform.conn;
                connOle = mainform.connOle;
                isSqlOk = mainform.isSqlOk;

                InitializeComponent();
                Init();
                getPeople();
                setUserState();
                getOtherData();

                string asql = "select * from 吹膜机组清洁记录表 where ID=" + id;
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());


                asql = "select * from 生产指令信息表 where ID=" + instrid;
                comm = new SqlCommand(asql, mySystem.Parameter.conn);
                da = new SqlDataAdapter(comm);
                tempdt = new DataTable();
                da.Fill(tempdt);
                instr = tempdt.Rows[0]["生产指令编号"].ToString();


                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                ckb白班.Checked = (bool)dt_out.Rows[0]["班次"];
                ckb夜班.Checked = !ckb白班.Checked;

                readInnerData((int)dt_out.Rows[0]["ID"]);
                innerBind();

                DialogResult result;
                if (!is_sameto_setting())
                {
                    result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)//保留当前设置
                    {
                        while (dataGridView1.Rows.Count > 0)
                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                        da_in.Update((DataTable)bs_in.DataSource);
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow ndr = dt_in.NewRow();
                            ndr[1] = (int)dt_out.Rows[0]["ID"];
                            // 注意ID不要复制过去了，所以从1开始
                            for (int i = 1; i < dr.Table.Columns.Count; ++i)
                            {
                                ndr[i + 1] = dr[i];
                            }
                            // 这里添加检查是否合格、检查人、审核人等默认信息
                            ndr["合格"] = "合格";
                            ndr["清洁人"] = mySystem.Parameter.userName;
                            ndr["检查人"] = "";
                            ndr["清洁员备注"] = "";
                            dt_in.Rows.Add(ndr);
                        }
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        //da_in.Update((DataTable)bs_in.DataSource);

                    }
                }

                setFormState();
                setEnableReadOnly();
            }

        }

        //获取设置清洁项目
        private void getOtherData()
        {
            readset();
            fill_printer();
        }
        //// 获取操作员和审核员
        void getPeople()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                list_操作员 = new List<string>();
                list_审核员 = new List<string>();
                DataTable dt = new DataTable("用户权限");

                OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜机组清洁记录'", mySystem.Parameter.connOle);

                da.Fill(dt);

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
            else
            {
                list_操作员 = new List<string>();
                list_审核员 = new List<string>();
                DataTable dt = new DataTable("用户权限");


                //OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜机组清洁记录'", mySystem.Parameter.connOle);

                //*****************改为sql连接***********************
                SqlDataAdapter da = new SqlDataAdapter(@"select * from 用户权限 where 步骤='吹膜机组清洁记录'", mySystem.Parameter.conn);
                da.Fill(dt);

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

        // 如果给了一个参数为true，则表示处于无数据状态
        void setFormState()
        {
            string s = dt_out.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dt_out.Rows[0]["审核是否通过"]);
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
            //if (Parameter.FormState.无数据 == _formState)
            //{
            //    setControlFalse();
            //    btn查询插入.Enabled = true;
            //    tb生产指令编号.ReadOnly = false;
            //    return;
            //}
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
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
        }

        //读取设置里面的清洁内容
        private void readset()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                string asql = "select * from 设置吹膜机组清洁项目";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);
                da.Fill(dt);
            }
            else
            {
                string asql = "select * from 设置吹膜机组清洁项目";
                SqlCommand comm_sql = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da_sql = new SqlDataAdapter(comm_sql);
                da_sql.Fill(dt);
            }

                   
        }
        //判断之前的内容是否与设置表中内容一致
        private bool is_sameto_setting()
        {
            if (dt.Rows.Count != dt_in.Rows.Count)
                return false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["清洁区域"].ToString() != dt_in.Rows[i]["清洁区域"].ToString() || dt.Rows[i]["清洁内容"].ToString() != dt_in.Rows[i]["清洁内容"].ToString())
                    return false;
            }
            return true;
        }
        private void begin()
        {
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();
            if (dt_out.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                return;
            }

            if (dt_out.Rows.Count <= 0)
            {
                //新建记录
                DataRow dr = dt_out.NewRow();
                dr = writeOuterDefault(dr);
                dt_out.Rows.Add(dr);
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(mySystem.Parameter.proInstruID);
                removeOuterBinding();
                outerBind();
                instrid = mySystem.Parameter.proInstruID;

            }
            instrid = mySystem.Parameter.proInstruID;

            ckb白班.Checked = (bool)dt_out.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            readInnerData((int)dt_out.Rows[0]["ID"]);
            innerBind();

            DialogResult result;
            if (!is_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {                  
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    da_in.Update((DataTable)bs_in.DataSource);
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow ndr = dt_in.NewRow();
                        ndr[1] = (int)dt_out.Rows[0]["ID"];
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 1; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 1] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["合格"] = "合格";
                        ndr["清洁人"] = mySystem.Parameter.userName;
                        ndr["检查人"] = "";
                        ndr["清洁员备注"] = "";
                        dt_in.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);

                }

            }
            setFormState();
            setEnableReadOnly();
        }
        //初始化
        private void Init()
        {
            dataGridView1.Font = new Font("宋体", 10);
            bt审核.Enabled = false;
            tb复核人.Enabled = false;

            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;
            dtp复核日期.Enabled = false;
            dt = new DataTable();

            connOle = mySystem.Parameter.connOle;
            dt_out = new DataTable();
            dt_in = new DataTable();

            da_out = new OleDbDataAdapter();
            da_in = new OleDbDataAdapter();

            da_out_sql = new SqlDataAdapter();
            da_in_sql = new SqlDataAdapter();

            bs_out = new BindingSource();
            bs_in = new BindingSource();

            cb_out = new OleDbCommandBuilder();
            cb_in = new OleDbCommandBuilder();

            cb_out_sql = new SqlCommandBuilder();
            cb_in_sql = new SqlCommandBuilder();
            
        }

       //供界面显示,参数为数据库中对应记录的id，------------------------作废的函数-----------------------
        public void show(int paraid)
        {
            string asql = "select 生产指令ID from 吹膜机组清洁记录表 where ID=" + paraid;
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(comm);
            //DataTable tempdt = new DataTable();
            //da.Fill(tempdt);

            //*****************改为sql连接***********************
            SqlCommand comm_sql = new SqlCommand(asql, mySystem.Parameter.conn);
            SqlDataAdapter da_sql = new SqlDataAdapter(comm_sql);
            DataTable tempdt = new DataTable();
            da_sql.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return;
            else
                fill_by_id((int)tempdt.Rows[0][0]);
        }

        //根据生产指令id将数据填写到各控件中,------------------------作废的函数-----------------------
        private int fill_by_id(int id)
        {
            #region 以前
            //string asql = "select s1_clean_date,s1_flight,s1_reviewer_id,s1_review_date,s1_region_result_cleaner_reviewer from extrusion_s1_cleanrecord where production_instruction="  + id ;
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //DataTable tempdt = new DataTable();
            //da.Fill(tempdt);
            //if (tempdt.Rows.Count == 0)
            //    return -1;
            //else
            //{
            //    //将tempdt填入控件
            //    dateTimePicker1.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());
            //    //comboBox1.Text = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? "白班" : "夜班";
            //    checkBox1.Checked = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? true : false;
            //    checkBox2.Checked = !checkBox1.Checked;
            //    string rev = tempdt.Rows[0][2].ToString();
            //    if (rev == "")
            //        button2.Enabled = true;
            //    else
            //    {
            //        button1.Enabled = false;
            //        button2.Enabled = false;
            //        textBox2.Text = mySystem.Parameter.IDtoName(int.Parse(rev));
            //        dateTimePicker2.Value = DateTime.Parse(tempdt.Rows[0][3].ToString());
            //        dataGridView1.ReadOnly = true;
            //        dateTimePicker1.Enabled = false;
            //        dateTimePicker2.Enabled = false;
            //    }

            //    string jstr = tempdt.Rows[0][4].ToString();
            //    JArray jarray = JArray.Parse(jstr);
            //    for (int i = 0; i < jarray.Count; i++)
            //    {
            //        JObject jobj = JObject.Parse(jarray[i].ToString());
            //        foreach (var p in jobj)
            //        {
            //            DataGridViewRow dr = new DataGridViewRow();
            //            dataGridView1.Rows.Add(dr);
            //            dataGridView1.Rows[i].Cells[0].Value = p.Key;//名称
            //            dataGridView1.Rows[i].Cells[1].Value =cont_findby_name( p.Key);//内容
            //            if (int.Parse(jobj[p.Key][0].ToString()) == 1)
            //            {
            //                //白班
            //                dataGridView1.Rows[i].Cells[2].Value = "True";
            //                dataGridView1.Rows[i].Cells[3].Value = "False";
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[i].Cells[3].Value = "True";
            //                dataGridView1.Rows[i].Cells[2].Value = "False";
            //            }
            //            dataGridView1.Rows[i].Cells[4].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][1].ToString()));
            //            dataGridView1.Rows[i].Cells[5].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][2].ToString()));
            //        }
            //    }
            //}
            //return 0;
            #endregion

            string asql = "select 清洁日期,班次,审核人,审核是否通过,审核时间,ID from 吹膜机组清洁记录表 where 生产指令ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
            {
                //将tempdt填入控件
                dtp清洁日期.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());//清洁日期
                ckb白班.Checked = tempdt.Rows[0][1].ToString()=="True";//班次
                ckb夜班.Checked = !ckb白班.Checked;

                bool revisok = (bool)tempdt.Rows[0][3];//审核是否通过

                if (!revisok)//审核未通过
                    bt审核.Enabled = true;
                else
                {
                    bt保存.Enabled = false;
                    bt审核.Enabled = false;
                    bt打印.Enabled = true;
                    tb复核人.Text = tempdt.Rows[0][2].ToString();
                    dtp复核日期.Value = (DateTime)tempdt.Rows[0][4];
                    dataGridView1.ReadOnly = true;
                    dtp清洁日期.Enabled = false;
                    dtp复核日期.Enabled = false;
                }
                //填写表格
                int tempid = (int)tempdt.Rows[0][5];
                comm.CommandText = "select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + tempid;
                da.Dispose();
                tempdt.Dispose();
                da = new OleDbDataAdapter(comm);
                tempdt = new DataTable();
                da.Fill(tempdt);//获得数据

                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dataGridView1.Rows.Add(dr);
                    dataGridView1.Rows[i].Cells[0].Value = (string)tempdt.Rows[i][2];//名称
                    dataGridView1.Rows[i].Cells[1].Value = (string)tempdt.Rows[i][3];//内容
                    dataGridView1.Rows[i].Cells[2].Value = (string)tempdt.Rows[i][4]=="合格"?"合格":"不合格";//合格 是
                    dataGridView1.Rows[i].Cells[3].Value = (string)tempdt.Rows[i][4] == "合格" ? "不合格" : "合格";
                    dataGridView1.Rows[i].Cells[4].Value = (string)tempdt.Rows[i][6];
                    dataGridView1.Rows[i].Cells[5].Value = (string)tempdt.Rows[i][7];
                }
                return 0;
            }

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (this.dataGridView1.IsCurrentCellDirty)
            //{

            //    this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //}
        }

        //单元格编辑结束触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //更改清洁人项
            if (e.ColumnIndex == 5)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[5].Value == null || dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "")
                    return;
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("清洁员id不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = "";
                }
                return;
            }

            //不合格标红，合格标白
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString()=="合格" )
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "不合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }
            
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                string asql = "select ID from users where 姓名=" + "'" + s + "'";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOleUser);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return (int)tempdt.Rows[0][0];
            }
            else
            {
                string asql = "select ID from users where 姓名=" + "'" + s + "'";
                //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOleUser);
                //OleDbDataAdapter da = new OleDbDataAdapter(comm);

                //*****************改为sql连接***********************
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.connUser);
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return (int)tempdt.Rows[0][0];
            }

        }

        private bool input_Judge()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "")
                {
                    MessageBox.Show("清洁员不能为空");
                    return false;
                }
            }
            return true;
        }

        //保存内表和外表数据
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
                bs_out.EndEdit();
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                //内表保存
                da_in.Update((DataTable)bs_in.DataSource);
                readInnerData(Convert.ToInt32(dt_out.Rows[0]["ID"]));
                innerBind();
            }
            else
            {
                //外表保存
                bs_out.EndEdit();
                da_out_sql.Update((DataTable)bs_out.DataSource);
                readOuterData(instrid);
                removeOuterBinding();
                outerBind();

                //内表保存
                da_in_sql.Update((DataTable)bs_in.DataSource);
                readInnerData(Convert.ToInt32(dt_out.Rows[0]["ID"]));
                innerBind();
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
            try
            {
                (this.Owner as ExtructionMainForm).InitBtn();

            }
            catch (NullReferenceException exp)
            {

            }
                
        }

        //重写函数，获得审查人信息
        public override void CheckResult()
        {
            //获得审核信息
            dt_out.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt_out.Rows[0]["审核时间"] = checkform.time;
            dt_out.Rows[0]["审核意见"] = checkform.opinion;
            dt_out.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            
            //状态
            setControlFalse();

            if (!mySystem.Parameter.isSqlOk)
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜机组清洁记录表' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                //写日志
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
                log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
                log += "审核意见：" + checkform.opinion;
                dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

                bs_out.EndEdit();
                da_out.Update((DataTable)bs_out.DataSource);
                base.CheckResult();
                try
                {
                    (this.Owner as ExtructionMainForm).InitBtn();
                }
                catch (NullReferenceException exp)
                {

                }
            }
            else
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='吹膜机组清洁记录表' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                //写日志
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
                log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
                log += "审核意见：" + checkform.opinion;
                dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

                bs_out.EndEdit();
                //da_out.Update((DataTable)bs_out.DataSource);
                da_out_sql.Update((DataTable)bs_out.DataSource);
                base.CheckResult();
                try
                {
                    (this.Owner as ExtructionMainForm).InitBtn();
                }
                catch (NullReferenceException exp)
                {

                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //测试清场记录
        private void button4_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean s = new Record_extrusSiteClean(mainform);
            s.Show();
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] =mySystem.Parameter.proInstruID ;
            dr["清洁日期"] = DateTime.Now;
            dr["班次"] = mySystem.Parameter.userflight=="白班";//白班代表是
            dr["审核时间"] = DateTime.Now;
            dr["审核是否通过"] = false;
            ckb白班.Checked = (bool)dr["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            dr["审核人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编号：" + instr + "\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["吹膜机组清洁记录ID"] = dt_out.Rows[0]["ID"];
            dr["合格"] = "合格";
            dr["清洁人"] = mySystem.Parameter.userName;
            dr["清洁员备注"] = "无";
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dt_out = new DataTable("吹膜机组清洁记录表");
                bs_out = new BindingSource();

                da_out = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where 生产指令ID=" +instrid, connOle);
                cb_out = new OleDbCommandBuilder(da_out);
                da_out.Fill(dt_out);

            }
            else
            {
                dt_out = new DataTable("吹膜机组清洁记录表");
                bs_out = new BindingSource();

                //da_out = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where 生产指令ID=" +instrid, connOle);
                //cb_out = new OleDbCommandBuilder(da_out);
                //da_out.Fill(dt_out);

                //*****************改为sql连接***********************
                da_out_sql = new SqlDataAdapter("select * from 吹膜机组清洁记录表 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                cb_out_sql = new SqlCommandBuilder(da_out_sql);
                da_out_sql.Fill(dt_out);
            }



        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dt_in = new DataTable("吹膜机组清洁项目记录表");
                bs_in = new BindingSource();

                da_in = new OleDbDataAdapter("select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + id, connOle);
                cb_in = new OleDbCommandBuilder(da_in);
                da_in.Fill(dt_in);

                if (dt_in.Rows.Count <= 0)//空表，按照设置表内容进行插入
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow ndr = dt_in.NewRow();
                        ndr[1] = (int)dt_out.Rows[0]["ID"];
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 1; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 1] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["合格"] = "合格";
                        ndr["清洁人"] = mySystem.Parameter.userName;
                        ndr["检查人"] = "";
                        ndr["清洁员备注"] = "无";
                        dt_in.Rows.Add(ndr);
                    }
                }
            }
            else
            {
                dt_in = new DataTable("吹膜机组清洁项目记录表");
                bs_in = new BindingSource();

                //da_in = new OleDbDataAdapter("select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + id, connOle);
                //cb_in = new OleDbCommandBuilder(da_in);
                //da_in.Fill(dt_in);

                //*****************改为sql连接***********************
                da_in_sql = new SqlDataAdapter("select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + id, mySystem.Parameter.conn);
                cb_in_sql = new SqlCommandBuilder(da_in_sql);
                da_in_sql.Fill(dt_in);

                if (dt_in.Rows.Count <= 0)//空表，按照设置表内容进行插入
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow ndr = dt_in.NewRow();
                        ndr[1] = (int)dt_out.Rows[0]["ID"];
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 1; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 1] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["合格"] = "合格";
                        ndr["清洁人"] = mySystem.Parameter.userName;
                        ndr["检查人"] = "";
                        ndr["清洁员备注"] = "无";
                        dt_in.Rows.Add(ndr);
                    }
                }
            }

        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            dtp清洁日期.DataBindings.Clear();
            dtp复核日期.DataBindings.Clear();
            //ckb白班.DataBindings.Clear();
            tb复核人.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_out.DataSource = dt_out;

            tb复核人.DataBindings.Add("Text", bs_out.DataSource, "审核人");
            dtp清洁日期.DataBindings.Add("Value", bs_out.DataSource, "清洁日期");
            dtp复核日期.DataBindings.Add("Value", bs_out.DataSource, "审核时间");
            //ckb白班.DataBindings.Add("Checked", bs_out.DataSource,"班次");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewCombox();
            bs_in.DataSource = dt_in;
            dataGridView1.DataSource = bs_in.DataSource;
            setDataGridViewColumns();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_in.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "合格":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("合格");
                        c1.Items.Add("不合格");
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    case "清洁人":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = "清洁员";
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[6].Visible = false;
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}
        }

        private void Record_extrusClean_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            (new Record_extrusClean(mainform, 4)).Show();
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

        public void print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "/../../xls/Extrusion/A/SOP-MFG-301-R03 吹膜机组清洁记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];   
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = instr + "-03-" + "" + find_indexofprint().ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {                
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
                    if (1==label_打印成功)
                    {
                        string log = "\n=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;
                        bs_out.EndEdit();

                        if (!mySystem.Parameter.isSqlOk)
                        {
                            da_out.Update((DataTable)bs_out.DataSource);
                        }
                        else
                        {
                            da_out_sql.Update((DataTable)bs_out.DataSource);
                        }
                        
                    }
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

            }                       
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            if (dataGridView1.Rows.Count > 11)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 11;i++ )
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[6, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
            }

            my.Cells[3, 2].Value = dtp清洁日期.Value.ToString("yyyy年MM月dd日");
            my.Cells[3, 7].Value = ckb白班.Checked==true?"白班":"夜班";
            my.Cells[3, 11].Value = tb复核人.Text + "  " + dtp复核日期.Value.ToString("yyyy年MM月dd日");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[5 + i, 1] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                my.Cells[5 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 9] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[5 + i, 11] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                my.Cells[5 + i, 12] = dataGridView1.Rows[i].Cells[6].Value.ToString();
            }          
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                List<int> list_id = new List<int>();
                string asql = "select * from 吹膜机组清洁记录表 where 生产指令ID=" + instrid;
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);

                for (int i = 0; i < tempdt.Rows.Count; i++)
                    list_id.Add((int)tempdt.Rows[i]["ID"]);
                return list_id.IndexOf((int)dt_out.Rows[0]["ID"]) + 1;
            }
            else
            {
                List<int> list_id = new List<int>();
                string asql = "select * from 吹膜机组清洁记录表 where 生产指令ID=" + instrid;
                //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                //OleDbDataAdapter da = new OleDbDataAdapter(comm);

                //*****************改为sql连接***********************
                SqlCommand comm = new SqlCommand(asql, mySystem.Parameter.conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);

                for (int i = 0; i < tempdt.Rows.Count; i++)
                    list_id.Add((int)tempdt.Rows[i]["ID"]);
                return list_id.IndexOf((int)dt_out.Rows[0]["ID"]) + 1;
            }

        }

        private void setUndoColor()
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void bt提交审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }

            if (!mySystem.Parameter.isSqlOk)
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                BindingSource bs_temp = new BindingSource();

                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜机组清洁记录表' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);


                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜机组清洁记录表";
                    dr["对应ID"] = (int)dt_out.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);

                //写日志 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 提交审核
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
                dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

                dt_out.Rows[0]["审核人"] = "__待审核";
                dt_out.Rows[0]["审核时间"] = DateTime.Now;

                save();

                //空间都不能点
                setControlFalse();
            }
            else
            {
                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                BindingSource bs_temp = new BindingSource();

                //OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜机组清洁记录表' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
                //OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);

                //*****************改为sql连接***********************
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='吹膜机组清洁记录表' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);


                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜机组清洁记录表";
                    dr["对应ID"] = (int)dt_out.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);

                //写日志 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 提交审核
                string log = "\n=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
                dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

                dt_out.Rows[0]["审核人"] = "__待审核";
                dt_out.Rows[0]["审核时间"] = DateTime.Now;

                save();

                //空间都不能点
                setControlFalse();
            }

        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt_out.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_out.Rows[0]["日志"].ToString()).Show();
        }
    }
}
