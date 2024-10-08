// -----------------------------------------------------------------------
// <copyright file="PTax.cs" company="Microsoft">
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
    using System.Globalization;

    /// <summary>
    /// To handle the PTax
    /// </summary>
    public class PTax
    {

        #region private variable
        private PTaxRangeList _ptaxRangeList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PTax()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PTax(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, 0, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTLocation"])))
                    this.PTLocation = new Guid(Convert.ToString(dtValue.Rows[0]["PTLocation"]));
                this.FormNo = Convert.ToString(dtValue.Rows[0]["FormNo"]);
                this.Calculationtype = Convert.ToString(dtValue.Rows[0]["Calculationtype"]);
                this.DeductionMonth1 = Convert.ToString(dtValue.Rows[0]["DeductionMonth1"]);
                this.DeductionMonth2 = Convert.ToString(dtValue.Rows[0]["DeductionMonth2"]);
                this.RegCertificateNo = Convert.ToString(dtValue.Rows[0]["RegCertificateNo"]);
                this.PTOCircleNo = Convert.ToString(dtValue.Rows[0]["PTOCircleNo"]);
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

        public PTax(Guid ptlocationId, int companyId)
        {
            this.PTLocation = ptlocationId;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(Guid.Empty, this.CompanyId, this.PTLocation);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTLocation"])))
                    this.PTLocation = new Guid(Convert.ToString(dtValue.Rows[0]["PTLocation"]));
                this.FormNo = Convert.ToString(dtValue.Rows[0]["FormNo"]);
                this.Calculationtype = Convert.ToString(dtValue.Rows[0]["Calculationtype"]);
                this.DeductionMonth1 = Convert.ToString(dtValue.Rows[0]["DeductionMonth1"]);
                this.DeductionMonth2 = Convert.ToString(dtValue.Rows[0]["DeductionMonth2"]);
                this.RegCertificateNo = Convert.ToString(dtValue.Rows[0]["RegCertificateNo"]);
                this.PTOCircleNo = Convert.ToString(dtValue.Rows[0]["PTOCircleNo"]);
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
        /// Get or Set the PTLocation
        /// </summary>
        public Guid PTLocation { get; set; }

        /// <summary>
        /// Get or Set the FormNo
        /// </summary>
        public string FormNo { get; set; }

        /// <summary>
        /// Get or Set the Calculationtype
        /// </summary>
        public string Calculationtype { get; set; }

        /// <summary>
        /// Get or Set the DeductionMonth1
        /// </summary>
        public string DeductionMonth1 { get; set; }

        /// <summary>
        /// Get or Set the DeductionMonth2
        /// </summary>
        public string DeductionMonth2 { get; set; }

        /// <summary>
        /// Get or Set the RegCertificateNo
        /// </summary>
        public string RegCertificateNo { get; set; }

        /// <summary>
        /// Get or Set the PTOCircleNo
        /// </summary>
        public string PTOCircleNo { get; set; }

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


        public PTaxRangeList PTaxRangeLists
        {
            get
            {
                if (object.ReferenceEquals(_ptaxRangeList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _ptaxRangeList = new PTaxRangeList(this.Id);

                    }
                    else
                    {
                        _ptaxRangeList = new PTaxRangeList();
                    }
                }
                return _ptaxRangeList;
            }
            set
            {
                _ptaxRangeList = value;
            }
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the PTax
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PTax_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@PTLocation", this.PTLocation);
            sqlCommand.Parameters.AddWithValue("@FormNo", this.FormNo);
            sqlCommand.Parameters.AddWithValue("@Calculationtype", this.Calculationtype);
            sqlCommand.Parameters.AddWithValue("@DeductionMonth1", this.DeductionMonth1);
            sqlCommand.Parameters.AddWithValue("@DeductionMonth2", this.DeductionMonth2);
            sqlCommand.Parameters.AddWithValue("@RegCertificateNo", this.RegCertificateNo);
            sqlCommand.Parameters.AddWithValue("@PTOCircleNo", this.PTOCircleNo);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
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

        /// <summary>
        /// Delete the PTax
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PTax_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PTax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId, Guid ptLocationId)
        {

            SqlCommand sqlCommand = new SqlCommand("PTax_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@PTLocation", ptLocationId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public double GetPTaxCalculation(TaxComputationInfo taxInfo, Employee employee, int companyId, double EranedGrossAmt, double FixedGrossAmt, int year, int month, int userid, out decimal ProjectedPTax, bool FFFlag, int projectedMonth = 1)
        {
            ProjectedPTax = 0;
            PTax pTax = new PTax(employee.PTLocation, companyId);
            DateTime dateofpayroll = new DateTime(year, month, 1);
            if (pTax.Calculationtype == "Formula")
            {
                PTaxRange pTaxRange = new PTaxRange(pTax.Id, EranedGrossAmt); //Directly passed Eraned Gross Amt
                ProjectedPTax = pTaxRange.Amt;
                int ProjectedMonth = 0;
                var getActiveFinYear = taxInfo.FinanceYear;
                if (getActiveFinYear == null)
                {
                    getActiveFinYear = new TXFinanceYear
                    {
                        EndingDate = Convert.ToDateTime(getActiveFinYear.EndingDate, new CultureInfo("en-gb"))
                    };
                }
                DateTime sdate = dateofpayroll;
                if (!FFFlag)
                {
                    do
                    {
                        sdate = sdate.AddMonths(1);
                        if (sdate <= getActiveFinYear.EndingDate)
                        {
                            ProjectedMonth++;
                        }
                    } while (sdate <= getActiveFinYear.EndingDate);
                }
                projectedMonth = ProjectedMonth;

                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;
               // AttributeModelList attributeModelList = new AttributeModelList(companyId);
                var EGID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTG");
                var PTaxID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTAX");

                for (int M = 1; M < 12 - projectedMonth; M++)
                {
                    DateTime currentpayroll = dateofpayroll.AddMonths(-M);
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist,null) &&  taxInfo.payrollhistorylist.Count > 0)
                    {
                      History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                      if (History.Count > 0)
                      {
                         PreMonthEG = History[0].PayrollHistoryValueList.Where(ph => ph.AttributeModelId == EGID.Id).FirstOrDefault();
                        PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(ph => ph.AttributeModelId == PTaxID.Id).FirstOrDefault();
                      }
                    }

                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        //if (Convert.ToDouble(PreMonthEG.Value) > 0) // if Earned gross zero due to lop in this case should ptaxamount also zero
                        //{
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                        // }
                    }
                }


                PTaxRange FpTaxRange = new PTaxRange(pTax.Id, (FixedGrossAmt)); //Directly passed Fixed Gross Amt
                ProjectedPTax = (FpTaxRange.Amt * projectedMonth) + Convert.ToDecimal(SumofPTaxAmt) + pTaxRange.Amt;
                return Convert.ToDouble(pTaxRange.Amt);
            }
            else if (pTax.Calculationtype == "Monthly")
            {
                // AttributeModelList attributeModelList = new AttributeModelList(companyId);
                //var FGID = attributeModelList.FirstOrDefault(u => u.Name == "FG");
                var EGID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTG");
                var PTaxID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTAX");

                // find out Ptax for 1st Half (*In setting month 8* 4---9 ) or 2nd Half (*In setting month 2* 10---3)
                int FixedGrossofmonth = 0;

                string[] FH = pTax.DeductionMonth1.Split('_');
                string[] SH = pTax.DeductionMonth2.Split('_');

                List<int> Fmonth = new List<int>();
                for (int i = Convert.ToInt32(FH[1]); i <= 12; i++)
                {
                    Fmonth.Add(i);
                    if (i == 12)
                        i = 0;
                    if (Fmonth.Count == 6)
                        break;
                }

                List<int> Smonth = new List<int>();
                for (int i = Convert.ToInt32(SH[1]); i <= 12; i++)
                {
                    Smonth.Add(i);
                    if (i == 12)
                        i = 0;
                    if (Smonth.Count == 6)
                        break;
                }
                int ptaxProjection = 0;

                if (Fmonth.Contains(month))
                {
                    ptaxProjection = Fmonth.IndexOf(Convert.ToInt32(FH[0])) - Fmonth.IndexOf(month);
                    FixedGrossofmonth = Fmonth.IndexOf(Convert.ToInt32(FH[2])) - Fmonth.IndexOf(month);
                }
                else
                {
                    ptaxProjection = Smonth.IndexOf(Convert.ToInt32(SH[0])) - Smonth.IndexOf(month);
                    FixedGrossofmonth = Smonth.IndexOf(Convert.ToInt32(SH[2])) - Smonth.IndexOf(month);
                }

                double Projection = Convert.ToDouble(EranedGrossAmt) + Convert.ToDouble(FixedGrossAmt) * (FFFlag ? 0 : FixedGrossofmonth);

                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;
                decimal ptaxrecovered = 0;
                /*prof tax recovered for half year arrived here */
                for (int M = 1; M < 6 - FixedGrossofmonth; M++)
                {
                    DateTime currentpayroll = dateofpayroll.AddMonths(-M);
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist,null) && taxInfo.payrollhistorylist.Count > 0)
                    {
                        History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                        if (History.Count > 0)
                        {
                            PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                            PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                        }
                    }

                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                    }
                }
                /*total professional tax recovered from april upto current month arrived here */
                DateTime curpayroll = dateofpayroll.AddMonths(1);
                for (int M = 0; M < 12; M++)
                {
                    curpayroll = curpayroll.AddMonths(-1);
                    if (curpayroll >= taxInfo.FinanceYear.StartingDate)
                    {
                        PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                        PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                        PayrollHistoryList History = new PayrollHistoryList();
                        if (!ReferenceEquals(taxInfo.payrollhistorylist, null) && taxInfo.payrollhistorylist.Count > 0)
                        {
                            History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == curpayroll.Month && ph.Year == curpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                            if (History.Count > 0)
                            {
                                PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                                PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                            }
                        }

                        if (PreMonthEG != null)
                        {
                            ptaxrecovered = ptaxrecovered + Convert.ToDecimal(PreMonthPTaxAmt.Value);
                        }
                    }
                }

                /*ends here */


                PTaxRange pTaxRange = new PTaxRange(pTax.Id, Projection + SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                decimal ptaxAmount = 0;
                if (!Object.ReferenceEquals(pTaxRange, null))
                {
                    ptaxAmount = pTaxRange.Amt;
                }

                ptaxProjection = ptaxProjection + 1;
                if (Convert.ToDouble(ptaxAmount) > SumofPTaxAmt) //If already excess deducted PTax means Ptax return 0 
                {
                    ProjectedPTax = ptaxrecovered;
                    if (ptaxProjection > 0 && !FFFlag)
                        return ((Convert.ToDouble(ptaxAmount) - SumofPTaxAmt) / (ptaxProjection));
                    else
                        return (Convert.ToDouble(ptaxAmount) - SumofPTaxAmt);
                }
                else
                {
                    ProjectedPTax = Convert.ToDecimal(ptaxrecovered);
                    return 0;
                }


            }
            //As per the Ptax setting Six month as Aug and feb month.. Every half yearly (HR or Admin)need to check whether ptax deducted correctly or not, so they will change in Ptax setting as SEP and MAR   **(month ==9 && month == 3))
            else if (pTax.Calculationtype == "SixMont")
            {
                // AttributeModelList attributeModelList = new AttributeModelList(companyId);
                // var FGID = attributeModelList.FirstOrDefault(u => u.Name == "FG");
                var EGID = taxInfo.AttributemodelList.Where(u => u.Name == "PTG").FirstOrDefault();
                var PTaxID = taxInfo.AttributemodelList.Where(u => u.Name == "PTAX").FirstOrDefault();


                string[] FH = pTax.DeductionMonth1.Split('_');
                string[] SH = pTax.DeductionMonth2.Split('_');
                int FHR = Convert.ToInt32(FH[0]);
                int FH1 = Convert.ToInt32(FH[1]);
                int FH2 = Convert.ToInt32(FH[2]);

                int SHR = Convert.ToInt32(SH[0]);
                int SH1 = Convert.ToInt32(SH[1]);
                int SH2 = Convert.ToInt32(SH[2]);

                // find out Ptax for 1st Half (*In setting month 8* 4---9 ) or 2nd Half (*In setting month 2* 10---3)
                int FixedGrossofmonth = 0;
                int Balprojmonths = 0;
                int Balprojdays = 0;
                int Balmaxdays = 0;
                if (month >= 4 && month <= 9)
                {
                    FixedGrossofmonth = 8 + 1 - month;
                }
                else
                {
                    if (month > 9)
                        FixedGrossofmonth = 12 - month + 3;
                    else
                        FixedGrossofmonth = 3 - month;
                }

                if (employee.DateOfJoining >= taxInfo.FinanceYear.StartingDate && employee.DateOfJoining <= taxInfo.FinanceYear.EndingDate)
                {
                    int procyymm = year * 100 + month;
                    int projyymm = 0;
                    if (month >= 4 && month <= 9)
                    {
                        projyymm = taxInfo.FinanceYear.StartingDate.Year * 100 + 09;
                    }
                    else
                    {
                        projyymm = taxInfo.FinanceYear.EndingDate.Year * 100 + taxInfo.FinanceYear.EndingDate.Month;
                    }
                      int addyear = year;
                      while (procyymm < projyymm)
                      {
                          Balprojmonths = Balprojmonths + 1;
                          procyymm = procyymm + 1;
                          if (procyymm > ((addyear * 100) + 12))
                          {
                              procyymm = (((addyear + 1) * 100) + 1);
                              addyear = addyear + 1;
                          }
                      }
                }

                //current Month Data of EranedGrossAmt & FixedGrossAmt  --this month record will not avail in payrollhistroyvalue table bcoz not yet process
                double Projection = 0;
                if (Balprojmonths > 0 || Balprojdays > 0)
                {
                    Projection = Convert.ToDouble(EranedGrossAmt) + Convert.ToDouble(FixedGrossAmt) * (FFFlag ? 0 : Balprojmonths);
                    if (Balprojdays > 0)
                    {
                        Projection = Projection + Convert.ToDouble(FixedGrossAmt) * (FFFlag ? 0 : Balprojdays / Balmaxdays); 
                    }
                }
                else
                {
                    Projection = Convert.ToDouble(EranedGrossAmt) + Convert.ToDouble(FixedGrossAmt) * (FFFlag ? 0 : FixedGrossofmonth);
                }


                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;

                for (int M = 1; M < 6 - FixedGrossofmonth; M++)
                {
                    DateTime currentpayroll = dateofpayroll.AddMonths(-M);
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist,null) && taxInfo.payrollhistorylist.Count > 0)
                    {
                        History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                        if (History.Count > 0)
                        {
                            PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                            PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                        }
                    }

                    // PayrollHistory PreviousMonth_PayrollHistory = new PayrollHistory(companyId, employee.Id, currentpayroll.Year, currentpayroll.Month);
                    // var PreMonthEG = PreviousMonth_PayrollHistory.PayrollHistoryValueList.FirstOrDefault(u => u.AttributeModelId == EGID.Id);
                    // var PreMonthPTaxAmt = PreviousMonth_PayrollHistory.PayrollHistoryValueList.FirstOrDefault(u => u.AttributeModelId == PTaxID.Id);
                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        //if (Convert.ToDouble(PreMonthEG.Value) > 0) // if Earned gross zero due to lop in this case should ptaxamount also zero
                        //{
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                        //}
                    }
                }


                /* total prof tax recovered from april arrived here */
                decimal ptaxrecovered = 0;
                DateTime curpayroll = dateofpayroll.AddMonths(1);
                for (int M = 0; M < 12; M++)
                {
                    curpayroll = curpayroll.AddMonths(-1);
                    if (curpayroll >= taxInfo.FinanceYear.StartingDate)
                    {
                        PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                        PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                        PayrollHistoryList History = new PayrollHistoryList();
                        if (!ReferenceEquals(taxInfo.payrollhistorylist, null) && taxInfo.payrollhistorylist.Count > 0)
                        {
                            History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == curpayroll.Month && ph.Year == curpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                            if (History.Count > 0)
                            {
                                PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                                PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                            }
                        }

                        if (PreMonthEG != null)
                        {
                            ptaxrecovered = ptaxrecovered + Convert.ToDecimal(PreMonthPTaxAmt.Value);
                        }
                    }
                }


                PTaxRange pTaxRange = new PTaxRange(pTax.Id, Projection + SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                ProjectedPTax = ptaxrecovered;
                if (Convert.ToDouble(pTaxRange.Amt) > SumofPTaxAmt && (month == FHR || month == SHR )) //If already excess deducted PTax means Ptax return 0 
                {
                    if (FixedGrossofmonth > 0 && !FFFlag)
                        return ((Convert.ToDouble(pTaxRange.Amt) - SumofPTaxAmt) / FixedGrossofmonth);
                    else
                        return (Convert.ToDouble(pTaxRange.Amt) - SumofPTaxAmt);
                }
                else
                {
                    if (FFFlag)
                    {
                        return Convert.ToDouble(pTaxRange.Amt) > SumofPTaxAmt ? (Convert.ToDouble(pTaxRange.Amt) - SumofPTaxAmt) : 0;
                    }
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public double GetPTaxProjectionCalculation(TaxComputationInfo taxInfo, Employee employee, int companyId, double EranedGrossAmt, double FixedGrossAmt, int year, int month, int userid, out decimal ProjectedPTax,bool FFflag, int projectedMonth = 1)
        {

            ProjectedPTax = 0;
        //    Employee employee = new Employee(companyId, Guid.Empty, employeeId); //instead of category id send empty
            PTax pTax = new PTax(employee.PTLocation, companyId);

            DateTime dateofpayroll = new DateTime(year, month, 1);

            if (pTax.Calculationtype == "Formula")
            {
                PTaxRange pTaxRange = new PTaxRange(pTax.Id, EranedGrossAmt); //Directly passed Eraned Gross Amt
                ProjectedPTax = pTaxRange.Amt;
                // PTaxRange FpTaxRange = new PTaxRange(pTax.Id, FixedGrossAmt); //Directly passed Eraned Gross Amt
                //  ProjectedPTax = ProjectedPTax + (FpTaxRange.Amt * projectedMonth);
                int ProjectedMonth = 0;
                var txFinYear = taxInfo.FinanceYear;
                var getActiveFinYear = txFinYear;
                DateTime sdate = dateofpayroll;
                do
                {
                    sdate = sdate.AddMonths(1);
                    if (sdate <= getActiveFinYear.EndingDate)
                    {
                        ProjectedMonth++;
                    }
                } while (sdate <= getActiveFinYear.EndingDate);
                projectedMonth = ProjectedMonth;

                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;
         //       AttributeModelList attributeModelList = new AttributeModelList(companyId);
                var EGID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTG");
                var PTaxID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTAX");

                for (int M = 1; M < 12 - projectedMonth; M++)
                {
                    DateTime currentpayroll = dateofpayroll.AddMonths(-M);
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist,null) && taxInfo.payrollhistorylist.Count > 0)
                    {
                        History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                        if (History.Count > 0)
                        {
                            PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                            PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                        }
                    }

                   // PayrollHistory PreviousMonth_PayrollHistory = new PayrollHistory(companyId, employee.Id, currentpayroll.Year, currentpayroll.Month);
                   // var PreMonthEG = PreviousMonth_PayrollHistory.PayrollHistoryValueList.FirstOrDefault(u => u.AttributeModelId == EGID.Id);
                   // var PreMonthPTaxAmt = PreviousMonth_PayrollHistory.PayrollHistoryValueList.FirstOrDefault(u => u.AttributeModelId == PTaxID.Id);
                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        //if (Convert.ToDouble(PreMonthEG.Value) > 0) // if Earned gross zero due to lop in this case should ptaxamount also zero
                        //{
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                        // }
                    }
                }


                PTaxRange FpTaxRange = new PTaxRange(pTax.Id, (FixedGrossAmt)); //Directly passed Fixed Gross Amt
                ProjectedPTax = (FpTaxRange.Amt * projectedMonth) + Convert.ToDecimal(SumofEarnedGross + EranedGrossAmt);                                                                                                            // ProjectedPTax = ProjectedPTax+(FpTaxRange.Amt * projectedMonth);
                return Convert.ToDouble(pTaxRange.Amt);
            }

            else if (pTax.Calculationtype == "Monthly")
            {
                // AttributeModelList attributeModelList = new AttributeModelList(companyId);
                //var FGID = attributeModelList.FirstOrDefault(u => u.Name == "FG");
                var EGID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTG");
                var PTaxID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTAX");

                // find out Ptax for 1st Half (*In setting month 8* 4---9 ) or 2nd Half (*In setting month 2* 10---3)
                int FixedGrossofmonth = 0;
                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;
                int Balprojmonths = 0, Balprojdays = 0, Balmaxdays = 0;

                string[] FH = pTax.DeductionMonth1.Split('_');
                string[] SH = pTax.DeductionMonth2.Split('_');


                if (employee.DateOfJoining >= taxInfo.FinanceYear.StartingDate && employee.DateOfJoining <= taxInfo.FinanceYear.EndingDate)
                {
                    int procyymm = year * 100 + month;
                    int projyymm = 0;
                    if (month >= Convert.ToInt32(FH[1]) && month <= Convert.ToInt32(FH[2]))
                    {
                        projyymm = taxInfo.FinanceYear.StartingDate.Year * 100 + Convert.ToInt32(FH[2]);
                    }
                    else
                    {
                        projyymm = taxInfo.FinanceYear.EndingDate.Year * 100 + taxInfo.FinanceYear.EndingDate.Month;
                    }
                    if (month == employee.DateOfJoining.Month)
                    {
                        Balprojmonths = 0;
                        Balprojdays = DateTime.DaysInMonth(year, month) + 1 - employee.DateOfJoining.Day;
                        Balmaxdays = DateTime.DaysInMonth(year, month);
                    }
                    else
                    {
                        int addyear = year;
                        while (procyymm < projyymm)
                        {
                            Balprojmonths = Balprojmonths + 1;
                            procyymm = procyymm + 1;
                            if (procyymm > ((addyear * 100) + 12))
                            {
                                procyymm = (((addyear + 1) * 100) + 1);
                                addyear = addyear + 1;
                            }
                        }
                    }
                }

                if (month == employee.LastWorkingDate.Month && year == employee.LastWorkingDate.Year)
                {
                    Balprojmonths = 0;
                    Balprojdays = employee.LastWorkingDate.Day;
                    Balmaxdays = DateTime.DaysInMonth(year, month);
                    FixedGrossAmt = 0;

                }

                string sal_processed = "";
                var txFinYear = taxInfo.FinanceYear;
                var getActiveFinYear = txFinYear;
                DateTime currentpayroll;
                DateTime EndingDate;
                int days = DateTime.DaysInMonth(year, month);
                FixedGrossofmonth = 0;
                SumofEarnedGross = 0;
                EndingDate = Convert.ToDateTime(01 + "/" + month + "/" + year, new CultureInfo("en-GB"));
                if (month >= Convert.ToInt32(FH[1]) && month <= Convert.ToInt32(FH[2]))
                {
                    FixedGrossofmonth = Convert.ToInt32(FH[2]) + 1 - month;
                    currentpayroll = Convert.ToDateTime("1/" + Convert.ToInt32(FH[1]) + "/" + getActiveFinYear.StartingDate.Year, new CultureInfo("en-GB"));
                }
                else
                {
                    if (month <= Convert.ToInt32(SH[2]))
                    {
                        FixedGrossofmonth = 3 + 1 - month;
                    }
                    else
                    {
                        FixedGrossofmonth = 15 + 1 - month;
                    }
                    currentpayroll = Convert.ToDateTime("1/" + Convert.ToInt32(SH[1])  + "/" + getActiveFinYear.StartingDate.Year, new CultureInfo("en-GB"));
                }

                while (currentpayroll <= EndingDate)
                {
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist, null) && taxInfo.payrollhistorylist.Count > 0)
                    {
                        History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                        if (History.Count > 0)
                        {
                            if (History[0].Month == month && History[0].Year == year)
                            {
                                sal_processed = "Y";
                            }
                            PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                            PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                        }
                    }

                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                    }

                    currentpayroll = currentpayroll.AddMonths(1);
                }

                if (month >= Convert.ToInt32(FH[1]) && month <= Convert.ToInt32(FH[2]))
                {
                    if (Balprojdays > 0 && sal_processed == "")
                    {
                        SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * Balprojdays / Balmaxdays);
                    }

                    if (Balprojmonths > 0)
                    {
                        FixedGrossofmonth = Balprojmonths + 1;
                    }

                    if (sal_processed == "Y" && FixedGrossofmonth > 0)
                    {
                        FixedGrossofmonth = FixedGrossofmonth - 1;
                    }

                    SumofEarnedGross = SumofEarnedGross + (FixedGrossofmonth * FixedGrossAmt);//+EranedGrossAmt;
                    PTaxRange pTaxRange = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                    ProjectedPTax = pTaxRange.Amt - Convert.ToDecimal(SumofPTaxAmt);
                    if (ProjectedPTax < 0)
                    {
                        ProjectedPTax = 0;
                    }
                    if (!FFflag)
                    {
                        SumofEarnedGross = SumofEarnedGross + FixedGrossAmt * 6;
                        PTaxRange pTaxRange1 = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                        SumofEarnedGross = 0;
                        ProjectedPTax = ProjectedPTax + pTaxRange1.Amt;
                    }
                }
                else
                {
                    if (Balprojmonths < 6)
                    {
                        if (Balprojdays > 0 && sal_processed == "")
                        {
                            SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * Balprojdays / Balmaxdays);
                        }
                    }

                    if (Balprojmonths > 0)
                    {
                        FixedGrossofmonth = Balprojmonths;
                    }

                    if (sal_processed == "Y" && FixedGrossofmonth > 0)
                    {
                        FixedGrossofmonth = FixedGrossofmonth - 1;
                    }

                    SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * FixedGrossofmonth);
                    PTaxRange pTaxRange = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                    SumofEarnedGross = 0;

                    if (pTaxRange.Amt > Convert.ToDecimal(SumofPTaxAmt))
                    {
                        ProjectedPTax = pTaxRange.Amt - Convert.ToDecimal(SumofPTaxAmt);
                    }
                    else
                    {
                        ProjectedPTax = 0;
                    }
                    // ProjectedPTax = Convert.ToDecimal(SumofPTaxAmt);
                }

                return 0;
            }
            //As per the Ptax setting Six month as Aug and feb month.. Every half yearly (HR or Admin)need to check whether ptax deducted correctly or not, so they will change in Ptax setting as SEP and MAR   **(month ==9 && month == 3))
            else if (pTax.Calculationtype == "SixMont")
            {
                var EGID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTG");
                var PTaxID = taxInfo.AttributemodelList.FirstOrDefault(u => u.Name == "PTAX");

                // find out Ptax for 1st Half (*In setting month 8* 4---9 ) or 2nd Half (*In setting month 2* 10---3)
                int FixedGrossofmonth = 0;
                double SumofEarnedGross = 0;
                double SumofPTaxAmt = 0;
                int Balprojmonths = 0, Balprojdays = 0, Balmaxdays = 0;

                if (employee.DateOfJoining >= taxInfo.FinanceYear.StartingDate && employee.DateOfJoining <= taxInfo.FinanceYear.EndingDate)
                {
                    int procyymm = year * 100 + month;
                    int projyymm = 0;
                    if (month >= 4 && month <= 9)
                    {
                        projyymm = taxInfo.FinanceYear.StartingDate.Year * 100 + 09;
                    }
                    else
                    {
                        projyymm = taxInfo.FinanceYear.EndingDate.Year * 100 + taxInfo.FinanceYear.EndingDate.Month;
                    }
                    if (month == employee.DateOfJoining.Month)
                    {
                        Balprojmonths = 0;
                        Balprojdays = DateTime.DaysInMonth(year, month) + 1 - employee.DateOfJoining.Day;
                        Balmaxdays = DateTime.DaysInMonth(year, month);
                    }
                    else
                    {
                        int addyear = year;
                        while (procyymm < projyymm)
                        {
                            Balprojmonths = Balprojmonths + 1;
                            procyymm = procyymm + 1;
                            if (procyymm > ((addyear * 100) + 12))
                            {
                                procyymm = (((addyear + 1) * 100) + 1);
                                addyear = addyear + 1;
                            }
                        }
                    }
                }

                if (month == employee.LastWorkingDate.Month && year == employee.LastWorkingDate.Year)
                {
                    Balprojmonths = 0;
                    Balprojdays = employee.LastWorkingDate.Day;
                    Balmaxdays = DateTime.DaysInMonth(year, month);
                    FixedGrossAmt = 0;

                }


                /* introduced here for six month  */
                string sal_processed = "";
                var txFinYear = taxInfo.FinanceYear;
                var getActiveFinYear = txFinYear;
                DateTime currentpayroll;
                DateTime EndingDate;
                int days = DateTime.DaysInMonth(year, month);
                FixedGrossofmonth = 0;
                SumofEarnedGross = 0;
                EndingDate = Convert.ToDateTime(01 + "/" + month + "/" + year, new CultureInfo("en-GB"));
                if (month >=4 && month <= 9)
                {
                    FixedGrossofmonth = 9 - month;
                    currentpayroll = Convert.ToDateTime("1/4/" + getActiveFinYear.StartingDate.Year, new CultureInfo("en-GB"));
                }
                else
                {
                    if (month < 4)
                    {
                        FixedGrossofmonth = 3 - month;
                    }
                    else
                    {
                        FixedGrossofmonth = 15 - month;
                    }
                    currentpayroll = Convert.ToDateTime("1/10/" + getActiveFinYear.StartingDate.Year, new CultureInfo("en-GB"));
                }

                while (currentpayroll <= EndingDate)
                {
                    PayrollHistoryValue PreMonthEG = new PayrollHistoryValue();
                    PayrollHistoryValue PreMonthPTaxAmt = new PayrollHistoryValue();
                    PayrollHistoryList History = new PayrollHistoryList();
                    if (!ReferenceEquals(taxInfo.payrollhistorylist, null) && taxInfo.payrollhistorylist.Count > 0)
                    {
                        History.AddRange(taxInfo.payrollhistorylist.Where(ph => ph.Month == currentpayroll.Month && ph.Year == currentpayroll.Year && ph.EmployeeId == employee.Id).ToList());
                        if (History.Count > 0)
                        {
                            if (History[0].Month == month && History[0].Year == year)
                            {
                                sal_processed = "Y";
                            }
                            PreMonthEG = History[0].PayrollHistoryValueList.Where(ul => ul.AttributeModelId == EGID.Id).FirstOrDefault();
                            PreMonthPTaxAmt = History[0].PayrollHistoryValueList.Where(u => u.AttributeModelId == PTaxID.Id).FirstOrDefault();
                        }
                    }

                    if (PreMonthEG != null)
                    {
                        SumofEarnedGross = SumofEarnedGross + Convert.ToDouble(PreMonthEG.Value);
                        SumofPTaxAmt = SumofPTaxAmt + Convert.ToDouble(PreMonthPTaxAmt.Value);
                    }

                    currentpayroll = currentpayroll.AddMonths(1);
                }



                    /* */


                if (month >= 4 && month <= 9)
                {
                    if (Balprojdays > 0 && sal_processed == "")
                    {
                        SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * Balprojdays / Balmaxdays);
                    }

                    if (Balprojmonths > 0)
                    {
                        FixedGrossofmonth = Balprojmonths - 6;
                    }

                    if (sal_processed == "Y" && FixedGrossofmonth > 0)
                    {
                        FixedGrossofmonth = FixedGrossofmonth - 1;
                    }
                    SumofEarnedGross = SumofEarnedGross + (FixedGrossofmonth * FixedGrossAmt);//+EranedGrossAmt;
                    PTaxRange pTaxRange = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                    ProjectedPTax = pTaxRange.Amt - Convert.ToDecimal(SumofPTaxAmt);
                    if (ProjectedPTax < 0)
                    {
                        ProjectedPTax = 0;
                    }
                    if (!FFflag)
                    {
                        SumofEarnedGross = SumofEarnedGross + FixedGrossAmt * 6;
                        PTaxRange pTaxRange1 = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                        SumofEarnedGross = 0;
                        ProjectedPTax = ProjectedPTax + pTaxRange1.Amt;
                    }
                }
                else
                {
                    if (Balprojmonths < 6)
                    {
                        if (Balprojdays > 0 && sal_processed == "")
                        {
                            SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * Balprojdays / Balmaxdays);
                        }
                    }

                    if (Balprojmonths > 0 )
                    {
                        FixedGrossofmonth = Balprojmonths;
                    }

                    if (sal_processed == "Y" && FixedGrossofmonth > 0)
                    {
                        FixedGrossofmonth = FixedGrossofmonth - 1;
                    }

                    SumofEarnedGross = SumofEarnedGross + (FixedGrossAmt * FixedGrossofmonth);
                    PTaxRange pTaxRange = new PTaxRange(pTax.Id, SumofEarnedGross); //Get Ptaxamt from PtaxRange for current month
                    SumofEarnedGross = 0;

                    if (pTaxRange.Amt > Convert.ToDecimal(SumofPTaxAmt))
                    {
                        ProjectedPTax = pTaxRange.Amt - Convert.ToDecimal(SumofPTaxAmt);
                    }
                    else
                    {
                        ProjectedPTax = 0;
                    }
                   // ProjectedPTax = Convert.ToDecimal(SumofPTaxAmt);
                }

                return 0;

            }
            else
            {
                return 0;
            }

        }

        #endregion

    }
}

