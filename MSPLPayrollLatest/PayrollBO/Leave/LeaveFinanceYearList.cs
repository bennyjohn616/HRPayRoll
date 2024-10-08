using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveFinanceYearList : List<LeaveFinanceYear>
    {
        public LeaveFinanceYearList(int companyId)
        {

            LeaveFinanceYear leavYear = new LeaveFinanceYear();
            leavYear.CompanyId = companyId;
            DataTable dtValue = leavYear.GetFinanceYear();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LeaveFinanceYear lvFinancYear = new LeaveFinanceYear();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        lvFinancYear.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StartMonth"])))
                        lvFinancYear.StartMonth = (Convert.ToDateTime(dtValue.Rows[rowcount]["StartMonth"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EndMonth"])))
                        lvFinancYear.EndMonth = (Convert.ToDateTime(dtValue.Rows[rowcount]["EndMonth"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefault"])))
                        lvFinancYear.IsDefault = (Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefault"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        lvFinancYear.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        lvFinancYear.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        lvFinancYear.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        lvFinancYear.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        lvFinancYear.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(lvFinancYear);
                }

            }
        }
    }
}
