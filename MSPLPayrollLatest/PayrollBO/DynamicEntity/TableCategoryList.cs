using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class TableCategoryList : List<TableCategory>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TableCategoryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TableCategoryList(int companyId)
        {
            this.CompanyId = companyId;
            TableCategory tableCategory = new TableCategory();
            DataTable dtValue = tableCategory.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TableCategory tableCategoryTemp = new TableCategory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        tableCategoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    tableCategoryTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    tableCategoryTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        tableCategoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        tableCategoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        tableCategoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        tableCategoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        tableCategoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        tableCategoryTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        tableCategoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(tableCategoryTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int CompanyId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the TableCategory
        /// </summary>
        /// <returns></returns>
        public void AddNew(TableCategory tableCategory)
        {

            if (tableCategory.Save())
            {
                this.Add(tableCategory);
            }
        }

        /// <summary>
        /// Delete the TableCategory
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(TableCategory tableCategory)
        {
            if (tableCategory.Delete())
            {
                this.Remove(tableCategory);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
