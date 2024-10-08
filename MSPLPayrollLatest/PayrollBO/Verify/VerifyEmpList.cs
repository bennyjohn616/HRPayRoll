using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class VerifyEmpList : List<VerifyEmp>
    {
        public VerifyEmpList()
        {

        }

        public VerifyEmpList(Guid finyear)
        {
            DataTable dtValue = GetTableValues(finyear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    VerifyEmp emptemp = new VerifyEmp();
                    emptemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    emptemp.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    emptemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    emptemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    emptemp.DBConnectionId = Convert.ToInt32(dtValue.Rows[rowcount]["DBConnectionId"]);
                    this.Add(emptemp);
                }
            }
        }

        protected internal DataTable GetTableValues(Guid finyear)
        {

            SqlCommand sqlCommand = new SqlCommand("VerifyEmp_Selectall");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@finyear", finyear);
            VerifyDBOpeartion vdboperation = new VerifyDBOpeartion();
            return vdboperation.GetTableData(sqlCommand);
        }



    }
}
