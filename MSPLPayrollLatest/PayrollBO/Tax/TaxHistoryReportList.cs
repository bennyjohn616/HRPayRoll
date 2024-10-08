using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxHistoryReportList: List<TaxHistoryReport>
    {

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TaxHistoryReportList()
        {

        }

        public TaxHistoryReportList(Guid financeyearId, int companyid, DateTime applyDate)
        {


            TaxHistoryReport txHistory = new TaxHistoryReport();
           
            DataTable dtValue = txHistory.GetTableValues(financeyearId, companyid, applyDate);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistoryReport txSectionTemp = new TaxHistoryReport();


                    txSectionTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.Fieldid = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FirstOrder"])))
                        txSectionTemp.FirstOrder = Convert.ToString(dtValue.Rows[rowcount]["FirstOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SecondOrder"])))
                        txSectionTemp.SecondOrder = Convert.ToString(dtValue.Rows[rowcount]["SecondOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ThirdOrder"])))
                        txSectionTemp.ThirdOrder = Convert.ToString(dtValue.Rows[rowcount]["ThirdOrder"]);

                    this.Add(txSectionTemp);
                }

            }
        }
        #endregion
    }
}
