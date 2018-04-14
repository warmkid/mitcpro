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
using ZXing;

namespace mySystem.Query
{
    public partial class 库存查询 : Form
    {
         SqlDataAdapter da退货台账,da出库单,da库存台账,da检验台账;
        DataTable dt退货台账,dt出库单,dt库存台账,dt检验台账;
        SqlCommandBuilder cb库存台账;

        public 库存查询()
        {
            InitializeComponent();

            dtp退货台账申请开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dgv退货台账.AllowUserToAddRows = false;
            dgv退货台账.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv退货台账_DataBindingComplete);
            dgv退货台账.ReadOnly = true;
            tb退货台账产品名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb退货台账产品名称_PreviewKeyDown);
            tb退货台账产品批号.PreviewKeyDown += new PreviewKeyDownEventHandler(tb退货台账产品批号_PreviewKeyDown);
            tb退货台账客户名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb退货台账客户名称_PreviewKeyDown);
            tb退货台账销售合同号.PreviewKeyDown += new PreviewKeyDownEventHandler(tb退货台账销售合同号_PreviewKeyDown);

            dtp出库单出库开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dgv出库单.AllowUserToAddRows = false;
            dgv出库单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv出库单_DataBindingComplete);
            dgv出库单.ReadOnly = true;
            tb出库单产品名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb出库单产品名称_PreviewKeyDown);
            tb出库单产品批号.PreviewKeyDown += new PreviewKeyDownEventHandler(tb出库单产品批号_PreviewKeyDown);
            tb出库单客户名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb出库单客户名称_PreviewKeyDown);
            tb出库单发货公司.PreviewKeyDown += new PreviewKeyDownEventHandler(tb出库单发货公司_PreviewKeyDown);

            dgv库存台账.AllowUserToAddRows = false;
            //dgv库存台账.ReadOnly = true;
            dgv库存台账.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv库存台账_DataBindingComplete);
            tb存货台账厂家名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb存货台账厂家名称_PreviewKeyDown);
            tb库存台账存货代码.PreviewKeyDown += new PreviewKeyDownEventHandler(tb库存台账存货代码_PreviewKeyDown);
            cmb库存台账状态.PreviewKeyDown += new PreviewKeyDownEventHandler(cmb库存台账状态_PreviewKeyDown);

            dgv检验台账.AllowUserToAddRows = false;
            dgv检验台账.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv检验台账_DataBindingComplete);
            tb检验台账厂家名称.PreviewKeyDown += new PreviewKeyDownEventHandler(tb检验台账厂家名称_PreviewKeyDown);
            tb检验台账存货代码.PreviewKeyDown += new PreviewKeyDownEventHandler(tb检验台账存货代码_PreviewKeyDown);

        }

        void tb检验台账存货代码_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询检验台账.PerformClick();
        }

        void tb检验台账厂家名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询检验台账.PerformClick();
        }

        void cmb库存台账状态_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询库存台账.PerformClick();
        }

        void tb库存台账存货代码_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询库存台账.PerformClick();
        }

        void tb存货台账厂家名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询库存台账.PerformClick();
        }

        void tb出库单客户名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询出库单.PerformClick();
        }

        void tb出库单产品批号_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询出库单.PerformClick();
        }

        void tb出库单产品名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询出库单.PerformClick();
        }

        void tb出库单发货公司_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询出库单.PerformClick();
        }

        void tb退货台账销售合同号_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn退货台账查询.PerformClick();
        }

        void tb退货台账客户名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn退货台账查询.PerformClick();
        }

        void tb退货台账产品批号_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn退货台账查询.PerformClick();
        }

        void tb退货台账产品名称_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn退货台账查询.PerformClick();
        }

        

       

        void dgv检验台账_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv检验台账.RowHeadersVisible = false;
            dgv检验台账.Columns["ID"].Visible = false;
            try
            { 
                //setDataGridViewBackColor("审核人", dgv检验台账); 
            }
            catch
            { }
            Utility.setDataGridViewAutoSizeMode(dgv检验台账, DataGridViewAutoSizeColumnMode.AllCells);
        }

        void dgv库存台账_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv库存台账.RowHeadersVisible = false;
            dgv库存台账.Columns["ID"].Visible = false;
            // readonly
            foreach (DataGridViewColumn dgvc in dgv库存台账.Columns)
            {
                if (dgvc.Name == "实盘数量"||dgvc.Name=="货位"||dgvc.Name=="有效期")
                {
                    dgvc.ReadOnly = false;
                }
                else
                {
                    dgvc.ReadOnly = true;
                }
            }
            // 设置颜色
            foreach (DataGridViewRow dgvr in dgv库存台账.Rows)
            {
                if (Convert.ToBoolean(dgvr.Cells["是否盘点"].Value))
                {
                }
                else
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            Utility.setDataGridViewAutoSizeMode(dgv库存台账, DataGridViewAutoSizeColumnMode.NotSet);
            dgv库存台账.Columns["产品代码"].Width = 200;
            dgv库存台账.Columns["产品名称"].Width = 200;
            dgv库存台账.Columns["产品规格"].Width = 200;
        }

        void dgv出库单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv出库单.RowHeadersVisible = false;
            dgv出库单.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv出库单, DataGridViewAutoSizeColumnMode.AllCells);
        }

        void dgv退货台账_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv退货台账.RowHeadersVisible = false;
            dgv退货台账.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv退货台账, DataGridViewAutoSizeColumnMode.AllCells);
        }



        private void btn退货台账查询_Click(object sender, EventArgs e)
        {
            string sql = "select * from 产品退货记录 where 申请日期 between '{0}' and '{1}' and 拟退货产品销售订单编号 like '%{2}%' and 客户名称 like '%{3}%' and 产品名称 like '%{4}%' and 产品批号 like '%{5}%'";
            DateTime start = dtp退货台账申请开始时间.Value;
            DateTime end = dtp退货台账申请结束时间.Value;
            string 订单号 = tb退货台账销售合同号.Text;
            string 客户 = tb退货台账客户名称.Text;
            string 产品名称 = tb退货台账产品名称.Text;
            string 产品批号 = tb退货台账产品批号.Text;
            da退货台账 = new SqlDataAdapter(string.Format(sql, start, end, 订单号, 客户, 产品名称, 产品批号), mySystem.Parameter.conn);
            dt退货台账 = new DataTable("产品退货记录");
            da退货台账.Fill(dt退货台账);
            dgv退货台账.DataSource = dt退货台账;
        }

        private void btn查询出库单_Click(object sender, EventArgs e)
        {
            string sql = "select * from 出库单详细信息 where 出库日期 between '{0}' and '{1}' and 发货公司 like '%{2}%' and 客户 like '%{3}%' and 存货名称 like '%{4}%' and 批号 like '%{5}%'";
            DateTime start = dtp出库单出库开始时间.Value;
            DateTime end = dtp出库单出库结束时间.Value;
            string 发货公司 = tb出库单发货公司.Text;
            string 客户 = tb退货台账客户名称.Text;
            string 产品名称 = tb退货台账产品名称.Text;
            string 产品批号 = tb退货台账产品批号.Text;
            da出库单 = new SqlDataAdapter(string.Format(sql, start, end, 发货公司, 客户, 产品名称, 产品批号), mySystem.Parameter.conn);
            dt出库单 = new DataTable("出库单详细信息");
            da出库单.Fill(dt出库单);
            dgv出库单.DataSource = dt出库单;
        }

        private void btn查询库存台账_Click(object sender, EventArgs e)
        {
            string sql = "select * from 库存台帐 where 供应商名称 like '%{0}%' and 产品代码 like '%{1}%' and 状态 like '%{2}%'";
           
            string 供应商名称 = tb存货台账厂家名称.Text;
            string 产品代码 = tb库存台账存货代码.Text;
            string 状态 = cmb库存台账状态.Text;
            da库存台账 = new SqlDataAdapter(string.Format(sql, 供应商名称, 产品代码, 状态), mySystem.Parameter.conn);
            dt库存台账 = new DataTable("库存台帐");
            cb库存台账 = new SqlCommandBuilder(da库存台账);
            da库存台账.Fill(dt库存台账);
            dgv库存台账.DataSource = dt库存台账;
        }


        private void btn查询检验台账_Click(object sender, EventArgs e)
        {
            string sql = "select * from 检验台账 where 供应商名称 like '%{0}%' and 物料代码 like '%{1}%'";

            string 供应商名称 = tb检验台账厂家名称.Text;
            string 产品代码 = tb检验台账存货代码.Text;
            da检验台账 = new SqlDataAdapter(string.Format(sql, 供应商名称, 产品代码), mySystem.Parameter.conn);
            dt检验台账 = new DataTable("检验台账");
            da检验台账.Fill(dt检验台账);
            dgv检验台账.DataSource = dt检验台账;
        }

        private void btn保存库存台账_Click(object sender, EventArgs e)
        {
            da库存台账.Update(dt库存台账);
        }

        private void btn盘点库存台账_Click(object sender, EventArgs e)
        {
            if (dgv库存台账.SelectedCells != null)
            {
                foreach (DataGridViewCell dgvc in dgv库存台账.SelectedCells)
                {
                    dgv库存台账["是否盘点", dgvc.RowIndex].Value = true;
                    dgv库存台账.Rows[dgvc.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgv库存台账["现存件数", dgvc.RowIndex].Value = dgv库存台账["实盘数量", dgvc.RowIndex].Value;
                }
            }
        }

        //设置datagridview背景颜色，待审核标红
        private void setDataGridViewBackColor(String checker, DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[checker].Value.ToString() == "__待审核")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 0);
                }
                else if (dgv.Rows[i].Cells[checker].Value.ToString() == "")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
            }
        }

        private void btn打印二维码_Click(object sender, EventArgs e)
        {
            if (dgv库存台账.CurrentCell == null) return;
            int 库存ID = Convert.ToInt32(dgv库存台账.CurrentCell.OwningRow.Cells["ID"].Value);
            SqlDataAdapter da = new SqlDataAdapter(String.Format("select * from 二维码信息 where 库存ID={0}",库存ID),mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            String id = mySystem.Other.InputDataGridView.getIDs("", dt, false);
            if ("" == id)
            {
                return;
            }
            da = new SqlDataAdapter(String.Format("select * from 二维码信息 where ID={0}", id), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count==0)   {
                MessageBox.Show("找不到ID为"+id+"的二维码");
            }
            String erwema = dt.Rows[0]["二维码"].ToString();
            if (erwema == "") return;
            ZXing.BarcodeWriter bw = new BarcodeWriter();

            bw.Format = BarcodeFormat.QR_CODE;
            Bitmap b = bw.Write(erwema);
            b.Save("code.bmp");


            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += pd_PrintPage;
            PrintDialog pdlg = new PrintDialog();
            pdlg.Document = pd;
            if (pdlg.ShowDialog() != DialogResult.OK) return;
            pd.Print();
        }

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float w = e.MarginBounds.Width;
            float h = e.MarginBounds.Height;

            e.Graphics.DrawImage(Image.FromFile("code.bmp"), x, y, w, h);
        }


    }
}
