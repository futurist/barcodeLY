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
    public partial class frmPackageList : BaseForm
    {
        [DllImport("coredll.dll", SetLastError = true)]
        extern static int SipShowIM(int dwFlag);
        const int SIPF_ON = 1;
        const int SIPF_OFF = 0;
        public void ShowSIP(Boolean ShowIt)
        {

            SipShowIM(ShowIt ? SIPF_ON : SIPF_OFF);
        }

        public bool IsClosed = false;

        public enum Beep : uint
        {
            SND_FILENAME = 0x00020000,
            SND_ASYNC = 0x00000001
        }

        [DllImport("coredll.dll", EntryPoint = "PlaySound")]
        public static extern int PlaySound(System.String pszSound, IntPtr hmod, uint fdwSound);


        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer inter2 = new System.Windows.Forms.Timer();

        public string filter = "";


        public int count = 0;
        public string prevDebugStr = "";

        public frmPackage form = null;

        public frmPackageList(frmPackage f)
        {
            InitializeComponent();

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.listBox1.DataSource = Data.pkgOrderList;

            form = f;

            txtDebug.Visible = false;
           
            Form_OnShow();

            listBox1.Focus();

            textBox1.Text = Data.lastPkgCode;
            btnAdd_Click();

        }


        public void Form_OnShow()
        {
            BaseForm.ScannerAutoInit();
            lblDuplicate.Hide();
        }

        public void Form_OnHide()
        {
            try
            {
                BaseForm.ScannerAutoExit();
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
            Data.pkgOrderList.Clear();

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
            if (IsClosed)
            {
                form.OnBarCodeNotify(BarCodeData, nLength);
                return;
            }
            this.textBox1.Focus();
            this.textBox1.Text = Encoding.Default.GetString(BarCodeData, 0, nLength);
            btnAdd_Click();

            

            //PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            //StandardKeyboradOut(BarCodeData, nLength, _SCAN_DATA_SUFFIX.ENTER_SUFFIX);
        }


        public void btnAdd_Click()
        {
            
            addLisBox(textBox1.Text, true);

            //textBox1.Focus();
            //txtPkg.Focus();

            getPkgData(txtPkg.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            btnAdd_Click();
        }

        public void addLisBox(string code, bool moveToTop)
        {

            if (code.Length < 8) return;

           
            PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            getProductData(code);

        }


        public void showDuplicateMsg(pkgOrderClass code, string str)
        {
            lblDuplicate.Show();
            lblDuplicate.Text = str;

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
            listBox1.DataSource = Data.pkgOrderList;
            listBox1.Enabled = true;

            try
            {
                listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.pkgOrderList.Count ? prevIndex : 0;
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
            if (prevFocus) listBox1.Focus();
        }


        
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            switch (e.KeyCode.ToString())
            {

                case "Delete":
                case "Back":
                    
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
            IsClosed = true;
            Form_OnHide();
            this.Hide();
            this.Close();
            form.Form_OnShow();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            returnToFolder();
        }

        public void returnToFolder() {
            Data.pkgOrderList.Clear();
            updateLisBox_datasource();

            txtPkg.Text = "";
        }

        private void txtPkg_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {

                case "Return":
                    getPkgData(txtPkg.Text);
                    break;
            }
        }

        public void getProductData(string code)
        {
            DataTable dt = null;
            string sql = string.Format(@"select top 1 sSampleMaterialNo from mmInDtl where sFabricNo='{0}' or sPackageNo='{0}'", code);

            try
            {
                dt = DB.Query(sql);
            }
            catch (Exception e)
            {
                string str = "{@error@}" + e.Message;
                MessageBox.Show(str);
                return;
            }
            if (object.ReferenceEquals(null, dt) || dt.Rows.Count < 1)
            {
                txtPkg.Text = "";
                Data.pkgOrderList.Clear();
                updateLisBox_datasource();
                return;
            }
            txtPkg.Text = dt.Rows[0][0].ToString();

        }

        public void getPkgData(string code)
        {
            if (code.Length < 1) return;

            Data.pkgOrderList.Clear();

            DataTable dt = null;
            string sql = string.Format(@"select sMaterialDesc, mmInDtl.sColorNo as sColorNo, mmInDtl.sBatchNo as sBatchNo, 
ISNULL( min(mmInDtl.iPackageOrder), 0) as minPkgNo, ISNULL( max(mmInDtl.iPackageOrder), 0) as maxPkgNo
from mmInDtl 
left join sdOrderDtl on usdOrderDtlGUID=sdOrderDtl.uGUID
left join sdOrderHdr on usdOrderHdrGUID=sdOrderHdr.uGUID
where (mmInDtl.sSampleMaterialNo='{0}' or sMaterialDesc='{0}' or sdOrderDtl.sCustomerStyleNo='{0}' )
and (sMaterialDesc is not null or mmInDtl.sSampleMaterialNo is not null or sdOrderDtl.sCustomerStyleNo is not null ) 
and mmInDtl.sColorNo is not null and iPackageOrder is not null
and (nStockPkgQty>0 )
group by sMaterialDesc,mmInDtl.sColorNo, sBatchNo
order by sMaterialDesc, maxPkgNo desc,mmInDtl.sColorNo, sBatchNo", code);

            try
            {
                dt = DB.Query(sql);
            }
            catch (Exception e)
            {
                string str = "{@error@}" + e.Message;
                MessageBox.Show(str);
                return;
            }
            if (object.ReferenceEquals(null, dt) || dt.Rows.Count < 1) return;

            foreach (DataRow row in dt.Rows)
            {
                Data.pkgOrderList.Add(new pkgOrderClass( code, row[0].ToString(), row[1].ToString(), row[2].ToString(), int.Parse(row[3].ToString()), int.Parse(row[4].ToString())));
            }

            updateLisBox_datasource();

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            getPkgData( txtPkg.Text );
        }

        private void txtPkg_GotFocus(object sender, EventArgs e)
        {
            ShowSIP(true);
        }

        private void txtPkg_LostFocus(object sender, EventArgs e)
        {
            ShowSIP(false);
        }


    }
}