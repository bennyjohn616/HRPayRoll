using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class EntityList : List<Entity>
    {
        #region private variable

      //  private EntityAttributeModelList _entityAttributeModelList;

        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityList(Guid entityModelId)
        {
            this.EntityModelId = entityModelId;
            Entity entity = new Entity();
            DataTable dtValue = entity.GetTableValues(this.EntityModelId, Guid.Empty);

            // this.EntityAttributeModelList = enityAttributeModellist;
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityAttributeModelList enityAttributeModellist = new EntityAttributeModelList(this.EntityModelId);
                    EntityAttributeValueList entityAttributeValuelist = new EntityAttributeValueList(this.EntityModelId);
                    Entity entityTemp = new Entity();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        entityTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    //Add the entityattributeModel

                    EntityAttributeModelList entityAttributeModellistTemp = new EntityAttributeModelList();
                    foreach (EntityAttributeModel ea in enityAttributeModellist)
                    {
                        ea.EntityAttributeValue = entityAttributeValuelist.GetEntityAttributeValue(ea.Id, entityTemp.Id);
                        entityAttributeModellistTemp.Add(ea);
                    }
                    entityTemp.EntityAttributeModelList = entityAttributeModellistTemp;

                    //foreach (EntityAttributeModel ea in enityAttributeModellist)
                    //{
                    //    entityTemp.EntityAttributeValueList.Add(entityAttributeValuelist.GetEntityAttributeValue(ea.Id, entityTemp.Id));
                    //}
                    this.Add(entityTemp);
                }
                // this.EntityAttributeModelList = enityAttributeModellist;
            }
            else
            {
                Entity entityTemp = new Entity();
                entityTemp.EntityAttributeModelList = new EntityAttributeModelList(this.EntityModelId);
                this.Add(entityTemp);
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// get or set the EntityAttributeModelList
        /// </summary>
        //public EntityAttributeModelList EntityAttributeModelList
        //{
        //    get
        //    {
        //        if (object.ReferenceEquals(_entityAttributeModelList, null))
        //        {
        //            if (this.EntityModelId != Guid.Empty)
        //            {
        //                _entityAttributeModelList = new EntityAttributeModelList(this.EntityModelId);

        //            }
        //            else
        //            {
        //                _entityAttributeModelList = new EntityAttributeModelList();
        //            }
        //        }
        //        return _entityAttributeModelList;
        //    }
        //    set
        //    {
        //        _entityAttributeModelList = value;
        //    }
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Entity
        /// </summary>
        /// <returns></returns>
        public void AddNew(Entity entity)
        {

            if (entity.Save())
            {
                this.Add(entity);
            }
        }

        /// <summary>
        /// Delete the Entity
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(Entity entity)
        {
            if (entity.Delete())
            {
                this.Remove(entity);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
