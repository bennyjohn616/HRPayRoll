using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class CreditDaysList:List<CreditDays>
    {
        public CreditDaysList()
        {

        }


        public CreditDaysList(int companyId,int month,int year,Guid empid,string type="",string ProPayroll="")
        {
            
            CreditDays creditDays = new CreditDays();
            creditDays.CompanyId = companyId;
            creditDays.ApplyMonth = month;
            creditDays.ApplyYear = year;
            creditDays.CType = type;
            creditDays.EmployeeId = empid;
            creditDays.ProPayroll = ProPayroll;
            DataTable dtValue = creditDays.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    CreditDays creditDay = new CreditDays();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        creditDay.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        creditDay.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        creditDay.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyMonth"])))
                        creditDay.ApplyMonth = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyYear"])))
                        creditDay.ApplyYear = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        creditDay.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        creditDay.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if(!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PaidDays"])))
                        creditDay.PaidDays = Convert.ToInt32(dtValue.Rows[rowcount]["PaidDays"]);
                    if(!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LopDays"])))
                        creditDay.LopDays = Convert.ToInt32(dtValue.Rows[rowcount]["LopDays"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        creditDay.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        creditDay.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsProcessed"])))
                        creditDay.IsProcessed = Convert.ToBoolean(dtValue.Rows[rowcount]["IsProcessed"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Type"])))
                        creditDay.CType = Convert.ToString(dtValue.Rows[rowcount]["Type"]);

                    this.Add(creditDay);
                }

            }



        }
       
    }
}
