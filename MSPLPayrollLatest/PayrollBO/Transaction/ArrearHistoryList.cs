using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ArrearHistoryList : List<ArrearHistory>
    {
        public ArrearHistoryList()
        {

        }
        public ArrearHistoryList(Guid empId, Guid historyid)
        {
            ArrearHistory arHistory = new ArrearHistory();
            arHistory.EmployeeId = empId;
            arHistory.PayHistoryId = historyid;
            DataTable dtValue = arHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    ArrearHistory newhistry = new ArrearHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        newhistry.Id = Convert.ToInt32(dtValue.Rows[i]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                        newhistry.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["PayrollHistoryId"])))
                        newhistry.PayHistoryId = new Guid(Convert.ToString(dtValue.Rows[i]["PayrollHistoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttributemodelId"])))
                        newhistry.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[i]["AttributemodelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Month"])))
                        newhistry.Month = (Convert.ToInt16(dtValue.Rows[i]["Month"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Year"])))
                        newhistry.Year = (Convert.ToInt16(dtValue.Rows[i]["Year"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Value"])))
                        newhistry.Value = Convert.ToInt32(dtValue.Rows[i]["Value"]);
                    this.Add(newhistry);
                }
            }


        }
    }
}
