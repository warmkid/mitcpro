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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Waste));
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp生产结束时间 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lbInstruction = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn数据审核 = new System.Windows.Forms.Button();
            this.btn提交数据审核 = new System.Windows.Forms.Button();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.txb审核员 = new System.Windows.Forms.TextBox();
            this.lb审核人 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.cmb打印机选择 = new System.Windows.Forms.ComboBox();
            this.lbl生产指令 = new System.Windows.Forms.Label();
            this.lbl生产开始时间 = new System.Windows.Forms.Label();
            this.lbl合计不良品数量 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(477, 32);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 19);
            this.label7.TabIndex = 52;
            this.label7.Text = "吹膜工序废品记录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(281, 106);
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
            this.dtp生产结束时间.Location = new System.Drawing.Point(339, 99);
            this.dtp生产结束时间.Margin = new System.Windows.Forms.Padding(4);
            this.dtp生产结束时间.Name = "dtp生产结束时间";
            this.dtp生产结束时间.Size = new System.Drawing.Size(151, 26);
            this.dtp生产结束时间.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(35, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 54;
            this.label1.Text = "生产时段";
            // 
            // lbInstruction
            // 
            this.lbInstruction.AutoSize = true;
            this.lbInstruction.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInstruction.Location = new System.Drawing.Point(37, 72);
            this.lbInstruction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbInstruction.Name = "lbInstruction";
            this.lbInstruction.Size = new System.Drawing.Size(72, 16);
            this.lbInstruction.TabIndex = 52;
            this.lbInstruction.Text = "生产指令";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 147);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(860, 231);
            this.dataGridView1.TabIndex = 60;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(761, 423);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 61;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(37, 423);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 62;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(909, 147);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(59, 90);
            this.btn添加.TabIndex = 63;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(558, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 64;
            this.label3.Text = "合计不良品数量";
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(995, 147);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(59, 94);
            this.btn删除.TabIndex = 66;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn打印
            // 
            this.btn打印.Location = new System.Drawing.Point(118, 423);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(75, 23);
            this.btn打印.TabIndex = 74;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Location = new System.Drawing.Point(850, 423);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(83, 23);
            this.btn提交审核.TabIndex = 75;
            this.btn提交审核.Text = "最后审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn数据审核
            // 
            this.btn数据审核.Location = new System.Drawing.Point(909, 284);
            this.btn数据审核.Name = "btn数据审核";
            this.btn数据审核.Size = new System.Drawing.Size(59, 90);
            this.btn数据审核.TabIndex = 76;
            this.btn数据审核.Text = "数据审核";
            this.btn数据审核.UseVisualStyleBackColor = true;
            this.btn数据审核.Click += new System.EventHandler(this.btn数据审核_Click);
            // 
            // btn提交数据审核
            // 
            this.btn提交数据审核.Location = new System.Drawing.Point(995, 284);
            this.btn提交数据审核.Name = "btn提交数据审核";
            this.btn提交数据审核.Size = new System.Drawing.Size(59, 94);
            this.btn提交数据审核.TabIndex = 77;
            this.btn提交数据审核.Text = "提交数据审核";
            this.btn提交数据审核.UseVisualStyleBackColor = true;
            this.btn提交数据审核.Click += new System.EventHandler(this.btn提交数据审核_Click);
            // 
            // btn查看日志
            // 
            this.btn查看日志.Location = new System.Drawing.Point(950, 423);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(83, 23);
            this.btn查看日志.TabIndex = 78;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // txb审核员
            // 
            this.txb审核员.Location = new System.Drawing.Point(933, 89);
            this.txb审核员.Name = "txb审核员";
            this.txb审核员.Size = new System.Drawing.Size(100, 26);
            this.txb审核员.TabIndex = 80;
            // 
            // lb审核人
            // 
            this.lb审核人.AutoSize = true;
            this.lb审核人.Location = new System.Drawing.Point(871, 95);
            this.lb审核人.Name = "lb审核人";
            this.lb审核人.Size = new System.Drawing.Size(56, 16);
            this.lb审核人.TabIndex = 79;
            this.lb审核人.Text = "审核人";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(677, 32);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 146;
            this.label角色.Text = "角色";
            // 
            // cmb打印机选择
            // 
            this.cmb打印机选择.FormattingEnabled = true;
            this.cmb打印机选择.Location = new System.Drawing.Point(204, 423);
            this.cmb打印机选择.Name = "cmb打印机选择";
            this.cmb打印机选择.Size = new System.Drawing.Size(187, 24);
            this.cmb打印机选择.TabIndex = 147;
            // 
            // lbl生产指令
            // 
            this.lbl生产指令.AutoSize = true;
            this.lbl生产指令.Location = new System.Drawing.Point(130, 72);
            this.lbl生产指令.Name = "lbl生产指令";
            this.lbl生产指令.Size = new System.Drawing.Size(72, 16);
            this.lbl生产指令.TabIndex = 148;
            this.lbl生产指令.Text = "生产指令";
            // 
            // lbl生产开始时间
            // 
            this.lbl生产开始时间.AutoSize = true;
            this.lbl生产开始时间.Location = new System.Drawing.Point(130, 106);
            this.lbl生产开始时间.Name = "lbl生产开始时间";
            this.lbl生产开始时间.Size = new System.Drawing.Size(104, 16);
            this.lbl生产开始时间.TabIndex = 149;
            this.lbl生产开始时间.Text = "生产开始时间";
            // 
            // lbl合计不良品数量
            // 
            this.lbl合计不良品数量.AutoSize = true;
            this.lbl合计不良品数量.Location = new System.Drawing.Point(677, 99);
            this.lbl合计不良品数量.Name = "lbl合计不良品数量";
            this.lbl合计不良品数量.Size = new System.Drawing.Size(120, 16);
            this.lbl合计不良品数量.TabIndex = 150;
            this.lbl合计不良品数量.Text = "合计不良品数量";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(294, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 151;
            this.pictureBox1.TabStop = false;
            // 
            // Waste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 466);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl合计不良品数量);
            this.Controls.Add(this.lbl生产开始时间);
            this.Controls.Add(this.lbl生产指令);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmb打印机选择);
            this.Controls.Add(this.dtp生产结束时间);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.txb审核员);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb审核人);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.lbInstruction);
            this.Controls.Add(this.btn提交数据审核);
            this.Controls.Add(this.btn数据审核);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Waste";
            this.Text = "吹膜工序废品记录";
            this.Load += new System.EventHandler(this.Waste_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp生产结束时间;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbInstruction;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn数据审核;
        private System.Windows.Forms.Button btn提交数据审核;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.TextBox txb审核员;
        private System.Windows.Forms.Label lb审核人;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.ComboBox cmb打印机选择;
        private System.Windows.Forms.Label lbl生产指令;
        private System.Windows.Forms.Label lbl生产开始时间;
        private System.Windows.Forms.Label lbl合计不良品数量;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}