using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Stock
{
    public partial class 采购需求单 : Form
    {
       

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
   
        public 采购需求单()
        {
            InitializeComponent();
            tb编号.Text = "PALL-XXX-" + DateTime.Now.ToString("yyyy-MM");
            conn = new OleDbConnection(strConnect);
            conn.Open();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb编号.Enabled = true;
            btn查询插入.Enabled = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

       

        private void btn查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb编号.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, tb编号.Text);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb编号.Text);
                removeOuterBinding();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();
            // 控件状态
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            tb编号.Enabled = false;
            btn查询插入.Enabled = false;

            addComputerEventHandler();
        }

        void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7||e.ColumnIndex==8)
            {
                calcSum();
            }
        }


        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 采购需求单 where 编号='" + name + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("采购需求单");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr, String name)
        {
            dr["采购年月"] = DateTime.Now.ToString("yyyy-MM");
            dr["编号"] = name;
            dr["合计数量"] = 0;
            dr["合计理论生产数量"] = 0;
            dr["申请人"] = mySystem.Parameter.userName;
            dr["申请日期"] = DateTime.Now;
            dr["批准日期"] = DateTime.Now;
            return dr;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {
                if (c.Name == "cmb负责人") continue;
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

        private void removeOuterBinding()
        {
            
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 采购需求单详细信息 where 采购需求单ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("采购需求单详细信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        private DataRow writeInnerDefault(DataRow dr)
        {
            dr["采购需求单ID"] = dtOuter.Rows[0]["ID"];
            dr["数量"] = 0;
            dr["期望到货时间"] = DateTime.Now;
            dr["预计使用时间"] = DateTime.Now;
            dr["是否紧急"] = "是";
            dr["是否有库存"] = "是";
            return dr;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
            calcSum();
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            int idx = dataGridView1.SelectedCells[0].RowIndex;
            dtInner.Rows[idx].Delete();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            calcSum();
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(tb编号.Text);
            removeOuterBinding();
            outerBind();
        }

        private void calcSum()
        {
            Double sum1 = 0, sum2 = 0;
            foreach (DataRow tdr in dtInner.Rows)
            {
                try
                {
                    Double quantity1 = Convert.ToDouble(tdr["数量"]);
                    Double quantity2 = Convert.ToDouble(tdr["理论生产数量"]);
                    sum1 += quantity1;
                    sum2 += quantity2;
                }
                catch
                {
                    sum1 += 0;
                    sum2 += 0;
                }
            }
            dtOuter.Rows[0]["合计数量"] = sum1;
            lbl合计数量.DataBindings[0].ReadValue();
            dtOuter.Rows[0]["合计理论生产数量"] = sum2;
            lbl合计理论生产数量.DataBindings[0].ReadValue();
        }

        void setDataGridViewColumns()
        {
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dtInner.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "是否紧急")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("是");
                    cbc.Items.Add("否");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                if (dc.ColumnName == "是否有库存")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    cbc.Items.Add("是");
                    cbc.Items.Add("否");
                    dataGridView1.Columns.Add(cbc);
                    continue;
                }
                // 根据数据类型自动生成列的关键信息
                switch (dc.DataType.ToString())
                {

                    case "System.Int32":
                    case "System.String":
                    case "System.Double":
                    case "System.DateTime":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(ckbc);
                        break;
                }
            }
        }



       

       
    }
}

