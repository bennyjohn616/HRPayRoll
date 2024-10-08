using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ManagerEligiblityList : List<ManagerEligiblity>
    {
        #region property

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ManagerEligiblityList()
        {

        }

        public ManagerEligiblityList(int CompanyId, Guid FinancialYear)
        {
            int RoleId = 0;
            ManagerEligiblity manager = new ManagerEligiblity();
            DataTable dtValue = manager.GetTableValues(CompanyId, RoleId, FinancialYear);
            if (dtValue.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ManagerEligiblity managertemp = new ManagerEligiblity();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        managertemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RoleId"])))
                        managertemp.RoleId = Convert.ToInt32(dtValue.Rows[rowcount]["RoleId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        managertemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinanacialYear"])))
                        managertemp.FinanacialYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanacialYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        managertemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        managertemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(managertemp);
                }
            }
        }



        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>



        #endregion


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        public String FieldName { get; set; }

        /// <summary>
        /// Get or Set the RoleId
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the TanNo
        /// </summary>
        public Guid FinanacialYear { get; set; }
        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }



        #endregion
     
                
    }
}
