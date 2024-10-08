// -----------------------------------------------------------------------
// <copyright file="Entity.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the Entity
    /// </summary>
    public class Entity
    {

        #region private variable

        private EntityAttributeModelList _entityAttributeModelList;

        private EntityAttributeValueList _entityAttributeValueList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Entity()
        {
            EntityAttributeModelList = new EntityAttributeModelList();
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Entity(Guid entityModelId, Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(entityModelId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// get or set the EntityAttributeModelList
        /// </summary>
        public EntityAttributeModelList EntityAttributeModelList
        {
            get
            {
                if (object.ReferenceEquals(_entityAttributeModelList, null)|| _entityAttributeModelList.Count==0)
                {
                    if (this.EntityModelId != Guid.Empty)
                    {
                        _entityAttributeModelList = new EntityAttributeModelList(this.EntityModelId, this.Id);

                    }
                    else
                    {
                        _entityAttributeModelList = new EntityAttributeModelList();
                    }
                }
                
                return _entityAttributeModelList;
            }
            set
            {
                _entityAttributeModelList = value;
            }
        }

        //public EntityAttributeValueList EntityAttributeValueList
        //{
        //    get
        //    {
        //        if (object.ReferenceEquals(_entityAttributeValueList, null))
        //        {
        //            if (this.Id != Guid.Empty && this.EntityModelId != Guid.Empty)
        //            {
        //                _entityAttributeValueList = new EntityAttributeValueList(this.EntityModelId, this.Id);

        //            }
        //            else
        //            {
        //                _entityAttributeValueList = new EntityAttributeValueList();
        //            }
        //        }
        //        return _entityAttributeValueList;
        //    }
        //    set
        //    {
        //        _entityAttributeValueList = value;
        //    }
        //}

        #endregion

        #region Public methods


        /// <summary>
        /// Save the Entity
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Entity_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
                EntityBehaviorList entitybehavior = new EntityBehaviorList(this.Id, this.EntityModelId);
                for (int cnt = 0; cnt < this.EntityAttributeModelList.Count; cnt++)
                {
                    this.EntityAttributeModelList[cnt].EntityAttributeValue.EntityAttributeModelId = this.EntityAttributeModelList[cnt].Id;
                    this.EntityAttributeModelList[cnt].EntityAttributeValue.EntityId = this.Id;
                    this.EntityAttributeModelList[cnt].EntityAttributeValue.EntityModelId = this.EntityModelId;
                    this.EntityAttributeModelList[cnt].AttributeModelId = this.EntityAttributeModelList[cnt].AttributeModel.Id;
                    this.EntityAttributeModelList[cnt].EntityAttributeValue.Save();
                    if (this.EntityAttributeModelList[cnt].AttributeModel.IsMonthlyInput)
                    {
                        var tmp = entitybehavior.Where(u => u.AttributeModelId == this.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                        if (object.ReferenceEquals(tmp, null))
                        {
                            EntityBehavior entbehav = new EntityBehavior();
                            entbehav.AttributeModelId = this.EntityAttributeModelList[cnt].AttributeModelId;
                            entbehav.CreatedBy = this.CreatedBy;
                            entbehav.EntityId = this.Id;
                            entbehav.EntityModelId = this.EntityModelId;
                            entbehav.Formula = "0";
                            entbehav.IsActive = true;
                            //entbehav.ValueType = 2;//monthly input Arrear Component
                            entbehav.RoundingId = 1;
                            entbehav.Save();
                        }
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// Delete the Entity
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Entity_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityModelID", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool Copy(Guid sourceEntityId, Guid sourceEntityModelId, int createdBy)
        {
            SqlCommand sqlCommand = new SqlCommand("Entity_Copy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SourceEntityId", sourceEntityId);
            sqlCommand.Parameters.AddWithValue("@SourceEntityModelId", sourceEntityModelId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", createdBy);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;

            //    Entity entity = new Entity(sourceEntityModelId, sourceEntityId);
            //Entity newEntity = entity;
            //newEntity.Id = Guid.Empty;
            //newEntity.CreatedBy = createdBy;
            //newEntity.Name = entity.Name + "- Copy";
            //if (newEntity.Save())
            //{
            //    entity.EntityAttributeModelList.ForEach(u =>
            //    {
            //        u.EntityAttributeValue.Id = Guid.Empty;
            //        u.EntityAttributeValue.EntityId = newEntity.Id;
            //        u.EntityAttributeValue.Save();
            //    });
            //    EntityBehaviorList entityBehavior = new EntityBehaviorList(sourceEntityId, sourceEntityModelId);
            //    entityBehavior.ForEach(u =>
            //    {
            //        u.EntityId = newEntity.Id;
            //        u.Save();
            //    });


            //}
            //return true;
        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid entityModelId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Entity_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

