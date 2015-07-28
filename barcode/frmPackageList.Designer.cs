namespace barcode
{
    partial class frmPackageList
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.LinkLabel();
            this.panelBar = new System.Windows.Forms.Panel();
            this.lblFINo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.lblDuplicate = new System.Windows.Forms.LinkLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.txtPkg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExit = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOrder = new System.Windows.Forms.Label();
            this.panelBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(148, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(41, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "检查";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "条码";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(113, 23);
            this.textBox1.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(7, 272);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(18, 18);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "<";
            // 
            // panelBar
            // 
            this.panelBar.Controls.Add(this.lblFINo);
            this.panelBar.Controls.Add(this.label4);
            this.panelBar.Controls.Add(this.txtDebug);
            this.panelBar.Controls.Add(this.lblDuplicate);
            this.panelBar.Controls.Add(this.listBox1);
            this.panelBar.Controls.Add(this.panel1);
            this.panelBar.Location = new System.Drawing.Point(6, 59);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(232, 198);
            // 
            // lblFINo
            // 
            this.lblFINo.Location = new System.Drawing.Point(57, 39);
            this.lblFINo.Name = "lblFINo";
            this.lblFINo.Size = new System.Drawing.Size(166, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.Text = "落布号";
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(16, 94);
            this.txtDebug.MaxLength = 71200;
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(199, 56);
            this.txtDebug.TabIndex = 22;
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Underline);
            this.lblDuplicate.Location = new System.Drawing.Point(16, 82);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(201, 68);
            this.lblDuplicate.TabIndex = 24;
            this.lblDuplicate.Text = "已存在";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(6, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(220, 130);
            this.listBox1.TabIndex = 23;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRepeat);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 33);
            // 
            // btnRepeat
            // 
            this.btnRepeat.Location = new System.Drawing.Point(191, 1);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(41, 30);
            this.btnRepeat.TabIndex = 3;
            this.btnRepeat.Text = "重打";
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            // 
            // txtPkg
            // 
            this.txtPkg.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular);
            this.txtPkg.Location = new System.Drawing.Point(71, 31);
            this.txtPkg.Name = "txtPkg";
            this.txtPkg.Size = new System.Drawing.Size(110, 23);
            this.txtPkg.TabIndex = 32;
            this.txtPkg.GotFocus += new System.EventHandler(this.txtPkg_GotFocus);
            this.txtPkg.LostFocus += new System.EventHandler(this.txtPkg_LostFocus);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.Text = "产品编码";
            // 
            // txtExit
            // 
            this.txtExit.Location = new System.Drawing.Point(81, 261);
            this.txtExit.Name = "txtExit";
            this.txtExit.Size = new System.Drawing.Size(72, 29);
            this.txtExit.TabIndex = 36;
            this.txtExit.Text = "返回";
            this.txtExit.Click += new System.EventHandler(this.txtExit_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(183, 31);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(52, 25);
            this.btnQuery.TabIndex = 39;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(5, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 20);
            this.label3.Text = "包号查询";
            // 
            // lblOrder
            // 
            this.lblOrder.Location = new System.Drawing.Point(81, 7);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(148, 20);
            // 
            // frmPackageList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.lblOrder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.txtPkg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtExit);
            this.Name = "frmPackageList";
            this.Text = "frmPackageList";
            this.panelBar.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.LinkLabel lblStatus;
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.LinkLabel lblDuplicate;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPkg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button txtExit;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label lblFINo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRepeat;
    }
}