using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Models;
using System.Web.Security;
using SystemWindowsFile;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NotificationEngine;


namespace Payroll.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            Session.Clear();
            //Session["UserId"] = "1";
            //Session["RequestId"] = "";
            //Session["CompanyId"] = "1";
            //Session["UserSessionId"] = "1";
            return View();
        }
        public ActionResult ChangePass()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View("~/Views/Login/ChangePassword.cshtml");
            }
        }
        // Modified By Keerthika 
        // Modifeid on 12/05/2017
        [HttpPost]
        public ActionResult ValidateLogin(User user)

        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string ExpiryDatefrom = string.Empty;
            string ExpiryDateTo = string.Empty;


            int userId = Convert.ToInt32(Session["UserId"]);
            PayrollBO.User userObj = new PayrollBO.User();
            //   PayrollBO.UserList User = new PayrollBO.UserList(userId);
           

            //DataTable dt = userObj.GetProductDetails(user.UserName);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    ExpiryDatefrom = dt.Rows[0]["ExpiryDateFrom"].ToString();
            //    ExpiryDateTo = dt.Rows[0]["ExpiryDateTo"].ToString();
            //}

            /*
            if (user.UserName == "admin" && user.Password == "admin")
            {
                // PayrollBO.User userLog = new PayrollBO.User(user.UserName, user.Password);
                Session["UserId"] = "1";
                Session["RequestId"] = "";
                Session["UserSessionId"] = "1";
                // ViewBag.userProfileImage = "../../CompanyData/1/User/0/DSC5965.JPG";
                Session["userProfileImage"] = "../../assets/images/profile.png";// "../../CompanyData/1/User/0/DSC5965.JPG";
                
                //to save the login History
                PayrollBO.LoginHistory logHistory = new PayrollBO.LoginHistory();
                logHistory.BrowserInfo = HttpContext.Request.Browser.Browser;
                logHistory.UserHost = HttpContext.Request.UserHostAddress;
                logHistory.UserId = Convert.ToInt32(Session["UserId"]);
                logHistory.SessionId = HttpContext.Request.RequestContext.HttpContext.Session.SessionID;
                logHistory.Save();
                

             return BuidJsonResult(true, Url.Action("Index", "Home"));////Company


        }*/
            FileLibrary objPwd = new FileLibrary();
            user.Password = objPwd.enfile(user.Password);
            ExpiryDateTo = objPwd.defile(ExpiryDateTo);
            //Guid employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));

            if (userObj.ValidateLogin(user.UserName, user.Password) == "Success")
            {
                PayrollBO.LoginHistory logHistoryTemp = new PayrollBO.LoginHistory(userObj.Id);
                //if (!string.IsNullOrEmpty(logHistoryTemp.SessionId))
                //{
                //    PayrollBO.LoginHistory logHistoryTemp = new PayrollBO.LoginHistory(userObj.Id);
                if (!string.IsNullOrEmpty(logHistoryTemp.SessionId))
                {
                    //already logged in so we should not allow the user to login again
                    //    return BuidJsonResult(false, "You already logged in.You can not login again.");
                }

                Session["UserId"] = userObj.Id;
                Session["RequestId"] = "";
                Session["UserName"] = userObj.Username;
                Session["DBString"] = userObj.DBString == null ? "" : userObj.DBString;
                Session["DBConnectionId"] = userObj.DBConnectionId;
                Session["compCode"] = userObj.compCode;
                Session["UserPwd"] = objPwd.defile(userObj.Password);
                string fullname = userObj.FirstName + " " + (userObj.LastName == null ? "" : userObj.LastName);
                Session["FullName"] = fullname;
                //  Session["RoleId"] = role.Id;
                // Session["UserSessionId"] = "1";
                string profileImg = userObj.ProfileImage;

                if (!string.IsNullOrEmpty(profileImg))
                {
                    profileImg = profileImg.Replace("~/", "");
                }
                else
                {
                    profileImg = "assets/images/profile.png";
                }
                Session["userProfileImage"] = profileImg;// "../../assets/images/profile.png";// "../../CompanyData/1/User/0/DSC5965.JPG";

                PayrollBO.Employee emp = new PayrollBO.Employee(userObj.EmployeeId);
                PayrollBO.RoleList role = new PayrollBO.RoleList(0, emp.CompanyId);
                //to save the login History
                PayrollBO.LoginHistory logHistory = new PayrollBO.LoginHistory();
                logHistory.BrowserInfo = HttpContext.Request.Browser.Browser;
                logHistory.UserHost = HttpContext.Request.UserHostAddress;
                logHistory.UserId = Convert.ToInt32(Session["UserId"]);
                logHistory.SessionId = HttpContext.Request.RequestContext.HttpContext.Session.SessionID;
                Session["UserSessionId"] = logHistory.SessionId;

                logHistory.Save();
                role.ForEach(u =>
                {
                    var Role = role.Where(r => r.Id == userObj.UserRole).FirstOrDefault();
                    if (!object.ReferenceEquals(Role, null))
                        userObj.RoleName = Role.Name;
                });
                PayrollBO.UserCompanymapping logedUser = new PayrollBO.UserCompanymapping(Convert.ToInt32(Session["UserId"]));
                PayrollBO.UserCompanymappingList loguser = new PayrollBO.UserCompanymappingList(Convert.ToInt32(Session["UserId"]));

               
                Session["EmployeeCode"] = emp != null ? emp.EmployeeCode!=null? emp.EmployeeCode:"0" : "0";
                //check User Role is null Get roles based on company id 
                if (string.IsNullOrEmpty(userObj.RoleName))
                {
                    PayrollBO.RoleList rolelist = new PayrollBO.RoleList(0, emp.CompanyId == 0 ? userObj.CompanyId : emp.CompanyId);// emp.CompanyId==0 ? userObj.CompanyId: emp.CompanyId
                    rolelist.ForEach(u =>
                    {
                        var Role = rolelist.Where(r => r.Id == userObj.UserRole).FirstOrDefault();
                        if (!object.ReferenceEquals(Role, null))
                            userObj.RoleName = Role.Name;
                    });
                }

                if (userObj.Username.ToLower()=="superadmin" && userObj.UserRole==999)
                {
                    userObj.RoleName = "Superadmin";
                }
                Session["UserRole"] = userObj.UserRole;
                Session["RoleName"] = userObj.RoleName;
                Session["EmployeeId"] = userObj.EmployeeId;
                Session["EmployeeName"] = userObj.FirstName + " " + userObj.LastName;
                Session["TaxEmpId"] = userObj.EmployeeId;
                Session["serverDate"] = DateTime.Now.Year + "/" +  DateTime.Now.Month  + "/" + DateTime.Now.Day;

              
                if (userObj.RoleName == "Employee")
                {
                    Session["EmployeeGUID"] = userObj.EmployeeId;
                }
                else
                {
                    Session["EmployeeGUID"] = Guid.Empty;
                }

                if (!object.ReferenceEquals(logedUser, null) && userObj.RoleName != "Admin" && (loguser.Count == 1))
                {

                    Session["CompanyId"] = logedUser.CompanyId;
                }
                else
                {
                    Session["CompanyId"] = userObj.CompanyId;
                }

                if (!userObj.IsForget)
                {

                    return BuidJsonResult(true, Url.Action("Index", "Home"));////Company
                }
                else
                {

                    return BuidJsonResult(true, Url.Action("ChangePass", "Login"));
                }
            }

            else
            {
                return BuidJsonResult(false, "Invalid User Name and Password.");
                //  return new JsonResult { Data = "false" };

                //Need to check with Login History alread logined
                //Check the Role of the User .Admin and User ->Company MApping
                //Environment ->Singlton or Multiple
                //Master Company
                ///Company
                /// User
            }
        }


        public ActionResult BrowserDetails()
        {
            List<object> result = new List<object>();
            string browser = HttpContext.Request.Browser.Browser;
            string userAgent = Request.UserAgent;
            if (userAgent.IndexOf("Edge") > -1)
            {
                browser = "Edge";
            }
            result.Add(browser);
            return BuidJsonResult(true, result);
        }
        public ActionResult ValidateLogin1(int user)
        {

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout(string actionVal)
        {
            if (!object.ReferenceEquals(Session["UserId"], null))
            {
                PayrollBO.LoginHistory logHistoryTemp = new PayrollBO.LoginHistory();
                logHistoryTemp.SessionId = Convert.ToString(Session["UserSessionId"]);
                logHistoryTemp.UserId = Convert.ToInt32(Session["UserId"]);
                logHistoryTemp.Save();// update the Log off
            }
            return RedirectToAction("Index", "Login");
        }
        public virtual JsonResult BuidJsonResult(bool successIndicator, object resultData)
        {
            return new JsonResult
            {
                Data = new { success = successIndicator, result = resultData },
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        //Created on 07/07/2016
        public JsonResult ForgetPassword(PayrollBO.User dataValue)
        {
            PayrollBO.UserList user = new PayrollBO.UserList(0);
            var emailUser = user.Where(u => u.Email.ToLower() == dataValue.Email.ToLower()).FirstOrDefault();
            bool status = false;
            bool IsSaved = false;
            var msg = "";
            if (emailUser != null)
            {
                string allowedChars = "";
                int passLength = 8;
                allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
                allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
                allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
                char[] sep = { ',' };
                string[] arr = allowedChars.Split(sep);
                string passwordString = "";
                string temp = "";
                Random rand = new Random();
                for (int i = 0; i < passLength; i++)
                {
                    temp = arr[rand.Next(0, arr.Length)];
                    passwordString += temp;
                }
                var newPass = passwordString;

                string userMail = emailUser.Email;

                string activationUrl = ConfigurationManager.AppSettings["AppLoginUrl"].ToString();

                // message = " < div style='border: 1px solid green;background-color: lightgrey; padding: 25px;margin: 25px;'>";
                string message = "<p>Hello " + emailUser.Username + ",</br> Your Temporary Password is <b>" + newPass + "</b> <div>";
                message = message + "<div> <a href='" + activationUrl + "'>Click here to Login</a> <div>";
                string subject = "Your Reset Password";

                PayRoleMail payrolemail = new PayRoleMail(userMail, subject, message);
                FileLibrary objPwd = new FileLibrary();
                // status = payrolemail.Send();

                dataValue.Username = emailUser.Username;
                dataValue.Password = objPwd.enfile(newPass);
                dataValue.Email = emailUser.Email;
                dataValue.FirstName = emailUser.FirstName;
                dataValue.LastName = emailUser.LastName;
                dataValue.Phone = emailUser.Phone;
                dataValue.ProfileImage = dataValue.ProfileImage;
                dataValue.UserRole = emailUser.UserRole;
                dataValue.CreatedBy = emailUser.CreatedBy;
                dataValue.ModifiedBy = emailUser.ModifiedBy;
                dataValue.EmployeeId = emailUser.EmployeeId;
                dataValue.IsActive = true;
                dataValue.IsForget = true;
                dataValue.Id = emailUser.Id;
                IsSaved = dataValue.Save();
                if (IsSaved)
                {

                    status = payrolemail.Send();
                }
                //else
                //{
                //    return base.BuildJson(true, 200, "Mail not send", dataValue);
                //}

                return base.BuildJson(true, 200, msg = status == true ? "Send a Password to Your Email" : "Mail Not Send", dataValue);
                // return BuidJsonResult(true, Url.Action("Index", "Home"));

            }
            else
            {
                return base.BuildJson(false, 100, "MailId was Not Found", dataValue);
            }

        }
        public JsonResult ChangePassword(PayrollBO.User dataValue)
        {
            try
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);
                int userid = Convert.ToInt32(Session["UserId"]);
                PayrollBO.User user = new PayrollBO.User(userid);
                bool IsSaved = false;
                // var emailUser = user.Where(u => u.Id == userid).FirstOrDefault();
                FileLibrary objPwd = new FileLibrary();
                dataValue.Username = user.Username;
                dataValue.Password = objPwd.enfile(dataValue.Password);
                dataValue.Email = user.Email;
                dataValue.FirstName = user.FirstName;
                dataValue.LastName = user.LastName;
                dataValue.Phone = user.Phone;
                dataValue.ProfileImage = user.ProfileImage;
                dataValue.UserRole = user.UserRole;
                dataValue.CreatedBy = userid;
                dataValue.ModifiedBy = userid;
                dataValue.EmployeeId = user.EmployeeId;
                dataValue.IsForget = false;
                dataValue.Id = user.Id;
                dataValue.IsActive = true;
                IsSaved = dataValue.Save();
                if (IsSaved)
                {
                    TraceError.ErrorLog.ChangePwd("Change Password UserID : " + userid.ToString() + "");
                    return BuidJsonResult(true, Url.Action("Index", "Login"));
                    // return base.BuildJson(true,200,"Password Changed Successfully",Url.Action("Index", "Home"));
                }
                else
                {
                    TraceError.ErrorLog.ChangePwd("Trying to Change Password UserID : " + userid.ToString() + "");
                    return base.BuildJson(false, 200, "There was some Problem While Saving Password", dataValue);
                }
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex);
                return base.BuildJson(false, 200, "There was some Problem While Saving Password", dataValue);
            }

        }

        //-------------------


        public JsonResult Forgetpasswordchecking(PayrollBO.User dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            FileLibrary objPwd = new FileLibrary();
            int user = Convert.ToInt32(Session["UserId"]);
            PayrollBO.User UserEmp;
            UserEmp = new PayrollBO.User(user);

            if (dataValue.Password.Trim() == objPwd.defile(UserEmp.Password))
            {
                return base.BuildJson(true, 400, string.Empty, dataValue);
            }
            else
            {
                return base.BuildJson(false, 200, "Please Enter The Correct password", dataValue);
            }

        }



        //-------------------



        public JsonResult CheckEmail(string useroremail)
        {
            bool IsExist = false;
            PayrollBO.User user = new PayrollBO.User();
            IsExist = user.CheckUserExist(useroremail);
            return BuidJsonResult(true, IsExist);

        }





    }
}
