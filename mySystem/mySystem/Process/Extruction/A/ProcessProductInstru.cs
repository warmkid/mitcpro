﻿using System;
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

namespace BatchProductRecord
{
    public partial class ProcessProductInstru : mySystem.BaseForm
    {
        DataTable dt=null;//存储从产品表中读到的产品代码
        public ProcessProductInstru(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            init();
            addrows();
        }
        private void init()
        {
            textBox4.Text = "AA-EQM-032";
            button2.Enabled = false;

            //从产品表中读数据填入产品代码下拉列表中
            if (mainform.isSqlOk)
            {
            }
            else
            {
                string asql = "select product_code from product_aoxing";
                OleDbCommand comm = new OleDbCommand(asql, mainform.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                comm.Dispose();
                
            }
        }
        //添加编辑行
        private void addrows()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = dataGridView1.Rows.Count+1;
            dataGridView1.Rows.Add(dr);
            DataGridViewComboBoxCell combox = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1] as DataGridViewComboBoxCell;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                combox.Items.Add(dt.Rows[i][0]);
            }
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProcessProductInstru_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mySystem.CheckForm checkform = new mySystem.CheckForm(mainform);
            checkform.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            float in_amout;
            float mid_amout;
            float juan_amout;
            float extruproc_label_amout;
            float doubclean_amout;

            if (!float.TryParse(textBox21.Text, out  in_amout) || !float.TryParse(textBox22.Text, out mid_amout) || !float.TryParse(textBox14.Text, out juan_amout) || !float.TryParse(textBox10.Text, out extruproc_label_amout) || !float.TryParse(textBox11.Text, out doubclean_amout))
            {
                MessageBox.Show("领料量必须为数值类型");
                return;
            }

            string prodname = textBox1.Text;
            string instrcode = textBox2.Text;
            string art = textBox3.Text;
            string number = textBox4.Text;
            DateTime d = dateTimePicker1.Value;
            string in_matcode = textBox15.Text;//内外层物料代码
            string in_matbatch = textBox17.Text;
            string in_format = textBox19.Text;

            string mid_matcode = textBox16.Text;//中层物料代码
            string mid_matbatch = textBox18.Text;
            string mid_format = textBox20.Text;

            string juan_extr=textBox12.Text;
            string juan_format = textBox13.Text;

            string extruproc_label_format = textBox8.Text;
            string doubclean_format = textBox9.Text;

            string chargeman = textBox5.Text;
            string extra = textBox23.Text;
            string compman = textBox24.Text;//编制人
            DateTime compdate = dateTimePicker2.Value;
            string checkman = textBox25.Text;//审批人
            DateTime checkdate = dateTimePicker3.Value;
            string recman = textBox26.Text;//接收人
            DateTime recdate = dateTimePicker4.Value;

            button2.Enabled = true;
            //jason格式产品代码
            JArray ret = JArray.Parse("[]");
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) //最后一行不添加
            {
                string st = "{'";
                string t = dataGridView1.Rows[i].Cells[1].Value.ToString() + "':";
                st += t;
                st += "[" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[5].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[7].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[8].Value.ToString() + "']}";

                JObject temp = JObject.Parse(st);
                ret.Add(temp);
            }
            System.Console.WriteLine(ret.ToString());

            if (mainform.isSqlOk)
            {
            }
            else
            {
                //int result = 0;
                //OleDbCommand comm = new OleDbCommand();
                //comm.Connection = mainform.connOle;
                //comm.CommandText = "insert into production_instruction set product_name=@name,production_instruction_code= @instrcode, where id= @id";
                //comm.Parameters.Add("@cleandate", .Data.OleDb.OleDbType.Date);
                //comm.Parameters.Add("@flight", System.Data.OleDb.OleDbType.Integer);
                //comm.Parameters.Add("@reviewerid", System.Data.OleDb.OleDbType.Integer);
                //comm.Parameters.Add("@reviewdate", System.Data.OleDb.OleDbType.Date);
                //comm.Parameters.Add("@cont", System.Data.OleDb.OleDbType.VarChar);
                //comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

                //comm.Parameters["@cleandate"].Value = cleantime;
                //comm.Parameters["@flight"].Value = classid;
                //comm.Parameters["@reviewerid"].Value = checkerid;
                //comm.Parameters["@reviewdate"].Value = checktime;
                //comm.Parameters["@cont"].Value = jarray.ToString();
                //comm.Parameters["@id"].Value = 1;

                //result = comm.ExecuteNonQuery();
                //if (result > 0)
                //{
                //    MessageBox.Show("添加成功");
                //}
                //else { MessageBox.Show("错误"); }
            }
            button2.Enabled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
                addrows();
            ////更改清洁人项
            //if (e.ColumnIndex == 3)
            //{
            //    int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            //    if (rt > 0)
            //        cleanmans[e.RowIndex] = rt;
            //    else
            //        MessageBox.Show("清洁人id不存在，请重新输入");
            //    return;
            //}
            ////更改审核人项
            //if (e.ColumnIndex == 4)
            //{
            //    int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            //    if (rt > 0)
            //        checkmans[e.RowIndex] = rt;
            //    else
            //        MessageBox.Show("审核人id不存在，请重新输入");
            //    return;
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
