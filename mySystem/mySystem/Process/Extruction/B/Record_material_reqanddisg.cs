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
        private void init()
        {
            list_1 = new List<cont>();
            list_4 = new List<cont>();
            list_6 = new List<cont>();
            list_11 = new List<cont>();

            checkBox1.Checked = true;
            addblankrow();
            label = 1;
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

        //保存领料退料数据到数据库中
        private void save_to_database(List<cont> list)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //switch (label)
            //{
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //    case 3:
            //        break;
            //    case 4:
            //        break;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                label = 1;
            }
            //清空表格
            while (dataGridView1.Rows.Count != 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //填充数据
            for (int i = 0; i < list_1.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = list_1[i].date;
                dr.Cells[1].Value = list_1[i].num;
                dr.Cells[2].Value = list_1[i].weight;
                dr.Cells[3].Value = list_1[i].numperw;
                dr.Cells[4].Value = list_1[i].ispatch;
                dr.Cells[5].Value = list_1[i].isclean;
                dr.Cells[6].Value = list_1[i].oper;
                dr.Cells[7].Value = list_1[i].checker;

                dataGridView1.Rows.Add(dr);
            }
            addblankrow();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                label = 2;
            }
            //清空表格
            while (dataGridView1.Rows.Count != 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //填充数据
            for (int i = 0; i < list_4.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = list_4[i].date;
                dr.Cells[1].Value = list_4[i].num;
                dr.Cells[2].Value = list_4[i].weight;
                dr.Cells[3].Value = list_4[i].numperw;
                dr.Cells[4].Value = list_4[i].ispatch;
                dr.Cells[5].Value = list_4[i].isclean;
                dr.Cells[6].Value = list_4[i].oper;
                dr.Cells[7].Value = list_4[i].checker;

                dataGridView1.Rows.Add(dr);
            }
            addblankrow();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                label = 3;
            }
            //清空表格
            while (dataGridView1.Rows.Count != 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //填充数据
            for (int i = 0; i < list_6.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = list_6[i].date;
                dr.Cells[1].Value = list_6[i].num;
                dr.Cells[2].Value = list_6[i].weight;
                dr.Cells[3].Value = list_6[i].numperw;
                dr.Cells[4].Value = list_6[i].ispatch;
                dr.Cells[5].Value = list_6[i].isclean;
                dr.Cells[6].Value = list_6[i].oper;
                dr.Cells[7].Value = list_6[i].checker;

                dataGridView1.Rows.Add(dr);
            }
            addblankrow();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                label = 4;
            }
            //清空表格
            while (dataGridView1.Rows.Count != 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            //填充数据
            for (int i = 0; i < list_11.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = list_11[i].date;
                dr.Cells[1].Value = list_11[i].num;
                dr.Cells[2].Value = list_11[i].weight;
                dr.Cells[3].Value = list_11[i].numperw;
                dr.Cells[4].Value = list_11[i].ispatch>0;
                dr.Cells[5].Value = list_11[i].isclean>0;
                dr.Cells[6].Value = list_11[i].oper;
                dr.Cells[7].Value = list_11[i].checker;

                dataGridView1.Rows.Add(dr);
            }
            addblankrow();
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex, col = e.ColumnIndex;
            if (row == -1)
                return;
            if (row == dataGridView1.Rows.Count - 1)
                addblankrow();
            if (checkBox1.Checked)
            {
                //int row = e.RowIndex, col = e.ColumnIndex;
                //if (row == -1)
                //    return;
                if (row < list_1.Count)
                {
                    //更新数据
                    addtolist(list_1, row, col);
                }
                else
                {
                    cont con = new cont();
                    list_1.Add(con);
                    addtolist(list_1, row, col);
                    
                }
                return;
            }

            if (checkBox2.Checked)
            {
                //int row = e.RowIndex, col = e.ColumnIndex;
                //if (row == -1)
                //    return;
                if (row < list_4.Count)
                {
                    //更新数据
                    addtolist(list_4, row, col);
                }
                else
                {
                    cont con = new cont();
                    list_4.Add(con);
                    addtolist(list_4, row, col);

                }
                return;
            }

            if (checkBox3.Checked)
            {
                //int row = e.RowIndex, col = e.ColumnIndex;
                //if (row == -1)
                //    return;
                if (row < list_6.Count)
                {
                    //更新数据
                    addtolist(list_6, row, col);
                }
                else
                {
                    cont con = new cont();
                    list_6.Add(con);
                    addtolist(list_6, row, col);

                }
                return;
            }

            if (checkBox4.Checked)
            {
                //int row = e.RowIndex, col = e.ColumnIndex;
                //if (row == -1)
                //    return;
                if (row < list_11.Count)
                {
                    //更新数据
                    addtolist(list_11, row, col);
                }
                else
                {
                    cont con = new cont();
                    list_11.Add(con);
                    addtolist(list_11, row, col);

                }
                return;
            }
            
            
        }

        private void addtolist(List<cont> list, int row, int col)
        {
            switch (col)
            {
                case 0:
                    list[row].date = DateTime.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
                    break;
                case 1:
                    list[row].num = float.Parse(dataGridView1.Rows[row].Cells[1].Value.ToString());
                    break;
                case 2:
                    list[row].weight = float.Parse(dataGridView1.Rows[row].Cells[2].Value.ToString());
                    break;
                case 3:
                    list[row].numperw = float.Parse(dataGridView1.Rows[row].Cells[3].Value.ToString());
                    break;
                case 4:
                    if (dataGridView1.Rows[row].Cells[4].EditedFormattedValue.ToString()=="True")
                        list[row].ispatch = 1;
                    else
                        list[row].ispatch = 0;
                    break;
                case 5:
                    if (dataGridView1.Rows[row].Cells[5].EditedFormattedValue.ToString() == "True")
                        list[row].isclean = 1;
                    else
                        list[row].isclean = 0;
                    break;
                case 6:
                    list[row].oper = dataGridView1.Rows[row].Cells[6].Value.ToString();
                    break;
                case 7:
                    list[row].checker = dataGridView1.Rows[row].Cells[7].Value.ToString();
                    break;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Record_material_reqanddisg_Load(object sender, EventArgs e)
        {

        }
    }
}
