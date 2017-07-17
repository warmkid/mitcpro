using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;

namespace 订单和库存管理
{


    public partial class 添加订单 : Form
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
        Hashtable goodsAndPrice;

        public 添加订单()
        {
            InitializeComponent();
            conn = new OleDbConnection(strConnect);
            conn.Open();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb订单名称.Enabled = true;
            btn查询插入.Enabled = true;

            OleDbDataAdapter daT = new OleDbDataAdapter("select * from 商品信息", conn);
            DataTable dtT = new DataTable("temp");
            daT.Fill(dtT);
            goodsAndPrice = new Hashtable();
            foreach(DataRow dr in dtT.Rows){
                goodsAndPrice.Add(Convert.ToString(dr["商品名称"]), Convert.ToDouble(dr["商品单价"]));
            }

            cmb产品类型.Items.Add("膜");
            cmb产品名称.Items.Add("医用膜PEF");

            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                //dtInner.Rows[e.RowIndex]["商品单价"] = goodsAndPrice[dtInner.Rows[e.RowIndex]["商品名称"]];
                dataGridView1.Rows[e.RowIndex].Cells["商品单价"].Value = goodsAndPrice[dtInner.Rows[e.RowIndex]["商品名称"]];
                //dataGridView1.Update();
            }
        }


       

       

        private void btn查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb订单名称.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb订单名称.Text);
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
            tb订单名称.Enabled = false;
            btn查询插入.Enabled = false;
        }

        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 订单信息 where 订单名称='" + name+"'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("订单信息");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["订单名称"] = tb订单名称.Text;
            dr["交付时间"] = DateTime.Now;
            return dr;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            tb订单名称.DataBindings.Add("Text", bsOuter.DataSource, "订单名称");
            tb客户名称.DataBindings.Add("Text", bsOuter.DataSource, "客户名称");
            cmb产品类型.DataBindings.Add("SelectedItem", bsOuter.DataSource, "产品类型");
            cmb产品名称.DataBindings.Add("SelectedItem", bsOuter.DataSource, "产品名称");
            dtp交付时间.DataBindings.Add("Value", bsOuter.DataSource, "交付时间");
            tb费用.DataBindings.Add("Text", bsOuter.DataSource, "费用");
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
        }

        private void removeOuterBinding()
        {
            tb订单名称.DataBindings.RemoveAt(0);
            tb客户名称.DataBindings.RemoveAt(0);
            cmb产品类型.DataBindings.RemoveAt(0);
            cmb产品名称.DataBindings.RemoveAt(0);
            dtp交付时间.DataBindings.Clear();
            tb费用.DataBindings.Clear();
            tb备注.DataBindings.Clear();
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 订单BOM信息 where 订单信息ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("订单BOM信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtInner.Columns)
            {

                switch (dc.ColumnName)
                {

                    case "ID":
                    case "订单信息ID":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.Visible = false;
                        break;
                    case "商品名称":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        HashSet<String> items = new HashSet<string>();
                        foreach (String s in goodsAndPrice.Keys.OfType<String>().ToList<String>())
                        {
                            items.Add(s);
                        }
                        foreach (DataRow dr in dtInner.Rows)
                        {
                            items.Add(Convert.ToString(dr["商品名称"]));
                        }
                        foreach (String s in items)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    case "商品数量":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "商品单价":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.ReadOnly = true;
                        dataGridView1.Columns.Add(tbc);
                        break;
                }
            }
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
            calcSum();
        }

        private DataRow writeInnerDefault(DataRow dr)
        {
            dr["订单信息ID"]=dtOuter.Rows[0]["ID"];
            return dr;
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
            readOuterData(tb订单名称.Text);
            removeOuterBinding();
            outerBind();

            // TODO：检查库存，然后决定是提醒建立生产指令还是做采购单
            OleDbDataAdapter daT = new OleDbDataAdapter("select * from 库存信息", conn);
            DataTable dtT = new DataTable("temp");
            daT.Fill(dtT);
            daT = new OleDbDataAdapter("select * from 采购详细信息 where 0=1", conn);
            DataTable dt采购详细信息 = new DataTable("采购详细信息");
            daT.Fill(dt采购详细信息);
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                String name = dgvr.Cells["商品名称"].Value.ToString();
                int n = Convert.ToInt32(dgvr.Cells["商品数量"].Value);
                DataRow[] foundRows = dtT.Select(String.Format("商品名称='{0}'", name));
                if (Convert.ToInt32(foundRows[0]["商品数量"]) < n)
                {
                    DataRow dr = dt采购详细信息.NewRow();
                    dr["商品名称"] = name;
                    dr["采购数量"] = n - Convert.ToInt32(foundRows[0]["商品数量"]);
                    dr["商品单价"] = dgvr.Cells["商品单价"].Value;
                    dt采购详细信息.Rows.Add(dr);
                }
            }
            if (dt采购详细信息.Rows.Count <= 0)
            {
                MessageBox.Show("库存足够完成该订单，请准备制定生产计划");
            }
            else
            {
                添加采购单 form = new 添加采购单(dt采购详细信息);
                form.Show();
            }
        }

        private void calcSum()
        {
            Double sum = 0;
            foreach (DataRow tdr in dtInner.Rows)
            {
                try
                {
                    Double price = Convert.ToDouble(tdr["商品单价"]);
                    Double num = Convert.ToDouble(tdr["商品数量"]);
                    sum += price * num;
                }
                catch
                {
                    sum += 0;
                }
            }
            dtOuter.Rows[0]["费用"] = sum;
        }
    }
}
