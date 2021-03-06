﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data.OleDb;
using ZXing;

namespace mySystem.Other
{
    public partial class 二维码打印 : Form
    {
        public static 二维码打印 create(String daima, string pihao)
        {
            二维码打印 ret = new 二维码打印();
            ret.tb产品代码.Text = daima;
            ret.tb产品批号.Text = pihao;
            ret.tb二维码.ReadOnly = true;
            return ret;
        }

        string strConnect;
        //OleDbConnection conn;
        SqlConnection conn;
        

        public 二维码打印()
        {
            InitializeComponent();
            fill_printer();
            //strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
            //                    Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
            //conn = new OleDbConnection(strConnect);

            strConnect = @"server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            conn = new SqlConnection(strConnect);
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印机.Items.Add(sPrint);
            }
            cmb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void btn关闭_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            // TODO 打印
            // 打印二维码
            if (tb二维码.Text == "") return;
            ZXing.BarcodeWriter bw = new BarcodeWriter();

            bw.Format = BarcodeFormat.QR_CODE;
            Bitmap b = bw.Write(tb二维码.Text);
            b.Save("code.bmp");


            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += pd_PrintPage;
            PrintDialog pdlg = new PrintDialog();
            pdlg.Document = pd;
            if (pdlg.ShowDialog() != DialogResult.OK) return;
            pd.Print();

            
            string sql;
            //OleDbDataAdapter da;
            SqlDataAdapter da;
            DataTable dt;
            SqlCommandBuilder cb;
            int yid = 0;
            if (tb原二维码.Text.Trim() != "")
            {
                sql = "select * from 二维码信息 where 二维码='{0}'";
                //da = new OleDbDataAdapter(String.Format(sql, tb原二维码.Text.Trim()), conn);
                da = new SqlDataAdapter(String.Format(sql, tb原二维码.Text.Trim()), conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    yid = Convert.ToInt32(dt.Rows[0]["库存ID"]);
                }
            }
            
            sql = "select * from 二维码信息 where 二维码='{0}'";
            //da = new OleDbDataAdapter(String.Format(sql, tb二维码.Text), conn);
            da = new SqlDataAdapter(String.Format(sql, tb二维码.Text), conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                DataRow dr = dt.NewRow();
                dr["二维码"] = tb二维码.Text;
                dr["库存ID"] = yid;
                dr["数量"] = 0;
                dt.Rows.Add(dr);
                da.Update(dt);
            }

           

        }

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float w = e.MarginBounds.Width;
            float h = e.MarginBounds.Height;

            e.Graphics.DrawImage(Image.FromFile("code.bmp"), x, y, w, h);
        }

        private void btn生成二维码_Click(object sender, EventArgs e)
        {
            string prefix = tb产品代码.Text + "@" + tb产品批号.Text + "@";
            int num = 1;
            string sql = "select top 1 二维码 from 二维码信息 where 二维码 like '%{0}%' order by ID DESC";
            //OleDbDataAdapter da = new OleDbDataAdapter(String.Format(sql, prefix), conn);
            SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, prefix), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                String[] s = dt.Rows[0]["二维码"].ToString().Split('@');
                num = Convert.ToInt32(s[2]);
                num++;
            }
            tb二维码.Text = prefix + num.ToString("D3");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tb二维码.Text == "") return;
            ZXing.BarcodeWriter bw = new BarcodeWriter();

            bw.Format = BarcodeFormat.QR_CODE;
            Bitmap b = bw.Write(tb二维码.Text);
            b.Save("code.bmp");
        }
    }
}
