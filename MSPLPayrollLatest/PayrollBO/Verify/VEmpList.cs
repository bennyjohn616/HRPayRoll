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
    public class VEmpList : List<VEmp>
    {
        public VEmpList()
        {

        }

        public VEmpList(Guid VerifierID,Guid finyear,int DBConnection)
        {
            DataTable dtValue = GetTableValues(VerifierID,finyear,DBConnection);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    VEmp vemptemp = new VEmp();
                    vemptemp.DBConnectionId = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["DBConnectionId"]));
                    vemptemp.Finyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyear"]));
                    vemptemp.VerifierID = new Guid(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"]));
                    vemptemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmpId"]));
                    this.Add(vemptemp);
                }
            }
        }

        public VEmpList(Guid finyear, int DBConnection)
        {
            DataTable dtValue = GetTableValues(finyear, DBConnection);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    VEmp vemptemp = new VEmp();
                    vemptemp.DBConnectionId = Convert.ToInt32(Convert.ToString(dtValue.Rows[rowcount]["DBConnectionId"]));
                    vemptemp.Finyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyear"]));
                    vemptemp.VerifierID = new Guid(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"]));
                    vemptemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmpId"]));
                    this.Add(vemptemp);
                }
            }
        }


        protected internal DataTable GetTableValues(Guid VerifierID,Guid finyear,int DBConnection)
        {

            SqlCommand sqlCommand = new SqlCommand("VEmp_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@finyear", finyear);
            sqlCommand.Parameters.AddWithValue("@VerifierID", VerifierID);
            sqlCommand.Parameters.AddWithValue("@DBConnectionID", DBConnection);
            DBOperation dboperation = new DBOperation();
            return dboperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid finyear, int DBConnection)
        {

            SqlCommand sqlCommand = new SqlCommand("VEmp_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@finyear", finyear);
            sqlCommand.Parameters.AddWithValue("@DBConnectionID", DBConnection);
            DBOperation dboperation = new DBOperation();
            return dboperation.GetTableData(sqlCommand);
        }




    }
}
