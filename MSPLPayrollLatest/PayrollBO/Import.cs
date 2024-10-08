using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using TraceError;

namespace PayrollBO
{
    public class Import
    {
        public List<string> importEmployee(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow, int DBConnectionId)
        {

            EmployeeList employeelist = new EmployeeList(companyId, userId, Guid.Empty);
            CategoryList categorylist = new CategoryList(companyId);
            DesignationList designationlist = new DesignationList(companyId);
            DepartmentList departmentList = new DepartmentList(companyId);
            LocationList locationlist = new LocationList(companyId);
            EsiLocationList esiLocationList = new EsiLocationList(companyId);
            ESIDespensaryList esiDespensaryList = new ESIDespensaryList(companyId);
            PTLocationList ptLocationList = new PTLocationList(companyId);
            CostCentreList costCentreList = new CostCentreList(companyId);
            GradeList gradeList = new GradeList(companyId);
            BranchList branchlist = new BranchList(companyId);
            EmployeeList newEmployeelist = new EmployeeList();



            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee employee = new Employee();
                employee.CompanyId = companyId;
                employee.CreatedBy = userId;
                employee.ModifiedBy = userId;
                employee.DBConnectionId = DBConnectionId;
                employee.ImportOption = "";

                Employee existEmp = null;
                string Query = string.Empty;

                importTbl.ImportColumns.ForEach(u =>
                {
                    string Query1 = string.Empty;
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";

                    //Employee existEmp = employeelist.Where(p => p.EmployeeCode == employee.EmployeeCode).FirstOrDefault();
                    //Employee existEmp = null; 
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        employee.EmployeeCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                        existEmp = employeelist.Where(p => p.EmployeeCode == employee.EmployeeCode).FirstOrDefault();
                        if (columns.Contains(u.MappedColumnName))
                        {
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }

                            if (!object.ReferenceEquals(existEmp, null))
                            {
                                employee.Id = existEmp.Id;
                                if (!importTbl.IsAmendment)
                                {
                                    error.Add("There is a duplicate employee code " + employee.EmployeeCode);
                                }

                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "first name")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.FirstName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[FirstName] =  '" + employee.FirstName.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(employee.FirstName) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.FirstName = existEmp.FirstName;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("First Name is required");
                            }

                        }
                    }
                    else if (u.Name.Trim().ToLower() == "last name")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.LastName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[LastName] =  '" + employee.LastName.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(employee.LastName) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.LastName = existEmp.LastName;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Last Name is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "date of joining")//Date
                    {

                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDoj = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.DateOfJoining = validator.ValidateDate(strDoj, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.DateOfJoining = employee.DateOfJoining == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.DateOfJoining;
                            Query = importTbl.IsAmendment == true ? Query + ",[DateOfJoining] = dbo.Datetimechk( '" + employee.DateOfJoining.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(strDoj) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.DateOfJoining = existEmp.DateOfJoining;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Date of Joining is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower().Trim() == "category")
                    {

                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.CategoryId = validator.ValidateCategory(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref categorylist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, userId);      //modifified "User Id"  by SuriyaPrakash.
                            Query = importTbl.IsAmendment == true ? Query + ",[CategoryId] =  '" + employee.CategoryId.ToString() + "'" : "";
                            if (employee.CategoryId == Guid.Empty && importTbl.AddMasterValue)
                            {
                                if (error.Count > 0)
                                {
                                    error.RemoveAt(error.Count - 1);
                                }
                                employee.CategoryId = AddNewMasterValues("Category", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet, 0);
                                if (employee.CategoryId != Guid.Empty)
                                {
                                    categorylist.Add(new Category { Name = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.CategoryId, DisOrder = 0 });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.CategoryId = existEmp.CategoryId;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Category is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "email")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Email = validator.ValidateEmail(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Email] =  '" + employee.Email.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Email = existEmp.Email;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Email is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "phone")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Phone = validator.ValidatePhoneNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "10");
                            Query = importTbl.IsAmendment == true ? Query + ",[Phone] =  '" + employee.Phone.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Phone = existEmp.Phone;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Phone number is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "designation")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Designation = validator.ValidateDesignation(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), designationlist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Designation] =  '" + employee.Designation.ToString() + "'" : "";
                            if (employee.Designation == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.Designation = AddNewMasterValues("Designation", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.Designation != Guid.Empty)
                                {
                                    designationlist.Add(new Designation { DesignationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.Designation });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Designation = existEmp.Designation;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Designation is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "date of birth")//Date
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDob = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.DateOfBirth = validator.ValidateDate(strDob, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.DateOfBirth = employee.DateOfBirth == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.DateOfBirth;
                            Query = importTbl.IsAmendment == true ? Query + ",[DateOfBirth] = dbo.Datetimechk( '" + employee.DateOfBirth.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment)
                            {
                                employee.DateOfBirth = existEmp.DateOfBirth == null ? employee.DateOfBirth : existEmp.DateOfBirth;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Date of Birth is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "date of wedding")//Date
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDob = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.DateOfWedding = validator.ValidateDate(strDob, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.DateOfWedding = employee.DateOfWedding == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.DateOfWedding;
                            Query = importTbl.IsAmendment == true ? Query + ",[DateOfWedding] = dbo.Datetimechk( '" + employee.DateOfWedding.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.DateOfWedding = existEmp.DateOfWedding;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Date Of Wedding is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "confirmation period")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.ConfirmationPeriod = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[ConfirmationPeriod] = '" + employee.ConfirmationPeriod + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            validator.ValidateMinValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MinVal, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.ConfirmationPeriod = existEmp.ConfirmationPeriod;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Confirmation Period is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "confirmation date")//Date
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDob = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.ConfirmationDate = validator.ValidateDate(strDob, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.ConfirmationDate = employee.ConfirmationDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.ConfirmationDate;
                            Query = importTbl.IsAmendment == true ? Query + ",[ConfirmationDate] = dbo.Datetimechk( '" + employee.ConfirmationDate.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.DateOfBirth = existEmp.DateOfBirth;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Confirmation Date is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "separation date")//Date
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDob = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.SeparationDate = validator.ValidateDate(strDob, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.SeparationDate = employee.SeparationDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.SeparationDate;
                            Query = importTbl.IsAmendment == true ? Query + ",[SeparationDate]= dbo.Datetimechk( '" + employee.SeparationDate.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.SeparationDate = existEmp.SeparationDate;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Separation Date is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "retirement years")//Max min
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.RetirementYears = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[RetirementYears] =  '" + employee.RetirementYears.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.RetirementYears = existEmp.RetirementYears;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Retirement Years is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "retirement date")//Date
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string strDob = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/');
                            employee.RetirementDate = validator.ValidateDate(strDob, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employee.RetirementDate = employee.RetirementDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employee.RetirementDate;
                            Query = importTbl.IsAmendment == true ? Query + ",[RetirementDate]= dbo.Datetimechk( '" + employee.RetirementDate.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.RetirementDate = existEmp.RetirementDate;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Retirement Date is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "gender")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Gender = validator.GetGender(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Gender] =  '" + employee.Gender.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Gender = existEmp.Gender;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Gender is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "department")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Department = validator.ValidateDepartment(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), departmentList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                            if (employee.Department == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.Department = AddNewMasterValues("Department", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.Department != Guid.Empty)
                                {
                                    departmentList.Add(new Department { DepartmentName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.Department });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Department] =  '" + employee.Department.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Department = existEmp.Department;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Department is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "metro")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.isMetro = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[isMetro] =  '" + employee.isMetro.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.isMetro = existEmp.isMetro;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Metro is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "location")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Location = validator.ValidateLocation(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), locationlist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.Location == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.Location = AddNewMasterValues("Location", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.Location != Guid.Empty)
                                {
                                    locationlist.Add(new Location { LocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.Location });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Location] =  '" + employee.Location.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Location = existEmp.Location;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Location is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "costcentre")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.CostCentre = validator.ValidateCostCentre(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), costCentreList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.CostCentre == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.CostCentre = AddNewMasterValues("CostCentre", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.CostCentre != Guid.Empty)
                                {
                                    costCentreList.Add(new CostCentre { CostCentreName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.CostCentre });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[CostCentre] =  '" + employee.CostCentre.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.CostCentre = existEmp.CostCentre;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Cost Centre is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "grade")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Grade = validator.ValidateGrade(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), gradeList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.Grade == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.Grade = AddNewMasterValues("Grade", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.Grade != Guid.Empty)
                                {
                                    gradeList.Add(new Grade { GradeName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.Grade });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Grade] =  '" + employee.Grade.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Grade = existEmp.Grade;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Grade is required");
                            }
                        }
                    }
                    //if (u.Name.Trim().ToLower() == "stop payment")
                    //{
                    //    if (columns.Contains(u.MappedColumnName))
                    //    {
                    //        employee.StopPayment = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        if (u.IsRequired)
                    //        {
                    //            validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        }
                    //        if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                    //        {
                    //            employee.DateOfBirth = existEmp.DateOfBirth;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (u.IsRequired && !importTbl.IsAmendment)
                    //        {
                    //            error.Add("Stop Payment is required");
                    //        }
                    //    }
                    //}
                    //if (u.Name.Trim().ToLower() == "payroll process")
                    //{
                    //    if (columns.Contains(u.MappedColumnName))
                    //    {
                    //        employee.PayrollProcess = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        if (u.IsRequired)
                    //        {
                    //            validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        }
                    //        if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                    //        {
                    //            employee.PayrollProcess = existEmp.PayrollProcess;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (u.IsRequired && !importTbl.IsAmendment)
                    //        {
                    //            error.Add("Payroll Process is required");
                    //        }
                    //    }
                    //}
                    //if (u.Name.Trim().ToLower() == "status")
                    //{
                    //    if (columns.Contains(u.MappedColumnName))
                    //    {
                    //        employee.Status = validator.GetStatus(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        if (u.IsRequired)
                    //        {
                    //            validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                    //        }
                    //        if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                    //        {
                    //            employee.Status = existEmp.Status;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (u.IsRequired && !importTbl.IsAmendment)
                    //        {
                    //            error.Add("Status is required");
                    //        }
                    //    }
                    //}
                    else if (u.Name.Trim().ToLower() == "esi location")
                    {
                        //employee.EmployeeCode = Convert.ToString(xlValue.Rows[rowCount]["employee code"]);
                        //existEmp = employeelist.Where(p => p.EmployeeCode == employee.EmployeeCode).FirstOrDefault();
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.ESILocation = validator.ValidateEsiLocation(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), esiLocationList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.ESILocation == Guid.Empty)
                            {
                                if (error.Count > 0) { error.RemoveAt(error.Count - 1); }
                                employee.ESILocation = AddNewMasterValues("ESI Location", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.ESILocation != Guid.Empty)
                                {
                                    esiLocationList.Add(new EsiLocation { LocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.ESILocation });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[ESILocation] =  '" + employee.ESILocation.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.ESILocation = existEmp.ESILocation;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("ESI Location is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "esi despensary")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.ESIDespensary = validator.ValidateEsiDespensaryList(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), esiDespensaryList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.ESIDespensary == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.ESIDespensary = AddNewMasterValues("ESI Despensary", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.ESIDespensary != Guid.Empty)
                                {
                                    esiDespensaryList.Add(new EsiDespensary { ESIDespensary = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.ESIDespensary });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[ESIDespensary]= '" + employee.ESIDespensary.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.ESIDespensary = existEmp.ESIDespensary;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("ESI Despensary is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "pt location")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.PTLocation = validator.ValidatePTLocation(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ptLocationList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.PTLocation == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.PTLocation = AddNewMasterValues("PT Location", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.PTLocation != Guid.Empty)
                                {
                                    ptLocationList.Add(new PTLocation { PTLocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.PTLocation });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PTLocation]= '" + employee.PTLocation.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.PTLocation = existEmp.PTLocation;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("PT Location is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "branch")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employee.Branch = validator.ValidateBranch(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), branchlist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (employee.Branch == Guid.Empty)
                            {
                                if (error.Count > 0)
                                { error.RemoveAt(error.Count - 1); }
                                employee.Branch = AddNewMasterValues("Branch", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, u.MappedColumnName, importTbl.MappedSheet);
                                if (employee.Branch != Guid.Empty)
                                {
                                    branchlist.Add(new Branch { BranchName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), Id = employee.Branch });
                                }
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Branch] =  '" + employee.Branch.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existEmp, null))
                            {
                                employee.Branch = existEmp.Branch;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Branch is required");
                            }
                        }
                    }

                });

                //if (!object.ReferenceEquals(employeelist.Where(p => p.EmployeeCode == employee.EmployeeCode).FirstOrDefault(), null) || object.ReferenceEquals(employeelist.Where(p => p.EmployeeCode == employee.EmployeeCode).FirstOrDefault(), null))
                //{
                if (importTbl.IsAmendment)
                {
                    employee.ImportOption = "ImportEmployeeUpdate";
                    string tempQuery = "UPDATE Employee SET";
                    Query = tempQuery + Query + ",[ModifiedBy] =  '" + employee.ModifiedBy.ToString() + "'";
                    Query = Query + ",[ModifiedOn] = GETDATE()";
                    Query = Query + "  Where Id = '" + employee.Id.ToString() + "'";
                    employee.Query = Query.Replace("SET,", "SET ");
                }
                if (!string.IsNullOrEmpty(employee.EmployeeCode))
                    newEmployeelist.Add(employee);
                else
                    error.Add("Employee Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                //}
                //else
                //{
                //    //error.Add("Category name '" + u.Name + "' already exist.");
                //}

            }
            if (error.Count <= 0)
            {
                newEmployeelist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "importemployeeupdate")
                    {
                        if (!u.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee '" + u.EmployeeCode + "'");
                        }
                    }
                    else
                    {
                        u.PayrollProcess = Convert.ToBoolean(1);
                        u.StopPayment = Convert.ToBoolean(0);
                        u.Status = 1;
                        if (!u.Save())
                        {
                            //MailSendEmployee mailsendemployee = new MailSendEmployee();
                            //Guid employeeId = u.Id;
                            //mailsendemployee.SendMailToEmpSave(employeeId);
                            error.Add("There is some error While intracting with database for the employee '" + u.EmployeeCode + "'");
                        }
                    }
                });
            }
            return error;
        }

        public List<string> importEmployeeTraining(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            string Query = string.Empty;
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeTrainingList newemployeetraininglist = new EmployeeTrainingList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EmployeeTraining employeeTraining = new EmployeeTraining();
                employeeTraining.CreatedBy = userId;
                employeeTraining.ModifiedBy = userId;
                Employee existingTran = new Employee();
                EmployeeTraining emptrn = new EmployeeTraining();
                importTbl.ImportColumns.ForEach(u =>
                {

                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeeTraining.EmployeeId = tmp.Id;

                                existingTran = employeeList.Where(p => p.Id == employeeTraining.EmployeeId).FirstOrDefault();
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[EmployeeId] =  '" + employeeTraining.EmployeeId.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "training name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeTraining.TrainingName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[TrainingName] =  '" + employeeTraining.TrainingName.ToString() + "'" : "";
                            if (!object.ReferenceEquals(existingTran, null))
                            {
                                emptrn = existingTran.EmployeeTrainingList.Where(s => s.TrainingName == employeeTraining.TrainingName).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Training Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "training from")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeTraining.TrainingDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employeeTraining.TrainingDate = employeeTraining.TrainingDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employeeTraining.TrainingDate;
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[TrainingDate] = dbo.Datetimechk( '" + employeeTraining.TrainingDate.ToString("dd/MMM/yyyy") + "')" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(emptrn, null))
                            {
                                employeeTraining.TrainingDate = emptrn.TrainingDate;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Training From is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "training to")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeTraining.TrainingTo = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employeeTraining.TrainingTo = employeeTraining.TrainingTo == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employeeTraining.TrainingTo;
                            Query = importTbl.IsAmendment == true ? Query + ",[TrainingTo] = dbo.Datetimechk( '" + employeeTraining.TrainingTo.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(emptrn, null))
                            {
                                employeeTraining.TrainingTo = emptrn.TrainingTo;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Training To is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "certificate number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeTraining.CertificateNumber = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[CertificateNumber] =  '" + employeeTraining.CertificateNumber.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(emptrn, null))
                            {
                                employeeTraining.CertificateNumber = emptrn.CertificateNumber;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Certificate Number is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "institute")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeTraining.Institute = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[Institute] =  '" + employeeTraining.Institute.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(emptrn, null))
                            {
                                employeeTraining.Institute = emptrn.Institute;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Institute is required");
                            }
                        }
                    }

                });
                if (employeeTraining.EmployeeId != Guid.Empty)
                {
                    if (!object.ReferenceEquals(existingTran, null))
                    {

                        if (object.ReferenceEquals(emptrn, null))
                        {
                            if (!string.IsNullOrEmpty(employeeTraining.TrainingName))
                                newemployeetraininglist.Add(employeeTraining);
                            else
                                error.Add("Employee Training is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                employeeTraining.Id = emptrn.Id;
                                string tempQuery = "update Emp_Training set";
                                Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeTraining.ModifiedBy.ToString() + "'";
                                Query = Query + ",[ModifiedOn] = GETDATE()";
                                Query = Query + "  Where Id = '" + employeeTraining.Id.ToString() + "'";
                                employeeTraining.Query = Query.Replace("set,", "set ");
                                employeeTraining.ImportOption = "EmployeeTraining";
                                if (!string.IsNullOrEmpty(employeeTraining.TrainingName))
                                    newemployeetraininglist.Add(employeeTraining);
                            }
                            employeeList.Where(p => p.Id == employeeTraining.EmployeeId).FirstOrDefault().EmployeeTrainingList.Add(employeeTraining);
                        }
                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeetraininglist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "employeetraining")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee Training'" + u.TrainingName + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.TrainingName))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Training'" + u.TrainingName + "'");
                            }
                        }
                    }
                });
            }

            return error;
        }
        public List<string> importEmployeeFamily(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeFamilyList newemployeefamilyList = new EmployeeFamilyList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EmployeeFamily employeeFamily = new EmployeeFamily();
                employeeFamily.CreatedBy = userId;
                employeeFamily.ModifiedBy = userId;
                EmployeeFamily existingFamily = new EmployeeFamily();
                Employee emp = new Employee();
                string relationshipColumnName = string.Empty;
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";

                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeeFamily.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeFamily.Name = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[Name] =  '" + employeeFamily.Name.ToString() + "'" : "";
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingFamily = emp.EmployeeFamilyList.Where(f => f.Name == employeeFamily.Name).FirstOrDefault();
                            }

                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeFamily.Address = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[Address] =  '" + employeeFamily.Address.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingFamily, null))
                            {
                                employeeFamily.Address = existingFamily.Address;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Address is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "relationship")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            relationshipColumnName = u.MappedColumnName;
                            employeeFamily.RelationShip = validator.GetRelationShip(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[RelationShip] =  '" + employeeFamily.RelationShip.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingFamily, null))
                            {
                                employeeFamily.RelationShip = existingFamily.RelationShip;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("RelationShip is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "date of birth")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeFamily.DateOfBirth = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employeeFamily.DateOfBirth = employeeFamily.DateOfBirth == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employeeFamily.DateOfBirth;
                            Query = importTbl.IsAmendment == true ? Query + ",[DateOfBirth] = dbo.Datetimechk( '" + employeeFamily.DateOfBirth.ToString("dd/MMM/yyyy") + "')" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingFamily, null))
                            {
                                employeeFamily.DateOfBirth = existingFamily.DateOfBirth;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Date Of Birth is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "age")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeFamily.Age = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (relationshipColumnName != "")
                            {
                                employeeFamily.RelationShip = validator.GetRelationShip(Convert.ToString(xlValue.Rows[rowCount][relationshipColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                if (employeeFamily.RelationShip != 3)
                                    validator.ValidateMinValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MinVal, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Age] =  '" + employeeFamily.Age.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingFamily, null))
                            {
                                employeeFamily.Age = existingFamily.Age;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Age is required");
                            }
                        }
                    }
                });
                if (employeeFamily.EmployeeId != Guid.Empty)
                {
                    var t1 = employeeList.Where(p => p.Id == employeeFamily.EmployeeId).FirstOrDefault();
                    if (!object.ReferenceEquals(t1, null))
                    {
                        if (object.ReferenceEquals(existingFamily, null))
                        {
                            if (!string.IsNullOrEmpty(employeeFamily.Name))
                                newemployeefamilyList.Add(employeeFamily);
                            else
                                error.Add("Employee Family is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {

                            if (importTbl.IsAmendment)
                            {
                                employeeFamily.Id = existingFamily.Id;
                                string tempQuery = "update Emp_Family set";
                                Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeFamily.ModifiedBy.ToString() + "'";
                                Query = Query + ",[ModifiedOn] = GETDATE()";
                                Query = Query + "  Where Id = '" + employeeFamily.Id.ToString() + "'";
                                employeeFamily.Query = Query.Replace("set,", "set ");
                                employeeFamily.ImportOption = "EmployeeFamily";
                                newemployeefamilyList.Add(employeeFamily);
                            }
                            employeeList.Where(p => p.Id == employeeFamily.EmployeeId).FirstOrDefault().EmployeeFamilyList.Add(employeeFamily);
                        }
                    }
                }
            }

            if (error.Count <= 0)
            {
                newemployeefamilyList.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "employeefamily")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee family'" + u.Name + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.Name))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee family'" + u.Name + "'");
                            }
                        }
                    }
                });
            }
            return error;
        }
        public List<string> importEmployeeAcademic(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeAcademicList newemployeeAcademicList = new EmployeeAcademicList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EmployeeAcademic employeeAcademic = new EmployeeAcademic();
                EmployeeAcademic exisitngAcademic = new EmployeeAcademic();
                Employee emp = new Employee();
                employeeAcademic.CreatedBy = userId;
                employeeAcademic.ModifiedBy = userId;
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();

                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeeAcademic.EmployeeId = emp.Id;

                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }

                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "degree name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAcademic.DegreeName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                exisitngAcademic = emp.EmployeeAcademicList.Where(a => a.DegreeName == employeeAcademic.DegreeName).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[DegreeName] =  '" + employeeAcademic.DegreeName.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Degree Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "institution name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAcademic.InstitionName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[InstitionName] =  '" + employeeAcademic.InstitionName.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(exisitngAcademic, null))
                            {
                                employeeAcademic.InstitionName = exisitngAcademic.InstitionName;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Institution Name is required");
                            }
                        }

                    }

                    else if (u.Name.Trim().ToLower() == "year of passing")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAcademic.YearOfPassing = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            validator.ValidateMinValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MinVal, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[YearOfPassing] =  '" + employeeAcademic.YearOfPassing.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(exisitngAcademic, null))
                            {
                                employeeAcademic.InstitionName = exisitngAcademic.InstitionName;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Year Of Passing is required");
                            }
                        }
                    }

                });
                if (employeeAcademic.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(exisitngAcademic, null))
                        {
                            if (!string.IsNullOrEmpty(employeeAcademic.DegreeName))
                                newemployeeAcademicList.Add(employeeAcademic);
                            else
                                error.Add("Employee Academic is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                exisitngAcademic.Id = exisitngAcademic.Id;
                                string tempQuery = "update Emp_Academic set";
                                Query = tempQuery + Query + ",[ModifiedBy] =  '" + exisitngAcademic.ModifiedBy.ToString() + "'";
                                Query = Query + ",[ModifiedOn] = GETDATE()";
                                Query = Query + "  Where Id = '" + exisitngAcademic.Id.ToString() + "'";
                                exisitngAcademic.Query = Query.Replace("set,", "set ");
                                exisitngAcademic.ImportOption = "EmployeeAcademic";
                                newemployeeAcademicList.Add(employeeAcademic);
                            }
                            employeeList.Where(p => p.Id == employeeAcademic.EmployeeId).FirstOrDefault().EmployeeAcademicList.Add(employeeAcademic);
                        }
                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeeAcademicList.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "employeeacademic")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee '" + u.DegreeName + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.DegreeName))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Academic'" + u.DegreeName + "'");
                            }
                        }
                    }
                });
            }
            return error;
        }
        public List<string> importEmployeeLanguage(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            Language lan = new Language();
            //List<Language> languages = lan.GetLanguages();
            Language lang = new PayrollBO.Language();
            List<Language> languages = lang.LanguagesList(companyId);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeLanguageKnownList newemployeelanguageknownlist = new EmployeeLanguageKnownList();
            Employee emp = new Employee();
            EmployeeLanguageKnown existingLang = new EmployeeLanguageKnown();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EmployeeLanguageKnown employeeLanguagesknown = new EmployeeLanguageKnown();
                employeeLanguagesknown.CreatedBy = userId;
                employeeLanguagesknown.ModifiedBy = userId;
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeeLanguagesknown.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "language name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeLanguagesknown.LanguageId = validator.ValidateLanguage(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref languages, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, companyId, userId);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingLang = emp.EmployeeLanguageKnownList.Where(a => a.LanguageId == employeeLanguagesknown.LanguageId).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[LanguageId] =  '" + employeeLanguagesknown.LanguageId.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Language Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "can speak")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeLanguagesknown.IsSpeak = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IsSpeak] =  '" + employeeLanguagesknown.IsSpeak.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingLang, null))
                            {
                                employeeLanguagesknown.IsSpeak = existingLang.IsSpeak;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Can Speak is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "can read")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeLanguagesknown.IsRead = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IsRead] =  '" + employeeLanguagesknown.IsRead.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingLang, null))
                            {
                                employeeLanguagesknown.IsRead = existingLang.IsRead;
                            }

                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Can Read is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "can write")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeLanguagesknown.IsWrite = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IsWrite] =  '" + employeeLanguagesknown.IsWrite.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingLang, null))
                            {
                                employeeLanguagesknown.IsWrite = existingLang.IsWrite;
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Can Write is required");
                            }
                        }
                    }

                });
                if (employeeLanguagesknown.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(existingLang, null))
                        {
                            if (employeeLanguagesknown.LanguageId != 0)
                                newemployeelanguageknownlist.Add(employeeLanguagesknown);
                            else
                                error.Add("Employee Language is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                employeeLanguagesknown.Id = employeeLanguagesknown.Id;
                                string tempQuery = "update Emp_LanguageKnown set";
                                Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeLanguagesknown.ModifiedBy.ToString() + "'";
                                Query = Query + ",[ModifiedOn] = GETDATE()";
                                Query = Query + "  Where Id = '" + employeeLanguagesknown.Id.ToString() + "'";
                                employeeLanguagesknown.Query = Query.Replace("set,", "set ");
                                employeeLanguagesknown.ImportOption = "EmpLanguageKnown";
                                newemployeelanguageknownlist.Add(employeeLanguagesknown);
                            }
                            employeeList.Where(p => p.Id == employeeLanguagesknown.EmployeeId).FirstOrDefault().EmployeeLanguageKnownList.Add(employeeLanguagesknown);
                        }
                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeelanguageknownlist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "emplanguageknown")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee '" + u.Language + "'");
                        }
                    }
                    else
                    {
                        if (u.LanguageId != 0)
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Language'" + u.Language + "'");
                            }
                        }
                    }
                });

            }
            return error;
        }

        public List<string> importEmployeeAddress(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            //Query = "update Emp_Address set ";
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeAddressList newemployeeAddressList = new EmployeeAddressList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee emp = new Employee();
                EmployeeAddress existAddr = new EmployeeAddress();
                EmployeeAddress employeeAddress = new EmployeeAddress();
                employeeAddress.CreatedBy = userId;
                employeeAddress.ModifiedBy = userId;
                employeeAddress.AddressType = 1;
                employeeAddress.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeeAddress.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address type")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.AddressType = validator.GetAddressType(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existAddr = emp.EmployeeAddressList.Where(s => s.AddressType == employeeAddress.AddressType).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[AddressType] =  '" + employeeAddress.AddressType.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Address Type is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address line1")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.AddressLine1 = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[AddressLine1] =  '" + employeeAddress.AddressLine1.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.AddressLine1 = existAddr.AddressLine1;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Address Line1 is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address line2")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.AddressLine2 = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[AddressLine2] =  '" + employeeAddress.AddressLine2.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.AddressLine2 = existAddr.AddressLine2;
                            }

                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Address Line2 is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "city")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.City = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[City] =  '" + employeeAddress.City.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateAlphaOnly(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.City = existAddr.City;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("City is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "state")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.State = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[State] =  '" + employeeAddress.State.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.State = existAddr.State;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("State is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "country")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.Country = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Country] =  '" + employeeAddress.Country.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.Country = existAddr.Country;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Country is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "pin code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.PinCode = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PinCode] =  '" + employeeAddress.PinCode.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.PinCode = existAddr.PinCode;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Pin Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "phone")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeAddress.Phone = validator.ValidatePhoneNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "2");
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Phone] =  '" + employeeAddress.Phone.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existAddr, null))
                            {
                                employeeAddress.Phone = existAddr.Phone;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Phone is required");
                            }
                        }
                    }



                });
                if (employeeAddress.EmployeeId != Guid.Empty)
                {
                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (importTbl.IsAmendment)
                        {
                            employeeAddress.Id = existAddr.Id;
                            employeeAddress.Id = employeeAddress.Id;
                            string tempQuery = "update Emp_Address set";
                            Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeAddress.ModifiedBy.ToString() + "'";
                            Query = Query + ",[ModifiedOn] = GETDATE()";
                            Query = Query + "  Where Id = '" + employeeAddress.Id.ToString() + "'";
                            employeeAddress.Query = Query.Replace("set,", "set ");
                            employeeAddress.ImportOption = "EmpAddress";
                            newemployeeAddressList.Add(employeeAddress);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(employeeAddress.AddressLine1))
                                newemployeeAddressList.Add(employeeAddress);
                            else
                                error.Add("Employee Address is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                        }
                        employeeList.Where(p => p.Id == employeeAddress.EmployeeId).FirstOrDefault().EmployeeAddressList.Add(employeeAddress);

                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeeAddressList.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "empaddress")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee Address line1'" + u.AddressLine1 + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.AddressLine1))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Address line1'" + u.AddressLine1 + "'");
                            }
                        }
                    }
                });
            }
            return error;
        }

        public List<string> importEmployeeNominee(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeNomineeList newemployeeNomineeList = new EmployeeNomineeList();

            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee emp = new Employee();
                EmployeeNominee existingNominee = new EmployeeNominee();
                EmployeeNominee employeenominee = new EmployeeNominee();
                employeenominee.CreatedBy = userId;
                employeenominee.ModifiedBy = userId;
                string Query = string.Empty;
                employeenominee.ImportOption = "";
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeenominee.EmployeeId = tmp.Id;
                                emp = employeeList.Where(p => p.Id == employeenominee.EmployeeId).FirstOrDefault();
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "nominee name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.NomineeName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingNominee = emp.EmployeeNomineeList.Where(s => s.NomineeName.ToLower() == employeenominee.NomineeName.ToLower()).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Nominee Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.Address = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);

                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Address] =  '" + existingNominee.Address.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.Address = existingNominee.Address;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Address is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "relation ship")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.RelationShip = validator.GetRelationShip(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[RelationShip] =  '" + existingNominee.RelationShip.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.RelationShip = existingNominee.RelationShip;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Relation Ship is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "dateofbirth")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.DateOfBirth = validator.ValidateDate((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employeenominee.DateOfBirth = employeenominee.DateOfBirth == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employeenominee.DateOfBirth;
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[DateOfBirth] =  '" + existingNominee.DateOfBirth.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.DateOfBirth = existingNominee.DateOfBirth;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("DateOfBirth is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "dmount percentage")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.AmountPercentage = Convert.ToInt32(validator.ValidateMoney((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName));
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[AmountPercentage] =  '" + existingNominee.AmountPercentage.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.AmountPercentage = existingNominee.AmountPercentage;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Amount Percentage is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "age")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.Age = validator.ValidateNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            validator.ValidateMinValue(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MinVal, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Age] =  '" + existingNominee.Age.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.Age = existingNominee.Age;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Age is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "name of guardian and address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeenominee.NameOfGuardianAndAddress = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[NameOfGuardianAndAddress] =  '" + existingNominee.NameOfGuardianAndAddress.ToString() + "'" : "";
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingNominee, null))
                            {
                                existingNominee.NameOfGuardianAndAddress = existingNominee.NameOfGuardianAndAddress;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Name Of Guardian And Address is required");
                            }
                        }
                    }


                });
                if (employeenominee.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(existingNominee, null))
                        {
                            if (!string.IsNullOrEmpty(employeenominee.NomineeName))
                                newemployeeNomineeList.Add(employeenominee);
                            else
                                error.Add("Employee Nominee is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                employeenominee.Id = existingNominee.Id;
                                if (!string.IsNullOrEmpty(employeenominee.NomineeName))
                                {
                                    string tempQuery = "update Emp_Nominee set";
                                    Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeenominee.ModifiedBy.ToString() + "'";
                                    Query = Query + ",[ModifiedOn] = GETDATE()";
                                    Query = Query + "  Where Id = '" + employeenominee.Id.ToString() + "'";
                                    employeenominee.Query = Query.Replace("set,", "set ");
                                    employeenominee.ImportOption = "EmployeeNominee";
                                    newemployeeNomineeList.Add(employeenominee);
                                }
                            }
                            employeeList.Where(p => p.Id == employeenominee.EmployeeId).FirstOrDefault().EmployeeNomineeList.Add(employeenominee);
                        }
                    }
                }

            }
            if (error.Count <= 0)
            {
                newemployeeNomineeList.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "employeenominee")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee Nominee line1'" + u.NomineeName + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.NomineeName))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Language'" + u.NomineeName + "'");
                            }
                        }
                    }
                });
            }
            return error;
        }
        public List<string> importEmployeeBenefitComponent(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            Employee emp = new Employee();
            EmployeeBenefitComponent existingComp = new EmployeeBenefitComponent();
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeBenefitComponentList newemployeebenefitcomponentlist = new EmployeeBenefitComponentList();
            List<keyValueItem> benefitComponet = new List<keyValueItem>();
            EntityModel entitModel = new EntityModel(ComValue.SalaryTable, companyId);
            entitModel.EntityAttributeModelList.ForEach(p =>
            {
                if (p.AttributeModel.IsReimbursement)
                {
                    benefitComponet.Add(new keyValueItem() { Id = p.AttributeModel.Id, Name = p.AttributeModel.Name, DisplayName = p.AttributeModel.DisplayAs });
                }
            });
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EmployeeBenefitComponent employeebenefitcomponent = new EmployeeBenefitComponent();
                employeebenefitcomponent.CreatedBy = userId;
                employeebenefitcomponent.ModifiedBy = userId;
                employeebenefitcomponent.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeebenefitcomponent.EmployeeId = tmp.Id;
                                emp = employeeList.Where(p => p.Id == employeebenefitcomponent.EmployeeId).FirstOrDefault();
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "component name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebenefitcomponent.BenefitComponentId = validator.ValidateBenefitComponent((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), benefitComponet, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);/// Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingComp = emp.EmployeeBenefitComponentList.Where(s => s.BenefitComponentId == employeebenefitcomponent.BenefitComponentId).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[BenefitComponentId] =  '" + employeebenefitcomponent.BenefitComponentId.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Component Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "amount")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebenefitcomponent.Amount = validator.ValidateMoney((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[Amount] =  '" + employeebenefitcomponent.Amount.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingComp, null))
                            {
                                employeebenefitcomponent.Amount = existingComp.Amount;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Amount is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "effective date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebenefitcomponent.EffectiveDate = validator.ValidateDate((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            employeebenefitcomponent.EffectiveDate = employeebenefitcomponent.EffectiveDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : employeebenefitcomponent.EffectiveDate;
                            Query = importTbl.IsAmendment == true ? Query + ",[EffectiveDate] =  '" + employeebenefitcomponent.EffectiveDate.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingComp, null))
                            {
                                employeebenefitcomponent.EffectiveDate = existingComp.EffectiveDate;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Effective Date is required");
                            }
                        }
                    }


                });
                if (employeebenefitcomponent.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(existingComp, null))
                        {
                            if (employeebenefitcomponent.BenefitComponentId != Guid.Empty)
                                newemployeebenefitcomponentlist.Add(employeebenefitcomponent);
                            else
                                error.Add("Employee Benefit Component is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                if (employeebenefitcomponent.BenefitComponentId != Guid.Empty)
                                {
                                    employeebenefitcomponent.Id = existingComp.Id;
                                    string tempQuery = "update Emp_BenefitComponent set";
                                    Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeebenefitcomponent.ModifiedBy.ToString() + "'";
                                    Query = Query + ",[ModifiedOn] = GETDATE()";
                                    Query = Query + "  Where Id = '" + employeebenefitcomponent.Id.ToString() + "'";
                                    employeebenefitcomponent.Query = Query.Replace("set,", "set ");
                                    employeebenefitcomponent.ImportOption = "Emp_Benefit";
                                    newemployeebenefitcomponentlist.Add(employeebenefitcomponent);
                                }

                            }
                            employeeList.Where(p => p.Id == employeebenefitcomponent.EmployeeId).FirstOrDefault().EmployeeBenefitComponentList.Add(employeebenefitcomponent);
                        }
                    }
                }

            }
            if (error.Count <= 0)
            {
                newemployeebenefitcomponentlist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "Emp_benefit")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee Benefit Component'" + u.BenefitComponentId + "'");
                        }
                    }
                    else
                    {
                        if (u.BenefitComponentId != Guid.Empty)
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Benefit Component'" + u.BenefitComponentId + "'");
                            }
                        }
                    }
                });
            }
            return error;
        }

        public List<string> importEmployeeEmergencyContact(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeEmegencyContactList newemployeeemergencycontactlist = new EmployeeEmegencyContactList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee emp = new Employee();
                EmployeeEmegencyContact existingCont = new EmployeeEmegencyContact();
                EmployeeEmegencyContact employeeemergencycontact = new EmployeeEmegencyContact();
                employeeemergencycontact.CreatedBy = userId;
                employeeemergencycontact.ModifiedBy = userId;
                employeeemergencycontact.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeeemergencycontact.EmployeeId = tmp.Id;
                                emp = employeeList.Where(p => p.Id == employeeemergencycontact.EmployeeId).FirstOrDefault();
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "contact name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemergencycontact.ContactName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingCont = emp.EmployeeEmegencyContactList.Where(s => s.ContactName == employeeemergencycontact.ContactName).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[ContactName] =  '" + employeeemergencycontact.ContactName.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Contact Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "contact number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemergencycontact.ContactNumber = validator.ValidatePhoneNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "10").ToString();
                            Query = importTbl.IsAmendment == true ? Query + ",[ContactNumber] =  '" + employeeemergencycontact.ContactName.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingCont, null))
                            {
                                existingCont.ContactNumber = existingCont.ContactNumber;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Contact Number is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "relation ship")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemergencycontact.RelationShip = validator.GetRelationShip((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[RelationShip] =  '" + employeeemergencycontact.RelationShip.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingCont, null))
                            {
                                existingCont.RelationShip = existingCont.RelationShip;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Relation Ship is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemergencycontact.Address = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[Address] =  '" + employeeemergencycontact.Address.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingCont, null))
                            {
                                existingCont.Address = existingCont.Address;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Address is required");
                            }
                        }
                    }

                });
                if (employeeemergencycontact.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(existingCont, null))
                        {
                            if (!string.IsNullOrEmpty(employeeemergencycontact.ContactName))
                                newemployeeemergencycontactlist.Add(employeeemergencycontact);
                            else
                                error.Add("Employee Contact Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                if (!string.IsNullOrEmpty(employeeemergencycontact.ContactName))
                                {
                                    employeeemergencycontact.Id = existingCont.Id;
                                    string tempQuery = "update Emp_EmegencyContact set";
                                    Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeemergencycontact.ModifiedBy.ToString() + "'";
                                    Query = Query + ",[ModifiedOn] = GETDATE()";
                                    Query = Query + "  Where Id = '" + employeeemergencycontact.Id.ToString() + "'";
                                    employeeemergencycontact.Query = Query.Replace("set,", "set ");
                                    employeeemergencycontact.ImportOption = "EmpEmergency";
                                    newemployeeemergencycontactlist.Add(employeeemergencycontact);
                                }
                            }
                            employeeList.Where(p => p.Id == employeeemergencycontact.EmployeeId).FirstOrDefault().EmployeeEmegencyContactList.Add(employeeemergencycontact);
                        }
                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeeemergencycontactlist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "empemergency")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee Contact Name'" + u.ContactName + "'");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(u.ContactName))
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Contact Name'" + u.ContactName + "'");
                            }
                        }
                    }

                });
            }
            return error;
        }
        public List<string> importEmployeeEmployeement(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            EmployeeEmployeementList newemployeeemployeementlist = new EmployeeEmployeementList();

            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee emp = new Employee();
                EmployeeEmployeement existingEmployment = new EmployeeEmployeement();
                EmployeeEmployeement employeeemployeement = new EmployeeEmployeement();
                employeeemployeement.CreatedBy = userId;
                employeeemployeement.ModifiedBy = userId;
                employeeemployeement.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeeemployeement.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "previous employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemployeement.EmployeeCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (!object.ReferenceEquals(emp, null))
                            {
                                existingEmployment = emp.EmployeeEmployeementList.Where(s => s.CompanyName == employeeemployeement.CompanyName).FirstOrDefault();
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = Query + ",[EmployeeCode] =  '" + employeeemployeement.EmployeeCode.ToString() + "'";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Previous Employee Code is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "company name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemployeement.CompanyName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[CompanyName] =  '" + employeeemployeement.CompanyName.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingEmployment, null))
                            {
                                existingEmployment.CompanyName = existingEmployment.CompanyName;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Company Name is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "position held")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemployeement.PositionHeld = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = importTbl.IsAmendment == true ? Query + ",[PositionHeld] =  '" + employeeemployeement.PositionHeld.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingEmployment, null))
                            {
                                existingEmployment.PositionHeld = existingEmployment.PositionHeld;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Position Held is required");
                            }
                        }
                    }
                    else if (u.Name.Trim().ToLower() == "work from")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemployeement.WorkFrom = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[WorkFrom] =  '" + employeeemployeement.WorkFrom.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingEmployment, null))
                            {
                                existingEmployment.CompanyName = existingEmployment.CompanyName;
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingEmployment, null))
                            {
                                existingEmployment.WorkFrom = existingEmployment.WorkFrom;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Work From is required");
                            }
                        }
                    }

                    else if (u.Name.Trim().ToLower() == "work to")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeeemployeement.WorkTo = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[WorkTo] =  '" + employeeemployeement.WorkTo.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existingEmployment, null))
                            {
                                existingEmployment.WorkTo = existingEmployment.WorkTo;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Work To is required");
                            }
                        }
                    }
                });
                if (employeeemployeement.EmployeeId != Guid.Empty)
                {

                    if (!object.ReferenceEquals(emp, null))
                    {
                        if (object.ReferenceEquals(existingEmployment, null))
                        {
                            if (!string.IsNullOrEmpty(employeeemployeement.CompanyName))
                                newemployeeemployeementlist.Add(employeeemployeement);
                            else
                                error.Add("Employee Benefit Component is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                        }
                        else
                        {
                            if (importTbl.IsAmendment)
                            {
                                employeeemployeement.Id = existingEmployment.Id;

                                string tempQuery = "update Emp_Employeement set";
                                Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeeemployeement.ModifiedBy.ToString() + "'";
                                Query = Query + ",[ModifiedOn] = GETDATE()";
                                Query = Query + "  Where Id = '" + employeeemployeement.Id.ToString() + "'";
                                employeeemployeement.Query = Query.Replace("set,", "set ");
                                employeeemployeement.ImportOption = "Emp_Employeement";
                                if (!string.IsNullOrEmpty(employeeemployeement.CompanyName))
                                    newemployeeemployeementlist.Add(employeeemployeement);

                            }
                            employeeList.Where(p => p.Id == employeeemployeement.EmployeeId).FirstOrDefault().EmployeeEmployeementList.Add(employeeemployeement);
                        }
                    }
                }
            }
            if (error.Count <= 0)
            {
                newemployeeemployeementlist.ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.CompanyName))
                    {
                        if (u.ImportOption.ToLower().Trim() == "emp_employeement")
                        {
                            Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                            if (!update.UpdateImportEmployeeDetais())
                            {
                                error.Add("There is some error While intracting with database for the employee Benefit Component'" + u.CompanyName + "'");
                            }
                        }
                        else
                        {
                            if (!u.Save())
                            {
                                error.Add("There is some error While intracting with database for the employee Contact Name'" + u.CompanyName + "'");
                            }
                        }
                    }

                });
            }
            return error;
        }
        public List<string> importEmployeeEmployeePersonal(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<Emp_Personal> newemployeepersonallist = new List<Emp_Personal>();
            BloodGroupList bloodgroups = new BloodGroupList(true);
            BankList banklist = new BankList(companyId);
            Bank bank = new Bank();
            Emp_Bank employeebank = new Emp_Bank();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            Bank existBank = banklist.Where(p => p.BankName == bank.BankName).FirstOrDefault();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Emp_Personal employeepersonal = new Emp_Personal();
                Employee emp = new Employee();
                Emp_Personal existingPersonal = new Emp_Personal();
                employeepersonal.CreatedBy = userId;
                employeepersonal.ModifiedBy = userId;
                employeepersonal.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeepersonal.EmployeeId = emp.Id;
                                existingPersonal = emp.EmployeePersonal;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "personal mobile no")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PersonalMobileNo = validator.ValidatePhoneNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "10").ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PersonalMobileNo] =  '" + employeepersonal.PersonalMobileNo.ToString() + "'" : "";

                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Personal Mobile No is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "office mobile no")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.OfficeMobileNo = validator.ValidatePhoneNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "10").ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[OfficeMobileNo] =  '" + employeepersonal.OfficeMobileNo.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Office Mobile No is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "extension no")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.ExtensionNo = validator.ValidateExtentionNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName, "2", "5").ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[ExtensionNo] =  '" + employeepersonal.ExtensionNo.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Extension No is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "personal email")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PersonalEmail = validator.ValidateEmail(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PersonalEmail] =  '" + employeepersonal.PersonalEmail.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Personal Email is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "office email")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.OfficeEmail = validator.ValidateEmail(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[OfficeEmail] =  '" + employeepersonal.OfficeEmail.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Office Email is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "blood group")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.BloodGroup = validator.ValidateBloodGroup(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), bloodgroups, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[BloodGroup] =  '" + employeepersonal.BloodGroup.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Blood Group is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "print cheque")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PrintCheque = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PrintCheque] =  '" + employeepersonal.PrintCheque.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Print Cheque is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "senior citizen")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.IsSeniorCitizen = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IsSeniorCitizen] =  '" + employeepersonal.IsSeniorCitizen.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Senior Citizen is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "payslip remarks")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PaySlipRemarks = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PaySlipRemarks] =  '" + employeepersonal.PaySlipRemarks.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PaySlip Remarks is required");
                            }
                        }
                    }

                    if (u.Name.Trim().ToLower() == "pan number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PANNumber = validator.ValidatePAN((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PANNumber] =  '" + employeepersonal.PANNumber.ToString() + "'" : "";

                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PAN Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "pf number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PFNumber = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);//validator.ValidateNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PFNumber] =  '" + employeepersonal.PFNumber.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PF Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "esi number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.ESINumber = validator.ValidateEsiNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[ESINumber] =  '" + employeepersonal.ESINumber.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("ESI Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "marital status")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.MaritalStatus = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[MaritalStatus] =  '" + employeepersonal.MaritalStatus.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Marital Status is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "no of children")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.NoOfChildren = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[NoOfChildren] =  '" + employeepersonal.NoOfChildren.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("No of children is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "pf confirmation date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PFConfirmationDate = validator.ValidateDate((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PFConfirmationDate] =  '" + employeepersonal.PFConfirmationDate.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PF Confirmation Date is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "aadhar number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.AADHARNumber = validator.Validate12dgtno((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (employeepersonal.AADHARNumber.Length > 12)
                            {
                                error.Add("AADHAR number should be 12 digit only");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[AADHARNumber] =  '" + employeepersonal.AADHARNumber.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("AADHAR Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "pf uan")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.PFUAN = validator.Validate12dgtno((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (employeepersonal.PFUAN.Length < 12)
                            {
                                error.Add("PF UAN number should minimum 12 digits only");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[PFUAN] =  '" + employeepersonal.PFUAN.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PF UAN is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "father name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.FatherName = validator.ValidateString(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[FatherName] =  '" + employeepersonal.FatherName.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Father Name is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "spouse name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeepersonal.SpouseName = validator.ValidateString(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[SpouseName] =  '" + employeepersonal.SpouseName.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Spouse Name is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank account no")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.AcctNo = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);// Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]); ;/ validator.ValidateNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[AcctNo] =  '" + employeebank.AcctNo.ToString() + "'" : "";

                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Bank Account No is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.BankId = validator.ValidateBank(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), banklist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[BankId] =  '" + employeebank.BankId.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existBank, null))
                            {
                                employeebank.BankId = existBank.Id;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Bank Name is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "ifsc number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.IFSC = validator.ValidateAlphaNum((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (employeebank.IFSC.Length > 11)
                            {
                                error.Add("IFSC Number should be 11 digits only");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IFSC] =  '" + employeebank.IFSC.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("IFSC Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.Address = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Address] =  '" + employeebank.Address.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Bank Address is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "city")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.City = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[City] =  '" + employeebank.City.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("City is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "state")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.State = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[State] =  '" + employeebank.State.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("State is required");
                            }
                        }
                    }
                });
                if (employeepersonal.EmployeeId != Guid.Empty)
                {
                    if (importTbl.IsAmendment)
                    {
                        employeepersonal.Id = existingPersonal.Id;
                        string tempQuery = "update Emp_Personal set";
                        Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeepersonal.ModifiedBy.ToString() + "'";
                        Query = Query + ",[ModifiedOn] = GETDATE()";
                        Query = Query + "  Where Id = '" + employeepersonal.Id.ToString() + "'";
                        employeepersonal.Query = Query.Replace("set,", "set ");
                        employeepersonal.ImportOption = "Emp_Personal";
                        newemployeepersonallist.Add(employeepersonal);
                    }

                    else// (!object.ReferenceEquals(emp, null))
                    {
                        newemployeepersonallist.Add(employeepersonal);
                    }

                }

            }
            if (error.Count <= 0)
            {
                newemployeepersonallist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "emp_personal")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee personal email '" + u.PersonalEmail + "'");
                        }
                    }
                    else
                    {
                        if (!u.Save())
                        {
                            error.Add("There is some error While intracting with database for the employee personal email '" + u.PersonalEmail + "'");
                        }
                    }


                });
            }
            return error;
        }
        public List<string> importEmployeeEmployeeBankDetails(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {

            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            BloodGroupList bloodgroups = new BloodGroupList(true);
            BankList banklist = new BankList(companyId);
            Bank bank = new Bank();
            Emp_BankList newemplist = new Emp_BankList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            Bank existBank = banklist.Where(p => p.BankName == bank.BankName).FirstOrDefault();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee emp = new Employee();
                Emp_Bank employeebank = new Emp_Bank();
                employeebank.CreatedBy = userId.ToString();
                employeebank.ModifiedBy = userId.ToString();
                employeebank.ImportOption = "";
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                employeebank.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank branch")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.BranchName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[BranchName] =  '" + employeebank.BranchName.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Bank Branch is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank account no")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.AcctNo = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);// Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]); ;/ validator.ValidateNumber((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName).ToString();
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[AcctNo] =  '" + employeebank.AcctNo.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Bank Account No is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.BankId = validator.ValidateBank(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), banklist, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            Query = importTbl.IsAmendment == true ? Query + ",[BankId] =  '" + employeebank.BankId.ToString() + "'" : "";
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])) && importTbl.IsAmendment && !object.ReferenceEquals(existBank, null))
                            {
                                employeebank.BankId = existBank.Id;
                            }
                        }
                        else
                        {
                            if (u.IsRequired && !importTbl.IsAmendment)
                            {
                                error.Add("Bank Name is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "ifsc number")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.IFSC = validator.ValidateAlphaNum((Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName).ToString();
                            if (employeebank.IFSC.Length > 11)
                            {
                                error.Add("IFSC Number should be 11 digits only");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[IFSC] =  '" + employeebank.IFSC.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("IFSC Number is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "bank address")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.Address = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[Address] =  '" + employeebank.Address.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Bank Address is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "city")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.City = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[City] =  '" + employeebank.City.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("City is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "state")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            employeebank.State = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            Query = importTbl.IsAmendment == true ? Query + ",[State] =  '" + employeebank.State.ToString() + "'" : "";
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("State is required");
                            }
                        }
                    }
                });
                if (employeebank.EmployeeId != Guid.Empty)
                {
                    if (importTbl.IsAmendment)
                    {
                        string tempQuery = "update Emp_Bank set";
                        Query = tempQuery + Query + ",[ModifiedBy] =  '" + employeebank.ModifiedBy.ToString() + "'";
                        Query = Query + ",[ModifiedOn] = GETDATE()";
                        Query = Query + "  Where Id = '" + employeebank.Id.ToString() + "'";
                        employeebank.Query = Query.Replace("set,", "set ");
                        employeebank.ImportOption = "Emp_Bank";
                        newemplist.Add(employeebank);
                    }
                    else
                    // if (!object.ReferenceEquals(emp, null))
                    {
                        newemplist.Add(employeebank);
                    }

                }
            }
            if (error.Count <= 0)
            {
                newemplist.ForEach(u =>
                {
                    if (u.ImportOption.ToLower().Trim() == "emp_personal")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database for the employee personal email '" + u.AcctNo + "'");
                        }
                    }
                    else
                    {
                        if (!u.Save())
                        {
                            error.Add("There is some error While intracting with database for the employee personal email '" + u.AcctNo + "'");
                        }

                    }

                });
            }
            return error;
        }

        #region Popup Import

        public List<string> importCategory(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            CategoryList categorylist = new CategoryList(companyId);
            CategoryList newCategorylist = new CategoryList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Category category = new Category();
                category.CompanyId = companyId;
                category.CreaateBy = userId;
                category.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";

                    if (u.Name.Trim().ToLower() == "display order")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            category.DisOrder = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (!object.ReferenceEquals(categorylist.Where(p => p.DisOrder == category.DisOrder).FirstOrDefault(), null))
                            {
                                error.Add("Display Order'" + category.DisOrder + "' already exist row at " + rowCount + " in the Sheet of " + importTbl.MappedSheet);
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "category name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            category.Name = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (!object.ReferenceEquals(categorylist.Where(p => p.Name.ToLower().Trim() == category.Name.ToLower().Trim()).FirstOrDefault(), null))
                            {
                                error.Add("Category name '" + category.Name + "' already exist row at " + rowCount + " in the Sheet of " + importTbl.MappedSheet);
                            }
                        }
                    }
                    if (object.ReferenceEquals(categorylist.Where(p => p.Name.ToLower().Trim() == category.Name.ToLower().Trim()).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(category.Name))
                            newCategorylist.Add(category);
                    }
                });
                if (error.Count == 0)
                {
                    categorylist.Add(category);
                }
            }
            if (error.Count <= 0)
            {
                newCategorylist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Category '" + u.Name + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importBranch(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            BranchList branchlist = new BranchList(companyId);
            BranchList newBranchlist = new BranchList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Branch branch = new Branch();
                branch.CompanyId = companyId;
                branch.CreatedBy = userId;
                branch.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "branch name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            branch.BranchName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Branch Name is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(branchlist.Where(p => p.BranchName == branch.BranchName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(branch.BranchName))
                            newBranchlist.Add(branch);
                        else
                            error.Add("Branch Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    branchlist.Add(branch);
                });
            }
            if (error.Count <= 0)
            {
                newBranchlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Branch '" + u.BranchName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importCostCentre(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            CostCentreList costcentrelist = new CostCentreList(companyId);
            CostCentreList newCostcentrelist = new CostCentreList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                CostCentre costcentre = new CostCentre();
                costcentre.CompanyId = companyId;
                costcentre.CreatedBy = userId;
                costcentre.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "cost centre")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            costcentre.CostCentreName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Cost Centre is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(costcentrelist.Where(p => p.CostCentreName == costcentre.CostCentreName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(costcentre.CostCentreName))
                            newCostcentrelist.Add(costcentre);
                        else
                            error.Add("Cost Centre is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    costcentrelist.Add(costcentre);
                });
            }
            if (error.Count <= 0)
            {
                newCostcentrelist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Cost Centre '" + u.CostCentreName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importDepartment(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            DepartmentList departmentlist = new DepartmentList(companyId);
            DepartmentList newDepartmentlist = new DepartmentList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Department department = new Department();
                department.CompanyId = companyId;
                department.CreatedBy = userId;
                department.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "department name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            department.DepartmentName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Department Name is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(departmentlist.Where(p => p.DepartmentName == department.DepartmentName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(department.DepartmentName))
                            newDepartmentlist.Add(department);
                        else
                            error.Add("Department Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    departmentlist.Add(department);
                });
            }
            if (error.Count <= 0)
            {
                newDepartmentlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Department '" + u.DepartmentName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importDesignation(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            DesignationList designationlist = new DesignationList(companyId);
            DesignationList newdesignationlist = new DesignationList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Designation designation = new Designation();
                designation.CompanyId = companyId;
                designation.CreatedBy = userId;
                designation.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "designation")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            designation.DesignationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Designation is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(designationlist.Where(p => p.DesignationName == designation.DesignationName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(designation.DesignationName))
                            newdesignationlist.Add(designation);
                        else
                            error.Add("Designation is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    designationlist.Add(designation);
                });
            }
            if (error.Count <= 0)
            {
                newdesignationlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Designation '" + u.DesignationName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importESIDespensary(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            ESIDespensaryList despensarylist = new ESIDespensaryList(companyId);
            ESIDespensaryList newdespensarylist = new ESIDespensaryList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EsiDespensary esidespensary = new EsiDespensary();
                esidespensary.CompanyId = companyId;
                esidespensary.CreatedBy = userId;
                esidespensary.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "esi despensary")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            esidespensary.ESIDespensary = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("ESI Despensary is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(despensarylist.Where(p => p.ESIDespensary == esidespensary.ESIDespensary).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(esidespensary.ESIDespensary))
                            newdespensarylist.Add(esidespensary);
                        else
                            error.Add("Location is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    despensarylist.Add(esidespensary);
                });
            }
            if (error.Count <= 0)
            {
                newdespensarylist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the ESI Despensary '" + u.ESIDespensary + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importESILocation(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            EsiLocationList esilocationlist = new EsiLocationList(companyId);
            EsiLocationList newdesignationlist = new EsiLocationList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                EsiLocation esilocation = new EsiLocation();
                esilocation.CompanyId = companyId;
                esilocation.CreatedBy = userId;
                esilocation.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "esi location")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            esilocation.LocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("ESI Location is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "applicable")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            esilocation.isApplicable = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            //esilocation.isApplicable = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]));
                            //    if (u.IsRequired)
                            //    {
                            //        validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                            //    }
                            //    validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                            //}
                            //else
                            //{
                            //    if (u.IsRequired)
                            //    {
                            //        error.Add("ESI Location is required");
                            //    }
                            //}
                        }
                    }
                    if (u.Name.Trim().ToLower() == "employer code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            esilocation.EmployerCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employer Code is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(esilocationlist.Where(p => p.LocationName == esilocation.LocationName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(esilocation.LocationName))
                            esilocationlist.Add(esilocation);
                        else
                            error.Add("ESI Location is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    esilocationlist.Add(esilocation);
                });
            }
            if (error.Count <= 0)
            {
                esilocationlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the ESI Location '" + u.LocationName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importGrade(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            GradeList gradelist = new GradeList(companyId);
            GradeList newgradelist = new GradeList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Grade grade = new Grade();
                grade.CompanyId = companyId;
                grade.CreatedBy = userId;
                grade.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            grade.GradeName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Name is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(gradelist.Where(p => p.GradeName == grade.GradeName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(grade.GradeName))
                            newgradelist.Add(grade);
                        else
                            error.Add("Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    gradelist.Add(grade);
                });
            }
            if (error.Count <= 0)
            {
                newgradelist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Grade '" + u.GradeName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importHRComponent(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            HRComponentList hrcomponentlist = new HRComponentList(companyId);
            HRComponentList newhrcomponentlist = new HRComponentList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                HRComponent hrcomponent = new HRComponent();
                hrcomponent.CompanyId = companyId;
                hrcomponent.CreatedBy = userId;
                hrcomponent.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "component name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            hrcomponent.Name = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Component Name is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(hrcomponentlist.Where(p => p.Name == hrcomponent.Name).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(hrcomponent.Name))
                            newhrcomponentlist.Add(hrcomponent);
                        else
                            error.Add("Component Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    hrcomponentlist.Add(hrcomponent);
                });
            }
            if (error.Count <= 0)
            {
                newhrcomponentlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the HR Component '" + u.Name + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importJoiningDocument(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            JoiningDocumentList joiningdocumentlist = new JoiningDocumentList(companyId);
            JoiningDocumentList newjoiningdocumentlist = new JoiningDocumentList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                JoiningDocument joiningdocument = new JoiningDocument();
                joiningdocument.CompanyId = companyId;
                joiningdocument.CreatedBy = userId;
                joiningdocument.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "document name")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            joiningdocument.DocumentName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Document Name is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(joiningdocumentlist.Where(p => p.DocumentName == joiningdocument.DocumentName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(joiningdocument.DocumentName))
                            newjoiningdocumentlist.Add(joiningdocument);
                        else
                            error.Add("Document Name is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    joiningdocumentlist.Add(joiningdocument);
                });
            }
            if (error.Count <= 0)
            {
                newjoiningdocumentlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Joining Document '" + u.DocumentName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importLocation(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            LocationList locationlist = new LocationList(companyId);
            LocationList newlocationlist = new LocationList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Location location = new Location();
                location.CompanyId = companyId;
                location.CreatedBy = userId;
                location.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "location")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            location.LocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Location is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(locationlist.Where(p => p.LocationName == location.LocationName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(location.LocationName))
                            newlocationlist.Add(location);
                        else
                            error.Add("Location is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    locationlist.Add(location);
                });
            }
            if (error.Count <= 0)
            {
                newlocationlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Location '" + u.LocationName + "'");
                    }

                });
            }
            return error;
        }
        public List<string> importPTLocation(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            PTLocationList ptlocationlist = new PTLocationList(companyId);
            PTLocationList newptlocationlist = new PTLocationList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                PTLocation ptlocation = new PTLocation();
                ptlocation.CompanyId = companyId;
                ptlocation.CreatedBy = userId;
                ptlocation.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "pt location")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            ptlocation.PTLocationName = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("PT Location is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(ptlocationlist.Where(p => p.PTLocationName == ptlocation.PTLocationName).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(ptlocation.PTLocationName))
                            newptlocationlist.Add(ptlocation);
                        else
                            error.Add("PT Location is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);

                    }
                    else
                    {
                        //error.Add("Category name '" + u.Name + "' already exist.");
                    }
                    ptlocationlist.Add(ptlocation);
                });
            }
            if (error.Count <= 0)
            {
                newptlocationlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the PT Location '" + u.PTLocationName + "'");
                    }

                });
            }
            return error;
        }
        #endregion
        public List<string> importMonthlyInput(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EntityModel entityModel = new EntityModel(ComValue.SalaryTable, companyId);
            EntityMapping entityMapping = new EntityMapping(ComValue.EmployeeTable, Convert.ToString(Guid.Empty), entityModel.Id);
            Guid entityId = new Guid(entityMapping.EntityId);
            EntityMappingList entityMappinglist = new EntityMappingList(ComValue.EmployeeTable);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            MonthlyInputList newMonthlyInputlist = new MonthlyInputList();
            MonthlyInputList getMonthlyInputlist = new MonthlyInputList();
            //MonthlyInputList monthlyinputlist = new MonthlyInputList(entityId, employeeId, month, year);
            //getMonthlyInputlist.mo
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                List<Guid> proccesdAttibute = new List<Guid>();
                Employee employee = new Employee();
                Guid employeeId = new Guid();
                int month = 0;
                int year = 0;
                string empCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    string Query = string.Empty;
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();

                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeeId = tmp.Id;
                                employee = tmp;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "month")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            month = validator.GetMonth(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Month is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "year")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            year = validator.ValidateYear(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Year is required");
                            }
                        }
                    }
                    if (columns.Contains(u.MappedColumnName))
                    {
                        if (u.OtherTableUniqueId != Guid.Empty)
                        {
                            var t1 = proccesdAttibute.Where(k => k == u.OtherTableUniqueId).FirstOrDefault();
                            if (object.ReferenceEquals(t1, null) || t1 == Guid.Empty)
                            {
                                proccesdAttibute.Add(u.OtherTableUniqueId);
                                DateTime dtDoj = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                                if (employee.DateOfJoining > dtDoj)//employee is not yet joined 
                                {
                                    error.Add("The Employee '" + empCode + "' is not Joined, so you can't import month input for this employee.");
                                }
                                PayrollHistory payrollHostor = new PayrollHistory(companyId, employee.Id, year, month);
                                if (payrollHostor.Status == ComValue.payrollProcessStatus[0])//check the pay roll process already done
                                {
                                    error.Add("The payroll process has been done for the the Employee '" + empCode + "', so you can't import month input for this employee.");
                                }
                                else if (true)
                                {

                                }
                                string strValue = string.Empty;


                                if (columns.Contains(u.MappedColumnName) == true)
                                {
                                    Decimal num = 0;
                                    if (!Decimal.TryParse(xlValue.Rows[rowCount][u.MappedColumnName].ToString(), out num))
                                    {
                                        error.Add("Please provide valid number at row " + (rowCount + startRow) + " and column " + u.MappedColumnName + " in the sheet of " + importTbl.MappedSheet);
                                    }
                                }

                                strValue = columns.Contains(u.MappedColumnName) == false ? "0" : Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                                var entMapping = entityMappinglist.Where(s => new Guid(s.EntityTableName) == entityModel.Id && new Guid(s.RefEntityId) == employeeId).FirstOrDefault();
                                MonthlyInput ptlocation = new MonthlyInput();
                                ptlocation.EntityModelId = entityModel.Id;
                                ptlocation.AttributeModelId = u.OtherTableUniqueId;
                                decimal inputVal = 0;

                                inputVal = Convert.ToDecimal(strValue);
                                inputVal = Math.Round(inputVal, 2);

                                ptlocation.Value = columns.Contains(u.MappedColumnName) == false ? "0" : Convert.ToString(inputVal);
                                ptlocation.EmployeeId = employeeId;
                                ptlocation.Month = month;
                                ptlocation.Year = year;
                                ptlocation.EntityModelId = entityModel.Id;
                                if (!object.ReferenceEquals(entMapping, null))
                                    ptlocation.EntityId = new Guid(entMapping.EntityId);
                                else
                                {
                                    error.Add("The Employee '" + empCode + "' is not mapped with any Salary grade.You can't import month input for this employee.");
                                }
                                                     
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                                validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                if (importTbl.IsAmendment)
                                {
                                    MonthlyInput updateMonthlyInput = new MonthlyInput();
                                    string tempQuery = "update MonthlyInput set";
                                    Query = Query + "[EntityId]= '" + ptlocation.EntityId.ToString() + "'";
                                    Query = Query + ",[EntityModelId]= '" + ptlocation.EntityModelId.ToString() + "'";
                                    Query = Query + ",[Value]= '" + ptlocation.Value.ToString() + "'";
                                    Query = tempQuery + Query + ",[ISDELETED] ='0'";
                                    Query = Query + "  Where [MONTH]= '" + month.ToString() + "'";
                                    Query = Query + "AND  [YEAR]= '" + year.ToString() + "'";
                                    Query = Query + "AND  [EmployeeId] = '" + employeeId.ToString() + "'";
                                    Query = Query + "AND  [EntityId] = '" + entityId.ToString() + "'";
                                    Query = Query + "AND  [AttributeModelId] = '" + u.OtherTableUniqueId.ToString() + "'";
                                    updateMonthlyInput.Query = Query.Replace("set,", "set ");
                                    updateMonthlyInput.ImportOption = "Emp_MonthlyInput";
                                    newMonthlyInputlist.Add(updateMonthlyInput);
                                }
                                else
                                {
                                    newMonthlyInputlist.Add(ptlocation);
                                }
                            }
                        }
                    }
                });

            }
            if (error.Count <= 0)
            {
                newMonthlyInputlist.ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.ImportOption) && u.ImportOption.ToLower().Trim() == "emp_monthlyinput")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database.");
                        }
                    }
                    else
                    {
                        if (!u.Save())
                        {
                            error.Add("There is some error While intracting with database.");
                        }
                    }
                });
            }
            return error;
        }

        public List<string> importIncrement(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EntityModel entityModel = new EntityModel(ComValue.SalaryTable, companyId);
            EntityMappingList entityMappinglist = new EntityMappingList(ComValue.EmployeeTable);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            // AttributeModelList attrmodlList = new AttributeModelList(companyId);


            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            IncrementList newIncrementlist = new IncrementList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Increment increment = new Increment();
                List<Guid> proccesdAttibute = new List<Guid>();
                Employee employee = new Employee();
                // DateTime applyOn = DateTime.Now;
                string empCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                increment.EmployeeId = tmp.Id;
                                employee = tmp;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "effective date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            increment.EffectiveDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Effective Date is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "apply month")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {

                            // applyOn = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow  - 1, u.MappedColumnName);
                            increment.ApplyMonth = validator.GetMonth(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            //  increment.ApplyYear = applyOn.Year;
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Apply Month is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "apply year")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            increment.ApplyYear = validator.ValidateYear(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Apply Year is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "before lop")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            increment.BeforeLop = validator.ValidateLop(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (increment.EffectiveDate.Day > 1)
                            {
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (increment.EffectiveDate.Day > 1)
                            {
                                if (u.IsRequired)
                                {
                                    error.Add("Before LOP is required");
                                }
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "after lop")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            increment.AfterLop = validator.ValidateLop(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (increment.EffectiveDate.Day > 1)
                            {
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (increment.EffectiveDate.Day > 1)
                            {
                                if (u.IsRequired)
                                {
                                    error.Add("After LOP is required");
                                }
                            }
                        }
                    }


                    if (u.OtherTableUniqueId != Guid.Empty)
                    {
                        var t1 = proccesdAttibute.Where(k => k == u.OtherTableUniqueId).FirstOrDefault();
                        if (object.ReferenceEquals(t1, null) || t1 == Guid.Empty)
                        {
                            proccesdAttibute.Add(u.OtherTableUniqueId);
                            if (columns.Contains(u.MappedColumnName))
                            {
                                if (employee.DateOfJoining > increment.EffectiveDate)//employee is not yet joined 
                                {
                                    error.Add("The Employee '" + empCode + "' is not Joined, so you can't import increment for this employee.");
                                }
                                if (employee.DateOfJoining > new DateTime(increment.ApplyYear, increment.ApplyMonth, 1))//employee is not yet joined 
                                {
                                    error.Add("The Employee '" + empCode + "' is not Joined, so you can't import increment for this employee.");
                                }
                                var entMapping = entityMappinglist.Where(s => new Guid(s.EntityTableName) == entityModel.Id && new Guid(s.RefEntityId) == increment.EmployeeId).FirstOrDefault();
                                IncrementDetail incrementDetails = new IncrementDetail();
                                incrementDetails.AttributeModelId = u.OtherTableUniqueId;
                                incrementDetails.NewValue = validator.ValidateMoney(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                if (object.ReferenceEquals(entMapping, null))
                                {
                                    error.Add("The Employee '" + empCode + "' is not mapped with any Salary grade.You can't import Increment for this employee.");
                                }
                                increment.IncrementDetailList.Add(incrementDetails);
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                                validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            else
                            {
                                //if (u.IsRequired)
                                //{
                                //    error.Add(u.Name + " is required");
                                //}
                            }

                        }
                    }


                });

                //Get the Old value
                IncrementList incrementlist = new IncrementList(increment.EmployeeId);
                Increment existincrement;
                if (incrementlist.Count <= 0)
                    existincrement = new Increment();
                else
                    existincrement = incrementlist[0];
                EntityMasterValueList entityMasterValus = new EntityMasterValueList(increment.EmployeeId, ComValue.EmployeeTable);
                increment.IncrementDetailList.ForEach(m =>
                {
                    m.OldValue = GetOldValueOfIncrement(m.AttributeModelId, existincrement, entityMasterValus, entityModel, increment.EmployeeId);

                });

                newIncrementlist.Add(increment);

            }
            if (error.Count <= 0)
            {
                newIncrementlist.ForEach(u =>
                {
                    u.CreatedBy = userId;
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database.");
                    }
                    else
                    {
                        u.IncrementDetailList.ForEach(c =>
                        {
                            c.CreatedBy = userId;
                            c.IncrementId = u.Id;
                            if (!c.Save())
                            {
                                error.Add("There is some error While intracting with database.");
                            }

                        });
                    }

                });
            }
            return error;
        }
        public List<string> importEmployeeAdditionalInfo(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            //EntityModel entitfyModel = new EntityModel(ComValue.SalaryTable, companyId);
            AttributeModelList AttributeModeList = new AttributeModelList(companyId);
            EntityModel entitfyModel = new EntityModel(ComValue.EMPADDLINFOTable, companyId);
            EntityMappingList entityMappinglist = new EntityMappingList(ComValue.EmployeeTable);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            EntityAdditionalInfoList newAdditionalInfolist = new EntityAdditionalInfoList();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                
                List<Guid> proccesdAttibute = new List<Guid>();
                Employee employee = new Employee();
                string empCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    EntityAdditionalInfo AdditionalInfo = new EntityAdditionalInfo();
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employee = tmp;
                                AdditionalInfo.EmployeeId = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.OtherTableUniqueId != Guid.Empty)
                    {
                        var t1 = proccesdAttibute.Where(k => k == u.OtherTableUniqueId).FirstOrDefault();
                        if (object.ReferenceEquals(t1, null) || t1 == Guid.Empty)
                        {
                            proccesdAttibute.Add(u.OtherTableUniqueId);
                            if (columns.Contains(u.MappedColumnName))
                            {
                                AttributeModel AttributeModel = AttributeModeList.Where(d => d.Name == u.Name).FirstOrDefault();
                                if (AttributeModel.RefEntityModelId != Guid.Empty)
                                {
                                    EntityList Entitylist = new EntityList(AttributeModel.RefEntityModelId);
                                    var CheckList = Entitylist.Where(d => d.Name == Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName])).ToList();
                                    if (CheckList.Count > 0)
                                    {
                                        AdditionalInfo.RefEntityId = CheckList[0].Id;
                                    }
                                    else
                                    {
                                        if (importTbl.AddMasterValue)
                                        {
                                            AdditionalInfo.RefEntityId = AddNewMasterValues("Entity", Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, companyId, userId, rowCount + startRow - 1, Convert.ToString(AttributeModel.RefEntityModelId), importTbl.MappedSheet, 0);
                                        }
                                        else
                                        {
                                            error.Add("Employee code '" + empCode + " " + u.MappedColumnName + " : " + Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]) + " doest not exist in master");
                                        }
                                    }
                                }
                                else
                                {
                                    AdditionalInfo.RefEntityId = Guid.Empty;
                                }
                                AdditionalInfo.AttributeModelId = u.OtherTableUniqueId;
                                AdditionalInfo.EntityModelId = entitfyModel.Id;
                                AdditionalInfo.CompanyId = entitfyModel.CompanyId;
                                AdditionalInfo.EmployeeId = employee.Id;
                                AdditionalInfo.Value = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                                validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                                //Add to list because every column as to insert into a row.
                                newAdditionalInfolist.Add(AdditionalInfo);
                            }
                            else
                            {
                                if (u.IsRequired)
                                {
                                    error.Add(u.Name + " is required");
                                }
                            }

                        }
                    }

                });




                //payrollHistory.Status = "Imported";
                //var entMapping = entityMappinglist.Where(s => new Guid(s.EntityTableName) == entityModel.Id && new Guid(s.RefEntityId) == payrollHistory.EmployeeId).FirstOrDefault();
                //if (!object.ReferenceEquals(entMapping, null))
                //    payrollHistory.EntityId = new Guid(entMapping.EntityId);
                //else
                //{
                //    error.Add("The Employee '" + empCode + "' is not mapped with any Salary grade.You can't import Past salary for this employee.");
                //}

            }
            if (error.Count <= 0)
            {
                newAdditionalInfolist.ForEach(u =>
                {
                    u.CreatedBy = userId;
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database.");
                    }                   

                });
            }
            return error;
        }

        public List<string> importSalary(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EntityModel entityModel = new EntityModel(ComValue.SalaryTable, companyId);
            EntityMappingList entityMappinglist = new EntityMappingList(ComValue.EmployeeTable);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            PayrollHistoryList newPayrollHistorylist = new PayrollHistoryList();
            ///testing///
            AttributeModelList attrModelList = new AttributeModelList(companyId);
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                PayrollHistory payrollHistory = new PayrollHistory();
                List<Guid> proccesdAttibute = new List<Guid>();
                Employee employee = new Employee();
                int month = 0;
                int year = 0;
                string empCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (u.Name == "CSR")
                    {

                    }
                    if (u.MappedColumnName == "LD")
                    {

                    }
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        //var curr_emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                        //var entitymasterValueslist = new EntityMasterValueList(curr_emp.Id, ComValue.EmployeeTable).Where(s => s.EntityModelId == entityModel.Id).ToList();

                        if (columns.Contains(u.MappedColumnName))
                        {
                            empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employee = tmp;
                                payrollHistory.EmployeeId = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "month")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            month = validator.GetMonth(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Month is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "year")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            year = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Year is required");
                            }
                        }
                    }
                    if (u.OtherTableUniqueId != Guid.Empty)
                    {
                        var t1 = proccesdAttibute.Where(k => k == u.OtherTableUniqueId).FirstOrDefault();
                        if (object.ReferenceEquals(t1, null) || t1 == Guid.Empty)
                        {
                            proccesdAttibute.Add(u.OtherTableUniqueId);
                            if (columns.Contains(u.MappedColumnName))
                            {
                                PayrollHistoryValue payrollHistoryValue = new PayrollHistoryValue();
                                payrollHistoryValue.AttributeModelId = u.OtherTableUniqueId;
                                payrollHistoryValue.Value = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                                payrollHistory.PayrollHistoryValueList.Add(payrollHistoryValue);
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                                validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            else
                            {
                                if (u.IsRequired)
                                {
                                    // error.Add(u.Name + " is required");
                                }
                            }

                        }
                    }


                });
                if (month != 0 && year != 0)
                {

                    DateTime dtDoj = new DateTime(year, month, DateTime.DaysInMonth(year, month));


                    if (employee.DateOfJoining > dtDoj)//employee is not yet joined 
                    {
                        error.Add("The Employee '" + empCode + "' is not Joined, so you can't import Past Salary for this employee.");
                    }
                }
                PayrollHistory payrollHostor = new PayrollHistory(companyId, employee.Id, year, month);
                if (payrollHostor.Status == ComValue.payrollProcessStatus[0])//check the pay roll process already done
                {
                    error.Add("The payroll process has been done for the the Employee '" + empCode + "', so you can't import Past Salary for this employee.");
                }
                payrollHistory.CompanyId = companyId;
                payrollHistory.Month = month;
                payrollHistory.Year = year;
                payrollHistory.EmployeeId = employee.Id;
                payrollHistory.EntityModelId = entityModel.Id;
                payrollHistory.Status = "Imported";
                var entMapping = entityMappinglist.Where(s => new Guid(s.EntityTableName) == entityModel.Id && new Guid(s.RefEntityId) == payrollHistory.EmployeeId).FirstOrDefault();
                if (!object.ReferenceEquals(entMapping, null))
                    payrollHistory.EntityId = new Guid(entMapping.EntityId);
                else
                {
                    error.Add("The Employee '" + empCode + "' is not mapped with any Salary grade.You can't import Past salary for this employee.");
                }
                newPayrollHistorylist.Add(payrollHistory);


            }
            if (error.Count <= 0)
            {
                newPayrollHistorylist.ForEach(u =>
                {
                    u.CreatedBy = userId;
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database.");
                    }
                    else
                    {
                        u.PayrollHistoryValueList.ForEach(c =>
                        {
                            //save monthly input data
                            if (new AttributeModel(c.AttributeModelId, companyId).IsMonthlyInput == true)
                            {
                                MonthlyInput monInp = new MonthlyInput();
                                monInp.EmployeeId = u.EmployeeId;
                                monInp.EntityId = u.EntityId;
                                monInp.EntityModelId = u.EntityModelId;
                                monInp.Month = u.Month;
                                monInp.Year = u.Year;
                                monInp.AttributeModelId = c.AttributeModelId;
                                monInp.Value = c.Value;
                                monInp.Save();
                            }

                            c.PayrollHistoryId = u.Id;
                            c.CreatedBy = userId;
                            if (!c.Save())
                            {
                                error.Add("There is some error While intracting with database.");
                            }

                        });
                    }

                });
            }
            return error;
        }

        public List<string> importSalaryMaster(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EntityModel entityModel = new EntityModel(ComValue.SalaryTable, companyId);
            CategoryList comp = new CategoryList(companyId);
            EntityList entityList = new EntityList(entityModel.Id);
            EntityMappingList entityMappinglist = new EntityMappingList(ComValue.EmployeeTable);
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            EntityMappingList newEntityMappinglist = new EntityMappingList();
            EntityMasterValueList newEntityMasterValuelist = new EntityMasterValueList();
            List<EntityBehaviorList> proccesdEntityBehaviorList = new List<EntityBehaviorList>();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                List<Guid> proccesdAttibute = new List<Guid>();
                Employee employee = new Employee();
                Guid catagoryid = new Guid();
                Guid employeeId = new Guid();
                Guid mappedEnityId = Guid.Empty;
                string empCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    string Query = string.Empty;
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();

                            if (!object.ReferenceEquals(tmp, null))
                            {
                                employeeId = tmp.Id;
                                catagoryid = tmp.CategoryId;
                                employee = tmp;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "salary grade")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            var temp = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = entityList.Where(enl => enl.Name.ToLower() == temp.ToLower()).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                var catagorycheck = comp.Where(cm1 => cm1.Id == catagoryid).FirstOrDefault();
                                if (!object.ReferenceEquals(catagorycheck, null))
                                {


                                    mappedEnityId = tmp.Id;
                                    var entMapping = entityMappinglist.Where(s => new Guid(s.EntityTableName) == entityModel.Id && new Guid(s.RefEntityId) == employeeId && new Guid(s.EntityId) == tmp.Id).FirstOrDefault();
                                    if (object.ReferenceEquals(entMapping, null))
                                    {
                                        var newEntiMap = new EntityMapping()
                                        {
                                            EntityId = tmp.Id.ToString(),
                                            EntityTableName = entityModel.Id.ToString(),
                                            RefEntityId = employee.Id.ToString(),
                                            RefEntityModelId = ComValue.EmployeeTable,
                                            IsPhysicalEntity = false
                                        };
                                        newEntityMappinglist.Add(newEntiMap);
                                        entityMappinglist.Add(newEntiMap);

                                        //var salarymasterset = entityMappinglist.Where(enm => enm.RefEntityId == employeeId.ToString()).FirstOrDefault();
                                        //EntityMapping entityMap = new EntityMapping();
                                        //entityMap.EntityId = salarymasterset.EntityId.ToString();
                                        //entityMap.EntityTableName = salarymasterset.EntityTableName.ToString();
                                        //entityMap.IsPhysicalEntity = false;
                                        //entityMap.RefEntityId = salarymasterset.RefEntityId.ToString();
                                        //entityMap.RefEntityModelId = salarymasterset.RefEntityModelId.ToString();
                                        //entityMap.Save();


                                    }
                                }
                                else
                                {
                                    error.Add("Please Update the PF-setting In the Settings");
                                }
                            }
                            else
                            {
                                //error.Add("The Employee '" + empCode + "' is not mapped with any Salary grade.You can't import salary master for this employee.");
                                error.Add("Please Create a Field For" + "'" + temp + "'" + "In Dynamic Group");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Salary Grade is required");
                            }
                        }
                    }
                    if (u.OtherTableUniqueId != Guid.Empty)
                    {
                        var t1 = proccesdAttibute.Where(k => k == u.OtherTableUniqueId).FirstOrDefault();
                        if (object.ReferenceEquals(t1, null) || t1 == Guid.Empty)
                        {
                            proccesdAttibute.Add(u.OtherTableUniqueId);
                            if (columns.Contains(u.MappedColumnName))
                            {
                                if (object.ReferenceEquals(proccesdEntityBehaviorList.Where(ad => ad.EntityId == mappedEnityId).FirstOrDefault(), null))
                                {
                                    EntityBehaviorList entityBehaviorList = new EntityBehaviorList(mappedEnityId, entityModel.Id);
                                    proccesdEntityBehaviorList.Add(entityBehaviorList);
                                }
                                Decimal num = 0;
                                if (!Decimal.TryParse(xlValue.Rows[rowCount][u.MappedColumnName].ToString(), out num))
                                {
                                    error.Add("Please provide valid number at row " + (rowCount + startRow) + " and column " + u.MappedColumnName + " in the sheet of " + importTbl.MappedSheet);
                                }
                                var temp = proccesdEntityBehaviorList.Where(sd => sd.EntityId == mappedEnityId).FirstOrDefault().Where(eb => eb.AttributeModelId == u.OtherTableUniqueId);
                                if (!object.ReferenceEquals(temp, null))
                                {
                                    EntityMasterValue entityMaser = new EntityMasterValue();
                                    entityMaser.AttributeModelId = u.OtherTableUniqueId;
                                    entityMaser.CreatedBy = userId;
                                    entityMaser.ModifiedBy = userId;
                                    entityMaser.EntityId = mappedEnityId;
                                    entityMaser.EntityModelId = entityModel.Id;
                                    entityMaser.RefEntityId = employee.Id;
                                    entityMaser.RefEntityModelId = ComValue.EmployeeTable;
                                    entityMaser.Value = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                                    if (importTbl.IsAmendment)
                                    {                                       
                                        string tempQuery = "update EntityMasterValue set";
                                        Query = Query + "[EntityId]= '" + entityMaser.EntityId.ToString() + "'";
                                        Query = Query + ",[EntityModelId]= '" + entityMaser.EntityModelId.ToString() + "'";
                                        Query = Query + ",[Value]= '" + entityMaser.Value.ToString() + "'";
                                        Query = Query + ",[ModifiedBy]= '" + entityMaser.ModifiedBy.ToString() + "'";
                                        Query = Query + ",[ModifiedOn] = GETDATE()";
                                        Query = tempQuery + Query;
                                        Query = Query + "  Where [EntityId]= '" + entityMaser.EntityId.ToString() + "'";
                                        Query = Query + "AND  [AttributeModelId]= '" + entityMaser.AttributeModelId.ToString() + "'";
                                        Query = Query + "AND  [RefEntityId] = '" + entityMaser.RefEntityId.ToString() + "'";
                                        Query = Query + "AND  [RefEntityModelId] = '" + entityMaser.RefEntityModelId.ToString() + "'";
                                        entityMaser.Query = Query.Replace("set,", "set ");
                                        entityMaser.ImportOption = "Emp_MasterInput";
                                        newEntityMasterValuelist.Add(entityMaser);
                                    }
                                    else
                                    {
                                        newEntityMasterValuelist.Add(entityMaser);
                                    }                                 
                                    
                                }
                                if (u.IsRequired)
                                {
                                    validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                                }
                                validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            else
                            {
                                //if (u.IsRequired)
                                //{
                                //    error.Add(u.Name + " is required");
                                //}
                            }

                        }
                    }


                });
            }
            if (error.Count <= 0)
            {
                newEntityMappinglist.ForEach(p =>
                {
                    if (!p.Save())
                    {
                        error.Add("There is some error While intracting with database.");
                    }

                });
                newEntityMasterValuelist.ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.ImportOption) && u.ImportOption.ToLower().Trim() == "emp_masterinput")
                    {
                        Employee update = new Employee(); update.Id = u.Id; update.ImportOption = u.ImportOption; update.Query = u.Query;
                        if (!update.UpdateImportEmployeeDetais())
                        {
                            error.Add("There is some error While intracting with database.");
                        }
                    }
                    else
                    {
                        if (!u.Save())
                        {
                            error.Add("There is some error While intracting with database.");
                        }
                    }
                });
            }
            return error;
        }

        public List<string> importLoanMaster(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            AttributeModelList attributeModelList = new AttributeModelList(companyId);
            AttributeModelList newattributeModelList = new AttributeModelList();

            LoanMasterList loanMasterlist = new LoanMasterList(companyId);
            LoanMasterList newloanMasterlist = new LoanMasterList();

            AttributeModelTypeList attributeModelTypeList = new AttributeModelTypeList(companyId);

            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                AttributeModel attributeModel = new AttributeModel();
                attributeModel.CompanyId = companyId;
                attributeModel.CreatedBy = userId;
                attributeModel.ModifiedBy = userId;
                attributeModel.AttributeModelTypeId = attributeModelTypeList.Where(u => u.Name == "Deduction").FirstOrDefault().Id;
                attributeModel.RefEntityModelId = Guid.Empty;
                attributeModel.DataType = "Number";
                attributeModel.DataSize = 0;
                attributeModel.IsMandatory = false;
                attributeModel.OrderNumber = 0;
                attributeModel.IsTransaction = false;
                attributeModel.IsFilter = true;
                attributeModel.IsIncludeForGrossPay = true;
                attributeModel.IsMonthlyInput = false;
                attributeModel.IsTaxable = false;
                attributeModel.IsIncrement = false;
                attributeModel.IsReimbursement = false;
                attributeModel.FullAndFinalSettlement = false;
                attributeModel.IsInstallment = true;
                attributeModel.IsDefault = false;
                attributeModel.BehaviorType = "Deduction";
                attributeModel.IsSetting = false;
                attributeModel.ParentId = Guid.Empty;
                attributeModel.ContributionType = 1;

                LoanMaster loanMaster = new LoanMaster();
                loanMaster.CompanyId = companyId;
                loanMaster.CreatedBy = userId;
                loanMaster.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "loan code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanMaster.LoanCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            loanMaster.AttributeModelId = validator.ValidateLoanMaster(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), attributeModelList, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (loanMaster.AttributeModelId == Guid.Empty)
                            {
                                attributeModel.Name = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Loan Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "loan description")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            attributeModel.DisplayAs = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            loanMaster.LoanDesc = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Loan Description is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "isinterest")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanMaster.IsInterest = validator.GetTruORFalse(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Interest is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "interest percentage")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanMaster.InterestPercent = validator.ValidateLop(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Interest is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(loanMasterlist.Where(p => p.LoanCode == loanMaster.LoanCode).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(loanMaster.LoanCode))
                        {
                            newattributeModelList.Add(attributeModel);
                            newloanMasterlist.Add(loanMaster);
                        }
                        else
                            error.Add("Loan Code is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                    }
                    else
                    {

                    }
                    attributeModelList.Add(attributeModel);
                    loanMasterlist.Add(loanMaster);
                });
            }
            if (error.Count <= 0)
            {
                newattributeModelList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanMaster '" + u.DisplayAs + "'");
                    }

                });

                newloanMasterlist.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanMaster '" + u.LoanCode + "'");
                    }

                });

            }
            return error;
        }
        public List<string> importLoanEntry(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            LoanMasterList loanMasterlist = new LoanMasterList(companyId);
            LoanEntryList loanEntryList = new LoanEntryList();
            LoanEntryList newloanEntryList = new LoanEntryList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                LoanEntry loanEntry = new LoanEntry();
                loanEntry.CreatedBy = userId;
                loanEntry.ModifiedBy = userId;
                loanEntry.CompanyId = companyId;
                string loanMasterCode = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                loanEntry.EmployeeId = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }

                    if (u.Name.Trim().ToLower() == "loanmaster code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanMasterCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = loanMasterlist.Where(p => p.LoanCode == loanMasterCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                loanEntry.LoanMasterId = tmp.Id;
                            }
                            else
                            {
                                error.Add("LoanMaster Code '" + loanMasterCode + "' is not matched with any LoanMaster");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("LoanMaster Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "loan date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanEntry.LoanDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            LoanEntryList loanEntrylistall = new LoanEntryList(loanEntry.EmployeeId);
                            var tmpLoanCheck = loanEntrylistall.Where(p => p.LoanMasterId == loanEntry.LoanMasterId && p.EmployeeId == loanEntry.EmployeeId && p.LoanDate == loanEntry.LoanDate).FirstOrDefault();
                            if (!object.ReferenceEquals(tmpLoanCheck, null))
                            {
                                error.Add("Loan already taken on this LoanDate of the Loan Code & Employee row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Loan Date is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "apply date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanEntry.ApplyDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Apply Date is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "loan amount")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanEntry.LoanAmt = validator.ValidateMoney(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Loan Amount is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "no of months")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanEntry.NoOfMonths = validator.ValidateNumber(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            var tmp = loanMasterlist.Where(p => p.LoanCode == loanMasterCode).FirstOrDefault();
                            if (tmp.IsInterest && !object.ReferenceEquals(tmp, null))
                            {
                                loanEntry.AmtPerMonth = Convert.ToDecimal(loanEntry.EMICalculation(loanEntry.NoOfMonths, Convert.ToDouble(loanEntry.LoanAmt), tmp.InterestPercent));
                            }
                            else
                                loanEntry.AmtPerMonth = loanEntry.LoanAmt / loanEntry.NoOfMonths;

                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("NoOf Months is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(loanMasterlist.Where(p => p.Id == loanEntry.LoanMasterId).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(loanEntry.LoanMasterId)))
                        {
                            newloanEntryList.Add(loanEntry);
                        }
                        else
                            error.Add("Loan Code is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                    }
                    else
                    {

                    }
                    loanEntryList.Add(loanEntry);
                });

            }
            if (error.Count <= 0)
            {
                newloanEntryList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanEntry");
                    }

                });
            }
            return error;

        }
        public List<string> importEmployeeSeperation(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            List<object> jsonObject = new List<object>();
            DataColumnCollection columns = xlValue.Columns;
            List<Employee> jsonSepraList = new List<Employee>();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee jsonSepra = new Employee();
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            jsonSepra.EmployeeCode = empCode;
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                jsonSepra.Id = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "type of separation")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            jsonSepra.TypeOfSeparation = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Type Of Separation is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "last working date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            jsonSepra.SeparationDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            jsonSepra.LastWorkingDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Last Working Date is required");
                            }
                        }
                    }

                    if (u.Name.Trim().ToLower() == "reason")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            jsonSepra.SeparationReason = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Reason is required");
                            }
                        }
                    }
                });
                jsonSepra.ReleaseDate = DateTime.MinValue;
                jsonSepra.Status = 0;
                jsonSepra.CreatedBy = userId;
                jsonSepra.ModifiedBy = userId;
                jsonSepra.CompanyId = companyId;
                jsonSepra.DBConnectionId = Convert.ToInt32(HttpContext.Current.Session["DBConnectionId"]);
                jsonSepra.ImportOption = "EmployeeImport";
                jsonSepraList.Add(jsonSepra);
            }

            if (error.Count <= 0)
            {
                jsonSepraList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanEntry");
                    }

                });
            }
            return error;
        }

        public List<string> importEmployeeRelease(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            List<object> jsonObject = new List<object>();
            DataColumnCollection columns = xlValue.Columns;
            List<Employee> jsonSepraList = new List<Employee>();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee jsonSepra = new Employee();
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            jsonSepra.EmployeeCode = empCode;
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                jsonSepra.Id = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "release date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            jsonSepra.ReleaseDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Release Date is required");
                            }
                        }
                    }
                    jsonSepra.SeparationDate = DateTime.MinValue;
                    jsonSepra.LastWorkingDate = DateTime.MinValue;
                    jsonSepra.Status = 1;
                    jsonSepra.CreatedBy = userId;
                    jsonSepra.ModifiedBy = userId;
                    jsonSepra.CompanyId = companyId;
                    jsonSepra.DBConnectionId = Convert.ToInt32(HttpContext.Current.Session["DBConnectionId"]);
                    jsonSepra.ImportOption = "EmployeeImport";
                    jsonSepraList.Add(jsonSepra);
                });
            }

            if (error.Count <= 0)
            {
                jsonSepraList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanEntry");
                    }

                });
            }
            return error;
        }
        public List<string> importLoanTransaction(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            LoanMasterList loanMasterlist = new LoanMasterList(companyId);
            LoanEntryList loanEntryList = new LoanEntryList();
            LoanTransactionList loanTransactionList = new LoanTransactionList();
            LoanTransactionList newloanTransactionList = new LoanTransactionList();
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                LoanEntry loanEntry = new LoanEntry();
                LoanTransaction loanTransaction = new LoanTransaction();
                loanTransaction.CreatedBy = userId;
                loanTransaction.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                loanEntry.EmployeeId = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }

                    if (u.Name.Trim().ToLower() == "loanmaster code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string loanMasterCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            var tmp = loanMasterlist.Where(p => p.LoanCode == loanMasterCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                loanEntry.LoanMasterId = tmp.Id;
                            }
                            else
                            {
                                error.Add("LoanMaster Code '" + loanMasterCode + "' is not matched with any LoanMaster");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("LoanMaster Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "loan date")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            loanEntry.LoanDate = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]).Replace('.', '/').Replace('-', '/'), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            LoanEntryList loanEntrylistall = new LoanEntryList(loanEntry.EmployeeId);
                            var tmpLoan = loanEntrylistall.Where(p => p.LoanMasterId == loanEntry.LoanMasterId && p.EmployeeId == loanEntry.EmployeeId && p.LoanDate == loanEntry.LoanDate).FirstOrDefault();
                            if (!object.ReferenceEquals(tmpLoan, null))
                            {
                                loanTransaction.LoanEntryId = tmpLoan.Id;
                                loanTransaction.AmtPaid = tmpLoan.AmtPerMonth;
                                loanEntry.LoanDate = tmpLoan.LoanDate;
                                loanEntry.LoanAmt = tmpLoan.LoanAmt;
                                loanEntry.NoOfMonths = tmpLoan.NoOfMonths;

                            }
                            else
                            {
                                error.Add("Loan Date '" + loanEntry.LoanDate + "' is not matched with any LoanEntry of the Employee row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Loan Date is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "month of installment")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            DateTime MonthofInstallment = validator.ValidateDate(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);

                            LoanTransactionList loanTransactionListall = new LoanTransactionList(loanEntry.Id);
                            var tmp = loanMasterlist.Where(p => p.Id == loanEntry.LoanMasterId).FirstOrDefault();
                            var tmpLoantranscheck = loanTransactionListall.Where(v => v.AppliedOn.Month == MonthofInstallment.Month && v.AppliedOn.Year == MonthofInstallment.Year);
                            if (loanTransactionListall.Count <= loanEntry.NoOfMonths && object.ReferenceEquals(tmpLoantranscheck, null))
                            {
                                if (tmp.IsInterest)
                                {
                                    loanTransaction.InterestAmt = Convert.ToDecimal(loanEntry.InterestAmtCalculation(Convert.ToDouble(loanEntry.LoanAmt), Convert.ToDouble(loanTransaction.AmtPaid), tmp.InterestPercent, loanEntry.NoOfMonths, loanEntry.LoanDate, MonthofInstallment));
                                }
                                else
                                {
                                    loanTransaction.InterestAmt = 0;
                                }
                                loanTransaction.isForClose = true;
                                loanTransaction.isPayRollProcess = true;
                            }
                            else
                            {
                                error.Add("Please provide Valid Month of Installment of the Employee row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Apply Date is required");
                            }
                        }
                    }

                    if (object.ReferenceEquals(loanMasterlist.Where(p => p.Id == loanEntry.LoanMasterId).FirstOrDefault(), null))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(loanEntry.LoanMasterId)))
                        {
                            newloanTransactionList.Add(loanTransaction);
                        }
                        else
                            error.Add("LoanMaster Code is empty row at " + rowCount + startRow + " in the Sheet of " + importTbl.MappedSheet);
                    }
                    else
                    {

                    }
                    loanEntryList.Add(loanEntry);
                });

            }
            if (error.Count <= 0)
            {
                newloanTransactionList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the LoanEntry");
                    }

                });
            }
            return error;

        }
        public List<string> importTaxDeclaration(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            List<string> error = new List<string>();
            DataColumnCollection columns = xlValue.Columns;
            TXSection sectionlist = new TXSection();
            TXSectionList section = new TXSectionList(companyId);
            Employee emp = new Employee();
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);

            TXEmployeeSectionList newList = new TXEmployeeSectionList();


            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                int month = 0;
                int year = 0;
                TXEmployeeSection EmpSection = new TXEmployeeSection();

                EmpSection.CreatedBy = userId;
                EmpSection.ModifiedBy = userId;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (u.Name.Trim().ToLower() == "employee id")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            emp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                EmpSection.EmployeeId = emp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("EMPLOYEE ID ID is required");
                            }
                        }
                    }



                    if (u.Name.Trim().ToLower() == "sextion id")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            sectionlist = section.Where(p => p.Name == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(sectionlist, null))
                            {
                                EmpSection.SectionId = sectionlist.Id;
                            }
                            else
                            {
                                error.Add("Session code '" + empCode + "' is not matched with any name");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("SEXTION ID ID is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "yearly value")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            EmpSection.ApprovedValue = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("YEARLY VALUE ID ID is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "MONTH")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            month = Convert.ToInt32(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("MODIFIED ON ID ID is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "year")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            year = Convert.ToInt32(xlValue.Rows[rowCount][u.MappedColumnName]);
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                            validator.ValidateMaxLength(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), u.MaxLength, ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("YEAR ID ID is required");
                            }
                        }
                    }




                });

                EmpSection.EffectiveDate = new DateTime(year, month, 1);
                newList.Add(EmpSection);
            }
            if (error.Count <= 0)
            {
                newList.ForEach(u =>
                {
                    if (!u.Save())
                    {
                        error.Add("There is some error While intracting with database for the Employee '" + u.EmployeeId + "'");
                    }

                }
                );
            }
            return error;
        }


        private decimal GetOldValueOfIncrement(Guid attributeId, Increment existincrement, EntityMasterValueList entityMasterValus, EntityModel entityModel, Guid employeeId)
        {
            //AttributeModelList attrmodlList,
            decimal returnVal = 0;
            // attrmodlList.ForEach(u =>
            // {
            var entityMasterval = entityMasterValus.Where(q => q.AttributeModelId == attributeId && q.EntityModelId == entityModel.Id).FirstOrDefault();
            // if (u.IsIncrement)
            // {
            var temp = existincrement.IncrementDetailList.Where(p => p.AttributeModelId == attributeId).FirstOrDefault();
            if (!object.ReferenceEquals(temp, null))
            {
                returnVal = temp.NewValue;
            }
            else if (!object.ReferenceEquals(entityMasterval, null))
            {
                returnVal = Convert.ToDecimal(entityMasterval.Value);
            }
            else
            {
                returnVal = 0;
            }
            //  }
            //  });
            return returnVal;
        }

        public static Guid AddNewMasterValues(string type, string value, ref List<string> error, int companyId, int userId, int rowno, string columnName, string sheet, int displayOrder = 0)
        {
            Guid Id;
            try
            {
                EmployeeList emplist = new EmployeeList(companyId, Guid.Empty);
                switch (type)
                {
                    case "Category":
                        Category newCategory = new Category();
                        newCategory.Name = value;
                        newCategory.CompanyId = companyId;
                        newCategory.CreaateBy = userId;
                        newCategory.DisOrder = displayOrder;
                        newCategory.Save();
                        Id = newCategory.Id;
                        break;
                    case "Branch":
                        Branch newbranch = new Branch();
                        newbranch.BranchName = value;
                        newbranch.CompanyId = companyId;
                        newbranch.CreatedBy = userId;
                        newbranch.Save();
                        Id = newbranch.Id;
                        break;
                    case "ESI Location":
                        EsiLocation newEsiLocation = new EsiLocation();
                        newEsiLocation.LocationName = value;
                        newEsiLocation.CompanyId = companyId;
                        newEsiLocation.CreatedBy = userId;
                        newEsiLocation.Save();
                        Id = newEsiLocation.Id;
                        break;
                    case "ESI Despensary":
                        EsiDespensary newEsiDespensary = new EsiDespensary();
                        newEsiDespensary.ESIDespensary = value;
                        newEsiDespensary.CompanyId = companyId;
                        newEsiDespensary.CreatedBy = userId;
                        newEsiDespensary.Save();
                        Id = newEsiDespensary.Id;
                        break;
                    case "Department":
                        Department newDepartment = new Department();
                        newDepartment.DepartmentName = value;
                        newDepartment.CompanyId = companyId;
                        newDepartment.CreatedBy = userId;
                        newDepartment.Save();
                        Id = newDepartment.Id;
                        break;
                    case "CostCentre":
                        CostCentre newCostCentre = new CostCentre();
                        newCostCentre.CostCentreName = value;
                        newCostCentre.CompanyId = companyId;
                        newCostCentre.CreatedBy = userId;
                        newCostCentre.Save();
                        Id = newCostCentre.Id;
                        break;
                    case "PT Location":
                        PTLocation newPTLocation = new PTLocation();
                        newPTLocation.PTLocationName = value;
                        newPTLocation.CompanyId = companyId;
                        newPTLocation.CreatedBy = userId;
                        newPTLocation.Save();
                        Id = newPTLocation.Id;
                        break;
                    case "Grade":
                        Grade newGrade = new Grade();
                        newGrade.GradeName = value;
                        newGrade.CompanyId = companyId;
                        newGrade.CreatedBy = userId;
                        newGrade.Save();
                        Id = newGrade.Id;
                        break;
                    case "Location":
                        Location newLocation = new Location();
                        newLocation.LocationName = value;
                        newLocation.CompanyId = companyId;
                        newLocation.CreatedBy = userId;
                        newLocation.Save();
                        Id = newLocation.Id;
                        break;
                    case "Designation":
                        Designation newDesignation = new Designation();
                        newDesignation.DesignationName = value;
                        newDesignation.CompanyId = companyId;
                        newDesignation.CreatedBy = userId;
                        newDesignation.Save();
                        Id = newDesignation.Id;
                        break;
                    case "Entity":
                        Entity newEntity = new Entity();
                        newEntity.Name = value;
                        newEntity.EntityModelId = new Guid(columnName); // Get columnName as EntityModelId 
                        newEntity.CreatedBy = userId;
                        newEntity.Save();
                        Id = newEntity.Id;
                        break;
                    default:
                        Id = Guid.Empty;
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                error.Add("Please provide valid " + type + " at " + (rowno - 1) + " and column " + columnName + " in the sheet of " + sheet);
                Id = Guid.Empty;
            }
            return Id;
        }

        public List<string> importEmployeeCodeChange(importTable importTbl, DataTable xlValue, int companyId, int userId, int startRow)
        {
            EmployeeList employeeList = new EmployeeList(companyId, userId, Guid.Empty);
            List<string> error = new List<string>();
            List<object> jsonObject = new List<object>();
            DataColumnCollection columns = xlValue.Columns;
            List<Employee> jsonUpdateList = new List<Employee>();
            for (int rowCount = 0; rowCount < xlValue.Rows.Count; rowCount++)
            {
                Employee updateEmpCode = new Employee();
                string Query = string.Empty;
                importTbl.ImportColumns.ForEach(u =>
                {
                    if (string.IsNullOrEmpty(u.MappedColumnName))
                        u.MappedColumnName = "";
                    if (u.Name.Trim().ToLower() == "employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            string empCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            updateEmpCode.EmployeeCode = empCode;


                            var tmp = employeeList.Where(p => p.EmployeeCode == empCode).FirstOrDefault();
                            if (!object.ReferenceEquals(tmp, null))
                            {
                                updateEmpCode.Id = tmp.Id;
                            }
                            else
                            {
                                error.Add("Employee code '" + empCode + "' is not matched with any employee");
                            }
                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("Employee Code is required");
                            }
                        }
                    }
                    if (u.Name.Trim().ToLower() == "new employee code")
                    {
                        if (columns.Contains(u.MappedColumnName))
                        {
                            updateEmpCode.NewEmployeeCode = Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]);
                            Query = "update Employee set ";
                            Query = importTbl.IsAmendment == true ? Query + "[EmployeeCode]='" + updateEmpCode.NewEmployeeCode.ToString() + "'" : "";

                            if (u.IsRequired)
                            {
                                validator.ValidateRequired(Convert.ToString(xlValue.Rows[rowCount][u.MappedColumnName]), ref error, importTbl.MappedSheet, rowCount + startRow - 1, u.MappedColumnName);
                            }
                        }
                        else
                        {
                            if (u.IsRequired)
                            {
                                error.Add("New Employee Code is required");
                            }
                        }
                    }
                    if (importTbl.IsAmendment)
                    {
                        updateEmpCode.Id = updateEmpCode.Id;
                        Query = Query + ",[ModifiedBy] =  '" + userId.ToString() + "'";
                        Query = Query + ",[ModifiedOn] = GETDATE()";
                        Query = Query + "WHERE ID='" + updateEmpCode.Id.ToString() + "'";
                        updateEmpCode.Query = Query;
                        updateEmpCode.ImportOption = "ImportEmployeeUpdate";
                        if (!string.IsNullOrEmpty(updateEmpCode.NewEmployeeCode))
                            jsonUpdateList.Add(updateEmpCode);
                    }
                });
            }

            if (error.Count <= 0)
            {
                jsonUpdateList.ForEach(u =>
                {
                    if (!u.UpdateImportEmployeeDetais())
                    {
                        error.Add("There is some error While intracting with database for the LoanEntry");
                    }

                });
            }
            return error;
        }
    }
    public class jsonSeparation
    {
        public Guid SepCatid { get; set; }
        public Guid SepEmpId { get; set; }
        public string SepEmpName { get; set; }
        public string SepDOJ { get; set; }
        public string SepType { get; set; }
        public string SepLWDate { get; set; }
        public string SepReason { get; set; }
        // Modified by Babu.R as on 24-Jul-2017 for Separation last working date validation
        public string SepMonthlyLastWorkingDate { get; set; }
        public string SepPayrollLastWorkingDate { get; set; }
        public string SepMonthlyDate { get; set; }
        public string SepPayrollDate { get; set; }


        public static jsonSeparation tojson(Employee employee)
        {
            return new jsonSeparation()
            {
                SepCatid = employee.CategoryId,
                SepEmpId = employee.Id,
                SepDOJ = employee.DateOfJoining != DateTime.MinValue ? employee.DateOfJoining.ToString("dd/MMM/yyyy") : "",
                SepType = employee.TypeOfSeparation,
                SepLWDate = employee.LastWorkingDate != DateTime.MinValue ? employee.LastWorkingDate.ToString("dd/MMM/yyyy") : "",
                SepReason = employee.SeparationReason,
                SepEmpName = employee.FirstName,
                // Modified by Babu.R as on 24-Jul-2017 for Separaion last working date validation
                SepMonthlyLastWorkingDate = employee.MonthlyInputLastDate != DateTime.MinValue ? employee.MonthlyInputLastDate.ToString("dd/MMM/yyyy") : "",
                SepPayrollLastWorkingDate = employee.PayrollInputLastDate != DateTime.MinValue ? employee.PayrollInputLastDate.ToString("dd/MMM/yyyy") : "",
                SepMonthlyDate = employee.MonthlyInputDate != DateTime.MinValue ? employee.MonthlyInputDate.ToString("dd/MMM/yyyy") : "",
                SepPayrollDate = employee.PayrollInputDate != DateTime.MinValue ? employee.PayrollInputDate.ToString("dd/MMM/yyyy") : ""
            };
        }
        public static Employee convertobject(jsonSeparation employee)
        {
            return new Employee()
            {
                CategoryId = employee.SepCatid,
                Id = employee.SepEmpId,
                DateOfJoining = employee.SepDOJ != string.Empty ? Convert.ToDateTime(employee.SepDOJ) : DateTime.Now,
                TypeOfSeparation = employee.SepType,
                LastWorkingDate = employee.SepLWDate != string.Empty ? Convert.ToDateTime(employee.SepLWDate) : DateTime.Now,
                SeparationReason = employee.SepReason
            };
        }

    }
}
