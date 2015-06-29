using System;

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
using System.Runtime.Serialization;


namespace barcode
{
    public partial class Form2 : Form
    {

        public folderClass folder = Data.folderList[Data.folderIndex];
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer inter2 = new System.Windows.Forms.Timer();

        public int count = 0;
        public string prevDebugStr = "";

        public bool commExited = false;

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
            WinCE.createMemFile("OK");


            inter2.Enabled = false;
            inter2.Interval = 3000; // 1 second
            inter2.Tick += delegate { if (inter2.Enabled) checkData(); };
            inter2.Enabled = true;

            this.Closing += exitApp;

            txtDebug.Visible = false;
            lv.Items.Clear();
        }



        public void exitApp() {
            inter2.Enabled = false;
            inter2.Dispose();
            WinCE.createMemFile("EXIT");
            if (commExited) WinCE.closeMemFile();
            //this.Close();
            Application.Exit();
        }

        public void exitApp(object obj, EventArgs e) {
            exitApp();
        }

        public void debug(string str) {
            if (!txtDebug.Visible) return;
            if (prevDebugStr == str) return;
            prevDebugStr = str;
            txtDebug.Text = str + "\r\n" + txtDebug.Text;
        }



        public void checkData()
        {
            string sql;
            string data = (WinCE.readMemFile());

            if (data == "EXIT")
            {
                debug("EXIT");
                Data.prevPutBuffer = "";
                return;

                commExited = true;
                exitApp();
                return;
            }

            if (data.StartsWith("<<<<")) return;

            if (data=="OK" && Data.putBuffer != "" && Data.prevPutBuffer!=Data.putBuffer ) {
                WinCE.createMemFile("<<<<" + Data.putBuffer);
                Data.prevPutBuffer = Data.putBuffer;
                debug("Send:"+Data.putBuffer);
                return;
            }

            if (data.StartsWith(">>>>"))
            {
                WinCE.createMemFile("OK");
                sql = data.Substring(4);
                debug("Get:"+sql);
                string [] lines = Regex.Split(sql, "{@head@}");
                string sn = lines[0];
                sql = lines[1];
                Data.dataListSN2.Add(sn, sql);
                
                updateLV2(sn, sql);
                return;
            }

            debug(data);
        }


        



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
                //BeginInvoke(new NewDel(getData));
                BeginInvoke(new Action<string>(getData), new object[] { sn });
            }
        }

        public void loadSN(string sn) 
        {


            debug("loadSN: "+sn);

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
                updateLV2(sn, Data.dataListSN2[ sn ] );
            }
            
        }

        public void getData(string sn)
        {
            DataTable dt=null;
            string wh = sn.StartsWith("P", StringComparison.CurrentCultureIgnoreCase) ? " mmindtl.sPackageNo='" + sn + "'" : " mmindtl.sFabricNo='" + sn + "'";
            try
            {
            dt = DB.Query(
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
mmInDtl.sFabricNo as sFabricNo,
mmInDtl.iFabricOrder as iFabricOrder,
mmInDtl.iPackageOrder as iPackageOrder
                from mmInDtl 
                left join mmFabric on mmFabric.sFabricNo = mmInDtl.sFabricNo 
                left join sdOrderHdr on sdOrderHdr.sOrderNo = mmInDtl.sOrderNo 
                left join mmMaterial on mmMaterial.uGUID = mmInDtl.ummMaterialGUID where " + wh
            );
            }
            catch (Exception e)
            {
                string str = "{@error@}" + e.Message;
                MessageBox.Show(str);
                return;
            }

            if (object.ReferenceEquals(null, dt)) return;
            
            Data.dataListSN.Add(sn, dt);

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

            lv.Update();

        }


        public void updateLV2(string sn, string sql)
        {

            if ( Data.prevSN2==sn ) return;

            Data.prevSN2 = sn;

            lv.Items.Clear();

            var thePack = Data.getCodeFromList(sn);


            if (sql == "")
            {
                thePack.OrderNo = "????";

            }
            else if (sql.StartsWith("{@error@}"))
            {
                thePack.OrderNo = "!!!!";

            }
            else
            {

                string[] lines = Regex.Split(sql, "{@row@}");
                debug("updateLV2:" + sn + " " + lines.Length.ToString());

                for (var i = 0; i < lines.Length; i++)
                {


                    string[] row = Regex.Split(lines[i], "{@column@}");

                    if (i == 0)
                    {
                        thePack.OrderNo = thePack.IsPackage ? row[11] : row[10];
                    }

                    thePack.addRow(row);

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

                foreach (ColumnHeader col in lv.Columns)
                {

                    col.Width = -1;
                }

            }

            updateLisBox();

            lblFolder.Text = folder.ToString();

        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            debug("Key:"+e.KeyValue.ToString()+" "+e.KeyCode.ToString() );

            switch (e.KeyCode.ToString())
            {
                case "F1":
                case "F4":
                    textBox1.Focus();
                    break;
                case "F2":
                    listBox1.Focus();
                    break;
                case "F3":
                    lv.Focus();
                    break;

                case "Return":
                    btnAdd_Click(sender, e);
                    break;
                case "Escape":
                    textBox1.Text = "";
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

            if (e.KeyValue == 13 || e.KeyValue == 115)
            {
                textBox1.Focus();
                e.Handled = true;

                return;
            }
            var val = (codeClass)listBox1.SelectedValue;
            var isDirty = false;

            //textBox1.Text = e.KeyCode.ToString();
            debug( e.KeyValue.ToString() + " " + e.KeyCode.ToString());

            switch (e.KeyCode.ToString())
            {
                case "F1":
                case "F4":
                case "Return":
                    textBox1.Focus();
                    break;
                case "F2":
                    listBox1.Focus();
                    break;
                case "F3":
                    lv.Focus();
                    break;


                case "Delete":
                    Data.codeList.RemoveAt(idx);
                    isDirty = true;
                    e.Handled = true;
                    break;
                case "Space":
                    

                    break;

                case "Tab":
                    textBox1.Focus();
                    e.Handled = true;
                    break;
                case "Up":
                case "Left":
                    e.Handled = true;
                    listBox1.SelectedIndex = Math.Max( listBox1.SelectedIndex-1, 0 );
                    break;
                case "Down":
                case "Right":
                    e.Handled = true;
                    listBox1.SelectedIndex = Math.Min(listBox1.SelectedIndex + 1, listBox1.Items.Count-1 );
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

            if (listBox1.Enabled && Data.prevSN != curID)
            {
                loadSN(curID);
            }
            Data.prevSN = curID;
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

            Data.codeList.Insert(0,code);

            updateLisBox();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) idx = Data.codeList.Count - 1;
            addLisBox(new codeClass( textBox1.Text, folder.Id ));
            //textBox1.Focus();
            textBox1.Text = "";
            listBox1.SelectedIndex = 0;
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
            var prevFocus = listBox1.Focused;

            listBox1.Enabled = false;
            listBox1.DataSource = null;
            listBox1.DataSource = Data.codeList;
            listBox1.Enabled = true;

            try
            {
                listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.codeList.Count ? prevIndex : 0;
            }catch(Exception e){
                
            }
            if (prevFocus) listBox1.Focus();
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
            switch (e.KeyCode.ToString())
            {
                case "F1":
                case "F4":
                case "Return":
                    textBox1.Focus();
                    break;
                case "F2":
                    listBox1.Focus();
                    break;
                case "F3":
                    lv.Focus();
                    break;

                case "Space":
                    Point pos = GetItemPosition(2);
                    if(pos.X<0)
                        ScrollHorizontal(-lv.Bounds.Width*100);
                    else
                        ScrollHorizontal(lv.Bounds.Width-40);

                    break;
                case "Left":
                    ScrollHorizontal( - 200);
                    break;
                case "Right":
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

        private void textBox1_ScanerDataReceivedEvent()
        {
            MessageBox.Show("SS");
        }


    }
}