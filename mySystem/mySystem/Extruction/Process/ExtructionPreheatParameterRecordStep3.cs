using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : Form
    {
        private ExtructionProcess extructionformfather = null;
        public string recorder; //记录人
        public string checker; //复核人
        public string date; //日期

        private class record
        {

            public string sizephi; //模芯规格 phi
            public string sizegap; //模芯规格 gap
            public string time1;   //预热开始时间
            public string time2;   //保温结束时间
            public string time3;   //保温开始时间
            public string time4;   //保温结束时间
            public string time5;   //保温结束时间
            public string PS;
            public record()
            {
                sizephi = "";
                sizegap = "";
                time1 = "";
                time2 = "";
                time3 = "";
                time4 = "";
                time5 = "";
                PS = "";
            }
        }
        private record recorddata = new record();

        public ExtructionPreheatParameterRecordStep3(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;

            InformationInitialize();
            //TabelPaint();
        }

        private void InformationInitialize()
        {
            ///***********************表头数据初始化************************///
            recorder = "记录人员";
            checker = "复核人员";
            date = DateTime.Now.ToLongDateString().ToString();
            /*
            this.recorderlabel.Text = recorder;
            this.checkerlabel.Text = checker;
            this.datelabel.Text = date;
             * */
            this.PSbox.AutoSize = false;
            this.PSbox.Height = 32;
        }       

        private void TabelPaint()
        {
            Graphics g = this.CreateGraphics();
            this.Show();
            //出来一个画笔,这只笔画出来的颜色是红的  
            Pen p = new Pen(Brushes.Red);

            //创建两个点  
            Point p1 = new Point(0, 0);
            Point p2 = new Point(1000, 1000);

            //将两个点连起来  
            g.DrawLine(p, p1, p2);
        }

        public void DataSave()
        {
            recorddata.sizephi = this.phiBox.Text;
            recorddata.sizegap = this.gapBox.Text;
            recorddata.time1 = this.timeBox1.Text;
            recorddata.time2 = this.timeBox1.Text;
            recorddata.time3 = this.timeBox1.Text;
            recorddata.time4 = this.timeBox1.Text;
            recorddata.time5 = this.timeBox1.Text;
            recorddata.PS = this.PSbox.Text;
        }
    }
}
