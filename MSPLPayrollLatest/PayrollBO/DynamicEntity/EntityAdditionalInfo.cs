using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EntityAdditionalInfo
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityAdditionalInfo()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityAdditionalInfo(int companyId, Guid entityId)
        {
            this.CompanyId = companyId;
            this.EntityModelId = entityId;
            this.EntityModelId = Guid.Empty;
            this.AttributeModelId = Guid.Empty;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        public EntityAdditionalInfo(int companyId, Guid entityId, Guid employeeId)
        {
            this.CompanyId = companyId;
            this.EntityModelId = entityId;
            this.EntityModelId = employeeId;
            this.AttributeModelId = Guid.Empty;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        public EntityAdditionalInfo(int companyId, Guid entityId, Guid employeeId, Guid attributeModelId)
        {
            this.CompanyId = companyId;
            this.EntityModelId = entityId;
            this.EntityModelId = employeeId;
            this.AttributeModelId = attributeModelId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
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
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the RefEntityId
        /// </summary>
        public Guid RefEntityId { get; set; }

        ///  /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the EntityAdditionalInfo
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAdditionalInfo_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@CreatedBy ", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy ", this.ModifiedBy);
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
        /// Delete the EntityAdditionalInfo
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAdditionalInfo_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            // sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the EntityAdditionalInfo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityAdditionalInfo_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@AttributemodelId", AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", EntityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }



        #endregion
    }
}
