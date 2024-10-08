
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class StopPaymentList : List<StopPayment>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public StopPaymentList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public StopPaymentList(Guid employeeId)
        {
            this.StopPaymentId = StopPaymentId;
            StopPayment increment = new StopPayment();
            DataTable dtValue = increment.GetTableValues(Guid.Empty, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    StopPayment StopPaymentTemp = new StopPayment();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        StopPaymentTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        StopPaymentTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPaymentMonth"])))
                        StopPaymentTemp.StopPaymentMonth = Convert.ToInt32(dtValue.Rows[rowcount]["StopPaymentMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPaymentYear"])))
                        StopPaymentTemp.StopPaymentYear = Convert.ToInt32(dtValue.Rows[rowcount]["StopPaymentYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPaymentType"])))
                        StopPaymentTemp.StopPaymentType = Convert.ToInt32(dtValue.Rows[rowcount]["StopPaymentType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Remarks"])))
                        StopPaymentTemp.Remarks = Convert.ToString(dtValue.Rows[rowcount]["Remarks"]);                   
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        StopPaymentTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        StopPaymentTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        StopPaymentTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        StopPaymentTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        StopPaymentTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(StopPaymentTemp);
                }

            }
        }


        #endregion

        #region property

        public Guid StopPaymentId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the StopPayment and add to the list
        /// </summary>
        /// <param name="StopPayment"></param>
        public void AddNew(StopPayment StopPayment)
        {
            if (StopPayment.Save())
            {
                this.Add(StopPayment);
            }
        }

        /// <summary>
        /// Delete the StopPayment and remove from the list
        /// </summary>
        /// <param name="StopPayment"></param>
        public void DeleteExist(StopPayment StopPayment)
        {
            if (StopPayment.Delete())
            {
                this.Remove(StopPayment);
            }
        }

        #endregion

        #region private methods


        #endregion
    }
}

