using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using System.Data;
using Payroll.CustomFilter;
using System.Globalization;
//using PayrollBO.Util;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class SettingController : BaseController//Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            return View("~/Views/Company/CompanyView.cshtml");
        }

        public JsonResult GetEmpcodeSetting()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Emp_CodeSettingList CodesettingList = new Emp_CodeSettingList(companyId);
            return base.BuildJson(true, 200, "success", CodesettingList);
        }

        public JsonResult SaveEmpcodeAutoManual(int AutoManual)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Emp_CodeSetting CodesettingList = new Emp_CodeSetting();
            DataTable dt = CodesettingList.GetSetEmpcodeAutoManual(companyId, AutoManual, "update");
            return base.BuildJson(true, 200, "success", dt.Rows[0]["EmpCodeAutoManual"].ToString());
        }

        public JsonResult GetcodeAutoManual()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Emp_CodeSetting CodesettingList = new Emp_CodeSetting();
            DataTable dt = CodesettingList.GetSetEmpcodeAutoManual(companyId, 0, "Select");
            return base.BuildJson(true, 200, "success", dt.Rows[0]["EmpCodeAutoManual"].ToString());
        }

        public JsonResult EmpCodeCheck(string EmployeeCde)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Emp_CodeSetting CodesettingList = new Emp_CodeSetting();
            DataTable dt = CodesettingList.GetEmployeeCodecheck(companyId, EmployeeCde);

            if (dt.Rows.Count == 0)
            {
                return base.BuildJson(true, 200, string.Empty, true);
            }
            else
            {
                return base.BuildJson(false, 200, "Employee Code is already Exist.", false);
            }
        }
        public JsonResult GetEmpCodeSettingDetail(string SettingName)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            CategoryList catList = new CategoryList(companyId);
            Emp_CodeSettingList Emp_CodeSetting = new Emp_CodeSettingList(companyId, SettingName);
            string CategoryName = string.Empty;

            Emp_CodeSetting.ForEach(u =>
            {
                Emp_CodeSettingList CodesettingList = new Emp_CodeSettingList(companyId);
                CodesettingList.ForEach(v =>
                {
                    if (v.Name == u.Name)
                    {
                        CategoryName = v.CategoryName;
                    }

                });
            });


            List<object> retobj = new List<object>();



            List<jsonEmpCodeSetting> jsondata = new List<jsonEmpCodeSetting>();
            List<jsonCategory> jsoncategory = new List<jsonCategory>();
            var EmpCodeStruct = Emp_CodeSetting.ToList().FirstOrDefault();
            Emp_CodeSetting.ForEach(u => { jsoncategory.Add(jsonCategory.tojson(u.CategoryId)); });
            Emp_CodeSetting.ForEach(u => { jsondata.Add(jsonEmpCodeSetting.tojson(u)); });
            retobj.Add(catList);
            retobj.Add(jsondata);
            retobj.Add(jsoncategory);
            retobj.Add(CategoryName);
            //CodesettingList.ForEach(u => { jsondata.Add(jsonEmpCodeSetting.tojson(u)); });
            return base.BuildJson(true, 200, "success", retobj);
        }
        public JsonResult DeleteEmpcodeSetting(string SettingName)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Emp_CodeSetting delEmp_CodeSetting = new Emp_CodeSetting();
            delEmp_CodeSetting.Id = 1;//soft delete
            delEmp_CodeSetting.Name = SettingName;
            delEmp_CodeSetting.CompanyId = companyId;

            isDeleted = delEmp_CodeSetting.Delete();

            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data deleted successfully", isDeleted);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", isDeleted);
            }



        }
        public JsonResult GetSetting()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            SettingList setting = new SettingList(companyId);
            return new JsonResult { Data = setting };
        }
        public JsonResult DeclarationCarryForward(int StartMonth, int EndMonth)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TXFinanceYear DefaultFinancialYr = new TXFinanceYear(Guid.Empty, companyId, true);
            DateTime FStart = DefaultFinancialYr.StartingDate; DateTime FEnd = DefaultFinancialYr.EndingDate;
            DateTime now = DateTime.Now;
            DateTime SDate = new DateTime();
            DateTime EDate = new DateTime();
            if (StartMonth >= 4 && StartMonth <= 12)
            { SDate = new DateTime(FStart.Year, StartMonth, 1); }
            else
            { SDate = new DateTime(FEnd.Year, StartMonth, 1); }
            if (EndMonth >= 4 && EndMonth <= 12) { EDate = new DateTime(FStart.Year, EndMonth, 1); }
            else { EDate = new DateTime(FEnd.Year, EndMonth, 1); }
            SettingDefinition st = new SettingDefinition();
            DataTable dt = st.MonthlyCarryForward(companyId, SDate, EDate);
            bool Result = false;
            if (dt.Rows.Count == 0)
            {
                Result = st.CarryForward(companyId, SDate, EDate);
            }
            else
            {
                Result = false;
            }
            if (Result)
            {
                return base.BuildJson(true, 0, "Success", null);
            }
            else
            {
                return base.BuildJson(false, 0, "Selected month already carry forwarded !Please choose another month to proceed", null);
            }
        }
        public JsonResult GetSettingForm(int id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            SettingDefinitionList settingDefList = new SettingDefinitionList(id, companyId);
            //SettingValueList settingValueList = new SettingValueList(id);

            //foreach (SettingDefinition datatemp in settingDefList)
            //{
            //    var tmp = settingValueList.Where(u => u.SettingDefinitionId == datatemp.Id).FirstOrDefault();
            //    if (!object.ReferenceEquals(tmp, null))
            //    {
            //        settingDefList.Where(u => u.Id == tmp.SettingDefinitionId).FirstOrDefault().Value = tmp.Value;
            //    }

            //}
            return base.BuildJson(true, 200, "success", settingDefList);

        }

        public JsonResult SaveSetting(jsonSetting dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            foreach (jsonsettingKeyValues data in dataValue.settingKeyValues)
            {
                if (data.settingDefid == "")
                {
                    isSaved = false;
                    break;
                }
                SettingValue settingValue = jsonSettingValue.convertobject(data);
                settingValue.CreatedBy = userId;
                settingValue.ModifiedBy = userId;
                settingValue.IsDeleted = false;
                isSaved = settingValue.Save();
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
        public JsonResult SavelockReleaseSetting(Guid PaySheetLockid, string AdminPassword, int PayrollMonth, int PayrollYear, bool PayrollLock)
        {
            string userpwd = Session["UserPwd"].ToString();
            if (userpwd == AdminPassword)
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);
                bool isSaved = false;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                jsonLockReleaseSetting payrolllock = new jsonLockReleaseSetting();
                payrolllock.PaySheetLockid = PaySheetLockid;
                payrolllock.PayrollMonth = PayrollMonth;
                payrolllock.PayrollYear = PayrollYear;
                payrolllock.PayrollLock = PayrollLock;
                payrolllock.PaysheetCompanyId = companyId;
                payrolllock.PaysheetCreatedBy = Convert.ToString(userId);
                if (PaySheetLockid == Guid.Empty)
                {
                    payrolllock.PaySheetType = "Insert";
                }
                else
                {
                    payrolllock.PaySheetType = "Update";
                }
                LockSetting payrlock = jsonLockReleaseSetting.convertObject(payrolllock);
                isSaved = payrlock.LockSave();
                if (isSaved)
                {
                    if (payrolllock.PaySheetType == "Insert")
                    {
                        return base.BuildJson(true, 200, "Data saved successfully", payrolllock.PaySheetType);
                    }
                    else
                    {
                        return base.BuildJson(true, 200, "Data updated successfully", payrolllock.PaySheetType);
                    }
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", payrolllock);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "Admin password is incorrect.", "");
            }

        }
        public JsonResult SelectlockReleaseSetting(Guid PaySheetLockid, string AdminPassword, int PayrollMonth, int PayrollYear, bool PayrollLock)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            LockSetting selectVal = new LockSetting(PayrollMonth, PayrollYear, companyId, "Select");
            return base.BuildJson(true, 200, "Success", selectVal);
        }
        public JsonResult SaveEmpcodeSetting(jsonEmpCodeSetting dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            bool CatValid = true;
            string Errorlist = string.Empty;
            dataValue.jsonCategories.ForEach(u =>
            {
                Emp_CodeSettingList chkCategory = new Emp_CodeSettingList(companyId, "");
                chkCategory.ForEach(v =>
                {
                    if (v.CategoryId == u.catId && v.Name != dataValue.SName)
                    {
                        CatValid = false;
                        CategoryList catList = new CategoryList(companyId);
                        var Slcat = catList.Where(c => c.Id == u.catId).FirstOrDefault();
                        Errorlist = Errorlist + Slcat.Name + " are used in " + v.Name + Environment.NewLine;
                    }
                });
            });

            if (CatValid)
            {
                Emp_CodeSetting delEmp_CodeSetting = new Emp_CodeSetting();
                delEmp_CodeSetting.Id = 0;//hard delete
                delEmp_CodeSetting.Name = dataValue.SName;
                delEmp_CodeSetting.CompanyId = companyId;
                delEmp_CodeSetting.Delete();

                dataValue.jsonCategories.ForEach(u =>
                {
                    Emp_CodeSetting empcodesetting = jsonEmpCodeSetting.convertobject(dataValue);
                    empcodesetting.CompanyId = companyId;
                    empcodesetting.CreatedBy = userId;
                    empcodesetting.CategoryId = u.catId;
                    empcodesetting.Save();
                });

                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, Errorlist, dataValue);
            }
        }

        public JsonResult GetCompSetting()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Company comp = new Company(companyId, userId);
            List<string> payrollprocess = new List<string>();
            payrollprocess.Add("Category");
            payrollprocess.Add("Designation");
            payrollprocess.Add("Department");
            payrollprocess.Add("Branch");
            payrollprocess.Add("Location");
            payrollprocess.Add("CostCentre");
            List<object> retobj = new List<object>();
            retobj.Add(comp);
            retobj.Add(payrollprocess);
            comp.Initialize();
            return base.BuildJson(true, 200, "Success", retobj);
        }
        public JsonResult SaveCompSetting(Company dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.Id = companyId;
            dataValue.ModifiedBy = userId;
            if (dataValue.SaveCompanySetting())
            {
                return base.BuildJson(true, 200, "Success", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }
        public JsonResult GetTDSdays()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Company company = new Company(companyId);
            return base.BuildJson(true, 200, "Success", company);
        }
        public JsonResult GetCategorySetting(Guid category)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Category comp = new Category(category, companyId);
            List<string> payrollprocess = new List<string>();
            payrollprocess.Add("Employee");
            payrollprocess.Add("Category");
            payrollprocess.Add("Designation");
            payrollprocess.Add("Department");
            payrollprocess.Add("Branch");
            payrollprocess.Add("Location");
            payrollprocess.Add("CostCentre");
            payrollprocess.Add("ESILocation");
            payrollprocess.Add("PTLocation");
            List<object> retobj = new List<object>();
            retobj.Add(comp);
            retobj.Add(payrollprocess);

            return base.BuildJson(true, 200, "Success", retobj);
        }
        public JsonResult GetSeperationSetting(Guid category)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Category comp = new Category(category, companyId);
            List<object> retobj = new List<object>();
            retobj.Add(comp);
            return base.BuildJson(true, 200, "Success", retobj);
        }
        public JsonResult SaveCategorySetting(Category dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.CompanyId = companyId;
            dataValue.ModifiedBy = userId;
            if (dataValue.Savesetting())
            {
                return base.BuildJson(true, 200, "Success", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }
        //public JsonResult GetSettingDropValues(string name)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    List<settingDropDown> settingDefList = new List<settingDropDown>();
        //    switch (name)
        //    {
        //        case "Category":
        //            CategoryList category = new CategoryList(companyId);
        //            category.ForEach(u => { settingDefList.Add(new settingDropDown() { id = u.Id.ToString(), name = u.Name }); });
        //            break;
        //    }
        //    return base.BuildJson(true, 200, "success", settingDefList);

        //}

        public JsonResult GetRoleFormSetting(int roleId)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string userName = Convert.ToString(Session["UserName"]);
            // int roleId = Convert.ToInt32(Session["UserRole"]);
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


            RoleFormCommandList roleFormCommand = new RoleFormCommandList(companyId, roleId);
            //get the item which is not saved in Roleformcommand object
            var result = formCommandlist.Where(p => !roleFormCommand.Any(p2 => p2.FormCommandId == p.Id)).ToList();
            result.ForEach(u =>
            {
                roleFormCommand.Add(new RoleFormCommand() { CompanyId = companyId, FormCommandId = u.Id });
            });


            List<jsonRoleFormCommand> retobj = new List<jsonRoleFormCommand>();
            roleFormCommand.ForEach(u =>
            {
                if (u.FormCommandId != 0 && formCommandlist.Where(x=>x.Id== u.FormCommandId).FirstOrDefault()!=null)
                {
                    retobj.Add(jsonRoleFormCommand.tojson(u, formCommandlist, roleId));
                }
            });
            return base.BuildJson(true, 200, "Success", retobj);
        }
        //------- Created by Keerthika on 11/05/2017
        //    public JsonResult GetRoleForm(int Id)
        //    {
        //        if (!base.checkSession())
        //            return base.BuildJson(true, 0, "Invalid user", null);
        //        int companyId = Convert.ToInt32(Session["CompanyId"]);
        //        int userId = Convert.ToInt32(Session["UserId"]);
        //        string userName = Convert.ToString(Session["UserName"]); //----
        //        RoleFormCommandList roleFormCommand = new RoleFormCommandList(companyId, roleId);
        //        List<jsonRoleFormCommand> retobj = new List<jsonRoleFormCommand>();
        //        roleFormCommand.ForEach(u =>
        //        {
        //            retobj.Add(jsonRoleFormCommand.convertobject(roleFormCommand));
        //        });

        //        return base.BuildJson(true, 200, "Success", retobj);
        //    }

        //}

        public JsonResult SaveRoleFormSetting(int roleId, List<jsonRoleFormCommand> dataValue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            bool isSaved = true;
            dataValue.ForEach(u =>
            {
                u.roleId = roleId;
                u.companyId = companyId;
                RoleFormCommand roleFormCmd = jsonRoleFormCommand.convertobject(u);
                if (!roleFormCmd.Save())
                    isSaved = false;
            });
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Success", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }



    }
    public class jsonSetting
    {
        public jsonSetting()
        {
            this.settingKeyValues = new List<jsonsettingKeyValues>();
        }
        public int settingId { get; set; }
        public List<jsonsettingKeyValues> settingKeyValues { get; set; }
    }
    public class jsonLockReleaseSetting
    {
        public Guid PaySheetLockid { get; set; }
        public int PaysheetCompanyId { get; set; }
        public int PayrollMonth { get; set; }
        public int PayrollYear { get; set; }
        public bool PayrollLock { get; set; }
        public string AdminPassword { get; set; }
        public string PaysheetCreatedBy { get; set; }
        public string PaySheetType { get; set; }
        public static jsonLockReleaseSetting tojson(LockSetting LockReleaseSetting)
        {
            return new jsonLockReleaseSetting()
            {
                PaySheetLockid = LockReleaseSetting.PaySheetLockid,
                PayrollLock = LockReleaseSetting.PayrollLock
            };
        }
        public static LockSetting convertObject(jsonLockReleaseSetting loSetting)
        {
            return new LockSetting()
            {
                PaySheetLockid = loSetting.PaySheetLockid,
                PayrollLock = loSetting.PayrollLock,
                PayrollYear = loSetting.PayrollYear,
                PayrollMonth = loSetting.PayrollMonth,
                PaySheetType = loSetting.PaySheetType,
                PaysheetCompanyId = loSetting.PaysheetCompanyId,
                PaysheetCreatedBy = loSetting.PaysheetCreatedBy
            };
        }
    }
    public class jsonsettingKeyValues
    {
        public string settingDefid { get; set; }
        public string value { get; set; }
        public string settingid { get; set; }

    }

    public class jsonSettingValue
    {
        public int SettId { get; set; }
        public int SettDefId { get; set; }
        public string SettValue { get; set; }

        public static jsonSettingValue tojson(SettingValue settingValue)
        {
            return new jsonSettingValue()
            {
                SettId = settingValue.SettingId,
                SettDefId = settingValue.SettingDefinitionId,
                SettValue = settingValue.Value

            };
        }
        public static SettingValue convertobject(jsonsettingKeyValues settingValue)
        {
            return new SettingValue()
            {
                SettingId = Convert.ToInt32(settingValue.settingid),
                SettingDefinitionId = Convert.ToInt32(settingValue.settingDefid.Split('_')[0]),
                Value = settingValue.value
            };
        }

    }

    public class jsonEmpCodeSetting
    {
        public int Sid { get; set; }
        public string SName { get; set; }
        public string SPrefix { get; set; }
        public string SlNo { get; set; }
        public List<jsonCategory> jsonCategories { get; set; }
        public static jsonEmpCodeSetting tojson(Emp_CodeSetting empcodesetting)
        {
            return new jsonEmpCodeSetting()
            {
                Sid = empcodesetting.Id,
                SName = empcodesetting.Name,
                SPrefix = empcodesetting.PreFix,
                SlNo = empcodesetting.SNumber
            };
        }
        public static Emp_CodeSetting convertobject(jsonEmpCodeSetting empcodesetting)
        {
            return new Emp_CodeSetting()
            {
                Id = empcodesetting.Sid,
                Name = empcodesetting.SName,
                PreFix = empcodesetting.SPrefix,
                SNumber = empcodesetting.SlNo
            };
        }
    }
    public class jsonCategory
    {
        public Guid catId { get; set; }
        public static jsonCategory tojson(Guid catid)
        {
            return new jsonCategory()
            {
                catId = catid
            };
        }
    }
    public class jsonRoleFormCommand
    {
        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Get or Set the FormCommandId
        /// </summary>
        public int formCommandId { get; set; }

        /// <summary>
        /// Get or Set the RoleId
        /// </summary>
        public int roleId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int companyId { get; set; }


        /// <summary>
        /// Get or Set the IsRead
        /// </summary>
        public bool isRead { get; set; }

        /// <summary>
        /// Get or Set the IsWrite
        /// </summary>
        public bool isWrite { get; set; }

        /// <summary>
        /// Get or Set the IsRequired
        /// </summary>
        public bool isRequired { get; set; }

        /// <summary>
        /// Get or Set the IsPayrollTransaction
        /// </summary>
        public bool isPayrollTransaction { get; set; }

        /// <summary>
        /// Get or Set the IsApproval
        /// </summary>
        public bool isApproval { get; set; }

        public bool isDelete { get; set; }

        /// <summary>
        /// Get or Set the ReadMessage
        /// </summary>
        public string readMessage { get; set; }

        /// <summary>
        /// Get or Set the WriteMessage
        /// </summary>
        public string writeMessage { get; set; }

        /// <summary>
        /// Get or Set the RequiredMessage
        /// </summary>
        public string requiredMessage { get; set; }

        /// <summary>
        /// Get or Set the TransactionMessage
        /// </summary>
        public string transactionMessage { get; set; }

        /// <summary>
        /// Get or Set the ApprovalMessage
        /// </summary>
        public string approvalMessage { get; set; }

        public string deleteMessage { get; set; }

        public string commandName { get; set; }

        public string commandDescription { get; set; }

        public string commandType { get; set; }

        public int parentId { get; set; }
        public int dependentId { get; set; }

        public string formName { get; set; }

        public bool readDisable { get; set; }
        public bool writeDisable { get; set; }

        public bool tranDisable { get; set; }

        public bool requireDisable { get; set; }

        public bool approvalDisable { get; set; }
        //---- Modified by keerthika on 09/05/2017
        public static jsonRoleFormCommand tojson(RoleFormCommand roleFormCommand, FormCommandList formCommandlist, int roleId)
        {
            var formCmd = formCommandlist.Where(u => u.Id == roleFormCommand.FormCommandId).FirstOrDefault();
            if (roleFormCommand.RoleId == roleId && formCmd != null)
            {
                return new jsonRoleFormCommand()
                {
                    id = roleFormCommand.Id,
                    formCommandId = roleFormCommand.FormCommandId,
                    roleId = roleFormCommand.RoleId,
                    companyId = roleFormCommand.CompanyId,
                    // isApproval = formCmd.IsDefaultApprovel == true ? true : roleFormCommand.IsApproval,
                    //  isRead = formCmd.IsDefaultRead == true ? true : roleFormCommand.IsRead, //--
                    // isRead = formCmd.IsDefaultRead == false ? false:'' ,
                    // isApproval = roleFormCommand.IsApproval,
                    //   isRead = roleFormCommand.IsRead,
                    // isRequired = formCmd.IsDefaultRequired == true ? true : roleFormCommand.IsRequired,
                    // isWrite = formCmd.IsDefaultWrite == true ? true : roleFormCommand.IsWrite,//---
                    //  isRequired = roleFormCommand.IsRequired,
                    //   isWrite = roleFormCommand.IsWrite,
                    // isPayrollTransaction = roleFormCommand.IsPayrollTransaction,
                    //   isDelete = roleFormCommand.IsDelete == true ? true : roleFormCommand.IsDelete, //----
                    //  isPayrollTransaction = formCmd.IsDefaultTransaction == true ? true : roleFormCommand.IsPayrollTransaction,
                    //------------
                    //isApproval = formCmd.IsDefaultApprovel == true ? true : roleFormCommand.IsApproval,
                    //// isRead = formCmd.IsDefaultRead == false ? false : roleFormCommand.IsRead,
                    //isRead = formCmd.IsDefaultRead == false ? false : roleFormCommand.IsRead,
                    //// isRead = formCmd.IsDefaultRead == false && roleFormCommand.IsRead ==false? false : true ,
                    //isRequired = formCmd.IsDefaultRequired == true ? true : roleFormCommand.IsRequired,
                    //isWrite = formCmd.IsDefaultWrite == false ? false : roleFormCommand.IsWrite,
                    //// isWrite = formCmd.IsDefaultWrite == false && roleFormCommand.IsWrite ==false? false:true,
                    //// isDelete = roleFormCommand.IsDelete == true ? true : roleFormCommand.IsDelete,
                    isRead = roleFormCommand.IsRead,
                    isApproval = roleFormCommand.IsApproval,
                    isRequired = roleFormCommand.IsRequired,
                    isWrite = roleFormCommand.IsWrite,
                    isDelete = roleFormCommand.IsDelete,
                    // isPayrollTransaction = formCmd.IsDefaultTransaction == true ? true : roleFormCommand.IsPayrollTransaction,
                    isPayrollTransaction = roleFormCommand.IsPayrollTransaction,
                    readMessage = roleFormCommand.ReadMessage,
                    writeMessage = roleFormCommand.WriteMessage,
                    requiredMessage = roleFormCommand.RequiredMessage,
                    transactionMessage = roleFormCommand.TransactionMessage,
                    approvalMessage = roleFormCommand.ApprovalMessage,
                    deleteMessage = roleFormCommand.DeleteMessage,
                    commandName = formCmd.CommandName,
                    formName = formCmd.CommandName,
                    commandDescription = formCmd.Description,
                    commandType = formCmd.CommandTypes,
                    parentId = formCmd.ParentId,
                    dependentId = formCmd.DependentId,

                    approvalDisable = formCmd.IsDefaultApprovel == true ? true : false,
                    readDisable = formCmd.IsDefaultRead == true ? true : false,//--
                    requireDisable = formCmd.IsDefaultRequired == true ? true : false,
                    writeDisable = formCmd.IsDefaultWrite == true ? true : false,//--
                    tranDisable = formCmd.IsDefaultTransaction == true ? true : false//--

                };
            }
            else
            {
                    return new jsonRoleFormCommand()
                    {
                        id = roleFormCommand.Id,
                        formCommandId = roleFormCommand.FormCommandId,
                        roleId = roleFormCommand.RoleId,
                        companyId = roleFormCommand.CompanyId,

                        isRead = formCmd.IsDefaultRead,
                        isApproval = formCmd.IsDefaultApprovel,
                        isRequired = formCmd.IsDefaultRequired,
                        isWrite = formCmd.IsDefaultWrite,
                        isDelete = roleFormCommand.IsDelete,
                        readMessage = roleFormCommand.ReadMessage,
                        writeMessage = roleFormCommand.WriteMessage,
                        requiredMessage = roleFormCommand.RequiredMessage,
                        transactionMessage = roleFormCommand.TransactionMessage,
                        isPayrollTransaction = formCmd.IsDefaultTransaction,
                        approvalMessage = roleFormCommand.ApprovalMessage,
                        deleteMessage = roleFormCommand.DeleteMessage,
                        commandName = formCmd.CommandName,
                        formName = formCmd.CommandName,
                        commandDescription = formCmd.Description,
                        commandType = formCmd.CommandTypes,
                        parentId = formCmd.ParentId,
                        dependentId = formCmd.DependentId,

                        approvalDisable = formCmd.IsDefaultApprovel == true ? true : false,
                        readDisable = formCmd.IsDefaultRead == true ? true : false,//--
                        requireDisable = formCmd.IsDefaultRequired == true ? true : false,
                        writeDisable = formCmd.IsDefaultWrite == true ? true : false,//--
                        tranDisable = formCmd.IsDefaultTransaction == true ? true : false//--

                    };
               
            }
        }



        public static RoleFormCommand convertobject(jsonRoleFormCommand roleFormCommand)
        {
            return new RoleFormCommand()
            {
                Id = roleFormCommand.id,
                FormCommandId = roleFormCommand.formCommandId,
                RoleId = roleFormCommand.roleId,
                CompanyId = roleFormCommand.companyId,
                IsApproval = roleFormCommand.isApproval,
                IsRead = roleFormCommand.isRead,
                IsRequired = roleFormCommand.isRequired,
                IsWrite = roleFormCommand.isWrite,
                IsDelete = roleFormCommand.isDelete,
                IsPayrollTransaction = roleFormCommand.isPayrollTransaction,
                ReadMessage = roleFormCommand.readMessage,
                WriteMessage = roleFormCommand.writeMessage,
                RequiredMessage = roleFormCommand.requiredMessage,
                TransactionMessage = roleFormCommand.transactionMessage,
                ApprovalMessage = roleFormCommand.approvalMessage,
                DeleteMessage = roleFormCommand.deleteMessage
            };
        }
    }


}