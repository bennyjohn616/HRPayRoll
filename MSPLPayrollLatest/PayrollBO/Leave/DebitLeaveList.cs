using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class DebitLeaveList : List<DebitLeave>
    {
       
        public DebitLeaveList(int companyId,Guid finyrid,Guid empid)
        {
            DebitLeave leavYear = new DebitLeave();
            leavYear.CompanyId = companyId;
            leavYear.FinanceYearId = finyrid;
            leavYear.EmployeeId = empid;
            DataTable dtValue = leavYear.GetDebitLeave();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    DebitLeave lvFinancYear = new DebitLeave();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lvFinancYear.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        lvFinancYear.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinYear"])))
                        lvFinancYear.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DebitLevEntryDate"])))
                        lvFinancYear.DebitLeaveEntryDate = (Convert.ToDateTime(dtValue.Rows[rowcount]["DebitLevEntryDate"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DebitLevType"])))
                        lvFinancYear.DebitLevType = new Guid(Convert.ToString(dtValue.Rows[rowcount]["DebitLevType"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LeaveType"])))
                        lvFinancYear.LeaveType = (Convert.ToString(dtValue.Rows[rowcount]["LeaveType"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NoOfDays"])))
                        lvFinancYear.NoOfDays = (Convert.ToDecimal(dtValue.Rows[rowcount]["NoOfDays"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        lvFinancYear.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Reason"])))
                        lvFinancYear.Reason = (Convert.ToString(dtValue.Rows[rowcount]["Reason"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        lvFinancYear.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(lvFinancYear);
                }

            }
        }
    }
}
