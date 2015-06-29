using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neolix.Device
{
    public delegate void MethodInvoker();

    public partial class ScanTextBox : TextBox
    {
        private Scaner scaner;
        public delegate void ScanerDataReceived(); 
        public event ScanerDataReceived ScanerDataReceivedEvent;

        public ScanTextBox()
        {
            InitializeComponent();
            try
            {
                scaner = new Scaner();
                scaner.ScanerDataReceived += scaner_ScanerDataReceived;
                scaner.Open();
            }
            catch{}
        }

        private void scaner_ScanerDataReceived(object sender, string code)
        {
            try
            {
                SetText(this, code);
                MessageBox.Show(code);
                if (ScanerDataReceivedEvent != null)
                    Invoke(ScanerDataReceivedEvent);              
            }
            catch (Exception ex)
            {
                MessageBox.Show("scaner_ScanerDataReceived: " + ex.Message);
            }
        }

        private delegate void SetTextHandler(TextBox ctrl, string text);
        private void SetText(TextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new SetTextHandler(SetText), ctrl, text);
            }
            else
            {
                ctrl.Text = text;
                ctrl.SelectionStart = ctrl.Text.Length;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F24:
                case Keys.F23:
                case Keys.F22:
                    scaner.Read();
                    break;
            }
            base.OnKeyDown(e);
        }
    }
}
