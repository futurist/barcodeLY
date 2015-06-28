namespace barcode
{
    partial class Form2
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDuplicate = new System.Windows.Forms.LinkLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.lv = new System.Windows.Forms.ListView();
            this.code = new System.Windows.Forms.ColumnHeader();
            this.name = new System.Windows.Forms.ColumnHeader();
            this.color = new System.Windows.Forms.ColumnHeader();
            this.num = new System.Windows.Forms.ColumnHeader();
            this.sn = new System.Windows.Forms.ColumnHeader();
            this.lblFolder00 = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(13, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 33);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(207, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.GotFocus += new System.EventHandler(this.textBox1_GotFocus);
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
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "条码";
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Underline);
            this.lblDuplicate.Location = new System.Drawing.Point(31, 57);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(257, 68);
            this.lblDuplicate.TabIndex = 8;
            this.lblDuplicate.Text = "已存在";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(9, 39);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(299, 114);
            this.listBox1.TabIndex = 7;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // lv
            // 
            this.lv.Columns.Add(this.code);
            this.lv.Columns.Add(this.name);
            this.lv.Columns.Add(this.color);
            this.lv.Columns.Add(this.num);
            this.lv.Columns.Add(this.sn);
            this.lv.FullRowSelect = true;
            listViewItem1.Text = "AAAAA2134";
            listViewItem1.SubItems.Add("sdfsdf");
            listViewItem1.SubItems.Add("sdfsdsd");
            listViewItem2.Text = "BBBBB";
            listViewItem2.SubItems.Add("111");
            listViewItem2.SubItems.Add("222");
            this.lv.Items.Add(listViewItem1);
            this.lv.Items.Add(listViewItem2);
            this.lv.Location = new System.Drawing.Point(9, 180);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(299, 123);
            this.lv.TabIndex = 10;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lv_KeyDown);
            // 
            // code
            // 
            this.code.Text = "品名";
            this.code.Width = 48;
            // 
            // name
            // 
            this.name.Text = "名称";
            this.name.Width = 60;
            // 
            // color
            // 
            this.color.Text = "花色";
            this.color.Width = 60;
            // 
            // num
            // 
            this.num.Text = "数量";
            this.num.Width = 60;
            // 
            // sn
            // 
            this.sn.Text = "条码";
            this.sn.Width = 60;
            // 
            // lblFolder00
            // 
            this.lblFolder00.Location = new System.Drawing.Point(9, 158);
            this.lblFolder00.Name = "lblFolder00";
            this.lblFolder00.Size = new System.Drawing.Size(137, 20);
            this.lblFolder00.Text = "备注：";
            // 
            // lblNum
            // 
            this.lblNum.Location = new System.Drawing.Point(171, 158);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(137, 20);
            this.lblNum.Text = "品种：";
            this.lblNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFolder
            // 
            this.lblFolder.Location = new System.Drawing.Point(51, 157);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(100, 20);
            this.lblFolder.TabIndex = 15;
            this.lblFolder.Text = "linkLabel1";
            this.lblFolder.Click += new System.EventHandler(this.lblFolder_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(283, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 20);
            this.lblStatus.Text = "NC!!";
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(46, 69);
            this.txtDebug.MaxLength = 71200;
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(176, 56);
            this.txtDebug.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(318, 317);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.lblFolder00);
            this.Controls.Add(this.lv);
            this.Controls.Add(this.lblDuplicate);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.LinkLabel lblDuplicate;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader code;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader color;
        private System.Windows.Forms.ColumnHeader num;
        private System.Windows.Forms.ColumnHeader sn;
        private System.Windows.Forms.Label lblFolder00;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.LinkLabel lblFolder;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtDebug;

    }
}