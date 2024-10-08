// -----------------------------------------------------------------------
// <copyright file="Company.cs" company="Microsoft">
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
    /// To handle the Company
    /// </summary>
    public class CompanyList : List<Company>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public CompanyList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="userId"></param>
        public CompanyList(int id,int userId)
        {
            this.UserId = userId;
            Company company = new Company();
            DataTable dtValue = company.GetTableValues(id, userId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Company companytemp = new Company();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        companytemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    companytemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                    companytemp.AddressLine1 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine1"]);
                    companytemp.AddressLine2 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine2"]);
                    companytemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    companytemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    companytemp.Country = Convert.ToString(dtValue.Rows[rowcount]["Country"]);
                    companytemp.PinCode = Convert.ToString(dtValue.Rows[rowcount]["PinCode"]);
                    companytemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    companytemp.EMail = Convert.ToString(dtValue.Rows[rowcount]["EMail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        companytemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        companytemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        companytemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        companytemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        companytemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        companytemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PrimaryCompanyId"])))
                        companytemp.PrimaryCompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PrimaryCompanyId"]);
                    companytemp.PFBankName = Convert.ToString(dtValue.Rows[rowcount]["PFBankName"]);
                    companytemp.PFBankAddress = Convert.ToString(dtValue.Rows[rowcount]["PFBankAddress"]);
                    companytemp.GroupCode = Convert.ToString(dtValue.Rows[rowcount]["GroupCode"]);
                    companytemp.PFEmployeerCode = Convert.ToString(dtValue.Rows[rowcount]["PFEmployeerCode"]);
                    companytemp.PensionFundAcNo = Convert.ToString(dtValue.Rows[rowcount]["PensionFundAcNo"]);
                    companytemp.EPFAcNo = Convert.ToString(dtValue.Rows[rowcount]["EPFAcNo"]);
                    companytemp.AdminChargeAcNo = Convert.ToString(dtValue.Rows[rowcount]["AdminChargeAcNo"]);
                    companytemp.InspectionChargeAcNo = Convert.ToString(dtValue.Rows[rowcount]["InspectionChargeAcNo"]);
                    companytemp.EDLIAcNo = Convert.ToString(dtValue.Rows[rowcount]["EDLIAcNo"]);
                    companytemp.ESIEmployeerContribution = Convert.ToString(dtValue.Rows[rowcount]["ESIEmployeerContribution"]);
                    companytemp.PayrollProcessBy = Convert.ToString(dtValue.Rows[rowcount]["PayrollProcessBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyLogo"])))
                    //    companytemp.Companylogo = Convert.ToString(dtValue.Rows[rowcount]["CompanyLogo"]);

                    this.Add(companytemp);
                }
            }
        }

        public CompanyList(int id)
        {
           // this.UserId = userId;
            Company company = new Company();
            DataTable dtValue = company.GetTableValues(id, 0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Company companytemp = new Company();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        companytemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    companytemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                    companytemp.AddressLine1 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine1"]);
                    companytemp.AddressLine2 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine2"]);
                    companytemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    companytemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    companytemp.Country = Convert.ToString(dtValue.Rows[rowcount]["Country"]);
                    companytemp.PinCode = Convert.ToString(dtValue.Rows[rowcount]["PinCode"]);
                    companytemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    companytemp.EMail = Convert.ToString(dtValue.Rows[rowcount]["EMail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        companytemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        companytemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        companytemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        companytemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        companytemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        companytemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PrimaryCompanyId"])))
                        companytemp.PrimaryCompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PrimaryCompanyId"]);
                    companytemp.PFBankName = Convert.ToString(dtValue.Rows[rowcount]["PFBankName"]);
                    companytemp.PFBankAddress = Convert.ToString(dtValue.Rows[rowcount]["PFBankAddress"]);
                    companytemp.GroupCode = Convert.ToString(dtValue.Rows[rowcount]["GroupCode"]);
                    companytemp.PFEmployeerCode = Convert.ToString(dtValue.Rows[rowcount]["PFEmployeerCode"]);
                    companytemp.PensionFundAcNo = Convert.ToString(dtValue.Rows[rowcount]["PensionFundAcNo"]);
                    companytemp.EPFAcNo = Convert.ToString(dtValue.Rows[rowcount]["EPFAcNo"]);
                    companytemp.AdminChargeAcNo = Convert.ToString(dtValue.Rows[rowcount]["AdminChargeAcNo"]);
                    companytemp.InspectionChargeAcNo = Convert.ToString(dtValue.Rows[rowcount]["InspectionChargeAcNo"]);
                    companytemp.EDLIAcNo = Convert.ToString(dtValue.Rows[rowcount]["EDLIAcNo"]);
                    companytemp.ESIEmployeerContribution = Convert.ToString(dtValue.Rows[rowcount]["ESIEmployeerContribution"]);
                    companytemp.PayrollProcessBy = Convert.ToString(dtValue.Rows[rowcount]["PayrollProcessBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyLogo"])))
                        companytemp.Companylogo = Convert.ToString(dtValue.Rows[rowcount]["CompanyLogo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsTwoStep"])))
                        companytemp.IsTwoStep = Convert.ToBoolean(dtValue.Rows[0]["IsTwoStep"]);
                    this.Add(companytemp);
                }
            }
        }


        public List<Empgender> EmployeeGender(int companyid)
        {

            List<Empgender> Genderresult = new List<Empgender>();
            DataTable dtValue = GetChartGenderValues(companyid);
            foreach (DataRow drow in dtValue.Rows)
            {
                Empgender empgender = new Empgender();
                empgender.Gender = drow["Gender"].ToString();
                empgender.GenderCount = drow["count"].ToString();
                Genderresult.Add(empgender);
            }
            return Genderresult;


        }
        protected internal DataTable GetChartGenderValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_GetEmployeeGenderdetails");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", id);
            //sqlCommand.Parameters.AddWithValue("@UserId", userId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public List<ActiveEmp> ActiveEmployees(int companyid)
        {
            List<ActiveEmp> result = new List<ActiveEmp>();
            DataTable dtValue = GetChartActiveEmplValues(companyid);
            foreach(DataRow drow in dtValue.Rows)
            {
                ActiveEmp activeemp = new ActiveEmp();
                activeemp.Year = Convert.ToString(drow["Year"]);
                activeemp.Count = Convert.ToInt32(drow["count"]);
                result.Add(activeemp);
            }              
            return result;
        }

        protected internal DataTable GetChartActiveEmplValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_GetActiveEmployeedetails");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", id);
            //sqlCommand.Parameters.AddWithValue("@UserId", userId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public List<INActiveEmp> INActiveEmployees(int companyid)
        {
            List<INActiveEmp> result = new List<INActiveEmp>();
            DataTable dtValue = GetChartInActiveEmplValues(companyid);
            foreach (DataRow drow in dtValue.Rows)
            {
                INActiveEmp inactiveemp = new INActiveEmp();
                inactiveemp.Year =Convert.ToString(drow["Year"]);
                inactiveemp.Count = Convert.ToInt32(drow["count"]);
                result.Add(inactiveemp);
            }
            return result;
        }
        protected internal DataTable GetChartInActiveEmplValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("[usp_GetINActiveEmployeedetails]");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", id);
            //sqlCommand.Parameters.AddWithValue("@UserId", userId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public class ActiveEmp
        {
            public string Year;
            public int Count;
        }

        public class INActiveEmp
        {
            public string Year;
            public int Count;
        }

        public class Empgender
        {
            public string Gender;
                 public string GenderCount;
        }
      






        public void Initialize()
        {
            for (int count = 0; count < this.Count; count++)
            {
               // this[count].AttributeModelList = new AttributeModelList();
                this[count].BranchList = new BranchList();
                this[count].CategoryList = new CategoryList();
                this[count].CostCentreList = new CostCentreList();
                this[count].DepartmentList = new DepartmentList();
                this[count].DesignationList = new DesignationList();
                this[count].ESIDespensaryList = new ESIDespensaryList();
                this[count].EsiLocationList = new EsiLocationList();
                this[count].GradeList = new GradeList();
                this[count].JoiningDocumentList = new JoiningDocumentList();
            }
        }

        #endregion

        #region property

        public int UserId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the company and add to the list
        /// </summary>
        /// <param name="company"></param>
        public void AddNew(Company company)
        {
            if (company.Save())
            {
                this.Add(company);
            }
        }

        /// <summary>
        /// Delete the company and remove from the list
        /// </summary>
        /// <param name="company"></param>
        public void DeleteExist(Company company)
        {
            if (company.Delete())
            {
                this.Remove(company);
            }
        }


        #endregion

        #region private methods



        #endregion

    }
}

