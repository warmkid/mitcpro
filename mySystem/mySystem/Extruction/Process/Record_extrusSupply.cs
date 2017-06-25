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
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusSupply : mySystem.BaseForm
    {
        //SqlConnection conn;
        //private ExtructionProcess extructionformfather = null;

        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access

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
        float sumout;//合计
        float sumin;
        float summid;
        int checker;//复核人

        private class record
        {
            public string time;
            public float srout;
            public float srin;
            public float srmid;
            public int isqua;
            public string man;
        }
        Dictionary<string, List<record>> dict;//日期为键值
        Dictionary<string, float> dictsum_out;
        Dictionary<string, float> dictsum_in;
        Dictionary<string, float> dictsum_mid;

        private void Init()
        {
            //string strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //SqlConnection conn = new SqlConnection(strCon);
            //conn.Open();
            //string s = "select product_id,product_batch,production_instruction,s5_feeding_info from extrusion where id=1";
            //SqlCommand comm = new SqlCommand(s, conn);
            //SqlDataAdapter da = new SqlDataAdapter(comm);
            //DataTable dtemp = new DataTable();
            //da.Fill(dtemp);
          


            //conn.Close();

            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ////dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //product_code = dtemp.Rows[0][0].ToString();
            //product_num = dtemp.Rows[0][1].ToString();
            //product_instrnum = dtemp.Rows[0][2].ToString();

            ////解析jason
            //JArray jo = JArray.Parse(dtemp.Rows[0][3].ToString());


            ////填数据
            //foreach (var ss in jo )  //查找某个字段与值
            //{
            //    //if(((JObject) ss)["a"]=="aa")
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = ss["s5_feeding_time"].ToString();
            //    dr.Cells[1].Value = ss["s5_a_feeding_quantity"].ToString();
            //    dr.Cells[2].Value = ss["s5_b1c_feeding_quantity"].ToString();
            //    dr.Cells[3].Value = ss["s5_b2_feeing_quantity"].ToString();
            //    dr.Cells[4].Value = ss["s5_raw_material_sampling_results"].ToString();
            //    dr.Cells[5].Value = ss["s5_supplier"].ToString();
            //    dataGridView1.Rows.Add(dr);
            //}


            dict = new Dictionary<string, List<record>>();
            dictsum_out = new Dictionary<string, float>();
            dictsum_in = new Dictionary<string, float>();
            dictsum_mid = new Dictionary<string, float>();

            bunker1 = "AB1C";
            bunker1_code = "";
            bunker1_batch = "";
            bunker2 = "B2";
            bunker2_code = "";
            bunker2_batch = "";

            checker = 332;
            sumout = 0;
            sumin = 0;
            summid = 0;
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
            object s=null;
            EventArgs e = null ;
            dateTimePicker1_ValueChanged(s,e);
        }

        public Record_extrusSupply(mySystem.MainForm mainform):base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
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

            checkout = comboBox1.Text.ToString();

            server = Serve_person.Text.ToString();
            if (checkout == "(空)" || server == "")
            {
                MessageBox.Show("原料抽查结果、供料人均不能为空");
                return;
            }

            //date = dateTimePicker1.Text.ToString();
            //date = dateTimePicker1.Value;

            if (!float.TryParse(Serve_out.Text, out temp) || !float.TryParse(Serve_in.Text, out temp) || !float.TryParse(Serve_mid.Text, out temp))
            {
                MessageBox.Show("供料量必须为数值类型");
                return;  
            }
            serve_out = (float)Convert.ToSingle(Serve_out.Text.ToString());
            serve_mid = (float)Convert.ToSingle(Serve_in.Text.ToString());
            serve_in = (float)Convert.ToSingle(Serve_mid.Text.ToString());
            int checkoutnum;
            if (checkout == "是")
                checkoutnum = 1;
            else
                checkoutnum = 0;


            //填数据
            
            //暂时删除最后合计行
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows[dataGridView1.Rows.Count-1].Index);
            }
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = DateTime.Now.ToLongTimeString().ToString();
            dr.Cells[1].Value = serve_out;
            dr.Cells[2].Value = serve_in;
            dr.Cells[3].Value = serve_mid;
            dr.Cells[4].Value = checkoutnum;
            dr.Cells[5].Value = server;
            dataGridView1.Rows.Add(dr);

            //添加数据到dict
            string key=dateTimePicker1.Value.ToShortDateString();
            record r=new record();
            r.time=dr.Cells[0].Value.ToString();
            r.srout=serve_out;
            r.srin=serve_in;
            r.srmid=serve_mid;
            r.isqua=checkoutnum;
            r.man=server;
            dict[key].Add(r);

            dictsum_out[key] += serve_out;
            dictsum_in[key] += serve_in;
            dictsum_mid[key] += serve_mid;

            sumout += serve_out;
            sumin += serve_in;
            summid += serve_mid;

            //添加合计
            DataGridViewRow drsum = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                drsum.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            drsum.Cells[0].Value = "小计";
            drsum.Cells[1].Value = dictsum_out[key];
            drsum.Cells[2].Value = dictsum_in[key];
            drsum.Cells[3].Value = dictsum_mid[key];

            dataGridView1.Rows.Add(drsum);


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
            int ind=dataGridView1.SelectedRows[0].Index;
            if (ind == dataGridView1.Rows.Count - 1)
                return;


            //更改合计行的值
            float a = float.Parse( dataGridView1.Rows[ind].Cells[1].Value.ToString());
            float b = float.Parse(dataGridView1.Rows[ind].Cells[2].Value.ToString());
            float c = float.Parse(dataGridView1.Rows[ind].Cells[3].Value.ToString());
            sumout -= a;
            sumin -= b;
            summid -= c;


            //删除dict中的值
            string key = dateTimePicker1.Value.ToShortDateString();
            if (dict.ContainsKey(key))
            {
                dict[key].RemoveAt(ind);
                dictsum_out[key] -= a;
                dictsum_in[key] -= b;
                dictsum_mid[key] -= c;

                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = dictsum_out[key];
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = dictsum_in[key];
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value = dictsum_mid[key];
            }
            dataGridView1.Rows.RemoveAt(ind);
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

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date = dateTimePicker1.Value;
            string key = date.ToShortDateString();
            //不存在key则创建
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<record>());
                dictsum_out.Add(key, 0);
                dictsum_in.Add(key, 0);
                dictsum_mid.Add(key, 0);
                //清空表格
                while (dataGridView1.Rows.Count != 0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                }
            }
            else
            {
                //清空表格
                while (dataGridView1.Rows.Count != 0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                }
                //充填表格
                foreach (record r in dict[key])
                {
                    DataGridViewRow dr= new DataGridViewRow();
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                    }
                    dr.Cells[0].Value = r.time;
                    dr.Cells[1].Value = r.srout;
                    dr.Cells[2].Value = r.srin;
                    dr.Cells[3].Value = r.srmid;
                    dr.Cells[4].Value = r.isqua;
                    dr.Cells[5].Value = r.man;
                    dataGridView1.Rows.Add(dr);
                    dr.Dispose();
                }

                //添加合计
                
                DataGridViewRow drsum = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    drsum.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                drsum.Cells[0].Value = "小计";
                drsum.Cells[1].Value = dictsum_out[key];
                drsum.Cells[2].Value = dictsum_in[key];
                drsum.Cells[3].Value = dictsum_mid[key];

                dataGridView1.Rows.Add(drsum);
            }
        }
    }
}
