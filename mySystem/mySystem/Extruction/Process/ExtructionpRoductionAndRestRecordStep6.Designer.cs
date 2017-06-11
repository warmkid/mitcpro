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
            this.InformationView = new System.Windows.Forms.DataGridView();
            this.DatecheckBox = new System.Windows.Forms.CheckBox();
            this.NeightcheckBox = new System.Windows.Forms.CheckBox();
            this.Datelabel = new System.Windows.Forms.Label();
            this.CheckName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DeleteLineBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InformationView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(198, 6);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(137, 12);
            this.Title.TabIndex = 3;
            this.Title.Text = "吹膜工序生产和检验记录";
            // 
            // RecordView
            // 
            this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecordView.Location = new System.Drawing.Point(12, 95);
            this.RecordView.Name = "RecordView";
            this.RecordView.RowTemplate.Height = 23;
            this.RecordView.Size = new System.Drawing.Size(498, 197);
            this.RecordView.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "班次：";
            // 
            // InformationView
            // 
            this.InformationView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InformationView.Location = new System.Drawing.Point(12, 23);
            this.InformationView.Name = "InformationView";
            this.InformationView.RowTemplate.Height = 23;
            this.InformationView.Size = new System.Drawing.Size(346, 66);
            this.InformationView.TabIndex = 4;
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Location = new System.Drawing.Point(408, 27);
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
            this.NeightcheckBox.Location = new System.Drawing.Point(462, 27);
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
            this.Datelabel.Location = new System.Drawing.Point(362, 52);
            this.Datelabel.Name = "Datelabel";
            this.Datelabel.Size = new System.Drawing.Size(137, 12);
            this.Datelabel.TabIndex = 9;
            this.Datelabel.Text = "生产日期:   年  月  日";
            // 
            // CheckName
            // 
            this.CheckName.AutoSize = true;
            this.CheckName.Location = new System.Drawing.Point(362, 77);
            this.CheckName.Name = "CheckName";
            this.CheckName.Size = new System.Drawing.Size(65, 12);
            this.CheckName.TabIndex = 10;
            this.CheckName.Text = "复核人:XXX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "注：正常或符合打“√”，不正常或不符合取消勾选。";
            // 
            // DeleteLineBtn
            // 
            this.DeleteLineBtn.Location = new System.Drawing.Point(435, 298);
            this.DeleteLineBtn.Name = "DeleteLineBtn";
            this.DeleteLineBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteLineBtn.TabIndex = 12;
            this.DeleteLineBtn.Text = "删除记录";
            this.DeleteLineBtn.UseVisualStyleBackColor = true;
            this.DeleteLineBtn.Click += new System.EventHandler(this.DeleteLineBtn_Click_1);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Location = new System.Drawing.Point(352, 298);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddLineBtn.TabIndex = 13;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
            // 
            // ExtructionpRoductionAndRestRecordStep6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 326);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.DeleteLineBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CheckName);
            this.Controls.Add(this.Datelabel);
            this.Controls.Add(this.NeightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RecordView);
            this.Controls.Add(this.InformationView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtructionpRoductionAndRestRecordStep6";
            this.Text = "ExtructionpRoductionAndRestRecordStep6";
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InformationView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView RecordView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView InformationView;
        private System.Windows.Forms.CheckBox DatecheckBox;
        private System.Windows.Forms.CheckBox NeightcheckBox;
        private System.Windows.Forms.Label Datelabel;
        private System.Windows.Forms.Label CheckName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DeleteLineBtn;
        private System.Windows.Forms.Button AddLineBtn;
    }
}