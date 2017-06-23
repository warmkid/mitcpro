namespace mySystem
{
    partial class SetExtruForm
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
            this.cleanPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.bfStartPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.handoverPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.preHeatPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.procClearPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cleanPanel
            // 
            this.cleanPanel.Location = new System.Drawing.Point(15, 48);
            this.cleanPanel.Name = "cleanPanel";
            this.cleanPanel.Size = new System.Drawing.Size(1123, 269);
            this.cleanPanel.TabIndex = 17;
            this.cleanPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.cleanPanel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "清洁区域设置";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CancelBtn.Location = new System.Drawing.Point(1018, 1688);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(81, 33);
            this.CancelBtn.TabIndex = 19;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(916, 1688);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(82, 33);
            this.SaveBtn.TabIndex = 18;
            this.SaveBtn.Text = "保存设置";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // bfStartPanel
            // 
            this.bfStartPanel.Location = new System.Drawing.Point(15, 381);
            this.bfStartPanel.Name = "bfStartPanel";
            this.bfStartPanel.Size = new System.Drawing.Size(1123, 305);
            this.bfStartPanel.TabIndex = 19;
            this.bfStartPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bfStartPanel_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 351);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "开机前确认项目设置";
            // 
            // handoverPanel
            // 
            this.handoverPanel.Location = new System.Drawing.Point(15, 1374);
            this.handoverPanel.Name = "handoverPanel";
            this.handoverPanel.Size = new System.Drawing.Size(1123, 269);
            this.handoverPanel.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(13, 1343);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(195, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "岗位交接班确认项目设置";
            // 
            // preHeatPanel
            // 
            this.preHeatPanel.Location = new System.Drawing.Point(15, 736);
            this.preHeatPanel.Name = "preHeatPanel";
            this.preHeatPanel.Size = new System.Drawing.Size(1123, 275);
            this.preHeatPanel.TabIndex = 23;
            this.preHeatPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.preHeatPanel_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 706);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "预热参数设置";
            // 
            // procClearPanel
            // 
            this.procClearPanel.Location = new System.Drawing.Point(15, 1058);
            this.procClearPanel.Name = "procClearPanel";
            this.procClearPanel.Size = new System.Drawing.Size(1123, 269);
            this.procClearPanel.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 1028);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "工序清场设置";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(862, 1727);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 15);
            this.panel1.TabIndex = 26;
            // 
            // SetExtruForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.procClearPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.preHeatPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.handoverPanel);
            this.Controls.Add(this.bfStartPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cleanPanel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SetExtruForm";
            this.Text = "SetExtruForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel cleanPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Panel bfStartPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel handoverPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel preHeatPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel procClearPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
    }
}