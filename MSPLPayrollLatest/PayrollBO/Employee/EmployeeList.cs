// -----------------------------------------------------------------------
// <copyright file="EmployeeList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace PayrollBO
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmployeeList : List<Employee>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="categoryId"></param>




        public EmployeeList(string EMPEMAIL, Guid empid)
        {
            Employee employee = new Employee();
            DataTable dtValue = employee.EmployeeEmailcheck(EMPEMAIL, empid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();

                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);

                    this.Add(employeeTemp);
                }
            }
        }


        public EmployeeList(int companyId, Guid categoryId)
        {
            this.CompanyId = companyId;
            this.CategoryId = categoryId;
            Guid employeeid = Guid.Empty;
            Employee employee = new Employee();
            DataTable dtValue = employee.GetEmployess(this.CompanyId, this.CategoryId, employeeid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    this.Add(employeeTemp);
                }
            }
        }

        public EmployeeList(int companyId)
        {
            Employee employee = new Employee();
            DataTable dtValue = employee.GetEmployess(companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    this.Add(employeeTemp);
                }
            }
        }

        public EmployeeList(int companyId, Guid categoryId, int sindex, int eindex)
        {
            this.CompanyId = companyId;
            this.CategoryId = categoryId;
            Guid employeeid = Guid.Empty;
            Employee employee = new Employee();
            DataTable dtValue = employee.GetEmployess(this.CompanyId, this.CategoryId, employeeid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = sindex; rowcount <= eindex; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    this.Add(employeeTemp);
                }
            }
        }
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="categoryId"></param>
        public EmployeeList(int companyId, Guid categoryId, string filterExpr, int userId, Guid EmployeeId)
        {
            this.CompanyId = companyId;
            this.CategoryId = categoryId;
            this.EmployeeId = EmployeeId;
            Employee employee = new Employee();
            DataTable dtValue = employee.GetEmployess(this.CompanyId, this.CategoryId, filterExpr, this.EmployeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    this.Add(employeeTemp);
                }
            }
        }



        /////////////

        public EmployeeList(int companyId, Guid categoryId, string Name)
        {
            this.CompanyId = companyId;
            this.CategoryId = categoryId;

            Employee employee = new Employee();
            DataTable dtValue = employee.GetEmployess(this.CompanyId, this.CategoryId, Name);
            if (dtValue.Rows.Count > 0)
            {

            }
        }
        public EmployeeList(int companyId, int userId, Guid employeeId,int temp,int temp1)
        {
            this.CompanyId = companyId;
            Employee employee = new Employee();
            this.EmployeeId = employeeId;

            //int employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            DataTable dtValue = new DataTable();
            string roll = Convert.ToString(HttpContext.Current.Session["RoleName"]);
            //if (roll != "Employee")
            //{
            //    dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, Guid.Empty);
            //}
            //else
            //{
            //    dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, this.EmployeeId);
            //}
            dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, this.EmployeeId);
            UserCompanymapping logedUser = new UserCompanymapping(userId);
               //UserCompanymappingList usermapping = new UserCompanymappingList(userId);
            string filter = string.Empty;

            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    employeeTemp.DesignationName = Convert.ToString(dtValue.Rows[rowcount]["DesignationName"]);
                    employeeTemp.DepartmentName = Convert.ToString(dtValue.Rows[rowcount]["DepartmentName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.PTLocationName = Convert.ToString(dtValue.Rows[rowcount]["PTLocationName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.CategoryName = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    employeeTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeTemp.GradeName = Convert.ToString(dtValue.Rows[rowcount]["GradeName"]);
                    employeeTemp.CostCentreName = Convert.ToString(dtValue.Rows[rowcount]["CostCentreName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    if (employeeId == Guid.Empty && !string.IsNullOrEmpty(logedUser.RightsOn) && logedUser.RightsOn != "0" && logedUser.RightsOnValue != Guid.Empty.ToString())
                    {
                        if (logedUser.RightsOnValue == Convert.ToString(employeeTemp.GetType().GetProperty(logedUser.RightsOn).GetValue(employeeTemp, null)))
                        {
                            this.Add(employeeTemp);
                        }
                    }
                    else
                    {
                        this.Add(employeeTemp);
                    }

                }
            }
        }

        ///////////////
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="categoryId"></param>
        /// 
        public EmployeeList(int companyId, int userId, Guid employeeId,int temp,int temp1,int temp2)
        {
            this.CompanyId = companyId;
            Employee employee = new Employee();
            this.EmployeeId = employeeId;

            //int employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            DataTable dtValue = new DataTable();
            string roll = Convert.ToString(HttpContext.Current.Session["RoleName"]);
           
                dtValue = employee.GetFullmanager(companyId);
           

            UserCompanymapping logedUser = new UserCompanymapping(userId);
            //   UserCompanymappingList usermapping = new UserCompanymappingList(userId);
            string filter = string.Empty;

            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    //employeeTemp.DesignationName = Convert.ToString(dtValue.Rows[rowcount]["DesignationName"]);
                    //employeeTemp.DepartmentName = Convert.ToString(dtValue.Rows[rowcount]["DepartmentName"]);
                    //employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    //employeeTemp.PTLocationName = Convert.ToString(dtValue.Rows[rowcount]["PTLocationName"]);
                    //employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    //employeeTemp.CategoryName = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    //employeeTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    //employeeTemp.GradeName = Convert.ToString(dtValue.Rows[rowcount]["GradeName"]);
                    //employeeTemp.CostCentreName = Convert.ToString(dtValue.Rows[rowcount]["CostCentreName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    if (employeeId == Guid.Empty && !string.IsNullOrEmpty(logedUser.RightsOn) && logedUser.RightsOn != "0" && logedUser.RightsOnValue != Guid.Empty.ToString())
                    {
                        if (logedUser.RightsOnValue == Convert.ToString(employeeTemp.GetType().GetProperty(logedUser.RightsOn).GetValue(employeeTemp, null)))
                        {
                            this.Add(employeeTemp);
                        }
                    }
                    else
                    {
                        this.Add(employeeTemp);
                    }

                }
            }
        }



        public EmployeeList(int companyId, int userId, Guid employeeId)
        {
            this.CompanyId = companyId;
            Employee employee = new Employee();
            this.EmployeeId = employeeId;

            //int employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            DataTable dtValue = new DataTable();
            string roll = Convert.ToString(HttpContext.Current.Session["RoleName"]);
            if (roll != "Employee")
            {
                dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, Guid.Empty);
            }
            else
            {
                dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, this.EmployeeId);
            }

            UserCompanymapping logedUser = new UserCompanymapping(userId);
            //   UserCompanymappingList usermapping = new UserCompanymappingList(userId);
            string filter = string.Empty;

            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    employeeTemp.DesignationName = Convert.ToString(dtValue.Rows[rowcount]["DesignationName"]);
                    employeeTemp.DepartmentName = Convert.ToString(dtValue.Rows[rowcount]["DepartmentName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.PTLocationName = Convert.ToString(dtValue.Rows[rowcount]["PTLocationName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.CategoryName = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    employeeTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeTemp.GradeName = Convert.ToString(dtValue.Rows[rowcount]["GradeName"]);
                    employeeTemp.CostCentreName = Convert.ToString(dtValue.Rows[rowcount]["CostCentreName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    if (employeeId == Guid.Empty && !string.IsNullOrEmpty(logedUser.RightsOn) && logedUser.RightsOn != "0" && logedUser.RightsOnValue != Guid.Empty.ToString())
                    {
                        if (logedUser.RightsOnValue == Convert.ToString(employeeTemp.GetType().GetProperty(logedUser.RightsOn).GetValue(employeeTemp, null)))
                        {
                            this.Add(employeeTemp);
                        }
                    }
                    else
                    {
                        this.Add(employeeTemp);
                    }

                }
            }
        }

        #endregion

        #region"Manager List"

        public EmployeeList(int companyId, int userId, Guid employeeId, int tepval)
        {
            this.CompanyId = companyId;
            Employee employee = new Employee();
            this.EmployeeId = employeeId;

            //int employeeId = new Guid(Convert.ToString(Session["EmployeeId"]));
            string roll = Convert.ToString(HttpContext.Current.Session["RoleName"]);
            DataTable dtValue = new DataTable();

            if (roll != "Employee")
            {
                dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, Guid.Empty);
            }
            else
            {
                dtValue = employee.GetEmployess(this.CompanyId, Guid.Empty, this.EmployeeId);
            }



            UserCompanymapping logedUser = new UserCompanymapping(userId);
            //   UserCompanymappingList usermapping = new UserCompanymappingList(userId);
            string filter = string.Empty;

            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Employee employeeTemp = new Employee();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        employeeTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        employeeTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    employeeTemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    employeeTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    employeeTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    employeeTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    employeeTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    employeeTemp.DesignationName = Convert.ToString(dtValue.Rows[rowcount]["DesignationName"]);
                    employeeTemp.DepartmentName = Convert.ToString(dtValue.Rows[rowcount]["DepartmentName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.PTLocationName = Convert.ToString(dtValue.Rows[rowcount]["PTLocationName"]);
                    employeeTemp.LocationName = Convert.ToString(dtValue.Rows[rowcount]["LocationName"]);
                    employeeTemp.CategoryName = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    employeeTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeTemp.GradeName = Convert.ToString(dtValue.Rows[rowcount]["GradeName"]);
                    employeeTemp.CostCentreName = Convert.ToString(dtValue.Rows[rowcount]["CostCentreName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Designation"])))
                        employeeTemp.Designation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Designation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfBirth"])))
                        employeeTemp.DateOfBirth = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfBirth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfJoining"])))
                        employeeTemp.DateOfJoining = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfJoining"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DateOfWedding"])))
                        employeeTemp.DateOfWedding = Convert.ToDateTime(dtValue.Rows[rowcount]["DateOfWedding"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationPeriod"])))
                        employeeTemp.ConfirmationPeriod = Convert.ToInt32(dtValue.Rows[rowcount]["ConfirmationPeriod"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfirmationDate"])))
                        employeeTemp.ConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ConfirmationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationDate"])))
                        employeeTemp.SeparationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["SeparationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementYears"])))
                        employeeTemp.RetirementYears = Convert.ToInt32(dtValue.Rows[rowcount]["RetirementYears"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RetirementDate"])))
                        employeeTemp.RetirementDate = Convert.ToDateTime(dtValue.Rows[rowcount]["RetirementDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Gender"])))
                        employeeTemp.Gender = Convert.ToInt32(dtValue.Rows[rowcount]["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Department"])))
                        employeeTemp.Department = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Department"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["isMetro"])))
                        employeeTemp.isMetro = Convert.ToBoolean(dtValue.Rows[rowcount]["isMetro"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Branch"])))
                        employeeTemp.Branch = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Branch"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Location"])))
                        employeeTemp.Location = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Location"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"])))
                        employeeTemp.ESILocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESILocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"])))
                        employeeTemp.ESIDespensary = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ESIDespensary"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"])))
                        employeeTemp.PTLocation = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PTLocation"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"])))
                        employeeTemp.CostCentre = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CostCentre"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Grade"])))
                        employeeTemp.Grade = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Grade"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["StopPayment"])))
                        employeeTemp.StopPayment = Convert.ToBoolean(dtValue.Rows[rowcount]["StopPayment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollProcess"])))
                        employeeTemp.PayrollProcess = Convert.ToBoolean(dtValue.Rows[rowcount]["PayrollProcess"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        employeeTemp.Status = Convert.ToInt32(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"])))
                        employeeTemp.TypeOfSeparation = Convert.ToString(dtValue.Rows[rowcount]["TypeOfSeparation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"])))
                        employeeTemp.SeparationReason = Convert.ToString(dtValue.Rows[rowcount]["SeparationReason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LastWorkingDate"])))
                        employeeTemp.LastWorkingDate = Convert.ToDateTime(dtValue.Rows[rowcount]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseDate"])))
                        employeeTemp.ReleaseDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ReleaseDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"])))
                        employeeTemp.ReleaseReason = Convert.ToString(dtValue.Rows[rowcount]["ReleaseReason"]);
                    if (employeeId == Guid.Empty && !string.IsNullOrEmpty(logedUser.RightsOn) && logedUser.RightsOn != "0" && logedUser.RightsOnValue != Guid.Empty.ToString())
                    {
                        if (logedUser.RightsOnValue == Convert.ToString(employeeTemp.GetType().GetProperty(logedUser.RightsOn).GetValue(employeeTemp, null)))
                        {
                            this.Add(employeeTemp);
                        }
                    }
                    else
                    {
                        this.Add(employeeTemp);
                    }

                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime TransactinDate { get; set; }
        #endregion

        #region Public methods

        /// <summary>
        /// Save the Employee and add to the list
        /// </summary>
        /// <param name="employee"></param>
        public void AddNew(Employee employee)
        {
            if (employee.Save())
            {
                this.Add(employee);
            }
        }

        /// <summary>
        /// Delete the Employee and remove from the list
        /// </summary>
        /// <param name="employee"></param>
        public void DeleteExist(Employee employee)
        {
            if (employee.Delete())
            {
                this.Remove(employee);
            }
        }

        public void Initialize()
        {
            for (int count = 0; count < this.Count; count++)
            {
                this[count].AttributeValueList = new AttributeValueList();
                this[count].EmployeeAcademicList = new EmployeeAcademicList();
                this[count].EmployeeAddressList = new EmployeeAddressList();
                this[count].EmployeeBenefitComponentList = new EmployeeBenefitComponentList();
                this[count].EmployeeEmegencyContactList = new EmployeeEmegencyContactList();
                this[count].EmployeeEmployeementList = new EmployeeEmployeementList();
                this[count].EmployeeFamilyList = new EmployeeFamilyList();
                this[count].EmployeeJoingDocumentList = new EmployeeJoingDocumentList();
                this[count].EmployeeLanguageKnownList = new EmployeeLanguageKnownList();
                this[count].EmployeeNomineeList = new EmployeeNomineeList();
                this[count].EmployeePersonal = new Emp_Personal();
                this[count].EmployeeTrainingList = new EmployeeTrainingList();
                this[count].EmployeeBankList = new Emp_BankList();
            }

        }
        #endregion

        #region private methods



        #endregion
    }
}
