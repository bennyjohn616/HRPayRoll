using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PayrollBO.Leave
{
   public class WeekoffComponentMatchingList:List<WeekoffComponentMatching>
    {
        public WeekoffComponentMatchingList(int companyId)
        {

            WeekoffComponentMatching wkcmpmatching = new WeekoffComponentMatching();
            wkcmpmatching.CompanyId = companyId;
            DataTable dtValue = wkcmpmatching.Getweekoffcmpmatching();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    WeekoffComponentMatching WeekoffCompMatching = new WeekoffComponentMatching();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        WeekoffCompMatching.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LeaveCategoryId"])))
                        WeekoffCompMatching.LeaveCategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["LeaveCategoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Leavecomponent"])))
                        WeekoffCompMatching.Leavecomponent = dtValue.Rows[rowcount]["Leavecomponent"].ToString();                   
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        WeekoffCompMatching.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        WeekoffCompMatching.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        WeekoffCompMatching.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        WeekoffCompMatching.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        WeekoffCompMatching.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(WeekoffCompMatching);
                }

            }
        }
    }
}
