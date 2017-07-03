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
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusClean : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access
        int label = 2;//判断选中列
        //private ExtructionProcess extructionformfather = null;

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

        List<int> cleanmans;
        List<int> checkmans;
        int instrid;

        public Record_extrusClean(mySystem.MainForm mainform)
            : base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            Init();
            //connToServer();
            qury();

        }
        //获得生产指令id
        private int getinstrid()
        {
            return mySystem.Parameter.proInstruID;
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

            dataGridView1.Font = new Font("宋体", 10);
            button2.Enabled = false;
            textBox2.Enabled = false;

            instrid = getinstrid();
            
            
        }
        public void connToServer()
        {
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }
        //通过清洁名称找到清洁内容
        private string cont_findby_name(string name)
        {
            string asql = "select 清洁内容 from cleanarea where 清洁区域='"  + name+"'" ;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return "";
            else
                return tempdt.Rows[0][0].ToString();
        }
        //根据生产指令id将数据填写到各控件中
        private void fill_by_id(int id)
        {
            string asql = "select s1_clean_date,s1_flight,s1_reviewer_id,s1_review_date,s1_region_result_cleaner_reviewer from extrusion_s1_cleanrecord where production_instruction="  + id ;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return;
            else
            {
                //将tempdt填入控件
                dateTimePicker1.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());
                comboBox1.Text = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? "白班" : "夜班";
                textBox2.Text = mySystem.Parameter.IDtoName(int.Parse(tempdt.Rows[0][2].ToString()));
                dateTimePicker2.Value = DateTime.Parse(tempdt.Rows[0][3].ToString());

                string jstr = tempdt.Rows[0][4].ToString();
                JArray jarray = JArray.Parse(jstr);
                for (int i = 0; i < jarray.Count; i++)
                {
                    JObject jobj = JObject.Parse(jarray[i].ToString());
                    foreach (var p in jobj)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = p.Key;//名称
                        dataGridView1.Rows[i].Cells[1].Value =cont_findby_name( p.Key);//内容
                        if (int.Parse(jobj[p.Key][0].ToString()) == 1)
                        {
                            //白班
                            dataGridView1.Rows[i].Cells[2].Value = "True";
                            dataGridView1.Rows[i].Cells[3].Value = "False";
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[3].Value = "True";
                            dataGridView1.Rows[i].Cells[2].Value = "False";
                        }
                        dataGridView1.Rows[i].Cells[4].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][1].ToString()));
                        dataGridView1.Rows[i].Cells[5].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][2].ToString()));
                    }
                }
            }
            
        }
        private void qury()
        {
            //if (!isOk)
            //{
            //    MessageBox.Show("连接数据库失败", "error");
            //    return;
            //}
            if (isSqlOk)
            {
                string sql = "select * from cleanarea";
                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);

                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                string accessql = "select * from cleanarea";
                OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
                OleDbDataAdapter data = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                data.Fill(dt);
            }
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
           
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 2)
            {
                string a = dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString();
                if (a == "False")
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "True";
                else
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "False";
                return;
            }

            if (e.ColumnIndex == 3)
            {
                string a = dataGridView1.Rows[e.RowIndex].Cells[3].EditedFormattedValue.ToString();
                if (a == "False")
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "True";
                else
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "False";
                return;
            }
            //更改清洁人项
            if (e.ColumnIndex == 4)
            {
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (rt > 0)
                    cleanmans[e.RowIndex] = rt;
                else
                {
                    MessageBox.Show("清洁人id不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = "";
                }

                return;
            }
            //更改审核人项
            if (e.ColumnIndex == 5)
            {
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (rt > 0)
                    checkmans[e.RowIndex] = rt;
                else
                {
                    MessageBox.Show("审核人id不存在，请重新输入");
                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = "";
                }

                return;
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
            //if (e.ColumnIndex == 2 && label==2)
            //{
            //    string a = dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString();
            //    if(a=="False")
            //        dataGridView1.Rows[e.RowIndex].Cells[3].Value = "True";
            //    else
            //        dataGridView1.Rows[e.RowIndex].Cells[3].Value="False";
            //    label = 3;
            //    return;
            //}

            //if (e.ColumnIndex == 3 && label==3)
            //{
            //    string a=dataGridView1.Rows[e.RowIndex].Cells[3].EditedFormattedValue.ToString();
            //    if(a=="False")
            //        dataGridView1.Rows[e.RowIndex].Cells[2].Value ="True";
            //    else
            //        dataGridView1.Rows[e.RowIndex].Cells[2].Value = "False";
            //    label = 2;
            //    return;
            //}
            ////更改清洁人项
            //if (e.ColumnIndex == 4)
            //{
            //    int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            //    if (rt > 0)
            //        cleanmans[e.RowIndex] = rt;
            //    else
            //    {
            //        MessageBox.Show("清洁人id不存在，请重新输入");
            //        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "";
            //    }
                    
            //    return;
            //}
            ////更改审核人项
            //if (e.ColumnIndex == 5)
            //{
            //    int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
            //    if (rt > 0)
            //        checkmans[e.RowIndex] = rt;
            //    else
            //    {
            //        MessageBox.Show("审核人id不存在，请重新输入");
            //        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "";
            //    }
                    
            //    return;
            //}

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //缺少生产批次。。。。。。。。。。。。。。。。。。

            cleantime = dateTimePicker1.Value;
            classes = comboBox1.Text.ToString();
            checker = textBox2.Text.ToString();
            checktime = dateTimePicker2.Value;

            if (classes == "" || checker == "")
            {
                MessageBox.Show("班次和复核人均不能为空");
                return;
            }

            int classid;//1代表白班，0代表夜班
            int checkerid = queryid(checker);//获取复核人id
            if (checkerid < 0)
            {
                MessageBox.Show("复核人id不存在！");
                return;
            }

            if (classes == "白班")
                classid = 1;
            else
                classid = 0;

            //添加记录到jason
            string json = @"[]";
            JArray jarray = JArray.Parse(json);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int a;
                string st = "{'";
                string t = dataGridView1.Rows[i].Cells[0].Value.ToString() + "':";
                st += t;
                if (dataGridView1.Rows[i].Cells[2].Value.ToString()=="True")
                    a = 1;
                else
                    a = 0;
                st += "[" + a.ToString() + "," + cleanmans[i].ToString() + "," + checkmans[i].ToString() + ",]}";

                JObject temp = JObject.Parse(st);
                jarray.Add(temp);
            }

            //选择本地还是远程并更新数据到数据库
            if (isSqlOk)
            {
                //string s = "update extrusion set s1_clean_date='" + cleantime + "',s1_flight=" + classes + ",s1_reviewer_id='" + int.Parse(checker) + "',s1_review_date='" + checktime + "',s1_region_content_result_cleaner_reviewer='" + j.ToString() + "',step_status=" + status + " where id=1";
                int result = 0;
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "update extrusion set s1_clean_date= @cleandate,s1_flight=@flight,s1_reviewer_id=@reviewerid,s1_review_date=@reviewdate,s1_region_content_result_cleaner_reviewer= @cont where id=@id";
                comm.Parameters.Add("@cleandate", System.Data.SqlDbType.Date);
                comm.Parameters.Add("@flight", System.Data.SqlDbType.Int);
                comm.Parameters.Add("@reviewerid", System.Data.SqlDbType.Int);
                comm.Parameters.Add("@reviewdate", System.Data.SqlDbType.Date);
                comm.Parameters.Add("@cont", System.Data.SqlDbType.VarChar);
                comm.Parameters.Add("@id", System.Data.SqlDbType.Int);

                comm.Parameters["@cleandate"].Value = cleantime;
                comm.Parameters["@flight"].Value = classid;
                comm.Parameters["@reviewerid"].Value = checkerid;
                comm.Parameters["@reviewdate"].Value = checktime;
                comm.Parameters["@cont"].Value = jarray.ToString();
                comm.Parameters["@id"].Value = 1;

                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }
            else
            {
                int result = 0;
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                //comm.CommandText = "update extrusion_s1_cleanrecord set s1_clean_date= @cleandate,s1_flight=@flight,s1_reviewer_id=@reviewerid,s1_review_date=@reviewdate,s1_region_result_cleaner_reviewer= @cont where id= @id";
                comm.CommandText = "insert into extrusion_s1_cleanrecord(s1_clean_date,s1_flight,s1_reviewer_id,s1_review_date,s1_region_result_cleaner_reviewer,production_instruction) values(@cleandate,@flight,@reviewerid,@reviewdate,@cont,@id)";
                comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@flight", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@reviewerid", System.Data.OleDb.OleDbType.Integer);
                comm.Parameters.Add("@reviewdate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@cont", System.Data.OleDb.OleDbType.VarChar);
                comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

                comm.Parameters["@cleandate"].Value = cleantime;
                comm.Parameters["@flight"].Value = classid;
                comm.Parameters["@reviewerid"].Value = checkerid;
                comm.Parameters["@reviewdate"].Value = checktime;
                comm.Parameters["@cont"].Value = jarray.ToString();
                comm.Parameters["@id"].Value = instrid;

                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("添加成功");
                }
                else { MessageBox.Show("错误"); }
            }
            button2.Enabled = true;


        }

        public override void CheckResult()
        {
            base.CheckResult();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mySystem.CheckForm checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

    }
}
