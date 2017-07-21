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
            this.tb生产指令编号 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.tb车间湿度 = new System.Windows.Forms.TextBox();
            this.tb车间温度 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt打印 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt确认 = new System.Windows.Forms.Button();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtp确认日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb审核人 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb确认人 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.cb夜班 = new System.Windows.Forms.CheckBox();
            this.cb白班 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bt日志 = new System.Windows.Forms.Button();
            this.bt发送审核 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb生产指令编号
            // 
            this.tb生产指令编号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb生产指令编号.Location = new System.Drawing.Point(136, 54);
            this.tb生产指令编号.Name = "tb生产指令编号";
            this.tb生产指令编号.Size = new System.Drawing.Size(196, 26);
            this.tb生产指令编号.TabIndex = 64;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(20, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 63;
            this.label8.Text = "生产指令编码：";
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb备注.Location = new System.Drawing.Point(72, 436);
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(824, 26);
            this.tb备注.TabIndex = 58;
            // 
            // tb车间湿度
            // 
            this.tb车间湿度.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb车间湿度.Location = new System.Drawing.Point(373, 369);
            this.tb车间湿度.Name = "tb车间湿度";
            this.tb车间湿度.Size = new System.Drawing.Size(100, 26);
            this.tb车间湿度.TabIndex = 57;
            // 
            // tb车间温度
            // 
            this.tb车间温度.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb车间温度.Location = new System.Drawing.Point(155, 369);
            this.tb车间温度.Name = "tb车间温度";
            this.tb车间温度.Size = new System.Drawing.Size(100, 26);
            this.tb车间温度.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(21, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(496, 16);
            this.label6.TabIndex = 55;
            this.label6.Text = "填写： 车间温度：             ℃，车间湿度：             ﹪。\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(20, 409);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(616, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "注: 开机确认结果正常或符合打“√”；不正常或不符合取消勾选，并在备注处说明。\r\n";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(369, 19);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(177, 20);
            this.Title.TabIndex = 26;
            this.Title.Text = "清洁分切开机确认";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 93);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(875, 270);
            this.dataGridView1.TabIndex = 17;
            // 
            // bt打印
            // 
            this.bt打印.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt打印.Location = new System.Drawing.Point(115, 529);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(80, 30);
            this.bt打印.TabIndex = 75;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // bt审核
            // 
            this.bt审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt审核.Location = new System.Drawing.Point(23, 529);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(80, 30);
            this.bt审核.TabIndex = 74;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.bt审核_Click);
            // 
            // bt确认
            // 
            this.bt确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt确认.Location = new System.Drawing.Point(632, 529);
            this.bt确认.Name = "bt确认";
            this.bt确认.Size = new System.Drawing.Size(80, 30);
            this.bt确认.TabIndex = 73;
            this.bt确认.Text = "确认";
            this.bt确认.UseVisualStyleBackColor = true;
            this.bt确认.Click += new System.EventHandler(this.bt确认_Click);
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(760, 487);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(136, 26);
            this.dtp审核日期.TabIndex = 97;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(679, 494);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 96;
            this.label4.Text = "审核日期：";
            // 
            // dtp确认日期
            // 
            this.dtp确认日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp确认日期.Location = new System.Drawing.Point(302, 486);
            this.dtp确认日期.Name = "dtp确认日期";
            this.dtp确认日期.Size = new System.Drawing.Size(147, 26);
            this.dtp确认日期.TabIndex = 95;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(221, 493);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 94;
            this.label3.Text = "确认日期：";
            // 
            // tb审核人
            // 
            this.tb审核人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb审核人.Location = new System.Drawing.Point(544, 490);
            this.tb审核人.Name = "tb审核人";
            this.tb审核人.Size = new System.Drawing.Size(100, 26);
            this.tb审核人.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(476, 497);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 92;
            this.label2.Text = "审核人：";
            // 
            // tb确认人
            // 
            this.tb确认人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb确认人.Location = new System.Drawing.Point(92, 486);
            this.tb确认人.Name = "tb确认人";
            this.tb确认人.Size = new System.Drawing.Size(100, 26);
            this.tb确认人.TabIndex = 91;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(24, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 90;
            this.label1.Text = "确认人：";
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp生产日期.Location = new System.Drawing.Point(476, 54);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(169, 26);
            this.dtp生产日期.TabIndex = 99;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F);
            this.label9.Location = new System.Drawing.Point(395, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 98;
            this.label9.Text = "生产日期：";
            // 
            // cb夜班
            // 
            this.cb夜班.AutoSize = true;
            this.cb夜班.Font = new System.Drawing.Font("SimSun", 12F);
            this.cb夜班.Location = new System.Drawing.Point(835, 63);
            this.cb夜班.Name = "cb夜班";
            this.cb夜班.Size = new System.Drawing.Size(59, 20);
            this.cb夜班.TabIndex = 102;
            this.cb夜班.Text = "夜班";
            this.cb夜班.UseVisualStyleBackColor = true;
            this.cb夜班.CheckedChanged += new System.EventHandler(this.cb夜班_CheckedChanged);
            // 
            // cb白班
            // 
            this.cb白班.AutoSize = true;
            this.cb白班.Checked = true;
            this.cb白班.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb白班.Font = new System.Drawing.Font("SimSun", 12F);
            this.cb白班.Location = new System.Drawing.Point(772, 63);
            this.cb白班.Name = "cb白班";
            this.cb白班.Size = new System.Drawing.Size(59, 20);
            this.cb白班.TabIndex = 101;
            this.cb白班.Text = "白班";
            this.cb白班.UseVisualStyleBackColor = true;
            this.cb白班.CheckedChanged += new System.EventHandler(this.cb白班_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F);
            this.label10.Location = new System.Drawing.Point(679, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 100;
            this.label10.Text = "生产班次：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(20, 441);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 103;
            this.label7.Text = "备注：";
            // 
            // bt日志
            // 
            this.bt日志.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt日志.Location = new System.Drawing.Point(816, 529);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(80, 30);
            this.bt日志.TabIndex = 105;
            this.bt日志.Text = "日志";
            this.bt日志.UseVisualStyleBackColor = true;
            this.bt日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // bt发送审核
            // 
            this.bt发送审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt发送审核.Location = new System.Drawing.Point(724, 529);
            this.bt发送审核.Name = "bt发送审核";
            this.bt发送审核.Size = new System.Drawing.Size(80, 30);
            this.bt发送审核.TabIndex = 104;
            this.bt发送审核.Text = "发送审核";
            this.bt发送审核.UseVisualStyleBackColor = true;
            this.bt发送审核.Click += new System.EventHandler(this.bt发送审核_Click);
            // 
            // CleanCut_CheckBeforePower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 569);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.bt发送审核);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cb夜班);
            this.Controls.Add(this.cb白班);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp确认日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb审核人);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb确认人);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt确认);
            this.Controls.Add(this.tb生产指令编号);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb车间湿度);
            this.Controls.Add(this.tb车间温度);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CleanCut_CheckBeforePower";
            this.Text = "清洁分切开机确认";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb车间温度;
        private System.Windows.Forms.TextBox tb车间湿度;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.TextBox tb生产指令编号;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt确认;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtp确认日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb审核人;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb确认人;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cb夜班;
        private System.Windows.Forms.CheckBox cb白班;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bt日志;
        private System.Windows.Forms.Button bt发送审核;
    }
}