namespace mySystem.Process.CleanCut
{
    partial class CleanCut_CheckBeforePower
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanCut_CheckBeforePower));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.NeightcheckBox = new System.Windows.Forms.CheckBox();
            this.DatecheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.RunRecordView = new System.Windows.Forms.DataGridView();
            this.生产时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.分切速度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自动张力设定 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自动张力显示 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.张力输出显示 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.操作人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.checkTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.recordTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.checkerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.recorderBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckBeforePowerView = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认内容 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认结果 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunRecordView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(210, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1066, 528);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 71;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(974, 528);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 70;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // NeightcheckBox
            // 
            this.NeightcheckBox.AutoSize = true;
            this.NeightcheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.NeightcheckBox.Location = new System.Drawing.Point(1095, 66);
            this.NeightcheckBox.Name = "NeightcheckBox";
            this.NeightcheckBox.Size = new System.Drawing.Size(59, 20);
            this.NeightcheckBox.TabIndex = 69;
            this.NeightcheckBox.Text = "夜班";
            this.NeightcheckBox.UseVisualStyleBackColor = true;
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.DatecheckBox.Location = new System.Drawing.Point(1032, 66);
            this.DatecheckBox.Name = "DatecheckBox";
            this.DatecheckBox.Size = new System.Drawing.Size(59, 20);
            this.DatecheckBox.TabIndex = 68;
            this.DatecheckBox.Text = "白班";
            this.DatecheckBox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F);
            this.label10.Location = new System.Drawing.Point(939, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 67;
            this.label10.Text = "生产班次：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("SimSun", 12F);
            this.dateTimePicker1.Location = new System.Drawing.Point(662, 60);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker1.TabIndex = 66;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F);
            this.label9.Location = new System.Drawing.Point(581, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 65;
            this.label9.Text = "生产日期：";
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox5.Location = new System.Drawing.Point(136, 60);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(196, 26);
            this.textBox5.TabIndex = 64;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(20, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 63;
            this.label8.Text = "生产指令编码：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(592, 383);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 60;
            this.label7.Text = "备注：";
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox4.Location = new System.Drawing.Point(654, 383);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(495, 26);
            this.textBox4.TabIndex = 59;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox3.Location = new System.Drawing.Point(55, 429);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(495, 26);
            this.textBox3.TabIndex = 58;
            this.textBox3.Text = "如若开机确认不正常或不符合打请在此填写说明。";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox2.Location = new System.Drawing.Point(372, 375);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 57;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox1.Location = new System.Drawing.Point(154, 375);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(20, 383);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(496, 16);
            this.label6.TabIndex = 55;
            this.label6.Text = "填写： 车间温度：             ℃，车间湿度：             ﹪。\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(21, 408);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(488, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "注: 开机确认结果正常或符合打“√”；不正常或不符合取消勾选。\r\n";
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(1066, 423);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 53;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(974, 423);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 52;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // RunRecordView
            // 
            this.RunRecordView.AllowUserToAddRows = false;
            this.RunRecordView.AllowUserToDeleteRows = false;
            this.RunRecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RunRecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.生产时间,
            this.分切速度,
            this.自动张力设定,
            this.自动张力显示,
            this.张力输出显示,
            this.操作人});
            this.RunRecordView.Location = new System.Drawing.Point(595, 102);
            this.RunRecordView.Name = "RunRecordView";
            this.RunRecordView.RowTemplate.Height = 23;
            this.RunRecordView.Size = new System.Drawing.Size(554, 270);
            this.RunRecordView.TabIndex = 27;
            // 
            // 生产时间
            // 
            this.生产时间.HeaderText = "生产时间";
            this.生产时间.Name = "生产时间";
            // 
            // 分切速度
            // 
            this.分切速度.HeaderText = "分切速度";
            this.分切速度.Name = "分切速度";
            // 
            // 自动张力设定
            // 
            this.自动张力设定.HeaderText = "自动张力设定";
            this.自动张力设定.Name = "自动张力设定";
            // 
            // 自动张力显示
            // 
            this.自动张力显示.HeaderText = "自动张力显示";
            this.自动张力显示.Name = "自动张力显示";
            // 
            // 张力输出显示
            // 
            this.张力输出显示.HeaderText = "张力输出显示";
            this.张力输出显示.Name = "张力输出显示";
            // 
            // 操作人
            // 
            this.操作人.HeaderText = "操作人";
            this.操作人.Name = "操作人";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(434, 9);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(269, 19);
            this.Title.TabIndex = 26;
            this.Title.Text = "清洁分切开机确认及运行记录";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkTimePicker
            // 
            this.checkTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkTimePicker.Location = new System.Drawing.Point(948, 489);
            this.checkTimePicker.Name = "checkTimePicker";
            this.checkTimePicker.Size = new System.Drawing.Size(200, 26);
            this.checkTimePicker.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(867, 496);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "审核日期：";
            // 
            // recordTimePicker
            // 
            this.recordTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.recordTimePicker.Location = new System.Drawing.Point(325, 488);
            this.recordTimePicker.Name = "recordTimePicker";
            this.recordTimePicker.Size = new System.Drawing.Size(200, 26);
            this.recordTimePicker.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(244, 495);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "确认日期：";
            // 
            // checkerBox
            // 
            this.checkerBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkerBox.Location = new System.Drawing.Point(709, 492);
            this.checkerBox.Name = "checkerBox";
            this.checkerBox.Size = new System.Drawing.Size(100, 26);
            this.checkerBox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(641, 499);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "审核人：";
            // 
            // recorderBox
            // 
            this.recorderBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.recorderBox.Location = new System.Drawing.Point(80, 488);
            this.recorderBox.Name = "recorderBox";
            this.recorderBox.Size = new System.Drawing.Size(100, 26);
            this.recorderBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(12, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "确认人：";
            // 
            // CheckBeforePowerView
            // 
            this.CheckBeforePowerView.AllowUserToAddRows = false;
            this.CheckBeforePowerView.AllowUserToDeleteRows = false;
            this.CheckBeforePowerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheckBeforePowerView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.确认项目,
            this.确认内容,
            this.确认结果});
            this.CheckBeforePowerView.Location = new System.Drawing.Point(21, 102);
            this.CheckBeforePowerView.Name = "CheckBeforePowerView";
            this.CheckBeforePowerView.RowTemplate.Height = 23;
            this.CheckBeforePowerView.Size = new System.Drawing.Size(529, 270);
            this.CheckBeforePowerView.TabIndex = 17;
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 确认项目
            // 
            this.确认项目.HeaderText = "确认项目";
            this.确认项目.Name = "确认项目";
            // 
            // 确认内容
            // 
            this.确认内容.HeaderText = "确认内容";
            this.确认内容.Name = "确认内容";
            // 
            // 确认结果
            // 
            this.确认结果.HeaderText = "确认结果";
            this.确认结果.Name = "确认结果";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(14, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 379);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(584, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(574, 379);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            // 
            // CleanCut_CheckBeforePower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 569);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.NeightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.RunRecordView);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.checkTimePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.recordTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkerBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.recorderBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBeforePowerView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CleanCut_CheckBeforePower";
            this.Text = "CleanCut_CheckBeforePower";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunRecordView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker checkTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker recordTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox checkerBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox recorderBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView CheckBeforePowerView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认内容;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 确认结果;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView RunRecordView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 分切速度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自动张力设定;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自动张力显示;
        private System.Windows.Forms.DataGridViewTextBoxColumn 张力输出显示;
        private System.Windows.Forms.DataGridViewTextBoxColumn 操作人;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox NeightcheckBox;
        private System.Windows.Forms.CheckBox DatecheckBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}