// -----------------------------------------------------------------------
// <copyright file="BloodGroupList.cs" company="Microsoft">
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
    public class BloodGroupList : List<BloodGroup>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public BloodGroupList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public BloodGroupList(bool isAll)
        {
            if (!isAll)
                return;
            BloodGroup bloodGroup = new BloodGroup();
            DataTable dtValue = bloodGroup.GetTableValues(0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    BloodGroup bloodGrouptemp = new BloodGroup();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        bloodGrouptemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    bloodGrouptemp.BloodGroupName = Convert.ToString(dtValue.Rows[rowcount]["BloodGroup"]);
                    this.Add(bloodGrouptemp);
                }

            }
        }


        #endregion

        #region property


        #endregion

        #region Public methods

        /// <summary>
        /// Save the BloodGroup and add to the list
        /// </summary>
        /// <param name="bloodGroup"></param>
        public void AddNew(BloodGroup bloodGroup)
        {
            if (bloodGroup.Save())
            {
                this.Add(bloodGroup);
            }
        }

        /// <summary>
        /// Delete the BloodGroup and remove from the list
        /// </summary>
        /// <param name="bloodGroup"></param>
        public void DeleteExist(BloodGroup bloodGroup)
        {
            if (bloodGroup.Delete())
            {
                this.Remove(bloodGroup);
            }
        }

        #endregion

        #region private methods


        #endregion

    }
}
