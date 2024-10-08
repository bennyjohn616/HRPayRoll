using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
   public class DebitLeave
    {

        #region "Properties"
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid FinanceYearId { get; set; }
        public int CompanyId { get; set; }
        public DateTime DebitLeaveEntryDate { get; set; }

        public Guid DebitLevType { get; set; }
        public string LeaveType { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        #endregion

        #region "Public Methods"
        /// <summary>
        /// Save the DebitLeave
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("DebitLeave_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@DebitLevEntryDate", this.DebitLeaveEntryDate);
            sqlCommand.Parameters.AddWithValue("@DebitLevType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@NoOfDays", this.NoOfDays);
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

        public DataTable GetDebitLeave()
        {
            SqlCommand sqlCommand = new SqlCommand("DebitLeave_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
       
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("DebitLeave_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }








        #endregion


    }
}
