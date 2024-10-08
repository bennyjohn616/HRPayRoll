// -----------------------------------------------------------------------
// <copyright file="ApprovelTransaction.cs" company="Microsoft">
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
    /// To handle the ApprovelTransaction
    /// </summary>
    public class ApprovelTransaction
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ApprovelTransaction()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ApprovelTransaction(Guid id,int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, companyId,"","","");
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.TablePrimaryId = Convert.ToString(dtValue.Rows[0]["TablePrimaryId"]);
                this.TableSubPrimaryId = Convert.ToString(dtValue.Rows[0]["TableSubPrimaryId"]);
                this.TableName = Convert.ToString(dtValue.Rows[0]["TableName"]);
                this.ColumnName = Convert.ToString(dtValue.Rows[0]["ColumnName"]);
                this.OldValue = Convert.ToString(dtValue.Rows[0]["OldValue"]);
                this.NewValue = Convert.ToString(dtValue.Rows[0]["NewValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsApproved"])))
                    this.IsApproved = Convert.ToBoolean(dtValue.Rows[0]["IsApproved"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApproverId"])))
                    this.ApproverId = Convert.ToInt32(dtValue.Rows[0]["ApproverId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
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
        /// Get or Set the TablePrimaryId
        /// </summary>
        public string TablePrimaryId { get; set; }

        /// <summary>
        /// Get or Set the TableSubPrimaryId
        /// </summary>
        public string TableSubPrimaryId { get; set; }

        /// <summary>
        /// Get or Set the TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get or Set the ColumnName
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Get or Set the OldValue
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Get or Set the NewValue
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Get or Set the IsApproved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Get or Set the ApproverId
        /// </summary>
        public int ApproverId { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the ApprovelTransaction
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ApprovelTransaction_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@TablePrimaryId", this.TablePrimaryId);
            sqlCommand.Parameters.AddWithValue("@TableSubPrimaryId", this.TableSubPrimaryId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@ColumnName", this.ColumnName);
            sqlCommand.Parameters.AddWithValue("@OldValue", this.OldValue);
            sqlCommand.Parameters.AddWithValue("@NewValue", this.NewValue);
            sqlCommand.Parameters.AddWithValue("@IsApproved", this.IsApproved);
            sqlCommand.Parameters.AddWithValue("@ApproverId", this.ApproverId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the ApprovelTransaction
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("ApprovelTransaction_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the ApprovelTransaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id ,int companyId,string tableprimaryid,string tableName,string columnName)
        {

            SqlCommand sqlCommand = new SqlCommand("ApprovelTransaction_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@TablePrimaryId", tableprimaryid);
            sqlCommand.Parameters.AddWithValue("@TableName", tableName);
            sqlCommand.Parameters.AddWithValue("@ColumnName", columnName);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

