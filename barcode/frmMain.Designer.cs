namespace barcode
{
    partial class frmMain
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
            this.btnOut = new System.Windows.Forms.Button();
            this.btnPack = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOut
            // 
            this.btnOut.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnOut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnOut.Location = new System.Drawing.Point(45, 63);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(122, 39);
            this.btnOut.TabIndex = 0;
            this.btnOut.Text = "出库";
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnPack
            // 
            this.btnPack.Location = new System.Drawing.Point(45, 13);
            this.btnPack.Name = "btnPack";
            this.btnPack.Size = new System.Drawing.Size(122, 40);
            this.btnPack.TabIndex = 1;
            this.btnPack.Text = "打包";
            this.btnPack.Click += new System.EventHandler(this.btnPack_Click);
            // 
            // btnExit
            // 
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnExit.Location = new System.Drawing.Point(45, 190);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(122, 38);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Silver;
            this.btnConfig.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnConfig.Location = new System.Drawing.Point(45, 146);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(122, 39);
            this.btnConfig.TabIndex = 3;
            this.btnConfig.Text = "配置";
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPack);
            this.Controls.Add(this.btnOut);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Button btnPack;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnConfig;
    }
}