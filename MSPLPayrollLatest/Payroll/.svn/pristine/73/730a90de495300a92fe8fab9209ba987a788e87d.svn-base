using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using PayRollReports;
using Microsoft.Reporting.WebForms;
using System.IO;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class ReportsController : BaseController
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }


        public List<Employee> GetEmployeeList(string category, List<DataWizardController.jsonPaySheetattr> filters)
        {
            string[] categories = category.TrimEnd(',').Split(',');
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            // Entity entity;
            string outfilePath = DocumentProcessingSettings.TempDirectoryPath;
            List<Employee> employeelist = new List<Employee>();
            List<Paysheetatrr> ps = new List<Paysheetatrr>();
            Dictionary<string, string> filterExpr = new Dictionary<string, string>();


            if (filters != null)
            {
                filterExpr = DataWizardController.GetFilterExpr(companyId, filters);
            }
            foreach (string st in categories)
            {
                employeelist.AddRange((new EmployeeList(companyId, new Guid(st), (from k in filterExpr where string.Compare(k.Key, "employee", true) == 0 select k.Value).FirstOrDefault(), userId, new Guid(Convert.ToString(Session["EmployeeGUID"])))));
            }

            return employeelist;

        }


        #region "Statutory Reports"
        public JsonResult getPFExtact(string category, string title, int smonth, int syear, List<DataWizardController.jsonPaySheetattr> filters)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attr = new AttributeModelList(companyId);
            PayrollHistory payhistory = new PayrollHistory();
            List<Employee> employeelist = GetEmployeeList(category, filters);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<AttributeModel> childList = attr.Where(a => a.Name == "PF").ToList<AttributeModel>();
            childList.AddRange(attr.Where(a => a.Name == "PF").ToList<AttributeModel>());
            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            string outfilePath = string.Empty;
            List<Object> result = new List<object>();
            employeelist.ForEach(em =>
            {
                title = string.Empty;
                ReportParameterCollection rpc = new ReportParameterCollection();
                payhistory = new PayrollHistory(companyId, em.Id, syear, smonth);
                childList.ForEach(clist =>
                {
                    if (ReferenceEquals(data.Where(d => d.Key == clist.Name).FirstOrDefault().Key, null))
                    {
                        data.Add(clist.Name, payhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault().Value);
                    }

                });
                var a = new object();//= data;

                result.Add(a);
                string outputfilePath = string.Empty;


            });
            Company company = new Company(companyId, userId);
            rptParameters newrpt = new rptParameters();
            newrpt.rptPath = "Reports/PFExtract.rdlc";
            newrpt.exportto = "PDF";
            newrpt.filePath = rptPath + "PFExtract_" + smonth.ToString() + syear.ToString() + ".pdf";
            newrpt.rptDataSetName = "DSPFExtract";
            newrpt.rptDataSet = result;

            newrpt.rpc.Add(new ReportParameter("Company", company.CompanyAddress));
            newrpt.rpc.Add(new ReportParameter("Title", ("PROVIDENT FUND EXTRACT FOR THE MONTH OF " + (MonthEnum)smonth + " " + syear).ToUpper()));
            rptParameters.generateprt(newrpt);
            outfilePath = newrpt.filePath;

            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
        }
        //public JsonResult getPFForm3A(string category, string title, int syear, int smonth, int nYear, int nMonth, List<DataWizardController.jsonPaySheetattr> filters)
        public JsonResult getPFForm3A(string category, string title, int syear, int smonth, int nYear, int nMonth, List<DataWizardController.jsonPaySheetattr> filters, String Empcode)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeList emplist = new EmployeeList(companyId);
            var emp = emplist.Where(f => f.EmployeeCode == Empcode).FirstOrDefault();
            if (emp != null)
            {
                Session["EmployeeId"] = emp.Id;
            }
            else
            {
                return base.BuildJson(false, 200, "Invalid Employee code", null);
            }
            AttributeModelList attr = new AttributeModelList(companyId);
            PayrollHistoryList payhistoryList = new PayrollHistoryList(companyId, syear, smonth, nYear, nMonth, emp == null ? Guid.Empty : emp.Id);
            List<Employee> employeelist = GetEmployeeList(category, filters);
            List<AttributeModel> childList = new List<AttributeModel>();
            childList.AddRange(attr.Where(a => a.Name == "PF").ToList<AttributeModel>());
            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            string outfilePath = string.Empty;
            employeelist.ForEach(em =>
            {
                title = string.Empty;
                ReportParameterCollection rpc = new ReportParameterCollection();
                PayrollHistoryList empPayhistory = payhistoryList;
                List<object> resultdata = new List<object>();
                empPayhistory.ForEach(pay =>
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    PayrollHistoryValue currdata = new PayrollHistoryValue();
                    childList.ForEach(clist =>
                    {
                        if (ReferenceEquals(data.Where(d => d.Key == clist.Name).FirstOrDefault().Key, null))
                        {
                            data.Add(clist.Name, pay.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault().Value);
                            currdata = pay.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault();
                        }
                    });
                    string EPF = ""; string FPF = "";
                    attr.ForEach(f =>
                    {
                        if (f.Name == "EPF")
                        {
                            EPF = pay.PayrollHistoryValueList.Where(p => p.AttributeModelId == f.Id).FirstOrDefault().Value;
                        }
                        else if (f.Name == "FPF")
                        {
                            FPF = pay.PayrollHistoryValueList.Where(p => p.AttributeModelId == f.Id).FirstOrDefault().Value;
                        }
                    });
                    MonthlyInput MIList = new MonthlyInput();
                    if (pay.Month >= 4 && pay.Month <= 12)
                    {
                        MonthlyInputList mlist = new MonthlyInputList(Guid.Empty, em.Id, pay.Month, syear);
                        MIList = mlist.Where(d => d.Attributemodel.Name == "LD").FirstOrDefault();
                    }
                    else
                    {
                        MonthlyInputList mlist = new MonthlyInputList(Guid.Empty, em.Id, pay.Month, nYear);
                        MIList = mlist.Where(d => d.Attributemodel.Name == "LD").FirstOrDefault();
                    }

                    //= data;
                    resultdata.Add(new
                    {
                        EmpCode = em.EmployeeCode,
                        EmpName = em.FirstName,
                        MONTH = ((MonthEnum)pay.Month).ToString(),
                        AMOUNTOFWAGES = data == null ? string.Empty : currdata.BaseValue,
                        EPF = data == null ? string.Empty : currdata.Value,
                        EmployerPF = EPF,
                        PENSIONFUND = FPF,
                        REFUND = "",
                        REMARKS = "",
                        NOOFDAYCONTR = MIList.Value,
                    });

                });
                string outputfilePath = string.Empty;
                Company company = new Company(companyId, userId);
                int TotalA_Wages = 0; int TotalEPF = 0; int TotalFPF = 0; int TotalEmp_PF = 0;
                if (resultdata.Count > 0)
                {
                    foreach (object[] item in resultdata)
                    {
                        TotalA_Wages = TotalA_Wages + Convert.ToInt16(item[3]);
                        TotalEPF = TotalEPF + Convert.ToInt16(item[4]);
                        TotalFPF = TotalFPF + Convert.ToInt16(item[6]);
                        TotalEmp_PF = TotalEmp_PF + Convert.ToInt16(item[5]);

                    }
                }
                rptParameters newrpt = new rptParameters();
                newrpt.rptPath = "Reports/Form3A.rdlc";
                newrpt.exportto = "PDF";
                newrpt.filePath = rptPath + em.EmployeeCode + ".pdf";
                newrpt.rptDataSetName = "DSPFExtract";
                newrpt.rptDataSet = resultdata;
                Emp_Personal empPersonal = new Emp_Personal(em.Id);
                newrpt.rpc.Add(new ReportParameter("Company", company.CompanyName));
                newrpt.rpc.Add(new ReportParameter("Title", "Contribution Card for the Currency Period from April " + syear + "To March " + nYear));
                newrpt.rpc.Add(new ReportParameter("EmployeeName", em.FirstName));
                newrpt.rpc.Add(new ReportParameter("AccountNo", empPersonal.BankAccountNo));
                newrpt.rpc.Add(new ReportParameter("RelationName", empPersonal.FatherName));
                newrpt.rpc.Add(new ReportParameter("EmployerAddress", company.CompanyAddress));
                rptParameters.generateprt(newrpt);
                outfilePath = newrpt.filePath;
            });
            if (employeelist.Count > 1)
            {
                string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
                ZipPath(PDFFilePath, outfilePath, null, true, null);
                outfilePath = PDFFilePath;
            }
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
        }
        public JsonResult getPFChallan(string category, string title, int smonth, int syear, List<DataWizardController.jsonPaySheetattr> filters, string chqno, DateTime? chqDate, DateTime? payDate)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attr = new AttributeModelList(companyId);
            PayrollHistory payhistory = new PayrollHistory();
            List<Employee> employeelist = GetEmployeeList(category, filters);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<AttributeModel> childList = attr.Where(a => a.Name == "PF").ToList<AttributeModel>();
            childList.AddRange(attr.Where(a => a.Name == "PF").ToList<AttributeModel>());
            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            string outfilePath = string.Empty;
            employeelist.ForEach(em =>
            {
                title = string.Empty;
                ReportParameterCollection rpc = new ReportParameterCollection();
                payhistory = new PayrollHistory(companyId, em.Id, syear, smonth);
                childList.ForEach(clist =>
                {
                    if (ReferenceEquals(data.Where(d => d.Key == clist.Name).FirstOrDefault().Key, null))
                    {
                        data.Add(clist.Name, payhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault().Value);
                    }

                });
                var a = new object();//= data;
                a = new
                {
                    EmployeeSharContrAcct1 = "",
                    EmployrSharContrAcct1 = "",
                    AdiminstrationChargeAcct1 = "",
                    InspectionChargeAcct1 = "",
                    PenalChargAcct1 = "",
                    miscellneouschargAcct1 = "",
                    EmployeeSharContrAcct2 = "",
                    EmployrSharContrAcct2 = "",
                    AdiminstrationChargeAcct2 = "",
                    InspectionChargeAcct2 = "",
                    PenalChargAcct2 = "",
                    miscellneouschargAcct2 = "",
                    EmployeeSharContrAcct10 = "",
                    EmployrSharContrAcct10 = "",
                    AdiminstrationChargeAcct10 = "",
                    InspectionChargeAcct10 = "",
                    PenalChargAcct10 = "",
                    miscellneouschargAcct10 = "",
                    EmployeeSharContrAcct21 = "",
                    EmployrSharContrAcct21 = "",
                    AdiminstrationChargeAcct21 = "",
                    InspectionChargeAcct21 = "",
                    PenalChargAcct21 = "",
                    miscellneouschargAcct21 = "",
                    EmployeeSharContrAcct22 = "",
                    EmployrSharContrAcct22 = "",
                    AdiminstrationChargeAcct22 = "",
                    InspectionChargeAcct22 = "",
                    PenalChargAcct22 = "",
                    miscellneouschargAcct22 = "",

                };

                string outputfilePath = string.Empty;
                Company company = new Company(companyId, userId);

                rptParameters newrpt = new rptParameters();
                newrpt.rptPath = "Reports/PFChallan.rdlc";
                newrpt.exportto = "PDF";
                newrpt.filePath = rptPath + em.EmployeeCode + ".pdf";
                newrpt.rptDataSetName = "DSPFExtract";
                newrpt.rptDataSet = a;

                newrpt.rpc.Add(new ReportParameter("Company", company.CompanyAddress));
                newrpt.rpc.Add(new ReportParameter("Title", "THE EMPLOYEE'S PROVIDENT FUND SCHEME, 1952. ( Paras 35 & 42 ) and The Employees' Family Pension Scheme, 1995 (Para 19)"));
                rptParameters.generateprt(newrpt);
                outfilePath = newrpt.filePath;
            });
            if (employeelist.Count > 1)
            {
                string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
                ZipPath(PDFFilePath, outfilePath, null, true, null);
                outfilePath = PDFFilePath;
            }
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
        }

        public JsonResult getESIExtract(string category, string title, int smonth, int syear, List<DataWizardController.jsonPaySheetattr> filters)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attr = new AttributeModelList(companyId);
            PayrollHistory payhistory = new PayrollHistory();
            List<Employee> employeelist = GetEmployeeList(category, filters);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<AttributeModel> childList = attr.Where(a => a.Name == "PF").ToList<AttributeModel>();
            childList.AddRange(attr.Where(a => a.Name == "PF").ToList<AttributeModel>());
            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            string outfilePath = string.Empty;
            List<Object> result = new List<object>();
            employeelist.ForEach(em =>
            {
                title = string.Empty;
                ReportParameterCollection rpc = new ReportParameterCollection();
                payhistory = new PayrollHistory(companyId, em.Id, syear, smonth);
                childList.ForEach(clist =>
                {
                    if (ReferenceEquals(data.Where(d => d.Key == clist.Name).FirstOrDefault().Key, null))
                    {
                        data.Add(clist.Name, payhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault().Value);
                    }
                });
                var a = new object();//= data;
                a = new
                {
                    EmployeeContr = "",
                    EmployerContr = "",
                    Damages = "",
                    TotalWages = "",
                };
                result.Add(a);
                string outputfilePath = string.Empty;

            });
            Company company = new Company(companyId, userId);
            rptParameters newrpt = new rptParameters();
            newrpt.rptPath = "Reports/PFExtract.rdlc";
            newrpt.exportto = "PDF";
            newrpt.filePath = rptPath + "PFExtract_" + smonth.ToString() + syear.ToString() + ".pdf";
            newrpt.rptDataSetName = "DSPFExtract";
            newrpt.rptDataSet = result;

            newrpt.rpc.Add(new ReportParameter("Company", company.CompanyAddress));
            newrpt.rpc.Add(new ReportParameter("BankName", company.CompanyAddress));

            newrpt.rpc.Add(new ReportParameter("Title", ("PROVIDENT FUND EXTRACT FOR THE MONTH OF " + (MonthEnum)smonth + " " + syear).ToUpper()));
            rptParameters.generateprt(newrpt);
            outfilePath = newrpt.filePath;

            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
        }
        public JsonResult getEsiChallan(string category, string title, int smonth, int syear, List<DataWizardController.jsonPaySheetattr> filters, string chqno, DateTime? chqDate, DateTime? payDate)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attr = new AttributeModelList(companyId);
            PayrollHistory payhistory = new PayrollHistory();
            List<Employee> employeelist = GetEmployeeList(category, filters);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<AttributeModel> childList = attr.Where(a => a.Name == "PF").ToList<AttributeModel>();
            childList.AddRange(attr.Where(a => a.Name == "PF").ToList<AttributeModel>());
            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            string outfilePath = string.Empty;
            employeelist.ForEach(em =>
            {
                title = string.Empty;
                ReportParameterCollection rpc = new ReportParameterCollection();
                payhistory = new PayrollHistory(companyId, em.Id, syear, smonth);
                childList.ForEach(clist =>
                {
                    if (ReferenceEquals(data.Where(d => d.Key == clist.Name).FirstOrDefault().Key, null))
                    {
                        data.Add(clist.Name, payhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == clist.Id).FirstOrDefault().Value);
                    }

                });
                var a = new object();//= data;
                a = new
                {
                    EmpCode = em.EmployeeCode,
                    EmpName = em.FirstName,
                    Dayspresent = "",
                    PFACNO = data.Where(d => d.Key == "PF").FirstOrDefault().Key != null ? data["PF"] : "0",
                    SALARY = "",
                    EPFACNO = data.Where(d => d.Key == "EPF").FirstOrDefault().Key != null ? data["EPF"] : "0",
                    PENSHME = string.Empty,
                    EMPLOYERCONTR = string.Empty,
                    VPF = "",
                    INSCHARG = data.Where(d => d.Key == "PFINSPECTION").FirstOrDefault().Key != null ? data["PFINSPECTION"] : "0",
                    EDLICHRG = data.Where(d => d.Key == "PFEDLI").FirstOrDefault().Key != null ? data["PFEDLI"] : "0"
                };
                string outputfilePath = string.Empty;
                Company company = new Company(companyId, userId);

                rptParameters newrpt = new rptParameters();
                newrpt.rptPath = "Reports/ESIChallan.rdlc";
                newrpt.exportto = "PDF";
                newrpt.filePath = rptPath + em.EmployeeCode + ".pdf";
                newrpt.rptDataSetName = "DSPFExtract";
                newrpt.rptDataSet = a;

                newrpt.rpc.Add(new ReportParameter("Company", company.CompanyAddress));
                newrpt.rpc.Add(new ReportParameter("Title", "THE EMPLOYEE'S PROVIDENT FUND SCHEME, 1952. ( Paras 35 & 42 ) and The Employees' Family Pension Scheme, 1995 (Para 19)"));
                rptParameters.generateprt(newrpt);
                outfilePath = newrpt.filePath;
            });
            if (employeelist.Count > 1)
            {
                string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
                ZipPath(PDFFilePath, outfilePath, null, true, null);
                outfilePath = PDFFilePath;
            }

            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
        }

        #endregion

    }



}