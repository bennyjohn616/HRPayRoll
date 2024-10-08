
// -----------------------------------------------------------------------
// <copyright file="TXSectionDefinitionList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO.Tax
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To handle the TXFinanceYear
    /// </summary>
    public class TXSectionDefinitionList : List<TXSectionDefinition>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSectionDefinitionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSectionDefinitionList(int companyId)
        {
            this.CompanyId = companyId;
            TXSectionDefinition txSectionDefinition = new TXSectionDefinition();
            DataTable dtValue = txSectionDefinition.GetTableValues(Guid.Empty, Guid.Empty, Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSectionDefinition txSectionDefinitionTemp = new TXSectionDefinition();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionDefinitionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txSectionDefinitionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionDefinitionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionDefinitionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionDefinitionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionDefinitionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionDefinitionTemp.DefinitionValue = Convert.ToString(dtValue.Rows[rowcount]["DefinitionValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ComputeType"])))
                        txSectionDefinitionTemp.ComputeType = Convert.ToInt32(dtValue.Rows[rowcount]["ComputeType"]);
                    txSectionDefinitionTemp.ControlType = Convert.ToString(dtValue.Rows[rowcount]["ControlType"]);
                    txSectionDefinitionTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRequired"])))
                        txSectionDefinitionTemp.IsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovalRequired"])))
                        txSectionDefinitionTemp.IsApprovalRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovalRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionDefinitionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(txSectionDefinitionTemp);
                }

            }
        }

        public TXSectionDefinitionList(int companyId, Guid sectionId)
        {
            this.CompanyId = companyId;
            TXSectionDefinition txSectionDefinition = new TXSectionDefinition();
            DataTable dtValue = txSectionDefinition.GetTableValues(Guid.Empty, sectionId, Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSectionDefinition txSectionDefinitionTemp = new TXSectionDefinition();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionDefinitionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txSectionDefinitionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionDefinitionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionDefinitionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionDefinitionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionDefinitionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionDefinitionTemp.DefinitionValue = Convert.ToString(dtValue.Rows[rowcount]["DefinitionValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ComputeType"])))
                        txSectionDefinitionTemp.ComputeType = Convert.ToInt32(dtValue.Rows[rowcount]["ComputeType"]);
                    txSectionDefinitionTemp.ControlType = Convert.ToString(dtValue.Rows[rowcount]["ControlType"]);
                    txSectionDefinitionTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRequired"])))
                        txSectionDefinitionTemp.IsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovalRequired"])))
                        txSectionDefinitionTemp.IsApprovalRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovalRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionDefinitionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(txSectionDefinitionTemp);
                }

            }
        }

        public TXSectionDefinitionList(int companyId, Guid sectionId, Guid parentId)
        {
            this.CompanyId = companyId;
            TXSectionDefinition txSectionDefinition = new TXSectionDefinition();
            DataTable dtValue = txSectionDefinition.GetTableValues(Guid.Empty, sectionId, parentId, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSectionDefinition txSectionDefinitionTemp = new TXSectionDefinition();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionDefinitionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txSectionDefinitionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        txSectionDefinitionTemp.ParentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txSectionDefinitionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txSectionDefinitionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    txSectionDefinitionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    txSectionDefinitionTemp.DefinitionValue = Convert.ToString(dtValue.Rows[rowcount]["DefinitionValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ComputeType"])))
                        txSectionDefinitionTemp.ComputeType = Convert.ToInt32(dtValue.Rows[rowcount]["ComputeType"]);
                    txSectionDefinitionTemp.ControlType = Convert.ToString(dtValue.Rows[rowcount]["ControlType"]);
                    txSectionDefinitionTemp.DataType = Convert.ToString(dtValue.Rows[rowcount]["DataType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRequired"])))
                        txSectionDefinitionTemp.IsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApprovalRequired"])))
                        txSectionDefinitionTemp.IsApprovalRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApprovalRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocumentRequired"])))
                        txSectionDefinitionTemp.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocumentRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSectionDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(txSectionDefinitionTemp);
                }

            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public Guid SectionId { get; set; }

        public int ParentId { get; set; }


        #endregion

        #region Public methods

        /// <summary>
        /// Save the TXSectionDefinition and add to the list
        /// </summary>
        /// <param name="txSectionDefinition"></param>
        public void AddNew(TXSectionDefinition txSectionDefinition)
        {
            if (txSectionDefinition.Save())
            {
                this.Add(txSectionDefinition);
            }
        }

        /// <summary>
        /// delete the tax finance year data
        /// </summary>
        /// <param name="txSectionDefinition"></param>

        public void DeleteExist(TXSectionDefinition txSectionDefinition)
        {
            if (txSectionDefinition.Delete())
            {
                this.Remove(txSectionDefinition);
            }
        }


        #endregion

    }
}
