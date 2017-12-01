using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Process.Stock
{
    // TODO 打印
    public partial class 取样证 : BaseForm
    {
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter daOuter;
        DataTable dtOuter;
        SqlCommandBuilder cbOuter;
        BindingSource bsOuter;
        public 取样证(int id)
        {
            InitializeComponent();
            
            //conn = new OleDbConnection(strConnect);
            //conn.Open();


            readOuterData(id);
            outerBind();

            if (dtOuter.Rows.Count == 0)
            {

                SqlDataAdapter daT = new SqlDataAdapter("select * from 取样记录详细信息 where ID=" + id, mySystem.Parameter.conn);
                DataTable dtT = new DataTable("取样记录详细信息");

                daT.Fill(dtT);
                DataRow dr = dtT.Rows[0];

                DataRow ndr = dtOuter.NewRow();
                ndr["取样记录详细信息ID"] = id;
                ndr["物料名称"] = dr["物料名称"].ToString();
                ndr["物料代码"] = dr["物料代码"].ToString();
                ndr["数量"] = Convert.ToInt32(dr["数量"]);
                ndr["取样人"] = dr["取样人"].ToString();
                try
                {
                    ndr["取样量"] = Convert.ToInt32(dr["取样量"]);
                }
                catch
                {
                    MessageBox.Show("取样量错误，请检查");
                    return;
                }
                ndr["本厂批号"] = dr["本厂批号"].ToString();
                ndr["单位"] = dr["单位"].ToString();
                ndr["备注"] = dr["备注"].ToString();
                ndr["取样日期"] = DateTime.Now;
                dtOuter.Rows.Add(ndr);

                daOuter.Update((DataTable)bsOuter.DataSource);

                readOuterData(id);
                outerBind();
            }

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                combobox打印机选择.Items.Add(sPrint);
            }

        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            // TODO 完成打印

            // TODO 记录
            SqlDataAdapter da = new SqlDataAdapter("select * from 取样证打印记录 where 0=1", mySystem.Parameter.conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable("取样证打印记录");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["物料代码"] = tb物料代码.Text;
            dr["本厂批号"] = tb本厂批号.Text;
            dr["取样人"] = tb取样人.Text;
            dr["取样时间"] = dtp取样日期.Value;
            dr["打印人"] = mySystem.Parameter.userName;
            dr["打印时间"] = DateTime.Now;
            dt.Rows.Add(dr);
            da.Update(dt);
        }


        void readOuterData(int id)
        {
            daOuter = new SqlDataAdapter("select * from 取样证 where 取样记录详细信息ID=" + id, mySystem.Parameter.conn);
            dtOuter = new DataTable("取样证");
            daOuter.Fill(dtOuter);
            cbOuter = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {

                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2), false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as ComboBox).DataBindings["Text"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as ComboBox).DataBindings["Text"].DataSourceUpdateMode;
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                    ControlUpdateMode cm = (c as DateTimePicker).DataBindings["Value"].ControlUpdateMode;
                    DataSourceUpdateMode dm = (c as DateTimePicker).DataBindings["Value"].DataSourceUpdateMode;
                }
            }
        }
    }
}
