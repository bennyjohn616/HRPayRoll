// -----------------------------------------------------------------------
// <copyright file="UserSession.cs" company="Microsoft">
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

    /// <summary>
    /// To handle the UserSession
    /// </summary>
    public class UserSession
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public UserSession()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="sessionid"></param>
        public UserSession(Guid sessionid)
        {
            this.SessionId = sessionid;
            DataTable dtValue = this.GetTableValues(this.SessionId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SessionId"])))
                    this.SessionId = new Guid(Convert.ToString(dtValue.Rows[0]["SessionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserId"])))
                    this.UserId = Convert.ToInt32(dtValue.Rows[0]["UserId"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the SessionId
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Get or Set the UserId
        /// </summary>
        public int UserId { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the UserSession
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("UserSession_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SessionId", this.SessionId);
            sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
            sqlCommand.Parameters.Add("@SessionIdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@SessionIdOut");
            if (status)
            {
                this.SessionId = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the UserSession
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("UserSession_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SessionId", this.SessionId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the UserSession
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid sessionid)
        {

            SqlCommand sqlCommand = new SqlCommand("UserSession_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SessionId", sessionid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

