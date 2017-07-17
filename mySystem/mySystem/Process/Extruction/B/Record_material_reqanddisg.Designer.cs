namespace mySystem.Extruction.Process
{
    partial class Record_material_reqanddisg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt保存 = new System.Windows.Forms.Button();
            this.bt退料审核 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tb重量 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb数量 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb退料量 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tb退料操作人 = new System.Windows.Forms.TextBox();
            this.tb退料审核人 = new System.Windows.Forms.TextBox();
            this.bt添加 = new System.Windows.Forms.Button();
            this.cB物料代码 = new System.Windows.Forms.ComboBox();
            this.bt打印 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(348, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "SOP-MFG-301-R14A吹膜工序领料退料记录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "物料代码：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(2, 78);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(941, 248);
            this.dataGridView1.TabIndex = 11;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_Endedit);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // bt保存
            // 
            this.bt保存.Location = new System.Drawing.Point(680, 417);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 23);
            this.bt保存.TabIndex = 12;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            this.bt保存.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt退料审核
            // 
            this.bt退料审核.Location = new System.Drawing.Point(778, 417);
            this.bt退料审核.Name = "bt退料审核";
            this.bt退料审核.Size = new System.Drawing.Size(75, 23);
            this.bt退料审核.TabIndex = 13;
            this.bt退料审核.Text = "审核";
            this.bt退料审核.UseVisualStyleBackColor = true;
            this.bt退料审核.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(156, 342);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 14;
            this.label7.Text = "合计：";
            // 
            // tb重量
            // 
            this.tb重量.Location = new System.Drawing.Point(305, 339);
            this.tb重量.Name = "tb重量";
            this.tb重量.ReadOnly = true;
            this.tb重量.Size = new System.Drawing.Size(100, 23);
            this.tb重量.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 342);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 14);
            this.label8.TabIndex = 16;
            this.label8.Text = "重量：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(466, 342);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 14);
            this.label9.TabIndex = 17;
            this.label9.Text = "数量：";
            // 
            // tb数量
            // 
            this.tb数量.Location = new System.Drawing.Point(522, 339);
            this.tb数量.Name = "tb数量";
            this.tb数量.ReadOnly = true;
            this.tb数量.Size = new System.Drawing.Size(100, 23);
            this.tb数量.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 387);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 19;
            this.label10.Text = "退料量：";
            // 
            // tb退料量
            // 
            this.tb退料量.Location = new System.Drawing.Point(78, 381);
            this.tb退料量.Name = "tb退料量";
            this.tb退料量.Size = new System.Drawing.Size(100, 23);
            this.tb退料量.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(239, 387);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 14);
            this.label11.TabIndex = 21;
            this.label11.Text = "退料操作人：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(506, 387);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 14);
            this.label12.TabIndex = 22;
            this.label12.Text = "退料审核人：";
            // 
            // tb退料操作人
            // 
            this.tb退料操作人.Location = new System.Drawing.Point(334, 381);
            this.tb退料操作人.Name = "tb退料操作人";
            this.tb退料操作人.Size = new System.Drawing.Size(100, 23);
            this.tb退料操作人.TabIndex = 23;
            // 
            // tb退料审核人
            // 
            this.tb退料审核人.Location = new System.Drawing.Point(592, 381);
            this.tb退料审核人.Name = "tb退料审核人";
            this.tb退料审核人.Size = new System.Drawing.Size(100, 23);
            this.tb退料审核人.TabIndex = 24;
            // 
            // bt添加
            // 
            this.bt添加.Location = new System.Drawing.Point(26, 338);
            this.bt添加.Name = "bt添加";
            this.bt添加.Size = new System.Drawing.Size(75, 23);
            this.bt添加.TabIndex = 25;
            this.bt添加.Text = "添加";
            this.bt添加.UseVisualStyleBackColor = true;
            this.bt添加.Click += new System.EventHandler(this.button3_Click);
            // 
            // cB物料代码
            // 
            this.cB物料代码.FormattingEnabled = true;
            this.cB物料代码.Location = new System.Drawing.Point(125, 37);
            this.cB物料代码.Name = "cB物料代码";
            this.cB物料代码.Size = new System.Drawing.Size(121, 22);
            this.cB物料代码.TabIndex = 26;
            this.cB物料代码.SelectedIndexChanged += new System.EventHandler(this.cB物料代码_SelectedIndexChanged);
            // 
            // bt打印
            // 
            this.bt打印.Location = new System.Drawing.Point(863, 417);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(75, 23);
            this.bt打印.TabIndex = 27;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            this.bt打印.Click += new System.EventHandler(this.bt打印_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 426);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Record_material_reqanddisg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 452);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.cB物料代码);
            this.Controls.Add(this.bt添加);
            this.Controls.Add(this.tb退料审核人);
            this.Controls.Add(this.tb退料操作人);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tb退料量);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb数量);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb重量);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bt退料审核);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Record_material_reqanddisg";
            this.Text = "吹膜工序领料退料记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Button bt退料审核;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb重量;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb数量;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb退料量;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb退料操作人;
        private System.Windows.Forms.TextBox tb退料审核人;
        private System.Windows.Forms.Button bt添加;
        private System.Windows.Forms.ComboBox cB物料代码;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button button1;
    }
}