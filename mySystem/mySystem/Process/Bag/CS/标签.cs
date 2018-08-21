using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace mySystem.Process.Bag.CS
{
    public partial class 标签 : Form
    {

        bool is内标签;
        string 名称,编码,规格,批号;
        double 数量, 序号;
        DateTime 生产日期, 有效期;



        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);

        public 标签(bool flag)
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

        void c打印机_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SetDefaultPrinter(c打印机.SelectedItem.ToString());
        }

        void getData()
        {
            SqlDataAdapter da;
            System.Data.DataTable dt;
            da = new SqlDataAdapter("select * from 生产指令 where ID="+mySystem.Parameter.csbagInstruID,mySystem.Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            名称 = dt.Rows[0]["产品名称"].ToString();
            tc产品名称.Text = 名称;

            da = new SqlDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.conn);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            编码 = dt.Rows[0]["产品代码"].ToString();
            tc产品编码.Text = 编码;
            批号 = dt.Rows[0]["产品批号"].ToString();
            tc产品批号.Text = 批号;


            string[] ss = 编码.ToString().Split('-');
            string guige = ss[ss.Length - 1];
            string[] nums = guige.Split('X');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nums.Length - 1; ++i)
            {
                sb.Append(nums[i]);
                sb.Append("mmX");
            }
            double last = Math.Round(Convert.ToDouble(nums[nums.Length - 1]) / 1000.0, 2);

            sb.Append(last.ToString("F2"));
            sb.Append("mm");
            tc产品规格.Text = sb.ToString();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (c标签模板.SelectedIndex < 0)
            {
                MessageBox.Show("请选择一个模板再打印");
                return;
            }



            // 单次打印
            int xuhao;
            if (Int32.TryParse(tc包装序号.Text, out xuhao))
            {
                printLable(is内标签);
            }
            // 多次打印
            else
            {
                try
                {
                    int start, end;
                    string[] ss = tc包装序号.Text.Split('-');
                    start = Convert.ToInt32(ss[0]);
                    end = Convert.ToInt32(ss[1]);
                    for (int i = start; i <= end; ++i)
                    {
                        tc包装序号.Text = i.ToString();
                        printLable(is内标签);
                    }
                }
                catch
                {
                    MessageBox.Show("包装序号有误！");
                    return;
                }
            }

            //printLable(is内标签);
            GC.Collect();
        }

        void printLable(bool flag)
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            //File.Copy(@"../../xls/Extrusion/吹膜标签.xlsx", path + @"/label.xlsx", true);
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            if(flag)
                wb = oXL.Workbooks.Open(path + @"/../../xls/CSBag/标签 CS 制袋 内标签.xlsx");
            else
                wb = oXL.Workbooks.Open(path + @"/../../xls/CSBag/标签 CS 制袋 外包标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            ;
            my.Select();
            my.Cells[1, 2].Value = tc产品名称.Text;
            my.Cells[2, 2].Value = tc产品编码.Text;
            my.Cells[3, 2].Value = tc产品规格.Text;
            my.Cells[4, 2].Value = tc产品批号.Text;
            my.Cells[5, 2].Value = dc生产日期.Value.ToString("yyyy/MM/dd");
            my.Cells[6, 2].Value = dc有效期至.Value.ToString("yyyy/MM");
            my.Cells[7, 2].Value = tc数量.Text;
            my.Cells[8, 2].Value = tc包装序号.Text;
            my.Cells[9, 2].Value = tc毛重.Text;
            my.Cells[10, 2].Value = tc箱体规格.Text;


            my.Cells[1, 5].Value = teName.Text;
            my.Cells[2, 5].Value = teCode.Text;
            my.Cells[3, 5].Value = teSize.Text;
            my.Cells[4, 5].Value = teBatch.Text;
            my.Cells[5, 5].Value = deMfg.Value.ToString("yyyy/MM/dd");
            my.Cells[6, 5].Value = deExpiry.Value.ToString("yyyy/MM");
            my.Cells[7, 5].Value = teQuantity.Text;
            my.Cells[8, 5].Value = tePack.Text;
             my.Cells[9, 5].Value = tegross.Text;
             my.Cells[10, 5].Value = teCarton.Text;

            my = wb.Worksheets[c标签模板.SelectedIndex + 1];
            my.Select();
            //oXL.Visible = true;
            try
            {
                my.PrintOut();
            }
            catch
            {
            }

            wb.Close(false);
            oXL.Quit();
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
            wb = null;
            oXL = null;
        }
    }
}
