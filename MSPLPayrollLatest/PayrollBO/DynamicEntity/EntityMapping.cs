// -----------------------------------------------------------------------
// <copyright file="EntityMapping.cs" company="Microsoft">
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
    /// To handle the EntityMapping
    /// </summary>
    public class EntityMapping
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityMapping()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityMapping(Guid id, Guid entityId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, entityId,Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.EntityId = Convert.ToString(dtValue.Rows[0]["EntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalEntity"])))
                    this.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalEntity"]);
                this.EntityTableName = Convert.ToString(dtValue.Rows[0]["EntityTableName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = Convert.ToString(dtValue.Rows[0]["RefEntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        //Modified By:Sharmila
        public EntityMapping(Guid entityId)
        {

            DataTable dtValue = this.GetTableValues(entityId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.EntityId = Convert.ToString(dtValue.Rows[0]["EntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalEntity"])))
                    this.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalEntity"]);
                this.EntityTableName = Convert.ToString(dtValue.Rows[0]["EntityTableName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = Convert.ToString(dtValue.Rows[0]["RefEntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        public EntityMapping(string refEntityModelId, string refEntityId, Guid entityModelId)
        {
            DataTable dtValue = this.GetTableValuesByRef(refEntityModelId, refEntityId, entityModelId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.EntityId = Convert.ToString(dtValue.Rows[0]["EntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalEntity"])))
                    this.IsPhysicalEntity = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalEntity"]);
                this.EntityTableName = Convert.ToString(dtValue.Rows[0]["EntityTableName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = Convert.ToString(dtValue.Rows[0]["RefEntityId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]);
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
        /// Get or Set the EntityId
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Get or Set the IsPhysicalEntity
        /// </summary>
        public bool IsPhysicalEntity { get; set; }

        /// <summary>
        /// Get or Set the EntityTableName
        /// </summary>
        public string EntityTableName { get; set; }

        /// <summary>
        /// Get or Set the RefEntityId
        /// </summary>
        public string RefEntityId { get; set; }

        /// <summary>
        /// Get or Set the RefEntityModelId
        /// </summary>
        public string RefEntityModelId { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the EntityMapping
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMapping_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@IsPhysicalEntity", this.IsPhysicalEntity);
            sqlCommand.Parameters.AddWithValue("@EntityTableName", this.EntityTableName);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
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

        /// <summary>
        /// Delete the EntityMapping
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMapping_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            // sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the EntityMapping
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid refEntityId, Guid entityModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMapping_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EntityId", refEntityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            sqlCommand.Parameters.AddWithValue("@ByRef", false);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", string.Empty);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", string.Empty);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid entityModelId,string blank)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMappingTable_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Modified By:Sharmila
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid entityId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMappingSelect_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValuesByRef(string refEntityModelId, string refEntityId, Guid entityModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMapping_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@EntityId", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            sqlCommand.Parameters.AddWithValue("@ByRef", true);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", refEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", refEntityId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

