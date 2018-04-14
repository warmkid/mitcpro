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
    public partial class Connecting : Form
    {
        BackgroundWorker worker;
        public Connecting(BackgroundWorker w)
        {
            InitializeComponent();
            worker = w;
            w.ProgressChanged += w_ProgressChanged;
            w.RunWorkerCompleted += w_RunWorkerCompleted;
        }

        void w_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void w_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Close();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
