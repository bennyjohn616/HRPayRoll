using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;

namespace PayrollBO
{
    public class EmployeePastData
    {
       
        public EmployeePastData()
        {

        }


       
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the companyid
        /// </summary>
        public int CompanyId { get; set; }

        public string EmployeeCode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

      





    }
    public class EmployeePastDataList:List<EmployeePastData>
    {
                


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeePastDataList()
        {

        }

  
        public EmployeePastDataList(Guid employeeId, int  companyId)
        {
           
            DataTable dtValue = this.GetTableValues(employeeId, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeePastData emp = new EmployeePastData();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        emp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        emp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        emp.CompanyId = Convert.ToInt16(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"])))
                        emp.EmployeeCode =Convert.ToString(dtValue.Rows[rowcount]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FromDate"])))
                        emp.FromDate = Convert.ToDateTime(dtValue.Rows[rowcount]["FromDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ToDate"])))
                        emp.ToDate = Convert.ToDateTime(dtValue.Rows[rowcount]["ToDate"]);
                    this.Add(emp);
                }
            }
        }




    

   
        protected internal DataTable GetTableValues(Guid employeeId, int companyid)
        {

            string query = "select * from Emp_Past_Data where EmployeeId='" + employeeId + "' and CompanyId='" + companyid + "'";


            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
      

    }
   
}
