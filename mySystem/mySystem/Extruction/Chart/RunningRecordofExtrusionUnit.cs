﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//this form is about the 8th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class RunningRecordofExtrusionUnit : Form
    {
        public RunningRecordofExtrusionUnit()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("data has benn collecteed and more operarion is under developmen");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
            check.ShowDialog();
        }

       
    }
}
