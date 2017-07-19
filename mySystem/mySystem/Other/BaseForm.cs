﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class BaseForm : Form
    {
        public MainForm mainform;
        public BaseForm(MainForm mForm)
        {
            mainform = mForm;
            InitializeComponent();
        }

        public BaseForm()
        { 
        
        }

        public virtual void CheckResult()
        {
        }



    }
}
