using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class SettingValueList : List<SettingValue>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public SettingValueList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public SettingValueList(int settingId)
        {
            this.SettingId = settingId;
            SettingValue settingValue = new SettingValue();
            DataTable dtValue = settingValue.GetTableValues(this.SettingId, 0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    SettingValue settingValueTemp = new SettingValue();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SettingId"])))
                        settingValueTemp.SettingId = Convert.ToInt32(dtValue.Rows[rowcount]["SettingId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SettingDefinitionId"])))
                        settingValueTemp.SettingDefinitionId = Convert.ToInt32(dtValue.Rows[rowcount]["SettingDefinitionId"]);
                    settingValueTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        settingValueTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        settingValueTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        settingValueTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        settingValueTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        settingValueTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(settingValueTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int SettingId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="settingValue"></param>
        public void AddNew(SettingValue settingValue)
        {
            if (settingValue.Save())
            {
                this.Add(settingValue);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="settingValue"></param>
        public void DeleteExist(SettingValue settingValue)
        {
            if (settingValue.Delete())
            {
                this.Remove(settingValue);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
