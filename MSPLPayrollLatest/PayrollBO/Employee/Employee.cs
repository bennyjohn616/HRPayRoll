// -----------------------------------------------------------------------
// <copyright file="Employee.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using SQLDBperation;

namespace PayrollBO
{
    /// <summary>
    /// To handle the Employee
    /// </summary>
    public class Employee
    {

        #region private variable

        private AttributeValueList _attributeValueList;
        private EmployeeAcademicList _employeeAcademicList;
        private EmployeeAddressList _employeeAddressList;
        private EmployeeBenefitComponentList _employeeBenefitComponentList;
        private EmployeeEmegencyContactList _employeeEmegencyContactList;
        private EmployeeEmployeementList _employeeEmployeementList;
        private EmployeeFamilyList _employeeFamilyList;
        private EmployeeJoingDocumentList _employeeJoingDocumentList;
        private EmployeeLanguageKnownList _employeeLanguageKnownList;
        private EmployeeNomineeList _employeeNomineeList;
        private Emp_Personal _employeePersonal;
        private EmployeeTrainingList _employeeTrainingList;
        private EntityBehaviorList _entityBehaviour;
        private Emp_BankList _employeeBankList;
        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Employee()
        {

        }
        //public Employee(int companyId)
        //{
        //    this.CompanyId = companyId;
        //    DataTable dt = this.GetActiveEmployee(companyId);
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["count"])))
        //                this.count = Convert.ToInt32(Convert.ToString(dt.Rows[i]["count"]));
        //            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["year"])))
        //                this.year = Convert.ToInt32(dt.Rows[i]["year"]);
        //        }
        //    }
        //}
        // Modified by Keerthika on 08/06/2017
        public Employee(int companyId, Guid employeeId)
        {
            this.Id = employeeId;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(companyId, Guid.Empty, this.Id);
            if (dtValue.Rows.Count > 0)
            {
                //  DateTime dt = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.BranchName = Convert.ToString(dtValue.Rows[0]["BranchName"]);
                this.DesignationName = Convert.ToString(dtValue.Rows[0]["DesignationName"]);
                this.CostCentreName = Convert.ToString(dtValue.Rows[0]["CostCentreName"]);
                this.DepartmentName = Convert.ToString(dtValue.Rows[0]["DepartmentName"]);
                this.GradeName = Convert.ToString(dtValue.Rows[0]["GradeName"]);
                this.LocationName = Convert.ToString(dtValue.Rows[0]["LocationName"]);
                this.PTLocationName = Convert.ToString(dtValue.Rows[0]["PTLocationName"]);
                this.ESIDespensaryName = Convert.ToString(dtValue.Rows[0]["ESIDespensaryId"]);
                this.CategoryName = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.ESILocationName = Convert.ToString(dtValue.Rows[0]["ESILocationName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Designation"])))
                    this.Designation = new Guid(Convert.ToString(dtValue.Rows[0]["Designation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfBirth"])))

                    //   this.DateOfBirth = dt.ToString("dd/mm/yyyy");
                    this.DateOfBirth = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfJoining"])))
                    this.DateOfJoining = Convert.ToDateTime(dtValue.Rows[0]["DateOfJoining"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfWedding"])))
                    this.DateOfWedding = Convert.ToDateTime(dtValue.Rows[0]["DateOfWedding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationPeriod"])))
                    this.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[0]["ConfirmationPeriod"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationDate"])))
                    this.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["ConfirmationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationDate"])))
                    this.SeparationDate = Convert.ToDateTime(dtValue.Rows[0]["SeparationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementYears"])))
                    this.RetirementYears = Convert.ToInt32(dtValue.Rows[0]["RetirementYears"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementDate"])))
                    this.RetirementDate = Convert.ToDateTime(dtValue.Rows[0]["RetirementDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Gender"])))
                    this.Gender = Convert.ToInt32(dtValue.Rows[0]["Gender"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Department"])))
                    this.Department = new Guid(Convert.ToString(dtValue.Rows[0]["Department"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isMetro"])))
                    this.isMetro = Convert.ToBoolean(dtValue.Rows[0]["isMetro"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Branch"])))
                    this.Branch = new Guid(Convert.ToString(dtValue.Rows[0]["Branch"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Location"])))
                    this.Location = new Guid(Convert.ToString(dtValue.Rows[0]["Location"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESILocation"])))
                    this.ESILocation = new Guid(Convert.ToString(dtValue.Rows[0]["ESILocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTLocation"])))
                    this.PTLocation = new Guid(Convert.ToString(dtValue.Rows[0]["PTLocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CostCentre"])))
                    this.CostCentre = new Guid(Convert.ToString(dtValue.Rows[0]["CostCentre"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Grade"])))
                    this.Grade = new Guid(Convert.ToString(dtValue.Rows[0]["Grade"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPayment"])))
                    this.StopPayment = Convert.ToBoolean(dtValue.Rows[0]["StopPayment"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollProcess"])))
                    this.PayrollProcess = Convert.ToBoolean(dtValue.Rows[0]["PayrollProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeImage"])))
                    this.EmployeeImage = (Convert.ToString(dtValue.Rows[0]["EmployeeImage"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"])))
                    this.TypeOfSeparation = Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationReason"])))
                    this.SeparationReason = Convert.ToString(dtValue.Rows[0]["SeparationReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LastWorkingDate"])))
                    this.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[0]["LastWorkingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseDate"])))
                    this.ReleaseDate = Convert.ToDateTime(dtValue.Rows[0]["ReleaseDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseReason"])))
                    this.ReleaseReason = Convert.ToString(dtValue.Rows[0]["ReleaseReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIEligibility"])))
                    this.ESIEligibility = Convert.ToBoolean(dtValue.Rows[0]["ESIEligibility"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIDespensary"])))
                    this.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[0]["ESIDespensary"]));

            }
        }
        /// <summary>
        /// Created By:Sharmila
        /// </summary
        /// <param name="employeeId"></param>
        public Employee(string EmpCode)
        {
            this.EmployeeCode = EmpCode;
            //this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.EmployeeCode);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.BranchName = Convert.ToString(dtValue.Rows[0]["BranchName"]);
                this.DesignationName = Convert.ToString(dtValue.Rows[0]["DesignationName"]);
                this.CostCentreName = Convert.ToString(dtValue.Rows[0]["CostCentreName"]);
                this.DepartmentName = Convert.ToString(dtValue.Rows[0]["DepartmentName"]);
                this.GradeName = Convert.ToString(dtValue.Rows[0]["GradeName"]);
                this.LocationName = Convert.ToString(dtValue.Rows[0]["LocationName"]);
                this.PTLocationName = Convert.ToString(dtValue.Rows[0]["PTLocationName"]);
                this.ESIDespensaryName = Convert.ToString(dtValue.Rows[0]["ESIDespensaryId"]);
                this.CategoryName = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.ESILocationName = Convert.ToString(dtValue.Rows[0]["ESILocationName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Designation"])))
                    this.Designation = new Guid(Convert.ToString(dtValue.Rows[0]["Designation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfBirth"])))
                    this.DateOfBirth = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfJoining"])))
                    this.DateOfJoining = Convert.ToDateTime(dtValue.Rows[0]["DateOfJoining"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfWedding"])))
                    this.DateOfWedding = Convert.ToDateTime(dtValue.Rows[0]["DateOfWedding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationPeriod"])))
                    this.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[0]["ConfirmationPeriod"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationDate"])))
                    this.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["ConfirmationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationDate"])))
                    this.SeparationDate = Convert.ToDateTime(dtValue.Rows[0]["SeparationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementYears"])))
                    this.RetirementYears = Convert.ToInt32(dtValue.Rows[0]["RetirementYears"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementDate"])))
                    this.RetirementDate = Convert.ToDateTime(dtValue.Rows[0]["RetirementDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Gender"])))
                    this.Gender = Convert.ToInt32(dtValue.Rows[0]["Gender"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Department"])))
                    this.Department = new Guid(Convert.ToString(dtValue.Rows[0]["Department"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isMetro"])))
                    this.isMetro = Convert.ToBoolean(dtValue.Rows[0]["isMetro"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Branch"])))
                    this.Branch = new Guid(Convert.ToString(dtValue.Rows[0]["Branch"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Location"])))
                    this.Location = new Guid(Convert.ToString(dtValue.Rows[0]["Location"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CostCentre"])))
                    this.CostCentre = new Guid(Convert.ToString(dtValue.Rows[0]["CostCentre"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Grade"])))
                    this.Grade = new Guid(Convert.ToString(dtValue.Rows[0]["Grade"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPayment"])))
                    this.StopPayment = Convert.ToBoolean(dtValue.Rows[0]["StopPayment"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollProcess"])))
                    this.PayrollProcess = Convert.ToBoolean(dtValue.Rows[0]["PayrollProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeImage"])))
                    this.EmployeeImage = (Convert.ToString(dtValue.Rows[0]["EmployeeImage"]));
            }
        }

        public Employee(Guid employeeId)
        {
            this.Id = employeeId;
            //this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.BranchName = Convert.ToString(dtValue.Rows[0]["BranchName"]);
                this.DesignationName = Convert.ToString(dtValue.Rows[0]["DesignationName"]);
                this.CostCentreName = Convert.ToString(dtValue.Rows[0]["CostCentreName"]);
                this.DepartmentName = Convert.ToString(dtValue.Rows[0]["DepartmentName"]);
                this.GradeName = Convert.ToString(dtValue.Rows[0]["GradeName"]);
                this.LocationName = Convert.ToString(dtValue.Rows[0]["LocationName"]);
                this.PTLocationName = Convert.ToString(dtValue.Rows[0]["PTLocationName"]);
                this.ESIDespensaryName = Convert.ToString(dtValue.Rows[0]["ESIDespensaryId"]);
                this.CategoryName = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.ESILocationName = Convert.ToString(dtValue.Rows[0]["ESILocationName"]);
                this.DesignationName = Convert.ToString(dtValue.Rows[0]["DesignationName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Designation"])))
                    this.Designation = new Guid(Convert.ToString(dtValue.Rows[0]["Designation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfBirth"])))
                    this.DateOfBirth = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfJoining"])))
                    this.DateOfJoining = Convert.ToDateTime(dtValue.Rows[0]["DateOfJoining"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfWedding"])))
                    this.DateOfWedding = Convert.ToDateTime(dtValue.Rows[0]["DateOfWedding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationPeriod"])))
                    this.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[0]["ConfirmationPeriod"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationDate"])))
                    this.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["ConfirmationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationDate"])))
                    this.SeparationDate = Convert.ToDateTime(dtValue.Rows[0]["SeparationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementYears"])))
                    this.RetirementYears = Convert.ToInt32(dtValue.Rows[0]["RetirementYears"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementDate"])))
                    this.RetirementDate = Convert.ToDateTime(dtValue.Rows[0]["RetirementDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Gender"])))
                    this.Gender = Convert.ToInt32(dtValue.Rows[0]["Gender"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Department"])))
                    this.Department = new Guid(Convert.ToString(dtValue.Rows[0]["Department"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isMetro"])))
                    this.isMetro = Convert.ToBoolean(dtValue.Rows[0]["isMetro"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Branch"])))
                    this.Branch = new Guid(Convert.ToString(dtValue.Rows[0]["Branch"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Location"])))
                    this.Location = new Guid(Convert.ToString(dtValue.Rows[0]["Location"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESILocation"])))
                    this.ESILocation = new Guid(Convert.ToString(dtValue.Rows[0]["ESILocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTLocation"])))
                    this.PTLocation = new Guid(Convert.ToString(dtValue.Rows[0]["PTLocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CostCentre"])))
                    this.CostCentre = new Guid(Convert.ToString(dtValue.Rows[0]["CostCentre"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Grade"])))
                    this.Grade = new Guid(Convert.ToString(dtValue.Rows[0]["Grade"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPayment"])))
                    this.StopPayment = Convert.ToBoolean(dtValue.Rows[0]["StopPayment"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollProcess"])))
                    this.PayrollProcess = Convert.ToBoolean(dtValue.Rows[0]["PayrollProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"])))
                    this.TypeOfSeparation = Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationReason"])))
                    this.SeparationReason = Convert.ToString(dtValue.Rows[0]["SeparationReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LastWorkingDate"])))
                    this.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[0]["LastWorkingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseDate"])))
                    this.ReleaseDate = Convert.ToDateTime(dtValue.Rows[0]["ReleaseDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseReason"])))
                    this.ReleaseReason = Convert.ToString(dtValue.Rows[0]["ReleaseReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIEligibility"])))
                    this.ESIEligibility = Convert.ToBoolean(dtValue.Rows[0]["ESIEligibility"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIDespensary"])))
                    this.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[0]["ESIDespensary"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DBConnectionId"])))
                    this.DBConnectionId = Convert.ToInt32(dtValue.Rows[0]["DBConnectionId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeImage"])))
                    this.EmployeeImage = (Convert.ToString(dtValue.Rows[0]["EmployeeImage"]));

            }
        }
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Employee(int companyId, Guid catogoryId, Guid id)
        {
            this.Id = id;
            this.CompanyId = companyId;
            this.CategoryId = catogoryId;
            DataTable dtValue = this.GetTableValues(companyId, catogoryId, this.Id);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Designation"])))
                    this.Designation = new Guid(Convert.ToString(dtValue.Rows[0]["Designation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfBirth"])))
                    this.DateOfBirth = Convert.ToDateTime(dtValue.Rows[0]["DateOfBirth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfJoining"])))
                    this.DateOfJoining = Convert.ToDateTime(dtValue.Rows[0]["DateOfJoining"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DateOfWedding"])))
                    this.DateOfWedding = Convert.ToDateTime(dtValue.Rows[0]["DateOfWedding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationPeriod"])))
                    this.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[0]["ConfirmationPeriod"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfirmationDate"])))
                    this.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["ConfirmationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationDate"])))
                    this.SeparationDate = Convert.ToDateTime(dtValue.Rows[0]["SeparationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementYears"])))
                    this.RetirementYears = Convert.ToInt32(dtValue.Rows[0]["RetirementYears"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RetirementDate"])))
                    this.RetirementDate = Convert.ToDateTime(dtValue.Rows[0]["RetirementDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Gender"])))
                    this.Gender = Convert.ToInt32(dtValue.Rows[0]["Gender"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Department"])))
                    this.Department = new Guid(Convert.ToString(dtValue.Rows[0]["Department"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["isMetro"])))
                    this.isMetro = Convert.ToBoolean(dtValue.Rows[0]["isMetro"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Branch"])))
                    this.Branch = new Guid(Convert.ToString(dtValue.Rows[0]["Branch"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Location"])))
                    this.Location = new Guid(Convert.ToString(dtValue.Rows[0]["Location"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESILocation"])))
                    this.ESILocation = new Guid(Convert.ToString(dtValue.Rows[0]["ESILocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTLocation"])))
                    this.PTLocation = new Guid(Convert.ToString(dtValue.Rows[0]["PTLocation"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CostCentre"])))
                    this.CostCentre = new Guid(Convert.ToString(dtValue.Rows[0]["CostCentre"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Grade"])))
                    this.Grade = new Guid(Convert.ToString(dtValue.Rows[0]["Grade"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["StopPayment"])))
                    this.StopPayment = Convert.ToBoolean(dtValue.Rows[0]["StopPayment"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollProcess"])))
                    this.PayrollProcess = Convert.ToBoolean(dtValue.Rows[0]["PayrollProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"])))
                    this.TypeOfSeparation = Convert.ToString(dtValue.Rows[0]["TypeOfSeparation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationReason"])))
                    this.SeparationReason = Convert.ToString(dtValue.Rows[0]["SeparationReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LastWorkingDate"])))
                    this.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[0]["LastWorkingDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseDate"])))
                    this.ReleaseDate = Convert.ToDateTime(dtValue.Rows[0]["ReleaseDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseReason"])))
                    this.ReleaseReason = Convert.ToString(dtValue.Rows[0]["ReleaseReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIEligibility"])))
                    this.ESIEligibility = Convert.ToBoolean(dtValue.Rows[0]["ESIEligibility"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIDespensary"])))
                    this.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[0]["ESIDespensary"]));
                // Modified by Babu.R as on 24-Jul-2017 for Separation last working date validation
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MonthlyInputLastDate"])))
                    this.MonthlyInputLastDate = Convert.ToDateTime(dtValue.Rows[0]["MonthlyInputLastDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollInputLastDate"])))
                    this.PayrollInputLastDate = Convert.ToDateTime(dtValue.Rows[0]["PayrollInputLastDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MonthlyInputDate"])))
                    this.MonthlyInputDate = Convert.ToDateTime(dtValue.Rows[0]["MonthlyInputDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollInputDate"])))
                    this.PayrollInputDate = Convert.ToDateTime(dtValue.Rows[0]["PayrollInputDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeImage"])))
                    this.EmployeeImage = (Convert.ToString(dtValue.Rows[0]["EmployeeImage"]));
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
        /// Get or Set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Get or Set the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or Set the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or Set the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get or Set the Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Get or Set the Designation
        /// </summary>
        public Guid Designation { get; set; }

        /// <summary>
        /// Get or Set the DateOfBirth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Get or Set the DateOfJoining
        /// </summary>
        public DateTime DateOfJoining { get; set; }

        /// <summary>
        /// Get or Set the DateOfWedding
        /// </summary>
        public DateTime DateOfWedding { get; set; }

        /// <summary>
        /// Get or Set the ConfirmationPeriod
        /// </summary>
        public int ConfirmationPeriod { get; set; }

        /// <summary>
        /// Get or Set the ConfirmationDate
        /// </summary>
        public DateTime ConfirmationDate { get; set; }

        /// <summary>
        /// Get or Set the SeparationDate
        /// </summary>
        public DateTime SeparationDate { get; set; }

        /// <summary>
        /// Get or Set the RetirementYears
        /// </summary>
        public int RetirementYears { get; set; }

        /// <summary>
        /// Get or Set the RetirementDate
        /// </summary>
        public DateTime RetirementDate { get; set; }

        /// <summary>
        /// Get or Set the Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Get or Set the Department
        /// </summary>
        public Guid Department { get; set; }

        /// <summary>
        /// Get or Set the isMetro
        /// </summary>
        public bool isMetro { get; set; }

        /// <summary>
        /// Get or Set the Branch
        /// </summary>
        public Guid Branch { get; set; }
        /// <summary>
        /// Get or Set the Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Get or Set the Location
        /// </summary>
        public Guid Location { get; set; }


        /// <summary>
        /// Get or Set the PTLocation
        /// </summary>
        public Guid PTLocation { get; set; }

        /// <summary>
        /// Get or Set the ESILocation
        /// </summary>
        public Guid ESILocation { get; set; }
        /// Get or Set the ESIDispensary
        /// </summary>
        public Guid ESIDespensary { get; set; }
        /// <summary>
        /// Get or Set the CostCentre
        /// </summary>
        public Guid CostCentre { get; set; }

        /// <summary>
        /// Get or Set the Grade
        /// </summary>
        public Guid Grade { get; set; }

        /// <summary>
        /// Get or Set the StopPayment
        /// </summary>
        public bool StopPayment { get; set; }

        /// <summary>
        /// Get or Set the PayrollProcess
        /// </summary>
        public bool PayrollProcess { get; set; }

        /// <summary>
        /// Get or Set the Status
        /// </summary>
        public int Status { get; set; }

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
        public bool Usercreationtype { get; set; }

        /// <summary>
        /// Get or Set the TypeOfSeparation
        /// </summary>
        public string TypeOfSeparation { get; set; }

        /// <summary>
        /// Get or Set the SeparationReason
        /// </summary>
        public string SeparationReason { get; set; }
        /// <summary>
        /// Get or Set the LastWorkingDate
        /// </summary>
        public DateTime LastWorkingDate { get; set; }
        /// <summary>
        /// Get or Set the ReleaseDate
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Get or Set the ReleaseReason
        /// </summary>
        public string ReleaseReason { get; set; }



        public bool ESIEligibility { get; set; }

        public string BranchName { get; set; }

        public string DesignationName { get; set; }


        public string ESIDespensaryName { get; set; }

        public string ESILocationName { get; set; }

        public string EmployeeImage { get; set; }
        public int count { get; set; }
        public int month { get; set; }

        public int year { get; set; }
        public DateTime MonthlyInputLastDate { get; set; }
        public DateTime PayrollInputLastDate { get; set; }
        public DateTime MonthlyInputDate { get; set; }
        public DateTime PayrollInputDate { get; set; }
        public bool IsMailSend { get; set; }
        public bool IsCircularMailSent { get; set; }
        public int DBConnectionId { get; set; }
        public string ImportOption { get; set; }

        public string NewEmployeeCode { get; set; }

        public Guid financeyearid { get; set; }

        public DateTime Startingdate { get; set;}

        public DateTime Endingdate { get; set; }

        public int Age
        {
            get
            {
                DateTime enddate = this.Endingdate;
                if (enddate == DateTime.MinValue && this.year == 0 && this.month == 0)
                {
                    enddate = DateTime.Now;
                }
                if (enddate == DateTime.MinValue)
                {
                    int yearval = this.year;
                    if (this.month > 03)
                    {
                        yearval = this.year + 1;
                    }

                    enddate = new DateTime(yearval, 03, 31);

                }


                int Age1 = 0;
                if (this.DateOfBirth != DateTime.MinValue && enddate != DateTime.MinValue)
                    {
                        Age1 = enddate.Year - this.DateOfBirth.Year;
                        if (((((this.DateOfBirth.Year + Age1) * 100) + this.DateOfBirth.Month)) > ((enddate.Year * 100) + enddate.Month))
                        {
                            Age1--;
                        }
                    }
                   
                return Age1;

                /*     int YearsPassed = DateTime.Now.Year - this.DateOfBirth.Year;

                     if (DateTime.Now.Month < this.DateOfBirth.Month || (DateTime.Now.Month == this.DateOfBirth.Month && DateTime.Now.Day < this.DateOfBirth.Day))
                     {
                         YearsPassed--;
                     }
                     return YearsPassed;*/
            }
        }
        public double NoOfServiceYear
        {
            get
            {

                DateTime doj = this.DateOfJoining;
                DateTime toDate = this.LastWorkingDate == DateTime.MinValue ? DateTime.Now : this.LastWorkingDate;

                int[,] betWeen = serviceYear(toDate, doj);


                int Years = betWeen[0, 0];

                int month = betWeen[0, 1];


                Company comp = new Company(this.CompanyId);
                if (comp.ServiceYearMonth != 0)
                {
                    Years = Years + (month >= comp.ServiceYearMonth ? 1 : 0);
                }



                return Years;
            }
        }

        /// <summary>
        /// get or set the attribute value list
        /// </summary>
        public AttributeValueList AttributeValueList
        {
            get
            {
                if (object.ReferenceEquals(_attributeValueList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _attributeValueList = new AttributeValueList(this.Id);
                    }
                    else
                        _attributeValueList = new AttributeValueList();
                }
                return _attributeValueList;

            }
            set
            {
                _attributeValueList = value;
            }
        }
        public EntityBehaviorList EntityBehaviorList
        {
            get
            {
                if (!object.ReferenceEquals(Id, null))
                {
                    EntityModel entityModel = new EntityModel(ComValue.SalaryTable, CompanyId);
                    EntityMapping entitmap = new EntityMapping("Employee", Id.ToString(), entityModel.Id);
                    if (entitmap.EntityId != null)
                        _entityBehaviour = new EntityBehaviorList(new Guid(entitmap.EntityId), entityModel.Id);

                }


                return _entityBehaviour;
            }
            set
            {
                _entityBehaviour = value;
            }
        }

        /// <summary>
        /// get or set the Employee Academic List
        /// </summary>
        public EmployeeAcademicList EmployeeAcademicList
        {
            get
            {
                if (object.ReferenceEquals(_employeeAcademicList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeAcademicList = new EmployeeAcademicList(this.Id);
                    }
                    else
                        _employeeAcademicList = new EmployeeAcademicList();
                }
                return _employeeAcademicList;

            }
            set
            {
                _employeeAcademicList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Address List
        /// </summary>
        public EmployeeAddressList EmployeeAddressList
        {
            get
            {
                if (object.ReferenceEquals(_employeeAddressList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeAddressList = new EmployeeAddressList(this.Id);
                    }
                    else
                        _employeeAddressList = new EmployeeAddressList();

                }
                return _employeeAddressList;

            }
            set
            {
                _employeeAddressList = value;
            }
        }

        /// <summary>
        /// get or set the Employee BenefitComponent List
        /// </summary>
        public EmployeeBenefitComponentList EmployeeBenefitComponentList
        {
            get
            {
                if (object.ReferenceEquals(_employeeBenefitComponentList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeBenefitComponentList = new EmployeeBenefitComponentList(this.Id);
                    }
                    else
                        _employeeBenefitComponentList = new EmployeeBenefitComponentList();

                }
                return _employeeBenefitComponentList;

            }
            set
            {
                _employeeBenefitComponentList = value;
            }
        }

        /// <summary>
        /// get or set the Employee EmegencyContact List
        /// </summary>
        public EmployeeEmegencyContactList EmployeeEmegencyContactList
        {
            get
            {
                if (object.ReferenceEquals(_employeeEmegencyContactList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeEmegencyContactList = new EmployeeEmegencyContactList(this.Id);
                    }
                    else
                        _employeeEmegencyContactList = new EmployeeEmegencyContactList();
                }
                return _employeeEmegencyContactList;

            }
            set
            {
                _employeeEmegencyContactList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Employeement List
        /// </summary>
        public EmployeeEmployeementList EmployeeEmployeementList
        {
            get
            {
                if (object.ReferenceEquals(_employeeEmployeementList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeEmployeementList = new EmployeeEmployeementList(this.Id);
                    }
                    else
                        _employeeEmployeementList = new EmployeeEmployeementList();
                }
                return _employeeEmployeementList;

            }
            set
            {
                _employeeEmployeementList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Family List
        /// </summary>
        public EmployeeFamilyList EmployeeFamilyList
        {
            get
            {
                if (object.ReferenceEquals(_employeeFamilyList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeFamilyList = new EmployeeFamilyList(this.Id);
                    }
                    else
                        _employeeFamilyList = new EmployeeFamilyList();
                }
                return _employeeFamilyList;

            }
            set
            {
                _employeeFamilyList = value;
            }
        }

        /// <summary>
        /// get or set the Employee JoingDocument List
        /// </summary>
        public EmployeeJoingDocumentList EmployeeJoingDocumentList
        {
            get
            {
                if (object.ReferenceEquals(_employeeJoingDocumentList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeJoingDocumentList = new EmployeeJoingDocumentList(this.Id);
                    }
                    else
                        _employeeJoingDocumentList = new EmployeeJoingDocumentList();
                }
                return _employeeJoingDocumentList;

            }
            set
            {
                _employeeJoingDocumentList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Language Known List
        /// </summary>
        public EmployeeLanguageKnownList EmployeeLanguageKnownList
        {
            get
            {
                if (object.ReferenceEquals(_employeeLanguageKnownList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeLanguageKnownList = new EmployeeLanguageKnownList(this.Id);
                    }
                    else
                        _employeeLanguageKnownList = new EmployeeLanguageKnownList();
                }
                return _employeeLanguageKnownList;

            }
            set
            {
                _employeeLanguageKnownList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Nominee List
        /// </summary>
        public EmployeeNomineeList EmployeeNomineeList
        {
            get
            {
                if (object.ReferenceEquals(_employeeNomineeList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeNomineeList = new EmployeeNomineeList(this.Id);
                    }
                    else
                        _employeeNomineeList = new EmployeeNomineeList();
                }
                return _employeeNomineeList;

            }
            set
            {
                _employeeNomineeList = value;
            }
        }

        /// <summary>
        /// get or set the Employee Personal
        /// </summary>
        public Emp_Personal EmployeePersonal
        {
            get
            {
                if (object.ReferenceEquals(_employeePersonal, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeePersonal = new Emp_Personal(this.Id);
                    }
                    else
                        _employeePersonal = new Emp_Personal();
                }
                return _employeePersonal;

            }
            set
            {
                _employeePersonal = value;
            }
        }

        /// <summary>
        /// get or set the Employee Training List
        /// </summary>
        public EmployeeTrainingList EmployeeTrainingList
        {
            get
            {
                if (object.ReferenceEquals(_employeeTrainingList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeTrainingList = new EmployeeTrainingList(this.Id);
                    }
                    else
                        _employeeTrainingList = new EmployeeTrainingList();
                }
                return _employeeTrainingList;

            }
            set
            {
                _employeeTrainingList = value;
            }
        }


        public Emp_BankList EmployeeBankList
        {
            get
            {
                if (object.ReferenceEquals(_employeeBankList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _employeeBankList = new Emp_BankList(this.Id);
                    }
                    else
                        _employeeBankList = new Emp_BankList();
                }
                return _employeeBankList;

            }
            set
            {
                _employeeBankList = value;
            }
        }
        public object ConfigurationManager { get; private set; }
        public PayrollHistoryValueList PayrollHistoryList { get; set; }
        public PayrollHistoryValueList PayrollofMonth { get; set; }

        public PayrollHistoryValueList PayrollMonth { get; set; }
        public TXEmployeeSection EmpTaxDeclaration { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string PTLocationName { get; set; }
        public string CategoryName { get; set; }
        public string GradeName { get; set; }
        public string CostCentreName { get; set; }

        public string Query { get; set; }

        #endregion
        public static DataTable checkemailsend(Guid empdid, string type)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_EMAILSENDCHECK");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", empdid);
            sqlCommand.Parameters.AddWithValue("@Checking", type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public static DataTable CurrentMonthDashboard(int compid, string types)
        {
            SqlCommand sqlCommand = new SqlCommand("payrollDashBoard_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            sqlCommand.Parameters.AddWithValue("@Type", types);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public static DataTable DynamicMonthDashboard(int compid, string types, int monthval)
        {
            SqlCommand sqlCommand = new SqlCommand("payrollDashBoard_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            sqlCommand.Parameters.AddWithValue("@Month", monthval);
            sqlCommand.Parameters.AddWithValue("@Type", types);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #region Public methods

        public int[,] serviceYear(DateTime d1, DateTime d2)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int syear;
            int smonth;
            int sday;

            fromDate = d2;
            toDate = d1;

            //day calculation
            int increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }

            if (increment != 0)
            {
                sday = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                sday = toDate.Day - fromDate.Day;
            }
            //months calculation
            if ((fromDate.Month + increment) > toDate.Month)
            {
                smonth = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                smonth = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            //year calculation
            syear = toDate.Year - (fromDate.Year + increment);





            int[,] ym = new int[1, 2];
            ym[0, 0] = syear;
            ym[0, 1] = smonth;



            return ym;
        }
        /// <summary>
        /// Save the Employee
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("Employee_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryId);
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", this.EmployeeCode);
            sqlCommand.Parameters.AddWithValue("@FirstName", this.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", this.LastName);
            sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            sqlCommand.Parameters.AddWithValue("@Phone", this.Phone);
            sqlCommand.Parameters.AddWithValue("@Designation", this.Designation.ToString() == Guid.Empty.ToString() ? null : this.Designation.ToString());
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", this.DateOfBirth == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@DateOfJoining", this.DateOfJoining == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.DateOfJoining);
            sqlCommand.Parameters.AddWithValue("@DateOfWedding", this.DateOfWedding == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.DateOfWedding);
            sqlCommand.Parameters.AddWithValue("@ConfirmationPeriod", this.ConfirmationPeriod);
            sqlCommand.Parameters.AddWithValue("@ConfirmationDate", this.ConfirmationDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.ConfirmationDate);
            sqlCommand.Parameters.AddWithValue("@SeparationDate", this.SeparationDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800") : this.SeparationDate);
            sqlCommand.Parameters.AddWithValue("@RetirementYears", this.RetirementYears);
            sqlCommand.Parameters.AddWithValue("@RetirementDate", this.RetirementDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.RetirementDate);
            sqlCommand.Parameters.AddWithValue("@Gender", this.Gender);
            sqlCommand.Parameters.AddWithValue("@Department", this.Department.ToString() == Guid.Empty.ToString() ? null : this.Department.ToString());
            sqlCommand.Parameters.AddWithValue("@isMetro", this.isMetro);
            sqlCommand.Parameters.AddWithValue("@Branch", this.Branch.ToString() == Guid.Empty.ToString() ? null : this.Branch.ToString());
            sqlCommand.Parameters.AddWithValue("@Location", this.Location.ToString() == Guid.Empty.ToString() ? null : this.Location.ToString());
            sqlCommand.Parameters.AddWithValue("@PTLocation", this.PTLocation.ToString() == Guid.Empty.ToString() ? null : this.PTLocation.ToString());
            sqlCommand.Parameters.AddWithValue("@ESILocation", this.ESILocation.ToString() == Guid.Empty.ToString() ? null : this.ESILocation.ToString());
            sqlCommand.Parameters.AddWithValue("@ESIDespensary", this.ESIDespensary.ToString() == Guid.Empty.ToString() ? null : this.ESIDespensary.ToString());
            sqlCommand.Parameters.AddWithValue("@CostCentre", this.CostCentre.ToString() == Guid.Empty.ToString() ? null : this.CostCentre.ToString());
            sqlCommand.Parameters.AddWithValue("@Grade", this.Grade.ToString() == Guid.Empty.ToString() ? null : this.Grade.ToString());
            sqlCommand.Parameters.AddWithValue("@StopPayment", this.StopPayment);
            sqlCommand.Parameters.AddWithValue("@EmployeeImage", this.EmployeeImage);
            sqlCommand.Parameters.AddWithValue("@PayrollProcess", this.PayrollProcess);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@TypeOfSeparation", this.TypeOfSeparation);
            sqlCommand.Parameters.AddWithValue("@SeparationReason", this.SeparationReason);
            sqlCommand.Parameters.AddWithValue("@LastWorkingDate", this.LastWorkingDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.LastWorkingDate);
            sqlCommand.Parameters.AddWithValue("@ReleaseDate", this.ReleaseDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.ReleaseDate);
            sqlCommand.Parameters.AddWithValue("@ReleaseReason", this.ReleaseReason);
            sqlCommand.Parameters.AddWithValue("@ESIEligibility", this.ESIEligibility);
            sqlCommand.Parameters.AddWithValue("@DBConnectionId", this.DBConnectionId);
            sqlCommand.Parameters.AddWithValue("@ImportOption", this.ImportOption);
            sqlCommand.Parameters.AddWithValue("@UpdateQuery", this.Query);
            //if (ImportOption == "EmployeeImport")
            //{
            //    string Query = string.Empty;
            //    string Query1 = string.Empty;
            //    Query = "update Employee set ";
            //    Query = Query + " [CompanyId] =  '" + this.CompanyId.ToString() + "'";
            //    if (this.CategoryId != Guid.Empty)
            //        Query = Query + " ,[CategoryId] =  '" + this.CategoryId.ToString() + "'";
            //    Query = Query + " ,[EmployeeCode] =  '" + this.EmployeeCode.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.FirstName))
            //        Query = Query + " ,[FirstName] =  '" + this.FirstName.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.LastName))
            //        Query = Query + " ,[LastName] =  '" + this.LastName.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Email))
            //        Query = Query + " ,[Email] =  '" + this.Email.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Phone))
            //        Query = Query + " ,[Phone] =  '" + this.Phone.ToString() + "'";
            //    if (this.Designation.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[Designation] =  '" + this.Designation.ToString() + "'";
            //    if (this.DateOfBirth.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.DateOfBirth = Convert.ToDateTime(this.DateOfBirth);
            //        Query = Query + " ,[DateOfBirth] = dbo.Datetimechk( '" + this.DateOfBirth.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[DateOfBirth] = dbo.Datetimechk( '01/01/1800 12:00:00')";
            //    //}
            //    if (this.DateOfJoining.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.DateOfJoining = Convert.ToDateTime(this.DateOfJoining);
            //        Query = Query + " ,[DateOfJoining] = dbo.Datetimechk( '" + this.DateOfJoining.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[DateOfJoining] = dbo.Datetimechk( '01/01/1800 12:00:00')";
            //    //}
            //    if (this.DateOfWedding.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.DateOfWedding = Convert.ToDateTime(this.DateOfWedding);
            //        Query = Query + " ,[DateOfWedding] = dbo.Datetimechk( '" + this.DateOfWedding.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[DateOfWedding] = dbo.Datetimechk( '01/01/1800 12:00:00')";
            //    //}

            //    if (!string.IsNullOrEmpty(this.ConfirmationPeriod.ToString()))
            //        Query = Query + " ,[ConfirmationPeriod] =  '" + this.ConfirmationPeriod.ToString() + "'";

            //    if (this.ConfirmationDate.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.ConfirmationDate = Convert.ToDateTime(this.ConfirmationDate);
            //        Query = Query + " ,[ConfirmationDate] = dbo.Datetimechk( '" + this.ConfirmationDate.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[ConfirmationDate] = dbo.Datetimechk( '01/01/1800 12:00:00')";
            //    //}
            //    if (this.SeparationDate.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.SeparationDate = Convert.ToDateTime(this.SeparationDate);
            //        Query = Query + " ,[SeparationDate] = dbo.Datetimechk( '" + this.SeparationDate.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[SeparationDate] = dbo.Datetimechk('01/01/1800 12:00:00')";
            //    //}
            //    if (!string.IsNullOrEmpty(this.RetirementYears.ToString()))
            //        Query = Query + " ,[RetirementYears] =  '" + this.RetirementYears.ToString() + "'";
            //    if (this.RetirementDate.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.RetirementDate = Convert.ToDateTime(this.RetirementDate);
            //        Query = Query + " ,[RetirementDate] = dbo.Datetimechk( '" + this.RetirementDate.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[RetirementDate] = dbo.Datetimechk('01/01/1800 12:00:00')";
            //    //}
            //    if (!string.IsNullOrEmpty(this.Gender.ToString()))
            //        Query = Query + " ,[Gender] =  '" + this.Gender.ToString() + "'";
            //    if (this.Department.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[Department] =  '" + this.Department.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.isMetro.ToString()))
            //        Query = Query + " ,[isMetro] =  '" + this.isMetro.ToString() + "'";
            //    if (this.Branch.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[Branch] =  '" + this.Branch.ToString() + "'";
            //    if (this.Location.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[Location] =  '" + this.Location.ToString() + "'";
            //    if (this.ESILocation.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[ESILocation] =  '" + this.ESILocation.ToString() + "'";
            //    if (this.PTLocation.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[PTLocation]= '" + this.PTLocation.ToString() + "'";
            //    if (this.ESIDespensary.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[ESIDespensary]= '" + this.ESIDespensary.ToString() + "'";
            //    if (this.CostCentre.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[CostCentre] =  '" + this.CostCentre.ToString() + "'";
            //    if (this.Grade.ToString() != Guid.Empty.ToString())
            //        Query = Query + " ,[Grade] =  '" + this.Grade.ToString() + "'";

            //    Query = Query + " ,[StopPayment] =  '" + this.StopPayment.ToString() + "'";
            //    Query = Query + " ,[PayrollProcess] =  '" + this.PayrollProcess.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.Status.ToString()))
            //        Query = Query + " ,[Status] =  '" + this.Status.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
            //        Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";

            //    Query = Query + " ,[ModifiedOn] = GETDATE()";
            //    Query = Query + " ,[IsDeleted] =  '" + this.IsDeleted.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.TypeOfSeparation))
            //        Query = Query + " ,[TypeOfSeparation]= '" + this.TypeOfSeparation.ToString() + "'";
            //    if (!string.IsNullOrEmpty(this.SeparationReason))
            //        Query = Query + " ,[SeparationReason]=  '" + this.SeparationReason.ToString() + "'";
            //    if (this.LastWorkingDate.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.LastWorkingDate = Convert.ToDateTime(this.LastWorkingDate);
            //        Query = Query + " ,[LastWorkingDate]=dbo.Datetimechk( '" + this.LastWorkingDate.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[LastWorkingDate]=dbo.Datetimechk( '01/01/1800 12:00:00')";
            //    //}
            //    if (this.ReleaseDate.ToString() != DateTime.MinValue.ToString())
            //    {
            //        this.ReleaseDate = Convert.ToDateTime(this.ReleaseDate);
            //        Query = Query + " ,[ReleaseDate]=dbo.Datetimechk( '" + this.ReleaseDate.ToString("dd/MMM/yyyy") + "')";
            //    }
            //    //else
            //    //{
            //    //    Query = Query + " ,[ReleaseDate]=dbo.Datetimechk('01/01/1800 12:00:00')";
            //    //}
            //    if (!string.IsNullOrEmpty(this.ReleaseReason))
            //        Query = Query + " ,[ReleaseReason]= '" + this.ReleaseReason.ToString() + "'";

            //    Query = Query + "  Where Id = '" + this.Id.ToString() + "'";


            //   sqlCommand.Parameters.AddWithValue("@UpdateQuery", Query);

            //}

            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            string upd = "";
            if (status)
            {
                SqlCommand sqlCmd;
                sqlCmd = new SqlCommand("select * from [user]  where employeeid='" + this.Id + "'");
                sqlCmd.CommandType = CommandType.Text;
                LoginDBOperation LgnDBOperation = new LoginDBOperation();
                DataTable dt1 = new DataTable();
                dt1 = LgnDBOperation.GetTableData(sqlCmd);
                if (dt1.Rows.Count > 0)
                {
                    upd = "Y";
                }
                else
                {
                    upd = "";
                }
            }
            if (upd == "Y")
            {
                SqlCommand sqlCmd;
                if (this.ReleaseDate <= DateTime.MinValue && this.LastWorkingDate <= DateTime.MinValue)
                {
                    sqlCmd = new SqlCommand("update [user] set IsActive= 1,IsDeleted=0,ModifiedBy='" + this.ModifiedBy + "',ModifiedOn= getdate() where employeeid='" + this.Id + "'");

                }
                else
                {
                    sqlCmd = new SqlCommand("update [user] set IsActive= 0 ,ModifiedBy='" + this.ModifiedBy + "',ModifiedOn= getdate() where employeeid='" + this.Id + "'");
                }

                sqlCmd.CommandType = CommandType.Text;
                LoginDBOperation LgnDBOperation = new LoginDBOperation();
                string outValue11 = string.Empty;
                LgnDBOperation.SaveData(sqlCmd, out outValue11, "");
                this.Id = new Guid(outValue);
            }
            return status;
        }

        public bool UpdateImportEmployeeDetais()
        {
            SqlCommand sqlCommand = new SqlCommand("EmpAmendmentImport");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ImportOption", this.ImportOption);
            sqlCommand.Parameters.AddWithValue("@UpdateQuery", this.Query);
            // sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }


        //GetTableData


        public bool SavePast(Guid empid, int companyid, string oldemp)
        {
            SqlCommand sqlCommand = new SqlCommand("Emp_Past_Data_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", empid);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyid);
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", oldemp);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        /// <summary>
        /// Delete the Employee
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Employee_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        public void Initialize()
        {
            this.AttributeValueList = new AttributeValueList();
            this.EmployeeAcademicList = new EmployeeAcademicList();
            this.EmployeeAddressList = new EmployeeAddressList();
            this.EmployeeBenefitComponentList = new EmployeeBenefitComponentList();
            this.EmployeeEmegencyContactList = new EmployeeEmegencyContactList();
            this.EmployeeEmployeementList = new EmployeeEmployeementList();
            this.EmployeeFamilyList = new EmployeeFamilyList();
            this.EmployeeJoingDocumentList = new EmployeeJoingDocumentList();
            this.EmployeeLanguageKnownList = new EmployeeLanguageKnownList();
            this.EmployeeNomineeList = new EmployeeNomineeList();
            this.EmployeePersonal = new Emp_Personal();
            this.EmployeeTrainingList = new EmployeeTrainingList();

        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        public bool UpdateESIEligibility(Guid employeeId, int userId, bool eligibility)
        {
            int eli = eligibility == true ? 1 : 0;
            SqlCommand sqlCommand = new SqlCommand("update Employee set ESIEligibility= '" + eli + "' ,ModifiedBy='" + userId + "',ModifiedOn= '" + (DateTime.Today).ToString("dd/MM/yyyy") + "' where Id='" + employeeId + "'");
            sqlCommand.CommandType = CommandType.Text;

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;

        }
        public static DataTable GetActiveEmployee(int companyId)
        {
            SqlCommand sqlCommand = new SqlCommand("usp_GetActiveEmployeedetails");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(int companyId, Guid categoryId, Guid id)
        {
            SqlCommand sqlCommand = new SqlCommand("Employee_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", categoryId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(Guid id)
        {
            SqlCommand sqlCommand = new SqlCommand("Employee_SelectOne");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(string EmpCode)
        {
            SqlCommand sqlCommand = new SqlCommand("Employee_One");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@EmployeeCode", EmpCode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        /// <summary>
        /// Select the Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEmployess(int companyId, Guid categoryId, Guid employeeId)
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("EmployeeSelectALL");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", categoryId);
            sqlCommand.Parameters.AddWithValue("@Id", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetFullmanager(int compid)
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("ManagerRole_SelectALL");
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetEmployess(int companyId)
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("EmployeeSelect");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable EmployeeEmailcheck(string emailcheck, Guid empid)
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("EmployeeEmail_Check");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Email", emailcheck);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", empid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetEmployess(int companyId, Guid categoryId, string Name)
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("EmployeeSelectALL");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", categoryId);
            sqlCommand.Parameters.AddWithValue("@Name", Name);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        ///////////////////////////////////////




        /// <summary>
        /// Employees Filter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetEmployess(int companyId, Guid categoryId, string filterExpr, Guid employeeId)
        {
            string query = "SELECT[Id],[CompanyId],[CategoryId],[EmployeeCode],[FirstName],[LastName],[Email],[Phone],[Designation] ,[DateOfBirth],[DateOfJoining]"
                         + " ,[DateOfWedding],[ConfirmationPeriod],[ConfirmationDate],[SeparationDate],[RetirementYears],[RetirementDate],[Gender],[Department],[isMetro],[Branch]"
                         + " ,[Location],[CostCentre],[Grade],[StopPayment],[PayrollProcess],[Status],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[IsDeleted]"
                         + " ,[TypeOfSeparation],[SeparationReason],[LastWorkingDate],[ReleaseDate],[ReleaseReason],[ESIEligibility],[ESILocation],[PTLocation],[ESIDespensary]"
                         + " FROM Employee AS emp  WHERE emp.CategoryId = CASE '" + categoryId + "' WHEN CAST(0x0 AS UNIQUEIDENTIFIER) THEN emp.CategoryId"
                         + " ELSE '" + categoryId + "' END AND emp.CompanyId = " + companyId + " AND IsDeleted = 0 " + filterExpr
                         + " AND emp.Id=CASE'" + employeeId + "' WHEN CAST(0x0 AS UNIQUEIDENTIFIER) THEN emp.Id"
                         + " ELSE '" + employeeId + "' END  order by EmployeeCode";
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion

    }
}

