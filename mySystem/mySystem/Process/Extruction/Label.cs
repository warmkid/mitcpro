using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data.OleDb;


namespace mySystem.Process.Extruction
{
    public partial class LabelPrint : Form
    {

        Hashtable codeToBatch = new Hashtable();
        Hashtable codeToLabel = new Hashtable();

        //添加打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        


        public LabelPrint()
        {
            InitializeComponent();


            cc班次.Items.Add("白班");
            cc班次.Items.Add("夜班");
            cc班次.SelectedIndex = 0;

            c标签模板.Items.Add("吹膜半成品标签");
            c标签模板.Items.Add("内标签-英文照射");
            c标签模板.Items.Add("内标签-英文不照射");
            c标签模板.Items.Add("内标签-中文照射");
            c标签模板.Items.Add("内标签-中文不照射");
            c标签模板.Items.Add("外标签-中文照射");
            c标签模板.Items.Add("外标签-中文不照射");
            c标签模板.Items.Add("外箱-英文照射");
            c标签模板.Items.Add("外箱-英文不照射");


            getCodeAndBatch();
            foreach (string c in codeToBatch.Keys.OfType<String>().ToList<String>())
            {
                cmb膜代码.Items.Add(c);
            }
            cmb膜代码.SelectedIndexChanged += cmb膜代码_SelectedIndexChanged;

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                c打印机.Items.Add(sPrint);
            }
            c打印机.SelectedText = print.PrinterSettings.PrinterName;
        }

        void cmb膜代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            tc批号.Text = codeToBatch[cmb膜代码.SelectedItem].ToString();
            // 
            c标签模板.SelectedIndex = Convert.ToInt32(codeToLabel[cmb膜代码.SelectedItem]) - 1;
        }

        private void getCodeAndBatch()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
            System.Data.DataTable dt = new System.Data.DataTable("temp");
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                codeToBatch.Add(dr["产品编码"].ToString(), dr["产品批号"].ToString());
                codeToLabel.Add(dr["产品编码"].ToString(), Convert.ToInt32(dr["标签"]));
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

            printLable();
            GC.Collect();
        }

        private void printLable()
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            //File.Copy(@"../../xls/Extrusion/吹膜标签.xlsx", path + @"/label.xlsx", true);
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(path + @"/../../xls/Extrusion/吹膜标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            my.Cells[1, 2].Value = cmb膜代码.SelectedItem;
            my.Select();

            my.Cells[3, 2].Value = tc数量米.Text + "米；" + tc数量KG.Text + "KG";
            my.Cells[4, 2].Value = dc日期.Value.ToShortDateString() + "     " +
                (cc班次.SelectedItem.ToString() == "白班" ? "白班" : "夜班");

            my.Cells[6, 2].Value = tc产品名称.Text;
            my.Cells[7, 2].Value = tc产品编码.Text;
            my.Cells[8, 2].Value = tc产品规格.Text;
            my.Cells[9, 2].Value = tc产品批号.Text;
            my.Cells[10, 2].Value = dc生产日期.Value.ToShortDateString();
            my.Cells[11, 2].Value = dc有效期至.Value.ToShortDateString();
            my.Cells[12, 2].Value = tc数量.Text;
            my.Cells[13, 2].Value = tc包装序号.Text;
            my.Cells[15, 2].Value = tc注册证号.Text;
            my.Cells[16, 2].Value = tc毛重.Text;
            my.Cells[17, 2].Value = tc箱体规格.Text;
            my.Cells[6, 5].Value = teName.Text;
            my.Cells[7, 5].Value = teCode.Text;
            my.Cells[8, 5].Value = teSize.Text;
            my.Cells[9, 5].Value = teBatch.Text;
            my.Cells[10, 5].Value = deMfg.Value.ToShortDateString();
            my.Cells[11, 5].Value = deExpiry.Value.ToShortDateString();
            my.Cells[12, 5].Value = teQuantity.Text;
            my.Cells[13, 5].Value = tePack.Text;
            my.Cells[15, 6].Value = teCFDA.Text;
            my.Cells[16, 6].Value = teGross.Text;
            my.Cells[17, 6].Value = teCarton.Text;

            my = wb.Worksheets[c标签模板.SelectedIndex+1];
            my.Select();
            oXL.Visible = true;
            my.PrintOut();

            wb.Close(false);
            oXL.Quit();
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
            wb = null;
            oXL = null;
        }
  
    }
}
