using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxBehavior
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TaxBehavior()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TaxBehavior(Guid id, Guid attributeId)
        {
            this.Id = id;
            this.AttributemodelId = attributeId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYearId"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributemodelId"])))
                    this.AttributemodelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributemodelId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                this.Formula = Convert.ToString(dtValue.Rows[0]["Formula"]);
                this.FieldFor = Convert.ToString(dtValue.Rows[0]["FieldFor"]);
                this.BaseValue = Convert.ToString(dtValue.Rows[0]["BaseValue"]);
                this.BaseFormula = Convert.ToString(dtValue.Rows[0]["BaseFormula"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SlabCategory"])))
                    this.SlabCategory = Convert.ToInt32(dtValue.Rows[0]["SlabCategory"]);
                this.FieldType = Convert.ToString(dtValue.Rows[0]["FieldType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["InputType"])))
                    this.InputType = Convert.ToInt32(dtValue.Rows[0]["InputType"]);
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
        public Guid FinanceYearId { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public Guid AttributemodelId { get; set; }

        public string Attributename { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the OrderNo
        /// </summary>
        public int SlabCategory { get; set; }

        /// <summary>
        /// Get or Set the Limit
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the ExemptionType
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Get or Set the IsGrossDeductable
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Get or Set the IsDocumentRequired
        /// </summary>
        public int InputType { get; set; }


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

        public string FieldFor { get; set; }

        public string BaseValue { get; set; }
        public string BaseFormula { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEntityBehaviour_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", this.AttributemodelId);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@SlabCategory", this.SlabCategory);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@Formula", this.Formula);
            sqlCommand.Parameters.AddWithValue("@FieldFor", this.FieldFor);
            sqlCommand.Parameters.AddWithValue("@FieldType", this.FieldType);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@InputType", this.InputType);
            sqlCommand.Parameters.AddWithValue("@BaseValue", this.BaseValue);
            sqlCommand.Parameters.AddWithValue("@BaseFormula", this.BaseFormula);
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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEntityBehaviour_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TaxBehavior
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEntityBehaviour_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", this.AttributemodelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
