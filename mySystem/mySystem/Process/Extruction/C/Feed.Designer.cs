namespace mySystem.Process.Extruction.C
{
    partial class Feed
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
            this.ckb夜班 = new System.Windows.Forms.CheckBox();
            this.ckb白班 = new System.Windows.Forms.CheckBox();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn审核 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txb生产指令编号 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(424, 40);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 19);
            this.label7.TabIndex = 51;
            this.label7.Text = "吹膜供料系统运行记录";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckb夜班);
            this.panel1.Controls.Add(this.ckb白班);
            this.panel1.Controls.Add(this.dtp生产日期);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btn审核);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txb生产指令编号);
            this.panel1.Location = new System.Drawing.Point(53, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(958, 67);
            this.panel1.TabIndex = 52;
            // 
            // ckb夜班
            // 
            this.ckb夜班.AutoSize = true;
            this.ckb夜班.Location = new System.Drawing.Point(620, 39);
            this.ckb夜班.Name = "ckb夜班";
            this.ckb夜班.Size = new System.Drawing.Size(59, 20);
            this.ckb夜班.TabIndex = 45;
            this.ckb夜班.Text = "夜班";
            this.ckb夜班.UseVisualStyleBackColor = true;
            this.ckb夜班.CheckedChanged += new System.EventHandler(this.ckb夜班_CheckedChanged);
            // 
            // ckb白班
            // 
            this.ckb白班.AutoSize = true;
            this.ckb白班.Location = new System.Drawing.Point(620, 13);
            this.ckb白班.Name = "ckb白班";
            this.ckb白班.Size = new System.Drawing.Size(59, 20);
            this.ckb白班.TabIndex = 44;
            this.ckb白班.Text = "白班";
            this.ckb白班.UseVisualStyleBackColor = true;
            this.ckb白班.CheckedChanged += new System.EventHandler(this.ckb白班_CheckedChanged);
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.CustomFormat = "yyyy-MM-dd";
            this.dtp生产日期.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp生产日期.Location = new System.Drawing.Point(373, 22);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(200, 26);
            this.dtp生产日期.TabIndex = 43;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(747, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 30);
            this.btnSave.TabIndex = 42;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(-6, 553);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 30;
            this.label11.Text = "生产指令";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(74, 550);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 26);
            this.textBox1.TabIndex = 29;
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(845, 18);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(92, 30);
            this.btn审核.TabIndex = 32;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "生产日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "生产指令";
            // 
            // txb生产指令编号
            // 
            this.txb生产指令编号.Location = new System.Drawing.Point(94, 20);
            this.txb生产指令编号.Margin = new System.Windows.Forms.Padding(4);
            this.txb生产指令编号.Name = "txb生产指令编号";
            this.txb生产指令编号.Size = new System.Drawing.Size(172, 26);
            this.txb生产指令编号.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(53, 184);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(921, 192);
            this.dataGridView1.TabIndex = 53;
            // 
            // Feed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 741);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Feed";
            this.Text = "Feed";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb生产指令编号;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox ckb夜班;
        private System.Windows.Forms.CheckBox ckb白班;
    }
}