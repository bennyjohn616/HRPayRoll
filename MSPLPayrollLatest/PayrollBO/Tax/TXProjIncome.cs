namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the TXEmployeeSection
    /// </summary>
    public class TXProjIncome
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXProjIncome()
        {
        }
        public TXProjIncome(Guid financeyear,Guid employeeId,int month,int year)
        {
            this.EmployeeId = employeeId;
            this.financeyear = financeyear;
            this.Month = month;
            this.Year = year;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    if (Convert.ToInt32(dtValue.Rows[rowcount]["MonthCol"]) == this.Month &&
                        Convert.ToInt32(dtValue.Rows[rowcount]["YearCol"]) == this.Year)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyear"])))
                            this.financeyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyear"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                            this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome1"])))
                            this.Income1 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome1"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome2"])))
                            this.Income2 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome2"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ProjIncome3"])))
                            this.Income3 = Convert.ToInt32(dtValue.Rows[rowcount]["ProjIncome3"]);
                    }
                }
            }
        }


        #endregion

        #region property

        public Guid Id { get; set; }
        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Month { get; set; }

        public int Year { get; set; }
        public Guid financeyear { get; set; }

        /// <summary>
        /// Get or Set the SectionId
        /// </summary>
        public Guid EmployeeId { get; set; }

        public double Income1 { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public double Income2 { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public double Income3 { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXProjIncome
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXProjIncome_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@financeyear", this.financeyear);
            sqlCommand.Parameters.AddWithValue("@month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ProjIncome1", this.Income1);
            sqlCommand.Parameters.AddWithValue("@ProjIncome2", this.Income2);
            sqlCommand.Parameters.AddWithValue("@ProjIncome3", this.Income3);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            var Dstatus = outValue;
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the TXEmployeeSection
        /// </summary>
        /// <returns></returns>

        #endregion

        #region private methods


        /// <summary>
        /// Select the TXEmployeeSection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXProjIncome_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@financeyear", this.financeyear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

