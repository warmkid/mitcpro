namespace mySystem.Extruction.Process
{
    partial class ProductInnerPackagingRecord
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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.包材 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.接上班数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.领取数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.剩余数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.使用数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.退库数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.checkerBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.recorderBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.生产日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内包序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品外观 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.包装后外观 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.包装袋热封线 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.贴标签 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.贴指示剂 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.包装人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.包材,
            this.批号,
            this.接上班数量,
            this.领取数量,
            this.剩余数量,
            this.使用数量,
            this.退库数量});
            this.dataGridView2.Location = new System.Drawing.Point(16, 402);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(880, 67);
            this.dataGridView2.TabIndex = 24;
            // 
            // 包材
            // 
            this.包材.HeaderText = "包材";
            this.包材.Name = "包材";
            // 
            // 批号
            // 
            this.批号.HeaderText = "批号";
            this.批号.Name = "批号";
            // 
            // 接上班数量
            // 
            this.接上班数量.HeaderText = "接上班数量";
            this.接上班数量.Name = "接上班数量";
            // 
            // 领取数量
            // 
            this.领取数量.HeaderText = "领取数量";
            this.领取数量.Name = "领取数量";
            // 
            // 剩余数量
            // 
            this.剩余数量.HeaderText = "剩余数量";
            this.剩余数量.Name = "剩余数量";
            // 
            // 使用数量
            // 
            this.使用数量.HeaderText = "使用数量";
            this.使用数量.Name = "使用数量";
            // 
            // 退库数量
            // 
            this.退库数量.HeaderText = "退库数量";
            this.退库数量.Name = "退库数量";
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(819, 597);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 23;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(727, 597);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 22;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // checkerBox
            // 
            this.checkerBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkerBox.Location = new System.Drawing.Point(569, 559);
            this.checkerBox.Name = "checkerBox";
            this.checkerBox.Size = new System.Drawing.Size(100, 26);
            this.checkerBox.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 12F);
            this.label8.Location = new System.Drawing.Point(504, 563);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "复核人：";
            // 
            // recorderBox
            // 
            this.recorderBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.recorderBox.Location = new System.Drawing.Point(81, 559);
            this.recorderBox.Name = "recorderBox";
            this.recorderBox.Size = new System.Drawing.Size(100, 26);
            this.recorderBox.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(16, 563);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "操作人：";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Font = new System.Drawing.Font("SimSun", 12F);
            this.dateTimePicker3.Location = new System.Drawing.Point(750, 558);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(144, 26);
            this.dateTimePicker3.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(701, 563);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "日期：";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("SimSun", 12F);
            this.dateTimePicker2.Location = new System.Drawing.Point(272, 558);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(147, 26);
            this.dateTimePicker2.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(222, 563);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "日期：";
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(816, 351);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 13;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(724, 351);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 12;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.生产日期,
            this.内包序号,
            this.产品外观,
            this.包装后外观,
            this.包装袋热封线,
            this.贴标签,
            this.贴指示剂,
            this.包装人});
            this.dataGridView1.Location = new System.Drawing.Point(16, 120);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(880, 225);
            this.dataGridView1.TabIndex = 7;
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 生产日期
            // 
            this.生产日期.HeaderText = "生产日期";
            this.生产日期.Name = "生产日期";
            // 
            // 内包序号
            // 
            this.内包序号.HeaderText = "内包序号";
            this.内包序号.Name = "内包序号";
            // 
            // 产品外观
            // 
            this.产品外观.HeaderText = "产品外观";
            this.产品外观.Name = "产品外观";
            this.产品外观.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.产品外观.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 包装后外观
            // 
            this.包装后外观.HeaderText = "包装后外观";
            this.包装后外观.Name = "包装后外观";
            this.包装后外观.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.包装后外观.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 包装袋热封线
            // 
            this.包装袋热封线.HeaderText = "包装袋热封线";
            this.包装袋热封线.Name = "包装袋热封线";
            this.包装袋热封线.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.包装袋热封线.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 贴标签
            // 
            this.贴标签.HeaderText = "贴标签";
            this.贴标签.Name = "贴标签";
            this.贴标签.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.贴标签.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 贴指示剂
            // 
            this.贴指示剂.HeaderText = "贴指示剂";
            this.贴指示剂.Name = "贴指示剂";
            this.贴指示剂.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.贴指示剂.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 包装人
            // 
            this.包装人.HeaderText = "包装人";
            this.包装人.Name = "包装人";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("SimSun", 12F);
            this.dateTimePicker1.Location = new System.Drawing.Point(696, 69);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(605, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "生产日期：";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox2.Location = new System.Drawing.Point(410, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(133, 26);
            this.textBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(328, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "产品批号：";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox1.Location = new System.Drawing.Point(137, 69);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 26);
            this.textBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(13, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "产品代码/规格：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(406, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "产品内包装记录";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F);
            this.label9.Location = new System.Drawing.Point(16, 482);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(888, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "标签： 发放数量                  张；       使用数量                  张；      销毁数量            " +
                "      张。";
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox5.Location = new System.Drawing.Point(144, 477);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(132, 26);
            this.textBox5.TabIndex = 26;
            // 
            // textBox6
            // 
            this.textBox6.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox6.Location = new System.Drawing.Point(440, 478);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(132, 26);
            this.textBox6.TabIndex = 27;
            // 
            // textBox7
            // 
            this.textBox7.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox7.Location = new System.Drawing.Point(729, 477);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(132, 26);
            this.textBox7.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F);
            this.label10.Location = new System.Drawing.Point(16, 513);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(216, 16);
            this.label10.TabIndex = 29;
            this.label10.Text = "包装规格：           只/包";
            // 
            // textBox8
            // 
            this.textBox8.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox8.Location = new System.Drawing.Point(97, 511);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(84, 26);
            this.textBox8.TabIndex = 30;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("SimSun", 12F);
            this.label11.Location = new System.Drawing.Point(277, 513);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(400, 16);
            this.label11.TabIndex = 31;
            this.label11.Text = "总计：共包装            包；计            只/片。";
            // 
            // textBox9
            // 
            this.textBox9.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox9.Location = new System.Drawing.Point(382, 511);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(84, 26);
            this.textBox9.TabIndex = 32;
            // 
            // textBox10
            // 
            this.textBox10.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox10.Location = new System.Drawing.Point(526, 511);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(84, 26);
            this.textBox10.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("SimSun", 12F);
            this.label12.Location = new System.Drawing.Point(722, 512);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 16);
            this.label12.TabIndex = 34;
            this.label12.Text = "标签：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkBox1.Location = new System.Drawing.Point(775, 509);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 20);
            this.checkBox1.TabIndex = 35;
            this.checkBox1.Text = "中文";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("SimSun", 12F);
            this.checkBox2.Location = new System.Drawing.Point(840, 509);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 20);
            this.checkBox2.TabIndex = 36;
            this.checkBox2.Text = "英文";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // ProductInnerPackagingRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 640);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.checkerBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.recorderBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ProductInnerPackagingRecord";
            this.Text = "ProductInnerPackagingRecord";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包序号;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 产品外观;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 包装后外观;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 包装袋热封线;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 贴标签;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 贴指示剂;
        private System.Windows.Forms.DataGridViewTextBoxColumn 包装人;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox recorderBox;
        private System.Windows.Forms.TextBox checkerBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 包材;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 接上班数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 领取数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 剩余数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 使用数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 退库数量;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}