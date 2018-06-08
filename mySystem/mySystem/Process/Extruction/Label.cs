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
using System.Data.SqlClient;


namespace mySystem.Process.Extruction
{
    public partial class LabelPrint : Form
    {

        Hashtable codeToBatch = new Hashtable();
        Hashtable codeToLabel = new Hashtable();
        String _s;

        //添加打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        


        public LabelPrint(String s)
        {
            _s = s;
            InitializeComponent();


            cc班次.Items.Add("白班");
            cc班次.Items.Add("夜班");
            cc班次.SelectedIndex = 0;

            cc质量状态.Items.Add("待验");
            cc质量状态.Items.Add("合格");
            cc质量状态.Items.Add("不合格");
            cc质量状态.SelectedIndex = 0;

            tc操作人.Text = mySystem.Parameter.userName;

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
            c打印机.SelectedItem = print.PrinterSettings.PrinterName;
            c打印机.SelectedIndexChanged += c打印机_SelectedIndexChanged;
        }

        void c打印机_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultPrinter(c打印机.SelectedItem.ToString());
        }

        void cmb膜代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from 已打印标签 where 生产指令='{0}' and 产品代码='{1}'";
            SqlDataAdapter da = new SqlDataAdapter( String.Format(sql, mySystem.Parameter.proInstruction,
                cmb膜代码.SelectedItem.ToString()), mySystem.Parameter.conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            System.Data.DataTable dt = new System.Data.DataTable("temp");
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["生产指令"] = mySystem.Parameter.proInstruction;
                dr["产品代码"] = cmb膜代码.SelectedItem.ToString();
                dr["卷号"] = 1;
                dt.Rows.Add(dr);
                da.Update(dt);
            }
            


            tc批号.Text = codeToBatch[cmb膜代码.SelectedItem].ToString() + " - " + dt.Rows[0]["卷号"].ToString();


            dt.Rows[0]["卷号"] = 1 + Convert.ToInt32(dt.Rows[0]["卷号"]);
            da.Update(dt);


            // 
            c标签模板.SelectedIndex = Convert.ToInt32(codeToLabel[cmb膜代码.SelectedItem]) - 1;
        }

        private void getCodeAndBatch()
        {
            if (!mySystem.Parameter.isSqlOk)
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
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.conn);
                System.Data.DataTable dt = new System.Data.DataTable("temp");
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    codeToBatch.Add(dr["产品编码"].ToString(), dr["产品批号"].ToString());
                    codeToLabel.Add(dr["产品编码"].ToString(), Convert.ToInt32(dr["标签"]));
                }
            }
            
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (c标签模板.SelectedIndex < 0)
            {
                MessageBox.Show("请选择一个模板再打印");
                return;
            }
            
            // 判断，如果数据库有存过一张标签，则不管，否则保存一张标签
            string sql = "select * from 标签 where 生产指令='{0}'";
            System.Data.DataTable dt = new System.Data.DataTable();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, _s), mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["生产指令ID"] = mySystem.Parameter.proInstruID;
                    dr["生产指令"] = _s;
                    dr["标签类型"] = c标签模板.SelectedIndex;
                    dr["膜代码"] = cmb膜代码.SelectedItem;
                    dr["批号中文"] = tc批号.Text;
                    dr["数量米"] = tc数量米.Text;
                    dr["数量千克"] = tc数量KG.Text;
                    dr["日期中文"] = dc日期.Value;
                    dr["班次中文"] = (cc班次.SelectedItem.ToString() == "白班" ? "白班" : "夜班");
                    dr["产品名称中文"] = tc产品名称.Text;
                    dr["产品编码中文"] = tc产品编码.Text;
                    dr["产品规格中文"] = tc产品规格.Text;
                    dr["产品批号中文"] = tc产品批号.Text;
                    dr["生产日期中文"] = dc生产日期.Value;
                    dr["有效期至中文"] = dc有效期至.Value;
                    dr["数量中文"] = tc数量.Text;
                    dr["包装序号中文"] = tc包装序号.Text;
                    dr["注册证号中文"] = tc注册证号.Text;
                    dr["毛重中文"] = tc毛重.Text;
                    dr["箱体规格中文"] = tc箱体规格.Text;
                    dr["Name_E"] = teName.Text;
                    dr["Code_E"] = teCode.Text;
                    dr["Size_E"] = teSize.Text;
                    dr["Batch_E"] = teBatch.Text;
                    dr["Mfg_E"] = deMfg.Value;
                    dr["Expiry_E"] = deExpiry.Value;
                    dr["Quantity_E"] = teQuantity.Text;
                    dr["Pack_E"] = tePack.Text;
                    dr["CFDA_E"] = teCFDA.Text;
                    dr["Gross_E"] = teGross.Text;
                    dr["Carton_E"] = teCarton.Text;
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, _s), mySystem.Parameter.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["生产指令ID"] = mySystem.Parameter.proInstruID;
                    dr["生产指令"] = _s;
                    dr["标签类型"] = c标签模板.SelectedIndex;

                    dr["质量状态"] = cc质量状态.SelectedItem.ToString();
                    dr["操作人"] = tc操作人.Text;
                    dr["备注"] = tc备注.Text;

                    dr["膜代码"] = cmb膜代码.SelectedItem;
                    dr["批号中文"] = tc批号.Text;
                    dr["数量米"] = tc数量米.Text;
                    dr["数量千克"] = tc数量KG.Text;
                    dr["日期中文"] = dc日期.Value;
                    dr["班次中文"] = (cc班次.SelectedItem.ToString() == "白班" ? "白班" : "夜班");
                    dr["产品名称中文"] = tc产品名称.Text;
                    dr["产品编码中文"] = tc产品编码.Text;
                    dr["产品规格中文"] = tc产品规格.Text;
                    dr["产品批号中文"] = tc产品批号.Text;
                    dr["生产日期中文"] = dc生产日期.Value;
                    dr["有效期至中文"] = dc有效期至.Value;
                    dr["数量中文"] = tc数量.Text;
                    dr["包装序号中文"] = tc包装序号.Text;
                    dr["注册证号中文"] = tc注册证号.Text;
                    dr["毛重中文"] = tc毛重.Text;
                    dr["箱体规格中文"] = tc箱体规格.Text;
                    dr["Name_E"] = teName.Text;
                    dr["Code_E"] = teCode.Text;
                    dr["Size_E"] = teSize.Text;
                    dr["Batch_E"] = teBatch.Text;
                    dr["Mfg_E"] = deMfg.Value;
                    dr["Expiry_E"] = deExpiry.Value;
                    dr["Quantity_E"] = teQuantity.Text;
                    dr["Pack_E"] = tePack.Text;
                    dr["CFDA_E"] = teCFDA.Text;
                    dr["Gross_E"] = teGross.Text;
                    dr["Carton_E"] = teCarton.Text;
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
            }
            
           
            

            printLable();
            GC.Collect();
        }

        public static void printLable(int id)
        {
            string sql = "select * from 标签 where ID={0}";
            OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, id), mySystem.Parameter.connOle);
            System.Data.DataTable dt = new System.Data.DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未找到标签信息，无法打印");
                return;
            }
            
            DataRow dr = dt.Rows[0];
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            //File.Copy(@"../../xls/Extrusion/吹膜标签.xlsx", path + @"/label.xlsx", true);
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(path + @"/../../xls/Extrusion/吹膜标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            my.Cells[1, 2].Value = dr["膜代码"];
            my.Select();
            my.Cells[2, 2].Value = dr["批号中文"];
            my.Cells[3, 2].Value = dr["数量米"] + "米；" + dr["数量千克"] + "KG";
            my.Cells[4, 2].Value = Convert.ToDateTime(dr["日期中文"]).ToString("yyyy/MM/dd") + "     " +
                (dr["班次中文"]);

            my.Cells[22, 2].Value = dr["质量状态"];
            my.Cells[23, 2].Value = dr["操作人"];
            my.Cells[24, 2].Value = dr["备注"];


            my.Cells[6, 2].Value = dr["产品名称中文"];
            my.Cells[7, 2].Value = dr["产品编码中文"];
            my.Cells[8, 2].Value = dr["产品规格中文"];
            my.Cells[9, 2].Value = dr["产品批号中文"];
            my.Cells[10, 2].Value = Convert.ToDateTime(dr["生产日期中文"]).ToString("yyyy/MM/dd");
            my.Cells[11, 2].Value = Convert.ToDateTime(dr["有效期至中文"]).ToString("yyyy/MM/dd");
            my.Cells[12, 2].Value = dr["数量中文"];
            my.Cells[13, 2].Value = dr["包装序号中文"];
            my.Cells[15, 2].Value = dr["注册证号中文"];
            my.Cells[16, 2].Value = dr["毛重中文"];
            my.Cells[17, 2].Value = dr["箱体规格中文"];
            my.Cells[6, 5].Value = dr["Name_E"];
            my.Cells[7, 5].Value = dr["Code_E"];
            my.Cells[8, 5].Value = dr["Size_E"];
            my.Cells[9, 5].Value = dr["Batch_E"];
            my.Cells[10, 5].Value = Convert.ToDateTime(dr["Mfg_E"]).ToString("yyyy/MM/dd");
            my.Cells[11, 5].Value = Convert.ToDateTime(dr["Expiry_E"]).ToString("yyyy/MM/dd");
            my.Cells[12, 5].Value = dr["Quantity_E"];
            my.Cells[13, 5].Value = dr["Pack_E"];
            my.Cells[15, 6].Value =dr["CFDA_E"];
            my.Cells[16, 6].Value = dr["Gross_E"];
            my.Cells[17, 6].Value = dr["Carton_E"];

            my = wb.Worksheets[Convert.ToInt32( dr["标签类型"]) + 1];
            my.Select();
            oXL.Visible = true;
            try
            {
                my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
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

        private void printLable()
        {
            string path = Directory.GetCurrentDirectory();
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            //File.Copy(@"../../xls/Extrusion/吹膜标签.xlsx", path + @"/label.xlsx", true);
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(path + @"/../../xls/Extrusion/吹膜标签.xlsx");
            _Worksheet my = wb.Worksheets[wb.Worksheets.Count];
            oXL.Visible = false;
            my.Cells[1, 2].Value = cmb膜代码.SelectedItem;
            my.Select();
            my.Cells[2, 2].Value = tc批号.Text;
            my.Cells[3, 2].Value = tc数量米.Text + "米；" + tc数量KG.Text + "KG";
            my.Cells[4, 2].Value = dc日期.Value.ToShortDateString() + "     " +
                (cc班次.SelectedItem.ToString() == "白班" ? "白班" : "夜班");
            my.Cells[22, 2].Value = cc质量状态.SelectedItem.ToString();
            my.Cells[23, 2].Value = tc操作人.Text;
            my.Cells[24, 2].Value = tc备注.Text;

            my.Cells[6, 2].Value = tc产品名称.Text;
            my.Cells[7, 2].Value = tc产品编码.Text;
            my.Cells[8, 2].Value = tc产品规格.Text;
            my.Cells[9, 2].Value = tc产品批号.Text;
            my.Cells[10, 2].Value = dc生产日期.Value.ToString("yyyy/MM/dd");
            my.Cells[11, 2].Value = dc有效期至.Value.ToString("yyyy/MM/dd");
            my.Cells[12, 2].Value = tc数量.Text;
            my.Cells[13, 2].Value = tc包装序号.Text;
            my.Cells[15, 2].Value = tc注册证号.Text;
            my.Cells[16, 2].Value = tc毛重.Text;
            my.Cells[17, 2].Value = tc箱体规格.Text;
            my.Cells[6, 5].Value = teName.Text;
            my.Cells[7, 5].Value = teCode.Text;
            my.Cells[8, 5].Value = teSize.Text;
            my.Cells[9, 5].Value = teBatch.Text;
            my.Cells[10, 5].Value = deMfg.Value.ToString("yyyy/MM/dd");
            my.Cells[11, 5].Value = deExpiry.Value.ToString("yyyy/MM/dd");
            my.Cells[12, 5].Value = teQuantity.Text;
            my.Cells[13, 5].Value = tePack.Text;
            my.Cells[15, 6].Value = teCFDA.Text;
            my.Cells[16, 6].Value = teGross.Text;
            my.Cells[17, 6].Value = teCarton.Text;

            my = wb.Worksheets[c标签模板.SelectedIndex+1];
            my.Select();
            
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
