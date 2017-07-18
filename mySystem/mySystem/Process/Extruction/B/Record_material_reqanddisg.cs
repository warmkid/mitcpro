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

namespace mySystem.Extruction.Process
{
    public partial class Record_material_reqanddisg : mySystem.BaseForm
    {
        private int instrid;
        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        CheckForm checkform;
        //通过原料代码查找原料id
        private int id_findby_matcode(string matcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select raw_material_id from raw_material where raw_material_code='" + matcode + "'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return int.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        public Record_material_reqanddisg(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();            
            init();
            addmatcode();
        }

        public Record_material_reqanddisg(MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            init();
            //addmatcode();
            string asql = "select * from 吹膜工序领料退料记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            cB物料代码.Text = tempdt.Rows[0]["物料代码"].ToString();
            readOuterData(instrid, tempdt.Rows[0]["物料代码"].ToString());
            removeOuterBinding();
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;

        }

        //供界面显示,参数为数据库中对应记录的id
        public void show(int paraid)
        {
        }
        //向combox中添加物料代码
        private void addmatcode()
        {
            cB物料代码.Items.Add("SMP-PE-01");
            cB物料代码.Items.Add("SMP-PE-04");
            cB物料代码.Items.Add("SMP-PE-06");
            cB物料代码.Items.Add("SMP-PE-11");
        }

        private void init()
        {
            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            instrid = mySystem.Parameter.proInstruID;
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            cB物料代码.Enabled = true;
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        //根据筛选条件查找id
        private int getid(int instrid,string matcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 吹膜工序领料退料记录 where 生产指令ID=" + instrid + " and 物料代码='"+matcode+"'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return (int)tempdt.Rows[0][0];
            }
        }

        //检查输入人是否合法
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

        private void button1_Click(object sender, EventArgs e)
        {
            //判断合法性
            if (!input_Judge())
                return;
            
            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(instrid,cB物料代码.Text);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();
            bt退料审核.Enabled = true;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region 之前
            //int row = e.RowIndex, col = e.ColumnIndex;
            //if (row == -1)
            //    return;
            //if (row == dataGridView1.Rows.Count - 1)
            //    addblankrow();
            //if (checkBox1.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_1.Count)
            //    {
            //        //更新数据
            //        addtolist(list_1, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_1.Add(con);
            //        addtolist(list_1, row, col);
                    
            //    }
            //    return;
            //}

            //if (checkBox2.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_4.Count)
            //    {
            //        //更新数据
            //        addtolist(list_4, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_4.Add(con);
            //        addtolist(list_4, row, col);

            //    }
            //    return;
            //}

            //if (checkBox3.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_6.Count)
            //    {
            //        //更新数据
            //        addtolist(list_6, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_6.Add(con);
            //        addtolist(list_6, row, col);

            //    }
            //    return;
            //}

            //if (checkBox4.Checked)
            //{
            //    //int row = e.RowIndex, col = e.ColumnIndex;
            //    //if (row == -1)
            //    //    return;
            //    if (row < list_11.Count)
            //    {
            //        //更新数据
            //        addtolist(list_11, row, col);
            //    }
            //    else
            //    {
            //        cont con = new cont();
            //        list_11.Add(con);
            //        addtolist(list_11, row, col);

            //    }
            //    return;
            //}
            #endregion
        }
        private void dataGridView1_Endedit(object sender, DataGridViewCellEventArgs e)
        {
            //重量计算
            if (e.ColumnIndex == 4)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                float b = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * b;
            }
            //操作人
            if (e.ColumnIndex == 8)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[8].Value == null || dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString() == "")
                    return;
                int rt = mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("操作人ID不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[8].Value = "";
                }
                return;
            }
            //审核人
            if (e.ColumnIndex == 9)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[9].Value == null || dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
                    return;
                int rt = mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
                if (rt <= 0)
                {
                    MessageBox.Show("审核人ID不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[9].Value = "";
                }
                return;
            }
            //计算合计
            float sum_num = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                    sum_num += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            //tb重量.Text = sum_num.ToString();
            //tb数量.Text = sum_weight.ToString();
            dt_prodinstr.Rows[0]["重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["数量合计"] = sum_num;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //添加行
        private void button3_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        //审核
        public override void CheckResult()
        {
            base.CheckResult();
            dt_prodinstr.Rows[0]["退料审核人"] = checkform.userName;//退料审核人
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            if (checkform.ischeckOk)
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                cB物料代码.Enabled = true;
                bt打印.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();

        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = instrid;
            dr["物料代码"] = cB物料代码.Text;
            dr["领料日期"] = DateTime.Now;
            dr["审核是否通过"] = false;
            dr["重量合计"] = 0;
            dr["数量合计"] = 0;
            dr["退料"] = 0;
            dr["退料操作人"] = mySystem.Parameter.userName;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T吹膜工序领料退料记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["领料日期"] = DateTime.Now;
            dr["数量"] = 0;
            dr["重量每件"] = 0;
            dr["重量"] = 0;
            dr["包装完好"] = "是";
            dr["清洁合格"] = "合格";
            dr["操作人"] = mySystem.Parameter.userName;
            //dr["包装完好_是"] = true;
            //dr["包装完好_否"] = false;
            //dr["清洁合格_是"] = true;
            //dr["清洁合格_否"] = false;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid,string matcode)
        {
            dt_prodinstr = new DataTable("吹膜工序领料退料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where 生产指令ID=" + instrid+" and 物料代码='"+ matcode +"'", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜工序领料详细记录");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜工序领料详细记录 where T吹膜工序领料退料记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb重量.DataBindings.Clear();
            tb数量.DataBindings.Clear();
            tb退料量.DataBindings.Clear();
            tb退料操作人.DataBindings.Clear();
            tb退料审核人.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            tb重量.DataBindings.Add("Text", bs_prodinstr.DataSource, "重量合计");
            tb数量.DataBindings.Add("Text", bs_prodinstr.DataSource, "数量合计");
            tb退料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料");
            tb退料操作人.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料操作人");
            tb退料审核人.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料审核人");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            while (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            }
            setDataGridViewColumns();

            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewRowNums();
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "包装完好":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c1.Items.Add("是");
                        c1.Items.Add("否");
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    case "清洁合格":
                        DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = dc.ColumnName;
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.Automatic;
                        c3.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        c3.Items.Add("合格");
                        c3.Items.Add("不合格");
                        dataGridView1.Columns.Add(c3);
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
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}

            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜工序领料退料记录ID
            dataGridView1.Columns[5].ReadOnly = true;//重量
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[8].Value == null || dataGridView1.Rows[i].Cells[8].Value.ToString() == "")
                {
                    MessageBox.Show("操作人不能为空");
                    return false;
                }
                if (dataGridView1.Rows[i].Cells[9].Value == null || dataGridView1.Rows[i].Cells[9].Value.ToString() == "")
                {
                    MessageBox.Show("审核人不能为空");
                    return false;
                }
                    
            }
            return true;
        }
        private void cB物料代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            bt退料审核.Enabled = false;
            bt打印.Enabled = false;

            readOuterData(instrid,cB物料代码.Text);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(instrid, cB物料代码.Text);
                removeOuterBinding();
                outerBind();
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                cB物料代码.Enabled = true;
                bt打印.Enabled = true;
            }
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            print(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            (new Record_material_reqanddisg(mainform, 4)).Show();
        }

        public void print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/B/SOP-MFG-301-R14 吹膜工序领料退料记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
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
            my.Cells[3, 1].Value = "物料代码："+cB物料代码.Text;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime tempdt = DateTime.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                my.Cells[5 + i, 1] = tempdt.ToLongDateString();
                my.Cells[5 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[5 + i, 3] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[5 + i, 4] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                my.Cells[5 + i, 5] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[5 + i, 6] = dataGridView1.Rows[i].Cells[7].Value.ToString();
                my.Cells[5 + i, 7] = dataGridView1.Rows[i].Cells[8].Value.ToString();
                my.Cells[5 + i, 8] = dataGridView1.Rows[i].Cells[9].Value.ToString();
            }

            my.Cells[29, 2].Value = tb数量.Text;
            my.Cells[29, 4].Value = tb重量.Text;
            my.Cells[29, 5].Value = "退料："+tb退料量.Text;
            my.Cells[29, 7].Value = tb退料操作人.Text;
            my.Cells[29, 8].Value = tb退料审核人.Text;
        }
    }
}
