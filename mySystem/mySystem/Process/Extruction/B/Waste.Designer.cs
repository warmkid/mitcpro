namespace mySystem.Process.Extruction.B
{
    partial class Waste
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
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp生产结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp生产开始时间 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txb生产指令 = new System.Windows.Forms.TextBox();
            this.lbInstruction = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(440, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 19);
            this.label7.TabIndex = 52;
            this.label7.Text = "吹膜工序废品记录";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtp生产结束时间);
            this.panel1.Controls.Add(this.dtp生产开始时间);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txb生产指令);
            this.panel1.Controls.Add(this.lbInstruction);
            this.panel1.Location = new System.Drawing.Point(32, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(860, 54);
            this.panel1.TabIndex = 59;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(638, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "——";
            // 
            // dtp生产结束时间
            // 
            this.dtp生产结束时间.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp生产结束时间.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp生产结束时间.Location = new System.Drawing.Point(686, 9);
            this.dtp生产结束时间.Margin = new System.Windows.Forms.Padding(4);
            this.dtp生产结束时间.Name = "dtp生产结束时间";
            this.dtp生产结束时间.Size = new System.Drawing.Size(151, 26);
            this.dtp生产结束时间.TabIndex = 56;
            // 
            // dtp生产开始时间
            // 
            this.dtp生产开始时间.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp生产开始时间.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp生产开始时间.Location = new System.Drawing.Point(477, 9);
            this.dtp生产开始时间.Margin = new System.Windows.Forms.Padding(4);
            this.dtp生产开始时间.Name = "dtp生产开始时间";
            this.dtp生产开始时间.Size = new System.Drawing.Size(151, 26);
            this.dtp生产开始时间.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(386, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 54;
            this.label1.Text = "生产时段";
            // 
            // txb生产指令
            // 
            this.txb生产指令.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txb生产指令.Location = new System.Drawing.Point(114, 9);
            this.txb生产指令.Margin = new System.Windows.Forms.Padding(4);
            this.txb生产指令.Name = "txb生产指令";
            this.txb生产指令.Size = new System.Drawing.Size(132, 26);
            this.txb生产指令.TabIndex = 53;
            // 
            // lbInstruction
            // 
            this.lbInstruction.AutoSize = true;
            this.lbInstruction.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInstruction.Location = new System.Drawing.Point(23, 16);
            this.lbInstruction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbInstruction.Name = "lbInstruction";
            this.lbInstruction.Size = new System.Drawing.Size(72, 16);
            this.lbInstruction.TabIndex = 52;
            this.lbInstruction.Text = "生产指令";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 167);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(860, 329);
            this.dataGridView1.TabIndex = 60;
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(925, 72);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 61;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(925, 103);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 62;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // Waste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 741);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Waste";
            this.Text = "Waste";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp生产结束时间;
        private System.Windows.Forms.DateTimePicker dtp生产开始时间;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb生产指令;
        private System.Windows.Forms.Label lbInstruction;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn审核;
    }
}