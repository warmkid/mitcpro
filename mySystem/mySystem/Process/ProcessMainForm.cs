using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using mySystem.Process.Bag;
using mySystem.Process.CleanCut;
using mySystem.Process.Bag.LDPE;
using mySystem.Process.Bag.PTV;
using mySystem.Process.Bag.BTV;
using 订单和库存管理;
using mySystem.Process.Stock;
using mySystem.Process.灭菌;

namespace mySystem
{
    public partial class ProcessMainForm : BaseForm
    {
        public ExtructionMainForm extruform = null;
        CleanCutMainForm cleancutform = null;
        CSBagMainForm csbagform = null;
        LDPEMainForm ldpebagform = null;
        PTVMainForm ptvbagform = null;
        BTVMainForm btvbagform = null;

        public ProcessMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            RoleInit();
        }

        private void RoleInit()
        {
            switch (Parameter.userRole)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                default:
                    break;
            }
        }


        bool checkRight(String tblName="用户")
        {
            String sql = "select * from {0} where 用户名='{1}'";
            SqlDataAdapter da = new SqlDataAdapter(String.Format(sql, tblName, mySystem.Parameter.userName), mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                
                return false;
            }
            return true;
        }

        //吹膜
        private void ExtructionBtn_Click(object sender, EventArgs e)
        {
           
            Parameter.selectCon = 1;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--


            checkFlight(); //获取用户班次
            BtnColor();
            Btn吹膜.BackColor = Color.FromArgb(138, 158, 196);            
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();

            extruform = new ExtructionMainForm(mainform);
            Parameter.parentExtru = extruform;      
            extruform.TopLevel = false;
            extruform.FormBorderStyle = FormBorderStyle.None;
            extruform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(extruform);
            extruform.Show();
        }

        //清洁分切
        private void CleanBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 2;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--

            checkFlight();
            BtnColor();
            Btn清洁分切.BackColor = Color.FromArgb(138, 158, 196);
            
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();        

            cleancutform = new CleanCutMainForm(mainform);         
            cleancutform.TopLevel = false;
            cleancutform.FormBorderStyle = FormBorderStyle.None;
            cleancutform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(cleancutform);
            cleancutform.Show();
            Parameter.parentClean = cleancutform;

        }

        //制袋
        private void BagBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn制袋.BackColor = Color.FromArgb(138, 158, 196);
            
            if (Panel制袋.Visible == true)
            {
                Panel其他按钮.Location = new Point(0, 130);
                Panel制袋.Visible = false;
            }
            else
            {
                Panel其他按钮.Location = new Point(0, 361);
                Panel制袋.Visible = true;
            }          

        }

        //CS制袋
        private void CSbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 3;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--


            checkFlight();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            BtnCS制袋.BackColor = Color.FromArgb(138, 158, 196);

            csbagform = new CSBagMainForm(mainform);
            csbagform.TopLevel = false;
            csbagform.FormBorderStyle = FormBorderStyle.None;
            csbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(csbagform);
            csbagform.Show();
            Parameter.parentCS = csbagform;
            
        }

        //LDPE制袋
        private void LDPEbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 7;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--

            checkFlight();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            BtnPE制袋.BackColor = Color.FromArgb(138, 158, 196);          

            ldpebagform = new LDPEMainForm();
            ldpebagform.TopLevel = false;
            ldpebagform.FormBorderStyle = FormBorderStyle.None;
            ldpebagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(ldpebagform);
            ldpebagform.Show();
            Parameter.parentLDPE = ldpebagform;
        }

        //连续袋
        private void bag3Btn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn连续袋.BackColor = Color.FromArgb(138, 158, 196);
            
        }

        //PTV制袋
        private void PTVbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 8;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--

            checkFlight();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            BtnPTV制袋.BackColor = Color.FromArgb(138, 158, 196);

            ptvbagform = new PTVMainForm(); 
            ptvbagform.TopLevel = false;
            ptvbagform.FormBorderStyle = FormBorderStyle.None;
            ptvbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(ptvbagform);
            ptvbagform.Show();
            Parameter.parentPTV = ptvbagform;
        }

        //BPV制袋
        private void BTVbagBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 6;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--


            checkFlight();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            BtnBPV制袋.BackColor = Color.FromArgb(138, 158, 196);

            btvbagform = new BTVMainForm();
            btvbagform.TopLevel = false;
            btvbagform.FormBorderStyle = FormBorderStyle.None;
            btvbagform.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(btvbagform);
            btvbagform.Show();
            Parameter.parentBPV = btvbagform;

        }

        //防护罩
        private void bag6Btn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn防护罩.BackColor = Color.FromArgb(138, 158, 196);
        }

        //灭菌
        private void KillBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 5;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight())
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--


            checkFlight();
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn灭菌.BackColor = Color.FromArgb(138, 158, 196);           

            灭菌mainform myDlg = new 灭菌mainform();
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //生产计划
        private void PlanBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn生产指令.BackColor = Color.FromArgb(138, 158, 196);
        }

        //订单管理
        private void OrderBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();

            //--无权限不能打开
            if (!checkRight("订单用户"))
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--


            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn订单管理.BackColor = Color.FromArgb(138, 158, 196);

            订单管理 myDlg = new 订单管理(mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        //库存管理
        private void StockBtn_Click(object sender, EventArgs e)
        {
            Parameter.selectCon = 4;
            Parameter.InitCon();


            //--无权限不能打开
            if (!checkRight("库存用户"))
            {
                MessageBox.Show("没有权限！");
                return;
            }
            //--

            foreach (Control control in ProducePanelRight.Controls)
            { control.Dispose(); }
            ProducePanelRight.Controls.Clear();
            BtnColor();
            Btn库存管理.BackColor = Color.FromArgb(138, 158, 196);

            库存管理 myDlg = new 库存管理(mainform);
            //库存管理主界面 myDlg = new 库存管理主界面(mainform);
            myDlg.TopLevel = false;
            myDlg.FormBorderStyle = FormBorderStyle.None;
            myDlg.Size = ProducePanelRight.Size;
            ProducePanelRight.Controls.Add(myDlg);
            myDlg.Show();
        }

        

        //按钮背景色不高亮
        private void BtnColor()
        {
            Btn吹膜.BackColor = Color.FromName("ControlLightLight");
            Btn清洁分切.BackColor = Color.FromName("ControlLightLight");
            Btn制袋.BackColor = Color.FromName("ControlLightLight");
            Btn灭菌.BackColor = Color.FromName("ControlLightLight");
            Btn生产指令.BackColor = Color.FromName("ControlLightLight");
            Btn订单管理.BackColor = Color.FromName("ControlLightLight");
            Btn库存管理.BackColor = Color.FromName("ControlLightLight");
            BtnCS制袋.BackColor = Color.FromName("ControlLightLight");
            BtnPE制袋.BackColor = Color.FromName("ControlLightLight");
            Btn连续袋.BackColor = Color.FromName("ControlLightLight");
            BtnPTV制袋.BackColor = Color.FromName("ControlLightLight");
            BtnBPV制袋.BackColor = Color.FromName("ControlLightLight");
            Btn防护罩.BackColor = Color.FromName("ControlLightLight");
        }

        //班次查询
        private void checkFlight()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "SELECT * FROM 用户 WHERE 用户名= " + "'" + Parameter.userName + "'";
                OleDbDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                    Parameter.userflight = reader["班次"].ToString();

                reader.Dispose();
                comm.Dispose();
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "SELECT * FROM 用户 WHERE 用户名= " + "'" + Parameter.userName + "'";
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                    Parameter.userflight = reader["班次"].ToString();

                reader.Dispose();
                comm.Dispose();
            }

        }

        
    }
}
