namespace mySystem.Extruction.Process
{
    partial class ExtructionCheckBeforePowerStep2
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
            this.CheckBeforePowerView = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.Label();
            this.printBtn = new System.Windows.Forms.Button();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认内容 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.确认结果Y = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.确认结果N = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).BeginInit();
            this.SuspendLayout();
            // 
            // checkTimePicker
            // 
            this.checkTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkTimePicker.Location = new System.Drawing.Point(940, 40);
            this.checkTimePicker.Name = "checkTimePicker";
            this.checkTimePicker.Size = new System.Drawing.Size(200, 26);
            this.checkTimePicker.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(859, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "审核日期：";
            // 
            // recordTimePicker
            // 
            this.recordTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.recordTimePicker.Location = new System.Drawing.Point(340, 39);
            this.recordTimePicker.Name = "recordTimePicker";
            this.recordTimePicker.Size = new System.Drawing.Size(200, 26);
            this.recordTimePicker.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(259, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "确认日期：";
            // 
            // checkerBox
            // 
            this.checkerBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkerBox.Location = new System.Drawing.Point(696, 43);
            this.checkerBox.Name = "checkerBox";
            this.checkerBox.Size = new System.Drawing.Size(100, 26);
            this.checkerBox.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(628, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "审核人：";
            // 
            // recorderBox
            // 
            this.recorderBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.recorderBox.Location = new System.Drawing.Point(95, 39);
            this.recorderBox.Name = "recorderBox";
            this.recorderBox.Size = new System.Drawing.Size(100, 26);
            this.recorderBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(27, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "确认人：";
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(965, 491);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 8;
            this.CheckBtn.Text = "审核";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(873, 491);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 7;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("SimSun", 12F);
            this.PSLabel.Location = new System.Drawing.Point(24, 491);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(498, 15);
            this.PSLabel.TabIndex = 6;
            this.PSLabel.Text = "注：\t正常或符合打“√”，不正常或不符合打“×”。";
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
            this.确认结果Y,
            this.确认结果N});
            this.CheckBeforePowerView.Location = new System.Drawing.Point(27, 82);
            this.CheckBeforePowerView.Name = "CheckBeforePowerView";
            this.CheckBeforePowerView.RowTemplate.Height = 23;
            this.CheckBeforePowerView.Size = new System.Drawing.Size(1113, 402);
            this.CheckBeforePowerView.TabIndex = 1;
            this.CheckBeforePowerView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CheckBeforePowerView_CellContentClick);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(482, 7);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(209, 19);
            this.Title.TabIndex = 0;
            this.Title.Text = "吹膜机组开机前确认表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // printBtn
            // 
            this.printBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.printBtn.Location = new System.Drawing.Point(1060, 491);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(80, 30);
            this.printBtn.TabIndex = 17;
            this.printBtn.Text = "打印";
            this.printBtn.UseVisualStyleBackColor = true;
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
            // 确认结果Y
            // 
            this.确认结果Y.HeaderText = "确认结果(Y)";
            this.确认结果Y.Name = "确认结果Y";
            // 
            // 确认结果N
            // 
            this.确认结果N.HeaderText = "确认结果(N)";
            this.确认结果N.Name = "确认结果N";
            this.确认结果N.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.确认结果N.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ExtructionCheckBeforePowerStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 532);
            this.Controls.Add(this.printBtn);
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
            this.Controls.Add(this.CheckBeforePowerView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExtructionCheckBeforePowerStep2";
            this.Text = "ExtructionCheckBeforePowerStep2";
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView CheckBeforePowerView;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox recorderBox;
        private System.Windows.Forms.TextBox checkerBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker recordTimePicker;
        private System.Windows.Forms.DateTimePicker checkTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button printBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 确认内容;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 确认结果Y;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 确认结果N;
    }
}