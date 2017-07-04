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


namespace mySystem.Process.Extruction
{
    public partial class LabelPrint : Form
    {
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
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            File.Copy(@"../../../吹膜标签Cmmon.xlsx", path + @"/label.xlsx", true);
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(path + @"/label.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            // my.Cells[6, 7].Value = "1";
            my.Cells[1, 2].Value = tc膜代码.Text;
            String pihao = tc批号.Text;
            if (!File.Exists(@"./pihaojuanhao"))
            {
                File.WriteAllText(@"./pihaojuanhao", "{}");
            }
            StreamReader sr = File.OpenText(@"./pihaojuanhao");
            JObject jobject = JObject.Parse(sr.ReadToEnd());
            sr.Close();

            if (null == jobject[pihao])
            {
                jobject[pihao] = 1;
            }
            else
            {
                jobject[pihao] = Convert.ToInt32(jobject[pihao]) + 1;
            }
            File.WriteAllText(@"./pihaojuanhao", jobject.ToString());
            my.Cells[2, 2].Value = tc批号.Text + "-" + jobject[pihao];

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



            wb.Save();
            wb.Close();
            oXL.Quit();
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
        }

  
    }
}
