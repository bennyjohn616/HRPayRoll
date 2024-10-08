using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EntityModelMappingList:List<EntityModelMapping>
    {

        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityModelMappingList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityModelMappingList(string refEntityModelName,int companyId)
        {
            this.RefEntityModelName = refEntityModelName;
            EntityModelMapping tableCategory = new EntityModelMapping();
            DataTable dtValue = tableCategory.GetTableValues(this.RefEntityModelName,string.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityModelMapping entityModelMappingTemp = new EntityModelMapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        entityModelMappingTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    entityModelMappingTemp.EntityTableName = Convert.ToString(dtValue.Rows[rowcount]["EntityTableName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelName"])))
                        entityModelMappingTemp.RefEntityModelName = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        entityModelMappingTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        entityModelMappingTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(entityModelMappingTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public string RefEntityModelName { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the TableCategory
        /// </summary>
        /// <returns></returns>
        public void AddNew(EntityModelMapping entityModelMapping)
        {

            if (entityModelMapping.Save())
            {
                this.Add(entityModelMapping);
            }
        }

        /// <summary>
        /// Delete the TableCategory
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(EntityModelMapping entityModelMapping)
        {
            if (entityModelMapping.Delete())
            {
                this.Remove(entityModelMapping);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
