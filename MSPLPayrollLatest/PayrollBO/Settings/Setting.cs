// -----------------------------------------------------------------------
// <copyright file="Setting.cs" company="Microsoft">
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
    /// To handle the Setting
    /// </summary>
    public class Setting
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Setting()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Setting(int id, int companyId)
        {
            this.Id = id;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.Id, this.CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = Convert.ToInt32(dtValue.Rows[0]["ParentId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
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
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the ParentId
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

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


        #endregion

        #region Public methods


        /// <summary>
        /// Save the Setting
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Setting_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@ParentId", this.ParentId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Setting
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Setting_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Setting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("Setting_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
    public class LockSetting
    {
        public LockSetting()
        {

        }
        public LockSetting(int PayrollMonth, int PayrollYear, int PCompanyid, string Type)
        {
            DataTable dtValue = this.LockGetTableValues(PayrollMonth, PayrollYear, PCompanyid, Type);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.PaySheetLockid = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.PaysheetCompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollMonth"])))
                    this.PayrollMonth = Convert.ToInt32(dtValue.Rows[0]["PayrollMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollYear"])))
                    this.PayrollYear = Convert.ToInt32(dtValue.Rows[0]["PayrollYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollLock"])))
                    this.PayrollLock = Convert.ToBoolean(dtValue.Rows[0]["PayrollLock"]);
            }
        }

        //Paysheet lock 
        public Guid PaySheetLockid { get; set; }
        public string PaySheetType { get; set; }
        public int PaysheetCompanyId { get; set; }
        public int PayrollMonth { get; set; }
        public int PayrollYear { get; set; }
        public bool PayrollLock { get; set; }
        public string AdminPassword { get; set; }
        public string PaysheetCreatedBy { get; set; }
        public bool LockSave()
        {
            SqlCommand sqlCommand = new SqlCommand("usp_GetSetPaysheetlock");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.PaySheetLockid);
            sqlCommand.Parameters.AddWithValue("@Type", this.PaySheetType);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.PaysheetCompanyId);
            sqlCommand.Parameters.AddWithValue("@PayrollMonth", this.PayrollMonth);
            sqlCommand.Parameters.AddWithValue("@PayrollYear", this.PayrollYear);
            sqlCommand.Parameters.AddWithValue("@PayrollLock", this.PayrollLock);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.PaysheetCreatedBy);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;
        }
        protected internal DataTable LockGetTableValues(int PPayrollMonth, int PPayrollYear, int PCompanyid, string PType)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_GetSetPaysheetlock");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Type", PType);
            sqlCommand.Parameters.AddWithValue("@CompanyId", PCompanyid);
            sqlCommand.Parameters.AddWithValue("@PayrollMonth", PPayrollMonth);
            sqlCommand.Parameters.AddWithValue("@PayrollYear", PPayrollYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}

