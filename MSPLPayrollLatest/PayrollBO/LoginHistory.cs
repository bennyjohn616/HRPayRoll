// -----------------------------------------------------------------------
// <copyright file="LoginHistory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    using SQLDBperation;

    /// <summary>
    /// To handle the LoginHistory
    /// </summary>
    public class LoginHistory
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoginHistory()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
      
        public LoginHistory(int userId)
        {
            this.UserId = userId;
            DataTable dtValue = this.GetTableValues(this.UserId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserId"])))
                    this.UserId = Convert.ToInt32(dtValue.Rows[0]["UserId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SessionId"])))
                    this.SessionId = Convert.ToString(dtValue.Rows[0]["SessionId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LogOn"])))
                    this.LogOn = Convert.ToDateTime(dtValue.Rows[0]["LogOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LofOff"])))
                    this.LofOff = Convert.ToDateTime(dtValue.Rows[0]["LofOff"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LastProcessTime"])))
                    this.LastProcessTime = Convert.ToDateTime(dtValue.Rows[0]["LastProcessTime"]);
                this.UserHost = Convert.ToString(dtValue.Rows[0]["UserHost"]);
                this.BrowserInfo = Convert.ToString(dtValue.Rows[0]["BrowserInfo"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or Set the SessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Get or Set the LogOn
        /// </summary>
        public DateTime LogOn { get; set; }

        /// <summary>
        /// Get or Set the LofOff
        /// </summary>
        public DateTime LofOff { get; set; }

        public string UserHost { get; set; }

        public string BrowserInfo { get; set; }

        public DateTime LastProcessTime { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the LoginHistory
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LoginHistory_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
            sqlCommand.Parameters.AddWithValue("@SessionId", this.SessionId);
            sqlCommand.Parameters.AddWithValue("@UserHost", this.UserHost);
            sqlCommand.Parameters.AddWithValue("@BrowserInfo", this.BrowserInfo);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            LoginDBOperation dbOperation = new LoginDBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the LoginHistory
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LoginHistory_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool CheckCanProceesRequest(string sessionId, int userId)
        {
            SqlCommand sqlCommand = new SqlCommand("LoginHistory_CheckCanProcees");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@UserId", userId);
            sqlCommand.Parameters.AddWithValue("@SessionId", sessionId);
            LoginDBOperation dbOperation = new LoginDBOperation();
            DataTable dt = dbOperation.GetTableData(sqlCommand);
            if (Convert.ToString(dt.Rows[0]["CheckValue"]) == "Can Process")
            {
                return true;
            }
            else
                return false;
        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the LoginHistory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int userId)
        {

            SqlCommand sqlCommand = new SqlCommand("LoginHistory_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@userId", userId);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
         }


        #endregion

    }
}

