﻿using System;
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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections;

namespace mySystem
{

    /// <summary>
    ///  吹膜生产日报表
    /// </summary>
    public partial class ProdctDaily_extrus : mySystem.BaseForm
    {
        int __生产指令ID;
        string __生产指令编码;
        Hashtable ht代码面数;
        Hashtable ht代码宽度;
        bool isFirstBind = true;

        public ProdctDaily_extrus(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            
            __生产指令ID = mySystem.Parameter.proInstruID;
            __生产指令编码 = mySystem.Parameter.proInstruction;
            getOtherData();
            fill_printer();
            Init();
            getPeople();
            setUserState();
            
            if ((_userState == Parameter.UserState.操作员))
            {
                button2.Enabled = false;
            }
        }



        private string idtocode(int id)
        {
            string ret = "";

            DataTable dt_temp = new DataTable("生产指令表");
            BindingSource bs_temp = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 生产指令信息表 where ID=" + id, mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 生产指令信息表 where ID=" + id, mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
            }

            if (dt_temp.Rows.Count > 0)
            {
                ret = dt_temp.Rows[0]["生产指令编号"].ToString();
            }
            return ret;
        }

        public ProdctDaily_extrus(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();

            string asql = "select * from 吹膜生产日报表 where ID=" + id;
            OleDbCommand comm = null ;
            OleDbDataAdapter da = null;
            SqlCommand commsql = null;
            SqlDataAdapter dasql = null;
            if (!mySystem.Parameter.isSqlOk)
            {
                comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                da = new OleDbDataAdapter(comm);
            }
            else
            {
                commsql = new SqlCommand(asql, mySystem.Parameter.conn);
                dasql = new SqlDataAdapter(commsql);
            }
            
            

            DataTable tempdt = new DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                da.Fill(tempdt);
            }
            else
            {
                dasql.Fill(tempdt);
            }
            
            __生产指令ID = (int)tempdt.Rows[0]["生产指令ID"];
            __生产指令编码 = idtocode(__生产指令ID);
            getOtherData();
            fill_printer();
            Init();
            getPeople();
            setUserState();
            ;
            if ((_userState == Parameter.UserState.操作员))
            {
                button2.Enabled = false;
            }


            readOuterData(__生产指令ID);
            removeOuterBinding();
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            query_by_instru(__生产指令ID);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            

            DataGridViewsum();
            dataGridView1.Columns[0].Visible = false;//ID

            //foreach (Control c in this.Controls)
            //    c.Enabled = false;
            //dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
        }

        private void getOtherData()
        {
            ht代码面数 = new Hashtable();

            DataTable dt = null;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
            }
            

            foreach (DataRow dr in dt.Rows)
            {
                ht代码面数.Add(dr["产品编码"].ToString(), Convert.ToInt32(dr["面数"]));
            }

            ht代码宽度 = new System.Collections.Hashtable();

            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + __生产指令ID, mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + __生产指令ID, mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
            }
            
            foreach (DataRow dr in dt.Rows)
            {
                ht代码宽度.Add(dr["产品编码"].ToString(), Convert.ToInt32(dr["宽"].ToString()));
            }
        }

        DataTable dt;

        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;

        private SqlDataAdapter da_prodinstrsql, da_prodlistsql;
        private SqlCommandBuilder cb_prodinstrsql, cb_prodlistsql;

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

            begin();

        }

        private void begin()
        {
            readOuterData(__生产指令ID);
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                if (!mySystem.Parameter.isSqlOk)
                {
                    da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                }
                else
                {
                    da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
                }
               
                readOuterData(__生产指令ID);
                removeOuterBinding();
                outerBind();              
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            query_by_instru(__生产指令ID);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            

            DataGridViewsum();
            dataGridView1.Columns[0].Visible = false;//ID
        }


        int get面数(string code)
        {
            if (code.Contains("SA"))
            {
                return 1;
            }
            return 2;
        }

        double get面积(string code, int width, double length)
        {
            int mian = get面数(code);
            if (mian == 1)
            {
                return (width + 40) * length / 1000;
            }
            if(mian==2){
                return width*length*2 / 1000;
            }
            return 0;
        }

        //查找同一条生产指令下的数据
        private void query_by_instru(int para_instrid)
        {
            while (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }

            string acsql = "";
            int id = para_instrid;//获得生产指令id

            DataTable dt_检验记录 = new DataTable();
            DataTable dt_废品记录 = new DataTable();
            
            DataTable dt_out = new DataTable();
            //检验记录
            acsql = "select * from 吹膜工序生产和检验记录 where 生产指令ID=" + id + " order by 生产日期";
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm2 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                OleDbDataAdapter da2 = new OleDbDataAdapter(comm2);
                da2.Fill(dt_检验记录); 
            }
            else
            {
                SqlCommand comm2 = new SqlCommand(acsql, mySystem.Parameter.conn);
                SqlDataAdapter da2 = new SqlDataAdapter(comm2);
                da2.Fill(dt_检验记录); 
            }
                    

            //废品记录
            acsql = "select * from 吹膜工序废品记录详细信息 where T吹膜工序废品记录ID=(select ID from 吹膜工序废品记录 where 生产指令ID="+id+")";
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm3 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                OleDbDataAdapter da3 = new OleDbDataAdapter(comm3);
                da3.Fill(dt_废品记录);
            }
            else
            {
                SqlCommand comm3 = new SqlCommand(acsql, mySystem.Parameter.conn);
                SqlDataAdapter da3 = new SqlDataAdapter(comm3);
                da3.Fill(dt_废品记录);
            }
            
           

            //根据产品代码和产品批号进行联合,以检验记录的行为标准
            for (int i = 0; i < dt_检验记录.Rows.Count; i++)
            {
                //检验记录详细信息
                DataTable dt_检验记录_详细 = new DataTable();
                acsql = "select 膜卷编号,膜卷长度,膜卷重量,开始时间,结束时间 from 吹膜工序生产和检验记录详细信息 where T吹膜工序生产和检验记录ID=" + (int)dt_检验记录.Rows[i][0];
                if (!mySystem.Parameter.isSqlOk)
                {
                    OleDbCommand comm2_new = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                    OleDbDataAdapter da2_new = new OleDbDataAdapter(comm2_new);
                    da2_new.Fill(dt_检验记录_详细);
                    comm2_new.Dispose();
                    da2_new.Dispose();
                }
                else
                {
                    SqlCommand comm2_new = new SqlCommand(acsql, mySystem.Parameter.conn);
                    SqlDataAdapter da2_new = new SqlDataAdapter(comm2_new);
                    da2_new.Fill(dt_检验记录_详细);
                    comm2_new.Dispose();
                    da2_new.Dispose();
                }
                
                string str膜卷编号 = "";
                if (dt_检验记录_详细.Rows.Count <= 0) continue;
                str膜卷编号 += dt_检验记录_详细.Rows[0][0].ToString() + "-" + dt_检验记录_详细.Rows[dt_检验记录_详细.Rows.Count - 1][0].ToString();
                float sum_膜卷长度 = 0, sum_膜卷重量 = 0;
                for (int kk = 0; kk < dt_检验记录_详细.Rows.Count; kk++)
                {
                    if (dt_检验记录_详细.Rows[kk][1] != null && dt_检验记录_详细.Rows[kk][1].ToString()!="")
                        sum_膜卷长度 += float.Parse(dt_检验记录_详细.Rows[kk][1].ToString());
                    if (dt_检验记录_详细.Rows[kk][2] != null && dt_检验记录_详细.Rows[kk][2].ToString()!="")
                        sum_膜卷重量 += float.Parse(dt_检验记录_详细.Rows[kk][2].ToString());
                }                
                DataRow dr = dt_prodlist.NewRow();

                string date_temp1 = ((DateTime)dt_检验记录.Rows[i]["生产日期"]). ToString("yyyy-MM-dd");//标准
                string code_temp1 = dt_检验记录.Rows[i]["产品名称"].ToString();//之前表上名字错误，应该是产品代码
                string flight_temp1=((bool)dt_检验记录.Rows[i]["班次"])==true?"白班":"夜班";
                bool flight_temp1_bool = (bool)dt_检验记录.Rows[i]["班次"];

                dr["T吹膜生产日报表ID"] = dt_prodinstr.Rows[0]["ID"];
                dr["序号"] = i + 1;
                dr["生产时间"] = date_temp1;//生产日期，具体到天
                dr["班次"] = ((bool)dt_检验记录.Rows[i]["班次"])==true?"白班":"夜班";
                dr["产品代码"] = code_temp1;//产品代码
                dr["产品批号"] = dt_检验记录.Rows[i]["产品批号"].ToString();
                dr["卷号"]= str膜卷编号;//卷号
                dr["生产数量"]= sum_膜卷长度;//生产数量
                dr["生产重量"] = Double.Parse( sum_膜卷重量.ToString("f1"));//生产重量
                dr["膜宽"] = Convert.ToInt32(ht代码宽度[code_temp1]);
                dr["膜层数"] = get面数(code_temp1);
                //dr["生产数量平米"] = Math.Round(sum_膜卷长度 * Convert.ToInt32(ht代码宽度[code_temp1]) * Convert.ToInt32(ht代码面数[code_temp1]) / 1000, 2);
                dr["生产数量平米"] = get面积(code_temp1, Convert.ToInt32(ht代码宽度[code_temp1]), sum_膜卷长度);
                if (dt_检验记录_详细.Rows[dt_检验记录_详细.Rows.Count - 1]["结束时间"] != null && dt_检验记录_详细.Rows[dt_检验记录_详细.Rows.Count - 1]["结束时间"].ToString() != "")
                {
                    TimeSpan delt = (DateTime)dt_检验记录_详细.Rows[dt_检验记录_详细.Rows.Count - 1]["结束时间"] - (DateTime)dt_检验记录_详细.Rows[0]["开始时间"];
                    //dr["工时"] = Math.Ceiling(delt.TotalHours);//工时
                    dr["工时"] = Math.Round(delt.TotalHours, 1);
                    
                }


                //查找废品记录中对应的记录查看的是当天的产品代码下供料合计
                float sum_废品重量 = 0;
                for (int j = 0; j < dt_废品记录.Rows.Count; j++)
                {
                    // 白班则正常检查
                    string date_temp2 = ((DateTime)dt_废品记录.Rows[j]["生产日期"]).ToString("yyyy-MM-dd");
                    string code_temp2 = dt_废品记录.Rows[j]["产品代码"].ToString();
                    string flight_temp2 = dt_废品记录.Rows[j]["班次"].ToString();
                    if (flight_temp2 == "白班")
                    {
                        if (date_temp1 == date_temp2 && code_temp1 == code_temp2 && flight_temp2 == flight_temp1)
                        {
                            if (dt_废品记录.Rows[j]["不良品数量"] != null && dt_废品记录.Rows[j]["不良品数量"].ToString() != "")
                                sum_废品重量 += float.Parse(dt_废品记录.Rows[j]["不良品数量"].ToString());
                        }
                    }
                    // 夜班则应该是当天12点后-第二天12点前的夜班
                    else
                    {
                        DateTime tttt= DateTime.Parse(date_temp1);
                        String today = tttt.ToString("yyyy-MM-dd");
                        String tomorrow = tttt.AddDays(1).ToString("yyyy-MM-dd");
                        String this_datetime = ((DateTime)dt_废品记录.Rows[j]["生产日期"]).ToString("yyyy-MM-dd");
                        int hour = Convert.ToInt32( ((DateTime)dt_废品记录.Rows[j]["生产日期"]).ToString("HH"));
                        if (flight_temp2 == flight_temp1 /*班次一样*/&& code_temp1 == code_temp2/*产品代码一样*/ &&
                            (this_datetime == today && hour >= 12 ||
                            this_datetime == tomorrow && hour <= 12))
                        {
                            if (dt_废品记录.Rows[j]["不良品数量"] != null && dt_废品记录.Rows[j]["不良品数量"].ToString() != "")
                                sum_废品重量 += float.Parse(dt_废品记录.Rows[j]["不良品数量"].ToString());
                        }
                    }
                }
                dr["废品重量"] = Double.Parse(sum_废品重量.ToString("f1"));//废品重量


                //查找供料记录中对应的记录，查看的是当天的产品代码下供料合计
                DataTable dt_供料记录 = new DataTable();
                if (!mySystem.Parameter.isSqlOk)
                {
                    acsql = "select * from 吹膜供料记录 where 生产指令ID=" + id + " and 产品代码='" + code_temp1 + "' and 班次=" + flight_temp1_bool + " and 供料日期=#" + date_temp1 + "#";
                    OleDbCommand comm4 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                    OleDbDataAdapter da4 = new OleDbDataAdapter(comm4);
                    da4.Fill(dt_供料记录);
                }
                else
                {
                    // 供料记录改了，一个生产指令只有一个记录了，所以查询条件不要那么多
                    // 应该查供料记录详细信息，按日期查
                    //acsql = "select * from 吹膜供料记录 where 生产指令ID=" + id;
                    acsql = "select sum(吹膜供料记录详细信息.外层供料量) as 外层供料量合计a ,sum(吹膜供料记录详细信息.中内层供料量) as 中内层供料量合计b from 吹膜供料记录详细信息,吹膜供料记录 where 吹膜供料记录详细信息.T吹膜供料记录ID=吹膜供料记录.ID and 吹膜供料记录详细信息.供料时间 between '{0}' and '{1}' and 吹膜供料记录.生产指令ID={2} and 吹膜供料记录详细信息.班次='{3}'";
                    // 这里分白班和夜班。
                    // 白班正常统计，夜班统计今天中午12点到明天中午12点的数据
                    SqlCommand comm4 = null;
                    if(flight_temp1=="白班"){
                        comm4 = new SqlCommand(String.Format(acsql,
                        date_temp1,
                        DateTime.Parse(date_temp1).AddDays(1).ToString("yyyy-MM-dd"),
                        id, flight_temp1), mySystem.Parameter.conn);
                    }
                    else if (flight_temp1 == "夜班")
                    {
                        String temp_start = DateTime.Parse(date_temp1).AddHours(12).ToString("yyyy-MM-dd HH:mm:ss");
                        String temp_end = DateTime.Parse(date_temp1).AddHours(12+24).ToString("yyyy-MM-dd HH:mm:ss");
                        comm4 = new SqlCommand(String.Format(acsql,
                        temp_start,
                        temp_end,
                        id, flight_temp1), mySystem.Parameter.conn);
                    }
                    SqlDataAdapter da4 = new SqlDataAdapter(comm4);
                    da4.Fill(dt_供料记录);
                }

                //如果上一行的本行是同一天同一个班次，则直接为0
                bool isPrevTheSame = false;
                if (dt_prodlist.Rows.Count > 0)
                {
                    DataRow prevdr = dt_prodlist.Rows[dt_prodlist.Rows.Count - 1];
                    if (prevdr["生产时间"].ToString() == dr["生产时间"].ToString() 
                        && prevdr["班次"].ToString() == dr["班次"].ToString())
                        isPrevTheSame = true;
                }

                if (dt_供料记录.Rows.Count > 0 && !isPrevTheSame)
                {
                    


                    if (dt_供料记录.Rows[0]["外层供料量合计a"] != null && dt_供料记录.Rows[0]["外层供料量合计a"].ToString() != "")
                        dr["加料A"] = float.Parse(dt_供料记录.Rows[0]["外层供料量合计a"].ToString());//加料A
                    else
                        dr["加料A"] = 0;
                    if (dt_供料记录.Rows[0]["中内层供料量合计b"] != null && dt_供料记录.Rows[0]["中内层供料量合计b"].ToString() != "")
                        dr["加料B"] = float.Parse(dt_供料记录.Rows[0]["中内层供料量合计b"].ToString());//加料B1C
                    else
                        dr["加料B"] = 0;
                    //if (dt_供料记录.Rows[0]["中层供料量合计c"] != null && dt_供料记录.Rows[0]["中层供料量合计c"].ToString() != "")
                    //    dr["加料B2"] = float.Parse(dt_供料记录.Rows[0]["中层供料量合计c"].ToString());//加料B2
                    //else
                        dr["加料B2"] = 0;
                }
                else
                {
                    dr["加料A"] = 0;
                    dr["加料B"] = 0;
                    dr["加料B2"] = 0;
                }

                


                dt_prodlist.Rows.Add(dr);
            }

            // 查找领料退料记录
            DataTable dtly = new DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter daly = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + para_instrid, mySystem.Parameter.connOle);
                daly.Fill(dtly);
            }
            else
            {
                SqlDataAdapter daly = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + para_instrid, mySystem.Parameter.conn);
                daly.Fill(dtly);
            }
            
            string code1 = dtly.Rows[0]["内外层物料代码"].ToString();
            string code2 = dtly.Rows[0]["中层物料代码"].ToString();
            double tuiliao1 = 0;
            double tuiliao2 = 0;

            DataTable dt_退料记录 = new DataTable();
            acsql = "select * from 吹膜工序领料退料记录 where 生产指令ID=" + id;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm5 = new OleDbCommand(acsql, mySystem.Parameter.connOle);
                OleDbDataAdapter da5 = new OleDbDataAdapter(comm5);
                da5.Fill(dt_退料记录);
            }
            else
            {
                SqlCommand comm5 = new SqlCommand(acsql, mySystem.Parameter.conn);
                SqlDataAdapter da5 = new SqlDataAdapter(comm5);
                da5.Fill(dt_退料记录);
            }
           
            foreach (DataRow ndr in dt_退料记录.Rows)
            {
                if (ndr["物料代码"].ToString() == code1)
                {
                    tuiliao1 = Convert.ToDouble(ndr["退料"]);
                }
                if (ndr["物料代码"].ToString() == code2)
                {
                    tuiliao2 = Convert.ToDouble(ndr["退料"]);
                }
            }
            try
            {
                dt_prodlist.Rows[dt_prodlist.Rows.Count - 1]["加料A"] = Convert.ToDouble(dt_prodlist.Rows[dt_prodlist.Rows.Count - 1]["加料A"]) - tuiliao1;
                dt_prodlist.Rows[dt_prodlist.Rows.Count - 1]["加料B"] = Convert.ToDouble(dt_prodlist.Rows[dt_prodlist.Rows.Count - 1]["加料B"]) - tuiliao2;
            }
            catch (Exception ee)
            {
                MessageBox.Show("还没有生产！");
            }
            dt = dt_prodlist.Copy();

            

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

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }

        public int print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/B/SOP-MFG-301-R02A 吹膜生产日报表.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            my.PageSetup.RightFooter = dt_prodinstr.Rows[0]["生产指令"] + "-02-001 &P/" + wb.ActiveSheet.PageSetup.Pages.Count;
            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                { }
                finally
                {
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;

                } 
                return pageCount;
            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            int ind = 0;
            if (dataGridView1.Rows.Count > 1)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[5, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                }
                ind = dataGridView1.Rows.Count - 1;
            }
            double 生产数量合计平米 = 0;
            my.Cells[3, 10].Value = "生产指令: "+__生产指令编码;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                my.Cells[5 + i, 1] = i + 1;
                my.Cells[5 + i, 2] = DateTime.Parse( dataGridView1.Rows[i].Cells[3].Value.ToString()).ToString("yyyy/MM/dd");
                my.Cells[5 + i, 3] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[5 + i, 4] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                my.Cells[5 + i, 5] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[5 + i, 6] = dataGridView1.Rows[i].Cells[7].Value.ToString();
                my.Cells[5 + i, 7] = dataGridView1.Rows[i].Cells[8].Value.ToString();
                my.Cells[5 + i, 8] = dataGridView1.Rows[i].Cells[9].Value.ToString();
                my.Cells[5 + i, 9] = dataGridView1.Rows[i].Cells[10].Value.ToString();
                my.Cells[5 + i, 10] = dataGridView1.Rows[i].Cells[11].Value.ToString();
                my.Cells[5 + i, 11] = dataGridView1.Rows[i].Cells[12].Value.ToString();
                my.Cells[5 + i, 12] = dataGridView1.Rows[i].Cells[14].Value.ToString();
                my.Cells[5 + i, 13] = dataGridView1.Rows[i].Cells[15].Value.ToString();
                my.Cells[5 + i, 14] = dataGridView1.Rows[i].Cells[16].Value.ToString();
                my.Cells[5 + i, 15] = dataGridView1.Rows[i].Cells[17].Value.ToString();
                my.Cells[5 + i, 16] = dataGridView1.Rows[i].Cells[18].Value.ToString();
                my.Cells[5 + i, 17] = dataGridView1.Rows[i].Cells[19].Value.ToString();
                生产数量合计平米 += Convert.ToDouble(dataGridView1.Rows[i].Cells[19].Value);

            }
            my.Cells[6 + ind, 3].Value = Math.Round(Convert.ToDouble(dt_prodinstr.Rows[0]["加料B1C合计"]) / (Convert.ToDouble(dt_prodinstr.Rows[0]["加料A合计"]) + Convert.ToDouble(dt_prodinstr.Rows[0]["加料B1C合计"])) * 100, 2);
            my.Cells[6+ind, 7].Value = dt_prodinstr.Rows[0]["生产数量合计"];
            my.Cells[6 + ind, 8].Value = dt_prodinstr.Rows[0]["生产重量合计"];
            my.Cells[6 + ind, 9].Value = dt_prodinstr.Rows[0]["废品重量合计"];
            my.Cells[6 + ind, 10].Value = dt_prodinstr.Rows[0]["加料A合计"];
            my.Cells[6 + ind, 11].Value = dt_prodinstr.Rows[0]["加料B1C合计"];
            my.Cells[6 + ind, 12].Value = dt_prodinstr.Rows[0]["工时合计"];
            my.Cells[6 + ind, 17].Value = 生产数量合计平米;
            my.Cells[7 + ind, 3].Value = dt_prodinstr.Rows[0]["成品率"];
            my.Cells[8 + ind, 3].Value = dt_prodinstr.Rows[0]["工时效率"];
            my.Cells[7 + ind, 6].Value = "备注: " + dt_prodinstr.Rows[0]["备注"];

            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.Date;//开始时间
            date2 = dateTimePicker2.Value.Date;//结束时间
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            string sql = "生产时间>=" + "'" + date1 + "'" + " and " + "生产时间<=" + "'" + date2 + "'";

            DataRow[] arrayDR = dt.Select(sql);
            //清空表格
            while (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            dt_prodlist.Clear();
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            //填充
            int i = 0;
            foreach (DataRow drow in arrayDR)
            {
                DataRow dr = dt_prodlist.NewRow();

                dr.ItemArray = drow.ItemArray.Clone() as object[];
                dr["T吹膜生产日报表ID"] = dt_prodinstr.Rows[0]["ID"];
                dr["序号"] = ++i;
                dt_prodlist.Rows.Add(dr);
            }
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            DataGridViewsum();

        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = __生产指令ID;
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
            dr["加料B"] = 0;
            dr["加料B2"] = 0;
            dr["工时"] = 0;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dt_prodinstr = new DataTable("吹膜生产日报表");
                bs_prodinstr = new BindingSource();
                da_prodinstr = new OleDbDataAdapter("select * from 吹膜生产日报表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
                da_prodinstr.Fill(dt_prodinstr);
            }
            else
            {
                dt_prodinstr = new DataTable("吹膜生产日报表");
                bs_prodinstr = new BindingSource();
                da_prodinstrsql = new SqlDataAdapter("select * from 吹膜生产日报表 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                cb_prodinstrsql = new SqlCommandBuilder(da_prodinstrsql);
                da_prodinstrsql.Fill(dt_prodinstr);
            }
            
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dt_prodlist = new DataTable("吹膜生产日报表详细信息");
                bs_prodlist = new BindingSource();
                da_prodlist = new OleDbDataAdapter("select * from 吹膜生产日报表详细信息 where T吹膜生产日报表ID=" + id, mySystem.Parameter.connOle);
                cb_prodlist = new OleDbCommandBuilder(da_prodlist);
                da_prodlist.Fill(dt_prodlist);
            }
            else
            {
                dt_prodlist = new DataTable("吹膜生产日报表详细信息");
                bs_prodlist = new BindingSource();
                da_prodlistsql = new SqlDataAdapter("select * from 吹膜生产日报表详细信息 where T吹膜生产日报表ID=" + id, mySystem.Parameter.conn);
                cb_prodlistsql = new SqlCommandBuilder(da_prodlistsql);
                da_prodlistsql.Fill(dt_prodlist);
            }
            
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
            tb成品率.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

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
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
            tb成品率.DataBindings.Add("Text", bs_prodinstr.DataSource, "成品率");

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
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜生产日报表ID
            dataGridView1.Columns["加料B2"].Visible = false;
            dataGridView1.Columns["填报人"].Visible = false;
            dataGridView1.Columns["审核人"].Visible = false;
            dataGridView1.Columns["生产数量平米"].HeaderText = "生产数量(平米)";
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //计算合计
        void DataGridViewsum()
        {
            float sum_生产数量=0,sum_生产重量=0,sum_废品重量=0,sum_加料A=0,sum_加料B1C=0,sum_加料B2=0,sum_工时=0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["生产数量"].Value.ToString() != "")
                {
                    sum_生产数量 += float.Parse(dataGridView1.Rows[i].Cells["生产数量"].Value.ToString());
                }
                if (dataGridView1.Rows[i].Cells["生产重量"].Value.ToString() != "")
                {
                    sum_生产重量 += float.Parse(float.Parse(dataGridView1.Rows[i].Cells["生产重量"].Value.ToString()).ToString("f1"));
                }
                if (dataGridView1.Rows[i].Cells["废品重量"].Value.ToString() != "")
                {
                    sum_废品重量 += float.Parse(float.Parse(dataGridView1.Rows[i].Cells["废品重量"].Value.ToString()).ToString("f1"));
                }
                if (dataGridView1.Rows[i].Cells["加料A"].Value.ToString() != "")
                {
                    sum_加料A += float.Parse(dataGridView1.Rows[i].Cells["加料A"].Value.ToString());
                }
                if (dataGridView1.Rows[i].Cells["加料B"].Value.ToString() != "")
                {
                    sum_加料B1C += float.Parse(dataGridView1.Rows[i].Cells["加料B"].Value.ToString());
                }
                if (dataGridView1.Rows[i].Cells["加料B2"].Value.ToString() != "")
                {
                    sum_加料B2 += float.Parse(dataGridView1.Rows[i].Cells["加料B2"].Value.ToString());
                }
                if (dataGridView1.Rows[i].Cells["工时"].Value.ToString() != "")
                {
                    sum_工时 += float.Parse(dataGridView1.Rows[i].Cells["工时"].Value.ToString());
                }
            }
            dt_prodinstr.Rows[0]["生产数量合计"] = sum_生产数量;
            dt_prodinstr.Rows[0]["生产重量合计"] = Math.Round(double.Parse(sum_生产重量.ToString("f1")), 1);
            dt_prodinstr.Rows[0]["废品重量合计"] = Math.Round(double.Parse(sum_废品重量.ToString("f1")), 1);
            dt_prodinstr.Rows[0]["加料A合计"] = sum_加料A;
            dt_prodinstr.Rows[0]["加料B1C合计"] = sum_加料B1C;
            dt_prodinstr.Rows[0]["加料B2合计"] = sum_加料B2;
            dt_prodinstr.Rows[0]["工时合计"] = Math.Round((float)sum_工时, 2);

            float temp = (sum_生产重量 + sum_废品重量) / (sum_工时+0.0001f);
            dt_prodinstr.Rows[0]["工时效率"] =Math.Round((float)temp, 2) ;
            dt_prodinstr.Rows[0]["成品率"] = Math.Round(sum_生产重量 / (sum_加料A + sum_加料B1C + sum_加料B2+0.000001) * 100);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
            else
            {
                da_prodinstrsql.Update((DataTable)bs_prodinstr.DataSource);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value= (DateTime)dt.Rows[0]["生产时间"];
            dateTimePicker2.Value = DateTime.Now;

            button1.PerformClick();
            setDataGridViewRowNums();
            dataGridView1.Columns["ID"].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new ProdctDaily_extrus(mainform, 2)).Show();
        }

        List<string> ls操作员, ls审核员;

        void getPeople()
        {
            ls审核员 = new List<string>();
            ls操作员 = new List<string>();
            DataTable dt = null;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='" + "吹膜生产日报表" + "'", mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='" + "吹膜生产日报表" + "'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
            }
            
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户权限设置有误，为避免出现错误，请尽快联系管理员完成设置！");
                this.Dispose();
                return;
            }

            string str操作员 = dt.Rows[0]["操作员"].ToString();
            string str审核员 = dt.Rows[0]["审核员"].ToString();
            String[] tmp = Regex.Split(str操作员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls操作员.Add(s);
                }
            }
            tmp = Regex.Split(str审核员, ",|，");
            foreach (string s in tmp)
            {
                if (s != "")
                {
                    ls审核员.Add(s);
                }
            }
        }


        mySystem.Parameter.UserState _userState;
        void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜生产日报表'", mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='吹膜生产日报表'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
           
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        private void ProdctDaily_extrus_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
        }



        public ProdctDaily_extrus(mySystem.MainForm mainform, int id, bool forprint)
            : base(mainform)
        {
            InitializeComponent();

            string asql = "select * from 吹膜生产日报表 where ID=" + id;
            OleDbCommand comm = null;
            OleDbDataAdapter da = null;
            SqlCommand commsql = null;
            SqlDataAdapter dasql = null;
            if (!mySystem.Parameter.isSqlOk)
            {
                comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
                da = new OleDbDataAdapter(comm);
            }
            else
            {
                commsql = new SqlCommand(asql, mySystem.Parameter.conn);
                dasql = new SqlDataAdapter(commsql);
            }



            DataTable tempdt = new DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                da.Fill(tempdt);
            }
            else
            {
                dasql.Fill(tempdt);
            }

            __生产指令ID = (int)tempdt.Rows[0]["生产指令ID"];
            __生产指令编码 = idtocode(__生产指令ID);
            getOtherData();
            fill_printer();
            Init();
            getPeople();
            //setUserState();
            _userState = Parameter.UserState.NoBody;
            ;
            if ((_userState == Parameter.UserState.操作员))
            {
                button2.Enabled = false;
            }


            readOuterData(__生产指令ID);
            removeOuterBinding();
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            query_by_instru(__生产指令ID);
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }


            DataGridViewsum();
            dataGridView1.Columns[0].Visible = false;//ID

            //foreach (Control c in this.Controls)
            //    c.Enabled = false;
            //dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
        }
    }
}
