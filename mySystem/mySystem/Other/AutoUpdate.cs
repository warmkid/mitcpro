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
    public partial class AutoUpdate : Form
    {
        public AutoUpdate()
        {
            InitializeComponent();
        }

        private void AutoUpdate_Load(object sender, EventArgs e)
        {
           
        }

        string[] readLines()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"filelist.txt"))
            {
                List<string> lst = new List<string>();
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    lst.Add(s);
                }
                return lst.ToArray();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                start();
                MessageBox.Show("更新完毕");
                this.Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show("更新失败，请发送AutoUpdate.log文件的内容给管理员");
                System.IO.File.WriteAllText("AutoUpdate.log", ee.Message + "\n" + ee.StackTrace);
                this.Close();
            }
        }

        void start()
        {
            // 获取文件列表
            MainForm.DownloadFile(@"http://123.206.90.229/erp/filelist.txt", @"filelist.txt");
            string[] files = readLines();
            int num_files = files.Length;
            progressBar总.Maximum = num_files;
            string url;
            string filepath;
            for (int i = 0; i < num_files; ++i)
            {
                progressBar总.Value = i + 1;
                label总.Text = string.Format("总进度:{0}/{1}", i + 1, num_files);
                url = files[i].Split('\t')[0].Replace("#", "%23");
                url = System.Web.HttpUtility.UrlPathEncode(
                    @"http://123.206.90.229/erp/" + url);
                filepath = files[i].Split('\t')[1];
                MainForm.DownloadFile(url, filepath, progressBar文件, label文件);
            }
            // 更新当前版本号
            System.IO.File.Delete(@"version.txt");
            System.IO.File.Move(@"r_version.txt", @"version.txt");
        }
    }
}
