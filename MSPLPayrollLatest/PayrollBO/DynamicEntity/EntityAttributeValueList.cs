using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class EntityAttributeValueList : List<EntityAttributeValue>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityAttributeValueList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAttributeValueList(Guid entityModelId)
        {
            this.EntityModelId = entityModelId;
            EntityAttributeValue entityAttributeValue = new EntityAttributeValue();
            DataTable dtValue = entityAttributeValue.GetTableValues(this.EntityModelId, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAttributeValue entityAttributeValueTemp = new EntityAttributeValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAttributeValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        entityAttributeValueTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAttributeValueTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityAttributeModelId"])))
                        entityAttributeValueTemp.EntityAttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityAttributeModelId"]));
                    
                    entityAttributeValueTemp.Value =Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    entityAttributeValueTemp.ValueCode = Convert.ToString(dtValue.Rows[rowcount]["ValueCode"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityAttributeValueTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAttributeValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityAttributeValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityAttributeValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityAttributeValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(entityAttributeValueTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAttributeValueList(Guid entityModelId, Guid entityId)
        {
            this.EntityModelId = entityModelId;
            EntityAttributeValue entityAttributeValue = new EntityAttributeValue();
            DataTable dtValue = entityAttributeValue.GetTableValues(this.EntityModelId, entityId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAttributeValue entityAttributeValueTemp = new EntityAttributeValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityAttributeValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        entityAttributeValueTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityAttributeValueTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityAttributeModelId"])))
                        entityAttributeValueTemp.EntityAttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityAttributeModelId"]));

                    entityAttributeValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    entityAttributeValueTemp.ValueCode = Convert.ToString(dtValue.Rows[rowcount]["ValueCode"]);    

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityAttributeValueTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityAttributeValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityAttributeValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityAttributeValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityAttributeValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(entityAttributeValueTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or set the Entity Id
        /// </summary>
        public Guid EntityId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the EntityAttributeValue
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityAttributeValue entityAttributeValue)
        {

            if (entityAttributeValue.Save())
            {
                this.Add(entityAttributeValue);
            }
        }

        /// <summary>
        /// Delete the EntityAttributeValue
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityAttributeValue entityAttributeValue)
        {
            if (entityAttributeValue.Delete())
            {
                this.Remove(entityAttributeValue);
            }
        }

        /// <summary>
        /// get the entity attribute value
        /// </summary>
        /// <param name="entityModelId"></param>
        /// <returns></returns>
        public EntityAttributeValue GetEntityAttributeValue(Guid entityAttributeModelId, Guid entityId)
        {
            return this.Where(u => u.EntityAttributeModelId == entityAttributeModelId && u.EntityId == entityId).FirstOrDefault();
        }
        #endregion

        #region private methods


        #endregion
    }
}
