namespace mySystem
{
    partial class BlowForm
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
            this.销售单号TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.自编号TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 销售单号TextBox
            // 
            this.销售单号TextBox.Location = new System.Drawing.Point(194, 23);
            this.销售单号TextBox.Name = "销售单号TextBox";
            this.销售单号TextBox.ReadOnly = true;
            this.销售单号TextBox.Size = new System.Drawing.Size(153, 21);
            this.销售单号TextBox.TabIndex = 62;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 61;
            this.label2.Text = "任务：";
            // 
            // 自编号TextBox
            // 
            this.自编号TextBox.Location = new System.Drawing.Point(54, 23);
            this.自编号TextBox.Name = "自编号TextBox";
            this.自编号TextBox.ReadOnly = true;
            this.自编号TextBox.Size = new System.Drawing.Size(92, 21);
            this.自编号TextBox.TabIndex = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 59;
            this.label1.Text = "日期：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(420, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(92, 21);
            this.textBox1.TabIndex = 64;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 63;
            this.label3.Text = "计划日期：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(359, 56);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(153, 21);
            this.textBox2.TabIndex = 68;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(320, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 67;
            this.label4.Text = "组装：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(89, 56);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(225, 21);
            this.textBox3.TabIndex = 66;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 65;
            this.label5.Text = "半成品材料：";
            // 
            // BlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.销售单号TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.自编号TextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BlowForm";
            this.Text = "BlowForm";
            this.Load += new System.EventHandler(this.BlowForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 销售单号TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 自编号TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
    }
}