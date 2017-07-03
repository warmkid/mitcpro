using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPEMainForm : Form
    {
        public LDPEMainForm()
        {
            InitializeComponent();
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_materialrecord material = new LDPEBag_materialrecord();
            material.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LDPEBag_batchproduction batch = new LDPEBag_batchproduction();
            batch.Show();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_innerpackaging inner = new LDPEBag_innerpackaging();
            inner.Show();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_cleanrance cleanrance = new LDPEBag_cleanrance();
            cleanrance.Show();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_productioninstruction pro_ins = new LDPEBag_productioninstruction();
            pro_ins.Show();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_checklist check = new LDPEBag_checklist();
            check.Show();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_runningrecord run = new LDPEBag_runningrecord();
            run.Show();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_dailyreport daily = new LDPEBag_dailyreport();
            daily.Show();
        }
    }
}
