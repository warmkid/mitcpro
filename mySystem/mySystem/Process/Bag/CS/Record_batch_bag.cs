using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Bag
{
    public partial class Record_batch_bag :mySystem.BaseForm
    {
        List<string> record = null;
        List<Int32> noid = null;
        int totalPage = 4;

        public Record_batch_bag(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            record = new List<string>();
            //record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面.xlsx");
            record.Add("SOP-MFG-303-R01A 2#制袋工序生产指令.xlsx");
            record.Add("SOP-MFG-303-R02A 2# 制袋机开机前确认表.xlsx");
            record.Add("SOP-MFG-102-R01A 生产领料使用记录.xlsx");
            record.Add("SOP-MFG-303-R03A 2# 制袋机运行记录.xlsx");
            record.Add("SOP-MFG-109-R01A 产品内包装记录.xlsx");
            record.Add("SOP-MFG-110-R01A 清场记录.xlsx");
            record.Add("SOP-MFG-303-R06A CS制袋日报表.xlsx");
            record.Add("QB-PA-PP-03-R01A 产品外观和尺寸检验记录");
            record.Add("QB-PA-PP-03-R02A 产品热合强度检验记录");

            initrecord();
            initly();
        }

        private void initly()
        {
            // 读取各表格数据，并显示页数

            OleDbDataAdapter da;
            DataTable dt;
            noid = new List<int>();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells[0].Value = false;
            }

            dataGridView1.Columns[totalPage].ReadOnly = true;
           
            //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            // 生产指令
            int temp;
            int idx = 0;
            int tempid;
            da = new OleDbDataAdapter("select * from 生产指令 where  生产指令编号='" + mySystem.Parameter.csbagInstruction + "'", mySystem.Parameter.connOle);
            dt = new DataTable("生产指令");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //开机前确认
            idx++;
            da = new OleDbDataAdapter("select * from 制袋机组开机前确认表 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("制袋机组开机前确认表");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 生产领料使用记录
            idx++;
            da = new OleDbDataAdapter("select * from CS制袋领料记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("CS制袋领料记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 制袋机运行记录
            idx++;
            da = new OleDbDataAdapter("select * from 制袋机组运行记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("制袋机组运行记录制袋机组运行记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            
            //  产品内包装记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品内包装记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("产品内包装记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 清场记录
            idx++;
            da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("清场记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //  CS制袋日报表
            idx++;
            da = new OleDbDataAdapter("select * from CS制袋日报表 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("CS制袋日报表");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            //  产品外观和尺寸检验记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("产品外观和尺寸检验记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 产品热合强度检验记录
            idx++;
            da = new OleDbDataAdapter("select * from 产品热合强度检验记录 where  生产指令ID=" + mySystem.Parameter.csbagInstruID, mySystem.Parameter.connOle);
            dt = new DataTable("产品热合强度检验记录");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                for (int i = 0; i < temp; ++i)
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
        }
        private void initrecord()
        {
            for (int i = 0; i < record.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i + 1;
                dr.Cells[3].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }
    }
}
