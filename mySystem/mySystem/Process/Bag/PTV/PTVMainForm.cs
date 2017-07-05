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
            material.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PTVBag_batchproduction batch = new PTVBag_batchproduction();
            batch.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            PTVBag_innerpackaging inner = new PTVBag_innerpackaging();
            inner.ShowDialog();
        }

        private void B8Btn_Click(object sender, EventArgs e)
        {
            PTVBag_clearance clearance = new PTVBag_clearance();
            clearance.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            PTVBag_productioninstruction pro_ins = new PTVBag_productioninstruction();
            pro_ins.ShowDialog();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            PTVBag_checklist check = new PTVBag_checklist();
            check.ShowDialog();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            PTVBag_dailyreport daily = new PTVBag_dailyreport();
            daily.ShowDialog();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            PTVBag_runningrecordofdf df = new PTVBag_runningrecordofdf();
            df.ShowDialog();
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            PTVBag_weldingrecordofwave wave = new PTVBag_weldingrecordofwave();
            wave.ShowDialog();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            PTVBag_runningrecordofyk yk = new PTVBag_runningrecordofyk();
            yk.ShowDialog();
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            PTVBag_weldingrecordofwave wave = new PTVBag_weldingrecordofwave();
            wave.ShowDialog();
        }
    }
}
