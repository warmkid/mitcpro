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

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPEBag_batchproduction : BaseForm
    {
        List<string> record = null;
        List<Int32> noid = null;
        int totalPage = 4;
        int _生产指令ID;
        string _生产指令;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;


        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOuter;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;

        CheckForm ckform;

        public LDPEBag_batchproduction(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView2.DataError += dataGridView2_DataError;
            _生产指令ID = mySystem.Parameter.ldpebagInstruID;
            _生产指令 = mySystem.Parameter.ldpebagInstruction;
            tb生产指令编号.Text = _生产指令;
            init();

            getPeople();
            setUseState();
            readOuterData(_生产指令ID);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_生产指令ID);
                outerBind();
            }
            setFormState();
            setEnableReadOnly();

        }


        public LDPEBag_batchproduction(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView2.DataError += dataGridView2_DataError;
            _生产指令ID = id;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            _生产指令 = dt.Rows[0]["生产指令编号"].ToString();
            tb生产指令编号.Text = _生产指令;
            init();

            getPeople();
            setUseState();
            readOuterData(_生产指令ID);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_生产指令ID);
                outerBind();
            }
            setFormState();
            setEnableReadOnly();

        }


        private void init()
        {
            record = new List<string>();
            //record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面.xlsx");
            record.Add("SOP-MFG-303-R01A 1#制袋工序生产指令.xlsx");
            record.Add("SOP-MFG-303-R02A 1# 制袋机开机前确认表.xlsx");
            record.Add("SOP-MFG-102-R01A 生产领料使用记录.xlsx");
            record.Add("SOP-MFG-303-R03A 1# 制袋机运行记录.xlsx");
            record.Add("SOP-MFG-109-R01A 产品内包装记录.xlsx");
            record.Add("SOP-MFG-110-R01A 清场记录.xlsx");
            record.Add("SOP-MFG-303-R06A LDPE制袋日报表.xlsx");
            record.Add("QB-PA-PP-03-R01A 产品外观和尺寸检验记录");
            record.Add("QB-PA-PP-03-R02A 产品热合强度检验记录");

            initrecord();
            initly();
        }

        private void initly()
        {
            // 读取各表格数据，并显示页数

            OleDbDataAdapter da;
            DataTable dt;
            noid = new List<int>();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells[0].Value = false;
            }

            dataGridView1.Columns[totalPage].ReadOnly = true;
           
            //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            // 生产指令
            int temp;
            int idx = 0;
            int tempid;
            da = new OleDbDataAdapter("select * from 生产指令 where  生产指令编号='" + _生产指令 + "'", mySystem.Parameter.connOle);
            dt = new DataTable("生产指令");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //开机前确认
            idx++;
            da = new OleDbDataAdapter("select * from 制袋机组开机前确认表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("制袋机组开机前确认表");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 生产领料使用记录
            idx++;
            da = new OleDbDataAdapter("select * from 生产领料使用记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("生产领料使用记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 制袋机运行记录
            idx++;
            da = new OleDbDataAdapter("select * from 制袋机组运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("制袋机组运行记录制袋机组运行记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            
            //  产品内包装记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品内包装记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("产品内包装记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 清场记录
            idx++;
            da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("清场记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //  CS制袋日报表
            idx++;
            da = new OleDbDataAdapter("select * from LDPE制袋日报表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("CS制袋日报表");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //  产品外观和尺寸检验记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("产品外观和尺寸检验记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 产品热合强度检验记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品热合强度检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("产品热合强度检验记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }


            // 另外一个datagridview
            // 读内包装
            da = new OleDbDataAdapter("select 产品代码,生产批号,产品数量只合计B as 生产数量 from 产品内包装记录 where 生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DataSource = dt;
            dataGridView2.ReadOnly = true;
        }
        private void initrecord()
        {
            for (int i = 0; i < record.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i + 1;
                dr.Cells[3].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }


        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='制袋工序批生产记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();
            //string[] s=Regex.Split("张三，,赵一,赵二", ",|，");

        }

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

        void readOuterData(int pid)
        {
            daOuter = new OleDbDataAdapter("select * from 批生产记录表 where 生产指令ID=" + pid, mySystem.Parameter.connOle);
            dtOuter = new DataTable("批生产记录表");
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {

            bsOuter.DataSource = dtOuter;
            // clear
            tb生产指令编号.DataBindings.Clear();
            tb汇总人.DataBindings.Clear();
            tb批准人.DataBindings.Clear();
            tb审核人.DataBindings.Clear();
            dtp汇总时间.DataBindings.Clear();
            dtp结束生产时间.DataBindings.Clear();
            dtp开始生产时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            // bind
            tb生产指令编号.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            tb汇总人.DataBindings.Add("Text", bsOuter.DataSource, "汇总员");
            tb批准人.DataBindings.Add("Text", bsOuter.DataSource, "批准员");
            tb审核人.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
            dtp汇总时间.DataBindings.Add("Value", bsOuter.DataSource, "汇总时间");
            dtp结束生产时间.DataBindings.Add("Value", bsOuter.DataSource, "结束生产时间");
            dtp开始生产时间.DataBindings.Add("Value", bsOuter.DataSource, "开始生产时间");
            dtp批准时间.DataBindings.Add("Value", bsOuter.DataSource, "批准时间");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");
        }

        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = _生产指令ID;
            dr["生产指令编号"] = _生产指令;
            dr["开始生产时间"] = DateTime.Now;
            dr["结束生产时间"] = DateTime.Now;
            dr["汇总员"] = mySystem.Parameter.userName;
            dr["汇总时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            return dr;
        }

        void setFormState()
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


        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            //// 查询插入，审核，提交审核，生产指令编码 在这里不用管
            //foreach (Control c in this.Controls)
            //{
            //    if (c is TextBox)
            //    {
            //        (c as TextBox).ReadOnly = false;
            //    }
            //    else if (c is DataGridView)
            //    {
            //        (c as DataGridView).ReadOnly = false;
            //    }
            //    else
            //    {
            //        c.Enabled = true;
            //    }
            //}
            //// 保证这两个按钮一直是false
            //btn审核.Enabled = false;
            //btn提交审核.Enabled = false;
            tb生产指令编号.Enabled = true;
            tb生产指令编号.ReadOnly = true;
            tb汇总人.Enabled = true;
            tb批准人.Enabled = true;
            tb审核人.Enabled = true;

            dtp批准时间.Enabled = true;
            dtp审核时间.Enabled = true;
            dtp结束生产时间.Enabled = true;
            dtp开始生产时间.Enabled = true;
            dtp汇总时间.Enabled = true;

            btn保存.Enabled = true;
            btn打印.Enabled = true;
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            comboBox打印机选择.Enabled = true;
        }

        /// <summary>
        /// 默认控件不可用状态
        /// </summary>
        void setControlFalse()
        {
            //// 查询插入，审核，提交审核，生产指令编码 在这里不用管
            //foreach (Control c in this.Controls)
            //{
            //    if (c.Name == "btn查询插入") continue;
            //    if (c.Name == "tb生产指令编码") continue;
            //    if (c is TextBox)
            //    {
            //        (c as TextBox).ReadOnly = true;
            //    }
            //    else if (c is DataGridView)
            //    {
            //        (c as DataGridView).ReadOnly = true;
            //    }
            //    else
            //    {
            //        c.Enabled = false;
            //    }
            //}
            //btn查看日志.Enabled = true;
            //btn打印.Enabled = true;

            tb生产指令编号.Enabled = true;
            tb生产指令编号.ReadOnly = true;
            tb汇总人.Enabled = false;
            tb批准人.Enabled = false;
            tb审核人.Enabled = false;

            dtp批准时间.Enabled = false;
            dtp审核时间.Enabled = false;
            dtp结束生产时间.Enabled = false;
            dtp开始生产时间.Enabled = false;
            dtp汇总时间.Enabled = false;

            btn保存.Enabled = false;
            btn打印.Enabled = true;
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            comboBox打印机选择.Enabled = true;

        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员)
            {
                btn提交审核.Enabled = true;
            }
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_生产指令ID);
            outerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            

            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "批生产记录表";
            dr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = "__待审核";
            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (dtOuter.Rows[0]["汇总员"].ToString() == mySystem.Parameter.userName)
            {
                MessageBox.Show("操作员和审核员不能是同一个人！");
                return;
            }
            ckform = new CheckForm(this);
            ckform.Show();
        }

        public override void CheckResult()
        {
            OleDbDataAdapter da;
            OleDbCommandBuilder cb;
            DataTable dt;

            da = new OleDbDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核员"] = ckform.userName;
            dtOuter.Rows[0]["审核是否通过"] = ckform.ischeckOk;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;


            save();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
            base.CheckResult();
        }

        private void fillPrinter()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                comboBox打印机选择.Items.Add(sPrint);
            }
            comboBox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        private void btn打印_Click(object sender, EventArgs e)
        {

        }

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {            
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }
        
    }
}
