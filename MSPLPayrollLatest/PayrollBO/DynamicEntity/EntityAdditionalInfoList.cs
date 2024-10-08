using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EntityAdditionalInfoList : List<EntityAdditionalInfo>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityAdditionalInfoList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAdditionalInfoList(int companyId, Guid entityModelId)
        {

            EntityAdditionalInfo entityAdditionalInfo = new EntityAdditionalInfo();
            entityAdditionalInfo.CompanyId = companyId;
            entityAdditionalInfo.EntityModelId = entityModelId;
            entityAdditionalInfo.EmployeeId = Guid.Empty;
            entityAdditionalInfo.AttributeModelId = Guid.Empty;
            DataTable dtValue = entityAdditionalInfo.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAdditionalInfo entityAdditionalInfoTemp = new EntityAdditionalInfo();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAdditionalInfoTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityAdditionalInfoTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        entityAdditionalInfoTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAdditionalInfoTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityAdditionalInfoTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityAdditionalInfoTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    entityAdditionalInfoTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAdditionalInfoTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityAdditionalInfoTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityAdditionalInfoTemp);
                }
            }
        }

        public EntityAdditionalInfoList(int companyId, Guid entityModelId, Guid employeeId)
        {
            EntityAdditionalInfo entityAdditionalInfo = new EntityAdditionalInfo();
            entityAdditionalInfo.CompanyId = companyId;
            entityAdditionalInfo.EntityModelId = entityModelId;
            entityAdditionalInfo.EmployeeId = employeeId;
            entityAdditionalInfo.AttributeModelId = Guid.Empty;
            DataTable dtValue = entityAdditionalInfo.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAdditionalInfo entityAdditionalInfoTemp = new EntityAdditionalInfo();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAdditionalInfoTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityAdditionalInfoTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        entityAdditionalInfoTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAdditionalInfoTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityAdditionalInfoTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityAdditionalInfoTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    entityAdditionalInfoTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAdditionalInfoTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityAdditionalInfoTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityAdditionalInfoTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid TableCategoryId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the EntityAdditionalInfo
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityAdditionalInfo entityAdditionalInfo)
        {

            if (entityAdditionalInfo.Save())
            {
                this.Add(entityAdditionalInfo);
            }
        }

        /// <summary>
        /// Delete the EntityAdditionalInfo
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityAdditionalInfo entityAdditionalInfo)
        {
            if (entityAdditionalInfo.Delete())
            {
                this.Remove(entityAdditionalInfo);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
