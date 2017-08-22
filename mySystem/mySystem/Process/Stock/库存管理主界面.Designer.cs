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
            this.label3 = new System.Windows.Forms.Label();
            this.tb退货编号 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp结束时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp开始时间 = new System.Windows.Forms.DateTimePicker();
            this.btn退货台账查询 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb客户名称 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb产品代码 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage退货管理.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.tabPage退货管理.Controls.Add(this.tb产品代码);
            this.tabPage退货管理.Controls.Add(this.label4);
            this.tabPage退货管理.Controls.Add(this.tb客户名称);
            this.tabPage退货管理.Controls.Add(this.label2);
            this.tabPage退货管理.Controls.Add(this.dataGridView1);
            this.tabPage退货管理.Controls.Add(this.btn退货台账查询);
            this.tabPage退货管理.Controls.Add(this.label3);
            this.tabPage退货管理.Controls.Add(this.tb退货编号);
            this.tabPage退货管理.Controls.Add(this.label1);
            this.tabPage退货管理.Controls.Add(this.dtp结束时间);
            this.tabPage退货管理.Controls.Add(this.dtp开始时间);
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
            this.btn退货记录.Location = new System.Drawing.Point(157, 91);
            this.btn退货记录.Name = "btn退货记录";
            this.btn退货记录.Size = new System.Drawing.Size(88, 23);
            this.btn退货记录.TabIndex = 1;
            this.btn退货记录.Text = "退货记录";
            this.btn退货记录.UseVisualStyleBackColor = true;
            // 
            // btn退货申请
            // 
            this.btn退货申请.Location = new System.Drawing.Point(21, 91);
            this.btn退货申请.Name = "btn退货申请";
            this.btn退货申请.Size = new System.Drawing.Size(88, 23);
            this.btn退货申请.TabIndex = 0;
            this.btn退货申请.Text = "退货申请";
            this.btn退货申请.UseVisualStyleBackColor = true;
            this.btn退货申请.Click += new System.EventHandler(this.btn退货申请_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "---";
            // 
            // tb退货编号
            // 
            this.tb退货编号.Location = new System.Drawing.Point(542, 37);
            this.tb退货编号.Name = "tb退货编号";
            this.tb退货编号.Size = new System.Drawing.Size(107, 26);
            this.tb退货编号.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(467, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "退货编号";
            // 
            // dtp结束时间
            // 
            this.dtp结束时间.Location = new System.Drawing.Point(253, 37);
            this.dtp结束时间.Name = "dtp结束时间";
            this.dtp结束时间.Size = new System.Drawing.Size(156, 26);
            this.dtp结束时间.TabIndex = 10;
            // 
            // dtp开始时间
            // 
            this.dtp开始时间.Location = new System.Drawing.Point(21, 38);
            this.dtp开始时间.Name = "dtp开始时间";
            this.dtp开始时间.Size = new System.Drawing.Size(156, 26);
            this.dtp开始时间.TabIndex = 9;
            // 
            // btn退货台账查询
            // 
            this.btn退货台账查询.Location = new System.Drawing.Point(1039, 91);
            this.btn退货台账查询.Name = "btn退货台账查询";
            this.btn退货台账查询.Size = new System.Drawing.Size(88, 23);
            this.btn退货台账查询.TabIndex = 16;
            this.btn退货台账查询.Text = "查询";
            this.btn退货台账查询.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 138);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1116, 416);
            this.dataGridView1.TabIndex = 17;
            // 
            // tb客户名称
            // 
            this.tb客户名称.Location = new System.Drawing.Point(791, 34);
            this.tb客户名称.Name = "tb客户名称";
            this.tb客户名称.Size = new System.Drawing.Size(107, 26);
            this.tb客户名称.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(716, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "客户名称";
            // 
            // tb产品代码
            // 
            this.tb产品代码.Location = new System.Drawing.Point(1020, 31);
            this.tb产品代码.Name = "tb产品代码";
            this.tb产品代码.Size = new System.Drawing.Size(107, 26);
            this.tb产品代码.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(945, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "产品代码";
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
            this.tabPage退货管理.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage Page库存管理;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage退货管理;
        private System.Windows.Forms.Button btn退货记录;
        private System.Windows.Forms.Button btn退货申请;
        private System.Windows.Forms.Button btn退货台账查询;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb退货编号;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp结束时间;
        private System.Windows.Forms.DateTimePicker dtp开始时间;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb客户名称;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb产品代码;
        private System.Windows.Forms.Label label4;

    }
}