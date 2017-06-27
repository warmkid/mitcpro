using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class PlanForm : Form
    {
        public PlanForm()
        {
            InitializeComponent();
        }

        private void AddPlanBtn_Click(object sender, EventArgs e)
        {
            AddPlanForm addplanDlg = new AddPlanForm();
            addplanDlg.Show();
        }

    }
}
