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
using System.Collections;

namespace mySystem.Process.Stock
{
    public partial class 原料入库管理 : BaseForm
    {

//        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        DataTable dt物资验收记录, dt物资请验单, dt复验记录, dt不合格品处理记录, dt取样记录,dt到货单,dt入库单,dt检验记录;

        public 原料入库管理(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConn);
            //conn.Open();


            // TODO 默认不读全部记录，一周内的
            read到货单Data();
            到货单Bind();
            read物资验收记录Data();
            物资验收记录Bind();
            read入库单Data();
            入库单Bind();
            read物资请验单Data();
            物资请验单Bind();
            read复验记录Data();
            复验记录Bind();
            read不合格品记录Data();
            不合格品记录Bind();
            read取样记录Data();
            取样记录Bind();
            read检验记录Data();
            检验记录Bind();
            addOtherEventHandler();

            setQueryControl();
        }

        private void setQueryControl()
        {
            dateTimePicker开始.Value = DateTime.Now.AddDays(-7);
            dateTimePicker结束.Value = DateTime.Now;
            comboBox审核状态.Items.Add("__待审核");
            comboBox审核状态.SelectedIndex = 0;
        }

        private void btn增加物资验收记录_Click(object sender, EventArgs e)
        {
            物资验收记录 form = new 物资验收记录(mainform);
            form.Show();
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        void read物资验收记录Data()
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from 物资验收记录 where 接收时间 between '" +
                DateTime.Now.AddDays(-7).Date + "' and '" + DateTime.Now + "' ", mySystem.Parameter.conn);
            dt物资验收记录 = new DataTable("物资验收记录");
            da.Fill(dt物资验收记录);
        }

        void 物资验收记录Bind()
        {
            dataGridView1.DataSource = dt物资验收记录;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        void read入库单Data()
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from 入库单 where 入库日期 between '" +
                DateTime.Now.AddDays(-7).Date + "' and '" + DateTime.Now + "' ", mySystem.Parameter.conn);
            dt入库单 = new DataTable("入库单");
            da.Fill(dt入库单);
        }

        void 入库单Bind()
        {
            dgv入库单.DataSource = dt入库单;
            dgv入库单.DataBindingComplete += dgv入库单_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dgv入库单);
        }

        void dgv入库单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        void read检验记录Data()
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from 检验记录 where 检验日期 between '" +
                DateTime.Now.AddDays(-7).Date + "' and '" + DateTime.Now + "' ", mySystem.Parameter.conn);
            dt检验记录 = new DataTable("检验记录");
            da.Fill(dt检验记录);
        }

        void 检验记录Bind()
        {
            dgv检验记录.DataSource = dt检验记录;
            dgv检验记录.DataBindingComplete += dgv检验记录_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dgv检验记录);
        }

        void dgv检验记录_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }


        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView5.AllowUserToAddRows = false;
            dgv到货单.AllowUserToAddRows = false;
            dgv入库单.AllowUserToAddRows = false;
            dgv检验记录.AllowUserToAddRows = false;
            // TODO  加一个绑定完成事件，把需要审核的行标记
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView3.CellDoubleClick += dataGridView3_CellDoubleClick;
            dataGridView4.CellDoubleClick += dataGridView4_CellDoubleClick;
            dataGridView5.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView5_CellDoubleClick);
            dgv到货单.CellDoubleClick += new DataGridViewCellEventHandler(dgv到货单_CellDoubleClick);
            dgv入库单.CellDoubleClick += new DataGridViewCellEventHandler(dgv入库单_CellDoubleClick);
            dgv检验记录.CellDoubleClick += new DataGridViewCellEventHandler(dgv检验记录_CellDoubleClick);

            // 隐藏ID等列
            dataGridView1.Columns["ID"].Visible = false;
            //dataGridView1.Columns["物资验收记录ID"].Visible = false;

            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["物资验收记录ID"].Visible = false;

            dataGridView3.Columns["ID"].Visible = false;
            dataGridView3.Columns["物资验收记录ID"].Visible = false;

            dataGridView4.Columns["ID"].Visible = false;
            dataGridView4.Columns["物资验收记录ID"].Visible = false;

            dataGridView5.Columns["ID"].Visible = false;
            dataGridView5.Columns["物资验收记录ID"].Visible = false;

            dgv到货单.Columns["ID"].Visible = false;

            dgv入库单.Columns["ID"].Visible = false;
            dgv入库单.Columns["关联的验收记录ID"].Visible = false;

            dgv检验记录.Columns["ID"].Visible = false;
            dgv检验记录.Columns["物资验收记录ID"].Visible = false;


            // dgv只读
            dgv到货单.ReadOnly = true;
            dgv检验记录.ReadOnly = true;
            dgv入库单.ReadOnly = true;
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
            dataGridView3.ReadOnly = true;
            dataGridView4.ReadOnly = true;
            dataGridView5.ReadOnly = true;

            // 选项卡点击
            tabControl1.TabIndexChanged += tabControl1_TabIndexChanged;
        }

        void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            search();
        }

        void dgv检验记录_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv检验记录.Rows[e.RowIndex].Cells[0].Value);
                检验记录 form = new 检验记录(mainform, id);
                form.ShowDialog();
            }
        }

        void dgv入库单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv入库单.Rows[e.RowIndex].Cells[0].Value);
                入库单 form = new 入库单(mainform, id);
                form.ShowDialog();
            }
        }


        void dgv到货单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv到货单.Rows[e.RowIndex].Cells[0].Value);
                到货单 form = new 到货单(mainform,id);
                form.ShowDialog();
            }
        }

        void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView5.Rows[e.RowIndex].Cells[0].Value);
                取样记录 form = new 取样记录(mainform, id);
                form.Owner = this;
                form.ShowDialog();
            }
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                物资验收记录 form = new 物资验收记录(mainform, id);
                form.Owner = this;
                form.ShowDialog();
            }
        }

        void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells[0].Value);
                不合格品处理记录 form = new 不合格品处理记录(mainform, id);
                form.Owner = this;
                form.ShowDialog();
            }
        }

        void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells[0].Value);
                复验记录 form = new 复验记录(mainform, id);
                form.Owner = this;
                form.ShowDialog();
            }
        }

        void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //双击 显示请验单
                int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
                物资请验单 form = new 物资请验单(mainform, id);
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private void btn读取_Click(object sender, EventArgs e)
        {
            read物资请验单Data();
            物资请验单Bind();
            
            
        }

        void read物资请验单Data()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 物资请验单 where 请验时间 between '"
            + DateTime.Now.AddDays(-7).Date + "' and '" + DateTime.Now + "'", mySystem.Parameter.conn);
            dt物资请验单 = new DataTable("物资请验单");
            da.Fill(dt物资请验单);
        }

        void 物资请验单Bind()
        {
            dataGridView2.DataSource = dt物资请验单;
            dataGridView2.DataBindingComplete += dataGridView2_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }

        void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        private void btn读取检验记录_Click(object sender, EventArgs e)
        {
            read复验记录Data();
            复验记录Bind();
        }

        void read复验记录Data()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 复验记录 where  检验日期 between '"
            + DateTime.Now.AddDays(-7).Date + "' and '" + DateTime.Now + "'", mySystem.Parameter.conn);
            dt复验记录 = new DataTable("复验记录");
            da.Fill(dt复验记录);
        }

        void 复验记录Bind()
        {
            dataGridView3.DataSource = dt复验记录;
            dataGridView3.DataBindingComplete += dataGridView3_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dataGridView3);
        }

        void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

       


        void read不合格品记录Data()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 不合格品处理记录", mySystem.Parameter.conn);
            dt不合格品处理记录 = new DataTable("不合格品处理记录");
            da.Fill(dt不合格品处理记录);
        }

        void 不合格品记录Bind()
        {
            dataGridView4.DataSource = dt不合格品处理记录;
            dataGridView4.DataBindingComplete += dataGridView4_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dataGridView4);
        }

        void dataGridView4_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        private void btn读取不合格品记录_Click(object sender, EventArgs e)
        {
            read不合格品记录Data();
            不合格品记录Bind();
        }

        private void btn读取验收记录_Click(object sender, EventArgs e)
        {
            read物资验收记录Data();
            物资验收记录Bind();
        }

        void read取样记录Data()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 取样记录 where 审核员='__待审核'", mySystem.Parameter.conn);
            dt取样记录 = new DataTable("取样记录");
            da.Fill(dt取样记录);
        }

        void 取样记录Bind()
        {
            dataGridView5.DataSource = dt取样记录;
            dataGridView5.DataBindingComplete += dataGridView5_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dataGridView5);
        }

        void dataGridView5_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        private void btn读取取样记录_Click(object sender, EventArgs e)
        {
            read取样记录Data();
            取样记录Bind();
        }

        private void button查询_Click(object sender, EventArgs e)
        {
            search();
        }

        public void search()
        {
            //MessageBox.Show(tabControl1.SelectedIndex.ToString()+"\n"+comboBox审核状态.Text);
            String shr = comboBox审核状态.Text;
            DateTime startT = dateTimePicker开始.Value.Date;
            DateTime endT = dateTimePicker结束.Value.AddDays(1);
            SqlDataAdapter da;
            String sql;
            switch (tabControl1.SelectedIndex)
            {
                case 0:// 到货单
                    if (shr != "")
                    {
                        sql = @"select * from 到货单 where 审核员 like '%{0}%' and 日期 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 到货单 where 日期 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt到货单 = new DataTable("到货单");
                    da.Fill(dt到货单);
                    到货单Bind();
                    break;
                case 1: // 验收记录
                    if (shr != "")
                    {
                        sql = @"select * from 物资验收记录 where 审核员 like '%{0}%' and 接收时间 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 物资验收记录 where 接收时间 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt物资验收记录 = new DataTable("物资验收记录");
                    da.Fill(dt物资验收记录);
                    物资验收记录Bind();
                    break;
                case 2: //入库单
                    if (shr != "")
                    {
                        sql = @"select * from 入库单 where 审核员 like '%{0}%' and 入库日期 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 入库单 where 入库日期 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt入库单 = new DataTable("入库单");
                    da.Fill(dt入库单);
                    入库单Bind();
                    break;
                case 3: // 请验记录
                    if (shr != "")
                    {
                        sql = @"select * from 物资请验单 where 审核员 like '%{0}%' and 请验时间 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 物资请验单 where 请验时间 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt物资请验单 = new DataTable("物资请验单");
                    da.Fill(dt物资请验单);
                    物资请验单Bind();
                    break;
                case 6: // 复验记录
                    if (shr != "")
                    {
                        sql = @"select * from 复验记录 where 审核员 like '%{0}%' and 检验日期 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 复验记录 where 检验日期 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt复验记录 = new DataTable("复验记录");
                    da.Fill(dt复验记录);
                    复验记录Bind();
                    break;
                case 7: // 不合格品记录
                    //MessageBox.Show("该表格数据项太多，不知道以哪几个为依据查询");

                    if (shr != "")
                    {
                        sql = @"select * from 不合格品处理记录 where 审核员 like '%{0}%' and 不合格项描述填写日期 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 不合格品处理记录 where 不合格项描述填写日期 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt不合格品处理记录 = new DataTable("不合格品处理记录");
                    da.Fill(dt不合格品处理记录);
                    不合格品记录Bind();
                    break;
                case 4: // 取样记录
                    if (shr != "")
                    {
                        sql = @"select * from 取样记录 where 审核员 like '%{0}%'";
                        da = new SqlDataAdapter(string.Format(sql, shr), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 取样记录";
                        da = new SqlDataAdapter(sql, mySystem.Parameter.conn);
                    }
                    dt取样记录 = new DataTable("取样记录");
                    da.Fill(dt取样记录);
                    取样记录Bind();
                    break;
                case 5://检验
                    if (shr != "")
                    {
                        sql = @"select * from 检验记录 where 审核员 like '%{0}%' and 检验日期 between '{1}' and '{2}'";
                        da = new SqlDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.conn);
                    }
                    else
                    {
                        sql = @"select * from 检验记录 where 检验日期 between '{0}' and '{1}'";
                        da = new SqlDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.conn);
                    }
                    dt检验记录 = new DataTable("检验记录");
                    da.Fill(dt检验记录);
                    检验记录Bind();
                    break;
            }
        }

        private void btn到货单增加记录_Click(object sender, EventArgs e)
        {
            到货单 form = new 到货单(mainform);
            form.Show();
        }

        private void btn到货单读取_Click(object sender, EventArgs e)
        {
            read到货单Data();
            到货单Bind();
        }

        void read到货单Data()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from 到货单 where 审核员='__待审核'", mySystem.Parameter.conn);
            dt到货单 = new DataTable("到货单");
            da.Fill(dt到货单);
        }

        private void setDataGridViewBackColor(DataGridView dgv, String checker)
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

        private void setDGVColor(object sender, String checker)
        {
            DataGridView dgv = sender as DataGridView;
            setDataGridViewBackColor(dgv, checker);
        }

        void 到货单Bind()
        {
            dgv到货单.DataSource = dt到货单;
            dgv到货单.DataBindingComplete += dgv到货单_DataBindingComplete;
            Utility.setDataGridViewAutoSizeMode(dgv到货单);
        }

        void dgv到货单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGVColor(sender, "审核员");
        }

        private void btn读取入库单_Click(object sender, EventArgs e)
        {
            read入库单Data();
            入库单Bind();
        }

        private void btn读取检验记录_Click_1(object sender, EventArgs e)
        {
            read检验记录Data();
            检验记录Bind();
        }


    }
}
