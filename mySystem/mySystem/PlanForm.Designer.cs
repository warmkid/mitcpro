namespace mySystem
{
    partial class PlanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanForm));
            this.StockCheckdataGrid = new System.Windows.Forms.DataGridView();
            this.生产计划信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddPlanBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.开始日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.结束日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.工序DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.原料DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.人员DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.人员数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.要求DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.生产计划信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StockCheckdataGrid
            // 
            this.StockCheckdataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StockCheckdataGrid.AutoGenerateColumns = false;
            this.StockCheckdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockCheckdataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.开始日期DataGridViewTextBoxColumn,
            this.结束日期DataGridViewTextBoxColumn,
            this.工序DataGridViewTextBoxColumn,
            this.原料DataGridViewTextBoxColumn,
            this.数量DataGridViewTextBoxColumn,
            this.单位DataGridViewTextBoxColumn,
            this.人员DataGridViewTextBoxColumn,
            this.人员数量DataGridViewTextBoxColumn,
            this.要求DataGridViewTextBoxColumn,
            this.备注DataGridViewTextBoxColumn});
            this.StockCheckdataGrid.DataSource = this.生产计划信息BindingSource;
            this.StockCheckdataGrid.Location = new System.Drawing.Point(5, 28);
            this.StockCheckdataGrid.Name = "StockCheckdataGrid";
            this.StockCheckdataGrid.RowTemplate.Height = 23;
            this.StockCheckdataGrid.Size = new System.Drawing.Size(515, 432);
            this.StockCheckdataGrid.TabIndex = 19;
            // 
            // 生产计划信息BindingSource
            // 
            this.生产计划信息BindingSource.DataMember = "生产计划信息";
            this.生产计划信息BindingSource.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddPlanBtn,
            this.toolStripButton2,
            this.toolStripButton6,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(524, 25);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddPlanBtn
            // 
            this.AddPlanBtn.Image = ((System.Drawing.Image)(resources.GetObject("AddPlanBtn.Image")));
            this.AddPlanBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddPlanBtn.Name = "AddPlanBtn";
            this.AddPlanBtn.Size = new System.Drawing.Size(97, 22);
            this.AddPlanBtn.Text = "新建生产计划";
            this.AddPlanBtn.Click += new System.EventHandler(this.AddPlanBtn_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton2.Text = "修改";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton6.Text = "保存";
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
            // Column1
            // 
            this.Column1.DataPropertyName = "开始日期";
            this.Column1.HeaderText = ".";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // 开始日期DataGridViewTextBoxColumn
            // 
            this.开始日期DataGridViewTextBoxColumn.DataPropertyName = "开始日期";
            this.开始日期DataGridViewTextBoxColumn.HeaderText = "开始日期";
            this.开始日期DataGridViewTextBoxColumn.Name = "开始日期DataGridViewTextBoxColumn";
            // 
            // 结束日期DataGridViewTextBoxColumn
            // 
            this.结束日期DataGridViewTextBoxColumn.DataPropertyName = "结束日期";
            this.结束日期DataGridViewTextBoxColumn.HeaderText = "结束日期";
            this.结束日期DataGridViewTextBoxColumn.Name = "结束日期DataGridViewTextBoxColumn";
            // 
            // 工序DataGridViewTextBoxColumn
            // 
            this.工序DataGridViewTextBoxColumn.DataPropertyName = "工序";
            this.工序DataGridViewTextBoxColumn.HeaderText = "工序";
            this.工序DataGridViewTextBoxColumn.Name = "工序DataGridViewTextBoxColumn";
            // 
            // 原料DataGridViewTextBoxColumn
            // 
            this.原料DataGridViewTextBoxColumn.DataPropertyName = "原料";
            this.原料DataGridViewTextBoxColumn.HeaderText = "原料";
            this.原料DataGridViewTextBoxColumn.Name = "原料DataGridViewTextBoxColumn";
            // 
            // 数量DataGridViewTextBoxColumn
            // 
            this.数量DataGridViewTextBoxColumn.DataPropertyName = "数量";
            this.数量DataGridViewTextBoxColumn.HeaderText = "数量";
            this.数量DataGridViewTextBoxColumn.Name = "数量DataGridViewTextBoxColumn";
            // 
            // 单位DataGridViewTextBoxColumn
            // 
            this.单位DataGridViewTextBoxColumn.DataPropertyName = "单位";
            this.单位DataGridViewTextBoxColumn.HeaderText = "单位";
            this.单位DataGridViewTextBoxColumn.Name = "单位DataGridViewTextBoxColumn";
            // 
            // 人员DataGridViewTextBoxColumn
            // 
            this.人员DataGridViewTextBoxColumn.DataPropertyName = "人员";
            this.人员DataGridViewTextBoxColumn.HeaderText = "人员";
            this.人员DataGridViewTextBoxColumn.Name = "人员DataGridViewTextBoxColumn";
            // 
            // 人员数量DataGridViewTextBoxColumn
            // 
            this.人员数量DataGridViewTextBoxColumn.DataPropertyName = "人员数量";
            this.人员数量DataGridViewTextBoxColumn.HeaderText = "人员数量";
            this.人员数量DataGridViewTextBoxColumn.Name = "人员数量DataGridViewTextBoxColumn";
            // 
            // 要求DataGridViewTextBoxColumn
            // 
            this.要求DataGridViewTextBoxColumn.DataPropertyName = "要求";
            this.要求DataGridViewTextBoxColumn.HeaderText = "要求";
            this.要求DataGridViewTextBoxColumn.Name = "要求DataGridViewTextBoxColumn";
            // 
            // 备注DataGridViewTextBoxColumn
            // 
            this.备注DataGridViewTextBoxColumn.DataPropertyName = "备注";
            this.备注DataGridViewTextBoxColumn.HeaderText = "备注";
            this.备注DataGridViewTextBoxColumn.Name = "备注DataGridViewTextBoxColumn";
            // 
            // PlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.StockCheckdataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PlanForm";
            this.Text = "PlanForm";
            ((System.ComponentModel.ISupportInitialize)(this.StockCheckdataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.生产计划信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView StockCheckdataGrid;
        private System.Windows.Forms.BindingSource 生产计划信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddPlanBtn;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 开始日期DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 结束日期DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 工序DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 原料DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 人员DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 人员数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 要求DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备注DataGridViewTextBoxColumn;
    }
}