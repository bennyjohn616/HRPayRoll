// -----------------------------------------------------------------------
// <copyright file="CategoryList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the CategoryList
    /// </summary>
    public class CategoryList : List<Category>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public CategoryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public CategoryList(int companyId)
        {
            this.CompanyId = companyId;
            Category category = new Category();
            DataTable dtValue = category.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Category categoryTemp = new Category();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        categoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    categoryTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisOrder"])))
                        categoryTemp.DisOrder = Convert.ToInt32(dtValue.Rows[rowcount]["DisOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        categoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        categoryTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        categoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreaateBy"])))
                        categoryTemp.CreaateBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreaateBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CretedOn"])))
                        categoryTemp.CretedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CretedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        categoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        categoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFLimit"])))
                        categoryTemp.PFLimit = Convert.ToDecimal(dtValue.Rows[rowcount]["PFLimit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFProcess"])))
                        categoryTemp.PFProcess = Convert.ToString(dtValue.Rows[rowcount]["PFProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFInspectionChargeProcess"])))
                        categoryTemp.PFInspectionChargeProcess = Convert.ToString(dtValue.Rows[rowcount]["PFInspectionChargeProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFAdminChargeProcess"])))
                        categoryTemp.PFAdminChargeProcess = Convert.ToString(dtValue.Rows[rowcount]["PFAdminChargeProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFEdliChargeProcess"])))
                        categoryTemp.PFEdliChargeProcess = Convert.ToString(dtValue.Rows[rowcount]["PFEdliChargeProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFRounding"])))
                        categoryTemp.PFRounding = Convert.ToString(dtValue.Rows[rowcount]["PFRounding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILimit"])))
                        categoryTemp.ESILimit = Convert.ToDecimal(dtValue.Rows[rowcount]["ESILimit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIProcess"])))
                        categoryTemp.ESIProcess = Convert.ToString(dtValue.Rows[rowcount]["ESIProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIRounding"])))
                        categoryTemp.ESIRounding = Convert.ToString(dtValue.Rows[rowcount]["ESIRounding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIInspectionChargeProcess"])))
                        categoryTemp.ESIInspectionChargeProcess = Convert.ToString(dtValue.Rows[rowcount]["ESIInspectionChargeProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIEdliChargeProcess"])))
                        categoryTemp.ESIEdliChargeProcess = Convert.ToString(dtValue.Rows[rowcount]["ESIEdliChargeProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MonthDayProcess"])))
                        categoryTemp.MonthDayProcess = Convert.ToString(dtValue.Rows[rowcount]["MonthDayProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MonthDayOrStartDay"])))
                        categoryTemp.MonthDayOrStartDay = Convert.ToInt32(dtValue.Rows[rowcount]["MonthDayOrStartDay"]);
                    this.Add(categoryTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }
        



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(Category category)
        {
            if (category.Save())
            {
                this.Add(category);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="category"></param>
        public void DeleteExist(Category category)
        {
            if (category.Delete())
            {
                this.Remove(category);
            }
        }


        #endregion

        #region private methods




        #endregion

    }
}

