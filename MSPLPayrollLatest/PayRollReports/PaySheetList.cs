using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
   public class PaySheetList:List<Paysheet>
    {
        private int companyId;
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySheetList()
        {

        }



        public PaySheetList(int companyId)
        {
            this.companyId = companyId;
            Paysheet Paysheet = new Paysheet();
            DataTable dtValue = Paysheet.GetTableValues1(companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Paysheet PaysheetTemp = new Paysheet();


                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PaysheetTemp.Id = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    PaysheetTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PaysheetTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);

                    PaysheetTemp.Title = Convert.ToString(dtValue.Rows[rowcount]["Title"]);
                    PaysheetTemp.Categories = Convert.ToString(dtValue.Rows[rowcount]["Categories"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        PaysheetTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        PaysheetTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        PaysheetTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        PaysheetTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        PaysheetTemp.Status = Convert.ToBoolean(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDetail"])))
                        PaysheetTemp.IsDetail = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDetail"]);
                    this.Add(PaysheetTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PaySheetList(int id,int companyId)
        {

            Paysheet Paysheet = new Paysheet();
            DataTable dtValue = Paysheet.GetTableValues(id,companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Paysheet PaysheetTemp = new Paysheet();

                    
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PaysheetTemp.Id = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    PaysheetTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PaysheetTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);

                    PaysheetTemp.Title = Convert.ToString(dtValue.Rows[rowcount]["Title"]);
                    PaysheetTemp.Categories = Convert.ToString(dtValue.Rows[rowcount]["Categories"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        PaysheetTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        PaysheetTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        PaysheetTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        PaysheetTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        PaysheetTemp.Status = Convert.ToBoolean(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDetail"])))
                        PaysheetTemp.IsDetail = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDetail"]);
                    this.Add(PaysheetTemp);
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
        public void AddNew(Paysheet Paysheet)
        {

            if (Paysheet.Save())
            {
                this.Add(Paysheet);
            }
        }

        /// <summary>
        /// Delete the Paysheetatrr
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(Paysheet Paysheet)
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
