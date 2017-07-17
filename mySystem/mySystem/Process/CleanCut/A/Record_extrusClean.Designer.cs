namespace WindowsFormsApplication1
{
    partial class Record_extrusClean
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp清洁日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckb夜班 = new System.Windows.Forms.CheckBox();
            this.dtp复核日期 = new System.Windows.Forms.DateTimePicker();
            this.ckb白班 = new System.Windows.Forms.CheckBox();
            this.tb复核人 = new System.Windows.Forms.TextBox();
            this.bt保存 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1150, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(546, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "吹膜机组清洁记录";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Location = new System.Drawing.Point(6, 135);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1142, 275);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(56, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "清洁日期";
            // 
            // dtp清洁日期
            // 
            this.dtp清洁日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp清洁日期.Location = new System.Drawing.Point(134, 28);
            this.dtp清洁日期.Name = "dtp清洁日期";
            this.dtp清洁日期.Size = new System.Drawing.Size(150, 26);
            this.dtp清洁日期.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(413, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "班次";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(705, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "复核人/日期";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckb夜班);
            this.groupBox2.Controls.Add(this.dtp复核日期);
            this.groupBox2.Controls.Add(this.ckb白班);
            this.groupBox2.Controls.Add(this.tb复核人);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dtp清洁日期);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(10, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1138, 65);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // ckb夜班
            // 
            this.ckb夜班.AutoSize = true;
            this.ckb夜班.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckb夜班.Location = new System.Drawing.Point(538, 33);
            this.ckb夜班.Name = "ckb夜班";
            this.ckb夜班.Size = new System.Drawing.Size(54, 18);
            this.ckb夜班.TabIndex = 7;
            this.ckb夜班.Text = "夜班";
            this.ckb夜班.UseVisualStyleBackColor = true;
            // 
            // dtp复核日期
            // 
            this.dtp复核日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp复核日期.Location = new System.Drawing.Point(921, 28);
            this.dtp复核日期.Name = "dtp复核日期";
            this.dtp复核日期.Size = new System.Drawing.Size(149, 26);
            this.dtp复核日期.TabIndex = 1;
            // 
            // ckb白班
            // 
            this.ckb白班.AutoSize = true;
            this.ckb白班.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckb白班.Location = new System.Drawing.Point(475, 33);
            this.ckb白班.Name = "ckb白班";
            this.ckb白班.Size = new System.Drawing.Size(54, 18);
            this.ckb白班.TabIndex = 6;
            this.ckb白班.Text = "白班";
            this.ckb白班.UseVisualStyleBackColor = true;
            // 
            // tb复核人
            // 
            this.tb复核人.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb复核人.Location = new System.Drawing.Point(807, 28);
            this.tb复核人.Name = "tb复核人";
            this.tb复核人.Size = new System.Drawing.Size(103, 26);
            this.tb复核人.TabIndex = 0;
            // 
            // bt保存
            // 
            this.bt保存.Location = new System.Drawing.Point(863, 427);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 23);
            this.bt保存.TabIndex = 9;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            this.bt保存.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt审核
            // 
            this.bt审核.Location = new System.Drawing.Point(969, 427);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(75, 23);
            this.bt审核.TabIndex = 10;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.button2_Click);
            // 
            // bt打印
            // 
            this.bt打印.Location = new System.Drawing.Point(1067, 425);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(75, 23);
            this.bt打印.TabIndex = 12;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(23, 424);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "测试清场记录";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Record_extrusClean
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 459);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Record_extrusClean";
            this.Text = "Record_extrusClean";
            this.Load += new System.EventHandler(this.Record_extrusClean_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp清洁日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtp复核日期;
        private System.Windows.Forms.TextBox tb复核人;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.CheckBox ckb夜班;
        private System.Windows.Forms.CheckBox ckb白班;
        private System.Windows.Forms.Button button4;
    }
}