using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PayrollHistoryValueList : List<PayrollHistoryValue>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public PayrollHistoryValueList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PayrollHistoryValueList(Guid payrollHistoryId)
        {
            this.PayrollHistroyId = payrollHistoryId;
            PayrollHistoryValue payrollHistoryValue = new PayrollHistoryValue();
            DataTable dtValue = payrollHistoryValue.GetTableValues(Guid.Empty, this.PayrollHistroyId, Guid.Empty, 0, 0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistoryValue payrollHistoryValueTemp = new PayrollHistoryValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"])))
                        payrollHistoryValueTemp.PayrollHistoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        payrollHistoryValueTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    payrollHistoryValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    payrollHistoryValueTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["Basevalue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ValueType"])))
                        payrollHistoryValueTemp.ValueType = Convert.ToInt32(dtValue.Rows[rowcount]["ValueType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryValueTemp);
                }
            }
        }

        public PayrollHistoryValueList(int year, int month)
        {
            PayrollHistoryValue payrollHistoryValue = new PayrollHistoryValue();
            DataTable dtValue = payrollHistoryValue.GetTableValues(Guid.Empty, Guid.Empty, Guid.Empty, month, year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistoryValue payrollHistoryValueTemp = new PayrollHistoryValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryValueTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"])))
                        payrollHistoryValueTemp.PayrollHistoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        payrollHistoryValueTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    payrollHistoryValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    payrollHistoryValueTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["Basevalue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ValueType"])))
                        payrollHistoryValueTemp.ValueType = Convert.ToInt32(dtValue.Rows[rowcount]["ValueType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryValueTemp);
                }
            }
        }
        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid PayrollHistroyId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the PayrollHistoryValue
        /// </summary>
        /// <returns></returns>
        public void AddNew(PayrollHistoryValue payrollHistoryValue)
        {

            if (payrollHistoryValue.Save())
            {
                this.Add(payrollHistoryValue);
            }
        }

        /// <summary>
        /// Delete the PayrollHistoryValue
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(PayrollHistoryValue payrollHistoryValue)
        {
            if (payrollHistoryValue.Delete())
            {
                this.Remove(payrollHistoryValue);
            }
        }


        #endregion

        #region private methods


        #endregion
    }
}
