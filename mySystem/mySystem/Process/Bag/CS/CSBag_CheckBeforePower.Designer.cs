namespace mySystem.Process.Bag
{
    partial class CSBag_CheckBeforePower
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.CheckView = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认内容 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认结果 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.checkTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.recordTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.checkerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.recorderBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.PSLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CheckView)).BeginInit();
            this.SuspendLayout();
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "国家";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "2";
            this.Column2.HeaderText = "城市";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "3";
            this.Column3.HeaderText = "男";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "4";
            this.Column4.HeaderText = "女";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(478, 9);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(209, 19);
            this.Title.TabIndex = 26;
            this.Title.Text = "制袋机组开机前确认表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CheckView
            // 
            this.CheckView.AllowUserToAddRows = false;
            this.CheckView.AllowUserToDeleteRows = false;
            this.CheckView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheckView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.确认项目,
            this.确认内容,
            this.确认结果});
            this.CheckView.Location = new System.Drawing.Point(14, 80);
            this.CheckView.Name = "CheckView";
            this.CheckView.RowTemplate.Height = 23;
            this.CheckView.Size = new System.Drawing.Size(1113, 349);
            this.CheckView.TabIndex = 27;
            this.CheckView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CheckView_CellContentClick);
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
            // checkTimePicker
            // 
            this.checkTimePicker.Font = new System.Drawing.Font("宋体", 12F);
            this.checkTimePicker.Location = new System.Drawing.Point(927, 45);
            this.checkTimePicker.Name = "checkTimePicker";
            this.checkTimePicker.Size = new System.Drawing.Size(200, 26);
            this.checkTimePicker.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(846, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 37;
            this.label4.Text = "复核日期：";
            // 
            // recordTimePicker
            // 
            this.recordTimePicker.Font = new System.Drawing.Font("宋体", 12F);
            this.recordTimePicker.Location = new System.Drawing.Point(327, 44);
            this.recordTimePicker.Name = "recordTimePicker";
            this.recordTimePicker.Size = new System.Drawing.Size(200, 26);
            this.recordTimePicker.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(246, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "确认日期：";
            // 
            // checkerBox
            // 
            this.checkerBox.Font = new System.Drawing.Font("宋体", 12F);
            this.checkerBox.Location = new System.Drawing.Point(683, 44);
            this.checkerBox.Name = "checkerBox";
            this.checkerBox.Size = new System.Drawing.Size(100, 26);
            this.checkerBox.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(615, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "复核人：";
            // 
            // recorderBox
            // 
            this.recorderBox.Font = new System.Drawing.Font("宋体", 12F);
            this.recorderBox.Location = new System.Drawing.Point(82, 44);
            this.recorderBox.Name = "recorderBox";
            this.recorderBox.Size = new System.Drawing.Size(100, 26);
            this.recorderBox.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(14, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "确认人：";
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1047, 436);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 30;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(955, 436);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 29;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("宋体", 12F);
            this.PSLabel.Location = new System.Drawing.Point(11, 433);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(498, 15);
            this.PSLabel.TabIndex = 28;
            this.PSLabel.Text = "注：\t正常或符合打“√”，不正常或不符合打“×”。";
            // 
            // CSBag_CheckBeforePower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 479);
            this.Controls.Add(this.checkTimePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.recordTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkerBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.recorderBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.CheckView);
            this.Controls.Add(this.Title);
            this.Name = "CSBag_CheckBeforePower";
            this.Text = "S";
            ((System.ComponentModel.ISupportInitialize)(this.CheckView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView CheckView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认内容;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 确认结果;
        private System.Windows.Forms.DateTimePicker checkTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker recordTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox checkerBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox recorderBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Label PSLabel;

    }
}