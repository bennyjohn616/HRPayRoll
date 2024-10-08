using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
   public class PayheetAttributesList: List<Paysheetatrr>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public PayheetAttributesList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PayheetAttributesList(int id)
        {

            Paysheetatrr Paysheetatrr = new Paysheetatrr();
            DataTable dtValue = Paysheetatrr.GetTableValues(id);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Paysheetatrr PaysheetatrrTemp = new Paysheetatrr();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PaysheetatrrTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PaySheetId"])))
                        PaysheetatrrTemp.PaySheetId = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["PaySheetId"]));
                  
                    PaysheetatrrTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    PaysheetatrrTemp.FieldName = Convert.ToString(dtValue.Rows[rowcount]["FieldName"]);
                    PaysheetatrrTemp.DisplayAs = !string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"])) ? Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]) : Convert.ToString(dtValue.Rows[rowcount]["FieldName"]);
                    
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["OrderBy"])))
                        PaysheetatrrTemp.OrderBy = Convert.ToInt32(dtValue.Rows[rowcount]["OrderBy"]);
                    PaysheetatrrTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    
                    this.Add(PaysheetatrrTemp);
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid ConfigurationId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Paysheetatrr
        /// </summary>
        /// <returns></returns>
        public void AddNew(Paysheetatrr Paysheetatrr)
        {

            if (Paysheetatrr.Save())
            {
                this.Add(Paysheetatrr);
            }
        }

        /// <summary>
        /// Delete the Paysheetatrr
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(Paysheetatrr Paysheetatrr)
        {
            //if (Paysheetatrr.Delete())
            //{
            //    this.Remove(Paysheetatrr);
            //}
        }


        #endregion

        #region private methods


        #endregion
    }
}
