using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO.Leave
{
    public class CompOffBO
    {
        public Guid Id { get; set; }
        public Guid CompOffGainId { get; set; }
        public Guid FinYrId { get; set; }
        public Guid EmpId { get; set; }
        public Guid LeaveReqId { get; set; }
        public decimal AvaliableDays { get; set; }
        public bool CompoffleaveMatchingsave()
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffLeaveTracking");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@LeaveReqId", this.LeaveReqId);
            sqlCommand.Parameters.AddWithValue("@CompOffGainId", this.CompOffGainId);
            sqlCommand.Parameters.AddWithValue("@UsedLeave", this.AvaliableDays);
            sqlCommand.Parameters.AddWithValue("@Type", "INSERT");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public int UserId { get; set; }

        public bool SaveCompoffsettings(Guid compoffParameter, int days, DateTime date, string type, int compid, int userid, Guid finyr)
        {

            SqlCommand sqlCommand = new SqlCommand("SP_CompoffRequest");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            sqlCommand.Parameters.AddWithValue("@compOffParameter", compoffParameter);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", userid);
            sqlCommand.Parameters.AddWithValue("@CSLastdate", date);
            sqlCommand.Parameters.AddWithValue("@CSValidType", type);
            sqlCommand.Parameters.AddWithValue("@CSvalidDays", days);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finyr);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveSettings");

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            return dbOperation.save(sqlCommand);
        }
        public DataTable SelectCompoffsettings(int companycode, Guid compoffParameter, Guid finYear)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_CompoffRequest");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", companycode);
            sqlCommand.Parameters.AddWithValue("@Type", "SettingSelect");
            sqlCommand.Parameters.AddWithValue("@compoffParameter", compoffParameter);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        public bool CompOffsettingsDelete()
        {

            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.UserId);
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Type", "DELETE");

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            return dbOperation.save(sqlCommand);
        }
        public DataTable GetCompOffLeaveTracking(Guid FinYrId, Guid EmpId, Guid LoginEmpId, string Type="")
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffLeaveTracking");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinYrId", FinYrId);
            sqlCommand.Parameters.AddWithValue("@EmpId", EmpId);
            if (string.IsNullOrEmpty(Type) || Type == "Employee")
                sqlCommand.Parameters.AddWithValue("@Type", "GETCOMPOFFLEAVEHISTORY");
            else if (Type == "Manager" && EmpId !=new Guid("11111111-1111-1111-1111-111111111111") && EmpId != new Guid("22222222-2222-2222-2222-222222222222"))    
            {
                sqlCommand.Parameters.AddWithValue("@AssEmpId", LoginEmpId);
                sqlCommand.Parameters.AddWithValue("@Type", "MANAGERGETUNDEREMPLOYEES");
            }
            else if (Type == "Manager" && EmpId == new Guid("11111111-1111-1111-1111-111111111111"))
            {
                sqlCommand.Parameters.AddWithValue("@AssEmpId", LoginEmpId);
                sqlCommand.Parameters.AddWithValue("@Type", "MANAGER_UNDER_ACTIVE_EMPLOYEES");
            }
            else if (Type == "Manager" && EmpId == new Guid("22222222-2222-2222-2222-222222222222"))
            {
                sqlCommand.Parameters.AddWithValue("@AssEmpId", LoginEmpId);
                sqlCommand.Parameters.AddWithValue("@Type", "MANAGER_UNDER_INACTIVE_EMPLOYEES");
            }
            else if (Type == "HR" && EmpId != new Guid("11111111-1111-1111-1111-111111111111") && EmpId != new Guid("22222222-2222-2222-2222-222222222222"))
                sqlCommand.Parameters.AddWithValue("@Type", "HRGETAllEMPLOYEES");

            else if (Type == "HR" && EmpId == new Guid("22222222-2222-2222-2222-222222222222"))
            {
                sqlCommand.Parameters.AddWithValue("@AssEmpId", LoginEmpId);
                sqlCommand.Parameters.AddWithValue("@Type", "HR_INACTIVE_EMPLOYEES");
            }
            else if (Type == "HR" && EmpId == new Guid("11111111-1111-1111-1111-111111111111"))
            {
                sqlCommand.Parameters.AddWithValue("@AssEmpId", LoginEmpId);
                sqlCommand.Parameters.AddWithValue("@Type", "HR_ACTIVE_EMPLOYEES");
            }
          

            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}
