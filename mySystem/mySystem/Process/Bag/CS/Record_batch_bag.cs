using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.Bag
{
    public partial class Record_batch_bag :mySystem.BaseForm
    {
        List<string> record = null;
        public Record_batch_bag(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            record = new List<string>();
            record.Add("SOP-MFG-105-R01 制袋工序批生产记录封面.xlsx");
            record.Add("SOP-MFG-303-R01A 2#制袋工序生产指令.xlsx");
            record.Add("SOP-MFG-303-R02A  2# 制袋机开机前确认表.xlsx");
            record.Add("SOP-MFG-102-R01A  生产领料使用记录.xlsx");
            record.Add("SOP-MFG-303-R03A  2# 制袋机运行记录.xlsx");
            record.Add("SOP-MFG-109-R01A 产品内包装记录.xlsx");
            record.Add("SOP-MFG-110-R01A 清场记录.xlsx");
            record.Add("SOP-MFG-303-R06A  CS制袋日报表.xlsx");
            record.Add("QB-PA-PP-03-R01A 产品外观和尺寸检验记录");
            record.Add("QB-PA-PP-03-R02A 产品热合强度检验记录");

            initrecord();
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
                dr.Cells[0].Value = i + 1;
                dr.Cells[1].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
