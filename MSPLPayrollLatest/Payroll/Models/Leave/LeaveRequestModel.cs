using PayrollBO;
using PayrollBO.Leave;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace Payroll.Models.Leave
{
    public class LeaveRequestModel
    {

        public void CompOffGainRequestSave(LeaveRequest compoffReq, string endDate,int validDays)
        {
            DateTime tempdate = compoffReq.FromDate;
            do
            {
                LeaveRequest compoffReqObj = new LeaveRequest();
                compoffReqObj.FinanceYear = compoffReq.FinanceYear;
                LeaveFinanceYear finyr = new LeaveFinanceYear(compoffReq.FinanceYear);
                compoffReqObj.Id = compoffReq.Id;
                compoffReqObj.EmployeeId = compoffReq.EmployeeId;
                compoffReqObj.FromDate = tempdate;
                if ((compoffReq.FromDate == compoffReq.EndDate || compoffReq.FromDate != compoffReq.EndDate) && compoffReq.FromDay == 0 && compoffReq.ToDay == 0)
                {
                    compoffReq.HFDay = 1;
                }
                else if ((compoffReq.FromDate == compoffReq.EndDate || compoffReq.FromDate != compoffReq.EndDate) && compoffReq.FromDay != 0 && compoffReq.ToDay != 0)
                {
                    compoffReq.HFDay = 0.5;
                }
                compoffReqObj.HFDay = compoffReq.HFDay;
                //this was commented in order to calculate end date based on no of days given in setting. Not based on date given in the setting.
                //compoffReqObj.EndDate = Convert.ToDateTime(endDate); // finyr.EndMonth;// tempdate.AddDays(90);// need to modify values get from settings           
                compoffReqObj.EndDate = tempdate.AddDays(validDays);
                compoffReqObj.CompOffGainhistorysave();
                tempdate = tempdate.AddDays(1);
            }
            while (tempdate <= compoffReq.EndDate);


        }


        public DataTable compoffparametercheck(LeaveRequest compoffReq, int companyId, Employee employee)
        {
            string returnVal = string.Empty;
            LeaveMasterList weekoffParameter = new LeaveMasterList(companyId, compoffReq.FinanceYear);
            LeaveRequest LeaverequestBO = new LeaveRequest();
            string compoffparameterid = LeaverequestBO.Parametersavailablecheck(weekoffParameter[0].Compoffparameter, employee);
            string[] LCvalues = compoffparameterid.Split(',');
            for (int i = 0; i < LCvalues.Length; i++)
            {
                LCvalues[i] = LCvalues[i].Trim();
            }
            if (LCvalues[1] != "")
            {
                compoffparameterid = LCvalues[1].ToString();
            }
            else
            {
                compoffparameterid = "00000000-0000-0000-0000-000000000000";
            }
            CompOffBO leaveYear = new CompOffBO();
            DataTable DTCompoffsettings = leaveYear.SelectCompoffsettings(companyId, new Guid(compoffparameterid), compoffReq.FinanceYear);         

            return DTCompoffsettings;
        }
        public bool CompOffGainRequestCheck(LeaveRequest compoffReq, int companyId, Employee employee, string type,LeaveRequestList floatingHoliday)
        {
            bool returnval = false;
            DataTable dtweekOff = weekOffDates(companyId, compoffReq.FinanceYear, employee);
            HolidaysList holidaylist = HolidayList(companyId, compoffReq.FinanceYear, employee);
            string expression;
            List<bool> iscompoff = new List<bool>();
            DateTime tempdate = compoffReq.FromDate;
           // string type = "both";
            do
            {
                DataRow[] foundRows;
                switch (type.ToLower())
                {
                    case "both":
                        expression = string.Format("Weekoffdate = #{0}#",
                         tempdate.ToString("MM/dd/yyyy hh:mm:ss tt"));
                        foundRows = dtweekOff.Select(expression);
                        if (foundRows.Count() > 0)
                            iscompoff.Add(false);
                        if (holidaylist.Where(x => x.Holidaydate.ToString("MM/dd/yyyy hh:mm:ss tt") == tempdate.ToString("MM/dd/yyyy hh:mm:ss tt")).ToList().Count() > 0)
                            iscompoff.Add(false);
                        if (floatingHoliday.Where(x => tempdate.Date >= x.FromDate.Date && tempdate.Date <= x.EndDate.Date).ToList().Count() > 0)
                            iscompoff.Add(false);
                        break;

                    case "weekoff"://week off only
                        expression = string.Format("Weekoffdate = #{0}#",
                         tempdate.ToString("MM/dd/yyyy hh:mm:ss tt"));
                        foundRows = dtweekOff.Select(expression);
                        if (foundRows.Count() > 0)
                            iscompoff.Add(false);
                        if (floatingHoliday.Where(x => tempdate.Date >= x.FromDate.Date && tempdate.Date <= x.EndDate.Date).ToList().Count() > 0)
                            iscompoff.Add(false);
                        break;
                    case "holiday":// holiday only
                        if (holidaylist.Where(x => x.Holidaydate.ToString("MM/dd/yyyy hh:mm:ss tt") == tempdate.ToString("MM/dd/yyyy hh:mm:ss tt")).ToList().Count() > 0)
                            iscompoff.Add(false);
                        if (floatingHoliday.Where(x => tempdate.Date >= x.FromDate.Date && tempdate.Date <= x.EndDate.Date).ToList().Count() > 0)
                            iscompoff.Add(false);
                        break;
                    default:
                        break;
                }
                tempdate = tempdate.AddDays(1);


            }
            while (tempdate <= compoffReq.EndDate);

            return returnval = iscompoff.Count > 0 ? true : false;
        }

        public DataTable weekOffDates(int companyId, Guid FinYrId, Employee employee)
        {
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, FinYrId);
            LeaveRequest LeaverequestBO = new LeaveRequest();
            string weekoffparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].Weekoffparameter, employee);                                //weekoffparameterid                       
            string[] WKvalues = weekoffparameterid.Split(',');
            for (int i = 0; i < WKvalues.Length; i++)
            {
                WKvalues[i] = WKvalues[i].Trim();
            }
            if (WKvalues[1] != "")
            {
                weekoffparameterid = WKvalues[1].ToString();
            }
            else
            {
                weekoffparameterid = "00000000-0000-0000-0000-000000000000";
            }



            LeaveSettingsBO WeekoffBO = new LeaveSettingsBO();
            //FOR GETTING WEEK OFF DAYS START

            WeekoffBO.CompanyId = companyId;
            WeekoffBO.FinyrId = FinYrId;
            WeekoffBO.DynamicComponentValue = new Guid(weekoffparameterid);
            DataTable weekoffdt = WeekoffBO.getWeekoffdataRequestingtime();

            return weekoffdt;
        }

        public HolidaysList HolidayList(int companyId, Guid FinYrId, Employee employee)
        {
            LeaveMasterList levmasterlist = new LeaveMasterList(companyId, FinYrId);
            LeaveRequest LeaverequestBO = new LeaveRequest();
            string Holidayparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].Holidayparameter, employee);                                //Holidayparameterid                       
            string[] Holidayvalues = Holidayparameterid.Split(',');
            for (int i = 0; i < Holidayvalues.Length; i++)
            {
                Holidayvalues[i] = Holidayvalues[i].Trim();
            }
            if (Holidayvalues[1] != "")
            {
                Holidayparameterid = Holidayvalues[1].ToString();
            }
            else
            {
                Holidayparameterid = "00000000-0000-0000-0000-000000000000";
            }
            HolidaysList HolidayList = new HolidaysList(Guid.Empty, FinYrId, new Guid(Holidayparameterid));
            return HolidayList;
        }

        public void compoffCreditSave(Guid FinYrId, Guid EmpId, Guid LeaveTypeId, double creditDays)
        {
            LeaveOpenings leaveOpening = new LeaveOpenings();
            leaveOpening.FinanceYearId = FinYrId;
            leaveOpening.EmployeeId = EmpId;
            leaveOpening.LeaveCredit = creditDays;
            leaveOpening.LeaveType = LeaveTypeId;
            leaveOpening.SaveCompOffCredit();
        }

        public void CompoffleaveMatchingsave(List<CompOffGainHistroy> CompOffEntry, Guid levReqId)
        {
            CompOffEntry.ForEach(x =>
            {
                CompOffBO compOff = new CompOffBO();
                compOff.Id = Guid.NewGuid();
                compOff.CompOffGainId = x.CompOffGainId;
                compOff.LeaveReqId = levReqId;
                compOff.AvaliableDays = x.AvaliableDays;
                compOff.CompoffleaveMatchingsave();

                LeaveRequest compOffGainHistoryUpdate = new LeaveRequest();
                compOffGainHistoryUpdate.Id = x.CompOffGainId;
                compOffGainHistoryUpdate.HFDay = Convert.ToDouble(x.AvaliableDays);
                compOffGainHistoryUpdate.CompOffGainUpdateAvaliableLeave();
            });
        }


        public bool InterveningValidation(ref List<DateTime?> FinalDateLst, ref string ErrMsg, List<DateTime?> WeekoffDates, List<DateTime?> Holidaydates, List<DateTime?> selectedDates, List<DateTime?> approveddate, string fromdate, string Enddate, string IsInterveningHoliday, string SubInterveningHoliday, LeaveType lev)
        {
            bool OutRslt = true;

            var BeforeHolidayWeekoffDates = new List<DateTime?>();               //Getting one Day before date of holiday list for intervening
            var AfterHolidayWeekoffDates = new List<DateTime?>();                //Getting one Day After date of holiday list for intervening
            var RemovedHolidaydatesList = new List<DateTime?>();                //Inorder to take  a list after removing holiday/adding holiday based on Intervening Concept
            var RemovedWeekOffList = new List<DateTime?>();                     //Inorder to take  a list after removing weekoff/adding weekoff based on Intervening Concept
            var FrmToDateLiesBeforeDate = new List<DateTime?>();                //Getting one Day before date of WeekOff list for intervening
            var FrmToDateLiesAfterDate = new List<DateTime?>();                 //Getting one Day After date of Weekoff list for intervening
            var FrmDateLiesBeforeDate = new List<DateTime?>();                  //Getting one Day before date of WeekOff/Holiday list for intervening with source as from date
            var FrmDateLiesAfterDate = new List<DateTime?>();                   //Getting one Day After date of Weekoff/Holiday list for intervening with source as from date
            var ToDateLiesBeforeDate = new List<DateTime?>();                  //Getting one Day before date of WeekOff/Holiday list for intervening with source as end date
            var ToDateLiesAfterDate = new List<DateTime?>();                  //Getting one Day After date of Weekoff/Holiday list for intervening with source as end date


            var Fdat = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
            var Tdat = Convert.ToDateTime(Enddate).ToString("dd/MM/yyyy");

            var FrmDate = Convert.ToDateTime(fromdate);
            var EndDate = Convert.ToDateTime(Enddate);
            var AddMonthFdat = FrmDate.AddMonths(1);
            var AddMonthTdat = EndDate.AddMonths(1);
            var SubMonthFdat = FrmDate.AddMonths(-1);
            var SubMonthTdat = EndDate.AddMonths(-1);

            bool InterveningValid = true;


            var TempDateList = new List<DateTime?>();
           // TempDateList = WeekoffDates.Where(n => n.Value.Month == FrmDate.Month || n.Value.Month == EndDate.Month || n.Value.Month == AddMonthFdat.Month || n.Value.Month == AddMonthTdat.Month || n.Value.Month == SubMonthFdat.Month || n.Value.Month == SubMonthTdat.Month).ToList();
            TempDateList = WeekoffDates.Where(n => n.Value >= FrmDate && n.Value <= EndDate).ToList();
            //filtering Weekoff list with refernce of applied dates.
            TempDateList.ForEach(p =>
            {
                DateTime MinusDateBefore = p.Value.AddDays(-1);
                BeforeHolidayWeekoffDates.Add(MinusDateBefore);
            });
            TempDateList.ForEach(k =>
            {
                DateTime PlusDateAfter = k.Value.AddDays(1);
                AfterHolidayWeekoffDates.Add(PlusDateAfter);
            });
            TempDateList.Clear();
            //TempDateList = Holidaydates.Where(r => r.Value.Month == FrmDate.Month || r.Value.Month == EndDate.Month || r.Value.Month == AddMonthFdat.Month || r.Value.Month == AddMonthTdat.Month || r.Value.Month == SubMonthFdat.Month || r.Value.Month == SubMonthTdat.Month).ToList();
            TempDateList = Holidaydates.Where(r => r.Value >= FrmDate && r.Value <= EndDate).ToList();
            TempDateList.ForEach(l =>
            {
                DateTime MinusDateBeforeHolid = l.Value.AddDays(-1);
                BeforeHolidayWeekoffDates.Add(MinusDateBeforeHolid);
            });
            TempDateList.ForEach(j =>
            {
                DateTime PlusDateAfterHolid = j.Value.AddDays(1);
                AfterHolidayWeekoffDates.Add(PlusDateAfterHolid);
            });
            bool BeforDateExist = false;
            bool AfterDateExist = false;
            bool BeforApprovedLeaveCheck = false;
            bool AfterApprovedLeaveCheck = false;

            selectedDates.ForEach(f =>
            {
                if (BeforDateExist == false)
                {
                    var tempCheck1 = BeforeHolidayWeekoffDates.Where(d => d.Value == f.Value).FirstOrDefault();
                    if (tempCheck1 != null || !object.ReferenceEquals(tempCheck1, null))
                    {
                        BeforDateExist = true;
                    }
                }
                if (AfterDateExist == false)
                {
                    var tempCheck2 = AfterHolidayWeekoffDates.Where(x => x.Value == f.Value).FirstOrDefault();
                    if (tempCheck2 != null || !object.ReferenceEquals(tempCheck2, null))
                    {
                        AfterDateExist = true;
                    }
                }
            });

            FrmDateLiesBeforeDate = BeforeHolidayWeekoffDates.Where(v => v.Value.ToString("dd/MM/yyyy") == FrmDate.ToString("dd/MM/yyyy")).ToList();
            FrmDateLiesAfterDate = AfterHolidayWeekoffDates.Where(g => g.Value.ToString("dd/MM/yyyy") == FrmDate.ToString("dd/MM/yyyy")).ToList();
            ToDateLiesBeforeDate = BeforeHolidayWeekoffDates.Where(v => v.Value.ToString("dd/MM/yyyy") == EndDate.ToString("dd/MM/yyyy")).ToList();
            ToDateLiesAfterDate = AfterHolidayWeekoffDates.Where(g => g.Value.ToString("dd/MM/yyyy") == EndDate.ToString("dd/MM/yyyy")).ToList();


            bool FlagtermBefore = true;
            bool FlagtermAfter = true;
            DateTime FrmMinusDate = Convert.ToDateTime(fromdate).AddDays(-1);
            DateTime FrmAddDate = Convert.ToDateTime(fromdate).AddDays(1);
            DateTime ToMinusDate = Convert.ToDateTime(Enddate).AddDays(-1);
            DateTime ToAddDate = Convert.ToDateTime(Enddate).AddDays(1);
            if (FrmDateLiesAfterDate.Count > 0)
            {
                while (FlagtermBefore)
                {
                    var tempchk1 = WeekoffDates.Where(o => o.Value.ToString("dd/MM/yyy") == FrmMinusDate.ToString("dd/MM/yyyy")).ToList();
                    var tempchk2 = Holidaydates.Where(t => t.Value.ToString("dd/MM/yyyy") == FrmMinusDate.ToString("dd/MM/yyyy")).ToList();
                    if (tempchk1.Count > 0 || tempchk2.Count > 0)
                    {
                        FrmMinusDate = FrmMinusDate.AddDays(-1);
                        FlagtermBefore = true;
                    }
                    else
                    {
                        var tempchk3 = approveddate.Where(y => y.Value.ToString("dd/MM/yyyy") == FrmMinusDate.ToString("dd/MM/yyyy")).ToList();
                        if (tempchk3.Count > 0)
                        {
                            BeforApprovedLeaveCheck = true;
                            FlagtermBefore = false;
                        }
                        else
                        {
                            BeforApprovedLeaveCheck = false;
                            FlagtermBefore = false;
                        }

                    }
                }
            }

            if (ToDateLiesAfterDate.Count > 0)
            {
                while (FlagtermBefore)
                {
                    var tempchk1 = WeekoffDates.Where(o => o.Value.ToString("dd/MM/yyy") == ToMinusDate.ToString("dd/MM/yyyy")).ToList();
                    var tempchk2 = Holidaydates.Where(t => t.Value.ToString("dd/MM/yyyy") == ToMinusDate.ToString("dd/MM/yyyy")).ToList();
                    if (tempchk1.Count > 0 || tempchk2.Count > 0)
                    {
                        ToMinusDate = ToMinusDate.AddDays(-1);
                        FlagtermBefore = true;
                    }
                    else
                    {
                        var tempchk3 = approveddate.Where(y => y.Value.ToString("dd/MM/yyyy") == ToMinusDate.ToString("dd/MM/yyyy")).ToList();
                        if (tempchk3.Count > 0)
                        {
                            BeforApprovedLeaveCheck = true;
                            FlagtermBefore = false;
                        }
                        else
                        {
                            BeforApprovedLeaveCheck = false;
                            FlagtermBefore = false;
                        }

                    }
                }
            }

            if (FrmDateLiesBeforeDate.Count > 0)
            {
                while (FlagtermAfter)
                {
                    var tempchk1 = WeekoffDates.Where(o => o.Value.ToString("dd/MM/yyy") == FrmAddDate.ToString("dd/MM/yyyy")).ToList();
                    var tempchk2 = Holidaydates.Where(t => t.Value.ToString("dd/MM/yyyy") == FrmAddDate.ToString("dd/MM/yyyy")).ToList();
                    if (tempchk1.Count > 0 || tempchk2.Count > 0)
                    {
                        FrmAddDate = FrmAddDate.AddDays(1);
                        FlagtermAfter = true;
                    }
                    else
                    {
                        var tempchk3 = approveddate.Where(y => y.Value.ToString("dd/MM/yyyy") == FrmAddDate.ToString("dd/MM/yyyy")).ToList();
                        if (tempchk3.Count > 0)
                        {
                            AfterApprovedLeaveCheck = true;
                            FlagtermAfter = false;
                        }
                        else
                        {
                            AfterApprovedLeaveCheck = false;
                            FlagtermAfter = false;
                        }

                    }
                }
            }

            if (ToDateLiesBeforeDate.Count > 0)
            {
                while (FlagtermAfter)
                {
                    var tempchk1 = WeekoffDates.Where(o => o.Value.ToString("dd/MM/yyy") == ToAddDate.ToString("dd/MM/yyyy")).ToList();
                    var tempchk2 = Holidaydates.Where(t => t.Value.ToString("dd/MM/yyyy") == ToAddDate.ToString("dd/MM/yyyy")).ToList();
                    if (tempchk1.Count > 0 || tempchk2.Count > 0)
                    {
                        ToAddDate = ToAddDate.AddDays(1);
                        FlagtermAfter = true;
                    }
                    else
                    {
                        var tempchk3 = approveddate.Where(y => y.Value.ToString("dd/MM/yyyy") == ToAddDate.ToString("dd/MM/yyyy")).ToList();
                        if (tempchk3.Count > 0)
                        {
                            AfterApprovedLeaveCheck = true;
                            FlagtermAfter = false;
                        }
                        else
                        {
                            AfterApprovedLeaveCheck = false;
                            FlagtermAfter = false;
                        }

                    }
                }
            }

            FrmToDateLiesBeforeDate = BeforeHolidayWeekoffDates.Where(v => v.Value.ToString("dd/MM/yyyy") == FrmDate.ToString("dd/MM/yyyy") && v.Value.ToString("dd/MM/yyyy") == EndDate.ToString("dd/MM/yyyy")).ToList();
            FrmToDateLiesAfterDate = AfterHolidayWeekoffDates.Where(g => g.Value.ToString("dd/MM/yyyy") == FrmDate.ToString("dd/MM/yyyy") && g.Value.ToString("dd/MM/yyyy") == EndDate.ToString("dd/MM/yyyy")).ToList();
            if (IsInterveningHoliday == "Y")
            {


                if (SubInterveningHoliday == "0")
                {

                    for (int i = 0; i <= Holidaydates.Count - 1; i++)
                    {
                        var checkdat = Convert.ToDateTime(Holidaydates[i].Value).ToString("dd/MM/yyyy");
                        if (checkdat == Fdat || checkdat == Tdat)
                        {
                            OutRslt = false;
                            ErrMsg = "You cannot apply leave for Holiday date";
                            InterveningValid = false;
                        }

                    }
                    if (OutRslt != false)
                    {
                        for (int l = 0; l <= WeekoffDates.Count - 1; l++)
                        {

                            DateTime weekofdate = Convert.ToDateTime(WeekoffDates[l].Value.ToString());
                            if (Convert.ToDateTime(fromdate) == weekofdate || Convert.ToDateTime(Enddate) == weekofdate)
                            {
                                OutRslt = false;
                                ErrMsg = "You cannot apply leave on weekoff day";
                                InterveningValid = false;
                            }
                        }
                    }
                    if (BeforApprovedLeaveCheck == true && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (AfterApprovedLeaveCheck == true && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave After Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (InterveningValid == true && OutRslt != false)
                    {

                        if (BeforDateExist == true && AfterDateExist == true)
                        {
                            selectedDates.ForEach(ij =>
                            {
                                DateTime tempDate1 = ij.Value;
                                RemovedWeekOffList.Add(tempDate1);
                            });
                        }
                        else
                        {
                            RemovedWeekOffList = FinalLeaveDateList(WeekoffDates, Holidaydates, selectedDates);
                        }
                    }
                }
                //Cannot Apply Leave for Start dates(Before weekoff/Holidays.)
                else if (SubInterveningHoliday == "SD")
                {

                    for (int i = 0; i <= Holidaydates.Count - 1; i++)
                    {
                        var checkdat = Convert.ToDateTime(Holidaydates[i].Value).ToString("dd/MM/yyyy");
                        if (checkdat == Fdat)
                        {
                            OutRslt = false;
                            ErrMsg = "You cannot apply leave for Holiday date(From date falls on Holiday)";
                            InterveningValid = false;
                        }

                    }
                    if (OutRslt != false)
                    {
                        for (int l = 0; l <= WeekoffDates.Count - 1; l++)
                        {

                            DateTime weekofdate = Convert.ToDateTime(WeekoffDates[l].Value.ToString());
                            if (Convert.ToDateTime(fromdate) == weekofdate)
                            {
                                OutRslt = false;
                                ErrMsg = "You cannot apply leave on weekoff day(From date falls on Weekoff day)";
                                InterveningValid = false;
                            }
                        }
                    }

                    if (FrmDateLiesBeforeDate.Count > 0 && ToDateLiesBeforeDate.Count > 0 && (FrmDate == EndDate) && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "From Date and To Date should not lies before Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if ((ToDateLiesBeforeDate.Count > 0) && BeforDateExist == true && AfterDateExist != true && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "To Date should not lies before Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if (BeforApprovedLeaveCheck == true && OutRslt != false)
                    {
                        //return BuildJsonResult(false, 200, "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days", null);
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (AfterApprovedLeaveCheck == true && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave After Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }

                    if (InterveningValid == true && OutRslt != false)
                    {

                        if ((BeforDateExist == true) || (BeforDateExist == true && AfterDateExist == true))
                        {
                            selectedDates.ForEach(ij =>
                            {
                                DateTime tempDate1 = ij.Value;
                                RemovedWeekOffList.Add(tempDate1);
                            });
                        }
                        else
                        {
                            RemovedWeekOffList = FinalLeaveDateList(WeekoffDates, Holidaydates, selectedDates);
                        }
                    }
                }
                else if (SubInterveningHoliday == "ED")
                {
                    //Cannot Apply Leave for End dates(After weekoff/Holidays.)
                    for (int i = 0; i <= Holidaydates.Count - 1; i++)
                    {
                        var checkdat = Convert.ToDateTime(Holidaydates[i].Value).ToString("dd/MM/yyyy");
                        if (checkdat == Tdat)
                        {
                            //return BuildJsonResult(false, 200, "You cannot apply leave for Holiday date(To date falls on Holiday)", dataValue);
                            OutRslt = false;
                            ErrMsg = "You cannot apply leave for Holiday date(To date falls on Holiday)";
                            InterveningValid = false;
                        }

                    }
                    if (OutRslt != false)
                    {
                        for (int l = 0; l <= WeekoffDates.Count - 1; l++)
                        {

                            DateTime weekofdate = Convert.ToDateTime(WeekoffDates[l].Value.ToString());
                            if (Convert.ToDateTime(Enddate) == weekofdate)
                            {
                                OutRslt = false;
                                ErrMsg = "You cannot apply leave on weekoff day(To date falls on Weekoff day)";
                                InterveningValid = false;
                            }
                        }
                    }

                    if (FrmDateLiesAfterDate.Count > 0 && ToDateLiesAfterDate.Count > 0 && (FrmDate == EndDate) && OutRslt != false)
                    {
                        //return BuildJsonResult(false, 200, "From Date and To Date should not lies after Holiday/Weekoff", null);
                        OutRslt = false;
                        ErrMsg = "From Date and To Date should not lies after Holiday/Weekoff";
                        InterveningValid = false;
                    }
                    if (FrmDateLiesAfterDate.Count > 0 && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "From Date should not lies after Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if (BeforApprovedLeaveCheck == true && OutRslt != false)
                    {
                        //return BuildJsonResult(false, 200, "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days", null);
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (AfterApprovedLeaveCheck == true && OutRslt != false)
                    {
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave After Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (InterveningValid == true && OutRslt != false)
                    {

                        if ((AfterDateExist == true) || (BeforDateExist == true && AfterDateExist == true))
                        {
                            selectedDates.ForEach(ij =>
                            {
                                DateTime tempDate1 = ij.Value;
                                RemovedWeekOffList.Add(tempDate1);
                            });
                        }
                        else
                        {
                            RemovedWeekOffList = FinalLeaveDateList(WeekoffDates, Holidaydates, selectedDates);
                        }
                    }
                }
                else
                {
                    //BOTH SETTINGS Cannot apply leave for Starting dates (Before weekoff/Holidays) and End Dates (After weekoff/Holidays) also.



                    if (FrmDateLiesBeforeDate.Count > 0 && ToDateLiesBeforeDate.Count > 0 && (FrmDate == EndDate))
                    {
                        OutRslt = false;
                        ErrMsg = "From Date and To Date should not lies before Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if (FrmDateLiesAfterDate.Count > 0 && ToDateLiesAfterDate.Count > 0 && (FrmDate == EndDate))
                    {
                        //return BuildJsonResult(false, 200, "From Date and To Date should not lies after Holiday/Weekoff", null);
                        OutRslt = false;
                        ErrMsg = "From Date and To Date should not lies after Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if ((ToDateLiesBeforeDate.Count > 0) && BeforDateExist == true && AfterDateExist != true)
                    {
                        //return BuildJsonResult(false, 200, "To Date should not lies before Holiday/Weekoff", null);
                        OutRslt = false;
                        ErrMsg = "To Date should not lies before Holiday/Weekoff";
                        InterveningValid = false;
                    }
                    if (FrmDateLiesAfterDate.Count > 0)
                    {
                        //return BuildJsonResult(false, 200, "From Date should not lies after Holiday/Weekoff", null);
                        OutRslt = false;
                        ErrMsg = "From Date should not lies after Holiday/Weekoff";
                        InterveningValid = false;
                    }

                    if (BeforApprovedLeaveCheck == true)
                    {
                        //return BuildJsonResult(false, 200, "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days", null);
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave Before Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }
                    if (AfterApprovedLeaveCheck == true)
                    {
                        //return BuildJsonResult(false, 200, "You had already Applied leave After Weekoff/Holiday,You cannot apply leave these days", null);
                        OutRslt = false;
                        ErrMsg = "You had already Applied leave After Weekoff/Holiday,You cannot apply leave these days";
                        InterveningValid = false;
                    }



                    if (InterveningValid == true)
                    {

                        if ((AfterDateExist == true) || (BeforDateExist == true) || (BeforDateExist == true && AfterDateExist == true))
                        {
                            selectedDates.ForEach(ij =>
                            {
                                DateTime tempDate1 = ij.Value;
                                RemovedWeekOffList.Add(tempDate1);
                            });
                        }
                        else
                        {
                            RemovedWeekOffList = FinalLeaveDateList(WeekoffDates, Holidaydates, selectedDates);
                        }
                    }

                }
            }
            else
            {
                LeaveTypeList leavetype = new LeaveTypeList(lev.CompanyId);
               
                var leave = leavetype.Where(l => l.Id == lev.Id).FirstOrDefault();
                if (leave.LeaveTypeName != "ONDUTY" && leave.LeaveTypeName != "WORK FROM HOME")
                {
                    for (int i = 0; i <= Holidaydates.Count - 1; i++)
                    {
                        var checkdat = Convert.ToDateTime(Holidaydates[i].Value).ToString("dd/MM/yyyy");
                        if (checkdat == Fdat || checkdat == Tdat)
                        {
                            //return BuildJsonResult(false, 200, "You cannot apply leave for Holiday date", dataValue);
                            OutRslt = false;
                            ErrMsg = "You cannot apply leave for Holiday date";
                        }

                    }
                    if (OutRslt != false)
                    {
                        for (int l = 0; l <= WeekoffDates.Count - 1; l++)
                        {

                            DateTime weekofdate = Convert.ToDateTime(WeekoffDates[l].Value.ToString());
                            if (Convert.ToDateTime(fromdate) == weekofdate || Convert.ToDateTime(Enddate) == weekofdate)
                            {
                                //return BuildJsonResult(false, 200, "You cannot apply leave on weekoff day", null);
                                OutRslt = false;
                                ErrMsg = "You cannot apply leave on weekoff day";
                            }
                        }
                    }
                }
                else 
                {

                    var dates = new List<DateTime?>();

                    dates.AddRange(Holidaydates);
                    dates.AddRange(WeekoffDates);


                    for (var dt = Convert.ToDateTime(fromdate); dt <= Convert.ToDateTime(Enddate); dt = dt.AddDays(1))
                    {


                        if (leave.LeaveTypeName == "ONDUTY")
                        {
                            if (!dates.Contains(dt))
                            {
                                ErrMsg = "You cannot apply leave on Week days for ONDUTY";
                                return OutRslt = false;
                            }
                        }
                        else if (leave.LeaveTypeName == "WORK FROM HOME")
                        {

                            if (dates.Contains(dt))
                            {
                                ErrMsg = "You cannot apply leave on ( Holiday OR Weekoff)  for WORK FROM HOME";
                                return OutRslt = false;
                            }
                        }

                    }

                  
                
                   
                }
               
            

            }




            if (IsInterveningHoliday == "N")
            {
                RemovedWeekOffList = FinalLeaveDateList(WeekoffDates, Holidaydates, selectedDates);
            }


            FinalDateLst = RemovedWeekOffList;


            return OutRslt;
        }
        public List<DateTime?> FinalLeaveDateList(List<DateTime?> WeekoffLst, List<DateTime?> HolidayLst, List<DateTime?> SelectedLst)
        {
            var FinalLst = new List<DateTime?>();
            var RemovedHolidaydatesList = new List<DateTime?>();
            //HOLIDAY CHECK START
            var ExtractingHolidays = SelectedLst.Except(HolidayLst).ToList();
            for (int HDcnt = 0; HDcnt <= ExtractingHolidays.Count - 1; HDcnt++)
            {
                RemovedHolidaydatesList.Add(Convert.ToDateTime(ExtractingHolidays[HDcnt]));
            }
            //HOLIDAY CHECK END
            //Checking for applying on the weekoff date START

            var ExtractingWeekoff = RemovedHolidaydatesList.Except(WeekoffLst).ToList();
            for (int WKFINALcnt = 0; WKFINALcnt <= ExtractingWeekoff.Count - 1; WKFINALcnt++)
            {
                FinalLst.Add(Convert.ToDateTime(ExtractingWeekoff[WKFINALcnt]));
            }
            //Checking for applying on the weekoff date END
            return FinalLst;
        }
    }

    public class CompOffGainHistroy
    {
        public Guid Id { get; set; }
        public Guid CompOffGainId { get; set; }
        public Guid LeaveReqId { get; set; }
        public decimal AvaliableDays { get; set; }
    }
}