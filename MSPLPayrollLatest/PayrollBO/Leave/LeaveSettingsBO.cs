using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PayrollBO.Leave
{
   public class LeaveSettingsBO
    {

        public LeaveSettingsBO()
        {

        }
        #region Properties
        public Guid Id { get; set; }
        public Guid WeekoffTempId { get; set; }
        public int CompanyId { get; set; }
        public Guid FinyrId { get; set; }
        public string name { get; set; }
        public string leaveparameter { get; set; }
        public string Holidayparameter { get; set; }
        public string Compoffparameter { get; set; }
        public string Weekoffparameter { get; set; }
        public string Weekoffentryvalid { get; set; }
        public string RpComp1 { get; set; }
        public string RpComp2 { get; set; }
        public string RpComp3 { get; set; }
        public string RpComp4 { get; set; }
        public string RpComp5 { get; set; }
        public string Createdby { get; set; }
        public string dynamicvalue { get; set; }
        public DateTime FinYearStart { get; set; }
        public DateTime FinYearEnd { get; set; }
        public string LeaveTypeDesc { get; set; }
        public string LevTypeActive { get; set; }

        public string Minormaxparameter { get; set; }
        public decimal mindays { get; set; }
        public decimal maxmintimes { get; set; }
        public string minperiod { get; set; }
        public string mindeviation { get; set; } 
        public decimal maxdays { get; set; }
        public decimal maxmaxtimes { get; set; }
        public string  maxperiod { get; set; }
        public string maxdeviation { get; set; }
        public string DaysName { get; set; }
        public string Week1 { get; set; }
        public string Week2 { get; set; }
        public string Week3 { get; set; }
        public string Week4 { get; set; }
        public string Week5 { get; set; }
        public string LevEncashAvail { get; set; }
        //LeaveEncashment
        public Guid Encashcomponent { get; set; }
        public decimal EncashLimit { get; set; }

        //LeaveConfiguration Properties
        public Guid LeaveTypeId { get; set; }
        public string DynamicComponentName { get; set; }
        public Guid DynamicComponentValue { get; set; }
        public DateTime ConfigEffectiveDate { get; set; }
        public decimal MaxDayMonth { get; set; }
        public string AllowDevisionMonth { get; set; }
        public string LevopenReq { get; set; }
        public decimal overallMax { get; set; }
        public decimal carryLimit { get; set; }
        public string InvHoliday { get; set; }
        public string InvHolidaysubparameter { get; set; }
        public decimal MinatTime { get; set; }
        public decimal MaxatTime { get; set; }
        public string Compoffallow { get; set; }
        public Guid Openingbal { get; set; }
        public Guid avalbal { get; set; }
        public Guid usedlev { get; set; }
        public string Isattachreq { get; set; }
        public decimal Attachreqmaxdays { get; set; }

        public string OpeningbalName { get; set; }
        public string avalbalName { get; set; }
        public string usedlevName { get; set; }
        public Guid UsedLeaveId { get; set; }
        public DateTime dates { get; set; }
        public string datesname { get; set; }
        public string Weekoffset { get; set; }
        
        public string weekofftype { get; set; }

        public string leavecreditparameter { get; set; }
        public string encashmentparameter { get; set; }

        public bool btnSaveEnable { get; set; }
        #endregion

        public bool Saveleavesettingmaster()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@leaveparameter", this.leaveparameter);
            sqlCommand.Parameters.AddWithValue("@Holidayparameter", this.Holidayparameter);
            sqlCommand.Parameters.AddWithValue("@Compoffparameter", this.Compoffparameter);
            sqlCommand.Parameters.AddWithValue("@Weekoffparameter", this.Weekoffparameter);
            sqlCommand.Parameters.AddWithValue("@Weekoffentryvalid", this.Weekoffentryvalid);
            sqlCommand.Parameters.AddWithValue("@minmaxparameter", this.Minormaxparameter);
            sqlCommand.Parameters.AddWithValue("@maxdays", this.maxdays);
            sqlCommand.Parameters.AddWithValue("@Maximummaxtimes", this.maxmaxtimes);
            sqlCommand.Parameters.AddWithValue("@maxperiod", this.maxperiod);
            sqlCommand.Parameters.AddWithValue("@maxdeviation", this.maxdeviation);
            sqlCommand.Parameters.AddWithValue("@mindays", this.mindays);
            sqlCommand.Parameters.AddWithValue("@Maximummintimes", this.maxmintimes);
            sqlCommand.Parameters.AddWithValue("@minperiod", this.minperiod);
            sqlCommand.Parameters.AddWithValue("@mindeviation", this.mindeviation);
            sqlCommand.Parameters.AddWithValue("@RpComp1", this.RpComp1);
            sqlCommand.Parameters.AddWithValue("@RpComp2", this.RpComp2);
            sqlCommand.Parameters.AddWithValue("@RpComp3", this.RpComp3);
            sqlCommand.Parameters.AddWithValue("@RpComp4", this.RpComp4);
            sqlCommand.Parameters.AddWithValue("@RpComp5", this.RpComp5);
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@leavecreditparameter", this.leavecreditparameter);
            sqlCommand.Parameters.AddWithValue("@encashmentparameter", this.encashmentparameter);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveSettings");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;

        }
        public DataSet GetTableValues(int CompanyId,Guid FinyrId)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", FinyrId);
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@Type", "SelectSettings");
            DBOperation dbOperation = new DBOperation();
            DataSet dtvalue = dbOperation.GetDataSet(sqlCommand);
            return dtvalue;
        }
        public DataTable GetComponentmatchingforleave()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Type", "GetEarningsField");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }

        public DataTable GetAdditonalDropdownValues()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Type", "GetDDAdditionalvalues");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable GetENCASHLevtypeDropdownValues()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@Type", "Getencashmentlevtype");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }

        public DataTable GetDynamicvalue()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@dynamicvalue", this.dynamicvalue);
            sqlCommand.Parameters.AddWithValue("@Type", "GetDynamicValue");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public bool SaveleaveConfigurations()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //CommonFields
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@DynamicComponentName", this.DynamicComponentName);
            sqlCommand.Parameters.AddWithValue("@DynamicComponentValue", this.DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@FinYearStart", this.FinYearStart);
            sqlCommand.Parameters.AddWithValue("@FinYearEnd", this.FinYearEnd);
            sqlCommand.Parameters.AddWithValue("@ConfigEffectiveDate", this.ConfigEffectiveDate);
            //MonthlySettings
            sqlCommand.Parameters.AddWithValue("@MaxDayMonth", this.MaxDayMonth);
            sqlCommand.Parameters.AddWithValue("@AllowDevisionMonth", this.AllowDevisionMonth);
           //SpecifiedSettings
            sqlCommand.Parameters.AddWithValue("@overallMax", this.overallMax);
            sqlCommand.Parameters.AddWithValue("@carryLimit", this.carryLimit);
            sqlCommand.Parameters.AddWithValue("@Compoffallow", this.Compoffallow);        
            //InterveningHoliday
            sqlCommand.Parameters.AddWithValue("@InvHoliday", this.InvHoliday);
            sqlCommand.Parameters.AddWithValue("@InvHolidaySubparameter", this.InvHolidaysubparameter);
            //Attachmentfields
            sqlCommand.Parameters.AddWithValue("@isattachreq", this.Isattachreq);
            sqlCommand.Parameters.AddWithValue("@attachmaxdays", this.Attachreqmaxdays);
            //MinandMaxfields
            sqlCommand.Parameters.AddWithValue("@maxdays", this.maxdays);
            sqlCommand.Parameters.AddWithValue("@Maximummaxtimes", this.maxmaxtimes);
            sqlCommand.Parameters.AddWithValue("@maxperiod", this.maxperiod);
            sqlCommand.Parameters.AddWithValue("@maxdeviation", this.maxdeviation);
            sqlCommand.Parameters.AddWithValue("@mindays", this.mindays);
            sqlCommand.Parameters.AddWithValue("@Maximummintimes", this.maxmintimes);
            sqlCommand.Parameters.AddWithValue("@minperiod", this.minperiod);
            sqlCommand.Parameters.AddWithValue("@mindeviation", this.mindeviation);
            //Defaultfields
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveLeaveConfig");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }


        public bool SaveleaveTypeSettingsData()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeDesc", this.LeaveTypeDesc);
            sqlCommand.Parameters.AddWithValue("@LevopenReq", this.LevopenReq);
            sqlCommand.Parameters.AddWithValue("@Encashavail", this.LevEncashAvail);
            //sqlCommand.Parameters.AddWithValue("@Openingbal", this.Openingbal);
            //sqlCommand.Parameters.AddWithValue("@avalbal", this.avalbal);
            sqlCommand.Parameters.AddWithValue("@usedlev", this.usedlev);
            sqlCommand.Parameters.AddWithValue("@LevTypeAct", this.LevTypeActive);
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveLeaveTypeSettings");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        public bool DeleteleaveTypeSettingsData()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "DeleteLeaveTypeSettings");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        public DataTable GetleaveTypeSettingsData(int companyid,Guid finyrid)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyid);
            sqlCommand.Parameters.AddWithValue("@FinyrId", finyrid);
            sqlCommand.Parameters.AddWithValue("@Type", "GetLeaveTypeSettings");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }
        public DataTable GetLeaveConfigurationDetails(Guid FinyrId,Guid LeaveTypeId,Guid DynamicComponentValue)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinyrId", FinyrId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@DynamicComponentValue", DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@Type", "GetLeaveConfigDetails");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }


        public bool SaveWeekoffMasterdata()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FINYrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@ComponentName", this.DynamicComponentName);
            sqlCommand.Parameters.AddWithValue("@ComponentValue", this.DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@Fromdate", this.FinYearStart);
            sqlCommand.Parameters.AddWithValue("@Todate", this.FinYearEnd);
            sqlCommand.Parameters.AddWithValue("@EntryType", this.Weekoffentryvalid);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "MasterSave");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool SaveWeekoffGridSettings()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@WeekoffId", this.Id);
            sqlCommand.Parameters.AddWithValue("@DaysName", this.DaysName);
            sqlCommand.Parameters.AddWithValue("@Weekoffset", this.Weekoffset);
            sqlCommand.Parameters.AddWithValue("@Week1", this.Week1);
            sqlCommand.Parameters.AddWithValue("@Week2", this.Week2);
            sqlCommand.Parameters.AddWithValue("@Week3", this.Week3);
            sqlCommand.Parameters.AddWithValue("@Week4", this.Week4);
            sqlCommand.Parameters.AddWithValue("@Week5", this.Week5);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "GridSave");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool SaveWeekoffDatewise(string xmlResult)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_LeaveWeekoff");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@xmlstring", xmlResult);
            DBOperation dbOperation = new DBOperation();
            string status = string.Empty;
            return dbOperation.SaveData(sqlCommand, out status);
        }
        public bool SaveGridWeekoffDate(string xmlResult)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_LeaveWeekoffDateGrid");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@xmlstring", xmlResult);
            DBOperation dbOperation = new DBOperation();
            string status = string.Empty;
            return dbOperation.SaveData(sqlCommand, out status);
        }
        public DataTable WeekoffSaveCheck()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FINYrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@ComponentName", this.DynamicComponentName);
            sqlCommand.Parameters.AddWithValue("@ComponentValue", this.DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@Fromdate", this.FinYearStart);
            sqlCommand.Parameters.AddWithValue("@Todate", this.FinYearEnd);
            sqlCommand.Parameters.AddWithValue("@Type",this.weekofftype);
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        
    }

        public DataTable GetGridviewdata()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.WeekoffTempId);
            sqlCommand.Parameters.AddWithValue("@Type", "GetMonthGrid");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }


        public DataTable GetDateviewdata()
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.WeekoffTempId);
            sqlCommand.Parameters.AddWithValue("@Type", "GetDateGrid");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }





        public DataTable Checkweekoffforrequest(int companyid, Guid finyrid, string DynamicComponentName, Guid DynamicComponentValue)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyid);
            sqlCommand.Parameters.AddWithValue("@FinyrId", finyrid);
            sqlCommand.Parameters.AddWithValue("@DynamicComponentName", DynamicComponentName);
            sqlCommand.Parameters.AddWithValue("@DynamicComponentValue", DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@Type", "Weekoffavailablecheck");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }



        public bool SaveLeaveLeaveEncashmentSettings()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@encashmentParameter", this.encashmentparameter);
            sqlCommand.Parameters.AddWithValue("@Encashcomp", this.Encashcomponent);
            sqlCommand.Parameters.AddWithValue("@EncashLmit", this.EncashLimit);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveLeaveEncashment");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        public DataTable GetLeaveLeaveEncashmentSettings()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_LeaveSettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", this.LeaveTypeId);
            sqlCommand.Parameters.AddWithValue("@encashmentParameter", this.encashmentparameter);
            sqlCommand.Parameters.AddWithValue("@Type", "GetLeaveEncashment");
            DBOperation dbOperation = new DBOperation();
            DataTable dtvalue = dbOperation.GetTableData(sqlCommand);
            return dtvalue;
        }


        public DataTable getWeekoffdataRequestingtime()
        
        {
            SqlCommand sqlCommand = new SqlCommand("weekoff_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FINYrId", this.FinyrId);
            sqlCommand.Parameters.AddWithValue("@ComponentValue", this.DynamicComponentValue);
            sqlCommand.Parameters.AddWithValue("@Type", "LeaveRequestingCheck");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


    }


}
