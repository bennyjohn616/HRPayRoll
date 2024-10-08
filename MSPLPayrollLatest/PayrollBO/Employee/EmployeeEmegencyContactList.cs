// -----------------------------------------------------------------------
// <copyright file="EmployeeEmegencyContactList.cs" company="Microsoft">
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
    public class EmployeeEmegencyContactList : List<EmployeeEmegencyContact>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeEmegencyContactList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeEmegencyContactList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeEmegencyContact employeeEmegencyContact = new EmployeeEmegencyContact();
            DataTable dtValue = employeeEmegencyContact.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeEmegencyContact employeeEmegencyContactTemp = new EmployeeEmegencyContact();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeEmegencyContactTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeEmegencyContactTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeEmegencyContactTemp.ContactName = Convert.ToString(dtValue.Rows[rowcount]["ContactName"]);
                    employeeEmegencyContactTemp.ContactNumber = Convert.ToString(dtValue.Rows[rowcount]["ContactNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RelationShip"])))
                        employeeEmegencyContactTemp.RelationShip = Convert.ToInt32(dtValue.Rows[rowcount]["RelationShip"]);
                    employeeEmegencyContactTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeEmegencyContactTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeEmegencyContactTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeEmegencyContactTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeEmegencyContactTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeEmegencyContactTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeEmegencyContactTemp);
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
        /// Save the EmployeeEmegencyContact and add to the list
        /// </summary>
        /// <param name="employeeEmegencyContact"></param>
        public void AddNew(EmployeeEmegencyContact employeeEmegencyContact)
        {
            if (employeeEmegencyContact.Save())
            {
                this.Add(employeeEmegencyContact);
            }
        }

        /// <summary>
        /// Delete the EmployeeEmegencyContact and remove from the list
        /// </summary>
        /// <param name="employeeEmegencyContact"></param>
        public void DeleteExist(EmployeeEmegencyContact employeeEmegencyContact)
        {
            if (employeeEmegencyContact.Delete())
            {
                this.Remove(employeeEmegencyContact);
            }
        }


        #endregion

        #region private methods



        #endregion
    }
}
