using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TraceError;
using System.Configuration;

namespace SQLDBperation
{
    public class LoginDBOperation
    {
        //  ErrorLog errorLog;
        /// <summary>
        /// get the connection string
        /// </summary>
        /// <returns></returns>
        private string GetSqlConnection()
        {
            return ConfigurationManager.ConnectionStrings["LoginDBConnection"].ToString();
            //return "data source=localhost;initial catalog=PayrollLogin;User ID=sa;Password=excel.123;persist security info=False;packet size=4096";
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
    }
}
