using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;


namespace barcode
{
    class Data
    {

        public static string lastPkgCode = "";

        public static void debug(string str) { }

        public static void ShowMessage(string str)
        {
            MessageBox.Show(str, "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

        public static Dictionary<string, string> cacheDict = new Dictionary<string, string> { };

        public static string cacheFolder = @"\Application\barcode\cache\";

        public static void cacheToFile(string folderId)
        {

            List<string> s = new List<string> { };
            var codes = getCodesFromFolder(folderId);
            foreach (var c in codes)
            {
                s.Add(c.Id);
            }
            string curFileContent = string.Join("{br}", s.ToArray());
            if (!cacheDict.ContainsKey(folderId))
            {
                cacheDict.Add(folderId, "");
            }
            if (curFileContent != "" && cacheDict[folderId] != curFileContent)
            {
                cacheDict[folderId] = curFileContent;
                try
                {
                    if (!Directory.Exists(cacheFolder))
                    {
                        Directory.CreateDirectory(cacheFolder);
                    }

                    string filepath = cacheFolder + folderId + ".txt";

                    //StreamWriter file = null;

                    //if (!File.Exists(filepath))
                    //{
                    //    file = File.CreateText(filepath);
                    //}
                    //else {
                    //    //file = (StreamWriter)File.OpenWrite(filepath);
                    //}

                    var ws = new StreamWriter(filepath);

                    ws.Write(curFileContent);
                    ws.Close();
                }
                catch (Exception e)
                {
                    Data.ShowMessage("Exception: " + e.Message);
                }
                //File.WriteAllText(@"\\Application\\barcode\\" + folderId, curFileContent);
            }

        }

        public static string checkFileCache(string folderId)
        {
            if (folderId == "") return "";
            string filepath = cacheFolder + folderId + ".txt";

            if (!File.Exists(filepath)) return "";

            var file = new StreamReader(filepath);
            string str = file.ReadToEnd();
            file.Close();

            if (str == "") return "";

            return str;
        }


        public static string renameFileCache(string folderId, string newfolderId)
        {
            if (folderId == "") return "";
            string filepath = cacheFolder + folderId + ".txt";
            string newfilepath = cacheFolder + newfolderId + ".txt";

            if (!File.Exists(filepath)) return "";

            //make a backup of newfile if exists
            if (File.Exists(newfilepath))
            {
                //File.Delete(newfilepath);
                string dtname = string.Format("{0}-{1:yyyy-MM-dd_HH-mm-ss}.txt", newfilepath.Replace(".txt", ""), DateTime.Now);
                File.Move(newfilepath, dtname);
            }

            try
            {
                File.Move(filepath, newfilepath); // Try to move
            }
            catch (IOException e)
            {
                debug(e.Message);
            }

            return "";
        }

        public static string deleteFileCache(string folderId)
        {
            if (folderId == "") return "";
            string filepath = cacheFolder + folderId + ".txt";

            if (!File.Exists(filepath)) return "";

            //make a backup of newfile if exists
            if (File.Exists(filepath))
            {
                //File.Delete(filepath);
                string dtname = string.Format("{0}-{1:yyyy-MM-dd_HH-mm-ss}.txt", filepath.Replace(".txt",""), DateTime.Now);
                File.Move(filepath, dtname);
            }

            return "";
        }

        public static void updateFolderFromStr(string folderId, string str)
        {

            var folder = getFolderFromList(folderId);
            if (object.ReferenceEquals(null, folder)) return;

            if (str == "") return;

            string[] line = Regex.Split(str, "{br}");
            foreach (string id in line)
            {
                if (id != "") codeList.Add(new codeClass(id, folderId));
            }

        }

        public static void deleteAllFileCache()
        {
            Directory.Delete(cacheFolder, true);
        }


        public static List<pkgOrderClass> pkgOrderList = new List<pkgOrderClass> { };


        public static List<folderClass> folderList = new List<folderClass> { };
        public static int folderIndex = -1;

        public static List<codeClass> codeList = new List<codeClass> { };

        public static folderClass getFolderFromList(string id)
        {
            foreach (var c in folderList)
            {
                if (c.Id == id) return c;
            }
            return null;
        }


        public static codeClass getCodeFromList(string id)
        {
            foreach (var c in codeList)
            {
                if (c.Id == id) return c;
            }
            return null;
        }

        public static List<codeClass> getCodesFromFolder(string folder)
        {
            var CL = new List<codeClass> { };
            foreach (var c in codeList)
            {
                if (c.Folder == folder)
                {
                    CL.Add(c);
                }
            }
            return CL;
        }

        public static List<string> getEmptyCodesFromFolder(string folder, int limit)
        {
            var CL = new List<string> { };
            foreach (var c in codeList)
            {
                if (c.Folder == folder && c.OrderNo == "")
                {
                    CL.Add(c.Id);
                    if (CL.Count >= limit) break;
                }
            }
            return CL;
        }


        public static Dictionary<string, Form2> formList = new Dictionary<string, Form2>();

        public static string curSN = "";
        public static string prevSN = "";
        public static string prevSN2 = "";
        public static int prevID = 0;

        public static string putBuffer = "";
        public static string prevPutBuffer = "";
        public static bool commExited = false;

        public static Dictionary<string, DataTable> dataListSN = new Dictionary<string, DataTable>();
        public static Dictionary<string, string> dataListSN2 = new Dictionary<string, string>();

        public static Form curForm = null;
        public static void showMsg(string str)
        {
            if (curForm.Name == "Form1")
            {
                ((Form1)curForm).showMsg(str);
            }
            if (curForm.Name == "Form2")
            {
                ((Form2)curForm).showMsg(str);
            }

        }

        public static void exitApp()
        {
            if (curForm.Name == "Form1")
            {
                ((Form1)curForm).Form1_OnHide();
            }
            if (curForm.Name == "Form2")
            {
                ((Form2)curForm).Form2_OnHide();
            }


            WinCE.createMemFile("EXIT");
            if (commExited) WinCE.closeMemFile();
            //this.Close();
            Application.Exit();
        }


        /// <summary>  
        /// 判读字符串是否为数值型。可以代正负号(+-) ikmb@163.com  
        /// </summary>  
        /// <param name="strNumber">字符串</param>  
        /// <returns>是否</returns>  
        public static bool IsNumber(string strNumber)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"^\s*[+-]?\d+(\.)?\d*\s*$");
            return r.IsMatch(strNumber);
        }

        public static void deleteFolder(string id, int pos)
        {

            for (int i = 0; i < codeList.Count; i++)
            {
                var code = codeList[i];
                if (code.Folder == id)
                {
                    codeList.Remove(code);
                    i--;
                }

            }


            folderList.RemoveAt(pos);

            deleteFileCache(id);
        }


        public static void modifyFolder(string newid, int pos)
        {
            string oldId = folderList[pos].Id;
            for (int i = 0; i < codeList.Count; i++)
            {
                var code = codeList[i];
                if (code.Folder == oldId)
                {
                    code.Folder = newid;
                }
            }

            if (folderList[pos].Code != "")
            {
                int ret = DB.Exec(@"update mmOutHdr set sRemark=N'" + newid + "' where sStoreOutNo = N'" + folderList[pos].Code + "'");

                if (ret < 0)
                {
                    Data.ShowMessage("网络连接有误,请手动在电脑上更改备注以同步");
                }
            }

            renameFileCache(oldId, newid);

            folderList[pos].Id = newid;


        }



        public static bool Upload()
        {

            if (folderList.Count == 0) return false;

            Dictionary<string, string> codeDict = new Dictionary<string, string>();

            foreach (folderClass folder in folderList)
            {

                //生成出库号
                if (folder.Code == "")
                {
                    string mmout = @"insert into mmOutHdr
([sStoreOutNo],[sRemark],[sStoreOutStatus],[iCompanyID],[bIsBackOut],[ummStoreGUID],[ummStoreOutTypeGUID],[sStoreOutMan],[tStoreOutTime],[sCreator],[tCreateTime],[sUpdateMan],[tUpdateTime])
select N'STE'+ right(convert(varchar(20),GETDATE(),112), 6) + right('0000'+CONVERT (varchar(20), (cast ( isnull(max( RIGHT( sStoreOutNo, 3 ) ), 0) as int )+1) ),3) as maxOutID, 
N'"+ folder.Id +"',N'NEW',2,0,N'{637A600B-C40F-4933-991F-4426374649D2}',N'{49C502B9-C234-4002-9DA7-2145BC1001A9}',N'yangjm',GETDATE(),N'yangjm',GETDATE(),N'yangjm',GETDATE() from mmOutHdr where LEN(sStoreOutNo)=12 and tUpdateTime>= CAST(CAST( GETDATE() AS DATE) AS DATETIME)";

                    int ret = DB.Exec(mmout);

                    if (ret < 0)
                    {
                        Data.ShowMessage("网络连接有误,请重试");
                        return false;
                    }


                    DataTable dt = DB.Query("select top 1 sStoreOutNo from mmOutHdr where sRemark=N'" + folder.Id + "' order by tCreateTime desc ");
                    if (object.ReferenceEquals(dt, null) || dt.Rows.Count == 0)
                    {
                        Data.ShowMessage("生成单据有误,请重试");
                        return false;
                    }

                    string sOutNo = dt.Rows[0][0].ToString();
                    folder.Code = sOutNo;

                    codeDict.Add(folder.Id, folder.Code);
                }

                //插入条码
                List<string> fabricNos = new List<string> { };
                List<string> packageNos = new List<string> { };
                foreach (codeClass code in codeList)
                {
                    if (code.Folder == folder.Id)
                    {
                        if (code.IsPackage)
                        {
                            packageNos.Add(String.Format("N'{0}'", code.Id));
                        }
                        else
                        {
                            fabricNos.Add(String.Format("N'{0}'", code.Id));
                        }
                    }
                }

                string sNO = string.Join(",", fabricNos.ToArray());
                if (sNO != "")
                {
                    int ret = DB.Exec(@"insert into mmOutDtl
select newID() as uGUID, mmOutHdr.uGUID as ummOutHdrGUID,  mmInDtl.uGUID as ummInDtlGUID, nStockWeight, nStockLengthM , nStockLengthYD, nStockPieceQty, nStockPkgQty, mmInDtl.usdOrderDtlGUID,NULL, NULL, NULL,mmInDtl.nACPrice,mmInDtl.nAmount,mmInDtl.nTaxAmount, mmInDtl.sRemark, mmInDtl.bBalance, mmOutHdr.sCreator, GETDATE(), mmOutHdr.sUpdateMan, GETDATE(), mmInDtl.sFabricNo, mmInDtl.nTaxRate, mmInDtl.sUnit, mmInDtl.sBatchNo, mmInDtl.sCurrency, mmInDtl.sOrderNo, mmInDtl.upsSubContractDtlGUID, mmInDtl.nFreeQty, mmInDtl.sPackageNo, mmInDtl.sBoardNo, mmInDtl.iFabricOrder, mmPurchaseContractDtl.nAddAmount,mmInDtl.sShade, NULL as nDiscountAmount, mmInDtl.iPackageOrder from mmInDtl
left join mmPurchaseContractDtl on mmPurchaseContractDtl.uGUID=mmInDtl.ummPurchaseContractDtlGUID
left join mmOutHdr on mmOutHdr.sStoreOutNo = N'" + folder.Code + "' where mmInDtl.sFabricNo in ( " + sNO + " ) and not exists ( select 1 from mmOutDtl b where mmInDtl.sFabricNo=b.sFabricNo )");
                    if (ret < 0)
                    {
                        Data.ShowMessage("插入条码有误,请重试");
                        return false;
                    }
                }

                sNO = string.Join(",", packageNos.ToArray());
                if (sNO != "")
                {
                    int ret = DB.Exec(@"insert into mmOutDtl
select newID() as uGUID, mmOutHdr.uGUID as ummOutHdrGUID,  mmInDtl.uGUID as ummInDtlGUID, nStockWeight, nStockLengthM , nStockLengthYD, nStockPieceQty, nStockPkgQty, mmInDtl.usdOrderDtlGUID,NULL, NULL, NULL,mmInDtl.nACPrice,mmInDtl.nAmount,mmInDtl.nTaxAmount, mmInDtl.sRemark, mmInDtl.bBalance, mmOutHdr.sCreator, GETDATE(), mmOutHdr.sUpdateMan, GETDATE(), mmInDtl.sFabricNo, mmInDtl.nTaxRate, mmInDtl.sUnit, mmInDtl.sBatchNo, mmInDtl.sCurrency, mmInDtl.sOrderNo, mmInDtl.upsSubContractDtlGUID, mmInDtl.nFreeQty, mmInDtl.sPackageNo, mmInDtl.sBoardNo, mmInDtl.iFabricOrder, mmPurchaseContractDtl.nAddAmount,mmInDtl.sShade, NULL as nDiscountAmount, mmInDtl.iPackageOrder from mmInDtl
left join mmPurchaseContractDtl on mmPurchaseContractDtl.uGUID=mmInDtl.ummPurchaseContractDtlGUID
left join mmOutHdr on mmOutHdr.sStoreOutNo = N'" + folder.Code + "' where mmInDtl.sPackageNo in ( " + sNO + " ) and not exists ( select 1 from mmOutDtl b where mmInDtl.sFabricNo=b.sFabricNo )");
                    if (ret < 0)
                    {
                        Data.ShowMessage("插入条码有误,请重试");
                        return false;
                    }
                }


                if (curForm != null && curForm.Name == "Form1") ((Form1)curForm).updateLisBox();

            }

            Data.ShowMessage("上传数据成功！");

            return true;
        }

    }


    public class folderClass
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }

        public folderClass(string id, string text)
        {
            this.Id = id;
            this.Text = text;
            this.Code = "";
        }
        public folderClass(string id)
        {
            this.Id = id;
            this.Text = "";
            this.Code = "";
        }
        public override string ToString()
        {
            int NO = 0;
            int packNum = 0;
            int rollNum = 0;
            double lengthM = 0;
            double lengthYD = 0;
            double lengthKG = 0;

            foreach (var code in Data.codeList)
            {
                if (code.Folder == this.Id)
                {
                    NO++;
                    if (code.IsPackage) packNum++;
                    rollNum += code.IsPackage ? code.Rolls.Count : 1;
                    foreach (var r in code.Rolls)
                    {
                        if (string.Equals(r.sUnit, "KG", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lengthKG += r.nQty;
                        }
                        if (r.sUnit.StartsWith("Y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lengthYD += r.nQty;
                        }
                        if (r.sUnit.StartsWith("M", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lengthM += r.nQty;
                        }
                    }
                }
            }

            Text = String.Format("[{0:0}]", NO) + String.Format("[{0:0}][{1:0}][{2:0}M+{3:0}Y+{4:0}KG]", packNum, rollNum, lengthM, lengthYD, lengthKG);

            return Id + (Text != "" ? ":" + Text : "") + (Code != "" ? ":" + Code : "");
        }
    }


    public class codeClass
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public string Folder { get; set; }
        public bool IsPackage { get; set; }
        public List<rollClass> Rolls { get; set; }

        public codeClass(string id, string folder)
        {
            this.Id = id;
            this.OrderNo = "";
            this.Folder = folder;
            this.Rolls = new List<rollClass> { };
            this.IsPackage = id.StartsWith("P", StringComparison.CurrentCultureIgnoreCase);
        }

        public void addRow(string[] row)
        {
            bool isNew = true;
            foreach (var r in this.Rolls)
            {
                if (r.sFabricNo == row[9])
                {
                    isNew = false;
                    break;
                }
            }
            if (isNew) this.Rolls.Add(new rollClass(row));
        }

        public override string ToString()
        {
            var orderNO = "";

            if (object.ReferenceEquals(null, this.OrderNo))
            {
                orderNO = "!!!!";
            }
            else if (string.IsNullOrEmpty(this.OrderNo))
            {
                orderNO = this.IsPackage ? "----" : "****";
            }
            else if (Data.IsNumber(this.OrderNo))
            {
                orderNO = (this.IsPackage ? String.Format("{0,4}", OrderNo) : String.Format("{0,4}", OrderNo).Replace(" ", "*"));
            }
            else
            {
                orderNO = this.OrderNo;
            }

            int rollNum = 0;

            string detail = "";
            string MaxKey = "";
            int MaxVal = 0;

            Dictionary<string, int> dict = new Dictionary<string, int> { };
            if (IsPackage && Rolls.Count > 0)
            {
                foreach (var r in Rolls)
                {
                    rollNum++;
                    if (!dict.ContainsKey(r.sCode))
                    {
                        dict.Add(r.sCode, 1);
                    }
                    else
                    {
                        dict[r.sCode]++;
                    }
                    if (dict[r.sCode] > MaxVal)
                    {
                        MaxVal = dict[r.sCode];
                        MaxKey = r.sCode;
                    }
                }

                detail = "-" + MaxKey + (dict.Keys.Count > 1 ? "(" + MaxVal.ToString() + ")" + "..." : "");
            }

            var rollNO = (this.IsPackage ? ":[" + rollNum.ToString() + "]" : "");

            return orderNO + ":" + Id + rollNO + detail;
        }
    }


    public class rollClass
    {
        public int iFabricOrder { get; set; }
        public int iPackageOrder { get; set; }
        public string sCode { get; set; }
        public string sName { get; set; }
        public string sColorNo { get; set; }
        public double nPrice { get; set; }
        public string sBatch { get; set; }
        public string sWidth { get; set; }
        public string sUnit { get; set; }
        public double nWeight { get; set; }
        public double nQty { get; set; }
        public string sFabricNo { get; set; }

        public rollClass(string[] row)
        {

            this.sCode = row[0];
            this.sName = row[1];
            this.sColorNo = row[2];
            this.nPrice = Convert.ToDouble(row[3]);
            this.sBatch = row[4];
            this.sWidth = row[5];
            this.sUnit = row[6];
            this.nWeight = string.IsNullOrEmpty(row[7]) ? 0 : Convert.ToDouble(row[7]);
            this.nQty = string.IsNullOrEmpty(row[8]) ? 0 : Convert.ToDouble(row[8]);
            this.sFabricNo = row[9];
            this.iFabricOrder = string.IsNullOrEmpty(row[10]) ? 0 : Convert.ToInt16(row[10]);
            this.iPackageOrder = string.IsNullOrEmpty(row[11]) ? 0 : Convert.ToInt16(row[11]);

        }

    }




    public class pkgOrderClass
    {
        public string productSN { get; set; }
        public string sMaterialDesc { get; set; }
        public string sColorNo { get; set; }
        public string sBatchNo { get; set; }
        public int minPkgNo { get; set; }
        public int maxPkgNo { get; set; }

        public pkgOrderClass(string productSN)
        {
            this.productSN = productSN;
            this.sMaterialDesc = "";
            this.sColorNo = "";
            this.sBatchNo = "";
            this.minPkgNo = 0;
            this.maxPkgNo = 0;
        }
        public pkgOrderClass(string productSN, string sMaterialDesc, string sColorNo , string sBatchNo, int minPkgNo, int maxPkgNo )
        {
            this.productSN = productSN;
            this.sMaterialDesc = sMaterialDesc;
            this.sColorNo = sColorNo;
            this.sBatchNo = sBatchNo;
            this.minPkgNo = minPkgNo;
            this.maxPkgNo = maxPkgNo;
        }

        public override string ToString()
        {
            return string.Format(@"{0}:{1}[{2}]:{3}-{4}", 
                sMaterialDesc == "" ? productSN : sMaterialDesc, 
                sColorNo, sBatchNo, minPkgNo, maxPkgNo);
        }
    }


    public class CONFIG
    {

        public static string ConfigFile = @"\Program Files\barcode\config.xml";


        public static string initServer()
        {
            if (!File.Exists(ConfigFile)) return "";

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFile);

            string cur = doc.SelectSingleNode("/barcode/curServer").InnerText;

            XmlNode server = doc.SelectSingleNode("/barcode/server[name='" + cur + "']");
            string ip = server.SelectSingleNode("config").InnerText;
            DB.Sqlstr = ip;

            return cur;
        }

        public static string getServer()
        {
            if (!File.Exists(ConfigFile)) return "";

            string cur = initServer();
            string test = DB.TestConn()?"连接成功":"连接失败";
            return cur + test;
        }

        public static string setServer(string servername)
        {
            if (!File.Exists(ConfigFile)) return "";

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFile);

            doc.SelectSingleNode("/barcode/curServer").InnerText = servername;

            XmlNode server = doc.SelectSingleNode("/barcode/server[name='" + servername + "']");
            string ip = server.SelectSingleNode("config").InnerText;
            DB.Sqlstr = ip;

            using (XmlTextWriter xtw = new XmlTextWriter(ConfigFile, Encoding.UTF8))
            {
                //xtw.Formatting = Formatting.Indented; // leave this out, it breaks EWP!
                doc.WriteContentTo(xtw);
            }

            string test = DB.TestConn() ? "连接成功" : "连接失败";
            MessageBox.Show(servername + test);

            return "";
        }




    }


    public class DB
    {

        public static string Sqlstr = "Data Source=61.175.244.158,14433;Database=HSFabricTrade_LYCK;persist security info=True;Connection Timeout=10;User ID=dyeinguser;Password=dyeing@2011";


        public static bool TestConn()
        {
            using (SqlConnection conn = new SqlConnection(Sqlstr))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    //Data.ShowMessage("无法连接到数据库！" + ex.Message);
                    return false;
                }
            }
            return true;
        }

        public static DataTable Query(string sql)
        {
            using (SqlConnection conn = new SqlConnection(Sqlstr))
            using (System.Data.SqlClient.SqlCommand cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    //Data.ShowMessage("无法连接到数据库！" + ex.Message);
                    Data.showMsg("NC!! " + ex.Message);
                    return null;
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static int Exec(string sql)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(Sqlstr))
            using (System.Data.SqlClient.SqlCommand commandSql = new SqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    //Data.ShowMessage("无法连接到数据库！" + ex.Message);
                    Data.showMsg("NC!! " + ex.Message);
                    return -1;
                }
                try
                {
                    rowsAffected = commandSql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //Data.ShowMessage("无法连接到数据库！" + ex.Message);
                    Data.showMsg("NC!! " + ex.Message);
                    return -1;
                }
            }
            return rowsAffected;
        }

    }

    /**
     * DataAccessLayer: from http://stackoverflow.com/questions/20817198/c-sharp-sqlcommand-and-sqldataadapter-executenonquery-for-sql-server-insert-upd
     * 
     * Method with SqlCommand or SqlDataAdapter
     * 
     * **/
    public class DataAccessLayer
    {

        //Insert
        private bool Insert(string firstName, string lastName, string synonym)
        {
            bool isInserted = false;
            try
            {
                int rowsAffected = 0;
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append("INSERT INTO Customer ");
                insertQuery.Append("(");
                insertQuery.Append("FirstName, ");
                insertQuery.Append("LastName, ");
                insertQuery.Append("Synonym ");
                insertQuery.Append(") ");
                insertQuery.Append("VALUES ");
                insertQuery.Append("(");
                insertQuery.Append("@FirstName, ");
                insertQuery.Append("@LastName, ");
                insertQuery.Append("@Synonym ");
                insertQuery.Append(");");
                using (SqlCommand commandSql = new SqlCommand(insertQuery.ToString()))
                {
                    commandSql.Parameters.AddWithValue("@FirstName", firstName);
                    commandSql.Parameters.AddWithValue("@LastName", lastName);
                    commandSql.Parameters.AddWithValue("@Synonym", synonym);
                    DataAccessLayer dal = new DataAccessLayer();

                    rowsAffected = dal.Execute(commandSql); // SqlCommand ????
                    rowsAffected = dal.Insert(commandSql); // SqlDataAdapter ???

                }
                if (rowsAffected > 0)
                {
                    isInserted = true;
                }
            }
            catch (Exception ex)
            {
                Data.ShowMessage(ex.ToString());
            }
            return isInserted;
        }
        //UPDATE
        private bool Update(int custId, string firstName, string lastName, string synonym)
        {
            bool isUpdated = false;
            try
            {
                int rowsAffected = 0;
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append("UPDATE Customer ");
                updateQuery.Append("SET ");
                updateQuery.Append("FirstName = @FirstName, ");
                updateQuery.Append("LastName = @LastName, ");
                updateQuery.Append("Synonym = @Synonym ");
                updateQuery.Append("WHERE ");
                updateQuery.Append("CustomerId = @CustomerId;");
                using (SqlCommand commandSql = new SqlCommand(updateQuery.ToString()))
                {
                    commandSql.Parameters.AddWithValue("@CustomerId", custId);
                    commandSql.Parameters.AddWithValue("@FirstName", firstName);
                    commandSql.Parameters.AddWithValue("@LastName", lastName);
                    commandSql.Parameters.AddWithValue("@Synonym", synonym);
                    DataAccessLayer dal = new DataAccessLayer();
                    rowsAffected = dal.Execute(commandSql); // SqlCommand ????
                    rowsAffected = dal.Update(commandSql); // SqlDataAdapter ???
                }
                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Data.ShowMessage(ex.ToString());
            }
            return isUpdated;
        }
        //DELETE
        private bool Delete(int custId)
        {
            bool isDeleted = false;
            try
            {
                int rowsAffected = 0;
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append("DELETE FROM Customer ");
                deleteQuery.Append("WHERE ");
                deleteQuery.Append("CustomerId = @CustomerId;");
                using (SqlCommand commandSql = new SqlCommand(deleteQuery.ToString()))
                {
                    commandSql.Parameters.AddWithValue("@CustomerId", custId);
                    DataAccessLayer dal = new DataAccessLayer();
                    rowsAffected = dal.Delete(commandSql);
                    rowsAffected = dal.Execute(commandSql); // SqlCommand ????
                    rowsAffected = dal.Delete(commandSql); // SqlDataAdapter ???
                }
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                Data.ShowMessage(ex.ToString());
            }
            return isDeleted;
        }

        // SqlCommand
        public int Execute(SqlCommand commandSql)
        {
            int rowsAffected = 0;
            using (SqlConnection connectionSql = new SqlConnection("ConnectionString"))
            {
                commandSql.Connection = connectionSql;
                commandSql.Connection.Open();
                rowsAffected = commandSql.ExecuteNonQuery();
            }
            return rowsAffected;
        }

        // SqlDataAdapter
        public int Insert(SqlCommand commandSql)
        {
            int rowsAffected = 0;
            using (SqlConnection connectionSql = new SqlConnection("ConnectionString"))
            {
                using (SqlDataAdapter dataAdapterSql = new SqlDataAdapter())
                {
                    dataAdapterSql.InsertCommand = commandSql;
                    dataAdapterSql.InsertCommand.Connection = connectionSql;
                    dataAdapterSql.InsertCommand.Connection.Open();
                    rowsAffected = dataAdapterSql.InsertCommand.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        // SqlDataAdapter
        public int Update(SqlCommand commandSql)
        {
            int rowsAffected = 0;
            using (SqlConnection connectionSql = new SqlConnection("ConnectionString"))
            {
                using (SqlDataAdapter dataAdapterSql = new SqlDataAdapter())
                {
                    dataAdapterSql.UpdateCommand = commandSql;
                    dataAdapterSql.UpdateCommand.Connection = connectionSql;
                    dataAdapterSql.UpdateCommand.Connection.Open();
                    rowsAffected = dataAdapterSql.UpdateCommand.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        // SqlDataAdapter
        public int Delete(SqlCommand commandSql)
        {
            int rowsAffected = 0;
            using (SqlConnection connectionSql = new SqlConnection("ConnectionString"))
            {
                using (SqlDataAdapter dataAdapterSql = new SqlDataAdapter())
                {
                    dataAdapterSql.DeleteCommand = commandSql;
                    dataAdapterSql.DeleteCommand.Connection = connectionSql;
                    dataAdapterSql.DeleteCommand.Connection.Open();
                    rowsAffected = dataAdapterSql.DeleteCommand.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
    }





}
