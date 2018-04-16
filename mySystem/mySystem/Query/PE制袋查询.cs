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
        private SqlDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private SqlCommandBuilder cb;  

        public PE制袋查询()
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            Initdgv();

            textBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox1_PreviewKeyDown);
            comboBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox1_PreviewKeyDown);
            comboBox2.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox2_PreviewKeyDown);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
        }

        void comboBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        //下拉框获取生产指令
        public void comboInit()
        {
            if (Parameter.isSqlOk)
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = mySystem.Parameter.conn;
                comm.CommandText = "select * from 生产指令";
                SqlDataReader reader = comm.ExecuteReader();
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
            SqlCommand comm = new SqlCommand();
            comm.Connection = mySystem.Parameter.conn;
            comm.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "'";
            SqlDataReader reader = comm.ExecuteReader();
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
                        { EachBind(this.dgv, "生产领料使用记录", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "生产领料使用记录", "审核员", null, null); }
                        break;
                    case "产品内包装记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品内包装记录", "审核员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品内包装记录", "审核员", "生产日期", null); }
                        break;
                    case "LDPE生产日报表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "LDPE制袋日报表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "LDPE制袋日报表", "审核员", null, null); }
                        break;                   
                    case "1#制袋机开机前确认表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "制袋机组开机前确认表", "操作员", "操作日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "制袋机组开机前确认表", "操作员", "操作日期", null); }
                        break;
                    case "1#制袋机运行记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "制袋机组运行记录", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "制袋机组运行记录", "审核员", null, null); }
                        break;
                    case "清场记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "清场记录", "操作员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "清场记录", "操作员", "生产日期", null); }
                        break;
                    case "制袋工序批生产记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "批生产记录表", "汇总员", "开始生产时间", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "批生产记录表", "汇总员", "开始生产时间", null); }
                        break;
                    case "产品热合强度检验记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品热合强度检验记录", "整理人", "整理时间", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品热合强度检验记录", "整理人", "整理时间", null); }
                        break;
                    case "产品外观和尺寸检验记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品外观和尺寸检验记录", "操作员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品外观和尺寸检验记录", "操作员", "生产日期", null); }
                        break;
                    case "产品外包装记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品外包装记录表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品外包装记录表", "审核员", null, null); }
                        break;
                    case "生产退料记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "生产退料记录表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "生产退料记录表", "审核员", null, null); }
                        break;
                    case "洁净区温湿度记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "洁净区温湿度记录表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "洁净区温湿度记录表", "审核员", null, null); }
                        break;
                    case "岗位交接班记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "岗位交接班记录", null, "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "岗位交接班记录", null, "生产日期", null); }
                        break;
                    case "生产领料申请单":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "生产领料申请单表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "生产领料申请单表", "审核员", null, null); }
                        break;

                    default:
                        break;
                }
            }

            catch (Exception ee)
            {
                MessageBox.Show("输入有误，请重新输入" + ee.Message + "\n" + ee.StackTrace, "错误");
                return;
            }
            Utility.setDataGridViewAutoSizeMode(dgv);
        }

        // 各表查询
        private void EachBind(DataGridView dgv, String tblName, String person, String startDate, String instruID)
        {
            dt = new DataTable(tblName); //""中的是表名
            if (person != null && startDate != null && instruID == null) // 人 + 日期
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'", mySystem.Parameter.conn);
            else if (person == null && startDate != null && instruID != null) // 日期 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person != null && startDate == null && instruID != null) // 人 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person != null && startDate == null && instruID == null) // 人 
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'", mySystem.Parameter.conn);
            else if (person == null && startDate != null && instruID == null) // 日期
                da = new SqlDataAdapter("select * from " + tblName + " where " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'", mySystem.Parameter.conn);
            else if (person == null && startDate == null && instruID != null) // 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person == null && startDate == null && instruID == null) // 只有表名
                da = new SqlDataAdapter("select * from " + tblName, mySystem.Parameter.conn);
            else if (person != null && startDate != null && instruID != null) // 人 + 日期 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);

            cb = new SqlCommandBuilder(da);
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
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    int selectIndex = this.dgv.CurrentRow.Index;
                    int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                    switch (tableName)
                    {
                        case "生产领料使用记录":
                            LDPEBag_materialrecord material = new LDPEBag_materialrecord(mainform, ID);
                            material.ShowDialog();
                            break;
                        case "产品内包装记录":
                            LDPEBag_innerpackaging inner = new LDPEBag_innerpackaging(mainform, ID);
                            inner.ShowDialog();
                            break;
                        case "LDPE生产日报表":
                            //LDPEBag_dailyreport daily = new LDPEBag_dailyreport(mainform, ID);
                            //daily.Show();
                            break;
                        case "1#制袋机开机前确认表":
                            LDPEBag_checklist check = new LDPEBag_checklist(mainform, ID);
                            check.ShowDialog();
                            break;
                        case "1#制袋机运行记录":
                            LDPEBag_runningrecord run = new LDPEBag_runningrecord(mainform, ID);
                            run.ShowDialog();
                            break;
                        case "清场记录":
                            LDPEBag_cleanrance cleanrance = new LDPEBag_cleanrance(mainform, ID);
                            cleanrance.ShowDialog();
                            break;
                        case "制袋工序批生产记录":
                            LDPEBag_batchproduction batch = new LDPEBag_batchproduction(mainform, ID);
                            batch.ShowDialog();
                            break;
                        case "产品热合强度检验记录":
                            产品热合强度检验记录 form7 = new 产品热合强度检验记录(mainform, ID);
                            form7.ShowDialog();
                            break;
                        case "产品外观和尺寸检验记录":
                            产品外观和尺寸检验记录 form8 = new 产品外观和尺寸检验记录(mainform, ID);
                            form8.ShowDialog();
                            break;
                        case "产品外包装记录":
                            LDPE产品外包装记录 form9 = new LDPE产品外包装记录(mainform, ID);
                            form9.ShowDialog();
                            break;
                        case "生产退料记录":
                            LDPE生产退料记录 form10 = new LDPE生产退料记录(mainform, ID);
                            form10.ShowDialog();
                            break;
                        case "洁净区温湿度记录":
                            LDPE洁净区温湿度记录 form11 = new LDPE洁净区温湿度记录(mainform, ID);
                            form11.ShowDialog();
                            break;
                        case "岗位交接班记录":
                            HandOver form12 = new HandOver(mainform, ID);
                            form12.ShowDialog();
                            break;
                        case "生产领料申请单":
                            LDPE生产领料申请单 fomr13 = new LDPE生产领料申请单(mainform, ID,this);

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
                }
            }
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
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 0);
                }
                else if (dgv.Rows[i].Cells[checker].Value.ToString() == "")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
            }
        }


    }
}
