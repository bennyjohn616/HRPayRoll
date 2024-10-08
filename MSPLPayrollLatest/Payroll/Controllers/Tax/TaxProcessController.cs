﻿using Microsoft.Reporting.WebForms;
using Payroll.CustomFilter;
using Payroll.Helpers;
using PayrollBO;
using PayrollBO.Tax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TraceError;

namespace Payroll.Controllers.Tax
{
    [SessionExpireAttribute]
    public class TaxProcessController : BaseController
    {
        // GET: TaxProcess
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxProcess(Guid selectedId, int year, int month, string type, Guid financeYearId, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string username = Convert.ToString(Session["username"]);
            string Msg = string.Empty;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            var emproll = Convert.ToString(Session["RoleName"]);
            var cutoffdate = Convert.ToDateTime(Session["EntryDate"]);
            var cudate = cutoffdate.Day;
            var cumonth = cutoffdate.Month;
            var cuyear = cutoffdate.Year;
            var empapplyDate = new DateTime(cuyear, cumonth, 01);
            DateTime tempDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (processtype == "employee")
            {
                if (cumonth != month || cuyear != year)
                {
                    Msg = "Employee cutoffdate and processing month & year not matching";
                    return base.BuildJson(Msg == string.Empty ? true : false, Msg == string.Empty ? 200 : 100, Msg == string.Empty ? "Error" : Msg, null);
                }
            }

            EntityModel entModel = new EntityModel(ComValue.SalaryTable, companyId);
            Entity ent = new Entity(entModel.Id, Guid.Empty);
            string empty = "";
            EntityMappingList entMappinglist = new EntityMappingList(ent.EntityModelId, empty);


            employeelist.ToList().ForEach(e =>
                {
                    if (new DateTime(e.DateOfJoining.Year, e.DateOfJoining.Month, e.DateOfJoining.Day) > tempDate)
                        employeelist.Remove(e);
                    // if (new EntityMapping("Employee", "dsfdf".ToString(), entModel.Id) == null)
                    //     employeelist.Remove(e);
                });

            var emplist = employeelist.Where(u => u.Status == 1).ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            TaxHistory tx = new TaxHistory();
            string dbstring = Convert.ToString(Session["DBString"]);

            /*       HttpRuntime.Cache.Insert(
                      "DBString"
                      , dbstring
                      , null
                      , DateTime.Now.AddMinutes(1440)
                      , Cache.NoSlidingExpiration
                      );*/

            {

            }
            Msg = It_Init_Para(entMappinglist, emplist, selectedId, year, month, type, financeYearId, companyId, emproll, userId, username, Msg, processtype);
            return base.BuildJson(Msg == string.Empty ? true : false, Msg == string.Empty ? 200 : 100, Msg == string.Empty ? "Data saved successfully" : Msg, null);
        }

        public void MoveToClass(List<StructIncMatchList> structIncMatchLists, IncomeMatchingList IncList)
        {
            for (int i = 0; i < structIncMatchLists.Count; i++)
            {
                IncomeMatching im1 = new IncomeMatching();
                im1.AttributemodelId = structIncMatchLists[i].AttributemodelId;
                im1.CreatedBy = structIncMatchLists[i].CreatedBy;
                im1.CreatedOn = structIncMatchLists[i].CreatedOn;
                im1.ExemptionComponent = structIncMatchLists[i].ExemptionComponent;
                im1.FinancialYearId = structIncMatchLists[i].FinancialYearId;
                im1.Formula = structIncMatchLists[i].Formula;
                im1.GrossSection = structIncMatchLists[i].GrossSection;
                im1.Id = structIncMatchLists[i].Id;
                im1.IsDeleted = structIncMatchLists[i].IsDeleted;
                im1.MatchingComponent = structIncMatchLists[i].MatchingComponent;
                im1.ModifiedBy = structIncMatchLists[i].ModifiedBy;
                im1.ModifiedOn = structIncMatchLists[i].ModifiedOn;
                im1.Operator = structIncMatchLists[i].Operator;
                im1.OrderNo = structIncMatchLists[i].OrderNo;
                im1.OtherComponent = structIncMatchLists[i].OtherComponent;
                im1.Projection = structIncMatchLists[i].Projection;
                IncList.Add(im1);
            }

        }

        public String It_Init_Para(EntityMappingList entMappinglist, List<Employee> emplist, Guid selectedId, int year, int month, string type, Guid financeYearId, int companyId, string emproll, int userId, string username, string Msg, string processtype)
        {

            TXSectionList allSection = new TXSectionList(companyId, financeYearId, Guid.Empty);
            TXSectionList section = new TXSectionList();
            TXSectionList subSections = new TXSectionList();
            TXSectionList otherSections = new TXSectionList();
            AttributeModelList attrlist = new AttributeModelList(companyId);
            subSections.AddRange(allSection.Where(s => s.ParentId != Guid.Empty).ToList().OrderBy(s => s.OrderNo));
            section.AddRange(allSection.Where(s => s.ParentId == Guid.Empty && s.SectionType != "Others").OrderBy(s => s.OrderNo));
            otherSections.AddRange(allSection.Where(s => s.SectionType == "Others").OrderBy(s => s.OrderNo));
            TXFinanceYear FinanceYear = new TXFinanceYear(financeYearId, companyId);
            TaxBehaviorList taxbehav = new TaxBehaviorList(financeYearId, Guid.Empty, Guid.Empty);
            PayrollHistoryList payrollhistorylistall = new PayrollHistoryList();
            TaxHistoryList futureTaxList = new TaxHistoryList();
            TXFinanceYearList txfl = new TXFinanceYearList(companyId);
            TXEmployeeSectionList TxEmployeeSectionList = new TXEmployeeSectionList();
            IncrementList increment = new IncrementList();
            IncomeMatchingList IncMatchlist = new IncomeMatchingList(financeYearId);
            List<StructIncMatchList> structIncMatchLists = new List<StructIncMatchList>();
            if (!Object.ReferenceEquals(IncMatchlist, null))
            {
                foreach (var income in IncMatchlist)
                {
                    StructIncMatchList temp1 = new StructIncMatchList();
                    temp1.AttributemodelId = income.AttributemodelId;
                    temp1.CreatedBy = income.CreatedBy;
                    temp1.CreatedOn = income.CreatedOn;
                    temp1.ExemptionComponent = income.ExemptionComponent;
                    temp1.FinancialYearId = income.FinancialYearId;
                    temp1.Formula = income.Formula;
                    temp1.GrossSection = income.GrossSection;
                    temp1.Id = income.Id;
                    temp1.IsDeleted = income.IsDeleted;
                    temp1.MatchingComponent = income.MatchingComponent;
                    temp1.ModifiedBy = income.ModifiedBy;
                    temp1.ModifiedOn = income.ModifiedOn;
                    temp1.Operator = income.Operator;
                    temp1.OrderNo = income.OrderNo;
                    temp1.OtherComponent = income.OtherComponent;
                    temp1.Projection = income.Projection;
                    temp1.TaxDeductionMode = income.TaxDeductionMode;
                    structIncMatchLists.Add(temp1);
                }
            }

            // IncrementList increment = new IncrementList(emp.Id);

            DateTime EffectiveDate = new DateTime(year, month, 1);

            var VPFReq = new Company(companyId).VPFProjectionRequired;
            var VPFProjection = new Company(companyId).VPFProjection;

            DateTime applyDate = new DateTime(year, month, 1);


            if (type == "Single Employee")
            {
                payrollhistorylistall = new PayrollHistoryList(emplist[0].Id, companyId);
                TxEmployeeSectionList = new TXEmployeeSectionList(emplist[0].Id, EffectiveDate);
                increment = new IncrementList(emplist[0].Id);
            }
            else
            {
                // PayrollHistoryList payHistory = new PayrollHistoryList(taxinfo.CompanyId, financeStartYear, financeStartMonth, taxinfo.ApplyYear, taxinfo.ApplyMonth, emp.Id);
                Guid empnull = new Guid();
                payrollhistorylistall = new PayrollHistoryList(companyId, FinanceYear.StartingDate.Year, FinanceYear.StartingDate.Month, applyDate.Year, applyDate.Month, empnull);
                TxEmployeeSectionList = new TXEmployeeSectionList(Guid.Empty, EffectiveDate);
                increment = new IncrementList(month, year);
            }

            if (processtype == "employee")
            {
                futureTaxList = new TaxHistoryList(financeYearId, applyDate, emproll);
            }
            else
            {
                futureTaxList = new TaxHistoryList(financeYearId, applyDate);
            }


            DataTable dt1 = new DataTable();

            dt1.Columns.Add("FinanceYearId", typeof(Guid));
            dt1.Columns.Add("ApplyDate", typeof(DateTime));
            dt1.Columns.Add("EmployeeId", typeof(Guid));
            dt1.Columns.Add("FieldId", typeof(Guid));
            dt1.Columns.Add("Field", typeof(String));
            dt1.Columns.Add("FieldType", typeof(String));
            dt1.Columns.Add("Actual", typeof(Decimal));
            dt1.Columns.Add("Projection", typeof(Decimal));
            dt1.Columns.Add("Total", typeof(Decimal));
            dt1.Columns.Add("Limit", typeof(Decimal));
            dt1.Columns.Add("Createdby", typeof(int));
            dt1.Columns.Add("ModifiedBy", typeof(int));
            dt1.Columns.Add("ActualMonth", typeof(int));


            DataTable dt2 = new DataTable();

            dt2.Columns.Add("Id", typeof(Guid));
            dt2.Columns.Add("FinanceYearId", typeof(Guid));
            dt2.Columns.Add("ApplyDate", typeof(DateTime));
            dt2.Columns.Add("EmployeeId", typeof(Guid));
            dt2.Columns.Add("FieldId", typeof(Guid));
            dt2.Columns.Add("Field", typeof(String));
            dt2.Columns.Add("FieldType", typeof(String));
            dt2.Columns.Add("Actual", typeof(Decimal));
            dt2.Columns.Add("Projection", typeof(Decimal));
            dt2.Columns.Add("Total", typeof(Decimal));
            dt2.Columns.Add("Limit", typeof(Decimal));
            dt2.Columns.Add("CreatedBy", typeof(int));

            DataTable dt3 = new DataTable();


            dt3.Columns.Add("FinanceYearId", typeof(Guid));
            dt3.Columns.Add("ApplyDate", typeof(DateTime));
            dt3.Columns.Add("EmployeeId", typeof(Guid));
            dt3.Columns.Add("FieldId", typeof(Guid));
            dt3.Columns.Add("Field", typeof(String));
            dt3.Columns.Add("FieldType", typeof(String));
            dt3.Columns.Add("Actual", typeof(Decimal));
            dt3.Columns.Add("Projection", typeof(Decimal));
            dt3.Columns.Add("Total", typeof(Decimal));
            dt3.Columns.Add("Limit", typeof(Decimal));
            dt3.Columns.Add("Createdby", typeof(int));
            dt3.Columns.Add("ModifiedBy", typeof(int));
            dt3.Columns.Add("ActualMonth", typeof(int));

            DataTable dt4 = new DataTable();

            dt4.Columns.Add("Id", typeof(Guid));
            dt4.Columns.Add("FinanceYearId", typeof(Guid));
            dt4.Columns.Add("ApplyDate", typeof(DateTime));
            dt4.Columns.Add("EmployeeId", typeof(Guid));
            dt4.Columns.Add("FieldId", typeof(Guid));
            dt4.Columns.Add("Field", typeof(String));
            dt4.Columns.Add("FieldType", typeof(String));
            dt4.Columns.Add("Actual", typeof(Decimal));
            dt4.Columns.Add("Projection", typeof(Decimal));
            dt4.Columns.Add("Total", typeof(Decimal));
            dt4.Columns.Add("Limit", typeof(Decimal));
            dt4.Columns.Add("CreatedBy", typeof(int));




            int i = 0;


            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 12
            };

           
            //  Parallel.ForEach(emplist, emp =>

            // Parallel.ForEach(emplist,options, emp =>

            emplist.ForEach(emp =>
            {

                i++;
                string user = "";
                // var hubcontext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                foreach (var item in HubClient.clients)
                {
                    if (item.Value == username)
                    {
                        user = item.Key;
                        SignalRCls.SendProgress(user, "Process in progress...", i, emplist.Count);
                    }
                }

                EmployeeList emplis = new EmployeeList();
                TaxComputationInfo taxinfo = new TaxComputationInfo();
                taxinfo.ApplyMonth = month;
                taxinfo.ApplyYear = year;
                emp.financeyearid = financeYearId;
                emp.Startingdate = FinanceYear.StartingDate;
                emp.Endingdate = FinanceYear.EndingDate;
                emplis.Add(emp);
                List<PayrollError> payErrors = new List<PayrollError>();
                PayrollHistory payHistory = new PayrollHistory();
                var entmap = entMappinglist.Where(ent => ent.RefEntityId.ToLower() == emp.Id.ToString()).FirstOrDefault();
                if (!ReferenceEquals(entmap, null))
                {
                    taxinfo.EntityId = new Guid(entmap.EntityId);
                    taxinfo.EntityModelId = new Guid(entMappinglist.Where(ent => ent.RefEntityId.ToLower() == emp.Id.ToString()).FirstOrDefault().EntityTableName);
                    taxinfo.entity = payHistory.ExecuteProcessTemp(companyId, emp.Id, taxinfo.ApplyYear, taxinfo.ApplyMonth, taxinfo.EntityId, taxinfo.EntityModelId, out payErrors);
                    if (payErrors.Count > 0)
                    {
                        payErrors.ForEach(e =>
                        {
                            Msg += e.ErrorMessage.ToString();
                        });
                    }


                    taxinfo.CompanyId = companyId;
                    taxinfo.age = emp.Age;
                    taxinfo.FinanceYear = FinanceYear;
                    taxinfo.FinanceYearId = financeYearId;
                    taxinfo.StructList = structIncMatchLists;
                    taxinfo.incmatchList = new IncomeMatchingList();
                    taxinfo.MoveToClass(taxinfo.StructList, taxinfo.incmatchList);
                    // taxinfo.incmatchList.AddRange((IEnumerable<IncomeMatching>)structIncMatchLists);

                    taxinfo.UserId = userId;
                    taxinfo.AttributemodelList.AddRange(attrlist.OrderBy(o => o.OrderNumber).ToList());
                    taxinfo.EffectiveDate = new DateTime(taxinfo.ApplyYear, taxinfo.ApplyMonth, 1);
                    taxinfo.Sections = section;
                    taxinfo.SubSections = subSections;
                    taxinfo.OtherIncomeHeads = otherSections;
                    taxinfo.allsection = allSection;
                    taxinfo.VPFReq = VPFReq;
                    taxinfo.VPFProjection = VPFProjection;
                    taxinfo.FandFFlag = false;
                    if (emp.SeparationDate != DateTime.MinValue && emp.SeparationDate != null && emp.SeparationDate.Month == taxinfo.ApplyMonth && emp.SeparationDate.Year == taxinfo.ApplyYear)
                    {
                        taxinfo.FandFFlag = true;
                    }

                    if (type.ToUpper() == "FANDF")
                    {
                        taxinfo.FandFFlag = true;
                    }

                    taxinfo.emproll = emproll;

                    if (taxinfo.emproll.ToUpper() != "ADMIN")
                    {
                        taxinfo.emproll = "Employee";
                    }
                    taxinfo.finyearlist = new TXFinanceYearList();
                    taxinfo.finyearlist.AddRange(txfl.Where(tf => tf.CompanyId == taxinfo.CompanyId).ToList());
                    taxinfo.processtype = processtype;
                    taxinfo.payrollhistorylist = new PayrollHistoryList();
                    taxinfo.payrollhistorylist.AddRange(payrollhistorylistall.Where(ph => ph.CompanyId == taxinfo.CompanyId && ph.EmployeeId == emp.Id && ph.Status.ToUpper() == "PROCESSED").ToList());
                    PayrollHistoryList curpay = new PayrollHistoryList();
                    curpay.AddRange(taxinfo.payrollhistorylist.Where(ph => ph.CompanyId == taxinfo.CompanyId && ph.Year == taxinfo.ApplyYear && ph.Month == taxinfo.ApplyMonth && ph.EmployeeId == emp.Id).ToList());
                    if (!ReferenceEquals(curpay, null) && curpay.Count > 0)
                    {
                        if (processtype == "employee")
                        {
                            Msg += "Salary Already processed for " + emp.EmployeeCode;
                            emplis.Remove(emplis.Where(el => el.EmployeeCode == emp.EmployeeCode).FirstOrDefault());
                        }
                    }

                    TaxHistoryList futureTaxListCheck = new TaxHistoryList();
                    futureTaxListCheck.AddRange(futureTaxList.Where(ft => ft.EmployeeId == emp.Id).ToList());
                    taxinfo.TxEmployeeSectionList = new TXEmployeeSectionList();
                    taxinfo.TxEmployeeSectionList.AddRange(TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.EffectiveDate == taxinfo.EffectiveDate).ToList());
                    taxinfo.increment = new IncrementList();
                    taxinfo.increment.AddRange(increment.Where(inc => inc.EmployeeId == emp.Id).ToList());
                    taxinfo.TaxBehaviorList = new TaxBehaviorList();
                    taxinfo.TaxBehaviorList.AddRange(taxbehav);
                    taxinfo.TXProjIncome = new TXProjIncome(financeYearId, emp.Id, taxinfo.ApplyMonth, taxinfo.ApplyYear);
                    TaxHistory tx = new TaxHistory();
                    if (!ReferenceEquals(emplis, null) && emplis.Count > 0)
                    {
                        Msg += tx.ProcessTax(taxinfo, taxinfo.payrollhistorylist, futureTaxListCheck, emplis, year, month, type, financeYearId, companyId, userId);
                    }

                    if (processtype != "employee")
                    {
                        if ((!object.ReferenceEquals(null, taxinfo.dt1) && taxinfo.dt1.Rows.Count > 0))
                        {

                            for (int j = 0; j < taxinfo.dt1.Rows.Count; j++)
                            {
                                dt1.ImportRow(taxinfo.dt1.Rows[j]);
                            }
                        }
                    }
                    else
                    {
                        if ((!object.ReferenceEquals(null, taxinfo.dt1) && taxinfo.dt1.Rows.Count > 0))
                        {

                            for (int j = 0; j < taxinfo.dt1.Rows.Count; j++)
                            {
                                dt3.ImportRow(taxinfo.dt1.Rows[j]);
                            }
                        }
                    }


                    if (processtype != "employee")
                    {
                        if ((!object.ReferenceEquals(null, taxinfo.dt2) && taxinfo.dt2.Rows.Count > 0))
                        {
                            for (int j = 0; j < taxinfo.dt2.Rows.Count; j++)
                            {
                                dt2.ImportRow(taxinfo.dt2.Rows[j]);
                            }
                        }
                    }
                    else
                    {
                        if ((!object.ReferenceEquals(null, taxinfo.dt2) && taxinfo.dt2.Rows.Count > 0))
                        {
                            for (int j = 0; j < taxinfo.dt2.Rows.Count; j++)
                            {
                                dt4.ImportRow(taxinfo.dt2.Rows[j]);
                            }
                        }
                    }
                }
                else
                {

                    Msg += "Employee No. " + emplist[0].EmployeeCode + " Salary Not Mapped";
                }
            });

            TaxHistory txsave = new TaxHistory();


            if (dt1.Rows.Count > 0)
            {
                txsave.SaveAP(dt1);
            }

            if (dt2.Rows.Count > 0)
            {
                txsave.Import(dt2);
            }


            if (dt3.Rows.Count > 0)
            {
                txsave.SaveAPTemp(dt3);
            }

            if (dt4.Rows.Count > 0)
            {
                txsave.ImportTemp(dt4);
            }

            return Msg;
        }


        public JsonResult TdsStatement(Guid selectedId, int month, int year, Guid financeyearId, string type, bool AllCategory)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string outputfilePath = string.Empty;
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            DateTime applyDate = new DateTime(year, month, 1);
            if (AllCategory == false)
            {
                employeelist.RemoveAll(d => d.CategoryId != selectedId);

            }



            TaxHistoryList taxhistorylist = new TaxHistoryList(financeyearId, applyDate);
            TXFinanceYear tf = new TXFinanceYear(financeyearId, companyId);

            int projec = 0;
            DateTime sdate = tf.StartingDate;
            do
            {
                sdate = sdate.AddMonths(1);
                if (sdate < tf.EndingDate)
                {
                    projec++;
                }
            } while (sdate <= tf.EndingDate);

            List<tdsStatement> Ltds = new List<tdsStatement>();
            AttributeModelList attr = new AttributeModelList(companyId);
            TaxBehaviorList taxbehav = new TaxBehaviorList(financeyearId, Guid.Empty, Guid.Empty);

            if (taxhistorylist != null)
            {
                employeelist.ForEach(f =>
                {
                    // TaxHistoryList taxlist = new TaxHistoryList(financeyearId, f.Id, applyDate);
                    var taxlist = taxhistorylist.Where(th => th.FinanceYearId == financeyearId && th.EmployeeId == f.Id && th.ApplyDate == applyDate).ToList();
                    if (taxlist.Count() > 0)
                    {



                        TaxHistory thisMonth = taxlist.Where(w => w.Field.Contains("TOTTAXPERMONTH") && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory tottax = taxlist.Where(w => w.Field == "TOTTAX" && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory alPaid = taxlist.Where(w => w.Field == "TAXPAID" && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory balTax = taxlist.Where(w => w.Field == "TPR" && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory oneTax = taxlist.Where(w => w.Field == "ONETIMETAX" && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory perMon = taxlist.Where(w => w.Field.Contains("TDS to be deducted for") && w.EmployeeId == f.Id).FirstOrDefault();
                        TaxHistory remainingMonth = taxlist.Where(w => w.Field.Contains("TDS to be deducted for") && w.EmployeeId == f.Id).FirstOrDefault();

                        decimal? disMonth = thisMonth == null ? 0 : thisMonth.Total;
                        decimal? totalTax = tottax == null ? 0 : tottax.Total;
                        decimal? alreadPaid = alPaid == null ? 0 : alPaid.Total;
                        decimal? balancetax = balTax == null ? 0 : balTax.Total;
                        decimal? oneTimeTax = oneTax == null ? 0 : oneTax.Total;
                        decimal? perMonth = perMon == null ? 0 : perMon.Total;



                        List<rptWorkSheet> rpt = new List<rptWorkSheet>();
                        int age = taxlist.Where(W => W.Field == "EmployeeAge").FirstOrDefault() != null ? Convert.ToInt16(taxlist.Where(W => W.Field == "EmployeeAge").FirstOrDefault().Total) : 0;
                        //  TaxBehavior tx = new TaxBehaviorList(financeyearId, Guid.Empty, attr.Where(w => w.Name == (age < 60 ? "NRMALPERSON" : age >= 60 && age < 80 ? "SENIER" : "SUPERSINER")).FirstOrDefault().Id).FirstOrDefault();
                        Guid attrid = attr.Where(w => w.Name == (age < 60 ? "NRMALPERSON" : age >= 60 && age < 80 ? "SENIER" : "SUPERSINER")).FirstOrDefault().Id;
                        TaxBehavior tx1 = taxbehav.Where(tb => tb.FinanceYearId == financeyearId && tb.AttributemodelId == attrid).FirstOrDefault();
                        decimal? Basevalue = taxlist.Where(w => w.FieldId == new Guid(tx1.BaseFormula)).FirstOrDefault().Total;
                        SubSlabRange(tx1.Formula, Convert.ToDecimal(Basevalue), rpt);
                        string[] per = rpt.Last().Description.Split();
                        string percent = per.Where(p => p.Contains("%")).FirstOrDefault();
                        string[] remain = remainingMonth.Field.Split();

                        tdsStatement tds = new tdsStatement
                        {
                            Id = f.Id,
                            EmployeeCode = f.EmployeeCode,
                            EmployeeName = f.FirstName + f.LastName,
                            AlreadyDeducted = alreadPaid,
                            BalanceTax = balancetax,
                            OneTimeTax = oneTimeTax,
                            TotalTax = totalTax,
                            Permonth = perMonth,
                            NoOfMonths = Convert.ToInt16(remain[5]),
                            Thismonth = disMonth,
                            TaxPercentage = rpt.Count() == 1 ? "0%" : percent,
                            // TaxPercentage = rpt.Count() == 1 ? "0%" : per[5],
                        };
                        Ltds.Add(tds);
                    }

                });
            }

            if (Ltds.Count() > 0 && type == "Preview")
            {
                Company com = new Company(companyId);
                TXFinanceYear financeYear = new TXFinanceYear(financeyearId, companyId);
                List<string> rptparms = new List<string>();
                string compDetails = com.CompanyName.ToUpper() + "*" + com.CompanyAddress + "TDS STATEMENT";
                compDetails = compDetails + "*" + financeYear.StartingDate.Day + " " + financeYear.StartingDate.ToString("MMMM") + " " + financeYear.StartingDate.Year + " - " + financeYear.EndingDate.Day + " " + financeYear.EndingDate.ToString("MMMM") + " " + financeYear.EndingDate.Year;
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/" + (AllCategory == true ? "AllCategory" : employeelist[0].CategoryName) + ".xls";
                rptparms.Add(compDetails);
                generateTdsStatement(reportPath, Ltds, rptparms);
                outputfilePath = reportPath;
                return BuildJsonResult(true, 1, "File Saved Successfully", new { ltds = Ltds, filePath = outputfilePath, type = type });
            }


            return base.BuildJson(true, 200, "success", new { ltds = Ltds, filePath = outputfilePath, type = type });
        }

        public bool generateTdsStatement(string PDFFilePath, List<tdsStatement> dt, List<string> parameters)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rpt = viewer.LocalReport;
                rpt.Refresh();
                rpt.ReportPath = "Reports/tdsStatement.rdlc";

                ReportDataSource rptDs = new ReportDataSource("tdsStatement", dt);
                rpt.DataSources.Add(rptDs);
                ReportParameterCollection rpcollection = new ReportParameterCollection();

                rpcollection.Add(new ReportParameter("CompanyDetails", parameters[0]));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;

            }

        }



        public JsonResult GetTaxHistory(Guid selectedId, int month, int year, string type, Guid financeyearId, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            DateTime applyDate = new DateTime(year, month, 1);



            //foreach (object id in CategoryIds)
            //{
            //    employeelist.AddRange(new EmployeeList(companyId, new Guid(id.ToString()), userId));
            //}

            List<jsonPayroll> entity = new List<jsonPayroll>();



            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            TaxHistoryList taxhistorylist = new TaxHistoryList();

            /*when login is employee fetching of table is changed to temp table */


            if (processtype == "employee")
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate, emproll);
            }
            else
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate);
            }

            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int curyymm = Convert.ToInt32(year) * 100 + Convert.ToInt32(month);
            emplist.ForEach(u =>
            {
                var tmp = taxhistorylist.Where(s => s.EmployeeId == u.Id).FirstOrDefault();
                string status = string.Empty;
                Guid payrollId = Guid.Empty;
                bool canAdd = false;
                if (!object.ReferenceEquals(tmp, null))//alerady processed
                {
                    canAdd = true;
                    status = "Processed";
                    payrollId = tmp.Id;

                }
                else
                {
                    if (u.DateOfJoining <= CurrPayrollmonth && u.PayrollProcess == true && u.Status == 1)
                    {
                        canAdd = true;
                        status = "Not Process";
                    }

                    if (u.SeparationDate != DateTime.MinValue && curyymm <= (Convert.ToInt32(u.SeparationDate.Year) * 100 + Convert.ToInt32(u.SeparationDate.Month)))
                    {
                        canAdd = true;
                        status = "Not Process";
                    }
                }

                if (canAdd)
                {
                    entity.Add(new jsonPayroll()
                    {
                        employeeCode = u.EmployeeCode,
                        employeeId = u.Id,
                        employeeName = u.FirstName + " " + u.LastName,
                        year = year,
                        month = month,
                        status = status,
                        payrollId = payrollId
                    });

                }
            });



            return base.BuildJson(true, 200, "success", entity);
        }
        public ActionResult TaxWorksheet(Guid selectedId, int month, int year, string type, Guid financeyearId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            DateTime applyDate = new DateTime(year, month, 1);

            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            TaxHistoryList taxhistorylist = new TaxHistoryList(financeyearId, applyDate);
            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            TXSectionList secList = new TXSectionList(companyId, financeyearId);
            string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
            string outputfilePath = string.Empty;
            Company company = new Company(companyId, userId);

            string pdfPath;
            emplist.ForEach(emp =>
            {
                pdfPath = string.Empty;
                List<string> rptparms = new List<string>();
                List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                pdfPath = reportPath + month.ToString() + "_" + year.ToString() + "_" + emp.EmployeeCode + ".pdf";
                var tmpHist = taxhistorylist.Where(s => s.EmployeeId == emp.Id).ToList();
                EmployeeAddress addr = emp.EmployeeAddressList.Where(a => a.AddressType == 1).FirstOrDefault();
                foreach (TaxHistory hist in tmpHist)
                {
                    rptWorkSheet worksheet = new rptWorkSheet();
                    worksheet.EmpCode = emp.EmployeeCode;
                    if (hist.FieldType != "ITAX")
                    {
                        TXSection txsec = secList.Where(s => s.Id == hist.FieldId).FirstOrDefault();
                        worksheet.Description = txsec.DisplayAs;
                        worksheet.Actual = hist.Actual;
                        worksheet.Projection = hist.Projection;
                        worksheet.Total = hist.Total;
                        worksheetList.Add(worksheet);
                    }

                }

                string empAddress = !string.IsNullOrEmpty(addr.AddressLine1) ? addr.AddressLine1 + (!string.IsNullOrEmpty(addr.AddressLine2) ? "*" + addr.AddressLine2 : "")
                                 : !string.IsNullOrEmpty(addr.AddressLine2) ? addr.AddressLine2 + "*" : "";
                empAddress = !string.IsNullOrEmpty(empAddress) ? addr.City + "*" : "";
                empAddress = emp.FirstName + "*" + empAddress;

                rptparms.Add(emp.EmployeeCode + "*" + emp.Designation);
                rptparms.Add(empAddress);
                rptparms.Add(("WORKSHEET FOR THE MONTH OF " + (MonthEnum)month + " " + year).ToUpper());
                generateworksheet(pdfPath, worksheetList, rptparms);
                outputfilePath = pdfPath;
            });//end emp


            if (employeelist.Count() > 0)
            {
                if (employeelist.Count() > 1)
                {

                    outputfilePath = DocumentProcessingSettings.TempDirectoryPath + "worksheet.zip";
                    ZipPath(outputfilePath, reportPath, null, true, null);

                }

            }
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outputfilePath });
        }

        public JsonResult GetWorksheetColumn(Guid financialyearId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            List<jsonWorksheetColumn> litlwsc = new List<jsonWorksheetColumn>();
            List<AttributeModel> attModList = new AttributeModelList(companyId).Where(w => w.IsTaxable == true && w.BehaviorType == "Earning").ToList();
            TXSectionList txSectionlist = new TXSectionList(companyId, financialyearId);
            List<TaxBehavior> txBehavior = new TaxBehaviorList(financialyearId, Guid.Empty, Guid.Empty).Where(w => w.FieldFor.ToUpper() == "TAX").ToList();

            List<AttributeModel> attlst = new AttributeModelList(companyId).ToList();
            jsonWorksheetColumn jwscExtra = new jsonWorksheetColumn();
            var parentsection = txSectionlist.Where(x => x.ParentId == Guid.Empty && x.SectionType != "Others").ToList().OrderBy(y => y.OrderNo).ToList();
            string[] apt = { "Actual.", "Projection.", "Total." };
            attModList.ForEach(n =>
            {
                apt.ToList().ForEach(f =>
                {
                    jsonWorksheetColumn jwsc = new jsonWorksheetColumn();
                    jwsc.Id = n.Id;
                    jwsc.Description = f + n.DisplayAs;
                    jwsc.Section = n.BehaviorType;
                    jwsc.Order = 1;
                    litlwsc.Add(jwsc);
                });
            });
            apt.ToList().ForEach(f =>
            {
                litlwsc.Add(jwscExtra.grossSalary(f, companyId));
            });
            apt.ToList().ForEach(f =>
            {
                litlwsc.Add(jwscExtra.totalEarning(f, companyId));
            });


            parentsection.ForEach(s =>
            {
                txSectionlist.Where(w => w.SectionType != "Others").ToList().ForEach(f =>
               {
                   if (s.Id == f.Id || s.Id == f.ParentId)
                   {
                       jsonWorksheetColumn jwsc = new jsonWorksheetColumn();
                       jwsc.Id = f.Id;
                       jwsc.Description = f.ParentId == Guid.Empty ? "Total " + f.DisplayAs : f.DisplayAs;
                       jwsc.Section = f.ParentId == Guid.Empty && f.SectionType == "Others" ? f.SectionType : (f.ParentId != Guid.Empty ? txSectionlist.Find(j => j.Id == f.ParentId).DisplayAs : f.DisplayAs);
                       jwsc.Order = 2;
                       litlwsc.Add(jwsc);
                   }
               });
            });

            txSectionlist.Where(w => w.SectionType == "Others").ToList().ForEach(f =>
            {
                if (f.IncomeTypeId == 1 || f.IncomeTypeId == 2 || f.IncomeTypeId == 3)
                {
                    string[] ap = { "Actual.", "Projection.", "Total." };
                    ap.ToList().ForEach(j =>
                    {

                        jsonWorksheetColumn jwsc = new jsonWorksheetColumn();
                        jwsc.Id = f.Id;
                        jwsc.Description = j + f.DisplayAs;
                        jwsc.Section = f.ParentId == Guid.Empty && f.SectionType == "Others" ? f.SectionType : (f.ParentId != Guid.Empty ? txSectionlist.Find(l => l.Id == f.ParentId).DisplayAs : f.DisplayAs);
                        jwsc.Order = 3;
                        litlwsc.Add(jwsc);

                    });

                }
                else
                {
                    jsonWorksheetColumn jwsc = new jsonWorksheetColumn();
                    jwsc.Id = f.Id;
                    jwsc.Description = f.DisplayAs;
                    jwsc.Section = f.ParentId == Guid.Empty && f.SectionType == "Others" ? f.SectionType : (f.ParentId != Guid.Empty ? txSectionlist.Find(j => j.Id == f.ParentId).DisplayAs : f.DisplayAs);
                    jwsc.Order = 3;
                    litlwsc.Add(jwsc);
                }

            });
            jwscExtra.aftertotal(txSectionlist, litlwsc);
            txBehavior.ForEach(f =>
            {
                jsonWorksheetColumn jwsc = new jsonWorksheetColumn();
                jwsc.Id = f.AttributemodelId;
                jwsc.Description = attlst.Where(a => a.Id == f.AttributemodelId).FirstOrDefault().DisplayAs;
                jwsc.Section = "Taxable";
                jwsc.Order = 3;
                litlwsc.Add(jwsc);
            });

            jwscExtra.tsdToBeDeducted(litlwsc, companyId);

            return base.BuildJson(true, 200, "success", litlwsc);
        }
        public bool generaXLWorkSheet(List<TaxHistoryReport> dt, string pdfPath)
        {

            try
            {

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rpt = viewer.LocalReport;
                rpt.Refresh();
                rpt.ReportPath = "Reports/WorksheetExcelReport.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DsDeclare", dt);
                rpt.DataSources.Add(rptDs);
                //ReportParameterCollection rpcollection = new ReportParameterCollection();
                //rpcollection.Add(new ReportParameter("CompanyDetails", ""));
                //rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(pdfPath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;

            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }


        }
        public bool worksheetAP(string PDFFilePath, List<TaxHistoryAP> dt, List<string> parameters)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rpt = viewer.LocalReport;
                rpt.Refresh();
                rpt.ReportPath = "Reports/TdsActualProjection.rdlc";

                ReportDataSource rptDs = new ReportDataSource("taxhistoryap", dt);
                rpt.DataSources.Add(rptDs);
                ReportParameterCollection rpcollection = new ReportParameterCollection();

                rpcollection.Add(new ReportParameter("CompanyAddress", parameters[1]));
                rpcollection.Add(new ReportParameter("EmployeeAddress", parameters[0]));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;

            }

        }
        public bool generateworksheet(string PDFFilePath, List<rptWorkSheet> dt, List<string> parameters)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rpt = viewer.LocalReport;
                rpt.Refresh();
                rpt.ReportPath = "Reports/ITaxWorkSheet.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DSWorkSheet", dt);
                rpt.DataSources.Add(rptDs);
                ReportParameterCollection rpcollection = new ReportParameterCollection();
                rpcollection.Add(new ReportParameter("Month", parameters[2]));
                rpcollection.Add(new ReportParameter("CompanyAddress", parameters[1]));
                rpcollection.Add(new ReportParameter("EmployeeAddress", parameters[0]));
                rpcollection.Add(new ReportParameter("Sheethead", parameters[3]));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;

            }

        }
        public JsonResult GetWorksheetXL(string categories, int month, int year, string empCode, Guid financeyearId, column data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            DateTime applyDate = new DateTime(year, month, 1);
            TaxHistoryReportList txListReport = new TaxHistoryReportList(financeyearId, companyId, applyDate);

            for (int i = 0; i < data.listfield.Count; i++)
            {
                if (i == 120)
                {

                }
                string display = data.listfield[i].Substring(data.listfield[i].IndexOf('.') + 1);




                TaxHistoryReport byField = null;
                if (data.listid[i] == Guid.Empty)
                {
                    byField = txListReport.Where(w => w.Field.Trim().Contains(display.Trim()) && (w.FieldType == "Income" || w.FieldType == "SubSection" || w.FieldType == "grosssec" || w.FieldType == "TAX")).FirstOrDefault();


                    if (!object.ReferenceEquals(byField, null))
                    {
                        if (byField.FieldType == "grosssec")
                        {
                            byField.totallDisplay = "show";
                        }
                        else if (byField.FieldType == "SubSection")
                        {
                            byField.actualDisplay = "show";
                        }
                        else if (byField.FieldType == "TAX")
                        {
                            byField.totallDisplay = "show";
                        }
                        else if (data.listfield[i].Contains("Actual."))
                        {
                            byField.actualDisplay = "show";
                        }
                        else if (data.listfield[i].Contains("Projection."))
                        {
                            byField.projectionDisplay = "show";

                        }
                        else if (data.listfield[i].Contains("Total."))
                        {
                            byField.totallDisplay = "show";

                        }

                    }


                }

                if (data.listid[i] != Guid.Empty)
                {
                    TaxHistoryReport byId = txListReport.Where(w => w.Fieldid == data.listid[i]).FirstOrDefault();

                    if (!object.ReferenceEquals(byId, null))
                    {

                        if (data.listfield[i].Contains("Actual."))
                        {
                            byId.actualDisplay = "show";
                        }
                        else if (data.listfield[i].Contains("Projection."))
                        {
                            byId.projectionDisplay = "show";

                        }
                        else if (data.listfield[i].Contains("Total."))
                        {
                            byId.totallDisplay = "show";

                        }
                        else if (byId.FieldType == "Section")
                        {
                            byId.totallDisplay = "show";
                        }
                        else if (byId.FieldType == "grosssec")
                        {
                            byId.totallDisplay = "show";
                        }
                        else if (byId.FieldType == "SubSection" || byId.FieldType == "OtherIncomes")
                        {
                            txListReport.Where(w => w.Fieldid == data.listid[i]).ToList().ForEach(f =>
                            {
                                f.Field = f.Field.Trim();
                            });

                            byId.actualDisplay = "show";
                        }
                        else if (byId.FieldType == "TAX" || byId.FieldType == "ONETIMETAX" || byId.FieldType == "Total Income")
                        {
                            byId.totallDisplay = "show";
                        }
                    }
                }

            }



            txListReport.RemoveAll(r => r.FieldType == "TAX" && r.ThirdOrder == null);



            string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
            string pdfPath = string.Empty;
            string outputfilePath = string.Empty;
            pdfPath = reportPath + "taxxl_" + ".xls";

            generaXLWorkSheet(txListReport, pdfPath);
            outputfilePath = pdfPath;
            return base.BuildJson(true, 200, "success", outputfilePath);
        }
        public JsonResult DeleteWorksheet(string path)
        {


            if (path != null)
            {
                System.IO.File.Delete(Path.GetFileName(path));
                string filename = System.IO.Path.GetFileName(path);
                System.IO.File.Delete(filename);
                string deldir = path.Remove(path.Length - (filename.Length + 1));
                var dir = new DirectoryInfo(deldir);
                dir.Delete(true);
            }



            return base.BuildJson(true, 200, "success", null);
        }
        public JsonResult GetEmployeeLoginWorksheet(int month, int year, string empCode, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            TXFinanceYearList tx = new TXFinanceYearList(companyId);
            DateTime applyDate = new DateTime(year, month, 1);
            Guid fin = Guid.Empty;
            tx.ForEach(f =>
            {
                if (applyDate >= f.StartingDate && applyDate < f.EndingDate)
                {
                    fin = f.Id;
                }

            });

            string actual = "YES";
            return GetWorksheet("", month, year, empCode, fin, actual, processtype, null);
        }
        public JsonResult GetWorksheetAP(string categories, int month, int year, string empCode, Guid financeyearId, object datum, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            EmployeeList emplist = new EmployeeList();
            IncomeMatchingList imatch = new IncomeMatchingList();
            DateTime applyDate = new DateTime(year, month, 1);
            imatch.AddRange(new IncomeMatchingList(financeyearId).ToList());
            TXFinanceYear txf = new TXFinanceYear(financeyearId, companyId);
            AttributeModelList attrlist = new AttributeModelList(companyId);

            TaxHistoryList taxhistorylist = new TaxHistoryList();

            /*here it will search temporary table if role is employee */

            if (processtype == "employee")
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate, emproll);
            }
            else
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate);
            }

            string pdfPath = string.Empty;
            List<string> rptparms = new List<string>();

            int ProjectedMonth = 0;

            DateTime sdate = applyDate;
            do
            {
                sdate = sdate.AddMonths(1);
                if (sdate < txf.EndingDate)
                {
                    ProjectedMonth++;
                }
            } while (sdate <= txf.EndingDate);



            if (empCode != "")//single employee
            {
                emplist.AddRange(employeelist.Where(e => e.EmployeeCode == empCode).ToList());
            }
            emplist.ForEach(f =>
            {
                var tmpHist = taxhistorylist.Where(s => s.EmployeeId == f.Id).ToList();
                TaxHistoryList taxhistorylis = new TaxHistoryList();

                /*here it will search temporary table if role is employee */

                if (processtype == "employee")
                {
                    taxhistorylis = new TaxHistoryList(financeyearId, f.Id, applyDate, "", emproll);
                }
                else
                {
                    taxhistorylis = new TaxHistoryList(financeyearId, f.Id, applyDate, "");
                }
                TaxHistoryAPList txlist = new TaxHistoryAPList();
                string[] mOrder = { "J", "K", "L", "A", "B", "C", "D", "E", "F", "G", "H", "I" };
                DateTime date = txf.StartingDate;
                for (int i = 0; i < 12 - ProjectedMonth; i++)
                {
                    imatch.ForEach(m =>
                    {
                        var projec = taxhistorylis.Where(w => w.FieldId == m.AttributemodelId && w.ActualMonth == date.Month).FirstOrDefault();
                        if (!object.ReferenceEquals(projec, null))
                        {
                            txlist.Add(new TaxHistoryAP
                            {
                                // Field = new AttributeModel(m.AttributemodelId, companyId).DisplayAs,
                                Field = attrlist.Where(am => am.Id == m.AttributemodelId).FirstOrDefault().DisplayAs,
                                Month = Enum.GetName(typeof(MonthSmallEnum), date.Month),
                                Value = Convert.ToDecimal(projec.Actual),
                                Order = mOrder[date.Month - 1]
                            });
                        }
                        else
                        {

                            txlist.Add(new TaxHistoryAP
                            {
                                // Field = new AttributeModel(m.AttributemodelId, companyId).DisplayAs,
                                Field = attrlist.Where(am => am.Id == m.AttributemodelId).FirstOrDefault().DisplayAs,
                                Month = Enum.GetName(typeof(MonthSmallEnum), date.Month),
                                Value = Convert.ToDecimal(0.00M),
                                Order = mOrder[date.Month - 1]
                            });
                        }

                    });
                    date = date.AddMonths(1);
                }





                for (int i = 0; i < ProjectedMonth; i++)
                {
                    imatch.ForEach(m =>
                    {
                        var projec = tmpHist.Where(w => w.FieldId == m.AttributemodelId).FirstOrDefault();
                        if (!object.ReferenceEquals(projec, null))
                        {
                            txlist.Add(new TaxHistoryAP
                            {
                                // Field = new AttributeModel(m.AttributemodelId, companyId).DisplayAs,
                                Field = attrlist.Where(am => am.Id == m.AttributemodelId).FirstOrDefault().DisplayAs,
                                Month = Enum.GetName(typeof(MonthSmallEnum), date.Month),
                                Value = Decimal.Parse(Convert.ToDecimal(projec.Projection / ProjectedMonth).ToString("0.00")),
                                Order = mOrder[date.Month - 1]
                            });
                        }
                        else
                        {
                            txlist.Add(new TaxHistoryAP
                            {
                                // Field = new AttributeModel(m.AttributemodelId, companyId).DisplayAs,
                                Field = attrlist.Where(am => am.Id == m.AttributemodelId).FirstOrDefault().DisplayAs,
                                Month = Enum.GetName(typeof(MonthSmallEnum), date.Month),
                                Value = Convert.ToDecimal(0.00M),
                                Order = mOrder[date.Month - 1]
                            });
                        }

                    });
                    date = date.AddMonths(1);

                }
                Company company = new Company(companyId);

                rptparms.Add(f.EmployeeCode + "*" + f.FirstName + " " + "*" + f.DesignationName);
                string companyAddress = company.CompanyName + "*" + company.AddressLine1 + "*" + company.AddressLine2 + "*";
                companyAddress = companyAddress + company.City + " ," + company.State;

                rptparms.Add(companyAddress);
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
                pdfPath = reportPath + month.ToString() + "_" + year.ToString() + "_" + f.EmployeeCode + ".pdf";
                worksheetAP(pdfPath, txlist, rptparms);

            });






            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = pdfPath });
        }


        public JsonResult GetWorksheet(string categories, int month, int year, string empCode, Guid financeyearId, string actual, object datum, string processtype, bool IsCurrentmonth = true)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            DateTime applyDate = new DateTime(year, month, 1);
            TXFinanceYear txFin = new TXFinanceYear(financeyearId, companyId);
            DateTime lastdate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            EmployeeList emplist = new EmployeeList();

            if (empCode != "")//single employee
            {
                emplist.AddRange(employeelist.Where(e => e.EmployeeCode == empCode).ToList());
            }
            else if (categories != null)//by category
            {
                string[] category = categories.TrimEnd(',').Split(',');
                foreach (string cat in category)
                {
                    emplist.AddRange(employeelist.Where(e => e.CategoryId == new Guid(cat)).ToList());
                }
            }

            TaxHistoryList taxhistorylist = new TaxHistoryList();
            if (processtype != "employee")
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate);
            }
            else
            {
                taxhistorylist = new TaxHistoryList(financeyearId, applyDate, emproll);
            }

            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            TXSectionList secList = new TXSectionList(companyId, financeyearId);
            string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
            string outputfilePath = string.Empty;
            Company company = new Company(companyId, userId);
            List<AttributeModel> attlst = new AttributeModelList(companyId).ToList();
            AttributeModel attr1 = attlst.Find(t => t.Name.ToUpper() == "NEWTAXSCHEME");
            string pdfPath;
            List<rptWorkSheet> worksheetLis = new List<rptWorkSheet>();
            emplist.ForEach(emp =>
            {
                string newtaxscheme = string.Empty;
                TXEmployeeSectionList declaration1;
                TXEmployeeSection declaration;
                DateTime edate1 = applyDate;
                if (!object.ReferenceEquals(attr1, null))
                {
                    declaration1 = new TXEmployeeSectionList(emp.Id, edate1);
                    declaration = declaration1.Where(ds => ds.SectionId == attr1.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(declaration, null))
                    {
                        if (declaration.DeclaredValue == "1")
                        {
                            newtaxscheme = "1";
                        }
                    }
                }

                PayrollHistory payhis = new PayrollHistory(companyId, emp.Id, year, month);
                emp = PayrollTransaction.GetEmployeeTrasaction("Employee", DateTime.Now.ToShortDateString(), (dynamic)emp, payhis.Id);
                var tmpHist = taxhistorylist.Where(s => s.EmployeeId == emp.Id).ToList();

                if (tmpHist.Count() > 0)
                {
                    EmployeePastDataList empPast = new EmployeePastDataList(emp.Id, companyId);
                    if (empPast.Count > 0)
                    {
                        DateTime date = tmpHist.FirstOrDefault().ModifiedOn == DateTime.MinValue ? tmpHist.FirstOrDefault().CreatedOn : tmpHist.FirstOrDefault().ModifiedOn;
                        EmployeePastData empd = empPast.Where(w => w.FromDate < date && w.ToDate > date).FirstOrDefault();
                        if (!Object.ReferenceEquals(empd, null))
                        {
                            emp.EmployeeCode = empd.EmployeeCode;
                            if (!string.IsNullOrEmpty(empCode))
                            {
                                empCode = empd.EmployeeCode;
                            }
                        }


                    }
                    pdfPath = string.Empty;
                    List<string> rptparms = new List<string>();
                    List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                    List<rptWorkSheet> temp = new List<rptWorkSheet>();
                    pdfPath = reportPath + month.ToString() + "_" + year.ToString() + "_" + emp.EmployeeCode + ".pdf";
                    EmployeeAddress addr = emp.EmployeeAddressList.Where(a => a.AddressType == 1).FirstOrDefault();
                    foreach (TaxHistory hist in tmpHist)
                    {


                        decimal act = Convert.ToDecimal(hist.Actual);
                        decimal pro = Convert.ToDecimal(hist.Projection);
                        decimal tot = Convert.ToDecimal(hist.Total);



                        rptWorkSheet worksheet = new rptWorkSheet();
                        worksheet.EmpCode = emp.EmployeeCode;

                        if (hist.FieldType.ToLower() == "subsection" || hist.FieldType.ToLower() == "otherincomes")
                        {
                            if ((act != 0 || pro != 0 || tot != 0 || hist.Field == "House Rent Allowance") || hist.Field.Contains("TDS to be deducted for"))
                            {
                                bool hra = true;

                                if (hist.Field == "House Rent Allowance")
                                {


                                    TXEmployeeSection txEmpSec = new TXEmployeeSectionList(emp.Id, applyDate).Where(v => v.SectionId == new AttributeModelList(companyId).Where(w => w.Name == "ARP").FirstOrDefault().Id).FirstOrDefault();
                                    if (object.ReferenceEquals(txEmpSec, null))
                                    {
                                        hra = false;
                                    }
                                }
                                TXSection txsec = secList.Where(s => s.Id == hist.FieldId).FirstOrDefault();
                                TXSection ptxsec = secList.Where(s => s.Id == txsec.ParentId).FirstOrDefault();
                                if (txsec != null && hra == true)
                                {
                                    worksheet.fieldId = hist.FieldId;
                                    worksheet.Description = txsec.DisplayAs;
                                    worksheet.Actual = hist.Actual;
                                    worksheet.Projection = hist.Projection;
                                    worksheet.Total = hist.Total;
                                    worksheet.ParentSection = ptxsec == null ? "Any other income" : ptxsec.DisplayAs;
                                    worksheet.Type = hist.FieldType.ToLower() == "otherincomes" ? "SubSection" : hist.FieldType;
                                    worksheet.OtherIncomeType = hist.FieldType.ToLower() == "otherincomes" ? 1 : 0;
                                    worksheet.Order = "C";
                                    worksheet.OrderG = "";
                                    worksheet.OrderSec = "A";
                                    worksheet.FormulaType = txsec.FormulaType;
                                    worksheetList.Add(worksheet);
                                }
                            }
                        }
                        else if (hist.FieldType.ToLower() == "section")
                        {

                            TXSection txse = secList.Where(s => s.Id == hist.FieldId).FirstOrDefault();

                            if (txse != null)
                            {
                                worksheet.fieldId = hist.FieldId;
                                worksheet.Description = "Total " + txse.DisplayAs;
                                worksheet.Actual = hist.Actual;
                                worksheet.Projection = hist.Projection;
                                worksheet.Total = hist.Total;
                                worksheet.ParentSection = txse.DisplayAs;
                                worksheet.Type = "SubSection";
                                worksheet.Order = "C";
                                worksheet.OrderG = "";
                                worksheet.OrderSec = "C";
                                worksheetList.Add(worksheet);

                            }

                        }
                        else if (hist.FieldType.ToLower() == "grosssec")
                        {
                            var parsec = tmpHist.Where(t => t.Field == hist.Field && t.FieldType.ToLower() == "section").FirstOrDefault();
                            if (parsec != null)
                            {
                                TXSection txse = secList.Where(s => s.Id == parsec.FieldId).FirstOrDefault();
                                if (txse != null)
                                {
                                    worksheet.fieldId = hist.FieldId;
                                    worksheet.Description = "";
                                    worksheet.Projection = hist.Projection;
                                    worksheet.Total = hist.Total;
                                    worksheet.ParentSection = txse.DisplayAs;
                                    worksheet.Type = "SubSection";
                                    worksheet.Order = "C";
                                    worksheet.OrderG = "";
                                    worksheet.OrderSec = "D";
                                    worksheetList.Add(worksheet);
                                }
                            }
                            else if (hist.Field == "otherincometotal")
                            {
                                worksheet.fieldId = hist.FieldId;
                                worksheet.Description = "";
                                worksheet.Projection = hist.Projection;
                                worksheet.Total = hist.Total;
                                worksheet.ParentSection = "Any other income";
                                worksheet.Type = "SubSection";
                                worksheet.Order = "C";
                                worksheet.OrderG = "";
                                worksheet.OrderSec = "D";
                                worksheetList.Add(worksheet);
                            }



                        }
                        else if (hist.FieldType.ToLower() == "income")
                        {
                            if ((act != 0 || pro != 0 || tot != 0 || hist.Field.Contains("TDS to be deducted for")))
                            {
                                TXSection txsec = secList.Where(s => s.Id == hist.FieldId).FirstOrDefault();
                                if (txsec != null)
                                {
                                    if (hist.FieldType.ToLower() == "income" && txsec.SectionType == "Others")
                                    {
                                        worksheet.fieldId = hist.FieldId;
                                        worksheet.Description = hist.Field;
                                        worksheet.Actual = hist.Actual;
                                        worksheet.Projection = hist.Projection;
                                        worksheet.Total = hist.Total;
                                        worksheet.Type = hist.FieldType;
                                        worksheet.Order = "A";
                                        worksheet.OrderSec = "C";
                                        worksheetList.Add(worksheet);
                                    }
                                    else
                                    {
                                        worksheet.fieldId = hist.FieldId;
                                        worksheet.Description = hist.Field;
                                        worksheet.Actual = hist.Actual;
                                        worksheet.Projection = hist.Projection;
                                        worksheet.Total = hist.Total;
                                        worksheet.Type = hist.FieldType;
                                        worksheet.Order = "A";
                                        worksheet.OrderSec = hist.Field == "Gross Salary" ? "D" : hist.Field == "Total Earning" ? "B" : "A";
                                        worksheetList.Add(worksheet);
                                    }
                                }
                                else
                                {
                                    worksheet.fieldId = hist.FieldId;
                                    worksheet.Description = hist.Field;
                                    worksheet.Actual = hist.Actual;
                                    worksheet.Projection = hist.Projection;
                                    worksheet.Total = hist.Total;
                                    worksheet.Type = hist.FieldType;
                                    worksheet.Order = "A";
                                    worksheet.OrderSec = hist.Field == "Gross Salary" ? "D" : hist.Field == "Total Earning" ? "B" : "A";
                                    worksheetList.Add(worksheet);
                                }
                            }
                        }

                        else if (hist.FieldType.ToLower() != "itax")
                        {
                            if ((act != 0 || pro != 0 || tot != 0 || hist.Field == "TAXPAID" || hist.Field == "TOTTAX" || hist.Field == "TPR" || hist.Field == "TOTTAXPERMONTH" || hist.Field == "Total Income(Round By 10 Rupess)" || hist.Field.Contains("TDS to be deducted for")))
                            {
                                if (hist.Field == "Total Income(Round By 10 Rupess)")
                                {
                                    hist.Total = hist.Total > 0 ? hist.Total : 0;
                                }
                                worksheet.fieldId = hist.FieldId;
                                worksheet.Description = hist.Field;
                                worksheet.Actual = hist.Actual;
                                worksheet.Projection = hist.Projection;
                                worksheet.Total = hist.Total;
                                worksheet.Type = "Tax";

                                worksheet.Order = (hist.FieldType.ToLower() == "income" && hist.Field.ToLower().Trim() == "gross salary") ? "B" : hist.FieldType.ToLower() == "income" ? "A" : hist.FieldType.ToLower().Trim() == "otherincomes" ? "D" : hist.FieldType.ToLower().Trim() == "total income" ? "E" : "G";

                                worksheetList.Add(worksheet);
                            }


                        }

                    }
                    //string[] y = { "", "", "", "", "", "Section 10 Exemptions", "Section 16", "", "Section 80CCE: 80C, 80CCC,80-CCD(1)", "Under Section 80CCD", "Medical insurance premium (Mediclaim)  (80D)", "Other Deductible amounts under Chapter VI A" };

                    //for (int j = 5; j < y.Length; j++)
                    //{
                    //    string h = ConvertAlpha(j);
                    //    worksheetList.Where(p => p.Order == "C" && p.OrderSec == "C" && p.ParentSection != "otherincometotal" && p.ParentSection.Trim() == y[j]).ToList().ForEach(f=>f.OrderG = h);
                    //    if (y[j] == "Section 16")
                    //    {
                    //        j++;
                    //        string Q = ConvertAlpha(j);
                    //        var inc = worksheetList.Where(w => w.Order == "C" && w.OrderSec == "C" && w.ParentSection == "otherincometotal").FirstOrDefault();
                    //        worksheetList.Where(k => k.ParentSection == inc.ParentSection).ToList().ForEach(o=>o.OrderG = Q);
                    //    }
                    //}


                    string[] y = { "Section 10 Exemptions", "Section 16", "Section 80CCE: 80C, 80CCC,80-CCD(1)", "Under Section 80CCD", "Medical insurance premium (Mediclaim)  (80D)", "Other Deductible amounts under Chapter VI A" };

                    for (int j = 0; j < y.Length; j++)
                    {
                        var data = worksheetList.Where(p => p.Order == "C" && p.OrderSec == "C" && p.ParentSection != "Any other income" && p.ParentSection.ToLower().Trim() == y[j].ToLower().Trim()).FirstOrDefault();
                        if (data != null)
                        {
                            worksheetList.Where(p => p.Order == "C" && p.OrderSec == "C" && p.ParentSection != "Any other income" && p.ParentSection.ToLower().Trim() == y[j].ToLower().Trim()).FirstOrDefault().numOrderG = j;
                        }
                    }


                    var ag = worksheetList.Where(p => p.Order == "C" && p.OrderSec == "C" && p.ParentSection != "Any other income").ToList().OrderBy(o => o.numOrderG).ToList();
                    int i = 5;
                    ag.ForEach(f =>
                    {
                        i++;
                        string p = ConvertAlpha(i);
                        worksheetList.ForEach(j =>
                        {
                            if (j.ParentSection == f.ParentSection)
                            {
                                TXSection txs = secList.Where(w => w.DisplayAs.Trim() == j.Description.Trim()).FirstOrDefault();
                                if (!object.ReferenceEquals(txs, null))
                                {
                                    j.OrderSubSec = ConvertAlpha(txs.OrderNo);
                                }
                                j.OrderG = p;
                                // Adding sub list for house rent allowance
                                if (j.Description == "House Rent Allowance")
                                {
                                    j.OrderSec = "B";
                                    j.OrderSubSec = null;

                                    var hraGroup = tmpHist.Where(w => w.Field.Contains("ABS")).GroupBy(u => u.Field).ToList();
                                    var hra = new TaxHistoryList();
                                    hraGroup.ForEach(m =>
                                    {

                                        hra.Add(tmpHist.Where(w => w.Field == m.Key).FirstOrDefault());

                                    });

                                    if (hra.Count > 0)
                                    {
                                        string metro = "  * 50% of Basic For Metro/40% of Basic For Non Metro";
                                        string[] order = { metro, "  * HRA Received", "  * Excess of Rent Paid Over 10% of Salary" };
                                        rptWorkSheet rw = new rptWorkSheet();
                                        rw.Description = "Least Of The Following";
                                        rw.FormulaType = 9;
                                        rw.Order = "C";
                                        rw.OrderG = p;
                                        rw.OrderSec = "B";
                                        rw.Total = null;
                                        rw.ParentSection = "Section 10 Exemptions";
                                        rw.Type = "SubSection";
                                        temp.Add(rw);

                                        hra.ForEach(s =>
                                        {
                                            AttributeModel att = new AttributeModel(s.FieldId, companyId);

                                            rptWorkSheet rws = new rptWorkSheet();
                                            rws.Description = s.Field.Contains("MNM") ? order[0] : s.Field.Contains("EHRA") ? order[1] : order[2];
                                            rws.FormulaType = 9;
                                            rws.Order = "C";
                                            rws.OrderG = p;
                                            rws.OrderSec = "B";                         
                                            rws.Total = s.Total;
                                            rws.ParentSection = "Section 10 Exemptions";
                                            rws.Type = "SubSection";
                                            temp.Add(rws);

                                        });

                                    }
                                }
                            }
                        });
                        if (f.ParentSection == "Section 16 ")
                        {
                            i++;
                            string Q = ConvertAlpha(i);
                            var inc = worksheetList.Where(w => w.Order == "C" && w.OrderSec == "D" && w.ParentSection == "Any other income").FirstOrDefault();
                            worksheetList.Where(k => k.ParentSection == inc.ParentSection).FirstOrDefault().OrderG = Q;

                        }
                        //worksheetList.Where(w => w.ParentSection == f.ParentSection).ToList().Select(s => s.Order.Replace(" ", p));

                    });
                    temp.ForEach(f =>
                    {
                        worksheetList.Add(f);
                    });
                    AttributeModelList attr = new AttributeModelList(companyId);
                    attr.Where(a => a.BehaviorType == "Tax").ToList().ForEach(f =>
                    {
                        if (f.Name == "TOTITAX")
                        {
                            if (newtaxscheme == "1")
                            {
                                int age = tmpHist.Where(W => W.Field == "EmployeeAge").FirstOrDefault() != null ? Convert.ToInt16(tmpHist.Where(W => W.Field == "EmployeeAge").FirstOrDefault().Total) : 0;
                                TaxBehavior tx = new TaxBehaviorList(financeyearId, Guid.Empty, attr.Where(w => w.Name == (age < 60 ? "NEWNRMALPERSON" : age >= 60 && age < 80 ? "NEWSINERPER" : "NEWSUPSEN")).FirstOrDefault().Id).FirstOrDefault();

                                decimal? Basevalue = worksheetList.Where(w => w.fieldId == new Guid(tx.BaseFormula)).FirstOrDefault().Total;

                                SubSlabRange(tx.Formula, Convert.ToDecimal(Basevalue), worksheetList);
                            }
                            else
                            {
                                int age = tmpHist.Where(W => W.Field == "EmployeeAge").FirstOrDefault() != null ? Convert.ToInt16(tmpHist.Where(W => W.Field == "EmployeeAge").FirstOrDefault().Total) : 0;
                                TaxBehavior tx = new TaxBehaviorList(financeyearId, Guid.Empty, attr.Where(w => w.Name == (age < 60 ? "NRMALPERSON" : age >= 60 && age < 80 ? "SENIER" : "SUPERSINER")).FirstOrDefault().Id).FirstOrDefault();

                                decimal? Basevalue = worksheetList.Where(w => w.fieldId == new Guid(tx.BaseFormula)).FirstOrDefault().Total;

                                SubSlabRange(tx.Formula, Convert.ToDecimal(Basevalue), worksheetList);
                            }


                        }
                        if (f.Name == "TPR")
                        {
                            if (!IsCurrentmonth) // Current Month No
                            {
                                TaxHistory paidtax = new TaxHistory();
                                decimal paidTax = 0;
                                int month1 = month + 1;
                                int year1 = year;
                                if (month1 > 12)
                                {
                                    month1 = 1;
                                    year1 = year1 + 1;
                                }
                                paidTax = paidtax.paidtax(txFin, year1, month1, companyId, attr, emp);
                                paidTax = Convert.ToDecimal(string.Format("{0:F2}", paidTax));
                                var addNxtTax = tmpHist.Where(W => W.FieldId == f.Id).FirstOrDefault();
                                if (!ReferenceEquals(addNxtTax, null))
                                {

                                    var NxtTax = worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault();


                                    //   decimal temppaid = Convert.ToDecimal(worksheetList.Where(s => s.Description.Contains("TAXPAID")).FirstOrDefault().Total);
                                    //   decimal paidTax = temppaid == 0 ? Convert.ToDecimal(worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total) : temppaid;
                                    //  if (temppaid != 0) paidTax = paidTax - Convert.ToDecimal(worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total);
                                    var oneTax = tmpHist.Where(v => v.Field == "ONETIMETAX").FirstOrDefault();
                                    if (!ReferenceEquals(oneTax, null) && oneTax.Total > 0)
                                    {
                                        //Below one time tax value set to 0 in order to solve the "TDS to be deducted for" calculation issue.
                                        oneTax.Total = 0;
                                        var temptaxperMonth = worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault();
                                        if (temptaxperMonth != null)
                                            worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total = (temptaxperMonth != null ? temptaxperMonth.Total : 0) - oneTax.Total;
                                        worksheetList.Where(s => s.Description.Contains("TAXPAID")).FirstOrDefault().Total = paidTax;// + oneTax.Total;
                                        worksheetList.Where(g => g.Description == f.Name).FirstOrDefault().Total = worksheetList.Where(g => g.Description == f.Name).FirstOrDefault().Total - oneTax.Total;// - NxtTax.Total;
                                        worksheetList.Remove(worksheetList.Where(g => g.Description == "ONETIMETAX").FirstOrDefault());// onetime tax remove for future month because one time is current month value.
                                    }
                                    else
                                    {
                                        worksheetList.Where(g => g.Description == f.Name).FirstOrDefault().Total = worksheetList.Where(g => g.Description == f.Name).FirstOrDefault().Total - NxtTax.Total;
                                        worksheetList.Where(s => s.Description.Contains("TAXPAID")).FirstOrDefault().Total = paidTax;
                                    }
                                    if (month == txFin.StartingDate.Month)
                                        //  worksheetList.Where(r => r.Description == "TAXPAID").FirstOrDefault().Total = paidTax + worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total;
                                        // else
                                        worksheetList.Where(r => r.Description == "TAXPAID").FirstOrDefault().Total = paidTax;
                                }
                            }
                            else
                            {
                                TaxHistory paidtax = new TaxHistory();
                                decimal paidTax = paidtax.paidtax(txFin, year, month, companyId, attr, emp);
                                if (month != txFin.StartingDate.Month)
                                    worksheetList.Where(r => r.Description == "TAXPAID").FirstOrDefault().Total = paidTax;// + worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total;                              
                                else
                                    worksheetList.Where(r => r.Description == "TAXPAID").FirstOrDefault().Total = 0;

                            }
                        }


                        if (worksheetList.Where(g => g.Description == f.Name).FirstOrDefault() != null)
                        {
                            if (f.OrderNumber == 0)
                            {
                                worksheetList.RemoveAll(r => r.Description == f.Name);
                            }
                            else
                            {
                                if (f.Name == "TOTTAXPERMONTH")
                                {

                                    decimal tempTotalTax = Convert.ToDecimal(worksheetList.Where(r => r.Description == "TOTTAX").FirstOrDefault().Total);
                                    decimal tempTaxPaid = Convert.ToDecimal(worksheetList.Where(r => r.Description == "TAXPAID").FirstOrDefault().Total);
                                    var renameToNxtMonth = worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault();
                                    int nxtMonth = Convert.ToInt16(renameToNxtMonth.Description.Substring(23, 2));
                                    var oneTax = tmpHist.Where(v => v.Field == "ONETIMETAX").FirstOrDefault();
                                    decimal onetimeTax = 0;
                                    if (!ReferenceEquals(oneTax, null))
                                        onetimeTax = Convert.ToDecimal(oneTax.Total);


                                    if (!IsCurrentmonth)
                                    {
                                        if (processtype != "employee")
                                        {
                                            nxtMonth = nxtMonth - 1;
                                        }
                                        if (nxtMonth == 0)
                                        {
                                            nxtMonth = 1;
                                        }
                                        decimal taxperMonth = Math.Round(((tempTotalTax - tempTaxPaid - onetimeTax) / nxtMonth), 2);
                                        worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().Description = "TDS to be deducted for " + nxtMonth + " Months, Per Month Value is ";
                                        worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().Total = taxperMonth;
                                        worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total = taxperMonth;
                                        if (txFin.EndingDate == lastdate)
                                        {
                                            worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total = 0;
                                            worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().Total = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (nxtMonth == 0)
                                        {
                                            nxtMonth = 1;
                                        }
                                        decimal taxperMonth = Math.Round(((tempTotalTax - tempTaxPaid - onetimeTax) / nxtMonth), 2);
                                        worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().Description = "TDS to be deducted for " + nxtMonth + " Months, Per Month Value is ";
                                        worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().Total = taxperMonth;
                                        worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total = taxperMonth;
                                        if (!ReferenceEquals(oneTax, null))
                                        {
                                            worksheetList.Where(W => W.Description == "TOTTAXPERMONTH").FirstOrDefault().Total = taxperMonth + oneTax.Total;
                                        }
                                    }
                                    if (tempTaxPaid == 0)
                                        worksheetList.Where(W => W.Description == "TPR").FirstOrDefault().Total = tempTotalTax;
                                    else
                                        worksheetList.Where(W => W.Description == "TPR").FirstOrDefault().Total = tempTotalTax - tempTaxPaid;
                                    worksheetList.Where(m => m.Description.Contains("TDS to be deducted for")).FirstOrDefault().OrderSec = ConvertAlpha(f.OrderNumber); f.OrderNumber++;

                                }
                                string o = ConvertAlpha(f.OrderNumber);
                                worksheetList.Where(g => g.Description == f.Name).FirstOrDefault().OrderSec = o;
                            }
                        }
                    });
                    worksheetList.RemoveAll(r => r.Description.ToLower().Trim() == "employeeserviceyear" || r.Description.ToLower().Trim() == "employeeage" || r.Description.ToLower().Trim() == "ismetro");

                    attr.Where(at => at.BehaviorType == "Tax" || at.IsTaxable == true).ToList().ForEach(f =>
                   {
                       string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                       if (worksheetList.Where(w => w.Description == f.Name).FirstOrDefault() != null)
                       {

                           worksheetList.Where(w => w.Description == f.Name).FirstOrDefault().Description = f.Name == "TOTTAXPERMONTH" ? (IsCurrentmonth == true ? "Total Tax for this Month_" + monthName : "Total Tax for Further Month") : f.DisplayAs;
                       }



                   });




                    //string empAddress = !string.IsNullOrEmpty(addr.AddressLine1) ? addr.AddressLine1 + (!string.IsNullOrEmpty(addr.AddressLine2) ? "*" + addr.AddressLine2 : "")
                    //               : !string.IsNullOrEmpty(addr.AddressLine2) ? addr.AddressLine2 + "*" : "";
                    //empAddress = !string.IsNullOrEmpty(empAddress) ? empAddress+ ", "+addr.City + "*" : "";
                    //empAddress = emp.FirstName + "*" + empAddress;

                    string lastName = !string.IsNullOrEmpty(emp.LastName) ? emp.LastName : "";

                    string companyAddress = company.CompanyName + "*" + company.AddressLine1 + "*" + company.AddressLine2 + "*";
                    companyAddress = companyAddress + company.City + " ," + company.State;
                    rptparms.Add(emp.EmployeeCode + "*" + emp.FirstName + " " + lastName + "*" + emp.DesignationName);
                    rptparms.Add(companyAddress);
                    rptparms.Add(("WORKSHEET FOR THE MONTH OF " + (MonthEnum)month + " " + year).ToUpper());
                    if (newtaxscheme == "1")
                    {
                        rptparms.Add("TDS WORK SHEET (NEW TAX SCHEME)");
                    }
                    else
                    {
                        rptparms.Add("TDS WORK SHEET");
                    }
                    generateworksheet(pdfPath, worksheetList, rptparms);
                    outputfilePath = pdfPath;


                }

            });//end emp



            if (emplist.Count() > 0)
            {
                if (emplist.Count() > 1)
                {

                    outputfilePath = DocumentProcessingSettings.TempDirectoryPath + "worksheet.zip";
                    ZipPath(outputfilePath, reportPath, null, true, null);

                }

            }
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outputfilePath });
        }
        public JsonResult DeleteValidation(Guid selectedId, int year, int month, string type, Guid financeyearId, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            DateTime applyDate = new DateTime(year, month, 1);
            //EmployeeList notEligible = new EmployeeList();
            //EmployeeList eligible = new EmployeeList();
            List<TaxProcessEmployees> notEligible = new List<Tax.TaxProcessEmployees>();
            List<TaxProcessEmployees> eligible = new List<Tax.TaxProcessEmployees>();

            TaxHistoryList txhl = new TaxHistoryList();

            emplist.ForEach(u =>
            {
                /* when accessed by single employee the table will be changed to temp table here */

                if (processtype == "employee")
                {
                    txhl = new TaxHistoryList(financeyearId, u.Id, applyDate, type, emproll);
                }
                else
                {
                    txhl = new TaxHistoryList(financeyearId, u.Id);
                }

                if (txhl.Count() > 0)
                {
                    DateTime maxDate = txhl.OrderByDescending(o => o.ApplyDate).FirstOrDefault().ApplyDate;
                    TaxHistoryList txh = new TaxHistoryList();
                    txh.AddRange(txhl.Where(thl => thl.EmployeeId == u.Id && thl.FinanceYearId == financeyearId && thl.ApplyDate == applyDate).ToList());
                    // TaxHistoryList txh = new TaxHistoryList(financeyearId, u.Id, Convert.ToDateTime("1/" + month + "/" + year, new System.Globalization.CultureInfo("en-GB")));
                    if (txh.Count() > 0)
                    {
                        TaxProcessEmployees empl = new TaxProcessEmployees();
                        if (month == maxDate.Month && year == maxDate.Year)
                        {
                            empl.EmployeeCode = u.EmployeeCode;
                            empl.Id = u.Id;
                            eligible.Add(empl);
                        }
                        else
                        {
                            empl.EmployeeCode = u.EmployeeCode;
                            empl.Id = u.Id;
                            notEligible.Add(empl);
                        }
                    }
                }

            });

            return base.BuildJson(true, 100, "", new { notEligible = notEligible, eligible = eligible });
        }
        public JsonResult DeleteTaxProcess(Guid selectedId, int year, int month, string type, Guid financeyearId, List<Guid> empid, string processtype)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            TXFinanceYear txfinyear = new TXFinanceYear(financeyearId, companyId);
            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            EmployeeList employeeList = new EmployeeList();
            AttributeModelList AttributemodelList = new AttributeModelList(companyId);

            empid.ForEach(f =>
            {
                employeeList.Add(emplist.Where(w => w.Id == f).FirstOrDefault());
            });


            AttributeModel taxatrr = AttributemodelList.Where(a => a.Name == "TOTTAXPERMONTH").FirstOrDefault();
            AttributeModel payatrr = AttributemodelList.Where(a => a.Name == "TDS").FirstOrDefault();
            AttributeModel netatrr = AttributemodelList.Where(a => a.Name == "NETPAY").FirstOrDefault();
            AttributeModel dedatrr = AttributemodelList.Where(a => a.Name == "TOTDED").FirstOrDefault();

            int processcount = 0;
            bool t = false;
            bool t1 = false;

            employeeList.ForEach(u =>
            {
                TaxHistory txHistory = new TaxHistory();
                txHistory.EmployeeId = u.Id;
                txHistory.FinanceYearId = financeyearId;
                txHistory.ApplyDate = Convert.ToDateTime("1/" + month + "/" + year, new System.Globalization.CultureInfo("en-GB"));

                /* when accessed by single employee table will be changed to temp table here*/
                if (processtype == "employee")
                {
                    t = txHistory.TempDelete();
                    t1 = txHistory.TempDeleteAP();
                }
                else
                {
                    t = txHistory.Delete();
                    t1 = txHistory.DeleteAP();
                }
                processcount = processcount + 1;
            });

            if (t == true && t1 == true)
            {
                return base.BuildJson(true, 200, processcount > 0 ? "Deleted Tax Process successfully" : "No Record(s) Processed", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", null);
            }
        }
        public static string ConvertAlpha(int value)
        {
            const int a = (int)'A';
            value = value - 1;
            var returnValue = new StringBuilder();
            while (value > -1)
            {
                var remainder = value % 26;
                returnValue.Insert(0, (char)(a + remainder));
                value = value / 26 - 1;
            }
            return returnValue.ToString();
        }
        public void SubSlabRange(string expr, decimal baseValue, List<rptWorkSheet> wrkSheet)
        {
            string result;
            string output = "0";
            string[] ranges = expr.TrimEnd(':').Split(':');
            decimal currentBaseValue = baseValue;
            decimal tobedeductbase = 0;
            decimal deductedval = 0;
            decimal tovalue = 0;
            for (int range = 0; range < ranges.Count(); range++)
            {
                int count = range;
                string fromVal = ranges[range].Substring(0, ranges[range].IndexOf('-'));
                string toVal = ranges[range].Substring(ranges[range].IndexOf('-') + 1, ranges[range].ToUpper().IndexOf("THEN") - ranges[range].IndexOf('-') - 1);
                string thenVal = ranges[range].Substring(ranges[range].IndexOf("THEN") + 4);
                if (baseValue < Convert.ToDecimal(fromVal))
                {
                    tobedeductbase = currentBaseValue;
                    currentBaseValue = 0;
                }
                else if (baseValue > Convert.ToDecimal(fromVal) && baseValue <= Convert.ToDecimal(toVal))
                {
                    tobedeductbase = currentBaseValue;
                    currentBaseValue = 0;
                    range = ranges.Count();
                }
                else if (baseValue > Convert.ToDecimal(toVal))
                {
                    tobedeductbase = Math.Abs(Convert.ToDecimal(toVal) - tovalue);
                    currentBaseValue = currentBaseValue - tobedeductbase;
                    tovalue = Convert.ToDecimal(toVal);
                }
                string outExp = thenVal + "*" + tobedeductbase.ToString();
                deductedval = deductedval + tobedeductbase;
                if (baseValue == 0)
                {
                    outExp = "0";
                }

                Eval eval = new Eval();
                string val = eval.Execute(outExp).ToString();
                output = Convert.ToString(Convert.ToDecimal(output) + Convert.ToDecimal(val));
                // baseValue = currentBaseValue;

                string per = thenVal.Split('/').First();


                string des = count == 0 ? "* Exemption Rs." + (Convert.ToDecimal(toVal) > baseValue ? baseValue.ToString() : toVal) + "and the balance amount" : "* For" + tobedeductbase + " : Tax " + per + "% Tax Amount";




                rptWorkSheet rws = new rptWorkSheet();

                rws.Description = des;
                rws.FormulaType = 9;
                rws.Total = count == 0 ? currentBaseValue : Math.Round(Convert.ToDecimal(val) + Convert.ToDecimal(0.01));
                rws.Type = "Tax";
                rws.Order = "E";

                wrkSheet.Add(rws);



            }
            result = output;


        }
    }
    public class jsonWorksheetColumn
    {
        public Guid Id { get; set; }

        public int Order { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }

        public static jsonWorksheetColumn convertObject(TXSection imatch)
        {

            return new jsonWorksheetColumn()
            {
                Id = imatch.Id,
                Section = imatch.DisplayAs,
                Description = imatch.DisplayAs

            };
        }
        public jsonWorksheetColumn totalEarning(string f, int companyId)
        {

            return new jsonWorksheetColumn()
            {
                Id = Guid.Empty,
                Description = f + "Total Earning",
                Section = "Earning",
                Order = 1

            };
        }
        public jsonWorksheetColumn grossSalary(string f, int companyId)
        {

            return new jsonWorksheetColumn()
            {
                Id = new AttributeModelList(companyId).Where(a => a.Name == "GROSSSALARY").FirstOrDefault().Id,
                Description = f + "Gross Salary",
                Section = "Earning",
                Order = 1

            };
        }
        public void aftertotal(TXSectionList tsl, List<jsonWorksheetColumn> lst)
        {
            tsl.Where(w => w.SectionType != "Others" && w.ParentId == Guid.Empty).ToList().ForEach(f =>
               {
                   lst.Add(new jsonWorksheetColumn()
                   {
                       Id = Guid.Empty,
                       Description = "After." + f.DisplayAs,
                       Section = f.DisplayAs,
                       Order = 3

                   });
               });

            lst.Add(new jsonWorksheetColumn()
            {
                Id = Guid.Empty,
                Description = "After." + "otherincometotal",
                Section = "Others",
                Order = 2

            });

        }
        public void tsdToBeDeducted(List<jsonWorksheetColumn> litlwsc, int comp)
        {

            litlwsc.Add(new jsonWorksheetColumn()
            {
                Id = Guid.Empty,
                Description = "TDS to be deducted for _ Months, .Per Month Value ",
                Section = "Taxable",
                Order = 3

            });
            AttributeModelList attlist = new AttributeModelList(comp);
            litlwsc.Add(new jsonWorksheetColumn()
            {
                Id = attlist.Where(w => w.Name == "TOTINCOME").FirstOrDefault().Id,
                Description = attlist.Where(w => w.Name == "TOTINCOME").FirstOrDefault().DisplayAs,
                Section = "Taxable",
                Order = 3

            });

            litlwsc.Add(new jsonWorksheetColumn()
            {
                Id = attlist.Where(w => w.Name == "ONETIMETAX").FirstOrDefault().Id,
                Description = attlist.Where(w => w.Name == "ONETIMETAX").FirstOrDefault().DisplayAs,
                Section = "Taxable",
                Order = 3

            });

            litlwsc.Add(new jsonWorksheetColumn()
            {
                Id = attlist.Where(w => w.Name == "TAXPAID").FirstOrDefault().Id,
                Description = attlist.Where(w => w.Name == "TAXPAID").FirstOrDefault().DisplayAs,
                Section = "Taxable",
                Order = 3

            });

        }



    }

    public class jsonTaxProcess
    {
        public Guid employeeId { get; set; }
        public Guid financeYearId { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool isProofwise { get; set; }
    }
    public class rptWorkSheet
    {
        public string EmpCode { get; set; }

        public Guid fieldId { get; set; }
        public string Description { get; set; }

        public string ParentSection { get; set; }
        public string Type { get; set; }

        public int OtherIncomeType { get; set; }

        public string Order { get; set; }
        public string OrderG { get; set; }

        public string OrderSubSec { get; set; }
        public int numOrderG { get; set; }
        public string OrderSec { get; set; }
        public decimal? CalculationAmt { get; set; }
        public decimal? Actual { get; set; }
        public decimal? Projection { get; set; }

        public int? FormulaType { get; set; }
        public decimal? Total { get; set; }

    }

    public class tdsStatement
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? AlreadyDeducted { get; set; }
        public decimal? BalanceTax { get; set; }

        public decimal? OneTimeTax { get; set; }
        public decimal? Permonth { get; set; }
        public int? NoOfMonths { get; set; }

        public decimal? Thismonth { get; set; }
        public string TaxPercentage { get; set; }


    }

    public class column
    {
        public List<Guid> listid { get; set; }
        public List<string> listfield { get; set; }
    }

    public struct TaxProcessEmployees
    {
        public string EmployeeCode { get; set; }
        public Guid Id { get; set; }
    }
}