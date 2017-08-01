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
            this.btn取样记录 = new System.Windows.Forms.Button();
            this.btn取样证 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn检验台账 = new System.Windows.Forms.Button();
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
            // btn取样记录
            // 
            this.btn取样记录.Location = new System.Drawing.Point(155, 69);
            this.btn取样记录.Name = "btn取样记录";
            this.btn取样记录.Size = new System.Drawing.Size(75, 23);
            this.btn取样记录.TabIndex = 5;
            this.btn取样记录.Text = "取样记录";
            this.btn取样记录.UseVisualStyleBackColor = true;
            this.btn取样记录.Click += new System.EventHandler(this.btn取样记录_Click);
            // 
            // btn取样证
            // 
            this.btn取样证.Location = new System.Drawing.Point(155, 109);
            this.btn取样证.Name = "btn取样证";
            this.btn取样证.Size = new System.Drawing.Size(75, 23);
            this.btn取样证.TabIndex = 6;
            this.btn取样证.Text = "取样证";
            this.btn取样证.UseVisualStyleBackColor = true;
            this.btn取样证.Click += new System.EventHandler(this.btn取样证_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(215, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "物料取样、请验等级及报告发放记录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn检验台账
            // 
            this.btn检验台账.Location = new System.Drawing.Point(273, 69);
            this.btn检验台账.Name = "btn检验台账";
            this.btn检验台账.Size = new System.Drawing.Size(75, 23);
            this.btn检验台账.TabIndex = 8;
            this.btn检验台账.Text = "检验台账";
            this.btn检验台账.UseVisualStyleBackColor = true;
            this.btn检验台账.Click += new System.EventHandler(this.btn检验台账_Click);
            // 
            // 库存管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 407);
            this.Controls.Add(this.btn检验台账);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn取样证);
            this.Controls.Add(this.btn取样记录);
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
        private System.Windows.Forms.Button btn取样记录;
        private System.Windows.Forms.Button btn取样证;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn检验台账;
    }
}