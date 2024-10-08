using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace PayrollBO.TaxActivities
{

    public sealed class NetTaxActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxInfo = context.GetValue(this.TaxInfo);
            AttributeModel egattr = taxInfo.AttributemodelList.Where(a => a.Name == "EG").FirstOrDefault();
            
            taxInfo.Employees.ForEach(emp =>
            {
                TaxHistory tax = taxInfo.Result.Where(tx => tx.Field == "TOTTAX" && tx.EmployeeId==emp.Id ).FirstOrDefault();
                decimal projectedTax = tax==null?0: tax.Total.Value;
                TaxHistoryList prevTx = new TaxHistoryList(taxInfo.FinanceYearId,emp.Id);
                decimal actualTax = prevTx.Where(t => t.Field == "TOTTAXPERMONTH").Sum(t => Convert.ToDecimal(t.Total));


                decimal tobePayTax = (projectedTax - paidTax(taxInfo,emp));
                var onetimetaxObj = taxInfo.Result.Where(x => x.Field == "ONETIMETAX" && x.EmployeeId==emp.Id).FirstOrDefault();
                decimal OneTimeTax = onetimetaxObj != null ? onetimetaxObj.Total.Value : 0;
                OneTimeTax = Math.Min(tobePayTax, OneTimeTax);
                decimal payTaxPer_Month = 0;
                decimal cal_proj_month = 0;

                /* this condition is added since we have not added projection of salary in payroll activity but
                 * we have calculate income tax for the current processing month. but in case of employee we have added
                 * the projection in payroll activity itself*/

                cal_proj_month = taxInfo.ProjectionMonth;

                if (taxInfo.processtype != "employee")
                {
                    cal_proj_month = taxInfo.ProjectionMonth + 1;
                }
                //}

                payTaxPer_Month = taxInfo.ProjectionMonth == 0 ? (tobePayTax - OneTimeTax) : (tobePayTax - OneTimeTax) / (cal_proj_month);

               // taxInfo.Result.Where(tx => tx.Field == "TOTTAXPERMONTH" && tx.EmployeeId == emp.Id).FirstOrDefault().Total = egVal == 0 ? 0 : Eval.LimitNegativeValues(payTaxPer_Month + OneTimeTax);

                taxInfo.Result.Where(tx => tx.Field == "TOTTAXPERMONTH" && tx.EmployeeId == emp.Id).FirstOrDefault().Total = Eval.LimitNegativeValues(payTaxPer_Month + OneTimeTax);
                taxInfo.Result.Add(new TaxHistory
                {
                    EmployeeId = emp.Id,
                    FinanceYearId = taxInfo.FinanceYearId,
                    ApplyDate = taxInfo.EffectiveDate,
                    FieldId = Guid.Empty,
                    Field = "TDS to be deducted for "+(cal_proj_month)+" Months, Per Month Value ",
                    Total = Eval.LimitNegativeValues(Math.Round(payTaxPer_Month+Convert.ToDecimal(0.01))),
                    FieldType = "TAX"
                });
            });
        }

        public decimal paidTax(TaxComputationInfo taxInfo,Employee emp)
        {
            EntityModel entModel = new EntityModel(ComValue.SalaryTable, taxInfo.CompanyId);
            EntityMapping entMapping = new EntityMapping("Employee", emp.Id.ToString(), entModel.Id);
            int[,] ym = new int[12, 2];
            DateTime i, j;
            int rowIndex, columnIndex;
            rowIndex = 0;
            int syear = taxInfo.FinanceYear.StartingDate.Year;
            int smonth = taxInfo.FinanceYear.StartingDate.Month;
            int eyear = taxInfo.EffectiveDate.Year;
            int emonth = taxInfo.EffectiveDate.Month;

            i = taxInfo.FinanceYear.StartingDate;
            j = taxInfo.EffectiveDate;
            while (i < j)
            {
                columnIndex = 0;
                ym[rowIndex, columnIndex] = i.Month;
                columnIndex++;
                ym[rowIndex, columnIndex] = i.Year;
                rowIndex++;
                i = i.AddMonths(1);
            }
            decimal txpm = 0;
            AttributeModel payatrr = taxInfo.AttributemodelList.Where(s => s.Name == "TDS").FirstOrDefault();
            PayrollHistoryList phlist = new PayrollHistoryList(taxInfo.CompanyId, syear, smonth, eyear, emonth, emp.Id);
            for (int f = 0; f < 12; f++)
            {
                if (ym[f, 1] != 0 && ym[f, 0] != 0)
                {
                    //PayrollHistory payrollhistory = new PayrollHistory(taxInfo.CompanyId, emp.Id, ym[f, 1], ym[f, 0]);
                    //if (!ReferenceEquals(payrollhistory, null))
                    //{
                    //    PayrollHistoryValue paytax = payrollhistory.PayrollHistoryValueList.Where(p => p.AttributeModelId == payatrr.Id).FirstOrDefault();
                    //    if (!ReferenceEquals(paytax, null))
                    //    {
                    //        txpm = txpm + Convert.ToDecimal(paytax.Value);
                    //    }
                    //}
                    PayrollHistory payhist = phlist.Where(ph => ph.Month == ym[f, 0] && ph.Year == ym[f,1]).FirstOrDefault();
                    if (!ReferenceEquals(payhist,null))
                    {
                        Guid entid = payhist.EntityId;
                        MonthlyInputList TDSMonthlyVal = new MonthlyInputList(entid, emp.Id, ym[f, 0], ym[f, 1]);
                        if (!ReferenceEquals(TDSMonthlyVal, null))
                        {
                            var tempMonthval = TDSMonthlyVal.Where(x => x.AttributeModelId == payatrr.Id).FirstOrDefault();
                            if (!ReferenceEquals(tempMonthval, null))
                            {
                                txpm = txpm + Convert.ToDecimal(tempMonthval.Value);
                            }
                        }
                    }
                }
            }
            // add previous tds value of the employee

            TXFinanceYear txf = taxInfo.FinanceYear;
            DateTime joiningdate = new Employee(emp.Id).DateOfJoining;
           // AttributeModelList attrList = new AttributeModelList(taxInfo.CompanyId);
            if (joiningdate > txf.StartingDate && joiningdate <= txf.EndingDate)
            {
                if (!object.ReferenceEquals(taxInfo.AttributemodelList.Where(w => w.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault(), null))
                {
                    Guid pre = taxInfo.AttributemodelList.Where(h => h.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault().Id;
                    TXEmployeeSection txDelaration = new TXEmployeeSection(Guid.Empty, emp.Id, pre, taxInfo.EffectiveDate, taxInfo.Proofwise);
                    Decimal val = !string.IsNullOrEmpty(txDelaration.DeclaredValue) ? Convert.ToDecimal(txDelaration.DeclaredValue) : 0;

                    if (!object.ReferenceEquals(txDelaration, null))
                    {
                        txpm = txpm + val;
                    }
                }

            }
            return txpm;
        }
    }
}
