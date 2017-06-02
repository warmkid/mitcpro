namespace mySystem
{
    partial class StockOrderForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockOrderForm));
            this.PrintStockOrderBtn = new System.Windows.Forms.Button();
            this.SaveStockOrderBtn = new System.Windows.Forms.Button();
            this.AddStockOrderBtn = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.商品信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.说明TextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.经办人TextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.销售单号TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.出库日期DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StockOrderDataGridView = new System.Windows.Forms.DataGridView();
            this.物品类别DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品编号DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.质量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.出库信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EditStockOrderBtn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.商品信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOrderDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.出库信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // PrintStockOrderBtn
            // 
            this.PrintStockOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("PrintStockOrderBtn.Image")));
            this.PrintStockOrderBtn.Location = new System.Drawing.Point(221, 4);
            this.PrintStockOrderBtn.Name = "PrintStockOrderBtn";
            this.PrintStockOrderBtn.Size = new System.Drawing.Size(100, 23);
            this.PrintStockOrderBtn.TabIndex = 91;
            this.PrintStockOrderBtn.Text = "打印出库单";
            this.PrintStockOrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintStockOrderBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.PrintStockOrderBtn.UseVisualStyleBackColor = true;
            // 
            // SaveStockOrderBtn
            // 
            this.SaveStockOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("SaveStockOrderBtn.Image")));
            this.SaveStockOrderBtn.Location = new System.Drawing.Point(329, 4);
            this.SaveStockOrderBtn.Name = "SaveStockOrderBtn";
            this.SaveStockOrderBtn.Size = new System.Drawing.Size(100, 23);
            this.SaveStockOrderBtn.TabIndex = 77;
            this.SaveStockOrderBtn.Text = "保存出库单";
            this.SaveStockOrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SaveStockOrderBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SaveStockOrderBtn.UseVisualStyleBackColor = true;
            // 
            // AddStockOrderBtn
            // 
            this.AddStockOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("AddStockOrderBtn.Image")));
            this.AddStockOrderBtn.Location = new System.Drawing.Point(6, 4);
            this.AddStockOrderBtn.Name = "AddStockOrderBtn";
            this.AddStockOrderBtn.Size = new System.Drawing.Size(100, 23);
            this.AddStockOrderBtn.TabIndex = 74;
            this.AddStockOrderBtn.Text = "新增出库单";
            this.AddStockOrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AddStockOrderBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.AddStockOrderBtn.UseVisualStyleBackColor = true;
            // 
            // 商品信息BindingSource
            // 
            this.商品信息BindingSource.DataMember = "商品信息";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // 说明TextBox
            // 
            this.说明TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.说明TextBox.Location = new System.Drawing.Point(247, 87);
            this.说明TextBox.Name = "说明TextBox";
            this.说明TextBox.Size = new System.Drawing.Size(271, 21);
            this.说明TextBox.TabIndex = 63;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(204, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 62;
            this.label9.Text = "说明：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 61;
            this.label8.Text = "出库日期：";
            // 
            // 经办人TextBox
            // 
            this.经办人TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.经办人TextBox.Location = new System.Drawing.Point(232, 60);
            this.经办人TextBox.Name = "经办人TextBox";
            this.经办人TextBox.Size = new System.Drawing.Size(0, 21);
            this.经办人TextBox.TabIndex = 60;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(256, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 59;
            this.label7.Text = "供料班次：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 58;
            this.label6.Text = "供料人员：";
            // 
            // 销售单号TextBox
            // 
            this.销售单号TextBox.Location = new System.Drawing.Point(66, 34);
            this.销售单号TextBox.Name = "销售单号TextBox";
            this.销售单号TextBox.Size = new System.Drawing.Size(177, 21);
            this.销售单号TextBox.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 51;
            this.label2.Text = "出库单号：";
            // 
            // 出库日期DateTimePicker
            // 
            this.出库日期DateTimePicker.Location = new System.Drawing.Point(73, 87);
            this.出库日期DateTimePicker.Name = "出库日期DateTimePicker";
            this.出库日期DateTimePicker.Size = new System.Drawing.Size(124, 21);
            this.出库日期DateTimePicker.TabIndex = 76;
            // 
            // StockOrderDataGridView
            // 
            this.StockOrderDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StockOrderDataGridView.AutoGenerateColumns = false;
            this.StockOrderDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockOrderDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.物品类别DataGridViewTextBoxColumn,
            this.物品名称DataGridViewTextBoxColumn,
            this.物品编号DataGridViewTextBoxColumn,
            this.单位DataGridViewTextBoxColumn,
            this.质量DataGridViewTextBoxColumn});
            this.StockOrderDataGridView.DataSource = this.出库信息BindingSource;
            this.StockOrderDataGridView.Location = new System.Drawing.Point(4, 125);
            this.StockOrderDataGridView.MultiSelect = false;
            this.StockOrderDataGridView.Name = "StockOrderDataGridView";
            this.StockOrderDataGridView.ReadOnly = true;
            this.StockOrderDataGridView.RowTemplate.Height = 23;
            this.StockOrderDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.StockOrderDataGridView.Size = new System.Drawing.Size(515, 335);
            this.StockOrderDataGridView.TabIndex = 90;
            // 
            // 物品类别DataGridViewTextBoxColumn
            // 
            this.物品类别DataGridViewTextBoxColumn.DataPropertyName = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.HeaderText = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.Name = "物品类别DataGridViewTextBoxColumn";
            this.物品类别DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // 物品名称DataGridViewTextBoxColumn
            // 
            this.物品名称DataGridViewTextBoxColumn.DataPropertyName = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.HeaderText = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.Name = "物品名称DataGridViewTextBoxColumn";
            this.物品名称DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // 物品编号DataGridViewTextBoxColumn
            // 
            this.物品编号DataGridViewTextBoxColumn.DataPropertyName = "物品编号";
            this.物品编号DataGridViewTextBoxColumn.HeaderText = "物品编号";
            this.物品编号DataGridViewTextBoxColumn.Name = "物品编号DataGridViewTextBoxColumn";
            this.物品编号DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // 单位DataGridViewTextBoxColumn
            // 
            this.单位DataGridViewTextBoxColumn.DataPropertyName = "单位";
            this.单位DataGridViewTextBoxColumn.HeaderText = "单位";
            this.单位DataGridViewTextBoxColumn.Name = "单位DataGridViewTextBoxColumn";
            this.单位DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // 质量DataGridViewTextBoxColumn
            // 
            this.质量DataGridViewTextBoxColumn.DataPropertyName = "质量";
            this.质量DataGridViewTextBoxColumn.HeaderText = "质量";
            this.质量DataGridViewTextBoxColumn.Name = "质量DataGridViewTextBoxColumn";
            this.质量DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // 出库信息BindingSource
            // 
            this.出库信息BindingSource.DataMember = "出库信息";
            this.出库信息BindingSource.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(171, 21);
            this.textBox1.TabIndex = 92;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(338, 33);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(179, 21);
            this.textBox2.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(254, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 93;
            this.label1.Text = "生产计划单号：";
            // 
            // EditStockOrderBtn
            // 
            this.EditStockOrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditStockOrderBtn.Image")));
            this.EditStockOrderBtn.Location = new System.Drawing.Point(112, 4);
            this.EditStockOrderBtn.Name = "EditStockOrderBtn";
            this.EditStockOrderBtn.Size = new System.Drawing.Size(102, 23);
            this.EditStockOrderBtn.TabIndex = 79;
            this.EditStockOrderBtn.Text = "编辑出库单";
            this.EditStockOrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EditStockOrderBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.EditStockOrderBtn.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(338, 61);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 20);
            this.comboBox1.TabIndex = 95;
            // 
            // StockOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.PrintStockOrderBtn);
            this.Controls.Add(this.StockOrderDataGridView);
            this.Controls.Add(this.EditStockOrderBtn);
            this.Controls.Add(this.SaveStockOrderBtn);
            this.Controls.Add(this.出库日期DateTimePicker);
            this.Controls.Add(this.AddStockOrderBtn);
            this.Controls.Add(this.说明TextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.经办人TextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.销售单号TextBox);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StockOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "出库登记";
            ((System.ComponentModel.ISupportInitialize)(this.商品信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOrderDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.出库信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PrintStockOrderBtn;
        private System.Windows.Forms.Button SaveStockOrderBtn;
        private System.Windows.Forms.Button AddStockOrderBtn;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.BindingSource 商品信息BindingSource;
        //private MySaleDataSet mySaleDataSet;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        //private MySale.MySaleDataSetTableAdapters.商品信息TableAdapter 商品信息TableAdapter;
        private System.Windows.Forms.TextBox 说明TextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox 经办人TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox 销售单号TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker 出库日期DateTimePicker;
        private System.Windows.Forms.DataGridView StockOrderDataGridView;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品类别DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品编号DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 质量DataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource 出库信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button EditStockOrderBtn;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}