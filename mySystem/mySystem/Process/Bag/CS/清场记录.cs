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
    public partial class 清场记录 : BaseForm
    {
        // TODO   需要从Parameter 中读取生产指令ID或编号，这里假装填写当前生产指令编号和ID
        string CODE;
        int ID;
        // TODO : 注意处理生产指令的状态
        // TODO： 审核时要调用赵梦的函数
        // TODO: 打印
        // TODO：要加到mainform中去，现在连按钮都没有

        // 需要保存的状态
        /// <summary>
        /// 0:操作员，1：审核员，2：管理员
        /// </summary>
        int _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        int _formState;
        // 当前数据在自己表中的id
        int _id;
        String _code;

        // 显示界面需要的信息
        String str产品代码;
        String str产品批号;
        Int32 i生产指令ID;
        List<String> ls操作员;
        List<String> ls审核员;
        List<String> ls清场项目;
        List<String> ls清场要点;


        // 数据库连接
        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;


        public 清场记录()
        {
            // 判断设置是否变化
            InitializeComponent();
            variableInit();
            // 若为未保存状态，则判断设置是否变化
            getOtherData();
            getPeople();


            // 读取数据
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData();
                outerBind();
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                setDataGridViewColumn();
                innerBind();
                generateRowOfDataTableInner();
                setDataGridViewNO();

                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }
            if (null == dtInner)
            {
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                setDataGridViewColumn();
                innerBind();
            }
            matchInnerData();

            // 获取和显示内容有关的变量
            setFormVariable(Convert.ToInt32(dtOuter.Rows[0]["ID"]));

            // 设置状态和控件可用性
            setUseState();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            addComputerEventHandler();
            addOtherEvenHandler();
        }

        public 清场记录(int id)
        {
            
            InitializeComponent();
            variableInit(id);
            // 若为未保存状态，则判断设置是否变化
            getOtherData();
            getPeople();


            // 读取数据
            readOuterData(id);
            outerBind();
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumn();
            innerBind();
            setDataGridViewNO();
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
            conn = new OleDbConnection(strConn);
            conn.Open();
            ID = mySystem.Parameter.csbagInstruID;
            i生产指令ID = ID;
            CODE = mySystem.Parameter.csbagInstruction;
            

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            ls清场项目 = new List<string>();
            ls清场要点 = new List<string>();
            
        }

        void variableInit(int id)
        {
            conn = new OleDbConnection(strConn);
            conn.Open();

            OleDbDataAdapter da = new OleDbDataAdapter("select * from 清场记录 where ID=" + id, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            
            i生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            ID = i生产指令ID;
            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            ls清场项目 = new List<string>();
            ls清场要点 = new List<string>();
        }

        void getOtherData()
        {
            // 读取用于显示界面的重要信息
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + i生产指令ID, conn);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            str产品代码 = dt.Rows[0]["产品代码"].ToString();
            str产品批号 = dt.Rows[0]["产品批号"].ToString();

            da = new OleDbDataAdapter("select * from 设置清场记录", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls清场项目.Add(dr["清场项目"].ToString());
                ls清场要点.Add(dr["清场要点"].ToString());
            }

            cmb检查结果.Items.Add("合格");
            cmb检查结果.Items.Add("不合格");
        }

        void setUseState()
        {
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 0;
            else if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState = 1;
            else _userState = 2;
        }

        // 读取数据，根据自己表的ID
        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 清场记录 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("清场记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        // 读取数据，无参数表示从Paramter中读取数据
        void readOuterData()
        {
            String sql = "select * from 清场记录 where 生产指令ID={0} and 生产日期=#{1}# and 生产班次='{2}'";
            DateTime date = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            daOuter = new OleDbDataAdapter(String.Format(sql, i生产指令ID, date, mySystem.Parameter.userflight), conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("清场记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = i生产指令ID;
            dr["产品代码"] = str产品代码;
            dr["产品批号"] = str产品批号;
            dr["生产日期"] = DateTime.Parse( DateTime.Now.ToString("yyyy/MM/dd"));
            dr["生产班次"] = mySystem.Parameter.userflight;
            dr["操作员"] = mySystem.Parameter.userName;
            dr["检查结果"] = "合格";
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
            daInner = new OleDbDataAdapter("select * from 清场记录详细信息 where T清场记录ID=" + dtOuter.Rows[0]["ID"], conn);
            dtInner = new DataTable("清场记录详细信息");
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void matchInnerData()
        {
            if (0 != _formState) return;
            bool isChanged = false;
            if (ls清场要点.Count != dtInner.Rows.Count) isChanged = true;
            else
            {
                for (int i = 0; i < ls清场项目.Count; ++i)
                {
                    if (dtInner.Rows[i]["清场项目"].ToString() != ls清场项目[i] ||
                        dtInner.Rows[i]["清场要点"].ToString() != ls清场要点[i])
                    {
                        isChanged = true;
                    }
                }
            }

            if (isChanged)
            {
                if (DialogResult.OK == MessageBox.Show("检测到清场设置项已被修改，是否使用最新的设置?",
                    "提示", MessageBoxButtons.OKCancel))
                {
                    foreach (DataRow dr in dtInner.Rows) dr.Delete();
                    daInner.Update((DataTable)bsInner.DataSource);
                    readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                    innerBind();
                    generateRowOfDataTableInner();
                    daInner.Update((DataTable)bsInner.DataSource);
                    readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                    innerBind();
                }
            }

        }

        void generateRowOfDataTableInner()
        {
            for (int i = 0; i < ls清场项目.Count; ++i)
            {
                DataRow dr = dtInner.NewRow();
                dr = writeInnerDefault(dr, ls清场项目[i], ls清场要点[i]);
                dtInner.Rows.Add(dr);
            }
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
                if (dc.ColumnName == "清洁操作")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("完成");
                    cbc.Items.Add("不适用");
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
            
        }

        // 写序号
        void setDataGridViewNO()
        {

            for (int i = 0; i < dtInner.Rows.Count;++i )
            {
                dtInner.Rows[i]["序号"] = i + 1;
            }
        }

        DataRow writeInnerDefault(DataRow dr, String xm, String yd)
        {
            dr["T清场记录ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dr["序号"] = 0;
            dr["清场项目"] = xm;
            dr["清场要点"] = yd;
            dr["清洁操作"] = "完成";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;

            dataGridView1.DataSource = bsInner.DataSource;
        }

        // 获取和显示内容有关的变量
        void setFormVariable(int id)
        {
            _id = id;
        }


        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='清场记录'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

        }

        void setFormState()
        {
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

        }

        // 事件部分
        void addComputerEventHandler()
        {
        }

     
        void addOtherEvenHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // DataGridView的可见和只读等属性最好在这个事件中处理
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            
            for (int i = 0; i < 5; ++i)
            {
                dataGridView1.Columns[i].ReadOnly = true;
            }
                
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

            da = new OleDbDataAdapter("select * from 待审核 where 表名='清场记录' and 对应ID=" + _id, conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "清场记录";
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
            // TODO 弹出赵梦的窗口

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='清场记录' and 对应ID=" + _id, conn);
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
       

    }
}
