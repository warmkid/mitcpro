using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using mySystem;
using WindowsFormsApplication1;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionProcess : Form
    {
        private ExtructionForm blowformfather = null;
        private int StepState = 1;
        Record_extrusClean step1form = null;
        ExtructionCheckBeforePowerStep2 step2form = null;
        ExtructionPreheatParameterRecordStep3 step3form = null;
        ExtructionTransportRecordStep4 step4form = null;
        Record_extrusSupply step5form = null;
        ExtructionpRoductionAndRestRecordStep6 step6form = null;

        public ExtructionProcess(ExtructionForm Mainform, int stepcurrent)
        {
            InitializeComponent();
            blowformfather = Mainform;
            StepViewPanel.Size = new Size(1180, 523);

            StepState = stepcurrent;
            ShowView(stepcurrent);
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            if ((StepState > 1) && (StepState< 7 ))
            {
                StepState = StepState - 1;
                ShowView(StepState);
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if ((StepState > 0) && (StepState < 6))
            {
                StepState = StepState + 1;
                ShowView(StepState);
            }
        }

        private void ShowView(int ProcessState)
        {
            switch (ProcessState)
            {
                case 1:
                    this.BackBtn.Enabled = false;
                    step1form = new Record_extrusClean(this);
                    step1form.FormBorderStyle = FormBorderStyle.None;
                    step1form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step1form);
                    step1form.Show();
                    break;
                case 2:
                    this.BackBtn.Enabled = true;
                    step2form = new ExtructionCheckBeforePowerStep2(this);
                    step2form.FormBorderStyle = FormBorderStyle.None; 
                    step2form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step2form);
                    step2form.Show();
                    break;
                case 3:
                    step3form = new ExtructionPreheatParameterRecordStep3(this);
                    step3form.FormBorderStyle = FormBorderStyle.None;
                    step3form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step3form);
                    step3form.Show();
                    break;
                case 4:
                    step4form = new ExtructionTransportRecordStep4(this);
                    step4form.FormBorderStyle = FormBorderStyle.None;
                    step4form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step4form);
                    step4form.Show();
                    break;
                case 5:
                    this.NextBtn.Text = "下一页";
                    step5form = new Record_extrusSupply(this);
                    step5form.FormBorderStyle = FormBorderStyle.None;
                    step5form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step5form);
                    step5form.Show();
                    break;
                case 6:
                    this.NextBtn.Text = "完成";
                    step6form = new ExtructionpRoductionAndRestRecordStep6(this);
                    step6form.FormBorderStyle = FormBorderStyle.None;
                    step6form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step6form);
                    step6form.Show();
                    break;
                default:
                    break;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            switch (StepState)
            {
                case 1:
                    step1form.DataSave();
                    MessageBox.Show(this, "保存成功！");
                    break;
                case 2:                    
                    step2form.DataSave();
                    //MessageBox.Show(this, "成功向文件中写入内容！");
                    break;
                case 3:
                    step3form.DataSave();
                    //MessageBox.Show(this, "成功向文件中写入内容！");
                    break;
                case 4:
                    step4form.DataSave();
                    //MessageBox.Show(this, "成功向文件中写入内容！");
                    break;
                case 5:
                    //step5form.DataSave();
                    //MessageBox.Show(this, "成功向文件中写入内容！");
                    break;
                case 6:
                    step6form.DataSave();
                    //MessageBox.Show(this, "成功向文件中写入内容！");                    
                    break;
                default:
                    break;
            }
        }    
    }
}
