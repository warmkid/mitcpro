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

        //新版本
        int index = 0;//判断是插入模式还是更新模式，0代表插入，1代表更新
        int id;
        //OleDbConnection connOle;
        private DataTable dt_prodinstr, dt_prodlist,dt_prodlist2;//大表，供料工序清洁项目，吹膜工序清场项目
        private OleDbDataAdapter da_prodinstr, da_prodlist, da_prodlist2;
        private BindingSource bs_prodinstr, bs_prodlist, bs_prodlist2;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist, cb_prodlist2;




        //得到最新插入的行的id
        private int getid()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";
            return (int)comm.ExecuteScalar();
        }
        //根据筛选条件得到id,没有返回-1
        private int getid(int instrid)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 吹膜工序清场记录 where 生产指令ID=" + instrid;

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
                return (int)tempdt.Rows[0][0];
            }
        }

        //初次绑定数据库表,插入模式
        private void bind()
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("吹膜工序清场记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序清场记录 where 1=2", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            DataRow dr = dt_prodinstr.NewRow();
            dr[2] = dr[3] = dr[4] = dr[7] = dr[8] = dr[9] = dr[11] = "";
            dr[5] = dr[10] = DateTime.Now;
            dr[1] = 0;
            dr[6] = dr[12] = false;
            dt_prodinstr.Rows.Add(dr);

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();

            textBox5.DataBindings.Clear();
            textBox6.DataBindings.Clear();
            //textBox3.DataBindings.Clear();

            checkBox1.DataBindings.Clear();


            //BindingSource到控件的绑定
            bind_bs_contr();

            index = 0;
        }
        //再次绑定数据库表，相当与更新
        private void bind2(int id)
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("吹膜工序清场记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜工序清场记录 where ID=" + id, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();

            textBox5.DataBindings.Clear();
            textBox6.DataBindings.Clear();
            checkBox1.DataBindings.Clear();

            //BindingSource到控件的绑定
            bind_bs_contr();

            index = 1;
        }
        private void bind_bs_contr()
        {
            textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令");
            textBox2.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品代码");
            textBox4.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场前产品批号");
            dateTimePicker1.DataBindings.Add("Value", bs_prodinstr.DataSource, "清场日期");

            textBox5.DataBindings.Add("Text", bs_prodinstr.DataSource, "清场人");
            textBox6.DataBindings.Add("Text", bs_prodinstr.DataSource, "检查人");
            checkBox1.DataBindings.Add("Checked", bs_prodinstr.DataSource, "检查结果");
        }

        //datagridview1 初次绑定数据库表,插入模式
        private void bind_list1(int id)
        {
            dt_prodlist.Dispose();
            bs_prodlist.Dispose();
            da_prodlist.Dispose();
            cb_prodlist.Dispose();

            dt_prodlist = new DataTable("吹膜工序清场项目记录");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜工序清场项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            //先读取供料工序清场设置，拷贝到上面table中
            string asql = "select 序号,清场内容 from 设置供料工序清场项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            //默认设置里面和之前改的项目是一致的
            if (dt_prodlist.Rows.Count == 0)//空表,插入
            {
                int rowindex = 1;
                foreach (DataRow dr in tempdt.Rows)
                {

                    DataRow ndr = dt_prodlist.NewRow();
                    ndr[1] = id;
                    ndr[2] = rowindex;
                    // 注意ID不要复制过去了，所以从1开始
                    for (int i = 1; i < dr.Table.Columns.Count; ++i)
                    {
                        //ndr[dr.Table.Columns[i].ColumnName] = dr[dr.Table.Columns[i].ColumnName];
                        ndr[i + 2] = dr[i];
                    }
                    // 这里添加检查是否合格、检查人、审核人等默认信息
                    ndr["清场操作_是"] = true;
                    ndr["清场操作_否"] = false;

                    dt_prodlist.Rows.Add(ndr);
                    rowindex++;
                }
            }
            else
            {
                //替换？
                //for (int i = 0; i < dt_prodlist.Rows.Count; i++)
                //{
                //    dt_prodlist.Rows[i][2] = i+1;
                //    dt_prodlist.Rows[i][3] = tempdt.Rows[i][1];
                //}
            }

            comm.Dispose();
            da.Dispose();

            //DataTable到BindingSource的绑定
            bs_prodlist.DataSource = dt_prodlist;

            //BindingSource到控件的绑定
            dataGridView1.DataSource = bs_prodlist.DataSource;
        }
        //datagridview1 再次绑定数据库表，相当与更新
        private void bind2_list1()
        {
            
        }

        //datagridview2 初次绑定数据库表,插入模式
        private void bind_list2(int id)
        {
            dt_prodlist2.Dispose();
            bs_prodlist2.Dispose();
            da_prodlist2.Dispose();
            cb_prodlist2.Dispose();

            dt_prodlist2 = new DataTable("吹膜工序清场吹膜工序项目记录");
            bs_prodlist2 = new BindingSource();
            da_prodlist2 = new OleDbDataAdapter("select * from 吹膜工序清场吹膜工序项目记录 where 吹膜工序清场记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
            da_prodlist2.Fill(dt_prodlist2);

            //先读取吹膜工序清场设置，拷贝到上面table中
            string asql = "select 序号,清场内容 from 设置吹膜工序清场项目";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            //默认设置里面和之前改的项目是一致的
            if (dt_prodlist2.Rows.Count == 0)//空表,插入
            {
                int rowindex = 1;
                foreach (DataRow dr in tempdt.Rows)
                {

                    DataRow ndr = dt_prodlist2.NewRow();
                    ndr[1] = id;
                    ndr[2] = rowindex;
                    // 注意ID不要复制过去了，所以从1开始
                    for (int i = 1; i < dr.Table.Columns.Count; ++i)
                    {
                        //ndr[dr.Table.Columns[i].ColumnName] = dr[dr.Table.Columns[i].ColumnName];
                        ndr[i + 2] = dr[i];
                    }
                    // 这里添加检查是否合格、检查人、审核人等默认信息
                    ndr["清场操作_是"] = true;
                    ndr["清场操作_否"] = false;

                    dt_prodlist2.Rows.Add(ndr);
                    rowindex++;
                }
            }

            comm.Dispose();
            da.Dispose();

            //DataTable到BindingSource的绑定
            bs_prodlist2.DataSource = dt_prodlist2;

            //BindingSource到控件的绑定
            dataGridView2.DataSource = bs_prodlist2.DataSource;
        }
        //datagridview2 再次绑定数据库表，相当与更新
        private void bind2_list2()
        {

        }

        //查询数据库，获得供料工序和吹膜工序清场列表
        private void queryjob()
        {
            #region 之前
            //unit_serve=new List<string>();
            //unit_exstru=new List<string>();
            //if ( mySystem.Parameter.isSqlOk)
            //{
            //    string sql = "select * from feedingprocess_cleansite";
            //    SqlCommand cmd = new SqlCommand(sql, mainform.conn);
            //    SqlDataAdapter data = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    data.Fill(dt);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        unit_serve.Add(dt.Rows[i][1].ToString());
            //    }
            //    cmd.Dispose();
            //    data.Dispose();
            //    dt.Clear();
            //    sql = "select * from extrusion_cleansite";
            //    cmd = new SqlCommand(sql, mainform.conn);
            //    data = new SqlDataAdapter(cmd);
            //    data.Fill(dt);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        unit_exstru.Add(dt.Rows[i][1].ToString());
            //    }
            //    cmd.Dispose();
            //    data.Dispose();
            //    dt.Clear();
            //    //unit_serve = new string[] { "填写供料记录是否已归档", "使用剩余的原料是否称重退库", "设备是否按程序开机，并切断电源", "设备和工位器具是否已清洁", "生产废弃物是否已清，卫生是否已打扫", "其他" };
            //    //unit_exstru = new string[] { "填写的记录是否已归档", "使用的文件，设备运行参数是否已经归档", "设备是否已按程序关机，并切断电源", "设备和工位器具是否已清洁", "生产的产品是否定置摆放，粘贴标签，登记台账", "生产用半成品标签是否已销毁", "生产废物是否已清，卫生是否已打扫", "其他" };
            //}
            //else
            //{
            //    string accessql = "select * from feedingprocess_cleansite";
            //    OleDbCommand cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
            //    OleDbDataAdapter data = new OleDbDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    data.Fill(dt);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        unit_serve.Add(dt.Rows[i][1].ToString());
            //    }
            //    cmd.Dispose();
            //    data.Dispose();
            //    dt.Clear();
            //    accessql = "select * from extrusion_cleansite";
            //    cmd = new OleDbCommand(accessql, mySystem.Parameter.connOle);
            //    data = new OleDbDataAdapter(cmd);
            //    data.Fill(dt);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        unit_exstru.Add(dt.Rows[i][1].ToString());
            //    }
            //    cmd.Dispose();
            //    data.Dispose();
            //    dt.Clear();
            //}
            #endregion

        }
        private void Init()
        {
            #region 以前
            //cleaner = mainform.userID;
            //checker = 45;

            //prod_instrcode = mySystem.Parameter.proInstruction;
            //System.Console.WriteLine(mySystem.Parameter.proInstruction);

            ////date = "2017/6/10";
            //textBox1.Text = prod_instrcode;
            //comboBox2.Text = "供料工序";


            //dataGridView1.AllowUserToAddRows = false;
            //ischecked_1 = new List<int>();
            //ischecked_2 = new List<int>();

            //for (int i = 0; i < unit_serve.Count; i++)
            //    ischecked_1.Add(1);
            //for (int i = 0; i < unit_exstru.Count; i++)
            //    ischecked_2.Add(1);

            //dataGridView1.Font = new Font("宋体", 10);
            //button2.Enabled = false;
            //textBox6.ReadOnly= true;
            //button3.Enabled = false;
            //label = 0;
            //checkBox1.Enabled = false;
            //checkBox2.Enabled = false;
            //textBox2.Enabled = false;
            //textBox4.Enabled = false;
            #endregion

            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

            dt_prodlist2 = new System.Data.DataTable();
            bs_prodlist2 = new System.Windows.Forms.BindingSource();
            da_prodlist2 = new OleDbDataAdapter();
            cb_prodlist2 = new OleDbCommandBuilder();

            textBox1.Text = mySystem.Parameter.proInstruction;

        }

        //查找清场前产品代码和批号
        private void query_prodandbatch()
        {
            #region 以前
            //string asql = "select product_batch_id,s6_production_date from extrusion_s6_production_check where production_instruction_id=" + mySystem.Parameter.proInstruID + " ORDER BY s6_production_date";
            //OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //DataTable tempdt = new DataTable();
            //da.Fill(tempdt);
            //if (tempdt.Rows.Count == 0)
            //    return;
            //else
            //{
            //    int batchid = int.Parse(tempdt.Rows[0][0].ToString());
            //    comm.CommandText = "select product_code from product_aoxing where product_id in (select product_id from product_batch where product_batch_id="+batchid+")";
            //    OleDbDataAdapter db = new OleDbDataAdapter(comm);
            //    DataTable tempdt2 = new DataTable();
            //    db.Fill(tempdt2);
            //    if (tempdt2.Rows.Count == 0)
            //        return;
            //    else
            //    {
            //        comm.CommandText = "select product_batch_code from product_batch where product_batch_id=" + batchid;
            //        OleDbDataAdapter da2 = new OleDbDataAdapter(comm);
            //        DataTable dt3 = new DataTable();
            //        da2.Fill(dt3);
            //        textBox2.Text = tempdt2.Rows[0][0].ToString();//产品代码
            //        textBox4.Text = dt3.Rows[0][0].ToString();//产品批号
            //        dt3.Dispose();
            //        da2.Dispose();
            //    }
            //    db.Dispose();

            //}
            //da.Dispose();
            //comm.Dispose();
            #endregion
            string asql = "select 产品批号 from 吹膜工序生产和检验记录 where 生产指令ID=" + mySystem.Parameter.proInstruID + " ORDER BY 生产日期";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                comm.Dispose();
                da.Dispose();
                tempdt.Dispose();
                return;
            }               
            else
            {
                textBox4.Text = (string)tempdt.Rows[0][0];//产品批号
                //通过批号查找产品代码7
            }

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

                string jstr = tempdt.Rows[0][3].ToString();
                JArray jarray = JArray.Parse(jstr);
                //第一个选项
                JObject jobj1 = JObject.Parse(jarray[0].ToString());
                int i = 0;
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

                string strrev = tempdt.Rows[0][5].ToString();
                if (strrev == "")
                {
                    button2.Enabled = true; 
                    return -1;
                }
                   
                else
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    textBox6.Text = mySystem.Parameter.IDtoName(int.Parse(strrev));
                    //textBox5.Text=int.Parse(tempdt.Rows[0][5].ToString())>0?"合格":"不合格";
                    //textBox5.Enabled = false;
                    checkBox1.Checked = int.Parse(tempdt.Rows[0][5].ToString()) > 0 ? true : false;
                    checkBox2.Checked = !checkBox1.Checked;

                    dataGridView1.ReadOnly = true;
                    textBox2.Enabled = false;
                    textBox4.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    textBox6.Enabled = false;

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
            #region 以前
            //conn = mainform.conn;
            //connOle = mainform.connOle;
            //isSqlOk = mainform.isSqlOk;

            //InitializeComponent();
            //queryjob();
            //Init();
            //if (fill_by_id(mySystem.Parameter.proInstruID) == -1)//根据id填表失败，表未填写过
            //{
            //    AddtoGridView();
            //    label = 1;//代表是新的填写,保存采用插入数据库方式
            //    query_prodandbatch();
            //}
            //else
            //{
            //    label = 0;//代表是在原来基础上更改，保存采用更新方式
            //    fill_by_id(mySystem.Parameter.proInstruID);
            //}

            #endregion

            InitializeComponent();
            Init();
            //绑定内表
            int outkey = getid(mySystem.Parameter.proInstruID);
            bind_list1(outkey);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            bind_list2(outkey);
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            //bind();

            //先填表,再判断对应审核是否合格
            if (outkey != -1)
            {

                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                comm.CommandText = "select * from 吹膜工序清场记录 where ID="+outkey;
                OleDbDataAdapter data = new OleDbDataAdapter(comm);
                DataTable dtemp = new DataTable();
                data.Fill(dtemp);

                textBox1.Text = (string)dtemp.Rows[0][2];
                textBox2.Text = (string)dtemp.Rows[0][3];
                textBox4.Text = (string)dtemp.Rows[0][4];
                dateTimePicker1.Value = (DateTime)dtemp.Rows[0][5];
                textBox5.Text = (string)dtemp.Rows[0][7];
                textBox6.Text = (string)dtemp.Rows[0][8];
                checkBox1.Checked = (bool)dtemp.Rows[0][6];
                checkBox2.Checked = !checkBox1.Checked;

                if ((bool)dtemp.Rows[0][6])//合格
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                    dataGridView1.ReadOnly = true;
                    dataGridView2.ReadOnly = true;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    //bind2(outkey);
                }
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
            //    OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
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

        private void button1_Click(object sender, EventArgs e)
        {
            #region 以前
            ////cleaner = textBox4.Text;
            //string strcleaner = textBox5.Text;
            ////string strchecker = textBox6.Text;
            //if (strcleaner == "")
            //{
            //    MessageBox.Show("清场人不能为空");
            //    return;
            //}
            //cleaner = queryid(strcleaner);
            ////checker = queryid(strchecker);
            //if (cleaner == -1)
            //{
            //    MessageBox.Show("清场人id不存在");
            //    return;
            //}

            ////生产指令先用a代替
            //int a = mySystem.Parameter.proInstruID;

            //prod_code=textBox2.Text;
            //prod_batch=textBox4.Text;
            //date=dateTimePicker1.Value;

            ////checkout = comboBox1.Text == "合格";
            ////checker = textBox5.Text;
            //extr = textBox3.Text;

            ////添加记录到jason
            //string json = @"[]";
            //JArray jarray = JArray.Parse(json);
            //string st = "{}";
            //JObject temp1 = JObject.Parse(st);
            //JObject temp2 = JObject.Parse(st);
            //for (int i = 0; i < unit_serve.Count; i++)
            //{
            //    temp1.Add(unit_serve[i],new JValue(ischecked_1[i].ToString()));
            //}
            //for (int i = 0; i < unit_exstru.Count; i++)
            //{
            //    temp2.Add(unit_exstru[i],new JValue(ischecked_2[i].ToString()));
            //}
            //jarray.Add(temp1);
            //jarray.Add(temp2);       

            ////插入数据库
            //int result = 0;
            //if (mySystem.Parameter.isSqlOk)
            //{
            //}
            //else
            //{
            //    OleDbCommand comm = new OleDbCommand();
            //    comm.Connection = mySystem.Parameter.connOle;
            //    if (label == 1)
            //    {
            //        comm.CommandText = "insert into clean_record_of_extrusion_process(product_id_before,product_batch_before,clean_date,cleaner_id,is_cleaned,production_instruction_id) values(@beforeid,@beforebatch,@cleandate,@cleanerid,@cleancont,@id)";
            //        label = 0;
            //    }
                    
            //    else
            //        comm.CommandText = "update clean_record_of_extrusion_process set product_id_before=@beforeid,product_batch_before= @beforebatch,clean_date=@cleandate,cleaner_id=@cleanerid,is_cleaned=@cleancont where production_instruction_id= @id";
            //    comm.Parameters.Add("@beforeid", System.Data.OleDb.OleDbType.VarChar);
            //    comm.Parameters.Add("@beforebatch", System.Data.OleDb.OleDbType.VarChar);
            //    comm.Parameters.Add("@cleandate", System.Data.OleDb.OleDbType.Date);
            //    comm.Parameters.Add("@cleanerid", System.Data.OleDb.OleDbType.Integer);
            //    comm.Parameters.Add("@cleancont", System.Data.OleDb.OleDbType.VarChar);
            //    comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

            //    comm.Parameters["@beforeid"].Value = prod_code;
            //    comm.Parameters["@beforebatch"].Value = prod_batch;
            //    comm.Parameters["@cleandate"].Value = date;
            //    comm.Parameters["@cleanerid"].Value = cleaner;
            //    comm.Parameters["@cleancont"].Value = jarray.ToString();
            //    comm.Parameters["@id"].Value = a;
            //    result = comm.ExecuteNonQuery();
            //}
            //if (result > 0)
            //{
            //    MessageBox.Show("添加成功");
            //}
            //else { MessageBox.Show("错误"); }
            //button2.Enabled = true;
            #endregion

            //判断数据合法性，只有清场人
            int userid = queryid(textBox5.Text);
            if (userid == -1)
            {
                MessageBox.Show("清场人id不存在");
                return;
            }
            //判断是更新还是插入
            id = getid(mySystem.Parameter.proInstruID);
            if (id == -1)//进行插入
            {
                //保存原来控件中的值
                string instrcode = textBox1.Text;
                string prodcode = textBox2.Text;
                string prodbatch = textBox4.Text;
                DateTime date = dateTimePicker1.Value;
                string cleaner = textBox5.Text;

                bind();//重新绑定后数据清空

                // 保存非DataGridView中的数据必须先执行EndEdit;
                dt_prodinstr.Rows[0][1] = mySystem.Parameter.proInstruID;
                dt_prodinstr.Rows[0][2] = instrcode;
                dt_prodinstr.Rows[0][3] = prodcode;
                dt_prodinstr.Rows[0][4] = prodbatch;
                dt_prodinstr.Rows[0][5] = date;
                dt_prodinstr.Rows[0][7] = cleaner;
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                id = getid();//获得刚刚插入的记录的id

                for (int i = 0; i < dt_prodlist.Rows.Count; i++)
                {
                    dt_prodlist.Rows[i][1] = id;
                }
                for (int i = 0; i < dt_prodlist2.Rows.Count; i++)
                {
                    dt_prodlist2.Rows[i][1] = id;
                }
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);

                bind_list1(id);
                bind_list2(id);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                //更新datagridview1
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
                //fill(id);
                MessageBox.Show("添加成功");
            }
            else//进行更新
            {
                if (index == 0)
                {
                    bind2(id);
                    bind_list1(id);
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    bind_list2(id);
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                }
                    
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);


                //更新datagridview1
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
                da_prodlist2.Update((DataTable)bs_prodlist2.DataSource);
                //fill(id);
                MessageBox.Show("更新成功");
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

        //重写函数，获得审核信息
        public override void CheckResult()
        {
            #region 以前
            //base.CheckResult();
            //textBox6.Text = checkform.userName;
            ////comboBox1.Text = checkform.ischeckOk==true?"合格":"不合格";
            //checkBox1.Checked = checkform.ischeckOk;
            //checkBox2.Checked = !checkBox1.Checked;
            //OleDbCommand comm = new OleDbCommand();
            //comm.Connection = mySystem.Parameter.connOle;
            //comm.CommandText = "update clean_record_of_extrusion_process set reviewer_id= @revid,review_opinion=@revopinion,is_review_qualified= @isok where production_instruction_id= @id";
            //comm.Parameters.Add("@revid", System.Data.OleDb.OleDbType.Integer);
            //comm.Parameters.Add("@revopinion", System.Data.OleDb.OleDbType.VarChar);
            //comm.Parameters.Add("@isok", System.Data.OleDb.OleDbType.Boolean);
            //comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);

            //comm.Parameters["@revid"].Value = checkform.userID;
            //comm.Parameters["@revopinion"].Value = checkform.opinion;
            //comm.Parameters["@isok"].Value = checkform.ischeckOk;
            //comm.Parameters["@id"].Value = mySystem.Parameter.proInstruID;

            //int result = comm.ExecuteNonQuery();
            //if (result <= 0)
            //{
            //    MessageBox.Show("审核出错");
            //    return;
            //}
            //button3.Enabled = true;
            #endregion
            base.CheckResult();
            textBox6.Text = checkform.userName;
            dt_prodinstr.Rows[0][8] = checkform.userName;
            checkBox1.Checked = checkform.ischeckOk;
            checkBox2.Checked = !checkBox1.Checked;
            dt_prodinstr.Rows[0][6] = checkform.ischeckOk;
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            button3.Enabled = true;

            if (checkform.ischeckOk)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                dataGridView1.ReadOnly = true;
                dataGridView2.ReadOnly = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                dateTimePicker1.Enabled = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

    }
}
