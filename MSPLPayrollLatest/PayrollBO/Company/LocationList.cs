
// -----------------------------------------------------------------------
// <copyright file="LocationList.cs" company="Microsoft">
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
    public class LocationList : List<Location>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LocationList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LocationList(int companyId)
        {
            this.CompanyId = companyId;
            Location Location = new Location();
            DataTable dtValue = Location.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Location LocationTemp = new Location();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        LocationTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        LocationTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    LocationTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        LocationTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        LocationTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        LocationTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        LocationTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        LocationTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(LocationTemp);
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
        /// Save the Location and add to the list
        /// </summary>
        /// <param name="Location"></param>
        public void AddNew(Location Location)
        {
            if (Location.Save())
            {
                this.Add(Location);
            }
        }

        /// <summary>
        /// Delete the Location and remove from the list
        /// </summary>
        /// <param name="Location"></param>
        public void DeleteExist(Location Location)
        {
            if (Location.Delete())
            {
                this.Remove(Location);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
