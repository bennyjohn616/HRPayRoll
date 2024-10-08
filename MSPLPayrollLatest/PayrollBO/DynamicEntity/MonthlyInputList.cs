using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class MonthlyInputList : List<MonthlyInput>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public MonthlyInputList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public MonthlyInputList(Guid entityId)
        {
            this.EntityId = entityId;
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(entityId, Guid.Empty, DateTime.Now.Month, DateTime.Now.Year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }
        public MonthlyInputList(Guid entityId, Guid category, string last="lastentry")
        {
            this.EntityId = entityId;
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetSettingTableValues(entityId, category);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }
        public MonthlyInputList(Guid entityId, Guid emploeeId, int month, int year)
        {
            this.EntityId = entityId;
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(entityId, emploeeId, month, year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }



        public MonthlyInputList(Guid entityId, Guid entitymodelId, Guid employeeId)
        {
          //  this.EntityId = entityId;
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(entityId, entitymodelId, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }

        public MonthlyInputList(Guid entityId, int month, int year)
        {
            this.EntityId = entityId;
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(entityId, Guid.Empty, month, year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }


        /// <summary>
        /// Created By:sharmila
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>

        public MonthlyInputList(Guid AttributeModelId, Guid emploeeId)
        {
           
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(AttributeModelId, emploeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }
        public MonthlyInputList( int month, int year)
        {
          
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetTableValues(Guid.Empty, Guid.Empty, month, year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }
        }
        #endregion

        #region property

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid EntityId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the TableCategory
        /// </summary>
        /// <returns></returns>
        public void AddNew(MonthlyInput monthlyInput)
        {

            if (monthlyInput.Save())
            {
                this.Add(monthlyInput);
            }
        }

        /// <summary>
        /// Delete the TableCategory
        /// </summary>
        /// <returns></returns>
        public void DeleteExist(MonthlyInput monthlyInput)
        {
            if (monthlyInput.Delete())
            {
                this.Remove(monthlyInput);
            }
        }

        public MonthlyInputList  emplastEntryMonthInput(Guid entityId,Guid EntityModelId,Guid EmpId)
        {
            MonthlyInputList milst = new PayrollBO.MonthlyInputList();
            MonthlyInput monthlyInput = new MonthlyInput();
            DataTable dtValue = monthlyInput.GetEmpLastEntryMonthlyInputValues(entityId, EntityModelId, EmpId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    MonthlyInput monthlyInputTemp = new MonthlyInput();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        monthlyInputTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        monthlyInputTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityId"])))
                        monthlyInputTemp.EntityId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"])))
                        monthlyInputTemp.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EntityModelId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Month"])))
                        monthlyInputTemp.Month = Convert.ToInt32(dtValue.Rows[rowcount]["Month"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Year"])))
                        monthlyInputTemp.Year = Convert.ToInt32(dtValue.Rows[rowcount]["Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"])))
                        monthlyInputTemp.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeModelId"]));
                    monthlyInputTemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        monthlyInputTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(monthlyInputTemp);
                }
            }

            return this;
        }
        #endregion

        #region private methods


        #endregion
    }
}
