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
            this.btn出库退库单 = new System.Windows.Forms.Button();
            this.btn读取 = new System.Windows.Forms.Button();
            this.btn文件上传 = new System.Windows.Forms.Button();
            this.btn检验台账 = new System.Windows.Forms.Button();
            this.btn原料入库 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn退货 = new System.Windows.Forms.Button();
            this.btn出库 = new System.Windows.Forms.Button();
            this.btn入库 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn出库退库单
            // 
            this.btn出库退库单.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn出库退库单.Location = new System.Drawing.Point(361, 289);
            this.btn出库退库单.Name = "btn出库退库单";
            this.btn出库退库单.Size = new System.Drawing.Size(109, 32);
            this.btn出库退库单.TabIndex = 11;
            this.btn出库退库单.Text = "出库/退库单";
            this.btn出库退库单.UseVisualStyleBackColor = true;
            this.btn出库退库单.Click += new System.EventHandler(this.btn出库退库单_Click);
            // 
            // btn读取
            // 
            this.btn读取.Location = new System.Drawing.Point(382, 23);
            this.btn读取.Name = "btn读取";
            this.btn读取.Size = new System.Drawing.Size(75, 23);
            this.btn读取.TabIndex = 10;
            this.btn读取.Text = "读取1";
            this.btn读取.UseVisualStyleBackColor = true;
            this.btn读取.Visible = false;
            this.btn读取.Click += new System.EventHandler(this.btn读取_Click);
            // 
            // btn文件上传
            // 
            this.btn文件上传.Location = new System.Drawing.Point(791, 23);
            this.btn文件上传.Name = "btn文件上传";
            this.btn文件上传.Size = new System.Drawing.Size(75, 23);
            this.btn文件上传.TabIndex = 9;
            this.btn文件上传.Text = "文件上传";
            this.btn文件上传.UseVisualStyleBackColor = true;
            this.btn文件上传.Visible = false;
            this.btn文件上传.Click += new System.EventHandler(this.btn文件上传_Click);
            // 
            // btn检验台账
            // 
            this.btn检验台账.Location = new System.Drawing.Point(653, 23);
            this.btn检验台账.Name = "btn检验台账";
            this.btn检验台账.Size = new System.Drawing.Size(75, 23);
            this.btn检验台账.TabIndex = 8;
            this.btn检验台账.Text = "检验台账";
            this.btn检验台账.UseVisualStyleBackColor = true;
            this.btn检验台账.Visible = false;
            this.btn检验台账.Click += new System.EventHandler(this.btn检验台账_Click);
            // 
            // btn原料入库
            // 
            this.btn原料入库.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn原料入库.Location = new System.Drawing.Point(361, 186);
            this.btn原料入库.Name = "btn原料入库";
            this.btn原料入库.Size = new System.Drawing.Size(109, 32);
            this.btn原料入库.TabIndex = 4;
            this.btn原料入库.Text = "原料入库";
            this.btn原料入库.UseVisualStyleBackColor = true;
            this.btn原料入库.Click += new System.EventHandler(this.btn原料入库_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(45, 323);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(101, 43);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.Visible = false;
            // 
            // btn退货
            // 
            this.btn退货.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn退货.Location = new System.Drawing.Point(361, 83);
            this.btn退货.Name = "btn退货";
            this.btn退货.Size = new System.Drawing.Size(109, 32);
            this.btn退货.TabIndex = 2;
            this.btn退货.Text = "退货";
            this.btn退货.UseVisualStyleBackColor = true;
            this.btn退货.Click += new System.EventHandler(this.btn退货_Click);
            // 
            // btn出库
            // 
            this.btn出库.Location = new System.Drawing.Point(123, 23);
            this.btn出库.Name = "btn出库";
            this.btn出库.Size = new System.Drawing.Size(75, 23);
            this.btn出库.TabIndex = 1;
            this.btn出库.Text = "出库";
            this.btn出库.UseVisualStyleBackColor = true;
            this.btn出库.Visible = false;
            this.btn出库.Click += new System.EventHandler(this.btn出库_Click);
            // 
            // btn入库
            // 
            this.btn入库.Location = new System.Drawing.Point(12, 23);
            this.btn入库.Name = "btn入库";
            this.btn入库.Size = new System.Drawing.Size(75, 23);
            this.btn入库.TabIndex = 0;
            this.btn入库.Text = "入库";
            this.btn入库.UseVisualStyleBackColor = true;
            this.btn入库.Visible = false;
            this.btn入库.Click += new System.EventHandler(this.btn入库_Click);
            // 
            // 库存管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 407);
            this.Controls.Add(this.btn出库退库单);
            this.Controls.Add(this.btn读取);
            this.Controls.Add(this.btn文件上传);
            this.Controls.Add(this.btn检验台账);
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
        private System.Windows.Forms.Button btn原料入库;
        private System.Windows.Forms.Button btn检验台账;
        private System.Windows.Forms.Button btn文件上传;
        private System.Windows.Forms.Button btn读取;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn出库退库单;
    }
}