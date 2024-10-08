using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO.Tax
{
    public class TaxImportTables
    {
        private List<TaxImportColumns> _ImportColumn;
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string MappedSheet { get; set; }

        public bool IsAmendment { get; set; }

        public bool IsNewEntries { get; set; }
        public bool AddMasterValue { get; set; }
        public int order { get; set; }
        public List<TaxImportColumns> ImportColumns
        {
            get
            {
                if (object.ReferenceEquals(_ImportColumn, null))
                {
                    if (!string.IsNullOrEmpty(this.Name))
                    {
                        switch (this.Name)
                        {
                            case "Declaration Entry":
                                if (object.ReferenceEquals(_ImportColumn, null))
                                    _ImportColumn = TaxImportColumns.GetTxSectionColumns();
                                break;
                            case "House Rent Allowance":
                                _ImportColumn = TaxImportColumns.GetHRAHousePropertyIncomeColumns();
                                break;
                            case "LIC premium paid":
                                _ImportColumn = TaxImportColumns.GetLICpremiumColumns();
                                break;
                            case "Medical insurance premium":
                                _ImportColumn = TaxImportColumns.GetMedicalInsuranceColumns();
                                break;
                            case "Actual Rent Paid":
                                _ImportColumn = TaxImportColumns.GetActualRentPaidColumns();
                                break;
                            default:
                                _ImportColumn = new List<TaxImportColumns>();
                                break;
                        }
                    }
                    else _ImportColumn = new List<TaxImportColumns>();
                }
                return _ImportColumn;

            }
            set { _ImportColumn = value; }

        }
        public static List<TaxImportTables> GetImportTable(int companyId)
        {
            List<TaxImportTables> retObj = new List<TaxImportTables>();
            retObj.Add(new TaxImportTables() { Name = "Declaration Entry", order = 13, CompanyId = companyId });
            retObj.Add(new TaxImportTables() { Name = "House Rent Allowance", order = 14, CompanyId = companyId });
            retObj.Add(new TaxImportTables() { Name = "LIC premium paid", order = 15, CompanyId = companyId });
            retObj.Add(new TaxImportTables() { Name = "Medical insurance premium", order = 16, CompanyId = companyId });
            retObj.Add(new TaxImportTables() { Name = "Actual Rent Paid", order = 17, CompanyId = companyId });
            return retObj;
        }
    }
}
