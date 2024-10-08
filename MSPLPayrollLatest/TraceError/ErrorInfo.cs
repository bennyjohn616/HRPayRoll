// -----------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TraceError
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;
   

    /// <summary>
    /// To handle the ErrorInfo
    /// </summary>
    public class ErrorInfo
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ErrorInfo()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ErrorInfo(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.SessionId = Convert.ToString(dtValue.Rows[0]["SessionId"]);
                this.ErrorMessage = Convert.ToString(dtValue.Rows[0]["ErrorMessage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserId"])))
                    this.UserId = Convert.ToInt32(dtValue.Rows[0]["UserId"]);
                this.ControllerName = Convert.ToString(dtValue.Rows[0]["ControllerName"]);
                this.MethodName = Convert.ToString(dtValue.Rows[0]["MethodName"]);
                this.OtherInfo = Convert.ToString(dtValue.Rows[0]["OtherInfo"]);
                this.UserHost = Convert.ToString(dtValue.Rows[0]["UserHost"]);
                this.BrowserInfo = Convert.ToString(dtValue.Rows[0]["BrowserInfo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the SessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Get or Set the ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Get or Set the UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or Set the ControllerName
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Get or Set the MethodName
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Get or Set the OtherInfo
        /// </summary>
        public string OtherInfo { get; set; }

        /// <summary>
        /// Get or Set the UserHost
        /// </summary>
        public string UserHost { get; set; }

        /// <summary>
        /// Get or Set the BrowserInfo
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the ErrorInfo
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ErrorInfo_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@SessionId", this.SessionId);
            sqlCommand.Parameters.AddWithValue("@ErrorMessage", this.ErrorMessage);
            sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
            sqlCommand.Parameters.AddWithValue("@ControllerName", this.ControllerName);
            sqlCommand.Parameters.AddWithValue("@MethodName", this.MethodName);
            sqlCommand.Parameters.AddWithValue("@OtherInfo", this.OtherInfo);
            sqlCommand.Parameters.AddWithValue("@UserHost", this.UserHost);
            sqlCommand.Parameters.AddWithValue("@BrowserInfo", this.BrowserInfo);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            string outValue = string.Empty;
            bool status = SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the ErrorInfo
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("ErrorInfo_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);


            return DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the ErrorInfo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("ErrorInfo_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            return GetTableData(sqlCommand);
        }

        private string GetSqlConnection()
        {
            return ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            //return "data source=localhost;initial catalog=Payroll_orginal;User ID=sa;Password=excel.123;persist security info=False;packet size=4096";
        }

        private bool SaveData(SqlCommand sqlCommand, out string outPut, string outPutParam = "")
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
                    return false;
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
            }
        }

        private bool DeleteData(SqlCommand sqlCommand)
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

        private DataTable GetTableData(SqlCommand sqlCommand)
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
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }
                return dtTabelData;
            }
        }

        #endregion

    }
}

