namespace mySystem.Extruction.Process
{
    partial class ExtructionTransportRecordStep4
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
            this.Title = new System.Windows.Forms.Label();
            this.TransportRecordView = new System.Windows.Forms.DataGridView();
            this.传料日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物料代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kgper件 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.包装是否完好 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.是否清洁合格 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.操作人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.复核人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bt查看人员信息 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TransportRecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(511, 8);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(169, 19);
            this.Title.TabIndex = 3;
            this.Title.Text = "吹膜工序传料记录";
            // 
            // TransportRecordView
            // 
            this.TransportRecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TransportRecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.传料日期,
            this.时间,
            this.物料代码,
            this.数量,
            this.kgper件,
            this.数量Total,
            this.包装是否完好,
            this.是否清洁合格,
            this.操作人,
            this.复核人});
            this.TransportRecordView.Location = new System.Drawing.Point(22, 35);
            this.TransportRecordView.Name = "TransportRecordView";
            this.TransportRecordView.RowTemplate.Height = 23;
            this.TransportRecordView.Size = new System.Drawing.Size(1123, 384);
            this.TransportRecordView.TabIndex = 4;
            this.TransportRecordView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TransportRecordView_CellContentClick);
            this.TransportRecordView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.TransportRecordView_CellEndEdit);
            this.TransportRecordView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.TransportRecordView_DataBindingComplete);
            // 
            // 传料日期
            // 
            this.传料日期.HeaderText = "传料日期";
            this.传料日期.Name = "传料日期";
            // 
            // 时间
            // 
            this.时间.HeaderText = "时间";
            this.时间.Name = "时间";
            // 
            // 物料代码
            // 
            this.物料代码.HeaderText = "物料代码";
            this.物料代码.Name = "物料代码";
            // 
            // 数量
            // 
            this.数量.HeaderText = "数量(件)";
            this.数量.Name = "数量";
            // 
            // kgper件
            // 
            this.kgper件.HeaderText = "kg/件";
            this.kgper件.Name = "kgper件";
            // 
            // 数量Total
            // 
            this.数量Total.HeaderText = "数量(kg)";
            this.数量Total.Name = "数量Total";
            // 
            // 包装是否完好
            // 
            this.包装是否完好.HeaderText = "包装是否完好";
            this.包装是否完好.Name = "包装是否完好";
            // 
            // 是否清洁合格
            // 
            this.是否清洁合格.HeaderText = "是否清洁合格";
            this.是否清洁合格.Name = "是否清洁合格";
            // 
            // 操作人
            // 
            this.操作人.HeaderText = "操作人";
            this.操作人.Name = "操作人";
            // 
            // 复核人
            // 
            this.复核人.HeaderText = "复核人";
            this.复核人.Name = "复核人";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(973, 428);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 6;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1065, 467);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 10;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(973, 467);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 9;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(1065, 428);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 11;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(22, 428);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(924, 39);
            this.label1.TabIndex = 12;
            this.label1.Text = "备注：吹膜用料外包装表面应干净无积尘，每天由生产指定人员传递物料，传料时应检查外包装应完好，并用专用毛巾粘清水对包       装外表面进行清洁，传入供料间物料应" +
                "干净整洁。";
            // 
            // bt查看人员信息
            // 
            this.bt查看人员信息.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt查看人员信息.Location = new System.Drawing.Point(798, 467);
            this.bt查看人员信息.Name = "bt查看人员信息";
            this.bt查看人员信息.Size = new System.Drawing.Size(123, 30);
            this.bt查看人员信息.TabIndex = 27;
            this.bt查看人员信息.Text = "查看人员信息";
            this.bt查看人员信息.UseVisualStyleBackColor = true;
            this.bt查看人员信息.Click += new System.EventHandler(this.bt查看人员信息_Click);
            // 
            // ExtructionTransportRecordStep4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 509);
            this.Controls.Add(this.bt查看人员信息);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.TransportRecordView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExtructionTransportRecordStep4";
            this.Text = "ExtructionTransportRecordStep4";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExtructionTransportRecordStep4_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.TransportRecordView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView TransportRecordView;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 传料日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物料代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn kgper件;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量Total;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 包装是否完好;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否清洁合格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 操作人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 复核人;
        private System.Windows.Forms.Button bt查看人员信息;
    }
}