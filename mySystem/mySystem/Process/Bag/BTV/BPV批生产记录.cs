﻿using System;
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

namespace mySystem.Process.Bag.BTV
{
    public partial class BPV批生产记录 :mySystem.BaseForm
    {
        List<string> record = null;
        List<Int32> noid = null;
        delegate void printF (int id);
        printF[] prtBatch = null;
        int totalPage = 4;
        int _生产指令ID;
        string _生产指令;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;


        SqlDataAdapter daOuter;
        SqlCommandBuilder cbOuter;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;
        List<String> tableName;
        CheckForm ckform;
        DataTable dtSource = null;
        Boolean isFirstBind = true;
        public BPV批生产记录(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            _生产指令ID = mySystem.Parameter.bpvbagInstruID;
            _生产指令 = mySystem.Parameter.bpvbagInstruction;
            tb生产指令编号.Text = _生产指令;
            dtSource = (DataTable)dataGridView1.DataSource;
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


        public BPV批生产记录(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            
            SqlDataAdapter da = new SqlDataAdapter("select * from 批生产记录表 where ID=" + id, mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            _生产指令 = dt.Rows[0]["生产指令编号"].ToString();
            _生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            tb生产指令编号.Text = _生产指令;
            dtSource = (DataTable)dataGridView1.DataSource;
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
            record = new List<string>(); tableName = new List<string>(); prtBatch = new printF[26];
            //record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面.xlsx");
            //record.Add("SOP-MFG-303-R01A 2#生产指令.xlsx");
            record.Add("2 SOP-MFG-306-R02A  BPV生产前确认记录.xlsx"); tableName.Add("BPV生产前确认记录"); prtBatch[2] = new printF(PF2);
            record.Add("3 SOP-MFG-102-R01A 生产领料记录.xlsx"); tableName.Add("生产领料使用记录"); prtBatch[3] = new printF(PF3);       
            record.Add("4 SOP-MFG-102-R02A 生产退料记录.xlsx"); tableName.Add("生产退料记录表"); prtBatch[4] = new printF(PF4);         
            record.Add("5 SOP-MFG-109-R01A 产品内包装记录.xlsx"); tableName.Add("产品内包装记录"); prtBatch[5] = new printF(PF5);       
            record.Add("6 SOP-MFG-111-R01A 产品外包装记录.xlsx"); tableName.Add("产品外包装记录表"); prtBatch[6] = new printF(PF6);     
            record.Add("7 SOP-MFG-418-R01A 瓶口焊接机运行记录.xlsx"); tableName.Add("瓶口焊接机运行记录"); prtBatch[7] = new printF(PF7);
            record.Add("8 SOP-MFG-412-R01A 底封机运行记录.xlsx"); tableName.Add("底封机运行记录"); prtBatch[8] = new printF(PF8);        
            record.Add("9 SOP-MFG-417-R01A  2D袋体与船型接口热合记录.xlsx"); tableName.Add("2DBag袋体与船型接口热合记录"); prtBatch[9] = new printF(PF9); 
            record.Add("10 SOP-MFG-503-R01A  单管口热合机运行记录.xlsx"); tableName.Add("单管口热合机运行记录"); prtBatch[10] = new printF(PF10);    
            record.Add("11 SOP-MFG-501-R01A  多功能热合机运行记录.xlsx"); tableName.Add("多功能热合机运行记录"); prtBatch[11] = new printF(PF11);    
            record.Add("12 SOP-MFG-505-R01A  90度热合机运行记录.xlsx"); tableName.Add("90度热合机运行记录"); prtBatch[12] = new printF(PF12);        
            record.Add("13 SOP-MFG-506-R01A  封口热合机运行记录.xlsx"); tableName.Add("封口热合机运行记录"); prtBatch[13] = new printF(PF13);
            record.Add("14 SOP-MFG-502-R01A  BPV袋体生产记录.xlsx"); tableName.Add("2D袋体生产记录"); prtBatch[14] = new printF(PF14);       
            record.Add("15 SOP-MFG-508-R01A  打孔及与图纸确认记录.xlsx"); tableName.Add("打孔及与图纸确认记录"); prtBatch[15] = new printF(PF15); 
            record.Add("16 SOP-MFG-306-R04A  BPV切管记录.xlsx"); tableName.Add("BPV切管记录"); prtBatch[16] = new printF(PF16);          
            record.Add("17 SOP-MFG-306-R05A  BPV装配确认记录.xlsx"); tableName.Add("BPV装配确认记录"); prtBatch[17] = new printF(PF17);  
            record.Add("18 SOP-MFG-306-R08A  关键尺寸确认记录.xlsx"); tableName.Add("BPV关键尺寸确认记录"); prtBatch[18] = new printF(PF18);
            record.Add("19 BPV-内标签.xlsx"); tableName.Add("null");
            record.Add("20 BPV-外标签.xlsx"); tableName.Add("null");
            record.Add("21 SOP-MFG-306-R10A  BPV生产日报表.xlsx"); tableName.Add("null");
            record.Add("22 QB-PA-PP-03-R02A 产品热合强度检验记录.xlsx"); tableName.Add("产品热合强度检验记录"); prtBatch[22] = new printF(PF22);
            //record.Add("SOP-MFG-306-R09A  原材料分装记录.xlsx"); tableName.Add("原材料分装记录");         
            
            
            
            
            

            initrecord();
            initly();
        }

        private void initly()
        {
            // 读取各表格数据，并显示页数

            SqlDataAdapter da;
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

            {
                ///this block, add 批生产记录表 in the list
                da = new SqlDataAdapter("select * from 批生产记录表 where  生产指令ID=" + _生产指令ID + "", mySystem.Parameter.conn);
                dt = new DataTable("批生产记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    for (int j = 0; j < temp; ++j)
                    {
                        dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                idx++;
            }
            {
                ///this block, add 生产指令 in the list
                da = new SqlDataAdapter("select * from 生产指令 where  ID=" + _生产指令ID + "", mySystem.Parameter.conn);
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
                    for (int j = 0; j < temp; ++j)
                    {
                        dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                idx++;
            }

            for (int i = 0; i < tableName.Count; i++)
            {
                if ("null" == tableName[i])
                {
                    temp = 1;
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    for (int j = 0; j < temp; ++j)
                    {
                        dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    idx++;
                    continue;
                }
                da = new SqlDataAdapter("select * from [" + tableName[i] + "] where  生产指令ID=" + _生产指令ID + "", mySystem.Parameter.conn);
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
                    for (int j = 0; j < temp; ++j)
                    {
                        dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                idx++;
            }




            // 另外一个datagridview
            // 读内包装
            da = new SqlDataAdapter("select 产品代码,生产批号,产品数量只数合计B as 生产数量 from 产品内包装记录 where 生产指令ID=" + _生产指令ID, mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            dtSource = dt.Copy();
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DataSource = dt;
            dataGridView2.ReadOnly = true;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }
        private void initrecord()
        {
            // Add 批生产记录封面,制袋工序生产指令 alone
            DataGridViewRow drTemp = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                drTemp.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            drTemp.Cells[2].Value = 0;
            drTemp.Cells[3].Value = "0 SOP-MFG-105-R01A 制袋工序批生产记录封面BPV.xlsx";
            dataGridView1.Rows.Add(drTemp);
            
            drTemp = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                drTemp.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            drTemp.Cells[2].Value =  1;
            drTemp.Cells[3].Value = "1 SOP-MFG-306-R01A  BPV 制袋生产指令.xlsx";
            dataGridView1.Rows.Add(drTemp);

            for (int i = 0; i < record.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i + 2;
                dr.Cells[3].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
            ;
        }

        

        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }


        private void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 用户权限 where 步骤='制袋工序批生产记录'", mySystem.Parameter.conn);
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
            daOuter = new SqlDataAdapter("select * from 批生产记录表 where 生产指令ID=" + pid, mySystem.Parameter.conn);
            dtOuter = new DataTable("批生产记录表");
            cbOuter = new SqlCommandBuilder(daOuter);
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
            tb汇总人.DataBindings.Add("Text", bsOuter.DataSource, "汇总人");
            tb批准人.DataBindings.Add("Text", bsOuter.DataSource, "批准人");
            tb审核人.DataBindings.Add("Text", bsOuter.DataSource, "审核人");
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
            dr["汇总人"] = mySystem.Parameter.userName;
            dr["汇总时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            dr["审核是否通过"] = false;
            return dr;
        }

        void setFormState()
        {
           
            string s = dtOuter.Rows[0]["审核人"].ToString();
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
            

            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "批生产记录表";
            dr["对应ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = "__待审核";
            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            if (dtOuter.Rows[0]["汇总人"].ToString() == mySystem.Parameter.userName)
            {
                MessageBox.Show("操作员和审核员不能是同一个人！");
                return;
            }
            ckform = new CheckForm(this);
            ckform.Show();
        }

        public override void CheckResult()
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='批生产记录表' and 对应ID=" + dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = ckform.userName;
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


        private List<Int32> getIntList(String str)
        {
            try
            {
                List<Int32> ret = new List<int>();
                String[] strs = str.Split('-');
                if (1 == strs.Length)
                {
                    ret.Add(Convert.ToInt32(strs[0]));
                }
                else if (2 == strs.Length)
                {
                    int a = Convert.ToInt32(strs[0]);
                    int b = Convert.ToInt32(strs[1]);
                    if (a > b)
                    {
                        throw new Exception("后面数字比前面的大");
                    }
                    for (int i = a; i <= b; ++i)
                    {
                        ret.Add(i);
                    }
                }
                else
                {
                    throw new Exception("打印页码解析失败！");
                }
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<int>();
            }

        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            List<Int32> checkedRows = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                {
                    checkedRows.Add(i);
                }
            }
            checkedRows.Sort();
            if (0 == checkedRows[0])
            {
                checkedRows.RemoveAt(0);
                checkedRows.Add(0);
            }
            foreach (Int32 r in checkedRows)
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令 where ID=" + _生产指令ID, mySystem.Parameter.conn);
                DataTable dt = new DataTable("生产指令");
                int id;
                //List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                switch (r)
                {
                    case 0: // 制袋工序批生产记录封面
                        //PF0();
                        MessageBox.Show("fcvyhdusfhbydukia");
                        break;
                    case 1: // 生产指令
                        da = new SqlDataAdapter("select * from 生产指令 where  ID=" + _生产指令ID, mySystem.Parameter.conn);
                        dt = new DataTable("生产指令");
                        da.Fill(dt);                        
                        {
                            id = Convert.ToInt32(dt.Rows[0]["ID"]);
                            (new mySystem.Process.Bag.BTV.BPV制袋生产指令(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 19: // BPV-内标签
                        ;
                        break;
                    case 20:  // BPV-外标签
                        ;

                        break;
                    case 21: // BPV生产日报表
                        ;
                        break;
                    default:
                        try
                        {
                            List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                            da = new SqlDataAdapter("select * from " + tableName[r - 2] + " where  生产指令ID=" + _生产指令ID, mySystem.Parameter.conn);
                            dt = new DataTable("制袋机组运行记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                //(new mySystem.Process.Bag.RunningRecord(mainform, id)).print(false);
                                prtBatch[r](id);
                                GC.Collect();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("请确认打印范围!");
                        }
                        break;
                   
                }
            }
        }
        private void PF0()
        {
            //dataGridView1.Rows[idx-1].Cells[totalPage].Value
            var data = dataGridView1.DataSource;

            int[] accumu = new int[dataGridView1.Rows.Count];
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\0 SOP-MFG-105-R01A 制袋工序批生产记录封面BPV.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;

            int rowStartAt = 5;
            // 修改Sheet中某行某列的值
            my.Cells[3, 8].Value = dtOuter.Rows[0]["生产指令编号"];
            my.Cells[4, 8].Value = Convert.ToDateTime(dtOuter.Rows[0]["开始生产时间"]).ToString("yyyy年MM月dd日");
            my.Cells[22, 7].Value = dtOuter.Rows[0]["汇总人"];
            my.Cells[22, 9].Value = Convert.ToDateTime(dtOuter.Rows[0]["汇总时间"]).ToString("yyyy年MM月dd日");
            my.Cells[24, 7].Value = dtOuter.Rows[0]["审核人"];
            my.Cells[24, 9].Value = Convert.ToDateTime(dtOuter.Rows[0]["审核时间"]).ToString("yyyy年MM月dd日");
            my.Cells[26, 7].Value = dtOuter.Rows[0]["批准人"];
            my.Cells[26, 9].Value = Convert.ToDateTime(dtOuter.Rows[0]["批准时间"]).ToString("yyyy年MM月dd日");


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (0 == i)
                {
                    accumu[0]=1;
                    
                }
                else
                {
                    accumu[i] = Convert.ToInt32(dataGridView1.Rows[i].Cells[totalPage].Value) + accumu[i - 1];
                }
                my.Cells[i + rowStartAt, 3] = accumu[i];
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                my.Cells[i + 6, 6].Value = dtSource.Rows[i]["产品代码"];
                my.Cells[i + 6, 8].Value = dtSource.Rows[i]["生产数量"];
                my.Cells[i + 6, 9].Value = dtSource.Rows[i]["生产批号"];
                if (i > 22)
                {
                    MessageBox.Show("超出最大长度!");
                    break;
                }
            }
        }

        private void PF2(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVConfirmBefore(mainform, idParam).print(false);
        }
        private void PF3(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVMaterialRecord(mainform, idParam).print(false);
        }
        private void PF4(int idParam)
        {
            new mySystem.Process.Bag.BTV.BPV生产退料记录(mainform, idParam).print(false);
        }
        private void PF5(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVInnerPackage(mainform, idParam).print(false);
        }
        private void PF6(int idParam)
        {
            new mySystem.Process.Bag.BTV.BPV产品外包装记录(mainform, idParam).print(false);
        }
        private void PF7(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordPK(mainform, idParam).print(false);
        }
        private void PF8(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordDF(mainform, idParam).print(false);
        }
        private void PF9(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTV2DShipHeat(mainform, idParam).print(false);
        }
        private void PF10(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordRHJsingle(mainform, idParam).print(false);
        }
        private void PF11(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordRHJMulti(mainform, idParam).print(false);
        }
        private void PF12(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordRHJ90(mainform, idParam).print(false);
        }
        private void PF13(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVRunningRecordRHJseal(mainform, idParam).print(false);
        }
        private void PF14(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTV2DProRecord(mainform, idParam).print(false);
        }
        private void PF15(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVPunchDrawingConfirm(mainform, idParam).print(false);
        }
        private void PF16(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVCutPipeRecord(mainform, idParam).print(false);
        }
        private void PF17(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVAssemblyConfirm(mainform, idParam).print(false);
        }
        private void PF18(int idParam)
        {
            new mySystem.Process.Bag.BTV.BTVKeySizeConfirm(mainform, idParam).print(false);
        }
        private void PF22(int idParam)
        {
            new mySystem.Process.Bag.BTV.产品热合强度检验记录(mainform, idParam).print(false);
        }
    }
}
