namespace mySystem
{
    partial class ProcessMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessMainForm));
            this.KillBtn = new System.Windows.Forms.Button();
            this.CleanBtn = new System.Windows.Forms.Button();
            this.BagBtn = new System.Windows.Forms.Button();
            this.ProducePanelLeft = new System.Windows.Forms.Panel();
            this.bagPanel = new System.Windows.Forms.Panel();
            this.bag6Btn = new System.Windows.Forms.Button();
            this.BTVbagBtn = new System.Windows.Forms.Button();
            this.PTVbagBtn = new System.Windows.Forms.Button();
            this.bag3Btn = new System.Windows.Forms.Button();
            this.CSbagBtn = new System.Windows.Forms.Button();
            this.LDPEbagBtn = new System.Windows.Forms.Button();
            this.otherPanel = new System.Windows.Forms.Panel();
            this.StockBtn = new System.Windows.Forms.Button();
            this.PlanBtn = new System.Windows.Forms.Button();
            this.OrderBtn = new System.Windows.Forms.Button();
            this.ExtructionBtn = new System.Windows.Forms.Button();
            this.ProducePanelRight = new System.Windows.Forms.Panel();
            this.ProducePanelLeft.SuspendLayout();
            this.bagPanel.SuspendLayout();
            this.otherPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // KillBtn
            // 
            this.KillBtn.FlatAppearance.BorderSize = 0;
            this.KillBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KillBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.KillBtn.Image = ((System.Drawing.Image)(resources.GetObject("KillBtn.Image")));
            this.KillBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.KillBtn.Location = new System.Drawing.Point(3, 3);
            this.KillBtn.Name = "KillBtn";
            this.KillBtn.Size = new System.Drawing.Size(172, 43);
            this.KillBtn.TabIndex = 5;
            this.KillBtn.Text = "    灭菌";
            this.KillBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.KillBtn.UseVisualStyleBackColor = true;
            this.KillBtn.Click += new System.EventHandler(this.KillBtn_Click);
            // 
            // CleanBtn
            // 
            this.CleanBtn.FlatAppearance.BorderSize = 0;
            this.CleanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CleanBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CleanBtn.Image = ((System.Drawing.Image)(resources.GetObject("CleanBtn.Image")));
            this.CleanBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CleanBtn.Location = new System.Drawing.Point(3, 47);
            this.CleanBtn.Name = "CleanBtn";
            this.CleanBtn.Size = new System.Drawing.Size(172, 43);
            this.CleanBtn.TabIndex = 3;
            this.CleanBtn.Text = "    清洁分切";
            this.CleanBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CleanBtn.UseVisualStyleBackColor = true;
            this.CleanBtn.Click += new System.EventHandler(this.CleanBtn_Click);
            // 
            // BagBtn
            // 
            this.BagBtn.FlatAppearance.BorderSize = 0;
            this.BagBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BagBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BagBtn.Image = ((System.Drawing.Image)(resources.GetObject("BagBtn.Image")));
            this.BagBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BagBtn.Location = new System.Drawing.Point(3, 90);
            this.BagBtn.Name = "BagBtn";
            this.BagBtn.Size = new System.Drawing.Size(172, 43);
            this.BagBtn.TabIndex = 4;
            this.BagBtn.Text = "    制袋";
            this.BagBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BagBtn.UseVisualStyleBackColor = true;
            this.BagBtn.Click += new System.EventHandler(this.BagBtn_Click);
            // 
            // ProducePanelLeft
            // 
            this.ProducePanelLeft.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ProducePanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProducePanelLeft.Controls.Add(this.otherPanel);
            this.ProducePanelLeft.Controls.Add(this.BagBtn);
            this.ProducePanelLeft.Controls.Add(this.CleanBtn);
            this.ProducePanelLeft.Controls.Add(this.ExtructionBtn);
            this.ProducePanelLeft.Controls.Add(this.bagPanel);
            this.ProducePanelLeft.Location = new System.Drawing.Point(4, 2);
            this.ProducePanelLeft.Name = "ProducePanelLeft";
            this.ProducePanelLeft.Size = new System.Drawing.Size(180, 615);
            this.ProducePanelLeft.TabIndex = 4;
            // 
            // bagPanel
            // 
            this.bagPanel.Controls.Add(this.bag6Btn);
            this.bagPanel.Controls.Add(this.BTVbagBtn);
            this.bagPanel.Controls.Add(this.PTVbagBtn);
            this.bagPanel.Controls.Add(this.bag3Btn);
            this.bagPanel.Controls.Add(this.CSbagBtn);
            this.bagPanel.Controls.Add(this.LDPEbagBtn);
            this.bagPanel.Location = new System.Drawing.Point(-1, 128);
            this.bagPanel.Name = "bagPanel";
            this.bagPanel.Size = new System.Drawing.Size(180, 232);
            this.bagPanel.TabIndex = 9;
            this.bagPanel.Visible = false;
            // 
            // bag6Btn
            // 
            this.bag6Btn.FlatAppearance.BorderSize = 0;
            this.bag6Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bag6Btn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bag6Btn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bag6Btn.Location = new System.Drawing.Point(2, 193);
            this.bag6Btn.Name = "bag6Btn";
            this.bag6Btn.Size = new System.Drawing.Size(173, 38);
            this.bag6Btn.TabIndex = 15;
            this.bag6Btn.Text = "    6.防护罩";
            this.bag6Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bag6Btn.UseVisualStyleBackColor = true;
            this.bag6Btn.Click += new System.EventHandler(this.bag6Btn_Click);
            // 
            // BTVbagBtn
            // 
            this.BTVbagBtn.FlatAppearance.BorderSize = 0;
            this.BTVbagBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTVbagBtn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BTVbagBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTVbagBtn.Location = new System.Drawing.Point(3, 155);
            this.BTVbagBtn.Name = "BTVbagBtn";
            this.BTVbagBtn.Size = new System.Drawing.Size(173, 38);
            this.BTVbagBtn.TabIndex = 14;
            this.BTVbagBtn.Text = "    5.BPV制袋";
            this.BTVbagBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTVbagBtn.UseVisualStyleBackColor = true;
            this.BTVbagBtn.Click += new System.EventHandler(this.BTVbagBtn_Click);
            // 
            // PTVbagBtn
            // 
            this.PTVbagBtn.FlatAppearance.BorderSize = 0;
            this.PTVbagBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PTVbagBtn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PTVbagBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PTVbagBtn.Location = new System.Drawing.Point(3, 117);
            this.PTVbagBtn.Name = "PTVbagBtn";
            this.PTVbagBtn.Size = new System.Drawing.Size(173, 38);
            this.PTVbagBtn.TabIndex = 13;
            this.PTVbagBtn.Text = "    4.PTV制袋";
            this.PTVbagBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PTVbagBtn.UseVisualStyleBackColor = true;
            this.PTVbagBtn.Click += new System.EventHandler(this.PTVbagBtn_Click);
            // 
            // bag3Btn
            // 
            this.bag3Btn.FlatAppearance.BorderSize = 0;
            this.bag3Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bag3Btn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bag3Btn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bag3Btn.Location = new System.Drawing.Point(3, 79);
            this.bag3Btn.Name = "bag3Btn";
            this.bag3Btn.Size = new System.Drawing.Size(173, 38);
            this.bag3Btn.TabIndex = 12;
            this.bag3Btn.Text = "    3.连续袋";
            this.bag3Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bag3Btn.UseVisualStyleBackColor = true;
            this.bag3Btn.Click += new System.EventHandler(this.bag3Btn_Click);
            // 
            // CSbagBtn
            // 
            this.CSbagBtn.FlatAppearance.BorderSize = 0;
            this.CSbagBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CSbagBtn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CSbagBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CSbagBtn.Location = new System.Drawing.Point(4, 41);
            this.CSbagBtn.Name = "CSbagBtn";
            this.CSbagBtn.Size = new System.Drawing.Size(173, 38);
            this.CSbagBtn.TabIndex = 11;
            this.CSbagBtn.Text = "    2.CS制袋";
            this.CSbagBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CSbagBtn.UseVisualStyleBackColor = true;
            this.CSbagBtn.Click += new System.EventHandler(this.CSbagBtn_Click);
            // 
            // LDPEbagBtn
            // 
            this.LDPEbagBtn.FlatAppearance.BorderSize = 0;
            this.LDPEbagBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LDPEbagBtn.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LDPEbagBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LDPEbagBtn.Location = new System.Drawing.Point(4, 3);
            this.LDPEbagBtn.Name = "LDPEbagBtn";
            this.LDPEbagBtn.Size = new System.Drawing.Size(173, 38);
            this.LDPEbagBtn.TabIndex = 10;
            this.LDPEbagBtn.Text = "    1.PE制袋";
            this.LDPEbagBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LDPEbagBtn.UseVisualStyleBackColor = true;
            this.LDPEbagBtn.Click += new System.EventHandler(this.LDPEbagBtn_Click);
            // 
            // otherPanel
            // 
            this.otherPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.otherPanel.Controls.Add(this.KillBtn);
            this.otherPanel.Controls.Add(this.StockBtn);
            this.otherPanel.Controls.Add(this.PlanBtn);
            this.otherPanel.Controls.Add(this.OrderBtn);
            this.otherPanel.Location = new System.Drawing.Point(0, 130);
            this.otherPanel.Name = "otherPanel";
            this.otherPanel.Size = new System.Drawing.Size(177, 176);
            this.otherPanel.TabIndex = 10;
            // 
            // StockBtn
            // 
            this.StockBtn.FlatAppearance.BorderSize = 0;
            this.StockBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StockBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StockBtn.Image = ((System.Drawing.Image)(resources.GetObject("StockBtn.Image")));
            this.StockBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StockBtn.Location = new System.Drawing.Point(3, 132);
            this.StockBtn.Name = "StockBtn";
            this.StockBtn.Size = new System.Drawing.Size(172, 43);
            this.StockBtn.TabIndex = 8;
            this.StockBtn.Text = "    库存管理";
            this.StockBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StockBtn.UseVisualStyleBackColor = true;
            this.StockBtn.Click += new System.EventHandler(this.StockBtn_Click);
            // 
            // PlanBtn
            // 
            this.PlanBtn.FlatAppearance.BorderSize = 0;
            this.PlanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlanBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlanBtn.Image = ((System.Drawing.Image)(resources.GetObject("PlanBtn.Image")));
            this.PlanBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PlanBtn.Location = new System.Drawing.Point(3, 46);
            this.PlanBtn.Name = "PlanBtn";
            this.PlanBtn.Size = new System.Drawing.Size(172, 43);
            this.PlanBtn.TabIndex = 6;
            this.PlanBtn.Text = "    生产指令";
            this.PlanBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PlanBtn.UseVisualStyleBackColor = true;
            this.PlanBtn.Click += new System.EventHandler(this.PlanBtn_Click);
            // 
            // OrderBtn
            // 
            this.OrderBtn.FlatAppearance.BorderSize = 0;
            this.OrderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OrderBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OrderBtn.Image = ((System.Drawing.Image)(resources.GetObject("OrderBtn.Image")));
            this.OrderBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderBtn.Location = new System.Drawing.Point(3, 89);
            this.OrderBtn.Name = "OrderBtn";
            this.OrderBtn.Size = new System.Drawing.Size(172, 43);
            this.OrderBtn.TabIndex = 7;
            this.OrderBtn.Text = "    订单管理";
            this.OrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderBtn.UseVisualStyleBackColor = true;
            this.OrderBtn.Click += new System.EventHandler(this.OrderBtn_Click);
            // 
            // ExtructionBtn
            // 
            this.ExtructionBtn.FlatAppearance.BorderSize = 0;
            this.ExtructionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExtructionBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExtructionBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExtructionBtn.Image")));
            this.ExtructionBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExtructionBtn.Location = new System.Drawing.Point(3, 4);
            this.ExtructionBtn.Name = "ExtructionBtn";
            this.ExtructionBtn.Size = new System.Drawing.Size(172, 43);
            this.ExtructionBtn.TabIndex = 0;
            this.ExtructionBtn.Text = "    吹膜";
            this.ExtructionBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExtructionBtn.UseVisualStyleBackColor = true;
            this.ExtructionBtn.Click += new System.EventHandler(this.ExtructionBtn_Click);
            // 
            // ProducePanelRight
            // 
            this.ProducePanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProducePanelRight.Location = new System.Drawing.Point(185, 2);
            this.ProducePanelRight.Name = "ProducePanelRight";
            this.ProducePanelRight.Size = new System.Drawing.Size(1170, 615);
            this.ProducePanelRight.TabIndex = 5;
            // 
            // ProcessMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 620);
            this.Controls.Add(this.ProducePanelLeft);
            this.Controls.Add(this.ProducePanelRight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProcessMainForm";
            this.Text = "ProcessMainForm";
            this.ProducePanelLeft.ResumeLayout(false);
            this.bagPanel.ResumeLayout(false);
            this.otherPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button KillBtn;
        private System.Windows.Forms.Button CleanBtn;
        private System.Windows.Forms.Button BagBtn;
        private System.Windows.Forms.Panel ProducePanelLeft;
        private System.Windows.Forms.Button ExtructionBtn;
        private System.Windows.Forms.Button StockBtn;
        private System.Windows.Forms.Button OrderBtn;
        private System.Windows.Forms.Button PlanBtn;
        private System.Windows.Forms.Panel bagPanel;
        private System.Windows.Forms.Button bag6Btn;
        private System.Windows.Forms.Button BTVbagBtn;
        private System.Windows.Forms.Button PTVbagBtn;
        private System.Windows.Forms.Button bag3Btn;
        private System.Windows.Forms.Button CSbagBtn;
        private System.Windows.Forms.Button LDPEbagBtn;
        private System.Windows.Forms.Panel otherPanel;
        public System.Windows.Forms.Panel ProducePanelRight;
    }
}