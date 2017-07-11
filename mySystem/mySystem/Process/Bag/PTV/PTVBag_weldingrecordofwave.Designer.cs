namespace mySystem.Process.Bag.PTV
{
    partial class PTVBag_weldingrecordofwave
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
            this.dTime生产日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tB产品代码 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tB产品批号 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.焊接时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.焊接产品数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.压力 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.延迟时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.熔接时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.硬化熔接时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.操作人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tB不合格品数量 = new System.Windows.Forms.TextBox();
            this.tB合格品数量 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(406, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "超声波焊接记录";
            // 
            // dTime生产日期
            // 
            this.dTime生产日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dTime生产日期.Location = new System.Drawing.Point(764, 64);
            this.dTime生产日期.Name = "dTime生产日期";
            this.dTime生产日期.Size = new System.Drawing.Size(141, 26);
            this.dTime生产日期.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(679, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "生产日期：";
            // 
            // tB产品代码
            // 
            this.tB产品代码.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tB产品代码.Location = new System.Drawing.Point(115, 68);
            this.tB产品代码.Name = "tB产品代码";
            this.tB产品代码.Size = new System.Drawing.Size(179, 26);
            this.tB产品代码.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(29, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "产品代码：";
            // 
            // tB产品批号
            // 
            this.tB产品批号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tB产品批号.Location = new System.Drawing.Point(440, 69);
            this.tB产品批号.Name = "tB产品批号";
            this.tB产品批号.Size = new System.Drawing.Size(184, 26);
            this.tB产品批号.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(353, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "产品批号：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.焊接时间,
            this.焊接产品数量,
            this.压力,
            this.延迟时间,
            this.熔接时间,
            this.硬化熔接时间,
            this.操作人});
            this.dataGridView1.Location = new System.Drawing.Point(32, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(873, 336);
            this.dataGridView1.TabIndex = 13;
            // 
            // 焊接时间
            // 
            this.焊接时间.HeaderText = "焊接时间";
            this.焊接时间.Name = "焊接时间";
            this.焊接时间.Width = 120;
            // 
            // 焊接产品数量
            // 
            this.焊接产品数量.HeaderText = "焊接产品   数量";
            this.焊接产品数量.Name = "焊接产品数量";
            // 
            // 压力
            // 
            this.压力.HeaderText = "压力(bar)";
            this.压力.Name = "压力";
            this.压力.Width = 90;
            // 
            // 延迟时间
            // 
            this.延迟时间.HeaderText = "延迟时间(s)";
            this.延迟时间.Name = "延迟时间";
            this.延迟时间.Width = 150;
            // 
            // 熔接时间
            // 
            this.熔接时间.HeaderText = "熔接时间(s)";
            this.熔接时间.Name = "熔接时间";
            this.熔接时间.Width = 150;
            // 
            // 硬化熔接时间
            // 
            this.硬化熔接时间.HeaderText = "硬化熔接时间(s)";
            this.硬化熔接时间.Name = "硬化熔接时间";
            this.硬化熔接时间.Width = 170;
            // 
            // 操作人
            // 
            this.操作人.HeaderText = "操作人";
            this.操作人.Name = "操作人";
            this.操作人.Width = 90;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker2.Location = new System.Drawing.Point(734, 560);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(136, 26);
            this.dateTimePicker2.TabIndex = 17;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(734, 519);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(655, 566);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "审核日期：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(655, 522);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "审核人：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tB不合格品数量);
            this.groupBox1.Controls.Add(this.tB合格品数量);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(32, 494);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(566, 110);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 70);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(463, 26);
            this.textBox1.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 23;
            this.label9.Text = "不良描述：";
            // 
            // tB不合格品数量
            // 
            this.tB不合格品数量.Location = new System.Drawing.Point(318, 37);
            this.tB不合格品数量.Name = "tB不合格品数量";
            this.tB不合格品数量.Size = new System.Drawing.Size(81, 26);
            this.tB不合格品数量.TabIndex = 22;
            // 
            // tB合格品数量
            // 
            this.tB合格品数量.Location = new System.Drawing.Point(97, 37);
            this.tB合格品数量.Name = "tB合格品数量";
            this.tB合格品数量.Size = new System.Drawing.Size(81, 26);
            this.tB合格品数量.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(432, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "合格品数量            只，不合格品数量           只，\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "备注：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(135, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 16);
            this.label10.TabIndex = 19;
            this.label10.Text = "设定：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(253, 120);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(92, 26);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "0.4";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PTVBag_weldingrecordofwave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 638);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tB产品批号);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tB产品代码);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dTime生产日期);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PTVBag_weldingrecordofwave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "超声波焊接记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dTime生产日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tB产品代码;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB产品批号;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 焊接时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 焊接产品数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 压力;
        private System.Windows.Forms.DataGridViewTextBoxColumn 延迟时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 熔接时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 硬化熔接时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 操作人;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tB不合格品数量;
        private System.Windows.Forms.TextBox tB合格品数量;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox3;
    }
}