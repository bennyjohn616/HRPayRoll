using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveOpenings : LeaveBase
    {
        #region "Constructor"

        public LeaveOpenings()
        {

        }
        public LeaveOpenings(Guid id)
        {

            this.Id = id;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {


                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYear"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveType"])))
                    this.LeaveType = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveType"]));
                this.Leave_Encash = Convert.ToString(dtValue.Rows[0]["Leave_Encash"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveCredit"])))
                    this.LeaveCredit = (Convert.ToDouble(dtValue.Rows[0]["LeaveCredit"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveUsed"])))
                    this.LeaveUsed = Convert.ToDecimal(dtValue.Rows[0]["LeaveUsed"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);


            }

        }

        public LeaveOpenings(Guid leaveType, int companyId,Guid DefaultFinancialid,string EmployeeCode)
        {            
            DataTable dtValue = this.GetTableValues(leaveType, companyId, DefaultFinancialid, EmployeeCode);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYear"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveType"])))
                    this.LeaveType = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveType"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveOpening"])))
                    this.LeaveOpening = Convert.ToDouble(dtValue.Rows[0]["LeaveOpening"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveCredit"])))
                    this.LeaveCredit = (Convert.ToDouble(dtValue.Rows[0]["LeaveCredit"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
            }

        }
        public LeaveOpenings(Guid employeeId,Guid leaveType)
        {
           
            this.EmployeeId = employeeId;
            this.FinanceYearId = this.CurentFinanceYear.Id;
            this.LeaveType = leaveType;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {


                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYear"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveType"])))
                    this.LeaveType = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveType"]));
                this.Leave_Encash = Convert.ToString(dtValue.Rows[0]["Leave_Encash"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveCredit"])))
                    this.LeaveCredit = (Convert.ToDouble(dtValue.Rows[0]["LeaveCredit"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveUsed"])))
                    this.LeaveUsed = Convert.ToDecimal(dtValue.Rows[0]["LeaveUsed"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);

            }

        }

        #endregion

        #region "Properties"
        public new Guid Id { get; set; }
        public Guid FinanceYearId { get; set; }
        public Guid EmployeeId { get; set; }
        public double LeaveOpening { get; set; }
        public double LeaveCredit { get; set; }
        public decimal LeaveUsed { get; set; }
        public Guid LeaveType { get; set; }
        public string Leave_Encash { get; set; }
        public new int CreatedBy { get; set; }
        public new int ModifiedBy { get; set; }

        #endregion

        #region "Public Methods"
        /// <summary>
        /// Save the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public new bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveOpenings_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LeaveOpening", this.LeaveOpening);
            sqlCommand.Parameters.AddWithValue("@LeaveCredit", this.LeaveCredit);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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


        public  bool SaveCompOffCredit()
        {
            SqlCommand sqlCommand = new SqlCommand("CompOffCredit_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LeaveCredit", this.LeaveCredit);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public new bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveOpenings_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveOpenings_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableValues(Guid leaveType, int companyId, Guid DefaultFinancialid, string EmployeeCode)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveOpeningsimport");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LeaveType", leaveType);
            sqlCommand.Parameters.AddWithValue("@Companyid", companyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", DefaultFinancialid);
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
            
        }
        #endregion

    }


    public class LeaveCreditProcess
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public DateTime LastCreditProcessDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public int CreatedBy { get; set; }

      

    }
}
