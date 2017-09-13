using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using mySystem.Process.灭菌;

namespace mySystem.Query
{
    public partial class 灭菌查询 : BaseForm
    {
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        String writer;//操作员
        String Instruction = null;//下拉框获取的委托单号
        int InstruID;//下拉框获取的委托单号
        String tableName = null;
        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;  

        public 灭菌查询()
        {
            InitializeComponent();
            Initdgv();
            comboInit();

            textBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox1_PreviewKeyDown);
            comboBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox1_PreviewKeyDown);
            comboBox2.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox2_PreviewKeyDown);
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
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from Gamma射线辐射灭菌委托单 ";
                OleDbDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["委托单号"]);
                    }
                }
                comm.Dispose();
            }
            else
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instruction = comboBox1.SelectedItem.ToString();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from Gamma射线辐射灭菌委托单 where 委托单号 = '" + Instruction + "'";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                InstruID = Convert.ToInt32(reader["ID"]);
            }
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
                    case "Gamma 射线辐射灭菌委托单":
                        if (comboBox1.SelectedIndex == -1)
                            EachBind(this.dgv, "Gamma射线辐射灭菌委托单", "委托人", "委托日期", null);
                        else
                            EachBind(this.dgv, "Gamma射线辐射灭菌委托单", "委托人", "委托日期", "ID"); 
                        break;
                    case "Gamma 射线辐照灭菌产品验收记录":
                        if (comboBox1.SelectedIndex == -1)
                            EachBind(this.dgv, "辐照灭菌产品验收记录", "验收人", "验收日期", null);
                        else
                            EachBind(this.dgv, "辐照灭菌产品验收记录", "验收人", "验收日期", "灭菌委托单ID"); 
                        break;
                    case "辐照灭菌台帐":
                        EachBind(this.dgv, "辐照灭菌台帐详细信息", null, null, null); 
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
            if (dgv.Rows.Count > 0)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
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
                    case "Gamma 射线辐射灭菌委托单":
                        Gamma射线辐射灭菌委托单 mydlg1 = new Gamma射线辐射灭菌委托单(mainform, ID);
                        mydlg1.Show();
                        break;
                    case "Gamma 射线辐照灭菌产品验收记录":
                        辐照灭菌产品验收记录 mydlg2 = new 辐照灭菌产品验收记录(mainform, ID);
                        mydlg2.Show();
                        break;
                    case "辐照灭菌台帐":

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
            { dgv.Columns["灭菌委托单ID"].Visible = false; }
            catch
            { }
            try
            { setDataGridViewBackColor("审核人"); }
            catch
            { }
            try
            { setDataGridViewBackColor("审核员"); }
            catch
            { }
            try
            { setDataGridViewBackColor("审批"); }
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
