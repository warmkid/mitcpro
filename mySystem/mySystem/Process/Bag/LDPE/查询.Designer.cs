namespace mySystem.Process.Bag.LDPE
{
    partial class 查询
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv总 = new System.Windows.Forms.DataGridView();
            this.生产指令 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品批号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜材用量米 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜材用量平米 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内包用量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外包用量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.生产数量包 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.生产数量平米 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.收率 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv膜 = new System.Windows.Forms.DataGridView();
            this.膜材物料代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜材物料批号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.膜材数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv外 = new System.Windows.Forms.DataGridView();
            this.外包物料代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外包物料批号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外包数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn膜材查询 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb膜材代码 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb产品代码 = new System.Windows.Forms.TextBox();
            this.btn产品查询 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tb膜材批号 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb产品批号 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dgv内 = new System.Windows.Forms.DataGridView();
            this.内包物料代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内包物料批号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内包数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv总)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv膜)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv外)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv内)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(606, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 242;
            this.label1.Text = "查询";
            // 
            // dgv总
            // 
            this.dgv总.AllowUserToAddRows = false;
            this.dgv总.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv总.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.生产指令,
            this.产品代码,
            this.产品批号,
            this.膜材用量米,
            this.膜材用量平米,
            this.内包用量,
            this.外包用量,
            this.生产数量包,
            this.生产数量平米,
            this.收率});
            this.dgv总.Location = new System.Drawing.Point(409, 134);
            this.dgv总.MultiSelect = false;
            this.dgv总.Name = "dgv总";
            this.dgv总.ReadOnly = true;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv总.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv总.RowTemplate.Height = 23;
            this.dgv总.Size = new System.Drawing.Size(858, 489);
            this.dgv总.TabIndex = 248;
            this.dgv总.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv总_CellContentClick);
            this.dgv总.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.writeDGVColumnSettings);
            // 
            // 生产指令
            // 
            this.生产指令.HeaderText = "生产指令";
            this.生产指令.Name = "生产指令";
            this.生产指令.ReadOnly = true;
            // 
            // 产品代码
            // 
            this.产品代码.HeaderText = "产品代码";
            this.产品代码.Name = "产品代码";
            this.产品代码.ReadOnly = true;
            // 
            // 产品批号
            // 
            this.产品批号.HeaderText = "产品批号";
            this.产品批号.Name = "产品批号";
            this.产品批号.ReadOnly = true;
            // 
            // 膜材用量米
            // 
            this.膜材用量米.HeaderText = "膜材用量（米）";
            this.膜材用量米.Name = "膜材用量米";
            this.膜材用量米.ReadOnly = true;
            // 
            // 膜材用量平米
            // 
            this.膜材用量平米.HeaderText = "膜材用量（平米）";
            this.膜材用量平米.Name = "膜材用量平米";
            this.膜材用量平米.ReadOnly = true;
            // 
            // 内包用量
            // 
            this.内包用量.HeaderText = "内包用量";
            this.内包用量.Name = "内包用量";
            this.内包用量.ReadOnly = true;
            // 
            // 外包用量
            // 
            this.外包用量.HeaderText = "外包用量";
            this.外包用量.Name = "外包用量";
            this.外包用量.ReadOnly = true;
            // 
            // 生产数量包
            // 
            this.生产数量包.HeaderText = "生产数量（包）";
            this.生产数量包.Name = "生产数量包";
            this.生产数量包.ReadOnly = true;
            // 
            // 生产数量平米
            // 
            this.生产数量平米.HeaderText = "生产数量（平米）";
            this.生产数量平米.Name = "生产数量平米";
            this.生产数量平米.ReadOnly = true;
            // 
            // 收率
            // 
            this.收率.HeaderText = "收率";
            this.收率.Name = "收率";
            this.收率.ReadOnly = true;
            // 
            // dgv膜
            // 
            this.dgv膜.AllowUserToAddRows = false;
            this.dgv膜.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv膜.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.膜材物料代码,
            this.膜材物料批号,
            this.膜材数量});
            this.dgv膜.Location = new System.Drawing.Point(15, 153);
            this.dgv膜.MultiSelect = false;
            this.dgv膜.Name = "dgv膜";
            this.dgv膜.ReadOnly = true;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv膜.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv膜.RowTemplate.Height = 23;
            this.dgv膜.Size = new System.Drawing.Size(378, 142);
            this.dgv膜.TabIndex = 253;
            this.dgv膜.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv膜_CellContentClick);
            this.dgv膜.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.writeDGVColumnSettings);
            // 
            // 膜材物料代码
            // 
            this.膜材物料代码.HeaderText = "物料代码";
            this.膜材物料代码.Name = "膜材物料代码";
            this.膜材物料代码.ReadOnly = true;
            // 
            // 膜材物料批号
            // 
            this.膜材物料批号.HeaderText = "物料批号";
            this.膜材物料批号.Name = "膜材物料批号";
            this.膜材物料批号.ReadOnly = true;
            // 
            // 膜材数量
            // 
            this.膜材数量.HeaderText = "数量";
            this.膜材数量.Name = "膜材数量";
            this.膜材数量.ReadOnly = true;
            // 
            // dgv外
            // 
            this.dgv外.AllowUserToAddRows = false;
            this.dgv外.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv外.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.外包物料代码,
            this.外包物料批号,
            this.外包数量});
            this.dgv外.Location = new System.Drawing.Point(15, 477);
            this.dgv外.MultiSelect = false;
            this.dgv外.Name = "dgv外";
            this.dgv外.ReadOnly = true;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv外.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv外.RowTemplate.Height = 23;
            this.dgv外.Size = new System.Drawing.Size(378, 146);
            this.dgv外.TabIndex = 254;
            this.dgv外.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv外_CellContentClick);
            this.dgv外.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.writeDGVColumnSettings);
            // 
            // 外包物料代码
            // 
            this.外包物料代码.HeaderText = "物料代码";
            this.外包物料代码.Name = "外包物料代码";
            this.外包物料代码.ReadOnly = true;
            // 
            // 外包物料批号
            // 
            this.外包物料批号.HeaderText = "物料批号";
            this.外包物料批号.Name = "外包物料批号";
            this.外包物料批号.ReadOnly = true;
            // 
            // 外包数量
            // 
            this.外包数量.HeaderText = "数量";
            this.外包数量.Name = "外包数量";
            this.外包数量.ReadOnly = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 256;
            this.label8.Text = "膜材";
            // 
            // dtpStart
            // 
            this.dtpStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpStart.Location = new System.Drawing.Point(90, 39);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(200, 26);
            this.dtpStart.TabIndex = 237;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpEnd.Location = new System.Drawing.Point(90, 88);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(200, 26);
            this.dtpEnd.TabIndex = 238;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 239;
            this.label2.Text = "开始时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 240;
            this.label3.Text = "结束时间";
            // 
            // btn膜材查询
            // 
            this.btn膜材查询.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn膜材查询.Location = new System.Drawing.Point(952, 37);
            this.btn膜材查询.Name = "btn膜材查询";
            this.btn膜材查询.Size = new System.Drawing.Size(271, 26);
            this.btn膜材查询.TabIndex = 241;
            this.btn膜材查询.Text = "膜材查询";
            this.btn膜材查询.UseVisualStyleBackColor = true;
            this.btn膜材查询.Click += new System.EventHandler(this.btn膜材查询_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(331, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 243;
            this.label4.Text = "膜材代码";
            // 
            // tb膜材代码
            // 
            this.tb膜材代码.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb膜材代码.Location = new System.Drawing.Point(409, 39);
            this.tb膜材代码.Name = "tb膜材代码";
            this.tb膜材代码.Size = new System.Drawing.Size(200, 26);
            this.tb膜材代码.TabIndex = 244;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(331, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 245;
            this.label5.Text = "产品代码";
            // 
            // tb产品代码
            // 
            this.tb产品代码.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb产品代码.Location = new System.Drawing.Point(409, 88);
            this.tb产品代码.Name = "tb产品代码";
            this.tb产品代码.Size = new System.Drawing.Size(200, 26);
            this.tb产品代码.TabIndex = 246;
            // 
            // btn产品查询
            // 
            this.btn产品查询.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn产品查询.Location = new System.Drawing.Point(954, 90);
            this.btn产品查询.Name = "btn产品查询";
            this.btn产品查询.Size = new System.Drawing.Size(269, 26);
            this.btn产品查询.TabIndex = 247;
            this.btn产品查询.Text = "产品查询";
            this.btn产品查询.UseVisualStyleBackColor = true;
            this.btn产品查询.Click += new System.EventHandler(this.btn产品查询_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(640, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 249;
            this.label6.Text = "膜材批号";
            // 
            // tb膜材批号
            // 
            this.tb膜材批号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb膜材批号.Location = new System.Drawing.Point(722, 39);
            this.tb膜材批号.Name = "tb膜材批号";
            this.tb膜材批号.Size = new System.Drawing.Size(200, 26);
            this.tb膜材批号.TabIndex = 250;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(640, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 251;
            this.label7.Text = "产品批号";
            // 
            // tb产品批号
            // 
            this.tb产品批号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb产品批号.Location = new System.Drawing.Point(722, 88);
            this.tb产品批号.Name = "tb产品批号";
            this.tb产品批号.Size = new System.Drawing.Size(200, 26);
            this.tb产品批号.TabIndex = 252;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(12, 458);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 258;
            this.label10.Text = "外包";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(12, 308);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 260;
            this.label9.Text = "内包";
            // 
            // dgv内
            // 
            this.dgv内.AllowUserToAddRows = false;
            this.dgv内.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv内.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.内包物料代码,
            this.内包物料批号,
            this.内包数量});
            this.dgv内.Location = new System.Drawing.Point(15, 327);
            this.dgv内.MultiSelect = false;
            this.dgv内.Name = "dgv内";
            this.dgv内.ReadOnly = true;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv内.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv内.RowTemplate.Height = 23;
            this.dgv内.Size = new System.Drawing.Size(378, 117);
            this.dgv内.TabIndex = 259;
            this.dgv内.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv内_CellContentClick);
            this.dgv内.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.writeDGVColumnSettings);
            // 
            // 内包物料代码
            // 
            this.内包物料代码.HeaderText = "物料代码";
            this.内包物料代码.Name = "内包物料代码";
            this.内包物料代码.ReadOnly = true;
            // 
            // 内包物料批号
            // 
            this.内包物料批号.HeaderText = "物料批号";
            this.内包物料批号.Name = "内包物料批号";
            this.内包物料批号.ReadOnly = true;
            // 
            // 内包数量
            // 
            this.内包数量.HeaderText = "数量";
            this.内包数量.Name = "内包数量";
            this.内包数量.ReadOnly = true;
            // 
            // 查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 635);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dgv内);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgv外);
            this.Controls.Add(this.dgv膜);
            this.Controls.Add(this.tb产品批号);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb膜材批号);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dgv总);
            this.Controls.Add(this.btn产品查询);
            this.Controls.Add(this.tb产品代码);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb膜材代码);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn膜材查询);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Name = "查询";
            this.Text = "查询";
            ((System.ComponentModel.ISupportInitialize)(this.dgv总)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv膜)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv外)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv内)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv总;
        private System.Windows.Forms.DataGridView dgv膜;
        private System.Windows.Forms.DataGridView dgv外;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn膜材查询;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb膜材代码;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb产品代码;
        private System.Windows.Forms.Button btn产品查询;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb膜材批号;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb产品批号;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产指令;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品批号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜材用量米;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜材用量平米;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包用量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 外包用量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产数量包;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产数量平米;
        private System.Windows.Forms.DataGridViewTextBoxColumn 收率;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜材物料代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜材物料批号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 膜材数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 外包物料代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 外包物料批号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 外包数量;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgv内;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包物料代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包物料批号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包数量;
    }
}