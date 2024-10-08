using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO.Leave
{
   public class LeaveMasterList : List<LeaveSettingsBO>
    {

        public LeaveMasterList(int companyid, Guid finyrid)
        {

            LeaveSettingsBO LeaveMasterSettings = new LeaveSettingsBO();
            DataSet dtset = LeaveMasterSettings.GetTableValues(companyid, finyrid);
            DataTable DtMastersettings = dtset.Tables[0];
            DataTable checkMasterSave = dtset.Tables.Count >1? dtset.Tables[1]:new DataTable ();
            if (DtMastersettings.Rows.Count > 0)
            {
                for (int i = 0; i <= DtMastersettings.Rows.Count - 1; i++)
                {
                    LeaveSettingsBO levMsettings = new LeaveSettingsBO();
                    levMsettings.leaveparameter = DtMastersettings.Rows[0]["LeaveParameter"].ToString();
                    levMsettings.Holidayparameter = DtMastersettings.Rows[0]["HolidayParameter"].ToString();
                    levMsettings.Compoffparameter = DtMastersettings.Rows[0]["CompoffParameter"].ToString();
                    levMsettings.Weekoffparameter = DtMastersettings.Rows[0]["WeekoffParameter"].ToString();
                    levMsettings.Weekoffentryvalid = DtMastersettings.Rows[0]["WeekoffEntryValid"].ToString();
                    levMsettings.RpComp1 = DtMastersettings.Rows[0]["Rpcomp1"].ToString();
                    levMsettings.RpComp2 = DtMastersettings.Rows[0]["Rpcomp2"].ToString();
                    levMsettings.RpComp3 = DtMastersettings.Rows[0]["Rpcomp3"].ToString();
                    levMsettings.RpComp4 = DtMastersettings.Rows[0]["Rpcomp4"].ToString();
                    levMsettings.RpComp5 = DtMastersettings.Rows[0]["Rpcomp5"].ToString();
                    levMsettings.maxperiod= DtMastersettings.Rows[0]["Maxperiod"].ToString();
                    levMsettings.maxdeviation = DtMastersettings.Rows[0]["MaxDeviation"].ToString();
                    levMsettings.minperiod = DtMastersettings.Rows[0]["Minperiod"].ToString();
                    levMsettings.mindeviation = DtMastersettings.Rows[0]["MinDeviation"].ToString();
                    levMsettings.Minormaxparameter = DtMastersettings.Rows[0]["minmaxparameter"].ToString();
                    levMsettings.leavecreditparameter = DtMastersettings.Rows[0]["LeaveCreditParameter"].ToString();
					 levMsettings.encashmentparameter = DtMastersettings.Rows[0]["EncashmentParameter"].ToString();
                    if (DtMastersettings.Rows[0]["maxdays"].ToString()!=null)
                    {
                        levMsettings.maxdays =Convert.ToDecimal(DtMastersettings.Rows[0]["maxdays"].ToString());
                    }
                    else
                    {
                        levMsettings.maxdays = 0;
                    }
                    if (DtMastersettings.Rows[0]["Maximummaxtimes"].ToString() != null)
                    {
                        levMsettings.maxmaxtimes = Convert.ToDecimal(DtMastersettings.Rows[0]["Maximummaxtimes"].ToString());
                    }
                    else
                    {
                        levMsettings.maxmaxtimes = 0;
                    }
                    if (DtMastersettings.Rows[0]["mindays"].ToString() != null)
                    {
                        levMsettings.mindays = Convert.ToDecimal(DtMastersettings.Rows[0]["mindays"].ToString());
                     }
                    else
                    {
                        levMsettings.mindays = 0;
                    }
                    if (DtMastersettings.Rows[0]["Maximummintimes"].ToString() != null)
                    {
                        levMsettings.maxmintimes = Convert.ToDecimal(DtMastersettings.Rows[0]["Maximummintimes"].ToString());
                    }
                    else
                    {
                        levMsettings.maxmintimes = 0;
                    }
                    //if (checkMasterSave.Rows.Count>0)
                    //{
                    //    int count  = Convert.ToInt32(checkMasterSave.Rows[0]["Entrycount"]);
                    //    levMsettings.btnSaveEnable = count > 0 ? false : true;
                    //}
                   
                    this.Add(levMsettings);
                }
            }
        }


    }
}
