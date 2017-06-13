using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//this form is about the 13th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class HandoverRecordofExtrusionProcess : Form
    {
        public int quiz=14;
        //int quiz=14;
  

        public HandoverRecordofExtrusionProcess()
        {
            InitializeComponent();
            this.GenerateQuiz(quiz);
            //this part to add the confirm items
             
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            //this part automatically fill the blanks
            this.txbBatchNoD.Text = "111";
            this.txbBatchNoN.Text = "112";
            this.txbAmountsD.Text = "100";
            this.txbAmountsN.Text = "100";
            this.txbCodeD.Text = "10000";
            this.txbCodeN.Text = "10000";
            this.txbAbnormalD.Text = "no";
            this.txbAbnormalN.Text = "no";
            this.txbHandinD.Text = "wang";
            this.txbHandinN.Text = "zhang";
            this.txbTakeinD.Text = "sun";
            this.txbTakeinN.Text = "li";
            this.dtpDay.Format = DateTimePickerFormat.Time;
            this.dtpDay.Value = DateTime.Now;
            this.dtpNight.Format = DateTimePickerFormat.Time;
            this.dtpNight.Value = DateTime.Now;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        void GenerateQuiz(int quiz)
        {
            string[][] items = new string[quiz][];
            items[0] = new string[] {"confirm balabala..."};
            items[1] = new string[]{"confirm balabala... again"};
            items[2] = new string[]{"fffff"};
            items[3] = new string[]{"confirm balabala..."};
            items[4] = new string[]{"confirm balabala... again"};
            items[5] = new string[]{"fffff"};
            items[6] = new string[]{"confirm balabala..."};
            items[7] = new string[]{"confirm balabala... again"};
            items[8] = new string[]{"fffff"};
            items[9] = new string[]{"confirm balabala..."};
            items[10] = new string[]{"confirm balabala... again"};
            items[11] = new string[]{"fffff"};
            items[12] = new string[]{"confirm balabala..."};
            items[13] = new string[]{"confirm balabala... again"};


            int x0 = 30, y0 = 100, w = 160, d = 20;
            Label[] labels = new Label[quiz];
            CheckBox[,] checkboxes = new CheckBox[quiz,2];
            for (int r = 0; r < quiz; r++)
            {
                Label lb = new Label();
                //lb.Name = "lbQuiz" + (r + 1).ToString();
                lb.Top = y0 + r * d;
                lb.Left = x0;
                lb.Width = w;
                lb.Height = d;
                lb.Visible = true;
                lb.Text = ""+items[r];
                labels[r] = lb;
                this.Controls.Add(lb);
                for (int turn = 0; turn < 2; turn++)
                {
                    CheckBox ckb = new CheckBox();
                    ckb.Top = y0 + r * d;
                    ckb.Left = 2 * x0 + lb.Width+turn*d;
                    ckb.Width = d;
                    ckb.Height = d;
                    ckb.Visible = true;
                    ckb.Checked = true;
                    checkboxes[r, turn] = ckb;
                    this.Controls.Add(ckb);
                }
            }
        }
    }
}

