// -----------------------------------------------------------------------
// <copyright file="EntityAttributeModel.cs" company="Microsoft">
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
    /// To handle the EntityAttributeModel
    /// </summary>
    public class EntityAttributeModel
    {

        #region private variable

        private EntityAttributeValue _entityAttributeValue;

        private AttributeModel _attributeModel;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityAttributeModel()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAttributeModel(Guid entityModelId, Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(entityModelId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DisplayOrder"])))
                    this.DisplayOrder = Convert.ToInt32(dtValue.Rows[0]["DisplayOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsHidden"])))
                    this.IsHidden = Convert.ToBoolean(dtValue.Rows[0]["IsHidden"]);
                this.DefaultValue = Convert.ToString(dtValue.Rows[0]["DefaultValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsMasterField"])))
                    this.IsMasterField = Convert.ToBoolean(dtValue.Rows[0]["IsMasterField"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Get or Set the IsHidden
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Get or Set the DefaultValue
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

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
        /// get or set the IsMasterField
        /// </summary>
        public bool IsMasterField { get; set; }

        public string ValueType { get; set; }

        /// <summary>
        /// get or set the EntityAttributeModelList
        /// </summary>
        public EntityAttributeValue EntityAttributeValue
        {
            get
            {
                if (object.ReferenceEquals(_entityAttributeValue, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _entityAttributeValue = new EntityAttributeValue();

                    }
                    else
                    {
                        _entityAttributeValue = new EntityAttributeValue();
                    }
                }               
                return _entityAttributeValue;
            }
            set
            {
                _entityAttributeValue = value;
            }
        }

        public AttributeModel AttributeModel
        {
            get
            {
                if (object.ReferenceEquals(_attributeModel, null))
                {
                    if (this.AttributeModelId != Guid.Empty)
                    {
                        _attributeModel = new AttributeModel(this.AttributeModelId, 0);

                    }
                    else
                    {
                        _attributeModel = new AttributeModel();
                    }
                }
                return _attributeModel;
            }
            set
            {
                _attributeModel = value;
            }

        }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the EntityAttributeModel
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAttributeModel_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@DisplayOrder", this.DisplayOrder);
            sqlCommand.Parameters.AddWithValue("@IsHidden", this.IsHidden);
            sqlCommand.Parameters.AddWithValue("@DefaultValue", this.DefaultValue);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsMasterField", this.IsMasterField);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }
        public bool Merge(int companyId,Guid EntityModelId,Guid AttributeModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityId_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", EntityModelId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", AttributeModelId);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            return dbOperation.SaveData(sqlCommand, out outValue, "");
        }

        /// <summary>
        /// Delete the EntityAttributeModel
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAttributeModel_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool InterChangeOrder(Guid entityModelId, Guid entityAttributeModelId, string action)
        {
            EntityAttributeModelList enityAttmodelList = new EntityAttributeModelList(entityModelId);
            var entityAttr = enityAttmodelList.Where(u => u.Id == entityAttributeModelId).FirstOrDefault();
            if (object.ReferenceEquals(entityAttr, null))
                return false;
            int order = 0;
            EntityAttributeModel oldentityAttr = null;
            order = entityAttr.DisplayOrder;
            if (order <= 0)
                order = 0;
            if (action == "Down")
            {
                oldentityAttr =  enityAttmodelList.Where(u => u.DisplayOrder == order + 1).FirstOrDefault();
                if(oldentityAttr != null)
                    oldentityAttr.DisplayOrder = order;
                if (entityAttr != null)
                    entityAttr.DisplayOrder = order + 1;
            }
            else if (action == "Up")
            {
                oldentityAttr = enityAttmodelList.Where(u => u.DisplayOrder == order - 1).FirstOrDefault();
                if (oldentityAttr != null)
                    oldentityAttr.DisplayOrder = order;
                if (entityAttr != null)
                    entityAttr.DisplayOrder = order - 1;
            }
            if (object.ReferenceEquals(oldentityAttr, null))
                return false;
            else
            {
                if (oldentityAttr != null)
                    oldentityAttr.Save();
                if (entityAttr != null)
                    entityAttr.Save();
                return true;
            }

        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the EntityAttributeModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid entityModelId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAttributeModel_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

