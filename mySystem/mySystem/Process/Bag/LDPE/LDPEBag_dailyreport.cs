using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPEBag_dailyreport : BaseForm
    {
        int _id;
        string _sinstr_code;
        int _instr_id;
        //string cp产品代码;
        //string kh客户;
        //string ph批号;

        string table = "LDPE制袋日报表";



        private SqlConnection conn = null;
        private CheckForm checkform = null;

        private DataTable dtInner, dtOuter, dt代码批号, dt物料;  //生产指令：代码批号唯一确定
        private SqlDataAdapter daInner, daOuter, da记录详情;
        private BindingSource bsInner, bsOuter, bs记录详情;
        private SqlCommandBuilder cbInner, cbOuter, cb记录详情;



        DataTable dtShow;
        bool isFirstBind = true;

        public LDPEBag_dailyreport(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            _instr_id = Parameter.ldpebagInstruID;
            _sinstr_code = Parameter.ldpebagInstruction;
            conn = Parameter.conn;
            readOuterData(_instr_id);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_instr_id);
                outerBind();
            }
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            dateTimePickerStart.Value = DateTime.Now.AddDays(-7).Date;
            dateTimePickerEnd.Value = DateTime.Now;
            dtShow = query(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            setDGV(dtShow);
            readDGVWidthFromSettingAndSet(dataGridView1);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = _instr_id;
            dr["生产指令编号"] = _sinstr_code;
            dr["审核员"] = "";
            dr["审核意见"] = "";
            dr["审核是否通过"] = false ;
            dr["日志"] = "";
            return dr;
        }

        string[] getOtherData(int ins_id)
        {
            SqlDataAdapter da = new SqlDataAdapter(String.Format(
                "select * from 生产指令详细信息 where T生产指令ID={0}", ins_id), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                throw new Exception("读取生产指令数据失败!\n"+_sinstr_code);
            }
            string[] ret = new string[3];
            ret[0] = dt.Rows[0]["客户或订单号"].ToString();
            ret[1] = dt.Rows[0]["产品代码"].ToString();
            ret[2]  = dt.Rows[0]["产品批号"].ToString();
            //ret[3] = dt.Rows[0]["制袋物料代码1"].ToString();
            //ret[4] = dt.Rows[0]["制袋物料代码2"].ToString();
            //ret[5] = dt.Rows[0]["制袋物料代码3"].ToString();
            return ret;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
        }

        private void readOuterData(int _instr_id)
        {
            daOuter = new SqlDataAdapter("select * from LDPE制袋日报表 where 生产指令ID=" + _instr_id + "", conn);
            cbOuter = new SqlCommandBuilder(daOuter);
            dtOuter = new DataTable("生产指令");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }


        public LDPEBag_dailyreport(MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            conn = Parameter.conn;
            SqlDataAdapter da = new SqlDataAdapter(String.Format(
                "select * from LDPE制袋日报表 where ID={0}", id), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                throw new Exception("读取数据失败!");
            }

            _instr_id = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            _sinstr_code = dt.Rows[0]["生产指令编号"].ToString();
            _id = id;

            readOuterData(_instr_id);
            outerBind();
            dateTimePickerStart.Value = DateTime.Now.AddDays(-7).Date;
            dateTimePickerEnd.Value = DateTime.Now;
            dtShow = query(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            setDGV(dtShow);
        }


        int[] getChangAndKuan(string code)
        {
            string[] s = code.Split('-');
            string num = s[s.Length - 1];
            string[] nums = num.Split('X');
            int[] ret = new int[2];
            try
            {
                ret[0] = Convert.ToInt32(nums[0]);
            }
            catch
            {
                ret[0] = 0;
            }
            try
            {
                ret[1] = Convert.ToInt32(nums[1]) + 12;
            }
            catch
            {
                ret[1] = 0;
            }
            return ret;
        }

        DataTable query(DateTime start, DateTime end)
        {
            SqlDataAdapter da;
            string sql;
            start = start.Date;
            end = end.Date.AddDays(1).AddSeconds(-1);
            DataTable dt;
            int m膜材数量 = 0;
            List<Int32> sc生产指令Ids = new List<int>();

            List<string> 膜代码=null,内包代码=null,外包代码=null;
            // 创建列
            DataTable ret = new DataTable("PE制袋台账");
            ret.Columns.Add("生产指令ID", Type.GetType("System.Int32"));
            ret.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            ret.Columns.Add("班次", Type.GetType("System.String"));
            ret.Columns.Add("客户或订单号", Type.GetType("System.String"));
            ret.Columns.Add("产品代码", Type.GetType("System.String"));
            ret.Columns.Add("批号", Type.GetType("System.String"));
            ret.Columns.Add("入库量（只）", Type.GetType("System.Int32"));
            ret.Columns.Add("工时（小时）", Type.GetType("System.Double"));
            ret.Columns.Add("效率（只/小时）", Type.GetType("System.Double"));
            ret.Columns.Add("宽（mm）", Type.GetType("System.Int32"));
            ret.Columns.Add("长（mm）", Type.GetType("System.Int32"));
            ret.Columns.Add("数量（平方米）", Type.GetType("System.Double"));
            ret.Columns.Add("膜材规格（mm）", Type.GetType("System.Int32"));
            ret.Columns.Add("膜材批号", Type.GetType("System.String"));
            ret.Columns.Add("膜材用量（米）", Type.GetType("System.Double"));
            ret.Columns.Add("膜材用量（平方米）", Type.GetType("System.Double"));
            //ret.Columns.Add("膜材2规格（mm）", Type.GetType("System.Int32"));
            //ret.Columns.Add("膜材2批号", Type.GetType("System.String"));
            //ret.Columns.Add("膜材2用量（米）", Type.GetType("System.Double"));
            //ret.Columns.Add("膜材2用量（平方米）", Type.GetType("System.Double"));
            //ret.Columns.Add("膜材3规格（mm）", Type.GetType("System.Int32"));
            //ret.Columns.Add("膜材3批号", Type.GetType("System.String"));
            //ret.Columns.Add("膜材3用量（米）", Type.GetType("System.Double"));
            //ret.Columns.Add("膜材3用量（平方米）", Type.GetType("System.Double"));
            ret.Columns.Add("制袋收率（%）", Type.GetType("System.Double"));
            ret.Columns.Add("内包装袋代码", Type.GetType("System.String"));
            ret.Columns.Add("内包装袋批号", Type.GetType("System.String"));
            ret.Columns.Add("内包装袋用量（只）", Type.GetType("System.Int32"));
            ret.Columns.Add("内包规格（只/包）", Type.GetType("System.Int32"));
            ret.Columns.Add("内包产品数量（只）", Type.GetType("System.Int32"));
            ret.Columns.Add("纸箱代码", Type.GetType("System.String"));
            ret.Columns.Add("纸箱批号", Type.GetType("System.String"));
            ret.Columns.Add("纸箱用量", Type.GetType("System.Int32"));
            ret.Columns.Add("每包重量（Kg）", Type.GetType("System.Double"));
            ret.Columns.Add("每箱重量（Kg）", Type.GetType("System.Double"));
            ret.Columns.Add("外包规格（只/箱）", Type.GetType("System.Int32"));
            ret.Columns.Add("外包产品数量（只）", Type.GetType("System.Int32"));
            // 读取内包装表，确定行数，并把能填的值先填上
            sql = "select  distinct(生产指令Id) from 产品内包装记录 where 生产日期>='{0}' and 生产日期<='{1}'";
            da = new SqlDataAdapter(string.Format(sql, start.ToString("yyyy/MM/dd"), end.ToString("yyyy/MM/dd")), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                int instr_id = Convert.ToInt32( dr["生产指令Id"]);
                sc生产指令Ids.Add(instr_id);
                DataRow ndr = ret.NewRow();

                sql = "select * from 生产指令 where ID={0}";
                da = new SqlDataAdapter(string.Format(sql, instr_id), mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0) {
                    MessageBox.Show("ID为" + instr_id + "的生产指令读取错误");
                    continue;
                }
                else
                {
                    ndr["生产指令ID"] = instr_id;
                    string[] info = getOtherData(Convert.ToInt32(dr["生产指令ID"]));
                    int[] KandG = getChangAndKuan(info[1]);
                    ndr["膜材规格（mm）"] = KandG[0];
                    //dr["膜材2规格（mm）"] = KandG[0];
                    //dr["膜材3规格（mm）"] = KandG[0];
                    ndr["膜材批号"] = dt.Rows[0]["制袋物料批号1"];
                    ndr["内包装袋代码"] = dt.Rows[0]["内包物料代码1"];
                    ndr["内包装袋批号"] = dt.Rows[0]["内包物料批号1"];
                    ndr["纸箱代码"] = dt.Rows[0]["外包物料代码1"];
                    ndr["纸箱批号"] = dt.Rows[0]["外包物料批号1"];


                    膜代码 = new List<string>();
                    膜代码.Add(dt.Rows[0]["制袋物料代码1"].ToString());
                    膜代码.Add(dt.Rows[0]["制袋物料代码2"].ToString());
                    膜代码.Add(dt.Rows[0]["制袋物料代码3"].ToString());
                    内包代码 = new List<string>();
                    内包代码.Add(dt.Rows[0]["内包物料代码1"].ToString());
                    内包代码.Add(dt.Rows[0]["内包物料代码2"].ToString());
                    外包代码 = new List<string>();
                    外包代码.Add(dt.Rows[0]["外包物料代码1"].ToString());
                    外包代码.Add(dt.Rows[0]["外包物料代码2"].ToString());
                    外包代码.Add(dt.Rows[0]["外包物料代码3"].ToString());


                }


                
                //DataRow ndr = ret.NewRow();
                //ndr["生产指令ID"] = instr_id;
                //ndr["生产日期"] = dr["生产日期"];
                //ndr["班次"] = dr["班次"];
                //string[] info = getOtherData(Convert.ToInt32(ndr["生产指令ID"]));
                //ndr["客户或订单号"] = info[0];
                //ndr["产品代码"] = info[1];
                //ndr["批号"] = info[2];
                //ndr["入库量（只）"] = dr["产品数量包合计A"];
                //ndr["内包产品数量（只）"] = dr["产品数量包合计A"];
                //ndr["工时（小时）"] = dr["工时"];
                //try
                //{
                //    ndr["效率（只/小时）"] = Convert.ToDouble(ndr["入库量（只）"]) /
                //        Convert.ToDouble(ndr["工时（小时）"]);
                //}
                //catch
                //{
                //    ndr["效率（只/小时）"] = 0;
                //}
                //int[] KandG = getChangAndKuan(info[1]);
                //ndr["宽（mm）"] = KandG[0];
                //ndr["长（mm）"] = KandG[1];
                //// TODO: 是否要乘2？ 
                //ndr["数量（平方米）"] = Convert.ToInt32(ndr["入库量（只）"]) *
                //    Convert.ToDouble(ndr["宽（mm）"]) * Convert.ToDouble(ndr["长（mm）"])
                //    / 1000000.0;
                //ndr["内包规格（只/包）"] = dr["包装规格每包只数"];

                ret.Rows.Add(ndr);
            }


            // 读内包记录
            foreach (DataRow dr in ret.Rows)
            {
                int instr_id = Convert.ToInt32(dr["生产指令ID"]);
                sql = "select * from 产品内包装记录 where 生产指令ID={0} order by ID ASC";
                da = new SqlDataAdapter(string.Format(sql, instr_id), mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);

                dr["生产日期"] = dt.Rows[0]["生产日期"];
                dr["班次"] = dt.Rows[0]["班次"];
                string[] info = getOtherData(Convert.ToInt32(instr_id));
                dr["客户或订单号"] = info[0];
                dr["产品代码"] = info[1];
                dr["批号"] = info[2];
                dr["入库量（只）"] = dt.Rows[0]["产品数量包合计A"];
                dr["内包产品数量（只）"] = dt.Rows[0]["产品数量包合计A"];
                dr["工时（小时）"] = dt.Rows[0]["工时"];
                try
                {
                    dr["效率（只/小时）"] = Math.Round(Convert.ToDouble(dr["入库量（只）"]) /
                        Convert.ToDouble(dr["工时（小时）"]), 2);
                }
                catch
                {
                    dr["效率（只/小时）"] = 0;
                }
                int[] KandG = getChangAndKuan(info[1]);
                dr["宽（mm）"] = KandG[0];
                dr["长（mm）"] = KandG[1];
                // TODO: 是否要乘2？ 
                string[] codes = info[1].Split('-');
                if (codes.Length <= 1 || (codes[1].StartsWith("S")))
                {
                    dr["数量（平方米）"] = Convert.ToInt32(dr["入库量（只）"]) *
                        Convert.ToDouble(dr["宽（mm）"]) * Convert.ToDouble(dr["长（mm）"])
                        / 1000000.0;
                }
                else
                {
                    dr["数量（平方米）"] = Convert.ToInt32(dr["入库量（只）"]) *
                           Convert.ToDouble(dr["宽（mm）"]) * Convert.ToDouble(dr["长（mm）"])
                           / 1000000.0 * 2;
                }
                dr["数量（平方米）"] = Math.Round(Convert.ToDouble(dr["数量（平方米）"]), 4);
                dr["内包规格（只/包）"] = dt.Rows[0]["包装规格每包只数"];

            }




            //// 读生产指令，补充信息
            //foreach (DataRow dr in ret.Rows)
            //{
            //    int id = Convert.ToInt32(dr["生产指令ID"]);
                

            //    sql = "select * from 生产指令 where ID={0}";
            //    da = new SqlDataAdapter(string.Format(sql, id), mySystem.Parameter.conn);
            //    dt = new DataTable();
            //    da.Fill(dt);
            //    if (dt.Rows.Count == 0) MessageBox.Show("ID为" + id + "的生产指令读取错误");
            //    else
            //    {
            //        string[] info = getOtherData(Convert.ToInt32(dr["生产指令ID"]));
            //        int[] KandG = getChangAndKuan(info[1]);
            //        dr["膜材规格（mm）"] = KandG[0];
            //        //dr["膜材2规格（mm）"] = KandG[0];
            //        //dr["膜材3规格（mm）"] = KandG[0];
            //        dr["膜材批号"] = dt.Rows[0]["制袋物料批号1"];
            //        dr["内包装袋代码"] = dt.Rows[0]["内包物料代码1"];
            //        dr["内包装袋批号"] = dt.Rows[0]["内包物料批号1"];
            //        dr["纸箱代码"] = dt.Rows[0]["外包物料代码1"];
            //        dr["纸箱批号"] = dt.Rows[0]["外包物料批号1"];


            //        膜代码 = new List<string>();
            //        膜代码.Add(dt.Rows[0]["制袋物料代码1"].ToString());
            //        膜代码.Add(dt.Rows[0]["制袋物料代码2"].ToString());
            //        膜代码.Add(dt.Rows[0]["制袋物料代码3"].ToString());
            //        内包代码 = new List<string>();
            //        内包代码.Add(dt.Rows[0]["内包物料代码1"].ToString());
            //        内包代码.Add(dt.Rows[0]["内包物料代码2"].ToString());
            //        外包代码 = new List<string>();
            //        外包代码.Add(dt.Rows[0]["外包物料代码1"].ToString());
            //        外包代码.Add(dt.Rows[0]["外包物料代码2"].ToString());
            //        外包代码.Add(dt.Rows[0]["外包物料代码3"].ToString());


            //    }
               
            //}
            
            // 读领料记录，补充信息
            // 一天的记录，班次为夜，白，夜
            // 12点前的夜班为昨天的夜班，12点后的夜班为当天的夜班
            // 从生产指令中读出哪几个代码是膜材、内包、外包

            foreach (DataRow dr in ret.Rows)
            {
                // 获取本条记录的日期，班次，生产指令ID
                int id = Convert.ToInt32(dr["生产指令ID"]);
                DateTime currDateTime = Convert.ToDateTime(dr["生产日期"]);
                string fight = dr["班次"].ToString();
                sql = "select * from 生产领料使用记录,生产领料使用记录详细信息 where 生产领料使用记录详细信息.T生产领料使用记录ID=生产领料使用记录.ID and 生产领料使用记录.生产指令ID={0} and 生产领料使用记录详细信息.领料日期时间 between '{1}' and '{2}' and 生产领料使用记录详细信息.班次='{3}'";
                if (fight == "白班") {
                    da = new SqlDataAdapter(string.Format(sql, id, currDateTime.Date, currDateTime.AddDays(1).Date, fight), mySystem.Parameter.conn);
                }
                else {
                    da = new SqlDataAdapter(string.Format(sql, id, currDateTime.Date.AddHours(12), currDateTime.AddDays(1).Date.AddHours(12), fight), mySystem.Parameter.conn);
                }
                da = new SqlDataAdapter(string.Format(sql, id, currDateTime.Date.AddHours(-12), currDateTime.AddDays(1).Date.AddHours(12), fight), mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
                DataRow[] drs;
                double sum;
                dr["膜材用量（米）"] = 0;
                dr["内包装袋用量（只）"] = 0;
                dr["纸箱用量"] = 0;
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("生产指令ID为" + id + "的生产指令详细信息读取错误（无对应的领料日期）");
                }
                else
                {
                    // 膜材用量
                    drs = dt.Select(String.Format("物料代码='{0}' or 物料代码='{1}' or 物料代码='{2}'",
                        膜代码[0], 膜代码[1], 膜代码[2]));
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量"]);
                    }
                    dr["膜材用量（米）"] = sum;

                    // 内包用量
                    drs = dt.Select(String.Format("物料代码='{0}' or 物料代码='{1}'",
                       内包代码[0], 内包代码[1]));
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量"]);
                    }
                    dr["内包装袋用量（只）"] = sum;
                    
                    // 外包装袋用量
                    drs = dt.Select(String.Format("物料代码='{0}' or 物料代码='{1}' or 物料代码='{2}'",
                       外包代码[0], 外包代码[1], 外包代码[2]));
                    sum = 0;
                    foreach (DataRow ddr in drs)
                    {
                        sum += Convert.ToDouble(ddr["领取数量"]);
                    }
                    dr["纸箱用量"] = sum;
                }
                // 计算平方米之类的
                // TODO：判断是否要乘2
                dr["膜材用量（平方米）"] = Convert.ToDouble(dr["膜材规格（mm）"])
                    *Convert.ToDouble( dr["膜材用量（米）"])/1000.0;
                try
                {
                    dr["制袋收率（%）"] = Math.Round(Convert.ToDouble(dr["数量（平方米）"]) /
                        Convert.ToDouble(dr["膜材用量（平方米）"]), 2);
                }
                catch
                {
                    dr["制袋收率（%）"] = 0;
                }
            }

            foreach (DataRow dr in ret.Rows)
            {
                
                int instr_id = Convert.ToInt32(dr["生产指令ID"]);
                sql = "select * from 产品外包装记录表 where 生产指令ID={0} order by ID ASC";
                da = new SqlDataAdapter(string.Format(sql, instr_id), mySystem.Parameter.conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["每包重量（Kg）"] = 0;
                    dr["每箱重量（Kg）"] = 0;
                    dr["外包规格（只/箱）"] = 0;
                    dr["外包产品数量（只）"] = 0;
                }
                else
                {
                    dr["每包重量（Kg）"] = dt.Rows[0]["包装规格每包千克"];
                    dr["每箱重量（Kg）"] = dt.Rows[0]["包装规格每箱千克"];
                    dr["外包规格（只/箱）"] = dt.Rows[0]["包装规格每箱只数"];
                    dr["外包产品数量（只）"] = dt.Rows[0]["产品数量只数合计"];
                }
            }

            // 计算合计
            double 入库量合计 = 0,工时=0,工时效率,产品数量=0,膜材用量=0,制袋收率,
                内包用量=0,内包产量=0,纸箱用量=0,纸箱产量=0;
            foreach (DataRow dr in ret.Rows)
            {
                入库量合计 += Convert.ToDouble(dr["入库量（只）"]);
                工时 += Convert.ToDouble(dr["工时（小时）"]);
                产品数量 += Convert.ToDouble(dr["数量（平方米）"]);
                膜材用量 += Convert.ToDouble(dr["膜材用量（平方米）"]);
                内包用量 += Convert.ToDouble(dr["内包装袋用量（只）"]);
                内包产量 += Convert.ToDouble(dr["内包产品数量（只）"]);
                纸箱用量 += Convert.ToDouble(dr["纸箱用量"]);
                纸箱产量 += Convert.ToDouble(dr["外包产品数量（只）"]);
            }
            lbl入库量合计.Text = 入库量合计.ToString("F2");
            lbl工时合计.Text = 工时.ToString("F2");
            lbl工时效率.Text =(入库量合计/工时*100).ToString("F2");
                lbl成品数量合计.Text=产品数量.ToString("F2");
            lbl膜材用量合计.Text=膜材用量.ToString("F2");
            lbl制袋收率.Text = (产品数量/膜材用量*100).ToString("F2");
            lbl内包装用量.Text=内包用量.ToString("F2");
            lbl内包产品数量.Text=内包产量.ToString("F2");
            lbl纸箱用量.Text=纸箱用量.ToString("F2");
            lbl外包产品数量.Text = 纸箱产量.ToString("F2");

            
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
            readDGVWidthFromSettingAndSet(dataGridView1);
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);
        }

       


    }
}
