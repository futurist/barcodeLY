namespace barcode
{
    partial class frmConfig
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnWan = new System.Windows.Forms.Button();
            this.btnLan = new System.Windows.Forms.Button();
            this.btnRet = new System.Windows.Forms.Button();
            this.btnViewConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWan
            // 
            this.btnWan.Location = new System.Drawing.Point(51, 12);
            this.btnWan.Name = "btnWan";
            this.btnWan.Size = new System.Drawing.Size(137, 46);
            this.btnWan.TabIndex = 0;
            this.btnWan.Text = "使用广域网";
            this.btnWan.Click += new System.EventHandler(this.btnWan_Click);
            // 
            // btnLan
            // 
            this.btnLan.Location = new System.Drawing.Point(51, 64);
            this.btnLan.Name = "btnLan";
            this.btnLan.Size = new System.Drawing.Size(137, 46);
            this.btnLan.TabIndex = 1;
            this.btnLan.Text = "使用局域网";
            this.btnLan.Click += new System.EventHandler(this.btnLan_Click);
            // 
            // btnRet
            // 
            this.btnRet.Location = new System.Drawing.Point(51, 190);
            this.btnRet.Name = "btnRet";
            this.btnRet.Size = new System.Drawing.Size(137, 46);
            this.btnRet.TabIndex = 2;
            this.btnRet.Text = "返回";
            this.btnRet.Click += new System.EventHandler(this.btnRet_Click);
            // 
            // btnViewConfig
            // 
            this.btnViewConfig.Location = new System.Drawing.Point(51, 138);
            this.btnViewConfig.Name = "btnViewConfig";
            this.btnViewConfig.Size = new System.Drawing.Size(137, 46);
            this.btnViewConfig.TabIndex = 3;
            this.btnViewConfig.Text = "查看配置";
            this.btnViewConfig.Click += new System.EventHandler(this.btnViewConfig_Click);
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.btnViewConfig);
            this.Controls.Add(this.btnRet);
            this.Controls.Add(this.btnLan);
            this.Controls.Add(this.btnWan);
            this.Name = "frmConfig";
            this.Text = "frmConfig";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnWan;
        private System.Windows.Forms.Button btnLan;
        private System.Windows.Forms.Button btnRet;
        private System.Windows.Forms.Button btnViewConfig;
    }
}