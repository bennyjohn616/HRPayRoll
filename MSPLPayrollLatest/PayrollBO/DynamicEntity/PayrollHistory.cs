// -----------------------------------------------------------------------
// <copyright file="PayrollHistory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using Leave;
    using SQLDBOperation;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.WebControls;
    using TaxActivities;
    using TraceError;


    /// <summary>
    /// To handle the PayrollHistory
    /// </summary>
    public class PayrollHistory
    {

        #region private variable

        private PayrollHistoryValueList _payrollHistoryValueList;
        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PayrollHistory()
        {

        }
        /// <summary>
        /// Modified By:Sharmila
        /// </summary>
        /// <param name="entityId"></param>
        public PayrollHistory(Guid entityId)
        {
            DataTable dtValue = this.GetTableValues(entityId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
            }

        }
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PayrollHistory(Guid id, int CompanyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, CompanyId, Guid.Empty, 0, 0);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
            }
        }

        public PayrollHistory(int CompanyId, Guid employeeId, int year, int month)
        {
            DataTable dtValue = this.GetTableValues(Guid.Empty, CompanyId, employeeId, month, year);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                this.Status = Convert.ToString(dtValue.Rows[0]["Status"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsFandF"])))
                    this.IsFandF = Convert.ToBoolean(dtValue.Rows[0]["IsFandF"]);
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
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the EntityId
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or Set the Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or Set the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }



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
        /// Get or Set the ModifeidOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        public PayrollHistoryValueList PayrollHistoryValueList
        {
            get
            {
                if (object.ReferenceEquals(_payrollHistoryValueList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _payrollHistoryValueList = new PayrollHistoryValueList(this.Id);
                    }
                    else
                        _payrollHistoryValueList = new PayrollHistoryValueList();
                }
                return _payrollHistoryValueList;

            }
            set
            {
                _payrollHistoryValueList = value;
            }
        }

        public bool IsFandF { get; set; }
        public PayrollHistoryValueList currentPayValues { get; set; }
        private Entity _curentEntity { get; set; }
        private int currMonth { get; set; }
        private int currYear { get; set; }
        public string Importxmlstring { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the PayrollHistory
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistory_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", 0);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsFandF", this.IsFandF);
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
        /// Delete the PayrollHistory
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("PayrollHistory_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        private decimal GetLoanAmount(int companyId, Guid employeeId, Guid attributeModelId, int year, int month)
        {
            LoanMaster loanMasterr = new LoanMaster(companyId, attributeModelId);
            LoanEntryList loanEntryList = new LoanEntryList();//employeeId, loanMasterr.Id);
            if (loanMasterr.Id != Guid.Empty)
            {
                loanEntryList = new LoanEntryList(employeeId, loanMasterr.Id);
            }

            decimal loanAmount = 0;
            loanEntryList.ForEach(p =>
            {

                p.ApplyDate = p.ApplyDate.AddDays(-p.ApplyDate.Day);
                DateTime payTime = new DateTime(year, month, 1);
                if (p.ApplyDate <= payTime)
                {
                    var tranlist = p.LoanTransactionList.Where(q => q.Status.ToLower().Trim() == "unpaid").ToList();
                    if (!object.ReferenceEquals(tranlist, null) && tranlist.Count() >= 1)
                    {
                        bool isExist = false;
                        tranlist.ToList().ForEach(s =>
                        {
                            DateTime dtTempApply = new DateTime(s.AppliedOn.Year, s.AppliedOn.Month, 1);
                            if (payTime == dtTempApply)
                            {
                                isExist = true;
                                loanAmount = loanAmount + s.AmtPaid;
                            }
                        });
                        if (!isExist)
                        {
                            //comment by mubarak 
                            //1st mnth payroll processed after don't processed for 2nd month skip to other mnth to payroll processing it will deducting for that month(but actually it should not deduct.)
                            //loanAmount = loanAmount + p.AmtPerMonth;
                            loanAmount = 0;
                        }
                    }
                    else
                    {
                        bool isExist = false;
                        tranlist.ToList().ForEach(s =>
                        {
                            DateTime dtTempApply = new DateTime(s.AppliedOn.Year, s.AppliedOn.Month, 1);
                            if (payTime == dtTempApply)
                            {
                                isExist = true;
                                loanAmount = loanAmount + s.AmtPaid;
                            }
                        });
                        if (!isExist)
                        {
                            loanAmount = p.LoanTransactionList.Count < p.NoOfMonths ? loanAmount + p.AmtPerMonth : loanAmount;
                        }
                    }
                    //int mothDif = Math.Abs((p.ApplyDate.Month - payTime.Month) + 12 * (p.ApplyDate.Year - payTime.Year));
                    //if (mothDif <= p.NoOfMonths)
                    //{
                    //    loanAmount = loanAmount + p.AmtPerMonth;//need to check the if condition
                    //}
                }

                // if(p.LoanDate>)

            });
            return loanAmount;
        }

        private void CalculateIncrement(int companyId, Guid employeeId, EntityAttributeModel u, ref List<FormulaRecursive> lstFormulaRecursive, ref List<ArrearHistory> saveArrearHistory, EntityBehavior entityBehavior, Entity entity, AttributeModelList attributemodelList, int month, int year, ref ArrearHistoryList arrPF)
        {
            IncrementList incrementlist = new IncrementList(employeeId);
            List<ArrearHistory> arrearList = new List<ArrearHistory>();
            List<Increment> processIncrement = new List<Increment>();
            //  List<Increment> incremets = new List<Increment>();


            List<Increment> incremets = incrementlist.Where(p => p.IsProcessed == false && p.ApplyMonth == month && p.ApplyYear == year).OrderBy(p => p.EffectiveDate).ToList();
            //*** 2nd increment effective from date 1st
            //for (int inc = 0; inc < Curincremets.Count; inc++)
            //{
            //    incremets.AddRange(incrementlist.Where(p => p.IsProcessed && p.EffectiveDate >= Curincremets[inc].EffectiveDate & p.ApplyMonth != month && p.ApplyYear != year).OrderBy(W => W.EffectiveDate).ToList());
            //    incremets.ForEach(i =>
            //    {

            //        i.IncrementDetailList.ForEach(d =>
            //        {
            //            if (i.EffectiveDate.Day != 1)
            //            {
            //                d.OldValue = Curincremets[inc].IncrementDetailList.Where(w => w.AttributeModelId == d.AttributeModelId).FirstOrDefault().OldValue;
            //            }
            //            d.NewValue = Curincremets[inc].IncrementDetailList.Where(w => w.AttributeModelId == d.AttributeModelId).FirstOrDefault().NewValue;

            //        });
            //    });
            //    incremets.Add(Curincremets[inc]);

            //}

            double totalArrearVal = 0.0;
            double totalCurrentMonthVal = 0.0;
            Guid arrMatchId = Guid.Empty;
            if (entityBehavior != null && arrMatchId == Guid.Empty)
            {
                arrMatchId = entityBehavior.ArrearAttributeModelId;




            }
            if (incremets.Count > 0)
            {

                DateTime tempEffDate = incremets[incremets.Count - 1].EffectiveDate;

                #region "Previous month values"
                /*if (tempEffDate.Month != month && tempEffDate.Year != year) // need to modify for  prvs month arr value
                   {
                       DateTime currntmnth = new DateTime(tempEffDate.Year, tempEffDate.Month, 1);
                       List<Increment> previous = incrementlist.Where(p => p.EffectiveDate >= tempEffDate).OrderBy(p => p.EffectiveDate).ToList();
                       previous.ForEach(p =>
                       {
                           PayrollHistory payHisArr = new PayrollHistory(companyId, employeeId, tempEffDate.Year, tempEffDate.Month);
                           var temp = payHisArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == arrMatchId).FirstOrDefault();
                           if (!object.ReferenceEquals(temp, null))
                               prevArrAmount = Convert.ToDouble(temp.Value);
                       });
                   }*/
                #endregion
                for (int incCount = 0; incCount < incremets.Count();)
                {

                    Increment incTemp = incremets[incCount];

                    DateTime tempProcessDate = new DateTime(year, month, 1);
                    DateTime tempEffectiveDate = new DateTime(incTemp.EffectiveDate.Year, incTemp.EffectiveDate.Month, incTemp.EffectiveDate.Day);
                    double arrearVal = 0.0;
                    double currentMonthArrearVal = 0.0;

                    if (tempEffectiveDate <= tempProcessDate || (tempEffectiveDate.Month == tempProcessDate.Month && tempEffectiveDate.Year == tempProcessDate.Year))//10/10/2015 <10/10/2015 
                    {


                        double prevArrAmount = 0;
                        do
                        {

                            if (!object.ReferenceEquals(entityBehavior, null))
                            {
                                int MDDay = 0;

                                double PDDay = 0;
                                double prevAmount = 0;

                                PayrollHistory payArr = new PayrollHistory(companyId, employeeId, tempEffectiveDate.Year, tempEffectiveDate.Month);
                                var atrTmp = attributemodelList.Where(am => am.Name == "MD").FirstOrDefault();
                                var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                if (!object.ReferenceEquals(colTemp, null))
                                    MDDay = Convert.ToInt32(colTemp.Value);
                                atrTmp = attributemodelList.Where(am => am.Name == "PD").FirstOrDefault();
                                colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                if (!object.ReferenceEquals(colTemp, null))
                                    PDDay = Convert.ToDouble(colTemp.Value);
                                colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                if (!object.ReferenceEquals(colTemp, null))
                                    prevAmount = Convert.ToDouble(colTemp.BaseValue);
                                var arrlist = new ArrearHistoryList(employeeId, payArr.Id).ToList();
                                DateTime arrdate = tempEffectiveDate;//.AddMonths(1);
                                var arrval = arrlist.Where(r => r.Month == arrdate.Month && r.Year == arrdate.Year && r.AttributeModelId == arrMatchId).FirstOrDefault();
                                if (!object.ReferenceEquals(arrval, null))
                                {
                                    prevArrAmount = Convert.ToDouble(arrval.Value);
                                }
                                else { prevArrAmount = 0; }


                                double pdDays = 0;
                                if (tempEffectiveDate.Day != 1)
                                {

                                    // int previousDays = tempEffectiveDate.Day; //// Modified
                                    //  pdDays = DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month) - previousDays;
                                    //    PaidDays = Convert.ToDateTime((DateTime.Parse((DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month)).ToString() + "/" + Month.ToString() + "/" + Year.ToString()))) - tempEffectiveDate ;

                                    pdDays = (Convert.ToDateTime(((DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month)) + "/" + tempEffectiveDate.Month + "/" + tempEffectiveDate.Year), new CultureInfo("en-GB")) - Convert.ToDateTime(tempEffectiveDate)).TotalDays;
                                    pdDays = pdDays + 1;

                                    if (incCount <= incremets.Count() - 1)
                                    {
                                        Increment incNextTemp1 = incremets[incCount];
                                        if (tempEffectiveDate.Month == incNextTemp1.EffectiveDate.Month && incNextTemp1.EffectiveDate.Year == tempEffectiveDate.Year)
                                        {
                                            //pdDays = incNextTemp1.EffectiveDate.Day - previousDays;
                                            //  tempEffectiveDate = incNextTemp1.EffectiveDate;
                                            incCount = incCount + 1;
                                        }
                                        else
                                        {
                                            DateTime dtNextstartDate = new DateTime(tempEffectiveDate.Year, tempEffectiveDate.Month, DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month));
                                            // tempEffectiveDate = dtNextstartDate.AddDays(1);
                                        }
                                        pdDays = pdDays - incTemp.AfterLop;
                                    }


                                }
                                else
                                {
                                    pdDays = PDDay;
                                    if (incCount <= incremets.Count() - 1)
                                    {
                                        Increment incNextTemp1 = incremets[incCount];
                                        if (tempEffectiveDate.Month == incNextTemp1.EffectiveDate.Month && incNextTemp1.EffectiveDate.Year == tempEffectiveDate.Year)
                                        {
                                            // pdDays = incNextTemp1.EffectiveDate.Day - 1;///1-1=0
                                            //  tempEffectiveDate = incNextTemp1.EffectiveDate;
                                            incCount = incCount + 1;
                                        }
                                        else
                                        {
                                            DateTime dtNextstartDate = new DateTime(tempEffectiveDate.Year, tempEffectiveDate.Month, DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month));
                                            //   tempEffectiveDate = dtNextstartDate.AddDays(1);
                                        }
                                        if (tempEffectiveDate.Month == month && tempEffectiveDate.Year == year)
                                        {
                                            pdDays = pdDays - incTemp.BeforeLop;
                                        }
                                    }
                                }
                                var incDet = incTemp.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                if (tempEffectiveDate.Month != month && tempEffectiveDate.Year != year)
                                {

                                }
                                else
                                {
                                    //   incDet.OldValue = incDet.OldValue;// + (decimal)prevArrAmount;

                                    if (prevAmount == 0 && (tempEffectiveDate.Month != month && tempEffectiveDate.Year != year))
                                    {
                                        incDet.OldValue = (decimal)prevAmount;
                                    }
                                    else if (prevAmount != 0)
                                    {
                                        incDet.OldValue = (decimal)prevAmount;// + (decimal)prevArrAmount;
                                    }
                                }
                                string strOut = "0.0";
                                if (!object.ReferenceEquals(incDet, null) && MDDay != 0 && pdDays != 0)
                                {
                                    double newincrement = (double)incDet.NewValue / MDDay * pdDays;
                                    double oldincrement = (double)incDet.OldValue / MDDay * pdDays;

                                    double dif = (double)(newincrement - oldincrement);
                                    strOut = dif.ToString();
                                }
                                Eval eval = new Eval();
                                double result = double.Parse(eval.Execute(strOut));
                                if (tempEffectiveDate.Month == month && tempEffectiveDate.Year == year && tempEffectiveDate.Day > 1)
                                {
                                    // Created by Ajithpanner on 7/12/17 
                                    if (tempEffectiveDate.Day != 1 && incremets.Count() > 1)
                                    {

                                        //    int previousDays = tempEffectiveDate.Day;
                                        // pdDays = DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month) - previousDays;
                                        // pdDays = (Convert.ToDateTime((DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month)) + "/" + tempEffectiveDate.Month + "/" + tempEffectiveDate.Year) - Convert.ToDateTime(tempEffectiveDate)).TotalDays;
                                        double newincrement = 0.0;
                                        MDDay = DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month);
                                        if (!object.ReferenceEquals(incDet, null) && MDDay != 0 && pdDays != 0)
                                        {
                                            double oldincrement = (double)incDet.OldValue / MDDay * (MDDay - pdDays - incTemp.AfterLop - incTemp.BeforeLop);
                                            int nextpdy = 0;
                                            for (int inc = 0; inc < incremets.Count(); inc++)
                                            {
                                                pdDays = 0;
                                                DateTime currpd = incremets[inc].EffectiveDate;
                                                DateTime nextpd;

                                                if (inc + 1 != incremets.Count())
                                                {
                                                    nextpd = incremets[inc + 1].EffectiveDate;
                                                    nextpdy = 1;
                                                }
                                                else
                                                {
                                                    nextpd = incremets[inc].EffectiveDate;
                                                    nextpdy = currpd.Day - nextpd.Day;
                                                }
                                                if (nextpdy == 0)
                                                {
                                                    pdDays = MDDay - currpd.Day + 1;
                                                }
                                                else
                                                {
                                                    pdDays = nextpd.Day - currpd.Day;
                                                }
                                                var newal = incremets[inc].IncrementDetailList.Where(inee => inee.AttributeModelId == u.AttributeModelId).FirstOrDefault().NewValue;
                                                newincrement += (double)newal / MDDay * (pdDays);

                                            }

                                            double tot = ((newincrement + oldincrement) / (MDDay - incTemp.AfterLop - incTemp.BeforeLop)) * MDDay;
                                            double dif = Math.Abs((double)incDet.OldValue - tot);
                                            strOut = dif.ToString();
                                            incCount = incremets.Count();
                                        }
                                    }
                                    if (tempEffectiveDate.Day != 1 && incremets.Count() == 1)
                                    {

                                        //    int previousDays = tempEffectiveDate.Day;
                                        // pdDays = DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month) - previousDays;
                                        // pdDays = (Convert.ToDateTime((DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month)) + "/" + tempEffectiveDate.Month + "/" + tempEffectiveDate.Year) - Convert.ToDateTime(tempEffectiveDate)).TotalDays;
                                        MDDay = DateTime.DaysInMonth(tempEffectiveDate.Year, tempEffectiveDate.Month);
                                        if (!object.ReferenceEquals(incDet, null) && MDDay != 0 && pdDays != 0)
                                        {
                                            double newincrement = (double)incDet.NewValue / MDDay * (pdDays);
                                            double oldincrement = (double)incDet.OldValue / MDDay * (MDDay - pdDays - incTemp.AfterLop - incTemp.BeforeLop);
                                            double tot = ((newincrement + oldincrement) / (MDDay - incTemp.AfterLop - incTemp.BeforeLop)) * MDDay;
                                            double dif = Math.Abs((double)incDet.OldValue - tot);
                                            if ((double)incDet.NewValue < (double)incDet.OldValue)
                                            {
                                                dif = dif * -1;
                                            }
                                            strOut = dif.ToString();
                                        }
                                    }

                                    result = (double.Parse(eval.Execute(strOut)));
                                    currentMonthArrearVal = currentMonthArrearVal + result;
                                }
                                else
                                {
                                    arrearVal = arrearVal + result - prevArrAmount;
                                }
                                // tempEffectiveDate.AddMonths(1);
                                DateTime ndate = tempEffectiveDate.AddMonths(1);
                                tempEffectiveDate = new DateTime(ndate.Year, ndate.Month, 1);

                            }
                            // ELSE ADDED for  if behaviour is null loop is endless on 03/29/2018
                            else
                            {
                                incCount = incCount + 1;
                            }

                        } while (tempEffectiveDate < tempProcessDate);
                        totalArrearVal += arrearVal;
                        totalCurrentMonthVal += currentMonthArrearVal;
                        ArrearHistory arr = new ArrearHistory();
                        arr.AttributeModelId = arrMatchId;
                        arr.EmployeeId = employeeId;
                        arr.Value = Convert.ToDecimal(arrearVal);
                        arr.Month = tempEffectiveDate.Month;
                        arr.Year = tempEffectiveDate.Year;
                        if (arrearVal != 0)
                        {
                            saveArrearHistory.Add(arr);
                            arr.AttributeModelId = u.AttributeModelId;
                            arrPF.Add(arr);
                        }


                        processIncrement.Add(incTemp);
                    }



                }

            }
            FormulaRecursive roundoff = new FormulaRecursive();
            if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault(), null))
            {
                var tr = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault();
                if (entityBehavior != null)
                {

                    totalArrearVal = roundoff.RoundOff(totalArrearVal, entityBehavior.RoundingId);
                    lstFormulaRecursive.Add(new FormulaRecursive()
                    {
                        Assignedvalues = Convert.ToString(roundoff.RoundOff(totalArrearVal, entityBehavior.RoundingId)),
                        Id = arrMatchId.ToString(),
                        Name = tr != null ? tr.AttributeModel.Name : "",
                        ExecuteOrder = 1,
                        Rounding = entityBehavior.RoundingId,
                        BaseValue = Convert.ToString(roundoff.RoundOff(totalArrearVal, entityBehavior.RoundingId))
                    });
                }
            }
            else
            {
                lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = Convert.ToString(roundoff.RoundOff(totalArrearVal, entityBehavior.RoundingId));
                lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().BaseValue = Convert.ToString(roundoff.RoundOff(totalArrearVal, entityBehavior.RoundingId));
                lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().ExecuteOrder = 1;
            }
            if (processIncrement.Count() > 0)//update new value to the employee master
            {
                if (totalCurrentMonthVal != 0)//update the current month increment with old value
                {

                    for (int pic = 0; pic < processIncrement.Count; pic++)
                    {

                        if (processIncrement[pic].EffectiveDate.Month == month && processIncrement[pic].EffectiveDate.Year == year)
                        {







                            //assigning new incremented new value to the specific field(Master Field)
                            if (!object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                            {
                                var t1 = processIncrement[pic].IncrementDetailList.Where(q => q.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                if (t1 != null)
                                {
                                    double curMonthVal = Convert.ToDouble(t1.OldValue) + totalCurrentMonthVal;
                                    lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = t1.NewValue.ToString(); /// Modified
                                    lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().BaseValue = curMonthVal.ToString();
                                }
                            }
                        }
                    }
                }
                else
                {
                    var maxVal = processIncrement.Max(mx => mx.EffectiveDate);//Max effetive date and apply the New value
                    var maxElem = processIncrement.Where(a => a.EffectiveDate == maxVal).FirstOrDefault();
                    if (!object.ReferenceEquals(maxElem, null))
                    {



                        //assigning new incremented new value to the specific field(Master Field)
                        if (!object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                        {
                            var t1 = maxElem.IncrementDetailList.Where(q => q.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                            if (t1 != null)
                            {
                                lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = t1.NewValue.ToString();
                                lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().BaseValue = t1.NewValue.ToString();
                            }
                        }
                    }
                }
            }
            else
            {//if increment but apply month & year is not ,updating 0 as arrear

                if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                {
                    var tr = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault();
                    if (entityBehavior != null)
                    {
                        lstFormulaRecursive.Add(new FormulaRecursive()
                        {
                            Assignedvalues = entityBehavior.Formula,
                            Id = u.AttributeModelId.ToString(),
                            Name = tr != null ? tr.AttributeModel.Name : "",

                            ExecuteOrder = 4,
                            Rounding = entityBehavior.RoundingId
                        });

                        lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = entityBehavior.Formula;
                        lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Percentage = entityBehavior.Percentage;
                    }
                }
            }
        }
        private void CalculateIncrement_old(int companyId, Guid employeeId, EntityAttributeModel u, ref List<FormulaRecursive> lstFormulaRecursive, EntityBehavior entityBehavior, Entity entity, AttributeModelList attributemodelList, int month, int year)
        {
            IncrementList incrementlist = new IncrementList(employeeId);
            List<Increment> processIncrement = new List<Increment>();
            double totalArrearVal = 0.0;
            Guid arrMatchId = Guid.Empty;
            if (arrMatchId == Guid.Empty)
            {
                arrMatchId = entityBehavior.ArrearAttributeModelId;
            }
            for (int incCount = 0; incCount < incrementlist.Count(); incCount++)
            {
                Increment inc = incrementlist[incCount];
                if (inc.IsProcessed == false && inc.ApplyMonth == month && inc.ApplyYear == year) // && incrementlist[0].EffectiveDate <= DateTime.Now)
                {

                    if (arrMatchId == Guid.Empty)
                    {
                        arrMatchId = entityBehavior.ArrearAttributeModelId;
                    }
                    double arrearVal = 0.0;
                    if (inc.EffectiveDate.Day != 1)
                    {
                        decimal arrearMidofPay = GetMidOfPayment(companyId, employeeId, inc.EffectiveDate, inc, u, entityBehavior, entity, attributemodelList, month, year);
                        arrearVal = Convert.ToDouble(arrearMidofPay);
                        inc.EffectiveDate = inc.EffectiveDate.AddMonths(1);
                        inc.EffectiveDate = new DateTime(inc.EffectiveDate.Year, inc.EffectiveDate.Month, 1);
                    }
                    DateTime tempProcessDate = new DateTime(year, month, 1);
                    DateTime tempEffectiveDate = new DateTime(inc.EffectiveDate.Year, inc.EffectiveDate.Month, 1);

                    if (tempEffectiveDate < tempProcessDate)//10/10/2015 <10/10/2015 
                    {
                        do
                        {
                            if (!object.ReferenceEquals(entityBehavior, null))
                            {
                                var incDet = inc.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                string strOut = "0.0";
                                if (!object.ReferenceEquals(incDet, null))
                                {
                                    double dif = (double)(incDet.NewValue - incDet.OldValue);
                                    strOut = dif.ToString();
                                }
                                PayrollHistory payArr = new PayrollHistory(companyId, employeeId, tempEffectiveDate.Year, tempEffectiveDate.Month);
                                var atrTmp = attributemodelList.Where(am => am.Name == "MD").FirstOrDefault();
                                var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                if (!object.ReferenceEquals(colTemp, null))
                                    strOut = strOut + "/" + colTemp.Value;
                                atrTmp = attributemodelList.Where(am => am.Name == "PD").FirstOrDefault();
                                colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                if (!object.ReferenceEquals(colTemp, null))
                                    strOut = strOut + "*" + colTemp.Value;
                                Eval eval = new Eval();
                                double result = double.Parse(eval.Execute(strOut));
                                arrearVal = arrearVal + result;
                                tempEffectiveDate.AddMonths(1);

                            }
                        } while (tempEffectiveDate < tempProcessDate);
                        totalArrearVal += arrearVal;
                        processIncrement.Add(inc);
                    }
                    else if (tempEffectiveDate.Year == tempProcessDate.Year && tempEffectiveDate.Month == tempProcessDate.Month)//same month increment but not arrear
                    {
                        totalArrearVal += arrearVal;
                        processIncrement.Add(inc);
                    }

                    /*
                    int effYear = inc.EffectiveDate.Year;
                    int effMonth = inc.EffectiveDate.Month;
                    if (effYear <= year)
                    {

                        do
                        {
                            if (effMonth < month)
                            {
                                if (!object.ReferenceEquals(entityBehavior, null))
                                {
                                    do//month do
                                    {
                                        var incDet = inc.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                        string strOut = "0.0";
                                        if (!object.ReferenceEquals(incDet, null))
                                        {
                                            double dif = (double)(incDet.NewValue - incDet.OldValue);
                                            strOut = dif.ToString();
                                        }
                                        PayrollHistory payArr = new PayrollHistory(companyId, employeeId, effYear, effMonth);
                                        var atrTmp = attributemodelList.Where(am => am.Name == "MD").FirstOrDefault();
                                        var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                        if (!object.ReferenceEquals(colTemp, null))
                                            strOut = strOut + "/" + colTemp.Value;
                                        atrTmp = attributemodelList.Where(am => am.Name == "PD").FirstOrDefault();
                                        colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                        if (!object.ReferenceEquals(colTemp, null))
                                            strOut = strOut + "*" + colTemp.Value;
                                        Eval eval = new Eval();
                                        double result = eval.Execute(strOut);
                                        arrearVal = arrearVal + result;
                                        effMonth++;

                                    } while (effMonth < month);//month while
                                }
                            }
                            effYear = effYear + 1;
                            effMonth = 1;
                        } while (effYear <= year);
                        //assign arrear value to the arrear matched filed value
                        totalArrearVal += arrearVal;
                        processIncrement.Add(inc);

                    }*/

                }
            }//for loop end

            if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault(), null))
            {
                var tr = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault();
                lstFormulaRecursive.Add(new FormulaRecursive()
                {
                    Assignedvalues = totalArrearVal.ToString(),
                    Id = arrMatchId.ToString(),
                    Name = tr != null ? tr.AttributeModel.Name : "",
                    ExecuteOrder = 4,
                    Rounding = entityBehavior.RoundingId
                });
            }
            else
            {
                lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = totalArrearVal.ToString();
            }
            if (processIncrement.Count() > 0)//update new value to the employee master
            {
                var maxVal = processIncrement.Max(mx => mx.EffectiveDate);//Max effetive date and apply the New value
                var maxElem = processIncrement.Where(a => a.EffectiveDate == maxVal).FirstOrDefault();
                if (!object.ReferenceEquals(maxElem, null))
                {
                    //assigning new incremented new value to the specific field(Master Field)
                    if (!object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                    {
                        var t1 = maxElem.IncrementDetailList.Where(q => q.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                        if (t1 != null)
                        {
                            lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = t1.NewValue.ToString();
                        }
                    }
                }
            }
            else
            {//if increment but apply month & year is not ,updating 0 as arrear

                if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                {
                    lstFormulaRecursive.Add(new FormulaRecursive()
                    {
                        Assignedvalues = entityBehavior.Formula,
                        Id = u.AttributeModelId.ToString(),
                        Name = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name,
                        ExecuteOrder = 4,
                        Rounding = entityBehavior.RoundingId
                    });
                    lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = entityBehavior.Formula;
                }
            }
        }

        private decimal GetMidOfPayment(int companyId, Guid employeeId, DateTime effectiveDate, Increment increment, EntityAttributeModel u, EntityBehavior entityBehavior, Entity entity, AttributeModelList attributemodelList, int month, int year)
        {


            PayrollHistory payArr = new PayrollHistory(companyId, employeeId, year, month);
            var atrTmp = attributemodelList.Where(am => am.Name == "LD").FirstOrDefault();
            var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
            string strLop = string.Empty;
            if (!object.ReferenceEquals(colTemp, null))
                strLop = strLop + "/" + colTemp.Value;
            else
            {
                MonthlyInputList monnthlylist = new MonthlyInputList(entityBehavior.EntityId, month, year);
                var t1 = monnthlylist.Where(s => s.AttributeModelId == atrTmp.Id && s.EmployeeId == employeeId).FirstOrDefault();
                if (!object.ReferenceEquals(t1, null))
                    strLop = t1.Value;
                else
                    strLop = "0";

            }
            double tottalLop = 0;//2
            double totalIncLop = increment.AfterLop + increment.BeforeLop;//3=0+3
            if (double.TryParse(strLop, out tottalLop))
            {
                if (totalIncLop != tottalLop)
                {
                    if (totalIncLop > tottalLop)
                    {
                        increment.AfterLop = increment.AfterLop - (totalIncLop - tottalLop);
                        increment.BeforeLop = totalIncLop - increment.AfterLop;
                    }
                    else if (totalIncLop < tottalLop)
                    {
                        increment.AfterLop = increment.AfterLop + (tottalLop - totalIncLop);
                        increment.BeforeLop = tottalLop - increment.AfterLop;
                    }
                }
            }

            int days = effectiveDate.Day;
            int totday = DateTime.DaysInMonth(effectiveDate.Year, effectiveDate.Month);
            decimal withinc = totday - (days + 1);
            decimal withOutInc = totday - withinc;

            var incDet = increment.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
            string strOut = "0.0";
            decimal oldvalue = 0;
            decimal newvalue = 0;
            if (!object.ReferenceEquals(incDet, null))
            {
                double dif = (double)(incDet.NewValue - incDet.OldValue);
                oldvalue = incDet.OldValue;
                newvalue = incDet.NewValue;
                strOut = dif.ToString();
            }

            decimal perdayOldValu = oldvalue / totday;
            decimal perdayNewValu = newvalue / totday;
            decimal withIncrementAmt = perdayNewValu * withinc - Convert.ToDecimal(increment.AfterLop);
            decimal withOutIncrementAmt = perdayOldValu * withOutInc - Convert.ToDecimal(increment.BeforeLop);
            return (withIncrementAmt + withOutIncrementAmt);
        }

        public Entity ExecuteProcessTemp(int companyId, Guid employeeId, int year, int month, Guid entityId, Guid entityModelId, out List<PayrollError> errors)
        {
            errors = new List<PayrollError>();
            DateTime startdate;
            PayrollHistory payrollHistory = new PayrollHistory(companyId, employeeId, year, month);
            if (!object.ReferenceEquals(payrollHistory, null) && (payrollHistory.Status == ComValue.payrollProcessStatus[0] || payrollHistory.Status == ComValue.payrollProcessStatus[1]))//"Processed"
            {
                entityId = payrollHistory.EntityId;
                entityModelId = payrollHistory.EntityModelId;
                PayrollHistoryValueList payrollHistoryValuelist = new PayrollHistoryValueList(payrollHistory.Id);
                Entity entityDone = new Entity(entityModelId, entityId);
                for (int cnt = 0; cnt < entityDone.EntityAttributeModelList.Count; cnt++)
                {
                    var payrollValue = payrollHistoryValuelist.Where(p => p.AttributeModelId == entityDone.EntityAttributeModelList[cnt].AttributeModelId || p.AttributeModelId == new Guid("3b676118-8c41-41d2-ad8d-843ad7da8b2b")).FirstOrDefault();
                    if (object.ReferenceEquals(payrollValue, null))
                    {
                        entityDone.EntityAttributeModelList.RemoveAt(cnt);
                        cnt = cnt - 2;
                        if (cnt < 0)
                        {
                            cnt = 0;
                        }
                    }
                    else
                    {
                        entityDone.EntityAttributeModelList[cnt].EntityAttributeValue.Value = payrollValue.Value;
                    }
                }
                return entityDone;//already done the process
            }
            //PayrollHistoryValueList payrollHistoryValue = new PayrollHistoryValueList(payrollHistory.Id);
            List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
            List<PayrollError> payrollErrors = new List<PayrollError>();
            Entity entity = new Entity(entityModelId, entityId);
            EntityBehaviorList entityBehaviorList = new EntityBehaviorList(entity.Id, entity.EntityModelId);
            var entitymasterValueslist = new EntityMasterValueList(employeeId, ComValue.EmployeeTable).Where(s => s.EntityModelId == entity.EntityModelId && s.EntityId == entity.Id).ToList();
            if (object.ReferenceEquals(entitymasterValueslist, null))
                entitymasterValueslist = new List<EntityMasterValue>();
            MonthlyInputList monthlyinputlist = new MonthlyInputList(entity.Id, employeeId, month, year);
            TaxComputationInfo taxInfo = new TaxComputationInfo();
            taxInfo.AttributemodelList = new AttributeModelList(companyId);
            TXFinanceYearList tXFinanceYears = new TXFinanceYearList(companyId);
            DateTime curdate = new DateTime(year, month, 1);
            taxInfo.FinanceYear = tXFinanceYears.Where(tf => curdate >= tf.StartingDate && curdate <= tf.EndingDate).FirstOrDefault();
            Employee employee = new Employee(companyId, employeeId);
            Category category = new Category(employee.CategoryId, companyId);
            IncrementList increment = new IncrementList(employeeId);
            entity.EntityAttributeModelList.RemoveAll(r => r.AttributeModel.FullAndFinalSettlement == true);
            try
            {


                entity.EntityAttributeModelList.ForEach(u =>
                {
                    if (u.AttributeModel.Name == "FCON" || u.AttributeModel.Name == "CON" || u.AttributeModel.Name == "FG")
                    {

                    }
                    if (u.AttributeModel.Name == "PF" || u.AttributeModel.Name == "TDS" || u.AttributeModel.Name == "FCA")
                    {
                        string str = string.Empty;
                    }
                    var entityBehavior = entityBehaviorList.Where(s => s.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                    if (u.AttributeModel.IsSetting == true)//Setting
                    {
                        if (u.AttributeModel.Name == "MD")//Setting
                        {
                            #region execute setting 
                            string mddays = GetMonthDay(u, category, monthlyinputlist, month, year, ref payrollErrors, false, out startdate).ToString();
                            //DateTime.DaysInMonth(year, month).ToString()
                            AddValuesTemp(u, ref lstFormulaRecursive, mddays, null, null, 2, 1, 1);
                            #endregion
                        }
                        else if (u.AttributeModel.IsMonthlyInput && u.AttributeModel.Name == "LD")
                        {
                            #region execute monthly input
                            var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            string monValue = monInput != null ? monInput.Value : "0";
                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, 1, 1);
                            #endregion
                        }
                        else
                        {
                            #region execute setting 
                            if (!object.ReferenceEquals(entityBehavior, null))
                            {
                                if (entityBehavior != null && entityBehavior.ValueType == 1)//Employee Master  input
                                {
                                    #region execute employee master input
                                    var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                    string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                    #endregion

                                }
                                else if (entityBehavior != null && entityBehavior.ValueType == 2)//Monthly input
                                {
                                    #region execute monthly input
                                    var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                    string assValues = monInput != null ? monInput.Value : "0";
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2);

                                    #endregion
                                }

                                else if (entityBehavior != null && entityBehavior.ValueType == 3)//Formula
                                {
                                    #region execute formula/percentage

                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);

                                    #endregion
                                }
                                else if (entityBehavior != null && entityBehavior.ValueType == 4)//Conditionl formula  --if else
                                {
                                    #region execute Conditionl formula  --if else
                                    string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                    #endregion
                                }
                                else if (entityBehavior != null && entityBehavior.ValueType == 5)//Range type
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                                }
                            }
                            else if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault(), null))
                            {
                                lstFormulaRecursive.Add(new FormulaRecursive()
                                {
                                    Assignedvalues = "0",
                                    Id = u.AttributeModelId.ToString(),
                                    Name = u.AttributeModel.Name,
                                    ExecuteOrder = 2,
                                    ParentId = Convert.ToString(u.AttributeModel.ParentId)
                                });
                            }
                            else
                            {
                                lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = "0";
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        if (u.AttributeModel.IsMonthlyInput)
                        {
                            #region execute monthly input
                            var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            string monValue = monInput != null ? monInput.Value : "0";
                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, 1, 1);
                            #endregion
                        }
                        else if (u.AttributeModel.IsInstallment)//Loan
                        {
                            #region execute loan
                            decimal loanAmount = GetLoanAmount(companyId, employeeId, u.AttributeModelId, year, month);
                            AddValuesTemp(u, ref lstFormulaRecursive, loanAmount.ToString(), null, null, 2, 1, 1);
                            #endregion
                        }

                        else if (u.AttributeModel.IsIncrement)//increment and arrear process
                        {
                            #region execute increment setting 
                            if (entityBehavior != null && entityBehavior.ValueType == 1)//Employee Master  input
                            {
                                #region execute employee master input
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                #endregion
                            }
                            List<ArrearHistory> SaveArrearHistory = new List<ArrearHistory>();
                            // CalculateIncrement(companyId, employeeId, u, ref lstFormulaRecursive, ref SaveArrearHistory, entityBehavior, entity, attributemodelList, month, year,);

                            #endregion
                        }
                        else if (!object.ReferenceEquals(entityBehavior, null))
                        {
                            if (entityBehavior != null && entityBehavior.ValueType == 1)//Employee Master  input
                            {
                                #region execute employee master input
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                #endregion

                            }
                            else if (entityBehavior != null && entityBehavior.ValueType == 2)//Monthly input
                            {
                                #region execute monthly input
                                var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = monInput != null ? monInput.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2);
                                #endregion
                            }
                            else if (entityBehavior != null && entityBehavior.ValueType == 3)//Formula
                            {
                                #region execute formula/percentage
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                                #endregion
                            }
                            else if (entityBehavior != null && entityBehavior.ValueType == 4)//Conditionl formula  --if else
                            {
                                #region execute Conditionl formula  --if else
                                string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                #endregion
                            }

                            else if (entityBehavior != null && entityBehavior.ValueType == 5)//Range type
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                            }
                        }
                        else
                        {
                            AddValuesTemp(u, ref lstFormulaRecursive, u.EntityAttributeValue.Value, null, null, 0, 1, 1);
                        }
                    }
                });
                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();
                if (payrollErrors.Count > 0)//dont process the formula execution
                {
                    errors = payrollErrors;
                    return new Entity();
                }
                lstFormulaRecursive.ForEach(u =>
                {
                    u.UserId = 0;
                    u.Year = year;
                    u.CompanyId = companyId;
                    u.Month = month;
                    u.EmployeeId = employeeId;
                });
                recursive(taxInfo, employee, lstFormulaRecursive, entity, increment, false);

                lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
                lstFormulaRecursive.ForEach(u =>
                {
                    string error = string.Empty;
                    string output = string.Empty;
                    string rerun = string.Empty;
                    if (u.Assignedvalues.IndexOf("{") >= 0)
                    {
                        u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output, rerun);
                    }
                    else
                    {
                        output = u.Assignedvalues;
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        payrollErrors.Add(new PayrollError() { Name = u.Name, ErrorMessage = "There is a some problem in formula.Please check it." });
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                    }
                    else
                    {
                        string result = string.Empty;
                        //if (u.type == 4)
                        //{
                        //    ifElseStmt obj = new ifElseStmt();
                        //    List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                        //    ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                        //    var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                        //    if (!object.ReferenceEquals(tm, null))
                        //        output = tm.thenVal;
                        //}
                        Eval eval = new Eval();
                        result = eval.Execute(output).ToString();
                        u.Output = result;
                        u.Assignedvalues = result.ToString();
                        if (!object.ReferenceEquals(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault(), null))
                        {
                            entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = result.ToString();
                        }
                        else
                        {
                            string str = u.Id;
                        }

                    }

                });
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                throw;
            }
            errors = payrollErrors;
            return entity;
        }
        public decimal GetLOPCreaditCal(int month, int year, int PD, Guid curEmployee, int companyId, Guid AttributeModelId)
        {
            PayrollHistoryList PayrollListValues = new PayrollHistoryList(companyId, year, month);
            PayrollHistory PayrolHisval = new PayrollHistory(companyId, curEmployee, year, month);
            var empL = PayrollListValues.Where(p => p.EmployeeId == curEmployee).FirstOrDefault();
            PayrollHistoryValueList PayrollListValMatch = new PayrollHistoryValueList(PayrolHisval.Id);
            var PayrollValue = PayrollListValMatch.Where(k => k.AttributeModelId == AttributeModelId).FirstOrDefault();
            var PValue = PayrollValue != null ? PayrollValue.Value : "0";
            decimal Amount = Convert.ToDecimal(PValue);
            decimal Amnt = 0;
            AttributeModelList AttrModelList = new AttributeModelList(companyId);
            AttributeModel AttrMod = new AttributeModel();
            Guid Days = System.Guid.Empty;
            if (PayrollValue.ValueType == 3)
            {
                Days = AttrModelList.Where(s => s.Name == "PD").Select(k => k.Id).FirstOrDefault();
            }
            else
            {
                Days = AttrModelList.Where(s => s.Name == "MD").Select(p => p.Id).FirstOrDefault();
            }
            // var Days = AttrModelList.Where(s => s.Name == "MD").FirstOrDefault();
            var Input = PayrollListValMatch.Where(k => k.AttributeModelId == Days).FirstOrDefault();
            string IValue = Input != null ? Input.Value : "0";
            decimal IVal = Convert.ToDecimal(IValue);
            var PDays = AttrModelList.Where(s => s.Name == "PD").FirstOrDefault();
            if (Amount != 0 && Convert.ToDecimal(IValue) != 0)
            {
                Amnt = (Amount / IVal) * (PD);
            }
            return Amnt;
        }

        public decimal GetSUPPCreaditCal(int month, int year, decimal PD, Guid curEmployee, int companyId, Guid AttributeModelId)
        {
            decimal Amnt = 0;

            List<PayrollError> payErrors = new List<PayrollError>();
            List<PayrollBO.ArrearHistory> dummyobj = new List<PayrollBO.ArrearHistory>();
            int userId = 0;
            Employee emp = new Employee(companyId, curEmployee);
            if (_curentEntity == null || (month != currMonth && year != currYear))
            {
                var SuppEntity = ExecuteProcess(companyId, emp, year, month, userId, out dummyobj, out payErrors, false);
                var CalSupp = SuppEntity.EntityAttributeModelList.Where(x => x.AttributeModelId == AttributeModelId).FirstOrDefault().EntityAttributeValue.Value;
                Amnt = Convert.ToDecimal(CalSupp);
                _curentEntity = SuppEntity;
                currMonth = month;
                currYear = year;
            }
            //else if (month != currMonth && year != currYear)
            //{
            //    var SuppEntity = ExecuteProcess(companyId, emp, year, month, userId, out dummyobj, out payErrors);
            //    var CalSupp = SuppEntity.EntityAttributeModelList.Where(x => x.AttributeModelId == AttributeModelId).FirstOrDefault().EntityAttributeValue.Value;
            //    Amnt = Convert.ToDecimal(CalSupp);
            //    _curentEntity = SuppEntity;
            //    currMonth = month;
            //    currYear = year;
            //}
            else
            {
                var SuppEntity = _curentEntity;
                var CalSupp = SuppEntity.EntityAttributeModelList.Where(x => x.AttributeModelId == AttributeModelId).FirstOrDefault().EntityAttributeValue.Value;
                Amnt = Convert.ToDecimal(CalSupp);

            }
            return Amnt;
        }
        public Entity ExecuteProcess(int companyId, Employee curEmployee, int year, int month, int createdBy, out List<ArrearHistory> arearValues, out List<PayrollError> errors, bool FF_Flag, string type = "")
        {

            if (FF_Flag)
            {
                FullFinalSettlement ff = new FullFinalSettlement(Guid.Empty, curEmployee.Id);
                month = ff.LastWorkingDate.Month;
                year = ff.LastWorkingDate.Year;
            }
            ArrearHistoryList pfArr = new ArrearHistoryList();
            errors = new List<PayrollError>();
            Guid entityId;
            Guid entityModelId;
            DateTime startdate = DateTime.Parse("1/" + month + "/" + year, new CultureInfo("en-GB"));
            List<ArrearHistory> arrearHistory = new List<ArrearHistory>();
            EntityModel entModel = new EntityModel(ComValue.SalaryTable, companyId);
            PayrollHistoryList payHistory = new PayrollHistoryList(companyId, curEmployee.Id);
            string tempMsg = "Already Processed";
            //if (type != "history")
            //{
            //    foreach (var pp in payHistory)
            //    {
            //        DateTime dt = new DateTime(pp.Year, pp.Month, 01);
            //        DateTime curPP = new DateTime(year, month, 01);
            //        if (dt > curPP && pp.Status == ComValue.payrollProcessStatus[0] || pp.Status == ComValue.payrollProcessStatus[1])
            //        {
            //            year = pp.Year; month = pp.Month;
            //            tempMsg = "Future Month processed";
            //            break;
            //        }
            //    }
            //}

            PayrollHistory payrollHistory = new PayrollHistory(companyId, curEmployee.Id, year, month);
            if (!object.ReferenceEquals(payrollHistory, null) && (payrollHistory.Status == ComValue.payrollProcessStatus[0] || payrollHistory.Status == ComValue.payrollProcessStatus[1]))
            {

                entityId = payrollHistory.EntityId;
                entityModelId = payrollHistory.EntityModelId;
                errors.Add(new PayrollError() { ErrorMessage = tempMsg });
                PayrollHistoryValueList payrollHistoryValuelist = new PayrollHistoryValueList(payrollHistory.Id);
                Entity entityDone = new Entity(entityModelId, entityId);
                List<string> RemoveEntity = new List<string>();
                if (!FF_Flag)
                {
                    entityDone.EntityAttributeModelList.RemoveAll(r => r.AttributeModel.FullAndFinalSettlement == true);
                }
                for (int cnt = 0; cnt < entityDone.EntityAttributeModelList.Count; cnt++)
                {
                    var payrollValue = payrollHistoryValuelist.Where(p => p.AttributeModelId == entityDone.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                    if (object.ReferenceEquals(payrollValue, null))
                    {
                        //entityDone.EntityAttributeModelList.RemoveAt(cnt);
                        RemoveEntity.Add(entityDone.EntityAttributeModelList[cnt].AttributeModelId.ToString());

                    }
                    else
                    {
                        entityDone.EntityAttributeModelList[cnt].EntityAttributeValue.Value = string.Format("{0:0.00}", payrollValue.Value);
                    }
                }

                //Remove Null Values
                PF pf = new PF();
                foreach (string sid in RemoveEntity)
                {
                    pf.RemoveEntity(entityDone, sid);
                }
                arearValues = null;

                return entityDone;//already done the process
            }
            else
            {
                EntityMapping entMapping = new EntityMapping("Employee", curEmployee.Id.ToString(), entModel.Id);
                if (entMapping.EntityId == null)
                {
                    errors.Add(new PayrollError() { ErrorMessage = "Not Mapped with Salary" });
                    arearValues = null;
                    return new Entity();
                }
                else
                {
                    entityId = new Guid(entMapping.EntityId);
                    entityModelId = new Guid(entMapping.EntityTableName);
                }
            }
            List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
            List<PayrollError> payrollErrors = new List<PayrollError>();
            Entity entity = new Entity(entityModelId, entityId);
            if (!FF_Flag)
            {
                EntityAttributeModelList nw = new EntityAttributeModelList();
                nw = entity.EntityAttributeModelList;
                // entity.EntityAttributeModelList = null;
                //  entity.EntityAttributeModelList.AddRange(nw.Where(t => t.AttributeModel.FullAndFinalSettlement != true).ToList());
            }
            EntityBehaviorList entityBehaviorList = new EntityBehaviorList(entity.Id, entity.EntityModelId);
            var entitymasterValueslist = new EntityMasterValueList(curEmployee.Id, ComValue.EmployeeTable).Where(s => s.EntityModelId == entity.EntityModelId).ToList();
            EntityMasterSettings masset = new EntityMasterSettings();
            var masterSettingslist = masset.entityMastersettingList(entity.Id);
            if (object.ReferenceEquals(entitymasterValueslist, null))
                entitymasterValueslist = new List<EntityMasterValue>();
            MonthlyInputList monthlyinputlist = new MonthlyInputList(entity.Id, curEmployee.Id, month, year);
            TaxComputationInfo taxInfo = new TaxComputationInfo();
            taxInfo.AttributemodelList = new AttributeModelList(companyId);
            TXFinanceYearList tXFinanceYears = new TXFinanceYearList(companyId);
            DateTime curdate = new DateTime(year, month, 1);
            taxInfo.FinanceYear = tXFinanceYears.Where(tf => curdate >= tf.StartingDate && curdate <= tf.EndingDate).FirstOrDefault();
            IncrementList incrementlist = new IncrementList(curEmployee.Id);
            CreditDaysList lopcreditDaysList = new CreditDaysList(companyId, month, year, curEmployee.Id, "LOP");
            CreditDaysList suplcreditDaysList = new CreditDaysList(companyId, month, year, curEmployee.Id, "Supp");

            suplcreditDaysList.RemoveAll(s => s.IsProcessed == true);

            var attmodelid = taxInfo.AttributemodelList.Where(d => d.Name == "LWF").FirstOrDefault();
            var monthlyamount = "";
            var monthlyamountlist = monthlyinputlist.Where(e => e.AttributeModelId == attmodelid.Id).FirstOrDefault();
            if (monthlyamountlist == null)
            {
                monthlyamount = "0";
            }
            else
            {
                monthlyamount = monthlyamountlist.Value;
            }

            // var monthlyamountlist = monthlyinputlist.Where(e => e.AttributeModelId == attmodelid.Id).FirstOrDefault();
            //  var monthlyamount = monthlyamountlist.Value;


            Employee employee = new Employee(companyId, curEmployee.Id);
            Category category = new Category(employee.CategoryId, companyId);
            LWFSetting lwf = new LWFSetting();
            if (employee.Location != Guid.Empty)
            {
                lwf = new LWFSetting(employee.Location, companyId);
            }

            PremiumSettingComponentList lopComponents = new PremiumSettingComponentList(companyId, "LOP Credit Setting", employee.CategoryId);
            PremiumSettingComponentList suppComponents = new PremiumSettingComponentList(companyId, "Supplementary Setting", employee.CategoryId);
            if (!FF_Flag)
            {
                entity.EntityAttributeModelList.RemoveAll(r => r.AttributeModel.FullAndFinalSettlement == true);
            }

            IncrementList incdata = new IncrementList();
            Increment incOldVla = new Increment();
            IncrementDetailList incDet = new IncrementDetailList();
            if (payHistory.Count > 0 && payHistory.Where(p => p.Month > month && (p.Status == ComValue.payrollProcessStatus[0] || payrollHistory.Status == ComValue.payrollProcessStatus[1])) != null)
            {
                incdata = new IncrementList(curEmployee.Id);
                incOldVla = incdata.Where(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear) > Convert.ToDateTime("1/" + month + "/" + year)).OrderBy(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear)).FirstOrDefault();
                if (incOldVla != null)
                {
                    incDet = new IncrementDetailList(incOldVla.Id);
                    incDet.ForEach(x =>
                    {
                        if (entitymasterValueslist.Where(mas => mas.AttributeModelId == x.AttributeModelId && Convert.ToDecimal(mas.Value) != x.NewValue).FirstOrDefault() != null)
                        {
                            x.OldValue = Convert.ToDecimal(entitymasterValueslist.Where(mas => mas.AttributeModelId == x.AttributeModelId).FirstOrDefault().Value);
                        }
                    });
                }
                //else
                //{
                //    incOldVla = incdata.Where(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear) >= Convert.ToDateTime("1/" + month + "/" + year)).OrderBy(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear)).FirstOrDefault();
                //    if (incOldVla != null)
                //    {
                //        incDet = new IncrementDetailList(incOldVla.Id);
                //    }
                //}
            }
            else
            {
                incdata = new IncrementList(curEmployee.Id);
                incOldVla = incdata.Where(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear) > Convert.ToDateTime("1/" + month + "/" + year)).OrderBy(i => Convert.ToDateTime("1/" + i.ApplyMonth + "/" + i.ApplyYear)).FirstOrDefault();
                if (incOldVla != null)
                {
                    incDet = new IncrementDetailList(incOldVla.Id);
                }
            }


            entity.EntityAttributeModelList.ForEach(u =>
            {
                if (u.AttributeModel.Name == "AG")
                {
                    Console.WriteLine("MESSAGE");
                }
                if (u.AttributeModel.Name == "EB")
                {

                }
                if (u.AttributeModel.Name == "PD")
                {

                }
                var entityBehavior = entityBehaviorList.Where(s => s.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                if (u.AttributeModel.IsSetting == true)//Setting
                {

                    if (u.AttributeModel.Name == "MD")//Setting
                    {
                        #region execute setting                         
                        string mddays = GetMonthDay(u, category, monthlyinputlist, month, year, ref payrollErrors, true, out startdate).ToString();

                        //DateTime.DaysInMonth(year, month).ToString()                     
                        AddValuesTemp(u, ref lstFormulaRecursive, mddays, null, null, 2, 1, 1);
                        #endregion
                    }
                    else if (u.AttributeModel.IsMonthlyInput && u.AttributeModel.Name == "LD")
                    {
                        #region execute monthly input
                        var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(monInput, null))
                        {
                            string monValue = monInput != null ? monInput.Value : "0";

                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, 1, 1, true, monValue);
                        }
                        else
                        {
                            payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input values is not available" });
                        }
                        #endregion
                    }

                    else if (entityBehavior != null && entityBehavior.ValueType == 3 && u.AttributeModel.Name == "LD" && FF_Flag)
                    {
                        if (FF_Flag)
                        {
                            //FullFinalSettlement ff = new FullFinalSettlement(Guid.Empty, curEmployee.Id);
                            //var pdID = attributemodelList.Where(x => x.Name == "PD").FirstOrDefault().Id;
                            //var monInput = monthlyinputlist.Where(p => p.AttributeModelId == pdID).FirstOrDefault();

                            //if (!object.ReferenceEquals(monInput, null))
                            //{
                            //    string monValue = monInput != null ? monInput.Value : "0";
                            //    monValue = Convert.ToString(Convert.ToInt32(ff.LastWorkingDate.Day) - Convert.ToInt32(monInput.Value));

                            //    AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, 1, 3, true, monValue);
                            //}
                            AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, null, null, 1, 1, 3, true);

                        }
                    }
                    else if (u.AttributeModel.Name == "LWF") //Modified By Keerthika on 18/04/2017
                    {
                        if (lwf != null)
                        {
                            if (lwf.ApplyMonth == month)
                            {
                                var lwff = lwf.EmployeeAmount;
                                //      Eval eval = new Eval();
                                //   var result= eval.Execute(lwff);
                                //entityBehavior.Percentage = null;
                                //entityBehavior.EligibiltyFormula = null;
                                // entityBehavior.RoundingId = 3;
                                //  AddValuesTemp(u, ref lstFormulaRecursive, lwf.EmployeeAmount.ToString(), entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                //   if(entityBehavior.Percentage==null&& entityBehavior.EligibiltyFormula==null&& entityBehavior.RoundingId==null)
                                if (Convert.ToInt32(monthlyamount) != 0)
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, monthlyamount.ToString(), null, null, 4, 1, 1, true, monthlyamount.ToString());
                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, lwff.ToString(), null, null, 4, 1, 1, true, lwff.ToString());
                                }

                            }
                            else
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, monthlyamount.ToString(), null, null, 4, 1, 1, true, monthlyamount.ToString());
                            }
                        }
                        else
                        {
                            AddValuesTemp(u, ref lstFormulaRecursive, monthlyamount.ToString(), null, null, 4, 1, 1, true, monthlyamount.ToString());
                        }
                    }
                    else
                    {
                        #region execute setting 
                        if (!object.ReferenceEquals(entityBehavior, null))
                        {
                            EntityMasterSettings mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();

                            string tempPercentage = entityBehavior.Percentage;
                            if (mastersettingVal != null)
                            {
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                            }


                            if (entityBehavior.ValueType == 1)//Employee Master  input
                            {
                                #region execute employee master input
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                if (incOldVla != null)
                                {
                                    // IncrementDetailList incDet = new IncrementDetailList(incOldVla.Id);
                                    var detail = incDet.Where(d => d.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (detail != null)
                                    {
                                        entitymastervalue.Value = Convert.ToString(detail.OldValue);
                                    }
                                }
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1, true, assValues);


                                #endregion

                            }
                            else if (entityBehavior.ValueType == 2)//Monthly input
                            {
                                #region execute monthly input
                                var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = monInput != null ? monInput.Value : "0";

                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2, true, assValues);
                                #endregion
                            }

                            else if (entityBehavior.ValueType == 3)//Formula
                            {
                                #region execute formula/percentage

                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                                #endregion
                            }
                            else if (entityBehavior.ValueType == 4)//Conditionl formula  --if else
                            {
                                #region execute Conditionl formula  --if else

                                string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                                #endregion
                            }
                            else if (entityBehavior.ValueType == 5)//Range type
                            {

                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                            }
                        }
                        else if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault(), null))
                        {
                            lstFormulaRecursive.Add(new FormulaRecursive()
                            {
                                Assignedvalues = "0",
                                Id = u.AttributeModelId.ToString(),
                                Name = u.AttributeModel.Name,
                                ExecuteOrder = 2,
                                ParentId = Convert.ToString(u.AttributeModel.ParentId)
                            });

                        }
                        else
                        {
                            lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = "0";
                        }
                        #endregion
                    }

                }
                else
                {
                    //Getting Leave Type List inorder to assign leave used count to respective mapped component from leave module.
                    LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
                    LeaveTypeSettingList LevTypSetLst = new LeaveTypeSettingList(companyId, DefaultFinancialYr.Id);
                    var LevMapCompChk = LevTypSetLst.Where(p => p.UsedLeaveId == u.AttributeModel.Id).FirstOrDefault();

                    if (u.AttributeModel.IsMonthlyInput)
                    {
                        #region execute monthly input

                        var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(monInput, null))
                        {
                            if (u.AttributeModel.Name == "ARB")
                            {

                            }
                            string monValue = monInput != null ? monInput.Value : "0";
                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, entityBehavior.RoundingId, 1, true, monValue);
                        }
                        else
                        {
                            payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input values is not available" });
                        }
                        #endregion
                    }
                    else if (u.AttributeModel.IsInstallment)//Loan
                    {
                        #region execute loan
                        decimal loanAmount = 0;

                        if (FF_Flag)
                        {
                            LoanMaster loanmaster = new LoanMasterList(companyId).Where(w => w.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                            if (!object.ReferenceEquals(loanmaster, null))
                            {
                                List<LoanEntry> loanEntrylist = new LoanEntryList(curEmployee.Id).Where(w => w.LoanMasterId == loanmaster.Id).ToList();
                                if (loanEntrylist.Count > 0)
                                {
                                    loanEntrylist.ForEach(f =>
                                    {
                                        loanAmount = loanAmount + f.LoanTransactionList.Where(w => w.Status == "UnPaid").ToList().Sum(s => s.AmtPaid);
                                        f.LoanTransactionList.Where(w => w.Status == "UnPaid").ToList().ForEach(l =>
                                        {
                                            LoanTransaction loanTransaction = new LoanTransaction(l.Id, l.LoanEntryId);
                                            loanTransaction.Status = "Paid";
                                            loanTransaction.isPayRollProcess = true;
                                            loanTransaction.isFandFProcessv = true;
                                            //loanTransaction.Delete();
                                            loanTransaction.Save();
                                        });
                                    });
                                }
                            }

                        }
                        else
                        {
                            loanAmount = GetLoanAmount(companyId, curEmployee.Id, u.AttributeModelId, year, month);
                        }

                        AddValuesTemp(u, ref lstFormulaRecursive, loanAmount.ToString(), null, null, 2, 1, 1, true, loanAmount.ToString());
                        #endregion
                    }

                    else if (u.AttributeModel.IsIncrement)//increment and arrear process
                    {
                        #region execute increment setting 
                        if (entityBehavior != null && entityBehavior.ValueType == 1)//Employee Master  input
                        {
                            #region execute employee master input
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(entitymastervalue, null))
                            {
                                if (incOldVla != null)
                                {
                                    // IncrementDetailList incDet = new IncrementDetailList(incOldVla.Id);
                                    var detail = incDet.Where(d => d.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (detail != null)
                                    {
                                        entitymastervalue.Value = Convert.ToString(detail.OldValue);
                                    }
                                }
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1, true, assValues);
                            }
                            else
                            {
                                //modified by AjithPanner on 13/11/17
                                if (entityBehavior.AttributeModelId != entityBehavior.ArrearAttributeModelId)
                                {
                                    payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Master Value is not available" });
                                }
                                else if (entityBehavior.AttributeModelId == null)
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }
                            #endregion
                        }
                        if (entityBehavior != null && entityBehavior.ValueType == 3)
                        {
                            #region Percentage calculation
                            var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                            string tempPercentage = entityBehavior.Percentage;
                            if (mastersettingVal != null)
                            {
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                            }
                            AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                            #endregion
                        }
                        if (entityBehavior != null && entityBehavior.ValueType == 4)
                        {
                            #region execute Conditionl formula  --if else
                            var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                            string tempPercentage = entityBehavior.Percentage;
                            if (mastersettingVal != null)
                            {
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                            }
                            string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                            AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                            #endregion
                        }

                        if (entityBehavior.ValueType == 5)//Range type
                        {
                            var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                            string tempPercentage = entityBehavior.Percentage;
                            if (mastersettingVal != null)
                            {
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                            }
                            AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                        }

                        CalculateIncrement(companyId, curEmployee.Id, u, ref lstFormulaRecursive, ref arrearHistory, entityBehavior, entity, taxInfo.AttributemodelList, month, year, ref pfArr);

                        /*
                        if (incrementlist.Count > 0 && incrementlist[0].IsProcessed == false && incrementlist[0].EffectiveDate <= DateTime.Now)
                        {
                            Increment inc = incrementlist[0];
                            int appYear = inc.ApplyYear;
                            double arrearVal = 0.0;
                            int aplMonth = inc.ApplyMonth;
                            if (appYear <= year)
                            {
                                Guid arrMatchId = entityBehavior.ArrearAttributeModelId;
                                do
                                {
                                    if (aplMonth < month)
                                    {
                                        if (!object.ReferenceEquals(entityBehavior, null))
                                        {
                                            do//month do
                                            {
                                                var incDet = inc.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                                string strOut = "0.0";
                                                if (!object.ReferenceEquals(incDet, null))
                                                {
                                                    double dif = (double)(incDet.NewValue - incDet.OldValue);
                                                    strOut = dif.ToString();
                                                }
                                                // string arrerFormula = entityBehavior.ArrearFormula;
                                                PayrollHistory payArr = new PayrollHistory(employeeId, appYear, aplMonth);
                                                var atrTmp = attributemodelList.Where(am => am.Name == "MD").FirstOrDefault();
                                                var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                                if (!object.ReferenceEquals(colTemp, null))
                                                    strOut = strOut + "/" + colTemp.Value;
                                                atrTmp = attributemodelList.Where(am => am.Name == "PD").FirstOrDefault();
                                                colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                                if (!object.ReferenceEquals(colTemp, null))
                                                    strOut = strOut + "*" + colTemp.Value;
                                                Eval eval = new Eval();
                                                double result = eval.Execute(strOut);
                                                arrearVal = arrearVal + result;
                                                aplMonth++;

                                            } while (aplMonth < month);//month while
                                        }
                                    }
                                    appYear = appYear + 1;
                                    aplMonth = 1;
                                } while (appYear <= year);
                                if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault(), null))
                                {
                                    lstFormulaRecursive.Add(new FormulaRecursive()
                                    {
                                        //entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name == "AG" ? "7253" : "0",
                                        Assignedvalues = arrearVal.ToString(),
                                        Id = arrMatchId.ToString(),
                                        Name = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name,
                                        ExecuteOrder = 4,
                                        Rounding = entityBehavior.RoundingId
                                    });
                                }
                                else
                                {
                                    //entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name == "AG" ? "7253" : "0";//
                                    lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = arrearVal.ToString();
                                }

                                if (!object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                                {
                                    var t1 = inc.IncrementDetailList.Where(q => q.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (t1 != null)
                                    {
                                        lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = t1.NewValue.ToString();
                                    }
                                }
                            }

                        }
                        */
                        #endregion
                    }
                    else if (!object.ReferenceEquals(entityBehavior, null))
                    {

                        var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                        string tempPercentage = entityBehavior.Percentage;
                        if (mastersettingVal != null)
                        {
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                        }
                        if (entityBehavior.ValueType == 1)//Employee Master  input
                        {
                            #region execute employee master input
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(entitymastervalue, null) || entityBehavior.Formula == "0")
                            {

                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                //  AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                //Credit Days
                                if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                                {
                                    if (lopcreditDaysList.Count > 0)
                                    {
                                        IncrementList prevMaster = new IncrementList(curEmployee.Id);
                                        Increment preInc = prevMaster.Where(m => new DateTime(m.ApplyYear, m.ApplyMonth, 1) <= new DateTime(lopcreditDaysList[0].Month)).OrderByDescending(m => m.ApplyYear).OrderByDescending(m => m.ApplyMonth).FirstOrDefault();
                                        if (!ReferenceEquals(preInc, null))
                                        {
                                            IncrementDetailList inc = new IncrementDetailList(preInc.Id);
                                            if (!ReferenceEquals(inc, null))
                                            {
                                                if (inc.Where(i => i.AttributeModelId == u.AttributeModelId).Any())
                                                {
                                                    assValues = inc.Where(i => i.AttributeModelId == u.AttributeModelId).FirstOrDefault().NewValue.ToString();
                                                }

                                            }
                                        }
                                        var totpaidDays = lopcreditDaysList.Sum(p => p.PaidDays);
                                        AddValuesTemp(u, ref lstFormulaRecursive, (assValues + "/" + totpaidDays.ToString()), entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 3, true, (assValues + "/" + totpaidDays.ToString()));

                                    }
                                    else
                                    {
                                        AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                    }
                                }
                                else
                                {
                                    if (incOldVla != null)
                                    {
                                        // IncrementDetailList incDet = new IncrementDetailList(incOldVla.Id);
                                        var detail = incDet.Where(d => d.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                        if (detail != null)
                                        {
                                            entitymastervalue.Value = Convert.ToString(detail.OldValue);
                                        }
                                    }
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1, true, assValues);
                                }
                            }
                            else
                            {
                                //modified by AjithPanner on 13/11/2017
                                if (entityBehavior.AttributeModelId != entityBehavior.ArrearAttributeModelId)
                                {
                                    payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Master Value is not available" });
                                }
                                else if (entityBehavior.AttributeModelId == null)
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }

                            #endregion

                        }
                        else if (entityBehavior.ValueType == 2)//Monthly input
                        {
                            #region execute monthly input
                            var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(monInput, null))
                            {
                                string assValues = monInput != null ? monInput.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2, true, assValues);
                            }
                            else
                            {
                                payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input value is not available" });
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 3)//Formula
                        {
                            if (u.AttributeModel.Name == "EB")
                            {
                            }
                            string assValues = entityBehavior != null ? entityBehavior.Formula != null ? entityBehavior.Formula : "0" : "0";
                            #region execute formula/percentage
                            //   AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                            //Credit Days supplementary
                            if (u.AttributeModel.Name == "PD" && employee.DateOfJoining.Month == month && employee.DateOfJoining.Year == year && !FF_Flag)
                            {
                                int md = DateTime.DaysInMonth(year, month);
                                if (!string.IsNullOrEmpty(category.MonthDayProcess))
                                {
                                    DateTime sdate = DateTime.MinValue;
                                    if (category.MonthDayProcess.ToUpper() == "MONTHDAY")
                                    {
                                        sdate = new DateTime(year, month, 1);
                                    }
                                    else if (category.MonthDayProcess.ToUpper() == "STATICDAY")
                                    {
                                        sdate = new DateTime(year, month, 1);
                                    }
                                    else if (category.MonthDayProcess.ToUpper() == "STARTDAY")
                                    {
                                        DateTime newdate = new DateTime(year, month, category.MonthDayOrStartDay);


                                        DateTime edate = newdate.AddDays(-1);
                                        if (newdate > employee.DateOfJoining && edate > employee.DateOfJoining)
                                        {
                                            sdate = newdate.AddMonths(-1);
                                        }

                                    }
                                    TimeSpan pdts = (employee.DateOfJoining.AddDays(-1)) - sdate;
                                    md = pdts.Days + 1;

                                }

                                entityBehavior.Formula = entityBehavior.Formula + "-" + md.ToString();
                            }
                            //Calculating Leave used by the employee and mapping value to the respective component.
                            if (!object.ReferenceEquals(LevMapCompChk, null))
                            {
                                double LevUsedReqCnt = 0;
                                double LevUsedDebitCnt = 0;
                                double TotLevUsed = 0;
                                var FromDate = new DateTime(year, month, 1);
                                var EndDate = FromDate.AddMonths(1).AddDays(-1);
                                string Reqtyp = "leaveReq";
                                LeaveRequest objLevUsed = new LeaveRequest();
                                DataTable dtLevUsedReq = new DataTable();
                                DebitLeaveList objDebitLst = new DebitLeaveList(companyId, DefaultFinancialYr.Id, curEmployee.Id);// Getting Debit List of the employee.
                                LeaveRequestList objReqLst = new LeaveRequestList(curEmployee.Id, companyId, Reqtyp);//Getting Leave Request list of the employee.
                                //Filtering leave request list having approved ,pending list and applied date lies on current processing month.
                                var LeaveReqLst = objReqLst.Where(p => (p.Status == 0 || p.Status == 1) && (p.CreatedOn >= FromDate && p.CreatedOn <= EndDate)).ToList();
                                LeaveReqLst.ForEach(t =>
                                {
                                    if (t.LeaveType.ToString() == LevMapCompChk.LeaveTypeId.ToString())
                                    {
                                        LevUsedReqCnt = LevUsedReqCnt + Convert.ToDouble(t.NoOfDays);
                                    }
                                });
                                objDebitLst.ForEach(p =>
                                {
                                    if (p.DebitLevType.ToString() == LevMapCompChk.LeaveTypeId.ToString())
                                    {
                                        LevUsedDebitCnt = LevUsedDebitCnt + Convert.ToDouble(p.NoOfDays);
                                    }

                                });
                                TotLevUsed = LevUsedReqCnt + LevUsedDebitCnt;
                                AddValuesTemp(u, ref lstFormulaRecursive, TotLevUsed.ToString(), null, null, 2, 1, 1, true, TotLevUsed.ToString());
                            }
                            if (suppComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (suplcreditDaysList.Count > 0)
                                {


                                    decimal totpaidDays = suplcreditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = suplcreditDaysList.Sum(p => p.LopDays);
                                    decimal TOTSuppDays = totpaidDays - totlopDays;
                                    var Supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryDays).FirstOrDefault();
                                    FormulaRecursive FRecursive = new FormulaRecursive();
                                    if (Supid != null)
                                    {
                                        FRecursive.Id = Convert.ToString(Supid.AttributeModelId);
                                        FRecursive.Assignedvalues = Convert.ToString(totpaidDays);
                                        FRecursive.type = 2;
                                        FRecursive.Name = Supid.AttributeModel.Name;
                                        FRecursive.Order = 1;

                                        var ListRecursive = lstFormulaRecursive.Where(d => d.Id == FRecursive.Id).ToList();
                                        if (ListRecursive.Count == 0)
                                        {
                                            lstFormulaRecursive.Add(FRecursive);
                                        }
                                    }
                                    decimal TotalCal = 0;
                                    suplcreditDaysList.ForEach(f =>
                                    TotalCal = TotalCal + (GetSUPPCreaditCal(f.Month, f.Year, TOTSuppDays, curEmployee.Id, companyId, u.AttributeModel.Id))
                                    );

                                    entityBehavior.Formula = "(" + entityBehavior.Formula + ")+" + TotalCal;
                                    //lstFormulaRecursive.Remove(lstFormulaRecursive.Where(x => x.Id == null).FirstOrDefault());


                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3, true, assValues);
                                }
                            }
                            //Credit Days lop credit
                            else if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (lopcreditDaysList.Count > 0)
                                {

                                    decimal totpaidDays = lopcreditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = lopcreditDaysList.Sum(p => p.LopDays);
                                    var supp = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name.Trim() == ComValue.LOPCreditDays).FirstOrDefault();
                                    FormulaRecursive FRecursive = new FormulaRecursive();
                                    if (supp != null)
                                    {
                                        FRecursive.Id = Convert.ToString(supp.AttributeModelId);
                                        FRecursive.Assignedvalues = Convert.ToString(totpaidDays);
                                        FRecursive.type = 2;
                                        FRecursive.Name = supp.AttributeModel.Name;
                                        FRecursive.Order = 1;
                                        var ListRecursive = lstFormulaRecursive.Where(d => d.Id == FRecursive.Id).ToList();
                                        if (ListRecursive.Count == 0)
                                        {
                                            lstFormulaRecursive.Add(FRecursive);
                                        }
                                    }
                                    //lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();
                                    decimal TotalCal = 0;

                                    lopcreditDaysList.ForEach(s =>

                                     TotalCal = TotalCal + (GetLOPCreaditCal(s.Month, s.Year, s.PaidDays, curEmployee.Id, companyId, u.AttributeModel.Id))

                                    );
                                    entityBehavior.Formula = "(" + entityBehavior.Formula + ")+" + TotalCal;

                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                                }
                            }
                            else
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 4)//Conditionl formula  --if else
                        {
                            #region execute Conditionl formula  --if else
                            string assValues = entityBehavior != null ? entityBehavior.Formula : "0";

                            // AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                            //Credit Days supplementary
                            if (suppComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (suplcreditDaysList.Count > 0)
                                {

                                    decimal totpaidDays = suplcreditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = suplcreditDaysList.Sum(p => p.LopDays);
                                    decimal TOTSuppDays = totpaidDays - totlopDays;
                                    var Supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryDays).FirstOrDefault();
                                    FormulaRecursive FRecursive = new FormulaRecursive();
                                    FRecursive.Id = Convert.ToString(Supid.AttributeModelId);
                                    FRecursive.Assignedvalues = Convert.ToString(totpaidDays);
                                    FRecursive.type = 2;
                                    FRecursive.Name = Supid.AttributeModel.Name;
                                    FRecursive.Order = 1;
                                    var ListRecursive = lstFormulaRecursive.Where(d => d.Id == FRecursive.Id).ToList();
                                    if (ListRecursive.Count == 0)
                                    {
                                        lstFormulaRecursive.Add(FRecursive);
                                    }
                                    decimal TotalCal = 0;
                                    suplcreditDaysList.ForEach(f =>
                                    TotalCal = TotalCal + (GetSUPPCreaditCal(f.Month, f.Year, TOTSuppDays, curEmployee.Id, companyId, u.AttributeModel.Id))
                                    );

                                    assValues = "(" + entityBehavior.Formula + ")+" + TotalCal;

                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 1);
                                }
                            }
                            //Credit Days lop credit
                            else if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (lopcreditDaysList.Count > 0)
                                {


                                    decimal totpaidDays = lopcreditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = lopcreditDaysList.Sum(p => p.LopDays);
                                    var supp = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name.Trim() == ComValue.LOPCreditDays).FirstOrDefault();
                                    FormulaRecursive FRecursive = new FormulaRecursive();
                                    FRecursive.Id = Convert.ToString(supp.AttributeModelId);
                                    FRecursive.Assignedvalues = Convert.ToString(totpaidDays);
                                    FRecursive.type = 2;
                                    FRecursive.Name = supp.AttributeModel.Name;
                                    FRecursive.Order = 1;
                                    var ListRecursive = lstFormulaRecursive.Where(d => d.Id == FRecursive.Id).ToList();
                                    if (ListRecursive.Count == 0)
                                    {
                                        lstFormulaRecursive.Add(FRecursive);
                                    }
                                    //lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();
                                    decimal TotalCal = 0;

                                    lopcreditDaysList.ForEach(s =>

                                     TotalCal = TotalCal + (GetLOPCreaditCal(s.Month, s.Year, s.PaidDays, curEmployee.Id, companyId, u.AttributeModel.Id))

                                    );
                                    assValues = "(" + entityBehavior.Formula + ")+" + TotalCal;

                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4, true, assValues);
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 5)//Range type
                        {

                            AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                        }

                    }
                    else
                    {
                        AddValuesTemp(u, ref lstFormulaRecursive, u.EntityAttributeValue.Value, null, null, 0, 1, 1);

                    }
                }
            });
            if (FF_Flag) // Calculate Paid days for FF
            {
                FormulaRecursive LD = lstFormulaRecursive.Where(l => l.Name == "LD").FirstOrDefault();
                FormulaRecursive PD = lstFormulaRecursive.Where(l => l.Name == "PD").FirstOrDefault();
                FormulaRecursive Md = lstFormulaRecursive.Where(l => l.Name == "MD").FirstOrDefault();
                FullFinalSettlement ff = new FullFinalSettlement(Guid.Empty, employee.Id);

                // Time span which is month Days
                TimeSpan ts;
                if (ff.LastWorkingDate.Month == employee.DateOfJoining.Month && ff.LastWorkingDate.Year == employee.DateOfJoining.Year)
                {
                    ts = (ff.LastWorkingDate - employee.DateOfJoining);
                }
                else
                {
                    ts = (ff.LastWorkingDate - startdate);
                }


                int days = ts.Days + 1;

                if (!object.ReferenceEquals(LD, null) && !object.ReferenceEquals(PD, null))
                {
                    string md = "{" + Md.Id + "}";
                    if (LD.Assignformulavalue.Trim().ToLower().Contains("md"))
                    {

                        LD.Assignedvalues = LD.Assignedvalues.Replace(md, days.ToString());

                    }
                    else if (PD.Assignformulavalue.Trim().ToLower().Contains("md"))
                    {
                        PD.Assignedvalues = PD.Assignedvalues.Replace(md, days.ToString());
                    }

                }

                //lstFormulaRecursive.Where(l => l.Name == "MD").FirstOrDefault().Assignedvalues = "(" + (ts.Days) + "+1" +  ")";


                ////if pd is monthly input
                //if (entityBehaviorList.Where(w => w.AttributeModelId == new Guid(PD.Id)).FirstOrDefault().ValueType == 2)
                //{
                //    lstFormulaRecursive.Where(l => l.Name == "LD").FirstOrDefault().Assignedvalues = "(" + (ts.Days) + "+1-" + PD.Assignedvalues + ")";
                //}




            }

            AttributeModelList att = new AttributeModelList(companyId);
            List<AttributeModel> attr = att.Where(r => r.Name.ToLower() == "employeeserviceyear" || r.Name.ToLower() == "employeeage" || r.Name.ToLower() == "ismetro").ToList();

            curEmployee.month = month;
            curEmployee.year = year;

            attr.ForEach(f =>
            {
                lstFormulaRecursive.Add(new FormulaRecursive()
                {
                    Assignedvalues = f.Name.ToLower() != "employeeserviceyear" ? curEmployee.NoOfServiceYear.ToString() : f.Name.ToLower() != "employeeage" ? curEmployee.Age.ToString() : curEmployee.isMetro.ToString(),
                    Id = f.Id.ToString(),
                    Name = f.Name == null ? string.Empty : f.Name,
                    ExecuteOrder = 1,
                    Rounding = 0,
                    type = 1,
                    Percentage = "100",
                    EligibleFormula = string.Empty,
                    DoRoundOff = true,
                    Assignformulavalue = string.Empty
                });
            });





            lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();
            if (payrollErrors.Count > 0)//dont process the formula execution
            {
                errors = payrollErrors;
                arearValues = arrearHistory;
                return new Entity();
            }
            lstFormulaRecursive.ForEach(u =>
            {
                u.UserId = createdBy;
                u.Year = year;
                u.CompanyId = companyId;
                u.Month = month;
                u.EmployeeId = curEmployee.Id;
            });
            PayrollHistoryList Historylist = new PayrollHistoryList(companyId, taxInfo.FinanceYear.StartingDate.Year, taxInfo.FinanceYear.StartingDate.Month, taxInfo.FinanceYear.EndingDate.Year, taxInfo.FinanceYear.EndingDate.Month, curEmployee.Id);
            taxInfo.payrollhistorylist = new PayrollHistoryList();
            if (!object.ReferenceEquals(Historylist,null))
            {
                taxInfo.payrollhistorylist.AddRange(Historylist);
            }
            
            recursive(taxInfo, employee, lstFormulaRecursive, entity, incrementlist, FF_Flag);

            lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
            lstFormulaRecursive.ForEach(u =>
            {
                string error = string.Empty;
                string output = string.Empty;
                string rerun = string.Empty;
              //  if (u.Assignedvalues.IndexOf("{") >= 0)
               // {
                    u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output, rerun);
               // }
               // else
              //  {
                    output = u.Assignedvalues;
               // }
                if (!string.IsNullOrEmpty(error))
                {
                    payrollErrors.Add(new PayrollError() { Name = u.Name, ErrorMessage = "There is a some problem in formula.Please check it." });
                    entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                }
                else
                {
                    string result = string.Empty;
                    if (u.type == 4)
                    {
                        //ifElseStmt obj = new ifElseStmt();
                        //List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                        //ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                        //var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                        //if (!object.ReferenceEquals(tm, null))
                        //    output = tm.thenVal;
                    }
                    Eval eval = new Eval();
                    result = eval.Execute(output).ToString();
                    u.Output = result;
                    u.Assignedvalues = result.ToString();
                    if (!object.ReferenceEquals(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault(), null))
                    {
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = result.ToString();
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.BaseValue = u.BaseValue;
                    }
                    else
                    {
                        string str = u.Id;
                    }

                }

            });
            errors = payrollErrors;
            arearValues = arrearHistory;
            return entity;
        }
        public Entity ExecuteFFProcess(int companyId, Employee curEmployee, int year, int month, int createdBy, out List<PayrollError> errors)
        {
            errors = new List<PayrollError>();
            Guid entityId;
            Guid entityModelId;
            DateTime startdate;
            EntityModel entModel = new EntityModel(ComValue.SalaryTable, companyId);
            PayrollHistory payrollHistory = new PayrollHistory(companyId, curEmployee.Id, year, month);
            if (!object.ReferenceEquals(payrollHistory, null) && (payrollHistory.Status == ComValue.payrollProcessStatus[0] || payrollHistory.Status == ComValue.payrollProcessStatus[1]))
            {
                entityId = payrollHistory.EntityId;
                entityModelId = payrollHistory.EntityModelId;
                errors.Add(new PayrollError() { ErrorMessage = "Already Processed" });
                PayrollHistoryValueList payrollHistoryValuelist = new PayrollHistoryValueList(payrollHistory.Id);
                Entity entityDone = new Entity(entityModelId, entityId);
                List<string> RemoveEntity = new List<string>();
                for (int cnt = 0; cnt < entityDone.EntityAttributeModelList.Count; cnt++)
                {
                    var payrollValue = payrollHistoryValuelist.Where(p => p.AttributeModelId == entityDone.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                    if (object.ReferenceEquals(payrollValue, null))
                    {
                        //entityDone.EntityAttributeModelList.RemoveAt(cnt);
                        RemoveEntity.Add(entityDone.EntityAttributeModelList[cnt].AttributeModelId.ToString());

                    }
                    else
                    {
                        entityDone.EntityAttributeModelList[cnt].EntityAttributeValue.Value = payrollValue.Value;
                    }
                }

                //Remove Null Values
                PF pf = new PF();
                foreach (string sid in RemoveEntity)
                {
                    pf.RemoveEntity(entityDone, sid);
                }
                return entityDone;//already done the process
            }
            else
            {
                EntityMapping entMapping = new EntityMapping("Employee", curEmployee.Id.ToString(), entModel.Id); ;
                if (entMapping.EntityId == null)
                {
                    errors.Add(new PayrollError() { ErrorMessage = "Not Mapped with Salary" });
                    return new Entity();
                }
                else
                {
                    entityId = new Guid(entMapping.EntityId);
                    entityModelId = new Guid(entMapping.EntityTableName);
                }
            }
            List<FormulaRecursive> lstFormulaRecursive = new List<FormulaRecursive>();
            List<PayrollError> payrollErrors = new List<PayrollError>();
            Entity entity = new Entity(entityModelId, entityId);
            EntityBehaviorList entityBehaviorList = new EntityBehaviorList(entity.Id, entity.EntityModelId);
            var entitymasterValueslist = new EntityMasterValueList(curEmployee.Id, ComValue.EmployeeTable).Where(s => s.EntityModelId == entity.EntityModelId).ToList();
            EntityMasterSettings masset = new EntityMasterSettings();
            var masterSettingslist = masset.entityMastersettingList(entity.EntityModelId);

            if (object.ReferenceEquals(entitymasterValueslist, null))
                entitymasterValueslist = new List<EntityMasterValue>();
            MonthlyInputList monthlyinputlist = new MonthlyInputList(entity.Id, curEmployee.Id, month, year);
            TaxComputationInfo taxInfo = new TaxComputationInfo();
            taxInfo.AttributemodelList = new AttributeModelList(companyId);
            TXFinanceYearList tXFinanceYears = new TXFinanceYearList(companyId);
            DateTime curdate = new DateTime(year, month, 1);
            taxInfo.FinanceYear = tXFinanceYears.Where(tf => curdate >= tf.StartingDate && curdate <= tf.EndingDate).FirstOrDefault();
            IncrementList incrementlist = new IncrementList(curEmployee.Id);
            CreditDaysList creditDaysList = new CreditDaysList(companyId, month, year, curEmployee.Id);

            creditDaysList.RemoveAll(s => s.IsProcessed == true);

            Employee employee = new Employee(companyId, curEmployee.Id);
            Category category = new Category(employee.CategoryId, companyId);
            LWFSetting lwf = new LWFSetting(employee.Id, companyId);

            PremiumSettingComponentList lopComponents = new PremiumSettingComponentList(companyId, "LOP Credit Setting", employee.CategoryId);
            PremiumSettingComponentList suppComponents = new PremiumSettingComponentList(companyId, "Supplementary Setting", employee.CategoryId);
            entity.EntityAttributeModelList.ForEach(u =>
            {

                var entityBehavior = entityBehaviorList.Where(s => s.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();

                if (u.AttributeModel.IsSetting == true)//Setting
                {

                    if (u.AttributeModel.Name == "MD")//Setting
                    {
                        #region execute setting                         
                        string mddays = GetMonthDay(u, category, monthlyinputlist, month, year, ref payrollErrors, true, out startdate).ToString();
                        //DateTime.DaysInMonth(year, month).ToString()                     
                        AddValuesTemp(u, ref lstFormulaRecursive, mddays, null, null, 2, 1, 1);
                        #endregion
                    }
                    else if (u.AttributeModel.IsMonthlyInput && u.AttributeModel.Name == "LD")
                    {
                        #region execute monthly input
                        var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(monInput, null))
                        {
                            string monValue = monInput != null ? monInput.Value : "0";

                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, 1, 1);
                        }
                        else
                        {
                            payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input values is not available" });
                        }
                        #endregion
                    }
                    else
                    if (u.AttributeModel.Name == "LWF")
                    {
                        if (lwf.ApplyMonth == month)
                        {

                            AddValuesTemp(u, ref lstFormulaRecursive, lwf.EmployeeAmount.ToString(), entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                        }
                    }
                    else
                    {
                        #region execute setting 
                        if (!object.ReferenceEquals(entityBehavior, null))
                        {
                            if (entityBehavior.ValueType == 1)//Employee Master  input
                            {
                                #region execute employee master input
                                var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                #endregion

                            }
                            else if (entityBehavior.ValueType == 2)//Monthly input
                            {
                                #region execute monthly input
                                var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                string assValues = monInput != null ? monInput.Value : "0";

                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2);
                                #endregion
                            }

                            else if (entityBehavior.ValueType == 3)//Formula
                            {
                                #region execute formula/percentage
                                var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                                string tempPercentage = entityBehavior.Percentage;
                                if (mastersettingVal != null)
                                {
                                    var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                    tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                                }
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                                #endregion
                            }
                            else if (entityBehavior.ValueType == 4)//Conditionl formula  --if else
                            {
                                #region execute Conditionl formula  --if else
                                var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                                string tempPercentage = entityBehavior.Percentage;
                                if (mastersettingVal != null)
                                {
                                    var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                    tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                                }
                                string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                                #endregion
                            }
                            else if (entityBehavior.ValueType == 5)//Range type
                            {
                                var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                                string tempPercentage = entityBehavior.Percentage;
                                if (mastersettingVal != null)
                                {
                                    var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                                    tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                                }
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                            }
                        }
                        else if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault(), null))
                        {
                            lstFormulaRecursive.Add(new FormulaRecursive()
                            {
                                Assignedvalues = "0",
                                Id = u.AttributeModelId.ToString(),
                                Name = u.AttributeModel.Name,
                                ExecuteOrder = 2,
                                ParentId = Convert.ToString(u.AttributeModel.ParentId)
                            });

                        }
                        else
                        {
                            lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = "0";
                        }
                        #endregion
                    }

                }
                else
                {


                    if (u.AttributeModel.IsMonthlyInput)
                    {
                        #region execute monthly input

                        var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                        if (!object.ReferenceEquals(monInput, null))
                        {
                            if (u.AttributeModel.Name == "OTD")
                            {

                            }
                            string monValue = monInput != null ? monInput.Value : "0";
                            AddValuesTemp(u, ref lstFormulaRecursive, monValue, null, null, 1, entityBehavior.RoundingId, 1, false);
                        }
                        else
                        {
                            payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input values is not available" });
                        }
                        #endregion
                    }
                    else if (u.AttributeModel.IsInstallment)//Loan
                    {
                        #region execute loan
                        decimal loanAmount = GetLoanAmount(companyId, curEmployee.Id, u.AttributeModelId, year, month);
                        AddValuesTemp(u, ref lstFormulaRecursive, loanAmount.ToString(), null, null, 2, 1, 1);
                        #endregion
                    }
                    else if (u.AttributeModel.IsIncrement)//increment and arrear process
                    {
                        #region execute increment setting 
                        if (entityBehavior.ValueType == 1)//Employee Master  input
                        {
                            #region execute employee master input
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(entitymastervalue, null))
                            {
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                            }
                            else
                            {
                                //modified by AjithPanner on 13/11/2017
                                if (entityBehavior.AttributeModelId != entityBehavior.ArrearAttributeModelId)
                                {
                                    payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Master Value is not available" });
                                }
                                else if (entityBehavior.AttributeModelId == null)
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }
                            #endregion
                        }
                        List<ArrearHistory> saveArrearHistory = new List<ArrearHistory>();
                        //  CalculateIncrement(companyId, curEmployee.Id, u, ref lstFormulaRecursive, ref saveArrearHistory, entityBehavior, entity, attributemodelList, month, year);
                        if (u.AttributeModel.Name == "EB")
                        {

                        }

                        /*
                        if (incrementlist.Count > 0 && incrementlist[0].IsProcessed == false && incrementlist[0].EffectiveDate <= DateTime.Now)
                        {
                            Increment inc = incrementlist[0];
                            int appYear = inc.ApplyYear;
                            double arrearVal = 0.0;
                            int aplMonth = inc.ApplyMonth;
                            if (appYear <= year)
                            {
                                Guid arrMatchId = entityBehavior.ArrearAttributeModelId;
                                do
                                {
                                    if (aplMonth < month)
                                    {
                                        if (!object.ReferenceEquals(entityBehavior, null))
                                        {
                                            do//month do
                                            {
                                                var incDet = inc.IncrementDetailList.Where(ine => ine.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                                string strOut = "0.0";
                                                if (!object.ReferenceEquals(incDet, null))
                                                {
                                                    double dif = (double)(incDet.NewValue - incDet.OldValue);
                                                    strOut = dif.ToString();
                                                }
                                                // string arrerFormula = entityBehavior.ArrearFormula;
                                                PayrollHistory payArr = new PayrollHistory(employeeId, appYear, aplMonth);
                                                var atrTmp = attributemodelList.Where(am => am.Name == "MD").FirstOrDefault();
                                                var colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                                if (!object.ReferenceEquals(colTemp, null))
                                                    strOut = strOut + "/" + colTemp.Value;
                                                atrTmp = attributemodelList.Where(am => am.Name == "PD").FirstOrDefault();
                                                colTemp = payArr.PayrollHistoryValueList.Where(a => a.AttributeModelId == atrTmp.Id).FirstOrDefault();
                                                if (!object.ReferenceEquals(colTemp, null))
                                                    strOut = strOut + "*" + colTemp.Value;
                                                Eval eval = new Eval();
                                                double result = eval.Execute(strOut);
                                                arrearVal = arrearVal + result;
                                                aplMonth++;

                                            } while (aplMonth < month);//month while
                                        }
                                    }
                                    appYear = appYear + 1;
                                    aplMonth = 1;
                                } while (appYear <= year);
                                if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault(), null))
                                {
                                    lstFormulaRecursive.Add(new FormulaRecursive()
                                    {
                                        //entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name == "AG" ? "7253" : "0",
                                        Assignedvalues = arrearVal.ToString(),
                                        Id = arrMatchId.ToString(),
                                        Name = entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name,
                                        ExecuteOrder = 4,
                                        Rounding = entityBehavior.RoundingId
                                    });
                                }
                                else
                                {
                                    //entity.EntityAttributeModelList.Where(ar => ar.AttributeModelId == arrMatchId).FirstOrDefault().AttributeModel.Name == "AG" ? "7253" : "0";//
                                    lstFormulaRecursive.Where(p => p.Id.ToUpper() == arrMatchId.ToString().ToUpper()).FirstOrDefault().Assignedvalues = arrearVal.ToString();
                                }

                                if (!object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault(), null))
                                {
                                    var t1 = inc.IncrementDetailList.Where(q => q.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (t1 != null)
                                    {
                                        lstFormulaRecursive.Where(p => p.Id == u.AttributeModelId.ToString()).FirstOrDefault().Assignedvalues = t1.NewValue.ToString();
                                    }
                                }
                            }

                        }
                        */
                        #endregion
                    }
                    else if (!object.ReferenceEquals(entityBehavior, null))
                    {
                        var mastersettingVal = masterSettingslist.Where(x => x.AttributeId == entityBehavior.AttributeModelId).FirstOrDefault();
                        string tempPercentage = entityBehavior.Percentage;
                        if (mastersettingVal != null)
                        {
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            tempPercentage = entitymastervalue != null ? entitymastervalue.Value : tempPercentage;
                        }


                        if (entityBehavior.ValueType == 1)//Employee Master  input
                        {
                            #region execute employee master input
                            var entitymastervalue = entitymasterValueslist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(entitymastervalue, null))
                            {
                                string assValues = entitymastervalue != null ? entitymastervalue.Value : "0";
                                //  AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                //Credit Days
                                if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                                {
                                    if (creditDaysList.Count > 0)
                                    {
                                        IncrementList prevMaster = new IncrementList(curEmployee.Id);
                                        Increment preInc = prevMaster.Where(m => new DateTime(m.ApplyYear, m.ApplyMonth, 1) <= new DateTime(creditDaysList[0].Month)).OrderByDescending(m => m.ApplyYear).OrderByDescending(m => m.ApplyMonth).FirstOrDefault();
                                        if (!ReferenceEquals(preInc, null))
                                        {
                                            IncrementDetailList inc = new IncrementDetailList(preInc.Id);
                                            if (!ReferenceEquals(inc, null))
                                            {
                                                if (inc.Where(i => i.AttributeModelId == u.AttributeModelId).Any())
                                                {
                                                    assValues = inc.Where(i => i.AttributeModelId == u.AttributeModelId).FirstOrDefault().NewValue.ToString();
                                                }

                                            }
                                        }
                                        var totpaidDays = creditDaysList.Sum(p => p.PaidDays);
                                        AddValuesTemp(u, ref lstFormulaRecursive, (assValues + "/" + totpaidDays.ToString()), entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 3);

                                    }
                                    else
                                    {
                                        AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                    }
                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                            {
                                //modified by AjithPanner on 13/11/2017
                                if (entityBehavior.AttributeModelId != entityBehavior.ArrearAttributeModelId)
                                {
                                    payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Master Value is not available" });
                                }
                                else if (entityBehavior.AttributeModelId == null)
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }

                            #endregion

                        }
                        else if (entityBehavior.ValueType == 2)//Monthly input
                        {
                            #region execute monthly input
                            var monInput = monthlyinputlist.Where(p => p.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                            if (!object.ReferenceEquals(monInput, null))
                            {
                                string assValues = monInput != null ? monInput.Value : "0";
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 1, entityBehavior.RoundingId, 2);
                            }
                            else
                            {
                                payrollErrors.Add(new PayrollError() { Name = u.AttributeModel.Name, ErrorMessage = u.AttributeModel.DisplayAs + ": Monthly Input value is not available" });
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 3)//Formula
                        {
                            #region execute formula/percentage
                            //   AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                            //Credit Days supplementary
                            if (u.AttributeModel.Name == "PD" && employee.DateOfJoining.Month == month && employee.DateOfJoining.Year == year)
                            {
                                int md = DateTime.DaysInMonth(year, month);
                                if (!string.IsNullOrEmpty(category.MonthDayProcess))
                                {
                                    DateTime sdate = DateTime.MinValue;
                                    if (category.MonthDayProcess.ToUpper() == "MONTHDAY")
                                    {
                                        sdate = new DateTime(year, month, 1);
                                    }
                                    else if (category.MonthDayProcess.ToUpper() == "STATICDAY")
                                    {
                                        sdate = new DateTime(year, month, 1);
                                    }
                                    else if (category.MonthDayProcess.ToUpper() == "STARTDAY")
                                    {
                                        DateTime newdate = new DateTime(year, month, category.MonthDayOrStartDay);


                                        DateTime edate = newdate.AddDays(-1);
                                        if (newdate > employee.DateOfJoining && edate < employee.DateOfJoining)
                                        {
                                            sdate = newdate.AddMonths(-1);
                                        }

                                    }
                                    TimeSpan pdts = employee.DateOfJoining - sdate;
                                    md = pdts.Days + 1;

                                }

                                entityBehavior.Formula = entityBehavior.Formula + "-" + md.ToString();
                            }
                            if (suppComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                creditDaysList.RemoveAll(r => r.IsProcessed == true);
                                if (creditDaysList.Count > 0)
                                {

                                    decimal totpaidDays = creditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = creditDaysList.Sum(p => p.PaidDays);
                                    Guid supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();

                                    Guid suplopid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryLOPDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == suplopid.ToString()).FirstOrDefault().Assignedvalues = totlopDays.ToString();



                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                                    //Credit Days lop credit
                                    if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (creditDaysList.Count > 0)
                                {


                                    decimal totpaidDays = creditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = creditDaysList.Sum(p => p.PaidDays);
                                    Guid supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.LOPCreditDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();

                                    AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 4, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.Formula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 3);
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 4)//Conditionl formula  --if else
                        {
                            #region execute Conditionl formula  --if else
                            string assValues = entityBehavior != null ? entityBehavior.Formula : "0";
                            // AddValuesTemp(u, ref lstFormulaRecursive, assValues, entityBehavior.Percentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                            //Credit Days supplementary
                            if (suppComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (creditDaysList.Count > 0)
                                {
                                    decimal totpaidDays = creditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = creditDaysList.Sum(p => p.PaidDays);
                                    Guid supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();

                                    Guid suplopid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.SupplementaryLOPDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == suplopid.ToString()).FirstOrDefault().Assignedvalues = totlopDays.ToString();

                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                                //Credit Days lop credit
                                if (lopComponents.Where(l => l.AttrId == u.AttributeModel.Id).Any())
                            {
                                if (creditDaysList.Count > 0)
                                {


                                    decimal totpaidDays = creditDaysList.Sum(p => p.PaidDays);
                                    decimal totlopDays = creditDaysList.Sum(p => p.PaidDays);
                                    Guid supid = entity.EntityAttributeModelList.Where(s => s.AttributeModel.Name == ComValue.LOPCreditDays).FirstOrDefault().AttributeModelId;
                                    lstFormulaRecursive.Where(l => l.Id == supid.ToString()).FirstOrDefault().Assignedvalues = totpaidDays.ToString();

                                    AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);

                                }
                                else
                                {
                                    AddValuesTemp(u, ref lstFormulaRecursive, "0", tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 1);
                                }
                            }
                            else
                            {
                                AddValuesTemp(u, ref lstFormulaRecursive, assValues, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 4);
                            }
                            #endregion
                        }
                        else if (entityBehavior.ValueType == 5)//Range type
                        {
                            AddValuesTemp(u, ref lstFormulaRecursive, entityBehavior.BaseFormula, tempPercentage, entityBehavior.EligibiltyFormula, 5, entityBehavior.RoundingId, 5);
                        }

                    }
                    else
                    {
                        AddValuesTemp(u, ref lstFormulaRecursive, u.EntityAttributeValue.Value, null, null, 0, 1, 1);

                    }
                }
            });
            lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.ExecuteOrder).ToList();
            if (payrollErrors.Count > 0)//dont process the formula execution
            {
                errors = payrollErrors;
                return new Entity();
            }
            lstFormulaRecursive.ForEach(u =>
            {
                u.UserId = createdBy;
                u.Year = year;
                u.CompanyId = companyId;
                u.Month = month;
                u.EmployeeId = curEmployee.Id;
            });
            PayrollHistoryList Historylist = new PayrollHistoryList(companyId, taxInfo.FinanceYear.StartingDate.Year, taxInfo.FinanceYear.StartingDate.Month, taxInfo.FinanceYear.EndingDate.Year, taxInfo.FinanceYear.EndingDate.Month, curEmployee.Id);
            taxInfo.payrollhistorylist = new PayrollHistoryList();
            if (!object.ReferenceEquals(Historylist,null))
            {
                taxInfo.payrollhistorylist.AddRange(Historylist);
            }
            
            recursive(taxInfo, employee, lstFormulaRecursive, entity, incrementlist, true);

            lstFormulaRecursive = lstFormulaRecursive.OrderBy(u => u.Order).ToList();
            lstFormulaRecursive.ForEach(u =>
            {
                string error = string.Empty;
                string output = string.Empty;
                string rerun = string.Empty;
                u.Validate(u.Assignedvalues, lstFormulaRecursive, u.Id, ref error, out output, rerun);
                if (!string.IsNullOrEmpty(error))
                {
                    payrollErrors.Add(new PayrollError() { Name = u.Name, ErrorMessage = "There is a some problem in formula.Please check it." });
                    entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = error;
                }
                else
                {
                    string result = string.Empty;
                    if (u.type == 4)
                    {
                        ifElseStmt obj = new ifElseStmt();
                        List<ifElseStmt> ifElseCollection = obj.GetifElse(output);
                        ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                        var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                        if (!object.ReferenceEquals(tm, null))
                            output = tm.thenVal;
                    }
                    Eval eval = new Eval();
                    result = eval.Execute(output).ToString();
                    u.Output = result;
                    u.Assignedvalues = result.ToString();
                    if (!object.ReferenceEquals(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault(), null))
                    {
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.Value = result.ToString();
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == new Guid(u.Id)).FirstOrDefault().EntityAttributeValue.BaseValue = u.BaseValue;
                    }
                    else
                    {
                        string str = u.Id;
                    }

                }

            });
            errors = payrollErrors;
            return entity;
        }

        private void recursive(TaxComputationInfo taxInfo, Employee employee, List<FormulaRecursive> lstFormulaRecursive, Entity entity, IncrementList increment, bool ffFlag)
        {
            lstFormulaRecursive.ForEach(u =>
            {
                if (u.Assignedvalues == null)
                {
                    u.Assignedvalues = "0";
                }
                if (u.Name == "EmployeeServiceYear")
                {
                    u.Assignedvalues = new Employee(u.EmployeeId).NoOfServiceYear.ToString();
                }
                if (u.Name.Trim().ToUpper() == "GRATUITY")
                {
                    FormulaRecursive fRecursive = lstFormulaRecursive.Where(w => w.Name == "EmployeeServiceYear").FirstOrDefault();
                    double serviceYear = 0;
                    if (!Object.Equals(fRecursive, null))
                    {
                        serviceYear = Convert.ToDouble(fRecursive.Assignedvalues.Trim());
                        if (serviceYear <= 4)
                        {
                            lstFormulaRecursive.Where(w => w.Name == "EmployeeServiceYear").FirstOrDefault().Assignedvalues = "0";
                        }

                    }

                    string temp = u.Assignedvalues;
                    string eligibleTemp = u.EligibleFormula;
                    if ((!string.IsNullOrEmpty(temp) && temp.IndexOf("{") >= 0) || (!string.IsNullOrEmpty(eligibleTemp) && eligibleTemp.IndexOf('{') >= 0))
                    {
                        if (!string.IsNullOrEmpty(temp) && temp.IndexOf('{') >= 0)
                        {
                            PayrollHistoryList payHist = new PayrollHistoryList(u.EmployeeId, u.CompanyId, 0, 0);
                            if (payHist.Count > 0)
                            {
                                int year = payHist.Where(w => w.Status == "Processed" || w.Status == "Imported").OrderByDescending(o => o.Year).FirstOrDefault().Year;
                                int Month = payHist.Where(w => (w.Status == "Processed" || w.Status == "Imported") && w.Year == year).OrderByDescending(o => o.Month).FirstOrDefault().Month;
                                Guid payHis = payHist.Where(i => i.Month == Month && i.Year == year).FirstOrDefault().Id;

                                PayrollHistoryValueList payHistVal = new PayrollHistoryValueList(payHis);
                                do
                                {
                                    try
                                    {
                                        string id = temp.Substring(temp.IndexOf("{") + 1, 36);

                                        var colTemp = payHistVal.Where(g => g.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                                        string replacevalue = "{" + id + "}";
                                        if (!object.ReferenceEquals(colTemp, null))
                                        {
                                            temp = temp.Replace(replacevalue, colTemp.Value);


                                        }
                                        else
                                        {

                                            var col = lstFormulaRecursive.Where(v => v.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();


                                            if (!object.ReferenceEquals(col, null))
                                            {
                                                temp = temp.Replace(replacevalue, col.Assignedvalues);//if the attribute is not in the Entity attribute collection
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                                while (temp.IndexOf('{') >= 0);

                                u.Assignedvalues = temp;
                            }
                            else
                            {
                                u.Assignedvalues = "0";
                            }
                        }


                    }

                }
                u.ReOrder(taxInfo, employee, lstFormulaRecursive, u, entity, increment, ffFlag);
            });
            lstFormulaRecursive.ForEach(u =>
            {
                if (u.Order == 0)
                {
                    recursive(taxInfo, employee, lstFormulaRecursive, entity, increment, ffFlag);

                }

            });
        }

        public string DeletePayrollProcess(int companyId, Guid employeeId, int year, int month, int createdBy,string prname)
        {
            Employee emp = new Employee(employeeId);
            string returnval = "";
            if (string.IsNullOrEmpty(Convert.ToString(emp.LastWorkingDate)) || emp.LastWorkingDate <= DateTime.MinValue || emp.ReleaseDate > DateTime.MinValue || prname == "F&F")
            {

                //  PayrollHistoryList payrollHistorylist = new PayrollHistoryList(companyId, employeeId);

                //  payrollHistorylist.ToList().OrderByDescending(y => y.Month);
                bool isDelFlag = true;
                //  foreach (var pp in payrollHistorylist)
                // {
                //     DateTime dt = new DateTime(pp.Year, pp.Month, 01);
                //     DateTime curPP = new DateTime(year, month, 01);
                //     if (curPP < dt)
                //     {
                //        isDelFlag = false;
                //        returnval = "future month processed,";
                //       break;
                //   }
                //  }
                if (isDelFlag)
                {
                    PayrollHistory payrollHistory = new PayrollHistory(companyId, employeeId, year, month);
                    if (!object.ReferenceEquals(payrollHistory, null))  // && payrollHistory.Status == "Processed")
                    {

                        LoanEntryList loanEntrylist = new LoanEntryList(employeeId, Guid.Empty);
                        var ChkLoanEntry = loanEntrylist.Where(u => u.ApplyDate.Year <= year && u.ApplyDate.Month <= month).ToList();

                        ChkLoanEntry.ForEach(u =>
                        {
                            LoanTransactionList loanTransactionList = new LoanTransactionList(u.Id);
                            var ChkLoanTransaction = loanTransactionList.Where(v => v.AppliedOn.Year == year && v.AppliedOn.Month == month).ToList();
                            ChkLoanTransaction.ForEach(x =>
                            {
                                LoanTransaction loanTransaction = new LoanTransaction(x.Id, x.LoanEntryId);
                                loanTransaction.Status = "UnPaid";
                                loanTransaction.isPayRollProcess = false;
                                //loanTransaction.Delete();
                                loanTransaction.Save();
                            });

                        });

                        Increment increment = new Increment();
                        increment.EmployeeId = employeeId;
                        increment.ApplyMonth = month;
                        increment.ApplyYear = year;
                        increment.ProcessFlag = "IsProcessDel";
                        increment.Delete();


                        // After delete the payroll process check increment apply  for that month we reverse the old value of master values. 10 Aug 2018
                        increment.ProcessFlag = "IncrementDelCheck";
                        DataTable dtIncrementDel = increment.CheckIncrementDel();
                        if (dtIncrementDel.Rows.Count > 0)
                        {
                            EntityMasterValueList entitymaster = new EntityMasterValueList(employeeId, ComValue.EmployeeTable);//entity.EntityModelId.ToString()
                            IncrementList prevMaster = new IncrementList(employeeId);
                            Increment preInc = prevMaster.Where(m => new DateTime(m.ApplyYear, m.ApplyMonth, 1) <= new DateTime(year, month, 1)).OrderByDescending(m => m.ApplyYear).OrderByDescending(m => m.ApplyMonth).FirstOrDefault();
                            if (!ReferenceEquals(preInc, null))
                            {
                                IncrementDetailList inc = new IncrementDetailList(preInc.Id);
                                inc.ForEach(i =>
                                {
                                    var masVal = entitymaster.Where(x => x.AttributeModelId == i.AttributeModelId).FirstOrDefault();
                                    if (masVal != null)
                                    {
                                        masVal.ModifiedOn = DateTime.Now;
                                        masVal.Value = Convert.ToString(String.Format("{0:0.00}", Convert.ToDecimal(i.OldValue)));
                                        masVal.Save();
                                    }
                                });
                            }
                        }
                        PayrollHistory DeltePayrollHistory = new PayrollHistory();
                        DeltePayrollHistory.Id = payrollHistory.Id;
                        DeltePayrollHistory.Delete();
                        //       PayrollTransaction paytrans = new PayrollTransaction();
                        //      paytrans.PayrollHistoryId = payrollHistory.Id;
                        //      paytrans.EmployeeId = payrollHistory.EmployeeId;
                        //      paytrans.Delete();


                        // After delete payroll process supplimentary days isprocessed update.
                        CreditDaysList suplcreditDaysList = new CreditDaysList(companyId, month, year, employeeId, "Supp");
                        DateTime dtnow = DateTime.Now;
                        suplcreditDaysList.ForEach(sd =>
                        {
                            CreditDays suplday = new CreditDays(sd.Id);
                            suplday.IsProcessed = false;
                            suplday.ModifiedBy = createdBy;
                            suplday.Save();

                        });
                    }
                }
            }
            return returnval;
        }

        public void FandFProcess(int companyId, Guid employeeId, int year, int month, int createdBy, int paidDays = 0)
        {
            List<PayrollError> payrollErrors;
            Employee emp = new Employee(companyId, employeeId);
            Entity entity = this.ExecuteFFProcess(companyId, emp, year, month, createdBy, out payrollErrors);//, paidDays);
            if (payrollErrors.Count > 0)
            {
                if (payrollErrors[0].ErrorMessage == "Already Processed")
                {
                    payrollErrors.RemoveAt(0);
                    return;

                }
                else if (payrollErrors[0].ErrorMessage == "Not Mapped with Salary")
                {
                    PayrollHistory payHostory = new PayrollHistory();
                    payHostory.CompanyId = companyId;
                    payHostory.EmployeeId = employeeId;
                    payHostory.EntityId = entity.Id;
                    payHostory.EntityModelId = entity.EntityModelId;
                    payHostory.Month = month;
                    payHostory.Year = year;
                    payHostory.CreatedBy = createdBy;
                    payHostory.Status = "Not Mapped with Salary";
                    payHostory.Save();
                    payrollErrors.RemoveAt(0);
                    return;
                }
            }
            if (payrollErrors.Count <= 0)
            {
                DateTime payRollDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                PayrollHistory payHostory = new PayrollHistory();
                payHostory.CompanyId = companyId;
                payHostory.EmployeeId = employeeId;
                payHostory.EntityId = entity.Id;
                payHostory.EntityModelId = entity.EntityModelId;
                payHostory.Month = month;
                payHostory.Year = year;
                payHostory.CreatedBy = createdBy;
                payHostory.Status = ComValue.payrollProcessStatus[0];
                if (payHostory.Save())
                {
                    EntityBehaviorList entityBehaviorList = new EntityBehaviorList(entity.Id, entity.EntityModelId);
                    PayrollHistoryValueList payHistorylist = new PayrollHistoryValueList();
                    entity.EntityAttributeModelList.ForEach(u =>
                    {
                        EntityBehavior eb = entityBehaviorList.Where(w => w.AttributeModelId == u.AttributeModelId).FirstOrDefault();

                        payHistorylist.Add(new PayrollHistoryValue()
                        {
                            AttributeModelId = u.AttributeModelId,
                            CreatedBy = createdBy,
                            PayrollHistoryId = payHostory.Id,
                            Value = u.EntityAttributeValue.Value,
                            ValueType = eb == null ? 1 : eb.ValueType

                        });
                        if (u.AttributeModel.IsInstallment)
                        {
                            LoanMaster loanMasterr = new LoanMaster(companyId, u.AttributeModelId);
                            LoanEntryList loanEntryList = new LoanEntryList();
                            if (loanMasterr.Id != Guid.Empty)
                            {
                                loanEntryList = new LoanEntryList(employeeId, loanMasterr.Id);

                                loanEntryList.ForEach(s =>
                                {
                                    var loantransaction = new LoanTransactionList(s.Id);
                                    s.ApplyDate = s.ApplyDate.AddDays(-s.ApplyDate.Day);
                                    DateTime payTime = new DateTime(year, month, 1);
                                    if (s.ApplyDate <= payTime && loantransaction.Count < s.NoOfMonths)
                                    {
                                        LoanTransaction loanTran = new LoanTransaction();
                                        loanTran.LoanEntryId = s.Id;
                                        loanTran.Status = "Paid";
                                        loanTran.isPayRollProcess = true;
                                        loanTran.AmtPaid = s.AmtPerMonth;
                                        //loanTran.InterestAmt=s.
                                        loanTran.AppliedOn = payRollDate;//DateTime.Now;
                                        loanTran.CreatedBy = createdBy;
                                        loanTran.Save();
                                    }
                                });
                            }
                        }
                        DateTime Applydate = new DateTime();
                        if (u.AttributeModel.IsIncrement)
                        {
                            IncrementList incrementlist = new IncrementList(employeeId);
                            List<Increment> processIncrement = new List<Increment>();
                            PayrollHistoryList currpayroll = new PayrollHistoryList(employeeId, companyId);
                            Applydate = new DateTime(incrementlist[0].ApplyYear, incrementlist[0].ApplyMonth, DateTime.DaysInMonth(incrementlist[0].ApplyYear, incrementlist[0].ApplyMonth));
                            incrementlist.ForEach(inc =>
                            {
                                if (inc.IsProcessed == false && currpayroll.Count > 0)
                                //if (incrementlist.Count > 0 && incrementlist[0].IsProcessed == false && incrementlist[0].EffectiveDate <= payRollDate)//<= DateTime.Now)
                                {
                                    currpayroll.ForEach(s =>
                                    {
                                        if (s.Month == inc.ApplyMonth && s.Year == inc.ApplyYear)
                                        {
                                            inc.IsProcessed = true;
                                        }
                                    });
                                    //  Increment inc = incrementlist[0];
                                    inc.ModifiedBy = createdBy;
                                    inc.Save();
                                    processIncrement.Add(inc);

                                }
                            });
                            if (processIncrement.Count > 0)
                            {
                                var maxVal = processIncrement.Max(mx => mx.EffectiveDate);//Max effetive date and apply the New value
                                var maxElem = processIncrement.Where(a => a.EffectiveDate == maxVal).FirstOrDefault();
                                EntityBehavior entBehav = entityBehaviorList.Where(p => p.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                if (entBehav != null)
                                {
                                    var incDetails = maxElem.IncrementDetailList.Where(s => s.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (incDetails != null)
                                    {
                                        EntityMasterValueList entitymaster = new EntityMasterValueList(employeeId, ComValue.EmployeeTable);//entity.EntityModelId.ToString()
                                        var entMasterVal = entitymaster.Where(s => s.AttributeModelId == u.AttributeModelId && s.EntityModelId == entity.EntityModelId && s.EntityId == entity.Id).FirstOrDefault();
                                        if (entMasterVal != null)
                                        {
                                            entMasterVal.ModifiedBy = createdBy;
                                            entMasterVal.Value = incDetails.NewValue.ToString();
                                            entMasterVal.Save();
                                        }
                                    }
                                }
                            }

                        }

                    });
                    payHistorylist.ForEach(u => u.Save());
                }
            }
            else
            {
                string errmessage = string.Empty;
                payrollErrors.ForEach(p =>
                {
                    errmessage = errmessage + p.ErrorMessage + "&";
                });
                errmessage.Remove(errmessage.Length - 1);
                PayrollHistory payHostory = new PayrollHistory();
                payHostory.CompanyId = companyId;
                payHostory.EmployeeId = employeeId;
                payHostory.EntityId = entity.Id;
                payHostory.EntityModelId = entity.EntityModelId;
                payHostory.Month = month;
                payHostory.Year = year;
                payHostory.CreatedBy = createdBy;
                payHostory.Status = errmessage; //"There is some problem while process the Payroll";
                payHostory.Save();
            }

        }

        public PayrollHistory PayrollProcess(int companyId, Guid employeeId, int year, int month, int createdBy, bool FFflag ,string DBid)
        {
            bool flag = false;
            List<PayrollError> payrollErrors;
            List<ArrearHistory> saveArrearHistory = new List<ArrearHistory>();
            Employee emp = new Employee(companyId, employeeId);
            Entity entity = this.ExecuteProcess(companyId, emp, year, month, createdBy, out saveArrearHistory, out payrollErrors, FFflag);
            PayrollHistory payHistory = new PayrollHistory();
            if (payrollErrors.Count > 0)
            {
                if (payrollErrors[0].ErrorMessage == "Already Processed")
                {
                    //payrollErrors.RemoveAt(0);
                    flag = false;
                    return new PayrollHistory();
                }
                else if (payrollErrors[0].ErrorMessage == "Not Mapped with Salary")
                {
                    payHistory = new PayrollHistory();

                    payHistory.Id = Guid.NewGuid();
                    payHistory.CompanyId = companyId;
                    payHistory.EmployeeId = employeeId;
                    payHistory.EntityId = entity.Id;
                    payHistory.EntityModelId = entity.EntityModelId;
                    payHistory.Month = month;
                    payHistory.Year = year;
                    payHistory.CreatedBy = createdBy;
                    payHistory.Status = "Not Mapped with Salary";
                    // payHistory.Save();
                    //payrollErrors.RemoveAt(0);
                    flag = false;
                }
                else if (payrollErrors[0].ErrorMessage == "Future Month processed")
                {
                    payHistory = new PayrollHistory();

                    payHistory.Id = Guid.NewGuid();
                    payHistory.CompanyId = companyId;
                    payHistory.EmployeeId = employeeId;
                    payHistory.EntityId = entity.Id;
                    payHistory.EntityModelId = entity.EntityModelId;
                    payHistory.Month = month;
                    payHistory.Year = year;
                    payHistory.CreatedBy = createdBy;
                    payHistory.Status = "Future month payroll processed so can't process.";
                    //payHistory.Save();
                    //payrollErrors.RemoveAt(0);
                    flag = false;
                }
            }
            if (payrollErrors.Count <= 0)
            {
                DateTime payRollDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                payHistory = new PayrollHistory();
                payHistory.Id = Guid.NewGuid();
                payHistory.CompanyId = companyId;
                payHistory.EmployeeId = employeeId;
                payHistory.EntityId = entity.Id;
                payHistory.EntityModelId = entity.EntityModelId;
                payHistory.Month = month;
                payHistory.Year = year;
                payHistory.CreatedBy = createdBy;
                payHistory.Status = ComValue.payrollProcessStatus[0];
                payHistory.IsFandF = FFflag;

                // if (payHistory.Save())
                // {
                this.Id = payHistory.Id;
                EntityBehaviorList entityBehaviorList = new EntityBehaviorList(entity.Id, entity.EntityModelId);
                PayrollHistoryValueList payHistorylist = new PayrollHistoryValueList();
                AttributeModelList attrList = new AttributeModelList(companyId);

                var TDSattributeId = attrList.Where(x => x.Name == "TDS").FirstOrDefault();
                Guid TDSattId = TDSattributeId == null ? Guid.Empty : TDSattributeId.Id;

                // After payroll process supplimentary days isprocessed update.
                CreditDaysList suplcreditDaysList = new CreditDaysList(companyId, month, year, employeeId, "Supp", "payrollprocess");
                DateTime dtnow = DateTime.Now;
                suplcreditDaysList.ForEach(sd =>
                {
                    CreditDays suplday = new CreditDays(sd.Id);
                    suplday.IsProcessed = true;
                    suplday.ModifiedBy = createdBy;
                    suplday.Save();

                });


                PayrollHistoryList currpayroll = new PayrollHistoryList(employeeId, companyId);

                entity.EntityAttributeModelList.ForEach(u =>
                {
                    if (u.AttributeModelId.ToString() == "286dccb0-f985-4842-a1ed-0bc309de342e")
                    {
                        Console.WriteLine("message");
                    }
                    if (u.AttributeModelId == TDSattId)
                    {
                        TaxValueUpdate(companyId, emp, month, year, attrList, u.EntityAttributeValue.BaseValue);
                    }
                    EntityBehavior eb = entityBehaviorList.Where(w => w.AttributeModelId == u.AttributeModelId).FirstOrDefault();

                    var attDetails = attrList.Where(x => x.Id == u.AttributeModelId).FirstOrDefault();
                    payHistorylist.Add(new PayrollHistoryValue()
                    {
                        Id = Guid.NewGuid(),
                        AttributeModelId = u.AttributeModelId,
                        CreatedBy = createdBy,
                        PayrollHistoryId = payHistory.Id,
                        Value = u.EntityAttributeValue.Value,
                        ValueType = eb == null ? 1 : eb.ValueType,
                        BaseValue = u.EntityAttributeValue.BaseValue,
                        IsTaxable = attDetails.IsTaxable,
                        IncludeGrossPay = attDetails.IsIncludeForGrossPay,
                        BehaviorType = attDetails.BehaviorType

                    });


                    if (u.AttributeModel.IsInstallment)
                    {
                        LoanMaster loanMasterr = new LoanMaster(companyId, u.AttributeModelId);
                        LoanEntryList loanEntryList = new LoanEntryList();
                        if (loanMasterr.Id != Guid.Empty)
                        {
                            loanEntryList = new LoanEntryList(employeeId, loanMasterr.Id);

                            loanEntryList.ForEach(s =>
                            {
                                var loantransaction = new LoanTransactionList(s.Id);
                                LoanTransaction loanTran = loantransaction.Where(l => l.AppliedOn.Month == month && l.AppliedOn.Year == year).FirstOrDefault();
                                s.ApplyDate = s.ApplyDate.AddDays(-s.ApplyDate.Day);
                                DateTime payTime = new DateTime(year, month, 1);
                                if (s.ApplyDate <= payTime && loanTran != null)//&& loantransaction.Count < s.NoOfMonths)
                                {
                                    // LoanTransaction loanTran = new LoanTransaction();
                                    // loanTran.LoanEntryId = s.Id;
                                    loanTran.Status = "Paid";
                                    loanTran.isPayRollProcess = true;

                                    //loanTran.InterestAmt=s.
                                    loanTran.AppliedOn = payRollDate;//DateTime.Now;
                                    loanTran.CreatedBy = createdBy;
                                    loanTran.Save();
                                }
                            });
                        }
                    }
                    if (u.AttributeModel.IsIncrement)
                    {
                        IncrementList incrementlist = new IncrementList(employeeId);
                        //   PayrollHistoryList currpayroll = new PayrollHistoryList(employeeId, companyId);
                        List<Increment> processIncrement = new List<Increment>();
                        incrementlist.ForEach(inc =>
                        {
                            //if (inc.IsProcessed == false && currpayroll.Count > 0)
                            if (currpayroll.Count > 0)
                            //if (incrementlist.Count > 0 && incrementlist[0].IsProcessed == false && incrementlist[0].EffectiveDate <= payRollDate)//<= DateTime.Now)
                            {
                                //currpayroll.ForEach(s =>
                                //{
                                //    if (s.Month == inc.ApplyMonth && s.Year == inc.ApplyYear)
                                //    {
                                //        inc.IsProcessed = true;
                                //    }
                                //});
                                //  Increment inc = incrementlist[0];
                                inc.ModifiedBy = createdBy;
                                // inc.Save();
                                processIncrement.Add(inc);

                            }
                        });
                        if (processIncrement.Count > 0)
                        {
                            var maxVal = processIncrement.Max(mx => mx.EffectiveDate);//Max effetive date and apply the New value
                            var maxElem = processIncrement.Where(a => a.EffectiveDate == maxVal).FirstOrDefault();
                            var maxMonth = maxElem.EffectiveDate.Month + "-" + maxElem.EffectiveDate.Year;
                            var curentMoth = DateTime.Now.Month + "-" + DateTime.Now.Year;
                            if (maxMonth == curentMoth &&  DBid != "2007") {
                                
                                EntityBehavior entBehav = entityBehaviorList.Where(p => p.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                if (entBehav != null)
                                {
                                    var incDetails = maxElem.IncrementDetailList.Where(s => s.AttributeModelId == u.AttributeModelId).FirstOrDefault();
                                    if (incDetails != null)
                                    {
                                        EntityMasterValueList entitymaster = new EntityMasterValueList(employeeId, ComValue.EmployeeTable);//entity.EntityModelId.ToString()
                                        var entMasterVal = entitymaster.Where(s => s.AttributeModelId == u.AttributeModelId && s.EntityModelId == entity.EntityModelId && s.EntityId == entity.Id).FirstOrDefault();
                                        if (entMasterVal != null)
                                        {
                                            entMasterVal.ModifiedBy = createdBy;
                                            entMasterVal.Value = Convert.ToString(String.Format("{0:0.00}", (incDetails.NewValue)));
                                            entMasterVal.Save();
                                        }
                                    }
                                }
                            }
                        }

                    }

                });


                IncrementList incrementlistupdate = new IncrementList(employeeId);
                //    PayrollHistoryList currpayrollupdate = new PayrollHistoryList(employeeId, companyId);

                incrementlistupdate.ForEach(incupate =>
                {
                    if (incupate.IsProcessed == false && payHistory.Status == "Processed")
                    //if (incrementlist.Count > 0 && incrementlist[0].IsProcessed == false && incrementlist[0].EffectiveDate <= payRollDate)//<= DateTime.Now)
                    {
                        // currpayrollupdate.ForEach(s =>
                        //   {
                        if (payHistory.Month == incupate.ApplyMonth && payHistory.Year == incupate.ApplyYear)
                        {
                            incupate.IsProcessed = true;
                        }
                        //   });
                        //  Increment inc = incrementlist[0];
                        incupate.ModifiedBy = createdBy;
                        incupate.Save();


                    }
                });
                //payHistorylist.ForEach(u =>
                //u.Save());
                //bulk insert using xmlstring modified on 9/8/2018
                payHistory.currentPayValues = payHistorylist;

                if (saveArrearHistory != null)
                {
                    saveArrearHistory.ForEach(u =>
                    {
                        EntityBehavior ent = new EntityBehavior(u.AttributeModelId, payHistory.EntityModelId, payHistory.EntityId);
                        u.AttributeModelId = ent.ArrearAttributeModelId;
                        u.PayHistoryId = payHistory.Id;
                        u.Value = Convert.ToDecimal(payHistorylist.Where(x => x.AttributeModelId == ent.ArrearAttributeModelId && payHistory.EmployeeId == u.EmployeeId).FirstOrDefault().Value);
                        u.Save();

                    });
                }
                flag = true;
            }
            //   }
            if (payrollErrors.Count > 0)
            {
                string errmessage = string.Empty;
                payrollErrors.ForEach(p =>
                {
                    errmessage = errmessage + p.ErrorMessage + "&";
                });
                errmessage.Remove(errmessage.Length - 1);
                payHistory = new PayrollHistory();
                payHistory.Id = Guid.NewGuid();
                payHistory.CompanyId = companyId;
                payHistory.EmployeeId = employeeId;
                payHistory.EntityId = entity.Id;
                payHistory.EntityModelId = entity.EntityModelId;
                payHistory.Month = month;
                payHistory.Year = year;
                payHistory.CreatedBy = createdBy;
                payHistory.Status = errmessage; //"There is some problem while process the Payroll";

                //  payHistory.Save();
                flag = false;
            }
            return payHistory;
        }

        /// <summary>
        /// Income tax value update from Monthly input after that payroll process , update the processed tax value of TAXPAID,TPR,TAXPERMONTH,TDSDedProjectionMonth
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="attributeList"></param>
        /// <param name="TDSUpdateValue"></param>
        public void TaxValueUpdate(int companyId, Employee emp, int month, int year, AttributeModelList attrList, string TDSUpdateValue)
        {
            Guid employeeId = emp.Id;
            TXFinanceYear finyr = new TXFinanceYear(Guid.Empty, companyId, true);
            DateTime dtapplyDate = new DateTime(year, month, 1);
            TaxHistoryList txh = new TaxHistoryList(finyr.Id, employeeId, dtapplyDate);
            if (txh.Count > 0)
            {
                int ProjectedMonth = 0;
                DateTime sdate = dtapplyDate;
                do
                {
                    sdate = sdate.AddMonths(1);
                    if (sdate < finyr.EndingDate)
                    {
                        ProjectedMonth++;
                    }
                } while (sdate <= finyr.EndingDate);
                NetTaxActivity nta = new NetTaxActivity();
                TaxComputationInfo taxInfo = new PayrollBO.TaxComputationInfo();
                taxInfo.FinanceYear = finyr;
                taxInfo.EffectiveDate = new DateTime(year, month, 1);
                taxInfo.CompanyId = companyId;
                taxInfo.AttributemodelList = attrList;
                decimal temppaid = Convert.ToDecimal(Math.Round(Convert.ToDouble(Math.Round(nta.paidTax(taxInfo, emp), 2)) * 1.0) * 1);
                ProjectedMonth = ProjectedMonth + 1;

                var taxPaidval = attrList.Where(x => x.Name == "TAXPAID").FirstOrDefault();
                var tprObj = attrList.Where(x => x.Name == "TPR").FirstOrDefault();
                var totTaxObj = attrList.Where(x => x.Name == "TOTTAX").FirstOrDefault();
                var taxperMntObj = attrList.Where(x => x.Name == "TOTTAXPERMONTH").FirstOrDefault();

                if (taxPaidval != null)
                {
                    TaxHistory tottaxpaid = txh.Where(w => w.Field == "TAXPAID" && w.EmployeeId == employeeId && w.FieldId == taxPaidval.Id).FirstOrDefault();
                    if (tottaxpaid != null)
                    {
                        tottaxpaid.Total = temppaid + Convert.ToDecimal(TDSUpdateValue);
                        tottaxpaid.ApplyDate = dtapplyDate;
                        tottaxpaid.Save();
                    }
                    TaxHistory tpr = txh.Where(w => w.Field == "TPR" && w.EmployeeId == employeeId && w.FieldId == tprObj.Id).FirstOrDefault();
                    TaxHistory tottax2 = txh.Where(w => w.Field == "TOTTAX" && w.EmployeeId == employeeId && w.FieldId == totTaxObj.Id).FirstOrDefault();
                    TaxHistory taxperMonth = txh.Where(w => w.Field == "TOTTAXPERMONTH" && w.EmployeeId == employeeId && w.FieldId == taxperMntObj.Id).FirstOrDefault();
                    TaxHistory TDSDedProMonth = txh.Where(w => w.FieldId == Guid.Empty && w.EmployeeId == employeeId).FirstOrDefault();
                    decimal tempTotal = tottax2 != null ? Convert.ToDecimal(tottax2.Total) - Convert.ToDecimal(tottaxpaid.Total) : 0;
                    if (tpr != null)
                    {
                        tpr.Total = tempTotal;
                        tpr.ApplyDate = dtapplyDate;
                        tpr.Save();
                    }
                    if (taxperMonth != null)
                    {
                        taxperMonth.Total = tempTotal / ProjectedMonth;
                        taxperMonth.ApplyDate = dtapplyDate;
                        taxperMonth.Save();
                    }
                    if (TDSDedProMonth != null)
                    {
                        TDSDedProMonth.Total = tempTotal / ProjectedMonth;
                        TDSDedProMonth.ApplyDate = dtapplyDate;
                        TDSDedProMonth.Save();
                    }
                }
            }
        }
        #endregion

        #region private methods

        private void AddValuesTemp(EntityAttributeModel u, ref List<FormulaRecursive> lstFormulaRecursive, string AssignValue, string percentage, string eligiblityformula, int executeOrder, int roundingId, int type, bool doRoundoff = true, string basevalue = "0")
        {

            if (object.ReferenceEquals(lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper() && p.Id != null).FirstOrDefault(), null))
            {
                lstFormulaRecursive.Add(new FormulaRecursive()
                {
                    Assignedvalues = AssignValue,
                    Id = u.AttributeModelId.ToString(),
                    Name = u.AttributeModel.Name,
                    ExecuteOrder = executeOrder,
                    Rounding = roundingId,
                    type = type,
                    Percentage = percentage,
                    EligibleFormula = eligiblityformula,
                    ParentId = Convert.ToString(u.AttributeModel.ParentId),
                    DoRoundOff = doRoundoff,
                    BaseValue = basevalue,
                    Assignformulavalue = u.EntityAttributeValue.Value,
                });
            }
            else
            {
                var updateval = lstFormulaRecursive.Where(p => p.Id.ToString().ToUpper() == u.AttributeModelId.ToString().ToUpper()).FirstOrDefault();
                updateval.Rounding = roundingId;
                updateval.DoRoundOff = doRoundoff;
            }
        }

        private int GetMonthDay(EntityAttributeModel u, Category category, MonthlyInputList monthlyinputlist, int month, int year, ref List<PayrollError> payrollErrors, bool isPayrollProcess, out DateTime startdate)
        {
            startdate = DateTime.Parse("1/" + month + "/" + year, new CultureInfo("en-GB"));
            int md = DateTime.DaysInMonth(year, month);
            if (!string.IsNullOrEmpty(category.MonthDayProcess))
            {

                if (category.MonthDayProcess.ToUpper() == "MONTHDAY")
                {
                    md = DateTime.DaysInMonth(year, month);

                }
                else if (category.MonthDayProcess.ToUpper() == "STATICDAY")
                {
                    md = category.MonthDayOrStartDay;
                }
                else if (category.MonthDayProcess.ToUpper() == "STARTDAY")
                {
                    DateTime dt = new DateTime(year, month, category.MonthDayOrStartDay);
                    DateTime startDate = dt.AddMonths(-1);
                    DateTime endDate = dt.AddDays(-1);
                    TimeSpan ts = endDate - startDate;
                    md = ts.Days + 1;
                    startdate = startDate;
                }
                else if (category.MonthDayProcess.ToUpper() == "MONTHLYINPUT")
                {
                    var tmp = monthlyinputlist.Where(s => s.AttributeModelId == u.AttributeModel.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(tmp, null))
                    {
                        md = Convert.ToInt32(tmp.Value);
                    }
                    else
                    {
                        if (isPayrollProcess)
                        {
                            payrollErrors.Add(new PayrollError() { ErrorMessage = "Setting is not there for category ", Name = "Category in Saved" });
                        }
                    }
                }

            }
            else
            {
                if (isPayrollProcess)
                {
                    payrollErrors.Add(new PayrollError() { ErrorMessage = "Setting is not there for category ", Name = "Category in Saved" });
                }

            }

            return md;
        }

        /// <summary>
        /// Select the PayrollHistory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int CompanyId, Guid employeeId, int month, int year)
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistory_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Month", month);
            sqlCommand.Parameters.AddWithValue("@Year", year);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        /// <summary>
        /// Modified By:Sharmila
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CompanyId"></param>
        /// <param name="employeeId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid entityId)
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistorySelect_Details");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Select the PayrollHistory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int CompanyId, int smonth, int syear, int nmonth, int nyear, Guid employeeid)
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollHistorySelectPeriod");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@startMonth", smonth);
            sqlCommand.Parameters.AddWithValue("@startYear", syear);
            sqlCommand.Parameters.AddWithValue("@endMonth", nmonth);
            sqlCommand.Parameters.AddWithValue("@endYear", nyear);
            sqlCommand.Parameters.AddWithValue("@employeeId", employeeid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid id, int CompanyId, Guid employeeid)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_PayrollHistory");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@employeeId", employeeid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(int CompanyId)
        {
            SqlCommand sqlCommand = new SqlCommand("PayrollHistory_MonthYear_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool ImportPay()
        {
            string status;
            SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_PayrollHistory");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@xmlstring", this.Importxmlstring);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.SaveData(sqlCommand, out status);

        }


        #endregion

    }

    public class PayrollError
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }

    }
}

