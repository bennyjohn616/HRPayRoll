
// -----------------------------------------------------------------------
// <copyright file="TXSectionList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To handle the TXFinanceYear
    /// </summary>
    public class TXSectionList : List<TXSection>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSectionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSectionList(int companyId)
        {
            this.CompanyId = companyId;
            TXSection txSection = new TXSection();
            DataTable dtValue = txSection.GetTableValues(Guid.Empty, companyId, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSection txSectionTemp = new TXSection();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        txSectionTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionTemp.Projection = Convert.ToString(dtValue.Rows[rowcount]["Projection"]);
                    txSectionTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    txSectionTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    txSectionTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["BaseValue"]);
                    txSectionTemp.BaseFormula = Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"]);
                    txSectionTemp.SectionType = Convert.ToString(dtValue.Rows[rowcount]["SectionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormulaType"])))
                        txSectionTemp.FormulaType = Convert.ToInt32(dtValue.Rows[rowcount]["FormulaType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncomeType"])))
                        txSectionTemp.IncomeTypeId = Convert.ToInt32(dtValue.Rows[rowcount]["IncomeType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNo"])))
                        txSectionTemp.OrderNo = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                  
                        
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ExemptionType"])))
                        txSectionTemp.ExemptionType = Convert.ToInt32(dtValue.Rows[rowcount]["ExemptionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsGrossDeductable"])))
                        txSectionTemp.IsGrossDeductable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsGrossDeductable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovelRequired"])))
                        txSectionTemp.IsApprovelRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovelRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Eligible"])))
                        txSectionTemp.Eligible = Convert.ToBoolean(dtValue.Rows[rowcount]["Eligible"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TXSectionList(int companyId, Guid financeyearId)
        {
            this.CompanyId = companyId;
            TXSection txSection = new TXSection();
            DataTable dtValue = txSection.GetTableValues(Guid.Empty, companyId, Guid.Empty, financeyearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSection txSectionTemp = new TXSection();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        txSectionTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionTemp.Projection = Convert.ToString(dtValue.Rows[rowcount]["Projection"]);
                    txSectionTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    txSectionTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    txSectionTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["BaseValue"]);
                    txSectionTemp.BaseFormula = Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"]);
                    txSectionTemp.SectionType = Convert.ToString(dtValue.Rows[rowcount]["SectionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNo"])))
                        txSectionTemp.OrderNo = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormulaType"])))
                        txSectionTemp.FormulaType = Convert.ToInt32(dtValue.Rows[rowcount]["FormulaType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncomeType"])))
                        txSectionTemp.IncomeTypeId = Convert.ToInt32(dtValue.Rows[rowcount]["IncomeType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ExemptionType"])))
                        txSectionTemp.ExemptionType = Convert.ToInt32(dtValue.Rows[rowcount]["ExemptionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsGrossDeductable"])))
                        txSectionTemp.IsGrossDeductable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsGrossDeductable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovelRequired"])))
                        txSectionTemp.IsApprovelRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovelRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MatchingComponent"])))
                        txSectionTemp.MatchingComponent = Convert.ToString(dtValue.Rows[rowcount]["MatchingComponent"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Eligible"])))
                        txSectionTemp.Eligible = Convert.ToBoolean(dtValue.Rows[rowcount]["Eligible"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TXSectionList(int companyId, Guid financeyearId, Guid parentId)
        {
            this.CompanyId = companyId;
            TXSection txSection = new TXSection();
            DataTable dtValue = txSection.GetTableValues(Guid.Empty, companyId, parentId, financeyearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSection txSectionTemp = new TXSection();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        txSectionTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionTemp.Projection = Convert.ToString(dtValue.Rows[rowcount]["Projection"]);
                    txSectionTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    txSectionTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    txSectionTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["BaseValue"]);
                    txSectionTemp.BaseFormula = Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormulaType"])))
                        txSectionTemp.FormulaType = Convert.ToInt32(dtValue.Rows[rowcount]["FormulaType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncomeType"])))
                        txSectionTemp.IncomeTypeId = Convert.ToInt32(dtValue.Rows[rowcount]["IncomeType"]);
                    txSectionTemp.SectionType = Convert.ToString(dtValue.Rows[rowcount]["SectionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNo"])))
                        txSectionTemp.OrderNo = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ExemptionType"])))
                        txSectionTemp.ExemptionType = Convert.ToInt32(dtValue.Rows[rowcount]["ExemptionType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsGrossDeductable"])))
                        txSectionTemp.IsGrossDeductable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsGrossDeductable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovelRequired"])))
                        txSectionTemp.IsApprovelRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovelRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MatchingComponent"])))
                        txSectionTemp.MatchingComponent = Convert.ToString(dtValue.Rows[rowcount]["MatchingComponent"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Eligible"])))
                        txSectionTemp.Eligible = Convert.ToBoolean(dtValue.Rows[rowcount]["Eligible"]);
                    this.Add(txSectionTemp);
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

        public Guid Parentid { get; set; }
        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax section and add to the list
        /// </summary>
        /// <param name="txSection"></param>
        public void AddNew(TXSection txSection)
        {
            if (txSection.Save())
            {
                this.Add(txSection);
            }
        }

        /// <summary>
        /// delete the tax section data
        /// </summary>
        /// <param name="txSection"></param>

        public void DeleteExist(TXSection txSection)
        {
            if (txSection.Delete())
            {
                this.Remove(txSection);
            }
        }


        #endregion

    }
}
