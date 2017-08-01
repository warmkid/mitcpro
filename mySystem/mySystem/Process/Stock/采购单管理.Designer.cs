namespace 订单和库存管理
{
    partial class 采购单管理
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
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn采购需求单 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(12, 21);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 0;
            this.btn添加.Text = "添加采购单";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 65);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(444, 150);
            this.dataGridView1.TabIndex = 1;
            // 
            // btn采购需求单
            // 
            this.btn采购需求单.Location = new System.Drawing.Point(146, 21);
            this.btn采购需求单.Name = "btn采购需求单";
            this.btn采购需求单.Size = new System.Drawing.Size(75, 23);
            this.btn采购需求单.TabIndex = 2;
            this.btn采购需求单.Text = "采购需求单";
            this.btn采购需求单.UseVisualStyleBackColor = true;
            this.btn采购需求单.Click += new System.EventHandler(this.btn采购需求单_Click);
            // 
            // 采购单管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 238);
            this.Controls.Add(this.btn采购需求单);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn添加);
            this.Name = "采购单管理";
            this.Text = "采购单管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn采购需求单;
    }
}