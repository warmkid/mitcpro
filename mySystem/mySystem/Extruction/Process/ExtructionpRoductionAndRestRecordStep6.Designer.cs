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
            this.Datelabel = new System.Windows.Forms.Label();
            this.CheckNameLabel = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(501, 8);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(137, 12);
            this.Title.TabIndex = 3;
            this.Title.Text = "吹膜工序生产和检验记录";
            // 
            // RecordView
            // 
            this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecordView.Location = new System.Drawing.Point(30, 111);
            this.RecordView.Name = "RecordView";
            this.RecordView.RowTemplate.Height = 23;
            this.RecordView.Size = new System.Drawing.Size(1123, 374);
            this.RecordView.TabIndex = 5;
            this.RecordView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(995, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "班次：";
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Location = new System.Drawing.Point(1041, 76);
            this.DatecheckBox.Name = "DatecheckBox";
            this.DatecheckBox.Size = new System.Drawing.Size(48, 16);
            this.DatecheckBox.TabIndex = 7;
            this.DatecheckBox.Text = "白班";
            this.DatecheckBox.UseVisualStyleBackColor = true;
            this.DatecheckBox.CheckedChanged += new System.EventHandler(this.DatecheckBox_CheckedChanged);
            // 
            // NeightcheckBox
            // 
            this.NeightcheckBox.AutoSize = true;
            this.NeightcheckBox.Location = new System.Drawing.Point(1095, 76);
            this.NeightcheckBox.Name = "NeightcheckBox";
            this.NeightcheckBox.Size = new System.Drawing.Size(48, 16);
            this.NeightcheckBox.TabIndex = 8;
            this.NeightcheckBox.Text = "夜班";
            this.NeightcheckBox.UseVisualStyleBackColor = true;
            this.NeightcheckBox.CheckedChanged += new System.EventHandler(this.NeightcheckBox_CheckedChanged);
            // 
            // Datelabel
            // 
            this.Datelabel.AutoSize = true;
            this.Datelabel.Location = new System.Drawing.Point(556, 77);
            this.Datelabel.Name = "Datelabel";
            this.Datelabel.Size = new System.Drawing.Size(143, 12);
            this.Datelabel.TabIndex = 9;
            this.Datelabel.Text = "生产日期：   年  月  日";
            // 
            // CheckNameLabel
            // 
            this.CheckNameLabel.AutoSize = true;
            this.CheckNameLabel.Location = new System.Drawing.Point(791, 77);
            this.CheckNameLabel.Name = "CheckNameLabel";
            this.CheckNameLabel.Size = new System.Drawing.Size(65, 12);
            this.CheckNameLabel.TabIndex = 10;
            this.CheckNameLabel.Text = "复核人:XXX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 496);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Location = new System.Drawing.Point(1080, 491);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddLineBtn.TabIndex = 13;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "产品名称：";
            // 
            // productnameBox
            // 
            this.productnameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.productnameBox.Location = new System.Drawing.Point(101, 43);
            this.productnameBox.Name = "productnameBox";
            this.productnameBox.Size = new System.Drawing.Size(100, 21);
            this.productnameBox.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(556, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "依据工艺：吹膜工艺规程";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "产品批号：";
            // 
            // productnumberBox
            // 
            this.productnumberBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.productnumberBox.Location = new System.Drawing.Point(372, 43);
            this.productnumberBox.Name = "productnumberBox";
            this.productnumberBox.Size = new System.Drawing.Size(100, 21);
            this.productnumberBox.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(791, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "生产设备：AA-EQM-032";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(197, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "环境温度：                   °C";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(308, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(185, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "相对湿度：                   %";
            // 
            // temperatureBox
            // 
            this.temperatureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.temperatureBox.Location = new System.Drawing.Point(101, 73);
            this.temperatureBox.Name = "temperatureBox";
            this.temperatureBox.Size = new System.Drawing.Size(100, 21);
            this.temperatureBox.TabIndex = 22;
            // 
            // humidityBox
            // 
            this.humidityBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.humidityBox.Location = new System.Drawing.Point(372, 70);
            this.humidityBox.Name = "humidityBox";
            this.humidityBox.Size = new System.Drawing.Size(100, 21);
            this.humidityBox.TabIndex = 23;
            // 
            // ExtructionpRoductionAndRestRecordStep6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 523);
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
            this.Controls.Add(this.CheckNameLabel);
            this.Controls.Add(this.Datelabel);
            this.Controls.Add(this.NeightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RecordView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
        private System.Windows.Forms.Label Datelabel;
        private System.Windows.Forms.Label CheckNameLabel;
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
    }
}