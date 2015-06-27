using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neolix.Device;

namespace barcode
{

    public partial class Form1 : Form
    {

        public Timer inter1 = new Timer();

        public Form1()
        {
            InitializeComponent();

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            this.panel2.Hide();
            this.lblDuplicate.Hide();

            this.lblDuplicate.BackColor = Color.Transparent;

           
            //this.listBox1.DisplayMember = "Text";
            //this.listBox1.ValueMember = "Id";
            this.listBox1.DataSource = Data.folderList;
            
        }
        Scaner scaner = new Scaner();

       
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 133 || e.KeyValue == 134)
            {
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    scaner.Open();
                    scaner.ScanerDataReceived += ScanerDataReceived;
                    scaner.Read();
                    //Scaner.Close();
                }
            }

            switch(e.KeyCode.ToString()){
                case "Return":
                    btnAdd_Click(sender, e);
                    break;
                case "Escape":
                    textBox1.Text = "";
                    Application.Exit();
                    break;
            }

        }
        
        private void ScanerDataReceived(object sender, string code)
        {
            MethodInvoker mi = delegate
                                   {

                                       this.textBox1.Text = code;
                                       this.textBox1.Focus();
                                   };
            Invoke(mi);
        }

        public delegate void MethodInvoker();

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            var val = (folderClass)listBox1.SelectedValue;
            var isDirty = false;

            //textBox1.Text = e.KeyCode.ToString();

            switch (e.KeyCode.ToString()) { 
                case "Delete":
                    Data.folderList.RemoveAt(idx);
                    isDirty = true;
                    break;
                case "Space":
                    textBox2.Text = Data.folderList[idx].Id;

                    this.panel2.Location = panel1.Location;
                    this.panel2.Visible = true;
                    this.panel1.Visible = false;
                    textBox2.Focus();
                    textBox2.SelectAll();

                    break;
                case "Return":
                    if (Data.folderIndex == -1) return;

                    Data.folderIndex = listBox1.SelectedIndex;
                    string ID = Data.folderList[Data.folderIndex].Id;
                    //listBox1.Enabled = false;

                    showForm2( ID );

                    break;

                case "Tab":
                    textBox1.Focus();
                    e.Handled = true;
                    break;
                default:
                    Data.folderList[idx].Code = e.KeyCode.ToString();
                    //isDirty = true;
                    break;
            }

            if (isDirty)
            {
                updateLisBox();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            Data.folderIndex  = idx;
            if (idx == -1) return;
            
            string curID = ((folderClass)listBox1.SelectedItem).Id;
            //textBox1.Text =s;
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Return":
                    btnModify_Click(sender, e);
                    break;
                case "Escape":
                    btnCancel_Click(sender, e);
                    break;
            }
        }


        public void addLisBox( folderClass folder ) 
        {
            if (folder.Id == "")
            {
                listBox1.Focus();
                return;
            }
            if (checkDuplicate(folder.Id) != false )
            {
                showDuplicateMsg(folder);
                return;
            }
            
            Data.folderList.Add(folder);
            Data.folderIndex  = Data.folderList.Count-1;

            updateLisBox();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) idx = Data.folderList.Count - 1;
            addLisBox(new folderClass(textBox1.Text));
            //textBox1.Focus();
            textBox1.Text = "";
            listBox1.SelectedIndex = Data.folderList.Count - 1;
            listBox1.Focus();
        }


        public void modifyLisBox(folderClass folder, int idx)
        {
            if (folder.Id == "")
            {
                listBox1.Focus();
                return;
            }
            if (checkDuplicate(folder.Id)!=false)
            {
                showDuplicateMsg(folder);
                return;
            }

            Data.folderList[idx] = (folder);

            updateLisBox();
        }


        public void showDuplicateMsg( folderClass folder) {
            lblDuplicate.Show();
            lblDuplicate.Text = folder.Id + "\r\n" + "已存在";

            inter1.Enabled = false;
            inter1.Interval = 1000; // 1 second
            inter1.Tick += delegate { lblDuplicate.Hide(); inter1.Enabled = false; };
            inter1.Enabled = true;

        }

        public void updateLisBox() {
            var prevIndex = listBox1.SelectedIndex;
            listBox1.DataSource = null;
            listBox1.DataSource = Data.folderList;
            
            listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.folderList.Count ? prevIndex : Data.folderList.Count - 1;
            Data.folderIndex = listBox1.SelectedIndex;
            
        }

        public bool checkDuplicate(string ID)
        {
            foreach( var folder in Data.folderList )
            {
                if (ID == folder.Id) return true;
            }
            return false;
        }

        public void showForm2(string folderID) {
            this.Hide();
            if (!Data.formList.ContainsKey(folderID))
            {
                Data.formList.Add(folderID, new Form2() );
            }
            Data.formList[folderID].Show();
        }


        public Dictionary<folderClass, string> getUnique()
        {
            var dict = new Dictionary<folderClass,string>();

            foreach( var folder in Data.folderList )
            {
                if (!dict.ContainsKey(folder))
                    dict.Add(folder, folder.Id);
            }
            return dict;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            modifyLisBox( new folderClass(textBox2.Text), Data.folderIndex  );
            this.panel2.Visible = false;
            this.panel1.Visible = true;
            textBox1.Text = "";
            listBox1.SelectedIndex = Data.folderIndex ;
            listBox1.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.panel1.Visible = true;
            textBox1.Text = "";
            listBox1.SelectedIndex = Data.folderIndex ;
            listBox1.Focus();
        }


        private void Form1_GotFocus(object sender, EventArgs e)
        {
            textBox1.Focus();
            listBox1.Enabled = true;

            Data.curForm = this;
            showMsg("");
        }

        private void listBox1_EnabledChanged(object sender, EventArgs e)
        {
            if(listBox1.Enabled) updateLisBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            Form1_GotFocus(sender, e);
            //this.Paint += Form1_GotFocus;
            textBox1.GotFocus += Form1_GotFocus;

            //WinCE.createMemFile("[2015-6-21 13:40:02]单据号[150621021100]打印成功！ [2015-6-21 13:40:31]单据号[P1506210166]开始打印。。。 0]开始打印。。。 [2015-6-25 10:46:26]单据号[150625017100]打印成功！ [2015-6-25 10:46:34]单据号[P1506250174]开始打印。。。 [2015-6-25 10:46:37]单据号[P1506250174]打印成功！ [2015-6-25 10:47:58]单据号[150625017200]开始打印。。。 [2015-6-25 10:48:02]单据号[150625017200]打印成功！ [2015-6-25 10:49:22]单据号[P1506250175]开始打印。。。 [2015-6-25 10:49:26]单据号[P1506250175]打印成功！");

        }

        public void showMsg(string str)
        {
            lblStatus.Text = str;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }



    }

}