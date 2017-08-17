namespace 订单和库存管理
{
    partial class 订单管理
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
            this.tabPage销售订单 = new System.Windows.Forms.TabPage();
            this.tabPage采购需求单 = new System.Windows.Forms.TabPage();
            this.tabPage采购批准单 = new System.Windows.Forms.TabPage();
            this.tabPage采购订单 = new System.Windows.Forms.TabPage();
            this.dataGridView销售订单 = new System.Windows.Forms.DataGridView();
            this.dtp销售订单开始时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp销售订单结束时间 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb销售订单审核状态 = new System.Windows.Forms.ComboBox();
            this.btn查询销售订单 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn添加销售订单 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage销售订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView销售订单)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage销售订单);
            this.tabControl1.Controls.Add(this.tabPage采购需求单);
            this.tabControl1.Controls.Add(this.tabPage采购批准单);
            this.tabControl1.Controls.Add(this.tabPage采购订单);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(852, 398);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage销售订单
            // 
            this.tabPage销售订单.Controls.Add(this.btn添加销售订单);
            this.tabPage销售订单.Controls.Add(this.label3);
            this.tabPage销售订单.Controls.Add(this.btn查询销售订单);
            this.tabPage销售订单.Controls.Add(this.cmb销售订单审核状态);
            this.tabPage销售订单.Controls.Add(this.label2);
            this.tabPage销售订单.Controls.Add(this.textBox1);
            this.tabPage销售订单.Controls.Add(this.label1);
            this.tabPage销售订单.Controls.Add(this.dtp销售订单结束时间);
            this.tabPage销售订单.Controls.Add(this.dtp销售订单开始时间);
            this.tabPage销售订单.Controls.Add(this.dataGridView销售订单);
            this.tabPage销售订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage销售订单.Name = "tabPage销售订单";
            this.tabPage销售订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage销售订单.Size = new System.Drawing.Size(844, 365);
            this.tabPage销售订单.TabIndex = 1;
            this.tabPage销售订单.Text = "销售订单";
            this.tabPage销售订单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购需求单
            // 
            this.tabPage采购需求单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购需求单.Name = "tabPage采购需求单";
            this.tabPage采购需求单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购需求单.Size = new System.Drawing.Size(844, 365);
            this.tabPage采购需求单.TabIndex = 2;
            this.tabPage采购需求单.Text = "采购需求单";
            this.tabPage采购需求单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购批准单
            // 
            this.tabPage采购批准单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购批准单.Name = "tabPage采购批准单";
            this.tabPage采购批准单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购批准单.Size = new System.Drawing.Size(844, 365);
            this.tabPage采购批准单.TabIndex = 3;
            this.tabPage采购批准单.Text = "采购批准单";
            this.tabPage采购批准单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购订单
            // 
            this.tabPage采购订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购订单.Name = "tabPage采购订单";
            this.tabPage采购订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购订单.Size = new System.Drawing.Size(844, 365);
            this.tabPage采购订单.TabIndex = 4;
            this.tabPage采购订单.Text = "采购订单";
            this.tabPage采购订单.UseVisualStyleBackColor = true;
            // 
            // dataGridView销售订单
            // 
            this.dataGridView销售订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView销售订单.Location = new System.Drawing.Point(17, 111);
            this.dataGridView销售订单.Name = "dataGridView销售订单";
            this.dataGridView销售订单.RowTemplate.Height = 23;
            this.dataGridView销售订单.Size = new System.Drawing.Size(812, 248);
            this.dataGridView销售订单.TabIndex = 0;
            // 
            // dtp销售订单开始时间
            // 
            this.dtp销售订单开始时间.Location = new System.Drawing.Point(17, 21);
            this.dtp销售订单开始时间.Name = "dtp销售订单开始时间";
            this.dtp销售订单开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp销售订单开始时间.TabIndex = 1;
            // 
            // dtp销售订单结束时间
            // 
            this.dtp销售订单结束时间.Location = new System.Drawing.Point(294, 21);
            this.dtp销售订单结束时间.Name = "dtp销售订单结束时间";
            this.dtp销售订单结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp销售订单结束时间.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(572, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "订单号";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(647, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 26);
            this.textBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "审核状态";
            // 
            // cmb销售订单审核状态
            // 
            this.cmb销售订单审核状态.FormattingEnabled = true;
            this.cmb销售订单审核状态.Items.AddRange(new object[] {
            "__待审核"});
            this.cmb销售订单审核状态.Location = new System.Drawing.Point(96, 71);
            this.cmb销售订单审核状态.Name = "cmb销售订单审核状态";
            this.cmb销售订单审核状态.Size = new System.Drawing.Size(121, 24);
            this.cmb销售订单审核状态.TabIndex = 6;
            // 
            // btn查询销售订单
            // 
            this.btn查询销售订单.Location = new System.Drawing.Point(294, 71);
            this.btn查询销售订单.Name = "btn查询销售订单";
            this.btn查询销售订单.Size = new System.Drawing.Size(75, 23);
            this.btn查询销售订单.TabIndex = 7;
            this.btn查询销售订单.Text = "查询";
            this.btn查询销售订单.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "---";
            // 
            // btn添加销售订单
            // 
            this.btn添加销售订单.Location = new System.Drawing.Point(732, 72);
            this.btn添加销售订单.Name = "btn添加销售订单";
            this.btn添加销售订单.Size = new System.Drawing.Size(75, 23);
            this.btn添加销售订单.TabIndex = 9;
            this.btn添加销售订单.Text = "添加";
            this.btn添加销售订单.UseVisualStyleBackColor = true;
            // 
            // 订单管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 422);
            this.Controls.Add(this.tabControl1);
            this.Name = "订单管理";
            this.Text = "订单管理";
            this.tabControl1.ResumeLayout(false);
            this.tabPage销售订单.ResumeLayout(false);
            this.tabPage销售订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView销售订单)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage销售订单;
        private System.Windows.Forms.TabPage tabPage采购需求单;
        private System.Windows.Forms.TabPage tabPage采购批准单;
        private System.Windows.Forms.TabPage tabPage采购订单;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn查询销售订单;
        private System.Windows.Forms.ComboBox cmb销售订单审核状态;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp销售订单结束时间;
        private System.Windows.Forms.DateTimePicker dtp销售订单开始时间;
        private System.Windows.Forms.DataGridView dataGridView销售订单;
        private System.Windows.Forms.Button btn添加销售订单;
    }
}