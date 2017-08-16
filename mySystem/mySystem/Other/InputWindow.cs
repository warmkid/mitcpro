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
    public partial class InputTextWindow : BaseForm
    {
        public InputTextWindow()
        {
            InitializeComponent();
        }

        void setLabel(String lbl)
        {
            label1.Text = lbl;
        }

        public static String getString(String lbl)
        {
            InputTextWindow iw = new InputTextWindow();
            iw.label1.Text = lbl;
            if (DialogResult.OK == iw.ShowDialog())
            {
                return iw.textBox1.Text;
            }
            else
            {
                return "";
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
