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
            this.Page库存管理 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage退货管理 = new System.Windows.Forms.TabPage();
            this.btn退货记录 = new System.Windows.Forms.Button();
            this.btn退货申请 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage退货管理.SuspendLayout();
            this.SuspendLayout();
            // 
            // Page库存管理
            // 
            this.Page库存管理.Location = new System.Drawing.Point(4, 29);
            this.Page库存管理.Name = "Page库存管理";
            this.Page库存管理.Padding = new System.Windows.Forms.Padding(3);
            this.Page库存管理.Size = new System.Drawing.Size(1162, 577);
            this.Page库存管理.TabIndex = 0;
            this.Page库存管理.Text = "库存管理";
            this.Page库存管理.UseVisualStyleBackColor = true;
            this.Page库存管理.Paint += new System.Windows.Forms.PaintEventHandler(this.Page库存管理_Paint);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.Page库存管理);
            this.tabControl1.Controls.Add(this.tabPage退货管理);
            this.tabControl1.Location = new System.Drawing.Point(0, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1170, 610);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage退货管理
            // 
            this.tabPage退货管理.Controls.Add(this.btn退货记录);
            this.tabPage退货管理.Controls.Add(this.btn退货申请);
            this.tabPage退货管理.Location = new System.Drawing.Point(4, 29);
            this.tabPage退货管理.Name = "tabPage退货管理";
            this.tabPage退货管理.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage退货管理.Size = new System.Drawing.Size(1162, 577);
            this.tabPage退货管理.TabIndex = 1;
            this.tabPage退货管理.Text = "退货管理";
            this.tabPage退货管理.UseVisualStyleBackColor = true;
            // 
            // btn退货记录
            // 
            this.btn退货记录.Location = new System.Drawing.Point(259, 67);
            this.btn退货记录.Name = "btn退货记录";
            this.btn退货记录.Size = new System.Drawing.Size(88, 23);
            this.btn退货记录.TabIndex = 1;
            this.btn退货记录.Text = "退货记录";
            this.btn退货记录.UseVisualStyleBackColor = true;
            // 
            // btn退货申请
            // 
            this.btn退货申请.Location = new System.Drawing.Point(33, 67);
            this.btn退货申请.Name = "btn退货申请";
            this.btn退货申请.Size = new System.Drawing.Size(88, 23);
            this.btn退货申请.TabIndex = 0;
            this.btn退货申请.Text = "退货申请";
            this.btn退货申请.UseVisualStyleBackColor = true;
            this.btn退货申请.Click += new System.EventHandler(this.btn退货申请_Click);
            // 
            // 库存管理主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "库存管理主界面";
            this.Text = "库存管理主界面";
            this.tabControl1.ResumeLayout(false);
            this.tabPage退货管理.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage Page库存管理;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage退货管理;
        private System.Windows.Forms.Button btn退货记录;
        private System.Windows.Forms.Button btn退货申请;

    }
}