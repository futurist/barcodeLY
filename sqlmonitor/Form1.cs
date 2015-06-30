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
using System.Text.RegularExpressions;

namespace sqlmonitor
{
    public partial class Form1 : Form
    {
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public string prevDebugStr = "";

        public bool commExited = false;

        public Form1()
        {
            InitializeComponent();

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;


            //textBox1.Text = (WinCE.readMemFile());
            
            launchApp();


            WinCE.createMemFile("OK");

            //getDataAsync("P1506160166");

            inter1.Enabled = false;
            inter1.Interval = 500; // 1 second
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

            if (data == "EXIT")
            {
                debug("EXIT");
                Data.prevPutBuffer = "";
                return;
                
                commExited = true;
                exitApp();
                return;
            }

            if (data.StartsWith(">>>>")) return;

            if (data == "OK" && Data.putBuffer != "" && Data.prevPutBuffer!=Data.putBuffer )
            {
                WinCE.createMemFile(">>>>" + Data.putBuffer);
                Data.prevPutBuffer = Data.putBuffer;
                debug("Send:" + Data.putBuffer);
                return;
            }

            if (data.StartsWith("<<<<"))
            {

                string[] SNs = Regex.Split(data.Substring(4), "{@sn@}");

                Data.putBuffer = getManyData(SNs);
                WinCE.createMemFile("OK");

                //debug("Get:"+SN+" "+SN.Length.ToString());
                //getDataAsync(SN);

                return;
            }


            debug(Data.curSN+" "+ data);
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
            if (commExited) WinCE.closeMemFile();
            //MobileLaunch.exitApp();
            Application.Exit();
        }

        private void exitApp(object sender, EventArgs e) {
            exitApp();
        }



        // 声明一个委托 
        private delegate void NewDel();

        // 创建一个 新线程的方法
        public void beginGetData()
        {
            //Thread thread;
            //ThreadStart threadstart = new ThreadStart(invokeGetData);
            //thread = new Thread(threadstart);
            //thread.IsBackground = true;
            //thread.Start();
        }

        // 屏蔽错误的方法   说白了 就是通过了一个 委托  
        // 解决Control.Invoke 必须用于与在独立线程上创建的控件交互。
        private void invokeGetData(string sn)
        {
            if (InvokeRequired)
            {
                // 要 努力 工作的 方法
                //BeginInvoke(new NewDel(getData));
                //BeginInvoke( new Action<string>(getData), new object[] { sn });

            }
        }

        public void getDataAsync(string sn)
        {

            if (!Data.dataListSN.ContainsKey(sn))
            {
                //beginGetData();

                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate(object state)
                    { invokeGetData(sn); }), null
                );
            }
            else
            {
                updateLV(sn, Data.dataListSN[sn]);
            }

        }

        public string getManyData(string[] SNs) {

            List<string> ret = new List<string> { };
            foreach (string sn in SNs) {
                ret.Add(getData(sn));
            }
            return string.Join("{@record@}", ret.ToArray() );

        }

        public string getData(string sn)
        {
            DataTable dt = null;

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
                left join mmMaterial on mmMaterial.uGUID = mmInDtl.ummMaterialGUID where "+wh
                );
            }
            catch (Exception e) {
                string str = "{@error@}" + e.Message;
                //MessageBox.Show(str);
                //Data.putBuffer = str;
                return "";
            }

            Data.dataListSN.Add(sn, dt);

            return updateLV(sn, dt);
        }
        

        public string updateLV(string sn, DataTable dt) {


            List<string> dtTable = new List<string> { };
            
            foreach (DataRow row in dt.Rows)
            {
                List<string> dtRow = new List<string> { };
                for (var j=0; j<dt.Columns.Count; j++) {
                    dtRow.Add(row[j].ToString());
                }

                dtTable.Add( string.Join("{@column@}", dtRow.ToArray()) );
            }

            string head = (sn + "{@head@}");
            string str = head + string.Join("{@row@}", dtTable.ToArray());

            txtDebug.Text = str;

            Data.putBuffer = str;

            return str;

        }


        public void showMsg(string str)
        {

        }
    }
}