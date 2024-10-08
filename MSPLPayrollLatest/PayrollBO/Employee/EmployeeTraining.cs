// -----------------------------------------------------------------------
// <copyright file="EmployeeTraining.cs" company="Microsoft">
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
    /// To handle the EmployeeTraining
    /// </summary>
    public class EmployeeTraining
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeTraining()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="id"></param>
        public EmployeeTraining(Guid employeeId, Guid id)
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
                this.TrainingName = Convert.ToString(dtValue.Rows[0]["TrainingName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TrainingDate"])))
                    this.TrainingDate = Convert.ToDateTime(dtValue.Rows[0]["TrainingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TrainingTo"])))
                    this.TrainingTo = Convert.ToDateTime(dtValue.Rows[0]["TrainingTo"]);
                this.CertificateNumber = Convert.ToString(dtValue.Rows[0]["CertificateNumber"]);
                this.Institute = Convert.ToString(dtValue.Rows[0]["Institute"]);
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
        /// Get or Set the TrainingName
        /// </summary>
        public string TrainingName { get; set; }

        /// <summary>
        /// Get or Set the TrainingDate
        /// </summary>
        public DateTime TrainingDate { get; set; }

        /// <summary>
        /// Get or Set the TrainingTo
        /// </summary>
        public DateTime TrainingTo { get; set; }

        /// <summary>
        /// Get or Set the CertificateNumber
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// Get or Set the Institute
        /// </summary>
        public string Institute { get; set; }

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
        /// Save the Emp_Training
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Training_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@TrainingName", this.TrainingName);
            sqlCommand.Parameters.AddWithValue("@TrainingDate", this.TrainingDate);
            sqlCommand.Parameters.AddWithValue("@TrainingTo", this.TrainingTo);
            sqlCommand.Parameters.AddWithValue("@CertificateNumber", this.CertificateNumber);
            sqlCommand.Parameters.AddWithValue("@Institute", this.Institute);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@ImportOption", this.ImportOption);
            sqlCommand.Parameters.AddWithValue("@Query", this.Query);
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
        /// Delete the Emp_Training
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Training_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Training
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid employeeId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Training_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

