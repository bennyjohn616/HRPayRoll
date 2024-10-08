// -----------------------------------------------------------------------
// <copyright file="TXEmployeeSectionDefinition.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO.Tax
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the TXEmployeeSectionDefinition
    /// </summary>
    public class TXEmployeeSectionDefinition
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXEmployeeSectionDefinition()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXEmployeeSectionDefinition(Guid id, Guid employeeId, Guid sectinId, Guid sectionDefId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, employeeId, sectinId, sectionDefId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SectionId"])))
                    this.SectionId = new Guid(Convert.ToString(dtValue.Rows[0]["SectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SectionDefinitionId"])))
                    this.SectionDefinitionId = new Guid(Convert.ToString(dtValue.Rows[0]["SectionDefinitionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.SectionValue = Convert.ToString(dtValue.Rows[0]["SectionValue"]);
                this.SectionApprovedValue = Convert.ToString(dtValue.Rows[0]["SectionApprovedValue"]);
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                this.DocumentPath = Convert.ToString(dtValue.Rows[0]["DocumentPath"]);
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
        /// Get or Set the SectionId
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Get or Set the SectionDefinitionId
        /// </summary>
        public Guid SectionDefinitionId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the SectionValue
        /// </summary>
        public string SectionValue { get; set; }

        /// <summary>
        /// Get or Set the SectionApprovedValue
        /// </summary>
        public string SectionApprovedValue { get; set; }

        /// <summary>
        /// Get or Set the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get or Set the DocumentPath
        /// </summary>
        public string DocumentPath { get; set; }

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


        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXEmployeeSectionDefinition
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSectionDefinition_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@SectionId", this.SectionId);
            sqlCommand.Parameters.AddWithValue("@SectionDefinitionId", this.SectionDefinitionId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SectionValue", this.SectionValue);
            sqlCommand.Parameters.AddWithValue("@SectionApprovedValue", this.SectionApprovedValue);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@DocumentPath", this.DocumentPath);
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
        /// Delete the TXEmployeeSectionDefinition
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSectionDefinition_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXEmployeeSectionDefinition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid employeeId, Guid sectionId, Guid sectionDefId)
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSectionDefinition_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@SectionId", sectionId);
            sqlCommand.Parameters.AddWithValue("@SectionDefinitionId", sectionDefId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}


