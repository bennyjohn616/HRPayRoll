
namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    public class TxActualRentPaidList : List<TXActualRentPaid>
    {

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TxActualRentPaidList()
        {

        }
        public TxActualRentPaidList(Guid FinanceYearId, Guid employeeId,Guid Txsecempid)
        {
            TXActualRentPaid txActualRentPaid = new TXActualRentPaid();
            txActualRentPaid.FinanceYearId = FinanceYearId;
            txActualRentPaid.EmployeeId = employeeId;
            txActualRentPaid.TXEmployeeSectionId = Txsecempid;

            DataTable dtValue = txActualRentPaid.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXActualRentPaid txActualrentTemp = new TXActualRentPaid();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txActualrentTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txActualrentTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txActualrentTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    txActualrentTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    txActualrentTemp.MetroRent = Convert.ToDecimal(dtValue.Rows[rowcount]["MetroRent"]);
                    txActualrentTemp.NonMetroRent = Convert.ToDecimal(dtValue.Rows[rowcount]["NonMetroRent"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txActualrentTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txActualrentTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txActualrentTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txActualrentTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txActualrentTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TXEmployeeSectionId"])))
                        txActualrentTemp.TXEmployeeSectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TXEmployeeSectionId"]));
                    this.Add(txActualrentTemp);
                }
            }
        }
        #endregion
    }
}
