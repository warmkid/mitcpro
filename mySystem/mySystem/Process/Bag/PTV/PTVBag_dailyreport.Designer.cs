﻿namespace mySystem.Process.Bag.PTV
{
    partial class PTVBag_dailyreport
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
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.bt保存 = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.bt查看日志 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.label角色 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(328, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "PTV生产日报表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(884, 150);
            this.dataGridView1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(639, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "登录角色：";
            // 
            // bt保存
            // 
            this.bt保存.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt保存.Location = new System.Drawing.Point(199, 229);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 23);
            this.bt保存.TabIndex = 43;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label40.Location = new System.Drawing.Point(300, 231);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(104, 16);
            this.label40.TabIndex = 42;
            this.label40.Text = "选择打印机：";
            // 
            // cb打印机
            // 
            this.cb打印机.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(410, 228);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(205, 24);
            this.cb打印机.TabIndex = 41;
            // 
            // bt查看日志
            // 
            this.bt查看日志.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt查看日志.Location = new System.Drawing.Point(793, 227);
            this.bt查看日志.Name = "bt查看日志";
            this.bt查看日志.Size = new System.Drawing.Size(94, 23);
            this.bt查看日志.TabIndex = 40;
            this.bt查看日志.Text = "查看日志";
            this.bt查看日志.UseVisualStyleBackColor = true;
            // 
            // bt打印
            // 
            this.bt打印.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt打印.Location = new System.Drawing.Point(670, 227);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(75, 23);
            this.bt打印.TabIndex = 39;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(738, 15);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 44;
            this.label角色.Text = "角色";
            // 
            // PTVBag_dailyreport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 262);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.bt查看日志);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "PTVBag_dailyreport";
            this.Text = "PTVBag_dailyreport";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Button bt查看日志;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Label label角色;
    }
}