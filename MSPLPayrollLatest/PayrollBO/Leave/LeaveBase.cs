using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveBase: LeaveFinanceYear
    {
        //Initialize DefaultFinanceYear
        public LeaveFinanceYear CurentFinanceYear { get; set; }
        public LeaveBase()
        {
           this.CurentFinanceYear = new LeaveFinanceYear(Guid.Empty,true);           
        }
        
    }
}
