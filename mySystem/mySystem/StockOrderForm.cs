//文件名：SaleAddForm.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace mySystem
{
    public partial class StockOrderForm : Form
    {
        public StockOrderForm()
        {
            InitializeComponent();
        }
        public string MyCompany;
        private System.Data.DataTable MyTable;
        private int MyID;

        private void SaleAddForm_Load(object sender, EventArgs e)
        {
            //SetBuyer();
            //SetMerchandise();
            ////创建无连接的数据表
            //DataColumn[] MyKey = new DataColumn[1];
            //MyTable = new DataTable("销售明细表");
            //DataColumn MyColumn = new DataColumn();
            //MyColumn.DataType = System.Type.GetType("System.Int32");
            //MyColumn.ColumnName = "序号";
            //MyTable.Columns.Add(MyColumn);
            //MyKey[0] = MyColumn;
            //MyTable.PrimaryKey = MyKey;
            //MyTable.Columns.Add("商品编号", typeof(string));
            //MyTable.Columns.Add("商品名称", typeof(string));
            //MyTable.Columns.Add("规格型号", typeof(string));
            //MyTable.Columns.Add("单位", typeof(string));
            //MyTable.Columns.Add("建议销售价", typeof(float));
            //MyTable.Columns.Add("实际销售价", typeof(float));
            //MyTable.Columns.Add("数量", typeof(float));
            //MyTable.Columns.Add("金额", typeof(float));
            //this.销售明细DataGridView.DataSource = MyTable;
        }
        private void SetBuyer()
        {
            //this.客户名称ComboBox.Items.Clear();
            ////设置采购商客户名称
            //SqlConnection MyConnection = new SqlConnection();
            //MyConnection.ConnectionString = global::mySystem.Properties.Settings.Default.MySaleConnectionString;
            //MyConnection.Open();
            //SqlCommand MyCommand = new SqlCommand("Select DISTINCT 客户名称 From 采购商信息  ", MyConnection);
            //SqlDataReader MyReader = MyCommand.ExecuteReader();
            //while (MyReader.Read())
            //{
            //    this.客户名称ComboBox.Items.Add(MyReader.GetString(0));
            //}
            //if (MyConnection.State == ConnectionState.Open)
            //{
            //    MyConnection.Close();
            //}
        }
        private void SetMerchandise()
        {
            ////设置商品信息
            //SqlConnection MyConnection = new SqlConnection();
            //MyConnection.ConnectionString = global::mySystem.Properties.Settings.Default.MySaleConnectionString;
            //MyConnection.Open();
            //string MySQL = "SELECT 商品编号, 商品名称, 规格型号, 单位, 当前库存量,建议销售价,说明,生产厂商, 累计销售量, 累计采购量, 建议采购价 FROM dbo.商品信息";
            //DataTable MyMerchandiseTable = new DataTable();
            //SqlDataAdapter MyAdapter = new SqlDataAdapter(MySQL, MyConnection);
            //MyAdapter.Fill(MyMerchandiseTable);
            //this.商品明细DataGridView.DataSource = MyMerchandiseTable;
            //if (MyConnection.State == ConnectionState.Open)
            //{
            //    MyConnection.Close();
            //}
        }

        private void 新增采购商Button_Click(object sender, EventArgs e)
        {
            //BuyerForm MyDlg = new BuyerForm();
            //MyDlg.ShowDialog();
            //SetBuyer();
        }
        private void 新增商品种类Button_Click(object sender, EventArgs e)
        {
            //MerchandiseForm MyDlg = new MerchandiseForm();
            //MyDlg.ShowDialog();
            //SetMerchandise();            
        }

        private void 新增出库单Button_Click(object sender, EventArgs e)
        {
            ////自动计算销售单自编号
            //String MySQLConnectionString = global::mySystem.Properties.Settings.Default.MySaleConnectionString;
            //SqlConnection MyConnection = new SqlConnection(MySQLConnectionString);
            //MyConnection.Open();
            //SqlCommand MyCommand = MyConnection.CreateCommand();
            //MyCommand.CommandText = "Select max(自编号) 最大编号 From 销售信息";
            //object MyResult = MyCommand.ExecuteScalar();
            //Int64 MyID = 1;
            //if (MyResult != System.DBNull.Value)
            //{
            //    String MyMaxID = MyResult.ToString().Trim();
            //    MyMaxID = MyMaxID.Substring(2, MyMaxID.Length - 2);
            //    MyID = Convert.ToInt64(MyMaxID) + 1;
            //}
            //int MyLength = MyID.ToString().Length;
            //string MyNewID = "";
            //switch (MyLength)
            //{
            //    case 1:
            //        MyNewID = "XS0000000" + MyID.ToString();
            //        break;
            //    case 2:
            //        MyNewID = "XS000000" + MyID.ToString();
            //        break;
            //    case 3:
            //        MyNewID = "XS00000" + MyID.ToString();
            //        break;
            //    case 4:
            //        MyNewID = "XS0000" + MyID.ToString();
            //        break;
            //    case 5:
            //        MyNewID = "XS000" + MyID.ToString();
            //        break;
            //    case 6:
            //        MyNewID = "XS00" + MyID.ToString();
            //        break;
            //    case 7:
            //        MyNewID = "XS0" + MyID.ToString();
            //        break;
            //}
            //if (MyConnection.State == ConnectionState.Open)
            //{
            //    MyConnection.Close();
            //}
            //this.自编号TextBox.Text = MyNewID;
            //this.销售单号TextBox.Text = "";
            //this.客户名称ComboBox.Text = "";
            //this.应收金额TextBox.Text = "0";
            //this.实收金额TextBox.Text = "0";
            //this.收款方式ComboBox.Text = "";
            //this.经办人TextBox.Text = "";
            //this.说明TextBox.Text = "";
            //this.MyID = 0;     
        }

        private void 商品明细DataGridView_Click(object sender, EventArgs e)
        {
            //if (this.自编号TextBox.Text.Length < 1)
            //    return;
            //this.商品编号TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[0].Value.ToString();
            //this.商品名称TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[1].Value.ToString();
            //this.规格型号TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[2].Value.ToString();
            //this.单位TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[3].Value.ToString();
            //this.建议销售价TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[5].Value.ToString();
            //this.实际销售价TextBox.Text = this.商品明细DataGridView.CurrentRow.Cells[5].Value.ToString();
            //this.金额TextBox.Text = "";   
        }

        private void 实际销售价TextBox_TextChanged(object sender, EventArgs e)
        {
            //if (this.数量TextBox.Text.Length < 1)
            //    return;
            //if (this.实际销售价TextBox.Text.Length < 1)
            //    return;
            //double My实际销售价 = Convert.ToDouble(this.实际销售价TextBox.Text);
            //double My数量 = Convert.ToDouble(this.数量TextBox.Text);
            //double My金额 = My实际销售价 * My数量;
            //this.金额TextBox.Text = My金额.ToString();
        }

        private void 数量TextBox_TextChanged(object sender, EventArgs e)
        {
            //if (this.数量TextBox.Text.Length < 1)
            //    return;
            //if (this.实际销售价TextBox.Text.Length < 1)
            //    return;
            //double My实际销售价 = Convert.ToDouble(this.实际销售价TextBox.Text);
            //double My数量 = Convert.ToDouble(this.数量TextBox.Text);
            //double My金额 = My实际销售价 * My数量;
            //this.金额TextBox.Text = My金额.ToString();
        }

        private void 添加商品Button_Click(object sender, EventArgs e)
        {
            //if (this.金额TextBox.Text.Length < 1)
            //{
            //    return;
            //}
            //MyID = MyID + 1;
            //DataRow MyRow = MyTable.NewRow();
            //MyRow[0] = MyID;
            //MyRow["商品编号"] = this.商品编号TextBox.Text;
            //MyRow["商品名称"] = this.商品名称TextBox.Text;
            //MyRow["规格型号"] = this.规格型号TextBox.Text;
            //MyRow["单位"] = this.单位TextBox.Text;
            //MyRow["建议销售价"] = this.建议销售价TextBox.Text;
            //MyRow["实际销售价"] = this.实际销售价TextBox.Text;
            //MyRow["数量"] = this.数量TextBox.Text;
            //MyRow["金额"] = this.金额TextBox.Text;
            //MyTable.Rows.Add(MyRow);
            //Double My应收金额 = Convert.ToDouble(this.应收金额TextBox.Text);
            //My应收金额 += Convert.ToDouble(this.金额TextBox.Text);
            //this.应收金额TextBox.Text = My应收金额.ToString();
            //this.金额TextBox.Text = "";
        }

        private void 减少商品Button_Click(object sender, EventArgs e)
        {
            //Double My应收金额 = Convert.ToDouble(this.应收金额TextBox.Text);
            //My应收金额 -= Convert.ToDouble(this.销售明细DataGridView.CurrentRow.Cells[8].Value.ToString());
            //this.应收金额TextBox.Text = My应收金额.ToString();
            //int MyIndex = this.销售明细DataGridView.CurrentRow.Index;
            //MyTable.Rows.RemoveAt(MyIndex);
        }

        private void 打印出库单Button_Click(object sender, EventArgs e)
        {
            //this.printPreviewDialog1.Document = this.printDocument1;
            //this.printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            ////打印销售明细单
            //e.Graphics.DrawString(this.MyCompany + "商品销售出库单", new Font("宋体", 20), Brushes.Black, 140, 80);
            //e.Graphics.DrawString("销售单号：" + this.销售单号TextBox.Text.ToUpper(), new Font("宋体", 12), Brushes.Black, 100, 150);
            //e.Graphics.DrawString("记帐本位币：人民币(元)", new Font("宋体", 12), Brushes.Black, 530, 150);
            //e.Graphics.DrawLine(new Pen(Color.Black, (float)3.00), 100, 170, 720, 170);
            //e.Graphics.DrawString("客户名称：" + this.客户名称ComboBox.Text, new Font("宋体", 12), Brushes.Black, 110, 175);
            //e.Graphics.DrawString("应收金额：" + this.应收金额TextBox.Text, new Font("宋体", 12), Brushes.Black, 400, 175);
            //e.Graphics.DrawString("实收金额：" + this.实收金额TextBox.Text, new Font("宋体", 12), Brushes.Black, 570, 175);
            //e.Graphics.DrawLine(new Pen(Color.Black), 100, 195, 720, 195);
            //e.Graphics.DrawString("收款方式：" + this.收款方式ComboBox.Text, new Font("宋体", 12), Brushes.Black, 110, 200);
            //e.Graphics.DrawString("经办人：" + this.经办人TextBox.Text, new Font("宋体", 12), Brushes.Black, 400, 200);
            //e.Graphics.DrawString("出库日期：" + this.出库日期DateTimePicker.Value.ToShortDateString(), new Font("宋体", 12), Brushes.Black, 530, 200);
            //e.Graphics.DrawLine(new Pen(Color.Black), 100, 220, 720, 220);
            //e.Graphics.DrawString("说明：" + this.说明TextBox.Text, new Font("宋体", 12), Brushes.Black, 110, 225);
            //e.Graphics.DrawLine(new Pen(Color.Black, (float)3.00), 100, 245, 720, 245);
            //e.Graphics.DrawString("商品名称", new Font("宋体", 12), Brushes.Black, 160, 250);
            //e.Graphics.DrawString("规格型号", new Font("宋体", 12), Brushes.Black, 350, 250);
            //e.Graphics.DrawString("单位", new Font("宋体", 12), Brushes.Black, 450, 250);
            //e.Graphics.DrawString("单价", new Font("宋体", 12), Brushes.Black, 510, 250);
            //e.Graphics.DrawString("数量", new Font("宋体", 12), Brushes.Black, 570, 250);
            //e.Graphics.DrawString("金额", new Font("宋体", 12), Brushes.Black, 650, 250);
            //int MyPosY = 270;
            //float MyAmount = 0;
            //for (int i = 0; i < MyTable.Rows.Count; i++)
            //{
            //    e.Graphics.DrawLine(new Pen(Color.Black), 100, MyPosY, 720, MyPosY);
            //    e.Graphics.DrawString(MyTable.Rows[i][2].ToString(), new Font("宋体", 12), Brushes.Black, 110, MyPosY + 5);
            //    e.Graphics.DrawString(MyTable.Rows[i][3].ToString(), new Font("宋体", 12), Brushes.Black, 330, MyPosY + 5);
            //    e.Graphics.DrawString(MyTable.Rows[i][4].ToString(), new Font("宋体", 12), Brushes.Black, 450, MyPosY + 5);
            //    e.Graphics.DrawString(MyTable.Rows[i][6].ToString(), new Font("宋体", 12), Brushes.Black, 500, MyPosY + 5);
            //    e.Graphics.DrawString(MyTable.Rows[i][7].ToString(), new Font("宋体", 12), Brushes.Black, 570, MyPosY + 5);
            //    e.Graphics.DrawString(MyTable.Rows[i][8].ToString(), new Font("宋体", 12), Brushes.Black, 630, MyPosY + 5);
            //    MyAmount = MyAmount + (float)MyTable.Rows[i][5];
            //    MyPosY = MyPosY + 25;
            //}
            //e.Graphics.DrawLine(new Pen(Color.Black, (float)3.00), 100, MyPosY, 720, MyPosY);
            //e.Graphics.DrawString("打印日期：" + DateTime.Now.ToShortDateString(), new Font("宋体", 12), Brushes.Black, 530, MyPosY + 5);   
        }

        private void 保存出库单Button_Click(object sender, EventArgs e)
        {
            //if (this.应收金额TextBox.Text.Length <2)
            //    return;
            //if (MessageBox.Show("请检查商品销售信息是否正确，一旦保存就无法修改，是否继续？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}
            //String MySQLConnectionString = global::mySystem.Properties.Settings.Default.MySaleConnectionString;
            //string MySQL = "INSERT INTO 销售信息(自编号,销售单号,客户名称,应收金额,实收金额,收款方式,经办人,出库日期,说明)VALUES('";
            //MySQL += this.自编号TextBox.Text + "','";
            //MySQL += this.销售单号TextBox.Text + "','";
            //MySQL += this.客户名称ComboBox.Text + "',";
            //MySQL += this.应收金额TextBox.Text + ",";
            //MySQL += this.实收金额TextBox.Text + ",'";
            //MySQL += this.收款方式ComboBox.Text + "','";
            //MySQL += this.经办人TextBox.Text + "','";
            //MySQL += this.出库日期DateTimePicker.Value.ToString() + "','";
            //MySQL += this.说明TextBox.Text + "');";
            //SqlConnection MyConnection = new SqlConnection(MySQLConnectionString);
            //MyConnection.Open();
            //SqlCommand MyCommand = MyConnection.CreateCommand();
            //MyCommand.CommandText = MySQL;
            //MyCommand.ExecuteNonQuery();
            //foreach (DataRow MyRow in MyTable.Rows)
            //{
            //    MySQL = "INSERT INTO 销售明细(自编号,销售单号,商品编号,数量,单价,金额) VALUES('";
            //    MySQL += this.GetNewID() + "','";
            //    MySQL += this.销售单号TextBox.Text + "','";
            //    MySQL += MyRow[1].ToString() + "',";
            //    MySQL += MyRow[7].ToString() + ",";
            //    MySQL += MyRow[6].ToString() + ",";
            //    MySQL += MyRow[8].ToString() + ");";
            //    MySQL += "Update 商品信息 SET 累计销售量=累计销售量+" + MyRow[7].ToString() + " WHERE 商品编号='" + MyRow[1].ToString() + "';";
            //    MyCommand.CommandText = MySQL;
            //    MyCommand.ExecuteNonQuery();
            //}
            //if (MyConnection.State == ConnectionState.Open)
            //{
            //    MyConnection.Close();
            //}
            //MyTable.Rows.Clear();
            //this.应收金额TextBox.Text = "";
            //SetMerchandise();
            //新增出库单Button_Click(null, null);
        }

        //private string GetNewID()
        //{
            ////自动计算销售明细自编号
            //String MySQLConnectionString = global::mySystem.Properties.Settings.Default.MySaleConnectionString;
            //SqlConnection MyConnection = new SqlConnection(MySQLConnectionString);
            //MyConnection.Open();
            //SqlCommand MyCommand = MyConnection.CreateCommand();
            //MyCommand.CommandText = "Select max(自编号) 最大编号 From 销售明细";
            //object MyResult = MyCommand.ExecuteScalar();
            //Int64 MyID = 1;
            //if (MyResult != System.DBNull.Value)
            //{
            //    String MyMaxID = MyResult.ToString().Trim();
            //    MyMaxID = MyMaxID.Substring(2, MyMaxID.Length - 2);
            //    MyID = Convert.ToInt64(MyMaxID) + 1;
            //}
            //int MyLength = MyID.ToString().Length;
            //string MyNewID = "";
            //switch (MyLength)
            //{
            //    case 1:
            //        MyNewID = "MX0000000" + MyID.ToString();
            //        break;
            //    case 2:
            //        MyNewID = "MX000000" + MyID.ToString();
            //        break;
            //    case 3:
            //        MyNewID = "MX00000" + MyID.ToString();
            //        break;
            //    case 4:
            //        MyNewID = "MX0000" + MyID.ToString();
            //        break;
            //    case 5:
            //        MyNewID = "MX000" + MyID.ToString();
            //        break;
            //    case 6:
            //        MyNewID = "MX00" + MyID.ToString();
            //        break;
            //    case 7:
            //        MyNewID = "MX0" + MyID.ToString();
            //        break;
            //}
            //if (MyConnection.State == ConnectionState.Open)
            //{
            //    MyConnection.Close();
            //}
            //return MyNewID;
        //}
    }
}