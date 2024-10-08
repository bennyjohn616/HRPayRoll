using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PayrollHistoryList : List<PayrollHistory>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PayrollHistoryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="year"></param>
        public PayrollHistoryList(int CompanyId, int year, int month)
        {
            this.Year = year;
            this.Month = month;
            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(Guid.Empty, CompanyId, Guid.Empty, this.Month, this.Year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        payrollHistoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        payrollHistoryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        payrollHistoryTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        payrollHistoryTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    payrollHistoryTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsFandF"])))
                        payrollHistoryTemp.IsFandF = Convert.ToBoolean(dtValue.Rows[rowcount]["IsFandF"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }
        public PayrollHistoryList(Guid employeeId, int CompanyId,int month,int year)
        {
            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(Guid.Empty, CompanyId, employeeId, month, year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        payrollHistoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        payrollHistoryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        payrollHistoryTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        payrollHistoryTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    payrollHistoryTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }

        public PayrollHistoryList(Guid employeeId, int CompanyId)
        {
            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(Guid.Empty, CompanyId, employeeId, this.Month, this.Year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        payrollHistoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        payrollHistoryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        payrollHistoryTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        payrollHistoryTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    payrollHistoryTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }

        public PayrollHistoryList(int companyId)
        {
            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="year"></param>
      


        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="year"></param>
        public PayrollHistoryList(int CompanyId, int syear, int smonth, int nyear, int nmonth, Guid employeeId)
        {

            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(Guid.Empty, CompanyId, smonth, syear, nmonth, nyear, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        payrollHistoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        payrollHistoryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        payrollHistoryTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        payrollHistoryTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    payrollHistoryTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }


        public PayrollHistoryList(int CompanyId,  Guid employeeId)
        {

            PayrollHistory payrollHistory = new PayrollHistory();
            DataTable dtValue = payrollHistory.GetTableValues(Guid.Empty, CompanyId, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollHistory payrollHistoryTemp = new PayrollHistory();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        payrollHistoryTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        payrollHistoryTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        payrollHistoryTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        payrollHistoryTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        payrollHistoryTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        payrollHistoryTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        payrollHistoryTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    payrollHistoryTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        payrollHistoryTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        payrollHistoryTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        payrollHistoryTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        payrollHistoryTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        payrollHistoryTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(payrollHistoryTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// get or set the Type
        /// </summary>
        public int Year { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the PayrollHistory and add to the list
        /// </summary>
        /// <param name="payrollHistory"></param>
        public void AddNew(PayrollHistory payrollHistory)
        {
            if (payrollHistory.Save())
            {
                this.Add(payrollHistory);
            }
        }

        /// <summary>
        /// Delete the AttributeModel and remove from the list
        /// </summary>
        /// <param name="payrollHistory"></param>
        public void DeleteExist(PayrollHistory payrollHistory)
        {
            if (payrollHistory.Delete())
            {
                this.Remove(payrollHistory);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
