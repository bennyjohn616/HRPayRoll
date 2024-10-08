using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class BankList : List<Bank>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public BankList(int companyId)
        {
            Bank bank = new Bank();
            DataTable dtValue = bank.GetTableValues(Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Bank bankTemp = new Bank();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        bankTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    bankTemp.BankName = Convert.ToString(dtValue.Rows[rowcount]["BankName"]);                  
                  
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        bankTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        bankTemp.CreatedBy = dtValue.Rows[rowcount]["CreatedBy"].ToString();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        bankTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        bankTemp.ModifiedBy = dtValue.Rows[rowcount]["ModifiedBy"].ToString();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        bankTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(bankTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(Bank bank)
        {
            if (bank.Save())
            {
                this.Add(bank);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="category"></param>
        public void DeleteExist(Bank bank)
        {
            if (bank.Delete())
            {
                this.Remove(bank);
            }
        }


        #endregion

        #region private methods




        #endregion

    }
}
