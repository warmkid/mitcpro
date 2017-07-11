using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Process.Bag.BTV
{
    public partial class BTVMainForm : Form
    {
        public BTVMainForm()
        {
            InitializeComponent();
        }

        private void Btn生产领料_Click(object sender, EventArgs e)
        {
            BTVMaterialRecord mydlg = new BTVMaterialRecord();
            mydlg.ShowDialog();
        }

        private void Btn批生产记录_Click(object sender, EventArgs e)
        {
            BTVBatchProduction mydlg = new BTVBatchProduction();
            mydlg.ShowDialog();
        }

        private void Btn内包装_Click(object sender, EventArgs e)
        {
            BTVInnerPackage mydlg = new BTVInnerPackage();
            mydlg.ShowDialog();
        }

        private void Btn清场记录_Click(object sender, EventArgs e)
        {
            BTVClearanceRecord mydlg = new BTVClearanceRecord();
            mydlg.ShowDialog();
        }

        private void Btn生产指令_Click(object sender, EventArgs e)
        {
            BTVProInstruction mydlg = new BTVProInstruction();
            mydlg.ShowDialog();
        }

        private void Btn生产前确认_Click(object sender, EventArgs e)
        {
            BTVConfirmBefore mydlg = new BTVConfirmBefore();
            mydlg.ShowDialog();
        }

        private void Btn切管记录_Click(object sender, EventArgs e)
        {
            BTVCutPipeRecord mydlg = new BTVCutPipeRecord();
            mydlg.ShowDialog();
        }

        private void Btn装配确认_Click(object sender, EventArgs e)
        {
            BTVAssemblyConfirm mydlg = new BTVAssemblyConfirm();
            mydlg.ShowDialog();
        }

        private void Btn2D袋体生产记录_Click(object sender, EventArgs e)
        {
            BTV2DProRecord mydlg = new BTV2DProRecord();
            mydlg.ShowDialog();
        }

        private void Btn关键尺寸确认_Click(object sender, EventArgs e)
        {
            BTVKeySizeConfirm mydlg = new BTVKeySizeConfirm();
            mydlg.ShowDialog();
        }

        private void Btn原材料分装_Click(object sender, EventArgs e)
        {
            BTVRawMaterialDispensing mydlg = new BTVRawMaterialDispensing();
            mydlg.ShowDialog();
        }

        private void Btn底封机运行记录_Click(object sender, EventArgs e)
        {
            BTVRunningRecordDF mydlg = new BTVRunningRecordDF();
            mydlg.ShowDialog();
        }

        private void Btn泄漏测试记录_Click(object sender, EventArgs e)
        {
            BTVLeakTest mydlg = new BTVLeakTest();
            mydlg.ShowDialog();
        }

        private void Btn2D与船型_Click(object sender, EventArgs e)
        {
            BTV2DShipHeat mydlg = new BTV2DShipHeat();
            mydlg.ShowDialog();
        }

        private void Btn瓶口焊接机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordPK mydlg = new BTVRunningRecordPK();
            mydlg.ShowDialog();
        }

        private void Btn多功能热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJMulti mydlg = new BTVRunningRecordRHJMulti();
            mydlg.ShowDialog();
        }

        private void Btn3D袋体生产记录_Click(object sender, EventArgs e)
        {
            BTV3DProRecord mydlg = new BTV3DProRecord();
            mydlg.ShowDialog();
        }

        private void Btn单管口热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJsingle mydlg = new BTVRunningRecordRHJsingle();
            mydlg.ShowDialog();
        }

        private void Btn90度热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJ90 mydlg = new BTVRunningRecordRHJ90();
            mydlg.ShowDialog();
        }

        private void Btn封口热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJseal mydlg = new BTVRunningRecordRHJseal();
            mydlg.ShowDialog();
        }

        private void Btn打孔及与图纸_Click(object sender, EventArgs e)
        {
            BTVPunchDrawingConfirm mydlg = new BTVPunchDrawingConfirm();
            mydlg.ShowDialog();
        }
    }
}
