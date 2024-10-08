using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class EntityTempData
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityTempData()
        {
        }
        public EntityTempData(Guid financeyearId,Guid employeeId,int companyId)
        {
            this.EmployeeId = employeeId;
            this.financeyearId = financeyearId;
            this.companyId = companyId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        this.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        this.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                     this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompId"])))
                       this.CompId =  new Guid((string)dtValue.Rows[rowcount]["CompId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Value"])))
                       this.Value = Convert.ToDecimal(dtValue.Rows[rowcount]["Value"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Effectivedate"])))
                        this.Effectivedate = Convert.ToDateTime(dtValue.Rows[rowcount]["Effectivedate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Applymmyy"])))
                        this.Applymmyy = Convert.ToChar(dtValue.Rows[rowcount]["Applymmyy"]);
                }
            }
        }


        #endregion

        #region property

        public Guid Id { get; set; }
        /// <summary>
        /// Get or Set the Id
        /// </summary>
        /// 
        public int companyId { get; set; }

        public Guid financeyearId { get; set; }

        public DateTime Effectivedate { get; set; }
        public char Applymmyy { get; set; }
        public Guid EmployeeId { get; set; }

        public Guid CompId { get; set; }
        public decimal Value { get; set; }

        #endregion
        #region Public methods


        /// <summary>
        /// Save the TXProjIncome
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityTempData_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", this.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompId", this.CompId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@Applymmyy", this.Applymmyy);
            sqlCommand.Parameters.AddWithValue("@Effectivedate", this.Effectivedate);
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

        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("EntityTempData_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", this.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
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

            SqlCommand sqlCommand = new SqlCommand("EntityTempData_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", this.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }

}
