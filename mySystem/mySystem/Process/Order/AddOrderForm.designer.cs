namespace mySystem
{
    partial class AddOrderForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label 商品编号Label;
            System.Windows.Forms.Label 商品名称Label;
            System.Windows.Forms.Label 规格型号Label;
            System.Windows.Forms.Label 生产厂商Label;
            System.Windows.Forms.Label 单位Label;
            System.Windows.Forms.Label 说明Label;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            this.商品编号TextBox = new System.Windows.Forms.TextBox();
            this.商品名称TextBox = new System.Windows.Forms.TextBox();
            this.规格型号TextBox = new System.Windows.Forms.TextBox();
            this.生产厂商TextBox = new System.Windows.Forms.TextBox();
            this.单位TextBox = new System.Windows.Forms.TextBox();
            this.说明TextBox = new System.Windows.Forms.TextBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.工序DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.组件代码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.组件名称DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.厂家DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单组件信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.商品信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            商品编号Label = new System.Windows.Forms.Label();
            商品名称Label = new System.Windows.Forms.Label();
            规格型号Label = new System.Windows.Forms.Label();
            生产厂商Label = new System.Windows.Forms.Label();
            单位Label = new System.Windows.Forms.Label();
            说明Label = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.订单组件信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.商品信息BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // 商品编号Label
            // 
            商品编号Label.AutoSize = true;
            商品编号Label.Location = new System.Drawing.Point(4, 5);
            商品编号Label.Name = "商品编号Label";
            商品编号Label.Size = new System.Drawing.Size(59, 12);
            商品编号Label.TabIndex = 2;
            商品编号Label.Text = "产品编号:";
            // 
            // 商品名称Label
            // 
            商品名称Label.AutoSize = true;
            商品名称Label.Location = new System.Drawing.Point(4, 32);
            商品名称Label.Name = "商品名称Label";
            商品名称Label.Size = new System.Drawing.Size(59, 12);
            商品名称Label.TabIndex = 4;
            商品名称Label.Text = "产品名称:";
            // 
            // 规格型号Label
            // 
            规格型号Label.AutoSize = true;
            规格型号Label.Location = new System.Drawing.Point(4, 59);
            规格型号Label.Name = "规格型号Label";
            规格型号Label.Size = new System.Drawing.Size(59, 12);
            规格型号Label.TabIndex = 6;
            规格型号Label.Text = "规格型号:";
            // 
            // 生产厂商Label
            // 
            生产厂商Label.AutoSize = true;
            生产厂商Label.Location = new System.Drawing.Point(277, 58);
            生产厂商Label.Name = "生产厂商Label";
            生产厂商Label.Size = new System.Drawing.Size(59, 12);
            生产厂商Label.TabIndex = 8;
            生产厂商Label.Text = "生产厂商:";
            // 
            // 单位Label
            // 
            单位Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            单位Label.AutoSize = true;
            单位Label.Location = new System.Drawing.Point(386, 5);
            单位Label.Name = "单位Label";
            单位Label.Size = new System.Drawing.Size(47, 12);
            单位Label.TabIndex = 10;
            单位Label.Text = "订单号:";
            // 
            // 说明Label
            // 
            说明Label.AutoSize = true;
            说明Label.Location = new System.Drawing.Point(4, 349);
            说明Label.Name = "说明Label";
            说明Label.Size = new System.Drawing.Size(35, 12);
            说明Label.TabIndex = 20;
            说明Label.Text = "说明:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 86);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 12);
            label2.TabIndex = 26;
            label2.Text = "预计交货日期:";
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(232, 87);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(47, 12);
            label1.TabIndex = 28;
            label1.Text = "经办人:";
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(386, 32);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(59, 12);
            label3.TabIndex = 30;
            label3.Text = "产品数量:";
            // 
            // 商品编号TextBox
            // 
            this.商品编号TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.商品编号TextBox.Location = new System.Drawing.Point(69, 2);
            this.商品编号TextBox.Name = "商品编号TextBox";
            this.商品编号TextBox.Size = new System.Drawing.Size(307, 21);
            this.商品编号TextBox.TabIndex = 3;
            // 
            // 商品名称TextBox
            // 
            this.商品名称TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.商品名称TextBox.Location = new System.Drawing.Point(69, 29);
            this.商品名称TextBox.Name = "商品名称TextBox";
            this.商品名称TextBox.Size = new System.Drawing.Size(307, 21);
            this.商品名称TextBox.TabIndex = 5;
            // 
            // 规格型号TextBox
            // 
            this.规格型号TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.规格型号TextBox.Location = new System.Drawing.Point(69, 56);
            this.规格型号TextBox.Name = "规格型号TextBox";
            this.规格型号TextBox.Size = new System.Drawing.Size(202, 21);
            this.规格型号TextBox.TabIndex = 7;
            // 
            // 生产厂商TextBox
            // 
            this.生产厂商TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.生产厂商TextBox.Location = new System.Drawing.Point(342, 55);
            this.生产厂商TextBox.Name = "生产厂商TextBox";
            this.生产厂商TextBox.Size = new System.Drawing.Size(194, 21);
            this.生产厂商TextBox.TabIndex = 9;
            // 
            // 单位TextBox
            // 
            this.单位TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.单位TextBox.Location = new System.Drawing.Point(434, 2);
            this.单位TextBox.Name = "单位TextBox";
            this.单位TextBox.ReadOnly = true;
            this.单位TextBox.Size = new System.Drawing.Size(102, 21);
            this.单位TextBox.TabIndex = 11;
            // 
            // 说明TextBox
            // 
            this.说明TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.说明TextBox.Location = new System.Drawing.Point(6, 364);
            this.说明TextBox.Multiline = true;
            this.说明TextBox.Name = "说明TextBox";
            this.说明TextBox.Size = new System.Drawing.Size(530, 66);
            this.说明TextBox.TabIndex = 21;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(368, 439);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 22;
            this.SaveBtn.Text = "确定";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(455, 439);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 23;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(96, 84);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker1.TabIndex = 27;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(285, 84);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(91, 21);
            this.textBox1.TabIndex = 29;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(451, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(85, 21);
            this.textBox2.TabIndex = 31;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.工序DataGridViewTextBoxColumn,
            this.组件代码DataGridViewTextBoxColumn,
            this.组件名称DataGridViewTextBoxColumn,
            this.厂家DataGridViewTextBoxColumn,
            this.数量DataGridViewTextBoxColumn,
            this.单位DataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.订单组件信息BindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(7, 111);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(530, 225);
            this.dataGridView1.TabIndex = 32;
            // 
            // 工序DataGridViewTextBoxColumn
            // 
            this.工序DataGridViewTextBoxColumn.DataPropertyName = "工序";
            this.工序DataGridViewTextBoxColumn.HeaderText = "工序";
            this.工序DataGridViewTextBoxColumn.Name = "工序DataGridViewTextBoxColumn";
            // 
            // 组件代码DataGridViewTextBoxColumn
            // 
            this.组件代码DataGridViewTextBoxColumn.DataPropertyName = "组件代码";
            this.组件代码DataGridViewTextBoxColumn.HeaderText = "组件代码";
            this.组件代码DataGridViewTextBoxColumn.Name = "组件代码DataGridViewTextBoxColumn";
            // 
            // 组件名称DataGridViewTextBoxColumn
            // 
            this.组件名称DataGridViewTextBoxColumn.DataPropertyName = "组件名称";
            this.组件名称DataGridViewTextBoxColumn.HeaderText = "组件名称";
            this.组件名称DataGridViewTextBoxColumn.Name = "组件名称DataGridViewTextBoxColumn";
            // 
            // 厂家DataGridViewTextBoxColumn
            // 
            this.厂家DataGridViewTextBoxColumn.DataPropertyName = "厂家";
            this.厂家DataGridViewTextBoxColumn.HeaderText = "厂家";
            this.厂家DataGridViewTextBoxColumn.Name = "厂家DataGridViewTextBoxColumn";
            // 
            // 数量DataGridViewTextBoxColumn
            // 
            this.数量DataGridViewTextBoxColumn.DataPropertyName = "数量";
            this.数量DataGridViewTextBoxColumn.HeaderText = "数量";
            this.数量DataGridViewTextBoxColumn.Name = "数量DataGridViewTextBoxColumn";
            // 
            // 单位DataGridViewTextBoxColumn
            // 
            this.单位DataGridViewTextBoxColumn.DataPropertyName = "单位";
            this.单位DataGridViewTextBoxColumn.HeaderText = "单位";
            this.单位DataGridViewTextBoxColumn.Name = "单位DataGridViewTextBoxColumn";
            // 
            // 订单组件信息BindingSource
            // 
            this.订单组件信息BindingSource.DataMember = "订单组件信息";
            // 
            // 商品信息BindingSource
            // 
            this.商品信息BindingSource.DataMember = "商品信息";
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 469);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(label2);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(商品编号Label);
            this.Controls.Add(this.商品编号TextBox);
            this.Controls.Add(商品名称Label);
            this.Controls.Add(this.商品名称TextBox);
            this.Controls.Add(规格型号Label);
            this.Controls.Add(this.规格型号TextBox);
            this.Controls.Add(生产厂商Label);
            this.Controls.Add(this.生产厂商TextBox);
            this.Controls.Add(单位Label);
            this.Controls.Add(this.单位TextBox);
            this.Controls.Add(说明Label);
            this.Controls.Add(this.说明TextBox);
            this.Name = "AddOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加订单";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.订单组件信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.商品信息BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private MySaleDataSet mySaleDataSet;
        //private mySystem.MySaleDataSetTableAdapters.商品信息TableAdapter 商品信息TableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.TextBox 商品编号TextBox;
        private System.Windows.Forms.TextBox 商品名称TextBox;
        private System.Windows.Forms.TextBox 规格型号TextBox;
        private System.Windows.Forms.TextBox 生产厂商TextBox;
        private System.Windows.Forms.TextBox 单位TextBox;
        private System.Windows.Forms.TextBox 说明TextBox;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 工序DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 组件代码DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 组件名称DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 厂家DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位DataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource 订单组件信息BindingSource;
        private System.Windows.Forms.BindingSource 商品信息BindingSource;


    }
}