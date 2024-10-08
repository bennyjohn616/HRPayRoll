using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class MedicalInsuranceList : List<MedicalInsurance>
    {
        public MedicalInsuranceList()
        {

        }

        public MedicalInsuranceList(Guid EmployeeId, int companyId,  int EffectiveMonth, int EffectiveYear , string Fieldname)
        {
            
            MedicalInsurance medicalinsurance = new MedicalInsurance();
            DataTable dtValue = medicalinsurance.GetTableValues(EmployeeId, companyId,EffectiveMonth, EffectiveYear, Fieldname);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MedicalInsurance medicalinsurancetemp = new MedicalInsurance();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        medicalinsurancetemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    //    medicalinsurancetemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TxSectionId"])))
                    //    medicalinsurancetemp.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[0]["TxSectionId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinYearId"])))
                    //    medicalinsurancetemp.FinYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinYearId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveMonth"])))
                    //    medicalinsurancetemp.EffectiveMonth = Convert.ToInt32(dtValue.Rows[0]["EffectiveMonth"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveYear"])))
                    //    medicalinsurancetemp.EffectiveYear = Convert.ToInt32(dtValue.Rows[0]["EffectiveYear"]);
                    medicalinsurancetemp.InsuranceType = Convert.ToString(dtValue.Rows[rowcount]["InsuranceType"]);
                    medicalinsurancetemp.PolicyNo = Convert.ToString(dtValue.Rows[rowcount]["PolicyNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofCommencofpolicy"])))
                        medicalinsurancetemp.DateofCommencofpolicy = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofCommencofpolicy"]);
                    medicalinsurancetemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    medicalinsurancetemp.RelationshipoftheInsuredperson = Convert.ToString(dtValue.Rows[rowcount]["RelationshipoftheInsuredperson"]);
                    medicalinsurancetemp.CoveredinthepolicyisSeniorCitizen = Convert.ToString(dtValue.Rows[rowcount]["CoveredinthepolicyisSeniorCitizen"]);
                    medicalinsurancetemp.IncurredinrespectofVerySeniorCitizen = Convert.ToString(dtValue.Rows[rowcount]["IncurredinrespectofVerySeniorCitizen"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AmountofpremiumorExpense"])))
                        medicalinsurancetemp.AmountofpremiumorExpense = Convert.ToDecimal(dtValue.Rows[rowcount]["AmountofpremiumorExpense"]);
                    medicalinsurancetemp.PayMode = Convert.ToString(dtValue.Rows[rowcount]["PayMode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EligibleDeductionforthepolicy"])))
                        medicalinsurancetemp.EligibleDeductionforthepolicy = Convert.ToDecimal(dtValue.Rows[rowcount]["EligibleDeductionforthepolicy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SelfSpouseChildOveralldeduction"])))
                        medicalinsurancetemp.SelfSpouseChildOveralldeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["SelfSpouseChildOveralldeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentOveralldeduction"])))
                        medicalinsurancetemp.ParentOveralldeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["ParentOveralldeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalDeduction"])))
                        medicalinsurancetemp.TotalDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalDeduction"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    //    medicalinsurancetemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    //    medicalinsurancetemp.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    //    medicalinsurancetemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    //    medicalinsurancetemp.ModiifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    //    medicalinsurancetemp.IsDelete = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    //    medicalinsurancetemp.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    //    medicalinsurancetemp.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);

                    medicalinsurancetemp.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                    medicalinsurancetemp.Employeename = Convert.ToString(dtValue.Rows[0]["Employeename"]);
                    medicalinsurancetemp.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                    medicalinsurancetemp.PANNumber = Convert.ToString(dtValue.Rows[0]["PANNumber"]);
                    medicalinsurancetemp.Financial_Year = Convert.ToString(dtValue.Rows[0]["Financial_Year"]);
                    this.Add(medicalinsurancetemp);
                }
            }
        }
        public MedicalInsuranceList(Guid EmployeeId, int companyId, int EffectiveMonth, int EffectiveYear, Guid financialYear)
        {

            MedicalInsurance medicalinsurance = new MedicalInsurance();
            DataTable dtValue = medicalinsurance.GetTableValues(EmployeeId, companyId, EffectiveMonth, EffectiveYear, financialYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MedicalInsurance medicalinsurancetemp = new MedicalInsurance();
                    
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AmountofpremiumorExpense"])))
                        medicalinsurancetemp.TotalAmount = Convert.ToDecimal(dtValue.Rows[0]["AmountofpremiumorExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EligibleDeductionforthepolicy"])))
                        medicalinsurancetemp.TotalEligibleDeduction = Convert.ToDecimal(dtValue.Rows[0]["EligibleDeductionforthepolicy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SelfValue"])))
                        medicalinsurancetemp.SelfSpouseChildOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["SelfValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentValue"])))
                        medicalinsurancetemp.ParentOveralldeduction = Convert.ToDecimal(dtValue.Rows[0]["ParentValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Total"])))
                        medicalinsurancetemp.TotalDeduction = Convert.ToDecimal(dtValue.Rows[0]["Total"]);

                    this.Add(medicalinsurancetemp);
                }
            }
        }

        public MedicalInsuranceList(Guid financialyearId, int CompanyId, string EffectiveMonth, string EffectiveYear)
        {

            MedicalInsurance medicalinsurance = new MedicalInsurance();
            DataTable dtValue = medicalinsurance.GetTableValues(financialyearId, CompanyId, EffectiveMonth,EffectiveYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MedicalInsurance medicalinsurancetemp = new MedicalInsurance();

                    medicalinsurancetemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    medicalinsurancetemp.Employeename = Convert.ToString(dtValue.Rows[rowcount]["Employeename"]);
                    medicalinsurancetemp.InsuranceType = Convert.ToString(dtValue.Rows[rowcount]["InsuranceType"]);
                    medicalinsurancetemp.PolicyNo = Convert.ToString(dtValue.Rows[rowcount]["PolicyNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofCommencofpolicy"])))
                        medicalinsurancetemp.DateofCommencofpolicy = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofCommencofpolicy"]);
                    medicalinsurancetemp.InsuredPersonName = Convert.ToString(dtValue.Rows[rowcount]["InsuredPersonName"]);
                    medicalinsurancetemp.RelationshipoftheInsuredperson = Convert.ToString(dtValue.Rows[rowcount]["RelationshipoftheInsuredperson"]);
                    medicalinsurancetemp.CoveredinthepolicyisSeniorCitizen = Convert.ToString(dtValue.Rows[rowcount]["CoveredinthepolicyisSeniorCitizen"]);
                    medicalinsurancetemp.IncurredinrespectofVerySeniorCitizen = Convert.ToString(dtValue.Rows[rowcount]["IncurredinrespectofVerySeniorCitizen"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AmountofpremiumorExpense"])))
                        medicalinsurancetemp.AmountofpremiumorExpense = Convert.ToDecimal(dtValue.Rows[rowcount]["AmountofpremiumorExpense"]);
                    medicalinsurancetemp.PayMode = Convert.ToString(dtValue.Rows[rowcount]["PayMode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EligibleDeductionforthepolicy"])))
                        medicalinsurancetemp.EligibleDeductionforthepolicy = Convert.ToDecimal(dtValue.Rows[rowcount]["EligibleDeductionforthepolicy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SelfSpouseChildOveralldeduction"])))
                        medicalinsurancetemp.SelfSpouseChildOveralldeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["SelfSpouseChildOveralldeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentOveralldeduction"])))
                        medicalinsurancetemp.ParentOveralldeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["ParentOveralldeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalDeduction"])))
                        medicalinsurancetemp.TotalDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalDeduction"]);

                    string InsureType = medicalinsurancetemp.InsuranceType == "1" ? "Medical Insurance" : medicalinsurancetemp.InsuranceType == "2" ? "CGHS contribution" : medicalinsurancetemp.InsuranceType == "3" ? "Preventive Health Check up" : "Medical Expenditure";
                    string RelationshipInsuredperson = medicalinsurancetemp.RelationshipoftheInsuredperson == "1" ? "Self Spouse & Child" : "Parent(s)";
                    string CoveredpolicyisSeniorCitizen = medicalinsurancetemp.CoveredinthepolicyisSeniorCitizen == "1" ? "Senior Citizen" : "Normal";
                    string IncurredrespectofVerySeniorCitizen = medicalinsurancetemp.IncurredinrespectofVerySeniorCitizen == "1" ? "Yes" : medicalinsurancetemp.IncurredinrespectofVerySeniorCitizen == "2" ? "NO" : "N/A";
                    string pay = medicalinsurancetemp.PayMode == "1" ? "Cash" : "Other than Cash";

                    medicalinsurancetemp.InsuranceType = InsureType;
                    medicalinsurancetemp.RelationshipoftheInsuredperson = RelationshipInsuredperson;
                    medicalinsurancetemp.CoveredinthepolicyisSeniorCitizen = CoveredpolicyisSeniorCitizen;
                    medicalinsurancetemp.IncurredinrespectofVerySeniorCitizen = IncurredrespectofVerySeniorCitizen;
                    medicalinsurancetemp.PayMode = pay;
                    this.Add(medicalinsurancetemp);
                }
            }
        }

        public MedicalInsuranceList(Guid EmployeeId, Guid TxSectionId, Guid FinancialYear, int EffectiveMonth, int EffectiveYear, decimal TotalDeduction)
        {

            MedicalInsurance medicalinsurance = new MedicalInsurance();
            DataTable dtValue = medicalinsurance.GetTableValues(EmployeeId, TxSectionId, FinancialYear, EffectiveMonth, EffectiveYear, TotalDeduction);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MedicalInsurance medicalinsurancetemp = new MedicalInsurance();
                    
                    this.Add(medicalinsurancetemp);
                }
            }
        }
    }
}
