// -----------------------------------------------------------------------
// <copyright file="AttributeValueList.cs" company="Microsoft">
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
    public class AttributeValueList : List<AttributeValue>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeValueList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public AttributeValueList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            AttributeValue attributeValue = new AttributeValue();
            DataTable dtValue = new DataTable();
           // DataTable dtValue = attributeValue.GetTableValues(this.EmployeeId, Guid.Empty);
            if (dtValue != null && dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeValue attributeValueTemp = new AttributeValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        attributeValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        attributeValueTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        attributeValueTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    attributeValueTemp.ValueCode = Convert.ToString(dtValue.Rows[rowcount]["ValueCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(attributeValueTemp);
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
        /// Save the AttributeValue and add to the list
        /// </summary>
        /// <param name="attributeValue"></param>
        public void AddNew(AttributeValue attributeValue)
        {
            if (attributeValue.Save())
            {
                this.Add(attributeValue);
            }
        }

        /// <summary>
        /// Delete the AttributeValue and remove from the list
        /// </summary>
        /// <param name="attributeValue"></param>
        public void DeleteExist(AttributeValue attributeValue)
        {
            if (attributeValue.Delete())
            {
                this.Remove(attributeValue);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
