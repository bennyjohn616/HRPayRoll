// -----------------------------------------------------------------------
// <copyright file="EmployeeEmployeement.cs" company="Microsoft">
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
    /// To handle the EmployeeEmployeement
    /// </summary>
    public class EmployeeEmployeement
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeEmployeement()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="id"></param>
        public EmployeeEmployeement(Guid employeeId, Guid id)
        {
            this.Id = id;
            this.EmployeeId = employeeId;
            DataTable dtValue = this.GetTableValues(this.EmployeeId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                this.PositionHeld = Convert.ToString(dtValue.Rows[0]["PositionHeld"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["WorkFrom"])))
                    this.WorkFrom = Convert.ToDateTime(dtValue.Rows[0]["WorkFrom"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["WorkTo"])))
                    this.WorkTo = Convert.ToDateTime(dtValue.Rows[0]["WorkTo"]);
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
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Get or Set the CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Get or Set the PositionHeld
        /// </summary>
        public string PositionHeld { get; set; }

        /// <summary>
        /// Get or Set the WorkFrom
        /// </summary>
        public DateTime WorkFrom { get; set; }

        /// <summary>
        /// Get or Set the WorkTo
        /// </summary>
        public DateTime WorkTo { get; set; }

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
        public string ImportOption { get; set; }

        public string Query { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the Emp_Employeement
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Employeement_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", this.EmployeeCode);
            sqlCommand.Parameters.AddWithValue("@CompanyName", this.CompanyName);
            sqlCommand.Parameters.AddWithValue("@PositionHeld", this.PositionHeld);
            sqlCommand.Parameters.AddWithValue("@WorkFrom", this.WorkFrom);
            sqlCommand.Parameters.AddWithValue("@WorkTo", this.WorkTo);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");

            if (ImportOption == "EmployeeEmployeement")
            {
                string Query = string.Empty;
                string Query1 = string.Empty;
                Query = "update Emp_Address set ";
                Query = Query + " [EmployeeId] =  '" + this.EmployeeId.ToString() + "'";
                if (!string.IsNullOrEmpty(this.EmployeeCode.ToString()))
                    Query = Query + " ,[EmployeeCode] =  '" + this.EmployeeCode.ToString() + "'";
                if (!string.IsNullOrEmpty(this.CompanyName.ToString()))
                    Query = Query + " ,[CompanyName] =  '" + this.CompanyName.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PositionHeld.ToString()))
                    Query = Query + " ,[PositionHeld] =  '" + this.PositionHeld.ToString() + "'";
                if (!string.IsNullOrEmpty(this.WorkFrom.ToString()))
                    Query = Query + " ,[WorkFrom] =  '" + this.WorkFrom.ToString() + "'";
                if (!string.IsNullOrEmpty(this.WorkTo.ToString()))
                    Query = Query + " ,[WorkTo] =  '" + this.WorkTo.ToString() + "'";
                if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
                    Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";
                Query = Query + " ,[ModifiedOn] = GETDATE()";
                Query = Query + "  Where  Id ='" + this.Id.ToString() + "'";
            }
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Emp_Employeement
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Employeement_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Employeement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Employeement_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

