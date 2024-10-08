// -----------------------------------------------------------------------
// <copyright file="TXFinanceYear.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;   
    using System.Data.OleDb;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    using TraceError;

    /// <summary>
    /// To handle the TXFinanceYear
    /// </summary>
    public class TXFinanceYear
    {

        #region private variable

        private OtherExamptionList _otherExmaption;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXFinanceYear()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXFinanceYear(Guid id, int companyId,bool isactive=false)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, companyId, isactive);
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


        #endregion

        #region property


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

        public OtherExamptionList OtherExemptionList
        {
            get
            {
                if (object.ReferenceEquals(_otherExmaption, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _otherExmaption = new OtherExamptionList(this.Id);
                    }
                    else
                        _otherExmaption = new OtherExamptionList();
                }
                return _otherExmaption;

            }
            set
            {
                _otherExmaption = value;
            }
        }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXFinanceYear
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXFinanceYear_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@StartingDate", this.StartingDate);
            sqlCommand.Parameters.AddWithValue("@EndingDate", this.EndingDate);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@TanNo", this.TanNo);
            sqlCommand.Parameters.AddWithValue("@TDSCircle", this.TDSCircle);
            sqlCommand.Parameters.AddWithValue("@PANorGIRNO", this.PANorGIRNO);
            sqlCommand.Parameters.AddWithValue("@TaxDeuctionAcNo", this.TaxDeuctionAcNo);
            sqlCommand.Parameters.AddWithValue("@InchargeEmployeeId", this.InchargeEmployeeId);
            sqlCommand.Parameters.AddWithValue("@Place", this.Place);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        /// Delete the TXFinanceYear
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXFinanceYear_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXFinanceYear
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId,bool isactive)
        {

            SqlCommand sqlCommand = new SqlCommand("TXFinanceYear_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@IsActive", isactive);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable getTdsPreviousEmployer(Guid sedId,DateTime sdate,DateTime edate)
        {
            SqlCommand sqlCommand = new SqlCommand("TDSPreviousEmployer_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@sectionId", sedId);
            sqlCommand.Parameters.AddWithValue("@sdate", sdate);
            sqlCommand.Parameters.AddWithValue("@edate", edate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion

    }
}

