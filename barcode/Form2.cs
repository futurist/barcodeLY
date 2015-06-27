﻿using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neolix.Device;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading;
using System.Text.RegularExpressions;


namespace SmartDeviceProject1
{
    public partial class Form2 : Form
    {

        public folderClass folder = Data.folderList[Data.folderIndex];
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer inter2 = new System.Windows.Forms.Timer();

        public Form2()
        {
            InitializeComponent();

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;


            textBox1.Text = folder.Id;
            
            this.lblDuplicate.Hide();
            this.lblDuplicate.BackColor = Color.Transparent;

            
            this.listBox1.DataSource = Data.codeList;



            //textBox2.Text = (WinCE.readMemFile());

            inter2.Enabled = false;
            inter2.Interval = 100; // 1 second
            inter2.Tick += delegate { checkData(); };
            inter2.Enabled = true;



        }



        public void checkData()
        {
            string sql;
            string data = (WinCE.readMemFile());

            if (data.StartsWith("11OK") && Data.putBuffer != "") {
                txtDebug.Text = Data.putBuffer;
                WinCE.createMemFile("11<<<<" + Data.putBuffer);
                Data.putBuffer = "";
                return;
            }

            if (data.StartsWith(">>>>"))
            {
                WinCE.createMemFile("OK");
                sql = data.Substring(4);
                string [] lines = Regex.Split(sql, "@@@@");
                Data.dataListSN2.Add(lines[0], sql);
                if (lines[0]==Data.curSN) updateLV2(sql);
                return;
            }

            char[] array = data.ToCharArray();
            string d="";
            // Loop through array.
            for (int i = 0; i < array.Length; i++)
            {
                // Get character from array.
                 d += ((int)array[i]).ToString() + " ";
            }

            txtDebug.Text = d;
        }


        Scaner scaner = new Scaner();



        // 声明一个委托 
        private delegate void NewDel();

        // 创建一个 新线程的方法
        public void beginGetData()
        {
            Thread thread;
            ThreadStart threadstart = new ThreadStart(invokeGetData);
            thread = new Thread(threadstart);
            thread.IsBackground = true;
            thread.Start();
        }

        // 屏蔽错误的方法   说白了 就是通过了一个 委托  
        // 解决Control.Invoke 必须用于与在独立线程上创建的控件交互。
        private void invokeGetData()
        {
            if (InvokeRequired)
            {
                // 要 努力 工作的 方法
                BeginInvoke(new NewDel(getData));
            }
        }

        public void loadSN(string sn) {
            
            Data.curSN = sn;

            if (!Data.dataListSN2.ContainsKey(sn))
            {
                //beginGetData();

                //ThreadPool.QueueUserWorkItem(
                //    new WaitCallback(delegate(object state)
                //    { invokeGetData(); }), null
                //);


                Data.putBuffer = sn;

            }
            else 
            {
                updateLV2( Data.dataListSN2[ sn ] );
            }
            
        }

        public void getData( ) {

            DataTable dt = DB.Query(
                @"select 
sdOrderHdr.sMaterialDesc as sCode,
mmMaterial.sMaterialName as sName,
mmInDtl.sColorNo as sColorNo, 
mmInDtl.nACPrice as nPrice,
mmInDtl.sBatchNo as sBatch, 
mmFabric.sFactWidth as sWidth,  
mmFabric.sUnit as sUnit, 
mmFabric.nNetWeight as nWeight,
mmFabric.nQty as nQty,
mmInDtl.sFabricNo as sFabricNo 
                from mmInDtl 
                left join mmFabric on mmFabric.sFabricNo = mmInDtl.sFabricNo 
                left join sdOrderHdr on sdOrderHdr.sOrderNo = mmInDtl.sOrderNo 
                left join mmMaterial on mmMaterial.uGUID = mmInDtl.ummMaterialGUID where mmindtl.sPackageNo='"+ Data.curSN +"'"
            );

            if (object.ReferenceEquals(null, dt)) return;
            
            Data.dataListSN.Add(Data.curSN, dt);

            folder.TotalRoll += dt.Rows.Count;

            lblNum.Text = "品种：" + Data.folderList[Data.folderIndex].TotalRoll.ToString();

            updateLV(dt);
        }

        public void updateLV(DataTable dt) {

            lv.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {

                ListViewItem item = new ListViewItem(row["sCode"].ToString());

                var BatchNo = row["sBatch"].ToString();
                BatchNo = (BatchNo != "" ? "[" + BatchNo + "]" : "");

                string num = row["sUnit"].ToString().ToUpper() == "KG" ? String.Format("{0:0.0}", row["nWeight"]) : String.Format("{0:0.0}", row["nQty"]);

                item.SubItems.Add(row["sName"].ToString());
                item.SubItems.Add(row["sColorNo"].ToString() + BatchNo);
                item.SubItems.Add(num + row["sUnit"]);
                item.SubItems.Add(row["sFabricNo"].ToString());

                lv.Items.Add(item);

            }

            foreach (ColumnHeader col in lv.Columns)
            {
                col.Width = -1;
            }

        }


        public void updateLV2(string sql)
        {
            string[] lines = Regex.Split(sql, "@&&@");
            string sn = lines[0];
            for (var i = 1; i < lines.Length; i++) {

                string[] row = Regex.Split(lines[i], "|**|");

                ListViewItem item = new ListViewItem(row[0].ToString());

                var BatchNo = row[4].ToString();
                BatchNo = (BatchNo != "" ? "[" + BatchNo + "]" : "");

                string num = row[6].ToString().ToUpper() == "KG" ? String.Format("{0:0.0}", row[7]) : String.Format("{0:0.0}", row[8]);

                item.SubItems.Add(row[1].ToString());
                item.SubItems.Add(row[2].ToString() + BatchNo);
                item.SubItems.Add(num + row[6]);
                item.SubItems.Add(row[9].ToString());

                lv.Items.Add(item);
            }

        }


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

            switch (e.KeyCode.ToString())
            {
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


        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            var val = (codeClass)listBox1.SelectedValue;
            var isDirty = false;

            //textBox1.Text = e.KeyCode.ToString();

            switch (e.KeyCode.ToString())
            {
                case "Delete":
                    Data.codeList.RemoveAt(idx);
                    isDirty = true;
                    e.Handled = true;
                    break;
                case "Space":
                    

                    break;
                case "Return":
                    if (listBox1.SelectedIndex == -1) return;
                    e.Handled = true;
                    break;
                case "Tab":
                    textBox1.Focus();
                    e.Handled = true;
                    break;
                default:
                    
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

            if (idx == -1) return;

            string curID = ((codeClass)listBox1.SelectedItem).Id;

            if (listBox1.Enabled && Data.curSN != curID)
            {
                loadSN(curID);
            }
        }

        
        public void addLisBox(codeClass code)
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

            Data.codeList.Add(code);

            updateLisBox();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) idx = Data.codeList.Count - 1;
            addLisBox(new codeClass( textBox1.Text, folder.Id ));
            //textBox1.Focus();
            textBox1.Text = "";
            listBox1.SelectedIndex = Data.codeList.Count - 1;
            textBox1.Focus();
            
        }


        public void showDuplicateMsg(codeClass code)
        {
            lblDuplicate.Show();
            lblDuplicate.Text = code.Id + "\r\n" + "已存在";

            inter1.Enabled = false;
            inter1.Interval = 1000; // 1 second
            inter1.Tick += delegate { lblDuplicate.Hide(); inter1.Enabled = false; };
            inter1.Enabled = true;

        }

        public void updateLisBox()
        {
            var prevIndex = listBox1.SelectedIndex;

            listBox1.Enabled = false;
            listBox1.DataSource = null;
            listBox1.DataSource = Data.codeList;
            listBox1.Enabled = true;

            listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.codeList.Count ? prevIndex : Data.codeList.Count - 1;
            
        }

        public bool checkDuplicate(string ID)
        {
            foreach (var code in Data.codeList)
            {
                if (ID == code.Id) return true;
            }
            return false;
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            Form2_GotFocus(sender, e);
            //this.Paint += Form2_GotFocus;
            textBox1.GotFocus += this.Form2_GotFocus;


            //ListViewItem item = new ListViewItem("iowejfojweof");
            //item.SubItems.Add("iojw34534435345345eof");
            //item.SubItems.Add("iojw345345345345345eof");
            //item.SubItems.Add("iojwe345345345345of");
            //item.SubItems.Add("iojweof");
            //lv.Items.Add(item);
            // item = new ListViewItem("iowejfojweof");
            //item.SubItems.Add("iojw34534435345345eof");
            //item.SubItems.Add("iojw345345345345345eof");
            //item.SubItems.Add("iojwe345345345345of");
            //item.SubItems.Add("iojweof");
            //lv.Items.Add(item);
            // item = new ListViewItem("iowejfojweof");
            //item.SubItems.Add("iojw34534435345345eof");
            //item.SubItems.Add("iojw345345345345345eof");
            //item.SubItems.Add("iojwe345345345345of");
            //item.SubItems.Add("iojweof");
            //lv.Items.Add(item);
            // item = new ListViewItem("iowejfojweof");
            //item.SubItems.Add("iojw34534435345345eof");
            //item.SubItems.Add("iojw345345345345345eof");
            //item.SubItems.Add("iojwe345345345345of");
            //item.SubItems.Add("iojweof");
            //lv.Items.Add(item);
            // item = new ListViewItem("iowejfojweof");
            //item.SubItems.Add("iojw34534435345345eof");
            //item.SubItems.Add("iojw345345345345345eof");
            //item.SubItems.Add("iojwe345345345345of");
            //item.SubItems.Add("iojweof");
            //lv.Items.Add(item);
           
            //foreach(ColumnHeader col in lv.Columns){
            //    col.Width = -1;
            //}

            lblFolder.Text = folder.Id;



        }








        // to scroll horizontally 



        const int MARGIN = 20;

        /// <summary>
        /// native windows message to scroll the listview.
        /// </summary>
        const Int32 LVM_FIRST = 0x1000;
        const Int32 LVM_SCROLL = LVM_FIRST + 20;
        const Int32 LVM_GETITEMPOSITION = (LVM_FIRST + 16);

        [DllImport("coredll")]
        static extern IntPtr SendMessage(IntPtr Handle, Int32 msg, IntPtr wParam,
        IntPtr lParam);

        [DllImport("coredll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, out Point position);


        private void ScrollHorizontal(int pixelsToScroll)
        {
            SendMessage(lv.Handle, LVM_SCROLL, (IntPtr)pixelsToScroll,IntPtr.Zero);
        }
        private Point GetItemPosition(int idx)
        {
            Point point = Point.Empty;
            SendMessage(lv.Handle, LVM_GETITEMPOSITION, (IntPtr)idx, out point);
            //textBox1.Text = point.X.ToString();
            return point;
        }

        /// <summary>
        /// Ensure visible of a ListViewItem and SubItem Index.
        /// 
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="subItemIndex"></param>
        public void EnsureVisible(ListViewItem item, int subItemIndex)
        {
            if (item == null || subItemIndex > item.SubItems.Count - 1)
            {
                throw new ArgumentException();
            }

            // scroll to the item row.
            lv.EnsureVisible(item.Index);
            //Rectangle bounds = item.SubItems[subItemIndex].Bounds;

            // need to set width from columnheader, first subitem includes
            // all subitems.
            //bounds.Width = this.Columns[subItemIndex].Width;

            Rectangle bounds = lv.Bounds;
            bounds.X = bounds.Width;
            bounds.Y = item.Index * 25;

            ScrollToRectangle(bounds);
        }

        /// <summary>
        /// Scrolls the listview.
        /// </summary>
        /// <param name="bounds"></param>
        private void ScrollToRectangle(Rectangle bounds)
        {
            int scrollToLeft = bounds.X + bounds.Width + MARGIN;
            if (scrollToLeft > lv.Bounds.Width)
            {
                this.ScrollHorizontal(scrollToLeft - lv.Bounds.Width);
            }
            else
            {
                int scrollToRight = bounds.X - MARGIN;
                if (scrollToRight < 0)
                {
                    this.ScrollHorizontal(scrollToRight);
                }
            }
        }

        private void lv_KeyDown(object sender, KeyEventArgs e)
        {
            switch( e.KeyCode.ToString() ){
                case "Space":
                    Point pos = GetItemPosition(2);
                    if(pos.X<0)
                        ScrollHorizontal(-lv.Bounds.Width*100);
                    else
                        ScrollHorizontal(lv.Bounds.Width-40);

                    break;
                case "D1":
                    ScrollHorizontal( - 200);
                    break;
                case "D2":
                    ScrollHorizontal(200);
                    break;

            }
        }

        private void lblFolder_Click(object sender, EventArgs e)
        {
            this.Hide();
        }



        private void Form2_GotFocus(object sender, EventArgs e)
        {
            Data.curForm = this;
            showMsg("");
        }

        public void showMsg(string str)
        {
            lblStatus.Text = str;
        }


    }
}