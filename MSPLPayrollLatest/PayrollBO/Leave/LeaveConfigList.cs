using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace PayrollBO.Leave
{
   public class LeaveConfigList: List<LeaveSettingsBO>
    {

        public LeaveConfigList(Guid FinyrId, Guid LeaveTypeId, Guid DynamicComponentValue)
        {

            LeaveSettingsBO LeaveConfigurations = new LeaveSettingsBO();
           
            DataTable DTLeaveConfigurationDetails = LeaveConfigurations.GetLeaveConfigurationDetails(FinyrId, LeaveTypeId, DynamicComponentValue);
            if (DTLeaveConfigurationDetails.Rows.Count > 0)
            {
                LeaveSettingsBO Configlist = new LeaveSettingsBO();
                Configlist.LeaveTypeId = new Guid(DTLeaveConfigurationDetails.Rows[0]["LeaveTypeId"].ToString());
                Configlist.DynamicComponentName = DTLeaveConfigurationDetails.Rows[0]["Component"].ToString();
                Configlist.DynamicComponentValue = new Guid(DTLeaveConfigurationDetails.Rows[0]["ComponentValueId"].ToString());
                Configlist.ConfigEffectiveDate = Convert.ToDateTime(DTLeaveConfigurationDetails.Rows[0]["EntryEffectiveon"].ToString());
                Configlist.MaxDayMonth = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MonthMaxDays"].ToString());
                Configlist.AllowDevisionMonth = DTLeaveConfigurationDetails.Rows[0]["AllowPreviousleaveUsed"].ToString();
                Configlist.overallMax = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MaxOpeningpluscredit"].ToString());
                Configlist.carryLimit = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["CarryforwardLimit"].ToString());
                Configlist.Compoffallow = DTLeaveConfigurationDetails.Rows[0]["Compoffbetweenlevdays"].ToString();
                //Intervening Holidays
                Configlist.InvHoliday = DTLeaveConfigurationDetails.Rows[0]["InterveningHolidays"].ToString();
                Configlist.InvHolidaysubparameter = DTLeaveConfigurationDetails.Rows[0]["InvHolidaysSubcondition"].ToString();
                //Min&MaxSettings
                Configlist.mindays = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MinNoOfDaysAtaTime"].ToString());
                Configlist.maxmintimes = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MaximumMinNoOfDaysapply"].ToString());
                Configlist.minperiod = DTLeaveConfigurationDetails.Rows[0]["Minperiod"].ToString();
                Configlist.mindeviation = DTLeaveConfigurationDetails.Rows[0]["MinDeviation"].ToString();
                Configlist.maxdays = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MaxNoOfDaysAtaTime"].ToString());
                Configlist.maxdeviation = DTLeaveConfigurationDetails.Rows[0]["MaxDeviation"].ToString();
                Configlist.maxmaxtimes = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["MaximumMaxNoOfDaysapply"].ToString());
                Configlist.maxperiod = DTLeaveConfigurationDetails.Rows[0]["Maxperiod"].ToString();
                //Attachment 
                Configlist.Isattachreq = DTLeaveConfigurationDetails.Rows[0]["AttachmentRequired"].ToString();
                Configlist.Attachreqmaxdays = Convert.ToDecimal(DTLeaveConfigurationDetails.Rows[0]["AttachmentRequiredMaxDays"].ToString());
                this.Add(Configlist);
            }
        }
    }
}
