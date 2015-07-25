using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Diagnostics;
using barcode;

namespace barcode
{
    public partial class frmPackage : BaseForm
    {
		//declare @id as nchar(40)
//set @id = (select N'P'+ CONVERT (varchar(20), (cast ( max( RIGHT( sPackageNo, 10 ) ) as int )+1) ) as maxPkgID from mmInDtl  where  LEN(sPackageNo)=11 )
//update TestDB.dbo.testdata set id=@id where num=10


        public enum Beep : uint
        {
            SND_FILENAME = 0x00020000,
            SND_ASYNC = 0x00000001
        }

        [DllImport("coredll.dll", EntryPoint = "PlaySound")]
        public static extern int PlaySound(System.String pszSound, IntPtr hmod, uint fdwSound);


        public Form1 form1;
        public folderClass folder = null;
        public System.Windows.Forms.Timer inter1 = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer inter2 = new System.Windows.Forms.Timer();

        public string filter = "";


        public int count = 0;
        public string prevDebugStr = "";


        public frmPackage()
        {
            InitializeComponent();
            panelBar.Hide();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.listBox1.DataSource = Data.codeList;

            txtDebug.Visible = false;
            panelBar.Location = new Point(5,14);
            Form_OnShow();
            
        }



        public void Form_OnShow()
        {
            BaseForm.ScannerAutoInit();
            lblDuplicate.Hide();
        }

        public void Form_OnHide()
        {
            try
            {
                BaseForm.ScannerAutoExit();
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
            Data.codeList.Clear();
            Data.folderList.Clear();
        }


        public void debug(string str)
        {
            if (!txtDebug.Visible) return;
            if (prevDebugStr == str) return;
            prevDebugStr = str;
            txtDebug.Text = str + "\r\n" + txtDebug.Text;
        }

        //we get barcode here
        public override void OnBarCodeNotify(byte[] BarCodeData, int nLength)
        {
            if (!panelBar.Visible) return;
            this.textBox1.Focus();
            this.textBox1.Text = Encoding.Default.GetString(BarCodeData, 0, nLength);
            btnAdd_Click();

            //PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            //StandardKeyboradOut(BarCodeData, nLength, _SCAN_DATA_SUFFIX.ENTER_SUFFIX);
        }


        public void btnAdd_Click()
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) idx = Data.codeList.Count - 1;
            addLisBox(new codeClass(textBox1.Text, folder.Id), true);
            
            Data.lastPkgCode = textBox1.Text;

            //textBox1.Focus();
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            btnAdd_Click();
        }

        public void addLisBox(codeClass code, bool moveToTop)
        {

            if (code.Id.Length < 8) return;
            code.Id = Regex.Replace(code.Id, "^0+", "P");

            if (code.Id.StartsWith("P", StringComparison.CurrentCultureIgnoreCase))
            {
                showDuplicateMsg(code, code.Id + "\r\n" + "不可添加包号");
                return;
            }

            if (checkDuplicate(code.Id) != false)
            {
                showDuplicateMsg(code, code.Id + "\r\n" + "已存在");
                return;
            }



            PlaySound("beep.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            Data.codeList.Add( code );

            updateLisBox_datasource();

            if (moveToTop) listBox1.SelectedIndex = 0;

        }

        public bool checkDuplicate(string ID)
        {
            foreach (var code in Data.codeList)
            {
                if (ID == code.Id) return true;
            }
            return false;
        }

        public void showDuplicateMsg(codeClass code, string str)
        {
            lblDuplicate.Show();
            lblDuplicate.Text = str;

            PlaySound("infbeg.wav", IntPtr.Zero, (Int32)Beep.SND_FILENAME | (Int32)Beep.SND_ASYNC);

            inter1.Enabled = false;
            inter1.Interval = 1000; // 1 second
            inter1.Tick += delegate { lblDuplicate.Hide(); inter1.Enabled = false; };
            inter1.Enabled = true;

        }

        public void updateLisBox_datasource()
        {
            var prevIndex = listBox1.SelectedIndex;
            var prevFocus = listBox1.Focused;

            listBox1.Enabled = false;
            listBox1.DataSource = null;
            listBox1.DataSource = Data.getCodesFromFolder(folder.Id);
            listBox1.Enabled = true;

            try
            {
                listBox1.SelectedIndex = prevIndex >= 0 && prevIndex < Data.codeList.Count ? prevIndex : 0;
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
            if (prevFocus) listBox1.Focus();
        }


        private void btnPkg_Click(object sender, EventArgs e)
        {
            folder = new folderClass( txtPkg.Text );
            lblLastPkg.Text = folder.Id;
            lblCurPkg.Text = folder.Id;
            txtPkg.Text = "";

            panelBar.Show();
            
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var idx = listBox1.SelectedIndex;
            if (idx == -1) return;

            switch (e.KeyCode.ToString())
            {

                case "Delete":
                case "Back":
                    Data.codeList.RemoveAt(idx);
                    
                    updateLisBox_datasource();

                    //isDirty = true;
                    e.Handled = true;
                    break;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Return":
                    btnAdd_Click(sender, e);
                    break;
                case "F8":
                    textBox1.Text = "";
                    break;
            }

        }

        private void txtExit_Click(object sender, EventArgs e)
        {
            Form_OnHide();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            returnToFolder();
        }

        public void returnToFolder() {
            Data.codeList.Clear();
            updateLisBox_datasource();
            panelBar.Hide();

            txtPkg.Text = "";
            txtPkg.Focus();
        }

        private void txtPkg_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {

                case "Return":
                    btnPkg_Click(sender, e);
                    break;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            uploadData();

        }


        public void uploadData()
        {
            DataTable dt = null;

            string lastID="", totalID="";
            for (var i = 0; i < Data.codeList.Count; i++ )
            {
                lastID = Data.codeList[i].Id;
                totalID += string.Format( "'{0}',", lastID );
            }
            totalID = totalID.Remove(totalID.Length - 1, 1);

            string sql = string.Format(@"DECLARE @sUpdateMan VARCHAR(20), @report uniqueidentifier;
set @sUpdateMan=(select sUpdateMan from mmInDtl where sFabricNo='{0}');
set @report=(select upbReportFormatDtlGUID from pbBillReportDefine left join mmInDtl on sBillNo=sOrderNo where sFabricNo='{0}');

declare @curdate nvarchar(20) = N'P'+ right(convert(varchar(20),GETDATE(),112), 6);
if exists( select top 1 * from [pbBillFormulaDtl] where sBillFormulaResetNo=@curdate )
	UPDATE [pbBillFormulaDtl] set iBillSequence=iBillSequence+1 where sBillFormulaResetNo=@curdate;
else
	INSERT INTO pbBillFormulaDtl(uGUID, upbBillFormulaHdrGUID, sBillFormulaResetNo, iBillSequence, tGenerateTime)VALUES([DBO].[fnpbNewCombGUID](), N'{{875EFBBA-DBA8-42A5-A9DC-372D02BC1294}}', @curdate, 1, GETDATE());
declare @maxPkg nvarchar(20) = @curdate + right('0000'+ CONVERT (varchar(20), (select iBillSequence from [pbBillFormulaDtl] where sBillFormulaResetNo=@curdate)), 4)

update mmInDtl set sPackageNo=@maxPkg, iPackageOrder={2}, tUpdateTime=GETDATE() where sFabricNo in ({1});

exec sppbRegisterBillReportTask  @maxPkg, 1003, @sUpdateMan,NULL, NULL, NULL, @report,1;", lastID, totalID, folder.Id);



            try
            {
                dt = DB.Query(sql);
            }
            catch (Exception e)
            {
                string str = "{@error@}" + e.Message;
                MessageBox.Show(str);
                return;
            }

            returnToFolder();


        }

        private void btnViewPkg_Click(object sender, EventArgs e)
        {
            Form_OnHide();
            frmPackageList f = new frmPackageList(this);
            f.Show();
        }
        
    }
}