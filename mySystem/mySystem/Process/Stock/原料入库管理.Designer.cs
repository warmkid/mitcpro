namespace mySystem.Process.Stock
{
    partial class 原料入库管理
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
            this.tp物资验收记录 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn增加物资验收记录 = new System.Windows.Forms.Button();
            this.tp物资请验单 = new System.Windows.Forms.TabPage();
            this.btn读取物资请验单 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tp检查记录 = new System.Windows.Forms.TabPage();
            this.btn读取检查记录 = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tp不合格品处理记录 = new System.Windows.Forms.TabPage();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.btn读取不合格品记录 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tp物资验收记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tp物资请验单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tp检查记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tp不合格品处理记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tp物资验收记录);
            this.tabControl1.Controls.Add(this.tp物资请验单);
            this.tabControl1.Controls.Add(this.tp检查记录);
            this.tabControl1.Controls.Add(this.tp不合格品处理记录);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(19, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 510);
            this.tabControl1.TabIndex = 0;
            // 
            // tp物资验收记录
            // 
            this.tp物资验收记录.Controls.Add(this.dataGridView1);
            this.tp物资验收记录.Controls.Add(this.btn增加物资验收记录);
            this.tp物资验收记录.Location = new System.Drawing.Point(4, 29);
            this.tp物资验收记录.Name = "tp物资验收记录";
            this.tp物资验收记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp物资验收记录.Size = new System.Drawing.Size(689, 477);
            this.tp物资验收记录.TabIndex = 0;
            this.tp物资验收记录.Text = "物资验收记录";
            this.tp物资验收记录.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(27, 73);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(637, 389);
            this.dataGridView1.TabIndex = 1;
            // 
            // btn增加物资验收记录
            // 
            this.btn增加物资验收记录.Location = new System.Drawing.Point(27, 24);
            this.btn增加物资验收记录.Name = "btn增加物资验收记录";
            this.btn增加物资验收记录.Size = new System.Drawing.Size(150, 28);
            this.btn增加物资验收记录.TabIndex = 0;
            this.btn增加物资验收记录.Text = "增加物资验收记录";
            this.btn增加物资验收记录.UseVisualStyleBackColor = true;
            this.btn增加物资验收记录.Click += new System.EventHandler(this.btn增加物资验收记录_Click);
            // 
            // tp物资请验单
            // 
            this.tp物资请验单.Controls.Add(this.btn读取物资请验单);
            this.tp物资请验单.Controls.Add(this.dataGridView2);
            this.tp物资请验单.Location = new System.Drawing.Point(4, 29);
            this.tp物资请验单.Name = "tp物资请验单";
            this.tp物资请验单.Padding = new System.Windows.Forms.Padding(3);
            this.tp物资请验单.Size = new System.Drawing.Size(689, 477);
            this.tp物资请验单.TabIndex = 1;
            this.tp物资请验单.Text = "物资请验单";
            this.tp物资请验单.UseVisualStyleBackColor = true;
            // 
            // btn读取物资请验单
            // 
            this.btn读取物资请验单.Location = new System.Drawing.Point(597, 26);
            this.btn读取物资请验单.Name = "btn读取物资请验单";
            this.btn读取物资请验单.Size = new System.Drawing.Size(75, 23);
            this.btn读取物资请验单.TabIndex = 1;
            this.btn读取物资请验单.Text = "读取";
            this.btn读取物资请验单.UseVisualStyleBackColor = true;
            this.btn读取物资请验单.Click += new System.EventHandler(this.btn读取_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(19, 64);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(653, 396);
            this.dataGridView2.TabIndex = 0;
            // 
            // tp检查记录
            // 
            this.tp检查记录.Controls.Add(this.btn读取检查记录);
            this.tp检查记录.Controls.Add(this.dataGridView3);
            this.tp检查记录.Location = new System.Drawing.Point(4, 29);
            this.tp检查记录.Name = "tp检查记录";
            this.tp检查记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp检查记录.Size = new System.Drawing.Size(689, 477);
            this.tp检查记录.TabIndex = 2;
            this.tp检查记录.Text = "检查记录";
            this.tp检查记录.UseVisualStyleBackColor = true;
            // 
            // btn读取检查记录
            // 
            this.btn读取检查记录.Location = new System.Drawing.Point(596, 21);
            this.btn读取检查记录.Name = "btn读取检查记录";
            this.btn读取检查记录.Size = new System.Drawing.Size(75, 23);
            this.btn读取检查记录.TabIndex = 3;
            this.btn读取检查记录.Text = "读取";
            this.btn读取检查记录.UseVisualStyleBackColor = true;
            this.btn读取检查记录.Click += new System.EventHandler(this.btn读取检查记录_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(18, 59);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(653, 396);
            this.dataGridView3.TabIndex = 2;
            // 
            // tp不合格品处理记录
            // 
            this.tp不合格品处理记录.Controls.Add(this.btn读取不合格品记录);
            this.tp不合格品处理记录.Controls.Add(this.dataGridView4);
            this.tp不合格品处理记录.Location = new System.Drawing.Point(4, 29);
            this.tp不合格品处理记录.Name = "tp不合格品处理记录";
            this.tp不合格品处理记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp不合格品处理记录.Size = new System.Drawing.Size(689, 477);
            this.tp不合格品处理记录.TabIndex = 3;
            this.tp不合格品处理记录.Text = "不合格品处理记录";
            this.tp不合格品处理记录.UseVisualStyleBackColor = true;
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(17, 93);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowTemplate.Height = 23;
            this.dataGridView4.Size = new System.Drawing.Size(650, 365);
            this.dataGridView4.TabIndex = 0;
            // 
            // btn读取不合格品记录
            // 
            this.btn读取不合格品记录.Location = new System.Drawing.Point(592, 40);
            this.btn读取不合格品记录.Name = "btn读取不合格品记录";
            this.btn读取不合格品记录.Size = new System.Drawing.Size(75, 23);
            this.btn读取不合格品记录.TabIndex = 1;
            this.btn读取不合格品记录.Text = "读取";
            this.btn读取不合格品记录.UseVisualStyleBackColor = true;
            this.btn读取不合格品记录.Click += new System.EventHandler(this.btn读取不合格品记录_Click);
            // 
            // 原料入库管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 552);
            this.Controls.Add(this.tabControl1);
            this.Name = "原料入库管理";
            this.Text = "原料入库管理";
            this.tabControl1.ResumeLayout(false);
            this.tp物资验收记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tp物资请验单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tp检查记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tp不合格品处理记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp物资验收记录;
        private System.Windows.Forms.TabPage tp物资请验单;
        private System.Windows.Forms.TabPage tp检查记录;
        private System.Windows.Forms.TabPage tp不合格品处理记录;
        private System.Windows.Forms.Button btn增加物资验收记录;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn读取物资请验单;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btn读取检查记录;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button btn读取不合格品记录;
        private System.Windows.Forms.DataGridView dataGridView4;
    }
}