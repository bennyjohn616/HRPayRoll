using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using PayrollBO.TaxActivities;

namespace PayrollBO.Tax
{

    public sealed class OneTimeTaxActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxInfo = context.GetValue(this.TaxInfo);

            //One time tax 
            taxInfo.Employees.ForEach(emp =>
            {
                TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
                taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => b.SlabCategory == emp.Gender || b.SlabCategory == 0).ToList());
                emp.PayrollHistoryList = new PayrollHistoryValueList();
                IncrementList increment = new IncrementList(emp.Id);
                IncomeMatchingList imatch = new IncomeMatchingList();
                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();

                /*                                            */
                int financeStartMonth = taxInfo.FinanceYear.StartingDate.Month;
                int financeStartYear = taxInfo.FinanceYear.StartingDate.Year;
                PayrollHistoryList payHistory = new PayrollHistoryList();
                //if (taxInfo.emproll.ToUpper() != "ADMIN")
                //{
                //    payHistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year * 100) + ph.Month) >= ((financeStartYear * 100) + financeStartMonth) && ((ph.Year * 100) + ph.Month) < (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                //    //payHistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ph.Year >= financeStartYear && ph.Month >= financeStartMonth && ph.Year <= taxInfo.ApplyYear && ph.Month < taxInfo.ApplyMonth && ph.EmployeeId == emp.Id).ToList());
                //}
                //else
                //{
                payHistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year * 100) + ph.Month) >= (financeStartYear * 100 + financeStartMonth) && ((ph.Year * 100) + ph.Month) <= (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                    //payHistory.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ph.Year >= financeStartYear && ph.Month >= financeStartMonth && ph.Year <= taxInfo.ApplyYear && ph.Month <= taxInfo.ApplyMonth && ph.EmployeeId == emp.Id).ToList());
                //}
                var lastmonth = 0;
                var lastyear = 0;
                lastyear = taxInfo.ApplyYear;
                lastmonth = taxInfo.ApplyMonth - 1;
                if (lastmonth == 0)
                {
                    lastmonth = 12;
                    lastyear = lastyear - 1;
                }
                //int count = 0;
                //for (int n = 0; n < payHistory.Count; n++)
                //{
                //    if (payHistory[n].Month == taxInfo.ApplyMonth && payHistory[n].Year == taxInfo.ApplyYear)
                //    {
                //        count = count + 1;
                //    }
                //}

                //if (count == 0 && taxInfo.emproll.ToUpper() !== "ADMIN")
                //{
                //    emp.PayrollofMonth = new PayrollHistory(taxInfo.CompanyId, emp.Id, Lastyear, Lastmonth).PayrollHistoryValueList;
                //    // emp.PayrollofMonth = payHistory.Where(ph => ph.Month == Lastmonth && ph.Year == Lastyear).FirstOrDefault().PayrollHistoryValueList;
                //}
                //else
                //{
                //    emp.PayrollofMonth = new PayrollHistory(taxInfo.CompanyId, emp.Id, taxInfo.ApplyYear, taxInfo.ApplyMonth).PayrollHistoryValueList;
                //    // emp.PayrollofMonth = payHistory.Where(ph => ph.Month == taxInfo.ApplyMonth && ph.Year == taxInfo.ApplyYear).FirstOrDefault().PayrollHistoryValueList;
                //}

                /*                                             */
                emp.PayrollofMonth = new PayrollHistoryValueList();

        /*        if (taxInfo.emproll.ToUpper() != "ADMIN")
                {
                    emp.PayrollofMonth = new PayrollHistory(taxInfo.CompanyId, emp.Id, lastyear, lastmonth).PayrollHistoryValueList;
                }
                else
                {*/
                emp.PayrollofMonth = new PayrollHistory(taxInfo.CompanyId, emp.Id, taxInfo.ApplyYear, taxInfo.ApplyMonth).PayrollHistoryValueList;


                lstFormulaRecursive.AddRange(emp.PayrollofMonth.Select(payhistory => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(Math.Round(!String.IsNullOrEmpty(payhistory.Value) ? (Convert.ToDecimal(payhistory.Value) + Convert.ToDecimal(0.01)) : 0)),
                    Id = payhistory.AttributeModelId.ToString(),
                    Name = "",
                    ExecuteOrder = 6,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false
                }));
                taxInfo.incmatchList = null;
                taxInfo.incmatchList = new IncomeMatchingList();
                taxInfo.MoveToClass(taxInfo.StructList, taxInfo.incmatchList);
                imatch.AddRange(taxInfo.incmatchList.Where(i => i.TaxDeductionMode == "ONETIME").ToList());
                // imatch.AddRange(new IncomeMatchingList(taxInfo.FinanceYearId).Where(i => i.TaxDeductionMode == "ONETIME").ToList());
                decimal totOnetimeTax = 0;
                decimal totOnetimeTaxExemp = 0;

                imatch.ForEach(i =>
                {
                    FormulaRecursive lstMatch = lstFormulaRecursive.Where(r => new Guid(r.Id) == i.AttributemodelId).FirstOrDefault();
                    if (lstMatch != null)
                    {
                        if (Convert.ToString(lstMatch.Assignedvalues) != "0")
                            //totOnetimeTax = totOnetimeTax + (taxInfo.Result.Where(x => x.FieldType == "Income").Sum(x => x.Actual).Value +
                            //                taxInfo.Result.Where(x => x.FieldType == "Income").Sum(x => x.Projection).Value - Convert.ToDecimal(lstMatch.Assignedvalues));
                            totOnetimeTax = totOnetimeTax + Convert.ToDecimal(lstMatch.Assignedvalues);
                        else
                            totOnetimeTax = totOnetimeTax + 0;
                    }
                });

                DateTime prevmonth = taxInfo.EffectiveDate.AddMonths(-1);
 
                imatch.Where(i => i.ExemptionComponent != Guid.Empty).ToList().ForEach(i =>
                    {
                        TaxHistory txHis = taxInfo.Result.Where(r => r.FieldId == i.ExemptionComponent).FirstOrDefault();
                        if (txHis != null)
                        {
                            if (Convert.ToString(txHis.Actual) != "0")
                                //totOnetimeTax = totOnetimeTax + (taxInfo.Result.Where(x => x.FieldType == "Income").Sum(x => x.Actual).Value +
                                //                taxInfo.Result.Where(x => x.FieldType == "Income").Sum(x => x.Projection).Value - Convert.ToDecimal(lstMatch.Assignedvalues));
                                totOnetimeTaxExemp = totOnetimeTaxExemp +( Convert.ToDecimal(txHis.Actual)-getPrevOTExemption(i.ExemptionComponent,taxInfo.FinanceYearId,emp.Id, prevmonth));
                            else
                                totOnetimeTaxExemp = totOnetimeTaxExemp + 0;
                        }
                    });
                totOnetimeTax = totOnetimeTax - totOnetimeTaxExemp;
                
                //FormulaRecursive onetaxFormula = new FormulaRecursive
                //{
                //    Assignedvalues = Convert.ToString(totOnetimeTax),
                //    Id = Convert.ToString(Id),
                //    Name = "ONETIMETAX",
                //    ExecuteOrder = 6,
                //    Rounding = 0,
                //    type = 5,
                //    Percentage = "100",
                //    DoRoundOff = false
                //};
                //lstFormulaRecursive.Add(onetaxFormula);
                //TaxBehavior taxbeh = taxbehaviorlist.Where(b => b.Attributename != null).ToList().Where(b => b.Attributename.IndexOf("ONETIMETAX") > -1).FirstOrDefault();
                //if (taxbeh != null)
                //{

                //    //    string output = FormulaRecursive.EvalRange(taxbeh.Formula, Convert.ToDecimal(totOnetimeTax), onetaxFormula,lstFormulaRecursive);
                //    IncomeTaxComputeActivity iact = new IncomeTaxComputeActivity();
                //    switch (taxbeh.InputType)
                //    {
                //        case 1:
                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, taxbeh.Value, "100", string.Empty, 4, 0, 1);
                //            break;
                //        case 3:

                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, taxbeh.Formula, "100", string.Empty, 5, 0, 3);
                //            break;
                //        case 4:

                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, taxbeh.Formula, "100", string.Empty, 6, 0, 4);
                //            break;
                //        case 5:

                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, taxbeh.BaseFormula, "100", string.Empty, 5, 0, 5);
                //            break;
                //        case 2:
                //            TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, emp.Id, taxbeh.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, txDelaration.DeclaredValue, "100", string.Empty, 4, 0, 1);
                //            break;
                //        default:
                //            iact.AddValuesTemp(taxbeh, ref lstFormulaRecursive, taxbeh.Value, "100", string.Empty, 4, 0, 1);
                //            break;
                //    }

                //    lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                //    iact.recursive(lstFormulaRecursive, new Entity { });//no need to pass entity
                //    lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                //    lstFormulaRecursive.Where(l => l.Name == "ONETIMETAX").ToList().ForEach(u =>
                //    {
                //        string error = string.Empty;
                //        string output = string.Empty;
                //        u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output);
                //        if (!string.IsNullOrEmpty(error))
                //        {
                //            taxInfo.Errors.Add("There is a some problem in formula.Please check it.");
                //        // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                //    }
                //        else
                //        {
                //            taxbeh = taxbehaviorlist.Where(b => b.AttributemodelId == new Guid(u.Id)).FirstOrDefault();
                //            if (!ReferenceEquals(taxbeh, null))
                //            {


                //                string result = string.Empty;
                //                if (u.type == 4)
                //                {
                //                    ifElseStmt obj = new ifElseStmt();
                //                    List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                //                    ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                //                    var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                //                    if (!object.ReferenceEquals(tm, null))
                //                        output = tm.thenVal;
                //                }
                //                else
                //                if (u.type == 5)
                //                {

                //                //  output = FormulaRecursive.EvalRange(taxbeh.Formula, Convert.ToDecimal(u.Output), u, lstFormulaRecursive);
                //            }
                //                if (taxbeh.FieldType == "-")
                //                {
                //                    output = output + "*(-1)";
                //                }
                //                Eval eval = new Eval();
                //                result = eval.Execute(output).ToString();
                //                u.Output = result;
                //                u.Assignedvalues = result.ToString();
                //                if (!object.ReferenceEquals(taxbeh, null))
                //                {
                //                    taxbeh.Value = result.ToString();
                //                }
                //                else
                //                {
                //                    string str = u.Id;
                //                }
                //            }
                //        }
                //    }); //listformula end
                //        //  Eval eval = new Eval();
                //        //     output = eval.Execute(output).ToString();
                //    lstFormulaRecursive.Where(l => l.Name == "ONETIMETAX").ToList().ForEach(l =>
                //     {
                //         taxInfo.Result.Add(new TaxHistory
                //         {
                //             EmployeeId = emp.Id,
                //             FinanceYearId = taxInfo.FinanceYearId,
                //             ApplyDate = taxInfo.EffectiveDate,
                //             FieldId = taxbeh.AttributemodelId,
                //             Field = taxbeh.Attributename,

                //             Total = Convert.ToDecimal(taxbeh.Value),
                //             FieldType = "ONETIMETAX"
                //         });
                //         taxInfo.Result.Where(r => r.Field == "NETTAX").FirstOrDefault().Total = taxInfo.Result.Where(r => r.Field == "NETTAX").FirstOrDefault().Total+Convert.ToDecimal(taxbeh.Value);



                //     });
                //}
                // TaxComputeActivity oneTimeTax = new TaxComputeActivity();
                // oneTimeTax.
                #region without One time component value Total income -one time component
                decimal TotalIncome = 0;
                var totIncome = taxInfo.Result.Where(x => x.FieldType == "Total Income" && x.EmployeeId == emp.Id).FirstOrDefault();
                if (totIncome != null)
                {
                    TotalIncome = totIncome != null ? Convert.ToDecimal(totIncome.Total) : 0;
                    taxInfo.Result.Where(x => x.FieldType == "Total Income" && x.EmployeeId == emp.Id).FirstOrDefault().Total = TotalIncome - totOnetimeTax;// one time tax calculation without one time component value
                }

                OneTimeTaxCalculation(taxInfo, emp, "TAX", totOnetimeTax,increment);

                taxInfo.Result.Where(x => x.FieldType == "Total Income" && x.EmployeeId == emp.Id).FirstOrDefault().Total = TotalIncome;// assign original value
                #endregion
            });

        }

        /// <summary>
        /// One Time Tax Calculation 
        /// </summary>
        /// <param name="taxInfo"></param>
        /// <param name="emp"></param>
        /// <param name="taxType"></param>
        /// <param name="totOneTimeTax"></param>
        private void OneTimeTaxCalculation(TaxComputationInfo taxInfo, Employee emp, string taxType, decimal totOneTimeTax,IncrementList increment)
        {
            IncomeTaxComputeActivity iTax = new IncomeTaxComputeActivity();
            List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
            TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
            taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => b.SlabCategory == emp.Gender || b.SlabCategory == 0).ToList());

            if (totOneTimeTax == 0)
            {
                taxbehaviorlist.ForEach(t =>
                {
                    if (t.Attributename == "ONETIMETAX")
                    {
                        taxInfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxInfo.FinanceYearId,
                            ApplyDate = taxInfo.EffectiveDate,
                            FieldId = t.AttributemodelId,
                            Field = t.Attributename,
                            Total = totOneTimeTax,// One time component  value is zero 
                            FieldType = taxType
                        });
                    }
                });
            }
            else
            {

                #region OneTime Tax process 

                lstFormulaRecursive.AddRange(taxInfo.Result.Where(r => r.EmployeeId == emp.Id && r.FieldType != "ITAX" && r.FieldType != "TAX").Select(re => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(Math.Round(Convert.ToDecimal(re.Total) + Convert.ToDecimal(0.01))),
                    Id = re.FieldId.ToString(),
                    ExecuteOrder = 4,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false,
                    Name = re.Field,
                }));

 
                taxbehaviorlist.Where(tb => tb.FieldFor.ToUpper() == "TAX" || tb.FieldFor.ToUpper() == "ITAX").ToList().ForEach(tb =>
                {
                    tb.Attributename = taxInfo.AttributemodelList.Where(t => t.Id == tb.AttributemodelId).FirstOrDefault().Name;
                    //if (tb.BaseValue == "TOTINCOME")
                    //{
                    //    tb.BaseFormula = tb.BaseFormula + "-" + totOneTimeTax;
                    //}
                    switch (tb.InputType)
                    {
                        case 1:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                            break;
                        case 3:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 9, 3);
                            break;
                        case 4:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 6, 0, 4);
                            break;
                        case 5:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.BaseFormula, "100", string.Empty, 5, 0, 5);
                            break;
                        case 2:
                            TXEmployeeSection txDelaration = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == tb.AttributemodelId && ts.EffectiveDate == taxInfo.EffectiveDate && ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                            // TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, emp.Id, tb.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                            decimal val = 0;
                            if (!object.ReferenceEquals(null, txDelaration))
                            {
                               val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                            }
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, val.ToString(), "100", string.Empty, 4, 9, 1);
                            break;
                        default:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                            break;
                    }

                });
                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                iTax.recursive(taxInfo, emp,lstFormulaRecursive, new Entity { },increment);//no need to pass entity
                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                int j = 0;
                lstFormulaRecursive.ForEach(u =>
                {
                    j++;
                    string error = string.Empty;
                    string output = string.Empty;
                    string rerun = string.Empty;
                    string chk = string.Empty;
                    chk = u.Assignedvalues;
                 /*   if(u.Name== "TOTTAX" || u.Name == "TOTITAX")
                    {

                        Console.Write(u.Name + "" + j);
                    }*/
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
                        taxInfo.Errors.Add("There is a some problem in formula.Please check it.");
                    }
                    else
                    {
                        TaxBehavior taxbeh = taxbehaviorlist.Where(b => b.AttributemodelId == new Guid(u.Id)).FirstOrDefault();
                        if (!ReferenceEquals(taxbeh, null))
                        {
                            string result = string.Empty;
                            if (u.type == 4)
                            {
                                if (!object.ReferenceEquals(u.Output, null))
                                    output = u.Output;
                            }
                            if (taxbeh.FieldType == "-")
                            {
                                output = output + "*(-1)";
                            }
                            Eval eval = new Eval();
                            result = eval.Execute(output).ToString();
                            u.Output = result;
                            u.Assignedvalues = result.ToString();
                            u.Assignformulavalue = result.ToString();
                            if (!object.ReferenceEquals(taxbeh, null))
                            {
                                AttributeModel taxatt = taxInfo.AttributemodelList.Where(x => x.Id == new Guid(u.Id)).FirstOrDefault();
                                taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                            }
                            else
                            {
                                string str = u.Id;
                            }
                        }
                    }
                });
                taxbehaviorlist.ForEach(t =>
                {
                    if (t.Attributename == "ONETIMETAX")
                    {
                        //taxInfo.Result.Add(new TaxHistory
                        //{
                        //    EmployeeId = emp.Id,
                        //    FinanceYearId = taxInfo.FinanceYearId,
                        //    ApplyDate = taxInfo.EffectiveDate,
                        //    FieldId = t.AttributemodelId,
                        //    Field = t.Attributename,
                        //    Total = Convert.ToDecimal(taxInfo.Result.Where(x => x.Field == "TOTTAX" &&x.EmployeeId==emp.Id).FirstOrDefault().Total) -
                        //            Convert.ToDecimal(taxbehaviorlist.Where(x => x.Attributename == "TOTTAX").FirstOrDefault().Value),//WITH INCOME TAX AND WITH OUT INCOME TAX DIFFRENCE
                        //    FieldType = taxType
                        //});
                        
                        taxInfo.Result.Where(x => x.Field == "ONETIMETAX" && x.EmployeeId == emp.Id).FirstOrDefault().Total =
                                             Convert.ToDecimal(taxInfo.Result.Where(x => x.Field == "TOTTAX" && x.EmployeeId == emp.Id).FirstOrDefault().Total) -
                                             Convert.ToDecimal(taxbehaviorlist.Where(x => x.Attributename == "TOTTAX").FirstOrDefault().Value);//WITH INCOME TAX AND WITH OUT INCOME TAX DIFFRENCE
                        decimal ottax = Convert.ToDecimal(taxInfo.Result.Where(x => x.Field == "TOTTAX" && x.EmployeeId == emp.Id).FirstOrDefault().Total);
                        decimal ottax1 = Convert.ToDecimal(taxbehaviorlist.Where(x => x.Attributename == "TOTTAX").FirstOrDefault().Value);
                    }
                });
                #endregion
            }
        }
        public decimal getPrevOTExemption(Guid ottExemptionId, Guid financeYrId, Guid empId, DateTime prevApplydate)
         {
            decimal prevExem = 0;
            TaxHistoryList txHis = new TaxHistoryList(financeYrId, empId, prevApplydate);

            txHis.Where(h => h.FieldId == ottExemptionId).OrderBy(h => h.ApplyDate).ToList().ForEach(t =>
                  {
                      prevExem = prevExem + t.Total == null || t.Total == 0 ? t.Actual.Value : t.Total.Value;
                  });
            return prevExem;
        }



    }
}
