using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class CreditDays
    {
        #region "Private Variables"
        private Employee _employee;
        #endregion

        #region"Constructor"

        public CreditDays()
        {

        }

        public CreditDays(int id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyMonth"])))
                    this.ApplyMonth = Convert.ToInt32(dtValue.Rows[0]["ApplyMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyYear"])))
                    this.ApplyYear = Convert.ToInt32(dtValue.Rows[0]["ApplyYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsProcessed"])))
                    this.IsProcessed = Convert.ToBoolean(dtValue.Rows[0]["IsProcessed"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Type"])))
                    this.CType = Convert.ToString(dtValue.Rows[0]["Type"]);

            }

        }


        #endregion

        #region "Properties"

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Guid EmployeeId { get; set; }
        public string CType { get; set; }

        public int ApplyMonth { get; set; }
        public int ApplyYear { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int PaidDays { get; set; }

        public int LopDays { get; set; }
       
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsProcessed { get; set; }
        public string ProPayroll { get; set; }

        public Employee Employee
        {
            get
            {
                if (object.ReferenceEquals(_employee, null))
                {
                    if (this.EmployeeId != Guid.Empty)
                    {
                        _employee = new Employee(this.CompanyId, this.EmployeeId);
                    }
                    else
                        _employee = new Employee();

                }
                return _employee;

            }
            set
            {
                _employee = value;
            }
        }
        #endregion

        #region "Public Methods"
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("CreditDaysEntry_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@PaidDays", this.PaidDays);
            sqlCommand.Parameters.AddWithValue("@LopDays", this.LopDays);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@Type", this.CType);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", false);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsProcessed", this.IsProcessed);
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
        /// Delete the StopPayment
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("CreditDaysEntry_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region "Protected Methods"

        protected internal DataTable GetTableValues()
        {
            if (string.IsNullOrEmpty(this.ProPayroll))
                this.ProPayroll = "";
            SqlCommand sqlCommand = new SqlCommand("CreditDaysEntry_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Type", this.CType==null?"": this.CType);
            sqlCommand.Parameters.AddWithValue("@ApplyMonth", this.ApplyMonth);
            sqlCommand.Parameters.AddWithValue("@ApplyYear", this.ApplyYear);
            sqlCommand.Parameters.AddWithValue("@ProPayroll", this.ProPayroll);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
