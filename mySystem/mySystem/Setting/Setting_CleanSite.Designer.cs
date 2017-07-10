namespace mySystem.Setting
{
    partial class Setting_CleanSite
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgv吹膜 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.del吹膜 = new System.Windows.Forms.Button();
            this.add吹膜 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv供料 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.del供料 = new System.Windows.Forms.Button();
            this.add供料 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv吹膜)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv供料)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgv吹膜);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.del吹膜);
            this.groupBox1.Controls.Add(this.add吹膜);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(563, -3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(553, 265);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dgv吹膜
            // 
            this.dgv吹膜.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv吹膜.Location = new System.Drawing.Point(10, 54);
            this.dgv吹膜.Name = "dgv吹膜";
            this.dgv吹膜.RowTemplate.Height = 23;
            this.dgv吹膜.Size = new System.Drawing.Size(536, 204);
            this.dgv吹膜.TabIndex = 7;
            this.dgv吹膜.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv吹膜_DataError);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(263, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "吹膜工序";
            // 
            // del吹膜
            // 
            this.del吹膜.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.del吹膜.Location = new System.Drawing.Point(102, 16);
            this.del吹膜.Margin = new System.Windows.Forms.Padding(4);
            this.del吹膜.Name = "del吹膜";
            this.del吹膜.Size = new System.Drawing.Size(70, 30);
            this.del吹膜.TabIndex = 5;
            this.del吹膜.Text = "删除";
            this.del吹膜.UseVisualStyleBackColor = true;
            this.del吹膜.Click += new System.EventHandler(this.del吹膜_Click);
            // 
            // add吹膜
            // 
            this.add吹膜.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add吹膜.Location = new System.Drawing.Point(10, 16);
            this.add吹膜.Margin = new System.Windows.Forms.Padding(4);
            this.add吹膜.Name = "add吹膜";
            this.add吹膜.Size = new System.Drawing.Size(70, 30);
            this.add吹膜.TabIndex = 4;
            this.add吹膜.Text = "添加";
            this.add吹膜.UseVisualStyleBackColor = true;
            this.add吹膜.Click += new System.EventHandler(this.add吹膜_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgv供料);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.del供料);
            this.groupBox2.Controls.Add(this.add供料);
            this.groupBox2.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, -3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(560, 265);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // dgv供料
            // 
            this.dgv供料.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv供料.Location = new System.Drawing.Point(10, 54);
            this.dgv供料.Name = "dgv供料";
            this.dgv供料.RowTemplate.Height = 23;
            this.dgv供料.Size = new System.Drawing.Size(538, 204);
            this.dgv供料.TabIndex = 7;
            this.dgv供料.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv供料_DataError);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(263, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "供料工序";
            // 
            // del供料
            // 
            this.del供料.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.del供料.Location = new System.Drawing.Point(102, 16);
            this.del供料.Margin = new System.Windows.Forms.Padding(4);
            this.del供料.Name = "del供料";
            this.del供料.Size = new System.Drawing.Size(70, 30);
            this.del供料.TabIndex = 5;
            this.del供料.Text = "删除";
            this.del供料.UseVisualStyleBackColor = true;
            this.del供料.Click += new System.EventHandler(this.del供料_Click);
            // 
            // add供料
            // 
            this.add供料.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add供料.Location = new System.Drawing.Point(10, 16);
            this.add供料.Margin = new System.Windows.Forms.Padding(4);
            this.add供料.Name = "add供料";
            this.add供料.Size = new System.Drawing.Size(70, 30);
            this.add供料.TabIndex = 4;
            this.add供料.Text = "添加";
            this.add供料.UseVisualStyleBackColor = true;
            this.add供料.Click += new System.EventHandler(this.add供料_Click);
            // 
            // Setting_CleanSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 268);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Setting_CleanSite";
            this.Text = "Setting_CleanSite";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv吹膜)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv供料)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button del吹膜;
        private System.Windows.Forms.Button add吹膜;
        private System.Windows.Forms.DataGridView dgv吹膜;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgv供料;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button del供料;
        private System.Windows.Forms.Button add供料;
    }
}