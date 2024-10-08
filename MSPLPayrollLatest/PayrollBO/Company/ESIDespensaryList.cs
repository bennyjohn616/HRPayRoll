// -----------------------------------------------------------------------
// <copyright file="ESIDespensaryList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.SqlClient;
    using SQLDBOperation;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ESIDespensaryList : List<EsiDespensary>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ESIDespensaryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public ESIDespensaryList(int companyId)
        {
            this.CompanyId = companyId;
            EsiDespensary esiDespensary = new EsiDespensary();
            DataTable dtValue = esiDespensary.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EsiDespensary esiDespensaryTemp = new EsiDespensary();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        esiDespensaryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        esiDespensaryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    esiDespensaryTemp.ESIDespensary = Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        esiDespensaryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        esiDespensaryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        esiDespensaryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        esiDespensaryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        esiDespensaryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(esiDespensaryTemp);

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
        /// Save the EsiDespensary and add to the list
        /// </summary>
        /// <param name="esiDespensary"></param>
        public void AddNew(EsiDespensary esiDespensary)
        {
            if (esiDespensary.Save())
            {
                this.Add(esiDespensary);
            }
        }

        /// <summary>
        /// Delete the EsiDespensary and remove from the list
        /// </summary>
        /// <param name="esiDespensary"></param>
        public void DeleteExist(EsiDespensary esiDespensary)
        {
            if (esiDespensary.Delete())
            {
                this.Remove(esiDespensary);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
