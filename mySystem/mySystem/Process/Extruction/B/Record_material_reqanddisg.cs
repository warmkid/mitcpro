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
using System.Text.RegularExpressions;

namespace mySystem.Extruction.Process
{
    public partial class Record_material_reqanddisg : mySystem.BaseForm
    {
        //private int instrid;
        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        CheckForm checkform;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;
        string matcode;

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

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

        //判断内表是否完全审核
        bool is_inner_checked()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() == "" || dataGridView1.Rows[i].Cells["审核人"].Value.ToString()=="__待审核")
                {
                    return false;
                }
            }
            return true;
        }

        void setEnableReadOnly()
        {
            //if (stat_user == 2)//管理员
            //{
            //    //控件都能点  TODO:控件状态*********************************
            //    setControlTrue();
            //}
            //else if (stat_user == 1)//审核人
            //{
            //    if (stat_form == 3 || stat_form == 2)//审核不通过，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();
            //    }
            //    else if (stat_form == 0)//草稿
            //    {
            //        //其他控件不能点，内表审核可点
            //        setControlFalse();
            //        bt领料审核.Enabled = true;
            //    }
            //    else//待审核
            //    {
            //        //发送审核，提交领料审核不可点，其他都可点
            //        setControlTrue();
            //        bt退料审核.Enabled = true;
            //        bt领料审核.Enabled = true;
            //    }

            //    dataGridView1.ReadOnly = false;
            //    //遍历datagridview，如果有一行为未审核，则该行可以修改
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() == "__待审核")
            //            dataGridView1.Rows[i].ReadOnly = false;
            //        else
            //            dataGridView1.Rows[i].ReadOnly = true;
            //    }

            //}
            //else//操作员
            //{
            //    if (stat_form == 1 || stat_form == 2)//待审核，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();

            //        cB物料代码.Enabled = true;

            //    }
            //    else//未审核与审核不通过
            //    {
            //        //发送审核，审核不能点
            //        setControlTrue();
            //        bt领料提交审核.Enabled = true;
            //    }
            //    dataGridView1.ReadOnly = false;
            //    //遍历datagridview，如果有一行为未审核，则该行可以修改
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")
            //            dataGridView1.Rows[i].ReadOnly = true;
            //        else
            //            dataGridView1.Rows[i].ReadOnly = false;
            //    }
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
                    bt退料审核.Enabled = true;
                    bt领料审核.Enabled = true;

                    bt添加.Enabled = false;
                }
                else if (Parameter.FormState.未保存 == _formState)
                {
                    setControlFalse();
                    bt领料审核.Enabled = true;
                }
                else setControlFalse();

                dataGridView1.ReadOnly = false;
                //遍历datagridview，如果有一行为未审核，则该行可以修改
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() == "__待审核")
                    {
                        dataGridView1.Rows[i].ReadOnly = false;
                        dataGridView1.Rows[i].Cells[5].ReadOnly = true;//重量
                    }
                        
                    else
                        dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState)
                {
                    setControlTrue();
                    bt领料提交审核.Enabled = true;
                }
                else
                {
                    //空间都不能点
                    setControlFalse();
                    cB物料代码.Enabled = true;
                }

                dataGridView1.ReadOnly = false;
                //遍历datagridview，如果有一行为未审核，则该行可以修改
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")
                        dataGridView1.Rows[i].ReadOnly = true;
                    else
                    {
                        dataGridView1.Rows[i].ReadOnly = false;
                        dataGridView1.Rows[i].Cells[5].ReadOnly = true;//重量

                    }
                        
                }
            }
        }

        private void addOtherEventHandler()
        {
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
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
            bt退料审核.Enabled = false;
            bt提交审核.Enabled = false;

            bt领料提交审核.Enabled = false;
            bt领料审核.Enabled = false;
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

        //通过原料代码查找原料id
        private int id_findby_matcode(string matcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select raw_material_id from raw_material where raw_material_code='" + matcode + "'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return int.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        public Record_material_reqanddisg(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fill_printer();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            setControlFalse();
            bt打印.Enabled = false;
            bt日志.Enabled = false;

            cB物料代码.Enabled = true;
            instrid = mySystem.Parameter.proInstruID;

            addOtherEventHandler();
            //init();
            //addmatcode();
        }

        void addDataEventHandler()
        {
            this.cB物料代码.SelectedIndexChanged += new System.EventHandler(this.cB物料代码_SelectedIndexChanged);
        }

        //获取设置中物料代码
        private void getOtherData()
        {
             addmatcode();
        }

        //// 获取操作员和审核员
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜工序领料退料记录'", mySystem.Parameter.connOle);
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

        public Record_material_reqanddisg(MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            fill_printer();
            getPeople();
            setUserState();

            setControlFalse();

            string asql = "select * from 吹膜工序领料退料记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            //TODO instrid matcode可以放在读外表函数内************************
            //cB物料代码.Text = tempdt.Rows[0]["物料代码"].ToString();
            matcode = tempdt.Rows[0]["物料代码"].ToString();
            instrid = (int)tempdt.Rows[0]["生产指令ID"];

            readOuterData(instrid, matcode);
            removeOuterBinding();
            outerBind();

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            cB物料代码.Enabled = false;

            addOtherEventHandler();

        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setEnableReadOnly();
        }

        //向combox中添加物料代码
        private void addmatcode()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令信息表 where ID="+mySystem.Parameter.proInstruID;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                cB物料代码.Items.Add(tempdt.Rows[i]["内外层物料代码"].ToString());
                cB物料代码.Items.Add(tempdt.Rows[i]["中层物料代码"].ToString());
            }
            da.Dispose();
            tempdt.Dispose();
            comm.Dispose();
        }

        private void init()
        {
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        //根据筛选条件查找id
        private int getid(int instrid,string matcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 吹膜工序领料退料记录 where 生产指令ID=" + instrid + " and 物料代码='"+matcode+"'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return (int)tempdt.Rows[0][0];
            }
        }

        //检查输入人是否合法
        private int queryid(string s)
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt提交审核.Enabled = true;
        }
        //保存内表和外表数据
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            readOuterData(instrid, matcode);
            
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            setUndoColor();
            return true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region 之前
            //int row = e.RowIndex, col = e.ColumnIndex;
            //if (row == -1)
            //    return;
            //if (row == dataGridView1.Rows.Count - 1)
            //    addblankrow();
            //if (checkBox1.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_1.Count)
            //    {
            //        //更新数据
            //        addtolist(list_1, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_1.Add(con);
            //        addtolist(list_1, row, col);
                    
            //    }
            //    return;
            //}

            //if (checkBox2.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_4.Count)
            //    {
            //        //更新数据
            //        addtolist(list_4, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_4.Add(con);
            //        addtolist(list_4, row, col);

            //    }
            //    return;
            //}

            //if (checkBox3.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_6.Count)
            //    {
            //        //更新数据
            //        addtolist(list_6, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_6.Add(con);
            //        addtolist(list_6, row, col);

            //    }
            //    return;
            //}

            //if (checkBox4.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_11.Count)
            //    {
            //        //更新数据
            //        addtolist(list_11, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_11.Add(con);
            //        addtolist(list_11, row, col);

            //    }
            //    return;
            //}
            #endregion
        }
        private void dataGridView1_Endedit(object sender, DataGridViewCellEventArgs e)
        {
            //重量计算
            if (e.ColumnIndex == 3)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                float b = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * b;
            }
            if (e.ColumnIndex == 4)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                float b = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * b;
            }
            //操作人
            if (e.ColumnIndex == 8)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[8].Value == null || dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString() == "")
                    return;
                int rt = mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("操作人ID不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[8].Value = "";
                }
                return;
            }

            //计算合计
            float sum_num = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                    sum_num += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            //tb重量.Text = sum_num.ToString();
            //tb数量.Text = sum_weight.ToString();
            dt_prodinstr.Rows[0]["重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["数量合计"] = sum_num;

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            if (e.ColumnIndex == 6)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "是" && dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString() == "合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }

            if (e.ColumnIndex == 7)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "是" && dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString() == "合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //添加行
        private void button3_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
            //da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            //readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
        }

        //审核
        public override void CheckResult()
        {

            //获得审核信息
            //dtp审批日期.Value = checkform.time;
            dt_prodinstr.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["审核日期"] = checkform.time;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            if (checkform.ischeckOk)
            {
                (this.Owner as ExtructionMainForm).InitBtn();
            }
            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜工序领料退料记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
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

            base.CheckResult();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //判断内表是否完全审核
            if (!is_inner_checked())
            {
                MessageBox.Show("领料没有完全审核，请先审核领料");
                return;
            }
            checkform = new CheckForm(this);
            checkform.Show();
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["物料代码"] = cB物料代码.Text;
            dr["领料日期"] = DateTime.Now;
            dr["审核是否通过"] = false;
            dr["重量合计"] = 0;
            dr["数量合计"] = 0;
            dr["退料"] = 0;
            dr["退料操作人"] = mySystem.Parameter.userName;
            dr["审核日期"] = DateTime.Now;

            dr["退料审核人"] = "";
            dr["审核人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编号：" + mySystem.Parameter.proInstruction + "\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T吹膜工序领料退料记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["领料日期"] = DateTime.Now;
            dr["数量"] = 0;
            dr["重量每件"] = 0;
            dr["重量"] = 0;
            dr["包装完好"] = "是";
            dr["清洁合格"] = "合格";
            dr["操作人"] = mySystem.Parameter.userName;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid,string matcode)
        {
            dt_prodinstr = new DataTable("吹膜工序领料退料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where 生产指令ID=" + instrid+" and 物料代码='"+ matcode +"'", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜工序领料详细记录");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜工序领料详细记录 where T吹膜工序领料退料记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            cB物料代码.DataBindings.Clear();
            tb重量.DataBindings.Clear();
            tb数量.DataBindings.Clear();
            tb退料量.DataBindings.Clear();
            tb退料操作人.DataBindings.Clear();
            tb退料审核人.DataBindings.Clear();
            tb退料操作员备注.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            cB物料代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "物料代码");
            tb重量.DataBindings.Add("Text", bs_prodinstr.DataSource, "重量合计");
            tb数量.DataBindings.Add("Text", bs_prodinstr.DataSource, "数量合计");
            tb退料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料");
            tb退料操作人.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料操作人");
            tb退料审核人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审核人");
            tb退料操作员备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料操作员备注");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            while (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            }
            setDataGridViewColumns();

            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewRowNums();
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "包装完好":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("是");
                        c1.Items.Add("否");
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    case "清洁合格":
                        DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = dc.ColumnName;
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c3.Items.Add("合格");
                        c3.Items.Add("不合格");
                        dataGridView1.Columns.Add(c3);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    case "数量":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = "数量(件)";
                        c4.Name = dc.ColumnName;
                        c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c4.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c4);
                        break;

                    case "重量每件":
                        DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
                        c5.DataPropertyName = dc.ColumnName;
                        c5.HeaderText = "kg/件";
                        c5.Name = dc.ColumnName;
                        c5.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c5.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c5);
                        break;

                    case "重量":
                        DataGridViewTextBoxColumn c6 = new DataGridViewTextBoxColumn();
                        c6.DataPropertyName = dc.ColumnName;
                        c6.HeaderText = "重量(kg)";
                        c6.Name = dc.ColumnName;
                        c6.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c6.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c6);
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
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}

            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜工序领料退料记录ID
            dataGridView1.Columns[5].ReadOnly = true;//重量
            dataGridView1.Columns[9].ReadOnly = true;//领料审核人

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")//审核过或者待审核
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[8].Value == null || dataGridView1.Rows[i].Cells[8].Value.ToString() == "")
                {
                    MessageBox.Show("操作人不能为空");
                    return false;
                }                
            }
            return true;
        }
        private void cB物料代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            setControlTrue();
            matcode = cB物料代码.Text;
            readOuterData(instrid,matcode);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                return;
            }
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid, matcode);
                removeOuterBinding();
                outerBind();
            }
            instrid = mySystem.Parameter.proInstruID;
            matcode = cB物料代码.Text;
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            if (dataGridView1.Rows.Count == 0)
            {
                dt_prodinstr.Rows[0]["重量合计"] = 0;
                dt_prodinstr.Rows[0]["数量合计"] = 0;

                tb重量.Text = "0";
                tb数量.Text = "0";
            }

            setFormState();
            setEnableReadOnly();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            (new Record_material_reqanddisg(mainform, 4)).Show();
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
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from 吹膜工序领料退料记录 where 生产指令ID=" + instrid;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]) + 1;

        }

        public void print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/B/SOP-MFG-301-R14 吹膜工序领料退料记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = mySystem.Parameter.proInstruction + "--" + "步骤序号3--" + "表序号" + find_indexofprint() + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

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
                { label_打印成功 = 0; }
                finally
                {
                    if (1 == label_打印成功)
                    {
                        string log = "\n=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;
                        bs_prodinstr.EndEdit();
                        da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
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
            int ind = 0;//偏移
            if (dataGridView1.Rows.Count > 24)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 24; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[6, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView1.Rows.Count - 24;
            }

            my.Cells[3, 1].Value = "物料代码："+cB物料代码.Text;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime tempdt = DateTime.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                my.Cells[5 + i, 1] = tempdt.ToLongDateString();
                my.Cells[5 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 3] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[5 + i, 4] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                my.Cells[5 + i, 5] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[5 + i, 6] = dataGridView1.Rows[i].Cells[7].Value.ToString();
                my.Cells[5 + i, 7] = dataGridView1.Rows[i].Cells[8].Value.ToString();
                my.Cells[5 + i, 8] = dataGridView1.Rows[i].Cells[9].Value.ToString();
            }

            my.Cells[29+ind, 2].Value = tb数量.Text;
            my.Cells[29+ind, 4].Value = tb重量.Text;
            my.Cells[29+ind, 5].Value = "退料："+tb退料量.Text;
            my.Cells[29+ind, 7].Value = tb退料操作人.Text;
            my.Cells[29+ind, 8].Value = tb退料审核人.Text;
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["审核人"].Value.ToString() != "")
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            //计算合计
            float sum_num = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                    sum_num += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            //tb重量.Text = sum_num.ToString();
            //tb数量.Text = sum_weight.ToString();
            dt_prodinstr.Rows[0]["重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["数量合计"] = sum_num;
        }

        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;

            //设置readonly
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;

            //设置readonly
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
        }

        private void setUndoColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "是" && dataGridView1.Rows[i].Cells[7].Value.ToString() == "合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }

        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_prodinstr.Rows[0]["日志"].ToString()).Show();
        }

        private void bt提交审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "否" || dataGridView1.Rows[i].Cells[7].Value.ToString() == "不合格")
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }

            //判断内表审核人是否有空值
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[9].Value.ToString() == "")
                {
                    MessageBox.Show("未提交领料审核");
                    return;
                }
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜工序领料退料记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "吹膜工序领料退料记录";
                dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
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
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows[0]["审核人"] = "__待审核";
            dt_prodinstr.Rows[0]["审核日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();

            //可以处理其他物料情况
            cB物料代码.Enabled = true;
        }

        //遍历DataGridView的行：只要审核人不为空，则该行ReadOnly
        void setDataGridViewColumnReadOnly()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() != "")
            //        dataGridView1.Rows[i].ReadOnly = true;
            //}
        }

        private void bt领料审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() == "__待审核")
                {
                    //dataGridView1.Rows[i].Cells["审核人"].Value = person_审核员;
                    dt_prodlist.Rows[i]["审核人"] = mySystem.Parameter.userName;
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            bs_prodlist.DataSource = dt_prodlist;
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
        }

        private void bt领料提交审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].Value.ToString() == "")
                {
                    //dataGridView1.Rows[i].Cells["审核人"].Value = "__待审核";
                    dt_prodlist.Rows[i]["审核人"] = "__待审核";
                    dataGridView1.Rows[i].ReadOnly = true;
                }                    
            }
            bs_prodlist.DataSource = dt_prodlist;
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
        }
    }
}
