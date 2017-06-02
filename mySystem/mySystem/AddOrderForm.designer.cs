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
            System.Windows.Forms.Label 累计采购量Label;
            System.Windows.Forms.Label 累计销售量Label;
            System.Windows.Forms.Label 建议采购价Label;
            System.Windows.Forms.Label 建议销售价Label;
            System.Windows.Forms.Label 说明Label;
            this.商品信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.商品编号TextBox = new System.Windows.Forms.TextBox();
            this.商品名称TextBox = new System.Windows.Forms.TextBox();
            this.规格型号TextBox = new System.Windows.Forms.TextBox();
            this.生产厂商TextBox = new System.Windows.Forms.TextBox();
            this.单位TextBox = new System.Windows.Forms.TextBox();
            this.累计采购量TextBox = new System.Windows.Forms.TextBox();
            this.累计销售量TextBox = new System.Windows.Forms.TextBox();
            this.建议采购价TextBox = new System.Windows.Forms.TextBox();
            this.建议销售价TextBox = new System.Windows.Forms.TextBox();
            this.说明TextBox = new System.Windows.Forms.TextBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            商品编号Label = new System.Windows.Forms.Label();
            商品名称Label = new System.Windows.Forms.Label();
            规格型号Label = new System.Windows.Forms.Label();
            生产厂商Label = new System.Windows.Forms.Label();
            单位Label = new System.Windows.Forms.Label();
            累计采购量Label = new System.Windows.Forms.Label();
            累计销售量Label = new System.Windows.Forms.Label();
            建议采购价Label = new System.Windows.Forms.Label();
            建议销售价Label = new System.Windows.Forms.Label();
            说明Label = new System.Windows.Forms.Label();
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
            商品编号Label.Text = "商品编号:";
            // 
            // 商品名称Label
            // 
            商品名称Label.AutoSize = true;
            商品名称Label.Location = new System.Drawing.Point(4, 32);
            商品名称Label.Name = "商品名称Label";
            商品名称Label.Size = new System.Drawing.Size(59, 12);
            商品名称Label.TabIndex = 4;
            商品名称Label.Text = "商品名称:";
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
            生产厂商Label.Location = new System.Drawing.Point(4, 86);
            生产厂商Label.Name = "生产厂商Label";
            生产厂商Label.Size = new System.Drawing.Size(59, 12);
            生产厂商Label.TabIndex = 8;
            生产厂商Label.Text = "生产厂商:";
            // 
            // 单位Label
            // 
            单位Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            单位Label.AutoSize = true;
            单位Label.Location = new System.Drawing.Point(397, 5);
            单位Label.Name = "单位Label";
            单位Label.Size = new System.Drawing.Size(35, 12);
            单位Label.TabIndex = 10;
            单位Label.Text = "单位:";
            // 
            // 累计采购量Label
            // 
            累计采购量Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            累计采购量Label.AutoSize = true;
            累计采购量Label.Location = new System.Drawing.Point(397, 32);
            累计采购量Label.Name = "累计采购量Label";
            累计采购量Label.Size = new System.Drawing.Size(71, 12);
            累计采购量Label.TabIndex = 12;
            累计采购量Label.Text = "累计采购量:";
            // 
            // 累计销售量Label
            // 
            累计销售量Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            累计销售量Label.AutoSize = true;
            累计销售量Label.Location = new System.Drawing.Point(397, 59);
            累计销售量Label.Name = "累计销售量Label";
            累计销售量Label.Size = new System.Drawing.Size(71, 12);
            累计销售量Label.TabIndex = 14;
            累计销售量Label.Text = "累计销售量:";
            // 
            // 建议采购价Label
            // 
            建议采购价Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            建议采购价Label.AutoSize = true;
            建议采购价Label.Location = new System.Drawing.Point(397, 86);
            建议采购价Label.Name = "建议采购价Label";
            建议采购价Label.Size = new System.Drawing.Size(71, 12);
            建议采购价Label.TabIndex = 16;
            建议采购价Label.Text = "建议采购价:";
            // 
            // 建议销售价Label
            // 
            建议销售价Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            建议销售价Label.AutoSize = true;
            建议销售价Label.Location = new System.Drawing.Point(397, 113);
            建议销售价Label.Name = "建议销售价Label";
            建议销售价Label.Size = new System.Drawing.Size(71, 12);
            建议销售价Label.TabIndex = 18;
            建议销售价Label.Text = "建议销售价:";
            // 
            // 说明Label
            // 
            说明Label.AutoSize = true;
            说明Label.Location = new System.Drawing.Point(4, 113);
            说明Label.Name = "说明Label";
            说明Label.Size = new System.Drawing.Size(35, 12);
            说明Label.TabIndex = 20;
            说明Label.Text = "说明:";
            // 
            // 商品信息BindingSource
            // 
            this.商品信息BindingSource.DataMember = "商品信息";
            // 
            // 商品编号TextBox
            // 
            this.商品编号TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.商品编号TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "商品编号", true));
            this.商品编号TextBox.Location = new System.Drawing.Point(69, 2);
            this.商品编号TextBox.Name = "商品编号TextBox";
            this.商品编号TextBox.Size = new System.Drawing.Size(320, 21);
            this.商品编号TextBox.TabIndex = 3;
            // 
            // 商品名称TextBox
            // 
            this.商品名称TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.商品名称TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "商品名称", true));
            this.商品名称TextBox.Location = new System.Drawing.Point(69, 29);
            this.商品名称TextBox.Name = "商品名称TextBox";
            this.商品名称TextBox.Size = new System.Drawing.Size(320, 21);
            this.商品名称TextBox.TabIndex = 5;
            // 
            // 规格型号TextBox
            // 
            this.规格型号TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.规格型号TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "规格型号", true));
            this.规格型号TextBox.Location = new System.Drawing.Point(69, 56);
            this.规格型号TextBox.Name = "规格型号TextBox";
            this.规格型号TextBox.Size = new System.Drawing.Size(320, 21);
            this.规格型号TextBox.TabIndex = 7;
            // 
            // 生产厂商TextBox
            // 
            this.生产厂商TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.生产厂商TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "生产厂商", true));
            this.生产厂商TextBox.Location = new System.Drawing.Point(69, 83);
            this.生产厂商TextBox.Name = "生产厂商TextBox";
            this.生产厂商TextBox.Size = new System.Drawing.Size(320, 21);
            this.生产厂商TextBox.TabIndex = 9;
            // 
            // 单位TextBox
            // 
            this.单位TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.单位TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "单位", true));
            this.单位TextBox.Location = new System.Drawing.Point(474, 2);
            this.单位TextBox.Name = "单位TextBox";
            this.单位TextBox.Size = new System.Drawing.Size(62, 21);
            this.单位TextBox.TabIndex = 11;
            // 
            // 累计采购量TextBox
            // 
            this.累计采购量TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.累计采购量TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "累计采购量", true));
            this.累计采购量TextBox.Location = new System.Drawing.Point(474, 29);
            this.累计采购量TextBox.Name = "累计采购量TextBox";
            this.累计采购量TextBox.ReadOnly = true;
            this.累计采购量TextBox.Size = new System.Drawing.Size(62, 21);
            this.累计采购量TextBox.TabIndex = 13;
            // 
            // 累计销售量TextBox
            // 
            this.累计销售量TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.累计销售量TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "累计销售量", true));
            this.累计销售量TextBox.Location = new System.Drawing.Point(474, 56);
            this.累计销售量TextBox.Name = "累计销售量TextBox";
            this.累计销售量TextBox.ReadOnly = true;
            this.累计销售量TextBox.Size = new System.Drawing.Size(62, 21);
            this.累计销售量TextBox.TabIndex = 15;
            // 
            // 建议采购价TextBox
            // 
            this.建议采购价TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.建议采购价TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "建议采购价", true));
            this.建议采购价TextBox.Location = new System.Drawing.Point(474, 83);
            this.建议采购价TextBox.Name = "建议采购价TextBox";
            this.建议采购价TextBox.Size = new System.Drawing.Size(62, 21);
            this.建议采购价TextBox.TabIndex = 17;
            // 
            // 建议销售价TextBox
            // 
            this.建议销售价TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.建议销售价TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "建议销售价", true));
            this.建议销售价TextBox.Location = new System.Drawing.Point(474, 110);
            this.建议销售价TextBox.Name = "建议销售价TextBox";
            this.建议销售价TextBox.Size = new System.Drawing.Size(62, 21);
            this.建议销售价TextBox.TabIndex = 19;
            // 
            // 说明TextBox
            // 
            this.说明TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.说明TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.商品信息BindingSource, "说明", true));
            this.说明TextBox.Location = new System.Drawing.Point(69, 110);
            this.说明TextBox.Name = "说明TextBox";
            this.说明TextBox.Size = new System.Drawing.Size(320, 21);
            this.说明TextBox.TabIndex = 21;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(368, 392);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 22;
            this.SaveBtn.Text = "确定";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(455, 392);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 23;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 416);
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
            this.Controls.Add(累计采购量Label);
            this.Controls.Add(this.累计采购量TextBox);
            this.Controls.Add(累计销售量Label);
            this.Controls.Add(this.累计销售量TextBox);
            this.Controls.Add(建议采购价Label);
            this.Controls.Add(this.建议采购价TextBox);
            this.Controls.Add(建议销售价Label);
            this.Controls.Add(this.建议销售价TextBox);
            this.Controls.Add(说明Label);
            this.Controls.Add(this.说明TextBox);
            this.Name = "AddOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加订单";
            this.Load += new System.EventHandler(this.MerchandiseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.商品信息BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private MySaleDataSet mySaleDataSet;
        private System.Windows.Forms.BindingSource 商品信息BindingSource;
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
        private System.Windows.Forms.TextBox 累计采购量TextBox;
        private System.Windows.Forms.TextBox 累计销售量TextBox;
        private System.Windows.Forms.TextBox 建议采购价TextBox;
        private System.Windows.Forms.TextBox 建议销售价TextBox;
        private System.Windows.Forms.TextBox 说明TextBox;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancelBtn;


    }
}