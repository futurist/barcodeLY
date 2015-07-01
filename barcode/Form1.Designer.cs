namespace barcode
{
    partial class Form1
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.lblDuplicate = new System.Windows.Forms.LinkLabel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(184, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.GotFocus += new System.EventHandler(this.textBox1_GotFocus);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(10, 54);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(220, 114);
            this.listBox1.TabIndex = 1;
            this.listBox1.EnabledChanged += new System.EventHandler(this.listBox1_EnabledChanged);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(6, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 33);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "备注";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(192, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加";
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnModify);
            this.panel2.Location = new System.Drawing.Point(5, -13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(233, 32);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(68, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(157, 23);
            this.textBox2.TabIndex = 0;
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.Text = "编辑备注";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(167, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(46, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(217, 1);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(44, 30);
            this.btnModify.TabIndex = 1;
            this.btnModify.Text = "修改";
            this.btnModify.Visible = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Underline);
            this.lblDuplicate.Location = new System.Drawing.Point(21, 77);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(199, 65);
            this.lblDuplicate.TabIndex = 6;
            this.lblDuplicate.Text = "已存在";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(124, 184);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(105, 32);
            this.btnUpload.TabIndex = 9;
            this.btnUpload.Text = "上传数据";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(92, 189);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 20);
            this.lblStatus.Text = "NC!!";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 184);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(77, 32);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.lblDuplicate);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblStatus);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.LinkLabel lblDuplicate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnExit;
    }
}

