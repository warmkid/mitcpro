using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1;
using mySystem.Extruction.Process;

namespace mySystem.Query
{
    public partial class QueryExtruForm : Form
    {
        public QueryExtruForm()
        {
            InitializeComponent();
        }

        //吹膜生产日报表
        private void Chart1Btn_Click(object sender, EventArgs e)
        {
            ProdctDaily_extrus myDlg = new ProdctDaily_extrus();
            myDlg.Show();
        }

        //吹膜工序物料平衡记录
        private void Chart6Btn_Click(object sender, EventArgs e)
        {
            MaterialBalenceofExtrusionProcess myDlg = new MaterialBalenceofExtrusionProcess();
            myDlg.Show();
        }
    }
}
