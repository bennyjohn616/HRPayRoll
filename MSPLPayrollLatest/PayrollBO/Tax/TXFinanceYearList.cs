
// -----------------------------------------------------------------------
// <copyright file="TXFinanceYearList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To handle the TXFinanceYear
    /// </summary>
    public class TXFinanceYearList : List<TXFinanceYear>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXFinanceYearList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXFinanceYearList(int companyId,bool isactive=false)
        {
            this.CompanyId = companyId;
            TXFinanceYear LoanEntry = new TXFinanceYear();
            DataTable dtValue = LoanEntry.GetTableValues(Guid.Empty, companyId, isactive);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXFinanceYear txFinanceYearTemp = new TXFinanceYear();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txFinanceYearTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StartingDate"])))
                        txFinanceYearTemp.StartingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["StartingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EndingDate"])))
                        txFinanceYearTemp.EndingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EndingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        txFinanceYearTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    txFinanceYearTemp.TanNo = Convert.ToString(dtValue.Rows[rowcount]["TanNo"]);
                    txFinanceYearTemp.TDSCircle = Convert.ToString(dtValue.Rows[rowcount]["TDSCircle"]);
                    txFinanceYearTemp.PANorGIRNO = Convert.ToString(dtValue.Rows[rowcount]["PANorGIRNO"]);
                    txFinanceYearTemp.TaxDeuctionAcNo = Convert.ToString(dtValue.Rows[rowcount]["TaxDeuctionAcNo"]);
                    txFinanceYearTemp.Place = Convert.ToString(dtValue.Rows[rowcount]["Place"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InchargeEmployeeId"])))
                        txFinanceYearTemp.InchargeEmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["InchargeEmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txFinanceYearTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txFinanceYearTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txFinanceYearTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txFinanceYearTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txFinanceYearTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txFinanceYearTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txFinanceYearTemp);
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
        /// Save the Tax FinanceYear and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(TXFinanceYear txFinanceYear)
        {
            if (txFinanceYear.Save())
            {
                this.Add(txFinanceYear);
            }
        }

        /// <summary>
        /// delete the tax finance year data
        /// </summary>
        /// <param name="txFinanceYear"></param>

        public void DeleteExist(TXFinanceYear txFinanceYear)
        {
            if (txFinanceYear.Delete())
            {
                this.Remove(txFinanceYear);
            }
        }


        #endregion

    }
}
