using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mySystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                String dt = DateTime.Now.ToString("yyyy-MM-dd_hh-mm_ss");
                MessageBox.Show(String.Format("出现错误，信息已记录至{0}.txt ", dt));
                System.IO.File.WriteAllText(String.Format("{0}.txt ", dt), e.StackTrace);
                System.Diagnostics.Process.Start("explorer.exe", ".");
            }
        }
    }
}
