using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class VEmp
    {
        public VEmp()
        {

        }
        public int DBConnectionId { get; set; }
        public Guid VerifierID { get; set; }
        public Guid Finyear { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
