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
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPE标签 : Form
    {
        //注意：英文生产日期是 日/月/年

        bool is内标签;
        string 名称, 编码, 规格, 批号;
        double 数量, 序号;
        DateTime 生产日期, 有效期;

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
                tc注册证号.Visible = true;
                label5.Visible = true;
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
            OleDbDataAdapter da;
            System.Data.DataTable dt;
            da = new OleDbDataAdapter("select * from 生产指令 where ID=" + mySystem.Parameter.ldpebagInstruID, mySystem.Parameter.connOle);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            名称 = dt.Rows[0]["产品名称"].ToString();
            tc产品名称.Text = 名称;

            da = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + mySystem.Parameter.ldpebagInstruID, mySystem.Parameter.connOle);
            dt = new System.Data.DataTable();
            da.Fill(dt);
            编码 = dt.Rows[0]["产品代码"].ToString();
            tc产品编码.Text = 编码;
            批号 = dt.Rows[0]["产品批号"].ToString();
            tc产品批号.Text = 批号;
        }

        private void c打印机_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultPrinter(c打印机.SelectedItem.ToString());
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (c标签模板.SelectedIndex < 0)
            {
                MessageBox.Show("请选择一个模板再打印");
                return;
            }

            printLable(is内标签);
            GC.Collect();
        }

        void printLable(bool flag)
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            if (flag)
                wb = oXL.Workbooks.Open(path + @"/../../xls/LDPEBag/LDPE 制袋内包标签.xlsx");
            else
                wb = oXL.Workbooks.Open(path + @"/../../xls/LDPEBag/LDPE 制袋外包标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

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
            my.Cells[11, 2].Value = tc注册证号.Text;


            my.Cells[1, 5].Value = teName.Text;
            my.Cells[2, 5].Value = teCode.Text;
            my.Cells[3, 5].Value = teSize.Text;
            my.Cells[4, 5].Value = teBatch.Text;
            my.Cells[5, 5].Value = deMfg.Value.ToString("dd/MM/yyyy");
            my.Cells[6, 5].Value = deExpiry.Value.ToString("MM/yyyy");
            my.Cells[7, 5].Value = teQuantity.Text;
            my.Cells[8, 5].Value = tePack.Text;
            my.Cells[9, 5].Value = tegross.Text;
            my.Cells[10, 5].Value = teCarton.Text;

            my = wb.Worksheets[c标签模板.SelectedIndex + 1];
            my.Select();
            oXL.Visible = false;
            my.PrintOut();
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
