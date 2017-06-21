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
            this.Title = new System.Windows.Forms.Label();
            this.RecordView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.DatecheckBox = new System.Windows.Forms.CheckBox();
            this.NeightcheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.productnameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.productnumberBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.temperatureBox = new System.Windows.Forms.TextBox();
            this.humidityBox = new System.Windows.Forms.TextBox();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
            this.SuspendLayout();
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
            // RecordView
            // 
            this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecordView.Location = new System.Drawing.Point(30, 101);
            this.RecordView.Name = "RecordView";
            this.RecordView.RowTemplate.Height = 23;
            this.RecordView.Size = new System.Drawing.Size(1118, 318);
            this.RecordView.TabIndex = 5;
            this.RecordView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(954, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "班次：";
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.DatecheckBox.Location = new System.Drawing.Point(1009, 66);
            this.DatecheckBox.Name = "DatecheckBox";
            this.DatecheckBox.Size = new System.Drawing.Size(59, 20);
            this.DatecheckBox.TabIndex = 7;
            this.DatecheckBox.Text = "白班";
            this.DatecheckBox.UseVisualStyleBackColor = true;
            this.DatecheckBox.CheckedChanged += new System.EventHandler(this.DatecheckBox_CheckedChanged);
            // 
            // NeightcheckBox
            // 
            this.NeightcheckBox.AutoSize = true;
            this.NeightcheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.NeightcheckBox.Location = new System.Drawing.Point(1070, 66);
            this.NeightcheckBox.Name = "NeightcheckBox";
            this.NeightcheckBox.Size = new System.Drawing.Size(59, 20);
            this.NeightcheckBox.TabIndex = 8;
            this.NeightcheckBox.Text = "夜班";
            this.NeightcheckBox.UseVisualStyleBackColor = true;
            this.NeightcheckBox.CheckedChanged += new System.EventHandler(this.NeightcheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(27, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(376, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "注：外观和判定栏合格划“√”，不合格取消勾选。";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(1068, 430);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 13;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
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
            // productnameBox
            // 
            this.productnameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.productnameBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.productnameBox.Location = new System.Drawing.Point(112, 33);
            this.productnameBox.Name = "productnameBox";
            this.productnameBox.Size = new System.Drawing.Size(100, 26);
            this.productnameBox.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(654, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "依据工艺：吹膜工艺规程";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(325, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "产品批号：";
            // 
            // productnumberBox
            // 
            this.productnumberBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.productnumberBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.productnumberBox.Location = new System.Drawing.Point(401, 33);
            this.productnumberBox.Name = "productnumberBox";
            this.productnumberBox.Size = new System.Drawing.Size(100, 26);
            this.productnumberBox.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(654, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "生产设备：AA-EQM-032";
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(325, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(200, 16);
            this.label8.TabIndex = 21;
            this.label8.Text = "相对湿度：             %";
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
            // humidityBox
            // 
            this.humidityBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.humidityBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.humidityBox.Location = new System.Drawing.Point(401, 63);
            this.humidityBox.Name = "humidityBox";
            this.humidityBox.Size = new System.Drawing.Size(100, 26);
            this.humidityBox.TabIndex = 23;
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1068, 477);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 25;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(976, 477);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 24;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // ExtructionpRoductionAndRestRecordStep6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 519);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.humidityBox);
            this.Controls.Add(this.temperatureBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.productnumberBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.productnameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NeightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RecordView);
            this.Controls.Add(this.Title);
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
        private System.Windows.Forms.CheckBox NeightcheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox productnameBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox productnumberBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox temperatureBox;
        private System.Windows.Forms.TextBox humidityBox;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
    }
}