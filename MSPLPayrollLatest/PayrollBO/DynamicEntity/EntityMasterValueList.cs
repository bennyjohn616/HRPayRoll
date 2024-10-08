using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EntityMasterValueList : List<EntityMasterValue>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityMasterValueList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public EntityMasterValueList(Guid refEntityId, string refEntityModelId)
        {
            this.RefEntityId = refEntityId;
            this.RefEntityModelId = refEntityModelId;
            EntityMasterValue entityMasterValue = new EntityMasterValue();
            DataTable dtValue = entityMasterValue.GetTableValues(Guid.Empty, refEntityId, refEntityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMasterValue entityMasterValueTemp = new EntityMasterValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMasterValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        entityMasterValueTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityMasterValueTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityMasterValueTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMasterValueTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMasterValueTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        entityMasterValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityMasterValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityMasterValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityMasterValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityMasterValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityMasterValueTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMasterValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMasterValueTemp);
                }
            }
        }

        public EntityMasterValueList(Guid EntityId, Guid refEntityId, string refEntityModelId)
        {
            this.EntityId = EntityId;
            this.RefEntityId = refEntityId;
            this.RefEntityModelId = refEntityModelId;
            EntityMasterValue entityMasterValue = new EntityMasterValue();
            DataTable dtValue = entityMasterValue.GetEntityValues(EntityId, refEntityId, refEntityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMasterValue entityMasterValueTemp = new EntityMasterValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityMasterValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        entityMasterValueTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        entityMasterValueTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityMasterValueTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityMasterValueTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"])))
                        entityMasterValueTemp.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        entityMasterValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityMasterValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityMasterValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityMasterValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityMasterValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityMasterValueTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityMasterValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityMasterValueTemp);
                }
            }
        }
          

        #endregion

        #region property
        public string RefEntityModelId { get; set; }

        public Guid RefEntityId { get; set; }

        #endregion
        public Guid EntityId { get; set; }

        #region Public methods

        /// <summary>
        /// Save the entityMasterValue and add to the list
        /// </summary>
        /// <param name="entityMasterValue"></param>
        public void AddNew(EntityMasterValue entityMasterValue)
        {
            if (entityMasterValue.Save())
            {
                this.Add(entityMasterValue);
            }
        }

        /// <summary>
        /// Delete the entityMasterValue and remove from the list
        /// </summary>
        /// <param name="entityMasterValue"></param>
        public void DeleteExist(EntityMasterValue entityMasterValue)
        {
            if (entityMasterValue.Delete())
            {
                this.Remove(entityMasterValue);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
