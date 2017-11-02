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

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVBag_batchproduction :mySystem.BaseForm
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

        public PTVBag_batchproduction(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            _生产指令ID = mySystem.Parameter.ptvbagInstruID;
            _生产指令 = mySystem.Parameter.ptvbagInstruction;
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


        public PTVBag_batchproduction(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            fillPrinter();
            
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 批生产记录表 where ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            _生产指令 = dt.Rows[0]["生产指令编号"].ToString();
            _生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
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
            record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面");
            record.Add("SOP-MFG-303-R01A 2#制袋工序生产指令");
            record.Add("SOP-MFG-303-R02A  2# 制袋机开机前确认表");
            record.Add("SOP-MFG-102-R01A  生产领料记录");
            record.Add("SOP-MFG-102-R02A  生产退料记录");

            record.Add("SOP-MFG-109-R01A  产品内包装记录");
            record.Add("SOP-MFG-111-R01A  产品外包装记录");

            record.Add("SOP-MFG-418-R01A 瓶口焊接机运行记录");
            record.Add("SOP-MFG-412-R01A 底封机运行记录");
            record.Add("SOP-MFG-413-R01A 圆口焊接机运行记录");
            record.Add("SOP-MFG-415-R01A 泄漏测试记录");
            record.Add("SOP-MFG-416-R01A 超声波焊接记录");


            record.Add("SOP-MFG-110-R01A 清场记录");
            record.Add("SOP-MFG-305-R04A PTV生产台帐");  //默认页数为1

            record.Add("SOP-MFG-108-R01A 制袋岗位交接班记录");
            record.Add("PTV 内标签");
            record.Add("PTV 外标签");
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
                dgvr.Cells[0].Value = false;//未勾选
            }

            dataGridView1.Columns[totalPage].ReadOnly = true;
           
            //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            //封面
            int temp;
            int idx = 0;
            dataGridView1.Rows[idx].Cells[totalPage].Value = 1;//封面页数默认为1

            // 生产指令
            idx++;
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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //生产退料使用记录
            idx++;
            da = new OleDbDataAdapter("select * from 生产退料记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("生产退料记录表");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //  产品外包装记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("产品外包装记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //  瓶口焊接机运行记录
            idx++;
            da = new OleDbDataAdapter("select * from 瓶口焊接机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("瓶口焊接机运行记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //底封机运行记录
            idx++;
            da = new OleDbDataAdapter("select * from 底封机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("底封机运行记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //  圆口焊接机运行记录
            idx++;
            da = new OleDbDataAdapter("select * from 圆口焊接机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("圆口焊接机运行记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //泄漏测试记录
            idx++;
            da = new OleDbDataAdapter("select * from 泄漏测试记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("泄漏测试记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }


            //超声波焊接记录
            idx++;
            da = new OleDbDataAdapter("select * from 超声波焊接记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("超声波焊接记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //  PTV生产台帐
            idx++;
            dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
            disableRow(idx);//默认不能打印

            //  制袋岗位交接班记录
            idx++;
            da = new OleDbDataAdapter("select * from 岗位交接班记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable("岗位交接班记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //PTV 内标签
            idx++;
            dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
            disableRow(idx);//先不打印

            //PTV 外标签
            idx++;
            dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
            disableRow(idx);//先不打印

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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
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
                //for (int i = 0; i < temp; ++i)
                //{
                //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                //}
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }


            // 另外一个datagridview
            // 读内包装
            da = new OleDbDataAdapter("select 产品代码,生产批号,产品数量只数合计B as 生产数量 from 产品内包装记录 where 生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            dt = new DataTable();           
            da.Fill(dt);

            if (dt.Rows.Count > 1)
            {
                int sum = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    sum += Convert.ToInt32(dt.Rows[i]["生产数量"].ToString());
                dt.Rows[0]["生产数量"] = sum;
                while (1 != dt.Rows.Count)
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);                                     
            }

            dataGridView2.AllowUserToAddRows = false;
            int index = this.dataGridView2.Rows.Add();
            this.dataGridView2.Rows[index].Cells[0].Value = dt.Rows[0]["产品代码"].ToString();//产品代码
            this.dataGridView2.Rows[index].Cells[1].Value = dt.Rows[0]["生产批号"].ToString();
            this.dataGridView2.Rows[index].Cells[2].Value = dt.Rows[0]["生产数量"].ToString(); //生产数量
            //dataGridView2.DataSource = dt;
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
                dr.Cells[2].Value = i + 1;//序号
                dr.Cells[3].Value = record[i];//记录
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

        void getOtherData(out DateTime dtime,int id)
        {
            //获得开始生产时间
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 生产指令 where  ID=" + id, mySystem.Parameter.connOle);
            dt = new DataTable("生产指令");
            da.Fill(dt);
            if (dt.Rows.Count > 0)
                dtime = DateTime.Parse(dt.Rows[0]["计划生产日期"].ToString());
            else
                dtime = DateTime.Now;
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
            DateTime dtime;
            getOtherData(out dtime, _生产指令ID);
            dr["开始生产时间"] = dtime;
            dr["结束生产时间"] = DateTime.Now;
            dr["汇总人"] = mySystem.Parameter.userName;
            dr["汇总时间"] = DateTime.Now;
            dr["审核人"] = "";
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            dr["批准人"] = "";
            dr["备注"] = "";
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

            dtOuter.Rows[0]["审核人"] = "__待审核";
            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {

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

            dtOuter.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["批准人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核时间"] = DateTime.Now;
            dtOuter.Rows[0]["批准时间"] = DateTime.Now;

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

            foreach (Int32 r in checkedRows)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + _生产指令ID, mySystem.Parameter.connOle);
                DataTable dt = new DataTable("生产指令");
                int id;
                List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                switch (r)
                {
                    case 0:// 制袋工序批生产记录封面
                        perform打印本页();
                        break;

                    case 1: // 生产指令
                        da = new OleDbDataAdapter("select * from 生产指令 where  ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("生产指令");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_productioninstruction(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 2: // 开机确认
                        da = new OleDbDataAdapter("select * from 制袋机组开机前确认表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("制袋机组开机前确认表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_checklist(mainform, id)).print(false);
                            GC.Collect();
                        }

                        break;
                    case 3: // 制袋领料记录
                        da = new OleDbDataAdapter("select * from 生产领料使用记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("制袋领料记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_materialrecord(mainform, id)).print(false);
                            GC.Collect();
                        }

                        break;
                    case 4://生产退料记录
                        da = new OleDbDataAdapter("select * from 生产退料记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("生产退料记录表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.RunningRecord(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 5:// 内包                                    
                        da = new OleDbDataAdapter("select * from 产品内包装记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品内包装记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            // TODO
                            (new mySystem.Process.Bag.PTV.PTVBag_innerpackaging(mainform, id)).print(false);
                            GC.Collect();

                        }
                        break;

                    case 6://外包
                        da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品外包装记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTV产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 7://瓶口焊接机运行记录
                        da = new OleDbDataAdapter("select * from 瓶口焊接机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("瓶口焊接机运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTV产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 8://底封机运行记录
                        da = new OleDbDataAdapter("select * from 底封机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("底封机运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTV产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 9://圆口焊接机运行记录
                        da = new OleDbDataAdapter("select * from 圆口焊接机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("圆口焊接机运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTV产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 10://圆口焊接机运行记录
                        da = new OleDbDataAdapter("select * from 圆口焊接机运行记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("圆口焊接机运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTV产品外包装记录(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 11://泄漏测试记录
                        da = new OleDbDataAdapter("select * from 泄漏测试记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("泄漏测试记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_clearance(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 12://超声波焊接记录
                        da = new OleDbDataAdapter("select * from 超声波焊接记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("超声波焊接记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_clearance(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 13://清场记录
                        da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("清场记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.PTVBag_clearance(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 14:// PTV生产台帐
                        break;

                    case 15:// 制袋岗位交接班记录
                        da = new OleDbDataAdapter("select * from 岗位交接班记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("岗位交接班记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.HandOver(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 16:// 内标签
                        break;

                    case 17://外标签
                        break;

                    case 18:// 产品外观和尺寸检验记录
                        da = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品外观和尺寸检验记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.HandOver(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 19:// 产品热合强度检验记录
                        da = new OleDbDataAdapter("select * from 产品热合强度检验记录 where  生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品热合强度检验记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Bag.PTV.HandOver(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                }
            }
        }

        public void perform打印本页()
        {
            if (comboBox打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(comboBox打印机选择.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
        }
        private void btn打印本页_Click(object sender, EventArgs e)
        {
            if (comboBox打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(comboBox打印机选择.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
        }

        public void print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\PTV\0 SOP-MFG-105-R01A 制袋工序批生产记录封面-PTV.xlsx");

            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my, wb);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = _生产指令 + "-" + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码


            if (b)
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
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        string log = "=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
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
        }

        //打印填数据
        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            int ind = 0;
            if (dataGridView2.Rows.Count > 12)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView2.Rows.Count - 12; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[6, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    Microsoft.Office.Interop.Excel.Range range1 = mysheet.get_Range("F6");
                    range1.Merge(mysheet.get_Range("G6"));
                }
                ind = dataGridView1.Rows.Count - 12;
            }

            //外表信息
            mysheet.Cells[3, 8].Value = dtOuter.Rows[0]["生产指令编号"].ToString();
            mysheet.Cells[4, 8].Value = dtp开始生产时间.Value.ToShortDateString() + "--" + dtp结束生产时间.Value.ToShortDateString();

            //内表2信息,生产记录
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                mysheet.Cells[6 + i, 6] = dataGridView2.Rows[i].Cells["产品代码"].Value.ToString();
                mysheet.Cells[6 + i, 8] = dataGridView2.Rows[i].Cells["生产数量"].Value.ToString();
                mysheet.Cells[6 + i, 9] = dataGridView2.Rows[i].Cells["生产批号"].Value.ToString();

            }

            //内表1，目录
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string s;
                if (dataGridView1.Rows[i].Cells[4].Value != null)
                    s = dataGridView1.Rows[i].Cells[4].Value.ToString();
                else
                    s = "0";
                mysheet.Cells[5 + i, 1] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                mysheet.Cells[5 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                mysheet.Cells[5 + i, 3] = s;

            }

            //外表，汇总，审核，批准
            mysheet.Cells[18 + ind, 7] = dtOuter.Rows[0]["汇总人"].ToString();
            mysheet.Cells[18 + ind, 9] = DateTime.Parse(dtOuter.Rows[0]["汇总时间"].ToString()).ToString("D") ;

            mysheet.Cells[20 + ind, 7] = dtOuter.Rows[0]["审核人"].ToString();
            mysheet.Cells[20 + ind, 9] = DateTime.Parse(dtOuter.Rows[0]["审核时间"].ToString()).ToString("D");

            mysheet.Cells[22 + ind, 7] = dtOuter.Rows[0]["批准人"].ToString();
            mysheet.Cells[22 + ind, 9] = DateTime.Parse(dtOuter.Rows[0]["批准时间"].ToString()).ToString("D");
        }

    }
}


