using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Neolix.Device;
using barcode.Properties;
using Neolix.WinCE.Common;

namespace barcode
{
    public delegate bool InputCheck(string code);
    public delegate string PreInputCheck(string code);
    public partial class ScanTextBox : TextBox
    {
        public ScanTextBox()
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                scaner.Open();
                scaner.ScanerDataReceived += ScanerDataReceived;
            }
            MaxLength = 13;
        }
        public event EventHandler ScanFinish;

        public void InvokeScanFinish(EventArgs e)
        {
            EventHandler handler = ScanFinish;
            if (handler != null)
            {
                barcode.Form1.MethodInvoker mi = delegate
                {
                    handler(this, e);
                };
                if (InvokeRequired)
                {
                    Invoke(mi);
                }
                else
                {
                    mi();
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            //SelectionStart = Text.Length;
            base.SelectAll();
        }
        /// <summary>扫描头</summary>
        private readonly Scaner scaner = new Scaner();

        /// <summary>
        /// return true if barcode is valid
        /// </summary>
        public InputCheck InputCheck;
        public PreInputCheck PreInputCheck;

        private static SoundPlayer okHint;
        private static SoundPlayer errorHint;

        static void OkHint()
        {
            if (okHint == null)
            {
                okHint = new SoundPlayer(Resources.ok);
            }

            okHint.Play();
        }

        static void ErrorHint()
        {
            if (errorHint == null)
            {
                errorHint = new SoundPlayer(Resources.error);
            }
            errorHint.Play();
        }

        private string textValue = string.Empty;

        public override string Text
        {
            get
            {
                return textValue;
            }
            set
            {
                
                    base.Text = value;
                
                textValue = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            textValue = base.Text;
            base.OnTextChanged(e);
        }

        private void ScanerDataReceived(object sender, string code)
        {
            barcode.Form1.MethodInvoker mi = delegate
            {
                int maxlen = MaxLength;
                if (maxlen == 13)
                {
                    maxlen = 24;
                }
                if (code.Length > maxlen)
                {
                    ErrorHint();
                    return;
                }

                if (PreInputCheck != null)
                {
                    code = PreInputCheck(code);
                }
                if (InputCheck != null && !InputCheck(code))
                {
                    ErrorHint();
                }
                else
                {
                    OkHint();
                    Text = code;
                    SelectionStart = Text.Length;
                    if (ScanFinish != null)
                    {
                        InvokeScanFinish(EventArgs.Empty);
                    }
                }
            };
            Invoke(mi);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F22 ||
                e.KeyCode == Keys.F23 ||
                e.KeyCode == Keys.F24)
            {
                scaner.Read();
                //scaner.Read();
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F22 ||
                e.KeyCode == Keys.F23 ||
                e.KeyCode == Keys.F24)
            {
                Scaner.OpenScanerLaser(false);
            }
            base.OnKeyUp(e);
        }

        private uint WM_CHAR = 0x102;

        [DllImport("coredll")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);


        public void OnUserInputKeyPress(char ch)
        {
            SendMessage(Handle, WM_CHAR, ch, 0);
        }

    }
}
