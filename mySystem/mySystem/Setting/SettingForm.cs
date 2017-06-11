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
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Controls.Clear();
            Setting_CleanArea setcleanDlg = new Setting_CleanArea();
            setcleanDlg.TopLevel = false;
            setcleanDlg.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(setcleanDlg);
            setcleanDlg.Show();
        }
    }
}
