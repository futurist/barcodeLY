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
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnRet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLan_Click(object sender, EventArgs e)
        {
            CONFIG.setServer("lan");
            this.Close();
        }

        private void btnWan_Click(object sender, EventArgs e)
        {
            CONFIG.setServer("wan");
            this.Close();
        }

        private void btnViewConfig_Click(object sender, EventArgs e)
        {
            MessageBox.Show( CONFIG.getServer() );
        }
    }
}