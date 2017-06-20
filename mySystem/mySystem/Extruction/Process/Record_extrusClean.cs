using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusClean : Form
    {
        private ExtructionProcess extructionformfather = null;

        //string cleantime;//清洁日期
        DateTime cleantime;
        string classes;//班次
        string checker;//复核人
        //string checktime;//复核日期
        DateTime checktime;
        List<cont> cleancont;
        DataTable dt;
        bool isOk;
        string strCon;
        string sql;
        SqlConnection conn;

        List<int> cleanmans;
        List<int> checkmans;
        
        public Record_extrusClean(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            Init();
            connToServer();
            qury();

        }
        private void Init()
        {
            strCon = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            sql = "select * from cleanarea";
            isOk = false;
            cleancont = new List<cont>();
            cleanmans = new List<int>();
            checkmans = new List<int>();
            cont_clean = new cont();

            //dataGridView1.RowsDefaultCellStyle.Font = new Font("宋体", 12);  
            dataGridView1.Font = new Font("宋体", 12);
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
        }
        public void connToServer()
        {
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }

        private void qury()
        {
            if (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return;
            }
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);

            dt = new DataTable();
            da.Fill(dt);
            //dataGridView1.DataSource = dt;

            //DataColumn dc = dt.Columns[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dt.Rows[i][0].ToString();
                dr.Cells[1].Value = dt.Rows[i][1].ToString();
                dr.Cells[2].Value = true;
                //dr.Cells[2].Value = "是".ToString();
                dataGridView1.Rows.Add(dr);

                //清洁人和审核人均为-1代表初始状态留空
                cleanmans.Add(-1);
                checkmans.Add(-1);
            }


            
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        public class cont 
        {
            public bool cleanstat;
            public string cleaner;
            public string cleanchecker;
            public cont() { cleanstat = true; cleaner = ""; cleanchecker = ""; }
        }
        cont cont_clean;

        public void DataSave()
        {
            //cleantime = dateTimePicker1.Text.ToString();
            cleantime = dateTimePicker1.Value;
            //textBox1.Text = cleantime;
            classes = textBox1.Text.ToString();
            checker = textBox2.Text.ToString();
            //checktime = dateTimePicker2.Text.ToString();
            checktime = dateTimePicker2.Value;

            if (classes == "" || checker == "")
            {
                MessageBox.Show("班次和复核人均不能为空");
                return;
            }

            //添加清洁记录到jason
            //string json = @"{}";
            //JObject j = JObject.Parse(json);
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    string t = dataGridView1.Rows[i].Cells[0].Value.ToString();
            //    if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "True")
            //        j.Add(t, new JValue('1'));
            //    else
            //        j.Add(t, new JValue("0"));
            //}

            string json = @"[]";
            JArray jarray = JArray.Parse(json);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int a, b, c;
                string st = "{'";
                string t = dataGridView1.Rows[i].Cells[0].Value.ToString() + "':";
                st += t;
                //是否清洁合格
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "True")
                    a = 1;
                else
                    a = 0;
                st += "["+a.ToString()+","+cleanmans[i].ToString()+","+checkmans.ToString()+",]";

                JObject temp = JObject.Parse(st);
                jarray.Add(temp);
            }



            //System.Console.WriteLine(j.ToString());
            int status=0;
            //int checkerid = 5;
            //string s = "update extrusion set s1_clean_date='" + cleantime.ToString() + "' where id=1";
            //string s = "update extrusion set s1_flight='"+classes+"' where id=1";
            string s = "update extrusion set s1_clean_date='" + cleantime + "',s1_flight=" + classes + ",s1_reviewer_id='" + int.Parse(checker) + "',s1_review_date='" + checktime + "',s1_region_content_result_cleaner_reviewer='" + jarray.ToString() + "',step_status=" + status + " where id=1";
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
                    //MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }


            
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    cont_clean.cleanstat = dataGridView1.Rows[i].Cells[2].Value.ToString() == "True";
            //    if (null == dataGridView1.Rows[i].Cells[3].Value)
            //        cont_clean.cleaner = "";
            //    else cont_clean.cleaner = dataGridView1.Rows[i].Cells[3].Value.ToString();
            //    if (null == dataGridView1.Rows[i].Cells[4].Value)
            //        cont_clean.cleanchecker = "";
            //    else cont_clean.cleanchecker = dataGridView1.Rows[i].Cells[4].Value.ToString();
            //    cleancont.Add(cont_clean);

            //    System.Console.WriteLine(cleancont[i].cleanstat.ToString() + cleancont[i].cleaner.ToString() + cleancont[i].cleanchecker.ToString());
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //更改清洁人项
            if (e.ColumnIndex == 3)
            {
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (rt > 0)
                    cleanmans[e.RowIndex] = rt;
                else
                    MessageBox.Show("清洁人id不存在，请重新输入");
                return;
            }
            //更改审核人项
            if (e.ColumnIndex == 4)
            {
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (rt > 0)
                    checkmans[e.RowIndex] = rt;
                else
                    MessageBox.Show("审核人id不存在，请重新输入");
                return;
            }


        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            int rtnum = 0xfffff ;
            return rtnum;//如果查找成功返回id，否则返回-1
        }

    }
}
