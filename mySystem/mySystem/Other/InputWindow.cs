using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Other
{
    public partial class InputWindow : BaseForm
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        void setLabel(String lbl)
        {
            label1.Text = lbl;
        }

        public static String getString(String lbl)
        {
            InputWindow iw = new InputWindow();
            iw.label1.Text = lbl;
            iw.ShowDialog();
            return iw.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
