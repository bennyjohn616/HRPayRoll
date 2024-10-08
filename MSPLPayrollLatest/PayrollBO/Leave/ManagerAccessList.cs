using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ManagerAccessList : List<ManagerAccess>
    {
        public ManagerAccessList(Guid managerId, int companyId, Guid employeeId)
        {

            ManagerAccess mgr = new ManagerAccess();

            mgr.CompanyId = companyId;
            mgr.EmployeeId = employeeId;
            mgr.ManagerId = managerId;
            DataTable dtValue = mgr.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    ManagerAccess mgraccess = new ManagerAccess();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        mgraccess.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                        mgraccess.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ManagerId"])))
                        mgraccess.ManagerId = new Guid(Convert.ToString(dtValue.Rows[i]["ManagerId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Prioritylevel"])))
                        mgraccess.Prioritylevel = (Convert.ToInt32(dtValue.Rows[i]["Prioritylevel"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ApprovalMust"])))
                        mgraccess.ApprovalMust = (Convert.ToBoolean(dtValue.Rows[i]["ApprovalMust"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CancelRights"])))
                        mgraccess.CancelRights = (Convert.ToBoolean(dtValue.Rows[i]["CancelRights"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompanyId"])))
                        mgraccess.CompanyId = Convert.ToInt32(dtValue.Rows[i]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        mgraccess.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        mgraccess.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        mgraccess.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        mgraccess.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        mgraccess.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    this.Add(mgraccess);
                }
            }

        }

    }
}
