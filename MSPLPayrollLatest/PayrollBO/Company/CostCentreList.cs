// -----------------------------------------------------------------------
// <copyright file="CostCentreList.cs" company="Microsoft">
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
    public class CostCentreList : List<CostCentre>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public CostCentreList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public CostCentreList(int companyId)
        {
            this.CompanyId = companyId;
            CostCentre costCentre = new CostCentre();
            DataTable dtValue = costCentre.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    CostCentre costCentreTemp = new CostCentre();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        costCentreTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        costCentreTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    costCentreTemp.CostCentreName = Convert.ToString(dtValue.Rows[rowcount]["CostCentreName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        costCentreTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        costCentreTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        costCentreTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        costCentreTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        costCentreTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(costCentreTemp);
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
        /// Save the CostCentre and add to the list
        /// </summary>
        /// <param name="costCentre"></param>
        public void AddNew(CostCentre costCentre)
        {
            if (costCentre.Save())
            {
                this.Add(costCentre);
            }
        }

        /// <summary>
        /// Delete the CostCentre and remove from the list
        /// </summary>
        /// <param name="costCentre"></param>
        public void DeleteExist(CostCentre costCentre)
        {
            if (costCentre.Delete())
            {
                this.Remove(costCentre);
            }
        }


        #endregion

        #region private methods



        #endregion
    }
}
