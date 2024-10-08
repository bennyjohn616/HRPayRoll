using System;
using System.Collections.Generic;
using System.Data;

namespace PayrollBO
{
    public class VerifyFinYrList :  List<VerifyFinYr>
    {
        public VerifyFinYrList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public VerifyFinYrList(DateTime sdate,DateTime edate)
        {
            VerifyFinYr vfyear = new VerifyFinYr();
            DataTable dtValue = vfyear.GetTableValues(sdate,edate);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    VerifyFinYr vftemp = new VerifyFinYr();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        vftemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StartingDate"])))
                        vftemp.StartingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["StartingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EndingDate"])))
                        vftemp.EndingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EndingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        vftemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    vftemp.TanNo = Convert.ToString(dtValue.Rows[rowcount]["TanNo"]);
                    vftemp.TDSCircle = Convert.ToString(dtValue.Rows[rowcount]["TDSCircle"]);
                    vftemp.PANorGIRNO = Convert.ToString(dtValue.Rows[rowcount]["PANorGIRNO"]);
                    vftemp.TaxDeuctionAcNo = Convert.ToString(dtValue.Rows[rowcount]["TaxDeuctionAcNo"]);
                    vftemp.Place = Convert.ToString(dtValue.Rows[rowcount]["Place"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InchargeEmployeeId"])))
                        vftemp.InchargeEmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["InchargeEmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        vftemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        vftemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        vftemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        vftemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        vftemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        vftemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(vftemp);
                }

            }
        }

    }
}

