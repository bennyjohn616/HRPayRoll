using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PaySlipAttributeList : List<PaySlipAttributes>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySlipAttributeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PaySlipAttributeList(Guid configurationId)
        {

            PaySlipAttributes PaySlipAttributes = new PaySlipAttributes();
            DataTable dtValue = PaySlipAttributes.GetTableValues(configurationId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PaySlipAttributes PaySlipAttributesTemp = new PaySlipAttributes();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PaySlipAttributesTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CofigurationId"])))
                        PaySlipAttributesTemp.CofigurationId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CofigurationId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        PaySlipAttributesTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    PaySlipAttributesTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    PaySlipAttributesTemp.FieldName = Convert.ToString(dtValue.Rows[rowcount]["FieldName"]);
                    PaySlipAttributesTemp.DisplayAs = !string.IsNullOrWhiteSpace(Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]))? Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]): Convert.ToString(dtValue.Rows[rowcount]["FieldName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HeaderDisplayOrder"])))
                        PaySlipAttributesTemp.HeaderDisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["HeaderDisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FooterDisplayOrder"])))
                        PaySlipAttributesTemp.FooterDisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["FooterDisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FandFHeaderDisplayOrder"])))
                        PaySlipAttributesTemp.FandFHeaderDisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["FandFHeaderDisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EarningDisplayOrder"])))
                        PaySlipAttributesTemp.EarningDisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["EarningDisplayOrder"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DeductionDisplayOrder"])))
                        PaySlipAttributesTemp.DeductionDisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["DeductionDisplayOrder"]);
                    PaySlipAttributesTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPhysicalTable"])))
                        PaySlipAttributesTemp.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPhysicalTable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MatchingId"])))
                        PaySlipAttributesTemp.MatchingId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["MatchingId"]));
                    this.Add(PaySlipAttributesTemp);
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
        /// Save the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public void AddNew(PaySlipAttributes PaySlipAttributes)
        {

            if (PaySlipAttributes.Save())
            {
                this.Add(PaySlipAttributes);
            }
        }

        /// <summary>
        /// Delete the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(PaySlipAttributes PaySlipAttributes)
        {
            //if (PaySlipAttributes.Delete())
            //{
            //    this.Remove(PaySlipAttributes);
            //}
        }


        #endregion

        #region private methods


        #endregion
    }
}
