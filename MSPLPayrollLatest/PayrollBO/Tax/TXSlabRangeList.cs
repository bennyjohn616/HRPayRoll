
// -----------------------------------------------------------------------
// <copyright file="TXFinanceYearList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO.Tax
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To handle the TXSlabRangeList
    /// </summary>
    public class TXSlabRangeList : List<TXSlabRange>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSlabRangeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSlabRangeList(int slabCategoryId)
        {
            this.SlabCategoryId = slabCategoryId;
            TXSlabRange txSlabRange = new TXSlabRange();
            DataTable dtValue = txSlabRange.GetTableValues(0, this.SlabCategoryId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSlabRange txSlabRangeTemp = new TXSlabRange();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSlabRangeTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SlabCategoryId"])))
                        txSlabRangeTemp.SlabCategoryId = Convert.ToInt32(dtValue.Rows[rowcount]["SlabCategoryId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        txSlabRangeTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RangeFrom"])))
                        txSlabRangeTemp.RangeFrom = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RangeTo"])))
                        txSlabRangeTemp.RangeTo = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeTo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxValue"])))
                        txSlabRangeTemp.TaxValue = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPercentage"])))
                        txSlabRangeTemp.IsPercentage = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPercentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSlabRangeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSlabRangeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSlabRangeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSlabRangeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSlabRangeTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSlabRangeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSlabRangeTemp);
                }

            }
        }

        public TXSlabRangeList(Guid financialYearId)
        {
            this.FinancialYearId = financialYearId;
            TXSlabRange txSlabRange = new TXSlabRange();
            DataTable dtValue = txSlabRange.GetTableValues(0, 0, this.FinancialYearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXSlabRange txSlabRangeTemp = new TXSlabRange();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSlabRangeTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SlabCategoryId"])))
                        txSlabRangeTemp.SlabCategoryId = Convert.ToInt32(dtValue.Rows[rowcount]["SlabCategoryId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
                        txSlabRangeTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RangeFrom"])))
                        txSlabRangeTemp.RangeFrom = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RangeTo"])))
                        txSlabRangeTemp.RangeTo = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeTo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxValue"])))
                        txSlabRangeTemp.TaxValue = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPercentage"])))
                        txSlabRangeTemp.IsPercentage = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPercentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSlabRangeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSlabRangeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSlabRangeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSlabRangeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txSlabRangeTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSlabRangeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSlabRangeTemp);
                }

            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int SlabCategoryId { get; set; }

        public Guid FinancialYearId { get; set; }
        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax SlabRange and add to the list
        /// </summary>
        /// <param name="txSlabRange"></param>
        public void AddNew(TXSlabRange txSlabRange)
        {
            if (txSlabRange.Save())
            {
                this.Add(txSlabRange);
            }
        }

        /// <summary>
        /// delete the tax SlabRange year data
        /// </summary>
        /// <param name="txSlabRange"></param>

        public void DeleteExist(TXSlabRange txSlabRange)
        {
            if (txSlabRange.Delete())
            {
                this.Remove(txSlabRange);
            }
        }


        #endregion

    }
}
