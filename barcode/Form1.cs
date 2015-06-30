using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neolix.Device;
using System.Runtime.InteropServices;

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
            
            this.Activated += new EventHandler(Form1_Activated);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            
            scrollLB(listBox1.Handle);
            
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
            
            SendMessage(hWndListBox, LB_SETHORIZONTALEXTENT, (IntPtr)620, IntPtr.Zero);
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
            if (si.nPos > (si.nMax - si.nPage)) si.nPos = (int)(si.nMax - si.nPage);
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




        void btnUpload_Click(object sender, EventArgs e)
        {
            Data.Upload();
        }

        void Form1_Activated(object sender, EventArgs e)
        {
            Data.curForm = this;
        }

       
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch(e.KeyCode.ToString()){
                case "F1":
                    textBox1.Focus();
                    break;
                case "F2":
                    listBox1.Focus();
                    break;
                case "Return":
                    btnAdd_Click(sender, e);
                    break;
                case "Escape":
                    textBox1.Text = "";
                    break;
                default:
                    //scrollHoz(listBox1.Handle, 50 );
                    
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
            switch (e.KeyCode.ToString())
            {
                case "F1":
                    textBox1.Focus();
                    break;
                case "F2":
                    listBox1.Focus();
                    break;
                case "Escape":
                    textBox1.Focus();
                    break;
                case "Left":
                    scrollHoz( listBox1.Handle, -100 );
                    e.Handled = true;
                    return;
                    break;
                case "Right":
                    scrollHoz(listBox1.Handle, 100);
                    e.Handled = true;
                    return;
                    break;
            }
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            var val = (folderClass)Data.folderList[ listBox1.SelectedIndex ];
            var isDirty = false;

            //textBox1.Text = e.KeyCode.ToString();

            switch (e.KeyCode.ToString()) {
                
                case "Back":
                case "Delete":
                    DialogResult dialogResult = MessageBox.Show("确定要删除当前选择下的所有记录?", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Data.deleteFolder(val.Id, idx);
                        isDirty = true;
                    }
                    break;
                case "F4":
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
                    
                    //isDirty = true;
                    break;
            }
            
            updateLisBox();

            if (isDirty)
            {
                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            Data.folderIndex  = idx;
            if (idx == -1) return;
            
            string curID = ((folderClass) Data.folderList[ listBox1.SelectedIndex ]).Id;
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

            Data.modifyFolder( folder.Id, idx );
            
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
            //this.Hide();
            if (!Data.formList.ContainsKey(folderID))
            {
                Data.formList.Add(folderID, new Form2( this  ) );
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
            updateLisBox();

            showMsg("");
        }

        private void listBox1_EnabledChanged(object sender, EventArgs e)
        {
            //if(listBox1.Enabled) updateLisBox();
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