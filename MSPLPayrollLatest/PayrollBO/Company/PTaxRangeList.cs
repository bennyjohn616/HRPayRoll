namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    public class PTaxRangeList : List<PTaxRange>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PTaxRangeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public PTaxRangeList(Guid pTaxId)
        {
           
            PTaxRange PTax = new PTaxRange();
            DataTable dtValue = PTax.GetTableValues(Guid.Empty, pTaxId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PTaxRange PTaxtemp = new PTaxRange();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PTaxtemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));                    
                    PTaxtemp.RangeFrom = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeFrom"]);
                    PTaxtemp.RangeTo = Convert.ToDecimal(dtValue.Rows[rowcount]["RangeTo"]);
                    PTaxtemp.Amt = Convert.ToDecimal(dtValue.Rows[rowcount]["Amt"]);                   
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        PTaxtemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        PTaxtemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        PTaxtemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        PTaxtemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        PTaxtemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(PTaxtemp);
                }
            }
        }
                
        #endregion

        #region Public methods

        /// <summary>
        /// Save the JoiningDocument and add to the list
        /// </summary>
        /// <param name="joiningDocument"></param>
        public void AddNew(PTaxRange PTaxRange)
        {
            if (PTaxRange.Save())
            {
                this.Add(PTaxRange);
            }
        }

        /// <summary>
        /// Delete the JoiningDocument and remove from the list
        /// </summary>
        /// <param name="joiningDocument"></param>
        public void DeleteExist(PTaxRange PTaxRange)
        {
            if (PTaxRange.Delete())
            {
                this.Remove(PTaxRange);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
