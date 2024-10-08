// -----------------------------------------------------------------------
// <copyright file="AttributeModel.cs" company="Microsoft">
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
    /// To handle the AttributeModel
    /// </summary>
    public class AttributeModel
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModel()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public AttributeModel(Guid id, int companyId)
        {
          
            DataTable dtValue = this.GetTableValues(companyId, id, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]));
                this.DataType = Convert.ToString(dtValue.Rows[0]["DataType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DataSize"])))
                    this.DataSize = Convert.ToInt32(dtValue.Rows[0]["DataSize"]);
                this.DefaultValue = Convert.ToString(dtValue.Rows[0]["DefaultValue"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsMandatory"])))
                    this.IsMandatory = Convert.ToBoolean(dtValue.Rows[0]["IsMandatory"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["OrderNumber"])))
                    this.OrderNumber = Convert.ToInt32(dtValue.Rows[0]["OrderNumber"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsTransaction"])))
                    this.IsTransaction = Convert.ToBoolean(dtValue.Rows[0]["IsTransaction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsFilter"])))
                    this.IsFilter = Convert.ToBoolean(dtValue.Rows[0]["IsFilter"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsIncludeForGrossPay"])))
                    this.IsIncludeForGrossPay = Convert.ToBoolean(dtValue.Rows[0]["IsIncludeForGrossPay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsMonthlyInput"])))
                    this.IsMonthlyInput = Convert.ToBoolean(dtValue.Rows[0]["IsMonthlyInput"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsTaxable"])))
                    this.IsTaxable = Convert.ToBoolean(dtValue.Rows[0]["IsTaxable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsIncrement"])))
                    this.IsIncrement = Convert.ToBoolean(dtValue.Rows[0]["IsIncrement"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsReimbursement"])))
                    this.IsReimbursement = Convert.ToBoolean(dtValue.Rows[0]["IsReimbursement"]);
                //if(!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FlexiPay"])))
                //    this.IsFlexiPay = Convert.ToBoolean(dtValue.Rows[0]["FlexiPay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FullAndFinalSettlement"])))
                    this.FullAndFinalSettlement = Convert.ToBoolean(dtValue.Rows[0]["FullAndFinalSettlement"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsInstallment"])))
                    this.IsInstallment = Convert.ToBoolean(dtValue.Rows[0]["IsInstallment"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDefault"])))
                    this.IsDefault = Convert.ToBoolean(dtValue.Rows[0]["IsDefault"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelTypeId"])))
                    this.AttributeModelTypeId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelTypeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BehaviorType"])))
                    this.BehaviorType = Convert.ToString(dtValue.Rows[0]["BehaviorType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsSetting"])))
                    this.IsSetting = Convert.ToBoolean(dtValue.Rows[0]["IsSetting"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = new Guid(Convert.ToString(dtValue.Rows[0]["ParentId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ContributionType"])))
                    this.ContributionType = Convert.ToInt32(dtValue.Rows[0]["ContributionType"]);

            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

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
        /// Get or Set the RefEntityModelId
        /// </summary>
        public Guid RefEntityModelId { get; set; }

        /// <summary>
        /// Get or Set the DataType
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Get or Set the DataSize
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// Get or Set the DefaultValue
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Get or Set the IsMandatory
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Get or Set the OrderNumber
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Get or Set the IsTransaction
        /// </summary>
        public bool IsTransaction { get; set; }

        /// <summary>
        /// Get or Set the IsFilter
        /// </summary>
        public bool IsFilter { get; set; }

        /// <summary>
        /// Get or Set the IsIncludeForGrossPay
        /// </summary>
        public bool IsIncludeForGrossPay { get; set; }

        /// <summary>
        /// Get or Set the IsMonthlyInput
        /// </summary>
        public bool IsMonthlyInput { get; set; }

        /// <summary>
        /// Get or Set the IsTaxable
        /// </summary>
        public bool IsTaxable { get; set; }

        /// <summary>
        /// Get or Set the IsIncrement
        /// </summary>
        public bool IsIncrement { get; set; }

        /// <summary>
        /// Get or Set the IsReimbursement
        /// </summary>
        public bool IsReimbursement { get; set; }


        public bool IsFlexiPay { get; set; }
        /// <summary>
        /// Get or Set the FullAndFinalSettlement
        /// </summary>
        public bool FullAndFinalSettlement { get; set; }

        /// <summary>
        /// Get or Set the IsInstallment
        /// </summary>
        public bool IsInstallment { get; set; }

        /// <summary>
        /// Get or Set the IsDefault
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// get or set the isSetting
        /// </summary>
        public bool IsSetting { get; set; }

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

        /// <summary>
        /// get or set the AttributeModelTypeId
        /// </summary>
        public Guid AttributeModelTypeId { get; set; }

        /// <summary>
        /// get or set the behavior type
        /// </summary>
        public string BehaviorType { get; set; }

        /// <summary>
        /// get or set the parent id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// get or set the ContributionType
        /// </summary>
        public int ContributionType { get; set; }

       // public string TaxDeductionMode { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the AttributeModel
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (this.ContributionType != 2)
            {
                this.ContributionType = 1;
                this.ParentId = Guid.Empty;
            }
            SqlCommand sqlCommand = new SqlCommand("AttributeModel_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@DataType", this.DataType);
            sqlCommand.Parameters.AddWithValue("@DataSize", this.DataSize);
            sqlCommand.Parameters.AddWithValue("@DefaultValue", this.DefaultValue);
            sqlCommand.Parameters.AddWithValue("@IsMandatory", this.IsMandatory);
            sqlCommand.Parameters.AddWithValue("@OrderNumber", this.OrderNumber);
            sqlCommand.Parameters.AddWithValue("@IsTransaction", this.IsTransaction);
            sqlCommand.Parameters.AddWithValue("@IsFilter", this.IsFilter);
            sqlCommand.Parameters.AddWithValue("@IsIncludeForGrossPay", this.IsIncludeForGrossPay);
            sqlCommand.Parameters.AddWithValue("@IsMonthlyInput", this.IsMonthlyInput);
            sqlCommand.Parameters.AddWithValue("@IsTaxable", this.IsTaxable);
            sqlCommand.Parameters.AddWithValue("@IsIncrement", this.IsIncrement);
            sqlCommand.Parameters.AddWithValue("@IsReimbursement", this.IsReimbursement);
           // sqlCommand.Parameters.AddWithValue("@IsFlexiPay", this.IsFlexiPay);
            sqlCommand.Parameters.AddWithValue("@FullAndFinalSettlement", this.FullAndFinalSettlement);
            sqlCommand.Parameters.AddWithValue("@IsInstallment", this.IsInstallment);
            sqlCommand.Parameters.AddWithValue("@AttributeModelTypeId", this.AttributeModelTypeId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@BehaviorType", this.BehaviorType);
            sqlCommand.Parameters.AddWithValue("@ParentId", this.ParentId);
            sqlCommand.Parameters.AddWithValue("@ContributionType", this.ContributionType);
            sqlCommand.Parameters.AddWithValue("@IsSetting", this.IsSetting);
            sqlCommand.Parameters.AddWithValue("@IsDefault", this.IsDefault);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
                //if (this.IsInstallment)//Loan
                //{
                //    LoanMaster loanmaster = new LoanMaster();
                //    loanmaster.AttributeModelId = this.Id;
                //    loanmaster.LoanCode = this.Name;
                //    loanmaster.LoanDesc = this.DisplayAs;
                //    loanmaster.IsInterest = false;
                //    loanmaster.InterestPercent = 0;
                //    loanmaster.CreatedBy = this.CreatedBy;
                //    loanmaster.CompanyId = this.CompanyId;
                //    loanmaster.Save();
                //}

                if (this.IsTaxable)
                {
                    TXFinanceYear finyr = new TXFinanceYear(Guid.Empty, this.CompanyId, true);
                   if (finyr != null)
                    {
                        Guid txFinYr = finyr != null ? finyr.Id : Guid.Empty;
                        IncomeMatching imatch = new IncomeMatching();
                        imatch.FinancialYearId = txFinYr;
                        imatch.AttributemodelId = this.Id;
                        imatch.Projection = false;
                        imatch.TaxDeductionMode = "Normal";
                        imatch.Save();
                    }
                    
                }
            }
            return status;
        }

        /// <summary>
        /// Delete the AttributeModel
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModel_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Bit).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.DeleteData(sqlCommand, out outValue, "@IdOut");
            var Delstatus = Convert.ToBoolean(outValue);
            //if (outValue=="false")
            //{
            //   // return base.BuildJson(false, 100, "There is some error while deleting the data.", false);
            //}
            //return dbOperation.DeleteData(sqlCommand,out outValue);
            return Delstatus;
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the AttributeModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId, Guid id, Guid attributeModelTypeId)
        {
            SqlCommand sqlCommand = new SqlCommand("AttributeModel_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelTypeId", attributeModelTypeId);
       
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid entityModelId)
        {
            SqlCommand sqlCommand = new SqlCommand("AttributeModel_Select_EntityModel");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
           
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(int CompanyId, Guid EntityModelId)
        {
            SqlCommand sqlCommand = new SqlCommand("EntityCom_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", EntityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

