namespace mySystem.Process.Stock
{
    partial class 采购需求单
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
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb申请人 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb编号 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb采购年月 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb批准人 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtp申请日期 = new System.Windows.Forms.DateTimePicker();
            this.dtp批准日期 = new System.Windows.Forms.DateTimePicker();
            this.lbl合计理论生产数量 = new System.Windows.Forms.Label();
            this.lbl合计数量 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn查询插入
            // 
            this.btn查询插入.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn查询插入.Location = new System.Drawing.Point(251, 25);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(92, 23);
            this.btn查询插入.TabIndex = 40;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // btn保存
            // 
            this.btn保存.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn保存.Location = new System.Drawing.Point(254, 460);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 39;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn删除
            // 
            this.btn删除.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn删除.Location = new System.Drawing.Point(143, 460);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 38;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn添加
            // 
            this.btn添加.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn添加.Location = new System.Drawing.Point(40, 460);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 37;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(38, 161);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1071, 261);
            this.dataGridView1.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(994, 463);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 35;
            this.label4.Text = "理论合计";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(492, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "采购时间";
            // 
            // tb申请人
            // 
            this.tb申请人.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb申请人.Location = new System.Drawing.Point(966, 34);
            this.tb申请人.Name = "tb申请人";
            this.tb申请人.Size = new System.Drawing.Size(112, 26);
            this.tb申请人.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(888, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 32;
            this.label2.Text = "申请人";
            // 
            // tb编号
            // 
            this.tb编号.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb编号.Location = new System.Drawing.Point(115, 27);
            this.tb编号.Name = "tb编号";
            this.tb编号.Size = new System.Drawing.Size(112, 26);
            this.tb编号.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(35, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 30;
            this.label1.Text = "编号";
            // 
            // tb采购年月
            // 
            this.tb采购年月.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb采购年月.Location = new System.Drawing.Point(572, 30);
            this.tb采购年月.Name = "tb采购年月";
            this.tb采购年月.Size = new System.Drawing.Size(149, 26);
            this.tb采购年月.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(492, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 46;
            this.label6.Text = "申请日期";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(35, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 50;
            this.label7.Text = "批准时间";
            // 
            // tb批准人
            // 
            this.tb批准人.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb批准人.Location = new System.Drawing.Point(966, 75);
            this.tb批准人.Name = "tb批准人";
            this.tb批准人.Size = new System.Drawing.Size(112, 26);
            this.tb批准人.TabIndex = 49;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(888, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 48;
            this.label8.Text = "批准人";
            // 
            // dtp申请日期
            // 
            this.dtp申请日期.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp申请日期.Location = new System.Drawing.Point(572, 78);
            this.dtp申请日期.Name = "dtp申请日期";
            this.dtp申请日期.Size = new System.Drawing.Size(149, 26);
            this.dtp申请日期.TabIndex = 52;
            // 
            // dtp批准日期
            // 
            this.dtp批准日期.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp批准日期.Location = new System.Drawing.Point(115, 82);
            this.dtp批准日期.Name = "dtp批准日期";
            this.dtp批准日期.Size = new System.Drawing.Size(149, 26);
            this.dtp批准日期.TabIndex = 53;
            // 
            // lbl合计理论生产数量
            // 
            this.lbl合计理论生产数量.AutoSize = true;
            this.lbl合计理论生产数量.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl合计理论生产数量.Location = new System.Drawing.Point(1072, 463);
            this.lbl合计理论生产数量.Name = "lbl合计理论生产数量";
            this.lbl合计理论生产数量.Size = new System.Drawing.Size(16, 16);
            this.lbl合计理论生产数量.TabIndex = 54;
            this.lbl合计理论生产数量.Text = "0";
            // 
            // lbl合计数量
            // 
            this.lbl合计数量.AutoSize = true;
            this.lbl合计数量.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl合计数量.Location = new System.Drawing.Point(947, 463);
            this.lbl合计数量.Name = "lbl合计数量";
            this.lbl合计数量.Size = new System.Drawing.Size(16, 16);
            this.lbl合计数量.TabIndex = 56;
            this.lbl合计数量.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(869, 463);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 16);
            this.label9.TabIndex = 55;
            this.label9.Text = "数量合计";
            // 
            // 采购需求单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 514);
            this.Controls.Add(this.lbl合计数量);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbl合计理论生产数量);
            this.Controls.Add(this.dtp批准日期);
            this.Controls.Add(this.dtp申请日期);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb批准人);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb采购年月);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb申请人);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb编号);
            this.Controls.Add(this.label1);
            this.Name = "采购需求单";
            this.Text = "采购需求单";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb申请人;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb编号;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb采购年月;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb批准人;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtp申请日期;
        private System.Windows.Forms.DateTimePicker dtp批准日期;
        private System.Windows.Forms.Label lbl合计理论生产数量;
        private System.Windows.Forms.Label lbl合计数量;
        private System.Windows.Forms.Label label9;
    }
}