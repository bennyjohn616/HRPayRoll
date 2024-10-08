using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using TraceError;
using Payroll.CustomFilter;
using SelectPdf;
using System.IO;
using PayrollBO.Leave;
using System.Data;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class LoanController : BaseController//Controller
    {
        //
        // GET: /Loan/

        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            return View();
        }


        public JsonResult GetLoanEntry(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LoanEntryList loanEntrylist = new LoanEntryList(employeeId);
            LoanMasterList loanmasterlist = new LoanMasterList(companyId);
            List<jsonLoanEntry> jsondata = new List<jsonLoanEntry>();
            loanEntrylist.ForEach(u => { jsondata.Add(jsonLoanEntry.tojson(u, loanmasterlist)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult pdf(Guid Id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LoanEntry loan = new LoanEntry(Id);
            string pdfPath = string.Empty;
            string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
            Employee emp =new Employee(loan.EmployeeId);
            pdfPath = reportPath + "/" + emp.EmployeeCode + ".pdf";
            string Authority = Request.Url.ToString();

            Authority= Authority.Replace("Loan/pdf",string.Empty);

            string baseUr = Authority;
            ErrorLog.LogTestWrite(baseUr);
            GetPdf(pdfPath, loan, companyId, baseUr);
            
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = pdfPath });
        }


        public JsonResult SaveLoanEntry(jsonLoanEntry dataValue)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            var employee = dataValue.employeeId;
            DateTime loandate = Convert.ToDateTime(dataValue.loanDate);
            DateTime applymonth = Convert.ToDateTime(dataValue.loanApplyMonthYear);
            Employee employeeDetails = new Employee(companyId, employee);
            var loandatecheck = employeeDetails.DateOfJoining <= loandate;
            if (loandatecheck == false)
            {
                return base.BuildJson(false, 100, "Loan date is " + loandate.ToString("dd/MMM/yyyy").TrimEnd(',') + " lesser the employee joining date", dataValue);
            }
            int year = applymonth.Year;
            int month = applymonth.Month;
            PayrollHistory payrollHistory = new PayrollHistory(companyId, employee, year, month);
            if (payrollHistory.Status == "Processed" || payrollHistory.PayrollHistoryValueList.Count != 0)
            {
                return base.BuildJson(false, 100, "Payroll is already processed for applied month and year", dataValue);
            }

            bool Transactioneligible = true;
            bool LoanEligible = true;
            string Errmsg = string.Empty;
            DateTime newapply = Convert.ToDateTime(dataValue.loanApplyMonthYear);
            int install = dataValue.NoofInstall;


            LoanEntryList loanEntrylist = new LoanEntryList(employee, dataValue.loanMasterId);

            //loanEntrylist.ForEach(loan =>
            //{
            //    if (loan.Id != dataValue.loanEntryid)
            //    {
            //        LoanTransactionList loanTransList = new LoanTransactionList(loan.Id);
            //        for (int m = 0; m < install; m++)
            //        {
            //            DateTime ldate = Convert.ToDateTime(dataValue.loanApplyMonthYear).AddMonths(m);
            //            int Transactioncheck = loanTransList.Where(d => d.AppliedOn.Year == ldate.Year && d.AppliedOn.Month == ldate.Month).Count();
            //            if (Transactioncheck > 0)
            //            {
            //                Errmsg = Errmsg + ldate.ToString("MMMM yyyy") + ",";
            //                Transactioneligible = false;
            //            }
            //        }
            //    }
            //});


            LoanEligible = this.LoanEligTest(dataValue.employeeId, dataValue.loanMasterId, Convert.ToDouble(dataValue.loanAmt));

            //for (int i=0;i<loanEntrylist.Count;i++)
            //{
            //    LoanTransactionList loanTransList = new LoanTransactionList(loanEntrylist[i].Id);
            //    //loanTransList.Where(u=>u.AppliedOn.Year== dataValue.loanApplyMonthYear.ye)
            //    //int install = loanEntrylist[i].NoOfMonths;
            //    DateTime ldate = loanEntrylist[i].ApplyDate.AddMonths(install);
            //    DateTime loandate = new DateTime(ldate.Year, ldate.Month, DateTime.DaysInMonth(ldate.Year, ldate.Month));
            //    //string apply = loandate.ToString("dd/MMM/yyyy");
            //   // DateTime newapply = Convert.ToDateTime(dataValue.loanApplyMonthYear);
            //    if (newapply<=loandate.AddMonths(-1) && loanEntrylist[i].LoanMasterId==dataValue.loanMasterId && loanEntrylist[i].Id!= dataValue.loanEntryid)
            //    {
            //        return base.BuildJson(false, 100, "You are already applied loan on this date", dataValue);
            //    }
            //}

            if (Transactioneligible)
            {
                if (LoanEligible)
                {
                    LoanEntry loanentry = jsonLoanEntry.convertobject(dataValue);
                    bool isSaved = false;
                    //int companyId = Convert.ToInt32(Session["CompanyId"]);
                    //int userId = Convert.ToInt32(Session["UserId"]);

                    loanentry.CreatedBy = userId;
                    loanentry.ModifiedBy = loanentry.CreatedBy;
                    loanentry.IsDeleted = false;


                    isSaved = loanentry.Save();


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
                    return base.BuildJson(false, 100, "Applied amount Exceeds the Eligiblity Amount ", dataValue);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "Following Applied Month(s) are : " + Errmsg.TrimEnd(',') + " Already exists.", dataValue);
            }

        }
        public bool LoanEligTest(Guid EmpId, Guid loanMasterId, double LoanAmount)
        {
            bool EligFlag = false;
            double PayProcessVal = 0;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int month = 0;
            int year = 0;
            PayrollHistoryList objPayHistLst = new PayrollHistoryList(EmpId, companyId, month, year);
            var FiltLst = objPayHistLst.Where(n => n.CreatedOn == objPayHistLst.Max(p => (p.CreatedOn))).FirstOrDefault();
            PayrollHistoryValueList objPayHistVal = new PayrollHistoryValueList(FiltLst.Id);
            LoanMasterList objLoanmasterLst = new LoanMasterList(companyId);
            var filtLstLoan = objLoanmasterLst.Where(q => q.Id == loanMasterId).FirstOrDefault();
            if (filtLstLoan.loanEligComp != Guid.Empty)
            {
                var FiltValuLst = objPayHistVal.Where(e => e.AttributeModelId == filtLstLoan.loanEligComp).FirstOrDefault();
                PayProcessVal = Convert.ToDouble(FiltValuLst.Value);
                //if (LoanAmount >= PayProcessVal)   //It was commented in order to solve the allowing equal  Eligiblity amount issue.
                if (LoanAmount > PayProcessVal)
                {
                    EligFlag = false;
                }
                else
                {
                    EligFlag = true;
                }
            }
            else
            {
                EligFlag = true;
            }
            return EligFlag;
        }

        public JsonResult SaveLoanTrans(List<jsonLoanTrans> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool status = true;
            foreach (jsonLoanTrans trans in dataValue)
            {
                if (status)
                {
                    LoanTransaction loanTran = new LoanTransaction();
                    loanTran.Id = trans.id;
                    loanTran.LoanEntryId = trans.loanEntryId;
                    loanTran.Status = "UnPaid";
                    loanTran.isPayRollProcess = true;
                    loanTran.AmtPaid = trans.amtPaid;
                    loanTran.InterestAmt = trans.interestAmt;
                    DateTime ldate = Convert.ToDateTime(trans.appliedOn);
                    loanTran.AppliedOn = new DateTime(ldate.Year, ldate.Month, DateTime.DaysInMonth(ldate.Year, ldate.Month));
                    loanTran.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    if (trans.deleted)
                    {
                        status = loanTran.Delete();
                    }
                    else
                    {
                        status = loanTran.Save();
                    }
                }
            }

            if (status)
            {

                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult GetLoanTrans(jsonLoanEntry dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);


            LoanTransactionList loanTrans = new LoanTransactionList(dataValue.loanEntryid);

            List<jsonLoanTrans> jsondata = new List<jsonLoanTrans>();
            loanTrans.ForEach(l =>
            {
                jsondata.Add(jsonLoanTrans.toJson(l));
            });


            return base.BuildJson(true, 200, "Data Saved successfully", jsondata);
        }
        public JsonResult ForeCloseSaveLoanEntry(jsonLoanEntry dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            bool ISsaved = true;
            LoanEntry loanEntry = new LoanEntry(dataValue.loanEntryid);
            loanEntry.Reason = dataValue.reason;
            loanEntry.ModifiedBy = userId;
            //var translist = loanEntry.LoanTransactionList;
            if (dataValue.foreclosed)
            {
                loanEntry.ForeCloseReverseDate = Convert.ToDateTime(dataValue.forecloseOrReversedDate);
                LoanTransactionList translistrf = new LoanTransactionList(dataValue.loanEntryid);
                var reversecheck = translistrf.Where(s => s.isForClose == true && s.isPayRollProcess == true).ToList();
                if (reversecheck.Count != 0)
                {
                    return base.BuildJson(false, 100, "You Cant Reverse,PayRoll Get processed", loanEntry);
                }
                else
                {
                    ISsaved = loanEntry.ForeCloseReverse();
                    if (ISsaved)
                    {

                        return base.BuildJson(true, 200, "Data saved successfully", loanEntry);
                    }
                    else
                    {
                        return base.BuildJson(false, 100, "There is some error while saving the data.", loanEntry);
                    }
                }

            }
            else
            {
                loanEntry.ForeCloseDate = Convert.ToDateTime(dataValue.forecloseOrReversedDate);
                LoanTransactionList translistrf = new LoanTransactionList(dataValue.loanEntryid);
                var foreclosecheck = translistrf.Where(s => s.Status != "Paid" && s.isPayRollProcess == false).First();
                DateTime date = foreclosecheck.AppliedOn;
                if (date.Month != loanEntry.ForeCloseDate.Month || date.Year != loanEntry.ForeCloseDate.Year)
                {
                    return base.BuildJson(false, 100, "Date Range should be in between " + date.Month + "th" + " " + " Month of  " + date.Year + " " + " Year", loanEntry);
                }
                ISsaved = loanEntry.ForeClose();
                if (ISsaved)
                {

                    return base.BuildJson(true, 200, "Data saved successfully", loanEntry);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while saving the data.", loanEntry);
                }
            }

            //return base.BuildJson(true, 200, "Data saved successfully", loanEntry);

        }

        public JsonResult GetLoanEntryData(jsonLoanEntry dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);


            LoanEntry loanEntry = new LoanEntry(dataValue.loanEntryid);
            LoanMasterList loanmasterlist = new LoanMasterList(companyId);
            jsonLoanEntry jsondata = new jsonLoanEntry();
            jsondata = jsonLoanEntry.tojson(loanEntry, loanmasterlist);

            return base.BuildJson(true, 200, "Data Saved successfully", jsondata);
        }

        public JsonResult DeleteLoanEntryData(Guid id)
        {
            try
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);

                bool isDeleted = false;
                bool status = false;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                LoanEntry LoanEntry;
                LoanEntry = new LoanEntry(id);
                LoanTransactionList LoantranObj = new LoanTransactionList(id);
                LoantranObj.ForEach(u =>
                {
                    if (u.Status == "Paid")
                    {
                        status = true;
                    }
                });
                if (status != true)
                {
                    LoanEntry.Id = id;
                    LoanEntry.CreatedBy = userId;
                    LoanEntry.ModifiedBy = LoanEntry.CreatedBy;
                    LoanEntry.IsDeleted = false;
                    isDeleted = LoanEntry.Delete();
                    return base.BuildJson(true, 200, "Data deleted successfully", LoanEntry);
                }
                else
                {
                    isDeleted = false;
                    return base.BuildJson(false, 100, "Payroll is Processed or loan is Foreclosed ,So you cant able to delete this loan entry", LoanEntry);
                }
            }


            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", "");
            }



            //if (isDeleted)
            //{
            //    return base.BuildJson(true, 200, "Data deleted successfully", LoanEntry);
            //}
            //else
            //{
            //    return base.BuildJson(false, 100, "There is some error while Deleting the data.", LoanEntry);
            //}

        }

        //public JsonResult DeleteLoanTrans(Guid id)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);

        //    bool isDeleted = false;
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    int userId = Convert.ToInt32(Session["UserId"]);

        //    LoanTransactionList loanTrans = new LoanTransactionList(id);
        //     LoanTransaction loanTran = new LoanTransaction();
        //    loanTran.CreatedBy = userId;
        //    loanTran.ModifiedBy = loanTran.CreatedBy;
        //    loanTran.IsDeleted = false;
        //    isDeleted = loanTran.Delete();

        //    if (isDeleted)
        //    {
        //        return base.BuildJson(true, 200, "Data deleted successfully", loanTrans);
        //    }
        //    else
        //    {
        //        return base.BuildJson(false, 100, "There is some error while Deleting the data.", loanTrans);
        //    }

        //}
        public class jsonLoanEntry
        {
            public Guid loanEntryid { get; set; }
            public Guid loanMasterId { get; set; }
            public Guid employeeId { get; set; }
            public string loanName { get; set; }
            public string loanDate { get; set; }
            public string loanApplyMonthYear { get; set; }
            public decimal loanAmt { get; set; }
            public int NoofInstall { get; set; }
            public decimal Permonth { get; set; }
            public decimal interestrate { get; set; }

            public bool foreclosed { get; set; }

            public string reason { get; set; }

            public string forecloseOrReversedDate { get; set; }

            public List<jsonLoanTrans> loanTrans { get; set; }

            public decimal amtPaid { get; set; }

            public static jsonLoanEntry tojson(LoanEntry loanEntry, LoanMasterList loanmasterlist)
            {
                jsonLoanEntry retObject = new jsonLoanEntry()
                {

                    loanEntryid = loanEntry.Id,
                    employeeId = loanEntry.EmployeeId,
                    loanMasterId = loanEntry.LoanMasterId,
                    loanName = loanmasterlist.Where(u => u.Id == loanEntry.LoanMasterId).FirstOrDefault().LoanDesc,
                    loanDate = loanEntry.LoanDate != DateTime.MinValue ? loanEntry.LoanDate.ToString("dd/MMM/yyyy") : "",
                    loanApplyMonthYear = loanEntry.ApplyDate != DateTime.MinValue ? loanEntry.ApplyDate.ToString("MMMM yyyy") : "",
                    loanAmt = loanEntry.LoanAmt,
                    NoofInstall = loanEntry.NoOfMonths,
                    Permonth = loanEntry.AmtPerMonth,
                    foreclosed = loanEntry.IsForeClose,
                    reason = loanEntry.Reason,
                    forecloseOrReversedDate = loanEntry.ForeCloseDate == DateTime.MinValue ? Convert.ToString(loanEntry.ForeCloseDate) : Convert.ToString(loanEntry.ForeCloseReverseDate),

                    loanTrans = new List<jsonLoanTrans>()
                };

                loanEntry.LoanTransactionList.ForEach(u => { retObject.loanTrans.Add(jsonLoanTrans.toJson(u)); });
                if (retObject.loanTrans.Count() > 0)
                {

                    //retObject.amtPaid = retObject.loanAmt - retObject.loanTrans.AsEnumerable().Sum(x => x.amtPaid);
                    //var AmtToPay = retObject.loanTrans.Where(d => d.status == "Paid").AsEnumerable().Sum(x => x.amtPaid);
                    retObject.amtPaid = retObject.loanTrans.Where(d => d.status == "Paid").AsEnumerable().Sum(x => x.amtPaid);
                }
                return retObject;


            }
            public static LoanEntry convertobject(jsonLoanEntry loanEntry)
            {
                return new LoanEntry()
                {
                    Id = loanEntry.loanEntryid,
                    EmployeeId = loanEntry.employeeId,
                    LoanMasterId = loanEntry.loanMasterId,
                    LoanDate = loanEntry.loanDate != string.Empty ? Convert.ToDateTime(loanEntry.loanDate) : DateTime.Now,
                    interest = loanEntry.interestrate,
                    ApplyDate = loanEntry.loanApplyMonthYear != string.Empty ? Convert.ToDateTime(loanEntry.loanApplyMonthYear) : DateTime.Now,
                    LoanAmt = loanEntry.loanAmt,
                    NoOfMonths = loanEntry.NoofInstall,
                    AmtPerMonth = loanEntry.Permonth,
                    IsForeClose = loanEntry.foreclosed,
                    ForeCloseDate = loanEntry.forecloseOrReversedDate == "" ? DateTime.Now : Convert.ToDateTime(loanEntry.forecloseOrReversedDate),
                    ForeCloseReverseDate = loanEntry.forecloseOrReversedDate == "" ? DateTime.Now : Convert.ToDateTime(loanEntry.forecloseOrReversedDate),
                    Reason = loanEntry.reason

                };
            }


        }

        public class jsonLoanMaster
        {
            public Guid loanid { get; set; }

            public Guid attributeModelid { get; set; }
            public Guid loanEligComp { get; set; }
            public string loanCode { get; set; }
            public string loanDesc { get; set; }
            public bool loanIsInterest { get; set; }
            public double loanInterestPercent { get; set; }



            public static jsonLoanMaster tojson(LoanMaster loanMaster)
            {
                return new jsonLoanMaster()
                {
                    loanid = loanMaster.Id,
                    attributeModelid = loanMaster.AttributeModelId,
                    loanCode = loanMaster.LoanCode,
                    loanDesc = loanMaster.LoanDesc,
                    loanIsInterest = loanMaster.IsInterest,
                    loanInterestPercent = loanMaster.InterestPercent,
                    loanEligComp = loanMaster.loanEligComp

                };
            }
            public static LoanMaster convertobject(jsonLoanMaster loanMaster)
            {
                return new LoanMaster()
                {
                    Id = loanMaster.loanid,
                    AttributeModelId = loanMaster.attributeModelid,
                    LoanCode = loanMaster.loanCode,
                    LoanDesc = loanMaster.loanDesc,
                    IsInterest = loanMaster.loanIsInterest,
                    InterestPercent = loanMaster.loanInterestPercent,
                    loanEligComp = loanMaster.loanEligComp
                };
            }


        }
        public JsonResult GetLoanMaster()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LoanMasterList loanmasterlist = new LoanMasterList(companyId);
            List<jsonLoanMaster> jsondata = new List<jsonLoanMaster>();
            foreach (var item in loanmasterlist)
            {
                if (item.IsActive)
                {
                    jsondata.Add(jsonLoanMaster.tojson(item));
                }
            }
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult SaveLoanMasterData(jsonLoanMaster dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            bool isSavedLoanElig = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LoanMaster loanmaster = jsonLoanMaster.convertobject(dataValue);

            int userId = Convert.ToInt32(Session["UserId"]);
            loanmaster.CompanyId = companyId;
            loanmaster.CreatedBy = userId;
            loanmaster.ModifiedBy = loanmaster.CreatedBy;
            loanmaster.IsDeleted = false;

            isSaved = loanmaster.Save();
            if (isSaved)
            {
                isSavedLoanElig = loanmaster.SaveLoanEligSet();
                if (isSavedLoanElig)
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
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }
        public JsonResult GetLoanMasterData(jsonLoanMaster dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            LoanMaster loanmaster = new LoanMaster(dataValue.loanid, companyId);
            jsonLoanMaster jsondata = new jsonLoanMaster();
            jsondata = jsonLoanMaster.tojson(loanmaster);
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult DeleteLoanMasterData(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            //LoanData data = new LoanData();
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            LoanMaster loanMaster;
            loanMaster = new LoanMaster(id, companyId);
            loanMaster.CreatedBy = userId;
            loanMaster.ModifiedBy = loanMaster.CreatedBy;
            loanMaster.IsDeleted = false;
            isDeleted = loanMaster.Delete();

            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data deleted successfully", loanMaster);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", loanMaster);
            }

        }
        public JsonResult LoadDynamicPopupData(string popuptype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            List<jsonLoadDropdown> objDdlList = new List<jsonLoadDropdown>();
            LeaveSettingsBO dynamic1 = new LeaveSettingsBO();
            dynamic1.CompanyId = companyId;
            dynamic1.dynamicvalue = popuptype;
            DataTable dtcombo = dynamic1.GetDynamicvalue();
            if (dtcombo.Rows.Count > 0)
            {
                for (int i = 0; i < dtcombo.Rows.Count; i++)
                {
                    jsonLoadDropdown objDdl = new jsonLoadDropdown();
                    objDdl.Id = new Guid(dtcombo.Rows[i]["Id"].ToString());
                    objDdl.DropdownComponent = dtcombo.Rows[i]["name"].ToString();
                    objDdlList.Add(objDdl);
                }
                return base.BuildJson(true, 200, "", objDdlList);
            }
            else
            {
                return base.BuildJson(false, 100, "", objDdlList);
            }

        }

        public JsonResult LoadEligblityComponent()
        {
            try
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                AttributeModelTypeList data = new AttributeModelTypeList(companyId);
                var EarnVar = data.Where(g => g.Name == "Earning").FirstOrDefault();
                if (EarnVar != null)
                {
                    AttributeModelList AttLst = new AttributeModelList(companyId, EarnVar.Id);
                    return base.BuildJson(true, 200, "", AttLst);
                }
                return base.BuildJson(false, 100, "There is no Data to Bind", companyId);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, ex.Message, ex.Message);
            }



        }
        public static void GetPdf( string PDFFilePath,LoanEntry loan,int com,string baseurl)
        {
        

            //html
            string html = "";
            string header = "";
          
            html = "<html><style>body{padding:60px;}table.cl {border:none; border-collapse: collapse;}table.cl td.rw { border-left: 1px solid #000; border-right: 1px solid #000;}table.cl td.rw:first-child {border-left: none;}table.cl td.rw:last-child {border-right: none;}" +
            "table{max-width:2480px; width:100%; }tr.border_top ,td.bds{border-top:1pt solid black;}tr.border_bottom ,td.bd{border-bottom:1pt solid black;}.tab{ border:1px solid black;}</style><body>";


            //Header

           
            var mas =new LoanMaster(loan.LoanMasterId, com);
            Employee emp = new Employee(loan.EmployeeId);
            Company comp = new Company(com);
            if (comp.Companylogo != null)
            {
                comp.Companylogo = comp.Companylogo.Replace("~/", "");
                baseurl = baseurl + comp.Companylogo;
            }
            

                header = "<table>";


            if (comp.Companylogo != null)
            {
                header += "<tr>";
                header += "<td colspan=\"2\" style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "<img src =\"" + baseurl + "\"   height =100 width = 100 ></img >" + "</td>";
                header += "</tr>";
            }

            header += "<tr>";
            header += "<td colspan=\"2\" style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "<b>Company Name: </b>" + comp.CompanyName + "</td>";
            header += "</tr>";

            header += "<tr>";
            header += "<td colspan=\"2\" style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "<b>Company Address:</b> " + comp.AddressLine1 + comp.AddressLine2 + "</td>";
            header += "</tr>";

            header += "<tr>";
            header += "<td colspan=\"2\" style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "<b>Employee Code: </b>" + emp.EmployeeCode + "</td>";
            header += "</tr>";


            header += "<tr>";
            header += "<td class=\"bd\" colspan=\"2\" style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "<b>Employee Name: </b>" + emp.FirstName + emp.LastName + "</td>";
            header += "</tr>";



            header += "<tr>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Loan Description" + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + mas.LoanDesc + "</td>";
            header += "</tr>";           
                                         
                                         
            header += "<tr>";            
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Loan Date" + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + loan.LoanDate.ToString("dd/MM/yyyy") + "</td>";
            header += "</tr>";          
                                         
            header += "<tr>";            
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Loan ApplyOn Month And Year" + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + ((MonthEnum)Convert.ToInt16(loan.ApplyDate.Month)).ToString().ToUpper()+" "+ loan.ApplyDate.Year.ToString()+ "</td>";
            header += "</tr>";          
                                         
            header += "<tr>";            
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Loan Amount " + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + loan.LoanAmt.ToString("0.00") + "</td>";
            header += "</tr>";

            if (mas.InterestPercent > 0)
            {
                header += "<tr>";
                header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Loan Interest " + "</td>";
                header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + mas.InterestPercent.ToString()+"%" + "</td>";
                header += "</tr>";
            }
        

            header += "<tr>";            
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "No of Installments " + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + loan.NoOfMonths.ToString() + "</td>";
            header += "</tr>";          
                                        
            header += "<tr>";           
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + "Installment Per Month" + "</td>";
            header += "<td style=\"width:24cm;padding:10px;font-size: 20px;\" >" + loan.AmtPerMonth.ToString("0.00") + "</td>";
            header += "</tr>";

            header += "</table>";

            


            html += "<table class=\"tab\">";
           
            if (header != "<table></table>")
            {
                html += "<tr class=\"border_bottom\"><td   colspan = \"6\">" + header + " </td></tr>";
            }


            html += "</table></body></html> ";



            string pdf_page_size = "A4";
            PdfPageSize pageSize =
                (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);


            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;

            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(html, "");

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";




            using (FileStream fs = System.IO.File.Create(PDFFilePath))
            {
                fs.Write(pdf, 0, (int)pdf.Length);
            }



        }

    }
    public class jsonLoanTrans
    {
        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Get or Set the LoanEntryId
        /// </summary>
        public Guid loanEntryId { get; set; }

        /// <summary>
        /// Get or Set the AmtPaid
        /// </summary>
        public decimal amtPaid { get; set; }


        public decimal interestAmt { get; set; }
        /// <summary>
        /// Get or Set the isForClose
        /// </summary>
        public bool isForClose { get; set; }

        /// <summary>
        /// Get or Set the isPayRollProcess
        /// </summary>
        public bool isPayRollProcess { get; set; }

        /// <summary>
        /// Get or Set the AppliedOn
        /// </summary>
        public string appliedOn { get; set; }

        public string status { get; set; }

        public bool deleted { get; set; }
        public static jsonLoanTrans toJson(LoanTransaction ltrans)
        {
            return new jsonLoanTrans()
            {
                amtPaid = ltrans.AmtPaid,
                interestAmt = ltrans.InterestAmt,
                appliedOn = ltrans.AppliedOn != DateTime.MinValue ? ltrans.AppliedOn.ToString("dd/MMM/yyyy") : "",
                id = ltrans.Id,
                status = ltrans.Status,
                loanEntryId = ltrans.LoanEntryId
            };
        }
        public static LoanTransaction convertObject(jsonLoanTrans ltrans)
        {
            return new LoanTransaction()
            {
                AmtPaid = ltrans.amtPaid,
                InterestAmt = ltrans.interestAmt,
                LoanEntryId = ltrans.loanEntryId,
                AppliedOn = ltrans.appliedOn != string.Empty ? Convert.ToDateTime(ltrans.appliedOn) : DateTime.Now,
                Id = ltrans.id,
                Status = ltrans.status
            };
        }
    }
    public class jsonLoadDropdown
    {
        public Guid Id { get; set; }
        public string DropdownComponent { get; set; }
    }
}
