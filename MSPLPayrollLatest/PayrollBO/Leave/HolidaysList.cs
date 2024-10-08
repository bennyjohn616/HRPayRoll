using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PayrollBO
{
   public class HolidaysList : List<Holidays>
    {
        public HolidaysList()
        {

        }
        public HolidaysList(Guid id, Guid finid)
            {
            Holidays holiday = new Holidays();
            holiday.Id= id;
            holiday.financeyearId = finid;
            DataTable dtValue = holiday.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    Holidays holdayset = new Holidays();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        holdayset.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceyearId"])))
                        holdayset.financeyearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceyearId"]));
                    holdayset.HolidayReason = Convert.ToString(dtValue.Rows[i]["Reason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Holiday Date"])))
                        holdayset.Holidaydate = Convert.ToDateTime(dtValue.Rows[i]["Holiday Date"]);
                    holdayset.holidayDAY = Convert.ToString(dtValue.Rows[i]["Holiday Day"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ComponentValue"])))
                        holdayset.ComponentValue = new Guid(Convert.ToString(dtValue.Rows[i]["ComponentValue"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ComponentName"])))
                        holdayset.ComponentName = Convert.ToString(dtValue.Rows[i]["ComponentName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Component"])))
                        holdayset.Component = Convert.ToString(dtValue.Rows[i]["Component"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Holiday Type"])))
                        holdayset.Type = Convert.ToString(dtValue.Rows[i]["Holiday Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["cretedOn"])))
                        holdayset.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["cretedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        holdayset.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        holdayset.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        holdayset.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    this.Add(holdayset);
                }

            }
        }



        public HolidaysList(Guid id, Guid finid,Guid Componentvalue)
        {
            Holidays holiday = new Holidays();
            holiday.Id = id;
            holiday.financeyearId = finid;
            DataTable dtValue = holiday.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    if(new Guid(Convert.ToString(dtValue.Rows[i]["ComponentValue"]))== Componentvalue)
                    {
                        Holidays holdayset = new Holidays();
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                            holdayset.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceyearId"])))
                            holdayset.financeyearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceyearId"]));
                        holdayset.HolidayReason = Convert.ToString(dtValue.Rows[i]["Reason"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Holiday Date"])))
                            holdayset.Holidaydate = Convert.ToDateTime(dtValue.Rows[i]["Holiday Date"]);
                        holdayset.holidayDAY = Convert.ToString(dtValue.Rows[i]["Holiday Day"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ComponentValue"])))
                            holdayset.ComponentValue = new Guid(Convert.ToString(dtValue.Rows[i]["ComponentValue"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ComponentName"])))
                            holdayset.ComponentName = Convert.ToString(dtValue.Rows[i]["ComponentName"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Component"])))
                            holdayset.Component = Convert.ToString(dtValue.Rows[i]["Component"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Holiday Type"])))
                            holdayset.Type = Convert.ToString(dtValue.Rows[i]["Holiday Type"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["cretedOn"])))
                            holdayset.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["cretedOn"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                            holdayset.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                            holdayset.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                            holdayset.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                        this.Add(holdayset);
                    }
                }

            }
        }



        public HolidaysList(int compid)
        {
            Holidays holiday = new Holidays();
            DataTable dtValue = holiday.GetTableyearlylevopeningsdecvalue(compid);
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    Holidays holdayset = new Holidays();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        holdayset.yrlylevopndecID = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinYear"])))
                        holdayset.financeyearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Companyid"])))
                        holdayset.Companyid = Convert.ToInt32(dtValue.Rows[i]["Companyid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Category"])))
                        holdayset.catid = new Guid(Convert.ToString(dtValue.Rows[i]["Category"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["leaveType"])))
                        holdayset.levid = new Guid(Convert.ToString(dtValue.Rows[i]["leaveType"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["openingDays"])))
                        holdayset.opdays = Convert.ToDecimal(dtValue.Rows[i]["openingDays"]);
                    holdayset.CATIDNAME = Convert.ToString(dtValue.Rows[i]["Name"]);
                    holdayset.LEVIDNAME = Convert.ToString(dtValue.Rows[i]["leavename"]);
                    this.Add(holdayset);
                }

            }
        }
    }
}
