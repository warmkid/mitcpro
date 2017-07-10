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
    //由生产指令唯一确定该表
    public partial class Record_extrusClean : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access
        int label;//判断是插入数据库还是更新数据库
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
        int instrid;//生产指令id
        int id;//项目记录表中存储的外键

        mySystem.CheckForm checkform;//审核信息

        public Record_extrusClean(mySystem.MainForm mainform)
            : base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            Init();
            //connToServer();
            if (fill_by_id(instrid) == -1)//根据id填表失败，表未填写过
            {
                qury();
                label = 1;//代表是新的填写,保存采用插入数据库方式
            }
            else
                label = 0;//代表是在原来基础上更改，保存采用更新方式


        }
        //获得生产指令id
        private int getinstrid()
        {
            return mySystem.Parameter.proInstruID;
        }

        //初始化
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
            label = 0;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;

            checkBox1.Checked = mySystem.Parameter.userflight;
            checkBox2.Checked = !checkBox1.Checked;
            
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
            string asql = "select 清洁内容 from cleanarea where 清洁区域='" + name + "'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return "";
            else
                return tempdt.Rows[0][0].ToString();
        }

       //供界面显示,参数为数据库中对应记录的id
        public void show(int paraid)
        {
            string asql = "select 生产指令ID from 吹膜机组清洁记录表 where ID=" + paraid;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return;
            else
                fill_by_id((int)tempdt.Rows[0][0]);
        }

        //根据生产指令id将数据填写到各控件中
        private int fill_by_id(int id)
        {
            #region 以前
            //string asql = "select s1_clean_date,s1_flight,s1_reviewer_id,s1_review_date,s1_region_result_cleaner_reviewer from extrusion_s1_cleanrecord where production_instruction="  + id ;
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //DataTable tempdt = new DataTable();
            //da.Fill(tempdt);
            //if (tempdt.Rows.Count == 0)
            //    return -1;
            //else
            //{
            //    //将tempdt填入控件
            //    dateTimePicker1.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());
            //    //comboBox1.Text = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? "白班" : "夜班";
            //    checkBox1.Checked = int.Parse(tempdt.Rows[0][1].ToString()) == 1 ? true : false;
            //    checkBox2.Checked = !checkBox1.Checked;
            //    string rev = tempdt.Rows[0][2].ToString();
            //    if (rev == "")
            //        button2.Enabled = true;
            //    else
            //    {
            //        button1.Enabled = false;
            //        button2.Enabled = false;
            //        textBox2.Text = mySystem.Parameter.IDtoName(int.Parse(rev));
            //        dateTimePicker2.Value = DateTime.Parse(tempdt.Rows[0][3].ToString());
            //        dataGridView1.ReadOnly = true;
            //        dateTimePicker1.Enabled = false;
            //        dateTimePicker2.Enabled = false;
            //    }

            //    string jstr = tempdt.Rows[0][4].ToString();
            //    JArray jarray = JArray.Parse(jstr);
            //    for (int i = 0; i < jarray.Count; i++)
            //    {
            //        JObject jobj = JObject.Parse(jarray[i].ToString());
            //        foreach (var p in jobj)
            //        {
            //            DataGridViewRow dr = new DataGridViewRow();
            //            dataGridView1.Rows.Add(dr);
            //            dataGridView1.Rows[i].Cells[0].Value = p.Key;//名称
            //            dataGridView1.Rows[i].Cells[1].Value =cont_findby_name( p.Key);//内容
            //            if (int.Parse(jobj[p.Key][0].ToString()) == 1)
            //            {
            //                //白班
            //                dataGridView1.Rows[i].Cells[2].Value = "True";
            //                dataGridView1.Rows[i].Cells[3].Value = "False";
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[i].Cells[3].Value = "True";
            //                dataGridView1.Rows[i].Cells[2].Value = "False";
            //            }
            //            dataGridView1.Rows[i].Cells[4].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][1].ToString()));
            //            dataGridView1.Rows[i].Cells[5].Value = mySystem.Parameter.IDtoName(int.Parse(jobj[p.Key][2].ToString()));
            //        }
            //    }
            //}
            //return 0;
            #endregion

            string asql = "select 清洁日期,班次,审核人,审核是否通过,审核时间,ID from 吹膜机组清洁记录表 where 生产指令ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
            {
                //将tempdt填入控件
                dateTimePicker1.Value = DateTime.Parse(tempdt.Rows[0][0].ToString());//清洁日期
                checkBox1.Checked = tempdt.Rows[0][1].ToString()=="True";//班次
                checkBox2.Checked = !checkBox1.Checked;

                bool revisok = (bool)tempdt.Rows[0][3];//审核是否通过

                if (!revisok)//审核未通过
                    button2.Enabled = true;
                else
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    textBox2.Text = tempdt.Rows[0][2].ToString();
                    dateTimePicker2.Value = (DateTime)tempdt.Rows[0][4];
                    dataGridView1.ReadOnly = true;
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                }
                //填写表格
                int tempid = (int)tempdt.Rows[0][5];
                comm.CommandText = "select * from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + tempid;
                da.Dispose();
                tempdt.Dispose();
                da = new OleDbDataAdapter(comm);
                tempdt = new DataTable();
                da.Fill(tempdt);//获得数据

                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dataGridView1.Rows.Add(dr);
                    dataGridView1.Rows[i].Cells[0].Value = (string)tempdt.Rows[i][2];//名称
                    dataGridView1.Rows[i].Cells[1].Value = (string)tempdt.Rows[i][3];//内容
                    dataGridView1.Rows[i].Cells[2].Value = ((bool)tempdt.Rows[i][4]) == true;//合格 是
                    dataGridView1.Rows[i].Cells[3].Value = ((bool)tempdt.Rows[i][4]) == false;
                    dataGridView1.Rows[i].Cells[4].Value = (string)tempdt.Rows[i][6];
                    dataGridView1.Rows[i].Cells[5].Value = (string)tempdt.Rows[i][7];
                }
                return 0;
            }



        }

        //查找清洁区域和清洁内容，并填入表格
        private void qury()
        {
            #region 以前
            ////if (!isOk)
            ////{
            ////    MessageBox.Show("连接数据库失败", "error");
            ////    return;
            ////}
            //if (isSqlOk)
            //{
            //    string sql = "select * from cleanarea";
            //    SqlCommand comm = new SqlCommand(sql, conn);
            //    SqlDataAdapter da = new SqlDataAdapter(comm);

            //    dt = new DataTable();
            //    da.Fill(dt);
            //}
            //else
            //{
            //    string accessql = "select * from cleanarea";
            //    OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
            //    OleDbDataAdapter data = new OleDbDataAdapter(cmd);
            //    dt = new DataTable();
            //    data.Fill(dt);
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = dt.Rows[i][0].ToString();
            //    dr.Cells[1].Value = dt.Rows[i][1].ToString();
            //    dr.Cells[2].Value = true;
            //    //dr.Cells[2].Value = "是".ToString();
            //    dataGridView1.Rows.Add(dr);

            //    //清洁人和审核人均为-1代表初始状态留空
            //    cleanmans.Add(-1);
            //    checkmans.Add(-1);
            //}
            #endregion
            string accessql = "select 清洁区域,清洁内容 from 设置吹膜机组清洁项目";
            OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
            OleDbDataAdapter data = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            data.Fill(dt);
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

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.IsCurrentCellDirty)
            {

                this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

            }
        }

        //单元格编辑结束触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //更改清洁人项
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value == null || dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "")
                    return;
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (rt > 0)
                {
                    //cleanmans[e.RowIndex] = rt;
                }
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
                if (dataGridView1.Rows[e.RowIndex].Cells[5].Value == null || dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "")
                    return;
                int rt = queryid(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (rt > 0)
                {
                    //checkmans[e.RowIndex] = rt;
                }
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


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        //得到最新插入的行的id
        private int getid()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";
            return (int)comm.ExecuteScalar();
        }
        private int getid(int instr)
        {
            string asql = "select ID from 吹膜机组清洁记录表 where 生产指令ID="+instr;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
                return (int)tempdt.Rows[0][0];
        }
        //通过外键获得id
        private DataTable getid_by_outkey(int key)
        {
            string asql = "select ID from 吹膜机组清洁项目记录表 where 吹膜机组清洁记录ID=" + key;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            return tempdt;
        }
        //查找输入清场人和检查人名字是否合法
        private int queryid(string s)
        {
            #region 以前
            ////如果查找成功返回id，否则返回-1
            ////int rtnum = -1;
            //if (mainform.isSqlOk)
            //{
            //    //未完成
            //    //string sql = "select user_id from cleanarea";
            //    //SqlCommand comm = new SqlCommand(sql, conn);
            //    //SqlDataAdapter da = new SqlDataAdapter(comm);

            //    //dt = new DataTable();
            //    //da.Fill(dt);
            //    return -1;
            //}
            //else
            //{
            //    string asql = "select user_id from user_aoxing where user_name=" + "'" + s + "'"; 
            //    OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
            //    OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //    DataTable tempdt = new DataTable();
            //    da.Fill(tempdt);
            //    if (tempdt.Rows.Count == 0)
            //        return -1;
            //    else
            //        return Int32.Parse(tempdt.Rows[0][0].ToString());
            //}
            #endregion
            string asql = "select ID from users where 姓名=" + "'" + s + "'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOleUser);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
                return (int)tempdt.Rows[0][0];

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 之前
            ////缺少生产批次。。。。。。。。。。。。。。。。。。

            //cleantime = dateTimePicker1.Value;
            ////classes = comboBox1.Text.ToString();
            ////checker = textBox2.Text.ToString();
            //checktime = dateTimePicker2.Value;
            //classes = checkBox1.Checked == true ? "白班" : "夜班";
            //int classid;//1代表白班，0代表夜班
            //if (classes == "白班")
            //    classid = 1;
            //else
            //    classid = 0;

            ////添加记录到jason
            //string json = @"[]";
            //JArray jarray = JArray.Parse(json);
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    int a;
            //    string st = "{'";
            //    string t = dataGridView1.Rows[i].Cells[0].Value.ToString() + "':";
            //    st += t;
            //    if (dataGridView1.Rows[i].Cells[2].Value.ToString()=="True")
            //        a = 1;
            //    else
            //        a = 0;
            //    if (dataGridView1.Rows[i].Cells[4].Value==null || dataGridView1.Rows[i].Cells[5].Value==null)
            //    {
            //        MessageBox.Show("清洁人和检查人不能为空");
            //        return;
            //    }
            //    st += "[" + a.ToString() + "," + mySystem.Parameter.NametoID(dataGridView1.Rows[i].Cells[4].Value.ToString()) + "," + mySystem.Parameter.NametoID(dataGridView1.Rows[i].Cells[5].Value.ToString()) + "]}";

            //    JObject temp = JObject.Parse(st);
            //    jarray.Add(temp);
            //}

            ////选择本地还是远程并更新数据到数据库
            //if (isSqlOk)
            //{
            //}
            //else
            //{
            //    int result = 0;
            //    OleDbCommand comm = new OleDbCommand();
            //    comm.Connection = mySystem.Parameter.connOle;
            //    //comm.CommandText = "update extrusion_s1_cleanrecord set s1_clean_date= @cleandate,s1_flight=@flight,s1_reviewer_id=@reviewerid,s1_review_date=@reviewdate,s1_region_result_cleaner_reviewer= @cont where id= @id";
            //    if (label == 1)//插入数据库
            //    {
            //        comm.CommandText = "insert into extrusion_s1_cleanrecord(s1_clean_date,s1_flight,s1_region_result_cleaner_reviewer,production_instruction) values(@cleandate,@flight,@cont,@id)";
            //        label = 0;
            //    }                  
            //    else//更新数据库
            //        comm.CommandText = "update extrusion_s1_cleanrecord set s1_clean_date= @cleandate,s1_flight=@flight,s1_region_result_cleaner_reviewer= @cont where production_instruction= @id";
            //    comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
            //    comm.Parameters.Add("@flight", System.Data.OleDb.OleDbType.Integer);
            //    comm.Parameters.Add("@cont", System.Data.OleDb.OleDbType.VarChar);
            //    comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

            //    comm.Parameters["@cleandate"].Value = cleantime;
            //    comm.Parameters["@flight"].Value = classid;
            //    comm.Parameters["@cont"].Value = jarray.ToString();
            //    comm.Parameters["@id"].Value = instrid;

            //    result = comm.ExecuteNonQuery();
            //    if (result > 0)
            //    {
            //        MessageBox.Show("添加成功");
            //        label = 0;
            //        button2.Enabled = true;
            //    }
            //    else { MessageBox.Show("错误"); }
            //    comm.Dispose();
            //}
            #endregion
            cleantime = dateTimePicker1.Value;//清洁日期
            checktime = dateTimePicker2.Value;//复核日期
            bool classid = checkBox1.Checked == true;//true代表白班，false代表夜班

            int result = 0;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            if (label == 1)//插入数据库
            {
                comm.CommandText = "insert into 吹膜机组清洁记录表(清洁日期,班次,生产指令ID) values(@cleandate,@flight,@id)";

                comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@flight", System.Data.OleDb.OleDbType.Boolean);
                comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

                comm.Parameters["@cleandate"].Value = cleantime;
                comm.Parameters["@flight"].Value = classid;
                comm.Parameters["@id"].Value = instrid;

                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    //插入数据库成功，即刻获取id
                    id = getid();
                    //往项目记录表中插入数据
                    comm.Dispose();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        comm = new OleDbCommand();
                        comm.Connection = mySystem.Parameter.connOle;
                        comm.CommandText = "insert into 吹膜机组清洁项目记录表(吹膜机组清洁记录ID,清洁区域,清洁内容,合格_是,合格_否,清洁人,检查人) values(@instrid,@area,@cont,@yes,@no,@cleaner,@checker)";
                        comm.Parameters.Add("@instrid", System.Data.OleDb.OleDbType.Integer);
                        comm.Parameters.Add("@area", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@cont", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@yes", System.Data.OleDb.OleDbType.Boolean);
                        comm.Parameters.Add("@no", System.Data.OleDb.OleDbType.Boolean);
                        comm.Parameters.Add("@cleaner", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@checker", System.Data.OleDb.OleDbType.VarChar);

                        comm.Parameters["@instrid"].Value = id;
                        comm.Parameters["@area"].Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        comm.Parameters["@cont"].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        Boolean tempbool = dataGridView1.Rows[i].Cells[2].Value.ToString() == "True";
                        comm.Parameters["@yes"].Value = tempbool;
                        comm.Parameters["@no"].Value = !tempbool;
                        if (dataGridView1.Rows[i].Cells[4].Value == null)
                            comm.Parameters["@cleaner"].Value = "";
                        else
                            comm.Parameters["@cleaner"].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();

                        if (dataGridView1.Rows[i].Cells[5].Value == null)
                            comm.Parameters["@checker"].Value = "";
                        else
                            comm.Parameters["@checker"].Value = dataGridView1.Rows[i].Cells[5].Value.ToString();

                        comm.ExecuteNonQuery();
                        comm.Dispose();

                    }

                    MessageBox.Show("添加成功");
                    label = 0;
                    button2.Enabled = true;
                }
                else { MessageBox.Show("错误"); }
                comm.Dispose();

                label = 0;
            }
            else//更新数据库
            {
                comm.CommandText = "update 吹膜机组清洁记录表 set 清洁日期= @cleandate,班次=@flight where 生产指令ID= @id";
                comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
                comm.Parameters.Add("@flight", System.Data.OleDb.OleDbType.Boolean);
                comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

                comm.Parameters["@cleandate"].Value = cleantime;
                comm.Parameters["@flight"].Value = classid;
                comm.Parameters["@id"].Value = instrid;

                result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    //更新外表成功，之后更新内表
                    id = getid(instrid);
                    //获得内表所有的id,按顺序排列
                    DataTable dt_temp=getid_by_outkey(id);//与外键对应的所有内表记录id
                    comm.Dispose();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        comm = new OleDbCommand();
                        comm.Connection = mySystem.Parameter.connOle;
                        comm.CommandText = "update 吹膜机组清洁项目记录表 set 吹膜机组清洁记录ID=@instrid,清洁区域=@area,清洁内容=@cont,合格_是=@yes,合格_否=@no,清洁人=@cleaner,检查人=@checker where ID="+(int)dt_temp.Rows[i][0];
                        comm.Parameters.Add("@instrid", System.Data.OleDb.OleDbType.Integer);
                        comm.Parameters.Add("@area", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@cont", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@yes", System.Data.OleDb.OleDbType.Boolean);
                        comm.Parameters.Add("@no", System.Data.OleDb.OleDbType.Boolean);
                        comm.Parameters.Add("@cleaner", System.Data.OleDb.OleDbType.VarChar);
                        comm.Parameters.Add("@checker", System.Data.OleDb.OleDbType.VarChar);

                        comm.Parameters["@instrid"].Value = id;
                        comm.Parameters["@area"].Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        comm.Parameters["@cont"].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        Boolean tempbool = dataGridView1.Rows[i].Cells[2].Value.ToString() == "True";
                        comm.Parameters["@yes"].Value = tempbool;
                        comm.Parameters["@no"].Value = !tempbool;
                        comm.Parameters["@cleaner"].Value = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        comm.Parameters["@checker"].Value = dataGridView1.Rows[i].Cells[5].Value.ToString();

                        comm.ExecuteNonQuery();
                        comm.Dispose();
                    }

                    MessageBox.Show("更新成功");
                    label = 0;
                    button2.Enabled = true;
                }
                else { MessageBox.Show("错误"); }
            }

            comm.Dispose();


        }

        //重写函数，获得审查人信息
        public override void CheckResult()
        {
            #region 以前
            //base.CheckResult();
            //textBox2.Text = mySystem.Parameter.IDtoName(checkform.userID);//审核人名字
            //dateTimePicker2.Value = checkform.time;

            ////获得上次的记录并更新
            //string asql = "update extrusion_s1_cleanrecord set s1_reviewer_id=@revid,s1_review_date=@revdate,s1_review_opinion=@revopinion,s1_is_review_qualified=@revisok where production_instruction=@id";
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //comm.Parameters.Add("@revid",System.Data.OleDb.OleDbType.Integer);
            //comm.Parameters.Add("@revdate",System.Data.OleDb.OleDbType.Date);
            //comm.Parameters.Add("@revopinion",System.Data.OleDb.OleDbType.VarChar);
            //comm.Parameters.Add("@revisok",System.Data.OleDb.OleDbType.Boolean);
            //comm.Parameters.Add("@id",System.Data.OleDb.OleDbType.Integer);

            //comm.Parameters["@revid"].Value = checkform.userID;
            //comm.Parameters["@revdate"].Value = checkform.time;
            //comm.Parameters["@revopinion"].Value = checkform.opinion;
            //comm.Parameters["@revisok"].Value = checkform.ischeckOk;
            //comm.Parameters["@id"].Value = instrid;

            //int result=comm.ExecuteNonQuery();
            //if(result<0)
            //{
            //    MessageBox.Show("插入数据库发生错误");
            //    return;
            //}
            //button1.Enabled = false;
            //button2.Enabled = false;
            //button3.Enabled = true;
            //dataGridView1.ReadOnly = true;
            //dateTimePicker2.Enabled = false;
            //dateTimePicker1.Enabled = false;
            #endregion

            base.CheckResult();
            textBox2.Text = checkform.userName;//审核人名字
            dateTimePicker2.Value = checkform.time;

            //获得上次的记录并更新
            string asql = "update 吹膜机组清洁记录表 set 审核人=@revid,审核时间=@revdate,审核意见=@revopinion,审核是否通过=@revisok where 生产指令ID=@id";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            comm.Parameters.Add("@revid", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@revdate", System.Data.OleDb.OleDbType.Date);
            comm.Parameters.Add("@revopinion", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@revisok", System.Data.OleDb.OleDbType.Boolean);
            comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

            comm.Parameters["@revid"].Value = checkform.userName;
            comm.Parameters["@revdate"].Value = checkform.time;
            comm.Parameters["@revopinion"].Value = checkform.opinion;
            comm.Parameters["@revisok"].Value = checkform.ischeckOk;
            comm.Parameters["@id"].Value = instrid;

            int result = comm.ExecuteNonQuery();
            if (result < 0)
            {
                MessageBox.Show("插入数据库发生错误");
                return;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            dataGridView1.ReadOnly = true;
            dateTimePicker2.Enabled = false;
            dateTimePicker1.Enabled = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean s = new Record_extrusSiteClean(mainform);
            s.Show();
        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}
    }
}
