namespace mySystem
{
    partial class InOutListForm
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.StockCheckdataGrid = new System.Windows.Forms.DataGridView();
            this.出库入库DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.时间DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品类别DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品编码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经手人DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.出入库记录BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.SearchStockCheckBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.出入库记录BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(13, 14);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "全部";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(60, 14);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "出库";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(107, 14);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "入库";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // StockCheckdataGrid
            // 
            this.StockCheckdataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StockCheckdataGrid.AutoGenerateColumns = false;
            this.StockCheckdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockCheckdataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.出库入库DataGridViewTextBoxColumn,
            this.时间DataGridViewTextBoxColumn,
            this.物品类别DataGridViewTextBoxColumn,
            this.物品名称DataGridViewTextBoxColumn,
            this.数量DataGridViewTextBoxColumn,
            this.物品编码DataGridViewTextBoxColumn,
            this.经手人DataGridViewTextBoxColumn});
            this.StockCheckdataGrid.DataSource = this.出入库记录BindingSource;
            this.StockCheckdataGrid.Location = new System.Drawing.Point(5, 42);
            this.StockCheckdataGrid.Name = "StockCheckdataGrid";
            this.StockCheckdataGrid.RowTemplate.Height = 23;
            this.StockCheckdataGrid.Size = new System.Drawing.Size(515, 417);
            this.StockCheckdataGrid.TabIndex = 20;
            // 
            // 出库入库DataGridViewTextBoxColumn
            // 
            this.出库入库DataGridViewTextBoxColumn.DataPropertyName = "出库/入库";
            this.出库入库DataGridViewTextBoxColumn.HeaderText = "出库/入库";
            this.出库入库DataGridViewTextBoxColumn.Name = "出库入库DataGridViewTextBoxColumn";
            // 
            // 时间DataGridViewTextBoxColumn
            // 
            this.时间DataGridViewTextBoxColumn.DataPropertyName = "时间";
            this.时间DataGridViewTextBoxColumn.HeaderText = "时间";
            this.时间DataGridViewTextBoxColumn.Name = "时间DataGridViewTextBoxColumn";
            // 
            // 物品类别DataGridViewTextBoxColumn
            // 
            this.物品类别DataGridViewTextBoxColumn.DataPropertyName = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.HeaderText = "物品类别";
            this.物品类别DataGridViewTextBoxColumn.Name = "物品类别DataGridViewTextBoxColumn";
            // 
            // 物品名称DataGridViewTextBoxColumn
            // 
            this.物品名称DataGridViewTextBoxColumn.DataPropertyName = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.HeaderText = "物品名称";
            this.物品名称DataGridViewTextBoxColumn.Name = "物品名称DataGridViewTextBoxColumn";
            // 
            // 数量DataGridViewTextBoxColumn
            // 
            this.数量DataGridViewTextBoxColumn.DataPropertyName = "数量";
            this.数量DataGridViewTextBoxColumn.HeaderText = "数量";
            this.数量DataGridViewTextBoxColumn.Name = "数量DataGridViewTextBoxColumn";
            // 
            // 物品编码DataGridViewTextBoxColumn
            // 
            this.物品编码DataGridViewTextBoxColumn.DataPropertyName = "物品编码";
            this.物品编码DataGridViewTextBoxColumn.HeaderText = "物品编码";
            this.物品编码DataGridViewTextBoxColumn.Name = "物品编码DataGridViewTextBoxColumn";
            // 
            // 经手人DataGridViewTextBoxColumn
            // 
            this.经手人DataGridViewTextBoxColumn.DataPropertyName = "经手人";
            this.经手人DataGridViewTextBoxColumn.HeaderText = "经手人";
            this.经手人DataGridViewTextBoxColumn.Name = "经手人DataGridViewTextBoxColumn";
            // 
            // 出入库记录BindingSource
            // 
            this.出入库记录BindingSource.DataMember = "出入库记录";
            // 
            // SearchStockCheckBtn
            // 
            this.SearchStockCheckBtn.Location = new System.Drawing.Point(359, 8);
            this.SearchStockCheckBtn.Name = "SearchStockCheckBtn";
            this.SearchStockCheckBtn.Size = new System.Drawing.Size(75, 23);
            this.SearchStockCheckBtn.TabIndex = 21;
            this.SearchStockCheckBtn.Text = "查询";
            this.SearchStockCheckBtn.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(439, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "打印";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "物品名称",
            "物品类别",
            "物品编号",
            "时间",
            "经手人"});
            this.comboBox1.Location = new System.Drawing.Point(161, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(70, 20);
            this.comboBox1.TabIndex = 23;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(234, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 21);
            this.textBox1.TabIndex = 24;
            // 
            // InOutListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SearchStockCheckBtn);
            this.Controls.Add(this.StockCheckdataGrid);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InOutListForm";
            this.Text = "InOutListForm";
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.出入库记录BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.DataGridView StockCheckdataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn 出库入库DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 时间DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品类别DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品编码DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经手人DataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource 出入库记录BindingSource;
        private System.Windows.Forms.Button SearchStockCheckBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}