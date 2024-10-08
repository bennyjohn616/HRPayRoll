// -----------------------------------------------------------------------
// <copyright file="ImportTemplateList.cs" company="Microsoft">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ImportTemplateList : List<ImportTemplate>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ImportTemplateList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ImportTemplateList(int companyId)
        {

            ImportTemplate importTemplate = new ImportTemplate();
            DataTable dtValue = importTemplate.GetTableValues(Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ImportTemplate importTemplatetemp = new ImportTemplate();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        importTemplatetemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        importTemplatetemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    importTemplatetemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        importTemplatetemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        importTemplatetemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        importTemplatetemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        importTemplatetemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        importTemplatetemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(importTemplatetemp);
                }

            }
        }


        #endregion

        #region property


        #endregion

        #region Public methods

        /// <summary>
        /// Save the importTemplate and add to the list
        /// </summary>
        /// <param name="importTemplate"></param>
        public void AddNew(ImportTemplate importTemplate)
        {
            if (importTemplate.Save())
            {
                this.Add(importTemplate);
            }
        }

        /// <summary>
        /// Delete the importTemplate and remove from the list
        /// </summary>
        /// <param name="importTemplate"></param>
        public void DeleteExist(ImportTemplate importTemplate)
        {
            if (importTemplate.Delete())
            {
                this.Remove(importTemplate);
            }
        }

        public void InitializeEmptyObject()
        {
            this.ForEach(u => {
                u.ImportTemplateDetailsList = new ImportTemplateDetailsList();
            });
           
        }

        #endregion

        #region private methods


        #endregion

    }
}
