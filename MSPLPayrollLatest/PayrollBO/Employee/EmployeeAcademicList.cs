// -----------------------------------------------------------------------
// <copyright file="EmployeeAcademicList.cs" company="Microsoft">
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
    public class EmployeeAcademicList : List<EmployeeAcademic>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeAcademicList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeAcademicList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeAcademic employeeAcademic = new EmployeeAcademic();
            DataTable dtValue = employeeAcademic.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeAcademic employeeAcademicTemp = new EmployeeAcademic();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeAcademicTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeAcademicTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeAcademicTemp.DegreeName = Convert.ToString(dtValue.Rows[rowcount]["DegreeName"]);
                    employeeAcademicTemp.InstitionName = Convert.ToString(dtValue.Rows[rowcount]["InstitionName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["YearOfPassing"])))
                        employeeAcademicTemp.YearOfPassing = Convert.ToInt32(dtValue.Rows[rowcount]["YearOfPassing"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeAcademicTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeAcademicTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeAcademicTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeAcademicTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeAcademicTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeAcademicTemp);
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
        /// Save the EmployeeAcademic and add to the list
        /// </summary>
        /// <param name="employeeAcademic"></param>
        public void AddNew(EmployeeAcademic employeeAcademic)
        {
            if (employeeAcademic.Save())
            {
                this.Add(employeeAcademic);
            }
        }

        /// <summary>
        /// Delete the EmployeeAcademic and remove from the list
        /// </summary>
        /// <param name="employeeAcademic"></param>
        public void DeleteExist(EmployeeAcademic employeeAcademic)
        {
            if (employeeAcademic.Delete())
            {
                this.Remove(employeeAcademic);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
