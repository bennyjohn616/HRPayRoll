// -----------------------------------------------------------------------
// <copyright file="SettingValue.cs" company="Microsoft">
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
    /// To handle the SettingValue
    /// </summary>
    public class SettingValue
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public SettingValue()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="settingdefinitionid"></param>
        public SettingValue( int settingId, int settingdefinitionid)
        {
            this.SettingDefinitionId = settingdefinitionid;
            this.SettingId = settingId;
            DataTable dtValue = this.GetTableValues(this.SettingId, this.SettingDefinitionId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SettingId"])))
                    this.SettingId = Convert.ToInt32(dtValue.Rows[0]["SettingId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SettingDefinitionId"])))
                    this.SettingDefinitionId = Convert.ToInt32(dtValue.Rows[0]["SettingDefinitionId"]);
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the SettingId
        /// </summary>
        public int SettingId { get; set; }

        /// <summary>
        /// Get or Set the SettingDefinitionId
        /// </summary>
        public int SettingDefinitionId { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

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
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        public string UniqueConstraintValue { get; set; }

        public string UniqueConstraintName { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the SettingValue
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("SettingValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SettingId", this.SettingId);
            sqlCommand.Parameters.AddWithValue("@SettingDefinitionId", this.SettingDefinitionId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;
        }

        /// <summary>
        /// Delete the SettingValue
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("SettingValue_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SettingDefinitionId", this.SettingDefinitionId);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the SettingValue
        /// </summary>
        /// <param name="settingdefinitionid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int settingId, int settingdefinitionid)
        {

            SqlCommand sqlCommand = new SqlCommand("SettingValue_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@SettingDefinitionId", settingdefinitionid);
            sqlCommand.Parameters.AddWithValue("@SettingId", settingId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

