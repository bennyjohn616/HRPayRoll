using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PFChallanList : List<PFChallan>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PFChallanList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="year"></param>
        public PFChallanList(int CompanyId,int id)
        {
            PFChallan PFChellon = new PFChallan();
            DataTable dtValue = PFChellon.GetTableValues(CompanyId,id);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PFChallan PFChallanTemp = new PFChallan();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PFChallanTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Company"])))
                        PFChallanTemp.CompanyId = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["Company"]));
                    PFChallanTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    PFChallanTemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                    PFChallanTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        PFChallanTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        PFChallanTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        PFChallanTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        PFChallanTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        PFChallanTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayOrder"])))
                        PFChallanTemp.DisplayOrder= Convert.ToInt32(dtValue.Rows[rowcount]["DisplayOrder"]);
                    this.Add(PFChallanTemp);
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
        /// Save the PFChellon and add to the list
        /// </summary>
        /// <param name="PFChellon"></param>
        public void AddNew(PFChallan PFChellon)
        {
            if (PFChellon.Save())
            {
                this.Add(PFChellon);
            }
        }




        #endregion

    }
}
