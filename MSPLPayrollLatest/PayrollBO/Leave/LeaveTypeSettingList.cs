using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO.Leave
{
   public class LeaveTypeSettingList : List<LeaveSettingsBO>
    {

        public LeaveTypeSettingList(int companyid, Guid finyrid)
        {

            LeaveSettingsBO LeaveTypeSettings = new LeaveSettingsBO();

            DataTable DTLeaveTypeSettingDetails = LeaveTypeSettings.GetleaveTypeSettingsData(companyid, finyrid);
            if (DTLeaveTypeSettingDetails.Rows.Count > 0)
            {
                for (int i = 0; i <= DTLeaveTypeSettingDetails.Rows.Count - 1; i++)
                {
                    LeaveSettingsBO levtypesettinglist = new LeaveSettingsBO();
                    levtypesettinglist.Id = new Guid(DTLeaveTypeSettingDetails.Rows[i]["Id"].ToString()); 
                    levtypesettinglist.LeaveTypeId = new Guid(DTLeaveTypeSettingDetails.Rows[i]["LevTypeId"].ToString());
                    levtypesettinglist.leaveparameter = DTLeaveTypeSettingDetails.Rows[i]["LeaveType"].ToString();
                    levtypesettinglist.LeaveTypeDesc = DTLeaveTypeSettingDetails.Rows[i]["LevDesc"].ToString();
                    levtypesettinglist.LevopenReq = DTLeaveTypeSettingDetails.Rows[i]["Leaveopeningrequired"].ToString();
                    levtypesettinglist.LevEncashAvail = DTLeaveTypeSettingDetails.Rows[i]["IsEncashment"].ToString();
                    //levtypesettinglist.OpeningbalName = DTLeaveTypeSettingDetails.Rows[i]["openingcomponent"].ToString();
                    //levtypesettinglist.avalbalName = DTLeaveTypeSettingDetails.Rows[i]["Availablecomponent"].ToString();
                    levtypesettinglist.usedlevName = DTLeaveTypeSettingDetails.Rows[i]["usedcomponent"].ToString();
                    if (!string.IsNullOrEmpty(Convert.ToString(DTLeaveTypeSettingDetails.Rows[i]["UsedLeaveComponent"].ToString())))
                        levtypesettinglist.UsedLeaveId = new Guid(DTLeaveTypeSettingDetails.Rows[i]["UsedLeaveComponent"].ToString());   //Added Component id in order to assign value during payroll process.
                    levtypesettinglist.LevTypeActive = DTLeaveTypeSettingDetails.Rows[i]["Active"].ToString();
                    this.Add(levtypesettinglist);
                }
            }
        }

    }
}
