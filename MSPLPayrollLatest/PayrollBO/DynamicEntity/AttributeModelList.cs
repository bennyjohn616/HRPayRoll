// -----------------------------------------------------------------------
// <copyright file="AttributeModelList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using PayrollBO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttributeModelList : List<AttributeModel>
    {


        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModelList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public AttributeModelList(int companyId, Guid attributeModelTypeId)
        {
            this.CompanyId = companyId;
            AttributeModel attributeModel = new AttributeModel();
            DataTable dtValue = attributeModel.GetTableValues(this.CompanyId, Guid.Empty, attributeModelTypeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModel attributeModelTemp = new AttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                    {
                        attributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                        //TXFinanceYearList financialyrlist = new TXFinanceYearList(companyId);
                        //var financialyr = financialyrlist.Where(x => x.IsActive == true).FirstOrDefault();
                        //IncomeMatching tx = new PayrollBO.IncomeMatching(financialyr.Id, attributeModelTemp.Id);
                        //attributeModelTemp.TaxDeductionMode = tx.TaxDeductionMode;
                    }
                      
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        attributeModelTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    attributeModelTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    attributeModelTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        attributeModelTemp.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]));
                    attributeModelTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DataSize"])))
                        attributeModelTemp.DataSize = Convert.ToInt32(dtValue.Rows[rowcount]["DataSize"]);
                    attributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMandatory"])))
                        attributeModelTemp.IsMandatory = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMandatory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNumber"])))
                        attributeModelTemp.OrderNumber = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTransaction"])))
                        attributeModelTemp.IsTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsFilter"])))
                        attributeModelTemp.IsFilter = Convert.ToBoolean(dtValue.Rows[rowcount]["IsFilter"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncludeForGrossPay"])))
                        attributeModelTemp.IsIncludeForGrossPay = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncludeForGrossPay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMonthlyInput"])))
                        attributeModelTemp.IsMonthlyInput = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMonthlyInput"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTaxable"])))
                        attributeModelTemp.IsTaxable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTaxable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncrement"])))
                        attributeModelTemp.IsIncrement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncrement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsReimbursement"])))
                        attributeModelTemp.IsReimbursement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsReimbursement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FullAndFinalSettlement"])))
                        attributeModelTemp.FullAndFinalSettlement = Convert.ToBoolean(dtValue.Rows[rowcount]["FullAndFinalSettlement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsInstallment"])))
                        attributeModelTemp.IsInstallment = Convert.ToBoolean(dtValue.Rows[rowcount]["IsInstallment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefault"])))
                        attributeModelTemp.IsDefault = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefault"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"])))
                        attributeModelTemp.AttributeModelTypeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"])))
                        attributeModelTemp.BehaviorType = Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSetting"])))
                        attributeModelTemp.IsSetting = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSetting"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        attributeModelTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ContributionType"])))
                        attributeModelTemp.ContributionType = Convert.ToInt32(dtValue.Rows[rowcount]["ContributionType"]);
                    this.Add(attributeModelTemp);
                }
            }
        }

        public AttributeModelList(Guid entityModelId)
        {
            AttributeModel attributeModel = new AttributeModel();
            DataTable dtValue = attributeModel.GetTableValues(entityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModel attributeModelTemp = new AttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        attributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        attributeModelTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    attributeModelTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    attributeModelTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        attributeModelTemp.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]));
                    attributeModelTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DataSize"])))
                        attributeModelTemp.DataSize = Convert.ToInt32(dtValue.Rows[rowcount]["DataSize"]);
                    attributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMandatory"])))
                        attributeModelTemp.IsMandatory = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMandatory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNumber"])))
                        attributeModelTemp.OrderNumber = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTransaction"])))
                        attributeModelTemp.IsTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsFilter"])))
                        attributeModelTemp.IsFilter = Convert.ToBoolean(dtValue.Rows[rowcount]["IsFilter"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncludeForGrossPay"])))
                        attributeModelTemp.IsIncludeForGrossPay = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncludeForGrossPay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMonthlyInput"])))
                        attributeModelTemp.IsMonthlyInput = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMonthlyInput"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTaxable"])))
                        attributeModelTemp.IsTaxable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTaxable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncrement"])))
                        attributeModelTemp.IsIncrement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncrement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsReimbursement"])))
                        attributeModelTemp.IsReimbursement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsReimbursement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FullAndFinalSettlement"])))
                        attributeModelTemp.FullAndFinalSettlement = Convert.ToBoolean(dtValue.Rows[rowcount]["FullAndFinalSettlement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsInstallment"])))
                        attributeModelTemp.IsInstallment = Convert.ToBoolean(dtValue.Rows[rowcount]["IsInstallment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefault"])))
                        attributeModelTemp.IsDefault = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefault"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"])))
                        attributeModelTemp.AttributeModelTypeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"])))
                        attributeModelTemp.BehaviorType = Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSetting"])))
                        attributeModelTemp.IsSetting = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSetting"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        attributeModelTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ContributionType"])))
                        attributeModelTemp.ContributionType = Convert.ToInt32(dtValue.Rows[rowcount]["ContributionType"]);
                    this.Add(attributeModelTemp);
                }
            }
        }

        public AttributeModelList(Guid EntityModelId, int CompanyId)
        {
            AttributeModel attributeModel = new AttributeModel();
            DataTable dtValue = attributeModel.GetTableValues(CompanyId, EntityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModel attributeModelTemp = new AttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        attributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        attributeModelTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    attributeModelTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    attributeModelTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        attributeModelTemp.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]));
                    attributeModelTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DataSize"])))
                        attributeModelTemp.DataSize = Convert.ToInt32(dtValue.Rows[rowcount]["DataSize"]);
                    attributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMandatory"])))
                        attributeModelTemp.IsMandatory = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMandatory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNumber"])))
                        attributeModelTemp.OrderNumber = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTransaction"])))
                        attributeModelTemp.IsTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsFilter"])))
                        attributeModelTemp.IsFilter = Convert.ToBoolean(dtValue.Rows[rowcount]["IsFilter"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncludeForGrossPay"])))
                        attributeModelTemp.IsIncludeForGrossPay = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncludeForGrossPay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMonthlyInput"])))
                        attributeModelTemp.IsMonthlyInput = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMonthlyInput"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTaxable"])))
                        attributeModelTemp.IsTaxable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTaxable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncrement"])))
                        attributeModelTemp.IsIncrement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncrement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsReimbursement"])))
                        attributeModelTemp.IsReimbursement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsReimbursement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FullAndFinalSettlement"])))
                        attributeModelTemp.FullAndFinalSettlement = Convert.ToBoolean(dtValue.Rows[rowcount]["FullAndFinalSettlement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsInstallment"])))
                        attributeModelTemp.IsInstallment = Convert.ToBoolean(dtValue.Rows[rowcount]["IsInstallment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefault"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefault"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"])))
                        attributeModelTemp.AttributeModelTypeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"])))
                        attributeModelTemp.BehaviorType = Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSetting"])))
                        attributeModelTemp.IsSetting = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSetting"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        attributeModelTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ContributionType"])))
                        attributeModelTemp.ContributionType = Convert.ToInt32(dtValue.Rows[rowcount]["ContributionType"]);
                    this.Add(attributeModelTemp);
                }
            }
        }

        public AttributeModelList(int companyId)
        {
            this.CompanyId = companyId;
            AttributeModel attributeModel = new AttributeModel();
            DataTable dtValue = attributeModel.GetTableValues(this.CompanyId, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModel attributeModelTemp = new AttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        attributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        attributeModelTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    attributeModelTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    attributeModelTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        attributeModelTemp.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]));
                    attributeModelTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DataSize"])))
                        attributeModelTemp.DataSize = Convert.ToInt32(dtValue.Rows[rowcount]["DataSize"]);
                    attributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMandatory"])))
                        attributeModelTemp.IsMandatory = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMandatory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderNumber"])))
                        attributeModelTemp.OrderNumber = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTransaction"])))
                        attributeModelTemp.IsTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsFilter"])))
                        attributeModelTemp.IsFilter = Convert.ToBoolean(dtValue.Rows[rowcount]["IsFilter"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncludeForGrossPay"])))
                        attributeModelTemp.IsIncludeForGrossPay = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncludeForGrossPay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMonthlyInput"])))
                        attributeModelTemp.IsMonthlyInput = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMonthlyInput"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsTaxable"])))
                        attributeModelTemp.IsTaxable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsTaxable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsIncrement"])))
                        attributeModelTemp.IsIncrement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsIncrement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsReimbursement"])))
                        attributeModelTemp.IsReimbursement = Convert.ToBoolean(dtValue.Rows[rowcount]["IsReimbursement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FullAndFinalSettlement"])))
                        attributeModelTemp.FullAndFinalSettlement = Convert.ToBoolean(dtValue.Rows[rowcount]["FullAndFinalSettlement"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsInstallment"])))
                        attributeModelTemp.IsInstallment = Convert.ToBoolean(dtValue.Rows[rowcount]["IsInstallment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefault"])))
                        attributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefault"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"])))
                        attributeModelTemp.AttributeModelTypeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelTypeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"])))
                        attributeModelTemp.BehaviorType = Convert.ToString(dtValue.Rows[rowcount]["BehaviorType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSetting"])))
                        attributeModelTemp.IsSetting = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSetting"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        attributeModelTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ContributionType"])))
                        attributeModelTemp.ContributionType = Convert.ToInt32(dtValue.Rows[rowcount]["ContributionType"]);
                    this.Add(attributeModelTemp);
                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// get or set the Type
        /// </summary>
        public int Type { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the AttributeModel and add to the list
        /// </summary>
        /// <param name="attributeModel"></param>
        public void AddNew(AttributeModel attributeModel)
        {
            if (attributeModel.Save())
            {
                this.Add(attributeModel);
            }
        }

        /// <summary>
        /// Delete the AttributeModel and remove from the list
        /// </summary>
        /// <param name="attributeModel"></param>
        public void DeleteExist(AttributeModel attributeModel)
        {
            if (attributeModel.Delete())
            {
                this.Remove(attributeModel);
            }
        }

        public AttributeModel GetAttributeModel(Guid attributeModelId)
        {
            return this.Where(u => u.Id == attributeModelId).FirstOrDefault();
        }

        public AttributeModelList FilterByContributionType(int contributionType)
        {
            var temp = this.Where(u => u.ContributionType == contributionType).ToList();
            AttributeModelList ret = new AttributeModelList();
            temp.ForEach(u =>
            {
                ret.Add(u);
            });
            return ret;
        }

        #endregion

        #region private methods




        #endregion
    }
}
