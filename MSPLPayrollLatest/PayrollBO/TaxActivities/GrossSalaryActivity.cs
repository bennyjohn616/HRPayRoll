using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace PayrollBO.TaxActivities
{

    public sealed class GrossSalaryActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<TaxComputationInfo> TaxInfo { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            TaxComputationInfo taxInfo = context.GetValue(this.TaxInfo);
            taxInfo.Employees.ForEach(emp =>
            {


                taxInfo.Result.Add(new TaxHistory
                {
                    EmployeeId = emp.Id,
                    FinanceYearId = taxInfo.FinanceYearId,
                    FieldId = Guid.NewGuid(),
                    Field = "Total Earning",
                    Actual = taxInfo.Result.Where(r => r.EmployeeId == emp.Id && r.FieldType == "Income").Sum(r => r.Actual),
                    Projection = taxInfo.Result.Where(r => r.EmployeeId == emp.Id && r.FieldType == "Income").Sum(r => r.Projection),
                    Total =  taxInfo.Result.Where(r => r.EmployeeId == emp.Id && r.FieldType == "Income").Sum(r => r.Total) ,
                    FieldType = "Income",

                });
                taxInfo.Result.ForEach(r =>
                {
                    if (r.FieldType == "OtherIncome" && r.EmployeeId==emp.Id)
                    {
                        r.FieldType = "Income";
                    }
                    else if (!r.Field.Contains("Total Earning") && r.EmployeeId == emp.Id)
                    {
                        r.Total = r.Projection + r.Actual;
                    }
                });

            });

            // TXFinanceYearList txfl = new TXFinanceYearList(taxInfo.CompanyId);
            TXFinanceYearList txfl = taxInfo.finyearlist;

            taxInfo.Employees.ForEach(emp =>
            {

                List<TXFinanceYear> txf = txfl.Where(w => (w.StartingDate < emp.DateOfJoining && w.EndingDate >= emp.DateOfJoining)).ToList();
               
                taxInfo.Result.Add(new TaxHistory
                {
                    EmployeeId = emp.Id,
                    FinanceYearId = taxInfo.FinanceYearId,
                    FieldId =taxInfo.AttributemodelList.Where(a => a.Name == "GROSSSALARY").FirstOrDefault().Id,
                    Field = "Gross Salary",
                    Total = txf.Count > 0? taxInfo.Result.Where(r => r.EmployeeId == emp.Id && (r.FieldType == "Income" && r.Field!= "Total Earning")).Sum(r => r.Total): taxInfo.Result.Where(r => r.EmployeeId == emp.Id && (r.FieldType == "Income" && r.Field != "Total Earning") && !r.Field.Contains("Previous employer income after exemptions u/s 10")).Sum(r => r.Total),
                    FieldType="Income",                                                                                                                   
                    
                });

                if(txf.Count == 0)
                {
                    taxInfo.Result.RemoveAll(r => r.Field.Contains("Previous employer income after exemptions u/s 10"));
                }

            });

        }
    }
}
