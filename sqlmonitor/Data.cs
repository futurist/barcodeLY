using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.Xml;
using System.IO;

namespace sqlmonitor
{
    class Data
    {
        public static List<folderClass> folderList = new List<folderClass> { };
        public static int folderIndex = -1;

        public static List<codeClass> codeList = new List<codeClass> { };
        public static int codeIndex = -1;

        public static Dictionary<string, Form2> formList = new Dictionary<string, Form2>();

        public static string curSN = "";
        public static string putBuffer = "";
        public static string prevPutBuffer = "";

        public static Dictionary<string, DataTable> dataListSN = new Dictionary<string, DataTable>();

        public static Form curForm = null;
        public static void showMsg(string str){
            if (curForm.Name == "Form1") {
                ((sqlmonitor)curForm).showMsg(str);
            }
            if (curForm.Name == "Form2")
            {
                ((Form2)curForm).showMsg(str);
            }

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

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFile);

            string cur = doc.SelectSingleNode("/barcode/curServer").InnerText;

            return cur;
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

            return "";
        }




    }


    public class DB {

        public static string Sqlstr = "Data Source=61.175.244.158,14433;Database=HSFabricTrade_LYCK;persist security info=True;Connection Timeout=10;User ID=dyeinguser;Password=dyeing@2011";


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
            return Id + (Text != "" ? ":" + Text : "") + (Code != "" ? ":" + Code : "");
        }
    }

    public class codeClass
    {
        public string Id { get; set; }
        public string NO { get; set; }
        public string Parent { get; set; }
        public int RollNum { get; set; }

        public codeClass(string id, string parent)
        {
            this.Id = id;
            this.NO = "   　";
            this.Parent = parent;
            this.RollNum = 0;
            
        }

        public codeClass(string id)
        {
            this.Id = id;
            this.NO = "   　";
            this.Parent = "";
            this.RollNum = 0;
        }
        public override string ToString()
        {
            return Id + (Parent != "" ? ":" + Parent : "") + (RollNum > 0 ? ":" + RollNum.ToString() : "");
        }
    }



}
