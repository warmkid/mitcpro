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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PSLabel = new System.Windows.Forms.Label();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.dtp更换日期 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.tb更换后模头型号 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb更换前模头型号 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb更换原因 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtp检查日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb审核人 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb检查人 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.printBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(427, 22);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(324, 20);
            this.Title.TabIndex = 15;
            this.Title.Text = "吹膜机更换模头记录及安装检查表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(31, 104);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1113, 413);
            this.dataGridView1.TabIndex = 24;
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("SimSun", 12F);
            this.PSLabel.Location = new System.Drawing.Point(33, 523);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(603, 15);
            this.PSLabel.TabIndex = 25;
            this.PSLabel.Text = "注：\t安装正确的在“是”栏中标“√”，安装不正确的在“否”栏中标“×”。";
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(974, 588);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 27;
            this.CheckBtn.Text = "审核";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(882, 588);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 26;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // dtp更换日期
            // 
            this.dtp更换日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp更换日期.Location = new System.Drawing.Point(423, 64);
            this.dtp更换日期.Name = "dtp更换日期";
            this.dtp更换日期.Size = new System.Drawing.Size(163, 26);
            this.dtp更换日期.TabIndex = 52;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(342, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 51;
            this.label8.Text = "更换日期：";
            // 
            // tb更换后模头型号
            // 
            this.tb更换后模头型号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb更换后模头型号.Location = new System.Drawing.Point(1018, 64);
            this.tb更换后模头型号.Name = "tb更换后模头型号";
            this.tb更换后模头型号.Size = new System.Drawing.Size(125, 26);
            this.tb更换后模头型号.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(881, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 16);
            this.label7.TabIndex = 49;
            this.label7.Text = "更换后模头型号：";
            // 
            // tb更换前模头型号
            // 
            this.tb更换前模头型号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb更换前模头型号.Location = new System.Drawing.Point(730, 64);
            this.tb更换前模头型号.Name = "tb更换前模头型号";
            this.tb更换前模头型号.Size = new System.Drawing.Size(125, 26);
            this.tb更换前模头型号.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(603, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 16);
            this.label6.TabIndex = 47;
            this.label6.Text = "更换前模头型号：";
            // 
            // tb更换原因
            // 
            this.tb更换原因.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb更换原因.Location = new System.Drawing.Point(110, 64);
            this.tb更换原因.Name = "tb更换原因";
            this.tb更换原因.Size = new System.Drawing.Size(213, 26);
            this.tb更换原因.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(26, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 45;
            this.label5.Text = "更换原因：";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(944, 550);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(200, 26);
            this.dtp审核日期.TabIndex = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(863, 557);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 59;
            this.label4.Text = "审核日期：";
            // 
            // dtp检查日期
            // 
            this.dtp检查日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp检查日期.Location = new System.Drawing.Point(346, 554);
            this.dtp检查日期.Name = "dtp检查日期";
            this.dtp检查日期.Size = new System.Drawing.Size(200, 26);
            this.dtp检查日期.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(265, 561);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 57;
            this.label3.Text = "操作日期：";
            // 
            // tb审核人
            // 
            this.tb审核人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb审核人.Location = new System.Drawing.Point(698, 553);
            this.tb审核人.Name = "tb审核人";
            this.tb审核人.Size = new System.Drawing.Size(100, 26);
            this.tb审核人.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(630, 560);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 55;
            this.label2.Text = "审核员：";
            // 
            // tb检查人
            // 
            this.tb检查人.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb检查人.Location = new System.Drawing.Point(101, 554);
            this.tb检查人.Name = "tb检查人";
            this.tb检查人.Size = new System.Drawing.Size(100, 26);
            this.tb检查人.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(33, 561);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "操作员：";
            // 
            // printBtn
            // 
            this.printBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.printBtn.Location = new System.Drawing.Point(1064, 588);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(80, 30);
            this.printBtn.TabIndex = 61;
            this.printBtn.Text = "打印";
            this.printBtn.UseVisualStyleBackColor = true;
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // ReplaceHeadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 623);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp检查日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb审核人);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb检查人);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtp更换日期);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb更换后模头型号);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb更换前模头型号);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb更换原因);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Title);
            this.Name = "ReplaceHeadForm";
            this.Text = "ReplaceHeadForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.DateTimePicker dtp更换日期;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb更换后模头型号;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb更换前模头型号;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb更换原因;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtp检查日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb审核人;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb检查人;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button printBtn;
    }
}