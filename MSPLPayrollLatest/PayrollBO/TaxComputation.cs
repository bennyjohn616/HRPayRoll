using PayrollBO.Tax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxComputationInfo
    {
        #region "Constructor"
        public TaxComputationInfo()
        {
            this.Result = new List<TaxHistory>();
            this.Employees = new EmployeeList();
            this.SubSections = new TXSectionList();
            this.Sections = new TXSectionList();
            this.Errors = new List<string>();
            this.AttributemodelList = new AttributeModelList();
        }
        #endregion
        public int CompanyId { get; set; }
        public Guid FinanceYearId { get; set; }
        public int ApplyMonth { get; set; }
        public int ApplyYear { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int UserId { get; set; }
        public int ProjectionMonth { get; set; }
        public int Balprojmonth { get; set; }
        public int Balprojdays { get; set; }
        public int Balmaxdays { get; set; }
        public EmployeeList Employees { get; set; }
        public TXFinanceYear FinanceYear { get; set; }
        public List<TaxHistory> Result { get; set; }
        public TaxBehaviorList TaxBehaviorList { get; set; }
        public List<string> Errors { get; set; }
        public TXSectionList Sections { get; set; }
        public TXSectionList SubSections { get; set; }
        public TXSectionList OtherIncomeHeads { get; set; }
        public bool Proofwise { get; set; }

        public AttributeModelList AttributemodelList { get; set; }
        public bool VPFReq { get; set; }
        public bool VPFProjection { get; set; }

        public bool FandFFlag { get; set; }

        public Guid EntityId { get; set; }

        public Guid EntityModelId { get; set; }

        public TXEmployeeSectionList TxEmployeeSectionList { get; set; }

        public  IncomeMatchingList incmatchList { get; set; }

        public List<StructIncMatchList> StructList { get; set; }

        public PayrollHistoryList payrollhistorylist { get; set; }

        public TXFinanceYearList finyearlist { get; set; }

        public DataTable Monthly_input { get; set; }

        public DataTable TaxSave { get; set; }

        public DataTable dt2 { get; set; }

        public DataTable dt1 { get; set; }

        public IncrementList increment { get; set; }

        public TXSectionList allsection { get; set; }

        public string emproll {get;set;}

        public int age { get; set; }

        public Entity entity { get; set; }

        public TXProjIncome TXProjIncome { get; set; }

        public string processtype { get; set; }

        public void MoveToClass(List<StructIncMatchList> structIncMatchLists, IncomeMatchingList IncList)
        {
            for (int i = 0; i < structIncMatchLists.Count; i++)
            {
                IncomeMatching im1 = new IncomeMatching();
                im1.AttributemodelId = structIncMatchLists[i].AttributemodelId;
                im1.CreatedBy = structIncMatchLists[i].CreatedBy;
                im1.CreatedOn = structIncMatchLists[i].CreatedOn;
                im1.ExemptionComponent = structIncMatchLists[i].ExemptionComponent;
                im1.FinancialYearId = structIncMatchLists[i].FinancialYearId;
                im1.Formula = structIncMatchLists[i].Formula;
                im1.GrossSection = structIncMatchLists[i].GrossSection;
                im1.Id = structIncMatchLists[i].Id;
                im1.IsDeleted = structIncMatchLists[i].IsDeleted;
                im1.MatchingComponent = structIncMatchLists[i].MatchingComponent;
                im1.ModifiedBy = structIncMatchLists[i].ModifiedBy;
                im1.ModifiedOn = structIncMatchLists[i].ModifiedOn;
                im1.Operator = structIncMatchLists[i].Operator;
                im1.OrderNo = structIncMatchLists[i].OrderNo;
                im1.OtherComponent = structIncMatchLists[i].OtherComponent;
                im1.Projection = structIncMatchLists[i].Projection;
                im1.TaxDeductionMode = structIncMatchLists[i].TaxDeductionMode;
                IncList.Add(im1);
            }

        }

    }

}
