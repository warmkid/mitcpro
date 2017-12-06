namespace mySystem.Process.灭菌
{
    partial class 辐照灭菌台帐
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
            this.b打印 = new System.Windows.Forms.Button();
            this.bt添加 = new System.Windows.Forms.Button();
            this.bt保存 = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.bt删除 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.bt查看日志 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker委托日期开始 = new System.Windows.Forms.DateTimePicker();
            this.textBox委托单号 = new System.Windows.Forms.TextBox();
            this.btn查询 = new System.Windows.Forms.Button();
            this.dateTimePicker委托日期结束 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox产品代码 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(296, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Gamma 射线辐照灭菌台帐";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(26, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(858, 253);
            this.dataGridView1.TabIndex = 1;
            // 
            // b打印
            // 
            this.b打印.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.b打印.Location = new System.Drawing.Point(663, 423);
            this.b打印.Name = "b打印";
            this.b打印.Size = new System.Drawing.Size(75, 24);
            this.b打印.TabIndex = 2;
            this.b打印.Text = "打印";
            this.b打印.UseVisualStyleBackColor = true;
            this.b打印.Click += new System.EventHandler(this.b打印_Click);
            // 
            // bt添加
            // 
            this.bt添加.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt添加.Location = new System.Drawing.Point(28, 423);
            this.bt添加.Name = "bt添加";
            this.bt添加.Size = new System.Drawing.Size(75, 24);
            this.bt添加.TabIndex = 3;
            this.bt添加.Text = "添加";
            this.bt添加.UseVisualStyleBackColor = true;
            this.bt添加.Visible = false;
            this.bt添加.Click += new System.EventHandler(this.bt添加_Click);
            // 
            // bt保存
            // 
            this.bt保存.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt保存.Location = new System.Drawing.Point(140, 423);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 24);
            this.bt保存.TabIndex = 4;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            this.bt保存.Visible = false;
            this.bt保存.Click += new System.EventHandler(this.bt保存_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label40.Location = new System.Drawing.Point(331, 426);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(104, 16);
            this.label40.TabIndex = 32;
            this.label40.Text = "选择打印机：";
            // 
            // cb打印机
            // 
            this.cb打印机.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(441, 423);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(205, 24);
            this.cb打印机.TabIndex = 31;
            // 
            // bt删除
            // 
            this.bt删除.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt删除.Location = new System.Drawing.Point(239, 423);
            this.bt删除.Name = "bt删除";
            this.bt删除.Size = new System.Drawing.Size(75, 24);
            this.bt删除.TabIndex = 33;
            this.bt删除.Text = "删除";
            this.bt删除.UseVisualStyleBackColor = true;
            this.bt删除.Visible = false;
            this.bt删除.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(643, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(742, 19);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(0, 16);
            this.label角色.TabIndex = 35;
            // 
            // bt查看日志
            // 
            this.bt查看日志.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt查看日志.Location = new System.Drawing.Point(777, 423);
            this.bt查看日志.Name = "bt查看日志";
            this.bt查看日志.Size = new System.Drawing.Size(98, 24);
            this.bt查看日志.TabIndex = 36;
            this.bt查看日志.Text = "查看日志";
            this.bt查看日志.UseVisualStyleBackColor = true;
            this.bt查看日志.Visible = false;
            this.bt查看日志.Click += new System.EventHandler(this.bt查看日志_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(31, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 37;
            this.label3.Text = "委托日期";
            // 
            // dateTimePicker委托日期开始
            // 
            this.dateTimePicker委托日期开始.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker委托日期开始.Location = new System.Drawing.Point(130, 48);
            this.dateTimePicker委托日期开始.Name = "dateTimePicker委托日期开始";
            this.dateTimePicker委托日期开始.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker委托日期开始.TabIndex = 38;
            // 
            // textBox委托单号
            // 
            this.textBox委托单号.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox委托单号.Location = new System.Drawing.Point(130, 91);
            this.textBox委托单号.Name = "textBox委托单号";
            this.textBox委托单号.Size = new System.Drawing.Size(200, 26);
            this.textBox委托单号.TabIndex = 39;
            // 
            // btn查询
            // 
            this.btn查询.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn查询.Location = new System.Drawing.Point(809, 99);
            this.btn查询.Name = "btn查询";
            this.btn查询.Size = new System.Drawing.Size(75, 24);
            this.btn查询.TabIndex = 40;
            this.btn查询.Text = "查询";
            this.btn查询.UseVisualStyleBackColor = true;
            this.btn查询.Click += new System.EventHandler(this.btn查询_Click);
            // 
            // dateTimePicker委托日期结束
            // 
            this.dateTimePicker委托日期结束.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker委托日期结束.Location = new System.Drawing.Point(478, 48);
            this.dateTimePicker委托日期结束.Name = "dateTimePicker委托日期结束";
            this.dateTimePicker委托日期结束.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker委托日期结束.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(31, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 42;
            this.label4.Text = "委托单号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(363, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 43;
            this.label5.Text = "产品代码";
            // 
            // textBox产品代码
            // 
            this.textBox产品代码.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox产品代码.Location = new System.Drawing.Point(478, 97);
            this.textBox产品代码.Name = "textBox产品代码";
            this.textBox产品代码.Size = new System.Drawing.Size(200, 26);
            this.textBox产品代码.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(378, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 16);
            this.label6.TabIndex = 45;
            this.label6.Text = "---";
            // 
            // 辐照灭菌台帐
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 458);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox产品代码);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker委托日期结束);
            this.Controls.Add(this.btn查询);
            this.Controls.Add(this.textBox委托单号);
            this.Controls.Add(this.dateTimePicker委托日期开始);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt查看日志);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bt删除);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.bt添加);
            this.Controls.Add(this.b打印);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "辐照灭菌台帐";
            this.Text = "辐照灭菌台帐";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.辐照灭菌台帐_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button b打印;
        private System.Windows.Forms.Button bt添加;
        private System.Windows.Forms.Button bt保存;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Button bt删除;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Button bt查看日志;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker委托日期开始;
        private System.Windows.Forms.TextBox textBox委托单号;
        private System.Windows.Forms.Button btn查询;
        private System.Windows.Forms.DateTimePicker dateTimePicker委托日期结束;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox产品代码;
        private System.Windows.Forms.Label label6;
    }
}