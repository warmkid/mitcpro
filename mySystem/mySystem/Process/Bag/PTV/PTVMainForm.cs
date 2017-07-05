using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVMainForm : Form
    {
        public PTVMainForm()
        {
            InitializeComponent();
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            PTVBag_materialrecord material = new PTVBag_materialrecord();
            material.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PTVBag_batchproduction batch = new PTVBag_batchproduction();
            batch.Show();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            PTVBag_innerpackaging inner = new PTVBag_innerpackaging();
            inner.Show();
        }

        private void B8Btn_Click(object sender, EventArgs e)
        {
            PTVBag_clearance clearance = new PTVBag_clearance();
            clearance.Show();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            PTVBag_productioninstruction pro_ins = new PTVBag_productioninstruction();
            pro_ins.Show();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            PTVBag_checklist check = new PTVBag_checklist();
            check.Show();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            PTVBag_dailyreport daily = new PTVBag_dailyreport();
            daily.Show();
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            PTVBag_weldingrecordofwave wave = new PTVBag_weldingrecordofwave();
            wave.ShowDialog();
        }
    }
}
