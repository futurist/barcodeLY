namespace barcode
{
    partial class frmPackage
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
            this.txtPkg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBar = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.LinkLabel();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.lblDuplicate = new System.Windows.Forms.LinkLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPkg = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLast = new System.Windows.Forms.Label();
            this.txtExit = new System.Windows.Forms.Button();
            this.panelBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPkg
            // 
            this.txtPkg.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular);
            this.txtPkg.Location = new System.Drawing.Point(61, 14);
            this.txtPkg.Name = "txtPkg";
            this.txtPkg.Size = new System.Drawing.Size(153, 34);
            this.txtPkg.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 34);
            this.label1.Text = "托盘";
            // 
            // panelBar
            // 
            this.panelBar.Controls.Add(this.btnUpload);
            this.panelBar.Controls.Add(this.lblStatus);
            this.panelBar.Controls.Add(this.txtDebug);
            this.panelBar.Controls.Add(this.lblDuplicate);
            this.panelBar.Controls.Add(this.listBox1);
            this.panelBar.Controls.Add(this.panel1);
            this.panelBar.Location = new System.Drawing.Point(6, 9);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(232, 240);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(67, 160);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 35);
            this.btnUpload.TabIndex = 27;
            this.btnUpload.Text = "上传数据";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(205, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(18, 18);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "<";
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(11, 62);
            this.txtDebug.MaxLength = 71200;
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(199, 56);
            this.txtDebug.TabIndex = 22;
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Underline);
            this.lblDuplicate.Location = new System.Drawing.Point(11, 50);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(201, 68);
            this.lblDuplicate.TabIndex = 24;
            this.lblDuplicate.Text = "已存在";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(1, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(220, 114);
            this.listBox1.TabIndex = 23;
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 33);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(170, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(192, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加";
            this.btnAdd.Visible = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "条码";
            // 
            // btnPkg
            // 
            this.btnPkg.Location = new System.Drawing.Point(61, 60);
            this.btnPkg.Name = "btnPkg";
            this.btnPkg.Size = new System.Drawing.Size(108, 47);
            this.btnPkg.TabIndex = 27;
            this.btnPkg.Text = "确定";
            this.btnPkg.Click += new System.EventHandler(this.btnPkg_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 20);
            this.label3.Text = "上次使用的托盘:";
            // 
            // lblLast
            // 
            this.lblLast.Location = new System.Drawing.Point(130, 155);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(55, 20);
            // 
            // txtExit
            // 
            this.txtExit.Location = new System.Drawing.Point(81, 210);
            this.txtExit.Name = "txtExit";
            this.txtExit.Size = new System.Drawing.Size(72, 20);
            this.txtExit.TabIndex = 30;
            this.txtExit.Text = "退出";
            this.txtExit.Click += new System.EventHandler(this.txtExit_Click);
            // 
            // frmPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.lblLast);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPkg);
            this.Controls.Add(this.txtPkg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtExit);
            this.Name = "frmPackage";
            this.Text = "frmPackage";
            this.panelBar.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPkg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.LinkLabel lblStatus;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.LinkLabel lblDuplicate;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPkg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.Button txtExit;
    }
}