using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class VerifyEmp
    {
        public VerifyEmp()
        {

        }
        public Guid Finyear { get; set; }
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int DBConnectionId { get; set; }

    }
}
