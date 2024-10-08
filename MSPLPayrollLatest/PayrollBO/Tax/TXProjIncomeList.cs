using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO.Tax
{
        public class TXProjIncomeList : List<TXProjIncome>
        {
            #region private variable


            #endregion

            #region construstor

            /// <summary>
            /// initialize the object
            /// </summary>
            public TXProjIncomeList()
            {

            }
            public TXProjIncomeList(Guid financeyear, Guid employeeId,int Month,int year)
            {
                TXProjIncome txprojIncome = new TXProjIncome();
                txprojIncome.EmployeeId = employeeId;
                txprojIncome.financeyear = financeyear;
                txprojIncome.Month = Month;
                txprojIncome.Year = year;
                DataTable dtValue = txprojIncome.GetTableValues();
                if (dtValue.Rows.Count > 0)
                {
                    for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                    {
                    TXProjIncome txprjincTemp = new TXProjIncome();
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyear"])))
                            txprjincTemp.financeyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyear"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                            txprjincTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MonthCol"])))
                        txprjincTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["MonthCol"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["YearCol"])))
                        txprjincTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["YearCol"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome1"])))
                            txprjincTemp.Income1 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome1"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome2"])))
                            txprjincTemp.Income2 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome2"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome3"])))
                            txprjincTemp.Income3 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome3"]);

                    this.Add(txprjincTemp);
                    }
                }
            }


            #endregion

            #region property

            /// <summary>
            /// Get or Set the CompanyId
            /// </summary>
            public Guid EmployeeId { get; set; }



            #endregion

            #region Public methods

            /// <summary>
            /// Save the Tax txEmployeeSection and add to the list
            /// </summary>
            /// <param name="txEmployeeSection"></param>
            public void AddNew(TXProjIncome txprjIncome)
            {
                if (txprjIncome.Save())
                {
                    this.Add(txprjIncome);
                }
            }

            #endregion

        }
    }
