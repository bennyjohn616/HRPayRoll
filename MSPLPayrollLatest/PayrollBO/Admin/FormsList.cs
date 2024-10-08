using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class FormsList : List<Forms>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FormsList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="roleName"></param>
        /// 

            //Modified By Keerthika on 01/05/2017
        public FormsList(bool getAll)
        {
            if (!getAll)
                return;
            Forms form = new Forms();
            DataTable dtValue = form.GetTableValues(Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Forms formsTemp = new Forms();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        formsTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    formsTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    formsTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);  //----
                    //formsTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    //formsTemp.Module = Convert.ToString(dtValue.Rows[rowcount]["Module"]);
                 //   formsTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    this.Add(formsTemp);
                }
            }
        }


        #endregion

        #region property


       
        #endregion

        #region Public methods

        /// <summary>
        /// Save the FormRights and add to the list
        /// </summary>
        /// <param name="FormRights"></param>
        public void AddNew(Forms form)
        {
            if (form.Save())
            {
                this.Add(form);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="category"></param>
        public void DeleteExist(Forms form)
        {
            if (form.Delete())
            {
                this.Remove(form);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
