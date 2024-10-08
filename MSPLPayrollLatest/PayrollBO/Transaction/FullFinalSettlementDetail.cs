// -----------------------------------------------------------------------
// <copyright file="FullFinalSettlementDetail.cs" company="Microsoft">
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
    /// To handle the FullFinalSettlementDetail
    /// </summary>
    public class FullFinalSettlementDetail
    {

        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public FullFinalSettlementDetail()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public FullFinalSettlementDetail(Guid id, Guid FullFinalSettlementId)
        {
            this.Id = id;
            this.FullFinalSettlementId = FullFinalSettlementId;
            DataTable dtValue = this.GetTableValues(this.Id, this.FullFinalSettlementId);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FullFinalSettlementId"])))
                    this.FullFinalSettlementId = new Guid(Convert.ToString(dtValue.Rows[0]["FullFinalSettlementId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Amount"])))
                    this.Amount = Convert.ToDecimal(dtValue.Rows[0]["Amount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TaxAmount"])))
                    this.TaxAmount = Convert.ToDecimal(dtValue.Rows[0]["TaxAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Get or Set the FullFinalSettlementId
        /// </summary>
        public Guid FullFinalSettlementId { get; set; }
        /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }
        /// <summary>
        /// Get or Set the Month
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Get or Set the Year
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Get or Set the Amount
        /// </summary>
        public Decimal Amount { get; set; }
        /// <summary>
        /// Get or Set the TaxAmount
        /// </summary>
        public Decimal TaxAmount { get; set; }
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
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the IncrementDetail
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlementDetail_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id.ToString() == Guid.Empty.ToString() ? null : this.Id.ToString());
            sqlCommand.Parameters.AddWithValue("@FullFinalSettlementId", this.FullFinalSettlementId.ToString() == Guid.Empty.ToString() ? null : this.FullFinalSettlementId.ToString());
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId.ToString() == Guid.Empty.ToString() ? null : this.AttributeModelId.ToString());
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@Amount", this.Amount);
            sqlCommand.Parameters.AddWithValue("@TaxAmount", this.TaxAmount);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        /// Delete the FullFinalSettlementDetail
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlementDetail_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        #endregion

        #region private methods


        /// <summary>
        /// Select the FullFinalSettlementDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        protected internal DataTable GetTableValues(Guid id, Guid FullFinalSettlementId)
        {

            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlementDetail_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@FullFinalSettlementId", FullFinalSettlementId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }

}