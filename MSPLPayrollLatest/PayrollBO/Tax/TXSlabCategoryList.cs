
// -----------------------------------------------------------------------
// <copyright file="TXSlabCategoryList.cs" company="Microsoft">
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
    /// To handle the TXSlabCategoryList
    /// </summary>
    public class TXSlabCategoryList : List<TXSlabCategory>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSlabCategoryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSlabCategoryList(int companyId)
        {
            this.CompanyId = companyId;
            TXSlabCategory txSlabCategory = new TXSlabCategory();
            DataTable dtValue = txSlabCategory.GetTableValues(Guid.Empty, this.CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSlabCategory txSlabCategoryTemp = new TXSlabCategory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSlabCategoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    txSlabCategoryTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        this.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    this.Add(txSlabCategoryTemp);
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
        /// Save the Tax FinanceYear and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(TXSlabCategory txSlabCategory)
        {
            if (txSlabCategory.Save())
            {
                this.Add(txSlabCategory);
            }
        }

        /// <summary>
        /// delete the tax finance year data
        /// </summary>
        /// <param name="txSlabCategory"></param>

        public void DeleteExist(TXSlabCategory txSlabCategory)
        {
            if (txSlabCategory.Delete())
            {
                this.Remove(txSlabCategory);
            }
        }


        #endregion

    }
}
