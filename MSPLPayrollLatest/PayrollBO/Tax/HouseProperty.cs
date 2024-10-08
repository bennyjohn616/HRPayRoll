// -----------------------------------------------------------------------
// <copyright file="HouseProperty.cs" company="Microsoft">
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
    /// To handle the HouseProperty
    /// </summary>
    public class HouseProperty
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public HouseProperty()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public HouseProperty(Guid EmployeeId, int PropertyId, Guid FinancialYear, int companyId, string EffectiveMonth, string EffectiveYear)
        {

            //DataTable dtValue = this.GetTableValues(companyId, this.Id, Guid.Empty, Guid.Empty);
            DataTable dtValue = this.GetTableValues(EmployeeId, companyId, PropertyId, TxSectionId, FinancialYear, EffectiveMonth, EffectiveYear);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TxSectionId"])))
                    this.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[0]["TxSectionId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfProperties"])))
                    this.NoOfProperties = Convert.ToInt32(Convert.ToString(dtValue.Rows[0]["NoOfProperties"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyId"])))
                    this.PropertyId = Convert.ToInt32(Convert.ToString(dtValue.Rows[0]["PropertyId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinancialYear"])))
                    this.FinancialYear = new Guid(Convert.ToString(dtValue.Rows[0]["FinancialYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyReference"])))
                    this.PropertyReference = Convert.ToInt32(Convert.ToString(dtValue.Rows[0]["PropertyReference"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyCount"])))
                    this.PropertyCount = Convert.ToString(dtValue.Rows[0]["PropertyCount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Property_OwnersName"])))
                    this.Property_OwnersName = (Convert.ToString(dtValue.Rows[0]["Property_OwnersName"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyAddress"])))
                    this.PropertyAddress = Convert.ToString(dtValue.Rows[0]["PropertyAddress"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyLoanAmount"])))
                    this.PropertyLoanAmount = Convert.ToDecimal(dtValue.Rows[0]["PropertyLoanAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PurposeOfLoan"])))
                    this.PurposeOfLoan = Convert.ToString(dtValue.Rows[0]["PurposeOfLoan"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateofSanctionofLoan"])))
                    this.DateofSanctionofLoan = Convert.ToDateTime(dtValue.Rows[0]["DateofSanctionofLoan"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyJointName"])))
                    this.PropertyJointName = Convert.ToInt32(dtValue.Rows[0]["PropertyJointName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyJointInterest"])))
                    this.PropertyJointInterest = Convert.ToString(dtValue.Rows[0]["PropertyJointInterest"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayableHousingLoanPerYear"])))
                    this.PayableHousingLoanPerYear = Convert.ToDecimal(dtValue.Rows[0]["PayableHousingLoanPerYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PreConstructionInterest"])))
                    this.PreConstructionInterest = Convert.ToDecimal(dtValue.Rows[0]["PreConstructionInterest"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TotalInterestOfYear"])))
                    this.TotalInterestOfYear = Convert.ToDecimal(dtValue.Rows[0]["TotalInterestOfYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Interest_RestrictedtoEmployee"])))
                    this.Interest_RestrictedtoEmployee = Convert.ToDecimal(dtValue.Rows[0]["Interest_RestrictedtoEmployee"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConstrutionIsCompleted"])))
                    this.ConstrutionIsCompleted = Convert.ToInt32(dtValue.Rows[0]["ConstrutionIsCompleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertySelfOccupied"])))
                    this.PropertySelfOccupied = Convert.ToString(dtValue.Rows[0]["PropertySelfOccupied"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["HousingLoanTakenBefore_01_04_1999"])))
                    this.HousingLoanTakenBefore_01_04_1999 = Convert.ToInt32(dtValue.Rows[0]["HousingLoanTakenBefore_01_04_1999"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["GrossRentalIncome_PA"])))
                    this.GrossRentalIncome_PA = Convert.ToDecimal(dtValue.Rows[0]["GrossRentalIncome_PA"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Municipal_Water_Sewerage_taxpaid"])))
                    this.Municipal_Water_Sewerage_taxpaid = Convert.ToDecimal(dtValue.Rows[0]["Municipal_Water_Sewerage_taxpaid"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["GrossRentalIncome"])))
                    this.GrossRentalIncome = Convert.ToDecimal(dtValue.Rows[0]["GrossRentalIncome"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LessMunicipalTaxes"])))
                    this.LessMunicipalTaxes = Convert.ToDecimal(dtValue.Rows[0]["LessMunicipalTaxes"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Balance"])))
                    this.Balance = Convert.ToDecimal(dtValue.Rows[0]["Balance"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LessStandardDeduction"])))
                    this.LessStandardDeduction = Convert.ToDecimal(dtValue.Rows[0]["LessStandardDeduction"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LessInterestOnHousingLoan"])))
                    this.LessInterestOnHousingLoan = Convert.ToDecimal(dtValue.Rows[0]["LessInterestOnHousingLoan"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["HousePropertyNetIncome"])))
                    this.HousePropertyNetIncome = Convert.ToDecimal(dtValue.Rows[0]["HousePropertyNetIncome"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsHRACompleted"])))
                    this.IsHRACompleted = Convert.ToBoolean(dtValue.Rows[0]["IsHRACompleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveMonth"])))
                    this.EffectiveMonth = Convert.ToString(dtValue.Rows[0]["EffectiveMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EffectiveYear"])))
                    this.EffectiveYear = Convert.ToString(dtValue.Rows[0]["EffectiveYear"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeCode"])))
                    this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PANNumber"])))
                    this.PANNumber = Convert.ToString(dtValue.Rows[0]["PANNumber"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyName"])))
                    this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeName"])))
                    this.EmployeeName = Convert.ToString(dtValue.Rows[0]["EmployeeName"]);
            }
        }


        #endregion

        #region property

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid TxSectionId { get; set; }
        public int NoOfProperties { get; set; }
        public int PropertyId { get; set; }
        public Guid FinancialYear { get; set; }
        public int CompanyId { get; set; }
        public int PropertyReference { get; set; }
        public string Property_OwnersName { get; set; }
        public string PropertyAddress { get; set; }
        public Decimal PropertyLoanAmount { get; set; }
        public string PurposeOfLoan { get; set; }
        public DateTime DateofSanctionofLoan { get; set; }
        public int PropertyJointName { get; set; }
        public string PropertyJointInterest { get; set; }
        public Decimal PayableHousingLoanPerYear { get; set; }
        public Decimal PreConstructionInterest { get; set; }
        public Decimal TotalInterestOfYear { get; set; }
        public Decimal Interest_RestrictedtoEmployee { get; set; }
        public int ConstrutionIsCompleted { get; set; }
        public string PropertySelfOccupied { get; set; }
        public int HousingLoanTakenBefore_01_04_1999 { get; set; }
        public Decimal GrossRentalIncome_PA { get; set; }
        public Decimal Municipal_Water_Sewerage_taxpaid { get; set; }
        public Decimal GrossRentalIncome { get; set; }
        public Decimal LessMunicipalTaxes { get; set; }
        public Decimal Balance { get; set; }
        public Decimal LessStandardDeduction { get; set; }
        public Decimal LessInterestOnHousingLoan { get; set; }
        public Decimal HousePropertyNetIncome { get; set; }
        public bool IsHRACompleted { get; set; }
        public string EffectiveMonth { get; set; }
        public string EffectiveYear { get; set; }

        public DateTime EffectiveDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public string LenderAddress { get; set; }
        public string LenderName { get; set; }
        public string LenderHRAAddress { get; set; }
        public string LenderPAN { get; set; }
        public int LenderType { get; set; }
        public string EmployeeCode { get; set; }
        public string PANNumber { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string Financial_Year { get; set; }
        public Guid SectionId { get; private set; }
        public string PropertyCount { get; set; }



        #endregion

        #region Public methods









        /// <summary>
        /// Save the HouseProperty
        /// </summary>
        /// <returns></returns>
        public bool Save(string Type)
        {
            SqlCommand sqlCommand = new SqlCommand("stp_SavePropertyIncome_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Type", Type);
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@TxSectionId", this.TxSectionId);
            sqlCommand.Parameters.AddWithValue("@NoOfProperties", this.NoOfProperties);
            sqlCommand.Parameters.AddWithValue("@PropertyId", this.PropertyId);
            sqlCommand.Parameters.AddWithValue("@FinancialYear", this.FinancialYear);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@PropertyReference", this.PropertyReference);
            sqlCommand.Parameters.AddWithValue("@Property_OwnersName", this.Property_OwnersName);
            sqlCommand.Parameters.AddWithValue("@PropertyAddress", this.PropertyAddress);
            sqlCommand.Parameters.AddWithValue("@PropertyLoanAmount", this.PropertyLoanAmount);
            sqlCommand.Parameters.AddWithValue("@PurposeOfLoan", this.PurposeOfLoan);
            sqlCommand.Parameters.AddWithValue("@DateofSanctionofLoan", this.DateofSanctionofLoan == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.DateofSanctionofLoan);
            sqlCommand.Parameters.AddWithValue("@PropertyJointName", this.PropertyJointName);
            sqlCommand.Parameters.AddWithValue("@PropertyJointInterest", this.PropertyJointInterest);
            sqlCommand.Parameters.AddWithValue("@PayableHousingLoanPerYear", this.PayableHousingLoanPerYear);
            sqlCommand.Parameters.AddWithValue("@PreConstructionInterest", this.PreConstructionInterest);
            sqlCommand.Parameters.AddWithValue("@TotalInterestOfYear", this.TotalInterestOfYear);
            sqlCommand.Parameters.AddWithValue("@Interest_RestrictedtoEmployee", this.Interest_RestrictedtoEmployee);
            sqlCommand.Parameters.AddWithValue("@ConstrutionIsCompleted", this.ConstrutionIsCompleted);
            sqlCommand.Parameters.AddWithValue("@PropertySelfOccupied", this.PropertySelfOccupied);
            sqlCommand.Parameters.AddWithValue("@HousingLoanTakenBefore_01_04_1999", this.HousingLoanTakenBefore_01_04_1999);
            sqlCommand.Parameters.AddWithValue("@GrossRentalIncome_PA", this.GrossRentalIncome_PA);
            sqlCommand.Parameters.AddWithValue("@Municipal_Water_Sewerage_taxpaid", this.Municipal_Water_Sewerage_taxpaid);
            sqlCommand.Parameters.AddWithValue("@GrossRentalIncome", this.GrossRentalIncome);
            sqlCommand.Parameters.AddWithValue("@LessMunicipalTaxes", this.LessMunicipalTaxes);
            sqlCommand.Parameters.AddWithValue("@Balance", this.Balance);
            sqlCommand.Parameters.AddWithValue("@LessStandardDeduction", this.LessStandardDeduction);
            sqlCommand.Parameters.AddWithValue("@LessInterestOnHousingLoan", this.LessInterestOnHousingLoan);
            sqlCommand.Parameters.AddWithValue("@HousePropertyNetIncome", this.HousePropertyNetIncome);
            sqlCommand.Parameters.AddWithValue("@IsHRACompleted", this.IsHRACompleted);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", this.EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", this.EffectiveYear);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            //sqlCommand.Parameters.AddWithValue("@LenderAddress", this.LenderAddress);
            sqlCommand.Parameters.AddWithValue("@LenderHRAAddress", this.LenderHRAAddress);
            sqlCommand.Parameters.AddWithValue("@LenderName", this.LenderName);
            sqlCommand.Parameters.AddWithValue("@LenderPAN", this.LenderPAN);
            sqlCommand.Parameters.AddWithValue("@LenderType", this.LenderType);


            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }

            UpdateHousePropertyDeclaredValue(); /// This method added on 04-10-2022 by K. Tamilvanan
            return status;
        }
        /// <summary>
        /// The below method is used to restrict house propert to 2lakhs.
        /// This method added on 04-10-2022 by K. Tamilvanan
        /// </summary>
        public void UpdateHousePropertyDeclaredValue()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("sp_UpdateHousePropertyDeclaredValue");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
                sqlCommand.Parameters.AddWithValue("@TxSectionId", this.TxSectionId);
                sqlCommand.Parameters.AddWithValue("@EffectiveMonth", this.EffectiveMonth);
                sqlCommand.Parameters.AddWithValue("@EffectiveYear", this.EffectiveYear);
                sqlCommand.Parameters.AddWithValue("@FinancialYear", this.FinancialYear);
                DBOperation dbOperation = new DBOperation();
                dbOperation.save(sqlCommand);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Delete the HouseProperty
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("stp_DeletePropertyIncome_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the HouseProperty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid EmployeeId, int companyId, int PropertyId, Guid TxSectionId, Guid FinancialYear, string EffectiveMonth, string EffectiveYear)
        {
            SqlCommand sqlCommand = new SqlCommand("stp_GetPropertyIncome_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@TxSectionId", TxSectionId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", FinancialYear);
            sqlCommand.Parameters.AddWithValue("@PropertyId", PropertyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid EmployeeId, int companyId, Guid FinancialYear)
        {
            SqlCommand sqlCommand = new SqlCommand("select_HRAHousePropertyIncome");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", FinancialYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(Guid EmployeeId, Guid FinancialYear, Guid TxSectionId, string EffectiveMonth , string EffectiveYear , DateTime EffectiveDate)
        {
            SqlCommand sqlCommand = new SqlCommand("stp_SubmitPropertyIncome_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYear", FinancialYear);
            sqlCommand.Parameters.AddWithValue("@SectionId", TxSectionId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", EffectiveDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand); 

        }
        protected internal DataTable GetTableValues(Guid financialyearId, int companyId, string EffectiveMonth , string EffectiveYear, string scode, string ecpde)
        {
            SqlCommand sqlCommand = new SqlCommand("HousePropertyReport");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYear", financialyearId);
            sqlCommand.Parameters.AddWithValue("@CompanyID", companyId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            sqlCommand.Parameters.AddWithValue("@eCode", ecpde);
            sqlCommand.Parameters.AddWithValue("@sCode", scode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid EmployeeId, Guid FinancialYear, Guid TxSectionId, string EffectiveMonth, string EffectiveYear)
        {
            SqlCommand sqlCommand = new SqlCommand("stp_GetAllPropertyIncome_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYear", FinancialYear);
            sqlCommand.Parameters.AddWithValue("@SectionId", TxSectionId);
            sqlCommand.Parameters.AddWithValue("@EffectiveMonth", EffectiveMonth);
            sqlCommand.Parameters.AddWithValue("@EffectiveYear", EffectiveYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);

        }



        #endregion

    }
}

