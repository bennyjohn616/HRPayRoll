using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO.Tax
{
   public class TXChallanEntryList:List<TXChallanEntry>
    {
        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXChallanEntryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>    

        public TXChallanEntryList(Guid financeyearId, DateTime applyDate)
        {

            TXChallanEntry txHistory = new TXChallanEntry();
            txHistory.FinanceYearId = financeyearId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXChallanEntry TaxChallanEntry = new TXChallanEntry();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TaxChallanEntry.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        TaxChallanEntry.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        TaxChallanEntry.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        TaxChallanEntry.bankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        TaxChallanEntry.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanDate"])))
                        TaxChallanEntry.challanDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ChallanDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"])))
                        TaxChallanEntry.challanNo = Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"])))
                        TaxChallanEntry.checkdd = Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BookEntry"])))
                        TaxChallanEntry.bookEntry = Convert.ToBoolean(dtValue.Rows[rowcount]["BookEntry"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxAmount"])))
                        TaxChallanEntry.TaxAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxAmount"]);



                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        TaxChallanEntry.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        TaxChallanEntry.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        TaxChallanEntry.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        TaxChallanEntry.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        TaxChallanEntry.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BSRCode"])))
                        TaxChallanEntry.BSRCode = Convert.ToString(dtValue.Rows[rowcount]["BSRCode"]);
                    this.Add(TaxChallanEntry);
                }

            }
        }

        public TXChallanEntryList(Guid financeyearId, Guid employeeId, DateTime applyDate)
        {


            TXChallanEntry txHistory = new TXChallanEntry();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXChallanEntry TaxChallanEntry = new TXChallanEntry();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TaxChallanEntry.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        TaxChallanEntry.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        TaxChallanEntry.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        TaxChallanEntry.bankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        TaxChallanEntry.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanDate"])))
                        TaxChallanEntry.challanDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ChallanDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"])))
                        TaxChallanEntry.challanNo = Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"])))
                        TaxChallanEntry.checkdd = Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BookEntry"])))
                        TaxChallanEntry.bookEntry = Convert.ToBoolean(dtValue.Rows[rowcount]["BookEntry"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxAmount"])))
                        TaxChallanEntry.TaxAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxAmount"]);


                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        TaxChallanEntry.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        TaxChallanEntry.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        TaxChallanEntry.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        TaxChallanEntry.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        TaxChallanEntry.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BSRCode"])))
                        TaxChallanEntry.BSRCode = Convert.ToString(dtValue.Rows[rowcount]["BSRCode"]);
                    this.Add(TaxChallanEntry);
                }

            }
        }

        public TXChallanEntryList(Guid financeyearId, Guid employeeId)
        {


            TXChallanEntry txHistory = new TXChallanEntry();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;

            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXChallanEntry TaxChallanEntry = new TXChallanEntry();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TaxChallanEntry.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        TaxChallanEntry.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        TaxChallanEntry.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        TaxChallanEntry.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        TaxChallanEntry.bankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        TaxChallanEntry.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanDate"])))
                        TaxChallanEntry.challanDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ChallanDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"])))
                        TaxChallanEntry.challanNo = Convert.ToString(dtValue.Rows[rowcount]["ChallanNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"])))
                        TaxChallanEntry.checkdd = Convert.ToString(dtValue.Rows[rowcount]["Cheque/DD"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BookEntry"])))
                        TaxChallanEntry.bookEntry = Convert.ToBoolean(dtValue.Rows[rowcount]["BookEntry"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxAmount"])))
                        TaxChallanEntry.TaxAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxAmount"]);



                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        TaxChallanEntry.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        TaxChallanEntry.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        TaxChallanEntry.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        TaxChallanEntry.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        TaxChallanEntry.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BSRCode"])))
                        TaxChallanEntry.BSRCode = Convert.ToString(dtValue.Rows[rowcount]["BSRCode"]);
                    this.Add(TaxChallanEntry);
                }

            }
        }
        #endregion
    }
}
