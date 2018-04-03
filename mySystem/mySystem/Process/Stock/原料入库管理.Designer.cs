namespace mySystem.Process.Stock
{
    partial class 原料入库管理
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
            this.tp到货单 = new System.Windows.Forms.TabPage();
            this.dgv到货单 = new System.Windows.Forms.DataGridView();
            this.btn到货单增加记录 = new System.Windows.Forms.Button();
            this.btn到货单读取 = new System.Windows.Forms.Button();
            this.tp物资验收记录 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn增加物资验收记录 = new System.Windows.Forms.Button();
            this.btn读取验收记录 = new System.Windows.Forms.Button();
            this.tp入库单 = new System.Windows.Forms.TabPage();
            this.dgv入库单 = new System.Windows.Forms.DataGridView();
            this.btn读取入库单 = new System.Windows.Forms.Button();
            this.tp物资请验单 = new System.Windows.Forms.TabPage();
            this.btn读取物资请验单 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tp取样记录 = new System.Windows.Forms.TabPage();
            this.btn读取取样记录 = new System.Windows.Forms.Button();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.tp检验记录 = new System.Windows.Forms.TabPage();
            this.dgv检验记录 = new System.Windows.Forms.DataGridView();
            this.btn读取检验记录 = new System.Windows.Forms.Button();
            this.tp复验记录 = new System.Windows.Forms.TabPage();
            this.btn读取复验记录 = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tp不合格品处理记录 = new System.Windows.Forms.TabPage();
            this.btn读取不合格品记录 = new System.Windows.Forms.Button();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.dateTimePicker开始 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker结束 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox审核状态 = new System.Windows.Forms.ComboBox();
            this.button查询 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tp到货单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv到货单)).BeginInit();
            this.tp物资验收记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tp入库单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv入库单)).BeginInit();
            this.tp物资请验单.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tp取样记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.tp检验记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv检验记录)).BeginInit();
            this.tp复验记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tp不合格品处理记录.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tp到货单);
            this.tabControl1.Controls.Add(this.tp物资验收记录);
            this.tabControl1.Controls.Add(this.tp入库单);
            this.tabControl1.Controls.Add(this.tp物资请验单);
            this.tabControl1.Controls.Add(this.tp取样记录);
            this.tabControl1.Controls.Add(this.tp检验记录);
            this.tabControl1.Controls.Add(this.tp复验记录);
            this.tabControl1.Controls.Add(this.tp不合格品处理记录);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(19, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(767, 461);
            this.tabControl1.TabIndex = 0;
            // 
            // tp到货单
            // 
            this.tp到货单.Controls.Add(this.dgv到货单);
            this.tp到货单.Controls.Add(this.btn到货单增加记录);
            this.tp到货单.Controls.Add(this.btn到货单读取);
            this.tp到货单.Location = new System.Drawing.Point(4, 29);
            this.tp到货单.Name = "tp到货单";
            this.tp到货单.Padding = new System.Windows.Forms.Padding(3);
            this.tp到货单.Size = new System.Drawing.Size(759, 428);
            this.tp到货单.TabIndex = 5;
            this.tp到货单.Text = "到货单";
            this.tp到货单.UseVisualStyleBackColor = true;
            // 
            // dgv到货单
            // 
            this.dgv到货单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv到货单.Location = new System.Drawing.Point(33, 62);
            this.dgv到货单.Name = "dgv到货单";
            this.dgv到货单.RowTemplate.Height = 23;
            this.dgv到货单.Size = new System.Drawing.Size(693, 348);
            this.dgv到货单.TabIndex = 4;
            // 
            // btn到货单增加记录
            // 
            this.btn到货单增加记录.Location = new System.Drawing.Point(33, 18);
            this.btn到货单增加记录.Name = "btn到货单增加记录";
            this.btn到货单增加记录.Size = new System.Drawing.Size(150, 28);
            this.btn到货单增加记录.TabIndex = 3;
            this.btn到货单增加记录.Text = "增加记录";
            this.btn到货单增加记录.UseVisualStyleBackColor = true;
            this.btn到货单增加记录.Click += new System.EventHandler(this.btn到货单增加记录_Click);
            // 
            // btn到货单读取
            // 
            this.btn到货单读取.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn到货单读取.Location = new System.Drawing.Point(651, 18);
            this.btn到货单读取.Name = "btn到货单读取";
            this.btn到货单读取.Size = new System.Drawing.Size(75, 28);
            this.btn到货单读取.TabIndex = 5;
            this.btn到货单读取.Text = "读取";
            this.btn到货单读取.UseVisualStyleBackColor = true;
            this.btn到货单读取.Visible = false;
            this.btn到货单读取.Click += new System.EventHandler(this.btn到货单读取_Click);
            // 
            // tp物资验收记录
            // 
            this.tp物资验收记录.Controls.Add(this.dataGridView1);
            this.tp物资验收记录.Controls.Add(this.btn增加物资验收记录);
            this.tp物资验收记录.Controls.Add(this.btn读取验收记录);
            this.tp物资验收记录.Location = new System.Drawing.Point(4, 29);
            this.tp物资验收记录.Name = "tp物资验收记录";
            this.tp物资验收记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp物资验收记录.Size = new System.Drawing.Size(759, 428);
            this.tp物资验收记录.TabIndex = 0;
            this.tp物资验收记录.Text = "物资验收记录";
            this.tp物资验收记录.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(27, 60);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(693, 348);
            this.dataGridView1.TabIndex = 1;
            // 
            // btn增加物资验收记录
            // 
            this.btn增加物资验收记录.Location = new System.Drawing.Point(27, 16);
            this.btn增加物资验收记录.Name = "btn增加物资验收记录";
            this.btn增加物资验收记录.Size = new System.Drawing.Size(150, 28);
            this.btn增加物资验收记录.TabIndex = 0;
            this.btn增加物资验收记录.Text = "增加记录";
            this.btn增加物资验收记录.UseVisualStyleBackColor = true;
            this.btn增加物资验收记录.Visible = false;
            this.btn增加物资验收记录.Click += new System.EventHandler(this.btn增加物资验收记录_Click);
            // 
            // btn读取验收记录
            // 
            this.btn读取验收记录.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn读取验收记录.Location = new System.Drawing.Point(645, 16);
            this.btn读取验收记录.Name = "btn读取验收记录";
            this.btn读取验收记录.Size = new System.Drawing.Size(75, 28);
            this.btn读取验收记录.TabIndex = 2;
            this.btn读取验收记录.Text = "读取";
            this.btn读取验收记录.UseVisualStyleBackColor = true;
            this.btn读取验收记录.Click += new System.EventHandler(this.btn读取验收记录_Click);
            // 
            // tp入库单
            // 
            this.tp入库单.Controls.Add(this.dgv入库单);
            this.tp入库单.Controls.Add(this.btn读取入库单);
            this.tp入库单.Location = new System.Drawing.Point(4, 29);
            this.tp入库单.Name = "tp入库单";
            this.tp入库单.Padding = new System.Windows.Forms.Padding(3);
            this.tp入库单.Size = new System.Drawing.Size(759, 428);
            this.tp入库单.TabIndex = 6;
            this.tp入库单.Text = "入库单";
            this.tp入库单.UseVisualStyleBackColor = true;
            // 
            // dgv入库单
            // 
            this.dgv入库单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv入库单.Location = new System.Drawing.Point(33, 62);
            this.dgv入库单.Name = "dgv入库单";
            this.dgv入库单.RowTemplate.Height = 23;
            this.dgv入库单.Size = new System.Drawing.Size(693, 348);
            this.dgv入库单.TabIndex = 3;
            // 
            // btn读取入库单
            // 
            this.btn读取入库单.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn读取入库单.Location = new System.Drawing.Point(651, 18);
            this.btn读取入库单.Name = "btn读取入库单";
            this.btn读取入库单.Size = new System.Drawing.Size(75, 28);
            this.btn读取入库单.TabIndex = 4;
            this.btn读取入库单.Text = "读取";
            this.btn读取入库单.UseVisualStyleBackColor = true;
            this.btn读取入库单.Click += new System.EventHandler(this.btn读取入库单_Click);
            // 
            // tp物资请验单
            // 
            this.tp物资请验单.Controls.Add(this.btn读取物资请验单);
            this.tp物资请验单.Controls.Add(this.dataGridView2);
            this.tp物资请验单.Location = new System.Drawing.Point(4, 29);
            this.tp物资请验单.Name = "tp物资请验单";
            this.tp物资请验单.Padding = new System.Windows.Forms.Padding(3);
            this.tp物资请验单.Size = new System.Drawing.Size(759, 428);
            this.tp物资请验单.TabIndex = 1;
            this.tp物资请验单.Text = "物资请验单";
            this.tp物资请验单.UseVisualStyleBackColor = true;
            // 
            // btn读取物资请验单
            // 
            this.btn读取物资请验单.Location = new System.Drawing.Point(646, 23);
            this.btn读取物资请验单.Name = "btn读取物资请验单";
            this.btn读取物资请验单.Size = new System.Drawing.Size(75, 23);
            this.btn读取物资请验单.TabIndex = 1;
            this.btn读取物资请验单.Text = "读取";
            this.btn读取物资请验单.UseVisualStyleBackColor = true;
            this.btn读取物资请验单.Click += new System.EventHandler(this.btn读取_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(19, 64);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(702, 342);
            this.dataGridView2.TabIndex = 0;
            // 
            // tp取样记录
            // 
            this.tp取样记录.Controls.Add(this.btn读取取样记录);
            this.tp取样记录.Controls.Add(this.dataGridView5);
            this.tp取样记录.Location = new System.Drawing.Point(4, 29);
            this.tp取样记录.Name = "tp取样记录";
            this.tp取样记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp取样记录.Size = new System.Drawing.Size(759, 428);
            this.tp取样记录.TabIndex = 4;
            this.tp取样记录.Text = "取样记录";
            this.tp取样记录.UseVisualStyleBackColor = true;
            // 
            // btn读取取样记录
            // 
            this.btn读取取样记录.Location = new System.Drawing.Point(646, 20);
            this.btn读取取样记录.Name = "btn读取取样记录";
            this.btn读取取样记录.Size = new System.Drawing.Size(75, 23);
            this.btn读取取样记录.TabIndex = 3;
            this.btn读取取样记录.Text = "读取";
            this.btn读取取样记录.UseVisualStyleBackColor = true;
            this.btn读取取样记录.Click += new System.EventHandler(this.btn读取取样记录_Click);
            // 
            // dataGridView5
            // 
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Location = new System.Drawing.Point(19, 58);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.RowTemplate.Height = 23;
            this.dataGridView5.Size = new System.Drawing.Size(702, 364);
            this.dataGridView5.TabIndex = 2;
            // 
            // tp检验记录
            // 
            this.tp检验记录.Controls.Add(this.dgv检验记录);
            this.tp检验记录.Controls.Add(this.btn读取检验记录);
            this.tp检验记录.Location = new System.Drawing.Point(4, 29);
            this.tp检验记录.Name = "tp检验记录";
            this.tp检验记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp检验记录.Size = new System.Drawing.Size(759, 428);
            this.tp检验记录.TabIndex = 7;
            this.tp检验记录.Text = "检验记录";
            this.tp检验记录.UseVisualStyleBackColor = true;
            // 
            // dgv检验记录
            // 
            this.dgv检验记录.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv检验记录.Location = new System.Drawing.Point(33, 62);
            this.dgv检验记录.Name = "dgv检验记录";
            this.dgv检验记录.RowTemplate.Height = 23;
            this.dgv检验记录.Size = new System.Drawing.Size(693, 348);
            this.dgv检验记录.TabIndex = 5;
            // 
            // btn读取检验记录
            // 
            this.btn读取检验记录.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn读取检验记录.Location = new System.Drawing.Point(651, 18);
            this.btn读取检验记录.Name = "btn读取检验记录";
            this.btn读取检验记录.Size = new System.Drawing.Size(75, 28);
            this.btn读取检验记录.TabIndex = 6;
            this.btn读取检验记录.Text = "读取";
            this.btn读取检验记录.UseVisualStyleBackColor = true;
            this.btn读取检验记录.Click += new System.EventHandler(this.btn读取检验记录_Click_1);
            // 
            // tp复验记录
            // 
            this.tp复验记录.Controls.Add(this.btn读取复验记录);
            this.tp复验记录.Controls.Add(this.dataGridView3);
            this.tp复验记录.Location = new System.Drawing.Point(4, 29);
            this.tp复验记录.Name = "tp复验记录";
            this.tp复验记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp复验记录.Size = new System.Drawing.Size(759, 428);
            this.tp复验记录.TabIndex = 2;
            this.tp复验记录.Text = "复验记录";
            this.tp复验记录.UseVisualStyleBackColor = true;
            // 
            // btn读取复验记录
            // 
            this.btn读取复验记录.Location = new System.Drawing.Point(649, 19);
            this.btn读取复验记录.Name = "btn读取复验记录";
            this.btn读取复验记录.Size = new System.Drawing.Size(75, 23);
            this.btn读取复验记录.TabIndex = 3;
            this.btn读取复验记录.Text = "读取";
            this.btn读取复验记录.UseVisualStyleBackColor = true;
            this.btn读取复验记录.Click += new System.EventHandler(this.btn读取检验记录_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(18, 59);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(706, 351);
            this.dataGridView3.TabIndex = 2;
            // 
            // tp不合格品处理记录
            // 
            this.tp不合格品处理记录.Controls.Add(this.btn读取不合格品记录);
            this.tp不合格品处理记录.Controls.Add(this.dataGridView4);
            this.tp不合格品处理记录.Location = new System.Drawing.Point(4, 29);
            this.tp不合格品处理记录.Name = "tp不合格品处理记录";
            this.tp不合格品处理记录.Padding = new System.Windows.Forms.Padding(3);
            this.tp不合格品处理记录.Size = new System.Drawing.Size(759, 428);
            this.tp不合格品处理记录.TabIndex = 3;
            this.tp不合格品处理记录.Text = "不合格品处理记录";
            this.tp不合格品处理记录.UseVisualStyleBackColor = true;
            // 
            // btn读取不合格品记录
            // 
            this.btn读取不合格品记录.Location = new System.Drawing.Point(651, 27);
            this.btn读取不合格品记录.Name = "btn读取不合格品记录";
            this.btn读取不合格品记录.Size = new System.Drawing.Size(75, 23);
            this.btn读取不合格品记录.TabIndex = 1;
            this.btn读取不合格品记录.Text = "读取";
            this.btn读取不合格品记录.UseVisualStyleBackColor = true;
            this.btn读取不合格品记录.Click += new System.EventHandler(this.btn读取不合格品记录_Click);
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(17, 67);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowTemplate.Height = 23;
            this.dataGridView4.Size = new System.Drawing.Size(709, 345);
            this.dataGridView4.TabIndex = 0;
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
            // 原料入库管理
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
            this.Name = "原料入库管理";
            this.Text = "原料入库管理";
            this.tabControl1.ResumeLayout(false);
            this.tp到货单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv到货单)).EndInit();
            this.tp物资验收记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tp入库单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv入库单)).EndInit();
            this.tp物资请验单.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tp取样记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.tp检验记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv检验记录)).EndInit();
            this.tp复验记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tp不合格品处理记录.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp物资验收记录;
        private System.Windows.Forms.TabPage tp物资请验单;
        private System.Windows.Forms.TabPage tp复验记录;
        private System.Windows.Forms.TabPage tp不合格品处理记录;
        private System.Windows.Forms.Button btn增加物资验收记录;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn读取物资请验单;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btn读取复验记录;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button btn读取不合格品记录;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Button btn读取验收记录;
        private System.Windows.Forms.TabPage tp取样记录;
        private System.Windows.Forms.Button btn读取取样记录;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.ComboBox comboBox审核状态;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker结束;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker开始;
        private System.Windows.Forms.Button button查询;
        private System.Windows.Forms.TabPage tp到货单;
        private System.Windows.Forms.DataGridView dgv到货单;
        private System.Windows.Forms.Button btn到货单增加记录;
        private System.Windows.Forms.Button btn到货单读取;
        private System.Windows.Forms.TabPage tp入库单;
        private System.Windows.Forms.DataGridView dgv入库单;
        private System.Windows.Forms.Button btn读取入库单;
        private System.Windows.Forms.TabPage tp检验记录;
        private System.Windows.Forms.DataGridView dgv检验记录;
        private System.Windows.Forms.Button btn读取检验记录;
    }
}