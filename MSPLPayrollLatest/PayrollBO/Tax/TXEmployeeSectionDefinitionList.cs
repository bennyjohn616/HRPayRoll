
// -----------------------------------------------------------------------
// <copyright file="TXEmployeeSectionDefinitionList.cs" company="Microsoft">
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
    /// To handle the TXEmployeeSectionDefinitionList
    /// </summary>
    public class TXEmployeeSectionDefinitionList : List<TXEmployeeSectionDefinition>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXEmployeeSectionDefinitionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXEmployeeSectionDefinitionList(Guid employeeId, Guid sectionId)
        {
            this.EmployeeId = employeeId;
            TXEmployeeSectionDefinition txEmployeeSectionDefinition = new TXEmployeeSectionDefinition();
            DataTable dtValue = txEmployeeSectionDefinition.GetTableValues(Guid.Empty, employeeId, sectionId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXEmployeeSectionDefinition txEmployeeSectionDefinitionTemp = new TXEmployeeSectionDefinition();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txEmployeeSectionDefinitionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txEmployeeSectionDefinitionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionDefinitionId"])))
                        txEmployeeSectionDefinitionTemp.SectionDefinitionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionDefinitionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txEmployeeSectionDefinitionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    txEmployeeSectionDefinitionTemp.SectionValue = Convert.ToString(dtValue.Rows[rowcount]["SectionValue"]);
                    txEmployeeSectionDefinitionTemp.SectionApprovedValue = Convert.ToString(dtValue.Rows[rowcount]["SectionApprovedValue"]);
                    txEmployeeSectionDefinitionTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    txEmployeeSectionDefinitionTemp.DocumentPath = Convert.ToString(dtValue.Rows[rowcount]["DocumentPath"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txEmployeeSectionDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txEmployeeSectionDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txEmployeeSectionDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txEmployeeSectionDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txEmployeeSectionDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txEmployeeSectionDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(txEmployeeSectionDefinitionTemp);
                }

            }
        }

        public TXEmployeeSectionDefinitionList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            TXEmployeeSectionDefinition txEmployeeSectionDefinition = new TXEmployeeSectionDefinition();
            DataTable dtValue = txEmployeeSectionDefinition.GetTableValues(Guid.Empty, employeeId, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXEmployeeSectionDefinition txEmployeeSectionDefinitionTemp = new TXEmployeeSectionDefinition();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txEmployeeSectionDefinitionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txEmployeeSectionDefinitionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionDefinitionId"])))
                        txEmployeeSectionDefinitionTemp.SectionDefinitionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionDefinitionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txEmployeeSectionDefinitionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    txEmployeeSectionDefinitionTemp.SectionValue = Convert.ToString(dtValue.Rows[rowcount]["SectionValue"]);
                    txEmployeeSectionDefinitionTemp.SectionApprovedValue = Convert.ToString(dtValue.Rows[rowcount]["SectionApprovedValue"]);
                    txEmployeeSectionDefinitionTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    txEmployeeSectionDefinitionTemp.DocumentPath = Convert.ToString(dtValue.Rows[rowcount]["DocumentPath"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txEmployeeSectionDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txEmployeeSectionDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txEmployeeSectionDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txEmployeeSectionDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txEmployeeSectionDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txEmployeeSectionDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(txEmployeeSectionDefinitionTemp);
                }

            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public Guid EmployeeId { get; set; }

        public Guid SectionId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the TXEmployeeSectionDefinition and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(TXEmployeeSectionDefinition txEmployeeSectionDefinition)
        {
            if (txEmployeeSectionDefinition.Save())
            {
                this.Add(txEmployeeSectionDefinition);
            }
        }

        /// <summary>
        /// delete the tax TXEmployeeSectionDefinition
        /// </summary>
        /// <param name="txEmployeeSectionDefinition"></param>

        public void DeleteExist(TXEmployeeSectionDefinition txEmployeeSectionDefinition)
        {
            if (txEmployeeSectionDefinition.Delete())
            {
                this.Remove(txEmployeeSectionDefinition);
            }
        }


        #endregion

    }
}
