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


namespace mySystem.Extruction.Process
{
    /// <summary>
    /// 吹膜工序清场记录
    /// </summary>
    public partial class Record_extrusSiteClean : Form
    {
        SqlConnection conn=null;
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

        string[] unit_serve;//供料工序
        string[] unit_exstru;//吹膜工序

        List<int> ischecked_1;//供料工序 检查结果是否合格列表
        List<int> ischecked_2;//吹膜工序 检查结果是否合格列表

        static int k = 0;
        private void Init()
        {
            cleaner = 32;
            checker = 45;

            prod_instrcode = "ox32";
            prod_code = "rs";
            prod_batch = "0x55";

            //date = "2017/6/10";

            comboBox2.Text = "供料工序";
            unit_serve = new string[] {"填写供料记录是否已归档","使用剩余的原料是否称重退库","设备是否按程序开机，并切断电源","设备和工位器具是否已清洁","生产废弃物是否已清，卫生是否已打扫","其他"};
            unit_exstru = new string[]{"填写的记录是否已归档","使用的文件，设备运行参数是否已经归档","设备是否已按程序关机，并切断电源","设备和工位器具是否已清洁","生产的产品是否定置摆放，粘贴标签，登记台账","生产用半成品标签是否已销毁","生产废物是否已清，卫生是否已打扫","其他"};

            dataGridView1.AllowUserToAddRows = false;
            ischecked_1 = new List<int>();
            ischecked_2 = new List<int>();

            for (int i = 0; i < unit_serve.Length; i++)
                ischecked_1.Add(1);
            for (int i = 0; i < unit_exstru.Length; i++)
                ischecked_2.Add(1);

            comboBox1.Text = "合格";
            dataGridView1.Font = new Font("宋体", 12);
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
                        for (int i = 0; i < unit_serve.Length; i++)
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
                        for (int i = 0; i < unit_exstru.Length; i++)
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
        public Record_extrusSiteClean(SqlConnection mycon)
        {
            conn = mycon;
            InitializeComponent();
            Init();
            AddtoGridView();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cleaner = textBox4.Text;
            prod_instrcode=textBox1.Text;
            prod_code=textBox2.Text;
            prod_batch=textBox4.Text;
            date=dateTimePicker1.Value;

            checkout = comboBox1.Text == "合格";
            //checker = textBox5.Text;
            extr = textBox3.Text;

            //if (cleaner == "" || checker == "")
            //{
            //    MessageBox.Show("清场人和复核人均不能为空");
            //    return;
            //}


            //插入数据库
            string s = "update clean_record_of_extrusion_process set production_instruction='" + prod_instrcode + "',product_id_before='" + prod_code + "',product_batch_before='" + prod_batch + "',clean_date='" + date + "'";
            s += ",cleaner_id=" + cleaner;
            s += ",reviewer_id=" + checker;
            for (int i = 0; i < unit_serve.Length; i++)
            {
                s += ",item" + (i + 1).ToString() + "_is_cleaned="+ischecked_1[i];
            }
            for (int i = 7; i < 15; i++)
            {
                s += ",item" + i.ToString() + "_is_cleaned=" + ischecked_2[i-7];
            }

            s += " where id=1";
            //SqlCommand comm = new SqlCommand(s, conn);
            //SqlDataAdapter da = new SqlDataAdapter(comm);
            //更新数据库
            int result = 0;
            using (SqlCommand comm = new SqlCommand(s, conn))
            {
                //conn.Open();
                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }

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

        private void button2_Click(object sender, EventArgs e)
        {
            /*LoginForm lf = new LoginForm();
            lf.LoginButton.Text = "审核通过";
            lf.ExitButton.Text = "取消";
            lf.Show();*/
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
