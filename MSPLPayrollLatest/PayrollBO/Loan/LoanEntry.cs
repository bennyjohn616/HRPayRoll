// -----------------------------------------------------------------------
// <copyright file="LoanEntry.cs" company="Microsoft">
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
    /// To handle the LoanEntry
    /// </summary>
    public class LoanEntry
    {

        #region private variable

        private LoanTransactionList _loanTransactionList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public LoanEntry()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public LoanEntry(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, Guid.Empty, Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LoanMasterId"])))
                    this.LoanMasterId = new Guid(Convert.ToString(dtValue.Rows[0]["LoanMasterId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LoanDate"])))
                    this.LoanDate = Convert.ToDateTime(dtValue.Rows[0]["LoanDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApplyDate"])))
                    this.ApplyDate = Convert.ToDateTime(dtValue.Rows[0]["ApplyDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LoanAmt"])))
                    this.LoanAmt = Convert.ToDecimal(dtValue.Rows[0]["LoanAmt"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfMonths"])))
                    this.NoOfMonths = Convert.ToInt32(dtValue.Rows[0]["NoOfMonths"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AmtPerMonth"])))
                    this.AmtPerMonth = Convert.ToDecimal(dtValue.Rows[0]["AmtPerMonth"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsForeClose"])))
                    this.IsForeClose = Convert.ToBoolean(dtValue.Rows[0]["IsForeClose"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ForeCloseDate"])))
                    this.ForeCloseDate = Convert.ToDateTime(dtValue.Rows[0]["ForeCloseDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ForeCloseReverseDate"])))
                    this.ForeCloseReverseDate = Convert.ToDateTime(dtValue.Rows[0]["ForeCloseReverseDate"]);
                this.Reason = Convert.ToString(dtValue.Rows[0]["Reason"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the LoanMasterId
        /// </summary>
        public Guid LoanMasterId { get; set; }

        /// <summary>
        /// Get or Set the LoanDate
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Get or Set the ApplyDate
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// Get or Set the LoanAmt
        /// </summary>
        public Decimal LoanAmt { get; set; }

        /// <summary>
        /// Get or Set the NoOfMonths
        /// </summary>
        public int NoOfMonths { get; set; }

        /// <summary>
        /// Get or Set the AmtPerMonth
        /// </summary>
        public Decimal AmtPerMonth { get; set; }
        public Decimal interest { get; set; }


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


        public bool IsForeClose { get; set; }

        public DateTime ForeCloseDate { get; set; }

        public DateTime ForeCloseReverseDate { get; set; }

        public string Reason { get; set; }

        public LoanTransactionList LoanTransactionList
        {
            get
            {
                //if (object.ReferenceEquals(_loanTransactionList, null))
               // {
                    if (this.Id != Guid.Empty)
                    {LoanTransactionList trans= new LoanTransactionList(this.Id);
                    _loanTransactionList = trans;
                        //LoanMaster loanMaster = new LoanMaster(this.LoanMasterId, 0);
                        //if (loanMaster.IsInterest)
                        //{

                        //}
                        ///// this.LoanAmt//100000
                        //DateTime dtTempApply;
                        //if (_loanTransactionList.Count == 0)
                        //{
                        //    dtTempApply = this.ApplyDate;
                        //    dtTempApply = dtTempApply.AddMonths(-1);
                        //}
                        //else
                        //{
                        //    dtTempApply = _loanTransactionList.Max(u => u.AppliedOn);                            
                        //}
                        //if (_loanTransactionList.Count < this.NoOfMonths)
                        //{
                        //    int tempTranCount = this.NoOfMonths - _loanTransactionList.Count;
                        //    decimal tempLoanAmt = this.LoanAmt;
                        //    decimal tempInterest = 0;
                        //    for (int cnt = 0; cnt < tempTranCount; cnt++)
                        //    {
                        //        _loanTransactionList.Add(new LoanTransaction()
                        //        {
                        //            AmtPaid = this.AmtPerMonth,
                        //            AppliedOn = new DateTime(dtTempApply.AddMonths(cnt + 1).Year, dtTempApply.AddMonths(cnt + 1).Month, DateTime.DaysInMonth(dtTempApply.AddMonths(cnt + 1).Year, dtTempApply.AddMonths(cnt + 1).Month)), //dtTempApply.AddMonths(cnt + 1),
                        //            InterestAmt = loanMaster.IsInterest==true?Math.Round(tempLoanAmt * Convert.ToDecimal(loanMaster.InterestPercent) / 100 / 12,2):0,                                    
                        //            Id = Guid.Empty,
                        //            LoanEntryId = this.Id,
                        //            Status = "Not Paid"
                        //        });
                        //        if (loanMaster.IsInterest)
                        //        {
                        //            tempInterest = tempLoanAmt * Convert.ToDecimal(loanMaster.InterestPercent) / 100 / 12;
                        //            tempLoanAmt = tempLoanAmt - (this.AmtPerMonth - tempInterest);
                        //        }
                        //    }
                        //}
                    }
                    else
                        _loanTransactionList = new LoanTransactionList();
          //      }
                return _loanTransactionList;

            }
            set
            {
                _loanTransactionList = value;
            }
        }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the LoanEntry
        /// </summary>
        /// <returns></returns>
        public bool Save(bool foreClose=false )
        {
            

            SqlCommand sqlCommand = new SqlCommand("LoanEntry_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LoanMasterId", this.LoanMasterId);
            sqlCommand.Parameters.AddWithValue("@LoanDate", this.LoanDate);
            sqlCommand.Parameters.AddWithValue("@ApplyDate", this.ApplyDate);
            sqlCommand.Parameters.AddWithValue("@LoanAmt", this.LoanAmt);
            sqlCommand.Parameters.AddWithValue("@NoOfMonths", this.NoOfMonths);
            sqlCommand.Parameters.AddWithValue("@AmtPerMonth", this.AmtPerMonth);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@ForeClose", foreClose);
            sqlCommand.Parameters.AddWithValue("@IsForeClose", this.IsForeClose);
            sqlCommand.Parameters.AddWithValue("@ForeCloseDate", this.ForeCloseDate == DateTime.MinValue ? DateTime.Now : this.ForeCloseDate);
            sqlCommand.Parameters.AddWithValue("@ForeCloseReverseDate", this.ForeCloseReverseDate == DateTime.MinValue ? DateTime.Now : this.ForeCloseReverseDate);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            this.Id =new Guid( outValue);
            
            if (this.ForeCloseDate == DateTime.MinValue && this.ForeCloseReverseDate == DateTime.MinValue && this.Reason==null)
            {
                //this.Id = new Guid(outValue);
                //if (!foreClose)
                //{
                //    LoanTransactionList lon = new LoanTransactionList(this.Id);
                //    lon.ForEach(l =>
                //    {
                //        l.Delete();
                //    });
                //}
                //Createdby Mubarak on 24/03/2018
                //It was created in order to solve the issue in loan deviation
                //First you will create loan entry and deviate the loan transaction then save.
                //After that you will edit loan entry and save, the deviated loan transaction remains in the entry.
                //This stored procedure is used as one of the part to solve the issue.
                //Start Here
                LoanTransaction ObjLoanTrans = new LoanTransaction();
                ObjLoanTrans.LoanEntryId = this.Id;
                status = ObjLoanTrans.UpdateLoanTransDev();
                //End Here
                for (int startMonth = 0; startMonth < this.NoOfMonths; startMonth++)
                {
                    LoanTransaction loanTran = new LoanTransaction();
                    loanTran.LoanEntryId = this.Id;
                    loanTran.Status = "UnPaid";
                    loanTran.isPayRollProcess = false;
                    loanTran.isForClose = false;
                    loanTran.AmtPaid = this.AmtPerMonth;
                    loanTran.InterestAmt = this.interest;
                    DateTime ldate = this.ApplyDate.AddMonths(startMonth);
                    loanTran.AppliedOn = new DateTime(ldate.Year, ldate.Month, DateTime.DaysInMonth(ldate.Year, ldate.Month));
                    loanTran.CreatedBy = this.CreatedBy;
                    status= loanTran.Save();
                }
                //status= true;
                return status;
            }
            else if(this.IsForeClose==true && this.ForeCloseDate != DateTime.MinValue)
            {
                LoanTransaction loantr = new LoanTransaction();
                loantr.LoanEntryId = this.Id;                
                LoanTransactionList translist = new LoanTransactionList(this.Id);
                translist.ForEach(loan =>
                {
                    if (loan.Status != "Paid" && loan.isPayRollProcess != true)
                    {
                        loantr = loan;
                        loantr.Id = loan.Id;
                        loantr.Status = "Paid";
                        loantr.isForClose = true;
                        loantr.ModifiedBy = this.ModifiedBy;
                        status = loantr.Save();
                    }

                });


                //DataTable dtval = loantr.GetTableValues(loantr.Id, loantr.LoanEntryId);
                //for(int i=0; i<dtval.Rows.Count;i++)
                //{
                //    loantr.Status = Convert.ToString(dtval.Rows[i]["Status"]);
                //    loantr.isPayRollProcess = Convert.ToBoolean(dtval.Rows[i]["isPayRollProcess"]);
                //    if(loantr.Status!="Paid" && loantr.isPayRollProcess!=true)
                //    {
                //        loantr.Status = "Paid";
                //        loantr.isForClose = true;
                //        loantr.isPayRollProcess = false;
                //        if (!string.IsNullOrEmpty(Convert.ToString(dtval.Rows[i]["Id"])))
                //            loantr.Id = new Guid(Convert.ToString(dtval.Rows[i]["Id"]));
                //        if (!string.IsNullOrEmpty(Convert.ToString(dtval.Rows[i]["LoanEntryId"])))
                //            loantr.LoanEntryId = new Guid(Convert.ToString(dtval.Rows[i]["LoanEntryId"]));
                //        if (!string.IsNullOrEmpty(Convert.ToString(dtval.Rows[i]["AmtPaid"])))
                //            loantr.AmtPaid = Convert.ToDecimal(dtval.Rows[i]["AmtPaid"]);
                //        if (!string.IsNullOrEmpty(Convert.ToString(dtval.Rows[i]["InterestAmt"])))
                //            loantr.InterestAmt = Convert.ToDecimal(dtval.Rows[i]["InterestAmt"]);
                //        if (!string.IsNullOrEmpty(Convert.ToString(dtval.Rows[i]["AppliedOn"])))
                //            loantr.AppliedOn = Convert.ToDateTime(dtval.Rows[i]["AppliedOn"]);
                //        loantr.ModifiedBy = this.CreatedBy;
                //        status = loantr.Save();
                //    }
                //}
                //status = true;
                return status;
                //LoanTransaction loantransaction = new LoanTransaction(loantr.Id, loantr.LoanEntryId);
            }
            else if(this.IsForeClose != true && this.ForeCloseReverseDate != DateTime.MinValue)
            {
                LoanTransaction loantr = new LoanTransaction();
                loantr.LoanEntryId = this.Id;
                LoanTransactionList translist = new LoanTransactionList(this.Id);
                translist.ForEach(loan =>
                {
                    if (loan.Status == "Paid" && loan.isPayRollProcess != true && loan.isForClose == true)
                    {
                        loantr = loan;
                        loantr.Id = loan.Id;
                        loantr.Status = "UnPaid";
                        loantr.isForClose = false;                      
                        loantr.ModifiedBy = this.ModifiedBy;
                        status = loantr.Save();
                    }

                });

                //bool result=false;
                //LoanTransaction loantr = new LoanTransaction();
                //loantr.LoanEntryId = this.Id;
                //LoanTransactionList translistrf = new LoanTransactionList(this.Id);
                //translistrf.ForEach(u =>
                //{
                //    //var reversecheck = translistrf.Where(s => s.isForClose == true && s.isPayRollProcess == true).ToList();
                //    //if ( reversecheck.Count !=0)
                //    //{
                //    //    status = false;
                        
                //    //}
                //    //else
                //    //{
                //        LoanTransaction loantrrf = new LoanTransaction();
                //        loantrrf.LoanEntryId = this.Id;
                //        loantrrf.Id = Guid.Empty;
                //        DataTable dtvalue = loantrrf.GetTableValues(loantr.Id, loantr.LoanEntryId);
                //        for (int i = 0; i < dtvalue.Rows.Count; i++)
                //        {
                //            loantrrf.Status = Convert.ToString(dtvalue.Rows[i]["Status"]);
                //            loantrrf.isPayRollProcess = Convert.ToBoolean(dtvalue.Rows[i]["isPayRollProcess"]);
                //            loantrrf.isForClose = Convert.ToBoolean(dtvalue.Rows[i]["isForClose"]);
                //            if (loantrrf.Status == "Paid" && loantrrf.isPayRollProcess != true && loantrrf.isForClose == true)
                //            {
                //                loantrrf.Status = "UnPaid";
                //                loantrrf.isForClose = false;
                //                //loantrrf.isPayRollProcess = false;
                //                if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["Id"])))
                //                    loantrrf.Id = new Guid(Convert.ToString(dtvalue.Rows[i]["Id"]));
                //                if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["LoanEntryId"])))
                //                    loantrrf.LoanEntryId = new Guid(Convert.ToString(dtvalue.Rows[i]["LoanEntryId"]));
                //                if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["AmtPaid"])))
                //                    loantrrf.AmtPaid = Convert.ToDecimal(dtvalue.Rows[i]["AmtPaid"]);
                //                if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["InterestAmt"])))
                //                    loantrrf.InterestAmt = Convert.ToDecimal(dtvalue.Rows[i]["InterestAmt"]);
                //                if (!string.IsNullOrEmpty(Convert.ToString(dtvalue.Rows[i]["AppliedOn"])))
                //                    loantrrf.AppliedOn = Convert.ToDateTime(dtvalue.Rows[i]["AppliedOn"]);
                //                loantrrf.ModifiedBy = this.CreatedBy;
                //                status = loantrrf.Save();
                                
                //            }
                //        }
                //        //status = true;
                //        //return status;
                        
                //    //}
                //});
                return status;
            }
            return status;
        }

        /// <summary>
        /// Delete the LoanEntry
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LoanEntry_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool ForeClose()
        {
            this.IsForeClose = true;
            this.ForeCloseDate = this.ForeCloseDate == DateTime.MinValue ? DateTime.Now : this.ForeCloseDate;
            return this.Save(true);

        }
        public bool ForeCloseReverse()
        {
            this.IsForeClose = false;
            this.ForeCloseReverseDate = this.ForeCloseReverseDate == DateTime.MinValue ? DateTime.Now : this.ForeCloseReverseDate;
            return this.Save(true);

        }

        public double EMICalculation(int numberOfPayments, double loanAmount, double yearlyInterestRate)
        {
            double rate = yearlyInterestRate / 100 / 12;
            double denaminator = Math.Pow((1 + rate), numberOfPayments) - 1;
            return (rate + (rate / denaminator)) * loanAmount;
        }

        public double InterestAmtCalculation(double principle,double emi, double yearlyInterestRate,int NoofMonth,DateTime loanDate, DateTime PaidDate)
        {
            double tempprincipleatm = principle;
            double interestatm = 0; ;
            for (int cnt = 0; cnt < NoofMonth;cnt++)
            {                
                interestatm = tempprincipleatm * yearlyInterestRate / 100 / 12;
                tempprincipleatm = tempprincipleatm - (emi - interestatm);

                if (loanDate.Month == PaidDate.Month && loanDate.Year == PaidDate.Year) //if it match 
                    break;
            }
            return interestatm;
        }

        #endregion

        #region private methods


        /// <summary>
        /// Select the LoanEntry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid loanMasterId, Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("LoanEntry_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@LoanMasterId", loanMasterId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

