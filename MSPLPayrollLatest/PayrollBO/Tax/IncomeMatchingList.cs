using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class IncomeMatchingList : List<IncomeMatching>
    {
        public IncomeMatchingList()
        {

        }
        public IncomeMatchingList(Guid financeYearId)
        {
            IncomeMatching imatch = new IncomeMatching();
            imatch.FinancialYearId = financeYearId;
            DataTable dtValue = imatch.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    IncomeMatching im = new IncomeMatching();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        im.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttributemodelId"])))
                        im.AttributemodelId = new Guid(Convert.ToString(dtValue.Rows[i]["AttributemodelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYearId"])))
                        im.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["MatchingComponent"])))
                        im.MatchingComponent = new Guid(Convert.ToString(dtValue.Rows[i]["MatchingComponent"]));
                    im.OtherComponent = Convert.ToString(dtValue.Rows[i]["OtherComponent"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ExemptionComponent"])))
                        im.ExemptionComponent = new Guid(Convert.ToString(dtValue.Rows[i]["ExemptionComponent"]));
                    im.Formula = Convert.ToString(dtValue.Rows[i]["Formula"]);
                    im.Projection = Convert.ToBoolean(dtValue.Rows[i]["Projection"]);
                    im.Operator = Convert.ToString(dtValue.Rows[i]["Operator"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["OrderNo"])))
                        im.OrderNo = Convert.ToInt32(dtValue.Rows[i]["OrderNo"]);
                    im.TaxDeductionMode = Convert.ToString(dtValue.Rows[i]["TaxDeductionMode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        im.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        im.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        im.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        im.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        im.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    im.GrossSection = Convert.ToInt32(dtValue.Rows[i]["GrossSection"]);
                    this.Add(im);
                }
            }
        }
    }

    public struct StructIncMatchList
    {
        public Guid Id { get; set; }
        public Guid AttributemodelId { get; set; }
        public Guid FinancialYearId { get; set; }
        public Guid MatchingComponent { get; set; }
        public string OtherComponent { get; set; }
        public Guid ExemptionComponent { get; set; }
        public String Formula { get; set; }
        public bool Projection { get; set; }
        public string Operator { get; set; }
        public int OrderNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int GrossSection { get; set; }
        public string TaxDeductionMode { get; set; }

        public void Init()
        {
            this.AttributemodelId = Guid.Empty;
            this.CreatedBy = 0;
            this.CreatedOn = Convert.ToDateTime(null);
            this.ExemptionComponent = Guid.Empty;
            this.FinancialYearId = Guid.Empty;
            this.Formula = string.Empty;
            this.GrossSection = 0;
            this.Id = Guid.Empty;
            this.IsDeleted = false;
            this.MatchingComponent = Guid.Empty;
            this.ModifiedBy = 0;
            this.ModifiedOn = Convert.ToDateTime(null);
            this.Operator = string.Empty;
            this.OrderNo = 0;
            this.OtherComponent = string.Empty;
            this.Projection = false;
            this.TaxDeductionMode = string.Empty;
        }

    }
}
