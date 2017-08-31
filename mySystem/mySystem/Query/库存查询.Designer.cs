namespace mySystem.Query
{
    partial class 库存查询
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
            this.tabPage退货台账 = new System.Windows.Forms.TabPage();
            this.dgv退货台账 = new System.Windows.Forms.DataGridView();
            this.btn退货台账查询 = new System.Windows.Forms.Button();
            this.tb退货台账销售合同号 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp退货台账申请开始时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp退货台账申请结束时间 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb退货台账客户名称 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb退货台账产品名称 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb退货台账产品批号 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage退货台账.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv退货台账)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage退货台账);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1081, 552);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage退货台账
            // 
            this.tabPage退货台账.Controls.Add(this.tb退货台账产品批号);
            this.tabPage退货台账.Controls.Add(this.label6);
            this.tabPage退货台账.Controls.Add(this.tb退货台账产品名称);
            this.tabPage退货台账.Controls.Add(this.label5);
            this.tabPage退货台账.Controls.Add(this.tb退货台账客户名称);
            this.tabPage退货台账.Controls.Add(this.label4);
            this.tabPage退货台账.Controls.Add(this.label3);
            this.tabPage退货台账.Controls.Add(this.dtp退货台账申请结束时间);
            this.tabPage退货台账.Controls.Add(this.dtp退货台账申请开始时间);
            this.tabPage退货台账.Controls.Add(this.dgv退货台账);
            this.tabPage退货台账.Controls.Add(this.btn退货台账查询);
            this.tabPage退货台账.Controls.Add(this.tb退货台账销售合同号);
            this.tabPage退货台账.Controls.Add(this.label2);
            this.tabPage退货台账.Controls.Add(this.label1);
            this.tabPage退货台账.Location = new System.Drawing.Point(4, 29);
            this.tabPage退货台账.Name = "tabPage退货台账";
            this.tabPage退货台账.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage退货台账.Size = new System.Drawing.Size(1073, 519);
            this.tabPage退货台账.TabIndex = 0;
            this.tabPage退货台账.Text = "退货台账";
            this.tabPage退货台账.UseVisualStyleBackColor = true;
            // 
            // dgv退货台账
            // 
            this.dgv退货台账.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv退货台账.Location = new System.Drawing.Point(18, 108);
            this.dgv退货台账.Name = "dgv退货台账";
            this.dgv退货台账.RowTemplate.Height = 23;
            this.dgv退货台账.Size = new System.Drawing.Size(1039, 391);
            this.dgv退货台账.TabIndex = 38;
            // 
            // btn退货台账查询
            // 
            this.btn退货台账查询.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn退货台账查询.Location = new System.Drawing.Point(982, 20);
            this.btn退货台账查询.Name = "btn退货台账查询";
            this.btn退货台账查询.Size = new System.Drawing.Size(75, 30);
            this.btn退货台账查询.TabIndex = 37;
            this.btn退货台账查询.Text = "查询";
            this.btn退货台账查询.UseVisualStyleBackColor = true;
            this.btn退货台账查询.Click += new System.EventHandler(this.btn退货台账查询_Click);
            // 
            // tb退货台账销售合同号
            // 
            this.tb退货台账销售合同号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb退货台账销售合同号.Location = new System.Drawing.Point(727, 25);
            this.tb退货台账销售合同号.Name = "tb退货台账销售合同号";
            this.tb退货台账销售合同号.Size = new System.Drawing.Size(202, 26);
            this.tb退货台账销售合同号.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(622, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "销售合同号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 33;
            this.label1.Text = "申请日期";
            // 
            // dtp退货台账申请开始时间
            // 
            this.dtp退货台账申请开始时间.Location = new System.Drawing.Point(111, 18);
            this.dtp退货台账申请开始时间.Name = "dtp退货台账申请开始时间";
            this.dtp退货台账申请开始时间.Size = new System.Drawing.Size(200, 26);
            this.dtp退货台账申请开始时间.TabIndex = 39;
            // 
            // dtp退货台账申请结束时间
            // 
            this.dtp退货台账申请结束时间.Location = new System.Drawing.Point(399, 20);
            this.dtp退货台账申请结束时间.Name = "dtp退货台账申请结束时间";
            this.dtp退货台账申请结束时间.Size = new System.Drawing.Size(200, 26);
            this.dtp退货台账申请结束时间.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(337, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "---";
            // 
            // tb退货台账客户名称
            // 
            this.tb退货台账客户名称.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb退货台账客户名称.Location = new System.Drawing.Point(109, 59);
            this.tb退货台账客户名称.Name = "tb退货台账客户名称";
            this.tb退货台账客户名称.Size = new System.Drawing.Size(202, 26);
            this.tb退货台账客户名称.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(23, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 42;
            this.label4.Text = "客户名称";
            // 
            // tb退货台账产品名称
            // 
            this.tb退货台账产品名称.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb退货台账产品名称.Location = new System.Drawing.Point(448, 59);
            this.tb退货台账产品名称.Name = "tb退货台账产品名称";
            this.tb退货台账产品名称.Size = new System.Drawing.Size(176, 26);
            this.tb退货台账产品名称.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(351, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "产品名称";
            // 
            // tb退货台账产品批号
            // 
            this.tb退货台账产品批号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb退货台账产品批号.Location = new System.Drawing.Point(753, 62);
            this.tb退货台账产品批号.Name = "tb退货台账产品批号";
            this.tb退货台账产品批号.Size = new System.Drawing.Size(176, 26);
            this.tb退货台账产品批号.TabIndex = 47;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(656, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 46;
            this.label6.Text = "产品批号";
            // 
            // 库存查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 581);
            this.Controls.Add(this.tabControl1);
            this.Name = "库存查询";
            this.Text = "库存查询";
            this.tabControl1.ResumeLayout(false);
            this.tabPage退货台账.ResumeLayout(false);
            this.tabPage退货台账.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv退货台账)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage退货台账;
        private System.Windows.Forms.DataGridView dgv退货台账;
        private System.Windows.Forms.Button btn退货台账查询;
        private System.Windows.Forms.TextBox tb退货台账销售合同号;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp退货台账申请结束时间;
        private System.Windows.Forms.DateTimePicker dtp退货台账申请开始时间;
        private System.Windows.Forms.TextBox tb退货台账客户名称;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb退货台账产品批号;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb退货台账产品名称;
        private System.Windows.Forms.Label label5;
    }
}