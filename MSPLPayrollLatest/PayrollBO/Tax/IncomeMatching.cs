using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class IncomeMatching
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public IncomeMatching()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public IncomeMatching(Guid financeyearid, Guid attributeId)
        {
            this.FinancialYearId = financeyearid;
            this.AttributemodelId = attributeId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributemodelId"])))
                    this.AttributemodelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributemodelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYearId"])))
                    this.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MatchingComponent"])))
                    this.MatchingComponent = new Guid(Convert.ToString(dtValue.Rows[0]["MatchingComponent"]));
               
               this.OtherComponent = Convert.ToString(dtValue.Rows[0]["OtherComponent"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ExemptionComponent"])))
                    this.ExemptionComponent = new Guid(Convert.ToString(dtValue.Rows[0]["ExemptionComponent"]));
                this.Formula = Convert.ToString(dtValue.Rows[0]["Formula"]);
                this.Projection = Convert.ToBoolean(dtValue.Rows[0]["Projection"]);
                this.Operator = Convert.ToString(dtValue.Rows[0]["Operator"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["OrderNo"])))
                    this.OrderNo = Convert.ToInt32(dtValue.Rows[0]["OrderNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["GrossSection"])))
                    this.GrossSection = Convert.ToInt32(dtValue.Rows[0]["GrossSection"]);
                this.TaxDeductionMode = Convert.ToString(dtValue.Rows[0]["TaxDeductionMode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
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
        /// Get or Set the FinancialYearId
        /// </summary>
        public Guid FinancialYearId { get; set; }

        /// <summary>
        /// Get or Set the AttributemodelId
        /// </summary>
        public Guid AttributemodelId { get; set; }

        /// <summary>
        /// Get or Set the Projection
        /// </summary>
        public bool Projection { get; set; }

        /// <summary>
        /// Get or Set the MatchingComponent
        /// </summary>
        public Guid MatchingComponent { get; set; }

        /// <summary>
        /// Get or Set the ExamptionComponent
        /// </summary>
        public Guid ExemptionComponent { get; set; }

        /// <summary>
        /// Get or Set the OtherComponent
        /// </summary>
        public string OtherComponent { get; set; }

        /// <summary>
        /// Get or Set the Opearator
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Get or Set the TaxDeductionMode
        /// </summary>
        public string TaxDeductionMode { get; set; }

        public int GrossSection { get; set; }

        public int OrderNo { get; set; }
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

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the IncomeMatching
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TxIncomeMatching_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", this.AttributemodelId);
            
            sqlCommand.Parameters.AddWithValue("@Projection", this.Projection);
            sqlCommand.Parameters.AddWithValue("@MatchingComponent", this.MatchingComponent);
            sqlCommand.Parameters.AddWithValue("@ExemptionComponent", this.ExemptionComponent);
            sqlCommand.Parameters.AddWithValue("@Operator", this.Operator);
            sqlCommand.Parameters.AddWithValue("@Formula", this.Formula);
            sqlCommand.Parameters.AddWithValue("@TaxDeductionMode", this.TaxDeductionMode);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@OtherComponent", this.OtherComponent);
            sqlCommand.Parameters.AddWithValue("@GrossSection", this.GrossSection);
            sqlCommand.Parameters.AddWithValue("@OrderNo", this.OrderNo);

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
        /// Delete the IncomeMatching
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TxIncomeMatching_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the IncomeMatching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXIncomeMatching_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", this.AttributemodelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
