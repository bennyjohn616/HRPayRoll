
namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    public class TXActualRentPaid
    {
        private Guid financeYearId;
        private Guid employeeId;
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXActualRentPaid()
        {

        }

        public TXActualRentPaid(Guid financeYearId, Guid employeeId)
        {
            this.financeYearId = financeYearId;
            this.employeeId = employeeId;
        }


        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        //public TXActualRentPaid(Guid FinanceYearId, Guid employeeId)
        //{
        //    TXActualRentPaid txActualRentPaid = new TXActualRentPaid();
        //    txActualRentPaid.FinanceYearId = FinanceYearId;
        //    txActualRentPaid.EmployeeId = employeeId;

        //    DataTable dtValue = txActualRentPaid.GetTableValues();
        //    if (dtValue.Rows.Count > 0)
        //    {
        //        for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
        //        {
        //            TXActualRentPaid txActualrentTemp = new TXActualRentPaid();

        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
        //                this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYearId"])))
        //                this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYearId"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
        //                this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
        //            this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
        //            this.MetroRent = Convert.ToDecimal(dtValue.Rows[0]["MetroRent"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
        //                this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
        //                this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
        //                this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
        //                this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
        //                this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
        //            this.Add(txActualrentTemp);
        //        }
        //    }
        //}


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the FinanceYearId
        /// </summary>
        public Guid FinanceYearId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or Set the MetroRent
        /// </summary>
        public decimal MetroRent { get; set; }

        public decimal NonMetroRent { get; set; }

        

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

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        
        public Guid TXEmployeeSectionId { get; set; }

        public Guid TxSecId { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXActualRentPaid
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ActualRentPaid_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@MetroRent", this.MetroRent);
            sqlCommand.Parameters.AddWithValue("@NonMetroRent", this.NonMetroRent);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@TXEmployeeSectionId", this.TXEmployeeSectionId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the TXActualRentPaid
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXActualRentPaid_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXActualRentPaid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("ActualRentPaid_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@TXEmployeeSectionId", this.TXEmployeeSectionId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public  DataTable GetTableValuesForReport(Guid fin ,DateTime date,string scode ,string ecpde,int cid)
        {

            SqlCommand sqlCommand = new SqlCommand("ActualRentPaidForReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", cid);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", fin);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", date);
            sqlCommand.Parameters.AddWithValue("@sCode", scode);
            sqlCommand.Parameters.AddWithValue("@eCode", ecpde);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion
    }
}


