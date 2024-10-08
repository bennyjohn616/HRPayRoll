// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Microsoft">
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
    /// To handle the FullFinalSettlement
    /// </summary>
    public class FullFinalSettlement
    {

        #region private variable
        private FullFinalSettlementDetailList _fullFinalSettlementDetailList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FullFinalSettlement()
        {

        }

        public FullFinalSettlement(Guid id, Guid employeeId)
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyMonth"])))
                    this.ApplyMonth = Convert.ToInt32(dtValue.Rows[0]["ApplyMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyYear"])))
                    this.ApplyYear = Convert.ToInt32(dtValue.Rows[0]["ApplyYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ResignationDate"])))
                    this.ResignationDate = Convert.ToDateTime(dtValue.Rows[0]["ResignationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LastWorkingDate"])))
                    this.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[0]["LastWorkingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SettlementDate"])))
                    this.SettlementDate = Convert.ToDateTime(dtValue.Rows[0]["SettlementDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RelievingDate"])))
                    this.RelievingDate = Convert.ToDateTime(dtValue.Rows[0]["RelievingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoticePeriodToBeServed"])))
                    this.NoticePeriodToBeServed = Convert.ToInt32(dtValue.Rows[0]["NoticePeriodToBeServed"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SalaryDays"])))
                    this.SalaryDays = Convert.ToDecimal(dtValue.Rows[0]["SalaryDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LopDays"])))
                    this.LopDays = Convert.ToDecimal(dtValue.Rows[0]["LopDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MonthDays"])))
                    this.MonthDays = Convert.ToInt32(dtValue.Rows[0]["MonthDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsTax"])))
                    this.IsTax = Convert.ToBoolean(dtValue.Rows[0]["IsTax"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Notes"])))
                    this.Notes = Convert.ToString(dtValue.Rows[0]["Notes"]);
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
        /// Get or Set the ApplyMonth
        /// </summary>
        public int ApplyMonth { get; set; }
        /// <summary>
        /// Get or Set the ApplyYear
        /// </summary>
        public int ApplyYear { get; set; }
        /// <summary>
        /// Get or Set the ResignationDate
        /// </summary>
        public DateTime ResignationDate { get; set; }
        /// <summary>
        /// Get or Set the LastWorkingDate
        /// </summary>
        public DateTime LastWorkingDate { get; set; }
        /// <summary>
        /// Get or Set the SettlementDate
        /// </summary>
        public DateTime SettlementDate { get; set; }
        /// <summary>
        /// Get or Set the RelievingDate
        /// </summary>
        public DateTime RelievingDate { get; set; }
        /// <summary>
        /// Get or Set the NoticePeriodToBeServed
        /// </summary>
        public int NoticePeriodToBeServed { get; set; }
        /// <summary>
        /// Get or Set the SalaryDays
        /// </summary>
        public Decimal SalaryDays { get; set; }
        /// <summary>
        /// Get or Set the LopDays
        /// </summary>
        public Decimal LopDays { get; set; }
        /// <summary>
        /// Get or Set the MonthDays
        /// </summary>
        public int MonthDays { get; set; }
        /// <summary>
        /// Get or Set the IsTax
        /// </summary>
        public bool IsTax { get; set; }
        /// <summary>
        /// Get or Set the Notes
        /// </summary>
        public string Notes { get; set; }
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

        public FullFinalSettlementDetailList FullFinalSettlementDetailList
        {
            get
            {
                if (object.ReferenceEquals(_fullFinalSettlementDetailList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _fullFinalSettlementDetailList = new FullFinalSettlementDetailList(this.Id);
                    }
                    else
                        _fullFinalSettlementDetailList = new FullFinalSettlementDetailList();
                }
                return _fullFinalSettlementDetailList;

            }
            set
            {
                _fullFinalSettlementDetailList = value;
            }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Save the FullFinalSettlement
        /// </summary>
        /// <returns></returns>

        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlement_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId.ToString() == Guid.Empty.ToString() ? null : this.EmployeeId.ToString());
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@ResignationDate", this.ResignationDate);
            sqlCommand.Parameters.AddWithValue("@LastWorkingDate", this.LastWorkingDate);
            sqlCommand.Parameters.AddWithValue("@SettlementDate", this.SettlementDate);
            sqlCommand.Parameters.AddWithValue("@RelievingDate", this.RelievingDate);
            sqlCommand.Parameters.AddWithValue("@NoticePeriodToBeServed", this.NoticePeriodToBeServed);
            sqlCommand.Parameters.AddWithValue("@SalaryDays", this.SalaryDays);
            sqlCommand.Parameters.AddWithValue("@LopDays", this.LopDays);
            sqlCommand.Parameters.AddWithValue("@MonthDays", this.MonthDays);
            sqlCommand.Parameters.AddWithValue("@IsTax", this.IsTax);
            sqlCommand.Parameters.AddWithValue("@Notes", this.Notes);
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

        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlement_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@empId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@modifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods

        /// <summary>
        /// Select the FullFinalSettlement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        protected internal DataTable GetTableValues(Guid id, Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("FullFinalSettlement_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetEmpFandFDetails(Guid employeeId, int month, int year)
        {

            SqlCommand sqlCommand = new SqlCommand("Getemployee_detail");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@employee_id", employeeId);
            sqlCommand.Parameters.AddWithValue("@month", month);
            sqlCommand.Parameters.AddWithValue("@year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }


}