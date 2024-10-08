// -----------------------------------------------------------------------
// <copyright file="RoleFormCommand.cs" company="Microsoft">
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
    /// To handle the RoleFormCommand
    /// </summary>
    public class RoleFormCommand
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public RoleFormCommand()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyid"></param>
        public RoleFormCommand(Guid id, int companyid)
        {
            this.CompanyId = companyid;
            DataTable dtValue = this.GetTableValues(id, companyid, 0, 0);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FormCommandId"])))
                    this.FormCommandId = Convert.ToInt32(dtValue.Rows[0]["FormCommandId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RoleId"])))
                    this.RoleId = Convert.ToInt32(dtValue.Rows[0]["RoleId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsRead"])))
                    this.IsRead = Convert.ToBoolean(dtValue.Rows[0]["IsRead"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsWrite"])))
                    this.IsWrite = Convert.ToBoolean(dtValue.Rows[0]["IsWrite"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsRequired"])))
                    this.IsRequired = Convert.ToBoolean(dtValue.Rows[0]["IsRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPayrollTransaction"])))
                    this.IsPayrollTransaction = Convert.ToBoolean(dtValue.Rows[0]["IsPayrollTransaction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsApproval"])))
                    this.IsApproval = Convert.ToBoolean(dtValue.Rows[0]["IsApproval"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDelete"])))
                    this.IsDelete = Convert.ToBoolean(dtValue.Rows[0]["IsDelete"]);
                this.ReadMessage = Convert.ToString(dtValue.Rows[0]["ReadMessage"]);
                this.WriteMessage = Convert.ToString(dtValue.Rows[0]["WriteMessage"]);
                this.RequiredMessage = Convert.ToString(dtValue.Rows[0]["RequiredMessage"]);
                this.TransactionMessage = Convert.ToString(dtValue.Rows[0]["TransactionMessage"]);
                this.ApprovalMessage = Convert.ToString(dtValue.Rows[0]["ApprovalMessage"]);
                this.DeleteMessage = Convert.ToString(dtValue.Rows[0]["DeleteMessage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                this.FormName = Convert.ToString(dtValue.Rows[0]["FormName"]);
                
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the FormCommandId
        /// </summary>
        public int FormCommandId { get; set; }

        /// <summary>
        /// Get or Set the RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public string FormName { get; set; }
        /// <summary>
        /// Get or Set the IsRead
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Get or Set the IsWrite
        /// </summary>
        public bool IsWrite { get; set; }

        /// <summary>
        /// Get or Set the IsRequired
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Get or Set the IsPayrollTransaction
        /// </summary>
        public bool IsPayrollTransaction { get; set; }

        /// <summary>
        /// Get or Set the IsApproval
        /// </summary>
        public bool IsApproval { get; set; }

        /// <summary>
        /// get or set the delete
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Get or Set the ReadMessage
        /// </summary>
        public string ReadMessage { get; set; }

        /// <summary>
        /// Get or Set the WriteMessage
        /// </summary>
        public string WriteMessage { get; set; }

        /// <summary>
        /// Get or Set the RequiredMessage
        /// </summary>
        public string RequiredMessage { get; set; }

        /// <summary>
        /// Get or Set the TransactionMessage
        /// </summary>
        public string TransactionMessage { get; set; }

        /// <summary>
        /// Get or Set the ApprovalMessage
        /// </summary>
        public string ApprovalMessage { get; set; }

        /// <summary>
        /// get or set the delete message
        /// </summary>
        public string DeleteMessage { get; set; }

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
        /// Save the RoleFormCommand
        /// </summary>
        /// <returns></returns>
        /// //---Modified by Keerthika on 09/05/2017
        public bool Save()
        {
            if(this.FormCommandId == 0)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("RoleFormCommand_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FormCommandId", this.FormCommandId);
            sqlCommand.Parameters.AddWithValue("@RoleId", this.RoleId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@IsRead", this.IsRead);
            sqlCommand.Parameters.AddWithValue("@IsWrite", this.IsWrite);
            sqlCommand.Parameters.AddWithValue("@IsRequired", this.IsRequired);
            sqlCommand.Parameters.AddWithValue("@IsPayrollTransaction", this.IsPayrollTransaction);
            sqlCommand.Parameters.AddWithValue("@IsApproval", this.IsApproval);
            //sqlCommand.Parameters.AddWithValue("@IsDelete", this.IsDelete);
            sqlCommand.Parameters.AddWithValue("@ReadMessage", this.ReadMessage);
            sqlCommand.Parameters.AddWithValue("@WriteMessage", this.WriteMessage);
            sqlCommand.Parameters.AddWithValue("@RequiredMessage", this.RequiredMessage);
            sqlCommand.Parameters.AddWithValue("@TransactionMessage", this.TransactionMessage);
            sqlCommand.Parameters.AddWithValue("@ApprovalMessage", this.ApprovalMessage);
          //  sqlCommand.Parameters.AddWithValue("@DeleteMessage", this.DeleteMessage);
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
        /// Delete the RoleFormCommand
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("RoleFormCommand_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the RoleFormCommand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        /// <param name="formCommandId"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId, int roleId, int formCommandId)
        {

            SqlCommand sqlCommand = new SqlCommand("RoleFormCommand_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@RoleId", roleId);
            sqlCommand.Parameters.AddWithValue("@FormCommandId", formCommandId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

