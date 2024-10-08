using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TraceError;

namespace PayrollBO
{
    public class LeaveFinanceYear
    {
        #region "Constructor"
        public LeaveFinanceYear()
        { }

        public LeaveFinanceYear(int CompanyId, bool isdefault = false)
        {

            this.CompanyId = CompanyId;
            this.IsDefault = isdefault;
            DataTable dtValue = this.GetFinanceYr();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StartMonth"])))
                    this.StartMonth = (Convert.ToDateTime(dtValue.Rows[0]["StartMonth"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EndMonth"])))
                    this.EndMonth = (Convert.ToDateTime(dtValue.Rows[0]["EndMonth"]));
                this.IsDefault = Convert.ToBoolean(dtValue.Rows[0]["IsDefault"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }

        }

        public LeaveFinanceYear(Guid id, bool isdefault=false)
        {
           
            this.Id = id;
            this.IsDefault = isdefault;
            DataTable dtValue = this.GetFinanceYear();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StartMonth"])))
                    this.StartMonth = (Convert.ToDateTime(dtValue.Rows[0]["StartMonth"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EndMonth"])))
                    this.EndMonth = (Convert.ToDateTime(dtValue.Rows[0]["EndMonth"]));
                this.IsDefault = Convert.ToBoolean(dtValue.Rows[0]["IsDefault"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }

        }

        #endregion

        #region"Properties"
        public Guid Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime EndMonth { get; set; }
        public bool IsDefault { get; set; }
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
        public bool CompOff { get; set; }
        #endregion


        #region'financeyrleaveupdation'

        public bool CheckLeavewithfinYr()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveFinyearUpdate");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinId", this.Id);
            sqlCommand.Parameters.AddWithValue("@StartMonth", this.StartMonth);
            sqlCommand.Parameters.AddWithValue("@EndMonth", this.EndMonth);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            //sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            // string outValue = string.Empty;
            try
            {
                bool status = dbOperation.DeleteData(sqlCommand);
                return status;
            }
           catch(Exception ex)
            {
                ErrorLog.Log(ex);
                return true;
            }
          
           
        }


        #endregion



        #region "Public Methods"
        /// <summary>
        /// Save the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LevFinanceYear_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@StartMonth", this.StartMonth);
            sqlCommand.Parameters.AddWithValue("@EndMonth", this.EndMonth);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDefault", this.IsDefault);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        /// 
        public DataTable finyrcheck(int companycode)
        {

            SqlCommand sqlCommand = new SqlCommand("leavefinanceyearcheck_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@From_Date", this.StartMonth);
            sqlCommand.Parameters.AddWithValue("@To_Date", this.EndMonth);
            sqlCommand.Parameters.AddWithValue("@Companyid", companycode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LevFinanceYear_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            //sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal  DataTable GetFinanceYear()
        {
            SqlCommand sqlCommand = new SqlCommand("LevFinanceYear_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@IsDefault", this.IsDefault);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetFinanceYr()
        {
            SqlCommand sqlCommand = new SqlCommand("FinanceYear_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@IsDefault", this.IsDefault);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion


        #region "fullCalendarRender"
        public Guid EmpGUid { get; set; }

        public Guid LeavetypeGUid { get; set; }

        public DateTime LeaveDate { get; set; }

        public string Leavename { get; set; }
        public int HFday { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime Todate { get; set; }

        public DataTable GetFullCalendar(Guid finid)
        {
            SqlCommand sqlCommand = new SqlCommand("FullcalendarRender_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinID", finid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        public DataTable GetFullCalendarwithfilter()
        {
            SqlCommand sqlCommand = new SqlCommand("FullcalendarRenderwithfilter_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Fromdate",this.fromdate);
            sqlCommand.Parameters.AddWithValue("@Todate",this.Todate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
        #region "Calendardaterender"
        public DataTable CalendarDate(string date, Guid leveid, Guid finid,Guid DefLOPid,Guid DefONDUTYid)
        {
            SqlCommand sqlCommand = new SqlCommand("CalendarLeaveDetail_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finid", finid);
            sqlCommand.Parameters.AddWithValue("@leavetypeid", leveid);
            sqlCommand.Parameters.AddWithValue("@date", date);
            sqlCommand.Parameters.AddWithValue("@DefLOPid", DefLOPid);
            sqlCommand.Parameters.AddWithValue("@DefONDUTYid", DefONDUTYid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

        #region "LeaveReport"

        public DataTable getemployeeleavereport(DateTime Fromdate, DateTime Todate, Guid EmpID)
        {
            SqlCommand sqlCommand = new SqlCommand("employeeleaveREPORT_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Employeeid", EmpID);
            sqlCommand.Parameters.AddWithValue("@Fromdate", Fromdate);
            sqlCommand.Parameters.AddWithValue("@Todate", Todate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion
        public DataTable getManageremployeereport(DateTime Fromdate, DateTime Todate,Guid Managerid, Guid EmpID,int leaverptstatus)
        {
            SqlCommand sqlCommand = new SqlCommand("AssignManagerReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmpID);
            sqlCommand.Parameters.AddWithValue("@FromDate", Fromdate);
            sqlCommand.Parameters.AddWithValue("@EndDate", Todate);
            sqlCommand.Parameters.AddWithValue("@LeaveStatus", leaverptstatus);
            sqlCommand.Parameters.AddWithValue("@AssignManagerId", Managerid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #region "Weekoff"

        public string weekofday { get; set; }


      
        public DataTable InactiveWeekoffdata()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

       

        #endregion

        #region "employeeDashboarddata"

        public DataTable Getemployeedashboarddata(Guid empid, Guid finid)
        {
            SqlCommand sqlCommand = new SqlCommand("EmployeeLeaveDashboard_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finid", finid);
            sqlCommand.Parameters.AddWithValue("@Empid", empid);
  
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion
        public DataTable GetemployeeLeaveBalanceReport(Guid empid, Guid finid)
        {
            SqlCommand sqlCommand = new SqlCommand("EmployeeLeaveBalanceReport_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finid", finid);
            sqlCommand.Parameters.AddWithValue("@Empid", empid);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }



        public DataTable GetCreditordebit(string Person,string type,Guid empid,Guid finid ,int compid)
        {
            SqlCommand sqlCommand = new SqlCommand("CreditandDebit_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finid", finid);
            sqlCommand.Parameters.AddWithValue("@Empid", empid);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            sqlCommand.Parameters.AddWithValue("@Person", Person);
            sqlCommand.Parameters.AddWithValue("@companyid", compid);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable HRmanagerexport(string Person, string type, Guid empid, Guid finid, int compid,string Report,Guid Emptype)
        {
            SqlCommand sqlCommand = new SqlCommand("CreditandDebit_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finid", finid);
            sqlCommand.Parameters.AddWithValue("@Empid", empid);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            sqlCommand.Parameters.AddWithValue("@Person", Person);
            sqlCommand.Parameters.AddWithValue("@companyid", compid); 
            sqlCommand.Parameters.AddWithValue("@report", Report);
            sqlCommand.Parameters.AddWithValue("@Emptype", Emptype);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }
}
