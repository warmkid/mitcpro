using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;


namespace mySystem.Process.Stock
{
    public partial class 物资验收记录 : BaseForm
    {
        bool isSaved = false;
        List<String> ls操作员;
        List<String> ls审核员;
        List<String> ls供应商代码,ls供应商名称;
        List<String> ls物料名称, ls物料代码;
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        mySystem.Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        mySystem.Parameter.FormState _formState;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";

        OleDbConnection conn;
        CheckForm ckform;

        public 物资验收记录(MainForm mainform):base(mainform)
        {
            // TODO 审核人不走这条路
            InitializeComponent();
            conn = new OleDbConnection(strConn);
            conn.Open();
            getPeople();
            setUserState();
            getOtherData();
            
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefaultValue(dr);
                dtOuter.Rows.Add(dr);

                daOuter.Update((DataTable)bsOuter.DataSource);
                OleDbCommand comm = new OleDbCommand();
                
                comm.Connection = conn;
                comm.CommandText = "select @@identity";
                Int32 idd1 = (Int32)comm.ExecuteScalar();
                readOuterData(idd1);
                outerBind();
            }

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();

            addComputerEvnetHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();

        }

        void cmb供应商名称_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl供应商代码.Text = ls供应商代码[(sender as ComboBox).SelectedIndex];
            lbl供应商代码.DataBindings[0].WriteValue();
        }


        public 物资验收记录(MainForm mainform,  int id):base(mainform)
        {
            InitializeComponent();
            isSaved = true;
            conn = new OleDbConnection(strConn);
            conn.Open();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData(id);
            outerBind();
            

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            getInnerOtherData();
            setDataGridViewColumn();
            innerBind();

            addComputerEvnetHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();

            
        }
        //getPeople()--> setUserState()--> getOtherData()-->
        // 读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->
        //addComputerEventHandler()--> setFormState()--> setEnableReadOnly() --> 
        //addOtherEvnetHandler()

        void getPeople()
        {
            // TODO
            ls审核员 = new List<string>();
            ls操作员 = new List<string>();
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 库存用户权限 where 步骤='" + "物资验收记录" + "'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户权限设置有误，为避免出现错误，请尽快联系管理员完成设置！");
                this.Dispose();
            }
            
            string str操作员 = dt.Rows[0]["操作员"].ToString();
            string str审核员 = dt.Rows[0]["审核员"].ToString();
            String[] tmp = Regex.Split(str操作员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls操作员.Add(s);
                }
            }
            tmp = Regex.Split(str审核员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls审核员.Add(s);
                }
            }
        }

        void setUserState()
        {
            // TODO

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

        void getOtherData()
        {
            // TODO
            ls供应商代码 = new List<string>();
            ls供应商名称 = new List<string>();
            //ls供应商代码.Add("厂家1");

            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 设置供应商信息", conn);
            dt = new DataTable("tmp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls供应商代码.Add(dr["供应商代码"].ToString());
                ls供应商名称.Add(dr["供应商名称"].ToString());
            }
        }

        void readOuterData(int id = -1)
        {
            conn = new OleDbConnection(strConn);

            conn.Open();
            if (-1 == id)
            {
                daOuter = new OleDbDataAdapter("select * from 物资验收记录 where 验收记录编号='" + lbl验收记录编号.Text + "'", conn);
            }
            else
            {
                daOuter = new OleDbDataAdapter("select * from 物资验收记录 where ID=" + id, conn);
            }

            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("物资验收记录");
            daOuter.Fill(dtOuter);

            
            
        }

        DataRow writeOuterDefaultValue(DataRow dr)
        {
            dr["接收时间"] = DateTime.Now;
            dr["验收人"] = mySystem.Parameter.userName;
            dr["请验人"] = mySystem.Parameter.userName;
            dr["请验时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["验收记录编号"] = create验收记录编号();
            return dr;
        }

        String create验收记录编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 验收记录编号 from 物资验收记录 order by ID DESC", conn);
            DataTable dt = new DataTable("物资验收记录");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count==0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            else
            {
                string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
                string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
                if (Int32.Parse(yearInCode) == yearNow)
                {
                    codeNow = Int32.Parse(NOInCode) + 1;
                }
                else
                {
                    codeNow = 1;
                }
                return yearNow.ToString() + codeNow.ToString("D4");
            }
        }

        void outerBind()
        {

            bsOuter.DataSource = dtOuter;

            cmb供应商名称.DataBindings.Clear();
            lbl供应商代码.DataBindings.Clear();
            lbl供应商代码.DataBindings.Add("Text", bsOuter.DataSource, "供应商代码");
            dtp接收时间.DataBindings.Clear();
            tb验收人.DataBindings.Clear();


            cmb厂家随附检验报告.DataBindings.Clear();

            tb检验报告理由.DataBindings.Clear();

            cmb是否有样品.DataBindings.Clear();

            tb样品理由.DataBindings.Clear();

            tb请验人.DataBindings.Clear();
            dtp请验时间.DataBindings.Clear();

            tb审核员.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            lbl验收记录编号.DataBindings.Clear();
            
            // ----


            cmb供应商名称.DataSource = ls供应商名称;
            cmb供应商名称.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb供应商名称.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //cmb供应商名称.Items.Clear();


            //foreach (String s in ls供应商名称)
            //{
            //    cmb供应商名称.Items.Add(s);
            //}

            cmb供应商名称.DataBindings.Add("SelectedItem", bsOuter.DataSource, "供应商名称");
            
            dtp接收时间.DataBindings.Add("Value", bsOuter.DataSource, "接收时间");
            tb验收人.DataBindings.Add("Text", bsOuter.DataSource, "验收人");

            cmb厂家随附检验报告.Items.Clear();
            cmb厂家随附检验报告.Items.Add("无");
            cmb厂家随附检验报告.Items.Add("不齐全");
            cmb厂家随附检验报告.Items.Add("齐全");
            cmb厂家随附检验报告.DataBindings.Add("SelectedItem", bsOuter.DataSource, "厂家随附检验报告");

            tb检验报告理由.DataBindings.Add("Text", bsOuter.DataSource, "无检验报告理由");

            cmb是否有样品.Items.Clear();
            cmb是否有样品.Items.Add("有");
            cmb是否有样品.Items.Add("无");
            cmb是否有样品.DataBindings.Add("SelectedItem", bsOuter.DataSource, "是否有样品");

            tb样品理由.DataBindings.Add("Text", bsOuter.DataSource, "无样品理由");

            tb请验人.DataBindings.Add("Text", bsOuter.DataSource, "请验人");
            dtp请验时间.DataBindings.Add("Value", bsOuter.DataSource, "请验时间");

            tb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");

            lbl验收记录编号.DataBindings.Add("Text", bsOuter.DataSource, "验收记录编号");
            
        }

        void readInnerData(int id)
        {

            daInner = new OleDbDataAdapter("select * from 物资验收记录详细信息 where 物资验收记录ID=" + id, strConn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("物资验收记录详细信息");
            daInner.Fill(dtInner);
        }

        DataRow writeInnerDefaultValue(DataRow dr)
        {
            dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            dr["数量"] = 0;
            dr["是否需要检验"] = "否";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        void addComputerEvnetHandler()
        {

        }

        void setFormState()
        {
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核结果"]);
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
            //    if (0 == _formState || 2 == _formState || 3 == _formState)
            //    {
            //        setControlFalse();
            //    }
            //    else
            //    {
            //        setControlTrue();
            //        btn审核.Enabled = true;
            //    }
            //}
            //if (0 == _userState)
            //{
            //    if (0 == _formState || 3 == _formState)
            //    {
            //        setControlTrue();
            //    }
            //    if (1 == _formState || 2 == _formState)
            //    {
            //        setControlFalse();
            //    }
            //}
        }

        void addOtherEventHandler()
        {
            foreach (ToolStripItem tsi in contextMenuStrip1.Items)
            {
                tsi.Click += new EventHandler(tsi_Click);
            }
            dataGridView1.AllowUserToAddRows = false;
            cmb供应商名称.SelectedIndexChanged += new EventHandler(cmb供应商名称_SelectedIndexChanged);
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);

            // 物料代码和物料名称可选可输
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            String curStr;
            double curDou;
            int idx;
            bool ok;
            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "物料代码":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls物料代码.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["物料代码", e.RowIndex].Value = ls物料代码[idx];
                        dataGridView1["物料名称", e.RowIndex].Value = ls物料名称[idx];
                    }
                    else
                    {
                        dataGridView1["物料代码", e.RowIndex].Value = "";
                        dataGridView1["物料名称", e.RowIndex].Value = "";
                    }
                    break;
                case "物料名称":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls物料名称.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["物料代码", e.RowIndex].Value = ls物料代码[idx];
                        dataGridView1["物料名称", e.RowIndex].Value = ls物料名称[idx];
                    }
                    else
                    {
                        dataGridView1["物料代码", e.RowIndex].Value = "";
                        dataGridView1["物料名称", e.RowIndex].Value = "";
                    }
                    break;
            }
        }

        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dgv = (sender as DataGridView);
            if (dgv.SelectedCells.Count == 0) return;
            int colIdx = dgv.SelectedCells[0].ColumnIndex;
            if (2==colIdx || 3==colIdx)
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
            DataGridView dgv = (sender as DataGridView);

            if (dgv.SelectedCells.Count == 0) return;
            int colIdx = dgv.SelectedCells[0].ColumnIndex;
            if (2==colIdx || 3==colIdx)
            {
                ComboBox c = e.Control as ComboBox;
                if (c != null) c.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["物资验收记录ID"].Visible = false;
        }

        void tsi_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            DataTable dt;
            if (this.Name == sender.ToString())
            {
                return;
            }
            int id;
            if (this.Name == "物资验收记录")
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            }
            else
            {
                id = Convert.ToInt32(dtOuter.Rows[0]["物资验收记录ID"]);
            }
            try
            {
                switch (sender.ToString())
                {
                    case "物资验收记录":
                        物资验收记录 form1 = new 物资验收记录(mainform, id);
                        form1.Show();
                        break;
                    case "物资请验单":
                        da = new OleDbDataAdapter("select * from 物资请验单 where 物资验收记录ID="+id,conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        物资请验单 form2 = new 物资请验单(Convert.ToInt32(dt.Rows[0]["ID"]));
                        form2.Show();
                        break;
                    case "检验记录":
                        da = new OleDbDataAdapter("select * from 检验记录 where 物资验收记录ID=" + id, conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0) MessageBox.Show("没有关联的检验记录");
                        foreach (DataRow dr in dt.Rows)
                        {
                            (new 检验记录(Convert.ToInt32(dr["ID"]))).Show();                            //form3.Show();
                        }
                        break;
                    case "取样记录":
                        da = new OleDbDataAdapter("select * from 取样记录 where 物资验收记录ID=" + id, conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        取样记录 form4 = new 取样记录(Convert.ToInt32(dt.Rows[0]["ID"]));
                        form4.Show();
                        break;
                }
            }
            catch
            {
                MessageBox.Show("关联失败，请检查是否有相应数据");
            }
            //MessageBox.Show(this.Name + "\n" + sender.ToString());
        }

        void setControlTrue()
        {
            // textbox,datagridview
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
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            isSaved = true;
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            outerBind();


            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;



            da = new OleDbDataAdapter("select * from 待审核 where 表名='物资验收记录' and 对应ID=" + dtOuter.Rows[0]["ID"], conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "物资验收记录";
            dr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);
            da.Update(dt);


            dtOuter.Rows[0]["审核员"] = "__待审核";
            _formState = Parameter.FormState.待审核;
            btn提交审核.Enabled = false;
            daOuter.Update((DataTable)bsOuter.DataSource);
            btn保存.PerformClick();
            setFormState();
            setEnableReadOnly();

            string log = "=============================\n";
            log += DateTime.Now.ToShortDateString();
            log += " 验收人：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            setControlFalse();

            // TODO 判断，然后决定是新建 请验单 还是  检验记录
            // 还有自动增加若干条检验台账
            
            //bool isAllOK = true;
            //List<Int32> RowToCheck = new List<int>();
            //for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            //{
            //    if (dataGridView1.Rows[i].Cells["是否需要检验"].Value.ToString() == "是")
            //    {
            //        isAllOK = false;
            //        RowToCheck.Add(i);
            //    }
            //}
            //// 开请验单，取样记录，检验台账
            //if (isAllOK)
            //{
            //    create请验单();
            //    // 
            //    create取样记录();
            //    insert检验台账();
            //    // TODO 加入库存台账
            //    insert库存台帐();
            //}
            //// 开检验记录
            //else
            //{
            //    foreach (int r in RowToCheck)
            //    {
            //        da = new OleDbDataAdapter("select * from 检验记录 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"] + " and 物料名称='" + dtInner.Rows[r]["物料名称"] + "'", conn);
            //        dt = new DataTable("检验记录");
            //        cb = new OleDbCommandBuilder(da);
            //        BindingSource bs = new BindingSource();
            //        da.Fill(dt);
            //        dr = dt.NewRow();
            //        dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            //        dr["物料名称"] = dtInner.Rows[r]["物料名称"];
            //        dr["产品批号"] = dtInner.Rows[r]["本厂批号"];
            //        dr["数量"] = dtInner.Rows[r]["数量"];
            //        dr["检验日期"] = DateTime.Now;
            //        dr["审核日期"] = DateTime.Now;
            //        dr["物料代码"] = dtInner.Rows[r]["物料代码"];
            //        dr["检验结论"] = "合格";
            //        dt.Rows.Add(dr);
            //        da.Update(dt);
            //    }
            //    MessageBox.Show("已自动生产" + RowToCheck.Count + "张检验记录");
            //}

          
            
        }

        public void create请验单()
        {
            if (dtOuter.Rows[0]["审核员"].ToString() == "")
            {
                return;
            }
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);
            DataTable dt = new DataTable("物资请验单");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow dr = dt.NewRow();
            dr["供应商代码"] = dtOuter.Rows[0]["供应商代码"];
            dr["供应商名称"] = dtOuter.Rows[0]["供应商名称"];
            dr["请验时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["请验人"] = mySystem.Parameter.userName;
            dr["请验编号"] = create请验编号();
            dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);

            da.Update(dt);
            da = new OleDbDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);

            dt = new DataTable("物资请验单");
            da.Fill(dt);
            // Inner
            da = new OleDbDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + dt.Rows[0]["ID"], conn);
            DataTable dtMore = new DataTable("物资请验单详细信息");
            cb = new OleDbCommandBuilder(da);
            da.Fill(dtMore);
            for (int i = 0; i < dtInner.Rows.Count; ++i)
            {
                if (dtInner.Rows[i]["是否需要检验"].ToString() == "是") continue;
                DataRow ndr = dtMore.NewRow();
                // 注意ID的值
                for (int j = 2; j < dtInner.Rows[i].ItemArray.Length - 1; ++j)
                {
                    ndr[j] = dtInner.Rows[i][j];
                }
                ndr[1] = dt.Rows[0]["ID"];
                ndr[ndr.ItemArray.Length - 1] = dtOuter.Rows[0]["厂家随附检验报告"].ToString() ;
                dtMore.Rows.Add(ndr);
            }
            da.Update(dtMore);
            MessageBox.Show("已自动生产物资请验单！");
        }

        public void create取样记录()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 取样记录 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);
            DataTable dt = new DataTable("取样记录");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow dr = dt.NewRow();

            dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            dr["审核时间"] = DateTime.Now;
            dr["供应商代码"] = dtOuter.Rows[0]["供应商代码"].ToString();
            dr["供应商名称"] = dtOuter.Rows[0]["供应商名称"].ToString();
            dt.Rows.Add(dr);

            da.Update(dt);
            da = new OleDbDataAdapter("select * from 取样记录 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);

            dt = new DataTable("取样记录");
            da.Fill(dt);
            // Inner
            da = new OleDbDataAdapter("select * from 取样记录详细信息 where 取样记录ID=" + dt.Rows[0]["ID"], conn);
            DataTable dtMore = new DataTable("取样记录详细信息");
            cb = new OleDbCommandBuilder(da);
            da.Fill(dtMore);
            for (int i = 0; i < dtInner.Rows.Count; ++i)
            {
                if (dtInner.Rows[i][9].ToString() == "是") continue;
                DataRow ndr = dtMore.NewRow();
                // 注意ID的值
                for (int j = 2; j <= 8; ++j)
                {
                    
                    ndr[j] = dtInner.Rows[i][j];
                }
                ndr[1] = dt.Rows[0]["ID"];

                ndr[9] = DateTime.Now;
                ndr["取样目的"] = "入场检验";
                ndr[12] = mySystem.Parameter.userName;
                dtMore.Rows.Add(ndr);
            }
            da.Update(dtMore);
            MessageBox.Show("已自动生产取样记录！");
        }

        private string create请验编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 请验编号 from 物资请验单 order by ID DESC", conn);
            DataTable dt = new DataTable("物资请验单");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
            string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
            if (Int32.Parse(yearInCode) == yearNow)
            {
                codeNow = Int32.Parse(NOInCode) + 1;
            }
            else
            {
                codeNow = 1;
            }
            return yearNow.ToString() + codeNow.ToString("D4");

        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.Show();
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefaultValue(dr);
            dtInner.Rows.Add(dr);
        }



        public override void CheckResult()
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;
            DataRow dr;
            da = new OleDbDataAdapter("select * from 待审核 where 表名='物资验收记录' and 对应ID=" + dtOuter.Rows[0]["ID"], conn);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;

            if (ckform.ischeckOk)
            {
                bool isAllOK = true;
                List<Int32> RowToCheck = new List<int>();
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    if (dataGridView1.Rows[i].Cells["是否需要检验"].Value.ToString() == "是")
                    {
                        isAllOK = false;
                        RowToCheck.Add(i);
                    }
                }
                // 开请验单，取样记录，检验台账
                if (isAllOK)
                {
                    create请验单();
                    // 
                    create取样记录();
                    insert检验台账();
                    // TODO 加入库存台账
                    insert库存台帐();
                }
                // 开检验记录
                else
                {
                    foreach (int r in RowToCheck)
                    {
                        da = new OleDbDataAdapter("select * from 检验记录 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"] + " and 物料名称='" + dtInner.Rows[r]["物料名称"] + "'", conn);
                        dt = new DataTable("检验记录");
                        cb = new OleDbCommandBuilder(da);
                        BindingSource bs = new BindingSource();
                        da.Fill(dt);
                        dr = dt.NewRow();
                        dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
                        dr["物料名称"] = dtInner.Rows[r]["物料名称"];
                        dr["产品批号"] = dtInner.Rows[r]["本厂批号"];
                        dr["数量"] = dtInner.Rows[r]["数量"];
                        dr["检验日期"] = DateTime.Now;
                        dr["审核日期"] = DateTime.Now;
                        dr["物料代码"] = dtInner.Rows[r]["物料代码"];
                        dr["检验结论"] = "合格";
                        dt.Rows.Add(dr);
                        da.Update(dt);
                    }
                    MessageBox.Show("已自动生产" + RowToCheck.Count + "张检验记录");
                }

          
            }



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
            base.CheckResult();
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

                if (dc.ColumnName == "是否需要检验")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("是");
                    cbc.Items.Add("否");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "物料名称")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (string s in ls物料名称)
                    {
                        cbc.Items.Add(s);
                    }
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "物料代码")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    foreach (string s in ls物料代码)
                    {
                        cbc.Items.Add(s);
                    }
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


        public void insert检验台账()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 检验台账 where 0=1" + dtOuter.Rows[0]["ID"], conn);
            DataTable dt = new DataTable("检验台账");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            foreach (DataRow dr in dtInner.Rows)
            {
                if (dr["是否需要检验"].ToString() == "是") continue;
                DataRow ndr =  dt.NewRow();
                for (int j= 2; j <= 8; ++j)
                {
                    ndr[j] = dr[j];
                    
                }
                ndr[1] = DateTime.Now;
                ndr[9] = dtOuter.Rows[0]["供应商代码"];
                ndr[10] = dtOuter.Rows[0]["供应商名称"];
                ndr[11] = "有";
                ndr[13] = mySystem.Parameter.userName;
                ndr[14] = "No";
                ndr[16] = DateTime.Now;
                ndr[19] = "是";
                ndr["物资验收记录详细信息ID"] = dr["ID"]; 
                dt.Rows.Add(ndr);
            }
            

            da.Update(dt);
            
            MessageBox.Show("已加入检验台账！");
        }

        public void insert库存台帐()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 库存台帐 where 0=1" + dtOuter.Rows[0]["ID"], conn);
            DataTable dt = new DataTable("库存台帐");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            foreach (DataRow dr in dtInner.Rows)
            {
                if (dr["是否需要检验"].ToString() == "是") continue;
                DataRow ndr = dt.NewRow();
                //for (int j = 2; j <= 8; ++j)
                //{
                //    ndr[j] = dr[j];

                //}
                //ndr[1] = DateTime.Now;
                //ndr[9] = dtOuter.Rows[0]["供应商代码"];
                //ndr[10] = dtOuter.Rows[0]["供应商名称"];
                //ndr[11] = "有";
                //ndr[13] = mySystem.Parameter.userName;
                //ndr[14] = "Yes";
                //ndr[16] = DateTime.Now;
                //ndr[19] = "是";
                ndr["仓库名称"] = "SPM仓库";
                ndr["供应商代码"] = dtOuter.Rows[0]["供应商代码"];
                ndr["供应商名称"] = dtOuter.Rows[0]["供应商名称"];
                ndr["产品名称"] = dr["物料名称"];
                ndr["产品规格"] = dr["包装规格"];
                ndr["产品批号"] = dr["本厂批号"];
                ndr["现存件数"] = 0;
                ndr["现存数量"] = dr["数量"];
                ndr["主计量单位"] = dr["单位"];
                ndr["实盘数量"] = 0;
                ndr["用途"] = dr["用途"];
                ndr["状态"] = "待验";
                ndr["借用日志"] = "";
                ndr["冻结状态"] = true;
                ndr["物资验收记录详细信息ID"] = dr["ID"]; 
                dt.Rows.Add(ndr);
            }


            da.Update(dt);

            MessageBox.Show("已加入库存台账！");
        }


        void getInnerOtherData()
        {
            OleDbDataAdapter da;
            DataTable dt;
            ls物料代码 = new List<string>();
            ls物料名称 = new List<string>();
            da = new OleDbDataAdapter("select * from 设置存货档案",conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls物料代码.Add(dr["存货代码"].ToString());
                ls物料名称.Add(dr["存货名称"].ToString());
            }

           

        }

        private void 物资验收记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (dtOuter != null && dtOuter.Rows.Count > 0)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update(dtOuter);


                    foreach (DataRow dr in dtInner.Rows)
                    {
                        dr.Delete();
                    }
                    daInner.Update(dtInner);
                }
            }
        }

    }
}
