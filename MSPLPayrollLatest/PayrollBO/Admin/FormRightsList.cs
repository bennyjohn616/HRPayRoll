using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class FormRightsList : List<FormRights>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FormRightsList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="roleName"></param>
        public FormRightsList(string roleName)
        {
            this.RoleName = roleName;
            FormRights formrights = new FormRights();
            DataTable dtValue = formrights.GetTableValues(Guid.Empty,roleName);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    FormRights formRightsTemp = new FormRights();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        formRightsTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormId"])))
                        formRightsTemp.FormId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FormId"]));
                    formRightsTemp.RoleName = Convert.ToString(dtValue.Rows[rowcount]["RoleName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ViewRights"])))
                        formRightsTemp.ViewRights = Convert.ToBoolean(dtValue.Rows[rowcount]["ViewRights"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EditRights"])))
                        formRightsTemp.EditRights = Convert.ToBoolean(dtValue.Rows[rowcount]["EditRights"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DeleteRights"])))
                        formRightsTemp.DeleteRights = Convert.ToBoolean(dtValue.Rows[rowcount]["DeleteRights"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        formRightsTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    this.Add(formRightsTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the RoleName
        /// </summary>
        public string RoleName { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the FormRights and add to the list
        /// </summary>
        /// <param name="FormRights"></param>
        public void AddNew(FormRights formRights)
        {
            if (formRights.Save())
            {
                this.Add(formRights);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="category"></param>
        public void DeleteExist(FormRights formRights)
        {
            if (formRights.Delete())
            {
                this.Remove(formRights);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
