// -----------------------------------------------------------------------
// <copyright file="ImportTemplateDetailsList.cs" company="Microsoft">
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
    public class ImportTemplateDetailsList : List<ImportTemplateDetail>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ImportTemplateDetailsList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ImportTemplateDetailsList(Guid templateId,int companyId)
        {

            ImportTemplateDetail importTemplateDetail = new ImportTemplateDetail();
            DataTable dtValue = importTemplateDetail.GetTableValues(Guid.Empty, templateId, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ImportTemplateDetail importTemplateDetailtemp = new ImportTemplateDetail();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        importTemplateDetailtemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        importTemplateDetailtemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ImportTemplateId"])))
                        importTemplateDetailtemp.ImportTemplateId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ImportTemplateId"]));
                    importTemplateDetailtemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    importTemplateDetailtemp.MappedSheetName = Convert.ToString(dtValue.Rows[rowcount]["MappedSheetName"]);
                    importTemplateDetailtemp.TableColumn = Convert.ToString(dtValue.Rows[rowcount]["TableColumn"]);
                    importTemplateDetailtemp.MappedSheetColumn = Convert.ToString(dtValue.Rows[rowcount]["MappedSheetColumn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        importTemplateDetailtemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        importTemplateDetailtemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        importTemplateDetailtemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        importTemplateDetailtemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        importTemplateDetailtemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(importTemplateDetailtemp);
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
        /// <param name="importTemplateDetail"></param>
        public void AddNew(ImportTemplateDetail importTemplateDetail)
        {
            if (importTemplateDetail.Save())
            {
                this.Add(importTemplateDetail);
            }
        }

        /// <summary>
        /// Delete the importTemplate and remove from the list
        /// </summary>
        /// <param name="importTemplateDetail"></param>
        public void DeleteExist(ImportTemplateDetail importTemplateDetail)
        {
            //if (importTemplateDetail.Delete())
            //{
            //    this.Remove(importTemplateDetail);
            //}
        }

        #endregion

        #region private methods


        #endregion

    }
}
