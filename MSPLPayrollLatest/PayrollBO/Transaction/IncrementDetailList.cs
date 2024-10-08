using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class IncrementDetailList : List<IncrementDetail>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public IncrementDetailList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public IncrementDetailList(Guid incrementId)
        {
            this.IncrementId = incrementId;
            IncrementDetail increment = new IncrementDetail();
            DataTable dtValue = increment.GetTableValues(Guid.Empty,this.IncrementId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    IncrementDetail incrementDetailTemp = new IncrementDetail();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        incrementDetailTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncrementId"])))
                        incrementDetailTemp.IncrementId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["IncrementId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        incrementDetailTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OldValue"])))
                        incrementDetailTemp.OldValue = Convert.ToDecimal(dtValue.Rows[rowcount]["OldValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NewValue"])))
                        incrementDetailTemp.NewValue = Convert.ToDecimal(dtValue.Rows[rowcount]["NewValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        incrementDetailTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        incrementDetailTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        incrementDetailTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        incrementDetailTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        incrementDetailTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(incrementDetailTemp);
                }

            }
        }


        #endregion

        #region property

        public Guid IncrementId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the incrementDetail and add to the list
        /// </summary>
        /// <param name="incrementDetail"></param>
        public void AddNew(IncrementDetail incrementDetail)
        {
            if (incrementDetail.Save())
            {
                this.Add(incrementDetail);
            }
        }

        /// <summary>
        /// Delete the incrementDetail and remove from the list
        /// </summary>
        /// <param name="incrementDetail"></param>
        public void DeleteExist(IncrementDetail incrementDetail)
        {
            if (incrementDetail.Delete())
            {
                this.Remove(incrementDetail);
            }
        }

        #endregion

        #region private methods


        #endregion
    }
}
