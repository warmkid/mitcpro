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
    }
}
