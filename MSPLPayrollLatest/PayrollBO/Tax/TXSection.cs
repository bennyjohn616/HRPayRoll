// -----------------------------------------------------------------------
// <copyright file="TXSection.cs" company="Microsoft">
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
    /// To handle the TXSection
    /// </summary>
    public class TXSection
    {

        #region private variable

        private TXSection _parentSection;
        private OtherExamption _otherExamption;
        public string DeclaredValue;
        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public TXSection()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXSection(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, companyId, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = new Guid(Convert.ToString(dtValue.Rows[0]["ParentId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinancialYearId"])))
                    this.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinancialYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.Projection = Convert.ToString(dtValue.Rows[0]["Projection"]);
                this.Formula = Convert.ToString(dtValue.Rows[0]["Formula"]);
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                this.BaseFormula = Convert.ToString(dtValue.Rows[0]["BaseFormula"]);
                this.BaseValue = Convert.ToString(dtValue.Rows[0]["BaseValue"]);
                this.SectionType = Convert.ToString(dtValue.Rows[0]["SectionType"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FormulaType"])))
                    this.FormulaType = Convert.ToInt32(dtValue.Rows[0]["FormulaType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IncomeType"])))
                    this.IncomeTypeId = Convert.ToInt32(dtValue.Rows[0]["IncomeType"]);
                this.SectionType = Convert.ToString(dtValue.Rows[0]["SectionType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["OrderNo"])))
                    this.OrderNo = Convert.ToInt32(dtValue.Rows[0]["OrderNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Limit"])))
                    this.Limit = Convert.ToDecimal(dtValue.Rows[0]["Limit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ExemptionType"])))
                    this.ExemptionType = Convert.ToInt32(dtValue.Rows[0]["ExemptionType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsGrossDeductable"])))
                    this.IsGrossDeductable = Convert.ToBoolean(dtValue.Rows[0]["IsGrossDeductable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDocumentRequired"])))
                    this.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[0]["IsDocumentRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsApprovelRequired"])))
                    this.IsApprovelRequired = Convert.ToBoolean(dtValue.Rows[0]["IsApprovelRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MatchingComponent"])))
                    this.MatchingComponent = Convert.ToString(dtValue.Rows[0]["MatchingComponent"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Eligible"])))
                    this.IsDocumentRequired = Convert.ToBoolean(dtValue.Rows[0]["Eligible"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the ParentId
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Get or Set the FinancialYearId
        /// </summary>
        public Guid FinancialYearId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the OrderNo
        /// </summary>
        public int OrderNo { get; set; }

        public int ExecOrder { get; set; }

        /// <summary>
        /// Get or Set the Limit
        /// </summary>
        public Decimal Limit { get; set; }

        /// <summary>
        /// Get or Set the ExemptionType
        /// </summary>
        public int ExemptionType { get; set; }

        /// <summary>
        /// Get or Set the IsGrossDeductable
        /// </summary>
        public bool IsGrossDeductable { get; set; }

        /// <summary>
        /// Get or Set the IsDocumentRequired
        /// </summary>
        public bool IsDocumentRequired { get; set; }

        /// <summary>
        /// Get or Set the IsApprovelRequired
        /// </summary>
        public bool IsApprovelRequired { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        public string Projection { get; set; }
        public string Formula { get; set; }
        public string Value { get; set; }
        public int FormulaType { get; set; }
        public string BaseValue { get; set; }
        public string BaseFormula { get; set; }
        public int IncomeTypeId { get; set; }
        public string SectionType { get; set; }
        public double DisorderNo { get; set; }
        public string FormulaValue { get; set; }
        public string MatchingComponent { get; set; }

        public bool Eligible { get; set; }
        public TXSection ParentSection
        {
            get
            {
                if (this.ParentId != Guid.Empty)
                {
                    _parentSection = new TXSection(this.ParentId, this.CompanyId);
                }
                else
                {
                    _parentSection = new TXSection();
                }
                return _parentSection;
            }
            set { _parentSection = value; }
        }
        public OtherExamption IncomeType
        {
            get
            {
                if (this.IncomeTypeId != 0)
                {
                    _otherExamption = new OtherExamption(this.IncomeTypeId, "");
                }
                else
                {
                    _otherExamption = new OtherExamption();
                }
                return _otherExamption;
            }
            set { _otherExamption = value; }
        }

        public string IncomeTypeName { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the TXSection
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
          //  UpdateExistingValues(this.Id,this.CompanyId, this.ParentId,this.FinancialYearId);
            SqlCommand sqlCommand = new SqlCommand("TXSection_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ParentId", this.ParentId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", this.FinancialYearId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@OrderNo", this.OrderNo);
            sqlCommand.Parameters.AddWithValue("@Limit", this.Limit);
            sqlCommand.Parameters.AddWithValue("@ExemptionType", this.ExemptionType);
            sqlCommand.Parameters.AddWithValue("@Projection", this.Projection);
            sqlCommand.Parameters.AddWithValue("@SectionType", this.SectionType);
            sqlCommand.Parameters.AddWithValue("@IncomeType", this.IncomeTypeId);
            sqlCommand.Parameters.AddWithValue("@Formula", this.Value);
            sqlCommand.Parameters.AddWithValue("@IsGrossDeductable", this.IsGrossDeductable);
            sqlCommand.Parameters.AddWithValue("@IsDocumentRequired", this.IsDocumentRequired);
            sqlCommand.Parameters.AddWithValue("@IsApprovelRequired", this.IsApprovelRequired);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@FormulaType", this.FormulaType);
            sqlCommand.Parameters.AddWithValue("@Value", this.Formula);
            sqlCommand.Parameters.AddWithValue("@BaseValue", this.BaseValue);
            sqlCommand.Parameters.AddWithValue("@BaseFormula", this.BaseFormula);
            sqlCommand.Parameters.AddWithValue("@MatchingComponent", this.MatchingComponent);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.AddWithValue("@Eligible", this.Eligible);
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
        /// Delete the TXSection
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TXSection_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXSection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId, Guid parentId, Guid financeYearId)
        {

            SqlCommand sqlCommand = new SqlCommand("TXSection_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@ParentId", parentId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearId", financeYearId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        private void UpdateExistingValues(Guid id, int companyId, Guid parentId, Guid financeYearId)
        {
            DataTable dtValue = GetTableValues(id, companyId, parentId, financeYearId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    
                  //  if (string.IsNullOrEmpty(this.Projection))
                      //  this.Projection = Convert.ToString(dtValue.Rows[rowcount]["Projection"]);
                    //if (string.IsNullOrEmpty(this.Formula))
                    //    this.Formula = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    //if (string.IsNullOrEmpty(this.Value))
                    //    this.Value = Convert.ToString(dtValue.Rows[rowcount]["Formula"]);
                    //if (string.IsNullOrEmpty(this.BaseValue))
                    //    this.BaseValue = Convert.ToString(dtValue.Rows[rowcount]["BaseValue"]);
                    //if (string.IsNullOrEmpty(this.BaseFormula))
                    //    this.BaseFormula = Convert.ToString(dtValue.Rows[rowcount]["BaseFormula"]);
                    if (string.IsNullOrEmpty(this.SectionType))
                        this.SectionType = Convert.ToString(dtValue.Rows[rowcount]["SectionType"]);
                    if (string.IsNullOrEmpty(this.MatchingComponent))
                        this.MatchingComponent = Convert.ToString(dtValue.Rows[rowcount]["MatchingComponent"]);

                    if ((Convert.ToInt32(this.FormulaType))==0)
                        this.FormulaType = Convert.ToInt32(dtValue.Rows[rowcount]["FormulaType"]);
                    if (Convert.ToInt32(this.IncomeTypeId)==0)
                        this.IncomeTypeId = Convert.ToInt32(dtValue.Rows[rowcount]["IncomeType"]);
                    if ((Convert.ToInt32(this.OrderNo)==0))
                        this.OrderNo = Convert.ToInt32(dtValue.Rows[rowcount]["OrderNo"]);
                    if (Convert.ToDecimal(this.Limit)==0)
                        this.Limit = Convert.ToDecimal(dtValue.Rows[rowcount]["Limit"]);
                }

            }
        }

        #endregion

    }
}

