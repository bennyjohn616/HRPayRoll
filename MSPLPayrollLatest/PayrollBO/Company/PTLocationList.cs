// -----------------------------------------------------------------------
// <copyright file="PTLocationList.cs" company="Microsoft">
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
    using SQLDBOperation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PTLocationList : List<PTLocation>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PTLocationList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PTLocationList(int companyId)
        {
            this.CompanyId = companyId;
            PTLocation PTLocation = new PTLocation();
            DataTable dtValue = PTLocation.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PTLocation PTLocationTemp = new PTLocation();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PTLocationTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PTLocationTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    PTLocationTemp.PTLocationName = Convert.ToString(dtValue.Rows[rowcount]["PTLocationName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        PTLocationTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        PTLocationTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        PTLocationTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        PTLocationTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        PTLocationTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(PTLocationTemp);
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
        /// Save the PTLocation and add to the list
        /// </summary>
        /// <param name="PTLocation"></param>
        public void AddNew(PTLocation PTLocation)
        {
            if (PTLocation.Save())
            {
                this.Add(PTLocation);
            }
        }

        /// <summary>
        /// Delete the PTLocation and remove from the list
        /// </summary>
        /// <param name="PTLocation"></param>
        public void DeleteExist(PTLocation PTLocation)
        {
            if (PTLocation.Delete())
            {
                this.Remove(PTLocation);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
