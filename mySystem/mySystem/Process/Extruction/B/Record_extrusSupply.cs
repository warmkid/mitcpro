using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using mySystem;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusSupply : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        mySystem.CheckForm checkform;

        private int label_prodcode;//0代表往里填列表项
        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        private Dictionary<string, string> dict_procode_batch;
        private Dictionary<string, string> dict_inoutmatcode_batch;
        private Dictionary<string, string> dict_midmatcode_batch;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        int instrid;
        string prodcode;
        string instr;
        DateTime time;
        bool flight;

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDataEventHandler()
        {
            //this.cb产品代码.SelectedIndexChanged += new System.EventHandler(this.cb产品代码_SelectedIndexChanged);
            this.tb用料ab1c.TextChanged += new EventHandler(tb用料ab1c_TextChanged);
            this.tb用料b2.TextChanged += new EventHandler(tb用料b2_TextChanged);
        }

        void tb用料b2_TextChanged(object sender, EventArgs e)
        {
            if (label_prodcode == 1)
            {
                if (tb用料b2.Text == "")
                    return;
                float a;
                if (!float.TryParse(tb用料b2.Text, out a))
                {
                    MessageBox.Show("用料量必须为数字");
                    return;
                }
                if (dt_prodinstr.Rows.Count > 0)
                {
                    label_prodcode = 0;
                    //dt_prodinstr.Rows[0]["中层原料用量"] = a;
                    //dt_prodinstr.Rows[0]["中层原料余量"] = float.Parse(dt_prodinstr.Rows[0]["中内层供料量合计b"].ToString()) - a;
                    dt_prodinstr.Rows[0]["中层原料余量"] = a;
                    dt_prodinstr.Rows[0]["中层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["中内层供料量合计b"].ToString()) - a;
                    bs_prodinstr.EndEdit();
                    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                    label_prodcode = 1;
                }

            }
        }

        void tb用料ab1c_TextChanged(object sender, EventArgs e)
        {
            if (label_prodcode == 1)
            {
                if (tb用料ab1c.Text == "")
                    return;
                float a;
                if (!float.TryParse(tb用料ab1c.Text, out a))
                {
                    MessageBox.Show("用料量必须为数字");
                    return;
                }
                if (dt_prodinstr.Rows.Count > 0)
                {
                    label_prodcode = 0;
                    //dt_prodinstr.Rows[0]["外中内层原料用量"] = a;
                    //dt_prodinstr.Rows[0]["外中内层原料余量"] = float.Parse(dt_prodinstr.Rows[0]["外层供料量合计a"].ToString()) - a;
                    dt_prodinstr.Rows[0]["外中内层原料余量"] = a;
                    dt_prodinstr.Rows[0]["外中内层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["外层供料量合计a"].ToString()) - a;
                    bs_prodinstr.EndEdit();
                    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                    label_prodcode = 1;
                }

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
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜供料记录'", mySystem.Parameter.connOle);
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

        //获取设置中产品代码
        private void getOtherData()
        {
            label_prodcode = 0;
            addprodcode(instrid);
            addmatcode(instrid);
            label_prodcode = 1;
        }

        private void init()
        {
            dict_inoutmatcode_batch = new Dictionary<string, string>();
            dict_midmatcode_batch = new Dictionary<string, string>();
            dict_procode_batch = new Dictionary<string, string>();

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

        }
        //表格错误处理
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        public Record_extrusSupply(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            instrid = mySystem.Parameter.proInstruID;
            instr = mySystem.Parameter.proInstruction;
            fill_printer();
            getPeople();
            setUserState();
            getOtherData();
            readOuterData(instrid);
            removeOuterBinding();
            outerBind();
            
            if (dt_prodinstr.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                return;
            }
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid);
                removeOuterBinding();
                outerBind();
            }
            //ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            //ckb夜班.Checked = !ckb白班.Checked;

            
            time = DateTime.Parse(dtp供料日期.Value.ToShortDateString());
            flight = mySystem.Parameter.userflight == "白班";

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();


            setFormState();
            setEnableReadOnly();
            addDataEventHandler();

            dtp供料日期.Enabled = false;
            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;
            
            //setControlFalse();
            //bt打印.Enabled = false;
            //bt日志.Enabled = false;
            //cb打印机.Enabled = false;

            //cb产品代码.Enabled = true;
            //dtp供料日期.Enabled = true;
            //bt插入查询.Enabled = true;
        }

        public Record_extrusSupply(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            fill_printer();
            getPeople();
            setUserState();
            
            setControlFalse();

            string asql = "select * from 吹膜供料记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            
            instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());
            instr = tempdt.Rows[0]["生产指令编号"].ToString();
            prodcode = tempdt.Rows[0]["产品代码"].ToString();
            time = (DateTime)tempdt.Rows[0]["供料日期"];
            flight = (bool)tempdt.Rows[0]["班次"];

            addmatcode(instrid);
            label_prodcode = 1;

            readOuterData(instrid);
            removeOuterBinding();
            outerBind();
            cb产品代码.Text = prodcode;
            cb原料代码ab1c.Text = dt_prodinstr.Rows[0]["外中内层原料代码"].ToString();
            cb原料代码b2.Text = dt_prodinstr.Rows[0]["中层原料代码"].ToString();
            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();
            addDataEventHandler();

            cb产品代码.Enabled = false;
            dtp供料日期.Enabled = false;
            bt插入查询.Enabled = false;
        }

        //添加供料
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            dr = writeInnerDefault(dr);
            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
            this.dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        //删除一条供料记录
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            //刷新合计
            float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
                    sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
                    sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
                    sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;

            //刷新用料和余料
            float a1, a2;
            if (float.TryParse(tb用料ab1c.Text, out a1))
            {
                dt_prodinstr.Rows[0]["外中内层原料余量"] = a1;
                dt_prodinstr.Rows[0]["外中内层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["外层供料量合计a"].ToString()) - a1;
            }
            if (float.TryParse(tb用料b2.Text, out a2))
            {
                dt_prodinstr.Rows[0]["中层原料余量"] = a2;
                dt_prodinstr.Rows[0]["中层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["中内层供料量合计b"].ToString()) - a2;
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        //审核信息
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
                try
                {
                    (this.Owner as ExtructionMainForm).InitBtn();
                }
                catch (NullReferenceException) { }
            }
            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜供料记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
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

        //审核按钮
        private void button4_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        //读取该生产指令下所有的产品代码，加入 生产代码的 items
        private void addprodcode(int instrid)
        {
            dict_procode_batch = new Dictionary<string, string>();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select 产品编码,产品批号 from 生产指令产品列表 where 生产指令ID=" + instrid ;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                if (tempdt.Rows[i]["产品编码"] != null && tempdt.Rows[i]["产品批号"] != null)
                {
                    cb产品代码.Items.Add((string)tempdt.Rows[i]["产品编码"]);
                    dict_procode_batch.Add((string)tempdt.Rows[i]["产品编码"], (string)tempdt.Rows[i]["产品批号"]);
                }
            }
            da.Dispose();
            tempdt.Dispose();
            comm.Dispose();
        }

        //读取生产指令下物料代码，并保存
        private void addmatcode(int instrid)
        {
            dict_midmatcode_batch = new Dictionary<string, string>();
            dict_inoutmatcode_batch = new Dictionary<string, string>();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select 内外层物料代码,内外层物料批号,中层物料代码,中层物料批号 from 生产指令信息表 where ID=" + instrid;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                if (tempdt.Rows[i]["内外层物料代码"] != null && tempdt.Rows[i]["内外层物料批号"] != null)
                {
                    cb原料代码ab1c.Items.Add(Convert.ToString(tempdt.Rows[i]["内外层物料代码"]));        
                    dict_inoutmatcode_batch.Add(Convert.ToString(tempdt.Rows[i]["内外层物料代码"]), Convert.ToString(tempdt.Rows[i]["内外层物料批号"]));
                    label9.Text = tempdt.Rows[i]["内外层物料代码"].ToString();
                    tb原料批号ab1c.Text = tempdt.Rows[i]["内外层物料批号"].ToString();
                }
                if (tempdt.Rows[i]["中层物料代码"] != null && tempdt.Rows[i]["中层物料批号"] != null)
                {
                    cb原料代码b2.Items.Add(Convert.ToString(tempdt.Rows[i]["中层物料代码"]));
                    dict_midmatcode_batch.Add(Convert.ToString(tempdt.Rows[i]["中层物料代码"]), Convert.ToString(tempdt.Rows[i]["中层物料批号"]));
                    label11.Text = Convert.ToString(tempdt.Rows[i]["中层物料代码"]);
                    tb原料批号b2.Text = tempdt.Rows[i]["中层物料批号"].ToString();
                }                     
            }
            if(tempdt.Rows.Count>0)
            {
                dt_prodinstr = new DataTable();
                cb原料代码ab1c.Text = Convert.ToString(tempdt.Rows[0]["内外层物料代码"]);
                cb原料代码b2.Text=Convert.ToString(tempdt.Rows[0]["中层物料代码"]);
            }
                
            da.Dispose();
            tempdt.Dispose();
            comm.Dispose();
        }


        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            //dr["产品代码"] = cb产品代码.Text;
            //dr["产品批号"] = dict_procode_batch[cb产品代码.Text];
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;
            dr["外中内层原料代码"] = cb原料代码ab1c.Text;
            dr["中层原料代码"] = cb原料代码b2.Text;
            dr["外中内层原料批号"] = tb原料批号ab1c.Text;
            dr["中层原料批号"] = tb原料批号b2.Text;
            dr["外中内层原料用量"] = 0;
            dr["外中内层原料余量"] = 0;
            dr["中层原料用量"] = 0;
            dr["中层原料余量"] = 0;
            dr["外层供料量合计a"] = 0;
            dr["中内层供料量合计b"] = 0;
            dr["中层供料量合计c"] = 0;
            dr["供料日期"] = DateTime.Parse(dtp供料日期.Value.ToShortDateString());
            dr["班次"] = mySystem.Parameter.userflight == "白班";
            dr["审核日期"] = DateTime.Now;
            ckb白班.Checked = (bool)dr["班次"] ;
            ckb夜班.Checked = !ckb白班.Checked;

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
            dr["T吹膜供料记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["供料时间"] = DateTime.Now;
            dr["外层供料量"] = 0;
            dr["中内层供料量"] = 0;
            dr["中层供料量"] = 0;
            dr["原料抽查结果"] = "合格";
            dr["班次"] = mySystem.Parameter.userflight;
            dr["操作员备注"] = "无";
            dr["供料人"] =mySystem.Parameter.userName;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid, string prodcode,DateTime time,bool flight)
        {
            dt_prodinstr = new DataTable("吹膜供料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜供料记录 where 生产指令ID=" + instrid + " and 产品代码='" + prodcode + "' and 供料日期=#"+time+"# and 班次="+flight, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("吹膜供料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜供料记录 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜供料记录详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            dtp供料日期.DataBindings.Clear();
            //ckb白班.DataBindings.Clear();
            cb产品代码.DataBindings.Clear();
            tb产品批号.DataBindings.Clear();
            tb生产指令.DataBindings.Clear();
            cb原料代码ab1c.DataBindings.Clear();
            cb原料代码b2.DataBindings.Clear();
            tb原料批号ab1c.DataBindings.Clear();
            tb原料批号b2.DataBindings.Clear();
            tb外层合计.DataBindings.Clear();
            //tb中内层合计.DataBindings.Clear();
            tb中内层合计.DataBindings.Clear();
            tb用料ab1c.DataBindings.Clear();
            tb余料ab1c.DataBindings.Clear();
            tb用料b2.DataBindings.Clear();
            tb余料b2.DataBindings.Clear();
            tb复核人.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            dtp供料日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "供料日期");
            //ckb白班.DataBindings.Add("Checked", bs_prodinstr.DataSource, "班次");
            cb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品代码");
            tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品批号");
            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            cb原料代码ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料代码");
            cb原料代码b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料代码");
            tb原料批号ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料批号");
            tb原料批号b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料批号");
            tb用料ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料余量");
            tb余料ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料用量");
            tb用料b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料余量");
            tb余料b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料用量");
            tb外层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "外层供料量合计a");
            tb中内层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "中内层供料量合计b");
            //tb内层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层供料量合计c");
            tb复核人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审核人");

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
        }

        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "外层供料量":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = cb原料代码ab1c.Text;
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;

                    case "中内层供料量":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = cb原料代码b2.Text;
                        c4.Name = dc.ColumnName;
                        c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c4.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c4);
                        break;
                    case "原料抽查结果":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;

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
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜供料记录ID
            dataGridView1.Columns[6].Visible = false;//中内层供料量
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}
        }
        //中文逗号转英文逗号
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性
            //ab1c原料批号和b2原料批号
            string s = tb原料批号ab1c.Text;
            s = ToDBC(s);
            string[] s1 = s.Split(',');
            string[] str_inout = dict_inoutmatcode_batch.Values.ToArray();
            string[] strlist_inout = str_inout[0].Split(',');
            for (int i = 0; i < s1.Length; i++)
            {
                if (Array.IndexOf(strlist_inout,s1[i]) < 0)
                {
                    MessageBox.Show(cb原料代码ab1c.Text+"中没有对应的批号:"+s1[i]);
                    return false;
                }
            }
            s = tb原料批号b2.Text;
            s = ToDBC(s);
            s1 = s.Split(',');         
            string[] strlist_mid = dict_midmatcode_batch.Values.ToArray()[0].Split(',');
            for (int i = 0; i < s1.Length; i++)
            {
                if (Array.IndexOf(strlist_mid, s1[i]) < 0)
                {
                    MessageBox.Show(cb原料代码b2.Text+"中没有对应的批号:"+s1[i]);
                    return false;
                }
            }
            return true;
        }

        private void cb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            //label_prodcode = 0;
            //setControlTrue();

            //readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text,DateTime.Parse(dtp供料日期.Value.ToShortDateString()),mySystem.Parameter.userflight=="白班");
            //removeOuterBinding();
            //outerBind();

            //if (dt_prodinstr.Rows.Count <= 0)
            //{
            //    DataRow dr = dt_prodinstr.NewRow();
            //    dr = writeOuterDefault(dr);
            //    dt_prodinstr.Rows.Add(dr);
            //    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            //    readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, DateTime.Parse(dtp供料日期.Value.ToShortDateString()), mySystem.Parameter.userflight == "白班");
            //    removeOuterBinding();
            //    outerBind();
            //}
            //ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            //ckb夜班.Checked = !ckb白班.Checked;

            //instrid = mySystem.Parameter.proInstruID;
            //prodcode = cb产品代码.Text;
            //time = DateTime.Parse(dtp供料日期.Value.ToShortDateString());
            //flight = mySystem.Parameter.userflight == "白班";

            //readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            //innerBind();
            //if (dataGridView1.Rows.Count == 0)
            //{
            //    dt_prodinstr.Rows[0]["外层供料量合计a"] = 0;
            //    dt_prodinstr.Rows[0]["中内层供料量合计b"] = 0;
            //    dt_prodinstr.Rows[0]["外中内层原料用量"] = 0;
            //    dt_prodinstr.Rows[0]["外中内层原料余量"] = 0;
            //    dt_prodinstr.Rows[0]["中层原料用量"] = 0;
            //    dt_prodinstr.Rows[0]["中层原料余量"] = 0;

            //    tb外层合计.Text = "0";
            //    tb中内层合计.Text = "0";
            //    tb用料ab1c.Text = "0";
            //    tb余料ab1c.Text = "0";
            //    tb用料b2.Text = "0";
            //    tb余料b2.Text = "0";                
            //}

            //setFormState();
            //setEnableReadOnly();

            
            //label_prodcode = 1;

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
            //if (stat_user == 2)//管理员
            //{
            //    //控件都能点
            //    setControlTrue();
            //}
            //else if (stat_user == 1)//审核人
            //{
            //    if (stat_form == 0 || stat_form == 3 || stat_form == 2)//草稿,审核不通过，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();
            //    }
            //    else//待审核
            //    {
            //        //发送审核不可点，其他都可点
            //        setControlTrue();
            //        bt审核.Enabled = true;
                    
            //    }

            //}
            //else//操作员
            //{
            //    if (stat_form == 1 || stat_form == 2)//待审核，审核通过
            //    {
            //        //空间都不能点
            //        setControlFalse();

            //        cb产品代码.Enabled = true;
            //        dtp供料日期.Enabled = true;

            //    }
            //    else//未审核与审核不通过
            //    {
            //        //发送审核，审核不能点
            //        setControlTrue();
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
                    bt审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else
                {
                    setControlFalse();
                    cb产品代码.Enabled = true;
                    dtp供料日期.Enabled = true;
                } 
            }
        }

        //保存内外表信息
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

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

            setUndoColor();
            return true;
        }
        private void bt保存_Click(object sender, EventArgs e)
        {
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt提交审核.Enabled = true;
            try { (this.Owner as ExtructionMainForm).InitBtn(); }
            catch (NullReferenceException exp) { }
            
        }

        private void cb原料代码ab1c_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt_prodinstr.Rows.Count > 0)
            {
                dt_prodinstr.Rows[0]["外中内层原料代码"] = cb原料代码ab1c.Text;
                //dt_prodinstr.Rows[0]["外中内层原料批号"] = dict_inoutmatcode_batch[cb原料代码ab1c.Text];
                if (cb原料代码ab1c.Text != "")
                {
                    cb原料代码ab1c.Enabled = false;
                    label9.Text = cb原料代码ab1c.Text;
                    label4.Text = cb原料代码ab1c.Text;
                }
            }

        }

        private void cb原料代码b2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt_prodinstr.Rows.Count > 0)
            {
                dt_prodinstr.Rows[0]["中层原料代码"] = cb原料代码b2.Text;
                //dt_prodinstr.Rows[0]["中层原料批号"] = dict_midmatcode_batch[cb原料代码b2.Text];
                if (cb原料代码b2.Text != "")
                {
                    cb原料代码b2.Enabled = false;
                    label11.Text = cb原料代码b2.Text;
                    label5.Text = cb原料代码b2.Text;
                }
            }

        }

        //datagridview单元格编辑结束
        private void dataGridView1_Endedit(object sender, DataGridViewCellEventArgs e)
        {
            //计算合计
            float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//外层
                    sum_out += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//中内层
                    sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() != "")//内层
                    sum_mid += float.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
            }

            dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;

            //刷新用料和余料
            float a1, a2;
            if (float.TryParse(tb用料ab1c.Text, out a1))
            {
                dt_prodinstr.Rows[0]["外中内层原料余量"] = a1;
                dt_prodinstr.Rows[0]["外中内层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["外层供料量合计a"].ToString()) - a1;
            }
            if (float.TryParse(tb用料b2.Text, out a2))
            {
                dt_prodinstr.Rows[0]["中层原料余量"] =a2;
                dt_prodinstr.Rows[0]["中层原料用量"] = float.Parse(dt_prodinstr.Rows[0]["中内层供料量合计b"].ToString()) - a2;
            }



            //供料人是否合法
            if (e.ColumnIndex == 7)
            {
                if(queryid(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString())==-1)
                {
                    MessageBox.Show("供料人id不存在");
                    dataGridView1.Rows[e.RowIndex].Cells[7].Value = "";
                }
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            if (e.ColumnIndex == 6)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "不合格")
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count <= 0)
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

            ////计算合计
            //float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
            //        sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
            //        sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
            //        sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            //}
            //dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            //dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            //dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;


        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count <= 0)
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

            ////计算合计
            //float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
            //        sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
            //        sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
            //        sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            //}
            //dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            //dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            //dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Record_extrusSupply s=new Record_extrusSupply(mainform, 5);           
            s.Show();
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
            dir += "./../../xls/Extrusion/B/SOP-MFG-301-R06 吹膜供料记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = instr + "-06-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

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
            if (dataGridView1.Rows.Count > 4)
            {
                //在第10行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 4; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[10, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView1.Rows.Count - 4;
            }

            my.Cells[3, 1].Value = "产品代码: "+cb产品代码.Text;
            my.Cells[3, 5].Value = "产品批号: " + tb产品批号.Text ;
            my.Cells[3, 7].Value = "生产指令编号: " + tb生产指令.Text ;
            my.Cells[5, 6].Value = cb原料代码ab1c.Text;
            my.Cells[5, 8].Value = tb原料批号ab1c.Text;
            my.Cells[6, 6].Value = cb原料代码b2.Text;
            my.Cells[6, 8].Value = tb原料批号b2.Text;

            my.Cells[7, 1].Value = "供料日期时间: "+dtp供料日期.Value.ToString("yyyy年MM月dd日");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime tempdt=DateTime.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                string time = string.Format("{0}:{1}:{2}", tempdt.Hour.ToString(), tempdt.Minute.ToString(), tempdt.Second.ToString());
                my.Cells[9 + i, 1] = time;
                my.Cells[9 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[9 + i, 4] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[9 + i, 5] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[9 + i, 6] = dataGridView1.Rows[i].Cells[7].Value.ToString();
            }

            my.Cells[9, 8].Value = tb余料ab1c.Text;
            my.Cells[9, 9].Value = tb用料ab1c.Text;

            my.Cells[10+ind, 8].Value = tb余料b2.Text;
            my.Cells[10+ind, 9].Value = tb用料b2.Text;
            my.Cells[9, 10].Value = tb复核人.Text;
            my.Cells[13+ind, 2].Value = tb外层合计.Text ;
            my.Cells[13+ind, 4].Value = tb中内层合计.Text;
        }

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from 吹膜供料记录 where 生产指令ID=" + instrid;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]) + 1;

        }

        private void setUndoColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "不合格")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        private void bt提交审核_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("确认本表已经填完吗？提交审核之后不可修改", "提示", MessageBoxButtons.YesNo))
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "不合格")
                    {
                        MessageBox.Show("有条目待确认");
                        bt提交审核.Enabled = false;
                        return;
                    }
                }

                //写待审核表
                DataTable dt_temp = new DataTable("待审核");
                BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='吹膜供料记录' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);

                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "吹膜供料记录";
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

            //班次不能编辑
            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;

            cb原料代码ab1c.Enabled = false;
            cb原料代码b2.Enabled = false;
            tb原料批号ab1c.ReadOnly = true;
            tb原料批号b2.ReadOnly = true;
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
            readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, DateTime.Parse(dtp供料日期.Value.ToShortDateString()), mySystem.Parameter.userflight == "白班");
            removeOuterBinding();
            outerBind();

            if (dt_prodinstr.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                return;
            }
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, DateTime.Parse(dtp供料日期.Value.ToShortDateString()), mySystem.Parameter.userflight == "白班");
                removeOuterBinding();
                outerBind();
            }
            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            instrid = mySystem.Parameter.proInstruID;
            prodcode = cb产品代码.Text;
            time = DateTime.Parse(dtp供料日期.Value.ToShortDateString());
            flight = mySystem.Parameter.userflight == "白班";

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();


            setFormState();
            setEnableReadOnly();
            addDataEventHandler();

            dtp供料日期.Enabled = false;
            cb产品代码.Enabled = false;
            bt插入查询.Enabled = false;
        }

    }
}
