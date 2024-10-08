using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class TaxHistoryReport
    {

        public TaxHistoryReport()
        {

        }
        public TaxHistoryReport(Guid finid, int companyId,DateTime applydate)
        {



        }
        public string EmployeeCode { get; set; }

        public string FieldType { get; set; }

        public Guid Fieldid { get; set; }


        public string Field { get; set; }


        public decimal? Actual { get; set; }


        public decimal? Projection { get; set; }


        public decimal? Total { get; set; }



        public string FirstOrder { get; set; }

        public string SecondOrder { get; set; }

        public string ThirdOrder { get; set; }

        public string actualDisplay { get; set; }

        public string projectionDisplay { get; set; }

        public string totallDisplay { get; set; }



        protected internal DataTable GetTableValues(Guid finid, int companyId, DateTime applydate)
        {
            SqlCommand sqlCommand = new SqlCommand("TaxHistoryXLReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYearId", finid);
          
            if (applydate > DateTime.MinValue)
                sqlCommand.Parameters.AddWithValue("@ApplyDate", applydate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);




        }














        }
}
