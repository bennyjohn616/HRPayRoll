﻿using Microsoft.Reporting.WebForms;
using Payroll.CustomFilter;
using PayrollBO;
using System;
using PayRollReports;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class TransactionController : BaseController
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ArrearViewPeriod(Guid selectedId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeList Employee = new EmployeeList(companyId, selectedId);
            List<DateTime> allPeriod = new List<DateTime>();
            List<DateTime> groupPeriod = new List<DateTime>();
            Employee.ForEach(f =>
            {
                IncrementList incrementlist = new IncrementList(f.Id);
                incrementlist.ForEach(i =>
                {
                    allPeriod.Add(new DateTime(i.ApplyYear, i.ApplyMonth, 1));
                });
            });
            allPeriod.Add(new DateTime(2018, 5, 1));

            allPeriod.GroupBy(g => g).ToList().ForEach(f =>
            {
                groupPeriod.Add(f.Key);
            });

            return base.BuildJson(true, 200, "success", groupPeriod);
        }

        public JsonResult ArrearView(Guid selectedId, DateTime period)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EmployeeList Employee = new EmployeeList(companyId, selectedId);

            DataTable dt = new ArrearHistory().ArrearViewValues(selectedId, period.Month, period.Year);
            List<jsonArrearViewlist> listatt = new List<jsonArrearViewlist>();
            if (dt.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dt.Rows.Count; rowcount++)
                {
                    jsonArrearViewlist att = new jsonArrearViewlist();

                    att.EmployeeCode = Convert.ToString(dt.Rows[rowcount]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["month"])))
                        att.month = Convert.ToInt16(dt.Rows[rowcount]["month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["year"])))
                        att.year = Convert.ToInt16(dt.Rows[rowcount]["year"]);
                    att.Attribute = Convert.ToString(dt.Rows[rowcount]["attriDisplay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["Value"])))
                        att.Value = Convert.ToString(dt.Rows[rowcount]["Value"]);


                    listatt.Add(att);

                }
            }

            if (listatt.Count > 1)
            {
                EmployeeList emplist = new EmployeeList(companyId);
                //Header
                int c = listatt.GroupBy(g => g.Attribute).ToList().Count();
                string[] header = new string[c + 3];
                int i = 1;
                header[0] = "EmployeeCode";
                header[1] = "EmployeeName";
                listatt.GroupBy(g => g.Attribute).ToList().ForEach(f =>
                {
                    i++;
                    header[i] = f.Key;
                });
                i++;
                header[i] = "Total";

                //Rows
                List<string[]> rows = new List<string[]>();

                listatt.GroupBy(g => g.EmployeeCode).ToList().ForEach(f =>
                {
                    string[] li = new string[c + 3];

                    li[0] = f.Key;
                    li[1] = emplist.Where(w => w.EmployeeCode == f.Key).FirstOrDefault().FirstName;
                    listatt.GroupBy(g => g.Attribute).ToList().ForEach(m =>
                    {
                        int index1 = Array.IndexOf(header, m.Key);

                        if (!object.ReferenceEquals(listatt.Where(w => w.EmployeeCode == f.Key && w.Attribute == m.Key).FirstOrDefault(), null))
                        {
                            li[index1] = listatt.Where(w => w.EmployeeCode == f.Key && w.Attribute == m.Key).FirstOrDefault().Value;
                        }
                        else
                        {
                            li[index1] = "0";
                        }

                    });
                    li[Array.IndexOf(header, "Total")] = listatt.Where(w => w.EmployeeCode == f.Key).ToList().Sum(s => Convert.ToDecimal(s.Value)).ToString();
                    rows.Add(li);
                });
                decimal finalTotal = 0;
                //Footer
                string[] footer = new string[c + 3];
                footer[0] = "";
                footer[1] = "Total";
                listatt.GroupBy(g => g.Attribute).ToList().ForEach(m =>
                {
                    int index1 = Array.IndexOf(header, m.Key);
                    footer[index1] = listatt.Where(w => w.Attribute == m.Key).Sum(s => Convert.ToDecimal(s.Value)).ToString();

                    finalTotal = finalTotal + listatt.Where(w => w.Attribute == m.Key).Sum(s => Convert.ToDecimal(s.Value));
                });
                footer[Array.IndexOf(header, "Total")] = finalTotal.ToString();

                return base.BuildJson(true, 200, "success", new { rowheader = header, rows = rows, rowfooter = footer });
            }




            return base.BuildJson(false, 200, "NO Data Available", null);
        }


        public JsonResult DeleteCtegoryIncrements(Guid categoryid)
        {
            int count = 0;
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            EmployeeList empList = new EmployeeList(companyId);
            IncrementList lncrelist = new IncrementList();

            empList.Where(w => w.CategoryId == categoryid).ToList().ForEach(f =>
            {
                IncrementList incrementlist = new IncrementList(f.Id);
                incrementlist.ForEach(inc =>
                {
                    if (inc.IsProcessed == false)
                    {
                        lncrelist.Add(inc);
                    }
                });

            });

            lncrelist.ForEach(f =>
            {
                f.IsProcessed = false;
                f.ProcessFlag = "Delete";
                if (f.Delete())
                {

                }
                else
                {
                    count++;
                }
            });


            if (count == 0)
            {

                return base.BuildJson(true, 200, "Data Deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }
        }

        public JsonResult GetEmployeeIncrements(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            IncrementList incrementlist = new IncrementList(employeeId);
            List<jsonIncrement> data = new List<jsonIncrement>();
            incrementlist.ForEach(u =>
            {
                data.Add(jsonIncrement.toJson(u, new AttributeModelList()));
            });
            jsonIncrement lastEntry = data.FirstOrDefault();
            return base.BuildJson(true, 200, "success", new { data = data, lastEntry = lastEntry });
        }

        public JsonResult GetIncrement(Guid employeeId, Guid incrementId, bool Isnew)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Increment increment = new Increment(incrementId, employeeId);
            if (increment.Id != incrementId)
                increment = new Increment();

            EntityMasterValueList entityMasterValus = new EntityMasterValueList(employeeId, ComValue.EmployeeTable);
            AttributeModelList attrmodlList = new AttributeModelList(companyId);
            EntityAttributeModelList entityAttributeModelList = new EntityAttributeModelList(entityMasterValus.Count == 0 ? Guid.Empty : entityMasterValus[0].EntityModelId);
            EntityBehaviorList entityBehaviour = new EntityBehaviorList();
            if (entityMasterValus.Count > 0)
            {
                entityBehaviour = new EntityBehaviorList(entityMasterValus[0].EntityId, entityMasterValus.Count == 0 ? Guid.Empty : entityMasterValus[0].EntityModelId);
            }

            attrmodlList.ForEach(u =>
            {
                var entityMasterval = entityMasterValus.Where(q => q.AttributeModelId == u.Id).FirstOrDefault();

                if (u.IsIncrement)
                {
                    var tempBehaviour = entityBehaviour.Where(e => e.AttributeModelId == u.Id).FirstOrDefault();
                    if (tempBehaviour != null && tempBehaviour.ValueType == 1)//increment yes and Master input data only show a increment components --5/25/2018
                    {
                        var temp = increment.IncrementDetailList.Where(p => p.AttributeModelId == u.Id).FirstOrDefault();
                        if (object.ReferenceEquals(temp, null))
                        {
                            if (!object.ReferenceEquals(entityMasterval, null))
                            {
                                EntityBehavior beh = new EntityBehavior(u.Id, entityMasterval.EntityModelId, entityMasterval.EntityId);
                                decimal val;
                                if (Decimal.TryParse(entityMasterval.Value ?? "0", out val) && beh.ValueType == 1)
                                {
                                    increment.IncrementDetailList.Add(new IncrementDetail() { AttributeModelId = u.Id, NewValue = Convert.ToDecimal(entityMasterval.Value ?? "0"), OldValue = 0 });
                                }
                            }
                            else
                            {
                                //  increment.IncrementDetailList.Add(new IncrementDetail() { AttributeModelId = u.Id, NewValue = 0, OldValue = 0 });
                                var entityAttributeModelval = entityAttributeModelList.Where(q => q.AttributeModelId == u.Id).FirstOrDefault();
                                if (!object.ReferenceEquals(entityAttributeModelval, null))
                                {
                                    //EntityBehavior beh = new EntityBehavior(u.Id, entityAttributeModelval.EntityModelId,Guid.Empty);
                                    // if (beh !=null&& beh.ValueType == 1)
                                    increment.IncrementDetailList.Add(new IncrementDetail() { AttributeModelId = u.Id, NewValue = 0, OldValue = 0 });
                                }

                            }

                        }
                    }
                }

            });
            jsonIncrement data = jsonIncrement.toJson(increment, attrmodlList, Isnew);
            if (entityMasterValus.Count == 0)
            {
                return base.BuildJson(false, 200, "Dynamic Group was mapped with employee", data);
            }
            return base.BuildJson(true, 200, "success", data);
        }
        // created By AjithPanner on 11/20/2017
        public JsonResult GetEmployeeJoiningDate(Guid employeeId, DateTime date)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Employee employee = new Employee(companyId, employeeId);
            employee.Initialize();
            jsonEmployee jsondata = jsonEmployee.tojson(employee);
            if (employee.DateOfJoining <= date)
            {
                return base.BuildJson(true, 200, "valide effective date", jsondata);
            }
            else
            {
                return base.BuildJson(false, 200, "Effective date lesser than joining date", jsondata);
            }

        }

        public JsonResult GetLopDays(Guid employeeId, int Month, int Year)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TableCategoryList tabCatList = new TableCategoryList(companyId);
            var tabCat = tabCatList.Where(u => u.Name == "Payroll").FirstOrDefault();
            EntityModelList entiModelList = new EntityModelList(tabCat.Id);
            var entityModel = entiModelList.Where(u => u.Name == "Salary").FirstOrDefault();
            EntityModelMappingList entitymodelMappinglist = new EntityModelMappingList("Employee", companyId);
            var ent = entitymodelMappinglist.Where(u => Convert.ToString(u.EntityTableName).ToUpper() == Convert.ToString(entityModel.Id).ToUpper()).FirstOrDefault();
            double lop = 0;
            if (ent != null)
            {
                EntityMappingList entitymappinglist = new EntityMappingList("Employee");
                var LopMI = entitymappinglist.Where(u => u.EntityTableName.ToUpper() == entityModel.Id.ToString().ToUpper() && new Guid(u.RefEntityId) == employeeId).FirstOrDefault();

                MonthlyInputList monthlyInput = new MonthlyInputList(new Guid(Convert.ToString(LopMI.EntityId)), employeeId, Month, Year);
                PayrollHistory payhist = new PayrollHistory(companyId, employeeId, Year, Month);
                AttributeModelList attrlist = new AttributeModelList(companyId);
                var attrModel = attrlist.Where(u => u.Name.ToUpper() == "LD").FirstOrDefault();
                if (monthlyInput == null || monthlyInput.Count == 0)
                {
                    return base.BuildJson(false, 200, "Monthly Input Not available for the month", lop);
                }
                var lds = monthlyInput.Where(u => u.AttributeModelId == attrModel.Id).FirstOrDefault();
                if (lds != null)
                {
                    lop = Convert.ToDouble(lds.Value);
                }
                var ldsp = payhist.PayrollHistoryValueList.Where(u => u.AttributeModelId == attrModel.Id).FirstOrDefault();
                if (ldsp != null)
                {
                    lop = Convert.ToDouble(ldsp.Value);
                }
            }
            return base.BuildJson(true, 200, "success", lop);
        }

        public JsonResult SaveIncrement(jsonIncrement dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Increment increment = jsonIncrement.convertObject(dataValue);
            PayrollHistoryList payrollHistoryList = new PayrollHistoryList(dataValue.employeeId, companyId);
            increment.CreatedBy = userId;
            increment.ModifiedBy = userId;
            increment.IsProcessed = false;
            var payrolllist = payrollHistoryList.Where(ph => ph.Month == dataValue.month && ph.Year == dataValue.year && ph.IsDeleted == false).FirstOrDefault();
            if (payrolllist == null)
            {
                if (DateTime.Parse(dataValue.effDate) <= Convert.ToDateTime(Convert.ToDateTime(dataValue.effDate, new CultureInfo("en-GB")).Day + "/" + dataValue.month + "/" + dataValue.year, new CultureInfo("en-GB")))
                {


                    if (increment.Save())
                    {
                        increment.IncrementDetailList.ForEach(u =>
                        {
                            u.IncrementId = increment.Id;
                            u.CreatedBy = userId;
                            u.ModifiedBy = userId;
                            u.Save();
                        });
                        return base.BuildJson(true, 200, "Data saved successfully", dataValue);
                    }
                    else
                    {
                        return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
                    }
                }
                else
                {
                    return base.BuildJson(false, 100, "Effective Month is greater than Apply Month", dataValue);

                }
            }
            else
            {
                return base.BuildJson(false, 100, "Already Payroll Processed for Given Apply month  ", dataValue);
            }
        }

        public JsonResult DeleteIncrement(jsonIncrement dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int userId = Convert.ToInt32(Session["UserId"]);
            Increment increment = jsonIncrement.convertObject(dataValue);
            increment.CreatedBy = userId;
            increment.ModifiedBy = userId;
            increment.IsProcessed = false;
            increment.ProcessFlag = "Delete";
            if (increment.Delete())
            {

                return base.BuildJson(true, 200, "Data Deleted successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", dataValue);
            }

        }

        public JsonResult GetStopPaymentData(jsonStopPayment dataValue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            StopPayment stopPayment = new StopPayment(Guid.Empty, dataValue.SPayEmpId);

            EmployeeList employeeList = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            jsonStopPayment data = jsonStopPayment.tojson(stopPayment, employeeList);

            //   .ForEach(u => { jsondata.Add(jsonEmpFamily.tojson(u)); });
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult GetStopPayment(jsonStopPayment dataValue)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            EmployeeList employeeList = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            StopPaymentList stopPaymentList = new StopPaymentList(dataValue.SPayEmpId);
            List<jsonStopPayment> jsondata = new List<jsonStopPayment>();
            stopPaymentList.ForEach(u => { jsondata.Add(jsonStopPayment.tojson(u, employeeList)); });
            return base.BuildJson(true, 200, "success", jsondata);


        }

        public JsonResult SaveStopPaymentData(jsonStopPayment dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PayrollHistoryList payrollHistoryList = new PayrollHistoryList(dataValue.SPayEmpId, companyId);
            if (!object.ReferenceEquals(payrollHistoryList.Where(u => u.Month == dataValue.SPayMonth && u.Year == dataValue.SPayYear && u.Status == "Completed").FirstOrDefault(), null))
            {
                return base.BuildJson(false, 100, "Cannot proceed stop payment for this month.", dataValue);
            }
            StopPayment stopPayment = jsonStopPayment.convertobject(dataValue);
            int userId = Convert.ToInt32(Session["UserId"]);
            stopPayment.CreatedBy = userId;
            stopPayment.ModifiedBy = stopPayment.CreatedBy;
            stopPayment.IsDeleted = false;

            isSaved = stopPayment.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult DeleteStopPaymentData(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            bool isDeleted = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            StopPayment stopPayment = new StopPayment(id, Guid.Empty);

            stopPayment.CreatedBy = userId;
            stopPayment.ModifiedBy = stopPayment.CreatedBy;
            stopPayment.IsDeleted = true;
            isDeleted = stopPayment.Delete();
            if (isDeleted)
            {
                return base.BuildJson(true, 200, "Data deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Deleting the data.", null);
            }
        }
        public JsonResult GetFullAndFinalData(jsonFullfinalSettlement datavalue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            FullFinalSettlement fullFinalSettlement = new FullFinalSettlement(datavalue.Fafid, datavalue.FafEmpId);
            Employee employee = new Employee(companyId, Guid.Empty, datavalue.FafEmpId);
            jsonFullfinalSettlement data = jsonFullfinalSettlement.tojson(fullFinalSettlement, employee);

            LoanEntryList loanEntrylist = new LoanEntryList(datavalue.FafEmpId);
            LoanMasterList loanmasterlist = new LoanMasterList(companyId);
            decimal loanAmount = 0;
            loanEntrylist.ForEach(f =>
            {
                loanAmount = loanAmount + f.LoanTransactionList.Where(w => w.Status == "UnPaid").ToList().Sum(s => s.AmtPaid);
            });
            List<LoanController.jsonLoanEntry> jsondata = new List<LoanController.jsonLoanEntry>();
            loanEntrylist.ForEach(u => { jsondata.Add(LoanController.jsonLoanEntry.tojson(u, loanmasterlist)); });
            if (loanEntrylist.Count > 0)
            {
                data.Fafloanamount = loanAmount;
            }


            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult GetGridData(Guid employeeId, int month, int year)
        {
            var att1 = Guid.Empty;
            var att2 = Guid.Empty;
            var att3 = Guid.Empty;
            var status = Guid.Empty;
            var value1 = 0;
            var value2 = 0;
            var value3 = 0;
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PayrollHistory payrollhis = new PayrollHistory(companyId, employeeId, year, month);
            PayrollHistoryValueList payrollValueList = new PayrollHistoryValueList(payrollhis.Id);
            AttributeModelList attributeModel = new AttributeModelList(companyId);
            FullFinalSettlement fullandfinal = new FullFinalSettlement(Guid.Empty, employeeId);
            var attributNetId = attributeModel.Where(att => att.Name == "NETPAY").FirstOrDefault();
            if (attributNetId != null)
            {
                att1 = attributNetId.Id;
            }
            var attributGrossId = attributeModel.Where(attg => attg.Name == "EG").FirstOrDefault();
            if (attributGrossId != null)
            {
                att2 = attributGrossId.Id;
            }
            var attributeTds = attributeModel.Where(attd => attd.Name == "TDS").FirstOrDefault();
            if (attributeTds != null)
            {
                att3 = attributeTds.Id;
            }
            var pvlValue1 = payrollValueList.Where(pvl1 => pvl1.AttributeModelId == att1 && pvl1.PayrollHistoryId == payrollhis.Id).FirstOrDefault();
            if (pvlValue1 != null)
            {
                value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(pvlValue1.Value)));
            }
            var pvlValue2 = payrollValueList.Where(pvl2 => pvl2.AttributeModelId == att2 && pvl2.PayrollHistoryId == payrollhis.Id).FirstOrDefault();
            if (pvlValue2 != null)
            {
                value2 = Convert.ToInt32(Math.Round(Convert.ToDecimal(pvlValue2.Value)));
            }
            var pvlValue3 = payrollValueList.Where(pvl3 => pvl3.AttributeModelId == att3 && pvl3.PayrollHistoryId == payrollhis.Id).FirstOrDefault();
            if (pvlValue3 != null)
            {
                value3 = Convert.ToInt32(Math.Round(Convert.ToDecimal(pvlValue3.Value)));
            }



            List<object> val = new List<object>();
            val.Add(new { payMonth = month, payYear = year, gross = value2, tds = value3, netPay = value1 });
            return base.BuildJson(true, 200, "Success", val);
        }
        public JsonResult GetFullAndFinalMonthlyComponent(Guid empId, int month, int year)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModel entityModel = new EntityModel(ComValue.SalaryTable, companyId);
            EntityMapping entitmap = new EntityMapping("Employee", empId.ToString(), entityModel.Id);
            Entity entity = new Entity();
            if (!string.IsNullOrEmpty(entitmap.EntityId) && entitmap.EntityId != "00000000-0000-0000-0000-000000000000")
            {
                List<PayrollError> payErrors = new List<PayrollError>();
                PayrollHistory payHistory = new PayrollHistory();
                Employee emp = new Employee(companyId, empId);
                entity = payHistory.ExecuteProcessTemp(companyId, empId, year, month, new Guid(entitmap.EntityId), new Guid(entitmap.EntityTableName), out payErrors);
            }
            List<jsonFullFinalMonthlyComponent> retData = new List<jsonFullFinalMonthlyComponent>();
            entity.EntityAttributeModelList.ForEach(u =>
            {
                if (u.AttributeModel.FullAndFinalSettlement || u.AttributeModel.IsMonthlyInput)
                {
                    retData.Add(new jsonFullFinalMonthlyComponent() { componentVal = u.EntityAttributeValue.Value, id = u.Id, name = u.AttributeModel.Name });
                }

            });
            return base.BuildJson(true, 200, "success", retData);
        }
        public JsonResult DeleteFFProcess(Guid empid,bool includeTax)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            FullFinalSettlement ff = new FullFinalSettlement(Guid.Empty, empid);
            ff.EmployeeId = empid;
            PayrollHistory payhistory = new PayrollHistory(companyId, empid, ff.ApplyYear, ff.ApplyMonth);

            LoanEntryList loanEntrylist = new LoanEntryList(empid, Guid.Empty);
            // var ChkLoanEntry = loanEntrylist.Where(u => u.ApplyDate.Year <= ff.ApplyYear && u.ApplyDate.Month <= ff.ApplyMonth).ToList();

            loanEntrylist.ForEach(u =>
            {
                LoanTransactionList loanTransactionList = new LoanTransactionList(u.Id);
                var ChkLoanTransaction = loanTransactionList.Where(v => v.isFandFProcessv == true).ToList();
                ChkLoanTransaction.ForEach(x =>
                {
                    LoanTransaction loanTransaction = new LoanTransaction(x.Id, x.LoanEntryId);
                    loanTransaction.Status = "UnPaid";
                    loanTransaction.isPayRollProcess = false;
                    loanTransaction.isFandFProcessv = false;
                    //loanTransaction.Delete();
                    loanTransaction.Save();
                });
            });
            TaxHistory txHistory = new TaxHistory();
            TXFinanceYear fin = new TXFinanceYear(Guid.Empty, companyId, true);
            if (includeTax == true)
            {

                txHistory.EmployeeId = empid;
                txHistory.FinanceYearId = fin.Id;
                txHistory.ApplyDate = Convert.ToDateTime("1/" + ff.ApplyMonth + "/" + ff.ApplyYear, new System.Globalization.CultureInfo("en-GB"));
                txHistory.Delete();
                txHistory.DeleteAP();
            }
            string prname = "F&F";
            //   payhistory.Delete();
            payhistory.DeletePayrollProcess(companyId, empid, ff.ApplyYear, ff.ApplyMonth, 0,prname);
            ff.Delete();

            return base.BuildJson(true, 200, "Deleted Successfully", null);

        }
        public JsonResult SaveFullAndFinal(jsonFullfinalSettlement dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            FullFinalSettlement t1 = new FullFinalSettlement();
            isSaved = SaveFullFinal(dataValue, out t1);
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        private bool SaveFullFinal(jsonFullfinalSettlement dataValue, out FullFinalSettlement fullFinalSettlement)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            fullFinalSettlement = jsonFullfinalSettlement.convertobject(dataValue);
            fullFinalSettlement.CreatedBy = userId;
            fullFinalSettlement.ModifiedBy = userId;
            return fullFinalSettlement.Save();
        }

        public JsonResult AddFullAndFinal(List<jsonFullFinalMonthlyComponent> dataValue, jsonFullfinalSettlement data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (dataValue.Count > 0)
            {
                FullFinalSettlement t1 = new FullFinalSettlement();
                if (SaveFullFinal(data, out t1))
                {
                    //FullFinalSettlement full = new FullFinalSettlement(Guid.Empty, dataValue[0)
                    dataValue.ForEach(u =>
                    {
                        FullFinalSettlementDetail tmp = new FullFinalSettlementDetail();
                        tmp.Amount = Convert.ToDecimal(u.componentVal);
                        tmp.AttributeModelId = u.id;
                        tmp.CreatedBy = userId;
                        tmp.FullFinalSettlementId = t1.Id;
                        tmp.TaxAmount = 0;
                        tmp.Year = u.year;
                        tmp.Month = u.month;
                        isSaved = tmp.Save();

                    });
                }
            }

            if (isSaved)
            {
                return GetFullAndFinalData(data);
                // return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        #region "Credit Days"

        public JsonResult SaveCreditDays(List<jsonCreditDays> dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool result = true;
            var EmpName = "";
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelList AttrModelList = new AttributeModelList(companyId);
            var AttrModel_PD = AttrModelList.Where(u => u.Name == "PD").FirstOrDefault();
            var AttrModel_LD = AttrModelList.Where(u => u.Name == "LD").FirstOrDefault();
            TableCategoryList tabCatList = new TableCategoryList(companyId);
            var tabCat = tabCatList.Where(u => u.Name == "Payroll").FirstOrDefault();
            EntityModelList entiModelList = new EntityModelList(tabCat.Id);
            var entityModel = entiModelList.Where(u => u.Name == "Salary").FirstOrDefault();
            EntityModelMappingList entitymodelMappinglist = new EntityModelMappingList("Employee", companyId);
            var ent = entitymodelMappinglist.Where(u => Convert.ToString(u.EntityTableName).ToUpper() == Convert.ToString(entityModel.Id).ToUpper()).FirstOrDefault();
            if (dataValue != null)
            {
                dataValue.ForEach(d =>
                {

                    EntityMappingList entitymappinglist = new EntityMappingList(ent.RefEntityModelName);
                    var EmployeeEntity = entitymappinglist.Where(r => new Guid(r.RefEntityId) == d.employeeId).FirstOrDefault();
                    if (EmployeeEntity == null)
                    {
                        EmployeeList EmpList = new EmployeeList(companyId, userId, d.employeeId);
                        var emp = EmpList.Where(k => k.Id == d.employeeId).FirstOrDefault();
                        EmpName = emp.EmployeeCode;
                        result = false;
                    }
                    EntityMapping entityMap = new EntityMapping();
                    CreditDays creditday = jsonCreditDays.convertobject(d);
                    creditday.CompanyId = companyId;
                    creditday.CreatedBy = userId;
                    creditday.ModifiedBy = userId;
                    creditday.IsDeleted = false;
                    creditday.IsProcessed = false;
                    string Month_N = d.month;
                    var Month = DateTime.ParseExact(Month_N, "MMMM", CultureInfo.CurrentCulture).Month;

                    if (!creditday.Save())
                    {
                        result = false;
                    }
                    MonthlyInputList MIList = new MonthlyInputList(new Guid(EmployeeEntity.EntityId), d.employeeId, Month, d.year);
                    MonthlyInput MIlist = new MonthlyInput();
                    MIlist = MIList.Where(s => s.EmployeeId == d.employeeId).FirstOrDefault();
                    var AttrPD_ID = MIList.Where(s => s.EmployeeId == d.employeeId && s.AttributeModelId == AttrModel_PD.Id).FirstOrDefault();
                    var AttrLD_ID = MIList.Where(s => s.EmployeeId == d.employeeId && s.AttributeModelId == AttrModel_LD.Id).FirstOrDefault();
                    if (d.type == "Supp" && EmployeeEntity != null)
                    {


                        if (AttrModel_PD.IsMonthlyInput == true)
                        {
                            MonthlyInput MI = new MonthlyInput();
                            MI.AttributeModelId = AttrModel_PD.Id;
                            MI.Id = AttrPD_ID == null ? Guid.Empty : AttrPD_ID.Id;
                            MI.EmployeeId = d.employeeId;
                            MI.Month = Month;
                            MI.Year = d.year;
                            MI.EntityModelId = entityModel.Id;
                            MI.EntityId = new Guid(EmployeeEntity.EntityId);
                            MI.Value = Convert.ToString(d.paidDays);
                            MI.Save();
                        }

                        if (AttrModel_LD.IsMonthlyInput == true)
                        {
                            MonthlyInput MI = new MonthlyInput();
                            MI.AttributeModelId = AttrModel_LD.Id;
                            MI.Id = AttrLD_ID == null ? Guid.Empty : AttrLD_ID.Id;
                            MI.EmployeeId = d.employeeId;
                            MI.Month = Month;
                            MI.Year = d.year;
                            MI.EntityModelId = entityModel.Id;
                            MI.EntityId = new Guid(EmployeeEntity.EntityId);
                            MI.Value = Convert.ToString(d.lopDays);
                            MI.Save();
                        }
                    }
                });
            }

            if (result)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else if (result == false && EmpName != "")
            {
                return base.BuildJson(false, 100, "Salary Not Mapped for" + EmpName + ".", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }




        }
        public JsonResult getLOPCreditDays(jsonCreditDays dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PremiumSetting premium = new PremiumSetting(companyId, "LOP Credit Setting");
            List<jsonCreditDays> jresult = new List<jsonCreditDays>();
            CreditDaysList Creditlist = new CreditDaysList();
            if (premium.BackMonth > 0)
            {
                MonthlyInputList result = new MonthlyInputList();
                DateTime sdate = new DateTime(dataValue.applyYear, dataValue.applyMonth, 1);
                for (int smon = 1; smon <= premium.BackMonth; smon++)
                {
                    // sdate = sdate.AddMonths(-1 * smon);
                    sdate = sdate.AddMonths(-1);
                    int month = sdate.Month;
                    int year = sdate.Year;
                    MonthlyInputList mlist = new MonthlyInputList(Guid.Empty, dataValue.employeeId, month, year);

                    result.AddRange(mlist.Where(d => d.Attributemodel.Name == "LD" && Convert.ToInt32(d.Value) > 0));

                    CreditDaysList LOPcreditlist = new CreditDaysList(companyId, month, year, dataValue.employeeId, "LOP");
                    if (LOPcreditlist.Count > 0)
                    {
                        Creditlist.Add(LOPcreditlist.FirstOrDefault());
                    }

                }
                if (result.Count > 0)
                {
                    result.ForEach(r =>
                    {
                        PayrollHistory PayrolHisval = new PayrollHistory(companyId, r.EmployeeId, r.Year, r.Month);
                        if (PayrolHisval != null && PayrolHisval.Status == "Processed")
                        {
                            if (Creditlist.Count > 0)
                            {
                                Creditlist.ForEach(k =>
                                {
                                    if (k.Month == r.Month)
                                    {
                                        int currLOPvalue = Convert.ToInt32(r.Value) - Convert.ToInt32(k.PaidDays);
                                        r.Value = currLOPvalue.ToString();
                                    }
                                });
                            }
                            if (Convert.ToInt32(r.Value) > 0)
                            {
                                jresult.Add(jsonCreditDays.tojson(new CreditDays
                                {
                                    CompanyId = companyId,
                                    Month = r.Month,
                                    Year = r.Year,
                                    EmployeeId = r.EmployeeId,
                                    LopDays = Convert.ToInt32(r.Value),
                                    ApplyMonth = Convert.ToInt32(dataValue.applyMonth),
                                    ApplyYear = dataValue.applyYear,

                                }));
                            }
                        }
                    });
                }
                CreditDaysList credits = new CreditDaysList(companyId, dataValue.applyMonth, dataValue.applyYear, dataValue.employeeId, "LOP");
                if (!object.ReferenceEquals(credits, null))
                {
                    jresult.ForEach(r =>
                    {
                        if (credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year).Any())
                        {
                            r.id = credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year).FirstOrDefault().Id;
                            r.paidDays = credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year).FirstOrDefault().PaidDays;
                        }
                    });


                }
            }

            return base.BuildJson(true, 2, "", jresult);

        }
        public JsonResult getSupplementaryDays(jsonCreditDays dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PremiumSetting premium = new PremiumSetting(companyId, "Supplementary Setting");
            List<jsonCreditDays> jresult = new List<jsonCreditDays>();
            EmployeeList emplist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            PayrollHistoryList payrollHistoryList = new PayrollHistoryList(dataValue.employeeId, companyId);
            if (premium.BackMonth > 0)
            {
                MonthlyInputList result = new MonthlyInputList();
                DateTime sdate = new DateTime(dataValue.applyYear, dataValue.applyMonth, 1);
                for (int smon = 1; smon <= premium.BackMonth; smon++)
                {
                    //sdate = sdate.AddMonths(-1 * smon);
                    sdate = sdate.AddMonths(-1);
                    int cmonth = sdate.Month;
                    int year = sdate.Year;

                    MonthlyInputList mlist = new MonthlyInputList(Guid.Empty, dataValue.employeeId, cmonth, year);

                    // result.AddRange(mlist.Where(d => d.Attributemodel.Name == "LD" && Convert.ToInt32(d.Value) > 0));

                    emplist.Where(d => d.DateOfJoining.Month == cmonth && d.DateOfJoining.Year == year).ToList().ForEach(e =>
                      {
                          string DOJ = e.DateOfJoining.ToString("dd/MMM/yyyy");
                          string[] DOJdetail = DOJ.Replace('-', '/').Split('/');
                          int monthInDigit = DateTime.ParseExact(DOJdetail[1], "MMM", CultureInfo.InvariantCulture).Month;
                          var payrolllist = payrollHistoryList.Where(ph => ph.Month == monthInDigit && ph.Year == Convert.ToInt16(DOJdetail[2])
                                            && ph.EmployeeId == e.Id && ph.IsDeleted == false).FirstOrDefault();
                          if (payrolllist == null)
                          {
                              jresult.Add(new jsonCreditDays
                              {
                                  companyId = companyId,
                                  month = ((MonthEnum)cmonth).ToString(),
                                  year = year,
                                  empCode = e.EmployeeCode,
                                  employeename = e.FirstName,
                                  employeeId = e.Id,
                                  dateofjoining = e.DateOfJoining.ToString("dd/MMM/yyyy"),
                                  paidDays = (DateTime.DaysInMonth(year, cmonth)) - e.DateOfJoining.Day + 1,// (Ex : March,DateOfJoining 10th --> 31-10 = 21 + 1 [include for joining date]) Modified by benny as on 17-Nov-2017 for Supplementary days issue
                                  applyMonth = Convert.ToInt32(dataValue.applyMonth),
                                  applyYear = dataValue.applyYear

                              });
                          }

                      });
                }
                CreditDaysList credits = new CreditDaysList(companyId, dataValue.applyMonth, dataValue.applyYear, Guid.Empty, "Supp");
                if (!object.ReferenceEquals(credits, null))
                {
                    jresult.ForEach(r =>
                    {
                        if (credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year && e.EmployeeId == r.employeeId).Any())
                        {
                            r.id = credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year && e.EmployeeId == r.employeeId).FirstOrDefault().Id;
                            r.paidDays = credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year && e.EmployeeId == r.employeeId).FirstOrDefault().PaidDays;
                            r.lopDays = credits.Where(e => e.ApplyMonth == r.applyMonth && e.ApplyYear == r.applyYear && ((MonthEnum)e.Month).ToString() == r.month && e.Year == r.year && e.EmployeeId == r.employeeId).FirstOrDefault().LopDays;
                        }
                    });


                }
            }

            return base.BuildJson(true, 2, "", jresult);

        }

        public JsonResult GetSettlementOutput(jsonFullfinalSettlement dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int userId = Convert.ToInt32(Session["UserId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var ckeditorheader = string.Empty;
            DataTable fsList = new DataTable();
            List<ffHeader> ffHeader = new List<ffHeader>();
            FullFinalSettlement fs = new FullFinalSettlement(dataValue.Fafid, dataValue.FafEmpId);
            FullFinalSettlementDetailList fsd = new FullFinalSettlementDetailList(dataValue.Fafid);
            PaySlipList setting = new PaySlipList();
            PayrollHistory pay = new PayrollBO.PayrollHistory(companyId, dataValue.FafEmpId, fs.LastWorkingDate.Year, fs.LastWorkingDate.Month);
            PayrollHistoryValueList payValue = new PayrollHistoryValueList(pay.Id);
            List<PayrollHistoryValueList> payValueList = new List<PayrollHistoryValueList>();

            AttributeModelList att = new AttributeModelList(companyId);
            List<AttributeModel> attlist = att.Where(w => w.IsIncludeForGrossPay == true || w.FullAndFinalSettlement == true ||
            w.Name == "PD" || w.Name == "LD" || w.Name == "MD" || w.Name == "EG" || w.Name == "TOTDED" || w.Name == "NETPAY").ToList();

            //  List<AttributeModel> daysattlist = att.Where(w => w.Name == "PD" || w.Name == "LD" || w.Name == "MD").ToList();
            payValueList.Add(payValue);

            fsList.Columns.Add("TYPE", typeof(string));
            fsList.Columns.Add("NAME", typeof(string));
            fsList.Columns.Add("AMOUNT", typeof(string));
            fsList.Columns.Add("MONTH", typeof(int));
            fsList.Columns.Add("YEAR", typeof(int));
            fsList.Columns.Add("DISPLAYORDER", typeof(string));
            fsList.Columns.Add("DISPLAYORDERSETTING", typeof(int));

            StopPaymentList stPayment = new StopPaymentList(dataValue.FafEmpId);

            stPayment.Where(w => w.StopPaymentType == 1).ToList().ForEach(f =>
                {

                    PayrollHistory payHist = new PayrollBO.PayrollHistory(companyId, dataValue.FafEmpId, f.StopPaymentYear, f.StopPaymentMonth);
                    PayrollHistoryValueList payValues = new PayrollHistoryValueList(payHist.Id);
                    payValueList.Add(payValues);
                });


            Employee emp = new Employee(companyId, dataValue.FafEmpId);
            //  List<Object> netpay = new List<Object>();
            string Netpay = ""; string Netamount = "";
            payValueList.ForEach(pv =>
            {
                if (pv.Count > 0)
                {
                    PayrollHistory ph = new PayrollHistory(pv[0].PayrollHistoryId, companyId);
                    PayrollHistoryValueList filteredPayValue = new PayrollHistoryValueList();


                    //filter by payslipSetting
                    DataTable dtCategories = new PaySlip().GetSetting("'" + emp.CategoryId.ToString() + "'");
                    if (dtCategories.Rows.Count > 0)
                    {
                        DataRow drCat = dtCategories.Rows[0];
                        setting = new PaySlipList(new Guid(drCat["CofigurationId"].ToString()));
                        PaySlipSetting ps = new PaySlipSetting(new Guid(drCat["CofigurationId"].ToString()));
                        ckeditorheader = ps.Header;
                        if (setting.Count > 0)
                        {
                            setting.Where(w => w.Section == "Earnings" || w.Section == "Deductions").ToList().ForEach(f =>
                            {
                                var val = pv.Where(w => w.AttributeModelId == new Guid(f.FieldName)).FirstOrDefault();
                                if (!Object.ReferenceEquals(val, null))
                                {
                                    filteredPayValue.Add(val);
                                }

                            });
                        }
                    }
                    //filter by include for grosspay or full and final settlement,days,total earnings and totaldeduction,netpay
                    if (attlist.Count > 0)
                    {
                        attlist.OrderBy(o => o.Name).ToList().ForEach(f =>
                        {

                            var alreadyExist = filteredPayValue.Where(w => w.AttributeModelId == f.Id).FirstOrDefault();
                            if (Object.ReferenceEquals(alreadyExist, null))
                            {
                                var val = pv.Where(w => w.AttributeModelId == f.Id).FirstOrDefault();
                                if (!Object.ReferenceEquals(val, null))
                                {
                                    filteredPayValue.Add(val);
                                }
                            }
                        });

                    }








                    filteredPayValue.ForEach(f =>
                    {

                        AttributeModel curAttr = att.Where(am=>am.Id ==f.AttributeModelId).FirstOrDefault();
                        if (!ReferenceEquals(curAttr, null))
                        {
                            var insetting = setting.Where(w => w.FieldName == curAttr.Id.ToString()).FirstOrDefault();
                            if ((curAttr.Name == "PD" || curAttr.Name == "LD" || curAttr.Name == "MD") ||
                            ((curAttr.IsIncludeForGrossPay == true && object.ReferenceEquals(insetting, null)) ||
                            (curAttr.FullAndFinalSettlement == true && object.ReferenceEquals(insetting, null))))
                            {
                                fsList.Rows.Add(
                                    curAttr.BehaviorType,
                                    curAttr.DisplayAs,
                                    Convert.ToString(f.Value),
                                    ph.Month,
                                    ph.Year,
                                    "A",
                                    0);
                            }
                            else if (curAttr.Name == "EG" || curAttr.Name == "TOTDED")
                            {
                                fsList.Rows.Add(
                                    curAttr.BehaviorType,
                                    curAttr.Name == "EG" ? "Total Earnings" : "Total Deduction",
                                    Convert.ToString(f.Value),
                                    ph.Month,
                                    ph.Year,
                                    "C",
                                    100);
                            }

                            else if (curAttr.Name == "NETPAY")
                            {
                                Netpay = curAttr.DisplayAs;
                                if (Netamount == "")
                                {
                                    Netamount = "0";
                                }
                                Netamount = Convert.ToString(Convert.ToInt32(Netamount) + Convert.ToInt32(f.Value));

                                fsList.Rows.Add(
                                    "  ",
                                    "NETPAY",
                                    Convert.ToString(f.Value),
                                    ph.Month,
                                    ph.Year,
                                    "D",
                                    100);
                            }
                            else
                            {
                                int order = setting.Where(w => w.FieldName == curAttr.Id.ToString()).FirstOrDefault().DisplayOrder;
                                fsList.Rows.Add(
                                    curAttr.BehaviorType,
                                    curAttr.DisplayAs,
                                    Convert.ToString(f.Value),
                                    ph.Month,
                                    ph.Year,
                                    "B",
                                    order);
                            }
                        }

                    });
                }
            });
            Company compDetails = new Company(companyId);
            CategoryList categoryList = new CategoryList(companyId);
            DepartmentList deptList = new DepartmentList(companyId);
            BranchList branchList = new BranchList(companyId);
            CostCentreList costCentreList = new CostCentreList(companyId);
            DesignationList desgntionList = new DesignationList(companyId);
            PTLocationList ptLocList = new PTLocationList(companyId);
            GradeList gradeList = new GradeList(companyId);
            ESIDespensaryList esiDespen = new ESIDespensaryList(companyId);
            LocationList locationList = new LocationList(companyId);
            EntityModel entityModel = new EntityModel("AddtionalInfo", companyId);
            BankList banklist = new BankList(companyId);
            Emp_BankList empbank = new Emp_BankList(emp.Id);
            EmployeeAddressList empaddr = new EmployeeAddressList(emp.Id);
            Emp_Personal emppersonal = new Emp_Personal(emp.Id);
            EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModel.Id, emp.Id);



            setting.Where(w => w.Section == "FandFHeader").OrderBy(o => o.DisplayOrder).ToList().ForEach(r =>
              {
                  if(r.DisplayAs== "PF ACCOUNT NO")
                  {

                  }
                  if (r.DisplayAs == "EmployeeServiceYear")
                  {
                      r.FieldName = r.DisplayAs;
                      r.Value1 = emp.NoOfServiceYear.ToString();
                  }
                  char[] splitchar = { ' ' };
                  string[] split = r.FieldName.Split(splitchar);
                  //Assign Master Values from Physical Table
                  switch (r.TableName.ToLower())
                  {
                      case "employee":


                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }

                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                          var empDetails = emp.GetType().GetProperty(r.FieldName).GetValue(emp, null);
                          r.Value1 = Convert.ToString(empDetails) != null ? Convert.ToString(empDetails) : "";
                          // r.Value1 = e.GetType().GetProperty(r.FieldName).GetValue(e, null).ToString();
                          if (emp.GetType().GetProperty(r.FieldName).PropertyType.Name == "Guid")
                          {
                              if (new Guid(r.Value1) != Guid.Empty)
                              {
                                  switch (r.FieldName)
                                  {
                                      case "CategoryId":
                                      if(!object.ReferenceEquals(categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                          }      
                                          break;
                                      case "Category":
                                          if (!object.ReferenceEquals(categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                          }
                                            
                                          break;
                                      case "Department":
                                          if (!object.ReferenceEquals(deptList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = deptList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DepartmentName;
                                          }

                                             
                                          break;
                                      case "Branch":
                                          if (!object.ReferenceEquals(branchList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = branchList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().BranchName;
                                          }
                                         
                                          break;
                                      case "Designation":
                                          if (!object.ReferenceEquals(desgntionList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = desgntionList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DesignationName;
                                          }
                                       
                                          break;
                                      case "CostCentre":
                                          if (!object.ReferenceEquals(costCentreList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = costCentreList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().CostCentreName;
                                          }
                                         
                                          break;
                                      case "Grade":
                                          if (!object.ReferenceEquals(gradeList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = gradeList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().GradeName;
                                          }
                                          
                                          break;
                                      case "PTLocation":
                                          if (!object.ReferenceEquals(ptLocList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = ptLocList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().PTLocationName;
                                          }
                                         
                                          break;
                                      case "ESIDespensary":
                                          if (!object.ReferenceEquals(esiDespen.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = esiDespen.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().ESIDespensary;
                                          }
                                        
                                          break;
                                      case "Location":
                                          if (!object.ReferenceEquals(locationList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault(), null))
                                          {
                                          r.Value1 = locationList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().LocationName;
                                          }
                                          
                                          break;
                                  }
                              }
                              else
                              {
                                  r.Value1 = "";
                              }
                          }
                          else if (emp.GetType().GetProperty(r.FieldName).PropertyType.Name.ToUpper() == "DATETIME")
                          {
                              r.Value1 = Convert.ToDateTime(r.Value1).ToString("dd/MMM/yyyy");
                          }


                          break;
                      case "emp_bank":


                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }

                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                          if (empbank.Count > 0)
                          {
                              if (r.FieldName != "BankName")
                              {
                                  var bankDetails = empbank[0].GetType().GetProperty(r.FieldName).GetValue(empbank[0], null);
                                  r.Value1 = Convert.ToString(bankDetails) != null ? Convert.ToString(bankDetails) : "";
                              }


                              switch (r.FieldName)
                              {
                                  case "BankName":
                                      var bankofemp = banklist.Where(d => d.Id == empbank[0].BankId).FirstOrDefault();
                                      if (!object.ReferenceEquals(bankofemp, null))
                                      {
                                      r.Value1 = banklist.Where(d => d.Id == empbank[0].BankId).FirstOrDefault().BankName;
                                      }
                                     
                                      break;
                              }

                          }
                          else
                          {
                              r.Value1 = string.Empty;
                          }

                          break;
                      case "emp_personal":


                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }


                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;

                          if (r.FieldName == "BloodGroup")
                          {
                              r.Value1 = new BloodGroup(Convert.ToInt32(r.Value1)).BloodGroupName;
                          }
                          else
                          {
                              var OtherDetail = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null);
                              r.Value1 = Convert.ToString(OtherDetail) != null ? Convert.ToString(OtherDetail) : "";
                              // r.Value1 = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null).ToString();
                              if (r.FieldName == "PFNumber")
                              {
                                  r.Value1 = string.IsNullOrEmpty(r.Value1) ? "" : compDetails.PFEmployeerCode + (r.Value1);
                              }
                          }
                          if (emppersonal.GetType().GetProperty(r.FieldName).PropertyType.Name.ToUpper() == "DATETIME")
                          {
                              r.Value1 = Convert.ToDateTime(r.Value1).ToString("dd/MMM/yyyy");
                          }
                          break;
                      case "emp_address":


                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }

                          if (empaddr.Count > 0)
                          {
                              r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                              var AddressDetail = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null);
                              r.Value1 = Convert.ToString(AddressDetail) != null ? Convert.ToString(AddressDetail) : "";
                              //r.Value1 = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null).ToString();
                          }
                          break;
                      case "additionalinfo":


                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }

                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                          if (empAddInfoList.Count > 0)
                          {
                              for (int cnt = 0; cnt < empAddInfoList.Count; cnt++)
                              {

                                  if (r.FieldName == empAddInfoList[cnt].AttributeModelId.ToString())
                                  {

                                      AttributeModel at = att.Where(aml=>aml.Id == empAddInfoList[cnt].AttributeModelId).FirstOrDefault();
                                      r.Value1 = empAddInfoList[cnt].Value;
                                      if (!ReferenceEquals(at, null))
                                      {
                                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? at.DisplayAs : r.DisplayAs;
                                      }

                                  }
                              }
                          }
                          break;
                      case "category":

                          if (split.Count() > 1)
                          {
                              r.FieldName = split[1];
                          }
                          r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                          var empDetails1 = emp.CategoryId;
                          r.Value1 = Convert.ToString(empDetails1) != null ? Convert.ToString(empDetails1) : "";
                          switch (r.FieldName)
                          {
                              case "CategoryId":
                                  var catId = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault();
                                  if (!object.ReferenceEquals(catId, null))
                                  {
                                  r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                  }                                 
                                  break;
                              case "Category":
                                var cat = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault();
                                  if (!object.ReferenceEquals(cat, null))
                                  {
                                  r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                  }
                                  break;
                          }
                          break;
                  }
                  //Assign payroll History

                  ffHeader.Add(new ffHeader()
                  {
                      Name = r.DisplayAs,
                      Value = r.Value1,
                      column = 0
                  });

              });

            ffHeader.Add(new ffHeader()
            {
                Name = "Relieving Date",
                Value = fs.RelievingDate.ToString("dd/MMM/yyyy"),
                column = 0
            });
            ffHeader.Add(new ffHeader()
            {
                Name = "Settlement Date",
                Value = fs.SettlementDate.ToString("dd/MMM/yyyy"),
                column = 0
            });

            int remainder = ffHeader.Count() % 3;

            int quotient = ffHeader.Count() / 3;
            int cl = 1; int rw = 0;
            ffHeader.ToList().ForEach(f =>
            {
                if (ffHeader.Count <= 3)
                {
                    f.column = cl;
                    cl++;
                }
                else
                {
                    rw++;

                    f.column = cl;
                    if (rw == (remainder + quotient) || (rw == ((remainder + quotient) * 2)))
                    {
                        cl++;
                    }

                }


            });

            //fsList.Add(new
            //{
            //    TYPE = "Earning",
            //    NAME = " ",
            //    AMOUNT = " ",
            //    MONTH = 13,
            //    DISPLAYORDER ="A",
            //    DISPLAYORDERSETTING = 100
            //});
            //fsList.Add(new
            //{

            //    TYPE = "Earning",
            //    NAME = " ",
            //    AMOUNT = " ",
            //    MONTH = 14,
            //    DISPLAYORDER = "A",
            //    DISPLAYORDERSETTING = 100
            //});


            //("EmpCode", emp.EmployeeCode,
            //            EmpName = emp.FirstName,
            //            JoiningDate = emp.DateOfJoining,
            //            Department = emp.Department,
            //            Designation = emp.Designation,
            //            RelievingDate = fs.RelievingDate,
            //            SeperationDate = fs.SettlementDate,
            //            SettlementYear = fs.SettlementDate,
            //            ServiceYear = (fs.RelievingDate.Year - emp.DateOfJoining.Year),


            Company comp = new Company(companyId, userId);
            string title = string.Empty;
            title = comp.CompanyName + "*";
            if (comp.AddressLine1.Trim() != string.Empty)
            {
                title = title + comp.AddressLine1.Trim() + ",";
            }
            if (comp.AddressLine2.Trim() != string.Empty)
            {
                title = title + comp.AddressLine2.Trim() + ",";
            }
            if (comp.City.Trim() != string.Empty)
            {
                title = title + comp.City.Trim() + ",";
            }
            title = title.TrimEnd(',');
            //title = title + "**" + "PAYSLIP FOR THE MONTH OF " + ((MonthEnum)Convert.ToInt16(dataValue.FafapplyMonth)).ToString().ToUpper() + " - " + dataValue.FafapplyYear;

            // this condition added to show 2 blank columns if it is blank 
            int rows = stPayment.Where(s => s.StopPaymentType == 1).ToList().Count();
            for (int k=-1;k<(1-rows);k++)
            {
              fsList.Rows.Add(
               string.Empty,
               "Total Earnings",
               "0",
               k,
               0,
               "C",
               100);
            }

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
            rpt.ReportPath = "Reports/FFSettlement.rdlc";
            string month = new DateTimeFormatInfo().GetMonthName(fs.SettlementDate.Month);
            month = month.ToUpper();
            string rpt1 = "FULL AND FINAL SETTLEMENT FOR THE MONTH OF " +  month + "  " + fs.SettlementDate.Year;
            ReportDataSource rptDs = new ReportDataSource("DSFFS", fsList);
            ReportDataSource rptD = new ReportDataSource("DSHeader", ffHeader);
            rpt.DataSources.Add(rptDs);
            rpt.DataSources.Add(rptD);
            ReportParameterCollection rpc = new ReportParameterCollection();
            rpc.Add(new ReportParameter("EmpCode", emp.EmployeeCode));
            rpc.Add(new ReportParameter("EmpName", emp.FirstName + " " + emp.LastName));
            rpc.Add(new ReportParameter("ServiceYears", (System.Math.Round(emp.NoOfServiceYear, 1)).ToString()));
            rpc.Add(new ReportParameter("SettlementDate", fs.SettlementDate.ToString("dd/MMM/yyyy")));
            rpc.Add(new ReportParameter("SeparationDate", emp.SeparationDate.ToString("dd/MMM/yyyy")));
            rpc.Add(new ReportParameter("RelievingDate", fs.RelievingDate.ToString("dd/MMM/yyyy")));
            rpc.Add(new ReportParameter("Title", title));
            rpc.Add(new ReportParameter("Netpay", Netpay));
            rpc.Add(new ReportParameter("NetpayAmount", Netamount));
            rpc.Add(new ReportParameter("Notes", fs.Notes));
            rpc.Add(new ReportParameter("NoOfMonths", stPayment.Where(w => w.StopPaymentType == 1).ToList().Count().ToString()));
            rpc.Add(new ReportParameter("rptHead", rpt1));


            byte[] renderedBytes = null;
            rpt.SetParameters(rpc);
            renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string contentype = mimeType;

            string rptPath = DocumentProcessingSettings.TempDirectoryPath;
            rptPath = rptPath + "\\" + emp.EmployeeCode + "_FF_" + fs.ApplyMonth.ToString() + "_" + fs.ApplyYear.ToString() + ".pdf";
            using (FileStream ffs = System.IO.File.Create(rptPath))
            {
                ffs.Write(renderedBytes, 0, (int)renderedBytes.Length);
            }
            return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = rptPath });
        }
        #endregion

    }

    public class jsonIncrement
    {
        public Guid employeeId { get; set; }
        public Guid id { get; set; }

        public int month { get; set; }

        public int year { get; set; }
        public string effDate { get; set; }
        public double afterLop { get; set; }
        public double beforeLop { get; set; }
        public string status { get; set; }

        public List<jsonIncrementDetails> incrementDatails { get; set; }

        public static jsonIncrement toJson(Increment increment, AttributeModelList attrModels, bool isnewEntry = false)
        {
            jsonIncrement retobj = new jsonIncrement()
            {
                effDate = increment.EffectiveDate != DateTime.MinValue ? increment.EffectiveDate.ToString("dd/MMM/yyyy") : "",
                beforeLop = increment.BeforeLop,
                afterLop = increment.AfterLop,
                employeeId = increment.EmployeeId,
                month = increment.ApplyMonth,
                year = increment.ApplyYear,
                id = increment.Id,
                status = increment.IsProcessed == true ? "Processed" : "Not Processed",
                incrementDatails = new List<jsonIncrementDetails>()
            };
            increment.IncrementDetailList.ForEach(u =>
            {
                retobj.incrementDatails.Add(jsonIncrementDetails.toJson(u, attrModels, isnewEntry));
            });
            //  retobj.incrementDatails
            return retobj;
        }

        public static Increment convertObject(jsonIncrement increment)
        {
            Increment retobj = new Increment()
            {
                EffectiveDate = increment.effDate != string.Empty ? Convert.ToDateTime(increment.effDate) : DateTime.Now,
                BeforeLop = increment.beforeLop,
                AfterLop = increment.afterLop,
                EmployeeId = increment.employeeId,
                ApplyMonth = increment.month,
                ApplyYear = increment.year,
                Id = increment.id,
                IncrementDetailList = new IncrementDetailList()
            };
            if (!object.ReferenceEquals(increment.incrementDatails, null))
            {
                increment.incrementDatails.ForEach(u =>
                {
                    retobj.IncrementDetailList.Add(jsonIncrementDetails.convertObject(u));
                });
            }
            //  retobj.incrementDatails
            return retobj;
        }
    }

    public class jsonIncrementDetails
    {
        public Guid incrementId { get; set; }

        public Guid attributeModId { get; set; }

        public string attrModelName { get; set; }

        public decimal currentVal { get; set; }

        public decimal newVal { get; set; }

        public static jsonIncrementDetails toJson(IncrementDetail increment, AttributeModelList attrmodels, bool isnew)
        {
            var attr = attrmodels.Where(u => u.Id == increment.AttributeModelId).FirstOrDefault();

            jsonIncrementDetails retobj = new jsonIncrementDetails();
            if (attr != null)
            {
                retobj.attributeModId = increment.AttributeModelId;
                retobj.attrModelName = attr.Name;
                retobj.currentVal = isnew ? increment.NewValue : increment.OldValue;
                retobj.incrementId = increment.Id;
                retobj.newVal = increment.NewValue;

            }
            return retobj;
        }
        public static IncrementDetail convertObject(jsonIncrementDetails increment)
        {
            IncrementDetail retobj = new IncrementDetail();
            retobj.AttributeModelId = increment.attributeModId;
            retobj.NewValue = increment.newVal;
            retobj.OldValue = increment.currentVal;
            return retobj;
        }
    }

    public class jsonStopPayment
    {
        public Guid SPayid { get; set; }
        public Guid SPayEmpId { get; set; }
        public string SPayEmpName { get; set; }
        public int SPayMonth { get; set; }
        public int SPayYear { get; set; }
        public int SPayType { get; set; }
        public string SPayRemarks { get; set; }


        public static jsonStopPayment tojson(StopPayment stopPayment, EmployeeList employeeList)
        {
            return new jsonStopPayment()
            {
                SPayid = stopPayment.Id,
                SPayEmpId = stopPayment.EmployeeId,
                SPayMonth = stopPayment.StopPaymentMonth,
                SPayYear = stopPayment.StopPaymentYear,
                SPayType = stopPayment.StopPaymentType,
                SPayRemarks = stopPayment.Remarks,
                SPayEmpName = employeeList.Where(u => u.Id == stopPayment.EmployeeId).FirstOrDefault().FirstName,//loanName = loanmasterlist.Where(u => u.Id == loanEntry.LoanMasterId).FirstOrDefault().LoanDesc,

            };
        }
        public static StopPayment convertobject(jsonStopPayment stopPayment)
        {
            return new StopPayment()
            {
                Id = stopPayment.SPayid,
                EmployeeId = stopPayment.SPayEmpId,
                StopPaymentMonth = stopPayment.SPayMonth,
                StopPaymentYear = stopPayment.SPayYear,
                StopPaymentType = stopPayment.SPayType,
                Remarks = stopPayment.SPayRemarks
            };
        }

    }

    public class jsonFullfinalSettlement
    {
        public Guid Fafid { get; set; }
        public Guid FafEmpId { get; set; }
        public int FafapplyMonth { get; set; }
        public int FafapplyYear { get; set; }
        public String FafresignationDate { get; set; }
        public String FaflastWorkingDate { get; set; }
        public decimal Fafloanamount { get; set; }
        public String FafsettlementDate { get; set; }
        public String FafrelievingDate { get; set; }
        public String FafDateOfJoining { get; set; }
        public int FafnoticePeriodToBeServed { get; set; }
        public decimal FafsalaryDays { get; set; }
        public decimal FaflopDays { get; set; }
        public int FafmonthDays { get; set; }
        public bool FafisTax { get; set; }
        public String Fafnotes { get; set; }
        public String Fafreportname { get; set; }



        public List<jsonFullFinalDetails> FullFinalDetails { get; set; }

        public static jsonFullfinalSettlement tojson(FullFinalSettlement fullFinalSettlement, Employee employee)
        {
            jsonFullfinalSettlement retObj = new jsonFullfinalSettlement();

            retObj.Fafid = fullFinalSettlement.Id;
            retObj.FafEmpId = fullFinalSettlement.EmployeeId;
            retObj.FafapplyMonth = fullFinalSettlement.ApplyMonth;
            retObj.FafapplyYear = fullFinalSettlement.ApplyYear;
            retObj.FafresignationDate = fullFinalSettlement.ResignationDate != DateTime.MinValue ? fullFinalSettlement.ResignationDate.ToString("dd/MMM/yyyy") : "";//employee.SeparationDate.ToString("dd/MMM/yyyy");//fullFinalSettlement.ResignationDate != DateTime.MinValue ? fullFinalSettlement.ResignationDate.ToString("dd/MMM/yyyy") : "";
            retObj.FaflastWorkingDate = employee.LastWorkingDate.ToString("dd/MMM/yyyy");//fullFinalSettlement.LastWorkingDate != DateTime.MinValue ? fullFinalSettlement.LastWorkingDate.ToString("dd/MMM/yyyy") : "";
            retObj.FafDateOfJoining = employee.DateOfJoining.ToString("dd/MMM/yyyy");
            retObj.FafsettlementDate = fullFinalSettlement.SettlementDate != DateTime.MinValue ? fullFinalSettlement.SettlementDate.ToString("dd/MMM/yyyy") : "";
            retObj.FafrelievingDate = fullFinalSettlement.RelievingDate != DateTime.MinValue ? fullFinalSettlement.RelievingDate.ToString("dd/MMM/yyyy") : "";
            retObj.FafnoticePeriodToBeServed = fullFinalSettlement.NoticePeriodToBeServed;
            retObj.FafsalaryDays = fullFinalSettlement.SalaryDays;
            retObj.FaflopDays = fullFinalSettlement.LopDays;
            retObj.FafmonthDays = fullFinalSettlement.MonthDays;
            retObj.FafisTax = fullFinalSettlement.IsTax;
            retObj.Fafnotes = fullFinalSettlement.Notes;
            retObj.FullFinalDetails = jsonFullFinalDetails.toJson(fullFinalSettlement.FullFinalSettlementDetailList);
            return retObj;
        }
        public static FullFinalSettlement convertobject(jsonFullfinalSettlement fullFinalSettlement)
        {
            return new FullFinalSettlement()
            {
                Id = fullFinalSettlement.Fafid,
                EmployeeId = fullFinalSettlement.FafEmpId,
                ApplyMonth = fullFinalSettlement.FafapplyMonth,
                ApplyYear = fullFinalSettlement.FafapplyYear,
                ResignationDate = fullFinalSettlement.FafresignationDate != string.Empty ? Convert.ToDateTime(fullFinalSettlement.FafresignationDate) : DateTime.MinValue,
                LastWorkingDate = fullFinalSettlement.FaflastWorkingDate != string.Empty ? Convert.ToDateTime(fullFinalSettlement.FaflastWorkingDate) : DateTime.MinValue,
                SettlementDate = fullFinalSettlement.FafsettlementDate != string.Empty ? Convert.ToDateTime(fullFinalSettlement.FafsettlementDate) : DateTime.MinValue,
                RelievingDate = fullFinalSettlement.FafrelievingDate != string.Empty ? Convert.ToDateTime(fullFinalSettlement.FafrelievingDate) : DateTime.MinValue,
                NoticePeriodToBeServed = fullFinalSettlement.FafnoticePeriodToBeServed,
                SalaryDays = fullFinalSettlement.FafsalaryDays,
                LopDays = fullFinalSettlement.FaflopDays,
                MonthDays = fullFinalSettlement.FafmonthDays,
                IsTax = fullFinalSettlement.FafisTax,
                Notes = fullFinalSettlement.Fafnotes
            };
        }

    }

    public class jsonFullFinalDetails
    {

        public int payMonth { get; set; }

        public int payYear { get; set; }

        public decimal gross { get; set; }

        public decimal tds { get; set; }

        public decimal netPay { get; set; }

        public static List<jsonFullFinalDetails> toJson(FullFinalSettlementDetailList fullFinalinput)
        {
            List<jsonFullFinalDetails> ret = new List<jsonFullFinalDetails>();
            fullFinalinput.ForEach(u =>
            {
                var tmp = ret.Where(p => p.payMonth == u.Month && p.payYear == u.Year).FirstOrDefault();

                if (object.ReferenceEquals(tmp, null))
                {
                    ret.Add(new jsonFullFinalDetails() { gross = u.Amount, payMonth = u.Month, payYear = u.Year, tds = 0 });
                }
                else
                {
                    ret.Where(q => q.payMonth == u.Month && q.payYear == u.Year).FirstOrDefault().gross = tmp.gross + u.Amount;
                }

            });
            ret.ForEach(u =>
            {
                u.netPay = u.gross - u.tds;
            });
            return ret;
        }
    }

    public class jsonFullFinalMonthlyComponent
    {

        public Guid id { get; set; }

        public string name { get; set; }

        public string componentVal { get; set; }

        public int month { get; set; }

        public int year { get; set; }
    }
    public struct jsonArrearViewlist
    {
        public string EmployeeCode;
        public int month;
        public int year;
        public string Attribute;
        public String Value;

    }

    public class ffHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int column { get; set; }
    }
    public class jsonCreditDays
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public Guid employeeId { get; set; }
        public int applyMonth { get; set; }
        public int applyYear { get; set; }
        public string month { get; set; }

        public int year { get; set; }
        public string type { get; set; }
        public int paidDays { get; set; }
        public int lopDays { get; set; }
        public string empCode { get; set; }
        public string employeename { get; set; }
        public string dateofjoining { get; set; }
        public int creditdays { get; set; }


        public static jsonCreditDays tojson(CreditDays creditDays)
        {
            creditDays.Employee.CompanyId = creditDays.CompanyId;
            return new jsonCreditDays()
            {
                id = creditDays.Id,
                employeeId = creditDays.EmployeeId,
                applyMonth = creditDays.ApplyMonth,
                applyYear = creditDays.ApplyYear,
                month = ((MonthEnum)creditDays.Month).ToString(),
                year = creditDays.Year,
                lopDays = creditDays.LopDays,
                paidDays = creditDays.PaidDays,
                employeename = creditDays.Employee.FirstName,
                dateofjoining = creditDays.Employee.DateOfJoining.ToString("dd-MMM-yyyy"),
                type = creditDays.CType

            };
        }
        public static CreditDays convertobject(jsonCreditDays creditDays)
        {
            int imonth;
            return new CreditDays()
            {
                Id = creditDays.id,
                EmployeeId = creditDays.employeeId,
                ApplyMonth = creditDays.applyMonth,
                ApplyYear = creditDays.applyYear,
                Month = Int32.TryParse(creditDays.month, out imonth) ? Convert.ToInt32(creditDays.month) : Enum.GetNames(typeof(MonthEnum)).AsEnumerable().ToList().IndexOf(creditDays.month) + 1,
                Year = creditDays.year,
                LopDays = creditDays.lopDays,
                PaidDays = creditDays.paidDays,
                CType = creditDays.type

            };
        }

    }

}