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
            this.Btn灭菌 = new System.Windows.Forms.Button();
            this.Btn清洁分切 = new System.Windows.Forms.Button();
            this.Btn制袋 = new System.Windows.Forms.Button();
            this.ProducePanelLeft = new System.Windows.Forms.Panel();
            this.Panel其他按钮 = new System.Windows.Forms.Panel();
            this.Btn库存管理 = new System.Windows.Forms.Button();
            this.Btn生产指令 = new System.Windows.Forms.Button();
            this.Btn订单管理 = new System.Windows.Forms.Button();
            this.Btn吹膜 = new System.Windows.Forms.Button();
            this.Panel制袋 = new System.Windows.Forms.Panel();
            this.Btn防护罩 = new System.Windows.Forms.Button();
            this.BtnBPV制袋 = new System.Windows.Forms.Button();
            this.BtnPTV制袋 = new System.Windows.Forms.Button();
            this.Btn连续袋 = new System.Windows.Forms.Button();
            this.BtnCS制袋 = new System.Windows.Forms.Button();
            this.BtnPE制袋 = new System.Windows.Forms.Button();
            this.ProducePanelRight = new System.Windows.Forms.Panel();
            this.ProducePanelLeft.SuspendLayout();
            this.Panel其他按钮.SuspendLayout();
            this.Panel制袋.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn灭菌
            // 
            this.Btn灭菌.FlatAppearance.BorderSize = 0;
            this.Btn灭菌.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn灭菌.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn灭菌.Image = ((System.Drawing.Image)(resources.GetObject("Btn灭菌.Image")));
            this.Btn灭菌.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn灭菌.Location = new System.Drawing.Point(3, 3);
            this.Btn灭菌.Name = "Btn灭菌";
            this.Btn灭菌.Size = new System.Drawing.Size(172, 43);
            this.Btn灭菌.TabIndex = 5;
            this.Btn灭菌.Text = "    灭菌";
            this.Btn灭菌.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn灭菌.UseVisualStyleBackColor = true;
            this.Btn灭菌.Click += new System.EventHandler(this.KillBtn_Click);
            // 
            // Btn清洁分切
            // 
            this.Btn清洁分切.FlatAppearance.BorderSize = 0;
            this.Btn清洁分切.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn清洁分切.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn清洁分切.Image = ((System.Drawing.Image)(resources.GetObject("Btn清洁分切.Image")));
            this.Btn清洁分切.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn清洁分切.Location = new System.Drawing.Point(3, 47);
            this.Btn清洁分切.Name = "Btn清洁分切";
            this.Btn清洁分切.Size = new System.Drawing.Size(172, 43);
            this.Btn清洁分切.TabIndex = 3;
            this.Btn清洁分切.Text = "    清洁分切";
            this.Btn清洁分切.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn清洁分切.UseVisualStyleBackColor = true;
            this.Btn清洁分切.Click += new System.EventHandler(this.CleanBtn_Click);
            // 
            // Btn制袋
            // 
            this.Btn制袋.FlatAppearance.BorderSize = 0;
            this.Btn制袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn制袋.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn制袋.Image = ((System.Drawing.Image)(resources.GetObject("Btn制袋.Image")));
            this.Btn制袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn制袋.Location = new System.Drawing.Point(3, 90);
            this.Btn制袋.Name = "Btn制袋";
            this.Btn制袋.Size = new System.Drawing.Size(172, 43);
            this.Btn制袋.TabIndex = 4;
            this.Btn制袋.Text = "    制袋";
            this.Btn制袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn制袋.UseVisualStyleBackColor = true;
            this.Btn制袋.Click += new System.EventHandler(this.BagBtn_Click);
            // 
            // ProducePanelLeft
            // 
            this.ProducePanelLeft.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ProducePanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProducePanelLeft.Controls.Add(this.Btn制袋);
            this.ProducePanelLeft.Controls.Add(this.Btn清洁分切);
            this.ProducePanelLeft.Controls.Add(this.Btn吹膜);
            this.ProducePanelLeft.Controls.Add(this.Panel制袋);
            this.ProducePanelLeft.Controls.Add(this.Panel其他按钮);
            this.ProducePanelLeft.Location = new System.Drawing.Point(4, 2);
            this.ProducePanelLeft.Name = "ProducePanelLeft";
            this.ProducePanelLeft.Size = new System.Drawing.Size(180, 615);
            this.ProducePanelLeft.TabIndex = 4;
            // 
            // Panel其他按钮
            // 
            this.Panel其他按钮.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Panel其他按钮.Controls.Add(this.Btn灭菌);
            this.Panel其他按钮.Controls.Add(this.Btn库存管理);
            this.Panel其他按钮.Controls.Add(this.Btn生产指令);
            this.Panel其他按钮.Controls.Add(this.Btn订单管理);
            this.Panel其他按钮.Location = new System.Drawing.Point(0, 130);
            this.Panel其他按钮.Name = "Panel其他按钮";
            this.Panel其他按钮.Size = new System.Drawing.Size(177, 176);
            this.Panel其他按钮.TabIndex = 10;
            // 
            // Btn库存管理
            // 
            this.Btn库存管理.FlatAppearance.BorderSize = 0;
            this.Btn库存管理.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn库存管理.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn库存管理.Image = ((System.Drawing.Image)(resources.GetObject("Btn库存管理.Image")));
            this.Btn库存管理.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn库存管理.Location = new System.Drawing.Point(3, 132);
            this.Btn库存管理.Name = "Btn库存管理";
            this.Btn库存管理.Size = new System.Drawing.Size(172, 43);
            this.Btn库存管理.TabIndex = 8;
            this.Btn库存管理.Text = "    库存管理";
            this.Btn库存管理.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn库存管理.UseVisualStyleBackColor = true;
            this.Btn库存管理.Click += new System.EventHandler(this.StockBtn_Click);
            // 
            // Btn生产指令
            // 
            this.Btn生产指令.FlatAppearance.BorderSize = 0;
            this.Btn生产指令.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn生产指令.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn生产指令.Image = ((System.Drawing.Image)(resources.GetObject("Btn生产指令.Image")));
            this.Btn生产指令.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn生产指令.Location = new System.Drawing.Point(3, 46);
            this.Btn生产指令.Name = "Btn生产指令";
            this.Btn生产指令.Size = new System.Drawing.Size(172, 43);
            this.Btn生产指令.TabIndex = 6;
            this.Btn生产指令.Text = "    生产指令";
            this.Btn生产指令.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn生产指令.UseVisualStyleBackColor = true;
            this.Btn生产指令.Click += new System.EventHandler(this.PlanBtn_Click);
            // 
            // Btn订单管理
            // 
            this.Btn订单管理.FlatAppearance.BorderSize = 0;
            this.Btn订单管理.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn订单管理.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn订单管理.Image = ((System.Drawing.Image)(resources.GetObject("Btn订单管理.Image")));
            this.Btn订单管理.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn订单管理.Location = new System.Drawing.Point(3, 89);
            this.Btn订单管理.Name = "Btn订单管理";
            this.Btn订单管理.Size = new System.Drawing.Size(172, 43);
            this.Btn订单管理.TabIndex = 7;
            this.Btn订单管理.Text = "    订单管理";
            this.Btn订单管理.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn订单管理.UseVisualStyleBackColor = true;
            this.Btn订单管理.Click += new System.EventHandler(this.OrderBtn_Click);
            // 
            // Btn吹膜
            // 
            this.Btn吹膜.FlatAppearance.BorderSize = 0;
            this.Btn吹膜.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn吹膜.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn吹膜.Image = ((System.Drawing.Image)(resources.GetObject("Btn吹膜.Image")));
            this.Btn吹膜.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn吹膜.Location = new System.Drawing.Point(3, 4);
            this.Btn吹膜.Name = "Btn吹膜";
            this.Btn吹膜.Size = new System.Drawing.Size(172, 43);
            this.Btn吹膜.TabIndex = 0;
            this.Btn吹膜.Text = "    吹膜";
            this.Btn吹膜.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn吹膜.UseVisualStyleBackColor = true;
            this.Btn吹膜.Click += new System.EventHandler(this.ExtructionBtn_Click);
            // 
            // Panel制袋
            // 
            this.Panel制袋.Controls.Add(this.Btn防护罩);
            this.Panel制袋.Controls.Add(this.BtnBPV制袋);
            this.Panel制袋.Controls.Add(this.BtnPTV制袋);
            this.Panel制袋.Controls.Add(this.Btn连续袋);
            this.Panel制袋.Controls.Add(this.BtnCS制袋);
            this.Panel制袋.Controls.Add(this.BtnPE制袋);
            this.Panel制袋.Location = new System.Drawing.Point(-1, 128);
            this.Panel制袋.Name = "Panel制袋";
            this.Panel制袋.Size = new System.Drawing.Size(180, 232);
            this.Panel制袋.TabIndex = 9;
            this.Panel制袋.Visible = false;
            // 
            // Btn防护罩
            // 
            this.Btn防护罩.FlatAppearance.BorderSize = 0;
            this.Btn防护罩.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn防护罩.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn防护罩.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn防护罩.Location = new System.Drawing.Point(2, 193);
            this.Btn防护罩.Name = "Btn防护罩";
            this.Btn防护罩.Size = new System.Drawing.Size(173, 38);
            this.Btn防护罩.TabIndex = 15;
            this.Btn防护罩.Text = "    6.防护罩";
            this.Btn防护罩.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn防护罩.UseVisualStyleBackColor = true;
            this.Btn防护罩.Click += new System.EventHandler(this.bag6Btn_Click);
            // 
            // BtnBPV制袋
            // 
            this.BtnBPV制袋.FlatAppearance.BorderSize = 0;
            this.BtnBPV制袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBPV制袋.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnBPV制袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnBPV制袋.Location = new System.Drawing.Point(3, 155);
            this.BtnBPV制袋.Name = "BtnBPV制袋";
            this.BtnBPV制袋.Size = new System.Drawing.Size(173, 38);
            this.BtnBPV制袋.TabIndex = 14;
            this.BtnBPV制袋.Text = "    5.BPV制袋";
            this.BtnBPV制袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnBPV制袋.UseVisualStyleBackColor = true;
            this.BtnBPV制袋.Click += new System.EventHandler(this.BTVbagBtn_Click);
            // 
            // BtnPTV制袋
            // 
            this.BtnPTV制袋.FlatAppearance.BorderSize = 0;
            this.BtnPTV制袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPTV制袋.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnPTV制袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPTV制袋.Location = new System.Drawing.Point(3, 117);
            this.BtnPTV制袋.Name = "BtnPTV制袋";
            this.BtnPTV制袋.Size = new System.Drawing.Size(173, 38);
            this.BtnPTV制袋.TabIndex = 13;
            this.BtnPTV制袋.Text = "    4.PTV制袋";
            this.BtnPTV制袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPTV制袋.UseVisualStyleBackColor = true;
            this.BtnPTV制袋.Click += new System.EventHandler(this.PTVbagBtn_Click);
            // 
            // Btn连续袋
            // 
            this.Btn连续袋.FlatAppearance.BorderSize = 0;
            this.Btn连续袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn连续袋.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn连续袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn连续袋.Location = new System.Drawing.Point(3, 79);
            this.Btn连续袋.Name = "Btn连续袋";
            this.Btn连续袋.Size = new System.Drawing.Size(173, 38);
            this.Btn连续袋.TabIndex = 12;
            this.Btn连续袋.Text = "    3.连续袋";
            this.Btn连续袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn连续袋.UseVisualStyleBackColor = true;
            this.Btn连续袋.Click += new System.EventHandler(this.bag3Btn_Click);
            // 
            // BtnCS制袋
            // 
            this.BtnCS制袋.FlatAppearance.BorderSize = 0;
            this.BtnCS制袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCS制袋.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnCS制袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCS制袋.Location = new System.Drawing.Point(4, 41);
            this.BtnCS制袋.Name = "BtnCS制袋";
            this.BtnCS制袋.Size = new System.Drawing.Size(173, 38);
            this.BtnCS制袋.TabIndex = 11;
            this.BtnCS制袋.Text = "    2.CS制袋";
            this.BtnCS制袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCS制袋.UseVisualStyleBackColor = true;
            this.BtnCS制袋.Click += new System.EventHandler(this.CSbagBtn_Click);
            // 
            // BtnPE制袋
            // 
            this.BtnPE制袋.FlatAppearance.BorderSize = 0;
            this.BtnPE制袋.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPE制袋.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnPE制袋.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPE制袋.Location = new System.Drawing.Point(4, 3);
            this.BtnPE制袋.Name = "BtnPE制袋";
            this.BtnPE制袋.Size = new System.Drawing.Size(173, 38);
            this.BtnPE制袋.TabIndex = 10;
            this.BtnPE制袋.Text = "    1.PE制袋";
            this.BtnPE制袋.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnPE制袋.UseVisualStyleBackColor = true;
            this.BtnPE制袋.Click += new System.EventHandler(this.LDPEbagBtn_Click);
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
            this.Panel其他按钮.ResumeLayout(false);
            this.Panel制袋.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn灭菌;
        private System.Windows.Forms.Button Btn清洁分切;
        private System.Windows.Forms.Button Btn制袋;
        private System.Windows.Forms.Panel ProducePanelLeft;
        private System.Windows.Forms.Button Btn吹膜;
        private System.Windows.Forms.Button Btn库存管理;
        private System.Windows.Forms.Button Btn订单管理;
        private System.Windows.Forms.Button Btn生产指令;
        private System.Windows.Forms.Panel Panel制袋;
        private System.Windows.Forms.Button Btn防护罩;
        private System.Windows.Forms.Button BtnBPV制袋;
        private System.Windows.Forms.Button BtnPTV制袋;
        private System.Windows.Forms.Button Btn连续袋;
        private System.Windows.Forms.Button BtnCS制袋;
        private System.Windows.Forms.Button BtnPE制袋;
        private System.Windows.Forms.Panel Panel其他按钮;
        public System.Windows.Forms.Panel ProducePanelRight;
    }
}