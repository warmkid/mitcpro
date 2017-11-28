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

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVBag_dailyreport : BaseForm
    {

        DataTable dtShow;

        public PTVBag_dailyreport(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            dateTimePickerStart.Value = DateTime.Now.AddDays(-7).Date;
            dateTimePickerEnd.Value = DateTime.Now;
            dtShow = query(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            setDGV(dtShow);
            fill_printer();
        }

        DataTable query(DateTime start, DateTime end)
        {
            OleDbDataAdapter da;
            string sql;
            start = start.Date;
            end = end.Date.AddDays(1).AddSeconds(-1);
            DataTable dt;
  
            DataTable ret = new DataTable("PTV制袋台账");

            ret.Columns.Add("生产指令号", Type.GetType("System.String"));
            ret.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            ret.Columns.Add("班次", Type.GetType("System.String"));
            ret.Columns.Add("客户或订单号", Type.GetType("System.String"));
            ret.Columns.Add("产品代码", Type.GetType("System.String"));
            ret.Columns.Add("批号", Type.GetType("System.String"));

            ret.Columns.Add("产品数量（只）", Type.GetType("System.Int32"));
            //ret.Columns.Add("产品数量（平米）", Type.GetType("System.Double"));
            ret.Columns.Add("生产指令ID", Type.GetType("System.Int32"));

            sql = "select * from 产品内包装记录 where 生产日期>='{0}' and 生产日期<='{1}'";
            da = new OleDbDataAdapter(string.Format(sql, start.ToString("yyyy/MM/dd"), end.ToString("yyyy/MM/dd")), mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                DataRow ndr = ret.NewRow();
                ndr["生产指令号"] = dr["生产指令编号"];
                ndr["生产日期"] = dr["生产日期"];
                ndr["班次"] = dr["班次"];
                ndr["产品代码"] = dr["产品代码"];
                ndr["批号"] = dr["生产批号"];

                ndr["产品数量（只）"] = dr["产品数量只数合计B"];

                ndr["生产指令ID"] = dr["生产指令ID"];

                ret.Rows.Add(ndr);
            }
             //读生产指令，补充信息
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
                    dr["客户或订单号"] = dt.Rows[0]["订单号"];

                }
            }

            return ret;
//            OleDbDataAdapter da;
//            string sql;
//            start = start.Date;
//            end = end.Date.AddDays(1).AddSeconds(-1);
//            DataTable dt;

//            string xp1膜代码 = "", ty膜代码 = "";
//            // 创建列
//            DataTable ret = new DataTable("PTV制袋台账");

//            ret.Columns.Add("生产指令号", Type.GetType("System.String"));
//            ret.Columns.Add("生产日期", Type.GetType("System.DateTime"));
//            ret.Columns.Add("班次", Type.GetType("System.String"));
//            ret.Columns.Add("客户或订单号", Type.GetType("System.String"));
//            ret.Columns.Add("产品代码", Type.GetType("System.String"));
//            ret.Columns.Add("批号", Type.GetType("System.String"));
            
//            ret.Columns.Add("产品数量（只）", Type.GetType("System.Int32"));
//            ret.Columns.Add("产品数量（平米）", Type.GetType("System.Double"));

//            ret.Columns.Add("TY膜代码", Type.GetType("System.String"));
//            ret.Columns.Add("TY膜批号", Type.GetType("System.String"));
//            ret.Columns.Add("TY膜用量（米）", Type.GetType("System.Double"));
//            ret.Columns.Add("TY膜用量（平米）", Type.GetType("System.Double"));

//            ret.Columns.Add("XP1膜代码", Type.GetType("System.String"));
//            ret.Columns.Add("XP1膜批号", Type.GetType("System.String"));
//            ret.Columns.Add("XP1膜用量（米）", Type.GetType("System.Double"));
//            ret.Columns.Add("XP1膜用量（平米）", Type.GetType("System.Double"));

//            ret.Columns.Add("制袋收率(%)", Type.GetType("System.Double"));

//            ret.Columns.Add("工时(h)", Type.GetType("System.Double"));
//            ret.Columns.Add("产品系数", Type.GetType("System.Double"));
//            ret.Columns.Add("换算后产品数量", Type.GetType("System.Double"));
//            ret.Columns.Add("工时效率", Type.GetType("System.Double"));

//            ret.Columns.Add("内包装袋规格", Type.GetType("System.Int32"));//内包规格就是 只/包
//            ret.Columns.Add("内包装袋代码", Type.GetType("System.String"));
//            ret.Columns.Add("内包装袋批号", Type.GetType("System.String"));
//            ret.Columns.Add("内包装袋用量（只）", Type.GetType("System.Double"));

//            ret.Columns.Add("外包装袋规格", Type.GetType("System.Int32"));//外包规格就是 只/箱
//            ret.Columns.Add("外包装袋代码", Type.GetType("System.String"));
//            ret.Columns.Add("外包装袋批号", Type.GetType("System.String"));
//            ret.Columns.Add("外包装袋用量", Type.GetType("System.Double"));

//            ret.Columns.Add("内标签", Type.GetType("System.String"));
//            ret.Columns.Add("外标签", Type.GetType("System.String"));

//            ret.Columns.Add("生产指令ID", Type.GetType("System.Int32"));

//            // 读取内包装表，确定行数，并把能填的值先填上
//            sql = "select * from 产品内包装记录 where 生产日期>='{0}' and 生产日期<='{1}'";
//            da = new OleDbDataAdapter(string.Format(sql, start.ToString("yyyy/MM/dd"), end.ToString("yyyy/MM/dd")), mySystem.Parameter.connOle);
//            dt = new DataTable();
//            da.Fill(dt);
//            foreach (DataRow dr in dt.Rows)
//            {
//                DataRow ndr = ret.NewRow();
//                ndr["生产指令号"] = dr["生产指令编号"];
//                ndr["生产日期"] = dr["生产日期"];
//                ndr["班次"] = dr["班次"];
//                ndr["产品代码"] = dr["产品代码"];
//                ndr["批号"] = dr["生产批号"];

//                ndr["内包装袋规格"] = dr["内包装规格"];
//                ndr["产品数量（只）"] = dr["产品数量只数合计B"];
//                ndr["工时(h)"] = dr["工时"];

//                ndr["生产指令ID"] = dr["生产指令ID"];
                
//                ret.Rows.Add(ndr);
//            }


//            // 读生产指令，补充信息
//            foreach (DataRow dr in ret.Rows)
//            {
//                int id = Convert.ToInt32(dr["生产指令ID"]);
//                sql = "select * from 生产指令详细信息 where T生产指令ID={0}";
//                da = new OleDbDataAdapter(string.Format(sql, id), mySystem.Parameter.connOle);
//                dt = new DataTable();
//                da.Fill(dt);
//                if (dt.Rows.Count == 0) MessageBox.Show("ID为" + id + "的生产指令详细信息读取错误");
//                else
//                {
//                    dr["客户或订单号"] = dt.Rows[0]["订单号"];
//                    dr["产品系数"] = dt.Rows[0]["生产系数"];
//                    dr["外包装袋规格"] = dt.Rows[0]["外包规格"];
//                    dr["内标签"] = dt.Rows[0]["内标签"];
//                    dr["外标签"] = dt.Rows[0]["外标签"];
//                }

//                sql = "select * from 生产指令 where ID={0}";
//                da = new OleDbDataAdapter(string.Format(sql, id), mySystem.Parameter.connOle);
//                dt = new DataTable();
//                da.Fill(dt);
//                if (dt.Rows.Count == 0) MessageBox.Show("ID为" + id + "的生产指令读取错误");
//                else
//                {
//                    dr["内包装袋代码"] = dt.Rows[0]["内包物料代码1"];
//                    dr["外包装袋代码"] = dt.Rows[0]["外包物料代码2"];

//                    dr["内包装袋批号"] = dt.Rows[0]["内包物料批号1"];
//                    dr["外包装袋批号"] = dt.Rows[0]["外包物料批号2"];

//                    ty膜代码 = dt.Rows[0]["制袋物料代码1"].ToString();
//                    xp1膜代码 = dt.Rows[0]["制袋物料代码2"].ToString();

//                    dr["TY膜代码"] = ty膜代码;
//                    dr["XP1膜代码"] = xp1膜代码;
//                    dr["TY膜批号"] = dt.Rows[0]["制袋物料批号1"];
//                    dr["XP1膜批号"] = dt.Rows[0]["制袋物料批号2"];

//                }
               
//            }
            
//            // 读领料记录，补充信息

//            foreach (DataRow dr in ret.Rows)
//            {
//                // 获取本条记录的日期，班次，生产指令ID
//                int id = Convert.ToInt32(dr["生产指令ID"]);
//                DateTime currDateTime = Convert.ToDateTime(dr["生产日期"]);
//                string fight = dr["班次"].ToString();

//                //计算退料量
//                double sum_TY退料 = 0;
//                double sum_XP1退料 = 0;
//                double sum_内包退料 = 0;
//                double sum_外包退料 = 0;

//                sql = "select * from 生产退料记录表,生产退料记录详细信息 where 生产退料记录详细信息.T生产退料记录ID=生产退料记录表.ID and 生产退料记录表.生产指令ID={0} and 生产退料记录详细信息.领料日期时间 between #{1}# and #{2}# and 生产退料记录详细信息.班次='{3}'";
//                da = new OleDbDataAdapter(string.Format(sql, id, currDateTime.Date, currDateTime.AddDays(1).Date, fight), mySystem.Parameter.connOle);
//                dt = new DataTable();
//                da.Fill(dt);
//                if (dt.Rows.Count>0)
//                {
//                    DataRow[] drs_temp;
//                    // TY膜退料量
//                    drs_temp = dt.Select("物料简称='Tyvek印刷卷材'");
//                    sum_TY退料 = 0;
//                    foreach (DataRow ddr in drs_temp)
//                    {
//                        sum_TY退料 += Convert.ToDouble(ddr["退库数量"]);
//                    }

//                    // XP1膜退料量
//                    drs_temp = dt.Select("物料简称='药品包装用聚乙烯膜（XP1）'");
//                    sum_XP1退料 = 0;
//                    foreach (DataRow ddr in drs_temp)
//                    {
//                        sum_XP1退料 += Convert.ToDouble(ddr["退库数量"]);
//                    }

//                    //内包退料量
//                    drs_temp = dt.Select("物料简称='内包装袋'");
//                    sum_内包退料 = 0;
//                    foreach (DataRow ddr in drs_temp)
//                    {
//                        sum_内包退料 += Convert.ToDouble(ddr["退库数量"]);
//                    }

//                    //外包退料量
//                    drs_temp = dt.Select("物料简称='纸箱'");
//                    sum_外包退料 = 0;
//                    foreach (DataRow ddr in drs_temp)
//                    {
//                        sum_外包退料 += Convert.ToDouble(ddr["退库数量"]);
//                    }
//                }

//                sql = "select * from 生产领料使用记录,生产领料使用记录详细信息 where 生产领料使用记录详细信息.T生产领料使用记录ID=生产领料使用记录.ID and 生产领料使用记录.生产指令ID={0} and 生产领料使用记录详细信息.领料日期时间 between #{1}# and #{2}# and 生产领料使用记录详细信息.班次='{3}'";
//                da = new OleDbDataAdapter(string.Format(sql, id, currDateTime.Date, currDateTime.AddDays(1).Date, fight), mySystem.Parameter.connOle);
//                dt = new DataTable();
//                da.Fill(dt);
//                DataRow[] drs;
//                double sum;
//                if (dt.Rows.Count == 0)
//                {
//                    MessageBox.Show("生产指令ID为" + id + "的生产指令详细信息读取错误（无对应的领料日期）");
//                    dr["TY膜用量（米）"] = 0;
//                    dr["XP1膜用量（米）"] = 0;
//                    dr["内包装袋用量（只）"] = 0;
//                    dr["外包装袋用量"] = 0;
//                }
//                else
//                {
//                    // TY膜用量
//                    drs = dt.Select("物料简称='Tyvek印刷卷材'");
//                    dr["TY膜用量（米）"] = 0;
//                    sum = 0;
//                    foreach (DataRow ddr in drs)
//                    {
//                        sum += Convert.ToDouble(ddr["领取数量"]);
//                    }
//                    dr["TY膜用量（米）"] = sum - sum_TY退料;
//                    // XP1膜用量
//                    drs = dt.Select("物料简称='药品包装用聚乙烯膜（XP1）'");
//                    dr["XP1膜用量（米）"] = 0;
//                    sum = 0;
//                    foreach (DataRow ddr in drs)
//                    {
//                        sum += Convert.ToDouble(ddr["领取数量"]);
//                    }
//                    dr["XP1膜用量（米）"] = sum - sum_XP1退料;
//                    // 内包装袋用量
//                    drs = dt.Select("物料简称='内包装袋'");
//                    dr["内包装袋用量（只）"] = 0;
//                    sum = 0;
//                    foreach (DataRow ddr in drs)
//                    {
//                        sum += Convert.ToDouble(ddr["领取数量"]);
//                    }
//                    dr["内包装袋用量（只）"] = sum - sum_内包退料;
//                    // 外包装袋用量
//                    drs = dt.Select("物料简称='纸箱'");
//                    dr["外包装袋用量"] = 0;
//                    sum = 0;
//                    foreach (DataRow ddr in drs)
//                    {
//                        sum += Convert.ToDouble(ddr["领取数量"]);
//                    }
//                    dr["外包装袋用量"] = sum - sum_外包退料;
//                }
//            }

//            // 读取存货档案，完成计算
//            double h换算率;
//            foreach (DataRow dr in ret.Rows)
//            {
//                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//                OleDbConnection Tconn = new OleDbConnection(strConnect);
//                Tconn.Open();
//                // TY膜
//                sql = "select * from 设置存货档案 where 存货代码='{0}'";
//                da = new OleDbDataAdapter(string.Format(sql, ty膜代码), Tconn);
//                dt = new DataTable();
//                da.Fill(dt);
//                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
//                dr["TY膜用量（平米）"] = Math.Round(Convert.ToDouble(dr["TY膜用量（米）"]) / h换算率, 2);
//                // XP1膜
//                sql = "select * from 设置存货档案 where 存货代码='{0}'";
//                da = new OleDbDataAdapter(string.Format(sql, xp1膜代码), Tconn);
//                dt = new DataTable();
//                da.Fill(dt);
//                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
//                dr["XP1膜用量（平米）"] = Math.Round(Convert.ToDouble(dr["XP1膜用量（米）"]) / h换算率, 2);
//                // 产品
//                sql = "select * from 设置存货档案 where 存货代码='{0}'";
//                da = new OleDbDataAdapter(string.Format(sql, dr["产品代码"].ToString()), Tconn);
//                dt = new DataTable();
//                da.Fill(dt);
//                h换算率 = Convert.ToDouble(dt.Rows[0]["换算率"]);
//                dr["产品数量（平米）"] = Math.Round(Convert.ToDouble(dr["产品数量（只）"]) / h换算率);

//                dr["换算后产品数量"] = Math.Round( Convert.ToDouble(dr["产品数量（平米）"]) * Convert.ToDouble(dr["产品系数"]),2);
//                dr["工时效率"] = Math.Round(Convert.ToDouble(dr["换算后产品数量"]) / Convert.ToDouble(dr["工时(h)"]), 2);
//                dr["制袋收率(%)"] = Math.Round(100 * Convert.ToDouble(dr["产品数量（平米）"]) / (Convert.ToDouble(dr["XP1膜用量（平米）"]) + Convert.ToDouble(dr["TY膜用量（平米）"])), 2);
//            }

//            return ret;            
        }

        void setDGV(DataTable dt)
        {
            dataGridView1.DataSource = dt;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
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


        private void bt打印_Click(object sender, EventArgs e)
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

        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\PTV\13 SOP-MFG-305-R04A PTV生产台帐.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        //string log = "=====================================\n";
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                        //bsOuter.EndEdit();
                        //daOuter.Update((DataTable)bsOuter.DataSource);
                    }
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
            }
        }


        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            

            //内表信息
            int i内表行数 = dtShow.Rows.Count;
            int i超出行数 = 0;

            //插入新行
            if (dtShow.Rows.Count > 2)
            {
                i超出行数 = dtShow.Rows.Count - 2;
                for (int i = 0; i < i超出行数; i++)
                {
                    //在第6行插入
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[4 + i, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
            }




            for (int i = 0; i < i内表行数; i++)
            {
                mysheet.Cells[4 + i, 1].Value = dtShow.Rows[i]["生产日期"];
                mysheet.Cells[4 + i, 2].Value = dtShow.Rows[i]["班次"];
                mysheet.Cells[4 + i, 3].Value = dtShow.Rows[i]["生产指令号"];
                mysheet.Cells[4 + i, 4].Value = dtShow.Rows[i]["客户或订单号"];
                mysheet.Cells[4 + i, 5].Value = dtShow.Rows[i]["产品代码"];
                mysheet.Cells[4 + i, 6].Value = dtShow.Rows[i]["产品数量（只）"];
                mysheet.Cells[4 + i, 7].Value = dtShow.Rows[i]["批号"];
                
            }

           
            return mysheet;
        }

       

    }
}

