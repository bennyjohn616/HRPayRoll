using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxBehaviorList : List<TaxBehavior>
    {
        public TaxBehaviorList()
        {
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TaxBehaviorList(Guid financeYearId, Guid id, Guid attributeId)
        {
            TaxBehavior txbehavior = new TaxBehavior();
            txbehavior.FinanceYearId = financeYearId;
            txbehavior.Id = id;
            txbehavior.AttributemodelId = attributeId;
            DataTable dtValue = txbehavior.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    TaxBehavior behavior = new TaxBehavior();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        behavior.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceYearId"])))
                        behavior.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AttributemodelId"])))
                        behavior.AttributemodelId = new Guid(Convert.ToString(dtValue.Rows[i]["AttributemodelId"]));
                    behavior.Value = Convert.ToString(dtValue.Rows[i]["Value"]);
                    behavior.Formula = Convert.ToString(dtValue.Rows[i]["Formula"]);
                    behavior.FieldFor = Convert.ToString(dtValue.Rows[i]["FieldFor"]);
                    behavior.BaseValue = Convert.ToString(dtValue.Rows[i]["BaseValue"]);
                    behavior.BaseFormula = Convert.ToString(dtValue.Rows[i]["BaseFormula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["SlabCategory"])))
                        behavior.SlabCategory = Convert.ToInt32(dtValue.Rows[i]["SlabCategory"]);
                    behavior.FieldType = Convert.ToString(dtValue.Rows[i]["FieldType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["InputType"])))
                        behavior.InputType = Convert.ToInt32(dtValue.Rows[i]["InputType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        behavior.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        behavior.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        behavior.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        behavior.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        behavior.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    this.Add(behavior);
                }
            }
        }

    }
}
