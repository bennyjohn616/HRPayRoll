using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class EntityAttributeModelList : List<EntityAttributeModel>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityAttributeModelList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAttributeModelList(Guid entityModelId)
        {
            this.EntityModelId = entityModelId;
            AttributeModelList attributModels = new AttributeModelList(this.EntityModelId);
            EntityAttributeModel entityAttributeModel = new EntityAttributeModel();
            DataTable dtValue = entityAttributeModel.GetTableValues(this.EntityModelId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAttributeModel entityAttributeModelTemp = new EntityAttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAttributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAttributeModelTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityAttributeModelTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayOrder"])))
                        entityAttributeModelTemp.DisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["DisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHidden"])))
                        entityAttributeModelTemp.IsHidden = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHidden"]);
                    entityAttributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityAttributeModelTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityAttributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAttributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityAttributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityAttributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityAttributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMasterField"])))
                        entityAttributeModelTemp.IsMasterField = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMasterField"]);
                    entityAttributeModelTemp.AttributeModel = attributModels.GetAttributeModel(entityAttributeModelTemp.AttributeModelId);

                    this.Add(entityAttributeModelTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAttributeModelList(Guid entityModelId, Guid entityId)
        {
            this.EntityModelId = entityModelId;
            AttributeModelList attributModels = new AttributeModelList(this.EntityModelId);
            EntityAttributeModel entityAttributeModel = new EntityAttributeModel();
            DataTable dtValue = entityAttributeModel.GetTableValues(this.EntityModelId, Guid.Empty);
            EntityAttributeValueList entityAttributeValuelist = new EntityAttributeValueList(entityModelId, entityId);
            if (dtValue.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAttributeModel entityAttributeModelTemp = new EntityAttributeModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAttributeModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAttributeModelTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityAttributeModelTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayOrder"])))
                        entityAttributeModelTemp.DisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["DisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHidden"])))
                        entityAttributeModelTemp.IsHidden = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHidden"]);
                    entityAttributeModelTemp.DefaultValue = Convert.ToString(dtValue.Rows[rowcount]["DefaultValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityAttributeModelTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityAttributeModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAttributeModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityAttributeModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityAttributeModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityAttributeModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsMasterField"])))
                        entityAttributeModelTemp.IsMasterField = Convert.ToBoolean(dtValue.Rows[rowcount]["IsMasterField"]);
                    entityAttributeModelTemp.AttributeModel = attributModels.GetAttributeModel(entityAttributeModelTemp.AttributeModelId);
                    entityAttributeModelTemp.EntityAttributeValue = entityAttributeValuelist.GetEntityAttributeValue(entityAttributeModelTemp.Id, entityId);
                    this.Add(entityAttributeModelTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid EntityModelId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the EntityAttributeModel
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityAttributeModel entityAttributeModel)
        {

            if (entityAttributeModel.Save())
            {
                this.Add(entityAttributeModel);
            }
        }

        /// <summary>
        /// Delete the EntityAttributeModel
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityAttributeModel entityAttributeModel)
        {
            if (entityAttributeModel.Delete())
            {
                this.Remove(entityAttributeModel);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
