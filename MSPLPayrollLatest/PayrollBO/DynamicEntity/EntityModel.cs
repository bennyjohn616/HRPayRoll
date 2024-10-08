// -----------------------------------------------------------------------
// <copyright file="EntityModel.cs" company="Microsoft">
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
    /// To handle the EntityModel
    /// </summary>
    public class EntityModel
    {

        #region private variable

        private EntityAttributeModelList _entityAttributeModelList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityModel()
        {

        }

        public EntityModel(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(Guid.Empty, this.Id, string.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalTable"])))
                    this.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalTable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TableCategoryId"])))
                    this.TableCategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["TableCategoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
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
            }
        }

        public EntityModel(string name, int companyId)
        {
            DataTable dtValue = this.GetTableValues(Guid.Empty, Guid.Empty, name, companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalTable"])))
                    this.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalTable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TableCategoryId"])))
                    this.TableCategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["TableCategoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
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
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityModel(Guid tableCategoryId, Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(tableCategoryId, this.Id, string.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalTable"])))
                    this.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalTable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TableCategoryId"])))
                    this.TableCategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["TableCategoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
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
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the IsPhysicalTable
        /// </summary>
        public bool IsPhysicalTable { get; set; }

        /// <summary>
        /// Get or Set the RefEntityModelId
        /// </summary>
        public Guid RefEntityModelId { get; set; }

        /// <summary>
        /// Get or Set the TableCategoryId
        /// </summary>
        public Guid TableCategoryId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

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
        /// get or set the EntityAttributeModelList
        /// </summary>
        public EntityAttributeModelList EntityAttributeModelList
        {
            get
            {
                if (object.ReferenceEquals(_entityAttributeModelList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _entityAttributeModelList = new EntityAttributeModelList(this.Id);

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


        #endregion

        #region Public methods


        /// <summary>
        /// Save the EntityModel
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityModel_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@IsPhysicalTable", this.IsPhysicalTable);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@TableCategoryId", this.TableCategoryId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        /// Delete the EntityModel
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityModel_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the EntityModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid tableCategoryId, Guid id, string name, int companyId = 0)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityModel_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@TableCategoryId", tableCategoryId);
            sqlCommand.Parameters.AddWithValue("@Name", name);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
    public class DefaultAttribute
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

