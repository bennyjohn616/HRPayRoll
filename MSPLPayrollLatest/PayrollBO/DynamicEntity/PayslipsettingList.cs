using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PayslipsettingList : List<PaySlipSetting>
    {

        public PayslipsettingList()
        {

        }

        public PayslipsettingList(Guid configurationId)
        {

            PaySlipSetting setting = new PaySlipSetting();
            DataTable dtValue = setting.GetTableValues(configurationId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PaySlipSetting PaySlipsettinTemp = new PaySlipSetting();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"])))
                        PaySlipsettinTemp.ConfigurationId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"]));
                    PaySlipsettinTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PaySlipsettinTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    PaySlipsettinTemp.Logo = Convert.ToString(dtValue.Rows[rowcount]["Logo"]);
                    PaySlipsettinTemp.Title = Convert.ToString(dtValue.Rows[rowcount]["Title"]);
                    PaySlipsettinTemp.String1 = Convert.ToString(dtValue.Rows[rowcount]["String1"]);
                    PaySlipsettinTemp.String2 = Convert.ToString(dtValue.Rows[rowcount]["String2"]);
                    PaySlipsettinTemp.FooterString1 = Convert.ToString(dtValue.Rows[rowcount]["FooterString1"]);
                    PaySlipsettinTemp.FooterString2 = Convert.ToString(dtValue.Rows[rowcount]["FooterString2"]);
                    PaySlipsettinTemp.Header = Convert.ToString(dtValue.Rows[rowcount]["Header"]);
                    PaySlipsettinTemp.Footer = Convert.ToString(dtValue.Rows[rowcount]["Footer"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayCumulative"])))
                        PaySlipsettinTemp.DisplayCumulative = Convert.ToBoolean(dtValue.Rows[rowcount]["DisplayCumulative"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CumulativeMonth"])))
                        PaySlipsettinTemp.CumulativeMonth = Convert.ToInt32(dtValue.Rows[rowcount]["CumulativeMonth"]);
                    PaySlipsettinTemp.String1 = Convert.ToString(dtValue.Rows[rowcount]["Header"]);
                    PaySlipsettinTemp.String2 = Convert.ToString(dtValue.Rows[rowcount]["Footer"]);
                    PaySlipsettinTemp.Matchingtype = Convert.ToString(dtValue.Rows[rowcount]["Matchingtype"]);
                    this.Add(PaySlipsettinTemp);
                }
            }
        }

        public PayslipsettingList(int CompanyId)
        {
            
            PaySlipSetting setting = new PaySlipSetting();
            DataTable dtValue = setting.GetTableValues1(CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PaySlipSetting PaySlipsettinTemp = new PaySlipSetting();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"])))
                        PaySlipsettinTemp.ConfigurationId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        PaySlipsettinTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    PaySlipsettinTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    PaySlipsettinTemp.FieldName = Convert.ToString(dtValue.Rows[rowcount]["FieldName"]);
                    PaySlipsettinTemp.Type = Convert.ToString(dtValue.Rows[rowcount]["Type"]);
                    this.Add(PaySlipsettinTemp);
                }
            }
        }


        public PayslipsettingList(Guid configurationId, int CompanyId)
        {

            PaySlipSetting setting = new PaySlipSetting();
            DataTable dtValue = setting.GetTableValues(configurationId, CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PaySlipSetting PaySlipsettinTemp = new PaySlipSetting();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        PaySlipsettinTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    this.Add(PaySlipsettinTemp);
                }
            }
        }
        


    }
}
