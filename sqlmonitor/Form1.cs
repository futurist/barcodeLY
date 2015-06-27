using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace sqlmonitor
{
    public partial class Form1 : Form
    {
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public string prevDebugStr = "";

        public Form1()
        {
            InitializeComponent();

            this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;


            //textBox1.Text = (WinCE.readMemFile());
            
            launchApp();


            WinCE.createMemFile("OK");

            //getDataAsync("P1506160166");

            inter1.Enabled = false;
            inter1.Interval = 3000; // 1 second
            inter1.Tick += delegate { checkData(); };
            inter1.Enabled = true;

        }

        public void debug(string str)
        {
            if (prevDebugStr == str) return;
            prevDebugStr = str;
            txtDebug.Text = str + "\r\n" + txtDebug.Text;
        }

        public void checkData()
        {
            string SN;
            
            string data = (WinCE.readMemFile());

            if (data.StartsWith(">>>>")) return;

            if (data == "OK" && Data.putBuffer != "" && Data.prevPutBuffer!=Data.putBuffer )
            {
                WinCE.createMemFile(">>>>" + Data.putBuffer);
                Data.prevPutBuffer = Data.putBuffer;
                debug("Send:" + Data.putBuffer);
                return;
            }

            if (data.StartsWith("<<<<") && Data.curSN=="")
            {
                WinCE.createMemFile("OK");
                SN = data.Substring(4);
                debug("Get:"+SN);
                getDataAsync(SN);
                return;
            }

            if (data == "EXIT") {
                exitApp();
                return;
            }

            debug(data);
            //txtDebug.Text = (data == "").ToString();
            //if (data.Length > 0) txtDebug.Text += (data == "OK").ToString() + data + ": " + ((int)data[0]).ToString() + " " + ((int)data[data.Length - 1]).ToString();
    
        }


        public void launchApp() {

            MobileLaunch.LaunchApp("\\Program Files\\barcode\\barcode.exe", "");

            this.Closing += exitApp;

        }

        public void exitApp() {
            inter1.Enabled = false;
            WinCE.createMemFile("EXIT");
            MobileLaunch.exitApp();
        }

        private void exitApp(object sender, EventArgs e) {
            exitApp();
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
                BeginInvoke(new NewDel(getData));
            }
        }

        public void getDataAsync(string sn)
        {

            Data.curSN = sn;

            if (!Data.dataListSN.ContainsKey(sn))
            {
                //beginGetData();

                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate(object state)
                    { invokeGetData(); }), null
                );
            }
            else
            {
                updateLV(sn, Data.dataListSN[sn]);
            }

        }

        public void getData()
        {
            DataTable dt = null;
            string sn = Data.curSN;
            Data.curSN = ""; //可以进行下一次查询了
            try
            {
                dt = DB.Query(
                    @"select sdOrderHdr.sMaterialDesc as sCode, mmMaterial.sMaterialName as sName, mmInDtl.sColorNo as sColorNo, mmInDtl.nACPrice as nPrice,
                mmInDtl.sBatchNo as sBatch, mmFabric.sFactWidth as sWidth,  mmFabric.sUnit as sUnit, mmFabric.nNetWeight as nWeight,
                mmFabric.nQty as nQty,mmInDtl.sFabricNo as sFabricNo from mmInDtl 
                left join mmFabric on mmFabric.sFabricNo = mmInDtl.sFabricNo 
                left join sdOrderHdr on sdOrderHdr.sOrderNo = mmInDtl.sOrderNo 
                left join mmMaterial on mmMaterial.uGUID = mmInDtl.ummMaterialGUID where mmindtl.sPackageNo='" + sn + "'"
                );
            }
            catch (Exception e) {
                string str = "ERROR:" + e.Message;
                MessageBox.Show(str);
                //WinCE.createMemFile(str);
                return;
            }

            Data.dataListSN.Add(sn, dt);

            updateLV(sn, dt);
        }


        public void updateLV(string sn, DataTable dt) {


            List<string> dtRow = new List<string> { };
            List<string> dtTable = new List<string> { };


            
            foreach (DataRow row in dt.Rows)
            {
                for (var j=1; j<dt.Columns.Count; j++) {
                    dtRow.Add(row[j].ToString());
                }

                dtTable.Add(string.Join("{@column@}", dtRow.ToArray()));
            }

            string head = (sn + "{@head@}");
            string str = ">>>>" + head + string.Join("{@row@}", dtTable.ToArray());

            txtDebug.Text = str;

            Data.putBuffer = str;

        }


        public void showMsg(string str)
        {

        }
    }
}