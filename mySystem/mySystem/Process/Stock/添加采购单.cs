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

namespace 订单和库存管理
{
    public partial class 添加采购单 : Form
    {

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        DataTable dtOuter, dtInner;
        BindingSource bsOuter, bsInner;
   
        public 添加采购单()
        {
            InitializeComponent();
            dtp采购时间.Format = DateTimePickerFormat.Custom;
            dtp采购时间.CustomFormat = "yyyy/MM/dd HH:mm:ss";

            conn = new OleDbConnection(strConnect);
            conn.Open();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb采购单名称.Enabled = true;
            btn查询插入.Enabled = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        public 添加采购单(DataTable dt)
        {
            InitializeComponent();
            dtp采购时间.Format = DateTimePickerFormat.Custom;
            dtp采购时间.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            conn = new OleDbConnection(strConnect);
            conn.Open();
            String name = "自动生成采购单" + DateTime.Now.ToString("yyyyMMddHHmmss");
            readOuterData(name);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, name);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(name);
                removeOuterBinding();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            foreach (DataRow dr in dt.Rows)
            {
                DataRow ndr = dtInner.NewRow();
                ndr.ItemArray = dr.ItemArray.Clone() as object[];
                ndr["采购单信息ID"] = dtOuter.Rows[0]["ID"];
                dtInner.Rows.Add(ndr);
            }
            // setDataGridViewColumns();
            innerBind();
            calcSum();
            // 控件状态
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            tb采购单名称.Enabled = false;
            btn查询插入.Enabled = false;

        }

        private void btn查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb采购单名称.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr, tb采购单名称.Text);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb采购单名称.Text);
                removeOuterBinding();
                outerBind();
            }
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            // setDataGridViewColumns();
            innerBind();
            // 控件状态
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            tb采购单名称.Enabled = false;
            btn查询插入.Enabled = false;
        }

        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 采购单信息 where 采购单名称='" + name + "'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("采购单信息");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr, String name)
        {
            dr["采购单名称"] = name;
            dr["采购时间"] = DateTime.Now;
            return dr;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            tb采购单名称.DataBindings.Add("Text", bsOuter.DataSource, "采购单名称");
            tb采购人.DataBindings.Add("Text", bsOuter.DataSource, "采购人");
            dtp采购时间.DataBindings.Add("Value", bsOuter.DataSource, "采购时间");
            tb采购单总价.DataBindings.Add("Text", bsOuter.DataSource, "总价");
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
        }

        private void removeOuterBinding()
        {
            tb采购单名称.DataBindings.Clear();
            tb采购人.DataBindings.Clear();
            dtp采购时间.DataBindings.Clear();
            tb采购单总价.DataBindings.Clear();
            tb备注.DataBindings.Clear();
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 采购详细信息 where 采购单信息ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("采购详细信息");
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
            dr["采购单信息ID"] = dtOuter.Rows[0]["ID"];
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
            readOuterData(tb采购单名称.Text);
            removeOuterBinding();
            outerBind();
        }

        private void calcSum()
        {
            Double sum = 0;
            foreach (DataRow tdr in dtInner.Rows)
            {
                try
                {
                    Double price = Convert.ToDouble(tdr["商品单价"]);
                    Double num = Convert.ToDouble(tdr["采购数量"]);
                    sum += price * num;
                }
                catch
                {
                    sum += 0;
                }
            }
            dtOuter.Rows[0]["总价"] = sum;
        }

    }
}
