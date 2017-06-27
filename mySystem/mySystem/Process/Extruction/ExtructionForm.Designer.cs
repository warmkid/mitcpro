namespace mySystem
{
    partial class ExtructionForm
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
            this.Chart2Btn = new System.Windows.Forms.Button();
            this.Chart4Btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Chart5Btn = new System.Windows.Forms.Button();
            this.Chart3Btn = new System.Windows.Forms.Button();
            this.Chart7Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Chart2Btn
            // 
            this.Chart2Btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Chart2Btn.Location = new System.Drawing.Point(14, 12);
            this.Chart2Btn.Name = "Chart2Btn";
            this.Chart2Btn.Size = new System.Drawing.Size(185, 38);
            this.Chart2Btn.TabIndex = 3;
            this.Chart2Btn.Text = "吹膜供料系统运行记录";
            this.Chart2Btn.UseVisualStyleBackColor = true;
            this.Chart2Btn.Click += new System.EventHandler(this.Chart2Btn_Click);
            // 
            // Chart4Btn
            // 
            this.Chart4Btn.Font = new System.Drawing.Font("宋体", 12F);
            this.Chart4Btn.Location = new System.Drawing.Point(425, 12);
            this.Chart4Btn.Name = "Chart4Btn";
            this.Chart4Btn.Size = new System.Drawing.Size(155, 38);
            this.Chart4Btn.TabIndex = 2;
            this.Chart4Btn.Text = "吹膜工序废品记录";
            this.Chart4Btn.UseVisualStyleBackColor = true;
            this.Chart4Btn.Click += new System.EventHandler(this.Chart4Btn_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(2, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1168, 520);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "生产计划：";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(93, 57);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(257, 27);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Chart5Btn
            // 
            this.Chart5Btn.Font = new System.Drawing.Font("宋体", 12F);
            this.Chart5Btn.Location = new System.Drawing.Point(618, 12);
            this.Chart5Btn.Name = "Chart5Btn";
            this.Chart5Btn.Size = new System.Drawing.Size(174, 38);
            this.Chart5Btn.TabIndex = 7;
            this.Chart5Btn.Text = "吹膜工序清场记录";
            this.Chart5Btn.UseVisualStyleBackColor = true;
            this.Chart5Btn.Click += new System.EventHandler(this.Chart5Btn_Click);
            // 
            // Chart3Btn
            // 
            this.Chart3Btn.Font = new System.Drawing.Font("宋体", 12F);
            this.Chart3Btn.Location = new System.Drawing.Point(231, 12);
            this.Chart3Btn.Name = "Chart3Btn";
            this.Chart3Btn.Size = new System.Drawing.Size(161, 38);
            this.Chart3Btn.TabIndex = 9;
            this.Chart3Btn.Text = "吹膜机组运行记录";
            this.Chart3Btn.UseVisualStyleBackColor = true;
            this.Chart3Btn.Click += new System.EventHandler(this.Chart3Btn_Click);
            // 
            // Chart7Btn
            // 
            this.Chart7Btn.Font = new System.Drawing.Font("宋体", 12F);
            this.Chart7Btn.Location = new System.Drawing.Point(834, 12);
            this.Chart7Btn.Name = "Chart7Btn";
            this.Chart7Btn.Size = new System.Drawing.Size(175, 38);
            this.Chart7Btn.TabIndex = 10;
            this.Chart7Btn.Text = "吹膜岗位交接班记录";
            this.Chart7Btn.UseVisualStyleBackColor = true;
            this.Chart7Btn.Click += new System.EventHandler(this.Chart7Btn_Click);
            // 
            // ExtructionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.Chart7Btn);
            this.Controls.Add(this.Chart3Btn);
            this.Controls.Add(this.Chart5Btn);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Chart2Btn);
            this.Controls.Add(this.Chart4Btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtructionForm";
            this.Text = "ExtructionForm";
            this.Load += new System.EventHandler(this.BlowForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Chart2Btn;
        private System.Windows.Forms.Button Chart4Btn;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Chart5Btn;
        private System.Windows.Forms.Button Chart3Btn;
        private System.Windows.Forms.Button Chart7Btn;
    }
}