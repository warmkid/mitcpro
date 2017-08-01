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
    public partial class 取样记录 : Form
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;

        public 取样记录()
        {
            InitializeComponent();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb名称.Enabled = true;
            btn查询插入.Enabled = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }


        private void btn查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb名称.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, tb名称.Text);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb名称.Text);
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
            tb名称.Enabled = false;
            btn查询插入.Enabled = false;


        }

      

        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 取样记录 where 名称='" + name + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("取样记录");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr, String name)
        {
            dr["名称"] = name;

            return dr;
        }

        private void outerBind()
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

        private void removeOuterBinding()
        {

        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 取样记录详细信息 where 取样记录ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("取样记录详细信息");
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
            dr["取样记录ID"] = dtOuter.Rows[0]["ID"];
            dr["取样量"] = 0;
            dr["日期"] = DateTime.Now;
            dr["取样人"] = mySystem.Parameter.userName;
            return dr;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
        }

        private void btn删除_Click(object sender, EventArgs e)
        {
            int idx = dataGridView1.SelectedCells[0].RowIndex;
            dtInner.Rows[idx].Delete();
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(tb名称.Text);
            removeOuterBinding();
            outerBind();
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
