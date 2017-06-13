namespace mySystem
{
    partial class StockCheckForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockCheckForm));
            this.SearchStockCheckBtn = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.物品名称 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.类别 = new System.Windows.Forms.Label();
            this.StockChecktoolStrip = new System.Windows.Forms.ToolStrip();
            this.NewItemBtn = new System.Windows.Forms.ToolStripButton();
            this.SortStockBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExcelStockBtn = new System.Windows.Forms.ToolStripButton();
            this.PrintStockBtn = new System.Windows.Forms.ToolStripButton();
            this.RefreshStockBtn = new System.Windows.Forms.ToolStripButton();
            this.StockCheckdataGrid = new System.Windows.Forms.DataGridView();
            this.物品类别DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品代码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.总库存DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预警库存DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.缺货数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品状态DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.库存信息BindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.库存信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StockChecktoolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchStockCheckBtn
            // 
            this.SearchStockCheckBtn.Location = new System.Drawing.Point(433, 59);
            this.SearchStockCheckBtn.Name = "SearchStockCheckBtn";
            this.SearchStockCheckBtn.Size = new System.Drawing.Size(75, 23);
            this.SearchStockCheckBtn.TabIndex = 17;
            this.SearchStockCheckBtn.Text = "查询";
            this.SearchStockCheckBtn.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(264, 58);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(147, 21);
            this.textBox2.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "物品状态：";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(262, 31);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(246, 21);
            this.textBox.TabIndex = 14;
            // 
            // 物品名称
            // 
            this.物品名称.AutoSize = true;
            this.物品名称.Location = new System.Drawing.Point(198, 34);
            this.物品名称.Name = "物品名称";
            this.物品名称.Size = new System.Drawing.Size(65, 12);
            this.物品名称.TabIndex = 13;
            this.物品名称.Text = "物品名称：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "原材料",
            "半成品",
            "成品",
            "组件"});
            this.comboBox1.Location = new System.Drawing.Point(48, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(136, 20);
            this.comboBox1.TabIndex = 12;
            // 
            // 类别
            // 
            this.类别.AutoSize = true;
            this.类别.Location = new System.Drawing.Point(9, 34);
            this.类别.Name = "类别";
            this.类别.Size = new System.Drawing.Size(41, 12);
            this.类别.TabIndex = 11;
            this.类别.Text = "类别：";
            // 
            // StockChecktoolStrip
            // 
            this.StockChecktoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewItemBtn,
            this.SortStockBtn,
            this.toolStripSeparator2,
            this.ExcelStockBtn,
            this.PrintStockBtn,
            this.RefreshStockBtn});
            this.StockChecktoolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.StockChecktoolStrip.Location = new System.Drawing.Point(0, 0);
            this.StockChecktoolStrip.Name = "StockChecktoolStrip";
            this.StockChecktoolStrip.Size = new System.Drawing.Size(524, 23);
            this.StockChecktoolStrip.TabIndex = 10;
            this.StockChecktoolStrip.Text = "toolStrip2";
            // 
            // NewItemBtn
            // 
            this.NewItemBtn.Image = ((System.Drawing.Image)(resources.GetObject("NewItemBtn.Image")));
            this.NewItemBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewItemBtn.Name = "NewItemBtn";
            this.NewItemBtn.Size = new System.Drawing.Size(73, 20);
            this.NewItemBtn.Text = "新建种类";
            // 
            // SortStockBtn
            // 
            this.SortStockBtn.Image = ((System.Drawing.Image)(resources.GetObject("SortStockBtn.Image")));
            this.SortStockBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SortStockBtn.Name = "SortStockBtn";
            this.SortStockBtn.Size = new System.Drawing.Size(49, 20);
            this.SortStockBtn.Text = "排序";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // ExcelStockBtn
            // 
            this.ExcelStockBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExcelStockBtn.Image")));
            this.ExcelStockBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExcelStockBtn.Name = "ExcelStockBtn";
            this.ExcelStockBtn.Size = new System.Drawing.Size(79, 20);
            this.ExcelStockBtn.Text = "Excel导出";
            // 
            // PrintStockBtn
            // 
            this.PrintStockBtn.Image = ((System.Drawing.Image)(resources.GetObject("PrintStockBtn.Image")));
            this.PrintStockBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintStockBtn.Name = "PrintStockBtn";
            this.PrintStockBtn.Size = new System.Drawing.Size(49, 20);
            this.PrintStockBtn.Text = "打印";
            // 
            // RefreshStockBtn
            // 
            this.RefreshStockBtn.Image = ((System.Drawing.Image)(resources.GetObject("RefreshStockBtn.Image")));
            this.RefreshStockBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshStockBtn.Name = "RefreshStockBtn";
            this.RefreshStockBtn.Size = new System.Drawing.Size(49, 20);
            this.RefreshStockBtn.Text = "刷新";
            // 
            // StockCheckdataGrid
            // 
            this.StockCheckdataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StockCheckdataGrid.AutoGenerateColumns = false;
            this.StockCheckdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockCheckdataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.物品类别DataGridViewTextBoxColumn,
            this.物品代码DataGridViewTextBoxColumn,
            this.物品名称DataGridViewTextBoxColumn,
            this.总库存DataGridViewTextBoxColumn,
            this.单位DataGridViewTextBoxColumn,
            this.预警库存DataGridViewTextBoxColumn,
            this.缺货数量DataGridViewTextBoxColumn,
            this.物品状态DataGridViewTextBoxColumn});
            this.StockCheckdataGrid.DataSource = this.库存信息BindingSource1;
            this.StockCheckdataGrid.Location = new System.Drawing.Point(3, 88);
            this.StockCheckdataGrid.Name = "StockCheckdataGrid";
            this.StockCheckdataGrid.RowTemplate.Height = 23;
            this.StockCheckdataGrid.Size = new System.Drawing.Size(515, 372);
            this.StockCheckdataGrid.TabIndex = 18;
            // 
            // 物品类别DataGridViewTextBoxColumn
            // 
            this.物品类别DataGridViewTextBoxColumn.DataPropertyName = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.HeaderText = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.Name = "物品类别DataGridViewTextBoxColumn";
            // 
            // 物品代码DataGridViewTextBoxColumn
            // 
            this.物品代码DataGridViewTextBoxColumn.DataPropertyName = "物品代码";
            this.物品代码DataGridViewTextBoxColumn.HeaderText = "物品代码";
            this.物品代码DataGridViewTextBoxColumn.Name = "物品代码DataGridViewTextBoxColumn";
            // 
            // 物品名称DataGridViewTextBoxColumn
            // 
            this.物品名称DataGridViewTextBoxColumn.DataPropertyName = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.HeaderText = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.Name = "物品名称DataGridViewTextBoxColumn";
            // 
            // 总库存DataGridViewTextBoxColumn
            // 
            this.总库存DataGridViewTextBoxColumn.DataPropertyName = "总库存";
            this.总库存DataGridViewTextBoxColumn.HeaderText = "总库存";
            this.总库存DataGridViewTextBoxColumn.Name = "总库存DataGridViewTextBoxColumn";
            // 
            // 单位DataGridViewTextBoxColumn
            // 
            this.单位DataGridViewTextBoxColumn.DataPropertyName = "单位";
            this.单位DataGridViewTextBoxColumn.HeaderText = "单位";
            this.单位DataGridViewTextBoxColumn.Name = "单位DataGridViewTextBoxColumn";
            // 
            // 预警库存DataGridViewTextBoxColumn
            // 
            this.预警库存DataGridViewTextBoxColumn.DataPropertyName = "预警库存";
            this.预警库存DataGridViewTextBoxColumn.HeaderText = "预警库存";
            this.预警库存DataGridViewTextBoxColumn.Name = "预警库存DataGridViewTextBoxColumn";
            // 
            // 缺货数量DataGridViewTextBoxColumn
            // 
            this.缺货数量DataGridViewTextBoxColumn.DataPropertyName = "缺货数量";
            this.缺货数量DataGridViewTextBoxColumn.HeaderText = "缺货数量";
            this.缺货数量DataGridViewTextBoxColumn.Name = "缺货数量DataGridViewTextBoxColumn";
            // 
            // 物品状态DataGridViewTextBoxColumn
            // 
            this.物品状态DataGridViewTextBoxColumn.DataPropertyName = "物品状态";
            this.物品状态DataGridViewTextBoxColumn.HeaderText = "物品状态";
            this.物品状态DataGridViewTextBoxColumn.Name = "物品状态DataGridViewTextBoxColumn";
            // 
            // 库存信息BindingSource1
            // 
            this.库存信息BindingSource1.DataMember = "库存信息";
            this.库存信息BindingSource1.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 库存信息BindingSource
            // 
            this.库存信息BindingSource.DataMember = "库存信息";
            this.库存信息BindingSource.DataSource = this.myDataSet;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(70, 58);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(114, 20);
            this.comboBox2.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "物品编号：";
            // 
            // StockCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StockCheckdataGrid);
            this.Controls.Add(this.SearchStockCheckBtn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.物品名称);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.类别);
            this.Controls.Add(this.StockChecktoolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StockCheckForm";
            this.Text = "StockCheckForm";
            this.StockChecktoolStrip.ResumeLayout(false);
            this.StockChecktoolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchStockCheckBtn;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label 物品名称;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label 类别;
        private System.Windows.Forms.ToolStrip StockChecktoolStrip;
        private System.Windows.Forms.ToolStripButton NewItemBtn;
        private System.Windows.Forms.ToolStripButton SortStockBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ExcelStockBtn;
        private System.Windows.Forms.ToolStripButton PrintStockBtn;
        private System.Windows.Forms.ToolStripButton RefreshStockBtn;
        private System.Windows.Forms.DataGridView StockCheckdataGrid;
        private System.Windows.Forms.BindingSource 库存信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品类别DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品代码DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总库存DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预警库存DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 缺货数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品状态DataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource 库存信息BindingSource1;
    }
}