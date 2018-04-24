namespace mySystem.Forms
{
    partial class 交接班记录
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(交接班记录));
            this.btn上一条记录 = new System.Windows.Forms.Button();
            this.lbl生产指令编号 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmb打印机选择 = new System.Windows.Forms.ComboBox();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.dtp夜班交接班时间 = new System.Windows.Forms.DateTimePicker();
            this.btn打印 = new System.Windows.Forms.Button();
            this.夜班交接班时间 = new System.Windows.Forms.Label();
            this.txb夜班接班员 = new System.Windows.Forms.TextBox();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn保存 = new System.Windows.Forms.Button();
            this.lb夜班接班员 = new System.Windows.Forms.Label();
            this.txb夜班交班员 = new System.Windows.Forms.TextBox();
            this.lb夜班交班员 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txb夜班异常情况处理 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lb夜班异常情况处理 = new System.Windows.Forms.Label();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.dtp白班交接班时间 = new System.Windows.Forms.DateTimePicker();
            this.lb生产日期 = new System.Windows.Forms.Label();
            this.lb白班交接班时间 = new System.Windows.Forms.Label();
            this.txb白班接班员 = new System.Windows.Forms.TextBox();
            this.lb生产指令编号 = new System.Windows.Forms.Label();
            this.lb白班接班员 = new System.Windows.Forms.Label();
            this.lb白班异常情况处理 = new System.Windows.Forms.Label();
            this.txb白班交班员 = new System.Windows.Forms.TextBox();
            this.txb白班异常情况处理 = new System.Windows.Forms.TextBox();
            this.lb白班交班人 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn上一条记录
            // 
            this.btn上一条记录.Location = new System.Drawing.Point(649, 97);
            this.btn上一条记录.Name = "btn上一条记录";
            this.btn上一条记录.Size = new System.Drawing.Size(117, 23);
            this.btn上一条记录.TabIndex = 196;
            this.btn上一条记录.Text = "上一条记录";
            this.btn上一条记录.UseVisualStyleBackColor = true;
            this.btn上一条记录.Visible = false;
            // 
            // lbl生产指令编号
            // 
            this.lbl生产指令编号.AutoSize = true;
            this.lbl生产指令编号.Location = new System.Drawing.Point(187, 104);
            this.lbl生产指令编号.Name = "lbl生产指令编号";
            this.lbl生产指令编号.Size = new System.Drawing.Size(77, 12);
            this.lbl生产指令编号.TabIndex = 195;
            this.lbl生产指令编号.Text = "生产指令编号";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(646, 35);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 194;
            this.label角色.Text = "角色";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(248, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 193;
            this.pictureBox1.TabStop = false;
            // 
            // cmb打印机选择
            // 
            this.cmb打印机选择.FormattingEnabled = true;
            this.cmb打印机选择.Location = new System.Drawing.Point(135, 532);
            this.cmb打印机选择.Name = "cmb打印机选择";
            this.cmb打印机选择.Size = new System.Drawing.Size(211, 20);
            this.cmb打印机选择.TabIndex = 192;
            // 
            // btn查看日志
            // 
            this.btn查看日志.Location = new System.Drawing.Point(1013, 533);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(86, 23);
            this.btn查看日志.TabIndex = 191;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Location = new System.Drawing.Point(921, 533);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(86, 23);
            this.btn提交审核.TabIndex = 190;
            this.btn提交审核.Text = "交班";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // dtp夜班交接班时间
            // 
            this.dtp夜班交接班时间.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtp夜班交接班时间.Location = new System.Drawing.Point(934, 484);
            this.dtp夜班交接班时间.Name = "dtp夜班交接班时间";
            this.dtp夜班交接班时间.Size = new System.Drawing.Size(165, 21);
            this.dtp夜班交接班时间.TabIndex = 186;
            // 
            // btn打印
            // 
            this.btn打印.Location = new System.Drawing.Point(367, 532);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(81, 26);
            this.btn打印.TabIndex = 189;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            // 
            // 夜班交接班时间
            // 
            this.夜班交接班时间.AutoSize = true;
            this.夜班交接班时间.Location = new System.Drawing.Point(808, 491);
            this.夜班交接班时间.Name = "夜班交接班时间";
            this.夜班交接班时间.Size = new System.Drawing.Size(89, 12);
            this.夜班交接班时间.TabIndex = 185;
            this.夜班交接班时间.Text = "夜班交接班时间";
            // 
            // txb夜班接班员
            // 
            this.txb夜班接班员.Location = new System.Drawing.Point(902, 234);
            this.txb夜班接班员.Name = "txb夜班接班员";
            this.txb夜班接班员.Size = new System.Drawing.Size(100, 21);
            this.txb夜班接班员.TabIndex = 184;
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(45, 533);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 26);
            this.btn审核.TabIndex = 188;
            this.btn审核.Text = "接班";
            this.btn审核.UseVisualStyleBackColor = true;
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(840, 533);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 187;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // lb夜班接班员
            // 
            this.lb夜班接班员.AutoSize = true;
            this.lb夜班接班员.Location = new System.Drawing.Point(808, 237);
            this.lb夜班接班员.Name = "lb夜班接班员";
            this.lb夜班接班员.Size = new System.Drawing.Size(65, 12);
            this.lb夜班接班员.TabIndex = 183;
            this.lb夜班接班员.Text = "夜班接班员";
            // 
            // txb夜班交班员
            // 
            this.txb夜班交班员.Location = new System.Drawing.Point(902, 422);
            this.txb夜班交班员.Name = "txb夜班交班员";
            this.txb夜班交班员.Size = new System.Drawing.Size(100, 21);
            this.txb夜班交班员.TabIndex = 182;
            // 
            // lb夜班交班员
            // 
            this.lb夜班交班员.AutoSize = true;
            this.lb夜班交班员.Location = new System.Drawing.Point(808, 428);
            this.lb夜班交班员.Name = "lb夜班交班员";
            this.lb夜班交班员.Size = new System.Drawing.Size(65, 12);
            this.lb夜班交班员.TabIndex = 181;
            this.lb夜班交班员.Text = "夜班交班员";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(31, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(707, 361);
            this.dataGridView1.TabIndex = 170;
            // 
            // txb夜班异常情况处理
            // 
            this.txb夜班异常情况处理.Location = new System.Drawing.Point(811, 323);
            this.txb夜班异常情况处理.Multiline = true;
            this.txb夜班异常情况处理.Name = "txb夜班异常情况处理";
            this.txb夜班异常情况处理.Size = new System.Drawing.Size(288, 69);
            this.txb夜班异常情况处理.TabIndex = 180;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(413, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 19);
            this.label7.TabIndex = 169;
            this.label7.Text = "吹膜岗位交接班记录";
            // 
            // lb夜班异常情况处理
            // 
            this.lb夜班异常情况处理.AutoSize = true;
            this.lb夜班异常情况处理.Location = new System.Drawing.Point(808, 304);
            this.lb夜班异常情况处理.Name = "lb夜班异常情况处理";
            this.lb夜班异常情况处理.Size = new System.Drawing.Size(101, 12);
            this.lb夜班异常情况处理.TabIndex = 179;
            this.lb夜班异常情况处理.Text = "夜班异常情况处理";
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp生产日期.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp生产日期.Location = new System.Drawing.Point(473, 97);
            this.dtp生产日期.Margin = new System.Windows.Forms.Padding(4);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(129, 26);
            this.dtp生产日期.TabIndex = 168;
            // 
            // dtp白班交接班时间
            // 
            this.dtp白班交接班时间.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtp白班交接班时间.Location = new System.Drawing.Point(934, 267);
            this.dtp白班交接班时间.Name = "dtp白班交接班时间";
            this.dtp白班交接班时间.Size = new System.Drawing.Size(165, 21);
            this.dtp白班交接班时间.TabIndex = 178;
            // 
            // lb生产日期
            // 
            this.lb生产日期.AutoSize = true;
            this.lb生产日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb生产日期.Location = new System.Drawing.Point(395, 104);
            this.lb生产日期.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb生产日期.Name = "lb生产日期";
            this.lb生产日期.Size = new System.Drawing.Size(72, 16);
            this.lb生产日期.TabIndex = 167;
            this.lb生产日期.Text = "生产日期";
            // 
            // lb白班交接班时间
            // 
            this.lb白班交接班时间.AutoSize = true;
            this.lb白班交接班时间.Location = new System.Drawing.Point(808, 274);
            this.lb白班交接班时间.Name = "lb白班交接班时间";
            this.lb白班交接班时间.Size = new System.Drawing.Size(89, 12);
            this.lb白班交接班时间.TabIndex = 177;
            this.lb白班交接班时间.Text = "白班交接班时间";
            // 
            // txb白班接班员
            // 
            this.txb白班接班员.Location = new System.Drawing.Point(902, 453);
            this.txb白班接班员.Name = "txb白班接班员";
            this.txb白班接班员.Size = new System.Drawing.Size(100, 21);
            this.txb白班接班员.TabIndex = 176;
            // 
            // lb生产指令编号
            // 
            this.lb生产指令编号.AutoSize = true;
            this.lb生产指令编号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb生产指令编号.Location = new System.Drawing.Point(75, 104);
            this.lb生产指令编号.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb生产指令编号.Name = "lb生产指令编号";
            this.lb生产指令编号.Size = new System.Drawing.Size(104, 16);
            this.lb生产指令编号.TabIndex = 166;
            this.lb生产指令编号.Text = "生产指令编号";
            // 
            // lb白班接班员
            // 
            this.lb白班接班员.AutoSize = true;
            this.lb白班接班员.Location = new System.Drawing.Point(808, 456);
            this.lb白班接班员.Name = "lb白班接班员";
            this.lb白班接班员.Size = new System.Drawing.Size(65, 12);
            this.lb白班接班员.TabIndex = 175;
            this.lb白班接班员.Text = "白班接班员";
            // 
            // lb白班异常情况处理
            // 
            this.lb白班异常情况处理.AutoSize = true;
            this.lb白班异常情况处理.Location = new System.Drawing.Point(808, 87);
            this.lb白班异常情况处理.Name = "lb白班异常情况处理";
            this.lb白班异常情况处理.Size = new System.Drawing.Size(101, 12);
            this.lb白班异常情况处理.TabIndex = 171;
            this.lb白班异常情况处理.Text = "白班异常情况处理";
            // 
            // txb白班交班员
            // 
            this.txb白班交班员.Location = new System.Drawing.Point(902, 202);
            this.txb白班交班员.Name = "txb白班交班员";
            this.txb白班交班员.Size = new System.Drawing.Size(100, 21);
            this.txb白班交班员.TabIndex = 174;
            // 
            // txb白班异常情况处理
            // 
            this.txb白班异常情况处理.Location = new System.Drawing.Point(811, 106);
            this.txb白班异常情况处理.Multiline = true;
            this.txb白班异常情况处理.Name = "txb白班异常情况处理";
            this.txb白班异常情况处理.Size = new System.Drawing.Size(288, 80);
            this.txb白班异常情况处理.TabIndex = 172;
            // 
            // lb白班交班人
            // 
            this.lb白班交班人.AutoSize = true;
            this.lb白班交班人.Location = new System.Drawing.Point(808, 210);
            this.lb白班交班人.Name = "lb白班交班人";
            this.lb白班交班人.Size = new System.Drawing.Size(65, 12);
            this.lb白班交班人.TabIndex = 173;
            this.lb白班交班人.Text = "白班交班员";
            // 
            // 交接班记录
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 591);
            this.Controls.Add(this.btn上一条记录);
            this.Controls.Add(this.lbl生产指令编号);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cmb打印机选择);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.dtp夜班交接班时间);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.夜班交接班时间);
            this.Controls.Add(this.txb夜班接班员);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.lb夜班接班员);
            this.Controls.Add(this.txb夜班交班员);
            this.Controls.Add(this.lb夜班交班员);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txb夜班异常情况处理);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lb夜班异常情况处理);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.dtp白班交接班时间);
            this.Controls.Add(this.lb生产日期);
            this.Controls.Add(this.lb白班交接班时间);
            this.Controls.Add(this.txb白班接班员);
            this.Controls.Add(this.lb生产指令编号);
            this.Controls.Add(this.lb白班接班员);
            this.Controls.Add(this.lb白班异常情况处理);
            this.Controls.Add(this.txb白班交班员);
            this.Controls.Add(this.txb白班异常情况处理);
            this.Controls.Add(this.lb白班交班人);
            this.Name = "交接班记录";
            this.Text = "交接班记录";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn上一条记录;
        private System.Windows.Forms.Label lbl生产指令编号;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmb打印机选择;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.DateTimePicker dtp夜班交接班时间;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Label 夜班交接班时间;
        private System.Windows.Forms.TextBox txb夜班接班员;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Label lb夜班接班员;
        private System.Windows.Forms.TextBox txb夜班交班员;
        private System.Windows.Forms.Label lb夜班交班员;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txb夜班异常情况处理;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lb夜班异常情况处理;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.DateTimePicker dtp白班交接班时间;
        private System.Windows.Forms.Label lb生产日期;
        private System.Windows.Forms.Label lb白班交接班时间;
        private System.Windows.Forms.TextBox txb白班接班员;
        private System.Windows.Forms.Label lb生产指令编号;
        private System.Windows.Forms.Label lb白班接班员;
        private System.Windows.Forms.Label lb白班异常情况处理;
        private System.Windows.Forms.TextBox txb白班交班员;
        private System.Windows.Forms.TextBox txb白班异常情况处理;
        private System.Windows.Forms.Label lb白班交班人;
    }
}