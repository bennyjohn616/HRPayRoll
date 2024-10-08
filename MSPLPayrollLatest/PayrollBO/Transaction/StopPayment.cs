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
    /// To handle the StopPayment
    /// </summary>
    public class StopPayment
    {

        #region private variable
      
        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public StopPayment()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public StopPayment(Guid id, Guid employeeId)
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPaymentMonth"])))
                    this.StopPaymentMonth = Convert.ToInt32(dtValue.Rows[0]["StopPaymentMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPaymentYear"])))
                    this.StopPaymentYear = Convert.ToInt32(dtValue.Rows[0]["StopPaymentYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPaymentType"])))
                    this.StopPaymentType = Convert.ToInt32(dtValue.Rows[0]["StopPaymentType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Remarks"])))
                    this.Remarks = Convert.ToString(dtValue.Rows[0]["Remarks"]);
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
        /// Get or Set the StopPaymentMonth
        /// </summary>
        public int StopPaymentMonth { get; set; }

        /// <summary>
        /// Get or Set the StopPaymentYear
        /// </summary>
        public int StopPaymentYear { get; set; }

        /// <summary>
        /// Get or Set the StopPaymentType
        /// </summary>
        public int StopPaymentType { get; set; }

        /// <summary>
        /// Get or Set the Remarks
        /// </summary>
        public string Remarks { get; set; }

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

       
      
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("StopPayment_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@StopPaymentMonth", this.StopPaymentMonth);
            sqlCommand.Parameters.AddWithValue("@StopPaymentYear", this.StopPaymentYear);
            sqlCommand.Parameters.AddWithValue("@StopPaymentType", this.StopPaymentType);
            sqlCommand.Parameters.AddWithValue("@Remarks", this.Remarks);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
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
        /// Delete the StopPayment
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("StopPayment_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the StopPayment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("StopPayment_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}


