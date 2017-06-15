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
            this.KillBtn = new System.Windows.Forms.Button();
            this.CleanBtn = new System.Windows.Forms.Button();
            this.BagBtn = new System.Windows.Forms.Button();
            this.ProducePanelLeft = new System.Windows.Forms.Panel();
            this.StockBtn = new System.Windows.Forms.Button();
            this.OrderBtn = new System.Windows.Forms.Button();
            this.PlanBtn = new System.Windows.Forms.Button();
            this.ExtructionBtn = new System.Windows.Forms.Button();
            this.ProducePanelRight = new System.Windows.Forms.Panel();
            this.ProducePanelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // KillBtn
            // 
            this.KillBtn.FlatAppearance.BorderSize = 0;
            this.KillBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KillBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.KillBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.KillBtn.Location = new System.Drawing.Point(3, 133);
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
            this.ProducePanelLeft.Controls.Add(this.StockBtn);
            this.ProducePanelLeft.Controls.Add(this.OrderBtn);
            this.ProducePanelLeft.Controls.Add(this.PlanBtn);
            this.ProducePanelLeft.Controls.Add(this.KillBtn);
            this.ProducePanelLeft.Controls.Add(this.BagBtn);
            this.ProducePanelLeft.Controls.Add(this.CleanBtn);
            this.ProducePanelLeft.Controls.Add(this.ExtructionBtn);
            this.ProducePanelLeft.Location = new System.Drawing.Point(4, 2);
            this.ProducePanelLeft.Name = "ProducePanelLeft";
            this.ProducePanelLeft.Size = new System.Drawing.Size(180, 615);
            this.ProducePanelLeft.TabIndex = 4;
            this.ProducePanelLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.ProducePanelLeft_Paint);
            // 
            // StockBtn
            // 
            this.StockBtn.FlatAppearance.BorderSize = 0;
            this.StockBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StockBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StockBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StockBtn.Location = new System.Drawing.Point(3, 262);
            this.StockBtn.Name = "StockBtn";
            this.StockBtn.Size = new System.Drawing.Size(172, 43);
            this.StockBtn.TabIndex = 8;
            this.StockBtn.Text = "    库存管理";
            this.StockBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StockBtn.UseVisualStyleBackColor = true;
            this.StockBtn.Click += new System.EventHandler(this.StockBtn_Click);
            // 
            // OrderBtn
            // 
            this.OrderBtn.FlatAppearance.BorderSize = 0;
            this.OrderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OrderBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OrderBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderBtn.Location = new System.Drawing.Point(3, 219);
            this.OrderBtn.Name = "OrderBtn";
            this.OrderBtn.Size = new System.Drawing.Size(172, 43);
            this.OrderBtn.TabIndex = 7;
            this.OrderBtn.Text = "    订单管理";
            this.OrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderBtn.UseVisualStyleBackColor = true;
            this.OrderBtn.Click += new System.EventHandler(this.OrderBtn_Click);
            // 
            // PlanBtn
            // 
            this.PlanBtn.FlatAppearance.BorderSize = 0;
            this.PlanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlanBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlanBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PlanBtn.Location = new System.Drawing.Point(3, 176);
            this.PlanBtn.Name = "PlanBtn";
            this.PlanBtn.Size = new System.Drawing.Size(172, 43);
            this.PlanBtn.TabIndex = 6;
            this.PlanBtn.Text = "    生产计划";
            this.PlanBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PlanBtn.UseVisualStyleBackColor = true;
            this.PlanBtn.Click += new System.EventHandler(this.PlanBtn_Click);
            // 
            // ExtructionBtn
            // 
            this.ExtructionBtn.FlatAppearance.BorderSize = 0;
            this.ExtructionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExtructionBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.ProducePanelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.ProducePanelRight_Paint);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button KillBtn;
        private System.Windows.Forms.Button CleanBtn;
        private System.Windows.Forms.Button BagBtn;
        private System.Windows.Forms.Panel ProducePanelLeft;
        private System.Windows.Forms.Button ExtructionBtn;
        private System.Windows.Forms.Panel ProducePanelRight;
        private System.Windows.Forms.Button StockBtn;
        private System.Windows.Forms.Button OrderBtn;
        private System.Windows.Forms.Button PlanBtn;
    }
}