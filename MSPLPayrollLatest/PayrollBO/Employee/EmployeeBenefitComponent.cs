// -----------------------------------------------------------------------
// <copyright file="EmployeeBenefitComponent.cs" company="Microsoft">
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
    /// To handle the EmployeeBenefitComponent
    /// </summary>
    public class EmployeeBenefitComponent
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeBenefitComponent()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EmployeeBenefitComponent(Guid employeeId, Guid id)
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BenefitComponentId"])))
                    this.BenefitComponentId = new Guid(Convert.ToString(dtValue.Rows[0]["BenefitComponentId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Amount"])))
                    this.Amount = Convert.ToDecimal(dtValue.Rows[0]["Amount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveDate"])))
                    this.EffectiveDate = Convert.ToDateTime(dtValue.Rows[0]["EffectiveDate"]);
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
        /// Get or Set the ComponentName
        /// </summary>
        public Guid BenefitComponentId { get; set; }

        /// <summary>
        /// Get or Set the Amount
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// Get or Set the EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; }

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

        public string ImportOption { get; set; }
        public string Query { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the Emp_BenefitComponent
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_BenefitComponent_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@BenefitComponentId", this.BenefitComponentId);
            sqlCommand.Parameters.AddWithValue("@Amount", this.Amount);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.EffectiveDate);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
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
        /// Delete the Emp_BenefitComponent
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_BenefitComponent_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_BenefitComponent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_BenefitComponent_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

