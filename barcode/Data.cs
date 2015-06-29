﻿using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace barcode
{
    class Data
    {
        public static List<folderClass> folderList = new List<folderClass> { };
        public static int folderIndex = -1;

        public static List<codeClass> codeList = new List<codeClass> { };
        public static codeClass getCodeFromList(string id) {
            foreach (var c in codeList) {
                if (c.Id == id) return c;
            }
            return null;
        }

        public static Dictionary<string, Form2> formList = new Dictionary<string, Form2>();

        public static string curSN = "";
        public static string prevSN = "";
        public static string prevSN2 = "";
        public static int prevID = 0;

        public static string putBuffer = "";
        public static string prevPutBuffer = "";


        public static Dictionary<string, DataTable> dataListSN = new Dictionary<string, DataTable>();
        public static Dictionary<string, string> dataListSN2 = new Dictionary<string, string>();

        public static Form curForm = null;
        public static void showMsg(string str){
            if (curForm.Name == "Form1") {
                ((Form1)curForm).showMsg(str);
            }
            if (curForm.Name == "Form2")
            {
                ((Form2)curForm).showMsg(str);
            }

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

    }


    public class folderClass
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
        public int TotalRoll { get; set; }

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
            int packNum=0;
            int rollNum=0;
            double lengthM=0;
            double lengthYD = 0;
            double lengthKG = 0;

            foreach( var code in Data.codeList ){
                if (code.Folder == this.Id) {
                    packNum++;
                    rollNum += code.Rolls.Count;
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

            Text = String.Format("[{0:0}][{1:0}][{2:0}M+{3:0}Y+{4:0}KG]", packNum, rollNum, lengthM, lengthYD, lengthKG );

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
            this.Rolls = new List<rollClass>{};
            this.IsPackage = id.StartsWith("P", StringComparison.CurrentCultureIgnoreCase);
        }

        public void addRow(string []row) {
            bool isNew = true;
            foreach (var r in this.Rolls) {
                if (r.sFabricNo == row[9])
                {
                    isNew = false;
                    break;
                }
            }
            if(isNew) this.Rolls.Add(new rollClass(row));
        }

        public override string ToString()
        {
            var orderNO="";

            if ( object.ReferenceEquals(null, this.OrderNo))
            {
                orderNO = "!!!!";
            }else if (string.IsNullOrEmpty(this.OrderNo))
            { 
                orderNO = this.IsPackage ?  "----" : "****";
            }
            else if ( Data.IsNumber(this.OrderNo) )
            {
                orderNO = (this.IsPackage ? String.Format("{0,4}", OrderNo) : String.Format("{0,4}", OrderNo).Replace(" ", "*"));
            }
            else {
                orderNO = this.OrderNo;
            }

            int rollNum = 0;
            
            string detail = "";
            string MaxKey = "";
            int MaxVal = 0;
            
            Dictionary<string, int> dict = new Dictionary<string, int> { };
            if (IsPackage && Rolls.Count > 0) {
                foreach (var r in Rolls) {
                    rollNum++;
                    if( !dict.ContainsKey(r.sCode) ){
                        dict.Add(r.sCode,1);
                    }else{
                        dict[r.sCode]++;
                    }
                    if (dict[r.sCode] > MaxVal) {
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


    public class rollClass {
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
            this.nWeight = string.IsNullOrEmpty( row[7])? 0 : Convert.ToDouble(row[7]);
            this.nQty = string.IsNullOrEmpty(row[8]) ? 0 : Convert.ToDouble(row[8]);
            this.sFabricNo = row[9];
            this.iFabricOrder = string.IsNullOrEmpty(row[10]) ? 0 : Convert.ToInt16(row[10]);
            this.iPackageOrder = string.IsNullOrEmpty(row[11]) ? 0 : Convert.ToInt16(row[11]);
        
        }

    }

    public class DB {

        static string Sqlstr = "Data Source=161.175.244.158,14433;Database=HSFabricTrade_LYCK;persist security info=True;Connection Timeout=10;User ID=dyeinguser;Password=dyeing@2011";


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
                    //MessageBox.Show("无法连接到数据库！" + ex.Message);
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
                    //MessageBox.Show("无法连接到数据库！" + ex.Message);
                    Data.showMsg("NC!! " + ex.Message);
                    return -1;
                }
                commandSql.Connection = conn;
                commandSql.Connection.Open();
                rowsAffected = commandSql.ExecuteNonQuery();
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
    public class DataAccessLayer {

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
                MessageBox.Show(ex.ToString());
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
                MessageBox.Show(ex.ToString());
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
                MessageBox.Show(ex.ToString());
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
