using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TraceError;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;


namespace SQLDBOperation
{
    public class DBOperation  

    {
        //  ErrorLog errorLog;
        /// <summary>
        /// get the connection string
        /// </summary>
        /// <returns></returns>
        private string GetSqlConnection()
            {
            string str = string.Empty;

            string tempcon = "";

            if (HttpContext.Current.Session.Count > 1)
            {
                tempcon = Convert.ToString(HttpContext.Current.Session["DBString"]);
            }

            if (!string.IsNullOrEmpty(tempcon))
            {
                return tempcon;
            }
            else
            {
                if (HttpContext.Current.Session.Count > 1)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["DBString"])))
                    {
                        str = Convert.ToString(HttpContext.Current.Session["DBString"]);
                    }
                    else
                    {
                        str = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
                    }
                }
                else
                {
                    str = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
                }
                return str;
            }
          
            //return "data source=localhost;initial catalog=Payroll_orginal;User ID=sa;Password=excel.123;persist security info=False;packet size=4096";
        }

        /// <summary>
        /// It will save the data to a data source
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="outPut"></param>
        /// <param name="outPutParam"></param>
        /// <returns></returns>
        public bool SaveData(SqlCommand sqlCommand, out string outPut, string outPutParam = "")
        {

            outPut = string.Empty;
            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.ExecuteNonQuery();
                    if (!string.IsNullOrEmpty(outPutParam))
                        outPut = sqlCommand.Parameters[outPutParam].Value.ToString();
                    return true;
                }
                catch (Exception ex)
                {
                    // errorLog = new ErrorLog(ex);
                    ErrorLog.Log(ex);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// It will save the data to a data source
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="outPut"></param>
        /// <param name="outPutParam"></param>
        /// <returns></returns>
        public bool SaveData(SqlCommand sqlCommand, out List<string> outPut, List<string> outPutParam)
        {

            outPut = null;
            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.ExecuteNonQuery();
                    if (outPutParam != null)
                    {
                        outPut = new List<string>();
                        for (int count = 0; count < outPutParam.Count; count++)
                        {
                            outPut.Add(sqlCommand.Parameters[outPutParam[count]].Value.ToString());
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    // errorLog = new ErrorLog(ex);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
               
            }
        }

        /// <summary>
        /// delete the data 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public bool DeleteData(SqlCommand sqlCommand)
        {

            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    //errorLog = new ErrorLog(ex);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
            }
        }
        public bool save(SqlCommand sqlCommand)
        {

            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
            }
        }
        public bool DeleteData(SqlCommand sqlCommand, out string outPut, string outPutParam = "")
        {
            outPut = string.Empty;
            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.ExecuteNonQuery();
                    if (!string.IsNullOrEmpty(outPutParam))
                        outPut = sqlCommand.Parameters[outPutParam].Value.ToString();
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    //errorLog = new ErrorLog(ex);
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
            }
        }


        /// <summary>
        /// It will get the Data from the Data source
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public DataTable GetTableData(SqlCommand sqlCommand)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                DataTable dtTabelData = new DataTable();
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = sqlCommand;
                        da.Fill(dtTabelData);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    //errorLog = new ErrorLog(ex);

                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
                return dtTabelData;
            }
        }

        public DataSet GetDataSet(SqlCommand sqlCommand)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetSqlConnection()))
            {
                DataSet dtTabelData = new DataSet();
                try
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlCommand;
                    da.Fill(dtTabelData);

                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    //errorLog = new ErrorLog(ex);

                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
                return dtTabelData;
            }
        }


        public bool bulksave(DataTable dt)
        {
            bool returnVal = false;
            using (var bulkCopy = new SqlBulkCopy(GetSqlConnection(), SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = "MasterInputSettings";
                bulkCopy.WriteToServer(dt);
                returnVal = true;
            }

            return returnVal;
        }

        public bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;
            string servercon = ConfigurationManager.ConnectionStrings["ServerCon"].ToString();
            try
            {
                SqlConnection tmpConn = new SqlConnection(servercon);
                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                result = false;
            }

            return result;
        }

        public bool CreateNewDB(string databaseName)
        {
            bool returnval = false;
            string servercon = ConfigurationManager.ConnectionStrings["ServerCon"].ToString();
            System.Data.SqlClient.SqlConnection tmpConn;
            string sqlCreateDBQuery;
            tmpConn = new SqlConnection();
            tmpConn.ConnectionString = servercon;
            sqlCreateDBQuery = " CREATE DATABASE " + databaseName;
            SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
            try
            {
                tmpConn.Open();
                myCommand.ExecuteNonQuery();
                returnval = true;
            }
            catch (System.Exception ex)
            {
                ErrorLog.Log(ex);
                ErrorLog.LogInFile(ex);
                returnval = false;
            }
            finally
            {
                tmpConn.Close();
            }

            return returnval;
        }

        public bool ExcuteNewDBScript(string databaseName)
        {
            bool returnval = false;
            try
            {
                string servercon = ConfigurationManager.ConnectionStrings["ServerCon"].ToString();
                string dbname = "initial catalog =" + databaseName + ";";
                servercon = servercon.Replace("initial catalog=Master;", dbname);
                string sqlConnectionString = servercon;
                string script = File.ReadAllText(@"D:\Sourcecode\MindspayLeaveFinal\DBScript\DB whole_Script_old.sql");
                System.Data.SqlClient.SqlConnection tmpConn;
                tmpConn = new SqlConnection();
                tmpConn.ConnectionString = servercon;
                //   SqlCommand myCommand = new SqlCommand(script, tmpConn);

                tmpConn.Open();
                IEnumerable<string> strSQLCmds = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                //int rowcount = 0;

                foreach (string strQuery in strSQLCmds)
                {
                    if (strQuery.Trim() != string.Empty)
                    {
                        new SqlCommand(strQuery, tmpConn).ExecuteNonQuery();
                    }

                }
                //myCommand.ExecuteNonQuery();
                returnval = true;
                tmpConn.Close();
            }
            catch (System.Exception ex)
            {
                ErrorLog.Log(ex);
                ErrorLog.LogInFile(ex);
                returnval = false;
            }
            finally
            {
                // tmpConn.Close();
            }
            return returnval;
        }
    }
}
