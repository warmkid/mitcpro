namespace mySystem
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.HelpPage = new System.Windows.Forms.TabPage();
            this.SystemPage = new System.Windows.Forms.TabPage();
            this.SystemtoolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.StorePage = new System.Windows.Forms.TabPage();
            this.StockPanelLeft = new System.Windows.Forms.Panel();
            this.BuyBtn = new System.Windows.Forms.Button();
            this.InOutListBtn = new System.Windows.Forms.Button();
            this.StockOrderBtn = new System.Windows.Forms.Button();
            this.StockCheckBtn = new System.Windows.Forms.Button();
            this.StockPanelRight = new System.Windows.Forms.Panel();
            this.OrderPage = new System.Windows.Forms.TabPage();
            this.OrderdataGrid = new System.Windows.Forms.DataGridView();
            this.订单日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单详情 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单号DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单状态DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.OrderPanelTop = new System.Windows.Forms.Panel();
            this.OrderStateBox = new System.Windows.Forms.ComboBox();
            this.订单状态 = new System.Windows.Forms.Label();
            this.SearchOrderBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.订单号 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.至 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.日期 = new System.Windows.Forms.Label();
            this.OrdertoolStrip = new System.Windows.Forms.ToolStrip();
            this.AddOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.EditOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.DeleteOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.SortOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExcelOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.PrintOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.RefreshOrderBtn = new System.Windows.Forms.ToolStripButton();
            this.订单管理 = new System.Windows.Forms.TabControl();
            this.ProducePage = new System.Windows.Forms.TabPage();
            this.ProducePanelRight = new System.Windows.Forms.Panel();
            this.ProducePanelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.BlowBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PlanBtn = new System.Windows.Forms.Button();
            this.SystemPage.SuspendLayout();
            this.SystemtoolStrip.SuspendLayout();
            this.StorePage.SuspendLayout();
            this.StockPanelLeft.SuspendLayout();
            this.OrderPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrderdataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.订单信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            this.OrderPanelTop.SuspendLayout();
            this.OrdertoolStrip.SuspendLayout();
            this.订单管理.SuspendLayout();
            this.ProducePage.SuspendLayout();
            this.ProducePanelLeft.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HelpPage
            // 
            this.HelpPage.Location = new System.Drawing.Point(4, 22);
            this.HelpPage.Name = "HelpPage";
            this.HelpPage.Padding = new System.Windows.Forms.Padding(3);
            this.HelpPage.Size = new System.Drawing.Size(692, 470);
            this.HelpPage.TabIndex = 5;
            this.HelpPage.Text = "帮助";
            this.HelpPage.UseVisualStyleBackColor = true;
            // 
            // SystemPage
            // 
            this.SystemPage.Controls.Add(this.SystemtoolStrip);
            this.SystemPage.Location = new System.Drawing.Point(4, 22);
            this.SystemPage.Name = "SystemPage";
            this.SystemPage.Padding = new System.Windows.Forms.Padding(3);
            this.SystemPage.Size = new System.Drawing.Size(692, 470);
            this.SystemPage.TabIndex = 4;
            this.SystemPage.Text = "系统设置";
            this.SystemPage.UseVisualStyleBackColor = true;
            // 
            // SystemtoolStrip
            // 
            this.SystemtoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.SystemtoolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.SystemtoolStrip.Location = new System.Drawing.Point(3, 3);
            this.SystemtoolStrip.Name = "SystemtoolStrip";
            this.SystemtoolStrip.Size = new System.Drawing.Size(686, 23);
            this.SystemtoolStrip.TabIndex = 0;
            this.SystemtoolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(73, 20);
            this.toolStripButton1.Text = "系统设置";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(73, 20);
            this.toolStripButton2.Text = "用户管理";
            // 
            // StorePage
            // 
            this.StorePage.Controls.Add(this.StockPanelLeft);
            this.StorePage.Controls.Add(this.StockPanelRight);
            this.StorePage.Location = new System.Drawing.Point(4, 22);
            this.StorePage.Name = "StorePage";
            this.StorePage.Padding = new System.Windows.Forms.Padding(3);
            this.StorePage.Size = new System.Drawing.Size(692, 470);
            this.StorePage.TabIndex = 1;
            this.StorePage.Text = "库存管理";
            this.StorePage.UseVisualStyleBackColor = true;
            // 
            // StockPanelLeft
            // 
            this.StockPanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StockPanelLeft.Controls.Add(this.BuyBtn);
            this.StockPanelLeft.Controls.Add(this.InOutListBtn);
            this.StockPanelLeft.Controls.Add(this.StockOrderBtn);
            this.StockPanelLeft.Controls.Add(this.StockCheckBtn);
            this.StockPanelLeft.Location = new System.Drawing.Point(5, 4);
            this.StockPanelLeft.Name = "StockPanelLeft";
            this.StockPanelLeft.Size = new System.Drawing.Size(159, 464);
            this.StockPanelLeft.TabIndex = 4;
            // 
            // BuyBtn
            // 
            this.BuyBtn.Location = new System.Drawing.Point(3, 109);
            this.BuyBtn.Name = "BuyBtn";
            this.BuyBtn.Size = new System.Drawing.Size(153, 34);
            this.BuyBtn.TabIndex = 5;
            this.BuyBtn.Text = "采购计划";
            this.BuyBtn.UseVisualStyleBackColor = true;
            this.BuyBtn.Click += new System.EventHandler(this.BuyBtn_Click);
            // 
            // InOutListBtn
            // 
            this.InOutListBtn.Location = new System.Drawing.Point(3, 74);
            this.InOutListBtn.Name = "InOutListBtn";
            this.InOutListBtn.Size = new System.Drawing.Size(153, 34);
            this.InOutListBtn.TabIndex = 4;
            this.InOutListBtn.Text = "出/入库记录";
            this.InOutListBtn.UseVisualStyleBackColor = true;
            this.InOutListBtn.Click += new System.EventHandler(this.InOutListBtn_Click);
            // 
            // StockOrderBtn
            // 
            this.StockOrderBtn.Location = new System.Drawing.Point(3, 39);
            this.StockOrderBtn.Name = "StockOrderBtn";
            this.StockOrderBtn.Size = new System.Drawing.Size(153, 34);
            this.StockOrderBtn.TabIndex = 3;
            this.StockOrderBtn.Text = "出库计划";
            this.StockOrderBtn.UseVisualStyleBackColor = true;
            this.StockOrderBtn.Click += new System.EventHandler(this.StockOrderBtn_Click);
            // 
            // StockCheckBtn
            // 
            this.StockCheckBtn.Location = new System.Drawing.Point(3, 4);
            this.StockCheckBtn.Name = "StockCheckBtn";
            this.StockCheckBtn.Size = new System.Drawing.Size(153, 34);
            this.StockCheckBtn.TabIndex = 2;
            this.StockCheckBtn.Text = "库存盘点";
            this.StockCheckBtn.UseVisualStyleBackColor = true;
            this.StockCheckBtn.Click += new System.EventHandler(this.StockCheckBtn_Click);
            // 
            // StockPanelRight
            // 
            this.StockPanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StockPanelRight.Location = new System.Drawing.Point(170, 4);
            this.StockPanelRight.Name = "StockPanelRight";
            this.StockPanelRight.Size = new System.Drawing.Size(524, 464);
            this.StockPanelRight.TabIndex = 3;
            // 
            // OrderPage
            // 
            this.OrderPage.Controls.Add(this.OrderdataGrid);
            this.OrderPage.Controls.Add(this.OrderPanelTop);
            this.OrderPage.Controls.Add(this.OrdertoolStrip);
            this.OrderPage.Location = new System.Drawing.Point(4, 22);
            this.OrderPage.Name = "OrderPage";
            this.OrderPage.Padding = new System.Windows.Forms.Padding(3);
            this.OrderPage.Size = new System.Drawing.Size(692, 470);
            this.OrderPage.TabIndex = 0;
            this.OrderPage.Text = "订单管理";
            this.OrderPage.UseVisualStyleBackColor = true;
            // 
            // OrderdataGrid
            // 
            this.OrderdataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OrderdataGrid.AutoGenerateColumns = false;
            this.OrderdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrderdataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.订单日期DataGridViewTextBoxColumn,
            this.订单详情,
            this.订单号DataGridViewTextBoxColumn,
            this.订单状态DataGridViewTextBoxColumn});
            this.OrderdataGrid.DataSource = this.订单信息BindingSource;
            this.OrderdataGrid.Location = new System.Drawing.Point(11, 117);
            this.OrderdataGrid.Name = "OrderdataGrid";
            this.OrderdataGrid.RowTemplate.Height = 23;
            this.OrderdataGrid.Size = new System.Drawing.Size(673, 352);
            this.OrderdataGrid.TabIndex = 3;
            // 
            // 订单日期DataGridViewTextBoxColumn
            // 
            this.订单日期DataGridViewTextBoxColumn.DataPropertyName = "订单日期";
            this.订单日期DataGridViewTextBoxColumn.HeaderText = "订单日期";
            this.订单日期DataGridViewTextBoxColumn.Name = "订单日期DataGridViewTextBoxColumn";
            // 
            // 订单详情
            // 
            this.订单详情.DataPropertyName = "订单详情";
            this.订单详情.HeaderText = "订单详情";
            this.订单详情.Name = "订单详情";
            // 
            // 订单号DataGridViewTextBoxColumn
            // 
            this.订单号DataGridViewTextBoxColumn.DataPropertyName = "订单号";
            this.订单号DataGridViewTextBoxColumn.HeaderText = "订单号";
            this.订单号DataGridViewTextBoxColumn.Name = "订单号DataGridViewTextBoxColumn";
            // 
            // 订单状态DataGridViewTextBoxColumn
            // 
            this.订单状态DataGridViewTextBoxColumn.DataPropertyName = "订单状态";
            this.订单状态DataGridViewTextBoxColumn.HeaderText = "订单状态";
            this.订单状态DataGridViewTextBoxColumn.Name = "订单状态DataGridViewTextBoxColumn";
            // 
            // 订单信息BindingSource
            // 
            this.订单信息BindingSource.DataMember = "订单信息";
            this.订单信息BindingSource.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // OrderPanelTop
            // 
            this.OrderPanelTop.Controls.Add(this.OrderStateBox);
            this.OrderPanelTop.Controls.Add(this.订单状态);
            this.OrderPanelTop.Controls.Add(this.SearchOrderBtn);
            this.OrderPanelTop.Controls.Add(this.textBox1);
            this.OrderPanelTop.Controls.Add(this.订单号);
            this.OrderPanelTop.Controls.Add(this.dateTimePicker2);
            this.OrderPanelTop.Controls.Add(this.至);
            this.OrderPanelTop.Controls.Add(this.dateTimePicker1);
            this.OrderPanelTop.Controls.Add(this.日期);
            this.OrderPanelTop.Location = new System.Drawing.Point(1, 29);
            this.OrderPanelTop.Name = "OrderPanelTop";
            this.OrderPanelTop.Size = new System.Drawing.Size(693, 87);
            this.OrderPanelTop.TabIndex = 2;
            // 
            // OrderStateBox
            // 
            this.OrderStateBox.FormattingEnabled = true;
            this.OrderStateBox.Items.AddRange(new object[] {
            "未完成",
            "已完成"});
            this.OrderStateBox.Location = new System.Drawing.Point(74, 31);
            this.OrderStateBox.Name = "OrderStateBox";
            this.OrderStateBox.Size = new System.Drawing.Size(124, 20);
            this.OrderStateBox.TabIndex = 8;
            // 
            // 订单状态
            // 
            this.订单状态.AutoSize = true;
            this.订单状态.Location = new System.Drawing.Point(8, 35);
            this.订单状态.Name = "订单状态";
            this.订单状态.Size = new System.Drawing.Size(65, 12);
            this.订单状态.TabIndex = 7;
            this.订单状态.Text = "订单状态：";
            // 
            // SearchOrderBtn
            // 
            this.SearchOrderBtn.Location = new System.Drawing.Point(609, 59);
            this.SearchOrderBtn.Name = "SearchOrderBtn";
            this.SearchOrderBtn.Size = new System.Drawing.Size(75, 23);
            this.SearchOrderBtn.TabIndex = 6;
            this.SearchOrderBtn.Text = "查询";
            this.SearchOrderBtn.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(362, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 21);
            this.textBox1.TabIndex = 5;
            // 
            // 订单号
            // 
            this.订单号.AutoSize = true;
            this.订单号.Location = new System.Drawing.Point(310, 8);
            this.订单号.Name = "订单号";
            this.订单号.Size = new System.Drawing.Size(53, 12);
            this.订单号.TabIndex = 4;
            this.订单号.Text = "订单号：";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(181, 5);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(115, 21);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // 至
            // 
            this.至.AutoSize = true;
            this.至.Location = new System.Drawing.Point(162, 9);
            this.至.Name = "至";
            this.至.Size = new System.Drawing.Size(17, 12);
            this.至.TabIndex = 2;
            this.至.Text = "至";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(45, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(115, 21);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            // 
            // 日期
            // 
            this.日期.AutoSize = true;
            this.日期.Location = new System.Drawing.Point(6, 9);
            this.日期.Name = "日期";
            this.日期.Size = new System.Drawing.Size(41, 12);
            this.日期.TabIndex = 0;
            this.日期.Text = "日期：";
            // 
            // OrdertoolStrip
            // 
            this.OrdertoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddOrderBtn,
            this.EditOrderBtn,
            this.DeleteOrderBtn,
            this.SortOrderBtn,
            this.toolStripSeparator1,
            this.ExcelOrderBtn,
            this.PrintOrderBtn,
            this.RefreshOrderBtn});
            this.OrdertoolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.OrdertoolStrip.Location = new System.Drawing.Point(3, 3);
            this.OrdertoolStrip.Name = "OrdertoolStrip";
            this.OrdertoolStrip.Size = new System.Drawing.Size(686, 23);
            this.OrdertoolStrip.TabIndex = 1;
            this.OrdertoolStrip.Text = "toolStrip2";
            // 
            // AddOrderBtn
            // 
            this.AddOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("AddOrderBtn.Image")));
            this.AddOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddOrderBtn.Name = "AddOrderBtn";
            this.AddOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.AddOrderBtn.Text = "添加";
            this.AddOrderBtn.Click += new System.EventHandler(this.AddOrderBtn_Click);
            // 
            // EditOrderBtn
            // 
            this.EditOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditOrderBtn.Image")));
            this.EditOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditOrderBtn.Name = "EditOrderBtn";
            this.EditOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.EditOrderBtn.Text = "编辑";
            // 
            // DeleteOrderBtn
            // 
            this.DeleteOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("DeleteOrderBtn.Image")));
            this.DeleteOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteOrderBtn.Name = "DeleteOrderBtn";
            this.DeleteOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.DeleteOrderBtn.Text = "删除";
            // 
            // SortOrderBtn
            // 
            this.SortOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("SortOrderBtn.Image")));
            this.SortOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SortOrderBtn.Name = "SortOrderBtn";
            this.SortOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.SortOrderBtn.Text = "排序";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // ExcelOrderBtn
            // 
            this.ExcelOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExcelOrderBtn.Image")));
            this.ExcelOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExcelOrderBtn.Name = "ExcelOrderBtn";
            this.ExcelOrderBtn.Size = new System.Drawing.Size(79, 20);
            this.ExcelOrderBtn.Text = "Excel导出";
            // 
            // PrintOrderBtn
            // 
            this.PrintOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("PrintOrderBtn.Image")));
            this.PrintOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintOrderBtn.Name = "PrintOrderBtn";
            this.PrintOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.PrintOrderBtn.Text = "打印";
            // 
            // RefreshOrderBtn
            // 
            this.RefreshOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("RefreshOrderBtn.Image")));
            this.RefreshOrderBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshOrderBtn.Name = "RefreshOrderBtn";
            this.RefreshOrderBtn.Size = new System.Drawing.Size(49, 20);
            this.RefreshOrderBtn.Text = "刷新";
            // 
            // 订单管理
            // 
            this.订单管理.AccessibleDescription = "";
            this.订单管理.AccessibleName = "";
            this.订单管理.Controls.Add(this.OrderPage);
            this.订单管理.Controls.Add(this.StorePage);
            this.订单管理.Controls.Add(this.ProducePage);
            this.订单管理.Controls.Add(this.SystemPage);
            this.订单管理.Controls.Add(this.HelpPage);
            this.订单管理.Dock = System.Windows.Forms.DockStyle.Fill;
            this.订单管理.Location = new System.Drawing.Point(0, 0);
            this.订单管理.Name = "订单管理";
            this.订单管理.SelectedIndex = 0;
            this.订单管理.Size = new System.Drawing.Size(700, 496);
            this.订单管理.TabIndex = 0;
            this.订单管理.Tag = "";
            // 
            // ProducePage
            // 
            this.ProducePage.Controls.Add(this.ProducePanelRight);
            this.ProducePage.Controls.Add(this.ProducePanelLeft);
            this.ProducePage.Location = new System.Drawing.Point(4, 22);
            this.ProducePage.Name = "ProducePage";
            this.ProducePage.Padding = new System.Windows.Forms.Padding(3);
            this.ProducePage.Size = new System.Drawing.Size(692, 470);
            this.ProducePage.TabIndex = 2;
            this.ProducePage.Text = "生产流程管理";
            this.ProducePage.UseVisualStyleBackColor = true;
            // 
            // ProducePanelRight
            // 
            this.ProducePanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProducePanelRight.Location = new System.Drawing.Point(170, 4);
            this.ProducePanelRight.Name = "ProducePanelRight";
            this.ProducePanelRight.Size = new System.Drawing.Size(524, 464);
            this.ProducePanelRight.TabIndex = 1;
            // 
            // ProducePanelLeft
            // 
            this.ProducePanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProducePanelLeft.Controls.Add(this.panel1);
            this.ProducePanelLeft.Controls.Add(this.button2);
            this.ProducePanelLeft.Controls.Add(this.PlanBtn);
            this.ProducePanelLeft.Location = new System.Drawing.Point(5, 4);
            this.ProducePanelLeft.Name = "ProducePanelLeft";
            this.ProducePanelLeft.Size = new System.Drawing.Size(159, 464);
            this.ProducePanelLeft.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.BlowBtn);
            this.panel1.Location = new System.Drawing.Point(9, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 115);
            this.panel1.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(11, 86);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(118, 27);
            this.button6.TabIndex = 3;
            this.button6.Text = "灭菌";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(11, 58);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(118, 27);
            this.button5.TabIndex = 2;
            this.button5.Text = "制袋";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(11, 30);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 27);
            this.button4.TabIndex = 1;
            this.button4.Text = "清洁分切";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // BlowBtn
            // 
            this.BlowBtn.Location = new System.Drawing.Point(11, 3);
            this.BlowBtn.Name = "BlowBtn";
            this.BlowBtn.Size = new System.Drawing.Size(118, 27);
            this.BlowBtn.TabIndex = 0;
            this.BlowBtn.Text = "吹膜";
            this.BlowBtn.UseVisualStyleBackColor = true;
            this.BlowBtn.Click += new System.EventHandler(this.BlowBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 34);
            this.button2.TabIndex = 1;
            this.button2.Text = "生产工序";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // PlanBtn
            // 
            this.PlanBtn.Location = new System.Drawing.Point(3, 4);
            this.PlanBtn.Name = "PlanBtn";
            this.PlanBtn.Size = new System.Drawing.Size(153, 34);
            this.PlanBtn.TabIndex = 0;
            this.PlanBtn.Text = "生产计划";
            this.PlanBtn.UseVisualStyleBackColor = true;
            this.PlanBtn.Click += new System.EventHandler(this.PlanBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 496);
            this.Controls.Add(this.订单管理);
            this.Name = "MainForm";
            this.Text = "欢迎使用管理系统";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SystemPage.ResumeLayout(false);
            this.SystemPage.PerformLayout();
            this.SystemtoolStrip.ResumeLayout(false);
            this.SystemtoolStrip.PerformLayout();
            this.StorePage.ResumeLayout(false);
            this.StockPanelLeft.ResumeLayout(false);
            this.OrderPage.ResumeLayout(false);
            this.OrderPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrderdataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.订单信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            this.OrderPanelTop.ResumeLayout(false);
            this.OrderPanelTop.PerformLayout();
            this.OrdertoolStrip.ResumeLayout(false);
            this.OrdertoolStrip.PerformLayout();
            this.订单管理.ResumeLayout(false);
            this.ProducePage.ResumeLayout(false);
            this.ProducePanelLeft.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage HelpPage;
        private System.Windows.Forms.TabPage SystemPage;
        private System.Windows.Forms.ToolStrip SystemtoolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TabPage StorePage;
        private System.Windows.Forms.TabPage OrderPage;
        private System.Windows.Forms.ToolStrip OrdertoolStrip;
        private System.Windows.Forms.ToolStripButton AddOrderBtn;
        private System.Windows.Forms.ToolStripButton EditOrderBtn;
        private System.Windows.Forms.TabControl 订单管理;
        private System.Windows.Forms.ToolStripButton DeleteOrderBtn;
        private System.Windows.Forms.ToolStripButton SortOrderBtn;
        private System.Windows.Forms.ToolStripButton ExcelOrderBtn;
        private System.Windows.Forms.ToolStripButton PrintOrderBtn;
        private System.Windows.Forms.ToolStripButton RefreshOrderBtn;
        private System.Windows.Forms.Panel OrderPanelTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label 订单号;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label 至;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label 日期;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SearchOrderBtn;
        private System.Windows.Forms.DataGridView OrderdataGrid;
        private System.Windows.Forms.Label 订单状态;
        private System.Windows.Forms.ComboBox OrderStateBox;
        private System.Windows.Forms.Panel StockPanelRight;
        private System.Windows.Forms.TabPage ProducePage;
        private System.Windows.Forms.Panel ProducePanelRight;
        private System.Windows.Forms.Panel ProducePanelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BlowBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button PlanBtn;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.BindingSource 订单信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单日期DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单详情;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单号DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单状态DataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel StockPanelLeft;
        private System.Windows.Forms.Button BuyBtn;
        private System.Windows.Forms.Button InOutListBtn;
        private System.Windows.Forms.Button StockOrderBtn;
        private System.Windows.Forms.Button StockCheckBtn;


    }
}