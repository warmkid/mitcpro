using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;

namespace mySystem.Process.Bag.BTV
{
    public partial class 产品物料代码查询 : BaseForm
    {
        DataTable dtShow;
        public 产品物料代码查询()
        {
            InitializeComponent();
            dateTimePickerStart.Value = DateTime.Now.AddDays(-7).Date;
            //dtShow = new DataTable();
            //addDefaultCol();
        }

        private void btn产品代码查询_Click(object sender, EventArgs e)
        {
            //HashSet<String> hs所有物料代码 = new HashSet<String>();
            dtShow = new DataTable();
            addDefaultCol();
            DataTable dt产品信息 = queryProdct(dateTimePickerStart.Value, dateTimePickerEnd.Value, tb产品代码.Text);
            queryDetail(dt产品信息);
            dataGridView1.DataSource = dtShow;
            
        }

        private void btn物料代码查询_Click(object sender, EventArgs e)
        {
            dtShow = new DataTable();
            addDefaultCol();
            DataTable dt产品信息 = queryMaterail(dateTimePickerStart.Value, dateTimePickerEnd.Value, tb物料代码.Text);
            queryDetail(dt产品信息);
            dataGridView1.DataSource = dtShow;
        }

        DataTable queryProdct(DateTime start, DateTime end, String code)
        {
            SqlDataAdapter da;
            DataTable dt;
            String sql;
            sql = "select 生产指令ID, 生产指令编号, 生产日期,班次,产品代码,生产批号,产品数量包数合计A,产品数量只数合计B from 产品内包装记录 where 生产日期 between '{0}' and '{1}' and 产品代码 like '%{2}%'";
            da = new SqlDataAdapter(String.Format(sql, start, end, code), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        DataTable queryMaterail(DateTime start, DateTime end, String code)
        {
            SqlDataAdapter da;
            DataTable dt;
            String sql;
            // 产品内包装记录  生产领料使用记录  生产领料使用记录详细信息

            sql = "select 产品内包装记录.生产指令ID as 生产指令ID, 产品内包装记录.生产指令编号 as 生产指令编号, 产品内包装记录.生产日期 as 生产日期, 产品内包装记录.班次 as 班次,产品内包装记录.产品代码 as 产品代码, 产品内包装记录.生产批号 as 生产批号,产品内包装记录.产品数量包数合计A as 产品数量包数合计A,产品内包装记录.产品数量只数合计B as 产品数量只数合计B from 产品内包装记录,生产领料使用记录,生产领料使用记录详细信息 " +
                "where 生产领料使用记录详细信息.物料代码 like '%{2}%' and  生产领料使用记录详细信息.T生产领料使用记录ID=生产领料使用记录.ID and 生产领料使用记录.生产指令ID=产品内包装记录.生产指令ID  and 产品内包装记录.生产日期 between '{0}' and '{1}'";
            da = new SqlDataAdapter(String.Format(sql, start, end, code), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        void addDefaultCol()
        {
            dtShow.Columns.Add("生产指令编码", Type.GetType("System.String"));
            dtShow.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            dtShow.Columns.Add("班次", Type.GetType("System.String"));
            dtShow.Columns.Add("产品代码", Type.GetType("System.String"));
            dtShow.Columns.Add("生产批号", Type.GetType("System.String"));
            dtShow.Columns.Add("生产数量（包）", Type.GetType("System.Double"));
            dtShow.Columns.Add("生产数量（只）", Type.GetType("System.Double"));
        }

        void queryDetail(DataTable dt产品信息)
        {
            HashSet<String> hs所有物料代码 = new HashSet<String>();
            foreach (DataRow dr in dt产品信息.Rows)
            {
                SqlDataAdapter da;
                DataTable dt;
                String sql;
                DateTime st, ed;
                st = Convert.ToDateTime(dr["生产日期"]).Date;
                ed = Convert.ToDateTime(dr["生产日期"]).AddDays(1).Date.AddMilliseconds(-1) ;
                sql = "select 生产领料使用记录详细信息.物料代码 as 物料代码, 生产领料使用记录详细信息.领取数量 as 数量 from 生产领料使用记录,生产领料使用记录详细信息 where 生产领料使用记录.生产指令ID={0} and 生产领料使用记录详细信息.T生产领料使用记录ID=生产领料使用记录.ID and 生产领料使用记录详细信息.领料日期时间 between '{1}' and '{2}' and 生产领料使用记录详细信息.班次='{3}'";
                da = new SqlDataAdapter(String.Format(sql, dr["生产指令ID"], st, ed, dr["班次"]), mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow tdr in dt.Rows)
                {
                    if (!hs所有物料代码.Contains(tdr["物料代码"].ToString()))
                    {
                        hs所有物料代码.Add(tdr["物料代码"].ToString());
                        //给总表加列
                        //dtShow.Columns.Add(tdr["物料代码"].ToString(), Type.GetType("System.String"));
                        dtShow.Columns.Add(tdr["物料代码"].ToString()+"用量", Type.GetType("System.Double"));
                        dtShow.Columns[tdr["物料代码"].ToString() + "用量"].DefaultValue = 0;
                    }
                }
            }
            // 填数据
            foreach (DataRow dr in dt产品信息.Rows)
            {
                DataRow row = dtShow.NewRow();
                row["生产指令编码"] = dr["生产指令编号"];
                row["生产日期"] = dr["生产日期"];
                row["班次"] = dr["班次"];
                row["产品代码"] = dr["产品代码"];
                row["生产批号"] = dr["生产批号"];
                row["生产数量（包）"] = dr["产品数量包数合计A"];
                row["生产数量（只）"] = dr["产品数量只数合计B"];
                SqlDataAdapter daL;
                DataTable dtL;
                String sql;
                DateTime st, ed;
                st = Convert.ToDateTime(dr["生产日期"]).Date;
                ed = Convert.ToDateTime(dr["生产日期"]).AddDays(1).Date.AddMilliseconds(-1);
                sql = "select 生产领料使用记录详细信息.物料代码 as 物料代码, 生产领料使用记录详细信息.领取数量 as 数量 from 生产领料使用记录,生产领料使用记录详细信息 where 生产领料使用记录.生产指令ID={0} and 生产领料使用记录详细信息.T生产领料使用记录ID=生产领料使用记录.ID and 生产领料使用记录详细信息.领料日期时间 between '{1}' and '{2}' and 生产领料使用记录详细信息.班次='{3}'";
                daL = new SqlDataAdapter(String.Format(sql, dr["生产指令ID"], st, ed, dr["班次"]), mySystem.Parameter.conn);
                dtL = new DataTable();
                daL.Fill(dtL);
                foreach (DataRow tdr in dtL.Rows)
                {
                    row[tdr["物料代码"].ToString() + "用量"] = tdr["数量"];
                }
                SqlDataAdapter daT;
                DataTable dtT;
                st = Convert.ToDateTime(dr["生产日期"]).Date;
                ed = Convert.ToDateTime(dr["生产日期"]).AddDays(1).Date.AddMilliseconds(-1);
                sql = "select 生产退料记录详细信息.物料代码 as 物料代码, 生产退料记录详细信息.退库数量 as 数量 from 生产退料记录表,生产退料记录详细信息 where 生产退料记录表.生产指令ID={0} and 生产退料记录详细信息.T生产退料记录ID=生产退料记录表.ID and 生产退料记录详细信息.领料日期 between '{1}' and '{2}' and 生产退料记录详细信息.班次='{3}'";
                daT = new SqlDataAdapter(String.Format(sql, dr["生产指令ID"], st, ed, dr["班次"]), mySystem.Parameter.conn);
                dtT = new DataTable();
                daT.Fill(dtT);
                foreach (DataRow tdr in dtL.Rows)
                {
                    row[tdr["物料代码"].ToString() + "用量"] = Convert.ToDouble(row[tdr["物料代码"].ToString() + "用量"]) - Convert.ToDouble(tdr["数量"]);
                }
                dtShow.Rows.Add(row); 
            }
        }

        private void 产品物料代码查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1) ;
        }

    }
}
