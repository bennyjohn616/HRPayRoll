using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using System.IO;
using TraceError;
using SystemWindowsFile;
using NotificationEngine;
using System.Configuration;
using System.Data;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    public class AdminController : BaseController//Controller
    {
        // private object roleName;

        //
        // GET: /Loan/

        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            return View();
        }

        // GET: /File/
        //public ActionResult Index(int id)
        //{

        //    var fileToRetrieve = db.Files.Find(id);
        //    return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        //}
        public JsonResult GetFormRights()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            companyId = 1;
            FormsList Forms = new FormsList(true);
            return new JsonResult { Data = Forms };
        }

        // Modified By Keerthika on 13/05/2017
        public JsonResult GetUser()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["UserId"]);
            int Id = 0;
            UserList User = new UserList(0);
            UserList newuser = new UserList();
            RoleList Role = new RoleList(Id, companyId);

            //  PayrollBO.UserCompanymappingList loguser = new UserCompanymappingList(userid);

            User.ForEach(u =>
            {
                var role = Role.Where(r => r.Id == u.UserRole).FirstOrDefault();
                if (role != null)
                {
                    u.RoleName = role.Name;
                }
                var checkUser = u.userCompanyMapping.Where(r => r.CompanyId == companyId).FirstOrDefault();
                if (checkUser != null)
                {
                    newuser.Add(u);
                }
            });
            return new JsonResult { Data = newuser };
        }
        //-------------
        public JsonResult GetAllUser(int companyId)

        {
            // int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["UserId"]);
            int Id = 0;
            UserList User = new UserList(0);
            UserList newuser = new UserList();
            RoleList Role = new RoleList(Id, companyId);

            //  PayrollBO.UserCompanymappingList loguser = new UserCompanymappingList(userid);


            User.ForEach(u =>
            {
                var role = Role.Where(r => r.Id == u.UserRole).FirstOrDefault();
                if (role != null)
                {
                    u.RoleName = role.Name;
                }
                //var checkUser = u.userCompanyMapping.Where(r => r.CompanyId == companyId).FirstOrDefault();
                //if (checkUser != null)
                //{
                newuser.Add(u);
                //}

            });

            return new JsonResult { Data = newuser };
        }

        //Modified by Keerthika on 09/06/2017
        public JsonResult GetRole()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string roleName = Convert.ToString(Session["RoleName"]);
            int id = 0;
            RoleList Role = new RoleList(id, companyId);
            RoleList rolelist = new RoleList();
            RoleList rolel = new RoleList();
            var rolelis = Role;// Role.Where(r => r.Name != "Employee").ToList();
            rolelist.AddRange(rolelis);
            if (roleName != "Admin")
            {
                var role = Role.Where(r => r.Name != "Admin" && r.Name != "Employee").ToList();
                rolel.AddRange(role);
                return new JsonResult { Data = rolel };
            }


            return new JsonResult { Data = rolelis };
        }


        public JsonResult GetFULLRoleList()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string roleName = Convert.ToString(Session["RoleName"]);
            int id = 0;
            RoleList Role = new RoleList(id, companyId);
            RoleList rolelist = new RoleList();
            RoleList rolel = new RoleList();
            if (roleName != "Admin")
            {
                var role = Role.Where(r => r.Name != "Admin").ToList();
                rolel.AddRange(role);
                return new JsonResult { Data = rolel };
            }


            return new JsonResult { Data = Role };
        }

        public JsonResult GetFormData()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            FormsList Forms = new FormsList(true);
            //return new JsonResult { Data = Forms };
            return base.BuildJson(true, 200, "success", Forms);
        }
        public JsonResult getuserrole()
        {
            int CompanyId = Convert.ToInt32(Session["CompanyId"]);

            UserCompanymappingList UserCompanymappingList = new UserCompanymappingList(CompanyId, Guid.Empty);
            //return new JsonResult { Data = Forms };
            return base.BuildJson(true, 200, "success", UserCompanymappingList);
        }
        public JsonResult getCompanyrole()
        {
            EmployeeRole objEMPROLE = new EmployeeRole();
            int CompanyId = Convert.ToInt32(Session["CompanyId"]);
            //UserCompanymappingList UserCompanymappingList = new UserCompanymappingList(Guid.Empty,CompanyId);

            //getting the user company mapping list 
            UserCompanymappingList CompList = new UserCompanymappingList(0);

            //adding the where condition in the list and getting only the users belongs to this company 
            var usercomp = CompList.Where(d => d.CompanyId == CompanyId).ToList();
            List<EmployeeRole> Lst = new List<EmployeeRole>();
            Lst = objEMPROLE.role(CompanyId, 0);


            //DataTable dtRolSel = new DataTable();
            //UserCompanymapping ObjCompMap = new UserCompanymapping();
            //dtRolSel = ObjCompMap.GetPayrolRole();
            //List<jsonUserCompMap> jsonCompRol = new List<jsonUserCompMap>();
            //UserCompanymappingList.ForEach(u => { jsonCompRol.Add(jsonUserCompMap.tojson(u)); });        
            return base.BuildJson(true, 200, "success", Lst);
        }

        public JsonResult GetRoleData(Role dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Role role = new Role(dataValue.Id, companyId);
            return base.BuildJson(true, 200, "success", role);
        }

        public JsonResult SaveRoleData(Role dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            //dataValue.Id(Convert.ToInt32(Session["UserId"]));
            dataValue.CompanyId = companyId;
            //  dataValue.Id = userId;
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
        //Modified By Keerthika on 03/05/2017 
        public JsonResult SaveRightsData(FormRights dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            dataValue.CompanyId = companyId; //---
            int userId = Convert.ToInt32(Session["UserId"]);

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
        public JsonResult UpdateFormCommands(FormCommand dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            int userId = Convert.ToInt32(Session["UserId"]);

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
        public JsonResult DeleteRoleData(Role datavalue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            Role Role;
            Role = new Role(datavalue.Id, datavalue.CompanyId);
            UserList User = new UserList(Role.Id, 0);
            if (User.Count == 0)
            {
                isDeleted = Role.Delete();
                if (isDeleted)
                {
                    return base.BuildJson(true, 200, "Data deleted successfully", datavalue);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while Deleting the data.", datavalue);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "You can't Delete this Role,It is still assigned to employees", datavalue);
            }



        }

        public JsonResult EmailCheck(User dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            UserList User = new UserList(0);
            var useremail = User.Where(d => (d.Email.ToLower().Trim() == dataValue.Email.ToLower().Trim() && d.Id != dataValue.Id)).ToList();
            if (useremail.Count == 0)
            {
                return base.BuildJson(true, 200, string.Empty, true);
            }
            else
            {
                return base.BuildJson(false, 200, "Email ID is already Exist.", false);
            }

            //employeeList.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });

        }



        // Modified by Keerthika on 23/05/2017
        public JsonResult SaveUserData(User userValue)//, HttpPostedFileBase[] File1)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var userrole = userValue.UserRole;
            var company = userValue.userCompanyMappingset;
            // UserCompanymappingList usermappinglist = new UserCompanymappingList(userId);
            // UserList userlist = new UserList(0);        
            Employee employee = new Employee(userValue.EmployeeId);
            if (employee.SeparationDate != DateTime.MinValue && userValue.Id==0)
            {
                return base.BuildJson(false, 100, "Separated employees can't create a new user.", userValue);
            }
            else
            {
                UserList user = new UserList(0);

                FileLibrary objPwd = new FileLibrary();

                var oldPass = userValue.Password;

                userValue.CreatedBy = userId;
                userValue.ModifiedBy = userId;
                var mailTo = userValue.Email;
                var userVar1 = user.Where(ul => ul.Id == userValue.Id).FirstOrDefault();
                if (userVar1 != null && userVar1.Id == userValue.Id)
                {
                    //userValue.Password = userValue.Password;

                    User user1 = new User(userValue.Id);
                    user1.userCompanyMapping = user1.userCompanyMapping;
                    user1.Password = objPwd.defile(user1.Password);
                    if (user1.Password == userValue.Password)
                    {
                        userValue.IsForget = false;
                    }
                    else
                    {
                        userValue.IsForget = true;
                    }
                    userValue.Password = objPwd.enfile(userValue.Password);

                }
                else
                {
                    userValue.Password = objPwd.enfile(userValue.Password);
                    userValue.IsForget = true;
                }
                //string activationUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString() + @"/Login/Index";

                //string message = "<p>Hello User, </br> Your UserName: <b>" + userValue.Username + "</b>,</br> Your  Password: <b>" + oldPass + "</b> <div>";
                //message = message + "<div> <a href='" + activationUrl + "'>Click here to Login</a> <div>";
                //string subject = "New UserName and Password";
                //PayRoleMail payrolemail = new PayRoleMail(mailTo, subject, message);
                if (userrole != 0 && company != null)
                {
                   
                    userValue.DBConnectionId = employee.DBConnectionId==0?Convert.ToInt32(Session["DBConnectionId"]): employee.DBConnectionId;
                    userValue.CompanyId = employee.CompanyId== 0?companyId: employee.CompanyId;
                    if (company.Count != 0)

                        isSaved = userValue.Save();
                }
                else
                {

                    var errmsg = string.Empty;
                    errmsg = (userrole == 0 ? "Please Select The User Role" : "Please Select your company.");
                    return base.BuildJson(false, 100, errmsg, userValue);
                }
                // string[] compsplit;
                // company.TrimEnd(',');
                //  compsplit = company.TrimEnd(',').Split(',');
                if (isSaved)
                {
                    var userVar = user.Where(ul => ul.Id == userValue.Id).FirstOrDefault();
                    if (userVar != null && userVar.Id == userValue.Id)
                    {
                        // status = payrolemail.Send();
                    }
                    else
                    {

                        //status = payrolemail.Send();
                    }
                    UserCompanymappingList usermappinglist = new UserCompanymappingList(userValue.Id);

                    usermappinglist.ForEach(um =>
                    {

                        UserCompanymapping usermapping = new UserCompanymapping();
                        usermapping.UserId = userValue.Id;
                        usermapping.Delete();

                    });

                    userValue.userCompanyMappingset.ForEach(uc =>
                    {
                        UserCompanymapping usermapping = new UserCompanymapping();
                        usermapping.UserId = userValue.Id;

                        usermapping.CompanyId = uc.CompanyId;
                        usermapping.RightsOn = uc.RightsOn;
                        usermapping.RightsOnValue = uc.RightsOnValue;
                        usermapping.Save();


                    });

                    int id = userValue.Id;

                    if (userValue.ProfileImage == null)
                    {
                        userValue.ProfileImage = !string.IsNullOrEmpty(employee.EmployeeImage) ? employee.EmployeeImage.Trim() : ""; 
                        userValue.Save();
                    }
                    else
                    {
                        if (userValue.ProfileImage.Contains("/Temp/"))
                        {
                            Guid tempId;
                            if (Guid.TryParse(userValue.ProfileImage.Replace("/Temp/", ""), out tempId))
                            {
                                if (tempId != Guid.Empty)
                                {
                                    string strRelationPath = "~/CompanyData/" + companyId + "/Temp/" + tempId;

                                    var path = Path.Combine(Server.MapPath(strRelationPath));
                                    if (System.IO.Directory.Exists(path))
                                    {
                                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                                        FileInfo[] files = dirInfo.GetFiles();
                                        if (files.Length > 0)
                                        {

                                            string filePath = "~/CompanyData/" + companyId + "/User/" + id + "/" + files[0].Name;
                                            string destName = Server.MapPath(filePath);
                                            if (!Directory.Exists(destName))
                                            {
                                                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destName));
                                            }
                                            System.IO.File.Copy(files[0].FullName, destName, true);
                                            //files[0].MoveTo(destName);
                                            userValue.ProfileImage = filePath;
                                            userValue.Save();

                                            string tmp = filePath;
                                            employee.EmployeeImage = tmp.Trim().Replace("~/", String.Empty);
                                            employee.Save();
                                            System.IO.Directory.Delete(path, true);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    return base.BuildJson(true, 200, "Data saved successfully", userValue);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", userValue);
                }
            }
        }
        //Created by Keerthika on 07/06/2017
        public JsonResult SaveUserEmpData(User dataValue, HttpPostedFileBase[] File1)
        {
            Employee employee = new Employee(dataValue.EmployeeId);

            int companyId = dataValue.userCompanyMapping[0].CompanyId;
            bool isSaved = false;


            // UserCompanymappingList usermappinglist = new UserCompanymappingList(userId);
            FileLibrary objPwd = new FileLibrary();
            UserList user = new UserList(0);
            RoleList roles = new RoleList(0, companyId);

            dataValue.Password = objPwd.enfile(dataValue.Password);
            dataValue.FirstName = employee.FirstName;
            dataValue.LastName = employee.LastName;
            dataValue.Email = employee.Email;
            dataValue.CreatedBy = employee.CreatedBy;
            dataValue.ModifiedBy = employee.ModifiedBy;
            dataValue.DBConnectionId = employee.DBConnectionId;
            var role = roles.Where(r => r.Name == "Employee").FirstOrDefault();
            if (role != null)
            {
                dataValue.UserRole = role.Id;

            }

            //var userEmp=  user.Where(u => u.EmployeeId == dataValue.EmployeeId).FirstOrDefault();
            // if (userEmp!=null && userEmp.EmployeeId!=dataValue.EmployeeId)
            //  { 

            isSaved = dataValue.Save();
            //}
            //else if()
            //{
            //    isSaved = dataValue.Save();
            //}
            // string[] compsplit;
            // company.TrimEnd(',');
            //  compsplit = company.TrimEnd(',').Split(',');
            if (isSaved)
            {

                UserCompanymappingList usermappinglist = new UserCompanymappingList(dataValue.Id);

                usermappinglist.ForEach(um =>
                {

                    UserCompanymapping usermapping = new UserCompanymapping();
                    usermapping.UserId = dataValue.Id;
                    usermapping.Delete();

                });


                // int i = dataValue.userCompanyMapping.Count();
                //    PayrollBO.UserCompanymappingList mappinglist = new PayrollBO.UserCompanymappingList(userId);
                //----
                //   for (int i = 0; i < compsplit.Length; i++)




                //  usermapping.CompanyId = Convert.ToInt32(compsplit[i]);

                dataValue.userCompanyMapping.ForEach(uc =>
                {
                    UserCompanymapping usermapping = new UserCompanymapping();
                    usermapping.UserId = dataValue.Id;

                    usermapping.CompanyId = uc.CompanyId;
                    usermapping.RightsOn = uc.RightsOn;
                    usermapping.RightsOnValue = uc.RightsOnValue;
                    usermapping.Save();


                });


                int id = dataValue.Id;
                if (dataValue.ProfileImage == null)
                {

                }
                else
                {
                    if ((dataValue.ProfileImage.Contains("/Temp/")))
                    {
                        Guid tempId;
                        if (Guid.TryParse(dataValue.ProfileImage.Replace("/Temp/", ""), out tempId))
                        {
                            if (tempId != Guid.Empty)
                            {
                                string strRelationPath = "~/CompanyData/" + companyId + "/Temp/" + tempId;

                                var path = Path.Combine(Server.MapPath(strRelationPath));
                                if (System.IO.Directory.Exists(path))
                                {
                                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                                    FileInfo[] files = dirInfo.GetFiles();
                                    if (files.Length > 0)
                                    {

                                        string filePath = "~/CompanyData/" + companyId + "/User/" + id + "/" + files[0].Name;
                                        string destName = Server.MapPath(filePath);
                                        if (!Directory.Exists(destName))
                                        {
                                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destName));
                                        }
                                        System.IO.File.Copy(files[0].FullName, destName, true);
                                        //files[0].MoveTo(destName);
                                        dataValue.ProfileImage = filePath;
                                        dataValue.Save();
                                        Employee employe = new Employee(dataValue.EmployeeId);
                                        string tmp = filePath;
                                        employe.EmployeeImage = tmp.Trim().Replace("~/", String.Empty);
                                        employe.Save();
                                        System.IO.Directory.Delete(path, true);
                                    }
                                }
                            }
                        }

                    }
                }
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }
        public JsonResult GetUserData(int id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            FileLibrary objPwd = new FileLibrary();
            User user = new User(id);
            user.userCompanyMapping = user.userCompanyMapping;
            user.Password = objPwd.defile(user.Password);
            user.ConfirmPassword = objPwd.defile(user.Password);
            return base.BuildJson(true, 200, "success", user);
        }
        public JsonResult GetRegisterData(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            User user = new User(id);
            return base.BuildJson(true, 200, "success", user);
        }

        public JsonResult DeleteUserData(int id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            User data = new User(id);
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            User User;
            User = new User(id);
            User.CreatedBy = userId;
            User.ModifiedBy = User.CreatedBy;
            User.IsDeleted = true; // Modified by 

            isDeleted = User.Delete();

            if (isDeleted)
            {

                return base.BuildJson(true, 200, "Data deleted successfully", data);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", data);
            }

        }
        public JsonResult GetFormRightsData(string role)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            FormsList formlist = new FormsList(true);
            FormRightsList fromRightsList = new FormRightsList(role);
            var unavailableForm = formlist.Where(u => !fromRightsList.Any(v => v.FormId == u.Id)).ToList();
            List<jsonFromRights> jsondata = new List<jsonFromRights>();
            fromRightsList.ForEach(u => { jsondata.Add(jsonFromRights.tojson(u, formlist)); });
            unavailableForm.ForEach(u => { jsondata.Add(new jsonFromRights() { formid = u.Id, formName = u.DisplayAs, delete = false, edit = false, view = false }); });

            return base.BuildJson(true, 200, "success", jsondata);



        }
        public JsonResult GetFormCommandsData(string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            FormCommandList frmCmd = new FormCommandList(type);

            return base.BuildJson(true, 200, "success", frmCmd);

        }

        public JsonResult SaveUserImage()
        {
            //if (!base.checkSession())
            //    return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
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
                        var fileName = Path.GetFileName(file);
                        fileName = fileName.Replace("_", "").Replace("-", "").Replace(" ", "");
                        string strRelationPath = "~/CompanyData/" + companyId + "/Temp/" + tempId + "/" + fileName;
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
                return base.BuildJson(true, 200, "User Profile image has been saved.", "/Temp/" + tempId);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while saving the file.", "/Temp/");
            }

        }


        //---new---- created by Keerthika on 31/05/2017
        public JsonResult GetCategoriesComp(int companyId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            // int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            CategoryList comp = new CategoryList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "CategoryId" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return base.BuildJson(true, 200, "Success", comp);
            }
            // return new JsonResult { Data = comp };
        }
        //---
        //---- created by keerthika
        public JsonResult GetDesignationsComp(int companyId)
        {
            // int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            DesignationList comp = new DesignationList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "Designation" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //----
        public JsonResult GetPTLocationsComp(int companyId)
        {
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            PTLocationList comp = new PTLocationList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "PTLocation" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //----created 
        public JsonResult GetCostcentresComp(int companyId)

        {
            //  int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            CostCentreList comp = new CostCentreList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "CostCentre" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //---
        //-----
        public JsonResult GetBranchesComp(int companyId)
        {
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            BranchList comp = new BranchList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "Branch" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //--
        public JsonResult GetESIDespensarysComp(int companyId)
        {
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            ESIDespensaryList comp = new ESIDespensaryList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "ESIDespensary" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //---
        public JsonResult GetDepartmentsComp(int companyId)
        {
            //  int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            DepartmentList comp = new DepartmentList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "Department" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //---
        public JsonResult GetLocationComp(int companyId)
        {
            // int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            LocationList comp = new LocationList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "Location" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //--
        public JsonResult GetGradesComp(int companyId)
        {
            //     int companyId = Convert.ToInt32(Session["CompanyId"]);
            GradeList comp = new GradeList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "Grade" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }
        //----
        public JsonResult GetBanks()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            BankList bankList = new BankList(companyId);
            return new JsonResult { Data = bankList };
        }
        public JsonResult GetEsilocationComp(int companyId)
        {
            //   int companyId = Convert.ToInt32(Session["CompanyId"]);
            EsiLocationList comp = new EsiLocationList(companyId);
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "ESILocation" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return new JsonResult { Data = comp };
            }
        }

    }
    public class jsonFromRights
    {
        public Guid formid { get; set; }
        public string formName { get; set; }
        public bool view { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }

        public static jsonFromRights tojson(FormRights formrights, FormsList formlist)
        {
            return new jsonFromRights()
            {
                formid = formrights.FormId,
                formName = formlist.Where(u => u.Id == formrights.FormId).FirstOrDefault().Name,
                view = formrights.ViewRights,
                edit = formrights.EditRights,
                delete = formrights.DeleteRights
            };
        }

    }
    public class jsonUserCompMap
    {
        public string Displayas { get; set; }

        public string FirstName { get; set; }

        public string EmployeeCode { get; set; }
        public Guid Id { get; set; }
        public int RoleID { get; set; }

        public int UserId { get; set; }

        public static jsonUserCompMap tojson(UserCompanymapping ObjUsercompmap)
        {
            return new jsonUserCompMap()
            {
                Id = ObjUsercompmap.Id,
                UserId = ObjUsercompmap.UserId,
                FirstName = ObjUsercompmap.FirstName,
                EmployeeCode = ObjUsercompmap.EmployeeCode,
                Displayas = ObjUsercompmap.Displayas,
                RoleID = ObjUsercompmap.RoleId
            };
        }

    }
}
