using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.OleDb;

namespace mySystem
{
    /// <summary>
    ///  吹膜生产日报表
    /// </summary>
    public partial class ProdctDaily_extrus : mySystem.BaseForm
    {
        public ProdctDaily_extrus(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            Init();
        }
        DataTable dt;

        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;

        //针对查询
        DateTime date1;//起始时间
        DateTime date2;//结束时间

        //自定义函数
        private void Init()
        {

            dataGridView1.Font = new Font("宋体", 12);

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;


            dt = new DataTable();

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();


            //dt.Columns.Add("id",System.Type.GetType("System.Int32"));
            dt.Columns.Add("date", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("classes", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("prodcode", System.Type.GetType("System.String"));
            dt.Columns.Add("prodbatch", System.Type.GetType("System.String"));
            dt.Columns.Add("number", System.Type.GetType("System.String"));
            dt.Columns.Add("count", System.Type.GetType("System.String"));
            dt.Columns.Add("weight", System.Type.GetType("System.String"));
            dt.Columns.Add("mA", System.Type.GetType("System.String"));
            dt.Columns.Add("mB1C", System.Type.GetType("System.String"));
            dt.Columns.Add("mB2", System.Type.GetType("System.String"));
            dt.Columns.Add("time", System.Type.GetType("System.String"));

            //query_by_instru(mySystem.Parameter.proInstruction);
            begin();

        }

        private void begin()
        {
            readOuterData(mySystem.Parameter.proInstruID);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(mySystem.Parameter.proInstruID);
                removeOuterBinding();
                outerBind();

                query_by_instru(mySystem.Parameter.proInstruction);
            }

            readInnerData((int)dt_prodlist.Rows[0]["ID"]);
            innerBind();
        }


        //查找同一条生产指令下的数据
        private void query_by_instru(string instru_code)
        {
            #region 以前
            //if (mainform.isSqlOk)
            //{
    
            //}
            //else
            //{
            //    //通过生产指令代码找到对应的生产指令id
            //    string acsql = "select production_instruction_id from production_instruction where production_instruction_code='"+instru_code+"'";
            //    OleDbCommand comm = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            //    OleDbDataAdapter da = new OleDbDataAdapter(comm);
            //    DataTable dt1 = new DataTable();
            //    da.Fill(dt1);
                
            //    int id = int.Parse(dt1.Rows[0][0].ToString());//获得生产指令id
            //    da.Dispose();

            //    //通过生产指令id查找相应的子集
            //    acsql = "select product_batch_id,s6_production_date,s6_flight,s6_mojuan_number,s6_mojuan_length,s6_mojuan_weight,s6_time,s6_recorder_id,s6_reviewer_id from extrusion_s6_production_check where production_instruction_id="+id;
            //    comm.CommandText = acsql;
            //    da = new OleDbDataAdapter(comm);
            //    DataTable dt2 = new DataTable();
            //    da.Fill(dt2);
            //    da.Dispose();

            //    acsql = "select product_batch_id,s5_feeding_info from extrusion_s5_feeding where production_instruction_id="+id;
            //    comm.CommandText = acsql;
            //    da = new OleDbDataAdapter(comm);
            //    DataTable dt3 = new DataTable();
            //    da.Fill(dt3);

            //    //子集通过连接生成最后的联合表
            //    var query = from r in dt2.AsEnumerable()
            //                join s in dt3.AsEnumerable()
            //                on r.Field<int>("product_batch_id") equals s.Field<int>("product_batch_id") into temp
            //                from t in temp.DefaultIfEmpty()
            //                select new
            //                {
            //                    batch_id = r.Field<int>("product_batch_id").ToString(),
            //                    date=r.Field<DateTime>("s6_production_date").ToShortDateString(),
            //                    flight = r.Field<bool>("s6_flight").ToString(),
            //                    number=r.Field<string>("s6_mojuan_number"),
            //                    length = r.Field<int>("s6_mojuan_length").ToString(),
            //                    weight = r.Field<int>("s6_mojuan_weight").ToString(),
            //                    time = r.Field<DateTime>("s6_time").ToString(),
            //                    recid = r.Field<int>("s6_recorder_id").ToString(),
            //                    revid = r.Field<int>("s6_reviewer_id").ToString(),
            //                    feedinfo=t!=null ? t.Field<string>("s5_feeding_info") : "***"
            //                };
            //    var query_r = from r in dt3.AsEnumerable()
            //                join s in dt2.AsEnumerable()
            //                on r.Field<int>("product_batch_id") equals s.Field<int>("product_batch_id") into temp
            //                from t in temp.DefaultIfEmpty()
            //                select new
            //                {
            //                    batch_id = r.Field<int>("product_batch_id").ToString(),
            //                    date=t!=null?t.Field<DateTime>("s6_production_date").ToShortDateString():"***",
            //                    flight = t!=null? t.Field<bool>("s6_flight").ToString():"***",
            //                    number=t!=null? t.Field<string>("s6_mojuan_number"):"***",
            //                    length = t!=null? t.Field<int>("s6_mojuan_length").ToString():"***",
            //                    weight = t!=null? t.Field<int>("s6_mojuan_weight").ToString():"***",
            //                    time = t!=null? t.Field<DateTime>("s6_time").ToString():"***",
            //                    recid = t!=null? t.Field<int>("s6_recorder_id").ToString():"***",
            //                    revid = t!=null? t.Field<int>("s6_reviewer_id").ToString():"***",
            //                    feedinfo = r.Field<string>("s5_feeding_info")
            //                };
            //    var fullquery = query.Union(query_r);//最后查找的结果
                
            //    //将结果填入表格中和dt中
            //    dt.Clear();
            //    int index = 0;
            //    foreach (var item in fullquery)
            //    {
            //        Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", item.batch_id, item.date, item.flight, item.number,item.length, item.weight, item.time, item.recid, item.revid, item.feedinfo);
            //        DataGridViewRow dr = new DataGridViewRow();
            //        foreach (DataGridViewColumn c in dataGridView1.Columns)
            //        {
            //            dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //        }
                    
            //        dr.Cells[0].Value = index+1;
            //        dr.Cells[1].Value = item.date;
            //        dr.Cells[2].Value = item.flight=="True";
            //        dr.Cells[3].Value = prodcode_findby_batchid(int.Parse(item.batch_id));//产品代码
            //        dr.Cells[4].Value = batchcode_findby_batchid(int.Parse(item.batch_id));//批号代码
            //        dr.Cells[5].Value = item.number;//卷号
            //        dr.Cells[6].Value = item.length;
            //        dr.Cells[7].Value = item.weight;
            //        dr.Cells[8].Value = "";//废品重量
            //        dr.Cells[9].Value = item.feedinfo;//加料A
            //        dr.Cells[10].Value = item.feedinfo;//加料B1+C
            //        dr.Cells[11].Value = item.feedinfo;//加料B2
            //        dr.Cells[12].Value = item.time;//工时
            //        if (item.recid == "***")
            //            dr.Cells[13].Value = item.recid;
            //        else
            //            dr.Cells[13].Value = name_findby_userid(int.Parse(item.recid));
            //        if (item.revid == "***")
            //            dr.Cells[14].Value = item.revid;
            //        else
            //            dr.Cells[14].Value = name_findby_userid(int.Parse(item.revid));

            //        dataGridView1.Rows.Add(dr);

            //        DataRow drow = dt.NewRow();
            //        if (item.date == "***")
            //            drow["date"] = DateTime.Parse("1900-1-1 0:00:00");
            //        else
            //            drow["date"] = DateTime.Parse(item.date);
            //        drow["classes"] = item.flight == "True";
            //        drow["prodcode"] = dr.Cells[3].Value;
            //        drow["prodbatch"] = dr.Cells[4].Value;
            //        drow["number"] = item.number;
            //        drow["count"] = item.length;
            //        drow["weight"] = item.weight;
            //        drow["mA"] = item.feedinfo;
            //        drow["mB1C"] = item.feedinfo;
            //        drow["mB2"] = item.feedinfo;
            //        drow["time"] = item.time;
            //        drow["rec"] = dr.Cells[13].Value;
            //        drow["rev"] = dr.Cells[14].Value;
            //        dt.Rows.Add(drow);

            //        index = index + 1;
            //    }
                    
                
            //    //查询条件可见
            //    setvisible(true);
            //    flag = true;

            //    //往查询条件中添加目录
            //    comboBox1.Items.Clear();
            //    comboBox3.Items.Clear();
            //    comboBox4.Items.Clear();
            //    comboBox1.Items.Add("(空)");
            //    comboBox3.Items.Add("(空)");
            //    comboBox4.Items.Add("(空)");

            //    DistinctValueCount(dt, "prodcode", comboBox1);
            //    DistinctValueCount(dt, "rec", comboBox3);
            //    DistinctValueCount(dt, "rev", comboBox4);
            //}
            #endregion

            string acsql = "";
            int id = mySystem.Parameter.proInstruID;//获得生产指令id

            DataTable dt_检验记录 = new DataTable();
            DataTable dt_废品记录 = new DataTable();
            DataTable dt_供料记录 = new DataTable();
            DataTable dt_out = new DataTable();
            //检验记录
            acsql = "select * from 吹膜工序生产和检验记录 where 生产指令ID=" + id + " order by 生产日期"; ;
            OleDbCommand comm2 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            OleDbDataAdapter da2 = new OleDbDataAdapter(comm2);
            da2.Fill(dt_检验记录);          

            //废品记录
            acsql = "select * from 吹膜工序废品记录详细信息 where T吹膜工序废品记录ID=(select ID from 吹膜工序废品记录 where 生产指令ID="+id+")";
            OleDbCommand comm3 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            OleDbDataAdapter da3 = new OleDbDataAdapter(comm3);
            da3.Fill(dt_废品记录);

            //供料记录
            acsql = "select * from 吹膜供料记录 where 生产指令ID=" + id;
            OleDbCommand comm4 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            OleDbDataAdapter da4 = new OleDbDataAdapter(comm4);
            da4.Fill(dt_供料记录);

            //根据产品代码和产品批号进行联合,以检验记录的行为标准
            for (int i = 0; i < dt_检验记录.Rows.Count; i++)
            {
                //检验记录详细信息
                DataTable dt_检验记录_详细 = new DataTable();
                acsql = "select count(膜卷编号),sum(膜卷长度),sum(膜卷重量) from 吹膜工序生产和检验记录详细信息 where T吹膜工序生产和检验记录ID=" + (int)dt_检验记录.Rows[i][0];
                OleDbCommand comm2_new = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                OleDbDataAdapter da2_new = new OleDbDataAdapter(comm2_new);
                da2_new.Fill(dt_检验记录_详细);
                comm2_new.Dispose();
                da2_new.Dispose();
                

                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                string date_temp1 = ((DateTime)dt_检验记录.Rows[i]["生产日期"]).ToShortDateString();//标准
                string code_temp1 = dt_检验记录.Rows[i]["产品名称"].ToString();//之前表上名字错误，应该是产品代码

                dr.Cells[0].Value = i+1;//序号
                dr.Cells[1].Value = date_temp1;//生产日期，具体到天
                dr.Cells[2].Value = (bool)dt_检验记录.Rows[i]["班次"];
                dr.Cells[3].Value = code_temp1;//产品代码
                dr.Cells[4].Value = dt_检验记录.Rows[i]["产品批号"].ToString();
                dr.Cells[5].Value = float.Parse(dt_检验记录_详细.Rows[0][0].ToString());//卷号
                dr.Cells[6].Value = float.Parse(dt_检验记录_详细.Rows[0][1].ToString());//生产数量
                dr.Cells[7].Value = float.Parse(dt_检验记录_详细.Rows[0][2].ToString());//生产重量
                dr.Cells[14].Value = dt_检验记录.Rows[i]["审核人"].ToString();//复核人


                //查找废品记录中对应的记录
                float sum_废品重量 = 0;
                for (int j = 0; j < dt_废品记录.Rows.Count; j++)
                {                   
                    string date_temp2=((DateTime)dt_废品记录.Rows[j]["生产日期"]).ToShortDateString();                    
                    string code_temp2 = dt_废品记录.Rows[j]["产品代码"].ToString();

                    if (date_temp1 == date_temp2 && code_temp1 == code_temp2)
                    {
                        sum_废品重量 += (float)dt_废品记录.Rows[j]["不良品数量"];
                    }
                }
                dr.Cells[8].Value = sum_废品重量;//废品重量

                DataTable dt_废品记录_工时 = new DataTable();
                acsql = "select 生产开始时间,生产结束时间,记录人 from 吹膜工序废品记录 where 生产指令ID=" + id;
                OleDbCommand comm2_new_time = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                OleDbDataAdapter da2_new_time = new OleDbDataAdapter(comm2_new_time);
                da2_new_time.Fill(dt_废品记录_工时);
                comm2_new_time.Dispose();
                da2_new_time.Dispose();

                dr.Cells[12].Value = (DateTime)dt_废品记录_工时.Rows[0][1] - (DateTime)dt_废品记录_工时.Rows[0][0];//工时
                dr.Cells[13].Value = dt_废品记录_工时.Rows[0][2].ToString();//填报人

                //查找供料记录中对应的记录，由于查看的是当天的产品代码下供料合计，不能直接读合计
                float sum_A = 0, sum_B1C = 0, sum_B2 = 0;
                for (int k = 0; k < dt_供料记录.Rows.Count; k++)
                {
                    if (dt_供料记录.Rows[k]["产品代码"].ToString() == code_temp1)
                    {
                        DataTable dt_供料记录_加料量 = new DataTable();
                        acsql = "select 外层供料量,中内层供料量,中层供料量 from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + (int)dt_供料记录.Rows[k]["ID"];
                        OleDbCommand comm2_new_jialiao = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                        OleDbDataAdapter da2_new_jialiao = new OleDbDataAdapter(comm2_new_jialiao);
                        da2_new_jialiao.Fill(dt_供料记录_加料量);
                        comm2_new_jialiao.Dispose();
                        da2_new_jialiao.Dispose();

                        for (int p = 0; p < dt_供料记录_加料量.Rows.Count; p++)
                        {
                            string temptime=((DateTime)dt_供料记录_加料量.Rows[p]["供料时间"]).ToShortDateString();
                            if (date_temp1 == temptime)
                            {
                                sum_A += float.Parse(dt_供料记录_加料量.Rows[p]["外层供料量"].ToString());
                                sum_B1C += float.Parse(dt_供料记录_加料量.Rows[p]["中内层供料量"].ToString());
                                sum_B2 += float.Parse(dt_供料记录_加料量.Rows[p]["中层供料量"].ToString());
                            }
                        }
                    }
                    break;
                }

                dr.Cells[9].Value = sum_A;//加料A
                dr.Cells[10].Value = sum_B1C;//加料B1C
                dr.Cells[11].Value = sum_B2;//加料B2

                dataGridView1.Rows.Add(dr);
            }

            

        }

        //统计datatable中某一列不同值，并将其填入控件中
        private int DistinctValueCount(DataTable dt, string colname,ComboBox combox)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object tmp = dt.Rows[i][colname];
                if (!ht.Contains(tmp))
                {
                    ht.Add(tmp, tmp);
                    combox.Items.Add(tmp.ToString());
                }
            }

            return ht.Count;
        }

        //通过查询条件来查
        private void query_by_condt()
        {
            //DateTime date1 = dateTimePicker1.Value.Date;
            //DateTime date2 = dateTimePicker2.Value.Date;
            //TimeSpan delt = date2 - date1;
            //if (delt.TotalDays < 0)
            //{
            //    MessageBox.Show("起止时间有误，请重新输入");
            //    return;
            //}


            //////删除合计行
            ////dt.Rows.RemoveAt(dt.Rows.Count - 1);

            //string sql = "date>=" + "'" + date1 + "'" + " and " + "date<=" + "'" + date2 + "'";

            //if (prodcode != "(空)")
            //    sql += " and " + "prodcode like" + "'%" + prodcode + "%'";
            //if (writer!= "(空)")
            //    sql += " and " + "rec like" + "'%" + writer + "%'";
            //if (checker != "(空)")
            //    sql += " and " + "rev like" + "'%" + checker + "%'";
            //DataRow[] arrayDR=dt.Select(sql);

            ////清空表格
            //while (dataGridView1.Rows.Count > 0)
            //    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            ////填充
            //int i=0;
            //foreach(DataRow drow in arrayDR)
            //{
            //     DataGridViewRow dr = new DataGridViewRow();
            //    foreach (DataGridViewColumn c in dataGridView1.Columns)
            //    {
            //        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            //    }
            //    dr.Cells[0].Value = i + 1;
            //    dr.Cells[1].Value = drow[0];
            //    dr.Cells[2].Value = drow[1];
            //    dr.Cells[3].Value = drow[2];
            //    dr.Cells[4].Value = drow[3];
            //    dr.Cells[5].Value = drow[4];
            //    dr.Cells[6].Value = drow[5];
            //    dr.Cells[7].Value = drow[6];
            //    dr.Cells[8].Value = "";//废品重量
            //    dr.Cells[9].Value = drow[7]; //加料A
            //    dr.Cells[10].Value = drow[8]; //加料B1+C
            //    dr.Cells[11].Value = drow[9]; //加料B2
            //    dr.Cells[12].Value = drow[10]; //工时
            //    dr.Cells[13].Value = drow[11];
            //    dr.Cells[14].Value = drow[12];
            //    dataGridView1.Rows.Add(dr);
            //}

        }

        private void queryAndShow()
        {            

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产指令"] = mySystem.Parameter.proInstruction;
            dr["生产数量合计"] = 0;
            dr["生产重量合计"] = 0;
            dr["废品重量合计"] = 0;
            dr["加料A合计"] = 0;
            dr["加料B1C合计"] = 0;
            dr["加料B2合计"] = 0;
            dr["工时合计"] = 0;
            dr["中层B2物料占比"] = 0;
            dr["工时效率"] = 0;

            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T吹膜生产日报表ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["生产时间"] = DateTime.Now;

            dr["班次"] = true;
            dr["生产数量"] = 0;
            dr["生产重量"] = 0;
            dr["废品重量"] = 0;
            dr["加料A"] = 0;
            dr["加料B1C"] = 0;
            dr["加料B2"] = 0;
            dr["工时"] = 0;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("吹膜生产日报表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜生产日报表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜生产日报表详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜生产日报表详细信息 where T吹膜生产日报表ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb生产指令.DataBindings.Clear();
            tb中层B2物料占比.DataBindings.Clear();
            tb工时效率.DataBindings.Clear();
            tb生产数量.DataBindings.Clear();
            tb生产重量.DataBindings.Clear();
            tb废品重量.DataBindings.Clear();
            tb加料A.DataBindings.Clear();
            tb加料B1C.DataBindings.Clear();
            tb加料B2.DataBindings.Clear();
            tb工时.DataBindings.Clear();
            tb备注.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令");
            tb中层B2物料占比.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层B2物料占比");
            tb工时效率.DataBindings.Add("Text", bs_prodinstr.DataSource, "工时效率");
            tb生产数量.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产数量合计");
            tb生产重量.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产重量合计");
            tb废品重量.DataBindings.Add("Text", bs_prodinstr.DataSource, "废品重量合计");
            tb加料A.DataBindings.Add("Text", bs_prodinstr.DataSource, "加料A合计");
            tb加料B1C.DataBindings.Add("Text", bs_prodinstr.DataSource, "加料B1C合计");
            tb加料B2.DataBindings.Add("Text", bs_prodinstr.DataSource, "加料B2合计");
            tb工时.DataBindings.Add("Text", bs_prodinstr.DataSource, "工时合计");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "tb备注");

        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);

            //setDataGridViewCombox();
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
        }

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜生产日报表ID
            dataGridView1.Columns["填报人"].Visible = false;
            dataGridView1.Columns["审核人"].Visible = false;
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

    }
}
