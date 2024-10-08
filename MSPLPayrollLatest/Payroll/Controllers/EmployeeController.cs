﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PayrollBO;
using System.IO;
using TraceError;
using System.Data;
using System.Configuration;
using NotificationEngine;
using SystemWindowsFile;
using Payroll.Models.Common;
using Microsoft.Ajax.Utilities;
using System.Web;

namespace Payroll.Controllers
{

    public class EmployeeController : BaseController
    {
        User userobj = new User();
        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            if (object.ReferenceEquals(Session["CompanyId"], null))
                return RedirectToAction("Index", "Home");
            return View();
        }
        public ActionResult UserRegistration(Guid empid, string UserId)
        {
            FileLibrary objPwd = new FileLibrary();
            string useridpwd = objPwd.enfile("1");
            string useridss = objPwd.defile(useridpwd);
            DataTable dt = new DataTable();
            DataTable dtDBconn = new DataTable();
            dtDBconn = userobj.GetUserDBconnectionValues(Convert.ToInt32(UserId));
            ViewBag.TempTitle = "Registration";
            if (dtDBconn.Rows.Count > 0)
            {
                Session["DBString"] = dtDBconn.Rows[0]["DBString"].ToString();
                dt = userobj.GetTableValues(empid);
                if (dt.Rows.Count == 0)
                {
                    Employee employee = new Employee(empid);
                    Company compDetails = new Company(employee.CompanyId);
                    ViewBag.CompanyName = compDetails.CompanyName;

                    return View("UserRegistration", new UserMenu { empproperty = employee, emailapprvalproperty = null });
                }
                else
                {

                    return View("AlreadyRegistered");
                }
            }
            else
            {
                return View("Your url is wrong");
            }
        }
        public JsonResult GetManagerEmployees()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId, 0);
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult GetEmployees()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId);
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        //created by madhavan on 22/9/2023
        public JsonResult GetEmployeeExpense()
        {
            string EmployeeCode = Convert.ToString(Session["EmployeeId"]);
            List<EmpExpense> empExpense = new List<EmpExpense>();
            EmpExpense expense = new EmpExpense();
            empExpense = expense.GetExpenses(Guid.Empty, EmployeeCode);

            var data = empExpense.Select(e => new
            {
                ID = e.ID,
                EmployeeID = e.EmployeeID,
                CostCenter = e.CostCenter,
                CostOfExpense = e.CostOfExpense,
                CostCenterMgr = e.CostCenterMgr,
                DateOfExpense = e.DateOfExpense,
                DescriptExpense = e.DescriptExpense,
                Status = e.Status,
                SubmitDate = e.SubmitDate,
                PurposeForExpense = e.PurposeForExpense,
                CategeroyOfExpense = e.CategeroyOfExpense,
                Attachment = Url.Content(e.Attachment) // Map the image URL to an absolute URL
            });

            return base.BuildJson(true, 200, "Success", data);
        }
        [HttpPost]
        public JsonResult ExpenseEntry(JsonExpenseEntry expenseEntry)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid EmployeeId = new Guid(Convert.ToString(Session["EmployeeGUID"]));
            EmpExpense expense = new EmpExpense();
            expense.EmployeeID = expenseEntry.EmpID;
            expense.CostCenter = expenseEntry.CostCenter;
            expense.CategeroyOfExpense = expenseEntry.CategeroyOfExpense;
            expense.CostCenterMgr = expenseEntry.CostCenterMgr;
            expense.CostOfExpense = expenseEntry.CostOfExpense;
            expense.DateOfExpense = expenseEntry.DateOfExpense;
            expense.DescriptExpense = expenseEntry.DescriptExpense;
            expense.PurposeForExpense = expenseEntry.PurposeForExpense;
            expense.SubmitDate = expenseEntry.SubmitDate;
            expense.CreatedBy = Convert.ToString(this.Session["EmployeeCode"]);
            expense.CompanyId = companyId;
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var fileName = Guid.NewGuid() + Path.GetExtension(file);
                        string strFolderPath = "~/CompanyData/" + companyId + "/Employee/" + EmployeeId.ToString().ToUpper() + "/JoiningDocument2/" + expense.EmployeeID.ToString().ToUpper();
                        string strRelationPath = "~/CompanyData/" + companyId + "/Employee/" + EmployeeId.ToString().ToUpper() + "/JoiningDocument2/" + expense.EmployeeID.ToString().ToUpper() + "/" + fileName;
                        var Fpath = Path.Combine(Server.MapPath(strFolderPath));
                        var path = Path.Combine(Server.MapPath(strRelationPath));
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                        }
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        expense.Attachment = strRelationPath;
                        expense.Save();
                        return base.BuildJson(true, 200, "Save SuccessFuly", "");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                string exss = ex.Message;
                // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //  return Json("Upload failed");
                return base.BuildJson(false, 100, "There is some error while saving the file.", "");
            }

            return base.BuildJson(false, 100, "There is some error while saving the file.", "");



        }
        public JsonResult SaveAssignManager(JsonAssignMgr SaveAssignMgrValue)
        {
            bool isSaved;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            if (DefaultFinancialYr.Id == Guid.Empty)
            {
                return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            ExpenseAssignMgr AssMgr = new ExpenseAssignMgr();
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
        public JsonResult GetAssignManager(string EMPID)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                if (DefaultFinancialYr.Id == Guid.Empty)
                {
                    return BuildJsonResult(false, 200, "Please Set the Financial Year", null);
                }
                Guid employeeId = new Guid(EMPID);
                Guid id = Guid.Empty;
                int apprmust = 0;
                ExpenseAssignMgr AssignManager = new ExpenseAssignMgr(employeeId, companyId, DefaultFinancialYr.Id, id, apprmust);
                List<JsonAssignMgr> jsondata = new List<JsonAssignMgr>();
                AssignManager.ForEach(u => { jsondata.Add(JsonAssignMgr.toJson(u)); });
                return base.BuildJson(true, 200, "success", jsondata);
            }
            catch (Exception ex)
            {

                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "failure", ex.Message);
            }
        }
        public JsonResult GetAssignManage(string EMPLOYEEID)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid Firstlevel = new Guid(Convert.ToString(EMPLOYEEID));
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            ExpenseAssignMgr assignManager = new ExpenseAssignMgr(Firstlevel, companyId, 0, DefaultFinancialYr.Id);
            List<JsonAssignMgr> jsondata = new List<JsonAssignMgr>();
            assignManager.ForEach(u => { jsondata.Add(JsonAssignMgr.toJson(u)); });
            if (jsondata.Count != 0)
            {
                return base.BuildJson(true, 200, "success", jsondata);
            }
            else
            {
                return base.BuildJson(false, 200, "success", "");
            }

        }
        public JsonResult GetAssMgrSelectedData(Guid Id, Guid EmpId)
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
            ExpenseAssignMgr AssignManager = new ExpenseAssignMgr(EmpId, companyId, DefaultFinancialYr.Id, Id, apprmust);
            List<JsonAssignMgr> jsondata = new List<JsonAssignMgr>();
            AssignManager.ForEach(u => { jsondata.Add(JsonAssignMgr.toJson(u)); });
            if (jsondata.Count > 0)
            {
                return base.BuildJson(true, 200, "success", jsondata);
            }
            return base.BuildJson(true, 400, "failure", null);
        }
        public JsonResult DeleteAssignMgrData(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            ExpenseAssignMgr AssMgrML = new ExpenseAssignMgr();
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
        public JsonResult GetEmployeesWithName()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId);
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            for (int i = 0; i <= employeeList.Count - 1; i++)
            {
                jsonEmployee temp = new jsonEmployee();
                temp.EmpcodeName = employeeList[i].EmployeeCode + " - " + employeeList[i].FirstName + " " + employeeList[i].LastName;
                temp.empid = employeeList[i].Id;
                jsondata.Add(temp);
            }

            //employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult SaveEmployeeCode(List<jsonEmployeeCodeChange> attr)
        {
            int i = 0;
            attr.ForEach(f =>
            {
                Employee emp = new Employee(f.empid);
                emp.EmployeeCode = f.empCode;
                if (emp.Save())
                {
                    User use = new User(emp.Id);
                    if (use.Username == f.oldCode)
                    {
                        use.Username = f.empCode;
                        use.Save();
                    }

                }
                else
                {
                    i++;
                }

            });
            attr.ForEach(f =>
            {
                Employee emp = new Employee(f.empid);
                if (f.oldCode != f.empCode)
                {
                    bool res = emp.SavePast(f.empid, emp.CompanyId, f.oldCode);
                }
            });
            if (i > 0)
            {
                return base.BuildJson(true, 200, "failed", null);
            }

            return base.BuildJson(true, 200, "success", null);
        }




        public JsonResult GetFullManagerList()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId, 0, 0, 0);
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetFormData(string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            FormCommandList frmCmd = new FormCommandList(type);
            var dfrmCmd = frmCmd.Where(f => f.IsDefaultRequired == true).ToList();
            return base.BuildJson(true, 200, "success", dfrmCmd);

        }
        public JsonResult GetResignedEmployees()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //int userId = Convert.ToInt32(Session["UserId"]);
            //Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            //EmployeeList employeeList = new EmployeeList(companyId, userId, employeeId, 0, 0, 0);
            //employeeList.Initialize();
            //List<jsonEmployee> jsondata = new List<jsonEmployee>();
            ////employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });


            //employeeList.ForEach(u =>
            //{
            //    if (u.Status == 0)
            //    {
            //        jsondata.Add(jsonEmployee.tojson(u));
            //    }
            //});


            //return base.BuildJson(true, 200, "success", jsondata);
            EmployeeRole objER = new EmployeeRole();
            List<EmployeeRole> LstF = new List<EmployeeRole>();
            List<EmployeeRole> Lst = new List<EmployeeRole>();
            //LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            //ManagerEligiblityList ObjMGRList = new ManagerEligiblityList(companyId, DefaultFinancialYr.Id);
            Lst = objER.role(companyId);
            //Lst.ForEach(j =>
            //{
            //    ObjMGRList.ForEach(k =>
            //    {
            //        if (j.RoleId == k.RoleId)
            //        {

            //        }
            //    });
            //});
            //Lst.ForEach(u =>
            //{
            //    if (u.EmpStatus == 0)
            //    {
            //        LstF.Add(u);
            //    }
            //});
            return base.BuildJson(true, 200, "success", Lst);
        }
        public JsonResult GetUNResignedEmployees()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeRole objER = new EmployeeRole();
            List<EmployeeRole> LstF = new List<EmployeeRole>();
            List<EmployeeRole> Lst = new List<EmployeeRole>();
            Lst = objER.role(companyId);
            //Lst.ForEach(u =>
            //{
            //    if (u.EmpStatus == 0)
            //    {
            //        LstF.Add(u);
            //    }
            //});
            return base.BuildJson(true, 200, "success", Lst);
        }

        public JsonResult GetEmpdetails(string empId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Employee employee = new Employee(empId);
            employee.Initialize();
            jsonEmployee jsondata = jsonEmployee.tojson(employee);
            return base.BuildJson(true, 200, "success", jsondata);
        }


        public JsonResult GetPayrolldashboarddatas(string Type, int Monthvalue)
        {
            string monthname = string.Empty;
            int MNo = 0;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            DataTable DTList = new DataTable();
            if (Type == "BirthdayCurrentMonth")
            {
                DTList = Employee.CurrentMonthDashboard(companyId, Type);
                MNo = System.DateTime.Now.Month;
            }
            else if (Type == "BirthdayDynamicMonth")
            {
                DTList = Employee.DynamicMonthDashboard(companyId, Type, Monthvalue);
                MNo = Monthvalue;
            }
            else if (Type == "ServiceCurrentMonth")
            {
                DTList = Employee.CurrentMonthDashboard(companyId, Type);
                MNo = System.DateTime.Now.Month;
            }
            else if (Type == "ServiceDynamicMonth")
            {
                DTList = Employee.DynamicMonthDashboard(companyId, Type, Monthvalue);
                MNo = Monthvalue;
            }
            else if (Type == "TodayBirthday")
            {
                DTList = Employee.DynamicMonthDashboard(companyId, Type, Monthvalue);
                MNo = System.DateTime.Now.Month;
            }
            else if (Type == "TodayService")
            {

            }
            else
            {

            }
            List<jsonDashboardPayroll> jsondata = new List<jsonDashboardPayroll>();

            for (int i = 0; i <= DTList.Rows.Count - 1; i++)
            {
                jsonDashboardPayroll templist = new jsonDashboardPayroll();
                templist.Id = DTList.Rows[i]["id"].ToString();
                templist.empName = DTList.Rows[i]["Employeename"].ToString();
                templist.Date = Convert.ToDateTime(DTList.Rows[i]["Date"].ToString());
                templist.Imgcode = DTList.Rows[i]["EmployeeImage"].ToString().Trim();
                templist.Displayname = DTList.Rows[i]["Display"].ToString();
                templist.Monthnumb = Convert.ToInt32(DTList.Rows[i]["monthNumber"]);
                templist.currentyear = Convert.ToInt32(DTList.Rows[i]["yearnumber"]);
                templist.NotedYear = Convert.ToInt32(DTList.Rows[i]["notedyear"]);
                templist.ServiceyrORAge = templist.currentyear - templist.NotedYear;
                if (templist.ServiceyrORAge != 0)
                {
                    jsondata.Add(templist);
                }

            }
            monthname = Convert.ToString((MonthEnum)MNo);
            return base.BuildJson(true, MNo, monthname.ToUpper(), jsondata);
        }

        public JsonResult SendGreetingsmail(Guid empId, string Event, string messageGreetingContent)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            Guid UserId = new Guid(Convert.ToString(Session["EmployeeId"]));
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Company com = new Company(companyId);
            Employee employee = new Employee(companyId, empId);
            Employee LoginUser = new Employee(companyId, UserId);

            string FromMail = LoginUser.Email;
            string ToMail = employee.Email;
            string UserName = LoginUser.FirstName + "  " + LoginUser.LastName + " ( " + LoginUser.EmployeeCode + " ) ";
            string EmployeeName = employee.FirstName + "  " + employee.LastName + " ( " + employee.EmployeeCode + " ) ";
            if (ToMail == "")
            {
                return base.BuildJson(false, 200, EmployeeName + " Dont have EmailId ", "");

            }

            string Subject = string.Empty;
            string message = string.Empty;
            string Authority = Request.Url.GetLeftPart(UriPartial.Authority) + "/";
            string baseUrl = Authority + "assets/";
            BirthdayGreeting messagecontent = new BirthdayGreeting();
            if (Event == "Birthday")
            {
                //Commented because cant able to use the fa fa class in dynamically passing html code.
                //Subject = "Happy Birthday " + EmployeeName + "<span class=\"fa fa-birthday - cake\" style=\"float:left; color: red; font - size:30px\"></span>";
                Subject = "Happy Birthday " + EmployeeName;
                message = messagecontent.Birthdaygreetings(baseUrl, messageGreetingContent, EmployeeName);
            }
            else
            {

            }
            MailConfig mailConfig4 = new MailConfig(companyId);
            string[] StrCCMail = mailConfig4.CCMail.Split(',');
            PayRoleMail payrolemail = new PayRoleMail(ToMail, StrCCMail, Subject, message, "");
            string mailstat = null;
            bool status = false;
            status = payrolemail.SendGreetingmail(mailConfig4.IPAddress, mailConfig4.PortNo, mailConfig4.FromEmail, mailConfig4.MailPassword, mailConfig4.EnableSSL);
            if (status)
            {
                mailstat = "Wishes Sended succesfully";
                return base.BuildJson(true, 200, mailstat, "");
            }
            else
            {
                mailstat = "Wishes send failed";
                return base.BuildJson(false, 200, mailstat, "");
            }

        }




        public JsonResult GetEmployeeData(Guid empId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Company com = new Company(companyId);
            Employee employee = new Employee(companyId, empId);
            employee.Usercreationtype = com.Usercreations;
            if (employee.EmployeeImage == "[object Object]")
            {
                employee.EmployeeImage = null;
            }
            employee.Initialize();
            jsonEmployee jsondata = jsonEmployee.tojson(employee);
            return base.BuildJson(true, 200, "success", jsondata);
        }
        //---------Created by Keerthika on 08/06/2017

        public JsonResult GetSelectedEmployeeData()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            Employee employee = new Employee(employeeId);

            employee.Initialize();


            Emp_Personal empPersonal = new Emp_Personal(employeeId);
            EmployeeAcademicList employeeAcademic = new EmployeeAcademicList(employeeId);
            EmployeeAddressList employeeAddress = new EmployeeAddressList(employeeId);
            EmployeeBenefitComponentList employeeComponent = new EmployeeBenefitComponentList(employeeId);
            EmployeeFamilyList empFamilyList = new EmployeeFamilyList(employeeId);
            EmployeeEmegencyContactList empEmergencyContact = new EmployeeEmegencyContactList(employeeId);
            EmployeeEmployeementList EmpEmployeement = new EmployeeEmployeementList(employeeId);
            EmployeeNomineeList EmpNominee = new EmployeeNomineeList(employeeId);
            EmployeeLanguageKnownList EmpLangKnow = new EmployeeLanguageKnownList(employeeId);
            EmployeeHrComponentList EmpHrComponent = new EmployeeHrComponentList(employeeId);
            EmployeeTrainingList EmpTraining = new EmployeeTrainingList(employeeId);
            //jsonEmpbenefitComponent jsondata1 = jsonEmpbenefitComponent.tojson(employeeComponent,);
            //jsonEmpEmegencyContact jsondata2= jsonEmpEmegencyContact.tojson()
            //jsonEmployee jsondata = jsonEmployee.tojson(employee);

            return base.BuildJson(true, 200, "success", new
            {
                EmpPersonal = empPersonal,
                EmployeeDetails = employee,
                EmployeeAcademic = employeeAcademic,
                EmployeeAddress = employeeAddress,
                EmployeeComponent = employeeComponent,
                EmpFamilyList = empFamilyList,
                EmpEmergencyComtact = empEmergencyContact,
                EmpEmployeement = EmpEmployeement,
                EmpNominee = EmpNominee,
                EmpLangKnow = EmpLangKnow,
                EmpHrComponent = EmpHrComponent,
                EmpTraining = EmpTraining
            });
        }
        public JsonResult GetAppSetting(Guid categoryId, string setting)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Emp_CodeSetting codeSet = new Emp_CodeSetting(categoryId, companyId);
            if (!object.ReferenceEquals(codeSet.SNumber, null))
            {
                if (string.Equals(setting, "Salary"))
                {
                    Category cat = new Category(categoryId, companyId);
                    if (object.ReferenceEquals(cat.PFEdliChargeProcess, null))
                    {
                        return base.BuildJson(false, 200, "Please update the PF Setting for this category", null);
                    }


                }
                return base.BuildJson(true, 200, string.Empty, null);
            }
            else
            {
                return base.BuildJson(false, 200, "Please update the application Setting for this category", null);
            }
        }
        public JsonResult GetSelectiveEmployees(string Condition)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid EmployeeID = new Guid(Convert.ToString(Session["EmployeeGUID"]));
            Guid EmployeeRef = new Guid(Convert.ToString(Session["EmployeeId"]));
            string[] WCondition;
            WCondition = Condition.Split('.');
            EmployeeList employeeList = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            if (WCondition[0].ToString() == "empDOJ")
            {
                int curyymm = Convert.ToInt32(WCondition[1]) * 100 + Convert.ToInt32(WCondition[2]);
                DateTime CurrPayrollmonth = new DateTime(Convert.ToInt32(WCondition[1]), Convert.ToInt32(WCondition[2]), DateTime.DaysInMonth(Convert.ToInt32(WCondition[1]), Convert.ToInt32(WCondition[2])));
                var seletedEmpl = employeeList.Where(u => u.DateOfJoining <= CurrPayrollmonth &&
                (u.SeparationDate == DateTime.MinValue || u.SeparationDate == null || curyymm <= Convert.ToInt32(u.SeparationDate.Year) * 100 + Convert.ToInt32(u.SeparationDate.Month))).ToList();
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "Category")
            {
                Guid selectedCatagoryId = new Guid(WCondition[1]);
                var seletedEmpl = employeeList.Where(u => u.CategoryId == selectedCatagoryId && u.Status == 1).ToList();
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }

            else if (WCondition[0].ToString() == "Category-DeclartionEntry")
            {
                Guid selectedCatagoryId = new Guid(WCondition[1]);
                int prcyymm = (Convert.ToInt32(WCondition[3]) * 100) + Convert.ToInt32(WCondition[2]);
                var seletedEmpl = employeeList.Where(u => u.CategoryId == selectedCatagoryId &&
                 //(u.SeparationDate.Month >= (Convert.ToInt32(WCondition[2])) && u.SeparationDate.Year >= (Convert.ToInt32(WCondition[3])
                 (((u.SeparationDate.Year * 100) + u.SeparationDate.Month) >= prcyymm)
                || u.SeparationDate == DateTime.MinValue && u.ReleaseDate == DateTime.MinValue).ToList();

                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "Category-Release")
            {
                Guid selectedCatagoryId = new Guid(WCondition[1]);
                var seletedEmpl = employeeList.Where(u => u.CategoryId == selectedCatagoryId && u.SeparationDate != DateTime.MinValue && u.ReleaseDate == DateTime.MinValue).ToList();
                var seletedEmplFilter = employeeList.Where(u => u.CategoryId == selectedCatagoryId && u.SeparationDate != DateTime.MinValue && u.ReleaseDate == DateTime.MinValue).ToList();
                seletedEmplFilter.ForEach(f =>
                {
                    FullFinalSettlement fullFinalSettlement = new FullFinalSettlement(Guid.Empty, f.Id);
                    if (fullFinalSettlement.Id != Guid.Empty)
                    {
                        seletedEmpl.RemoveAll(r => r.Id == f.Id);
                    }
                });



                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "Category-FF")
            {
                Guid selectedCatagoryId = new Guid(WCondition[1]);
                var seletedEmpl = employeeList.Where(u => u.CategoryId == selectedCatagoryId && u.SeparationDate.Month == (Convert.ToInt32(WCondition[2])) && u.SeparationDate.Year == (Convert.ToInt32(WCondition[3])) && u.SeparationDate != DateTime.MinValue && u.ReleaseDate == DateTime.MinValue && u.TypeOfSeparation != Convert.ToInt16(SeparationTypeEnum.OTHER_REASON).ToString()).ToList();
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "Ballw")
            {
                var seletedEmpl = employeeList.Where(u => u.Status == 1).ToList();
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "Savg")
            {
                var seletedEmpl = new EmployeeList();
                if (EmployeeRef != Guid.Empty)
                {
                    seletedEmpl.AddRange(employeeList.Where(e => e.Id == EmployeeRef));
                }
                else
                {
                    seletedEmpl.AddRange(employeeList.Where(u => u.Status == 1).ToList());
                }
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else if (WCondition[0].ToString() == "SavgView")
            {
                var seletedEmpl = new EmployeeList();
                seletedEmpl.AddRange(employeeList.Where(u => u.Status == 1).ToList());
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
            else
            {
                employeeList.Initialize();
                List<jsonEmployee> jsondata = new List<jsonEmployee>();
                employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });
                return base.BuildJson(true, 200, "success", new { employeeID = EmployeeID, Jsondata = jsondata });
            }
        }






        public JsonResult GetEmployeeCodeAutonumber()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;;
            bool EmpPreffixConfigchk = false;
            bool AutoNumberConfigchk = false;
            bool NewEmpCode = false;
            List<string> retList = new List<string>();
            SettingList settinglist = new SettingList(companyId);
            Setting setting = settinglist.Where(u => u.Name.ToUpper() == "EMPCODEAUTONUMBER").FirstOrDefault();
            if (!object.ReferenceEquals(setting.Id, null))
            {
                SettingDefinitionList settingDefinitionlist = new SettingDefinitionList(setting.Id, companyId);
                SettingValueList settingvalueList = new SettingValueList(setting.Id);
                settingDefinitionlist.ForEach(u =>
                {
                    if (u.Name.ToUpper() == "PREFIXEMPCODE")
                    {
                        var setval = settingvalueList.Where(p => p.SettingDefinitionId == u.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(setval, null))
                        {
                            EmpPreffixConfigchk = true;
                        }
                    }



                    if (u.Name.ToUpper() == "STARTINGNUMBER")
                    {
                        var setval = settingvalueList.Where(p => p.SettingDefinitionId == u.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(setval, null))
                        {
                            AutoNumberConfigchk = true;
                        }
                    }
                });

                NewEmpCode = (EmpPreffixConfigchk == true && AutoNumberConfigchk == true) ? true : false;
            }
            else
            {
                NewEmpCode = false;
            }
            return base.BuildJson(true, 200, "success", NewEmpCode);
        }

        public JsonResult GetSeparatedEmployees()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeList employeeList = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            var SeparatedemployeeList = employeeList.Where(u => u.Status == 0 && !String.IsNullOrEmpty(Convert.ToString(u.SeparationDate))).ToList();
            List<jsonDropdownEmployee> jsondata = new List<jsonDropdownEmployee>();
            SeparatedemployeeList.ForEach(u => { jsondata.Add(jsonDropdownEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetResignationEmployees()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmpResignation emplist = new EmpResignation();
            emplist.CompanyId = companyId;
            var empRegList = emplist.EmpResignationList();
            empRegList = empRegList.Where(u => u.Isdeleted == true).ToList();
            List<jsonDropdownEmployee> jsondata = new List<jsonDropdownEmployee>();
            empRegList.ForEach(u => { jsondata.Add(jsonDropdownEmployee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult DeleteResignationData(Guid id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmpResignation empData = new EmpResignation();
            empData.Id = id;
            var empRegList = empData.Delete();
            return base.BuildJson(true, 200, "Data Deleted successfully", id);
        }
        public JsonResult GetFamilys(Guid employeeId)
        {
            EmployeeFamilyList familylist = new EmployeeFamilyList(employeeId);
            List<jsonEmpFamily> jsondata = new List<jsonEmpFamily>();
            familylist.ForEach(u => { jsondata.Add(jsonEmpFamily.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpAcademics(Guid employeeId)
        {
            EmployeeAcademicList academiclist = new EmployeeAcademicList(employeeId);
            List<jsonEmpAcademic> jsondata = new List<jsonEmpAcademic>();
            academiclist.ForEach(u => { jsondata.Add(jsonEmpAcademic.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        //------
        //public JsonResult GetEmpAcademics()
        //{
        //    Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
        //    EmployeeAcademicList academiclist = new EmployeeAcademicList(employeeid);
        //    List<jsonEmpAcademic> jsondata = new List<jsonEmpAcademic>();
        //    academiclist.ForEach(u => { jsondata.Add(jsonEmpAcademic.tojson(u)); });
        //    return base.BuildJson(true, 200, "success", jsondata);
        //}

        public JsonResult GetEmpEmployeement(Guid employeeId)
        {
            EmployeeEmployeementList employeementlist = new EmployeeEmployeementList(employeeId);
            List<jsonEmpEmployeeMent> jsondata = new List<jsonEmpEmployeeMent>();
            employeementlist.ForEach(u => { jsondata.Add(jsonEmpEmployeeMent.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpTraining(Guid employeeId)
        {
            EmployeeTrainingList employeeTrainingList = new EmployeeTrainingList(employeeId);
            List<jsonEmpTraining> jsondata = new List<jsonEmpTraining>();
            employeeTrainingList.ForEach(u => { jsondata.Add(jsonEmpTraining.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpNominees(Guid employeeId)
        {
            EmployeeNomineeList employeeNomineeList = new EmployeeNomineeList(employeeId);
            List<jsonEmpNominee> jsondata = new List<jsonEmpNominee>();
            employeeNomineeList.ForEach(u => { jsondata.Add(jsonEmpNominee.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpBenefitcomponents(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeBenefitComponentList empBenefitComponentList = new EmployeeBenefitComponentList(employeeId);
            List<keyValueItem> benefitComponet = new List<keyValueItem>();
            EntityModel entitModel = new EntityModel(ComValue.SalaryTable, companyId);
            entitModel.EntityAttributeModelList.ForEach(p =>
            {
                if (p.AttributeModel.IsReimbursement)
                {
                    benefitComponet.Add(new keyValueItem() { Id = p.AttributeModel.Id, Name = p.AttributeModel.Name, DisplayName = p.AttributeModel.DisplayAs });
                }
            });
            List<jsonEmpbenefitComponent> jsondata = new List<jsonEmpbenefitComponent>();
            empBenefitComponentList.ForEach(u => { jsondata.Add(jsonEmpbenefitComponent.tojson(u, benefitComponet)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        //public JsonResult GetActiveEmployeeDetails()
        //{
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    DataTable dt = PayrollBO.Employee.GetActiveEmployee(companyId);
        //    List<jsonActiveEmployee> data = new List<jsonActiveEmployee>();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col]);
        //        }
        //        rows.Add(row);
        //    }            
        //        return base.BuildJson(true, 200, "success", rows);
        //}
        public JsonResult GetEmpEmergencyContacts(Guid employeeId)
        {
            EmployeeEmegencyContactList empEmegencyContactList = new EmployeeEmegencyContactList(employeeId);
            List<jsonEmpEmegencyContact> jsondata = new List<jsonEmpEmegencyContact>();
            empEmegencyContactList.ForEach(u => { jsondata.Add(jsonEmpEmegencyContact.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpLanguageKnowns(Guid employeeId)
        {
            EmployeeLanguageKnownList empLanguageKnownList = new EmployeeLanguageKnownList(employeeId);
            List<jsonEmpLangKnown> jsondata = new List<jsonEmpLangKnown>();
            empLanguageKnownList.ForEach(u => { jsondata.Add(jsonEmpLangKnown.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult GetEmployeeContractDetail(Guid employeeId)
        {
            Guid ContrId = Guid.Empty;
            EmployeeContractDetail objContrDet = new EmployeeContractDetail();
            List<EmployeeContractDetail> objContrLst = new List<EmployeeContractDetail>();
            objContrLst = objContrDet.GetEmployeeContractDetail(employeeId);
            List<jsonEmpContractDetails> objJsonRslt = new List<jsonEmpContractDetails>();
            objContrLst.ForEach(k => { objJsonRslt.Add(jsonEmpContractDetails.tojson(k)); });
            return base.BuildJson(true, 200, "Success", objJsonRslt);
        }


        //Created by Rajesh for Salary slip download on 05th Dec 2017       
        public JsonResult GetSalaryList1()
        {

            return base.BuildJson(true, 200, "success", null);
            #region
            //if (table.Rows.Count > 0)
            //{

            //    //table.DefaultView.Sort = "datefield desc";                   
            //    DataView view = new DataView(table);
            //    view.Sort = ("datefield asc");
            //    DataTable dtsal = view.ToTable();
            //    DataTable distinctyr = view.ToTable(true, "Year");
            //    TreeNode parntyr = new TreeNode();
            //    parntyr.Text = "Please expand this to download your payslip";
            //    parntyr.NavigateUrl = "#";
            //    parntyr.Expanded = false;
            //    foreach (DataRow row in distinctyr.Rows)
            //    {
            //        TreeNode parsal = new TreeNode
            //        {
            //            Text = row["Year"].ToString(),
            //            NavigateUrl = "#",
            //            Expanded = false
            //        };

            //        DataTable dtmnth = dtsal.Select("Year='" + row["Year"].ToString() + "'").CopyToDataTable();

            //        foreach (DataRow mrow in dtmnth.Rows)
            //        {
            //            TreeNode parsal1 = new TreeNode
            //            {
            //                Text = mrow["Month"].ToString(),
            //                Value = mrow["ID"].ToString(),
            //                NavigateUrl = "~/pip/payslipview.aspx?&ID=" + mrow["ID"].ToString()

            //            };

            //            parsal.ChildNodes.Add(parsal1);
            //        }
            //        parntyr.ChildNodes.Add(parsal);
            //    }
            //    grdSalary.Nodes.Add(parntyr);
            //    //grdSalary1.DataSource = table;
            //    grdSalary.DataBind();
            //    this.pnlSlip.Visible = true;
            //}
            //else
            //{
            //    this.lblNotAvail.Text = "Payslip not found";
            //    this.pnlSlip.Visible = false;
            //}
            #endregion

            //}
            //else
            //{
            //    this.lblNotAvail.Text = "Payslip doesn't exists";
            //    this.pnlSlip.Visible = false;
            //}
            // Ultimately to get a string list.
        }
        private string decrypt(string str)
        {
            string _result = string.Empty;
            char[] temp = str.ToCharArray();
            foreach (var _singleChar in temp)
            {
                var i = (int)_singleChar;
                i = i + 2;
                _result += (char)i;
            }
            return _result;
        }

        public JsonResult GetEmpJoiningDoc(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeJoingDocumentList empJoiningdocumentlist = new EmployeeJoingDocumentList(employeeId);
            JoiningDocumentList joinDocuments = new JoiningDocumentList(companyId);
            List<jsonEmployeeJoiningDoc> ret = new List<jsonEmployeeJoiningDoc>();
            joinDocuments.ForEach(u =>
            {
                ret.Add(jsonEmployeeJoiningDoc.tojson(u, empJoiningdocumentlist));

            });
            return base.BuildJson(true, 200, "success", ret);
        }
        public JsonResult GetEmpHRComponentData(Guid id, Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeHrComponent empHrComponent = new EmployeeHrComponent(employeeId, id);
            HRComponentList hrComponentList = new HRComponentList(companyId);
            jsonEmpHrComponent jsondata = jsonEmpHrComponent.tojson(empHrComponent, hrComponentList);
            return base.BuildJson(true, 200, "success", jsondata);

        }

        public JsonResult GetEmpHrComponent(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeHrComponentList employeementlist = new EmployeeHrComponentList(employeeId);
            List<jsonEmpHrComponent> jsondata = new List<jsonEmpHrComponent>();
            HRComponentList hrComponentList = new HRComponentList(companyId);
            employeementlist.ForEach(u => { jsondata.Add(jsonEmpHrComponent.tojson(u, hrComponentList)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult EmailCheck(jsonEmployee dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeList employeeList = new EmployeeList(dataValue.empEmail.ToString(), dataValue.empid);

            if (employeeList.Count == 0)
            {
                return base.BuildJson(true, 200, string.Empty, true);
            }
            else
            {
                return base.BuildJson(false, 200, "Email ID is already Exist.", false);
            }

            //employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });

        }

        //public JsonResult WrktodateCheck(jsonEmpEmployeeMent dataValue)
        //{
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    var employeeId = dataValue.empEmployeementEmployeeId;
        //    EmployeeList employeeList = new EmployeeList(companyId,0, employeeId);
        //    var wrkTo = employeeList.Where(d => d.DateOfJoining > Convert.ToDateTime(dataValue.empEmployeementWorkTo)).ToList();
        //    if (wrkTo.Count == 0)
        //    {
        //        return base.BuildJson(true, 200, string.Empty, true);
        //    }
        //    else
        //    {
        //        return base.BuildJson(false, 200, "S.", false);
        //    }

        //    //employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });

        //}

        public JsonResult UploadFile()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var fileName = "";
            string strRelationPath = "";
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
                        strRelationPath = "~/EmpImage/" + companyId + "/" + tempId + "/" + fileName;
                        var path = Path.Combine(Server.MapPath(strRelationPath));
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
                return base.BuildJson(true, 200, "User Profile image has been saved.", strRelationPath);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the file.", strRelationPath);
            }
        }

        public JsonResult SaveEmployee(jsonEmployee dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["UserId"]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            Employee employee = new Employee();
            employee = jsonEmployee.convertobject(dataValue);
            employee.CompanyId = companyId;
            employee.CreatedBy = userid;
            employee.ModifiedBy = userid;
            Company com = new Company(companyId);

            if (com.Usercreations == true)
            {
                if (dataValue.empEmail == "")
                {
                    return base.BuildJson(false, 200, "Please Enter the Email", false);
                }
            }
            employee.DBConnectionId = Convert.ToInt32(Session["DBConnectionId"]);
            if (employee.Save())
            {
                User user = new User(employee.Id);
                if (user.Username != null)
                {
                    user.ProfileImage = employee.EmployeeImage.Trim();
                    user.FirstName = employee.FirstName;
                    user.LastName = employee.LastName;
                    user.Email = employee.Email;
                    user.Phone = employee.Phone;
                    user.Save();
                }

                if (com.Usercreations == true)
                {
                    DataTable DTemployeemailchecking = Employee.checkemailsend(employee.Id, "CHECK");
                    string mailstatus = DTemployeemailchecking.Rows[0]["IsMailSend"].ToString();
                    if (mailstatus == "False") //UnCommand by maddy
                    {
                        bool stat = SendMailToEmpSave(employee.Id);
                        if (stat == true)
                        {
                            Employee.checkemailsend(employee.Id, "UPDATE");
                        }

                    }
                }
                Employee emp = new Employee(employee.Id);
                jsonEmployee data = jsonEmployee.tojson(emp);
                //if (dataValue.empid == Guid.Empty)
                //{
                //    Emp_Personal emp_Personal = new Emp_Personal();
                //    emp_Personal.EmployeeId = employee.Id;
                //    emp_Personal.Save();
                //}

                return base.BuildJson(true, 200, "Data saved successfully", data);
            }
            else
            {
                jsonEmployee data = jsonEmployee.tojson(employee);
                return base.BuildJson(false, 100, "There is some error while saving the data.", data);
            }



        }
        // created by keerthika  
        public Boolean SendMailToEmpSave(Guid employeeId)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Employee employee = new Employee(employeeId);
            bool status = false;
            try
            {

                Guid empid = employee.Id;
                string empMail = employee.Email;
                if (!string.IsNullOrEmpty(empMail))
                {
                    //Session["EmployeeName"] = employee.FirstName + " " + employee.LastName;
                    Company company = new Company(companyId, userid);
                    string activationUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + "/Employee/UserRegistration?empid=" + empid + "&UserId=" + employee.DBConnectionId;

                    //string message = "<div style='border: 1px solid green;background-color: lightgrey; padding: 25px;margin: 25px;'>";
                    //message = message + "<p>Hello " + employee.FirstName + " " + employee.LastName + ",<br/> Welcome to " + company.CompanyName + ". Register here to see your Details. <a href='" + activationUrl + "'>Click Here to Register your acount</a> <div>";
                    string message = "<p>Hello " + employee.FirstName + ",<br/><br/>We are glad to inform you that we have introduced a new employee portal. The new portal requires you to make an one time registration at your end. You are requested to follow the below link to proceed.<br/><br/> Link :  <a href='" + activationUrl + "'>Click Here to Register your acount</a> <br/><br/>You will have to login to the portal " + ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + " <br/>Your user ID is your official email ID which is stated in the registration link given above. <br/><br/>Regards,<br/>Payroll Team";
                    string subject = "Greetings From " + company.CompanyName;

                    PayRoleMail payrolemail = new PayRoleMail(empMail, subject, message);

                    status = payrolemail.Send();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }
        public JsonResult SaveEmpAddress(jsonEmpAddress[] dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            foreach (jsonEmpAddress data in dataValue)
            {
                if (data.empAddEmployeeId == Guid.Empty)
                {
                    isSaved = false;
                    break;
                }
                EmployeeAddress empAddress = jsonEmpAddress.convertobject(data);
                empAddress.CreatedBy = userId;
                empAddress.IsDeleted = false;
                isSaved = empAddress.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult SaveEmpBank(jsonEmpBank dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);


            if (dataValue.empBankEmployeeId == Guid.Empty)
            {
                isSaved = false;

            }
            Emp_Bank empBank = jsonEmpBank.convertobject(dataValue);
            empBank.CreatedBy = userId.ToString();
            empBank.ModifiedBy = userId.ToString();
            empBank.IsActive = false;
            empBank.EmployeeId = dataValue.empBankEmployeeId;
            isSaved = empBank.Save();

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpFamily(jsonEmpFamily dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmployeeFamily employeeAddress = jsonEmpFamily.convertobject(dataValue);

            if (dataValue.empFamilyEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                employeeAddress.CreatedBy = userId;
                employeeAddress.IsDeleted = false;
                isSaved = employeeAddress.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpAcademic(jsonEmpAcademic dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmployeeAcademic employeeAcademic = jsonEmpAcademic.convertobject(dataValue);
            if (dataValue.empAcademicEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                employeeAcademic.CreatedBy = userId;
                employeeAcademic.IsDeleted = false;
                isSaved = employeeAcademic.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpBenfitComponent(jsonEmpbenefitComponent dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeBenefitComponent empBenefitComponent = jsonEmpbenefitComponent.convertobject(dataValue);
            if (dataValue.empBenefitCompEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                empBenefitComponent.CreatedBy = userId;
                empBenefitComponent.IsDeleted = false;
                isSaved = empBenefitComponent.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpEmergencyContact(jsonEmpEmegencyContact dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeEmegencyContact empEmegencyContact = jsonEmpEmegencyContact.convertobject(dataValue);
            if (dataValue.empEmrgContEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                empEmegencyContact.CreatedBy = userId;
                empEmegencyContact.IsDeleted = false;
                isSaved = empEmegencyContact.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpEmployeement(jsonEmpEmployeeMent dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmployeeEmployeement employeeEmployeement = jsonEmpEmployeeMent.convertobject(dataValue);
            if (dataValue.empEmployeementEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                employeeEmployeement.CreatedBy = userId;
                employeeEmployeement.IsDeleted = false;
                isSaved = employeeEmployeement.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpJoiningDoc()//jsonEmployeeJoiningDoc datavalue
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            jsonEmployeeJoiningDoc datavalue = new jsonEmployeeJoiningDoc();
            if (Request.Form["EmpJoiningDocId"] == null)
                return base.BuildJson(false, 100, "The Form should not be empty.", datavalue);
            if (Request.Form["employeeId"] == null)
                return base.BuildJson(false, 100, "The Form should not be empty.", datavalue);
            if (Request.Form["joingDocumentId"] == null)
                return base.BuildJson(false, 100, "The Form should not be empty.", datavalue);
            Guid empId;
            if (!Guid.TryParse(Request.Form["employeeId"], out empId))
            {
                return base.BuildJson(false, 100, "The Form should not be empty.", datavalue);
            }
            Guid joinDocId;
            if (!Guid.TryParse(Request.Form["joingDocumentId"], out joinDocId))
            {
                return base.BuildJson(false, 100, "The Form should not be empty.", datavalue);
            }
            Guid EmpjoinDocId;
            if (Guid.TryParse(Request.Form["EmpJoiningDocId"], out EmpjoinDocId))
            {
                datavalue.id = EmpjoinDocId;
            }
            datavalue.employeeId = empId;
            datavalue.joingDocumentId = joinDocId;

            string strExistPath = string.Empty;
            if (datavalue.employeeId != Guid.Empty && datavalue.joingDocumentId != Guid.Empty)
            {
                EmployeeJoingDocument empJoinDoc = new EmployeeJoingDocument();// datavalue.employeeId, datavalue.id);
                empJoinDoc.JoiningDocumentId = joinDocId;
                empJoinDoc.EmployeeId = empId;
                empJoinDoc.ModifiedBy = userId;
                empJoinDoc.CreatedBy = userId;
                empJoinDoc.IsDeleted = false;
                if (empJoinDoc.JoiningDocumentId != Guid.Empty)//already there
                {
                    strExistPath = empJoinDoc.FilePath;
                    datavalue.filePath = empJoinDoc.FilePath;
                }
                else
                {
                    empJoinDoc = jsonEmployeeJoiningDoc.convertObject(datavalue);
                }
                if (empJoinDoc.Save())
                {
                    datavalue.id = empJoinDoc.Id;
                    try
                    {
                        foreach (string file in Request.Files)
                        {
                            var fileContent = Request.Files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                // get a stream
                                var stream = fileContent.InputStream;
                                // and optionally write the file to disk
                                var fileName = Path.GetFileName(file);
                                string strFolderPath = "~/CompanyData/" + companyId + "/Employee/" + empJoinDoc.EmployeeId.ToString().ToUpper() + "/JoiningDocument/" + empJoinDoc.Id.ToString().ToUpper();
                                string strRelationPath = "~/CompanyData/" + companyId + "/Employee/" + empJoinDoc.EmployeeId.ToString().ToUpper() + "/JoiningDocument/" + empJoinDoc.Id.ToString().ToUpper() + "/" + fileName;
                                var Fpath = Path.Combine(Server.MapPath(strFolderPath));
                                var path = Path.Combine(Server.MapPath(strRelationPath));

                                //Delete existing created folder.
                                if (System.IO.Directory.Exists(Fpath))
                                {
                                    System.IO.DirectoryInfo di = new DirectoryInfo(Fpath);
                                    foreach (FileInfo docfile in di.GetFiles())
                                    {
                                        docfile.Delete();
                                    }
                                    System.IO.Directory.Delete(Fpath);
                                }

                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                                }
                                using (var fileStream = System.IO.File.Create(path))
                                {
                                    stream.CopyTo(fileStream);
                                }
                                empJoinDoc.FilePath = strRelationPath;
                                empJoinDoc.Save();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log(ex);
                        string exss = ex.Message;
                        // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        //  return Json("Upload failed");
                        return base.BuildJson(false, 100, "There is some error while saving the file.", datavalue);
                    }

                    return base.BuildJson(true, 200, "Data saved successfully", datavalue);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", datavalue);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "The Employee and Joining document should not be empty.", datavalue);

            }


            //if (datavalue.employeeId != Guid.Empty && datavalue.joingDocumentId != Guid.Empty)
            //{
            //    EmployeeJoingDocument retObject = jsonEmployeeJoiningDoc.convertObject(datavalue);
            //    retObject.CreatedBy = userId;
            //    retObject.IsDeleted = false;
            //    if (retObject.Save())
            //    {
            //        return base.BuildJson(true, 200, "Data saved successfully", datavalue);
            //    }
            //    else
            //    {
            //        return base.BuildJson(false, 100, "There is some error while saving the data.", datavalue);
            //    }

            //}
            //else
            //{
            //    return base.BuildJson(false, 100, "The Employee and Joining document should not be empty.", datavalue);

            //}


        }
        public JsonResult SaveEmpHRComponent(jsonEmpHrComponent dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeHrComponent employeeHrComponent = jsonEmpHrComponent.convertobject(dataValue);
            isSaved = false;
            employeeHrComponent.CreatedBy = userId;
            employeeHrComponent.IsDeleted = false;
            isSaved = employeeHrComponent.Save();

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpLanguage(jsonEmpLangKnown dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeLanguageKnown empLanguageKnown = jsonEmpLangKnown.convertobject(dataValue);

            if (dataValue.empLangKnownEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                empLanguageKnown.CreatedBy = userId;
                empLanguageKnown.IsDeleted = false;
                isSaved = empLanguageKnown.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpNominee(jsonEmpNominee dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmployeeNominee employeeNominee = jsonEmpNominee.convertobject(dataValue);
            if (dataValue.empNomineeEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                employeeNominee.CreatedBy = userId;
                employeeNominee.IsDeleted = false;
                isSaved = employeeNominee.Save();

            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpPersonal(jsonEmpPersonalDetails dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Emp_Personal emp_Personal = jsonEmpPersonalDetails.convertobject(dataValue);
            if (dataValue.employeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                emp_Personal.CreatedBy = userId;
                emp_Personal.ModifiedBy = userId;
                emp_Personal.IsDeleted = false;
                isSaved = emp_Personal.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEmpTraining(jsonEmpTraining dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeTraining employeeTraining = jsonEmpTraining.convertobject(dataValue);
            if (dataValue.empTrainingEmployeeId == Guid.Empty)
            {
                isSaved = false;
            }
            else
            {
                employeeTraining.CreatedBy = userId;
                employeeTraining.IsDeleted = false;
                isSaved = employeeTraining.Save();
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult DeleteEmpData(Guid id, Guid empId, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            switch (type)
            {
                case "academic":
                    EmployeeAcademic cat = new EmployeeAcademic(empId, id);
                    cat.CreatedBy = userId;
                    cat.ModifiedBy = cat.CreatedBy;
                    cat.IsDeleted = true;
                    isDeleted = cat.Delete();
                    break;
                case "benefitComponent":
                    EmployeeBenefitComponent benefitComp = new EmployeeBenefitComponent(empId, id);
                    benefitComp.CreatedBy = userId;
                    benefitComp.ModifiedBy = benefitComp.CreatedBy;
                    benefitComp.IsDeleted = true;
                    isDeleted = benefitComp.Delete();
                    break;
                case "emergencyContact":
                    EmployeeEmegencyContact emergency = new EmployeeEmegencyContact(empId, id);
                    emergency.CreatedBy = userId;
                    emergency.ModifiedBy = emergency.CreatedBy;
                    emergency.IsDeleted = true;
                    isDeleted = emergency.Delete();
                    break;
                case "employeement":
                    EmployeeEmployeement employeement = new EmployeeEmployeement(empId, id);
                    employeement.CreatedBy = userId;
                    employeement.ModifiedBy = employeement.CreatedBy;
                    employeement.IsDeleted = true;
                    isDeleted = employeement.Delete();
                    break;
                case "family":
                    EmployeeFamily family = new EmployeeFamily(empId, id);
                    family.CreatedBy = userId;
                    family.ModifiedBy = family.CreatedBy;
                    family.IsDeleted = true;
                    isDeleted = family.Delete();
                    break;
                case "joinDoc":
                    EmployeeJoingDocument joinDoc = new EmployeeJoingDocument(empId, id);
                    joinDoc.CreatedBy = userId;
                    joinDoc.ModifiedBy = joinDoc.CreatedBy;
                    joinDoc.IsDeleted = true;
                    isDeleted = joinDoc.Delete();
                    break;
                case "HrComponent":
                    EmployeeHrComponent HrComponent = new EmployeeHrComponent(empId, id);
                    HrComponent.CreatedBy = userId;
                    HrComponent.ModifiedBy = HrComponent.CreatedBy;
                    HrComponent.IsDeleted = true;
                    isDeleted = HrComponent.Delete();
                    break;
                case "languageknown":
                    EmployeeLanguageKnown langaugeknown = new EmployeeLanguageKnown(empId, id);
                    langaugeknown.CreatedBy = userId;
                    langaugeknown.ModifiedBy = langaugeknown.CreatedBy;
                    langaugeknown.IsDeleted = true;
                    isDeleted = langaugeknown.Delete();
                    break;
                case "nominee":
                    EmployeeNominee nominee = new EmployeeNominee(empId, id);
                    nominee.CreatedBy = userId;
                    nominee.ModifiedBy = nominee.CreatedBy;
                    nominee.IsDeleted = true;
                    isDeleted = nominee.Delete();
                    break;
                case "training":
                    EmployeeTraining training = new EmployeeTraining(empId, id);
                    training.CreatedBy = userId;
                    training.ModifiedBy = training.CreatedBy;
                    training.IsDeleted = true;
                    isDeleted = training.Delete();
                    break;
                default:
                    break;
            }
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }
        }

        public JsonResult GetEmployeePayrollDetailsData(Guid empId)
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);

            PayrollHistory payrollHistory = new PayrollHistory(companyId, empId, 0, 0);
            if (payrollHistory.Status != "Processed")
            {


                return base.BuildJson(true, 200, "", null);

            }
            else
            {
                return base.BuildJson(false, 100, "You cannot able to change the joinning date", null);
            }
        }



        public JsonResult DeleteEmployee(Guid empId, string IsDeleteUser)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            PayrollHistory payrollHistory = new PayrollHistory(companyId, empId, 0, 0);
            if (payrollHistory.Status != "Processed")
            {
                Employee emp = new Employee();
                emp.Id = empId;
                emp.ModifiedBy = userId;
                emp.CreatedBy = userId;
                if (emp.Delete())
                {
                    User User = new User(empId);
                    User.ModifiedBy = userId;
                    //User.IsDeleted = true; // isdelete  handled on SP
                    if (User.Delete())
                    {
                        return base.BuildJson(true, 200, "Data deleted successfully", null);
                    }
                    return base.BuildJson(true, 200, "Employee data deleted successfully", null);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "Transaction already processed employee cannot be deleted", null);
            }
        }

        public JsonResult GetEmpAddress(Guid employeeId)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            //EmployeeAddressList familylist = new EmployeeAddressList(employeeId);
            //return base.BuildJson(true, 200, "success", familylist);




            EmployeeAddressList Addresslist = new EmployeeAddressList(employeeId);
            List<jsonEmpAddress> jsondata = new List<jsonEmpAddress>();
            Addresslist.ForEach(u => { jsondata.Add(jsonEmpAddress.tojson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetEmpBankDetails(Guid employeeId)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            Emp_BankList empBank = new Emp_BankList(employeeId);
            List<jsonEmpBank> jsondata = new List<jsonEmpBank>();
            empBank.ForEach(u => { jsondata.Add(jsonEmpBank.tojson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult GetEmpPersonalDetails(Guid employeeId)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            Emp_Personal empPersonal = new Emp_Personal(employeeId);
            return base.BuildJson(true, 200, "success", jsonEmpPersonalDetails.tojson(empPersonal));
        }
        public JsonResult GetSeparationData(jsonSeparation dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            Employee employee = new Employee(companyId, dataValue.SepCatid, dataValue.SepEmpId);
            jsonSeparation data = jsonSeparation.tojson(employee);
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult GetSalaryList()
        {
            string str2 = "";
            string pipServer = (ConfigurationManager.AppSettings["PIPUrl"]);
            //Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            if ((Convert.ToString(this.Session["EmployeeCode"]) != " ") && (Convert.ToString(this.Session["EmployeeCode"]) != null))
            {
                str2 = Convert.ToString(this.Session["EmployeeCode"]);
            }
            List<paylist> result = new List<paylist>();
            string code = ConfigurationManager.AppSettings["compCode"];//this.Session["CompanyId"].ToString().Trim();
            string dbCompcode = Convert.ToString(this.Session["compCode"]);
            string companyId = Convert.ToString(this.Session["CompanyId"]);
            pipServer = pipServer + "/" + dbCompcode + "/" + companyId + "/";
            string path1 = (pipServer + "/pip/payslipreports/" + code + "/");

            DirectoryInfo info = new DirectoryInfo(path1);

            ErrorLog.LogTestWrite(info.FullName);
            if (info.Exists)
            {
                ErrorLog.LogTestWrite("1" + path1);
                DirectoryInfo[] directories1 = info.GetDirectories();
                DirectoryInfo[] directories; //= //info.GetDirectories();
                directories = directories1.ToList().OrderBy(d => d.CreationTime).ToArray();


                List<paylist> yearlist = new List<paylist>();
                foreach (DirectoryInfo fileinfo in directories)
                {
                    string[] strArray = new string[10];
                    ErrorLog.LogTestWrite("2" + path1);
                    strArray = fileinfo.Name.Split(new char[] { '_' });
                    ErrorLog.LogTestWrite(fileinfo.Name);
                    if (strArray.Count() > 1)
                    {
                        if (!yearlist.Any(y => y.year == Convert.ToString(strArray[1])))
                        {
                            yearlist.Add(new paylist
                            {

                                year = Convert.ToString(strArray[1])
                            });
                        }
                    }

                }

                foreach (DirectoryInfo info2 in directories)
                {
                    string YearFolder = info2.Name.ToString();
                    string filepath = pipServer + "/pip/payslipreports/" + code + "/" + YearFolder + "/" + str2 + ".pdf";
                    FileInfo fname = new FileInfo(filepath);
                    ErrorLog.LogTestWrite(filepath);
                    if (fname.Exists)
                    {
                        ErrorLog.LogTestWrite(str2 + "Done");
                        string[] strArray = new string[10];
                        strArray = YearFolder.Split(new char[] { '_' });

                        yearlist.Where(y => y.year == Convert.ToString(strArray[1])).ToList().ForEach(payYear =>
                        {
                            payYear.filelist.Add(
                            new
                            {
                                filepath = decrypt(strArray[0].ToString() + "_" + strArray[1].ToString() + "_" + str2),
                                month = strArray[0].ToString()
                            });

                        });

                    }
                }
                result = yearlist;

                // to do add
            }


            return base.BuildJson(true, 200, "success", result);
        }

        public JsonResult GetTDSFormList(string formName)
        {
            string str2 = "";
            string userId = string.Empty;
            string pipServer = (ConfigurationManager.AppSettings["PIPUrl"]);
            string dbCompcode = Convert.ToString(this.Session["compCode"]);
            string companyId = Convert.ToString(this.Session["CompanyId"]);
            pipServer = pipServer + "\\" + dbCompcode + "\\" + companyId;
            //Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            List<paylist> result = new List<paylist>();
            if (!string.IsNullOrEmpty((Convert.ToString(this.Session["EmployeeCode"]))))
            {
                str2 = this.Session["EmployeeCode"].ToString();
                userId = Convert.ToString(this.Session["UserId"]).Trim();

                // List<paylist> result = new List<paylist>();
                string code = ConfigurationManager.AppSettings["compCode"];//this.Session["CompanyId"].ToString().Trim();
                string path1 = (pipServer + "\\pip\\payslipreports\\" + code + "\\TDS\\Form16\\");
                userId = Convert.ToString(this.Session["UserId"]).Trim();
                Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
                Emp_Personal empPersonal = new Emp_Personal(employeeId);
                string panNo = empPersonal.PANNumber != null ? empPersonal.PANNumber.Trim() : "";
                DirectoryInfo info = new DirectoryInfo(path1);

                ErrorLog.LogTestWrite(info.FullName);
                if (info.Exists)
                {
                    ErrorLog.LogTestWrite("1" + path1);
                    DirectoryInfo[] directories1 = info.GetDirectories();
                    DirectoryInfo[] directories; //= //info.GetDirectories();
                    directories = directories1.ToList().OrderBy(d => d.CreationTime).ToArray();


                    List<paylist> yearlist = new List<paylist>();
                    foreach (DirectoryInfo fileinfo in directories)
                    {
                        string[] strArray = new string[10];
                        ErrorLog.LogTestWrite("2" + path1);
                        strArray = fileinfo.Name.Split(new char[] { '_' });
                        if (Convert.ToString(strArray[0]).Replace(" ", "").Replace("Part", "").Trim().ToUpper() == formName)
                        {
                            ErrorLog.LogTestWrite(fileinfo.Name);
                            if (strArray.Count() > 1)
                            {
                                if (!yearlist.Any(y => y.year == Convert.ToString(strArray[1])))
                                {
                                    yearlist.Add(new paylist
                                    {

                                        year = Convert.ToString(strArray[1])
                                    });
                                }
                            }
                        }
                    }

                    foreach (DirectoryInfo info2 in directories)
                    {
                        string TDSFormFileYear = info2.Name.ToString();
                        string filepath = string.Empty;
                        string fileRename = string.Empty;
                        if (formName.ToUpper() == "FORM16A")
                        {
                            fileRename = TDSFormFileYear.Replace("Form16", "").Replace("Part A_", "").Replace(" ", "").Trim();
                            filepath = pipServer + "\\pip\\payslipreports\\" + code + "\\TDS\\Form16\\" + info2 + "\\" + panNo + "_" + fileRename + ".pdf";
                        }
                        else if (formName.ToUpper() == "FORM16B")
                        {
                            fileRename = TDSFormFileYear.Replace("Form16", "F16").Replace("Part B_", "Part B_AY ");
                            filepath = pipServer + "\\pip\\payslipreports\\" + code + "\\TDS/Form16\\" + info2 + "\\" + str2 + "_" + panNo + "_" + fileRename + ".pdf";
                        }
                        else if (formName.ToUpper() == "FORM12BA")
                        {
                            fileRename = TDSFormFileYear.Replace("Form 12BA_", "F12BA_AY ");
                            filepath = pipServer + "\\pip\\payslipreports\\" + code + "\\TDS\\Form16\\" + info2 + "\\" + str2 + "_" + panNo + "_" + fileRename + ".pdf";
                        }

                        FileInfo fname = new FileInfo(filepath);
                        ErrorLog.LogTestWrite(filepath);
                        if (fname.Exists)
                        {
                            ErrorLog.LogTestWrite(str2 + "Done");
                            string[] strArray = new string[10];
                            strArray = TDSFormFileYear.Split(new char[] { '_' });

                            yearlist.Where(y => y.year == Convert.ToString(strArray[1])).ToList().ForEach(payYear =>
                            {
                                payYear.filelist.Add(
                                new
                                {
                                    filepath = decrypt(strArray[0].ToString() + "_" + strArray[1].ToString() + "_" + str2),
                                    month = strArray[0].ToString()
                                });

                            });

                        }
                    }
                    result = yearlist;

                    // to do add
                }
            }

            return base.BuildJson(true, 200, "success", result);
        }
        private class paylist
        {
            public string year;
            public List<object> filelist = new List<object>();

        }

        public JsonResult downloadWorkSheet()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(this.Session["EmployeeCode"])))
            {
                string dbCompcode = Convert.ToString(this.Session["compCode"]);
                string str3 = Convert.ToString(ConfigurationManager.AppSettings["compCode"]);
                string str4 = Convert.ToString(this.Session["EmployeeCode"]).Trim();
                string WorkSheetFTPPath = Convert.ToString(ConfigurationManager.AppSettings["WorkSheetFTPPath"]);
                WorkSheetFTPPath = WorkSheetFTPPath.Replace("companyname", dbCompcode);
                string Payslipfile = Server.MapPath(WorkSheetFTPPath + str3 + "/" + str4 + ".pdf");
                FileInfo fname = new FileInfo(Payslipfile);
                if (fname.Exists)
                {
                    return base.BuildJson(true, 200, "success", new { filePath = Payslipfile });
                }
                else
                {
                    return base.BuildJson(false, 200, "File not Found", null);
                }
            }
            else
            {
                return base.BuildJson(false, 200, "EmployeeCode not Found", null);
            }
        }
        public JsonResult LoadPayslip(string Payslipfile)
        {
            string[] filepath = base.encrypt(Payslipfile).Split('_');
            string str_month = filepath[0].ToString().Trim();
            string str_year = filepath[1].ToString().Trim();
            string str3 = ConfigurationManager.AppSettings["compCode"];
            string str4 = Session["EmployeeCode"].ToString();


            //string path1 = Server.MapPath("~/pip/payslipreports/" + str3 + "/" + str_month + "_" + str_year + "/" + str4 + ".pdf");

            string pipServer = (ConfigurationManager.AppSettings["PIPUrl"]);
            string dbCompcode = Convert.ToString(this.Session["compCode"]);
            string companyId = Convert.ToString(this.Session["CompanyId"]);
            pipServer = pipServer + "/" + dbCompcode + "/" + companyId;

            Payslipfile = pipServer + "/pip/payslipreports/" + str3 + "/" + str_month + "_" + str_year + "/" + str4 + ".pdf";

            FileInfo fname = new FileInfo(Payslipfile);
            if (fname.Exists)
            {
                return base.BuildJson(true, 200, "success", new { filePath = Payslipfile });
            }
            else
            {
                return base.BuildJson(false, 200, "Payslip not available for " + Payslipfile, null);
            }
        }

        public JsonResult LoadForm16(string Payslipfile)
        {
            string[] filepath = base.encrypt(Payslipfile).Split('_');
            string str_form = filepath[0].ToString().Trim();
            string str_year = filepath[1].ToString().Trim();
            string code = ConfigurationManager.AppSettings["compCode"];
            string empcode = Session["EmployeeCode"].ToString();
            string userId = Convert.ToString(this.Session["UserId"]).Trim();
            string folderPath = str_form + "_" + str_year;
            string formReplaceVal = string.Empty;
            string filepath1 = string.Empty;
            string fileRename = string.Empty;
            string pipServer = (ConfigurationManager.AppSettings["PIPUrl"]);
            string dbCompcode = Convert.ToString(this.Session["compCode"]);
            string companyId = Convert.ToString(this.Session["CompanyId"]);
            pipServer = pipServer + "\\" + dbCompcode + "\\" + companyId;
            Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            Emp_Personal empPersonal = new Emp_Personal(employeeId);
            string panNo = empPersonal.PANNumber != null ? empPersonal.PANNumber.Trim() : "";

            if (str_form == "Form16 Part A")
            {
                fileRename = str_form.Replace("Form16", "").Replace("Part A", "").Replace(" ", "").Trim();
                Payslipfile = pipServer + "\\pip\\payslipreports\\" + code + "\\TDS\\Form16\\" + folderPath + "\\" + panNo + "_" + fileRename + str_year.Replace(" ", "") + ".pdf";
            }
            else if (str_form == "Form16 Part B")
            {
                fileRename = str_form.Replace("Form16", "F16").Replace("Part B", "Part B_AY");
                Payslipfile = pipServer + "/pip/payslipreports/" + code + "/TDS/Form16/" + folderPath + "/" + empcode + "_" + panNo + "_" + fileRename + " " + str_year + ".pdf";
            }
            else if (str_form == "Form 12BA")
            {
                fileRename = str_form.Replace("Form 12BA", "F12BA_AY ");
                Payslipfile = pipServer + "/pip/payslipreports/" + code + "/TDS/Form16/" + folderPath + "/" + empcode + "_" + panNo + "_" + fileRename + str_year + ".pdf";
            }




            FileInfo fname = new FileInfo(Payslipfile);
            if (fname.Exists)
            { //maddy




                return base.BuildJson(true, 200, "success", new { filePath = Payslipfile });
            }
            else
            {
                return base.BuildJson(false, 200, "File not found", null);
            }
        }
        public JsonResult SaveSeparation(jsonSeparation dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            Employee employee = new Employee(companyId, dataValue.SepCatid, dataValue.SepEmpId);
            employee.TypeOfSeparation = dataValue.SepType;
            employee.LastWorkingDate = Convert.ToDateTime(dataValue.SepLWDate);
            employee.SeparationReason = dataValue.SepReason;
            employee.ReleaseDate = DateTime.MinValue;
            employee.Status = 0;
            employee.SeparationDate = Convert.ToDateTime(dataValue.SepLWDate);//DateTime.Now;
            employee.CreatedBy = userId;
            employee.ModifiedBy = userId;
            employee.DBConnectionId = Convert.ToInt32(Session["DBConnectionId"]);
            if (employee.Save())
            {
                jsonSeparation data = jsonSeparation.tojson(employee);
                return base.BuildJson(true, 200, "Data saved successfully", data);
            }
            else
            {
                jsonSeparation data = jsonSeparation.tojson(employee);
                return base.BuildJson(false, 100, "There is some error while saving the data.", data);
            }

        }

        public JsonResult SaveResignation(jsonSeparation dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmpResignation empreg = new EmpResignation();
            empreg.EmpId = dataValue.SepEmpId;
            empreg.CompanyId = companyId;
            empreg.CreatedBy = userId;
            empreg.Reason = dataValue.SepReason;
            empreg.ResignationDate = Convert.ToDateTime(dataValue.SepResgDate);
            empreg.Save();

            Employee employee = new Employee(companyId, dataValue.SepCatid, dataValue.SepEmpId);
            employee.LastWorkingDate = Convert.ToDateTime(dataValue.SepLWDate);
            employee.TypeOfSeparation = "1";
            employee.SeparationReason = dataValue.SepReason;
            employee.ReleaseDate = DateTime.MinValue;
            employee.Status = 0;
            employee.CreatedBy = userId;
            employee.ModifiedBy = userId;
            employee.DBConnectionId = Convert.ToInt32(Session["DBConnectionId"]);
            if (employee.Save())
            {
                jsonSeparation data = jsonSeparation.tojson(employee);
                return base.BuildJson(true, 200, "Data saved successfully", data);
            }
            else
            {
                jsonSeparation data = jsonSeparation.tojson(employee);
                return base.BuildJson(false, 100, "There is some error while saving the data.", data);
            }

        }
        public JsonResult GetReleaseData(jsonRelease dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Employee employee = new Employee(companyId, dataValue.RelCatid, dataValue.RelEmpId);
            jsonRelease data = jsonRelease.tojson(employee);
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult SaveRelease(jsonRelease dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            Employee employee = new Employee(companyId, dataValue.RelCatid, dataValue.RelEmpId);
            employee.ReleaseDate = Convert.ToDateTime(dataValue.RelDate);
            employee.SeparationDate = DateTime.MinValue;
            employee.LastWorkingDate = DateTime.MinValue;
            employee.Status = 1;
            employee.TypeOfSeparation = "";
            employee.SeparationReason = "";
            employee.CreatedBy = userId;
            employee.ModifiedBy = userId;
            employee.DBConnectionId = Convert.ToInt32(Session["DBConnectionId"]);
            if (employee.Save())
            {
                jsonRelease data = jsonRelease.tojson(employee);
                return base.BuildJson(true, 200, "Data saved successfully", data);
            }
            else
            {
                jsonRelease data = jsonRelease.tojson(employee);
                return base.BuildJson(false, 100, "There is some error while saving the data.", data);
            }

        }

        public JsonResult GetBenefitComponent()
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            List<keyValueItem> benefitComponet = new List<keyValueItem>();
            EntityModel entitModel = new EntityModel(ComValue.SalaryTable, companyId);
            entitModel.EntityAttributeModelList.ForEach(p =>
            {
                if (p.AttributeModel.IsReimbursement)
                {
                    benefitComponet.Add(new keyValueItem() { Id = p.AttributeModel.Id, Name = p.AttributeModel.Name, DisplayName = p.AttributeModel.DisplayAs });
                }
            });

            List<jsonKeyValueItem> jsondata = new List<jsonKeyValueItem>();
            benefitComponet.ForEach(u => { jsondata.Add(jsonKeyValueItem.tojson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult downloadForm16()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(this.Session["EmployeeCode"])))
            {
                string str3 = Convert.ToString(ConfigurationManager.AppSettings["compCode"]);
                string empCode = Convert.ToString(this.Session["EmployeeCode"]);
                string str4 = Convert.ToString(this.Session["EmployeeCode"]).Trim();
                string userId = Convert.ToString(this.Session["UserId"]).Trim();
                Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));

                Emp_Personal empPersonal = new Emp_Personal(employeeId);
                string panNo = empPersonal.PANNumber.Trim();
                string Form16FTPPath = Convert.ToString(ConfigurationManager.AppSettings["Form16FTPPath"]);
                string Form16FileYear = Convert.ToString(ConfigurationManager.AppSettings["Form16FileYear"]);

                string Payslipfile = Server.MapPath(Form16FTPPath + "/" + empCode + "_" + panNo + "_" + Form16FileYear + ".pdf");
                string PayslipfileNew = Server.MapPath(Form16FTPPath + "/" + userId + "_" + panNo + "_" + Form16FileYear + ".pdf");
                FileInfo fname = new FileInfo(Payslipfile);
                FileInfo fnameNew = new FileInfo(PayslipfileNew);
                if (fname.Exists)
                {
                    return base.BuildJson(true, 200, "success", new { filePath = Payslipfile });
                }
                else if (fnameNew.Exists)
                {
                    return base.BuildJson(true, 200, "success", new { filePath = PayslipfileNew });
                }
                else
                {
                    return base.BuildJson(false, 200, "File not Found", null);
                }
            }
            else
            {
                return base.BuildJson(false, 200, "EmployeeCode not Found", null);
            }
        }
    }
    public class jsonKeyValueItem
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string displayName { get; set; }

        public static jsonKeyValueItem tojson(keyValueItem keyvalueitem)
        {
            jsonKeyValueItem retObject = new jsonKeyValueItem();
            retObject.displayName = keyvalueitem.DisplayName;
            retObject.id = keyvalueitem.Id;
            retObject.name = keyvalueitem.Name;
            return retObject;

        }
    }



    public class jsonEmpHrComponent
    {
        public Guid empHrComponentid { get; set; }
        public Guid empHrComponentEmployeeId { get; set; }
        public Guid empHrComponentHRComponentId { get; set; }
        public string empHrComponentHRComponentName { get; set; }
        public string empHRCompEffectiveDate { get; set; }
        public string empHRCompEndDate { get; set; }
        public string empHRCompComments { get; set; }



        public static jsonEmpHrComponent tojson(EmployeeHrComponent employeeHrComponent, HRComponentList hrComponentList)
        {
            return new jsonEmpHrComponent()
            {
                empHrComponentid = employeeHrComponent.Id,
                empHrComponentEmployeeId = employeeHrComponent.EmployeeId,
                empHrComponentHRComponentId = employeeHrComponent.HrComponentId,
                empHrComponentHRComponentName = hrComponentList.Where(u => u.Id == employeeHrComponent.HrComponentId).FirstOrDefault().Name,
                empHRCompEffectiveDate = employeeHrComponent.EffDate != DateTime.MinValue ? employeeHrComponent.EffDate.ToString("dd/MMM/yyyy") : "",
                empHRCompEndDate = employeeHrComponent.EndDate != DateTime.MinValue ? employeeHrComponent.EndDate.ToString("dd/MMM/yyyy") : "",
                empHRCompComments = employeeHrComponent.Comments

            };
        }
        public static EmployeeHrComponent convertobject(jsonEmpHrComponent employeeHrComponent)
        {
            DateTime endDate = new DateTime(1900, 1, 1);
            return new EmployeeHrComponent()
            {

                Id = employeeHrComponent.empHrComponentid,
                EmployeeId = employeeHrComponent.empHrComponentEmployeeId,
                HrComponentId = employeeHrComponent.empHrComponentHRComponentId,
                EffDate = !string.IsNullOrEmpty(employeeHrComponent.empHRCompEffectiveDate) ? Convert.ToDateTime(employeeHrComponent.empHRCompEffectiveDate) : DateTime.Now,
                EndDate = !string.IsNullOrEmpty(employeeHrComponent.empHRCompEndDate) ? Convert.ToDateTime(employeeHrComponent.empHRCompEndDate) : endDate,
                Comments = employeeHrComponent.empHRCompComments,
            };
        }

    }
    public class jsonEmployeeJoiningDoc
    {
        public Guid id { get; set; }

        public Guid employeeId { get; set; }

        public Guid joingDocumentId { get; set; }

        public string documentName { get; set; }

        public string status { get; set; }

        public string filePath { get; set; }

        //public string CreatedBy { get; set; }

        //public string IsDeleted { get; set; }

        public static jsonEmployeeJoiningDoc tojson(JoiningDocument joinDocs, EmployeeJoingDocumentList empJoinDoc)
        {
            jsonEmployeeJoiningDoc retObject = new jsonEmployeeJoiningDoc();
            retObject.documentName = joinDocs.DocumentName;
            var tmp = empJoinDoc.Where(u => u.JoiningDocumentId == joinDocs.Id).FirstOrDefault();
            if (!object.ReferenceEquals(tmp, null))
            {
                retObject.employeeId = empJoinDoc.EmployeeId;
                retObject.id = tmp.Id;
                retObject.joingDocumentId = tmp.JoiningDocumentId;
                retObject.filePath = tmp.FilePath;
                retObject.status = "Given";

            }
            else
            {
                retObject.joingDocumentId = joinDocs.Id;
                retObject.status = "Not Given";
            }

            return retObject;

        }
        public static EmployeeJoingDocument convertObject(jsonEmployeeJoiningDoc jsonjoinDocs)
        {
            EmployeeJoingDocument retObject = new EmployeeJoingDocument();
            // retObject.documentName = joinDocs.Where(u => u.Id == empJoinDoc.JoiningDocumentId).FirstOrDefault().DocumentName;
            retObject.EmployeeId = jsonjoinDocs.employeeId;
            retObject.Id = jsonjoinDocs.id;
            retObject.JoiningDocumentId = jsonjoinDocs.joingDocumentId;
            retObject.FilePath = jsonjoinDocs.filePath;
            return retObject;

        }



    }
    public class jsonDashboardPayroll
    {
        public string Id { get; set; }
        public string empName { get; set; }
        public DateTime Date { get; set; }
        public string Imgcode { get; set; }
        public string Displayname { get; set; }
        public int Monthnumb { get; set; }
        public int currentyear { get; set; }
        public int NotedYear { get; set; }
        public int ServiceyrORAge { get; set; }
    }

    public class jsonEmployeeCodeChange
    {
        public Guid empid { get; set; }
        public string empCode { get; set; }
        public string oldCode { get; set; }
    }
    public class jsonEmployee
    {
        public Guid categoryId { get; set; }
        public Guid empid { get; set; }
        public string empCode { get; set; }
        public string empFName { get; set; }
        public string empLName { get; set; }
        public string EmpcodeName { get; set; }
        public string empEmail { get; set; }
        public string empPhone { get; set; }
        public string empDOB { get; set; }
        public string empDOJ { get; set; }
        public string empDOW { get; set; }
        public int empConfPeriod { get; set; }
        public string empConfDate { get; set; }
        public string empSeparationDate { get; set; }
        public int empRetYears { get; set; }
        public bool empcreationtype { get; set; }
        public string empRetDate { get; set; }

        public int empGender { get; set; }
        public Guid empDesign { get; set; }

        public Guid empDepart { get; set; }

        public bool empisMetro { get; set; }

        public Guid empBranch { get; set; }

        public Guid empLocation { get; set; }

        public Guid empCostCentre { get; set; }

        public Guid empGrade { get; set; }

        public bool empStopPayment { get; set; }

        public bool empPayrollProcess { get; set; }

        public int empStatus { get; set; }

        public Guid empESILocation { get; set; }
        public Guid empESIDispensary { get; set; }

        public Guid empPTLocation { get; set; }
        public string dept { get; set; }
        public string designation { get; set; }
        public string location { get; set; }
        public string esilocation { get; set; }
        public string branch { get; set; }
        public string category { get; set; }
        public string grade { get; set; }
        public string ptlocation { get; set; }
        public string costcenter { get; set; }
        public Guid LeaveID { get; set; }
        public string EmployeeImage { get; set; }
        public bool empPwdMailSend { get; set; }

        public static jsonEmployee tojson(Employee employee)
        {
            //DepartmentList departmentList = new DepartmentList(employee.CompanyId);
            //DesignationList designationList = new DesignationList(employee.CompanyId);
            //LocationList locationlist = new LocationList(employee.CompanyId);
            //PTLocationList ptlocationlist = new PTLocationList(employee.CompanyId);
            //EsiLocationList esiLocationlist = new EsiLocationList(employee.CompanyId);
            //CategoryList categorylist = new CategoryList(employee.CompanyId);
            //BranchList branchlist = new BranchList(employee.CompanyId);
            //GradeList gradelist = new GradeList(employee.CompanyId);
            //CostCentreList costcentrlist = new CostCentreList(employee.CompanyId);
            return new jsonEmployee()
            {
                categoryId = employee.CategoryId,
                empid = employee.Id,
                empCode = employee.EmployeeCode,
                empFName = employee.FirstName,
                empLName = employee.LastName,
                empEmail = employee.Email,
                empPhone = employee.Phone,
                empDOB = employee.DateOfBirth != DateTime.MinValue ? employee.DateOfBirth.ToString("dd/MMM/yyyy") : "",
                empDOJ = employee.DateOfJoining != DateTime.MinValue ? employee.DateOfJoining.ToString("dd/MMM/yyyy") : "",
                empDOW = employee.DateOfWedding != DateTime.MinValue ? employee.DateOfWedding.ToString("dd/MMM/yyyy") : "",
                empConfPeriod = employee.ConfirmationPeriod,
                empConfDate = employee.ConfirmationDate != DateTime.MinValue ? employee.ConfirmationDate.ToString("dd/MMM/yyyy") : "",
                empSeparationDate = employee.SeparationDate != DateTime.MinValue ? employee.SeparationDate.ToString("dd/MMM/yyyy") : "",
                empRetYears = employee.RetirementYears,
                empRetDate = employee.RetirementDate != DateTime.MinValue ? employee.RetirementDate.ToString("dd/MMM/yyyy") : "",
                empGender = employee.Gender,
                empDesign = employee.Designation,//designationList.Where (u => u.Id == employee.Designation).FirstOrDefault().DesignationName,//loanName = loanmasterlist.Where(u => u.Id == loanEntry.LoanMasterId).FirstOrDefault().LoanDesc,
                empDepart = employee.Department,
                empisMetro = employee.isMetro,
                empBranch = employee.Branch,
                empLocation = employee.Location,
                empCostCentre = employee.CostCentre,
                empGrade = employee.Grade,
                empStopPayment = employee.StopPayment,
                empPayrollProcess = employee.PayrollProcess,
                empStatus = employee.Status,
                empESILocation = employee.ESILocation,
                empESIDispensary = employee.ESIDespensary,
                empPTLocation = employee.PTLocation,
                empcreationtype = employee.Usercreationtype,
                //dept = departmentList.Where(d => d.Id == employee.Department).ToList().FirstOrDefault() == null ? "" :
                //       departmentList.Where(d => d.Id == employee.Department).ToList().FirstOrDefault().DepartmentName,
                //designation = designationList.Where(d => d.Id == employee.Designation).ToList().FirstOrDefault() == null ? "" :
                //              designationList.Where(d => d.Id == employee.Designation).ToList().FirstOrDefault().DesignationName,
                //location = locationlist.Where(d => d.Id == employee.Location).ToList().FirstOrDefault() == null ? "" :
                //           locationlist.Where(d => d.Id == employee.Location).ToList().FirstOrDefault().LocationName,
                //branch = branchlist.Where(d => d.Id == employee.Branch).ToList().FirstOrDefault() == null ? "" :
                //         branchlist.Where(d => d.Id == employee.Branch).ToList().FirstOrDefault().BranchName,
                //category = categorylist.Where(d => d.Id == employee.CategoryId).ToList().FirstOrDefault() == null ? "" :
                //         categorylist.Where(d => d.Id == employee.CategoryId).ToList().FirstOrDefault().Name,
                //grade = gradelist.Where(d => d.Id == employee.Grade).ToList().FirstOrDefault() == null ? "" : gradelist.Where(d => d.Id == employee.Grade).ToList().FirstOrDefault().GradeName,
                //costcenter = costcentrlist.Where(d => d.Id == employee.CostCentre).ToList().FirstOrDefault() == null ? "" :
                //         costcentrlist.Where(d => d.Id == employee.CostCentre).ToList().FirstOrDefault().CostCentreName,
                //esilocation = esiLocationlist.Where(d => d.Id == employee.ESILocation).ToList().FirstOrDefault() == null ? "" :
                //         esiLocationlist.Where(d => d.Id == employee.ESILocation).ToList().FirstOrDefault().LocationName,
                //ptlocation = ptlocationlist.Where(d => d.Id == employee.PTLocation).ToList().FirstOrDefault() == null ? "" :
                //         ptlocationlist.Where(d => d.Id == employee.PTLocation).ToList().FirstOrDefault().PTLocationName
                dept = employee.DepartmentName,
                designation = employee.DesignationName,
                location = employee.LocationName,
                branch = employee.BranchName,
                category = employee.CategoryName,
                grade = employee.GradeName,
                costcenter = employee.CostCentreName,
                esilocation = employee.LocationName,
                ptlocation = employee.PTLocationName,
                EmployeeImage = employee.EmployeeImage,
                empPwdMailSend = employee.IsMailSend
            };
        }
        public static Employee convertobject(jsonEmployee employee)
        {
            return new Employee()
            {
                CategoryId = employee.categoryId,
                Id = employee.empid,
                EmployeeCode = employee.empCode,
                FirstName = employee.empFName,
                LastName = employee.empLName,
                Email = employee.empEmail,
                Phone = employee.empPhone,
                DateOfBirth = employee.empDOB != string.Empty ? Convert.ToDateTime(employee.empDOB) : DateTime.MinValue,//DateTime.Now,
                DateOfJoining = employee.empDOJ != string.Empty ? Convert.ToDateTime(employee.empDOJ) : DateTime.MinValue,
                DateOfWedding = employee.empDOW != string.Empty ? Convert.ToDateTime(employee.empDOW) : DateTime.MinValue,
                ConfirmationPeriod = employee.empConfPeriod,
                ConfirmationDate = employee.empConfDate != string.Empty ? Convert.ToDateTime(employee.empConfDate) : DateTime.MinValue,
                SeparationDate = employee.empSeparationDate != string.Empty ? Convert.ToDateTime(employee.empSeparationDate) : DateTime.MinValue,
                RetirementYears = employee.empRetYears,
                RetirementDate = employee.empRetDate != string.Empty ? Convert.ToDateTime(employee.empRetDate) : DateTime.MinValue,
                Gender = employee.empGender,
                Designation = employee.empDesign,
                Department = employee.empDepart,
                isMetro = employee.empisMetro,
                Branch = employee.empBranch,
                Location = employee.empLocation,
                CostCentre = employee.empCostCentre,
                Category = employee.category,
                Grade = employee.empGrade,
                StopPayment = employee.empStopPayment,
                PayrollProcess = employee.empPayrollProcess,
                Status = employee.empStatus,
                EmployeeImage = employee.EmployeeImage,
                ESILocation = employee.empESILocation,
                ESIDespensary = employee.empESIDispensary,
                PTLocation = employee.empPTLocation
            };
        }


    }


    public class jsonDropdownEmployee
    {
        public Guid Id { get; set; }
        public Guid categoryId { get; set; }
        public Guid empid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }

        public string category { get; set; }
        public string lastworkingDate { get; set; }
        public string resignationDate { get; set; }
        public string separtionDate { get; set; }
        public string reason { get; set; }
        public string separationType { get; set; }

        public static jsonDropdownEmployee tojson(Employee employee)
        {
            return new jsonDropdownEmployee()
            {
                categoryId = employee.CategoryId,
                empid = employee.Id,
                empCode = employee.EmployeeCode,
                empName = employee.FirstName + " " + !string.IsNullOrEmpty(employee.LastName),
                category = employee.CategoryName,
                lastworkingDate = employee.LastWorkingDate != DateTime.MinValue ? employee.LastWorkingDate.ToString("dd/MM/yyyy") : "",
                separtionDate = employee.SeparationDate != DateTime.MinValue ? employee.SeparationDate.ToString("dd/MM/yyyy") : "",
                reason = employee.SeparationReason,
                separationType = Convert.ToString((SeparationTypeEnum)Convert.ToInt32(employee.TypeOfSeparation))
            };
        }

        public static jsonDropdownEmployee tojson(EmpResignation employee)
        {
            return new jsonDropdownEmployee()
            {
                Id = employee.Id,
                categoryId = employee.CategoryId,
                empid = employee.EmpId,
                empCode = employee.EmpCode,
                category = employee.Category,
                lastworkingDate = employee.LastWorkingDate != DateTime.MinValue ? employee.LastWorkingDate.ToString("dd/MMM/yyyy") : "",
                resignationDate = employee.ResignationDate != DateTime.MinValue ? employee.ResignationDate.ToString("dd/MMM/yyyy") : "",
                reason = employee.Reason,

            };
        }
    }

    public class jsonEmpAddress
    {
        public Guid empAddid { get; set; }
        public Guid empAddEmployeeId { get; set; }
        public string empAddressLine1 { get; set; }
        public string empAddressLine2 { get; set; }
        public string empCity { get; set; }
        public string empState { get; set; }
        public string empCountry { get; set; }
        public string empPinCode { get; set; }
        public string empPhone { get; set; }
        public int empAddressType { get; set; }


        public static jsonEmpAddress tojson(EmployeeAddress employeeAddress)
        {
            return new jsonEmpAddress()
            {
                empAddid = employeeAddress.Id,
                empAddEmployeeId = employeeAddress.EmployeeId,
                empAddressLine1 = employeeAddress.AddressLine1,
                empAddressLine2 = employeeAddress.AddressLine2,
                empCity = employeeAddress.City,
                empState = employeeAddress.State,
                empPhone = employeeAddress.Phone,
                empCountry = employeeAddress.Country,
                empPinCode = employeeAddress.PinCode,
                empAddressType = employeeAddress.AddressType


            };
        }
        public static EmployeeAddress convertobject(jsonEmpAddress employeeAddress)
        {
            return new EmployeeAddress()
            {
                Id = employeeAddress.empAddid,
                EmployeeId = employeeAddress.empAddEmployeeId,
                AddressLine1 = employeeAddress.empAddressLine1,
                AddressLine2 = employeeAddress.empAddressLine2,
                City = employeeAddress.empCity,
                State = employeeAddress.empState,
                Phone = employeeAddress.empPhone,
                Country = employeeAddress.empCountry,
                PinCode = employeeAddress.empPinCode,
                AddressType = employeeAddress.empAddressType

            };
        }


    }

    public class jsonEmpFamily
    {
        public Guid empFamilyid { get; set; }
        public Guid empFamilyEmployeeId { get; set; }
        public string empFamilyName { get; set; }
        public string empFamilyAddress { get; set; }
        //public int empFamilyRelationShip { get; set; }

        public jsonRelationShip relation { get; set; }

        public string empFamilyDOB { get; set; }
        public int empFamilyAge { get; set; }

        public static jsonEmpFamily tojson(EmployeeFamily employeeFamily)
        {
            return new jsonEmpFamily()
            {
                empFamilyid = employeeFamily.Id,
                empFamilyEmployeeId = employeeFamily.EmployeeId,
                empFamilyName = employeeFamily.Name,
                empFamilyAddress = employeeFamily.Address,
                // empFamilyRelationShip = employeeFamily.RelationShip,
                empFamilyDOB = employeeFamily.DateOfBirth != DateTime.MinValue ? employeeFamily.DateOfBirth.ToString("dd/MMM/yyyy") : "",
                empFamilyAge = employeeFamily.Age,
                relation = jsonRelationShip.Get(employeeFamily.RelationShip)
            };
        }
        public static EmployeeFamily convertobject(jsonEmpFamily employeeFamily)
        {
            return new EmployeeFamily()
            {
                Id = employeeFamily.empFamilyid,
                EmployeeId = employeeFamily.empFamilyEmployeeId,
                Name = employeeFamily.empFamilyName,
                Address = employeeFamily.empFamilyAddress,
                RelationShip = employeeFamily.relation.id,//employeeFamily.empFamilyRelationShip,
                DateOfBirth = employeeFamily.empFamilyDOB != string.Empty ? Convert.ToDateTime(employeeFamily.empFamilyDOB) : DateTime.Now,
                Age = employeeFamily.empFamilyAge,
            };
        }

    }

    public class jsonEmpAcademic
    {
        public Guid empAcademicid { get; set; }
        public Guid empAcademicEmployeeId { get; set; }
        public string empAcademicDegreeName { get; set; }
        public string empAcademicInstitionName { get; set; }
        public int empAcademicYearOfPassing { get; set; }

        public static jsonEmpAcademic tojson(EmployeeAcademic employeeAcademic)
        {
            return new jsonEmpAcademic()
            {
                empAcademicid = employeeAcademic.Id,
                empAcademicEmployeeId = employeeAcademic.EmployeeId,
                empAcademicDegreeName = employeeAcademic.DegreeName,
                empAcademicInstitionName = employeeAcademic.InstitionName,
                empAcademicYearOfPassing = employeeAcademic.YearOfPassing
            };
        }
        public static EmployeeAcademic convertobject(jsonEmpAcademic employeeAcademic)
        {
            return new EmployeeAcademic()
            {
                Id = employeeAcademic.empAcademicid,
                EmployeeId = employeeAcademic.empAcademicEmployeeId,
                DegreeName = employeeAcademic.empAcademicDegreeName,
                InstitionName = employeeAcademic.empAcademicInstitionName,
                YearOfPassing = employeeAcademic.empAcademicYearOfPassing
            };
        }

    }

    public class jsonEmpEmployeeMent
    {
        public Guid empEmployeementid { get; set; }
        public Guid empEmployeementEmployeeId { get; set; }
        public string empEmployeementEmpCode { get; set; }
        public string empEmployeementCompanyName { get; set; }
        public string empEmployeementPositionHeld { get; set; }
        public string empEmployeementWorkFrom { get; set; }
        public string empEmployeementWorkTo { get; set; }



        public static jsonEmpEmployeeMent tojson(EmployeeEmployeement employeeEmployeement)
        {
            return new jsonEmpEmployeeMent()
            {
                empEmployeementid = employeeEmployeement.Id,
                empEmployeementEmployeeId = employeeEmployeement.EmployeeId,
                empEmployeementEmpCode = employeeEmployeement.EmployeeCode,
                empEmployeementCompanyName = employeeEmployeement.CompanyName,
                empEmployeementPositionHeld = employeeEmployeement.PositionHeld,
                empEmployeementWorkFrom = employeeEmployeement.WorkFrom != DateTime.MinValue ? employeeEmployeement.WorkFrom.ToString("dd/MMM/yyyy") : "",
                empEmployeementWorkTo = employeeEmployeement.WorkTo != DateTime.MinValue ? employeeEmployeement.WorkTo.ToString("dd/MMM/yyyy") : "",

            };
        }
        public static EmployeeEmployeement convertobject(jsonEmpEmployeeMent employeeEmployeement)
        {
            return new EmployeeEmployeement()
            {
                Id = employeeEmployeement.empEmployeementid,
                EmployeeId = employeeEmployeement.empEmployeementEmployeeId,
                EmployeeCode = employeeEmployeement.empEmployeementEmpCode,
                CompanyName = employeeEmployeement.empEmployeementCompanyName,
                PositionHeld = employeeEmployeement.empEmployeementPositionHeld,
                WorkFrom = employeeEmployeement.empEmployeementWorkFrom != string.Empty ? Convert.ToDateTime(employeeEmployeement.empEmployeementWorkFrom) : DateTime.Now,
                WorkTo = employeeEmployeement.empEmployeementWorkTo != string.Empty ? Convert.ToDateTime(employeeEmployeement.empEmployeementWorkTo) : DateTime.Now,
            };
        }

    }

    public class jsonEmpTraining
    {
        public Guid empTrainingNameid { get; set; }
        public Guid empTrainingEmployeeId { get; set; }
        public string empTrainingName { get; set; }
        public string empTrainingFDate { get; set; }
        public string empTrainingTDate { get; set; }
        public string empTrainingCertfNo { get; set; }
        public string empTrainingInstitute { get; set; }



        public static jsonEmpTraining tojson(EmployeeTraining employeeTraining)
        {
            return new jsonEmpTraining()
            {
                empTrainingNameid = employeeTraining.Id,
                empTrainingEmployeeId = employeeTraining.EmployeeId,
                empTrainingName = employeeTraining.TrainingName,
                empTrainingFDate = employeeTraining.TrainingDate != DateTime.MinValue ? employeeTraining.TrainingDate.ToString("dd/MMM/yyyy") : "",
                empTrainingTDate = employeeTraining.TrainingTo != DateTime.MinValue ? employeeTraining.TrainingTo.ToString("dd/MMM/yyyy") : "",
                empTrainingCertfNo = employeeTraining.CertificateNumber,
                empTrainingInstitute = employeeTraining.Institute

            };
        }
        public static EmployeeTraining convertobject(jsonEmpTraining employeeTraining)
        {
            return new EmployeeTraining()
            {
                Id = employeeTraining.empTrainingNameid,
                EmployeeId = employeeTraining.empTrainingEmployeeId,
                TrainingName = employeeTraining.empTrainingName,
                TrainingDate = employeeTraining.empTrainingFDate != string.Empty ? Convert.ToDateTime(employeeTraining.empTrainingFDate) : DateTime.Now,
                TrainingTo = employeeTraining.empTrainingTDate != string.Empty ? Convert.ToDateTime(employeeTraining.empTrainingTDate) : DateTime.Now,
                CertificateNumber = employeeTraining.empTrainingCertfNo,
                Institute = employeeTraining.empTrainingInstitute
            };
        }

    }

    public class jsonEmpNominee
    {
        public Guid empNomineeid { get; set; }
        public Guid empNomineeEmployeeId { get; set; }
        public string empNomineeName { get; set; }
        public string empNomineeAddress { get; set; }
        // public int empNomineeRelationShip { get; set; }
        public jsonRelationShip relation { get; set; }
        public string empNomineeDOB { get; set; }
        public double empNomineeAmtPercent { get; set; }
        public int empNomineeAge { get; set; }
        public string empGuardianAddr { get; set; }

        public static jsonEmpNominee tojson(EmployeeNominee employeeNominee)
        {
            return new jsonEmpNominee()
            {
                empNomineeid = employeeNominee.Id,
                empNomineeEmployeeId = employeeNominee.EmployeeId,
                empNomineeName = employeeNominee.NomineeName,
                empNomineeAddress = employeeNominee.Address,
                // empNomineeRelationShip = employeeNominee.RelationShip,
                empNomineeDOB = employeeNominee.DateOfBirth != DateTime.MinValue ? employeeNominee.DateOfBirth.ToString("dd/MMM/yyyy") : "",
                empNomineeAmtPercent = employeeNominee.AmountPercentage,
                empNomineeAge = employeeNominee.Age,
                empGuardianAddr = employeeNominee.NameOfGuardianAndAddress,
                relation = jsonRelationShip.Get(employeeNominee.RelationShip)
            };
        }
        public static EmployeeNominee convertobject(jsonEmpNominee employeeNominee)
        {
            return new EmployeeNominee()
            {
                Id = employeeNominee.empNomineeid,
                EmployeeId = employeeNominee.empNomineeEmployeeId,
                NomineeName = employeeNominee.empNomineeName,
                Address = employeeNominee.empNomineeAddress,
                RelationShip = employeeNominee.relation.id,//employeeNominee.empNomineeRelationShip,
                DateOfBirth = employeeNominee.empNomineeDOB != string.Empty ? Convert.ToDateTime(employeeNominee.empNomineeDOB) : DateTime.Now,
                AmountPercentage = Convert.ToDouble(employeeNominee.empNomineeAmtPercent),
                Age = employeeNominee.empNomineeAge,
                NameOfGuardianAndAddress = employeeNominee.empGuardianAddr
            };
        }

    }

    public class jsonEmpbenefitComponent
    {
        public Guid empBenefitCompid { get; set; }
        public Guid empBenefitCompEmployeeId { get; set; }
        public string empBenefitComponentName { get; set; }
        public Guid empBenefitComponentId { get; set; }
        public Decimal empBenefitCompAmt { get; set; }
        public string empBenefitCompEffDate { get; set; }

        public static jsonEmpbenefitComponent tojson(EmployeeBenefitComponent empBenefitComponent, List<keyValueItem> benefitComponent)
        {
            var tmp = benefitComponent.Where(u => u.Id == empBenefitComponent.BenefitComponentId).FirstOrDefault();

            return new jsonEmpbenefitComponent()
            {
                empBenefitCompid = empBenefitComponent.Id,
                empBenefitCompEmployeeId = empBenefitComponent.EmployeeId,
                empBenefitComponentId = empBenefitComponent.BenefitComponentId,
                empBenefitCompAmt = empBenefitComponent.Amount,
                empBenefitCompEffDate = empBenefitComponent.EffectiveDate != DateTime.MinValue ? empBenefitComponent.EffectiveDate.ToString("dd/MMM/yyyy") : "",
                empBenefitComponentName = tmp != null ? tmp.DisplayName : ""

            };
        }
        public static EmployeeBenefitComponent convertobject(jsonEmpbenefitComponent empBenefitComponent)
        {
            return new EmployeeBenefitComponent()
            {
                Id = empBenefitComponent.empBenefitCompid,
                EmployeeId = empBenefitComponent.empBenefitCompEmployeeId,
                BenefitComponentId = empBenefitComponent.empBenefitComponentId,
                Amount = empBenefitComponent.empBenefitCompAmt,
                EffectiveDate = empBenefitComponent.empBenefitCompEffDate != string.Empty ? Convert.ToDateTime(empBenefitComponent.empBenefitCompEffDate) : DateTime.Now
            };
        }

    }

    public class jsonEmpEmegencyContact
    {
        public Guid empEmrgContid { get; set; }
        public Guid empEmrgContEmployeeId { get; set; }
        public string empEmrgContName { get; set; }
        public string empEmrgContNumber { get; set; }
        // public int empEmrgContRelationShip { get; set; }
        public jsonRelationShip relation { get; set; }
        public string empEmrgContAddress { get; set; }
        public static jsonEmpEmegencyContact tojson(EmployeeEmegencyContact empEmegencyContact)
        {
            return new jsonEmpEmegencyContact()
            {
                empEmrgContid = empEmegencyContact.Id,
                empEmrgContEmployeeId = empEmegencyContact.EmployeeId,
                empEmrgContName = empEmegencyContact.ContactName,
                empEmrgContNumber = empEmegencyContact.ContactNumber,
                // empEmrgContRelationShip = empEmegencyContact.RelationShip,
                empEmrgContAddress = empEmegencyContact.Address,
                relation = jsonRelationShip.Get(empEmegencyContact.RelationShip)
            };
        }
        public static EmployeeEmegencyContact convertobject(jsonEmpEmegencyContact empEmegencyContact)
        {
            return new EmployeeEmegencyContact()
            {
                Id = empEmegencyContact.empEmrgContid,
                EmployeeId = empEmegencyContact.empEmrgContEmployeeId,
                ContactName = empEmegencyContact.empEmrgContName,
                ContactNumber = empEmegencyContact.empEmrgContNumber,
                RelationShip = empEmegencyContact.relation.id,//empEmrgContRelationShip,
                Address = empEmegencyContact.empEmrgContAddress
            };
        }

    }

    public class jsonEmpLangKnown
    {
        public Guid empLangKnownid { get; set; }
        public Guid empLangKnownEmployeeId { get; set; }
        //public int empLangKnownLanguageId { get; set; }
        public jsonLanguage language { get; set; }
        public bool empLangKnownIsSpeak { get; set; }
        public bool empLangKnownIsRead { get; set; }
        public bool empLangKnownIsWrite { get; set; }
        public string languages { get; set; }
        public int languageId { get; set; }
        public static jsonEmpLangKnown tojson(EmployeeLanguageKnown empLanguageKnown)
        {
            return new jsonEmpLangKnown()
            {
                empLangKnownid = empLanguageKnown.Id,
                empLangKnownEmployeeId = empLanguageKnown.EmployeeId,
                //empLangKnownLanguageId = empLanguageKnown.LanguageId,
                empLangKnownIsSpeak = empLanguageKnown.IsSpeak,
                empLangKnownIsRead = empLanguageKnown.IsRead,
                empLangKnownIsWrite = empLanguageKnown.IsWrite,
                languages = empLanguageKnown.Language,// jsonLanguage.Get(empLanguageKnown.LanguageId)
                languageId = empLanguageKnown.LanguageId
            };
        }
        public static EmployeeLanguageKnown convertobject(jsonEmpLangKnown empLanguageKnown)
        {
            return new EmployeeLanguageKnown()
            {
                Id = empLanguageKnown.empLangKnownid,
                EmployeeId = empLanguageKnown.empLangKnownEmployeeId,
                LanguageId = empLanguageKnown.language.id,//.empLangKnownLanguageId,
                IsSpeak = empLanguageKnown.empLangKnownIsSpeak,
                IsRead = empLanguageKnown.empLangKnownIsRead,
                IsWrite = empLanguageKnown.empLangKnownIsWrite
            };
        }

    }
    public class jsonEmpContractDetails
    {
        public Guid Id { get; set; }
        public Guid EmpID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public static jsonEmpContractDetails tojson(EmployeeContractDetail objContrDet)
        {
            return new jsonEmpContractDetails()
            {
                Id = objContrDet.Id,
                EmpID = objContrDet.EmpId,
                StartDate = objContrDet.StartDate,
                EndDate = objContrDet.EndDate,
                Remarks = objContrDet.Remarks
            };
        }
        public static EmployeeContractDetail convertobject(jsonEmpContractDetails objJsonRslt)
        {
            return new EmployeeContractDetail()
            {
                Id = objJsonRslt.Id,
                EmpId = objJsonRslt.EmpID,
                StartDate = objJsonRslt.StartDate,
                EndDate = objJsonRslt.EndDate,
                Remarks = objJsonRslt.Remarks
            };
        }
    }

    public class jsonRelationShip
    {
        public int id { get; set; }
        public string name { get; set; }

        private static List<jsonRelationShip> _relationShips;
        public jsonRelationShip()
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                _relationShips = new List<jsonRelationShip>();
                RelationShip rel = new RelationShip();
                List<RelationShip> list = rel.GetRelationship();
                list.ForEach(u =>
                {
                    _relationShips.Add(new jsonRelationShip() { id = u.Id, name = u.Name });
                });

            }

        }
        public static jsonRelationShip Get(int id)
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                jsonRelationShip tmp = new jsonRelationShip();
            }
            var ret = _relationShips.Where(u => u.id == id).FirstOrDefault();
            if (object.ReferenceEquals(ret, null))
                ret = new jsonRelationShip();
            return ret;
        }
        public List<jsonRelationShip> GetRelationship()
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                jsonRelationShip tmp = new jsonRelationShip();
            }
            return _relationShips;
        }
    }

    public class jsonLanguage
    {
        public int id { get; set; }
        public string name { get; set; }

        private static List<jsonLanguage> _languages;
        public jsonLanguage()
        {
            if (object.ReferenceEquals(_languages, null))
            {
                _languages = new List<jsonLanguage>();
                Language lang = new Language();
                List<Language> lst = lang.GetLanguages();
                lst.ForEach(u =>
                {
                    _languages.Add(new jsonLanguage() { id = u.LangId, name = u.Name });
                });

            }

        }

        public static jsonLanguage Get(int id)
        {
            if (object.ReferenceEquals(_languages, null))
            {
                jsonLanguage tmp = new jsonLanguage();
            }
            var ret = _languages.Where(u => u.id == id).FirstOrDefault();
            if (object.ReferenceEquals(ret, null))
                ret = new jsonLanguage();
            return ret;
        }
        public List<jsonLanguage> GetLanguages()
        {
            if (object.ReferenceEquals(_languages, null))
            {
                jsonLanguage tmp = new jsonLanguage();
            }
            return _languages;
        }
    }

    public class jsonSeparation
    {
        public Guid SepCatid { get; set; }
        public Guid SepEmpId { get; set; }
        public string SepEmpName { get; set; }
        public string SepDOJ { get; set; }
        public string SepType { get; set; }
        public string SepLWDate { get; set; }
        public string SepReason { get; set; }
        // Modified by Babu.R as on 24-Jul-2017 for Separation last working date validation
        public string SepMonthlyLastWorkingDate { get; set; }
        public string SepPayrollLastWorkingDate { get; set; }
        public string SepMonthlyDate { get; set; }
        public string SepPayrollDate { get; set; }
        public string SepResgDate { get; set; }

        public static jsonSeparation tojson(Employee employee)
        {
            return new jsonSeparation()
            {
                SepCatid = employee.CategoryId,
                SepEmpId = employee.Id,
                SepDOJ = employee.DateOfJoining != DateTime.MinValue ? employee.DateOfJoining.ToString("dd/MMM/yyyy") : "",
                SepType = employee.TypeOfSeparation,
                SepLWDate = employee.LastWorkingDate != DateTime.MinValue ? employee.LastWorkingDate.ToString("dd/MMM/yyyy") : "",
                SepReason = employee.SeparationReason,
                SepEmpName = employee.FirstName,
                // Modified by Babu.R as on 24-Jul-2017 for Separaion last working date validation
                SepMonthlyLastWorkingDate = employee.MonthlyInputLastDate != DateTime.MinValue ? employee.MonthlyInputLastDate.ToString("dd/MMM/yyyy") : "",
                SepPayrollLastWorkingDate = employee.PayrollInputLastDate != DateTime.MinValue ? employee.PayrollInputLastDate.ToString("dd/MMM/yyyy") : "",
                SepMonthlyDate = employee.MonthlyInputDate != DateTime.MinValue ? employee.MonthlyInputDate.ToString("dd/MMM/yyyy") : "",
                SepPayrollDate = employee.PayrollInputDate != DateTime.MinValue ? employee.PayrollInputDate.ToString("dd/MMM/yyyy") : "",

            };
        }
        public static Employee convertobject(jsonSeparation employee)
        {
            return new Employee()
            {
                CategoryId = employee.SepCatid,
                Id = employee.SepEmpId,
                DateOfJoining = employee.SepDOJ != string.Empty ? Convert.ToDateTime(employee.SepDOJ) : DateTime.Now,
                TypeOfSeparation = employee.SepType,
                LastWorkingDate = employee.SepLWDate != string.Empty ? Convert.ToDateTime(employee.SepLWDate) : DateTime.Now,
                SeparationReason = employee.SepReason
            };
        }

    }
    public class jsonRelease
    {
        public Guid RelCatid { get; set; }
        public Guid RelEmpId { get; set; }
        public string RelEmpName { get; set; }
        public string SepType { get; set; }
        public string SepDate { get; set; }
        public string SepReason { get; set; }
        public string RelDate { get; set; }


        public static jsonRelease tojson(Employee employee)
        {
            return new jsonRelease()
            {
                RelCatid = employee.CategoryId,
                RelEmpId = employee.Id,
                SepType = employee.TypeOfSeparation,
                SepDate = employee.LastWorkingDate != DateTime.MinValue ? employee.LastWorkingDate.ToString("dd/MMM/yyyy") : "",
                SepReason = employee.SeparationReason,
                RelEmpName = employee.FirstName,
                RelDate = employee.ReleaseDate != DateTime.MinValue ? employee.ReleaseDate.ToString("dd/MMM/yyyy") : "",
            };
        }
        public static Employee convertobject(jsonRelease employee)
        {
            return new Employee()
            {
                CategoryId = employee.RelCatid,
                Id = employee.RelEmpId,
                ReleaseDate = employee.RelDate != string.Empty ? Convert.ToDateTime(employee.RelDate) : DateTime.Now,
            };
        }

    }
    public class jsonActiveEmployee
    {
        public int count { get; set; }
        public int year { get; set; }


        public static jsonActiveEmployee tojson(Employee employee)
        {
            return new jsonActiveEmployee()
            {
                count = employee.count,
                year = employee.year
            };
        }
        public static Employee convertobject(jsonActiveEmployee employee)
        {
            return new Employee()
            {
                count = employee.count,
                year = employee.year
            };
        }

    }

    public class jsonEmpBank
    {
        public Guid empBankid { get; set; }
        public Guid empBankEmployeeId { get; set; }
        public Guid bankId { get; set; }
        public string ifsc { get; set; }
        public string acctno { get; set; }
        public string branchName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }


        public static jsonEmpBank tojson(Emp_Bank empBank)
        {
            return new jsonEmpBank()
            {
                empBankid = empBank.Id,
                empBankEmployeeId = empBank.EmployeeId,
                bankId = empBank.BankId,
                ifsc = empBank.IFSC,
                acctno = empBank.AcctNo,
                branchName = empBank.BranchName,
                address = empBank.Address,
                city = empBank.City,
                state = empBank.State,



            };
        }
        public static Emp_Bank convertobject(jsonEmpBank empBank)
        {
            return new Emp_Bank()
            {
                Id = empBank.empBankid,
                EmployeeId = empBank.empBankEmployeeId,
                BankId = empBank.bankId,
                IFSC = empBank.ifsc,
                City = empBank.city,
                State = empBank.state,
                Address = empBank.address,
                AcctNo = empBank.acctno,
                BranchName = empBank.branchName

            };
        }


    }

    public class jsonEmpPersonalDetails
    {
        public Guid empPersonalid { get; set; }
        public Guid employeeId { get; set; }

        public string personalmobileno { get; set; }
        public string officemobileno { get; set; }
        public string extensionno { get; set; }
        public string personalmail { get; set; }
        public string officemail { get; set; }
        public string empfathername { get; set; }

        public int bloodgroup { get; set; }
        public bool isprintcheque { get; set; }

        public bool iseniorcitizen { get; set; }
        public string payslipremarks { get; set; }
        public bool isdisable { get; set; }
        public string maritalstatus { get; set; }
        public string spousename { get; set; }
        public int noofchildren { get; set; }
        public string pfnumber { get; set; }
        public string pfconfirmationdate { get; set; }
        public string pfuan { get; set; }
        public string pannumber { get; set; }
        public string esinumber { get; set; }
        public string adharnumber { get; set; }
        public int PensionEligible { get; set; }


        public static jsonEmpPersonalDetails tojson(Emp_Personal empBank)
        {
            return new jsonEmpPersonalDetails()
            {
                empPersonalid = empBank.Id,
                employeeId = empBank.EmployeeId,
                personalmobileno = empBank.PersonalMobileNo,
                officemobileno = empBank.OfficeMobileNo,
                extensionno = empBank.ExtensionNo,
                officemail = empBank.OfficeEmail,
                personalmail = empBank.PersonalEmail,
                bloodgroup = empBank.BloodGroup,
                isprintcheque = empBank.PrintCheque,
                iseniorcitizen = empBank.IsSeniorCitizen,
                isdisable = empBank.IsDisable,
                payslipremarks = empBank.PaySlipRemarks,
                empfathername = empBank.FatherName,
                maritalstatus = empBank.MaritalStatus,
                spousename = empBank.SpouseName,
                noofchildren = empBank.NoOfChildren,
                pfnumber = empBank.PFNumber,
                pfconfirmationdate = empBank.PFConfirmationDate == null || empBank.PFConfirmationDate == DateTime.MinValue ? "" : empBank.PFConfirmationDate.ToString("dd/MMM/yyyy"),
                pfuan = empBank.PFUAN,
                pannumber = empBank.PANNumber,
                esinumber = empBank.ESINumber,
                adharnumber = empBank.AADHARNumber,
                PensionEligible = empBank.PensionEligible

            };
        }
        public static Emp_Personal convertobject(jsonEmpPersonalDetails empBank)
        {
            return new Emp_Personal()
            {
                Id = empBank.empPersonalid,
                EmployeeId = empBank.employeeId,
                PersonalMobileNo = empBank.personalmobileno,
                OfficeMobileNo = empBank.officemobileno,
                ExtensionNo = empBank.extensionno,
                PersonalEmail = empBank.personalmail,
                OfficeEmail = empBank.officemail,
                BloodGroup = empBank.bloodgroup,
                IsDisable = empBank.isdisable,
                IsSeniorCitizen = empBank.iseniorcitizen,
                PrintCheque = empBank.isprintcheque,
                PaySlipRemarks = empBank.payslipremarks,
                FatherName = empBank.empfathername,
                MaritalStatus = empBank.maritalstatus,
                SpouseName = empBank.spousename,
                NoOfChildren = empBank.noofchildren,
                PFNumber = empBank.pfnumber,
                PFConfirmationDate = empBank.pfconfirmationdate != null && empBank.pfconfirmationdate != string.Empty ? Convert.ToDateTime(empBank.pfconfirmationdate) : DateTime.MinValue,
                PFUAN = empBank.pfuan,
                PANNumber = empBank.pannumber,
                ESINumber = empBank.esinumber,
                AADHARNumber = empBank.adharnumber,
                PensionEligible = empBank.PensionEligible
            };
        }


    }
    //Created BY Madhavan On 27/09/23
    public class JsonExpenseEntry
    {
        public Guid Id { get; set; }
        public string EmpID { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterMgr { get; set; }
        public string PurposeForExpense { get; set; }
        public string CategeroyOfExpense { get; set; }
        public string DescriptExpense { get; set; }
        public DateTime DateOfExpense { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Status { get; set; }
        public string Attachment { get; set; }
        public string CostOfExpense { get; set; }
    }
    public class JsonAssignMgr
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
        public string ExpenseStatus { get; set; }
        public string ModifiedBy { get; set; }
        public string ApprovMustString { get; set; }
        public string AppCancelRightString { get; set; }
        public string Email { get; set; }
        public Guid AssEmpId { get; set; }
        public string firstlevelData { get; set; }
        public static JsonAssignMgr toJson(ExpenseAssignMgr AssMgr)
        {
            return new JsonAssignMgr()
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
                ExpenseStatus = AssMgr.ExpenseStatus,
                Email = AssMgr.Email,
                ApprovMustString = AssMgr.ApprovMustString,
                AppCancelRightString = AssMgr.AppCancelRightString,
                firstlevelData = AssMgr.MgrEmpCode + "-" + AssMgr.MgrEmpName
            };
        }
    }
}
