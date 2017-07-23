namespace 订单和库存管理
{
    partial class 库存管理
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
            this.btn入库 = new System.Windows.Forms.Button();
            this.btn出库 = new System.Windows.Forms.Button();
            this.btn退货 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn原料入库 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn入库
            // 
            this.btn入库.Location = new System.Drawing.Point(12, 27);
            this.btn入库.Name = "btn入库";
            this.btn入库.Size = new System.Drawing.Size(75, 23);
            this.btn入库.TabIndex = 0;
            this.btn入库.Text = "入库";
            this.btn入库.UseVisualStyleBackColor = true;
            this.btn入库.Click += new System.EventHandler(this.btn入库_Click);
            // 
            // btn出库
            // 
            this.btn出库.Location = new System.Drawing.Point(12, 69);
            this.btn出库.Name = "btn出库";
            this.btn出库.Size = new System.Drawing.Size(75, 23);
            this.btn出库.TabIndex = 1;
            this.btn出库.Text = "出库";
            this.btn出库.UseVisualStyleBackColor = true;
            this.btn出库.Click += new System.EventHandler(this.btn出库_Click);
            // 
            // btn退货
            // 
            this.btn退货.Location = new System.Drawing.Point(12, 109);
            this.btn退货.Name = "btn退货";
            this.btn退货.Size = new System.Drawing.Size(75, 23);
            this.btn退货.TabIndex = 2;
            this.btn退货.Text = "退货";
            this.btn退货.UseVisualStyleBackColor = true;
            this.btn退货.Click += new System.EventHandler(this.btn退货_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 147);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(476, 248);
            this.dataGridView1.TabIndex = 3;
            // 
            // btn原料入库
            // 
            this.btn原料入库.Location = new System.Drawing.Point(155, 27);
            this.btn原料入库.Name = "btn原料入库";
            this.btn原料入库.Size = new System.Drawing.Size(75, 23);
            this.btn原料入库.TabIndex = 4;
            this.btn原料入库.Text = "原料入库";
            this.btn原料入库.UseVisualStyleBackColor = true;
            this.btn原料入库.Click += new System.EventHandler(this.btn原料入库_Click);
            // 
            // 库存管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 407);
            this.Controls.Add(this.btn原料入库);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn退货);
            this.Controls.Add(this.btn出库);
            this.Controls.Add(this.btn入库);
            this.Name = "库存管理";
            this.Text = "库存管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn入库;
        private System.Windows.Forms.Button btn出库;
        private System.Windows.Forms.Button btn退货;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn原料入库;
    }
}