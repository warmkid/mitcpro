using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPE标签 : Form
    {
        //注意：英文生产日期是 日/月/年

        bool is内标签;
        string 名称, 编码, 规格, 批号;
        string name;
        double 数量, 序号;
        DateTime 生产日期, 有效期;
        bool isSavedInDatabase;

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        public LDPE标签(bool flag)
        {
            is内标签 = flag;
            InitializeComponent();

            if (flag)
            {
                this.Text = "内标签";
                tc毛重.Visible = false;
                tc箱体规格.Visible = false;
                tegross.Visible = false;
                teCarton.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                tc注册证号.Visible = false;
                label5.Visible = false;
                checkBox1.Visible = false;
                teOrder.Visible = false;
                label6.Visible = false;
            }
            else
            {
                this.Text = "外标签";
                tc毛重.Visible = true;
                tc箱体规格.Visible = true;
                tegross.Visible = true;
                teCarton.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                tc注册证号.Visible = false;
                label5.Visible = false;
                checkBox1.Visible = true;
                teOrder.Visible = true;
                label6.Visible = true;
                c标签模板.Items.Add("进口");
                c标签模板.Items.Add("中文-不体现储存条件");
                c标签模板.Items.Add("英文-不体现重量");
            }

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                c打印机.Items.Add(sPrint);
            }
            c打印机.SelectedItem = print.PrinterSettings.PrinterName;
            c打印机.SelectedIndexChanged += new EventHandler(c打印机_SelectedIndexChanged);

            getData();

        }

        void getData()
        {
            // 产品名称
            SqlDataAdapter da;
            System.Data.DataTable dt;
            da = new SqlDataAdapter("select * from 生产指令 where ID=" + mySystem.Parameter.ldpebagInstruID, mySystem.Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            名称 = dt.Rows[0]["产品名称"].ToString();
            name = dt.Rows[0]["产品英文名称"].ToString();
            tc产品名称.Text = 名称;
            teName.Text = name;

            

            // 箱体规格
            string[] outSize = new string[3];
            outSize[0] = dt.Rows[0]["外包物料代码1"].ToString();
            outSize[1] = dt.Rows[0]["外包物料代码2"].ToString();
            outSize[2] = dt.Rows[0]["外包物料代码3"].ToString();
            string foundSize = "";
            foundSize = Utility.get外包规格(outSize);
            if (foundSize != "")
            {
                string[] ckg = foundSize.Split('X');
                StringBuilder sb1 = new StringBuilder();
                for (int i = 0; i < ckg.Length - 1; ++i)
                {
                    sb1.Append(ckg[i]);
                    sb1.Append("mmX");
                }
                sb1.Append(ckg[ckg.Length - 1]);
                sb1.Append("mm");
                tc箱体规格.Text = sb1.ToString();
                teCarton.Text = sb1.ToString();
            }

            // 内/外包数量
            da = new SqlDataAdapter(string.Format(
                "select * from 生产指令详细信息 where T生产指令ID={0}", mySystem.Parameter.ldpebagInstruID), Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            int n = 0;
            if (dt.Rows.Count >= 0)
            {
                if (is内标签)
                {
                    n = Convert.ToInt32(dt.Rows[0]["内包装规格每包只数"]);
                }
                else
                {
                    n = Convert.ToInt32(dt.Rows[0]["外包规格"]);
                }
            }
            tc数量.Text = n.ToString();
            teQuantity.Text = n.ToString();


            // 产品编码，批号
            da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + mySystem.Parameter.ldpebagInstruID, mySystem.Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            编码 = dt.Rows[0]["产品代码"].ToString();
            tc产品编码.Text = 编码;
            teCode.Text = 编码;
            批号 = dt.Rows[0]["产品批号"].ToString();
            tc产品批号.Text = 批号;
            teBatch.Text = 批号;

            // 产品规格
            string[] ss = 编码.ToString().Split('-');
            string guige = ss[ss.Length - 1];
            string[] nums = guige.Split('X');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nums.Length - 1; ++i)
            {
                sb.Append(nums[i]);
                sb.Append("mmX");
            }
            double last = Math.Round( Convert.ToDouble( nums[nums.Length - 1])/1000.0,2);

            sb.Append(last.ToString("F2"));
            sb.Append("mm");
            tc产品规格.Text = sb.ToString();
            teSize.Text = sb.ToString();

            // 日期
            string tmp = tc产品批号.Text.Substring(0, 8);
            DateTime startDT = new DateTime(Convert.ToInt32(tmp.Substring(0, 4)),
                Convert.ToInt32(tmp.Substring(4, 2)),
                Convert.ToInt32(tmp.Substring(6, 2)));
            dc生产日期.Value = startDT;
            deMfg.Value = startDT;
            dc有效期至.Value = startDT.AddYears(3).AddMonths(-1);
            deExpiry.Value = startDT.AddYears(3).AddMonths(-1);

            

        }

        private void c打印机_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultPrinter(c打印机.SelectedItem.ToString());
        }

        bool checkIsSavedToDatabase()
        {
            string sql = "select * from {0} where 生产指令ID={1}";
            SqlDataAdapter da;
            System.Data.DataTable dt;
            if (is内标签)
            {
                sql = string.Format(sql, "内标签", Parameter.ldpebagInstruID);
            }
            else
            {
                sql = string.Format(sql, "外标签", Parameter.ldpebagInstruID);
            }
            da = new SqlDataAdapter(sql, Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void saveToDateBase()
        {
            // 中英文存一样的东西
            // 根据cmb决定
            string sql = "";
            SqlCommand comm = null;
            if (is内标签)
            {
                sql = "INSERT INTO 内标签 (生产指令ID,产品名称,产品代码,产品规格,产品批号 ,生产日期,有效期,数量 ,包装序号" +
                  ",Name,Code,Size,BatchNo,MfgDate ,Expiry,Quantity,NO,模板) " +
                  "values ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},{8}," +
                  "'{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},'{9}')";
                if(c标签模板.SelectedIndex==0){
                    // 中文
                    sql = string.Format(sql, Parameter.ldpebagInstruID, tc产品名称.Text, tc产品编码.Text, tc产品规格.Text,
                        tc产品批号.Text, dc生产日期.Value, dc有效期至.Value, tc数量.Text, tc包装序号.Text,"中文");
                }else{
                    sql = string.Format(sql, Parameter.ldpebagInstruID, teName.Text, teCode.Text, teSize.Text,
                        teBatch.Text, deMfg.Value, deExpiry.Value, teQuantity.Text, tePack.Text,"英文");
                }
                
                comm = new SqlCommand(sql,Parameter.conn);
            }
            else
            {
                sql = "INSERT INTO 外标签 (生产指令ID,产品名称,产品代码,产品规格,产品批号 ,生产日期,有效期,数量 ,包装序号" +
                  ",Name,Code,Size,BatchNo,MfgDate ,Expiry,Quantity,NO" +
                  ",毛重,箱体规格,注册证号" +
                  ",GrossWeight,CartonSize,模板) " +
                  "values ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},{8}," +
                  "'{1}','{2}','{3}','{4}','{5}','{6}',{7},{8}," +
                  "{9},'{10}','{11}',{9},'{10}','{12}')";
                if (c标签模板.SelectedIndex == 0)
                {
                    // 中文
                    sql = string.Format(sql, Parameter.ldpebagInstruID, tc产品名称.Text, tc产品编码.Text, tc产品规格.Text,
                        tc产品批号.Text, dc生产日期.Value, dc有效期至.Value, tc数量.Text, tc包装序号.Text,
                        tc毛重.Text, tc箱体规格.Text, tc注册证号.Text,"中文");
                }
                else
                {
                    sql = string.Format(sql, Parameter.ldpebagInstruID, teName.Text, teCode.Text, teSize.Text,
                        teBatch.Text, deMfg.Value, deExpiry.Value, teQuantity.Text, tePack.Text,
                        tegross.Text, teCarton.Text, "","英文");
                }

                comm = new SqlCommand(sql, Parameter.conn);
            }
            comm.ExecuteNonQuery();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (c标签模板.SelectedIndex < 0)
            {
                MessageBox.Show("请选择一个模板再打印");
                return;
            }
            
            // 查询是否已经打印，并设置isPrinted
            isSavedInDatabase = checkIsSavedToDatabase();
            //isSavedInDatabase = false;
           


            // 单次打印
            int xuhao;
            if (Int32.TryParse(c标签模板.SelectedIndex == 0 ? tc包装序号.Text : tePack.Text, out xuhao))
            {
                printLable(false, is内标签);
                if (checkBox1.Checked)
                {
                    printLable(false, is内标签);
                }
                if (!isSavedInDatabase)
                {
                    saveToDateBase();
                }
            }
            // 多次打印
            else
            {
                try
                {
                    int start, end;
                    string[] ss = (c标签模板.SelectedIndex == 0 ? tc包装序号.Text.Split('-') : tePack.Text.Split('-'));
                    start = Convert.ToInt32(ss[0]);
                    end = Convert.ToInt32(ss[1]);
                    for (int i = start; i <= end; ++i)
                    {
                        if (c标签模板.SelectedIndex == 0)
                        {
                            tc包装序号.Text = i.ToString();
                        }
                        else
                        {
                            tePack.Text = i.ToString();
                        }
                        
                        if (!isSavedInDatabase && i == start)
                        {
                            saveToDateBase();
                        }
                        printLable(false,is内标签);
                        if (checkBox1.Checked)
                        {
                            printLable(false,is内标签);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("包装序号有误！");
                    return;
                }
            }
            
            GC.Collect();
        }

        void printLable(bool flag, bool inOrOut)
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            if (inOrOut)
                wb = oXL.Workbooks.Open(path + @"/../../xls/LDPEBag/LDPE 制袋内包标签.xlsx");
            else
                wb = oXL.Workbooks.Open(path + @"/../../xls/LDPEBag/LDPE 制袋外包标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            my.Select();
            my.Cells[1, 2].Value = tc产品名称.Text;
            my.Cells[2, 2].Value = tc产品编码.Text;
            my.Cells[3, 2].Value = tc产品规格.Text;
            my.Cells[4, 2].Value = tc产品批号.Text;
            my.Cells[5, 2].Value = dc生产日期.Value.ToString("yyyy.MM.dd");
            my.Cells[6, 2].Value = dc有效期至.Value.ToString("yyyy.MM");
            my.Cells[7, 2].Value = tc数量.Text;
            my.Cells[8, 2].Value = tc包装序号.Text;
            my.Cells[9, 2].Value = tc毛重.Text;
            my.Cells[10, 2].Value = tc箱体规格.Text;
            //my.Cells[11, 2].Value = tc注册证号.Text;


            my.Cells[1, 5].Value = teName.Text;
            my.Cells[2, 5].Value = teCode.Text;
            my.Cells[3, 5].Value = teSize.Text;
            my.Cells[4, 5].Value = teBatch.Text;
            my.Cells[5, 5].Value = deMfg.Value.ToString("dd.MM.yyyy");
            my.Cells[6, 5].Value = deExpiry.Value.ToString("MM.yyyy");
            my.Cells[7, 5].Value = teQuantity.Text;
            my.Cells[8, 5].Value = tePack.Text;
            my.Cells[9, 5].Value = tegross.Text;
            my.Cells[10, 5].Value = teCarton.Text;
            my.Cells[12, 5].Value = teOrder.Text;

            my = wb.Worksheets[c标签模板.SelectedIndex + 1];
            my.Select();
            if (flag)
            {
                oXL.Visible = true;
                my.PrintPreview(true);
            }
            else
            {
                //MessageBox
                //    .Show(
                try
                {
                    oXL.Visible = false;
                    my.PrintOut();
                }
                catch
                {
                }
            }
            //oXL.Visible = true;
            //my.PrintPreview(true);
            //my.PrintOut();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (c标签模板.SelectedIndex < 0)
            {
                MessageBox.Show("请选择一个模板");
                return;
            }

            



            // 单次打印
            printLable(true, is内标签);
            //int xuhao;
            //if (Int32.TryParse(c标签模板.SelectedIndex == 0 ? tc包装序号.Text : tePack.Text, out xuhao))
            //{
            //    printLable(true);


            //}
            //else
            //{
            //    MessageBox.Show("请输入包装序号");
            //}
            
            

            GC.Collect();
        }


    }
}
