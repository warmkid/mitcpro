﻿namespace mySystem.Process.Bag.PTV
{
    partial class PTVMainForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Btn生产指令 = new System.Windows.Forms.Button();
            this.Btn开机前确认 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn生产领料 = new System.Windows.Forms.Button();
            this.Btn产品内包装 = new System.Windows.Forms.Button();
            this.Btn瓶口焊接机 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Btn生产日报表 = new System.Windows.Forms.Button();
            this.Btn底封机 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Btn批生产记录 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn泄露测试 = new System.Windows.Forms.Button();
            this.Btn清场记录 = new System.Windows.Forms.Button();
            this.Btn超声波 = new System.Windows.Forms.Button();
            this.Btn圆口焊接机 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(97, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(290, 27);
            this.comboBox1.TabIndex = 29;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Btn生产指令
            // 
            this.Btn生产指令.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn生产指令.Location = new System.Drawing.Point(31, 40);
            this.Btn生产指令.Name = "Btn生产指令";
            this.Btn生产指令.Size = new System.Drawing.Size(200, 38);
            this.Btn生产指令.TabIndex = 12;
            this.Btn生产指令.Text = "PTV生产指令";
            this.Btn生产指令.UseVisualStyleBackColor = true;
            this.Btn生产指令.Click += new System.EventHandler(this.B1Btn_Click);
            // 
            // Btn开机前确认
            // 
            this.Btn开机前确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn开机前确认.Location = new System.Drawing.Point(31, 100);
            this.Btn开机前确认.Name = "Btn开机前确认";
            this.Btn开机前确认.Size = new System.Drawing.Size(200, 38);
            this.Btn开机前确认.TabIndex = 16;
            this.Btn开机前确认.Text = "PTV生产开机确认表";
            this.Btn开机前确认.UseVisualStyleBackColor = true;
            this.Btn开机前确认.Click += new System.EventHandler(this.B2Btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "生产指令：";
            // 
            // Btn生产领料
            // 
            this.Btn生产领料.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn生产领料.Location = new System.Drawing.Point(28, 40);
            this.Btn生产领料.Name = "Btn生产领料";
            this.Btn生产领料.Size = new System.Drawing.Size(200, 38);
            this.Btn生产领料.TabIndex = 12;
            this.Btn生产领料.Text = "生产领料使用记录";
            this.Btn生产领料.UseVisualStyleBackColor = true;
            this.Btn生产领料.Click += new System.EventHandler(this.A1Btn_Click);
            // 
            // Btn产品内包装
            // 
            this.Btn产品内包装.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn产品内包装.Location = new System.Drawing.Point(28, 110);
            this.Btn产品内包装.Name = "Btn产品内包装";
            this.Btn产品内包装.Size = new System.Drawing.Size(200, 38);
            this.Btn产品内包装.TabIndex = 16;
            this.Btn产品内包装.Text = "产品内包装记录";
            this.Btn产品内包装.UseVisualStyleBackColor = true;
            this.Btn产品内包装.Click += new System.EventHandler(this.A2Btn_Click);
            // 
            // Btn瓶口焊接机
            // 
            this.Btn瓶口焊接机.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn瓶口焊接机.Location = new System.Drawing.Point(31, 400);
            this.Btn瓶口焊接机.Name = "Btn瓶口焊接机";
            this.Btn瓶口焊接机.Size = new System.Drawing.Size(200, 38);
            this.Btn瓶口焊接机.TabIndex = 15;
            this.Btn瓶口焊接机.Text = "瓶口焊接机运行记录";
            this.Btn瓶口焊接机.UseVisualStyleBackColor = true;
            this.Btn瓶口焊接机.Click += new System.EventHandler(this.B7Btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Btn生产领料);
            this.groupBox2.Controls.Add(this.Btn产品内包装);
            this.groupBox2.Controls.Add(this.Btn生产日报表);
            this.groupBox2.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(112, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox2.Size = new System.Drawing.Size(260, 321);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "物料流转";
            // 
            // Btn生产日报表
            // 
            this.Btn生产日报表.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn生产日报表.Location = new System.Drawing.Point(28, 180);
            this.Btn生产日报表.Name = "Btn生产日报表";
            this.Btn生产日报表.Size = new System.Drawing.Size(200, 38);
            this.Btn生产日报表.TabIndex = 11;
            this.Btn生产日报表.Text = "PTV生产日报表";
            this.Btn生产日报表.UseVisualStyleBackColor = true;
            this.Btn生产日报表.Click += new System.EventHandler(this.A3Btn_Click);
            // 
            // Btn底封机
            // 
            this.Btn底封机.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn底封机.Location = new System.Drawing.Point(31, 160);
            this.Btn底封机.Name = "Btn底封机";
            this.Btn底封机.Size = new System.Drawing.Size(200, 38);
            this.Btn底封机.TabIndex = 11;
            this.Btn底封机.Text = "底封机运行记录";
            this.Btn底封机.UseVisualStyleBackColor = true;
            this.Btn底封机.Click += new System.EventHandler(this.B3Btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Btn批生产记录);
            this.groupBox3.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(796, 105);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox3.Size = new System.Drawing.Size(260, 321);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "封面";
            // 
            // Btn批生产记录
            // 
            this.Btn批生产记录.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn批生产记录.Location = new System.Drawing.Point(28, 40);
            this.Btn批生产记录.Name = "Btn批生产记录";
            this.Btn批生产记录.Size = new System.Drawing.Size(200, 38);
            this.Btn批生产记录.TabIndex = 12;
            this.Btn批生产记录.Text = "制袋工序批生产记录";
            this.Btn批生产记录.UseVisualStyleBackColor = true;
            this.Btn批生产记录.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn泄露测试);
            this.groupBox1.Controls.Add(this.Btn清场记录);
            this.groupBox1.Controls.Add(this.Btn瓶口焊接机);
            this.groupBox1.Controls.Add(this.Btn超声波);
            this.groupBox1.Controls.Add(this.Btn生产指令);
            this.groupBox1.Controls.Add(this.Btn开机前确认);
            this.groupBox1.Controls.Add(this.Btn圆口焊接机);
            this.groupBox1.Controls.Add(this.Btn底封机);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(443, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(260, 505);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "管理类";
            // 
            // Btn泄露测试
            // 
            this.Btn泄露测试.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn泄露测试.Location = new System.Drawing.Point(31, 280);
            this.Btn泄露测试.Name = "Btn泄露测试";
            this.Btn泄露测试.Size = new System.Drawing.Size(200, 38);
            this.Btn泄露测试.TabIndex = 19;
            this.Btn泄露测试.Text = "泄露测试记录";
            this.Btn泄露测试.UseVisualStyleBackColor = true;
            this.Btn泄露测试.Click += new System.EventHandler(this.B5Btn_Click);
            // 
            // Btn清场记录
            // 
            this.Btn清场记录.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn清场记录.Location = new System.Drawing.Point(31, 460);
            this.Btn清场记录.Name = "Btn清场记录";
            this.Btn清场记录.Size = new System.Drawing.Size(200, 38);
            this.Btn清场记录.TabIndex = 18;
            this.Btn清场记录.Text = "清场记录";
            this.Btn清场记录.UseVisualStyleBackColor = true;
            this.Btn清场记录.Click += new System.EventHandler(this.B8Btn_Click);
            // 
            // Btn超声波
            // 
            this.Btn超声波.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn超声波.Location = new System.Drawing.Point(31, 340);
            this.Btn超声波.Name = "Btn超声波";
            this.Btn超声波.Size = new System.Drawing.Size(200, 38);
            this.Btn超声波.TabIndex = 17;
            this.Btn超声波.Text = "超声波焊接记录";
            this.Btn超声波.UseVisualStyleBackColor = true;
            this.Btn超声波.Click += new System.EventHandler(this.B6Btn_Click);
            // 
            // Btn圆口焊接机
            // 
            this.Btn圆口焊接机.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn圆口焊接机.Location = new System.Drawing.Point(31, 220);
            this.Btn圆口焊接机.Name = "Btn圆口焊接机";
            this.Btn圆口焊接机.Size = new System.Drawing.Size(200, 38);
            this.Btn圆口焊接机.TabIndex = 15;
            this.Btn圆口焊接机.Text = "圆口焊接机运行记录";
            this.Btn圆口焊接机.UseVisualStyleBackColor = true;
            this.Btn圆口焊接机.Click += new System.EventHandler(this.B4Btn_Click);
            // 
            // PTVMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 623);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PTVMainForm";
            this.Text = "PTVMainForm";
            this.Load += new System.EventHandler(this.PTVMainForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Btn生产指令;
        private System.Windows.Forms.Button Btn开机前确认;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn生产领料;
        private System.Windows.Forms.Button Btn产品内包装;
        private System.Windows.Forms.Button Btn瓶口焊接机;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn生产日报表;
        private System.Windows.Forms.Button Btn底封机;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Btn批生产记录;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn圆口焊接机;
        private System.Windows.Forms.Button Btn泄露测试;
        private System.Windows.Forms.Button Btn清场记录;
        private System.Windows.Forms.Button Btn超声波;
    }
}