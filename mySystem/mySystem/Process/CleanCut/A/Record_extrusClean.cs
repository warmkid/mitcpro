﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    //由生产指令唯一确定该表
    public partial class Record_extrusClean : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access

        int instrid;//生产指令id

        mySystem.CheckForm checkform;//审核信息
        private DataTable dt;//之前清洁项目

        private DataTable dt_out, dt_in;
        private OleDbDataAdapter da_out, da_in;
        private BindingSource bs_out, bs_in;
        private OleDbCommandBuilder cb_out, cb_in;

        public Record_extrusClean(mySystem.MainForm mainform)
            : base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            Init();
            readset();
            begin();

        }
        //读取设置里面的清洁内容
        private void readset()
        {
            string asql = "select * from 设置吹膜机组清洁项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            da.Fill(dt);
            
        }
        //判断之前的内容是否与设置表中内容一致
        private bool is_sameto_setting()
        {
            if (dt.Rows.Count != dt_in.Rows.Count)
                return false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["清洁区域"].ToString() != dt_in.Rows[i]["清洁区域"].ToString() || dt.Rows[i]["清洁内容"].ToString() != dt_in.Rows[i]["清洁内容"].ToString())
                    return false;
            }
            return true;
        }
        private void begin()
        {
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();
            if (dt_out.Rows.Count <= 0)
            {
                DataRow dr = dt_out.NewRow();
                dr = writeOuterDefault(dr);
                dt_out.Rows.Add(dr);
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(mySystem.Parameter.proInstruID);
                removeOuterBinding();
                outerBind();
            }

            ckb白班.Checked = (bool)dt_out.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            readInnerData((int)dt_out.Rows[0]["ID"]);
            innerBind();

            DialogResult result;
            if (!is_sameto_setting())
            {
                result = MessageBox.Show("检测到之前的记录与目前设置中不一致，保留当前设置选择是，保留之前记录设置选择否", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)//保留当前设置
                {                  
                    while (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    da_in.Update((DataTable)bs_in.DataSource);
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow ndr = dt_in.NewRow();
                        ndr[1] = (int)dt_out.Rows[0]["ID"];
                        // 注意ID不要复制过去了，所以从1开始
                        for (int i = 1; i < dr.Table.Columns.Count; ++i)
                        {
                            ndr[i + 1] = dr[i];
                        }
                        // 这里添加检查是否合格、检查人、审核人等默认信息
                        ndr["合格"] = "合格";
                        ndr["清洁人"] = mySystem.Parameter.userName;
                        ndr["检查人"] = "";
                        dt_in.Rows.Add(ndr);
                    }
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    //da_in.Update((DataTable)bs_in.DataSource);

                }

            }


            if ((bool)dt_out.Rows[0]["审核是否通过"])
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

            dataGridView1.Font = new Font("宋体", 10);
            bt审核.Enabled = false;
            tb复核人.Enabled = false;

            instrid = mySystem.Parameter.proInstruID;
            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;
            dtp复核日期.Enabled = false;
            dt = new DataTable();

            connOle = mySystem.Parameter.connOle;
            dt_out = new DataTable();
            dt_in = new DataTable();
            da_out = new OleDbDataAdapter();
            da_in = new OleDbDataAdapter();
            bs_out = new BindingSource();
            bs_in = new BindingSource();
            cb_out = new OleDbCommandBuilder();
            cb_in = new OleDbCommandBuilder();

            bt打印.Enabled = false;
            bt审核.Enabled = false;
            
        }

       //供界面显示,参数为数据库中对应记录的id
        public void show(int paraid)
        {
            string asql = "select 生产指令ID from 吹膜机组清洁记录表 where ID=" + paraid;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return;
            else
                fill_by_id((int)tempdt.Rows[0][0]);
        }

        //根据生产指令id将数据填写到各控件中
        private int fill_by_id(int id)
        {
            #region 以前
            //string asql = "select s1_clean_date,s1_flight,s1_reviewer_id,s1_review_date,s1_region_result_cleaner_reviewer from extrusion_s1_cleanrecord where production_instruction="  + id ;
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //DataTable tempdt = new DataTable();
            //da.Fill(tempdt);
            //if (tempdt.Rows.Count == 0)
            //    return -1;
            //else
            //{
            //    //将tempdt填入控件
            //    dateTimePicker1.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());
            //    //comboBox1.Text = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? "白班" : "夜班";
            //    checkBox1.Checked = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? true : false;
            //    checkBox2.Checked = !checkBox1.Checked;
            //    string rev = tempdt.Rows[0][2].ToString();
            //    if (rev == "")
            //        button2.Enabled = true;
            //    else
            //    {
            //        button1.Enabled = false;
            //        button2.Enabled = false;
            //        textBox2.Text = mySystem.Parameter.IDtoName(int.Parse(rev));
            //        dateTimePicker2.Value = DateTime.Parse(tempdt.Rows[0][3].ToString());
            //        dataGridView1.ReadOnly = true;
            //        dateTimePicker1.Enabled = false;
            //        dateTimePicker2.Enabled = false;
            //    }

            //    string jstr = tempdt.Rows[0][4].ToString();
            //    JArray jarray = JArray.Parse(jstr);
            //    for (int i = 0; i < jarray.Count; i++)
            //    {
            //        JObject jobj = JObject.Parse(jarray[i].ToString());
            //        foreach (var p in jobj)
            //        {
            //            DataGridViewRow dr = new DataGridViewRow();
            //            dataGridView1.Rows.Add(dr);
            //            dataGridView1.Rows[i].Cells[0].Value = p.Key;//名称
            //            dataGridView1.Rows[i].Cells[1].Value =cont_findby_name( p.Key);//内容
            //            if (int.Parse(jobj[p.Key][0].ToString()) == 1)
            //            {
            //                //白班
            //                dataGridView1.Rows[i].Cells[2].Value = "True";
            //                dataGridView1.Rows[i].Cells[3].Value = "False";
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[i].Cells[3].Value = "True";
            //                dataGridView1.Rows[i].Cells[2].Value = "False";
            //            }
            //            dataGridView1.Rows[i].Cells[4].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][1].ToString()));
            //            dataGridView1.Rows[i].Cells[5].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][2].ToString()));
            //        }
            //    }
            //}
            //return 0;
            #endregion

            string asql = "select 清洁日期,班次,审核人,审核是否通过,审核时间,ID from 吹膜机组清洁记录表 where 生产指令ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
            {
                //将tempdt填入控件
                dtp清洁日期.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());//清洁日期
                ckb白班.Checked = tempdt.Rows[0][1].ToString()=="True";//班次
                ckb夜班.Checked = !ckb白班.Checked;

                bool revisok = (bool)tempdt.Rows[0][3];//审核是否通过

                if (!revisok)//审核未通过
                    bt审核.Enabled = true;
                else
                {
                    bt保存.Enabled = false;
                    bt审核.Enabled = false;
                    bt打印.Enabled = true;
                    tb复核人.Text = tempdt.Rows[0][2].ToString();
                    dtp复核日期.Value = (DateTime)tempdt.Rows[0][4];
                    dataGridView1.ReadOnly = true;
                    dtp清洁日期.Enabled = false;
                    dtp复核日期.Enabled = false;
                }
                //填写表格
                int tempid = (int)tempdt.Rows[0][5];
                comm.CommandText = "select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + tempid;
                da.Dispose();
                tempdt.Dispose();
                da = new OleDbDataAdapter(comm);
                tempdt = new DataTable();
                da.Fill(tempdt);//获得数据

                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dataGridView1.Rows.Add(dr);
                    dataGridView1.Rows[i].Cells[0].Value = (string)tempdt.Rows[i][2];//名称
                    dataGridView1.Rows[i].Cells[1].Value = (string)tempdt.Rows[i][3];//内容
                    dataGridView1.Rows[i].Cells[2].Value = (string)tempdt.Rows[i][4]=="合格"?"合格":"不合格";//合格 是
                    dataGridView1.Rows[i].Cells[3].Value = (string)tempdt.Rows[i][4] == "合格" ? "不合格" : "合格";
                    dataGridView1.Rows[i].Cells[4].Value = (string)tempdt.Rows[i][6];
                    dataGridView1.Rows[i].Cells[5].Value = (string)tempdt.Rows[i][7];
                }
                return 0;
            }

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (this.dataGridView1.IsCurrentCellDirty)
            //{

            //    this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //}
        }

        //单元格编辑结束触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //更改清洁人项
            if (e.ColumnIndex == 5)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[5].Value == null || dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "")
                    return;
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("清洁人id不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "";
                }
                return;
            }
            //更改审核人项
            if (e.ColumnIndex == 6)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value == null || dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "")
                    return;
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("审核人id不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[6].Value = "";
                }
                return;
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            string asql = "select ID from users where 姓名=" + "'" + s + "'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOleUser);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
                return (int)tempdt.Rows[0][0];

        }

        private bool input_Judge()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "" || dataGridView1.Rows[i].Cells[6].Value.ToString() == "")
                {
                    return false;
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bt审核.Enabled = false;
            //判断合法性
            if (!input_Judge())
            {
                MessageBox.Show("清洁人与审核人均不能为空");
                return;
            }
                
            //外表保存
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_in.Update((DataTable)bs_in.DataSource);
            readInnerData(Convert.ToInt32(dt_out.Rows[0]["ID"]));
            innerBind();

            bt审核.Enabled = true;
        }

        //重写函数，获得审查人信息
        public override void CheckResult()
        {
            base.CheckResult();
            dt_out.Rows[0]["审核人"] = checkform.userName;
            dt_out.Rows[0]["审核时间"] = checkform.time;
            dt_out.Rows[0]["审核意见"] = checkform.opinion;
            dt_out.Rows[0]["审核是否通过"] = checkform.ischeckOk;
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

            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean s = new Record_extrusSiteClean(mainform);
            s.Show();
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] =mySystem.Parameter.proInstruID ;
            dr["清洁日期"] = DateTime.Now;
            dr["班次"] = mySystem.Parameter.userflight=="白班";//白班代表是
            dr["审核时间"] = DateTime.Now;
            dr["审核是否通过"] = false;
            ckb白班.Checked = (bool)dr["班次"];
            ckb夜班.Checked = !ckb白班.Checked;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["吹膜机组清洁记录ID"] = dt_out.Rows[0]["ID"];
            dr["合格"] = "合格";
            dr["清洁人"] = mySystem.Parameter.userName;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {

            dt_out = new DataTable("吹膜机组清洁记录表");
            bs_out = new BindingSource();
            da_out = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where 生产指令ID=" +instrid, connOle);
            cb_out = new OleDbCommandBuilder(da_out);
            da_out.Fill(dt_out);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_in = new DataTable("吹膜机组清洁项目记录表");
            bs_in = new BindingSource();
            da_in = new OleDbDataAdapter("select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + id, connOle);
            cb_in = new OleDbCommandBuilder(da_in);
            da_in.Fill(dt_in);

            if (dt_in.Rows.Count <= 0)//空表，按照设置表内容进行插入
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow ndr = dt_in.NewRow();
                    ndr[1] = (int)dt_out.Rows[0]["ID"];
                    // 注意ID不要复制过去了，所以从1开始
                    for (int i = 1; i < dr.Table.Columns.Count; ++i)
                    {
                        ndr[i + 1] = dr[i];
                    }
                    // 这里添加检查是否合格、检查人、审核人等默认信息
                    ndr["合格"] = "合格";
                    ndr["清洁人"] = mySystem.Parameter.userName;
                    ndr["检查人"] = "";
                    dt_in.Rows.Add(ndr);
                }
            }
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            dtp清洁日期.DataBindings.Clear();
            dtp复核日期.DataBindings.Clear();
            //ckb白班.DataBindings.Clear();
            tb复核人.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_out.DataSource = dt_out;

            tb复核人.DataBindings.Add("Text", bs_out.DataSource, "审核人");
            dtp清洁日期.DataBindings.Add("Value", bs_out.DataSource, "清洁日期");
            dtp复核日期.DataBindings.Add("Value", bs_out.DataSource, "审核时间");
            //ckb白班.DataBindings.Add("Checked", bs_out.DataSource,"班次");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewCombox();
            bs_in.DataSource = dt_in;
            dataGridView1.DataSource = bs_in.DataSource;
            setDataGridViewColumns();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_in.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "合格":
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
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}
        }

        private void Record_extrusClean_Load(object sender, EventArgs e)
        {

        }
    }
}
