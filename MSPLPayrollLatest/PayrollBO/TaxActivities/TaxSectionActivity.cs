using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace PayrollBO.TaxActivities
{

    public sealed class TaxSectionActivity : CodeActivity
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
            TXSectionList txInfoSections = new TXSectionList();

            TXSectionList txInfoSubSections = new TXSectionList();
            IncomeMatchingList imatchList = new IncomeMatchingList();
            // since this is not going to affect the class values struct has not been move here 
            imatchList.AddRange(taxInfo.incmatchList);

            taxInfo.Employees.ForEach(emp =>
            {
                TXSectionList allSection =
   new TXSectionList(taxInfo.CompanyId, taxInfo.FinanceYearId, Guid.Empty);
                txInfoSubSections.AddRange(allSection.Where(s => s.ParentId != Guid.Empty).OrderBy(s => s.OrderNo).ToList());
                txInfoSections.AddRange(allSection.Where(s => s.ParentId == Guid.Empty && s.SectionType != "Others").OrderBy(s => s.OrderNo).ToList());

                /* new changes done */
                string newtaxscheme = string.Empty;
                //          TXEmployeeSectionList TXEmpSectionList = new TXEmployeeSectionList(emp.Id, taxInfo.EffectiveDate);
                AttributeModel attr1 = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "NEWTAXSCHEME");
                TXEmployeeSection newtaxdecl = new TXEmployeeSection();
                DateTime edate1 = taxInfo.EffectiveDate;
                // newtaxdecl = TXEmpSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == attr1.Id).FirstOrDefault();
                newtaxdecl = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == attr1.Id).FirstOrDefault();
                // newtaxdecl = new TXEmployeeSection(Guid.Empty, emp.Id, attr1.Id, edate1, false);
                if (!object.ReferenceEquals(newtaxdecl, null))
                {
                    if (newtaxdecl.DeclaredValue == "1")
                    {
                        newtaxscheme = "1";
                    }
                }



                var otherincome = new Action(() =>
                {

                    List<TXSection> list = new List<TXSection>();
                    list.AddRange(allSection.Where(s => s.SectionType == "Others" && (s.IncomeTypeId == 4 || s.IncomeTypeId == 5)).OrderBy(s => s.OrderNo));

                    list.ForEach(other =>
                    {
                        string otheVal;
                        decimal otherval;
                        TXEmployeeSection declaredComponent;
                        if (taxInfo.Proofwise)
                        {
                            declaredComponent = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == other.Id && ts.Proof == true).FirstOrDefault();
                            //declaredComponent = new TXEmployeeSection(Guid.Empty, emp.Id, other.Id, taxInfo.EffectiveDate, true);
                        }
                        else
                        {
                            declaredComponent = taxInfo.TxEmployeeSectionList.Where(ts => ts.EmployeeId == emp.Id && ts.SectionId == other.Id && ts.Proof == false).FirstOrDefault();
                            // declaredComponent = new TXEmployeeSection(Guid.Empty, emp.Id, other.Id, taxInfo.EffectiveDate, false);
                        }
                        if (other.DisplayAs == "Income from house property")
                        {
                            HousePropertyList HPList = new HousePropertyList(emp.Id, other.FinancialYearId, other.Id, Convert.ToString(taxInfo.EffectiveDate.Month), Convert.ToString(taxInfo.EffectiveDate.Year));
                            double HousepropertyNet = 0;
                            if (HPList.Count > 0)
                            {
                                HPList.ForEach(hp =>
                                {
                                    if (hp.PropertySelfOccupied == "1" && newtaxscheme == "1")
                                    {
                                        hp.LessInterestOnHousingLoan = 0;
                                    }
                                    HousepropertyNet = HousepropertyNet + Convert.ToDouble(hp.Balance) - Convert.ToDouble(hp.LessStandardDeduction) - Convert.ToDouble(hp.LessInterestOnHousingLoan);
                                });
                                //   }

                                if (!object.ReferenceEquals(null, declaredComponent))
                                {
                                    declaredComponent.DeclaredValue = Convert.ToString(HousepropertyNet);
                                }

                                //if (newtaxscheme == "1")
                                //{
                                //    if (!object.ReferenceEquals(null, declaredComponent))
                                //    {
                                //        if (Convert.ToDecimal(declaredComponent.DeclaredValue) < 0)
                                //        {
                                //            declaredComponent.DeclaredValue = "0";
                                //        }
                                //    }
                                //}
                                //16-04-2023 Mr. Murali confirmed
                                //that if calculated amount is in negative and tax scheme is "New" then amount to be restricted as "Negative -200000"
                                //Let out property
                                if (newtaxscheme == "1")
                                {
                                    if (!object.ReferenceEquals(null, declaredComponent))
                                    {
                                        if (Convert.ToDecimal(declaredComponent.DeclaredValue) < 0)
                                        {
                                            if (Math.Abs(Convert.ToDecimal(declaredComponent.DeclaredValue)) > 200000 && HPList.Find(x => x.PropertySelfOccupied == "2") != null) //PropertySelfOccupied==2 means letout
                                                declaredComponent.DeclaredValue = "-200000";
                                            else declaredComponent.DeclaredValue = Convert.ToString(declaredComponent.DeclaredValue);
                                        }
                                    }
                                }

                            }
                            // housing property condition removed from here
                            otherval = 0;
                            if (!object.ReferenceEquals(null, declaredComponent))
                            {
                                otherval = declaredComponent.DeclaredValue == "" ? 0 : Convert.ToDecimal(declaredComponent.DeclaredValue);
                            }

                            if (otherval < 0)
                            {
                                otherval = otherval * (-1);
                                if (otherval > 200000)
                                {
                                    otherval = 200000;
                                    otherval = -(otherval);
                                    other.Limit = otherval;
                                }
                                else
                                {
                                    otherval = -(otherval);
                                    other.Limit = otherval;
                                }
                            }
                            else
                            {
                                otheVal = "0";
                                if (!object.ReferenceEquals(null, declaredComponent))
                                {
                                    otheVal = !string.IsNullOrEmpty(declaredComponent.DeclaredValue) ? declaredComponent.DeclaredValue : "0";
                                }
                                otherval = Convert.ToDecimal(otheVal);
                                if (otherval > other.Limit && other.Limit != 0)
                                {
                                    otherval = other.Limit;
                                }
                            }
                        }
                        else
                        {
                            otheVal = "0";
                            if (!object.ReferenceEquals(null, declaredComponent))
                            {
                                otheVal = !string.IsNullOrEmpty(declaredComponent.DeclaredValue) ? declaredComponent.DeclaredValue : "0";
                            }
                            otherval = Convert.ToDecimal(otheVal);
                            if (otherval > other.Limit && other.Limit != 0)
                            {
                                otherval = other.Limit;
                            }
                        }
                        taxInfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxInfo.FinanceYearId,
                            ApplyDate = taxInfo.EffectiveDate,
                            FieldId = other.Id,
                            Field = other.DisplayAs,
                            Actual = other.IncomeTypeId == 4 ? otherval : (-1 * otherval),
                            FieldType = "OtherIncomes"
                        });
                    });
                    var pre = taxInfo.Result.Where(r => r.FieldType == "Section" && r.EmployeeId == emp.Id).Sum(r => r.Total);
                    var dedo = taxInfo.Result.Where(y => y.FieldType == "OtherIncomes" && y.EmployeeId == emp.Id).Sum(y => y.Actual);
                    dedo = taxInfo.Result.Where(y => y.FieldType == "OtherIncomes" && y.EmployeeId == emp.Id).Count() > 0 ? dedo : 0;
                    AttributeModel attr = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "GROSSSALARY");


                    decimal totIn = (decimal)taxInfo.Result.Where(w => w.Field.Trim() == "Section 16" && w.FieldType == "grosssec" && w.EmployeeId == emp.Id).FirstOrDefault().Total + (decimal)(dedo.Value);




                    if (!ReferenceEquals(pre, null))
                    {
                        taxInfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxInfo.FinanceYearId,
                            ApplyDate = taxInfo.EffectiveDate,
                            FieldId = Guid.NewGuid(),
                            Field = "otherincometotal",
                            Total = Eval.LimitNegativeValues(totIn),

                            FieldType = "grosssec"
                        });
                    }
                });
                List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
                List<FormulaRecursive> lstFormulaRecuriveGetActualfrmPayroll = new List<FormulaRecursive>();
                TXSectionList sections = new TXSectionList();

                lstFormulaRecursive.AddRange(taxInfo.Result.Where(r => r.EmployeeId == emp.Id).Select(re => new FormulaRecursive
                {
                    Assignedvalues = Convert.ToString(Math.Round(Convert.ToDecimal(re.Total))),//+ Convert.ToDecimal(0.01)),
                    Name = re.Field == null ? string.Empty : re.Field,
                    Id = re.FieldId.ToString(),
                    ExecuteOrder = 4,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    DoRoundOff = false
                }));



                lstFormulaRecursive.ForEach(f =>
                {
                    // var hjg = emp.PayrollofMonth.Where(w => w.AttributeModelId == new Guid(f.Id));
                    var hjg = taxInfo.entity.EntityAttributeModelList.Where(w => w.AttributeModelId == new Guid(f.Id));
                    if (hjg != null)
                    {
                        // emp.PayrollofMonth.RemoveAll(r => r.AttributeModelId == new Guid(f.Id));
                        taxInfo.entity.EntityAttributeModelList.RemoveAll(r => r.AttributeModelId == new Guid(f.Id));
                    }

                });

                // lstFormulaRecursive.AddRange(emp.PayrollofMonth.Select(payhistory => new FormulaRecursive
                lstFormulaRecursive.AddRange(taxInfo.entity.EntityAttributeModelList.Select(payhistory => new FormulaRecursive
                {

                    Assignedvalues = Convert.ToString(Math.Round(!String.IsNullOrEmpty(payhistory.EntityAttributeValue.Value) ? Convert.ToDecimal(payhistory.EntityAttributeValue.Value) : 0)), //+ Convert.ToDecimal(0.01) : 0)),
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

                string[] a = { "Section 10 Exemptions", "Section 16", "Section 80CCE: 80C, 80CCC,80-CCD(1)", "Under Section 80CCD", "Medical insurance premium (Mediclaim)  (80D)", "Other Deductible amounts under Chapter VI A" };

                for (int i = 0; i < a.Length; i++)
                {

                    txInfoSections.Where(w => w.Name.Trim() == a[i]).FirstOrDefault().ExecOrder = i;
                }
                int j = 0;
                txInfoSections.OrderBy(o => o.ExecOrder).ToList();



                txInfoSections.OrderBy(o => o.ExecOrder).ToList().ForEach(s =>
                {
                    j++;
                    decimal totalsub = 0;

                    TXSectionList empsections = new TXSectionList();
                    empsections.AddRange(txInfoSubSections.Where(sec => sec.ParentId == s.Id).ToList());
                    if (empsections != null && empsections.Count > 0)
                    {
                        // empsections.Add(s);
                    }
                    else
                    {
                        empsections.Add(s);
                    }


                    empsections.ForEach(sub =>
                    {
                        int type = sub.FormulaType;

                        if (sub.FormulaType == 2)//Declared Component
                        {
                            DateTime edate = taxInfo.EffectiveDate;
                            sub.Value = (DeclaredComponent(imatchList, newtaxscheme, sub, emp, taxInfo.Proofwise, edate, ref taxInfo, taxInfo.TxEmployeeSectionList)).ToString();
                        }
                        else if (type == 3 && sub.ExemptionType == 1)
                        {
                            int financeStartMonth = taxInfo.FinanceYear.StartingDate.Month;
                            int financeStartYear = taxInfo.FinanceYear.StartingDate.Year;
                            PayrollHistoryList payHistory = new PayrollHistoryList(taxInfo.CompanyId, financeStartYear, financeStartMonth, taxInfo.ApplyYear, taxInfo.ApplyMonth, emp.Id);
                            sub.Value = GetActualpayrollVal(payHistory, sub.Formula, emp.Id);
                            if (newtaxscheme == "1")
                            {
                                if (sub.Eligible == false)
                                {
                                    sub.Value = "0";
                                }
                            }
                            if (sub.Limit > 0)
                                sub.Value = Convert.ToString((System.Math.Min(sub.Limit, Convert.ToDecimal(sub.Value))));

                        }
                        else if (sub.FormulaType == 7)// Get Projection
                        {

                            decimal acutualval = 0;
                            decimal total = 0;
                            //Need to calculate PTAX
                            if (sub.Value.ToUpper() == "PTAX")
                            {
                                PTax ptax = new PTax();
                                var egid = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "EG");
                                var fgid = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "FG");
                                string egValue = Convert.ToString(taxInfo.entity.EntityAttributeModelList.FirstOrDefault(p => p.AttributeModelId == egid.Id).EntityAttributeValue.Value);
                                string fgValue =  Convert.ToString(taxInfo.entity.EntityAttributeModelList.FirstOrDefault(p => p.AttributeModelId == fgid.Id).EntityAttributeValue.Value);
                                PTax pTax = new PTax(emp.PTLocation, taxInfo.CompanyId);

                                decimal ProjectedPTax;
                                ptax.GetPTaxCalculation(taxInfo, emp, taxInfo.CompanyId, Convert.ToDouble(egValue), Convert.ToDouble(fgValue), taxInfo.ApplyYear, taxInfo.ApplyMonth, 1, out ProjectedPTax, taxInfo.FandFFlag, taxInfo.ProjectionMonth);
                                acutualval = ProjectedPTax;
                                if (pTax.Calculationtype == "Monthly" || pTax.Calculationtype == "SixMont")
                                {
                                    DateTime applydate = new DateTime(taxInfo.ApplyYear, taxInfo.ApplyMonth, 1);
                                    // Last working date based Ptax projection calculation if last working month same as applydate month set as zero

                                    // (DateTime.Parse("1/" + taxInfo.ApplyMonth.ToString() + "/" + taxInfo.ApplyYear));
                                    //  if ((applydate.AddMonths(6) > taxInfo.FinanceYear.EndingDate))
                                    // {
                                    //egValue==fgValue for get projection only 

                                    if (applydate.Month == emp.LastWorkingDate.Month && applydate.Year == emp.LastWorkingDate.Year)
                                    {
                                        ptax.GetPTaxProjectionCalculation(taxInfo, emp, taxInfo.CompanyId, Convert.ToDouble(egValue), Convert.ToDouble(fgValue), applydate.Year, applydate.Month, 1, out ProjectedPTax, taxInfo.FandFFlag, taxInfo.ProjectionMonth);
                                        acutualval = acutualval + ProjectedPTax;
                                        //acutualval = acutualval;
                                    }
                                    else
                                    {
                                        ptax.GetPTaxProjectionCalculation(taxInfo, emp, taxInfo.CompanyId, Convert.ToDouble(egValue), Convert.ToDouble(fgValue), applydate.Year, applydate.Month, 1, out ProjectedPTax, taxInfo.FandFFlag, taxInfo.ProjectionMonth);
                                        acutualval = acutualval + ProjectedPTax;
                                    }

                                    //  }
                                    //  else
                                    //   {
                                    //egValue==fgValue for get projection only 
                                    //      ptax.GetPTaxProjectionCalculation(emp.Id, taxInfo.CompanyId, Convert.ToDouble(fgValue), Convert.ToDouble(fgValue), applydate.Year, applydate.Month, 1, out ProjectedPTax, taxInfo.ProjectionMonth);
                                    //      acutualval = acutualval + ProjectedPTax;
                                    //   }

                                }
                                if (newtaxscheme == "1")
                                {
                                    if (sub.Eligible == false)
                                    {
                                        acutualval = 0;
                                    }
                                }
                                total = acutualval;
                            }

                            else if (sub.Value.ToUpper() == "PF")
                            {
                                PF pfCal = new PF();
                                var pfattr = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PF");
                                PayrollHistoryList plist = new PayrollHistoryList();
                                //if (taxInfo.emproll.ToUpper() != "ADMIN")
                                //{
                                //    plist.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year*100) + ph.Month)  >= ((taxInfo.FinanceYear.StartingDate.Year*100) + taxInfo.FinanceYear.StartingDate.Month) && ((ph.Year * 100) + ph.Month) < (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                                //}
                                //else
                                //{
                                plist.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.CompanyId == taxInfo.CompanyId && ((ph.Year * 100) + ph.Month) >= ((taxInfo.FinanceYear.StartingDate.Year * 100) + taxInfo.FinanceYear.StartingDate.Month) && ((ph.Year * 100) + ph.Month) <= (taxInfo.ApplyYear * 100 + taxInfo.ApplyMonth) && ph.EmployeeId == emp.Id).ToList());
                                //}
                                decimal actualPf = 0;
                                for (int k = 0; k < plist.Count; k++)
                                {
                                    if (!object.ReferenceEquals(plist[k].PayrollHistoryValueList.Where(p => p.AttributeModelId == pfattr.Id).FirstOrDefault(), null))
                                    {
                                        actualPf = actualPf + Convert.ToDecimal(plist[k].PayrollHistoryValueList.Where(p => p.AttributeModelId == pfattr.Id).FirstOrDefault().Value);
                                    }
                                }
                                // decimal autualPf = plist.ForEach(p => p.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == pfattr.Id).Sum(ph=>Convert.ToDecimal(ph.Value)));
                                // => p.AttributeModelId == pfattr.Id).Sum(p => Convert.ToDecimal(p.Value));
                                decimal fPF = Convert.ToDecimal(pfCal.GetProjection(pfattr, emp, taxInfo));
                                if (taxInfo.Balprojmonth != 0 || taxInfo.Balprojdays != 0)
                                {
                                    total = (Convert.ToDecimal(fPF) * taxInfo.ProjectionMonth) + (Convert.ToDecimal(fPF) * taxInfo.Balprojmonth) + (Convert.ToDecimal(fPF) * taxInfo.Balprojdays / taxInfo.Balmaxdays);
                                }
                                else
                                {
                                    total = (Convert.ToDecimal(fPF) * taxInfo.ProjectionMonth) + actualPf;
                                }
                                if (taxInfo.VPFReq)// Company is allowed for VPF then added a PF + VPF
                                {
                                    var vpfattr = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "VPF");
                                    decimal autualVPF = vpfattr != null ? emp.PayrollHistoryList.Where(p => p.AttributeModelId == vpfattr.Id).Sum(p => Convert.ToDecimal(p.Value)) : 0;
                                    var masterVPFvalue = taxInfo.entity.EntityAttributeModelList.Where(p => p.AttributeModelId == vpfattr.Id).FirstOrDefault();
                                    decimal EmpVPFAmount = masterVPFvalue != null ? Convert.ToDecimal(masterVPFvalue.EntityAttributeValue.Value) : 0;
                                    if (taxInfo.VPFProjection && (taxInfo.Balprojmonth != 0 || taxInfo.Balprojdays != 0))
                                    {
                                        Decimal vpf = Convert.ToDecimal(EmpVPFAmount);
                                        total = total + (vpf * taxInfo.ProjectionMonth) + (vpf * taxInfo.Balprojmonth) + (vpf * taxInfo.Balprojdays / taxInfo.Balmaxdays) + autualVPF;
                                    }
                                    else
                                    {
                                        if (taxInfo.VPFProjection)
                                            total = total + (Convert.ToDecimal(EmpVPFAmount) * taxInfo.ProjectionMonth) + autualVPF;
                                        else
                                            total = total + autualVPF;
                                    }
                                }
                                if (newtaxscheme == "1")
                                {
                                    if (sub.Eligible == false)
                                    {
                                        total = 0;
                                    }

                                }
                            }
                            else
                            {
                                acutualval = emp.PayrollHistoryList.Where(p => p.AttributeModelId == new Guid(sub.Formula)).Distinct().Sum(p => Convert.ToDecimal(p.Value));
                                var temp = new PayrollHistory(sub.CompanyId, emp.Id, taxInfo.ApplyYear, taxInfo.ApplyMonth).PayrollHistoryValueList;
                                decimal projectedVal = temp.Count != 0 ? Convert.ToDecimal(temp.Where(p => p.AttributeModelId == new Guid(sub.MatchingComponent)).FirstOrDefault().Value) * taxInfo.ProjectionMonth : 0;
                                total = acutualval + projectedVal;
                                if (newtaxscheme == "1")
                                {
                                    if (sub.Eligible == false)
                                    {
                                        total = 0;
                                    }
                                }
                            }

                            lstFormulaRecursive.Add(new FormulaRecursive()
                            {
                                Assignedvalues = Convert.ToString(total),// sub.Value,
                                Id = sub.Id.ToString(),
                                Name = sub.Name == null ? string.Empty : sub.Name,
                                ExecuteOrder = 4,
                                Rounding = 0,
                                type = sub.FormulaType,
                                Percentage = "100",
                                EligibleFormula = string.Empty,
                                DoRoundOff = false,
                                ExemptionType = sub.ExemptionType
                            });
                        }
                        else if (type != 6 && !string.IsNullOrEmpty(sub.Value))
                        {
                            if (newtaxscheme == "1")
                            {
                                if (sub.Eligible == false)
                                {
                                    sub.Formula = "";
                                }
                            }
                            lstFormulaRecursive.Add(new FormulaRecursive()
                            {
                                Assignedvalues = sub.Formula,
                                Id = sub.Id.ToString(),
                                Name = sub.Name == null ? string.Empty : sub.Name,
                                ExecuteOrder = 4,
                                Rounding = 0,
                                type = type,
                                Percentage = "100",
                                EligibleFormula = string.Empty,
                                DoRoundOff = false,
                            });
                        }

                        else if (type == 6 || type == 3)
                        {
                            if (newtaxscheme == "1")
                            {
                                if (sub.Eligible == false)
                                {
                                    sub.Formula = "";
                                }
                            }
                            lstFormulaRecursive.Add(new FormulaRecursive()
                            {
                                Assignedvalues = sub.Formula,
                                Id = sub.Id.ToString(),
                                Name = sub.Name == null ? string.Empty : sub.Name,
                                ExecuteOrder = 5,
                                Rounding = type == 6 ? 99 : 9,
                                type = 3,
                                Percentage = "100",
                                EligibleFormula = string.Empty,
                                DoRoundOff = type == 6 ? false : true,
                                ExemptionType = sub.ExemptionType,
                                ActualFormula = sub.Formula,
                            });
                            //DateTime edate = taxInfo.EffectiveDate;
                            //sub.Value=(ExamptionComponent(sub, emp, taxInfo.Proofwise, edate, ref taxInfo)).ToString();
                        }
                    });//end subsection
                    lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();

                    iTax.recursiveSubsection(taxInfo.AttributemodelList, lstFormulaRecursive, new Entity { }, taxInfo, emp.Id, taxInfo.TxEmployeeSectionList);//no need to pass entity
                    lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                    var tempList = lstFormulaRecursive.Where(T => T.ExemptionType == 2).ToList();
                    //lstFormulaRecursive.ForEach(u =>
                    //{
                    //    if (u.ExemptionType == 2)// 1-yearly,2- Monthly 2-> getting a actual value of that component of that month
                    //    {
                    //        u.Assignedvalues = ExemptionTypeMonthlyValue(emp.PayrollMonth, u);
                    //        u.BaseValue = u.Assignedvalues;
                    //        u.Output = u.Assignedvalues;
                    //    }
                    //});
                    lstFormulaRecursive.ForEach(u =>
                    {
                        string error = string.Empty;
                        string output = string.Empty;
                        string chk = u.Assignedvalues;
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
                            taxInfo.Errors.Add("There is a some problem in formula.Please check it.");
                            // entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                        }
                        else
                        {
                            TXSection taxbeh = empsections.Where(x => x.Id == new Guid(u.Id)).FirstOrDefault();
                            string result = string.Empty;
                            if (u.type == 4)
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

                                output = FormulaRecursive.EvalRange(taxbeh.Formula, Convert.ToDecimal(u.Output), u, lstFormulaRecursive);
                            }


                            Eval eval = new Eval();
                            result = eval.Execute(output).ToString();
                            u.Output = result;
                            u.Assignedvalues = result.ToString();
                            if (!object.ReferenceEquals(taxbeh, null))
                            {
                                empsections.Where(x => x.Id == new Guid(u.Id)).FirstOrDefault().Value = result.ToString();
                                if (taxbeh.FormulaType == 6 && taxbeh.Formula != string.Empty)
                                {
                                    empsections.Where(x => x.Id == new Guid(u.Id)).FirstOrDefault().Limit = Convert.ToDecimal(result);
                                }
                            }
                            else
                            {
                                string str = u.Id;
                            }
                        }
                    });


                    //taxInfo.Result.Add(new TaxHistory
                    //{
                    //    EmployeeId = emp.Id,
                    //    FinanceYearId = taxInfo.FinanceYearId,
                    //    ApplyDate = new DateTime(taxInfo.ApplyYear, taxInfo.ApplyMonth, 1),
                    //    FieldId = s.Id,
                    //    Field = s.Name,
                    //    FieldType = "Section"
                    //});
                    //empsections.Where(e => e.FormulaType == 6).ToList().ForEach(e =>
                    //{



                    //});
                    empsections.ForEach(sub =>
                    {
                        if (sub.FormulaType == 6)
                        {
                            DateTime edate = taxInfo.EffectiveDate;

                            sub.Value = (ExamptionComponent(imatchList, newtaxscheme, sub, emp, taxInfo.Proofwise, edate, ref taxInfo)).ToString();
                        }

                        if (sub.Value == string.Empty)
                        {
                            sub.Value = "0";
                        }

                        decimal tl = (Convert.ToDecimal(sub.Value) >= sub.Limit && sub.FormulaType != 6 && sub.FormulaType != 2 ? (sub.Limit == 0 ? Convert.ToDecimal(sub.Value) : sub.Limit) : Convert.ToDecimal(sub.Value));
                        totalsub = totalsub + tl;
                        taxInfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxInfo.FinanceYearId,
                            ApplyDate = taxInfo.EffectiveDate,
                            FieldId = sub.Id,
                            Field = sub.Name,
                            Actual = (Convert.ToDecimal(sub.Value) >= sub.Limit && sub.FormulaType != 6 && sub.FormulaType != 2 ? (sub.Limit == 0 ? Convert.ToDecimal(sub.Value) : sub.Limit) : Convert.ToDecimal(sub.Value)),
                            Limit = Convert.ToDecimal(sub.Limit),
                            FieldType = "SubSection"
                        });
                    });
                    decimal tot = Convert.ToDecimal(totalsub >= s.Limit ? (s.Limit == 0 ? totalsub : s.Limit) : totalsub);
                    decimal? gross = taxInfo.Result.Where(w => w.Field == "Gross Salary" && w.EmployeeId == emp.Id).FirstOrDefault().Total;
                    decimal? previousSec = 0;
                    if (j == 2) { previousSec = taxInfo.Result.Where(w => w.Field == a[j - 2].Trim() && w.FieldType == "grosssec" && w.EmployeeId == emp.Id).FirstOrDefault().Total; }

                    taxInfo.Result.Add(new TaxHistory
                    {
                        EmployeeId = emp.Id,
                        FinanceYearId = taxInfo.FinanceYearId,
                        ApplyDate = taxInfo.EffectiveDate,
                        FieldId = s.Id,
                        Field = s.Name,
                        Actual = totalsub,
                        Projection = s.Name.Trim() == "Section 16" || s.Name.Trim() == "Section 10 Exemptions" ? tot : 0,
                        Total = j == 1 ? (tot > gross ? gross : tot) : j == 2 ? (tot > previousSec ? previousSec : tot) : tot,
                        Limit = Convert.ToDecimal(s.Limit),
                        FieldType = "Section"
                    });
                    var pre = taxInfo.Result.Where(r => r.FieldType == "Section" && r.EmployeeId == emp.Id).Sum(r => r.Total);
                    var dedo = taxInfo.Result.Where(y => y.FieldType == "OtherIncomes" && y.EmployeeId == emp.Id).Sum(y => y.Actual);
                    dedo = taxInfo.Result.Where(y => y.FieldType == "OtherIncomes" && y.EmployeeId == emp.Id).Count() > 0 ? dedo : 0;


                    AttributeModel attr = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "GROSSSALARY");
                    decimal totIn = 0;
                    if (s.Name.Trim() == "Section 10 Exemptions")
                    {
                        totIn = ((decimal)taxInfo.Result.Find(t => t.FieldId == attr.Id && t.EmployeeId == emp.Id).Total - (decimal)(pre.Value)) + (decimal)(dedo.Value);
                    }
                    else if (s.Name.Trim() == "Section 80CCE: 80C, 80CCC,80-CCD(1)")
                    {
                        decimal lessFrom = (decimal)taxInfo.Result.Where(w => w.Field == "otherincometotal" && w.FieldType == "grosssec" && w.EmployeeId == emp.Id).FirstOrDefault().Total;
                        decimal lessTo = (decimal)taxInfo.Result.Where(w => w.FieldId == s.Id && w.FieldType == "Section" && w.EmployeeId == emp.Id).FirstOrDefault().Total;
                        totIn = lessFrom - lessTo;
                    }
                    else
                    {
                        decimal lessFrom = (decimal)taxInfo.Result.Where(w => w.Field.Trim() == a[j - 2].Trim() && w.FieldType == "grosssec" && w.EmployeeId == emp.Id).FirstOrDefault().Total;
                        decimal lessTo = (decimal)taxInfo.Result.Where(w => w.FieldId == s.Id && w.FieldType == "Section" && w.EmployeeId == emp.Id).FirstOrDefault().Total;
                        totIn = lessFrom - lessTo;
                    }
                    if (!ReferenceEquals(pre, null))
                    {
                        taxInfo.Result.Add(new TaxHistory
                        {
                            EmployeeId = emp.Id,
                            FinanceYearId = taxInfo.FinanceYearId,
                            ApplyDate = taxInfo.EffectiveDate,
                            FieldId = Guid.NewGuid(),
                            Field = s.Name,
                            Total = Eval.LimitNegativeValues(totIn),
                            Limit = Convert.ToDecimal(s.Limit),
                            FieldType = "grosssec"
                        });
                    }
                    if (s.Name.Trim() == "Section 16")
                    {
                        otherincome();
                    }
                    empsections = new TXSectionList();
                });//end section
                   //Include other income heads to be deducted

                decimal totInc = 0;
                decimal? deduc = taxInfo.Result.Where(r => r.FieldType == "Section" && r.EmployeeId == emp.Id).Sum(r => (r.Total.Value));
                decimal? ded = taxInfo.Result.Where(y => y.FieldType == "OtherIncomes" && y.EmployeeId == emp.Id).Sum(y => (y.Actual.Value));

                if (!ReferenceEquals(deduc, null))
                {
                    AttributeModel attr = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "GROSSSALARY");
                    totInc = (decimal)taxInfo.Result.Last(r => r.FieldType == "grosssec" && r.EmployeeId == emp.Id).Total;
                    attr = taxInfo.AttributemodelList.Find(t => t.Name.ToUpper() == "TOTINCOME");
                    taxInfo.Result.Add(new TaxHistory
                    {
                        EmployeeId = emp.Id,
                        FinanceYearId = taxInfo.FinanceYearId,
                        ApplyDate = taxInfo.EffectiveDate,
                        FieldId = attr.Id,
                        Field = attr.DisplayAs + "(Round By 10 Rupess)",
                        Total = Eval.LimitNegativeValues(Convert.ToDecimal(Math.Round(Convert.ToDouble(totInc) / 10.0) * 10)),
                        FieldType = "Total Income"
                    });
                }
                txInfoSections = new TXSectionList();
                txInfoSubSections = new TXSectionList();
            });//end Employees


        }

        private decimal ExamptionComponent(IncomeMatchingList imatchList, string newtaxscheme, TXSection section, Employee employee, bool byProofWise, DateTime effectiveDate, ref TaxComputationInfo taxinfo)
        {
            //        IncomeMatchingList imatchList = new IncomeMatchingList(section.FinancialYearId);
            IncomeMatching imatch = imatchList.Where(i => i.ExemptionComponent == section.Id).FirstOrDefault();
            decimal taxTotal = 0;
            decimal actual = 0;
            decimal projection = 0;
            int projectionMonth = 0;
            decimal returnval = 0;

            TXSection curSecSetting = new TXSection(section.Id, taxinfo.CompanyId);
            int limitZero = -1;
            if (string.IsNullOrEmpty(curSecSetting.Formula) && curSecSetting.Limit == 0)
            {
                limitZero = 0;
            }

            if (!ReferenceEquals(imatch, null))
            {
                projectionMonth = imatch.Projection == true ? taxinfo.ProjectionMonth : 1;
                decimal limtedVal = section.Limit;// !string.IsNullOrEmpty(declaration.DeclaredValue) ? Convert.ToDecimal(declaration.DeclaredValue) : 0;

                employee.PayrollHistoryList.Where(p => p.AttributeModelId == imatch.AttributemodelId).ToList().ForEach(p =>
                {
                    PayrollHistoryValue pay = new PayrollHistoryValue();

                    //if (imatch.MatchingComponent == p.AttributeModelId)
                    //{
                    if (section.ExemptionType == 2)
                    {
                        if (Convert.ToDecimal(p.Value) > limtedVal && limtedVal > limitZero)
                        {
                            taxTotal = taxTotal + limtedVal;
                        }
                        else
                        {
                            taxTotal = taxTotal + Convert.ToDecimal(p.Value);
                        }
                    }
                    else
                    {
                        taxTotal = taxTotal + Convert.ToDecimal(p.Value);
                    }

                    if (newtaxscheme == "1")
                    {
                        if (section.Eligible == false)
                        {
                            taxTotal = 0;
                        }
                    }
                    //}


                });
                actual = taxTotal;
                if (taxTotal != 0)
                {
                    if (section.ExemptionType == 2) //Monthly Limit
                    {
                        if (imatch.Projection)
                        {
                            var fixdeVal = employee.PayrollofMonth.Where(p => p.AttributeModelId == imatch.MatchingComponent).FirstOrDefault();
                            if (fixdeVal != null && Convert.ToDecimal(fixdeVal.Value) > limtedVal && limtedVal > limitZero)
                            {
                                projection = (limtedVal * projectionMonth);
                            }
                            else
                            {
                                projection = (fixdeVal != null ? Convert.ToDecimal(fixdeVal.Value) * projectionMonth : 0);
                            }

                            taxTotal = actual + projection;
                        }
                        else
                        {
                            //var payValue = employee.PayrollofMonth.Where(p => p.AttributeModelId == imatch.AttributemodelId).FirstOrDefault();
                            //taxTotal = payValue != null ? Convert.ToDecimal(payValue.Value) : 0;
                            //actual = taxTotal;
                        }
                    }
                    else
                    if (section.ExemptionType == 1) //Yearly Limit
                    {
                        var emptotal = employee.PayrollofMonth.Where(p => p.AttributeModelId == imatch.MatchingComponent).FirstOrDefault();
                        if (limtedVal == 0)
                        {

                            taxTotal = (emptotal != null ? Convert.ToDecimal(emptotal.Value) * projectionMonth : 0) + taxTotal;
                        }
                        else if (limtedVal < (Convert.ToDecimal((emptotal != null ? Convert.ToDecimal(emptotal.Value) : 0) * projectionMonth) + taxTotal))
                        {
                            taxTotal = limtedVal;
                        }
                        else
                        {
                            taxTotal = (Convert.ToDecimal(emptotal != null ? Convert.ToDecimal(emptotal.Value) : 0) * projectionMonth) + taxTotal;
                        }

                    }

                }
                //     if (!ReferenceEquals(imatch, null))
                //     {
                //taxinfo.Result.Add(new TaxHistory
                //{
                //    FieldId = section.Id,
                //    FieldType = "SubSection",
                //    Actual = taxTotal,
                //    Total = taxTotal
                //});
                //   }
                // return taxTotal;
                // if (limtedVal == 0)
                //     returnval = taxTotal;
                //  else
                //     returnval = (System.Math.Min(limtedVal, taxTotal));
                returnval = taxTotal;
            }


            return Eval.LimitNegativeValues(returnval);
        }
        private decimal DeclaredComponent(IncomeMatchingList imatchList, string newtaxscheme, TXSection section, Employee employee, bool byProofWise, DateTime effectiveDate, ref TaxComputationInfo taxinfo, TXEmployeeSectionList TXEmpSecList)
        {
            //    IncomeMatchingList imatchList = new IncomeMatchingList(section.FinancialYearId);
            IncomeMatching imatch = imatchList.Where(i => i.ExemptionComponent == section.Id).FirstOrDefault();
            decimal taxTotal = 0;
            decimal earningTot = 0;
            decimal limtedVal = section.Limit;
            limtedVal = (section.ExemptionType == 2 ? (limtedVal * 12) : limtedVal);
            if (!ReferenceEquals(imatch, null))
            {
                employee.PayrollHistoryList.Where(p => p.AttributeModelId == imatch.AttributemodelId).ToList().ForEach(p =>
                {
                    earningTot = earningTot + Convert.ToDecimal(p.Value);

                });
            }

            TXEmployeeSection declaration;

            if (byProofWise)
            {
                declaration = TXEmpSecList.Where(ts => ts.EmployeeId == employee.Id && ts.SectionId == section.Id && ts.Proof == true).FirstOrDefault();
                // declaration = new TXEmployeeSection(Guid.Empty, employee.Id, section.Id, effectiveDate, true);
            }
            else
            {
                declaration = TXEmpSecList.Where(ts => ts.EmployeeId == employee.Id && ts.SectionId == section.Id && ts.Proof == false).FirstOrDefault();
                // declaration = new TXEmployeeSection(Guid.Empty, employee.Id, section.Id, effectiveDate, false);
            }
            if (!object.ReferenceEquals(null, declaration))
            {
                if (newtaxscheme == "1")
                {
                    if (section.Eligible == false)
                    {
                        declaration.DeclaredValue = "0";
                    }
                }
            }
            Decimal declaredVal = 0;
            if (!object.ReferenceEquals(null, declaration))
            {
                declaredVal = !string.IsNullOrEmpty(declaration.DeclaredValue) ? Convert.ToDecimal(declaration.DeclaredValue) : 0;
            }
            if (declaredVal == 0) return declaredVal;
            if (limtedVal > declaredVal || limtedVal == 0)
            {
                taxTotal = Math.Round(declaredVal + Convert.ToDecimal(0.01));
                limtedVal = limtedVal == 0 ? taxTotal : limtedVal;
            }
            else
            {
                taxTotal = Math.Round(declaredVal + Convert.ToDecimal(0.01));
            }

            decimal projection = 0;
            if (taxTotal != 0)
            {
                if (section.ExemptionType == 2) //Monthly Limit
                {

                    limtedVal = (limtedVal * 12);
                    // taxTotal = actual + projection;
                    // limtedVal = projection;
                    if (!ReferenceEquals(imatch, null))
                    {
                        var emptotal = employee.PayrollHistoryList.Where(p => p.AttributeModelId == imatch.AttributemodelId).FirstOrDefault();
                        projection = imatch.Projection == true ? ((Convert.ToDecimal(emptotal.Value) * taxinfo.ProjectionMonth) + earningTot) : earningTot;
                    }
                    earningTot = projection;
                }
                else
                if (section.ExemptionType == 1) //Yearly Limit
                {
                    if (!ReferenceEquals(imatch, null))
                    {
                        var emptotal = employee.PayrollHistoryList.Where(p => p.AttributeModelId == imatch.AttributemodelId).FirstOrDefault();
                        projection = imatch.Projection == true ? ((Convert.ToDecimal(emptotal.Value) * taxinfo.ProjectionMonth) + earningTot) : earningTot;
                    }

                    earningTot = projection;
                }
            }
            decimal returnval = 0;
            if (earningTot == 0)
                returnval = (System.Math.Min(limtedVal, taxTotal));
            else
                returnval = (System.Math.Min(System.Math.Min(earningTot, taxTotal), limtedVal));

            return Eval.LimitNegativeValues(returnval);

        }

        public String ExemptionTypeMonthlyValue(List<PayrollHistoryValue> lstCollection, FormulaRecursive inp)
        {
            string strOut = "0";
            IncrementList increment = new IncrementList(inp.EmployeeId);
            var newinc = increment.Where(inc => inc.ApplyMonth == inp.Month && inc.ApplyYear == inp.Year).FirstOrDefault();
            string temp = inp.ActualFormula;
            string eligibleTemp = inp.EligibleFormula;
            if ((!string.IsNullOrEmpty(temp) && temp.IndexOf('{') >= 0) || (!string.IsNullOrEmpty(eligibleTemp) && eligibleTemp.IndexOf('{') >= 0))
            {
                if (!string.IsNullOrEmpty(temp) && temp.IndexOf('{') >= 0)
                {
                    do
                    {
                        try
                        {
                            string id = temp.Substring(temp.IndexOf('{') + 1, 36);
                            var colTemp = lstCollection.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                            string replacevalue = "{" + id + "}";
                            if (!object.ReferenceEquals(colTemp, null))
                            {
                                if (colTemp.AttributeModelId.ToString().IndexOf('{') >= 0)
                                {
                                    temp = temp.Replace(replacevalue, colTemp.BaseValue.Replace('{', '$').Replace('}', '#'));
                                }
                                else
                                {
                                    temp = temp.Replace(replacevalue, colTemp.BaseValue);
                                }
                            }
                            else
                            {
                                temp = temp.Replace(replacevalue, "0");//if the attribute is not in the Entity attribute collection
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    while (temp.IndexOf('{') >= 0);

                }

                FormulaRecursive maxminvalue = new FormulaRecursive();
                temp = maxminvalue.GetMax(temp);
                temp = maxminvalue.GetMin(temp);
                Eval eval = new Eval();
                string result = eval.Execute(temp).ToString();
                double baseValue = 0;

                double dresult;

                if (double.TryParse(result, out dresult))
                {
                    if (inp.type != 1)
                    {
                        baseValue = dresult;
                    }
                    else
                    {
                        //baseValue = Convert.ToDouble(lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue);
                    }

                }
                strOut = baseValue.ToString();
            }
            return strOut;

        }


        public string GetActualpayrollVal(PayrollHistoryList payrollVal, string formula, Guid empId)
        {
            string returnVal = "0";
            FormulaRecursive formulacls = new FormulaRecursive();
            string temp = formula.Replace("null", "");
            string eligibleTemp = formula;
            if ((!string.IsNullOrEmpty(temp) && temp.IndexOf("{") >= 0) || (!string.IsNullOrEmpty(eligibleTemp) && eligibleTemp.IndexOf('{') >= 0))
            {
                if (!string.IsNullOrEmpty(temp) && temp.IndexOf('{') >= 0)
                {

                    do
                    {
                        try
                        {
                            string id = temp.Substring(temp.IndexOf("{") + 1, 36);
                            decimal tempval = 0;
                            payrollVal.Where(p => p.EmployeeId == empId).OrderBy(o => o.Month).ToList().ForEach(p =>
                            {
                                var temppay = p.PayrollHistoryValueList.Where(i => i.AttributeModelId == new Guid(id)).FirstOrDefault();
                                if (temppay != null)
                                    tempval = tempval + Convert.ToDecimal(temppay.Value);
                            });
                            //   var colTemp = payrollVal.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                            string replacevalue = "{" + id + "}";


                            temp = temp.Replace(replacevalue, Convert.ToString(tempval));
                            //   temp = temp.Replace(replacevalue, colTemp.Assignedvalues);



                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    while (temp.IndexOf('{') >= 0);

                }
            }
            temp = formulacls.GetMax(temp);
            temp = formulacls.GetMin(temp);
            Eval eval = new Eval();
            returnVal = eval.Execute(temp).ToString();

            return returnVal;
        }
    }
}
