namespace mySystem.Query
{
    partial class 订单查询
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb销售订单产品代码 = new System.Windows.Forms.TextBox();
            this.tb销售订单产品名称 = new System.Windows.Forms.TextBox();
            this.btn销售订单查询 = new System.Windows.Forms.Button();
            this.dgv销售订单 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage销售订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage销售订单);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1081, 552);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage销售订单
            // 
            this.tabPage销售订单.Controls.Add(this.dgv销售订单);
            this.tabPage销售订单.Controls.Add(this.btn销售订单查询);
            this.tabPage销售订单.Controls.Add(this.tb销售订单产品名称);
            this.tabPage销售订单.Controls.Add(this.tb销售订单产品代码);
            this.tabPage销售订单.Controls.Add(this.label2);
            this.tabPage销售订单.Controls.Add(this.label1);
            this.tabPage销售订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage销售订单.Name = "tabPage销售订单";
            this.tabPage销售订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage销售订单.Size = new System.Drawing.Size(1073, 519);
            this.tabPage销售订单.TabIndex = 0;
            this.tabPage销售订单.Text = "销售订单";
            this.tabPage销售订单.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 33;
            this.label1.Text = "产品代码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(441, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "产品名称";
            // 
            // tb销售订单产品代码
            // 
            this.tb销售订单产品代码.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb销售订单产品代码.Location = new System.Drawing.Point(109, 20);
            this.tb销售订单产品代码.Name = "tb销售订单产品代码";
            this.tb销售订单产品代码.Size = new System.Drawing.Size(202, 26);
            this.tb销售订单产品代码.TabIndex = 35;
            // 
            // tb销售订单产品名称
            // 
            this.tb销售订单产品名称.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb销售订单产品名称.Location = new System.Drawing.Point(546, 20);
            this.tb销售订单产品名称.Name = "tb销售订单产品名称";
            this.tb销售订单产品名称.Size = new System.Drawing.Size(202, 26);
            this.tb销售订单产品名称.TabIndex = 36;
            // 
            // btn销售订单查询
            // 
            this.btn销售订单查询.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn销售订单查询.Location = new System.Drawing.Point(982, 20);
            this.btn销售订单查询.Name = "btn销售订单查询";
            this.btn销售订单查询.Size = new System.Drawing.Size(75, 30);
            this.btn销售订单查询.TabIndex = 37;
            this.btn销售订单查询.Text = "查询";
            this.btn销售订单查询.UseVisualStyleBackColor = true;
            this.btn销售订单查询.Click += new System.EventHandler(this.btn销售订单查询_Click);
            // 
            // dgv销售订单
            // 
            this.dgv销售订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv销售订单.Location = new System.Drawing.Point(18, 72);
            this.dgv销售订单.Name = "dgv销售订单";
            this.dgv销售订单.RowTemplate.Height = 23;
            this.dgv销售订单.Size = new System.Drawing.Size(1039, 427);
            this.dgv销售订单.TabIndex = 38;
            // 
            // 订单查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 613);
            this.Controls.Add(this.tabControl1);
            this.Name = "订单查询";
            this.Text = "订单查询";
            this.tabControl1.ResumeLayout(false);
            this.tabPage销售订单.ResumeLayout(false);
            this.tabPage销售订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage销售订单;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb销售订单产品名称;
        private System.Windows.Forms.TextBox tb销售订单产品代码;
        private System.Windows.Forms.Button btn销售订单查询;
        private System.Windows.Forms.DataGridView dgv销售订单;
    }
}