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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace mySystem.Process.CleanCut
{
    public partial class Record_cleansite_cut : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist;//
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        DataTable dt_清场设置;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        //private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        private Dictionary<string, string> dict_prod;//产品代码与产品批号对应字典

        private int instrid;
        private string prodcode;
        public Record_cleansite_cut(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            
            getPeople();//清场记录权限
            setUserState();
            getOtherData();
            addDataEventHandler();

            instrid = mySystem.Parameter.cleancutInstruID;

            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            cb产品代码.Enabled = true;
            bt插入查询.Enabled = true;
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);



            //if (cb产品代码.Text == "")
            //{
            //    MessageBox.Show("选择一条产品代码");
            //    return;
            //}
            //if (_userState != Parameter.UserState.操作员)
            //{
            //    MessageBox.Show("只有操作员才能新建记录");
            //    return;
            //}
            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;

            instrid = mySystem.Parameter.cleancutInstruID;
            prodcode = cb产品代码.Text;
            readOuterData(instrid, prodcode);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid, prodcode);
                removeOuterBinding();
                outerBind();
            }

            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["生产班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            if (dt_prodinstr.Rows[0]["审核人"].ToString() != "" && dt_prodinstr.Rows[0]["审核人"].ToString() != "__待审核")
            {
                ckb合格.Checked = (bool)dt_prodinstr.Rows[0]["审核是否通过"];
                ckb不合格.Checked = !ckb合格.Checked;
            }
            else
            {
                ckb合格.Checked = false;
                ckb不合格.Checked = false;
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            addOtherEvnetHandler();
            setFormState();
            setEnableReadOnly();

            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;


        }

        public Record_cleansite_cut(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();

            getPeople();//清场记录权限
            setUserState();
            getOtherData();

            string asql = "select * from 清场记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());
            //prodcode = tempdt.Rows[0]["产品代码"].ToString();

            readOuterData(instrid, prodcode);
            removeOuterBinding();
            outerBind();

            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["生产班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            if (dt_prodinstr.Rows[0]["审核人"].ToString() != "" && dt_prodinstr.Rows[0]["审核人"].ToString() != "__待审核")
            {
                ckb合格.Checked = (bool)dt_prodinstr.Rows[0]["审核是否通过"];
                ckb不合格.Checked = !ckb合格.Checked;
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            addOtherEvnetHandler();
            setFormState();
            setEnableReadOnly();

            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        void addDataEventHandler()
        {
            //cb产品代码.SelectedIndexChanged += new EventHandler(cb产品代码_SelectedIndexChanged);
            //dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            }
        }

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

        //// 获取操作员和审核员
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='全部'", mySystem.Parameter.connOle);
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

        
        private void getOtherData()
        {
            //获取设置表中清场项目
            readsetting();

            //获取该生产指令下的产品代码 
            dict_prod = new Dictionary<string, string>();
            OleDbDataAdapter tda = new OleDbDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID="+mySystem.Parameter.cleancutInstruID, mySystem.Parameter.connOle);
            DataTable tdt = new DataTable("清洁分切工序生产指令详细信息");
            tda.Fill(tdt);
            //foreach (DataRow tdr in tdt.Rows)
            //{
            //    cb产品代码.Items.Add(tdr["清洁前产品代码"].ToString());
            //    dict_prod.Add(tdr["清洁前产品代码"].ToString(), tdr["清洁前批号"].ToString());
            //}

            //添加打印机
            fill_printer();
        }

        //判断之前的内容是否与设置表中内容一致
        private bool is_sameto_setting()
        {
            if (dt_清场设置.Rows.Count != dt_prodlist.Rows.Count)
                return false;
            for (int i = 0; i < dt_清场设置.Rows.Count; i++)
            {
                if (dt_清场设置.Rows[i]["清场内容"].ToString() != dt_prodlist.Rows[i]["清场内容"].ToString())
                    return false;
            }
            return true;
        }


        // 其他事件，比如按钮的点击，数据有效性判断
        void addOtherEvnetHandler()
        {
            //判断是否与设置中数据一致
            DialogResult result;
            if (!is_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                    int tempi = 1;
                    foreach (DataRow dr in dt_清场设置.Rows)
                    {
                        DataRow ndr = dt_prodlist.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr[2] = tempi++;//序号
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 1; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 2] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["清洁操作"] = "合格";

                        dt_prodlist.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;

                }

            }
        }

        // 设置自动计算类事件
        void addComputerEventHandler()
        { }

        //设置窗口状态
        void setFormState()
        {
            //if (dt_prodinstr.Rows[0]["审核人"].ToString() == "")//草稿
            //    stat_form = 0;
            //else if (dt_prodinstr.Rows[0]["审核人"].ToString() == "__待审核")//待审核
            //    stat_form = 1;
            //else if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])//审核通过
            //    stat_form = 2;
            //else//审核未通过
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

        //设置控件可见
        private void setControlTrue()
        {
            //控件都能点,除不可点控件
            foreach (Control c in this.Controls)
                c.Enabled = true;
            dataGridView1.ReadOnly = false;
            ckb不合格.Enabled = false;
            ckb合格.Enabled = false;
            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;
            bt发送审核.Enabled = false;
            bt审核.Enabled = false;
        }
        //设置控件不可见
        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt日志.Enabled = true;
            bt打印.Enabled = true;
            cb打印机.Enabled = true;
        }

        //设置控件可读性
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

        //读取设置中清场项目
        private void readsetting()
        {
            dt_清场设置 = new DataTable("设置清场项目");
            string asql = "select * from 设置清场项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            da.Fill(dt_清场设置);
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where ID="+instrid,mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            dr["生产指令编码"] = dt.Rows[0]["生产指令编号"];
            //dr["产品代码"] = prodcode;
            //dr["产品批号"] = dict_prod[prodcode];
            dr["生产日期"] = DateTime.Now;
            dr["生产班次"] = mySystem.Parameter.userflight=="白班"?true:false;
            dr["清场人"] = mySystem.Parameter.userName;
            dr["检查人"] = "";
            dr["审核人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编号：" + mySystem.Parameter.cleancutInstruction + "\n";
            dr["日志"] = log;
            return dr;

        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid,string prodcode)
        {
            dt_prodinstr = new DataTable("清场记录");
            bs_prodinstr = new BindingSource();
            //da_prodinstr = new OleDbDataAdapter("select * from 清场记录 where 生产指令ID=" + instrid + " and 产品代码='" + prodcode + "'", mySystem.Parameter.connOle);
            da_prodinstr = new OleDbDataAdapter("select * from 清场记录 where 生产指令ID=" + instrid , mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据,datagridview1,对应供料清场项目
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("清场记录详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 清场记录详细信息 where T清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            if (dt_prodlist.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_清场设置.Rows)
                {
                    DataRow ndr = dt_prodlist.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场内容"] = dr["清场内容"];
                    //ndr["清场要点"] = dr["清场要点"];
                    ndr["清洁操作"] = "合格";
                    dt_prodlist.Rows.Add(ndr);
                }
            }
        }

        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            cb产品代码.DataBindings.Clear();
            tb产品规格.DataBindings.Clear();
            tb产品批号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();

            tb清场人.DataBindings.Clear();
            tb检查人.DataBindings.Clear();
            tb备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Clear();

            lbl生产指令编码.DataBindings.Clear();


        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

            cb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品代码");
            tb产品规格.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品规格");
            tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品批号");
            tb清场人.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场人");
            tb检查人.DataBindings.Add("Text", bs_prodinstr.DataSource, "检查人");
            dtp生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "生产日期");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
            tb操作员备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "操作员备注");

            lbl生产指令编码.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编码");
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
            //setDataGridViewColumns();
            setDataGridViewRowNums();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清洁操作":
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

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//外键

        }

        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }

            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//外键
        }

        //保存外表和内表数据
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(instrid,prodcode);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            setUndoColor();
            return true;
        }

        //给不合格记录标记颜色
        private void setUndoColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        //保存按钮
        private void bt保存_Click(object sender, EventArgs e)
        {
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt发送审核.Enabled = true;
        }

        //判断合法性
        private bool input_Judge()
        {
            if (mySystem.Parameter.NametoID(tb清场人.Text) <= 0)
            {
                MessageBox.Show("操作员ID不存在");
                return false;
            }
            return true;
        }

        //审核按钮
        private void bt审核_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes == MessageBox.Show("是否确定完成当前生产指令", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                if (checkform != null)
                    checkform.Dispose();
                checkform = new mySystem.CheckForm(this);
                checkform.FormClosed += new FormClosedEventHandler(checkform_FormClosed);
                checkform.ShowDialog();
            }
            //checkform = new CheckForm(this);
            //checkform.Show();
        }

        void checkform_FormClosed(object sender, FormClosedEventArgs e)
        {
            //审核通过
            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                //日报表调用带ID的
                DataTable dt_日报表 = new DataTable("清洁分切日报表");
                OleDbDataAdapter da_日报表 = new OleDbDataAdapter("select * from 清洁分切日报表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_日报表 = new OleDbCommandBuilder(da_日报表);
                da_日报表.Fill(dt_日报表);
                int id_日报表 = (int)dt_日报表.Rows[0]["ID"];
                new DailyRecord(mainform, id_日报表);

                //result = MessageBox.Show("是否确定完成当前生产指令", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (true)
                {
                    DataTable dt_tempdt = new DataTable("清洁分切工序生产指令");
                    OleDbDataAdapter da_tempdt = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where ID=" + instrid, mySystem.Parameter.connOle);
                    OleDbCommandBuilder cb_prodinstr = new OleDbCommandBuilder(da_tempdt);
                    da_tempdt.Fill(dt_tempdt);

                    dt_tempdt.Rows[0]["状态"] = 4;
                    da_tempdt.Update(dt_tempdt);
                }
            }
        }

        //审核
        public override void CheckResult()
        {
            dt_prodinstr.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["检查人"] = mySystem.Parameter.userName;

            dt_prodinstr.Rows[0]["检查结果"] = checkform.ischeckOk==true?"合格":"不合格" ;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            if (checkform.ischeckOk)
            {
                ckb合格.Checked = true;
                ckb不合格.Checked = false;
            }
            else
            {
                ckb合格.Checked = false;
                ckb不合格.Checked = true;
            }
            setControlFalse();


            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            base.CheckResult();
        }

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "不合格")
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清场记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "清场记录";
                dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows[0]["审核人"] = "__待审核";
            //dt_prodinstr.Rows[0]["审批时间"] = DateTime.Now;
            dt_prodinstr.Rows[0]["检查人"] = "__待审核";

            save();

            //空间都不能点
            setControlFalse();
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_prodinstr.Rows[0]["日志"].ToString()).Show();
        }

        private void bt插入查询_Click(object sender, EventArgs e)
        {
            if (cb产品代码.Text == "")
            {
                MessageBox.Show("选择一条产品代码");
                return;
            }
            if (_userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员才能新建记录");
                return;
            }
            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;

            instrid = mySystem.Parameter.cleancutInstruID;
            prodcode = cb产品代码.Text;
            readOuterData(instrid,prodcode);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid,prodcode);
                removeOuterBinding();
                outerBind();
            }

            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["生产班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            if (dt_prodinstr.Rows[0]["审核人"].ToString() != "" && dt_prodinstr.Rows[0]["审核人"].ToString() != "__待审核")
            {
                ckb合格.Checked = (bool)dt_prodinstr.Rows[0]["审核是否通过"];
                ckb不合格.Checked = !ckb合格.Checked;
            }
            else
            {
                ckb合格.Checked = false;
                ckb不合格.Checked = false;
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            addOtherEvnetHandler();
            setFormState();
            setEnableReadOnly();

            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;
            
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

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {

            int ind = 0;
            if (dataGridView1.Rows.Count > 14)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 14; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[6, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView1.Rows.Count - 14;
            }

            my.Cells[3, 1].Value = "产品代码/规格：" + cb产品代码.Text+"   "+tb产品规格.Text;
            my.Cells[3, 5].Value = "产品批号：" + tb产品批号.Text;
            if (ckb白班.Checked)
                my.Cells[3, 7].Value = String.Format("生产日期：{0}\n生产班次： 白班☑   夜班□", dtp生产日期.Value.ToString("yyyy年MM月dd日"));
            else
                my.Cells[3, 7].Value = String.Format("生产日期：{0}\n生产班次： 白班□   夜班☑", dtp生产日期.Value.ToString("yyyy年MM月dd日"));
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[5 + i, 1].Value = i + 1;
                my.Cells[5 + i, 2].Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 3].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[5 + i, 6].Value = dataGridView1.Rows[i].Cells[5].Value.ToString();
            }
            my.Cells[5, 7].Value = tb清场人.Text;
            if(ckb合格.Checked && !ckb不合格.Checked)
                my.Cells[5, 8].Value = "合格☑\n不合格□";
            else if(ckb不合格.Checked && !ckb合格.Checked)
                my.Cells[5, 8].Value = "合格□\n不合格☑";
            else
                my.Cells[5, 8].Value = "合格□\n不合格□";
            my.Cells[5, 9].Value = tb检查人.Text;
            my.Cells[19 + ind, 1].Value = "备注："+tb备注.Text;

        }
        public void print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/cleancut/SOP-MFG-110-R01A 清场记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = "&P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

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


        private void setDataGridViewBackColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["清洁操作"].Value.ToString() == "合格")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                if (dataGridView1.Rows[i].Cells["清洁操作"].Value.ToString() == "不合格")
                {
                    //dataGridView1.Rows[i].Cells["确认结果"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

    }
}
