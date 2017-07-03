using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
//using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;



namespace mySystem.Extruction.Process
{
    /// <summary>
    /// 吹膜工序清场记录
    /// </summary>
    public partial class Record_extrusSiteClean : mySystem.BaseForm
    {
        //SqlConnection conn=null;
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access

        string prod_instrcode;//生产指令
        string prod_code;//清场前产品代码及
        string prod_batch;//清场前批号
        //string date;//清场日期
        DateTime date;

        string cleanorder;//清场工序

        int cleaner;//清场人
        bool checkout;//检查结果
        int checker;//检查人
        string extr;//备注
        bool ok;//是否清洁操作

        //string[] unit_serve;//供料工序
        //string[] unit_exstru;//吹膜工序

        List<string> unit_serve;//供料工序
        List<string> unit_exstru;//吹膜工序
        List<int> ischecked_1;//供料工序 检查结果是否合格列表
        List<int> ischecked_2;//吹膜工序 检查结果是否合格列表
        mySystem.CheckForm checkform;

        int label;//判断是更新数据库还是插入数据库

        static int k = 0;

        //查询数据库，获得供料工序和吹膜工序清场列表
        private void queryjob()
        {
            unit_serve=new List<string>();
            unit_exstru=new List<string>();
            if ( mySystem.Parameter.isSqlOk)
            {
                string sql = "select * from feedingprocess_cleansite";
                SqlCommand cmd = new SqlCommand(sql, mainform.conn);
                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    unit_serve.Add(dt.Rows[i][1].ToString());
                }
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
                sql = "select * from extrusion_cleansite";
                cmd = new SqlCommand(sql, mainform.conn);
                data = new SqlDataAdapter(cmd);
                data.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    unit_exstru.Add(dt.Rows[i][1].ToString());
                }
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
                //unit_serve = new string[] { "填写供料记录是否已归档", "使用剩余的原料是否称重退库", "设备是否按程序开机，并切断电源", "设备和工位器具是否已清洁", "生产废弃物是否已清，卫生是否已打扫", "其他" };
                //unit_exstru = new string[] { "填写的记录是否已归档", "使用的文件，设备运行参数是否已经归档", "设备是否已按程序关机，并切断电源", "设备和工位器具是否已清洁", "生产的产品是否定置摆放，粘贴标签，登记台账", "生产用半成品标签是否已销毁", "生产废物是否已清，卫生是否已打扫", "其他" };
            }
            else
            {
                string accessql = "select * from feedingprocess_cleansite";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    unit_serve.Add(dt.Rows[i][1].ToString());
                }
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
                accessql = "select * from extrusion_cleansite";
                cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                data = new OleDbDataAdapter(cmd);
                data.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    unit_exstru.Add(dt.Rows[i][1].ToString());
                }
                cmd.Dispose();
                data.Dispose();
                dt.Clear();
            }

        }
        private void Init()
        {
            cleaner = mainform.userID;
            checker = 45;

            prod_instrcode = mySystem.Parameter.proInstruction;
            System.Console.WriteLine(mySystem.Parameter.proInstruction);
            prod_code = "rs";
            prod_batch = "0x55";
            //prod_instrcode = mainform.proInstruction;

            //date = "2017/6/10";
            textBox1.Text = prod_instrcode;
            comboBox2.Text = "供料工序";


            dataGridView1.AllowUserToAddRows = false;
            ischecked_1 = new List<int>();
            ischecked_2 = new List<int>();

            for (int i = 0; i < unit_serve.Count; i++)
                ischecked_1.Add(1);
            for (int i = 0; i < unit_exstru.Count; i++)
                ischecked_2.Add(1);

            comboBox1.Text = "合格";
            dataGridView1.Font = new Font("宋体", 10);
            button2.Enabled = false;
            textBox6.ReadOnly= true;
            button3.Enabled = false;
            label = 0;
           
        }

        //根据生产指令id将数据填写到各控件中
        private int fill_by_id(int id)
        {
            string asql = "select product_id_before,product_batch_before,clean_date,is_cleaned,cleaner_id,reviewer_id,is_qualified from clean_record_of_extrusion_process where production_instruction_id=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
            {
                //将tempdt填入控件
                textBox2.Text = tempdt.Rows[0][0].ToString();
                textBox4.Text = tempdt.Rows[0][1].ToString();
                dateTimePicker1.Value =DateTime.Parse(tempdt.Rows[0][2].ToString());
                textBox5.Text = mySystem.Parameter.IDtoName(int.Parse(tempdt.Rows[0][4].ToString()));


                string rev = mySystem.Parameter.IDtoName(int.Parse(tempdt.Rows[0][5].ToString())); 
                if (rev == "")
                    button2.Enabled = true;
                else
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                    textBox6.Text = rev;
                    textBox5.Text=int.Parse(tempdt.Rows[0][5].ToString())>0?"合格":"不合格";
                }

                string jstr = tempdt.Rows[0][3].ToString();
                JArray jarray = JArray.Parse(jstr);
                //第一个选项
                JObject jobj1 = JObject.Parse(jarray[0].ToString());
                int i=0;
                foreach (var p in jobj1)
                {
                    ischecked_1[i++] = int.Parse(jobj1[p.Key].ToString());
                }
                //第二个选项
                JObject jobj2 = JObject.Parse(jarray[1].ToString());
                i = 0;
                foreach (var p in jobj2)
                {
                    ischecked_2[i++] = int.Parse(jobj2[p.Key].ToString());
                }
            }
            return 0;
        }

        private void AddtoGridView()
        {
            cleanorder = comboBox2.Text.ToString();
            switch (cleanorder)
            {
                case "供料工序":
                    {
                        Datagrid_del();
                        //添加
                        for (int i = 0; i < unit_serve.Count; i++)
                        {
                            DataGridViewRow dr = new DataGridViewRow();
                            foreach (DataGridViewColumn c in dataGridView1.Columns)
                            {
                                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                            }
                            dr.Cells[0].Value = i + 1;
                            dr.Cells[1].Value = unit_serve[i];
                            dr.Cells[2].Value = ischecked_1[i];
                            dataGridView1.Rows.Add(dr);
                        }
                    }
                    break;
                case "吹膜工序":
                    {
                        Datagrid_del();
                        //添加
                        for (int i = 0; i < unit_exstru.Count; i++)
                        {
                            DataGridViewRow dr = new DataGridViewRow();
                            foreach (DataGridViewColumn c in dataGridView1.Columns)
                            {
                                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                            }
                            dr.Cells[0].Value = i + 1;
                            dr.Cells[1].Value = unit_exstru[i];
                            dr.Cells[2].Value = ischecked_2[i];
                            dataGridView1.Rows.Add(dr);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void Datagrid_del()
        {
            //System.Console.WriteLine(dataGridView1.Rows.Count+"********************************************************");
            //if (dataGridView1.Rows.Count == 0)
            //    return;
            //for (int i = dataGridView1.Rows.Count-2; i >0;i-- )
            //    dataGridView1.Rows.RemoveAt(i);

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                dataGridView1.Rows.RemoveAt(i);

        }
        public Record_extrusSiteClean(mySystem.MainForm mainform):base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            queryjob();
            Init();
            if (fill_by_id(mySystem.Parameter.proInstruID) == -1)//根据id填表失败，表未填写过
            {
                AddtoGridView();
                label = 1;//代表是新的填写,保存采用插入数据库方式
            }
            else
            {
                label = 0;//代表是在原来基础上更改，保存采用更新方式
                fill_by_id(mySystem.Parameter.proInstruID);
            }
                
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            //如果查找成功返回id，否则返回-1
            //int rtnum = -1;
            if (mainform.isSqlOk)
            {
                //未完成
                //string sql = "select user_id from cleanarea";
                //SqlCommand comm = new SqlCommand(sql, conn);
                //SqlDataAdapter da = new SqlDataAdapter(comm);

                //dt = new DataTable();
                //da.Fill(dt);
                return -1;
            }
            else
            {
                string asql = "select user_id from user_aoxing where user_name=" + "'" + s + "'";
                OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cleaner = textBox4.Text;
            string strcleaner = textBox5.Text;
            //string strchecker = textBox6.Text;
            if (strcleaner == "")
            {
                MessageBox.Show("清场人不能为空");
                return;
            }
            cleaner = queryid(strcleaner);
            //checker = queryid(strchecker);
            if (cleaner == -1)
            {
                MessageBox.Show("清场人id不存在");
                return;
            }

            //生产指令先用a代替
            int a = mySystem.Parameter.proInstruID;

            prod_code=textBox2.Text;
            prod_batch=textBox4.Text;
            date=dateTimePicker1.Value;

            checkout = comboBox1.Text == "合格";
            //checker = textBox5.Text;
            extr = textBox3.Text;

            //添加记录到jason
            string json = @"[]";
            JArray jarray = JArray.Parse(json);
            string st = "{}";
            JObject temp1 = JObject.Parse(st);
            JObject temp2 = JObject.Parse(st);
            for (int i = 0; i < unit_serve.Count; i++)
            {
                temp1.Add(unit_serve[i],new JValue(ischecked_1[i].ToString()));
            }
            for (int i = 0; i < unit_exstru.Count; i++)
            {
                temp2.Add(unit_exstru[i],new JValue(ischecked_2[i].ToString()));
            }
            jarray.Add(temp1);
            jarray.Add(temp2);       

            //插入数据库
            int result = 0;
            if (mySystem.Parameter.isSqlOk)
            {
                //需要修改。。。。。。。。。。。。。。
                //string s = "update clean_record_of_extrusion_process set production_instruction_id='" + prod_instrcode + "',product_id_before='" + prod_code + "',product_batch_before='" + prod_batch + "',clean_date='" + date + "'";
                //s += ",cleaner_id=" + cleaner;
                //s += ",reviewer_id=" + checker;
                //for (int i = 0; i < unit_serve.Count; i++)
                //{
                //    s += ",item" + (i + 1).ToString() + "_is_cleaned=" + ischecked_1[i];
                //}
                //for (int i = 7; i < 15; i++)
                //{
                //    s += ",item" + i.ToString() + "_is_cleaned=" + ischecked_2[i - 7];
                //}

                //s += " where id=1";
                //using (SqlCommand comm = new SqlCommand(s, conn))
                //{
                //    result = comm.ExecuteNonQuery();

                //}
            }
            else
            {
                //string s = "update clean_record_of_extrusion_process set production_instruction_id=" + a + ",product_id_before='" + prod_code + "',product_batch_before='" + prod_batch + "',clean_date='" + date + "'";
                //s += ",cleaner_id=" + cleaner;
                //s += ",reviewer_id=" + checker;
                //s+=",is_cleaned='"+jarray.ToString();
                //s += "',is_qualified=" + checkout;
                //s += " where id=1";
                //OleDbCommand comm = new OleDbCommand(s, mySystem.Parameter.connOle);
                //result = comm.ExecuteNonQuery();

                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                if (label == 1)
                    comm.CommandText = "";
                else
                    comm.CommandText = "update clean_record_of_extrusion_process set product_id_before=@beforeid,product_batch_before= @beforebatch,clean_date=@cleandate,cleaner_id=@cleanerid,is_cleaned=@cleancont where production_instruction_id= @id";
                comm.Parameters.Add("@beforeid", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@beforebatch", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@cleanerid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@cleancont", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

                comm.Parameters["@beforeid"].Value = prod_code;
                comm.Parameters["@beforebatch"].Value = prod_batch;
                comm.Parameters["@cleandate"].Value = checkform.ischeckOk;
                comm.Parameters["@id"].Value = mySystem.Parameter.proInstruID;

            }
            if (result > 0)
            {
                MessageBox.Show("添加成功");
            }
            else { MessageBox.Show("错误"); }
            button2.Enabled = true;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (k > 0)
                AddtoGridView();
            else
                k=1;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //System.Console.WriteLine(e.ColumnIndex.ToString() + "," + e.RowIndex.ToString());
            //cleanorder = comboBox2.Text.ToString();
            switch (cleanorder)
            {
                case "供料工序":
                    {
                        //System.Console.WriteLine("供料工序");
                        bool v = dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString() == "True";
                        //System.Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString());
                        if(v)
                            ischecked_1[e.RowIndex] = 1;
                        else
                            ischecked_1[e.RowIndex] = 0;
                    }
                    break;
                case "吹膜工序":
                    {
                        //System.Console.WriteLine("吹膜工序");
                        bool v = dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString() == "True";
                        if (v)
                            ischecked_2[e.RowIndex] = 1;
                        else
                            ischecked_2[e.RowIndex] = 0;
                    }
                    break;
            }

        }

        //重写函数，获得审核信息
        public override void CheckResult()
        {
            base.CheckResult();
            textBox6.Text = checkform.userName;

            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "update clean_record_of_extrusion_process set reviewer_id= @revid,review_opinion=@revopinion,is_review_qualified= @isok where production_instruction_id= @id";
            comm.Parameters.Add("@revid", System.Data.OleDb.OleDbType.Integer);
            comm.Parameters.Add("@revopinion", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@isok", System.Data.OleDb.OleDbType.Boolean);
            comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

            comm.Parameters["@revid"].Value = checkform.userID;
            comm.Parameters["@revopinion"].Value = checkform.opinion;
            comm.Parameters["@isok"].Value = checkform.ischeckOk;
            comm.Parameters["@id"].Value = mySystem.Parameter.proInstruID;

            int result = comm.ExecuteNonQuery();
            if (result <= 0)
            {
                MessageBox.Show("审核出错");
                return;
            }
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //LoginForm check = new LoginForm(conn);
            ///check.ShowDialog();
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
