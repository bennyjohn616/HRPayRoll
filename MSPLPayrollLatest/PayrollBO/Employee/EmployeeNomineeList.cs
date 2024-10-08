// -----------------------------------------------------------------------
// <copyright file="EmployeeNomineeList.cs" company="Microsoft">
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
    public class EmployeeNomineeList : List<EmployeeNominee>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeNomineeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EmployeeNomineeList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeNominee employeeNominee = new EmployeeNominee();
            DataTable dtValue = employeeNominee.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeNominee employeeNomineeTemp = new EmployeeNominee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeNomineeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeNomineeTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeNomineeTemp.NomineeName = Convert.ToString(dtValue.Rows[rowcount]["NomineeName"]);
                    employeeNomineeTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RelationShip"])))
                        employeeNomineeTemp.RelationShip = Convert.ToInt32(dtValue.Rows[rowcount]["RelationShip"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeNomineeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AmountPercentage"])))
                        employeeNomineeTemp.AmountPercentage = Convert.ToDouble(dtValue.Rows[rowcount]["AmountPercentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Age"])))
                        employeeNomineeTemp.Age = Convert.ToInt32(dtValue.Rows[rowcount]["Age"]);
                    employeeNomineeTemp.NameOfGuardianAndAddress = Convert.ToString(dtValue.Rows[rowcount]["NameOfGuardianAndAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeNomineeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeNomineeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeNomineeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeNomineeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeNomineeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeNomineeTemp);
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
        /// Save the EmployeeNominee and add to the list
        /// </summary>
        /// <param name="employeeNominee"></param>
        public void AddNew(EmployeeNominee employeeNominee)
        {
            if (employeeNominee.Save())
            {
                this.Add(employeeNominee);
            }
        }

        /// <summary>
        /// Delete the EmployeeNominee and remove from the list
        /// </summary>
        /// <param name="employeeNominee"></param>
        public void DeleteExist(EmployeeNominee employeeNominee)
        {
            if (employeeNominee.Delete())
            {
                this.Remove(employeeNominee);
            }
        }

        #endregion

        #region private methods



        #endregion

    }
}
