using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace PayrollBO
{
    public class EntityTempDataList : List<EntityTempData>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityTempDataList()
        {

        }
        #endregion
        public EntityTempDataList(Guid financeyearId,Guid employeeId,int companyId)
        {
            EntityTempData entityTemp = new EntityTempData();
            entityTemp.financeyearId = financeyearId;
            entityTemp.EmployeeId = employeeId;
            entityTemp.companyId = companyId;
            this.EmployeeId = employeeId;
            this.companyId = companyId;
            DataTable dtValue = entityTemp.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityTempData entityTemp1 = new EntityTempData();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        entityTemp1.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        this.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                        entityTemp1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompId"])))
                        entityTemp1.CompId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CompId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        entityTemp1.Value = Convert.ToDecimal(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Effectivedate"])))
                        entityTemp1.Effectivedate = Convert.ToDateTime(dtValue.Rows[rowcount]["Effectivedate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Applymmyy"])))
                        entityTemp1.Applymmyy = Convert.ToChar(dtValue.Rows[rowcount]["Applymmyy"]);

                    this.Add(entityTemp1);
                }
            }
        }


        public EntityTempDataList(int companyId)
        {
            EntityTempData entityTemp = new EntityTempData();
            entityTemp.companyId = companyId;
            entityTemp.EmployeeId = Guid.Empty;
            this.companyId = companyId;
            this.EmployeeId = Guid.Empty;
            DataTable dtValue = entityTemp.GetTableValues();
            
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityTempData entityTemp1 = new EntityTempData();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        entityTemp1.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        this.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                        entityTemp1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompId"])))
                        entityTemp1.CompId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CompId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                        entityTemp1.Value = Convert.ToDecimal(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Effectivedate"])))
                        entityTemp1.Effectivedate = Convert.ToDateTime(dtValue.Rows[rowcount]["Effectivedate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Applymmyy"])))
                        entityTemp1.Applymmyy = Convert.ToChar(dtValue.Rows[rowcount]["Applymmyy"]);

                    this.Add(entityTemp1);
                }
            }
        }



        public AttributeModelList AttributeModelList { get; set; }
        public Employee Employee { get; set; }

        public int companyId { get; set; }

        public Guid financeyearId { get; set; }
        public  DateTime Effectivedate { get; set; }
        public char Applymmyy { get; set; }
        public Guid EmployeeId { get; set; }

        public Guid CompId { get; set; }
        public decimal Value { get; set; }
        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax txEmployeeSection and add to the list
        /// </summary>
        public void AddNew(EntityTempData entityTemp)
        {
            if (entityTemp.Save())
            {
                this.Add(entityTemp);
            }
        }

        #endregion

    }

    public class JsonEmpTempDataList
    {
        public EntityTempDataList entTemp;
        public Employee emp;
        public string EntityId;
        public string EntityModelId;
        public AttributeModelList AttributeModels;
        public EntityMasterValueList entityMasterValues;
    }
}
