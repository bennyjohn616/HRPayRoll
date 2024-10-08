using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class FullFinalSettlementDetailList : List<FullFinalSettlementDetail>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FullFinalSettlementDetailList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public FullFinalSettlementDetailList(Guid fullFinalSettlementId)
        {
            this.FullFinalSettlementId = fullFinalSettlementId;
            FullFinalSettlementDetail fullFinalSettlementDetail = new FullFinalSettlementDetail();
            DataTable dtValue = fullFinalSettlementDetail.GetTableValues(Guid.Empty, this.FullFinalSettlementId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    FullFinalSettlementDetail fullFinalSettlementDetailTemp = new FullFinalSettlementDetail();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        fullFinalSettlementDetailTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FullFinalSettlementId"])))
                        fullFinalSettlementDetailTemp.FullFinalSettlementId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FullFinalSettlementId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        fullFinalSettlementDetailTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        fullFinalSettlementDetailTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        fullFinalSettlementDetailTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Amount"])))
                        fullFinalSettlementDetailTemp.Amount = Convert.ToDecimal(dtValue.Rows[rowcount]["Amount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TaxAmount"])))
                        fullFinalSettlementDetailTemp.TaxAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["TaxAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        fullFinalSettlementDetailTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        fullFinalSettlementDetailTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        fullFinalSettlementDetailTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        fullFinalSettlementDetailTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(fullFinalSettlementDetailTemp);
                }

            }
        }


        #endregion

        #region property

        public Guid FullFinalSettlementId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the incrementDetail and add to the list
        /// </summary>
        /// <param name="fullFinalSettlementDetail"></param>
        public void AddNew(FullFinalSettlementDetail fullFinalSettlementDetail)
        {
            if (fullFinalSettlementDetail.Save())
            {
                this.Add(fullFinalSettlementDetail);
            }
        }

        /// <summary>
        /// Delete the incrementDetail and remove from the list
        /// </summary>
        /// <param name="fullFinalSettlementDetail"></param>
        public void DeleteExist(FullFinalSettlementDetail fullFinalSettlementDetail)
        {
            if (fullFinalSettlementDetail.Delete())
            {
                this.Remove(fullFinalSettlementDetail);
            }
        }

        #endregion

        #region private methods


        #endregion
    }
}
