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
            this.btnDeleteBar = new System.Windows.Forms.Button();
            this.lblCurPkg = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.lblLastPkg = new System.Windows.Forms.Label();
            this.txtExit = new System.Windows.Forms.Button();
            this.btnViewPkg = new System.Windows.Forms.Button();
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
            this.txtPkg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPkg_KeyDown);
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
            this.panelBar.Controls.Add(this.btnDeleteBar);
            this.panelBar.Controls.Add(this.lblCurPkg);
            this.panelBar.Controls.Add(this.label4);
            this.panelBar.Controls.Add(this.btnUpload);
            this.panelBar.Controls.Add(this.lblStatus);
            this.panelBar.Controls.Add(this.txtDebug);
            this.panelBar.Controls.Add(this.lblDuplicate);
            this.panelBar.Controls.Add(this.listBox1);
            this.panelBar.Controls.Add(this.panel1);
            this.panelBar.Location = new System.Drawing.Point(220, 14);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(232, 240);
            // 
            // btnDeleteBar
            // 
            this.btnDeleteBar.Location = new System.Drawing.Point(132, 191);
            this.btnDeleteBar.Name = "btnDeleteBar";
            this.btnDeleteBar.Size = new System.Drawing.Size(80, 35);
            this.btnDeleteBar.TabIndex = 31;
            this.btnDeleteBar.Text = "删除条码";
            this.btnDeleteBar.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCurPkg
            // 
            this.lblCurPkg.Location = new System.Drawing.Point(68, 159);
            this.lblCurPkg.Name = "lblCurPkg";
            this.lblCurPkg.Size = new System.Drawing.Size(100, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.Text = "托盘号:";
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(11, 191);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 35);
            this.btnUpload.TabIndex = 27;
            this.btnUpload.Text = "上传数据";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
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
            this.btnPkg.Location = new System.Drawing.Point(61, 54);
            this.btnPkg.Name = "btnPkg";
            this.btnPkg.Size = new System.Drawing.Size(108, 38);
            this.btnPkg.TabIndex = 27;
            this.btnPkg.Text = "确定";
            this.btnPkg.Click += new System.EventHandler(this.btnPkg_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 20);
            this.label3.Text = "上次使用的托盘:";
            // 
            // lblLastPkg
            // 
            this.lblLastPkg.Location = new System.Drawing.Point(130, 112);
            this.lblLastPkg.Name = "lblLastPkg";
            this.lblLastPkg.Size = new System.Drawing.Size(55, 20);
            // 
            // txtExit
            // 
            this.txtExit.Location = new System.Drawing.Point(81, 216);
            this.txtExit.Name = "txtExit";
            this.txtExit.Size = new System.Drawing.Size(72, 28);
            this.txtExit.TabIndex = 30;
            this.txtExit.Text = "退出";
            this.txtExit.Click += new System.EventHandler(this.txtExit_Click);
            // 
            // btnViewPkg
            // 
            this.btnViewPkg.Location = new System.Drawing.Point(81, 172);
            this.btnViewPkg.Name = "btnViewPkg";
            this.btnViewPkg.Size = new System.Drawing.Size(72, 28);
            this.btnViewPkg.TabIndex = 31;
            this.btnViewPkg.Text = "查看包号";
            this.btnViewPkg.Click += new System.EventHandler(this.btnViewPkg_Click);
            // 
            // frmPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.lblLastPkg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPkg);
            this.Controls.Add(this.txtPkg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtExit);
            this.Controls.Add(this.btnViewPkg);
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
        private System.Windows.Forms.Label lblLastPkg;
        private System.Windows.Forms.Button txtExit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCurPkg;
        private System.Windows.Forms.Button btnDeleteBar;
        private System.Windows.Forms.Button btnViewPkg;
    }
}