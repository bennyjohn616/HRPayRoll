// -----------------------------------------------------------------------
// <copyright file="EmployeeBenefitComponentList.cs" company="Microsoft">
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
    public class EmployeeBenefitComponentList : List<EmployeeBenefitComponent>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeBenefitComponentList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EmployeeBenefitComponentList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeBenefitComponent employeeBenefitComponent = new EmployeeBenefitComponent();
            DataTable dtValue = employeeBenefitComponent.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeBenefitComponent employeeBenefitComponenttemp = new EmployeeBenefitComponent();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeBenefitComponenttemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeBenefitComponenttemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BenefitComponentId"])))
                        employeeBenefitComponenttemp.BenefitComponentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BenefitComponentId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Amount"])))
                        employeeBenefitComponenttemp.Amount = Convert.ToDecimal(dtValue.Rows[rowcount]["Amount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffectiveDate"])))
                        employeeBenefitComponenttemp.EffectiveDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EffectiveDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeBenefitComponenttemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeBenefitComponenttemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeBenefitComponenttemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeBenefitComponenttemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeBenefitComponenttemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeBenefitComponenttemp);
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
        /// Save the EmployeeBenefitComponent and add to the list
        /// </summary>
        /// <param name="employeeBenefitComponent"></param>
        public void AddNew(EmployeeBenefitComponent employeeBenefitComponent)
        {
            if (employeeBenefitComponent.Save())
            {
                this.Add(employeeBenefitComponent);
            }
        }

        /// <summary>
        /// Delete the EmployeeBenefitComponent and remove from the list
        /// </summary>
        /// <param name="employeeBenefitComponent"></param>
        public void DeleteExist(EmployeeBenefitComponent employeeBenefitComponent)
        {
            if (employeeBenefitComponent.Delete())
            {
                this.Remove(employeeBenefitComponent);
            }
        }


        #endregion

        #region private methods



        #endregion
    }
}
