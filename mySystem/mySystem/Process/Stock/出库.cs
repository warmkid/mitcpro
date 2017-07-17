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
    public partial class 出库 : Form
    {

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
        List<String> goods;


        public 出库()
        {
            InitializeComponent();

            dtp出库时间.Format = DateTimePickerFormat.Custom;
            dtp出库时间.CustomFormat = "yyyy/MM/dd HH:mm:ss";

            conn = new OleDbConnection(strConnect);
            conn.Open();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb出库人.Enabled = true;
            dtp出库时间.Enabled = true;
            btn查询插入.Enabled = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            OleDbDataAdapter daT = new OleDbDataAdapter("select * from 商品信息", conn);
            DataTable dtT = new DataTable("temp");
            daT.Fill(dtT);
            goods = new List<string>();
            foreach (DataRow dr in dtT.Rows)
            {
                goods.Add(Convert.ToString(dr["商品名称"]));
            }

        }


        private void btn查询插入_Click(object sender, EventArgs e)
        {
            bool isNew = false;
            if (tb出库人.Text.Trim() == "")
            {
                MessageBox.Show("采购人不能为空");
                return;
            }
            tb出库单名称.Text = tb出库人.Text + dtp出库时间.Value.ToString("yyyyMMddHHmmss");
            readOuterData(tb出库单名称.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                isNew = true;
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb出库单名称.Text);
                removeOuterBinding();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();
            // 控件状态
            if (isNew)
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }
                tb出库单名称.Enabled = false;
                tb出库人.Enabled = false;
                dtp出库时间.Enabled = false;
                btn查询插入.Enabled = false;
            }
            else
            {
                // 历史记录不能改了
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
            }
        }


        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 出库信息 where 出库单名称='" + name + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("出库信息");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["出库单名称"] = tb出库单名称.Text;
            dr["出库人"] = tb出库人.Text;
            dr["出库时间"] = dtp出库时间.Value;
            return dr;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            tb出库单名称.DataBindings.Add("Text", bsOuter.DataSource, "出库单名称");
            tb出库人.DataBindings.Add("Text", bsOuter.DataSource, "出库人");
            dtp出库时间.DataBindings.Add("Value", bsOuter.DataSource, "出库时间");
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
        }

        private void removeOuterBinding()
        {
            tb出库单名称.DataBindings.Clear();
            tb出库人.DataBindings.Clear();
            dtp出库时间.DataBindings.Clear();
            tb备注.DataBindings.Clear();
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 出库详细信息 where 出库信息ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("出库详细信息");
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
            dr["出库信息ID"] = dtOuter.Rows[0]["ID"];
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

        private void btn出库_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("确认码？", "", MessageBoxButtons.OKCancel);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                // 把数量从库存中减去
                OleDbDataAdapter daK = new OleDbDataAdapter("select * from 库存信息", conn);
                DataTable dtK = new DataTable("库存信息");
                BindingSource bsK = new BindingSource();
                OleDbCommandBuilder cbK = new OleDbCommandBuilder(daK);
                daK.Fill(dtK);
                bsK.DataSource = dtK;
                foreach (DataRow dr in dtInner.Rows)
                {
                    string name = dr["商品名称"].ToString();
                    int n = Convert.ToInt32(dr["出库数量"]);
                    DataRow[] drs = dtK.Select(String.Format("商品名称='{0}'", name));
                    if (Convert.ToInt32(drs[0]["商品数量"]) < n)
                    {
                        MessageBox.Show(String.Format("{0} 库存不足，出库失败", name));
                        return;
                    }
                    drs[0]["商品数量"] = Convert.ToInt32(drs[0]["商品数量"]) - n;
                }
                daK.Update((DataTable)bsK.DataSource);

                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();


                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb出库单名称.Text);
                removeOuterBinding();
                outerBind();

                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }

            }
           
            
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
                    case "出库信息ID":
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
                        foreach (String s in goods)
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
                    case "出库数量":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        break;
                   
                }
            }
        }

        

       

    }
}
