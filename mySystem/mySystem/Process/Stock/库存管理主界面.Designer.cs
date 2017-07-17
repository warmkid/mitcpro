namespace mySystem.Process.Stock
{
    partial class 库存管理主界面
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Page库存管理 = new System.Windows.Forms.TabPage();
            this.Page商品设置 = new System.Windows.Forms.TabPage();
            this.Page采购单管理 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.Page库存管理);
            this.tabControl1.Controls.Add(this.Page商品设置);
            this.tabControl1.Controls.Add(this.Page采购单管理);
            this.tabControl1.Location = new System.Drawing.Point(0, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1170, 610);
            this.tabControl1.TabIndex = 0;
            // 
            // Page库存管理
            // 
            this.Page库存管理.Location = new System.Drawing.Point(4, 29);
            this.Page库存管理.Name = "Page库存管理";
            this.Page库存管理.Padding = new System.Windows.Forms.Padding(3);
            this.Page库存管理.Size = new System.Drawing.Size(1162, 582);
            this.Page库存管理.TabIndex = 0;
            this.Page库存管理.Text = "库存管理";
            this.Page库存管理.UseVisualStyleBackColor = true;
            this.Page库存管理.Paint += new System.Windows.Forms.PaintEventHandler(this.Page库存管理_Paint);
            // 
            // Page商品设置
            // 
            this.Page商品设置.Location = new System.Drawing.Point(4, 29);
            this.Page商品设置.Name = "Page商品设置";
            this.Page商品设置.Padding = new System.Windows.Forms.Padding(3);
            this.Page商品设置.Size = new System.Drawing.Size(1162, 582);
            this.Page商品设置.TabIndex = 1;
            this.Page商品设置.Text = "商品设置";
            this.Page商品设置.UseVisualStyleBackColor = true;
            this.Page商品设置.Paint += new System.Windows.Forms.PaintEventHandler(this.Page商品设置_Paint);
            // 
            // Page采购单管理
            // 
            this.Page采购单管理.Location = new System.Drawing.Point(4, 29);
            this.Page采购单管理.Name = "Page采购单管理";
            this.Page采购单管理.Padding = new System.Windows.Forms.Padding(3);
            this.Page采购单管理.Size = new System.Drawing.Size(1162, 577);
            this.Page采购单管理.TabIndex = 2;
            this.Page采购单管理.Text = "采购单管理";
            this.Page采购单管理.UseVisualStyleBackColor = true;
            this.Page采购单管理.Paint += new System.Windows.Forms.PaintEventHandler(this.Page采购单管理_Paint);
            // 
            // 库存管理主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "库存管理主界面";
            this.Text = "库存管理主界面";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Page库存管理;
        private System.Windows.Forms.TabPage Page商品设置;
        private System.Windows.Forms.TabPage Page采购单管理;
    }
}