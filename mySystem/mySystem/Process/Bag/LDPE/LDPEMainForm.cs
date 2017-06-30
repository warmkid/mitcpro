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
    }
}
