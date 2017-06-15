namespace mySystem
{
    partial class QueryMainForm
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
            this.StockBtn = new System.Windows.Forms.Button();
            this.PlanBtn = new System.Windows.Forms.Button();
            this.OrderBtn = new System.Windows.Forms.Button();
            this.QueryPanelRight = new System.Windows.Forms.Panel();
            this.KillBtn = new System.Windows.Forms.Button();
            this.QueryPanelLeft = new System.Windows.Forms.Panel();
            this.BagBtn = new System.Windows.Forms.Button();
            this.CleanBtn = new System.Windows.Forms.Button();
            this.ExtructionBtn = new System.Windows.Forms.Button();
            this.QueryPanelLeft.SuspendLayout();
            this.SuspendLayout();
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
            this.StockBtn.Text = "    库存查询";
            this.StockBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StockBtn.UseVisualStyleBackColor = true;
            this.StockBtn.Click += new System.EventHandler(this.StockBtn_Click);
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
            this.OrderBtn.Text = "    订单查询";
            this.OrderBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderBtn.UseVisualStyleBackColor = true;
            this.OrderBtn.Click += new System.EventHandler(this.OrderBtn_Click);
            // 
            // QueryPanelRight
            // 
            this.QueryPanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QueryPanelRight.Location = new System.Drawing.Point(185, 2);
            this.QueryPanelRight.Name = "QueryPanelRight";
            this.QueryPanelRight.Size = new System.Drawing.Size(1170, 615);
            this.QueryPanelRight.TabIndex = 7;
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
            // QueryPanelLeft
            // 
            this.QueryPanelLeft.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.QueryPanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QueryPanelLeft.Controls.Add(this.StockBtn);
            this.QueryPanelLeft.Controls.Add(this.OrderBtn);
            this.QueryPanelLeft.Controls.Add(this.PlanBtn);
            this.QueryPanelLeft.Controls.Add(this.KillBtn);
            this.QueryPanelLeft.Controls.Add(this.BagBtn);
            this.QueryPanelLeft.Controls.Add(this.CleanBtn);
            this.QueryPanelLeft.Controls.Add(this.ExtructionBtn);
            this.QueryPanelLeft.Location = new System.Drawing.Point(4, 2);
            this.QueryPanelLeft.Name = "QueryPanelLeft";
            this.QueryPanelLeft.Size = new System.Drawing.Size(180, 615);
            this.QueryPanelLeft.TabIndex = 6;
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
            // QueryMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 620);
            this.Controls.Add(this.QueryPanelRight);
            this.Controls.Add(this.QueryPanelLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QueryMainForm";
            this.Text = "QueryMainForm";
            this.QueryPanelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StockBtn;
        private System.Windows.Forms.Button PlanBtn;
        private System.Windows.Forms.Button OrderBtn;
        private System.Windows.Forms.Panel QueryPanelRight;
        private System.Windows.Forms.Button KillBtn;
        private System.Windows.Forms.Panel QueryPanelLeft;
        private System.Windows.Forms.Button BagBtn;
        private System.Windows.Forms.Button CleanBtn;
        private System.Windows.Forms.Button ExtructionBtn;
    }
}