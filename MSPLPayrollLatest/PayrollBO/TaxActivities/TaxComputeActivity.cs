using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace PayrollBO.TaxActivities
{

    public  class TaxComputeActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxInfo = context.GetValue(this.TaxInfo);

            
            IncomeTaxComputeActivity iTax = new IncomeTaxComputeActivity();
            // taxInfo.TaxBehaviorList = new TaxBehaviorList(taxInfo.FinanceYearId, Guid.Empty, Guid.Empty);


            //Use payrollhistory value for tax Caluclation
            taxInfo.Employees.ForEach(emp =>
            {
                AttributeModel attr = new AttributeModel();
                NetTaxActivity nta = new NetTaxActivity();
                IncrementList increment = new IncrementList();
                increment.AddRange(taxInfo.increment.Where(il => il.EmployeeId == emp.Id).ToList());
                // IncrementList increment = new IncrementList(emp.Id);
                attr = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper().Trim() == "TAXPAID");
                if (attr != null)
                {
                    taxInfo.Result.Add(new TaxHistory
                    {
                        EmployeeId = emp.Id,
                        FinanceYearId = taxInfo.FinanceYearId,
                        ApplyDate = taxInfo.EffectiveDate,
                        FieldId = attr.Id,
                        Field = attr.Name,
                        Total = Convert.ToDecimal(Math.Round(Convert.ToDouble(Math.Round(nta.paidTax(taxInfo, emp),2)) * 1.0) * 1),
                        FieldType = "TAX"
                    });
                }
                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                TaxBehaviorList taxbehaviorlist = new TaxBehaviorList();
                taxbehaviorlist.AddRange(taxInfo.TaxBehaviorList.Where(b => (b.SlabCategory == emp.Gender || b.SlabCategory == 0) && b.FinanceYearId == taxInfo.FinanceYearId ).ToList());
                lstFormulaRecursive.AddRange(taxInfo.Result.Where(r => r.EmployeeId == emp.Id).Select(re => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(re.Total),
                    Id = re.FieldId.ToString(),
                    ExecuteOrder = 4,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false,
                    Name = re.Field
                }));

               //  TXEmployeeSectionList TXEmpSectionList = new TXEmployeeSectionList(emp.Id, taxInfo.EffectiveDate);


                taxbehaviorlist.Where(tb => tb.FieldFor.ToUpper() == "TAX").ToList().ForEach(tb =>
                {
                    tb.Attributename = taxInfo.AttributemodelList.Where(t => t.Id == tb.AttributemodelId).FirstOrDefault().Name;
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
                            TXEmployeeSection txDelaration = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.EffectiveDate == taxInfo.EffectiveDate && ts.SectionId == tb.AttributemodelId & ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                         //   TXEmployeeSection txDelaration1 = TXEmpSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == tb.AttributemodelId && ts.Proof == taxInfo.Proofwise).FirstOrDefault();
                            Decimal val = 0;
                            if (!object.ReferenceEquals(null,txDelaration))
                            {
                                val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                            }
                            // TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, emp.Id, tb.AttributemodelId, taxInfo.EffectiveDate, taxInfo.Proofwise);
                            // Decimal val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, val.ToString(), "100", string.Empty, 4, 9, 1);
                            break;
                        default:
                            iTax.AddValuesTemp(tb, ref lstFormulaRecursive, tb.Value, "100", string.Empty, 4, 0, 1);
                            break;
                    }

                });
                
                lstFormulaRecursive = lstFormulaRecursive.OrderByDescending(u => u.type).ToList();

                iTax.recursive(taxInfo, emp,lstFormulaRecursive, new Entity { },increment);//no need to pass entity
                string rerun = "Y";
                string second_run = "Y";
                do
                {
                    rerun = ""; second_run = "";
                    lstFormulaRecursive.ForEach(u =>
                    {
                        string error = string.Empty;
                        string output = string.Empty;
                        string chk = u.Assignedvalues;

                        if (chk.IndexOf("{") >= 0)
                        {
                            u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output, rerun);
                            if (rerun=="Y")
                            {
                                second_run = "Y";
                            }
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
                                /*    if (u.type == 4)
                                    {
                                        ifElseStmt obj = new ifElseStmt();
                                        List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                                        ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                        var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                        if (!object.ReferenceEquals(tm, null))
                                            output = tm.thenVal;
                                    }
                                    else
                                    if (u.type == 5)
                                    {
                                      output = FormulaRecursive.EvalRange(output, Convert.ToDecimal(u.Output), u, lstFormulaRecursive);
                                    }*/

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
                                    //if (!ReferenceEquals(taxatt, null) && taxatt.Name.Contains("SURCHARGE") && taxatt.Name != "SURCHARGE")
                                    //{
                                    //    taxatt = taxInfo.AttributemodelList.Where(x => x.Name == "SUBCHARGE").FirstOrDefault();
                                    //    if (!ReferenceEquals(taxbehaviorlist.Where(x => x.AttributemodelId == taxatt.Id).FirstOrDefault(), null))
                                    //    {
                                    //        taxbehaviorlist.Where(x => x.AttributemodelId == taxatt.Id).FirstOrDefault().Value = result.ToString();
                                    //        taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                                    //    }
                                    //}
                                    //   else if(!ReferenceEquals(taxatt, null) && taxatt.Name != "SUBCHARGE")
                                    //   {
                                    taxbehaviorlist.Where(x => x.AttributemodelId == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                                    //   }
                                }
                                else
                                {
                                    string str = u.Id;
                                }
                            }
                        }
                    });
                } while (second_run == "Y");
                taxbehaviorlist.ForEach(t =>
                {
                    if (t.Attributename == "TOTTAXPERMONTH")
                    {

                    }
                    taxInfo.Result.Add(new TaxHistory
                    {
                        EmployeeId = emp.Id,
                        FinanceYearId = taxInfo.FinanceYearId,
                        ApplyDate = taxInfo.EffectiveDate,
                        FieldId = t.AttributemodelId,
                        Field = t.Attributename,
                        Total = Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", t.Value)) + Convert.ToDecimal(0.01)),
                        FieldType = "TAX"
                    });

                });
            });

        }
    }
}
