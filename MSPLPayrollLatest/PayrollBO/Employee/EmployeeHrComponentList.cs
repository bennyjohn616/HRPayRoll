// -----------------------------------------------------------------------
// <copyright file="EmployeeJoingDocumentList.cs" company="Microsoft">
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
    public class EmployeeHrComponentList : List<EmployeeHrComponent>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeHrComponentList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeHrComponentList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeHrComponent employeehrcomponent = new EmployeeHrComponent();
            DataTable dtValue = employeehrcomponent.GetTableValues(Guid.Empty,this.EmployeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeHrComponent employeeHrComponentTemp = new EmployeeHrComponent();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeHrComponentTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeHrComponentTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HrComponentId"])))
                        employeeHrComponentTemp.HrComponentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["HrComponentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffDate"])))
                        employeeHrComponentTemp.EffDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EffDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EndDate"])))
                        employeeHrComponentTemp.EndDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EndDate"]);
                    employeeHrComponentTemp.Comments = Convert.ToString(dtValue.Rows[rowcount]["Comments"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeHrComponentTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeHrComponentTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeHrComponentTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeHrComponentTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeHrComponentTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                        employeeHrComponentTemp.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                    this.Add(employeeHrComponentTemp);
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
        /// Save the EmployeeJoingDocument and add to the list
        /// </summary>
        /// <param name="employeeHrComponent"></param>
        public void AddNew(EmployeeHrComponent employeeHrComponent)
        {
            if (employeeHrComponent.Save())
            {
                this.Add(employeeHrComponent);
            }
        }

        /// <summary>
        /// Delete the EmployeeFamily and remove from the list
        /// </summary>
        /// <param name="employeeHrComponent"></param>
        public void DeleteExist(EmployeeHrComponent employeeHrComponent)
        {
            if (employeeHrComponent.Delete())
            {
                this.Remove(employeeHrComponent);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
