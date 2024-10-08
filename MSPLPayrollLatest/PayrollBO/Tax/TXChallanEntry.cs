using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace PayrollBO.Tax
{
    public class TXChallanEntry
    {
        public TXChallanEntry()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXChallanEntry(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYearId"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BankId"])))
                    this.bankId = new Guid(Convert.ToString(dtValue.Rows[0]["BankId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyDate"])))
                    this.ApplyDate = Convert.ToDateTime(dtValue.Rows[0]["ApplyDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ChallanDate"])))
                    this.challanDate = Convert.ToDateTime(dtValue.Rows[0]["ChallanDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ChallanNo"])))
                    this.challanNo = Convert.ToString(dtValue.Rows[0]["ChallanNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Cheque/DD"])))
                    this.checkdd = Convert.ToString(dtValue.Rows[0]["Cheque/DD"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BookEntry"])))
                    this.bookEntry = Convert.ToBoolean(dtValue.Rows[0]["BookEntry"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TaxAmount"])))
                    this.TaxAmount = Convert.ToDecimal(dtValue.Rows[0]["TaxAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.Createdby = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        public Guid Id { get; set; }
        public Guid FinanceYearId { get; set; }
        public DateTime ApplyDate { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime challanDate { get; set; }

        public string challanNo { get; set; }

        public Guid bankId { get; set; }

        public string checkdd { get; set; }

        public bool bookEntry { get; set; }

        public decimal TaxAmount { get; set; }
        public int Createdby { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string BSRCode { get; set; }

        public string Name { get; set; }
        public string EmployeeCode { get; set; }
        public bool Save(string Type="")
        {

            SqlCommand sqlCommand = new SqlCommand("TXChallanEntry_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@BankId", this.bankId);
            sqlCommand.Parameters.AddWithValue("@ChallanDate", this.challanDate);
            sqlCommand.Parameters.AddWithValue("@ChallanNo", this.challanNo);
            sqlCommand.Parameters.AddWithValue("@ChequeDD", this.checkdd);
            sqlCommand.Parameters.AddWithValue("@BookEntry", this.bookEntry);
            sqlCommand.Parameters.AddWithValue("@TaxAmount", this.TaxAmount);
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@BSRCode", this.BSRCode);
            sqlCommand.Parameters.AddWithValue("@Type", Type);
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
        public bool Delete(string Type="")
        {

            SqlCommand sqlCommand = new SqlCommand("TXChallanEntry_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") :this.ApplyDate);
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Type", Type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXChallanEntry_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            if (this.ApplyDate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }
}
