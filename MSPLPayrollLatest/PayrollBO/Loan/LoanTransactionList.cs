using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LoanTransactionList : List<LoanTransaction>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoanTransactionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LoanTransactionList(Guid loanEntryId)
        {
            LoanTransaction loanTransaction = new LoanTransaction();
            DataTable dtValue = loanTransaction.GetTableValues(Guid.Empty, loanEntryId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LoanTransaction loanTransactionTemp = new LoanTransaction();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        loanTransactionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LoanEntryId"])))
                        loanTransactionTemp.LoanEntryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["LoanEntryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AmtPaid"])))
                        loanTransactionTemp.AmtPaid = Convert.ToDecimal(dtValue.Rows[rowcount]["AmtPaid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InterestAmt"])))
                        loanTransactionTemp.InterestAmt = Convert.ToDecimal(dtValue.Rows[rowcount]["InterestAmt"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isForClose"])))
                        loanTransactionTemp.isForClose = Convert.ToBoolean(dtValue.Rows[rowcount]["isForClose"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isPayRollProcess"])))
                        loanTransactionTemp.isPayRollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["isPayRollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FAndFprocess"])))
                        loanTransactionTemp.isFandFProcessv = Convert.ToBoolean(dtValue.Rows[rowcount]["FAndFprocess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AppliedOn"])))
                        loanTransactionTemp.AppliedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["AppliedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        loanTransactionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        loanTransactionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        loanTransactionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        loanTransactionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        loanTransactionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    loanTransactionTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    this.Add(loanTransactionTemp);
                }

            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public Guid LoanEntryId { get; set; }
             
        #endregion

        #region Public methods

        /// <summary>
        /// Save the loanTransaction and add to the list
        /// </summary>
        /// <param name="loanTransaction"></param>
        public void AddNew(LoanTransaction loanTransaction)
        {
            if (loanTransaction.Save())
            {
                this.Add(loanTransaction);
            }
        }


        public void DeleteExist(LoanTransaction loanTransaction)
        {
            if (loanTransaction.Delete())
            {
                this.Remove(loanTransaction);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
