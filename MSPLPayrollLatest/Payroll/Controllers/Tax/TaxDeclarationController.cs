using PayrollBO;
using PayrollBO.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class TaxDeclarationController : BaseController
    {
        // GET: TaxDeclaration
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTaxDeclaration(Guid employeeId, DateTime effectiveDate, bool proof)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXEmployeeSectionList TXEmployeeSectionList = new TXEmployeeSectionList(employeeId, effectiveDate, proof);
            List<jsonTaxDeclaration> jsondata = new List<jsonTaxDeclaration>();

            TXEmployeeSectionList.ForEach(u => { jsondata.Add(jsonTaxDeclaration.toJson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);

        }

        public JsonResult GetProjIncome(Guid financeyear, Guid employeeId,int month,int year)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXProjIncomeList TXProjIncomeList = new TXProjIncomeList(financeyear,employeeId,month,year);
            List<jsonProjIncome> jsondata = new List<jsonProjIncome>();

            TXProjIncomeList.ForEach(u => { jsondata.Add(jsonProjIncome.toJson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);

        }

        public JsonResult SaveProjIncome(List<TXProjIncome> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            dataValue.ForEach(data =>
            {
                TXProjIncome txprj = data;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                isSaved = txprj.Save();

            });

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue );
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }




        public JsonResult GetChallanEmployeeEntry(jsonChallanEntry data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            List<ChallanEmployee> empList = new List<ChallanEmployee>();
            AttributeModelList AttributemodelList = new AttributeModelList(companyId);
            AttributeModel payatrr = AttributemodelList.Where(s => s.Name == "TDS").FirstOrDefault();
            TXFinanceYear fnyear = new TXFinanceYear(data.financialYear, companyId);
            DateTime startDate = fnyear.StartingDate;
            DateTime endDate = fnyear.EndingDate;
            int year;
            if (endDate.Month >= data.month)
            {
                year = fnyear.EndingDate.Year;
            }
            else
            {
                year = fnyear.StartingDate.Year;
            }
            data.categoryID.ForEach(f =>
            {
                EmployeeList emp = new EmployeeList(companyId, f);
                if (emp.Count > 0)
                {
                    List<TXChallanEntry> txchallanList = new TXChallanEntryList(data.financialYear, new DateTime(year, data.month, 1)).ToList();
                    emp.ForEach(e =>
                    {
                        TXChallanEntry tx = txchallanList.Where(c => c.EmployeeId == e.Id).FirstOrDefault();
                        if (object.ReferenceEquals(tx, null))
                        {
                            PayrollHistory payrollhistory = new PayrollHistory(companyId, e.Id, year, data.month);
                            if (!ReferenceEquals(payrollhistory, null))
                            {
                                PayrollHistoryValue paytax = payrollhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == payatrr.Id).FirstOrDefault();
                                if ((!ReferenceEquals(paytax, null)) && Convert.ToDecimal(paytax.Value) > 0)
                                {
                                    ChallanEmployee cEmp = new ChallanEmployee();
                                    cEmp.Id = e.Id;
                                    cEmp.EmployeeCode = e.EmployeeCode;
                                    cEmp.Name = e.FirstName + " " + e.LastName;
                                    cEmp.TaxAmount = Convert.ToDecimal(paytax.Value);
                                    empList.Add(cEmp);
                                }
                            }
                        }
                    });
                }

            });

            return base.BuildJson(true, 200, "success", empList);

        }
        public JsonResult GetChallanEmployeeView(Guid finId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            List<jsonChallanEntry> grpList = new List<jsonChallanEntry>();
            AttributeModelList AttributemodelList = new AttributeModelList(companyId);
            AttributeModel payatrr = AttributemodelList.Where(s => s.Name == "TDS").FirstOrDefault();
            TXFinanceYear fnyear = new TXFinanceYear(finId, companyId);

            TXChallanEntryList txChallanList = new TXChallanEntryList(finId, Guid.Empty);
            if (txChallanList.Count() > 0)
            {
                var groups = txChallanList.GroupBy(d => new { d.challanDate, d.bankId });

                foreach (var grp in groups)
                {
                    TXChallanEntryList txChallList = new TXChallanEntryList(finId, Guid.Empty);
                    jsonChallanEntry data = new jsonChallanEntry();
                    data.challanDate = grp.Key.challanDate;
                    data.bankName = new Bank(grp.Key.bankId, companyId).Id;
                    data.bank = new Bank(grp.Key.bankId, companyId).BankName;
                    data.challanAmount = txChallList.Where(w => (w.challanDate == grp.Key.challanDate) && (w.bankId == grp.Key.bankId)).Sum(s => s.TaxAmount);
                    data.PayrollMonth = txChallList.Where(w => (w.challanDate == grp.Key.challanDate) && (w.bankId == grp.Key.bankId)).FirstOrDefault().ApplyDate.ToString("MMMM");
                    grpList.Add(data);

                }
            }

            return base.BuildJson(true, 200, "success", grpList);

        }
        public JsonResult GetChallanEmployeeListView(Guid finId, DateTime challanDate, Guid bankId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<TXChallanEntry> empList = new List<TXChallanEntry>();
            TXChallanEntryList txChallList = new TXChallanEntryList(finId, Guid.Empty);
            if (txChallList.Count() > 0)
            {
                if (txChallList.Where(w => w.challanDate == challanDate && w.bankId == bankId).ToList().Count() > 0)
                {
                    txChallList.Where(w => w.challanDate == challanDate && w.bankId == bankId).ToList().ForEach(f =>
                    {
                        Employee employ = new Employee(f.EmployeeId);

                        TXChallanEntry TXChallan = new TXChallanEntry();
                        TXChallan.Id = f.Id;
                        TXChallan.EmployeeCode = employ.EmployeeCode;
                        TXChallan.Name = employ.FirstName + " " + employ.LastName;
                        TXChallan.FinanceYearId = f.FinanceYearId;
                        TXChallan.EmployeeId = f.EmployeeId;
                        TXChallan.bankId = f.bankId;
                        TXChallan.ApplyDate = f.ApplyDate;
                        TXChallan.challanDate = f.challanDate;
                        TXChallan.challanNo = f.challanNo;
                        TXChallan.checkdd = f.checkdd;
                        TXChallan.bookEntry = f.bookEntry;
                        TXChallan.BSRCode = f.BSRCode;
                        TXChallan.TaxAmount = Convert.ToDecimal(f.TaxAmount);
                        empList.Add(TXChallan);
                    });
                }
            }

            return base.BuildJson(true, 200, "success", empList);

        }
        public JsonResult GetLockLoad(int month, int year)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LockSetting selectVal = new LockSetting(month, year, companyId, "Select");
            if (selectVal.PaySheetLockid == Guid.Empty)
            {
                return base.BuildJson(false, 200, "success", null);
            }
            else
            {
                return base.BuildJson(true, 200, "success", selectVal);
            }
        }
        public JsonResult SaveChallanEntry(jsonChallanEntry data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int error = 0;
            AttributeModelList AttributemodelList = new AttributeModelList(companyId);
            AttributeModel payatrr = AttributemodelList.Where(s => s.Name == "TDS").FirstOrDefault();
            TXFinanceYear fnyear = new TXFinanceYear(data.financialYear, companyId);
            DateTime startDate = fnyear.StartingDate;
            DateTime endDate = fnyear.EndingDate;
            int year;
            if (endDate.Month >= data.month)
            {
                year = fnyear.EndingDate.Year;
            }
            else
            {
                year = fnyear.StartingDate.Year;
            }
            data.employeeID.ForEach(f =>
            {
                PayrollHistory payrollhistory = new PayrollHistory(companyId, f, year, data.month);
                if (!ReferenceEquals(payrollhistory, null))
                {
                    PayrollHistoryValue paytax = payrollhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == payatrr.Id).FirstOrDefault();
                    if ((!ReferenceEquals(paytax, null)) && Convert.ToDecimal(paytax.Value) > 0)
                    {
                        bool isSave = false;
                        TXChallanEntry TXChallan = new TXChallanEntry();
                        TXChallan.FinanceYearId = data.financialYear;
                        TXChallan.EmployeeId = f;
                        TXChallan.bankId = data.bankName;
                        TXChallan.ApplyDate = new DateTime(year, data.month, 1);
                        TXChallan.challanDate = data.challanDate;
                        TXChallan.challanNo = data.challanNo;
                        TXChallan.checkdd = data.checkdd;
                        TXChallan.bookEntry = data.bookEntry;
                        TXChallan.BSRCode = data.BSRCode;
                        TXChallan.TaxAmount = Convert.ToDecimal(paytax.Value);
                        isSave = TXChallan.Save();

                        if (isSave)
                        {

                        }
                        else
                        {
                            error++;
                        }


                    }
                }

            });

            if (error == 0)
            {
                return base.BuildJson(true, 200, "Data saved successfully", null);
            }
            else if (error > 0)
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", null);
            }
            return base.BuildJson(true, 200, "Data saved successfully", null);
        }

        public JsonResult UpdateChallanEntry(jsonChallanEntry data)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);          
            TXFinanceYear fnyear = new TXFinanceYear(data.financialYear, companyId);
            DateTime startDate = fnyear.StartingDate;
            DateTime endDate = fnyear.EndingDate;
            int year;
            if (endDate.Month >= data.month)
            {
                year = fnyear.EndingDate.Year;
            }
            else
            {
                year = fnyear.StartingDate.Year;
            }
            TXChallanEntry TXChallan = new TXChallanEntry();
            TXChallan.Id = data.Id;
            TXChallan.bankId = data.bankName;
            TXChallan.ApplyDate = new DateTime(year, data.month, 1);
            TXChallan.challanDate = data.challanDate;
            TXChallan.challanNo = data.challanNo;
            TXChallan.checkdd = data.checkdd;
            TXChallan.bookEntry = data.bookEntry;
            TXChallan.BSRCode = data.BSRCode;
            bool isSave = TXChallan.Save("Update");

            if (isSave)
            {

            }
            return base.BuildJson(true, 200, "Data saved successfully", null);
        }
        public JsonResult DeleteEmployeeChallanEntry(TXChallanEntry data)
        {
            TXChallanEntry TXChallan = new TXChallanEntry();
            TXChallan.Id = data.Id;
            TXChallan.EmployeeId = data.EmployeeId;
            TXChallan.FinanceYearId = data.FinanceYearId;
            bool Status = TXChallan.Delete("Single");
            return base.BuildJson(Status, 200, Status == true ? "success" : "Failure", null);
        }
        public JsonResult GetActualRentpaid(Guid FinanceYearId, Guid employeeId, DateTime EffectiveDate, Guid TxSecid)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TXEmployeeSectionList txEmpSecList = new TXEmployeeSectionList(employeeId, EffectiveDate);
            TXEmployeeSection txEmpSec = txEmpSecList.Where(a => a.SectionId == TxSecid).FirstOrDefault();
            if (!ReferenceEquals(txEmpSec, null))
            {
                TxActualRentPaidList ActualRentpaidList = new TxActualRentPaidList(FinanceYearId, employeeId, txEmpSec.Id);

                return base.BuildJson(true, 200, "success", ActualRentpaidList);
            }
            else
            {
                return base.BuildJson(true, 200, "success", new TxActualRentPaidList());
            }

        }

        public JsonResult GetPayhistory(Guid FinanceYearId, Guid employeeId, DateTime StartDate, DateTime EndDate)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int smonth = StartDate.Month;
            int syear = StartDate.Year;
            int emonth = EndDate.Month;
            int eyear = EndDate.Year;
            PayrollHistoryList Payrollhistorylistall = new PayrollHistoryList(companyId, syear, smonth, eyear, emonth, employeeId);
            if (!ReferenceEquals(Payrollhistorylistall, null))
            {
                return base.BuildJson(true, 200, "success", Payrollhistorylistall);
            }
            else
            {
                return base.BuildJson(true, 200, "success", new PayrollHistoryList());
            }

        }

        public JsonResult GetEmpPayhistory(int smonth,int syear,int nmonth,int nyear,Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PayrollHistoryList Payrollhistorylistall1 = new PayrollHistoryList(companyId,syear,smonth,nyear,nmonth, employeeId);
            var Payrollhistorylistall = Payrollhistorylistall1.Where(ph => ph.Status == "Processed" && ph.IsDeleted == false).ToList();
            if (!ReferenceEquals(Payrollhistorylistall, null))
            {
                return base.BuildJson(true, 200, "success", Payrollhistorylistall);
            }
            else
            {
                return base.BuildJson(true, 200, "success", new PayrollHistoryList());
            }

        }


        public JsonResult SaveActualRentpaid(List<TXActualRentPaid> dataValue, Guid TxSectionId, Guid employeeId, DateTime EffectiveDate)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            TXEmployeeSectionList txEmpSecList = new TXEmployeeSectionList(employeeId, EffectiveDate);
            TXEmployeeSection txEmpSec = txEmpSecList.Where(a => a.SectionId == TxSectionId).FirstOrDefault();
            dataValue.ForEach(data =>
            {
                TXActualRentPaid txDec = data;

                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                txDec.TXEmployeeSectionId = txEmpSec.Id;
                txDec.CreatedBy = userId;
                txDec.ModifiedBy = txDec.CreatedBy;
                txDec.IsDeleted = false;
                isSaved = txDec.Save();

            });

            if (isSaved)
            {
                string sourceFile = Server.MapPath(ConfigurationManager.AppSettings["StaticDeclaration"].ToString());
                return base.BuildJson(true, 200, "Data saved successfully", new { filePath = sourceFile, dataValue = dataValue });
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult SaveTaxDeclaration(List<jsonTaxDeclaration> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            Employee emp = new Employee(dataValue[0].employeeId);
            DateTime dt = Convert.ToDateTime(dataValue[0].effectiveDate, new CultureInfo("en-GB"));

            if (dt >= Convert.ToDateTime("1/" + emp.DateOfJoining.Month + "/" + emp.DateOfJoining.Year, new CultureInfo("en-GB")))
            {
                dataValue.ForEach(data =>
                {
                    TXEmployeeSection txDec = jsonTaxDeclaration.convertObject(data);

                    int companyId = Convert.ToInt32(Session["CompanyId"]);
                    int userId = Convert.ToInt32(Session["UserId"]);

                    txDec.CreatedBy = userId;
                    txDec.ModifiedBy = txDec.CreatedBy;
                    txDec.IsDeleted = false;
                    isSaved = txDec.Save();

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
            else
            {
                return base.BuildJson(false, 100, "Declaration entry month and year greater than date of joining.", dataValue);
            }
        }
        public JsonResult SubmitIFHP(Guid EmployeeId, Guid FinancialYear, Guid TxSectionId, string EffectiveMonth, string EffectiveYear, DateTime EffectiveDate)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            HousePropertyList houseproperty = new HousePropertyList(EmployeeId, FinancialYear, TxSectionId, EffectiveMonth, EffectiveYear, EffectiveDate);
            List<jsonHouseProperty> jsondata = new List<jsonHouseProperty>();
            houseproperty.ForEach(u => { jsondata.Add(jsonHouseProperty.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult DeleteTaxDeclaration(Guid id, Guid employeeid, Guid sectionId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXEmployeeSection txEmpSection = new TXEmployeeSection();
            txEmpSection.Id = id;
            txEmpSection.SectionId = sectionId;
            txEmpSection.EmployeeId = employeeid;

            txEmpSection.ModifiedBy = userId;
            txEmpSection.Delete();
            return base.BuildJson(true, 200, "success", null);

        }

        public JsonResult SaveLifeInsurance(jsonLifeInsurance dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.companyId = companyId;
            LifeInsurance lifeInsurance = jsonLifeInsurance.convertObject(dataValue);
            lifeInsurance.CreatedBy = userId;
            lifeInsurance.ModifiedBy = lifeInsurance.CreatedBy;
            lifeInsurance.IsDeleted = false;
            isSaved = lifeInsurance.Save();
            dataValue.id = lifeInsurance.Id;
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult SaveLifePremiumInsurance(jsonLifeInsurance dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.companyId = companyId;
            LifeInsurance lifeInsurance = jsonLifeInsurance.convertObject(dataValue);
            lifeInsurance.CreatedBy = userId;
            lifeInsurance.ModifiedBy = lifeInsurance.CreatedBy;
            lifeInsurance.IsDeleted = false;
            isSaved = lifeInsurance.SaveLICPremium();
            LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(companyId, dataValue.financialYearId, dataValue.employeeId, dataValue.month, dataValue.year);
            List<jsonLifeInsurance> jsondata = new List<jsonLifeInsurance>();
            lifeInsuranceList.ForEach(u =>
            {
                if (u.Id == dataValue.LifeInsuranceId)
                {
                    jsondata.Add(jsonLifeInsurance.toJson(u));
                }
            });
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveMedicalInsurance(jsonMedicalInsurance dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            MedicalInsurance MedicalInsurance = jsonMedicalInsurance.convertObject(dataValue);
            MedicalInsurance.CompanyId = companyId;
            MedicalInsurance.CreatedOn = DateTime.Today;
            MedicalInsurance.ModifiedOn = DateTime.Today;
            MedicalInsurance.CreatedBy = userId;
            MedicalInsurance.ModiifiedBy = MedicalInsurance.CreatedBy;
            MedicalInsurance.IsDelete = false;
            isSaved = MedicalInsurance.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult DeleteMedicalInsurance(jsonMedicalInsurance datavalue)
        {
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            MedicalInsurance medicalInsurance = new MedicalInsurance();
            medicalInsurance.Id = datavalue.Id;
            medicalInsurance.CreatedBy = userId;
            medicalInsurance.ModiifiedBy = medicalInsurance.CreatedBy;
            isDeleted = medicalInsurance.delete();
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", datavalue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", datavalue);
            }
        }
        public JsonResult DeleteLifeInsurance(jsonLifeInsurance dataValue)
        {
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            LifeInsurance lifeInsurance = new LifeInsurance();
            lifeInsurance.Id = dataValue.id;
            lifeInsurance.CreatedBy = userId;
            lifeInsurance.ModifiedBy = lifeInsurance.CreatedBy;
            isDeleted = lifeInsurance.Delete();
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", dataValue);
            }
        }

        public JsonResult GetLifeinsurancelist(Guid financialYearId, int year)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(companyId, financialYearId);
            List<jsonLifeInsurance> jsondata = new List<jsonLifeInsurance>();
            List<string> PolicynoList = new List<string>();
            PolicynoList = lifeInsuranceList.Select(d => d.PolicyNumber).ToList();
            lifeInsuranceList.ForEach(u =>
            {
                if (u.Year == year)
                { jsondata.Add(jsonLifeInsurance.toJson(u)); }
            });
            return base.BuildJson(true, 200, "success", new
            {
                InsuranceList = jsondata,
                Policylist = PolicynoList
            }
            );
        }

        public JsonResult GetLifeInsurance(Guid employeeId, Guid financialYearId, int month, int year, Guid sectionId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(companyId, financialYearId, employeeId, month, year);
            List<jsonLifeInsurance> jsondata = new List<jsonLifeInsurance>();
            DateTime effectiveDate = Convert.ToDateTime(year + "-" + month + "-" + "01 00:00:00.000 ");
            TXEmployeeSectionList TXEmployeeSectionList = new TXEmployeeSectionList(employeeId, effectiveDate, true);
            decimal totaldeduction = 0;
            decimal TotalDeclarevalue = 0;
            lifeInsuranceList.ForEach(p =>
            {
                if (p.PolicyDate == null || p.Relationship == "" || p.InsuredPersonName == "" || p.SumAssured == 0)
                {
                    totaldeduction = 0;
                }
                else
                {
                    if (p.annualPremium > 0 && p.SumAssured > 0)
                    {
                        if (p.PolicyDate >= Convert.ToDateTime("01/04/2013", new System.Globalization.CultureInfo("en-GB")))
                        {
                            if (p.IsDisabilityPerson == 0 || p.IsPersonTakingTreatement == 0)
                            {
                                totaldeduction = p.SumAssured / 100 * 15;
                            }
                            else
                            {
                                totaldeduction = p.SumAssured / 100 * 10;
                            }
                        }
                        else if (p.PolicyDate >= Convert.ToDateTime("01/04/2012", new System.Globalization.CultureInfo("en-GB")))
                        {
                            totaldeduction = p.SumAssured / 100 * 10;
                        }
                        else if (p.PolicyDate <= Convert.ToDateTime("01/04/2012", new System.Globalization.CultureInfo("en-GB")))
                        {
                            totaldeduction = p.SumAssured / 100 * 20;
                        }
                        else
                        {
                            totaldeduction = 0;
                        }
                    }
                    p.Premiumdeduction = Math.Min(p.annualPremium, totaldeduction);
                    TotalDeclarevalue = TotalDeclarevalue + p.Premiumdeduction;
                }
            });

            TXEmployeeSection txempsection = new TXEmployeeSection();
            txempsection.CreatedBy = userId;
            txempsection.ModifiedBy = txempsection.CreatedBy;
            txempsection.IsDeleted = false;
            txempsection.Id = Guid.Empty;
            txempsection.EmployeeId = employeeId;
            txempsection.SectionId = sectionId;
            txempsection.EffectiveDate = effectiveDate;
            txempsection.Proof = true;
            txempsection.DeclaredValue = Convert.ToString(TotalDeclarevalue);
            bool issaved = txempsection.Save();

            lifeInsuranceList.ForEach(u =>
            {
                u.totaldeclarevalue = TotalDeclarevalue;
                jsondata.Add(jsonLifeInsurance.toJson(u));
            });


            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetLifeInsurancePolicy(Guid employeeId, Guid financialYearId, Guid TXLICid)
        {
            List<jsonLifeInsurance> jsondata = new List<jsonLifeInsurance>();
            LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(financialYearId, employeeId, TXLICid);
            lifeInsuranceList.ForEach(u =>
            {
                jsondata.Add(jsonLifeInsurance.toJson(u));
            });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult DeleteLifeInsurancepolicy(jsonLifeInsurance dataValue, int month, int year)
        {
            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            LifeInsurance lifeInsurance = new LifeInsurance();
            lifeInsurance.LifeInsuranceId = dataValue.LifeInsuranceId;
            lifeInsurance.PolicyNumber = dataValue.policyNumber;
            lifeInsurance.CreatedBy = userId;
            lifeInsurance.ModifiedBy = lifeInsurance.CreatedBy;
            lifeInsurance.Id = dataValue.id;
            isDeleted = lifeInsurance.Deletepremiumpolicy();
            LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(companyId, dataValue.financialYearId, dataValue.employeeId, month, year);
            List<jsonLifeInsurance> jsondata = new List<jsonLifeInsurance>();
            lifeInsuranceList.ForEach(u =>
            {
                if (u.Id == dataValue.LifeInsuranceId)
                {
                    jsondata.Add(jsonLifeInsurance.toJson(u));
                }
            });
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", dataValue);
            }
        }


        public JsonResult checkselfproperty(jsonHouseProperty datavalue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsActive).FirstOrDefault();
            bool limitcrossed = false;
            HousePropertyList hsProplist = new HousePropertyList(datavalue.EmployeeId, companyId, defaultyear.Id);

            var hpl = hsProplist.Where(w => w.EffectiveMonth == datavalue.EffectiveMonth && w.EffectiveYear == datavalue.EffectiveYear).ToList();

            if (hpl.Count > 0)
            {
                if (defaultyear.StartingDate>=new DateTime(2019,4,1) && defaultyear.EndingDate <= new DateTime(2020,3,31))
                {

                    hpl.RemoveAll(r => r.PropertyId == datavalue.PropertyId);
                    var selfproperty = hpl.Where(w => w.PropertySelfOccupied == "1").ToList();
                        if (selfproperty.Count >= 2 && datavalue.PropertySelfOccupied=="1")
                        {
                            limitcrossed = true;
                        }
                    

                }
            }

            if (limitcrossed)
            {
                return base.BuildJson(false, 200, "Only 2 properties are allowed to be claimed as Self Occupied Properties", datavalue);
            }
            else
            {
                return base.BuildJson(true, 100, "There is some error while saving the data.", datavalue);
            }
        }
        public JsonResult SaveHouseProperty(jsonHouseProperty datavalue, string Type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            HouseProperty houseProperty = jsonHouseProperty.convertObject(datavalue);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsActive).FirstOrDefault();
            HousePropertyList hsProplist = new HousePropertyList(datavalue.EmployeeId, companyId, defaultyear.Id);
            houseProperty.CompanyId = companyId;
            houseProperty.FinancialYear = defaultyear.Id;
            houseProperty.CreatedBy = userId;
            houseProperty.ModifiedBy = houseProperty.CreatedBy;
            houseProperty.IsDelete = false;
            Guid EmployeeId = houseProperty.EmployeeId;
            int PropertyId = houseProperty.PropertyId;
            Guid FinancialYear = houseProperty.FinancialYear;
            string EffectiveMonth = houseProperty.EffectiveMonth;
            string EffectiveYear = houseProperty.EffectiveYear;
            HouseProperty housepropertyID = new HouseProperty(EmployeeId, PropertyId, FinancialYear, companyId, EffectiveMonth, EffectiveYear);
            houseProperty.Id = housepropertyID.Id;
            isSaved = houseProperty.Save(Type);
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", datavalue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", datavalue);
            }
        }
        public JsonResult DeleteHRA(jsonHouseProperty datavalue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            HouseProperty houseProperty = jsonHouseProperty.convertObject(datavalue);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsActive).FirstOrDefault();
            houseProperty.FinancialYear = defaultyear.Id;
            houseProperty.ModifiedBy = userId;
            Guid EmployeeId = houseProperty.EmployeeId;
            int PropertyId = houseProperty.PropertyId;
            Guid FinancialYear = houseProperty.FinancialYear;
            string EffectiveMonth = houseProperty.EffectiveMonth;
            string EffectiveYear = houseProperty.EffectiveYear;
            HouseProperty housepropertyID = new HouseProperty(EmployeeId, PropertyId, FinancialYear, companyId, EffectiveMonth, EffectiveYear);
            houseProperty.Id = housepropertyID.Id;
            isSaved = houseProperty.Delete();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", datavalue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", datavalue);
            }
        }

        public JsonResult GetMedicalInsurance(Guid EmployeeId, Guid FinancialYear, int Month, int Year, Guid TXSection, string Fieldname)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int CompanyId = Convert.ToInt32(Session["companyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            MedicalInsuranceList medicalinsuranceList = new MedicalInsuranceList(EmployeeId, CompanyId, Month, Year, Fieldname);
            List<jsonMedicalInsurance> jsondata = new List<jsonMedicalInsurance>();
            medicalinsuranceList.ForEach(u => jsondata.Add(jsonMedicalInsurance.toJson(u)));
            return base.BuildJson(true, 200, "success", jsondata);
        }

        //public JsonResult GetHouseProperty(jsonHouseProperty dataValue)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);
        //    bool isSaved = false;
        //    HouseProperty houseProperty = jsonHouseProperty.convertObject(dataValue);
        //    int companyId = Convert.ToInt32(Session["CompanyId"]);
        //    int userId = Convert.ToInt32(Session["UserId"]);
        //    isSaved = houseProperty.getdata();
        //    if (isSaved)
        //    {
        //        return base.BuildJson(true, 200, "Data saved successfully", dataValue);
        //    }
        //    else
        //    {
        //        return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
        //    }
        //}
        public JsonResult GetHouseProperty(Guid EmployeeId, int PropertyId, Guid TxSectionId, Guid FinancialYear, string EffectiveMonth, string EffectiveYear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = true;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            HousePropertyList housePropertyList = new HousePropertyList(EmployeeId, companyId, PropertyId, TxSectionId, FinancialYear, EffectiveMonth, EffectiveYear);
            List<jsonHouseProperty> jsondata = new List<jsonHouseProperty>();
            housePropertyList.ForEach(u => { jsondata.Add(jsonHouseProperty.toJson(u)); });
            // HouseProperty houseProperty = jsonHouseProperty.convertObject(dataValue);
            //List<jsonHouseProperty> jsondata = new List<jsonHouseProperty>();
            // HouseProperty houseproperty = new HouseProperty(employeeId, companyId);
            if (isSaved)
            {
                return base.BuildJson(true, 200, "success", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", jsondata);
            }

        }

        /// <summary>
        /// created by suriya 
        /// HRA calculation
        /// </summary>
        /// <param name="datavalue"></param>
        /// <returns></returns>
        public JsonResult Totalinterestcalculation(jsonHouseProperty datavalue)
        {
            Decimal PropertyJointInterest = Convert.ToDecimal(datavalue.PropertyJointInterest);
            int jointpropertydname = datavalue.PropertyJointName;
            Decimal PayableHousingLoanPerYear = datavalue.PayableHousingLoanPerYear;
            Decimal PreConstructionInterest = datavalue.PreConstructionInterest;
            Decimal GrossRentalIncome_PA = datavalue.GrossRentalIncome_PA;
            Decimal Municipal_Water_Sewerage_taxpaid = datavalue.Municipal_Water_Sewerage_taxpaid;
            int PropertySelfOccupied = Convert.ToInt32(datavalue.PropertySelfOccupied);
            int ConstrutionIsCompleted = datavalue.ConstrutionIsCompleted;
            int HousingLoanTakenBefore_01_04_1999 = datavalue.HousingLoanTakenBefore_01_04_1999;
            int PurposeOfLoan = Convert.ToInt32(datavalue.PurposeOfLoan);
            HouseProperty houseProperty = jsonHouseProperty.convertObject(datavalue);
            if (jointpropertydname == 1)
            {
                Decimal TotalInterestOfYear = 0;
                Decimal Interest_RestrictedtoEmployee = 0;
                Decimal totalvalue = 0;

                totalvalue = PayableHousingLoanPerYear + PreConstructionInterest;
                TotalInterestOfYear = totalvalue;
                Interest_RestrictedtoEmployee = totalvalue * PropertyJointInterest / 100;
                houseProperty.TotalInterestOfYear = TotalInterestOfYear;
                houseProperty.Interest_RestrictedtoEmployee = Interest_RestrictedtoEmployee;

                if (PropertySelfOccupied == 2)
                {
                    if (jointpropertydname == 1)
                    {
                        houseProperty.GrossRentalIncome = GrossRentalIncome_PA * PropertyJointInterest / 100;
                        houseProperty.LessMunicipalTaxes = Municipal_Water_Sewerage_taxpaid * PropertyJointInterest / 100;
                        houseProperty.Balance = houseProperty.GrossRentalIncome - houseProperty.LessMunicipalTaxes;
                        houseProperty.LessStandardDeduction = Math.Round(houseProperty.Balance * 30 / 100);
                        if (PayableHousingLoanPerYear > 0)
                        {
                            if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                            else if (PropertySelfOccupied == 2)
                            {
                                if (GrossRentalIncome_PA > 0)
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = 0;
                                }
                            }
                            else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                            {
                                if (Interest_RestrictedtoEmployee > 30000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 30000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                            else
                            {
                                if (Interest_RestrictedtoEmployee > 200000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 200000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                        }
                        else
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                    }
                    else
                    {
                        houseProperty.GrossRentalIncome = GrossRentalIncome_PA;
                        houseProperty.LessMunicipalTaxes = Municipal_Water_Sewerage_taxpaid;
                        houseProperty.Balance = houseProperty.GrossRentalIncome - houseProperty.LessMunicipalTaxes;
                        houseProperty.LessStandardDeduction = houseProperty.Balance * 30 / 100;
                        if (PayableHousingLoanPerYear > 0)
                        {
                            if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                            else if (PropertySelfOccupied == 2)
                            {
                                if (GrossRentalIncome_PA > 0)
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = 0;
                                }
                            }
                            else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                            {
                                if (Interest_RestrictedtoEmployee > 30000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 30000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                            else
                            {
                                if (Interest_RestrictedtoEmployee > 200000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 200000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                        }
                        else
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                    }
                }
                else
                {
                    houseProperty.Balance = 0;
                    houseProperty.LessMunicipalTaxes = 0;
                    houseProperty.GrossRentalIncome = 0;
                    houseProperty.LessStandardDeduction = 0;
                    if (PayableHousingLoanPerYear > 0)
                    {
                        if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                        else if (PropertySelfOccupied == 2)
                        {
                            if (GrossRentalIncome_PA > 0)
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                        }
                        else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                        {
                            if (Interest_RestrictedtoEmployee > 30000)
                            {
                                houseProperty.LessInterestOnHousingLoan = 30000;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                        }
                        else
                        {
                            if (Interest_RestrictedtoEmployee > 200000)
                            {
                                houseProperty.LessInterestOnHousingLoan = 200000;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                        }
                    }
                    else
                    {
                        houseProperty.LessInterestOnHousingLoan = 0;
                    }
                }

                houseProperty.HousePropertyNetIncome = houseProperty.Balance - houseProperty.LessStandardDeduction - houseProperty.LessInterestOnHousingLoan;

                return base.BuildJson(true, 200, "success", houseProperty);
            }
            else
            {

                Decimal TotalInterestOfYear = 0;
                Decimal Interest_RestrictedtoEmployee = 0;
                Decimal totalvalue = 0;

                totalvalue = PayableHousingLoanPerYear + PreConstructionInterest;
                TotalInterestOfYear = totalvalue;
                Interest_RestrictedtoEmployee = totalvalue;
                houseProperty.TotalInterestOfYear = TotalInterestOfYear;
                houseProperty.Interest_RestrictedtoEmployee = Interest_RestrictedtoEmployee;

                if (PropertySelfOccupied == 2)
                {
                    if (jointpropertydname == 1)
                    {
                        houseProperty.GrossRentalIncome = GrossRentalIncome_PA * PropertyJointInterest / 100;
                        houseProperty.LessMunicipalTaxes = Municipal_Water_Sewerage_taxpaid * PropertyJointInterest / 100;
                        houseProperty.Balance = houseProperty.GrossRentalIncome - houseProperty.LessMunicipalTaxes;
                        houseProperty.LessStandardDeduction = Math.Round(houseProperty.Balance * 30 / 100);
                        if (PayableHousingLoanPerYear > 0)
                        {
                            if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                            else if (PropertySelfOccupied == 2)
                            {
                                if (GrossRentalIncome_PA > 0)
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = 0;
                                }
                            }
                            else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                            {
                                if (Interest_RestrictedtoEmployee > 30000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 30000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                            else
                            {
                                if (Interest_RestrictedtoEmployee > 200000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 200000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                        }
                        else
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                    }
                    else
                    {
                        houseProperty.GrossRentalIncome = GrossRentalIncome_PA;
                        houseProperty.LessMunicipalTaxes = Municipal_Water_Sewerage_taxpaid;
                        houseProperty.Balance = houseProperty.GrossRentalIncome - houseProperty.LessMunicipalTaxes;
                        houseProperty.LessStandardDeduction = houseProperty.Balance * 30 / 100;
                        if (PayableHousingLoanPerYear > 0)
                        {
                            if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                            else if (PropertySelfOccupied == 2)
                            {
                                if (GrossRentalIncome_PA > 0)
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = 0;
                                }
                            }
                            else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                            {
                                if (Interest_RestrictedtoEmployee > 30000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 30000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                            else
                            {
                                if (Interest_RestrictedtoEmployee > 200000)
                                {
                                    houseProperty.LessInterestOnHousingLoan = 200000;
                                }
                                else
                                {
                                    houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                                }
                            }
                        }
                        else
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                    }
                }
                else
                {
                    houseProperty.Balance = 0;
                    houseProperty.LessMunicipalTaxes = 0;
                    houseProperty.GrossRentalIncome = 0;
                    houseProperty.LessStandardDeduction = 0;
                    if (PayableHousingLoanPerYear > 0)
                    {
                        if (PurposeOfLoan == 2 && ConstrutionIsCompleted == 2)
                        {
                            houseProperty.LessInterestOnHousingLoan = 0;
                        }
                        else if (PropertySelfOccupied == 2)
                        {
                            if (GrossRentalIncome_PA > 0)
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = 0;
                            }
                        }
                        else if (HousingLoanTakenBefore_01_04_1999 == 1 || PurposeOfLoan == 3)
                        {
                            if (Interest_RestrictedtoEmployee > 30000)
                            {
                                houseProperty.LessInterestOnHousingLoan = 30000;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                        }
                        else
                        {
                            if (Interest_RestrictedtoEmployee > 200000)
                            {
                                houseProperty.LessInterestOnHousingLoan = 200000;
                            }
                            else
                            {
                                houseProperty.LessInterestOnHousingLoan = houseProperty.Interest_RestrictedtoEmployee;
                            }
                        }
                    }
                    else
                    {
                        houseProperty.LessInterestOnHousingLoan = 0;
                    }
                }

/* this condition manipulated in tax section activity due to the reason that housing lona interest should not be considered for
   self occupied property under new tax scheme */
                houseProperty.HousePropertyNetIncome = (houseProperty.Balance - houseProperty.LessStandardDeduction - houseProperty.LessInterestOnHousingLoan);

                return base.BuildJson(true, 200, "success", houseProperty);
            }
        }


        public JsonResult MedicalPremiumcal(jsonMedicalInsurance datavalue)
        {
            Decimal AmountofpremiumorExpense = datavalue.AmountofpremiumorExpense;
            int InsuranceType = Convert.ToInt32(datavalue.InsuranceType);
            int RelationshipoftheInsuredperson = Convert.ToInt32(datavalue.RelationshipoftheInsuredperson);
            int CoveredinthepolicyisSeniorCitizen = Convert.ToInt32(datavalue.CoveredinthepolicyisSeniorCitizen);
            int IncurredinrespectofVerySeniorCitizen = Convert.ToInt32(datavalue.IncurredinrespectofVerySeniorCitizen);
            int PayMode = Convert.ToInt32(datavalue.PayMode);
            MedicalInsurance medicalinsurance = jsonMedicalInsurance.convertObject(datavalue);
            if (InsuranceType == 3)
            {
                if (AmountofpremiumorExpense < 5000)
                {
                    medicalinsurance.EligibleDeductionforthepolicy = AmountofpremiumorExpense;
                }
                else
                {
                    medicalinsurance.EligibleDeductionforthepolicy = 5000;
                }
                return base.BuildJson(true, 200, "success", medicalinsurance);
            }
            else
            {
                if (PayMode == 1)
                {
                    medicalinsurance.EligibleDeductionforthepolicy = 0;
                }
                else
                {
                    if (InsuranceType == 4)
                    {
                        if (CoveredinthepolicyisSeniorCitizen == 1)
                        {
                            if (IncurredinrespectofVerySeniorCitizen == 1)
                            {
                                if (AmountofpremiumorExpense < 50000)
                                {
                                    medicalinsurance.EligibleDeductionforthepolicy = AmountofpremiumorExpense;
                                }
                                else
                                {
                                    medicalinsurance.EligibleDeductionforthepolicy = 50000;
                                }
                            }
                            else
                            {
                                medicalinsurance.EligibleDeductionforthepolicy = 0;
                            }
                        }
                        else
                        {
                            medicalinsurance.EligibleDeductionforthepolicy = 0;
                        }
                    }
                    else
                    {
                        if (CoveredinthepolicyisSeniorCitizen == 1)
                        {
                            if (AmountofpremiumorExpense < 50000)
                            {
                                medicalinsurance.EligibleDeductionforthepolicy = AmountofpremiumorExpense;
                            }
                            else
                            {
                                medicalinsurance.EligibleDeductionforthepolicy = 50000;
                            }
                        }
                        else
                        {
                            if (AmountofpremiumorExpense < 25000)
                            {
                                medicalinsurance.EligibleDeductionforthepolicy = AmountofpremiumorExpense;
                            }
                            else
                            {
                                medicalinsurance.EligibleDeductionforthepolicy = 25000;
                            }
                        }

                    }
                }
                return base.BuildJson(true, 200, "success", medicalinsurance);
            }

        }
        public JsonResult medicalInsuranceCalculation(jsonMedicalInsurance datavalue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int CompanyId = Convert.ToInt32(Session["companyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            MedicalInsuranceList medicalinsuranceList = new MedicalInsuranceList(datavalue.EmployeeId, CompanyId, datavalue.EffectiveMonth, datavalue.EffectiveYear, datavalue.FinYearId);
            List<jsonMedicalInsurance> jsondata = new List<jsonMedicalInsurance>();
            medicalinsuranceList.ForEach(u => jsondata.Add(jsonMedicalInsurance.toJson(u)));
            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult SubmitMedicalinsurance(jsonMedicalInsurance datavalue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int CompanyId = Convert.ToInt32(Session["companyId"]);
            MedicalInsurance medicalcalculation = new MedicalInsurance(datavalue.EmployeeId, CompanyId, datavalue.EffectiveMonth, datavalue.EffectiveYear, datavalue.FinYearId);
            MedicalInsuranceList MedicalInsurance = new MedicalInsuranceList(datavalue.EmployeeId, datavalue.TxSectionId, datavalue.FinYearId, datavalue.EffectiveMonth, datavalue.EffectiveYear, medicalcalculation.TotalDeduction);
            List<jsonMedicalInsurance> jsondata = new List<jsonMedicalInsurance>();
            MedicalInsurance.ForEach(u => { jsondata.Add(jsonMedicalInsurance.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }
    }

    public class jsonTaxDeclaration
    {

        public Guid id { get; set; }


        public Guid sectionId { get; set; }


        public Guid employeeId { get; set; }


        public string value { get; set; }

        public string effectiveDate { get; set; }

        public bool Proof { get; set; }

        public bool? hasPan { get; set; }

        public string panNumber { get; set; }

        public bool? hasdeclaration { get; set; }

        public string landLordName { get; set; }

        public string landlordAddress { get; set; }

        public string TotalRent { get; set; }
        public static jsonTaxDeclaration toJson(TXEmployeeSection txDec)
        {

            return new jsonTaxDeclaration()
            {
                id = txDec.Id,
                sectionId = txDec.SectionId,
                effectiveDate = txDec.EffectiveDate.ToString("dd/MMM/yyyy"),
                employeeId = txDec.EmployeeId,
                value = txDec.DeclaredValue,
                Proof = txDec.Proof,
                hasPan = txDec.HasPan,
                panNumber = txDec.PanNumber,
                hasdeclaration = txDec.HasDeclaration,
                landLordName = txDec.LandLordName,
                landlordAddress = txDec.LandLordAddress,
                TotalRent = txDec.TotalRent.ToString()

            };
        }
    

            public static TXEmployeeSection convertObject(jsonTaxDeclaration txDec)
        {
            DateTime EffectiveDate;
            if (txDec.effectiveDate == " ")
            {
                EffectiveDate = DateTime.MinValue;
            }
            else
            {
                EffectiveDate = Convert.ToDateTime(txDec.effectiveDate, new CultureInfo("en-GB"));
            }
            return new TXEmployeeSection()
            {
                Id = txDec.id,
                SectionId = txDec.sectionId,
                EffectiveDate = EffectiveDate,
                EmployeeId = txDec.employeeId,
                DeclaredValue = txDec.value,
                Proof = txDec.Proof,
                HasPan = txDec.hasPan,
                PanNumber = txDec.panNumber,
                HasDeclaration = txDec.hasdeclaration,
                LandLordName = txDec.landLordName,
                LandLordAddress = txDec.landlordAddress,
                TotalRent = Convert.ToDecimal(txDec.TotalRent)
            };
        }
    }

    public class jsonProjIncome
    {

        public Guid financeyear { get; set; }

        public Guid employeeId { get; set; }

        public int MonthCol { get; set; }
        public int YearCol { get; set; }

        public double ProjIncome1 { get; set; }

        public double ProjIncome2 { get; set; }

        public double ProjIncome3 { get; set; }
        public static jsonProjIncome toJson(TXProjIncome txproj)
        {

            return new jsonProjIncome()
            {
                financeyear = txproj.financeyear,
                employeeId = txproj.EmployeeId,
                MonthCol = txproj.Month,
                YearCol = txproj.Year,
                ProjIncome1 = Convert.ToDouble(txproj.Income1),
                ProjIncome2 = Convert.ToDouble(txproj.Income2),
                ProjIncome3 = Convert.ToDouble(txproj.Income3)
            };
        }
    }


    public class Jsonpayhistory
    {
        public static PayrollHistory Tojson(PayrollHistory payhistorylist)
        {
            return new PayrollHistory()
            {
                Id = payhistorylist.Id
            };
        }
    }

    public class jsonLifeInsurance
    {



        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid employeeId { get; set; }

        public string employeeCode { get; set; }

        public string employeeName { get; set; }

        /// <summary>
        /// Get or Set the FinancialYearId
        /// </summary>
        public Guid financialYearId { get; set; }

        /// <summary>
        /// Get or Set the LifeInsuranceId
        /// </summary>
        public Guid LifeInsuranceId { get; set; }


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int companyId { get; set; }

        /// <summary>
        /// Get or Set the PolicyNumber
        /// </summary>
        public string policyNumber { get; set; }

        /// <summary>
        /// Get or Set the PolicyDate
        /// </summary>
        public string policyDate { get; set; }

        /// <summary>
        /// Get or Set the InsuredPersonName
        /// </summary>
        public string insuredPersonName { get; set; }

        /// <summary>
        /// Get or Set the IsDisabilityPerson
        /// </summary>
        public int isDisabilityPerson { get; set; }

        /// <summary>
        /// Get or Set the IsPersonTakingTreatement
        /// </summary>
        public int isPersonTakingTreatement { get; set; }

        /// <summary>
        /// Get or Set the PremiumAmount
        /// </summary>
        public decimal premiumAmount { get; set; }

        ///<summary>
        /// Get or set the PremiumDate 
        ///</summary>
        public string PremiumDate { get; set; }
        /// <summary>
        /// Get or Set the PremiumAmountFallingDueInFeb
        /// </summary>
        public decimal premiumAmountFallingDueInFeb { get; set; }

        /// <summary>
        /// Get or Set the PremiumConsideredForDeduction
        /// </summary>
        public decimal premiumConsideredForDeduction { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime createdOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int createdBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime modifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int modifiedBy { get; set; }

        public string relationship { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool isDeleted { get; set; }
        public Guid sectionid { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public decimal Sumassured { get; set; }
        public decimal annualPremium { get; set; }
        public decimal Premiumdeduction { get; set; }
        public decimal TotalDeclarevalue { get; set; }

        public static jsonLifeInsurance toJson(LifeInsurance lifeInsurance)
        {

            return new jsonLifeInsurance()
            {
                id = lifeInsurance.Id,
                financialYearId = lifeInsurance.FinancialYearId,
                companyId = lifeInsurance.CompanyId,
                employeeId = lifeInsurance.EmployeeId,
                insuredPersonName = lifeInsurance.InsuredPersonName,
                isDisabilityPerson = lifeInsurance.IsDisabilityPerson,
                isPersonTakingTreatement = lifeInsurance.IsPersonTakingTreatement,
                relationship = lifeInsurance.Relationship,
                policyNumber = lifeInsurance.PolicyNumber,
                Sumassured = lifeInsurance.SumAssured,
                premiumAmount = lifeInsurance.PremiumAmount,
                premiumAmountFallingDueInFeb = lifeInsurance.PremiumAmountFallingDueInFeb,
                premiumConsideredForDeduction = lifeInsurance.PremiumConsideredForDeduction,
                policyDate = lifeInsurance.PolicyDate.ToString("dd/MMM/yyyy"),
                PremiumDate = lifeInsurance.PremiumDate.ToString("dd/MMM/yyyy"),
                sectionid = lifeInsurance.SectionId,
                month = lifeInsurance.Month,
                year = lifeInsurance.Year,
                annualPremium = lifeInsurance.annualPremium,
                Premiumdeduction = lifeInsurance.Premiumdeduction,
                TotalDeclarevalue = lifeInsurance.totaldeclarevalue,
                LifeInsuranceId = lifeInsurance.LifeInsuranceId

            };
        }
        public static LifeInsurance convertObject(jsonLifeInsurance txDec)
        {
            DateTime policyDate;
            if (txDec.policyDate == " ")
            {
                policyDate = DateTime.MinValue;
            }
            else
            {
                policyDate = Convert.ToDateTime(txDec.policyDate);
            }
            return new LifeInsurance()
            {
                Id = txDec.id,
                FinancialYearId = txDec.financialYearId,
                PolicyDate = policyDate,
                EmployeeId = txDec.employeeId,
                CompanyId = txDec.companyId,
                InsuredPersonName = txDec.insuredPersonName,
                IsDisabilityPerson = txDec.isDisabilityPerson,
                IsPersonTakingTreatement = txDec.isPersonTakingTreatement,
                PolicyNumber = txDec.policyNumber,
                SumAssured = txDec.Sumassured,
                PremiumAmount = txDec.premiumAmount,
                PremiumAmountFallingDueInFeb = txDec.premiumAmountFallingDueInFeb,
                PremiumConsideredForDeduction = txDec.premiumConsideredForDeduction,
                SectionId = txDec.sectionid,
                Month = txDec.month,
                Year = txDec.year,
                Relationship = txDec.relationship,
                annualPremium = txDec.annualPremium,
                Premiumdeduction = txDec.Premiumdeduction,
                totaldeclarevalue = txDec.TotalDeclarevalue,
                PremiumDate = Convert.ToDateTime(txDec.PremiumDate),
                LifeInsuranceId = txDec.LifeInsuranceId
            };
        }
    }

    public class jsonHouseProperty
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid TxSectionId { get; set; }
        public int NoOfProperties { get; set; }
        public int PropertyId { get; set; }
        public Guid FinancialYear { get; set; }
        public int CompanyId { get; set; }
        public int PropertyReference { get; set; }
        public string Property_OwnersName { get; set; }
        public string PropertyAddress { get; set; }
        public Decimal PropertyLoanAmount { get; set; }
        public string PurposeOfLoan { get; set; }
        public DateTime DateofSanctionofLoan { get; set; }
        public int PropertyJointName { get; set; }
        public string PropertyJointInterest { get; set; }
        public Decimal PayableHousingLoanPerYear { get; set; }
        public Decimal PreConstructionInterest { get; set; }
        public Decimal TotalInterestOfYear { get; set; }
        public Decimal Interest_RestrictedtoEmployee { get; set; }
        public int ConstrutionIsCompleted { get; set; }
        public string PropertySelfOccupied { get; set; }
        public int HousingLoanTakenBefore_01_04_1999 { get; set; }
        public Decimal GrossRentalIncome_PA { get; set; }
        public Decimal Municipal_Water_Sewerage_taxpaid { get; set; }
        public Decimal GrossRentalIncome { get; set; }
        public Decimal LessMunicipalTaxes { get; set; }
        public Decimal Balance { get; set; }
        public Decimal LessStandardDeduction { get; set; }
        public Decimal LessInterestOnHousingLoan { get; set; }
        public Decimal HousePropertyNetIncome { get; set; }
        public bool IsHRACompleted { get; set; }
        public string EffectiveMonth { get; set; }
        public string EffectiveYear { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }

        public string LenderAddress { get; set; }
        public string LenderName { get; set; }
        public string LenderHRAAddress { get; set; }
        public string LenderPAN { get; set; }
        public int LenderType { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string CompanyName { get; set; }
        public string PANNumber { get; set; }
        public string Financial_Year { get; set; }
        public string DateofSancLoan { get; set; }
        public string PropertyCount { get; set; }




        public static jsonHouseProperty toJson(HouseProperty houseProperty)
        {
            string LoadsantionDate = houseProperty.DateofSanctionofLoan.ToString("dd/MMM/yyyy");

            return new jsonHouseProperty()
            {
                Id = houseProperty.Id,
                FinancialYear = houseProperty.FinancialYear,
                TxSectionId = houseProperty.TxSectionId,
                CompanyId = houseProperty.CompanyId,
                EmployeeId = houseProperty.EmployeeId,
                PropertyId = houseProperty.PropertyId,
                LenderAddress = houseProperty.LenderAddress,
                LenderName = houseProperty.LenderName,
                LenderHRAAddress = houseProperty.LenderHRAAddress,
                LenderPAN = houseProperty.LenderPAN,
                LenderType = houseProperty.LenderType,
                DateofSancLoan = LoadsantionDate,
                PropertyReference = houseProperty.PropertyReference,
                PropertyJointName = houseProperty.PropertyJointName,
                PropertyAddress = houseProperty.PropertyAddress,
                HousePropertyNetIncome = houseProperty.HousePropertyNetIncome,
                PayableHousingLoanPerYear = houseProperty.PayableHousingLoanPerYear,
                PropertyJointInterest = houseProperty.PropertyJointInterest,
                ConstrutionIsCompleted = houseProperty.ConstrutionIsCompleted,
                //isDetailsRequired = houseProperty.IsDetailsRequired,
                HousingLoanTakenBefore_01_04_1999 = houseProperty.HousingLoanTakenBefore_01_04_1999,
                PropertySelfOccupied = houseProperty.PropertySelfOccupied,
                PropertyLoanAmount = houseProperty.PropertyLoanAmount,
                PurposeOfLoan = houseProperty.PurposeOfLoan,
                Interest_RestrictedtoEmployee = houseProperty.Interest_RestrictedtoEmployee,
                DateofSanctionofLoan = houseProperty.DateofSanctionofLoan,
                Municipal_Water_Sewerage_taxpaid = houseProperty.Municipal_Water_Sewerage_taxpaid,
                Property_OwnersName = houseProperty.Property_OwnersName,
                //netIncomeOrLoss = houseProperty.NetIncomeOrLoss,
                PreConstructionInterest = houseProperty.PreConstructionInterest,
                NoOfProperties = houseProperty.NoOfProperties,
                GrossRentalIncome_PA = houseProperty.GrossRentalIncome_PA,
                TotalInterestOfYear = houseProperty.TotalInterestOfYear,
                LessMunicipalTaxes = houseProperty.LessMunicipalTaxes,
                Balance = houseProperty.Balance,
                EffectiveMonth = houseProperty.EffectiveMonth,
                EffectiveYear = houseProperty.EffectiveYear,
                LessStandardDeduction = houseProperty.LessStandardDeduction,
                LessInterestOnHousingLoan = houseProperty.LessInterestOnHousingLoan,
                IsHRACompleted = houseProperty.IsHRACompleted,
                GrossRentalIncome = houseProperty.GrossRentalIncome,
                EmployeeCode = houseProperty.EmployeeCode,
                EmployeeName = houseProperty.EmployeeName,
                CompanyName = houseProperty.CompanyName,
                PANNumber = houseProperty.PANNumber,
                Financial_Year = houseProperty.Financial_Year,
                PropertyCount = houseProperty.PropertyCount

            };
        }
        public static HouseProperty convertObject(jsonHouseProperty houseProperty)
        {
            return new HouseProperty()
            {
                Id = houseProperty.Id,
                FinancialYear = houseProperty.FinancialYear,
                TxSectionId = houseProperty.TxSectionId,
                CompanyId = houseProperty.CompanyId,
                EmployeeId = houseProperty.EmployeeId,
                PropertyId = houseProperty.PropertyId,

                LenderAddress = houseProperty.LenderAddress,
                LenderName = houseProperty.LenderName,
                LenderHRAAddress = houseProperty.LenderHRAAddress,
                LenderPAN = houseProperty.LenderPAN,
                LenderType = houseProperty.LenderType,

                PropertyReference = houseProperty.PropertyReference,
                PropertyJointName = houseProperty.PropertyJointName,
                PropertyAddress = houseProperty.PropertyAddress,
                HousePropertyNetIncome = houseProperty.HousePropertyNetIncome,
                PayableHousingLoanPerYear = houseProperty.PayableHousingLoanPerYear,
                PropertyJointInterest = houseProperty.PropertyJointInterest == null ? Convert.ToString(0) : houseProperty.PropertyJointInterest,
                ConstrutionIsCompleted = houseProperty.ConstrutionIsCompleted,
                //isDetailsRequired = houseProperty.IsDetailsRequired,
                HousingLoanTakenBefore_01_04_1999 = houseProperty.HousingLoanTakenBefore_01_04_1999,
                PropertySelfOccupied = houseProperty.PropertySelfOccupied,
                PropertyLoanAmount = houseProperty.PropertyLoanAmount,
                PurposeOfLoan = houseProperty.PurposeOfLoan,
                Interest_RestrictedtoEmployee = houseProperty.Interest_RestrictedtoEmployee,
                DateofSanctionofLoan = houseProperty.DateofSanctionofLoan,
                Municipal_Water_Sewerage_taxpaid = houseProperty.Municipal_Water_Sewerage_taxpaid,
                Property_OwnersName = houseProperty.Property_OwnersName,
                //netIncomeOrLoss = houseProperty.NetIncomeOrLoss,
                PreConstructionInterest = houseProperty.PreConstructionInterest,
                NoOfProperties = houseProperty.NoOfProperties,
                GrossRentalIncome_PA = houseProperty.GrossRentalIncome_PA,
                TotalInterestOfYear = houseProperty.TotalInterestOfYear,
                LessMunicipalTaxes = houseProperty.LessMunicipalTaxes,
                Balance = houseProperty.Balance,
                EffectiveMonth = houseProperty.EffectiveMonth,
                EffectiveYear = houseProperty.EffectiveYear,
                LessStandardDeduction = houseProperty.LessStandardDeduction,
                LessInterestOnHousingLoan = houseProperty.LessInterestOnHousingLoan,
                IsHRACompleted = houseProperty.IsHRACompleted,
                GrossRentalIncome = houseProperty.GrossRentalIncome,
                EmployeeCode = houseProperty.EmployeeCode,
                EmployeeName = houseProperty.EmployeeName,
                CompanyName = houseProperty.CompanyName,
                PANNumber = houseProperty.PANNumber,
                Financial_Year = houseProperty.Financial_Year

            };
        }
    }

    public class jsonChallanEntry
    {
        public List<Guid> employeeID { get; set; }
        public List<Guid> categoryID { get; set; }
        public int month { get; set; }
        public DateTime challanDate { get; set; }

        public string bank { get; set; }

        public decimal challanAmount { get; set; }
        public string PayrollMonth { get; set; }
        public string challanNo { get; set; }

        public Guid bankName { get; set; }

        public string checkdd { get; set; }

        public bool bookEntry { get; set; }

        public Guid financialYear { get; set; }
        public string BSRCode { get; set; }
        public Guid Id { get; set; }

    }
    public class ChallanEmployee
    {
        public Guid Id { get; set; }
        public Guid EmpId { get; set; }

        public Guid Finyr { get; set; }
        public string EmployeeCode { get; set; }

        public String Name { get; set; }

        public decimal TaxAmount { get; set; }

        public string ChallanNo { get; set; }


        public string CheckDD { get; set; }
    }


    public class jsonMedicalInsurance
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid TxSectionId { get; set; }

        public Guid FinYearId { get; set; }

        public int EffectiveMonth { get; set; }

        public int EffectiveYear { get; set; }

        public string InsuranceType { get; set; }

        public string PolicyNo { get; set; }

        public DateTime DateofCommencofpolicy { get; set; }

        public string InsuredPersonName { get; set; }

        public string RelationshipoftheInsuredperson { get; set; }

        public string CoveredinthepolicyisSeniorCitizen { get; set; }

        public string IncurredinrespectofVerySeniorCitizen { get; set; }

        public decimal AmountofpremiumorExpense { get; set; }

        public string PayMode { get; set; }

        public decimal EligibleDeductionforthepolicy { get; set; }

        public decimal SelfSpouseChildOveralldeduction { get; set; }

        public decimal ParentOveralldeduction { get; set; }

        public decimal TotalDeduction { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModiifiedBy { get; set; }

        public bool IsDelete { get; set; }

        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        public string EmployeeCode { get; set; }
        public string Employeename { get; set; }
        public string CompanyName { get; set; }
        public string PANNumber { get; set; }
        public string Financial_Year { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalEligibleDeduction { get; set; }
        public string PolicyDate { get; set; }

        public static jsonMedicalInsurance toJson(MedicalInsurance MedicalInsurance)
        {
            string InsureType = MedicalInsurance.InsuranceType == "1" ? "Medical Insurance" : MedicalInsurance.InsuranceType == "2" ? "CGHS contribution" : MedicalInsurance.InsuranceType == "3" ? "Preventive Health Check up" : "Medical Expenditure";
            string RelationshipInsuredperson = MedicalInsurance.RelationshipoftheInsuredperson == "1" ? "Self Spouse & Child" : "Parent(s)";
            string CoveredpolicyisSeniorCitizen = MedicalInsurance.CoveredinthepolicyisSeniorCitizen == "1" ? "Senior Citizen" : "Normal";
            string IncurredrespectofVerySeniorCitizen = MedicalInsurance.IncurredinrespectofVerySeniorCitizen == "1" ? "Yes" : MedicalInsurance.IncurredinrespectofVerySeniorCitizen == "2" ? "NO" : "N/A";
            string pay = MedicalInsurance.PayMode == "1" ? "Cash" : "Other than Cash";
            string Policydate = MedicalInsurance.DateofCommencofpolicy.ToString("dd/MMM/yyyy");
            return new jsonMedicalInsurance()
            {
                Id = MedicalInsurance.Id,
                EmployeeId = MedicalInsurance.EmployeeId,
                TxSectionId = MedicalInsurance.TxSectionId,
                FinYearId = MedicalInsurance.FinYearId,
                EffectiveMonth = MedicalInsurance.EffectiveMonth,
                EffectiveYear = MedicalInsurance.EffectiveYear,
                InsuranceType = InsureType,
                PolicyNo = MedicalInsurance.PolicyNo,
                PolicyDate = Policydate,
                InsuredPersonName = MedicalInsurance.InsuredPersonName,
                RelationshipoftheInsuredperson = RelationshipInsuredperson,
                CoveredinthepolicyisSeniorCitizen = CoveredpolicyisSeniorCitizen,
                IncurredinrespectofVerySeniorCitizen = IncurredrespectofVerySeniorCitizen,
                AmountofpremiumorExpense = MedicalInsurance.AmountofpremiumorExpense,
                PayMode = pay,
                EligibleDeductionforthepolicy = MedicalInsurance.EligibleDeductionforthepolicy,
                SelfSpouseChildOveralldeduction = MedicalInsurance.SelfSpouseChildOveralldeduction,
                ParentOveralldeduction = MedicalInsurance.ParentOveralldeduction,
                TotalDeduction = MedicalInsurance.TotalDeduction,
                EmployeeCode = MedicalInsurance.EmployeeCode,
                Employeename = MedicalInsurance.Employeename,
                CompanyName = MedicalInsurance.CompanyName,
                PANNumber = MedicalInsurance.PANNumber,
                Financial_Year = MedicalInsurance.Financial_Year,
                TotalAmount = MedicalInsurance.TotalAmount,
                TotalEligibleDeduction = MedicalInsurance.TotalEligibleDeduction


            };
        }
        public static MedicalInsurance convertObject(jsonMedicalInsurance MedicalInsurance)
        {
            return new MedicalInsurance()
            {
                Id = MedicalInsurance.Id,
                EmployeeId = MedicalInsurance.EmployeeId,
                TxSectionId = MedicalInsurance.TxSectionId,
                FinYearId = MedicalInsurance.FinYearId,
                EffectiveMonth = MedicalInsurance.EffectiveMonth,
                EffectiveYear = MedicalInsurance.EffectiveYear,
                InsuranceType = MedicalInsurance.InsuranceType,
                PolicyNo = MedicalInsurance.PolicyNo,
                DateofCommencofpolicy = MedicalInsurance.DateofCommencofpolicy,
                InsuredPersonName = MedicalInsurance.InsuredPersonName,
                RelationshipoftheInsuredperson = MedicalInsurance.RelationshipoftheInsuredperson,
                CoveredinthepolicyisSeniorCitizen = MedicalInsurance.CoveredinthepolicyisSeniorCitizen,
                IncurredinrespectofVerySeniorCitizen = MedicalInsurance.IncurredinrespectofVerySeniorCitizen,
                AmountofpremiumorExpense = MedicalInsurance.AmountofpremiumorExpense,
                PayMode = MedicalInsurance.PayMode,
                EligibleDeductionforthepolicy = MedicalInsurance.EligibleDeductionforthepolicy,
                SelfSpouseChildOveralldeduction = MedicalInsurance.SelfSpouseChildOveralldeduction,
                ParentOveralldeduction = MedicalInsurance.ParentOveralldeduction,
                TotalDeduction = MedicalInsurance.TotalDeduction,
            };
        }
        public static jsonMedicalInsurance toJsonreport(MedicalInsurance MedicalInsurance, decimal p, decimal s, decimal td, decimal ted, decimal ta)
        {
            string InsureType = MedicalInsurance.InsuranceType == "1" ? "Medical Insurance" : MedicalInsurance.InsuranceType == "2" ? "CGHS contribution" : MedicalInsurance.InsuranceType == "3" ? "Preventive Health Check up" : "Medical Expenditure";
            string RelationshipInsuredperson = MedicalInsurance.RelationshipoftheInsuredperson == "1" ? "Self Spouse & Child" : "Parent(s)";
            string CoveredpolicyisSeniorCitizen = MedicalInsurance.CoveredinthepolicyisSeniorCitizen == "1" ? "Senior Citizen" : "Normal";
            string IncurredrespectofVerySeniorCitizen = MedicalInsurance.IncurredinrespectofVerySeniorCitizen == "1" ? "Yes" : MedicalInsurance.IncurredinrespectofVerySeniorCitizen == "2" ? "NO" : "N/A";
            string pay = MedicalInsurance.PayMode == "1" ? "Cash" : "Other than Cash";
            string Policydate = MedicalInsurance.DateofCommencofpolicy.ToString("dd/MMM/yyyy");
            return new jsonMedicalInsurance()
            {
                Id = MedicalInsurance.Id,
                EmployeeId = MedicalInsurance.EmployeeId,
                TxSectionId = MedicalInsurance.TxSectionId,
                FinYearId = MedicalInsurance.FinYearId,
                EffectiveMonth = MedicalInsurance.EffectiveMonth,
                EffectiveYear = MedicalInsurance.EffectiveYear,
                InsuranceType = InsureType,
                PolicyNo = MedicalInsurance.PolicyNo,
                PolicyDate = Policydate,
                InsuredPersonName = MedicalInsurance.InsuredPersonName,
                RelationshipoftheInsuredperson = RelationshipInsuredperson,
                CoveredinthepolicyisSeniorCitizen = CoveredpolicyisSeniorCitizen,
                IncurredinrespectofVerySeniorCitizen = IncurredrespectofVerySeniorCitizen,
                AmountofpremiumorExpense = MedicalInsurance.AmountofpremiumorExpense,
                PayMode = pay,
                EligibleDeductionforthepolicy = MedicalInsurance.EligibleDeductionforthepolicy,
                SelfSpouseChildOveralldeduction = s,
                ParentOveralldeduction = p,
                TotalDeduction = td,
                EmployeeCode = MedicalInsurance.EmployeeCode,
                Employeename = MedicalInsurance.Employeename,
                CompanyName = MedicalInsurance.CompanyName,
                PANNumber = MedicalInsurance.PANNumber,
                Financial_Year = MedicalInsurance.Financial_Year,
                TotalAmount = ta,
                TotalEligibleDeduction = ted


            };
        }

    }
}

