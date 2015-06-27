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
            inter1.Interval = 1000; // 1 second
            inter1.Tick += delegate { checkData(); };
            inter1.Enabled = true;

        }

        public void checkData()
        {
            string SN;
            
            string data = (WinCE.readMemFile());

            if (data == "OK" && Data.putBuffer != "")
            {
                WinCE.createMemFile(">>>>" + Data.putBuffer);
                Data.putBuffer = "";
                return;
            }

            if (data.StartsWith("<<<<") && Data.curSN=="")
            {
                debug(data);
                //WinCE.createMemFile("OK");
                SN = data.Substring(4);
                debug(SN);
                getDataAsync(SN);
                return;
            }

            txtDebug.Text = data;
            //txtDebug.Text = (data == "").ToString();
            //if (data.Length > 0) txtDebug.Text += (data == "OK").ToString() + data + ": " + ((int)data[0]).ToString() + " " + ((int)data[data.Length - 1]).ToString();
    
        }

        public void debug(string str) {
            WinCE.createMemFile(str);
        }


        public void launchApp() {

            MobileLaunch.LaunchApp("\\Program Files\\barcode\\barcode.exe", "");

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
                updateLV(Data.dataListSN[sn]);
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
                    @"select mmindtl.sPackageNo, sdOrderHdr.sMaterialDesc as sCode, mmMaterial.sMaterialName as sName, mmInDtl.sColorNo as sColorNo, mmInDtl.nACPrice as nPrice,
                mmInDtl.sBatchNo as sBatch, mmFabric.sFactWidth as sWidth,  mmFabric.sUnit as sUnit, mmFabric.nNetWeight as nWeight,
                mmFabric.nQty as nQty,mmInDtl.sFabricNo as sFabricNo from mmInDtl 
                left join mmFabric on mmFabric.sFabricNo = mmInDtl.sFabricNo 
                left join sdOrderHdr on sdOrderHdr.sOrderNo = mmInDtl.sOrderNo 
                left join mmMaterial on mmMaterial.uGUID = mmInDtl.ummMaterialGUID where mmindtl.sPackageNo='" + sn + "'"
                );
            }
            catch (Exception e) {
                string str = "ERROR:" + e.Message;
                //WinCE.createMemFile(str);
                return;
            }

            Data.dataListSN.Add(sn, dt);

            updateLV(dt);
        }


        public void updateLV(DataTable dt) {


            List<string> dtRow = new List<string> { };
            List<string> dtTable = new List<string> { };

            if (dt.Rows.Count == 0) return;

            string sn = dt.Rows[0][0].ToString();

            dtTable.Add(sn);
            foreach (DataRow row in dt.Rows)
            {
                for (var j=1; j<dt.Columns.Count; j++) {
                    dtRow.Add(row[j].ToString());
                }

                dtTable.Add(string.Join("|**|", dtRow.ToArray()));
            }

            string str = ">>>>" + string.Join("@&&@", dtTable.ToArray());

            txtDebug.Text = str;

            Data.putBuffer = str;

        }


        public void showMsg(string str)
        {

        }
    }
}