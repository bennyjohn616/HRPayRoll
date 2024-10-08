using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class importTable
    {
        private List<ImportColumn> _ImportColumn;
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string MappedSheet { get; set; }

        public bool IsAmendment { get; set; }

        public bool IsNewEntries { get; set; }
        public bool AddMasterValue { get; set; }
        public int order { get; set; }
        public List<ImportColumn> ImportColumns
        {
            get
            {
                if (object.ReferenceEquals(_ImportColumn, null))
                {
                    if (!string.IsNullOrEmpty(this.Name))
                    {
                        switch (this.Name)
                        {
                            case "Employee":
                                if (object.ReferenceEquals(_ImportColumn, null))
                                    _ImportColumn = ImportColumn.GetEmployeeColumn();
                                break;
                            case "Employee Training":
                                _ImportColumn = ImportColumn.GetEmployeeTrainingColumn();
                                break;
                            case "Employee Family":
                                _ImportColumn = ImportColumn.GetEmployeeFamilyColumn();
                                break;
                            case "Employee Academic":
                                _ImportColumn = ImportColumn.GetEmployeeAcademicColumn();
                                break;
                            case "Employee Language":
                                _ImportColumn = ImportColumn.GetEmployeeLanguageColumn();
                                break;
                            case "Employee Address":
                                _ImportColumn = ImportColumn.GetEmployeeAddressColumn();
                                break;
                            case "Employee Nominee":
                                _ImportColumn = ImportColumn.GetEmployeeNomineeColumn();
                                break;
                            case "Employee Benefit Component":
                                _ImportColumn = ImportColumn.GetEmployeeBenefitComponentColumn();
                                break;
                            case "Employee Emergency Contact":
                                _ImportColumn = ImportColumn.GetEmployeeEmergencyContactColumn();
                                break;
                            case "Employee Employeement":
                                _ImportColumn = ImportColumn.GetEmployeeEmployeementColumn();
                                break;
                            case "Employee Personal":
                                _ImportColumn = ImportColumn.GetEmployeePersonalColumn();
                                break;
                            case "Employee AdditionalInfo":
                                _ImportColumn = ImportColumn.GetEmployeeAdditionalInfoColumn(this.CompanyId);
                                break;
                            case "Employee Bank Details":
                                _ImportColumn = ImportColumn.GetEmployeeBankInfoColumn();
                                break;
                            case "Category":
                                _ImportColumn = ImportColumn.GetCategoryColumn();
                                break;
                            case "Branch":
                                _ImportColumn = ImportColumn.GetBranchColumn();
                                break;
                            case "Cost Centre":
                                _ImportColumn = ImportColumn.GetCostCentreColumn();
                                break;
                            case "Department":
                                _ImportColumn = ImportColumn.GetDepartmentColumn();
                                break;
                            case "Designation":
                                _ImportColumn = ImportColumn.GetDesignationColumn();
                                break;
                            case "ESI Dispensary":
                                _ImportColumn = ImportColumn.GetESIDespensaryColumn();
                                break;
                            case "ESI Location":
                                _ImportColumn = ImportColumn.GetESILocationColumn();
                                break;
                            case "Grade":
                                _ImportColumn = ImportColumn.GetGradeColumn();
                                break;
                            case "HR Component":
                                _ImportColumn = ImportColumn.GetHRComponentColumn();
                                break;
                            case "Joining Document":
                                _ImportColumn = ImportColumn.GetJoiningDocumentColumn();
                                break;
                            case "Location":
                                _ImportColumn = ImportColumn.GetLocationColumn();
                                break;
                            case "PT Location":
                                _ImportColumn = ImportColumn.GetPTLocationColumn();
                                break;
                            case "Monthly Input":
                                _ImportColumn = ImportColumn.GetMonthlyInputColumn(this.CompanyId);
                                break;
                            case "Salary":
                                _ImportColumn = ImportColumn.GetSalaryColumn(this.CompanyId);
                                break;
                            case "Increment":
                                _ImportColumn = ImportColumn.GetIncrementColumn(this.CompanyId);
                                break;
                            case "Salary Master":
                                _ImportColumn = ImportColumn.GetSalaryMasterColumn(this.CompanyId);
                                break;
                            case "Loan Master":
                                _ImportColumn = ImportColumn.GetLoanMasterColumn(this.CompanyId);
                                break;
                            case "Loan Entry":
                                _ImportColumn = ImportColumn.GetLoanEntryColumn(this.CompanyId);
                                break;
                            case "Loan Transaction":
                                _ImportColumn = ImportColumn.GetLoanTransactionColumn(this.CompanyId);
                                break;
                            case "Employee Separation":
                                _ImportColumn = ImportColumn.GetEmployeeSeparationColumn();
                                break;
                            case "Employee Release":
                                _ImportColumn = ImportColumn.GetEmployeeReleaseColumn();
                                break;
                            case "Tax Declaration":
                                _ImportColumn = ImportColumn.GetTaxDeclarationColumn();
                                break;
                            case "Employee Code Change":
                                _ImportColumn = ImportColumn.GetEmployeeCodeChangeColumn();
                                break;
                            default:
                                _ImportColumn = new List<ImportColumn>();
                                break;
                        }
                    }
                    else _ImportColumn = new List<ImportColumn>();
                }
                return _ImportColumn;

            }
            set { _ImportColumn = value; }

        }
        public static List<importTable> GetImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            //employee related
            retObj.Add(new importTable() { Name = "Employee", order = 13, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Training", order = 14, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Family", order = 15, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Academic", order = 16, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Language", order = 17, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Address", order = 18, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Nominee", order = 20, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Benefit Component", order = 21, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Emergency Contact", order = 22, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Employeement", order = 23, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Personal", order = 24, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee AdditionalInfo", order = 25, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Bank Details", order = 26, CompanyId = companyId });
            //PAst Data
            retObj.Add(new importTable() { Name = "Monthly Input", order = 27, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Salary", order = 28, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Increment", order = 29, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Salary Master", order = 29, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Loan Master", order = 31, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Loan Entry", order = 32, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Loan Transaction", order = 33, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Separation", order = 34, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Release", order = 35, CompanyId = companyId });

            retObj.Add(new importTable() { Name = "Employee Code Change", order = 36, CompanyId = companyId });
            //need to do Address type
            //need to Template
            //need to do personal info
            //Company Display order
            //Color mappped
            //
            //company Related
            retObj.Add(new importTable() { Name = "Category", order = 1, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Branch", order = 2, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Cost Centre", order = 3, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Department", order = 4, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Designation", order = 5, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "ESI Dispensary", order = 6, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "ESI Location", order = 7, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Grade", order = 8, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "HR Component", order = 9, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Joining Document", order = 10, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Location", order = 11, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "PT Location", order = 12, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetEmployeeImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            //employee related
            retObj.Add(new importTable() { Name = "Employee", order = 13, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Training", order = 14, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Family", order = 15, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Academic", order = 16, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Language", order = 17, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Address", order = 18, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Nominee", order = 20, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Benefit Component", order = 21, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Emergency Contact", order = 22, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Employeement", order = 23, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Personal", order = 24, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee AdditionalInfo", order = 25, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Employee Bank Details", order = 26, CompanyId = companyId });
            //PAst Data
            //retObj.Add(new importTable() { Name = "Monthly Input", order = 27, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Salary", order = 28, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Increment", order = 29, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Salary Master", order = 29, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Loan Master", order = 31, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Loan Entry", order = 32, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Loan Transaction", order = 33, CompanyId = companyId });
            ////need to do Address type
            ////need to Template
            ////need to do personal info
            ////Company Display order
            ////Color mappped
            ////
            ////company Related
            //retObj.Add(new importTable() { Name = "Category", order = 1, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Branch", order = 2, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Cost Centre", order = 3, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Department", order = 4, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Designation", order = 5, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "ESI Dispensary", order = 6, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "ESI Location", order = 7, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Grade", order = 8, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "HR Component", order = 9, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Joining Document", order = 10, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "Location", order = 11, CompanyId = companyId });
            //retObj.Add(new importTable() { Name = "PT Location", order = 12, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetIncrementImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Increment", order = 29, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetMonthlyInputImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Monthly Input", order = 27, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetEmployeeSeparationImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Employee Separation", order = 27, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetEmployeeReleaseImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Employee Release", order = 27, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetPostDataImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Salary", order = 28, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetPopUpImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Category", order = 1, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Branch", order = 2, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Cost Centre", order = 3, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Department", order = 4, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Designation", order = 5, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "ESI Dispensary", order = 6, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "ESI Location", order = 7, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Grade", order = 8, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "HR Component", order = 9, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Joining Document", order = 10, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Location", order = 11, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "PT Location", order = 12, CompanyId = companyId });
            return retObj;
        }
        public static List<importTable> GetLoanImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Loan Master", order = 31, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Loan Entry", order = 32, CompanyId = companyId });
            retObj.Add(new importTable() { Name = "Loan Transaction", order = 33, CompanyId = companyId });
            return retObj;
        }

        public static List<importTable> GetEmployeeCodeChangeImportTable(int companyId)
        {
            List<importTable> retObj = new List<importTable>();
            retObj.Add(new importTable() { Name = "Employee Code Change", order = 1, CompanyId = companyId });
            return retObj;
        }
    }
}
