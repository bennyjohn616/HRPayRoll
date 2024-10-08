using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO.Leave
{
   public class WeekoffCalculationList : List<LeaveSettingsBO>
    {
        public WeekoffCalculationList(string[] weekdaynames, DateTime FROMDATE, DateTime TODATE)
        {
            for (int i = 0; i < weekdaynames.Count(); i++)
            {



                switch (weekdaynames[i])
                {
                    case "Sunday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                LeaveSettingsBO SundayCalculation = new LeaveSettingsBO();
                                SundayCalculation.dates = date;
                                SundayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(SundayCalculation);
                            }

                        }
                        break;
                    case "Monday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Monday)
                            {
                                LeaveSettingsBO MondayCalculation = new LeaveSettingsBO();
                                MondayCalculation.dates = date;
                                MondayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(MondayCalculation);
                            }

                        }
                        break;
                    case "Tuesday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Tuesday)
                            {
                                LeaveSettingsBO TuesdayCalculation = new LeaveSettingsBO();
                                TuesdayCalculation.dates = date;
                                TuesdayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(TuesdayCalculation);
                            }

                        }
                        break;
                    case "Wednesday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Wednesday)
                            {
                                LeaveSettingsBO WednesdayCalculation = new LeaveSettingsBO();
                                WednesdayCalculation.dates = date;
                                WednesdayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(WednesdayCalculation);
                            }

                        }
                        break;
                    case "Thursday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Thursday)
                            {
                                LeaveSettingsBO ThursdayCalculation = new LeaveSettingsBO();
                                ThursdayCalculation.dates = date;
                                ThursdayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(ThursdayCalculation);
                            }

                        }
                        break;
                    case "Friday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Friday)
                            {
                                LeaveSettingsBO FridayCalculation = new LeaveSettingsBO();
                                FridayCalculation.dates = date;
                                FridayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(FridayCalculation);
                            }

                        }
                        break;
                    case "Saturday":
                        for (DateTime date = Convert.ToDateTime(FROMDATE); date <= Convert.ToDateTime(TODATE); date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == DayOfWeek.Saturday)
                            {
                                LeaveSettingsBO SaturdayCalculation = new LeaveSettingsBO();
                                SaturdayCalculation.dates = date;
                                SaturdayCalculation.datesname = date.DayOfWeek.ToString();
                                this.Add(SaturdayCalculation);
                            }

                        }
                        break;
                    default:

                        break;
                }
            }

        }
    }
}
