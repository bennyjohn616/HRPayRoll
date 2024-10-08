// -----------------------------------------------------------------------
// <copyright file="FormRights.cs" company="Microsoft">
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
    /// To handle the FormRights
    /// </summary>
    public class FormRights
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FormRights()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public FormRights(Guid id,string roleName)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, roleName);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FormId"])))
                    this.FormId = new Guid(Convert.ToString(dtValue.Rows[0]["FormId"]));
                this.RoleName = Convert.ToString(dtValue.Rows[0]["RoleName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ViewRights"])))
                    this.ViewRights = Convert.ToBoolean(dtValue.Rows[0]["ViewRights"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EditRights"])))
                    this.EditRights = Convert.ToBoolean(dtValue.Rows[0]["EditRights"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DeleteRights"])))
                    this.DeleteRights = Convert.ToBoolean(dtValue.Rows[0]["DeleteRights"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
            }
        }


        #endregion

        #region property
        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the FormId
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Get or Set the RoleName
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Get or Set the ViewRights
        /// </summary>
        public bool ViewRights { get; set; }

        /// <summary>
        /// Get or Set the EditRights
        /// </summary>
        public bool EditRights { get; set; }

        /// <summary>
        /// Get or Set the DeleteRights
        /// </summary>
        public bool DeleteRights { get; set; }

        //public int CompanyId { get; set; } //--


        #endregion

        #region Public methods


        /// <summary>
        /// Save the FormRights
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("FormRights_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FormId", this.FormId);
            sqlCommand.Parameters.AddWithValue("@RoleName", this.RoleName);
            sqlCommand.Parameters.AddWithValue("@ViewRights", this.ViewRights);
            sqlCommand.Parameters.AddWithValue("@EditRights", this.EditRights);
            sqlCommand.Parameters.AddWithValue("@DeleteRights", this.DeleteRights);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
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
        /// Delete the FormRights
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("FormRights_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the FormRights
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid formId,string roleName)
        {

            SqlCommand sqlCommand = new SqlCommand("FormRights_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FormId", formId);
            sqlCommand.Parameters.AddWithValue("@RoleName", roleName);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

