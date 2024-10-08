using PayrollBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using NotificationEngine;
using System.Reflection;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using PayrollBO.Leave;
using TraceError;
using Payroll.CustomFilter;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Text;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class LeaveController : BaseController
    {
        // GET: Leave
        #region "Leave Finance Year"

        public ActionResult HolidayRevertleave()
        {
            //  LeaveApproveMail(empid, id);
            return View("~/Views/Leave/HolidayRevertleave.cshtml");

        }
        public JsonResult Getweekoffcmpmatchings()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            WeekoffComponentMatchingList weekcmpmatchinglist = new WeekoffComponentMatchingList(companyId);
            List<jsonwkcmpmtching> jsondata = new List<jsonwkcmpmtching>();
            weekcmpmatchinglist.ForEach(u => { jsondata.Add(jsonwkcmpmtching.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetFinanceYears()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveFinanceYearList financeYearlist = new LeaveFinanceYearList(companyId);
            List<jsonleavFinanceYear> jsondata = new List<jsonleavFinanceYear>();
            financeYearlist.ForEach(u => { jsondata.Add(jsonleavFinanceYear.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetFinanceYear(Guid financeYearId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear financeYearlist = new LeaveFinanceYear(financeYearId);
            financeYearlist = new LeaveFinanceYear(financeYearId);
            if (!object.ReferenceEquals(financeYearlist, null))
            {

                return base.BuildJson(true, 200, "success", jsonleavFinanceYear.toJson(financeYearlist));
            }
            else
            {
                return base.BuildJson(false, 200, "Please Complete Finance Year Setting", jsonleavFinanceYear.toJson(financeYearlist));
            }

        }
        public JsonResult SaveCompOffSettings(string compOffparameter, string Compoffdays, string Compoffdate, string Compoffvalidtype)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            try
            {
                CompOffBO leaveYear = new CompOffBO();
                bool isSaved = false;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                Guid tempparams = compOffparameter == "select" ? Guid.Empty : new Guid(compOffparameter);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                isSaved = leaveYear.SaveCompoffsettings(tempparams, Convert.ToInt32(Compoffdays), Convert.ToDateTime(Compoffdate), Convert.ToString(Compoffvalidtype), companyId, userId, DefaultFinancialYr.Id);


                return base.BuildJson(true, 200, "Data saved successfully", "");
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the data.", "");
            }
        }
        public JsonResult SelectCompOffSettings(string compoffParameter)
        {
            try
            {
                CompOffBO leaveYear = new CompOffBO();
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                compoffParameter = string.IsNullOrEmpty(compoffParameter) ? Guid.Empty.ToString() : compoffParameter;
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                DataTable DTCompoffsettings = leaveYear.SelectCompoffsettings(companyId, new Guid(compoffParameter), DefaultFinancialYr.Id);
                List<jsonCompoffsettings> result = new List<jsonCompoffsettings>();
                for (int i = 0; i < DTCompoffsettings.Rows.Count; i++)
                {
                    jsonCompoffsettings CSsettings = new jsonCompoffsettings();
                    if (!string.IsNullOrEmpty(Convert.ToString(DTCompoffsettings.Rows[i]["CreditvalidityType"])))
                        CSsettings.CSDAYS = Convert.ToInt32(DTCompoffsettings.Rows[i]["CreditvalidityType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(DTCompoffsettings.Rows[i]["CreditvalidityDate"])))
                        CSsettings.CSDATE = Convert.ToDateTime(DTCompoffsettings.Rows[i]["CreditvalidityDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(DTCompoffsettings.Rows[i]["Creditvaliditydays"])))
                        CSsettings.CSTYPE = Convert.ToInt32(DTCompoffsettings.Rows[i]["Creditvaliditydays"]);
                    CSsettings.compoffParameter = Convert.ToString(DTCompoffsettings.Rows[i]["compoffParameter"]);

                    result.Add(CSsettings);
                }
                return base.BuildJson(true, 200, "Data saved successfully", result);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the data.", "");
            }
        }



        public JsonResult GetCompOffsettings()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId, true);
            Guid LoginEmpId = new Guid(Session["EmployeeId"].ToString());
            CompOffBO leaveYear = new CompOffBO();
            DataTable DTCompoffsettings = leaveYear.SelectCompoffsettings(companyId, Guid.Empty, DefaultFinancialYr.Id);
            var result = jsonSerializedDtToString(DTCompoffsettings);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteComOffSettings(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            CompOffBO leaveYear = new CompOffBO();
            leaveYear.Id = id;
            leaveYear.UserId = Convert.ToInt32(Session["UserId"]);
            if (leaveYear.CompOffsettingsDelete())
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", "");
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", "");
            }
        }
        public JsonResult GetDefaultFinaceYear()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveFinanceYearList financeYearlist = new LeaveFinanceYearList(companyId);
            LeaveFinanceYear defaultyear = new LeaveFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsDefault).FirstOrDefault();
            return base.BuildJson(true, 200, "success", jsonleavFinanceYear.toJson(defaultyear));
        }
        public JsonResult SaveFinanceYear(jsonleavFinanceYear dataValue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            try
            {
                LeaveFinanceYear leaveYear = jsonleavFinanceYear.convertObject(dataValue);
                bool isSaved = false;
                bool NewFinYear = false;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefYear = new LeaveFinanceYear(companyId);
                List<AssignManager> objMgrLst = new List<AssignManager>();
                AssignManager objAssingMgr = new AssignManager();
                DataTable dt = new DataTable();
                dt = leaveYear.finyrcheck(companyId);
                if (dt.Rows.Count != 0)
                {
                    NewFinYear = true;
                    if (dt.Rows[0]["Id"].ToString() != dataValue.id.ToString())
                    {
                        return base.BuildJson(false, 100, "Finance year Already Exist!!!", dataValue);
                    }

                }
                if (DefYear.Id==Guid.Empty)
                {
                    NewFinYear = true;
                }
                else
                {
                    objMgrLst = new AssignManager(Guid.Empty, companyId, DefYear.Id, Guid.Empty, 0);
                }
                int userId = Convert.ToInt32(Session["UserId"]);
                leaveYear.CompanyId = companyId;
                leaveYear.CreatedBy = userId;
                leaveYear.ModifiedBy = leaveYear.CreatedBy;
                leaveYear.IsDeleted = false;
                isSaved = leaveYear.Save();
                //Inserting Assignmanager data for new created fin year.
                if (NewFinYear == false && isSaved==true)
                {
                    AssignMgrCreationFinyear(objMgrLst, leaveYear.Id);
                }
                leaveYear.CheckLeavewithfinYr();
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public void AssignMgrCreationFinyear(List<AssignManager> objMgrLst,Guid newFinYear)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                objMgrLst.ForEach(k =>
                {
                    AssignManager objMgr = new AssignManager();
                    objMgr.Id = Guid.Empty;
                    objMgr.EmployeeId = k.EmployeeId;
                    objMgr.CompanyId = k.CompanyId;
                    objMgr.FinYear = newFinYear;
                    objMgr.AssEmpId = k.AssEmpId;
                    objMgr.ApprovMust = k.ApprovMust;
                    objMgr.MgrPriority = k.MgrPriority;
                    objMgr.AppCancelRights = k.AppCancelRights;
                    objMgr.CreatedBy = userId.ToString();
                    objMgr.SaveAssignMgrData();
                });
            }
            catch (Exception ex)
            {

                ErrorLog.Log(ex);
            }
        }
        public JsonResult DeleteFinanceYear(Guid finId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequestList LevReqlist = new LeaveRequestList(Guid.Empty, companyId, finId);
            //LeaveOpeningList LevOpplist = new LeaveOpeningList(Guid.Empty, finId, Guid.Empty);
            //HolidaysList Holidaylist = new HolidaysList(Guid.Empty, finId);
            int userId = Convert.ToInt32(Session["UserId"]);
            LeaveFinanceYear leave = new LeaveFinanceYear();
            leave.Id = finId;
            leave.ModifiedBy = userId;




            if (DefaultFinancialYr.Id != finId)
            {
                if (LevReqlist.Count == 0)
                {
                    if (leave.Delete())
                    {
                        return base.BuildJson(true, 200, "Data deleted successfully", null);
                    }

                    else
                    {
                        return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
                    }
                }
                else
                {
                    return base.BuildJson(false, 100, "This financial year is already Used.", null);
                }

            }
            else
            {
                return base.BuildJson(false, 100, "This is default year ", null);
            }
        }
        public class jsonwkcmpmtching
        {

            public Guid id { get; set; }

            public Guid leavecategoryid { get; set; }

            public string leavecomponent { get; set; }



            public static jsonwkcmpmtching toJson(WeekoffComponentMatching wkoffcmpmatchng)
            {
                return new jsonwkcmpmtching()
                {
                    id = wkoffcmpmatchng.Id,

                    leavecategoryid = wkoffcmpmatchng.LeaveCategoryId,
                    leavecomponent = wkoffcmpmatchng.Leavecomponent.ToString()

                };
            }
            public static WeekoffComponentMatching convertObject(jsonwkcmpmtching wkoffcmpmatchng)
            {
                return new WeekoffComponentMatching()
                {
                    Id = wkoffcmpmatchng.id,
                    LeaveCategoryId = wkoffcmpmatchng.leavecategoryid,
                    Leavecomponent = wkoffcmpmatchng.leavecomponent

                };
            }
        }

        public class jsonleavFinanceYear
        {

            public Guid id { get; set; }

            public string startDate { get; set; }

            public string EndDate { get; set; }

            public bool defaultyear { get; set; }

            public static jsonleavFinanceYear toJson(LeaveFinanceYear txFinance)
            {
                return new jsonleavFinanceYear()
                {
                    id = txFinance.Id,
                    startDate = txFinance.StartMonth.ToString("dd/MMM/yyyy"),
                    EndDate = txFinance.EndMonth.ToString("dd/MMM/yyyy"),
                    defaultyear = txFinance.IsDefault
                };
            }
            public static LeaveFinanceYear convertObject(jsonleavFinanceYear txFinance)
            {
                return new LeaveFinanceYear()
                {
                    Id = txFinance.id,
                    StartMonth = Convert.ToDateTime(txFinance.startDate),
                    EndMonth = Convert.ToDateTime(txFinance.EndDate),
                    IsDefault = txFinance.defaultyear

                };
            }
        }
        public class jsonCreditLeave
        {

            public Guid id { get; set; }

            public string EmployeeCode { get; set; }

            public string LeaveDate { get; set; }

            public string LeaveType { get; set; }
            public string Reason { get; set; }

            public decimal Credit { get; set; }

            public static jsonCreditLeave toJson(CreditLeave crdL, string empcode)
            {
                return new jsonCreditLeave()
                {
                    id = crdL.Id,
                    EmployeeCode = empcode,
                    LeaveDate = crdL.CreditLeaveEntryDate.ToString("dd/MMM/yyyy"),
                    LeaveType = crdL.LeaveType,
                    Credit = crdL.NoOfDays,
                    Reason = crdL.Reason
                };
            }
            public static LeaveFinanceYear convertObject(jsonleavFinanceYear txFinance)
            {
                return new LeaveFinanceYear()
                {
                    Id = txFinance.id,
                    StartMonth = Convert.ToDateTime(txFinance.startDate),
                    EndMonth = Convert.ToDateTime(txFinance.EndDate),
                    IsDefault = txFinance.defaultyear

                };
            }
        }
        public class jsonDebitLeave
        {

            public Guid id { get; set; }

            public string EmployeeCode { get; set; }

            public string LeaveDate { get; set; }

            public string LeaveType { get; set; }
            public string Reason { get; set; }
            public decimal Debit { get; set; }

            public static jsonDebitLeave toJson(DebitLeave debL, string empcode)
            {
                return new jsonDebitLeave()
                {
                    id = debL.Id,
                    EmployeeCode = empcode,
                    LeaveDate = debL.DebitLeaveEntryDate.ToString("dd/MMM/yyyy"),
                    LeaveType = debL.LeaveType,
                    Debit = debL.NoOfDays,
                    Reason = debL.Reason
                };
            }
            public static LeaveFinanceYear convertObject(jsonleavFinanceYear txFinance)
            {
                return new LeaveFinanceYear()
                {
                    Id = txFinance.id,
                    StartMonth = Convert.ToDateTime(txFinance.startDate),
                    EndMonth = Convert.ToDateTime(txFinance.EndDate),
                    IsDefault = txFinance.defaultyear

                };
            }
        }
        #endregion
        public class jsonAssignedmanager
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string EmployeeCode { get; set; }

            public int ManagerPriority { get; set; }

            public static jsonAssignedmanager toJson(AssignManager debL)
            {
                return new jsonAssignedmanager()
                {
                    Id = debL.Id,
                    FirstName = debL.FirstName,
                    EmployeeCode = debL.EmployeeCode,
                    ManagerPriority = debL.ManagerPriority
                };

            }
        }

        #region "employeeHolidayList"


        public JsonResult GetEmpHolidaylistdata()
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveRequest LeaverequestBO = new LeaveRequest();
            LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialid.Id);                                                           //LEAVE MASTER LIST
            Employee employee = new Employee(new Guid(Convert.ToString(Session["EmployeeId"])));

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
            HolidaysList HolidayList = new HolidaysList(Guid.Empty, DefaultFinancialid.Id, new Guid(Holidayparameterid));

            //  HolidaysList holidaylist = new HolidaysList(Guid.Empty, DefaultFinancialid.Id);

            return base.BuildJson(true, 200, "", HolidayList);


        }

        #endregion


        #region "employeeleavedashboard"


        public JsonResult GetLeaveBalanceReport(Guid Empid)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            DataTable dt = DefaultFinancialYr.GetemployeeLeaveBalanceReport(Empid, DefaultFinancialYr.Id);


            List<jsonFullcalendar> result = new List<jsonFullcalendar>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonFullcalendar EMPdashboard = new jsonFullcalendar();

                EMPdashboard.Empcode = (Convert.ToString(dt.Rows[i]["EmployeeCode"])) + "-" + (Convert.ToString(dt.Rows[i]["Name"]));

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalLeave"])))
                    EMPdashboard.TotalDays = Convert.ToDouble(dt.Rows[i]["TotalLeave"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leaveused"])))
                    EMPdashboard.UsedDays = Convert.ToDouble(dt.Rows[i]["Leaveused"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Debitdays"])))
                    EMPdashboard.Debitdays = Convert.ToDouble(dt.Rows[i]["Debitdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leave Name"])))
                    EMPdashboard.LeaveTitle = Convert.ToString(dt.Rows[i]["Leave Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["leaveAvailable"])))
                    EMPdashboard.AvailableDays = Convert.ToDouble(dt.Rows[i]["leaveAvailable"]);
                EMPdashboard.UsedDays = EMPdashboard.UsedDays + EMPdashboard.Debitdays;
                result.Add(EMPdashboard);
            }
            return base.BuildJson(true, 200, "", result);

        }

        public JsonResult GetLevbalforyearend(Guid Empid)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            DataTable dt = DefaultFinancialYr.GetemployeeLeaveBalanceReport(Empid, DefaultFinancialYr.Id);


            List<jsonFullcalendar> result = new List<jsonFullcalendar>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonFullcalendar EMPdashboard = new jsonFullcalendar();

                EMPdashboard.Empcode = (Convert.ToString(dt.Rows[i]["EmployeeCode"])) + "-" + (Convert.ToString(dt.Rows[i]["Name"]));
                MonthlyLeaveLimit monthlyleavelimit = new MonthlyLeaveLimit();
                var data = monthlyleavelimit.GetMonthlyLeaveLimit(companyId);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalLeave"])))
                    EMPdashboard.TotalDays = Convert.ToDouble(dt.Rows[i]["TotalLeave"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leaveused"])))
                    EMPdashboard.UsedDays = Convert.ToDouble(dt.Rows[i]["Leaveused"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Debitdays"])))
                    EMPdashboard.Debitdays = Convert.ToDouble(dt.Rows[i]["Debitdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leave Name"])))
                    EMPdashboard.LeaveTitle = Convert.ToString(dt.Rows[i]["Leave Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["leaveAvailable"])))
                    EMPdashboard.AvailableDays = Convert.ToDouble(dt.Rows[i]["leaveAvailable"]);
                EMPdashboard.UsedDays = EMPdashboard.UsedDays + EMPdashboard.Debitdays;

                EMPdashboard.CreditDays = EMPdashboard.UsedDays + EMPdashboard.Debitdays;
                result.Add(EMPdashboard);
            }
            return base.BuildJson(true, 200, "", result);

        }
        public JsonResult GetEmpdashboarddata()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            int user = Convert.ToInt32(Session["UserId"]);
            User emp = new User(user);
            Guid empid = emp.EmployeeId;
            EmployeeList emplist = new EmployeeList(companyId, Guid.Empty);
            string fname = null;
            string lname = null;
            string empcode = null;
            var temp = emplist.Where(u => u.Id == empid).ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == empid)
                {
                    fname = temp[i].FirstName;
                    lname = temp[i].LastName;
                    empcode = temp[i].EmployeeCode;
                }
            }

            string employeename = empcode + "-" + fname + lname;
            DataTable dt = DefaultFinancialYr.Getemployeedashboarddata(empid, DefaultFinancialYr.Id);

            // string toptitle = (Convert.ToString(dt.Rows[0]["EmployeeCode"]))+"-" + (Convert.ToString(dt.Rows[0]["Name"]));
            List<jsonFullcalendar> result = new List<jsonFullcalendar>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonFullcalendar EMPdashboard = new jsonFullcalendar();

                EMPdashboard.Empcode = (Convert.ToString(dt.Rows[i]["EmployeeCode"])) + "-" + (Convert.ToString(dt.Rows[i]["Name"]));

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalLeave"])))
                    EMPdashboard.TotalDays = Convert.ToDouble(dt.Rows[i]["TotalLeave"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leaveused"])))
                    EMPdashboard.UsedDays = Convert.ToDouble(dt.Rows[i]["Leaveused"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Debitdays"])))
                    EMPdashboard.Debitdays = Convert.ToDouble(dt.Rows[i]["Debitdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leave Name"])))
                    EMPdashboard.LeaveTitle = Convert.ToString(dt.Rows[i]["Leave Name"]);


                EMPdashboard.AvailableDays = EMPdashboard.TotalDays - EMPdashboard.UsedDays - EMPdashboard.Debitdays;
                EMPdashboard.UsedDays = EMPdashboard.UsedDays + EMPdashboard.Debitdays;
                result.Add(EMPdashboard);
            }
            return base.BuildJson(true, 200, employeename, result);

        }
        #endregion
        /// <summary>
        /// BalanceLeave
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="leaveType"></param>
        /// <returns></returns>
        public JsonResult BalanceLeave(Guid empid, Guid leaveType)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            int user = Convert.ToInt32(Session["UserId"]);



            DataTable dt = DefaultFinancialYr.Getemployeedashboarddata(empid, DefaultFinancialYr.Id);


            List<jsonFullcalendar> result = new List<jsonFullcalendar>();
            jsonFullcalendar EMPdashboard = new jsonFullcalendar();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToString(dt.Rows[i]["LeaveType"]) == leaveType.ToString())
                {


                    EMPdashboard.Empcode = (Convert.ToString(dt.Rows[i]["EmployeeCode"])) + "-" + (Convert.ToString(dt.Rows[i]["Name"]));

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalLeave"])))
                        EMPdashboard.TotalDays = Convert.ToDouble(dt.Rows[i]["TotalLeave"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leaveused"])))
                        EMPdashboard.UsedDays = Convert.ToDouble(dt.Rows[i]["Leaveused"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leave Name"])))
                        EMPdashboard.LeaveTitle = Convert.ToString(dt.Rows[i]["Leave Name"]);
                    decimal debitd = 0;
                    DebitLeaveList dbllist = new DebitLeaveList(companyId, DefaultFinancialYr.Id, empid);
                    dbllist.ForEach(f =>
                    {
                        if (f.DebitLevType == leaveType)
                        {
                            debitd = debitd + f.NoOfDays;
                        }
                    });

                    EMPdashboard.AvailableDays = EMPdashboard.TotalDays - EMPdashboard.UsedDays - Convert.ToDouble(debitd);

                }
            }



            return base.BuildJson(true, 200, "", EMPdashboard);

        }



        public JsonResult AssignedUserselect(Guid EmployeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AssignManager AssignManager = new AssignManager(EmployeeId, companyId);
            List<jsonAssignedmanager> jsondata = new List<jsonAssignedmanager>();
            AssignManager.ForEach(u => { jsondata.Add(jsonAssignedmanager.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }


        public JsonResult GetDebitLeave(Guid EMPID)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            Employee emp = new Employee(companyId, EMPID);
            DebitLeaveList dbllist = new DebitLeaveList(companyId, DefaultFinancialYr.Id, EMPID);
            List<jsonDebitLeave> jsondata = new List<jsonDebitLeave>();
            dbllist.ForEach(u => { jsondata.Add(jsonDebitLeave.toJson(u, emp.EmployeeCode)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetCreditLeave(Guid EMPID)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            Employee emp = new Employee(companyId, EMPID);
            CreditLeaveList dbllist = new CreditLeaveList(companyId, DefaultFinancialYr.Id, EMPID);
            List<jsonCreditLeave> jsondata = new List<jsonCreditLeave>();
            dbllist.ForEach(v => { jsondata.Add(jsonCreditLeave.toJson(v, emp.EmployeeCode)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult SaveDebitleave(List<DebitLeave> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            int userId = Convert.ToInt32(Session["UserId"]);

            dataValue.ForEach(data =>
            {
                DebitLeave Dbl = data;

                Dbl.FinanceYearId = DefaultFinancialYr.Id;
                Dbl.CompanyId = companyId;
                Dbl.CreatedBy = userId;
                Dbl.ModifiedBy = Dbl.CreatedBy;
                Dbl.IsDeleted = false;
                isSaved = Dbl.Save();

            });

            if (isSaved)
            {

                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult ExportDebitcreditReport(string Person, string Type, Guid UserId)
        {
            try
            {

                int company = Convert.ToInt32(Session["CompanyId"]);
                LeaveRequest LevRequest = new LeaveRequest();
                Guid EmployeeId = Guid.Empty;
                DataTable dtvalue = new DataTable();
                LeaveFinanceYear DefaultFinancialYr1 = new LeaveFinanceYear(company);
                dtvalue = DefaultFinancialYr1.GetCreditordebit(Person, Type, UserId, DefaultFinancialYr1.Id, company);
                //List<jsonDebitLeave> jsondata = new List<jsonDebitLeave>();


                //for (int i = 0; i < dtvalue.Rows.Count; i++)
                //{
                //    jsonDebitLeave LevExp = new jsonDebitLeave();

                //    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["EMPLOYEE NAME"])))
                //        LevExp.EmployeeCode = Convert.ToString(dtvalue.Rows[i]["EMPLOYEE NAME"]);
                //    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["DATE"])))
                //        LevExp.LeaveDate = Convert.ToString(dtvalue.Rows[i]["DATE"]);
                //    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["LEAVE TYPE"])))
                //        LevExp.LeaveType = Convert.ToString(dtvalue.Rows[i]["LEAVE TYPE"]);
                //    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["No.of DAYS"])))
                //        LevExp.Debit = Convert.ToDecimal(dtvalue.Rows[i]["No.of DAYS"]);
                //    if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["REASON"])))
                //        LevExp.Reason = Convert.ToString(dtvalue.Rows[i]["REASON"]);
                //    jsondata.Add(LevExp.toExportjson(LevReqLst));

                //}



                //DataTable dtvalue1 = LevRequest.ToDataTable(resultExport);
                if (dtvalue.Rows.Count != 0)
                {
                    GridView GridView1 = new GridView();
                    GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell1 = new TableCell();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtvalue;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GridView1.RenderControl(hw);
                    string ExcelFilePath = string.Empty;

                    if (Type == "Debit")
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "DebitReport.xls";
                    }
                    else
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "CreditReport.xls";
                    }

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
        public JsonResult HrmangerExportReport(string Person, string Type, Guid UserId, Guid Finyr, string Report, Guid Emptype)
        {
            try
            {

                int company = Convert.ToInt32(Session["CompanyId"]);
                LeaveRequest LevRequest = new LeaveRequest();
                Guid EmployeeId = Guid.Empty;
                DataTable dtvalue = new DataTable();
                LeaveFinanceYear DefaultFinancialYr1 = new LeaveFinanceYear(company);
                dtvalue = DefaultFinancialYr1.HRmanagerexport(Person, Type, UserId, Finyr, company, Report, Emptype);

                if (dtvalue.Rows.Count != 0)
                {
                    GridView GridView1 = new GridView();
                    GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell1 = new TableCell();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dtvalue;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GridView1.RenderControl(hw);
                    string ExcelFilePath = string.Empty;

                    if (Type == "Debit" && Emptype == new Guid("11111111-1111-1111-1111-111111111111"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "Active Employees_DebitReport.xls";
                    }
                    else if (Type == "Debit" && Emptype == new Guid("22222222-2222-2222-2222-222222222222"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "InActive Employees_DebitReport.xls";
                    }
                    else if (Type == "Debit" && Emptype == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "All Employees_DebitReport.xls";
                    }
                    if (Type == "Credit" && Emptype == new Guid("11111111-1111-1111-1111-111111111111"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "Active Employees_CreditReport.xls";
                    }
                    else if (Type == "Credit" && Emptype == new Guid("22222222-2222-2222-2222-222222222222"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "InActive Employees_CreditReport.xls";
                    }
                    else if (Type == "Credit" && Emptype == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "All Employees_CreditReport.xls";
                    }

                    if (Type == "Gainreport" && Emptype == new Guid("11111111-1111-1111-1111-111111111111"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "Active Employees_Compoff Gain Report.xls";
                    }
                    else if (Type == "Gainreport" && Emptype == new Guid("22222222-2222-2222-2222-222222222222"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "InActive Employees_Compoff Gain Report.xls";
                    }
                    else if (Type == "Gainreport" && Emptype == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "All Employees_Compoff Gain Report.xls";
                    }
                    else
                    {
                        // ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + "CreditReport.xls";
                    }

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





        public JsonResult SaveCreditleave(List<CreditLeave> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            int userId = Convert.ToInt32(Session["UserId"]);

            dataValue.ForEach(data =>
            {
                CreditLeave Crdlev = data;

                Crdlev.FinanceYearId = DefaultFinancialYr.Id;
                Crdlev.CompanyId = companyId;
                Crdlev.CreatedBy = userId;
                Crdlev.ModifiedBy = Crdlev.CreatedBy;
                Crdlev.IsDeleted = false;
                isSaved = Crdlev.Save();

            });

            if (isSaved)
            {

                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult DeleteDebitLeave(Guid finId)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            DebitLeave leave = new DebitLeave();
            leave.Id = finId;
            leave.ModifiedBy = userId;

            if (leave.Delete())
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }

            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }


        }

        #region "Manager Eligiblity"

        public JsonResult Savemanager(List<Jsonmanagereligibility> attr)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            ManagerEligiblity managerEligiblity = new ManagerEligiblity();
            ManagerEligiblity managereligiblityUpdate = new ManagerEligiblity(companyId, DefaultFinancialYr.Id);
            attr.ForEach(a =>
            {
                ManagerEligiblity managereligiblityID = new ManagerEligiblity(companyId, a.Id, DefaultFinancialYr.Id);
                managerEligiblity.Id = managereligiblityID.Id;
                managerEligiblity.RoleId = a.Id;
                managerEligiblity.FinanacialYear = DefaultFinancialYr.Id;
                managerEligiblity.CompanyId = companyId;
                managerEligiblity.FieldName = a.FieldName;
                managerEligiblity.CreatedBy = userId;
                managerEligiblity.ModifiedBy = userId;
                managerEligiblity.Save();
            });
            return base.BuildJson(true, 200, "Data saved successfully", managerEligiblity);

        }

        public JsonResult GetManagerEligiblity()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            ManagerEligiblityList getRoles = new ManagerEligiblityList(companyId, DefaultFinancialYr.Id);
            return base.BuildJson(true, 200, "Data saved successfully", getRoles);
        }

        #endregion

        #region "Weekoffcomponentmatching"
        public JsonResult SaveWeekoffcmpmatch(List<jsonwkcmpmtching> dataValue)
        {
            var result = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);


            WeekoffComponentMatching wkcmpmtching = new WeekoffComponentMatching();
            wkcmpmtching.CompanyId = companyId;
            foreach (jsonwkcmpmtching trans in dataValue)
            {


                wkcmpmtching.LeaveCategoryId = trans.leavecategoryid;
                wkcmpmtching.Leavecomponent = trans.leavecomponent;
                wkcmpmtching.CompanyId = companyId;
                wkcmpmtching.SaveWeekoffComponentMatching();
                result = true;
            }
            if (result == false)
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
            else
            {
                return base.BuildJson(true, 200, "Weekoff Saved Successfully!", dataValue);
            }


        }
        #endregion

        #region "WeekoffSave"
        public class jsonweekoffdata
        {

            public string dayname { get; set; }
            public string weekoff { get; set; }
            public string weekType { get; set; }
            public string weekONE { get; set; }
            public string weekTWO { get; set; }
            public string weekTHREE { get; set; }
            public string weekFOUR { get; set; }
            public string weekFIVE { get; set; }
            public string weekoffdate { get; set; }
            public string MonthorDate { get; set; }



        }
        public class jsonweekoffComondats
        {

            public string CutoffFrom { get; set; }
            public string CutoffTo { get; set; }
            public string Month { get; set; }
            public string ComponentName { get; set; }
            public string ComponentValue { get; set; }
        }
        public JsonResult GetWeekoffsetting()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear leaveYear = new LeaveFinanceYear();
            leaveYear.CompanyId = companyId;
            //DataTable weekoffdt = leaveYear.getWeekoffdata();

            //if (weekoffdt.Rows.Count != 0)
            //{
            //    List<jsonweekoffdata> result = new List<jsonweekoffdata>();
            //    for (int i = 0; i < weekoffdt.Rows.Count; i++)
            //    {
            //        jsonweekoffdata weekday = new jsonweekoffdata();
            //        if (!string.IsNullOrEmpty(Convert.ToString(weekoffdt.Rows[i]["Weekoffday"])))
            //            weekday.dayname = Convert.ToString(weekoffdt.Rows[i]["Weekoffday"]);

            //        result.Add(weekday);
            //    }
            //    return base.BuildJson(true, 200, "Data deleted successfully", result);
            //}


            //else
            //{
            return base.BuildJson(false, 100, "There is some error while saving the data.", null);
            //}
        }
        public JsonResult GetDatesforweekoff(string[] weekdaynames, jsonweekoffComondats CommonDataValues)
        {
            string Fromdate = CommonDataValues.CutoffFrom;
            string Todate = CommonDataValues.CutoffTo;

            WeekoffCalculationList weekofflist = new WeekoffCalculationList(weekdaynames, Convert.ToDateTime(Fromdate), Convert.ToDateTime(Todate));
            List<jsonWeekoffdatesget> FinalDateGridList = new List<jsonWeekoffdatesget>();


            for (int i = 0; i <= weekofflist.Count - 1; i++)
            {
                jsonWeekoffdatesget DateGridList = new jsonWeekoffdatesget();
                DateGridList.dates = weekofflist[i].dates;
                DateGridList.datesname = weekofflist[i].datesname;
                DateGridList.Weekoff = "";
                FinalDateGridList.Add(DateGridList);
            }
            return base.BuildJson(true, 200, "success", FinalDateGridList);
        }

        public JsonResult GetWeekoffExistingcheck(string[] weekdaynames, jsonweekoffComondats CommonDataValues)
        {
            LeaveSettingsBO Weekoffsetting = new LeaveSettingsBO();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);
            if (levmasterlist.Count != 0)
            {
                string parameter = levmasterlist[0].Weekoffparameter;
                string Entryvalidity = levmasterlist[0].Weekoffentryvalid;
                if (Entryvalidity == "C")
                {
                    if (parameter == "companywise")
                    {
                        Weekoffsetting.FinYearStart = Convert.ToDateTime(CommonDataValues.CutoffFrom.ToString());
                        Weekoffsetting.FinYearEnd = Convert.ToDateTime(CommonDataValues.CutoffTo.ToString());
                        Weekoffsetting.weekofftype = "companywise";
                    }
                    else
                    {
                        Weekoffsetting.FinYearStart = Convert.ToDateTime(CommonDataValues.CutoffFrom.ToString());
                        Weekoffsetting.FinYearEnd = Convert.ToDateTime(CommonDataValues.CutoffTo.ToString());
                        Weekoffsetting.DynamicComponentName = CommonDataValues.ComponentName;
                        if (string.IsNullOrEmpty(CommonDataValues.ComponentValue) || CommonDataValues.ComponentValue.ToLower() == "select")
                            CommonDataValues.ComponentValue = "00000000-0000-0000-0000-000000000000";
                        Weekoffsetting.DynamicComponentValue = new Guid(CommonDataValues.ComponentValue);
                        Weekoffsetting.weekofftype = "Noncompanywise";
                    }
                }
                else if (Entryvalidity == "M")
                {
                    List<jsonWeekoffdatesget> jsonfullweekdatas = new List<jsonWeekoffdatesget>();
                    jsonWeekoffdatesget weekdatas = new jsonWeekoffdatesget();
                    var start = DefaultFinancialYr.StartMonth;
                    var end = DefaultFinancialYr.EndMonth;
                    string[] diff = Enumerable.Range(0, 13).Select(a => start.AddMonths(a))
                               .TakeWhile(a => a <= end)
                               .Select(a => String.Concat(a.ToString("MM") + ", " + a.Year)).ToArray();
                    for (int i = 0; i <= diff.Count() - 1; i++)
                    {
                        string[] values = diff[i].ToString().Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                        }

                        string FinalMonth = values[0].ToString();
                        int FinalYear = Convert.ToInt32(values[1].ToString());
                        if (FinalMonth == CommonDataValues.Month)
                        {
                            Weekoffsetting.FinYearStart = new DateTime(FinalYear, Convert.ToInt32(FinalMonth), 1);
                            Weekoffsetting.FinYearEnd = Convert.ToDateTime(Weekoffsetting.FinYearStart.AddMonths(1).AddDays(-1));
                            break;
                        }
                    }
                    if (parameter == "companywise")
                    {
                        Weekoffsetting.weekofftype = "companywise";
                    }
                    else
                    {
                        Weekoffsetting.DynamicComponentName = CommonDataValues.ComponentName;
                        if (string.IsNullOrEmpty(CommonDataValues.ComponentValue) || CommonDataValues.ComponentValue.ToLower() == "select")
                            CommonDataValues.ComponentValue = "00000000-0000-0000-0000-000000000000";
                        Weekoffsetting.DynamicComponentValue = new Guid(CommonDataValues.ComponentValue);
                        Weekoffsetting.weekofftype = "Noncompanywise";
                    }
                }
                else
                {
                    Weekoffsetting.FinYearStart = DefaultFinancialYr.StartMonth;
                    Weekoffsetting.FinYearEnd = DefaultFinancialYr.EndMonth;
                    if (parameter == "companywise")
                    {
                        Weekoffsetting.weekofftype = "companywise";
                    }
                    else
                    {
                        Weekoffsetting.DynamicComponentName = CommonDataValues.ComponentName;
                        if (string.IsNullOrEmpty(CommonDataValues.ComponentValue) || CommonDataValues.ComponentValue.ToLower() == "select")
                            CommonDataValues.ComponentValue = "00000000-0000-0000-0000-000000000000";
                        Weekoffsetting.DynamicComponentValue = new Guid(CommonDataValues.ComponentValue);
                        Weekoffsetting.weekofftype = "Noncompanywise";
                    }
                }
                Weekoffsetting.CompanyId = companyId;
                Weekoffsetting.FinyrId = DefaultFinancialYr.Id;

                DataTable weekoffexistval = Weekoffsetting.WeekoffSaveCheck();
                List<jsonWeekoffdatesget> FinalDateGridList = new List<jsonWeekoffdatesget>();
                List<jsonweekoffdata> FinalMonthGridList = new List<jsonweekoffdata>();
                if (weekoffexistval.Rows.Count != 0)
                {
                    if (parameter != "employeewise" && Entryvalidity != "C")//MONTH GRID VIEW
                    {
                        Weekoffsetting.WeekoffTempId = new Guid(weekoffexistval.Rows[0]["Id"].ToString());
                        DataTable MonthGridData = Weekoffsetting.GetGridviewdata();
                        for (int Monthloop = 0; Monthloop <= MonthGridData.Rows.Count - 1; Monthloop++)
                        {
                            jsonweekoffdata MonthGridList = new jsonweekoffdata();
                            MonthGridList.weekoff = MonthGridData.Rows[Monthloop]["DaysName"].ToString();
                            MonthGridList.weekType = MonthGridData.Rows[Monthloop]["Weekoffset"].ToString();
                            MonthGridList.weekONE = MonthGridData.Rows[Monthloop]["Week1"].ToString();
                            MonthGridList.weekTWO = MonthGridData.Rows[Monthloop]["Week2"].ToString();
                            MonthGridList.weekTHREE = MonthGridData.Rows[Monthloop]["Week3"].ToString();
                            MonthGridList.weekFOUR = MonthGridData.Rows[Monthloop]["Week4"].ToString();
                            MonthGridList.weekFIVE = MonthGridData.Rows[Monthloop]["Week5"].ToString();
                            MonthGridList.MonthorDate = "MG";
                            FinalMonthGridList.Add(MonthGridList);
                        }
                        FinalDataGrid(FinalMonthGridList);
                        return base.BuildJson(true, 100, "Data already Existing!!!", FinalMonthGridList);
                    }
                    else //DATE GRID VIEW
                    {
                        Weekoffsetting.WeekoffTempId = new Guid(weekoffexistval.Rows[0]["Id"].ToString());
                        DataTable DateGridData = Weekoffsetting.GetDateviewdata();
                        for (int dateloop = 0; dateloop <= DateGridData.Rows.Count - 1; dateloop++)
                        {
                            jsonWeekoffdatesget DateGridList = new jsonWeekoffdatesget();
                            DateGridList.dates = Convert.ToDateTime(DateGridData.Rows[dateloop]["Weekoffdate"].ToString());
                            DateGridList.datesname = DateGridData.Rows[dateloop]["Weekoffday"].ToString();
                            DateGridList.Weekoff = DateGridData.Rows[dateloop]["Weekoff"].ToString();
                            DateGridList.MonthorDate = "DG";
                            FinalDateGridList.Add(DateGridList);
                        }

                        return base.BuildJson(true, 100, "Data already Existing!!!", FinalDateGridList);
                    }
                }
                else
                {
                    return base.BuildJson(true, 100, "Data Not Exist", FinalDateGridList);
                }

            }
            else
            {
                return base.BuildJson(true, 100, "There is some error while saving the data.", "");
            }
        }
        public JsonResult SaveWeekoffsetting(List<jsonweekoffdata> dataValue, jsonweekoffComondats CommonDatas)
        {
            Guid MasterId = Guid.NewGuid();
            List<jsonWeekoffdatesget> FinalSaveList = new List<jsonWeekoffdatesget>();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);

            DateTime Fromdate = DateTime.Now;
            DateTime Todate = DateTime.Now;
            if (levmasterlist[0].Weekoffentryvalid == "C")
            {
                Fromdate = Convert.ToDateTime(CommonDatas.CutoffFrom);
                Todate = Convert.ToDateTime(CommonDatas.CutoffTo);
            }
            else if (levmasterlist[0].Weekoffentryvalid == "M")
            {
                List<jsonWeekoffdatesget> jsonfullweekdatas = new List<jsonWeekoffdatesget>();
                jsonWeekoffdatesget weekdatas = new jsonWeekoffdatesget();
                var start = DefaultFinancialYr.StartMonth;
                var end = DefaultFinancialYr.EndMonth;
                string[] diff = Enumerable.Range(0, 13).Select(a => start.AddMonths(a))
                           .TakeWhile(a => a <= end)
                           .Select(a => String.Concat(a.ToString("MM") + ", " + a.Year)).ToArray();
                for (int i = 0; i <= diff.Count() - 1; i++)
                {
                    string[] values = diff[i].ToString().Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                    }

                    string FinalMonth = values[0].ToString();
                    int FinalYear = Convert.ToInt32(values[1].ToString());
                    if (FinalMonth == CommonDatas.Month)
                    {
                        Fromdate = new DateTime(FinalYear, Convert.ToInt32(FinalMonth), 1);
                        Todate = Convert.ToDateTime(Fromdate.AddMonths(1).AddDays(-1));
                    }
                }

            }
            else
            {
                Fromdate = DefaultFinancialYr.StartMonth;
                Todate = DefaultFinancialYr.EndMonth;

            }

            if (levmasterlist[0].Weekoffentryvalid != "C" && levmasterlist[0].Weekoffparameter != "employeewise") //MONTH WISE GRID DATA SAVING LOOP
            {

                string[] dayslist = new string[dataValue.Count];
                for (int wKoffdays = 0; wKoffdays <= dataValue.Count - 1; wKoffdays++)
                {
                    string name = dataValue[wKoffdays].weekoff.ToString();
                    dayslist[wKoffdays] = name;
                }




                WeekoffCalculationList weekofflist = new WeekoffCalculationList(dayslist, Fromdate, Todate);
                bool contflag = false;
                for (int Variablecheck = 0; Variablecheck <= dataValue.Count - 1; Variablecheck++)
                {
                    List<jsonWeekoffdatesget> Listfoundeddates = new List<jsonWeekoffdatesget>();
                    contflag = false;
                    if (dataValue[Variablecheck].weekType == "Varible")//Finding the Variable Days
                    {

                        for (int weekofflistVariablecheck = 0; weekofflistVariablecheck <= weekofflist.Count - 1; weekofflistVariablecheck++)
                        {

                            if (dataValue[Variablecheck].weekoff.ToUpper().Trim() == weekofflist[weekofflistVariablecheck].datesname.ToUpper().Trim()) //getting the variable dates
                            {
                                jsonWeekoffdatesget foundeddates = new jsonWeekoffdatesget();
                                foundeddates.dates = weekofflist[weekofflistVariablecheck].dates;
                                foundeddates.datesname = weekofflist[weekofflistVariablecheck].datesname;
                                Listfoundeddates.Add(foundeddates);
                                contflag = true;
                            }

                        }
                        if (contflag == true)
                        {
                            List<jsonWeekoffdatesget> SortedfoundeddateList = new List<jsonWeekoffdatesget>();
                            SortedfoundeddateList = Listfoundeddates.OrderBy(o => o.dates).ToList();
                            List<jsonWeekoffdatesget> FinalList = new List<jsonWeekoffdatesget>();
                            for (int orderingvariabledates = 0; orderingvariabledates <= SortedfoundeddateList.Count - 1; orderingvariabledates++)
                            {
                                switch (orderingvariabledates)
                                {
                                    case 0:
                                        jsonWeekoffdatesget weekone = new jsonWeekoffdatesget();
                                        if (dataValue[Variablecheck].weekONE != "Working")
                                        {
                                            weekone.Id = Guid.NewGuid();
                                            weekone.Weekoffid = MasterId;
                                            weekone.CreatedBy = Session["userid"].ToString();
                                            weekone.dates = SortedfoundeddateList[orderingvariabledates].dates;
                                            weekone.datesname = SortedfoundeddateList[orderingvariabledates].datesname;
                                            if (dataValue[Variablecheck].weekONE == "Full")
                                            {
                                                weekone.Halfday = false;
                                            }
                                            else
                                            {
                                                weekone.Halfday = true;
                                            }
                                            FinalSaveList.Add(weekone);
                                        }
                                        break;
                                    case 1:
                                        jsonWeekoffdatesget weektwo = new jsonWeekoffdatesget();
                                        if (dataValue[Variablecheck].weekTWO != "Working")
                                        {
                                            weektwo.Id = Guid.NewGuid();
                                            weektwo.Weekoffid = MasterId;
                                            weektwo.CreatedBy = Session["userid"].ToString();
                                            weektwo.dates = SortedfoundeddateList[orderingvariabledates].dates;
                                            weektwo.datesname = SortedfoundeddateList[orderingvariabledates].datesname;
                                            if (dataValue[Variablecheck].weekTWO == "Full")
                                            {
                                                weektwo.Halfday = false;
                                            }
                                            else
                                            {
                                                weektwo.Halfday = true;
                                            }
                                            FinalSaveList.Add(weektwo);
                                        }
                                        break;
                                    case 2:
                                        jsonWeekoffdatesget weekthree = new jsonWeekoffdatesget();
                                        if (dataValue[Variablecheck].weekTHREE != "Working")
                                        {
                                            weekthree.Id = Guid.NewGuid();
                                            weekthree.Weekoffid = MasterId;
                                            weekthree.CreatedBy = Session["userid"].ToString();
                                            weekthree.dates = SortedfoundeddateList[orderingvariabledates].dates;
                                            weekthree.datesname = SortedfoundeddateList[orderingvariabledates].datesname;
                                            if (dataValue[Variablecheck].weekTHREE == "Full")
                                            {
                                                weekthree.Halfday = false;
                                            }
                                            else
                                            {
                                                weekthree.Halfday = true;
                                            }
                                            FinalSaveList.Add(weekthree);
                                        }
                                        break;
                                    case 3:
                                        jsonWeekoffdatesget weekfour = new jsonWeekoffdatesget();
                                        if (dataValue[Variablecheck].weekFOUR != "Working")
                                        {
                                            weekfour.Id = Guid.NewGuid();
                                            weekfour.Weekoffid = MasterId;
                                            weekfour.CreatedBy = Session["userid"].ToString();
                                            weekfour.dates = SortedfoundeddateList[orderingvariabledates].dates;
                                            weekfour.datesname = SortedfoundeddateList[orderingvariabledates].datesname;
                                            if (dataValue[Variablecheck].weekFOUR == "Full")
                                            {
                                                weekfour.Halfday = false;
                                            }
                                            else
                                            {
                                                weekfour.Halfday = true;
                                            }
                                            FinalSaveList.Add(weekfour);
                                        }
                                        break;
                                    case 4:
                                        jsonWeekoffdatesget weekfive = new jsonWeekoffdatesget();
                                        if (dataValue[Variablecheck].weekFIVE != "Working")
                                        {
                                            weekfive.Id = Guid.NewGuid();
                                            weekfive.Weekoffid = MasterId;
                                            weekfive.CreatedBy = Session["userid"].ToString();
                                            weekfive.dates = SortedfoundeddateList[orderingvariabledates].dates;
                                            weekfive.datesname = SortedfoundeddateList[orderingvariabledates].datesname;
                                            if (dataValue[Variablecheck].weekFIVE == "Full")
                                            {
                                                weekfive.Halfday = false;
                                            }
                                            else
                                            {
                                                weekfive.Halfday = true;
                                            }
                                            FinalSaveList.Add(weekfive);
                                        }
                                        break;
                                    default:

                                        break;
                                }

                            }
                        }
                    }
                    else
                    {
                        contflag = false;
                        for (int weekofflistNONVariablecheck = 0; weekofflistNONVariablecheck <= weekofflist.Count - 1; weekofflistNONVariablecheck++)
                        {


                            if (dataValue[Variablecheck].weekoff.ToUpper().Trim() == weekofflist[weekofflistNONVariablecheck].datesname.ToUpper().Trim()) //getting the variable dates
                            {
                                jsonWeekoffdatesget NONVarfoundeddates = new jsonWeekoffdatesget();
                                NONVarfoundeddates.dates = weekofflist[weekofflistNONVariablecheck].dates;
                                NONVarfoundeddates.datesname = weekofflist[weekofflistNONVariablecheck].datesname;
                                Listfoundeddates.Add(NONVarfoundeddates);
                                contflag = true;
                            }
                        }
                        if (contflag == true)
                        {
                            List<jsonWeekoffdatesget> SortedfoundeddateList1 = new List<jsonWeekoffdatesget>();
                            SortedfoundeddateList1 = Listfoundeddates.OrderBy(o => o.dates).ToList();
                            for (int orderingvariabledates1 = 0; orderingvariabledates1 <= SortedfoundeddateList1.Count - 1; orderingvariabledates1++)
                            {
                                jsonWeekoffdatesget week = new jsonWeekoffdatesget();
                                if (dataValue[Variablecheck].weekType != "Working")
                                {
                                    week.Id = Guid.NewGuid();
                                    week.Weekoffid = MasterId;
                                    week.CreatedBy = Session["userid"].ToString();
                                    week.dates = SortedfoundeddateList1[orderingvariabledates1].dates;
                                    week.datesname = SortedfoundeddateList1[orderingvariabledates1].datesname;
                                    if (dataValue[Variablecheck].weekType == "Full")
                                    {
                                        week.Halfday = false;
                                    }
                                    else
                                    {
                                        week.Halfday = true;
                                    }
                                    FinalSaveList.Add(week);
                                }

                            }
                        }

                    }

                }
                LeaveSettingsBO Weekoffsetting = new LeaveSettingsBO();
                Weekoffsetting.CompanyId = companyId;
                Weekoffsetting.FinyrId = DefaultFinancialYr.Id;
                Weekoffsetting.Weekoffentryvalid = levmasterlist[0].Weekoffentryvalid;
                Weekoffsetting.FinYearStart = Fromdate;
                Weekoffsetting.FinYearEnd = Todate;
                Weekoffsetting.Createdby = Session["userid"].ToString();
                Weekoffsetting.DynamicComponentName = levmasterlist[0].Weekoffparameter;
                Weekoffsetting.Id = MasterId;
                if (levmasterlist[0].Weekoffparameter == "companywise")
                {
                    Weekoffsetting.DynamicComponentValue = Guid.Empty;
                }
                else
                {
                    Weekoffsetting.DynamicComponentValue = new Guid(CommonDatas.ComponentValue);
                }

                if (Weekoffsetting.SaveWeekoffMasterdata())
                {
                    for (int gridsaveCnt = 0; gridsaveCnt <= dataValue.Count - 1; gridsaveCnt++)
                    {
                        Weekoffsetting.DaysName = dataValue[gridsaveCnt].weekoff;
                        Weekoffsetting.Weekoffset = dataValue[gridsaveCnt].weekType;
                        Weekoffsetting.Week1 = dataValue[gridsaveCnt].weekONE;
                        Weekoffsetting.Week2 = dataValue[gridsaveCnt].weekTWO;
                        Weekoffsetting.Week3 = dataValue[gridsaveCnt].weekTHREE;
                        Weekoffsetting.Week4 = dataValue[gridsaveCnt].weekFOUR;
                        Weekoffsetting.Week5 = dataValue[gridsaveCnt].weekFIVE;
                        Weekoffsetting.Createdby = Session["userid"].ToString();
                        Weekoffsetting.SaveWeekoffGridSettings();
                    }

                    StringWriter stringWriter = new StringWriter();
                    XmlDocument xmlDoc = new XmlDocument();

                    XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<jsonWeekoffdatesget>));

                    serializer.Serialize(xmlWriter, FinalSaveList);

                    string xmlResult = stringWriter.ToString();

                    if (Weekoffsetting.SaveWeekoffDatewise(xmlResult))
                    {
                        return base.BuildJson(true, 200, "Weekoff Saved Successfully!", dataValue);
                    }
                    else
                    {
                        return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                    }
                }

                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                }
            }

            else //DATE WISE GRID DATA SAVING LOOP
            {

                LeaveSettingsBO Weekoffsetting = new LeaveSettingsBO();
                Weekoffsetting.CompanyId = companyId;
                Weekoffsetting.FinyrId = DefaultFinancialYr.Id;
                Weekoffsetting.Weekoffentryvalid = levmasterlist[0].Weekoffentryvalid;
                Weekoffsetting.FinYearStart = Fromdate;
                Weekoffsetting.FinYearEnd = Todate;
                Weekoffsetting.Createdby = Session["userid"].ToString();
                Weekoffsetting.DynamicComponentName = levmasterlist[0].Weekoffparameter;
                Weekoffsetting.Id = MasterId;
                if (levmasterlist[0].Weekoffparameter == "companywise")
                {
                    Weekoffsetting.DynamicComponentValue = Guid.Empty;
                }
                else
                {
                    Weekoffsetting.DynamicComponentValue = new Guid(CommonDatas.ComponentValue);
                }

                if (Weekoffsetting.SaveWeekoffMasterdata())
                {
                    List<jsonWeekoffdatesget> TempFinalSaveList = new List<jsonWeekoffdatesget>();

                    for (int Temploop = 0; Temploop <= dataValue.Count - 1; Temploop++)
                    {
                        jsonWeekoffdatesget weekCheck = new jsonWeekoffdatesget();
                        weekCheck.Id = Guid.NewGuid();
                        weekCheck.Weekoffid = MasterId;
                        weekCheck.CreatedBy = Session["userid"].ToString();
                        weekCheck.dates = Convert.ToDateTime(dataValue[Temploop].weekoffdate);
                        weekCheck.datesname = dataValue[Temploop].dayname;
                        weekCheck.Weekoff = dataValue[Temploop].weekoff;
                        TempFinalSaveList.Add(weekCheck);
                    }
                    StringWriter TempstringWriter = new StringWriter();
                    XmlDocument TempxmlDoc = new XmlDocument();
                    XmlTextWriter TempxmlWriter = new XmlTextWriter(TempstringWriter);
                    XmlSerializer Tempserializer = new XmlSerializer(typeof(List<jsonWeekoffdatesget>));
                    Tempserializer.Serialize(TempxmlWriter, TempFinalSaveList);
                    string TempxmlResult = TempstringWriter.ToString();
                    if (Weekoffsetting.SaveGridWeekoffDate(TempxmlResult))
                    {
                        for (int loop = 0; loop <= dataValue.Count - 1; loop++)
                        {
                            if (dataValue[loop].weekoff != "Working")
                            {
                                jsonWeekoffdatesget week = new jsonWeekoffdatesget();
                                week.Id = Guid.NewGuid();
                                week.Weekoffid = MasterId;
                                week.CreatedBy = Session["userid"].ToString();
                                week.dates = Convert.ToDateTime(dataValue[loop].weekoffdate);
                                week.datesname = dataValue[loop].dayname;
                                if (dataValue[loop].weekoff == "Full")
                                {
                                    week.Halfday = false;
                                }
                                else
                                {
                                    week.Halfday = true;
                                }
                                FinalSaveList.Add(week);
                            }
                        }

                        StringWriter stringWriter = new StringWriter();
                        XmlDocument xmlDoc = new XmlDocument();

                        XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<jsonWeekoffdatesget>));

                        serializer.Serialize(xmlWriter, FinalSaveList);

                        string xmlResult = stringWriter.ToString();

                        if (Weekoffsetting.SaveWeekoffDatewise(xmlResult))
                        {
                            return base.BuildJson(true, 200, "Weekoff Saved Successfully!", dataValue);
                        }
                        else
                        {
                            return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                        }
                    }
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                }
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }



        }


        public List<jsonweekoffdata> FinalDataGrid(List<jsonweekoffdata> getvaluesFromDB)
        {
            List<jsonweekoffdata> FinalData = new List<jsonweekoffdata>();
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            for (int i = 0; i < days.Length; i++)
            {
                if (getvaluesFromDB.Where(x => x.weekoff.ToLower() == days[i].ToLower()).FirstOrDefault() == null)
                {
                    jsonweekoffdata MonthGridList = new jsonweekoffdata();
                    MonthGridList.weekoff = days[i].ToString();
                    MonthGridList.weekType = "Working";
                    MonthGridList.weekONE = "Working";
                    MonthGridList.weekTWO = "Working";
                    MonthGridList.weekTHREE = "Working";
                    MonthGridList.weekFOUR = "Working";
                    MonthGridList.weekFIVE = "Working";
                    MonthGridList.MonthorDate = "MG";
                    getvaluesFromDB.Add(MonthGridList);
                }
            }

            return getvaluesFromDB;
        }



        #endregion

        #region "LeaveReport"
        public class jsonLeavereport
        {

            public string LeaveDay { get; set; }
            public DateTime LeaveDate { get; set; }
            public string LeaveType { get; set; }
            public string Duration { get; set; }
            //public Guid leaveid { get; set; }



        }
        public class jsonDownloadLeavereport
        {

            public string FromDate { get; set; }
            public string Fromday { get; set; }
            public string ToDate { get; set; }
            public string ToDay { get; set; }
            public string NoOfDays { get; set; }

            public string LeaveType { get; set; }
            public string Applied_Reason { get; set; }

            public string Rejected_Reason { get; set; }

            public string cancelled_Reason { get; set; }

            public jsonDownloadLeavereport tojson(LeaveRequest lr)
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
                return new jsonDownloadLeavereport
                {
                    FromDate = lr.FromDate.ToString("dd-M-yyyy"),
                    Fromday = fromdaytype,
                    ToDate = lr.EndDate.ToString("dd-M-yyyy"),
                    ToDay = EndDayType,
                    NoOfDays = lr.NoOfDays,
                    LeaveType = lr.LeaveTypeName,
                    Applied_Reason = lr.Reason,
                    Rejected_Reason = lr.Rejectreason,
                    cancelled_Reason = lr.Rejectreason

                };
            }




        }
        //public JsonResult GetLeaveReport(DateTime FromDate, DateTime ToDate, Guid EmployeeId, int LeaveStat)
        //{




        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
        //    Guid employeeid = new Guid(Session["EmployeeId"].ToString()); 
        //         DataTable dt = DefaultFinancialYr.getemployeeleavereport(FromDate, ToDate, employeeid);


        //    List<jsonLeavereport> result = new List<jsonLeavereport>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (dt.Rows[i]["IsDeleted"].ToString() != "True")
        //        {
        //            jsonLeavereport leavereport = new jsonLeavereport();

        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevDate"])))
        //                leavereport.LeaveDate = Convert.ToDateTime(dt.Rows[i]["LevDate"]).Date;


        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevDayName"])))
        //                leavereport.LeaveDay = Convert.ToString(dt.Rows[i]["LevDayName"]);

        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LeaveType"])))
        //            {
        //                leavereport.LeaveType = Convert.ToString(dt.Rows[i]["LeaveType"]);
        //            }
        //            else
        //            {
        //                leavereport.LeaveType = "Loss of Pay";
        //            }


        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["daytype"])))
        //                leavereport.Duration = Convert.ToString(dt.Rows[i]["daytype"]);
        //            result.Add(leavereport);
        //        }
        //    }


        //    return base.BuildJson(true, 200, "Data deleted successfully", result);
        //}


        public JsonResult GetManagerReport(DateTime FromDate, DateTime ToDate, Guid Employeecodeid, int Leaverptstatus)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            Guid Managerid = new Guid(Session["EmployeeId"].ToString());
            DataTable dt = DefaultFinancialYr.getManageremployeereport(FromDate, ToDate, Managerid, Employeecodeid, Leaverptstatus);


            List<jsonLeavereport> result = new List<jsonLeavereport>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsDeleted"].ToString() != "True")
                {
                    jsonLeavereport leavereport = new jsonLeavereport();

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevDate"])))
                        leavereport.LeaveDate = Convert.ToDateTime(dt.Rows[i]["LevDate"]).Date;


                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevDayName"])))
                        leavereport.LeaveDay = Convert.ToString(dt.Rows[i]["LevDayName"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LeaveType"])))
                    {
                        leavereport.LeaveType = Convert.ToString(dt.Rows[i]["LeaveType"]);
                    }
                    else
                    {
                        leavereport.LeaveType = "LOSS OF PAY DAYS";
                    }


                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["daytype"])))
                        leavereport.Duration = Convert.ToString(dt.Rows[i]["daytype"]);
                    result.Add(leavereport);
                }
            }


            return base.BuildJson(true, 200, "Data deleted successfully", result);
        }


        public JsonResult SaveApprovedLeaveCancelRequest(Guid LeaveRequestId, string CancelReason)
        {




            return base.BuildJson(true, 200, "Leave Report Exported successfully", "");
        }


        public JsonResult DownloadLeaveReport(Guid EmployeeId, int status)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid empid = new Guid(Session["EmployeeId"].ToString());
            List<LeaveRequest> LevFinalList = new List<LeaveRequest>();
            LeaveTypeList leavetype = new LeaveTypeList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if ((empid == Guid.Empty && EmployeeId == Guid.Empty) || (empid != Guid.Empty && EmployeeId != Guid.Empty))
            {


                //LeaveRequestList leav = new LeaveRequestList(EmployeeId, companyId);
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

                List<jsonDownloadLeavereport> lev = new List<jsonDownloadLeavereport>();
                jsonDownloadLeavereport lv = new jsonDownloadLeavereport();

                leav.ForEach(n =>
                {
                    if (n.Status == status)
                    {
                        lev.Add(lv.tojson(n));
                    }
                });


                DataTable dt1 = ToDataTable(lev);
                if (status == 0 || status == 1) { dt1.Columns.Remove("Rejected_Reason"); dt1.Columns.Remove("cancelled_Reason"); }
                if (status == 2) { dt1.Columns.Remove("Applied_Reason"); dt1.Columns.Remove("cancelled_Reason"); }
                if (status == 3) { dt1.Columns.Remove("Applied_Reason"); dt1.Columns.Remove("Rejected_Reason"); }
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
                    string[] arr = { "Pending", "Approved", "Rejected", "Cancelled", "Approved_Leave_Cancel_PendingList", "Approved_Leave_Cancel_AcceptedList", "Approved_Leave_Cancel_RejectedList" };
                    string ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + "Leave_" + arr[status] + "_Report.xls";
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
            return base.BuildJson(false, 100, "No Data Available ", "");
        }

        public JsonResult ExportLeaveReport(DateTime FromDate, DateTime ToDate)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            Guid employeeid = new Guid(Session["EmployeeId"].ToString());
            DataTable dttt = DefaultFinancialYr.getemployeeleavereport(FromDate, ToDate, employeeid);


            List<jsonLeavereport> result2 = new List<jsonLeavereport>();
            for (int i = 0; i < dttt.Rows.Count; i++)
            {
                if (dttt.Rows[i]["IsDeleted"].ToString() != "True")
                {
                    jsonLeavereport leavereport1 = new jsonLeavereport();

                    if (!string.IsNullOrEmpty(Convert.ToString(dttt.Rows[i]["LevDate"])))
                        leavereport1.LeaveDate = Convert.ToDateTime(dttt.Rows[i]["LevDate"]).Date;
                    if (!string.IsNullOrEmpty(Convert.ToString(dttt.Rows[i]["LevDayName"])))
                        leavereport1.LeaveDay = Convert.ToString(dttt.Rows[i]["LevDayName"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dttt.Rows[i]["LeaveType"])))
                    {
                        leavereport1.LeaveType = Convert.ToString(dttt.Rows[i]["LeaveType"]);
                    }
                    else
                    {
                        leavereport1.LeaveType = "LOSS OF PAY DAYS";
                    }


                    if (!string.IsNullOrEmpty(Convert.ToString(dttt.Rows[i]["daytype"])))
                        leavereport1.Duration = Convert.ToString(dttt.Rows[i]["daytype"]);
                    result2.Add(leavereport1);
                }
            }
            DataTable dt1 = ToDataTable(result2);

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


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }



        #endregion


        #region "calendarleavedateselect"

        public JsonResult GetCalendardatedata(string LeaveDate, Guid LeavetypeGUid)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            DefaultLOPid DEFLOPID = new DefaultLOPid(companyId);
            DefaultLOPid DEFONDUTYID = new DefaultLOPid(companyId, 1);
            DataTable dt = DefaultFinancialYr.CalendarDate(LeaveDate, LeavetypeGUid, DefaultFinancialYr.Id, DEFLOPID.LOPId, DEFONDUTYID.ONDUTYId);
            //DataTable dt = DefaultFinancialYr.CalendarDate(LeaveDate, LeavetypeGUid, DefaultFinancialYr.Id);

            List<jsonFullcalendar> result = new List<jsonFullcalendar>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonFullcalendar calenderdate = new jsonFullcalendar();

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeId"])))
                    calenderdate.EmpId = new Guid(Convert.ToString(dt.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevType"])))
                    calenderdate.LeavetypeGUid = new Guid(Convert.ToString(dt.Rows[i]["LevType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Name"])))
                    calenderdate.Name = Convert.ToString(dt.Rows[i]["Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalLeave"])))
                    calenderdate.TotalDays = Convert.ToDouble(dt.Rows[i]["TotalLeave"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Leaveused"])))
                    calenderdate.UsedDays = Convert.ToDouble(dt.Rows[i]["Leaveused"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeCode"])))
                    calenderdate.Empcode = Convert.ToString(dt.Rows[i]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LeaveType"])))
                    calenderdate.LeaveTitle = Convert.ToString(dt.Rows[i]["LeaveType"]);
                //ViewBag.LeaveTitle = calenderdate.LeaveTitle;

                calenderdate.AvailableDays = calenderdate.TotalDays - calenderdate.UsedDays;
                result.Add(calenderdate);
            }

            return base.BuildJson(true, 200, "Data deleted successfully", result);




        }




        public JsonResult GetCalendardatedataLOP(string LeaveDate, Guid LeavetypeGUid)
        {


            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            DefaultLOPid DEFLOPID = new DefaultLOPid(companyId);
            DefaultLOPid DEFONDUTYID = new DefaultLOPid(companyId, 1);
            DataTable dt = DefaultFinancialYr.CalendarDate(LeaveDate, LeavetypeGUid, DefaultFinancialYr.Id, DEFLOPID.LOPId, DEFONDUTYID.ONDUTYId);

            List<jsonFullcalendar> result = new List<jsonFullcalendar>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonFullcalendar calenderdate = new jsonFullcalendar();

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeId"])))
                    calenderdate.EmpId = new Guid(Convert.ToString(dt.Rows[i]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LevType"])))
                    calenderdate.LeavetypeGUid = new Guid(Convert.ToString(dt.Rows[i]["LevType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Name"])))
                    calenderdate.Name = Convert.ToString(dt.Rows[i]["Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeCode"])))
                    calenderdate.Empcode = Convert.ToString(dt.Rows[i]["EmployeeCode"]);
                if (LeavetypeGUid == DEFLOPID.LOPId)
                {
                    calenderdate.LeaveTitle = "LOSS OF PAY DAYS";
                }
                if (LeavetypeGUid == DEFONDUTYID.ONDUTYId)
                {
                    calenderdate.LeaveTitle = "ONDUTY";
                }
                result.Add(calenderdate);
            }

            return base.BuildJson(true, 200, "Data deleted successfully", result);




        }



        public JsonResult SaveMailConfig(jsonMailConfig dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            MailConfig mailConfig = new MailConfig();
            mailConfig.AuthenEmail = dataValue.AuthenEmail;
            mailConfig.IPAddress = dataValue.IPAddress;
            mailConfig.PortNo = dataValue.PortNo;
            mailConfig.FromEmail = dataValue.FromEmail;
            mailConfig.MailPassword = dataValue.MailPassword;
            mailConfig.CCMail = dataValue.CCMail;
            mailConfig.mailapproval = dataValue.mailapproval;
            mailConfig.AuthenSMTPUser = dataValue.AuthenSMTPUser;
            mailConfig.AuthenSMTPPwd = dataValue.AuthenSMTPPwd;
            mailConfig.EnableSSL = dataValue.EnableSSL;
            mailConfig.CreatedBy = userId;
            mailConfig.CreatedOn = DateTime.Now;
            mailConfig.CompanyId = companyId;
            mailConfig.Id = dataValue.Id;
            isSaved = mailConfig.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult GetLOPandONDUTYID()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            DefaultLOPid DEFLOPID = new DefaultLOPid(companyId);
            DefaultLOPid DEFONDUTYID = new DefaultLOPid(companyId, 1);

            DEFLOPID.ONDUTYId = DEFONDUTYID.ONDUTYId;

            return base.BuildJson(true, 200, "success", DEFLOPID);
        }

        public JsonResult GetMailConfig()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            MailConfig mailConfig = new MailConfig(companyId);


            return base.BuildJson(true, 200, "success", mailConfig);
        }

        public JsonResult SendTestMail(string toMail)
        {
            ErrorLog.LogTestWrite("SendTestMail");
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            bool status = false;
            string subject = "Test Mail";
            string message = "Test Mail Message from MINDSPAY";


            MailConfig mailConfig = new MailConfig(companyId);


            PayRoleMail payrolemail = new PayRoleMail(toMail, subject, message);
            //payrolemail.author =1;
            ////payrolemail.password = password;
            payrolemail.TOmail = toMail;
            //int portnumber = Convert.ToInt32(portNo);
            //payrolemail.ssl = ssl;
            status = payrolemail.SendTestMail(mailConfig.IPAddress, mailConfig.PortNo, mailConfig.FromEmail, mailConfig.MailPassword, mailConfig.EnableSSL);
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
            mailConfig.Savemailhistory(companyId, empid, mailConfig.FromEmail, toMail, null, null, message, subject, mailstat);

            if (status)
            {
                return base.BuildJson(true, 200, "Test Mail sent Successfully", status);
            }
            else
            {
                return base.BuildJson(false, 200, "Mail not Sent", status);
            }
        }


        #endregion

        #region "fullcalendarRender"

        //public ActionResult GetLeaveSchedule(int LeaveCalender, string txtFromDateid, string txtToDateid)
        public JsonResult GetLeaveSchedule()
        {

            DataTable dtCalendar = new DataTable();
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //if(LeaveCalender != 0)
            //{
            //    DefaultFinancialYr.fromdate = Convert.ToDateTime(txtFromDateid);
            //    DefaultFinancialYr.Todate = Convert.ToDateTime(txtToDateid);
            //    DefaultFinancialYr.CompanyId = companyId;
            //    dtCalendar = DefaultFinancialYr.GetFullCalendarwithfilter();
            //}
            //  else{
            //    dtCalendar = DefaultFinancialYr.GetFullCalendar(DefaultFinancialYr.Id);
            //} 
            dtCalendar = DefaultFinancialYr.GetFullCalendar(DefaultFinancialYr.Id);
            DataTable dtcopy = dtCalendar.Copy();
            HolidaysList holidaylist = new HolidaysList(Guid.Empty, DefaultFinancialYr.Id);

            //foreach (DataRow dr in dtCalendar.Rows)
            //{
            //    //var holiday = holidaylist.Where(h => h.Holidaydate == Convert.ToDateTime(dr["LevDate"].ToString()));

            //    for(int i = 0; i < holidaylist.Count; i++)
            //        {

            //        if(holidaylist[i].Holidaydate != Convert.ToDateTime(dr["LevDate"].ToString()))
            //            {
            //            DataRow newRow = dtcopy.NewRow();
            //            newRow["LevType"] = dr["LevType"];
            //            newRow["FinanceYearId"] = dr["FinanceYearId"];
            //            newRow["LevDate"] = dr["LevDate"];
            //            newRow["leaveName"] = dr["leaveName"];
            //            newRow["CntLev"] = dr["CntLev"];


            //            dtcopy.Rows.Add(newRow);
            //        }


            //    }
            //    //if (ReferenceEquals(holiday, null))
            //    //    {
            //    //    DataRow newRow = dtcopy.NewRow();
            //    //    newRow["LevType"] = dr["LevType"];
            //    //    newRow["FinanceYearId"] = dr["FinanceYearId"];
            //    //    newRow["LevDate"] = dr["LevDate"];
            //    //    newRow["leaveName"] = dr["leaveName"];
            //    //    newRow["CntLev"] = dr["CntLev"];


            //    //    dtcopy.Rows.Add(newRow); 
            //    //}

            //}




            //var result1 = from r in holidaylist.AsEnumerable()
            //             join sg in dt
            //                  on r.Holidaydate equals sg.Id
            //             select sg;

            //var tableIds = dt.Rows.Cast<DataRow>().Select(row => row["LevDate"].ToString());

            //var listIds = holidaylist;

            //return listIds.Except(tableIds).ToList();
            //var holidaysubtract = holidaylist.Where (a=>a.Holidaydate)  .Except(Holidaydates).ToList();



            List<jsonFullcalendar> result = new List<jsonFullcalendar>();
            for (int i = 0; i < dtcopy.Rows.Count; i++)
            {
                if (dtcopy.Rows[i]["IsDeleted"].ToString() != "True")
                {
                    jsonFullcalendar jres = new jsonFullcalendar();
                    jres.Userrole = "LEAVE";
                    if (!string.IsNullOrEmpty(Convert.ToString(dtcopy.Rows[i]["LevType"])))
                        jres.LeavetypeGUid = new Guid(Convert.ToString(dtcopy.Rows[i]["LevType"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtcopy.Rows[i]["FinanceYearId"])))
                        jres.FinyrId = new Guid(Convert.ToString(dtcopy.Rows[i]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtcopy.Rows[i]["LevDate"])))
                        jres.LeaveDate = Convert.ToDateTime(dtcopy.Rows[i]["LevDate"]).ToString("yyyy-MM-dd");
                    //if (jres.LeavetypeGUid !=new Guid("199F5DB2-14B7-46D3-A0E4-715D56682277"))
                    if (jres.LeavetypeGUid != new Guid(lossofpayid.LOPId.ToString()))   //199F5DB2-14B7-46D3-A0E4-715D56682277

                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dtcopy.Rows[i]["leaveName"])))
                            jres.title = Convert.ToString(dtcopy.Rows[i]["leaveName"]) + " : " + Convert.ToString(dtcopy.Rows[i]["CntLev"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(dtcopy.Rows[i]["Colour"])))
                            jres.className = Convert.ToString(dtcopy.Rows[i]["Colour"]);
                    }
                    else
                    {
                        jres.title = "LOP" + " : " + Convert.ToString(dtcopy.Rows[i]["CntLev"]);
                        jres.className = "Black";
                    }



                    result.Add(jres);
                }


            }

            holidaylist.ForEach(d =>
            {
                jsonFullcalendar jres = new jsonFullcalendar();
                jres.LeavetypeGUid = new Guid(Convert.ToString(d.Id));
                jres.FinyrId = new Guid(Convert.ToString(d.financeyearId));
                jres.LeaveDate = Convert.ToDateTime(d.Holidaydate).ToString("yyyy-MM-dd");
                jres.title = Convert.ToString(d.HolidayReason);
                jres.Userrole = "HOLIDAY";
                jres.className = "Red";
                result.Add(jres);

            });


            return base.BuildJson(true, 200, "Data deleted successfully", result);
            //return View("~/Views/Company/LeaveCalendar.cshtml");
        }
        public class jsonFullcalendar
        {


            public Guid LeavetypeGUid { get; set; }
            public string LeaveTitle { get; set; }
            public Guid FinyrId { get; set; }
            public Guid EmpId { get; set; }
            public string Empcode { get; set; }
            public string Name { get; set; }
            public double UsedDays { get; set; }
            public double Debitdays { get; set; }
            public double TotalDays { get; set; }
            public double AvailableDays { get; set; }
            public double CreditDays { get; set; }
            public string LeaveDate { get; set; }
            public string Userrole { get; set; }
            public string title { get; set; }
            public string className { get; set; }

            //public static jsonFullcalendar toJson(LeaveFinanceYear txFinance)
            //{
            //    return new jsonFullcalendar()
            //    {
            //        EmpGUid = txFinance.EmpGUid,

            //        LeavetypeGUid = txFinance.LeavetypeGUid,
            //        LeaveDate = txFinance.LeaveDate,
            //        Leavename = txFinance.Leavename,
            //        HFday = txFinance.HFday,
            //    };
            //}

        }

        public class jsonCompoffsettings
        {
            public int CSDAYS { get; set; }
            public DateTime CSDATE { get; set; }
            public int CSTYPE { get; set; }

            public string compoffParameter { get; set; }
        }






        #endregion

        #region "Holiday Setting"

        public JsonResult GetHolidayempty()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            HolidaysList holidaylist = new HolidaysList(Guid.Empty, DefaultFinancialYr.Id);
            HolidaysList hl = new HolidaysList();
            for (int i = 0; i <= holidaylist.Count - 1; i++)
            {

                if (holidaylist[i].financeyearId == DefaultFinancialYr.Id)
                {
                    hl.Add(holidaylist[i]);
                }
            }

            List<jsonHolidaySettings> jsondata = new List<jsonHolidaySettings>();
            hl.ForEach(u => { jsondata.Add(jsonHolidaySettings.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }



        public JsonResult GetYearlylevopeningsdec()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            HolidaysList holidaylist = new HolidaysList(companyId);
            List<jsonHolidaySettings> jsondata = new List<jsonHolidaySettings>();
            holidaylist.ForEach(u => { jsondata.Add(jsonHolidaySettings.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        //public JsonResult GetHoliday(Guid Id)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    Holidays holidaylist = new Holidays(Id);
        //    if (!object.ReferenceEquals(holidaylist, null))
        //    {

        //        return base.BuildJson(true, 200, "success", jsonHolidaySettings.toJson(holidaylist));
        //    }
        //    else
        //    {
        //        return base.BuildJson(false, 200, "Please Complete Finance Year Setting", jsonHolidaySettings.toJson(holidaylist));
        //    }

        //}
        public class jsonHolidaychechabs
        {
            public string empcode { get; set; }
            public string empname { get; set; }
            public DateTime date { get; set; }
        }

        public class jsonWeekoffdatesget
        {
            public Guid Id { get; set; }
            public Guid Weekoffid { get; set; }
            public DateTime dates { get; set; }
            public string datesname { get; set; }
            public bool Halfday { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsActive { get; set; }
            public string Weekoff { get; set; }
            public string MonthorDate { get; set; }
        }

        public class jsonRevertdates
        {
            public string Revertdate { get; set; }
        }

        public class jsonHolidaydates
        {
            public string Holidaydate { get; set; }
            public string holiDay { get; set; }
            public string Reason { get; set; }
        }
        //public JsonResult checkHolidaySave(jsonHolidaySettings dataValue)
        //{

        //    bool status = false;
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
        //    Holidays holiday = new Holidays();
        //    var HolidayEntrydates = new List<DateTime?>();
        //    List<jsonHolidaychechabs> jsondataholiday = new List<jsonHolidaychechabs>();
        //    for (DateTime date = dataValue.HolidayFromDate; date <= dataValue.HolidayToDate; date = date.AddDays(1))
        //    {
        //        HolidayEntrydates.Add(date);
        //    }
        //    for (int m = 0; m <= HolidayEntrydates.Count - 1; m++)
        //    {
        //        //HolidayEntrydates.Add(date);
        //        DateTime checkingdates = HolidayEntrydates[m].Value;
        //        DataTable dtlevabs = holiday.GetleaabsentValues(checkingdates, DefaultFinancialid.Id);
        //        if (dtlevabs.Rows.Count > 0)
        //        {
        //            for (int n = 0; n <= dtlevabs.Rows.Count - 1; n++)
        //            {
        //                jsonHolidaychechabs holitemp = new jsonHolidaychechabs();
        //                holitemp.empcode = Convert.ToString(dtlevabs.Rows[n]["EmployeeCode"]);
        //                holitemp.empname = Convert.ToString(dtlevabs.Rows[n]["FirstName"]);
        //                holitemp.date = Convert.ToDateTime(dtlevabs.Rows[n]["LevDate"]);
        //                jsondataholiday.Add(holitemp);
        //                status = true;
        //            }
        //        }

        //    }

        //    if(status==false)
        //    {
        //        //this.SaveHoliday()
        //        //this.SaveHoliday(dataValue);
        //        return base.BuildJson(false, 100, "Holiday is already Exist for this date.", dataValue);
        //    }
        //    else
        //    {
        //        return base.BuildJson(false, 100, "", jsondataholiday);
        //    }

        //}
        public JsonResult yrlylevopndec(jsonHolidaySettings dataValue)
        {
            Holidays holiday = new Holidays();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["userid"]);
            //LeaveFinanceYearList financeYearlist = new LeaveFinanceYearList(companyId);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            bool isSaved = holiday.saveLevopendeclare(dataValue.levopndecID, companyId, dataValue.Category, dataValue.levtype, dataValue.opendays, userid, DefaultFinancialYr.Id);
            if (isSaved == true)
            {
                return base.BuildJson(true, 200, "Data Saved Succesfully!!!", "");
            }
            else
            {
                return base.BuildJson(false, 100, "Error While Saving the data!!!", "");
            }
        }

        public JsonResult SaveHoliday(jsonHolidaySettings dataValue)
        {
            var url = Url.Action("HolidayRevertleave", "Leave");
            int statuserror = 0;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
            List<jsonHolidaychechabs> jsondataholiday = new List<jsonHolidaychechabs>();
            List<jsonRevertdates> jsondataRevertdates = new List<jsonRevertdates>();
            List<jsonHolidaydates> jsondataRevHolidates = new List<jsonHolidaydates>();
            Holidays holiday = new Holidays();
            var HolidayEntrydates = new List<DateTime?>();
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            try
            {
                for (DateTime date = dataValue.HolidayFromDate; date <= dataValue.HolidayToDate; date = date.AddDays(1))
                {
                    HolidayEntrydates.Add(date);
                }
                for (int m = 0; m <= HolidayEntrydates.Count - 1; m++)
                {
                    //HolidayEntrydates.Add(date);
                    DateTime checkingdates = HolidayEntrydates[m].Value;
                    DataTable dtlevabs = holiday.GetleaabsentValues(checkingdates, DefaultFinancialid.Id);
                    if (dtlevabs.Rows.Count > 0)
                    {
                        for (int n = 0; n <= dtlevabs.Rows.Count - 1; n++)
                        {
                            jsonHolidaychechabs holitemp = new jsonHolidaychechabs();
                            holitemp.empcode = Convert.ToString(dtlevabs.Rows[n]["EmployeeCode"]);
                            holitemp.empname = Convert.ToString(dtlevabs.Rows[n]["FirstName"]);
                            holitemp.date = Convert.ToDateTime(dtlevabs.Rows[n]["LevDate"]);
                            jsondataholiday.Add(holitemp);

                        }
                    }

                }




                //IEnumerable<jsonHolidaychechabs> filteredList = jsondataholiday.Distinct().ToList(); ;
                var groupeddate = jsondataholiday
                    .GroupBy(u => u.date)
                    .ToList();
                for (int x = 0; x <= groupeddate.Count - 1; x++)
                {
                    jsonRevertdates jRevdate = new jsonRevertdates();
                    jRevdate.Revertdate = groupeddate[x].Key.Date.ToString();
                    jsondataRevertdates.Add(jRevdate);
                }
                for (int y = 0; y <= HolidayEntrydates.Count - 1; y++)
                {
                    jsonHolidaydates jHoliRevdate = new jsonHolidaydates();
                    jHoliRevdate.Holidaydate = HolidayEntrydates[y].Value.Date.ToString();
                    jHoliRevdate.holiDay = HolidayEntrydates[y].Value.DayOfWeek.ToString();
                    jHoliRevdate.Reason = dataValue.Reason;
                    jsondataRevHolidates.Add(jHoliRevdate);
                }

                //  for (int r = 0; r <= groupeddate.Count - 1; r++)
                /// {
                //Filtereddates[r].Value
                //}
                List<Holidays> hlist = new List<Holidays>();
                for (int i = 0; i <= HolidayEntrydates.Count - 1; i++)
                {
                    dataValue.HolidayDate = HolidayEntrydates[i].Value;




                    var holidayday = Convert.ToDateTime(dataValue.HolidayDate).DayOfWeek.ToString();
                    dataValue.HolidayDay = holidayday;
                    var Holidaydates = new List<DateTime?>();


                    //LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
                    DateTime fromdate = Convert.ToDateTime(DefaultFinancialid.StartMonth);
                    DateTime todate = Convert.ToDateTime(DefaultFinancialid.EndMonth);
                    DateTime holidate = Convert.ToDateTime(dataValue.HolidayDate);

                    if (DefaultFinancialid.Id == Guid.Empty)
                    {
                        statuserror = 2;
                        return base.BuildJson(false, 100, "Please set the Default Finance year in the Financial Setting.", dataValue);
                    }

                    if (holidate < fromdate || holidate > todate)
                    {
                        statuserror = 2;
                        return base.BuildJson(false, 100, "Please set the Holiday with in the Finance year.", dataValue);
                    }

                    HolidaysList holidaylist = new HolidaysList(Guid.Empty, DefaultFinancialid.Id);

                    //if((temp.Count !=0 && dataValue.Id == Guid.Empty)|| (temp.Count != 0 && dataValue.Id != Guid.Empty))
                    //{
                    //    return base.BuildJson(false, 100, "Holiday is already Exist for this date.", dataValue);
                    //}
                    for (int j = 0; j <= holidaylist.Count - 1; j++)
                    {

                        for (var Holidaydate = holidaylist[j].Holidaydate; Holidaydate <= holidaylist[j].Holidaydate; Holidaydate = Holidaydate.AddDays(1))
                        {
                            Holidaydates.Add(Holidaydate);
                        }

                    }



                    if (Holidaydates.Contains(dataValue.HolidayDate))
                    {
                        var temp = holidaylist.Where(u => u.Holidaydate == dataValue.HolidayDate).ToList();
                        if (temp[0].ComponentValue == dataValue.ComponentValue && temp[0].Holidaydate == dataValue.HolidayDate)
                        {
                            statuserror = 2;
                            return base.BuildJson(false, 100, "Holiday is already Exist for this date.", dataValue);
                        }
                        else
                        {

                        }

                    }

                    LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                    dataValue.FinId = DefaultFinancialYr.Id;
                    Holidays Holiday = jsonHolidaySettings.convertObject(dataValue);
                    hlist.Add(Holiday);
                    int userId = Convert.ToInt32(Session["UserId"]);
                    Holiday.CreatedBy = userId;
                    Holiday.ModifiedBy = Holiday.CreatedBy;
                }
                if (jsondataholiday.Count == 0)
                {
                    hlist.ForEach(h =>
                    {
                        bool isSaved = false;
                        isSaved = h.Save();

                    });
                    return base.BuildJson(true, 200, "Data saved successfully", dataValue);
                }
                else
                {
                    statuserror = 1;
                    return base.BuildJson(false, 100, "", new { jsondataholiday, jsondataRevHolidates, url, jsondataRevertdates, statuserror });
                    // return BuidJsonResult(true, Url.Action("HolidayRevertleave", "Leave"));
                }



            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                statuserror = 2;
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult RevertLeaveholiday(List<jsonHolidaydates> Holidaydates, List<jsonRevertdates> Revertdate, int bstatus)
        {
            if (bstatus == 1)
            {


                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
                Holidays holiday = new Holidays();
                for (int j = 0; j <= Revertdate.Count - 1; j++)
                {
                    holiday.RevertholiDelete(DefaultFinancialid.Id, Convert.ToDateTime(Revertdate[j].Revertdate));
                }
            }

            RevertLeaveholidaySave(Holidaydates);

            return base.BuildJson(true, 200, "", null);
        }

        public JsonResult RevertLeaveholidaySave(List<jsonHolidaydates> Holidaydates)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
            Holidays holiday = new Holidays();
            for (int l = 0; l <= Holidaydates.Count - 1; l++)
            {
                holiday.Id = Guid.Empty;
                holiday.Holidaydate = Convert.ToDateTime(Holidaydates[l].Holidaydate);
                holiday.holidayDAY = Holidaydates[l].holiDay;
                holiday.HolidayReason = Holidaydates[l].Reason;
                holiday.financeyearId = DefaultFinancialid.Id;
                holiday.Save();
            }
            return base.BuildJson(true, 200, "Holiday Saved  successfully", null);

        }
        public JsonResult DeleteHoliday(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Holidays holiday = new Holidays();
            holiday.Id = id;
            holiday.ModifiedBy = userId;
            if (holiday.Delete())
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }

        }

        public JsonResult DeleteYearlyopeningDeclaration(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Holidays holiday = new Holidays();
            holiday.Id = id;
            holiday.ModifiedBy = userId;
            if (holiday.DeleteYrlyopendec())
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }

        }
        public class jsonHolidaySettings
        {

            public Guid Id { get; set; }
            public Guid FinId { get; set; }

            public DateTime HolidayFromDate { get; set; }
            public DateTime HolidayToDate { get; set; }
            public DateTime HolidayDate { get; set; }

            public string HolidayDay { get; set; }

            public string Reason { get; set; }
            public Guid Category { get; set; }
            public Guid levtype { get; set; }
            public decimal opendays { get; set; }
            public Guid levopndecID { get; set; }
            public string CATIDNAME { get; set; }
            public string LEVIDNAME { get; set; }
            public string Component { get; set; }
            public Guid ComponentValue { get; set; }
            public string Type { get; set; }
            public string ComponentName { get; set; }

            public static jsonHolidaySettings toJson(Holidays holiday)
            {
                return new jsonHolidaySettings()
                {
                    Id = holiday.Id,
                    FinId = holiday.financeyearId,
                    HolidayDate = holiday.Holidaydate,
                    HolidayDay = holiday.holidayDAY,
                    Reason = holiday.HolidayReason,
                    Category = holiday.catid,
                    levtype = holiday.levid,
                    opendays = holiday.opdays,
                    levopndecID = holiday.yrlylevopndecID,
                    CATIDNAME = holiday.CATIDNAME,
                    LEVIDNAME = holiday.LEVIDNAME,
                    Component = holiday.Component,
                    ComponentValue = holiday.ComponentValue,
                    Type = holiday.Type,
                    ComponentName = holiday.ComponentName
                };
            }
            public static Holidays convertObject(jsonHolidaySettings holset)
            {
                return new Holidays()
                {
                    Id = holset.Id,
                    financeyearId = holset.FinId,
                    Holidaydate = holset.HolidayDate,
                    holidayDAY = holset.HolidayDay,
                    HolidayReason = holset.Reason,
                    Component = holset.Component,
                    ComponentValue = holset.ComponentValue,
                    Type = holset.Type,
                    ComponentName = holset.ComponentName
                };
            }
        }

        public class jsonMailConfig
        {

            public Guid Id { get; set; }
            public string IPAddress { get; set; }

            public int PortNo { get; set; }
            public string FromEmail { get; set; }
            public bool EnableSSL { get; set; }

            public bool AuthenEmail { get; set; }

            public string AuthenSMTPUser { get; set; }

            public string AuthenSMTPPwd { get; set; }

            public string MailPassword { get; set; }

            public string CCMail { get; set; }

            public string mailapproval { get; set; }
            public static jsonMailConfig toJson(MailConfig mailConfig)
            {
                return new jsonMailConfig()
                {
                    Id = mailConfig.Id,
                    IPAddress = mailConfig.IPAddress,
                    PortNo = mailConfig.PortNo,
                    FromEmail = mailConfig.FromEmail,
                    EnableSSL = mailConfig.EnableSSL,
                    AuthenEmail = mailConfig.AuthenEmail,
                    AuthenSMTPUser = mailConfig.AuthenSMTPUser,
                    AuthenSMTPPwd = mailConfig.AuthenSMTPPwd,
                    MailPassword = mailConfig.MailPassword,
                    CCMail = mailConfig.CCMail,
                    mailapproval = mailConfig.mailapproval

                };
            }
            public static MailConfig convertObject(jsonMailConfig mailConfig)
            {
                return new MailConfig()
                {
                    Id = mailConfig.Id,
                    IPAddress = mailConfig.IPAddress,
                    PortNo = mailConfig.PortNo,
                    FromEmail = mailConfig.FromEmail,
                    EnableSSL = mailConfig.EnableSSL,
                    AuthenEmail = mailConfig.AuthenEmail,
                    AuthenSMTPUser = mailConfig.AuthenSMTPUser,
                    AuthenSMTPPwd = mailConfig.AuthenSMTPPwd,
                    MailPassword = mailConfig.MailPassword,
                    CCMail = mailConfig.CCMail,
                    mailapproval = mailConfig.mailapproval
                };
            }
        }
        #endregion

        #region "Leave openings"
        public JsonResult GetLeaveOpenings(Guid leavType)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            LeaveOpeningList leavOpeningList = new LeaveOpeningList(Guid.Empty, DefaultFinancialYr.Id, leavType);
            //LeaveRequest debitbal = new LeaveRequest(LeaveType, EmployeeId, companyId, DefaultFinancialYr.Id, 0);
            EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId);
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });

            LeaveTypeSettingList levtypeSettinglist = new LeaveTypeSettingList(companyId, DefaultFinancialYr.Id);
            var levtypesettings = levtypeSettinglist.Where(l => l.LeaveTypeId == leavType).FirstOrDefault();
            string leaveopenings = string.Empty;
            if (!object.ReferenceEquals(levtypesettings, null))
            {
                leaveopenings = levtypesettings.LevopenReq;
            }

            List<object> result = new List<object>();

            List<jsonEmployee> tempjsondata = new List<jsonEmployee>();
            tempjsondata = jsondata.Where(x => x.empSeparationDate == "").ToList();
            tempjsondata.ForEach(d =>
            {
                LeaveOpenings lev = leavOpeningList.Where(l => l.EmployeeId == d.empid).FirstOrDefault();
                result.Add(

               new
               {
                   empid = d.empid,
                   empCode = d.empCode,
                   empName = d.empFName + d.empLName,
                   designation = d.designation,
                   dateOfJoin = d.empDOJ,
                   leaveOpening = lev == null ? "" : lev.LeaveOpening.ToString(),
                   leaveCredit = lev == null ? "" : lev.LeaveCredit.ToString(),
                   leaveUsed = lev == null ? "" : lev.LeaveUsed.ToString(),
                   leaveopening = leaveopenings,

               });
            });

            return base.BuildJson(true, 200, "success", result);

        }
        public JsonResult loadCreditCategory()
        {
            LeaveCreditSettingsBO objcrset = new LeaveCreditSettingsBO();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            objcrset.CompanyId = companyId;
            var comp = objcrset.LoadCreditcategory(companyId);
            return base.BuildJson(true, 200, "" + "00000000-0000-0000-0000-000000000000", comp);
        }
        public JsonResult loadCreditLeavetype(Guid Cattype)
        {
            LeaveCreditSettingsBO objcrset = new LeaveCreditSettingsBO();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            objcrset.CompanyId = companyId;
            var comp = objcrset.LoadCreditLeavetype(companyId, Cattype);
            return base.BuildJson(true, 200, "" + "00000000-0000-0000-0000-000000000000", comp);
        }
        public JsonResult loadCreditprocessDates(Guid Cattype, Guid levid)
        {
            LeaveCreditSettingsBO objcrset = new LeaveCreditSettingsBO();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            objcrset.CompanyId = companyId;
            var comp = objcrset.LoadCreditLeaveDates(companyId, Cattype, levid);
            return base.BuildJson(true, 200, "", comp);
        }


        #region "Leave Master Setting"
        public JsonResult getleavemastersettings()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            Leavemastersettings.FinyrId = DefaultFinancialYr.Id;
            Leavemastersettings.CompanyId = companyId;
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);

            if (levmasterlist.Count != 0)
            {
                levmasterlist.ForEach(f =>
                {
                    f.FinYearEnd = DefaultFinancialYr.EndMonth;
                });
                return base.BuildJson(true, 100, "", levmasterlist);
            }

            else
            {
                return base.BuildJson(false, 200, "", "");
            }

        }
        public JsonResult GetLeavemasterDropdown()
        {
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> FullListaddfldDD = new List<LeaveSettingsBO>();
            Leavemastersettings.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            DataTable DtAdditionaloptionalfieldDD = Leavemastersettings.GetAdditonalDropdownValues();
            LeaveSettingsBO DDaditionalOptfld = new LeaveSettingsBO();
            if (DtAdditionaloptionalfieldDD.Rows.Count != 0)
            {
                for (int i = 0; i <= DtAdditionaloptionalfieldDD.Rows.Count - 1; i++)
                {
                    LeaveSettingsBO addfldDD = new LeaveSettingsBO();
                    addfldDD.name = DtAdditionaloptionalfieldDD.Rows[i]["name"].ToString();
                    FullListaddfldDD.Add(addfldDD);
                }
                return base.BuildJson(true, 200, "", FullListaddfldDD);
            }
            else
            {
                return base.BuildJson(true, 200, "", "");
            }
        }

        public JsonResult GetEncashmentlevtypeDropdown()
        {
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> FullListaddfldDD = new List<LeaveSettingsBO>();
            Leavemastersettings.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(Leavemastersettings.CompanyId);
            Leavemastersettings.FinyrId = DefaultFinancialYr.Id;
            DataTable DtEncashlevtypeDDL = Leavemastersettings.GetENCASHLevtypeDropdownValues();
            LeaveSettingsBO DDaditionalOptfld = new LeaveSettingsBO();
            if (DtEncashlevtypeDDL.Rows.Count != 0)
            {
                for (int i = 0; i <= DtEncashlevtypeDDL.Rows.Count - 1; i++)
                {
                    LeaveSettingsBO addfldDD = new LeaveSettingsBO();
                    addfldDD.name = DtEncashlevtypeDDL.Rows[i]["LevDesc"].ToString();
                    addfldDD.Id = new Guid(DtEncashlevtypeDDL.Rows[i]["LevTypeId"].ToString());
                    FullListaddfldDD.Add(addfldDD);
                }
                return base.BuildJson(true, 200, "", FullListaddfldDD);
            }
            else
            {
                return base.BuildJson(true, 200, "", "");
            }
        }


        public JsonResult leavemastersettingssave(JsonLeaveSettings LeaveSettingData)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id != Guid.Empty)
            {
                LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
                Leavemastersettings.Id = Guid.Empty;
                Leavemastersettings.FinyrId = DefaultFinancialYr.Id;
                Leavemastersettings.CompanyId = companyId;
                Leavemastersettings.leaveparameter = LeaveSettingData.leaveparameter;
                Leavemastersettings.Holidayparameter = LeaveSettingData.Holidayparameter;
                Leavemastersettings.Compoffparameter = LeaveSettingData.Compoffparameter;
                Leavemastersettings.Weekoffparameter = LeaveSettingData.Weekoffparameter;
                Leavemastersettings.Weekoffentryvalid = LeaveSettingData.Weekoffentryvalid;
                Leavemastersettings.RpComp1 = LeaveSettingData.RpComp1;
                Leavemastersettings.RpComp2 = LeaveSettingData.RpComp2;
                Leavemastersettings.RpComp3 = LeaveSettingData.RpComp3;
                Leavemastersettings.RpComp4 = LeaveSettingData.RpComp4;
                Leavemastersettings.RpComp5 = LeaveSettingData.RpComp5;
                Leavemastersettings.Minormaxparameter = LeaveSettingData.Minormaxparameter;
                Leavemastersettings.mindays = Convert.ToDecimal(LeaveSettingData.mindays);
                Leavemastersettings.maxmintimes = Convert.ToDecimal(LeaveSettingData.maxmintimes);
                Leavemastersettings.leavecreditparameter = LeaveSettingData.leavecreditparameter;
                Leavemastersettings.encashmentparameter = LeaveSettingData.encashmentparameter;

                if (LeaveSettingData.maxdays != null)
                {
                    Leavemastersettings.maxdays = Convert.ToDecimal(LeaveSettingData.maxdays);
                }
                else
                {
                    Leavemastersettings.maxdays = 0;
                }
                if (LeaveSettingData.maxmaxtimes != null)
                {
                    Leavemastersettings.maxmaxtimes = Convert.ToDecimal(LeaveSettingData.maxmaxtimes);
                }
                else
                {
                    Leavemastersettings.maxmaxtimes = 0;
                }
                Leavemastersettings.maxperiod = LeaveSettingData.maxperiod;
                Leavemastersettings.maxdeviation = LeaveSettingData.maxdeviation;
                if (LeaveSettingData.mindays != null)
                {
                    Leavemastersettings.mindays = Convert.ToDecimal(LeaveSettingData.mindays);
                }
                else
                {
                    Leavemastersettings.mindays = 0;
                }
                if (LeaveSettingData.maxmintimes != null)
                {
                    Leavemastersettings.maxmintimes = Convert.ToDecimal(LeaveSettingData.maxmintimes);
                }
                else
                {
                    Leavemastersettings.maxmintimes = 0;
                }
                Leavemastersettings.minperiod = LeaveSettingData.minperiod;
                Leavemastersettings.mindeviation = LeaveSettingData.mindeviation;

                Leavemastersettings.Createdby = Session["UserId"].ToString();

                Leavemastersettings.Saveleavesettingmaster();

                return base.BuildJson(true, 200, "success", LeaveSettingData);
            }
            else
            {
                return base.BuildJson(false, 200, "Please Set Default Financial year", LeaveSettingData);
            }


        }

        #endregion

        #region "Leave Type Setting"

        public JsonResult LeaveTypeSettingsSave(JsonLeaveTypeSettings LeaveTypeData)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id != Guid.Empty)
            {
                LeaveSettingsBO Leavetypesetting = new LeaveSettingsBO();
                Leavetypesetting.CompanyId = companyId;
                Leavetypesetting.FinyrId = DefaultFinancialYr.Id;
                Leavetypesetting.LeaveTypeId = new Guid(LeaveTypeData.LeaveTypeId);
                Leavetypesetting.LeaveTypeDesc = LeaveTypeData.LeaveTypeDesc;
                Leavetypesetting.LevopenReq = LeaveTypeData.LevopenReq;
                Leavetypesetting.LevEncashAvail = LeaveTypeData.LevEncashAvail;
                //Leavetypesetting.Openingbal = new Guid(LeaveTypeData.Openingbal);
                //Leavetypesetting.avalbal = new Guid(LeaveTypeData.avalbal);
                Leavetypesetting.usedlev = new Guid(LeaveTypeData.usedlev);
                Leavetypesetting.LevTypeActive = LeaveTypeData.LevTypeActive;
                Leavetypesetting.Createdby = Session["UserId"].ToString();
                Leavetypesetting.SaveleaveTypeSettingsData();

                return base.BuildJson(true, 200, "success", "");
            }
            else
            {
                return base.BuildJson(false, 200, "Please Set Default Financial year", "");
            }


        }
        public JsonResult DeleteLeaveTypeSetting(Guid id)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveRequestList leav = new LeaveRequestList(Guid.Empty, companyId, DefaultFinancialYr.Id);
            if (leav.Count == 0)
            {
                LeaveSettingsBO BOdelLevTypeSettings = new LeaveSettingsBO();
                BOdelLevTypeSettings.Id = id;
                BOdelLevTypeSettings.Createdby = Session["UserId"].ToString();
                bool isSuccess = BOdelLevTypeSettings.DeleteleaveTypeSettingsData();
                if (isSuccess == true)
                {
                    return base.BuildJson(true, 200, "Leave Settings has been succesfully deleted", "");
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while deleting the data!!!", "");
                }
            }
            else
            {
                return base.BuildJson(false, 100, "You can't able to delete the Master settings,once the Leave request transaction has been done", "");
            }


        }









        public JsonResult GetLeaveTypeSettingsSave()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveSettingsBO LeaveTypesettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> Fulllevtypesettinglist = new List<LeaveSettingsBO>();
            LeaveTypesettings.CompanyId = companyId;
            LeaveTypesettings.FinyrId = DefaultFinancialYr.Id;

            LeaveTypeSettingList levtyplist = new LeaveTypeSettingList(companyId, DefaultFinancialYr.Id);
            if (levtyplist.Count != 0)
            {

                return base.BuildJson(true, 100, "", levtyplist);
            }
            else
            {
                return base.BuildJson(true, 200, "", "");
            }

        }

        #endregion

        #region "Leave Configuration Setting"


        public JsonResult GetDYNAMICconfigurationsettings(string parametertype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> FullListofdynamic = new List<LeaveSettingsBO>();
            Leavemastersettings.FinyrId = DefaultFinancialYr.Id;
            Leavemastersettings.CompanyId = companyId;
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);
            DataSet dtset = Leavemastersettings.GetTableValues(companyId, DefaultFinancialYr.Id);
            DataTable DtMastersettings = dtset.Tables[0];

            if (levmasterlist.Count != 0)
            {
                Leavemastersettings.dynamicvalue = DtMastersettings.Rows[0][parametertype].ToString();

                if (Leavemastersettings.dynamicvalue != "companywise")
                {
                    DataTable DTDynamicvalue = Leavemastersettings.GetDynamicvalue();
                    if (DTDynamicvalue.Rows.Count != 0)
                    {
                        for (int i = 0; i <= DTDynamicvalue.Rows.Count - 1; i++)
                        {
                            LeaveSettingsBO dynamic = new LeaveSettingsBO();
                            dynamic.Id = new Guid(DTDynamicvalue.Rows[i]["Id"].ToString());
                            dynamic.name = DTDynamicvalue.Rows[i]["name"].ToString();
                            dynamic.FinYearStart = DefaultFinancialYr.StartMonth;
                            dynamic.FinYearEnd = DefaultFinancialYr.EndMonth;
                            dynamic.Minormaxparameter = levmasterlist[0].Minormaxparameter;
                            dynamic.mindays = levmasterlist[0].mindays;
                            dynamic.maxmintimes = levmasterlist[0].maxmintimes;
                            dynamic.maxdays = levmasterlist[0].maxdays;
                            dynamic.maxmaxtimes = levmasterlist[0].maxmaxtimes;
                            FullListofdynamic.Add(dynamic);
                        }
                    }
                    else
                    {
                        LeaveSettingsBO dynamictemp = new LeaveSettingsBO();
                        dynamictemp.FinYearStart = DefaultFinancialYr.StartMonth;
                        dynamictemp.FinYearEnd = DefaultFinancialYr.EndMonth;
                        FullListofdynamic.Add(dynamictemp);
                    }
                    return base.BuildJson(true, 100, Leavemastersettings.dynamicvalue.ToString().ToUpper(), FullListofdynamic);
                }
                else
                {
                    LeaveSettingsBO dynamic1 = new LeaveSettingsBO();
                    dynamic1.FinYearStart = DefaultFinancialYr.StartMonth;
                    dynamic1.FinYearEnd = DefaultFinancialYr.EndMonth;
                    dynamic1.Minormaxparameter = levmasterlist[0].Minormaxparameter;
                    dynamic1.mindays = levmasterlist[0].mindays;
                    dynamic1.maxmintimes = levmasterlist[0].maxmintimes;
                    dynamic1.maxdays = levmasterlist[0].maxdays;
                    dynamic1.maxmaxtimes = levmasterlist[0].maxmaxtimes;
                    FullListofdynamic.Add(dynamic1);
                    return base.BuildJson(true, 100, Leavemastersettings.dynamicvalue.ToString().ToUpper(), FullListofdynamic);
                }




            }
            else
            {
                return base.BuildJson(false, 200, "Please Set the Leave Master settings", "");
            }

        }

        public JsonResult GetComponentmatchingforleaveconfig()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> FullComplist = new List<LeaveSettingsBO>();
            Leavemastersettings.CompanyId = companyId;
            DataTable Dtcomponentmatching = Leavemastersettings.GetComponentmatchingforleave();
            if (Dtcomponentmatching.Rows.Count != 0)
            {
                for (int i = 0; i <= Dtcomponentmatching.Rows.Count - 1; i++)
                {
                    LeaveSettingsBO Complist = new LeaveSettingsBO();
                    Complist.Id = new Guid(Dtcomponentmatching.Rows[i]["Id"].ToString());
                    Complist.name = Dtcomponentmatching.Rows[i]["Name"].ToString();
                    FullComplist.Add(Complist);
                }
            }

            return base.BuildJson(false, 200, "", FullComplist);


        }
        public JsonResult Getleaveconfigdetails(Guid LeaveTypeId, Guid DynamicTypeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);

            LeaveSettingsBO LeaveConfigurations = new LeaveSettingsBO();

            LeaveConfigurations.FinyrId = DefaultFinancialYr.Id;
            LeaveConfigurations.DynamicComponentValue = DynamicTypeId;
            LeaveConfigurations.LeaveTypeId = LeaveTypeId;

            LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialYr.Id, LeaveTypeId, DynamicTypeId);
            if (Levconfiglist.Count != 0)
            {

                return base.BuildJson(true, 100, "", Levconfiglist);
            }
            else
            {
                return base.BuildJson(false, 200, "", "");

            }



        }

        public JsonResult LeaveConfigurationSave(JsonLeaveConfiguration LeaveConfigData)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id != Guid.Empty)
            {
                LeaveSettingsBO LeaveConfiguration = new LeaveSettingsBO();
                //CompanyID
                LeaveConfiguration.CompanyId = companyId;

                //LeaveTypeID
                LeaveConfiguration.LeaveTypeId = new Guid(LeaveConfigData.LeaveTypeId);

                //DynamicComponentNameAndValue
                LeaveConfiguration.DynamicComponentName = LeaveConfigData.DynamicComponentName;
                if (LeaveConfiguration.DynamicComponentName.ToLower() == "companywise")
                {
                    LeaveConfiguration.DynamicComponentValue = Guid.Empty;
                }
                else
                {
                    LeaveConfiguration.DynamicComponentValue = new Guid(LeaveConfigData.DynamicComponentValue);
                }



                //FinYearDetails
                LeaveConfiguration.FinyrId = DefaultFinancialYr.Id;
                LeaveConfiguration.FinYearStart = DefaultFinancialYr.StartMonth;
                LeaveConfiguration.FinYearEnd = DefaultFinancialYr.EndMonth;
                LeaveConfiguration.ConfigEffectiveDate = Convert.ToDateTime(LeaveConfigData.ConfigEffectiveDate);

                //Monthly EligibilityDetails
                LeaveConfiguration.MaxDayMonth = Convert.ToDecimal(LeaveConfigData.MaxDayMonth);
                LeaveConfiguration.AllowDevisionMonth = LeaveConfigData.AllowDevisionMonth;



                //Specified SettingsDetails
                // LeaveConfiguration.LevopenReq = LeaveConfigData.LevopenReq;
                LeaveConfiguration.overallMax = Convert.ToDecimal(LeaveConfigData.overallMax.ToString());
                LeaveConfiguration.carryLimit = Convert.ToDecimal(LeaveConfigData.carryLimit.ToString());
                LeaveConfiguration.Compoffallow = LeaveConfigData.Compoffallow;

                //InterveningHolidays
                LeaveConfiguration.InvHoliday = LeaveConfigData.InvHoliday;
                LeaveConfiguration.InvHolidaysubparameter = LeaveConfigData.InvHolidaysubparameter;

                //CreationdetailDetails
                LeaveConfiguration.Createdby = Session["UserId"].ToString();


                //MINANDMAXDetails
                if (LeaveConfigData.maxdays != null)
                {
                    LeaveConfiguration.maxdays = Convert.ToDecimal(LeaveConfigData.maxdays);
                }
                else
                {
                    LeaveConfiguration.maxdays = 0;
                }
                if (LeaveConfigData.maxmaxtimes != null)
                {
                    LeaveConfiguration.maxmaxtimes = Convert.ToDecimal(LeaveConfigData.maxmaxtimes);
                }
                else
                {
                    LeaveConfiguration.maxmaxtimes = 0;
                }
                LeaveConfiguration.maxperiod = LeaveConfigData.maxperiod;
                LeaveConfiguration.maxdeviation = LeaveConfigData.maxdeviation;
                if (LeaveConfigData.mindays != null)
                {
                    LeaveConfiguration.mindays = Convert.ToDecimal(LeaveConfigData.mindays);
                }
                else
                {
                    LeaveConfiguration.mindays = 0;
                }
                if (LeaveConfigData.maxmintimes != null)
                {
                    LeaveConfiguration.maxmintimes = Convert.ToDecimal(LeaveConfigData.maxmintimes);
                }
                else
                {
                    LeaveConfiguration.maxmintimes = 0;
                }
                LeaveConfiguration.minperiod = LeaveConfigData.minperiod;
                LeaveConfiguration.mindeviation = LeaveConfigData.mindeviation;


                //attachmentrequired
                LeaveConfiguration.Isattachreq = LeaveConfigData.Isattachreq;
                if (LeaveConfiguration.Isattachreq == "Y")
                {
                    LeaveConfiguration.Attachreqmaxdays = Convert.ToDecimal(LeaveConfigData.Attachreqmaxdays.ToString());
                }
                else
                {

                    LeaveConfiguration.Attachreqmaxdays = 0;
                }


                LeaveConfiguration.SaveleaveConfigurations();

                return base.BuildJson(true, 200, "success", "");
            }
            else
            {
                return base.BuildJson(false, 200, "Please Set Default Financial year", "");
            }


        }


        #endregion
        #region "Leave Encashment Settings"

        public JsonResult LeaveEncashmentsave(JsonLeaveEncashment LeaveEncashment)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id != Guid.Empty)
            {
                LeaveSettingsBO BOleaveencashment = new LeaveSettingsBO();
                BOleaveencashment.LeaveTypeId = new Guid(LeaveEncashment.LeaveTypeId);
                BOleaveencashment.encashmentparameter =(LeaveEncashment.Encashparameter);
                BOleaveencashment.EncashLimit = Convert.ToDecimal(LeaveEncashment.EncashLimit);
                BOleaveencashment.Encashcomponent =new Guid(LeaveEncashment.Encashcomponent);
                BOleaveencashment.Createdby = Session["UserId"].ToString();
                BOleaveencashment.FinyrId = DefaultFinancialYr.Id;
                BOleaveencashment.CompanyId = companyId;
                BOleaveencashment.SaveLeaveLeaveEncashmentSettings();
                return base.BuildJson(true, 200, "success", "");
            }
            else
            {
                return base.BuildJson(false, 200, "Please Set Default Financial year", "");
            }


        }
        public JsonResult GetLeaveEncashment(string EncashComp, string LeaveTypeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id != Guid.Empty)
            {
                LeaveSettingsBO BOleaveencashment = new LeaveSettingsBO();
                List<JsonLeaveEncashment> FullDetailsofencashment = new List<JsonLeaveEncashment>();
                BOleaveencashment.LeaveTypeId = new Guid(LeaveTypeId);
                Guid tempencash = string.IsNullOrEmpty(EncashComp) || EncashComp.ToLower().Trim()=="select" ? Guid.Empty : new Guid(EncashComp);
                BOleaveencashment.encashmentparameter =Convert.ToString(tempencash);
                BOleaveencashment.Createdby = Session["UserId"].ToString();
                BOleaveencashment.FinyrId = DefaultFinancialYr.Id;
                BOleaveencashment.CompanyId = companyId;
                DataTable Dtencashsettings = BOleaveencashment.GetLeaveLeaveEncashmentSettings();
                if (Dtencashsettings.Rows.Count != 0)
                {
                    JsonLeaveEncashment Detailsofencashment = new JsonLeaveEncashment();
                    Detailsofencashment.LeaveTypeId = Dtencashsettings.Rows[0]["LeaveTypeId"].ToString();
                    Detailsofencashment.Encashcomponent = Dtencashsettings.Rows[0]["Encashcomp"].ToString();
                    Detailsofencashment.EncashLimit = Dtencashsettings.Rows[0]["EncashLmit"].ToString();
                    FullDetailsofencashment.Add(Detailsofencashment);
                    return base.BuildJson(true, 200, "success", FullDetailsofencashment);
                }
                else
                {
                    return base.BuildJson(false, 200, "success", "");
                }

            }
            else
            {
                return base.BuildJson(false, 200, "Please Set Default Financial year", "");
            }


        }


        #endregion
        #region "Week Off Setting"


        #endregion
        public JsonResult GetDynamicHolidaysettings()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveSettingsBO Leavemastersettings = new LeaveSettingsBO();
            List<LeaveSettingsBO> FullListofdynamic = new List<LeaveSettingsBO>();
            Leavemastersettings.FinyrId = DefaultFinancialYr.Id;
            Leavemastersettings.CompanyId = companyId;
            DataSet dtset = Leavemastersettings.GetTableValues(companyId, DefaultFinancialYr.Id);
            DataTable DtMastersettings = dtset.Tables[0];
            if (DtMastersettings.Rows.Count != 0)
            {
                Leavemastersettings.dynamicvalue = DtMastersettings.Rows[0]["HolidayParameter"].ToString();

                if (Leavemastersettings.dynamicvalue != "companywise")
                {
                    DataTable DTDynamicvalue = Leavemastersettings.GetDynamicvalue();
                    if (DTDynamicvalue.Rows.Count != 0)
                    {
                        for (int i = 0; i <= DTDynamicvalue.Rows.Count - 1; i++)
                        {
                            LeaveSettingsBO dynamic = new LeaveSettingsBO();
                            dynamic.Id = new Guid(DTDynamicvalue.Rows[i]["Id"].ToString());
                            dynamic.name = DTDynamicvalue.Rows[i]["name"].ToString();
                            dynamic.FinYearStart = DefaultFinancialYr.StartMonth;
                            dynamic.FinYearEnd = DefaultFinancialYr.EndMonth;
                            FullListofdynamic.Add(dynamic);
                        }
                    }

                    return base.BuildJson(true, 100, Leavemastersettings.dynamicvalue.ToString().ToUpper(), FullListofdynamic);
                }
                else
                {
                    LeaveSettingsBO dynamic1 = new LeaveSettingsBO();
                    dynamic1.FinYearStart = DefaultFinancialYr.StartMonth;
                    dynamic1.FinYearEnd = DefaultFinancialYr.EndMonth;
                    FullListofdynamic.Add(dynamic1);
                    return base.BuildJson(true, 100, Leavemastersettings.dynamicvalue.ToString().ToUpper(), FullListofdynamic);
                }




            }
            else
            {
                return base.BuildJson(false, 200, "Please Set the Leave Master settings", "");
            }

        }



        public JsonResult SaveLeaveOpenings(List<LeaveOpenings> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            bool ConfigSetStat = true;
            bool LeaveOpeningLmtExcdStat = false;
            double MaxLeavOpening = 0;
            Guid EmployeeExceedsLmt = Guid.Empty;
            LeaveRequest LeaverequestBO = new LeaveRequest();
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialYr.Id);
            dataValue.ForEach(k =>
            {
                if (ConfigSetStat == true && LeaveOpeningLmtExcdStat == false)
                {
                    Employee employeeDetail = new Employee(k.EmployeeId);
                    string LeaveConfigurationparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].leaveparameter, employeeDetail);                       //LeaveConfigurationparameterid
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
                    LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialYr.Id, k.LeaveType, new Guid(LeaveConfigurationparameterid));
                    if (Levconfiglist.Count > 0)
                    {
                        MaxLeavOpening = 0;
                        MaxLeavOpening = Convert.ToDouble(Levconfiglist[0].overallMax);
                        double ActMaxOpenCred = 0;
                        ActMaxOpenCred = k.LeaveOpening + k.LeaveCredit;
                        if (ActMaxOpenCred > MaxLeavOpening)
                        {
                            LeaveOpeningLmtExcdStat = true;
                            EmployeeExceedsLmt = k.EmployeeId;
                        }
                    }
                    else
                    {
                        ConfigSetStat = false;
                    }
                }
            });
            Employee LmtExceedsEmployee = new Employee(EmployeeExceedsLmt);
            if (ConfigSetStat == true && LeaveOpeningLmtExcdStat == false)
            {
                if (DefaultFinancialYr.Id != Guid.Empty)
                {
                    dataValue.ForEach(d =>
                    {
                        LeaveOpenings leaveOpening = new LeaveOpenings();
                        leaveOpening.Id = d.Id;
                        leaveOpening.FinanceYearId = DefaultFinancialYr.Id;
                        leaveOpening.EmployeeId = d.EmployeeId;
                        leaveOpening.LeaveOpening = d.LeaveOpening;
                        leaveOpening.LeaveCredit = d.LeaveCredit;
                        leaveOpening.LeaveType = d.LeaveType;
                        leaveOpening.Save();
                    });

                    return base.BuildJson(true, 200, "success", dataValue);
                }
                else
                {
                    return base.BuildJson(false, 200, "Please Set Default Financial year", dataValue);
                }
            }
            else if (ConfigSetStat == false)
            {
                return base.BuildJson(false, 200, "Kindly Set the leave configuration", dataValue);
            }
            else
            {
                return base.BuildJson(false, 200, "Leave Opening and Credits of employee " + LmtExceedsEmployee.FirstName + " " + LmtExceedsEmployee.LastName + " (" + LmtExceedsEmployee.EmployeeCode + ")" + "exceeds the Maximum Limit.For more Details, Kindly check the Leave configuration setting of Particular Employee.", dataValue);
            }


        }





        public JsonResult LeaveCreditProcess(string CategoryTypeId, string LeaveTypeId, string processdate, string LastProcessDate)
        {
            //LeaveCreditProcess creditpro = new LeaveCreditProcess();
            bool status = false;
            //bool statusloop;
            LeaveCreditSettingsBO creditpro = new LeaveCreditSettingsBO();
            creditpro.ProcessDate = Convert.ToDateTime(processdate);
            creditpro.LastProcessDate = Convert.ToDateTime(LastProcessDate);
            creditpro.LeaveTypeId = new Guid(LeaveTypeId.ToString());
            creditpro.CategoryTypeId = new Guid(CategoryTypeId.ToString());
            creditpro.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            creditpro.CreatedBy = Convert.ToInt32(Session["UserId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(Convert.ToInt32(Session["CompanyId"]));
            creditpro.FinyearId = DefaultFinancialYr.Id;
            EmployeeList emplist = new EmployeeList(Convert.ToInt32(Session["CompanyId"]), new Guid(CategoryTypeId));
            DataTable DTCreditsettings = creditpro.GetSinglevalue();
            TimeSpan difference = Convert.ToDateTime(processdate) - Convert.ToDateTime(LastProcessDate);
            int restrictiondays = Convert.ToInt32(DTCreditsettings.Rows[0]["midmontdate"].ToString());
            creditpro.CrDays = Convert.ToDouble(DTCreditsettings.Rows[0]["CrDays"].ToString());
            if (Convert.ToBoolean(DTCreditsettings.Rows[0]["Monthflag"].ToString()) == true)//Monthly Cycle
            {
                DateTime LPdate = Convert.ToDateTime(LastProcessDate);
                int LPMonth = LPdate.Month;
                DateTime CPdate = Convert.ToDateTime(processdate);
                int CPMonth = CPdate.Month;
                int MLoop = CPMonth - LPMonth;
                for (int j = 0; j <= MLoop; j++)
                {

                    DateTime Monthcurrentprocessdate = Convert.ToDateTime(LastProcessDate);

                    if (Monthcurrentprocessdate.Month == 12) // its end of year , we need to add another year to new date:
                    {
                        Monthcurrentprocessdate = new DateTime((Monthcurrentprocessdate.Year + 1), 1, 1);
                    }
                    else
                    {
                        Monthcurrentprocessdate = new DateTime(Monthcurrentprocessdate.Year, (Monthcurrentprocessdate.Month + 1), 1);
                    }

                    LastProcessDate = Monthcurrentprocessdate.ToString();
                    DateTime Monthrestrictionperiod = Convert.ToDateTime(LastProcessDate).AddDays(restrictiondays - 1);
                    var MonthCurrentmontANDlessthanEmployees = emplist.Where(u => u.DateOfJoining <= Monthrestrictionperiod).ToList();
                    bool Monthsavestatus;

                    for (int emp = 0; emp < MonthCurrentmontANDlessthanEmployees.Count; emp++)
                    {
                        //statusloop = true;
                        if (Convert.ToDateTime(processdate) > Convert.ToDateTime(LastProcessDate))
                        {
                            Monthsavestatus = creditpro.CreditProcessing(MonthCurrentmontANDlessthanEmployees[emp].Id);
                            // statusloop = true;
                        }


                    }

                    if (Convert.ToDateTime(processdate) > Convert.ToDateTime(LastProcessDate))
                    {
                        status = creditpro.CreditProcessSave(Convert.ToDateTime(LastProcessDate), Convert.ToDateTime(processdate));
                    }

                }
            }
            else//not Monthly cycle
            {
                int differtentdays = Convert.ToInt32(difference.TotalDays);
                int rotationaldays = Convert.ToInt32(DTCreditsettings.Rows[0]["Rotationdays"].ToString());

                int totalloop = differtentdays / rotationaldays;

                for (int i = 1; i <= totalloop; i++)
                {
                    DateTime restrictionperiod = Convert.ToDateTime(LastProcessDate).AddDays(restrictiondays - 1);
                    DateTime currentprocessdate = Convert.ToDateTime(LastProcessDate).AddDays(rotationaldays);
                    LastProcessDate = currentprocessdate.ToString();
                    if (currentprocessdate <= Convert.ToDateTime(processdate))
                    {
                        var CurrentmontANDlessthanEmployees = emplist.Where(u => u.DateOfJoining <= restrictionperiod).ToList();
                        bool savestatus;
                        for (int emp1 = 0; emp1 < CurrentmontANDlessthanEmployees.Count; emp1++)
                        {

                            //if (Convert.ToDateTime(processdate) > Convert.ToDateTime(LastProcessDate))
                            // {
                            savestatus = creditpro.CreditProcessing(CurrentmontANDlessthanEmployees[emp1].Id);

                            // }


                        }
                        status = creditpro.CreditProcessSave(Convert.ToDateTime(LastProcessDate), Convert.ToDateTime(processdate));
                    }
                }
            }
            string msg = status == true ? "success" : "Failure";
            return base.BuildJson(status, 200, msg, "");
        }

        #endregion

        #region "Assign Manager"
        public JsonResult SaveManagerAccess(List<ManagerAccess> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.ForEach(d =>
            {
                ManagerAccess mgr = new ManagerAccess();
                mgr.Id = d.Id;
                mgr.EmployeeId = mgr.EmployeeId;
                mgr.ManagerId = d.ManagerId;
                mgr.Prioritylevel = d.Prioritylevel;
                mgr.ApprovalMust = d.ApprovalMust;
                mgr.CancelRights = d.CancelRights;
                mgr.CompanyId = companyId;
                mgr.CreatedBy = userId;
                mgr.ModifiedBy = userId;
                mgr.Save();
            });

            return base.BuildJson(true, 200, "success", dataValue);


        }

        #endregion

        #region "Leave Post to Payroll"
        #endregion

        #region Monthly Leave Limit
        public class JsonMonthlyLeaveLimit
        {
            public Guid Id { get; set; }
            public Guid LeaVeTypeId { get; set; }

            public DateTime CreatedOn { get; set; }
            public DateTime ModifiedOn { get; set; }

            public int CompanyId { get; set; }
            public double MaxDays { get; set; }
            public double CrDays { get; set; }

        }
        public class JsonLeaveSettings
        {
            public int CompanyId { get; set; }
            public Guid FinyrId { get; set; }
            public string leaveparameter { get; set; }
            public string Holidayparameter { get; set; }
            public string Compoffparameter { get; set; }
            public string Weekoffparameter { get; set; }

            public string Weekoffentryvalid { get; set; }
            public string RpComp1 { get; set; }
            public string RpComp2 { get; set; }
            public string RpComp3 { get; set; }
            public string RpComp4 { get; set; }
            public string RpComp5 { get; set; }
            public string Minormaxparameter { get; set; }
            public string mindays { get; set; }
            public string maxmintimes { get; set; }
            public string minperiod { get; set; }
            public string mindeviation { get; set; }
            public string maxdays { get; set; }
            public string maxmaxtimes { get; set; }
            public string maxperiod { get; set; }
            public string maxdeviation { get; set; }
            public string Createdby { get; set; }
            public string leavecreditparameter { get; set; }
            public string encashmentparameter { get; set; }





        }
        public class JsonLeaveTypeSettings
        {
            public string LeaveTypeId { get; set; }
            public string LeaveTypeDesc { get; set; }
            public string LevopenReq { get; set; }
            public string Openingbal { get; set; }
            public string avalbal { get; set; }
            public string usedlev { get; set; }
            public string LevTypeActive { get; set; }
            public string LevEncashAvail { get; set; }
        }
        public class JsonLeaveEncashment
        {
            public string LeaveTypeId { get; set; }
            public string Encashcomponent { get; set; }
            public string EncashLimit { get; set; }
            public string Encashparameter { get; set; }
        }
        public class JsonLeaveConfiguration
        {
            public string LeaveTypeId { get; set; }
            public string DynamicComponentName { get; set; }
            public string DynamicComponentValue { get; set; }
            public string ConfigEffectiveDate { get; set; }
            public float MaxDayMonth { get; set; }
            public string AllowDevisionMonth { get; set; }
            public string LevopenReq { get; set; }
            public float overallMax { get; set; }
            public float carryLimit { get; set; }
            public string InvHoliday { get; set; }
            public string InvHolidaysubparameter { get; set; }
            public float MinatTime { get; set; }
            public float MaxatTime { get; set; }
            public string Compoffallow { get; set; }
            public string Openingbal { get; set; }
            public string avalbal { get; set; }
            public string usedlev { get; set; }
            public string mindays { get; set; }
            public string maxmintimes { get; set; }
            public string minperiod { get; set; }
            public string mindeviation { get; set; }
            public string maxdays { get; set; }
            public string maxmaxtimes { get; set; }
            public string maxperiod { get; set; }
            public string maxdeviation { get; set; }
            public string Isattachreq { get; set; }
            public string Attachreqmaxdays { get; set; }

        }
        public JsonResult GetMonthlyLeaveLimit()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            MonthlyLeaveLimit monthlyleavelimit = new MonthlyLeaveLimit();
            var data = monthlyleavelimit.GetMonthlyLeaveLimit(companyId);
            return base.BuildJson(true, 200, "success", data);
            //return new JsonResult { Data = data };
        }
        public JsonResult GetCreditdayssettings()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveCreditSettingsBO leavecreditsettings = new LeaveCreditSettingsBO();
            var data = leavecreditsettings.GetCreditLeavesettings(companyId);
            return base.BuildJson(true, 200, "success", data);
            //return new JsonResult { Data = data };
        }
        public JsonResult SaveMonthlyLeaveLimit(MonthlyLeaveLimit dataValue)
        {

            dataValue.Id = Guid.NewGuid();
            dataValue.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            dataValue.CreatedBy = Convert.ToInt32(Session["UserId"]);
            dataValue.ModifiedBy = Convert.ToInt32(Session["UserId"]);
            dataValue.CreatedOn = DateTime.Now;
            dataValue.ModifiedOn = DateTime.Now;
            bool isSuccess = dataValue.SaveMonthlyLeaveLimit();
            return base.BuildJson(true, 200, isSuccess == true ? "success" : "danger", isSuccess);
        }


        public JsonResult CreditLeaveSettings(LeaveCreditSettingsBO dataValue)
        {

            dataValue.Id = Guid.NewGuid();
            dataValue.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            dataValue.CreatedBy = Convert.ToInt32(Session["UserId"]);
            dataValue.ModifiedBy = Convert.ToInt32(Session["UserId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(Convert.ToInt32(Session["CompanyId"]));
            dataValue.FinyearId = DefaultFinancialYr.Id;
            dataValue.CreatedOn = DateTime.Now;
            dataValue.ModifiedOn = DateTime.Now;
            bool isSuccess = dataValue.SaveCreditLeaveSettings();
            return base.BuildJson(true, 200, isSuccess == true ? "success" : "danger", isSuccess);
        }




        public JsonResult DeleteMonthlyLeaveLimit(Guid id)
        {
            MonthlyLeaveLimit delMnthlyLevLmt = new MonthlyLeaveLimit();
            delMnthlyLevLmt.Id = id;
            delMnthlyLevLmt.ModifiedBy = Convert.ToInt32(Session["UserId"]);
            delMnthlyLevLmt.ModifiedOn = DateTime.Now;
            bool isSuccess = delMnthlyLevLmt.DeleteMonthlyLeaveLimit();
            return base.BuildJson(true, 200, isSuccess == true ? "success" : "danger", isSuccess);
        }


        public JsonResult DeleteCreditLeaveSetting(Guid id)
        {
            LeaveCreditSettingsBO delCRLevSettings = new LeaveCreditSettingsBO();
            delCRLevSettings.Id = id;
            delCRLevSettings.ModifiedBy = Convert.ToInt32(Session["UserId"]);
            delCRLevSettings.ModifiedOn = DateTime.Now;
            bool isSuccess = delCRLevSettings.DeleteMonthlyLeaveLimit();
            return base.BuildJson(true, 200, isSuccess == true ? "success" : "danger", isSuccess);
        }

        #endregion

        #region CompOff leave Tracking
        public JsonResult GetCompOffLeaveTracking(Guid EmployeeId, Guid FinYrId, string Type)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (FinYrId == Guid.Empty)
            {
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId, true);
                FinYrId = DefaultFinancialYr.Id;
            }
            Guid LoginEmpId = new Guid(Session["EmployeeId"].ToString());
            //  EmployeeId = EmployeeId == Guid.Empty ? new Guid(Session["EmployeeId"].ToString()) : EmployeeId;
            DataTable dtCompOffTracking = new DataTable();
            CompOffBO compOffTracking = new CompOffBO();
            dtCompOffTracking = compOffTracking.GetCompOffLeaveTracking(FinYrId, EmployeeId, LoginEmpId, Type);
            var result = jsonSerializedDtToString(dtCompOffTracking);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DownloadCompOffGainHistoryReport(Guid EmployeeId, Guid FinYrId, string Type)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (FinYrId == Guid.Empty)
            {
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId, true);
                FinYrId = DefaultFinancialYr.Id;
            }
            Guid LoginEmpId = new Guid(Session["EmployeeId"].ToString());
            DataTable dtCompOffTracking = new DataTable();
            CompOffBO compOffTracking = new CompOffBO();
            dtCompOffTracking = compOffTracking.GetCompOffLeaveTracking(FinYrId, EmployeeId, LoginEmpId, Type);
            dtCompOffTracking.Columns.Remove("Id"); dtCompOffTracking.Columns.Remove("Status");
            if (dtCompOffTracking.Rows.Count != 0)
            {
                GridView GridView1 = new GridView();
                GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell1 = new TableCell();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dtCompOffTracking;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView1.RenderControl(hw);
                //string[] arr = { "Pending", "Approved", "Rejected", "Cancelled", "Approved_Leave_Cancel_PendingList", "Approved_Leave_Cancel_AcceptedList", "Approved_Leave_Cancel_RejectedList" };
                string ExcelFilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + "CompOffGainTracking_Report.xls";
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

        #endregion
        public class Jsonmanagereligibility
        {
            public int Id { get; set; }

            public int RoleId { get; set; }

            public string FieldName { get; set; }

            public Guid FianaceYear { get; set; }

            public static Jsonmanagereligibility tojson(Jsonmanagereligibility attr)
            {
                return new Jsonmanagereligibility()
                {
                    Id = attr.Id,
                    RoleId = attr.RoleId,
                    FianaceYear = attr.FianaceYear,
                    FieldName = attr.FieldName
                };
            }
            public static Jsonmanagereligibility convertobject(Jsonmanagereligibility attr)
            {
                return new Jsonmanagereligibility()
                {
                    Id = attr.Id,
                    RoleId = attr.RoleId,
                    FianaceYear = attr.FianaceYear,
                    FieldName = attr.FieldName

                };
            }
        }


        public class JsonLeaveCreditProcess
        {
            public Guid CategoryId { get; set; }
            public Guid LeaveTypeId { get; set; }

            public DateTime ProcessDate { get; set; }
        }
    }
}
