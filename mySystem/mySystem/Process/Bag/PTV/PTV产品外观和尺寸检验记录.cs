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
using System.Data.SqlClient;

namespace mySystem.Process.Bag.PTV
{
    public partial class PTV产品外观和尺寸检验记录 : BaseForm
    {
        // TODO   需要从Parameter 中读取生产指令ID或编号，这里假装填写当前生产指令编号和ID
        // TODO : 注意处理生产指令的状态
        private CheckForm checkform = null;
        // TODO: 打印
        // TODO：要加到mainform中去，现在连按钮都没有

        // 需要保存的状态
        /// <summary>
        /// 0:操作员，1：审核员，2：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        // 当前数据在自己表中的id
        int _id;
        String _code;

        // 显示界面需要的信息
        String str产品代码;
        String str产品批号;
        String str生产指令编号;
        Int32 i生产指令ID;
        List<String> ls操作员;
        List<String> ls审核员;


        // 数据库连接
//        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/PTV.mdb;Persist Security Info=False";
        SqlConnection conn;
        SqlDataAdapter daOuter, daInner;
        SqlCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;

        bool isFirstBind = true;

        public PTV产品外观和尺寸检验记录(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fill_printer();
            variableInit();
            getOtherData();
            getPeople();
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                if (((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] == DBNull.Value)
                    ((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = 0;
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            setFormVariable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));

            setUseState();
            setFormState();
            setEnableReadOnly();

            addComputerEventHandler();
            addOtherEvenHandler();
        }

        public PTV产品外观和尺寸检验记录(MainForm mainform, int id)
            : base(mainform)
        {
            // 待显示
            InitializeComponent();
            fill_printer();
            variableInit(id);
            getOtherData();
            getPeople();


            // 读取数据
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            // 获取和显示内容有关的变量
            setFormVariable(id);

            // 设置状态和控件可用性
            setUseState();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        void variableInit()
        {
            conn = mySystem.Parameter.conn;
            //conn.Open();
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            i生产指令ID = mySystem.Parameter.ptvbagInstruID;
            str生产指令编号 = mySystem.Parameter.ptvbagInstruction;
        }

        void variableInit(int id)
        {
            conn = mySystem.Parameter.conn;
            //conn.Open();
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            SqlDataAdapter da = new SqlDataAdapter("select * from 产品外观和尺寸检验记录 where ID=" + id, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            i生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            da = new SqlDataAdapter("select * from 生产指令 where ID=" + i生产指令ID, conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            str生产指令编号 = dt.Rows[0]["生产指令编号"].ToString();
        }

        void getOtherData()
        {
            // 读取用于显示界面的重要信息
            SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + i生产指令ID, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            str产品代码 = dt.Rows[0]["产品代码"].ToString();
            str产品批号 = dt.Rows[0]["产品批号"].ToString();
        }

        void setUseState()
        {
            //if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 0;
            //else if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 1;
            //else _userState = 2;

            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
        }

        // 读取数据，根据自己表的ID
        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 产品外观和尺寸检验记录 where ID=" + id, conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("产品外观和尺寸检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        // 读取数据，无参数表示从Paramter中读取数据
        void readOuterData()
        {
            daOuter = new SqlDataAdapter("select * from 产品外观和尺寸检验记录 where 生产指令ID=" + i生产指令ID, conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("产品外观和尺寸检验记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = i生产指令ID;
            dr["生产指令编码"] = str生产指令编号;
            dr["产品代码"] = str产品代码;
            dr["产品批号"] = str产品批号;
            dr["生产日期"] = DateTime.Now;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["操作日期"] = DateTime.Now;
            dr["审核日期"] = DateTime.Now;
            dr["外观抽检量合计"] = 0;
            dr["游离异物合计"] = 0;
            dr["内含黑点晶点合计"] = 0;
            dr["热封线不良合计"] = 0;
            dr["其他合计"] = 0;
            dr["尺寸抽检量合计"] = 0;
            dr["不良合计"] = 0;
            return dr;
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

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
                    (c as ComboBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        void readInnerData(int id)
        {
            daInner = new SqlDataAdapter("select * from 产品外观和尺寸检验记录详细信息 where T产品外观和尺寸检验记录ID=" + dtOuter.Rows[0]["ID"], conn);
            dtInner = new DataTable("产品外观和尺寸检验记录详细信息");
            cbInner = new SqlCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

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
                if (dc.ColumnName == "判定外观检查")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("合格");
                    cbc.Items.Add("不合格");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "判定尺寸检测")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("合格");
                    cbc.Items.Add("不合格");
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

            // 然后修改其他特殊属性
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

            dataGridView1.Columns[2].HeaderText = "抽样时间（外观）";
            dataGridView1.Columns[3].HeaderText = "抽样量（外观）";
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].HeaderText = "判定（外观）";
            dataGridView1.Columns[10].HeaderText = "抽样时间（尺寸）";
            dataGridView1.Columns[14].HeaderText = "判定（尺寸）";
        }

        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T产品外观和尺寸检验记录ID"] = dtOuter.Rows[0]["ID"];
            dr["抽样时间外观检查"] = DateTime.Now;
            dr["抽检量外观检查"] = 0;
            dr["游离异物"] = 0;
            dr["内含黑点晶点"] = 0;
            dr["热封线不良"] = 0;
            dr["其他"] = 0;
            dr["不良合计"] = 0;
            dr["判定外观检查"] = "合格";
            dr["抽检时间尺寸检测"] = DateTime.Now;
            dr["抽检量尺寸检测"] = 0;
            dr["宽"] = 0;
            dr["长"] = 0;
            dr["判定尺寸检测"] = "合格";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        // 获取和显示内容有关的变量
        void setFormVariable(int id)
        {
            _id = id;
        }

        void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='产品外观和尺寸检验记录'", conn);
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

        void setFormState()
        {
            //string s = dtOuter.Rows[0]["审核员"].ToString();
            //bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            //if (s == "") _formState = 0;
            //else if (s == "__待审核") _formState = 1;
            //else
            //{
            //    if (b) _formState = 2;
            //    else _formState = 3;
            //}
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
            //if (2 == _userState)
            //{
            //    setControlTrue();
            //}
            //if (1 == _userState)
            //{
            //    if (1 == _formState)
            //    {
            //        setControlTrue();
            //        btn审核.Enabled = true;
            //    }
            //    else setControlFalse();
            //}
            //if (0 == _userState)
            //{
            //    if (0 == _formState || 3 == _formState) setControlTrue();
            //    else setControlFalse();
            //}
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
            // 保证这两个按钮一直是false
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
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cb打印机.Enabled = true;
        }

        // 事件部分
        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int sum;
            switch (e.ColumnIndex)
            {
                // 外观抽检量合计
                case 3:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["抽检量外观检查"]);
                    }
                    dtOuter.Rows[0]["外观抽检量合计"] = sum;
                    break;
                // 游离异物合计
                case 4:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["游离异物"]);
                    }
                    dtOuter.Rows[0]["游离异物合计"] = sum;
                    break;
                // 内含黑点晶点合计
                case 5:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["内含黑点晶点"]);
                    }
                    dtOuter.Rows[0]["内含黑点晶点合计"] = sum;
                    break;
                // 热封线不良合计
                case 6:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["热封线不良"]);
                    }
                    dtOuter.Rows[0]["热封线不良合计"] = sum;
                    break;
                // 其他合计
                case 7:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["其他"]);
                    }
                    dtOuter.Rows[0]["其他合计"] = sum;
                    break;
                //// 不良合计
                //case 8:
                //    sum = 0;
                //    foreach (DataRow dr in dtInner.Rows)
                //    {
                //        sum += Convert.ToInt32(dr["不良合计"]);
                //    }
                //    dtOuter.Rows[0]["不良合计"] = sum;
                //    break;
                // 尺寸合计
                case 11:
                    sum = 0;
                    foreach (DataRow dr in dtInner.Rows)
                    {
                        sum += Convert.ToInt32(dr["抽检量尺寸检测"]);
                    }
                    dtOuter.Rows[0]["尺寸抽检量合计"] = sum;
                    break;
            }

            if (e.ColumnIndex >= 4 && e.ColumnIndex <= 7)
            {
                sum = 0;
                for (int i = 4; i <= 7; ++i)
                {
                    sum += Convert.ToInt32(dtInner.Rows[e.RowIndex][i]);
                }
                dtInner.Rows[e.RowIndex]["不良合计"] = sum;
                // 为什么DataGridVew中的值不会及时刷新？
                dataGridView1.Rows[e.RowIndex].Cells["不良合计"].Value = sum;
                sum = 0;
                foreach (DataRow dr in dtInner.Rows)
                {
                    sum += Convert.ToInt32(dr["不良合计"]);
                }
                dtOuter.Rows[0]["不良合计"] = sum;
            }
        }

        void addOtherEvenHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            outerBind();


            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void btn上移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (dataGridView1.SelectedCells.Count == 0) return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;
        }

        private void btn下移_Click(object sender, EventArgs e)
        {
            int count = dtInner.Rows.Count;
            if (dataGridView1.SelectedCells.Count == 0) return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dtInner.Rows[index];
            DataRow desRow = dtInner.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dtInner.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dtInner.Rows[i];
                DataRow tdesRow = dtInner.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dtInner.Rows.Add(tdesRow);
            }
            daInner.Update((DataTable)bsInner.DataSource);
            dtInner.Clear();
            daInner.Fill(dtInner);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) return;
            dtInner.Rows[dataGridView1.SelectedCells[0].RowIndex].Delete();
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            // 重新计算合计
            int sum = 0;
            foreach (DataRow dr in dtInner.Rows)
            {
                sum += Convert.ToInt32(dr["不良合计"]);
            }
            dtOuter.Rows[0]["不良合计"] = sum;
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

            da = new SqlDataAdapter("select * from 待审核 where 表名='产品外观和尺寸检验记录' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "产品外观和尺寸检验记录";
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

        bool dataValidate()
        {
            return true;
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.ShowDialog();
        }
        public override void CheckResult()
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核意见"] = checkform.opinion;
            dtOuter.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            da = new SqlDataAdapter("select * from 待审核 where 表名='产品外观和尺寸检验记录' and 对应ID=" + _id, conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";

            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
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
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\PTV\17 QB-PA-PP-03-R01A 产品外观和尺寸检验记.xlsx");
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
                return 0;
            }
            else
            {
                int pageCount = 0;

                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
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

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[3, 1].Value = "生产指令编号：" + dtOuter.Rows[0]["生产指令编码"].ToString();
            mysheet.Cells[3, 6].Value = "产品代码：" + dtOuter.Rows[0]["产品代码"].ToString();
            mysheet.Cells[3, 12].Value = "产品批号："+dtOuter.Rows[0]["产品批号"].ToString();
            //mysheet.Cells[3, 10].Value = "生产日期：" + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"].ToString()).Day.ToString() + "日";
            //mysheet.Cells[12, 2].Value = dtOuter.Rows[0]["外观抽检量合计"].ToString();
            //mysheet.Cells[12, 3].Value = dtOuter.Rows[0]["游离异物合计"].ToString();
            //mysheet.Cells[12, 4].Value = dtOuter.Rows[0]["内含黑点晶点合计"].ToString();
            //mysheet.Cells[12, 5].Value = dtOuter.Rows[0]["热封线不良合计"].ToString();
            //mysheet.Cells[12, 6].Value = dtOuter.Rows[0]["其他合计"].ToString();
            //mysheet.Cells[12, 7].Value = dtOuter.Rows[0]["不良合计"].ToString();
            //mysheet.Cells[12, 8].Value = dtOuter.Rows[0]["判定"].ToString() == "Yes" ? "√" : "×";
            //mysheet.Cells[13, 9].Value = "尺寸规格： 宽 " + dtOuter.Rows[0]["尺寸规格宽"].ToString() + " mm × 长 " + dtOuter.Rows[0]["尺寸规格长"].ToString() + " mm（标示±5mm）";
            //String stringtemp = "";
            //stringtemp = "检测人：" + dtOuter.Rows[0]["操作员"].ToString();
            //stringtemp = stringtemp + "       检测日期：" + Convert.ToDateTime(dtOuter.Rows[0]["操作日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["操作日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["操作日期"].ToString()).Day.ToString() + "日";
            //mysheet.Cells[16, 2].Value = stringtemp;
            //stringtemp = "复核人：" + dtOuter.Rows[0]["审核员"].ToString();
            //stringtemp = stringtemp + "       复核日期：" + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"].ToString()).Day.ToString() + "日";
            //mysheet.Cells[16, 9].Value = stringtemp;
            //内表信息
            int rownum = dtInner.Rows.Count;
            int addedN = rownum - 9;
            if (addedN < 0) addedN = 0;
            if (rownum > 9)
            {
                for (int i = 9; i < rownum; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[9, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
            }

            //无需插入的部分
            for (int i = 0; i < (rownum > 9 ? 9 : rownum); i++)
            {
                mysheet.Cells[6 + i, 1].Value = Convert.ToDateTime(dtInner.Rows[i]["抽样时间外观检查"]).ToString("yyyy/MM/dd");
                mysheet.Cells[6 + i, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["抽样时间外观检查"]).ToString("HH:mm:ss");
                mysheet.Cells[6 + i, 3].Value = dtInner.Rows[i]["抽检量外观检查"].ToString();
                mysheet.Cells[6 + i, 4].Value = dtInner.Rows[i]["游离异物"].ToString();
                mysheet.Cells[6 + i, 5].Value = dtInner.Rows[i]["内含黑点晶点"].ToString();
                mysheet.Cells[6 + i, 6].Value = dtInner.Rows[i]["热封线不良"].ToString();
                mysheet.Cells[6 + i, 7].Value = dtInner.Rows[i]["其他"].ToString();
                mysheet.Cells[6 + i, 8].Value = dtInner.Rows[i]["不良合计"].ToString();
                mysheet.Cells[6 + i, 9].Value = dtInner.Rows[i]["判定外观检查"].ToString() == "Yes" ? "√" : "×";
                //mysheet.Cells[6 + i, 10].Value = Convert.ToDateTime(dtInner.Rows[i]["抽检时间尺寸检测"].ToString()).ToString("yyyy/MM/dd HH:mm");
                mysheet.Cells[6 + i, 10].Value = dtInner.Rows[i]["抽检量尺寸检测"].ToString();
                mysheet.Cells[6 + i, 11].Value = dtInner.Rows[i]["宽"].ToString() + " × " + dtInner.Rows[i]["长"].ToString();
                mysheet.Cells[6 + i, 12].Value = dtInner.Rows[i]["判定尺寸检测"].ToString() == "Yes" ? "√" : "×";
            }
            //需要插入的部分
            if (rownum > 9)
            {
                for (int i = 9; i < rownum; i++)
                {
                    //Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[6 + i-1, Type.Missing];

                    //range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    //    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    mysheet.Cells[6 + i, 1].Value = Convert.ToDateTime(dtInner.Rows[i]["抽样时间外观检查"]).ToString("yyyy/MM/dd");
                    mysheet.Cells[6 + i, 2].Value = Convert.ToDateTime(dtInner.Rows[i]["抽样时间外观检查"]).ToString("HH:mm:ss");
                    mysheet.Cells[6 + i, 3].Value = dtInner.Rows[i]["抽检量外观检查"].ToString();
                    mysheet.Cells[6 + i, 4].Value = dtInner.Rows[i]["游离异物"].ToString();
                    mysheet.Cells[6 + i, 5].Value = dtInner.Rows[i]["内含黑点晶点"].ToString();
                    mysheet.Cells[6 + i, 6].Value = dtInner.Rows[i]["热封线不良"].ToString();
                    mysheet.Cells[6 + i, 7].Value = dtInner.Rows[i]["其他"].ToString();
                    mysheet.Cells[6 + i, 8].Value = dtInner.Rows[i]["不良合计"].ToString();
                    mysheet.Cells[6 + i, 9].Value = dtInner.Rows[i]["判定外观检查"].ToString() == "Yes" ? "√" : "×";
                    //mysheet.Cells[6 + i, 10].Value = Convert.ToDateTime(dtInner.Rows[i]["抽检时间尺寸检测"].ToString()).ToString("yyyy/MM/dd HH:mm");
                    mysheet.Cells[6 + i, 10].Value = dtInner.Rows[i]["抽检量尺寸检测"].ToString();
                    mysheet.Cells[6 + i, 11].Value = dtInner.Rows[i]["宽"].ToString() + " × " + dtInner.Rows[i]["长"].ToString();
                    mysheet.Cells[6 + i, 12].Value = dtInner.Rows[i]["判定尺寸检测"].ToString() == "Yes" ? "√" : "×";

                }
            }
            mysheet.Cells[15+addedN, 3].Value = dtOuter.Rows[0]["外观抽检量合计"].ToString();
            mysheet.Cells[15 + addedN, 4].Value = dtOuter.Rows[0]["游离异物合计"].ToString();
            mysheet.Cells[15 + addedN, 5].Value = dtOuter.Rows[0]["内含黑点晶点合计"].ToString();
            mysheet.Cells[15 + addedN, 6].Value = dtOuter.Rows[0]["热封线不良合计"].ToString();
            mysheet.Cells[15 + addedN, 7].Value = dtOuter.Rows[0]["其他合计"].ToString();
            mysheet.Cells[15 + addedN, 8].Value = dtOuter.Rows[0]["不良合计"].ToString();
            mysheet.Cells[15 + addedN, 10].Value = dtOuter.Rows[0]["尺寸抽检量合计"].ToString();
            string ttt = "尺寸规格： 宽   {0}       mm×长   {1}         mm（标示±5mm）"+"\n"+
"尺寸检测规则："+"\n"+
"每2小时1次，每次测量3只产品，长和宽均应符合标准；记录第一个样品的实际测量值，其他样品确认是否在标准范围内。每批不少于8只,均应合格。 Roll bag只确认宽度。";
            mysheet.Cells[16 + addedN, 10].Value = string.Format(ttt, dtOuter.Rows[0]["尺寸规格宽"], dtOuter.Rows[0]["尺寸规格长"]);
            //加页脚
            int sheetnum;
            SqlDataAdapter da = new SqlDataAdapter("select ID from 产品外观和尺寸检验记录 where 生产指令ID=" + dtOuter.Rows[0]["ID"].ToString(), conn);
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
            mysheet.PageSetup.RightFooter = Instruction + "-16-" + sheetnum.ToString("D3") + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }

        private void PTV产品外观和尺寸检验记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
                writeDGVWidthToSetting(dataGridView1);
        }

    }
}
