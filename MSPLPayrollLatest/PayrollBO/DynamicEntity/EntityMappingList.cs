using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EntityMappingList : List<EntityMapping>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityMappingList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityMappingList(string refEntityModelId)
        {
            this.RefEntityModelId = refEntityModelId;
            EntityMapping entityMapping = new EntityMapping();
            DataTable dtValue = entityMapping.GetTableValuesByRef(this.RefEntityModelId, string.Empty,Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMapping entityMappingTemp = new EntityMapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMappingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityMappingTemp.EntityId = Convert.ToString(dtValue.Rows[rowcount]["EntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalEntity"])))
                        entityMappingTemp.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalEntity"]);
                    entityMappingTemp.EntityTableName = Convert.ToString(dtValue.Rows[rowcount]["EntityTableName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMappingTemp.RefEntityId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMappingTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMappingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMappingTemp);
                }
            }
        }

        //Created in order to check the Dynamic entity mapping for leave settings.
        public EntityMappingList(string refEntityModelId,string refEntityId,Guid EntityModelId)
        {
            
            EntityMapping entityMapping = new EntityMapping();
            DataTable dtValue = entityMapping.GetTableValuesByRef(refEntityModelId, refEntityId, EntityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMapping entityMappingTemp = new EntityMapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMappingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityMappingTemp.EntityId = Convert.ToString(dtValue.Rows[rowcount]["EntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalEntity"])))
                        entityMappingTemp.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalEntity"]);
                    entityMappingTemp.EntityTableName = Convert.ToString(dtValue.Rows[rowcount]["EntityTableName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMappingTemp.RefEntityId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMappingTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMappingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMappingTemp);
                }
            }
        }

        public EntityMappingList(Guid entityId)
        {

            EntityMapping entityMapping = new EntityMapping();
            DataTable dtValue = entityMapping.GetTableValues(Guid.Empty, entityId,Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMapping entityMappingTemp = new EntityMapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMappingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityMappingTemp.EntityId = Convert.ToString(dtValue.Rows[rowcount]["EntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalEntity"])))
                        entityMappingTemp.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalEntity"]);
                    entityMappingTemp.EntityTableName = Convert.ToString(dtValue.Rows[rowcount]["EntityTableName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMappingTemp.RefEntityId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMappingTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMappingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMappingTemp);
                }
            }
        }

        public EntityMappingList(Guid entityModelId,string blank)
        {

            EntityMapping entityMapping = new EntityMapping();
            DataTable dtValue = entityMapping.GetTableValues(entityModelId,blank);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMapping entityMappingTemp = new EntityMapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMappingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityMappingTemp.EntityId = Convert.ToString(dtValue.Rows[rowcount]["EntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalEntity"])))
                        entityMappingTemp.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalEntity"]);
                    entityMappingTemp.EntityTableName = Convert.ToString(dtValue.Rows[rowcount]["EntityTableName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMappingTemp.RefEntityId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMappingTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMappingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMappingTemp);
                }
            }
        }




        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public string RefEntityModelId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the entityMapping
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityMapping entityMapping)
        {

            if (entityMapping.Save())
            {
                this.Add(entityMapping);
            }
        }

        /// <summary>
        /// Delete the entityMapping
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityMapping entityMapping)
        {
            if (entityMapping.Delete())
            {
                this.Remove(entityMapping);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
