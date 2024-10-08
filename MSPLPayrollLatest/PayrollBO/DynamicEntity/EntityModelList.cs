using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class EntityModelList : List<EntityModel>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityModelList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityModelList(Guid tableCategoryId)
        {
            this.TableCategoryId = tableCategoryId;
            EntityModel entityModel = new EntityModel();
            DataTable dtValue = entityModel.GetTableValues(this.TableCategoryId, Guid.Empty,string.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityModel entityModelTemp = new EntityModel();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityModelTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityModelTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    entityModelTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalTable"])))
                        entityModelTemp.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalTable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"])))
                        entityModelTemp.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TableCategoryId"])))
                        entityModelTemp.TableCategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TableCategoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        entityModelTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        entityModelTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityModelTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        entityModelTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        entityModelTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        entityModelTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        entityModelTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(entityModelTemp);
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
        /// Save the EntityModel
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityModel entityModel)
        {

            if (entityModel.Save())
            {
                this.Add(entityModel);
            }
        }

        /// <summary>
        /// Delete the EntityModel
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityModel entityModel)
        {
            if (entityModel.Delete())
            {
                this.Remove(entityModel);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
