using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxHistoryList : List<TaxHistory>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TaxHistoryList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>    

        public TaxHistoryList(Guid financeyearId, DateTime applyDate)
        {
        
            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TaxHistoryList(Guid financeyearId, DateTime applyDate,string emproll)
        {

            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValuesTemp();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TaxHistoryList( Guid financeyearId, Guid employeeId,DateTime applyDate)
        {
            
          
            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSectionTemp);
                }

            }
        }
        public TaxHistoryList(Guid financeyearId, Guid employeeId, DateTime applyDate, string type)
        {


            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValuesAP();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ActualMonth"])))
                        txSectionTemp.ActualMonth = Convert.ToInt32(dtValue.Rows[rowcount]["ActualMonth"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TaxHistoryList(Guid financeyearId, Guid employeeId, DateTime applyDate, string type,string emproll)
        {


            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;
            txHistory.ApplyDate = applyDate;
            DataTable dtValue = txHistory.GetTableValuesTempAP();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ActualMonth"])))
                        txSectionTemp.ActualMonth = Convert.ToInt32(dtValue.Rows[rowcount]["ActualMonth"]);
                    this.Add(txSectionTemp);
                }

            }
        }

        public TaxHistoryList(Guid financeyearId, Guid employeeId)
        {


            TaxHistory txHistory = new TaxHistory();
            txHistory.FinanceYearId = financeyearId;
            txHistory.EmployeeId = employeeId;
          
            DataTable dtValue = txHistory.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TaxHistory txSectionTemp = new TaxHistory();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                        txSectionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyDate"])))
                        txSectionTemp.ApplyDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ApplyDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FieldId"])))
                        txSectionTemp.FieldId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FieldId"]));
                    txSectionTemp.Field = Convert.ToString(dtValue.Rows[rowcount]["Field"]);
                    txSectionTemp.FieldType = Convert.ToString(dtValue.Rows[rowcount]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Actual"])))
                        txSectionTemp.Actual = Convert.ToDecimal(dtValue.Rows[rowcount]["Actual"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Projection"])))
                        txSectionTemp.Projection = Convert.ToDecimal(dtValue.Rows[rowcount]["Projection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Total"])))
                        txSectionTemp.Total = Convert.ToDecimal(dtValue.Rows[rowcount]["Total"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Limit"])))
                        txSectionTemp.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txSectionTemp.Createdby = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(txSectionTemp);
                }

            }
        }
        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public Guid FinancialYearId { get; set; }

        public Guid Parentid { get; set; }
        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax section and add to the list
        /// </summary>
        /// <param name="txSection"></param>
        public void AddNew(TaxHistory txHistory)
        {
            if (txHistory.Save())
            {
                this.Add(txHistory);
            }
        }

        /// <summary>
        /// delete the tax section data
        /// </summary>
        /// <param name="txSection"></param>

        public void DeleteExist(TaxHistory txHistory)
        {
            if (txHistory.Delete())
            {
                this.Remove(txHistory);
            }
        }


        #endregion
    }
}
