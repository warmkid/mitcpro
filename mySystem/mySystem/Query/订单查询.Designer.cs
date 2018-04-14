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
            this.dgv销售订单 = new System.Windows.Forms.DataGridView();
            this.btn销售订单查询 = new System.Windows.Forms.Button();
            this.tb销售订单产品名称 = new System.Windows.Forms.TextBox();
            this.tb销售订单产品代码 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage采购订单 = new System.Windows.Forms.TabPage();
            this.tb采购订单供应商 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn采购订单保存 = new System.Windows.Forms.Button();
            this.dgv采购订单 = new System.Windows.Forms.DataGridView();
            this.btn查询采购订单 = new System.Windows.Forms.Button();
            this.tb采购订单销售订单 = new System.Windows.Forms.TextBox();
            this.tb采购订单产品代码 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage销售订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).BeginInit();
            this.tabPage采购订单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购订单)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage销售订单);
            this.tabControl1.Controls.Add(this.tabPage采购订单);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1055, 552);
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
            this.tabPage销售订单.Size = new System.Drawing.Size(1047, 519);
            this.tabPage销售订单.TabIndex = 0;
            this.tabPage销售订单.Text = "销售订单";
            this.tabPage销售订单.UseVisualStyleBackColor = true;
            // 
            // dgv销售订单
            // 
            this.dgv销售订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv销售订单.Location = new System.Drawing.Point(18, 72);
            this.dgv销售订单.Name = "dgv销售订单";
            this.dgv销售订单.RowTemplate.Height = 23;
            this.dgv销售订单.Size = new System.Drawing.Size(1022, 427);
            this.dgv销售订单.TabIndex = 38;
            // 
            // btn销售订单查询
            // 
            this.btn销售订单查询.Font = new System.Drawing.Font("宋体", 12F);
            this.btn销售订单查询.Location = new System.Drawing.Point(965, 20);
            this.btn销售订单查询.Name = "btn销售订单查询";
            this.btn销售订单查询.Size = new System.Drawing.Size(75, 30);
            this.btn销售订单查询.TabIndex = 37;
            this.btn销售订单查询.Text = "查询";
            this.btn销售订单查询.UseVisualStyleBackColor = true;
            this.btn销售订单查询.Click += new System.EventHandler(this.btn销售订单查询_Click);
            // 
            // tb销售订单产品名称
            // 
            this.tb销售订单产品名称.Font = new System.Drawing.Font("宋体", 12F);
            this.tb销售订单产品名称.Location = new System.Drawing.Point(546, 20);
            this.tb销售订单产品名称.Name = "tb销售订单产品名称";
            this.tb销售订单产品名称.Size = new System.Drawing.Size(202, 26);
            this.tb销售订单产品名称.TabIndex = 36;
            // 
            // tb销售订单产品代码
            // 
            this.tb销售订单产品代码.Font = new System.Drawing.Font("宋体", 12F);
            this.tb销售订单产品代码.Location = new System.Drawing.Point(109, 20);
            this.tb销售订单产品代码.Name = "tb销售订单产品代码";
            this.tb销售订单产品代码.Size = new System.Drawing.Size(202, 26);
            this.tb销售订单产品代码.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(441, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "产品名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 33;
            this.label1.Text = "产品代码";
            // 
            // tabPage采购订单
            // 
            this.tabPage采购订单.Controls.Add(this.tb采购订单供应商);
            this.tabPage采购订单.Controls.Add(this.label5);
            this.tabPage采购订单.Controls.Add(this.btn采购订单保存);
            this.tabPage采购订单.Controls.Add(this.dgv采购订单);
            this.tabPage采购订单.Controls.Add(this.btn查询采购订单);
            this.tabPage采购订单.Controls.Add(this.tb采购订单销售订单);
            this.tabPage采购订单.Controls.Add(this.tb采购订单产品代码);
            this.tabPage采购订单.Controls.Add(this.label3);
            this.tabPage采购订单.Controls.Add(this.label4);
            this.tabPage采购订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购订单.Name = "tabPage采购订单";
            this.tabPage采购订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购订单.Size = new System.Drawing.Size(1047, 519);
            this.tabPage采购订单.TabIndex = 1;
            this.tabPage采购订单.Text = "采购订单";
            this.tabPage采购订单.UseVisualStyleBackColor = true;
            // 
            // tb采购订单供应商
            // 
            this.tb采购订单供应商.Font = new System.Drawing.Font("宋体", 12F);
            this.tb采购订单供应商.Location = new System.Drawing.Point(663, 20);
            this.tb采购订单供应商.Name = "tb采购订单供应商";
            this.tb采购订单供应商.Size = new System.Drawing.Size(123, 26);
            this.tb采购订单供应商.TabIndex = 47;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F);
            this.label5.Location = new System.Drawing.Point(558, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 46;
            this.label5.Text = "供应商";
            // 
            // btn采购订单保存
            // 
            this.btn采购订单保存.Font = new System.Drawing.Font("宋体", 12F);
            this.btn采购订单保存.Location = new System.Drawing.Point(848, 20);
            this.btn采购订单保存.Name = "btn采购订单保存";
            this.btn采购订单保存.Size = new System.Drawing.Size(75, 30);
            this.btn采购订单保存.TabIndex = 45;
            this.btn采购订单保存.Text = "保存";
            this.btn采购订单保存.UseVisualStyleBackColor = true;
            this.btn采购订单保存.Click += new System.EventHandler(this.btn采购订单保存_Click);
            // 
            // dgv采购订单
            // 
            this.dgv采购订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv采购订单.Location = new System.Drawing.Point(17, 72);
            this.dgv采购订单.Name = "dgv采购订单";
            this.dgv采购订单.RowTemplate.Height = 23;
            this.dgv采购订单.Size = new System.Drawing.Size(1006, 427);
            this.dgv采购订单.TabIndex = 44;
            // 
            // btn查询采购订单
            // 
            this.btn查询采购订单.Font = new System.Drawing.Font("宋体", 12F);
            this.btn查询采购订单.Location = new System.Drawing.Point(948, 20);
            this.btn查询采购订单.Name = "btn查询采购订单";
            this.btn查询采购订单.Size = new System.Drawing.Size(75, 30);
            this.btn查询采购订单.TabIndex = 43;
            this.btn查询采购订单.Text = "查询";
            this.btn查询采购订单.UseVisualStyleBackColor = true;
            this.btn查询采购订单.Click += new System.EventHandler(this.btn采购订单查询_Click);
            // 
            // tb采购订单销售订单
            // 
            this.tb采购订单销售订单.Font = new System.Drawing.Font("宋体", 12F);
            this.tb采购订单销售订单.Location = new System.Drawing.Point(406, 20);
            this.tb采购订单销售订单.Name = "tb采购订单销售订单";
            this.tb采购订单销售订单.Size = new System.Drawing.Size(123, 26);
            this.tb采购订单销售订单.TabIndex = 42;
            // 
            // tb采购订单产品代码
            // 
            this.tb采购订单产品代码.Font = new System.Drawing.Font("宋体", 12F);
            this.tb采购订单产品代码.Location = new System.Drawing.Point(115, 20);
            this.tb采购订单产品代码.Name = "tb采购订单产品代码";
            this.tb采购订单产品代码.Size = new System.Drawing.Size(123, 26);
            this.tb采购订单产品代码.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(301, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 40;
            this.label3.Text = "销售订单";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(21, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 39;
            this.label4.Text = "产品代码";
            // 
            // 订单查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 613);
            this.Controls.Add(this.tabControl1);
            this.Name = "订单查询";
            this.Text = "订单查询";
            this.tabControl1.ResumeLayout(false);
            this.tabPage销售订单.ResumeLayout(false);
            this.tabPage销售订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv销售订单)).EndInit();
            this.tabPage采购订单.ResumeLayout(false);
            this.tabPage采购订单.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv采购订单)).EndInit();
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
        private System.Windows.Forms.TabPage tabPage采购订单;
        private System.Windows.Forms.DataGridView dgv采购订单;
        private System.Windows.Forms.Button btn查询采购订单;
        private System.Windows.Forms.TextBox tb采购订单销售订单;
        private System.Windows.Forms.TextBox tb采购订单产品代码;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn采购订单保存;
        private System.Windows.Forms.TextBox tb采购订单供应商;
        private System.Windows.Forms.Label label5;
    }
}