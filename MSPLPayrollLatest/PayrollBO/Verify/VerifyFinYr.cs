using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;
using TraceError;


namespace PayrollBO
{

    /// <summary>
    /// To handle the TXFinanceYear
    /// </summary>
    public class VerifyFinYr
    {


        /// <summary>
        /// initialize the object
        /// </summary>
        public VerifyFinYr()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public VerifyFinYr(DateTime sdate,DateTime edate)
        {
            DataTable dtValue = this.GetTableValues(sdate,edate);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StartingDate"])))
                    this.StartingDate = Convert.ToDateTime(dtValue.Rows[0]["StartingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EndingDate"])))
                    this.EndingDate = Convert.ToDateTime(dtValue.Rows[0]["EndingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.TanNo = Convert.ToString(dtValue.Rows[0]["TanNo"]);
                this.TDSCircle = Convert.ToString(dtValue.Rows[0]["TDSCircle"]);
                this.PANorGIRNO = Convert.ToString(dtValue.Rows[0]["PANorGIRNO"]);
                this.Place = Convert.ToString(dtValue.Rows[0]["Place"]);
                this.TaxDeuctionAcNo = Convert.ToString(dtValue.Rows[0]["TaxDeuctionAcNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["InchargeEmployeeId"])))
                    this.InchargeEmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["InchargeEmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }




        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the StartingDate
        /// </summary>
        public DateTime StartingDate { get; set; }

        /// <summary>
        /// Get or Set the EndingDate
        /// </summary>
        public DateTime EndingDate { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the TanNo
        /// </summary>
        public string TanNo { get; set; }

        /// <summary>
        /// Get or Set the TDSCircle
        /// </summary>
        public string TDSCircle { get; set; }

        /// <summary>
        /// Get or Set the PANorGIRNO
        /// </summary>
        public string PANorGIRNO { get; set; }

        /// <summary>
        /// Get or Set the TaxDeuctionAcNo
        /// </summary>
        public string TaxDeuctionAcNo { get; set; }

        /// <summary>
        /// Get or Set the InchargeEmployeeId
        /// </summary>
        public Guid InchargeEmployeeId { get; set; }

        public string Place { get; set; }
        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

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



        /// <summary>
        /// Save the TXFinanceYear
        /// </summary>
        /// <returns></returns>
        #region private methods


        /// <summary>
        /// Select the TXFinanceYear
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(DateTime sdate,DateTime edate)
        {

            SqlCommand sqlCommand = new SqlCommand("TXFinanceYear_SelectbyDate");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@sdate", sdate);
            sqlCommand.Parameters.AddWithValue("@edate", edate);
            VerifyDBOpeartion vdboperation = new VerifyDBOpeartion();
            return vdboperation.GetTableData(sqlCommand);
        }
        #endregion

    }
}

