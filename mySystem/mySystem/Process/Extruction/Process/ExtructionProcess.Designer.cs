namespace mySystem.Extruction.Process
{
    partial class ExtructionProcess
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
            this.SaveBtn = new System.Windows.Forms.Button();
            this.BackBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.StepViewPanel = new System.Windows.Forms.Panel();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(740, 476);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 0;
            this.SaveBtn.Text = "确定";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // BackBtn
            // 
            this.BackBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.BackBtn.Location = new System.Drawing.Point(978, 476);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(80, 30);
            this.BackBtn.TabIndex = 1;
            this.BackBtn.Text = "上一页";
            this.BackBtn.UseVisualStyleBackColor = true;
            this.BackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.NextBtn.Location = new System.Drawing.Point(1068, 476);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(80, 30);
            this.NextBtn.TabIndex = 2;
            this.NextBtn.Text = "下一页";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // StepViewPanel
            // 
            this.StepViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StepViewPanel.AutoScroll = true;
            this.StepViewPanel.Location = new System.Drawing.Point(0, 0);
            this.StepViewPanel.Name = "StepViewPanel";
            this.StepViewPanel.Size = new System.Drawing.Size(1168, 464);
            this.StepViewPanel.TabIndex = 3;
            this.StepViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.StepViewPanel_Paint);
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(835, 476);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 4;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // ExtructionProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 520);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.StepViewPanel);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.BackBtn);
            this.Controls.Add(this.SaveBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtructionProcess";
            this.Text = "ExtructionProcess";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Panel StepViewPanel;
        private System.Windows.Forms.Button CheckBtn;
    }
}