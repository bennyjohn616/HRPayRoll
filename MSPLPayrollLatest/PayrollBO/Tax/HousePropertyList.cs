using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class HousePropertyList : List<HouseProperty>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public HousePropertyList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public HousePropertyList(Guid EmployeeId,int companyId, int PropertyId, Guid TxSectionId, Guid FinancialYear, string EffectiveMonth, string EffectiveYear)
        {
            this.CompanyId = companyId;
            HouseProperty houseProperty = new HouseProperty();
            DataTable dtValue = houseProperty.GetTableValues(EmployeeId, companyId , PropertyId, TxSectionId, FinancialYear, EffectiveMonth, EffectiveYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    HouseProperty housePropertyTemp = new HouseProperty();
                    housePropertyTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    housePropertyTemp.EmployeeName = Convert.ToString(dtValue.Rows[rowcount]["EmployeeName"]);
                    housePropertyTemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                    housePropertyTemp.PANNumber = Convert.ToString(dtValue.Rows[rowcount]["PANNumber"]);
                    housePropertyTemp.Financial_Year = Convert.ToString(dtValue.Rows[rowcount]["Financial_Year"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        housePropertyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        housePropertyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"])))
                        housePropertyTemp.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NoOfProperties"])))
                        housePropertyTemp.NoOfProperties = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfProperties"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PropertyCount"])))
                        housePropertyTemp.PropertyCount = Convert.ToString(dtValue.Rows[0]["PropertyCount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyId"])))
                        housePropertyTemp.PropertyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"])))
                        housePropertyTemp.FinancialYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyReference"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyReference"]);
                    housePropertyTemp.Property_OwnersName = Convert.ToString(dtValue.Rows[rowcount]["Property_OwnersName"]);
                    housePropertyTemp.PropertyAddress = Convert.ToString(dtValue.Rows[rowcount]["PropertyAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyLoanAmount"])))
                        housePropertyTemp.PropertyLoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PropertyLoanAmount"]);
                    housePropertyTemp.PurposeOfLoan = Convert.ToString(dtValue.Rows[rowcount]["PurposeOfLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofSanctionofLoan"])))
                        housePropertyTemp.DateofSanctionofLoan = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofSanctionofLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyJointName"])))
                        housePropertyTemp.PropertyJointName = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyJointName"]);
                    housePropertyTemp.PropertyJointInterest = Convert.ToString(dtValue.Rows[rowcount]["PropertyJointInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"])))
                        housePropertyTemp.PayableHousingLoanPerYear = Convert.ToDecimal(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionInterest"])))
                        housePropertyTemp.PreConstructionInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterestOfYear"])))
                        housePropertyTemp.TotalInterestOfYear = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterestOfYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"])))
                        housePropertyTemp.Interest_RestrictedtoEmployee = Convert.ToDecimal(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConstrutionIsCompleted"])))
                        housePropertyTemp.ConstrutionIsCompleted = Convert.ToInt32(dtValue.Rows[rowcount]["ConstrutionIsCompleted"]);
                    housePropertyTemp.PropertySelfOccupied = Convert.ToString(dtValue.Rows[rowcount]["PropertySelfOccupied"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"])))
                        housePropertyTemp.HousingLoanTakenBefore_01_04_1999 = Convert.ToInt32(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome_PA"])))
                        housePropertyTemp.GrossRentalIncome_PA = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome_PA"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"])))
                        housePropertyTemp.Municipal_Water_Sewerage_taxpaid = Convert.ToDecimal(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome"])))
                        housePropertyTemp.GrossRentalIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessMunicipalTaxes"])))
                        housePropertyTemp.LessMunicipalTaxes = Convert.ToDecimal(dtValue.Rows[rowcount]["LessMunicipalTaxes"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Balance"])))
                        housePropertyTemp.Balance = Convert.ToDecimal(dtValue.Rows[rowcount]["Balance"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessStandardDeduction"])))
                        housePropertyTemp.LessStandardDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["LessStandardDeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"])))
                        housePropertyTemp.LessInterestOnHousingLoan = Convert.ToDecimal(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousePropertyNetIncome"])))
                        housePropertyTemp.HousePropertyNetIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["HousePropertyNetIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHRACompleted"])))
                        housePropertyTemp.IsHRACompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHRACompleted"]);
                    housePropertyTemp.EffectiveMonth = Convert.ToString(dtValue.Rows[rowcount]["EffectiveMonth"]);
                    housePropertyTemp.EffectiveYear = Convert.ToString(dtValue.Rows[rowcount]["EffectiveYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        housePropertyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreateOn"])))
                        housePropertyTemp.CreateOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreateOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        housePropertyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        housePropertyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDelete"])))
                        housePropertyTemp.IsDelete = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDelete"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        housePropertyTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    //housePropertyTemp.LenderAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderName"])))
                        housePropertyTemp.LenderName = Convert.ToString(dtValue.Rows[rowcount]["LenderName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"])))
                        housePropertyTemp.LenderPAN = Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderType"])))
                        housePropertyTemp.LenderType = Convert.ToInt32(dtValue.Rows[rowcount]["LenderType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"])))
                        housePropertyTemp.LenderHRAAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"]);
                    this.Add(housePropertyTemp);
                }

            }
        }
        public HousePropertyList(Guid financialyearId, int companyId, string EffectiveMonth, string EffectiveYear ,string scode,string ecode)
        {
            this.CompanyId = companyId;
            HouseProperty houseProperty = new HouseProperty();
            DataTable dtValue = houseProperty.GetTableValues(financialyearId, companyId, EffectiveMonth, EffectiveYear,scode,ecode );
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    HouseProperty housePropertyTemp = new HouseProperty();
                    housePropertyTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    housePropertyTemp.EmployeeName = Convert.ToString(dtValue.Rows[rowcount]["EmployeeName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyId"])))
                        housePropertyTemp.PropertyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyReference"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyReference"]);
                    housePropertyTemp.Property_OwnersName = Convert.ToString(dtValue.Rows[rowcount]["Property_OwnersName"]);
                    housePropertyTemp.PropertyAddress = Convert.ToString(dtValue.Rows[rowcount]["PropertyAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyLoanAmount"])))
                        housePropertyTemp.PropertyLoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PropertyLoanAmount"]);
                    housePropertyTemp.PurposeOfLoan = Convert.ToString(dtValue.Rows[rowcount]["PurposeOfLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofSanctionofLoan"])))
                        housePropertyTemp.DateofSanctionofLoan = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofSanctionofLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyJointName"])))
                        housePropertyTemp.PropertyJointName = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyJointName"]);
                    housePropertyTemp.PropertyJointInterest = Convert.ToString(dtValue.Rows[rowcount]["PropertyJointInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"])))
                        housePropertyTemp.PayableHousingLoanPerYear = Convert.ToDecimal(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionInterest"])))
                        housePropertyTemp.PreConstructionInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterestOfYear"])))
                        housePropertyTemp.TotalInterestOfYear = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterestOfYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"])))
                        housePropertyTemp.Interest_RestrictedtoEmployee = Convert.ToDecimal(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConstrutionIsCompleted"])))
                        housePropertyTemp.ConstrutionIsCompleted = Convert.ToInt32(dtValue.Rows[rowcount]["ConstrutionIsCompleted"]);
                    housePropertyTemp.PropertySelfOccupied = Convert.ToString(dtValue.Rows[rowcount]["PropertySelfOccupied"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"])))
                        housePropertyTemp.HousingLoanTakenBefore_01_04_1999 = Convert.ToInt32(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome_PA"])))
                        housePropertyTemp.GrossRentalIncome_PA = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome_PA"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"])))
                        housePropertyTemp.Municipal_Water_Sewerage_taxpaid = Convert.ToDecimal(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome"])))
                        housePropertyTemp.GrossRentalIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessMunicipalTaxes"])))
                        housePropertyTemp.LessMunicipalTaxes = Convert.ToDecimal(dtValue.Rows[rowcount]["LessMunicipalTaxes"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Balance"])))
                        housePropertyTemp.Balance = Convert.ToDecimal(dtValue.Rows[rowcount]["Balance"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessStandardDeduction"])))
                        housePropertyTemp.LessStandardDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["LessStandardDeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"])))
                        housePropertyTemp.LessInterestOnHousingLoan = Convert.ToDecimal(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousePropertyNetIncome"])))
                        housePropertyTemp.HousePropertyNetIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["HousePropertyNetIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHRACompleted"])))
                        housePropertyTemp.IsHRACompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHRACompleted"]);
                    housePropertyTemp.EffectiveMonth = Convert.ToString(dtValue.Rows[rowcount]["EffectiveMonth"]);
                    housePropertyTemp.EffectiveYear = Convert.ToString(dtValue.Rows[rowcount]["EffectiveYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderName"])))
                        housePropertyTemp.LenderName = Convert.ToString(dtValue.Rows[rowcount]["LenderName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"])))
                        housePropertyTemp.LenderPAN = Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderType"])))
                        housePropertyTemp.LenderType = Convert.ToInt32(dtValue.Rows[rowcount]["LenderType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"])))
                        housePropertyTemp.LenderHRAAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"]);
                    this.Add(housePropertyTemp);
                }

            }
        }

        public HousePropertyList(Guid EmployeeId, Guid FinancialYear, Guid TxSectionId, string EffectiveMonth, string EffectiveYear, DateTime EffectiveDate)
        {
            HouseProperty houseProperty = new HouseProperty();
            DataTable dtValue = houseProperty.GetTableValues( EmployeeId,  FinancialYear, TxSectionId, EffectiveMonth, EffectiveYear,  EffectiveDate);
        }

        public HousePropertyList(Guid EmployeeId, int companyId,  Guid FinancialYear)
        {
            this.CompanyId = companyId;
            HouseProperty houseProperty = new HouseProperty();
            DataTable dtValue = houseProperty.GetTableValues(EmployeeId, companyId,  FinancialYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    HouseProperty housePropertyTemp = new HouseProperty();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        housePropertyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        housePropertyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"])))
                        housePropertyTemp.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NoOfProperties"])))
                        housePropertyTemp.NoOfProperties = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfProperties"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyId"])))
                        housePropertyTemp.PropertyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"])))
                        housePropertyTemp.FinancialYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyReference"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyReference"]);
                    housePropertyTemp.Property_OwnersName = Convert.ToString(dtValue.Rows[rowcount]["Property_OwnersName"]);
                    housePropertyTemp.PropertyAddress = Convert.ToString(dtValue.Rows[rowcount]["PropertyAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyLoanAmount"])))
                        housePropertyTemp.PropertyLoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PropertyLoanAmount"]);
                    housePropertyTemp.PurposeOfLoan = Convert.ToString(dtValue.Rows[rowcount]["PurposeOfLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofSanctionofLoan"])))
                        housePropertyTemp.DateofSanctionofLoan = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofSanctionofLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyJointName"])))
                        housePropertyTemp.PropertyJointName = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyJointName"]);
                    housePropertyTemp.PropertyJointInterest = Convert.ToString(dtValue.Rows[rowcount]["PropertyJointInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"])))
                        housePropertyTemp.PayableHousingLoanPerYear = Convert.ToDecimal(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionInterest"])))
                        housePropertyTemp.PreConstructionInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterestOfYear"])))
                        housePropertyTemp.TotalInterestOfYear = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterestOfYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"])))
                        housePropertyTemp.Interest_RestrictedtoEmployee = Convert.ToDecimal(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConstrutionIsCompleted"])))
                        housePropertyTemp.ConstrutionIsCompleted = Convert.ToInt32(dtValue.Rows[rowcount]["ConstrutionIsCompleted"]);
                    housePropertyTemp.PropertySelfOccupied = Convert.ToString(dtValue.Rows[rowcount]["PropertySelfOccupied"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"])))
                        housePropertyTemp.HousingLoanTakenBefore_01_04_1999 = Convert.ToInt32(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome_PA"])))
                        housePropertyTemp.GrossRentalIncome_PA = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome_PA"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"])))
                        housePropertyTemp.Municipal_Water_Sewerage_taxpaid = Convert.ToDecimal(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome"])))
                        housePropertyTemp.GrossRentalIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessMunicipalTaxes"])))
                        housePropertyTemp.LessMunicipalTaxes = Convert.ToDecimal(dtValue.Rows[rowcount]["LessMunicipalTaxes"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Balance"])))
                        housePropertyTemp.Balance = Convert.ToDecimal(dtValue.Rows[rowcount]["Balance"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessStandardDeduction"])))
                        housePropertyTemp.LessStandardDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["LessStandardDeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"])))
                        housePropertyTemp.LessInterestOnHousingLoan = Convert.ToDecimal(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousePropertyNetIncome"])))
                        housePropertyTemp.HousePropertyNetIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["HousePropertyNetIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHRACompleted"])))
                        housePropertyTemp.IsHRACompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHRACompleted"]);
                    housePropertyTemp.EffectiveMonth = Convert.ToString(dtValue.Rows[rowcount]["EffectiveMonth"]);
                    housePropertyTemp.EffectiveYear = Convert.ToString(dtValue.Rows[rowcount]["EffectiveYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        housePropertyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreateOn"])))
                        housePropertyTemp.CreateOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreateOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        housePropertyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        housePropertyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDelete"])))
                        housePropertyTemp.IsDelete = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDelete"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        housePropertyTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    housePropertyTemp.LenderAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderName"])))
                        housePropertyTemp.LenderName = Convert.ToString(dtValue.Rows[rowcount]["LenderName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"])))
                        housePropertyTemp.LenderPAN = Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderType"])))
                        housePropertyTemp.LenderType = Convert.ToInt32(dtValue.Rows[rowcount]["LenderType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"])))
                        housePropertyTemp.LenderHRAAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"]);
                    this.Add(housePropertyTemp);
                }

            }
        }

        public HousePropertyList(Guid EmployeeId, Guid FinancialYear, Guid TxSectionId, string EffectiveMonth, string EffectiveYear)
        {
            HouseProperty houseProperty = new HouseProperty();
            DataTable dtValue = houseProperty.GetTableValues(EmployeeId, FinancialYear, TxSectionId, EffectiveMonth, EffectiveYear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    HouseProperty housePropertyTemp = new HouseProperty();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        housePropertyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        housePropertyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"])))
                        housePropertyTemp.TxSectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["TxSectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NoOfProperties"])))
                        housePropertyTemp.NoOfProperties = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfProperties"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyId"])))
                        housePropertyTemp.PropertyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"])))
                        housePropertyTemp.FinancialYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyReference"])))
                        housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyReference"]);
                    housePropertyTemp.Property_OwnersName = Convert.ToString(dtValue.Rows[rowcount]["Property_OwnersName"]);
                    housePropertyTemp.PropertyAddress = Convert.ToString(dtValue.Rows[rowcount]["PropertyAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyLoanAmount"])))
                        housePropertyTemp.PropertyLoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["PropertyLoanAmount"]);
                    housePropertyTemp.PurposeOfLoan = Convert.ToString(dtValue.Rows[rowcount]["PurposeOfLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateofSanctionofLoan"])))
                        housePropertyTemp.DateofSanctionofLoan = Convert.ToDateTime(dtValue.Rows[rowcount]["DateofSanctionofLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PropertyJointName"])))
                        housePropertyTemp.PropertyJointName = Convert.ToInt32(dtValue.Rows[rowcount]["PropertyJointName"]);
                    housePropertyTemp.PropertyJointInterest = Convert.ToString(dtValue.Rows[rowcount]["PropertyJointInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"])))
                        housePropertyTemp.PayableHousingLoanPerYear = Convert.ToDecimal(dtValue.Rows[rowcount]["PayableHousingLoanPerYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionInterest"])))
                        housePropertyTemp.PreConstructionInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionInterest"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterestOfYear"])))
                        housePropertyTemp.TotalInterestOfYear = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterestOfYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"])))
                        housePropertyTemp.Interest_RestrictedtoEmployee = Convert.ToDecimal(dtValue.Rows[rowcount]["Interest_RestrictedtoEmployee"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConstrutionIsCompleted"])))
                        housePropertyTemp.ConstrutionIsCompleted = Convert.ToInt32(dtValue.Rows[rowcount]["ConstrutionIsCompleted"]);
                    housePropertyTemp.PropertySelfOccupied = Convert.ToString(dtValue.Rows[rowcount]["PropertySelfOccupied"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"])))
                        housePropertyTemp.HousingLoanTakenBefore_01_04_1999 = Convert.ToInt32(dtValue.Rows[rowcount]["HousingLoanTakenBefore_01_04_1999"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome_PA"])))
                        housePropertyTemp.GrossRentalIncome_PA = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome_PA"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"])))
                        housePropertyTemp.Municipal_Water_Sewerage_taxpaid = Convert.ToDecimal(dtValue.Rows[rowcount]["Municipal_Water_Sewerage_taxpaid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["GrossRentalIncome"])))
                        housePropertyTemp.GrossRentalIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["GrossRentalIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessMunicipalTaxes"])))
                        housePropertyTemp.LessMunicipalTaxes = Convert.ToDecimal(dtValue.Rows[rowcount]["LessMunicipalTaxes"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Balance"])))
                        housePropertyTemp.Balance = Convert.ToDecimal(dtValue.Rows[rowcount]["Balance"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessStandardDeduction"])))
                        housePropertyTemp.LessStandardDeduction = Convert.ToDecimal(dtValue.Rows[rowcount]["LessStandardDeduction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"])))
                        housePropertyTemp.LessInterestOnHousingLoan = Convert.ToDecimal(dtValue.Rows[rowcount]["LessInterestOnHousingLoan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HousePropertyNetIncome"])))
                        housePropertyTemp.HousePropertyNetIncome = Convert.ToDecimal(dtValue.Rows[rowcount]["HousePropertyNetIncome"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHRACompleted"])))
                        housePropertyTemp.IsHRACompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHRACompleted"]);
                    housePropertyTemp.EffectiveMonth = Convert.ToString(dtValue.Rows[rowcount]["EffectiveMonth"]);
                    housePropertyTemp.EffectiveYear = Convert.ToString(dtValue.Rows[rowcount]["EffectiveYear"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        housePropertyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreateOn"])))
                        housePropertyTemp.CreateOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreateOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        housePropertyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        housePropertyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDelete"])))
                        housePropertyTemp.IsDelete = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDelete"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        housePropertyTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    housePropertyTemp.LenderAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderName"])))
                        housePropertyTemp.LenderName = Convert.ToString(dtValue.Rows[rowcount]["LenderName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"])))
                        housePropertyTemp.LenderPAN = Convert.ToString(dtValue.Rows[rowcount]["LenderPAN"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderType"])))
                        housePropertyTemp.LenderType = Convert.ToInt32(dtValue.Rows[rowcount]["LenderType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"])))
                        housePropertyTemp.LenderHRAAddress = Convert.ToString(dtValue.Rows[rowcount]["LenderHRAAddress"]);
                    this.Add(housePropertyTemp);
                }

            }
        }


        //public HousePropertyList(int companyId, Guid financeyearId)
        //{
        //    this.CompanyId = companyId;
        //    HouseProperty houseProperty = new HouseProperty();
        //    DataTable dtValue = houseProperty.GetTableValues(companyId, Guid.Empty, Guid.Empty, financeyearId);
        //    if (dtValue.Rows.Count > 0)
        //    {
        //        for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
        //        {
        //            HouseProperty housePropertyTemp = new HouseProperty();
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
        //                housePropertyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
        //                housePropertyTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
        //                housePropertyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
        //                housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncomeFromHouseProp"])))
        //                housePropertyTemp.IncomeFromHouseProp = Convert.ToDecimal(dtValue.Rows[rowcount]["IncomeFromHouseProp"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDetailsRequired"])))
        //                housePropertyTemp.IsDetailsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDetailsRequired"]);
        //            housePropertyTemp.PropertyReferenceNo = Convert.ToString(dtValue.Rows[rowcount]["PropertyReferenceNo"]);
        //            housePropertyTemp.NameOfTheOwner = Convert.ToString(dtValue.Rows[rowcount]["NameOfTheOwner"]);
        //            housePropertyTemp.AddressOfTheProperty = Convert.ToString(dtValue.Rows[rowcount]["AddressOfTheProperty"]);
        //            housePropertyTemp.AddressOfTheLender = Convert.ToString(dtValue.Rows[rowcount]["AddressOfTheLender"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LoanAmount"])))
        //                housePropertyTemp.LoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["LoanAmount"]);
        //            housePropertyTemp.LoanrPupose = Convert.ToString(dtValue.Rows[rowcount]["LoanrPupose"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LoanSanctionDate"])))
        //                housePropertyTemp.LoanSanctionDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LoanSanctionDate"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsJointProperty"])))
        //                housePropertyTemp.IsJointProperty = Convert.ToBoolean(dtValue.Rows[rowcount]["IsJointProperty"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InterestShare"])))
        //                housePropertyTemp.InterestShare = Convert.ToDecimal(dtValue.Rows[rowcount]["InterestShare"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InterestPayableFortheYear"])))
        //                housePropertyTemp.InterestPayableFortheYear = Convert.ToDecimal(dtValue.Rows[rowcount]["InterestPayableFortheYear"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionPeriodInterest"])))
        //                housePropertyTemp.PreConstructionPeriodInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionPeriodInterest"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterest"])))
        //                housePropertyTemp.TotalInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterest"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsConstructionCompleted"])))
        //                housePropertyTemp.IsConstructionCompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsConstructionCompleted"]);
        //            housePropertyTemp.PropertyType = Convert.ToString(dtValue.Rows[rowcount]["PropertyType"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHouseLoanTakenBefore1999"])))
        //                housePropertyTemp.IsHouseLoanTakenBefore1999 = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHouseLoanTakenBefore1999"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RentalIncomePA"])))
        //                housePropertyTemp.RentalIncomePA = Convert.ToDecimal(dtValue.Rows[rowcount]["RentalIncomePA"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MunicipalTaxPaidAmount"])))
        //                housePropertyTemp.MunicipalTaxPaidAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["MunicipalTaxPaidAmount"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NetIncomeOrLoss"])))
        //                housePropertyTemp.NetIncomeOrLoss = Convert.ToDecimal(dtValue.Rows[rowcount]["NetIncomeOrLoss"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
        //                housePropertyTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
        //                housePropertyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
        //                housePropertyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
        //                housePropertyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
        //                housePropertyTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
        //            this.Add(housePropertyTemp);
        //        }

        //    }
        //}

        //public HousePropertyList(int companyId, Guid financeyearId, Guid employeeId)
        //{
        //    this.CompanyId = companyId;
        //    HouseProperty houseProperty = new HouseProperty();
        //    DataTable dtValue = houseProperty.GetTableValues(companyId, Guid.Empty, employeeId, financeyearId);
        //    if (dtValue.Rows.Count > 0)
        //    {
        //        for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
        //        {
        //            HouseProperty housePropertyTemp = new HouseProperty();
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
        //                housePropertyTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"])))
        //                housePropertyTemp.FinancialYearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinancialYearId"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
        //                housePropertyTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
        //                housePropertyTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IncomeFromHouseProp"])))
        //                housePropertyTemp.IncomeFromHouseProp = Convert.ToDecimal(dtValue.Rows[rowcount]["IncomeFromHouseProp"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDetailsRequired"])))
        //                housePropertyTemp.IsDetailsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDetailsRequired"]);
        //            housePropertyTemp.PropertyReferenceNo = Convert.ToString(dtValue.Rows[rowcount]["PropertyReferenceNo"]);
        //            housePropertyTemp.NameOfTheOwner = Convert.ToString(dtValue.Rows[rowcount]["NameOfTheOwner"]);
        //            housePropertyTemp.AddressOfTheProperty = Convert.ToString(dtValue.Rows[rowcount]["AddressOfTheProperty"]);
        //            housePropertyTemp.AddressOfTheLender = Convert.ToString(dtValue.Rows[rowcount]["AddressOfTheLender"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LoanAmount"])))
        //                housePropertyTemp.LoanAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["LoanAmount"]);
        //            housePropertyTemp.LoanrPupose = Convert.ToString(dtValue.Rows[rowcount]["LoanrPupose"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LoanSanctionDate"])))
        //                housePropertyTemp.LoanSanctionDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LoanSanctionDate"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsJointProperty"])))
        //                housePropertyTemp.IsJointProperty = Convert.ToBoolean(dtValue.Rows[rowcount]["IsJointProperty"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InterestShare"])))
        //                housePropertyTemp.InterestShare = Convert.ToDecimal(dtValue.Rows[rowcount]["InterestShare"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["InterestPayableFortheYear"])))
        //                housePropertyTemp.InterestPayableFortheYear = Convert.ToDecimal(dtValue.Rows[rowcount]["InterestPayableFortheYear"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PreConstructionPeriodInterest"])))
        //                housePropertyTemp.PreConstructionPeriodInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["PreConstructionPeriodInterest"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalInterest"])))
        //                housePropertyTemp.TotalInterest = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalInterest"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsConstructionCompleted"])))
        //                housePropertyTemp.IsConstructionCompleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsConstructionCompleted"]);
        //            housePropertyTemp.PropertyType = Convert.ToString(dtValue.Rows[rowcount]["PropertyType"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsHouseLoanTakenBefore1999"])))
        //                housePropertyTemp.IsHouseLoanTakenBefore1999 = Convert.ToBoolean(dtValue.Rows[rowcount]["IsHouseLoanTakenBefore1999"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RentalIncomePA"])))
        //                housePropertyTemp.RentalIncomePA = Convert.ToDecimal(dtValue.Rows[rowcount]["RentalIncomePA"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MunicipalTaxPaidAmount"])))
        //                housePropertyTemp.MunicipalTaxPaidAmount = Convert.ToDecimal(dtValue.Rows[rowcount]["MunicipalTaxPaidAmount"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NetIncomeOrLoss"])))
        //                housePropertyTemp.NetIncomeOrLoss = Convert.ToDecimal(dtValue.Rows[rowcount]["NetIncomeOrLoss"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
        //                housePropertyTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
        //                housePropertyTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
        //                housePropertyTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
        //                housePropertyTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
        //                housePropertyTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
        //            this.Add(housePropertyTemp);
        //        }

        //    }
        //}

        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        public Guid FinancialYearId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax section and add to the list
        /// </summary>
        /// <param name="houseProperty"></param>
        public void AddNew(HouseProperty houseProperty , string Type)
        {
            if (houseProperty.Save(Type))
            {
                this.Add(houseProperty);
            }
        }

        /// <summary>
        /// delete the tax section data
        /// </summary>
        /// <param name="houseProperty"></param>

        public void DeleteExist(HouseProperty houseProperty)
        {
            if (houseProperty.Delete())
            {
                this.Remove(houseProperty);
            }
        }


        #endregion
    }
}
