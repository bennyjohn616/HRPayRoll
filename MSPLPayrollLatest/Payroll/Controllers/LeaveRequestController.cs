﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using TraceError;
using System.Globalization;
using Payroll.Controllers;
using System.Configuration;
using System.Data;
using NotificationEngine;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data.SqlClient;
using SystemWindowsFile;
using Payroll.CustomFilter;
using PayrollBO.Leave;
using System.Xml;
using System.Xml.Serialization;
using Payroll.Models.Leave;
using Newtonsoft.Json;
using iTextSharp.text;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class LeaveRequestController : BaseController
    {
        // GET: LeaveRequest
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ApproveRejectionByMail(Guid empid, Guid Leaveid, Guid assgnmgrid, int prioritynum, int AorRstat, int status, string Navtab)
        {
            try
            {


                UserList User = new UserList(assgnmgrid);
                Session["DBString"] = User[0].DBString;
                emailapproval emailview = new emailapproval();
                PayrollBO.RoleList role = new PayrollBO.RoleList(0, User[0].CompanyId);
                PayrollBO.User userObj = new PayrollBO.User();
                AssignManager assignManagercheck = new AssignManager(Leaveid, empid, 0);
                int levstatus = Convert.ToInt32(assignManagercheck[0].LevStatus);
                Employee employee = new Employee(empid);

                if (Session["Userid"] != null)
                {
                    ViewData["sucessstatus"] = emailview;
                    emailview.displaystatus = "Already a User Signed In, Kindly logout and continue";
                    return View("ApproveRejectionByEMail", new UserMenu { empproperty = employee, emailapprvalproperty = emailview });
                }

                Session["UserId"] = User[0].Id;
                PayrollBO.UserCompanymapping logedUser = new PayrollBO.UserCompanymapping(Convert.ToInt32(Session["UserId"]));
                Session["UserName"] = User[0].Username;
                Session["UserRole"] = User[0].UserRole;
                Session["CompanyId"] = logedUser.CompanyId;
                Session["Password"] = User[0].Password;
                Session["EmployeeId"] = User[0].EmployeeId;
                Session["EmployeeName"] = User[0].FirstName + " " + User[0].LastName;
                string profileImg = userObj.ProfileImage;
                if (!string.IsNullOrEmpty(profileImg))
                {
                    profileImg = profileImg.Replace("~/", "");
                }
                Session["userProfileImage"] = profileImg;
                role.ForEach(u =>
                {
                    var Role = role.Where(r => r.Id == User[0].UserRole).FirstOrDefault();
                    if (!object.ReferenceEquals(Role, null))
                        userObj.RoleName = Role.Name;
                });
                Session["RoleName"] = userObj.RoleName;
                if (userObj.RoleName == "Employee")
                {

                    Session["EmployeeGUID"] = userObj.EmployeeId;
                    PayrollBO.Employee emp = new PayrollBO.Employee(userObj.EmployeeId);
                    Session["EmployeeCode"] = emp.EmployeeCode;
                }
                else
                {
                    Session["EmployeeGUID"] = Guid.Empty;
                }

                if (levstatus == 0)
                {
                    if (AorRstat == 1)
                    {
                        UpdateLeaveStatusThroughMail(AorRstat, prioritynum, assgnmgrid, Leaveid, "", status, Navtab);

                        emailview.appstatus = 1;
                        emailview.displaystatus = "Leave Request Approved Successfully!!!";
                    }
                    else
                    {
                        emailview.appstatus = 2;
                        emailview.assgnmngrid = assgnmgrid;
                        emailview.Leaveid = Leaveid;
                        emailview.prioritynum = prioritynum;
                        emailview.AorRstat = AorRstat;
                        emailview.status = status;
                        emailview.displaystatus = "";
                        emailview.NavTab = Navtab;
                    }

                    ViewData["sucessstatus"] = emailview;
                    return View("ApproveRejectionByEMail", new UserMenu { empproperty = employee, emailapprvalproperty = emailview });
                }
                else if (levstatus == 1)
                {
                    //emailview.displaystatus = "Leave Request Already Approved ";
                    emailview.displaystatus = "Leave Request Approved Successfully";
                }
                else if (levstatus == 2)
                {
                    emailview.displaystatus = "Leave Request Already Rejected ";
                }
                else
                {
                    emailview.displaystatus = "Leave Request Already Cancelled by Employee ";
                }
                ViewData["sucessstatus"] = emailview;
                Session.Abandon();
                return View("ApproveRejectionByEMail", new UserMenu { empproperty = employee, emailapprvalproperty = emailview });
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return Redirect("~/Login/Index");
            }
        }


        public ActionResult LeaveRequestApprove(Guid empid, Guid id)
        {
            LeaveRequest lev = new LeaveRequest(id);

            //  LeaveApproveMail(empid, id);
            return View("LeaveRequestApprove");

        }
        public bool LeaveApproveMail(Guid employeeId, Guid id, Guid NxtAssEmpId, Guid AssEmpId, int prioritynumber, string type)
        {
            Employee employee = new Employee(employeeId);
            Guid sesionid = new Guid(Session["Employeeid"].ToString());
            Employee Assignedemployee = new Employee(sesionid);
            int companyId = employee.CompanyId;
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            string sendMailTo = string.Empty;
            bool status = false;
            MailConfig mailConfig = new MailConfig(companyId);
            string smtpServer = mailConfig.IPAddress;
            int portNo = mailConfig.PortNo;
            string fromMail = mailConfig.FromEmail;
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveRequest leaveRequest = new LeaveRequest(id, Guid.Empty);
            if (leaveRequest.LeaveType != new Guid(lossofpayid.LOPId.ToString()))//199f5db2-14b7-46d3-a0e4-715d56682277
            {
                var leave = leavetype.Where(l => l.Id == leaveRequest.LeaveType).FirstOrDefault();
                if (leave != null)
                {
                    leaveRequest.LeaveTypeName = leave.LeaveTypeName;
                }

            }
            else
            {
                leaveRequest.LeaveTypeName = "LOSS OF PAY DAYS";
            }

            DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");
            if (NxtAssEmpId == Guid.Empty)
            {
                sendMailTo = employee.Email;
                //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
                if (!string.IsNullOrEmpty(employee.Email.ToString()))
                {
                    try
                    {
                        //  string sendMailTo = Convert.ToString(leaveRequest.FirstLvlContact);
                        string AssignedemployeeName = Assignedemployee.FirstName + " " + Assignedemployee.LastName + "(" + Assignedemployee.EmployeeCode + ")";

                        string message = lveCompoffApprovedMailBody(employee, leaveRequest, AssignedemployeeName, "ToMsg", leaveRequest.CompOff);
                        string CCmessage = lveCompoffApprovedMailBody(employee, leaveRequest, AssignedemployeeName, "CCmessage", leaveRequest.CompOff);
                        string reqtype = leaveRequest.CompOff == true ? "Comp-Off" : leaveRequest.IsApprvCancel == true ? " Approved Leave Cancel Request " : "Leave";
                        //string reqtype = leaveRequest.CompOff == true ? "Comp-Off" : "Leave";
                        string subject = reqtype + " Approved for " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
                        MailConfig mailConfig2 = new MailConfig(companyId);
                        string[] StrCCMail = mailConfig2.CCMail.Split(',');
                        PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, CCmessage);
                        status = payrolemail.SendTestMail(mailConfig2.IPAddress, mailConfig2.PortNo, mailConfig2.FromEmail, mailConfig2.MailPassword, mailConfig2.EnableSSL);
                        string mailstat = null;
                        if (status)
                        {
                            mailstat = "Mail send succesfully";
                        }
                        else
                        {
                            mailstat = "Mail send failed";
                        }
                        Guid empid = new Guid(Session["EmployeeId"].ToString());
                        mailConfig2.Savemailhistory(companyId, empid, mailConfig2.FromEmail, employee.Email, null, null, reqtype + " Request Approved by First Level Contact", subject, mailstat);
                        return status;
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log(ex);
                        return status;
                    }
                }
                else
                {
                    status = true;
                    return status;
                }
            }
            else
            {
                Employee AssignEmployee = new Employee(NxtAssEmpId);
                Employee PrevLevelEmployee = new Employee(AssEmpId);
                sendMailTo = AssignEmployee.Email;
                //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
                if (!string.IsNullOrEmpty(AssignEmployee.Email.ToString()))
                {
                    try
                    {
                        //  string sendMailTo = Convert.ToString(leaveRequest.FirstLvlContact);

                        string AssignEmployeemanager = PrevLevelEmployee.FirstName + " " + PrevLevelEmployee.LastName + "(" + PrevLevelEmployee.EmployeeCode + ")";
                        //  Company company = new Company(companyId, userid);
                        //string reqType = leaveRequest.CompOff == true ? "Comp-off" : "Leave";
                        string reqType = leaveRequest.CompOff == true ? "Comp-Off" : leaveRequest.IsApprvCancel == true ? " Approved Leave Cancel Request " : "Leave";
                        string subject = reqType + " Approval";
                        string message = NextLevellveCompoffApprovedMailBody(employee, leaveRequest, AssignEmployeemanager, "ToMsg", leaveRequest.CompOff, NxtAssEmpId, prioritynumber, type);
                        string ccmsg = NextLevellveCompoffApprovedMailBody(employee, leaveRequest, AssignEmployeemanager, "CCmessage", leaveRequest.CompOff, NxtAssEmpId, prioritynumber, type);
                        MailConfig mailConfig2 = new MailConfig(companyId);
                        string[] StrCCMail = mailConfig2.CCMail.Split(',');
                        PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, ccmsg);
                        status = payrolemail.SendTestMail(mailConfig2.IPAddress, mailConfig2.PortNo, mailConfig2.FromEmail, mailConfig2.MailPassword, mailConfig2.EnableSSL);
                        string mailstat = null;
                        if (status)
                        {
                            mailstat = "Mail send succesfully";
                        }
                        else
                        {
                            mailstat = "Mail send failed";
                        }
                        Guid empid = new Guid(Session["EmployeeId"].ToString());
                        mailConfig2.Savemailhistory(companyId, empid, mailConfig2.FromEmail, employee.Email, null, null, reqType + " Request Approved by First Level Contact", subject, mailstat);
                        return status;
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log(ex);
                        return status;
                    }
                }
                else
                {
                    status = true;
                    return status;
                }
            }

        }
        public ActionResult LeaveRequestReject(Guid empid, Guid id, string txtReject)
        {
            // string reasonReject = txtReject;
            // LeaveRejectMail(empid,id,txtReject);
            return View("LeaveRequestReject");

        }
        public bool HRApprovedcancelconformmail(Guid employeeId, Guid id, string text, Guid AssEmpId)
        {
            Employee employee = new Employee(employeeId);
            Employee AssignEmployee = new Employee(AssEmpId);
            int companyId = employee.CompanyId;
            bool status = false;
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            string sendMailTo = employee.Email;
            //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
            if (!string.IsNullOrEmpty(employee.Email.ToString()))
            {
                LeaveTypeList leavetype = new LeaveTypeList(companyId);
                LeaveRequest leaveRequest = new LeaveRequest(id, employeeId);

                MailConfig mailConfig = new MailConfig(companyId);
                string smtpServer = mailConfig.IPAddress;
                int portNo = mailConfig.PortNo;
                string fromMail = mailConfig.FromEmail;
                var leave = leavetype.Where(l => l.Id == leaveRequest.LeaveType).FirstOrDefault();
                if (leaveRequest.LeaveType != new Guid(lossofpayid.LOPId.ToString()))//199f5db2-14b7-46d3-a0e4-715d56682277
                {
                    if (leave != null)
                    {
                        leaveRequest.LeaveTypeName = leave.LeaveTypeName;
                    }
                }
                else
                {
                    leaveRequest.LeaveTypeName = "LOSS OF PAY DAYS";
                }

                DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
                var FDate = fromDate.ToString("dd/MM/yyyy");
                DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
                var EDate = endDate.ToString("dd/MM/yyyy");
                if (AssEmpId == Guid.Empty)
                {
                }
                try
                {

                    Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                    string AssignEmployeename = AssignEmployee.FirstName + " " + AssignEmployee.LastName + "(" + AssignEmployee.EmployeeCode + ")";
                    string FromDay = leaveRequest.FromDay == 0 ? "FullDay" : leaveRequest.FromDay == 1 ? "FirstHalf" : "SecondHalf";
                    string ToDay = leaveRequest.ToDay == 0 ? "FullDay" : leaveRequest.ToDay == 1 ? "FirstHalf" : "SecondHalf";

                    string message = " The Leave that you have requested and approved  for the following period has been cancelled  by" + " " + AssignEmployeename + "<br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

                    message += "<tr><td align=center><font color=#2D98eo size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#2D98eo size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#2D98eo size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#2D98eo size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#2D98eo size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#2D98eo size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

                    message += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
                    message += "<td align=center>" + FDate + "</td>";
                    message += "<td align=center>" + EDate + "</td>";
                    message += "<td align=center>" + FromDay + "</td>";
                    message += "<td align=center>" + ToDay + "</td>";
                    message += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
                    //message += "<td align=center>" + leaveRequest.Rejectreason + "</td></tr>";


                    message += "</table>";
                    message += "<font color=#2D98eo size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + leaveRequest.Reason + "<br>";
                    message += "<font color=#2D98eo size=2 face=Verdana><b>Leave Canceled Reason: </b></font>" + leaveRequest.Rejectreason;
                    //message += "</br> Reason for Reject:" + text;
                    // message += "<div></br><a href='#' > Click here to Login</a></div>";
                    //    message += "</br><a href='" + rejectUrl + "' > Click the link to Reject Leave</a>";
                    string subject = "Leave Rejected";


                    MailConfig mailConfig1 = new MailConfig(companyId);
                    string[] StrCCMail = mailConfig1.CCMail.Split(',');
                    PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, message);
                    status = payrolemail.SendTestMail(mailConfig1.IPAddress, mailConfig1.PortNo, mailConfig1.FromEmail, mailConfig1.MailPassword, mailConfig1.EnableSSL);
                    string mailstat = null;
                    if (status)
                    {
                        mailstat = "Mail send succesfully";
                    }
                    else
                    {
                        mailstat = "Mail send failed";
                    }
                    Guid empid = new Guid(Session["EmployeeId"].ToString());
                    mailConfig1.Savemailhistory(companyId, empid, mailConfig1.FromEmail, employee.Email, null, null, "The Leave that you have requested for the following period has been rejected", subject, mailstat);

                    //   dataValue.Save();

                    return status;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    return status;
                }
            }
            else
            {
                status = true;
                return status;
            }
            //return true;
        }
        //--
        public bool LeaveRejectMail(Guid employeeId, Guid id, string text, Guid AssEmpId, string type1)
        {
            Employee employee = new Employee(employeeId);
            Guid sessionid = new Guid(Session["EmployeeId"].ToString());
            Employee AssignEmployee = new Employee(sessionid);
            int companyId = employee.CompanyId;
            bool status = false;
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            string sendMailTo = employee.Email;
            //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
            if (!string.IsNullOrEmpty(employee.Email))
            {
                LeaveTypeList leavetype = new LeaveTypeList(companyId);
                LeaveRequest leaveRequest = new LeaveRequest(id, employeeId);

                MailConfig mailConfig = new MailConfig(companyId);
                string smtpServer = mailConfig.IPAddress;
                int portNo = mailConfig.PortNo;
                string fromMail = mailConfig.FromEmail;
                var leave = leavetype.Where(l => l.Id == leaveRequest.LeaveType).FirstOrDefault();
                if (leaveRequest.LeaveType != new Guid(lossofpayid.LOPId.ToString()))//199f5db2-14b7-46d3-a0e4-715d56682277
                {
                    if (leave != null)
                    {
                        leaveRequest.LeaveTypeName = leave.LeaveTypeName;
                    }
                }
                else
                {
                    leaveRequest.LeaveTypeName = "LOSS OF PAY DAYS";
                }

                DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
                var FDate = fromDate.ToString("dd/MM/yyyy");
                DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
                var EDate = endDate.ToString("dd/MM/yyyy");
                if (AssEmpId == Guid.Empty)
                {
                }
                try
                {

                    Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                    string AssignEmployeename = AssignEmployee.FirstName + " " + AssignEmployee.LastName + "(" + AssignEmployee.EmployeeCode + ")";
                    string message = lveCompoffRejectMailBody(employee, leaveRequest, AssignEmployeename, "ToMsg", leaveRequest.CompOff);
                    string CCmessage = lveCompoffRejectMailBody(employee, leaveRequest, AssignEmployeename, "CCmessage", leaveRequest.CompOff);
                    string type = leaveRequest.CompOff == true ? "Comp-off" : "Leave";
                    string subject = type + " Rejected for " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";


                    MailConfig mailConfig1 = new MailConfig(companyId);
                    string[] StrCCMail = mailConfig1.CCMail.Split(',');
                    PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, CCmessage);
                    status = payrolemail.SendTestMail(mailConfig1.IPAddress, mailConfig1.PortNo, mailConfig1.FromEmail, mailConfig1.MailPassword, mailConfig1.EnableSSL);
                    string mailstat = null;
                    if (status)
                    {
                        mailstat = "Mail send succesfully";
                    }
                    else
                    {
                        mailstat = "Mail send failed";
                    }
                    Guid empid = new Guid(Session["EmployeeId"].ToString());
                    mailConfig1.Savemailhistory(companyId, empid, mailConfig1.FromEmail, employee.Email, null, null, "The " + type + " that you have requested for the following period has been rejected", subject, mailstat);

                    //   dataValue.Save();

                    return status;
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    return status;
                }
            }
            else
            {
                status = true;
                return status;
            }
            //return true;
        }

        public string lveCompoffRejectMailBody(Employee employee, LeaveRequest leaveRequest, string AssignEmployeename, string msgType, bool compoff)
        {
            string returnval = string.Empty;
            DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");

            string leavetype = compoff == true ? "Comp-Off" : "Leave";
            Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
            string RequestedEmployeename = employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
            string FromDay = leaveRequest.FromDay == 0 ? "FullDay" : leaveRequest.FromDay == 1 ? "FirstHalf" : "SecondHalf";
            string ToDay = leaveRequest.ToDay == 0 ? "FullDay" : leaveRequest.ToDay == 1 ? "FirstHalf" : "SecondHalf";
            if (msgType == "CCmessage")
            {
                string CCmessage = " The " + leavetype + " that " + RequestedEmployeename + " have requested for the following period has been rejected by" + " " + AssignEmployeename + "<br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";
                CCmessage += "<tr><td align=center><font color=#FF0000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";
                CCmessage += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
                CCmessage += "<td align=center>" + FDate + "</td>";
                CCmessage += "<td align=center>" + EDate + "</td>";
                CCmessage += "<td align=center>" + FromDay + "</td>";
                CCmessage += "<td align=center>" + ToDay + "</td>";
                CCmessage += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
                //CCmessage += "<td align=center>" + leaveRequest.Rejectreason + "</td></tr>";
                CCmessage += "</table>";
                CCmessage += "<font color=#FF0000 size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + leaveRequest.Reason + "<br>";
                CCmessage += "<font color=#FF0000 size=2 face=Verdana><b>Leave Rejected Reason: </b></font>" + leaveRequest.Rejectreason;
                returnval = CCmessage;
            }
            else
            {
                string message = " The " + leavetype + " that you have requested for the following period has been rejected by" + " " + AssignEmployeename + "<br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";
                message += "<tr><td align=center><font color=#FF0000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";
                message += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
                message += "<td align=center>" + FDate + "</td>";
                message += "<td align=center>" + EDate + "</td>";
                message += "<td align=center>" + FromDay + "</td>";
                message += "<td align=center>" + ToDay + "</td>";
                message += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
                //message += "<td align=center>" + leaveRequest.Rejectreason + "</td></tr>";
                message += "</table>";
                message += "<font color=#FF0000 size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + leaveRequest.Reason + "<br>";
                message += "<font color=#FF0000 size=2 face=Verdana><b>Leave Rejected Reason: </b></font>" + leaveRequest.Rejectreason;
                returnval = message;
            }


            return returnval;
        }

        public string lveCompoffApprovedMailBody(Employee employee, LeaveRequest leaveRequest, string AssignEmployeename, string msgType, bool compoff)
        {
            string returnval = string.Empty;
            Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
            DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");
            //  Company company = new Company(companyId, userid);
            string FromDay = leaveRequest.FromDay == 0 ? "FullDay" : leaveRequest.FromDay == 1 ? "FirstHalf" : "SecondHalf";
            string ToDay = leaveRequest.ToDay == 0 ? "FullDay" : leaveRequest.ToDay == 1 ? "FirstHalf" : "SecondHalf";
            string leavetype = compoff == true ? "Comp-Off" : "Leave";

            if (msgType == "CCmessage")
            {
                string CCmessage = "<b> " + leavetype + " Request by " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ") was Approved succesfully by " + AssignEmployeename + " </b> <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";
                CCmessage += "<tr><td align=center><font color=#008000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";
                CCmessage += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
                CCmessage += "<td align=center>" + FDate + "</td>";
                CCmessage += "<td align=center>" + EDate + "</td>";
                CCmessage += "<td align=center>" + FromDay + "</td>";
                CCmessage += "<td align=center>" + ToDay + "</td>";
                CCmessage += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
                CCmessage += "</table>";
                CCmessage += "<font color=#008000 size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + leaveRequest.Reason;
                returnval = CCmessage;
            }
            else
            {
                string message = "<b> " + leavetype + " Request was Approved by " + AssignEmployeename + "</b> <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";
                message += "<tr><td align=center><font color=#008000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";
                message += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
                message += "<td align=center>" + FDate + "</td>";
                message += "<td align=center>" + EDate + "</td>";
                message += "<td align=center>" + FromDay + "</td>";
                message += "<td align=center>" + ToDay + "</td>";
                message += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
                message += "</table>";
                message += "<font color=#008000 size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + leaveRequest.Reason;
                returnval = message;
            }
            return returnval;
        }

        public string NextLevellveCompoffApprovedMailBody(Employee employee, LeaveRequest leaveRequest, string AssignEmployeemanager, string msgType, bool compoff, Guid NxtAssEmpId, int prioritynumber, string type)
        {
            string returnval = string.Empty;

            DateTime fromDate = Convert.ToDateTime(leaveRequest.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(leaveRequest.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");
            string appliedemployee = employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
            string FromDay = leaveRequest.FromDay == 0 ? "FullDay" : leaveRequest.FromDay == 1 ? "FirstHalf" : "SecondHalf";
            string ToDay = leaveRequest.ToDay == 0 ? "FullDay" : leaveRequest.ToDay == 1 ? "FirstHalf" : "SecondHalf";
            string approveUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString();
            string actionApproveLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + NxtAssEmpId + "&Leaveid=" + leaveRequest.Id + "&assgnmgrid=" + NxtAssEmpId + "&prioritynum=" + prioritynumber + "&AorRstat=1 &status=1 &Navtab=" + type;
            string actionRejectLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + NxtAssEmpId + "&Leaveid=" + leaveRequest.Id + "&assgnmgrid=" + NxtAssEmpId + "&prioritynum=" + prioritynumber + "&AorRstat=2 &status=2 &Navtab=" + type;
            string leavetype = compoff == true ? "Comp-Off" : "Leave";
            // string rejectUrl = "http://localhost:52993/LeaveRequest/LeaveRequestReject?empid=" + dataValue.EmployeeId + "&id=" + dataValue.Id;
            // string message = "<div style='border: 1px solid green;background-color: lightgrey; padding: 25px;margin: 25px;'>";
            string message = "<b> Employee Name : </b>" + " " + appliedemployee + "<br/><b>  Previous " + leavetype + " Approved By : </b>" + " " + AssignEmployeemanager + "  <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

            message += "<tr><td align=center><font color=#008000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>From Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>To Day</b></font></td><td align=center><font color=#008000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

            message += "<tr><td align=center>" + leaveRequest.LeaveTypeName + "</td>";
            message += "<td align=center>" + FDate + "</td>";
            message += "<td align=center>" + EDate + "</td>";
            message += "<td align=center>" + FromDay + "</td>";
            message += "<td align=center>" + ToDay + "</td>";
            message += "<td align=center>" + leaveRequest.NoOfDays + "</td></tr>";
            //message += "<td align=center>" + leaveRequest.Reason + "</td></tr>";
            //   message += "<tr><td><font color=red size=2 face=Verdana>Reason -</font></td><td align=left colspan=5>&nbsp;" +  + "</td></tr>";

            message += "</table>";
            message += "<font color=#008000 size=2 face=Verdana><b> " + leavetype + " Requested Reason: </b></font>" + leaveRequest.Reason;
            if (msgType == "CCmessage")
            {
                returnval = message;

            }
            else
            {
                message += "<div></br><a href='" + actionApproveLeave + "' > Approve</a><a href='" + actionRejectLeave + "' > Reject</a></div>";
                message += "<div></br><a href='" + approveUrl + "' > Click Here to Sign in to the Application </a></div>";
                returnval = message;
            }

            return returnval;
        }


        public JsonResult CancelRequest(jsonLeaveEnty dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["UserId"]);
            //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
            if (!string.IsNullOrEmpty(dataValue.FirstLvlContact))
            {
                DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
                Employee employee = new Employee(dataValue.EmployeeId);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                LeaveTypeList leavetype = new LeaveTypeList(companyId);
                AssignManager assignManager = new AssignManager(dataValue.EmployeeId, companyId, 0, DefaultFinancialYr.Id);
                LeaveRequest leaveRequest = new LeaveRequest(dataValue.Id);
                var leave = leavetype.Where(l => l.Id == dataValue.LeaveType).FirstOrDefault();
                if (leave != null)
                {
                    if (leaveRequest.LeaveType != new Guid(lossofpayid.LOPId.ToString()))//199f5db2-14b7-46d3-a0e4-715d56682277
                    {
                        dataValue.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        leaveRequest.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                }
                else
                {
                    return BuildJsonResult(true, 200, "Select The Leave Type.", dataValue);
                }

                DateTime fromDate = Convert.ToDateTime(dataValue.FromDate);
                var FDate = fromDate.ToString("dd/MM/yyyy");
                DateTime endDate = Convert.ToDateTime(dataValue.EndDate);
                var EDate = endDate.ToString("dd/MM/yyyy");
                // string path
                try
                {

                    //  dataValue.Save();
                    string sendMailTo = dataValue.FirstLvlContact;
                    string reqType = leaveRequest.CompOff == true ? "Comp-off" : "Leave";
                    Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                    string FromDay = leaveRequest.FromDay == 0 ? "FullDay" : leaveRequest.FromDay == 1 ? "FirstHalf" : "SecondHalf";
                    string ToDay = leaveRequest.ToDay == 0 ? "FullDay" : leaveRequest.ToDay == 1 ? "FirstHalf" : "SecondHalf";
                    //Guid amId = assignManager.EmployeeId;
                    var AssignMgrId = assignManager.Where(ass => ass.MgrPriority == 1).FirstOrDefault();
                    Guid amId = AssignMgrId.AssEmpId;
                    Company company = new Company(companyId, userid);
                    MailConfig mailConfig = new MailConfig(companyId);
                    string smtpServer = mailConfig.IPAddress;
                    int portNo = mailConfig.PortNo;
                    string fromMail = mailConfig.FromEmail;
                    //    String halfFullDay = dataValue.HalfFullDay == 0 ? "HalfDay" : "FullDay";

                    string message = employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ") has cancelled the " + reqType + " that he/she applied for following period : <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

                    message += "<tr><td align=center><font color=#FF0000 size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>FromDay</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>ToDay</b></font></td><td align=center><font color=#FF0000 size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

                    message += "<tr><td align=center>" + dataValue.LeaveTypeName + "</td>";
                    message += "<td align=center>" + FDate + "</td>";
                    message += "<td align=center>" + EDate + "</td>";
                    message += "<td align=center>" + FromDay + "</td>";
                    message += "<td align=center>" + ToDay + "</td>";
                    message += "<td align=center>" + dataValue.NoOfDays + "</td></tr>";
                    //message += "<td align=center>" + dataValue.Rejectreason + "</td></tr>";


                    message += "</table>";
                    message += "<font color=#FF0000 size=2 face=Verdana><b>" + reqType + " Requested Reason: </b></font>" + dataValue.Reason + "<br>";
                    message += "<font color=#FF0000 size=2 face=Verdana><b>" + reqType + " Canceled Reason: </b></font>" + dataValue.Rejectreason;

                    string subject = reqType + " Cancellation by " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";

                    MailConfig mailConfig3 = new MailConfig(companyId);
                    string[] StrCCMail = mailConfig3.CCMail.Split(',');
                    PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, message);
                    string mailstat = null;
                    bool status = false;
                    status = payrolemail.SendTestMail(mailConfig3.IPAddress, mailConfig3.PortNo, mailConfig3.FromEmail, mailConfig3.MailPassword, mailConfig3.EnableSSL);
                    if (status)
                    {
                        mailstat = "Mail send succesfully";
                    }
                    else
                    {
                        mailstat = "Mail send failed";
                    }
                    Guid empid = new Guid(Session["EmployeeId"].ToString());
                    mailConfig3.Savemailhistory(companyId, empid, mailConfig3.FromEmail, employee.Email, null, null, "cancelled the leave", subject, mailstat);

                    //payrolemail.SendTestMail(smtpServer, portNo, fromMail);
                    if (leaveRequest.Status == 0)
                    {

                    }

                    return BuildJsonResult(true, 200, "Data Saved Successfully", dataValue);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    return BuildJsonResult(false, 200, "Error while saving data", null);
                }
            }
            else
            {
                return BuildJsonResult(true, 200, "Data Saved Successfully", dataValue);
            }
        }
        public JsonResult GetEmpLeaveReport(Guid EmployeeId, int status)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            // int a =Convert.ToInt32 (Session["CompanyId"]);
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //  LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, dataValue.FinanceYear, dataValue.Status);
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {


                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId, DefaultFinancialYr.Id);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }

                });
                LeaveRequestList lev = new LeaveRequestList();
                leav.ForEach(n =>
                {
                    if (n.Status == status)
                    {
                        if (n.Status == 2 || n.Status == 3)
                        {
                            n.Reason = n.Rejectreason;
                        }
                        lev.Add(n);

                    }
                });
                return BuildJsonResult(true, 200, "", lev);
            }
            else
            {
                Guid financialyr = new Guid((DefaultFinancialYr.Id).ToString());
                LeaveRequestList leav1 = new LeaveRequestList(empid, companyId, 0, financialyr);
                for (int n = 0; n < leav1.Count; n++)
                {

                    int priornum = leav1[n].prioritynumber;
                    Guid reqid = leav1[n].Id;
                    LeaveRequestList leav2 = new LeaveRequestList(reqid, priornum, 0);
                    if (leav2.Count != 0)
                    {
                        int stat = leav2[0].MANAGERStatus;
                        if (stat == 1)
                        {
                            LevFinalList.Add(leav1[n]);
                            //leav1.ForEach{ }
                            //LeaveRequest requests = new LeaveRequest();
                            //this.Add(requests);
                        }
                    }
                    else
                    {
                        LevFinalList.Add(leav1[n]);
                    }
                }

                LevFinalList.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                });
                return BuildJsonResult(true, 200, "", LevFinalList);
            }


        }


        public JsonResult GetAppcanlevrepdata(Guid EmployeeId, string type)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            // int a =Convert.ToInt32 (Session["CompanyId"]);
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //  LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, dataValue.FinanceYear, dataValue.Status);
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {
                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId, DefaultFinancialYr.Id, type);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }

                });
                LeaveRequestList lev = new LeaveRequestList();
                leav.ForEach(n =>
                {
                    if (n.Status == 2)
                    {
                        n.Reason = n.Rejectreason;
                    }
                    lev.Add(n);
                });
                return BuildJsonResult(true, 200, "", lev);
            }
            else
            {
                Guid financialyr = new Guid((DefaultFinancialYr.Id).ToString());
                LeaveRequestList leav1 = new LeaveRequestList(empid, companyId, 0, financialyr);
                for (int n = 0; n < leav1.Count; n++)
                {

                    int priornum = leav1[n].prioritynumber;
                    Guid reqid = leav1[n].Id;
                    LeaveRequestList leav2 = new LeaveRequestList(reqid, priornum, 0);
                    if (leav2.Count != 0)
                    {
                        int stat = leav2[0].MANAGERStatus;
                        if (stat == 1)
                        {
                            LevFinalList.Add(leav1[n]);
                            //leav1.ForEach{ }
                            //LeaveRequest requests = new LeaveRequest();
                            //this.Add(requests);
                        }
                    }
                    else
                    {
                        LevFinalList.Add(leav1[n]);
                    }
                }

                LevFinalList.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                });
                return BuildJsonResult(true, 200, "", LevFinalList);
            }


        }





        public JsonResult GetCompoffGainReport(Guid EmployeeId, string type)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            // int a =Convert.ToInt32 (Session["CompanyId"]);
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //  LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, dataValue.FinanceYear, dataValue.Status);
            LeaveRequestList lev = new LeaveRequestList();
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {
                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId, DefaultFinancialYr.Id, type);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }

                });

                leav.ForEach(n =>
                {
                    if (n.Status == 2)
                    {
                        n.Reason = n.Rejectreason;
                    }
                    lev.Add(n);
                });

            }
            return BuildJsonResult(true, 200, "", lev);
        }
        public JsonResult NonemployeeroleGetCompoffGainReport(Guid EmployeeId, string type, Guid Finyr)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequestList lev = new LeaveRequestList();
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {
                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId, Finyr, type);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }

                });

                leav.ForEach(n =>
                {
                    if (n.Status == 2)
                    {
                        n.Reason = n.Rejectreason;
                    }
                    lev.Add(n);
                });

            }
            return BuildJsonResult(true, 200, "", lev);
        }
        public JsonResult GetPendingStatReport(Guid Id)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                if (DefaultFinancialYr.Id == Guid.Empty)
                {
                    return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
                }
                AssignManager PendLevStat = new AssignManager(Id, companyId, DefaultFinancialYr.Id);
                List<JsonAssignMgrML> jsondata = new List<JsonAssignMgrML>();
                PendLevStat.ForEach(u => { jsondata.Add(JsonAssignMgrML.toJson(u)); });
                return base.BuildJson(true, 200, "success", jsondata);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "Error While Retriving Data.See ErrorLog");
            }


        }


        public JsonResult GetPendingStatReportwithoutfinyear(Guid ParentId)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                if (DefaultFinancialYr.Id == Guid.Empty)
                {
                    return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
                }
                AssignManager PendLevStat = new AssignManager(ParentId, companyId, DefaultFinancialYr.Id);
                List<JsonAssignMgrML> jsondata = new List<JsonAssignMgrML>();
                PendLevStat.ForEach(u => { jsondata.Add(JsonAssignMgrML.toJson(u)); });
                return base.BuildJson(true, 200, "success", jsondata);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "Error While Retriving Data.See ErrorLog");
            }


        }



        public JsonResult GetLeaveRequests(LeaveRequest dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveRequestList leav = new LeaveRequestList(companyId, dataValue.FinanceYear, dataValue.Status);


            return BuildJsonResult(true, 200, "", leav);

        }




        public JsonResult DeleteLeaveRequests(Guid GID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveRequest req = new LeaveRequest();
            req.Id = GID;
            //req.ModifiedBy = dataValue.ModifiedBy;
            if (req.Delete())
            {
                return BuildJsonResult(true, 200, "Deleted Successfully", null);
            }
            else
            {
                return BuildJsonResult(false, 200, "There is some error while deleting", null);
            }
        }

        public JsonResult GetAssignManagerIssue(DateTime FromDate, DateTime ToDate, Guid EmployeeId, int LeaveStat)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                Guid LoggedUserId = new Guid(Session["EmployeeId"].ToString());
                LeaveRequestList levReqLst = new LeaveRequestList(FromDate, ToDate, EmployeeId, LeaveStat, DefaultFinancialYr.Id, LoggedUserId);
                return base.BuildJson(true, 200, "Data Bind SuccessFully", levReqLst);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Error While fetching data", ex.Message);
            }

        }
        public JsonResult GetLeaveReport(DateTime FromDate, DateTime ToDate, int LeaveStat)
        {

            try
            {
                int companyId1 = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialYr1 = new LeaveFinanceYear(companyId1);
                Guid LoggedUserId = new Guid(Session["EmployeeId"].ToString());
                LeaveRequestList leaveRequestLists = new LeaveRequestList(FromDate, ToDate, LoggedUserId, LeaveStat, companyId1);
                LeaveRequestList levReqLst = leaveRequestLists;
                return base.BuildJson(true, 200, "Data Bind SuccessFully", levReqLst);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Error While fetching data", ex.Message);
            }
        }
        public JsonResult ExportLeaveReport(DateTime FromDate, DateTime ToDate, int LeaveStatus, string Employeeid, string ReportType)
        {
            try
            {
                int companyId1 = Convert.ToInt32(Session["CompanyId"]);
                LeaveRequest LevReq = new LeaveRequest();
                Guid employeeid, LoggedUser;
                DataTable dtt = new DataTable();
                LeaveFinanceYear DefaultFinancialYr1 = new LeaveFinanceYear(companyId1);
                if (ReportType == "MyReport")
                {
                    employeeid = new Guid(Session["EmployeeId"].ToString());
                    dtt = LevReq.GetMYREPORTLeaveReport(FromDate, ToDate, employeeid, LeaveStatus, companyId1);
                }
                else if (ReportType == "HrReport")
                {
                    employeeid = new Guid(Employeeid);
                    dtt = LevReq.GetMYREPORTLeaveReport(FromDate, ToDate, employeeid, LeaveStatus, companyId1);
                }
                else if (ReportType == "AssignReport")
                {
                    employeeid = new Guid(Employeeid);
                    LoggedUser = new Guid(Session["EmployeeId"].ToString());
                    dtt = LevReq.GetAssignManagerViewReport(FromDate, ToDate, employeeid, LeaveStatus, DefaultFinancialYr1.Id, LoggedUser);
                }

                List<jsonLeaveReportExport> result = new List<jsonLeaveReportExport>();
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    LeaveRequest LevReqLst = new LeaveRequest();
                    jsonLeaveReportExport LevExp = new jsonLeaveReportExport();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["EmployeeCode"])))
                        LevReqLst.Empcode = Convert.ToString(dtt.Rows[i]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["EmployeeName"])))
                        LevReqLst.EmployeeName = Convert.ToString(dtt.Rows[i]["EmployeeName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["FromDate"])))
                        LevReqLst.FromDate = Convert.ToDateTime(dtt.Rows[i]["FromDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["fromday"])))
                        LevReqLst.FromDay = Convert.ToInt32(dtt.Rows[i]["fromday"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["EndDate"])))
                        LevReqLst.EndDate = Convert.ToDateTime(dtt.Rows[i]["EndDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["today"])))
                        LevReqLst.ToDay = Convert.ToInt32(dtt.Rows[i]["today"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["noofdays"])))
                        LevReqLst.NoOfDays = Convert.ToString(dtt.Rows[i]["noofdays"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["LeaveName"])))
                        LevReqLst.LeaveTypeName = Convert.ToString(dtt.Rows[i]["LeaveName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtt.Rows[i]["LeaveStatus"])))
                        LevReqLst.LeaveStatus = Convert.ToString(dtt.Rows[i]["LeaveStatus"]);
                    result.Add(LevExp.tojson(LevReqLst));

                }
                DataTable dt1 = LevReq.ToDataTable(result);
                if (dt1.Rows.Count != 0)
                {
                    GridView GridView1 = new GridView();
                    GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell1 = new TableCell();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt1;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GridView1.RenderControl(hw);
                    string ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "LeaveReport.xls";
                    //string ExcelFilePath = "D://Development//";
                    string renderedGridView = sw.ToString();
                    System.IO.File.WriteAllText(ExcelFilePath, renderedGridView);


                    return base.BuildJson(true, 200, "Leave Report Exported successfully", ExcelFilePath);
                }
                else
                {
                    return base.BuildJson(false, 100, "No Data Available ", "");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Error While Fetching Data ", ex.Message);
            }


        }
        public JsonResult ExportLeaveBalanceForAllEmployee()
        {
            try
            {
                int company = Convert.ToInt32(Session["CompanyId"]);
                LeaveRequest LevRequest = new LeaveRequest();
                Guid EmployeeId = Guid.Empty;
                DataTable dtvalue = new DataTable();
                LeaveFinanceYear DefaultFinancialYr1 = new LeaveFinanceYear(company);
                dtvalue = DefaultFinancialYr1.GetemployeeLeaveBalanceReport(EmployeeId, DefaultFinancialYr1.Id);

                List<jsonLeaveBalanceReportExport> resultExport = new List<jsonLeaveBalanceReportExport>();
                for (int i = 0; i < dtvalue.Rows.Count; i++)
                {
                    LeaveRequest LevReqLst = new LeaveRequest();
                    jsonLeaveBalanceReportExport LevExp = new jsonLeaveBalanceReportExport();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["EmployeeCode"])))
                        LevReqLst.Empcode = Convert.ToString(dtvalue.Rows[i]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["Name"])))
                        LevReqLst.EmployeeName = Convert.ToString(dtvalue.Rows[i]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["Leave Name"])))
                        LevReqLst.LeaveTypeName = Convert.ToString(dtvalue.Rows[i]["Leave Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["LeaveOpening"])))
                        LevReqLst.Leaveopen = Convert.ToDouble(dtvalue.Rows[i]["LeaveOpening"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["LeaveCredit"])))
                        LevReqLst.Leavecred = Convert.ToDouble(dtvalue.Rows[i]["LeaveCredit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["TotalLeave"])))
                        LevReqLst.Totaldays = Convert.ToDouble(dtvalue.Rows[i]["TotalLeave"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["Leaveused"])))
                        LevReqLst.Useddays = Convert.ToDouble(dtvalue.Rows[i]["Leaveused"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["Debitdays"])))
                        LevReqLst.Debitdays = Convert.ToDouble(dtvalue.Rows[i]["Debitdays"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["leaveAvailable"])))
                        LevReqLst.LeaveBalance = Convert.ToDouble(dtvalue.Rows[i]["leaveAvailable"]);

                    resultExport.Add(LevExp.toExportjson(LevReqLst));

                }
                DataTable dtvalue1 = LevRequest.ToDataTable(resultExport);
                if (dtvalue1.Rows.Count != 0)
                {
                    GridView GridView1 = new GridView();
                    GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell1 = new TableCell();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtvalue1;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GridView1.RenderControl(hw);
                    string ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "LeaveBalanceAndOpening.xls";
                    //string ExcelFilePath = "D://Development//";
                    string renderedGridView = sw.ToString();
                    System.IO.File.WriteAllText(ExcelFilePath, renderedGridView);


                    return base.BuildJson(true, 200, "Leave Report Exported successfully", ExcelFilePath);
                }
                else
                {
                    return base.BuildJson(false, 100, "No Data Available ", "");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Error While Fetching Data ", ex.Message);
            }


        }









        public JsonResult GetHRviewReport(DateTime FromDate, DateTime ToDate, Guid EmployeeId, int LeaveStat)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                Guid LoggedUserId = new Guid(Session["EmployeeId"].ToString());
                LeaveRequestList levReqLst = new LeaveRequestList(FromDate, ToDate, EmployeeId, LeaveStat, companyId);
                return base.BuildJson(true, 200, "Data Bind SuccessFully", levReqLst);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Error While fetching data", ex.Message);
            }

        }
        public JsonResult GetEmpLeaveRequest(Guid EmployeeId, string Reqtype)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            List<LeaveRequest> ReqLst = new List<LeaveRequest>();
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {


                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId, Reqtype);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                    if (l.Status == 0)
                    {
                        ReqLst.Add(l);
                    }

                });

                return BuildJsonResult(true, 200, "", ReqLst);
            }
            else
            {
                Guid financialyr = new Guid((DefaultFinancialYr.Id).ToString());
                LeaveRequestList leav1 = new LeaveRequestList(empid, companyId, 0, financialyr, Reqtype);
                for (int n = 0; n < leav1.Count; n++)
                {

                    int priornum = leav1[n].prioritynumber;
                    Guid reqid = leav1[n].Id;
                    LeaveRequestList leav2 = new LeaveRequestList(reqid, priornum, 0);
                    if (leav2.Count != 0)
                    {
                        int stat = leav2[0].MANAGERStatus;
                        if (stat == 1)
                        {
                            LevFinalList.Add(leav1[n]);
                        }
                    }
                    else
                    {
                        LevFinalList.Add(leav1[n]);
                    }
                }

                LevFinalList.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                    if (l.Status == 0)
                    {
                        ReqLst.Add(l);
                    }
                });
                return BuildJsonResult(true, 200, "", ReqLst);
            }


        }

        public JsonResult HRGetEmpLeaveRequest(Guid EmployeeId, string Selectiontype)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = EmployeeId;
            List<LeaveRequest> LevFinalListmanager = new List<LeaveRequest>();
            List<LeaveRequest> LevFinalListEmployee = new List<LeaveRequest>();
            // int a =Convert.ToInt32 (Session["CompanyId"]);
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //  LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, dataValue.FinanceYear, dataValue.Status);
            Guid financialyr = new Guid((DefaultFinancialYr.Id).ToString());
            LeaveRequestList leav1 = new LeaveRequestList(empid, companyId, 0, financialyr);
            if (Selectiontype == "Manager")
            {


                for (int n = 0; n < leav1.Count; n++)
                {

                    int priornum = leav1[n].prioritynumber;
                    Guid reqid = leav1[n].Id;
                    LeaveRequestList leav2 = new LeaveRequestList(reqid, priornum, 0);
                    if (leav2.Count != 0)
                    {
                        int stat = leav2[0].MANAGERStatus;
                        if (stat == 1)
                        {
                            LevFinalListmanager.Add(leav1[n]);
                            //leav1.ForEach{ }
                            //LeaveRequest requests = new LeaveRequest();
                            //this.Add(requests);
                        }
                    }
                    else
                    {
                        LevFinalListmanager.Add(leav1[n]);
                    }
                }

                LevFinalListmanager.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }
                });
                return BuildJsonResult(true, 200, "", LevFinalListmanager);
            }
            else
            {
                LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId);
                leav.ForEach(l =>
                {
                    var leave = leavetype.Where(lt => lt.Id == l.LeaveType).FirstOrDefault();
                    if (leave != null)
                    {
                        l.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        l.LeaveTypeName = "LOSS OF PAY DAYS";
                    }

                });
                for (int m = 0; m < leav.Count; m++)
                {
                    if (leav[m].Status == 0)
                    {
                        LevFinalListEmployee.Add(leav[m]);
                    }
                }


                return BuildJsonResult(true, 200, "", LevFinalListEmployee);

            }




        }
        public JsonResult GetLeaveRequest(Guid LeaveReqId)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveRequest leav = new LeaveRequest(LeaveReqId);
            leav.CompanyId = companyId;
            return BuildJsonResult(true, 200, "", jsonLeaveEnty.fromDB(leav));

        }
        //----------------------------------
        public JsonResult CheckforAssignedmanagereditordelete(Guid EmployeeId)
        {
            DataTable avacnt = new DataTable();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            Tableleave REQCNT = new Tableleave();
            avacnt = REQCNT.GetAvailableleavecount(EmployeeId);
            DataTable avacnt1 = new DataTable();
            if (avacnt.Rows.Count > 0)
            {
                for (int i = 0; i < avacnt.Rows.Count; i++)
                {
                    if (avacnt.Rows[i]["FinanceYear"].ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        avacnt1.Rows.Add(avacnt.Rows[i]);
                    }
                }
            }
            if (avacnt1.Rows.Count == 0)
            {
                return BuildJsonResult(true, 200, "", "");
            }
            else
            {
                return BuildJsonResult(false, 200, "", "");
            }

        }

        public JsonResult GetLeaveBalance(Guid EmployeeId, Guid LeaveType)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequest leav = new LeaveRequest(LeaveType, EmployeeId, companyId, DefaultFinancialYr.Id);
            leav.CompanyId = companyId;
            LeaveRequest debitbal = new LeaveRequest(LeaveType, EmployeeId, companyId, DefaultFinancialYr.Id, 0);
            leav.LeaveBalance = leav.LeaveBalance - debitbal.Debitdays;
            LeaveRequest LeaverequestBO = new LeaveRequest();
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);
            Employee employee = new Employee(EmployeeId);
            string LeaveConfigurationparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].leaveparameter, employee);
            string[] LCvalues = LeaveConfigurationparameterid.Split(',');
            for (int i = 0; i < LCvalues.Length; i++)
            {
                LCvalues[i] = LCvalues[i].Trim();
            }
            if (LCvalues[1] != "")
            {
                LeaveConfigurationparameterid = LCvalues[1].ToString();
            }
            else
            {
                LeaveConfigurationparameterid = "00000000-0000-0000-0000-000000000000";
            }
            LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialYr.Id, LeaveType, new Guid(LeaveConfigurationparameterid));
            if (Levconfiglist.Count != 0)
            {
                leav.IsAttachReq = Levconfiglist[0].Isattachreq;
            }
            return BuildJsonResult(true, 200, "", jsonLeaveEnty.fromDB(leav));

        }
        public JsonResult CompoffEntrySave(jsonLeaveEnty dataValue)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id == Guid.Empty)
            {
                return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
            }
            AssignManager objAssMgrLst = new AssignManager(dataValue.EmployeeId, companyId, 0, DefaultFinancialYr.Id);
            if (objAssMgrLst.Count <= 0)
            {
                return BuildJsonResult(false, 200, "Please set the Assign manager before you Request", null);
            }
            MailConfig mailConfig = new MailConfig(companyId);
            if (string.IsNullOrEmpty(mailConfig.IPAddress) || string.IsNullOrEmpty(Convert.ToString(mailConfig.PortNo)) || string.IsNullOrEmpty(mailConfig.FromEmail))
            {
                return BuildJsonResult(false, 200, "Please Set the Mail Configuration", null);
            }
            var selectedDates = new List<DateTime?>();
            var AppliedDates = new List<DateTime?>();
            DateTime fromDate = Convert.ToDateTime(dataValue.FromDate, System.Globalization.CultureInfo.InvariantCulture);
            dataValue.FromDate = fromDate.ToString("dd/MMM/yyyy");

            DateTime endDate = Convert.ToDateTime(dataValue.EndDate, System.Globalization.CultureInfo.InvariantCulture);
            dataValue.EndDate = endDate.ToString("dd/MMM/yyyy");


            for (var date = Convert.ToDateTime(dataValue.FromDate); date <= Convert.ToDateTime(dataValue.EndDate); date = date.AddDays(1))
            {
                selectedDates.Add(date);
            }
            LeaveRequest levReqObj = new LeaveRequest();
            DataTable DTAlreadyAppliedDated = levReqObj.getalreadyapplieddates(dataValue.EmployeeId, companyId, DefaultFinancialYr.Id);
            if (DTAlreadyAppliedDated.Rows.Count != 0)
            {
                for (int i = 0; i <= DTAlreadyAppliedDated.Rows.Count - 1; i++)
                {
                    for (var date = Convert.ToDateTime(DTAlreadyAppliedDated.Rows[i]["FromDate"]); date <= Convert.ToDateTime(DTAlreadyAppliedDated.Rows[i]["EndDate"]); date = date.AddDays(1))
                    {
                        AppliedDates.Add(date);
                    }
                }


            }
            LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, companyId, "compoff");
            Guid fin = DefaultFinancialYr.Id;
            leav.RemoveAll(r => r.FinanceYear != fin);

            leav.RemoveAll(r => r.Status != 0 && r.Status != 1);

            LeaveRequestList floatingHoliday = new LeaveRequestList(dataValue.EmployeeId, companyId, "FH");
             floatingHoliday.RemoveAll(fh => fh.FinanceYear != fin);

            floatingHoliday.RemoveAll(fh => fh.Status != 1);


            var dates = new List<DateTime>();
            leav.ForEach(f =>
            {

                for (var dt = f.FromDate; dt <= f.EndDate; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                }

            });



            var datesnow = new List<DateTime>();

            for (var dts = Convert.ToDateTime(dataValue.FromDate); dts <= Convert.ToDateTime(dataValue.EndDate); dts = dts.AddDays(1))
            {
                datesnow.Add(dts);
            }


            var newData = dates.Select(i => i).Intersect(datesnow);


            if (newData.ToList().Count() > 0)
            {
                return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
            }


            var SelectedVSAppliedList = selectedDates.Except(AppliedDates).ToList();
            if (SelectedVSAppliedList.Count != selectedDates.Count)
            {
                return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
            }
            //To get the Leave type name from the List.
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            var temp = leavetype.Where(l => l.Id == dataValue.LeaveType).FirstOrDefault();
            if (!object.ReferenceEquals(temp, null) || temp != null)
            {
                dataValue.LeaveTypeName = temp.LeaveTypeName;
            }

            levReqObj.Id = new Guid();
            levReqObj.FinanceYear = DefaultFinancialYr.Id;
            levReqObj.EmployeeId = dataValue.EmployeeId;
            levReqObj.FromDate = Convert.ToDateTime(dataValue.FromDate);
            levReqObj.EndDate = Convert.ToDateTime(dataValue.EndDate);
            levReqObj.LeaveType = dataValue.LeaveType;
            levReqObj.FromDay = dataValue.FromDay;
            levReqObj.ToDay = dataValue.ToDay;
            levReqObj.Reason = dataValue.Reason;
            levReqObj.Contact = dataValue.Contact;
            levReqObj.NoOfDays = dataValue.NoOfDays;
            levReqObj.Status = dataValue.Status;
            levReqObj.Rejectreason = dataValue.Rejectreason;
            levReqObj.IsDeleted = false;
            levReqObj.FirstLvlContact = dataValue.FirstLvlContact;
            levReqObj.Childid = Guid.Empty;
            levReqObj.childflag = 0;
            levReqObj.compid = companyId;
            levReqObj.HRapprovalflag = 0;
            levReqObj.HRentrystatus = 0;



            Employee employee = new Employee(dataValue.EmployeeId);
            LeaveRequestModel comoff = new LeaveRequestModel();
            // Comp-off gain histroy save
            DataTable dtCompoffparameter = comoff.compoffparametercheck(levReqObj, companyId, employee);
            if (dtCompoffparameter.Rows.Count == 0)
            {
                return base.BuildJson(false, 200, "Sorry,Comp-off parameter not updated", "");
            }
            else
            {
                DateTime dt = DateTime.Now;
                floatingHoliday.RemoveAll(fh => (((fh.FromDate.Year * 10000) + (fh.FromDate.Month * 100) + fh.FromDate.Day) > ((dt.Year * 10000) + (dt.Month * 100) + (dt.Day))));
                var leavetype1 = leavetype.Where(lt => lt.LeaveTypeName.ToLower() == "floating holiday").ToList();
                if (leavetype1.Count > 0)
                {
                    floatingHoliday.RemoveAll(fh => fh.LeaveType != leavetype.Where(lt => lt.LeaveTypeName.ToLower() == "floating holiday").FirstOrDefault().Id);
                }
                string CreditvalidityType = Convert.ToString((dtCompoffparameter.Rows[0]["CreditvalidityType"]));
                string CreditvalidityDate = Convert.ToString((dtCompoffparameter.Rows[0]["CreditvalidityDate"]));
                //Added in order to calculate the validity date based on validity days.
                int CreditvalidityDays = Convert.ToInt32((dtCompoffparameter.Rows[0]["Creditvaliditydays"]));
                if (comoff.CompOffGainRequestCheck(levReqObj, companyId, employee, CreditvalidityType, floatingHoliday))
                {
                    levReqObj.CompoffSave();
                    comoff.CompOffGainRequestSave(levReqObj, CreditvalidityDate, CreditvalidityDays);
                    DataTable dtmanagertoapprove = new DataTable();
                    dtmanagertoapprove = levReqObj.getmanagertoapprovelist(dataValue.EmployeeId, companyId, DefaultFinancialYr.Id);
                    for (int q = 0; q < dtmanagertoapprove.Rows.Count; q++)
                    {
                        Guid managerid = new Guid(dtmanagertoapprove.Rows[q]["AssignManagerId"].ToString());
                        Guid employeeid = new Guid(dtmanagertoapprove.Rows[q]["EmployeeId"].ToString());
                        int mrgprioritylevel = Convert.ToInt32(dtmanagertoapprove.Rows[q]["ManagerPriority"].ToString());
                        int compid = companyId;
                        Guid finnnyear = DefaultFinancialYr.Id;
                        Guid levreqid = levReqObj.Id;
                        Guid loggedinID = new Guid(Session["Employeeid"].ToString());
                        bool stst = levReqObj.saveapprovemanagerrequestID(managerid, employeeid, mrgprioritylevel, compid, finnnyear, levreqid, loggedinID);
                    }

                    compoffRequestMail(dataValue, employee, DefaultFinancialYr, companyId, userid, levReqObj.Id);
                }
                else
                {
                    return base.BuildJson(false, 200, "Sorry,Comp-off not eligible for selected date(s)", "");
                }
            }
            return base.BuildJson(true, 200, "Comp-off Requested Sucessfully!!!", "");
        }

        public JsonResult GetBalanceCompOff(Guid EmployeeId)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            DataTable dt = new DataTable();
            LeaveRequest compoffreq = new LeaveRequest();
            dt = compoffreq.GetBalanceCompOff(DefaultFinancialYr.Id, EmployeeId);

            var result = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Json(result, JsonRequestBehavior.AllowGet);

            //return BuildJsonResult(true, 200, "", dt);
        }
        //----------------------------------

        public JsonResult UploadFile()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var fileName = "";
            string strRelationPath = "";
            string fullpath = "";
            Guid tempId;
            try
            {
                tempId = Guid.NewGuid();
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        fileName = Path.GetFileName(file);
                        fileName = fileName.Replace("_", "").Replace("-", "").Replace(" ", "");
                        strRelationPath = "~/LeaveAttachment/" + companyId + "/" + tempId + "/" + fileName;
                        var path = Path.Combine(Server.MapPath(strRelationPath));
                        fullpath = "/LeaveAttachment/" + companyId + "/" + tempId + "/" + fileName;
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                        }
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }

                    }
                }
                return base.BuildJson(true, 200, "User Profile image has been saved.", tempId + "," + fileName + "," + fullpath);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the file.", strRelationPath);
            }
        }


        public JsonResult LeaveEntry(jsonLeaveEnty dataValue, List<CompOffGainHistroy> CompOffEntry)
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            DefaultLOPid ONDUTYID = new DefaultLOPid(companyId, 1);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequest LeaverequestBO = new LeaveRequest();
            LeaverequestBO.compid = companyId;
            LeaverequestBO.FinanceYear = DefaultFinancialYr.Id;
            LeaverequestBO.EmployeeId = dataValue.EmployeeId;
            LeaverequestBO.StartMonth = DefaultFinancialYr.StartMonth;
            LeaverequestBO.EndMonth = DefaultFinancialYr.EndMonth;
            LeaverequestBO.FromDate = Convert.ToDateTime(dataValue.FromDate.ToString());
            LeaverequestBO.EndDate = Convert.ToDateTime(dataValue.EndDate.ToString());
            LeaverequestBO.DefaultLOPid = lossofpayid.LOPId;
            LeaverequestBO.LeavetypeGUid = dataValue.LeaveType;
            string Flag = string.Empty;
            if (dataValue.HRorUser != "HR Leave Entry")
            {
                Flag = LeaverequestBO.LeaveRequestcommonfieldscheck();
            }
            if (dataValue.LeaveType == Guid.Empty)
            {
                return BuildJsonResult(false, 200, "Please Select Leave Type", dataValue);
            }
            else
            {
                //LeaveType levtype = new LeaveType(dataValue.LeaveType, companyId);

                //LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialYr.Id, levtype.Id, Guid.Empty);
                //if (Convert.ToDecimal(dataValue.NoOfDays) < Levconfiglist.FirstOrDefault().mindays)
                //{
                //    return BuildJsonResult(false, 200, "Minimum No of Days can be taken at a time for this leave type is"+ Levconfiglist.FirstOrDefault().mindays.ToString(), dataValue);
                //}
                //else if(Convert.ToDecimal(dataValue.NoOfDays) > Levconfiglist.FirstOrDefault().maxdays)
                //{
                //    return BuildJsonResult(false, 200, "Maximum No of Days can be taken at a time for this leave type is" + Levconfiglist.FirstOrDefault().maxdays.ToString(), dataValue);
                //}




            }

            try
            {
#pragma warning disable
                if (dataValue.HRorUser != "HR Leave Entry")
                {
                    if (Flag != "")
                    {
                        string[] values = Flag.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();
                        }
                        return BuildJsonResult(Convert.ToBoolean(values[0]), Convert.ToInt32(values[1]), values[2].ToString(), values[3].ToString());
                    }
                }
                int sat = 0;
                int sattemp = 0;
                int weekoffzcount = 0;
                int weekoffzcounttemp = 0;
                double RemoveWeekoffDays = 0;
                string temporarycount = null;
                string temporarycounttemp = null;
                var Holidaydates = new List<DateTime?>();                     //Holiday Dates List
                var approveddate = new List<DateTime?>();                     //ALREADY APPROVED LIST
                var WeekoffDates = new List<DateTime?>();                     //WEEK-OFF LIST
                var RemovedWeekOffList = new List<DateTime?>();               //Inorder to take  a list after removing weekoff/adding weekoff based on Intervening Concept
                var selectedDatescurrectyear = new List<DateTime?>();
                var selectedDateswithoutcurrectyear = new List<DateTime?>();
                var selectedDatescurrectyear1 = new List<DateTime?>();
                var selectedDateswithoutcurrectyear1 = new List<DateTime?>();
                LeaveRequest levReqObj = new LeaveRequest();
                LeaveRequest lev = new LeaveRequest();
                Employee employee = new Employee(dataValue.EmployeeId);
                int userid = Convert.ToInt32(Session["UserId"]);
                DateTime fromDate = Convert.ToDateTime(dataValue.FromDate);
                var FDate = fromDate.ToString("dd/MM/yyyy");
                DateTime endDate = Convert.ToDateTime(dataValue.EndDate);
                var EDate = endDate.ToString("dd/MM/yyyy");
                string levnm = string.Empty;
                string atreq = string.Empty;
                decimal atreqday = 0;
                string LeaveConfigurationparameterid = string.Empty;
                //NewLogicalCode  STARTS

                EmpResignation resigCheck = new EmpResignation();
                resigCheck.EmpId = dataValue.EmployeeId;
                resigCheck.CompanyId = companyId;
                var empResigncheck = resigCheck.EmpResignationList().Where(x => x.Isdeleted == true).ToList();
                if (empResigncheck.Count > 0)
                {
                    return BuildJsonResult(false, 200, "Leave Should be apply you are in resignation", dataValue);
                }

                //Converting the Fromdate and Enddate in to a List  STARTS

                var selectedDates = new List<DateTime?>();                                                                                                          //REQUESTED DATE LIST
                for (var date = Convert.ToDateTime(dataValue.FromDate); date <= Convert.ToDateTime(dataValue.EndDate); date = date.AddDays(1))
                {
                    selectedDates.Add(date);
                }

                //Converting the Fromdate and Enddate in to a List  ENDS


                //Compoff Expiry date validation starts
                bool ComoffValCond = true;
                var CompoffSelectedDates = selectedDates;
                var CompValidityDate = new List<DateTime>();
                if (!object.ReferenceEquals(CompOffEntry, null))
                {
                    CompOffEntry.ForEach(p =>
                    {
                        DataTable dtCompoffData = lev.CompOffGainHistroySelect(DefaultFinancialYr.Id, Guid.Empty, dataValue.EmployeeId, p.CompOffGainId);
                        if (dtCompoffData.Rows.Count > 0 && dtCompoffData.Rows[0]["ValidDate"].ToString() != null)
                        {
                            CompValidityDate.Add(Convert.ToDateTime(dtCompoffData.Rows[0]["ValidDate"].ToString()));
                        }
                    });
                    CompoffSelectedDates.ForEach(w =>
                    {
                        if (ComoffValCond != false)
                        {
                            var tempExist = CompValidityDate.Select(e => e.Date < w.Value).FirstOrDefault();
                            if (!tempExist)
                            {
                                ComoffValCond = true;
                            }
                            else
                            {
                                ComoffValCond = false;
                            }
                        }

                    });
                    if (!ComoffValCond)
                    {
                        return BuildJsonResult(false, 200, "Apply leave within Compoff Validity Date !!!", dataValue);
                    }
                }


                //Compoff Expiry date validation ends.



                //FINYear CHECKING START

                DateTime fromdate = Convert.ToDateTime(DefaultFinancialYr.StartMonth);
                DateTime todate = Convert.ToDateTime(DefaultFinancialYr.EndMonth);
                DateTime startingdate = Convert.ToDateTime(dataValue.FromDate);
                DateTime endingdate = Convert.ToDateTime(dataValue.EndDate);
                for (int i = 0; i <= selectedDates.Count - 1; i++)
                {
                    DateTime currentleavedate = selectedDates[i].Value.Date;
                    if (currentleavedate >= fromdate && currentleavedate <= todate)
                    {
                        selectedDatescurrectyear1.Add(currentleavedate);
                    }
                    else
                    {
                        selectedDateswithoutcurrectyear1.Add(currentleavedate);
                    }

                }
                if (selectedDateswithoutcurrectyear1.Count != 0)
                {
                    return BuildJsonResult(false, 200, "Leave Should be apply with in financial year!!!", dataValue);
                }

                //FINYear CHECKING END
                //var approveddate = new List<DateTime?>();                                                                                                           //ALREADY APPROVED LIST
                List<LeaveRequest> objlistEligCheck = new List<LeaveRequest>();
                LeaveRequestList leav = new LeaveRequestList(dataValue.EmployeeId, companyId);
                LeaveTypeList leavetype = new LeaveTypeList(companyId, DefaultFinancialYr.Id);
                List<LeaveType> Leaveid = leavetype.Where(lt => lt.LeaveTypeName == "ONDUTY").ToList();
                if (Leaveid.Count > 0)
                {
                    if (leavetype.Where(lt => lt.LeaveTypeName == "ONDUTY").FirstOrDefault().Id == dataValue.LeaveType)
                    {
                        leav.RemoveAll(le => le.LeaveType == leavetype.Where(lt => lt.LeaveTypeName.ToLower() == "floating holiday").FirstOrDefault().Id);
                    }
                }

                objlistEligCheck = leav.Where(w => (w.Status == 1 || w.Status == 0) && w.LeaveType == dataValue.LeaveType).ToList();
                for (int i = 0; i <= leav.Count - 1; i++)
                {
                    if (leav[i].Status == 1 || leav[i].Status == 0)
                    {
                        for (var date = leav[i].FromDate; date <= leav[i].EndDate; date = date.AddDays(1))
                        {
                            approveddate.Add(date);
                        }
                    }
                }

                //Getting Already REQUESTED and APPROVED dates LIST END




                //Already APPLIED CHECKING START

                int newleavecount = 0;
                newleavecount = selectedDates.Count;
                var firstNotSecond = selectedDates.Except(approveddate).ToList();
                if (firstNotSecond.Count != newleavecount)
                {
                    int datechecked = 0;
                    for (int i = 0; i <= leav.Count - 1; i++)
                    {
                        if (leav[i].Status == 0 || leav[i].Status == 1)
                        {
                            if (leav[i].FromDate == Convert.ToDateTime(dataValue.FromDate))
                            {
                                if (dataValue.FromDay == 0)
                                {
                                    return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                }
                                else
                                {
                                    if (leav[i].FromDay == dataValue.FromDay)
                                    {
                                        return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                    }
                                    else
                                    {
                                        datechecked = 1;
                                    }
                                }


                            }
                        }
                    }
                    for (int j = 0; j <= leav.Count - 1; j++)
                    {
                        if (leav[j].Status == 0 || leav[j].Status == 1)
                        {

                            if (leav[j].FromDate == Convert.ToDateTime(dataValue.EndDate))
                            {
                                if (dataValue.ToDay == 0)
                                {
                                    return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                }
                                else
                                {
                                    if (leav[j].FromDay == dataValue.ToDay)
                                    {
                                        return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                    }
                                    else
                                    {
                                        datechecked = 1;
                                    }

                                }
                            }
                        }
                    }
                    for (int k = 0; k <= leav.Count - 1; k++)
                    {
                        if (leav[k].Status == 0 || leav[k].Status == 1)
                        {
                            if (leav[k].EndDate == Convert.ToDateTime(dataValue.FromDate))
                            {
                                if (dataValue.FromDay == 0)
                                {
                                    return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                }
                                else
                                {
                                    if (leav[k].ToDay == dataValue.FromDay)
                                    {
                                        return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                    }
                                    else
                                    {
                                        datechecked = 1;

                                    }
                                }

                            }
                        }
                    }
                    for (int l = 0; l <= leav.Count - 1; l++)
                    {
                        if (leav[l].Status == 0 || leav[l].Status == 1)
                        {
                            if (leav[l].EndDate == Convert.ToDateTime(dataValue.EndDate))
                            {
                                if (dataValue.ToDay == 0)
                                {
                                    return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                }
                                else
                                {
                                    if (leav[l].FromDay == dataValue.ToDay)
                                    {
                                        return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                                    }
                                    else
                                    {
                                        datechecked = 1;
                                    }
                                }
                            }
                        }
                    }

                    if (datechecked != 1)
                    {
                        return BuildJsonResult(false, 200, "Date mismatch,You have already applied on these days!!!", null);
                    }

                }
                //Already APPLIED CHECKING END
                LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);
                LeaveConfigurationparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].leaveparameter, employee);                       //LeaveConfigurationparameterid
                string[] LCvalues = LeaveConfigurationparameterid.Split(',');
                for (int i = 0; i < LCvalues.Length; i++)
                {
                    LCvalues[i] = LCvalues[i].Trim();
                }
                if (LCvalues[1] != "")
                {
                    LeaveConfigurationparameterid = LCvalues[1].ToString();
                }
                else
                {
                    LeaveConfigurationparameterid = "00000000-0000-0000-0000-000000000000";
                }

                dataValue.FinanceYear = DefaultFinancialYr.Id;
                string Empcode = employee.EmployeeCode;
                AssignManager assignManager = new AssignManager(dataValue.EmployeeId, companyId, 0, DefaultFinancialYr.Id);
                var leave = leavetype.Where(l => l.Id == dataValue.LeaveType).FirstOrDefault();
                levnm = leave.LeaveTypeName;
                if (dataValue.LeaveType != new Guid(lossofpayid.LOPId.ToString()))//199f5db2-14b7-46d3-a0e4-715d56682277
                {

                    if (leave != null)
                    {
                        dataValue.LeaveTypeName = leave.LeaveTypeName;
                    }
                    else
                    {
                        return BuildJsonResult(true, 200, "Select The Leave Type.", dataValue);
                    }
                }
                else
                {
                    dataValue.LeaveTypeName = "LOSS OF PAY DAYS";
                }
                LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialYr.Id, dataValue.LeaveType, new Guid(LeaveConfigurationparameterid));         //LEAVE CONFIGURATION LIST
                if (dataValue.LeaveTypeName.ToUpper() != "ONDUTY" && dataValue.LeaveTypeName.ToUpper() != "WORK FROM HOME" && dataValue.LeaveTypeName.ToUpper() != "LOSS OF PAY DAYS")
                {
                    if (Levconfiglist.Count() == 0)
                    {
                        return BuildJsonResult(false, 200, "Please leave configuration settings.", null);
                    }
                    else
                    {
                        atreq = Levconfiglist[0].Isattachreq;
                        atreqday = Levconfiglist[0].Attachreqmaxdays;
                    }
                }
                if (dataValue.HRorUser == "HR Leave Entry")
                {
                    if (Convert.ToDouble(dataValue.NoOfDays) > dataValue.balanceLeave)
                    {
                        return BuildJsonResult(false, 200, "You don't have enoughf leave balance", null);
                    }
                }
                else
                {
                    //LEAVE MASTER LIST
                    //EMPLOYEE LIST
                    string weekoffparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].Weekoffparameter, employee);                                //weekoffparameterid                       
                    string[] WKvalues = weekoffparameterid.Split(',');
                    for (int i = 0; i < WKvalues.Length; i++)
                    {
                        WKvalues[i] = WKvalues[i].Trim();
                    }
                    if (WKvalues[1] != "")
                    {
                        weekoffparameterid = WKvalues[1].ToString();
                    }
                    else
                    {
                        weekoffparameterid = "00000000-0000-0000-0000-000000000000";
                    }

                    LeaveSettingsBO WeekoffBO = new LeaveSettingsBO();
                    //FOR GETTING WEEK OFF DAYS START

                    WeekoffBO.CompanyId = companyId;
                    WeekoffBO.FinyrId = DefaultFinancialYr.Id;
                    WeekoffBO.DynamicComponentValue = new Guid(weekoffparameterid);
                    DataTable weekoffdt = WeekoffBO.getWeekoffdataRequestingtime();
                    List<DataRow> WEEKOFFlist = weekoffdt.AsEnumerable().ToList();//Converts the dataTable in to List   
                                                                                  //var WeekoffDates = new List<DateTime?>();   //WEEK-OFF LIST
                    for (int WKcnt = 0; WKcnt <= WEEKOFFlist.Count - 1; WKcnt++)
                    {
                        WeekoffDates.Add(Convert.ToDateTime(WEEKOFFlist[WKcnt].ItemArray[0]));
                    }

                    //FOR GETTING WEEK OFF DAYS START

                    //FOR GETTING HOLIDAY DAYS START

                    string Holidayparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].Holidayparameter, employee);                                //Holidayparameterid                       
                    string[] Holidayvalues = Holidayparameterid.Split(',');
                    for (int i = 0; i < Holidayvalues.Length; i++)
                    {
                        Holidayvalues[i] = Holidayvalues[i].Trim();
                    }
                    if (Holidayvalues[1] != "")
                    {
                        Holidayparameterid = Holidayvalues[1].ToString();
                    }
                    else
                    {
                        Holidayparameterid = "00000000-0000-0000-0000-000000000000";
                    }
                    HolidaysList HolidayList = new HolidaysList(Guid.Empty, DefaultFinancialYr.Id, new Guid(Holidayparameterid));                                        //HOLIDAY LIST

                    //FOR GETTING HOLIDAY DAYS ENDS




                    //Getting Already REQUESTED and APPROVED dates LIST START 
                    //Getting Leave config Details START

                    string IsInterveningHoliday = string.Empty;
                    string SubInterveningHoliday = string.Empty;
                    IsInterveningHoliday = Levconfiglist.Count == 0 ? "" : Levconfiglist[0].InvHoliday;
                    if (IsInterveningHoliday == "Y")
                    {
                        SubInterveningHoliday = Levconfiglist.Count == 0 ? "" : Levconfiglist[0].InvHolidaysubparameter;
                    }

                    //Getting Leave config Details END 

                    //Intervening Concepts Start.
                    for (int i = 0; i <= HolidayList.Count - 1; i++)
                    {

                        for (var Holiday = HolidayList[i].Holidaydate; Holiday <= HolidayList[i].Holidaydate; Holiday = Holiday.AddDays(1))
                        {
                            Holidaydates.Add(Holiday); ;
                        }
                    }
                    bool interveningRslt = false;
                    string errmsg = string.Empty;
                    LeaveRequestModel objLevreqMod = new LeaveRequestModel();
                    List<DateTime?> FinalDateLst = new List<DateTime?>();
                    LeaveType levtype = new LeaveType(dataValue.LeaveType, companyId, DefaultFinancialYr.Id);
                    if (dataValue.LeaveTypeName != "ONDUTY")
                    {
                        interveningRslt = objLevreqMod.InterveningValidation(ref FinalDateLst, ref errmsg, WeekoffDates, Holidaydates, selectedDates, approveddate, dataValue.FromDate, dataValue.EndDate, IsInterveningHoliday, SubInterveningHoliday, levtype);
                        if (interveningRslt != true)
                        {
                            return BuildJsonResult(false, 200, errmsg, "");
                        }
                        else
                        {
                            RemovedWeekOffList = FinalDateLst;
                        }
                    }
                    else
                    {
                        RemovedWeekOffList = selectedDates;
                    }


                    //Intervening Concepts End.





                    //NewLogicalCode  ENDS

                    if (dataValue.FromDate == dataValue.EndDate)
                    {
                        if (dataValue.FromDay != 0 && dataValue.ToDay != 0)
                        {
                            RemoveWeekoffDays = RemovedWeekOffList.Count - 0.5;
                        }
                        else
                        {
                            RemoveWeekoffDays = RemovedWeekOffList.Count;
                        }
                    }
                    else
                    {
                        if (dataValue.FromDay != 0)
                        {
                            RemoveWeekoffDays = RemoveWeekoffDays != 0 ? RemoveWeekoffDays - 0.5 : RemovedWeekOffList.Count - 0.5;
                        }
                        if (dataValue.ToDay != 0)
                        {
                            RemoveWeekoffDays = RemoveWeekoffDays != 0 ? RemoveWeekoffDays - 0.5 : RemovedWeekOffList.Count - 0.5;
                        }
                        else
                        {
                            RemoveWeekoffDays = RemovedWeekOffList.Count;
                        }
                    }
                    var FilterAppfromHoliday = approveddate.Except(Holidaydates).ToList();
                    var FilterAppfromWeekoff = FilterAppfromHoliday.Except(WeekoffDates).ToList();
                    approveddate = FilterAppfromWeekoff;
                    //if (dataValue.FromDay==0 && dataValue.ToDay==0)
                    //{
                    //    RemoveWeekoffDays = RemovedWeekOffList.Count;
                    //}
                    //else if (dataValue.FromDay!=0 && dataValue.ToDay!=0 && Convert.ToDouble(dataValue.NoOfDays)!=0.5)
                    //{
                    //    RemoveWeekoffDays = RemovedWeekOffList.Count - 0.5 - 0.5;
                    //}
                    //else if (dataValue.FromDay != 0 && dataValue.ToDay != 0 && Convert.ToDouble(dataValue.NoOfDays) == 0.5)
                    //{
                    //    RemoveWeekoffDays = RemovedWeekOffList.Count - 0.5;
                    //}
                    //if (dataValue.FromDay!=0 && dataValue.ToDay==0)
                    //{
                    //    RemoveWeekoffDays = RemovedWeekOffList.Count - 0.5;
                    //}
                    //else if (dataValue.FromDay == 0 && dataValue.ToDay != 0)
                    //{
                    //    RemoveWeekoffDays = RemovedWeekOffList.Count - 0.5;
                    //}

                    LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
                    dataValue.NoOfDays = RemoveWeekoffDays.ToString();
                    double currentyeartemp = RemovedWeekOffList.Count;


                    // New Logic Starts

                    //Min Max day taken at a stretch Started
                    //LeaveType levtypeobj = new LeaveType(dataValue.LeaveType, companyId);

                    //LeaveConfigList Levconfiglist1 = new LeaveConfigList(DefaultFinancialYr.Id, levtypeobj.Id, Guid.Empty);
                    //if (RemoveWeekoffDays < Convert.ToDouble(Levconfiglist1.FirstOrDefault().mindays))
                    //{
                    //    return BuildJsonResult(false, 200, "Minimum No of Days can be taken at a time for this leave type is " + Levconfiglist1.FirstOrDefault().mindays.ToString(), dataValue);
                    //}
                    //else if (RemoveWeekoffDays >Convert.ToDouble(Levconfiglist1.FirstOrDefault().maxdays))
                    //{
                    //    return BuildJsonResult(false, 200, "Maximum No of Days can be taken at a time for this leave type is " + Levconfiglist1.FirstOrDefault().maxdays.ToString(), dataValue);
                    //}
                    //Min Max day taken at a stretch Ends
                    // Monthly eligiblity Starts
                    string AllowDeviation = string.Empty;
                    DateTime SetEffec = DateTime.Now;
                    double MonEligCount = 0;
                    if (Levconfiglist.Count > 0)
                    {
                        Levconfiglist.ForEach(k =>
                        {
                            AllowDeviation = k.AllowDevisionMonth;
                            SetEffec = k.ConfigEffectiveDate;
                            MonEligCount = Convert.ToDouble(k.MaxDayMonth);
                        });
                    }
                    int EffectMonth = SetEffec.Month;
                    int Endmonth = Convert.ToDateTime(dataValue.FromDate).Month;
                    int MonthDiff = (Endmonth - EffectMonth) + 1;
                    double availEligCount = 0;
                    double tempCount = 0;
                    if (AllowDeviation == "Y")
                    {
                        objlistEligCheck.ForEach(k1 =>
                        {
                            tempCount = tempCount != 0 ? tempCount + Convert.ToDouble(k1.NoOfDays) : Convert.ToDouble(k1.NoOfDays);
                        });
                        availEligCount = (MonthDiff * MonEligCount) - tempCount;
                        //for (int u = 0; u <= MonthDiff; u++)
                        //{
                        //    var tempelig1 = approveddate.Where(p => p.Value.Month == EffectMonth).ToList();
                        //    if (tempelig1.Count < MonEligCount)
                        //    {
                        //        double tempcount1 = MonEligCount - tempelig1.Count;
                        //        availEligCount = availEligCount + tempcount1;
                        //    }
                        //    else if (tempelig1.Count == MonEligCount)
                        //    {
                        //        double tempcount2 = 0;
                        //        availEligCount = availEligCount + tempcount2;
                        //    }
                        //    else if (tempelig1.Count > MonEligCount)
                        //    {
                        //        double tempcount3 = tempelig1.Count - MonEligCount;
                        //        availEligCount = availEligCount - tempcount3;
                        //    }
                        //    EffectMonth = EffectMonth + 1;

                        //}
                        if (RemoveWeekoffDays > availEligCount)
                        {
                            return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(dataValue.FromDate).Month == Convert.ToDateTime(dataValue.EndDate).Month)
                        {
                            List<DateTime> objDateLst = new List<DateTime>();
                            double tempcount1 = 0;
                            var tempelig1 = objlistEligCheck.Where(p => (p.FromDate.Month == Convert.ToDateTime(dataValue.FromDate).Month && p.FromDate.Year == Convert.ToDateTime(dataValue.FromDate).Year) && (p.EndDate.Month == Convert.ToDateTime(dataValue.EndDate).Month && p.EndDate.Year == Convert.ToDateTime(dataValue.EndDate).Year)).ToList();
                            var tempelig3 = objlistEligCheck.Where(p1 => (p1.FromDate.Month == Convert.ToDateTime(dataValue.FromDate).Month) && (p1.EndDate.Month != Convert.ToDateTime(dataValue.FromDate).Month)).ToList();
                            if (tempelig3.Count > 0)
                            {
                                for (int l = 0; l < tempelig3.Count; l++)
                                {
                                    for (var date = tempelig3[l].FromDate; date <= tempelig3[l].EndDate; date = date.AddDays(1))
                                    {
                                        if (date.Month == Convert.ToDateTime(dataValue.FromDate).Month)
                                        {
                                            objDateLst.Add(date);
                                        }
                                    }
                                    if (tempelig3[l].FromDay != 0)
                                    {
                                        tempcount1 = objDateLst.Count - 0.5;
                                    }
                                    else
                                    {
                                        tempcount1 = objDateLst.Count;
                                    }
                                    objDateLst.Clear();
                                }
                            }
                            if (tempelig1.Count > 0)
                            {
                                tempelig1.ForEach(p1 =>
                                {
                                    tempCount = tempCount != 0 ? tempCount + Convert.ToDouble(p1.NoOfDays) : Convert.ToDouble(p1.NoOfDays);
                                });
                                tempCount = tempCount + tempcount1;
                                if (tempCount < MonEligCount || tempCount == MonEligCount)
                                {
                                    availEligCount = availEligCount != 0 ? availEligCount + (MonEligCount - tempCount) : (MonEligCount - tempCount);
                                }

                            }
                            else
                            {
                                availEligCount = MonEligCount;
                            }
                            //var tempelig1 = approveddate.Where(p => p.Value.Month == Convert.ToDateTime(dataValue.FromDate).Month).ToList();
                            //if (tempelig1.Count < MonEligCount)
                            //{
                            //    double tempcount1 = MonEligCount - tempelig1.Count;
                            //    availEligCount = availEligCount + tempcount1;
                            //}
                            //else if (tempelig1.Count == MonEligCount)
                            //{
                            //    double tempcount2 = 0;
                            //    availEligCount = availEligCount + tempcount2;
                            //}

                            ////condition change from RemovedWeekOffList.Count >= MonEligCount to RemovedWeekOffList.Count > MonEligCount in order to solve the monthly elig issue.
                            if (RemoveWeekoffDays > availEligCount)
                            {
                                return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                            }
                        }
                        else
                        {
                            var tempelig1 = objlistEligCheck.Where(p => p.FromDate.Month == Convert.ToDateTime(dataValue.FromDate).Month).ToList();
                            if (tempelig1.Count > 0)
                            {
                                tempelig1.ForEach(p1 =>
                                {
                                    tempCount = tempCount != 0 ? tempCount + Convert.ToDouble(p1.NoOfDays) : Convert.ToDouble(p1.NoOfDays);
                                });
                                //if (tempCount < MonEligCount || tempCount == MonEligCount)
                                //{
                                //    availEligCount = availEligCount != 0 ? availEligCount + (MonEligCount - tempCount) : (MonEligCount - tempCount);
                                //}
                                var tempfromdatecnt = RemovedWeekOffList.Where(l => l.Value.Month == Convert.ToDateTime(dataValue.FromDate).Month).ToList();
                                if (dataValue.FromDay != 0)
                                {
                                    availEligCount = tempCount + (tempfromdatecnt.Count - 0.5);
                                }
                                else
                                {
                                    availEligCount = tempCount + tempfromdatecnt.Count;
                                }
                            }
                            else
                            {
                                availEligCount = MonEligCount;
                            }
                            if (availEligCount > MonEligCount)
                            {
                                return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                            }
                            var tempelig2 = objlistEligCheck.Where(p => p.EndDate.Month == Convert.ToDateTime(dataValue.EndDate).Month).ToList();
                            if (tempelig2.Count > 0)
                            {
                                tempelig2.ForEach(p1 =>
                                {
                                    tempCount = tempCount != 0 ? tempCount + Convert.ToDouble(p1.NoOfDays) : Convert.ToDouble(p1.NoOfDays);
                                });
                                //if (tempCount < MonEligCount || tempCount == MonEligCount)
                                //{
                                //    availEligCount = availEligCount != 0 ? availEligCount + (MonEligCount - tempCount) : (MonEligCount - tempCount);
                                //}
                                var tempfromdatecnt = RemovedWeekOffList.Where(l => l.Value.Month == Convert.ToDateTime(dataValue.EndDate).Month).ToList();
                                if (dataValue.ToDay != 0)
                                {
                                    availEligCount = tempCount + (tempfromdatecnt.Count - 0.5);
                                }
                                else
                                {
                                    availEligCount = tempCount + tempfromdatecnt.Count;
                                }
                            }
                            else
                            {
                                availEligCount = MonEligCount;
                            }
                            if (availEligCount > MonEligCount)
                            {
                                return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                            }

                            //var tempelig1 = approveddate.Where(p => p.Value.Month == Convert.ToDateTime(dataValue.FromDate).Month).ToList();
                            //var tempfromdatecnt = RemovedWeekOffList.Where(l => l.Value.Month == Convert.ToDateTime(dataValue.FromDate).Month).ToList();
                            //int tempvar1 = tempelig1.Count + tempfromdatecnt.Count;
                            //if (tempvar1>MonEligCount)
                            //{
                            //    return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                            //}
                            //var tempelig2 = approveddate.Where(p => p.Value.Month == Convert.ToDateTime(dataValue.EndDate).Month).ToList();
                            //var temptodatecnt = RemovedWeekOffList.Where(l => l.Value.Month == Convert.ToDateTime(dataValue.EndDate).Month).ToList();
                            //int tempvar2 = tempelig2.Count + temptodatecnt.Count;
                            //if (tempvar2 > MonEligCount)
                            //{
                            //    return BuildJsonResult(false, 200, "Number of Days count Exceeds the Monthly Eligibility count", null);
                            //}

                        }


                    }

                    // Monthly eligiblity End


                    // Min/Max settings starts

                    double Mindays, Maxdays, MindayMaxtime, Maxdaymaxtime;
                    Mindays = Maxdays = MindayMaxtime = Maxdaymaxtime = 0;
                    string MindaysAllowDev, MaxdaysAlloDev, MindaysPerd, MaxdaysPerd;
                    MindaysAllowDev = MaxdaysAlloDev = MindaysPerd = MaxdaysPerd = string.Empty;
                    if (dataValue.LeaveTypeName.ToUpper() != "ONDUTY" && dataValue.LeaveTypeName.ToUpper() != "WORK FROM HOME" && dataValue.LeaveTypeName.ToUpper() != "LOSS OF PAY DAYS")
                    {
                        if (levmasterlist.Count > 0)
                        {
                            levmasterlist.ForEach(l =>
                            {
                                var MinMaxMasterchk = l.Minormaxparameter.ToString();
                                if (MinMaxMasterchk == "C")//Common to all leave type.
                                {
                                    if (l.minperiod == "0")
                                    {
                                        Mindays = Convert.ToDouble(l.mindays);
                                        MindayMaxtime = Convert.ToDouble(l.maxmintimes);
                                        MindaysPerd = l.minperiod;
                                        MindaysAllowDev = l.mindeviation;
                                    }
                                    if (l.maxperiod == "0")
                                    {
                                        Maxdays = Convert.ToDouble(l.maxdays);
                                        Maxdaymaxtime = Convert.ToDouble(l.maxmaxtimes);
                                        MaxdaysPerd = l.maxperiod;
                                        MaxdaysAlloDev = l.maxdeviation;
                                    }
                                }
                                else if (MinMaxMasterchk == "P") //Applicable for particular leave type.
                                {
                                    if (Levconfiglist.Count > 0)
                                    {
                                        Levconfiglist.ForEach(o =>
                                        {
                                            if (o.minperiod == "0")
                                            {
                                                Mindays = Convert.ToDouble(o.mindays);
                                                MindayMaxtime = Convert.ToDouble(o.maxmintimes);
                                                MindaysPerd = o.minperiod;
                                                MindaysAllowDev = o.mindeviation;
                                            }
                                            if (o.maxperiod == "0")
                                            {
                                                Maxdays = Convert.ToDouble(o.maxdays);
                                                Maxdaymaxtime = Convert.ToDouble(o.maxmaxtimes);
                                                MaxdaysPerd = o.maxperiod;
                                                MaxdaysAlloDev = o.maxdeviation;
                                            }
                                        });
                                    }
                                }
                            });
                        }
                        if (RemoveWeekoffDays < Mindays)
                        {
                            return BuildJsonResult(false, 200, "Number of Days count Less than the Minimum No. of days taken at a time Eligibility.", null);
                        }
                        if (RemoveWeekoffDays > Maxdays)
                        {
                            return BuildJsonResult(false, 200, "Number of Days count Greater than the Maximum No. of days taken at a time Eligibility.", null);
                        }
                        //Min/Max settings Ends


                        // New Logic End


                        if (selectedDatescurrectyear1.Count != 0)
                        {
                            if (dataValue.LeaveType != new Guid(lossofpayid.LOPId.ToString()) && dataValue.LeaveType != new Guid(ONDUTYID.ONDUTYId.ToString()))//"199f5db2-14b7-46d3-a0e4-715d56682277"
                            {
                                if (dataValue.NoOfDays == "0.5")
                                {
                                    if (Convert.ToDouble(dataValue.NoOfDays) > dataValue.balanceLeave)
                                    {
                                        return BuildJsonResult(false, 200, "You Dont have Enough leave balance", dataValue);
                                    }
                                }
                                else
                                {
                                    if (currentyeartemp > dataValue.balanceLeave)
                                    {
                                        return BuildJsonResult(false, 200, "You Dont have Enough leave balance", dataValue);
                                    }
                                }

                            }
                        }
                    }
                }
                levReqObj = jsonLeaveEnty.convertobject(dataValue);
                double numberofdayscount = Convert.ToDouble(lev.NoOfDays);
                levReqObj.FinanceYear = Guid.Empty;
                lev.Childid = Guid.Empty;
                lev.childflag = 0;
                lev.compid = companyId;
                levReqObj.compid = companyId;
                levReqObj.HRentrystatus = dataValue.HRentrystatus;

                //New Logic For attachment Starts
                if (atreq == "Y")
                {
                    if (atreqday <= Convert.ToDecimal(levReqObj.NoOfDays))
                    {
                        if (dataValue.Imgpath == null)
                        {
                            return BuildJsonResult(false, 200, "Your applying leave for " + lev.NoOfDays + " days,Please provide proof!!!", dataValue);
                        }
                        else
                        {
                            levReqObj.Imgpath = dataValue.Imgpath;
                        }
                    }
                }


                //New Logic For attachment Ends







                levReqObj.Save();

                //New Logic For Saving Leave Absent Table Starts
                List<jsonAbsentTable> FinalLeaveAbsentList = new List<jsonAbsentTable>();

                if (dataValue.HRorUser != "HR Leave Entry")
                {
                    for (int ABScnt = 0; ABScnt <= RemovedWeekOffList.Count - 1; ABScnt++)
                    {
                        jsonAbsentTable LeaveAbsentList = new jsonAbsentTable();
                        LeaveAbsentList.Id = Guid.NewGuid();
                        LeaveAbsentList.LeaveDate = RemovedWeekOffList[ABScnt].Value.Date;
                        LeaveAbsentList.LeaveDayName = RemovedWeekOffList[ABScnt].Value.DayOfWeek.ToString();
                        LeaveAbsentList.LeaveRequestId = levReqObj.Id;
                        LeaveAbsentList.LeaveTypeId = levReqObj.LeaveType;
                        LeaveAbsentList.EmployeeId = levReqObj.EmployeeId;
                        LeaveAbsentList.FinyearId = DefaultFinancialYr.Id;
                        LeaveAbsentList.CreatedBy = Session["userid"].ToString();
                        LeaveAbsentList.CompanyId = levReqObj.compid;
                        LeaveAbsentList.HFDay = 1;
                        if (Convert.ToDateTime(dataValue.FromDate) == RemovedWeekOffList[ABScnt].Value.Date)
                        {
                            if (dataValue.FromDay == 0)
                            {
                                LeaveAbsentList.HFDay = 1;
                            }
                            else
                            {
                                LeaveAbsentList.HFDay = 0.5F;
                            }
                        }
                        if (Convert.ToDateTime(dataValue.EndDate) == RemovedWeekOffList[ABScnt].Value.Date)
                        {
                            if (dataValue.ToDay == 0)
                            {
                                LeaveAbsentList.HFDay = 1;
                            }
                            else
                            {
                                LeaveAbsentList.HFDay = 0.5F;
                            }
                        }
                        FinalLeaveAbsentList.Add(LeaveAbsentList);
                        if (!Object.ReferenceEquals(CompOffEntry, null))
                        {
                            if (CompOffEntry.Count() > 0)
                            {
                                CompOffEntry[ABScnt].AvaliableDays = Convert.ToDecimal(LeaveAbsentList.HFDay);
                            }
                        }

                    }
                }
                else
                {
                    for (int ABScnt = 0; ABScnt <= selectedDates.Count - 1; ABScnt++)
                    {
                        jsonAbsentTable LeaveAbsentList = new jsonAbsentTable();
                        LeaveAbsentList.Id = Guid.NewGuid();
                        LeaveAbsentList.LeaveDate = selectedDates[ABScnt].Value.Date;
                        LeaveAbsentList.LeaveDayName = selectedDates[ABScnt].Value.DayOfWeek.ToString();
                        LeaveAbsentList.LeaveRequestId = levReqObj.Id;
                        LeaveAbsentList.LeaveTypeId = levReqObj.LeaveType;
                        LeaveAbsentList.EmployeeId = levReqObj.EmployeeId;
                        LeaveAbsentList.FinyearId = DefaultFinancialYr.Id;
                        LeaveAbsentList.CreatedBy = Session["userid"].ToString();
                        LeaveAbsentList.CompanyId = levReqObj.compid;
                        LeaveAbsentList.HFDay = 1;
                        if (Convert.ToDateTime(dataValue.FromDate) == selectedDates[ABScnt].Value.Date)
                        {
                            if (dataValue.FromDay == 0)
                            {
                                LeaveAbsentList.HFDay = 1;
                            }
                            else
                            {
                                LeaveAbsentList.HFDay = 0.5F;
                            }
                        }
                        if (Convert.ToDateTime(dataValue.EndDate) == selectedDates[ABScnt].Value.Date)
                        {
                            if (dataValue.ToDay == 0)
                            {
                                LeaveAbsentList.HFDay = 1;
                            }
                            else
                            {
                                LeaveAbsentList.HFDay = 0.5F;
                            }
                        }
                        FinalLeaveAbsentList.Add(LeaveAbsentList);
                        if (!Object.ReferenceEquals(CompOffEntry, null))
                        {
                            if (CompOffEntry.Count() > 0)
                            {
                                CompOffEntry[ABScnt].AvaliableDays = Convert.ToDecimal(LeaveAbsentList.HFDay);
                            }
                        }
                    }
                }



                StringWriter stringWriter = new StringWriter();
                System.Xml.XmlDocument xmlDoc = new XmlDocument();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                XmlSerializer serializer = new XmlSerializer(typeof(List<jsonAbsentTable>));
                serializer.Serialize(xmlWriter, FinalLeaveAbsentList);
                string xmlResult = stringWriter.ToString();

                levReqObj.SaveAbsentTable(xmlResult);


                //New Logic For Saving Leave Absent Table Ends
                if (!Object.ReferenceEquals(CompOffEntry, null))
                {
                    if (levnm.ToLower().Trim() == "compoff" && CompOffEntry.Count() > 0)
                    {
                        LeaveRequestModel CompOfflevReq = new LeaveRequestModel();
                        CompOfflevReq.CompoffleaveMatchingsave(CompOffEntry, levReqObj.Id);
                    }

                }



                DataTable dtmanagertoapprove = new DataTable();

                dtmanagertoapprove = levReqObj.getmanagertoapprovelist(dataValue.EmployeeId, companyId, DefaultFinancialYr.Id);
                for (int q = 0; q < dtmanagertoapprove.Rows.Count; q++)
                {

                    Guid managerid = new Guid(dtmanagertoapprove.Rows[q]["AssignManagerId"].ToString());
                    Guid employeeid = new Guid(dtmanagertoapprove.Rows[q]["EmployeeId"].ToString());
                    int mrgprioritylevel = Convert.ToInt32(dtmanagertoapprove.Rows[q]["ManagerPriority"].ToString());
                    int compid = companyId;
                    Guid finnnyear = DefaultFinancialYr.Id;
                    Guid levreqid = levReqObj.Id;
                    Guid loggedinID = new Guid(Session["Employeeid"].ToString());
                    bool stst = levReqObj.saveapprovemanagerrequestID(managerid, employeeid, mrgprioritylevel, compid, finnnyear, levreqid, loggedinID);


                }





                var holidaycount1 = 0;
                var newleavecount1 = 0;
                var temp1 = 0;
                var weekoffzcount1 = 0;
                int sat1 = 0;
                string temporarycount1 = null;

                var holidaycount2 = 0;
                var newleavecount2 = 0;
                var temp2 = 0;
                var weekoffzcount2 = 0;
                int sat2 = 0;
                string temporarycount2 = null;
                Guid childiid = Guid.Empty;


                //for (int i = 0; i <= holidaysubtract.Count - 1; i++)
                //{
                //    DateTime currentleavedate = holidaysubtract[i].Value.Date;

                if (dataValue.HRorUser != "HR Leave Entry")
                {
                    for (int i = 0; i <= RemovedWeekOffList.Count - 1; i++)
                    {
                        DateTime currentleavedate = RemovedWeekOffList[i].Value.Date;
                        if (currentleavedate >= fromdate && currentleavedate <= todate)
                        {
                            selectedDatescurrectyear.Add(currentleavedate);
                        }
                        else
                        {
                            selectedDateswithoutcurrectyear.Add(currentleavedate);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= selectedDates.Count - 1; i++)
                    {
                        DateTime currentleavedate = selectedDates[i].Value.Date;
                        if (currentleavedate >= fromdate && currentleavedate <= todate)
                        {
                            selectedDatescurrectyear.Add(currentleavedate);
                        }
                        else
                        {
                            selectedDateswithoutcurrectyear.Add(currentleavedate);
                        }



                    }
                }


                childiid = levReqObj.Id;
                //only for enlighted
                if (dataValue.LeaveTypeName == "ONDUTY")
                {
                    levReqObj.Id = Guid.Empty;
                    levReqObj.FinanceYear = dataValue.FinanceYear;
                    levReqObj.childflag = 1;
                    levReqObj.Childid = childiid;
                    levReqObj.Save();
                }
                if (selectedDatescurrectyear.Count != 0)
                {

                    double finyearwithcount = selectedDatescurrectyear.Count;
                    double finyearwithoutcount = selectedDateswithoutcurrectyear.Count;
                    //for (int n = 0; n <= weekoffdt.Rows.Count - 1; n++)
                    //{

                    //    string weekdayoff1 = weekoffdt.Rows[n]["weekoffday"].ToString();
                    //    for (int d = 0; d <= selectedDatescurrectyear.Count - 1; d++)
                    //    {
                    //        string holidayrejectedlistdayoff1 = selectedDatescurrectyear[d].Value.DayOfWeek.ToString();
                    //        if (holidayrejectedlistdayoff1 == weekdayoff1)
                    //        {

                    //            weekoffzcount1 = weekoffzcount1 + 1;
                    //        }
                    //    }

                    //}
                    int daystatus1 = 0;
                    int daystatus2 = 0;
                    //double totaldaysvalue = finyearwithcount - weekoffzcount1;
                    //dataValue.NoOfDays = totaldaysvalue.ToString();
                    double totaldaysvalue = 0;
                    if (dataValue.HRorUser != "HR Leave Entry")
                    {
                        totaldaysvalue = RemovedWeekOffList.Count;
                        dataValue.NoOfDays = RemovedWeekOffList.Count.ToString();
                    }
                    else
                    {
                        totaldaysvalue = selectedDates.Count;
                        dataValue.NoOfDays = selectedDates.Count.ToString();
                    }

                    levReqObj.Childid = levReqObj.Id;

                    childiid = levReqObj.Id;
                    levReqObj.Id = Guid.Empty;
                    levReqObj.FinanceYear = dataValue.FinanceYear;
                    var list1 = selectedDatescurrectyear.OrderBy(x => x.Value.Date).ToList();
                    var first = list1[0]; //first element
                    var last = list1[list1.Count - 1]; //last element
                    levReqObj.FromDate = Convert.ToDateTime(first);
                    levReqObj.EndDate = Convert.ToDateTime(last);

                    if (first >= fromdate && first <= todate)
                    {
                        daystatus1 = 1;
                    }
                    if (last >= fromdate && last <= todate)
                    {
                        daystatus2 = 1;
                    }
                    if (daystatus1 == 1)
                    {
                        if (dataValue.fromHFDAY != 0 && Convert.ToDateTime(dataValue.FromDate) == first)
                        {
                            totaldaysvalue = totaldaysvalue - 0.5;
                            dataValue.NoOfDays = totaldaysvalue.ToString();
                        }
                    }
                    if (dataValue.FromDate != dataValue.EndDate)
                    {


                        if (daystatus2 == 1)
                        {
                            if (dataValue.ToHFDAY != 0 && Convert.ToDateTime(dataValue.EndDate) == last)
                            {
                                totaldaysvalue = totaldaysvalue - 0.5;
                                dataValue.NoOfDays = totaldaysvalue.ToString();
                            }
                        }
                    }
                    levReqObj.childflag = 1;
                    levReqObj.NoOfDays = dataValue.NoOfDays;
                    levReqObj.compid = companyId;
                    levReqObj.FinanceYear = DefaultFinancialYr.Id;
                    levReqObj.NoOfDays = dataValue.NoOfDays;
                    if (Convert.ToDateTime(dataValue.FromDate) == first)
                    {
                        levReqObj.FromDay = dataValue.FromDay;
                    }
                    else
                    {
                        levReqObj.FromDay = 0;
                    }
                    if (Convert.ToDateTime(dataValue.EndDate) == last)
                    {
                        levReqObj.ToDay = dataValue.ToDay;
                    }
                    else
                    {
                        levReqObj.ToDay = 0;
                    }


                    levReqObj.Save();
                }



                if (selectedDateswithoutcurrectyear.Count != 0)
                {
                    double finyearwithoutcount = selectedDateswithoutcurrectyear.Count;
                    int daystatus1 = 0;
                    int daystatus2 = 0;
                    double totaldaysvalue = RemovedWeekOffList.Count;
                    dataValue.NoOfDays = RemovedWeekOffList.Count.ToString();
                    levReqObj.Childid = childiid;
                    levReqObj.Id = Guid.Empty;
                    levReqObj.FinanceYear = Guid.Empty;
                    var list1 = selectedDateswithoutcurrectyear.OrderBy(x => x.Value.Date).ToList();
                    var first = list1[0]; //first element
                    var last = list1[list1.Count - 1]; //last element
                    levReqObj.FromDate = Convert.ToDateTime(first);
                    levReqObj.EndDate = Convert.ToDateTime(last);
                    if (first >= fromdate && first <= todate)
                    {
                        daystatus1 = 1;
                    }
                    if (last >= fromdate && last <= todate)
                    {
                        daystatus2 = 1;
                    }
                    if (daystatus1 == 0)
                    {
                        if (dataValue.fromHFDAY != 0 && Convert.ToDateTime(dataValue.FromDate) == first)
                        {
                            totaldaysvalue = totaldaysvalue - 0.5;
                            dataValue.NoOfDays = totaldaysvalue.ToString();
                        }
                    }
                    if (dataValue.FromDate != dataValue.EndDate)
                    {

                        if (daystatus2 == 0)
                        {
                            if (dataValue.ToHFDAY != 0 && Convert.ToDateTime(dataValue.EndDate) == last)
                            {
                                totaldaysvalue = totaldaysvalue - 0.5;
                                dataValue.NoOfDays = totaldaysvalue.ToString();
                            }
                        }
                    }
                    levReqObj.childflag = 1;
                    levReqObj.NoOfDays = dataValue.NoOfDays;
                    levReqObj.compid = companyId;
                    if (Convert.ToDateTime(dataValue.FromDate) == first)
                    {
                        levReqObj.FromDay = dataValue.FromDay;
                    }
                    else
                    {
                        levReqObj.FromDay = 0;
                    }
                    if (Convert.ToDateTime(dataValue.EndDate) == last)
                    {
                        levReqObj.ToDay = dataValue.ToDay;
                    }
                    else
                    {
                        levReqObj.ToDay = 0;
                    }


                    levReqObj.Save();
                }


#pragma warning restore

                ////dataValue.Save();
                if (dataValue.Status == 0)
                {
                    LeaveRequestMail(dataValue, employee, DefaultFinancialYr, companyId, userid, childiid);

                    return BuildJsonResult(true, 200, "Data Submitted Successfully", dataValue);

                }
                return BuildJsonResult(true, 200, "Data Saved Successfully", dataValue);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "Error while saving data", null);
            }

        }
        public List<DateTime?> FinalLeaveDateList(List<DateTime?> WeekoffLst, List<DateTime?> HolidayLst, List<DateTime?> SelectedLst)
        {
            var FinalLst = new List<DateTime?>();
            var RemovedHolidaydatesList = new List<DateTime?>();
            //HOLIDAY CHECK START
            var ExtractingHolidays = SelectedLst.Except(HolidayLst).ToList();
            for (int HDcnt = 0; HDcnt <= ExtractingHolidays.Count - 1; HDcnt++)
            {
                RemovedHolidaydatesList.Add(Convert.ToDateTime(ExtractingHolidays[HDcnt]));
            }
            //HOLIDAY CHECK END
            //Checking for applying on the weekoff date START

            var ExtractingWeekoff = RemovedHolidaydatesList.Except(WeekoffLst).ToList();
            for (int WKFINALcnt = 0; WKFINALcnt <= ExtractingWeekoff.Count - 1; WKFINALcnt++)
            {
                FinalLst.Add(Convert.ToDateTime(ExtractingWeekoff[WKFINALcnt]));
            }
            //Checking for applying on the weekoff date END
            return FinalLst;
        }
        //----
        public JsonResult SaveApprovedLeaveCancelRequest(Guid LeaveRequestId, string CancelReason)
        {
            bool SaveStat = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequestList Reqlist = new LeaveRequestList(LeaveRequestId, string.Empty);
            Employee employee = new Employee(Reqlist[0].EmployeeId);
            LeaveRequest OBJlevreq = new LeaveRequest();
            OBJlevreq.Id = Guid.NewGuid();
            OBJlevreq.compid = Convert.ToInt32(Session["CompanyId"]);
            OBJlevreq.FinanceYear = Reqlist[0].FinanceYear;
            OBJlevreq.EmployeeId = Reqlist[0].EmployeeId;
            OBJlevreq.FromDate = Reqlist[0].FromDate;
            OBJlevreq.EndDate = Reqlist[0].EndDate;
            OBJlevreq.LeaveType = Reqlist[0].LeaveType;
            OBJlevreq.FromDay = Reqlist[0].FromDay;
            OBJlevreq.ToDay = Reqlist[0].ToDay;
            OBJlevreq.Reason = CancelReason;
            OBJlevreq.Contact = Reqlist[0].Contact;
            OBJlevreq.NoOfDays = Reqlist[0].NoOfDays;
            OBJlevreq.Status = Reqlist[0].Status;
            OBJlevreq.Rejectreason = Reqlist[0].Rejectreason;
            OBJlevreq.ModifiedBy = Reqlist[0].ModifiedBy;
            OBJlevreq.CreatedBy = Reqlist[0].CreatedBy;
            OBJlevreq.IsDeleted = Reqlist[0].IsDeleted;
            OBJlevreq.ApprovedBy = Reqlist[0].ApprovedBy;
            OBJlevreq.RejectedBy = Reqlist[0].RejectedBy;
            OBJlevreq.FirstLvlContact = Reqlist[0].FirstLvlContact;
            OBJlevreq.Childid = LeaveRequestId;
            OBJlevreq.childflag = Reqlist[0].childflag;
            OBJlevreq.HRapprovalflag = Reqlist[0].HRapprovalflag;
            OBJlevreq.HRentrystatus = Reqlist[0].HRentrystatus;
            OBJlevreq.EnteredBy = Reqlist[0].EnteredBy;
            OBJlevreq.Imgpath = Reqlist[0].Imgpath;
            SaveStat = OBJlevreq.SaveApprovedLeaveCancelRequest();
            string FDate = Reqlist[0].FromDate.ToString("dd/MM/yyyy");
            string EDate = Reqlist[0].EndDate.ToString("dd/MM/yyyy");
            //LeaveType objLevType = new LeaveType();
            LeaveType objLevType = new LeaveType(Reqlist[0].LeaveType, companyId, Reqlist[0].FinanceYear);
            if (SaveStat == true)
            {
                if (Reqlist[0].Status == 1)
                {
                    AssignManager assignManager1 = new AssignManager(Reqlist[0].EmployeeId, companyId, 0, DefaultFinancialYr.Id);
                    //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
                    if (!string.IsNullOrEmpty(Reqlist[0].FirstLvlContact))
                    {
                        string sendMailTo = Reqlist[0].FirstLvlContact;
                        Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;

                        //Company company = new Company(companyId, userid);
                        string FromDay = Reqlist[0].FromDay == 0 ? "FullDay" : Reqlist[0].FromDay == 1 ? "FirstHalf" : "SecondHalf";
                        string ToDay = Reqlist[0].ToDay == 0 ? "FullDay" : Reqlist[0].ToDay == 1 ? "FirstHalf" : "SecondHalf";


                        string approveUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString();

                        string actionApproveLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + OBJlevreq.Id + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=1 &status=1 &Navtab=AppCanReq";
                        string actionRejectLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + OBJlevreq.Id + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=2 &status=2 &Navtab=AppCanReq";

                        string subject = "Request for Approved Leave Cancel from " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
                        string message = "<b> Request for Approved Leave Cancel from " + employee.FirstName + " " + employee.LastName + "</b> <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

                        message += "<tr><td align=center><font color=#0000FF size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

                        message += "<tr><td align=center>" + objLevType.LeaveTypeName + "</td>";
                        message += "<td align=center>" + FDate + "</td>";
                        message += "<td align=center>" + EDate + "</td>";
                        message += "<td align=center>" + FromDay + "</td>";
                        message += "<td align=center>" + ToDay + "</td>";
                        message += "<td align=center>" + Reqlist[0].NoOfDays + "</td></tr>";


                        message += "</table>";
                        message += "<font color=#0000FF size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + Reqlist[0].Reason;

                        string CCmsg = message;
                        message += "<div></br><a href='" + actionApproveLeave + "' > Approve</a> <a href='" + actionRejectLeave + "' > Reject</a></div>";
                        message += "<div></br><a href='" + approveUrl + "' > Click Here to Sign in to the application  </a></div>";

                        //message += "</br><div><a href='" + rejectUrl + "' > Click the link to Reject Leave</a></div>";


                        MailConfig mailConfig4 = new MailConfig(companyId);
                        string[] StrCCMail = mailConfig4.CCMail.Split(',');
                        PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, CCmsg);
                        string mailstat = null;
                        bool status = false;
                        status = payrolemail.SendTestMail(mailConfig4.IPAddress, mailConfig4.PortNo, mailConfig4.FromEmail, mailConfig4.MailPassword, mailConfig4.EnableSSL);
                        if (status)
                        {
                            mailstat = "Mail send succesfully";
                        }
                        else
                        {
                            mailstat = "Mail send failed";
                        }
                        Guid empid = new Guid(Session["EmployeeId"].ToString());
                        mailConfig4.Savemailhistory(companyId, empid, mailConfig4.FromEmail, employee.Email, null, null, "Request for Leave from ", subject, mailstat);



                        return BuildJsonResult(true, 200, "Data Submitted Successfully", "");
                    }
                    else
                    {
                        return BuildJsonResult(true, 200, "Data Submitted Successfully", "");
                    }
                }
            }
            else
            {
                return base.BuildJson(false, 400, "There is some Error occured While Saving Data", "");
            }

            return base.BuildJson(true, 200, "Request for Approved leave cancel was saved successfully", "");

        }

        public void compoffRequestMail(jsonLeaveEnty dataValue, Employee employee, LeaveFinanceYear DefaultFinancialYr, int companyId, int userid, Guid childiid)
        {
            DateTime fromDate = Convert.ToDateTime(dataValue.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(dataValue.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");
            AssignManager assignManager1 = new AssignManager(dataValue.EmployeeId, companyId, 0, DefaultFinancialYr.Id);
            //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
            if (!string.IsNullOrEmpty(dataValue.FirstLvlContact))
            {
                string sendMailTo = dataValue.FirstLvlContact;
                Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                //Guid amId = assignManager.EmployeeId;
                //var AssignMgrId = assignManager.Where(ass => ass.MgrPriority == 1).FirstOrDefault();
                //Guid amId = AssignMgrId.AssEmpId;
                Company company = new Company(companyId, userid);
                string FromDay = dataValue.FromDay == 0 ? "FullDay" : dataValue.FromDay == 1 ? "FirstHalf" : "SecondHalf";
                string ToDay = dataValue.ToDay == 0 ? "FullDay" : dataValue.ToDay == 1 ? "FirstHalf" : "SecondHalf";

                //Guid id= '3D8f0ba28d - 8ee1 - 4096 - 9a80 - 6ee190862a85';
                //   string approveUrl = "http://localhost:52993/Login/Index?empid=" + dataValue.EmployeeId+"&id="+dataValue.Id+"&amid="+amId;
                string approveUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString();
                //string rejectUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + @"/Login/Index?id=" + dataValue.Id + "&amid=" + amId;
                string actionApproveLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + childiid + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=1 &status=1 &Navtab=compoff";
                string actionRejectLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + childiid + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=2 &status=2 &Navtab=compoff";
                //strAppURL = strURL + "approvalbymail.aspx?type=Approve&GrpNo=" + strGrpNo + "&uid=" + STRUser + "&LeaderCompCode=" + STRToLeaderCompCode + "&LeaderId=" + STRToLeader;
                //string ApporrejURL = " <a href='" + activationUrl + "'>Click Here to Approve or Reject</a>";
                //string rejectUrl = "http://localhost:52993/Leave/ApproveRejectionByMail";
                //  approveUrl = approveUrl+"empid=" + dataValue.EmployeeId;

                //   string rejectUrl = Server.MapPath(@"~/Login/Index?&empid="+dataValue.EmployeeId);
                // string message = " < div style='border: 1px solid green;background-color: lightgrey; padding: 25px;margin: 25px;'>";
                string subject = "Request for Comp-off from " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
                string message = "<b> Request for Comp-off from " + employee.FirstName + " " + employee.LastName + "</b> <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

                message += "<tr><td align=center><font color=#0000FF size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

                message += "<tr><td align=center>" + dataValue.LeaveTypeName + "</td>";
                message += "<td align=center>" + FDate + "</td>";
                message += "<td align=center>" + EDate + "</td>";
                message += "<td align=center>" + FromDay + "</td>";
                message += "<td align=center>" + ToDay + "</td>";
                message += "<td align=center>" + dataValue.NoOfDays + "</td></tr>";
                //message += "<tr><td align=center><font color=#2D98eo size=2 face=Verdana> <b>Leave Reason</b></font></td>";
                //message += "<td align=center>" + dataValue.Reason + "</td></tr>";
                //message += "<tr><td><font color=red size=2 face=Verdana>Reason -</font></td><td align=left colspan=5>&nbsp;" +  + "</td></tr>";

                message += "</table>";
                message += "<font color=#0000FF size=2 face=Verdana><b>Comp-off Requested Reason: </b></font>" + dataValue.Reason;

                string CCmsg = message;
                message += "<div></br><a href='" + actionApproveLeave + "' > Approve</a> <a href='" + actionRejectLeave + "' > Reject</a></div>";
                message += "<div></br><a href='" + approveUrl + "' > Click Here to Sign in to the application  </a></div>";

                //message += "</br><div><a href='" + rejectUrl + "' > Click the link to Reject Leave</a></div>";


                MailConfig mailConfig4 = new MailConfig(companyId);
                string[] StrCCMail = mailConfig4.CCMail.Split(',');
                PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, CCmsg);
                string mailstat = null;
                bool status = false;
                status = payrolemail.SendTestMail(mailConfig4.IPAddress, mailConfig4.PortNo, mailConfig4.FromEmail, mailConfig4.MailPassword, mailConfig4.EnableSSL);
                if (status)
                {
                    mailstat = "Mail send succesfully";
                }
                else
                {
                    mailstat = "Mail send failed";
                }
                Guid empid = new Guid(Session["EmployeeId"].ToString());
                mailConfig4.Savemailhistory(companyId, empid, mailConfig4.FromEmail, employee.Email, null, null, "Request for Leave from ", subject, mailstat);
            }
        }
        public void LeaveRequestMail(jsonLeaveEnty dataValue, Employee employee, LeaveFinanceYear DefaultFinancialYr, int companyId, int userid, Guid childiid)
        {
            DateTime fromDate = Convert.ToDateTime(dataValue.FromDate);
            var FDate = fromDate.ToString("dd/MM/yyyy");
            DateTime endDate = Convert.ToDateTime(dataValue.EndDate);
            var EDate = endDate.ToString("dd/MM/yyyy");
            AssignManager assignManager1 = new AssignManager(dataValue.EmployeeId, companyId, 0, DefaultFinancialYr.Id);
            //This condition was added by mubarak in order to apply leave by employee and show success msg if there is no mail id also.
            if (!string.IsNullOrEmpty(dataValue.FirstLvlContact))
            {
                string sendMailTo = dataValue.FirstLvlContact;
                Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                //Guid amId = assignManager.EmployeeId;
                //var AssignMgrId = assignManager.Where(ass => ass.MgrPriority == 1).FirstOrDefault();
                //Guid amId = AssignMgrId.AssEmpId;
                Company company = new Company(companyId, userid);
                string FromDay = dataValue.FromDay == 0 ? "FullDay" : dataValue.FromDay == 1 ? "FirstHalf" : "SecondHalf";
                string ToDay = dataValue.ToDay == 0 ? "FullDay" : dataValue.ToDay == 1 ? "FirstHalf" : "SecondHalf";

                //Guid id= '3D8f0ba28d - 8ee1 - 4096 - 9a80 - 6ee190862a85';
                //   string approveUrl = "http://localhost:52993/Login/Index?empid=" + dataValue.EmployeeId+"&id="+dataValue.Id+"&amid="+amId;
                string approveUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString();
                //string rejectUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + @"/Login/Index?id=" + dataValue.Id + "&amid=" + amId;
                string actionApproveLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + childiid + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=1 &status=1 &Navtab=leaveReq";
                string actionRejectLeave = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/LeaveRequest/ApproveRejectionByMail?empid=" + assignManager1[0].AssEmpId + "&Leaveid=" + childiid + "&assgnmgrid=" + assignManager1[0].AssEmpId + "&prioritynum=" + assignManager1[0].MgrPriority + "&AorRstat=2 &status=2 &Navtab=leaveReq";
                //strAppURL = strURL + "approvalbymail.aspx?type=Approve&GrpNo=" + strGrpNo + "&uid=" + STRUser + "&LeaderCompCode=" + STRToLeaderCompCode + "&LeaderId=" + STRToLeader;
                //string ApporrejURL = " <a href='" + activationUrl + "'>Click Here to Approve or Reject</a>";
                //string rejectUrl = "http://localhost:52993/Leave/ApproveRejectionByMail";
                //  approveUrl = approveUrl+"empid=" + dataValue.EmployeeId;

                //   string rejectUrl = Server.MapPath(@"~/Login/Index?&empid="+dataValue.EmployeeId);
                // string message = " < div style='border: 1px solid green;background-color: lightgrey; padding: 25px;margin: 25px;'>";
                string subject = "Request for Leave from " + employee.FirstName + " " + employee.LastName + "(" + employee.EmployeeCode + ")";
                string message = "<b> Request for Leave from " + employee.FirstName + " " + employee.LastName + "</b> <br/> <table border=1 bordercolor=#cccccc bgcolor=#F1F1F1 width=500  style=border-collapse:collapse;>";

                message += "<tr><td align=center><font color=#0000FF size=2 face=Verdana> <b>LeaveType</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDate</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>FromDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>ToDay</b></font></td><td align=center><font color=#0000FF size=2 face=Verdana> <b>No Of Days</b></font></td></tr>";

                message += "<tr><td align=center>" + dataValue.LeaveTypeName + "</td>";
                message += "<td align=center>" + FDate + "</td>";
                message += "<td align=center>" + EDate + "</td>";
                message += "<td align=center>" + FromDay + "</td>";
                message += "<td align=center>" + ToDay + "</td>";
                message += "<td align=center>" + dataValue.NoOfDays + "</td></tr>";
                //message += "<tr><td align=center><font color=#2D98eo size=2 face=Verdana> <b>Leave Reason</b></font></td>";
                //message += "<td align=center>" + dataValue.Reason + "</td></tr>";
                //message += "<tr><td><font color=red size=2 face=Verdana>Reason -</font></td><td align=left colspan=5>&nbsp;" +  + "</td></tr>";

                message += "</table>";
                message += "<font color=#0000FF size=2 face=Verdana><b>Leave Requested Reason: </b></font>" + dataValue.Reason;

                string CCmsg = message;
                message += "<div></br><a href='" + actionApproveLeave + "' > Approve</a> <a href='" + actionRejectLeave + "' > Reject</a></div>";
                message += "<div></br><a href='" + approveUrl + "' > Click Here to Sign in to the application  </a></div>";

                //message += "</br><div><a href='" + rejectUrl + "' > Click the link to Reject Leave</a></div>";


                MailConfig mailConfig4 = new MailConfig(companyId);
                string[] StrCCMail = mailConfig4.CCMail.Split(',');
                PayRoleMail payrolemail = new PayRoleMail(sendMailTo, StrCCMail, subject, message, CCmsg);
                string mailstat = null;
                bool status = false;
                status = payrolemail.SendTestMail(mailConfig4.IPAddress, mailConfig4.PortNo, mailConfig4.FromEmail, mailConfig4.MailPassword, mailConfig4.EnableSSL);
                if (status)
                {
                    mailstat = "Mail send succesfully";
                }
                else
                {
                    mailstat = "Mail send failed";
                }
                Guid empid = new Guid(Session["EmployeeId"].ToString());
                mailConfig4.Savemailhistory(companyId, empid, mailConfig4.FromEmail, employee.Email, null, null, "Request for Leave from ", subject, mailstat);
            }
        }
        //----
        public class jsonAbsentTable
        {
            public Guid Id { get; set; }
            public int CompanyId { get; set; }
            public Guid FinyearId { get; set; }
            public Guid LeaveRequestId { get; set; }
            public Guid EmployeeId { get; set; }
            public Guid LeaveTypeId { get; set; }
            public DateTime LeaveDate { get; set; }
            public string LeaveDayName { get; set; }
            public float HFDay { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsDeleted { get; set; }

        }

        //-----
        public JsonResult SaveAssignManager(AssignManager dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AssignManager AssMang = new AssignManager();
            AssMang.AssEmpId = dataValue.AssEmpId;
            Employee emp = new Employee(dataValue.AssEmpId);
            if (emp.Email != "")
            {
                dataValue.Email = emp.Email;
                AssMang.Email = dataValue.Email;

                //   DataTable dtAssignManagerCheck = new DataTable();
                ///    dtAssignManagerCheck = dataValue.GetAssignManager(companyId);
                //  if (dtAssignManagerCheck.Rows.Count == 0)
                //  {
                isSaved = dataValue.Save();


                if (isSaved)
                {
                    return base.BuildJson(true, 200, "Data saved successfully", dataValue);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "Please set EmailID for Assign Employeecode", dataValue);
            }
        }
        //    else
        //    {
        //        return base.BuildJson(false, 100, "Your Company has already assigned Manager.", dataValue);
        //    }
        //}
        public JsonResult GetAssignManager(string EMPLOYEEID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid Firstlevel = new Guid(Convert.ToString(EMPLOYEEID));
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            AssignManager assignManager = new AssignManager(Firstlevel, companyId, 0, DefaultFinancialYr.Id);
            List<JsonAssignMgrML> jsondata = new List<JsonAssignMgrML>();
            assignManager.ForEach(u => { jsondata.Add(JsonAssignMgrML.toJson(u)); });
            if (jsondata.Count != 0)
            {
                return base.BuildJson(true, 200, "success", jsondata);
            }
            else
            {
                return base.BuildJson(false, 200, "success", "");
            }

        }

        public JsonResult SaveAssignManagerML(JsonAssignMgrML SaveAssignMgrValue)
        {
            bool isSaved;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id == Guid.Empty)
            {
                return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            AssignManager AssMgr = new AssignManager();
            AssMgr.Id = SaveAssignMgrValue.Id;
            AssMgr.EmployeeId = SaveAssignMgrValue.EmployeeId;
            AssMgr.CompanyId = companyId;
            AssMgr.FinYear = DefaultFinancialYr.Id;
            AssMgr.AssEmpId = SaveAssignMgrValue.AssEmpId;
            AssMgr.ApprovMust = SaveAssignMgrValue.ApprovMust;
            AssMgr.MgrPriority = SaveAssignMgrValue.MgrPriority;
            AssMgr.AppCancelRights = SaveAssignMgrValue.AppCancelRights;
            AssMgr.CreatedBy = userId.ToString();

            isSaved = AssMgr.SaveAssignMgrData();


            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", SaveAssignMgrValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", SaveAssignMgrValue);
            }
        }
        public JsonResult GetEmployeeUnderAssignManager()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            AssignManager assmgr = new AssignManager();
            EmployeeList emplist = new EmployeeList();
            DataTable dtValue = assmgr.EmployeeUnderAssignManager(employeeId, companyId, DefaultFinancialYr.Id);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    Employee employee = new Employee();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"])))
                        employee.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"]));

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeCode"])))
                        employee.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount1]["EmployeeCode"]) + " - " + Convert.ToString(dtValue.Rows[rowcount1]["EmpployeeName"]); ;

                    emplist.Add(employee);
                }
                List<jsonEmployee> jsondata = new List<jsonEmployee>();

                return base.BuildJson(true, 200, "success", emplist);
            }
            return base.BuildJson(true, 200, "no data available", emplist);
        }



        public JsonResult GetMULTILEVELAssignManager(string EMPID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                if (DefaultFinancialYr.Id == Guid.Empty)
                {
                    return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
                }
                Guid employeeId = new Guid(EMPID);
                Guid id = Guid.Empty;
                int apprmust = 0;
                //AssignManager AssignManager = new AssignManager(employeeId, companyId, Guid.Empty, id, apprmust);
                AssignManager AssignManager = new AssignManager(employeeId, companyId, DefaultFinancialYr.Id, id, apprmust);
                List<JsonAssignMgrML> jsondata = new List<JsonAssignMgrML>();
                AssignManager.ForEach(u => { jsondata.Add(JsonAssignMgrML.toJson(u)); });
                return base.BuildJson(true, 200, "success", jsondata);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "failure", ex.Message);
            }

        }
        public JsonResult GetAssMgrSelectedData(Guid Id, Guid EmpId)
        {
            try
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                if (DefaultFinancialYr.Id == Guid.Empty)
                {
                    return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
                }
                int apprmust = 0;
                AssignManager AssignManager = new AssignManager(EmpId, companyId, DefaultFinancialYr.Id, Id, apprmust);
                List<JsonAssignMgrML> jsondata = new List<JsonAssignMgrML>();
                AssignManager.ForEach(u => { jsondata.Add(JsonAssignMgrML.toJson(u)); });
                return base.BuildJson(true, 200, "success", jsondata);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(true, 400, "failure", ex.Message);
            }

        }
        public JsonResult DeleteAssignMgrData(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AssignManager AssMgrML = new AssignManager();
            AssMgrML.Id = id;
            AssMgrML.ModifiedBy = userId.ToString();
            if (AssMgrML.Delete())
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }

        }

        public JsonResult SaveChangeManager(Guid Exixtingmgrid, Guid Changemgrid)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AssignManager AssMgrML = new AssignManager();
            AssMgrML.Loginid = userId;
            AssMgrML.CompanyId = companyId;
            AssMgrML.Existingmgrid = Exixtingmgrid;
            AssMgrML.Changemgrid = Changemgrid;
            if (AssMgrML.SaveChangeManager())
            {
                return base.BuildJson(true, 200, "Manager Changed successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }

        }

        //public JsonResult GetMULTILEVELAssignManager( string EMPID)
        //{
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    Guid employeeId = new Guid(EMPID);
        //    AssignManager assignManager = new AssignManager(employeeId, companyId);
        //    return new JsonResult { Data = assignManager };
        //}
        public JsonResult Showmanagername(string EMPID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid employeeId = new Guid(EMPID);
            Employee empdetails = new Employee(companyId, employeeId);
            string managername = empdetails.FirstName + empdetails.LastName;
            return base.BuildJson(true, 200, "", managername);
        }

        public JsonResult ShowPassword(string EMPID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid employeeId = new Guid(EMPID);
            UserList User = new UserList(employeeId);
            FileLibrary objPwd = new FileLibrary();

            if (User.Count != 0)
            {
                User[0].Password = objPwd.defile(User[0].Password);
                return base.BuildJson(true, 200, "", User);
            }
            return base.BuildJson(false, 200, "This employee does not have User", null);
        }

        public JsonResult GetEmpAssignManager(Guid EmployeeId)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //  Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            AssignManager assignManager = new AssignManager(EmployeeId, companyId, 0, DefaultFinancialYr.Id);

            return new JsonResult { Data = assignManager };
        }
        public JsonResult EmpLeaveDetails(Guid employeeId, Guid leaveType)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Employee emp = new Employee(companyId, employeeId);

            if (!ReferenceEquals(emp, null) && leaveType != Guid.Empty)
            {
                LeaveOpenings opn = new LeaveOpenings(emp.Id, leaveType);
                ManagerAccess mgr = new ManagerAccess(emp.Id);
                Employee empMgr = new Employee(companyId, mgr.ManagerId);
                var result = new
                {
                    employeeId = emp.Id,
                    empCode = emp.EmployeeCode,
                    empName = emp.FirstName,
                    empDOJ = emp.DateOfJoining.ToString("dd/MMM/yyyy"),
                    balanceLeave = opn.LeaveOpening - Convert.ToDouble(opn.LeaveUsed),
                    firstlevelContact = empMgr.FirstName,
                    enteredBy = Convert.ToString(Session["UserRole"])
                };
                return BuildJsonResult(true, 200, "", result);
            }
            else
            {
                return BuildJsonResult(true, 200, "", null);
            }

        }
        public JsonResult SaveHRApprovedCancel(Guid EmployeeId, Guid leaveType, DateTime fromdate, DateTime todate, Guid levreqid, string Cancelreson)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string Loginid = Convert.ToString(Session["EmployeeId"]);
            Guid signedonid = new Guid(Convert.ToString(Session["EmployeeId"]));
            AssignManager AssMgrML = new AssignManager();

            var selectedDates = new List<DateTime?>();

            for (var date = Convert.ToDateTime(fromdate); date <= Convert.ToDateTime(todate); date = date.AddDays(1))
            {
                selectedDates.Add(date);
            }

            for (int j = 0; j <= selectedDates.Count - 1; j++)
            {
                AssMgrML.SaveApprovedcancelleavebyHR(selectedDates[j].Value, EmployeeId, leaveType, companyId, Loginid, levreqid, Cancelreson);
                HRApprovedcancelconformmail(EmployeeId, levreqid, null, signedonid);
            }




            return BuildJsonResult(true, 200, "Leave Cancelled Succesfully!!!", null);


        }
        /// <summary>
        /// Created By:Sharmila
        /// Created On:19.06.17
        /// </summary>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        /// 
        public JsonResult UpdateLeaveStatusThroughMail(int ApporRejorcancStat, int prioritynumber, Guid AssignmanagerId, Guid Id, string dataValueRejectreason, int dataValueStatus, string NavTabStatus)
        {


            try
            {
                DataTable dtnextprioritylvl = new DataTable();
                Guid empid = new Guid(Session["Employeeid"].ToString());
                LeaveRequest req = new LeaveRequest();
                List<AssignManager> AssMgr = new List<AssignManager>();
                if (ApporRejorcancStat == 1)
                {
                    dtnextprioritylvl = req.checkfornextproritynum(prioritynumber, Id);
                }


                req.updatelevreqleadertbl(AssignmanagerId, Id, ApporRejorcancStat, empid, 0);
                for (int q = 0; q < dtnextprioritylvl.Rows.Count; q++)
                {
                    AssignManager assMg = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["AssignManagerId"])))
                        assMg.AssEmpId = new Guid(dtnextprioritylvl.Rows[q]["AssignManagerId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["EmployeeId"])))
                        assMg.EmployeeId = new Guid(dtnextprioritylvl.Rows[q]["EmployeeId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["ManagerPriority"])))
                        assMg.MgrPriority = Convert.ToInt32(dtnextprioritylvl.Rows[q]["ManagerPriority"].ToString());
                    AssMgr.Add(assMg);
                }
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string username = Convert.ToString(Session["UserName"]);

                //   AttributeModelList AttrModelList = new AttributeModelList(companyId);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                Guid AttrModelID = Guid.Empty;
                //   AttributeModel AttrModel = new AttributeModel();
                Employee employee = new Employee();
                LeaveRequest lev = new LeaveRequest(Id);
                if (dtnextprioritylvl.Rows.Count > 0)
                {
                    Guid amId = new Guid(dtnextprioritylvl.Rows[0]["AssignManagerId"].ToString());
                    int prioritynum = Convert.ToInt32(dtnextprioritylvl.Rows[0]["ManagerPriority"]);
                    if (ApporRejorcancStat == 1)
                    {
                        LeaveApproveMail(lev.EmployeeId, lev.Id, amId, AssignmanagerId, prioritynum, NavTabStatus);
                    }
                    else if (ApporRejorcancStat == 2)
                    {
                        LeaveRejectMail(lev.EmployeeId, lev.Id, null, AssignmanagerId, NavTabStatus);
                    }
                }
                if ((dtnextprioritylvl.Rows.Count == 0 && ApporRejorcancStat == 1) || (ApporRejorcancStat != 1))
                {

                    if (Id != Guid.Empty)
                    {
                        var status = lev.Status;
                        switch (status)
                        {
                            case 0:
                                lev.Id = Id;
                                lev.Status = dataValueStatus;
                                lev.Rejectreason = dataValueRejectreason;
                                string compOffReqUpdatestatus = string.Empty;

                                switch (dataValueStatus)
                                {
                                    case 1:
                                        lev.ApprovedBy = userId;
                                        lev.ApprovedOn = DateTime.Now;
                                        compOffReqUpdatestatus = "APPROVED";
                                        break;
                                    case 2:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        compOffReqUpdatestatus = "REJECT";
                                        break;
                                    case 3:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        compOffReqUpdatestatus = "REJECT";
                                        break;
                                    case 4:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                }

                                lev.FinanceYear = Guid.Empty;
                                lev.compid = companyId;
                                lev.HRapprovalflag = 0;
                                lev.apprejsavethroughmail();
                                if (NavTabStatus == "leaveReq")
                                {
                                    lev.Save();
                                }
                                else if (NavTabStatus == "AppCanReq")
                                {
                                    lev.ModifiedBy = userId;
                                    lev.SaveApprovedcancelResponse();
                                }
                                else if (NavTabStatus == "compoffGainReq" || NavTabStatus == "compoff")
                                {

                                    lev.Save();
                                    lev.CompOffGainRequestUpdate(compOffReqUpdatestatus);
                                    LeaveRequestModel compOffCredit = new LeaveRequestModel();
                                    //Below line is commented in order to prohibite the 0.5 day not updating in leaveopening table at leave credit field
                                    //double creditdays = (lev.EndDate - lev.FromDate).TotalDays;
                                    double creditdays = Convert.ToDouble(lev.NoOfDays);
                                    if (compOffReqUpdatestatus == "APPROVED")
                                        //Below line is commented in order to prohibite the 0.5 day not updating in leaveopening table at leave credit field
                                        // compOffCredit.compoffCreditSave(DefaultFinancialYr.Id, lev.EmployeeId, dataValue.LeaveType, creditdays + 1);
                                        compOffCredit.compoffCreditSave(DefaultFinancialYr.Id, lev.EmployeeId, lev.LeaveType, creditdays);
                                }
                                else   //for Comp off request
                                {

                                }

                                if (lev.Status == 1)
                                {
                                    LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, AssignmanagerId, 0, NavTabStatus);
                                }
                                if (lev.Status == 2)
                                {
                                    LeaveRejectMail(lev.EmployeeId, lev.Id, null, AssignmanagerId, NavTabStatus);
                                }


                                break;



                            case 1:
                                LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, AssignmanagerId, 0, NavTabStatus);
                                break;
                            case 2:
                                LeaveRejectMail(lev.EmployeeId, lev.Id, null, AssignmanagerId, NavTabStatus);
                                break;
                        }
                        // return BuildJsonResult(true, 200, "Leave Request", lev);
                    }

                    else
                    {
                        return BuildJsonResult(false, 200, "Empoyee is not choosen", null);
                    }


                }
                Session.Abandon();
                return BuildJsonResult(true, 200, "Leave Request", lev);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "There is some error while saving the data.", null);
            }
        }

        public JsonResult UpdateLeaveStatus(jsonLeaveEnty dataValue)
        {






            try
            {
                DataTable dtnextprioritylvl = new DataTable();
                Guid empid = new Guid(Session["Employeeid"].ToString());
                LeaveRequest req = new LeaveRequest();
                List<AssignManager> AssMgr = new List<AssignManager>();
                if (dataValue.ApporRejorcancStat == 1)
                {
                    dtnextprioritylvl = req.checkfornextproritynum(dataValue.prioritynumber, dataValue.Id);
                }
                req.updatelevreqleadertbl(dataValue.AssignmanagerId, dataValue.Id, dataValue.ApporRejorcancStat, empid, 0);
                for (int q = 0; q < dtnextprioritylvl.Rows.Count; q++)
                {
                    AssignManager assMg = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["AssignManagerId"])))
                        assMg.AssEmpId = new Guid(dtnextprioritylvl.Rows[q]["AssignManagerId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["EmployeeId"])))
                        assMg.EmployeeId = new Guid(dtnextprioritylvl.Rows[q]["EmployeeId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["ManagerPriority"])))
                        assMg.MgrPriority = Convert.ToInt32(dtnextprioritylvl.Rows[q]["ManagerPriority"].ToString());
                    AssMgr.Add(assMg);
                }
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string username = Convert.ToString(Session["UserName"]);
                AttributeModelList AttrModelList = new AttributeModelList(companyId);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                Guid AttrModelID = Guid.Empty;
                AttributeModel AttrModel = new AttributeModel();
                Employee employee = new Employee();
                LeaveRequest lev = new LeaveRequest(dataValue.Id);
                if (dtnextprioritylvl.Rows.Count > 0)
                {
                    Guid amId = new Guid(dtnextprioritylvl.Rows[0]["AssignManagerId"].ToString());
                    int prioritynum = Convert.ToInt32(dtnextprioritylvl.Rows[0]["ManagerPriority"]);
                    if (dataValue.ApporRejorcancStat == 1)
                    {
                        LeaveApproveMail(lev.EmployeeId, lev.Id, amId, dataValue.AssignmanagerId, prioritynum, dataValue.NavTabStatus);
                    }
                    else if (dataValue.ApporRejorcancStat == 2)
                    {
                        LeaveRejectMail(lev.EmployeeId, lev.Id, null, dataValue.AssignmanagerId, dataValue.NavTabStatus);
                    }
                }
                if ((dtnextprioritylvl.Rows.Count == 0 && dataValue.ApporRejorcancStat == 1) || (dataValue.ApporRejorcancStat != 1))
                {
                    if (dataValue.Id != Guid.Empty)
                    {
                        var status = lev.Status;
                        string compOffReqUpdatestatus = string.Empty;
                        switch (status)
                        {
                            case 0:
                                lev.Id = dataValue.Id;
                                lev.Status = dataValue.Status;
                                //lev.NoOfDays = dataValue.NoOfDays;
                                lev.Rejectreason = dataValue.Rejectreason;

                                switch (dataValue.Status)
                                {
                                    case 1:
                                        lev.ApprovedBy = userId;
                                        lev.ApprovedOn = DateTime.Now;
                                        compOffReqUpdatestatus = "APPROVED";
                                        break;
                                    case 2://reject
                                        lev.RejectedBy = userId;
                                        compOffReqUpdatestatus = "REJECT";
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                    case 3://cancel
                                        lev.RejectedBy = userId;
                                        compOffReqUpdatestatus = "REJECT";
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                    case 4://
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                }
                                lev.FinanceYear = Guid.Empty;
                                lev.compid = companyId;
                                lev.HRapprovalflag = 0;
                                if (dataValue.NavTabStatus == "leaveReq")
                                {

                                    lev.Save();
                                }
                                else if (dataValue.NavTabStatus == "AppCanReq")
                                {
                                    lev.ModifiedBy = userId;
                                    lev.SaveApprovedcancelResponse();
                                }
                                else if (dataValue.NavTabStatus == "compoffGainReq" || dataValue.NavTabStatus == "compoff")
                                {

                                    lev.Save();
                                    lev.CompOffGainRequestUpdate(compOffReqUpdatestatus);
                                    LeaveRequestModel compOffCredit = new LeaveRequestModel();
                                    //Below line is commented in order to prohibite the 0.5 day not updating in leaveopening table at leave credit field
                                    //double creditdays = (lev.EndDate - lev.FromDate).TotalDays;
                                    double creditdays = Convert.ToDouble(lev.NoOfDays);
                                    if (compOffReqUpdatestatus == "APPROVED")
                                        //Below line is commented in order to prohibite the 0.5 day not updating in leaveopening table at leave credit field
                                        // compOffCredit.compoffCreditSave(DefaultFinancialYr.Id, lev.EmployeeId, dataValue.LeaveType, creditdays + 1);
                                        compOffCredit.compoffCreditSave(DefaultFinancialYr.Id, lev.EmployeeId, dataValue.LeaveType, creditdays);
                                }
                                else   //for Comp off request
                                {

                                }

                                if (lev.Status == 1)
                                {
                                    LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, dataValue.AssignmanagerId, 0, dataValue.NavTabStatus);
                                }
                                else if (lev.Status == 2)
                                {
                                    LeaveRejectMail(lev.EmployeeId, lev.Id, null, dataValue.AssignmanagerId, dataValue.NavTabStatus);
                                }
                                else
                                {
                                    CancelRequest(dataValue);
                                }

                                break;
                            case 1:
                                LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, dataValue.AssignmanagerId, 0, dataValue.NavTabStatus);
                                break;
                            case 2:
                                LeaveRejectMail(lev.EmployeeId, lev.Id, null, dataValue.AssignmanagerId, dataValue.NavTabStatus);
                                break;
                        }
                    }

                    else
                    {
                        return BuildJsonResult(false, 200, "Empoyee is not choosen", null);
                    }
                }
                return BuildJsonResult(true, 200, "Leave Request", lev);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "There is some error while saving the data.", null);
            }
        }







        public JsonResult PayrollProcessedCheckforapprovalleavecancel(Guid Employeeid, int month, int year)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PayrollHistoryList payrollHistoryList1 = new PayrollHistoryList(Employeeid, companyId, month, year);
            if (payrollHistoryList1.Count != 0)
            {
                return base.BuildJson(false, 400, "Payrole Process has been completed for this month!", payrollHistoryList1);
            }
            else
            {

                DateTime startOfMonth = new DateTime(year, month, 1);
                DateTime endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                LeaveRequestList levReqLst = new LeaveRequestList(startOfMonth, endOfMonth, Employeeid, 1, companyId);
                return base.BuildJson(true, 200, "Success", levReqLst);
            }
        }










        public JsonResult HRUpdateLeaveStatus(jsonLeaveEnty dataValue)
        {
            int fullflowstatus = 0;
            DataTable dtnextprioritylvl = new DataTable();
            LeaveRequest req = new LeaveRequest();
            List<AssignManager> AssMgr = new List<AssignManager>();
            Guid empid = new Guid(Session["Employeeid"].ToString());
            if (dataValue.Selectiontype == "Employee")
            {
                req.updatelevreqleadertbl(dataValue.AssignmanagerId, dataValue.Id, dataValue.ApporRejorcancStat, empid, 2);
                fullflowstatus = 1;
            }
            if (dataValue.Selectiontype == "Manager")
            {
                if (dataValue.ApporRejorcancStat == 1)
                {
                    dtnextprioritylvl = req.checkfornextproritynum(dataValue.prioritynumber, dataValue.Id);
                }
                for (int q = 0; q < dtnextprioritylvl.Rows.Count; q++)
                {
                    AssignManager assMg = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["AssignManagerId"])))
                        assMg.AssEmpId = new Guid(dtnextprioritylvl.Rows[q]["AssignManagerId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["EmployeeId"])))
                        assMg.EmployeeId = new Guid(dtnextprioritylvl.Rows[q]["EmployeeId"].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtnextprioritylvl.Rows[q]["ManagerPriority"])))
                        assMg.MgrPriority = Convert.ToInt32(dtnextprioritylvl.Rows[q]["ManagerPriority"].ToString());
                    AssMgr.Add(assMg);
                }
                req.updatelevreqleadertbl(dataValue.AssignmanagerId, dataValue.Id, dataValue.ApporRejorcancStat, empid, 1);
                if ((dtnextprioritylvl.Rows.Count == 0 && dataValue.ApporRejorcancStat == 1) || (dataValue.ApporRejorcancStat != 1))
                {
                    fullflowstatus = 1;
                }


            }
            try
            {
                int newleavecount = 0;
                var Holidaydates = new List<DateTime?>();
                var selectedDates = new List<DateTime?>();
                var selectedDatescurrectyear = new List<DateTime?>();
                var selectedDateswithoutcurrectyear = new List<DateTime?>();
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string username = Convert.ToString(Session["UserName"]);

                AttributeModelList AttrModelList = new AttributeModelList(companyId);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                Guid AttrModelID = Guid.Empty;
                AttributeModel AttrModel = new AttributeModel();
                Employee employee = new Employee();
                LeaveRequest lev = new LeaveRequest(dataValue.Id);
                if (dtnextprioritylvl.Rows.Count > 0)
                {
                    //var AssignMgrId = AssMgr.Where(ass => ass.MgrPriority == dataValue.prioritynumber + 1).FirstOrDefault();
                    //Guid amId = AssignMgrId.AssEmpId;
                    //int priority = AssignMgrId.MgrPriority;
                    Guid amId = new Guid(dtnextprioritylvl.Rows[0]["AssignManagerId"].ToString());
                    int prioritynum = Convert.ToInt32(dtnextprioritylvl.Rows[0]["ManagerPriority"]);
                    if (dataValue.ApporRejorcancStat == 1)
                    {
                        LeaveApproveMail(lev.EmployeeId, lev.Id, amId, empid, prioritynum, "");
                    }
                    else if (dataValue.ApporRejorcancStat == 2)
                    {
                        LeaveRejectMail(lev.EmployeeId, lev.Id, null, empid, "");
                    }
                }
                if (fullflowstatus == 1)
                {

                    for (var date = Convert.ToDateTime(dataValue.FromDate); date <= Convert.ToDateTime(dataValue.EndDate); date = date.AddDays(1))
                    {
                        selectedDates.Add(date);
                    }
                    newleavecount = selectedDates.Count;
                    HolidaysList holidaylist = new HolidaysList(Guid.Empty, DefaultFinancialYr.Id);

                    for (int i = 0; i <= holidaylist.Count - 1; i++)
                    {

                        for (var Holiday = holidaylist[i].Holidaydate; Holiday <= holidaylist[i].Holidaydate; Holiday = Holiday.AddDays(1))
                        {
                            Holidaydates.Add(Holiday);
                        }

                    }


                    DateTime fromdate = Convert.ToDateTime(DefaultFinancialYr.StartMonth);
                    DateTime todate = Convert.ToDateTime(DefaultFinancialYr.EndMonth);

                    string tempoorarycount = null;
                    int statuss = 0;
                    int weekffcount = 0;
                    var holidaycount = 0;
                    var temp = 0;
                    var holidaysubtract = selectedDates.Except(Holidaydates).ToList();
                    holidaycount = holidaysubtract.Count;
                    if (newleavecount == holidaycount)
                    {
                        //lev.NoOfDays = dataValue.NoOfDays;
                    }
                    else
                    {
                        temp = newleavecount - holidaycount;
                        var templeave = Convert.ToInt32(dataValue.NoOfDays) - temp;
                        tempoorarycount = templeave.ToString();
                        dataValue.NoOfDays = templeave.ToString();
                    }
                    LeaveFinanceYear leaveYear = new LeaveFinanceYear();
                    leaveYear.CompanyId = companyId;
                    //DataTable weekoffdt = leaveYear.getWeekoffdata();

                    //for (int n = 0; n <= weekoffdt.Rows.Count - 1; n++)
                    //{

                    //    string weekdayoff = weekoffdt.Rows[n]["weekoffday"].ToString();
                    //    for (int d = 0; d <= holidaysubtract.Count - 1; d++)
                    //    {
                    //        string holidayrejectedlistdayoff = holidaysubtract[d].Value.DayOfWeek.ToString();
                    //        if (holidayrejectedlistdayoff == weekdayoff)
                    //        {

                    //            weekffcount = weekffcount + 1;
                    //            statuss = 1;
                    //        }
                    //    }

                    //}
                    if (statuss == 1)
                    {
                        if (tempoorarycount == null)
                        {
                            dataValue.NoOfDays = (Convert.ToDouble(dataValue.NoOfDays) - Convert.ToDouble(weekffcount)).ToString();
                        }
                        else
                        {
                            dataValue.NoOfDays = (Convert.ToInt32(tempoorarycount) - weekffcount).ToString();
                        }

                    }


                    if (dataValue.Id != Guid.Empty)
                    {
                        var status = lev.Status;
                        switch (status)
                        {
                            case 0:
                                lev.Id = dataValue.Id;
                                lev.Status = dataValue.Status;
                                lev.NoOfDays = dataValue.NoOfDays;
                                lev.Rejectreason = dataValue.Rejectreason;

                                switch (dataValue.Status)
                                {
                                    case 1:
                                        lev.ApprovedBy = userId;
                                        lev.ApprovedOn = DateTime.Now;
                                        break;
                                    case 2:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                    case 3:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                    case 4:
                                        lev.RejectedBy = userId;
                                        lev.RejectedOn = DateTime.Now;
                                        break;
                                }

                                lev.FinanceYear = Guid.Empty;
                                lev.compid = companyId;
                                lev.HRapprovalflag = 1;
                                lev.Save();
                                if (lev.Status == 1)
                                {
                                    LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, empid, 0, "");
                                }
                                else if (lev.Status == 2)
                                {
                                    LeaveRejectMail(lev.EmployeeId, lev.Id, null, empid, "");
                                }
                                else
                                {
                                    CancelRequest(dataValue);
                                }

                                break;



                            case 1:
                                LeaveApproveMail(lev.EmployeeId, lev.Id, Guid.Empty, empid, 0, "");
                                break;
                            case 2:
                                LeaveRejectMail(lev.EmployeeId, lev.Id, null, empid, "");
                                break;
                        }
                        // return BuildJsonResult(true, 200, "Leave Request", lev);
                    }

                    else
                    {
                        return BuildJsonResult(false, 200, "Empoyee is not choosen", null);
                    }


                }
                return BuildJsonResult(true, 200, "Leave Request", lev);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "There is some error while saving the data.", null);
            }
        }
    }

    public class jsonLeaveEnty
    {
        public Guid Id { get; set; }
        public Guid FinanceYear { get; set; }
        public Guid EmployeeId { get; set; }
        public string NavTabStatus { get; set; }
        public int ApporRejorcancStat { get; set; }
        public int HRentrystatus { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public Guid LeaveType { get; set; }
        public Guid AssignmanagerId { get; set; }
        public string LeaveTypeName { get; set; }
        public string FirstLvlContact { get; set; }
        public string Rejectreason { get; set; }
        public int FromDay { get; set; }
        public int ToDay { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public string Contact { get; set; }
        public string NoOfDays { get; set; }
        public int fromHFDAY { get; set; }
        public int ToHFDAY { get; set; }
        public string Tempid { get; set; }
        public string Imgpath { get; set; }
        public int ApprovedBy { get; set; }
        public int RejectedBy { get; set; }
        public int EnteredBy { get; set; }
        public string IsAttachReq { get; set; }
        public string ApprovedOn { get; set; }
        public string RejectedOn { get; set; }
        public int prioritynumber { get; set; }
        public string Selectiontype { get; set; }
        public string HRorUser { get; set; }
        public jsonEmployee emp { get; set; }
        public double balanceLeave { get; set; }
        //    public DateTime FromDay { get;  set; }
        //   public DateTime ToDay { get;  set; }
        public bool CompOff { get; set; }
        public static jsonLeaveEnty fromDB(LeaveRequest leave)
        {
            return new jsonLeaveEnty
            {
                Id = leave.Id,
                FinanceYear = leave.FinanceYear,
                EmployeeId = leave.EmployeeId,
                FromDate = leave.FromDate.ToString("dd/MMM/yyyy"),
                EndDate = leave.EndDate.ToString("dd/MMM/yyyy"),
                LeaveType = leave.LeaveType,
                //HalfFullDay = leave.HalfFullDay,
                FromDay = leave.FromDay,
                ToDay = leave.ToDay,

                Status = leave.Status,
                Reason = leave.Reason,
                Contact = leave.Contact,
                NoOfDays = leave.NoOfDays,
                ApprovedBy = leave.ApprovedBy,
                RejectedBy = leave.RejectedBy,
                EnteredBy = leave.EnteredBy,
                ApprovedOn = leave.ApprovedOn.ToString("dd/MMM/yyyy"),
                RejectedOn = leave.RejectedOn.ToString("dd/MMM/yyyy"),
                emp = jsonEmployee.tojson(new Employee(leave.CompanyId, leave.EmployeeId)),
                balanceLeave = leave.LeaveBalance,
                FirstLvlContact = leave.FirstLvlContact,
                CompOff = leave.CompOff,
                IsAttachReq = leave.IsAttachReq,
                Imgpath = leave.Imgpath,
            };
        }

        public static LeaveRequest convertobject(jsonLeaveEnty jsLenEntry)
        {
            return new LeaveRequest()
            {
                Id = jsLenEntry.Id,
                FinanceYear = jsLenEntry.FinanceYear,
                EmployeeId = jsLenEntry.EmployeeId,
                FromDate = Convert.ToDateTime(jsLenEntry.FromDate),
                EndDate = Convert.ToDateTime(jsLenEntry.EndDate),
                LeaveType = jsLenEntry.LeaveType,
                FromDay = jsLenEntry.FromDay,
                ToDay = jsLenEntry.ToDay,
                Status = jsLenEntry.Status,
                Reason = jsLenEntry.Reason,
                Contact = jsLenEntry.Contact,
                NoOfDays = jsLenEntry.NoOfDays,
                ApprovedBy = jsLenEntry.ApprovedBy,
                RejectedBy = jsLenEntry.RejectedBy,
                EnteredBy = jsLenEntry.EnteredBy,
                ApprovedOn = Convert.ToDateTime(jsLenEntry.ApprovedOn),
                RejectedOn = Convert.ToDateTime(jsLenEntry.RejectedOn),
                LeaveBalance = jsLenEntry.balanceLeave,
                FirstLvlContact = jsLenEntry.FirstLvlContact,
            };
        }

    }


    #region "JsonAssignMgrML"
    public class JsonAssignMgrML
    {

        public Guid Id { set; get; }
        public Guid EmployeeId { get; set; }
        public Guid FinYear { get; set; }
        public string MgrEmpCode { get; set; }
        public string MgrEmpName { get; set; }
        public int CompanyId { get; set; }
        public int ApprovMust { get; set; }
        public int MgrPriority { get; set; }
        public int AppCancelRights { get; set; }
        public string CreatedBy { get; set; }
        public string LevStatus { get; set; }
        public string ModifiedBy { get; set; }
        public string ApprovMustString { get; set; }
        public string AppCancelRightString { get; set; }
        public string Email { get; set; }
        public Guid AssEmpId { get; set; }
        public string firstlevelData { get; set; }

        public static JsonAssignMgrML toJson(AssignManager AssMgr)
        {
            return new JsonAssignMgrML()
            {
                Id = AssMgr.Id,
                EmployeeId = AssMgr.EmployeeId,
                AssEmpId = AssMgr.AssEmpId,
                FinYear = AssMgr.FinYear,
                CompanyId = AssMgr.CompanyId,
                ApprovMust = AssMgr.ApprovMust,
                MgrPriority = AssMgr.MgrPriority,
                AppCancelRights = AssMgr.AppCancelRights,
                CreatedBy = AssMgr.CreatedBy,
                MgrEmpCode = AssMgr.MgrEmpCode,
                MgrEmpName = AssMgr.MgrEmpName,
                LevStatus = AssMgr.LevStatus,
                Email = AssMgr.Email,
                ApprovMustString = AssMgr.ApprovMustString,
                AppCancelRightString = AssMgr.AppCancelRightString,
                firstlevelData = AssMgr.MgrEmpCode + "-" + AssMgr.MgrEmpName
            };
        }
        public static AssignManager convertObject(JsonAssignMgrML AssMgrJson)
        {
            return new AssignManager()
            {
                Id = AssMgrJson.Id,
                EmployeeId = AssMgrJson.EmployeeId,
                FinYear = AssMgrJson.FinYear,
                CompanyId = AssMgrJson.CompanyId,
                ApprovMust = AssMgrJson.ApprovMust,
                MgrPriority = AssMgrJson.MgrPriority,
                AppCancelRights = AssMgrJson.AppCancelRights,
                CreatedBy = AssMgrJson.CreatedBy,
                MgrEmpCode = AssMgrJson.MgrEmpCode,
                MgrEmpName = AssMgrJson.MgrEmpName


            };
        }
    }
    #endregion
    public class emailapproval
    {
        public int appstatus { get; set; }
        public Guid assgnmngrid { get; set; }
        public Guid Leaveid { get; set; }
        public int prioritynum { get; set; }
        public int AorRstat { get; set; }
        public int status { get; set; }
        public string displaystatus { get; set; }
        public string NavTab { get; set; }

    }


    public class UserMenu
    {

        public Employee empproperty;
        public emailapproval emailapprvalproperty;
    }
    #region "LeaveReportExport"
    public class jsonLeaveReportExport
    {
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }

        public string FromDate { get; set; }
        public string FromDay { get; set; }
        public string ToDate { get; set; }
        public string ToDay { get; set; }
        public string NoOfDays { get; set; }
        public string LeavStatus { get; set; }



        public jsonLeaveReportExport tojson(LeaveRequest lr)
        {
            string fromdaytype = string.Empty;
            string EndDayType = string.Empty;
            if (lr.FromDay == 0)
            {
                fromdaytype = "Full Day";
            }
            else if (lr.FromDay == 1)
            {
                fromdaytype = "First Half";
            }
            else if (lr.FromDay == 2)
            {
                fromdaytype = "Second Half";
            }
            if (lr.ToDay == 0)
            {
                EndDayType = "Full Day";
            }
            else if (lr.ToDay == 1)
            {
                EndDayType = "First Half";
            }
            else if (lr.ToDay == 2)
            {
                EndDayType = "Second Half";
            }
            return new jsonLeaveReportExport
            {
                EmployeeCode = lr.Empcode,
                EmployeeName = lr.EmployeeName,
                FromDate = lr.FromDate.ToString("dd-M-yyyy"),
                FromDay = fromdaytype,
                ToDate = lr.EndDate.ToString("dd-M-yyyy"),
                ToDay = EndDayType,
                NoOfDays = lr.NoOfDays,
                LeaveType = lr.LeaveTypeName,
                LeavStatus = lr.LeaveStatus

            };
        }





    }
    #endregion
    #region LeaveBalanceReportExport
    public class jsonLeaveBalanceReportExport
    {
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public double LeaveOpening { get; set; }
        public double LeaveCredits { get; set; }
        public double TotalLeave { get; set; }
        public double LeaveUsed { get; set; }
        public double DebitLeaves { get; set; }
        public double BalanceLeave { get; set; }





        public jsonLeaveBalanceReportExport toExportjson(LeaveRequest lr)
        {

            return new jsonLeaveBalanceReportExport
            {
                EmployeeCode = lr.Empcode,
                EmployeeName = lr.EmployeeName,
                LeaveType = lr.LeaveTypeName,
                LeaveOpening = lr.Leaveopen,
                LeaveCredits = lr.Leavecred,
                TotalLeave = lr.Totaldays,
                LeaveUsed = lr.Useddays,
                DebitLeaves = lr.Debitdays,
                BalanceLeave = lr.LeaveBalance

            };
        }




    }
    #endregion
}
