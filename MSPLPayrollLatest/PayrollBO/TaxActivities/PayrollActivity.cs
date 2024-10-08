using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Data;

namespace PayrollBO.TaxActivities
{

    public sealed class PayrollActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxinfo = context.GetValue(this.TaxInfo);
            var attrmodels = taxinfo.AttributemodelList.Where(al => al.CompanyId == taxinfo.CompanyId).ToList();
            TXFinanceYearList txfl = taxinfo.finyearlist;
            // TXFinanceYearList txfl = new TXFinanceYearList(taxinfo.CompanyId);
            int financeStartMonth = taxinfo.FinanceYear.StartingDate.Month;
            int financeStartYear = taxinfo.FinanceYear.StartingDate.Year;
            int ProjectedMonth = 0;
            taxinfo.Balmaxdays = 0;
            taxinfo.Balprojdays = 0;
            taxinfo.Balprojmonth = 0;
            IncomeTaxComputeActivity iTax = new IncomeTaxComputeActivity();
            DateTime sdate = taxinfo.EffectiveDate;
            if (!taxinfo.FandFFlag)
            {
                do
                {
                    sdate = sdate.AddMonths(1);
                    if (sdate < taxinfo.FinanceYear.EndingDate)
                    {
                        ProjectedMonth++;
                    }
                } while (sdate <= taxinfo.FinanceYear.EndingDate);

                taxinfo.ProjectionMonth = ProjectedMonth;
            }
            else//No Projection for F&F Process 
            {
                taxinfo.ProjectionMonth = 0;
            }

                taxinfo.Employees.ForEach(emp =>
                {
                    List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                    List<TXFinanceYear> txf = txfl.Where(w => (w.StartingDate <= emp.DateOfJoining && w.EndingDate >= emp.DateOfJoining)).ToList();
                    PayrollHistoryList payHistory = new PayrollHistoryList();
                    if (taxinfo.processtype == "employee")
                    {
                        ProjectedMonth = ProjectedMonth + 1;
                        taxinfo.ProjectionMonth = ProjectedMonth;
                    }
                    //if (taxinfo.emproll.ToUpper() != "ADMIN")
                    //{
                    //    payHistory.AddRange(taxinfo.payrollhistorylist.Where(ph => ph.CompanyId == taxinfo.CompanyId && ((ph.Year*100) + ph.Month)  >= ((financeStartYear*100) + financeStartMonth) && ((ph.Year*100) + ph.Month) < (taxinfo.ApplyYear*100 + taxinfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                    //}
                    //else
                    //{
                    int sum = 0;
                    if (emp.Id.ToString().ToUpper() == "06239E7A-E91A-4918-9675-0C62FCFF6E78")
                    {
                        var id = emp.Id;
                        sum = sum + 1;
                    }
                   payHistory.AddRange(taxinfo.payrollhistorylist.Where(ph => ph.CompanyId == taxinfo.CompanyId && ((ph.Year*100) + ph.Month) >= (financeStartYear*100 + financeStartMonth) && ((ph.Year*100) + ph.Month) <= (taxinfo.ApplyYear*100 + taxinfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                   // }
                  // PayrollHistoryList payHistory = new PayrollHistoryList(taxinfo.CompanyId, financeStartYear, financeStartMonth, taxinfo.ApplyYear, taxinfo.ApplyMonth, emp.Id);
                  emp.PayrollHistoryList = new PayrollHistoryValueList();
                  // taxinfo.TxEmployeeSectionList = new TXEmployeeSectionList(emp.Id, taxinfo.EffectiveDate);
                 IncrementList increment = new IncrementList();
                    increment.AddRange(taxinfo.increment.Where(inc => inc.EmployeeId == emp.Id).ToList());
                    PayrollHistoryValueList payarr = new PayrollHistoryValueList();
                    emp.PayrollofMonth = new PayrollHistoryValueList();
                    var Lastmonth = 0;
                    var Lastyear = 0;
                    Lastyear = taxinfo.ApplyYear;
                    Lastmonth = taxinfo.ApplyMonth - 1;
                    if (Lastmonth == 0)
                    {
                        Lastmonth = 12;
                        Lastyear = Lastyear - 1;
                    }
                    int count = 0;
                    for (int n = 0; n < payHistory.Count; n++)
                    {
                        if (payHistory[n].Month == taxinfo.ApplyMonth && payHistory[n].Year == taxinfo.ApplyYear)
                        {
                            count = count + 1;
                        }
                    }

                    // emp.PayrollofMonth = new PayrollHistory(taxinfo.CompanyId, emp.Id, taxinfo.ApplyYear, taxinfo.ApplyMonth).PayrollHistoryValueList;
                    // emp.PayrollMonth = emp.PayrollofMonth;
                    // emp.PayrollMonth = new PayrollHistory(taxinfo.CompanyId, emp.Id, taxinfo.ApplyYear, taxinfo.ApplyMonth).PayrollHistoryValueList;
                    IncomeMatchingList imatch = new IncomeMatchingList();
                 //imatch.AddRange(new IncomeMatchingList(taxinfo.FinanceYearId).Where(i => i.TaxDeductionMode != "ONETIME").ToList());
                 imatch.AddRange(taxinfo.incmatchList);

                    IncomeMatchingList imats = new IncomeMatchingList();
                    imats.AddRange(taxinfo.incmatchList);
                    IncomeMatchingList imat = imats;
                    var taxableAtt = attrmodels.Where(e => e.IsTaxable == true).Select(e => e.Id);
                 List<Guid> taxatt = attrmodels.Where(h => h.IsTaxable == true).Select(h => h.Id).ToList();
                    // imatch.AddRange(new IncomeMatchingList(taxinfo.FinanceYearId).ToList());//Modified by muthu show all deduction type
                    List<TaxHistory> taxHistAp = new List<TaxHistory>();
                   if (!ReferenceEquals(payHistory, null) && payHistory.Count > 0)
                   {
                       payHistory.Where(p => p.EmployeeId == emp.Id).OrderBy(o => o.Month).ToList().ForEach(p =>
                       {
                           List<FormulaRecursive> lstFormulaRec = new List<FormulaRecursive>();
                           PayrollHistoryValueList payar = new PayrollHistoryValueList();

                           taxinfo.SubSections.Where(ss => ss.FormulaType == 7).ToList().ForEach(ss =>
                           {
                               AttributeModel attr = taxinfo.AttributemodelList.Where(a => a.Id == new Guid(ss.Formula)).FirstOrDefault();
                               emp.PayrollHistoryList.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == attr.Id).ToList());
                           });

                           var vpf = taxinfo.AttributemodelList.Where(x => x.Name == "VPF").FirstOrDefault();
                           if (vpf != null)
                               emp.PayrollHistoryList.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == vpf.Id).ToList());

                           imatch = new  IncomeMatchingList();
                           taxinfo.incmatchList = null;
                           taxinfo.incmatchList = new IncomeMatchingList();
                           taxinfo.MoveToClass(taxinfo.StructList, taxinfo.incmatchList);
                           imatch.AddRange(taxinfo.incmatchList);
                           imatch.ForEach(im =>
                           {

                              emp.PayrollHistoryList.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == im.AttributemodelId).ToList());
                          //emp.PayrollHistoryList.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == im.MatchingComponent).ToList());
                             var tempPayHistroy = emp.PayrollHistoryList.Distinct().ToList();
                             emp.PayrollHistoryList = new PayrollHistoryValueList();
                             emp.PayrollHistoryList.AddRange(tempPayHistroy);
                             if (!string.IsNullOrEmpty(im.Formula) || im.Formula != "")
                             {
                                   string[] values = im.OtherComponent.Split('-', '+');
                                    values.ToList().ForEach(f =>
                                      {
                                          Guid id = attrmodels.Where(w => w.Name.Trim() == f.Trim()).FirstOrDefault().Id;
                                          payarr.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == id).ToList());
                                          payar.AddRange(p.PayrollHistoryValueList.Where(i => i.AttributeModelId == id).ToList());
                                      });

                             }
                           });



                      // To save the actual value of each month before the current month



                        if (p.PayrollHistoryValueList.Count > 0)
                        {

                                lstFormulaRec.AddRange(payar.GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                                {
                                    Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))),
                                    Name = "",
                                    Id = Convert.ToString(group.Key.AttributeModelId),
                                    ExecuteOrder = 4,
                                    Rounding = 0,
                                    type = 3,
                                    Percentage = "100",
                                    DoRoundOff = false
                                }));
                               IncomeMatchingList imat1 = new IncomeMatchingList();
                               taxinfo.incmatchList = null;
                               taxinfo.incmatchList = new IncomeMatchingList();
                               taxinfo.MoveToClass(taxinfo.StructList, taxinfo.incmatchList);
                               imat1.AddRange(taxinfo.incmatchList);
                               imat1.ForEach(i =>
                               {
                                    if (!string.IsNullOrEmpty(i.Formula) || i.Formula != "")
                                    {
                                        string output = string.Empty;
                                        string[] valuesAdd = i.OtherComponent.Split('+');
                                        if (i.Id.ToString() == "7751f5cc-e0d6-41ee-8e55-ff069e401bdc")
                                       {
                                           Console.WriteLine();
                                       }
                                        valuesAdd.ToList().ForEach(f =>
                                        {
                                            if (f.IndexOf('-') != -1)
                                            {
                                                f.Split('-').ToList().ForEach(s =>
                                                {
                                                    Guid id = attrmodels.Where(w => w.Name.Trim() == s.Trim()).FirstOrDefault().Id;
                                                    var val = lstFormulaRec.Where(d => d.Id == id.ToString()).FirstOrDefault();
                                                    if (!object.ReferenceEquals(val, null))
                                                    {
                                                  i.Formula = i.Formula.Replace(id.ToString(), val.Assignedvalues);
                                                  //i.Formula = val.Assignedvalues;
                                                    }
                                                    else
                                                    {
                                                        i.Formula = i.Formula.Replace(id.ToString(), "0");
                                                    }
                                                });

                                            }
                                            else
                                            {
                                                Guid id = attrmodels.Where(w => w.Name.Trim() == f.Trim()).FirstOrDefault().Id;
                                                var val = lstFormulaRec.Where(d => d.Id == id.ToString()).FirstOrDefault();
                                                if (!object.ReferenceEquals(val, null))
                                                {
                                                 i.Formula = i.Formula.Replace(id.ToString(), val.Assignedvalues);
                                                 // i.Formula = val.Assignedvalues;
                                                }
                                                else
                                                {
                                                 i.Formula = i.Formula.Replace(id.ToString(), "0");
                                                }

                                            }
                                        });



                                        i.Formula = i.Formula.Replace("{", String.Empty);
                                        i.Formula = i.Formula.Replace("}", String.Empty);
                                  // var temp = lstFormulaRecursive.Where(d => d.Id == i.Formula).FirstOrDefault();
                                  if (i.Operator == "-")
                                        {
                                            i.Formula = i.Formula + "*(-1)";
                                        }
                                        lstFormulaRec.AddRange(p.PayrollHistoryValueList.Where(w => w.AttributeModelId == i.AttributemodelId).GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                                        {
                                            Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))) + "+" + (i.Formula),
                                            Name = attrmodels.Where(a => a.Id == group.Key.AttributeModelId).FirstOrDefault().Name,
                                            Id = Convert.ToString(group.Key.AttributeModelId),
                                            ExecuteOrder = 4,
                                            Rounding = 0,
                                            type = 1,
                                            Percentage = "100",
                                            DoRoundOff = false
                                        }));

                                    }
                                    else
                                    {
                                        lstFormulaRec.AddRange(p.PayrollHistoryValueList.Where(w => w.AttributeModelId == i.AttributemodelId).GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                                        {
                                            Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))),
                                            Name = attrmodels.Where(a => a.Id == group.Key.AttributeModelId).FirstOrDefault().Name,
                                            Id = Convert.ToString(group.Key.AttributeModelId),
                                            ExecuteOrder = 4,
                                            Rounding = 0,
                                            type = 1,
                                            Percentage = "100",
                                            DoRoundOff = false
                                        }));
                                    }

                               });

                                lstFormulaRec = lstFormulaRec.OrderBy(u => u.ExecuteOrder).ToList();
                                int tempOrd = 0;
                                lstFormulaRec.ForEach(f => { f.Order = tempOrd++; });
                                iTax.recursive(taxinfo, emp, lstFormulaRec, new Entity { }, increment);//no need to pass entity
                                lstFormulaRec = lstFormulaRec.OrderBy(u => u.Order).ToList();
                                PayrollHistoryValueList payValu = new PayrollHistoryValueList();
                                if (!object.ReferenceEquals(emp.PayrollHistoryList, null) && emp.PayrollHistoryList.Count > 0)
                                {
                                    payValu.AddRange(emp.PayrollHistoryList.GroupBy(f => new { f.AttributeModelId })
                                              .Select(group => new PayrollHistoryValue
                                              {
                                                  AttributeModelId = group.Key.AttributeModelId,
                                                  Value = Convert.ToString(group.Sum(f => Convert.ToDecimal(f.Value)))
                                              }));
                                }
                                lstFormulaRec.ForEach(u =>
                                {
                                    string error = string.Empty;
                                    string output = string.Empty;
                                    string rerun = string.Empty;
                                    if (u.Assignedvalues.IndexOf("{") > 0)
                                    {
                                        u.Validate(u.Assignedvalues, lstFormulaRec, u.Id, ref error, out output, rerun);
                                    }
                                    else
                                    {
                                        output = u.Assignedvalues;
                                    }

                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        taxinfo.Errors.Add("There is a some problem in formula.Please check it.");
                                  // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                                    }
                                    else
                                    {
                                        string result = string.Empty;
                                        if (u.type == 4)
                                        {

                                        }
                                        else
                                        if (u.type == 5)
                                        {

                                        }
                                        Eval eval = new Eval();
                                        result = eval.Execute(output).ToString();
                                        u.Output = result;
                                        u.Assignedvalues = result.ToString();
                                        if (!object.ReferenceEquals(payValu.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault(), null))
                                            payValu.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().Value = result;

                                    }
                                }); //listformula end
                                    //Actual Payroll History
                                IncomeMatching immode = new IncomeMatching();
                                if (!object.ReferenceEquals(emp.PayrollHistoryList, null) && emp.PayrollHistoryList.Count > 0)
                                {

                                    for (int cnt = 0; cnt < payValu.Count; cnt++)
                                    {
                                       taxatt.ForEach(f =>
                                       {
                                           if (f == payValu[cnt].AttributeModelId)
                                           {
                                               immode = imat.Where(x => x.AttributemodelId == f).FirstOrDefault();
                                               decimal? projection = 0;
                                               if (immode != null && immode.Projection == true)
                                               {
                                                    // var payval = emp.PayrollofMonth.Where(e => e.AttributeModelId == immode.MatchingComponent).FirstOrDefault();
                                                    var payval = taxinfo.entity.EntityAttributeModelList.Where(e => e.AttributeModelId == immode.MatchingComponent).FirstOrDefault();
                                                   projection = payval != null ? Convert.ToDecimal(payval.EntityAttributeValue.Value) : 0;
                                                   projection = projection * ProjectedMonth;
                                               }
                                               else if (immode != null && immode.Projection == false)
                                               {
                                                   projection = 0;
                                               }
                                               else
                                               {
                                                   projection = 0;
                                               }
                                               var attDetails = taxinfo.AttributemodelList.Where(al => al.Id == payValu[cnt].AttributeModelId && al.CompanyId == taxinfo.CompanyId).FirstOrDefault();
                                                // var attDetails = new AttributeModel(payValu[cnt].AttributeModelId, taxinfo.CompanyId);
                                                taxHistAp.Add(new TaxHistory
                                               {
                                                   EmployeeId = emp.Id,
                                                   FinanceYearId = taxinfo.FinanceYearId,
                                                   ApplyDate = taxinfo.EffectiveDate,
                                                   FieldId = payValu[cnt].AttributeModelId,
                                                   Field = attDetails.Name,
                                                   Actual = Convert.ToDecimal(payValu[cnt].Value),
                                                   Projection = projection,
                                                   Total = Convert.ToDecimal(payValu[cnt].Value) + projection,
                                                   FieldType = "Income",
                                                   ActualMonth = p.Month

                                               });
                                           }
                                       });
                                    }
                                }
                                else
                                {
                                    for (int cnt = 0; cnt < imat.Count; cnt++)
                                    {
                                  // var payval = emp.PayrollofMonth.Where(v => v.AttributeModelId == payValu[cnt].AttributeModelId && v.AttributeModelId == imat[cnt].MatchingComponent).FirstOrDefault();
                                  var payval = taxinfo.entity.EntityAttributeModelList.Where(v => v.AttributeModelId == payValu[cnt].AttributeModelId && v.AttributeModelId == imat[cnt].MatchingComponent).FirstOrDefault();
                                  // var payval = emp.PayrollofMonth.Where(p => p.AttributeModelId == payValue[cnt].AttributeModelId && taxableAttr.Contains(p.AttributeModelId)).FirstOrDefault();
                                      if (payval != null)
                                      {
                                            decimal? projection = Convert.ToDecimal(payval.EntityAttributeValue.Value);
                                            projection = projection * ProjectedMonth;
                                            if (imat[cnt].Projection == false)
                                            {
                                                projection = 0;
                                            }
                                            taxHistAp.Add(new TaxHistory
                                            {
                                                EmployeeId = emp.Id,
                                                FinanceYearId = taxinfo.FinanceYearId,
                                                ApplyDate = taxinfo.EffectiveDate,
                                                FieldId = imat[cnt].AttributemodelId,
                                                Field = attrmodels.Where(at=>at.Id == imat[cnt].AttributemodelId).FirstOrDefault().Name,
                                                Actual = 0,
                                                Projection = projection,
                                                // Total = Convert.ToDecimal(payValu[cnt].Value) + projection,
                                                Total = projection,
                                                FieldType = "Income",
                                                ActualMonth = p.Month
                                            });
                                      }
                                    }
                                }


                        }
                       });
                   }
                   else
                   {
                        int temp_projection = 0 , temp_projection_days = 0 , maxdays = 0, temp_projection_month = 0;
                        temp_projection = ProjectedMonth;
                        maxdays = Convert.ToInt32(DateTime.DaysInMonth(emp.DateOfJoining.Year, emp.DateOfJoining.Month));

                        /* commented by Tamilvanan Kathirvelu on 22-04-2023
                        if (taxinfo.ApplyYear > emp.DateOfJoining.Year && (taxinfo.ApplyYear - 1) == emp.DateOfJoining.Year)
                        {
                            temp_projection_month = taxinfo.ApplyMonth + 11 - emp.DateOfJoining.Month;
                            temp_projection = ProjectedMonth;
                            temp_projection_days = maxdays + 1 - Convert.ToInt32(emp.DateOfJoining.Date);
                        }

                        if (taxinfo.ApplyYear == emp.DateOfJoining.Year)
                        {
                            temp_projection = ProjectedMonth - 1;
                            temp_projection_month = taxinfo.ApplyMonth  - emp.DateOfJoining.Month;
                            if (taxinfo.ApplyMonth == emp.DateOfJoining.Month)
                            {
                               temp_projection_month = 0;
                            }
                            temp_projection_days = maxdays + 1 - Convert.ToInt32(emp.DateOfJoining.Day);
                        }
                        */

                        //Amended by Tamilvanan Kathirvelu on 22-04-2023 started
                        if (taxinfo.ApplyYear > emp.DateOfJoining.Year && (taxinfo.ApplyYear - 1) == emp.DateOfJoining.Year)
                        {
                            if (taxinfo.ApplyMonth > 4)
                            {
                                temp_projection_month = taxinfo.ApplyMonth + 11 - emp.DateOfJoining.Month;
                                temp_projection = ProjectedMonth;
                                temp_projection_days = maxdays + 1 - Convert.ToInt32(emp.DateOfJoining.Day);
                            }

                        }

                        if (taxinfo.ApplyYear == emp.DateOfJoining.Year)
                        {
                            if (taxinfo.ApplyMonth > 4) temp_projection = ProjectedMonth - 1;
                            if (taxinfo.ApplyMonth == emp.DateOfJoining.Month)
                            {
                                temp_projection_month = 0;
                            }

                            if (emp.DateOfJoining.Month >= 4 && taxinfo.ApplyMonth > 4)
                                temp_projection_days = maxdays + 1 - Convert.ToInt32(emp.DateOfJoining.Day);
                        }
                        //Amended by Tamilvanan Kathirvelu on 22-04-2023 end

                        taxinfo.ProjectionMonth = temp_projection;
                        taxinfo.Balprojmonth = temp_projection_month;
                        taxinfo.Balprojdays = temp_projection_days;
                        taxinfo.Balmaxdays = maxdays;

                        for (int cnt = 0; cnt < taxinfo.entity.EntityAttributeModelList.Count; cnt++)
                        {
                            var taxID = attrmodels.Where(at => at.Id == taxinfo.entity.EntityAttributeModelList[cnt].AttributeModelId && at.IsTaxable == true).FirstOrDefault();
                            if (taxID != null)
                            {
                                IncomeMatching immode = new IncomeMatching();
                                Guid ID = taxinfo.entity.EntityAttributeModelList[cnt].AttributeModelId;
                                immode = imat.Where(x => x.AttributemodelId == ID).FirstOrDefault();
                                decimal? projection = 0;
                                if (immode != null)
                                {
                                    var payval = taxinfo.entity.EntityAttributeModelList.Where(ta => ta.AttributeModelId == immode.MatchingComponent).FirstOrDefault();
                                    if (immode.Projection == true)
                                    {
                                        if (payval != null)
                                        {
                                            projection = Convert.ToDecimal(payval.EntityAttributeValue.Value);
                                            projection = (projection * temp_projection) + (projection * temp_projection_month) + (projection * temp_projection_days / maxdays);
                                        }
                                    }
                                    else
                                    {
                                        payval = taxinfo.entity.EntityAttributeModelList.Where(ta => ta.AttributeModelId == immode.AttributemodelId).FirstOrDefault();
                                        projection = Convert.ToDecimal(payval.EntityAttributeValue.Value);
                                    }
                                    taxHistAp.Add(new TaxHistory
                                    {
                                        EmployeeId = emp.Id,
                                        FinanceYearId = taxinfo.FinanceYearId,
                                        ApplyDate = taxinfo.EffectiveDate,
                                        FieldId = ID,
                                        Field = attrmodels.Where(at => at.Id == ID).FirstOrDefault().Name,
                                        Actual = 0,
                                        Projection = projection,
                                        Total = projection,
                                        FieldType = "Income",
                                        ActualMonth = taxinfo.ApplyMonth
                                    });

                                    taxinfo.Result.Add(new TaxHistory
                                    {
                                        EmployeeId = emp.Id,
                                        FinanceYearId = taxinfo.FinanceYearId,
                                        ApplyDate = taxinfo.EffectiveDate,
                                        FieldId = ID,
                                        Field = attrmodels.Where(at => at.Id == ID).FirstOrDefault().Name,
                                        Actual = 0,
                                        Projection = Math.Round(Convert.ToDecimal(projection) + Convert.ToDecimal(0.01)),
                                        Total = Math.Round(Convert.ToDecimal(projection)) + Convert.ToDecimal(0.01),
                                        FieldType = "Income",

                                    });
                                }
                            }
                        }
                   }


                    /*miscellanwous income added here   */
                    // if (taxinfo.emproll.ToUpper() == "EMPLOYEE")
                    if (taxinfo.processtype == "employee")
                    {
                        if (!ReferenceEquals(taxinfo.TXProjIncome, null))
                        {
                            Decimal Misc_Total = Math.Round((Convert.ToDecimal(taxinfo.TXProjIncome.Income1) + Convert.ToDecimal(taxinfo.TXProjIncome.Income2) + Convert.ToDecimal(taxinfo.TXProjIncome.Income3)), 0);
                            taxHistAp.Add(new TaxHistory
                            {
                                EmployeeId = emp.Id,
                                FinanceYearId = taxinfo.FinanceYearId,
                                ApplyDate = taxinfo.EffectiveDate,
                                FieldId = attrmodels.Where(at => at.Name.ToUpper() == "MISCINCOME").FirstOrDefault().Id,
                                Field = attrmodels.Where(at => at.Name.ToUpper() == "MISCINCOME").FirstOrDefault().Name,
                                Actual = Misc_Total,
                                Projection = 0,
                                Total = Misc_Total,
                                FieldType = "Income",
                                ActualMonth = taxinfo.ApplyMonth
                            });

                            taxinfo.Result.Add(new TaxHistory
                            {
                                EmployeeId = emp.Id,
                                FinanceYearId = taxinfo.FinanceYearId,
                                ApplyDate = taxinfo.EffectiveDate,
                                FieldId = attrmodels.Where(at => at.Name.ToUpper() == "MISCINCOME").FirstOrDefault().Id,
                                Field = attrmodels.Where(at => at.Name.ToUpper() == "MISCINCOME").FirstOrDefault().Name,
                                Actual = Misc_Total,
                                Projection = 0,
                                Total = Misc_Total,
                                FieldType = "Income",

                            });

                        }
                    }



                    taxinfo.dt1 = new DataTable();

                    taxinfo.dt1.Columns.Add("FinanceYearId", typeof(Guid));
                    taxinfo.dt1.Columns.Add("ApplyDate", typeof(DateTime));
                    taxinfo.dt1.Columns.Add("EmployeeId", typeof(Guid));
                    taxinfo.dt1.Columns.Add("FieldId", typeof(Guid));
                    taxinfo.dt1.Columns.Add("Field", typeof(String));
                    taxinfo.dt1.Columns.Add("FieldType", typeof(String));
                    taxinfo.dt1.Columns.Add("Actual", typeof(Decimal));
                    taxinfo.dt1.Columns.Add("Projection", typeof(Decimal));
                    taxinfo.dt1.Columns.Add("Total", typeof(Decimal));
                    taxinfo.dt1.Columns.Add("Limit", typeof(Decimal));
                    taxinfo.dt1.Columns.Add("Createdby", typeof(int));
                    taxinfo.dt1.Columns.Add("ModifiedBy", typeof(int));
                    taxinfo.dt1.Columns.Add("ActualMonth", typeof(int));

                    if (!ReferenceEquals(null, taxHistAp))
                    {
                        taxHistAp.ForEach(f =>
                        {
                            taxinfo.dt1.Rows.Add(f.FinanceYearId, f.ApplyDate, f.EmployeeId, f.FieldId, f.Field, f.FieldType, f.Actual, f.Projection, f.Total, f.Limit, f.Createdby, f.ModifiedBy, f.ActualMonth);
                        });
                    /*                    TaxHistory txsave = new TaxHistory();
                                        txsave.SaveAP(dt1);*/
                    }



                //       end  of saving the actual value of each month before the current month























                 lstFormulaRecursive.AddRange(payarr.GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                 {
                        Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))),
                        Name = "",
                        Id = Convert.ToString(group.Key.AttributeModelId),
                        ExecuteOrder = 4,
                        Rounding = 0,
                        type = 3,
                        Percentage = "100",
                        DoRoundOff = false
                 }));
                    imatch.ForEach(i =>
                    {
                        if (!string.IsNullOrEmpty(i.Formula) || i.Formula != "")
                        {
                            string output = string.Empty;
                            string[] valuesAdd = i.OtherComponent.Split('+');

                            valuesAdd.ToList().ForEach(f =>
                            {
                                if (f.IndexOf('-') != -1)
                                {
                                    f.Split('-').ToList().ForEach(s =>
                                    {
                                        Guid id = attrmodels.Where(w => w.Name.Trim() == s.Trim()).FirstOrDefault().Id;
                                        var val = lstFormulaRecursive.Where(d => d.Id == id.ToString()).FirstOrDefault();
                                        if (!object.ReferenceEquals(val, null))
                                        {
                                            i.Formula = i.Formula.Replace(id.ToString(), val.Assignedvalues);
                                            //i.Formula = val.Assignedvalues;
                                        }
                                        else
                                        {
                                            i.Formula = i.Formula.Replace(id.ToString(), "0");
                                        }
                                    });

                                }
                                else
                                {
                                    Guid id = attrmodels.Where(w => w.Name.Trim() == f.Trim()).FirstOrDefault().Id;
                                    var val = lstFormulaRecursive.Where(d => d.Id == id.ToString()).FirstOrDefault();
                                    if (!object.ReferenceEquals(val, null))
                                    {
                                        i.Formula = i.Formula.Replace(id.ToString(), val.Assignedvalues);
                                        // i.Formula = val.Assignedvalues;
                                    }
                                    else
                                    {
                                        i.Formula = i.Formula.Replace(id.ToString(), "0");
                                    }

                                }



                            });



                            i.Formula = i.Formula.Replace("{", String.Empty);
                            i.Formula = i.Formula.Replace("}", String.Empty);
                        // var temp = lstFormulaRecursive.Where(d => d.Id == i.Formula).FirstOrDefault();
                        if (i.Operator == "-")
                            {
                                i.Formula = i.Formula + "*(-1)";
                            }
                            lstFormulaRecursive.AddRange(emp.PayrollHistoryList.Where(w => w.AttributeModelId == i.AttributemodelId).GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                            {
                                Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))) + "+" + (i.Formula),
                                Name = attrmodels.Where(a => a.Id == group.Key.AttributeModelId).FirstOrDefault().Name,
                                Id = Convert.ToString(group.Key.AttributeModelId),
                                ExecuteOrder = 4,
                                Rounding = 0,
                                type = 1,
                                Percentage = "100",
                                DoRoundOff = false
                            }));

                        }
                        else
                        {
                            lstFormulaRecursive.AddRange(emp.PayrollHistoryList.Where(w => w.AttributeModelId == i.AttributemodelId).GroupBy(h => new { h.AttributeModelId }).Select(group => new FormulaRecursive
                            {
                                Assignedvalues = Convert.ToString(group.Sum(h => Convert.ToDecimal(string.IsNullOrEmpty(h.Value) ? "0" : h.Value))),
                                Name = attrmodels.Where(a => a.Id == group.Key.AttributeModelId).FirstOrDefault().Name,
                                Id = Convert.ToString(group.Key.AttributeModelId),
                                ExecuteOrder = 4,
                                Rounding = 0,
                                type = 1,
                                Percentage = "100",
                                DoRoundOff = false
                            }));
                        }

                    });
                 //imatch.ForEach(s =>
                 //{
                 //    if (s.Operator == "+" || s.Operator == "-")
                 //    {
                 //        lstFormulaRecursive.Where(e=>new Guid(e.Id)==s.AttributemodelId).FirstOrDefault().Assignedvalues = 
                 //    }
                 //});
                 lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();
                    int tempOrder = 0;
                    lstFormulaRecursive.ForEach(f => { f.Order = tempOrder++; });
                    iTax.recursive(taxinfo, emp, lstFormulaRecursive, new Entity { }, increment);//no need to pass entity
                    lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                    PayrollHistoryValueList payValue = new PayrollHistoryValueList();
                    if (!object.ReferenceEquals(emp.PayrollHistoryList, null) && emp.PayrollHistoryList.Count > 0)
                    {
                        payValue.AddRange(emp.PayrollHistoryList.GroupBy(f => new { f.AttributeModelId })
                                  .Select(group => new PayrollHistoryValue
                                  {
                                      AttributeModelId = group.Key.AttributeModelId,
                                      Value = Convert.ToString(group.Sum(f => Convert.ToDecimal(f.Value)))
                                  }));
                    }
                    lstFormulaRecursive.ForEach(u =>
                    {
                        string error = string.Empty;
                        string output = string.Empty;
                        string chk = string.Empty;
                        chk = u.Assignedvalues;
                        string rerun = string.Empty;
                        if (chk.IndexOf("{") >= 0)
                        {
                            u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output, rerun);
                        }
                        else
                        {
                            output = u.Assignedvalues;
                        }
                        if (!string.IsNullOrEmpty(error))
                        {
                            taxinfo.Errors.Add("There is a some problem in formula.Please check it.");
                        // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                    }
                      else
                      {
                            string result = string.Empty;
                            if (u.type == 4)
                            {
                             //ifElseStmt obj = new ifElseStmt();
                             //List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                             //ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                             //var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                             //if (!object.ReferenceEquals(tm, null))
                             //    output = tm.thenVal;
                            }
                            else
                            if (u.type == 5)
                            {

                            }
                            Eval eval = new Eval();
                            result = eval.Execute(output).ToString();
                            u.Output = result;
                            u.Assignedvalues = result.ToString();
                            if (!object.ReferenceEquals(payValue.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault(), null))
                                payValue.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().Value = result;
                      }
                    }); //listformula end
                        //Actual Payroll History

                    var taxableAttr = attrmodels.Where(e => e.IsTaxable == true).Select(e => e.Id);
                    List<Guid> taxattr = attrmodels.Where(h => h.IsTaxable == true).Select(h => h.Id).ToList();
                    IncomeMatching immodel = new IncomeMatching();
                    if (!object.ReferenceEquals(emp.PayrollHistoryList, null) && emp.PayrollHistoryList.Count > 0)
                    {

                       for (int cnt = 0; cnt < payValue.Count; cnt++)
                       {
                            taxattr.ForEach(f =>
                            {
                               if (f == payValue[cnt].AttributeModelId)
                               {
                                    immodel = imatch.Where(x => x.AttributemodelId == f).FirstOrDefault();
                                    decimal? projection = 0;
                                    if (immodel != null && immodel.Projection == true)
                                    {
                                        // var payval = emp.PayrollofMonth.Where(p => p.AttributeModelId == immodel.MatchingComponent).FirstOrDefault();
                                        var payval = taxinfo.entity.EntityAttributeModelList.Where(p => p.AttributeModelId == immodel.MatchingComponent).FirstOrDefault();
                                        projection = payval != null ? Convert.ToDecimal(payval.EntityAttributeValue.Value) : 0;
                                        projection = projection * ProjectedMonth;
                                    }
                                    else if (immodel != null && immodel.Projection == false)
                                    {
                                        projection = 0;
                                    }
                                    else
                                    {
                                        projection = 0;
                                    }
                                    var attDetails = taxinfo.AttributemodelList.Where(al => al.Id == payValue[cnt].AttributeModelId && al.CompanyId == taxinfo.CompanyId).FirstOrDefault();
                                //  var attDetails = new AttributeModel(payValue[cnt].AttributeModelId, taxinfo.CompanyId);
                                 taxinfo.Result.Add(new TaxHistory
                                 {
                                        EmployeeId = emp.Id,
                                        FinanceYearId = taxinfo.FinanceYearId,
                                        ApplyDate = taxinfo.EffectiveDate,
                                        FieldId = payValue[cnt].AttributeModelId,
                                        Field = attDetails.Name,
                                        Actual = Math.Round(Convert.ToDecimal(payValue[cnt].Value) + Convert.ToDecimal(0.01)),
                                        Projection = Math.Round(Convert.ToDecimal(projection) + Convert.ToDecimal(0.01)),
                                        Total = Math.Round(Convert.ToDecimal(Convert.ToDecimal(payValue[cnt].Value) + projection) + Convert.ToDecimal(0.01)),
                                        FieldType = "Income",

                                 });
                               }
                            });
                       
                       }
                    }
                    else
                    {
                        for (int cnt = 0; cnt < imatch.Count; cnt++)
                        {
                            //  var payval = emp.PayrollofMonth.Where(p => p.AttributeModelId == payValue[cnt].AttributeModelId && p.AttributeModelId == imatch[cnt].MatchingComponent).FirstOrDefault();
                            // var payval = taxinfo.entity.EntityAttributeModelList.Where(p => p.AttributeModelId == payValue[cnt].AttributeModelId && p.AttributeModelId == imatch[cnt].MatchingComponent).FirstOrDefault();
                             var payval = taxinfo.entity.EntityAttributeModelList.Where(p => p.AttributeModelId == imatch[cnt].MatchingComponent && taxableAttr.Contains(p.AttributeModelId)).FirstOrDefault();
                            if (payval != null)
                            {
                                decimal? projection = Convert.ToDecimal(payval.EntityAttributeValue.Value);
                                projection = projection * ProjectedMonth;
                                if (imatch[cnt].Projection == false)
                                {
                                    projection = 0;
                                }
                                taxinfo.Result.Add(new TaxHistory
                                {
                                    EmployeeId = emp.Id,
                                    FinanceYearId = taxinfo.FinanceYearId,
                                    ApplyDate = taxinfo.EffectiveDate,
                                    FieldId = imatch[cnt].AttributemodelId,
                                    Field = new AttributeModel(imatch[cnt].AttributemodelId, taxinfo.CompanyId).Name,
                                    Actual = 0,
                                    Projection = Math.Round(Convert.ToDecimal(projection) + Convert.ToDecimal(0.01)),
                                    // Total = Math.Round(Convert.ToDecimal(Convert.ToDecimal(payValue[cnt].Value) + projection) + Convert.ToDecimal(0.01)),
                                    Total = Math.Round(Convert.ToDecimal(projection) + Convert.ToDecimal(0.01)),
                                    FieldType = "Income"
                                });
                            }
                        }
                    }
                //other income heads (Gross Income)

                //    TXEmployeeSectionList TXEmpSectionList = new TXEmployeeSectionList(emp.Id,taxinfo.EffectiveDate);

                taxinfo.OtherIncomeHeads.Where(t => t.IncomeTypeId != 4 && t.IncomeTypeId != 5).ToList().ForEach(other =>
                    {
                        TXEmployeeSection declaredComponent = new TXEmployeeSection();
                        if (taxinfo.Proofwise)
                        {
                            declaredComponent = taxinfo.TxEmployeeSectionList.Where(ts => ts.SectionId == other.Id && ts.Proof == true).FirstOrDefault();
                        // declaredComponent = new TXEmployeeSection(Guid.Empty, emp.Id, other.Id, taxinfo.EffectiveDate, true);
                    }
                        else
                        {
                            declaredComponent = taxinfo.TxEmployeeSectionList.Where(ts => ts.SectionId == other.Id && ts.Proof == false).FirstOrDefault();
                        // declaredComponent = new TXEmployeeSection(Guid.Empty, emp.Id, other.Id, taxinfo.EffectiveDate, false);
                    }
                        String otheVal = "0";
                        if (!object.ReferenceEquals(null, declaredComponent))
                        {
                            otheVal = !string.IsNullOrEmpty(declaredComponent.DeclaredValue) ? declaredComponent.DeclaredValue : "0";
                        }
                        decimal otherval = Convert.ToDecimal(otheVal);

                        if (otherval > other.Limit && other.Limit != 0)
                        {
                            otherval = other.Limit;
                        }

                        if (other.Name.Contains("Previous employer income after exemptions u/s 10"))
                        {
                            otherval = txf.Count == 0 ? 0 : otherval;
                        }
                        taxinfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxinfo.FinanceYearId,
                            ApplyDate = taxinfo.EffectiveDate,
                            FieldId = other.Id,
                            Field = other.Name,
                            Total = Math.Round(Convert.ToDecimal(otherval) + Convert.ToDecimal(0.01)),
                            FieldType = "OtherIncome"

                        });
                    });
                });
        }



    }
}

