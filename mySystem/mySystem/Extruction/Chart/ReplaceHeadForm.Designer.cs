namespace mySystem
{
    partial class ReplaceHeadForm
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.检查项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检查标准 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检查结果 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PSLabel = new System.Windows.Forms.Label();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.recordTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.checkerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.recorderBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(392, 18);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(324, 20);
            this.Title.TabIndex = 15;
            this.Title.Text = "吹膜机更换模头记录及安装检查表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.检查项目,
            this.检查标准,
            this.检查结果});
            this.dgv.Location = new System.Drawing.Point(31, 103);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(1113, 411);
            this.dgv.TabIndex = 24;
            // 
            // 检查项目
            // 
            this.检查项目.HeaderText = "检查项目";
            this.检查项目.Name = "检查项目";
            this.检查项目.Width = 200;
            // 
            // 检查标准
            // 
            this.检查标准.HeaderText = "检查标准";
            this.检查标准.Name = "检查标准";
            this.检查标准.Width = 660;
            // 
            // 检查结果
            // 
            this.检查结果.HeaderText = "检查结果";
            this.检查结果.Name = "检查结果";
            this.检查结果.Width = 200;
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("SimSun", 12F);
            this.PSLabel.Location = new System.Drawing.Point(33, 520);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(603, 15);
            this.PSLabel.TabIndex = 25;
            this.PSLabel.Text = "注：\t安装正确的在“是”栏中标“√”，安装不正确的在“否”栏中标“×”。";
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1063, 585);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 27;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(971, 585);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 26;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("SimSun", 12F);
            this.dateTimePicker1.Location = new System.Drawing.Point(410, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(163, 26);
            this.dateTimePicker1.TabIndex = 52;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(329, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 51;
            this.label8.Text = "更换日期：";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox3.Location = new System.Drawing.Point(1018, 56);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(125, 26);
            this.textBox3.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(881, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 16);
            this.label7.TabIndex = 49;
            this.label7.Text = "更换后模头型号：";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox2.Location = new System.Drawing.Point(730, 56);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(125, 26);
            this.textBox2.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(603, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 16);
            this.label6.TabIndex = 47;
            this.label6.Text = "更换前模头型号：";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox1.Location = new System.Drawing.Point(110, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(213, 26);
            this.textBox1.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(26, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 45;
            this.label5.Text = "更换原因：";
            // 
            // checkTimePicker
            // 
            this.checkTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkTimePicker.Location = new System.Drawing.Point(944, 547);
            this.checkTimePicker.Name = "checkTimePicker";
            this.checkTimePicker.Size = new System.Drawing.Size(200, 26);
            this.checkTimePicker.TabIndex = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(863, 554);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 59;
            this.label4.Text = "复核日期：";
            // 
            // recordTimePicker
            // 
            this.recordTimePicker.Font = new System.Drawing.Font("SimSun", 12F);
            this.recordTimePicker.Location = new System.Drawing.Point(346, 551);
            this.recordTimePicker.Name = "recordTimePicker";
            this.recordTimePicker.Size = new System.Drawing.Size(200, 26);
            this.recordTimePicker.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(265, 558);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 57;
            this.label3.Text = "检查日期：";
            // 
            // checkerBox
            // 
            this.checkerBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkerBox.Location = new System.Drawing.Point(698, 550);
            this.checkerBox.Name = "checkerBox";
            this.checkerBox.Size = new System.Drawing.Size(100, 26);
            this.checkerBox.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(630, 557);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 55;
            this.label2.Text = "复核人：";
            // 
            // recorderBox
            // 
            this.recorderBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.recorderBox.Location = new System.Drawing.Point(101, 551);
            this.recorderBox.Name = "recorderBox";
            this.recorderBox.Size = new System.Drawing.Size(100, 26);
            this.recorderBox.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(33, 558);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "检查人：";
            // 
            // ReplaceHeadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 622);
            this.Controls.Add(this.checkTimePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.recordTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkerBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.recorderBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.Title);
            this.Name = "ReplaceHeadForm";
            this.Text = "ReplaceHeadForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检查项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检查标准;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 检查结果;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker checkTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker recordTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox checkerBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox recorderBox;
        private System.Windows.Forms.Label label1;
    }
}