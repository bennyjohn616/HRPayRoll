// -----------------------------------------------------------------------
// <copyright file="EmployeeTrainingList.cs" company="Microsoft">
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
    public class EmployeeTrainingList : List<EmployeeTraining>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeTrainingList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeTrainingList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeTraining employeeTraining = new EmployeeTraining();
            DataTable dtValue = employeeTraining.GetTableValues(this.EmployeeId,Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeTraining employeeTrainingTemp = new EmployeeTraining();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTrainingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeTrainingTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeTrainingTemp.TrainingName = Convert.ToString(dtValue.Rows[rowcount]["TrainingName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TrainingDate"])))
                        employeeTrainingTemp.TrainingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["TrainingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TrainingTo"])))
                        employeeTrainingTemp.TrainingTo = Convert.ToDateTime(dtValue.Rows[rowcount]["TrainingTo"]);
                    employeeTrainingTemp.CertificateNumber = Convert.ToString(dtValue.Rows[rowcount]["CertificateNumber"]);
                    employeeTrainingTemp.Institute = Convert.ToString(dtValue.Rows[rowcount]["Institute"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTrainingTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTrainingTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTrainingTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTrainingTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTrainingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeTrainingTemp);
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
        /// Save the EmployeeTraining and add to the list
        /// </summary>
        /// <param name="employeeTraining"></param>
        public void AddNew(EmployeeTraining employeeTraining)
        {
            if (employeeTraining.Save())
            {
                this.Add(employeeTraining);
            }
        }

        /// <summary>
        /// Delete the EmployeeTraining and remove from the list
        /// </summary>
        /// <param name="employeeTraining"></param>
        public void DeleteExist(EmployeeTraining employeeTraining)
        {
            if (employeeTraining.Delete())
            {
                this.Remove(employeeTraining);
            }
        }


        #endregion

        #region private methods



        #endregion
    }
}
