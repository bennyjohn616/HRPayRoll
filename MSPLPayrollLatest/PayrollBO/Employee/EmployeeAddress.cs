// -----------------------------------------------------------------------
// <copyright file="Emp_Address.cs" company="Microsoft">
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
    /// To handle the EmployeeAddress
    /// </summary>
    public class EmployeeAddress
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeAddress()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EmployeeAddress(Guid employeeId, Guid id)
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
                this.AddressLine1 = Convert.ToString(dtValue.Rows[0]["AddressLine1"]);
                this.AddressLine2 = Convert.ToString(dtValue.Rows[0]["AddressLine2"]);
                this.City = Convert.ToString(dtValue.Rows[0]["City"]);
                this.State = Convert.ToString(dtValue.Rows[0]["State"]);
                this.Country = Convert.ToString(dtValue.Rows[0]["Country"]);
                this.PinCode = Convert.ToString(dtValue.Rows[0]["PinCode"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AddressType"])))
                    this.AddressType = Convert.ToInt32(dtValue.Rows[0]["AddressType"]);
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
        /// Get or Set the AddressLine1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Get or Set the AddressLine2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Get or Set the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or Set the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Set the Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Get or Set the PinCode
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Get or Set the Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Get or Set the AddressType
        /// </summary>
        public int AddressType { get; set; }

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
        /// Save the Emp_Address
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (this.AddressType == 2)
                this.AddressLine1 = this.AddressLine1 == null ? "" : this.AddressLine1;

            SqlCommand sqlCommand = new SqlCommand("Emp_Address_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@AddressLine1", this.AddressLine1);
            sqlCommand.Parameters.AddWithValue("@AddressLine2", this.AddressLine2);
            sqlCommand.Parameters.AddWithValue("@City", this.City);
            sqlCommand.Parameters.AddWithValue("@State", this.State);
            sqlCommand.Parameters.AddWithValue("@Country", this.Country);
            sqlCommand.Parameters.AddWithValue("@PinCode", this.PinCode);
            sqlCommand.Parameters.AddWithValue("@Phone", this.Phone);
            sqlCommand.Parameters.AddWithValue("@AddressType", this.AddressType);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");

            //if (ImportOption == "EmployeeAddress")
            //{
            //    string Query = string.Empty;
            //    string Query1 = string.Empty;
            //    Query = "update Emp_Address set ";
            //    Query = Query + " [EmployeeId] =  '" + this.EmployeeId.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.AddressLine1.ToString()))
            //        Query = Query + " ,[AddressLine1] =  '" + this.AddressLine1.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.AddressLine2.ToString()))
            //        Query = Query + " ,[AddressLine2] =  '" + this.AddressLine2.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.City.ToString()))
            //        Query = Query + " ,[City] =  '" + this.City.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.State.ToString()))
            //        Query = Query + " ,[State] =  '" + this.State.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Country.ToString()))
            //        Query = Query + " ,[Country] =  '" + this.Country.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.PinCode.ToString()))
            //        Query = Query + " ,[PinCode] =  '" + this.PinCode.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Phone.ToString()))
            //        Query = Query + " ,[Phone] =  '" + this.Phone.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.AddressType.ToString()))
            //        Query = Query + " ,[AddressType] =  '" + this.AddressType.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
            //        Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";
            //    Query = Query + " ,[ModifiedOn] = GETDATE()";
            //    Query = Query + "  Where  Id ='" + this.Id.ToString() + "'";
            //}

            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Emp_Address
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Address_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Address
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Address_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(String Id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Address_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Emp_Address Filter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id, string filterExpr)
        {
            string query = "SELECT [Id],[EmployeeId],[AddressLine1],[AddressLine2],[City],[State],[Country],[PinCode],[Phone],[AddressType],[CreatedBy],[CreatedOn]"
                         + " ,[ModifiedBy],[ModifiedOn],[IsDeleted] FROM Emp_Address AS addres WHERE IsDeleted = 0 AND addres.[Id] = CASE '" + id
                          + "' WHEN CAST(0x0 AS UNIQUEIDENTIFIER) THEN addres.[Id] ELSE '" + id + "' END AND addres.EmployeeId = '" + employeeId + "' " + filterExpr;
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

