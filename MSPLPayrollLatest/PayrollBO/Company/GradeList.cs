// -----------------------------------------------------------------------
// <copyright file="GradeList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GradeList : List<Grade>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public GradeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="comapnyId"></param>
        public GradeList(int comapnyId)
        {
            this.CompanyId = comapnyId;
            Grade grade = new Grade();
            DataTable dtValue = grade.GetTableValues(this.CompanyId, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Grade gradeTemp = new Grade();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        gradeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        gradeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    gradeTemp.GradeName = Convert.ToString(dtValue.Rows[rowcount]["GradeName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        gradeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        gradeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        gradeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        gradeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        gradeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(gradeTemp);

                }
            }
        }


        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Grade and add to the list
        /// </summary>
        /// <param name="grade"></param>
        public void AddNew(Grade grade)
        {
            if (grade.Save())
            {
                this.Add(grade);
            }
        }

        /// <summary>
        /// Delete the EsiLocation and remove from the list
        /// </summary>
        /// <param name="grade"></param>
        public void DeleteExist(Grade grade)
        {
            if (grade.Delete())
            {
                this.Remove(grade);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
