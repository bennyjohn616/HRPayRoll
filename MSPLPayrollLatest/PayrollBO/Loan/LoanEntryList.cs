using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;


namespace PayrollBO
{
    public class LoanEntryList : List<LoanEntry>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoanEntryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LoanEntryList(Guid employeeId)
        {
            LoanEntry LoanEntry = new LoanEntry();
            DataTable dtValue = LoanEntry.GetTableValues(Guid.Empty, Guid.Empty, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LoanEntry LoanEntryTemp = new LoanEntry();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        LoanEntryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    LoanEntryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    LoanEntryTemp.LoanMasterId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["LoanMasterId"]));
                    LoanEntryTemp.LoanDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LoanDate"]);
                    LoanEntryTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    LoanEntryTemp.LoanAmt = Convert.ToDecimal(dtValue.Rows[rowcount]["LoanAmt"]);
                    LoanEntryTemp.NoOfMonths = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfMonths"]);
                    LoanEntryTemp.AmtPerMonth = Convert.ToInt32(dtValue.Rows[rowcount]["AmtPerMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        LoanEntryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        LoanEntryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        LoanEntryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        LoanEntryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        LoanEntryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                        LoanEntryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsForeClose"])))
                        LoanEntryTemp.IsForeClose = Convert.ToBoolean(dtValue.Rows[rowcount]["IsForeClose"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ForeCloseDate"])))
                        LoanEntryTemp.ForeCloseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ForeCloseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ForeCloseReverseDate"])))
                        LoanEntryTemp.ForeCloseReverseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ForeCloseReverseDate"]);
                    LoanEntryTemp.Reason = Convert.ToString(dtValue.Rows[rowcount]["Reason"]);


                    this.Add(LoanEntryTemp);
                }

            }
        }

        public LoanEntryList(Guid employeeId, Guid loanMasterId)
        {
            LoanEntry LoanEntry = new LoanEntry();
            DataTable dtValue = LoanEntry.GetTableValues(Guid.Empty, loanMasterId, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    LoanEntry LoanEntryTemp = new LoanEntry();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        LoanEntryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    LoanEntryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    LoanEntryTemp.LoanMasterId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["LoanMasterId"]));
                    LoanEntryTemp.LoanDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LoanDate"]);
                    LoanEntryTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    LoanEntryTemp.LoanAmt = Convert.ToDecimal(dtValue.Rows[rowcount]["LoanAmt"]);
                    LoanEntryTemp.NoOfMonths = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfMonths"]);
                    LoanEntryTemp.AmtPerMonth = Convert.ToInt32(dtValue.Rows[rowcount]["AmtPerMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        LoanEntryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        LoanEntryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        LoanEntryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        LoanEntryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        LoanEntryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                        LoanEntryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsForeClose"])))
                        LoanEntryTemp.IsForeClose = Convert.ToBoolean(dtValue.Rows[rowcount]["IsForeClose"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ForeCloseDate"])))
                        LoanEntryTemp.ForeCloseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ForeCloseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ForeCloseReverseDate"])))
                        LoanEntryTemp.ForeCloseReverseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ForeCloseReverseDate"]);
                    LoanEntryTemp.Reason = Convert.ToString(dtValue.Rows[rowcount]["Reason"]);
                    this.Add(LoanEntryTemp);
                }

            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(LoanEntry LoanEntry)
        {
            if (LoanEntry.Save())
            {
                this.Add(LoanEntry);
            }
        }


        public void DeleteExist(LoanEntry LoanEntry)
        {
            if (LoanEntry.Delete())
            {
                this.Remove(LoanEntry);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}

