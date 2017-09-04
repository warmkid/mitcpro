﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using mySystem.Process.Bag.LDPE;

namespace mySystem.Query
{
    public partial class PE制袋查询 : BaseForm
    {
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        String writer;//操作员
        String Instruction = null;//下拉框获取的生产指令
        int InstruID;//下拉框获取的生产指令ID
        String tableName = null;
        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;  

        public PE制袋查询()
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            Initdgv();
        }

        //下拉框获取生产指令
        public void comboInit()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from 生产指令";
                OleDbDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }
                comm.Dispose();
            }
            else
            { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instruction = comboBox1.SelectedItem.ToString();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "'";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                InstruID = Convert.ToInt32(reader["ID"]);
            }
            reader.Dispose();
            comm.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = comboBox2.SelectedItem.ToString();
        }

        //dgv样式初始化
        private void Initdgv()
        {
            bs = new BindingSource();
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = true;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);

        }

        private void Bind()
        {
            try
            {
                switch (tableName)
                {
                    case "生产领料使用记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "生产领料使用记录", "领料员", "领料日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "生产领料使用记录", "领料员", "领料日期", null); }
                        break;
                    case "产品内包装记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品内包装记录", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品内包装记录", "审核员", null, null); }
                        break;
                    case "LDPE生产日报表":
                        //if (comboBox1.SelectedIndex != -1)
                        //{ EachBind(this.dgv, "CS制袋日报表", "审核员", null, "生产指令ID"); }
                        //else
                        //{ EachBind(this.dgv, "CS制袋日报表", "审核员", null, null); }
                        break;                   
                    case "1#制袋机开机前确认表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "制袋机组开机前确认表", "操作员", "操作日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "制袋机组开机前确认表", "操作员", "操作日期", null); }
                        break;
                    case "1#制袋机运行记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "制袋机组运行记录", "审核员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "制袋机组运行记录", "审核员", "生产日期", null); }
                        break;
                    case "清场记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "清场记录", "清场员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "清场记录", "清场员", "生产日期", null); }
                        break;
                    case "制袋工序批生产记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "批生产记录表", "汇总员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "批生产记录表", "汇总员", "生产日期", null); }
                        break;

                    default:
                        break;
                }
            }

            catch
            {
                MessageBox.Show("输入有误，请重新输入", "错误");
                return;
            }

        }

        // 各表查询
        private void EachBind(DataGridView dgv, String tblName, String person, String startDate, String instruID)
        {
            dt = new DataTable(tblName); //""中的是表名
            if (person != null && startDate != null && instruID == null) // 人 + 日期
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            else if (person == null && startDate != null && instruID != null) // 日期 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person != null && startDate == null && instruID != null) // 人 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person != null && startDate == null && instruID == null) // 人 
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'", mySystem.Parameter.connOle);
            else if (person == null && startDate != null && instruID == null) // 日期
                da = new OleDbDataAdapter("select * from " + tblName + " where " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            else if (person == null && startDate == null && instruID != null) // 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person == null && startDate == null && instruID == null) // 只有表名
                da = new OleDbDataAdapter("select * from " + tblName, mySystem.Parameter.connOle);
            else if (person != null && startDate != null && instruID != null) // 人 + 日期 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);

            cb = new OleDbCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            dgv.DataBindings.Clear();
            dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();
        }


        //填序号列的值
        private void setDataGridViewRowNums()
        {
            int coun = this.dgv.Rows.Count;
            for (int i = 0; i < coun; i++)
            {
                this.dgv.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }



        private void SearchBtn_Click(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.Date;
            date2 = dateTimePicker2.Value.Date;
            writer = textBox1.Text.Trim();

            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择表单！", "提示");
                return;
            }

            Bind();
        }

        //双击弹出界面
        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int selectIndex = this.dgv.CurrentRow.Index;
                int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                switch (tableName)
                {
                    case "生产领料使用记录":
                        LDPEBag_materialrecord material = new LDPEBag_materialrecord(mainform, ID);
                        material.Show();
                        break;
                    case "产品内包装记录":
                        LDPEBag_innerpackaging inner = new LDPEBag_innerpackaging(mainform, ID);
                        inner.Show();
                        break;
                    case "LDPE生产日报表":
                        //LDPEBag_dailyreport daily = new LDPEBag_dailyreport(mainform, ID);
                        //daily.Show();
                        break;
                    case "1#制袋机开机前确认表":
                        LDPEBag_checklist check = new LDPEBag_checklist(mainform, ID);
                        check.Show();
                        break;
                    case "1#制袋机运行记录":
                        LDPEBag_runningrecord run = new LDPEBag_runningrecord(mainform, ID);
                        run.Show();
                        break;
                    case "清场记录":
                        LDPEBag_cleanrance cleanrance = new LDPEBag_cleanrance(mainform, ID);
                        cleanrance.Show();
                        break;
                    case "制袋工序批生产记录":
                        LDPEBag_batchproduction batch = new LDPEBag_batchproduction(mainform, ID);
                        batch.Show();
                        break;

                    default:
                        break;
                }
            }
            catch
            { }
        }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //设置列宽
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                String colName = dgv.Columns[i].HeaderText;
                int strlen = colName.Length;
                dgv.Columns[i].MinimumWidth = strlen * 25;
            }

            dgv.Columns["ID"].Visible = false;
            try
            { dgv.Columns["生产指令ID"].Visible = false; }
            catch
            { }
            try
            { setDataGridViewBackColor("审核员"); }
            catch
            { }
        }

        //设置datagridview背景颜色，待审核标红
        private void setDataGridViewBackColor(String checker)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[checker].Value.ToString() == "__待审核")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }


    }
}
