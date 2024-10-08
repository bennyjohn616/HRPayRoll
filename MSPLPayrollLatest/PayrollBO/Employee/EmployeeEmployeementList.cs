// -----------------------------------------------------------------------
// <copyright file="EmployeeEmployeementList.cs" company="Microsoft">
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
    public class EmployeeEmployeementList : List<EmployeeEmployeement>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeEmployeementList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeEmployeementList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeEmployeement employeeEmployeement = new EmployeeEmployeement();
            DataTable dtValue = employeeEmployeement.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeEmployeement employeeEmployeementTemp = new EmployeeEmployeement();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeEmployeementTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeEmployeementTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeEmployeementTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeEmployeementTemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                    employeeEmployeementTemp.PositionHeld = Convert.ToString(dtValue.Rows[rowcount]["PositionHeld"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["WorkFrom"])))
                        employeeEmployeementTemp.WorkFrom = Convert.ToDateTime(dtValue.Rows[rowcount]["WorkFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["WorkTo"])))
                        employeeEmployeementTemp.WorkTo = Convert.ToDateTime(dtValue.Rows[rowcount]["WorkTo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeEmployeementTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeEmployeementTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeEmployeementTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeEmployeementTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeEmployeementTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeEmployeementTemp);
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
        /// Save the EmployeeEmployeement and add to the list
        /// </summary>
        /// <param name="employeeEmployeement"></param>
        public void AddNew(EmployeeEmployeement employeeEmployeement)
        {
            if (employeeEmployeement.Save())
            {
                this.Add(employeeEmployeement);
            }
        }

        /// <summary>
        /// Delete the EmployeeEmployeement and remove from the list
        /// </summary>
        /// <param name="employeeEmployeement"></param>
        public void DeleteExist(EmployeeEmployeement employeeEmployeement)
        {
            if (employeeEmployeement.Delete())
            {
                this.Remove(employeeEmployeement);
            }
        }

        #endregion

        #region private methods


        #endregion
    }
}
