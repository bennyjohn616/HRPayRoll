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
    public class EmployeeJoingDocumentList : List<EmployeeJoingDocument>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeJoingDocumentList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeJoingDocumentList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeJoingDocument employeeJoingDocument = new EmployeeJoingDocument();
            DataTable dtValue = employeeJoingDocument.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeJoingDocument employeeJoingDocumentTemp = new EmployeeJoingDocument();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeJoingDocumentTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeJoingDocumentTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["JoiningDocumentId"])))
                        employeeJoingDocumentTemp.JoiningDocumentId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["JoiningDocumentId"]));
                    employeeJoingDocumentTemp.FilePath = Convert.ToString(dtValue.Rows[rowcount]["FilePath"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeJoingDocumentTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeJoingDocumentTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeJoingDocumentTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeJoingDocumentTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeJoingDocumentTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeJoingDocumentTemp);
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
        /// <param name="employeeJoingDocument"></param>
        public void AddNew(EmployeeJoingDocument employeeJoingDocument)
        {
            if (employeeJoingDocument.Save())
            {
                this.Add(employeeJoingDocument);
            }
        }

        /// <summary>
        /// Delete the EmployeeFamily and remove from the list
        /// </summary>
        /// <param name="employeeJoingDocument"></param>
        public void DeleteExist(EmployeeJoingDocument employeeJoingDocument)
        {
            if (employeeJoingDocument.Delete())
            {
                this.Remove(employeeJoingDocument);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
