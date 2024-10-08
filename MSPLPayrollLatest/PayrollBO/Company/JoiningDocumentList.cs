// -----------------------------------------------------------------------
// <copyright file="JoiningDocumentList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class JoiningDocumentList : List<JoiningDocument>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public JoiningDocumentList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        public JoiningDocumentList(int companyId)
        {
            this.CompanyId = companyId;
            JoiningDocument joiningDocument = new JoiningDocument();
            DataTable dtValue = joiningDocument.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    JoiningDocument joiningDocumentTemp = new JoiningDocument();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        joiningDocumentTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        joiningDocumentTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    joiningDocumentTemp.DocumentName = Convert.ToString(dtValue.Rows[rowcount]["DocumentName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        joiningDocumentTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        joiningDocumentTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        joiningDocumentTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        joiningDocumentTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        joiningDocumentTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(joiningDocumentTemp);
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
        public void AddNew(JoiningDocument joiningDocument)
        {
            if (joiningDocument.Save())
            {
                this.Add(joiningDocument);
            }
        }

        /// <summary>
        /// Delete the JoiningDocument and remove from the list
        /// </summary>
        /// <param name="joiningDocument"></param>
        public void DeleteExist(JoiningDocument joiningDocument)
        {
            if (joiningDocument.Delete())
            {
                this.Remove(joiningDocument);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
