// -----------------------------------------------------------------------
// <copyright file="PayrollHistoryValue.cs" company="Microsoft">
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
    /// To handle the PayrollHistoryValue
    /// </summary>
    public class PayrollHistoryValue
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PayrollHistoryValue()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PayrollHistoryValue(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, Guid.Empty, Guid.Empty, 0, 0);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"])))
                    this.PayrollHistoryId = new Guid(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                this.BaseValue = Convert.ToString(dtValue.Rows[0]["Basevalue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ValueType"])))
                    this.ValueType = Convert.ToInt32(dtValue.Rows[0]["ValueType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
            }
        }

        public PayrollHistoryValue(Guid payrollHistoryId, Guid attributeModelId)
        {
            this.PayrollHistoryId = payrollHistoryId;
            this.AttributeModelId = attributeModelId;
            DataTable dtValue = this.GetTableValues(Guid.Empty, this.PayrollHistoryId, this.AttributeModelId, 0, 0);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"])))
                    this.PayrollHistoryId = new Guid(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                this.BaseValue = Convert.ToString(dtValue.Rows[0]["Basevalue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ValueType"])))
                    this.ValueType = Convert.ToInt32(dtValue.Rows[0]["ValueType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the PayrollHistoryId
        /// </summary>
        public Guid PayrollHistoryId { get; set; }

        /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the ValueType
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

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

        public string BaseValue { get; set; }
        public bool IncludeGrossPay { get; set; }
        public bool IsTaxable { get; set; }
        public string BehaviorType { get; set; }
        public string Importxmlstring { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the PayrollHistoryValue
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistoryValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PayrollHistoryId", this.PayrollHistoryId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@ValueType", this.ValueType);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@BaseValue", this.BaseValue);
            sqlCommand.Parameters.AddWithValue("@IsTaxable", this.IsTaxable);
            sqlCommand.Parameters.AddWithValue("@IncludeGrossPay", this.IncludeGrossPay);
            sqlCommand.Parameters.AddWithValue("@BehaviorType", this.BehaviorType);
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
        /// Delete the PayrollHistoryValue
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistoryValue_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PayrollHistoryValue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid payrollHistoryId, Guid attributeModelId, int month, int year)
        {
            SqlCommand sqlCommand = new SqlCommand("PayrollHistoryValue_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@PayrollHistoryId", payrollHistoryId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", attributeModelId);
            sqlCommand.Parameters.AddWithValue("@Month", month);
            sqlCommand.Parameters.AddWithValue("@Year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public  bool ImportpayValues()
        {
            string status;
            SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_PayrollHistoryValue");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@xmlstring", this.Importxmlstring);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.SaveData(sqlCommand,out status);
      
        }

        #endregion

    }
}

