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
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.cmb班次 = new System.Windows.Forms.ComboBox();
            this.lb班次 = new System.Windows.Forms.Label();
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
            this.panel1.Controls.Add(this.btn打印);
            this.panel1.Controls.Add(this.btn添加);
            this.panel1.Controls.Add(this.cmb班次);
            this.panel1.Controls.Add(this.lb班次);
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
            this.panel1.Size = new System.Drawing.Size(1060, 67);
            this.panel1.TabIndex = 52;
            // 
            // btn打印
            // 
            this.btn打印.Location = new System.Drawing.Point(966, 21);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(81, 30);
            this.btn打印.TabIndex = 75;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(770, 21);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(92, 30);
            this.btn添加.TabIndex = 64;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // cmb班次
            // 
            this.cmb班次.FormattingEnabled = true;
            this.cmb班次.Location = new System.Drawing.Point(545, 23);
            this.cmb班次.Name = "cmb班次";
            this.cmb班次.Size = new System.Drawing.Size(121, 24);
            this.cmb班次.TabIndex = 45;
            // 
            // lb班次
            // 
            this.lb班次.AutoSize = true;
            this.lb班次.Location = new System.Drawing.Point(499, 25);
            this.lb班次.Name = "lb班次";
            this.lb班次.Size = new System.Drawing.Size(40, 16);
            this.lb班次.TabIndex = 44;
            this.lb班次.Text = "班次";
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.CustomFormat = "yyyy-MM-dd";
            this.dtp生产日期.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp生产日期.Location = new System.Drawing.Point(373, 22);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(110, 26);
            this.dtp生产日期.TabIndex = 43;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(672, 22);
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
            this.btn审核.Location = new System.Drawing.Point(868, 21);
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
            this.Text = "吹膜供料系统运行记录";
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
        private System.Windows.Forms.ComboBox cmb班次;
        private System.Windows.Forms.Label lb班次;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn打印;
    }
}