namespace mySystem.Extruction.Process
{
    partial class ExtructionpRoductionAndRestRecordStep6
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
            this.CheckerBox = new System.Windows.Forms.TextBox();
            this.productionDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.humidityBox = new System.Windows.Forms.TextBox();
            this.temperatureBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.batchIdBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.NightcheckBox = new System.Windows.Forms.CheckBox();
            this.DatecheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordView = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜卷编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜卷长度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜卷重量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.记录人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外观 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.宽度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最大厚度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最小厚度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.平均厚度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.厚度公差 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检查人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.判定 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.printBtn = new System.Windows.Forms.Button();
            this.productnamecomboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckerBox
            // 
            this.CheckerBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CheckerBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckerBox.Location = new System.Drawing.Point(887, 34);
            this.CheckerBox.Name = "CheckerBox";
            this.CheckerBox.Size = new System.Drawing.Size(100, 26);
            this.CheckerBox.TabIndex = 30;
            // 
            // productionDatePicker
            // 
            this.productionDatePicker.CalendarFont = new System.Drawing.Font("SimSun", 12F);
            this.productionDatePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.productionDatePicker.Location = new System.Drawing.Point(887, 64);
            this.productionDatePicker.Name = "productionDatePicker";
            this.productionDatePicker.Size = new System.Drawing.Size(145, 26);
            this.productionDatePicker.TabIndex = 29;
            this.productionDatePicker.ValueChanged += new System.EventHandler(this.productionDatePicker_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F);
            this.label10.Location = new System.Drawing.Point(801, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 16);
            this.label10.TabIndex = 28;
            this.label10.Text = "复核人:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F);
            this.label9.Location = new System.Drawing.Point(801, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "生产日期:";
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(1070, 440);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 26;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(978, 480);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 25;
            this.CheckBtn.Text = "审核";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(886, 480);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 24;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // humidityBox
            // 
            this.humidityBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.humidityBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.humidityBox.Location = new System.Drawing.Point(438, 63);
            this.humidityBox.Name = "humidityBox";
            this.humidityBox.Size = new System.Drawing.Size(100, 26);
            this.humidityBox.TabIndex = 23;
            // 
            // temperatureBox
            // 
            this.temperatureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.temperatureBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.temperatureBox.Location = new System.Drawing.Point(112, 63);
            this.temperatureBox.Name = "temperatureBox";
            this.temperatureBox.Size = new System.Drawing.Size(100, 26);
            this.temperatureBox.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(362, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(200, 16);
            this.label8.TabIndex = 21;
            this.label8.Text = "相对湿度：             %";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(30, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(216, 16);
            this.label7.TabIndex = 20;
            this.label7.Text = "环境温度：             °C";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(584, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "生产设备：AA-EQM-032";
            // 
            // batchIdBox
            // 
            this.batchIdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.batchIdBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.batchIdBox.Location = new System.Drawing.Point(438, 33);
            this.batchIdBox.Name = "batchIdBox";
            this.batchIdBox.Size = new System.Drawing.Size(100, 26);
            this.batchIdBox.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(362, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "产品批号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(584, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "依据工艺：吹膜工艺规程";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(30, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "产品名称：";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(978, 440);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 13;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(27, 442);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(376, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "注：外观和判定栏合格划“√”，不合格取消勾选。";
            // 
            // NightcheckBox
            // 
            this.NightcheckBox.AutoSize = true;
            this.NightcheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.NightcheckBox.Location = new System.Drawing.Point(1089, 67);
            this.NightcheckBox.Name = "NightcheckBox";
            this.NightcheckBox.Size = new System.Drawing.Size(59, 20);
            this.NightcheckBox.TabIndex = 8;
            this.NightcheckBox.Text = "夜班";
            this.NightcheckBox.UseVisualStyleBackColor = true;
            this.NightcheckBox.CheckedChanged += new System.EventHandler(this.NeightcheckBox_CheckedChanged);
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.DatecheckBox.Location = new System.Drawing.Point(1089, 31);
            this.DatecheckBox.Name = "DatecheckBox";
            this.DatecheckBox.Size = new System.Drawing.Size(59, 20);
            this.DatecheckBox.TabIndex = 7;
            this.DatecheckBox.Text = "白班";
            this.DatecheckBox.UseVisualStyleBackColor = true;
            this.DatecheckBox.CheckedChanged += new System.EventHandler(this.DatecheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(1040, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "班次：";
            // 
            // RecordView
            // 
            this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.时间,
            this.膜卷编号,
            this.膜卷长度,
            this.膜卷重量,
            this.记录人,
            this.外观,
            this.宽度,
            this.最大厚度,
            this.最小厚度,
            this.平均厚度,
            this.厚度公差,
            this.检查人,
            this.判定});
            this.RecordView.Location = new System.Drawing.Point(30, 101);
            this.RecordView.Name = "RecordView";
            this.RecordView.RowTemplate.Height = 23;
            this.RecordView.Size = new System.Drawing.Size(1118, 330);
            this.RecordView.TabIndex = 5;
            this.RecordView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellContentClick);
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 时间
            // 
            this.时间.HeaderText = "时间";
            this.时间.Name = "时间";
            // 
            // 膜卷编号
            // 
            this.膜卷编号.HeaderText = "膜卷编号\\r/n(卷)";
            this.膜卷编号.Name = "膜卷编号";
            // 
            // 膜卷长度
            // 
            this.膜卷长度.HeaderText = "膜卷长度\\r(m)";
            this.膜卷长度.Name = "膜卷长度";
            // 
            // 膜卷重量
            // 
            this.膜卷重量.HeaderText = "膜卷重量\\r(kg)";
            this.膜卷重量.Name = "膜卷重量";
            // 
            // 记录人
            // 
            this.记录人.HeaderText = "记录人";
            this.记录人.Name = "记录人";
            // 
            // 外观
            // 
            this.外观.HeaderText = "外观";
            this.外观.Name = "外观";
            // 
            // 宽度
            // 
            this.宽度.HeaderText = "宽度\\r(mm)";
            this.宽度.Name = "宽度";
            // 
            // 最大厚度
            // 
            this.最大厚度.HeaderText = "最大厚度\\r（μm）";
            this.最大厚度.Name = "最大厚度";
            // 
            // 最小厚度
            // 
            this.最小厚度.HeaderText = "最小厚度\\r（μm）";
            this.最小厚度.Name = "最小厚度";
            // 
            // 平均厚度
            // 
            this.平均厚度.HeaderText = "平均厚度\\r（μm）";
            this.平均厚度.Name = "平均厚度";
            // 
            // 厚度公差
            // 
            this.厚度公差.HeaderText = "厚度公差\\r(%)";
            this.厚度公差.Name = "厚度公差";
            // 
            // 检查人
            // 
            this.检查人.HeaderText = "检查人";
            this.检查人.Name = "检查人";
            // 
            // 判定
            // 
            this.判定.HeaderText = "判定";
            this.判定.Name = "判定";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(501, 8);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(229, 19);
            this.Title.TabIndex = 3;
            this.Title.Text = "吹膜工序生产和检验记录";
            // 
            // printBtn
            // 
            this.printBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.printBtn.Location = new System.Drawing.Point(1070, 480);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(80, 30);
            this.printBtn.TabIndex = 31;
            this.printBtn.Text = "打印";
            this.printBtn.UseVisualStyleBackColor = true;
            // 
            // productnamecomboBox
            // 
            this.productnamecomboBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.productnamecomboBox.FormattingEnabled = true;
            this.productnamecomboBox.Location = new System.Drawing.Point(112, 34);
            this.productnamecomboBox.Name = "productnamecomboBox";
            this.productnamecomboBox.Size = new System.Drawing.Size(220, 24);
            this.productnamecomboBox.TabIndex = 32;
            this.productnamecomboBox.SelectedIndexChanged += new System.EventHandler(this.productnamecomboBox_SelectedIndexChanged);
            // 
            // ExtructionpRoductionAndRestRecordStep6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 519);
            this.Controls.Add(this.productnamecomboBox);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.CheckerBox);
            this.Controls.Add(this.productionDatePicker);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.humidityBox);
            this.Controls.Add(this.temperatureBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.batchIdBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.RecordView);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExtructionpRoductionAndRestRecordStep6";
            this.Text = "ExtructionpRoductionAndRestRecordStep6";
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView RecordView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DatecheckBox;
        private System.Windows.Forms.CheckBox NightcheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox batchIdBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox temperatureBox;
        private System.Windows.Forms.TextBox humidityBox;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker productionDatePicker;
        private System.Windows.Forms.TextBox CheckerBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜卷编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜卷长度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜卷重量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 记录人;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 外观;
        private System.Windows.Forms.DataGridViewTextBoxColumn 宽度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最大厚度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最小厚度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 平均厚度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 厚度公差;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检查人;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 判定;
        private System.Windows.Forms.Button printBtn;
        private System.Windows.Forms.ComboBox productnamecomboBox;
    }
}