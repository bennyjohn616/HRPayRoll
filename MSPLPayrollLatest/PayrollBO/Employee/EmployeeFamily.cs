// -----------------------------------------------------------------------
// <copyright file="EmployeeFamily.cs" company="Microsoft">
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
    /// To handle the Emp_Family
    /// </summary>
    public class EmployeeFamily
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeFamily()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EmployeeFamily(Guid employeeId, Guid id)
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
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.Address = Convert.ToString(dtValue.Rows[0]["Address"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RelationShip"])))
                    this.RelationShip = Convert.ToInt32(dtValue.Rows[0]["RelationShip"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfBirth"])))
                    this.DateOfBirth = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Age"])))
                    this.Age = Convert.ToInt32(dtValue.Rows[0]["Age"]);
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
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get or Set the RelationShip
        /// </summary>
        public int RelationShip { get; set; }

        /// <summary>
        /// Get or Set the DateOfBirth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Get or Set the Age
        /// </summary>
        public int Age { get; set; }

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
        /// Save the Emp_Family
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Family_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@Address", this.Address);
            sqlCommand.Parameters.AddWithValue("@RelationShip", this.RelationShip);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", this.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@Age", this.Age);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");

            //if (ImportOption == "EmployeeFamily")
            //{
            //    string Query = string.Empty;
            //    string Query1 = string.Empty;
            //    Query = "update Emp_Family set ";
            //    Query = Query + " [EmployeeId] =  '" + this.EmployeeId.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Name))
            //        Query = Query + " ,[Name] =  '" + this.Name.ToString() + "'";

            //    if (!string.IsNullOrEmpty(this.RelationShip.ToString()))
            //        Query = Query + " ,[RelationShip] =  '" + this.RelationShip.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Age.ToString()))
            //        Query = Query + " ,[Age] =  '" + this.Age.ToString() + "'";
            //    if (this.DateOfBirth.ToString() != DateTime.MinValue.ToString())
            //    {
            //        Query = Query + " ,[DateOfBirth] = dbo.Datetimechk( '" + this.DateOfBirth.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    if (!string.IsNullOrEmpty(this.Address))
            //        Query = Query + " ,[Address] =  '" + this.Address.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
            //        Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";
            //    Query = Query + " ,[ModifiedOn] = GETDATE()";
            //    Query = Query + "  Where Id = '" + this.Id.ToString() + "'";
            //}
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Emp_Family
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Family_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Family
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Family_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

