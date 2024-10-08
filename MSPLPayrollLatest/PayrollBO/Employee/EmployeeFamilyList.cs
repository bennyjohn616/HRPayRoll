// -----------------------------------------------------------------------
// <copyright file="EmployeeFamilyList.cs" company="Microsoft">
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
    public class EmployeeFamilyList : List<EmployeeFamily>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeFamilyList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeFamilyList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeFamily employeeFamily = new EmployeeFamily();
            DataTable dtValue = employeeFamily.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeFamily employeeFamilyTemp = new EmployeeFamily();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeFamilyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeFamilyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeFamilyTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    employeeFamilyTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RelationShip"])))
                        employeeFamilyTemp.RelationShip = Convert.ToInt32(dtValue.Rows[rowcount]["RelationShip"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeFamilyTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Age"])))
                        employeeFamilyTemp.Age = Convert.ToInt32(dtValue.Rows[rowcount]["Age"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeFamilyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeFamilyTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeFamilyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeFamilyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeFamilyTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeFamilyTemp);
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
        /// Save the EmployeeFamily and add to the list
        /// </summary>
        /// <param name="employeeFamily"></param>
        public void AddNew(EmployeeFamily employeeFamily)
        {
            if (employeeFamily.Save())
            {
                this.Add(employeeFamily);
            }
        }

        /// <summary>
        /// Delete the EmployeeFamily and remove from the list
        /// </summary>
        /// <param name="employeeFamily"></param>
        public void DeleteExist(EmployeeFamily employeeFamily)
        {
            if (employeeFamily.Delete())
            {
                this.Remove(employeeFamily);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
