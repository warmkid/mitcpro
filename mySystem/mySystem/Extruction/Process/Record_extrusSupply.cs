using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusSupply : Form
    {
        //SqlConnection conn;
        private ExtructionProcess extructionformfather = null;

        string product_code;//产品代码
        string product_num;//产品批号
        string product_instrnum;//生产指令编号

        string bunker1;//料仓外中
        string bunker1_code;//
        string bunker1_batch;// 1 批号
        string bunker2;//料仓中层
        string bunker2_code;//
        string bunker2_batch;//2 批号

        DateTime date;//供料日期
        float serve_out;//外层供料量
        float serve_mid;//中层供料量
        float serve_in;//中内层供料量
        string checkout;
        string server;//供料人

        string bunker_use;//物料使用料仓
        float use1;//物料使用量
        float left1;//余料
        float use2;
        float left2;

        int checker;//复核人

        private void Init()
        {
            string strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();
            string s = "select product_id,product_batch,production_instruction,s5_feeding_info from extrusion where id=1";
            SqlCommand comm = new SqlCommand(s, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable dtemp = new DataTable();
            da.Fill(dtemp);
          


            conn.Close();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            product_code = dtemp.Rows[0][0].ToString();
            product_num = dtemp.Rows[0][1].ToString();
            product_instrnum = dtemp.Rows[0][2].ToString();

            //解析jason
            JArray jo = JArray.Parse(dtemp.Rows[0][3].ToString());


            //填数据
            foreach (var ss in jo )  //查找某个字段与值
            {
                //if(((JObject) ss)["a"]=="aa")
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = ss["s5_feeding_time"].ToString();
                dr.Cells[1].Value = ss["s5_a_feeding_quantity"].ToString();
                dr.Cells[2].Value = ss["s5_b1c_feeding_quantity"].ToString();
                dr.Cells[3].Value = ss["s5_b2_feeing_quantity"].ToString();
                dr.Cells[4].Value = ss["s5_raw_material_sampling_results"].ToString();
                dr.Cells[5].Value = ss["s5_supplier"].ToString();
                dataGridView1.Rows.Add(dr);
            }

            

            bunker1 = "AB1C";
            bunker1_code = "";
            bunker1_batch = "";
            bunker2 = "B2";
            bunker2_code = "";
            bunker2_batch = "";

            checker = 332;

            //string date;//供料日期
            dataGridView1.Font = new Font("宋体", 12);
            //dataGridView2.Font = new Font("宋体", 12);
        }

        private void Setup()
        {
            textBox1.Text = product_code;
            textBox2.Text = product_num;
            textBox3.Text = product_instrnum;
            textBox4.Text = bunker1;
            textBox5.Text = bunker2;
            textBox6.Text = bunker1_code;
            textBox7.Text = bunker2_code;
            textBox8.Text = bunker1_batch;
            textBox9.Text = bunker2_batch;

            //textBox1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView2.AllowUserToAddRows = false;
        }

        public Record_extrusSupply(ExtructionProcess winMain)
        {
            //conn = con;
            InitializeComponent();
            extructionformfather = winMain;
            Init();
            Setup();
        }

        private void splitContainer11_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
 

        private void button1_Click(object sender, EventArgs e)
        {   
            float temp;
            //if(dateTimePicker1.Text==""||!float.TryParse(Serve_out.Text,out temp)||)

            checkout = Serve_checkresult.Text.ToString();
            server = Serve_person.Text.ToString();
            if (checkout == "" || server == "")
            {
                MessageBox.Show("原料抽查结果、供料人均不能为空");
                return;
            }

            //date = dateTimePicker1.Text.ToString();
            date = dateTimePicker1.Value;

            if (!float.TryParse(Serve_out.Text, out temp) || !float.TryParse(Serve_in.Text, out temp) || !float.TryParse(Serve_mid.Text, out temp))
            {
                MessageBox.Show("供料量必须为数值类型");
                return;  
            }
            serve_out = (float)Convert.ToSingle(Serve_out.Text.ToString());
            serve_in = (float)Convert.ToSingle(Serve_in.Text.ToString());
            serve_mid = (float)Convert.ToSingle(Serve_mid.Text.ToString());



            //填数据
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = date;
            dr.Cells[1].Value = serve_out;
            dr.Cells[2].Value = serve_in;
            dr.Cells[3].Value = serve_mid;
            dr.Cells[4].Value = checkout;
            dr.Cells[5].Value = server;
            dataGridView1.Rows.Add(dr);
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
            
        //    //bunker_use = comboBox1.Text.ToString();
        //    //checker = textBox10.Text.ToString();
        //    //if (bunker_use == "")
        //    //{
        //    //    MessageBox.Show("料仓不能为空");
        //    //    return;
        //    //}

        //    float temp;
        //    if (!float.TryParse(textBox11.Text, out temp) || !float.TryParse(textBox12.Text, out temp) || !float.TryParse(textBox14.Text, out temp) || !float.TryParse(textBox15.Text, out temp))
        //    {
        //        MessageBox.Show("用料、余料必须为数值类型");
        //        return;
        //    }
        //    use1 = (float)Convert.ToSingle(textBox11.Text.ToString());
        //    left1 = (float)Convert.ToSingle(textBox12.Text.ToString());
        //    use2 = (float)Convert.ToSingle(textBox14.Text.ToString());
        //    left2 = (float)Convert.ToSingle(textBox15.Text.ToString());
            
        //    //添加到表格
        //    DataGridViewRow dr = new DataGridViewRow();
        //    foreach (DataGridViewColumn c in dataGridView2.Columns)
        //    {
        //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
        //    }
        //    dr.Cells[0].Value = use1;
        //    dr.Cells[1].Value = left1;
        //    dr.Cells[2].Value = use2;
        //    dr.Cells[3].Value = left2;

        //    dataGridView2.Rows.Add(dr);
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count==0)
                return;
            //if (dataGridView1.SelectedRows[0].Index < dataGridView1.Rows.Count - 1 )
            //    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
        }

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView2.SelectedRows.Count== 0)
        //        return;
        //    //if (dataGridView2.SelectedRows[0].Index < dataGridView2.Rows.Count - 1 )
        //    //    dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
        //    dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
        //}

        private void Serve_out_TextChanged(object sender, EventArgs e)
        {

        }

        public void DataSave()
        {
            //string date;//供料日期
            //float serve_out;//外层供料量
            //float serve_mid;//中层供料量
            //float serve_in;//中内层供料量
            //string checkout;
            //string server;//供料人

            //string bunker_use;//物料使用料仓
            //float use;//物料使用量
            //float left;//余料
            //int checker;//复核人

            bunker1_code = textBox6.Text;
            bunker1_batch = textBox8.Text;
            bunker2_code = textBox7.Text;
            bunker2_batch = textBox9.Text;
            if (bunker1_code == "" || bunker1_batch == "" || bunker2_code == "" || bunker2_batch == "")
            {
                MessageBox.Show("原料代码和原料批次均不能为空！");
                return;
            }
            date = dateTimePicker1.Value;

            float temp;
            if (!float.TryParse(textBox11.Text, out temp) || !float.TryParse(textBox12.Text, out temp) || !float.TryParse(textBox14.Text, out temp) || !float.TryParse(textBox15.Text, out temp))
            {
                MessageBox.Show("用料、余料必须为数值类型");
                return;
            }
            use1 = (float)Convert.ToSingle(textBox11.Text.ToString());
            left1 = (float)Convert.ToSingle(textBox12.Text.ToString());
            use2 = (float)Convert.ToSingle(textBox14.Text.ToString());
            left2 = (float)Convert.ToSingle(textBox15.Text.ToString());

            string strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();

            //jason 保存表格
            JArray jarray = JArray.Parse("[]");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string json = @"{}";
                JObject j = JObject.Parse(json);
                //j.Add("s", new JValue("3"));
                j.Add("s5_feeding_time", new JValue(Convert.ToDateTime(dataGridView1.Rows[i].Cells[0].Value.ToString())));
                j.Add("s5_a_feeding_quantity", new JValue(Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString())));
                j.Add("s5_b1c_feeding_quantity", new JValue(Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString())));
                j.Add("s5_b2_feeing_quantity", new JValue(Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString())));
                j.Add("s5_raw_material_sampling_results", new JValue(dataGridView1.Rows[i].Cells[4].Value.ToString()));
                j.Add("s5_supplier", new JValue(dataGridView1.Rows[i].Cells[5].Value.ToString()));

                //System.Console.WriteLine(json.ToString());
                jarray.Add(j);
            }

            //System.Console.WriteLine(jarray.ToString());

            string s = "update extrusion set s5_ab1c_raw_material_id='"+bunker1_code;
            s += "',s5_b2_raw_material_id='" + bunker2_code;
            s += "',s5_ab1c_raw_material_batch='" + bunker1_batch;
            s += "',s5_b2_raw_material_batch='" + bunker2_batch;
            s += "',s5_feeding_info='" + jarray.ToString();
            s += "',s5_ab1c_raw_material_consumption=" + Convert.ToInt32(use1);
            s += ",s5_ab1c_raw_material_margin=" + Convert.ToInt32(left1);
            s += ",s5_b2_raw_material_consumption=" + Convert.ToInt32(use2);
            s += ",s5_b2_raw_material_margin=" + Convert.ToInt32(left2);
            s += " where id=1";
            //更新数据库
            int result = 0;
            using (SqlCommand comm = new SqlCommand(s, conn))
            {
                //conn.Open();
                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    //MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
