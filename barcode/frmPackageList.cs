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
using System.Diagnostics;

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
            textBox1.Text = Regex.Replace(textBox1.Text, "^0+", "P");

            btnAdd_Click();

            scrollLB(listBox1.Handle);

            listBox1.KeyDown+=new KeyEventHandler(listBox1_KeyDown);

        }




        [DllImport("coredll.dll")]
        static extern uint GetWindowLong(IntPtr hwnd, int index);
        [DllImport("coredll.dll")]
        static extern void SetWindowLong(IntPtr hwnd, int index, uint value);
        [DllImport("coredll")]
        static extern IntPtr SendMessage(IntPtr Handle, Int32 msg, IntPtr wParam,
        IntPtr lParam);
        [DllImport("coredll.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        const int LB_SETHORIZONTALEXTENT = 0x194;

        void scrollLB(IntPtr hWndListBox)
        {
            uint windowLong = GetWindowLong(hWndListBox, -16) | 0x00100000;
            SetWindowLong(hWndListBox, -16, windowLong);

            SendMessage(hWndListBox, LB_SETHORIZONTALEXTENT, (IntPtr)1520, IntPtr.Zero);
            //SetWindowPos(hWndListBox, IntPtr.Zero, 100, 0, listBox1.Width, listBox1.Height, SWP_NOMOVE || SWP_NOZORDER || SWP_NOSIZE || SWP_FRAMECHANGED);
        }



        // Scrolls a given textbox. handle: an handle to our textbox. pixels: number of pixels to scroll.
        //http://stackoverflow.com/questions/13975463/scrolling-using-setscrollinfo-api
        //http://stackoverflow.com/questions/25867581/appears-horizontal-scrollbar-after-minimizing-listview-that-has-auto-resize-colu

        const int WM_VSCROLL = 0x0115;
        const int WM_HSCROLL = 0x0114;

        public enum ScrollInfoMask : uint
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }

        public enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }
        public enum ScrollBarCommands
        {
            SB_LINEUP = 0,
            SB_LINELEFT = 0,
            SB_LINEDOWN = 1,
            SB_LINERIGHT = 1,
            SB_PAGEUP = 2,
            SB_PAGELEFT = 2,
            SB_PAGEDOWN = 3,
            SB_PAGERIGHT = 3,
            SB_THUMBPOSITION = 4,
            SB_THUMBTRACK = 5,
            SB_TOP = 6,
            SB_LEFT = 6,
            SB_BOTTOM = 7,
            SB_RIGHT = 7,
            SB_ENDSCROLL = 8
        }

        [DllImport("coredll.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        [DllImport("coredll.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("coredll.dll")]
        static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO
           lpsi, bool fRedraw);

        public static bool GetScrollInfo(Control ctrl, ref SCROLLINFO si, ScrollBarDirection scrollBarDirection)
        {
            if (ctrl != null)
            {
                si.cbSize = (uint)Marshal.SizeOf(si);
                si.fMask = (int)ScrollInfoMask.SIF_ALL;
                if (GetScrollInfo(ctrl.Handle, (int)scrollBarDirection, ref si))
                    return true;
            }
            return false;
        }

        void scrollHoz(IntPtr handle, int pixels)
        {
            scrollLB(handle);

            //ShowScrollBar(handle, (int)ScrollBarDirection.SB_HORZ, (bool)false);

            // Get current scroller posion

            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = (uint)Marshal.SizeOf(si);
            si.fMask = (uint)ScrollInfoMask.SIF_ALL;
            GetScrollInfo(handle, (int)ScrollBarDirection.SB_HORZ, ref si);

            // Increase posion by pixles
            si.nPos += pixels;
            if (si.nPos > (si.nMax - si.nPage) * 1.5) si.nPos = (int)((si.nMax - si.nPage) * 1.5);
            if (si.nPos < 0) si.nPos = 0;

            /*
            if (si.nPos < (si.nMax - si.nPage))
                si.nPos += pixels;
            else
            {
                SendMessage(handle, WM_HSCROLL, (IntPtr)ScrollBarCommands.SB_PAGERIGHT, IntPtr.Zero);
            }
             */

            // Reposition scroller
            SetScrollInfo(handle, (int)ScrollBarDirection.SB_HORZ, ref si, true);

            // Send a WM_HSCROLL scroll message using SB_THUMBTRACK as wParam
            // SB_THUMBTRACK: low-order word of wParam, si.nPos high-order word of wParam
            SendMessage(handle, WM_HSCROLL, (IntPtr)(ScrollBarCommands.SB_THUMBTRACK + 0x10000 * si.nPos), IntPtr.Zero);
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
            textBox1.Text = Regex.Replace(textBox1.Text, "^0+", "P");
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
            code = Regex.Replace(code, "^0+", "P");
           
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

                case "Left":
                    scrollHoz(listBox1.Handle, -200);
                    e.Handled = true;
                    return;
                    break;
                case "Right":
                    scrollHoz(listBox1.Handle, 200);
                    e.Handled = true;
                    return;
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
            string sql = string.Format(@"select top 1 mmInDtl.sSampleMaterialNo,mmInDtl.sOrderNo,mmFabric.sInNo from mmInDtl left join mmFabric on mmFabric.sFabricNo=mmInDtl.sFabricNo where mmInDtl.sFabricNo='{0}' or mmInDtl.sPackageNo='{0}'", code);

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
            lblOrder.Text = dt.Rows[0][1].ToString();
            lblFINo.Text = dt.Rows[0][2].ToString();
        }

        public void getPkgData(string code)
        {
            if (code.Length < 1) return;

            Data.pkgOrderList.Clear();

            DataTable dt = null;
            string sql = string.Format(@"select sMaterialDesc, mmInDtl.sColorNo as sColorNo, mmInDtl.sBatchNo as sBatchNo, 
ISNULL( min(mmInDtl.iPackageOrder), 0) as minPkgNo, ISNULL( max(mmInDtl.iPackageOrder), 0) as maxPkgNo
, count(*) as countPkg from mmInDtl 
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
                Data.pkgOrderList.Add(new pkgOrderClass(code, row[0].ToString(), row[1].ToString(), row[2].ToString(), int.Parse(row[3].ToString()), int.Parse(row[4].ToString()), int.Parse(row[5].ToString())));
            }

            updateLisBox_datasource();

            listBox1.Focus();

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

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            string updateMan = Prompt.ShowDialog("", "选择机台号(无需输入qc)");

            if(updateMan=="") return;

            updateMan = "qc" + updateMan.PadLeft(2, '0');

            textBox1.Text = Regex.Replace(textBox1.Text, "^0+", "P");
            string code = textBox1.Text;
            int codeType = code.StartsWith("P", StringComparison.CurrentCultureIgnoreCase) ? 1003 : 1004;

            DataTable dt = null;
            string sql = string.Format(@"DECLARE @report uniqueidentifier;
set @report=(select upbReportFormatDtlGUID from pbBillReportDefine left join mmInDtl on sBillNo=sOrderNo where sFabricNo='{0}' or sPackageNo='{0}');
exec sppbRegisterBillReportTask  '{0}', {1}, '{2}', NULL, NULL, NULL, @report, 1 ", code, codeType, updateMan);
            
            Debug.WriteLine(sql);

            return;

            try
            {
                dt = DB.Query(sql);
            }
            catch (Exception ex)
            {
                string str = "{@error@}" + ex.Message;
                MessageBox.Show(str);
                return;
            }
        }


    }
}