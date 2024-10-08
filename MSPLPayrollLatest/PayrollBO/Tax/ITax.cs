using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PayrollBO
{
    class ITax
    {
        public static void ComputeTax(TaxComputationInfo taxInfo)
        {
            //Need to validate Inputs
            Activity itax = new TaxComputationActivity();
            Dictionary<string, object> taxArgument = new Dictionary<string, object>();
            taxArgument.Add("TaxInfo", taxInfo);
            WorkflowInvoker.Invoke(itax, taxArgument);
            decimal TotalDeduction = 0;
            decimal oldIncome = 0;
            int financeStartMonth = taxInfo.FinanceYear.StartingDate.Month;
            int financeStartYear = taxInfo.FinanceYear.StartingDate.Year;

            if (taxInfo.Errors.Count == 0)
            {
                AttributeModel taxatrr = taxInfo.AttributemodelList.Where(a => a.Name == "TOTTAXPERMONTH").FirstOrDefault();
                AttributeModel payatrr = taxInfo.AttributemodelList.Where(a => a.Name == "TDS").FirstOrDefault();
                AttributeModel netatrr = taxInfo.AttributemodelList.Where(a => a.Name == "NETPAY").FirstOrDefault();
                AttributeModel dedatrr = taxInfo.AttributemodelList.Where(a => a.Name == "TOTDED").FirstOrDefault();
                taxInfo.Employees.ForEach(emp =>
                {
                    if (!ReferenceEquals(taxatrr, null))
                    {
                        TaxHistory ihis = taxInfo.Result.Find(r => r.FieldId == taxatrr.Id && r.EmployeeId==emp.Id);
                        TotalDeduction = (decimal)ihis.Total;
                        PayrollHistoryList payhistory = new PayrollHistoryList();
                        PayrollHistoryValueList payrollhistory = new PayrollHistoryValueList();
                        //if (taxInfo.emproll.ToUpper() == "EMPLOYEE")
                        //{
                        //    payhistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year * 100) + ph.Month) >= ((financeStartYear * 100) + financeStartMonth) && ((ph.Year * 100) + ph.Month) < (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                        //}
                        //else
                        //{
                        payhistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year * 100) + ph.Month) >= (financeStartYear * 100 + financeStartMonth) && ((ph.Year * 100) + ph.Month) <= (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                        //}

                        // payhistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ph.Year >= financeStartYear && ph.Month >= financeStartMonth && ph.Year <= taxInfo.ApplyYear && ph.Month <= taxInfo.ApplyMonth && ph.EmployeeId == emp.Id).ToList());
                        var Lastmonth = 0;
                        var Lastyear = 0;
                        Lastyear = taxInfo.ApplyYear;
                        Lastmonth = taxInfo.ApplyMonth - 1;
                        if (Lastmonth == 0)
                        {
                            Lastmonth = 12;
                            Lastyear = Lastyear - 1;
                        }
                        int count = 0;
                        for (int n = 0; n < payhistory.Count; n++)
                        {
                            if (payhistory[n].Month == taxInfo.ApplyMonth && payhistory[n].Year == taxInfo.ApplyYear)
                            {
                                count = count + 1;
                            }
                        }

                        if (!ReferenceEquals(payhistory, null) && payhistory.Count > 0 && taxInfo.processtype != "employee")
                        {
                           payrollhistory = payhistory.Where(ph => ph.Month == taxInfo.ApplyMonth && ph.Year == taxInfo.ApplyYear).FirstOrDefault().PayrollHistoryValueList;
                        }

                        if (taxInfo.processtype != "employee")
                        {
                            PayrollHistoryValue paytax = payrollhistory.Where(p => p.AttributeModelId == payatrr.Id).FirstOrDefault();
                            PayrollHistoryValue nettax = payrollhistory.Where(p => p.AttributeModelId == netatrr.Id).FirstOrDefault();
                            PayrollHistoryValue dedtax = payrollhistory.Where(p => p.AttributeModelId == dedatrr.Id).FirstOrDefault();
                            if (!ReferenceEquals(paytax, null) && !ReferenceEquals(nettax, null) && !ReferenceEquals(dedtax, null))
                            {
                                if (!ReferenceEquals(payrollhistory, null))
                                {
                                    oldIncome = Math.Round(Convert.ToDecimal(paytax.Value), 2);

                                    double payta = Math.Round(Convert.ToDouble(Math.Round(TotalDeduction, 2)) * 1.0) * 1;

                                    double dedta = Math.Round(Convert.ToDouble(Math.Round(Convert.ToDecimal(dedtax.Value) - oldIncome + TotalDeduction, 2)) * 1.0) * 1;

                                    double netta = Math.Round(Convert.ToDouble(Math.Round(Convert.ToDecimal(nettax.Value) + oldIncome - TotalDeduction, 2)) * 1.0) * 1;
                                    EntityBehaviorList enbhav = emp.EntityBehaviorList;


                                    MonthlyInput monthInput = new MonthlyInput
                                    {
                                        AttributeModelId = payatrr.Id,
                                        EmployeeId = emp.Id,
                                        Month = taxInfo.ApplyMonth,
                                        Year = taxInfo.ApplyYear,
                                        Value = Convert.ToString(payta),
                                        EntityId = enbhav.EntityId,
                                        EntityModelId = enbhav.EntityModelId
                                    };

                                    //if (taxInfo.emproll.ToUpper() != "EMPLOYEE")
                                    if (taxInfo.processtype != "employee")
                                    {
                                        monthInput.Save();
                                    }

                                    //paytax.Value = Convert.ToString(Math.Round(TotalDeduction,2));
                                    //dedtax.Value= Convert.ToString(Math.Round(Convert.ToDecimal(dedtax.Value)-oldIncome + TotalDeduction,2));
                                    //nettax.Value = Convert.ToString(Math.Round(Convert.ToDecimal(nettax.Value)+oldIncome - TotalDeduction,2));

                                    paytax.Value = Convert.ToString(payta);
                                    dedtax.Value = Convert.ToString(dedta);
                                    nettax.Value = Convert.ToString(netta);
                                    //    if (taxInfo.emproll.ToUpper() != "EMPLOYEE")
                                    if (taxInfo.processtype != "employee")
                                    {
                                        if (paytax.Save() && dedtax.Save() && nettax.Save())
                                        {
                                            //taxInfo.Errors.Add(string.Format("There is an errors  while saving Tax "));
                                        }
                                        else
                                        {
                                            taxInfo.Errors.Add(string.Format("There is an errors  while saving Tax "));
                                        }
                                    }
                                }
                            }
                        }
                       /* }
                        else
                        {
                            taxInfo.Errors.Add(string.Format("Payroll not Proccessed for the employee {0} ", emp.EmployeeCode));
                        }*/
                        PayrollHistoryValue payroll = new PayrollHistoryValue();

                    }
                });

                taxInfo.Result.ForEach(r =>
                 {
                     r.Id = Guid.NewGuid();
                     r.Createdby = r.UserId;
                     if (r.Actual != null)
                     {
                         decimal act = Convert.ToDecimal(r.Actual);

                         r.Actual = Math.Round(act + Convert.ToDecimal(0.01));
                     }
                     if (r.Projection != null)
                     {
                         decimal pro = Convert.ToDecimal(r.Projection);

                         r.Projection = Math.Round(pro + Convert.ToDecimal(0.01));
                     }
                     if (r.Total != null)
                     {
                         decimal tot = Convert.ToDecimal(r.Total);

                         r.Total = Math.Round(tot + Convert.ToDecimal(0.01));
                     }

                     if (r.Limit != null)
                     {
                         decimal tot = Convert.ToDecimal(r.Limit);

                         r.Limit = Math.Round(tot + Convert.ToDecimal(0.01));
                     }

                     r.ApplyDate = Convert.ToDateTime("1/" + taxInfo.ApplyMonth + "/" + taxInfo.ApplyYear, new System.Globalization.CultureInfo("en-GB"));
                     //if (!r.Save())
                     //{
                     //    taxInfo.Errors.Add(string.Format("There is an errors  while saving {0} ", r.Field));
                     //}

                 });

                taxInfo.dt2 = new DataTable();

               taxInfo.dt2.Columns.Add("Id", typeof(Guid));
               taxInfo.dt2.Columns.Add("FinanceYearId", typeof(Guid));
               taxInfo.dt2.Columns.Add("ApplyDate", typeof(DateTime));
               taxInfo.dt2.Columns.Add("EmployeeId", typeof(Guid));
               taxInfo.dt2.Columns.Add("FieldId", typeof(Guid));
               taxInfo.dt2.Columns.Add("Field", typeof(String));
               taxInfo.dt2.Columns.Add("FieldType", typeof(String));
               taxInfo.dt2.Columns.Add("Actual", typeof(Decimal));
               taxInfo.dt2.Columns.Add("Projection", typeof(Decimal));
               taxInfo.dt2.Columns.Add("Total", typeof(Decimal));
               taxInfo.dt2.Columns.Add("Limit", typeof(Decimal));
               taxInfo.dt2.Columns.Add("CreatedBy", typeof(int));

                taxInfo.Result.ForEach(ti =>
                {
                    if (ti.Actual == null) { ti.Actual = 0;}
                    if (ti.Projection == null) {ti.Projection = 0;}
                    if (ti.Total == null) {ti.Total = 0;}
                    if (ti.Limit == null) {ti.Limit = 0;}
                    if (ti.Field.Length >50)
                    {
                        ti.Field = ti.Field.Substring(0, 50);
                    }
                    taxInfo.dt2.Rows.Add(ti.Id,ti.FinanceYearId, ti.ApplyDate, ti.EmployeeId, ti.FieldId, ti.Field, ti.FieldType, ti.Actual, ti.Projection,ti.Total, ti.Limit, ti.Createdby);
                });
            }
        }
    }
}
