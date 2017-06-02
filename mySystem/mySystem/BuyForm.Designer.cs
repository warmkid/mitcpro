namespace mySystem
{
    partial class BuyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyForm));
            this.StockCheckdataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.采购日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.到货日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商信息DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备选供应商信息DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.加工商信息DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.价格DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.平均价格DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商服务水平DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否特定DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.采购信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.采购信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StockCheckdataGrid
            // 
            this.StockCheckdataGrid.AllowUserToOrderColumns = true;
            this.StockCheckdataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StockCheckdataGrid.AutoGenerateColumns = false;
            this.StockCheckdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockCheckdataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.采购日期DataGridViewTextBoxColumn,
            this.物品名称DataGridViewTextBoxColumn,
            this.数量DataGridViewTextBoxColumn,
            this.到货日期DataGridViewTextBoxColumn,
            this.供应商信息DataGridViewTextBoxColumn,
            this.备选供应商信息DataGridViewTextBoxColumn,
            this.加工商信息DataGridViewTextBoxColumn,
            this.价格DataGridViewTextBoxColumn,
            this.平均价格DataGridViewTextBoxColumn,
            this.供应商服务水平DataGridViewTextBoxColumn,
            this.是否特定DataGridViewTextBoxColumn});
            this.StockCheckdataGrid.DataSource = this.采购信息BindingSource;
            this.StockCheckdataGrid.Location = new System.Drawing.Point(5, 28);
            this.StockCheckdataGrid.Name = "StockCheckdataGrid";
            this.StockCheckdataGrid.RowTemplate.Height = 23;
            this.StockCheckdataGrid.Size = new System.Drawing.Size(515, 429);
            this.StockCheckdataGrid.TabIndex = 21;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "采购日期";
            this.Column1.HeaderText = ".";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // 采购日期DataGridViewTextBoxColumn
            // 
            this.采购日期DataGridViewTextBoxColumn.DataPropertyName = "采购日期";
            this.采购日期DataGridViewTextBoxColumn.HeaderText = "采购日期";
            this.采购日期DataGridViewTextBoxColumn.Name = "采购日期DataGridViewTextBoxColumn";
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
            this.数量DataGridViewTextBoxColumn.Width = 70;
            // 
            // 到货日期DataGridViewTextBoxColumn
            // 
            this.到货日期DataGridViewTextBoxColumn.DataPropertyName = "到货日期";
            this.到货日期DataGridViewTextBoxColumn.HeaderText = "到货日期";
            this.到货日期DataGridViewTextBoxColumn.Name = "到货日期DataGridViewTextBoxColumn";
            // 
            // 供应商信息DataGridViewTextBoxColumn
            // 
            this.供应商信息DataGridViewTextBoxColumn.DataPropertyName = "供应商信息";
            this.供应商信息DataGridViewTextBoxColumn.HeaderText = "供应商信息";
            this.供应商信息DataGridViewTextBoxColumn.Name = "供应商信息DataGridViewTextBoxColumn";
            // 
            // 备选供应商信息DataGridViewTextBoxColumn
            // 
            this.备选供应商信息DataGridViewTextBoxColumn.DataPropertyName = "备选供应商信息";
            this.备选供应商信息DataGridViewTextBoxColumn.HeaderText = "备选供应商信息";
            this.备选供应商信息DataGridViewTextBoxColumn.Name = "备选供应商信息DataGridViewTextBoxColumn";
            // 
            // 加工商信息DataGridViewTextBoxColumn
            // 
            this.加工商信息DataGridViewTextBoxColumn.DataPropertyName = "加工商信息";
            this.加工商信息DataGridViewTextBoxColumn.HeaderText = "加工商信息";
            this.加工商信息DataGridViewTextBoxColumn.Name = "加工商信息DataGridViewTextBoxColumn";
            // 
            // 价格DataGridViewTextBoxColumn
            // 
            this.价格DataGridViewTextBoxColumn.DataPropertyName = "价格";
            this.价格DataGridViewTextBoxColumn.HeaderText = "价格";
            this.价格DataGridViewTextBoxColumn.Name = "价格DataGridViewTextBoxColumn";
            // 
            // 平均价格DataGridViewTextBoxColumn
            // 
            this.平均价格DataGridViewTextBoxColumn.DataPropertyName = "平均价格";
            this.平均价格DataGridViewTextBoxColumn.HeaderText = "平均价格";
            this.平均价格DataGridViewTextBoxColumn.Name = "平均价格DataGridViewTextBoxColumn";
            // 
            // 供应商服务水平DataGridViewTextBoxColumn
            // 
            this.供应商服务水平DataGridViewTextBoxColumn.DataPropertyName = "供应商服务水平";
            this.供应商服务水平DataGridViewTextBoxColumn.HeaderText = "供应商服务水平";
            this.供应商服务水平DataGridViewTextBoxColumn.Name = "供应商服务水平DataGridViewTextBoxColumn";
            // 
            // 是否特定DataGridViewTextBoxColumn
            // 
            this.是否特定DataGridViewTextBoxColumn.DataPropertyName = "是否特定";
            this.是否特定DataGridViewTextBoxColumn.HeaderText = "是否特定";
            this.是否特定DataGridViewTextBoxColumn.Name = "是否特定DataGridViewTextBoxColumn";
            // 
            // 采购信息BindingSource
            // 
            this.采购信息BindingSource.DataMember = "采购信息";
            this.采购信息BindingSource.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(524, 25);
            this.toolStrip1.TabIndex = 22;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(97, 22);
            this.toolStripButton1.Text = "新增采购计划";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton2.Text = "修改";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton3.Text = "删除";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton4.Text = "打印";
            // 
            // BuyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.StockCheckdataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BuyForm";
            this.Text = "BuyForm";
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.采购信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView StockCheckdataGrid;
        private System.Windows.Forms.BindingSource 采购信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 采购日期DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 到货日期DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商信息DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备选供应商信息DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 加工商信息DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 价格DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 平均价格DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商服务水平DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否特定DataGridViewTextBoxColumn;

    }
}