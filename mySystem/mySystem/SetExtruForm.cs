using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace mySystem
{
    public partial class SetExtruForm : Form
    {
        public SetExtruForm()
        {
            InitializeComponent();
        }

        private void cleanPanel_Paint(object sender, PaintEventArgs e)
        {
            cleanPanel.Controls.Clear();
            Setting_CleanArea setcleanDlg = new Setting_CleanArea();
            setcleanDlg.TopLevel = false;
            setcleanDlg.FormBorderStyle = FormBorderStyle.None;
            cleanPanel.Controls.Add(setcleanDlg);
            setcleanDlg.Show();
        }
    }
}
