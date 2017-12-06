using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace mySystem.Process.Bag.BTV
{
    public partial class 产品热合强度检验记录 : BaseForm
    {

        // TODO ：要加到Mainform中去
        // TODO: 打印  选打印机
        // TODO：构造函数添加参数mainform
        // TODO: 用正则表达式获取操作员和审核员姓名

        // 需要保存的状态
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        int _id,_instrId;
        String _code;

        // 显示界面需要的信息
        List<String> ls操作员, ls审核员;
        Hashtable ht;
        String 产品规格, 产品批号;
        String nowString = DateTime.Now.ToString("yyyy/MM/dd");

        // DataGridView 中用到的一些变量


        // 数据库连接
        
        SqlConnection conn;
        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
        Boolean isFirstBind = true;
        CheckForm ckForm = null;

        public 产品热合强度检验记录(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            fill_printer();
            variableInit();
            getOuterOtherData();
            getPeople();
            setUseState();
            readOuterData();
            outerBind();
            if (0 == dtOuter.Rows.Count)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                ((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = 0;
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData();
                outerBind();
            }
            setKeyInfoFromDataTable();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();

        }


        public 产品热合强度检验记录(MainForm mainform,int id):base(mainform)
        {
            InitializeComponent();
            fill_printer();
            variableInit();
            getOuterOtherData();
            getPeople();
            setUseState();
            readOuterData(id);
            outerBind();
            setKeyInfoFromDataTable(id);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }


        /// <summary>
        /// 所有变量实例化，一些固定的变量赋值
        /// </summary>
        void variableInit()
        {
            conn = mySystem.Parameter.conn;
        }

        /// <summary>
        /// 获取当期显示的数据的关键信息，包括但不限于ID
        /// </summary>
        /// <param name="id"></param>
        void setKeyInfoFromDataTable(int id)
        {
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 产品热合强度检验记录 where ID=" + id + "", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            _instrId = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            _id = id;
            // _code = dtOuter.Rows[0]["生产指令编号"].ToString();
        }

        void setKeyInfoFromDataTable()
        {
            _instrId = mySystem.Parameter.bpvbagInstruID;
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            _code = mySystem.Parameter.bpvbagInstruction;
        }

        /// <summary>
        /// 获取操作员和审核员名单
        /// </summary>
        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='产品热合强度检验记录';", conn);
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

        /// <summary>
        /// 和外表显示相关的数据赋值，主要是下拉框的Items
        /// </summary>
        void getOuterOtherData()
        {
            
        }

        /// <summary>
        /// 和内表显示相关的数据赋值，主要是下拉框的Items
        /// 对可选可输的控件要记得将DataTable中的值也加入Items
        /// </summary>
        void getInnerOtherData()
        {
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + _instrId + "", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            产品规格 = dt.Rows[0]["产品代码"].ToString();
            产品批号 = dt.Rows[0]["产品批号"].ToString();
        }

        /// <summary>
        /// 根据外表ID读取外表数据
        /// </summary>
        /// <param name="id"></param>
        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 产品热合强度检验记录 where ID=" + id + "", conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("产品热合强度检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        /// <summary>
        /// 根据其他条件读取外表数据
        /// </summary>
        /// <param name="code"></param>
        void readOuterData()
        {
            string sql = "select * from 产品热合强度检验记录 where 生产指令ID={0} and 整理时间='{1}'";
            daOuter = new SqlDataAdapter(String.Format(sql, mySystem.Parameter.bpvbagInstruID, nowString), conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("产品热合强度检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }


        /// <summary>
        /// 外表写入默认值
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.bpvbagInstruID;
            dr["标准"] = 15;
            dr["整理人"] = mySystem.Parameter.userName;
            dr["整理时间"] = nowString;
            dr["审核日期"] = DateTime.Now;
            string log = "===================================\n";
            log+="时间： "+DateTime.Now.ToLocalTime()+"\n";
            log+="生产指令： "+mySystem.Parameter.bpvbagInstruction+"\n";
            log += mySystem.Parameter.userName + " 开始填写产品热合检验记录";
            dr["日志"] = log;
            return dr;
        }


        /// <summary>
        /// 外表和控件的绑定
        /// 注意变量名命名规则，区分该绑定的和不该绑定控件
        /// </summary>
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

        /// <summary>
        /// 根据外表ID读取内表信息
        /// </summary>
        /// <param name="outerID"></param>
        void readInnerData(int outerID)
        {
            daInner = new SqlDataAdapter("select * from 产品热合强度检验记录详细记录 where T产品热合强度检验记录ID=" + outerID, conn);
            dtInner = new DataTable("产品热合强度检验记录详细记录");
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }


        /// <summary>
        /// 内表写默认值
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T产品热合强度检验记录ID"] = _id;
            dr["产品规格"] = 产品规格;
            dr["产品批号"] = 产品批号;
            dr["生产日期"] = DateTime.Now.ToString("yyyy年MM月dd日");
            dr["生产时间"] = DateTime.Now.ToString("HH:mm");
            dr["位置1"] = "东";
            dr["位置2"] = "左";
            dr["判定"] = "Y";
            dr["检测人"] = mySystem.Parameter.userName;
            dr["检测值1"] = 0;
            dr["检测值2"] = 0;
            dr["检测值3"] = 0;
            dr["检测值4"] = 0;
            dr["检测值5"] = 0;
            dr["检测值6"] = 0;
            dr["最小"] = 0;
            dr["最大"] = 0;
            dr["平均"] = 0;
            return dr;
        }

        /// <summary>
        /// 内表绑定
        /// </summary>
        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;

        }

        /// <summary>
        /// 设置DataGridView的列
        /// 该函数主要设置列的类型，尤其要注意变成下拉框的列
        /// 列的可见性，只读性，HeadText都不要在这里设置，放在DataBindComplete事件中处理
        /// </summary>
        void setDataGridViewColumn()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "判定")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("Y");
                    cbc.Items.Add("N");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "位置1")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("东");
                    cbc.Items.Add("西");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "位置2")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("左");
                    cbc.Items.Add("右");
                    cbc.Items.Add("底");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }

                // 根据数据类型自动生成列的关键信息
                switch (dc.DataType.ToString())
                {
                    case "System.Int32":
                    case "System.String":
                    case "System.Double":
                    case "System.DateTime":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(ckbc);
                        break;
                }
            }
        }

        /// <summary>
        /// 设置用户状态
        /// </summary>
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

        /// <summary>
        /// 设置窗体状态，传true表示无数据状态，不传参数则根据 审核员 信息自动判断
        /// 如果点开窗体后，需要操作控件才能显示数据的，增加一个 无数据 状态，例如生产指令
        /// 点开窗体就能显示值的，没有这个状态，例如运行记录
        /// </summary>
        /// <param name="newForm"></param>

        void setFormState(bool newForm = false)
        {
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        /// <summary>
        /// 根据用户和窗体状态，设置控件的Enable和ReadOnly
        /// </summary>
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
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核通过 == _formState) setControlTrue();
                else setControlFalse();
            }


        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "btn查询插入") continue;
                if (c.Name == "tb生产指令编码") continue;
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

        /// <summary>
        /// 默认控件不可用状态.
        /// </summary>
        void setControlFalse()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
            foreach (Control c in this.Controls)
            {
                if (c.Name == "btn查询插入") continue;
                if (c.Name == "tb生产指令编码") continue;
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
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cb打印机.Enabled = true;
        }

        /// <summary>
        /// 不好归类的属性设置或者事件注册
        /// </summary>
        void addOtherEvenHandler()
        {
            // TODO 其他无法分类的代码放在这里
            dataGridView1.AllowUserToAddRows = false;
            // 实现下拉框可选可输

            // 设置DataGridVew的可见性和只读属性等都放在绑定结束之后
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;

            dataGridView1.DataError += dataGridView1_DataError;
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewBackColor();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            int[] readonlyIdx = { 2, 3, 4, 14, 15, 16 };
            foreach (int i in readonlyIdx)
            {
                dataGridView1.Columns[i].ReadOnly = true;
            }
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        /// <summary>
        /// 计算类事件的注册
        /// </summary>
        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<int> idxes = new List<int>(new int[] { 8, 9, 10, 11, 12, 13 });
            if (idxes.IndexOf(e.ColumnIndex) >= 0)
            {
                List<double> vals = new List<double>();
                foreach (int i in idxes)
                {
                    vals.Add(Convert.ToDouble(dtInner.Rows[e.RowIndex][i]));
                }
                dtInner.Rows[e.RowIndex][14] = Double.Parse(vals.Max().ToString("0.0"));
                dtInner.Rows[e.RowIndex][15] = Double.Parse(vals.Min().ToString("0.0"));
                dtInner.Rows[e.RowIndex][16] = Double.Parse(vals.Average().ToString("0.0"));
            }
        }
        
        /// <summary>
        /// 确保控件和DataTable中的数据能同步的方法
        /// 凡是需要在程序中通过代码来改变控件值时，请用本方法避免不同步的情况
        /// 第一个参数是控件的变量名，第二个参数是要填入的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        void outerDataSync(String name, String val)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == name)
                {
                    c.Text = val;
                    c.DataBindings[0].WriteValue();
                }
            }
        }

        /// <summary>
        /// 数据有效性验证
        /// </summary>
        /// <returns></returns>
        private bool dataValidate()
        {
            //dataGridView1.DataError += dataGridView1_DataError;

            // TODO 更多条件有待补充
            
            return true;
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            // 一次加一条
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.InsertAt(dr, dtInner.Rows.Count);
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;

            // 一次加六条
            //DataRow[] drs = new DataRow[6];
            //for (int i = 0; i < 6; ++i)
            //{
            //    drs[i] = dtInner.NewRow();
            //    drs[i] = writeInnerDefault(drs[i]);
            //    if (i <= 2) drs[i]["位置1"] = "东";
            //    else drs[i]["位置1"] = "西";
            //}
            //drs[0]["位置2"] = "左";
            //drs[1]["位置2"] = "右";
            //drs[2]["位置2"] = "底";
            //drs[3]["位置2"] = "左";
            //drs[4]["位置2"] = "右";
            //drs[5]["位置2"] = "底";
            //foreach (DataRow dr in drs)
            //{
            //    dtInner.Rows.Add(dr);
            //}

        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            // 一次性删除1行
            int deletenum = dataGridView1.CurrentRow.Index;
            //dt记录详情.Rows.RemoveAt(deletenum);
            dtInner.Rows[deletenum].Delete();

            // 保存
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(_id);
            innerBind();

            setDataGridViewBackColor();

            // 一次性删除6行
            //if (DialogResult.OK == MessageBox.Show("确认要一次性删除六条记录吗？", "提示", MessageBoxButtons.OK))
            //{
            //    int rIdx = dataGridView1.SelectedCells[0].RowIndex;
            //    int startIdx = rIdx - rIdx % 6;
            //    int endIdx = startIdx + 6; // not included
            //    List<DataRow> toDel = new List<DataRow>();
            //    for (int i = startIdx; i < endIdx; ++i)
            //    {
            //        toDel.Add(dtInner.Rows[i]);
            //    }
            //    foreach (DataRow dr in toDel) dr.Delete();
            //    daInner.Update((DataTable)bsInner.DataSource);
            //    readInnerData(_id);
            //    innerBind();
            //}
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            if (setDataGridViewBackColor() == false)
            {
                MessageBox.Show("红色的检测值不符合要求，不可保存！");
                return; 
            }

            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            outerBind();


            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {

            if (!dataValidate())
            {
                MessageBox.Show("数据填写不完整，请仔细检查！");
                return;
            }

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='生产指令' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "产品热合强度检验记录";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();
                logForm.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        public override void CheckResult()
        {


            
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='产品热合强度检验记录' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);


            base.CheckResult();
            

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核是否通过"] = ckForm.ischeckOk;
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            log += "审核结果为：" + (ckForm.ischeckOk ? "通过" : "不通过") + "\n";
            log += "审核意见为："+ckForm.opinion+"\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckForm = new CheckForm(this);
            ckForm.Show();
            
        }

        //设置datagridview背景颜色
        private Boolean setDataGridViewBackColor()
        {
            Boolean ColorReturn = true; //全部为白-True；有红色-False
            Double DoubleStandard = 0;
            if (Double.TryParse(tb标准.Text, out DoubleStandard))
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值1"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值1"].Style.BackColor = Color.Red;
                        //dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值1"].Style.BackColor = Color.White;
                    }
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值2"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值2"].Style.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值2"].Style.BackColor = Color.White;
                    }
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值3"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值3"].Style.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值3"].Style.BackColor = Color.White;
                    }
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值4"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值4"].Style.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值4"].Style.BackColor = Color.White;
                    }
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值5"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值5"].Style.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值5"].Style.BackColor = Color.White;
                    }
                    if (double.Parse(dataGridView1.Rows[i].Cells["检测值6"].Value.ToString()) < DoubleStandard)
                    {
                        dataGridView1.Rows[i].Cells["检测值6"].Style.BackColor = Color.Red;
                        ColorReturn = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["检测值6"].Style.BackColor = Color.White;
                    }
                }
            }
            else
            {
                ColorReturn = false;
                MessageBox.Show("【标准】框内数据不符合要求，请重新输入！");
            }

            return ColorReturn;
        }

        //添加打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        private void fill_printer()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
        }

        //打印功能
        public int print(bool isShow)
        {
            int pageCount = 0;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\PTV\18 QB-PA-PP-03-R02A 产品热合强度检验记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        string log = "=====================================\n";
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒 打印文档\n");
                        dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                        bsOuter.EndEdit();
                        daOuter.Update((DataTable)bsOuter.DataSource);
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
            return pageCount;
        }

        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            //mysheet.Cells[16, 16].Value = dtOuter.Rows[0]["平均值"].ToString();
            //String stringtemp = "";
            //stringtemp = "整理人：" + dtOuter.Rows[0]["整理人"].ToString();
            //stringtemp = stringtemp + "       整理日期：" + Convert.ToDateTime(dtOuter.Rows[0]["整理时间"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["整理时间"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["整理时间"].ToString()).Day.ToString() + "日";
            //mysheet.Cells[17, 1].Value = stringtemp;
            //stringtemp = "复核人：" + dtOuter.Rows[0]["审核员"].ToString();
            //stringtemp = stringtemp + "       复核日期：" + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            //mysheet.Cells[17, 7].Value = stringtemp;
            mysheet.Cells[4, 1].Value = String.Format("标准：≥{0}N/15mm,合格划√，不合格划×。", dtOuter.Rows[0]["标准"]);
            //内表信息
            int rownum = dtInner.Rows.Count;
            //无需插入的部分
            for (int i = 0; i < (rownum > 15 ? 15 : rownum); i++)
            {
                mysheet.Cells[6 + i, 1].Value = i + 1;
                mysheet.Cells[6 + i, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["生产日期"]).ToString("yyyy/MM/dd");
                mysheet.Cells[6 + i, 3].Value = Convert.ToDateTime(dtInner.Rows[i]["生产日期"]).ToString("HH:mm:ss");
                mysheet.Cells[6 + i, 4].Value = dtInner.Rows[i]["位置1"].ToString();
                mysheet.Cells[6 + i, 5].Value = dtInner.Rows[i]["位置2"].ToString();
                mysheet.Cells[6 + i, 6].Value = dtInner.Rows[i]["检测值1"].ToString();
                mysheet.Cells[6 + i, 7].Value = dtInner.Rows[i]["检测值2"].ToString();
                mysheet.Cells[6 + i, 8].Value = dtInner.Rows[i]["检测值3"].ToString();
                mysheet.Cells[6 + i, 9].Value = dtInner.Rows[i]["检测值4"].ToString();
                mysheet.Cells[6 + i, 10].Value = dtInner.Rows[i]["检测值5"].ToString();
                mysheet.Cells[6 + i, 11].Value = dtInner.Rows[i]["检测值6"].ToString();
                mysheet.Cells[6 + i, 12].Value = dtInner.Rows[i]["最小"].ToString();
                mysheet.Cells[6 + i, 13].Value = dtInner.Rows[i]["最大"].ToString();
                mysheet.Cells[6 + i, 14].Value = dtInner.Rows[i]["平均"].ToString();
                mysheet.Cells[6 + i, 15].Value = dtInner.Rows[i]["判定"].ToString() == "Y" ? "√" : "×";
                mysheet.Cells[6 + i, 16].Value = dtInner.Rows[i]["检测人"].ToString();
                mysheet.Cells[6 + i, 17].Value = dtInner.Rows[i]["审核员"].ToString();
            }
            //需要插入的部分
            if (rownum > 15)
            {
                for (int i = 15; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[7 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[6 + i, 1].Value = i + 1;
                    mysheet.Cells[6 + i, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["生产日期"]).ToString("yyyy/MM/dd");
                    mysheet.Cells[6 + i, 3].Value = Convert.ToDateTime(dtInner.Rows[i]["生产日期"]).ToString("HH:mm:ss");
                    mysheet.Cells[6 + i, 4].Value = dtInner.Rows[i]["位置1"].ToString();
                    mysheet.Cells[6 + i, 5].Value = dtInner.Rows[i]["位置2"].ToString();
                    mysheet.Cells[6 + i, 6].Value = dtInner.Rows[i]["检测值1"].ToString();
                    mysheet.Cells[6 + i, 7].Value = dtInner.Rows[i]["检测值2"].ToString();
                    mysheet.Cells[6 + i, 8].Value = dtInner.Rows[i]["检测值3"].ToString();
                    mysheet.Cells[6 + i, 9].Value = dtInner.Rows[i]["检测值4"].ToString();
                    mysheet.Cells[6 + i, 10].Value = dtInner.Rows[i]["检测值5"].ToString();
                    mysheet.Cells[6 + i, 11].Value = dtInner.Rows[i]["检测值6"].ToString();
                    mysheet.Cells[6 + i, 12].Value = dtInner.Rows[i]["最小"].ToString();
                    mysheet.Cells[6 + i, 13].Value = dtInner.Rows[i]["最大"].ToString();
                    mysheet.Cells[6 + i, 14].Value = dtInner.Rows[i]["平均"].ToString();
                    mysheet.Cells[6 + i, 15].Value = dtInner.Rows[i]["判定"].ToString() == "Y" ? "√" : "×";
                    mysheet.Cells[6 + i, 16].Value = dtInner.Rows[i]["检测人"].ToString();
                    mysheet.Cells[6 + i, 17].Value = dtInner.Rows[i]["审核员"].ToString();
                }
            }
            //加页脚
            int sheetnum;
            SqlDataAdapter da = new SqlDataAdapter("select ID from 产品热合强度检验记录 where 生产指令ID=" + dtOuter.Rows[0]["ID"].ToString(), conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
            da = new SqlDataAdapter("select ID, 生产指令编号 from 生产指令 where ID=" + dtOuter.Rows[0]["生产指令ID"].ToString(), conn);
            dt.Clear();
            da.Fill(dt);
            String Instruction = dt.Rows[0]["生产指令编号"].ToString();

            da = new SqlDataAdapter("select 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID=" + dtOuter.Rows[0]["生产指令ID"].ToString(), conn);
            dt.Clear();
            da.Fill(dt);
            String daima = dt.Rows[0]["产品代码"].ToString();
            string pihao = dt.Rows[0]["产品批号"].ToString();
            mysheet.Cells[3, 1].Value = "产品批号：" + pihao;
            mysheet.Cells[3, 6].Value = "产品代码：" + daima;
            mysheet.Cells[3, 14].Value = "生产指令：" + Instruction;
            mysheet.PageSetup.RightFooter = Instruction + "-16-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }

        private void btn数据审核_Click(object sender, EventArgs e)
        {
            HashSet<Int32> hi待审核行号 = new HashSet<int>();
            foreach (DataGridViewCell dgvc in dataGridView1.SelectedCells)
            {
                hi待审核行号.Add(dgvc.RowIndex);
            }
            //find the item in inner tagged the reviewer __待审核 and replace the content his name
            foreach (int i in hi待审核行号)
            {
                if ("__待审核" == Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim())
                {
                    if (Parameter.userName != dtInner.Rows[i]["检测人"].ToString())
                    {
                        dtInner.Rows[i]["审核员"] = Parameter.userName;
                    }
                    else
                    {
                        MessageBox.Show("记录员,审核员相同");
                    }
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            //find the uncheck item in inner list and tag the revoewer __待审核
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() == "")
                {
                    dtInner.Rows[i]["审核员"] = "__待审核";
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                continue;
            }
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            setEnableReadOnly();
        }

        private void 产品热合强度检验记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }
          
    }
}
