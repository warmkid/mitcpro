namespace mySystem.Process.CleanCut
{
    partial class CleanCut_RunRecord
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
            this.cb夜班 = new System.Windows.Forms.CheckBox();
            this.cb白班 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtp确认日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb审核人 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb确认人 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.bt日志 = new System.Windows.Forms.Button();
            this.bt发送审核 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt确认 = new System.Windows.Forms.Button();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.tb生产指令编号 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bt查询新建 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cb夜班
            // 
            this.cb夜班.AutoSize = true;
            this.cb夜班.Font = new System.Drawing.Font("SimSun", 12F);
            this.cb夜班.Location = new System.Drawing.Point(755, 69);
            this.cb夜班.Name = "cb夜班";
            this.cb夜班.Size = new System.Drawing.Size(59, 20);
            this.cb夜班.TabIndex = 81;
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
            this.cb白班.Location = new System.Drawing.Point(692, 69);
            this.cb白班.Name = "cb白班";
            this.cb白班.Size = new System.Drawing.Size(59, 20);
            this.cb白班.TabIndex = 80;
            this.cb白班.Text = "白班";
            this.cb白班.UseVisualStyleBackColor = true;
            this.cb白班.CheckedChanged += new System.EventHandler(this.cb白班_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F);
            this.label10.Location = new System.Drawing.Point(599, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 79;
            this.label10.Text = "生产班次：";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(373, 23);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(169, 19);
            this.Title.TabIndex = 70;
            this.Title.Text = "清洁分切运行记录";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(777, 474);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(136, 26);
            this.dtp审核日期.TabIndex = 89;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(696, 481);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 88;
            this.label4.Text = "审核日期：";
            // 
            // dtp确认日期
            // 
            this.dtp确认日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp确认日期.Location = new System.Drawing.Point(303, 474);
            this.dtp确认日期.Name = "dtp确认日期";
            this.dtp确认日期.Size = new System.Drawing.Size(147, 26);
            this.dtp确认日期.TabIndex = 87;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(222, 481);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 86;
            this.label3.Text = "确认日期：";
            // 
            // tb审核人
            // 
            this.tb审核人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb审核人.Location = new System.Drawing.Point(565, 474);
            this.tb审核人.Name = "tb审核人";
            this.tb审核人.Size = new System.Drawing.Size(100, 26);
            this.tb审核人.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(499, 480);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 84;
            this.label2.Text = "审核人：";
            // 
            // tb确认人
            // 
            this.tb确认人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb确认人.Location = new System.Drawing.Point(93, 474);
            this.tb确认人.Name = "tb确认人";
            this.tb确认人.Size = new System.Drawing.Size(100, 26);
            this.tb确认人.TabIndex = 83;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(25, 481);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 82;
            this.label1.Text = "确认人：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(29, 113);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(884, 275);
            this.dataGridView1.TabIndex = 93;
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb备注.Location = new System.Drawing.Point(81, 397);
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(832, 26);
            this.tb备注.TabIndex = 96;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(26, 397);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 97;
            this.label7.Text = "备注：";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(741, 429);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 94;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(833, 429);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 95;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // bt日志
            // 
            this.bt日志.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt日志.Location = new System.Drawing.Point(833, 523);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(80, 30);
            this.bt日志.TabIndex = 110;
            this.bt日志.Text = "日志";
            this.bt日志.UseVisualStyleBackColor = true;
            this.bt日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // bt发送审核
            // 
            this.bt发送审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt发送审核.Location = new System.Drawing.Point(741, 523);
            this.bt发送审核.Name = "bt发送审核";
            this.bt发送审核.Size = new System.Drawing.Size(80, 30);
            this.bt发送审核.TabIndex = 109;
            this.bt发送审核.Text = "发送审核";
            this.bt发送审核.UseVisualStyleBackColor = true;
            this.bt发送审核.Click += new System.EventHandler(this.bt发送审核_Click);
            // 
            // bt打印
            // 
            this.bt打印.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt打印.Location = new System.Drawing.Point(121, 523);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(80, 30);
            this.bt打印.TabIndex = 108;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // bt审核
            // 
            this.bt审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt审核.Location = new System.Drawing.Point(29, 523);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(80, 30);
            this.bt审核.TabIndex = 107;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            this.bt审核.Click += new System.EventHandler(this.bt审核_Click);
            // 
            // bt确认
            // 
            this.bt确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt确认.Location = new System.Drawing.Point(649, 523);
            this.bt确认.Name = "bt确认";
            this.bt确认.Size = new System.Drawing.Size(80, 30);
            this.bt确认.TabIndex = 106;
            this.bt确认.Text = "确认";
            this.bt确认.UseVisualStyleBackColor = true;
            this.bt确认.Click += new System.EventHandler(this.bt确认_Click);
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp生产日期.Location = new System.Drawing.Point(436, 62);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(142, 26);
            this.dtp生产日期.TabIndex = 112;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(355, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 111;
            this.label5.Text = "生产日期：";
            // 
            // tb生产指令编号
            // 
            this.tb生产指令编号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb生产指令编号.Location = new System.Drawing.Point(142, 63);
            this.tb生产指令编号.Name = "tb生产指令编号";
            this.tb生产指令编号.Size = new System.Drawing.Size(196, 26);
            this.tb生产指令编号.TabIndex = 114;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(26, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 113;
            this.label8.Text = "生产指令编码：";
            // 
            // bt查询新建
            // 
            this.bt查询新建.Font = new System.Drawing.Font("SimSun", 12F);
            this.bt查询新建.Location = new System.Drawing.Point(820, 63);
            this.bt查询新建.Name = "bt查询新建";
            this.bt查询新建.Size = new System.Drawing.Size(93, 30);
            this.bt查询新建.TabIndex = 115;
            this.bt查询新建.Text = "查询/新建";
            this.bt查询新建.UseVisualStyleBackColor = true;
            this.bt查询新建.Click += new System.EventHandler(this.bt查询新建_Click);
            // 
            // CleanCut_RunRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 572);
            this.Controls.Add(this.bt查询新建);
            this.Controls.Add(this.tb生产指令编号);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.bt发送审核);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt确认);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp确认日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb审核人);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb确认人);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb夜班);
            this.Controls.Add(this.cb白班);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Title);
            this.Name = "CleanCut_RunRecord";
            this.Text = "CleanCut_RunRecord";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb夜班;
        private System.Windows.Forms.CheckBox cb白班;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtp确认日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb审核人;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb确认人;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button bt日志;
        private System.Windows.Forms.Button bt发送审核;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt确认;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb生产指令编号;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt查询新建;
    }
}