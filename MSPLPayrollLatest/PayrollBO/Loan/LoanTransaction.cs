// -----------------------------------------------------------------------
// <copyright file="LoanTransaction.cs" company="Microsoft">
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
    /// To handle the LoanTransaction
    /// </summary>
    public class LoanTransaction
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoanTransaction()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LoanTransaction(Guid id, Guid loanEntryId)
        {
            this.Id = id;
            this.LoanEntryId = loanEntryId;
            DataTable dtValue = this.GetTableValues(this.Id, this.LoanEntryId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LoanEntryId"])))
                    this.LoanEntryId = new Guid(Convert.ToString(dtValue.Rows[0]["LoanEntryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AmtPaid"])))
                    this.AmtPaid = Convert.ToDecimal(dtValue.Rows[0]["AmtPaid"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["InterestAmt"])))
                    this.InterestAmt = Convert.ToDecimal(dtValue.Rows[0]["InterestAmt"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isForClose"])))
                    this.isForClose = Convert.ToBoolean(dtValue.Rows[0]["isForClose"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isPayRollProcess"])))
                    this.isPayRollProcess = Convert.ToBoolean(dtValue.Rows[0]["isPayRollProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FAndFprocess"])))
                    this.isFandFProcessv = Convert.ToBoolean(dtValue.Rows[0]["FAndFprocess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AppliedOn"])))
                    this.AppliedOn = Convert.ToDateTime(dtValue.Rows[0]["AppliedOn"]);
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
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the LoanEntryId
        /// </summary>
        public Guid LoanEntryId { get; set; }

        /// <summary>
        /// Get or Set the AmtPaid
        /// </summary>
        public Decimal AmtPaid { get; set; }

        public Decimal InterestAmt { get; set; }

        /// <summary>
        /// Get or Set the isForClose
        /// </summary>
        public bool isForClose { get; set; }

        /// <summary>
        /// Get or Set the isPayRollProcess
        /// </summary>
        public bool isPayRollProcess { get; set; }

        public bool isFandFProcessv { get; set; }

        /// <summary>
        /// Get or Set the AppliedOn
        /// </summary>
        public DateTime AppliedOn { get; set; }

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

        /// <summary>
        /// Get or Set the Status
        /// </summary>
        public string Status { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the LoanTransaction
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanTransaction_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@LoanEntryId", this.LoanEntryId);
            sqlCommand.Parameters.AddWithValue("@AmtPaid", this.AmtPaid);
            sqlCommand.Parameters.AddWithValue("@InterestAmt", this.InterestAmt);
            sqlCommand.Parameters.AddWithValue("@isForClose", this.isForClose);
            sqlCommand.Parameters.AddWithValue("@isPayRollProcess", this.isPayRollProcess);
            sqlCommand.Parameters.AddWithValue("@isFandFProcess", this.isFandFProcessv);
            sqlCommand.Parameters.AddWithValue("@AppliedOn", this.AppliedOn);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
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

        public bool UpdateLoanTransDev()
        {
            SqlCommand sqlCommand = new SqlCommand("LoanTransDevUpdate");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LoanEntryId", this.LoanEntryId);
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        /// <summary>
        /// Delete the LoanTransaction
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanTransaction_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the LoanTransaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid loanEntryId)
        {

            SqlCommand sqlCommand = new SqlCommand("LoanTransaction_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@LoanEntryId", loanEntryId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

