using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Extruction.Chart
{
    public partial class beeholetable : mySystem.BaseForm
    {
        public Button btAdd;
        public Button btSel;
        public Button btChk;
        public Label lbTitle;
        public ComboBox functions;
        public int filltop, fillleft, titletop, titleleft, btntop, btnleft;

        DataTable title = new DataTable();
        DataTable record1 = new DataTable();
        DataTable modelhead = new DataTable();
        DataTable creamout = new DataTable();
        DataTable parameters = new DataTable();
        DataGridView dgvTitle = new DataGridView();
        DataGridView dgvRecord1 = new DataGridView();
        DataGridView dgvModelhead = new DataGridView();
        DataGridView dgvCreamout = new DataGridView();
        DataGridView dgvParameters = new DataGridView();

        DateTimePicker dtp = new DateTimePicker();
        public beeholetable(mySystem.MainForm mainform)
            : base(mainform)
        {
            filltop = 150;
            titletop = 80;
            titleleft = 250;
            InitializeComponent();
            
            addLayout();
            fillTitle();
            fillRecord1();
            fillModelhead();
            fillCreamout();
            fillParameters();
            btAdd.Visible = false;
            btSel.Visible = false;
            btChk.Visible = false;
            functions.Visible = false;
        }
        private void fillTitle()
        {
            title.Columns.Add("产品代码", typeof(string));
            title.Columns.Add("批号", typeof(string));
            title.Columns.Add("生产日期", typeof(DateTime));
            title.Columns.Add("记录日期", typeof(DateTime));
            title.Columns.Add("记录人", typeof(string));
            title.Columns.Add("复核人", typeof(string));
            title.Rows.Add("产品代码1", "批号1", dtp.Value, dtp.Value, "记录人1", "复核人1");
            //this.dataGridView1.Width = 800;
            dgvTitle.Location = new System.Drawing.Point(30, filltop);
            dgvTitle.Size = new System.Drawing.Size(800, dgvTitle.RowTemplate.Height * 2);
            dgvTitle.AllowUserToAddRows = false;
            dgvTitle.RowHeadersVisible = false;
            dgvTitle.DataSource = title;
            this.Controls.Add(dgvTitle);
        }
        private void fillRecord1()
        {
            record1.Columns.Add("实际温度(℃)", typeof(string));
            record1.Columns.Add("一区", typeof(Single));
            record1.Columns.Add("二区", typeof(Single));
            record1.Columns.Add("三区", typeof(Single));
            record1.Columns.Add("四区", typeof(Single));
            record1.Columns.Add("换网", typeof(Single));
            record1.Columns.Add("流道", typeof(Single));
            record1.Rows.Add("A层", 0, 0, 0, 0, 0, 0);
            record1.Rows.Add("B层", 0, 0, 0, 0, 0, 0);
            record1.Rows.Add("C层", 0, 0, 0, 0, 0, 0);
            dgvRecord1.AllowUserToAddRows = false;
            dgvRecord1.RowHeadersVisible = false;
            dgvRecord1.Location = new System.Drawing.Point(30, filltop + dgvTitle.Height);
            dgvRecord1.Size = new System.Drawing.Size(800, dgvRecord1.RowTemplate.Height * 4);
            dgvRecord1.DataSource = record1;
            this.Controls.Add(dgvRecord1);
        }
        private void fillModelhead()
        {
            modelhead.Columns.Add("模头", typeof(string));
            modelhead.Columns.Add("模颈", typeof(Single));
            modelhead.Columns.Add("一区", typeof(Single));
            modelhead.Columns.Add("二区", typeof(Single));
            modelhead.Columns.Add("口模", typeof(Single));
            modelhead.Columns.Add("线速度", typeof(Single));
            modelhead.Rows.Add("", 0, 0, 0, 0, 0);
            dgvModelhead.AllowUserToAddRows = false;
            dgvModelhead.RowHeadersVisible = false;
            dgvModelhead.Location = new System.Drawing.Point(30, filltop + dgvTitle.Height + dgvRecord1.Height);
            dgvModelhead.Size = new System.Drawing.Size(800, dgvModelhead.RowTemplate.Height * 2);
            dgvModelhead.DataSource = modelhead;
            this.Controls.Add(dgvModelhead);
        }
        private void fillCreamout()
        {
            creamout.Columns.Add("挤出机", typeof(string));
            creamout.Columns.Add("实际频率", typeof(Single));
            creamout.Columns.Add("电流", typeof(Single));
            creamout.Columns.Add("熔体温度", typeof(Single));
            creamout.Columns.Add("前熔体", typeof(Single));
            creamout.Columns.Add("后熔压", typeof(Single));
            creamout.Columns.Add("螺杆转速", typeof(Single));
            creamout.Rows.Add("A层", 0, 0, 0, 0, 0, 0);
            creamout.Rows.Add("B层", 0, 0, 0, 0, 0, 0);
            creamout.Rows.Add("C层", 0, 0, 0, 0, 0, 0);
            dgvCreamout.AllowUserToAddRows = false;
            dgvCreamout.RowHeadersVisible = false;
            dgvCreamout.Location = new System.Drawing.Point(30, filltop + dgvTitle.Height + dgvRecord1.Height + dgvModelhead.Height);
            dgvCreamout.Size = new System.Drawing.Size(800, dgvCreamout.RowTemplate.Height * 4);
            dgvCreamout.DataSource = creamout;
            this.Controls.Add(dgvCreamout);
        }
        private void fillParameters()
        {
            parameters.Columns.Add("参数记录", typeof(string));
            parameters.Columns.Add("设置频率", typeof(Single));
            parameters.Columns.Add("实际频率", typeof(Single));
            parameters.Columns.Add("设定张力", typeof(Single));
            parameters.Columns.Add("实际张力", typeof(Single));
            parameters.Columns.Add("电流", typeof(Single));
            parameters.Columns.Add("转矩", typeof(Single));
            parameters.Rows.Add("第一牵引", null,null,null,null,null,null);
            parameters.Rows.Add("第二牵引", null, null, null, null, null, null);
            parameters.Rows.Add("外表面电机", null, null, null, null, null, null);
            parameters.Rows.Add("外冷进风机", null, null, null, null, null, null);
            parameters.Rows.Add("内冷排风机", null, null, null, null, null, null);
            parameters.Rows.Add("内冷进风机", null, null, null, null, null, null);
            parameters.Rows.Add("外中心电机", null, null, null, null, null, null);
            parameters.Rows.Add("内表面电机", null, null, null, null, null, null);
            parameters.Rows.Add("内中心电机", null, null, null, null, null, null);
            dgvParameters.AllowUserToAddRows = false;
            dgvParameters.RowHeadersVisible = false;
            dgvParameters.Location = new System.Drawing.Point(30, filltop + dgvTitle.Height + dgvRecord1.Height + dgvModelhead.Height + dgvCreamout.Height);
            dgvParameters.Size = new System.Drawing.Size(800, dgvParameters.RowTemplate.Height * 10);
            dgvParameters.DataSource = parameters;
            this.Controls.Add(dgvParameters);
        }
        private void addLayout()
        {
            btAdd = new Button();
            btAdd.Location = new System.Drawing.Point(btnleft, btntop);
            btAdd.Name = "btnAdd";
            btAdd.Size = new System.Drawing.Size(75, 23);
            btAdd.TabIndex = 28;
            btAdd.Text = "添加";
            btAdd.UseVisualStyleBackColor = true;
            this.Controls.Add(btAdd);
            //btAdd.Click += new EventHandler(btAdd_Click);

            btSel = new Button();
            btSel.Location = new System.Drawing.Point(btnleft + btAdd.Width, btntop);
            btSel.Size = new System.Drawing.Size(75, 23);
            btSel.Text = "查询";
            this.Controls.Add(btSel);
            //btSel.Click += new EventHandler(btSel_Click);

            btChk = new Button();
            btChk.Location = new System.Drawing.Point(btnleft + 2 * btAdd.Width, btntop);
            btChk.Size = new System.Drawing.Size(75, 23);
            btChk.Text = "审核";
            this.Controls.Add(btChk);
            //btChk.Click += new EventHandler(btChk_Click);

            functions = new ComboBox();
            functions.Location = new System.Drawing.Point(50, btntop);
            functions.Items.Add("添加");
            functions.Items.Add("查询");
            functions.SelectedIndex = 1;
            functions.Font = new System.Drawing.Font("宋体", 12);
            functions.Size = new System.Drawing.Size(80, 23);
            this.Controls.Add(functions);


            lbTitle = new Label();
            this.lbTitle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.Location = new System.Drawing.Point(titleleft, titletop);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTitle.Name = "lbTitle";
            //this.lbTitle.Size = new System.Drawing.Size(169, 19);
            this.lbTitle.AutoSize = true;
            this.lbTitle.TabIndex = 20;
            this.lbTitle.Text = "吹膜机组统运行记录";
            this.lbTitle.Visible = true;
            this.Controls.Add(lbTitle);
            this.Size = new System.Drawing.Size(900, 700);
        }


        




    }
}
