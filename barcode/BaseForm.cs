using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace barcode
{
    public enum KeyValueMap : int
    {
        //define your scanner key
        VK_LEFT_SIDE_SCAN = 133,  //左侧扫描
        VK_LEFT_SCAN = 134,       //左边扫描
        VK_RIGHT_SCAN = 135,      //右边扫描
    }

    public enum _SCAN_DATA_SUFFIX
    {
        NONE_SUFFIX = -1,
        STRING_SUFFIX = 0,
        NEWLINE_SUFFIX = 10,
        ENTER_SUFFIX = 13,
        SPACE_SUFFIX = 32,
    }

    public class BaseForm : Form
    {
        public static CustomMessageHandler m_msgAgent;
        private System.ComponentModel.IContainer components = null;

        //api
        [DllImport("lib_scanner.dll", EntryPoint = "ScannerAutoInit")]
        public static extern void ScannerAutoInit();

        [DllImport("lib_scanner.dll", EntryPoint = "ScannerAutoExit")]
        public static extern void ScannerAutoExit();

        [DllImport("lib_scanner.dll", EntryPoint = "ScannerTriggerOn")]
        public static extern void ScannerTriggerOn(IntPtr hWnd, int NotifyMessageId);

        [DllImport("lib_scanner.dll", EntryPoint = "ScannerTriggerOff")]
        public static extern void ScannerTriggerOff();

        [DllImport("lib_scanner.dll", EntryPoint = "GetScanBarcode")]
        public static extern int GetScanBarcode(byte[] barcode_data_buf, int max_len);

        [DllImport("lib_scanner.dll", EntryPoint = "StandardKeyboradOut")]
        public static extern void StandardKeyboradOut(byte[] BarcodeData, Int32 Len, _SCAN_DATA_SUFFIX DataSuffixType);

        public BaseForm()
        {
            this.InitializeComponent();
            BaseForm.m_msgAgent = new CustomMessageHandler(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool GetBarCodeRawData(out byte[] BarCodeData, ref int nLength)
        {
            if (nLength <= 0)
            {
                BarCodeData = null;
                nLength = 0;
                return false;
            }

            int dwSizeRet = 0;
            int cbSize = 0x40;
            BarCodeData = new byte[cbSize];
            dwSizeRet = BaseForm.GetScanBarcode(BarCodeData, cbSize);
            if (dwSizeRet == 0xea)
            {
                cbSize = ((dwSizeRet / 0x20) + 1) * 0x20;
                if (cbSize > 0x1000)
                {
                    cbSize = 0x1000;
                }
                BarCodeData = new byte[cbSize];
                dwSizeRet = BaseForm.GetScanBarcode(BarCodeData, cbSize);
            }
            if ((dwSizeRet > 0))
            {
                nLength = (int)dwSizeRet;
                return true;
            }
            BarCodeData = null;
            nLength = 0;
            return false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.KeyPreview = true;
            this.Name = "BaseForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BaseForm_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseForm_KeyDown);
            this.ResumeLayout(false);
        }

        //在子类中override这个方法,用于接收扫描数据和长度
        public virtual void OnBarCodeNotify(byte[] BarCodeData, int nLength)
        {

        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue == (int)KeyValueMap.VK_LEFT_SIDE_SCAN || e.KeyValue == (int)KeyValueMap.VK_LEFT_SCAN
                || e.KeyValue == (int)KeyValueMap.VK_RIGHT_SCAN
                ))
            {
                BaseForm.ScannerTriggerOn(m_msgAgent.Hwnd, m_msgAgent.GetBarCodeNotifyMessageId());
            }
        }

        private void BaseForm_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue == (int)KeyValueMap.VK_LEFT_SIDE_SCAN || e.KeyValue == (int)KeyValueMap.VK_LEFT_SCAN
                || e.KeyValue == (int)KeyValueMap.VK_RIGHT_SCAN
                ))
            {
                BaseForm.ScannerTriggerOff();
            }
        }
    }

    //用于接收系统消息
    public class CustomMessageHandler : MessageWindow
    {
        private BaseForm _BaseForm;
        public const uint WM_BARCODE_NOTIFY = 0x40c;
        public const uint WM_USER = 0x400;

        public CustomMessageHandler(BaseForm Frm)
        {
            this._BaseForm = Frm;
        }

        public int GetBarCodeNotifyMessageId()
        {
            return 0x40c;
        }

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case (int)0x40C:
                    {
                        byte[] buffer;
                        int wParam = (int)msg.WParam + 1;
                        uint lParam = (uint)((int)msg.LParam);

                        BaseForm.ScannerTriggerOff();

                        if (this._BaseForm.GetBarCodeRawData(out buffer, ref wParam))
                        {
                            this._BaseForm.OnBarCodeNotify(buffer, wParam);
                        }
                        msg.Result = IntPtr.Zero;
                    }
                    break;
            }

            base.WndProc(ref msg);
        }
    }
}
