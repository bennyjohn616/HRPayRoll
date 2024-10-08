using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class PremiumSettingComponentList : List<PremiumSettingComponent>
    {

        public PremiumSettingComponentList()
        {

        }




        public PremiumSettingComponentList(int companyId, string Type, Guid CategoryId)
        {
            PremiumSettingComponent premiumSettingComponent = new PremiumSettingComponent();
            DataTable dtValue = premiumSettingComponent.GetTableValues(companyId,Type,CategoryId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PremiumSettingComponent premiumSettingComponenttemp = new PremiumSettingComponent();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        premiumSettingComponenttemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        premiumSettingComponenttemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Component"])))
                        premiumSettingComponenttemp.AttrId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Component"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        premiumSettingComponenttemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Type"])))
                        premiumSettingComponenttemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        premiumSettingComponenttemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        premiumSettingComponenttemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        premiumSettingComponenttemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        premiumSettingComponenttemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        premiumSettingComponenttemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(premiumSettingComponenttemp);
                }
            }

        }
    }
}
