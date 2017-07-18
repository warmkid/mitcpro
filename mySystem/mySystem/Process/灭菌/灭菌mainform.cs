using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.灭菌
{
    public partial class 灭菌mainform : Form
    {
        public 灭菌mainform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gamma射线辐射灭菌委托单 mydlg = new Gamma射线辐射灭菌委托单();
            mydlg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            辐照灭菌产品验收记录 mydlg = new 辐照灭菌产品验收记录();
            mydlg.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            辐照灭菌台帐 mydlg = new 辐照灭菌台帐();
            mydlg.ShowDialog();
        }
    }
}
