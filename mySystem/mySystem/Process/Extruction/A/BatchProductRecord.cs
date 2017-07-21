using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BatchProductRecord
{
    public partial class BatchProductRecord : mySystem.BaseForm
    {
        List<string> record=null;
        public BatchProductRecord(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            record = new List<string>();
            record.Add("SOP-MFG-301-R01 吹膜工序生产指令");
            record.Add("SOP-MFG-301-R02 吹膜生产日报表");
            record.Add("SOP-MFG-301-R03 吹膜机组清洁记录");
            record.Add("SOP-MFG-301-R04 吹膜机组开机确认表");
            record.Add("SOP-MFG-301-R05 吹膜机组预热参数记录表");
            record.Add("SOP-MFG-301-R06 吹膜供料记录");
            record.Add("SOP-MFG-301-R07 吹膜供料系统运行记录");
            record.Add("SOP-MFG-301-R08 吹膜机组运行记录");
            record.Add("SOP-MFG-301-R09 吹膜工序生产和检验记录");
            record.Add("SOP-MFG-301-R10 吹膜工序废品记录");
            record.Add("SOP-MFG-301-R11 吹膜工序清场记录");
            record.Add("SOP-MFG-301-R12 吹膜工序物料平衡记录");
            record.Add("SOP-MFG-301-R13 吹膜工序领料退料记录");
            record.Add("SOP-MFG-301-R14 吹膜岗位交接班记录");
            record.Add("SOP-MFG-109-R01A 产品内包装记录");
            record.Add("SOP-MFG-111-R01A 产品外包装记录");
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
                dr.Cells[0].Value = i+1;
                dr.Cells[1].Value = record[i];
                dataGridView1.Rows.Add(dr);
            }
        }

        private void bt确认_Click(object sender, EventArgs e)
        {

        }
    }
}
