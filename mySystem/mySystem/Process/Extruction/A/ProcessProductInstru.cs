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
using System.Text.RegularExpressions;

namespace BatchProductRecord
{
    public partial class ProcessProductInstru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;
        DataTable dt=null;//存储从产品表中读到的产品代码
        string last_code;//最后一次保存数据库时生产指令编码,默认该表内不会重复
        float leng;
        float sumweight;

        public ProcessProductInstru(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            init();
            addrows();
        }
        private void init()
        {
            textBox4.Text = "AA-EQM-032";
            textBox24.Text = mySystem.Parameter.userName;
            button2.Enabled = false;
            sumweight = 0;

            //从产品表中读数据填入产品代码下拉列表中
            if (mainform.isSqlOk)
            {
            }
            else
            {
                string asql = "select product_code from product_aoxing";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
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
            //DataGridViewComboBoxCell combox = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1] as DataGridViewComboBoxCell;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    combox.Items.Add(dt.Rows[i][0]);
            //}
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
        public override void CheckResult()
        {
            base.CheckResult();
            //获得审核信息
            //string opinion = checkform.opinion;
            //bool isok = checkform.ischeckOk;
            textBox25.Text = name_findby_id(checkform.userID);
            dateTimePicker3.Value = checkform.time;

            //选择刚才的表中对应的记录，并更新里面的记录
            string asql = "select production_instruction_id from production_instruction where production_instruction_code='"+last_code+"'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                MessageBox.Show("对应的记录未找到");
                return;
            }
            int id = Int32.Parse(tempdt.Rows[0][0].ToString());
            comm.CommandText = "update production_instruction set reviewer_id=@id,review_date=@date,review_opinion=@opinion,is_review_qualified=@isok where production_instruction_id="+id;
            comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);
            comm.Parameters.Add("@date", System.Data.OleDb.OleDbType.Date);
            comm.Parameters.Add("@opinion", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@isok", System.Data.OleDb.OleDbType.Boolean);

            comm.Parameters["@id"].Value = checkform.userID;
            comm.Parameters["@date"].Value = checkform.time;
            comm.Parameters["@opinion"].Value = checkform.opinion;
            comm.Parameters["@isok"].Value = checkform.ischeckOk;
            int result = comm.ExecuteNonQuery();
            if (result<=0)
            {
                MessageBox.Show("添加错误");
            }


            da.Dispose();
            comm.Dispose();
            tempdt.Dispose();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            checkform = new mySystem.CheckForm(this);
            checkform.Show();  
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            float in_amout;
            float mid_amout;
            float juan_amout;
            float extruproc_label_amout;
            float doubclean_amout;

            if (!float.TryParse(textBox21.Text, out  in_amout) || !float.TryParse(textBox22.Text, out mid_amout) || !float.TryParse(textBox14.Text, out juan_amout))
            {
                MessageBox.Show("领料量必须为数值类型");
                return;
            }

            string prodname = textBox1.Text;
            string instrcode = textBox2.Text;
            if (instrcode == "")
            {
                MessageBox.Show("生产指令未填写");
                return;
            }
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
            string juan_quan = textBox14.Text;

            //string extruproc_label_format = textBox8.Text;
            //string extruproc_label_quan = textBox10.Text;
            string doubclean_format = textBox9.Text;//内包形式包装规格
            string doubclean_quan = textBox11.Text;//内包形式领料量

            string chargeman = textBox5.Text;
            int chargeman_id = id_findby_code(chargeman);
            if (chargeman_id == -1)
            {
                MessageBox.Show("负责人id不存在");
                return;
            }
                
            string extra = textBox23.Text;
            string compman = textBox24.Text;//编制人;
            //int compman_id = id_findby_code(compman);
            //if (compman_id == -1)
            //{
            //    MessageBox.Show("编制人id不存在");
            //    return;
            //}
                
            DateTime compdate = dateTimePicker2.Value;
            string checkman = textBox25.Text;//审批人
                
            DateTime checkdate = dateTimePicker3.Value;
            string recman = textBox26.Text;//接收人
            int recman_id = id_findby_code(recman);
            if (recman_id == -1)
            {
                MessageBox.Show("接受人id不存在");
                return;
            }
                
            DateTime recdate = dateTimePicker4.Value;

            //领料量是否合法
            sumweight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                sumweight += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            }
            if (in_amout < sumweight * 0.75 / 0.9)
            {
                MessageBox.Show("领料量小于供料重量，请重新填写");
                return;
            }


            //jason格式产品代码
            JArray ret = JArray.Parse("[]");
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) //最后一行不添加
            {
                string st = "{'";
                string t = dataGridView1.Rows[i].Cells[1].Value.ToString() + "':";
                st += t;
                st += "[" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[5].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[7].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[8].Value.ToString() +"','" + dataGridView1.Rows[i].Cells[9].Value.ToString()+ "']}";

                JObject temp = JObject.Parse(st);
                ret.Add(temp);
            }
            System.Console.WriteLine(ret.ToString());

            if (mainform.isSqlOk)
            {
            }
            else
            {
                int result = 0;
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;

//                comm.CommandText = "insert into production_instruction(product_name,production_instruction_code,production_process,machine,production_start_date,instruction_description,raw_material_id_in_out,raw_material_batch_in_out," +
//    "raw_material_id_middle,raw_material_batch_middle,package_specifications_in_out,package_specifications_middle," +
//    "receive_quantity_in_out,receive_quantity_middle,core_tube_parameter,core_tube_package_specifications,core_tube_receive_the_quantity_of_raw_material,package_specifications,package_receive_the_quantity_of_raw_material,package_specifications_inner,package_receive_the_quantity_of_raw_material_inner,extr," +
//    "principal_id,editor_id,reviewer_id,receiver_id,edit_date,review_date,receive_date)" +
//    " values(@name,@instrcode,@prodcess,@machine,@startdate,@desc,@inout_id,@inout_batch,@mid_id,@mid_batch,@inout_pac,@mid_pac," +
//"@inout_quan,@mid_quan,@tube_para,@tube_pac,@tube_quan,@pac_label,@quan_label,@pac_inner,@quan_inner,@extr,@princ_id,@editor_id,@rev_id,@rec_id,@editdate,@revdate,@recdate)";

                comm.CommandText = "insert into production_instruction(product_name,production_instruction_code,production_process,machine,production_start_date,instruction_description,raw_material_id_in_out,raw_material_batch_in_out," +
"raw_material_id_middle,raw_material_batch_middle,package_specifications_in_out,package_specifications_middle," +
"receive_quantity_in_out,receive_quantity_middle,core_tube_parameter,core_tube_package_specifications,core_tube_receive_the_quantity_of_raw_material,package_specifications,package_receive_the_quantity_of_raw_material,extr," +
"principal_id,editor_id,reviewer_id,receiver_id,edit_date,review_date,receive_date)" +
" values(@name,@instrcode,@prodcess,@machine,@startdate,@desc,@inout_id,@inout_batch,@mid_id,@mid_batch,@inout_pac,@mid_pac," +
"@inout_quan,@mid_quan,@tube_para,@tube_pac,@tube_quan,@pac_inner,@quan_inner,@extr,@princ_id,@editor_id,@rev_id,@rec_id,@editdate,@revdate,@recdate)";


                System.Console.WriteLine(comm.CommandText.ToString());
                comm.Parameters.Add("@name",System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@instrcode", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@prodcess", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@machine", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@startdate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@desc", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@inout_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@inout_batch", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@mid_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@mid_batch", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@inout_pac", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@mid_pac", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@inout_quan", System.Data.OleDb.OleDbType.Integer);//
                comm.Parameters.Add("@mid_quan", System.Data.OleDb.OleDbType.Integer);//
                comm.Parameters.Add("@tube_para", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@tube_pac", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@tube_quan", System.Data.OleDb.OleDbType.Integer);//
                //comm.Parameters.Add("@pac_label", System.Data.OleDb.OleDbType.VarChar);
                //comm.Parameters.Add("@quan_label", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@pac_inner", System.Data.OleDb.OleDbType.VarChar);//内包装 包装规格
                comm.Parameters.Add("@quan_inner", System.Data.OleDb.OleDbType.VarChar);//内包装 领量
                comm.Parameters.Add("@extr", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@princ_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@editor_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@rev_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@rec_id", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@editdate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@revdate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@recdate", System.Data.OleDb.OleDbType.Date);

                comm.Parameters["@name"].Value = prodname;
                comm.Parameters["@instrcode"].Value = instrcode;
                comm.Parameters["@prodcess"].Value = art;
                comm.Parameters["@machine"].Value = number;
                comm.Parameters["@startdate"].Value = d;
                comm.Parameters["@desc"].Value = ret.ToString();
                comm.Parameters["@inout_id"].Value =id_findby_code(ret.ToString());
                comm.Parameters["@inout_batch"].Value = in_matbatch;
                comm.Parameters["@mid_id"].Value = id_findby_code(mid_matcode);
                comm.Parameters["@mid_batch"].Value = mid_matbatch;
                comm.Parameters["@inout_pac"].Value = mid_format;
                comm.Parameters["@mid_pac"].Value = ret.ToString();
                comm.Parameters["@inout_quan"].Value =Convert.ToInt32(in_amout);//float to int
                comm.Parameters["@mid_quan"].Value =Convert.ToInt32(mid_amout);//float to int
                comm.Parameters["@tube_para"].Value = juan_extr;
                comm.Parameters["@tube_pac"].Value = juan_format;
                comm.Parameters["@tube_quan"].Value = juan_quan;
                //comm.Parameters["@pac_label"].Value = extruproc_label_format;
                //comm.Parameters["@quan_label"].Value = extruproc_label_quan;
                comm.Parameters["@pac_inner"].Value = doubclean_format;
                comm.Parameters["@quan_inner"].Value = doubclean_quan;
                comm.Parameters["@extr"].Value = extra;
                comm.Parameters["@princ_id"].Value = id_findby_code(chargeman);
                comm.Parameters["@editor_id"].Value = id_findby_code(compman);
                comm.Parameters["@rev_id"].Value = id_findby_code(checkman);
                comm.Parameters["@rec_id"].Value = id_findby_code(recman);
                comm.Parameters["@editdate"].Value = compdate;
                comm.Parameters["@revdate"].Value = checkdate;
                comm.Parameters["@recdate"].Value = recdate;

                //System.Console.WriteLine(comm.CommandText.ToString());

                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    last_code = instrcode;
                    MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }
            button2.Enabled = true;
        }
        private int id_findby_code(string code)
        {
            if (mainform.isSqlOk)
            {
                return -1;
            }
            else
            {
                string asql = "select user_id from user_aoxing where user_name=" + "'" + code + "'";
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        private int matid_findby_code(string code)
        {
            if (mainform.isSqlOk)
            {
                return -1;
            }
            else
            {
                string asql = "select raw_material_id from raw_material where raw_material_code=" + "'" + code + "'";
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        private string name_findby_id(int id)
        {
            if (mainform.isSqlOk)
            {
                return "";
            }
            else
            {
                string asql = "select user_name from user_aoxing where user_id=" +id;
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return "";
                else
                    return tempdt.Rows[0][0].ToString();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
                addrows();

            if (e.ColumnIndex == 1)
            {
                string str = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string pattern = @"^[a-zA-Z]+-[a-zA-Z]+-[0-9]+\*[0-9]";//正则表达式
                if (!Regex.IsMatch(str, pattern))
                {
                    MessageBox.Show("产品代码输入不符合规定，重新输入，例如 PEQ-QE-500*100");
                    //MessageBox.Show("...");
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = "";
                    leng = 0;
                    return;
                }
                string[] array = str.Split('*');
                string[] array2 = array[0].Split('-');
                leng = float.Parse(array2[2]);
            }
            //用料重量自己计算
            if(e.ColumnIndex==2)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[3].Value = a * leng/1000.0 * 2 * 0.093;
            }



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
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void fill(string prodinstr)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
