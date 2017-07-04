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

        float sum_provide;//总共供料量
        mySystem.CheckForm checkform;

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
            sum_provide = 0;
            sumout = 0;
            sumin = 0;
            summid = 0;
            //string date;//供料日期
            dataGridView1.Font = new Font("宋体", 12);
            textBox16.Text = mainform.proInstruction;
            button4.Enabled = false;
            textBox12.ReadOnly = true;
            textBox15.ReadOnly = true;
        }

        private void Setup()
        {
            //textBox1.Text = product_code;
            textBox2.Text = product_num;
            textBox3.Text = product_instrnum;
            textBox4.Text = bunker1;
            textBox5.Text = bunker2;
            //textBox6.Text = bunker1_code;
            //textBox7.Text = bunker2_code;
            textBox8.Text = bunker1_batch;
            textBox9.Text = bunker2_batch;

            //textBox1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView2.AllowUserToAddRows = false;
            object s=null;
            EventArgs e = null ;
            dateTimePicker1_ValueChanged(s,e);
            textBox16.Text = mySystem.Parameter.proInstruction;

            button4.Enabled = false;
            get_prodcode();
            get_matcode();
        }

        //查找物料代码填入物料代码复选框中
        private void get_matcode()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select raw_material_code from raw_material";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return;
            }
            else
            {
                da.Dispose();
                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    comboBox3.Items.Add(tempdt.Rows[i][0].ToString());//添加
                    comboBox4.Items.Add(tempdt.Rows[i][0].ToString());//添加
                }
            }
        }
        //查找产品代码填入复选框中
        private void get_prodcode()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select product_code from product_aoxing";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return;
            }
            else
            {
                da.Dispose();
                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    comboBox2.Items.Add(tempdt.Rows[i][0].ToString());//添加
                }
            }
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
 
        //添加供料
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
            if (mySystem.Parameter.NametoID(server) <= 0)
            {
                MessageBox.Show("供料人id不存在");
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

        //删除一条供料记录
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

            //bunker1_code = textBox6.Text;
            bunker1_batch = textBox8.Text;
            //bunker2_code = textBox7.Text;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.isSqlOk)
            {

            }
            else
            {
                //获取使用量信息
                string struse1 = textBox11.Text;
                string struse2 = textBox14.Text;
                if (!float.TryParse(struse1,out use1) || !float.TryParse(struse1,out use2))
                {
                    MessageBox.Show("使用量必需为数字");
                    return;
                }
                left1 = float.Parse(textBox12.Text);
                left2 = float.Parse(textBox15.Text);
                //供料总量
                float sum_provide1 = float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value.ToString()) + float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());
                float sum_provide2 = float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());

                

                int result = 0;
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                //判断对应生产指令和生产批号下数据库中是否存在对应记录

                //获得记录的jason块,最后形式如[{2017/7/3：[{8-23：---}，{8-27：---}]},{2017/7/4：[{8-23：---}，{8-27：---}]},...]
                //string json = @"[]";
                //JArray jarray = JArray.Parse(json);
                //foreach (var item in dict)
                //{
                //    string st = "{'";
                //    st += DateTime.Parse(item.Key).ToShortDateString() + "':";
                //    JArray jlist = JArray.Parse(@"[]");
                //    //解析list<record>
                //    for (int i = 0; i < item.Value.Count; i++)
                //    {
                //        string objstr = "{'";
                //        objstr += item.Value[i].time+"':";
                //        objstr += "[" + item.Value[i].srout + "," + item.Value[i].srin + "," + item.Value[i].srmid + "," + item.Value[i].isqua + "," + item.Value[i].man + "]";
                //        objstr += "}";
                //        JObject obj = JObject.Parse(objstr);
                //        jlist.Add(obj);
                //    }
                //    st += jlist.ToString() + "}";
                //    JObject temp = JObject.Parse(st);
                //    jarray.Add(temp);
                //}
                string str = "{'" + dateTimePicker1.Value.ToShortDateString() + "':";
                JArray jlist = JArray.Parse(@"[]");
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                        string objstr = "{'";
                        objstr += dataGridView1.Rows[i].Cells[0].Value.ToString()+"':";
                        objstr += "[" + dataGridView1.Rows[i].Cells[1].Value + "," + dataGridView1.Rows[i].Cells[2].Value + "," + dataGridView1.Rows[i].Cells[3].Value + "," + dataGridView1.Rows[i].Cells[4].Value + ",'" + dataGridView1.Rows[i].Cells[5].Value + "']";
                        objstr += "}";
                        JObject obj = JObject.Parse(objstr);
                        jlist.Add(obj);
                }
                str += jlist.ToString() + "}";
                JObject jobj= JObject.Parse(str);

                //插入原料批号表,并获取原料id和批号id
                bunker1_code = comboBox3.Text;
                bunker2_code = comboBox4.Text;
                bunker1_batch = textBox8.Text;
                bunker2_batch = textBox9.Text;
                insert_matbatch(id_findby_matcode(bunker1_code), bunker1_batch);
                insert_matbatch(id_findby_matcode(bunker2_code), bunker2_batch);
                int matid1 = id_findby_matcode(bunker1_code);
                int matid2 = id_findby_matcode(bunker2_code);
                int batchid1 = batchid_findby_matcode(bunker1_batch);
                int batchid2 = batchid_findby_matcode(bunker2_batch);

                //判断数据库中是否存在该记录


                //插入数据库新纪录
                comm.CommandText = "insert into extrusion_s5_feeding(product_batch_id,production_instruction_id,s5_ab1c_raw_material_id,s5_b2_raw_material_id,s5_ab1c_raw_material_batch,s5_b2_raw_material_batch,s5_feeding_info,s5_ab1c_raw_material_consumption,s5_ab1c_raw_material_margin,s5_b2_raw_material_consumption,s5_b2_raw_material_margin) values(@batchid,@instrid,@ab1c_matid,@b2_matid,@ab1c_matbatch,@b2_matbatch,@feedinfo,@ab1c_use,@ab1c_left,@b2_use,@b2_left)";
                comm.Parameters.Add("@batchid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@instrid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@ab1c_matid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@b2_matid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@ab1c_matbatch", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@b2_matbatch", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@feedinfo", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@ab1c_use", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@ab1c_left", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@b2_use", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@b2_left", System.Data.OleDb.OleDbType.Integer);

                comm.Parameters["@batchid"].Value = id_findby_batch(product_num);
                comm.Parameters["@instrid"].Value = id_findby_instr(product_instrnum);
                comm.Parameters["@ab1c_matid"].Value = matid1;
                comm.Parameters["@b2_matid"].Value = matid2;
                comm.Parameters["@ab1c_matbatch"].Value = batchid1;
                comm.Parameters["@b2_matbatch"].Value = batchid2;
                comm.Parameters["@feedinfo"].Value = jobj.ToString();
                comm.Parameters["@ab1c_use"].Value = Convert.ToInt32(use1);
                comm.Parameters["@ab1c_left"].Value = Convert.ToInt32(left1);
                comm.Parameters["@b2_use"].Value = Convert.ToInt32(use2);
                comm.Parameters["@b2_left"].Value = Convert.ToInt32(left2);


                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
                comm.Dispose();
            }
            button4.Enabled = true;
        }

        public override void CheckResult()
        {
            base.CheckResult();
            int revid = checkform.userID;
            bool isok = checkform.ischeckOk;
            string opinion = checkform.opinion;
            textBox1.Text = checkform.userName;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
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

        //通过生产指令编号查找生产指令id
        private int id_findby_instr(string instr)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select production_instruction_id from production_instruction where production_instruction_code='" + instr + "'";

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

        //产品批号表中插入一条记录
        private void insert_batch(string batch,int prodid)
        {
            int result = 0;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "insert into product_batch(product_batch_code,product_id) values(@code,@prodid)";

            comm.Parameters.Add("@code", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@prodid", System.Data.OleDb.OleDbType.Integer);

            comm.Parameters["@code"].Value = batch;
            comm.Parameters["@prodid"].Value = prodid;

            result = comm.ExecuteNonQuery();
            if (result <= 0)
            {
                MessageBox.Show("批号插入批号表出错");
                return;
            }
        }

        //通过批号查找数据库中id
        private int id_findby_batch(string batch)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select product_batch_id from product_batch where product_batch_code='" + batch + "'";

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

        //原料批号表中插入一条记录
        private void insert_matbatch(int matid, string batch)
        {
            //int result = 0;
            //OleDbCommand comm = new OleDbCommand();
            //comm.Connection = mySystem.Parameter.connOle;
            //comm.CommandText = "insert into raw_material_batch(raw_material_id,batch) values(@id,@batch)";

            //comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.VarChar);
            //comm.Parameters.Add("@prodid", System.Data.OleDb.OleDbType.Integer);

            //comm.Parameters["@code"].Value = batch;
            //comm.Parameters["@prodid"].Value = prodid;

            //result = comm.ExecuteNonQuery();
            //if (result <= 0)
            //{
            //    MessageBox.Show("批号插入批号表出错");
            //    return;
            //}
            List<string> cols=new List<string>();
            cols.Add("raw_material_id");
            cols.Add("batch");
            List<object> val=new List<object>();
            val.Add(matid);
            val.Add(batch);
            if (!mySystem.Utility.insertAccess(mySystem.Parameter.connOle, "raw_material_batch", cols, val))
            {
                MessageBox.Show("插入原料批号表出错");
                return;
            }
        }

        //通过原料批号查找批号id
        private int batchid_findby_matcode(string code)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select batch_id from raw_material_batch where batch='" + code+ "'";

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

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            //供料总量
            sum_provide = 0;
            sum_provide = float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value.ToString()) + float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());

            if (!float.TryParse(textBox11.Text, out use1))
            {
                MessageBox.Show("使用量必需为数字");
                textBox11.Text= "";
                return;
            }
            textBox12.Text = (sum_provide - use1).ToString();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            //供料总量
            sum_provide = 0;
            sum_provide = float.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString());

            if (!float.TryParse(textBox14.Text, out use2))
            {
                MessageBox.Show("使用量必需为数字");
                textBox14.Text = "";
                return;
            }
            textBox15.Text = (sum_provide - use2).ToString();
        }

    }
}
