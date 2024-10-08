using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class MonthlyLeaveLimit
    {
        #region Constructor
        public MonthlyLeaveLimit()
        {

        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public int CompanyId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public double MaxDays { get; set; }

        public double CrDays { get; set; }
        public string LeaveType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        public List<MonthlyLeaveLimit> GetMonthlyLeaveLimit(int companyId)
        {
            this.CompanyId = companyId;
            DataTable dt= GetTableValues();

            List<MonthlyLeaveLimit> MonthlyLeaveLimitList = new List<MonthlyLeaveLimit>();
            MonthlyLeaveLimitList = (from DataRow dr in dt.Rows
                           select new MonthlyLeaveLimit()
                           {
                               Id = new Guid(Convert.ToString(dr["Id"].ToString())),
                               LeaveType = dr["LeaveType"].ToString(),
                               MaxDays =Convert.ToDouble(dr["MaxDays"].ToString()) ,
                               CrDays = Convert.ToDouble(dr["CrDays"].ToString()),
                               LeaveTypeId =new Guid(Convert.ToString(dr["LeaveTypeId"].ToString()))                             
                           }).ToList();
            return MonthlyLeaveLimitList;

        }

        public  bool SaveMonthlyLeaveLimit()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_MonthlyLeaveLimit");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@MaxDays", this.MaxDays);
            sqlCommand.Parameters.AddWithValue("@CrDays", this.CrDays); 
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@Type", "ADD");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);           
            return status;
        }

        public bool DeleteMonthlyLeaveLimit()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_MonthlyLeaveLimit");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);           
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);           
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@Type", "DELETE");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        public DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_MonthlyLeaveLimit");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue= dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }

    }
}
