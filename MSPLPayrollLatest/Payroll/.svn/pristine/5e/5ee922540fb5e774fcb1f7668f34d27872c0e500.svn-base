using Payroll.CustomFilter;
using PayrollBO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //  Session["CompanyId"] = null;
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            Company comp = new Company();
            comp = comp.singleCompanyDetails(Convert.ToInt32(Session["CompanyId"]));
            Session["CompLogo"] = !string.IsNullOrEmpty(comp.Companylogo) ? comp.Companylogo.Replace("~/", "") : "assets/images/logo.png";
            ViewBag.CompanyName = comp.CompanyName;
            ViewBag.TempTitle = Session["Title"];
            ViewBag.userProfileImage = Session["userProfileImage"];// TempData["userProfileImage"];
            if (!object.ReferenceEquals(ViewBag.userProfileImage, null))
                ViewBag.userProfileImage = Convert.ToString(ViewBag.userProfileImage).Replace("~", "");
            string roleName = Convert.ToString(Session["RoleName"]);
            PayrollBO.UserCompanymappingList loguser = new PayrollBO.UserCompanymappingList(Convert.ToInt32(Session["UserId"]));

            if (roleName.ToLower() == "superadmin")
            {
                return View("~/Views/SuperAdmin/SA_CompanyList.cshtml");
            }
            if (loguser.Count > 1)
            {
                return View("~/Views/Company/CompanyList.cshtml");
            }

            if (!object.ReferenceEquals(Session["CompanyId"], null) && Convert.ToInt32(Session["CompanyId"]) != 0)
            {
                return View("~/Views/Company/CompanyView.cshtml");
            }

            return View("~/Views/Company/CompanyList.cshtml");
        }
        public ActionResult LeaveIndex()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));
            ViewBag.CompanyName = comp.CompanyName;
            ViewBag.TempTitle = Session["Title"];
            ViewBag.userProfileImage = Session["userProfileImage"];// TempData["userProfileImage"];
            if (!object.ReferenceEquals(ViewBag.userProfileImage, null))
                ViewBag.userProfileImage = ViewBag.userProfileImage == "" ? "" : ("../" + Convert.ToString(ViewBag.userProfileImage).Replace("~", ""));
            string roleName = Convert.ToString(Session["RoleName"]);
            if (roleName == "Admin" && (!object.ReferenceEquals(Session["CompanyId"], null) && Convert.ToInt32(Session["CompanyId"]) != 0))
            {
                return View("~/Views/Company/LeaveCalendar.cshtml");

            }
            else if (!object.ReferenceEquals(Session["CompanyId"], null) && Convert.ToInt32(Session["CompanyId"]) != 0)
            {
                return View("~/Views/Leave/EmployeeLeavedashboard.cshtml");
            }
            return View("~/Views/Company/CompanyList.cshtml");
        }

        public ActionResult LeaveCalendarIndex()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));
            ViewBag.CompanyName = comp.CompanyName;
            ViewBag.TempTitle = Session["Title"];
            ViewBag.userProfileImage = Session["userProfileImage"];// TempData["userProfileImage"];
            if (!object.ReferenceEquals(ViewBag.userProfileImage, null))
                ViewBag.userProfileImage = Convert.ToString(ViewBag.userProfileImage).Replace("~", "");
            string roleName = Convert.ToString(Session["RoleName"]);
            ViewBag.date = TempData["date"];
            ViewBag.id = TempData["id"];
            if (!object.ReferenceEquals(Session["CompanyId"], null) && Convert.ToInt32(Session["CompanyId"]) != 0)
            {
                return View("~/Views/Leave/LeaveCalendarDetails.cshtml");
            }
            return View("~/Views/Company/CompanyList.cshtml");
        }
        public ActionResult TaxIndex()
        {

            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            ViewBag.userProfileImage = Session["userProfileImage"];// TempData["userProfileImage"];
            return View("~/Views/Home/TaxIndex.cshtml");
        }

        public ActionResult LoadCompany(int id = 0)
        {
            Session["CompanyId"] = id;
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            else
                return RedirectToAction("Index", "Company");
        }

        //Modified by Keerthika on 18/05/2017
        public ActionResult GetUserFormrights(int roleId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            //formRights.Add(new jsonUserFormRights() { canDelete = false, canEdit = true, canRead = true, canVisible = true, formName = "Employee" });
            //formRights.Add(new jsonUserFormRights() { canDelete = false, canEdit = true, canRead = true, canVisible = true, formName = "Company" });
            //formRights.Add(new jsonUserFormRights() { canDelete = false, canEdit = true, canRead = true, canVisible = true, formName = "Category" });
            //formRights.Add(new jsonUserFormRights() { canDelete = false, canEdit = true, canRead = true, canVisible = true, formName = "PopUp" });
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string userName = Convert.ToString(Session["UserName"]);
            int Id = Convert.ToInt32(Session["UserRole"]);
            string roleName = Convert.ToString(Session["RoleName"]);
            //  int userRole = Convert.ToInt32(Session["UserRole"]);//----
            FormCommandList formCommandlist = new FormCommandList(true);
            User userobj = new PayrollBO.User();
            DataTable dtModule = new DataTable();

            dtModule = userobj.GetUserDBconnectionValues(Convert.ToInt32(Session["DBConnectionId"]));
            if (dtModule.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtModule.Rows[0]["Payroll"]) == false)
                    formCommandlist.RemoveAll(f => f.ModuleType.ToLower() == "payroll");
                if (Convert.ToBoolean(dtModule.Rows[0]["ITax"]) == false)
                    formCommandlist.RemoveAll(f => f.ModuleType.ToLower() == "itax");
                if (Convert.ToBoolean(dtModule.Rows[0]["Leave"]) == false)
                    formCommandlist.RemoveAll(f => f.ModuleType.ToLower() == "leave");
                //if (Convert.ToBoolean(dtModule.Rows[0]["TimeOffice"]) == false)
                //    formCommandlist.RemoveAll(f => f.ModuleType.ToLower() == "timeoffice");
            }

            RoleFormCommandList roleFormCommand = new RoleFormCommandList(companyId, Id, 0);
            if (roleFormCommand.Count == 0 && Id != 1)
            {
                roleFormCommand = new RoleFormCommandList(companyId, 2, 0);
                Id = 2;
            }


            if (roleFormCommand.Count == 0)
            {
                var result = formCommandlist.Where(p => !roleFormCommand.Any(p2 => p2.FormCommandId == p.Id)).ToList();
                result.ForEach(u =>
               {
                   if ((roleName.ToLower() != "Employee") && (u.CommandName.Trim() == "EmployeeNew" || u.CommandName.Trim() == "PayslipNew" || u.CommandName.Trim() == "leaveRequest"))
                   {
                       roleFormCommand.Add(new RoleFormCommand() { CompanyId = companyId, FormCommandId = u.Id, RoleId = roleId, IsRead = false, IsWrite = false });
                   }
                   else   //roleFormCommand.Add(new RoleFormCommand() { CompanyId = companyId, FormCommandId = u.Id, RoleId = roleId, IsRead = true, IsWrite = true});
                       roleFormCommand.Add(new RoleFormCommand() { CompanyId = companyId, FormCommandId = u.Id });

               });

            }


            List<jsonUserFormRights> formRights = new List<jsonUserFormRights>();
            List<jsonUserFormRights> controlsRights = new List<jsonUserFormRights>();
            roleFormCommand.ForEach(f =>
            {
                var formcomm = formCommandlist.Where(p => p.Id == f.FormCommandId).FirstOrDefault();
                if (formcomm != null && (formcomm.CommandTypes.Trim() == "SubMenu" || formcomm.CommandTypes.Trim() == "Menu" || formcomm.CommandTypes.Trim() == "tab" || formcomm.CommandTypes.Trim() == "Module"))
                {
                    formRights.Add(jsonUserFormRights.tojson(f, formCommandlist, Id));
                }

            });

            roleFormCommand.ForEach(f =>
            {
                var formcomm = formCommandlist.Where(p => p.Id == f.FormCommandId).FirstOrDefault();
                if (formcomm != null && (formcomm.CommandTypes.Trim() == "Control" || formcomm.CommandTypes.Trim() == "tab" || formcomm.CommandTypes.Trim() == "Form"))
                {
                    controlsRights.Add(jsonUserFormRights.tojson(f, formCommandlist, Id));
                }

            });
            string moduleType = string.Empty;
            var topmenu = formRights.Where(x => x.CommandType.ToLower().Trim() == "module" && x.canVisible == true).OrderBy(x => x.DisplayOrder).ToList();
            if (!object.ReferenceEquals(Session["Title"], null))
            {
                moduleType = Convert.ToString(Session["Title"]);
            }
            else
            {
                moduleType = topmenu.Count > 0 ? topmenu[0].ModuleType : "";
            }
            moduleType = moduleType.ToLower() == "tax" ? "itax" : moduleType;
            var leftMenu = formRights.Where(x => x.ModuleType.ToLower() == (moduleType.ToLower()) && x.DisplayOrder > 0 && x.canVisible == true && (x.CommandType.Trim() == "SubMenu" || x.CommandType.Trim() == "Menu")).OrderBy(x => x.DisplayOrder).ToList();

            //Session["menus"] = new
            //{
            //    formRights = formRights,
            //    topMenus = topmenu,
            //    leftMenus = leftMenu
            //};

            return base.BuildJson(true, 200, "success", new
            {
                formRights = controlsRights,
                topMenus = topmenu,
                leftMenus = leftMenu
            });

        }

    }

    public class jsonUserFormRights
    {
        public string formName { get; set; }
        public bool canVisible { get; set; }

        // public bool canRead { get; set; }

        public bool canEdit { get; set; }

        public bool canDelete { get; set; }

        public string CommandType { get; set; }
        public string Description { get; set; }
        public string ParentMenu { get; set; }
        public string ModuleType { get; set; }

        public int Id { get; set; }

        public int DisplayOrder { get; set; }

        //Created by Keerthia on 19/05/2017
        public static jsonUserFormRights tojson(RoleFormCommand roleFormCommand, FormCommandList formCommandlist, int RoleId)
        {
            //  int Id = Convert.ToInt32();
            var formCmd = formCommandlist.Where(u => u.Id == roleFormCommand.FormCommandId).FirstOrDefault();
            if (roleFormCommand.RoleId == RoleId)
            {
                return new jsonUserFormRights()
                {

                    formName = formCmd.CommandName,
                    canVisible = roleFormCommand.IsRead,
                    canEdit = roleFormCommand.IsWrite,
                    canDelete = roleFormCommand.IsDelete,
                    CommandType = formCmd.CommandTypes,
                    Description = formCmd.Description,
                    Id = formCmd.Id,
                    ParentMenu = formCmd.ParentMenu,
                    ModuleType = formCmd.ModuleType,
                    DisplayOrder = formCmd.DisplayOrder
                };
            }
            else
            {
                return new jsonUserFormRights()
                {
                    formName = formCmd.CommandName,
                    canVisible = formCmd.IsDefaultRead,
                    canEdit = formCmd.IsDefaultWrite,
                    canDelete = roleFormCommand.IsDelete,
                    CommandType = formCmd.CommandTypes,
                    Description = formCmd.Description,
                    Id = formCmd.Id,
                    ParentMenu = formCmd.ParentMenu,
                    ModuleType = formCmd.ModuleType,
                    DisplayOrder = formCmd.DisplayOrder
                };
            }
        }
    }
}




