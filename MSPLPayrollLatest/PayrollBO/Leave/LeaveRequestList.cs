using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveRequestList : List<LeaveRequest>
    {
        public LeaveRequestList()
        {

        }
        public LeaveRequestList(int companyId,Guid financeYear,int status)
        {
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = req.GetTableValues();
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest request = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    request.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    request.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    request.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    request.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                request.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                request.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    request.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    request.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    request.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    request.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);                
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    request.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    request.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    request.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    request.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    request.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    request.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    request.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                  request.DefaultLOPid= lossofpayid.LOPId;
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompOff"])))
                    request.CompOff = Convert.ToBoolean(dtValue.Rows[i]["CompOff"]);
                this.Add(request);
            }


        }
        public LeaveRequestList( Guid employeeId,Guid financeYearId , int status)
        {
            LeaveRequest req = new LeaveRequest();
            req.EmployeeId = employeeId;
            req.FinanceYear = financeYearId;
            req.Status = status;
            DataTable dtValue = req.GetTableValues();
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay  = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);

                this.Add(requests);

            }


        }
        //to get current financial year data for MYREPORT 
        public LeaveRequestList(Guid employeeId, int CompanyId,Guid finyear)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetValueforMYREPORT(employeeId, CompanyId, finyear);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Rejectreason = (Convert.ToString(dtValue.Rows[i]["RejReason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //   if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                //    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ChildId"])))
                    requests.ChildId = new Guid(Convert.ToString(dtValue.Rows[i]["ChildId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                this.Add(requests);

            }


        }
        //end of MYREPORT

        public LeaveRequestList(Guid employeeId, int CompanyId, Guid finyear,string type)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetApprovecancelreportdatas(employeeId, CompanyId, finyear,type);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                requests.EmployeeName = (Convert.ToString(dtValue.Rows[i]["EmployeeName"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Rejectreason = (Convert.ToString(dtValue.Rows[i]["RejReason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                requests.LeaveStatus = Convert.ToString(dtValue.Rows[i]["Statusvalue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ChildId"])))
                    requests.ChildId = new Guid(Convert.ToString(dtValue.Rows[i]["ChildId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                this.Add(requests);

            }


        }
        public LeaveRequestList(Guid employeeId, int CompanyId,string type)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            if (type == "FH")
            {
                dtValue = req.GetTableValuesfh(employeeId, CompanyId, "");
            }
            else
            {
                dtValue = req.GetTableValuesCompoff(employeeId, CompanyId, type);
            }

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Rejectreason = (Convert.ToString(dtValue.Rows[i]["RejReason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttachmentPath"])))
                    requests.Imgpath = (Convert.ToString(dtValue.Rows[i]["AttachmentPath"]));
                this.Add(requests);

            }


        }
        public LeaveRequestList(Guid employeeId,int CompanyId)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();
          
                 dtValue = req.GetTableValues(employeeId, CompanyId);
          
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Rejectreason = (Convert.ToString(dtValue.Rows[i]["RejReason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //   if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                //    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                this.Add(requests);

            }


         }
        public LeaveRequestList(DateTime from, DateTime to, Guid EmployeeId, int LevStat,int compid)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetMYREPORTLeaveReport(from, to, EmployeeId, LevStat, compid);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = (Convert.ToDateTime(dtValue.Rows[i]["FromDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeName"])))
                    requests.EmployeeName = Convert.ToString(dtValue.Rows[i]["EmployeeName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveName"])))
                    requests.LeaveTypeName = Convert.ToString(dtValue.Rows[i]["LeaveName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveStatus"])))
                    requests.LeaveStatus = Convert.ToString(dtValue.Rows[i]["LeaveStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                this.Add(requests);

            }


        }


        public LeaveRequestList(Guid leaveid,int compid,Guid finyrid,Guid employeeid)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetMRTakenlevCheck(leaveid, compid, finyrid, employeeid);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Leavecount"])))
                    requests.levcount = Convert.ToDouble(dtValue.Rows[i]["Leavecount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["curMonth"])))
                    requests.CurMonth = Convert.ToInt32(dtValue.Rows[i]["curMonth"]);
                this.Add(requests);

            }
        }


        public LeaveRequestList(DateTime from, DateTime to,Guid EmployeeId,int LevStat,Guid FinYear,Guid LoggedUser)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetAssignManagerViewReport(from, to, EmployeeId, LevStat, FinYear, LoggedUser);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = (Convert.ToDateTime(dtValue.Rows[i]["FromDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["fromday"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["fromday"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["today"])))
                    requests.ToDay= Convert.ToInt32(dtValue.Rows[i]["today"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["noofdays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["noofdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeName"])))
                    requests.EmployeeName = Convert.ToString(dtValue.Rows[i]["EmployeeName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveName"])))
                    requests.LeaveTypeName = Convert.ToString(dtValue.Rows[i]["LeaveName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveStatus"])))
                    requests.LeaveStatus = Convert.ToString(dtValue.Rows[i]["LeaveStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                this.Add(requests);

            }


        }
        public LeaveRequestList(Guid Leavetypeid)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.Checkingforleavedelete(Leavetypeid);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveOPening"])))
                    requests.Leaveopening = Convert.ToInt32(dtValue.Rows[i]["LeaveOPening"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveCredits"])))
                    requests.Leavecredits = Convert.ToInt32(dtValue.Rows[i]["LeaveCredits"]);
               
                this.Add(requests);

            }


        }

        public LeaveRequestList(Guid Leaverequestid, string empty)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetSingleLeaverequestvalue(Leaverequestid);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Rejectreason = (Convert.ToString(dtValue.Rows[i]["RejReason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = Convert.ToString(dtValue.Rows[i]["NoOfDays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttachmentPath"])))
                    requests.Imgpath = (Convert.ToString(dtValue.Rows[i]["AttachmentPath"]));
                this.Add(requests);

            }


        }


        public LeaveRequestList(int CompanyId)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = new DataTable();

            dtValue = req.GetLeaveDeleteData(CompanyId);

            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //   if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                this.Add(requests);

            }


        }



        public LeaveRequestList(Guid id, int prioritynumber,int tempvalue)
        {
            //list to get requested list of previous priority number
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = req.GetpreviouspriortyRequestedvalue(id, prioritynumber);
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests1 = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveStatus"])))
                    requests1.MANAGERStatus = Convert.ToInt32(dtValue.Rows[i]["LeaveStatus"]);
                this.Add(requests1);
            }


        }



        public LeaveRequestList(Guid employeeId, int CompanyId,int tempvalue,Guid financialyear,string ReqType="")
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = req.GetemployeeRequestedvalue(employeeId, CompanyId, financialyear, ReqType);
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYear"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LeaveType"]));
                requests.Reason = (Convert.ToString(dtValue.Rows[i]["Reason"]));
                requests.Contact = (Convert.ToString(dtValue.Rows[i]["Contact"]));
                requests.FirstLvlContact = (Convert.ToString(dtValue.Rows[i]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDay"])))
                    requests.FromDay = Convert.ToInt32(dtValue.Rows[i]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ToDay"])))
                    requests.ToDay = Convert.ToInt32(dtValue.Rows[i]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromDate"])))
                    requests.FromDate = Convert.ToDateTime(dtValue.Rows[i]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EndDate"])))
                    requests.EndDate = (Convert.ToDateTime(dtValue.Rows[i]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Status"])))
                    requests.Status = Convert.ToInt32(dtValue.Rows[i]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ManagerPriority"])))
                    requests.prioritynumber = Convert.ToInt32(dtValue.Rows[i]["ManagerPriority"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                //    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                //   if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                //    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                    requests.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeCode"])))
                    requests.Empcode = Convert.ToString(dtValue.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AssignManagerId"])))
                    requests.AssignmanagerId = new Guid(Convert.ToString(dtValue.Rows[i]["AssignManagerId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["NoOfDays"])))
                    requests.NoOfDays = (Convert.ToString(dtValue.Rows[i]["NoOfDays"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttachmentPath"])))
                    requests.Imgpath = (Convert.ToString(dtValue.Rows[i]["AttachmentPath"]));
                this.Add(requests); 

            }


        }







        public LeaveRequestList(Guid FinanceYear, int CompanyId, int month, int year)
        {
            LeaveRequest req = new LeaveRequest();
            DataTable dtValue = req.GetAbsentTableValues(FinanceYear, CompanyId, month, year);
            for (int i = 0; i < dtValue.Rows.Count; i++)
            {
                LeaveRequest requests = new LeaveRequest();
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                    requests.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYearId"])))
                    requests.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["EmployeeId"])))
                    requests.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LevType"])))
                    requests.LeaveType = new Guid(Convert.ToString(dtValue.Rows[i]["LevType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LevDate"])))
                    requests.LevDate = Convert.ToDateTime(dtValue.Rows[i]["LevDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["HFDay"])))
                    requests.HFDay = Convert.ToDouble(dtValue.Rows[i]["HFDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                    requests.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                    requests.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                    requests.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                    requests.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDelete"])))
                    requests.IsDeleted= Convert.ToBoolean(dtValue.Rows[i]["IsDelete"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["LeaveTypeName"])))
                    requests.LeaveTypeName = (Convert.ToString(dtValue.Rows[i]["LeaveTypeName"]));
                this.Add(requests);

            }


        }

    }
}
