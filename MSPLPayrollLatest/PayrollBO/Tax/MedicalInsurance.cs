namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    public class MedicalInsurance
    {
        #region private variable


        #endregion

        #region "Constructor"
        public MedicalInsurance()
        {

        }
        public MedicalInsurance(Guid EmployeeId, int companyId, int EffectiveMonth, int EffectiveYear, string Fieldname)
        {

            DataTable dtValue = this.GetTableValues(EmployeeId,  companyId, EffectiveMonth,  EffectiveYear, Fieldname);
            if (dtValue.Rows.Count > 0)
            {
               
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TxSectionId"])))
                    this.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[0]["TxSectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinYearId"])))
                    this.FinYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveMonth"])))
                    this.EffectiveMonth = Convert.ToInt32(dtValue.Rows[0]["EffectiveMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveYear"])))
                    this.EffectiveYear = Convert.ToInt32(dtValue.Rows[0]["EffectiveYear"]);
                this.InsuranceType = Convert.ToString(dtValue.Rows[0]["InsuranceType"]);
                this.PolicyNo= Convert.ToString(dtValue.Rows[0]["PolicyNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateofCommencofpolicy"])))
                    this.DateofCommencofpolicy = Convert.ToDateTime(dtValue.Rows[0]["DateofCommencofpolicy"]);
                this.InsuredPersonName = Convert.ToString(dtValue.Rows[0]["InsuredPersonName"]);
                this.RelationshipoftheInsuredperson = Convert.ToString(dtValue.Rows[0]["RelationshipoftheInsuredperson"]);
                this.CoveredinthepolicyisSeniorCitizen = Convert.ToString(dtValue.Rows[0]["CoveredinthepolicyisSeniorCitizen"]);
                this.IncurredinrespectofVerySeniorCitizen = Convert.ToString(dtValue.Rows[0]["IncurredinrespectofVerySeniorCitizen"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AmountofpremiumorExpense"])))
                    this.AmountofpremiumorExpense = Convert.ToDecimal(dtValue.Rows[0]["AmountofpremiumorExpense"]);
                this.PayMode = Convert.ToString(dtValue.Rows[0]["PayMode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EligibleDeductionforthepolicy"])))
                    this.EligibleDeductionforthepolicy = Convert.ToDecimal(dtValue.Rows[0]["EligibleDeductionforthepolicy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SelfSpouseChildOveralldeduction"])))
                    this.SelfSpouseChildOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["SelfSpouseChildOveralldeduction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentOveralldeduction"])))
                    this.ParentOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["ParentOveralldeduction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TotalDeduction"])))
                    this.TotalDeduction = Convert.ToDecimal(dtValue.Rows[0]["TotalDeduction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModiifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDelete = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);

            }
        }
        public MedicalInsurance(Guid EmployeeId, int companyId, int EffectiveMonth, int EffectiveYear, Guid financialYear)
        {
            DataTable dtValue = this.GetTableValues(EmployeeId, companyId, EffectiveMonth, EffectiveYear, financialYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                   

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AmountofpremiumorExpense"])))
                        this.TotalAmount = Convert.ToDecimal(dtValue.Rows[0]["AmountofpremiumorExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EligibleDeductionforthepolicy"])))
                        this.TotalEligibleDeduction = Convert.ToDecimal(dtValue.Rows[0]["EligibleDeductionforthepolicy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SelfValue"])))
                        this.SelfSpouseChildOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["SelfValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentValue"])))
                        this.ParentOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["ParentValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Total"])))
                        this.TotalDeduction = Convert.ToDecimal(dtValue.Rows[0]["Total"]);

                   
                }
            }
        }



        #endregion

        #region property
        /// <summary>
        /// get and set the ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        ///get and set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// get and set the TxSectionId
        /// </summary>
        public Guid TxSectionId { get; set; }
        /// <summary>
        /// get and set the FinYearId
        /// </summary>
        public Guid FinYearId { get; set; }
        /// <summary>
        /// get and set the EffectiveMonth
        /// </summary>
        public int EffectiveMonth { get; set; }
        /// <summary>
        /// get and set the EffectiveYear
        /// </summary>
        public int EffectiveYear { get; set; }
        /// <summary>
        /// get and set the InsuranceType
        /// </summary>
        public string InsuranceType { get; set; }
        /// <summary>
        /// get and set the PolicyNo
        /// </summary>
        public string PolicyNo { get; set; }
        /// <summary>
        /// get and set the DateofCommencofpolicy
        /// </summary>
        public DateTime DateofCommencofpolicy { get; set; }
        /// <summary>
        /// get and set the InsuredPersonName
        /// </summary>
        public string InsuredPersonName { get; set; }
        /// <summary>
        /// get and set the RelationshipoftheInsuredperson
        /// </summary>
        public string RelationshipoftheInsuredperson { get; set; }
        /// <summary>
        /// get and set the CoveredinthepolicyisSeniorCitizen
        /// </summary>
        public string CoveredinthepolicyisSeniorCitizen { get; set; }
        /// <summary>
        /// get and set the IncurredinrespectofVerySeniorCitizen
        /// </summary>
        public string IncurredinrespectofVerySeniorCitizen { get; set; }
        /// <summary>
        /// get and set the AmountofpremiumorExpense
        /// </summary>
        public decimal AmountofpremiumorExpense { get; set; }
        /// <summary>
        /// get and set the PayMode
        /// </summary>
        public string PayMode { get; set; }
        /// <summary>
        /// get and set the EligibleDeductionforthepolicy
        /// </summary>
        public decimal EligibleDeductionforthepolicy { get; set; }
        /// <summary>
        /// get and set the SelfSpouseChildOveralldeduction
        /// </summary>
        public decimal SelfSpouseChildOveralldeduction { get; set; }
        /// <summary>
        /// get and set the ParentOveralldeduction
        /// </summary>
        public decimal ParentOveralldeduction { get; set; }
        /// <summary>
        /// get and set the TotalDeduction
        /// </summary>
        public decimal TotalDeduction { get; set; }
        /// <summary>
        /// get and set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// get and set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// get and set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// get and set the ModiifiedBy
        /// </summary>
        public int ModiifiedBy { get; set; }
        /// <summary>
        /// get and set the IsDelete
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// get and set the IsActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// get and set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public string EmployeeCode { get; set; }

        public string Employeename { get; set; }

        public string CompanyName { get; set; }

        public string PANNumber { get; set; }

        public string Financial_Year { get; set; }
        public Guid financialYear { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalEligibleDeduction { get; set; }
        


        #endregion

        #region public methods
        /// <summary>
        /// save the medical insurance
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Savemedicalinsurance_Details");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", this.Id);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@TxSectionId", this.TxSectionId);
                sqlCommand.Parameters.AddWithValue("@FinYearId", this.FinYearId);
                sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
                sqlCommand.Parameters.AddWithValue("@EffectiveYear", this.EffectiveYear);
                sqlCommand.Parameters.AddWithValue("@InsuranceType", this.InsuranceType);
                sqlCommand.Parameters.AddWithValue("@PolicyNo", this.PolicyNo);
                sqlCommand.Parameters.AddWithValue("@DateofCommencofpolicy", this.DateofCommencofpolicy);
                sqlCommand.Parameters.AddWithValue("@InsuredPersonName", this.InsuredPersonName);
                sqlCommand.Parameters.AddWithValue("@RelationshipoftheInsuredperson", this.RelationshipoftheInsuredperson);
                sqlCommand.Parameters.AddWithValue("@CoveredinthepolicyisSeniorCitizen", this.CoveredinthepolicyisSeniorCitizen);
                sqlCommand.Parameters.AddWithValue("@IncurredinrespectofVerySeniorCitizen", this.IncurredinrespectofVerySeniorCitizen);
                sqlCommand.Parameters.AddWithValue("@AmountofpremiumorExpense", this.AmountofpremiumorExpense);
                sqlCommand.Parameters.AddWithValue("@PayMode", this.PayMode);
                sqlCommand.Parameters.AddWithValue("@EligibleDeductionforthepolicy", this.EligibleDeductionforthepolicy);
                sqlCommand.Parameters.AddWithValue("@SelfSpouseChildOveralldeduction", this.SelfSpouseChildOveralldeduction);
                sqlCommand.Parameters.AddWithValue("@ParentOveralldeduction", this.ParentOveralldeduction);
                sqlCommand.Parameters.AddWithValue("@TotalDeduction", this.TotalDeduction);
                sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
                sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
                sqlCommand.Parameters.AddWithValue("@ModiifiedBy", this.ModiifiedBy);
                sqlCommand.Parameters.AddWithValue("@IsDelete", this.IsDelete);
                sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
                sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
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
        /// delete the medical insurance
        /// </summary>
        /// <returns></returns>
        public bool delete()
        {
            SqlCommand sqlCommand = new SqlCommand("Deletemedicalinsurance_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModiifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// get the values
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <param name="FinancialYearId"></param>
        /// <returns></returns>
        /// 

        public DataTable GetTableRoprotValues( int companyId, int EffectiveMonth, int EffectiveYear,string scode,string ecode )
        {
            SqlCommand sqlCommand = new SqlCommand("Getmedicalinsurancereport_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@sCode", scode);
            sqlCommand.Parameters.AddWithValue("@eCode", ecode);
            sqlCommand.Parameters.AddWithValue("@CompanyID", companyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        protected internal DataTable GetTableValues(Guid EmployeeId, int companyId ,int EffectiveMonth,int EffectiveYear , string Fieldname)
        {
            SqlCommand sqlCommand = new SqlCommand("Getmedicalinsurance_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@Action", Fieldname);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid EmployeeId, int companyId, int EffectiveMonth, int EffectiveYear , Guid financialYear)
        {
            SqlCommand sqlCommand = new SqlCommand("calculationmedicalinsurance_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@financialYear", financialYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid EmployeeId, Guid TxSectionId, Guid FinancialYear , int EffectiveMonth, int EffectiveYear, decimal TotalDeduction)
        {
            string EffMonth = Convert.ToString(EffectiveMonth);
            string EffYear = Convert.ToString(EffectiveYear);
            string EffMonthYear = EffMonth + "-" + EffYear + "-01";
            DateTime EffectiveDate = Convert.ToDateTime(EffMonthYear);

            SqlCommand sqlCommand = new SqlCommand("SubmitMedicalInsurance");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SectionId", TxSectionId);
            sqlCommand.Parameters.AddWithValue("@FinancialYear", FinancialYear);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", EffectiveDate);
            sqlCommand.Parameters.AddWithValue("@TotalDeduction", TotalDeduction);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid financialyearId, int CompanyId, string EffectiveMonth, string EffectiveYear)
        {
            SqlCommand sqlCommand = new SqlCommand("MedicalInsuranceReport");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYear", financialyearId);
            sqlCommand.Parameters.AddWithValue("@CompanyID", CompanyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }



}
