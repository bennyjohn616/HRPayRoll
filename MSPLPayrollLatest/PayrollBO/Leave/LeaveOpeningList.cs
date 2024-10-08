using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveOpeningList : List<LeaveOpenings>
    {
        public LeaveOpeningList(Guid employeeId, Guid financeYearId,Guid leaveType)
        {
            LeaveOpenings openings = new LeaveOpenings();
            if(financeYearId == Guid.Empty)
            {
                //LeaveBase year = new LeaveBase();
                //financeYearId = year.CurentFinanceYear.Id;
            }
            openings.FinanceYearId = financeYearId;
            openings.EmployeeId = employeeId;
            openings.LeaveType = leaveType;
            DataTable dtValue = openings.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    LeaveOpenings leaveopenings = new LeaveOpenings();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        leaveopenings.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                        leaveopenings.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                        leaveopenings.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                        leaveopenings.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                    //leaveopenings.Leave_Encash = Convert.ToString(dtValue.Rows[i]["Leave_Encash"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveOpening"])))
                        leaveopenings.LeaveOpening = (Convert.ToDouble(dtValue.Rows[i]["LeaveOpening"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveCredit"])))
                        leaveopenings.LeaveCredit = (Convert.ToDouble(dtValue.Rows[i]["LeaveCredit"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveUsed"])))
                        leaveopenings.LeaveUsed = Convert.ToDecimal(dtValue.Rows[i]["LeaveUsed"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    //    leaveopenings.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        leaveopenings.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                    //    leaveopenings.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        leaveopenings.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    //    leaveopenings.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    this.Add(leaveopenings);
                }
            }
        }
    }
}
