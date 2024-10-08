using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace PayrollBO.Leave
{
     public class LeaveCreditSettingsBO
    {



        #region Constructor
        public LeaveCreditSettingsBO()
        {

        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public int CompanyId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public Guid CategoryTypeId { get; set; }
        public Guid FinyearId { get; set; }
        public string CategoryType { get; set; }
        public DateTime Effectivedate { get; set; }
        public int Rotationdays { get; set; }
        public double CrDays { get; set; }
        public int midmontdate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime LastProcessDate { get; set; }
        public DateTime NEXTProcessDate { get; set; }
        public string Monthflag { get; set; }
        public string LeaveCreditType { get; set; }

        public string leaveCreditAffect { get; set; }
        #endregion

        public List<LeaveCreditSettingsBO> GetCreditLeavesettings(int companyId)
        {
            this.CompanyId = companyId;
            DataTable dt = GetTableValues();

            List<LeaveCreditSettingsBO> CreditLeaveSettingsList = new List<LeaveCreditSettingsBO>();
            CreditLeaveSettingsList = (from DataRow dr in dt.Rows
                                     select new LeaveCreditSettingsBO()
                                     {
                                         Id = new Guid(Convert.ToString(dr["Id"].ToString())),
                                         LeaveType = dr["LeaveType"].ToString(),
                                         LeaveTypeId = new Guid(Convert.ToString(dr["LeaveTypeId"].ToString())),
                                         CategoryType = dr["CategoryType"].ToString(),
                                         CategoryTypeId = new Guid(Convert.ToString(dr["CategoryTypeId"].ToString())),
                                         Rotationdays = Convert.ToInt32(dr["Rotationdays"].ToString()),
                                         CrDays = Convert.ToDouble(dr["CrDays"].ToString()),
                                         Effectivedate=Convert.ToDateTime(dr["Effectivedate"].ToString()),
                                         midmontdate = Convert.ToInt32(dr["midmontdate"].ToString()),
                                         Monthflag= dr["Monthflag"].ToString(),

                                     }).ToList();
            return CreditLeaveSettingsList;

        }
        public List<LeaveCreditSettingsBO> LoadCreditcategory(int companyId)
        {
            this.CompanyId = companyId;
            DataTable dt = LoadcategoryforCredit();

            List<LeaveCreditSettingsBO> CreditLeaveSettingsList = new List<LeaveCreditSettingsBO>();
            CreditLeaveSettingsList = (from DataRow dr in dt.Rows
                                       select new LeaveCreditSettingsBO()
                                       {
                                           CategoryType = dr["CategoryType"].ToString(),
                                           CategoryTypeId = new Guid(Convert.ToString(dr["CategoryTypeId"].ToString())),
                                       }).ToList();
            return CreditLeaveSettingsList;

        }
        public List<LeaveCreditSettingsBO> LoadCreditLeavetype(int companyId,Guid catid)
        {
            this.CompanyId = companyId;
            DataTable dt = LoadleaveforCredit(catid);

            List<LeaveCreditSettingsBO> CreditLeaveSettingsList = new List<LeaveCreditSettingsBO>();
            CreditLeaveSettingsList = (from DataRow dr in dt.Rows
                                       select new LeaveCreditSettingsBO()
                                       {
                                           LeaveType = dr["LeaveType"].ToString(),
                                           LeaveTypeId = new Guid(Convert.ToString(dr["LeaveTypeId"].ToString())),
                                       }).ToList();
            return CreditLeaveSettingsList;

        }
        public List<LeaveCreditSettingsBO> LoadCreditLeaveDates(int companyId, Guid catid,Guid levid)
        {
            this.CompanyId = companyId;
            DataTable dt = getdateforlevcredits(companyId, catid, levid);

            List<LeaveCreditSettingsBO> CreditLeaveSettingsList = new List<LeaveCreditSettingsBO>();
            CreditLeaveSettingsList = (from DataRow dr in dt.Rows
                                       select new LeaveCreditSettingsBO()
                                       {
                                           LastProcessDate = Convert.ToDateTime(dr["EffectiveDate"].ToString()),
                                           NEXTProcessDate = Convert.ToDateTime(dr["nextCreditProcessDate"].ToString()),
                                       }).ToList();
            return CreditLeaveSettingsList;

        }
        public bool SaveCreditLeaveSettings()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            //sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@CategoryTypeId", this.CategoryTypeId);
            //sqlCommand.Parameters.AddWithValue("@CategoryType", this.CategoryType);
            sqlCommand.Parameters.AddWithValue("@Rotationdays", this.Rotationdays);
            sqlCommand.Parameters.AddWithValue("@CrDays", this.CrDays);
            sqlCommand.Parameters.AddWithValue("@Effectivedate", this.Effectivedate);
            sqlCommand.Parameters.AddWithValue("@midmontdate", this.midmontdate);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@Monthflag", this.Monthflag);
            sqlCommand.Parameters.AddWithValue("@finyear", this.FinyearId);
            sqlCommand.Parameters.AddWithValue("@LeaveCreditType", this.LeaveCreditType);
            sqlCommand.Parameters.AddWithValue("@LeaveCreditAffect", this.leaveCreditAffect);
            sqlCommand.Parameters.AddWithValue("@Type", "ADD");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        
        public bool DeleteMonthlyLeaveLimit()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
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
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable GetSinglevalue()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CategoryTypeId", this.CategoryTypeId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@Type", "SINGLE");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable LoadcategoryforCredit()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Type", "CATEGORY");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable LoadleaveforCredit(Guid catid)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CategoryTypeId", catid);
            sqlCommand.Parameters.AddWithValue("@Type", "LEAVE");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable getdateforlevcredits(int compid,Guid catid,Guid levid)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CategoryTypeId", catid);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", levid);
            sqlCommand.Parameters.AddWithValue("@Type", "DATES");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public  bool CreditProcessSave(DateTime LastCreditProcessDate, DateTime ProcessedDate)
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveCreditProcess_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryTypeId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@LastCreditProcessDate", LastCreditProcessDate);
            sqlCommand.Parameters.AddWithValue("@ProcessedDate", ProcessedDate);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            //sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool CreditProcessing(Guid employeeid)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Creditleavesettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@finyear", this.FinyearId);
            //sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.CategoryTypeId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@EMPId", employeeid);
            sqlCommand.Parameters.AddWithValue("@CrDays", this.CrDays);
            sqlCommand.Parameters.AddWithValue("@Type", "PROCESS");
            sqlCommand.Parameters.AddWithValue("ModifiedBy", this.CreatedBy);
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

    }
}
