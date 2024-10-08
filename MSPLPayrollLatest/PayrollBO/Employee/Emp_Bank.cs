// -----------------------------------------------------------------------
// <copyright file="Emp_Bank.cs" company="Microsoft">
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
    /// To handle the Emp_Bank
    /// </summary>
    public class Emp_Bank
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Emp_Bank()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Emp_Bank(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BankId"])))
                    this.BankId = new Guid(Convert.ToString(dtValue.Rows[0]["BankId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.AcctNo = Convert.ToString(dtValue.Rows[0]["AcctNo"]);
                this.IFSC = Convert.ToString(dtValue.Rows[0]["IFSC"]);
                this.BranchName = Convert.ToString(dtValue.Rows[0]["BranchName"]);
                this.Address = Convert.ToString(dtValue.Rows[0]["Address"]);
                this.City = Convert.ToString(dtValue.Rows[0]["City"]);
                this.State = Convert.ToString(dtValue.Rows[0]["State"]);
                this.CreatedBy = Convert.ToString(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                this.ModifiedBy = Convert.ToString(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the BankId
        /// </summary>
        public Guid BankId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        ///// <summary>
        ///// Get or Set the Bank Name
        ///// </summary>
        //public string BankName { get; set; }
        /// <summary>
        /// Get or Set the AcctNo
        /// </summary>
        public string AcctNo { get; set; }

        /// <summary>
        /// Get or Set the IFSC
        /// </summary>
        public string IFSC { get; set; }

        /// <summary>
        /// Get or Set the BranchName
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Get or Set the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get or Set the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or Set the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        public string ImportOption { get; set; }

        public string Query { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the Emp_Bank
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Bank_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@BankId", this.BankId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@AcctNo", this.AcctNo);
            sqlCommand.Parameters.AddWithValue("@IFSC", this.IFSC);
            sqlCommand.Parameters.AddWithValue("@BranchName", this.BranchName);
            sqlCommand.Parameters.AddWithValue("@Address", this.Address);
            sqlCommand.Parameters.AddWithValue("@City", this.City);
            sqlCommand.Parameters.AddWithValue("@State", this.State);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
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
        /// Delete the Emp_Bank
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Bank_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Bank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Bank_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@employeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(String Id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Bank_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Filter Emp_Bank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, string filterExpr)
        {

            string query = " SELECT [Id],[BankId],[EmployeeId],[AcctNo],[IFSC],[BranchName],[Address],[City],[State],[CreatedBy],[CreatedOn],"
                          + " [ModifiedBy],[ModifiedOn],[IsActive] FROM Emp_Bank WHERE[EmployeeId] = '" + employeeId + "' " + filterExpr;
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

