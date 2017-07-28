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
using WindowsFormsApplication1;
using mySystem;


namespace BatchProductRecord
{
    public partial class ProcessProductInstru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;
        float leng;
   
        int label = 0;
        OleDbConnection connOle;
        private DataTable dt_prodinstr,dt_prodlist;
        private OleDbDataAdapter da_prodinstr,da_prodlist;
        private BindingSource bs_prodinstr,bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr,cb_prodlist;

        private Dictionary<string, string> dict_白班;
        private Dictionary<string, string> dict_夜班;

        private float 面;
        private float 密度;
        private float 系数1;
        private float 系数2;

        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;

        //用于带id参数构造函数，存储已存在记录的相关信息
        string instrcode;

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
            //if (dt_prodinstr.Rows[0]["审批人"].ToString() == "")
            //    stat_form = 0;
            //else if (dt_prodinstr.Rows[0]["审批人"].ToString() == "__待审核")
            //    stat_form = 1;
            //else if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            //    stat_form = 2;
            //else
            //    stat_form = 3;

            string s = dt_prodinstr.Rows[0]["审批人"].ToString();
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
                else if (Parameter.FormState.审核通过 == _formState)
                {
                    setControlFalse();
                    bt更改.Enabled = true;
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
            bt更改.Enabled = false;
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

        //// 获取操作员和审核员
        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='吹膜工序生产指令'", mySystem.Parameter.connOle);
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
       
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument() ;
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            } 
        }

        private void getOtherData()
        {
            if (label == 0)
            {
                readparam();
                fill_prodart();
                fill_prodname();
                fill_matcode();
                fill_respons_user();
                fill_printer();
            }
            label = 1;

        }

        //读取产品列表填入产品名称下拉列表
        private void fill_prodname()
        {
            DataTable dt = new System.Data.DataTable();
            OleDbDataAdapter da= new OleDbDataAdapter("select 产品名称 from 设置吹膜产品", mySystem.Parameter.connOle);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(dt);
            da.Dispose();
            cb.Dispose();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i][0].ToString());
            }
        }
        //读取设置中生产工艺到下拉列表中
        private void fill_prodart()
        {
            DataTable dt = new System.Data.DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("select 工艺名称 from 设置吹膜工艺", mySystem.Parameter.connOle);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(dt);
            da.Dispose();
            cb.Dispose();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb工艺.Items.Add(dt.Rows[i][0].ToString());
            }
        }

        //读取设置中物料代码到下拉列表中
        private void fill_matcode()
        {
            DataTable dt = new System.Data.DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("select 物料代码 from 设置物料代码", mySystem.Parameter.connOle);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(dt);
            da.Dispose();
            cb.Dispose();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb内外层物料代码.Items.Add(dt.Rows[i][0].ToString());
                cb中层物料代码.Items.Add(dt.Rows[i][0].ToString());
            }
        }

        //读取负责人人员到下拉列表中
        private void fill_respons_user()
        {
            DataTable tempdt = new DataTable("users");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from users where 岗位 like '%吹膜%'", mySystem.Parameter.connOleUser);
            da.Fill(tempdt);
            da.Dispose();
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                cb负责人.Items.Add(tempdt.Rows[i][2].ToString());//姓名
            }
        }

        //读取生产指令参数
        private void readparam()
        {
            DataTable tempdt = new System.Data.DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置生产指令参数", mySystem.Parameter.connOle);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(tempdt);
            da.Dispose();
            cb.Dispose();
            面=float.Parse(tempdt.Rows[0][0].ToString());
            密度=float.Parse(tempdt.Rows[0][1].ToString());
            系数1=float.Parse(tempdt.Rows[0][2].ToString());
            系数2 = float.Parse(tempdt.Rows[0][3].ToString());
            tempdt.Dispose();
        }

        public ProcessProductInstru(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            init();
            getPeople();
            setUserState();
            getOtherData();

            setControlFalse();
            tb指令编号.ReadOnly = false;
            bt打印.Enabled = false;
            bt日志.Enabled = false;

            tb指令编号.Enabled = true;
            bt查询插入.Enabled = true;           
          
        }

        public ProcessProductInstru(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            init();

            getPeople();
            setUserState();
            getOtherData();

            string asql = "select * from 生产指令信息表 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            instrcode = tempdt.Rows[0]["生产指令编号"].ToString();
            

            readOuterData(instrcode,id);
            removeOuterBinding();
            outerBind();
            
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            string s = tb白班.Text;
            string[] s1 = s.Split(',');
            dict_白班.Clear();
            dict_夜班.Clear();
            for (int i = 0; i < s1.Length - 1; i++)
            {
                dict_白班.Add(s1[i], s1[i]);
            }
            s = tb夜班.Text;
            s1 = s.Split(',');
            for (int i = 0; i < s1.Length - 1; i++)
            {
                dict_夜班.Add(s1[i], s1[i]);
            }
            setFormState();
            setEnableReadOnly();

            tb指令编号.Enabled = false;
            bt查询插入.Enabled = false;
        }

        private void init()
        {
            connOle = mySystem.Parameter.connOle;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr=new System.Data.DataTable();
            bs_prodinstr=new System.Windows.Forms.BindingSource();
            da_prodinstr=new OleDbDataAdapter();
            cb_prodinstr=new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist=new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            dict_白班 = new Dictionary<string, string>();
            dict_夜班 = new Dictionary<string, string>();
        }

        private void bind_bs_contr()    
        {
            //textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品名称");
            comboBox1.DataBindings.Add("Text",bs_prodinstr.DataSource,"产品名称");
            tb指令编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            cb工艺.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产工艺");
            tb设备编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产设备编号");
            dtp开始生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "开始生产日期");

            //tb内外层物料代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料代码");
            cb内外层物料代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料代码");
            tb内外层物料批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料批号");
            tb内外层包装规格.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层包装规格");
            tb内外领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层领料量");
            //textBox16.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料代码");
            cb中层物料代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料代码");
            tb中层物料批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料批号");
            tb中层包装规格.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层包装规格");
            tb中层领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层领料量");
            textBox12.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管");
            textBox13.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管规格");
            tb卷心管领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管领料量");
            textBox9.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装包装规格");
            tb双层包装领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装领料量");
            //tb负责人.DataBindings.Add("Text", bs_prodinstr.DataSource, "负责人");
            tb白班.DataBindings.Add("Text", bs_prodinstr.DataSource, "白班负责人");
            tb夜班.DataBindings.Add("Text", bs_prodinstr.DataSource, "夜班负责人");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");

            tb编制人.DataBindings.Add("Text", bs_prodinstr.DataSource, "编制人");
            tb审批人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批人");
            tb接收人.DataBindings.Add("Text", bs_prodinstr.DataSource, "接收人");
            dateTimePicker2.DataBindings.Add("Value", bs_prodinstr.DataSource, "编制时间");
            dateTimePicker3.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批时间");
            dateTimePicker4.DataBindings.Add("Value", bs_prodinstr.DataSource, "接收时间");

            textBox6.DataBindings.Add("Text", bs_prodinstr.DataSource, "计划产量合计米");
            tb用料重量合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "用料重量合计");
            textBox10.DataBindings.Add("Text", bs_prodinstr.DataSource, "计划产量合计卷");
            tb操作员备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "操作员备注");
            tb内外层比例.DataBindings.Add("Text", bs_prodinstr.DataSource, "比例");
        }
        
        //根据筛选条件得到指令id,没有返回-1
        private int getid(string instrcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 生产指令信息表 where 生产指令编号='" + instrcode + "'";

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

        //表格错误处理
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        //添加编辑行
        private void addrows()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = dataGridView1.Rows.Count+1;
            dataGridView1.Rows.Add(dr);
            //DataGridViewComboBoxCell combox = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1] as DataGridViewComboBoxCell;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    combox.Items.Add(dt.Rows[i][0]);
            //}
        }

        private void ProcessProductInstru_Load(object sender, EventArgs e)
        {

        }

        //审核按钮点击
        public override void CheckResult()
        {           
            //获得审核信息
            dt_prodinstr.Rows[0]["审批人"] = mySystem.Parameter.userName;
            dt_prodinstr.Rows[0]["审批时间"] = checkform.time;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            if (checkform.ischeckOk)//审核通过
            {
                dt_prodinstr.Rows[0]["状态"] = 1;//待接收
            }
            else
            {
                dt_prodinstr.Rows[0]["状态"] = 0;//未审核，草稿
            }

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='生产指令信息表' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
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

            bs_prodinstr.DataSource = dt_prodinstr;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            base.CheckResult();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            checkform = new mySystem.CheckForm(this);
            checkform.Show();  
            
        }

        //确认按钮
        private void button1_Click_1(object sender, EventArgs e)
        {
            bool rt = save();
            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt提交审核.Enabled = true;
        }

        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            if (tb内外层比例.Text != "")
            {
                dt_prodinstr.Rows[0]["比例"] = float.Parse(tb内外层比例.Text);
            }
            bs_prodinstr.DataSource = dt_prodinstr;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(instrcode);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();
            return true;
        }

        //编辑单元格结束后触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //产品编码
            if (e.ColumnIndex == 3)
            {
                while (true)
                {
                    string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string pattern = @"^[a-zA-Z]+-[a-zA-Z]+-[0-9]+\*[0-9]";//正则表达式
                    if (!Regex.IsMatch(str, pattern))
                    {
                        MessageBox.Show("产品代码格式不符合规定，重新输入，例如 PEQ-QE-500*100");
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = "";
                        leng = 0;
                        break ;
                    }
                    string[] array = str.Split('*');
                    string[] array2 = array[0].Split('-');
                    leng = float.Parse(array2[2]);

                    //产品批号
                    string temp = array[1];
                    if (temp == "100")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "1";
                    }
                    else if (temp == "80")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "2";
                    }
                    else if (temp == "60")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "3";
                    }
                    else if (temp == "120")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "4";
                    }
                    else if (temp == "200")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "5";
                    }
                    else if (temp == "110")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "6";
                    }
                    else if (temp == "70")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dtp开始生产日期.Value.ToString("yyyyMMdd") + "7";
                    }
                    else { };

                    while (true)
                    {
                        string s4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        float a;
                        if (s4 == "" || !float.TryParse(s4, out a))
                        {
                            break;
                        }
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * leng / 1000.0 * 面 * 密度;//用料重量
                        break;
                    }
                    break;
                }
            }
            //计划产量米
            if (e.ColumnIndex == 4)
            {
                while (true)
                {
                    string s4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    float a;
                    if (s4 == "" || !float.TryParse(s4, out a))
                    {
                        break ;
                    }

                    string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string[] array;
                    string[] array2;
                    if (str != "")
                    {
                        array = str.Split('*');
                        array2 = array[0].Split('-');
                        leng = float.Parse(array2[2]);
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * leng / 1000.0 * 面 * 密度;//用料重量
                    }

                    string s7 = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    int b;
                    if (s7 == "" || !int.TryParse(s7, out b))
                    {
                        break;
                    }
                    if (b == 0)
                        break;
                    dataGridView1.Rows[e.RowIndex].Cells[8].Value = Math.Ceiling(Convert.ToDecimal(a / b));//计划产量（卷）
                    dataGridView1.Rows[e.RowIndex].Cells[12].Value = Math.Ceiling(Convert.ToDecimal(a / b));//标签领料量
                    break;
                }

            }
            //每卷长度
            if (e.ColumnIndex == 7)
            {
                while (true)
                {
                    int a;
                    string s7 = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (s7 == "" || !int.TryParse(s7, out a))
                    {
                        break;
                    }
                    if (a == 0)
                        break;
                    string s4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    float b;
                    if (s4 == "" || !float.TryParse(s4, out b))
                    {
                        break;
                    }
                    dataGridView1.Rows[e.RowIndex].Cells[8].Value = Math.Ceiling(Convert.ToDecimal(b / a));//计划产量（卷）
                    dataGridView1.Rows[e.RowIndex].Cells[12].Value = Math.Ceiling(Convert.ToDecimal(b / a));//标签领料量
                    break;
                }
                
            }

            //计算合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;
            //更新领料量
            string bili=tb内外层比例.Text;
            if (bili == "")
                return;
            float fbili = float.Parse(bili);
            if (fbili <= 100 && fbili >= 0)
            {
                dt_prodinstr.Rows[0]["内外层领料量"] = sum_weight / 100 * fbili;
                dt_prodinstr.Rows[0]["中层领料量"] = sum_weight / 100 * (100-fbili);
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        //datagridview 添加行
        private void button4_Click(object sender, System.EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        //根据id填空
        private void fill(int id)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令信息表 where ID=" +id;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                //tb工艺.Text = "";
                cb工艺.Text = "";
                tb设备编号.Text = "";
                dtp开始生产日期.Value = DateTime.Now;

                //tb内外层物料代码.Text = "";
                tb内外层物料批号.Text = "";
                tb内外层包装规格.Text = "";
                tb内外领料量.Text = "";
                //textBox16.Text = "";
                tb中层物料批号.Text = "";
                tb中层包装规格.Text = "";
                tb中层领料量.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                tb卷心管领料量.Text = "";
                textBox9.Text = "";
                tb双层包装领料量.Text = "";
                //tb负责人.Text = "";

                tb编制人.Text = mySystem.Parameter.userName;
                dateTimePicker2.Value = DateTime.Now;
                tb审批人.Text = "";
                dateTimePicker3.Value = DateTime.Now;
                tb接收人.Text = "";
                dateTimePicker4.Value = DateTime.Now;
                tb备注.Text = "";
            }
            if (tempdt.Rows.Count == 1)
            {
                tb指令编号.Text = (string)tempdt.Rows[0][2];
                //tb工艺.Text = (string)tempdt.Rows[0][3]; 
                cb工艺.Text = (string)tempdt.Rows[0][3]; 
                tb设备编号.Text = (string)tempdt.Rows[0][4];
                dtp开始生产日期.Value = (DateTime)tempdt.Rows[0][5];

                //tb内外层物料代码.Text = (string)tempdt.Rows[0][6]; 
                tb内外层物料批号.Text = (string)tempdt.Rows[0][7]; 
                tb内外层包装规格.Text = (string)tempdt.Rows[0][8];
                tb内外领料量.Text = ((double)tempdt.Rows[0][9]).ToString();
                //textBox16.Text = (string)tempdt.Rows[0][10]; 
                tb中层物料批号.Text = (string)tempdt.Rows[0][11];
                tb中层包装规格.Text = (string)tempdt.Rows[0][12];
                tb中层领料量.Text = ((double)tempdt.Rows[0][13]).ToString();
                textBox12.Text = (string)tempdt.Rows[0][14]; 
                textBox13.Text = (string)tempdt.Rows[0][15];
                tb卷心管领料量.Text = ((double)tempdt.Rows[0][16]).ToString();
                textBox9.Text = (string)tempdt.Rows[0][17];
                tb双层包装领料量.Text = ((double)tempdt.Rows[0][18]).ToString();
                //tb负责人.Text = (string)tempdt.Rows[0][19];

                tb编制人.Text = (string)tempdt.Rows[0][20];
                dateTimePicker2.Value = (DateTime)tempdt.Rows[0][21];
                tb审批人.Text = (string)tempdt.Rows[0][22];
                dateTimePicker3.Value = (DateTime)tempdt.Rows[0][23];
                tb接收人.Text = (string)tempdt.Rows[0][24];
                dateTimePicker4.Value = (DateTime)tempdt.Rows[0][25];
                tb备注.Text = (string)tempdt.Rows[0][29];

                da.Dispose();
                tempdt.Dispose();

            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            mySystem.Setting.Setting_CleanSite s = new mySystem.Setting.Setting_CleanSite(mainform);
            s.Show();
        }

        private void button7_Click(object sender, System.EventArgs e)
        {
            Setting_CleanArea s = new Setting_CleanArea(mainform);
            s.Show();
        }

        //通过生产指令查找对应id
        private int id_findby_instrcode(string code)
        {
            string acsql = "select ID from 生产指令信息表 where 生产指令编号='" + code + "'";
            OleDbCommand comm = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            comm.Dispose();
            da.Dispose();
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                return -1;
            }
            else
            {
                int ret = (int)dt1.Rows[0][0];
                dt1.Dispose();
                return ret;
            }
            
        }

        //查询/插入按钮
        private void button5_Click_1(object sender, System.EventArgs e)
        {
            setControlTrue();
            instrcode = tb指令编号.Text;
            readOuterData(tb指令编号.Text);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(tb指令编号.Text);
                removeOuterBinding();
                outerBind();
            }
            
            //string s_out = dt_prodinstr.Rows[0]["内外层领料量"].ToString();
            //string s_mid = dt_prodinstr.Rows[0]["中层领料量"].ToString();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            //默认值处理
            //tb内外层比例.Text = "25";
            //tb中层比例.Text = "75";
            //dt_prodinstr.Rows[0]["内外层领料量"]=s_out;
            //dt_prodinstr.Rows[0]["中层领料量"]=s_mid;

            string s = tb白班.Text;
            string[] s1 = s.Split(',');
            dict_白班.Clear();
            dict_夜班.Clear();
            for (int i = 0; i < s1.Length-1; i++)
            {
                dict_白班.Add(s1[i], s1[i]);
            }
            s = tb夜班.Text;
            s1 = s.Split(',');
            for (int i = 0; i < s1.Length - 1; i++)
            {
                dict_夜班.Add(s1[i], s1[i]);
            }

            setFormState();
            setEnableReadOnly();

            tb指令编号.Enabled = false;
            bt查询插入.Enabled = false;
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = tb指令编号.Text;
            dr["生产设备编号"] = "AA-EQM-032";
            dr["开始生产日期"]=DateTime.Now;
            dr["内外层领料量"]=0;
            dr["中层领料量"]=0;
            dr["卷心管领料量"]=0;
            dr["双层洁净包装领料量"]=0;
            dr["状态"]=0;//草稿
            dr["计划产量合计米"]=0;
            dr["用料重量合计"]=0;
            dr["计划产量合计卷"]=0;
            dr["编制人"]=mySystem.Parameter.userName;
            dr["编制时间"]=DateTime.Now;
            dr["审批时间"]=DateTime.Now;
            dr["接收时间"]=DateTime.Now;
            dr["备注"] = "批号末尾数字代表膜的厚度，分别为：100um-1,80um-2,60um-3,120um-4,200um-5,110um-6,70um-7";
            dr["比例"] = 25;

            dr["审批人"] = "";
            dr["接收人"] = "";

            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            dr["日志"] = log;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["生产指令ID"]=dt_prodinstr.Rows[0]["ID"];
            dr["计划产量米"]=0;
            dr["用料重量"]=0;
            dr["每卷长度"]=0;
            dr["计划产量卷"]=0;
            dr["卷心管规格"]=0;
            dr["标签"]=0;
            dr["标签领料量"]=0;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string code,int id=0)
        {
            //string sql = "select * from 生产指令信息表 where 生产指令编号='{0}' order by id DESC";
            string sql = "";
            if (id == 0)
            {
                sql = "select * from 生产指令信息表 where 生产指令编号='" + code + "'";
            }
            else { sql = "select * from 生产指令信息表 where ID=" + id; }
            dt_prodinstr = new DataTable("生产指令信息表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter(sql, connOle);
            //da_prodinstr = new OleDbDataAdapter(String.Format(sql, code) , connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("生产指令产品列表");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 生产指令产品列表 where 生产指令ID="+id, connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding() 
        {
            //解除之前的绑定
            comboBox1.DataBindings.Clear();
            tb指令编号.DataBindings.Clear();
            cb工艺.DataBindings.Clear();
            tb设备编号.DataBindings.Clear();
            dtp开始生产日期.DataBindings.Clear();

            cb内外层物料代码.DataBindings.Clear();
            tb内外层物料批号.DataBindings.Clear();
            tb内外层包装规格.DataBindings.Clear();
            tb内外领料量.DataBindings.Clear();

            cb中层物料代码.DataBindings.Clear();
            tb中层物料批号.DataBindings.Clear();
            tb中层包装规格.DataBindings.Clear();
            tb中层领料量.DataBindings.Clear();
            textBox12.DataBindings.Clear();
            textBox13.DataBindings.Clear();
            tb卷心管领料量.DataBindings.Clear();
            textBox9.DataBindings.Clear();
            tb双层包装领料量.DataBindings.Clear();
            //tb负责人.DataBindings.Clear();
            tb白班.DataBindings.Clear();
            tb夜班.DataBindings.Clear();
            tb备注.DataBindings.Clear();

            tb编制人.DataBindings.Clear();
            tb审批人.DataBindings.Clear();
            tb接收人.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
            dateTimePicker3.DataBindings.Clear();
            dateTimePicker4.DataBindings.Clear();

            textBox6.DataBindings.Clear();
            tb用料重量合计.DataBindings.Clear();
            textBox10.DataBindings.Clear();
            tb操作员备注.DataBindings.Clear();
            tb内外层比例.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind() 
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            bind_bs_contr();
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
                    case "计划产量米":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = "计划产量(米)";
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;

                    case "用料重量":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = "用料重量(kg)";
                        c4.Name = dc.ColumnName;
                        c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c4.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c4);
                        break;

                    case "每卷长度":
                        DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
                        c5.DataPropertyName = dc.ColumnName;
                        c5.HeaderText = "每卷长度(米/卷)";
                        c5.Name = dc.ColumnName;
                        c5.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c5.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c5);
                        break;

                    case "计划产量卷":
                        DataGridViewTextBoxColumn c6= new DataGridViewTextBoxColumn();
                        c6.DataPropertyName = dc.ColumnName;
                        c6.HeaderText = "计划产量卷(卷)";
                        c6.Name = dc.ColumnName;
                        c6.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c6.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c6);
                        break;

                    case "卷心管规格":
                        DataGridViewTextBoxColumn c7 = new DataGridViewTextBoxColumn();
                        c7.DataPropertyName = dc.ColumnName;
                        c7.HeaderText = "卷心管规格mm";
                        c7.Name = dc.ColumnName;
                        c7.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c7.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c7);
                        break;

                    case "产品编码":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "产品代码（规格型号）";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        OleDbDataAdapter tda = new OleDbDataAdapter("select 产品编码 from 设置吹膜产品编码", mySystem.Parameter.connOle);
                        DataTable tdt = new DataTable("产品编码");
                        tda.Fill(tdt);
                        foreach (DataRow tdr in tdt.Rows)
                        {
                            c1.Items.Add(tdr["产品编码"]);
                        }
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
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[5].ReadOnly = true;//用料重量
            dataGridView1.Columns[6].ReadOnly = true;//产品批号
            dataGridView1.Columns[8].ReadOnly = true;//计划产量卷
            dataGridView1.Columns[12].ReadOnly = true;//标签领料量

        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性

            ////判断领料量是否是合法
            float tempvalue, tempvalue2;
            if (!float.TryParse(tb内外领料量.Text, out tempvalue))
            {
                MessageBox.Show("内外层领料量输入不合法");
                return false;
            }
            if (!float.TryParse(tb中层领料量.Text, out tempvalue2))
            {
                MessageBox.Show("中层领料量输入不合法");
                return false;
            }
            if (tempvalue + tempvalue2 < float.Parse(tb用料重量合计.Text))
            {
                MessageBox.Show("内外、中层领料量之和必须大于等于用料量");
                return false;
            }
            if (!float.TryParse(tb卷心管领料量.Text, out tempvalue))
            {
                MessageBox.Show("卷心管领料量输入不合法");
                return false;
            }

            //编制人
            if (mySystem.Parameter.NametoID(tb编制人.Text)<=0)
            {
                MessageBox.Show("编制人ID不存在");
                return false;
            }
            //接收人
            if (mySystem.Parameter.NametoID(tb接收人.Text) <= 0)
            {
                MessageBox.Show("接受人ID不存在");
                return false;
            }

            //产品代码是否重复
            HashSet<string> hs_temp = new HashSet<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (hs_temp.Contains(dataGridView1.Rows[i].Cells["产品编码"].Value.ToString()))//产品代码
                {
                    MessageBox.Show("产品编码不能重复");
                    return false;
                }
                hs_temp.Add(dataGridView1.Rows[i].Cells["产品编码"].Value.ToString());
            }
            return true;
            
        }

        private void cb内外层物料代码_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //if (label == 1)
            //    dt_prodinstr.Rows[0]["内外层物料代码"] = cb内外层物料代码.Text;
        }

        private void cb中层物料代码_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //if(label==1)
            //    dt_prodinstr.Rows[0]["中层物料代码"] = cb中层物料代码.Text;
        }

        private void tb内外层比例_TextChanged(object sender, System.EventArgs e)
        {
            if (tb内外层比例.Text == "")
            {
                tb中层比例.Text = "";
                return;
            }
            float a;
            if (!float.TryParse(tb内外层比例.Text, out a))
            {
                MessageBox.Show("输入必须为0到100内整数");
                return;
            }
            if(a>100||a<0)
            {
                MessageBox.Show("输入必须为0到100内整数");
                return;
            }
            tb中层比例.Text = (100 - a).ToString() ;
            if (tb用料重量合计.Text != "")
            {
                float sum = float.Parse(tb用料重量合计.Text);
                //tb内外领料量.Text = (sum / 100 * a).ToString();
                //tb中层领料量.Text = (sum / 100 * (100 - a)).ToString();
                dt_prodinstr.Rows[0]["内外层领料量"] = (sum / 100 * a).ToString();
                dt_prodinstr.Rows[0]["中层领料量"] = (sum / 100 * (100 - a)).ToString();
                dt_prodinstr.Rows[0]["比例"] = a;

                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }

        }

        //白班按钮
        private void button2_Click_1(object sender, System.EventArgs e)
        {
            if (cb负责人.Text == "")
                return;
            if(!dict_白班.ContainsKey(cb负责人.Text))  //字典中不存在
            {
                dict_白班.Add(cb负责人.Text, cb负责人.Text);
                tb白班.Text += cb负责人.Text + ",";
                dt_prodinstr.Rows[0]["白班负责人"] = tb白班.Text;
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (cb负责人.Text == "")
                return;
            if (!dict_夜班.ContainsKey(cb负责人.Text))  //字典中不存在
            {
                dict_夜班.Add(cb负责人.Text, cb负责人.Text);
                tb夜班.Text += cb负责人.Text + ",";
                dt_prodinstr.Rows[0]["夜班负责人"] = tb夜班.Text;
            }
        }

        private void bt删除_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex< 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            //刷新序号
            setDataGridViewRowNums();

            //刷新合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;

            //更新领料量
            string bili = tb内外层比例.Text;
            if (bili == "")
                return;
            float fbili = float.Parse(bili);
            if (fbili <= 100 && fbili >= 0)
            {
                dt_prodinstr.Rows[0]["内外层领料量"] = sum_weight / 100 * fbili;
                dt_prodinstr.Rows[0]["中层领料量"] = sum_weight / 100 * (100 - fbili);
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        private void bt上移_Click(object sender, System.EventArgs e)
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

            //刷新序号
            setDataGridViewRowNums();

            //刷新合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;

        }

        private void bt下移_Click(object sender, System.EventArgs e)
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

            //刷新序号
            setDataGridViewRowNums();
            //刷新合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            (new ProcessProductInstru(mainform, 2)).Show();
        }

        private void bt打印_Click(object sender, System.EventArgs e)
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
            dir+="./../../xls/Extrusion/A/SOP-MFG-301-R01 吹膜工序生产指令.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = tb指令编号.Text + "--" + "步骤序号0--" + "表序号" + find_indexofprint() + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

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

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from 生产指令信息表 where 生产指令编号='" + tb指令编号.Text+"'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.Count-list_id.IndexOf((int)dt_prodinstr.Rows[0]["ID"]);

        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            int ind = 0;
            if (dataGridView1.Rows.Count > 7)
            {
                //在第8行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 7; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[8, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView1.Rows.Count - 7;
            }

            my.Cells[2, 1].Value = "PEF 吹膜工序生产指令";
            my.Cells[3, 1].Value = "产品名称："+comboBox1.Text;
            my.Cells[3, 9].Value = "生产指令编号：" + tb指令编号.Text;
            my.Cells[4, 1].Value = "生产工艺：" + cb工艺.Text;          
            my.Cells[4, 6].Value = "生产设备编号：" + tb设备编号.Text;
            my.Cells[4, 9].Value = "开始生产日期：" + dtp开始生产日期.Value.ToLongDateString();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[7 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[7 + i, 6] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[7 + i, 7] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                my.Cells[7 + i, 8] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[7 + i, 9] = dataGridView1.Rows[i].Cells[7].Value.ToString();
                my.Cells[7 + i, 10] = dataGridView1.Rows[i].Cells[8].Value.ToString();
                my.Cells[7 + i, 11] = dataGridView1.Rows[i].Cells[9].Value.ToString();
                my.Cells[7 + i, 12] = dataGridView1.Rows[i].Cells[10].Value.ToString();
            }

            my.Cells[14 + ind, 6].Value = textBox6.Text;//计划产量米
            my.Cells[14 + ind, 7].Value = tb用料重量合计.Text;//用料重量
            my.Cells[14 + ind, 10].Value = textBox10.Text;//计划产量卷
            my.Cells[16 + ind, 6].Value = cb内外层物料代码.Text;
            my.Cells[16 + ind, 8].Value = tb内外层物料批号.Text;
            my.Cells[16 + ind, 9].Value = tb内外层包装规格.Text;
            my.Cells[16 + ind, 10].Value = tb内外领料量.Text;

            my.Cells[17 + ind, 6].Value = cb中层物料代码.Text;
            my.Cells[17 + ind, 8].Value = tb中层物料批号.Text;
            my.Cells[17 + ind, 9].Value = tb中层包装规格.Text;
            my.Cells[17 + ind, 10].Value = tb中层领料量.Text;

            my.Cells[18 + ind, 6].Value = textBox12.Text;
            my.Cells[18 + ind, 9].Value = textBox13.Text;
            my.Cells[18 + ind, 10].Value = tb卷心管领料量.Text;

            my.Cells[20 + ind, 6].Value = textBox7.Text;
            my.Cells[20 + ind, 9].Value = textBox9.Text;
            my.Cells[20 + ind, 10].Value = tb双层包装领料量.Text;

            my.Cells[16 + ind, 12].Value = "白班：" + tb白班.Text + "\n" + "夜班：" + tb夜班.Text;
            my.Cells[21 + ind, 2].Value = tb备注.Text;

            my.Cells[22 + ind, 1].Value = "编制人：" + tb编制人.Text + "\n" + dateTimePicker2.Value.ToLongDateString();
            my.Cells[22 + ind, 6].Value = "审批人：" + tb审批人.Text + "\n" + dateTimePicker3.Value.ToLongDateString();
            my.Cells[22 + ind, 9].Value = "接受人：" + tb接收人.Text + "\n" + dateTimePicker4.Value.ToLongDateString();

        }

        private void bt复制_Click(object sender, System.EventArgs e)
        {

            if (dataGridView1.SelectedCells.Count <= 0)
                return;
            DataRow dr = dt_prodlist.NewRow();
            dr.ItemArray = dt_prodlist.Rows[dataGridView1.SelectedCells[0].RowIndex].ItemArray.Clone() as object[];
            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
           
            //刷新合计
            //计算合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;
            //更新领料量
            string bili = tb内外层比例.Text;
            if (bili == "")
                return;
            float fbili = float.Parse(bili);
            if (fbili <= 100 && fbili >= 0)
            {
                dt_prodinstr.Rows[0]["内外层领料量"] = sum_weight / 100 * fbili;
                dt_prodinstr.Rows[0]["中层领料量"] = sum_weight / 100 * (100 - fbili);
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        private void bt日志_Click(object sender, System.EventArgs e)
        {
            //MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
            (new mySystem.Other.LogForm()).setLog(dt_prodinstr.Rows[0]["日志"].ToString()).Show();
        }

        private void bt提交审核_Click(object sender, System.EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='生产指令信息表' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "生产指令信息表";
                dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bs_prodinstr.DataSource = dt_prodinstr;
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows[0]["审批人"] = "__待审核";
            dt_prodinstr.Rows[0]["审批时间"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
        }

        private void bt更改_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodinstr.NewRow();
            dr.ItemArray = dt_prodinstr.Rows[0].ItemArray.Clone() as object[];
            dt_prodinstr.Rows[0]["审批人"] = "";

            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 更改生产指令计划\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows.Add(dr);
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(instrcode);

            int newid = (int)dt_prodinstr.Rows[dt_prodinstr.Rows.Count-1]["ID"];
            int count = dt_prodlist.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow dr_list = dt_prodlist.NewRow();
                dr_list.ItemArray = dt_prodlist.Rows[i].ItemArray.Clone() as object[];
                dr_list["生产指令ID"] = newid;
                dt_prodlist.Rows.Add(dr_list);
            }
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            MessageBox.Show("更改成功");
        }
    }
}
