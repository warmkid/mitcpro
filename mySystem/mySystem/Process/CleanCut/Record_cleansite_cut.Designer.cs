﻿namespace mySystem.Process.CleanCut
{
    partial class Record_cleansite_cut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Record_cleansite_cut));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb产品规格 = new System.Windows.Forms.TextBox();
            this.tb产品批号 = new System.Windows.Forms.TextBox();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.ckb白班 = new System.Windows.Forms.CheckBox();
            this.ckb夜班 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb清场人 = new System.Windows.Forms.TextBox();
            this.tb检查人 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.bt保存 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.cb产品代码 = new System.Windows.Forms.ComboBox();
            this.ckb合格 = new System.Windows.Forms.CheckBox();
            this.ckb不合格 = new System.Windows.Forms.CheckBox();
            this.bt发送审核 = new System.Windows.Forms.Button();
            this.bt日志 = new System.Windows.Forms.Button();
            this.tb操作员备注 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bt插入查询 = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl生产指令编码 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(538, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "清场记录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "生产指令编码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(318, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "产品规格";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(498, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "产品批号";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(705, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "生产日期";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(942, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "生产班次";
            // 
            // tb产品规格
            // 
            this.tb产品规格.Location = new System.Drawing.Point(387, 19);
            this.tb产品规格.Name = "tb产品规格";
            this.tb产品规格.Size = new System.Drawing.Size(100, 23);
            this.tb产品规格.TabIndex = 7;
            this.tb产品规格.Visible = false;
            // 
            // tb产品批号
            // 
            this.tb产品批号.Location = new System.Drawing.Point(567, 63);
            this.tb产品批号.Name = "tb产品批号";
            this.tb产品批号.ReadOnly = true;
            this.tb产品批号.Size = new System.Drawing.Size(100, 23);
            this.tb产品批号.TabIndex = 8;
            this.tb产品批号.Visible = false;
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.Location = new System.Drawing.Point(774, 64);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(141, 23);
            this.dtp生产日期.TabIndex = 9;
            // 
            // ckb白班
            // 
            this.ckb白班.AutoSize = true;
            this.ckb白班.Location = new System.Drawing.Point(1011, 68);
            this.ckb白班.Name = "ckb白班";
            this.ckb白班.Size = new System.Drawing.Size(54, 18);
            this.ckb白班.TabIndex = 10;
            this.ckb白班.Text = "白班";
            this.ckb白班.UseVisualStyleBackColor = true;
            // 
            // ckb夜班
            // 
            this.ckb夜班.AutoSize = true;
            this.ckb夜班.Location = new System.Drawing.Point(1083, 68);
            this.ckb夜班.Name = "ckb夜班";
            this.ckb夜班.Size = new System.Drawing.Size(54, 18);
            this.ckb夜班.TabIndex = 11;
            this.ckb夜班.Text = "夜班";
            this.ckb夜班.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3});
            this.dataGridView1.Location = new System.Drawing.Point(3, 120);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1149, 206);
            this.dataGridView1.TabIndex = 12;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "清场项目";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "清场要点";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 700;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "清洁操作";
            this.Column3.Name = "Column3";
            this.Column3.Width = 200;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 353);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 13;
            this.label7.Text = "操作员";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(657, 353);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 14;
            this.label8.Text = "检查结果";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(495, 353);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 14);
            this.label9.TabIndex = 15;
            this.label9.Text = "审核员";
            // 
            // tb清场人
            // 
            this.tb清场人.Location = new System.Drawing.Point(99, 350);
            this.tb清场人.Name = "tb清场人";
            this.tb清场人.Size = new System.Drawing.Size(100, 23);
            this.tb清场人.TabIndex = 16;
            // 
            // tb检查人
            // 
            this.tb检查人.Location = new System.Drawing.Point(550, 347);
            this.tb检查人.Name = "tb检查人";
            this.tb检查人.ReadOnly = true;
            this.tb检查人.Size = new System.Drawing.Size(100, 23);
            this.tb检查人.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 395);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 14);
            this.label10.TabIndex = 19;
            this.label10.Text = "备注";
            // 
            // tb备注
            // 
            this.tb备注.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb备注.Location = new System.Drawing.Point(53, 392);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(1099, 54);
            this.tb备注.TabIndex = 20;
            // 
            // bt保存
            // 
            this.bt保存.Location = new System.Drawing.Point(897, 464);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 23);
            this.bt保存.TabIndex = 21;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            this.bt保存.Click += new System.EventHandler(this.bt保存_Click);
            // 
            // bt审核
            // 
            this.bt审核.Location = new System.Drawing.Point(18, 464);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(75, 23);
            this.bt审核.TabIndex = 22;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.bt审核_Click);
            // 
            // bt打印
            // 
            this.bt打印.Location = new System.Drawing.Point(368, 463);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(75, 23);
            this.bt打印.TabIndex = 23;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            this.bt打印.Click += new System.EventHandler(this.bt打印_Click);
            // 
            // cb产品代码
            // 
            this.cb产品代码.FormattingEnabled = true;
            this.cb产品代码.Location = new System.Drawing.Point(183, 26);
            this.cb产品代码.Name = "cb产品代码";
            this.cb产品代码.Size = new System.Drawing.Size(129, 22);
            this.cb产品代码.TabIndex = 24;
            this.cb产品代码.Visible = false;
            // 
            // ckb合格
            // 
            this.ckb合格.AutoSize = true;
            this.ckb合格.Location = new System.Drawing.Point(726, 352);
            this.ckb合格.Name = "ckb合格";
            this.ckb合格.Size = new System.Drawing.Size(54, 18);
            this.ckb合格.TabIndex = 25;
            this.ckb合格.Text = "合格";
            this.ckb合格.UseVisualStyleBackColor = true;
            // 
            // ckb不合格
            // 
            this.ckb不合格.AutoSize = true;
            this.ckb不合格.Location = new System.Drawing.Point(786, 352);
            this.ckb不合格.Name = "ckb不合格";
            this.ckb不合格.Size = new System.Drawing.Size(68, 18);
            this.ckb不合格.TabIndex = 26;
            this.ckb不合格.Text = "不合格";
            this.ckb不合格.UseVisualStyleBackColor = true;
            // 
            // bt发送审核
            // 
            this.bt发送审核.Location = new System.Drawing.Point(985, 464);
            this.bt发送审核.Name = "bt发送审核";
            this.bt发送审核.Size = new System.Drawing.Size(75, 23);
            this.bt发送审核.TabIndex = 27;
            this.bt发送审核.Text = "发送审核";
            this.bt发送审核.UseVisualStyleBackColor = true;
            this.bt发送审核.Click += new System.EventHandler(this.bt发送审核_Click);
            // 
            // bt日志
            // 
            this.bt日志.Location = new System.Drawing.Point(1066, 464);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(75, 23);
            this.bt日志.TabIndex = 28;
            this.bt日志.Text = "查看日志";
            this.bt日志.UseVisualStyleBackColor = true;
            this.bt日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // tb操作员备注
            // 
            this.tb操作员备注.Location = new System.Drawing.Point(300, 347);
            this.tb操作员备注.Multiline = true;
            this.tb操作员备注.Name = "tb操作员备注";
            this.tb操作员备注.Size = new System.Drawing.Size(171, 23);
            this.tb操作员备注.TabIndex = 30;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(217, 353);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 14);
            this.label11.TabIndex = 29;
            this.label11.Text = "操作员备注";
            // 
            // bt插入查询
            // 
            this.bt插入查询.Location = new System.Drawing.Point(368, 65);
            this.bt插入查询.Name = "bt插入查询";
            this.bt插入查询.Size = new System.Drawing.Size(88, 23);
            this.bt插入查询.TabIndex = 31;
            this.bt插入查询.Text = "插入/查询";
            this.bt插入查询.UseVisualStyleBackColor = true;
            this.bt插入查询.Visible = false;
            this.bt插入查询.Click += new System.EventHandler(this.bt插入查询_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label40.Location = new System.Drawing.Point(99, 468);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(91, 14);
            this.label40.TabIndex = 48;
            this.label40.Text = "选择打印机：";
            // 
            // cb打印机
            // 
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(196, 463);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(156, 22);
            this.cb打印机.TabIndex = 47;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label41.Location = new System.Drawing.Point(972, 20);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(93, 16);
            this.label41.TabIndex = 51;
            this.label41.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(1087, 20);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 50;
            this.label角色.Text = "角色";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, -2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 87;
            this.pictureBox1.TabStop = false;
            // 
            // lbl生产指令编码
            // 
            this.lbl生产指令编码.AutoSize = true;
            this.lbl生产指令编码.Location = new System.Drawing.Point(142, 68);
            this.lbl生产指令编码.Name = "lbl生产指令编码";
            this.lbl生产指令编码.Size = new System.Drawing.Size(91, 14);
            this.lbl生产指令编码.TabIndex = 88;
            this.lbl生产指令编码.Text = "生产指令编码";
            // 
            // Record_cleansite_cut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 499);
            this.Controls.Add(this.lbl生产指令编码);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.bt插入查询);
            this.Controls.Add(this.tb操作员备注);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.bt发送审核);
            this.Controls.Add(this.ckb不合格);
            this.Controls.Add(this.ckb合格);
            this.Controls.Add(this.cb产品代码);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb检查人);
            this.Controls.Add(this.tb清场人);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ckb夜班);
            this.Controls.Add(this.ckb白班);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.tb产品批号);
            this.Controls.Add(this.tb产品规格);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Record_cleansite_cut";
            this.Text = "Record_cleansite_cut";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Record_cleansite_cut_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb产品规格;
        private System.Windows.Forms.TextBox tb产品批号;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.CheckBox ckb白班;
        private System.Windows.Forms.CheckBox ckb夜班;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb清场人;
        private System.Windows.Forms.TextBox tb检查人;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.ComboBox cb产品代码;
        private System.Windows.Forms.CheckBox ckb合格;
        private System.Windows.Forms.CheckBox ckb不合格;
        private System.Windows.Forms.Button bt发送审核;
        private System.Windows.Forms.Button bt日志;
        private System.Windows.Forms.TextBox tb操作员备注;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bt插入查询;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl生产指令编码;
    }
}