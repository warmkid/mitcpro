namespace mySystem.Other
{
    partial class AutoUpdate
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
            this.progressBar总 = new System.Windows.Forms.ProgressBar();
            this.progressBar文件 = new System.Windows.Forms.ProgressBar();
            this.label总 = new System.Windows.Forms.Label();
            this.label文件 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStart = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar总
            // 
            this.progressBar总.Location = new System.Drawing.Point(23, 23);
            this.progressBar总.Name = "progressBar总";
            this.progressBar总.Size = new System.Drawing.Size(294, 23);
            this.progressBar总.TabIndex = 0;
            this.progressBar总.UseWaitCursor = true;
            // 
            // progressBar文件
            // 
            this.progressBar文件.Location = new System.Drawing.Point(23, 64);
            this.progressBar文件.Name = "progressBar文件";
            this.progressBar文件.Size = new System.Drawing.Size(294, 23);
            this.progressBar文件.TabIndex = 1;
            this.progressBar文件.UseWaitCursor = true;
            // 
            // label总
            // 
            this.label总.AutoSize = true;
            this.label总.Location = new System.Drawing.Point(23, 49);
            this.label总.Name = "label总";
            this.label总.Size = new System.Drawing.Size(65, 12);
            this.label总.TabIndex = 2;
            this.label总.Text = "总进度 0/0";
            // 
            // label文件
            // 
            this.label文件.AutoSize = true;
            this.label文件.Location = new System.Drawing.Point(23, 90);
            this.label文件.Name = "label文件";
            this.label文件.Size = new System.Drawing.Size(119, 12);
            this.label文件.TabIndex = 3;
            this.label文件.Text = "当前文件下载进度 0%";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(23, 134);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(294, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.UseWaitCursor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.progressBar总);
            this.flowLayoutPanel1.Controls.Add(this.label总);
            this.flowLayoutPanel1.Controls.Add(this.progressBar文件);
            this.flowLayoutPanel1.Controls.Add(this.label文件);
            this.flowLayoutPanel1.Controls.Add(this.btnStart);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(30, 30);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(30);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(344, 216);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(23, 105);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(294, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = " 开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.UseWaitCursor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // AutoUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 276);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AutoUpdate";
            this.Padding = new System.Windows.Forms.Padding(30);
            this.Text = "自动更新";
            this.Load += new System.EventHandler(this.AutoUpdate_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar总;
        private System.Windows.Forms.ProgressBar progressBar文件;
        private System.Windows.Forms.Label label总;
        private System.Windows.Forms.Label label文件;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnStart;
    }
}