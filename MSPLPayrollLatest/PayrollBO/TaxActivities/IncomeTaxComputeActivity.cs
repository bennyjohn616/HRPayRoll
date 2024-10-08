using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace PayrollBO.TaxActivities
{

    public sealed class IncomeTaxComputeActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxInfo = context.GetValue(this.TaxInfo);


            // taxInfo.TaxBehaviorList = new TaxBehaviorList(taxInfo.FinanceYearId, Guid.Empty, Guid.Empty);
            //Use payrollhistory value for tax Caluclation
            taxInfo.Employees.ForEach(emp =>
            {
                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
                taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => b.SlabCategory == emp.Gender || b.SlabCategory == 0).ToList());
                lstFormulaRecursive.AddRange(taxInfo.Result.Where(r => r.EmployeeId == emp.Id).Select(re => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(Math.Round(Convert.ToDecimal(re.Total) + Convert.ToDecimal(0.01))),
                    Name = re.Field == null ? string.Empty : re.Field,
                    Id = re.FieldId.ToString(),
                    ExecuteOrder = 4,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false
                }));

                EntityAttributeModelList tmplist = new EntityAttributeModelList();

                tmplist.AddRange(taxInfo.entity.EntityAttributeModelList.ToList());

                //tmplist.AddRange(emp.PayrollofMonth.ToList());

                lstFormulaRecursive.ForEach(f =>
                 {
                    var rem = tmplist.Where(w => w.AttributeModelId == new Guid(f.Id));
                    if (rem != null)
                    {
                        tmplist.RemoveAll(r => r.AttributeModelId == new Guid(f.Id));
                    }

                });


                //lstFormulaRecursive.ForEach(f =>
                //{
                //    var rem = emp.PayrollofMonth.Where(w => w.AttributeModelId == new Guid(f.Id));
                //    if (rem != null)
                //    {
                //        emp.PayrollofMonth.RemoveAll(r => r.AttributeModelId == new Guid(f.Id));
                //    }
                //});


                lstFormulaRecursive.ForEach(f =>
                {
                    var rem = emp.PayrollofMonth.Where(w => w.AttributeModelId == new Guid(f.Id));
                    if (rem != null)
                    {
                        emp.PayrollofMonth.RemoveAll(r => r.AttributeModelId == new Guid(f.Id));
                    }
                });

                //                lstFormulaRecursive.AddRange(emp.PayrollofMonth.Select(payhistory => new FormulaRecursive
                lstFormulaRecursive.AddRange(tmplist.Select(payhistory => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(Math.Round(!String.IsNullOrEmpty(payhistory.EntityAttributeValue.Value) ? (Convert.ToDecimal(payhistory.EntityAttributeValue.Value) + Convert.ToDecimal(0.01)) : 0)),
                    Id = payhistory.AttributeModelId.ToString(),
                    Name = taxInfo.AttributemodelList.Where(t => t.Id == payhistory.AttributeModelId).FirstOrDefault().Name,
                    ExecuteOrder = 4,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false,
                    Month = taxInfo.ApplyMonth,
                    Year = taxInfo.ApplyYear,
                    CompanyId = taxInfo.CompanyId
                }));

          //      TXEmployeeSectionList TXEmpSectionList = new TXEmployeeSectionList(emp.Id, taxInfo.EffectiveDate);

                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(tb =>
                    {
                        tb.Attributename = taxInfo.AttributemodelList.Where(t => t.Id == tb.AttributemodelId).FirstOrDefault().Name;
                        switch (tb.Attributename.ToLower())
                        {
                            case "employeeage":
                                tb.Value = Convert.ToString(taxInfo.age);
                                tb.InputType = 1;
                                break;
                            case "employeeserviceyear":
                                tb.Value = Convert.ToString(emp.NoOfServiceYear);
                                tb.InputType = 1;
                                break;
                            case "ismetro":
                                tb.Value = emp.isMetro == true ? Convert.ToString("1") : emp.isMetro == false ? Convert.ToString("0") : Convert.ToString("0");
                                tb.InputType = 1;
                                break;

                        }
                        switch (tb.InputType)
                        {
                            case 1:
                                AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                break;
                            case 3:

                                AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 9, 3);
                                break;
                            case 4:

                                AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 0, 4);
                                break;
                            case 5:

                                AddValuesTemp(tb, ref lstFormulaRecursive, tb.BaseFormula, "100", string.Empty, 6, 0, 5);
                                break;
                            case 2:
                                TXEmployeeSection txDelaration = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == tb.AttributemodelId && ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                                Decimal val = 0;
                                if (!object.ReferenceEquals(null, txDelaration))
                                {
                                    val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                                }
                                // TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, emp.Id, tb.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                                // Decimal val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                                AddValuesTemp(tb, ref lstFormulaRecursive, val.ToString(), "100", string.Empty, 4, 99, 1);
                                break;
                            default:
                                AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                break;
                        }
                    });
                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                recursiveSubsection(taxInfo.AttributemodelList, lstFormulaRecursive, new Entity { }, taxInfo, emp.Id,taxInfo.TxEmployeeSectionList);//no need to pass entity
                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                string rerun = string.Empty;
                lstFormulaRecursive.ForEach(u =>
                {
                    string error = string.Empty;
                    string output = string.Empty;
                    u.Assignformulavalue = u.Assignedvalues;
                    if (u.Assignedvalues.IndexOf("{") > 0)
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
                        // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                    }
                    else
                    {
                        TaxBehavior taxbeh = taxbehaviorlist.Where(b => b.AttributemodelId == new Guid(u.Id)).FirstOrDefault();
                        if (!ReferenceEquals(taxbeh, null))
                        {
                            string result = string.Empty;
                            if (u.type == 4)
                            {
                                //ifElseStmt obj = new ifElseStmt();
                                //List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                                //ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                //var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                if (!object.ReferenceEquals(u.Assignedvalues, null))
                                    output = u.Assignedvalues;
                            }
                            else
                            if (u.type == 5)
                            {

                                //output = FormulaRecursive.EvalRange(taxbeh.Formula, Convert.ToDecimal(u.Output), u, lstFormulaRecursive);
                            }
                            if (taxbeh.FieldType == "-")
                            {
                                output = output + "*(-1)";
                            }
                            Eval eval = new Eval();
                            result = eval.Execute(output).ToString();
                            u.Output = result;
                            u.Assignedvalues = result.ToString();
                            if (!object.ReferenceEquals(taxbeh, null))
                            {
                                taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                            }
                            else
                            {
                                string str = u.Id;
                            }
                        }
                    }
                   
                }); //listformula end

                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(t =>
                {
                    taxInfo.Result.Add(new TaxHistory
                    {
                        EmployeeId = emp.Id,
                        FinanceYearId = taxInfo.FinanceYearId,
                        ApplyDate = taxInfo.EffectiveDate,
                        FieldId = t.AttributemodelId,
                        Field = t.Attributename,
                        Total = Convert.ToDecimal(t.Value.ToLower() == "true" ? "1" : t.Value.ToLower() == "false" ? "0" : t.Value),
                        FieldType = "ITAX"
                    });

                });
            }); //employees End

        }
        public void AddValuesTemp(TaxBehavior u, ref List<FormulaRecursive> lstFormulaRecursive, string AssignValue, string percentage, string eligiblityformula, int executeOrder, int roundingId, int type, bool doRoundoff = true)
        {
            if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributemodelId.ToString().ToUpper()).FirstOrDefault(), null))
            {
                lstFormulaRecursive.Add(new FormulaRecursive()
                {
                    Assignedvalues = AssignValue,
                    Id = u.AttributemodelId.ToString(),
                    Name = u.Attributename == null ? string.Empty : u.Attributename,
                    ExecuteOrder = executeOrder,
                    Rounding = roundingId,
                    type = type,
                    Percentage = percentage,
                    EligibleFormula = eligiblityformula,
                    DoRoundOff = doRoundoff,
                    Assignformulavalue = u.Formula
                });
            }
            else
            {
                //  lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = AssignValue;
            }
        }
        public void recursive(TaxComputationInfo taxInfo, Employee employee, List<FormulaRecursive> lstFormulaRecursive, Entity entity,IncrementList increment)
        {
            lstFormulaRecursive.ForEach(u =>
            {
                u.ReOrder(taxInfo,employee,lstFormulaRecursive, u, entity,increment, false);
            });

/*            lstFormulaRecursive.ForEach(u =>
            {
                if (u.Order == 0)
                {
                    recursive(lstFormulaRecursive, u,entity,false);
                }
            });*/

        }
        public void recursiveSubsection(AttributeModelList attrList, List<FormulaRecursive> lstFormulaRecursive, Entity entity, TaxComputationInfo taxInfo, Guid emp,TXEmployeeSectionList TxEmpSecList)
        {
            var increment = new IncrementList();
            lstFormulaRecursive.ForEach(u =>
            {
               
                if (u.Name.Trim() == "House Rent Allowance" && u.status!="done")// HRA month wise calculation
                {
                    u.Assignedvalues = calculationhra(lstFormulaRecursive, u, taxInfo, emp,increment,TxEmpSecList);
                    u.status = "done";
                }

                u.EmployeeId = emp;
                if (increment == null)
                {
                    increment = new IncrementList(u.EmployeeId);
                }
                Employee employee = taxInfo.Employees.Where(ti => ti.Id == u.EmployeeId).FirstOrDefault();
                u.ReOrder(taxInfo, employee,lstFormulaRecursive, u, entity,increment, false);
            });

            lstFormulaRecursive.ForEach(u =>
            {
                if (u.Order == 0)
                {
                    recursiveSubsection(taxInfo.AttributemodelList, lstFormulaRecursive, entity, taxInfo, emp,TxEmpSecList);
                }
            });

        }
        public string hra(List<FormulaRecursive> listFor, FormulaRecursive currentFor, TaxComputationInfo taxInfo, Guid emp)
        {
            IncomeMatchingList imatch = new IncomeMatchingList();
            taxInfo.incmatchList = null;
            taxInfo.incmatchList = new IncomeMatchingList();
            taxInfo.MoveToClass(taxInfo.StructList, taxInfo.incmatchList);
            imatch.AddRange(taxInfo.incmatchList);//Modified by muthu show all deduction type
            int financeStartMonth = taxInfo.FinanceYear.StartingDate.Month;
            int financeStartYear = taxInfo.FinanceYear.StartingDate.Year;
            PayrollHistoryList payHistory = new PayrollHistoryList(taxInfo.CompanyId, financeStartYear, financeStartMonth, taxInfo.ApplyYear, taxInfo.ApplyMonth, emp);
            TXEmployeeSectionList txEmpSecList = new TXEmployeeSectionList(emp, taxInfo.EffectiveDate);
            TXEmployeeSection txEmpSec = txEmpSecList.Where(v => v.SectionId == taxInfo.AttributemodelList.Where(w => w.Name == "ARP").FirstOrDefault().Id).FirstOrDefault();
            if (!object.ReferenceEquals(txEmpSec, null))
            {
                TxActualRentPaidList ActualRentpaidList = new TxActualRentPaidList(taxInfo.FinanceYearId, emp, txEmpSec.Id);
                decimal[] Ehra = new decimal[12];
                decimal[] Ebmnm = new decimal[12];
                decimal[] Arpeb = new decimal[12];

                DateTime sdate = taxInfo.EffectiveDate;
                List<int> Projectmonth = new List<int>();
                if (!taxInfo.FandFFlag)
                {
                    do
                    {
                        sdate = sdate.AddMonths(1);
                        if (sdate < taxInfo.FinanceYear.EndingDate)
                        {
                            Projectmonth.Add(sdate.Month);
                        }
                    } while (sdate <= taxInfo.FinanceYear.EndingDate);
                }else
                {
                   
                  
                    do
                    {
                        sdate = sdate.AddMonths(1);
                        if (sdate < taxInfo.FinanceYear.EndingDate)
                        {
                            ActualRentpaidList.RemoveAll(a=>a.Month==sdate.Month);
                        }
                    } while (sdate <= taxInfo.FinanceYear.EndingDate);

                }
                
                for (int i = 0; i < ActualRentpaidList.Count; i++)
                {




                    Guid ebId = taxInfo.AttributemodelList.Where(v => v.Name == "EB").FirstOrDefault().Id;
                    Guid ehraId = taxInfo.AttributemodelList.Where(v => v.Name == "EHRA").FirstOrDefault().Id;
                    if (ActualRentpaidList[i].MetroRent == 0 && ActualRentpaidList[i].NonMetroRent == 0)
                    {
                        Ehra[i] = 0;
                        Ebmnm[i] = 0;
                        Arpeb[i] = 0;

                    }
                    else if (ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent || ActualRentpaidList[i].MetroRent < ActualRentpaidList[i].NonMetroRent)
                    {

                        PayrollHistory payHist = payHistory.Where(w => w.Month == ActualRentpaidList[i].Month).FirstOrDefault();
                        decimal eb = 0;
                        decimal ehra = 0;
                        decimal arpeb = 0;
                        if (!object.ReferenceEquals(payHist, null) || Projectmonth.Where(m => m == ActualRentpaidList[i].Month).Count() > 0)
                        {
                            if (!object.ReferenceEquals(payHist, null))
                            {
                                eb = Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == ebId).FirstOrDefault().Value);
                                ehra = Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == ehraId).FirstOrDefault().Value);
                                imatch.ForEach(m =>
                                {
                                    if (m.AttributemodelId == ehraId || m.AttributemodelId == ebId)
                                    {
                                        if (!string.IsNullOrEmpty(m.Formula) || m.Formula != "")
                                        {

                                            m.Formula = m.Formula.Replace("{", String.Empty);
                                            m.Formula = m.Formula.Replace("}", String.Empty);
                                        }
                                        if (m.AttributemodelId == ebId && (m.Operator == "+" || m.Operator == "-"))
                                        {
                                            //eb = m.Operator == "+" ? (eb + Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == new Guid(m.Formula)).FirstOrDefault().Value)) :
                                            //(eb - Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == new Guid(m.Formula)).FirstOrDefault().Value));
                                            eb = m.Operator == "+" ? (eb + manipulateOther(payHist.PayrollHistoryValueList,m, taxInfo.AttributemodelList)) :
                                             (eb - manipulateOther(payHist.PayrollHistoryValueList, m, taxInfo.AttributemodelList));

                                        }
                                        else if (m.AttributemodelId == ehraId && (m.Operator == "+" || m.Operator == "-"))
                                        {
                                         //   ehra = m.Operator == "+" ? (ehra + Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == new Guid(m.Formula)).FirstOrDefault().Value)) :
                                         //(ehra - Convert.ToDecimal(payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == new Guid(m.Formula)).FirstOrDefault().Value));

                                            ehra = m.Operator == "+" ? (ehra + manipulateOther(payHist.PayrollHistoryValueList, m, taxInfo.AttributemodelList)) :
                                             (ehra - manipulateOther(payHist.PayrollHistoryValueList, m, taxInfo.AttributemodelList));
                                        }
                                    }

                                });
                            }
                            else if (Projectmonth.Where(m => m == ActualRentpaidList[i].Month).Count() > 0)
                            {

                                var eBasic = taxInfo.Result.Where(w => w.FieldId == ebId && w.EmployeeId == emp).FirstOrDefault();
                                var eHra = taxInfo.Result.Where(w => w.FieldId == ehraId && w.EmployeeId == emp).FirstOrDefault();

                                ehra = Convert.ToDecimal(eHra.Projection) / Projectmonth.Count;
                                eb = (Convert.ToDecimal(eBasic.Projection) / Projectmonth.Count);

                            }
                            arpeb = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? ActualRentpaidList[i].MetroRent : ActualRentpaidList[i].NonMetroRent;
                            Ehra[i] = (arpeb - eb * Convert.ToDecimal(0.1)) <= 0 ? 0 : ehra;
                            Ebmnm[i] = (arpeb - eb * Convert.ToDecimal(0.1)) <= 0 ? 0 : eb * Convert.ToDecimal(ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? 0.5 : 0.4);
                            Arpeb[i] = (arpeb - eb * Convert.ToDecimal(0.1)) <= 0 ? 0 : (arpeb - eb * Convert.ToDecimal(0.1));
                        }

                        else
                        {
                            Ehra[i] = 0;
                            Ebmnm[i] = 0;
                            Arpeb[i] = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? ActualRentpaidList[i].MetroRent : ActualRentpaidList[i].NonMetroRent;

                        }


                    }

                }
                taxInfo.Result.Where(w => w.Field == "EHRAABS" && w.EmployeeId == emp).FirstOrDefault().Total = Ehra.Sum();
                taxInfo.Result.Where(w => w.Field == "EBMNMABS" && w.EmployeeId == emp).FirstOrDefault().Total = Ebmnm.Sum();
                taxInfo.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total = Arpeb.Sum();

                return Math.Min(Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Ehra.Sum())) + Convert.ToDecimal(0.01)), Math.Min(Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Ebmnm.Sum())) + Convert.ToDecimal(0.01)), Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Arpeb.Sum())) + Convert.ToDecimal(0.01)))).ToString();
            }



            return "0";



        }
        public string calculationhra(List<FormulaRecursive> listFor, FormulaRecursive currentFor, TaxComputationInfo taxInfo, Guid emp,IncrementList increment,TXEmployeeSectionList TxEmpSecList)
        {
            IncomeMatchingList imatch = new IncomeMatchingList();
            taxInfo.incmatchList = null;
            taxInfo.incmatchList = new IncomeMatchingList();
            taxInfo.MoveToClass(taxInfo.StructList, taxInfo.incmatchList);
            imatch.AddRange(taxInfo.incmatchList);//Modified by muthu show all deduction type
            int financeStartMonth = taxInfo.FinanceYear.StartingDate.Month;
            int financeStartYear = taxInfo.FinanceYear.StartingDate.Year;
            PayrollHistoryList payHistory = new PayrollHistoryList();
            var empapplymonth = taxInfo.ApplyMonth - 1;
            var empapplyyear = taxInfo.ApplyYear;
            if (empapplymonth == 0)
            {
                empapplymonth = 12;
                empapplyyear = empapplyyear - 1;
            }
            if (taxInfo.processtype ==  "employee")
            {
                payHistory = new PayrollHistoryList(taxInfo.CompanyId, financeStartYear, financeStartMonth, empapplyyear, empapplymonth, emp);
            }
            else
            {
                payHistory = new PayrollHistoryList(taxInfo.CompanyId, financeStartYear, financeStartMonth, taxInfo.ApplyYear, taxInfo.ApplyMonth, emp);
            }
            //     TXEmployeeSectionList txEmpSecList = new TXEmployeeSectionList(emp, taxInfo.EffectiveDate);
            //     TXSectionList allSection = new TXSectionList(taxInfo.CompanyId, taxInfo.FinanceYearId, Guid.Empty);
            TXSectionList allSection = taxInfo.allsection;
            TXEmployeeSection txEmpSec = TxEmpSecList.Where(v => v.SectionId == taxInfo.AttributemodelList.Where(w => w.Name == "ARP").FirstOrDefault().Id).FirstOrDefault();
            string newtaxscheme = string.Empty;
            AttributeModel attr1 = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "NEWTAXSCHEME");
            TXEmployeeSection declaration;
            DateTime edate1 = taxInfo.EffectiveDate;
            declaration = TxEmpSecList.Where(ts => ts.EmployeeId == emp && ts.SectionId == attr1.Id).FirstOrDefault();
            //declaration = new TXEmployeeSection(Guid.Empty, emp, attr1.Id, edate1, false);
            if (!object.ReferenceEquals(declaration, null))
            {
                if (declaration.DeclaredValue == "1")
                {
                    newtaxscheme = "1";
                }
            }

            if (object.ReferenceEquals(txEmpSec, null))
            {
                DateTime predate = taxInfo.EffectiveDate;
                predate = predate.AddMonths(-1);
                TXEmployeeSectionList txEmpSecLis = new TXEmployeeSectionList(emp,predate);
                 txEmpSec = txEmpSecLis.Where(v => v.SectionId == taxInfo.AttributemodelList.Where(w => w.Name == "ARP").FirstOrDefault().Id).FirstOrDefault();

            }

          //  Employee empl = new Employee(emp);
            Employee empl = taxInfo.Employees.Where(tx => tx.Id == emp).FirstOrDefault();
            TXSection empsec = new TXSection();
            Guid curforguid = new Guid(currentFor.Id);
            empsec = allSection.Where(sec => sec.Id == curforguid).FirstOrDefault();
            
            if (!object.ReferenceEquals(txEmpSec, null))
            {
              if (newtaxscheme == "1")
                {
                    if (empsec.Eligible == false)
                    {
                        return "0";
                    }
                }
                TxActualRentPaidList ActualRentpaidList = new TxActualRentPaidList(taxInfo.FinanceYearId, emp, txEmpSec.Id);
                
                decimal[] Ehra = new decimal[12];
                decimal[] Ebmnm = new decimal[12];
                decimal[] Arpeb = new decimal[12];

                DateTime sdate = taxInfo.EffectiveDate;
                List<int> Projectmonth = new List<int>();
                if (!taxInfo.FandFFlag)
                {
                    do
                    {
                        sdate = sdate.AddMonths(1);
                        if (sdate < taxInfo.FinanceYear.EndingDate)
                        {
                            Projectmonth.Add(sdate.Month);
                        }
                    } while (sdate <= taxInfo.FinanceYear.EndingDate);
                }
                else
                {


                    do
                    {
                        sdate = sdate.AddMonths(1);
                        if (sdate < taxInfo.FinanceYear.EndingDate)
                        {
                            ActualRentpaidList.RemoveAll(a => a.Month == sdate.Month);
                        }
                    } while (sdate <= taxInfo.FinanceYear.EndingDate);

                }

                if (empl.DateOfJoining > taxInfo.FinanceYear.StartingDate && empl.DateOfJoining <= taxInfo.FinanceYear.EndingDate)
                {
                    DateTime date1 = taxInfo.FinanceYear.StartingDate;
                    do
                    {
                        if (((date1.Date.Year * 100) + date1.Date.Month) < ((empl.DateOfJoining.Year * 100) + empl.DateOfJoining.Month))
                        {
                            ActualRentpaidList.RemoveAll(a => a.Month == date1.Month);
                        }
                        date1 = date1.AddMonths(1);
                    } while (date1 <= empl.DateOfJoining);


                }


                for (int i = 0; i < ActualRentpaidList.Count; i++)
                {




                    //Guid ebId = attrlist.Where(v => v.Name == "EB").FirstOrDefault().Id;
                    //Guid ehraId = attrlist.Where(v => v.Name == "EHRA").FirstOrDefault().Id;
                    if (ActualRentpaidList[i].MetroRent == 0 && ActualRentpaidList[i].NonMetroRent == 0)
                    {
                        Ehra[i] = 0;
                        Ebmnm[i] = 0;
                        Arpeb[i] = 0;

                    }
                    else if (ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent || ActualRentpaidList[i].MetroRent < ActualRentpaidList[i].NonMetroRent)
                    {
                        TaxComputationInfo taxInfohra = new TaxComputationInfo();
                        PayrollHistory payHist = payHistory.Where(w => w.Month == ActualRentpaidList[i].Month).FirstOrDefault();
                        decimal arpeb = 0;
                        if (!object.ReferenceEquals(payHist, null) || Projectmonth.Where(m => m == ActualRentpaidList[i].Month).Count() > 0)
                        {
                            arpeb = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? ActualRentpaidList[i].MetroRent : ActualRentpaidList[i].NonMetroRent;
                            if (!object.ReferenceEquals(payHist, null))
                            {

                                TaxHistoryList taxhistorylis = new TaxHistoryList(taxInfo.FinanceYear.Id, empl.Id, taxInfo.EffectiveDate, "");

                                taxhistorylis.RemoveAll(r => r.ActualMonth != ActualRentpaidList[i].Month);
                                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                                TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
                                taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => b.SlabCategory == empl.Gender || b.SlabCategory == 0).ToList());

                                imatch.ForEach(m =>
                                {
                                    var val = payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == m.AttributemodelId).FirstOrDefault();
                                    if (!Object.ReferenceEquals(val,null))
                                    {
                                        if (!string.IsNullOrEmpty(m.Formula) || m.Formula != "")
                                        {

                                            m.Formula = m.Formula.Replace("{", String.Empty);
                                            m.Formula = m.Formula.Replace("}", String.Empty);
                                        }
                                        if (m.AttributemodelId == val.AttributeModelId && (m.Operator == "+" || m.Operator == "-"))
                                        {

                                            payHist.PayrollHistoryValueList.Where(w => w.AttributeModelId == m.AttributemodelId).FirstOrDefault().Value = m.Operator == "+" ? (Convert.ToDecimal(val.Value) + manipulateOther(payHist.PayrollHistoryValueList, m, taxInfo.AttributemodelList)).ToString() :
                                             (Convert.ToDecimal(val.Value) - manipulateOther(payHist.PayrollHistoryValueList, m, taxInfo.AttributemodelList)).ToString();

                                        }
                                       
                                    }

                                });

                                lstFormulaRecursive.AddRange(payHist.PayrollHistoryValueList.Select(payhistory => new FormulaRecursive
                                {
                                    Assignedvalues = Convert.ToString(Math.Round(!String.IsNullOrEmpty(payhistory.Value) ? (Convert.ToDecimal(payhistory.Value) + Convert.ToDecimal(0.01)) : 0)),
                                    Id = payhistory.AttributeModelId.ToString(),
                                    Name = taxInfo.AttributemodelList.Where(t => t.Id == payhistory.AttributeModelId).FirstOrDefault().Name,
                                    ExecuteOrder = 4,
                                    Rounding = 0,
                                    type = 1,
                                    Percentage = "100",
                                    DoRoundOff = false,
                                    Month = taxInfo.ApplyMonth,
                                    Year = taxInfo.ApplyYear,
                                    CompanyId = taxInfo.CompanyId
                                }));
                                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(tb =>
                                {
                                    tb.Attributename = taxInfo.AttributemodelList.Where(t => t.Id == tb.AttributemodelId).FirstOrDefault().Name;
                                    switch (tb.Attributename.ToLower())
                                    {
                                        case "employeeage":
                                            tb.Value = Convert.ToString(taxInfo.age);
                                            tb.InputType = 1;
                                            break;
                                        case "employeeserviceyear":
                                            tb.Value = Convert.ToString(empl.NoOfServiceYear);
                                            tb.InputType = 1;
                                            break;
                                        case "ismetro":
                                            tb.Value = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? Convert.ToString("1") :  Convert.ToString("0");
                                            tb.InputType = 1;
                                            break;

                                        case "arp":
                                            tb.Value = Convert.ToString(arpeb);
                                           
                                            break;

                                    }
                                    switch (tb.InputType)
                                    {
                                        case 1:
                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                            break;
                                        case 3:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 9, 3);
                                            break;
                                        case 4:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 0, 4);
                                            break;
                                        case 5:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.BaseFormula, "100", string.Empty, 6, 0, 5);
                                            break;
                                        case 2:
                                            TXEmployeeSection txDelaration = TxEmpSecList.Where(ts => ts.EmployeeId == empl.Id && ts.SectionId == tb.AttributemodelId && ts.EffectiveDate == taxInfo.EffectiveDate && ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                                            //   Guid.Empty, empl.Id, tb.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                                            decimal val = 0;
                                            if (!object.ReferenceEquals(null,txDelaration))
                                            {
                                                val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                                            }
                                            AddValuesTemp(tb, ref lstFormulaRecursive, (tb.Attributename.ToLower()=="arp"?arpeb.ToString():val.ToString()), "100", string.Empty, 4, 99, 1);
                                            break;
                                        default:
                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                            break;
                                    }
                                });
                                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                                recursive(taxInfo, empl,lstFormulaRecursive, new Entity { },increment);//no need to pass entity
                                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                                lstFormulaRecursive.ForEach(u =>
                                {
                                    string error = string.Empty;
                                    string output = string.Empty;
                                    u.Assignformulavalue = u.Assignedvalues;
                                    string chk = string.Empty;
                                    chk = u.Assignedvalues;
                                    string rerun = string.Empty;
                                    if (chk.IndexOf("{") >= 0)
                                    {
                                        u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output,rerun);
                                    }
                                    else
                                    {
                                        output = u.Assignedvalues;
                                    }
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        taxInfo.Errors.Add("There is a some problem in formula.Please check it.");
                                        // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                                    }
                                    else
                                    {
                                        TaxBehavior taxbeh = taxbehaviorlist.Where(b => b.AttributemodelId == new Guid(u.Id)).FirstOrDefault();
                                        if (!ReferenceEquals(taxbeh, null))
                                        {


                                            string result = string.Empty;
                                            if (u.type == 4)
                                            {
                                                
                                                if (!object.ReferenceEquals(u.Assignedvalues, null))
                                                    output = u.Assignedvalues;
                                            }
                                            else
                                            if (u.type == 5)
                                            {

                                            }
                                            if (taxbeh.FieldType == "-")
                                            {
                                                output = output + "*(-1)";
                                            }
                                            Eval eval = new Eval();
                                            result = eval.Execute(output).ToString();
                                            u.Output = result;
                                            u.Assignedvalues = result.ToString();
                                            if (!object.ReferenceEquals(taxbeh, null))
                                            {
                                                taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                                            }
                                            else
                                            {
                                                string str = u.Id;
                                            }
                                        }
                                    }
                                }); //listformula end
                                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(t =>
                                {
                                    taxInfohra.Result.Add(new TaxHistory
                                    {
                                        EmployeeId = empl.Id,
                                        FinanceYearId = taxInfo.FinanceYearId,
                                        ApplyDate = taxInfo.EffectiveDate,
                                        FieldId = t.AttributemodelId,
                                        Field = t.Attributename,
                                        Total = Convert.ToDecimal(t.Value.ToLower() == "true" ? "1" : t.Value.ToLower() == "false" ? "0" : t.Value),
                                        FieldType = "ITAX"
                                    });

                                });
                            }
                            else if (Projectmonth.Where(m => m == ActualRentpaidList[i].Month).Count() > 0)
                            {

                                TaxHistoryList taxhistorylis = new TaxHistoryList(taxInfo.FinanceYear.Id, empl.Id, taxInfo.EffectiveDate, "");

                                taxhistorylis.RemoveAll(r => r.ActualMonth != ActualRentpaidList[i].Month);
                                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                                TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
                                taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => b.SlabCategory == empl.Gender || b.SlabCategory == 0).ToList());




                                lstFormulaRecursive.AddRange(taxInfo.Result.Where(r => r.EmployeeId == empl.Id && r.FieldType=="Income").Select(re => new FormulaRecursive
                                {
                                    Assignedvalues = Convert.ToString(Math.Round(Convert.ToDecimal(re.Projection/ Projectmonth.Count) + Convert.ToDecimal(0.01))),
                                    Name = re.Field == null ? string.Empty : re.Field,
                                    Id = re.FieldId.ToString(),
                                    ExecuteOrder = 4,
                                    Rounding = 0,
                                    type = 1,
                                    Percentage = "100",
                                    DoRoundOff = false
                                }));
                         
                                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(tb =>
                                {
                                    tb.Attributename = taxInfo.AttributemodelList.Where(t => t.Id == tb.AttributemodelId).FirstOrDefault().Name;
                                    switch (tb.Attributename.ToLower())
                                    {
                                        case "employeeage":
                                            tb.Value = Convert.ToString(taxInfo.age);
                                            tb.InputType = 1;
                                            break;
                                        case "employeeserviceyear":
                                            tb.Value = Convert.ToString(empl.NoOfServiceYear);
                                            tb.InputType = 1;
                                            break;
                                        case "ismetro":
                                            tb.Value = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? Convert.ToString("1") : Convert.ToString("0");
                                            tb.InputType = 1;
                                            break;

                                        case "arp":
                                            tb.Value = Convert.ToString(arpeb);

                                            break;

                                    }
                                    switch (tb.InputType)
                                    {
                                        case 1:
                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                            break;
                                        case 3:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 9, 3);
                                            break;
                                        case 4:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Formula, "100", string.Empty, 5, 0, 4);
                                            break;
                                        case 5:

                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.BaseFormula, "100", string.Empty, 6, 0, 5);
                                            break;
                                        case 2:
                                            TXEmployeeSection txDelaration = TxEmpSecList.Where(ts => ts.EmployeeId == empl.Id && ts.SectionId == tb.AttributemodelId && ts.EffectiveDate == taxInfo.EffectiveDate && ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                                            // TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, empl.Id, tb.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                                            decimal val = 0;
                                            if (!object.ReferenceEquals(null,txDelaration))
                                            {
                                                val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                                            }
                                            AddValuesTemp(tb, ref lstFormulaRecursive, (tb.Attributename.ToLower() == "arp" ? arpeb.ToString() : val.ToString()), "100", string.Empty, 4, 99, 1);
                                            break;
                                        default:
                                            AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                                            break;
                                    }
                                });
                                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                                recursive(taxInfo, empl,lstFormulaRecursive, new Entity { },increment);//no need to pass entity
                                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                                lstFormulaRecursive.ForEach(u =>
                                {
                                    string error = string.Empty;
                                    string output = string.Empty;
                                    u.Assignformulavalue = u.Assignedvalues;
                                    string chk = u.Assignedvalues;
                                    string rerun = string.Empty;
                                    if (chk.IndexOf("{") >= 0)
                                    {
                                        u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output,rerun);
                                    }
                                    else
                                    {
                                        output = u.Assignedvalues;
                                    }
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        taxInfo.Errors.Add("There is a some problem in formula.Please check it.");
                                        // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                                    }
                                    else
                                    {
                                        TaxBehavior taxbeh = taxbehaviorlist.Where(b => b.AttributemodelId == new Guid(u.Id)).FirstOrDefault();
                                        if (!ReferenceEquals(taxbeh, null))
                                        {


                                            string result = string.Empty;
                                            if (u.type == 4)
                                            {
                                                //ifElseStmt obj = new ifElseStmt();
                                                //List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                                                //ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                                //var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                                if (!object.ReferenceEquals(u.Assignedvalues, null))
                                                    output = u.Assignedvalues;
                                            }
                                            else
                                            if (u.type == 5)
                                            {

                                                //output = FormulaRecursive.EvalRange(taxbeh.Formula, Convert.ToDecimal(u.Output), u, lstFormulaRecursive);
                                            }
                                            if (taxbeh.FieldType == "-")
                                            {
                                                output = output + "*(-1)";
                                            }
                                            Eval eval = new Eval();
                                            result = eval.Execute(output).ToString();
                                            u.Output = result;
                                            u.Assignedvalues = result.ToString();
                                            if (!object.ReferenceEquals(taxbeh, null))
                                            {
                                                taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                                            }
                                            else
                                            {
                                                string str = u.Id;
                                            }
                                        }
                                    }
                                }); //listformula end
                                taxbehaviorlist.Where(tb => tb.FieldFor == "ITAX").ToList().ForEach(t =>
                                {
                                    taxInfohra.Result.Add(new TaxHistory
                                    {
                                        EmployeeId = empl.Id,
                                        FinanceYearId = taxInfo.FinanceYearId,
                                        ApplyDate = taxInfo.EffectiveDate,
                                        FieldId = t.AttributemodelId,
                                        Field = t.Attributename,
                                        Total = Convert.ToDecimal(t.Value.ToLower() == "true" ? "1" : t.Value.ToLower() == "false" ? "0" : t.Value),
                                        FieldType = "ITAX"
                                    });

                                });

                                //var eBasic = taxInfo.Result.Where(w => w.FieldId == ebId && w.EmployeeId == emp).FirstOrDefault();
                                //var eHra = taxInfo.Result.Where(w => w.FieldId == ehraId && w.EmployeeId == emp).FirstOrDefault();

                                //ehra = Convert.ToDecimal(eHra.Projection) / Projectmonth.Count;
                                //eb = (Convert.ToDecimal(eBasic.Projection) / Projectmonth.Count);

                            }
                            
                            Ehra[i] = (taxInfohra.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total) <= 0 ? 0 : Convert.ToDecimal(taxInfohra.Result.Where(w => w.Field == "EHRAABS" && w.EmployeeId == emp).FirstOrDefault().Total);
                            Ebmnm[i] = (taxInfohra.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total) <= 0 ? 0 : Convert.ToDecimal(taxInfohra.Result.Where(w => w.Field == "EBMNMABS" && w.EmployeeId == emp).FirstOrDefault().Total);
                            Arpeb[i] = (taxInfohra.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total) <= 0 ? 0 : Convert.ToDecimal(taxInfohra.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total);
                        }

                        else
                        {
                            Ehra[i] = 0;
                            Ebmnm[i] = 0;
                            Arpeb[i] = ActualRentpaidList[i].MetroRent >= ActualRentpaidList[i].NonMetroRent ? ActualRentpaidList[i].MetroRent : ActualRentpaidList[i].NonMetroRent;

                        }


                    }

                }
                taxInfo.Result.Where(w => w.Field == "EHRAABS" && w.EmployeeId == emp).FirstOrDefault().Total = Ehra.Sum();
                taxInfo.Result.Where(w => w.Field == "EBMNMABS" && w.EmployeeId == emp).FirstOrDefault().Total = Ebmnm.Sum();
                taxInfo.Result.Where(w => w.Field == "ARPABS" && w.EmployeeId == emp).FirstOrDefault().Total = Arpeb.Sum();

                return Math.Min(Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Ehra.Sum())) + Convert.ToDecimal(0.01)), Math.Min(Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Ebmnm.Sum())) + Convert.ToDecimal(0.01)), Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", Arpeb.Sum())) + Convert.ToDecimal(0.01)))).ToString();
            }

            return "0";



        }

        public decimal manipulateOther(PayrollHistoryValueList pv, IncomeMatching formula,AttributeModelList attr)
        {
            string[] valuesAdd = formula.OtherComponent.Trim().Split('+');
            string formulaa = formula.Formula;
            List<decimal> sum = new List<decimal>();
            
            valuesAdd.Where(f=>f!="").ToList().ForEach(f =>
            {
                if (f.IndexOf('-') != -1)
                {
                    f.Split('-').ToList().ForEach(s =>
                    {
                        var att = attr.Where(w => w.Name == s).FirstOrDefault();
                        var val = pv.Where(w => w.AttributeModelId == att.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(att, null))
                        {
                            formulaa = val == null?"0": val.Value;
                            //sum.Add(-1*Convert.ToDecimal(pv.Where(w => w.AttributeModelId == att.Id).FirstOrDefault().Value));
                        }

                    });

                }
                else
                {

                    var att = attr.Where(w => w.Name == f).FirstOrDefault();
                    var val = pv.Where(w => w.AttributeModelId == att.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(att, null))
                    {
                        formulaa = val == null ? "0" : val.Value;
                       // sum.Add(Convert.ToDecimal(pv.Where(w => w.AttributeModelId == att.Id).FirstOrDefault().Value));
                    }

                }

            });

            Eval eval = new Eval();
            return Convert.ToDecimal(eval.Execute(formulaa));

        }


    }
}
