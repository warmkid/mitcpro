namespace 订单和库存管理
{
    partial class 订单管理
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage销售订单 = new System.Windows.Forms.TabPage();
            this.btn添加销售订单 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn查询销售订单 = new System.Windows.Forms.Button();
            this.cmb销售订单审核状态 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb销售订单号 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp销售订单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp销售订单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.dgv销售订单 = new System.Windows.Forms.DataGridView();
            this.tabPage采购需求单 = new System.Windows.Forms.TabPage();
            this.dgv采购需求单 = new System.Windows.Forms.DataGridView();
            this.btn添加采购需求单 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn查询采购需求单 = new System.Windows.Forms.Button();
            this.cmb采购需求单审核状态 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb用途 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtp采购需求单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp采购需求单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.tabPage采购批准单 = new System.Windows.Forms.TabPage();
            this.dgv采购批准单 = new System.Windows.Forms.DataGridView();
            this.btn采购批准单添加 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btn采购批准单查询 = new System.Windows.Forms.Button();
            this.cmb采购批准单审核状态 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtp采购批准单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp采购批准单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.tabPage采购订单 = new System.Windows.Forms.TabPage();
            this.dgv采购订单 = new System.Windows.Forms.DataGridView();
            this.btn采购订单添加 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btn采购订单查询 = new System.Windows.Forms.Button();
            this.cmb采购订单审核状态 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb采购合同号 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dtp采购订单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp采购订单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.tabPage出库单 = new System.Windows.Forms.TabPage();
            this.dgv出库单 = new System.Windows.Forms.DataGridView();
            this.btn出库单添加 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btn出库单查询 = new System.Windows.Forms.Button();
            this.cmb出库单审核状态 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb出库单销售订单 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dtp出库单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp出库单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.tabControl1.SuspendLayout();
            this.tabPage销售订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).BeginInit();
            this.tabPage采购需求单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购需求单)).BeginInit();
            this.tabPage采购批准单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购批准单)).BeginInit();
            this.tabPage采购订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购订单)).BeginInit();
            this.tabPage出库单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv出库单)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage销售订单);
            this.tabControl1.Controls.Add(this.tabPage采购需求单);
            this.tabControl1.Controls.Add(this.tabPage采购批准单);
            this.tabControl1.Controls.Add(this.tabPage采购订单);
            this.tabControl1.Controls.Add(this.tabPage出库单);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1072, 513);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage销售订单
            // 
            this.tabPage销售订单.Controls.Add(this.btn添加销售订单);
            this.tabPage销售订单.Controls.Add(this.label3);
            this.tabPage销售订单.Controls.Add(this.btn查询销售订单);
            this.tabPage销售订单.Controls.Add(this.cmb销售订单审核状态);
            this.tabPage销售订单.Controls.Add(this.label2);
            this.tabPage销售订单.Controls.Add(this.tb销售订单号);
            this.tabPage销售订单.Controls.Add(this.label1);
            this.tabPage销售订单.Controls.Add(this.dtp销售订单结束时间);
            this.tabPage销售订单.Controls.Add(this.dtp销售订单开始时间);
            this.tabPage销售订单.Controls.Add(this.dgv销售订单);
            this.tabPage销售订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage销售订单.Name = "tabPage销售订单";
            this.tabPage销售订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage销售订单.Size = new System.Drawing.Size(1064, 480);
            this.tabPage销售订单.TabIndex = 1;
            this.tabPage销售订单.Text = "销售订单";
            this.tabPage销售订单.UseVisualStyleBackColor = true;
            // 
            // btn添加销售订单
            // 
            this.btn添加销售订单.Location = new System.Drawing.Point(17, 72);
            this.btn添加销售订单.Name = "btn添加销售订单";
            this.btn添加销售订单.Size = new System.Drawing.Size(75, 23);
            this.btn添加销售订单.TabIndex = 9;
            this.btn添加销售订单.Text = "添加";
            this.btn添加销售订单.UseVisualStyleBackColor = true;
            this.btn添加销售订单.Click += new System.EventHandler(this.btn添加销售订单_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "---";
            // 
            // btn查询销售订单
            // 
            this.btn查询销售订单.Location = new System.Drawing.Point(969, 72);
            this.btn查询销售订单.Name = "btn查询销售订单";
            this.btn查询销售订单.Size = new System.Drawing.Size(75, 23);
            this.btn查询销售订单.TabIndex = 7;
            this.btn查询销售订单.Text = "查询";
            this.btn查询销售订单.UseVisualStyleBackColor = true;
            this.btn查询销售订单.Click += new System.EventHandler(this.btn查询销售订单_Click);
            // 
            // cmb销售订单审核状态
            // 
            this.cmb销售订单审核状态.FormattingEnabled = true;
            this.cmb销售订单审核状态.Items.AddRange(new object[] {
            "编辑中",
            "待审核",
            "审核完成",
            "已生成采购需求单",
            "已关闭",
            "已取消"});
            this.cmb销售订单审核状态.Location = new System.Drawing.Point(923, 21);
            this.cmb销售订单审核状态.Name = "cmb销售订单审核状态";
            this.cmb销售订单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb销售订单审核状态.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(841, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "审核状态";
            // 
            // tb销售订单号
            // 
            this.tb销售订单号.Location = new System.Drawing.Point(647, 21);
            this.tb销售订单号.Name = "tb销售订单号";
            this.tb销售订单号.Size = new System.Drawing.Size(160, 26);
            this.tb销售订单号.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(572, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "订单号";
            // 
            // dtp销售订单结束时间
            // 
            this.dtp销售订单结束时间.Location = new System.Drawing.Point(294, 21);
            this.dtp销售订单结束时间.Name = "dtp销售订单结束时间";
            this.dtp销售订单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp销售订单结束时间.TabIndex = 2;
            // 
            // dtp销售订单开始时间
            // 
            this.dtp销售订单开始时间.Location = new System.Drawing.Point(17, 21);
            this.dtp销售订单开始时间.Name = "dtp销售订单开始时间";
            this.dtp销售订单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp销售订单开始时间.TabIndex = 1;
            // 
            // dgv销售订单
            // 
            this.dgv销售订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv销售订单.Location = new System.Drawing.Point(17, 111);
            this.dgv销售订单.Name = "dgv销售订单";
            this.dgv销售订单.RowTemplate.Height = 23;
            this.dgv销售订单.Size = new System.Drawing.Size(1041, 350);
            this.dgv销售订单.TabIndex = 0;
            // 
            // tabPage采购需求单
            // 
            this.tabPage采购需求单.Controls.Add(this.dgv采购需求单);
            this.tabPage采购需求单.Controls.Add(this.btn添加采购需求单);
            this.tabPage采购需求单.Controls.Add(this.label4);
            this.tabPage采购需求单.Controls.Add(this.btn查询采购需求单);
            this.tabPage采购需求单.Controls.Add(this.cmb采购需求单审核状态);
            this.tabPage采购需求单.Controls.Add(this.label5);
            this.tabPage采购需求单.Controls.Add(this.tb用途);
            this.tabPage采购需求单.Controls.Add(this.label6);
            this.tabPage采购需求单.Controls.Add(this.dtp采购需求单结束时间);
            this.tabPage采购需求单.Controls.Add(this.dtp采购需求单开始时间);
            this.tabPage采购需求单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购需求单.Name = "tabPage采购需求单";
            this.tabPage采购需求单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购需求单.Size = new System.Drawing.Size(1064, 480);
            this.tabPage采购需求单.TabIndex = 2;
            this.tabPage采购需求单.Text = "采购需求单";
            this.tabPage采购需求单.UseVisualStyleBackColor = true;
            // 
            // dgv采购需求单
            // 
            this.dgv采购需求单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv采购需求单.Location = new System.Drawing.Point(12, 108);
            this.dgv采购需求单.Name = "dgv采购需求单";
            this.dgv采购需求单.RowTemplate.Height = 23;
            this.dgv采购需求单.Size = new System.Drawing.Size(1041, 350);
            this.dgv采购需求单.TabIndex = 19;
            // 
            // btn添加采购需求单
            // 
            this.btn添加采购需求单.Location = new System.Drawing.Point(19, 68);
            this.btn添加采购需求单.Name = "btn添加采购需求单";
            this.btn添加采购需求单.Size = new System.Drawing.Size(75, 23);
            this.btn添加采购需求单.TabIndex = 18;
            this.btn添加采购需求单.Text = "添加";
            this.btn添加采购需求单.UseVisualStyleBackColor = true;
            this.btn添加采购需求单.Click += new System.EventHandler(this.btn添加采购需求单_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(235, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "---";
            // 
            // btn查询采购需求单
            // 
            this.btn查询采购需求单.Location = new System.Drawing.Point(971, 68);
            this.btn查询采购需求单.Name = "btn查询采购需求单";
            this.btn查询采购需求单.Size = new System.Drawing.Size(75, 23);
            this.btn查询采购需求单.TabIndex = 16;
            this.btn查询采购需求单.Text = "查询";
            this.btn查询采购需求单.UseVisualStyleBackColor = true;
            this.btn查询采购需求单.Click += new System.EventHandler(this.btn查询采购需求单_Click);
            // 
            // cmb采购需求单审核状态
            // 
            this.cmb采购需求单审核状态.FormattingEnabled = true;
            this.cmb采购需求单审核状态.Items.AddRange(new object[] {
            "编辑中",
            "待审核",
            "审核完成"});
            this.cmb采购需求单审核状态.Location = new System.Drawing.Point(925, 17);
            this.cmb采购需求单审核状态.Name = "cmb采购需求单审核状态";
            this.cmb采购需求单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb采购需求单审核状态.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(843, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "审核状态";
            // 
            // tb用途
            // 
            this.tb用途.Location = new System.Drawing.Point(643, 18);
            this.tb用途.Name = "tb用途";
            this.tb用途.Size = new System.Drawing.Size(160, 26);
            this.tb用途.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(574, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "用途";
            // 
            // dtp采购需求单结束时间
            // 
            this.dtp采购需求单结束时间.Location = new System.Drawing.Point(296, 17);
            this.dtp采购需求单结束时间.Name = "dtp采购需求单结束时间";
            this.dtp采购需求单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购需求单结束时间.TabIndex = 11;
            // 
            // dtp采购需求单开始时间
            // 
            this.dtp采购需求单开始时间.Location = new System.Drawing.Point(19, 17);
            this.dtp采购需求单开始时间.Name = "dtp采购需求单开始时间";
            this.dtp采购需求单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购需求单开始时间.TabIndex = 10;
            // 
            // tabPage采购批准单
            // 
            this.tabPage采购批准单.Controls.Add(this.dgv采购批准单);
            this.tabPage采购批准单.Controls.Add(this.btn采购批准单添加);
            this.tabPage采购批准单.Controls.Add(this.label7);
            this.tabPage采购批准单.Controls.Add(this.btn采购批准单查询);
            this.tabPage采购批准单.Controls.Add(this.cmb采购批准单审核状态);
            this.tabPage采购批准单.Controls.Add(this.label8);
            this.tabPage采购批准单.Controls.Add(this.textBox1);
            this.tabPage采购批准单.Controls.Add(this.label9);
            this.tabPage采购批准单.Controls.Add(this.dtp采购批准单结束时间);
            this.tabPage采购批准单.Controls.Add(this.dtp采购批准单开始时间);
            this.tabPage采购批准单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购批准单.Name = "tabPage采购批准单";
            this.tabPage采购批准单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购批准单.Size = new System.Drawing.Size(1064, 480);
            this.tabPage采购批准单.TabIndex = 3;
            this.tabPage采购批准单.Text = "采购批准单";
            this.tabPage采购批准单.UseVisualStyleBackColor = true;
            // 
            // dgv采购批准单
            // 
            this.dgv采购批准单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv采购批准单.Location = new System.Drawing.Point(13, 99);
            this.dgv采购批准单.Name = "dgv采购批准单";
            this.dgv采购批准单.RowTemplate.Height = 23;
            this.dgv采购批准单.Size = new System.Drawing.Size(1041, 350);
            this.dgv采购批准单.TabIndex = 28;
            // 
            // btn采购批准单添加
            // 
            this.btn采购批准单添加.Location = new System.Drawing.Point(13, 65);
            this.btn采购批准单添加.Name = "btn采购批准单添加";
            this.btn采购批准单添加.Size = new System.Drawing.Size(75, 23);
            this.btn采购批准单添加.TabIndex = 27;
            this.btn采购批准单添加.Text = "添加";
            this.btn采购批准单添加.UseVisualStyleBackColor = true;
            this.btn采购批准单添加.Click += new System.EventHandler(this.btn采购批准单添加_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 16);
            this.label7.TabIndex = 26;
            this.label7.Text = "---";
            // 
            // btn采购批准单查询
            // 
            this.btn采购批准单查询.Location = new System.Drawing.Point(965, 65);
            this.btn采购批准单查询.Name = "btn采购批准单查询";
            this.btn采购批准单查询.Size = new System.Drawing.Size(75, 23);
            this.btn采购批准单查询.TabIndex = 25;
            this.btn采购批准单查询.Text = "查询";
            this.btn采购批准单查询.UseVisualStyleBackColor = true;
            this.btn采购批准单查询.Click += new System.EventHandler(this.btn采购批准单查询_Click);
            // 
            // cmb采购批准单审核状态
            // 
            this.cmb采购批准单审核状态.FormattingEnabled = true;
            this.cmb采购批准单审核状态.Items.AddRange(new object[] {
            "编辑中",
            "待审核",
            "审核完成",
            "已关闭"});
            this.cmb采购批准单审核状态.Location = new System.Drawing.Point(919, 19);
            this.cmb采购批准单审核状态.Name = "cmb采购批准单审核状态";
            this.cmb采购批准单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb采购批准单审核状态.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(837, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "审核状态";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(637, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 26);
            this.textBox1.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(568, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "？？";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtp采购批准单结束时间
            // 
            this.dtp采购批准单结束时间.Location = new System.Drawing.Point(290, 19);
            this.dtp采购批准单结束时间.Name = "dtp采购批准单结束时间";
            this.dtp采购批准单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购批准单结束时间.TabIndex = 20;
            // 
            // dtp采购批准单开始时间
            // 
            this.dtp采购批准单开始时间.Location = new System.Drawing.Point(13, 19);
            this.dtp采购批准单开始时间.Name = "dtp采购批准单开始时间";
            this.dtp采购批准单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购批准单开始时间.TabIndex = 19;
            // 
            // tabPage采购订单
            // 
            this.tabPage采购订单.Controls.Add(this.dgv采购订单);
            this.tabPage采购订单.Controls.Add(this.btn采购订单添加);
            this.tabPage采购订单.Controls.Add(this.label10);
            this.tabPage采购订单.Controls.Add(this.btn采购订单查询);
            this.tabPage采购订单.Controls.Add(this.cmb采购订单审核状态);
            this.tabPage采购订单.Controls.Add(this.label11);
            this.tabPage采购订单.Controls.Add(this.tb采购合同号);
            this.tabPage采购订单.Controls.Add(this.label12);
            this.tabPage采购订单.Controls.Add(this.dtp采购订单结束时间);
            this.tabPage采购订单.Controls.Add(this.dtp采购订单开始时间);
            this.tabPage采购订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购订单.Name = "tabPage采购订单";
            this.tabPage采购订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购订单.Size = new System.Drawing.Size(1064, 480);
            this.tabPage采购订单.TabIndex = 4;
            this.tabPage采购订单.Text = "采购订单";
            this.tabPage采购订单.UseVisualStyleBackColor = true;
            // 
            // dgv采购订单
            // 
            this.dgv采购订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv采购订单.Location = new System.Drawing.Point(8, 100);
            this.dgv采购订单.Name = "dgv采购订单";
            this.dgv采购订单.RowTemplate.Height = 23;
            this.dgv采购订单.Size = new System.Drawing.Size(1041, 350);
            this.dgv采购订单.TabIndex = 37;
            // 
            // btn采购订单添加
            // 
            this.btn采购订单添加.Location = new System.Drawing.Point(22, 71);
            this.btn采购订单添加.Name = "btn采购订单添加";
            this.btn采购订单添加.Size = new System.Drawing.Size(75, 23);
            this.btn采购订单添加.TabIndex = 36;
            this.btn采购订单添加.Text = "添加";
            this.btn采购订单添加.UseVisualStyleBackColor = true;
            this.btn采购订单添加.Click += new System.EventHandler(this.btn采购订单添加_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(238, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 16);
            this.label10.TabIndex = 35;
            this.label10.Text = "---";
            // 
            // btn采购订单查询
            // 
            this.btn采购订单查询.Location = new System.Drawing.Point(974, 71);
            this.btn采购订单查询.Name = "btn采购订单查询";
            this.btn采购订单查询.Size = new System.Drawing.Size(75, 23);
            this.btn采购订单查询.TabIndex = 34;
            this.btn采购订单查询.Text = "查询";
            this.btn采购订单查询.UseVisualStyleBackColor = true;
            this.btn采购订单查询.Click += new System.EventHandler(this.btn采购订单审核状态_Click);
            // 
            // cmb采购订单审核状态
            // 
            this.cmb采购订单审核状态.FormattingEnabled = true;
            this.cmb采购订单审核状态.Items.AddRange(new object[] {
            "编辑中",
            "待审核",
            "审核完成",
            "已关闭"});
            this.cmb采购订单审核状态.Location = new System.Drawing.Point(928, 20);
            this.cmb采购订单审核状态.Name = "cmb采购订单审核状态";
            this.cmb采购订单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb采购订单审核状态.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(846, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 32;
            this.label11.Text = "审核状态";
            // 
            // tb采购合同号
            // 
            this.tb采购合同号.Location = new System.Drawing.Point(646, 21);
            this.tb采购合同号.Name = "tb采购合同号";
            this.tb采购合同号.Size = new System.Drawing.Size(160, 26);
            this.tb采购合同号.TabIndex = 31;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(555, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 16);
            this.label12.TabIndex = 30;
            this.label12.Text = "采购合同号";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtp采购订单结束时间
            // 
            this.dtp采购订单结束时间.Location = new System.Drawing.Point(299, 20);
            this.dtp采购订单结束时间.Name = "dtp采购订单结束时间";
            this.dtp采购订单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购订单结束时间.TabIndex = 29;
            // 
            // dtp采购订单开始时间
            // 
            this.dtp采购订单开始时间.Location = new System.Drawing.Point(22, 20);
            this.dtp采购订单开始时间.Name = "dtp采购订单开始时间";
            this.dtp采购订单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp采购订单开始时间.TabIndex = 28;
            // 
            // tabPage出库单
            // 
            this.tabPage出库单.Controls.Add(this.dgv出库单);
            this.tabPage出库单.Controls.Add(this.btn出库单添加);
            this.tabPage出库单.Controls.Add(this.label13);
            this.tabPage出库单.Controls.Add(this.btn出库单查询);
            this.tabPage出库单.Controls.Add(this.cmb出库单审核状态);
            this.tabPage出库单.Controls.Add(this.label14);
            this.tabPage出库单.Controls.Add(this.tb出库单销售订单);
            this.tabPage出库单.Controls.Add(this.label15);
            this.tabPage出库单.Controls.Add(this.dtp出库单结束时间);
            this.tabPage出库单.Controls.Add(this.dtp出库单开始时间);
            this.tabPage出库单.Location = new System.Drawing.Point(4, 29);
            this.tabPage出库单.Name = "tabPage出库单";
            this.tabPage出库单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage出库单.Size = new System.Drawing.Size(1064, 480);
            this.tabPage出库单.TabIndex = 5;
            this.tabPage出库单.Text = "出库单";
            this.tabPage出库单.UseVisualStyleBackColor = true;
            // 
            // dgv出库单
            // 
            this.dgv出库单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv出库单.Location = new System.Drawing.Point(13, 124);
            this.dgv出库单.Name = "dgv出库单";
            this.dgv出库单.RowTemplate.Height = 23;
            this.dgv出库单.Size = new System.Drawing.Size(1041, 350);
            this.dgv出库单.TabIndex = 46;
            // 
            // btn出库单添加
            // 
            this.btn出库单添加.Location = new System.Drawing.Point(13, 62);
            this.btn出库单添加.Name = "btn出库单添加";
            this.btn出库单添加.Size = new System.Drawing.Size(75, 23);
            this.btn出库单添加.TabIndex = 45;
            this.btn出库单添加.Text = "添加";
            this.btn出库单添加.UseVisualStyleBackColor = true;
            this.btn出库单添加.Click += new System.EventHandler(this.btn出库单添加_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(229, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 16);
            this.label13.TabIndex = 44;
            this.label13.Text = "---";
            // 
            // btn出库单查询
            // 
            this.btn出库单查询.Location = new System.Drawing.Point(965, 62);
            this.btn出库单查询.Name = "btn出库单查询";
            this.btn出库单查询.Size = new System.Drawing.Size(75, 23);
            this.btn出库单查询.TabIndex = 43;
            this.btn出库单查询.Text = "查询";
            this.btn出库单查询.UseVisualStyleBackColor = true;
            this.btn出库单查询.Click += new System.EventHandler(this.btn出库单查询_Click);
            // 
            // cmb出库单审核状态
            // 
            this.cmb出库单审核状态.FormattingEnabled = true;
            this.cmb出库单审核状态.Items.AddRange(new object[] {
            "编辑中",
            "待审核",
            "审核完成",
            "已关闭"});
            this.cmb出库单审核状态.Location = new System.Drawing.Point(919, 11);
            this.cmb出库单审核状态.Name = "cmb出库单审核状态";
            this.cmb出库单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb出库单审核状态.TabIndex = 42;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(837, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 16);
            this.label14.TabIndex = 41;
            this.label14.Text = "审核状态";
            // 
            // tb出库单销售订单
            // 
            this.tb出库单销售订单.Location = new System.Drawing.Point(637, 12);
            this.tb出库单销售订单.Name = "tb出库单销售订单";
            this.tb出库单销售订单.Size = new System.Drawing.Size(160, 26);
            this.tb出库单销售订单.TabIndex = 40;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(546, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 16);
            this.label15.TabIndex = 39;
            this.label15.Text = "销售订单";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtp出库单结束时间
            // 
            this.dtp出库单结束时间.Location = new System.Drawing.Point(290, 11);
            this.dtp出库单结束时间.Name = "dtp出库单结束时间";
            this.dtp出库单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp出库单结束时间.TabIndex = 38;
            // 
            // dtp出库单开始时间
            // 
            this.dtp出库单开始时间.Location = new System.Drawing.Point(13, 11);
            this.dtp出库单开始时间.Name = "dtp出库单开始时间";
            this.dtp出库单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp出库单开始时间.TabIndex = 37;
            // 
            // 订单管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 537);
            this.Controls.Add(this.tabControl1);
            this.Name = "订单管理";
            this.Text = "订单管理";
            this.tabControl1.ResumeLayout(false);
            this.tabPage销售订单.ResumeLayout(false);
            this.tabPage销售订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).EndInit();
            this.tabPage采购需求单.ResumeLayout(false);
            this.tabPage采购需求单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购需求单)).EndInit();
            this.tabPage采购批准单.ResumeLayout(false);
            this.tabPage采购批准单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购批准单)).EndInit();
            this.tabPage采购订单.ResumeLayout(false);
            this.tabPage采购订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购订单)).EndInit();
            this.tabPage出库单.ResumeLayout(false);
            this.tabPage出库单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv出库单)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage销售订单;
        private System.Windows.Forms.TabPage tabPage采购需求单;
        private System.Windows.Forms.TabPage tabPage采购批准单;
        private System.Windows.Forms.TabPage tabPage采购订单;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn查询销售订单;
        private System.Windows.Forms.ComboBox cmb销售订单审核状态;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb销售订单号;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp销售订单结束时间;
        private System.Windows.Forms.DateTimePicker dtp销售订单开始时间;
        private System.Windows.Forms.DataGridView dgv销售订单;
        private System.Windows.Forms.Button btn添加销售订单;
        private System.Windows.Forms.Button btn添加采购需求单;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn查询采购需求单;
        private System.Windows.Forms.ComboBox cmb采购需求单审核状态;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb用途;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtp采购需求单结束时间;
        private System.Windows.Forms.DateTimePicker dtp采购需求单开始时间;
        private System.Windows.Forms.DataGridView dgv采购需求单;
        private System.Windows.Forms.DataGridView dgv采购批准单;
        private System.Windows.Forms.Button btn采购批准单添加;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn采购批准单查询;
        private System.Windows.Forms.ComboBox cmb采购批准单审核状态;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtp采购批准单结束时间;
        private System.Windows.Forms.DateTimePicker dtp采购批准单开始时间;
        private System.Windows.Forms.Button btn采购订单添加;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn采购订单查询;
        private System.Windows.Forms.ComboBox cmb采购订单审核状态;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb采购合同号;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtp采购订单结束时间;
        private System.Windows.Forms.DateTimePicker dtp采购订单开始时间;
        private System.Windows.Forms.DataGridView dgv采购订单;
        private System.Windows.Forms.TabPage tabPage出库单;
        private System.Windows.Forms.Button btn出库单添加;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn出库单查询;
        private System.Windows.Forms.ComboBox cmb出库单审核状态;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb出库单销售订单;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtp出库单结束时间;
        private System.Windows.Forms.DateTimePicker dtp出库单开始时间;
        private System.Windows.Forms.DataGridView dgv出库单;
    }
}