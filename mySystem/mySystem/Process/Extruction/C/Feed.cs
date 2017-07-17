using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace mySystem.Process.Extruction.C
{
    public partial class Feed : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜供料系统运行记录";
        DataTable dtFeed;
        OleDbDataAdapter daFeed;
        BindingSource bsFeed;
        OleDbCommandBuilder cbFeed;

        string tablename2 = "吹膜供料系统运行记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        private CheckForm check = null;
        int outerId;
        int status;
        public Feed(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            txb生产指令编号.Text = Parameter.proInstruction;
            dtp生产日期.Value = Convert.ToDateTime(DateTime.Now.Date.ToString());
            for (int i = 0; i < flight.Count; i++)
            {
                cmb班次.Items.Add(flight[i]);
            }
            cmb班次.SelectedItem = Parameter.userflight;
            readFeedData(txb生产指令编号.Text, dtp生产日期.Value, cmb班次.SelectedItem.ToString());
            removeFeedBinding();
            feedBinding();
            if (0 == dtFeed.Rows.Count)
            {
                DataRow newrow = dtFeed.NewRow();
                newrow = writeFeedDefault(newrow);
                dtFeed.Rows.Add(newrow);
                daFeed.Update((DataTable)bsFeed.DataSource);
                readFeedData(txb生产指令编号.Text, dtp生产日期.Value, cmb班次.SelectedItem.ToString());
                removeFeedBinding();
                feedBinding();
            }
            btn审核.Enabled = false;
            txb生产指令编号.Enabled = false;
            dtp生产日期.Enabled = false;
            cmb班次.Enabled = false;
            readItemData(Convert.ToInt32( dtFeed.Rows[0]["ID"]));
            ItemBind();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Width = 220;
        }

        public Feed(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            dtFeed = new DataTable(tablename1);
            daFeed = new OleDbDataAdapter("SELECT * FROM 吹膜供料系统运行记录 WHERE ID =" + Id, conOle);
            bsFeed = new BindingSource();
            cbFeed = new OleDbCommandBuilder(daFeed);
            daFeed.Fill(dtFeed);
            removeFeedBinding();
            feedBinding();
            cmb班次.Text =Convert.ToString( dtFeed.Rows[0]["班次"]);
            readItemData(Convert.ToInt32(dtFeed.Rows[0]["ID"]));
            
            ItemBind();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            this.btnSave.Visible = false;
           
            this.btn审核.Visible = false;
            this.btn添加.Visible = false;
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
        }

        private DataRow writeFeedDefault(DataRow dr)
        {
            dr["生产指令编号"] = Parameter.proInstruction;
            dr["生产指令ID"] = Parameter.proInstruID;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dr["班次"] = Parameter.userflight;
            dr["审核人"] = "";
            return dr;
        }
        private DataRow writeItemDefault(DataRow dr)
        {
            dr["T吹膜供料系统运行记录ID"] = dtFeed.Rows[0]["ID"];
            dr["检查时间"] = Convert.ToDateTime(DateTime.Now.TimeOfDay.ToString());
            dr["电机工作是否正常"] = "正常";
            dr["气动阀工作是否正常"] = "正常";
            dr["供料运行是否正常"] = "正常";
            dr["有无警报显示"] = "无";
            dr["是否解除警报"] = "无";
            dr["检查人"]= Parameter.userName;
            return dr;
        }

        void readFeedData(string 生产指令编号, DateTime 生产日期, string 班次)
        {
            daFeed = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令编号='" + 生产指令编号 + "' AND 生产日期=#" + 生产日期 + "# AND 班次='" + 班次 + "';", conOle);
            cbFeed = new OleDbCommandBuilder(daFeed);
            dtFeed = new DataTable(tablename1);
            bsFeed = new BindingSource();
            daFeed.Fill(dtFeed);
        }

        private void readItemData(int id)
        {
            daItem = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T吹膜供料系统运行记录ID=" + id, conOle);
            cbItem = new OleDbCommandBuilder(daItem);
            dtItem = new DataTable(tablename2);
            bsItem = new BindingSource();
            daItem.Fill(dtItem);
        }

        private void removeFeedBinding()
        {
            txb生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            cmb班次.DataBindings.Clear();        
        }
        private void feedBinding()
        {
            bsFeed.DataSource = dtFeed;
            txb生产指令编号.DataBindings.Add("Text", bsFeed.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsFeed.DataSource, "生产日期");
            cmb班次.DataBindings.Add("SelectedItem", bsFeed.DataSource, "班次");
        }
        private void ItemBind()
        {
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            
            foreach (DataColumn dc in dtItem.Columns)
            {

                switch (dc.ColumnName)
                {
                    case "检查时间":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in flight)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    
                    //case "不良品数量":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    
                    //case "记录人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    //case "审核人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break; 

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);

                        break;
                }
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            //dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
        }

        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "';";
            return (int)comm.ExecuteScalar();           
        }
        
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();

                dtFeed.Rows[0]["审核人"] = check.userName;
                dtFeed.Rows[0]["审核意见"] = check.opinion.ToString();
                dtFeed.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                daFeed.Update((DataTable)bsFeed.DataSource);
                readFeedData(txb生产指令编号.Text, dtp生产日期.Value, cmb班次.SelectedItem.ToString());
                removeFeedBinding();
                feedBinding();
                this.dataGridView1.Enabled = false;
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {           
            bsFeed.EndEdit();
            daFeed.Update((DataTable)bsFeed.DataSource);
            readFeedData(txb生产指令编号.Text, dtp生产日期.Value, cmb班次.SelectedItem.ToString());
            removeFeedBinding();
            feedBinding();

            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtFeed.Rows[0]["ID"]));
            ItemBind();
            btnSave.Enabled = false;
            btn审核.Enabled = true;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtItem.NewRow();
            dr = writeItemDefault(dr);
            dtItem.Rows.Add(dr);
            //setDataGridViewRowNums();
            btnSave.Enabled = true;
            btn审核.Enabled = false;
        }
        private void btn打印_Click(object sender, EventArgs e)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\C\SOP-MFG-301-R07 吹膜供料系统运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 6].Value = "生产指令：" + txb生产指令编号.Text;
            my.Cells[5, 1].Value = dtp生产日期.Value.ToLongDateString() + "   " + cmb班次.Text;
            my.Cells[5, 9].Value = "";
            



            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                my.Cells[5 + i, 2].Value = Convert.ToDateTime(dtItem.Rows[i]["检查时间"]).ToShortTimeString();
                my.Cells[5 + i, 3].Value = dtItem.Rows[i]["电机工作是否正常"];
                my.Cells[5 + i, 4].Value = dtItem.Rows[i]["气动阀工作是否正常"];
                my.Cells[5 + i, 5].Value = dtItem.Rows[i]["供料运行是否正常"];
                my.Cells[5 + i, 6].Value = dtItem.Rows[i]["有无警报显示"];
                my.Cells[5 + i, 7].Value = dtItem.Rows[i]["是否解除警报"];
                my.Cells[5 + i, 8].Value = dtItem.Rows[i]["检查人"];
            }
            my.Cells[5, 9].Value = dtFeed.Rows[0]["审核人"];
            // 让这个Sheet为被选中状态
            my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            // 直接用默认打印机打印该Sheet
            // my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
        }
    }
}
