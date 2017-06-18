using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

//this form is about the 13th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class HandoverRecordofExtrusionProcess : Form
    {
        public int quiz=14;
        public int TN = 2;
        bool[,] itemcheck = new bool[14, 2];
        SqlConnection conn = null;
  

        public HandoverRecordofExtrusionProcess(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            this.GenerateQuiz(quiz);
            //this part to add the confirm items

             
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            //this part automatically fill the blanks
            this.txbInstructionId.Text = "111111";
            this.txbBatchNoD.Text = "111";
            this.txbBatchNoN.Text = "112";
            this.txbAmountsD.Text = "100";
            this.txbAmountsN.Text = "100";
            this.txbCodeD.Text = "10000";
            this.txbCodeN.Text = "10000";
            this.txbAbnormalD.Text = "no";
            this.txbAbnormalN.Text = "no";
            this.txbHandinD.Text = "wang";
            this.txbHandinN.Text = "zhang";
            this.txbTakeinD.Text = "sun";
            this.txbTakeinN.Text = "li";
            this.dtpDay.Format = DateTimePickerFormat.Time;
            this.dtpDay.Value = DateTime.Now;
            this.dtpNight.Format = DateTimePickerFormat.Time;
            this.dtpNight.Value = DateTime.Now;
            this.dtpDate.Format = DateTimePickerFormat.Long;
            this.dtpDate.Value = DateTime.Now;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            string production_date= this.dtpDate.Value.ToShortTimeString();
            string successor_time_day = this.dtpDay.Value.ToShortDateString();
            string successor_time_night = this.dtpNight.Value.ToShortDateString();
            //MessageBox.Show(production_date+"\t"+dayExchange+"\t"+nightExchange);
            string product_batch_day = this.txbBatchNoD.Text;
            string product_batch_night = this.txbBatchNoN.Text;
            
            


            
        }

        void GenerateQuiz(int quiz)
        {
            string[] items = new string[quiz];
            items[0] = "确认生产过程中无任何生产安全遗留和潜在隐患。";
            items[1] = "确认供料间原料使用及质量情况。";
            items[2] = "确认供料报警系统开关是否正常。";
            items[3] = "确认膜厚度公差是否<=10%";
            items[4] = "确认交接膜卷上下班各完成的米数，膜卷是否码放整齐。";
            items[5] = "确认温度参数是否符合工艺要求。";
            items[6] = "确认螺杆转速是否符合工艺要求。";
            items[7] = "确认牵引张力是否符合工艺要求。";
            items[8] = "确认膜面质量是否正常。";
            items[9] = "确认现场卫生是否清洁。";
            items[10] = "确认满足设备正常运转的水、电和压缩空气是否正常。";
            items[11] = "确认各部位开关是否正常开启。";
            items[12] = "确认记录填写准确、真实和及时。";
            items[13] = "确认电动叉车运行正常。";


            int x0 = 30, y0 = 150, w = 360, d = 20, margin=55;
            Label num = new Label();
            num.Top = y0 - d;
            num.Left = x0;
            num.Height = d;
            num.Width = 360;
            num.Visible = true;
            num.Text = "序号" + "    " + "确认项目";
            num.Font = new Font("宋体", 12);
            this.Controls.Add(num);

            /*Label[] DN = new Label[TN];
            Label D = new Label();
            D.Top = y0 - d;
            D.Left = 2 * x0 + w;
            D.Height = d;
            D.Width = 2*d;
            D.Visible = true;
            D.Font = new Font("宋体", 12);
            D.Text = "";
            DN[0] = DN[1] = D;
            DN[0].Text = "白班";
            DN[1].Text = "夜班";
            DN[1].Left = DN[0].Left + margin;
            this.Controls.Add(DN[0]);
            this.Controls.Add(DN[1]);*/


            Label[] labels = new Label[quiz];
            CheckBox[,] checkboxes = new CheckBox[quiz,TN];
            for (int r = 0; r < quiz; r++)
            {
                Label lb = new Label();
                //lb.Name = "lbQuiz" + (r + 1).ToString();
                lb.Top = y0 + r * d;
                lb.Left = x0;
                lb.Width = w;
                lb.Height = d;
                lb.Visible = true;
                lb.Text = (r+1).ToString()+"       "+items[r];
                lb.Font = new Font("宋体", 12);
 
                labels[r] = lb;
                this.Controls.Add(lb);
                for (int turn = 0; turn < TN; turn++)
                {
                    CheckBox ckb = new CheckBox();
                    ckb.Name="ckbCheck"+Convert.ToString(turn)+Convert.ToString(turn);
                    ckb.Top = y0 + r * d;
                    ckb.Left = 2 * x0 + lb.Width+turn*margin;
                    ckb.Width = d;
                    ckb.Height = d;
                    ckb.Visible = true;
                    ckb.Checked = true;
                    checkboxes[r, turn] = ckb;
                    this.Controls.Add(ckb);
                    itemcheck[r, turn] = ckb.Checked;
                }
            }
            Panel panQuiz = new Panel();
            panQuiz.Top = y0 - margin;
            panQuiz.Left = x0 - margin;
            panQuiz.Height = y0 + quiz * d + margin;
            panQuiz.Width = 2 * x0 + lbAbnormal.Width + 2 * d + margin;
            panQuiz.Visible = true;
            this.Controls.Add(panQuiz);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            LoginForm check = new LoginForm(conn);
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            check.ShowDialog();

        }
        
        

        

    }
}

