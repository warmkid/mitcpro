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

namespace mySystem.Process.CleanCut
{
    public partial class Record_cleansite_cut : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist;//
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        DataTable dt_清场设置;

        public Record_cleansite_cut(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            Init();
            readsetting();
            begin();
        }

        //判断之前的内容是否与设置表中内容一致
        private bool is_sameto_setting()
        {
            if (dt_清场设置.Rows.Count != dt_prodlist.Rows.Count)
                return false;
            for (int i = 0; i < dt_清场设置.Rows.Count; i++)
            {
                if (dt_清场设置.Rows[i]["清场项目"].ToString() != dt_prodlist.Rows[i]["清场项目"].ToString() || dt_清场设置.Rows[i]["清场要点"].ToString() != dt_prodlist.Rows[i]["清场要点"].ToString())
                    return false;
            }
            return true;
        }

        private void begin()
        {
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

            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["生产班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            DialogResult result;
            if (!is_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                    int tempi = 1;
                    foreach (DataRow dr in dt_清场设置.Rows)
                    {
                        DataRow ndr = dt_prodlist.NewRow();
                        ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                        ndr[2] = tempi++;//序号
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 2; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 1] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["清洁操作"] = "合格";

                        dt_prodlist.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
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

        //初始化
        private void Init()
        {
            bt审核.Enabled = false;
            bt打印.Enabled = false;
            ckb不合格.Enabled = false;
            ckb合格.Enabled = false;
            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;

            dt_清场设置 = new DataTable();

            dt_prodinstr = new DataTable();
            dt_prodlist = new DataTable();
            da_prodinstr = new OleDbDataAdapter();
            da_prodlist = new OleDbDataAdapter();
            bs_prodinstr = new BindingSource();
            bs_prodlist = new BindingSource();
            cb_prodinstr = new OleDbCommandBuilder();
            cb_prodlist = new OleDbCommandBuilder();

        }

        //读取设置中清场项目
        private void readsetting()
        {
            string asql = "select * from 设置清场项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            da.Fill(dt_清场设置);
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产日期"] = DateTime.Now;
            dr["生产班次"] = mySystem.Parameter.userflight=="白班"?true:false;
            dr["清场人"] = mySystem.Parameter.userName;
            return dr;

        }
        // 给内表的一行写入默认值 datagridview1
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T清洁分切运行记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["生产时间"] = DateTime.Now;
            dr["分切速度"] = 0;
            dr["自动张力设定"] = 0;
            dr["自动张力显示"] = 0;
            dr["张力输出显示"] = 0;
            dr["操作人"] =mySystem.Parameter.userName;
            return dr;
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("清场记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 清场记录 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据,datagridview1,对应供料清场项目
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("清场记录详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 清场记录详细信息 where T清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            if (dt_prodlist.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                int index = 1;
                foreach (DataRow dr in dt_清场设置.Rows)
                {
                    DataRow ndr = dt_prodlist.NewRow();
                    ndr[1] = (int)dt_prodinstr.Rows[0]["ID"];
                    ndr["序号"] = index++;
                    ndr["清场项目"] = dr["清场项目"];
                    ndr["清场要点"] = dr["清场要点"];
                    ndr["清洁操作"] = "合格";
                    dt_prodlist.Rows.Add(ndr);
                }
            }
        }

        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            cb产品代码.DataBindings.Clear();
            tb产品规格.DataBindings.Clear();
            tb产品批号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();

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

            cb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品代码");
            tb产品规格.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品规格");
            tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品批号");
            tb清场人.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场人");
            tb检查人.DataBindings.Add("Text", bs_prodinstr.DataSource, "检查人");
            dtp生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "生产日期");
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
            //setDataGridViewColumns();
            setDataGridViewRowNums();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "清洁操作":
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

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//外键

        }

        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }

            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//外键
        }



        //保存按钮
        private void bt保存_Click(object sender, EventArgs e)
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

            bt审核.Enabled = true;
        }

        //判断合法性
        private bool input_Judge()
        {
            if (mySystem.Parameter.NametoID(tb清场人.Text) <= 0)
            {
                MessageBox.Show("清场人ID不存在");
                return false;
            }
            return true;
        }

        //审核按钮
        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        public override void CheckResult()
        {
            dt_prodinstr.Rows[0]["审核人"] = checkform.userName;
            dt_prodinstr.Rows[0]["检查人"] = checkform.userName;

            dt_prodinstr.Rows[0]["检查结果"] = checkform.ischeckOk==true?"合格":"不合格" ;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            if (checkform.ischeckOk)
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                bt打印.Enabled = true;

            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            base.CheckResult();
        }
    }
}
