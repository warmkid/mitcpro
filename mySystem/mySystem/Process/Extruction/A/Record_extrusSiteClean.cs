using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
//using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;



namespace mySystem.Extruction.Process
{
    /// <summary>
    /// 吹膜工序清场记录
    /// </summary>
    public partial class Record_extrusSiteClean : mySystem.BaseForm
    {
        bool checkout;//检查结果
        mySystem.CheckForm checkform;

        //OleDbConnection connOle;
        private DataTable dt_prodinstr, dt_prodlist,dt_prodlist2;//大表，供料工序清洁项目，吹膜工序清场项目
        private OleDbDataAdapter da_prodinstr, da_prodlist, da_prodlist2;
        private BindingSource bs_prodinstr, bs_prodlist, bs_prodlist2;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist, cb_prodlist2;


        DataTable dt_供料设置,dt_吹膜设置;
        string code, batch;
      
        private void readsetting()
        {
            //先读取供料工序清场设置，拷贝到上面table中
            string asql = "select 序号,清场内容 from 设置供料工序清场项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            da.Fill(dt_供料设置);

            //先读取吹膜工序清场设置，拷贝到上面table中
            asql = "select 序号,清场内容 from 设置吹膜工序清场项目";
            OleDbCommand comm2 = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da2 = new OleDbDataAdapter(comm2);
            da2.Fill(dt_吹膜设置);
        }
        //判断之前的内容是否与设置表中内容一致
        private bool is_serve_sameto_setting()
        {
            if (dt_prodlist.Rows.Count != dt_供料设置.Rows.Count)
                return false;
            for (int i = 0; i < dt_prodlist.Rows.Count; i++)
            {
                if (dt_prodlist.Rows[i]["清场要点"].ToString() != dt_供料设置.Rows[i]["清场内容"].ToString())
                    return false;
            }
            return true;
        }
        private bool is_extrusion_sameto_setting()
        {
            if (dt_prodlist2.Rows.Count != dt_吹膜设置.Rows.Count)
                return false;
            for (int i = 0; i < dt_prodlist2.Rows.Count; i++)
            {
                if (dt_prodlist2.Rows[i]["清场要点"].ToString() != dt_吹膜设置.Rows[i]["清场内容"].ToString())
                    return false;
            }
            return true;
        }

        private void Init()
        {

            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            //dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            dt_prodlist2 = new System.Data.DataTable();
            bs_prodlist2 = new System.Windows.Forms.BindingSource();
            da_prodlist2 = new OleDbDataAdapter();
            cb_prodlist2 = new OleDbCommandBuilder();

            dt_吹膜设置 = new DataTable();
            dt_供料设置 = new DataTable();

            tb生产指令.Text = mySystem.Parameter.proInstruction;

        }

        //查找清场前产品代码和批号
        private void query_prodandbatch()
        {
            string asql = "select 产品名称,产品批号 from 吹膜工序生产和检验记录 where 生产指令ID=" + mySystem.Parameter.proInstruID + " ORDER BY 生产日期";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                comm.Dispose();
                da.Dispose();
                tempdt.Dispose();
                return;
            }               
            else
            {
                code = (string)tempdt.Rows[0][0];
                batch = (string)tempdt.Rows[0][1];
            }

        }

        private void begin()
        {
            bt打印.Enabled = false;
            bt审核.Enabled = false;

            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(mySystem.Parameter.proInstruID);
                removeOuterBinding();
                outerBind();
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            dataGridView1.Columns[0].Visible = false;

            readInnerData2((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind2();
            dataGridView2.Columns[0].Visible = false;

            DialogResult result;
            if (!is_serve_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的供料清场记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                    int index = 1;
                    foreach (DataRow dr in dt_供料设置.Rows)
                    {
                        DataRow ndr = dt_prodlist.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr["序号"] = index++;
                        ndr["清场要点"] = dr["清场内容"];
                        ndr["清场操作"] = "合格";
                        dt_prodlist.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);
                }

            }

            if (!is_extrusion_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的吹膜清场记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView2.Rows.Count > 0)
                        dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
                    da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
                    int index = 1;
                    foreach (DataRow dr in dt_吹膜设置.Rows)
                    {
                        DataRow ndr = dt_prodlist2.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr["序号"] = index++;
                        ndr["清场要点"] = dr["清场内容"];
                        ndr["清场操作"] = "合格";
                        dt_prodlist2.Rows.Add(ndr);
                    }
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);
                }

            }

            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                bt打印.Enabled = true;
            }
        }

        public Record_extrusSiteClean(mySystem.MainForm mainform):base(mainform)
        {

            InitializeComponent();
            Init();
            readsetting();
            query_prodandbatch();
            begin();

        }

        public Record_extrusSiteClean(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            Init();
            readsetting();
            query_prodandbatch();

            string asql = "select * from 吹膜工序清场记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            int instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());

            readOuterData(instrid);
            removeOuterBinding();
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            dataGridView1.Columns[0].Visible = false;

            readInnerData2((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind2();
            dataGridView2.Columns[0].Visible = false;

            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bt审核.Enabled = false;
            //判断合法性
            if (!input_Judge())
            {
                return;
            }

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();
            da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
            readInnerData2(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind2();

            bt审核.Enabled = true;
        }

        private bool input_Judge()
        {
            if (mySystem.Parameter.NametoID(tb清场人.Text) <= 0)
            {
                MessageBox.Show("清场人ID不存在");
                return false;
            }
            return true;
        }

        //重写函数，获得审核信息
        public override void CheckResult()
        {
            base.CheckResult();
            tb检查人.Text = checkform.userName;
            dt_prodinstr.Rows[0]["检查人"] = checkform.userName;
            dt_prodinstr.Rows[0]["审核人"] = checkform.userName;
            ckb合格.Checked = checkform.ischeckOk;
            ckb不合格.Checked = !ckb合格.Checked;
            dt_prodinstr.Rows[0]["检查结果"] = checkform.ischeckOk;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            bt打印.Enabled = true;

            checkout = checkform.ischeckOk;
            if (checkform.ischeckOk)
            {
                bt打印.Enabled = true;
                bt保存.Enabled = false;
                bt审核.Enabled = false;
                dataGridView1.ReadOnly = true;
                dataGridView2.ReadOnly = true;
                tb生产指令.Enabled = false;
                tb产品代码.Enabled = false;
                tb备注.Enabled = false;
                tb产品批号.Enabled = false;
                dtp清场日期.Enabled = false;

                new mySystem.ProdctDaily_extrus(mainform);
                new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform);

                DialogResult result;
                result = MessageBox.Show("是否确定完成当前生产指令", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataTable dt_tempdt = new DataTable("生产指令信息");
                    OleDbDataAdapter da_tempdt = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                    OleDbCommandBuilder cb_prodinstr = new OleDbCommandBuilder(da_tempdt);
                    da_tempdt.Fill(dt_tempdt);

                    dt_tempdt.Rows[0]["状态"] = 4;
                    da_tempdt.Update(dt_tempdt);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }


        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产指令"] = mySystem.Parameter.proInstruction;
            dr["清场前产品代码"] =code;
            dr["清场前产品批号"] = batch;
            dr["清场日期"] = DateTime.Now;
            dr["检查结果"]=false;
            dr["清场人"] = mySystem.Parameter.userName;
            dr["审核时间"] = DateTime.Now;
            //缺少备注....................
            
            return dr;

        }
        // 给内表的一行写入默认值 datagridview1
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["吹膜工序清场记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["清场操作"] = "合格";
            return dr;
        }

        // 给内表的一行写入默认值 datagridview2
        DataRow writeInnerDefault2(DataRow dr)
        {
            dr["吹膜工序清场记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["清场操作"] = "合格";
            return dr;
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("吹膜工序清场记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序清场记录 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据,datagridview1,对应供料清场项目
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜工序清场项目记录");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜工序清场项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            if (dt_prodlist.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_供料设置.Rows)
                {
                    DataRow ndr = dt_prodlist.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场要点"] = dr["清场内容"];
                    ndr["清场操作"] ="合格";
                    dt_prodlist.Rows.Add(ndr);
                }
            }
        }

        void readInnerData2(int id)
        {
            dt_prodlist2 = new DataTable("吹膜工序清场吹膜工序项目记录");
            bs_prodlist2 = new BindingSource();
            da_prodlist2 = new OleDbDataAdapter("select * from 吹膜工序清场吹膜工序项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
            da_prodlist2.Fill(dt_prodlist2);

            if (dt_prodlist2.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_吹膜设置.Rows)
                {
                    DataRow ndr = dt_prodlist2.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场要点"] = dr["清场内容"];
                    ndr["清场操作"] = "合格";
                    dt_prodlist2.Rows.Add(ndr);
                }
            }
        }

        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb生产指令.DataBindings.Clear();
            tb产品代码.DataBindings.Clear();
            tb产品批号.DataBindings.Clear();
            dtp清场日期.DataBindings.Clear();

            tb清场人.DataBindings.Clear();
            tb检查人.DataBindings.Clear();

            tb备注.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令");
            tb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品代码");
            tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品批号");
            tb清场人.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场人");
            tb检查人.DataBindings.Add("Text", bs_prodinstr.DataSource, "检查人");
            dtp清场日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "清场日期");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
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
            setDataGridViewRowNums();
        }

        void innerBind2()
        {
            //移除所有列
            while (dataGridView2.Columns.Count > 0)
                dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            setDataGridViewCombox2();
            bs_prodlist2.DataSource = dt_prodlist2;
            dataGridView2.DataSource = bs_prodlist2.DataSource;
            setDataGridViewColumns2();
            setDataGridViewRowNums2();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清场操作":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
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
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }

        void setDataGridViewCombox2()
        {
            foreach (DataColumn dc in dt_prodlist2.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清场操作":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("合格");
                        c1.Items.Add("不合格");
                        dataGridView2.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c2);
                        break;
                }
            }
        }

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }

        void setDataGridViewColumns2()
        {
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;

        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }
        void setDataGridViewRowNums2()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            (new Record_extrusSiteClean(mainform, 1)).Show();
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            print(true);
        }
        public void print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //string dir = System.IO.Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(@"E:\gitpro\mitprodoc\甲方给的吹膜工序文档\A 下拉菜单文件\SOP-MFG-301-R11 吹膜工序清场记录_吴.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            fill_excel(my);

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
                my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[3, 1].Value = "生产指令："+mySystem.Parameter.proInstruction;
            my.Cells[3, 3].Value = "清场前产品代码及批号：" + tb产品代码.Text + "  " + tb产品批号.Text ;
            my.Cells[3, 5].Value = "清场日期：" + dtp清场日期.Value.ToLongDateString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[5 + i, 2].Value = i+1;
                my.Cells[5 + i, 3].Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 4].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
            }
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                my.Cells[11 + i, 2].Value = i + 1;
                my.Cells[11 + i, 3].Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[11 + i, 4].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
            }
            my.Cells[5, 5].Value = tb清场人.Text;
            my.Cells[5, 6].Value = checkout==true?"合格":"不合格";
            my.Cells[5, 7].Value = tb检查人.Text;
        }
    }
}
