namespace mySystem.Setting
{
    partial class 库存设置
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
            this.tabPage基本信息设置 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgv检验标准 = new System.Windows.Forms.DataGridView();
            this.del检验标准 = new System.Windows.Forms.Button();
            this.add检验标准 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn保存项目 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgv供应商 = new System.Windows.Forms.DataGridView();
            this.del供应商 = new System.Windows.Forms.Button();
            this.add供应商 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.tabPage人员 = new System.Windows.Forms.TabPage();
            this.Btn保存人员 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv权限 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.dgv人员 = new System.Windows.Forms.DataGridView();
            this.del人员 = new System.Windows.Forms.Button();
            this.add人员 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage基本信息设置.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv检验标准)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv供应商)).BeginInit();
            this.tabPage人员.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv权限)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv人员)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage基本信息设置);
            this.tabControl1.Controls.Add(this.tabPage人员);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1128, 587);
            this.tabControl1.TabIndex = 57;
            // 
            // tabPage基本信息设置
            // 
            this.tabPage基本信息设置.Controls.Add(this.groupBox1);
            this.tabPage基本信息设置.Controls.Add(this.label1);
            this.tabPage基本信息设置.Controls.Add(this.Btn保存项目);
            this.tabPage基本信息设置.Controls.Add(this.groupBox6);
            this.tabPage基本信息设置.Controls.Add(this.label15);
            this.tabPage基本信息设置.Location = new System.Drawing.Point(4, 29);
            this.tabPage基本信息设置.Name = "tabPage基本信息设置";
            this.tabPage基本信息设置.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage基本信息设置.Size = new System.Drawing.Size(1120, 554);
            this.tabPage基本信息设置.TabIndex = 0;
            this.tabPage基本信息设置.Text = "基本信息设置";
            this.tabPage基本信息设置.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgv检验标准);
            this.groupBox1.Controls.Add(this.del检验标准);
            this.groupBox1.Controls.Add(this.add检验标准);
            this.groupBox1.Location = new System.Drawing.Point(35, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(846, 225);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            // 
            // dgv检验标准
            // 
            this.dgv检验标准.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv检验标准.Location = new System.Drawing.Point(12, 51);
            this.dgv检验标准.Name = "dgv检验标准";
            this.dgv检验标准.RowTemplate.Height = 23;
            this.dgv检验标准.Size = new System.Drawing.Size(813, 165);
            this.dgv检验标准.TabIndex = 2;
            // 
            // del检验标准
            // 
            this.del检验标准.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.del检验标准.Location = new System.Drawing.Point(100, 14);
            this.del检验标准.Name = "del检验标准";
            this.del检验标准.Size = new System.Drawing.Size(70, 30);
            this.del检验标准.TabIndex = 1;
            this.del检验标准.Text = "删除";
            this.del检验标准.UseVisualStyleBackColor = true;
            this.del检验标准.Click += new System.EventHandler(this.del检验标准_Click);
            // 
            // add检验标准
            // 
            this.add检验标准.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add检验标准.Location = new System.Drawing.Point(12, 14);
            this.add检验标准.Name = "add检验标准";
            this.add检验标准.Size = new System.Drawing.Size(70, 30);
            this.add检验标准.TabIndex = 0;
            this.add检验标准.Text = "添加";
            this.add检验标准.UseVisualStyleBackColor = true;
            this.add检验标准.Click += new System.EventHandler(this.add检验标准_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(35, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 58;
            this.label1.Text = "检验标准设置";
            // 
            // Btn保存项目
            // 
            this.Btn保存项目.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn保存项目.Location = new System.Drawing.Point(968, 468);
            this.Btn保存项目.Name = "Btn保存项目";
            this.Btn保存项目.Size = new System.Drawing.Size(90, 33);
            this.Btn保存项目.TabIndex = 57;
            this.Btn保存项目.Text = "保存设置";
            this.Btn保存项目.UseVisualStyleBackColor = true;
            this.Btn保存项目.Click += new System.EventHandler(this.Btn保存项目_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dgv供应商);
            this.groupBox6.Controls.Add(this.del供应商);
            this.groupBox6.Controls.Add(this.add供应商);
            this.groupBox6.Location = new System.Drawing.Point(31, 34);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(846, 225);
            this.groupBox6.TabIndex = 54;
            this.groupBox6.TabStop = false;
            // 
            // dgv供应商
            // 
            this.dgv供应商.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv供应商.Location = new System.Drawing.Point(12, 51);
            this.dgv供应商.Name = "dgv供应商";
            this.dgv供应商.RowTemplate.Height = 23;
            this.dgv供应商.Size = new System.Drawing.Size(813, 165);
            this.dgv供应商.TabIndex = 2;
            // 
            // del供应商
            // 
            this.del供应商.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.del供应商.Location = new System.Drawing.Point(100, 14);
            this.del供应商.Name = "del供应商";
            this.del供应商.Size = new System.Drawing.Size(70, 30);
            this.del供应商.TabIndex = 1;
            this.del供应商.Text = "删除";
            this.del供应商.UseVisualStyleBackColor = true;
            this.del供应商.Click += new System.EventHandler(this.del供应商_Click);
            // 
            // add供应商
            // 
            this.add供应商.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add供应商.Location = new System.Drawing.Point(12, 14);
            this.add供应商.Name = "add供应商";
            this.add供应商.Size = new System.Drawing.Size(70, 30);
            this.add供应商.TabIndex = 0;
            this.add供应商.Text = "添加";
            this.add供应商.UseVisualStyleBackColor = true;
            this.add供应商.Click += new System.EventHandler(this.add供应商_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(31, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(156, 20);
            this.label15.TabIndex = 53;
            this.label15.Text = "供应商信息设置";
            // 
            // tabPage人员
            // 
            this.tabPage人员.Controls.Add(this.Btn保存人员);
            this.tabPage人员.Controls.Add(this.groupBox5);
            this.tabPage人员.Controls.Add(this.label3);
            this.tabPage人员.Controls.Add(this.groupBox7);
            this.tabPage人员.Controls.Add(this.label4);
            this.tabPage人员.Location = new System.Drawing.Point(4, 29);
            this.tabPage人员.Name = "tabPage人员";
            this.tabPage人员.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage人员.Size = new System.Drawing.Size(1120, 554);
            this.tabPage人员.TabIndex = 3;
            this.tabPage人员.Text = "人员设置";
            this.tabPage人员.UseVisualStyleBackColor = true;
            // 
            // Btn保存人员
            // 
            this.Btn保存人员.Font = new System.Drawing.Font("SimSun", 12F);
            this.Btn保存人员.Location = new System.Drawing.Point(937, 508);
            this.Btn保存人员.Name = "Btn保存人员";
            this.Btn保存人员.Size = new System.Drawing.Size(90, 33);
            this.Btn保存人员.TabIndex = 57;
            this.Btn保存人员.Text = "保存设置";
            this.Btn保存人员.UseVisualStyleBackColor = true;
            this.Btn保存人员.Click += new System.EventHandler(this.Btn保存人员_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel1);
            this.groupBox5.Controls.Add(this.dgv权限);
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Location = new System.Drawing.Point(17, 313);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(802, 213);
            this.groupBox5.TabIndex = 56;
            this.groupBox5.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(819, 401);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 15);
            this.panel1.TabIndex = 55;
            // 
            // dgv权限
            // 
            this.dgv权限.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv权限.Location = new System.Drawing.Point(12, 17);
            this.dgv权限.Name = "dgv权限";
            this.dgv权限.RowTemplate.Height = 23;
            this.dgv权限.Size = new System.Drawing.Size(772, 187);
            this.dgv权限.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("SimSun", 12F);
            this.button3.Location = new System.Drawing.Point(975, 362);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 33);
            this.button3.TabIndex = 54;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("SimSun", 12F);
            this.button5.Location = new System.Drawing.Point(873, 362);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(82, 33);
            this.button5.TabIndex = 53;
            this.button5.Text = "保存设置";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(17, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 20);
            this.label3.TabIndex = 55;
            this.label3.Text = "库存管理人员权限设置";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.dgv人员);
            this.groupBox7.Controls.Add(this.del人员);
            this.groupBox7.Controls.Add(this.add人员);
            this.groupBox7.Location = new System.Drawing.Point(17, 34);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(802, 241);
            this.groupBox7.TabIndex = 54;
            this.groupBox7.TabStop = false;
            // 
            // dgv人员
            // 
            this.dgv人员.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv人员.Location = new System.Drawing.Point(12, 51);
            this.dgv人员.Name = "dgv人员";
            this.dgv人员.RowTemplate.Height = 23;
            this.dgv人员.Size = new System.Drawing.Size(772, 184);
            this.dgv人员.TabIndex = 2;
            // 
            // del人员
            // 
            this.del人员.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.del人员.Location = new System.Drawing.Point(100, 14);
            this.del人员.Name = "del人员";
            this.del人员.Size = new System.Drawing.Size(70, 30);
            this.del人员.TabIndex = 1;
            this.del人员.Text = "删除";
            this.del人员.UseVisualStyleBackColor = true;
            this.del人员.Click += new System.EventHandler(this.del人员_Click);
            // 
            // add人员
            // 
            this.add人员.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add人员.Location = new System.Drawing.Point(12, 14);
            this.add人员.Name = "add人员";
            this.add人员.Size = new System.Drawing.Size(70, 30);
            this.add人员.TabIndex = 0;
            this.add人员.Text = "添加";
            this.add人员.UseVisualStyleBackColor = true;
            this.add人员.Click += new System.EventHandler(this.add人员_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(17, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 20);
            this.label4.TabIndex = 53;
            this.label4.Text = "库存管理人员设置";
            // 
            // 库存设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 606);
            this.Controls.Add(this.tabControl1);
            this.Name = "库存设置";
            this.Text = "库存设置";
            this.tabControl1.ResumeLayout(false);
            this.tabPage基本信息设置.ResumeLayout(false);
            this.tabPage基本信息设置.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv检验标准)).EndInit();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv供应商)).EndInit();
            this.tabPage人员.ResumeLayout(false);
            this.tabPage人员.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv权限)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv人员)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage基本信息设置;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgv检验标准;
        private System.Windows.Forms.Button del检验标准;
        private System.Windows.Forms.Button add检验标准;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn保存项目;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dgv供应商;
        private System.Windows.Forms.Button del供应商;
        private System.Windows.Forms.Button add供应商;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage tabPage人员;
        private System.Windows.Forms.Button Btn保存人员;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgv权限;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.DataGridView dgv人员;
        private System.Windows.Forms.Button del人员;
        private System.Windows.Forms.Button add人员;
        private System.Windows.Forms.Label label4;

    }
}