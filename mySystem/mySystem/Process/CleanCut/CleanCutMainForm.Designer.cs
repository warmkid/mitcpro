namespace mySystem.Process.CleanCut
{
    partial class CleanCutMainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.Btn生产指令 = new System.Windows.Forms.Button();
            this.Btn清场 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Btn标签 = new System.Windows.Forms.Button();
            this.Btn日报表 = new System.Windows.Forms.Button();
            this.Btn生产记录 = new System.Windows.Forms.Button();
            this.Btn开机确认 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn运行记录 = new System.Windows.Forms.Button();
            this.Btn批生产 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn工序结束 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(97, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(290, 27);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "生产指令：";
            // 
            // Btn生产指令
            // 
            this.Btn生产指令.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn生产指令.Location = new System.Drawing.Point(15, 42);
            this.Btn生产指令.Name = "Btn生产指令";
            this.Btn生产指令.Size = new System.Drawing.Size(269, 38);
            this.Btn生产指令.TabIndex = 12;
            this.Btn生产指令.Text = "01 清洁分切生产指令";
            this.Btn生产指令.UseVisualStyleBackColor = true;
            this.Btn生产指令.Click += new System.EventHandler(this.A1Btn_Click);
            // 
            // Btn清场
            // 
            this.Btn清场.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn清场.Location = new System.Drawing.Point(15, 250);
            this.Btn清场.Name = "Btn清场";
            this.Btn清场.Size = new System.Drawing.Size(269, 38);
            this.Btn清场.TabIndex = 16;
            this.Btn清场.Text = "07 清场记录";
            this.Btn清场.UseVisualStyleBackColor = true;
            this.Btn清场.Click += new System.EventHandler(this.A2Btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Btn标签);
            this.groupBox2.Controls.Add(this.Btn日报表);
            this.groupBox2.Controls.Add(this.Btn生产记录);
            this.groupBox2.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(97, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox2.Size = new System.Drawing.Size(302, 324);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "物料流转";
            // 
            // Btn标签
            // 
            this.Btn标签.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn标签.Location = new System.Drawing.Point(15, 182);
            this.Btn标签.Name = "Btn标签";
            this.Btn标签.Size = new System.Drawing.Size(269, 38);
            this.Btn标签.TabIndex = 18;
            this.Btn标签.Text = "06 清洁分切标签";
            this.Btn标签.UseVisualStyleBackColor = true;
            this.Btn标签.Click += new System.EventHandler(this.A6Btn_Click);
            // 
            // Btn日报表
            // 
            this.Btn日报表.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn日报表.Location = new System.Drawing.Point(15, 112);
            this.Btn日报表.Name = "Btn日报表";
            this.Btn日报表.Size = new System.Drawing.Size(269, 38);
            this.Btn日报表.TabIndex = 17;
            this.Btn日报表.Text = "05 清洁分切日报表";
            this.Btn日报表.UseVisualStyleBackColor = true;
            this.Btn日报表.Click += new System.EventHandler(this.A5Btn_Click);
            // 
            // Btn生产记录
            // 
            this.Btn生产记录.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn生产记录.Location = new System.Drawing.Point(15, 42);
            this.Btn生产记录.Name = "Btn生产记录";
            this.Btn生产记录.Size = new System.Drawing.Size(269, 38);
            this.Btn生产记录.TabIndex = 15;
            this.Btn生产记录.Text = "04 清洁分切生产记录";
            this.Btn生产记录.UseVisualStyleBackColor = true;
            this.Btn生产记录.Click += new System.EventHandler(this.A4Btn_Click);
            // 
            // Btn开机确认
            // 
            this.Btn开机确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn开机确认.Location = new System.Drawing.Point(15, 112);
            this.Btn开机确认.Name = "Btn开机确认";
            this.Btn开机确认.Size = new System.Drawing.Size(269, 38);
            this.Btn开机确认.TabIndex = 11;
            this.Btn开机确认.Text = "02 清洁分切机开机前确认";
            this.Btn开机确认.UseVisualStyleBackColor = true;
            this.Btn开机确认.Click += new System.EventHandler(this.A3Btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn运行记录);
            this.groupBox1.Controls.Add(this.Btn开机确认);
            this.groupBox1.Controls.Add(this.Btn清场);
            this.groupBox1.Controls.Add(this.Btn生产指令);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(464, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(302, 324);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "管理类";
            // 
            // Btn运行记录
            // 
            this.Btn运行记录.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn运行记录.Location = new System.Drawing.Point(15, 182);
            this.Btn运行记录.Name = "Btn运行记录";
            this.Btn运行记录.Size = new System.Drawing.Size(269, 38);
            this.Btn运行记录.TabIndex = 17;
            this.Btn运行记录.Text = "03 清洁分切运行记录";
            this.Btn运行记录.UseVisualStyleBackColor = true;
            this.Btn运行记录.Click += new System.EventHandler(this.A8Btn_Click);
            // 
            // Btn批生产
            // 
            this.Btn批生产.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn批生产.Location = new System.Drawing.Point(15, 42);
            this.Btn批生产.Name = "Btn批生产";
            this.Btn批生产.Size = new System.Drawing.Size(269, 38);
            this.Btn批生产.TabIndex = 17;
            this.Btn批生产.Text = "00 清洁分切批生产记录";
            this.Btn批生产.UseVisualStyleBackColor = true;
            this.Btn批生产.Click += new System.EventHandler(this.A7Btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn工序结束);
            this.groupBox3.Controls.Add(this.Btn批生产);
            this.groupBox3.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(815, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox3.Size = new System.Drawing.Size(302, 324);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "封面";
            // 
            // btn工序结束
            // 
            this.btn工序结束.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn工序结束.Location = new System.Drawing.Point(15, 112);
            this.btn工序结束.Name = "btn工序结束";
            this.btn工序结束.Size = new System.Drawing.Size(269, 38);
            this.btn工序结束.TabIndex = 18;
            this.btn工序结束.Text = "工序结束";
            this.btn工序结束.UseVisualStyleBackColor = true;
            this.btn工序结束.Click += new System.EventHandler(this.btn工序结束_Click);
            // 
            // CleanCutMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CleanCutMainForm";
            this.Text = "CleanCutMainForm";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn生产指令;
        private System.Windows.Forms.Button Btn清场;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn生产记录;
        private System.Windows.Forms.Button Btn开机确认;
        private System.Windows.Forms.Button Btn日报表;
        private System.Windows.Forms.Button Btn标签;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn批生产;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Btn运行记录;
        private System.Windows.Forms.Button btn工序结束;
    }
}