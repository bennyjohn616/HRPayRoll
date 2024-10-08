using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LifeInsuranceList : List<LifeInsurance>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public LifeInsuranceList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LifeInsuranceList(int companyId)
        {
            this.CompanyId = companyId;
            LifeInsurance lifeInsurance = new LifeInsurance();
            DataTable dtValue = lifeInsurance.GetTableValues(companyId, Guid.Empty, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LifeInsurance lifeInsuranceTemp = new LifeInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lifeInsuranceTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lifeInsuranceTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        lifeInsuranceTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        lifeInsuranceTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    lifeInsuranceTemp.PolicyNumber = Convert.ToString(dtValue.Rows[rowcount]["PolicyNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PolicyDate"])))
                        lifeInsuranceTemp.PolicyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PolicyDate"]);
                    lifeInsuranceTemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDisabilityPerson"])))
                        lifeInsuranceTemp.IsDisabilityPerson = Convert.ToInt16(dtValue.Rows[rowcount]["IsDisabilityPerson"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPersonTakingTreatement"])))
                        lifeInsuranceTemp.IsPersonTakingTreatement = Convert.ToInt16(dtValue.Rows[rowcount]["IsPersonTakingTreatement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmount"])))
                        lifeInsuranceTemp.PremiumAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"])))
                        lifeInsuranceTemp.PremiumAmountFallingDueInFeb = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"])))
                        lifeInsuranceTemp.PremiumConsideredForDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        lifeInsuranceTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        lifeInsuranceTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        lifeInsuranceTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        lifeInsuranceTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        lifeInsuranceTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(lifeInsuranceTemp);
                }

            }
        }

        public LifeInsuranceList(int companyId, Guid financeyearId)
        {
            this.CompanyId = companyId;
            LifeInsurance lifeInsurance = new LifeInsurance();
            DataTable dtValue = lifeInsurance.GetTableValues(companyId, Guid.Empty, Guid.Empty,financeyearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LifeInsurance lifeInsuranceTemp = new LifeInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lifeInsuranceTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lifeInsuranceTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        lifeInsuranceTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        lifeInsuranceTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    lifeInsuranceTemp.PolicyNumber = Convert.ToString(dtValue.Rows[rowcount]["PolicyNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PolicyDate"])))
                        lifeInsuranceTemp.PolicyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PolicyDate"]);
                    lifeInsuranceTemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDisabilityPerson"])))
                        lifeInsuranceTemp.IsDisabilityPerson = Convert.ToInt16(dtValue.Rows[rowcount]["IsDisabilityPerson"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPersonTakingTreatement"])))
                        lifeInsuranceTemp.IsPersonTakingTreatement = Convert.ToInt16(dtValue.Rows[rowcount]["IsPersonTakingTreatement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmount"])))
                        lifeInsuranceTemp.PremiumAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        lifeInsuranceTemp.Month = Convert.ToInt16(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        lifeInsuranceTemp.Year = Convert.ToInt16(dtValue.Rows[rowcount]["Year"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"])))
                    //    lifeInsuranceTemp.PremiumAmountFallingDueInFeb = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"])))
                    //    lifeInsuranceTemp.PremiumConsideredForDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                    //    lifeInsuranceTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                    //    lifeInsuranceTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                    //    lifeInsuranceTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                    //    lifeInsuranceTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                    //    lifeInsuranceTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(lifeInsuranceTemp);
                }

            }
        }

        public LifeInsuranceList(int companyId, Guid financeyearId, Guid employeeId)
        {
            this.CompanyId = companyId;
            LifeInsurance lifeInsurance = new LifeInsurance();
            DataTable dtValue = lifeInsurance.GetTableValues(companyId, Guid.Empty, employeeId, financeyearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LifeInsurance lifeInsuranceTemp = new LifeInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lifeInsuranceTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lifeInsuranceTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        lifeInsuranceTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        lifeInsuranceTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    lifeInsuranceTemp.PolicyNumber = Convert.ToString(dtValue.Rows[rowcount]["PolicyNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PolicyDate"])))
                        lifeInsuranceTemp.PolicyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PolicyDate"]);
                    lifeInsuranceTemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDisabilityPerson"])))
                        lifeInsuranceTemp.IsDisabilityPerson = Convert.ToInt16(dtValue.Rows[rowcount]["IsDisabilityPerson"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPersonTakingTreatement"])))
                        lifeInsuranceTemp.IsPersonTakingTreatement = Convert.ToInt16(dtValue.Rows[rowcount]["IsPersonTakingTreatement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmount"])))
                        lifeInsuranceTemp.PremiumAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmount"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"])))
                    //    lifeInsuranceTemp.PremiumAmountFallingDueInFeb = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"])))
                    //    lifeInsuranceTemp.PremiumConsideredForDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                    //    lifeInsuranceTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                    //    lifeInsuranceTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                    //    lifeInsuranceTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                    //    lifeInsuranceTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                    //    lifeInsuranceTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(lifeInsuranceTemp);
                }

            }
        }


        public LifeInsuranceList(int companyId, Guid financeyearId, Guid employeeId,int month,int year)
        {
            this.CompanyId = companyId;
            LifeInsurance lifeInsurance = new LifeInsurance();
            lifeInsurance.Month = month;
            lifeInsurance.Year = year;
            DataTable dtValue = lifeInsurance.GetTableValues(companyId, Guid.Empty, employeeId, financeyearId);
            if (dtValue.Rows.Count > 0)
            {              
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LifeInsurance lifeInsuranceTemp = new LifeInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lifeInsuranceTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lifeInsuranceTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        lifeInsuranceTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        lifeInsuranceTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    lifeInsuranceTemp.PolicyNumber = Convert.ToString(dtValue.Rows[rowcount]["PolicyNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PolicyDate"])))
                        lifeInsuranceTemp.PolicyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PolicyDate"]);
                    lifeInsuranceTemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDisabilityPerson"])))
                        lifeInsuranceTemp.IsDisabilityPerson = Convert.ToInt16(dtValue.Rows[rowcount]["IsDisabilityPerson"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPersonTakingTreatement"])))
                        lifeInsuranceTemp.IsPersonTakingTreatement = Convert.ToInt16(dtValue.Rows[rowcount]["IsPersonTakingTreatement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmount"])))
                        lifeInsuranceTemp.PremiumAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AnnualPremium"])))
                        lifeInsuranceTemp.annualPremium = Convert.ToDecimal(dtValue.Rows[rowcount]["AnnualPremium"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"])))
                        lifeInsuranceTemp.PremiumAmountFallingDueInFeb = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmountFallingDueInFeb"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Sumassured"])))
                        lifeInsuranceTemp.SumAssured = Convert.ToDecimal(dtValue.Rows[rowcount]["Sumassured"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Relationship"])))
                        lifeInsuranceTemp.Relationship = Convert.ToString(dtValue.Rows[rowcount]["Relationship"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"])))
                    //    lifeInsuranceTemp.PremiumConsideredForDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumConsideredForDeduction"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                    //    lifeInsuranceTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                    //    lifeInsuranceTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                    //    lifeInsuranceTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                    //    lifeInsuranceTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                    //    lifeInsuranceTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(lifeInsuranceTemp);
                }

            }
        }


        public LifeInsuranceList(Guid financeyearId, Guid employeeId,Guid TXLICid)
        {
            LifeInsurance lifeInsurance = new LifeInsurance();
            DataTable dtValue = lifeInsurance.GetTableValuesLICPremium(Guid.Empty,employeeId, financeyearId,TXLICid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LifeInsurance lifeInsuranceTemp = new LifeInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumId"])))
                        lifeInsuranceTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PremiumId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxLICId"])))
                        lifeInsuranceTemp.LifeInsuranceId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TaxLICId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lifeInsuranceTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        lifeInsuranceTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    lifeInsuranceTemp.PolicyNumber = Convert.ToString(dtValue.Rows[rowcount]["PolicyNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PaidDate"])))
                        lifeInsuranceTemp.PremiumDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PaidDate"]);                 
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PremiumAmount"])))
                        lifeInsuranceTemp.PremiumAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PremiumAmount"]);
                    this.Add(lifeInsuranceTemp);
                }

            }
        }
        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public Guid FinancialYearId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the life Insurance and add to the list
        /// </summary>
        /// <param name="lifeInsurance"></param>
        public void AddNew(LifeInsurance lifeInsurance)
        {
            if (lifeInsurance.Save())
            {
                this.Add(lifeInsurance);
            }
        }

        /// <summary>
        /// delete the life Insurance data
        /// </summary>
        /// <param name="lifeInsurance"></param>

        public void DeleteExist(LifeInsurance lifeInsurance)
        {
            if (lifeInsurance.Delete())
            {
                this.Remove(lifeInsurance);
            }
        }


        #endregion
    }
}
