using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class AttributeModelBehaviorList : List<AttributeModelBehavior>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModelBehaviorList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public AttributeModelBehaviorList(Guid categoryid, int companyId)
        {
            this.CompanyId = companyId;
            this.CategoryId = categoryid;
            AttributeModelBehavior attributeModelBehavior = new AttributeModelBehavior();
            DataTable dtValue = attributeModelBehavior.GetTableValues(Guid.Empty, categoryid, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModelBehavior attributeModelBehaviorTemp = new AttributeModelBehavior();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttrubuteModelId"])))
                        attributeModelBehaviorTemp.AttrubuteModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttrubuteModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        attributeModelBehaviorTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        attributeModelBehaviorTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ValueType"])))
                        attributeModelBehaviorTemp.ValueType = Convert.ToInt32(dtValue.Rows[rowcount]["ValueType"]);
                    attributeModelBehaviorTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Percentage"])))
                        attributeModelBehaviorTemp.Percentage = Convert.ToDecimal(dtValue.Rows[rowcount]["Percentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Maximum"])))
                        attributeModelBehaviorTemp.Maximum = Convert.ToDecimal(dtValue.Rows[rowcount]["Maximum"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RoundingId"])))
                        attributeModelBehaviorTemp.RoundingId = Convert.ToInt32(dtValue.Rows[rowcount]["RoundingId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelBehaviorTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelBehaviorTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelBehaviorTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelBehaviorTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        attributeModelBehaviorTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelBehaviorTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(attributeModelBehaviorTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public AttributeModelBehaviorList(int companyId)
        {
            this.CompanyId = companyId;
            //this.CategoryId = categoryid;
            AttributeModelBehavior attributeModelBehavior = new AttributeModelBehavior();
            DataTable dtValue = attributeModelBehavior.GetTableValues(Guid.Empty, Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AttributeModelBehavior attributeModelBehaviorTemp = new AttributeModelBehavior();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttrubuteModelId"])))
                        attributeModelBehaviorTemp.AttrubuteModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttrubuteModelId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                    //    attributeModelBehaviorTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                    //    attributeModelBehaviorTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ValueType"])))
                    //    attributeModelBehaviorTemp.ValueType = Convert.ToInt32(dtValue.Rows[rowcount]["ValueType"]);
                    attributeModelBehaviorTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Percentage"])))
                        attributeModelBehaviorTemp.Percentage = Convert.ToDecimal(dtValue.Rows[rowcount]["Percentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Maximum"])))
                        attributeModelBehaviorTemp.Maximum = Convert.ToDecimal(dtValue.Rows[rowcount]["Maximum"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RoundingId"])))
                        attributeModelBehaviorTemp.RoundingId = Convert.ToInt32(dtValue.Rows[rowcount]["RoundingId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        attributeModelBehaviorTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        attributeModelBehaviorTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        attributeModelBehaviorTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        attributeModelBehaviorTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        attributeModelBehaviorTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        attributeModelBehaviorTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(attributeModelBehaviorTemp);
                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// get or set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="attributeModelBehavior"></param>
        public void AddNew(AttributeModelBehavior attributeModelBehavior)
        {
            if (attributeModelBehavior.Save())
            {
                this.Add(attributeModelBehavior);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="attributeModelBehavior"></param>
        public void DeleteExist(AttributeModelBehavior attributeModelBehavior)
        {
            if (attributeModelBehavior.Delete())
            {
                this.Remove(attributeModelBehavior);
            }
        }

        public bool ValidateFormula(string expression, int companyId, Guid categoryId)
        {

            return true;
        }
        public void Validate(string input, AttributeModelBehaviorList lstCollection, string lhs, ref string error, out string values)
        {
            values = string.Empty;
            if (!string.IsNullOrEmpty(error))
                return;
            if (!object.ReferenceEquals(input, null))
            {
                string temp = input;
                if (temp.IndexOf('{') >= 0)
                {
                    string id = temp.Substring(temp.IndexOf('{') + 1, 1);
                    var colTemp = lstCollection.Where(u => u.AttrubuteModelId == Guid.Parse(id)).FirstOrDefault();
                    if (lhs == id)
                    {
                        error = "Error";
                        return;
                    }
                    string replacevalue = "{" + id + "}";
                    if (!object.ReferenceEquals(colTemp, null))
                    {
                        if (colTemp.Formula.IndexOf('{') >= 0)
                        {
                            input = input.Replace(replacevalue, colTemp.Formula);
                            Validate(input, lstCollection, lhs, ref error, out values);
                        }
                        else
                        {
                            input = input.Replace(replacevalue, colTemp.Formula);
                            string outval = input;
                            values = input;
                        }
                    }

                }
                else
                {
                    values = input;
                }
            }


        }

        #endregion

        #region private methods




        #endregion

    }
}
