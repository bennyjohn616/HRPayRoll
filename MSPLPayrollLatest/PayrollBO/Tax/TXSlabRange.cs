// -----------------------------------------------------------------------
// <copyright file="TXSlabRange.cs" company="Microsoft">
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
    /// To handle the TXSlabRange
    /// </summary>
    public class TXSlabRange
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSlabRange()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSlabRange(int id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, 0, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SlabCategoryId"])))
                    this.SlabCategoryId = Convert.ToInt32(dtValue.Rows[0]["SlabCategoryId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinancialYearId"])))
                    this.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinancialYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeFrom"])))
                    this.RangeFrom = Convert.ToDecimal(dtValue.Rows[0]["RangeFrom"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeTo"])))
                    this.RangeTo = Convert.ToDecimal(dtValue.Rows[0]["RangeTo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TaxValue"])))
                    this.TaxValue = Convert.ToDecimal(dtValue.Rows[0]["TaxValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPercentage"])))
                    this.IsPercentage = Convert.ToBoolean(dtValue.Rows[0]["IsPercentage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the SlabCategoryId
        /// </summary>
        public int SlabCategoryId { get; set; }

        /// <summary>
        /// Get or Set the FinancialYearId
        /// </summary>
        public Guid FinancialYearId { get; set; }

        /// <summary>
        /// Get or Set the RangeFrom
        /// </summary>
        public Decimal RangeFrom { get; set; }

        /// <summary>
        /// Get or Set the RangeTo
        /// </summary>
        public Decimal RangeTo { get; set; }

        /// <summary>
        /// Get or Set the TaxValue
        /// </summary>
        public Decimal TaxValue { get; set; }

        /// <summary>
        /// Get or Set the IsPercentage
        /// </summary>
        public bool IsPercentage { get; set; }

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
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXSlabRange
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXSlabRange_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@SlabCategoryId", this.SlabCategoryId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", this.FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@RangeFrom", this.RangeFrom);
            sqlCommand.Parameters.AddWithValue("@RangeTo", this.RangeTo);
            sqlCommand.Parameters.AddWithValue("@TaxValue", this.TaxValue);
            sqlCommand.Parameters.AddWithValue("@IsPercentage", this.IsPercentage);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the TXSlabRange
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXSlabRange_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXSlabRange
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int slabCategoryId, Guid financialYearId)
        {

            SqlCommand sqlCommand = new SqlCommand("TXSlabRange_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@SlabCategoryId", slabCategoryId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", financialYearId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

