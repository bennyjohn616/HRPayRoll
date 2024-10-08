using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class OtherExamptionList : List<OtherExamption>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public OtherExamptionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public OtherExamptionList( string type)
        {
            OtherExamption otherExamption = new OtherExamption();

            DataTable dtValue = otherExamption.GetTableValues( type);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    OtherExamption OtherExamptionTemp = new OtherExamption();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        OtherExamptionTemp.Id = (Convert.ToInt32(dtValue.Rows[rowcount]["Id"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                    //    OtherExamptionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        OtherExamptionTemp.Value = Convert.ToDecimal(dtValue.Rows[rowcount]["Value"]);
                    OtherExamptionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    OtherExamptionTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        OtherExamptionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        OtherExamptionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        OtherExamptionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        OtherExamptionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        OtherExamptionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(OtherExamptionTemp);
                }

            }
            else
            {
                if (type == "HRA")
                {
                    this.Add(new OtherExamption
                    {
                        Id = 0,
                        Name = "Actual Rent Paid",

                    });
                    this.Add(new OtherExamption
                    {
                        Id = 0,
                        Name = "Metro",

                    });
                    this.Add(new OtherExamption
                    {
                        Id = 0,
                        Name = "Non Metro",

                    });

                }
            }
        }
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public OtherExamptionList(Guid financeYearId)
        {
            OtherExamption otherExamption = new OtherExamption();

            DataTable dtValue = otherExamption.GetTableValues(string.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    OtherExamption OtherExamptionTemp = new OtherExamption();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        OtherExamptionTemp.Id = (Convert.ToInt32(dtValue.Rows[rowcount]["Id"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"])))
                    //    OtherExamptionTemp.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        OtherExamptionTemp.Value = Convert.ToDecimal(dtValue.Rows[rowcount]["Value"]);
                    OtherExamptionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    OtherExamptionTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        OtherExamptionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        OtherExamptionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        OtherExamptionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        OtherExamptionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        OtherExamptionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(OtherExamptionTemp);
                }

            }
            else
            {
               
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
        /// Save the Tax OtherExamption and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(OtherExamption OtherExamption)
        {
            if (OtherExamption.Save())
            {
                this.Add(OtherExamption);
            }
        }

        /// <summary>
        /// delete the tax OtherExamption data
        /// </summary>
        /// <param name="OtherExamption"></param>

        //public void DeleteExist(OtherExamption OtherExamption)
        //{
        //    if (OtherExamption.Delete())
        //    {
        //        this.Remove(OtherExamption);
        //    }
        //}


        #endregion
    }
}
