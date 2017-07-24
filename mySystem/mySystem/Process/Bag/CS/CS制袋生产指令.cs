using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Bag.CS
{
    public partial class CS制袋生产指令 : BaseForm
    {


        // TODO : 注意处理生产指令的状态
        // TODO ：要加到Mainform中去
        // TODO： 审核时要调用赵梦的函数
        // TODO: 打印

        // 需要保存的状态
        /// <summary>
        /// 0:操作员，1：审核员，2：管理员
        /// </summary>
        int _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        int _formState;
        int _id;
        String _code;

        // 显示界面需要的信息
        List<String> ls产品名称;
        List<String> ls工艺;
        List<String> ls产品代码;
        List<String> ls负责人;
        List<String> ls封边;
        List<String> ls操作员;
        List<String> ls审核员;
        HashSet<String> hs制袋内包白班负责人, hs制袋内包夜班负责人, hs外包白班负责人, hs外包夜班负责人;

        // DataGridView 中用到的一些变量
        List<Int32> li可选可输的列;


        // 数据库连接
        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;

        public CS制袋生产指令()
        {
            InitializeComponent();
            variableInit();
            getOtherData();
            getPeople();
            setUseState();

            // 为了控制新界面的控件可用性，必须加的
            setFormState(true);
            setEnableReadOnly();
        }

        public CS制袋生产指令(int id)
        {
            // 待显示
            InitializeComponent();
            variableInit();
            getOtherData();
            getPeople();
            setUseState();

            // 读取数据
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            // 获取和显示内容有关的变量
            setFormVariable(id);

            // 设置状态和控件可用性
            getPeople();
            
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        private void btn查询插入_Click(object sender, EventArgs e)
        {
            // 读取数据
            _code = tb生产指令编号.Text;
            readOuterData(_code);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(dtOuter.Rows[0]["生产指令编号"].ToString());
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();

            // 获取和显示内容有关的变量
            setFormVariable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));

            // 设置状态和控件可用性
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();

            // 禁用自己
            btn查询插入.Enabled = false;
            tb生产指令编号.Enabled = false;
        }

        void variableInit()
        {
            conn = new OleDbConnection(strConn);
            hs外包白班负责人 = new HashSet<string>();
            hs外包夜班负责人 = new HashSet<string>();
            hs制袋内包白班负责人 = new HashSet<string>();
            hs制袋内包夜班负责人 = new HashSet<string>();
            li可选可输的列 = new List<int>();
            li可选可输的列.Add(2);
            li可选可输的列.Add(8);
        }

        void setFormVariable(int id)
        {

            _id = id;
            _code = dtOuter.Rows[0]["生产指令编号"].ToString();
        }
        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='CS制袋生产指令'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

        }
        void getOtherData()
        {
            OleDbDataAdapter da;
            DataTable dt;
            ls产品代码 = new List<string>();
            ls产品名称 = new List<string>();
            ls负责人 = new List<string>();
            ls工艺 = new List<string>();
            ls封边 = new List<string>();

            da = new OleDbDataAdapter("select * from 用户", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls负责人.Add(dr["用户名"].ToString());
                cmb负责人.Items.Add(dr["用户名"].ToString());
            }
            //　产品名称
            da = new OleDbDataAdapter("select * from 设置CS制袋产品", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows){
                ls产品名称.Add(dr["产品名称"].ToString());
                cmb产品名称.Items.Add(dr["产品名称"].ToString());
            }
            
            //　产品代码
            da = new OleDbDataAdapter("select * from 设置CS制袋产品代码", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls产品代码.Add(dr["产品代码"].ToString());
            }
            // 工艺
            da = new OleDbDataAdapter("select * from 设置CS制袋工艺", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls工艺.Add(dr["工艺名称"].ToString());
                cmb生产工艺.Items.Add(dr["工艺名称"].ToString());
            }
            // 封边
            da = new OleDbDataAdapter("select * from 设置CS制袋封边", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls封边.Add(dr["封边名称"].ToString());
            }

        }
        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 生产指令 where ID=" + id + "", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("生产指令");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }
        void readOuterData(String code)
        {
            
            daOuter = new OleDbDataAdapter("select * from 生产指令 where 生产指令编号='" + code + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("生产指令");
            bsOuter = new BindingSource();

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
        void readInnerData(int outerID)
        {
            daInner = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + outerID, conn);
            dtInner = new DataTable("生产指令详细信息");
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }
        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;

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
                if (dc.ColumnName == "产品代码")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (String s in ls产品代码)
                    {
                        cbc.Items.Add(s);
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "封边")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (String s in ls封边)
                    {
                        cbc.Items.Add(s);
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "内标签")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("中文");
                    cbc.Items.Add("英文");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "外标签")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("中文");
                    cbc.Items.Add("英文");
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
            dataGridView1.Columns[2].HeaderText = "产品代码（规格型号）";
            dataGridView1.Columns[3].HeaderText = "计划产量（只）";
            dataGridView1.Columns[4].HeaderText = "内包装规格（只/包）";
            dataGridView1.Columns[9].HeaderText = "外包装规格（只/箱）";
           
        }

        void assignNOToRowInDataGridView()
        {

        }
        void setUseState()
        {
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 0;
            else if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 1;
            else _userState = 2;
        }
        // 如果给了一个参数为true，则表示处于无数据状态
        void setFormState(bool newForm = false)
        {
            if (newForm)
            {

                _formState = -1;
                return;
            }
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = 1;
            else
            {
                if (b) _formState = 2;
                else _formState = 3;
            }
        }
        void setEnableReadOnly()
        {
            
            if(-1==_formState){
                setControlFalse();
                btn查询插入.Enabled = true;
                tb生产指令编号.ReadOnly = false;
                return ;
            }
            if (2 == _userState)
            {
                setControlTrue();
            }
            if (1 == _userState)
            {
                if (1 == _formState)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (0 == _userState)
            {
                if (0 == _formState || 3 == _formState) setControlTrue();
                else setControlFalse();
            }

  
        }

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

        }
        void addOtherEvenHandler()
        {
            // TODO 其他无法分类的代码放在这里
            dataGridView1.AllowUserToAddRows = false;
            // 实现下拉框可选可输
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
        }

        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (li可选可输的列.IndexOf(e.ColumnIndex) >= 0)
            {
                object eFV = e.FormattedValue;
                DataGridViewComboBoxColumn cbc = dataGridView1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (!cbc.Items.Contains(eFV))
                {
                    cbc.Items.Add(eFV);
                    dataGridView1.SelectedCells[0].Value = eFV;
                }
            }
        }

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colIdx = (sender as DataGridView).SelectedCells[0].ColumnIndex;
            if (li可选可输的列.IndexOf(colIdx) >= 0)
            {
                ComboBox c = e.Control as ComboBox;
                if (c != null) c.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }
        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int i计划产量只 = Convert.ToInt32( dataGridView1[3,e.RowIndex].Value);
            int i内包装规格 = Convert.ToInt32(dataGridView1[4, e.RowIndex].Value);
            int i外包装规格 = Convert.ToInt32(dataGridView1[9, e.RowIndex].Value);
            try
            {


                switch (e.ColumnIndex)
                {
                    // 计划产量
                    case 3:
                        //灭菌指示剂
                        dtOuter.Rows[0]["制袋物料领料量3"] = i计划产量只.ToString();
                        //tb制袋物料领料量3.Text = i计划产量只.ToString();
                        //内包装
                        //tb内包物料领料量1.Text = (i计划产量只 / i内包装规格 * 2).ToString();
                        dtOuter.Rows[0]["内包物料领料量1"] = (i计划产量只 / i内包装规格 * 2).ToString();
                        // 内标签
                        //tb内包物料领料量2.Text = (i计划产量只 / i内包装规格).ToString();
                        dtOuter.Rows[0]["内包物料领料量2"] = (i计划产量只 / i内包装规格).ToString();
                        // 外标签
                        //tb外包物料领料量1.Text = (i计划产量只 / i外包装规格 * 2).ToString();
                        dtOuter.Rows[0]["外包物料领料量1"] = (i计划产量只 / i外包装规格 * 2).ToString();
                        // 纸箱
                        //tb外包物料领料量2.Text = (i计划产量只 / i外包装规格).ToString();
                        dtOuter.Rows[0]["外包物料领料量2"] = (i计划产量只 / i外包装规格).ToString();
                        // 内衬袋
                        dtOuter.Rows[0]["外包物料领料量3"] = (i计划产量只 / i外包装规格).ToString();
                        //tb外包物料领料量3.Text = (i计划产量只 / i外包装规格).ToString();
                        break;
                    // 内包装规格
                    case 4:
                        //内包装
                        //tb内包物料领料量1.Text = (i计划产量只 / i内包装规格 * 2).ToString();
                        dtOuter.Rows[0]["内包物料领料量1"] = (i计划产量只 / i内包装规格 * 2).ToString();
                        // 内标签
                        //tb内包物料领料量2.Text = (i计划产量只 / i内包装规格).ToString();
                        dtOuter.Rows[0]["内包物料领料量2"] = (i计划产量只 / i内包装规格).ToString();
                        break;
                    // 外包装规格
                    case 9:
                        // 外标签
                        //tb外包物料领料量1.Text = (i计划产量只 / i外包装规格 * 2).ToString();
                        dtOuter.Rows[0]["外包物料领料量1"] = (i计划产量只 / i外包装规格 * 2).ToString();
                        // 纸箱
                        //tb外包物料领料量2.Text = (i计划产量只 / i外包装规格).ToString();
                        dtOuter.Rows[0]["外包物料领料量2"] = (i计划产量只 / i外包装规格).ToString();
                        // 内衬袋
                        //tb外包物料领料量3.Text = (i计划产量只 / i外包装规格).ToString();
                        dtOuter.Rows[0]["外包物料领料量3"] = (i计划产量只 / i外包装规格).ToString();
                        break;
                }
            }
            catch (System.DivideByZeroException)
            {

            }
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = _code;
            dr["生产设备"] = "制袋机 AA-EQU-001";
            dr["计划生产日期"] = DateTime.Now;
            dr["制袋物料名称1"] = "Tyvek印刷卷材";
            dr["制袋物料名称2"] = "药品包装用聚乙烯膜（XP1）";
            dr["制袋物料名称3"] = "蒸汽灭菌指示剂";
            dr["内包物料名称1"] = "内包装袋";
            dr["内包物料名称2"] = "内标签";
            dr["外包物料名称1"] = "外标签";
            dr["外包物料名称2"] = "纸箱";
            dr["外包物料批号2"] = "————————";
            dr["外包物料名称3"] = "内衬袋";
            dr["外包物料代码3"] = "专用袋";
            dr["外包物料批号3"] = "————————";
            dr["操作员"] = mySystem.Parameter.userName;
            dr["操作时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["接收时间"] = DateTime.Now;
            dr["状态"] = 0;
            return dr;
        }

        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T生产指令ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["计划产量只"] = 0;
            dr["内包装规格每包只数"] = 0;
            dr["外包规格"] = 0;
            dr["封边"] = "底封";
            return dr;
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

            if (_userState == 0) btn提交审核.Enabled = true;
            
        }

        private void btn外包白班_Click(object sender, EventArgs e)
        {
            hs外包白班负责人.Add(cmb负责人.SelectedItem.ToString());
            dtOuter.Rows[0]["外包白班负责人"] = String.Join(",", hs外包白班负责人.ToList<String>().ToArray());
            //tb外包白班负责人.Text = 
        }

        private void btn外包夜班_Click(object sender, EventArgs e)
        {
            hs外包夜班负责人.Add(cmb负责人.SelectedItem.ToString());
            dtOuter.Rows[0]["外包夜班负责人"] = String.Join(",", hs外包夜班负责人.ToList<String>().ToArray());
            //tb外包夜班负责人.Text = String.Join(",", hs外包夜班负责人.ToList<String>().ToArray());
        }

        private void btn制袋内包白班_Click(object sender, EventArgs e)
        {
            hs制袋内包白班负责人.Add(cmb负责人.SelectedItem.ToString());
            dtOuter.Rows[0]["制袋内包白班负责人"] = String.Join(",", hs制袋内包白班负责人.ToList<String>().ToArray());
            //tb制袋内包白班负责人.Text = String.Join(",", hs制袋内包白班负责人.ToList<String>().ToArray());
        }

        private void btn制袋内包夜班_Click(object sender, EventArgs e)
        {
            hs制袋内包夜班负责人.Add(cmb负责人.SelectedItem.ToString());
            dtOuter.Rows[0]["制袋内包夜班负责人"] = String.Join(",", hs制袋内包夜班负责人.ToList<String>().ToArray());
            //tb制袋内包夜班负责人.Text = String.Join(",", hs制袋内包夜班负责人.ToList<String>().ToArray());
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {

            if (!dataValidate())
            {
                MessageBox.Show("数据填写不完整，请仔细检查！");
                return;
            }

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;
            
            da = new OleDbDataAdapter("select * from 待审核 where 表名='生产指令' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);
            
            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "生产指令";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log+= "\n操作员："+mySystem.Parameter.userName+" 提交审核\n";
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
                MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
            
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            // TODO 弹出赵梦的窗口

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='生产指令' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核是否通过"] = true;
            String log = "===================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            log += "审核结果为：通过\n";
            log += "审核意见为：无\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
        }



        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
        }


        // 验证数据有效性
        private bool dataValidate()
        {
            // TODO 更多条件有待补充
            if (cmb产品名称.Text == "") return false;
            if (cmb生产工艺.Text == "") return false;
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows.Count > 1) return false;
            if (tb接收人.Text == "") return false;
            return true;
        }
 
    }
}
