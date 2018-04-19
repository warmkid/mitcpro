using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Other
{
    public partial class 我的任务 : Form
    {
        public 我的任务()
        {
            InitializeComponent();
            label2.Text = "========================";
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void set指令(string[] ins)
        {
        }
        public void set指令(string ins)
        {
            if (ins == null||ins.Trim()=="") label1.Text = "无待接收生产指令！";
            else label1.Text = "请接收指令：\n" + ins;
        }

        public void set表单(String[] tbls)
        {
        }

        public void set表单(String tbls)
        {
            if(tbls==null||tbls.Trim()=="")  label3.Text = "无";
            else label3.Text = tbls;
        }
       
        
    }
}
