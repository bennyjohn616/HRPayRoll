using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace PayrollBO.Tax
{
    public class TaxImportColumns
    {
        public Guid OtherTableUniqueId { get; set; }
        public string Name { get; set; }

        public string MappedColumnName { get; set; }

        public string MinVal { get; set; }

        public string MaxLength { get; set; }

        public bool IsRequired { get; set; }
        public static List<TaxImportColumns> GetTxSectionColumns()
        {
            List<TaxImportColumns> retobj = new List<TaxImportColumns>();
            string companyid = HttpContext.Current.Session["CompanyId"].ToString();
            DataTable dt = GetTaxSectionColumns(Convert.ToInt32(companyid));
            retobj.Add(new TaxImportColumns() { Name = "Employee Code", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Category", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Year", IsRequired = true, MaxLength = "40", MinVal = "" });

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    retobj.Add(new TaxImportColumns() { Name = dt.Rows[i]["DisplayAs"].ToString(), IsRequired = true, MaxLength = "40", MinVal = "" });
                }
            }
            return retobj;
        }

        public static List<TaxImportColumns> GetHRAHousePropertyIncomeColumns()
        {
            List<TaxImportColumns> retobj = new List<TaxImportColumns>();
            retobj.Add(new TaxImportColumns() { Name = "Employee Code", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Category", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Year", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyId", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyReference", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Property_OwnersName", IsRequired = true, MaxLength = "200", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyAddress", IsRequired = true, MaxLength = "200", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyLoanAmount", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PurposeOfLoan", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "DateofSanctionofLoan", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyJointName", IsRequired = true, MaxLength = "200", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertyJointInterest", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PayableHousingLoanPerYear", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PreConstructionInterest", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "TotalInterestOfYear", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Interest_RestrictedtoEmployee", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "ConstrutionIsCompleted", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PropertySelfOccupied", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "HousingLoanTakenBefore_01_04_1999", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "GrossRentalIncome_PA", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Municipal_Water_Sewerage_taxpaid", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "GrossRentalIncome", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LessMunicipalTaxes", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Balance", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LessStandardDeduction", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LessInterestOnHousingLoan", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "HousePropertyNetIncome", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "IsHRACompleted", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LenderAddress", IsRequired = true, MaxLength = "200", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LenderName", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LenderPAN", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LenderType", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "LenderHRAAddress", IsRequired = true, MaxLength = "200", MinVal = "" });
            return retobj;
        }

        public static List<TaxImportColumns> GetLICpremiumColumns()
        {
            List<TaxImportColumns> retobj = new List<TaxImportColumns>();
            retobj.Add(new TaxImportColumns() { Name = "Employee Code", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Category", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Year", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PolicyNumber", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PolicyDate", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "InsuredPersonName", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Relationship", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "IsDisabilityPerson", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "IsPersonTakingTreatement", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Sumassured", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PremiumAmount", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PremiumAmountFallingDueInFeb", IsRequired = true, MaxLength = "40", MinVal = "" });
            return retobj;
        }

        public static List<TaxImportColumns> GetMedicalInsuranceColumns()
        {
            List<TaxImportColumns> retobj = new List<TaxImportColumns>();
            retobj.Add(new TaxImportColumns() { Name = "Employee Code", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Category", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Year", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "InsuranceType", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PolicyNo", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "DateofCommencofpolicy", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "InsuredPersonName", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "RelationshipoftheInsuredperson", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "CoveredinthepolicyisSeniorCitizen", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "IncurredinrespectofVerySeniorCitizen", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "AmountofpremiumorExpense", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "PayMode", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "EligibleDeductionforthepolicy", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "SelfSpouseChildOveralldeduction", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "ParentOveralldeduction", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "TotalDeduction", IsRequired = true, MaxLength = "40", MinVal = "" });
            return retobj;
        }

        public static List<TaxImportColumns> GetActualRentPaidColumns()
        {
            List<TaxImportColumns> retobj = new List<TaxImportColumns>();
            retobj.Add(new TaxImportColumns() { Name = "Employee Code", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Category", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Effective Year", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "Rent Month", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "MetroRent", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "NonMetroRent", IsRequired = true, MaxLength = "40", MinVal = "" });
            retobj.Add(new TaxImportColumns() { Name = "TXEmployeeSectionId", IsRequired = true, MaxLength = "40", MinVal = "" });
            return retobj;
        }
        public static DataTable GetTaxSectionColumns(int Companyid)
        {
            SqlCommand sqlCommand = new SqlCommand("USP_GETTAXCOLUMNNAMES");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", Companyid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable SaveImportExcel(DataTable dt)
        {
            SqlCommand sqlCommand = new SqlCommand("USP_SaveDeclarationExportExcel");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@TXSectionType", dt);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable SaveActualRentImportExcel(DataTable dt, int Companyid,string EffMonth, string EffYear)
        {
            SqlCommand sqlCommand = new SqlCommand("USP_SaveActualRenyPaidExportExcel");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ActualRentPaidType", dt);
            sqlCommand.Parameters.AddWithValue("@CompanyId", Companyid);
            sqlCommand.Parameters.AddWithValue("@EffMonth", EffMonth);
            sqlCommand.Parameters.AddWithValue("@EffYear", EffYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetExportSampleExcel(int Companyid)
        {
            SqlCommand sqlCommand = new SqlCommand("USP_GetSampleExcelColumns");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", Companyid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }

}
