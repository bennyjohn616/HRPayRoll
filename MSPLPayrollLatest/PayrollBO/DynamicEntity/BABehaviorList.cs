using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class BABehaviorList : List<BABehavior>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public BABehaviorList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public BABehaviorList(Guid entityId, Guid entityModelId)
        {
            this.EntityModelId = entityModelId;
            this.EntityId = entityId;
            BABehavior attributeModelBehavior = new BABehavior();
            DataTable dtValue = attributeModelBehavior.GetTableValues(Guid.Empty, entityModelId, entityId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    BABehavior attributeModelBehaviorTemp = new BABehavior();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        attributeModelBehaviorTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        attributeModelBehaviorTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        attributeModelBehaviorTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ValueType"])))
                        attributeModelBehaviorTemp.ValueType = Convert.ToInt32(dtValue.Rows[rowcount]["ValueType"]);
                    attributeModelBehaviorTemp.Formula = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Percentage"])))
                        attributeModelBehaviorTemp.Percentage = Convert.ToString(dtValue.Rows[rowcount]["Percentage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Maximum"])))
                        attributeModelBehaviorTemp.Maximum = Convert.ToString(dtValue.Rows[rowcount]["Maximum"]);
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
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ArrearAttributeModelId"])))
                        attributeModelBehaviorTemp.ArrearAttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ArrearAttributeModelId"]));
                    attributeModelBehaviorTemp.ArrearFormula = Convert.ToString(dtValue.Rows[rowcount]["ArrearFormula"]);
                    attributeModelBehaviorTemp.EligibiltyFormula = Convert.ToString(dtValue.Rows[rowcount]["EligibiltyFormula"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"])))
                        attributeModelBehaviorTemp.BaseFormula = Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"]);
                    attributeModelBehaviorTemp.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["BaseValue"]);
                    attributeModelBehaviorTemp.CompType = Convert.ToString(dtValue.Rows[rowcount]["CompType"]);


                    this.Add(attributeModelBehaviorTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// get or set the CategoryId
        /// </summary>
        public Guid EntityId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="attributeModelBehavior"></param>
        public void AddNew(BABehavior attributeModelBehavior)
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
        public void DeleteExist(BABehavior attributeModelBehavior)
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
        public void Validate(string input, BABehaviorList lstCollection, string lhs, ref string error, out string values)
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
                    var colTemp = lstCollection.Where(u => u.EntityId == Guid.Parse(id)).FirstOrDefault();
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
