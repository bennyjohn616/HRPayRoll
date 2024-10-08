using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class SettingDefinitionList : List<SettingDefinition>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public SettingDefinitionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public SettingDefinitionList(int settingId, int companyId)
        {
            this.SettingId = settingId;
            SettingDefinition attributeModelBehavior = new SettingDefinition();
            DataTable dtValue = attributeModelBehavior.GetTableValues(0, this.SettingId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    SettingDefinition settingDefinitionTemp = new SettingDefinition();
                    settingDefinitionTemp.CompanyId = companyId;
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        settingDefinitionTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"])))
                        settingDefinitionTemp.ParentId = Convert.ToInt32(dtValue.Rows[rowcount]["ParentId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsUniqueConstraint"])))
                        settingDefinitionTemp.IsUniqueConstraint = Convert.ToBoolean(dtValue.Rows[rowcount]["IsUniqueConstraint"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SettingId"])))
                        settingDefinitionTemp.SettingId = Convert.ToInt32(dtValue.Rows[rowcount]["SettingId"]);
                    settingDefinitionTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    settingDefinitionTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    settingDefinitionTemp.ControlType = Convert.ToString(dtValue.Rows[rowcount]["ControlType"]);
                    settingDefinitionTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    settingDefinitionTemp.RefEntityModelId = Convert.ToString(dtValue.Rows[rowcount]["RefEntityModelId"]);
                    settingDefinitionTemp.RadioGroupName = Convert.ToString(dtValue.Rows[rowcount]["RadioGroupName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        settingDefinitionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        settingDefinitionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        settingDefinitionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        settingDefinitionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        settingDefinitionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        settingDefinitionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(settingDefinitionTemp);
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
        /// <param name="settingDefinition"></param>
        public void AddNew(SettingDefinition settingDefinition)
        {
            if (settingDefinition.Save())
            {
                this.Add(settingDefinition);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="settingDefinition"></param>
        public void DeleteExist(SettingDefinition settingDefinition)
        {
            if (settingDefinition.Delete())
            {
                this.Remove(settingDefinition);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
