namespace mySystem.Other
{
    partial class 属于工序
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.benDone = new System.Windows.Forms.Button();
            this.选择 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.工序 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选择,
            this.工序});
            this.dataGridView1.Location = new System.Drawing.Point(21, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(246, 211);
            this.dataGridView1.TabIndex = 0;
            // 
            // benDone
            // 
            this.benDone.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.benDone.Location = new System.Drawing.Point(117, 238);
            this.benDone.Name = "benDone";
            this.benDone.Size = new System.Drawing.Size(75, 23);
            this.benDone.TabIndex = 1;
            this.benDone.Text = "完成";
            this.benDone.UseVisualStyleBackColor = true;
            this.benDone.Click += new System.EventHandler(this.benDone_Click);
            // 
            // 选择
            // 
            this.选择.HeaderText = "选择";
            this.选择.Name = "选择";
            this.选择.Width = 70;
            // 
            // 工序
            // 
            this.工序.HeaderText = "工序";
            this.工序.Name = "工序";
            this.工序.Width = 170;
            // 
            // 属于工序
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.benDone);
            this.Controls.Add(this.dataGridView1);
            this.Name = "属于工序";
            this.Text = "属于工序";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button benDone;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选择;
        private System.Windows.Forms.DataGridViewTextBoxColumn 工序;
    }
}