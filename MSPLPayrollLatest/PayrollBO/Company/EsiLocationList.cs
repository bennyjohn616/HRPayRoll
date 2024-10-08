// -----------------------------------------------------------------------
// <copyright file="EsiLocationList.cs" company="Microsoft">
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
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EsiLocationList : List<EsiLocation>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EsiLocationList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public EsiLocationList(int companyId)
        {
            this.CompanyId = companyId;
            EsiLocation esiLocation = new EsiLocation();
            DataTable dtValue = esiLocation.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EsiLocation esiLocationTemp = new EsiLocation();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        esiLocationTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        esiLocationTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    esiLocationTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isApplicable"])))
                        esiLocationTemp.isApplicable = Convert.ToBoolean(dtValue.Rows[rowcount]["isApplicable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployerCode"])))
                        esiLocationTemp.EmployerCode = Convert.ToString(dtValue.Rows[rowcount]["EmployerCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        esiLocationTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        esiLocationTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        esiLocationTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        esiLocationTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        esiLocationTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(esiLocationTemp);
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
        /// Save the EsiLocation and add to the list
        /// </summary>
        /// <param name="esiLocation"></param>
        public void AddNew(EsiLocation esiLocation)
        {
            if (esiLocation.Save())
            {
                this.Add(esiLocation);
            }
        }

        /// <summary>
        /// Delete the EsiLocation and remove from the list
        /// </summary>
        /// <param name="esiLocation"></param>
        public void DeleteExist(EsiLocation esiLocation)
        {
            if (esiLocation.Delete())
            {
                this.Remove(esiLocation);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
