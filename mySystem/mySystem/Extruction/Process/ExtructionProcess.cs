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
using System.Data.SqlClient;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionProcess : Form
    {
        private ExtructionForm blowformfather = null;
        private int StepShowState = 1;
        public bool[] stepchecked = { false, false, false, false, false, false };
        private SqlConnection conn = null;
        
        Record_extrusClean step1form = null;
        ExtructionCheckBeforePowerStep2 step2form = null;
        ExtructionPreheatParameterRecordStep3 step3form = null;
        ExtructionTransportRecordStep4 step4form = null;
        Record_extrusSupply step5form = null;
        ExtructionpRoductionAndRestRecordStep6 step6form = null;
        ExtructionCheck stepcheckform = null;

        public ExtructionProcess(ExtructionForm Mainform, int stepcurrent, SqlConnection Formconn)
        {
            InitializeComponent();
            blowformfather = Mainform;
            conn = Formconn;

            //读取当前保存、审核完成的步骤
            string sql = "Select * From extrusion";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter daSQL = new SqlDataAdapter(comm);
            DataTable dtSQL = new DataTable();
            daSQL.Fill(dtSQL);
            int stepshow = Convert.ToInt32(dtSQL.Rows[0]["step_status"]);
            int stepcheck = Convert.ToInt32(dtSQL.Rows[0]["step_checked_status"]);

            //检查stepcheck>StepShowState的情况
            if (stepcheck > StepShowState)
            {
                sql = "update extrusion set step_checked_status = " + stepshow.ToString() + " where id =1";
                comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
            }
            //检查(StepShowState - stepcheck)>1的情况
            if ((StepShowState - stepcheck)>1)
            {
                sql = "update extrusion set step_status = " + stepcheck.ToString() + " where id =1";
                comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
            }

            comm.Dispose();
            daSQL.Dispose();
            dtSQL.Dispose();

            //根据StepShowState显示界面
            if (stepshow == 0)
            {
                ShowView(1);
                StepShowState = 1;
                CheckBtn.Enabled = false;
                NextBtn.Enabled = false;
            }
            else
            {
                StepShowState = stepshow;
                ShowView(StepShowState);
                if (stepcheck == StepShowState)
                {                    
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                    NextBtn.Enabled = true;
                }
                for (int i = 0; i < stepcheck; i++)
                {
                    stepchecked[i] = true;
                }            
            }

            MessageBox.Show("stepshow = " + stepshow.ToString() + ";  stepcheck = " + stepcheck.ToString()+ ";");

            /*StepShowState = stepcurrent;
            ShowView(StepShowState);

            CheckBtn.Enabled = false;
            NextBtn.Enabled = false;
             * */

        }

        //上一页按钮
        private void BackBtn_Click(object sender, EventArgs e)
        {
            if ((StepShowState > 1) && (StepShowState < 8))
            {
                StepShowState = StepShowState - 1;
                ShowView(StepShowState);
                if (stepchecked[StepShowState - 1])
                {
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                    NextBtn.Enabled = true;
                }
                else
                {
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = false;
                    NextBtn.Enabled = false;
                }
            }
        }

        //下一页按钮
        private void NextBtn_Click(object sender, EventArgs e)
        {
            if ((StepShowState > 0) && (StepShowState < 7))
            {
                StepShowState = StepShowState + 1;
                ShowView(StepShowState);
                if (StepShowState == 7)
                {
                    NextBtn.Enabled = true;
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                }
                else
                {
                    if (stepchecked[StepShowState - 1])
                    {
                        SaveBtn.Enabled = false;
                        CheckBtn.Enabled = false;
                        NextBtn.Enabled = true;
                    }
                    else
                    {
                        SaveBtn.Enabled = true;
                        CheckBtn.Enabled = false;
                        NextBtn.Enabled = false;
                    }
                }
            }       
        }

        //确认按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if ((StepShowState > 0) && (StepShowState < 7))
            {
                switch (StepShowState)
                {
                    case 1:
                        step1form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    case 2:
                        step2form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    case 3:
                        step3form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    case 4:
                        step4form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    case 5:
                        step5form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    case 6:
                        step6form.DataSave();
                        CheckBtn.Enabled = true;
                        break;
                    default:
                        break;
                }
            }  
        }

        //审核通过按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            
            LoginForm check = new LoginForm(conn);
            check.LoginButton.Text = "审核通过";
            check.ExitButton.Text = "取消";
            check.ShowDialog();

            if ((StepShowState > 0) && (StepShowState < 7))
            {
                SaveBtn.Enabled = false;
                CheckBtn.Enabled = false;
                NextBtn.Enabled = true;
                stepchecked[StepShowState - 1] = true;

                string sqlstr = "update extrusion set step_checked_status = " + StepShowState.ToString() + " where id =1";
                SqlCommand com = new SqlCommand(sqlstr, conn);
                com.ExecuteNonQuery();
                com.Dispose(); 
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
                    step2form = new ExtructionCheckBeforePowerStep2(this, conn);
                    step2form.FormBorderStyle = FormBorderStyle.None;
                    step2form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step2form);
                    //step2form.Disabled();
                    step2form.Show();
                    break;
                case 3:
                    step3form = new ExtructionPreheatParameterRecordStep3(this, conn);
                    step3form.FormBorderStyle = FormBorderStyle.None;
                    step3form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step3form);
                    //step3form.Disabled();
                    step3form.Show();
                    break;
                case 4:
                    step4form = new ExtructionTransportRecordStep4(this, conn);
                    step4form.FormBorderStyle = FormBorderStyle.None;
                    step4form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step4form);
                    //step4form.Disabled();
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
                    this.BackBtn.Text = "上一页";
                    this.SaveBtn.Visible = true;
                    this.CheckBtn.Visible = true;
                    step6form = new ExtructionpRoductionAndRestRecordStep6(this, conn);
                    step6form.FormBorderStyle = FormBorderStyle.None;
                    step6form.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(step6form);
                    //step6form.Disabled();
                    step6form.Show();
                    break;
                case 7:
                    this.BackBtn.Text = "返回";
                    this.SaveBtn.Visible = false;
                    this.CheckBtn.Visible = false;
                    this.NextBtn.Text = "确认打印";
                    stepcheckform = new ExtructionCheck(this, conn);
                    stepcheckform.FormBorderStyle = FormBorderStyle.None;
                    stepcheckform.TopLevel = false;
                    this.StepViewPanel.Controls.Clear();
                    this.StepViewPanel.Controls.Add(stepcheckform);
                    stepcheckform.Show();
                    break;
                default:
                    break;
            }
        }

        private void StepViewPanel_Paint(object sender, PaintEventArgs e)
        {

        }             
    }
}
