namespace mySystem
{
    partial class BuyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyForm));
            this.打印出库单Button = new System.Windows.Forms.Button();
            this.新增商品种类Button = new System.Windows.Forms.Button();
            this.保存出库单Button = new System.Windows.Forms.Button();
            this.新增出库单Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 打印出库单Button
            // 
            this.打印出库单Button.Image = ((System.Drawing.Image)(resources.GetObject("打印出库单Button.Image")));
            this.打印出库单Button.Location = new System.Drawing.Point(397, 12);
            this.打印出库单Button.Name = "打印出库单Button";
            this.打印出库单Button.Size = new System.Drawing.Size(76, 23);
            this.打印出库单Button.TabIndex = 95;
            this.打印出库单Button.Text = "打印";
            this.打印出库单Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.打印出库单Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.打印出库单Button.UseVisualStyleBackColor = true;
            // 
            // 新增商品种类Button
            // 
            this.新增商品种类Button.Image = ((System.Drawing.Image)(resources.GetObject("新增商品种类Button.Image")));
            this.新增商品种类Button.Location = new System.Drawing.Point(122, 12);
            this.新增商品种类Button.Name = "新增商品种类Button";
            this.新增商品种类Button.Size = new System.Drawing.Size(67, 23);
            this.新增商品种类Button.TabIndex = 94;
            this.新增商品种类Button.Text = "修改";
            this.新增商品种类Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.新增商品种类Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.新增商品种类Button.UseVisualStyleBackColor = true;
            // 
            // 保存出库单Button
            // 
            this.保存出库单Button.Image = ((System.Drawing.Image)(resources.GetObject("保存出库单Button.Image")));
            this.保存出库单Button.Location = new System.Drawing.Point(195, 12);
            this.保存出库单Button.Name = "保存出库单Button";
            this.保存出库单Button.Size = new System.Drawing.Size(71, 23);
            this.保存出库单Button.TabIndex = 93;
            this.保存出库单Button.Text = "删除";
            this.保存出库单Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.保存出库单Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.保存出库单Button.UseVisualStyleBackColor = true;
            // 
            // 新增出库单Button
            // 
            this.新增出库单Button.Image = ((System.Drawing.Image)(resources.GetObject("新增出库单Button.Image")));
            this.新增出库单Button.Location = new System.Drawing.Point(16, 12);
            this.新增出库单Button.Name = "新增出库单Button";
            this.新增出库单Button.Size = new System.Drawing.Size(100, 23);
            this.新增出库单Button.TabIndex = 92;
            this.新增出库单Button.Text = "新增采购单";
            this.新增出库单Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.新增出库单Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.新增出库单Button.UseVisualStyleBackColor = true;
            // 
            // BuyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 464);
            this.Controls.Add(this.打印出库单Button);
            this.Controls.Add(this.新增商品种类Button);
            this.Controls.Add(this.保存出库单Button);
            this.Controls.Add(this.新增出库单Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BuyForm";
            this.Text = "BuyForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button 打印出库单Button;
        private System.Windows.Forms.Button 新增商品种类Button;
        private System.Windows.Forms.Button 保存出库单Button;
        private System.Windows.Forms.Button 新增出库单Button;
    }
}