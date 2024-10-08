using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
   public  class UserCompanymappingList : List<UserCompanymapping>
    {

        public UserCompanymappingList()
        {

        }

        public UserCompanymappingList(int userid)
        {
            UserCompanymapping mapping = new UserCompanymapping();
            mapping.UserId = userid;
           
            DataTable dtValue = mapping.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {

                    UserCompanymapping TempMapping = new UserCompanymapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TempMapping.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["UserId"])))
                        TempMapping.UserId = Convert.ToInt32(dtValue.Rows[rowcount]["UserId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        TempMapping.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);

                    TempMapping.RightsOnValue = Convert.ToString(dtValue.Rows[rowcount]["RightsOnValue"]);
                    TempMapping.RightsOn = Convert.ToString(dtValue.Rows[rowcount]["RightsOn"]);
                    //   this.IsRights = Convert.ToString(dtValue.Rows[0]["IsRights"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        TempMapping.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        TempMapping.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        TempMapping.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        TempMapping.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(TempMapping);
                }
            }
        }

        public UserCompanymappingList(int CompanyId, Guid id)
        {
            UserCompanymapping mapping = new UserCompanymapping();
            DataTable dtValue = mapping.GetTableValues(CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {

                    UserCompanymapping TempMapping = new UserCompanymapping();
                        TempMapping.Displayas = Convert.ToString(dtValue.Rows[rowcount]["Displayas"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["UserId"])))
                        TempMapping.UserId = Convert.ToInt32(dtValue.Rows[rowcount]["UserId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TempMapping.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    TempMapping.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    TempMapping.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    this.Add(TempMapping);
                }
            }
        }

        public UserCompanymappingList(Guid id, int CompanyId)
        {
            UserCompanymapping mapping = new UserCompanymapping();
            DataTable dtValue = mapping.GetTableValues(Guid.Empty,CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {

                    UserCompanymapping TempMapping = new UserCompanymapping();
                    TempMapping.Displayas = Convert.ToString(dtValue.Rows[rowcount]["Displayas"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TempMapping.RoleId = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);                   
                    this.Add(TempMapping);
                }
            }
        }
        //--
        public UserCompanymappingList(int userid, int companyid)
        {
            UserCompanymapping mapping = new UserCompanymapping();
            mapping.CompanyId = companyid;

            DataTable dtValue = mapping.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {

                    UserCompanymapping TempMapping = new UserCompanymapping();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        TempMapping.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["UserId"])))
                        TempMapping.UserId = Convert.ToInt32(dtValue.Rows[rowcount]["UserId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        TempMapping.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);

                    TempMapping.RightsOnValue = Convert.ToString(dtValue.Rows[rowcount]["RightsOnValue"]);
                    TempMapping.RightsOn = Convert.ToString(dtValue.Rows[rowcount]["RightsOn"]);
                    //   this.IsRights = Convert.ToString(dtValue.Rows[0]["IsRights"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        TempMapping.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        TempMapping.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        TempMapping.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        TempMapping.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(TempMapping);
                }
            }
        }

     

    }
}
