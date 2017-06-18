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

namespace mySystem
{
    public partial class ExtructionForm : Form
    {
        SqlConnection conn = null;
        int ProcessStep = 0;     
        public ExtructionForm(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;

            //将内容列表添加到comboBox1中
            DataTable table = ProductionPlanDataTable();
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "display";//val这个字段为显示的值
            comboBox1.ValueMember = "id";//id这个字段为后台获取的值
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void BlowForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessStep = comboBox1.SelectedIndex;
            switch (ProcessStep)
            {
                case 0:
                    this.panel1.Controls.Clear();
                    break;
                case 1:
                    ExtructionProcess stepform = new ExtructionProcess(this, 1, this.conn);
                    stepform.FormBorderStyle = FormBorderStyle.None; //隐藏子窗体边框（去除最小化，最大化，关闭等按钮）
                    stepform.TopLevel = false; //指示子窗体非顶级窗体
                    this.panel1.Controls.Clear();
                    this.panel1.Controls.Add(stepform);//将子窗体载入panel
                    stepform.Show();
                    break;
                default:
                    break;
            }            
        }

        private DataTable ProductionPlanDataTable()
        {
            String[] PlanString = { "尚未开始", "开始" };
            DataTable dt = new DataTable();//创建一个数据集
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("display", typeof(String));
            for (int i = 0; i <= 1; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = PlanString[i];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void Chart2Btn_Click(object sender, EventArgs e)
        {
            RunningRecordofFeedingUnit myDlg = new RunningRecordofFeedingUnit(conn);
            myDlg.Show();
        }

        private void Chart3Btn_Click(object sender, EventArgs e)
        {
            RunningRecordofExtrusionUnit myDlg = new RunningRecordofExtrusionUnit(conn);
            myDlg.Show();
        }

        private void Chart4Btn_Click(object sender, EventArgs e)
        {
            WastingRecordofExtrusionUnit myDlg = new WastingRecordofExtrusionUnit(conn);
            myDlg.Show();
        }

        private void Chart6Btn_Click(object sender, EventArgs e)
        {
            MaterialBalenceofExtrusionProcess myDlg = new MaterialBalenceofExtrusionProcess(conn);
            myDlg.Show();
        }

        private void Chart7Btn_Click(object sender, EventArgs e)
        {
            HandoverRecordofExtrusionProcess myDlg = new HandoverRecordofExtrusionProcess(conn);
            myDlg.Show();
        }

        private void Chart1Btn_Click(object sender, EventArgs e)
        {
            ProdctDaily_extrus myDlg = new ProdctDaily_extrus();
            myDlg.Show();
        }

        private void Chart5Btn_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean myDlg = new Record_extrusSiteClean();
            myDlg.Show();
        }


    }
}
