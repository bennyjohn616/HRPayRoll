// -----------------------------------------------------------------------
// <copyright file="EmployeeLanguageKnown.cs" company="Microsoft">
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
    /// To handle the EmployeeLanguageKnown
    /// </summary>
    public class EmployeeLanguageKnown
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeLanguageKnown()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="id"></param>
        public EmployeeLanguageKnown(Guid employeeId, Guid id)
        {
            this.Id = id;
            this.EmployeeId = employeeId;
            DataTable dtValue = this.GetTableValues(this.EmployeeId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LanguageId"])))
                    this.LanguageId = Convert.ToInt32(dtValue.Rows[0]["LanguageId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsSpeak"])))
                    this.IsSpeak = Convert.ToBoolean(dtValue.Rows[0]["IsSpeak"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsRead"])))
                    this.IsRead = Convert.ToBoolean(dtValue.Rows[0]["IsRead"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsWrite"])))
                    this.IsWrite = Convert.ToBoolean(dtValue.Rows[0]["IsWrite"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                this.Language = Convert.ToString(dtValue.Rows[0]["Language"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the LanguageId
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Get or Set the IsSpeak
        /// </summary>
        public bool IsSpeak { get; set; }

        /// <summary>
        /// Get or Set the IsRead
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Get or Set the IsWrite
        /// </summary>
        public bool IsWrite { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public string Language { get; set; }
        public string ImportOption { get; set; }
        public string Query { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the Emp_LanguageKnown
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_LanguageKnown_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LanguageId", this.LanguageId);
            sqlCommand.Parameters.AddWithValue("@IsSpeak", this.IsSpeak);
            sqlCommand.Parameters.AddWithValue("@IsRead", this.IsRead);
            sqlCommand.Parameters.AddWithValue("@IsWrite", this.IsWrite);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");

            //if (ImportOption == "EmployeeLanguageKnown")
            //{
            //    string Query = string.Empty;
            //    string Query1 = string.Empty;
            //    Query = "update Emp_LanguageKnown set ";
            //    Query = Query + " [EmployeeId] =  '" + this.EmployeeId.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.LanguageId.ToString()))
            //        Query = Query + " ,[LanguageId] =  '" + this.LanguageId.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.IsSpeak.ToString()))
            //        Query = Query + " ,[IsSpeak] =  '" + this.IsSpeak.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.IsRead.ToString()))
            //        Query = Query + " ,[IsRead] =  '" + this.IsRead.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.IsWrite.ToString()))
            //        Query = Query + " ,[IsWrite] =  '" + this.IsWrite.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
            //        Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";
            //    Query = Query + " ,[ModifiedOn] = GETDATE()";
            //    Query = Query + "  Where  EmployeeId ='" + this.EmployeeId.ToString() + "' and  LanguageId = '" + this.LanguageId.ToString() + "' and IsDeleted=0 ";
            //}
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Emp_LanguageKnown
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_LanguageKnown_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_LanguageKnown
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_LanguageKnown_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

