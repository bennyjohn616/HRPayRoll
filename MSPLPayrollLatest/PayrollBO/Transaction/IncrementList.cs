using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class IncrementList : List<Increment>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public IncrementList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public IncrementList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            Increment increment = new Increment();
            DataTable dtValue = increment.GetTableValues(Guid.Empty, employeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Increment incrementTemp = new Increment();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        incrementTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        incrementTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffectiveDate"])))
                        incrementTemp.EffectiveDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EffectiveDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BeforeLop"])))
                        incrementTemp.BeforeLop = Convert.ToDouble(dtValue.Rows[rowcount]["BeforeLop"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AfterLop"])))
                        incrementTemp.AfterLop = Convert.ToDouble(dtValue.Rows[rowcount]["AfterLop"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyMonth"])))
                        incrementTemp.ApplyMonth = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyYear"])))
                        incrementTemp.ApplyYear = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        incrementTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        incrementTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        incrementTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        incrementTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        incrementTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsProcessed"])))
                        incrementTemp.IsProcessed = Convert.ToBoolean(dtValue.Rows[rowcount]["IsProcessed"]);
                    this.Add(incrementTemp);
                }

            }
        }


        public IncrementList(int month,int year)
        {
            Increment increment = new Increment();
            DataTable dtValue = increment.GetTableValues(month,year);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Increment incrementTemp = new Increment();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        incrementTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        incrementTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffectiveDate"])))
                        incrementTemp.EffectiveDate = Convert.ToDateTime(dtValue.Rows[rowcount]["EffectiveDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BeforeLop"])))
                        incrementTemp.BeforeLop = Convert.ToDouble(dtValue.Rows[rowcount]["BeforeLop"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AfterLop"])))
                        incrementTemp.AfterLop = Convert.ToDouble(dtValue.Rows[rowcount]["AfterLop"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyMonth"])))
                        incrementTemp.ApplyMonth = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApplyYear"])))
                        incrementTemp.ApplyYear = Convert.ToInt32(dtValue.Rows[rowcount]["ApplyYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        incrementTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        incrementTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        incrementTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        incrementTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        incrementTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsProcessed"])))
                        incrementTemp.IsProcessed = Convert.ToBoolean(dtValue.Rows[rowcount]["IsProcessed"]);
                    this.Add(incrementTemp);
                }

            }
        }


        #endregion

        #region property

        public Guid EmployeeId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the increment and add to the list
        /// </summary>
        /// <param name="increment"></param>
        public void AddNew(Increment increment)
        {
            if (increment.Save())
            {
                this.Add(increment);
            }
        }

        /// <summary>
        /// Delete the increment and remove from the list
        /// </summary>
        /// <param name="increment"></param>
        public void DeleteExist(Increment increment)
        {
            if (increment.Delete())
            {
                this.Remove(increment);
            }
        }

        #endregion

        #region private methods


        #endregion
    }
}
