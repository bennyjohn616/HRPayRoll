using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO.Settings
{
   public class PremiumSettingList: List<PremiumSetting>
    {
        public PremiumSettingList()
            {
            }
        public PremiumSettingList(int companyId,string Type, Guid Component)
        {
            PremiumSetting premiumSetting = new PremiumSetting();
            DataTable dtValue = premiumSetting.GetTableValues(companyId,Type);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PremiumSetting premiumSettingtemp = new PremiumSetting();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        premiumSettingtemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        premiumSettingtemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Component"])))
                        premiumSettingtemp.Component = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Component"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BackMonth"])))
                        premiumSettingtemp.BackMonth = Convert.ToInt32(dtValue.Rows[rowcount]["BackMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Type"])))
                        premiumSettingtemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        premiumSettingtemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        premiumSettingtemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        premiumSettingtemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        premiumSettingtemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        premiumSettingtemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(premiumSettingtemp);
                }
            }

        }
    }

}
