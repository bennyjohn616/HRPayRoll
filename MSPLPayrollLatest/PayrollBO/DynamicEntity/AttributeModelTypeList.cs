using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class AttributeModelTypeList : List<AttributeModelType>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModelTypeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public AttributeModelTypeList(int companyId)
        {
            this.CompanyId = companyId;
            AttributeModelType attributeModeltype = new AttributeModelType();
            DataTable dtValue = attributeModeltype.GetTableValues(Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModelType attributeModelTypeTemp = new AttributeModelType();
                    attributeModelTypeTemp.CompanyId = this.CompanyId;
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        attributeModelTypeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    attributeModelTypeTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    attributeModelTypeTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelTypeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelTypeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelTypeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelTypeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        attributeModelTypeTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(attributeModelTypeTemp);
                }
            }
        }

        #endregion

        #region property
        public int CompanyId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the AttributeModelType and add to the list
        /// </summary>
        /// <param name="attributeModelType"></param>
        public void AddNew(AttributeModelType attributeModelType)
        {
            if (attributeModelType.Save())
            {
                this.Add(attributeModelType);
            }
        }

        /// <summary>
        /// Delete the AttributeModelType and remove from the list
        /// </summary>
        /// <param name="attributeModelType"></param>
        public void DeleteExist(AttributeModelType attributeModelType)
        {
            if (attributeModelType.Delete())
            {
                this.Remove(attributeModelType);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
