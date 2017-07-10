using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Extruction.Process
{
    public partial class Record_material_reqanddisg : mySystem.BaseForm
    {
        List<cont> list_1=null;
        List<cont> list_4 = null;
        List<cont> list_6 = null;
        List<cont> list_11 = null;
        int label;//标记哪个物料被选中
        public class cont
        {
            public DateTime date;
            public float num;
            public float weight;
            public float numperw;
            public int ispatch;
            public int isclean;
            public string oper;
            public string checker;
        }

        int id;//外键
        int instrid;
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
        }

        //供界面显示,参数为数据库中对应记录的id
        public void show(int paraid)
        {
            bind(paraid);
            bind_list(paraid);
        }
        private void init()
        {
            list_1 = new List<cont>();
            list_4 = new List<cont>();
            list_6 = new List<cont>();
            list_11 = new List<cont>();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            instrid = mySystem.Parameter.proInstruID;
            checkBox1.Checked = true;
            //addblankrow();
            label = 1;

            int tempid = getid(instrid, "SPM-PE-01");
            if ( tempid== -1)
            {
                bind_insert(instrid, "SPM-PE-01");
                dt_prodinstr.Rows[0][1] = instrid;
                dt_prodinstr.Rows[0][10] = mySystem.Parameter.userName;//退料操作人
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();
                bind_list(id);
            }
            else
            {
                bind(tempid);
                bind_list(tempid);
            }
        
        }

        private void bind_insert(int instrid,string matcode)//插入模式
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("吹膜工序领料退料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where 1=2", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            DataRow dr = dt_prodinstr.NewRow();
            dr[1] = instrid;
            dr[2] = matcode;//物料代码
            dr[4] = dr[5] = dr[10] = dr[11] = "";
            dr[3] = DateTime.Now;
            dr[1] = dr[7] = dr[8] = dr[9] = 0;
            dr[6] = false;
            dt_prodinstr.Rows.Add(dr);

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            textBox5.DataBindings.Clear();

            //BindingSource到控件的绑定
            textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "重量合计");
            textBox2.DataBindings.Add("Text", bs_prodinstr.DataSource, "数量合计");
            textBox3.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料");
            textBox4.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料操作人");
            textBox5.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料审核人");
        }
        private void bind(int id)
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("吹膜工序领料退料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where ID="+id, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            //if (dt_prodinstr.Rows.Count == 0)
            //{
            //    DataRow dr = dt_prodinstr.NewRow();
            //    dr[2] = dr[4] = dr[5] = dr[10] = dr[11] = "";
            //    dr[3] = DateTime.Now;
            //    dr[1] = dr[7] = dr[8] = dr[9] = 0;
            //    dr[6] = false;
            //    dt_prodinstr.Rows.Add(dr);
            //}

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            textBox5.DataBindings.Clear();

            //BindingSource到控件的绑定
            textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "重量合计");
            textBox2.DataBindings.Add("Text", bs_prodinstr.DataSource, "数量合计");
            textBox3.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料");
            textBox4.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料操作人");
            textBox5.DataBindings.Add("Text", bs_prodinstr.DataSource, "退料审核人");
        }
        private void bind_list(int id)
        {
            dt_prodlist.Dispose();
            bs_prodlist.Dispose();
            da_prodlist.Dispose();
            cb_prodlist.Dispose();

            dt_prodlist = new DataTable("吹膜工序领料详细记录");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜工序领料详细记录 where T吹膜工序领料退料记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            //DataTable到BindingSource的绑定
            bs_prodlist.DataSource = dt_prodlist;

            //BindingSource到控件的绑定
            dataGridView1.DataSource = bs_prodlist.DataSource;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        //添加空行
        private void addblankrow()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dataGridView1.Rows.Add(dr);
        }

        private int getid()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";
            return (int)comm.ExecuteScalar();
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
        //保存领料退料数据到数据库中
        private void save_to_database(List<cont> list)
        {
            
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
            //判断输入合法性
            if (queryid(textBox4.Text) == -1)
            {
                MessageBox.Show("退料操作人不合法");
                return;
            }

            if (textBox1.Text == "")
            {
                textBox1.Text = "0";
            }
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
            }
            if (textBox3.Text == "")
            {
                textBox3.Text = "0";
            }
            string s1 = textBox1.Text;
            string s2 = textBox2.Text;
            string s3 = textBox3.Text;
            string s4 = textBox4.Text;
            string s5 = textBox5.Text;

            dt_prodinstr.Rows[0][1] = instrid;
            dt_prodinstr.Rows[0][7] = float.Parse(s1);
            dt_prodinstr.Rows[0][8] = float.Parse(s2);
            dt_prodinstr.Rows[0][9] = float.Parse(s3);
            dt_prodinstr.Rows[0][10] = s4;
            dt_prodinstr.Rows[0][11] = s5;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            MessageBox.Show("添加成功");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    checkBox2.Checked = false;
            //    checkBox3.Checked = false;
            //    checkBox4.Checked = false;
            //    label = 1;
            //}
            ////清空表格
            //while (dataGridView1.Rows.Count != 0)
            //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            ////填充数据
            //for (int i = 0; i < list_1.Count; i++)
            //{
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = list_1[i].date;
            //    dr.Cells[1].Value = list_1[i].num;
            //    dr.Cells[2].Value = list_1[i].weight;
            //    dr.Cells[3].Value = list_1[i].numperw;
            //    dr.Cells[4].Value = list_1[i].ispatch;
            //    dr.Cells[5].Value = list_1[i].isclean;
            //    dr.Cells[6].Value = list_1[i].oper;
            //    dr.Cells[7].Value = list_1[i].checker;

            //    dataGridView1.Rows.Add(dr);
            //}
            //addblankrow();

            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                label = 1;
            }
            else
                return;
            //绑定1
            id = getid(instrid, "SPM-PE-01");
            if (id == -1)//没找到就进行插入
            {
                bind_insert(instrid,"SPM-PE-01");
                dt_prodinstr.Rows[0][1] = instrid;
                dt_prodinstr.Rows[0][2] = "SPM-PE-01";
                dt_prodinstr.Rows[0][10] = mySystem.Parameter.userName;//退料操作人
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();
                bind_list(id);
            }
            else
            {
                bind(id);
                bind_list(id);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked)
            //{
            //    checkBox1.Checked = false;
            //    checkBox3.Checked = false;
            //    checkBox4.Checked = false;
            //    label = 2;
            //}
            ////清空表格
            //while (dataGridView1.Rows.Count != 0)
            //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            ////填充数据
            //for (int i = 0; i < list_4.Count; i++)
            //{
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = list_4[i].date;
            //    dr.Cells[1].Value = list_4[i].num;
            //    dr.Cells[2].Value = list_4[i].weight;
            //    dr.Cells[3].Value = list_4[i].numperw;
            //    dr.Cells[4].Value = list_4[i].ispatch;
            //    dr.Cells[5].Value = list_4[i].isclean;
            //    dr.Cells[6].Value = list_4[i].oper;
            //    dr.Cells[7].Value = list_4[i].checker;

            //    dataGridView1.Rows.Add(dr);
            //}
            //addblankrow();

            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                label = 2;
            }
            else
                return;
            //绑定1
            id = getid(instrid, "SPM-PE-04");
            if (id == -1)//没找到就进行插入
            {
                bind_insert(instrid, "SPM-PE-04");
                dt_prodinstr.Rows[0][1] = instrid;
                dt_prodinstr.Rows[0][2] = "SPM-PE-04";
                dt_prodinstr.Rows[0][10] = mySystem.Parameter.userName;//退料操作人
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();
                bind_list(id);
            }
            else
            {
                bind(id);
                bind_list(id);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox3.Checked)
            //{
            //    checkBox2.Checked = false;
            //    checkBox1.Checked = false;
            //    checkBox4.Checked = false;
            //    label = 3;
            //}
            ////清空表格
            //while (dataGridView1.Rows.Count != 0)
            //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            ////填充数据
            //for (int i = 0; i < list_6.Count; i++)
            //{
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = list_6[i].date;
            //    dr.Cells[1].Value = list_6[i].num;
            //    dr.Cells[2].Value = list_6[i].weight;
            //    dr.Cells[3].Value = list_6[i].numperw;
            //    dr.Cells[4].Value = list_6[i].ispatch;
            //    dr.Cells[5].Value = list_6[i].isclean;
            //    dr.Cells[6].Value = list_6[i].oper;
            //    dr.Cells[7].Value = list_6[i].checker;

            //    dataGridView1.Rows.Add(dr);
            //}
            //addblankrow();

            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                label = 3;
            }
            else
                return;
            //绑定1
            id = getid(instrid, "SPM-PE-06");
            if (id == -1)//没找到就进行插入
            {
                bind_insert(instrid, "SPM-PE-06");
                dt_prodinstr.Rows[0][1] = instrid;
                dt_prodinstr.Rows[0][2] = "SPM-PE-06";
                dt_prodinstr.Rows[0][10] = mySystem.Parameter.userName;//退料操作人
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();
                bind_list(id);
            }
            else
            {
                bind(id);
                bind_list(id);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox4.Checked)
            //{
            //    checkBox2.Checked = false;
            //    checkBox3.Checked = false;
            //    checkBox1.Checked = false;
            //    label = 4;
            //}
            ////清空表格
            //while (dataGridView1.Rows.Count != 0)
            //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            ////填充数据
            //for (int i = 0; i < list_11.Count; i++)
            //{
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = list_11[i].date;
            //    dr.Cells[1].Value = list_11[i].num;
            //    dr.Cells[2].Value = list_11[i].weight;
            //    dr.Cells[3].Value = list_11[i].numperw;
            //    dr.Cells[4].Value = list_11[i].ispatch>0;
            //    dr.Cells[5].Value = list_11[i].isclean>0;
            //    dr.Cells[6].Value = list_11[i].oper;
            //    dr.Cells[7].Value = list_11[i].checker;

            //    dataGridView1.Rows.Add(dr);
            //}
            //addblankrow();

            if (checkBox4.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                label = 4;
            }
            else
                return;
            //绑定1
            id = getid(instrid, "SPM-PE-011");
            if (id == -1)//没找到就进行插入
            {
                bind_insert(instrid, "SPM-PE-011");
                dt_prodinstr.Rows[0][1] = instrid;
                dt_prodinstr.Rows[0][2] = "SPM-PE-011";
                dt_prodinstr.Rows[0][10] = mySystem.Parameter.userName;//退料操作人
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();
                bind_list(id);
            }
            else
            {
                bind(id);
                bind_list(id);
            }
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
            //计算合计
            float sum_num = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                    sum_num += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            textBox1.Text = sum_num.ToString();
            textBox2.Text = sum_weight.ToString();
        }

        private void addtolist(List<cont> list, int row, int col)
        {
            //switch (col)
            //{
            //    case 0:
            //        list[row].date = DateTime.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            //        break;
            //    case 1:
            //        list[row].num = float.Parse(dataGridView1.Rows[row].Cells[1].Value.ToString());
            //        break;
            //    case 2:
            //        list[row].weight = float.Parse(dataGridView1.Rows[row].Cells[2].Value.ToString());
            //        break;
            //    case 3:
            //        list[row].numperw = float.Parse(dataGridView1.Rows[row].Cells[3].Value.ToString());
            //        break;
            //    case 4:
            //        if (dataGridView1.Rows[row].Cells[4].EditedFormattedValue.ToString()=="True")
            //            list[row].ispatch = 1;
            //        else
            //            list[row].ispatch = 0;
            //        break;
            //    case 5:
            //        if (dataGridView1.Rows[row].Cells[5].EditedFormattedValue.ToString() == "True")
            //            list[row].isclean = 1;
            //        else
            //            list[row].isclean = 0;
            //        break;
            //    case 6:
            //        list[row].oper = dataGridView1.Rows[row].Cells[6].Value.ToString();
            //        break;
            //    case 7:
            //        list[row].checker = dataGridView1.Rows[row].Cells[7].Value.ToString();
            //        break;
            //}
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Record_material_reqanddisg_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr[1] = id;
            dt_prodlist.Rows.InsertAt(dr, dt_prodlist.Rows.Count);
        }

        //审核
        public override void CheckResult()
        {
            base.CheckResult();
            dt_prodinstr.Rows[0][11] = checkform.userName;//退料审核人
            dt_prodinstr.Rows[0][5] = checkform.opinion;
            dt_prodinstr.Rows[0][6] = checkform.ischeckOk;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();

        }
    }
}
