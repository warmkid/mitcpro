namespace mySystem.Process.Stock
{
    partial class 出库退库单
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
            this.tp退库单 = new System.Windows.Forms.TabPage();
            this.dgv退库单 = new System.Windows.Forms.DataGridView();
            this.btn退库单读取 = new System.Windows.Forms.Button();
            this.tp出库单 = new System.Windows.Forms.TabPage();
            this.dgv出库单 = new System.Windows.Forms.DataGridView();
            this.btn出库单读取 = new System.Windows.Forms.Button();
            this.dateTimePicker开始 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker结束 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox审核状态 = new System.Windows.Forms.ComboBox();
            this.button查询 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tp退库单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv退库单)).BeginInit();
            this.tp出库单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv出库单)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tp退库单);
            this.tabControl1.Controls.Add(this.tp出库单);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(19, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(767, 461);
            this.tabControl1.TabIndex = 0;
            // 
            // tp退库单
            // 
            this.tp退库单.Controls.Add(this.dgv退库单);
            this.tp退库单.Controls.Add(this.btn退库单读取);
            this.tp退库单.Location = new System.Drawing.Point(4, 29);
            this.tp退库单.Name = "tp退库单";
            this.tp退库单.Padding = new System.Windows.Forms.Padding(3);
            this.tp退库单.Size = new System.Drawing.Size(759, 428);
            this.tp退库单.TabIndex = 5;
            this.tp退库单.Text = "退库单";
            this.tp退库单.UseVisualStyleBackColor = true;
            // 
            // dgv退库单
            // 
            this.dgv退库单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv退库单.Location = new System.Drawing.Point(33, 62);
            this.dgv退库单.Name = "dgv退库单";
            this.dgv退库单.RowTemplate.Height = 23;
            this.dgv退库单.Size = new System.Drawing.Size(693, 348);
            this.dgv退库单.TabIndex = 4;
            // 
            // btn退库单读取
            // 
            this.btn退库单读取.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn退库单读取.Location = new System.Drawing.Point(645, 16);
            this.btn退库单读取.Name = "btn退库单读取";
            this.btn退库单读取.Size = new System.Drawing.Size(75, 28);
            this.btn退库单读取.TabIndex = 2;
            this.btn退库单读取.Text = "读取";
            this.btn退库单读取.UseVisualStyleBackColor = true;
            this.btn退库单读取.Visible = false;
            this.btn退库单读取.Click += new System.EventHandler(this.btn退库单读取_Click);
            // 
            // tp出库单
            // 
            this.tp出库单.Controls.Add(this.dgv出库单);
            this.tp出库单.Controls.Add(this.btn出库单读取);
            this.tp出库单.Location = new System.Drawing.Point(4, 29);
            this.tp出库单.Name = "tp出库单";
            this.tp出库单.Padding = new System.Windows.Forms.Padding(3);
            this.tp出库单.Size = new System.Drawing.Size(759, 428);
            this.tp出库单.TabIndex = 0;
            this.tp出库单.Text = "出库单";
            this.tp出库单.UseVisualStyleBackColor = true;
            // 
            // dgv出库单
            // 
            this.dgv出库单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv出库单.Location = new System.Drawing.Point(27, 60);
            this.dgv出库单.Name = "dgv出库单";
            this.dgv出库单.RowTemplate.Height = 23;
            this.dgv出库单.Size = new System.Drawing.Size(693, 348);
            this.dgv出库单.TabIndex = 1;
            // 
            // btn出库单读取
            // 
            this.btn出库单读取.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn出库单读取.Location = new System.Drawing.Point(651, 18);
            this.btn出库单读取.Name = "btn出库单读取";
            this.btn出库单读取.Size = new System.Drawing.Size(75, 28);
            this.btn出库单读取.TabIndex = 5;
            this.btn出库单读取.Text = "读取";
            this.btn出库单读取.UseVisualStyleBackColor = true;
            this.btn出库单读取.Click += new System.EventHandler(this.btn出库单读取_Click);
            // 
            // dateTimePicker开始
            // 
            this.dateTimePicker开始.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker开始.Location = new System.Drawing.Point(145, 13);
            this.dateTimePicker开始.Name = "dateTimePicker开始";
            this.dateTimePicker开始.Size = new System.Drawing.Size(240, 26);
            this.dateTimePicker开始.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "开始时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "结束时间";
            // 
            // dateTimePicker结束
            // 
            this.dateTimePicker结束.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker结束.Location = new System.Drawing.Point(145, 45);
            this.dateTimePicker结束.Name = "dateTimePicker结束";
            this.dateTimePicker结束.Size = new System.Drawing.Size(240, 26);
            this.dateTimePicker结束.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(490, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "审核状态";
            // 
            // comboBox审核状态
            // 
            this.comboBox审核状态.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox审核状态.FormattingEnabled = true;
            this.comboBox审核状态.Location = new System.Drawing.Point(595, 15);
            this.comboBox审核状态.Name = "comboBox审核状态";
            this.comboBox审核状态.Size = new System.Drawing.Size(121, 24);
            this.comboBox审核状态.TabIndex = 8;
            // 
            // button查询
            // 
            this.button查询.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button查询.Location = new System.Drawing.Point(641, 57);
            this.button查询.Name = "button查询";
            this.button查询.Size = new System.Drawing.Size(75, 28);
            this.button查询.TabIndex = 3;
            this.button查询.Text = "查询";
            this.button查询.UseVisualStyleBackColor = true;
            this.button查询.Click += new System.EventHandler(this.button查询_Click);
            // 
            // 出库退库单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 569);
            this.Controls.Add(this.button查询);
            this.Controls.Add(this.comboBox审核状态);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker开始);
            this.Controls.Add(this.dateTimePicker结束);
            this.Controls.Add(this.label1);
            this.Name = "出库退库单";
            this.Text = "出库退库单";
            this.tabControl1.ResumeLayout(false);
            this.tp退库单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv退库单)).EndInit();
            this.tp出库单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv出库单)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp出库单;
        private System.Windows.Forms.DataGridView dgv退库单;
        private System.Windows.Forms.Button btn退库单读取;
        private System.Windows.Forms.ComboBox comboBox审核状态;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker结束;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker开始;
        private System.Windows.Forms.Button button查询;
        private System.Windows.Forms.TabPage tp退库单;
        private System.Windows.Forms.DataGridView dgv出库单;
        private System.Windows.Forms.Button btn出库单读取;
    }
}