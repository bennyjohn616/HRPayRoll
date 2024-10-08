using PayrollBO.TaxActivities;
using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxHistory
    {
        public TaxHistory()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TaxHistory(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYearId"])))
                    this.FinanceYearId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYearId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FieldId"])))
                    this.FieldId = new Guid(Convert.ToString(dtValue.Rows[0]["FieldId"]));
                this.Field = Convert.ToString(dtValue.Rows[0]["Field"]);
                this.FieldType = Convert.ToString(dtValue.Rows[0]["FieldType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Actual"])))
                    this.Actual = Convert.ToDecimal(dtValue.Rows[0]["Actual"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Projection"])))
                    this.Projection = Convert.ToDecimal(dtValue.Rows[0]["Projection"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Total"])))
                    this.Total = Convert.ToDecimal(dtValue.Rows[0]["Total"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Limit"])))
                    this.Limit = Convert.ToDecimal(dtValue.Rows[0]["Limit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.Createdby = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }

        public Guid Id { get; set; }
        public Guid FinanceYearId { get; set; }
        public DateTime ApplyDate { get; set; }
        public Guid EmployeeId { get; set; }
        public int UserId { get; set; }
        public Guid FieldId { get; set; }
        public string Field { get; set; }
        public string FieldType { get; set; }
        public decimal? Actual { get; set; }
        public decimal? Projection { get; set; }
        public decimal? Limit { get; set; }
        public decimal? Total { get; set; }
        public int Createdby { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int ActualMonth { get; set; }
        public string Importxmlstring { get; set; }
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistory_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            // sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
            sqlCommand.Parameters.AddWithValue("@FieldId", this.FieldId);
            sqlCommand.Parameters.AddWithValue("@Field", this.Field);
            sqlCommand.Parameters.AddWithValue("@FieldType", this.FieldType);
            sqlCommand.Parameters.AddWithValue("@Actual", this.Actual);
            sqlCommand.Parameters.AddWithValue("@Projection", this.Projection);
            sqlCommand.Parameters.AddWithValue("@Limit", this.Limit);
            sqlCommand.Parameters.AddWithValue("@Total", this.Total);
            sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        public bool SaveAP(DataTable dt1)
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistory_SaveAP1");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var parameter = sqlCommand.CreateParameter();
            parameter.TypeName = "dbo.Data1Type";
            parameter.Value = dt1;
            parameter.ParameterName = "@Data";
            sqlCommand.Parameters.Add(parameter);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;

            /*            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
                        sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
                        sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@ActualMonth", this.ActualMonth);
                        sqlCommand.Parameters.AddWithValue("@FieldId", this.FieldId);
                        sqlCommand.Parameters.AddWithValue("@Field", this.Field);
                        sqlCommand.Parameters.AddWithValue("@FieldType", this.FieldType);
                        sqlCommand.Parameters.AddWithValue("@Actual", this.Actual);
                        sqlCommand.Parameters.AddWithValue("@Projection", this.Projection);
                        sqlCommand.Parameters.AddWithValue("@Limit", this.Limit);
                        sqlCommand.Parameters.AddWithValue("@Total", this.Total);
                        sqlCommand.Parameters.AddWithValue("@Createdby", this.Createdby);
                        sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);*/

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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        /// 


        public bool SaveAPTemp(DataTable dt1)
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistory_SaveAPTemp");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 120;
            var parameter = sqlCommand.CreateParameter();
            parameter.TypeName = "dbo.Data1Type";
            parameter.Value = dt1;
            parameter.ParameterName = "@Data";
            sqlCommand.Parameters.Add(parameter);
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


        public bool DeleteAP()
        {

            SqlCommand sqlCommand = new SqlCommand("TX_History_DeleteAP");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("TX_History_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        public bool TempDeleteAP()
        {

            SqlCommand sqlCommand = new SqlCommand("TX_History_TempDeleteAP");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        public bool TempDelete()
        {

            SqlCommand sqlCommand = new SqlCommand("TX_History_TempDelete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistory_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            if (this.ApplyDate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValuesTemp()
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistoryTemp_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            if (this.ApplyDate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValuesAP()
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistory_SelectAP");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            if (this.ApplyDate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValuesTempAP()
        {

            SqlCommand sqlCommand = new SqlCommand("TaxHistoryTemp_SelectAP");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            if (this.ApplyDate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool Import(DataTable dt2)
        {
            string status;
            SqlCommand sqlCommand = new SqlCommand("TaxHistoryValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var parameter = sqlCommand.CreateParameter();
            parameter.TypeName = "dbo.HistoryType";
            parameter.Value = dt2;
            parameter.ParameterName = "@History";
            sqlCommand.Parameters.Add(parameter);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.SaveData(sqlCommand, out status);
        }

        public bool ImportTemp(DataTable dt2)
        {
            string status;
            SqlCommand sqlCommand = new SqlCommand("TaxHistoryValue_SaveTemp");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 120;
            var parameter = sqlCommand.CreateParameter();
            parameter.TypeName = "dbo.HistoryType";
            parameter.Value = dt2;
            parameter.ParameterName = "@History";
            sqlCommand.Parameters.Add(parameter);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.SaveData(sqlCommand, out status);
        }

        public string ProcessTax(TaxComputationInfo taxinfo,PayrollHistoryList payrollhistorylist,TaxHistoryList futureTaxListCheck, List<Employee> emplist, int year, int month, string type, Guid financeYearId, int companyId, int userId, bool ffFlag = false)
        {

            string Msg = string.Empty;
            string pay = string.Empty;
            string dob = string.Empty;
            string ITProcessedMsg = string.Empty;
            string ITPastMonthInfoMsg = string.Empty;


            ITax taxprocess = new ITax();

            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int processcount = 0;
            int employeecount = emplist.Count();
            emplist.ForEach(u =>
            {
                if (u.DateOfBirth == DateTime.MinValue)
                {
                    dob = dob + u.EmployeeCode + ",";
                }


                if (u.DateOfJoining <= CurrPayrollmonth && u.PayrollProcess == true)
                {
                    taxinfo.Employees.Add(new Employee(companyId, u.Id));

                    processcount = processcount + 1;
                }
                else
                {
                    ITProcessedMsg = Msg + "Cannot process payroll for " + u.EmployeeCode + " check PayrollProcess Status for the Employee.\n";
                }

                List<PayrollHistory> payrollhistorylis = new List<PayrollHistory>();

                if (taxinfo.processtype != "employee")
                {
                    payrollhistorylis = payrollhistorylist.Where(w => w.EmployeeId == u.Id && w.Year == year && w.Month == month && w.Status.Trim() == "Processed").ToList();
                    if (payrollhistorylis.Count() <= 0)
                    {
                        pay = pay + "Payroll Not Yet Processed for " + u.EmployeeCode + " check PayrollProcess Status for the Employee.\n";
                        ITProcessedMsg = pay;
                        taxinfo.Employees.Remove(taxinfo.Employees.Where(x => x.Id == u.Id).FirstOrDefault());
                    }
                }
                else
                {
                    payrollhistorylis = payrollhistorylist.Where(w => w.EmployeeId == u.Id).ToList();
                }

                //Check already processed process 
                DateTime applyDate = new DateTime(year, month, 1);

                //Income tax is processing for a month before need to check the selrcted month  furure month tax is processed or not.
                //The Future month tax is processed then the tax can't be process for the selected month



                if (futureTaxListCheck.Count > 0)
                {
                    if (futureTaxListCheck.Where(x => x.ApplyDate > applyDate).Count() > 0)
                    {
                        ITPastMonthInfoMsg = ITPastMonthInfoMsg + "Income Tax can't be process for the selected month future month already processed for the Employee " + u.EmployeeCode + "\n";
                        taxinfo.Employees.Remove(taxinfo.Employees.Where(x => x.Id == u.Id).FirstOrDefault());
                    }
                }


                // TaxHistoryList taxhistorylist = new TaxHistoryList(financeYearId, u.Id, applyDate);
                var taxhistorylist = futureTaxListCheck.Where(ft => ft.FinanceYearId == financeYearId && ft.EmployeeId == u.Id && ft.ApplyDate == applyDate).ToList();

                if (taxhistorylist.Count > 0)
                {
                    ITProcessedMsg = ITProcessedMsg + "Income Tax already processed for the Employee " + u.EmployeeCode + "\n";
                    taxinfo.Employees.Remove(taxinfo.Employees.Where(x => x.Id == u.Id).FirstOrDefault());
                }

             //   if (payrollhistorylis.Count() <= 0)
             //   {
             //       taxinfo.Employees.Remove(taxinfo.Employees.Where(x => x.Id == u.Id).FirstOrDefault());
             //   }
             //   else
             //   {
                   // taxinfo.FandFFlag = payrollhistorylis[0].IsFandF;
             //   }
            });

            if (dob != string.Empty) { Msg = "Please Enter Date of Birth for Employees " + dob; }
            if (pay != string.Empty) { Msg = pay == string.Empty ? "There is some error while saving the data." : pay; }

            //Check already processed process  send alert msg for single employee otherwise skipped
            if (taxinfo.Employees.Count == 0)
            {
                if (!string.IsNullOrEmpty(ITProcessedMsg))
                { Msg = ITProcessedMsg == string.Empty ? "There is some error while saving the data." : ITProcessedMsg; }
                if (!string.IsNullOrEmpty(ITPastMonthInfoMsg))
                { Msg = ITPastMonthInfoMsg == string.Empty ? "There is some error while saving the data." : ITPastMonthInfoMsg; }
                return Msg;
            }
                //if (!string.IsNullOrEmpty(Msg))
                //{
                //    return Msg;
                //}
                try
                {
                    ITax.ComputeTax(taxinfo);

                    if (taxinfo.Errors.Count == 0)
                    {
                        if (processcount > 0)
                        {
                            if (!string.IsNullOrEmpty(ITProcessedMsg))
                            { Msg = Msg + "\n" + ITProcessedMsg; }
                            if (!string.IsNullOrEmpty(ITPastMonthInfoMsg))
                            { Msg = Msg + "\n" + ITPastMonthInfoMsg; }

                            // Msg = string.Empty;  
                        }
                        else
                        {
                            Msg = string.Empty;
                        }
                    }
                    else
                    {
                        Msg = Msg == string.Empty ? "There is some error while saving the data." : Msg;
                    }
                }
                catch (Exception ex)
                {
                    Msg = "Please check your formula's " + ex.Message;
                }
            return Msg;
        }


        public decimal paidtax(TXFinanceYear txFin,int year,int month,int companyId,AttributeModelList attr, Employee emp)
        {
            NetTaxActivity nta = new NetTaxActivity();
            TaxComputationInfo taxInfo = new PayrollBO.TaxComputationInfo();
            taxInfo.FinanceYear = txFin;
            taxInfo.EffectiveDate = new DateTime(year, month, 1);
            taxInfo.CompanyId = companyId;
            taxInfo.AttributemodelList = attr;
            decimal temppaid = Convert.ToDecimal(Math.Round(Convert.ToDouble(Math.Round(nta.paidTax(taxInfo, emp), 2)) * 1.0) * 1);
            return temppaid;
        }
        public DataTable Form16PartB(Guid financeYearId, Guid employeeId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_GetTaxValuesForForm16B");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Employeeid", employeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearID", financeYearId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataSet Form16PartBGrossValues(Guid financeYearId, Guid employeeId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_GetGrossTaxValuesForForm16B");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Employeeid", employeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearID", financeYearId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetDataSet(sqlCommand);
        }

        public DataSet Form24Quaterly(Guid financeYearId, DateTime startDate, int CompanyId)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_Form24QuaterlyReport");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinYrId", financeYearId);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", startDate);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetDataSet(sqlCommand);
        }
        public DataTable Form24A(Guid financeYearId, Guid employeeId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("usp_Form24Annexture");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Employeeid", employeeId);
            sqlCommand.Parameters.AddWithValue("@FinancialYearID", financeYearId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }
}
