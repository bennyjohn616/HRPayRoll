using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayrollBO;
using TraceError;
using System.Data;
using SQLDBOperation;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;

namespace PayRollReports
{
    public class PFChallan
    {
        public PFChallan()
        {

        }
        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
       // public int Id { get; set; }

        /// <summary>
        /// Get or Set the CofigurationId
        /// </summary>
       // public int CompanyId { get; set; }



        /// <summary>
        /// Get or Set the TableName
        /// </summary>
      //  public string TableName { get; set; }

        /// <summary>
        /// Get or Set the FieldName
        /// </summary>
      //  public string ColumnName { get; set; }


        /// <summary>
        /// Get or Set the Type
        /// </summary>
        public string DisplayAs { get; set; }
        public int DisplayOrder { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
     //   public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
     //   public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
      //  public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
   //     public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifeidOn
        /// </summary>
     //   public DateTime ModifiedOn { get; set; }
        public string EmployeeCode { get; set; }

        //---
        public string BaseValue { get; set; }
        public string TableName { get; set; }

        #endregion


        public string pfchallanTxt(int month, int year, int companyId, string outFilePath, int userId)
        {
            string returnval = string.Empty;
            try
            {
                //Entity entity;                
                EmployeeList emp = new EmployeeList(companyId, userId, Guid.Empty);
                PFChallanList result = new PFChallanList();
                result = challanList(month, year, companyId, outFilePath, userId);
                string Details = string.Empty;
                StringBuilder sb = new StringBuilder();
                var empList = result.Where(x => x.ColumnName == "PFUAN" && x.Value == "0").ToList().Select(x => x.EmployeeId).ToList();
                for (int i = 0; i < empList.Count; i++)
                {
                    returnval = returnval + empList[i] + ",";
                    result.RemoveAll(x => x.EmployeeId == empList[i]);
                }
                emp.RemoveAll(e => (e.SeparationDate > DateTime.MinValue && e.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) && (e.LastWorkingDate.Month != month && e.LastWorkingDate.Year != year)));
                emp.ForEach(e =>
               {
                   bool status = false;
                   result.Where(r => r.EmployeeId == e.EmployeeCode).ToList().ForEach(r =>
                   {
                       status = true;
                       // char[] MyChar = { '#', '~', '#' };
                       // string NewString = Details.TrimEnd(MyChar);
                       //Details = Details + Convert.ToString(r.Value) +"#~#";
                       if (r.TableName.ToLower() == "salary" || r.TableName.ToLower() == "salarybase")
                       {
                           Details = Details + ((double)Math.Round((Convert.ToDouble(r.Value) + 0.01) / 1.0, 0)) * 1 + "#~#"; ;
                       }
                       else
                       {
                           if (r.DisplayOrder != 1)
                               Details = Details + Convert.ToString(r.Value) + "#~#";
                       }


                   });
                   // Modified by Keerthika on 21/04/2017
                   if (status)
                   {
                       char[] MyChar = { '#', '~', '#' };
                       string details = Details.TrimEnd(MyChar);
                       //the below line was commented inorder to remove the space between one line and other.
                       //comment by mubarak on 09/04/2018
                       //details = details + Environment.NewLine;
                       //ends here.


                       //   char[] splitchar = { '#', '~', '#' };

                       // sb.Append(Details.TrimEnd('#').TrimEnd('~').TrimEnd('#'));
                       //  sb.Append(Details.TrimEnd(new Char[] { '#', '~', '#' }));
                       sb.Append(details);
                       sb.AppendLine();
                       status = false;
                   }
                   Details = string.Empty;
               });

                File.Create(outFilePath).Dispose();
                using (TextWriter tw = new StreamWriter(outFilePath))
                {
                    tw.WriteLine(sb);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }

            return returnval;
        }
        public PFChallanList GetColumns(string tableName, int companyId)
        {
            PFChallanList columnList = new PFChallanList();
            if (tableName != ComValue.SalaryTable)
            {
                DataTable dtColumns = GetColumns(tableName);
                foreach (DataRow dr in dtColumns.Rows)
                {
                    columnList.Add(new PayrollBO.PFChallan { ColumnName = Convert.ToString(dr["COLUMN_NAME"]), DisplayAs = Convert.ToString(dr["COLUMN_NAME"]) });
                }
            }
            else
            {
                EntityModel entitModel = new EntityModel(ComValue.SalaryTable, companyId);
                entitModel.EntityAttributeModelList.ForEach(f => { columnList.Add(new PayrollBO.PFChallan { ColumnName = Convert.ToString(f.AttributeModelId), DisplayAs = f.AttributeModel.DisplayAs }); });
            }
            return columnList;
        }

        public List<PFChallan> pfchalanList(int month, int year, int companyId, string outFilePath, int userId)
        {
            List<PFChallan> pfTemplatelist = new List<PayRollReports.PFChallan>();
            EmployeeList emp = new EmployeeList(companyId, userId, Guid.Empty);
            var result = challanList(month, year, companyId, outFilePath, userId);
            emp.ForEach(e =>
            {
                result.Where(r => r.EmployeeId == e.EmployeeCode).ToList().ForEach(r =>
                {
                    PFChallan temp = new PayRollReports.PFChallan();
                    temp.DisplayAs = r.DisplayAs == null ? result.Where(x => x.DisplayOrder == r.DisplayOrder && x.DisplayAs != null).FirstOrDefault().DisplayAs : r.DisplayAs;
                    if (r.TableName == "Salary" || r.TableName == "SalaryBase")
                        temp.Value = r.Value != null ? Convert.ToString(Convert.ToDecimal(r.Value)) : "0.00";
                    else
                        temp.Value = r.Value != null ? r.Value : "0";
                    temp.EmployeeCode = r.EmployeeId;
                    temp.DisplayOrder = r.DisplayOrder;
                    temp.TableName = r.TableName;
                    pfTemplatelist.Add(temp);
                });
            });
            return pfTemplatelist;
        }

        public DataSet ESIchalanList(int month, int year, int companyId, string outFilePath, int userId)
        {
            return GetTableValues(month, year, companyId, "ESIExtractReport");
        }

        public DataSet GetESIExtractData(int month, int year, int companyId, string outFilePath, int userId)
        {
            return GetTableValues(month, year, companyId, "ESIExtractDataReport");
        }

        public PFChallanList challanList(int month, int year, int companyId, string outFilePath, int userId)
        {

            PaySlip payslip = new PaySlip();

            //   EmployeeList employees = new EmployeeList(companyId, userId, Guid.Empty);
            CategoryList categoryList = new CategoryList(companyId);
            DepartmentList deptList = new DepartmentList(companyId);
            BranchList branchList = new BranchList(companyId);
            CostCentreList costCentreList = new CostCentreList(companyId);
            DesignationList desgntionList = new DesignationList(companyId);
            GradeList gradeList = new GradeList(companyId);
            EmployeeList emp = new EmployeeList(companyId, userId, Guid.Empty);
            PayrollHistoryList payHistoryList = new PayrollHistoryList(companyId, year, month);
            PFChallanList result = new PFChallanList();
            PFChallanList template = new PFChallanList(companyId, 0);
           
            emp.RemoveAll(e => (e.SeparationDate > DateTime.MinValue && e.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) && (e.LastWorkingDate.Month!=month &&e.LastWorkingDate.Year!=year )) );
            emp.ForEach(e =>
            {
                List<PayrollError> payErrors = new List<PayrollError>();

                PayrollHistory payHistory = payHistoryList.Where(p => p.EmployeeId == e.Id).FirstOrDefault();
                if (!object.ReferenceEquals(payHistory, null))
                {

                    Emp_BankList empbank = new Emp_BankList(e.Id);
                    EmployeeAddressList empaddr = new EmployeeAddressList(e.Id);
                    Emp_Personal emppersonal = new Emp_Personal(e.Id);

                    payHistory.PayrollHistoryValueList = payHistory.PayrollHistoryValueList;
                    if (payHistory.Status == ComValue.payrollProcessStatus[0])
                    {

                        template.ForEach(r =>
                        {
                            PayrollBO.PFChallan tempvalue = new PayrollBO.PFChallan();
                            //Assign Master Values from Physical Table
                            if (r.TableName.ToLower() == "employee")
                            {
                                if (r.ColumnName == "FirstName" || r.ColumnName == "LastName")
                                {
                                    r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.ColumnName : r.DisplayAs;
                                    var type = e.GetType();
                                    string Fname = e.GetType().GetProperty("FirstName").GetValue(e, null).ToString();
                                    string Lname = e.GetType().GetProperty("LastName").GetValue(e, null).ToString();
                                    //r.Value = Fname + " " + Lname;
                                    tempvalue.DisplayAs = r.DisplayAs;
                                    tempvalue.DisplayOrder = r.DisplayOrder;
                                    tempvalue.Value = Fname + " " + Lname;
                                    tempvalue.TableName = r.TableName;
                                }
                                else
                                {
                                    r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.ColumnName : r.DisplayAs;
                                    var type = e.GetType();
                                    tempvalue.DisplayAs = r.DisplayAs;
                                    tempvalue.TableName = r.TableName;
                                    tempvalue.DisplayOrder = r.DisplayOrder;
                                    tempvalue.Value = e.GetType().GetProperty(r.ColumnName).GetValue(e, null).ToString();//r.Value = e.GetType().GetProperty(r.ColumnName).GetValue(e, null).ToString();
                                    if (e.GetType().GetProperty(r.ColumnName).PropertyType.Name == "Guid")
                                    {
                                        switch (r.ColumnName)
                                        {
                                            case "CategoryId":
                                                tempvalue.Value = categoryList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().Name;
                                                break;
                                            case "Category":
                                                tempvalue.Value = categoryList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().Name;
                                                break;
                                            case "Department":
                                                tempvalue.Value = deptList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().DepartmentName;
                                                break;
                                            case "Branch":
                                                tempvalue.Value = branchList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().BranchName;
                                                break;
                                            case "Designation":
                                                r.Value = desgntionList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().DesignationName;
                                                break;
                                            case "CostCentre":
                                                tempvalue.Value = costCentreList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().CostCentreName;
                                                break;
                                            case "Grade":
                                                tempvalue.Value = gradeList.Where(d => d.Id == new Guid(r.Value)).FirstOrDefault().GradeName;
                                                break;
                                        }
                                    }
                                }



                            }
                            if (r.TableName.ToLower() == "emp_bank")
                            {
                                r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.ColumnName : r.DisplayAs;
                                tempvalue.DisplayAs = r.DisplayAs;
                                tempvalue.TableName = r.TableName;
                                tempvalue.DisplayOrder = r.DisplayOrder;
                                if (empbank.Count > 0)
                                {
                                    var type = empbank[0].GetType();
                                    tempvalue.Value = empbank[0].GetType().GetProperty(r.ColumnName).GetValue(empbank[0], null).ToString();
                                }

                            }
                            if (r.TableName.ToLower() == "emp_personal")
                            {

                                r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.ColumnName : r.DisplayAs;
                                tempvalue.DisplayAs = r.DisplayAs;
                                tempvalue.TableName = r.TableName;
                                tempvalue.DisplayOrder = r.DisplayOrder;
                                var type = emppersonal.GetType();
                                if (r.ColumnName == "BloodGroup")
                                {
                                    tempvalue.Value = new BloodGroup(Convert.ToInt32(r.Value)).BloodGroupName;
                                }
                                else
                                {
                                    tempvalue.Value = Convert.ToString(emppersonal.GetType().GetProperty(r.ColumnName).GetValue(emppersonal, null));
                                }

                            }
                            if (r.TableName.ToLower() == "emp_address")
                            {
                                if (empaddr.Count > 0)
                                {
                                    r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.ColumnName : r.DisplayAs;
                                    tempvalue.TableName = r.TableName;
                                    tempvalue.DisplayAs = r.DisplayAs;
                                    tempvalue.DisplayOrder = r.DisplayOrder;
                                    var type = empaddr[0].GetType();
                                    tempvalue.Value = Convert.ToString(empaddr[0].GetType().GetProperty(r.ColumnName).GetValue(empaddr[0], null));
                                }
                            }
                            //Assign Master Values from Dynamic Group
                            // **** Need to work*****
                            //Assign payroll History
                            // Modified By Keerthika S on 22/04/2017
                            for (int cnt = 0; cnt < payHistory.PayrollHistoryValueList.Count; cnt++)
                            {
                                if (r.ColumnName == payHistory.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                {
                                    AttributeModel a = new AttributeModel(payHistory.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                    if (r.TableName.ToLower() == "salarybase")
                                    {
                                        // tempvalue.Value = payHistory.PayrollHistoryValueList[cnt].BaseValue;
                                        tempvalue.Value = Convert.ToString((Math.Round((Convert.ToDouble(payHistory.PayrollHistoryValueList[cnt].BaseValue) + 0.01) / 1.0, 0)) * 1);
                                        tempvalue.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                        tempvalue.DisplayOrder = r.DisplayOrder;
                                        tempvalue.TableName = r.TableName;
                                    }
                                    else
                                    {
                                        tempvalue.Value = Convert.ToString((Math.Round((Convert.ToDouble(payHistory.PayrollHistoryValueList[cnt].Value) + 0.01) / 1.0, 0)) * 1);
                                        tempvalue.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                        tempvalue.DisplayOrder = r.DisplayOrder;
                                        tempvalue.TableName = r.TableName;
                                    }
                                }
                            }

                            tempvalue.DisplayAs = string.IsNullOrEmpty(tempvalue.DisplayAs) ? r.DisplayAs : tempvalue.DisplayAs;
                            tempvalue.Value = string.IsNullOrEmpty(tempvalue.Value) ? "0" : tempvalue.Value;
                            tempvalue.EmployeeId = e.EmployeeCode;
                            tempvalue.DisplayOrder = r.DisplayOrder;
                            tempvalue.ColumnName = r.ColumnName;
                            result.Add(tempvalue);

                        });//attr End


                    }
                }

            });//Employee end
            return result;
        }
        private DataTable GetColumns(string tableName)
        {
            string query = "Select COLUMN_NAME,DATA_TYPE,TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME ='" + tableName + "'";

            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataSet GetTableValues(int month, int year, int companyId, string SpName)
        {

            SqlCommand sqlCommand = new SqlCommand(SpName);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Month", month);
            sqlCommand.Parameters.AddWithValue("@Year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetDataSet(sqlCommand);
        }
    }

    public class ESIExtract
    {
        public string ESINumber { get; set; }
        public string FirstName { get; set; }
        public string PaidDays { get; set; }
        public string GrossWages { get; set; }

        public string ReasonCode { get; set; }
        public DateTime LastWorkingDay { get; set; }
    }
}
