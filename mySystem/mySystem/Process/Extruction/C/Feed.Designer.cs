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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Feed));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.cmb班次 = new System.Windows.Forms.ComboBox();
            this.lb班次 = new System.Windows.Forms.Label();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.txb审核员 = new System.Windows.Forms.TextBox();
            this.lb审核人 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label角色 = new System.Windows.Forms.Label();
            this.lbl生产指令编号 = new System.Windows.Forms.Label();
            this.cmb打印机选择 = new System.Windows.Forms.ComboBox();
            this.btn删除 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(41, 134);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(982, 192);
            this.dataGridView1.TabIndex = 53;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // btn打印
            // 
            this.btn打印.Location = new System.Drawing.Point(151, 406);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(91, 30);
            this.btn打印.TabIndex = 75;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(41, 347);
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
            this.cmb班次.Location = new System.Drawing.Point(675, 92);
            this.cmb班次.Name = "cmb班次";
            this.cmb班次.Size = new System.Drawing.Size(121, 24);
            this.cmb班次.TabIndex = 45;
            this.cmb班次.Visible = false;
            // 
            // lb班次
            // 
            this.lb班次.AutoSize = true;
            this.lb班次.Location = new System.Drawing.Point(638, 96);
            this.lb班次.Name = "lb班次";
            this.lb班次.Size = new System.Drawing.Size(40, 16);
            this.lb班次.TabIndex = 44;
            this.lb班次.Text = "班次";
            this.lb班次.Visible = false;
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.CustomFormat = "yyyy-MM-dd";
            this.dtp生产日期.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp生产日期.Location = new System.Drawing.Point(407, 92);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(110, 26);
            this.dtp生产日期.TabIndex = 43;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(735, 408);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 30);
            this.btnSave.TabIndex = 42;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(42, 408);
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
            this.label2.Location = new System.Drawing.Point(328, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "生产日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 99);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "生产指令";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(424, 40);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 19);
            this.label7.TabIndex = 51;
            this.label7.Text = "吹膜供料系统运行记录";
            // 
            // btn查看日志
            // 
            this.btn查看日志.Location = new System.Drawing.Point(931, 408);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(92, 30);
            this.btn查看日志.TabIndex = 77;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Location = new System.Drawing.Point(833, 408);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(92, 30);
            this.btn提交审核.TabIndex = 76;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // txb审核员
            // 
            this.txb审核员.Location = new System.Drawing.Point(923, 92);
            this.txb审核员.Name = "txb审核员";
            this.txb审核员.Size = new System.Drawing.Size(100, 26);
            this.txb审核员.TabIndex = 79;
            // 
            // lb审核人
            // 
            this.lb审核人.AutoSize = true;
            this.lb审核人.Location = new System.Drawing.Point(861, 97);
            this.lb审核人.Name = "lb审核人";
            this.lb审核人.Size = new System.Drawing.Size(56, 16);
            this.lb审核人.TabIndex = 78;
            this.lb审核人.Text = "审核人";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 153;
            this.pictureBox1.TabStop = false;
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(672, 42);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 156;
            this.label角色.Text = "角色";
            // 
            // lbl生产指令编号
            // 
            this.lbl生产指令编号.AutoSize = true;
            this.lbl生产指令编号.Location = new System.Drawing.Point(122, 99);
            this.lbl生产指令编号.Name = "lbl生产指令编号";
            this.lbl生产指令编号.Size = new System.Drawing.Size(104, 16);
            this.lbl生产指令编号.TabIndex = 157;
            this.lbl生产指令编号.Text = "生产指令编号";
            // 
            // cmb打印机选择
            // 
            this.cmb打印机选择.FormattingEnabled = true;
            this.cmb打印机选择.Location = new System.Drawing.Point(260, 412);
            this.cmb打印机选择.Name = "cmb打印机选择";
            this.cmb打印机选择.Size = new System.Drawing.Size(181, 24);
            this.cmb打印机选择.TabIndex = 158;
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(151, 345);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(92, 32);
            this.btn删除.TabIndex = 159;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // Feed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 463);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.cmb打印机选择);
            this.Controls.Add(this.lbl生产指令编号);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txb审核员);
            this.Controls.Add(this.lb审核人);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.cmb班次);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lb班次);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn审核);
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Feed";
            this.Text = "吹膜供料系统运行记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmb班次;
        private System.Windows.Forms.Label lb班次;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.TextBox txb审核员;
        private System.Windows.Forms.Label lb审核人;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Label lbl生产指令编号;
        private System.Windows.Forms.ComboBox cmb打印机选择;
        private System.Windows.Forms.Button btn删除;
    }
}