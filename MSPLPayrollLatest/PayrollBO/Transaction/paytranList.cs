using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PayrollBO
{
    public class PaytranList :  List<PayrollTransaction>
    {
            public string payrollHistoryId { get; set; }
            public string EmployeeId { get; set; }
            public string ColumnName { get; set; }
            public string Value { get; set; }


        public PaytranList(DataTable dt1)
        {
            DataTable dtValue = this.GetEmpMasterTransaction(dt1);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PayrollTransaction paytemp = new PayrollTransaction();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"])))
                        paytemp.PayrollHistoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["PayrollHistoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        paytemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    paytemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                    paytemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    paytemp.Value = Convert.ToString(dtValue.Rows[rowcount]["Value"]);
                    this.Add(paytemp);
                }
            }
        }

        protected internal  DataTable GetEmpMasterTransaction(DataTable dt1)
    {

        SqlCommand sqlCommand = new SqlCommand("PayrollTransaction_SelectAll");
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@HistoryData", dt1);
        DBOperation dbOperation = new DBOperation();
        string outValue = string.Empty;
        return dbOperation.GetTableData(sqlCommand);


    }

    }


}