using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace barcode
{
    public partial class frmMain : Form
    {
        public Form1 form1 = null;

        public Timer inter2 = new Timer();

        public frmMain()
        {
            InitializeComponent();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        public void disbButton() {

            
            btnPack.Enabled = btnConfig.Enabled = btnOut.Enabled = false;

            inter2.Enabled = false;
            inter2.Interval = 3000; // 1 second
            inter2.Tick += delegate {
                btnPack.Enabled = btnConfig.Enabled = btnOut.Enabled = true;
                inter2.Enabled = false;
            };
            inter2.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            disbButton();

            frmConfig f = new frmConfig();
            f.Show();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            disbButton();

            MobileLaunch.LaunchApp("\\Program Files\\barcode\\sqlmonitor.exe", "");

            form1 = new Form1();
            form1.Show();
        }

        private void btnPack_Click(object sender, EventArgs e)
        {
            disbButton();
            frmPackage f = new frmPackage();
            f.Show();
        }
    }
}