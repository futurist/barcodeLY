using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using barcode;

namespace barcode
{
    public partial class frmPackage : BaseForm
    {
		//declare @id as nchar(40)
//set @id = (select N'P'+ CONVERT (varchar(20), (cast ( max( RIGHT( sPackageNo, 10 ) ) as int )+1) ) as maxPkgID from mmInDtl  where  LEN(sPackageNo)=11 )
//update TestDB.dbo.testdata set id=@id where num=10


        public enum Beep : uint
        {
            SND_FILENAME = 0x00020000,
            SND_ASYNC = 0x00000001
        }

        [DllImport("coredll.dll", EntryPoint = "PlaySound")]
        public static extern int PlaySound(System.String pszSound, IntPtr hmod, uint fdwSound);


        public Form1 form1;
        public folderClass folder = null;
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer inter2 = new System.Windows.Forms.Timer();

        public string filter = "";


        public int count = 0;
        public string prevDebugStr = "";


        public frmPackage()
        {
            InitializeComponent();
            panelBar.Hide();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.listBox1.DataSource = Data.codeList;

            txtDebug.Visible = false;
        }

        public void debug(string str)
        {
            if (!txtDebug.Visible) return;
            if (prevDebugStr == str) return;
            prevDebugStr = str;
            txtDebug.Text = str + "\r\n" + txtDebug.Text;
        }

        //we get barcode here
        public override void OnBarCodeNotify(byte[] BarCodeData, int nLength)
        {
            this.textBox1.Focus();
            this.textBox1.Text = Encoding.Default.GetString(BarCodeData, 0, nLength);
            btnAdd_Click();

            //PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            //StandardKeyboradOut(BarCodeData, nLength, _SCAN_DATA_SUFFIX.ENTER_SUFFIX);
        }


        public void btnAdd_Click()
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) idx = Data.codeList.Count - 1;
            addLisBox(new codeClass(textBox1.Text, folder.Id), true);

            //textBox1.Focus();
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            btnAdd_Click();
        }

        public void addLisBox(codeClass code, bool moveToTop)
        {
            if (code.Id == "")
            {
                return;
            }
            if (checkDuplicate(code.Id) != false)
            {
                showDuplicateMsg(code);
                return;
            }

            if (code.Id.Length < 8) return;

            PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            updateLisBox_datasource();

            if (moveToTop) listBox1.SelectedIndex = 0;

        }

        public bool checkDuplicate(string ID)
        {
            foreach (var code in Data.codeList)
            {
                if (ID == code.Id) return true;
            }
            return false;
        }

        public void showDuplicateMsg(codeClass code)
        {
            lblDuplicate.Show();
            lblDuplicate.Text = code.Id + "\r\n" + "已存在";

            PlaySound("infbeg.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            inter1.Enabled = false;
            inter1.Interval = 1000; // 1 second
            inter1.Tick += delegate { lblDuplicate.Hide(); inter1.Enabled = false; };
            inter1.Enabled = true;

        }

        public void updateLisBox_datasource()
        {
            var prevIndex = listBox1.SelectedIndex;
            var prevFocus = listBox1.Focused;

            listBox1.Enabled = false;
            listBox1.DataSource = null;
            listBox1.DataSource = Data.getCodesFromFolder(folder.Id);
            listBox1.Enabled = true;

            try
            {
                listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.codeList.Count ? prevIndex : 0;
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
            if (prevFocus) listBox1.Focus();
        }


        private void btnPkg_Click(object sender, EventArgs e)
        {
            folder = new folderClass( txtPkg.Text );
            lblLast.Text = txtPkg.Text;
            txtPkg.Text = "";

            panelBar.Show();
            
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            switch (e.KeyCode.ToString())
            {

                case "Delete":
                case "Back":
                    Data.codeList.RemoveAt(idx);
                    
                    updateLisBox_datasource();

                    //isDirty = true;
                    e.Handled = true;
                    break;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Return":
                    btnAdd_Click(sender, e);
                    break;
                case "F8":
                    textBox1.Text = "";
                    break;
            }

        }

        private void txtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}