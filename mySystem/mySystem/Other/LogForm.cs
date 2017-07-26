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
    /// <summary>
    /// 用于查看日志的窗体类
    /// 通过setLog(String)函数写入日志
    /// </summary>
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 该函数写入日志，并返回自身
        /// 所以可以这样写：logForm.setLog(log).Show();
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public LogForm setLog(String log)
        {
            textBox1.Lines = log.Split('\n');
            return this;
        }
    }
}
