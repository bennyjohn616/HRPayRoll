// -----------------------------------------------------------------------
// <copyright file="ImportTemplateDetail.cs" company="Microsoft">
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
    /// To handle the ImportTemplateDetail
    /// </summary>
    public class ImportTemplateDetail
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ImportTemplateDetail()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ImportTemplateDetail(Guid id,int companyId=0)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id,Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ImportTemplateId"])))
                    this.ImportTemplateId = new Guid(Convert.ToString(dtValue.Rows[0]["ImportTemplateId"]));
                this.TableName = Convert.ToString(dtValue.Rows[0]["TableName"]);
                this.MappedSheetName = Convert.ToString(dtValue.Rows[0]["MappedSheetName"]);
                this.TableColumn = Convert.ToString(dtValue.Rows[0]["TableColumn"]);
                this.MappedSheetColumn = Convert.ToString(dtValue.Rows[0]["MappedSheetColumn"]);
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
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the ImportTemplateId
        /// </summary>
        public Guid ImportTemplateId { get; set; }

        /// <summary>
        /// Get or Set the TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get or Set the MappedSheetName
        /// </summary>
        public string MappedSheetName { get; set; }

        /// <summary>
        /// Get or Set the TableColumn
        /// </summary>
        public string TableColumn { get; set; }

        /// <summary>
        /// Get or Set the MappedSheetColumn
        /// </summary>
        public string MappedSheetColumn { get; set; }

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
        /// Save the ImportTemplateDetail
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ImportTemplateDetail_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@ImportTemplateId", this.ImportTemplateId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@MappedSheetName", this.MappedSheetName);
            sqlCommand.Parameters.AddWithValue("@TableColumn", this.TableColumn);
            sqlCommand.Parameters.AddWithValue("@MappedSheetColumn", this.MappedSheetColumn);
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
        /// Delete the ImportTemplateDetail
        /// </summary>
        /// <returns></returns>
        public bool Delete(int companyId,Guid templateId)
        {

            SqlCommand sqlCommand = new SqlCommand("ImportTemplateDetail_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ImportTemplateId", templateId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the ImportTemplateDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id,Guid importTemplateId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("ImportTemplateDetail_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@ImportTemplateId", importTemplateId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

