namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    public class PTaxList : List<PTax>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PTaxList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public PTaxList(int companyId)
        {
            this.CompanyId = companyId;
            PTax PTax = new PTax();
            DataTable dtValue = PTax.GetTableValues(Guid.Empty, this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PTax PTaxtemp = new PTax();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PTaxtemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PTaxtemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        PTaxtemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    PTaxtemp.FormNo = Convert.ToString(dtValue.Rows[rowcount]["FormNo"]);
                    PTaxtemp.Calculationtype = Convert.ToString(dtValue.Rows[rowcount]["Calculationtype"]);
                    PTaxtemp.DeductionMonth1 = Convert.ToString(dtValue.Rows[rowcount]["DeductionMonth1"]);
                    PTaxtemp.DeductionMonth2 = Convert.ToString(dtValue.Rows[rowcount]["DeductionMonth2"]);
                    PTaxtemp.RegCertificateNo = Convert.ToString(dtValue.Rows[rowcount]["RegCertificateNo"]);
                    PTaxtemp.PTOCircleNo = Convert.ToString(dtValue.Rows[rowcount]["PTOCircleNo"]);
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

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the JoiningDocument and add to the list
        /// </summary>
        /// <param name="joiningDocument"></param>
        public void AddNew(PTax PTax)
        {
            if (PTax.Save())
            {
                this.Add(PTax);
            }
        }

        /// <summary>
        /// Delete the JoiningDocument and remove from the list
        /// </summary>
        /// <param name="joiningDocument"></param>
        public void DeleteExist(PTax PTax)
        {
            if (PTax.Delete())
            {
                this.Remove(PTax);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
