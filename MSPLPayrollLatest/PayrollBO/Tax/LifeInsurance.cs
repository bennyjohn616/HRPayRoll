// -----------------------------------------------------------------------
// <copyright file="LifeInsurance.cs" company="Microsoft">
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
    /// To handle the LifeInsurance
    /// </summary>
    public class LifeInsurance
    {
     

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LifeInsurance()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LifeInsurance(int companyId, Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(companyId, this.Id, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinancialYearId"])))
                    this.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinancialYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.PolicyNumber = Convert.ToString(dtValue.Rows[0]["PolicyNumber"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PolicyDate"])))
                    this.PolicyDate = Convert.ToDateTime(dtValue.Rows[0]["PolicyDate"]);
                this.InsuredPersonName = Convert.ToString(dtValue.Rows[0]["InsuredPersonName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDisabilityPerson"])))
                    this.IsDisabilityPerson = Convert.ToInt16(dtValue.Rows[0]["IsDisabilityPerson"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPersonTakingTreatement"])))
                    this.IsPersonTakingTreatement = Convert.ToInt16(dtValue.Rows[0]["IsPersonTakingTreatement"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PremiumAmount"])))
                    this.PremiumAmount = Convert.ToDecimal(dtValue.Rows[0]["PremiumAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AnnualPremium"])))
                    this.annualPremium = Convert.ToDecimal(dtValue.Rows[0]["AnnualPremium"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PremiumAmountFallingDueInFeb"])))
                    this.PremiumAmountFallingDueInFeb = Convert.ToDecimal(dtValue.Rows[0]["PremiumAmountFallingDueInFeb"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PremiumConsideredForDeduction"])))
                    this.PremiumConsideredForDeduction = Convert.ToDecimal(dtValue.Rows[0]["PremiumConsideredForDeduction"]);
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
        /// Get or Set the LifeInsuranceId
        /// </summary>
        public Guid LifeInsuranceId { get; set; }
       

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the FinancialYearId
        /// </summary>
        public Guid FinancialYearId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the PolicyNumber
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Get or Set the PolicyDate
        /// </summary>
        public DateTime PolicyDate { get; set; }
        /// <summary>
        /// Get or Set the PremiumDate
        /// </summary>
        public DateTime PremiumDate { get; set; }
        
        /// <summary>
        /// Get or Set the InsuredPersonName
        /// </summary>
        public string InsuredPersonName { get; set; }

        /// <summary>
        /// Get or Set the IsDisabilityPerson
        /// </summary>
        public int IsDisabilityPerson { get; set; }

        /// <summary>
        /// Get or Set the IsPersonTakingTreatement
        /// </summary>
        public int IsPersonTakingTreatement { get; set; }

        /// <summary>
        /// Get or Set the PremiumAmount
        /// </summary>
        public decimal PremiumAmount { get; set; }
        /// <summary>
        /// Get or Set the PremiumAmountFallingDueInFeb
        /// </summary>
        public decimal PremiumAmountFallingDueInFeb { get; set; }

        /// <summary>
        /// Get or Set the PremiumConsideredForDeduction
        /// </summary>
        public decimal PremiumConsideredForDeduction { get; set; }

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

        public decimal annualPremium { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        public Guid SectionId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Relationship { get; set; }
        public decimal SumAssured { get; set; }
        public decimal Premiumdeduction { get; set; }
        public decimal totaldeclarevalue { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the LifeInsurance
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("LifeInsurance_Save");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", this.Id);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@FinancialYearId", this.FinancialYearId);
                sqlCommand.Parameters.AddWithValue("@SectionId", this.SectionId);
                sqlCommand.Parameters.AddWithValue("@Relationship", this.Relationship);
                sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
                sqlCommand.Parameters.AddWithValue("@PolicyNumber", this.PolicyNumber);
                sqlCommand.Parameters.AddWithValue("@PolicyDate", this.PolicyDate);
                sqlCommand.Parameters.AddWithValue("@InsuredPersonName", this.InsuredPersonName);
                sqlCommand.Parameters.AddWithValue("@IsDisabilityPerson", this.IsDisabilityPerson);
                sqlCommand.Parameters.AddWithValue("@IsPersonTakingTreatement", this.IsPersonTakingTreatement);
                sqlCommand.Parameters.AddWithValue("@PremiumAmount", this.PremiumAmount);
                sqlCommand.Parameters.AddWithValue("@SumAssured", this.SumAssured);
                sqlCommand.Parameters.AddWithValue("@PremiumAmountFallingDueInFeb", this.PremiumAmountFallingDueInFeb);
                sqlCommand.Parameters.AddWithValue("@PremiumConsideredForDeduction", this.PremiumConsideredForDeduction);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
                sqlCommand.Parameters.AddWithValue("@Month", this.Month);
                sqlCommand.Parameters.AddWithValue("@Year", this.Year);
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
        ///<summary>
        /// Save life insurance premium 
        ///</summary>
        public bool SaveLICPremium()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("LifeInsurancePremium_Save");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", this.Id);
                sqlCommand.Parameters.AddWithValue("@LifeInsuranceId", this.LifeInsuranceId);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@FinancialYearId", this.FinancialYearId);
                sqlCommand.Parameters.AddWithValue("@PolicyNumber", this.PolicyNumber);
                sqlCommand.Parameters.AddWithValue("@PremiumDate", this.PremiumDate);
                sqlCommand.Parameters.AddWithValue("@PremiumAmount", this.PremiumAmount);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
                sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
                sqlCommand.Parameters.AddWithValue("@Totalpremium", 0);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete the LifeInsurance
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LifeInsurance_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        public bool Deletepremiumpolicy()
        {

            SqlCommand sqlCommand = new SqlCommand("LifeInsurancepolicy_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@Totalpremium", 0);
            sqlCommand.Parameters.AddWithValue("@LICID", this.LifeInsuranceId);
            sqlCommand.Parameters.AddWithValue("@PolicyNumber", this.PolicyNumber);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the LifeInsurance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId, Guid id, Guid employeeId, Guid FinancialYearId)
        {

            SqlCommand sqlCommand = new SqlCommand("LifeInsurance_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValuesLICPremium(Guid id, Guid employeeId, Guid FinancialYearId,Guid TXLICId)
        {

            SqlCommand sqlCommand = new SqlCommand("LifeInsurancePremium_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@TXLICId", TXLICId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        public DataTable GetLicValuesForReport(Guid fin, DateTime date, string scode, string ecpde, int cid)
        {

            SqlCommand sqlCommand = new SqlCommand("LifeInsuranceReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", cid);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", fin);
            sqlCommand.Parameters.AddWithValue("@Month", date.Month);
            sqlCommand.Parameters.AddWithValue("@Year", date.Year);
            sqlCommand.Parameters.AddWithValue("@sCode", scode);
            sqlCommand.Parameters.AddWithValue("@eCode", ecpde);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

