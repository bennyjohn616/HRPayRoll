﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Configuration;
using Payroll.CustomFilter;
using TraceError;

namespace Payroll.Controllers
{
    [CustomExceptionFilter]
    [SessionExpireAttribute]
    public class CompanyController : BaseController//Controller
    {
        //
        // GET: /Company/

        public ActionResult Index()
        {
            //Session["UserId"] = "1";
            //Session["RequestId"] = "";
            //Session["CompanyId"] = "1";
            //Session["UserSessionId"] = "1";
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            if (object.ReferenceEquals(Session["CompanyId"], null))
                return RedirectToAction("Index", "Home");
            if (Convert.ToInt32(Session["CompanyId"]) <= 0)
                return RedirectToAction("Index", "Login");

            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));
            ViewBag.CompanyName = comp.CompanyName;
            Session["CompLogo"] = !string.IsNullOrEmpty(comp.Companylogo) ? comp.Companylogo.Replace("~/", "") : "assets/images/logo.png";
            ViewBag.TempTitle = "Payroll";
            ViewBag.userProfileImage = Session["userProfileImage"];//TempData["userProfileImage"];
            if (!object.ReferenceEquals(ViewBag.userProfileImage, null))
                ViewBag.userProfileImage = Convert.ToString(ViewBag.userProfileImage).Replace("~", "");
            return View("~/Views/Company/CompanyView.cshtml");
            // return View("~/Views/Test.cshtml");

            //TempData[“Employee”] = employee;// will use rederect action
            //  ViewData[“Address”] // controller to view
            //  ViewBag.Address // controller to view

        }


        /// <summary>
        /// Company logo save
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveUserImage()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //int userId = Convert.ToInt32(Session["UserId"]);
            Guid tempId;
            try
            {
                tempId = Guid.NewGuid();
                string strRelationPath = "";
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
                        strRelationPath = "~/CompanyData/Companylogo/" + tempId + "/" + fileName;
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
                return base.BuildJson(false, 100, "There is some error while saving the file.", "/Temp/");
            }

        }
        public ActionResult CompanyIndex()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            Session["CompanyId"] = null;
            return View("~/Views/Company/CompanyList.cshtml");
        }
        public JsonResult GetLanguages()
        {
            int companyid = Convert.ToInt32(Session["CompanyId"]);
            // jsonLanguage comp = new jsonLanguage();
            // var data = comp.GetLanguages();
            Language lang = new Language();
            List<Language> _languages = new List<Language>();
            var langlist = lang.LanguagesList(companyid);
            langlist.ForEach(l =>
            {
                _languages.Add(new Language() { LangId = l.LangId, Name = l.Name });
            });
            if (langlist.Count == 0) _languages.Add(new Language() { LangId = 0, Name = "" });
            return new JsonResult { Data = langlist };
        }
        public JsonResult GetRealtionShips()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            jsonRelationShip comp = new jsonRelationShip();
            var data = comp.GetRelationship();
            return new JsonResult { Data = data };
        }
        //Modified by Keerthika on 23/05/2017
        public JsonResult GetCompany(string getempaction)
        {

            int userid = Convert.ToInt32(Session["userid"]);
            //userid = 1;
            int companyId = 0;
            if (Session["CompanyId"] != null && getempaction == "Employee")
            {
                companyId = Convert.ToInt32(Session["CompanyId"]);
            }
            CompanyList comp = new CompanyList(companyId, userid);
            CompanyList newcomp = new CompanyList();
            PayrollBO.UserCompanymappingList loguser = new UserCompanymappingList(userid);

            if (loguser.Count >= 1)
            {
                comp.ForEach(c =>
                {
                    var com = loguser.Where(l => l.CompanyId == c.Id).FirstOrDefault();
                    if (com != null)
                        newcomp.Add(c);
                    //  return base.BuildJson(true, 200, "Success", com);
                    // return new JsonResult { Data = com };
                });
                return new JsonResult { Data = newcomp };
            }



            comp.Initialize();
            return new JsonResult { Data = comp };

            //return base.BuildJson(true, 200, "success", comp);

        }
        //-------Created on 07/07/2017
        public JsonResult GetCompanies()
        {
            int companyId = 0;

            CompanyList companyList = new CompanyList(companyId, 0);
            return new JsonResult { Data = companyList };
        }

        



        public JsonResult GetCategories()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string EmpId = Convert.ToString(Session["EmployeeGUID"]);
            Guid EmployeeID = new Guid(Convert.ToString(Session["EmployeeGUID"]));
            Guid employeeId = new Guid(EmpId);
            CategoryList comp = new CategoryList();
            if (Convert.ToString(Session["RoleName"])== "Employee")
            {
                Employee empdetails = new Employee(companyId, employeeId);
                comp = new CategoryList(companyId);
                CategoryList comp1 = comp;
                comp = new CategoryList();
                comp.AddRange(comp1.Where(f => f.Id == empdetails.CategoryId).ToList());
            }
            else
            {
                comp = new CategoryList(companyId);
            }
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "CategoryId" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = comp.Where(d => d.Id == new Guid(logedUser.RightsOnValue)).ToList();
                return base.BuildJson(true, 200, "" + EmployeeID, result);
            }
            else
            {
                return base.BuildJson(true, 200, "" + EmployeeID, comp);
            }
            // return new JsonResult { Data = comp };
        }
        /// <summary>
        /// To get the Loan List
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLoan()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LoanMasterList Loanvalue = new LoanMasterList(companyId);
            var Loanname = Loanvalue.Where(d => d.CompanyId == companyId).ToList();
            UserCompanymapping logedUser = new UserCompanymapping(Convert.ToInt32(Session["userid"]));
            if (logedUser.RightsOn != null && logedUser.RightsOn == "CategoryId" && logedUser.RightsOnValue != Guid.Empty.ToString())
            {
                var result = Loanvalue.Where(d => d.CompanyId == companyId).ToList();
                return base.BuildJson(true, 200, "Success", result);
            }
            else
            {
                return base.BuildJson(true, 200, "Success", Loanname);
            }
            // return new JsonResult { Data = comp };
        }

        public JsonResult GetBanks()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            BankList bankList = new BankList(companyId);
            return new JsonResult { Data = bankList };
        }
        public JsonResult GetPreviousPayrollProcessMonthYear(string Condition)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userid = Convert.ToInt32(Session["userid"]);
            PayrollHistoryList payrollHistoryList = new PayrollHistoryList(companyId);//Get All processed payroll Month and year
            if (payrollHistoryList.Count > 0)
            {

                var firstElement = payrollHistoryList.First();
                //payrollHistoryList.Select(u=>u.Month)
                if (Condition == "Month")
                {
                    return new JsonResult { Data = firstElement.Month };
                }
                else if (Condition == "Year")
                {
                    return new JsonResult { Data = firstElement.Year };
                }
                else
                {
                    Company company = new Company(companyId, userid);
                    return new JsonResult { Data = company.PayrollProcessBy };

                }
            }
            else
            {
                Company company = new Company(companyId, userid);
                return new JsonResult { Data = company.PayrollProcessBy };
            }
        }
        //public JsonResult GetAttributeModel(string type)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    //companyId = 1;

        //    EntityAttributeModelList entityAttributeModellist;
        //    if (type == "master")
        //    {
        //        TableCategory tablecategory = new TableCategory(companyId, new Guid("08376B46-0C5B-49DC-800D-882F6D49FC7C"));
        //        entityAttributeModellist = new EntityAttributeModelList(tablecategory.EntityModelList[0].Id);
        //    }
        //    else if (type == "Earning")
        //    {
        //        TableCategory tablecategory = new TableCategory(companyId, new Guid("08376B46-0C5B-49DC-800D-882F6D49FC7C"));
        //        entityAttributeModellist = new EntityAttributeModelList(tablecategory.EntityModelList[0].Id);

        //    }
        //    else if (type == "Deduction")
        //    {
        //        TableCategory tablecategory = new TableCategory(companyId, new Guid("C237EA92-4E3A-4A91-917A-C990E14E30DE"));
        //        entityAttributeModellist = new EntityAttributeModelList(tablecategory.EntityModelList[0].Id);

        //    }
        //    else
        //        entityAttributeModellist = new EntityAttributeModelList();
        //    return base.BuildJson(true, 200, "success", entityAttributeModellist);

        //}
        public JsonResult GetJoiningDocuments()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            JoiningDocumentList comp = new JoiningDocumentList(companyId);
            return new JsonResult { Data = comp };
        }

        public JsonResult GetSavgProof()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            JoiningDocumentList comp = new JoiningDocumentList(companyId);
            return new JsonResult { Data = comp };
        }

        /// <summary>
        /// ////////////
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDesignations()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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

        public JsonResult GetCostcentres()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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

        //-----
        public JsonResult GetBranchs()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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
        public JsonResult GetESIDespensarys()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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
        public JsonResult GetDepartments()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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
        public JsonResult GetLocation()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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

        public JsonResult GetAttribute()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            AttributeModelList attlist1 = new AttributeModelList(companyId);
            var attlist = attlist1.Where(al => al.BehaviorType == "Earning" && al.IsMonthlyInput == false && al.IsIncludeForGrossPay == false && al.CompanyId == companyId).ToList();
            return new JsonResult { Data = attlist };
        }

        public JsonResult GetGrades()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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
        public JsonResult GetEsilocation()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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

        public JsonResult GetBloodGroup()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            BloodGroupList bloodGroup = new BloodGroupList(true);
            return new JsonResult { Data = bloodGroup };

        }

        public JsonResult SaveCreateNewCompany(jsonCompanyConnection dataValue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            CompanyConnection companyConnection = new CompanyConnection();
            dataValue.CUserId = userId;
            companyConnection = jsonCompanyConnection.convertobject(dataValue);
            companyConnection.CreatedBy = userId;
            companyConnection.ModifiedBy = userId;
            isSaved = companyConnection.Save();


            if (isSaved)
            {
                return base.BuildJson(true, 200, "DataBase Created successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Creating the database.", dataValue);
            }
            //if (!base.checkSession())
            //    return base.BuildJson(true, 0, "Invalid user", null);
            //int companyId = dataValue.Id;
            //dataValue.CreatedBy = Convert.ToInt32(Session["UserId"]);
            //dataValue.ModifiedBy = dataValue.CreatedBy;
            //dataValue.IsDeleted = false;

            //else
            //{
            //    return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            //}

        }
        public JsonResult SaveCompany(Company dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = dataValue.Id;
            int userid = Convert.ToInt32(Session["UserId"]);
            dataValue.CreatedBy = userid;
            dataValue.ModifiedBy = dataValue.CreatedBy;
            dataValue.IsDeleted = false;
            string serverpath = Server.MapPath("~");
            string filepath = serverpath + ConfigurationManager.AppSettings["StaticCompanyData"].ToString();
            if (companyId == 0 && (!System.IO.File.Exists(filepath)))
            {
                return base.BuildJson(false, 100, "Static data was avaliable.Please check with IT Team ", dataValue);
            }
            if (dataValue.Save())
            {
                if (companyId == 0)
                {
                    try
                    {
                        dataValue.InsertDefaultForNewCompany(dataValue.Id, userid, Server.MapPath("~"));
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log(ex);
                        return base.BuildJson(false, 100, "Company saved but static data was not saved. Please check with IT Team. ", dataValue);
                    }

                    /*
                    SettingList settinglist = new SettingList(1);
                    settinglist.ForEach(u =>
                    {
                        int settingId = u.Id;
                        u.CompanyId = dataValue.Id;
                        u.CreatedBy = (int)dataValue.CreatedBy;
                        if (u.Save())
                        {
                            SettingDefinitionList settingDef = new SettingDefinitionList(settingId, 1);
                            settingDef.ForEach(v =>
                            {
                                v.SettingId = u.Id;
                                v.CompanyId = u.CompanyId;
                                v.Save();
                            });
                        }
                    });
                    TableCategoryList tablCat = new TableCategoryList(1);
                    int tabCount = 0;
                    tablCat.ForEach(u =>
                    {
                        u.CompanyId = dataValue.Id;
                        if (u.Save())
                        {
                            if (u.Name == "Payroll")
                            {
                                if (tabCount == 0)
                                {
                                    EntityModel entityModel = new EntityModel();
                                    entityModel.TableCategoryId = u.Id;
                                    entityModel.Name = "Salary";
                                    entityModel.DisplayAs = entityModel.Name;
                                    entityModel.CompanyId = dataValue.Id;
                                    entityModel.IsPhysicalTable = false;
                                    if (entityModel.Save())
                                    {
                                        EntityModelMapping entityMap = new EntityModelMapping();
                                        entityMap.EntityTableName = Convert.ToString(entityModel.Id);
                                        entityMap.RefEntityModelName = "Employee";
                                        entityMap.Save();
                                    }
                                    tabCount++;
                                }
                            }
                        }
                    });*/
                }
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult GetCompanyData(int id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            if (id == 0)
            {
                id = Convert.ToInt32(Session["CompanyId"]);
            }

            //   RoleList role = new RoleList(0, id);

            Company company = new Company(id, Convert.ToInt32(Session["UserId"]));
            Session["EntryDate"] = company.cutoffdate.ToLongDateString();
            ViewBag.Title = company.CompanyName;
            return base.BuildJson(true, 200, "success", company);
        }

        public JsonResult GetChartsData(int id)
        {

            CompanyList companylistt = new CompanyList();
            var comid = Convert.ToInt32(Session["CompanyId"]);
            //   RoleList role = new RoleList(0, id)
            List<object> retobj = new List<object>();
            retobj.Add(companylistt.ActiveEmployees(comid));
            retobj.Add(companylistt.INActiveEmployees(comid));
            retobj.Add(companylistt.EmployeeGender(comid));
            return base.BuildJson(true, 200, "success", retobj);
        }





        public JsonResult DeleteCompany(int id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            Company company = new Company();
            company.Id = id;
            company.CreatedBy = Convert.ToInt32(Session["UserId"]);
            company.ModifiedBy = company.CreatedBy;
            company.IsDeleted = true;
            if (company.Delete())
            {
                if (company.COMDELSTAT == 1)
                {
                    return base.BuildJson(true, 200, "Data Deleted successfully", company);
                }
                else
                {
                    return base.BuildJson(false, 100, "You can't Delete this company,Employee is Still available.", company);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", company);
            }
        }
        /// <summary>
        /// Modified by:sharmila
        /// Modified on:17.04.17
        /// </summary>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public JsonResult SavePopup(popup dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            switch (dataValue.type)
            {
                case "category":
                    CategoryList categorylist = new CategoryList(companyId);
                    var displayorder = categorylist.Count == 0 ? dataValue.DisOrder : categorylist.Select(k => k.DisOrder).ToList().Max();
                    var Category_list = categorylist.Where(c => c.Name.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    var Dislpay_List = categorylist.Where(c => c.DisOrder == dataValue.DisOrder).ToList();
                    var dISP_CHECK = Dislpay_List.Where(s => s.Id == dataValue.Id).ToList();
                    var Category__CHECK = Category_list.Where(s => s.Id == dataValue.Id).ToList();

                    if (Category_list.Count == 0|| Dislpay_List.Count==0)
                    {
                        Category data;
                        if (dataValue.Id != Guid.Empty)
                            data = new Category(dataValue.Id, companyId);
                        else
                            data = new Category();
                        data.Name = dataValue.popuplalue;
                        data.DisOrder = dataValue.DisOrder;
                        data.CompanyId = companyId;
                        data.CreaateBy = userId;
                        data.ModifiedBy = data.CreaateBy;
                        data.IsDeleted = false;
                        isSaved = data.Save();
                    }
                    else
                    {
                        if (Category_list.Count != 0 && Category__CHECK.Count == 0)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                        else if (Dislpay_List.Count != 0 && dISP_CHECK.Count == 0)
                        {
                            return base.BuildJson(false, 200, "Display order" + dataValue.DisOrder + " Already exist ", dataValue);
                        }

                    }
                    break;
                case "branch":
                    BranchList branchlist = new BranchList(companyId);
                    bool branch_status = false;
                    var branch_list = branchlist.Where(d => d.BranchName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Branch branch;
                    if (branch_list.Count == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            branch = new Branch(dataValue.Id, companyId);
                        else
                            branch = new Branch();
                        branch.BranchName = dataValue.popuplalue;
                        branch.CompanyId = companyId;
                        branch.CreatedBy = userId;
                        branch.ModifiedBy = branch.CreatedBy;
                        branch.IsDeleted = false;
                        isSaved = branch.Save();
                    }
                    else
                    {
                        branch_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                branch = new Branch();
                                branch.BranchName = dataValue.popuplalue;
                                branch.CompanyId = companyId;
                                branch.CreatedBy = userId;
                                branch.Id = dataValue.Id;
                                branch.ModifiedBy = branch.CreatedBy;
                                branch.IsDeleted = false;
                                isSaved = branch.Save();
                                branch_status = true;
                            }
                        });
                        if (branch_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }

                    }
                    break;
                case "designation":
                    DesignationList designationlist = new DesignationList(companyId);
                    bool des_status = false;
                    var designation_list = designationlist.Where(d => d.DesignationName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Designation designation;
                    if (designation_list.Count == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            designation = new Designation(dataValue.Id, companyId);
                        else
                            designation = new Designation();
                        designation.DesignationName = dataValue.popuplalue;
                        designation.CompanyId = companyId;
                        designation.CreatedBy = userId;
                        designation.ModifiedBy = designation.CreatedBy;
                        designation.IsDeleted = false;
                        isSaved = designation.Save();
                    }
                    else
                    {
                        designation_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                designation = new Designation();
                                designation.DesignationName = dataValue.popuplalue;
                                designation.CompanyId = companyId;
                                designation.CreatedBy = userId;
                                designation.Id = dataValue.Id;
                                designation.ModifiedBy = designation.CreatedBy;
                                designation.IsDeleted = false;
                                isSaved = designation.Save();
                                des_status = true;
                            }
                        });
                        if (des_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }

                    break;


                case "costCentre":
                    CostCentreList costlist = new CostCentreList(companyId);
                    bool Cost_status = false;
                    var cost_list = costlist.Where(d => d.CostCentreName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    CostCentre costCentre;
                    if (cost_list.Count == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            costCentre = new CostCentre(dataValue.Id, companyId);
                        else
                            costCentre = new CostCentre();
                        costCentre.CostCentreName = dataValue.popuplalue;
                        costCentre.CompanyId = companyId;
                        costCentre.CreatedBy = userId;
                        costCentre.ModifiedBy = costCentre.CreatedBy;
                        costCentre.IsDeleted = false;
                        isSaved = costCentre.Save();
                    }
                    else
                    {
                        cost_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                costCentre = new CostCentre();
                                costCentre.CostCentreName = dataValue.popuplalue;
                                costCentre.CompanyId = companyId;
                                costCentre.CreatedBy = userId;
                                costCentre.Id = dataValue.Id;
                                costCentre.ModifiedBy = costCentre.CreatedBy;
                                costCentre.IsDeleted = false;
                                isSaved = costCentre.Save();
                                Cost_status = true;
                            }
                        });
                        if (Cost_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }
                    break;

                case "esiLocation":
                    EsiLocationList esilist = new EsiLocationList(companyId);
                    bool esi_loc_status = false;
                    var esi_list = esilist.Where(d => d.LocationName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    EsiLocation esiLocation;
                    if (esi_list.Count == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            esiLocation = new EsiLocation(dataValue.Id, companyId);
                        else
                            esiLocation = new EsiLocation();
                        esiLocation.LocationName = dataValue.popuplalue;
                        esiLocation.isApplicable = dataValue.isApplicable;
                        esiLocation.EmployerCode = dataValue.employerCode;
                        esiLocation.CompanyId = companyId;
                        esiLocation.CreatedBy = userId;
                        esiLocation.ModifiedBy = esiLocation.CreatedBy;
                        esiLocation.IsDeleted = false;
                        isSaved = esiLocation.Save();
                    }
                    else
                    {
                        esi_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                esiLocation = new EsiLocation();
                                esiLocation.LocationName = dataValue.popuplalue;
                                esiLocation.isApplicable = dataValue.isApplicable;
                                esiLocation.EmployerCode = dataValue.employerCode;
                                esiLocation.CompanyId = companyId;
                                esiLocation.Id = dataValue.Id;
                                esiLocation.CreatedBy = userId;
                                esiLocation.ModifiedBy = esiLocation.CreatedBy;
                                esiLocation.IsDeleted = false;
                                isSaved = esiLocation.Save();
                                esi_loc_status = true;
                            }
                        });
                        if (esi_loc_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }
                    break;
                case "grade":
                    GradeList gradelist = new GradeList(companyId);
                    bool grade_status = false;
                    var grade_list = gradelist.Where(d => d.GradeName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Grade grade;
                    if (grade_list.Count == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            grade = new Grade(dataValue.Id, companyId);
                        else
                            grade = new Grade();
                        grade.GradeName = dataValue.popuplalue;
                        grade.CompanyId = companyId;
                        grade.CreatedBy = userId;
                        grade.ModifiedBy = grade.CreatedBy;
                        grade.IsDeleted = false;
                        isSaved = grade.Save();
                    }
                    else
                    {
                        grade_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                grade = new Grade();
                                grade.GradeName = dataValue.popuplalue;
                                grade.CompanyId = companyId;
                                grade.CreatedBy = userId;
                                grade.ModifiedBy = grade.CreatedBy;
                                grade.IsDeleted = false;
                                isSaved = grade.Save();
                                grade_status = true;
                            }
                        });
                        if (grade_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }
                    break;
                case "esiDespensary":
                    ESIDespensaryList esidispensarylist = new ESIDespensaryList(companyId);
                    bool Curr_status = false;
                    var esidispensary_list = esidispensarylist.Where(d => d.ESIDespensary.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    EsiDespensary esiDespensary;
                    if (esidispensary_list.Count == 0)
                    {

                        if (dataValue.Id != Guid.Empty)
                            esiDespensary = new EsiDespensary(dataValue.Id, companyId);
                        else
                            esiDespensary = new EsiDespensary();
                        esiDespensary.ESIDespensary = dataValue.popuplalue;
                        esiDespensary.CompanyId = companyId;
                        esiDespensary.CreatedBy = userId;
                        esiDespensary.ModifiedBy = esiDespensary.CreatedBy;
                        esiDespensary.IsDeleted = false;
                        isSaved = esiDespensary.Save();
                    }
                    else
                    {
                        esidispensary_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                esiDespensary = new EsiDespensary();
                                esiDespensary.ESIDespensary = dataValue.popuplalue;
                                esiDespensary.Id = dataValue.Id;
                                esiDespensary.CompanyId = companyId;
                                esiDespensary.CreatedBy = userId;
                                esiDespensary.ModifiedBy = esiDespensary.CreatedBy;
                                esiDespensary.IsDeleted = false;
                                isSaved = esiDespensary.Save();
                                Curr_status = true;
                            }
                        });
                        if (Curr_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }

                    }
                    break;
                // Modified By Keerthika on 17/04/2017
                case "department":
                    DepartmentList departmentlist = new DepartmentList(companyId);
                    bool dep_status = false;
                    var Department_list = departmentlist.Where(d => d.DepartmentName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Department department;
                    if (Department_list.Count() == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            department = new Department(dataValue.Id, companyId);
                        else
                            department = new Department();
                        department.DepartmentName = dataValue.popuplalue;
                        department.CompanyId = companyId;
                        department.CreatedBy = userId;
                        department.ModifiedBy = department.CreatedBy;
                        department.IsDeleted = false;
                        isSaved = department.Save();
                    }
                    else
                    {
                        Department_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                department = new Department();
                                department.DepartmentName = dataValue.popuplalue;
                                department.CompanyId = companyId;
                                department.CreatedBy = userId;
                                department.Id = dataValue.Id;
                                department.ModifiedBy = department.CreatedBy;
                                department.IsDeleted = false;
                                isSaved = department.Save();
                                dep_status = true;
                            }
                        });
                        if (dep_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }

                    }
                    break;
                //Modified By Keerthika on 17/04/2017
                case "location":
                    LocationList locationlist = new LocationList(companyId);
                    bool Loc_status = false;
                    var location_list = locationlist.Where(d => d.LocationName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Location location;
                    if (location_list.Count() == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            location = new Location(dataValue.Id, companyId);
                        else
                            location = new Location();
                        location.LocationName = dataValue.popuplalue;
                        location.CompanyId = companyId;
                        location.CreatedBy = userId;
                        location.ModifiedBy = location.CreatedBy;
                        location.IsDeleted = false;
                        isSaved = location.Save();
                    }
                    else
                    {
                        location_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                location = new Location();
                                location.LocationName = dataValue.popuplalue;
                                location.CompanyId = companyId;
                                location.CreatedBy = userId;
                                location.Id = dataValue.Id;
                                location.ModifiedBy = location.CreatedBy;
                                location.IsDeleted = false;
                                isSaved = location.Save();
                                Loc_status = true;
                            }
                        });
                        if (Loc_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }
                    break;
                case "HRComponent":
                    HRComponent hrcomponent;
                    if (dataValue.Id != Guid.Empty)
                        hrcomponent = new HRComponent(dataValue.Id, companyId);
                    else
                        hrcomponent = new HRComponent();
                    hrcomponent.Name = dataValue.popuplalue;
                    hrcomponent.CompanyId = companyId;
                    hrcomponent.CreatedBy = userId;
                    hrcomponent.ModifiedBy = hrcomponent.CreatedBy;
                    hrcomponent.IsDeleted = false;
                    isSaved = hrcomponent.Save();
                    break;
                case "JoiningDocument":
                    JoiningDocument joiningdocument;
                    if (dataValue.Id != Guid.Empty)
                        joiningdocument = new JoiningDocument(dataValue.Id, companyId);
                    else
                        joiningdocument = new JoiningDocument();
                    joiningdocument.DocumentName = dataValue.popuplalue;
                    joiningdocument.CompanyId = companyId;
                    joiningdocument.CreatedBy = userId;
                    joiningdocument.ModifiedBy = joiningdocument.CreatedBy;
                    joiningdocument.IsDeleted = false;
                    isSaved = joiningdocument.Save();
                    break;
                // Modified By Keerthika on 17/04/2017
                case "ptlocation":
                    PTLocationList ptlist = new PTLocationList(companyId);
                    bool Ptax_status = false;
                    var pt_list = ptlist.Where(d => d.PTLocationName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    PTLocation PTLoc;
                    if (pt_list.Count() == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            PTLoc = new PTLocation(dataValue.Id, companyId);
                        else
                            PTLoc = new PTLocation();
                        PTLoc.PTLocationName = dataValue.popuplalue;
                        PTLoc.CompanyId = companyId;
                        PTLoc.CreatedBy = userId;
                        PTLoc.ModifiedBy = PTLoc.CreatedBy;
                        PTLoc.IsDeleted = false;
                        isSaved = PTLoc.Save();
                    }
                    else
                    {
                        pt_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {

                                PTLoc = new PTLocation();
                                PTLoc.PTLocationName = dataValue.popuplalue;
                                PTLoc.CompanyId = companyId;
                                PTLoc.Id = dataValue.Id;
                                PTLoc.CreatedBy = userId;
                                PTLoc.ModifiedBy = PTLoc.CreatedBy;
                                PTLoc.IsDeleted = false;
                                isSaved = PTLoc.Save();
                                Ptax_status = true;
                            }
                        });
                        if (Ptax_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }
                    }

                    break;
                //Modified By Keerthika on 17/04/2017
                case "bank":
                    BankList banklist = new BankList(companyId);
                    bool bank_status = false;
                    var bank_list = banklist.Where(d => d.BankName.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();
                    Bank bank;
                    if (bank_list.Count() == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            bank = new Bank(dataValue.Id, companyId);
                        else
                            bank = new Bank();
                        bank.BankName = dataValue.popuplalue;
                        bank.CreatedOn = DateTime.Now;
                        bank.ModifiedOn = DateTime.Now;
                        bank.CreatedBy = userId.ToString();
                        bank.ModifiedBy = bank.CreatedBy.ToString();
                        bank.IsActive = false;
                        bank.CompanyId = companyId;
                        isSaved = bank.Save();
                    }
                    else
                    {
                        bank_list.ForEach(k =>
                        {
                            if (k.Id == dataValue.Id)

                            {
                                bank = new Bank();
                                bank.BankName = dataValue.popuplalue;
                                bank.CreatedOn = DateTime.Now;
                                bank.ModifiedOn = DateTime.Now;
                                bank.CreatedBy = userId.ToString();
                                bank.Id = dataValue.Id;
                                bank.ModifiedBy = bank.CreatedBy.ToString();
                                bank.IsActive = false;
                                isSaved = bank.Save();
                                bank_status = true;
                            }
                        });
                        if (bank_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }

                    }
                    break;
                case "slab":
                    TXSlabCategory slab;
                    if (dataValue.Id != Guid.Empty)
                        slab = new TXSlabCategory(dataValue.Id);
                    else
                        slab = new TXSlabCategory();

                    slab.Name = dataValue.popuplalue;
                    slab.CompanyId = companyId;
                    slab.CreatedBy = userId;
                    slab.ModifiedBy = userId;
                    slab.IsDeleted = false;
                    isSaved = slab.Save();
                    break;
                case "leaveType":
                    LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                    LeaveType levType;
                    if (dataValue.Id != Guid.Empty)
                        levType = new LeaveType(dataValue.Id, companyId,DefaultFinancialYr.Id);
                    else
                        levType = new LeaveType();
                    levType.LeaveTypeName = dataValue.popuplalue;
                    levType.CreatedOn = DateTime.Now;
                    levType.ModifiedOn = DateTime.Now;
                    levType.CompanyId = companyId;
                    levType.CreatedBy = userId;
                    levType.ModifiedBy = userId;
                    levType.IsDeleted = false;
                    isSaved = levType.Save();
                    break;

                case "languagesknown":
                    Language language = new Language();
                    List<Language> languagelist = language.LanguagesList(companyId);
                    bool language_status = false;
                    var language_list = languagelist.Where(d => d.Name.ToLower().Trim() == dataValue.popuplalue.ToLower().Trim()).ToList();

                    if (language_list.Count() == 0)
                    {
                        if (dataValue.Id != Guid.Empty)
                            language = new Language((dataValue.Id), companyId);
                        else
                            language = new Language();

                        language.Name = dataValue.popuplalue;
                        language.CreatedOn = DateTime.Now;
                        language.ModifiedOn = DateTime.Now;
                        language.CreatedBy = userId.ToString();
                        language.ModifiedBy = language.CreatedBy.ToString();
                        language.IsActive = false;
                        language.CompanyId = companyId;
                        isSaved = language.Save();
                    }
                    else
                    {
                        languagelist.ForEach(k =>
                        {
                            if (k.Id == (dataValue.Id))

                            {
                                language = new Language();
                                language.Name = dataValue.popuplalue;
                                language.CreatedOn = DateTime.Now;
                                language.ModifiedOn = DateTime.Now;
                                language.CreatedBy = userId.ToString();
                                language.Id = dataValue.Id;
                                language.ModifiedBy = language.CreatedBy.ToString();
                                language.IsActive = false;
                                isSaved = language.Save();
                                bank_status = true;
                            }
                        });
                        if (language_status == false)
                        {
                            return base.BuildJson(false, 200, "Already exist " + dataValue.popuplalue, dataValue);
                        }

                    }
                    break;
                default:
                    break;
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

        public JsonResult GetPopupData(Guid id, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            popup data = new popup();
            switch (type)
            {
                case "category":
                    Category cat = new Category(id, companyId);
                    data = new popup { Id = cat.Id, popuplalue = cat.Name, type = "category", DisOrder = cat.DisOrder };
                    break;
                case "branch":
                    Branch branch = new Branch(id, companyId);
                    data = new popup { Id = branch.Id, popuplalue = branch.BranchName, type = "branch" };
                    break;
                case "designation":
                    Designation designation = new Designation(id, companyId);
                    data = new popup { Id = designation.Id, popuplalue = designation.DesignationName, type = "designation" };
                    break;

                case "costCentre":
                    CostCentre costcentre = new CostCentre(id, companyId);
                    data = new popup { Id = costcentre.Id, popuplalue = costcentre.CostCentreName, type = "costCentre" };
                    break;
                case "esiLocation":
                    EsiLocation esilocation = new EsiLocation(id, companyId);
                    data = new popup { Id = esilocation.Id, popuplalue = esilocation.LocationName, isApplicable = esilocation.isApplicable, employerCode = esilocation.EmployerCode, type = "esiLocation" };
                    break;
                case "grade":
                    Grade grade = new Grade(id, companyId);
                    data = new popup { Id = grade.Id, popuplalue = grade.GradeName, type = "grade" };
                    break;
                case "esiDespensary":
                    EsiDespensary esiDespensary = new EsiDespensary(id, companyId);
                    data = new popup { Id = esiDespensary.Id, popuplalue = esiDespensary.ESIDespensary, type = "esiDespensary" };
                    break;
                case "department":
                    Department department = new Department(id, companyId);
                    data = new popup { Id = department.Id, popuplalue = department.DepartmentName, type = "department" };
                    break;
                case "location":
                    Location location = new Location(id, companyId);
                    data = new popup { Id = location.Id, popuplalue = location.LocationName, type = "location" };
                    break;
                case "HRComponent":
                    HRComponent hrComponent = new HRComponent(id, companyId);
                    data = new popup { Id = hrComponent.Id, popuplalue = hrComponent.Name, type = "HRComponent" };
                    break;
                case "JoiningDocument":
                    JoiningDocument joiningdocument = new JoiningDocument(id, companyId);
                    data = new popup { Id = joiningdocument.Id, popuplalue = joiningdocument.DocumentName, type = "JoiningDocument" };
                    break;
                case "ptlocation":
                    PTLocation PTLoc = new PTLocation(id, companyId);
                    data = new popup { Id = PTLoc.Id, popuplalue = PTLoc.PTLocationName, type = "PTLocation" };
                    break;
                //case "PTax":
                //    PTax PTax = new PTax(id, companyId);
                //    data = new popup { Id = PTax.Id, popuplalue = PTax.ptlocation, type = "PTax" };
                //    break;
                case "bank":
                    Bank bank = new Bank(id, companyId);
                    data = new popup { Id = bank.Id, popuplalue = bank.BankName, type = "Bank" };
                    break;
                case "slab":
                    TXSlabCategory slab = new TXSlabCategory(id);
                    data = new popup { Id = slab.Id, popuplalue = slab.Name, type = "slab" };
                    break;
                case "leaveType":
                    LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                    LeaveType leaveType = new LeaveType(id, companyId,DefaultFinancialYr.Id);
                    data = new popup { Id = leaveType.Id, popuplalue = leaveType.LeaveTypeName, type = "leaveType" };
                    break;
                case "languagesknown":
                    Language lang = new Language((id), companyId);
                    data = new popup { Id = lang.Id, popuplalue = lang.Name, type = "languagesknown" };
                    break;
                default:
                    break;
            }
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult DeletePopupData(Guid id, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            popup data = new popup();
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            bool usedstatus = false;


            EmployeeList emplist = new EmployeeList(companyId, Guid.Empty);
            switch (type)
            {

                case "category":
                    //int count = (emplist.Count(u => u.CategoryId == id));

                    if (emplist.Where(u => u.CategoryId == id).ToList().Count == 0)
                    {

                        Category cat = new Category(id, companyId);
                        cat.CreaateBy = userId;
                        cat.ModifiedBy = cat.CreaateBy;
                        cat.IsDeleted = true;
                        isDeleted = cat.Delete();

                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;

                case "branch":

                    if (emplist.Where(u => u.Branch == id).ToList().Count == 0)
                    {
                        Branch branch = new Branch(id, companyId);
                        branch.CreatedBy = userId;
                        branch.ModifiedBy = branch.CreatedBy;
                        branch.IsDeleted = true;
                        isDeleted = branch.Delete();

                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "designation":
                    if (emplist.Where(u => u.Designation == id).ToList().Count == 0)
                    {
                        Designation designation;
                        designation = new Designation(id, companyId);
                        designation.CreatedBy = userId;
                        designation.ModifiedBy = designation.CreatedBy;
                        designation.IsDeleted = true;
                        isDeleted = designation.Delete();

                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "costCentre":
                    if (emplist.Where(u => u.CostCentre == id).ToList().Count == 0)
                    {
                        CostCentre costCentre;
                        costCentre = new CostCentre(id, companyId);
                        costCentre.CreatedBy = userId;
                        costCentre.ModifiedBy = costCentre.CreatedBy;
                        costCentre.IsDeleted = true;
                        isDeleted = costCentre.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "esiLocation":
                    if (emplist.Where(u => u.ESILocation == id).ToList().Count == 0)
                    {
                        EsiLocation esiLocation;
                        esiLocation = new EsiLocation(id, companyId);
                        esiLocation.CreatedBy = userId;
                        esiLocation.ModifiedBy = esiLocation.CreatedBy;
                        esiLocation.IsDeleted = true;
                        isDeleted = esiLocation.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "grade":
                    if (emplist.Where(u => u.Grade == id).ToList().Count == 0)
                    {
                        Grade grade;
                        grade = new Grade(id, companyId);
                        grade.CreatedBy = userId;
                        grade.ModifiedBy = grade.CreatedBy;
                        grade.IsDeleted = false;
                        isDeleted = grade.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "esiDespensary":
                    if (emplist.Where(u => u.ESIDespensary == id).ToList().Count == 0)
                    {
                        EsiDespensary esiDespensary;
                        esiDespensary = new EsiDespensary(id, companyId);
                        esiDespensary.CreatedBy = userId;
                        esiDespensary.ModifiedBy = esiDespensary.CreatedBy;
                        esiDespensary.IsDeleted = false;
                        isDeleted = esiDespensary.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "department":
                    if (emplist.Where(u => u.Department == id).ToList().Count == 0)
                    {
                        Department department;
                        department = new Department(id, companyId);
                        department.CreatedBy = userId;
                        department.ModifiedBy = department.CreatedBy;
                        department.IsDeleted = false;
                        isDeleted = department.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "location":
                    if (emplist.Where(u => u.Location == id).ToList().Count == 0)
                    {
                        Location location;
                        location = new Location(id, companyId);
                        location.CreatedBy = userId;
                        location.ModifiedBy = location.CreatedBy;
                        location.IsDeleted = false;
                        isDeleted = location.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "HRComponent":
                    if (emplist.Where(u => u.Location == id).ToList().Count == 0)
                    {
                        HRComponent hrComponent;
                        hrComponent = new HRComponent(id, companyId);
                        hrComponent.CreatedBy = userId;
                        hrComponent.ModifiedBy = hrComponent.CreatedBy;
                        hrComponent.IsDeleted = false;
                        isDeleted = hrComponent.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "JoiningDocument":
                    JoiningDocument joiningdocument;
                    joiningdocument = new JoiningDocument(id, companyId);
                    joiningdocument.CreatedBy = userId;
                    joiningdocument.ModifiedBy = joiningdocument.CreatedBy;
                    joiningdocument.IsDeleted = false;
                    isDeleted = joiningdocument.Delete();
                    break;
                case "ptlocation":
                    if (emplist.Where(u => u.PTLocation == id).ToList().Count == 0)
                    {
                        PTLocation PTLoc;
                        PTLoc = new PTLocation(id, companyId);
                        PTLoc.CreatedBy = userId;
                        PTLoc.ModifiedBy = PTLoc.CreatedBy;
                        PTLoc.IsDeleted = false;
                        isDeleted = PTLoc.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "bank":
                    if (emplist.Where(u => u.PTLocation == id).ToList().Count == 0)
                    {
                        Bank bank;
                        bank = new Bank(id, companyId);
                        bank.CreatedBy = userId.ToString();
                        bank.ModifiedBy = bank.CreatedBy;
                        bank.IsActive = false;
                        isDeleted = bank.Delete();
                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                case "leaveType":
                    LeaveRequestList req = new LeaveRequestList(id);



                    //if (req.Where(u => u.LeaveType == id).ToList().Count == 0)
                    if (req[0].Leaveopening == 0 && req[0].Leavecredits == 0)
                    {
                        LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                        LeaveType leavtype;
                        leavtype = new LeaveType(id, companyId,DefaultFinancialYr.Id);
                        if (leavtype.LeaveTypeName == "LOSS OF PAY DAYS")
                        {
                            return base.BuildJson(false, 100, "LOSS OF PAY DAYS should not be deleted", data);
                        }
                        else
                        {
                            leavtype.CreatedBy = userId;
                            leavtype.ModifiedBy = leavtype.CreatedBy;
                            leavtype.IsDeleted = false;
                            isDeleted = leavtype.Delete();
                        }


                    }
                    else
                    {
                        usedstatus = true;
                    }
                    break;
                default:
                    break;
            }
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data deleted successfully", data);
            }
            else
            {
                if (usedstatus == true)
                {
                    return base.BuildJson(false, 100, "Already Used.", data);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while Deleting the data.", data);
                }

            }

        }

        public JsonResult GetPopUpDatas(string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            List<popup> data = new List<popup>();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            switch (type.ToLower())
            {
                case "category":
                    CategoryList cat = new CategoryList(companyId);
                    cat.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "category" });
                    });
                    break;
                case "designation":
                    DesignationList designationlist = new DesignationList(companyId);
                    designationlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DesignationName, type = "designation" });
                    });
                    break;
                case "branch":
                    BranchList branchlist = new BranchList(companyId);
                    branchlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.BranchName, type = "branch" });
                    });
                    break;
                case "costcentre":
                    CostCentreList costcentrelist = new CostCentreList(companyId);
                    costcentrelist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.CostCentreName, type = "costCentre" });
                    });

                    break;
                case "esilocation":
                    EsiLocationList esilocationlist = new EsiLocationList(companyId);
                    esilocationlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LocationName, isApplicable = u.isApplicable, employerCode = u.EmployerCode, type = "esiLocation" });
                    });

                    break;
                case "grade":
                    GradeList gradelist = new GradeList(companyId);
                    gradelist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.GradeName, type = "grade" });
                    });

                    break;
                case "esidespensary":
                    ESIDespensaryList esiDespensarylist = new ESIDespensaryList(companyId);
                    esiDespensarylist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.ESIDespensary, type = "esiDespensary" });
                    });

                    break;
                case "department":
                    DepartmentList departmentlist = new DepartmentList(companyId);
                    departmentlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DepartmentName, type = "department" });
                    });

                    break;
                case "location":
                    LocationList locationList = new LocationList(companyId);
                    locationList.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LocationName, type = "location" });
                    });

                    break;
                case "hrcomponent":
                    HRComponentList HRComp = new HRComponentList(companyId);
                    HRComp.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "HRComponent" });
                    });
                    break;
                case "joiningdocument":
                    JoiningDocumentList joidoc = new JoiningDocumentList(companyId);
                    joidoc.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DocumentName, type = "JoiningDocument" });
                    });
                    break;
                case "ptlocation":
                    PTLocationList PTLoc = new PTLocationList(companyId);
                    PTLoc.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.PTLocationName, type = "ptlocation" });
                    });
                    break;
                //case "PTax":
                //    PTaxList PTax = new PTaxList(companyId);
                //    PTax.ForEach(u =>
                //    {
                //        data.Add(new popup { Id = u.Id, popuplalue = u.ptlocation, type = "PTax" });
                //    });
                //    break;
                case "bank":
                    BankList bank = new BankList(companyId);
                    bank.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.BankName, type = "bank" });
                    });
                    break;
                case "slab":
                    TXSlabCategoryList slab = new TXSlabCategoryList(companyId);
                    slab.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "slab" });
                    });
                    break;
                case "leavetype":
                    LeaveTypeList levType = new LeaveTypeList(companyId);
                    levType.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LeaveTypeName, type = "leaveType" });
                    });
                    break;
                case "languagesknown":
                    Language lang = new Language();
                    List<Language> langList = lang.LanguagesList(companyId);
                    langList.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "languagesknown" });
                    });
                    break;
                default:
                    break;
            }
            return base.BuildJson(true, 200, "success", data);
        }

        //-----------------------Created by Keerthika on 31/05/2017--
        public JsonResult GetPopUpDataComp(string type, int companyId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            List<popup> data = new List<popup>();
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            switch (type)
            {
                case "category":
                    CategoryList cat = new CategoryList(companyId);
                    cat.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "category" });
                    });
                    break;
                case "designation":
                    DesignationList designationlist = new DesignationList(companyId);
                    designationlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DesignationName, type = "designation" });
                    });
                    break;
                case "branch":
                    BranchList branchlist = new BranchList(companyId);
                    branchlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.BranchName, type = "branch" });
                    });
                    break;
                case "costCentre":
                    CostCentreList costcentrelist = new CostCentreList(companyId);
                    costcentrelist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.CostCentreName, type = "costCentre" });
                    });

                    break;
                case "esiLocation":
                    EsiLocationList esilocationlist = new EsiLocationList(companyId);
                    esilocationlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LocationName, isApplicable = u.isApplicable, employerCode = u.EmployerCode, type = "esiLocation" });
                    });

                    break;
                case "grade":
                    GradeList gradelist = new GradeList(companyId);
                    gradelist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.GradeName, type = "grade" });
                    });

                    break;
                case "esiDespensary":
                    ESIDespensaryList esiDespensarylist = new ESIDespensaryList(companyId);
                    esiDespensarylist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.ESIDespensary, type = "esiDespensary" });
                    });

                    break;
                case "department":
                    DepartmentList departmentlist = new DepartmentList(companyId);
                    departmentlist.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DepartmentName, type = "department" });
                    });

                    break;
                case "location":
                    LocationList locationList = new LocationList(companyId);
                    locationList.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LocationName, type = "location" });
                    });

                    break;
                case "HRComponent":
                    HRComponentList HRComp = new HRComponentList(companyId);
                    HRComp.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "HRComponent" });
                    });
                    break;
                case "JoiningDocument":
                    JoiningDocumentList joidoc = new JoiningDocumentList(companyId);
                    joidoc.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.DocumentName, type = "JoiningDocument" });
                    });
                    break;
                case "ptlocation":
                    PTLocationList PTLoc = new PTLocationList(companyId);
                    PTLoc.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.PTLocationName, type = "ptlocation" });
                    });
                    break;
                //case "PTax":
                //    PTaxList PTax = new PTaxList(companyId);
                //    PTax.ForEach(u =>
                //    {
                //        data.Add(new popup { Id = u.Id, popuplalue = u.ptlocation, type = "PTax" });
                //    });
                //    break;
                case "bank":
                    BankList bank = new BankList(companyId);
                    bank.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.BankName, type = "bank" });
                    });
                    break;
                case "slab":
                    TXSlabCategoryList slab = new TXSlabCategoryList(companyId);
                    slab.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.Name, type = "slab" });
                    });
                    break;
                case "leaveType":
                    LeaveTypeList levType = new LeaveTypeList(companyId);
                    levType.ForEach(u =>
                    {
                        data.Add(new popup { Id = u.Id, popuplalue = u.LeaveTypeName, type = "leaveType" });
                    });
                    break;
                default:
                    break;
            }
            return base.BuildJson(true, 200, "success", data);
        }
        //-------------------------
        public ActionResult Do(string actionVal)
        {
            switch (actionVal)
            {
                case "Company":
                    return PartialView("~/Views/Company/CompanyView.cshtml");
                case "JoiningDocument":
                    return PartialView("~/Views/Company/JoiningDocumentList.cshtml");
                case "Category":
                    return PartialView("~/Views/Company/CategoryList.cshtml");
                case "PTax":
                    return PartialView("~/Views/Company/PTaxList.cshtml");
                case "Lwf":
                    return PartialView("~/Views/Company/LWF.cshtml");
                case "Employee":
                    return PartialView("~/Views/Company/EmployeeList.cshtml");
                //Modified by madhavan on 07/07/2023
                case "Flexi Benefit":
                    return PartialView("~/Views/Company/SalaryMaster.cshtml");
                //Test
                case "FlexiPay Benefit":
                    return PartialView("~/Views/Company/FlexiPay.cshtml");
                case "ProofAssignUsers":
                    return PartialView("~/Views/TaxSection/ProofAssignUsers.cshtml");
                case "FlexipaySummary":
                    return PartialView("~/Views/DataWizard/FlexiPaySummary.cshtml");
                case "Expense Assign":
                    return PartialView("~/Views/Employee/ExpenseMgrAssign.cshtml");
                case "approveExpenseStatus":
                    return PartialView("~/Views/Employee/ExpenseApprove.cshtml");
                case "EmployeeNew":
                    return PartialView("~/Views/Shared/EmployeeDetails.cshtml");
                case "PayslipNew":
                    return PartialView("~/Views/Employee/EmpPayslipGenerate.cshtml");// PartialView("~/Views/Employee/SalaryView.cshtml");
                case "TdsView":
                    return PartialView("~/Views/TaxTransaction/TDSWorksheet.cshtml");//"~/Views/TaxTransaction/TDSWorksheet.cshtml");
                case "tdsworksheetexcel":
                    return PartialView("~/Views/TaxTransaction/WorkSheetExcel.cshtml");
                case "tdsForm16PartB":
                    return PartialView("~/Views/TaxTransaction/Form16PartB.cshtml");
                case "Form24Q":
                    return PartialView("~/Views/TaxTransaction/Form24Q.cshtml");
                case "Form24QA":
                    return PartialView("~/Views/TaxTransaction/Form24QA.cshtml");
                case "HrComponent":
                    return PartialView("~/Views/Company/HrComponentList.cshtml");
                //case "EmpDocument":
                //    return PartialView("~/Views/Company/CompanyView.cshtml");               
                case "Popup":
                    return PartialView("~/Views/Company/PopupList.cshtml");
                case "Entitymodel":
                    return PartialView("~/Views/Company/Entitymodel.cshtml");
                case "DynamicEntity":
                    return PartialView("~/Views/Company/DynamicEntity.cshtml");
                case "LoanEntry":
                    return PartialView("~/Views/Loan/LoanEntryList.cshtml");
                case "Separation":
                    // return PartialView("~/Views/Transaction/Separation.cshtml");
                    return PartialView("~/Views/Employee/Emp_SeparationList.cshtml");
                case "Release":
                    return PartialView("~/Views/Transaction/Release.cshtml");
                case "Increment":
                    return PartialView("~/Views/Transaction/Increment.cshtml");
                case "ArrearView":
                    return PartialView("~/Views/Transaction/ArrearView.cshtml");
                case "LoanMaster":
                    return PartialView("~/Views/Loan/LoanMasterList.cshtml");
                case "Baentry":
                    return PartialView("~/Views/Employee/Emp_BAentry.cshtml");
                case "Baupd":
                    return PartialView("~/Views/Employee/Emp_BAupd.cshtml");
                case "User":
                    return PartialView("~/Views/Admin/UserList.cshtml");
                case "Role":
                    return PartialView("~/Views/Admin/RoleList.cshtml");
                case "EmployeeCodeChange":
                    return PartialView("~/Views/Admin/EmployeeCodeTransfer.cshtml");
                case "FormRights":
                    return PartialView("~/Views/Admin/FormRights.cshtml");
                case "StopPayment":
                    return PartialView("~/Views/Transaction/StopPaymentList.cshtml");
                case "Setting":
                    return PartialView("~/Views/Setting/Setting.cshtml");
                case "PayslipSetting":
                    return PartialView("~/Views/DataWizard/Index.cshtml");
                case "PreviousComponentsSetting":
                    return PartialView("~/Views/Setting/PreviousComponentsSetting.cshtml");
                case "PremiumSetting":
                    return PartialView("~/Views/Setting/PremiumSetting.cshtml");
                case "MonthlyInput":
                    return PartialView("~/Views/Employee/MonthlyInput.cshtml");
                case "FullFinalSettlement":
                    return PartialView("~/Views/Transaction/FullFinalSettlement.cshtml");
                case "PayrollProcess":
                    return PartialView("~/Views/Employee/PayrollProcess.cshtml");
                case "PayslipGeneration":
                    return PartialView("~/Views/Employee/PayslipGeneration.cshtml");
                case "Import":
                    return PartialView("~/Views/Util/EmployeeImport.cshtml");
                //case "Import":
                //    return PartialView("~/Views/Util/Singleimport.cshtml");
                case "TaxImport":
                    return PartialView("~/Views/Util/Import.cshtml");
                case "paysheet":
                    return PartialView("~/Views/DataWizard/Paysheet.cshtml");
                case "pfChallan":
                    return PartialView("~/Views/PFChallan/TemplateEdit.cshtml");
                case "rploanDetails":
                    return PartialView("~/Views/Reports/LoanReport.cshtml");
                case "RoleFormCammandSetting":
                    return PartialView("~/Views/Setting/RoleFormCommandSetting.cshtml");
                case "EditTransactionField":
                    return PartialView("~/Views/Admin/EditTransactionField.cshtml");
                case "lopCredit":
                    return PartialView("~/Views/Transaction/LOPCredit.cshtml");
                case "supplementaryDays":
                    return PartialView("~/Views/Transaction/LOPCredit.cshtml");
                case "itax":
                    return View("~/Views/Home/TaxIndex.cshtml");
                case "payroll":
                    return View("~/Views/Home/Index.cshtml");
                case "financeYear":
                    return PartialView("~/Views/TaxSection/FinanceYearList.cshtml");
                case "txSection":
                    return PartialView("~/Views/TaxSection/TaxSectionList.cshtml");
                case "txSubSection":
                    return PartialView("~/Views/TaxSection/TaxSubSectionList.cshtml");
                case "slab":
                    return PartialView("~/Views/TaxSection/TaxSlabList.cshtml");
                case "incomeMatching":
                    return PartialView("~/Views/TaxSection/IncomeMatchingList.cshtml");
                case "sectionMatching":
                    return PartialView("~/Views/TaxSection/SectionMatchingList.cshtml");
                case "DeclarationEntry":
                    return PartialView("~/Views/TaxTransaction/DeclarationEntry.cshtml");
                case "challanEntry":
                    return PartialView("~/Views/TaxTransaction/Form16AChallanEntry.cshtml");
                case "challanView":
                    return PartialView("~/Views/TaxTransaction/Form16AChallanView.cshtml");
                case "HRAPropertiy":
                    return PartialView("~/Views/TaxSection/TAXHRAExemption.cshtml");
                case "taxComponent":
                    return PartialView("~/Views/Company/TaxEntityModel.cshtml");
                case "taxComputation":
                    return PartialView("~/Views/Company/TaxBehavior.cshtml");
                case "otherIncomeHead":
                    return PartialView("~/Views/TaxSection/OtherIncomeHeadsList.cshtml");
                case "taxProcess":
                    return PartialView("~/Views/TaxTransaction/TaxProcess.cshtml");
                case "taxEmpProcess":
                    return PartialView("~/Views/TaxTransaction/TaxEmpProcess.cshtml");
                case "Uploadproof":
                    return PartialView("~/Views/TaxSection/SavgProof.cshtml");
                case "Viewproof":
                    return PartialView("~/Views/TaxSection/SavgProofView.cshtml");
                case "Checkproof":
                    return PartialView("~/Views/Verify/CheckProof.cshtml");
                case "Verifyproof":
                    return PartialView("~/Views/Verify/VerifyProof.cshtml");
                case "tdsworksheet":
                    return PartialView("~/Views/TaxTransaction/TaxReport.cshtml");
                case "tdsstatement":
                    return PartialView("~/Views/TaxTransaction/TDSStatement.cshtml");
                case "previousemployertds":
                    return PartialView("~/Views/TaxTransaction/TDSPreviousEmployer.cshtml");
                case "tdsExcelReport":
                    return PartialView("~/Views/TaxTransaction/TaxDeclarationReport.cshtml");
                case "rptpf":
                    return PartialView("~/Views/Reports/PFReport.cshtml");
                case "rptesi":
                    return PartialView("~/Views/Reports/EsiReport.cshtml");
                case "leavefinanceYear":
                    return PartialView("~/Views/Leave/LeaveFinanceYearList.cshtml");
                case "leaveOpenings":
                    return PartialView("~/Views/Leave/LeaveOpenings.cshtml");
                case "leaveCreditProcess":
                    return PartialView("~/Views/Leave/LeaveCreditProcess.cshtml");
                case "assignmanager":
                    return PartialView("~/Views/Leave/AssignManagerNew.cshtml");
                case "ChgAssignMgr":
                    return PartialView("~/Views/Leave/ChangeAssignManager.cshtml");
                case "leaveRequest":
                    TempData["raisedBy"] = "Employee";
                    return PartialView("~/Views/Leave/LeaveRequest.cshtml");
                case "CompoffRequest":
                    TempData["raisedBy"] = "Employee";
                    return PartialView("~/Views/Leave/CompoffRequest.cshtml");
                //Modified by madhavan on 20/09/2023
                case "Employee Expense":
                    return PartialView("~/Views/Employee/Emp_Expenses.cshtml");
                case "leaveEntryHR":
                    TempData["raisedBy"] = "HR";
                    return PartialView("~/Views/Leave/HRLEAVEREQUEST.cshtml");
                case "HRApproverejection":
                    TempData["raisedBy"] = "HR";
                    return PartialView("~/Views/Leave/HRAPPROVEREJECT.cshtml");
                case "approveLeaveStatus":
                    return PartialView("~/Views/Leave/LeaveDetail.cshtml");
                case "HRapprovecancel":
                    return PartialView("~/Views/Leave/HRApproveCancel.cshtml");
                case "ManagerEligiblity":
                    return PartialView("~/Views/Leave/ManagerEligiblity.cshtml");
                case "debitLeave":
                    return PartialView("~/Views/Leave/DebitLeave.cshtml");
                case "CreditLeave":
                    return PartialView("~/Views/Leave/CreditLeave.cshtml");
                case "HolidaySettings":
                    return PartialView("~/Views/Leave/HolidayList.cshtml");
                case "HolidayList":
                    return PartialView("~/Views/Leave/EmployeeHolidayList.cshtml");
                case "LeaveSettings":
                    return PartialView("~/Views/Leave/LeaveSettings.cshtml");
                case "WeekOffSetting":
                    return PartialView("~/Views/Leave/WeekoffSettings.cshtml");
                case "FullLeaveReport":
                    return PartialView("~/Views/Leave/EmployeeLeaveReport.cshtml");
                case "ApprovedReport":
                    return PartialView("~/Views/Leave/ApprovedLeave.cshtml");
                case "RejectedReport":
                    return PartialView("~/Views/Leave/RejectedLeave.cshtml");
                case "PendingReport":
                    return PartialView("~/Views/Leave/PendingLeave.cshtml");
                case "DebitReport":
                    return PartialView("~/Views/Leave/DebitReport.cshtml");
                case "CreditReport":
                    return PartialView("~/Views/Leave/CreditReport.cshtml");
                case "ManagerDebitReport":
                    return PartialView("~/Views/Leave/ManagerDebitReport.cshtml");
                case "ManagerCreditReport":
                    return PartialView("~/Views/Leave/ManagerCreditReport.cshtml");
                case "HRDebitReport":
                    return PartialView("~/Views/Leave/HRDebitReport.cshtml");
                case "HRCreditReport":
                    return PartialView("~/Views/Leave/HRCreditReport.cshtml");
                case "userReport":
                    return PartialView("~/Views/Leave/UserReport.cshtml");
                case "CancelledReport":
                    return PartialView("~/Views/Leave/CancelledLeave.cshtml");
                case "mailConfig":
                    return PartialView("~/Views/Leave/MailConfiguration.cshtml");
                case "ManagerViewReport":
                    return PartialView("~/Views/Leave/AssignManagerViewReport.cshtml");
                case "HRViewReport":
                    return PartialView("~/Views/Leave/HRViewReport.cshtml");
                case "NewLeaveOpeningMasterImp":
                    return PartialView("~/Views/Leave/LeaveOpeningImportMaster.cshtml");
                case "ManagerLeaveBalanceReport":
                    return PartialView("~/Views/Leave/ManagerReportlevbalance.cshtml");
                case "HRLeaveBalanceReport":
                    return PartialView("~/Views/Leave/LeaveBalanceReport.cshtml");
                case "WeekOffcmpmatching":
                    return PartialView("~/Views/Leave/Weekoffcomponentmatching.cshtml");
                case "MonthlyLeaveSettings":
                    return PartialView("~/Views/Leave/MonthlyLeaveList.cshtml");
                case "LeaveCreditSettings":
                    return PartialView("~/Views/Leave/LeaveCreditlist.cshtml");
                case "Form-16":
                    return PartialView("~/Views/TaxTransaction/Form_16.cshtml");
                case "showpassword":
                    return PartialView("~/Views/Shared/ShowPassword.cshtml");
                case "MultiEntry":
                    return PartialView("~/Views/Employee/MultiEntry.cshtml");
                case "PayslipTreeView":
                    return PartialView("~/Views/Employee/SalaryView.cshtml");
                case "Form16PartBTreeView":
                    return PartialView("~/Views/Employee/Form16TreeView.cshtml");
                case "Form16PartATreeView":
                    return PartialView("~/Views/Employee/Form16PartATreeView.cshtml");
                case "Form12BATreeView":
                    return PartialView("~/Views/Employee/Form12BATreeView.cshtml");
                case "StaticEmailTemplate":
                    return PartialView("~/Views/EmailTemplate/EMailTemplate.cshtml");
                case "TimeOfficeSetting":
                    return PartialView("~/Views/TimeOfficeSetting/TimeOfficeSettingMenu.cshtml");
                case "SalarySummary":
                    return PartialView("~/Views/DataWizard/SalarySummary.cshtml");
                case "LeaveCalendar":
                    return PartialView("~/Views/Company/LeaveCalendar.cshtml");
                case "AppCancelReport":
                    return PartialView("~/Views/Leave/ApprovedCancelReport.cshtml");
                case "ViewAllDeclarationEntry":
                    return PartialView("~/Views/TaxTransaction/ViewAllDeclarationEntry.cshtml");
                case "PayrollDashboard":
                    return PartialView("~/Views/Shared/PayrollDashboard.cshtml");
                case "compofftracking":
                    return PartialView("~/Views/Leave/CompOffGainHistory.cshtml");
                case "managercompofftracking":
                    return PartialView("~/Views/Leave/ManagerCompOffHistoryReport.cshtml");
                case "Compoffgainrpt":
                    return PartialView("~/Views/Leave/CompoffGainReport.cshtml");
                case "ManagerCompoffGainHistory":
                    return PartialView("~/Views/Leave/ManagerCompoffGainReport.cshtml");
                case "HRcompofftracking":
                    return PartialView("~/Views/Leave/HRCompOffHistoryReport.cshtml");
                case "Resignation":
                    return PartialView("~/Views/Employee/Emp_ResignationList.cshtml");
                case "YearEndingProcess":
                    return PartialView("~/Views/Leave/LeaveYearEndProcess.cshtml");
                default:
                    return PartialView("~/Views/Company/LogoView.cshtml");
                    
            }
        }
        [HttpPost]
        public ActionResult DoModule(string actionVal, string date = null, string id = null)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            Session["CompLogo"] = Convert.ToString(Session["CompLogo"]).Replace("../", "");
            string logopath = Convert.ToString(Session["CompLogo"]);
            switch (actionVal)
            {
                case "timeoffice":
                    TempData["layout"] = "timeoffice";
                    Session["Title"] = "TimeOffice";
                    return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));
                case "itax":
                    TempData["layout"] = "tax";
                    Session["Title"] = "Tax";
                    return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));

                case "payroll":

                    TempData["layout"] = "pay";
                    Session["Title"] = "Payroll";
                    return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));

                case "leave":
                    TempData["layout"] = "leave";
                    Session["Title"] = "Leave";
                    Session["CompLogo"] = logopath == "" ? "" : ("../" + Convert.ToString(Session["CompLogo"]));
                    return base.BuildJson(true, 0, "", Url.Action("LeaveIndex", "Home"));

                case "LeaveCalendarDetailsRender":
                    TempData["layout"] = "leave";
                    TempData["date"] = date;
                    TempData["id"] = id;
                    Session["Title"] = "Leave";
                    return base.BuildJson(true, 0, "", Url.Action("LeaveCalendarIndex", "Home"));
                default:
                    TempData["layout"] = "pay";
                    Session["Title"] = "Payroll";
                    return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));

            }

        }
        public ActionResult ClickModule(string actionVal)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            //switch (actionVal)
            //{
            //    case "unameid":
            //    //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //    //if (companyId == 0)
            //    //{

            //    //}
            //    //    return base.BuildJson(true, 0, "", Url.Action("CompanyIndex", "Company"));
            //    case "company":

            //        return base.BuildJson(true, 0, "", Url.Action("CompanyIndex", "Company"));

            //    case "settings":


            //        return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));

            //    case "changepass":

            //        return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));
            //    default:

            //        return base.BuildJson(true, 0, "", Url.Action("Index", "Home"));


            //}

            return base.BuildJson(true, 200, "success", actionVal);
        }
        public JsonResult GetTableCategory()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TableCategoryList dataList = new TableCategoryList(companyId);
            dataList.ForEach(u =>
            {
                u.EntityModelList.Remove(u.EntityModelList.Where(d => d.Name.Trim() == "ITax").FirstOrDefault());
            });

            //TableCategory dataPayroll = dataList.Where(u => u.Name == "Payroll").FirstOrDefault(); 
            //EntityModel Itax= dataPayroll.EntityModelList.Where(d => d.Name.Trim() == "ITax").FirstOrDefault();
            //dataPayroll.EntityModelList.Remove(Itax);
            //List<TableCategory> WPayrolldata = dataList.Where(u => u.Name != "Payroll").ToList();
            //WPayrolldata.Add(dataPayroll);
            return base.BuildJson(true, 200, "success", dataList);

        }

        public JsonResult GetEntityAttributeModel(Guid tablecategoryId, Guid enityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModel data = new EntityModel(tablecategoryId, enityModelId);
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult GetAttributeModelList(string type, bool takFPF = false)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelTypeList result = new AttributeModelTypeList();
            AttributeModelTypeList data = new AttributeModelTypeList(companyId);
            if (type != "All")
            {
                if (type == "TaxField")
                {
                    data.Where(e => e.Name == "Tax").ToList().ForEach(u =>
                    {
                        AttributeModelList attr = new AttributeModelList();
                        attr.AddRange(u.AttributeModelList.FilterByContributionType(1).Where(e => !e.IsSetting).ToList());
                        u.AttributeModelList = attr;
                        result.Add(u);
                    });
                }
                else
                {
                    data.Where(e => e.Name == type).ToList().ForEach(u =>
                    {
                        u.AttributeModelList = u.AttributeModelList.FilterByContributionType(1);
                        result.Add(u);

                    });
                    if (takFPF)
                    {
                        data.Where(e => e.Name == "Deduction").ToList().ForEach(u =>
                        {
                            AttributeModel att = u.AttributeModelList.Where(a => a.Name == "FPF").FirstOrDefault();


                            result[0].AttributeModelList.Add(att);
                        });

                    }
                }

            }
            else
            {
                data.ForEach(u =>
                {
                    if (u.Name != "Tax")
                    {
                        u.AttributeModelList = u.AttributeModelList.FilterByContributionType(1);
                        result.Add(u);
                    }

                });
            }
            return base.BuildJson(true, 200, "success", result);
        }
         
        public JsonResult GetFlexiPayComponentSelect()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelType attributes = new AttributeModelType();
            var Datavalue = attributes.Flexipay(companyId, Guid.Empty);
            return base.BuildJson(true, 200, "success", Datavalue);
        }
        public JsonResult SaveFlexiPayComponent(JsonFlexi Component)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelType retobj = new AttributeModelType();
            retobj.CompanyId = companyId;
            retobj.CreatedBy = userId;
            retobj.Name = Component.Name;
            retobj.IsFlexiPay = Component.FlxiPay;
            retobj.IsBasicPay = Component.Basicpay;
            retobj.MasterCompentId = Guid.Empty;
            retobj.DisplayOrder =  null;
            retobj.IsActive = true;
            retobj.IsReadOnly = false;
            if (retobj.SaveFlexiComponent())
            {
                return base.BuildJson(true, 200, "Data saved successfully", retobj);

            }
            return base.BuildJson(false, 100, "There is some error while saving the data.", "");
        }
        public JsonResult SaveFlexiPayOrder(JsonFlexi dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelType retobj = new AttributeModelType();
            retobj.CompanyId = companyId;
            retobj.ModifiedBy = userId;
            retobj.Name = dataValue.Name;
            retobj.Id = dataValue.SelectorId;
            retobj.IsReadOnly = dataValue.IsReadOnly;
            retobj.IsActive = true;
            retobj.MasterCompentId = dataValue.Id;
            retobj.FixedAmount = dataValue.FixedAmount;
            retobj.DisplayOrder = dataValue.DisplayOrder;
            if(retobj.SaveFlexiComponent())
            {
                return base.BuildJson(true, 200, "Data saved successfully", retobj);
            }
            return base.BuildJson(false, 100, "There is some error while saving the data.", "");
        }
        public JsonResult GetAttributeModelData(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModel data = new AttributeModel(id, companyId);
            return base.BuildJson(true, 200, "success", data);
        }
        public JsonResult GetAttributeModelData_Field(string id, bool incudeMaster)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (Convert.ToString(Session["Title"]) == "Tax")
            {
                incudeMaster = true;
            }
            AttributeModelList attributeModelList = new AttributeModelList(companyId); // Guid.Empty, companyId);
            AttributeModel IsDuplicate = new AttributeModel();
            IsDuplicate = attributeModelList.FirstOrDefault(u => u.Name.ToUpper().Trim() == id.ToUpper().Trim());
            //if (incudeMaster)
            //{
            //    IsDuplicate = attributeModelList.FirstOrDefault(u => u.Name == id);
            //}
            //else
            //{
            //    IsDuplicate = attributeModelList.FirstOrDefault(u => u.Name == id && u.BehaviorType != "Master");
            //}
            if (object.ReferenceEquals(IsDuplicate, null))
            {
                return base.BuildJson(true, 200, "success", new { isexist = false });
            }
            else
            {
                return base.BuildJson(true, 200, "success", new jsonattribute { isexist = true, id = IsDuplicate.Id, name = IsDuplicate.Name });
            }

        }
        public JsonResult GetHRComponents()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //companyId = 1;
            HRComponentList HRComp = new HRComponentList(companyId);
            return new JsonResult { Data = HRComp };
        }


        public JsonResult GetPTLocations()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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


        public JsonResult GetPTaxData(Guid ptaxptlocation)
        {
            int companyid = Convert.ToInt32(Session["CompanyId"]);
            PTax ptax = new PTax(ptaxptlocation, companyid);
            jsonPTax jsonPtax = jsonPTax.tojson(ptax);
            return base.BuildJson(true, 200, "success", jsonPtax);
        }


        public JsonResult SavePTax(jsonPTax dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PTax ptax = jsonPTax.convertobject(dataValue);
            ptax.CompanyId = companyId;
            ptax.CreatedBy = userId;
            if (ptax.Save())
            {




                //Delete Ptax range before saving Data that's why pass ptax id
                PTaxRange delptrange = new PTaxRange();
                delptrange.Id = ptax.Id;
                delptrange.ModifiedBy = userId;
                delptrange.Delete();
                dataValue.jsonPTaxRange.ForEach(u =>
                {
                    PTaxRange ptrange = JsonPTaxRange.convertobject(u);
                    if (ptrange.IsDeleted)
                    {
                        ptrange.Delete();
                    }
                    else
                    {
                        ptrange.PTaxId = ptax.Id;
                        ptrange.CreatedBy = userId;
                        ptrange.ModifiedBy = userId;
                        ptrange.Save();
                    }

                });
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult Savelwf(jsonLWF dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            LWFSetting lwfSetting = jsonLWF.convertobject(dataValue);
            lwfSetting.CompanyId = companyId;
            lwfSetting.CreatedBy = userId;
            if (lwfSetting.Save())
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult GetLWFSetting(Guid locationId)
        {
            int companyid = Convert.ToInt32(Session["CompanyId"]);
            LWFSetting lwf = new LWFSetting(locationId, companyid);
            jsonLWF jsonlwf = jsonLWF.tojson(lwf);
            return base.BuildJson(true, 200, "success", jsonlwf);
        }
        //--------------------------------







        //--------------------------------

        public JsonResult GetLeaveType()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            LeaveTypeList comp = new LeaveTypeList(companyId, DefaultFinancialYr.Id);
            return new JsonResult { Data = comp };

        }
        public JsonResult GetMasterLeaveType()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveTypeList comp = new LeaveTypeList(companyId);
            return new JsonResult { Data = comp };

        }
    }
    public class JsonFlexi
    {
        public Guid Id { get; set; }
        public Guid SelectorId { get; set; }
        public int DisplayOrder { get; set; }
        public string FixedAmount { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public bool Basicpay { get; set; }
        public bool FlxiPay { get; set; }
    }
    public class popup
    {
        public string popuplalue { get; set; }

        public int DisOrder { get; set; }
        public bool isApplicable { get; set; }
        public string employerCode { get; set; }
        public Guid Id { get; set; }
        public string type { get; set; }
        public string colour { get; set; }
        public int LangId { get; set; }
    }
    public class jsonattribute
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public bool isexist { get; set; }
    }
    public class jsonCompanyConnection
    {
        public Guid Cid { get; set; }
        public int CPrimaryCompId { get; set; }
        public int CUserId { get; set; }
        public string CConnString { get; set; }
        public static jsonCompanyConnection tojson(CompanyConnection companyConn)
        {
            return new jsonCompanyConnection()
            {
                Cid = companyConn.Id,
                CPrimaryCompId = companyConn.PrimaryCompanyId,
                CUserId = companyConn.UserId,
                CConnString = companyConn.ConnectionString

            };
        }
        public static CompanyConnection convertobject(jsonCompanyConnection companyConn)
        {
            return new CompanyConnection()
            {
                Id = companyConn.Cid,
                PrimaryCompanyId = companyConn.CPrimaryCompId,
                UserId = companyConn.CUserId,
                ConnectionString = companyConn.CConnString
            };
        }
    }

    public class jsonPTax
    {
        public Guid ptaxid { get; set; }
        public int ptaxCompId { get; set; }
        public Guid ptaxptlocation { get; set; }
        public string ptaxpFormNo { get; set; }
        public string ptaxCalculationtype { get; set; }
        public string ptaxdeductionmonth1 { get; set; }
        public string ptaxdeductionmonth2 { get; set; }
        public string ptaxRegCertificateNo { get; set; }
        public string ptaxPTOCircleNo { get; set; }

        public List<JsonPTaxRange> jsonPTaxRange { get; set; }
        public static jsonPTax tojson(PTax pTax)
        {
            jsonPTax jsnPtax = new jsonPTax();
            jsnPtax.ptaxid = pTax.Id;
            jsnPtax.ptaxCompId = pTax.CompanyId;
            jsnPtax.ptaxptlocation = pTax.PTLocation;
            jsnPtax.ptaxpFormNo = pTax.FormNo;
            jsnPtax.ptaxCalculationtype = pTax.Calculationtype;
            jsnPtax.ptaxdeductionmonth1 = pTax.DeductionMonth1;
            jsnPtax.ptaxdeductionmonth2 = pTax.DeductionMonth2;
            jsnPtax.ptaxRegCertificateNo = pTax.RegCertificateNo;
            jsnPtax.ptaxPTOCircleNo = pTax.PTOCircleNo;
            jsnPtax.jsonPTaxRange = new List<JsonPTaxRange>();
            pTax.PTaxRangeLists.ForEach(u =>
            {
                jsnPtax.jsonPTaxRange.Add(JsonPTaxRange.tojson(u));
            });
            return jsnPtax;
        }
        public static PTax convertobject(jsonPTax pTax)
        {
            PTaxRangeList plist = new PTaxRangeList();
            pTax.jsonPTaxRange.ForEach(p =>
            {
                plist.Add(JsonPTaxRange.convertobject(p));
            });
            return new PTax()
            {
                Id = pTax.ptaxid,
                CompanyId = pTax.ptaxCompId,
                PTLocation = pTax.ptaxptlocation,
                FormNo = pTax.ptaxpFormNo,
                Calculationtype = pTax.ptaxCalculationtype,
                DeductionMonth1 = pTax.ptaxdeductionmonth1,
                DeductionMonth2 = pTax.ptaxdeductionmonth2,
                RegCertificateNo = pTax.ptaxRegCertificateNo,
                PTOCircleNo = pTax.ptaxPTOCircleNo,
                PTaxRangeLists = plist

            };
        }

    }

    public class JsonPTaxRange
    {
        public Guid ptrangeid { get; set; }
        public Guid ptaxid { get; set; }
        public bool deleted { get; set; }
        public decimal ptrangeRangeFrom { get; set; }
        public decimal ptrangeRangeTo { get; set; }
        public decimal ptrangeRangeAmt { get; set; }



        public static JsonPTaxRange tojson(PTaxRange pTRange)
        {
            return new JsonPTaxRange()
            {
                ptrangeid = pTRange.Id,
                ptaxid = pTRange.PTaxId,
                ptrangeRangeFrom = pTRange.RangeFrom,
                ptrangeRangeTo = pTRange.RangeTo,
                ptrangeRangeAmt = pTRange.Amt,
                deleted = pTRange.IsDeleted

            };
        }
        public static PTaxRange convertobject(JsonPTaxRange pTRange)
        {
            return new PTaxRange()
            {
                Id = pTRange.ptrangeid,
                PTaxId = pTRange.ptaxid,
                RangeFrom = pTRange.ptrangeRangeFrom,
                RangeTo = pTRange.ptrangeRangeTo,
                Amt = pTRange.ptrangeRangeAmt,
                IsDeleted = pTRange.deleted
            };
        }
    }

    public class jsonLWF
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public Guid locationId { get; set; }
        public int applyMonth { get; set; }
        public decimal employeeAmt { get; set; }
        public decimal employerAmt { get; set; }



        public static jsonLWF tojson(LWFSetting lwfSetting)
        {
            jsonLWF jsnPtax = new jsonLWF();
            jsnPtax.id = lwfSetting.Id;
            jsnPtax.companyId = lwfSetting.CompanyId;
            jsnPtax.locationId = lwfSetting.LocationId;
            jsnPtax.applyMonth = lwfSetting.ApplyMonth;
            jsnPtax.employeeAmt = lwfSetting.EmployeeAmount;
            jsnPtax.employerAmt = lwfSetting.EmployerAmount;
            return jsnPtax;
        }
        public static LWFSetting convertobject(jsonLWF jlwf)
        {
            return new LWFSetting()
            {
                Id = jlwf.id,
                CompanyId = jlwf.companyId,
                LocationId = jlwf.locationId,
                ApplyMonth = jlwf.applyMonth,
                EmployeeAmount = jlwf.employeeAmt,
                EmployerAmount = jlwf.employerAmt
            };
        }

    }
}