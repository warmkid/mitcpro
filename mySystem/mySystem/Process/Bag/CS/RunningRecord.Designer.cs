namespace mySystem.Process.Bag
{
    partial class RunningRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunningRecord));
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label18 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label40 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.bt打印 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt日志 = new System.Windows.Forms.Button();
            this.bt提交审核 = new System.Windows.Forms.Button();
            this.bt保存 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bt删除 = new System.Windows.Forms.Button();
            this.bt添加 = new System.Windows.Forms.Button();
            this.tb生产指令编码 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.btn数据审核 = new System.Windows.Forms.Button();
            this.btn提交数据审核 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(384, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "2# 制袋机运行记录";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 121);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(953, 263);
            this.dataGridView1.TabIndex = 5;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 406);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 16);
            this.label18.TabIndex = 29;
            this.label18.Text = "审核员";
            // 
            // tb审核员
            // 
            this.tb审核员.Location = new System.Drawing.Point(79, 400);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.ReadOnly = true;
            this.tb审核员.Size = new System.Drawing.Size(110, 26);
            this.tb审核员.TabIndex = 30;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(247, 403);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(72, 16);
            this.label19.TabIndex = 31;
            this.label19.Text = "审核日期";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Location = new System.Drawing.Point(325, 400);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(154, 26);
            this.dtp审核日期.TabIndex = 32;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label40.Location = new System.Drawing.Point(107, 452);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(91, 14);
            this.label40.TabIndex = 36;
            this.label40.Text = "选择打印机：";
            // 
            // cb打印机
            // 
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(200, 449);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(152, 24);
            this.cb打印机.TabIndex = 35;
            // 
            // bt打印
            // 
            this.bt打印.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt打印.Location = new System.Drawing.Point(358, 448);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(75, 23);
            this.bt打印.TabIndex = 34;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            this.bt打印.Click += new System.EventHandler(this.bt打印_Click);
            // 
            // bt审核
            // 
            this.bt审核.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt审核.Location = new System.Drawing.Point(12, 448);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(75, 23);
            this.bt审核.TabIndex = 33;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.bt审核_Click);
            // 
            // bt日志
            // 
            this.bt日志.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt日志.Location = new System.Drawing.Point(887, 448);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(75, 23);
            this.bt日志.TabIndex = 39;
            this.bt日志.Text = "查看日志";
            this.bt日志.UseVisualStyleBackColor = true;
            this.bt日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // bt提交审核
            // 
            this.bt提交审核.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt提交审核.Location = new System.Drawing.Point(794, 448);
            this.bt提交审核.Name = "bt提交审核";
            this.bt提交审核.Size = new System.Drawing.Size(75, 23);
            this.bt提交审核.TabIndex = 38;
            this.bt提交审核.Text = "最后审核";
            this.bt提交审核.UseVisualStyleBackColor = true;
            this.bt提交审核.Click += new System.EventHandler(this.bt提交审核_Click);
            // 
            // bt保存
            // 
            this.bt保存.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt保存.Location = new System.Drawing.Point(700, 448);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 23);
            this.bt保存.TabIndex = 37;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            this.bt保存.Click += new System.EventHandler(this.bt保存_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(771, 21);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(109, 19);
            this.label20.TabIndex = 41;
            this.label20.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(886, 21);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(49, 19);
            this.label角色.TabIndex = 40;
            this.label角色.Text = "角色";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(154, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // bt删除
            // 
            this.bt删除.Location = new System.Drawing.Point(890, 401);
            this.bt删除.Name = "bt删除";
            this.bt删除.Size = new System.Drawing.Size(63, 23);
            this.bt删除.TabIndex = 44;
            this.bt删除.Text = "删除";
            this.bt删除.UseVisualStyleBackColor = true;
            this.bt删除.Click += new System.EventHandler(this.bt删除_Click);
            // 
            // bt添加
            // 
            this.bt添加.Location = new System.Drawing.Point(805, 401);
            this.bt添加.Name = "bt添加";
            this.bt添加.Size = new System.Drawing.Size(63, 23);
            this.bt添加.TabIndex = 43;
            this.bt添加.Text = "添加";
            this.bt添加.UseVisualStyleBackColor = true;
            this.bt添加.Click += new System.EventHandler(this.bt添加_Click);
            // 
            // tb生产指令编码
            // 
            this.tb生产指令编码.Location = new System.Drawing.Point(775, 67);
            this.tb生产指令编码.Name = "tb生产指令编码";
            this.tb生产指令编码.Size = new System.Drawing.Size(160, 26);
            this.tb生产指令编码.TabIndex = 45;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(665, 73);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(104, 16);
            this.label21.TabIndex = 46;
            this.label21.Text = "生产指令编码";
            // 
            // btn数据审核
            // 
            this.btn数据审核.Location = new System.Drawing.Point(668, 402);
            this.btn数据审核.Name = "btn数据审核";
            this.btn数据审核.Size = new System.Drawing.Size(101, 23);
            this.btn数据审核.TabIndex = 48;
            this.btn数据审核.Text = "数据审核";
            this.btn数据审核.UseVisualStyleBackColor = true;
            this.btn数据审核.Click += new System.EventHandler(this.btn数据审核_Click);
            // 
            // btn提交数据审核
            // 
            this.btn提交数据审核.Location = new System.Drawing.Point(523, 402);
            this.btn提交数据审核.Name = "btn提交数据审核";
            this.btn提交数据审核.Size = new System.Drawing.Size(123, 23);
            this.btn提交数据审核.TabIndex = 47;
            this.btn提交数据审核.Text = "提交数据审核";
            this.btn提交数据审核.UseVisualStyleBackColor = true;
            this.btn提交数据审核.Click += new System.EventHandler(this.btn提交数据审核_Click);
            // 
            // RunningRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 484);
            this.Controls.Add(this.btn数据审核);
            this.Controls.Add(this.btn提交数据审核);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.tb生产指令编码);
            this.Controls.Add(this.bt删除);
            this.Controls.Add(this.bt添加);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.bt提交审核);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RunningRecord";
            this.Text = "RunningRecord";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt日志;
        private System.Windows.Forms.Button bt提交审核;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bt删除;
        private System.Windows.Forms.Button bt添加;
        private System.Windows.Forms.TextBox tb生产指令编码;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btn数据审核;
        private System.Windows.Forms.Button btn提交数据审核;
    }
}