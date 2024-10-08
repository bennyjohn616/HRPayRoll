// -----------------------------------------------------------------------
// <copyright file="TXSectionDefinition.cs" company="Microsoft">
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
    /// To handle the TXSectionDefinition
    /// </summary>
    public class TXSectionDefinition
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSectionDefinition()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSectionDefinition(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, Guid.Empty, Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SectionId"])))
                    this.SectionId = new Guid(Convert.ToString(dtValue.Rows[0]["SectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = new Guid(Convert.ToString(dtValue.Rows[0]["ParentId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                this.DefinitionValue = Convert.ToString(dtValue.Rows[0]["DefinitionValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ComputeType"])))
                    this.ComputeType = Convert.ToInt32(dtValue.Rows[0]["ComputeType"]);
                this.ControlType = Convert.ToString(dtValue.Rows[0]["ControlType"]);
                this.DataType = Convert.ToString(dtValue.Rows[0]["DataType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsRequired"])))
                    this.IsRequired = Convert.ToBoolean(dtValue.Rows[0]["IsRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsApprovalRequired"])))
                    this.IsApprovalRequired = Convert.ToBoolean(dtValue.Rows[0]["IsApprovalRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDocumentRequired"])))
                    this.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[0]["IsDocumentRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the SectionId
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Get or Set the ParentId
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the DefinitionValue
        /// </summary>
        public string DefinitionValue { get; set; }

        /// <summary>
        /// Get or Set the ComputeType
        /// </summary>
        public int ComputeType { get; set; }

        /// <summary>
        /// Get or Set the ControlType
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Get or Set the DataType
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Get or Set the IsRequired
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Get or Set the IsApprovalRequired
        /// </summary>
        public bool IsApprovalRequired { get; set; }

        /// <summary>
        /// Get or Set the IsDocumentRequired
        /// </summary>
        public bool IsDocumentRequired { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXSectionDefinition
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXSectionDefinition_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@SectionId", this.SectionId);
            sqlCommand.Parameters.AddWithValue("@ParentId", this.ParentId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@DefinitionValue", this.DefinitionValue);
            sqlCommand.Parameters.AddWithValue("@ComputeType", this.ComputeType);
            sqlCommand.Parameters.AddWithValue("@ControlType", this.ControlType);
            sqlCommand.Parameters.AddWithValue("@DataType", this.DataType);
            sqlCommand.Parameters.AddWithValue("@IsRequired", this.IsRequired);
            sqlCommand.Parameters.AddWithValue("@IsApprovalRequired", this.IsApprovalRequired);
            sqlCommand.Parameters.AddWithValue("@IsDocumentRequired", this.IsDocumentRequired);
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
        /// Delete the TXSectionDefinition
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXSectionDefinition_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXSectionDefinition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid sectionId, Guid parentId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("TXSectionDefinition_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@SectionId", sectionId);
            sqlCommand.Parameters.AddWithValue("@ParentId", parentId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

