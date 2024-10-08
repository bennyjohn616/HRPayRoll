// -----------------------------------------------------------------------
// <copyright file="Increment.cs" company="Microsoft">
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
    /// To handle the Increment
    /// </summary>
    public class Increment
    {

        #region private variable

        private IncrementDetailList _incrementDetailList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Increment()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Increment(Guid id, Guid employeeId)
        {
            this.Id = id;
            this.EmployeeId = employeeId;
            DataTable dtValue = this.GetTableValues(this.Id, this.EmployeeId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveDate"])))
                    this.EffectiveDate = Convert.ToDateTime(dtValue.Rows[0]["EffectiveDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BeforeLop"])))
                    this.BeforeLop = Convert.ToDouble(dtValue.Rows[0]["BeforeLop"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AfterLop"])))
                    this.AfterLop = Convert.ToDouble(dtValue.Rows[0]["AfterLop"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyMonth"])))
                    this.ApplyMonth = Convert.ToInt32(dtValue.Rows[0]["ApplyMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyYear"])))
                    this.ApplyYear = Convert.ToInt32(dtValue.Rows[0]["ApplyYear"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsProcessed"])))
                    this.IsProcessed = Convert.ToBoolean(dtValue.Rows[0]["IsProcessed"]);
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
        /// Get or Set the EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        public double BeforeLop { get; set; }

        public double AfterLop { get; set; }

        /// <summary>
        /// Get or Set the ApplyMonth
        /// </summary>
        public int ApplyMonth { get; set; }

        /// <summary>
        /// Get or Set the ApplyYear
        /// </summary>
        public int ApplyYear { get; set; }

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

        /// <summary>
        /// get or set the IsProcessed
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Get or Set the ProcessFlag
        /// </summary>
        public string ProcessFlag { get; set; }

        public IncrementDetailList IncrementDetailList
        {
            get
            {
                if (object.ReferenceEquals(_incrementDetailList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _incrementDetailList = new IncrementDetailList(this.Id);
                    }
                    else
                        _incrementDetailList = new IncrementDetailList();
                }
                return _incrementDetailList;

            }
            set
            {
                _incrementDetailList = value;
            }
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the Increment
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Increment_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.EffectiveDate);
            sqlCommand.Parameters.AddWithValue("@BeforeLop", this.BeforeLop);
            sqlCommand.Parameters.AddWithValue("@AfterLop", this.AfterLop);
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsProcessed", this.IsProcessed);
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
        /// Delete the Increment
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("Increment_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@ProcessFlag", this.ProcessFlag);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Increment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid employeeId)
        {
            SqlCommand sqlCommand = new SqlCommand("Increment_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(int month,int year)
        {
            SqlCommand sqlCommand = new SqlCommand("Increment_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@month", month);
            sqlCommand.Parameters.AddWithValue("@year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public  DataTable CheckIncrementDel()
        {
            SqlCommand sqlCommand = new SqlCommand("Increment_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@ProcessFlag", this.ProcessFlag);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

