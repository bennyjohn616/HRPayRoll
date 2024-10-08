// -----------------------------------------------------------------------
// <copyright file="LoanMaster.cs" company="Microsoft">
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
    /// To handle the LoanMaster
    /// </summary>
    public class LoanMaster
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoanMaster()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LoanMaster(Guid id, int companyId)
        {
            this.Id = id;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.CompanyId, this.Id, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.LoanCode = Convert.ToString(dtValue.Rows[0]["LoanCode"]);
                this.LoanDesc = Convert.ToString(dtValue.Rows[0]["LoanDesc"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsInterest"])))
                    this.IsInterest = Convert.ToBoolean(dtValue.Rows[0]["IsInterest"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["InterestPercent"])))
                    this.InterestPercent = Convert.ToDouble(dtValue.Rows[0]["InterestPercent"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EligComponentId"])))
                    this.loanEligComp =new  Guid(dtValue.Rows[0]["EligComponentId"].ToString());
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
            }
        }

        public LoanMaster(int companyId, Guid attributeModelId)
        {
            this.AttributeModelId = attributeModelId;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.CompanyId, Guid.Empty, this.AttributeModelId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.LoanCode = Convert.ToString(dtValue.Rows[0]["LoanCode"]);
                this.LoanDesc = Convert.ToString(dtValue.Rows[0]["LoanDesc"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsInterest"])))
                    this.IsInterest = Convert.ToBoolean(dtValue.Rows[0]["IsInterest"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["InterestPercent"])))
                    this.InterestPercent = Convert.ToDouble(dtValue.Rows[0]["InterestPercent"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }
        public Guid EligMasterId { get; set; }
        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the LoanCode
        /// </summary>
        public string LoanCode { get; set; }

        /// <summary>
        /// Get or Set the LoanDesc
        /// </summary>
        public string LoanDesc { get; set; }

        /// <summary>
        /// Get or Set the Interest
        /// </summary>
        public bool IsInterest { get; set; }

        /// <summary>
        /// Get or Set the InterestPercent
        /// </summary>
        public double InterestPercent { get; set; }

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
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// get or set the Attribute model Id 
        /// </summary>
        public Guid AttributeModelId { get; set; }
        public Guid loanEligComp { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the LoanMaster
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanMaster_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@LoanCode", this.LoanCode);
            sqlCommand.Parameters.AddWithValue("@LoanDesc", this.LoanDesc);
            sqlCommand.Parameters.AddWithValue("@IsInterest", this.IsInterest);
            sqlCommand.Parameters.AddWithValue("@InterestPercent", this.InterestPercent);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
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

        public bool SaveLoanEligSet()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanEligSet_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.EligMasterId);
            sqlCommand.Parameters.AddWithValue("@LoanMasterId", this.Id);
            sqlCommand.Parameters.AddWithValue("@IsInterest", this.IsInterest);
            sqlCommand.Parameters.AddWithValue("@InterestPercent", this.InterestPercent);
            sqlCommand.Parameters.AddWithValue("@EligComponentId", this.loanEligComp);
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
        /// Delete the LoanMaster
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanMaster_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the LoanMaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId, Guid id, Guid AttributeModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("LoanMaster_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

