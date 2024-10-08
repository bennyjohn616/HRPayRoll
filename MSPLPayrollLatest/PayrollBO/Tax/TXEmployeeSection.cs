// -----------------------------------------------------------------------
// <copyright file="TXEmployeeSection.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
    public class TXEmployeeSection
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXEmployeeSection()
        {
            DeclaredValue = "0";
        }
        public TXEmployeeSection(Guid employeeId,DateTime effectiveDate)
        {
            this.EmployeeId = employeeId;
            this.EffectiveDate = effectiveDate;
            DataTable dtValue = this.GetDeclareValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SectionId"])))
                    this.SectionId = new Guid(Convert.ToString(dtValue.Rows[0]["SectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.DeclaredValue = Convert.ToString(dtValue.Rows[0]["DeclaredValue"]);
                this.ApprovedValue = Convert.ToString(dtValue.Rows[0]["ApprovedValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveDate"])))
                    this.EffectiveDate = (Convert.ToDateTime(dtValue.Rows[0]["EffectiveDate"]));
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDocument"])))
                    this.IsDocument = Convert.ToBoolean(dtValue.Rows[0]["IsDocument"]);
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
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXEmployeeSection(Guid id, Guid employeeId, Guid sectionId, DateTime effectiveDate,bool byProof)
        {
            this.Id = id;
            this.SectionId = sectionId;
            this.EmployeeId = employeeId;
            this.EffectiveDate = effectiveDate;
            this.Proof = byProof;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SectionId"])))
                    this.SectionId = new Guid(Convert.ToString(dtValue.Rows[0]["SectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.DeclaredValue = Convert.ToString(dtValue.Rows[0]["DeclaredValue"]);
                this.ApprovedValue = Convert.ToString(dtValue.Rows[0]["ApprovedValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveDate"])))
                    this.EffectiveDate = (Convert.ToDateTime(dtValue.Rows[0]["EffectiveDate"]));
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDocument"])))
                    this.IsDocument = Convert.ToBoolean(dtValue.Rows[0]["IsDocument"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["HasPan"])))
                    this.HasPan = Convert.ToBoolean(dtValue.Rows[0]["HasPan"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PanCardNumber"])))
                    this.PanNumber = Convert.ToString(dtValue.Rows[0]["PanCardNumber"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["HasDeclaration"])))
                    this.HasDeclaration = Convert.ToBoolean(dtValue.Rows[0]["HasDeclaration"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LandLordName"])))
                    this.LandLordName = Convert.ToString(dtValue.Rows[0]["LandLordName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LandLordAddress"])))
                    this.LandLordAddress = Convert.ToString(dtValue.Rows[0]["LandLordAddress"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TotalRent"])))
                    this.TotalRent = Convert.ToDecimal(dtValue.Rows[0]["TotalRent"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the SectionId
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the DeclaredValue
        /// </summary>
        public string DeclaredValue { get; set; }

        /// <summary>
        /// Get or Set the ApprovedValue
        /// </summary>
        public string ApprovedValue { get; set; }

        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Get or Set the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get or Set the IsDocument
        /// </summary>
        public bool IsDocument { get; set; }

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

        public bool Proof { get; set; }

        public bool? HasPan { get; set; }

        public string PanNumber { get; set; }

        public bool? HasDeclaration { get; set; }

        public string LandLordName { get; set; }

        public string LandLordAddress { get; set; }

        public decimal TotalRent { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXEmployeeSection
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSection_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@SectionId", this.SectionId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@DeclaredValue", this.DeclaredValue);
            sqlCommand.Parameters.AddWithValue("@ApprovedValue", this.ApprovedValue);


            if (this.EffectiveDate != DateTime.MinValue)
            {
                sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.EffectiveDate);
            }

            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@IsDocument", this.IsDocument);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@Proof", this.Proof);
            sqlCommand.Parameters.AddWithValue("@HasPan", this.HasPan);
            sqlCommand.Parameters.AddWithValue("@PanCardNumber", this.PanNumber);
            sqlCommand.Parameters.AddWithValue("@HasDeclaration", this.HasDeclaration);
            sqlCommand.Parameters.AddWithValue("@LandLordName", this.LandLordName);
            sqlCommand.Parameters.AddWithValue("@LandLordAddress", this.LandLordAddress);
            sqlCommand.Parameters.AddWithValue("@TotalRent", this.TotalRent);
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
        /// Delete the TXEmployeeSection
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSection_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXEmployeeSection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeSection_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@SectionId", this.SectionId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.EffectiveDate);
            sqlCommand.Parameters.AddWithValue("@Proof", this.Proof);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetDeclareValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TXEmployeeDeclareValues_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.EffectiveDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable GetDeclareValuesForReport(Guid fin, DateTime date, string scode, string ecpde, int cid)
        {

            SqlCommand sqlCommand = new SqlCommand("TaxSectiononeReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", cid);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", fin);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", date);
            sqlCommand.Parameters.AddWithValue("@sCode", scode);
            sqlCommand.Parameters.AddWithValue("@eCode", ecpde);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetSectionValuesForReport(Guid fin, DateTime date, string scode, string ecpde, int cid)
        {

            SqlCommand sqlCommand = new SqlCommand("TaxSectiontwoReport_Select");
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

