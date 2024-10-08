using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveTypeList : List<LeaveType>
    {
        public LeaveTypeList(int CompanyId)
        {


            LeaveType leave = new LeaveType();
            leave.CompanyId = CompanyId;
            DataTable dtValue = leave.GetMasterTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    LeaveType levopn = new LeaveType();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        levopn.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    levopn.LeaveTypeName = Convert.ToString(dtValue.Rows[i]["LeaveType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompanyId"])))
                        levopn.CompanyId = Convert.ToInt32(dtValue.Rows[i]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        levopn.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        levopn.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        levopn.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        levopn.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        levopn.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    this.Add(levopn);
                }
            }

        }

        public LeaveTypeList(int CompanyId,Guid Finyear)
        {


            LeaveType leave = new LeaveType();
            leave.CompanyId = CompanyId;
            leave.FinyrId = Finyear;
            DataTable dtValue = leave.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    LeaveType levopn = new LeaveType();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        levopn.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    levopn.LeaveTypeName = Convert.ToString(dtValue.Rows[i]["LeaveType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompanyId"])))
                        levopn.CompanyId = Convert.ToInt32(dtValue.Rows[i]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        levopn.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        levopn.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        levopn.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        levopn.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        levopn.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LevDesc"])))
                        levopn.LeaveDescription = Convert.ToString(dtValue.Rows[i]["LevDesc"]);
                    this.Add(levopn);
                }
            }

        }
    }
}
