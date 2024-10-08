// -----------------------------------------------------------------------
// <copyright file="FormCommand.cs" company="Microsoft">
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

    // using System.Data;

    /// <summary>
    /// To handle the FormCommand
    /// </summary>
    public class FormCommand
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FormCommand()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public FormCommand(int id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.CommandName = Convert.ToString(dtValue.Rows[0]["CommandName"]);
                this.Description = Convert.ToString(dtValue.Rows[0]["Description"]);
                this.CommandTypes = Convert.ToString(dtValue.Rows[0]["CommandTypes"]);
                this.TableName = Convert.ToString(dtValue.Rows[0]["TableName"]);
                this.ColumnName = Convert.ToString(dtValue.Rows[0]["ColumnName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefaultRead"])))
                    this.IsDefaultRead = Convert.ToBoolean(dtValue.Rows[0]["IsDefaultRead"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefaultWrite"])))
                    this.IsDefaultWrite = Convert.ToBoolean(dtValue.Rows[0]["IsDefaultWrite"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefaultRequired"])))
                    this.IsDefaultRequired = Convert.ToBoolean(dtValue.Rows[0]["IsDefaultRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefaultApprovel"])))
                    this.IsDefaultApprovel = Convert.ToBoolean(dtValue.Rows[0]["IsDefaultApprovel"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefaultTransaction"])))
                    this.IsDefaultTransaction = Convert.ToBoolean(dtValue.Rows[0]["IsDefaultTransaction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = Convert.ToInt32(dtValue.Rows[0]["ParentId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DependentId"])))
                    this.DependentId = Convert.ToInt32(dtValue.Rows[0]["DependentId"]);
                this.FormName = Convert.ToString(dtValue.Rows[0]["FormName"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the CommandName
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Get or Set the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or Set the CommandType
        /// </summary>
        public string CommandTypes { get; set; }

        /// <summary>
        /// Get or Set the TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get or Set the ColumnName
        /// </summary>
        public string ColumnName { get; set; }

        public string FormName { get; set; }

        /// <summary>
        /// Get or Set the IsDefaultRead
        /// </summary>
        public bool IsDefaultRead { get; set; }

        /// <summary>
        /// Get or Set the IsDefaultWrite
        /// </summary>
        public bool IsDefaultWrite { get; set; }

        /// <summary>
        /// Get or Set the IsDefaultRequired
        /// </summary>
        public bool IsDefaultRequired { get; set; }

        /// <summary>
        /// Get or Set the IsDefaultApprovel
        /// </summary>
        public bool IsDefaultApprovel { get; set; }

        /// <summary>
        /// Get or Set the IsDefaultTransaction
        /// </summary>
        public bool IsDefaultTransaction { get; set; }

        /// <summary>
        /// get or set the parent Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// get or set the Dependentid
        /// </summary>
        public int DependentId { get; set; }
       
      

        public string ModuleType { get; set; }

        public string ParentMenu { get; set; }
        public int DisplayOrder { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the FormCommand
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("FormCommand_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CommandName", this.CommandName);
            sqlCommand.Parameters.AddWithValue("@Description", this.Description);
            sqlCommand.Parameters.AddWithValue("@CommandTypes", this.CommandTypes);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@ColumnName", this.ColumnName);
            sqlCommand.Parameters.AddWithValue("@IsDefaultRead", this.IsDefaultRead);
            sqlCommand.Parameters.AddWithValue("@IsDefaultWrite", this.IsDefaultWrite);
            sqlCommand.Parameters.AddWithValue("@IsDefaultRequired", this.IsDefaultRequired);
            sqlCommand.Parameters.AddWithValue("@IsDefaultApprovel", this.IsDefaultApprovel);
            sqlCommand.Parameters.AddWithValue("@IsDefaultTransaction", this.IsDefaultTransaction);
          
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
        /// Delete the FormCommand
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("FormCommand_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            // sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the FormCommand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, string type = "")
        {

            SqlCommand sqlCommand = new SqlCommand("FormCommand_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

