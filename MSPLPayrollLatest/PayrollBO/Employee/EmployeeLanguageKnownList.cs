// -----------------------------------------------------------------------
// <copyright file="EmployeeLanguageKnownList.cs" company="Microsoft">
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
    public class EmployeeLanguageKnownList : List<EmployeeLanguageKnown>
    {


        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeLanguageKnownList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeLanguageKnownList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeLanguageKnown employeeLanguageKnown = new EmployeeLanguageKnown();
            DataTable dtValue = employeeLanguageKnown.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeLanguageKnown employeeLanguageKnownTemp = new EmployeeLanguageKnown();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeLanguageKnownTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeLanguageKnownTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LanguageId"])))
                        employeeLanguageKnownTemp.LanguageId = Convert.ToInt32(dtValue.Rows[rowcount]["LanguageId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSpeak"])))
                        employeeLanguageKnownTemp.IsSpeak = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSpeak"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRead"])))
                        employeeLanguageKnownTemp.IsRead = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRead"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsWrite"])))
                        employeeLanguageKnownTemp.IsWrite = Convert.ToBoolean(dtValue.Rows[rowcount]["IsWrite"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeLanguageKnownTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeLanguageKnownTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeLanguageKnownTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeLanguageKnownTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeLanguageKnownTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    employeeLanguageKnownTemp.Language= Convert.ToString(dtValue.Rows[rowcount]["Language"]);
                    this.Add(employeeLanguageKnownTemp);
                }
            }
        }


        #endregion

        #region property



        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the EmployeeLanguageKnown and add to the list
        /// </summary>
        /// <param name="employeeLanguageKnown"></param>
        public void AddNew(EmployeeLanguageKnown employeeLanguageKnown)
        {
            if (employeeLanguageKnown.Save())
            {
                this.Add(employeeLanguageKnown);
            }
        }

        /// <summary>
        /// Delete the EmployeeLanguageKnown and remove from the list
        /// </summary>
        /// <param name="employeeLanguageKnown"></param>
        public void DeleteExist(EmployeeLanguageKnown employeeLanguageKnown)
        {
            if (employeeLanguageKnown.Delete())
            {
                this.Remove(employeeLanguageKnown);
            }
        }

        #endregion

        #region private methods



        #endregion

    }
}
