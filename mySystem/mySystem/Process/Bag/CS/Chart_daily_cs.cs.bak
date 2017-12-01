using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections;

namespace mySystem.Process.CleanCut
{
    public partial class Chart_daily_cs : BaseForm
    {

        DataTable dtShow;

        public Chart_daily_cs(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            dateTimePickerStart.Value = DateTime.Now.AddDays(-7).Date;
            dateTimePickerEnd.Value = DateTime.Now;
            dtShow = query(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            setDGV(dtShow);
        }

        DataTable query(DateTime start, DateTime end)
        {
            OleDbDataAdapter da;
            string sql;
            start = start.Date;
            end = end.Date.AddDays(1).AddSeconds(-1);
            DataTable dt;

            string xp1膜代码 = "", ty膜代码 = "";
            // 创建列
            DataTable ret = new DataTable("CS制袋台账");
            
            ret.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            ret.Columns.Add("班次", Type.GetType("System.String"));
            ret.Columns.Add("客户或订单号", Type.GetType("System.String"));
            ret.Columns.Add("产品代码", Type.GetType("System.String"));
            ret.Columns.Add("批号", Type.GetType("System.String"));
            ret.Columns.Add("内包规格", Type.GetType("System.Int32"));
            ret.Columns.Add("产品数量（只）", Type.GetType("System.Double"));
            ret.Columns.Add("产品数量（平米）", Type.GetType("System.Double"));
            ret.Columns.Add("TY膜用量（米）", Type.GetType("System.Double"));
            ret.Columns.Add("TY膜用量（平米）", Type.GetType("System.Double"));
            ret.Columns.Add("XP1膜用量（米）", Type.GetType("System.Double"));
            ret.Columns.Add("XP1膜用量（平米）", Type.GetType("System.Double"));
            ret.Columns.Add("制袋收率(%)", Type.GetType("System.Double"));
            ret.Columns.Add("工时(h)", Type.GetType("System.Double"));
            ret.Columns.Add("产品系数", Type.GetType("System.Double"));
            ret.Columns.Add("换算后产品数量", Type.GetType("System.Double"));
            ret.Columns.Add("工时效率", Type.GetType("System.Double"));
            ret.Columns.Add("内包装袋代码", Type.GetType("System.String"));
            ret.Columns.Add("内包装袋用量（只）", Type.GetType("System.Double"));
            ret.Columns.Add("外包装袋规格", Type.GetType("System.Double"));
            ret.Columns.Add("外包装袋代码", Type.GetType("System.String"));
            ret.Columns.Add("外包装袋用量", Type.GetType("System.Double"));
            ret.Columns.Add("生产指令ID", Type.GetType("System.Int32"));
            // 读取内包装表，确定行数，并把能填的值先填上
            sql = "select * from 产品内包装记录 where 生产日期>='{0}' and 生产日期<='{1}'";
            da = new OleDbDataAdapter(string.Format(sql, start.ToString("yyyy/MM/dd"), end.ToString("yyyy/MM/dd")), mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                DataRow ndr = ret.NewRow();
                ndr["生产日期"] = dr["生产日期"];
                ndr["班次"] = dr["班次"];
                ndr["产品代码"] = dr["产品代码"];
                ndr["批号"] = dr["生产批号"];
                ndr["内包规格"] = dr["内包装规格"];
                ndr["产品数量（只）"] = dr["产品数量只数合计B"];
                ndr["工时(h)"] = dr["工时"];
                ndr["生产指令ID"] = dr["生产指令ID"];
                ret.Rows.Add(ndr);
            }


            // 读生产指令，补充信息
            foreach (DataRow dr in ret.Rows)
            {
                int id = Convert.ToInt32(dr["生产指令ID"]);
                sql = "select * from 生产指令详细信息 where T生产指令ID={0}";
                da = new OleDbDataAdapter(string.Format(sql, id), mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0) MessageBox.Show("ID为" + id + "的生产指令详细信息读取错误");
                else
                {
                    dr["客户或订单号"] = dt.Rows[0]["客户或订单号"];
                    dr["产品系数"] = dt.Rows[0]["生产系数"];
                    dr["外包装袋规格"] = dt.Rows[0]["外包规格"];
                    
                }

                sql = "select * from 生产指令 where ID={0}";
                da = new OleDbDataAdapter(string.Format(sql, id), mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0) MessageBox.Show("ID为" + id + "的生产指令读取错误");
                else
                {
                    dr["内包装袋代码"] = dt.Rows[0]["内包物料代码1"];
                    dr["外包装袋代码"] = dt.Rows[0]["外包物料代码2"];
                    ty膜代码 = dt.Rows[0]["制袋物料代码1"].ToString();
                    xp1膜代码 = dt.Rows[0]["制袋物料代码2"].ToString();

                }
               
            }
            
            // 读领料记录，补充信息

            foreach (DataRow dr in ret.Rows)
            {
                // 获取本条记录的日期，班次，生产指令ID
                int id = Convert.ToInt32(dr["生产指令ID"]);
                DateTime currDateTime = Convert.ToDateTime(dr["生产日期"]);
                string fight = dr["班次"].ToString();
                sql = "select * from CS制袋领料记录,CS制袋领料记录详细记录 where CS制袋领料记录详细记录.TCS制袋领料记录ID=CS制袋领料记录.ID and CS制袋领料记录.生产指令ID={0} and CS制袋领料记录详细记录.领料日期时间 between #{1}# and #{2}# and CS制袋领料记录详细记录.班次='{3}'";
                da = new OleDbDataAdapter(string.Format(sql, id, currDateTime.Date, currDateTime.AddDays(1).Date, fight), mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
                DataRow[] drs;
                double sum;
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("生产指令ID为" + id + "的生产指令详细信息读取错误（无对应的领料日期）");
                    dr["TY膜用量（米）"] = 0;
                    dr["XP1膜用量（米）"] = 0;
                    dr["内包装袋用量（只）"] = 0;
                    dr["外包装袋用量"] = 0;
                }
                else
                {
                    // TY膜用量
                    drs = dt.Select("物料简称='Tyvek印刷卷材'");
                    dr["TY膜用量（米）"] = 0;
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量B"]);
                    }
                    dr["TY膜用量（米）"] = sum;
                    // XP1膜用量
                    drs = dt.Select("物料简称='药品包装用聚乙烯膜（XP1）'");
                    dr["XP1膜用量（米）"] = 0;
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量B"]);
                    }
                    dr["XP1膜用量（米）"] = sum;
                    // 内包装袋用量
                    drs = dt.Select("物料简称='内包装袋'");
                    dr["内包装袋用量（只）"] = 0;
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量B"]);
                    }
                    dr["内包装袋用量（只）"] = sum;
                    // 外包装袋用量
                    drs = dt.Select("物料简称='纸箱'");
                    dr["外包装袋用量"] = 0;
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量B"]);
                    }
                    dr["外包装袋用量"] = sum;
                }
            }

            // 读取存货档案，完成计算
            double h换算率;
            foreach (DataRow dr in ret.Rows)
            {
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection Tconn = new OleDbConnection(strConnect);
                Tconn.Open();
                // TY膜
                sql = "select * from 设置存货档案 where 存货代码='{0}'";
                da = new OleDbDataAdapter(string.Format(sql, ty膜代码), Tconn);
                dt = new DataTable();
                da.Fill(dt);
                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
                dr["TY膜用量（平米）"] = Math.Round(Convert.ToDouble(dr["TY膜用量（米）"]) / h换算率, 2);
                // XP1膜
                sql = "select * from 设置存货档案 where 存货代码='{0}'";
                da = new OleDbDataAdapter(string.Format(sql, xp1膜代码), Tconn);
                dt = new DataTable();
                da.Fill(dt);
                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
                dr["XP1膜用量（平米）"] = Math.Round(Convert.ToDouble(dr["XP1膜用量（米）"]) / h换算率, 2);
                // 产品
                sql = "select * from 设置存货档案 where 存货代码='{0}'";
                da = new OleDbDataAdapter(string.Format(sql, dr["产品代码"].ToString()), Tconn);
                dt = new DataTable();
                da.Fill(dt);
                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
                dr["产品数量（平米）"] = Math.Round(Convert.ToDouble(dr["产品数量（只）"]) / h换算率);

                dr["换算后产品数量"] = Math.Round( Convert.ToDouble(dr["产品数量（平米）"]) * Convert.ToDouble(dr["产品系数"]),2);
                dr["工时效率"] = Math.Round(Convert.ToDouble(dr["换算后产品数量"]) / Convert.ToDouble(dr["工时(h)"]), 2);
                dr["制袋收率(%)"] = Math.Round(100 * Convert.ToDouble(dr["产品数量（平米）"]) / (Convert.ToDouble(dr["XP1膜用量（平米）"]) + Convert.ToDouble(dr["TY膜用量（平米）"])), 2);
            }

            return ret;            
        }

        void setDGV(DataTable dt)
        {
            dataGridView1.DataSource = dt;
            dataGridView1.ReadOnly = true;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
            dataGridView1.Columns["生产指令ID"].Visible = false;
        }

        private void btn查询_Click(object sender, EventArgs e)
        {
            dtShow = query(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            setDGV(dtShow);
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            //print(false);
            //GC.Collect();
        }

       
    }
}
